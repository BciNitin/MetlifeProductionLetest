<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="EmployeeMaster.aspx.cs" Inherits="EmployeeMaster" Title="BCIL : ATS - Employee Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= txtEmployeeCode.ClientID%>').value = "";
            document.getElementById('<%= txtEmployeeName.ClientID%>').value = "";
            document.getElementById('<%= ddlProcess.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlReportingTo.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtEmailID.ClientID%>').value = "";
            document.getElementById('<%= txtDateofJoin.ClientID%>').value = "";
            document.getElementById('<%= txtPhone.ClientID%>').value = "";
            document.getElementById('<%= txtRemarks.ClientID %>').value = "";
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
            document.getElementById('<%= lblErrorMsg.ClientID %>').innerHTML = '';
            document.getElementById('<%= txtEmployeeCode.ClientID%>').focus();
        }
        function ShowSelfRptToMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowErrMsg(Message) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to execute this operation!');
        }
        function Validate() {
            var x = document.getElementById('<%= txtPhone.ClientID%>').value;
            if (isNaN(x) || x.indexOf(" ") != -1) {
                alert("Enter numeric value");
                document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Enter Numeric Value for Mobile No.';
                return false;
            }
            if (x.length < 10) {
                alert("Enter Min. 10 Digits");
                document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Enter minimum 10 digits';
                return false;
            }
            if (x.charAt(0) != "9" || x.charAt(0) != "8" || x.charAt(0) != "7") {
                alert("it should start with 7,8 or 9");
                return false
            }
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
            Employee Master
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label11" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
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
                                        <asp:Label ID="Label1" runat="server" Text="Employee Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtEmployeeCode" CssClass="textbox" MaxLength="10"
                                            runat="server" ToolTip="Enter Employee Code" Width="200px" TabIndex="1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                                            ControlToValidate="txtEmployeeCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[Alphanumeric]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Employee Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtEmployeeName" CssClass="textbox" MaxLength="50"
                                            runat="server" ToolTip="Enter Employee Name" Width="200px" TabIndex="2"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="dynamic"
                                            ControlToValidate="txtEmployeeName" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[Alphanumeric]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label14" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label5" runat="server" Text="Process Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlProcess" CssClass="dropdownlist" style="margin-left:15px;" ToolTip="Select Process Name"
                                            runat="server" Width="200px" TabIndex="4">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="Reporting To" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlReportingTo" CssClass="dropdownlist" style="margin-left:15px;" ToolTip="Select Reporting Manager Name"
                                            runat="server" Width="200px" TabIndex="5">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label10" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label7" runat="server" Text="E-Mail ID" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtEmailID" CssClass="textbox" MaxLength="50"
                                            runat="server" ToolTip="Enter Employee's E-mail Address" Width="200px" TabIndex="6">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REP_Email" runat="server" ControlToValidate="txtEmailID"
                                            ErrorMessage="[ Invalid E-Mail ]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                            CssClass="validation">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label9" runat="server" Text="Date Of Joining" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtDateofJoin" CssClass="textbox" TabIndex="7"
                                            ToolTip="Enter Employee's Date Of Joining" runat="server" onfocus="showCalendarControl(this);"
                                            Width="200px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label12" runat="server" Text="Set Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:CheckBox ID="chkSetStatus" runat="server" Text="  Active" ToolTip="Set Employee's Status"
                                            Checked="true" CssClass="label" TabIndex="10" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" MaxLength="10" Text="Mobile No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtPhone" CssClass="textbox" ValidationGroup="Submit"
                                            MaxLength="10" runat="server" Width="200px" ToolTip="Enter Employee's Mobile No."
                                            TabIndex="9">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="rfvPhoneNo" runat="server" ControlToValidate="txtPhone"
                                            ErrorMessage="[Numeric Only]" ValidationExpression="^([123456789]{1})([0-9]{1})([0-9]{8})$"
                                            ValidationGroup="Submit" CssClass="validation">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label13" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td colspan="4" style="text-align: left" valign="top">
                                        <asp:TextBox autocomplete="off" ID="txtRemarks" CssClass="multitextbox" MaxLength="400"
                                            ToolTip="Enter Remarks" ValidationGroup="Submit" TextMode="MultiLine" TabIndex="11"
                                            runat="server" Height="60px" Width="676px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:RequiredFieldValidator ID="RFV_EmpCode" CssClass="validation" runat="server"
                                            ControlToValidate="txtEmployeeCode" ErrorMessage="[Employee Code Required]" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_EmpName" CssClass="validation" runat="server"
                                            ControlToValidate="txtEmployeeName" ErrorMessage="[Employee Name Required]" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_EMail" runat="server" ControlToValidate="txtEmailID"
                                            ErrorMessage="[Employee E-Mail Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlProcess"
                                            ErrorMessage="[Process Name Required]" CssClass="validation" ValidationGroup="Submit"
                                            InitialValue="-- Select Process --"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                            ToolTip="Save Employee Details" Text="Save Employee" CssClass="button"
                                            Width="110px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                            ToolTip="Reset/Clear Fields" Text="Reset" CssClass="button" Width="60px"
                                            CausesValidation="false" />
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
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvEmpMaster" runat="server" AllowPaging="True" OnRowDeleting="gvEmpMaster_RowDeleting"
                                    OnRowEditing="gvEmpMaster_RowEditing" OnRowUpdating="gvEmpMaster_RowUpdating"
                                    OnRowCancelingEdit="gvEmpMaster_RowCancelingEdit" AutoGenerateColumns="False"
                                    OnRowDataBound="gvEmpMaster_RowDataBound" ShowFooter="false" CssClass="mGrid"
                                    PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvEmpMaster_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Employee Code" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("EMPLOYEE_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEEmpCode" runat="server" Text='<%#Eval("EMPLOYEE_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEEmpName" CssClass="textbox" Width="100px"
                                                    ValidationGroup="Grid" MaxLength="50" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEEmpName" runat="server" ControlToValidate="txtEEmpName"
                                                    CssClass="validation" ErrorMessage="[Required]" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Process">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProc" runat="server" Text='<%#Eval("PROCESS_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEProc" runat="server" CssClass="dropdownlist">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvEProc" runat="server" ControlToValidate="ddlEProc"
                                                    InitialValue="-- Select Process --" CssClass="validation" ErrorMessage="[Required]"
                                                    ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reporting To">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReptTo" runat="server" Text='<%#Eval("EMP_REPORTING_TO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEReptTo" runat="server" CssClass="dropdownlist">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhone" runat="server" Text='<%#Eval("EMP_PHONE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEPhone" CssClass="textbox" Width="100px" ValidationGroup="Grid"
                                                    runat="server" MaxLength="10" Text='<%#Eval("EMP_PHONE") %>'></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="rfvEPhoneNo" runat="server" ControlToValidate="txtEPhone"
                                                    ErrorMessage="[Numeric Only]" ValidationExpression="^([6789]{1})([0-9]{1})([0-9]{8})$"
                                                    ValidationGroup="Grid" CssClass="validation">
                                                </asp:RegularExpressionValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="E-Mail">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("EMP_EMAIL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEEMail" CssClass="textbox" Width="200px" ValidationGroup="Grid"
                                                    runat="server" MaxLength="50" Text='<%#Eval("EMP_EMAIL") %>'></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="REP_EEmail" runat="server" ControlToValidate="txtEEMail"
                                                    ErrorMessage="[Invalid E-Mail]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                                    CssClass="validation" ValidationGroup="Grid">
                                                </asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="rfvEEMail" runat="server" ControlToValidate="txtEEMail"
                                                    Text="[Required]" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created On">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCON" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECON" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="Edit/Delete">
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
                                <asp:AsyncPostBackTrigger ControlID="gvEmpMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvEmpMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvEmpMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvEmpMaster" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvEmpMaster" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Employee Master Data Into Excel File"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
