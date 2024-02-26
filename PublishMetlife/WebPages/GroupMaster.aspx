<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="GroupMaster.aspx.cs" Inherits="GroupMaster" Title="BCIL : ATS - Group Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtGroupCode.ClientID %>').value = "";
            document.getElementById('<%= txtGroupCode.ClientID %>').value = "";
            document.getElementById('<%= txtGroupName.ClientID %>').value = "";
            document.getElementById('<%= txtRemarks.ClientID %>').value = "";
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
            document.getElementById('<%= txtGroupCode.ClientID %>').focus();
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
            Group Master</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label19" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr >
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Group Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtGroupCode" CssClass="textbox" MaxLength="50"
                                            ToolTip="Enter Group Code" runat="server" Width="200px" TabIndex="1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                                            ControlToValidate="txtGroupCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="\w+" ErrorMessage="[Alphanumeric only]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td rowspan="6" valign="top" align="center">
                                        <img src="../images/GroupMaster.jpg" style="width: 200px; height: 200px;" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Group Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtGroupName" CssClass="textbox" MaxLength="50"
                                            ToolTip="Enter Group Name" runat="server" Width="200px" TabIndex="2"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="dynamic"
                                            ControlToValidate="txtGroupName" ValidationGroup="Submit" CssClass="validation"
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
                                        <asp:TextBox autocomplete="off" ID="txtRemarks" CssClass="multitextbox" MaxLength="100"
                                            runat="server" ToolTip="Enter Remarks" TextMode="MultiLine" Width="300px" Height="50px"
                                            TabIndex="2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="Set Group Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:CheckBox ID="chkSetStatus" runat="server" Text="  Active" Checked="true" ToolTip="Set Status"
                                            CssClass="label" TabIndex="7" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:RequiredFieldValidator ID="RFVGroupCode" runat="server" ControlToValidate="txtGroupCode"
                                            ErrorMessage="[Group Code Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFVGroupName" runat="server" ControlToValidate="txtGroupName"
                                            ErrorMessage="[Group Name Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style2">
                                    </td>
                                    <td align="left" class="style1">
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" ToolTip="Save Group Details"
                                            Text="Save Group" CssClass="button" TabIndex="11" Width="100px" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" OnClientClick="ClearFields();" Text="Reset" CssClass="button"
                                            TabIndex="12" Width="60px" ToolTip="Reset/Clear Fields" CausesValidation="False" />
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
                                <asp:GridView ID="gvGroupMaster" runat="server" AllowPaging="True" OnRowDeleting="gvGroupMaster_RowDeleting"
                                    OnRowEditing="gvGroupMaster_RowEditing" OnRowUpdating="gvGroupMaster_RowUpdating"
                                    OnRowCancelingEdit="gvGroupMaster_RowCancelingEdit" AutoGenerateColumns="False"
                                    OnRowDataBound="gvGroupMaster_RowDataBound" OnPageIndexChanging="gvGroupMaster_PageIndexChanging"
                                    ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    PageSize="50">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Group Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupCode" runat="server" Text='<%#Eval("GROUP_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEditGroupCode" runat="server" Text='<%#Eval("GROUP_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("GROUP_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEditGroupName" MaxLength="25" runat="server"
                                                    Text='<%#Eval("GROUP_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfveditgrpname" runat="server" ControlToValidate="txtEditGroupName"
                                                    Text="*" ValidationGroup="Grid"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtERemarks" TextMode="MultiLine" Width="250px"
                                                    Height="50px" MaxLength="400" runat="server" Text='<%#Eval("REMARKS") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Active Status">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" Enabled="false" runat="server" Checked='<%#Eval("ACTIVE") %>' />
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
                                        <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="40px">
                                            <EditItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                                    ImageUrl="../images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CausesValidation="false"
                                                    CommandName="Cancel" ImageUrl="../images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" CausesValidation="false"
                                                    ImageUrl="../images/Edit_16x16.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" CausesValidation="false"
                                                    OnClientClick="return confirm('Are you sure you want to delete?');" CommandName="Delete"
                                                    ImageUrl="../images/Delete_16x16.png" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvGroupMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvGroupMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvGroupMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvGroupMaster" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvGroupMaster" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Group Master data Into Excel File"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
