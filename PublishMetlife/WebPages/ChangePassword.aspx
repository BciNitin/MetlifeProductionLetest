<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="~/WebPages/ChangePassword.aspx.cs" Inherits="ChangePassword" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtOldPswd.ClientID%>').value = "";
            document.getElementById('<%= txtNewPswd.ClientID%>').value = "";
            document.getElementById('<%= txtConfPswd.ClientID%>').value = "";
        }
        function ShowErrMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Existing password is incorrect.';
        }
        function ShowSavedSuccessMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Your password is changed successfully.';
        }
        function DefaultUserMsg() {
            alert('Please Note : Default user cannot change the password!');
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to execute this operation!');
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
            width: 260px;
        }
        .style3
        {
            width: 250px;
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
            Change Password</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="17" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="User ID" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtUserID" CssClass="textbox" Enabled="false" ToolTip="User ID"
                                            runat="server" Width="200px" TabIndex="-1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_UserID" runat="server" ControlToValidate="txtUserID"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td rowspan="5" valign="top">
                                        <img src="../images/reset_pswd_icon.png" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="Old Password" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtOldPswd" CssClass="textbox" TextMode="Password" ToolTip="Enter Existing Password"
                                            runat="server" Width="200px" TabIndex="1" oncopy="return false;" onpaste="return false;" oncut="return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_OldPswd" runat="server" ControlToValidate="txtOldPswd"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="New Password" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtNewPswd" CssClass="textbox" TextMode="Password" ToolTip="Enter New Password"
                                            runat="server" Width="200px" TabIndex="2" oncopy="return false;" onpaste="return false;" oncut="return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewPswd"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" runat="server" Text="Confirm New Password" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtConfPswd" CssClass="textbox" TextMode="Password" ToolTip="Confirm New Password"
                                            runat="server" Width="200px" TabIndex="3" oncopy="return false;" onpaste="return false;" oncut="return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtConfPswd"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>&nbsp;
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtNewPswd"
                                            CssClass="validation" ControlToValidate="txtConfPswd" ErrorMessage="[Doesn't match]"
                                            ValidationGroup="Submit"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style2">
                                    </td>
                                    <td align="left">
                                    </td>
                                    <td align="left">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ToolTip="Submit"
                                            ImageUrl="../images/Submit.png" TabIndex="4" Height="35px" Width="85px" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="../images/Reset.png"
                                            TabIndex="5" Height="35px" Width="85px" CausesValidation="False" ToolTip="Reset/Clear" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
