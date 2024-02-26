<%@ Page Title="BCIL : ATS - Asset Allocation Report" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ReportAssetAllocation.aspx.cs" Inherits="ReportAssetAllocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtFromDate.ClientID%>').value = "";
            document.getElementById('<%= txtToDate.ClientID%>').value = "";
          
            document.getElementById('<%= ddlEmployee.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetMake.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= lstModelName.ClientID%>').selectedIndex = -1;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
            
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

        function GetAssetUploadType(rdoAssetType) {
            if (rdoAssetType.value == 'rdoITAsset') {
                document.getElementById('<%= rdoITAsset.ClientID%>').checked = true;
                document.getElementById('<%= rdoFacilityAsset.ClientID%>').checked = false;

            }
            else if (rdoAssetType.value == 'rdoFacilityAsset') {
                document.getElementById('<%= rdoFacilityAsset.ClientID%>').checked = true;
                document.getElementById('<%= rdoITAsset.ClientID%>').checked = false;

            }
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
            Asset Allocation Report</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <%--<tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="Label8" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Text="Allocation Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlAllocRtrn" Width="200px" runat="server" CssClass="dropdownlist"
                                            TabIndex="1" ToolTip="Select allocation type">
                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="IT" Value="ALLOCATED"></asp:ListItem>
                                            <asp:ListItem Text="Returned" Value="RETURNED"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddlAllocRtrn"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="Select Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAssetLocation" runat="server" OnSelectedIndexChanged="ddlAssetLocation_SelectedIndexChanged"
                                            Width="200px" CssClass="dropdownlist" ToolTip="Select allocated asset's location"
                                            AutoPostBack="true" TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="btnRefreshLocation" runat="server" CausesValidation="false"
                                            ImageUrl="../images/Refresh_16x16.png" ToolTip="Refresh/reset location" OnClick="btnRefreshLocation_Click" />
                                        <asp:Label ID="lblLocCode" CssClass="label" Text="0" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>--%>
                                   <tr>
                                                  
                                                    <td style="text-align: right; width: 24%">
                                                        <asp:RadioButton ID="rdoITAsset" Text="IT Assets" Checked="true"
                                                            CssClass="label" onClick="GetAssetUploadType(this);" runat="server" />
                                                    </td>
                                                     <td style="text-align: center; width: 2%">
                                                        <div style="font-weight: bold">
                                                           </div>
                                                    </td>
                                                    <td style="text-align: right; width: 24%">
                                                        <asp:RadioButton ID="rdoFacilityAsset" Text="Facility  Assets" onClick="GetAssetUploadType(this);"
                                                            CssClass="label" runat="server" />
                                                    </td>
                                                     <td style="text-align: right; width: 24%">
                                                      </td>
                                                     <td style="text-align: right; width: 24%">
                                                      </td>
                                                    <td style="text-align: right; width: 2%">
                                                      </td>
                                                </tr>
                                <tr>
                              
                                   <%-- <td style="text-align: right;">
                                        <asp:Label ID="Label7" runat="server" Enabled="false" Text="Asset Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlAssetType" Enabled="false" Width="200px" TabIndex="3" runat="server"
                                            AutoPostBack="true" ToolTip="Select asset type" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged"
                                            CssClass="dropdownlist">
                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                                            <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>--%>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label11" runat="server" Text="Asset Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAssetCategory" TabIndex="4" Width="200px" runat="server"
                                            CssClass="dropdownlist" AutoPostBack="true" ToolTip="Select asset's Type" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                      <%--  <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                           ToolTip="Refresh/reset category" CausesValidation="false" />--%>
                                        <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                </tr>

                             <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label4" runat="server" Text="Asset Make" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:DropDownList ID="ddlAssetMake" ToolTip="Select asset make" Width="200px" TabIndex="5"
                                            runat="server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged">
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
                                            Width="200px" runat="server" TabIndex="6"></asp:ListBox>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label9" runat="server" Text="Process" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:DropDownList ID="ddlProcess" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProcess_SelectedIndexChanged"
                                            Width="200px" CssClass="dropdownlist" ToolTip="Select allocated process name" TabIndex="7">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right; vertical-align: top">
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" CssClass="label" Text="Employee"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlEmployee" ToolTip="Select allocated employee name" TabIndex="8"
                                            runat="server" CssClass="dropdownlist" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                    </td>
                                    <td style="text-align: center">
                                    </td>
                                    <td style="text-align: left">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="From Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtFromDate"  style="margin-left:0px;" runat="server" onfocus="showCalendarControl(this);"
                                            Width="200px" CssClass="textbox" ToolTip="Enter/select from allocation date"
                                            TabIndex="9"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="To Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" style="margin-left:0px;" ID="txtToDate" runat="server" onfocus="showCalendarControl(this);"
                                            Width="200px" CssClass="textbox" ToolTip="Enter/select to allocation date" TabIndex="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="11"
                                            ToolTip="Get asset allocation report based on filters" ImageUrl="~/images/Submit.png"
                                            Height="35px" Width="85px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="12" OnClientClick="ClearFields();"
                                            ToolTip="Reset/Clear fields" ImageUrl="~/images/Reset.png" Height="35px" Width="85px"
                                            CausesValidation="false" OnClick="btnClear_Click" />
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
                    <div id="DivGrid" runat="server" style="max-width:900px; overflow-x:auto;">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvRptAssetAllocation" runat="server" AllowPaging="True" AutoGenerateColumns="true"
                                    OnPageIndexChanging="gvRptAssetAllocation_PageIndexChanging" ShowFooter="false"
                                    CssClass="mGrid" PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvRptAssetAllocation" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>

            
            <tr>
                <td style="text-align: right" colspan="4">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" ToolTip="Export asset allocation report into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
