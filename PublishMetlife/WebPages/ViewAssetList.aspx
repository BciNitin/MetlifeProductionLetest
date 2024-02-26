<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="ViewAssetList.aspx.cs" Inherits="ViewAssetList" Title="BCIL : ATS - View Assets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
          
          document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtAssetCode.ClientID%>').value = "";
       
            document.getElementById('<%= txtPONo.ClientID%>').value = "";
            document.getElementById('<%= ddlAssetMake.ClientID%>').selectedIndex = 0;
        
            document.getElementById('<%= lstModelName.ClientID%>').selectedIndex = -1;
            
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
          
            document.getElementById('<%= txtAssetCode.ClientID%>').focus();
            window.location.href = window.location;
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = "Please Note : You are not authorised to execute this operation!";
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
       
        function noBack() {
            window.history.forward(1);
        }
        var confirmed = false;
        function confirmDialog(obj) {
            if (!confirmed) {
                $("#dialog-confirm").dialog({
                    resizable: false,
                    height: 140,
                    modal: true,
                    buttons: {
                        "Yes": function() {
                            $(this).dialog("close");
                            confirmed = true; obj.click();
                        },
                        "No": function() {
                            $(this).dialog("close");
                        }
                    }
                });
            }
            return confirmed;
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 112px;
        }
    </style>
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
            View Assets List</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%;" align="center">
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label33" runat="server" Text="Serial No." CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:TextBox autocomplete="off" ID="txtAssetCode" runat="server" Text="" CssClass="textbox"
                        Width="200px" TabIndex="1" ToolTip="Enter first 5 characters of the asset code to search"></asp:TextBox>
                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                        TargetControlID="txtAssetCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetALLAssetCode"
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
                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAssetCode"
                        WatermarkText="Search Serial No..." WatermarkCssClass="watermarked" />
                    &nbsp;
                    <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/images/go_button.png" OnClick="btnGo_Click"
                        Height="25px" Width="25px" ToolTip="Enter asset code and click on go to get asset details" />
                </td>
                <%--<td style="text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="Serial Code" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:TextBox autocomplete="off" ID="txtSerialCode" runat="server" Text="" CssClass="textbox"
                        Width="200px" TabIndex="2" ToolTip="Enter first 5 characters of the serial code to search"></asp:TextBox>
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
                </td>--%>
            </tr>
            <tr>
                <%--<td style="text-align: right">
                    <asp:Label ID="Label8" runat="server" Text="FAMS ID" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:TextBox autocomplete="off" ID="txtFAMSId" runat="server" Text="" CssClass="textbox"
                        Width="200px" TabIndex="3" ToolTip="Enter first 4 characters of the FAMS Id to search"></asp:TextBox>
                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx3" ID="AutoCompleteExtender3"
                        TargetControlID="txtFAMSId" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetFAMSId"
                        MinimumPrefixLength="4" CompletionInterval="1000" EnableCaching="true" CompletionSetCount="20"
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
                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtFAMSId"
                        WatermarkText="Search FAMS ID..." WatermarkCssClass="watermarked" />
                </td>--%>
                <td style="text-align: right">
                    <asp:Label ID="Label9" runat="server" Text="PO No." CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:TextBox autocomplete="off" ID="txtPONo" runat="server" Text="" CssClass="textbox"
                        Width="200px" TabIndex="4" ToolTip="Enter first 5 characters of the port no. to search"></asp:TextBox>
                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx4" ID="AutoCompleteExtender4"
                        TargetControlID="txtPONo" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetPONo"
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
                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtPONo"
                        WatermarkText="Search PO No...." WatermarkCssClass="watermarked" />
                </td>
            </tr>
            <tr >
                <%--<td style="text-align: right">
                    <asp:Label ID="Label4" runat="server" Enabled="false" Text="Asset Type" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>--%>
               <%-- <td style="text-align: left">
                    <asp:DropDownList ID="ddlAssetType" Enabled="false" Width="200px" runat="server"
                        ToolTip="Select asset type" TabIndex="5" CssClass="dropdownlist" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                        <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                        <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                        <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblAssetType" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                </td>--%>
                <td style="text-align: right">
                    <asp:Label ID="Label5" runat="server" Text="Asset Type" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlAssetCategory" Width="200px" TabIndex="6" runat="server"
                        CssClass="dropdownlist" ToolTip="Select asset category" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                    <asp:ImageButton ID="btnRefreshCategory" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                        OnClick="btnRefreshCategory_Click" TabIndex="7" ToolTip="Refresh/reset category"
                        CausesValidation="false" />
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
                    <asp:DropDownList ID="ddlAssetMake" Width="200px" TabIndex="8" runat="server" CssClass="dropdownlist"
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
                <td style="text-align: left; vertical-align: top" rowspan="2">
                    <asp:ListBox ID="lstModelName" ToolTip="Select one or more model names" TabIndex="9"
                        SelectionMode="Multiple" Width="200px" CssClass="textbox" runat="server"></asp:ListBox>
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
                    <asp:DropDownList ID="ddlAssetLocation" Width="200px" TabIndex="10" runat="server"
                        AutoPostBack="true" ToolTip="Select asset location" OnSelectedIndexChanged="ddlAssetLocation_SelectedIndexChanged"
                        CssClass="dropdownlist">
                    </asp:DropDownList>
                    <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                    <asp:ImageButton ID="ibtnRefreshLocation" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                        OnClick="ibtnRefreshLocation_Click" TabIndex="11" ToolTip="Refresh/reset location"
                        CausesValidation="false" />
                    <asp:Label ID="lblLocCode" CssClass="label" Text="0" runat="server" Visible="false"></asp:Label>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
          <%--  <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label40" runat="server" CssClass="label" Text="AMC/Warranty"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlAMCWarranty" Width="200px" ToolTip="Select AMC/Warranty/None"
                        runat="server" TabIndex="12" CssClass="dropdownlist">
                        <asp:ListItem Text="-- Select --" Value="SELECT"></asp:ListItem>
                        <asp:ListItem Text="AMC" Value="AMC"></asp:ListItem>
                        <asp:ListItem Text="WARRANTY" Value="WARRANTY"></asp:ListItem>
                        <asp:ListItem Text="NONE" Value="NONE"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label6" runat="server" Text="Select Option" CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold">
                        :</div>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlVerNonVer" Width="200px" ToolTip="Select asset status" TabIndex="13"
                        runat="server" CssClass="dropdownlist">
                        <asp:ListItem Text="-- Select Option --" Value="SELECT"></asp:ListItem>
                        <asp:ListItem Text="Verifiable" Value="VER"></asp:ListItem>
                        <asp:ListItem Text="Non-Verifiable" Value="NVER"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>--%>
            <%--<tr>
                <td>
                </td>
                <td style="text-align: left">
                </td>
                <td style="text-align: left">
                    <asp:RadioButton ID="rdoApproved" runat="server" TabIndex="14" onClick="ApproveUnapprove(this);"
                        CssClass="label" Text=" Approved" ToolTip="Select to get a list of approved assets"
                        Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoUnApproved" ToolTip="Select to get a list of unapproved assets"
                        runat="server" TabIndex="15" onClick="ApproveUnapprove(this);" CssClass="label"
                        Text=" UnApproved" />
                </td>
                <td style="text-align: center">
                </td>
                <td style="text-align: left">
                </td>
                <td>
                    <asp:RadioButton ID="rdoScrapped" runat="server" TabIndex="16" onClick="SoldScrapped(this);"
                        CssClass="label" Text=" Scrapped" ToolTip="Select to get a list of scrapped assets" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoSold" runat="server" TabIndex="17" ToolTip="Select to get a list of sold assets"
                        onClick="SoldScrapped(this);" CssClass="label" Text=" Sold" />
                </td>
            </tr>--%>
            <tr>
                <td align="center" colspan="6">
                    <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ImageUrl="~/images/Submit.png"
                        TabIndex="18" Height="35px" Width="85px" ToolTip="Get a list of assets based on search criteria"
                        OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="~/images/Reset.png"
                        TabIndex="19" Height="35px" Width="85px" ToolTip="Reset/clear fields" CausesValidation="false"
                        OnClick="btnClear_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="20" Enabled="true" ToolTip="Export assets list into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: left">
                    <asp:Label ID="lblAssetCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="display:none;" visible="false">
                <td colspan="5">
                    <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:Button ID="btnBulkDeleteAssets" TabIndex="21" runat="server" Visible="false"
                        Width="120px" CssClass="button" Text="Bulk Delete Assets" OnClientClick="return confirm('Are you sure to delete assets in bulk?');"
                        OnClick="btnBulkDeleteAssets_Click" ToolTip="Delete assets in bulk as per search criteria" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvViewAsset" runat="server" AllowPaging="True" PageSize="50" AutoGenerateColumns="False"
                                    ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    OnPageIndexChanging="gvViewAsset_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Serial Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("Serial_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                             </asp:TemplateField>
                                           
                                        <asp:TemplateField HeaderText="RFID Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetID" runat="server" Text='<%#Eval("RFID_TAG") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="PO No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPONO" runat="server" Text='<%#Eval("PO_Number") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Invoice No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblinvoiceNo" runat="server" Text='<%#Eval("INVOICE_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Vendor ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorCode" runat="server" Text='<%#Eval("Vendor_Code") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                      <%--  <asp:TemplateField HeaderText="Serial Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetSrlCode" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Asset Make">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryName" runat="server" Text='<%#Eval("ASSET_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("Location") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField HeaderText="Process">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcess" runat="server" Text='<%#Eval("ASSET_PROCESS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       <%-- <asp:TemplateField HeaderText="Port No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPortNo" runat="server" Text='<%#Eval("PORT_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       <%-- <asp:TemplateField HeaderText="View" HeaderStyle-Width="50px">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imagebuttonView" ToolTip="View" CommandName="View" ImageUrl="~/images/View_16x16.png"
                                                    runat="server" CommandArgument='<%#Eval("ASSET_CODE") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       <%-- <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="50px">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" CommandName="Delete" ImageUrl="~/images/Delete_16x16.png"
                                                    runat="server" CommandArgument='<%#Eval("ASSET_CODE") %>' OnClientClick="return confirm('Are you sure to delete asset?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViewAsset" EventName="PageIndexChanging" />
                             <%--   <asp:AsyncPostBackTrigger ControlID="gvViewAsset" EventName="RowCommand" />
                                <asp:AsyncPostBackTrigger ControlID="gvViewAsset" EventName="RowDeleting" />--%>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
