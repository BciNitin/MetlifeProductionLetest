<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="KioskMovement.aspx.cs" 
    Inherits="WebPages_KioskMovement" Title="ATS - Kiosk Movement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script language="javascript" type="text/javascript">
       function ClearFields() {
           document.getElementById('<%= lblErrMsg.ClientID%>').value = "";
           document.getElementById('<%= lblAssetMake.ClientID%>').value = "";
           document.getElementById('<%= lblAssetModel.ClientID%>').value = "";
           document.getElementById('<%= lblAssetStatus.ClientID%>').value = "";
           document.getElementById('<%= lblAssetSubStatus.ClientID%>').value = "";
           document.getElementById('<%= lblAssetType.ClientID%>').value = "";
           document.getElementById('<%= lblEmployeeTag.ClientID%>').value = "";
           document.getElementById('<%= lblEmpName.ClientID%>').value = "";
           document.getElementById('<%= lblLocation.ClientID%>').value = "";
           document.getElementById('<%= lblRFIDTag.ClientID%>').value = "";
           document.getElementById('<%= lblSerialNo.ClientID%>').value = "";
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to execute this operation!');
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>
     <%--<script type="text/javascript">
         setTimeout(function () { debugger; window.location.reload(true); }, <%= (Convert.ToInt32(ConfigurationManager.AppSettings["KioskRefreshTime"])*1000) %> );
     </script>--%>
    <style type="text/css">
        .style1
        {
            width: 26px;
        }
        .style2
        {
            width: 250px;
        }
        .style3
        {
            width: 270px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Kiosk Movement</div>
    </div>
      <div id="wrapper1">
           <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                          
                                
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%;height:100%" align="center">
             <tr>
                <td colspan="4" style="text-align: left">
                    <asp:Label ID="lblMsg" Font-Bold="true" Text="" CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                    <asp:Label ID="lblKioskComp" Font-Bold="true" Text="" CssClass="ErrorLabel" Visible="false"
                        runat="server"></asp:Label>
                     <asp:Label ID="lblKiosk" Font-Bold="true" Text="" CssClass="ErrorLabel" Visible="false"
                        runat="server"></asp:Label>
                     <asp:Label ID="lblKioskRefreshTime" Font-Bold="true" Text="" CssClass="ErrorLabel" Visible="false"
                        runat="server"></asp:Label>
                </td>
            </tr>
             <tr>
                 
                  <td style="text-align: left">
                       <asp:Timer ID="KioskTimer" runat="server" OnTick="KioskTimer_Tick"></asp:Timer>
                      <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Location : " CssClass="label"></asp:Label>
                  </td>
                  
                  <td style="text-align: left">
                      <asp:Label ID="lblLocation" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
            <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Employee Tag : " CssClass="label"></asp:Label>
                  </td>
                  
                  <td style="text-align: left">
                      <asp:Label ID="lblEmployeeTag" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
               <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label9" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Employee Name : " CssClass="label"></asp:Label>
                 </td>
                  <td style="text-align: left">
                      <asp:Label ID="lblEmpName" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
            <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label3" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Asset Type : " CssClass="label"></asp:Label>
                  </td>
                  <td style="text-align: left">
                      <asp:Label ID="lblAssetType" Font-Bold="true" Font-Size="XX-Large" runat="server" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
                  <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label4" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Asset Make : " CssClass="label"></asp:Label>
                  </td>
                  <td style="text-align: left">
                      <asp:Label ID="lblAssetMake" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
              <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label5" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Asset Model : " CssClass="label"></asp:Label>
                  </td>
                  <td style="text-align: left">
                      <asp:Label ID="lblAssetModel" Font-Bold="true" Font-Size="XX-Large" runat="server" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
                    <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label6" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Serial No. : " CssClass="label"></asp:Label>
                  </td>
                  <td style="text-align: left">
                      <asp:Label ID="lblSerialNo" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
                 <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label7" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="RFID Tag : " CssClass="label"></asp:Label>
                  </td>
                  <td style="text-align: left">
                      <asp:Label ID="lblRFIDTag" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
            <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label8" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Asset Sub Status : " CssClass="label"></asp:Label>
                  </td>
                  <td style="text-align: left">
                      <asp:Label ID="lblAssetSubStatus" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
            <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label10" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Asset Status : " CssClass="label"></asp:Label>
                  </td>
                  <td style="text-align: left">
                      <asp:Label ID="lblAssetStatus" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
            <tr>
                  <td style="text-align: left">
                      <asp:Label ID="Label11" runat="server" Font-Bold="true" Font-Size="XX-Large" Text="Scan Status : " CssClass="label"></asp:Label>
                  </td>
                  <td style="text-align: left">
                      <asp:Label ID="lblScanStatus" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="XX-Large" Text="" CssClass="label"></asp:Label>
                  </td>
                 </tr>
            <tr cellspacing="500" >
                  <td style="text-align: left">
                      <asp:Label ID="lblErrMsg" runat="server" Font-Bold="true" Font-Size="XX-Large" ForeColor="Red" Text="" CssClass="label"></asp:Label>
                  </td>
            </tr>
       </table>

                            </ContentTemplate>
                 <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="KioskTimer" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
    </div>
</asp:Content>

