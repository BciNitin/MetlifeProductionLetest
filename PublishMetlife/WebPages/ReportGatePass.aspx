<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ReportGatePass.aspx.cs" Inherits="ReportGatePass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= rdoAll.ClientID%>').checked = true;
            document.getElementById('<%= rdoReturnable.ClientID%>').checked = false;
            document.getElementById('<%= rdoNonReturnable.ClientID%>').checked = false;
            document.getElementById('<%= txtFromDate.ClientID%>').value = "";
            document.getElementById('<%= txtToDate.ClientID%>').value = "";
            document.getElementById('<%= ddlGatePassNo.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetLocation.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ChkRtnDateExpired.ClientID%>').disabled = true;
            document.getElementById('<%= ChkRtnDateExpired.ClientID%>').checked = false;
            document.getElementById('<%= ChkLiveGatePass.ClientID%>').checked = false;
            document.getElementById('<%= ddlAssetLocation.ClientID%>').focus();
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
        function GetGatePassTypeValue(rdoType) {
            if (rdoType.value == 'rdoReturnable') {
                document.getElementById('<%= rdoReturnable.ClientID%>').checked = true;
                document.getElementById('<%= rdoNonReturnable.ClientID%>').checked = false;
                document.getElementById('<%= rdoAll.ClientID%>').checked = false;
                document.getElementById('<%= ChkRtnDateExpired.ClientID%>').disabled = false;
                document.getElementById('<%= ChkRtnDateExpired.ClientID%>').checked = false;
            }
            else if (rdoType.value == 'rdoNonReturnable') {
                document.getElementById('<%= rdoReturnable.ClientID%>').checked = false;
                document.getElementById('<%= rdoNonReturnable.ClientID%>').checked = true;
                document.getElementById('<%= rdoAll.ClientID%>').checked = false;
                document.getElementById('<%= ChkRtnDateExpired.ClientID%>').disabled = true;
                document.getElementById('<%= ChkRtnDateExpired.ClientID%>').checked = false;
            }
            else if (rdoType.value == 'rdoAll') {
                document.getElementById('<%= rdoAll.ClientID%>').checked = true;
                document.getElementById('<%= rdoReturnable.ClientID%>').checked = false;
                document.getElementById('<%= rdoNonReturnable.ClientID%>').checked = false;
                document.getElementById('<%= ChkRtnDateExpired.ClientID%>').disabled = true;
                document.getElementById('<%= ChkRtnDateExpired.ClientID%>').checked = false;
            }
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
            Gate Pass Report</div>
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
                                <asp:Label ID="Label5" runat="server" Text="Select Location" CssClass="label"></asp:Label>
                            </td>
                            <td style="text-align: center">
                                <div style="font-weight: bold">
                                    :</div>
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlAssetLocation" runat="server" OnSelectedIndexChanged="ddlAssetLocation_SelectedIndexChanged"
                                    Width="200px" CssClass="dropdownlist" ToolTip="Select asset's location" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                <asp:ImageButton ID="btnRefreshLocation" runat="server" CausesValidation="false"
                                    ImageUrl="../images/Refresh_16x16.png" ToolTip="Refresh/reset location" OnClick="btnRefreshLocation_Click" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="Label11" runat="server" Text="GatePass No." CssClass="label"></asp:Label>
                            </td>
                            <td style="text-align: center">
                                <div style="font-weight: bold">
                                    :</div>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlGatePassNo" ToolTip="Select gate pass no." Width="200px"
                                    runat="server" CssClass="dropdownlist">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="Label1" runat="server" Text="GatePass Type" CssClass="label"></asp:Label>
                            </td>
                            <td style="text-align: center">
                                <div style="font-weight: bold">
                                    :</div>
                            </td>
                            <td style="text-align: left">
                                <asp:RadioButton ID="rdoAll" ToolTip="Select to get all types of gate passes" runat="server"
                                    onClick="GetGatePassTypeValue(this);" CssClass="label" Text=" All" Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RadioButton ID="rdoReturnable" runat="server" onClick="GetGatePassTypeValue(this);"
                                    CssClass="label" Text=" Returnable" ToolTip="Select to get returnable gate passes" />&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RadioButton ID="rdoNonReturnable" runat="server" onClick="GetGatePassTypeValue(this);"
                                    CssClass="label" Text=" Non-Returnable" ToolTip="Select to get non-returnable gate passes" />
                            </td>
                            <td style="text-align: right;">
                                <asp:CheckBox ID="ChkLiveGatePass" CssClass="label" Text="  Live GatePass" runat="server" />
                            </td>
                            <td style="text-align: center">
                            </td>
                            <td style="text-align: left;display:none;">
                                <asp:CheckBox ID="ChkRtnDateExpired" ToolTip="Select to get a list of live gate passses"
                                    CssClass="label" Enabled="false" Text="  Return Date Expired" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="Label2" runat="server" Text="From Date" CssClass="label"></asp:Label>
                            </td>
                            <td style="text-align: center">
                                <div style="font-weight: bold">
                                    :</div>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox autocomplete="off" ID="txtFromDate" runat="server" onfocus="showCalendarControl(this);"
                                    Width="200px" CssClass="textbox" ToolTip="Enter/select from date"></asp:TextBox>
                            </td>
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
                                    ToolTip="Get a list of gate passes based on search criteria" ImageUrl="~/images/Submit.png"
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
                    <div id="DivGrid" runat="server">
                        <asp:GridView ID="gvRptGatePass" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            OnPageIndexChanging="gvRptGatePass_PageIndexChanging" ShowFooter="false" CssClass="mGrid"
                            PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:TemplateField HeaderText="GatePass No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPCode" runat="server" Text='<%#Eval("GATEPASS_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompCode" runat="server" Text='<%#Eval("COMP_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPDate" runat="server" Text='<%#Eval("GATEPASS_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPType" runat="server" Text='<%#Eval("GATEPASS_TYPE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exp. Return Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpRtnDate" runat="server" Text='<%#Eval("EXP_RETURN_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLoc" runat="server" Text='<%#Eval("ASSET_LOCATION") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="GatePass For">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPFor" runat="server" Text='<%#Eval("GATEPASS_FOR") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="In Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPInLoc" runat="server" Text='<%#Eval("GATEPASS_IN_LOC") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="In Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPIndate" runat="server" Text='<%#Eval("GATEPASS_IN_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Out Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPOutLoc" runat="server" Text='<%#Eval("GATEPASS_OUT_LOC") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Out Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetType" runat="server" Text='<%#Eval("GATEPASS_OUT_DATE") %>'></asp:Label>
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
