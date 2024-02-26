<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ReportCallLog.aspx.cs" Inherits="ReportCallLog" %>

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
            document.getElementById('<%= ddlCallNo.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlCallLogCode.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlCallStatus.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlVendor.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtCallDateFrom.ClientID%>').value = "";
            document.getElementById('<%= txtCallDateTo.ClientID%>').value = "";
            document.getElementById('<%= rdoRepair.ClientID%>').checked = false;
            document.getElementById('<%= rdoReplaced.ClientID%>').checked = false;
            document.getElementById('<%= ddlVendor.ClientID%>').focus();
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = "Please Note : You are not authorised to execute this operation!";
        }
        function GetRepairReplaced(rdbType) {
            if (rdbType.value == 'rdoRepair') {
                document.getElementById('<%= rdoRepair.ClientID%>').checked = true;
                document.getElementById('<%= rdoReplaced.ClientID%>').checked = false;
            }
            else if (rdbType.value == 'rdoReplaced') {
                document.getElementById('<%= rdoReplaced.ClientID%>').checked = true;
                document.getElementById('<%= rdoRepair.ClientID%>').checked = false;
            }
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 160px;
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
            Vendor Call Log Report</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="15" style="width: 100%;" align="center">
            <tr>
                <td colspan="6">
                    <asp:UpdatePanel ID="upSubmit" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="Select Vendor" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlVendor" runat="server" ToolTip="Select Vendor Name" AutoPostBack="true"
                                            CssClass="dropdownlist" Width="200px" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="Call Log No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlCallLogCode" runat="server" ToolTip="Select vendor call log no."
                                            CssClass="dropdownlist" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label15" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Text="Call No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlCallNo" runat="server" ToolTip="Select vendor call no."
                                            CssClass="dropdownlist" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label7" runat="server" Text="Call Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlCallStatus" runat="server" ToolTip="Select call log status"
                                            CssClass="dropdownlist" Width="200px">
                                            <asp:ListItem Text="-- Select Status --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="PENDING"></asp:ListItem>
                                            <asp:ListItem Text="Resolved" Value="RESOLVED"></asp:ListItem>
                                            <asp:ListItem Text="Un-Resolved" Value="UNRESOLVED"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="Call Date From" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtCallDateFrom" CssClass="textbox" runat="server"
                                            ToolTip="Enter/select call date from" onfocus="showCalendarControl(this);" Width="200px"
                                            TabIndex="1" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="Call Date To" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtCallDateTo" CssClass="textbox" runat="server"
                                            onfocus="showCalendarControl(this);" Width="200px" TabIndex="1" ToolTip="Enter/select call date to"
                                            MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Enabled="false" Text="Asset Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAssetType" Enabled="false" Width="200px" runat="server"
                                            TabIndex="5" CssClass="dropdownlist" AutoPostBack="True" ToolTip="Select asset type"
                                            OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                                            <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" runat="server" Text="Asset Category" CssClass="label"></asp:Label>
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
                                        <asp:Label ID="Label9" runat="server" Text="Asset Make" CssClass="label"></asp:Label>
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
                                        <asp:Label ID="Label10" runat="server" Text="Model Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td rowspan="2" style="text-align: left; vertical-align: top">
                                        <asp:ListBox ID="lstModelName" ToolTip="Select one or more model names" SelectionMode="Multiple"
                                            Width="200px" runat="server"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style1">
                                        <asp:Label ID="Label11" runat="server" Text="Part Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButton ID="rdoRepair" runat="server" TabIndex="27" onClick="GetRepairReplaced(this);"
                                            CssClass="label" Text=" Repair" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoReplaced" runat="server" TabIndex="28" onClick="GetRepairReplaced(this);"
                                            CssClass="label" Text=" Replaced" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ImageUrl="~/images/Submit.png"
                                            TabIndex="5" ToolTip="Get vendor call log report" Height="35px" Width="85px"
                                            OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="~/images/Reset.png"
                                            TabIndex="6" Height="35px" ToolTip="Reset/clear fields" Width="85px" CausesValidation="false"
                                            OnClick="btnClear_Click" />
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
                <td colspan="6">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvCallLog" runat="server" AllowPaging="True" PageSize="50" AutoGenerateColumns="False"
                                    ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    OnPageIndexChanging="gvCallLog_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAMSID" runat="server" Text='<%#Eval("VENDOR_LOCATION") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Call No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%#Eval("CALL_NO") %>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="Engr. Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("ENGINEER_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurDate" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Call Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAMCWarranty" runat="server" Text='<%#Eval("CALL_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Responded Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAWStDate" runat="server" Text='<%#Eval("RESPONDED_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Resolved Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAWEndDate" runat="server" Text='<%#Eval("RESOLVED_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcess" runat="server" Text='<%#Eval("RESOLVED_STATUS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Resolution Days">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcess" runat="server" Text='<%#Eval("RESOLUTION_DAYS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDept" runat="server" Text='<%#Eval("PART_STATUS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FRU No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYrsOld" runat="server" Text='<%#Eval("FRU_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GatePass no.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYrsOld" runat="server" Text='<%#Eval("GATEPASS_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYrsOld" runat="server" Text='<%#Eval("VENDOR_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvCallLog" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right" colspan="6">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" ToolTip="Export vendor call log report into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
