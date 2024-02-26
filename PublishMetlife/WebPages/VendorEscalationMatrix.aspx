<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="VendorEscalationMatrix.aspx.cs" Inherits="VendorEscalationMatrix" Title="BCIL : ATS - Vendor Escalation Matrix" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function noBack() {
            window.history.forward(1);
        }
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= ddlVendor.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlSupportType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlLevel.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtVendorEmail.ClientID%>').value = "";
            document.getElementById('<%= txtVendorName.ClientID%>').value = "";
            document.getElementById('<%= txtMobileNo.ClientID%>').value = "";
            document.getElementById('<%= txtAddress.ClientID%>').value = "";
            document.getElementById('<%= txtRemarks.ClientID%>').value = "";
            document.getElementById('<%= ddlVendor.ClientID%>').focus();
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to execute this operation!');
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 140px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Vendor Escalation Matrix
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label19" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label10" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Vendor Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlVendor" runat="server" CssClass="dropdownlist" ToolTip="Select vendor name"
                                            Width="200px" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label11" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Vendor Support Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlSupportType" runat="server" ToolTip="Select vendor support type"
                                            CssClass="dropdownlist" Width="200px">
                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="H/W Support" Value="HW_SUPPORT"></asp:ListItem>
                                            <asp:ListItem Text="S/W Support" Value="SW_SUPPORT"></asp:ListItem>
                                            <asp:ListItem Text="H/W & S/W Both" Value="BOTH"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label12" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Text="Escalation Level" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlLevel" runat="server" ToolTip="Select escalation level"
                                            CssClass="dropdownlist" Width="200px">
                                            <asp:ListItem Text="-- Select Level --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="Level 1" Value="LEVEL1"></asp:ListItem>
                                            <asp:ListItem Text="Level 2" Value="LEVEL2"></asp:ListItem>
                                            <asp:ListItem Text="Level 3" Value="LEVEL3"></asp:ListItem>
                                            <asp:ListItem Text="Level 4" Value="LEVEL4"></asp:ListItem>
                                            <asp:ListItem Text="Level 5" Value="LEVEL5"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" runat="server" Text="Vendor E-Mail" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtVendorEmail" ToolTip="Enter vendor e-mail address"
                                            runat="server" CssClass="textbox" Width="200px" MaxLength="50"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REP_Email" runat="server" CssClass="validation"
                                            ControlToValidate="txtVendorEmail" ErrorMessage="[ Invalid E-Mail ]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="Person Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtVendorName" ToolTip="Enter vendor name" runat="server"
                                            CssClass="textbox" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="Mobile No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtMobileNo" MaxLength="10" runat="server" CssClass="textbox"
                                            Width="200px" ToolTip="Enter vendor's mobile no."></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="validation"
                                            ControlToValidate="txtMobileNo" ErrorMessage="[Invalid Mobile No.]" ValidationExpression="^([123456789]{1})([0-9]{1})([0-9]{8})$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label7" runat="server" Text="Address" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:TextBox autocomplete="off" ToolTip="Enter vendor's address" ID="txtAddress"
                                            MaxLength="100" runat="server" CssClass="multitextbox" TextMode="MultiLine" Width="200px"
                                            Height="50px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label8" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:TextBox autocomplete="off" ID="txtRemarks" runat="server" CssClass="multitextbox"
                                            TextMode="MultiLine" Width="200px" ToolTip="Enter remarks" Height="50px" MaxLength="500"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style1">
                                        <asp:Label ID="Label9" runat="server" Text="Set Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:CheckBox ID="chkSetStatus" ToolTip="Set vendor's status" runat="server" Text="  Active"
                                            Checked="true" CssClass="label" TabIndex="7" />
                                    </td>
                                    <td style="text-align: right">
                                    </td>
                                    <td style="text-align: center">
                                    </td>
                                    <td style="text-align: left">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:RequiredFieldValidator ID="RFV_EmpCode" runat="server" ControlToValidate="ddlVendor"
                                            ErrorMessage="[Vendor Name Required]" CssClass="validation" ValidationGroup="Submit"
                                            InitialValue="-- Select Vendor --"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_Support" runat="server" ControlToValidate="ddlSupportType"
                                            ErrorMessage="[Support Type Required]" CssClass="validation" ValidationGroup="Submit"
                                            InitialValue="SELECT"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlLevel"
                                            ErrorMessage="[Escalation Level Required]" CssClass="validation" ValidationGroup="Submit"
                                            InitialValue="SELECT"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                            ImageUrl="~/images/Submit.png" ToolTip="Save vendor escalation matrix details"
                                            Height="35px" Width="85px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                            ImageUrl="~/images/Reset.png" Height="35px" Width="85px" ToolTip="Refresh/reset fields"
                                            CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvVenEscMatrix" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                    OnRowEditing="gvVenEscMatrix_RowEditing" OnRowCancelingEdit="gvVenEscMatrix_RowCancelingEdit"
                                    OnRowDeleting="gvVenEscMatrix_RowDeleting" OnRowDataBound="gvVenEscMatrix_RowDataBound"
                                    OnRowUpdating="gvVenEscMatrix_RowUpdating" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    PageSize="50" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvVenEscMatrix_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="VEM Code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVEMCode" runat="server" Text='<%#Eval("VEM_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEVEMCode" runat="server" Text='<%#Eval("VEM_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendName" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEVendor" runat="server" CssClass="dropdownlist">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Support Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSuppType" runat="server" Text='<%#Eval("VEM_SUPPORT_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlESuppType" runat="server" CssClass="dropdownlist">
                                                    <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                                    <asp:ListItem Text="H/W Support" Value="HW_SUPPORT"></asp:ListItem>
                                                    <asp:ListItem Text="S/W Support" Value="SW_SUPPORT"></asp:ListItem>
                                                    <asp:ListItem Text="H/W & S/W Both" Value="BOTH"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFV_ESupport" runat="server" ControlToValidate="ddlESuppType"
                                                    ErrorMessage="*" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Level">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLevel" runat="server" Text='<%#Eval("VEM_LEVEL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlELevel" runat="server" CssClass="dropdownlist">
                                                    <asp:ListItem Text="-- Select Level --" Value="SELECT"></asp:ListItem>
                                                    <asp:ListItem Text="Level 1" Value="LEVEL1"></asp:ListItem>
                                                    <asp:ListItem Text="Level 2" Value="LEVEL2"></asp:ListItem>
                                                    <asp:ListItem Text="Level 3" Value="LEVEL3"></asp:ListItem>
                                                    <asp:ListItem Text="Level 4" Value="LEVEL4"></asp:ListItem>
                                                    <asp:ListItem Text="Level 5" Value="LEVEL5"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="ddlELevel"
                                                    ErrorMessage="*" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Person Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPersName" runat="server" Text='<%#Eval("VEM_PERSON_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" MaxLength="50" ID="txtEPersName" runat="server" Text='<%#Eval("VEM_PERSON_NAME") %>'
                                                    CssClass="textbox"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="E-Mail">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("VEM_EMAIL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEEmail" runat="server" Text='<%#Eval("VEM_EMAIL") %>'
                                                    CssClass="textbox" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEEmail"
                                                    ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="REP_Email" runat="server" CssClass="validation"
                                                    ControlToValidate="txtEEmail" ErrorMessage="[Invalid E-Mail]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$">
                                                </asp:RegularExpressionValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMobile" runat="server" Text='<%#Eval("VEM_MOBILE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" MaxLength="10" ID="txtEMobile" runat="server" Text='<%#Eval("VEM_MOBILE") %>'
                                                    CssClass="textbox"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("VEM_ADDRESS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEAddress" MaxLength="100" TextMode="MultiLine"
                                                    runat="server" Text='<%#Eval("VEM_ADDRESS") %>' CssClass="textbox"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Active">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("VEM_ACTIVE") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%#Eval("VEM_ACTIVE") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" ImageUrl="~/images/Edit_16x16.png"
                                                    runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" OnClientClick="return confirm('Are you sure to delete?');"
                                                    CommandName="Delete" ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                                    ImageUrl="~/images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CommandName="Cancel" CausesValidation="false"
                                                    ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvVenEscMatrix" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvVenEscMatrix" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvVenEscMatrix" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvVenEscMatrix" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvVenEscMatrix" EventName="RowDataBound" />
                                <asp:AsyncPostBackTrigger ControlID="gvVenEscMatrix" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
