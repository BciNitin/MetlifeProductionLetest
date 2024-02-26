<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="UserMaster.aspx.cs" Inherits="_Default" Title="BCIL : ATS - User Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= ddlGroup.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= txtLocationCode.ClientID %>').value = "";
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtUserName.ClientID %>').value = "";
            document.getElementById('<%= txtUserID.ClientID %>').value = "";
            document.getElementById('<%= txtPassword.ClientID %>').value = "";
            document.getElementById('<%= txtConfPswd.ClientID %>').value = "";
            document.getElementById('<%= txtEmailAddress.ClientID %>').value = "";
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
            document.getElementById('<%= txtTechOpsEMailId.ClientID %>').value = "";
            document.getElementById('<%= ddlGroup.ClientID %>').focus();
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowAlert(msg) {
            alert(msg.toString());
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 26px;
        }
        .style2
        {
            width: 225px;
        }
        .style3
        {
            width: 250px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            User Master</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" style="width: 100%;" align="center">
           <%-- <tr>
                <td colspan="4">
                    <asp:Label ID="Label19" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>--%>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label11" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label7" runat="server" Text="Select Group" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlGroup" CssClass="dropdownlist" ToolTip="Select Group Name"
                                            runat="server" Width="200px" ValidationGroup="Submit" TabIndex="1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_Group" runat="server" ControlToValidate="ddlGroup"
                                            InitialValue="-- Select Group --" ErrorMessage="[Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td rowspan="9" valign="top" align="center">
                                        <img src="../images/green_user.png" style="width: 200px; height: 200px" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="Select Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlLocation" ToolTip="Select Location Name" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                            AutoPostBack="true" CssClass="dropdownlist" runat="server" Width="200px" TabIndex="3">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="btnRefreshLocation" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                            OnClick="btnRefreshLocation_Click" ToolTip="Refresh/reset location" />
                                        <asp:Label ID="lblLocCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label12" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label10" runat="server" CssClass="label" Text="Location"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td>
                                        <asp:TextBox autocomplete="off" ID="txtLocationCode" runat="server" ToolTip="Location Code"
                                            CssClass="readonlytext" ValidationGroup="submit" Width="200px" TabIndex="-1"
                                            ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLocationCode"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label13" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="User Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtUserName" CssClass="textbox" ToolTip="Enter User Name"
                                            ValidationGroup="Submit" runat="server" Width="200px" MaxLength="50" TabIndex="4"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_txtUsername" runat="server" ControlToValidate="txtUsername"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                                            ControlToValidate="txtUserName" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[Alphanumeric]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label14" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="User ID" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtUserID" CssClass="textbox" runat="server"
                                            ToolTip="Enter User ID" Width="200px" ValidationGroup="Submit" TabIndex="5" MaxLength="20"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_UserID" runat="server" ControlToValidate="txtUserID"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="dynamic"
                                            ControlToValidate="txtUserID" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[Alphanumeric]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label15" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Text="Password" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtPassword" CssClass="textbox" runat="server"
                                            ToolTip="Enter Password" Width="200px" TabIndex="6" ValidationGroup="submit"
                                            TextMode="Password" MaxLength="20" oncopy="return false;" onpaste="return false;"
                                            oncut="return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Password" runat="server" ControlToValidate="txtPassword"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label16" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Text="Confirm Password" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtConfPswd" CssClass="textbox" ToolTip="Enter Confirm Password"
                                            MaxLength="20" runat="server" Width="200px" TabIndex="7" ValidationGroup="Submit"
                                            TextMode="Password" oncopy="return false;" onpaste="return false;" oncut="return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_CnfPswd" runat="server" ControlToValidate="txtConfPswd"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        &nbsp;<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword"
                                            CssClass="validation" ControlToValidate="txtConfPswd" ErrorMessage="[Doesn't Match]"
                                            ValidationGroup="Submit"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label17" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label8" runat="server" Text="E-Mail Address" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtEmailAddress" CssClass="textbox" ToolTip="Enter E-mail Address"
                                            MaxLength="50" runat="server" Width="200px" TabIndex="8" ValidationGroup="Submit"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Email" runat="server" ControlToValidate="txtEmailAddress"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REP_Email" runat="server" ControlToValidate="txtEmailAddress"
                                            ErrorMessage="[Invalid E-Mail]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                            Font-Size="12px" ValidationGroup="Submit" CssClass="validation">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label18" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label9" runat="server" Text="Manager E-Mail ID" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtTechOpsEMailId" CssClass="textbox" ToolTip="Enter Techops E-mail Address"
                                            MaxLength="50" runat="server" Width="200px" TabIndex="8" ValidationGroup="Submit"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTechOpsEMailId"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtTechOpsEMailId"
                                            ErrorMessage="[Invalid E-Mail]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                            Font-Size="12px" ValidationGroup="Submit" CssClass="validation">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="Set Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td colspan="2" style="text-align: left">
                                        <asp:CheckBox ID="chkSetStatus" Checked="true" ToolTip="Set Status" runat="server"
                                            Text="  Active" CssClass="label" TabIndex="9" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="style3">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style2">
                                    </td>
                                    <td align="left" class="style1">
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" ToolTip="Save user details"
                                            Text="Save User" CssClass="button" TabIndex="10" Width="80px" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" ToolTip="Refresh/reset fields" runat="server" OnClientClick="ClearFields();"
                                            OnClick="btnClear_Click" Text="Reset" CssClass="button" TabIndex="11" 
                                            Width="60px" CausesValidation="False" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
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
                                <asp:GridView ID="gvUserMaster" runat="server" AllowPaging="True" OnRowDeleting="gvUserMaster_RowDeleting"
                                    OnRowEditing="gvUserMaster_RowEditing" OnRowUpdating="gvUserMaster_RowUpdating"
                                    PageSize="50" OnRowCancelingEdit="gvUserMaster_RowCancelingEdit" AutoGenerateColumns="False"
                                    OnRowDataBound="gvUserMaster_RowDataBound" ShowFooter="false" CssClass="mGrid"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvUserMaster_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="User ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("USER_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEditUserId" Width="60px" runat="server" Text='<%#Eval("USER_ID") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("USER_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEditUserName" MaxLength="50" Width="80px"
                                                    runat="server" Text='<%#Eval("USER_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvusername" runat="server" ControlToValidate="txtEditUserName"
                                                    ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="E-Mail">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("USER_EMAIL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEEmail" runat="server" CssClass="textbox"
                                                    Width="150px" MaxLength="50" Text='<%#Eval("USER_EMAIL") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFV_Email" runat="server" ControlToValidate="txtEEmail"
                                                    ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid" />
                                                <asp:RegularExpressionValidator ID="REP_EEmail" runat="server" ControlToValidate="txtEEmail"
                                                    ErrorMessage="[Invalid E-Mail]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                                    ValidationGroup="Grid">
                                                </asp:RegularExpressionValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField HeaderText="Company" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompany" runat="server" Text='<%#Eval("COMP_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECompany" runat="server" Text='<%#Eval("COMP_NAME") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoc" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEditLocation" CssClass="dropdownlist" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlEditLocation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblELoclevel" runat="server" Text="1" Visible="False"></asp:Label>
                                                <asp:Label ID="lblELocCode" runat="server" Text="0" Visible="False"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroup" runat="server" Text='<%#Eval("GROUP_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEditGroup" CssClass="dropdownlist" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvGroup" runat="server" ControlToValidate="ddlEditGroup"
                                                    ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid" InitialValue="-- Select Group --" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Manager E-mail" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTOEmail" runat="server" Text='<%#Eval("TECHOPS_EMAIL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" MaxLength="50" ID="txtEditTOEmail" runat="server"
                                                    Width="150px" CssClass="textbox" Text='<%#Eval("TECHOPS_EMAIL") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ACTIVE">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("ACTIVE") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%#Eval("ACTIVE") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LOC" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLC" runat="server" Text='<%#Eval("LOCATION_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblELC" Width="60px" runat="server" Text='<%#Eval("LOCATION_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGC" runat="server" Text='<%#Eval("GROUP_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEGC" Width="60px" runat="server" Text='<%#Eval("GROUP_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="40px">
                                            <EditItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                                    ImageUrl="~/images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CausesValidation="false"
                                                    CommandName="Cancel" ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" CausesValidation="false"
                                                    ImageUrl="~/images/Edit_16x16.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" CausesValidation="false"
                                                    OnClientClick="return confirm('Are you sure you want to delete?');" CommandName="Delete"
                                                    ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvUserMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvUserMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvUserMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvUserMaster" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvUserMaster" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export user master data into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
