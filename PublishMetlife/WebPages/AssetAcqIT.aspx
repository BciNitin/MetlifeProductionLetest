<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="AssetAcqIT.aspx.cs" Inherits="WebPages_AssetAcqIT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
    <script language="javascript" type="text/javascript">
        function ClearFields() {
            window.location.href = window.location;
        }
        function ShowSelfRptToMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowErrMsg(Message) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }
        function ShowAlert(Message) {
            alert(Message);
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }

        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .auto-style1 {
            height: 30px;
        }

        .auto-style2 {
            height: 27px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <%-- <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>--%>
     <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebPages/AutoComplete.asmx" />
        </Services>
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Asset Acquisition
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label11" Font-Bold="true" Text="* marked fields are mandatory. In bulk upload, please upload excel file with date in dd-MMM-yyyy format. Please Set the Cell as Date Format, do not Custom.  " CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right" class="auto-style2">&nbsp;</td>
                                    <td style="text-align: center" class="auto-style2">&nbsp;</td>
                                    <td style="text-align: left" class="auto-style2"></td>
                                    <td style="text-align: right" class="auto-style2">&nbsp;</td>
                                    <td style="text-align: center" class="auto-style2">&nbsp;</td>
                                    <td style="text-align: left" class="auto-style2"></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">&nbsp;</td>
                                    <td style="text-align: center">&nbsp;</td>
                                    <td style="text-align: left">&nbsp;</td>
                                    <td style="text-align: right">&nbsp;</td>
                                    <td style="text-align: center">&nbsp;</td>
                                    <td style="text-align: left">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <%--<asp:Label ID="Label1" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>--%>
                                        <asp:Label ID="Label7" runat="server" Text="Asset Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtAssetCode" runat="server" ReadOnly="true" autocomplete="off"  CssClass="textbox" MaxLength="50" TabIndex="1" ToolTip="Enter Asset Code" Width="200px" ></asp:TextBox>
                                     
                                        </td>
                                     <td style="text-align: right">
                                        <asp:Label ID="Label8" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label10" runat="server" Text="Site Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList runat="server" ID="ddlLocation" CssClass="dropdownlist" Width="200" TabIndex="5" ToolTip="Select Location" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" InitialValue="-- Select Location --" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                    </td>                                   
                                </tr>
                                      <tr  class="all-asset-display-none Facilities-display">
                                  <td style="text-align: right">
                                      <asp:Label ID="Label27" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label38" runat="server" Text="Asset Far Life" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtLifeCycle" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="25" ToolTip="Enter Far Life" Width="200px"></asp:TextBox>
                                    <%-- <asp:RequiredFieldValidator ID="RFVtxtLifeCycle" runat="server" ControlToValidate="txtLifeCycle" CssClass="validation" InitialValue="" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>--%>
                               </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label46" runat="server" Text="Asset Domain" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAssetDomain" Width="200px" runat="server" CssClass="dropdownlist" AutoPostBack="true"
                                            TabIndex="27" ToolTip="Select Domain" OnSelectedIndexChanged="ddlAssetDomain_SelectedIndexChanged">
                                           <%-- <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                            <asp:ListItem Text="EUC" Value="EUC"></asp:ListItem>
                                            <asp:ListItem Text="Network" Value="Network"></asp:ListItem>
                                            <asp:ListItem Text="Voice" Value="Voice" ></asp:ListItem>
                                            <asp:ListItem Text="Server" Value="Server"></asp:ListItem>
                                             <asp:ListItem Text="Multimedia" Value="Multimedia"></asp:ListItem>--%>
                                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtAssetDomain" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" 
                                            TabIndex="25" ToolTip="Enter Asset Domain" Width="148px" Visible="false"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnAssetDomain" runat="server" Visible="false" AutoPostBack="true"
                                            ToolTip="Save Asset Domain Details" Text="Add" CssClass="button"
                                            Width="74px" Height="16px" OnClick="btnAssetDomain_Click"  />
                                          </td>
                                      <td style="text-align: right">
                                        <asp:Label ID="Label5" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label9" runat="server" Text="Asset Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetType" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Type to search"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="rfvAssetType" runat="server" InitialValue="" ControlToValidate="txtAssetType" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                   
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx505" ID="AutoCompleteExtender505"
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
                                                                        var behavior = $find('AutoCompleteEx505');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx505')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx505')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" TargetControlID="txtAssetType"
                                                        WatermarkText="Search Asset Type..." WatermarkCssClass="watermarked" />
                                                </td>
                                    </tr>
                                <tr class="all-asset-display-none Facilities-display">
                                      <td style="text-align: right" >
                                        <asp:Label ID="Label12" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label18" runat="server" Text="Asset Sub Category" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtSubCategory" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of Asset Sub Category search"></asp:TextBox>
                                          <%-- <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx2909" ID="AutoCompleteExtender2909"
                                            TargetControlID="txtSubCategory" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetSubCategory"
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
                                                                        var behavior = $find('AutoCompleteEx2909');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx2909')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx2909')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                            </Animations>
                                        </act:AutoCompleteExtender>--%>
                                                   <%-- <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtSubCategory"
                                                        WatermarkText="Search Asset Sub Category..." WatermarkCssClass="watermarked" />      --%>                                             
                                                </td>

                                </tr>
                                <tr>
                                    
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Asset Make" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetMake" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset make to search"></asp:TextBox>
                                          <asp:RequiredFieldValidator ID="rfvAssetMake" runat="server" ControlToValidate="txtAssetMake" InitialValue="" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]">

                                          </asp:RequiredFieldValidator>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx209" ID="AutoCompleteExtender209"
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
                                                                        var behavior = $find('AutoCompleteEx209');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx209')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx209')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                            </Animations>
                                        </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAssetMake"
                                                        WatermarkText="Search Asset make..." WatermarkCssClass="watermarked" />                                                   
                                                </td>
                                     <td style="text-align: right">
                                        <asp:Label ID="Label49" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Text="Asset Model" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetModel" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Model to search"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAssetModel" runat="server" ControlToValidate="txtAssetModel" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                              
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx404" ID="AutoCompleteExtender404"
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
                                                                        var behavior = $find('AutoCompleteEx404');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx404')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx404')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtAssetModel"
                                                        WatermarkText="Search Asset Model..." WatermarkCssClass="watermarked" />
                                                </td>
                                    </tr>
                                <tr>
                                     <td style="text-align: right">
                                         <asp:Label ID="Label20" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label16" runat="server" Text="Asset Sub Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlSubStatus" Width="200px" runat="server" CssClass="dropdownlist" AutoPostBack="true"
                                            TabIndex="27" ToolTip="Select Status" OnSelectedIndexChanged="ddlSubStatus_SelectedIndexChanged">
                                             <%--<asp:ListItem Text="ALLOCATED" Value="ALLOCATED"></asp:ListItem>
                                                 <asp:ListItem Text="FAULTY" Value="FAULTY"></asp:ListItem>
                                            <asp:ListItem Text="DAMAGE" Value="DAMAGE"></asp:ListItem>
                                            <asp:ListItem Text="DEPLOYED" Value="DEPLOYED"></asp:ListItem>
                                            <asp:ListItem Text="LOST" Value="LOST"></asp:ListItem>
                                            <asp:ListItem Text="WORKING" Value="WORKING" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="IN SERVICE" Value="IN SERVICE"></asp:ListItem>
                                            <asp:ListItem Text="RETIRED" Value="RETIRED"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                        <%-- <asp:RequiredFieldValidator ID="RFVddlSubStatus" runat="server" ControlToValidate="ddlSubStatus" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>                              --%>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtSubStatusNew" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" 
                                             ToolTip="Enter Sub Status" Width="148px" Visible="false"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnsubStatusAdd" runat="server" Visible="false" AutoPostBack="true"
                                            ToolTip="Save Sub Status Details" Text="Add" CssClass="button"
                                            Width="74px" Height="16px" OnClick="btnsubStatusAdd_Click"  />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label45" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label13" runat="server" Text="Serial Number" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtSerialNumber" runat="server" AutoPostBack="true" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="6" OnTextChanged="txtSerialNumber_TextChanged" ToolTip="Enter Serial Number" Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvSerialNumber" runat="server" ControlToValidate="txtSerialNumber" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                     <%--<asp:CustomValidator ID="cvAssetSerial" runat="server" CssClass="validation" Visible="true" ClientIDMode="AutoID" ControlToValidate="txtSerialNumber" ValidationGroup="Submit" ErrorMessage="Asset SerialNo. is already exist." OnServerValidate="cvAssetSerial_ServerValidate" ValidateEmptyText="true" EnableClientScript="false" SetFocusOnError="true"></asp:CustomValidator>--%>
                               
                                        </td>
                                </tr>
                                 <tr class="all-asset-display-none Facilities-display">
                                   
                                    <td style="text-align: right">
                                        <asp:Label ID="Label36" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label42" runat="server" Text="Asset Far Tag" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtAssetFARTag" runat="server" AutoPostBack="true" OnTextChanged="txtAssetFARTag_TextChanged" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="26" ToolTip="Enter Asset Far Tag" Width="200px"></asp:TextBox>
                                <%--   <asp:RequiredFieldValidator ID="rfvtxtAssetFARTag" runat="server" InitialValue="" ControlToValidate="txtAssetFARTag" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>                       
                                    --%>   <%-- <asp:CustomValidator ID="cvAssetFarTag" runat="server" CssClass="validation" Visible="true" ClientIDMode="AutoID" ControlToValidate="txtAssetFARTag" ValidationGroup="Submit" ErrorMessage="Asset far Tag is already exist." OnServerValidate="cvAssetFarTag_ServerValidate" ValidateEmptyText="true" EnableClientScript="false" SetFocusOnError="true"></asp:CustomValidator>--%>
                                        </td>
                                        
                                   
                                </tr>
                                <tr  class="all-asset-display-none IT-display">                                                                     
                                 
                                    <td style="text-align: right">
                                        <%-- <asp:Label ID="Label50" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>--%>
                                        <asp:Label ID="Label17" runat="server" Text="Asset Tag" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtAssetTag" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="8" ToolTip="Enter Asset Tag" Width="200px" AutoPostBack="true" OnTextChanged="txtAssetTag_TextChanged"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RFVtxtAssetTag" runat="server" ControlToValidate="txtAssetTag" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                 <asp:CustomValidator ID="cvAssetTag" runat="server" CssClass="validation" Visible="true" ClientIDMode="AutoID" ControlToValidate="txtAssetTag" ValidationGroup="Submit" ErrorMessage="Asset Tag. is already exist." OnServerValidate="cvAssetTag_ServerValidate" ValidateEmptyText="true" EnableClientScript="false" SetFocusOnError="true"></asp:CustomValidator>   --%>
                                    </td>
                                    <td style="text-align: right">

                                        <asp:Label ID="Label15" runat="server" Text="RFID Tag" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtRFIDTag" runat="server" AutoPostBack="true" autocomplete="off" OnTextChanged="txtRFIDTag_TextChanged" CssClass="textbox" MaxLength="50" TabIndex="7" ToolTip="Enter RFID Tag" Width="200px"></asp:TextBox>
                                    <%--<asp:CustomValidator ID="cvRFIDTag" runat="server" CssClass="validation" Visible="true" ClientIDMode="AutoID" ControlToValidate="txtRFIDTag" ValidationGroup="Submit" ErrorMessage="RFID Tag is already exist." OnServerValidate="cvRFIDTag_ServerValidate" ValidateEmptyText="true" EnableClientScript="false" SetFocusOnError="true"></asp:CustomValidator>                            --%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                       <%-- <asp:Label ID="Label570" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>--%>
                                        <asp:Label ID="Label1" runat="server" Text="Floor" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList runat="server" ID="ddlFloor" CssClass="dropdownlist"  Width="200" TabIndex="9" ToolTip="Select Floor" AutoPostBack="true" OnSelectedIndexChanged="ddlFloor_SelectedIndexChanged"></asp:DropDownList>
                                       <%-- <asp:RequiredFieldValidator ID="rfvFloor" runat="server" ControlToValidate="ddlFloor" CssClass="validation" InitialValue="-- Select Floor --" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                  --%>  </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label21" runat="server" Text="Processor" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtProcessor" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="10" ToolTip="Enter Processor" Width="200px"></asp:TextBox>

                                    </td>
                                    <td style="text-align: right"></td>
                                    <td style="text-align: center"></td>
                                    <td style="text-align: left"></td>
                                </tr>
                                 <tr class="all-asset-display-none IT-display">
                                     <td style="text-align: right">
                                       <%-- <asp:Label ID="Label18" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>--%>
                                        <asp:Label ID="Label19" runat="server" Text="Store Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList runat="server" ID="ddlStore" CssClass="dropdownlist" Width="200" TabIndex="9" ToolTip="Select Store"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="rfvStore" runat="server" ControlToValidate="ddlStore" CssClass="validation" InitialValue="-- Select Store --" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>--%>
                                    </td>
                                     <td style="text-align: right">
                                        <asp:Label ID="Label23" runat="server" Text="RAM" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtRam" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="11" ToolTip="Enter RAM" Width="200px"></asp:TextBox>
                                    </td>
                                     
                                    
                                </tr>
                                <tr class="all-asset-display-none IT-display">
                                    <td style="text-align: right">
                                        <asp:Label ID="Label25" runat="server" Text="HDD" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtHHD" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="12" ToolTip="Enter HHD" Width="200px"></asp:TextBox>
                                    </td>
                                     <td style="text-align: right">
                                        <asp:Label ID="Label22" runat="server" Text="Warranty Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtWarrantyStatus" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="16" ToolTip="Enter Warranty Status" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr class="all-asset-display-none Facilities-display">
                                   <td style="text-align: right">

                                        <asp:Label ID="Label40" runat="server" Text="Asset Indentifier Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtIdentifierLocation" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="23" ToolTip="Enter Identifier Location" Width="200px"></asp:TextBox>
                                    </td>
                                      </tr>
                                 <tr class="all-asset-display-none IT-display">
                                   
                            <td style="text-align: right">
                                        <asp:Label ID="Label39" runat="server" Text="Warranty / AMC Start Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtWarrantyStartDate" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="17" ToolTip="Enter Warranty Start Date" Width="200px" onfocus="showCalendarControl(this);"></asp:TextBox>
                                    </td>
                                     <td style="text-align: right">
                                        <asp:Label ID="Label41" runat="server" Text="Warranty / AMC End Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtWarrantyEndDate" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="18" ToolTip="Enter Warranty End Date" Width="200px" onfocus="showCalendarControl(this);"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr class="all-asset-display-none IT-display">
                                     <td style="text-align: right">
                                        <asp:Label ID="Label24" runat="server" Text="GRN No" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtGRNNo" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="14" ToolTip="Enter GRN No" Width="200px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                       <%-- <asp:Label ID="Label29" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>--%>
                                        <asp:Label ID="Label47" runat="server" Text="GRN Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtGRNDate" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="20" ToolTip="Enter GRN Date" Width="200px" onfocus="showCalendarControl(this);"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfvPODate" runat="server" ControlToValidate="txtPODate" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>                       
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label52" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label28" runat="server" Text="PO No" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtPONo" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="19" ToolTip="Enter PO No" Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPONo" runat="server" ControlToValidate="txtPONo" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                          <asp:Label ID="Label51" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label30" runat="server" Text="PO Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtPODate" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="20" ToolTip="Enter PO Date" Width="200px" onfocus="showCalendarControl(this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPODate" runat="server" ControlToValidate="txtPODate" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>

                                <tr class="all-asset-display-none IT-display">
                                    <td style="text-align: right">
                                        <asp:Label ID="Label31" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label32" runat="server" Text="Invoice No" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtInvoiceNo" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="21" ToolTip="Enter Invoice No" Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvInvoiceNo" runat="server" ControlToValidate="txtInvoiceNo" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label33" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label34" runat="server" Text="Invoice Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtInvoiceDate" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="22" ToolTip="Enter Invoice Date" Width="200px" onfocus="showCalendarControl(this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvInvoiceDate" runat="server" ControlToValidate="txtInvoiceDate" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class="all-asset-display-none IT-display">
                                       <td style="text-align: right">
                                        <asp:Label ID="Label37" runat="server" Text="Scope" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">

                                        <asp:DropDownList ID="ddlProcurementBudget" runat="server" CssClass="dropdownlist" TabIndex="15" ToolTip="Enter Scope" Width="200px">
                                            <asp:ListItem Value="">-- Select Scope --</asp:ListItem>
                                            <asp:ListItem Value="BAU">BAU</asp:ListItem>
                                            <asp:ListItem Value="PROJECT">PROJECT</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label48" runat="server" Text="Purchase Cost (in INR/USD)" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtPurchaseCost" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="19" ToolTip="Enter Purchase cost" Width="200px"></asp:TextBox>
                                    </td>
                                   
                                </tr>
                                <tr class="all-asset-display-none IT-display">
                                     
                                     <td style="text-align: right">
                                           <asp:Label ID="Label29" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label26" runat="server" Text="OEM Vendor" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList runat="server" ID="ddlVendor" CssClass="dropdownlist" Width="200"></asp:DropDownList>
                                         <asp:RequiredFieldValidator ID="RFVddlVendor" runat="server" InitialValue="-- Select Vendor --" ControlToValidate="ddlVendor" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                   
                                    </td>
                                     </tr>        
                                <tr class="all-asset-display-none Facilities-display">
                                   
                                    <td style="text-align: right">
                                          <asp:Label ID="Label6" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label43" runat="server" Text="In Service Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtInServiceDate" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="24" ToolTip="Enter In service Date" Width="200px" onfocus="showCalendarControl(this);"></asp:TextBox>
                                     <%--<asp:RequiredFieldValidator ID="RFVtxtInServiceDate" runat="server" ControlToValidate="txtInServiceDate" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                           --%>     </td>
                                </tr>
                               
                              
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label35" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td colspan="4" style="text-align: left" valign="top">
                                        <asp:TextBox ID="txtRemarks" runat="server" autocomplete="off" CssClass="multitextbox" Height="60px" MaxLength="400" TabIndex="28" TextMode="MultiLine" ToolTip="Enter Remarks" ValidationGroup="Submit" Width="676px">
                                        </asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label44" runat="server" Text="File Upload" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left" colspan="4">
                                        <asp:FileUpload ID="AssetFileUpload" ToolTip="Browse excel file through here for asset data import" TabIndex="29"
                                            CssClass="textbox" runat="server" />
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="35"
                                            ToolTip="Save Details" Text="Save" CssClass="button"
                                            Width="110px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" TabIndex="36" OnClientClick="ClearFields();"
                                            ToolTip="Reset/Clear Fields" Text="Reset" CssClass="button" Width="60px"
                                            CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label14" runat="server" Text="Select Asset Acquisition Data File :" CssClass="label"></asp:Label>
                                        </td>
                                        <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                            <ContentTemplate>
                                                <td style="text-align: center">
                                                    <asp:FileUpload ID="fuBulkUpload" ToolTip="Browse excel file through here for asset data import"
                                                        CssClass="textbox" runat="server" />
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Button ID="btnUploadFile" runat="server" Text="Upload File" CssClass="button"
                                                        TabIndex="15" Width="150px" CausesValidation="true" ToolTip="Upload File"
                                                        OnClick="btnUploadFile_Click" />
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Button ID="btnBulkUpdateFile" runat="server" Text="Bulk Update" CssClass="button"
                                                        TabIndex="15" Width="150px" CausesValidation="true" ToolTip="Bulk Update"
                                                        OnClick="btnBulkUpdateFile_Click" />
                                                </td>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnUploadFile" />
                                                <asp:PostBackTrigger ControlID="btnBulkUpdateFile" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <td style="text-align: center">
                                            <a href="DownloadSample/ASSET_ACQUISITITION_IT.xlsx" class="button all-asset-display-none IT-display">
                                                <input type="button" class="button btn btn-secondary" value="Download Template" style="padding:5px" />
                                            </a>
                                            <a href="DownloadSample/ASSET_ACQUISITITION_FACILITIES.xlsx" class="all-asset-display-none Facilities-display">
                                                <input type="button" class="button btn btn-secondary" value="Download Template" style="padding:5px" />
                                            </a>
                                        </td>
                                    </td>
                                </tr>

                            </table>
                            <table>
                        <tr>
                        <td></td>
                        <td></td>                       
                    </tr>
                </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                 
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="2" style="text-align: right">
                            <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Report" 
                                ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click"/>
                        </td>
            </tr>
        </table>
    </div>
</asp:Content>


