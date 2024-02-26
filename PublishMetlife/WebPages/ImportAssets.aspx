<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WebPages/MobiVUEMaster.master" CodeFile="ImportAssets.aspx.cs" Inherits="WebPages_ImportAssets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            
        }
        function onselectionChange(evt) {
            $(".disp-none").hide();
            var ddlval = $("[id*='ddlImportType'] :selected").val();
            if (ddlval == 'ASSET') {
                $(".asset-acq").show();
            }
            else if (ddlval == 'ALLOCATE') {
                $(".asset-alloc").show();
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
    <style>
        .disp-none {
            display:none;
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
            Import/Export Data</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label19" Font-Bold="true" Text="* marked fields are mandatory. While uploading fresh assets, duplicate serial no. or IMEI no. are saved on server at C:/ATS folder."
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
                        <asp:ListItem Text="Import/Update Assets" Value="ASSET"></asp:ListItem>
                        <asp:ListItem Text="Allocate Assets" Value="ALLOCATE"></asp:ListItem>
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
                                <tr  class="disp-none asset-acq">
                                    <td>
                                        <asp:Panel ID="pnlAsset" CssClass="panel" runat="server" GroupingText="Import Assets In Bulk (In Stock only)">
                                            <table id="Table3" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                
                                                <tr>
                                                    
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
                                                    <td style="text-align: right; width: 24%">
                                                        
                                                    </td>
                                                    <td style="text-align: center; width: 2%">
                                                        <a href="DownloadSample/ASSETS_ACQUISTATION.xlsx">
                                                            <input type="button" class=" btn btn-secondary" value="Download Template" />
                                                        </a>   
                                                    </td>
                                                    <td style="text-align: left; width: 24%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr class="disp-none asset-alloc">
                                    <td>
                                        <asp:Panel ID="pnlAllocate" CssClass="panel" runat="server" GroupingText="Allocate Assets In Bulk">
                                            <table id="Table6" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                
                                                <tr>
                                                    <td style="text-align: right; width: 24%">
                                                        <asp:Label ID="Label13" runat="server" Text="Select Asset Allocation File" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center; width: 4%">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left; width: 24%">
                                                        <asp:FileUpload ID="AllocationFileUpload" ToolTip="Browse excel file through here for bulk asset allocation"
                                                            CssClass="textbox" runat="server" />
                                                    </td>
                                                    <td style="text-align: right; width: 24%">
                                                        
                                                    </td>
                                                    <td style="text-align: center; width: 2%">
                                                        <a href="DownloadSample/ASSETS_ALLOCATION.xlsx">
                                                            <input type="button" class=" btn btn-secondary" value="Download Template" />
                                                        </a>   
                                                    </td>
                                                    <td style="text-align: left; width: 24%">
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
         
        </table>
    </div>
</asp:Content>