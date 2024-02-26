<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="CategoryMaster.aspx.cs" Inherits="CategoryMaster" Title="BCIL : ATS - Category Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= rdoAdminAsset.ClientID%>').checked = false;
            document.getElementById('<%= rdoITAsset.ClientID%>').checked = true;
            document.getElementById('<%= txtCategoryCode.ClientID %>').value = "";
            document.getElementById('<%= txtCategoryName.ClientID %>').value = "";
            document.getElementById('<%= txtCategoryRemarks.ClientID %>').value = "";
            document.getElementById('<%= ddlParentCategory.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
            document.getElementById('<%= txtCategoryCode.ClientID %>').focus();
        }
        function GetAssetType(rdoAssetType) {
            if (rdoAssetType.value == 'rdoAdminAsset') {
                document.getElementById('<%= rdoAdminAsset.ClientID%>').checked = true;
                document.getElementById('<%= rdoITAsset.ClientID%>').checked = false;
            }
            else if (rdoAssetType.value == 'rdoITAsset') {
                document.getElementById('<%= rdoITAsset.ClientID%>').checked = true;
                document.getElementById('<%= rdoAdminAsset.ClientID%>').checked = false;
            }
        }
        function ShowUpdateErrMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Category Name already exists in the same asset type!';
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowLevelMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You cannot add category under this parent category, the selected parent category is of Level 3.';
        }
        function ShowCatNotDeletedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You cannot delete this category, since it has child categories.';
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
            width: 200px;
        }
        .style3
        {
            width: 270px;
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
            Category Master</div>
    </div>
    <div id="wrapper1" onload="noBack();">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label9" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right; width: 15%">
                                        <asp:Label ID="Label5" runat="server" Text="Asset Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; width: 2%">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="width: 43%">
                                        <asp:RadioButton ID="rdoITAsset" runat="server" OnCheckedChanged="rdoITAsset_CheckedChanged"
                                            onClick="GetAssetType(this);" ToolTip="Select asset type" ValidationGroup="Submit"
                                            AutoPostBack="true" Text="  IT Asset" CssClass="label" Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoAdminAsset" ToolTip="Select asset type" runat="server" OnCheckedChanged="rdoAdminAsset_CheckedChanged"
                                            onClick="GetAssetType(this);" ValidationGroup="Submit" AutoPostBack="true" Text="  Admin Asset"
                                            CssClass="label" />
                                    </td>
                                    <td rowspan="7" align="center" valign="top" style="width: 40%">
                                        <img src="../images/Category.gif" width="250px" height="200px" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Category Initials" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCategoryCode" autocomplete="off" CssClass="textbox" MaxLength="2"
                                            ValidationGroup="Submit" runat="server" Width="200px" TabIndex="1" ToolTip="Enter Category Code in 2 characters"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Code" runat="server" ControlToValidate="txtCategoryCode"
                                            Display="Dynamic" ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                                            ControlToValidate="txtCategoryCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[Alphanumeric only]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label7" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Category Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCategoryName" autocomplete="off" CssClass="textbox" runat="server"
                                            MaxLength="25" ValidationGroup="Submit" Width="200px" ToolTip="Enter Category Name"
                                            TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Name" runat="server" ControlToValidate="txtCategoryName"
                                            Display="Dynamic" ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="dynamic"
                                            ControlToValidate="txtCategoryName" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[Alphanumeric only]">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top;">
                                        <asp:Label ID="Label4" runat="server" Text="Parent Category" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top;">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList ID="ddlParentCategory" TabIndex="3" CssClass="dropdownlist" Width="200px"
                                            ValidationGroup="Submit" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged"
                                            ToolTip="Select Parent Category" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                            OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset parent category" CausesValidation="false" />
                                        <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                    </td>
                                    <td>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblParentCatCode" runat="server" Text="" CssClass="label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top;">
                                        <asp:Label ID="Label2" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top;">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:TextBox ID="txtCategoryRemarks" autocomplete="off" CssClass="multitextbox" ToolTip="Enter remarks"
                                            runat="server" ValidationGroup="Submit" MaxLength="300" Width="300px" TabIndex="4"
                                            Height="60px" TextMode="MultiLine"></asp:TextBox>
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
                                        <asp:CheckBox ID="chkSetStatus" runat="server" Text="  Active" ToolTip="Set Status"
                                            Checked="true" CssClass="label" TabIndex="5" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save Category" CssClass="button" TabIndex="6"
                                            Width="100px" ToolTip="Save category details" ValidationGroup="Submit"
                                            OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" Width="60px" Text="Reset" CssClass="button"
                                            OnClientClick="ClearFields();" ToolTip="Reset/clear fields" TabIndex="7" CausesValidation="False"
                                            OnClick="btnClear_Click" />
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
                <td colspan="4">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvCategoryMaster" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                    ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    OnRowEditing="gvCategoryMaster_RowEditing" OnRowDeleting="gvCategoryMaster_RowDeleting"
                                    OnRowDataBound="gvCategoryMaster_RowDataBound" OnRowCancelingEdit="gvCategoryMaster_RowCancelingEdit"
                                    OnRowUpdating="gvCategoryMaster_RowUpdating" OnPageIndexChanging="gvPageIndex"
                                    PageSize="50">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Category Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCatCode" runat="server" Text='<%#Eval("CATEGORY_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECatCode" runat="server" Text='<%#Eval("CATEGORY_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCatName" runat="server" Text='<%#Eval("CATEGORY_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtECatName" runat="server" Text='<%#Eval("CATEGORY_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFV_CatName" runat="server" ControlToValidate="txtECatName"
                                                    Text="*" ValidationGroup="Grid"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Parent Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParentCategory" runat="server" Text='<%#Eval("PARENT_CATEGORY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEParentCategory" runat="server" Text='<%#Eval("PARENT_CATEGORY") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetType" runat="server" Text='<%#Eval("ASSET_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEAssetType" runat="server" Text='<%#Eval("ASSET_TYPE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category Level">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCatLevel" runat="server" Text='<%#Eval("CATEGORY_LEVEL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECatLevel" runat="server" Text='<%#Eval("CATEGORY_LEVEL") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category Initials">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCatInit" runat="server" Text='<%#Eval("CATEGORY_INITIALS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECatInit" runat="server" Text='<%#Eval("CATEGORY_INITIALS") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtERemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Active">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Eval("ACTIVE") %>' Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%#Eval("ACTIVE") %>' Enabled="true" />
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
                                                    ImageUrl="~/images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CommandName="Cancel" CausesValidation="false"
                                                    ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" ImageUrl="~/images/Edit_16x16.png"
                                                    runat="server" />
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
                                <asp:AsyncPostBackTrigger ControlID="gvCategoryMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvCategoryMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvCategoryMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvCategoryMaster" EventName="RowCancelingEdit" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export category master data into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
