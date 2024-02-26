<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="GenerateGatePass.aspx.cs" Inherits="GenerateGatePass" Title="BCIL : ATS - Gate Pass Generation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        var TotalChkBx;
        var Counter;
        window.onload = function () {
            TotalChkBx = parseInt('<%= this.gvAssets.Rows.Count %>');
            Counter = 0;
        }
        function ClearFields() {
            window.location.href = window.location;
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
                    if (temp == 'STOCK') {
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
                alert("status not belongs to STOCK");
                return false;
            }

        }
        function ShowHideReturnableFields() {
            var GPType = document.getElementById('<%= ddlGatePassType.ClientID%>');
            var Text = GPType.options[GPType.selectedIndex].value;
            if (Text == "RETURNABLE") {
                document.getElementById('<%= txtReturnableDate.ClientID%>').disabled = false;
                document.getElementById('<%= RequiredFieldValidator9.ClientID%>').disabled = false;
            }
            else if (Text == "NOTRETURNABLE" || Text == "SELECT") {
                document.getElementById('<%= txtReturnableDate.ClientID%>').disabled = true;
                document.getElementById('<%= RequiredFieldValidator9.ClientID%>').disabled = true;
            }
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
        function ShowAlert(Message) {
            alert(Message);
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .style1 {
            width: 200px;
        }

        .style2 {
            width: 120px;
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
            GATE PASS GENERATION
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="6">
                    <asp:Label ID="Label19" Font-Bold="true" Text="* marked fields are mandatory. In bulk upload, please upload excel file with date in dd-MMM-yyyy format. Please Set the Cell as Date Format, do not Custom.  " CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="Gate Pass No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold; visibility: hidden">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtGatePassNo" MaxLength="50" CssClass="readonlytext"
                                            runat="server" Width="200px" TabIndex="-1" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label12" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label13" runat="server" Text="Document No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtDocumentNo" MaxLength="50" Style="margin-left: 0px !important;" runat="server" CssClass="textbox"
                                            Width="200px" TabIndex="7" ToolTip="Enter document no"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDocumentNo"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label7" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Gate Pass Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlGatePassType" runat="server" CssClass="dropdownlist" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlGatePassType_SelectedIndexChanged"
                                            TabIndex="1" ToolTip="Select gate pass type" onChange="ShowHideReturnableFields();">
                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="Returnable" Value="RETURNABLE"></asp:ListItem>
                                            <asp:ListItem Text="Not Returnable" Value="NOTRETURNABLE"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_GPType" runat="server" ControlToValidate="ddlGatePassType"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblReturnableDate" runat="server" Text="Returnable Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtReturnableDate" Style="margin-left: 0px !important;" MaxLength="50" runat="server"
                                            ToolTip="Enter/select gate pass returnable date" TabIndex="2" CssClass="textbox"
                                            Enabled="false" Width="200px" onfocus="showCalendarControl(this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Enabled="false"
                                            ControlToValidate="txtReturnableDate" ErrorMessage="[Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label14" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Gate Pass Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;" visible="false">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtGatePassDate" Style="margin-left: 0px !important;" CssClass="textbox" Width="200px"
                                            runat="server" ToolTip="Enter/select gate pass date" onfocus="showCalendarControl(this);"
                                            TabIndex="3"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_GPDate" runat="server" ControlToValidate="txtGatePassDate"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label20" runat="server" Text="Gate Pass For" Visible="false" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <%--  <div style="font-weight: bold" visible="false">
                                            :</div>--%>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlGatePassFor" runat="server" Width="200px" Visible="false" TabIndex="6" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlGatePassFor_SelectedIndexChanged" ToolTip="Select Gate pass for"
                                            CssClass="dropdownlist">
                                            <asp:ListItem Text="-- Select For --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="Employee" Value="EMPLOYEE"></asp:ListItem>
                                            <asp:ListItem Text="Vendor" Value="VENDOR"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td style="text-align: right">
                                        <asp:Label ID="Label16" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label15" runat="server" CssClass="label" Text="Select Location"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlGPLocation" Width="200px" runat="server" AutoPostBack="true"
                                            TabIndex="4" ToolTip="Select gate pass location" OnSelectedIndexChanged="ddlGPLocation_SelectedIndexChanged"
                                            CssClass="dropdownlist">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblLocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="btnRefreshLocation" runat="server" CausesValidation="false"
                                            ImageUrl="../images/Refresh_16x16.png" TabIndex="5" ToolTip="Refresh/reset location"
                                            OnClick="btnRefreshLocation_Click" />
                                        <asp:Label ID="lblLocCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>--%>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label21" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label22" runat="server" CssClass="label" Text="Destination Location"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlDestLocation" Width="200px" runat="server" AutoPostBack="true"
                                            TabIndex="4" ToolTip="Select gate pass location" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                                            CssClass="dropdownlist">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblDestlocLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false"
                                            ImageUrl="../images/Refresh_16x16.png" TabIndex="5" ToolTip="Refresh/reset location" OnClick="ImageButton1_Click" />
                                        <asp:Label ID="lblDestCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label17" Font-Bold="true" Text="*" CssClass="ErrorLabel" Visible="false" runat="server"></asp:Label>
                                        <asp:Label ID="Label5" runat="server" Text="Carrier Name" Visible="false" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <%--  <div style="font-weight: bold" Visible="false">
                                            :</div>--%>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtGPCarrier" Visible="false" MaxLength="50" runat="server" CssClass="textbox"
                                            Width="200px" TabIndex="7" ToolTip="Enter gate pass carrier name"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtGPCarrier"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" runat="server" Text="Select Name" CssClass="label" Visible="False"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <%--<div style="font-weight: bold">
                                            :</div>--%>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlGatePassForName" runat="server" Width="200px" TabIndex="8"
                                            CssClass="dropdownlist" ToolTip="Select gate pass for name" Visible="False">
                                        </asp:DropDownList>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlGatePassForName"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label18" Font-Bold="true" Text="*" CssClass="ErrorLabel" Visible="False" runat="server"></asp:Label>
                                        <asp:Label ID="Label6" runat="server" Text="Bearer Name" CssClass="label" Visible="False"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <%--  <div style="font-weight: bold">
                                            :</div>--%>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtGPBearerName" runat="server" TabIndex="9"
                                            CssClass="textbox" Width="200px" MaxLength="50" ToolTip="Enter gate pass bearer name" Visible="False"></asp:TextBox>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtGPBearerName"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td style="text-align: right"></td>
                                    <td style="text-align: center"></td>
                                    <td style="text-align: left"></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label8" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td colspan="4" style="text-align: left" valign="top">
                                        <asp:TextBox autocomplete="off" ID="txtGPRemarks" runat="server" TabIndex="10" TextMode="MultiLine"
                                            CssClass="multitextbox" ToolTip="Enter remarks" Height="50px" Width="680px" MaxLength="500"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6">
                                        <table id="tblAssets" runat="server" cellspacing="10" width="100%" style="border: 2px double #006600;"
                                            align="center">
                                            <%--<tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="Label10" runat="server" Enabled="false" CssClass="label" Text="Serial No/ Tag : "></asp:Label>
                                                </td>
                                               
                                                     <td style="text-align:left; vertical-align: center">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetCode"  runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 5 characters of asset code /tag to search" ></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                                                        TargetControlID="txtAssetCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetCode"
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtAssetCode"
                                                        WatermarkText="Search serial No..." WatermarkCssClass="watermarked" />
                                                    &nbsp;
                                                     </td>

                                            
                                                <td style="text-align: left;">
                                                    <asp:Label ID="Label9" runat="server" Text="Asset Category :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:DropDownList ID="ddlAssetCategory" Width="200px" TabIndex="12" runat="server"
                                                        CssClass="dropdownlist" ToolTip="Select asset cateory" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblCatLevel" runat="server" CssClass="label" Text="1" Visible="False"></asp:Label>
                                                    <asp:ImageButton ID="btnRefreshCategory" TabIndex="13" runat="server" ImageUrl="~/images/Refresh_16x16.png"
                                                        OnClick="btnRefreshCategory_Click" ToolTip="Refresh/reset category" CausesValidation="false" />
                                                    <asp:Label ID="lblCatCode" runat="server" Visible="false" CssClass="label" Text="0"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:Label ID="Label90" runat="server" CssClass="label" Text="Asset Make : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:DropDownList ID="ddlAssetMake" Width="200px" TabIndex="14" runat="server" CssClass="dropdownlist"
                                                        AutoPostBack="true" ToolTip="Select asset make" OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:Label ID="Label11" runat="server" CssClass="label" Text="Model Name : "></asp:Label>
                                                </td>
                                                <td style="text-align: left" rowspan="2">
                                                    <asp:ListBox ID="lstModelName" ToolTip="Select one or more model names" TabIndex="15"
                                                        SelectionMode="Multiple" Width="200px" CssClass="textbox" runat="server"></asp:ListBox>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="Label27" Font-Bold="true" Text="To Search Asset To Generate Gate Pass..." CssClass="ErrorLabel"
                                                        runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label9" runat="server" CssClass="label" Text="Asset Make : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetMake" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Make to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx88" ID="AutoCompleteExtender88"
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
                                                                        var behavior = $find('AutoCompleteEx88');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx88')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx88')._height" EndValue="0" />
                                                                </Parallel>
                                                            </OnHide>
                                                        </Animations>
                                                    </act:AutoCompleteExtender>
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAssetMake"
                                                        WatermarkText="Search Asset Make..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label10" runat="server" Enabled="true" CssClass="label" Text="Asset Model : "></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtAssetModel" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of asset Model to search"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx44" ID="AutoCompleteExtender44"
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
                                                                        var behavior = $find('AutoCompleteEx44');
                                                                        if (!behavior._height) {
                                                                            var target = behavior.get_completionList();
                                                                            behavior._height = target.offsetHeight - 2;
                                                                            target.style.height = '0px';
                                                                        }" />
                                                                    <Parallel Duration=".4">
                                                                        <FadeIn />
                                                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx44')._height" />
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Parallel Duration=".4">
                                                                    <FadeOut />
                                                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx44')._height" EndValue="0" />
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
                                                    <asp:Label ID="Label11" runat="server" Text="Asset Type :" CssClass="label"></asp:Label>
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
                                                    <asp:Label ID="Label15" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
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
                                                    <asp:Label ID="Label24" runat="server" CssClass="label" Text="Asset Processor : "></asp:Label>
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
                                                    <asp:Label ID="Label25" runat="server" Enabled="true" CssClass="label" Text="Asset RAM : "></asp:Label>
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

                                            <tr class="all-asset-display-none IT-display">

                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label26" runat="server" Text="Asset SerialNo :" CssClass="label"></asp:Label>
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
                                                <td class="style2"></td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:Label ID="lblAssetCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top" class="style1">
                                                    <asp:Button ID="btnSearch" TabIndex="16" runat="server" CssClass="button" Text="Get Assets"
                                                        OnClick="btnSearch_Click" Width="75px" ToolTip="Get a list of assets based on search criteria" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="gvAssets" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        OnPageIndexChanging="gvAssets_PageIndexChanging" ShowFooter="false" CssClass="mGrid"
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
                                                            <asp:TemplateField HeaderText="Serial No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Far Tag.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetFarTag" runat="server" Text='<%#Eval("ASSET_FAR_TAG") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="RFID TAG">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRafID" runat="server" Text='<%#Eval("Tag_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset TAG">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetTAG" runat="server" Text='<%#Eval("ASSET_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Make">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetMake" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Model Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelNameererss" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Vendor">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("VENDOR_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Location">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("ASSET_LOCATION") %>'></asp:Label>
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
                                    <td colspan="6" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="17"
                                            ToolTip="Save gate pass details" Text="Generate Gate Pass" CssClass="button"
                                            Width="150px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" TabIndex="18" OnClientClick="ClearFields();"
                                            ToolTip="Reset/clear fields" Text="Reset" CssClass="button" Width="60px"
                                            CausesValidation="False" OnClick="btnClear_Click" />
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
                                            <asp:Label ID="Label23" runat="server" Text="Select Asset Allocation Data File :" CssClass="label"></asp:Label>
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
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnUploadFile" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <td style="text-align: center" class="all-asset-display-none IT-display">
                                            <a href="DownloadSample/ASSET_TRANSFER_IT.xlsx">
                                                <input type="button" class="button btn btn-secondary" value="Download Template" style="padding: 5px" />
                                            </a>
                                        </td>
                                        <td style="text-align: center" class="all-asset-display-none Facilities-display">
                                            <a href="DownloadSample/ASSET_TRANSFER_FACILITIES.xlsx">
                                                <input type="button" class="button btn btn-secondary" value="Download Template" style="padding: 5px" />
                                            </a>
                                        </td>
                                    </td>
                                </tr>


                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                            <%-- <asp:AsyncPostBackTrigger ControlID="btnRefreshLocation" EventName="Click" />--%>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center" class="pageAnchor">
                    <asp:LinkButton ID="btnPrintGatePass" runat="server" TabIndex="19"
                        ToolTip="Print gate pass" CausesValidation="False"
                        OnClick="btnPrintGatePass_Click">Print Gatepass</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label16" runat="server" Text="Gate Pass No." CssClass="label"></asp:Label>
                </td>
                <td style="text-align: center">
                    <div style="font-weight: bold; visibility: hidden">
                        :
                    </div>
                </td>
                <td style="text-align: left">
                    <asp:TextBox autocomplete="off" ID="txtGatePassNoVerify" MaxLength="50" AutoPostBack="true"
                        runat="server" Width="200px" TabIndex="-1" OnTextChanged="txtGatePassNoVerify_TextChanged"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td colspan="4" align="center" class="pageAnchor">
                    <asp:LinkButton ID="btnRePrintGatePass" runat="server" TabIndex="19"
                        ToolTip="Reprint gate pass" CausesValidation="False"
                        OnClick="btnRePrintGatePass_Click">RePrint Gatepass</asp:LinkButton>
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
