<%@ Page Title="BCIL - ATS : Asset Reaplcament" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="AssetReplacement.aspx.cs" Inherits="AssetReplacement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            window.location.href = window.location;
           <%-- document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtActiveInAssetCode.ClientID%>').value = "";
            document.getElementById('<%= txtSecurityGENo.ClientID%>').value = "";
            document.getElementById('<%= txtSecurityGEDate.ClientID%>').value = "";
            document.getElementById('<%= txtSerialCode.ClientID%>').value = "";
          
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
        function noBack() {
            window.history.forward(1);
        }
        function ShowAlertN(ShowMsg) {
            alert(ShowMsg.toString());
        }
    </script>

    <style type="text/css">
        .style1 {
            width: 270px;
        }

        .style2 {
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
            Asset Replacement
        </div>
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
                                                <tr class="all-asset-display-none IT-display">
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label13" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                        <asp:Label ID="Label5" runat="server" Text="Asset Serial No" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :
                                                        </div>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtActiveInAssetCode" autocomplete="off" CssClass="textbox" MaxLength="30"
                                                            ValidationGroup="Submit" runat="server" Width="200px" TabIndex="1" ToolTip="Enter first 8 characters of Serial Number to search"></asp:TextBox>
                                                        <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                                                            TargetControlID="txtActiveInAssetCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetCode"
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
                                                        <act:TextBoxWatermarkExtender ID="TBWME1" runat="server" TargetControlID="txtActiveInAssetCode"
                                                            WatermarkText="Search Serial Number..." WatermarkCssClass="watermarked" />
                                                        &nbsp;
                                                        <asp:ImageButton ID="btnGo" runat="server" TabIndex="2" ImageUrl="~/images/go_button.png"
                                                            OnClick="btnGo_Click" Height="25px" Width="25px" ToolTip="Enter asset code and click go button to get asset details" />
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label14" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                        <asp:Label ID="Label8" runat="server" Text="New Serial No." CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :
                                                        </div>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtSerialCode" autocomplete="off" CssClass="textbox" MaxLength="30"
                                                            runat="server" Width="200px" TabIndex="5" ToolTip="Enter Asset Code"></asp:TextBox>

                                                    </td>
                                                </tr>

                                                 <tr class="all-asset-display-none Facilities-display">
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label4" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                        <asp:Label ID="Label6" runat="server" Text="Asset Far Tag" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :
                                                        </div>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtAssetFARTagOld" autocomplete="off" CssClass="textbox" MaxLength="30"
                                                            ValidationGroup="Submit" runat="server" Width="200px" TabIndex="1" ToolTip="Enter first 8 characters of Asset Far Tag to search"></asp:TextBox>
                                                        <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx111" ID="AutoCompleteExtender111"
                                                            TargetControlID="txtAssetFARTagOld" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetFarTagforAllocation"
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
                                                                            var behavior = $find('AutoCompleteEx111');
                                                                            if (!behavior._height) {
                                                                                var target = behavior.get_completionList();
                                                                                behavior._height = target.offsetHeight - 2;
                                                                                target.style.height = '0px';
                                                                            }" />
                                                                        <Parallel Duration=".4">
                                                                            <FadeIn />
                                                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx111')._height" />
                                                                        </Parallel>
                                                                    </Sequence>
                                                                </OnShow>
                                                                <OnHide>
                                                                    <Parallel Duration=".4">
                                                                        <FadeOut />
                                                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx111')._height" EndValue="0" />
                                                                    </Parallel>
                                                                </OnHide>
                                                            </Animations>
                                                        </act:AutoCompleteExtender>
                                                        <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender111" runat="server" TargetControlID="txtAssetFARTagOld"
                                                            WatermarkText="Search Asset Far Tag..." WatermarkCssClass="watermarked" />
                                                        &nbsp;
                                                        <asp:ImageButton ID="btnGoAssetFarTag" runat="server" TabIndex="2" ImageUrl="~/images/go_button.png"
                                                             Height="25px" Width="25px" ToolTip="Enter Asset Far Tag and click go button to get asset details" OnClick="btnGoAssetFarTag_Click" />
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label10" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                        <asp:Label ID="Label11" runat="server" Text="New Asset Far Tag." CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :
                                                        </div>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtAssetFarTagNew" autocomplete="off" CssClass="textbox" MaxLength="30"
                                                            runat="server" Width="200px" TabIndex="5" ToolTip="Enter Asset Far Tag"></asp:TextBox>

                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td style="text-align: left">
                                                        <%--<asp:Label ID="Label4" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>--%>
                                                        <asp:Label ID="Label7" runat="server" Text="Document No." CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :
                                                        </div>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtSecurityGENo" autocomplete="off" CssClass="textbox" MaxLength="30"
                                                            ValidationGroup="Submit" runat="server" Style="margin-left: 0px !important;" Width="200px" TabIndex="4" ToolTip="Enter document no."></asp:TextBox>
                                                       <%-- <asp:RequiredFieldValidator ID="rfvSecurityGENo" runat="server" ControlToValidate="txtSecurityGENo" CssClass="validation" ValidationGroup="Submit" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label16" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                        <asp:Label ID="Label1" runat="server" Text="Replacement Date" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :
                                                        </div>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtSecurityGEDate" autocomplete="off" CssClass="textbox" MaxLength="30"
                                                            onfocus="showCalendarControl(this);" ValidationGroup="Submit" runat="server"
                                                            Width="200px" TabIndex="3" ToolTip="Enter/select security gate entry date"></asp:TextBox>
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label3" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                        <asp:Label ID="Label2" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :
                                                        </div>
                                                    </td>
                                                    <td style="text-align: left" colspan="4">
                                                        <asp:TextBox autocomplete="off" ID="txtRemarks" ToolTip="Enter Remarks" runat="server"
                                                            CssClass="multitextbox" TabIndex="6" Height="100px" Width="600px" TextMode="MultiLine"
                                                            MaxLength="1000"></asp:TextBox>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label9" runat="server" Text="File Upload" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :
                                                        </div>
                                                    </td>
                                                    <td style="text-align: left" colspan="4">
                                                        <asp:FileUpload ID="AssetFileUpload" ToolTip="Browse excel file through here for asset data import" TabIndex="7"
                                                            CssClass="textbox" runat="server" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:GridView ID="gvAssets" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            ShowFooter="false" Visible="false" CssClass="mGrid"
                                                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                            PageSize="50">
                                                            <Columns>
                                                                <asp:TemplateField></asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Asset Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RFID TAG">
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
                                                                        <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("VENDOR_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Vendor Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("VENDOR_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Invoice No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("INVOICE_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Serial No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Asset Far Tag">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAssetFarTag" runat="server" Text='<%#Eval("ASSET_FAR_TAG") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("STATUS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Sub Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSubStatus" runat="server" Text='<%#Eval("ASSET_SUB_STATUS") %>'></asp:Label>
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

                                <%--<tr class="all-asset-display-none IT-display">
                                    <td style="text-align: center">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSerialCode"
                                            Display="Dynamic" ErrorMessage="[Active In Serial No. Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtActiveInAssetCode"
                                            Display="Dynamic" ErrorMessage="[Active In Asset Code Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSecurityGEDate"
                                            Display="Dynamic" ErrorMessage="[Active In Entry Date Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRemarks"
                                            Display="Dynamic" ErrorMessage="[Asset Replacement Remarks Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class="all-asset-display-none Facilities-display">
                                    <td style="text-align: center">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAssetFarTagNew"
                                            Display="Dynamic" ErrorMessage="[New Asset Far Tag. Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtAssetFARTagOld"
                                            Display="Dynamic" ErrorMessage="[old Asset Far Tag. is Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtSecurityGEDate"
                                            Display="Dynamic" ErrorMessage="[Active In Entry Date Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtRemarks"
                                            Display="Dynamic" ErrorMessage="[Asset Replacement Remarks Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save Replacement Details" CssClass="button" TabIndex="11"
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

                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvAssetReplacement" runat="server" AutoGenerateColumns="false" Width="100%"
                                    PageSize="50" AllowPaging="true" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvAssetReplacement_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ACTIVE_IN_ASSET_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Asset Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_TAG") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Faulty Serial Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%#Eval("FAULTY_OUT_SERIAL_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Faulty Asset Far Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFaultyAssetFarTag" runat="server" Text='<%#Eval("FAULTY_OUT_ASSET_FAR_TAG") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Document No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblASGENo" runat="server" Text='<%#Eval("DOCUMENT_Number") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Asset Far Tag">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAssetFarTag" runat="server" Text='<%#Eval("ASSET_FAR_TAG") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("STATUS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Sub Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSubStatus" runat="server" Text='<%#Eval("ASSET_SUB_STATUS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Replacement Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblASGEDate" runat="server" Text='<%#Eval("REPLACEMENT_DATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
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
            </tr>
            <tr>
                <td colspan="4" style="text-align: right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export assets replacement details into excel file" Visible="false"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
