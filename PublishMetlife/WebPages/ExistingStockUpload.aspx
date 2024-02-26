<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="ExistingStockUpload.aspx.cs" Inherits="ExistingStockUpload" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script>
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                    <asp:Label ID="Label19" Font-Bold="true" Text=" All fields are mandatory. While uploading fresh assets, duplicate serial no. are saved on server at C:/ATS folder."
                        CssClass="ErrorLabel" runat="server"></asp:Label>
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
                                        <asp:Panel ID="pnlAsset" CssClass="panel" runat="server" GroupingText="Import Assets Acquisition">
                                            <table id="Table3" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                <tr>
                                                  
                                                    <td style="text-align: right; width: 24%">
                                                        <asp:RadioButton ID="rdoITAsset" Text="  IT Assets" Checked="true"
                                                            CssClass="label" onClick="GetAssetUploadType(this);" runat="server" />
                                                    </td>
                                                     <td style="text-align: center; width: 2%">
                                                        <div style="font-weight: bold">
                                                           </div>
                                                    </td>
                                                    <td style="text-align: right; width: 24%">
                                                        <asp:RadioButton ID="rdoFacilityAsset" Text="  Facility  Assets" onClick="GetAssetUploadType(this);"
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
                                                          <a href="DownloadSample/IT_ASSETS.xlsx" class="textbox">Download IT Assets Format</a>
                                                      </td>
                                                     <td style="text-align: right; width: 24%">
                                                          <a href="DownloadSample/FACILITY_ASSET.xlsx" class="textbox">Download  Facility Assets</a>
                                                      </td>
                                                    <td style="text-align: right; width: 2%">
                                                      </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                              
                                
                                
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="btnUpload" runat="server" ToolTip="Upload selected excel file"
                                            ImageUrl="~/images/Upload_32x32.png" OnClick="btnUpload_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" Enabled="false" ValidationGroup="Submit"
                                            TabIndex="12" ToolTip="Save uploaded excel file data" ImageUrl="~/images/Submit.png"
                                            Height="35px" Width="85px" OnClick="btnSubmit_Click"  />
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

