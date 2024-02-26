<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="ReportProfitAndLoss.aspx.cs" Inherits="WebPages_ReportProfitAndLoss" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
          
           
            document.getElementById('<%= txtToDate.ClientID%>').value = "";
          
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
      
        function noBack() {
            window.history.forward(1);
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Profit And Loss Report
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="Label3" runat="server" Text="To Date" CssClass="label"></asp:Label>
                            </td>
                            <td style="text-align: center">
                                <div style="font-weight: bold">
                                    :</div>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox autocomplete="off" ID="txtToDate" runat="server" onfocus="showCalendarControl(this);"
                                    Width="200px" CssClass="textbox" ToolTip="Enter/select to date"></asp:TextBox>
                            </td>
                           
                            
                        </tr>
                        <tr>
                            <td colspan="6">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="center">
                                <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                    ToolTip="Get a list of depreciation report based on search criteria" ImageUrl="~/images/Submit.png"
                                    Height="35px" Width="85px" OnClick="btnSubmit_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                    ToolTip="Reset/clear fields" ImageUrl="~/images/Reset.png" Height="35px" Width="85px"
                                    CausesValidation="false" OnClick="btnClear_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="false" ToolTip="Export gate pass report into excel file"
                                    ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="text-align: left">
                                <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblRecordCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4" runat="server">
                    <div id="DivGrid" runat="server" style ="overflow:auto ;width: 950px; ">
                        <asp:GridView ID="gvDProfitAndLoss" runat="server" AllowPaging="True" AutoGenerateColumns="true"
                            OnPageIndexChanging="gvDProfitAndLoss_PageIndexChanging" ShowFooter="false" CssClass="mGrid"
                            PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            
                            <PagerStyle CssClass="pgr"></PagerStyle>
                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>


