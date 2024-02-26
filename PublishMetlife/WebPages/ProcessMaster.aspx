<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="ProcessMaster.aspx.cs" Inherits="ProcessMaster" Title="BCIL : ATS - Process Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtProcessCode.ClientID %>').value = "";
            document.getElementById('<%= txtProcessName.ClientID %>').value = "";
            document.getElementById('<%= ddlDepartment.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
            document.getElementById('<%= txtProcessCode.ClientID %>').focus();
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
            width: 231px;
        }
        .style3
        {
            width: 250px;
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
            Process Master</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%;" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label19" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Select Department" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlDepartment" ToolTip="Select Department Name" runat="server"
                                            TabIndex="1" CssClass="dropdownlist" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td rowspan="5" align="right" valign="top">
                                        <img src="../images/bpmIcon.jpg" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label7" runat="server" Text="Process Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtProcessCode" MaxLength="10" CssClass="textbox"
                                            Width="200px" runat="server" ToolTip="Enter Process Code" TabIndex="2"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                                            ControlToValidate="txtProcessCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="\w+" ErrorMessage="[No Space Allowed]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Process Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtProcessName" MaxLength="25" Width="200px"
                                            CssClass="textbox" runat="server" ToolTip="Enter Process Name" TabIndex="3"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="dynamic"
                                            ControlToValidate="txtProcessName" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[Alphanumeric only]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="Set Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:CheckBox ID="chkSetStatus" Checked="true" ToolTip="Set Process Status" runat="server"
                                            TabIndex="5" Text="  Active" CssClass="label" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style2">
                                    </td>
                                    <td style="text-align: center">
                                    </td>
                                    <td style="text-align: left">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlDepartment"
                                            ErrorMessage="[Department Required]" CssClass="validation" ValidationGroup="Submit"
                                            InitialValue="-- Select Department --"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_Group" runat="server" ControlToValidate="txtProcessCode"
                                            ErrorMessage="[Process Code Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProcessName"
                                            ErrorMessage="[Process Name Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" ToolTip="Save Process Details"
                                            Text="Save Process" CssClass="button" TabIndex="6" Width="100px" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" ToolTip="Reset/Clear Fields" runat="server" OnClientClick="ClearFields();"
                                            Text="Reset" CssClass="button" TabIndex="7" Width="60px" CausesValidation="False" />
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
                                <asp:GridView ID="gvProcessMaster" runat="server" AllowPaging="True" OnRowDeleting="gvProcessMaster_RowDeleting"
                                    OnRowEditing="gvProcessMaster_RowEditing" OnRowUpdating="gvProcessMaster_RowUpdating"
                                    PageSize="50" OnRowDataBound="gvProcessMaster_RowDataBound" OnRowCancelingEdit="gvProcessMaster_RowCancelingEdit"
                                    AutoGenerateColumns="False" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvProcessMaster_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Process Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcCode" runat="server" Text='<%#Eval("PROCESS_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEProcCode" Width="60px" runat="server" Text='<%#Eval("PROCESS_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptCode" runat="server" Text='<%#Eval("DEPT_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEDeptCode" runat="server" CssClass="dropdownlist">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvDC" runat="server" ControlToValidate="ddlEDeptCode"
                                                    ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid" InitialValue="-- Select Department --" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Process Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcName" runat="server" Text='<%#Eval("PROCESS_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" MaxLength="25" ID="txtEProcName" Width="150px" runat="server"
                                                    Text='<%#Eval("PROCESS_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvusername" runat="server" ControlToValidate="txtEProcName"
                                                    ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCrtdBy" runat="server" Text='<%#Eval("CREATED_BY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECrtdby" Width="60px" runat="server" Text='<%#Eval("CREATED_BY") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created On">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCrtdOn" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECrtdOn" Width="60px" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="40px">
                                            <EditItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                                    ImageUrl="~/images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CausesValidation="false"
                                                    CommandName="Cancel" ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" CausesValidation="false"
                                                    ImageUrl="~/images/Edit_16x16.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" CausesValidation="false"
                                                    OnClientClick="return confirm('Are you sure you want to delete?');" CommandName="Delete"
                                                    ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvProcessMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvProcessMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvProcessMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvProcessMaster" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvProcessMaster" EventName="RowDataBound" />
                                <asp:AsyncPostBackTrigger ControlID="gvProcessMaster" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Process Master Data Into Excel File"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
