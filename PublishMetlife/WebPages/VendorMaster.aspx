<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="VendorMaster.aspx.cs" Inherits="VendorMaster" Title="BCIL : ATS - Vendor Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= txtVendorCode.ClientID%>').value = "";
            document.getElementById('<%= txtVendorName.ClientID%>').value = "";
            document.getElementById('<%= txtAddress.ClientID%>').value = "";
            document.getElementById('<%= txtCountry.ClientID%>').value = "";
            document.getElementById('<%= txtState.ClientID%>').value = "";
            document.getElementById('<%= txtCity.ClientID%>').value = "";
            document.getElementById('<%= txtPIN.ClientID%>').value = "";
            document.getElementById('<%= txtContPerson.ClientID%>').value = "";
            document.getElementById('<%= txtEmailID.ClientID%>').value = "";
            document.getElementById('<%= txtPhone.ClientID%>').value = "";
            document.getElementById('<%= txtRemarks.ClientID %>').value = "";
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = '';
            document.getElementById('<%= txtVendorName.ClientID%>').focus();
            window.location.href = window.location;
        }
        function ShowErrMsg(Message) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to Perform this operation!!');
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Vendor Master
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
                                        <asp:Label ID="Label5" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Vendor ID" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtVendorCode" CssClass="textbox" runat="server"
                                            Width="200px" TabIndex="1" ToolTip="Enter vendor ID" MaxLength="15"></asp:TextBox>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="dynamic"
                                            ControlToValidate="txtVendorCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[a-zA-Z ]+$" ErrorMessage="[Alphabet]" >
                                        </asp:RegularExpressionValidator>--%>

                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label14" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Vendor Name" CssClass="label"></asp:Label>

                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtVendorName" CssClass="textbox" runat="server"
                                            Width="200px" TabIndex="2" ToolTip="Enter vendor name" MaxLength="50"></asp:TextBox>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="dynamic"
                                            ControlToValidate="txtVendorName" ValidationGroup="Submit" CssClass="validation"> 
                                            ValidationExpression="^[a-zA-Z ]+$" ErrorMessage="[Alphabet]"
                                        </asp:RegularExpressionValidator>--%>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label16" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label9" runat="server" CssClass="label" Text="Contact Name"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtContPerson" runat="server" CssClass="textbox"
                                            TabIndex="6" Width="200px" MaxLength="50" ToolTip="Enter contact person's name">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtContPerson"
                                            CssClass="validation" Display="dynamic" ErrorMessage="[Alphanumeric]" ValidationExpression="^[0-9a-zA-Z ]+$"
                                            ValidationGroup="Submit">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label11" runat="server" CssClass="label" Text="E-Mail ID"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtEmailID" runat="server" CssClass="textbox"
                                            TabIndex="8" ToolTip="Enter vendor's e-mail id" Width="200px" MaxLength="50">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REP_Email" runat="server" ControlToValidate="txtEmailID"
                                            CssClass="validation" ValidationGroup="Submit" ErrorMessage="[Invalid E-Mail]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label10" runat="server" CssClass="label" Text="Phone No."></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtPhone" runat="server" CssClass="textbox" TabIndex="9"
                                            MaxLength="10" ToolTip="Enter vendor's mobile no." Width="200px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="rfvPhoneNo" runat="server" ControlToValidate="txtPhone"
                                            ErrorMessage="[Numeric Only]" ValidationExpression="^([123456789]{1})([0-9]{1})([0-9]{8})$"
                                            ValidationGroup="Submit" CssClass="validation">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label4" runat="server" Text="Address" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:TextBox autocomplete="off" ID="txtAddress" MaxLength="50" CssClass="multitextbox"
                                            runat="server" TextMode="MultiLine" ToolTip="Enter vendor's address" Height="50px"
                                            Width="200px" TabIndex="3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <%--<asp:Label ID="Label15" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>--%>
                                        <asp:Label ID="Label2" runat="server" CssClass="label" Text="City"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtCity" runat="server" CssClass="textbox" MaxLength="20"
                                            TabIndex="7" Width="200px" ToolTip="Enter vendor's city name"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" Display="dynamic"
                                            ControlToValidate="txtCity" ValidationGroup="Submit" CssClass="validation" ValidationExpression="^[0-9a-zA-Z ]+$"
                                            ErrorMessage="[Alphanumeric]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label7" runat="server" CssClass="label" Text="State"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtState" runat="server" CssClass="textbox" MaxLength="20"
                                            TabIndex="5" Width="200px" ToolTip="Enter vendor's state name"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" Display="dynamic"
                                            ControlToValidate="txtState" ValidationGroup="Submit" CssClass="validation" ValidationExpression="^[0-9a-zA-Z ]+$"
                                            ErrorMessage="[Alphanumeric]">
                                        </asp:RegularExpressionValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="text-align: right;" valign="top">
                                        <asp:Label ID="Label6" runat="server" CssClass="label" Text="Country"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:TextBox autocomplete="off" ID="txtCountry" MaxLength="20" runat="server" CssClass="textbox"
                                            TabIndex="4" Width="200px" ToolTip="Enter vendor's country name"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" Display="dynamic"
                                            ControlToValidate="txtCountry" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[a-zA-Z ]+$" ErrorMessage="[Alphabet only]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" runat="server" CssClass="label" Text="PIN Code"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtPIN" runat="server" CssClass="textbox" MaxLength="6"
                                            TabIndex="9" ToolTip="Enter pin code of the city" Width="200px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPIN"
                                            CssClass="validation" ErrorMessage="[Only Numbers]" ValidationExpression="^\d+$"
                                            ValidationGroup="Submit"></asp:RegularExpressionValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top" rowspan="2">
                                        <asp:Label ID="Label13" runat="server" CssClass="label" Text="Remarks"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top" rowspan="2">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left" rowspan="2">
                                        <asp:TextBox autocomplete="off" ID="txtRemarks" runat="server" CssClass="multitextbox"
                                            Height="50px" ToolTip="Enter remarks" MaxLength="100" TabIndex="11" TextMode="MultiLine"
                                            Width="200px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label12" runat="server" CssClass="label" Text="Set Status"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:CheckBox ID="chkSetStatus" runat="server" Checked="true" CssClass="label" TabIndex="12"
                                            Text=" Active" />
                                    </td>

                                </tr>
                                <tr class="all-asset-display-none Facilities-display">
                                    <td style="text-align: right; vertical-align: top" rowspan="2">
                                        <asp:Label ID="Label17" runat="server" CssClass="label" Text="Work Category"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top" rowspan="2">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left" rowspan="2">
                                        <asp:DropDownList ID="ddlWorkCategory" runat="server" CssClass="dropdownlist" Width="200">
                                            <asp:ListItem Value="SELECT" Text="-- Select Category --"></asp:ListItem>
                                            <asp:ListItem Value="BAU" Text="BAU"></asp:ListItem>
                                            <asp:ListItem Value="Project" Text="Project"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:RequiredFieldValidator ID="RFV_VendCode" runat="server" ControlToValidate="txtVendorCode"
                                            CssClass="validation" ErrorMessage="[Vendor Code Required]" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_VendName" runat="server" ControlToValidate="txtVendorName"
                                            ErrorMessage="[Vendor Name Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContPerson"
                                            ErrorMessage="[Vendor Contact Person Name Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <%--  <asp:RequiredFieldValidator ID="RFV_City" runat="server" ControlToValidate="txtCity"
                                            ErrorMessage="[Vendor City Name Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="13"
                                            Text="Save Vendor" CssClass="button" ToolTip="Save vendor details" Width="100px"
                                            OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" TabIndex="14" OnClientClick="ClearFields();"
                                            Text="Reset" CssClass="button" Width="60px" ToolTip="Reset/clear fields"
                                            CausesValidation="False" />
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
                <td colspan="4" align="center">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvVendMaster" runat="server" AllowPaging="true" OnRowDeleting="gvVendMaster_RowDeleting"
                                    OnRowEditing="gvVendMaster_RowEditing" OnRowUpdating="gvVendMaster_RowUpdating"
                                    OnRowCancelingEdit="gvVendMaster_RowCancelingEdit" AutoGenerateColumns="False"
                                    OnRowDataBound="gvVendMaster_RowDataBound" ShowFooter="false" CssClass="mGrid"
                                    PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvVendMaster_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Vendor ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEVendCode" runat="server" Text='<%#Eval("VENDOR_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%-- <EditItemTemplate>
                                                <asp:Label ID="lblEVendCode" runat="server" Text='<%#Eval("VENDOR_CODE") %>'></asp:Label>
                                            </EditItemTemplate>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <%--<asp:Label ID="lblEVendName" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:Label>--%>
                                                <asp:TextBox autocomplete="off" ID="txtEVendName" MaxLength="50" ValidationGroup="Grid"
                                                    Width="100px" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEVendName" runat="server" ControlToValidate="txtEVendName"
                                                    ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdd" runat="server" Text='<%#Eval("VENDOR_ADDRESS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEAdd" ValidationGroup="Grid" TextMode="MultiLine"
                                                    CssClass="multitextbox" runat="server" MaxLength="100" Width="100px" Text='<%#Eval("VENDOR_ADDRESS") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Country">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("VENDOR_COUNTRY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtECountry" MaxLength="20" runat="server" Width="100px"
                                                    Text='<%#Eval("VENDOR_COUNTRY") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="State">
                                            <ItemTemplate>
                                                <asp:Label ID="lblState" runat="server" Text='<%#Eval("VENDOR_STATE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEState" MaxLength="20" runat="server" Width="100px"
                                                    Text='<%#Eval("VENDOR_STATE") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="City">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server" Text='<%#Eval("VENDOR_CITY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtECity" MaxLength="20" runat="server" Width="100px"
                                                    Text='<%#Eval("VENDOR_CITY") %>'></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="rfvECity" runat="server" ControlToValidate="txtECity"
                                                    ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid" />--%>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PIN">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPIN" runat="server" Text='<%#Eval("VENDOR_PIN") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEPIN" ValidationGroup="Grid" Width="50px"
                                                    MaxLength="6" runat="server" Text='<%#Eval("VENDOR_PIN") %>'></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator11" CssClass="validation"
                                                    runat="server" ControlToValidate="txtEPIN" ErrorMessage="Only Numbers" ValidationExpression="^\d+$"
                                                    ValidationGroup="Grid"></asp:RegularExpressionValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cont. Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContPerson" runat="server" Text='<%#Eval("VENDOR_CONT_PERSON") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEContPerson" ValidationGroup="Grid" Width="100px"
                                                    runat="server" Text='<%#Eval("VENDOR_CONT_PERSON") %>' MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEPerson" runat="server" ControlToValidate="txtEContPerson"
                                                    ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Phone">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhone" runat="server" Text='<%#Eval("VENDOR_PHONE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEPhone" ValidationGroup="Grid" Width="100px"
                                                    MaxLength="10" runat="server" Text='<%#Eval("VENDOR_PHONE") %>'></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" CssClass="validation"
                                                    runat="server" ControlToValidate="txtEPhone" ErrorMessage="Only Numbers" ValidationExpression="^\d+$"
                                                    ValidationGroup="Grid"></asp:RegularExpressionValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="E-Mail">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("VENDOR_EMAIL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEEMail" MaxLength="50" ValidationGroup="Grid"
                                                    Width="100px" runat="server" Text='<%#Eval("VENDOR_EMAIL") %>'></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revEEmail" runat="server" ControlToValidate="txtEEMail" 
                                                    CssClass="validation" ErrorMessage="[Invalid E-Mail]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$">
                                                </asp:RegularExpressionValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Active">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("ACTIVE") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%#Eval("ACTIVE") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit/Delete">
                                            <EditItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                                    ImageUrl="~/images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CommandName="Cancel" CausesValidation="false"
                                                    ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" CausesValidation="false"
                                                    ImageUrl="~/images/Edit_16x16.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" OnClientClick="return confirm('Are you sure to delete?');"
                                                    CommandName="Delete" ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvVendMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvVendMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvVendMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvVendMaster" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvVendMaster" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export vendor master data into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
