<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="AssetDeAllocation.aspx.cs" Inherits="WebPages_AssetDeAllocation" Title="ATS - Asset De-Allocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        var TotalChkBx;
        var Counter;

        function ClearFields() {
            window.location.href = window.location;
        }
        function ShowAllocTypeMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Asset Allocate/Return Type is not selected.';
        }
        function ShowAllocatedDateMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Allocated Date should not be greater than current date.';
        }
        function ShowAllocatedReturnMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Allocated Date should be less than Expected Return Date.';
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : ' + msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowAlert(ShowMsg) {
            alert(ShowMsg.toString());
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .style1 {
            width: 113px;
        }

        .style2 {
            width: 110px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebPages/AutoComplete.asmx" />
        </Services>
    </act:ToolkitScriptManager>
    <div id="wrapper">
        <div id="pageTitle">
            Asset De-Allocation
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table2" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label52" Font-Bold="true" Text="* marked fields are mandatory. In bulk upload, please upload excel file with date in dd-MMM-yyyy format. Please Set the Cell as Date Format, do not Custom.  " CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table1" runat="server" cellspacing="15" style="width: 100%;" align="center">
                                <tr>

                                    <td style="text-align: left">
                                        <asp:Label ID="Label13" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label7" runat="server" CssClass="label" Text="Deallocation Date : "></asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtDeAllocateDate" TabIndex="18" runat="server" CssClass="textbox"
                                            onfocus="showCalendarControl(this);" Text="" ToolTip="Enter asset's expected return date"
                                            Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtDeAllocateDate"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: left" class="all-asset-display-none Facilities-display">
                                        <a href="DownloadSample/FacilitiesDeallocation.xlsx">
                                            <input type="button" class="button btn btn-secondary" value="Download Template" />
                                        </a>
                                    </td>
                                    <td style="text-align: left" class="all-asset-display-none IT-display">
                                        <a href="DownloadSample/ITDeallocation.xlsx">
                                            <input type="button" class="button btn btn-secondary" value="Download Template" />
                                        </a>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label28" runat="server" Text="Select Asset Deallocation Data File :" CssClass="label"></asp:Label>
                                        </td>
                                        <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                            <ContentTemplate>
                                                <td style="text-align: left">
                                                    <asp:FileUpload ID="AssetFileUpload" ToolTip="Browse excel file through here for asset data import"
                                                        CssClass="textbox" runat="server" />
                                                    &nbsp; 
                                                        <asp:Button ID="btnUploadFile" runat="server" Text="Upload File" CssClass="button"
                                                            TabIndex="15" Width="150px" CausesValidation="true" ToolTip="Upload File"
                                                            OnClick="btnUploadFile_Click" />
                                                </td>
                                                <%--<td style="text-align: left">--%>

                                                <%--</td>--%>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnUploadFile" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <table id="tblAssets" runat="server" cellspacing="12" width="100%" style="border: 2px double #006600;"
                                            align="center">
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="Label17" Font-Bold="true" Text="To search the allocated Asset for deallocation..." CssClass="ErrorLabel"
                                                        runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label1" runat="server" CssClass="label" Text="Asset Code : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetCode" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 5 characters of asset code to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx12" ID="AutoCompleteExtender12"
                                                        TargetControlID="txtAssetCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetAllocatedAssetValue"
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
                                                                        var behavior = $find('AutoCompleteEx12');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx12')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx12')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtAssetCode"
                                                        WatermarkText="Search Asset code..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label2" runat="server" Enabled="true" CssClass="label" Text="RFID Tag : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtRFIDTag" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 5 characters of RFID tag id to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx13" ID="AutoCompleteExtender13"
                                                        TargetControlID="txtRFIDTag" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetRFIDTag"
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
                                                                        var behavior = $find('AutoCompleteEx13');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx13')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx13')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender7" runat="server" TargetControlID="txtRFIDTag"
                                                        WatermarkText="Search Asset RFID Tag..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr class="all-asset-display-none IT-display">
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label3" runat="server" CssClass="label" Text="Employee Id : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtEmployeeId" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 5 characters of txtEmployeeId to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx14" ID="AutoCompleteExtender14"
                                                        TargetControlID="txtEmployeeId" ServicePath="AutoComplete.asmx" ServiceMethod="GetEmployeeId"
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
                                                                        var behavior = $find('AutoCompleteEx14');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx14')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx14')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender8" runat="server" TargetControlID="txtEmployeeId"
                                                        WatermarkText="Search Employee Id..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label4" runat="server" Enabled="true" CssClass="label" Text="Employee name : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtEmployeeName" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 5 characters of asset Model to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx15" ID="AutoCompleteExtender15"
                                                        TargetControlID="txtEmployeeName" ServicePath="AutoComplete.asmx" ServiceMethod="GetEmployeeName"
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
                                                                        var behavior = $find('AutoCompleteEx15');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx15')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx15')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender9" runat="server" TargetControlID="txtEmployeeName"
                                                        WatermarkText="Search Employee Name..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label5" runat="server" CssClass="label" Text="Floor : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtFloor" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 5 characters of Floor to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx16" ID="AutoCompleteExtender16"
                                                        TargetControlID="txtFloor" ServicePath="AutoComplete.asmx" ServiceMethod="GetFloor"
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
                                                                        var behavior = $find('AutoCompleteEx16');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx16')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx16')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender10" runat="server" TargetControlID="txtFloor"
                                                        WatermarkText="Search Asset Floor..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label6" runat="server" Enabled="true" CssClass="label" Text="Seat No : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtSeatNo" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of Seat No to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx17" ID="AutoCompleteExtender17"
                                                        TargetControlID="txtSeatNo" ServicePath="AutoComplete.asmx" ServiceMethod="GetSeatNo"
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
                                                                        var behavior = $find('AutoCompleteEx17');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx17')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx17')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender11" runat="server" TargetControlID="txtSeatNo"
                                                        WatermarkText="Search Seat No..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr class="all-asset-display-none IT-display">
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label8" runat="server" CssClass="label" Text="Designation : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtDesignation" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 5 characters of Designation to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx18" ID="AutoCompleteExtender18"
                                                        TargetControlID="txtDesignation" ServicePath="AutoComplete.asmx" ServiceMethod="GetDesignation"
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
                                                                        var behavior = $find('AutoCompleteEx18');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx18')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx18')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender12" runat="server" TargetControlID="txtDesignation"
                                                        WatermarkText="Search Designation..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label9" runat="server" Enabled="true" CssClass="label" Text="Process Name : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtProcessName" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 5 characters of Process Name to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx19" ID="AutoCompleteExtender19"
                                                        TargetControlID="txtProcessName" ServicePath="AutoComplete.asmx" ServiceMethod="GetProcessName"
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
                                                                        var behavior = $find('AutoCompleteEx19');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx19')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx19')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender13" runat="server" TargetControlID="txtProcessName"
                                                        WatermarkText="Search Process Name..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label18" runat="server" CssClass="label" Text="Asset Make : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetMake" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Make to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx29" ID="AutoCompleteExtender29"
                                                        TargetControlID="txtAssetMake" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetMake"
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
                                                                        var behavior = $find('AutoCompleteEx29');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx29')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx29')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAssetMake"
                                                        WatermarkText="Search Asset Make..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label19" runat="server" Enabled="true" CssClass="label" Text="Asset Model : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetModel" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Model to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx20" ID="AutoCompleteExtender20"
                                                        TargetControlID="txtAssetModel" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetModel"
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
                                                                        var behavior = $find('AutoCompleteEx20');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx20')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx20')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtAssetModel"
                                                        WatermarkText="Search Asset Model..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr class="all-asset-display-none IT-display">
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label35" runat="server" CssClass="label" Text="Asset Domain : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetDomain" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Domain to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx94" ID="AutoCompleteExtender94"
                                                        TargetControlID="txtAssetDomain" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetDomain"
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
                                                                        var behavior = $find('AutoCompleteEx94');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx94')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx94')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender94" runat="server" TargetControlID="txtAssetDomain"
                                                        WatermarkText="Search Asset Domain..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label36" runat="server" Enabled="true" CssClass="label" Text="Asset HDD : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetHDD" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset HDD to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx95" ID="AutoCompleteExtender95"
                                                        TargetControlID="txtAssetHDD" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetHDD"
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
                                                                        var behavior = $find('AutoCompleteEx95');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx95')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx95')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender95" runat="server" TargetControlID="txtAssetHDD"
                                                        WatermarkText="Search Asset HDD..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr class="all-asset-display-none IT-display">
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label12" runat="server" CssClass="label" Text="Asset Processor : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetProcessor" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Processor to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx92" ID="AutoCompleteExtender92"
                                                        TargetControlID="txtAssetProcessor" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetProcessor"
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
                                                                        var behavior = $find('AutoCompleteEx92');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx92')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx92')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender92" runat="server" TargetControlID="txtAssetProcessor"
                                                        WatermarkText="Search Asset Processor..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label15" runat="server" Enabled="true" CssClass="label" Text="Asset RAM : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetRAM" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset RAM to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx93" ID="AutoCompleteExtender93"
                                                        TargetControlID="txtAssetRAM" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetRAM"
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
                                                                        var behavior = $find('AutoCompleteEx93');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx93')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx93')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender93" runat="server" TargetControlID="txtAssetRAM"
                                                        WatermarkText="Search Asset RAM..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label37" runat="server" CssClass="label" Text="PO Number : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtPONumber" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of PO Number to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx96" ID="AutoCompleteExtender96"
                                                        TargetControlID="txtPONumber" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetPONo"
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
                                                                        var behavior = $find('AutoCompleteEx96');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx96')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx96')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender96" runat="server" TargetControlID="txtPONumber"
                                                        WatermarkText="Search PO Number..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label38" runat="server" Enabled="true" CssClass="label" Text="Vendor : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetVendor" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of Vendor to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx97" ID="AutoCompleteExtender97"
                                                        TargetControlID="txtAssetVendor" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetVendor"
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
                                                                        var behavior = $find('AutoCompleteEx97');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx97')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx97')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender97" runat="server" TargetControlID="txtAssetVendor"
                                                        WatermarkText="Search Vendor..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label20" runat="server" Text="Asset Type :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetType" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Type to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx21" ID="AutoCompleteExtender21"
                                                        TargetControlID="txtAssetType" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetType"
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
                                                                        var behavior = $find('AutoCompleteEx21');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx21')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx21')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" TargetControlID="txtAssetType"
                                                        WatermarkText="Search Asset Type..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right; vertical-align: top">
                                                    <asp:Label ID="Label90" runat="server" CssClass="label" Text="Site Location :"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:DropDownList ID="ddlSiteLocationFilter" Width="200px" runat="server" AutoPostBack="true"
                                                        CssClass="dropdownlist"
                                                        TabIndex="3" ToolTip="Select Location">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label11" runat="server" Text="Asset ID :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetID" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of Asset id to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx25" ID="AutoCompleteExtender25"
                                                        TargetControlID="txtAssetID" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetID"
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
                                                                        var behavior = $find('AutoCompleteEx25');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx25')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx25')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender15" runat="server" TargetControlID="txtAssetID"
                                                        WatermarkText="Search Asset ID..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label10" runat="server" Text="Asset Sub Status :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetSubStatus" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of txtAsset Sub Status to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx24" ID="AutoCompleteExtender24"
                                                        TargetControlID="txtAssetSubStatus" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetSubStatus"
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
                                                                        var behavior = $find('AutoCompleteEx24');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx24')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx24')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender14" runat="server" TargetControlID="txtAssetSubStatus"
                                                        WatermarkText="Search Asset SubStatus..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr class="all-asset-display-none IT-display">
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label23" runat="server" Text="Asset SerialNo :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetSerialNo" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of Asset serial Number to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx23" ID="AutoCompleteExtender23"
                                                        TargetControlID="txtAssetSerialNo" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetSerialNumber"
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
                                                                        var behavior = $find('AutoCompleteEx23');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx23')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx23')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server" TargetControlID="txtAssetSerialNo"
                                                        WatermarkText="Search Asset Serial No..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label39" runat="server" Enabled="true" CssClass="label" Text="Invoice No : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetInvoiceNo" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of Vendor to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx98" ID="AutoCompleteExtender98"
                                                        TargetControlID="txtAssetInvoiceNo" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetInvoiceNo"
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
                                                                        var behavior = $find('AutoCompleteEx98');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx98')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx98')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender98" runat="server" TargetControlID="txtAssetInvoiceNo"
                                                        WatermarkText="Search Invoice No..." WatermarkCssClass="watermarked" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label41" runat="server" Enabled="true" CssClass="label" Text="GRN No : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtGRNNumber" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of GRN No to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx100" ID="AutoCompleteExtender100"
                                                        TargetControlID="txtGRNNumber" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetGRNNo"
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
                                                                        var behavior = $find('AutoCompleteEx100');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx100')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx100')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender100" runat="server" TargetControlID="txtGRNNumber"
                                                        WatermarkText="Search GRN No..." WatermarkCssClass="watermarked" />
                                                </td>

                                            </tr>
                                            <tr class="all-asset-display-none Facilities-display">

                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label32" runat="server" Text="Asset Far Tag :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetFarTag" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of Asset Far Tag to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx91" ID="AutoCompleteExtender91"
                                                        TargetControlID="txtAssetFarTag" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetFarTagforAllocation"
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
                                                                        var behavior = $find('AutoCompleteEx91');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx91')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx91')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender16" runat="server" TargetControlID="txtAssetFarTag"
                                                        WatermarkText="Search Asset Far Tag..." WatermarkCssClass="watermarked" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="center" colspan="6">
                                                    <asp:Button ID="btnAllSearch" runat="server" Text="Search" CssClass="button"
                                                        TabIndex="15" Width="150px" CausesValidation="true" ToolTip="Search Asset Details"
                                                        OnClick="btnAllSearch_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1"></td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblAssetCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                </td>

                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" runat="server" cellspacing="15" style="width: 100%;" align="center">
                                <tr>
                                    <td colspan="4">
                                        <div style="width: 1460px; overflow: auto">
                                            <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                ShowFooter="false" CssClass="mGrid" Visible="false"
                                                PagerStyle-CssClass="pgr" OnRowCreated="GridView2_RowCreated"
                                                AlternatingRowStyle-CssClass="alt"
                                                PageSize="500">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkHSelect" runat="server"
                                                                Text=" Select" AutoPostBack="true" OnCheckedChanged="OnCheckedHeaderChanged" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectAsset" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Serial No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSrno" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Tag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssettag" runat="server" Text='<%#Eval("ASSET_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RFID Tag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRfTag" runat="server" Text='<%#Eval("Tag_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Make">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmk" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Model" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblam" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Far Tag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFarTag" runat="server" Text='<%#Eval("ASSET_FAR_TAG") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblat" runat="server" Text='<%#Eval("ASSET_TYPE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAs" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Sub Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAss" runat="server" Text='<%#Eval("ASSET_SUB_STATUS") %>'></asp:Label>
                                                            <asp:HiddenField ID="hidESubStatus" Visible="false" runat="server" Value='<%#Eval("ASSET_SUB_STATUS") %>' />
                                                            <asp:DropDownList ID="ddlESubStatus" CssClass="dropdownlist" Width="120px"
                                                                runat="server" Visible="false">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                                            <asp:TextBox autocomplete="off" ID="txtERemarks" CssClass="textbox" Width="300px"
                                                                runat="server" Visible="false"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Site Location">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSite" runat="server" Text='<%#Eval("ASSET_LOCATION") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Floor">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFloor" runat="server" Text='<%#Eval("FLOOR") %>'></asp:Label>
                                                            <asp:HiddenField ID="hidEFloor" Visible="false" runat="server" Value='<%#Eval("FLOOR") %>' />
                                                            <asp:DropDownList ID="ddlEFloor" CssClass="dropdownlist" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlEFloor_SelectedIndexChanged"
                                                                runat="server" Visible="false">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgridStore" runat="server" Text='<%#Eval("STORE") %>'></asp:Label>
                                                            <asp:HiddenField ID="hidEStore" Visible="false" runat="server" Value='<%#Eval("STORE") %>' />
                                                            <asp:DropDownList ID="ddlEStore" CssClass="dropdownlist" Width="120px" 
                                                                runat="server" Visible="false">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="HDD" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHdd" runat="server" Text='<%#Eval("ASSET_HDD") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RAM" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRam" runat="server" Text='<%#Eval("ASSET_RAM") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PROCESSOR" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProcessor" runat="server" Text='<%#Eval("ASSET_PROCESSOR") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Host Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHostName" runat="server" Text='<%#Eval("ASSET_HOST_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Allocation Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAllocationType" runat="server" Text='<%#Eval("ALLOCATION_TYPE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Allocation Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAllocationDate" runat="server" Text='<%#Eval("ASSET_ALLOCATION_DATE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Employee Tag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmployeeTag" runat="server" Text='<%#Eval("EMP_TAG") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Expected Return Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblExpectedReturnDate" runat="server" Text='<%#Eval("EXPECTED_RTN_DATE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStore" runat="server" Text='<%#Eval("STORE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Employee Floor">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmployeeFloor" runat="server" Text='<%#Eval("Emp_Floor") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Seat No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSeatNo" runat="server" Text='<%#Eval("SEAT_NO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Service Ticket No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceTicket" runat="server" Text='<%#Eval("SERVICE_NOW_TICKET_NO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Employee ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmployeeID" runat="server" Text='<%#Eval("EMP_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Employee Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("EMP_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Designation" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesignation" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Identifier Location">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIdentifierLocation" runat="server" Text='<%#Eval("Identifier_Location") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Process Name" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProcessName" runat="server" Text='<%#Eval("PROCESS_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <PagerStyle CssClass="pgr"></PagerStyle>
                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save Asset De Allocation" CssClass="button"
                                            TabIndex="26" Width="150px" ToolTip="Save Asset Allocation Details" Enabled="false" ValidationGroup="Submit"
                                            OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" OnClientClick="ClearFields();" Text="Reset" CssClass="button"
                                            TabIndex="27" Width="60px" CausesValidation="false" ToolTip="Refresh/Reset fields"
                                            OnClick="btnClear_Click" />
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
                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>


        </table>
        <table>
            <tr>
                <td colspan="2" style="text-align: right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Report"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
