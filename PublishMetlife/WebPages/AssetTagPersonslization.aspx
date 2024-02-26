<%@ Page Title="Asset Tag Personslization" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="AssetTagPersonslization.aspx.cs" Inherits="AssetTagPersonslization" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<script runat="server">

  
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID %>').value = "";
         <%--   document.getElementById('<%= AssetFileUpload.ClientID %>').value = "";
            document.getElementById('<%= AllocationFileUpload.ClientID %>').value = "";
            document.getElementById('<%= VendorFileUpload.ClientID %>').value = "";
            document.getElementById('<%= EmployeeFileUpload.ClientID %>').value = "";
            document.getElementById('<%= rdoAllocateFreshAssets.ClientID%>').checked = true;
            document.getElementById('<%= rdoAllocateExistingAssets.ClientID%>').checked = false;
            document.getElementById('<%= ddlImportType.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= rdoUploadAsset.ClientID %>').checked = true;
            document.getElementById('<%= rdoUpdateAsset.ClientID %>').checked = false;
            document.getElementById('<%= rdoUpdateSerialNo.ClientID%>').checked = false;
            document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;--%>
            document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
          <%--  document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;
            document.getElementById('<%= ddlImportType.ClientID %>').focus();--%>
        }
        function onselectionChange(evt) {
            var ddlval = $("[id*='ddlImportType'] :selected").val();
            if (ddlval == 'ASSET') {
               <%-- document.getElementById('<%= pnlAsset.ClientID %>').disabled = false;
                document.getElementById('<%= pnlAllocate.ClientID %>').disabled = true;--%>
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
               <%-- document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;--%>
            }
            else if (ddlval == 'ALLOCATE') {
              <%--  document.getElementById('<%= pnlAllocate.ClientID %>').disabled = false;--%>
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
               <%-- document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;
                document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;--%>
            }
            else if (ddlval == 'VENDOR') {
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = false;
            <%--    document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;
                document.getElementById('<%= pnlAllocate.ClientID %>').disabled = true;
                document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;--%>
            }
            else if (ddlval == 'EMPLOYEE') {
              <%--  document.getElementById('<%= pnlEmployee.ClientID %>').disabled = false;
                document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;
                document.getElementById('<%= pnlAllocate.ClientID %>').disabled = true;--%>
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
            }
            else if (ddlval == 'SELECT') {
               <%-- document.getElementById('<%= pnlEmployee.ClientID %>').disabled = true;
                document.getElementById('<%= pnlAsset.ClientID %>').disabled = true;--%>
                document.getElementById('<%= pnlVendor.ClientID %>').disabled = true;
                d<%--ocument.getElementById('<%= pnlAllocate.ClientID %>').disabled = true;--%>
            }
}
<%--function GetAssetUploadType(rdoAssetType) {
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
<%--}--%>
<%--function GetAssetAllocateType(rdoAllocateType) {
    if (rdoAllocateType.value == 'rdoAllocateFreshAssets') {
        document.getElementById('<%= rdoAllocateFreshAssets.ClientID%>').checked = true;
                document.getElementById('<%= rdoAllocateExistingAssets.ClientID%>').checked = false;
            }
            else if (rdoAllocateType.value == 'rdoAllocateExistingAssets') {
                document.getElementById('<%= rdoAllocateExistingAssets.ClientID%>').checked = true;
                document.getElementById('<%= rdoAllocateFreshAssets.ClientID%>').checked = false;
            }
    }--%>--%>
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
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: right; width: 48%">
                    &nbsp;</td>
                <td colspan="2" style="text-align: center; width: 4%">
                    &nbsp;</td>
                <td style="text-align: left; width: 48%">
                
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
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlVendor" CssClass="panel" runat="server" GroupingText="Import Excel Data">
                                            <table id="Table4" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                <tr>
                                                    <td style="text-align: right; width: 48%">
                                                        <asp:Label ID="Label2" runat="server" Text="Select Excel  File" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center; width: 4%">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left; width: 48%">
                                                        <asp:FileUpload ID="VendorFileUpload" ToolTip="Browse excel file through here for vendor data import"
                                                            CssClass="textbox" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
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
                                            Height="35px" Width="85px" OnClick="btnSubmit_Click" />
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
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 48%">
                    &nbsp;</td>
                <td colspan="2" style="text-align: center; width: 4%">
                    <div style="font-weight: bold">
                        </div>
                </td>
                <td style="text-align: left; width: 48%">
                    &nbsp;</td>
            </tr>
        </table>
    </div>
</asp:Content>
