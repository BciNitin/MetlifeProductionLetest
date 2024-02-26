<%@ Page Title="BCIL : ATS - Swap Assets" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="SwapAsset.aspx.cs" Inherits="SwapAsset" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID %>').value = "";
            document.getElementById('<%= txtAssetCode1.ClientID %>').value = "";
            document.getElementById('<%= txtSerialCode1.ClientID %>').value = "";
            document.getElementById('<%= txtPortNo1.ClientID %>').value = "";
            document.getElementById('<%= txtPortNo2.ClientID %>').value = "";
            document.getElementById('<%= txtAssetCode2.ClientID %>').value = "";
            document.getElementById('<%= txtSerialCode2.ClientID %>').value = "";
            document.getElementById('<%= txtRemarks.ClientID %>').value = "";
            document.getElementById('<%= ChkAllocatedAssets1.ClientID %>').checked = false;
            document.getElementById('<%= ChkAllocatedAssets2.ClientID %>').checked = false;
            document.getElementById('<%= txtAssetCode1.ClientID %>').focus();
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
            width: 160px;
        }
        .style2
        {
            width: 240px;
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
            Swapping Assets</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td align="center">
                                        <table id="tblAssets" runat="server" cellspacing="18" width="100%" style="border: 2px double #006600;"
                                            align="center">
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label9" runat="server" CssClass="label" Text="Asset Code : "></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetCode1" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 8 characters of the asset code to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                                                        TargetControlID="txtAssetCode1" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetCode"
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAssetCode1"
                                                        WatermarkText="Search Asset Code..." WatermarkCssClass="watermarked" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="btnGo1" runat="server" ImageUrl="~/images/go_button.png" OnClick="btnGo1_Click"
                                                        Height="25px" Width="25px" ToolTip="Enter asset code and click go button to get asset details" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label12" runat="server" CssClass="label" Text="Port No. : "></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox autocomplete="off" ID="txtPortNo1" runat="server" CssClass="textbox"
                                                        TabIndex="4" Width="200px" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label5" runat="server" CssClass="label" Text="Location : "></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:DropDownList ID="ddlAssetLocation1" ToolTip="Select Location Name" OnSelectedIndexChanged="ddlAssetLocation1_SelectedIndexChanged"
                                                        AutoPostBack="true" CssClass="dropdownlist" runat="server" Width="200px" TabIndex="3">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblLocLevel1" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                                    <asp:ImageButton ID="btnRefreshLocation1" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                                        OnClick="btnRefreshLocation1_Click" ToolTip="Refresh/reset location" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label1" runat="server" CssClass="label" Text="Serial Code : "></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox autocomplete="off" ID="txtSerialCode1" runat="server" CssClass="textbox"
                                                        TabIndex="4" Width="200px" MaxLength="50" ToolTip="Enter first 5 characters of the serial code to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx3" ID="AutoCompleteExtender3"
                                                        TargetControlID="txtSerialCode1" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetSerialNo"
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
                                                                        var behavior = $find('AutoCompleteEx3');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx3')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx3')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtSerialCode1"
                                                        WatermarkText="Search Serial Code..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblLocCode1" runat="server" CssClass="label" Text="0" Visible="false"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:CheckBox ID="ChkAllocatedAssets1" CssClass="label" Text="   Select Allocated Assets"
                                                        runat="server" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblAssetCount1" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Button ID="btnSearch1" TabIndex="17" Width="75px" runat="server" CssClass="button"
                                                        Text="Get Assets" OnClick="btnSearch1_Click" ToolTip="Search Assets For Swapping" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="gvAssetSwap1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        ShowFooter="false" CssClass="mGrid" OnPageIndexChanging="gvAssetSwap1_PageIndexChanging"
                                                        PagerStyle-CssClass="pgr" PageSize="20" AlternatingRowStyle-CssClass="alt">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelectAsset1" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAsset1" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="FAMS ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetID1" runat="server" Text='<%#Eval("ASSET_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Serial Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerial1" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Location">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLoc1" runat="server" Text='<%#Eval("ASSET_LOCATION") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Employee">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmp1" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Process">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDept1" runat="server" Text='<%#Eval("PROCESS_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="Location">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAllLoc1" runat="server" Text='<%#Eval("ASSET_LOCATION") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="Emp">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCode1" runat="server" Text='<%#Eval("ALLOCATED_EMP_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="Process">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAllProc1" runat="server" Text='<%#Eval("ALLOCATED_PROCESS") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="WS">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblWS1" runat="server" Text='<%#Eval("WORKSTATION_NO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Port No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPN1" runat="server" Text='<%#Eval("PORT_NO") %>'></asp:Label>
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
                                    <td align="center">
                                        <table id="Table3" runat="server" cellspacing="18" width="100%" style="border: 2px double #006600;"
                                            align="center">
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label2" runat="server" CssClass="label" Text="Asset Code : "></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetCode2" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 8 characters of the asset code to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2" ID="AutoCompleteExtender2"
                                                        TargetControlID="txtAssetCode2" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetCode"
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtAssetCode2"
                                                        WatermarkText="Search Asset Code..." WatermarkCssClass="watermarked" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="btnGo2" runat="server" ImageUrl="~/images/go_button.png" OnClick="btnGo2_Click"
                                                        Height="25px" Width="25px" ToolTip="Enter asset code and click go button to get asset details" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label4" runat="server" CssClass="label" Text="Port No. : "></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox autocomplete="off" MaxLength="50" ID="txtPortNo2" runat="server" CssClass="textbox"
                                                        TabIndex="4" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label6" runat="server" CssClass="label" Text="Location : "></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:DropDownList ID="ddlAssetLocation2" ToolTip="Select Location Name" OnSelectedIndexChanged="ddlAssetLocation2_SelectedIndexChanged"
                                                        AutoPostBack="true" CssClass="dropdownlist" runat="server" Width="200px" TabIndex="3">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblLocLevel2" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                                    <asp:ImageButton ID="btnRefreshLocation2" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                                        OnClick="btnRefreshLocation2_Click" ToolTip="Refresh/reset location" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label3" runat="server" CssClass="label" Text="Serial Code : "></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox autocomplete="off" ID="txtSerialCode2" runat="server" CssClass="textbox"
                                                        TabIndex="4" Width="200px" MaxLength="50" ToolTip="Enter first 5 characters of the serial code to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx4" ID="AutoCompleteExtender4"
                                                        TargetControlID="txtSerialCode2" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetSerialNo"
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" TargetControlID="txtSerialCode2"
                                                        WatermarkText="Search Serial Code..." WatermarkCssClass="watermarked" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblLocCode2" runat="server" CssClass="label" Text="0" Visible="false"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:CheckBox ID="ChkAllocatedAssets2" CssClass="label" Text="   Select Allocated Assets"
                                                        runat="server" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblAssetCount2" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Button ID="btnSearch2" Text="Get Assets" runat="server" Width="75px" OnClick="btnSearch2_Click"
                                                        CssClass="button" ToolTip="Search Assets For Swapping" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="gvAssetSwap2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        ShowFooter="false" CssClass="mGrid" OnPageIndexChanging="gvAssetSwap2_PageIndexChanging"
                                                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" PageSize="20">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelectAsset2" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAsset2" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="FAMS ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetID2" runat="server" Text='<%#Eval("ASSET_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Serial Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerial2" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Location">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLoc2" runat="server" Text='<%#Eval("ASSET_LOCATION") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Employee">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmp2" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Process">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDept2" runat="server" Text='<%#Eval("PROCESS_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="Location">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAllLoc1" runat="server" Text='<%#Eval("ASSET_LOCATION") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="Emp">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpCode2" runat="server" Text='<%#Eval("ALLOCATED_EMP_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="Dept">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAllProc2" runat="server" Text='<%#Eval("ALLOCATED_PROCESS") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="WS">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblWS2" runat="server" Text='<%#Eval("WORKSTATION_NO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Port No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPN2" runat="server" Text='<%#Eval("PORT_NO") %>'></asp:Label>
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
                                    <td style="vertical-align: top">
                                        <asp:Label ID="Label7" runat="server" CssClass="label" Text="Enter Swapping Date  :  "></asp:Label>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox autocomplete="off" Width="200px" ID="txtSwapDate" runat="server" CssClass="textbox"
                                            TabIndex="30" ToolTip="Enter/select swapping date" onfocus="showCalendarControl(this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Date" runat="server" CssClass="validation" ControlToValidate="txtSwapDate"
                                            Display="Dynamic" ErrorMessage="[Required]" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox autocomplete="off" Width="930px" ID="txtRemarks" runat="server" CssClass="multitextbox"
                                            TabIndex="31" Height="100px" ToolTip="Enter asset swapping remarks" TextMode="MultiLine"
                                            MaxLength="1000"></asp:TextBox>
                                        <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtRemarks"
                                            WatermarkText="Enter Swapping Remarks" WatermarkCssClass="watermarked" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRemarks"
                                            Display="Dynamic" ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" Text="Swap Assets" CssClass="button"
                                            TabIndex="11" Width="100px" ToolTip="Save asset swapping details"
                                            OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" OnClientClick="ClearFields();" Text="Reset" CssClass="button"
                                            TabIndex="12" Width="60px" ToolTip="Reset/Clear fields" CausesValidation="False"
                                            OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvSwapHistory" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    ShowFooter="false" CssClass="mGrid" OnPageIndexChanging="gvSwapHistory_PageIndexChanging"
                                    PagerStyle-CssClass="pgr" PageSize="50" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAsset1" runat="server" Text='<%#Eval("ASSET_CODE1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialCode1" runat="server" Text='<%#Eval("SERIAL_CODE1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoc2" runat="server" Text='<%#Eval("LOCATION2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Process">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcess2" runat="server" Text='<%#Eval("PROCESS2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WorkStation">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWS2" runat="server" Text='<%#Eval("WORKSTATION2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Port">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPort2" runat="server" Text='<%#Eval("PORT2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmp2" runat="server" Text='<%#Eval("EMPLOYEE_NAME2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAsset2" runat="server" Text='<%#Eval("ASSET_CODE2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialCode2" runat="server" Text='<%#Eval("SERIAL_CODE2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLoc1" runat="server" Text='<%#Eval("LOCATION1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Process">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcess1" runat="server" Text='<%#Eval("PROCESS1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WorkStation">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWS1" runat="server" Text='<%#Eval("WORKSTATION1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Port">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPort1" runat="server" Text='<%#Eval("PORT1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmp1" runat="server" Text='<%#Eval("EMPLOYEE_NAME1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmp1" runat="server" Text='<%#Eval("SWAP_DATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export asset swapping data into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
