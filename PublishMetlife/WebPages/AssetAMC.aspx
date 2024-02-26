<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="AssetAMC.aspx.cs" Inherits="AssetAMC" Title="BCIL : ATS - Generate AMC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        var TotalChkBx;
        var Counter;
        window.onload = function()
        {
           //Get total no. of CheckBoxes in side the GridView.
           TotalChkBx = parseInt('<%= this.gvAMCAssets.Rows.Count %>');
           //Get total no. of checked CheckBoxes in side the GridView.
           Counter = 0;
        }

        function HeaderViewClick(CheckBox)
        {
           var TargetBaseControl = document.getElementById('<%= this.gvAMCAssets.ClientID %>');
           var TargetChildControl = "chkSelect";
           var Inputs = TargetBaseControl.getElementsByTagName("input");
           for(var n = 0; n < Inputs.length; ++n)
              if(Inputs[n].type == 'checkbox' && 
                        Inputs[n].id.indexOf(TargetChildControl,0) >= 0)
                 Inputs[n].checked = CheckBox.checked;
           Counter = CheckBox.checked ? TotalChkBx : 0;
        }

        function ChildClick(CheckBox, HCheckBox) {
            TotalChkBx = parseInt('<%= this.gvAMCAssets.Rows.Count %>');
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
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= ddlVendor.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtWarranty.ClientID%>').value = "";
            document.getElementById('<%= txtPurchaseDate.ClientID%>').value = "";
            document.getElementById('<%= txtAMCValue.ClientID%>').value = "";
            document.getElementById('<%= txtStartDate.ClientID%>').value = "";
            document.getElementById('<%= txtEndDate.ClientID%>').value = "";
            document.getElementById('<%= txtRespPerson.ClientID%>').value = "";
            document.getElementById('<%= UploadRefDoc.ClientID%>').value = "";
            document.getElementById('<%= txtAssetPONo.ClientID%>').value = "";
            document.getElementById('<%= txtSerialCode.ClientID%>').value = "";
            document.getElementById('<%= ddlAssetType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAssetCategory.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= rdoInAMC.ClientID%>').checked = false;
            document.getElementById('<%= rdoNotInAMC.ClientID%>').checked = true;
        }
        function CompareDates() {
            var first = document.getElementById('<%= txtStartDate.ClientID%>').value;
            var second = document.getElementById('<%= txtEndDate.ClientID%>').value;
            var firstDate = new Date(first.split('/')[2], first.split('/')[1], first.split('/')[0]);
            var secondDate = new Date(second.split('/')[2], second.split('/')[1], second.split('/')[0]);
            var Difference = firstDate.getDate() - secondDate.getDate();
            if (Difference > 0) {
                document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : End date cannot be earlier than start date.';
                return false;
            }
            else return true;
        }
        function getCheckedValue(radioObj) {
            if (radioObj.value == 'rdoInAMC') {
                document.getElementById('<%= rdoInAMC.ClientID%>').checked = true;
                document.getElementById('<%= rdoNotInAMC.ClientID%>').checked = false;
            }
            if (radioObj.value == 'rdoNotInAMC') {
                document.getElementById('<%= rdoNotInAMC.ClientID%>').checked = true;
                document.getElementById('<%= rdoInAMC.ClientID%>').checked = false;
            }
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg;
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
            width: 110px;
        }
        .style2
        {
            width: 180px;
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
            Generate Asset AMC
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="16" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="Select Vendor" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlVendor" runat="server" CssClass="dropdownlist" Width="200px"
                                            TabIndex="1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_Vendor" runat="server" ControlToValidate="ddlVendor"
                                            ErrorMessage="*" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="AMC Value" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtAMCValue" CssClass="textbox" MaxLength="10"
                                            runat="server" Width="200px" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_AMCValue" runat="server" ControlToValidate="txtAMCValue"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAMCValue"
                                            CssClass="validation" ErrorMessage="[ Only Numbers ]" ValidationExpression="^\d+$"
                                            ValidationGroup="Submit"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label7" runat="server" Text="AMC/Warranty" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtWarranty" CssClass="textbox" runat="server"
                                            Width="200px" TabIndex="3"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtWarranty"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" runat="server" Text="Purchase Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtPurchaseDate" CssClass="textbox" runat="server"
                                            Width="200px" onfocus="showCalendarControl(this);" TabIndex="4"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPurchaseDate"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="AMC Start Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtStartDate" CssClass="textbox" runat="server"
                                            Width="200px" onfocus="showCalendarControl(this);" TabIndex="3"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" runat="server" Text="AMC End Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtEndDate" CssClass="textbox" runat="server"
                                            Width="200px" onfocus="showCalendarControl(this);" TabIndex="4"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="Resp. Person Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtRespPerson" CssClass="textbox" runat="server"
                                            Width="200px" TabIndex="5"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtRespPerson"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right" class="style2">
                                        <asp:Label ID="Label9" runat="server" Text="AMC Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtAMCCode" CssClass="readonlytext" runat="server"
                                            Width="200px" TabIndex="-1" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label6" runat="server" Text="Reference Doc." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" colspan="4" valign="top">
                                        <asp:FileUpload ID="UploadRefDoc" runat="server" TabIndex="5" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="UploadRefDoc"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator><br />
                                        <asp:TextBox autocomplete="off" ID="txtAMCDoc" CssClass="textbox" runat="server"
                                            Width="200px" TabIndex="5" Enabled="False"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnDownload" runat="server" Height="16px" TabIndex="14" ImageUrl="~/images/Download_24x24.png"
                                            OnClick="btnDownload_Click" Enabled="false" ToolTip="Download document" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="text-align: center">
                                        <asp:LinkButton ID="lnkITDetails" CssClass="pageLinks" runat="server">Select Assets For AMC</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlContent"
                                            ExpandControlID="lnkITDetails" CollapseControlID="lnkITDetails" TextLabelID="Label1"
                                            ImageControlID="Image1" ExpandedImage="../images/Update_icon.png" CollapsedImage="../images/Update_icon.png"
                                            Collapsed="True" SuppressPostBack="true">
                                        </act:CollapsiblePanelExtender>
                                        <asp:Panel ID="pnlContent" runat="server">
                                            <table id="Table3" runat="server" cellspacing="15" style="width: 100%; border: 2px double #006600;"
                                                align="center">
                                                <tr>
                                                    <td style="text-align: right;" class="style1">
                                                        <asp:Label ID="Label14" runat="server" Text="Asset PO No." CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:TextBox autocomplete="off" ID="txtAssetPONo" runat="server" CssClass="textbox"
                                                            Width="200px"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAssetPONo"
                                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label16" runat="server" CssClass="label" Text="Asset Serial Code"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold; height: 17px;">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox autocomplete="off" ID="txtSerialCode" runat="server" CssClass="textbox"
                                                            Width="200px"></asp:TextBox>
                                                        <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx" ID="AutoCompleteExtender1"
                                                            TargetControlID="txtSerialCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetSerialNo"
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
                                                        <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtSerialCode"
                                                            WatermarkText="Search..." WatermarkCssClass="watermarked" />
                                                        &nbsp;
                                                        <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/images/go_button.png" OnClick="btnGo_Click"
                                                            Height="25px" Width="25px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label10" runat="server" Text="Asset Type" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:DropDownList ID="ddlAssetType" Width="200px" runat="server" CssClass="dropdownlist"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                                            <asp:ListItem Text="ADMIN ASSET" Value="AD"></asp:ListItem>
                                                            <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlAssetCategory"
                                                            ErrorMessage="*" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label12" runat="server" Text="FAMS ID" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox autocomplete="off" ID="txtAssetID" runat="server" CssClass="textbox"
                                                            Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label11" runat="server" Text="Asset Category" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <div style="font-weight: bold">
                                                            :</div>
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:DropDownList ID="ddlAssetCategory" Width="200px" runat="server" CssClass="dropdownlist">
                                                        </asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator ID="RFV_Category" runat="server" ControlToValidate="ddlAssetCategory"
                                                            ErrorMessage="*" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:RadioButton ID="rdoInAMC" runat="server" onClick="getCheckedValue(this);" Checked="false"
                                                            CssClass="label" Text="  Assets in AMC" AutoPostBack="true" OnCheckedChanged="rdoInAMC_CheckedChanged" />
                                                    </td>
                                                    <td style="text-align: center">
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:RadioButton ID="rdoNotInAMC" runat="server" onClick="getCheckedValue(this);"
                                                            Checked="true" CssClass="label" AutoPostBack="true" Text="  Assets not in AMC"
                                                            OnCheckedChanged="rdoNotInAMC_CheckedChanged" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="btnSearch" runat="server" Height="16px" TabIndex="14" ImageUrl="~/images/Search_16x16.png"
                                                            OnClick="btnSearch_Click" ToolTip="Search Assets" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <div id="dv" style="overflow: auto">
                                                            <asp:GridView ID="gvAMCAssets" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                PageSize="20" OnRowCreated="gvAMCAssets_RowCreated" ShowFooter="false" CssClass="mGrid"
                                                                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvAMCAssets_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkHSelect" runat="server" onclick="javascript:HeaderViewClick(this);"
                                                                                Text=" Select" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
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
                                                                            <asp:Label ID="lblSerialName" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Model Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO No.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPONo" runat="server" Text='<%#Eval("PO_NUMBER") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Location">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocCode" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPODate" runat="server" Text='<%#Eval("PO_DATE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="AMC Code" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAMCCode" runat="server" Text='<%#Eval("AMC_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="AMC Running No." Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAMCRunningNo" runat="server" Text='<%#Eval("AMC_RUNNING_NO") %>'></asp:Label>
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
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="6"
                                            ToolTip="Submit" ImageUrl="~/images/Submit.png" Height="35px" Width="85px" OnClientClick="return CompareDates();"
                                            OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="7" OnClientClick="ClearFields();"
                                            ToolTip="Reset" ImageUrl="~/images/Reset.png" Height="35px" Width="85px" CausesValidation="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblErrorMsg" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnDownload" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvAMCAssets" EventName="RowCreated" />
                            <asp:AsyncPostBackTrigger ControlID="gvAMCAssets" EventName="PageIndexChanging" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
