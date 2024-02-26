<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="ApproveAssets.aspx.cs" Inherits="ApproveAssets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtAssetCode.ClientID%>').value = "";
            document.getElementById('<%= txtSerialCode.ClientID%>').value = "";
            document.getElementById('<%= lstModelName.ClientID%>').selectedIndex = -1;
            //document.getElementById('<%= ddlAssetType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetMake.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= chkApproveAllAssets.ClientID%>').checked = false;
            document.getElementById('<%= txtAssetCode.ClientID%>').focus();
        }
        var TotalChkBx;
        var Counter;
        window.onload = function() {
            TotalChkBx = parseInt('<%= this.gvApproveAsset.Rows.Count %>');
            Counter = 0;
        }
        function HeaderViewClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.gvApproveAsset.ClientID %>');
            var TargetChildControl = "chkApprove";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }
        function ChildClick(CheckBox, HCheckBox) {
            TotalChkBx = parseInt('<%= this.gvApproveAsset.Rows.Count %>');
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
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="~/WebPages/AutoComplete.asmx" />
        </Services>
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Approve Assets</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="12" style="width: 100%;" align="center">
            <tr>
                <td colspan="6">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label33" runat="server" Text="Asset Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtAssetCode" runat="server" CssClass="textbox"
                                            Width="200px" TabIndex="1" ToolTip="Enter First 8 characters of the asset code to search"></asp:TextBox>
                                        <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                                            TargetControlID="txtAssetCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetCode"
                                            MinimumPrefixLength="8" CompletionInterval="1000" EnableCaching="true" CompletionSetCount="20"
                                            DelimiterCharacters=";, :" ShowOnlyCurrentWordInCompletionListItem="true" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                            <Animations>
                                                <OnShow>
                                                    <Sequence>
                                                        <OpacityAction Opacity="0" />
                                                        <HideAction Visible="true" />
                                                        <ScriptAction Script="
                                                            // Cache the size and setup the initial size
                                                            var behavior = $find('AutoCompleteEx1');
                                                            if (!behavior._height) {
                                                                var target = behavior.get_completionList();
                                                                behavior._height = target.offsetHeight - 2;
                                                                target.style.height = '0px';
                                                            }" />
                                                        <Parallel Duration=".4">
                                                            <FadeIn />
                                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx1')._height" />
                                                        </Parallel>
                                                    </Sequence>
                                                </OnShow>
                                                <OnHide>
                                                    <Parallel Duration=".4">
                                                        <FadeOut />
                                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx1')._height" EndValue="0" />
                                                    </Parallel>
                                                </OnHide>
                                            </Animations>
                                        </act:AutoCompleteExtender>
                                        <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAssetCode"
                                            WatermarkText="Search Asset Code..." WatermarkCssClass="watermarked" />
                                        &nbsp;
                                        <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/images/go_button.png" OnClick="btnGo_Click"
                                            Height="25px" Width="25px" ToolTip="Enter asset code and click go button to get serial code" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="Serial Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtSerialCode" runat="server" CssClass="textbox"
                                            Width="200px" TabIndex="3" ToolTip="Enter first 5 characters of serial code to search"></asp:TextBox>
                                        <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="AutoCompleteExtender2"
                                            TargetControlID="txtSerialCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetSerialNo"
                                            MinimumPrefixLength="5" CompletionInterval="1000" EnableCaching="true" CompletionSetCount="20"
                                            DelimiterCharacters=";, :" ShowOnlyCurrentWordInCompletionListItem="true" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                            <Animations>
                                                <OnShow>
                                                    <Sequence>
                                                        <OpacityAction Opacity="0" />
                                                        <HideAction Visible="true" />
                                                        <ScriptAction Script="
                                                            // Cache the size and setup the initial size
                                                            var behavior = $find('AutoCompleteEx2');
                                                            if (!behavior._height) {
                                                                var target = behavior.get_completionList();
                                                                behavior._height = target.offsetHeight - 2;
                                                                target.style.height = '0px';
                                                            }" />
                                                        <Parallel Duration=".4">
                                                            <FadeIn />
                                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx2')._height" />
                                                        </Parallel>
                                                    </Sequence>
                                                </OnShow>
                                                <OnHide>
                                                    <Parallel Duration=".4">
                                                        <FadeOut />
                                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx2')._height" EndValue="0" />
                                                    </Parallel>
                                                </OnHide>
                                            </Animations>
                                        </act:AutoCompleteExtender>
                                        <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtSerialCode"
                                            WatermarkText="Search Serial Code..." WatermarkCssClass="watermarked" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" runat="server" Text="Asset Type" Enabled="false" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAssetType" Width="200px" runat="server" Enabled="false"
                                            ToolTip="Select asset type" TabIndex="5" CssClass="dropdownlist" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                                            <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="Asset Category" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAssetCategory" Width="200px" TabIndex="5" runat="server"
                                            CssClass="dropdownlist" AutoPostBack="true" ToolTip="Select asset category" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                            OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset category" CausesValidation="false" />
                                        <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label7" runat="server" Text="Asset Make" CssClass="label"></asp:Label>
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
                                        <asp:Label ID="Label1" runat="server" Text="Model Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" rowspan="2">
                                        <asp:ListBox ID="lstModelName" TabIndex="6"
                                            SelectionMode="Multiple" ToolTip="Select one or more model names" Width="200px"
                                            CssClass="textbox" runat="server"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label3" runat="server" Text="Asset Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:DropDownList ID="ddlAssetLocation" Width="200px" TabIndex="4" runat="server"
                                            AutoPostBack="true" ToolTip="Select asset location" OnSelectedIndexChanged="ddlAssetLocation_SelectedIndexChanged"
                                            CssClass="dropdownlist">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="ibtnRefreshLocation" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                            OnClick="ibtnRefreshLocation_Click" ToolTip="Refresh/reset location" CausesValidation="false" />
                                        <asp:Label ID="lblLocCode" CssClass="label" Text="0" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label6" runat="server" Text="Operation Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                        <asp:DropDownList ID="ddlOperationType" Width="200px" TabIndex="4" runat="server"
                                            CssClass="dropdownlist" ToolTip="Select operation type">
                                            <asp:ListItem Text="-- Select Operation Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="SWAPPED" Value="SWAPPED"></asp:ListItem>
                                            <asp:ListItem Text="REPLACED" Value="REPLACED"></asp:ListItem>
                                            <asp:ListItem Text="TRANSFERRED" Value="TRANSFERRED"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblUnApproved" Text="Unapproved Assets" Visible="false" runat="server"
                                            CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblUnApprovedAssets" runat="server" Visible="false" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblApproved" runat="server" Text="Approved Assets" Visible="false"
                                            CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblApprovedAssets" runat="server" CssClass="label" Visible="false"></asp:Label>
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
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                            <asp:PostBackTrigger ControlID="btnClear" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" Text="Get Unapproved Assets" CssClass="button"
                        TabIndex="35" ToolTip="Get a list of unapproved assets" Width="150px"
                        OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" OnClientClick="ClearFields();" Text="Reset" CssClass="button"
                        TabIndex="36" ToolTip="Reset/clear fields" Width="60px" CausesValidation="false"
                        OnClick="btnClear_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: center">
                    <asp:CheckBox ID="chkApproveAllAssets" Text="   Approve All Assets" CssClass="label"
                        runat="server" Enabled="false" ToolTip="Check to approve all the unapproved assets" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvApproveAsset" runat="server" AllowPaging="True" PageSize="50"
                                    AutoGenerateColumns="False" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvApproveAsset_PageIndexChanging"
                                    OnRowCreated="gvApproveAsset_RowCreated" OnRowCommand="gvApproveAsset_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FAMS ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetID" runat="server" Text='<%#Eval("ASSET_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetSrlCode" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Make">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetMake" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryName" runat="server" Text='<%#Eval("CATEGORY_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Port No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPortNo" runat="server" Text='<%#Eval("PORT_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View" HeaderStyle-Width="50px">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imagebuttonView" ToolTip="View" CommandName="View" ImageUrl="~/images/View_16x16.png"
                                                    runat="server" CommandArgument='<%#Eval("ASSET_CODE") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHApprove" runat="server" onclick="javascript:HeaderViewClick(this);"
                                                    Text="  Approve" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkApprove" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvApproveAsset" EventName="PageIndexChanging" />
                                <asp:AsyncPostBackTrigger ControlID="gvApproveAsset" EventName="RowCreated" />
                                <asp:AsyncPostBackTrigger ControlID="gvApproveAsset" EventName="RowCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    <asp:Button ID="btnApprove" runat="server" Text="Approved Assets" CssClass="button" TabIndex="35"
                        Visible="false" ToolTip="Approve all the assets" Width="100px" OnClick="btnApprove_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
