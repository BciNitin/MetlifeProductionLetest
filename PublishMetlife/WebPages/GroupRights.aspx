<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="GroupRights.aspx.cs" Inherits="GroupRights" Title="BCIL : ATS - Group Rights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHtml = '';
            document.getElementById('<%= ddlGroup.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= rdoAdminAssetType.ClientID %>').checked = false;
            document.getElementById('<%= rdoITAssetType.ClientID %>').checked = false;
            document.getElementById('<%= ddlGroup.ClientID %>').focus();
        }
        function ResetType() {
            document.getElementById('<%= rdoAdminAssetType.ClientID %>').checked = false;
            document.getElementById('<%= rdoITAssetType.ClientID %>').checked = false;
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowNoRowMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : There is no row found for the selected group.';
        }
        function ShowNullMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : There is no row found for group rights updation.';
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowSuccessMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Group rights updated successfully!';
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to execute this operation!');
        }
        var TotalChkBx;
        var TotalChkBxSave;
        var TotalChkBxEdit;
        var TotalChkBxDelete;
        var TotalChkBxExport;
        var Counter;
        function GetRowsCount() {
            TotalChkBx = document.getElementById('<%= gvGroupRights.ClientID %>').rows.length;
            TotalChkBxSave = document.getElementById('<%= this.gvGroupRights.ClientID %>').rows.length;
            TotalChkBxEdit = document.getElementById('<%= this.gvGroupRights.ClientID %>').rows.length;
            TotalChkBxDelete = document.getElementById('<%= this.gvGroupRights.ClientID %>').rows.length;
            TotalChkBxExport = document.getElementById('<%= this.gvGroupRights.ClientID %>').rows.length;
            Counter = 0;
        }
        function HeaderViewClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.gvGroupRights.ClientID %>');
            var TargetChildControl = "chkView";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }
        function HeaderNewClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.gvGroupRights.ClientID %>');
            var TargetChildControl = "chkSave";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBxSave : 0;
        }
        function HeaderEditClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.gvGroupRights.ClientID %>');
            var TargetChildControl = "chkEdit";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBxEdit : 0;
        }
        function HeaderDeleteClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.gvGroupRights.ClientID %>');
            var TargetChildControl = "chkDelete";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBxDelete : 0;
        }
        function HeaderExportClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.gvGroupRights.ClientID %>');
            var TargetChildControl = "chkExport";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBxExport : 0;
        }
        function ChildViewClick(CheckBox, HCheckBox) {
            TotalChkBx = document.getElementById('<%= gvGroupRights.ClientID %>').rows.length;
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
        function ChildSaveClick(CheckBox, HCheckBox) {
            TotalChkBxSave = document.getElementById('<%= gvGroupRights.ClientID %>').rows.length;
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBxSave)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBxSave)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBxSave)
                HeaderCheckBox.checked = true;
        }
        function ChildEditClick(CheckBox, HCheckBox) {
            TotalChkBxEdit = document.getElementById('<%= gvGroupRights.ClientID %>').rows.length;
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBxEdit)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBxEdit)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBxEdit)
                HeaderCheckBox.checked = true;
        }
        function ChildDeleteClick(CheckBox, HCheckBox) {
            TotalChkBxDelete = document.getElementById('<%= gvGroupRights.ClientID %>').rows.length;
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBxDelete)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBxDelete)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBxDelete)
                HeaderCheckBox.checked = true;
        }
        function ChildExportClick(CheckBox, HCheckBox) {
            TotalChkBxExport = document.getElementById('<%= gvGroupRights.ClientID %>').rows.length;
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBxExport)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBxExport)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBxExport)
                HeaderCheckBox.checked = true;
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 170px;
        }
        .style2
        {
            width: 300px;
        }
        .style3
        {
            height: 143px;
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
            Group Rights</div>
    </div>
    <div id="wrapper1" onload="GetRowsCount();">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" cellspacing="10" style="width: 100%;" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="Select Group" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlGroup" ToolTip="Select Group Name" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"
                                            AutoPostBack="true" CssClass="dropdownlist" Width="250px" runat="server" ValidationGroup="Submit">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_GrpCode" ValidationGroup="Submit" InitialValue="-- Select Group --"
                                            runat="server" ControlToValidate="ddlGroup" Display="Dynamic" ErrorMessage="[Required]"
                                            CssClass="validation"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: left">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td style="text-align: left">
                                    </td>
                                    <td rowspan="6" style="vertical-align: top; text-align: center;">
                                        <img src="../images/GroupRights.jpg" style="width: 150px; height: 150px;" alt="" />
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label2" runat="server" Text="Select Asset Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:RadioButton ID="rdoAdminAssetType" CssClass="label" Checked="true" runat="server" Text="     ADMIN" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoITAssetType" CssClass="label" runat="server" Text="     IT" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnReset" runat="server" Visible="true" CssClass="button" Text="Reset Asset Type"
                                            ToolTip="Reset/Clear asset type selection" Width="120px" OnClientClick="ResetType();"
                                            CausesValidation="False" />
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
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
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
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSubmit" runat="server" Text="Update Group Rights" CssClass="button"
                                            OnClick="btnSubmit_Click" TabIndex="11" ToolTip="Update/change group rights"
                                            ValidationGroup="Submit" Width="150px" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" CausesValidation="False" Text="Reset" CssClass="button"
                                            OnClientClick="ClearFields();" TabIndex="12" ToolTip="Reset/Clear group rights"
                                            Width="60px" OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" runat="server" CssClass="ErrorLabel"></asp:Label>
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
                                <asp:GridView ID="gvGroupRights" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                    OnRowCreated="gvGroupRights_RowCreated" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvGroupRights_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Page Code" HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPageCode" runat="server" Text='<%#Eval("PAGE_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Page Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPageName" runat="server" Text='<%#Eval("PAGE_DESCRIPTION") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHView" runat="server" onclick="javascript:HeaderViewClick(this);"
                                                    Text="  View" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkView" runat="server" Checked='<%#Eval("VIEW_RIGHTS") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHSave" runat="server" onclick="javascript:HeaderNewClick(this);"
                                                    Text="  Save" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSave" runat="server" Checked='<%#Eval("SAVE_RIGHTS") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHEdit" runat="server" Text="  Edit" onclick="javascript:HeaderEditClick(this);" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEdit" runat="server" Checked='<%#Eval("EDIT_RIGHTS") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHDelete" runat="server" Text="  Delete" onclick="javascript:HeaderDeleteClick(this);" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkDelete" runat="server" Checked='<%#Eval("DELETE_RIGHTS") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHExport" runat="server" Text="  Export" onclick="javascript:HeaderExportClick(this);" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkExport" runat="server" Checked='<%#Eval("EXPORT_RIGHTS") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvGroupRights" EventName="RowCreated" />
                                <asp:AsyncPostBackTrigger ControlID="gvGroupRights" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export group rights data into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
