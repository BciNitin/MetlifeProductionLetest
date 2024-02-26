<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ReportAMCWarranty.aspx.cs" Inherits="ReportAMCWarranty" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtFromDate.ClientID%>').value = "";
            document.getElementById('<%= txtToDate.ClientID%>').value = "";
            document.getElementById('<%= txtNoOfYrsOld.ClientID%>').value = "";
            document.getElementById('<%= ddlAMCWarranty.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetMake.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= lstModelName.ClientID%>').selectedIndex = -1;
            //document.getElementById('<%= ddlAssetType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAgeType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAMCWarranty.ClientID%>').focus();
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
            width: 150px;
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
            Asset History Report
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="15" style="width: 100%;" align="center">
            <tr>
                <td colspan="6">
                    <asp:UpdatePanel ID="upSubmit" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="Label2" runat="server" Text="From Purchase Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtFromDate" autocomplete="off" CssClass="textbox" MaxLength="2"
                                            onfocus="showCalendarControl(this);" ValidationGroup="Submit" runat="server"
                                            Width="200px" TabIndex="1" ToolTip="Enter/select from purchased date"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;" class="style1">
                                        <asp:Label ID="Label1" runat="server" Text="To Purchase Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtToDate" autocomplete="off" CssClass="textbox" MaxLength="2" onfocus="showCalendarControl(this);"
                                            ValidationGroup="Submit" runat="server" Width="200px" TabIndex="2" ToolTip="Enter/select to purchased date"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label40" runat="server" CssClass="label" Text="AMC/Warranty"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold; height: 17px;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAMCWarranty" ToolTip="Select AMC/Warranty/None type" Width="200px"
                                            runat="server" TabIndex="3" CssClass="dropdownlist">
                                            <asp:ListItem Text="-- Select --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="AMC" Value="AMC"></asp:ListItem>
                                            <asp:ListItem Text="WARRANTY" Value="WARRANTY"></asp:ListItem>
                                            <asp:ListItem Text="NONE" Value="NONE"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label9" runat="server" Text="Select Age Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAgeType" ToolTip="Select to get asset's age in days/months/years"
                                            Width="200px" runat="server" TabIndex="3" CssClass="dropdownlist">
                                            <asp:ListItem Text="-- Select Age Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="DAY" Value="DAY"></asp:ListItem>
                                            <asp:ListItem Text="MONTH" Value="MONTH"></asp:ListItem>
                                            <asp:ListItem Text="YEAR" Value="YEAR"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label4" runat="server" Text="Asset Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:DropDownList ID="ddlAssetLocation" Width="200px" TabIndex="4" runat="server"
                                            AutoPostBack="true" ToolTip="Select asset's location" OnSelectedIndexChanged="ddlAssetLocation_SelectedIndexChanged"
                                            CssClass="dropdownlist">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="ibtnRefreshLocation" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                            OnClick="ibtnRefreshLocation_Click" ToolTip="Refresh/reset location" />
                                        <asp:Label ID="lblLocCode" CssClass="label" Text="0" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label3" runat="server" Text="Day/Month/Year" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:TextBox ID="txtNoOfYrsOld" autocomplete="off" CssClass="textbox" MaxLength="2"
                                            ValidationGroup="Submit" runat="server" Width="200px" TabIndex="4" ToolTip="Enter no. of days/months/years"></asp:TextBox>
                                        <asp:RadioButtonList ID="rblAgeCriteria" CssClass="label" RepeatDirection="Horizontal"
                                            runat="server">
                                            <asp:ListItem Text="Greater than equal to" Selected="True" Value="GTET"></asp:ListItem>
                                            <asp:ListItem Text="Less than" Value="LTET"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Enabled="false" Text="Asset Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAssetType" Enabled="false" Width="200px" runat="server"
                                            TabIndex="5" CssClass="dropdownlist" ToolTip="Select asset type" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                                            <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="Asset Category" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAssetCategory" Width="200px" TabIndex="5" runat="server"
                                            CssClass="dropdownlist" ToolTip="Select asset category" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                            OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset category" CausesValidation="false" />
                                        <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label7" runat="server" Text="Asset Make" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:DropDownList ID="ddlAssetMake" Width="200px" TabIndex="5" runat="server" CssClass="dropdownlist"
                                            AutoPostBack="true" ToolTip="Select asset make" OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label8" runat="server" Text="Model Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:ListBox ID="lstModelName" ToolTip="Select one or more model names" SelectionMode="Multiple"
                                            Width="200px" runat="server"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ImageUrl="~/images/Submit.png"
                                            TabIndex="5" Height="35px" Width="85px" ToolTip="Get asset history report as per filters selected"
                                            OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="~/images/Reset.png"
                                            TabIndex="6" Height="35px" Width="85px" ToolTip="Refresh/reset fields" CausesValidation="false"
                                            OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left" colspan="5">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblAssetCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
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
                <td colspan="6">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvAssetHistory" runat="server" AllowPaging="True" PageSize="50"
                                    AutoGenerateColumns="False" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvAssetHistory_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("[ASSET_CODE]") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FAMS ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAMSID" runat="server" Text='<%#Eval("[ASSET_ID]") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%#Eval("[SERIAL_CODE]") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Make">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModel" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Purchased Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurDate" runat="server" Text='<%#Eval("PURCHASED_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AMC/Warranty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAMCWarranty" runat="server" Text='<%#Eval("AMC_WARRANTY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="A/W Start Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAWStDate" runat="server" Text='<%#Eval("AMC_WARRANTY_START_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="A/W End Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAWEndDate" runat="server" Text='<%#Eval("AMC_WARRANTY_END_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Process">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcess" runat="server" Text='<%#Eval("PROCESS_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDept" runat="server" Text='<%#Eval("DEPT_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Age">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYrsOld" runat="server" Text='<%#Eval("NO_OF_YRS_OLD") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAssetHistory" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right" colspan="6">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" ToolTip="Export asset history report into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
