<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="PageMaster.aspx.cs" Inherits="PageMaster" Title="BCIL : ATS - Page Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtPageName.ClientID%>').value = "";
            document.getElementById('<%= txtPageDesc.ClientID%>').value = "";
            document.getElementById('<%= ddlGroup.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= rdoView.ClientID %>').checked = false;
            document.getElementById('<%= rdoSave.ClientID %>').checked = false;
            document.getElementById('<%= rdoEdit.ClientID %>').checked = false;
            document.getElementById('<%= rdoDelete.ClientID %>').checked = false;
            document.getElementById('<%= rdoExport.ClientID %>').checked = false;
        }
        //        function GetRightsValue(rdoRights)
        //        {
        //            if (rdoRights.value == 'rdoView')
        //            {
        //                if (document.getElementById('<%= rdoView.ClientID%>').checked==true)
        //                    document.getElementById('<%= rdoView.ClientID%>').checked=false;
        //            }
        //            if (rdoRights.value == 'rdoSave')
        //            {
        //                if (document.getElementById('<%= rdoSave.ClientID%>').checked==true)
        //                    document.getElementById('<%= rdoSave.ClientID%>').checked=false;
        //            }
        //            if (rdoRights.value == 'rdoEdit')
        //            {
        //                if (document.getElementById('<%= rdoEdit.ClientID%>').checked==true)
        //                    document.getElementById('<%= rdoEdit.ClientID%>').checked=false;
        //            }
        //            if (rdoRights.value == 'rdoDelete')
        //            {
        //                if (document.getElementById('<%= rdoDelete.ClientID%>').checked==true)
        //                    document.getElementById('<%= rdoDelete.ClientID%>').checked=false;
        //            }
        //            if (rdoRights.value == 'rdoExport')
        //            {
        //                if (document.getElementById('<%= rdoExport.ClientID%>').checked==true)
        //                    document.getElementById('<%= rdoExport.ClientID%>').checked=false;
        //            }
        //        }
        function ShowErrMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Duplicate Page Name cannot be saved.';
        }
        function ShowPageNotDeletedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Page Master details are not deleted.';
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
            Page Master</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="17" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="Page Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtPageName" CssClass="textbox" ToolTip="Enter Page Name" runat="server"
                                            Width="200px" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVGroupCode" runat="server" ControlToValidate="txtPageName"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="Page Description" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtPageDesc" CssClass="textbox" ToolTip="Enter Page Description"
                                            runat="server" Width="200px" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPageDesc"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="Group" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlGroup" CssClass="dropdownlist" runat="server" Width="200px" ToolTip="Select Group Name"
                                            ValidationGroup="Submit" TabIndex="1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_Group" runat="server" ControlToValidate="ddlGroup"
                                            InitialValue="SELECT" ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" runat="server" Text="Set Rights" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButton ID="rdoView" runat="server" ToolTip="Set Group Rights For This Page"
                                            onClick="GetRightsValue(this);" CssClass="label" Text=" View" Checked="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoSave" runat="server" ToolTip="Set Group Rights For This Page"
                                            onClick="GetRightsValue(this);" CssClass="label" Text=" Save" Checked="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoEdit" runat="server" onClick="GetRightsValue(this);" ToolTip="Set Group Rights For This Page"
                                            CssClass="label" Text=" Edit" Checked="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoDelete" runat="server" onClick="GetRightsValue(this);" ToolTip="Set Group Rights For This Page"
                                            CssClass="label" Text=" Delete" Checked="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoExport" runat="server" onClick="GetRightsValue(this);" ToolTip="Set Group Rights For This Page"
                                            CssClass="label" Text=" Export" Checked="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                    </td>
                                    <td style="text-align: center">
                                    </td>
                                    <td style="text-align: left">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ImageUrl="~/images/Submit.png"
                                            TabIndex="11" Height="35px" Width="85px" ToolTip="Submit" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="~/images/Reset.png"
                                            TabIndex="12" Height="35px" Width="85px" ToolTip="Reset/Clear" CausesValidation="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblErrorMsg" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:UpdatePanel ID="upGrid" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvPageMaster" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                OnRowDeleting="gvPageMaster_RowDeleting" OnRowEditing="gvPageMaster_RowEditing"
                                OnPageIndexChanging="gvPageMaster_PageIndexChanging" OnRowCancelingEdit="gvPageMaster_RowCancelingEdit"
                                OnRowUpdating="gvPageMaster_RowUpdating" OnRowDataBound="gvPageMaster_RowDataBound"
                                ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:TemplateField HeaderText="Page Code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPageCode" runat="server" Text='<%#Eval("PAGE_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEPageCode" runat="server" Text='<%#Eval("PAGE_CODE") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Page Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPageName" runat="server" Text='<%#Eval("PAGE_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEPageName" Width="200px" runat="server" Text='<%#Eval("PAGE_NAME") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Page Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPageDesc" runat="server" Text='<%#Eval("PAGE_DESCRIPTION") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEPageDesc" Width="200px" runat="server" Text='<%#Eval("PAGE_DESCRIPTION") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Group">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGroup" runat="server" Text='<%#Eval("GROUP_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEGroup" runat="server" AutoPostBack="true" CssClass="dropdownlist">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEGroup" runat="server" ControlToValidate="ddlEGroup"
                                                InitialValue="SELECT" Text="*" ValidationGroup="Grid" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkView" Enabled="false" runat="server" Checked='<%#Eval("VIEW_RIGHTS") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEView" Enabled="false" runat="server" Checked='<%#Eval("VIEW_RIGHTS") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Save">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSave" Enabled="false" runat="server" Checked='<%#Eval("SAVE_RIGHTS") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkESave" Enabled="false" runat="server" Checked='<%#Eval("SAVE_RIGHTS") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkEdit" Enabled="false" runat="server" Checked='<%#Eval("EDIT_RIGHTS") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEEdit" Enabled="false" runat="server" Checked='<%#Eval("EDIT_RIGHTS") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDelete" Enabled="false" runat="server" Checked='<%#Eval("DELETE_RIGHTS") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEDelete" Enabled="false" runat="server" Checked='<%#Eval("DELETE_RIGHTS") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Export">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkExport" Enabled="false" runat="server" Checked='<%#Eval("EXPORT_RIGHTS") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEExport" Enabled="false" runat="server" Checked='<%#Eval("EXPORT_RIGHTS") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" CausesValidation="false"
                                                ImageUrl="~/images/Edit_16x16.png" runat="server" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" CausesValidation="false"
                                                OnClientClick="return confirm('Are you sure you want to delete?');" CommandName="Delete"
                                                ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                                ImageUrl="~/images/Update_icon.png" runat="server" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CausesValidation="false"
                                                CommandName="Cancel" ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pgr"></PagerStyle>
                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvPageMaster" EventName="RowDeleting" />
                            <asp:AsyncPostBackTrigger ControlID="gvPageMaster" EventName="RowUpdating" />
                            <asp:AsyncPostBackTrigger ControlID="gvPageMaster" EventName="RowEditing" />
                            <asp:AsyncPostBackTrigger ControlID="gvPageMaster" EventName="RowCancelingEdit" />
                            <asp:AsyncPostBackTrigger ControlID="gvPageMaster" EventName="PageIndexChanging" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
