<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="CompanyMaster.aspx.cs" Inherits="CompanyMaster" Title="BCIL : ATS - Company Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtCompCode.ClientID %>').value = "";
            document.getElementById('<%= txtCompName.ClientID %>').value = "";
            document.getElementById('<%= txtRemarks.ClientID %>').value = "";
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
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
            width: 250px;
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
            Company Master
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label4" Font-Bold="true" CssClass="ErrorLabel" Text="Imp. Note : Deleting a company would delete entire details of that company from the database."
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right; width: 17%">
                                        <asp:Label ID="Label1" runat="server" Text="Company Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; width: 3%">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; width: 40%">
                                        <asp:TextBox ID="txtCompCode" autocomplete="off" Enabled="false" CssClass="textbox"
                                            runat="server" ToolTip="Enter Company Code" Width="200px" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVCompCode" runat="server" ControlToValidate="txtCompCode"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REV_CompCode" runat="server" Display="dynamic"
                                            ControlToValidate="txtCompCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^([\S\s]{0,2})$" ErrorMessage="[2 characters only]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td rowspan="6" align="center" valign="top" style="width: 40%">
                                        <img src="../images/CompanyMaster.jpg" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="Company Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtCompName" Enabled="false" autocomplete="off" CssClass="textbox"
                                            runat="server" Width="200px" ToolTip="Enter Company Name" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVCompName" runat="server" ControlToValidate="txtCompName"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                                            ControlToValidate="txtCompName" ValidationGroup="Submit" CssClass="validation"
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
                                        <asp:TextBox ID="txtRemarks" Enabled="false" autocomplete="off" CssClass="multitextbox"
                                            runat="server" ToolTip="Enter Remarks" TextMode="MultiLine" Width="200px" Height="50px"
                                            TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVRemarks" runat="server" ControlToValidate="txtRemarks"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
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
                                        <asp:CheckBox ID="chkSetStatus" Enabled="false" runat="server" Text="  Active" ToolTip="Set Status"
                                            Checked="true" CssClass="label" TabIndex="7" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style2">
                                    </td>
                                    <td align="left">
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btnSubmit" Enabled="false" runat="server" ValidationGroup="Submit"
                                            Text="Save Company" CssClass="button" TabIndex="11" ToolTip="Submit" Width="100px"
                                            OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" Enabled="false" runat="server" OnClientClick="ClearFields();"
                                            Text="Reset" CssClass="button" TabIndex="12" Width="60px" CausesValidation="False"
                                            ToolTip="Reset/Clear" />
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
                                <asp:GridView ID="gvCompMaster" runat="server" AllowPaging="True" OnRowDeleting="gvCompMaster_RowDeleting"
                                    OnRowEditing="gvCompMaster_RowEditing" OnRowUpdating="gvCompMaster_RowUpdating"
                                    OnRowCancelingEdit="gvCompMaster_RowCancelingEdit" AutoGenerateColumns="False"
                                    OnRowDataBound="gvCompMaster_RowDataBound" PageSize="50" OnPageIndexChanging="gvCompMaster_PageIndexChanging"
                                    ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Company Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompCode" runat="server" Text='<%#Eval("COMP_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEditCompCode" runat="server" Text='<%#Eval("COMP_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompName" runat="server" Text='<%#Eval("COMP_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditCompName" autocomplete="off" ValidationGroup="Grid" runat="server"
                                                    Text='<%#Eval("COMP_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfveditcmpname" runat="server" ControlToValidate="txtEditCompName"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtERemarks" autocomplete="off" CssClass="multitextbox" runat="server"
                                                    ToolTip="Enter Remarks" Text='<%#Eval("REMARKS") %>' TextMode="MultiLine" Width="200px"
                                                    Height="50px" TabIndex="2"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEditRemarks" runat="server" ControlToValidate="txtERemarks"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ACTIVE">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("ACTIVE") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditActive" runat="server" Enabled="true" Checked='<%#Eval("ACTIVE") %>' />
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
                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="50px">
                                            <EditItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                                    ImageUrl="~/images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CommandName="Cancel" CausesValidation="false"
                                                    ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" CausesValidation="false"
                                                    ImageUrl="~/images/Edit_16x16.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" OnClientClick="return confirm('Are you sure to delete company?');"
                                                    CommandName="Delete" ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvCompMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvCompMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvCompMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvCompMaster" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvCompMaster" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
