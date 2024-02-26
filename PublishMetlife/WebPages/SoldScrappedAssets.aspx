<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="SoldScrappedAssets.aspx.cs" Inherits="WebPages_SoldScrappedAssets" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ClearFields() {
            window.location.href = window.location;
            <%--document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtActiveInAssetCode.ClientID%>').value = "";
           
            document.getElementById('<%= txtSecurityGEDate.ClientID%>').value = "";
            document.getElementById('<%= txtRemarks.ClientID%>').value = "";
             document.getElementById('<%= gvAssets.ClientID%>').visible = false;
            document.getElementById('<%= txtActiveInAssetCode.ClientID%>').focus();--%>
        }
       
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : ' + msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to execute this operation!');
        }
        function ShowAlertMessage(Message) {
            alert(Message);
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }
        function ShowAlertNew(Message) {
            alert(Message);
            <%--document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();--%>
        }
        function noBack() {
            window.history.forward(1);
        }
        <%--function HeaderViewClick(CheckBox) {
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
        }--%>
        function HeaderViewClick(CheckBox) {
            debugger;
            var n1 = 0;
            var gridgv = document.getElementById('<%=gvAssets.ClientID %>');
            var rowCount = gridgv.rows.length - 1;
            var TargetBaseControl = document.getElementById('<%= this.gvAssets.ClientID %>');
            var TargetChildControl = "chkSelectAsset";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' &&
                    Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    var row = Inputs[n].parentNode.parentNode;
                    var temp = row.cells[2].innerText;
                    var temp1 = row.cells[3].innerText;
                    temp1 = temp1.toString().toUpperCase();
                    if (temp=='STOCK') {
                        Inputs[n].checked = CheckBox.checked;
                        n1++;
                    }
                    else {
                        /*CheckBox.checked = false;*/
                    }
                }
            }
            if (n1 != rowCount) { CheckBox.checked = false; }
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }
        function ChildClick(CheckBox, HCheckBox) {
            debugger;
            Counter = parseInt("0");
            var TargetBaseControl = document.getElementById('<%=gvAssets.ClientID %>');
            var TargetChildControl = "chkSelectAsset";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var row = CheckBox.parentNode.parentNode;
            var checkStatus = row.cells[2].innerText;
            var temp1 = row.cells[3].innerText;
            temp1 = temp1.toString().toUpperCase();
            if (checkStatus == 'STOCK') {
                for (var n = 0; n < Inputs.length; ++n) {
                    if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                        if (Inputs[n].checked)
                            Counter++;
                    }
                }
                var gridgv = document.getElementById('<%=gvAssets.ClientID %>');
                TotalChkBx = gridgv.rows.length - 1;
                Counter = HCheckBox.checked ? TotalChkBx : Counter;
                var HeaderCheckBox = document.getElementById(HCheckBox);
                if (Counter < TotalChkBx)
                    HeaderCheckBox.checked = false;
                else if (Counter == TotalChkBx)
                    HeaderCheckBox.checked = true;
            }
            else {
                CheckBox.checked = false;
                alert("Status not belongs to Stock");
                return false;
            }

        }
    </script>
     <style type="text/css">
        .style1
        {
            width: 270px;
        }
        .style2
        {
            width: 200px;
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
            Asset Scrap</div>
    </div>
    <div id="wrapper1" onload="noBack();">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4" style="text-align: left">
                    <asp:Label ID="Label15" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%" align="center" runat="server">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlActive" CssClass="panel" runat="server" GroupingText="Active In Asset">
                                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="15" align="center">
                                                <tr>
                                                    <td>
                                                        <table id="Table23" runat="server" cellspacing="12" width="100%" style="border: 2px double #006600;"
                                            align="center">
                                                            <tr>
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label16" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" runat="server" Text="Scrapped Date" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtSecurityGEDate" autocomplete="off" CssClass="textbox" MaxLength="30"
                                                            onfocus="showCalendarControl(this);" ValidationGroup="Submit" runat="server"
                                                            Width="200px" TabIndex="4" ToolTip="Enter/select security gate entry date"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label3" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                        <asp:Label ID="Label2" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                     <td style="text-align: left" colspan="4">
                                                        <asp:TextBox autocomplete="off" ID="txtRemarks" ToolTip="Enter Remarks" runat="server"
                                                            CssClass="multitextbox" TabIndex="13" Height="100px" Width="600px" TextMode="MultiLine"
                                                            MaxLength="1000"></asp:TextBox>
                                                    </td>
                                                </tr>
                                    <%--            <table id="Table4" cellspacing="15" style="width: 100%;" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label31" runat="server" Text="Mail To Address :" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtToAddress" runat="server" autocomplete="off" CssClass="textbox" MaxLength="250"  ToolTip="To Address" Width="200px"></asp:TextBox>                                        
                                    </td>
                                     
                                    <td style="text-align: right">
                                        <asp:Label ID="Label33" runat="server" Text="Mail Subject :" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtMailSubject" runat="server" autocomplete="off" CssClass="textbox" MaxLength="250"  ToolTip="Mail subject" Width="200px"></asp:TextBox>                                        
                                    </td>
                                     <td style="text-align: right">
                                        <asp:Label ID="Label30" runat="server" Text="Mail Body :" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtMailBody" runat="server" autocomplete="off" CssClass="multitextbox" MaxLength="500"
                                            Height="50px" Width="370px" TextMode="MultiLine"
                                            ToolTip="Mail subject"></asp:TextBox>                                        
                                    </td>
                                </tr>
                        </table>--%>
                                                    <tr>
                                    <td>                                            
                                                    <td style="text-align: right">
                                                        <asp:Label ID="Label28" runat="server" Text="Select Asset Scrapping Data File :" CssClass="label"></asp:Label>
                                                    </td>
                                                  <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                            <ContentTemplate>
                                                    <td style="text-align: center">
                                                        <asp:FileUpload ID="AssetFileUpload" ToolTip="Browse excel file through here for asset data import"
                                                            CssClass="textbox" runat="server" />
                                                    </td>
                                                    <td style="text-align: left">
                                                         <asp:Button ID="btnUploadFile" runat="server" Text="Upload File" CssClass="button"
                                            TabIndex="15" Width="150px" CausesValidation="true" ToolTip="Upload File"
                                            OnClick="btnUploadFile_Click" />
                                                    </td>
                                       </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID = "btnUploadFile" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                                    <td style="text-align: center" class="all-asset-display-none Facilities-display">
                                                        <a href="DownloadSample/FacilitiesScrap.xlsx">
                                                            <input type="button" class="button btn btn-secondary" value="Download Template" />
                                                        </a>   
                                                    </td>
                                        <td style="text-align: center" class="all-asset-display-none IT-display">
                                                        <a href="DownloadSample/ITScrap.xlsx">
                                                            <input type="button" class="button btn btn-secondary" value="Download Template" />
                                                        </a>   
                                                    </td>
                                    </td>
                                </tr>
                                                            </table>
                                                    </td>
                                                </tr>
                                                
                                                    <tr>
                                    <td colspan="12" align="center">
                                        <table id="tblAssets" runat="server" cellspacing="12" width="100%" style="border: 2px double #006600;"
                                            align="center">
                                             <tr>
                <td colspan="4">
                    <asp:Label ID="Label17" Font-Bold="true" Text="To search the fresh Asset for scrapping..." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label18" runat="server" CssClass="label" Text="Asset Make : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetMake" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Make to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="AutoCompleteExtender2"
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAssetMake"
                                                        WatermarkText="Search Asset Make..." WatermarkCssClass="watermarked" />                                                   
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label19" runat="server" Enabled="true" CssClass="label" Text="Asset Model : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetModel" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Model to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx4" ID="AutoCompleteExtender4"
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtAssetModel"
                                                        WatermarkText="Search Asset Model..." WatermarkCssClass="watermarked" />                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                               
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label20" runat="server" Text="Asset Type :" CssClass="label"></asp:Label>
                                                </td>
                                               <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetType" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Type to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx5" ID="AutoCompleteExtender5"
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
                                                                        var behavior = $find('AutoCompleteEx5');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx5')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx5')._height" EndValue="0" />
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
                                                   <asp:DropDownList ID="ddlSiteLocationFilter" Width="200px" runat="server"  AutoPostBack="true"
                                            CssClass="dropdownlist"
                                            TabIndex="3" ToolTip="Select Location">
                                        </asp:DropDownList>
                                                </td>
                                            </tr>

                                             <tr>

                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label30" runat="server" Text="Status :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAllocationStatus" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of status to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx89" ID="AutoCompleteExtender89"
                                                        TargetControlID="txtAllocationStatus" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetAllocationStatus"
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
                                                                        var behavior = $find('AutoCompleteEx89');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx89')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx89')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender7" runat="server" TargetControlID="txtAllocationStatus"
                                                        WatermarkText="Search Status..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right; vertical-align: top">
                                                    <asp:Label ID="Label31" runat="server" CssClass="label" Text="Sub Status :"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">

                                                    <asp:TextBox ID="txtAllocationSubStatus" runat="server" CssClass="textbox" AutoComplete="off" Enabled="true" AutoPostBack="true"
                                                        Width="200px" TabIndex="9" MaxLength="50" ToolTip="Enter first 3 characters of Sub Status to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx90" ID="AutoCompleteExtender90"
                                                        TargetControlID="txtAllocationSubStatus" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetSubStatus"
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
                                                var behavior = $find('AutoCompleteEx90');
                                                if (!behavior._height) {
                                                    var target = behavior.get_completionList();
                                                    behavior._height = target.offsetHeight - 2;
                                                    target.style.height = '0px';
                                                }" />
                                            <Parallel Duration=".4">
                                                <FadeIn />
                                                <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx90')._height" />
                                            </Parallel>
                                        </Sequence>
                                    </OnShow>
                                    <OnHide>
                                        <Parallel Duration=".4">
                                            <FadeOut />
                                            <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx90')._height" EndValue="0" />
                                        </Parallel>
                                    </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender8" runat="server" TargetControlID="txtAllocationSubStatus"
                                                        WatermarkText="Search Substatus..." WatermarkCssClass="watermarked" />
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
                                                    <asp:Label ID="Label9" runat="server" CssClass="label" Text="Asset Processor : "></asp:Label>
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
                                                    <asp:Label ID="Label6" runat="server" Enabled="true" CssClass="label" Text="Asset RAM : "></asp:Label>
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

                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label40" runat="server" Text="RFID Tag :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtRFIDTag" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of RFID to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx99" ID="AutoCompleteExtender99"
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
                                                                        var behavior = $find('AutoCompleteEx99');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx99')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx99')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender99" runat="server" TargetControlID="txtRFIDTag"
                                                        WatermarkText="Search RFID Tag..." WatermarkCssClass="watermarked" />
                                                </td>

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
                                            <tr class="all-asset-display-none IT-display">
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label23" runat="server" Text="Asset SerialNo :" CssClass="label"></asp:Label>
                                                </td>
                                               <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetSerialNo" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of Asset serial Number to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx6" ID="AutoCompleteExtender6"
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
                                                                        var behavior = $find('AutoCompleteEx6');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx6')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx6')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server" TargetControlID="txtAssetSerialNo"
                                                        WatermarkText="Search Asset Serial No..." WatermarkCssClass="watermarked" />                                                   
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender9" runat="server" TargetControlID="txtAssetFarTag"
                                                        WatermarkText="Search Asset Far Tag..." WatermarkCssClass="watermarked" />
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                
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
                                                 <td align="center" colspan="6">
                                        <asp:Button ID="btnAllSearch" runat="server" Text="Search" CssClass="button"
                                            TabIndex="15" Width="150px" CausesValidation="true" ToolTip="Search Asset Details"
                                            OnClick="btnAllSearch_Click" />
                                            </td>
                                            </tr>

                                            <tr>
                                                <td class="style1">
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblAssetCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                </td>
                                             
                                            </tr>

                                  </table>
                                    </td>
                                </tr>
                                                 <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="gvAssets" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                         ShowFooter="false"  Visible="false" CssClass="mGrid" OnRowCreated="gvAssets_RowCreated"
                                                        PagerStyle-CssClass="pgr"  AlternatingRowStyle-CssClass="alt"
                                                        PageSize="50" OnPageIndexChanging="gvAssets_PageIndexChanging">
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
                                                            <asp:TemplateField HeaderText="Asset Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAs" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Sub Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAss" runat="server" Text='<%#Eval("ASSET_SUB_STATUS") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Asset SerialNo.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Far Tag">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFarTag" runat="server" Text='<%#Eval("ASSET_FAR_TAG") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="RFID Tag">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetID" runat="server" Text='<%#Eval("Tag_ID") %>'></asp:Label>
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
                                                          
                                                            <asp:TemplateField HeaderText="Vendor Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVendorCode" runat="server" Text='<%#Eval("VENDOR_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="Vendor Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="Invoice No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("INVOICE_NO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="Po No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("PO_NUMBER") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pgr"></PagerStyle>
                                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                                
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td style="text-align: center">
                                       
                                       
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtActiveInAssetCode"
                                            Display="Dynamic" ErrorMessage="[Active In Asset Code Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSecurityGEDate"
                                            Display="Dynamic" ErrorMessage="[Scrap date Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRemarks"
                                            Display="Dynamic" ErrorMessage="[Asset scrap Remarks Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save Scrapped Details" CssClass="button" TabIndex="11"
                                            Width="170px" ToolTip="Save asset replacement details" Enabled="false" ValidationGroup="Submit"
                                            OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" Height="35px" Width="85px" Text="Reset" CssClass="button"
                                            OnClientClick="ClearFields();" ToolTip="Reset/Clear fields" TabIndex="12" CausesValidation="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrorMsg" CssClass="ErrorLabel" Font-Bold="true" runat="server"></asp:Label>
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
               <%--<tr>
                <td colspan="4">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvAssetReplacement" runat="server" AutoGenerateColumns="false"
                                    PageSize="50" AllowPaging="true" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvAssetReplacement_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Asset Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_TAG") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      
                                       
                                        <asp:TemplateField HeaderText="Scrapped Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblASGEDate" runat="server" Text='<%#Eval("SCRAP_DATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CREATED_BY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAssetReplacement" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>--%>
          
            <tr>
                <td colspan="4" style="text-align: right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export error Scrap details into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

