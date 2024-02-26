<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ReportAssetTracking.aspx.cs" Inherits="ReportAssetTracking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            //document.getElementById('<%= ddlAssetType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetLocation.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetMake.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= lstModelName.ClientID%>').selectedIndex = -1;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').focus();
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
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebPages/AutoComplete.asmx" />
        </Services>
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Asset Tracking Report</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="Label11" runat="server" Enabled="false" Text="Asset Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlAssetType" Width="200px" Enabled="false" runat="server"
                                            TabIndex="1" CssClass="dropdownlist" AutoPostBack="True" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                                            <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" runat="server" Text="Category" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAssetCategory" Width="200px" runat="server" CssClass="dropdownlist"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                            OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset category" CausesValidation="false" />
                                        <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label9" runat="server" Text="Asset Make" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:DropDownList ID="ddlAssetMake" Width="200px" TabIndex="5" runat="server" CssClass="dropdownlist"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label10" runat="server" Text="Model Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td rowspan="2" style="text-align: left; vertical-align: top">
                                        <asp:ListBox ID="lstModelName" ToolTip="Select Multiple Model Names" SelectionMode="Multiple"
                                            Width="200px" runat="server"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label5" runat="server" Text="Asset Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:DropDownList ID="ddlAssetLocation" runat="server" OnSelectedIndexChanged="ddlAssetLocation_SelectedIndexChanged"
                                            Width="200px" CssClass="dropdownlist" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="btnRefreshLocation" runat="server" CausesValidation="false"
                                            ImageUrl="../images/Refresh_16x16.png" ToolTip="Refresh/reset location" OnClick="btnRefreshLocation_Click" />
                                        <asp:Label ID="lblLocCode" runat="server" CssClass="label" Text="0" Visible="False"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                    </td>
                                    <td style="text-align: center">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                    </td>
                                    <td style="text-align: center">
                                    </td>
                                    <td style="text-align: left;">
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="Label2" runat="server" Text="Total Count" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lblTotalCount" Text="0" runat="server" CssClass="label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                            ToolTip="Get asset tracking report based on filters " ImageUrl="~/images/Submit.png"
                                            Height="35px" Width="85px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                            ToolTip="Reset/Clear page fields" ImageUrl="~/images/Reset.png" Height="35px"
                                            Width="85px" CausesValidation="false" OnClick="btnClear_Click" />
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
                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4" runat="server">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvRptAssetTracking" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    OnPageIndexChanging="gvRptAssetTracking_PageIndexChanging" ShowFooter="false"
                                    CssClass="mGrid" PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:BoundField DataField="ASSET_CODE" HeaderText="Asset Code" />
                                        <asp:BoundField DataField="SERIAL_CODE" HeaderText="Serial Code" />
                                        <asp:BoundField DataField="ASSET_MAKE" HeaderText="Make" />
                                        <asp:BoundField DataField="MODEL_NAME" HeaderText="Model" />
                                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Category" />
                                        <asp:BoundField DataField="LOC_NAME" HeaderText="Location" />
                                        <asp:BoundField DataField="COMP_NAME" HeaderText="Company" />
                                        <asp:BoundField DataField="VENDOR_NAME" HeaderText="Vendor" />
                                        <asp:BoundField DataField="PURCHASED_DATE" HeaderText="Pur. Date" />
                                        <asp:BoundField DataField="PO_NUMBER" HeaderText="PO No." />
                                        <asp:BoundField DataField="PO_DATE" HeaderText="PO Date" />
                                        <asp:BoundField DataField="INVOICE_NO" HeaderText="Invioce No." />
                                        <asp:BoundField DataField="ASSET_ALLOCATED" HeaderText="Allocated" />
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvRptAssetTracking" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right" colspan="4">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" ToolTip="Export asset tracking report into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
