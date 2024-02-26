<%@ Page  Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="TagMaster.aspx.cs" Inherits="TagMaster" Title="BCIL : ATS - Tag Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<script runat="server">

   
    
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
           <%--  document.getElementById('<%= txtLocationCode.ClientID %>').value = "";--%>
          <%--  document.getElementById('<%= txtLocationName.ClientID %>').value = "";--%>
           <%-- document.getElementById('<%= txtLocationDesc.ClientID %>').value = "";--%>
        <%-- document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;--%>
           <%--    document.getElementById('<%= txtParentLocName.ClientID %>').value = "";--%>
             <%--document.getElementById('<%= txtLocationCode.ClientID %>').focus();--%>
        }
        function ShowErrMsg(Message) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowLevelMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You cannot add the location under this parent location, the selected parent location is level 6.';
        }
        function ShowLocNotDeletedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You cannot delete this location, since it has child locations mapped.';
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to execute this operation!');
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 26px;
        }
        .style2
        {
            width: 225px;
        }
        .style3
        {
            width: 260px;
        }
        .auto-style1 {
            width: 225px;
            height: 28px;
        }
        .auto-style2 {
            height: 28px;
        }
        .auto-style3 {
            width: 225px;
            height: 32px;
        }
        .auto-style4 {
            height: 32px;
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
            Tag Master</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label19" Font-Bold="true" Text="* marked fields are mandatory." Visible="false" CssClass="ErrorLabel"
                        runat="server"></asp:Label>
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
                                   <asp:Panel ID="pnlEmployee" CssClass="panel" runat="server" GroupingText="Import Tag Master">
                                            <table id="Table5" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                                <tr>
                                                    <td style="text-align: right; width: 48%">
                                                        <asp:Label ID="Label1" runat="server" Text="Select Tag Master Data File" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center; width: 4%">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left; width: 48%">
                                                        <asp:FileUpload ID="TagMasterFileUpload" ToolTip="Browse excel file through here for Tag master data import"
                                                            CssClass="textbox" runat="server" />
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
                                    <td >
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnUpload" />
                             <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="Td1" colspan="4" runat="server">
                    <div id="DivGrid" runat="server">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvRptAssetStock" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    OnPageIndexChanging="gvRptAssetStock_PageIndexChanging" ShowFooter="false" CssClass="mGrid"
                                    PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Tag Serial No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompName" runat="server" Text='<%#Eval("SerialNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Active">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("Active") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="Upload By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAlloc" runat="server" Text='<%#Eval("UploadedBy") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Upload On">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptName" runat="server" Text='<%#Eval("UploadedOn") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvRptAssetStock" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
           
           
        </table>
    </div>
</asp:Content>
