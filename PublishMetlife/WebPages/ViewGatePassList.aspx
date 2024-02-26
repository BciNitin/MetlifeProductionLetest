<%@ Page Title="BCIL : ATS - View Gatepass List" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ViewGatePassList.aspx.cs" Inherits="ViewGatePassList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= ddlGatePassType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlGatePassNo.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlGPLocation.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= rdoApproved.ClientID%>').checked = true;
            document.getElementById('<%= rdoUnApproved.ClientID%>').checked = false;
            document.getElementById('<%= ddlGatePassType.ClientID%>').focus();
        }
        function ApproveUnapprove(rdoType) {
            if (rdoType.value == 'rdoApproved') {
                document.getElementById('<%= rdoApproved.ClientID%>').checked = true;
                document.getElementById('<%= rdoUnApproved.ClientID%>').checked = false;
            }
            else if (rdoType.value == 'rdoUnApproved') {
                document.getElementById('<%= rdoUnApproved.ClientID%>').checked = true;
                document.getElementById('<%= rdoApproved.ClientID%>').checked = false;
            }
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function noBack() {
            window.history.forward(1);
        }
        var TotalChkBx;
        var Counter;
        window.onload = function() {
            TotalChkBx = parseInt('<%= this.gvGatePassList.Rows.Count %>');
            Counter = 0;
        }
        function HeaderViewClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.gvGatePassList.ClientID %>');
            var TargetChildControl = "chkApprove";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }
        function ChildClick(CheckBox, HCheckBox) {
            TotalChkBx = parseInt('<%= this.gvGatePassList.Rows.Count %>');
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            View Gate Pass List
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="Gate Pass Type" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold;">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlGatePassType" runat="server" CssClass="dropdownlist" Width="200px"
                        TabIndex="1" ToolTip="Select gate pass type">
                        <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                        <asp:ListItem Text="Returnable" Value="RETURNABLE"></asp:ListItem>
                        <asp:ListItem Text="Not Returnable" Value="NOTRETURNABLE"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="Gate Pass No." CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlGatePassNo" runat="server" CssClass="dropdownlist" Width="200px"
                        TabIndex="2" ToolTip="Select gate pass no.">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label15" runat="server" CssClass="label" Text="Select Location"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold;">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlGPLocation" Width="200px" runat="server" AutoPostBack="true"
                        TabIndex="3" ToolTip="Select gate pass location" OnSelectedIndexChanged="ddlGPLocation_SelectedIndexChanged"
                        CssClass="dropdownlist">
                    </asp:DropDownList>
                    <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                    <asp:ImageButton ID="btnRefreshLocation" runat="server" TabIndex="4" CausesValidation="false"
                        ToolTip="Refresh/reset location" ImageUrl="../images/Refresh_16x16.png" OnClick="btnRefreshLocation_Click" />
                    <asp:Label ID="lblLocCode" CssClass="label" Text="0" runat="server" Visible="false"></asp:Label>
                </td>
                <td style="text-align: right">
                </td>
                <td style="text-align: center">
                </td>
                <td style="text-align: left" visible="false">
                    <asp:RadioButton ID="rdoApproved" runat="server" TabIndex="5" onClick="ApproveUnapprove(this);"
                        CssClass="label" Text=" Approved" ToolTip="Select to get approved gate passes"
                        Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoUnApproved" ToolTip="Select to get unapproved gate passes"
                        runat="server" TabIndex="6" onClick="ApproveUnapprove(this);" CssClass="label"
                        Text=" UnApproved" />
                </td>
            </tr>
            <tr visible="false">
                <td style="text-align: right">
                    <asp:Label ID="Label4" Text="Approved GatePasses" runat="server" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:Label ID="lblApprovedGPCount" runat="server" Text="0" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label5" Text="UnApproved GatePasses" runat="server" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:Label ID="lblUnApprovedGPCount" runat="server" Text="0" CssClass="label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6" align="center">
                    <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="7"
                        ToolTip="View gatepass list" ImageUrl="~/images/Submit.png" Height="35px" Width="85px"
                        OnClick="btnSubmit_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btnClear" runat="server" TabIndex="8" OnClientClick="ClearFields();"
                        ToolTip="Refresh/reset fields" ImageUrl="~/images/Reset.png" Height="35px" Width="85px"
                        CausesValidation="False" OnClick="btnClear_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="5" style="text-align: left">
                    <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblRecordCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:UpdatePanel ID="upGrid" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvGatePassList" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                OnPageIndexChanging="gvGatePassList_PageIndexChanging" PageSize="40" OnRowCommand="gvGatePassList_RowCommand"
                                ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" 
                                AlternatingRowStyle-CssClass="alt" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Gatepass Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGPCode" runat="server" Text='<%#Eval("GATEPASS_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gatepass Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGPDate" runat="server" Text='<%#Eval("GATEPASS_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGPType" runat="server" Text='<%#Eval("GATEPASS_TYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocCode" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDestLocCode" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                  <%--  <asp:TemplateField HeaderText="Vendor">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVendName" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Employee">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bearer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBearerName" runat="server" Text='<%#Eval("GATEPASS_BEARER_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imagebuttonView" ToolTip="View" CommandName="View" ImageUrl="../images/View_16x16.png"
                                                runat="server" CommandArgument='<%#Eval("GATEPASS_CODE") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                </Columns>
                                <PagerStyle CssClass="pgr"></PagerStyle>
                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvGatePassList" EventName="PageIndexChanging" />
                            <asp:AsyncPostBackTrigger ControlID="gvGatePassList" EventName="SelectedIndexChanged" />
                
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    <asp:ImageButton ID="btnApprove" runat="server" ImageUrl="~/images/Submit.png" TabIndex="9"
                        Height="35px" Visible="false" Width="85px" ToolTip="Approve gate passes" OnClick="btnApprove_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
