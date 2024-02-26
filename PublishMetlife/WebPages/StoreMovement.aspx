<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="StoreMovement.aspx.cs" Inherits="WebPages_StoreMovement" Title="ATS - Store Movement In and Out" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>
 
    <script language="javascript" type="text/javascript">
        var TotalChkBx;
        var Counter;

        function ClearFields() {
            window.location.href = window.location;
        }
        function ShowAlert(ShowMsg) {
            alert(ShowMsg.toString());
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>
   
   
    <style type="text/css">
        .style1 {
            width: 113px;
        }

        .style2 {
            width: 110px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/WebPages/AutoComplete.asmx" />
        </Services>
    </act:ToolkitScriptManager>
    <div id="wrapper">
        <div id="pageTitle">
            Store Movement In & Out
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table2" runat="server" cellspacing="10" style="width: 100%" align="center">
           
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table3" runat="server" cellspacing="15" style="width: 100%;" align="center">
                                 <tr>
                <td colspan="4" style="text-align: left">
                    <asp:Label ID="lblMsg" Font-Bold="true" Text="" CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                     <asp:Label ID="lblComp" Font-Bold="true" Text="" CssClass="ErrorLabel" Visible="false"
                        runat="server"></asp:Label>
                     <asp:Label ID="lblStore" Font-Bold="true" Text="" CssClass="ErrorLabel" Visible="false"
                        runat="server"></asp:Label>
                     <asp:Label ID="lblRefreshTime" Font-Bold="true" Text="" CssClass="ErrorLabel" Visible="false"
                        runat="server"></asp:Label>
                </td>
            </tr>
                                <tr>
                                    <td colspan="4">
                                         
                                        <asp:Timer ID="StoreTimer" runat="server" OnTick="StoreTimer_Tick"></asp:Timer>
                                        <div style="overflow: auto">
                                            <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                ShowFooter="false" CssClass="mGrid" Visible="true"
                                                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" PageSize="20">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="30px">
                                                        <HeaderStyle Width="30px" />
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imagebuttonDelete" OnClientClick="return confirm('Are you sure to delete?');"
                                                                ImageUrl="~/images/Delete_16x16.png" runat="server" AutoPostBack="true" OnClick="imagebuttonDelete_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Scan Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Scanned Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScannedDateTime" runat="server" Text='<%#Eval("ScannedDateTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Site Location">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsite" runat="server" Text='<%#Eval("SITE_CODE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Floor">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblfloor" runat="server" Text='<%#Eval("FLOOR_CODE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbllocation" runat="server" Text='<%#Eval("STORE_CODE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset RFID Tag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetTag" runat="server" Text='<%#Eval("TAG_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Asset Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetType" runat="server" Text='<%#Eval("ASSET_TYPE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Asset Make">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetMake" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   
                                                    <asp:TemplateField HeaderText="Asset Model">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetModel" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Serial Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSerialCode" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Far Tag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetFarTag" runat="server" Text='<%#Eval("ASSET_FAR_TAG") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Asset Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetStatus" runat="server" Text='<%#Eval("ASSET_STATUS") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Sub Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetSubStatus" runat="server" Text='<%#Eval("ASSET_SUB_STATUS") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Employee Tag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmployeeTag" runat="server" Text='<%#Eval("EmployeeTag") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Employee Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:Label>
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
                                    <td>
                                        <asp:Label runat="server" ID="lblErrorMSg"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="StoreTimer" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
 