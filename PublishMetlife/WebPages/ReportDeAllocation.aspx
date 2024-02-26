<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ReportDeAllocation.aspx.cs" Inherits="ReportAllocatedReturnable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtFromDate.ClientID%>').value = "";
            document.getElementById('<%= txtToDate.ClientID%>').value = "";
          
            document.getElementById('<%= ddlProcess.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlEmployee.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetMake.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= lstModelName.ClientID%>').selectedIndex = -1;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= rdoAllocationDate.ClientID%>').checked = true;
            document.getElementById('<%= rdoReturnDate.ClientID%>').checked = false;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').focus();
        }
        function GetDateType(rdoType) {
            if (rdoType.value == 'rdoAllocationDate') {
                document.getElementById('<%= rdoAllocationDate.ClientID%>').checked = true;
                document.getElementById('<%= rdoReturnDate.ClientID%>').checked = false;
            }
            else if (rdoType.value == 'rdoReturnDate') {
                document.getElementById('<%= rdoReturnDate.ClientID%>').checked = true;
                document.getElementById('<%= rdoAllocationDate.ClientID%>').checked = false;
            }
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            DE Allocation Report</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    
                                   
                                    <td style="text-align: right; width: 20%">
                                        <asp:Label ID="Label11" runat="server" Text="Select Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; width: 2%">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; width: 34%">
                                        <asp:DropDownList ID="ddlAssetCategory" Width="200px" runat="server" CssClass="dropdownlist"
                                            AutoPostBack="true" ToolTip="Select asset category" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
<%--                                        <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                            OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset category" CausesValidation="false" />--%>
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
                                <tr visible="false" style="display:none;">
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label9" runat="server" Text="Process" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:DropDownList ID="ddlProcess" ToolTip="Select process name" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlProcess_SelectedIndexChanged" Width="200px" CssClass="dropdownlist">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right; vertical-align: top">
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" CssClass="label" Text="Employee"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlEmployee" ToolTip="Select allocated employee name" runat="server"
                                            CssClass="dropdownlist" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                   
                                </tr>
                                <tr>
                                     <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" CssClass="label" Text="Date Search By"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButton ID="rdoAllocationDate" runat="server" TabIndex="27" onClick="GetDateType(this);"
                                            CssClass="label" Text=" Allocation Date" Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoReturnDate" runat="server" TabIndex="28" onClick="GetDateType(this);"
                                            CssClass="label" Text=" Return Date" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="From Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtFromDate" runat="server" onfocus="showCalendarControl(this);"
                                            Width="200px" ToolTip="Enter/select from date" CssClass="textbox"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="To Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtToDate" runat="server" onfocus="showCalendarControl(this);"
                                            Width="200px" ToolTip="Enter/select to date" CssClass="textbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                            ToolTip="Get returnable asset's list" ImageUrl="~/images/Submit.png" Height="35px"
                                            Width="85px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                            ToolTip="Reset/clear fields" ImageUrl="~/images/Reset.png" Height="35px" Width="85px"
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
                    <div id="DivGrid" runat="server">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvRptReturnable" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    OnPageIndexChanging="gvRptReturnable_PageIndexChanging" ShowFooter="false" CssClass="mGrid"
                                    PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Serial No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RFID Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptName" runat="server" Text='<%#Eval("TAG_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Asset Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblATTagName" runat="server" Text='<%#Eval("ASSET_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Allocation Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAllocDate" runat="server" Text='<%#Eval("ASSET_ALLOCATION_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Exp. Return Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpRtnDate" runat="server" Text='<%#Eval("EXPECTED_RTN_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Act. Return Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActRtnDate" runat="server" Text='<%#Eval("ACTUAL_RTN_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCatName" runat="server" Text='<%#Eval("ASSET_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Make">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetType" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetType" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAlloc" runat="server" Text='<%#Eval("STATUS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvRptReturnable" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right" colspan="4">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" ToolTip="Export allocated returnable assets' list into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
