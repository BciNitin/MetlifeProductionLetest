<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="CreateInitialData.aspx.cs" Inherits="WebPages_CreateInitialData" Title="BCIL : ATS - Create Initial Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= txtCompanyCode.ClientID%>').value = "";
            document.getElementById('<%= txtCompanyName.ClientID%>').value = "";
            document.getElementById('<%= txtLocationName.ClientID%>').value = "";
            //document.getElementById('<%= txtLocationCode.ClientID%>').value = "";
            //document.getElementById('<%= txtGroupCode.ClientID%>').value = "";
            //document.getElementById('<%= txtGroupName.ClientID%>').value = "";
            //document.getElementById('<%= txtUserID.ClientID%>').value = "";
            document.getElementById('<%= txtUserName.ClientID%>').value = "";
            document.getElementById('<%= txtPassword.ClientID%>').value = "";
            document.getElementById('<%= txtConfirmPassword.ClientID%>').value = "";
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtAdminEmail.ClientID%>').value = "";
            document.getElementById('<%= txtTechopsEmail.ClientID%>').value = "";
            document.getElementById('<%= txtCompanyCode.ClientID%>').focus();
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
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
            Create Initial Details
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="4" style="width: 100%" align="center">
            <tr>
                <td colspan="6">
                    <asp:Label ID="Label6" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="16" cellpadding="18"
                                align="center">
                                <tr>
                                    <td style="text-align: right; width: 17%">
                                        <asp:Label ID="Label13" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Company Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; width: 1%">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; width: 32%">
                                        <asp:TextBox ID="txtCompanyCode" CssClass="textbox" runat="server" Width="250px"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;"
                                            TabIndex="1" MaxLength="2" ToolTip="Enter company code in 2 characters" autocomplete="off"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right; width: 17%">
                                        <asp:Label ID="Label14" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Company Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; width: 1%">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; width: 32%">
                                        <asp:TextBox ID="txtCompanyName" CssClass="textbox" runat="server" Width="250px"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;"
                                            TabIndex="2" autocomplete="off" ToolTip="Enter company name"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label15" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Text="Location Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtLocationCode" runat="server" CssClass="readonlytext" TabIndex="3"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;"
                                            MaxLength="2" Width="250px" ToolTip="Enter location code in 2 characters" autocomplete="off"
                                            ReadOnly="true" Text="IN">
                                        </asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label16" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label12" runat="server" Text="Location Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtLocationName" runat="server" CssClass="textbox" TabIndex="4"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;"
                                            Width="250px" autocomplete="off" ToolTip="Enter location name"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label17" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label9" runat="server" CssClass="label" Text="Admin Group Code"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtGroupCode" Text="SYSADMIN" runat="server" CssClass="readonlytext"
                                            TabIndex="5" Width="250px" ToolTip="Enter default group code" MaxLength="10"
                                            ReadOnly="true" autocomplete="off">
                                        </asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label18" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label7" runat="server" CssClass="label" Text="Admin Group Name"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtGroupName" autocomplete="off" runat="server" CssClass="readonlytext"
                                            TabIndex="6" ReadOnly="true" ToolTip="Enter default group name" Text="System Administrator"
                                            Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label19" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label11" runat="server" CssClass="label" Text="Admin User ID"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtUserID" autocomplete="off" runat="server" CssClass="readonlytext"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;"
                                            TabIndex="7" ReadOnly="true" ToolTip="Enter default group's user id" Text="SYSADMIN"
                                            Width="250px">
                                        </asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label20" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" CssClass="label" Text="Admin User Name"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtUserName" autocomplete="off" runat="server" CssClass="textbox"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;"
                                            TabIndex="8" Width="250px" ToolTip="Enter user name"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label21" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label10" runat="server" CssClass="label" Text="Admin Password"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtPassword" autocomplete="off" runat="server" CssClass="textbox"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;" TabIndex="9"
                                            MaxLength="10" ToolTip="Enter password" Width="250px" TextMode="Password"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label22" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label8" runat="server" CssClass="label" Text="Confirm Password"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtConfirmPassword" autocomplete="off" runat="server" CssClass="textbox"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;" MaxLength="10"
                                            TabIndex="10" Width="250px" ToolTip="Confirm password" TextMode="Password"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label24" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label25" runat="server" CssClass="label" Text="Admin E-Mail"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtAdminEmail" autocomplete="off" runat="server" CssClass="textbox"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;" TabIndex="11"
                                            MaxLength="50" ToolTip="Enter user's e-mail id" Width="250px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtAdminEmail"
                                            CssClass="validation" ErrorMessage="[Invalid E-Mail ID]" Font-Size="12px" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                            ValidationGroup="Submit">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: center">
                                    </td>
                                    <td style="text-align: left">
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword"
                                            ControlToValidate="txtConfirmPassword" CssClass="validation" ValidationGroup="Submit"
                                            ErrorMessage="[Confirm Password Doesn't Match]">
                                        </asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label23" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label5" runat="server" CssClass="label" Text="Manager E-Mail"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtTechopsEmail" autocomplete="off" runat="server" CssClass="textbox"
                                            oncopy="return false;" onpaste="return false;" oncut="return false;" TabIndex="12"
                                            MaxLength="50" ToolTip="Enter user's manager id" Width="250px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtTechopsEmail"
                                            CssClass="validation" ErrorMessage="[Invalid E-Mail ID]" Font-Size="12px" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                            ValidationGroup="Submit">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: center">
                                    </td>
                                    <td style="text-align: left">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:RequiredFieldValidator ID="rfvCompCode" runat="server" ControlToValidate="txtCompanyCode"
                                            ErrorMessage="[Company Code Required]" Display="Dynamic" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_VendName" runat="server" ControlToValidate="txtCompanyName"
                                            ErrorMessage="[Company Name Required]" Display="Dynamic" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_Add1" runat="server" ControlToValidate="txtLocationCode"
                                            ErrorMessage="[Location Code Required]" Display="Dynamic" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_Country" runat="server" ControlToValidate="txtLocationName"
                                            ErrorMessage="[Location Name Required]" Display="Dynamic" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGroupCode"
                                            ErrorMessage="[Group Code Required]" Display="Dynamic" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_Group" runat="server" ControlToValidate="txtGroupName"
                                            ErrorMessage="[Group Name Required]" Display="Dynamic" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUserID"
                                            ErrorMessage="[User Id Required]" Display="Dynamic" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_Country1" runat="server" ControlToValidate="txtUserName"
                                            ErrorMessage="[User Name Required]" Display="Dynamic" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                                            ErrorMessage="[Password Required]" Display="Dynamic" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_Country2" runat="server" ControlToValidate="txtConfirmPassword"
                                            ErrorMessage="[Confirm Password Required]" Display="Dynamic" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtAdminEmail"
                                            ErrorMessage="[Admin User E-mail Required]" Display="Dynamic" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_Email" runat="server" ControlToValidate="txtTechopsEmail"
                                            ErrorMessage="[Manager E-mail Required]" Display="Dynamic" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:RegularExpressionValidator ID="revCompCode" runat="server" ControlToValidate="txtCompanyCode"
                                            CssClass="validation" Display="dynamic" ErrorMessage="[Company Code Should Be Alphanumeric]"
                                            ValidationExpression="^[0-9a-zA-Z]+$" ValidationGroup="Submit">
                                        </asp:RegularExpressionValidator>&nbsp;
                                        <asp:RegularExpressionValidator ID="REP_Email" runat="server" ControlToValidate="txtUserID"
                                            ValidationGroup="Submit" CssClass="validation" ErrorMessage="[User ID Should Be Alphanumeric]"
                                            ValidationExpression="^[0-9a-zA-Z]+$">
                                        </asp:RegularExpressionValidator>&nbsp;
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtGroupCode"
                                            CssClass="validation" Display="dynamic" ErrorMessage="[Group Code Should Be Alphanumeric]"
                                            ValidationExpression="^[0-9a-zA-Z]+$" ValidationGroup="Submit">
                                        </asp:RegularExpressionValidator>&nbsp;
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="dynamic"
                                            ControlToValidate="txtLocationCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z]+$" ErrorMessage="[Location Code Should Be Alphanumeric]">
                                        </asp:RegularExpressionValidator>&nbsp;
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtGroupName"
                                            CssClass="validation" Display="dynamic" ErrorMessage="[Group Name Should Be Alphanumeric]"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ValidationGroup="Submit">
                                        </asp:RegularExpressionValidator>&nbsp;
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtUserName"
                                            CssClass="validation" Display="dynamic" ErrorMessage="[User Name Should Be Alphanumeric]"
                                            ValidationExpression="^[0-9a-z A-Z]+$" ValidationGroup="Submit">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="13"
                                            Text="Save Initial Data" CssClass="button" ToolTip="Save initial details" Width="120px"
                                            OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
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
        </table>
    </div>
</asp:Content>
