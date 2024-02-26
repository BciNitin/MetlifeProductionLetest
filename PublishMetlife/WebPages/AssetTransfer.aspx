<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" EnableSessionState="True"
    AutoEventWireup="true" CodeFile="AssetTransfer.aspx.cs" Inherits="AssetTransfer"
    Title="BCIL : ATS - Asset Transfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        var TotalChkBx;
        var Counter;
        window.onload = function() {
            TotalChkBx = parseInt('<%= this.gvAssets.Rows.Count %>');
            Counter = 0;
        }
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtAssetCode.ClientID%>').value = "";
            document.getElementById('<%= txtSerialNo.ClientID%>').value = "";
            //document.getElementById('<%= ddlAssetType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= lstModelName.ClientID%>').selectedIndex = -1;
            document.getElementById('<%= ddlAssetMake.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlProcessName.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetIUT.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlToLocation.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlInterToProcess.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtToWorkStation.ClientID%>').value = "";
            document.getElementById('<%= txtToPort.ClientID%>').value = "";
            document.getElementById('<%= txtRemarks.ClientID%>').value = "";
            document.getElementById('<%= txtPortNo.ClientID%>').value = "";
            document.getElementById('<%= lblFromLocationName.ClientID%>').value = "";
            document.getElementById('<%= rdoInterTransfer.ClientID%>').checked = false;
            document.getElementById('<%= rdoCntryTransfer.ClientID%>').checked = false;
            document.getElementById('<%= txtAssetCode.ClientID%>').focus();
        }
        function GetTransferType(rdbTransferType) {
            if (rdbTransferType.value == 'rdoCntryTransfer') {
                document.getElementById('<%= rdoInterTransfer.ClientID%>').checked = false;
            }
            else if (rdbTransferType.value == 'rdoInterTransfer') {
                document.getElementById('<%= rdoCntryTransfer.ClientID%>').checked = false;
            }
        }
        function HeaderViewClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.gvAssets.ClientID %>');
            var TargetChildControl = "chkSelectAsset";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }
        function ChildClick(CheckBox, HCheckBox) {
            TotalChkBx = parseInt('<%= this.gvAssets.Rows.Count %>');
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
            width: 150px;
        }
        .style2
        {
            width: 130px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebPages/AutoComplete.asmx" />
        </Services>
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Asset Transfer</div>
    </div>
    <div id="wrapper1">
        <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="Table1" runat="server" cellspacing="12" style="width: 100%;" align="center">
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label16" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label33" runat="server" Text="Asset Code" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left; vertical-align: top">
                            <asp:TextBox ID="txtAssetCode" runat="server" CssClass="textbox" AutoComplete="off"
                                Width="200px" TabIndex="1" MaxLength="50" ToolTip="Enter first 8 characters of asset code to search"></asp:TextBox>
                            <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx" ID="AutoCompleteExtender1"
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
                                                var behavior = $find('AutoCompleteEx');
                                                if (!behavior._height) {
                                                    var target = behavior.get_completionList();
                                                    behavior._height = target.offsetHeight - 2;
                                                    target.style.height = '0px';
                                                }" />
                                            <Parallel Duration=".4">
                                                <FadeIn />
                                                <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx')._height" />
                                            </Parallel>
                                        </Sequence>
                                    </OnShow>
                                    <OnHide>
                                        <Parallel Duration=".4">
                                            <FadeOut />
                                            <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx')._height" EndValue="0" />
                                        </Parallel>
                                    </OnHide>
                                </Animations>
                            </act:AutoCompleteExtender>
                            <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAssetCode"
                                WatermarkText="Search Asset Code..." WatermarkCssClass="watermarked" />
                            &nbsp;
                            <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/images/go_button.png" OnClick="btnGo_Click"
                                Height="25px" Width="25px" ToolTip="Enter asset code and click on go to get asset details" />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label37" runat="server" Text="Serial No." CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox autocomplete="off" ID="txtSerialNo" runat="server" Width="200px" TabIndex="2"
                                CssClass="textbox" MaxLength="50" ToolTip="Enter serial code of the asset"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label1" runat="server" Text="Asset Type" Enabled="false" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlAssetType" Width="200px" runat="server" Enabled="false"
                                ToolTip="Select asset type" TabIndex="3" CssClass="dropdownlist" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                                <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                                <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label6" runat="server" Text="Asset Category" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlAssetCategory" Width="200px" TabIndex="4" runat="server"
                                ToolTip="Select asset category" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                            <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset category" CausesValidation="false" />
                            <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" TabIndex="5"
                                Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align: top">
                            <asp:Label ID="Label19" runat="server" Text="Asset Make" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center; vertical-align: top">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left; vertical-align: top">
                            <asp:DropDownList ID="ddlAssetMake" Width="200px" TabIndex="6" runat="server" CssClass="dropdownlist"
                                AutoPostBack="true" ToolTip="Select asset make" OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right; vertical-align: top">
                            <asp:Label ID="Label20" runat="server" Text="Model Name" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center; vertical-align: top">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left; vertical-align: top" rowspan="2">
                            <asp:ListBox ID="lstModelName" ToolTip="Select one or more model names" TabIndex="7"
                                SelectionMode="Multiple" Width="200px" CssClass="textbox" runat="server"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align: top">
                            <asp:Label ID="Label2" runat="server" Text="Asset Process" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center; vertical-align: top">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left; vertical-align: top">
                            <asp:DropDownList ID="ddlProcessName" Width="200px" TabIndex="8" CssClass="dropdownlist"
                                runat="server" ToolTip="Select process name">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right">
                            <asp:Button ID="btnSearch" TabIndex="9" runat="server" CssClass="button" Text="Get Assets"
                                Width="75px" ToolTip="Get a list of assets based on search criteria" OnClick="btnSearch_Click" />
                        </td>
                        <td style="text-align: center">
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align: top">
                            <asp:Label ID="Label21" runat="server" Text="Port No." CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center; vertical-align: top">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left; vertical-align: top">
                            <asp:TextBox autocomplete="off" ID="txtPortNo" runat="server" Width="200px" TabIndex="10"
                                CssClass="textbox" MaxLength="50" ToolTip="Enter port no."></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label22" runat="server" CssClass="label" Text="Transfer Date"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox autocomplete="off" ID="txtTransferDate" runat="server" Width="200px"
                                ToolTip="Enter/select asset transfer date" TabIndex="11" CssClass="textbox" onfocus="showCalendarControl(this);"
                                MaxLength="30"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFV_Date" runat="server" CssClass="validation" ControlToValidate="txtTransferDate"
                                Display="Dynamic" ErrorMessage="[Required]" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                        </td>
                        <td style="text-align: center">
                        </td>
                        <td style="text-align: left">
                            <asp:RadioButton ID="rdoCntryTransfer" runat="server" Text="  For Country Wide Movement/Transfer"
                                CssClass="label" Checked="true" onClick="GetTransferType(this);" OnCheckedChanged="rdoCntryTransfer_CheckedChanged"
                                AutoPostBack="true" TabIndex="12" ToolTip="Select for country wide asset transfer" />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label5" runat="server" Text="Asset IUT" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlAssetIUT" runat="server" CssClass="dropdownlist" ToolTip="Select asset IUT status"
                                Width="200px" TabIndex="13">
                                <asp:ListItem Text="-- Select --" Value="SELECT"></asp:ListItem>
                                <asp:ListItem Text="IUT" Value="IUT"></asp:ListItem>
                                <asp:ListItem Text="NONE" Value="NONE"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label3" runat="server" CssClass="label" Text="From Location"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold; height: 17px;">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lblFromLocationName" runat="server" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label4" runat="server" CssClass="label" Text="To Location"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold; height: 17px;">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlToLocation" Width="200px" TabIndex="14" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlToLocation_SelectedIndexChanged" ToolTip="Select transfer to location"
                                CssClass="dropdownlist">
                            </asp:DropDownList>
                            <asp:Label ID="lblLocLevel2" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                            <asp:ImageButton ID="ibtnRefreshLocation2" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                OnClick="ibtnRefreshLocation2_Click" TabIndex="15" ToolTip="Refresh/reset location" />
                            <asp:Label ID="lblToLocCode" runat="server" CssClass="label" Visible="false" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblFromLocCode" runat="server" CssClass="label" Visible="false" Text=""></asp:Label>
                        </td>
                        <td style="text-align: center">
                        </td>
                        <td style="text-align: left">
                            <asp:RadioButton ID="rdoInterTransfer" runat="server" Text="  For Inter Location Movement/Transfer"
                                CssClass="label" TabIndex="16" ToolTip="Select for inter location asset transfer"
                                onClick="GetTransferType(this);" OnCheckedChanged="rdoInterTransfer_CheckedChanged"
                                AutoPostBack="true" />
                        </td>
                        <td style="text-align: right">
                        </td>
                        <td style="text-align: center">
                        </td>
                        <td style="text-align: left">
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label9" runat="server" CssClass="label" Text="To Location"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold;">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlInterToLocation" Enabled="false" Width="200px" TabIndex="17"
                                runat="server" AutoPostBack="true" ToolTip="Select inter to location" OnSelectedIndexChanged="ddlInterToLocation_SelectedIndexChanged"
                                CssClass="dropdownlist">
                            </asp:DropDownList>
                            <asp:Label ID="lblInterLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                            <asp:ImageButton ID="ibtnRefreshLocation" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                OnClick="ibtnRefreshLocation_Click" TabIndex="18" ToolTip="Refresh/reset location"
                                CausesValidation="false" Enabled="false" />
                            <asp:Label ID="lblInterLocCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label11" runat="server" CssClass="label" Text="To WorkStation"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold;">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox autocomplete="off" ID="txtToWorkStation" runat="server" Width="200px"
                                TabIndex="19" CssClass="textbox" MaxLength="30" Enabled="false" ToolTip="Enter first 3 characters of the workstation no. to search"></asp:TextBox>
                            <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="AutoCompleteExtender3"
                                TargetControlID="txtToWorkStation" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetWorkStationNo"
                                MinimumPrefixLength="1" CompletionInterval="1000" EnableCaching="true" CompletionSetCount="20"
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
                            <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtToWorkStation"
                                WatermarkText="Search Workstation No..." WatermarkCssClass="watermarked" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label10" runat="server" CssClass="label" Text="To Process"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold; height: 17px;">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlInterToProcess" runat="server" TabIndex="20" Width="200px"
                                CssClass="dropdownlist" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label12" runat="server" CssClass="label" Text="To Port"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <div style="font-weight: bold; height: 17px;">
                                :</div>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox autocomplete="off" ID="txtToPort" runat="server" Width="200px" TabIndex="21"
                                CssClass="textbox" MaxLength="20" Enabled="false" ToolTip="Enter first 3 characters of the port no. to search"></asp:TextBox>
                            <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx4" ID="AutoCompleteExtender5"
                                TargetControlID="txtToPort" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetPortNo"
                                MinimumPrefixLength="1" CompletionInterval="1000" EnableCaching="true" CompletionSetCount="20"
                                DelimiterCharacters=";, :" ShowOnlyCurrentWordInCompletionListItem="true" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                <Animations>
                                    <OnShow>
                                        <Sequence>
                                            <OpacityAction Opacity="0" />
                                            <HideAction Visible="true" />
                                            <ScriptAction Script="
                                                // Cache the size and setup the initial size
                                                var behavior = $find('AutoCompleteEx4');
                                                if (!behavior._height) {
                                                    var target = behavior.get_completionList();
                                                    behavior._height = target.offsetHeight - 2;
                                                    target.style.height = '0px';
                                                }" />
                                            <Parallel Duration=".4">
                                                <FadeIn />
                                                <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx4')._height" />
                                            </Parallel>
                                        </Sequence>
                                    </OnShow>
                                    <OnHide>
                                        <Parallel Duration=".4">
                                            <FadeOut />
                                            <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx4')._height" EndValue="0" />
                                        </Parallel>
                                    </OnHide>
                                </Animations>
                            </act:AutoCompleteExtender>
                            <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" TargetControlID="txtToPort"
                                WatermarkText="Search Port No..." WatermarkCssClass="watermarked" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align: top">
                            <asp:Label ID="Label36" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                        </td>
                        <td style="text-align: center; vertical-align: top">
                            <div style="font-weight: bold">
                                :</div>
                        </td>
                        <td colspan="4">
                            <asp:TextBox autocomplete="off" ID="txtRemarks" runat="server" CssClass="multitextbox"
                                ToolTip="Enter asset transfer remarks" TabIndex="22" Height="100px" Width="700px"
                                TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRemarks"
                                Display="Dynamic" ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="lblAssetCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="gvAssets" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                ShowFooter="false" CssClass="mGrid" OnPageIndexChanging="gvAssets_PageIndexChanging"
                                PagerStyle-CssClass="pgr" OnRowCreated="gvAssets_RowCreated" AlternatingRowStyle-CssClass="alt"
                                PageSize="50">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHSelect" runat="server" onclick="javascript:HeaderViewClick(this);"
                                                Text=" Select" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelectAsset" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Serial No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSerialCode" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Make">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetMake" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCatCode" runat="server" Text='<%#Eval("CATEGORY_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Process">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProcess" runat="server" Text='<%#Eval("ASSET_PROCESS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Port No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPortNo" runat="server" Text='<%#Eval("PORT_NO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="WorkStation No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWorkStationNo" runat="server" Text='<%#Eval("WORKSTATION_NO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false" HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFromLocationCode" runat="server" Text='<%#Eval("ASSET_LOCATION") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pgr"></PagerStyle>
                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="6">
                            <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" Text="Transfer Assets" CssClass="button" 
                                TabIndex="23" Width="120px" ToolTip="Save asset transfer details" 
                                OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnClear" runat="server" OnClientClick="ClearFields();" Text="Reset" CssClass="button"
                                TabIndex="24" Width="60px" ToolTip="Reset/clear fields" CausesValidation="false"
                                OnClick="btnClear_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="lblErrorMsg" CssClass="ErrorLabel" Font-Bold="true" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div id="DivGrid">
                                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvAssetTransfer" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                            ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                            OnPageIndexChanging="gvAssetTransfer_PageIndexChanging" PageSize="50" OnRowDeleting="gvAssetTransfer_RowDeleting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Asset Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Serial Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Model">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Process">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProcess" runat="server" Text='<%#Eval("ASSET_PROCESS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="From Location">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFromLocation" runat="server" Text='<%#Eval("FROM_LOCATION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To Location">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblToLocation" runat="server" Text='<%#Eval("TO_LOCATION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="From Workstation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFromWSNo" runat="server" Text='<%#Eval("FROM_WORKSTATION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To Workstation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblToWSNo" runat="server" Text='<%#Eval("TO_WORKSTATION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="From Port">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFromPort" runat="server" Text='<%#Eval("FROM_PORT") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To Port">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblToPort" runat="server" Text='<%#Eval("TO_PORT") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inter From Location">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInterFromLocation" runat="server" Text='<%#Eval("INTER_FROM_LOCATION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inter To Location">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInterToLocation" runat="server" Text='<%#Eval("INTER_TO_LOCATION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inter To Process">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInterToProcess" runat="server" Text='<%#Eval("INTER_TO_PROCESS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Transfer Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTransferDate" runat="server" Text='<%#Eval("TRANSFER_DATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Transfer Type" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTransferType" runat="server" Text='<%#Eval("TRANSFER_TYPE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Delete" HeaderStyle-Width="50px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imagebuttonDelete" ToolTip="View" CommandName="Delete" ImageUrl="~/images/Delete_16x16.png"
                                                            runat="server" CommandArgument='<%#Eval("ASSET_CODE") %>' OnClientClick="return confirm('Are you sure to delete transfer details?');" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                            <PagerStyle CssClass="pgr"></PagerStyle>
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvAssetTransfer" EventName="PageIndexChanging" />
                                        <asp:AsyncPostBackTrigger ControlID="gvAssetTransfer" EventName="RowDeleting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="Table4" runat="server" cellspacing="16" style="width: 100%" align="center">
            <tr>
                <td colspan="6" style="text-align: center">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="25" Enabled="true" ToolTip="Export asset transfer data into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
        <table id="Table5" runat="server" cellspacing="14" style="width: 99%; border: 2px double #006600;"
            align="center">
            <tr>
                <td colspan="6" style="text-align: center">
                    <asp:Label ID="Label13" Font-Bold="true" Font-Size="Small" runat="server" Text="Export Country Wide Transferred Assets"
                        CssClass="label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label17" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                    <asp:Label ID="Label77" runat="server" Text="From Date" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:TextBox autocomplete="off" ID="txtFromDate" ValidationGroup="Transfer" CssClass="textbox"
                        onfocus="showCalendarControl(this);" runat="server" Width="200px" TabIndex="26"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtFromDate"
                        ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Transfer"></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label18" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                    <asp:Label ID="Label7" runat="server" Text="To Date" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:TextBox autocomplete="off" ID="txtToDate" ValidationGroup="Transfer" CssClass="textbox"
                        onfocus="showCalendarControl(this);" runat="server" Width="200px" TabIndex="27"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToDate"
                        ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Transfer"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label8" runat="server" Text="Transferred Location" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlLocation" Width="200px" TabIndex="28" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" CssClass="dropdownlist">
                    </asp:DropDownList>
                    <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                    <asp:ImageButton ID="btnRefreahLocation" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                        OnClick="btnRefreahLocation_Click" TabIndex="29" ToolTip="Refresh/reset location" />
                    <asp:Label ID="lblLocCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                </td>
                <td style="text-align: right">
                </td>
                <td style="text-align: center">
                </td>
                <td style="text-align: left">
                    <asp:Button ID="btnGetTransferredData" ValidationGroup="Transfer" runat="server"
                        Text="Export Data" CssClass="button" Width="110px" TabIndex="30" OnClick="btnGetTransferredData_Click"
                        ToolTip="Export transferred assets into excel file" />&nbsp;&nbsp;
                    <asp:Button ID="btnReset" CssClass="button" CausesValidation="false" runat="server"
                        Text="Reset" TabIndex="31" Width="60px" ToolTip="Reset/clear fields" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
