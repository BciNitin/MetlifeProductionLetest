<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="ViewAMC.aspx.cs" Inherits="ViewAMC" Title="BCIL : ATS - AMC List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= ddlVendor.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= rdoAllAMC.ClientID%>').checked = true;
            document.getElementById('<%= rdoActiveAMC.ClientID%>').checked = false;
            document.getElementById('<%= rdoExpiredAMC.ClientID%>').checked = false;
            document.getElementById('<%= ddlVendor.ClientID%>').focus();
        }
        function getCheckedValue(radioObj) {
            if (radioObj.value == 'rdoAllAMC') {
                document.getElementById('<%= rdoAllAMC.ClientID%>').checked = true;
                document.getElementById('<%= rdoActiveAMC.ClientID%>').checked = false;
                document.getElementById('<%= rdoExpiredAMC.ClientID%>').checked = false;
            }
            if (radioObj.value == 'rdoActiveAMC') {
                document.getElementById('<%= rdoAllAMC.ClientID%>').checked = false;
                document.getElementById('<%= rdoActiveAMC.ClientID%>').checked = true;
                document.getElementById('<%= rdoExpiredAMC.ClientID%>').checked = false;
            }
            if (radioObj.value == 'rdoExpiredAMC') {
                document.getElementById('<%= rdoAllAMC.ClientID%>').checked = false;
                document.getElementById('<%= rdoActiveAMC.ClientID%>').checked = false;
                document.getElementById('<%= rdoExpiredAMC.ClientID%>').checked = true;
            }
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = "Please Note : You are not authorised to execute this operation!";
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
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
            View Asset AMC
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
            <tr>
                <td style="text-align: right; width: 37%">
                    <asp:Label ID="Label3" runat="server" Text="Select Type" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center; width: 3%">
                    <div style="font-weight: bold;">
                        :</div>
                </td>
                <td style="text-align: left; width: 60%">
                    <asp:RadioButton ID="rdoAllAMC" CssClass="label" Text="   ALL AMC" onClick="getCheckedValue(this);"
                        Checked="true" runat="server" ToolTip="Select to get a list of all AMC's" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoActiveAMC" ToolTip="Select to get a list of active AMC's"
                        CssClass="label" Text="   ACTIVE AMC" onClick="getCheckedValue(this);" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoExpiredAMC" CssClass="label" ToolTip="Select to get a list of expired AMC's"
                        Text="   EXPIRED AMC" onClick="getCheckedValue(this);" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="Select Vendor" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold;">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlVendor" ToolTip="Select vendor name" runat="server" CssClass="dropdownlist"
                        Width="400px" TabIndex="1">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="6"
                        ToolTip="Get a list of assets based on search criteria" ImageUrl="~/images/Submit.png"
                        Height="35px" Width="85px" OnClick="btnSubmit_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btnClear" runat="server" TabIndex="7" OnClientClick="ClearFields();"
                        ToolTip="Reset/clear fields" ImageUrl="~/images/Reset.png" Height="35px" Width="85px"
                        CausesValidation="False" OnClick="btnClear_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export assets' AMC list into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: left">
                    <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: right">
                    <asp:Label ID="lblAssetCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvViewAMC" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    OnPageIndexChanging="gvViewAMC_PageIndexChanging" OnRowCommand="gvViewAMC_RowCommand"
                                    ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" PageSize="100"
                                    AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="AMC Start Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStDate" runat="server" Text='<%#Eval("AMC_WARRANTY_START_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AMC End Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("AMC_WARRANTY_END_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrlCode" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPONo" runat="server" Text='<%#Eval("PO_NUMBER") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocCode" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Make">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetMake" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModel" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModel" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViewAMC" EventName="PageIndexChanging" />
                                <asp:AsyncPostBackTrigger ControlID="gvViewAMC" EventName="RowCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
