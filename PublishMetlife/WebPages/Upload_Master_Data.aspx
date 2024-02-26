<%@ Page Title="Master Data" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="Upload_Master_Data.aspx.cs" Inherits="WebPages_Upload_Master_Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID %>').value = "";
            document.getElementById('<%= AssetFileUpload.ClientID %>').value = "";
            document.getElementById('<%= AllocationFileUpload.ClientID %>').value = "";
            document.getElementById('<%= VendorFileUpload.ClientID %>').value = "";
            document.getElementById('<%= EmployeeFileUpload.ClientID %>').value = "";
            document.getElementById('<%= rdoAllocateFreshAssets.ClientID%>').checked = true;
            document.getElementById('<%= rdoAllocateExistingAssets.ClientID%>').checked = false;
            document.getElementById('<%= ddlImportType.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= rdoUploadAsset.ClientID %>').checked = true;
            document.getElementById('<%= rdoUpdateAsset.ClientID %>').checked = false;
            document.getElementById('<%= rdoUpdateSerialNo.ClientID%>').checked = false;
            document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;
            document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
            document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;
            document.getElementById('<%= ddlImportType.ClientID %>').focus();
        }
        function onselectionChange(evt) {
            var ddlval = $("[id*='ddlImportType'] :selected").val();
            if (ddlval == 'ASSET') {
                document.getElementById('<%= pnlAsset.ClientID %>').disabled = false;
                document.getElementById('<%= pnlAllocate.ClientID %>').disabled = true;
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
                document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;
            }
            else if (ddlval == 'ALLOCATE') {
                document.getElementById('<%= pnlAllocate.ClientID %>').disabled = false;
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
                document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;
                document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;
            }
            else if (ddlval == 'VENDOR') {
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = false;
                document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;
                document.getElementById('<%= pnlAllocate.ClientID %>').disabled = true;
                document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;
            }
            else if (ddlval == 'EMPLOYEE') {
                document.getElementById('<%= pnlEmployee.ClientID %>').disabled = false;
                document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;
                document.getElementById('<%= pnlAllocate.ClientID %>').disabled = true;
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
            }
            else if (ddlval == 'SELECT') {
                document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;
                document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
                document.getElementById('<%= pnlAllocate.ClientID %>').disabled = true;
            }
        }
        function GetAssetUploadType(rdoAssetType) {
            if (rdoAssetType.value == 'rdoUploadAsset') {
                document.getElementById('<%= rdoUploadAsset.ClientID%>').checked = true;
                document.getElementById('<%= rdoUpdateAsset.ClientID%>').checked = false;
                document.getElementById('<%= rdoUpdateSerialNo.ClientID%>').checked = false;
            }
            else if (rdoAssetType.value == 'rdoUpdateAsset') {
                document.getElementById('<%= rdoUpdateAsset.ClientID%>').checked = true;
                document.getElementById('<%= rdoUploadAsset.ClientID%>').checked = false;
                document.getElementById('<%= rdoUpdateSerialNo.ClientID%>').checked = false;
            }
            else if (rdoAssetType.value == 'rdoUpdateSerialNo') {
                document.getElementById('<%= rdoUpdateSerialNo.ClientID%>').checked = true;
                document.getElementById('<%= rdoUpdateAsset.ClientID%>').checked = false;
                document.getElementById('<%= rdoUploadAsset.ClientID%>').checked = false;
            }
        }
        function GetAssetAllocateType(rdoAllocateType) {
            if (rdoAllocateType.value == 'rdoAllocateFreshAssets') {
                document.getElementById('<%= rdoAllocateFreshAssets.ClientID%>').checked = true;
                document.getElementById('<%= rdoAllocateExistingAssets.ClientID%>').checked = false;
            }
            else if (rdoAllocateType.value == 'rdoAllocateExistingAssets') {
                document.getElementById('<%= rdoAllocateExistingAssets.ClientID%>').checked = true;
                document.getElementById('<%= rdoAllocateFreshAssets.ClientID%>').checked = false;
            }
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowAlert(ShowMsg) {
            alert(ShowMsg.toString());
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Import Data</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label19" Font-Bold="true" Text=""
                        CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 48%">
                    <asp:Label ID="Label9" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                    <asp:Label ID="Label3" runat="server" Text="Select Operation Type" CssClass="label"></asp:Label>
                </td>
                <td colspan="2" style="text-align: center; width: 4%">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left; width: 48%">
                    <asp:DropDownList ID="ddlImportType" Width="200px" ValidationGroup="Submit" runat="server"
                        CssClass="dropdownlist" ToolTip="Select data import type">
                        <asp:ListItem Text="-- Select Import Type --" Value="SELECT"></asp:ListItem>
                        <asp:ListItem Text="Import Vendors" Value="VENDOR"></asp:ListItem>
                     <%--   <asp:ListItem Text="Import Employees" Value="EMPLOYEE"></asp:ListItem>--%>
                        <asp:ListItem Text="Import User" Value="USER"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlImportType"
                        ErrorMessage="[Select Operation Type]" CssClass="validation" ValidationGroup="Submit"
                        InitialValue="SELECT"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upSubmit"
                        DisplayAfter="0">
                        <ProgressTemplate>
                            <div id="IMGDIV" align="center" valign="middle" runat="server" style="position: absolute;
                                left: 50%; top: 50%; right: 50%; bottom: 50%; visibility: visible; vertical-align: middle;
                                background-color: White">
                                <img alt="Processing..." src="../images/updateprogress.gif" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr visible="false" style="display:none;"> 
                                    <td>
                                        <asp:Panel ID="pnlAsset" CssClass="panel" runat="server" GroupingText="Import Assets In Bulk (In Stock only)">
                                            <table id="Table3" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                <tr>
                                                    <td style="text-align: right; width: 24%">
                                                    </td>
                                                    <td style="text-align: center; width: 2%">
                                                    </td>
                                                    <td style="text-align: right; width: 24%">
                                                        <asp:RadioButton ID="rdoUploadAsset" Text="  Fresh Upload Assets" Checked="true"
                                                            CssClass="label" onClick="GetAssetUploadType(this);" runat="server" />
                                                    </td>
                                                    <td style="text-align: right; width: 24%">
                                                        <asp:RadioButton ID="rdoUpdateAsset" Text="  Update Existing Assets" onClick="GetAssetUploadType(this);"
                                                            CssClass="label" runat="server" />
                                                    </td>
                                                    <td style="text-align: center; width: 2%">
                                                    </td>
                                                    <td style="text-align: left; width: 24%">
                                                        <asp:RadioButton ID="rdoUpdateSerialNo" Text="  Update Asset Serial No." onClick="GetAssetUploadType(this);"
                                                            CssClass="label" Enabled="false" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right; width: 24%">
                                                        <%--<asp:Label ID="Label13" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>--%>
                                                        <asp:Label ID="Label7" runat="server" Text="Asset Type" Enabled="false" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center; width: 2%">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left; width: 24%">
                                                        <asp:DropDownList ID="ddlAssetType" Width="200px" runat="server" Enabled="false"
                                                            CssClass="dropdownlist" ToolTip="Select asset type">
                                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                                            <asp:ListItem Text="Admin Asset" Value="ADMIN"></asp:ListItem>
                                                            <asp:ListItem Text="IT Asset" Value="IT"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="text-align: right; width: 24%">
                                                        <asp:Label ID="Label5" runat="server" Text="Select Asset Data File" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center; width: 2%">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left; width: 24%">
                                                        <asp:FileUpload ID="AssetFileUpload" ToolTip="Browse excel file through here for asset data import"
                                                            CssClass="textbox" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr visible="false" style="display:none;">
                                    <td >
                                        <asp:Panel ID="pnlAllocate" CssClass="panel" runat="server" GroupingText="Allocate Assets In Bulk">
                                            <table id="Table6" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                <tr>
                                                    <td style="text-align: right; width: 48%">
                                                        <asp:RadioButton ID="rdoAllocateFreshAssets" Text="  Allocate Fresh Assets" Checked="true"
                                                            CssClass="label" onClick="GetAssetAllocateType(this);" runat="server" />
                                                    </td>
                                                    <td style="text-align: center; width: 4%">
                                                    </td>
                                                    <td style="text-align: left; width: 48%">
                                                        <asp:RadioButton ID="rdoAllocateExistingAssets" Text="  Allocate Existing Assets"
                                                            onClick="GetAssetAllocateType(this);" CssClass="label" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right; width: 48%">
                                                        <asp:Label ID="Label13" runat="server" Text="Select Asset Allocation File" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center; width: 4%">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left; width: 48%">
                                                        <asp:FileUpload ID="AllocationFileUpload" ToolTip="Browse excel file through here for bulk asset allocation"
                                                            CssClass="textbox" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlVendor" CssClass="panel" runat="server" GroupingText="Import Vendor Data">
                                            <table id="Table4" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                <tr>
                                                    <td style="text-align: right; width: 35%">
                                                        <asp:Label ID="Label2" runat="server" Text="Select Vendor Data File" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center; width: 4%">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left; width: 35%">
                                                        <asp:FileUpload ID="VendorFileUpload" ToolTip="Browse excel file through here for vendor data import"
                                                            CssClass="textbox" runat="server" />
                                                    </td>
                                                     <td style="text-align: left; width: 26%">
                                                       <a href="DownloadSample/vendormaster.xlsx" class="textbox">Download Format</a>
                                                    </td>
                                                </tr>
                                              
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr style="display:none;" visible="false" >
                                    <td>
                                        <asp:Panel ID="pnlEmployee" CssClass="panel" runat="server" GroupingText="Import Employee Data">
                                            <table id="Table5" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                <tr>
                                                    <td style="text-align: right; width: 48%">
                                                        <asp:Label ID="Label1" runat="server" Text="Select Employee Data File" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center; width: 4%">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left; width: 48%">
                                                        <asp:FileUpload ID="EmployeeFileUpload" ToolTip="Browse excel file through here for employee data import"
                                                            CssClass="textbox" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel2" CssClass="panel" runat="server" GroupingText="Import User Data">
                                            <table id="Table8" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                <tr>
                                                    <td style="text-align: right; width: 35%">
                                                        <asp:Label ID="Label11" runat="server" Text="Select User Data File" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center; width: 4%">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left; width: 35%">
                                                        <asp:FileUpload ID="FileUploadUser" ToolTip="Browse excel file through here for user data import"
                                                            CssClass="textbox" runat="server" />
                                                    </td>
                                                    <td style="text-align: left; width: 26%">
                                                       <a href="DownloadSample/UserMaster.xlsx" class="textbox">Download Format</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="btnUpload" runat="server" ValidationGroup="Submit" ToolTip="Upload selected excel file"
                                            ImageUrl="~/images/Upload_32x32.png" OnClick="btnUpload_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" Enabled="false" ValidationGroup="Submit"
                                            TabIndex="12" ToolTip="Save uploaded excel file data" ImageUrl="~/images/Submit.png"
                                            Height="35px" Width="75px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                            ToolTip="Clear/reset fields" ImageUrl="~/images/Reset.png" Height="35px" Width="85px"
                                            CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnUpload" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr visible="false" style="display:none;">
                <td colspan="4">
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" CssClass="panel" runat="server" GroupingText="Export Asset Data">
                                <table id="Table7" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                    <tr>
                                        <td style="text-align: right; width: 48%">
                                            <asp:Label ID="Label6" CssClass="label" runat="server" Enabled="false" Text="Select Asset Type"></asp:Label>
                                        </td>
                                        <td style="text-align: center; width: 4%">
                                            <div style="font-weight: bold">
                                            :
                                        </td>
                                        <td style="text-align: left; width: 48%">
                                            <asp:DropDownList ID="ddlAssetType2" Width="200px" runat="server" Enabled="false"
                                                ToolTip="Select asset type" TabIndex="1" CssClass="dropdownlist" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlAssetType2_SelectedIndexChanged">
                                                <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                                <asp:ListItem Text="ADMIN Asset" Value="ADMIN"></asp:ListItem>
                                                <asp:ListItem Text="IT Asset" Value="IT"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 48%">
                                            <asp:Label ID="Label4" runat="server" Text="Asset Category" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: center; width: 4%">
                                            <div style="font-weight: bold">
                                                :</div>
                                        </td>
                                        <td style="text-align: left; width: 100%">
                                            <asp:DropDownList ID="ddlAssetCategory" Width="200px" TabIndex="2" runat="server"
                                                ToolTip="Select asset category type" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                            <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                                OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset category" CausesValidation="false" />
                                            <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 48%">
                                            <asp:Label ID="Label10" runat="server" Text="Asset Make" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: center; width: 4%">
                                            <div style="font-weight: bold">
                                                :</div>
                                        </td>
                                        <td style="text-align: left; width: 100%">
                                            <asp:DropDownList ID="ddlAssetMake" Width="200px" TabIndex="3" runat="server" CssClass="dropdownlist"
                                                AutoPostBack="true" ToolTip="Select asset make" OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 48%" valign="top">
                                            <asp:Label ID="Label12" runat="server" Text="Model Name" CssClass="label"></asp:Label>
                                        </td>
                                        <td style="text-align: center; width: 4%" valign="top">
                                            <div style="font-weight: bold">
                                                :</div>
                                        </td>
                                        <td style="text-align: left; width: 100%">
                                            <asp:ListBox ID="lstModelName" TabIndex="4" ToolTip="Select one or more model names"
                                                SelectionMode="Multiple" Width="200px" CssClass="textbox" runat="server"></asp:ListBox>
                                            &nbsp;
                                            <asp:Button ID="btnGetExpAsset" Text="Get Asset Data" CssClass="button" runat="server"
                                                OnClick="btnGetExpAsset_Click" Width="120px" ToolTip="Get assets' details for being exported" />&nbsp;
                                            <asp:Button ID="btnReset" runat="server" TabIndex="5" CssClass="button" Text="Reset"
                                                CausesValidation="false" Width="70px" ToolTip="Reset/clear fields" OnClick="btnReset_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr visible="false" style="display:none;">
                <td style="text-align: right; width: 48%">
                    <asp:Label ID="Label8" CssClass="label" runat="server" Text="Export Asset Data"></asp:Label>
                </td>
                <td colspan="2" style="text-align: center; width: 4%">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left; width: 48%">
                    <asp:ImageButton ID="btnExportAsset" runat="server" TabIndex="6" ToolTip="Export asset data into excel file for update"
                        ImageUrl="~/images/Excel-icon (2).png" OnClick="btnExportAsset_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>


