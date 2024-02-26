<%@ Page Title="BCIL : ATS - Asset Summary Report" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ReportAssetSummary.aspx.cs" Inherits="ReportAssetSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= ddlAssetMake.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= lstModelName.ClientID%>').selectedIndex = -1;
            //document.getElementById('<%= ddlAssetType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlDepartment.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlProcess.ClientID%>').selectedIndex = 0;
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
        .style1
        {
            width: 112px;
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
            Asset Summary Report</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="12" style="width: 100%;" align="center">
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label3" runat="server" Text="Asset Location" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlAssetLocation" Width="200px" TabIndex="1" runat="server"
                        AutoPostBack="true" ToolTip="Select asset's location" OnSelectedIndexChanged="ddlAssetLocation_SelectedIndexChanged"
                        CssClass="dropdownlist">
                    </asp:DropDownList>
                    <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                    <asp:ImageButton ID="ibtnRefreshLocation" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                        OnClick="ibtnRefreshLocation_Click" ToolTip="Refresh/reset location" />
                    <asp:Label ID="lblLocCode" CssClass="label" Text="0" runat="server" Visible="false"></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label40" runat="server" CssClass="label" Text="Department"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold; height: 17px;">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlDepartment" ToolTip="Select department name" Width="200px"
                        runat="server" TabIndex="2" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label4" runat="server" Enabled="false" Text="Asset Type" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlAssetType" Enabled="false" Width="200px" runat="server"
                        TabIndex="3" CssClass="dropdownlist" AutoPostBack="True" ToolTip="Select asset type"
                        OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                        <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                        <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                        <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label6" runat="server" Text="Process" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlProcess" ToolTip="Select process name" Width="200px" TabIndex="4"
                        runat="server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlProcess_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label5" runat="server" Text="Asset Category" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlAssetCategory" Width="200px" TabIndex="5" runat="server"
                        CssClass="dropdownlist" ToolTip="Select asset's category" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                    <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                        OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset category" CausesValidation="false" />
                    <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                </td>
                <td style="text-align: right; vertical-align: top">
                    <asp:Label ID="Label2" runat="server" Text="Asset Make" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center; vertical-align: top">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left; vertical-align: top">
                    <asp:DropDownList ID="ddlAssetMake" Width="200px" TabIndex="6" runat="server" CssClass="dropdownlist"
                        AutoPostBack="true" ToolTip="Select asset make" OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; vertical-align: top">
                    <asp:Label ID="Label1" runat="server" Text="Model Name" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center; vertical-align: top">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:ListBox ID="lstModelName" ToolTip="Select one or more model names" SelectionMode="Multiple"
                        Width="200px" runat="server" TabIndex="7"></asp:ListBox>
                    <asp:Label ID="lblCompCode" runat="server" Visible="false" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: right; vertical-align: top">
                    <asp:Label ID="Label7" runat="server" Text="Port No." CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center; vertical-align: top">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left; vertical-align: top">
                    <asp:DropDownList ID="ddlPortNo" Width="200px" ToolTip="Select port no." TabIndex="6"
                        runat="server" CssClass="dropdownlist">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ImageUrl="~/images/Submit.png"
                        TabIndex="8" Height="35px" Width="85px" ToolTip="Get asset summary report as per filters selected"
                        OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="~/images/Reset.png"
                        TabIndex="9" Height="35px" Width="85px" ToolTip="Refresh/reset fields" CausesValidation="false"
                        OnClick="btnClear_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="10" Enabled="true" ToolTip="Export asset summary report into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="5" style="text-align: left">
                    <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Button ID="btnGetAssets" runat="server" CssClass="button" Visible="false" Width="200px"
                        Text="Get assets list for selected models" OnClick="btnGetAssets_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div id="DivGrid">
                        <asp:GridView ID="gvAssetSummary" runat="server" AllowPaging="True" PageSize="50"
                            AutoGenerateColumns="False" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvAssetSummary_PageIndexChanging"
                            OnRowUpdating="gvAssetSummary_RowUpdating">
                            <Columns>
                                <asp:TemplateField HeaderText="Category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("CATEGORY_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("DEPT_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Process">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProcess" runat="server" Text='<%#Eval("PROCESS_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Port No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPortNo" runat="server" Text='<%#Eval("PORT_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Make">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetMake" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModel" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCnt" runat="server" Text='<%#Eval("ASSET_COUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View" HeaderStyle-Width="50px">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imagebuttonView" ToolTip="View" CommandName="Update" ImageUrl="~/images/View_16x16.png"
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="CatCode">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCatCode" runat="server" Text='<%#Eval("CATEGORY_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="LocCode">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationCode" runat="server" Text='<%#Eval("ASSET_LOCATION") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="Dept">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeptCode" runat="server" Text='<%#Eval("DEPARTMENT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="Process">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProcessCode" runat="server" Text='<%#Eval("ASSET_PROCESS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="pgr"></PagerStyle>
                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: right">
                    <asp:Label ID="lblAssetCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvAssetDetails" runat="server" AllowPaging="True" PageSize="50"
                                    AutoGenerateColumns="False" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvAssetDetails_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FAMS ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetID" runat="server" Text='<%#Eval("ASSET_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetSrlCode" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("PO_NUMBER") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Make">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryName" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Process">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("PROCESS_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("DEPT_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAssetDetails" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: right">
                    <asp:ImageButton ID="btnExportDetails" Visible="false" runat="server" TabIndex="13"
                        Enabled="true" ToolTip="Export asset summary report into excel file" ImageUrl="~/images/Excel-icon (2).png"
                        CausesValidation="false" OnClick="btnExportDetails_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
