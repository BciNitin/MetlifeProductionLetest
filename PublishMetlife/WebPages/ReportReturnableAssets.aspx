<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ReportReturnableAssets.aspx.cs" Inherits="ReportReturnableAssets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= ChkRtnDateExpired.ClientID%>').checked = false;
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg;
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
            Returnable Asset Report</div>
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
                            <td colspan="2" align="right">
                                <asp:Label ID="Label36" runat="server" Text="Select Returnable Date Status" CssClass="label"></asp:Label>
                            </td>
                            <td colspan="2" align="center">
                                <div style="font-weight: bold">
                                    :</div>
                            </td>
                            <td colspan="2" align="left">
                                <asp:CheckBox ID="ChkRtnDateExpired" CssClass="label" Text="  Returnable Date Expired"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="center">
                                <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                    ToolTip="Submit" ImageUrl="~/images/Submit.png" Height="35px" Width="85px" OnClick="btnSubmit_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                    ToolTip="Reset/Clear" ImageUrl="~/images/Reset.png" Height="35px" Width="85px"
                                    CausesValidation="false" OnClick="btnClear_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="false" ToolTip="Export returnable assets' list into excel file"
                                    ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="Td1" colspan="4" runat="server">
                    <div id="DivGrid" runat="server">
                        <asp:GridView ID="gvRptReturnAsset" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            OnPageIndexChanging="gvRptReturnAsset_PageIndexChanging" ShowFooter="false" CssClass="mGrid"
                            PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:TemplateField HeaderText="Asset Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Serial No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAllocDate" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="GatePass No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAlloc" runat="server" Text='<%#Eval("GATEPASS_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="GatePass In Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeptName" runat="server" Text='<%#Eval("GATEPASS_IN_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exp. Return Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("EXP_RETURN_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="pgr"></PagerStyle>
                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
