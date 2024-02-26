<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="CallLogDetails.aspx.cs" Inherits="CallLogDetails" Title="BCIL : ATS - Call Log Details" %>

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
            document.getElementById('<%= ddlVendor.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtContPerson.ClientID%>').value = "";
            document.getElementById('<%= ddlCallStatus.ClientID%>').selectedIndex = 0;
            //document.getElementById('<%= ddlAssetType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtCallNo.ClientID%>').value = "";
            document.getElementById('<%= txtCallDate.ClientID%>').value = "";
            document.getElementById('<%= txtResolvedDate.ClientID%>').value = "";
            document.getElementById('<%= txtRespondedDate.ClientID%>').value = "";
            document.getElementById('<%= txtFRUNo.ClientID%>').value = "";
            document.getElementById('<%= txtGatePassNo.ClientID%>').value = "";
            document.getElementById('<%= txtEngrName.ClientID%>').value = "";
            document.getElementById('<%= txtVendorLocation.ClientID%>').value = "";
            document.getElementById('<%= txtActionTaken.ClientID%>').value = "";
            document.getElementById('<%= txtReplacedSrlNo.ClientID%>').value = "";
            document.getElementById('<%= txtRemarks.ClientID%>').value = "";
            document.getElementById('<%= ddlAssetMake.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= lstModelName.ClientID%>').selectedIndex = -1;
            document.getElementById('<%= rdoRepair.ClientID%>').checked = true;
            document.getElementById('<%= rdoReplaced.ClientID%>').checked = false;
            document.getElementById('<%= txtAssetCode.ClientID%>').value = "";
            document.getElementById('<%= txtSerialCode.ClientID%>').value = "";
            document.getElementById('<%= ddlVendor.ClientID%>').focus();
        }
        function EnableResolvedDate() {
            var Status = document.getElementById('<%= ddlCallStatus.ClientID%>');
            var Val = Status.options[Status.selectedIndex].value;
            if (Val == "RESOLVED") {
                document.getElementById('<%= txtResolvedDate.ClientID%>').value = "";
                document.getElementById('<%= txtResolvedDate.ClientID%>').disabled = false;
            }
            else if (Val == "PENDING" || Val == "UNRESOLVED" || Val == "SELECT") {
                document.getElementById('<%= txtResolvedDate.ClientID%>').value = "";
                document.getElementById('<%= txtResolvedDate.ClientID%>').disabled = true;
            }
        }
        function GetRepairReplaced(rdbType) {
            if (rdbType.value == 'rdoRepair') {
                document.getElementById('<%= txtReplacedSrlNo.ClientID%>').disabled = true;
                document.getElementById('<%= rdoRepair.ClientID%>').checked = true;
                document.getElementById('<%= rdoReplaced.ClientID%>').checked = false;
            }
            else if (rdbType.value == 'rdoReplaced') {
                document.getElementById('<%= txtReplacedSrlNo.ClientID%>').disabled = false;
                document.getElementById('<%= txtReplacedSrlNo.ClientID%>').value = "";
                document.getElementById('<%= rdoReplaced.ClientID%>').checked = true;
                document.getElementById('<%= rdoRepair.ClientID%>').checked = false;
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
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowAssetNotSelectedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Asset has not been selected.';
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
            width: 190px;
        }
        .style2
        {
            width: 125px;
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
            Vendor Call Log Management</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="12" align="center">
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="Label16" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                                            runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label14" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Call Log No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtCallLogCode" runat="server" ToolTip="Enter Call Log Code"
                                            CssClass="readonlytext" Width="200px" TabIndex="-1" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_CallLog" runat="server" ControlToValidate="txtCallLogCode"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label23" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Select Vendor" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlVendor" runat="server" ToolTip="Select Vendor Name" AutoPostBack="true"
                                            CssClass="dropdownlist" Width="200px" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_VendName" runat="server" ControlToValidate="ddlVendor"
                                            ErrorMessage="[Required]" InitialValue="-- Select Vendor --" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label24" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Text="Contact Person" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtContPerson" CssClass="textbox" runat="server"
                                            ToolTip="Enter Vendor Contact Person Name" Width="200px" TabIndex="1" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContPerson"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label25" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label17" runat="server" Text="Vendor Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtVendorLocation" CssClass="textbox" runat="server"
                                            ToolTip="Enter Vendor Location" Width="200px" TabIndex="1" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtVendorLocation"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label15" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Text="Call No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtCallNo" CssClass="textbox" runat="server"
                                            ToolTip="Enter Call No." Width="200px" TabIndex="1" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCallNo"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label26" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label5" runat="server" Text="Call Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtCallDate" CssClass="textbox" runat="server"
                                            ToolTip="Enter/Select Call Date" onfocus="showCalendarControl(this);" Width="200px"
                                            TabIndex="1" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCallDate"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label27" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label7" runat="server" Text="Call Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlCallStatus" runat="server" ToolTip="Select Call Log Status"
                                            CssClass="dropdownlist" Width="200px" onChange="EnableResolvedDate();">
                                            <asp:ListItem Text="-- Select Status --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="PENDING"></asp:ListItem>
                                            <asp:ListItem Text="Resolved" Value="RESOLVED"></asp:ListItem>
                                            <asp:ListItem Text="Un-Resolved" Value="UNRESOLVED"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlCallStatus"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="Resolved Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtResolvedDate" CssClass="textbox" runat="server"
                                            Enabled="false" onfocus="showCalendarControl(this);" Width="200px" TabIndex="1"
                                            ToolTip="Enter/Select Resolved Date" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label10" runat="server" Text="Part Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButton ID="rdoRepair" runat="server" TabIndex="27" onClick="GetRepairReplaced(this);"
                                            CssClass="label" Text=" Repair" Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoReplaced" runat="server" TabIndex="28" onClick="GetRepairReplaced(this);"
                                            CssClass="label" Text=" Replaced" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label12" runat="server" Text="Replaced S. No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtReplacedSrlNo" CssClass="textbox" runat="server"
                                            Enabled="false" Width="200px" TabIndex="1" ToolTip="Enter Replaced Serial No. in case of replacement"
                                            MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label19" runat="server" Text="FRU No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtFRUNo" CssClass="textbox" runat="server" Width="200px"
                                            TabIndex="1" ToolTip="Enter FRU No." MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label20" runat="server" Text="GatePass No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtGatePassNo" CssClass="textbox" runat="server"
                                            Width="200px" TabIndex="1" ToolTip="Enter Gate Pass No." MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label21" runat="server" Text="Engineer Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtEngrName" CssClass="textbox" runat="server"
                                            Width="200px" TabIndex="1" ToolTip="Enter Engineer Name" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label22" runat="server" Text="Action Taken" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtActionTaken" CssClass="textbox" runat="server"
                                            Width="200px" TabIndex="1" ToolTip="Enter Action Taken Details" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label8" runat="server" Text="Responded Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:TextBox autocomplete="off" ID="txtRespondedDate" CssClass="textbox" runat="server"
                                            MaxLength="50" onfocus="showCalendarControl(this);" Width="200px" TabIndex="1"
                                            ToolTip="Enter Responded Date"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label9" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:TextBox autocomplete="off" ID="txtRemarks" MaxLength="500" CssClass="multitextbox"
                                            runat="server" TextMode="MultiLine" Width="200px" Height="50px" TabIndex="1"
                                            ToolTip="Enter Call Log Remarks"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left" colspan="6">
                                        <table id="tblAssets" runat="server" cellspacing="10" width="100%" style="border: 3px double #006600;"
                                            align="center">
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label28" runat="server" CssClass="label" Text="Asset Code : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetCode" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 8 characters of the asset code to search"></asp:TextBox>
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtAssetCode"
                                                        WatermarkText="Search Asset Code..." WatermarkCssClass="watermarked" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="btnGo" runat="server" TabIndex="21" ImageUrl="~/images/go_button.png"
                                                        OnClick="btnGo_Click" Height="25px" ToolTip="Enter asset code and click go button"
                                                        Width="25px" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label29" runat="server" CssClass="label" Text="Serial No. : "></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox autocomplete="off" ID="txtSerialCode" runat="server" CssClass="textbox"
                                                        TabIndex="22" Width="200px" MaxLength="50" ToolTip="Enter first 5 characters of the serial code to search"></asp:TextBox>
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtSerialCode"
                                                        WatermarkText="Search Serial Code..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label13" runat="server" Enabled="false" Text="Asset Type :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlAssetType" Enabled="false" Width="200px" runat="server"
                                                        CssClass="dropdownlist" AutoPostBack="True" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                                                        <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                                        <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                                                        <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label18" runat="server" Text="Asset Category :" CssClass="label"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAssetCategory" Width="200px" TabIndex="4" runat="server"
                                                        ToolTip="Select asset category" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                                    <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                                        OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset category" CausesValidation="false" />
                                                    <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; vertical-align: top" class="style2">
                                                    <asp:Label ID="Label90" runat="server" CssClass="label" Text="Asset Make : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:DropDownList ID="ddlAssetMake" Width="200px" TabIndex="5" runat="server" CssClass="dropdownlist"
                                                        AutoPostBack="true" ToolTip="Select asset make" OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: right; vertical-align: top">
                                                    <asp:Label ID="Label11" runat="server" CssClass="label" Text="Model Name : "></asp:Label>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:ListBox ID="lstModelName" ToolTip="Select one or more model names" TabIndex="6"
                                                        SelectionMode="Multiple" Width="200px" CssClass="textbox" runat="server"></asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:Label ID="lblAssetCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                </td>
                                                <td style="text-align: right; vertical-align: top" class="style1">
                                                    <asp:Button ID="btnSearch" TabIndex="17" runat="server" CssClass="button" Text="Get Assets"
                                                        OnClick="btnSearch_Click" Width="75px" ToolTip="Get a list of assets based on search criteria" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="gvAssets" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        PageSize="50" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" OnPageIndexChanging="gvAssets_PageIndexChanging"
                                                        AlternatingRowStyle-CssClass="alt" OnRowCreated="gvAssets_RowCreated">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
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
                                                            <asp:TemplateField HeaderText="Serial Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerialCode" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="FAMS ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetID" runat="server" Text='<%#Eval("ASSET_ID") %>'></asp:Label>
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
                                                            <asp:TemplateField HeaderText="WorkStation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblWorkStation" runat="server" Text='<%#Eval("WORKSTATION_NO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pgr"></PagerStyle>
                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                            ToolTip="Save vendor call log details" ImageUrl="~/images/Submit.png" Height="35px"
                                            Width="85px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                            ToolTip="Refresh/reset page fields" OnClick="btnClear_Click" ImageUrl="~/images/Reset.png"
                                            Height="35px" Width="75px" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <div id="dv" style="max-height: 350px; overflow: auto">
                        <asp:GridView ID="gvCallLog" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            OnPageIndexChanging="gvCallLog_PageIndexChanging" OnRowCommand="gvCallLog_RowCommand"
                            ShowFooter="false" PageSize="50" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:TemplateField HeaderText="Call Log No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVCLCode" runat="server" Text='<%#Eval("CALL_LOG_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Serial Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrlCode" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVendName" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact Person">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContPerson" runat="server" Text='<%#Eval("VENDOR_CONT_PERSON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Call No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCallNo" runat="server" Text='<%#Eval("CALL_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Call Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCallDate" runat="server" Text='<%#Eval("CALL_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Responded Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCommitDate" runat="server" Text='<%#Eval("RESPONDED_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Resolved Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResolvedDate" runat="server" Text='<%#Eval("RESOLVED_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Call Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCallStatus" runat="server" Text='<%#Eval("RESOLVED_STATUS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Call Log Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCLCode" runat="server" Text='<%#Eval("CALL_LOG_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAsstCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVendCode" runat="server" Text='<%#Eval("VENDOR_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View" HeaderStyle-Width="50px">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imagebuttonView" ToolTip="View" CommandName="View" ImageUrl="~/images/View_16x16.png"
                                            runat="server" CommandArgument='<%#Eval("CALL_LOG_CODE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="pgr"></PagerStyle>
                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
