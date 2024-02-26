<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" Title="BCIL : ATS - Error Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Error</div>
    </div>
    <div id="wrapper1">
        <table style="width: 100%;height:600px;">
            <tr>
                <td colspan="4" style="text-align:center">
                    <h3>Error Occured !!</h3>
                </td>
            </tr>
            <tr>
                <td style="width: 151px">
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 151px; vertical-align:top;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/error2.jpg" Height="140px"
                        Width="140px" />
                </td>
                <td colspan="2" style="vertical-align:top;text-align:left">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="Label" CssClass="label"></asp:Label>
                </td>
                <td>
                </td>                
            </tr>
            <tr>
                <td style="width: 151px">
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
     </div>
</asp:Content>

