<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" Title="BCIL : ATS - Forgot Password" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>BCIL : ASSET TACKING SYSTEM</title>
    <link rel="stylesheet" href="../css/style.css" type="text/css" />
    <style type="text/css">
        .style4
        {
            height: 40px;
        }
    </style>
    <script language="javascript" type="text/javascript">
    window.history.forward(1);
        function ClearFields()
        {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtUserID.ClientID %>').value = "";
            document.getElementById('<%= txtEmailId.ClientID %>').value = "";
            document.getElementById('<%= txtCnfEmailId.ClientID %>').value = "";
        }
        function ShowErrMsg(msg)
        {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg()
        {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowNotFoundMsg()
        {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : User ID is not associated with the e-mail entered!';
        }
        function SendSuccessMsg()
        {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Your password has been sent to registered e-mail!';
        }
        function ShowAlert()
        {
            alert('Please Note : You are not authorised to execute this operation!');
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>
  
<link href="../css/style.css" rel="Stylesheet" type="text/css" />   

</head>
<body  class="gray">
    <form id="form1" runat="server">
    <div id="pageFrame" align="center">
        <div id="pageContainer" align="left">
            <div id="topnav">
                <div id="topnavInner">
                    <div id="logo">
                        <img src="../images/BCILLogo.png" alt="CompanyLogo" />
                    </div>
                </div>
            </div>
            <div id="wrapper">
                <div id="pageTitle">
                Forgot Password
                </div>
            </div>
            <div id="wrapper1">
                <table style="width:100%;height:600px" cellspacing="15">
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td rowspan="8" valign="top">
                            <img src="../images/forgot_pass.jpg" style="width:200px;height:200px;" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label1" runat="server" Text="Enter User ID" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold;">
                                :</div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserID" CssClass="textbox" runat="server" Width="200px" ToolTip="Enter User ID" 
                                TabIndex="1" ValidationGroup="Submit"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFV_UserID" runat="server" ControlToValidate="txtUserID"
                                ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td  style="text-align: right">
                            <asp:Label ID="Label2" runat="server" Text="Enter E-Mail ID" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold;">
                                :</div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmailId" CssClass="textbox" runat="server" ToolTip="Enter E-Mail Address"
                                Width="200px" TabIndex="2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmailId"
                                ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                            &nbsp;<asp:RegularExpressionValidator ID="REP_Email" runat="server" ValidationGroup="Submit"  CssClass="validation"
                                ControlToValidate="txtEmailId" ErrorMessage="[ Invalid E-Mail ]"
                                ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" Font-Size="12px">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label3" runat="server" Text="Confirm E-Mail ID" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold;">
                                :</div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCnfEmailId" CssClass="textbox" runat="server" ToolTip="Confirm E-Mail Address"
                                Width="200px" TabIndex="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCnfEmailId"
                                ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  CssClass="validation"
                                ValidationGroup="Submit" ControlToValidate="txtCnfEmailId" ErrorMessage="[ Invalid E-Mail ]"
                                ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" Font-Size="12px">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4"></td>
                        <td></td>
                        <td valign="top">
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="Submit" Font-Size="12px" 
                                ErrorMessage="[ Confirm E-Mail doesn't match. ]" ControlToValidate="txtCnfEmailId" CssClass="validation"
                                ControlToCompare="txtEmailId"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4"></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="style4"></td>
                        <td></td>
                        <td style="vertical-align: top; padding-top:15px;">
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ToolTip="Send E-Mail" 
                                ImageUrl="../images/Submit.png" TabIndex="4" Height="35px" Width="85px" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="../images/Reset.png"
                                TabIndex="5" Height="35px" Width="85px" CausesValidation="false" ToolTip="Reset/Clear" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:left">
                            <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-size: 12pt; text-align: center;font-weight:bold;text-transform:none;font-variant:normal;">
                            <a href="UserLogin.aspx" tabindex="6" style="text-align: center">Click here to go to login page</a>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div id="footer">
                <div class="copyright">
                    <span>Copyright &copy; 2013 Bar Code India Ltd. All Rights Reserved.</span>
                </div>
                <div class="pagebottomLinks">
                    <%--<a id="corphome" href="http://www.fisglobal.com/index.htm" target="_blank">BCIL Corporate Home</a>--%>
                </div>
                <div class="designedby">
                    <span>Designed & Developed By <a href="http://www.barcodeindia.com" target="_blank"> Bar Code India Limited
                    </a></span>
                </div>
            </div>
        </div>
    </div>
    <div>
    </div>
    </form>
</body>
</html>