<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ReportAssetDashBoard.aspx.cs" Inherits="ReportAssetDashBoard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= chkAllLocations.ClientID%>').checked = false;
            document.getElementById('<%= ddlAssetLocation.ClientID%>').focus();
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = "Please Note : You are not authorised to execute this operation.";
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .style2
        {
            height: 25px;
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
            Asset Dashboard</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" style="width: 100%;" align="center">
            <tr>
                <td colspan="6">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table id="Table3" runat="server" cellpadding="5" cellspacing="10" style="width: 100%;"
                                align="center">
                                <tr>
                                    <td style="text-align: right; width: 48%">
                                        <asp:Label ID="Label3" runat="server" Text="Select Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; width: 4%">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; width: 48%">
                                        <asp:DropDownList ID="ddlAssetLocation" Width="200px" TabIndex="4" runat="server"
                                            AutoPostBack="true" ToolTip="Select asset's location to get assets count list"
                                            OnSelectedIndexChanged="ddlAssetLocation_SelectedIndexChanged" CssClass="dropdownlist">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="ibtnRefreshLocation" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                            OnClick="ibtnRefreshLocation_Click" ToolTip="Refresh/reset location" />
                                        <asp:Label ID="lblLocCode" CssClass="label" Text="0" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:CheckBox ID="chkAllLocations" ToolTip="Select to get count of assets over all locations"
                                            Text="   All Locations" CssClass="label" runat="server" />
                                    </td>
                                    <td style="text-align: center">
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblLocationName" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ImageUrl="~/images/Submit.png"
                                            TabIndex="35" Height="35px" ToolTip="Get a count of all assets with respect to asset category"
                                            Width="85px" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="~/images/Reset.png"
                                            TabIndex="36" Height="35px" Width="85px" CausesValidation="false" OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                            <asp:PostBackTrigger ControlID="btnClear" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: center; width: 100%">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="pnlDashboard" runat="server">
                            <ContentTemplate>
                                <table id="Table2" runat="server" cellpadding="5" cellspacing="10" style="width: 100%;
                                    border: 2px double #006600;" align="center">
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label1" runat="server" Text="DESKTOPS" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label2" runat="server" Text="LAPTOPS" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label4" runat="server" Text="MONITORS" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label5" runat="server" Text="MISCELLANEOUS" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label6" runat="server" Text="STORAGE" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label7" runat="server" Text="SERVERS" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td rowspan="6">
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label15" runat="server" Text="PRODUCTION" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDTProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblLTProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMNProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMCProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSTProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSVProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label16" runat="server" Text="STOCK" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDTStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblLTStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMNStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMCStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSTStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSVStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label17" runat="server" Text="FAULTY" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDTFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblLTFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMNFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMCFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSTFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSVFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label21" runat="server" Text="SCRAPPED" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDTScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblLTScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMNScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMCScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSTScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSVScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label28" runat="server" Text="SOLD" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDTSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblLTSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMNSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMCSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSTSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSVSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" class="style2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label13" runat="server" Text="TOTAL" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDTTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblLTTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMNTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblMCTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSTTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSVTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label8" runat="server" Text="THIN CLIENTS" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label9" runat="server" Text="IP PHONE" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label10" runat="server" Text="BLACKBERRY" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label11" runat="server" Text="PROJECTORS" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label12" runat="server" Text="PRINTERS" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label22" runat="server" Text="PSTN PHONE" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td rowspan="6">
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label18" runat="server" Text="PRODUCTION" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTCProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblIPProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBBProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPRProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPTProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPPProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label19" runat="server" Text="STOCK" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTCStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblIPStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBBStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPRStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPTStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPPStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label20" runat="server" Text="FAULTY" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTCFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblIPFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBBFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPRFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPTFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPPFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label35" runat="server" Text="SCRAPPED" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTCScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblIPScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBBScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPRScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPTScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPPScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label42" runat="server" Text="SOLD" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTCSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblIPSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBBSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPRSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPTSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPPSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" class="style2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label14" runat="server" Text="TOTAL" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTCTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblIPTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBBTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPRTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPTTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPPTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label24" runat="server" Text="SWITCH" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label25" runat="server" Text="ROUTER" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label26" runat="server" Text="FIREWALL" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label27" runat="server" Text="BIOMETRICS" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td rowspan="6">
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label30" runat="server" Text="PRODUCTION" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSWProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblRTProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFWProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBMProd" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label38" runat="server" Text="STOCK" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSWStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblRTStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFWStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBMStk" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label46" runat="server" Text="FAULTY" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSWFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblRTFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFWFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBMFlty" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label53" runat="server" Text="SCRAPPED" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSWScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblRTScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFWScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBMScrapped" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label60" runat="server" Text="SOLD" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSWSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblRTSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFWSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBMSold" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" class="style2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label67" runat="server" Text="TOTAL" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSWTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblRTTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFWTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblBMTotal" runat="server" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTotalAssets" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
