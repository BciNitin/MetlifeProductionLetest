<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="~/WebPages/DepartmentMaster.aspx.cs" Inherits="DepartmentMaster" Title="BCIL : ATS - Department Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtDeptCode.ClientID%>').value = "";
            document.getElementById('<%= txtDeptName.ClientID%>').value = "";
            document.getElementById('<%= txtRemarks.ClientID %>').value = "";
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
            document.getElementById('<%= txtDeptCode.ClientID%>').focus();
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

    <style type="text/css">
        .style1
        {
            width: 26px;
        }
        .style2
        {
            width: 260px;
        }
        .style3
        {
            width: 270px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Department Master
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label9" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right; width:18%">
                                        <asp:Label ID="Label8" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Department Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; width:2%">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; width:40%">
                                        <asp:TextBox autocomplete="off" ID="txtDeptCode" CssClass="textbox" MaxLength="10"
                                            runat="server" ToolTip="Enter Department Code" TabIndex="1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                                            ControlToValidate="txtDeptCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z]+$" ErrorMessage="[Alphanumeric only]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td rowspan="6" align="center" valign="top" style="width: 40%">
                                        <img src="../images/Deptmaster1.jpg" style="width: 250px; height: 200px" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Department Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtDeptName" CssClass="textbox" MaxLength="25"
                                            runat="server" ToolTip="Enter Department Name" TabIndex="2"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="dynamic"
                                            ControlToValidate="txtDeptName" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[Alphanumeric only]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label6" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:TextBox autocomplete="off" ID="txtRemarks" CssClass="multitextbox" MaxLength="400"
                                            ToolTip="Enter Remarks" runat="server" TextMode="MultiLine" Width="300px" Height="80px"
                                            TabIndex="2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="Set Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:CheckBox ID="chkSetStatus" runat="server" ToolTip="Set Department Status" Text="  Active"
                                            Checked="true" CssClass="label" TabIndex="7" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:RequiredFieldValidator ID="RFVDeptCode" runat="server" ControlToValidate="txtDeptCode"
                                            ErrorMessage="[Department Code Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFVDeptName" runat="server" ControlToValidate="txtDeptName"
                                            ErrorMessage="[Department Name Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style2">
                                    </td>
                                    <td align="left" class="style1">
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" ToolTip="Save Department Details"
                                            Text="Save Department" CssClass="button" TabIndex="11" Width="120px" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" OnClientClick="ClearFields();" Text="Reset" CssClass="button"
                                            TabIndex="12" Width="60px" CausesValidation="False" ToolTip="Reset/Clear Fields" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvDeptMaster" runat="server" AllowPaging="True" OnRowDeleting="gvDeptMaster_RowDeleting"
                                    OnRowEditing="gvDeptMaster_RowEditing" OnRowUpdating="gvDeptMaster_RowUpdating"
                                    OnRowDataBound="gvDeptMaster_RowDataBound" OnPageIndexChanging="gvDeptMaster_PageIndexChanging"
                                    OnRowCancelingEdit="gvDeptMaster_RowCancelingEdit" AutoGenerateColumns="False"
                                    ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    PageSize="50">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Department Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptCode" runat="server" Text='<%#Eval("DEPT_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEDeptCode" runat="server" Text='<%#Eval("DEPT_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptName" runat="server" Text='<%#Eval("DEPT_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEDeptName" ValidationGroup="Grid" runat="server"
                                                    Text='<%#Eval("DEPT_NAME") %>' CssClass="textbox"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfveditcmpname" runat="server" ControlToValidate="txtEDeptName"
                                                    ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtERemarks" TextMode="MultiLine" ValidationGroup="Grid" runat="server"
                                                    Text='<%#Eval("REMARKS") %>' Width="250px" Height="50px" CssClass="multitextbox"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ACTIVE">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("ACTIVE") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%#Eval("ACTIVE") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CREATED_BY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECreatedBy" runat="server" Text='<%#Eval("CREATED_BY") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created On">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedOn" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECreatedOn" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="50px">
                                            <EditItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                                    ImageUrl="~/images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CommandName="Cancel" CausesValidation="false"
                                                    ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" CausesValidation="false"
                                                    ImageUrl="~/images/Edit_16x16.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" OnClientClick="return confirm('Are you sure to delete?');"
                                                    CommandName="Delete" ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvDeptMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvDeptMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvDeptMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvDeptMaster" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvDeptMaster" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export department master data into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
