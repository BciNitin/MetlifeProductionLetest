<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="Home" Title="BCIL : ATS - Home" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function GetUser() {
            var user = '<%= Session["CURRENTUSER"].ToString() %>';
            if (user == "test") {
                alert('Please Note : Default user cannot change the password!');
                return false;
            }
            else {
                return true;
            }
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>
      <style>
        .text-right {
            text-align: right;
        }
        .text-center {
            text-align: center;
        }
        .grid-data {
            margin: 30px;
            overflow: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="div1" style="padding-top: 8px; padding-left: 10px; font: small-caps bold 20px  Helvetica, Arial;
            color: Green;">
        </div>
    </div>
    <div id="wrapper1">
        <table style="width: 100%; height: 600px;">
            <tr>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: center; font-family: Helvetica,Arial; font-weight: bold">
                    <asp:Label ID="Label1" runat="server" CssClass="label" Text="Logged in user"></asp:Label>&nbsp;:&nbsp;
                    <asp:Label ID="lblLoggedUser" runat="server" CssClass="label"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: center; font-family: Helvetica,Arial; font-weight: bold">
                    <asp:Label ID="Label2" runat="server" CssClass="label" Text="Logged in as "></asp:Label>&nbsp;:&nbsp;
                    <asp:Label ID="lblLoggedLocation" runat="server" CssClass="label"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
           <%-- <tr>
                <td>
                </td>
                <td style="text-align: center; font-family: Helvetica,Arial; font-weight: bold">
                    <div id="divChangePassword" class="pageAnchor">
                        <a href="ChangePassword.aspx" onclick="return GetUser();">Change Password</a>
                    </div>
                </td>
                <td>
                </td>
            </tr>--%>
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
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: center">
                    <h2 style="color: #722982; font-family: Helvetica,Arial;">
                        A&nbsp;S&nbsp;S&nbsp;E&nbsp;T&nbsp;&nbsp;&nbsp;&nbsp; T&nbsp;R&nbsp;A&nbsp;C&nbsp;K&nbsp;I&nbsp;N&nbsp;G&nbsp;&nbsp;&nbsp;&nbsp;
                        S&nbsp;Y&nbsp;S&nbsp;T&nbsp;E&nbsp;M
                    </h2>
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
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: center;">
                    <img src="../images/10828.png" alt="" />
                </td>
                <td>
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
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: center; font-family: Helvetica,Arial; font-weight: bold">
                    <asp:Panel ID="pnlAcquisition" CssClass="pageAnchor" Enabled="false" runat="server">
                        <a href="Reports.aspx?ReportID=2">No of Assets in Stock</a>&nbsp;&nbsp;:&nbsp;
                        <asp:Label ID="lblAcquisition" runat="server"></asp:Label>
                    </asp:Panel>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: center; font-family: Helvetica,Arial; font-weight: bold">
                    <asp:Panel ID="pnlAllocation" CssClass="pageAnchor" Enabled="false" runat="server">
                        <a href="Reports.aspx?ReportID=3">No of Assets Allocated</a>&nbsp;&nbsp;:&nbsp;
                        <asp:Label ID="lblAllocation" runat="server"></asp:Label>
                    </asp:Panel>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: center; font-family: Helvetica,Arial; font-weight: bold">
                    <asp:Panel ID="pnlTransfer" CssClass="pageAnchor" Enabled="false" runat="server">
                        <a href="Reports.aspx?ReportID=4">No of Assets in IUT</a>&nbsp;&nbsp;:&nbsp;
                        <asp:Label ID="lblTransfer" runat="server"></asp:Label>
                    </asp:Panel>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: center; font-family: Helvetica,Arial; font-weight: bold">
                    <asp:Panel ID="pnlScrap" CssClass="pageAnchor" Enabled="false" runat="server">
                        <a href="Reports.aspx?ReportID=5">No of Assets Scrapped</a>&nbsp;&nbsp;:&nbsp;
                        <asp:Label ID="lblScrap" runat="server"></asp:Label>
                    </asp:Panel>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <table id="TableStoreList" runat="server" style="width: 100%; text-align:center;">
                </table>
       <%-- <div style="position:absolute; left:0; right:0; text-align: center;">
            <table id="TableStoreList" runat="server" style="width: 100%; position:absolute;left:0;right:0;text-align:center;">
                </table>
        </div>--%>
    </div>
</asp:Content>
