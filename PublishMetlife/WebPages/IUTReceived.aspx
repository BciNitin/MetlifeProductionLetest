<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="IUTReceived.aspx.cs" Inherits="WebPages_IUTReceived" Title="ATS - IUT Received" %>

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

        function HeaderViewClick(CheckBox) {
            debugger;
            var n1 = 0;
            var gridgv = document.getElementById('<%=GridView2.ClientID %>');
            var rowCount = gridgv.rows.length - 1;
            var TargetBaseControl = document.getElementById('<%= this.GridView2.ClientID %>');
            var TargetChildControl = "chkSelectAsset";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' &&
                    Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    var row = Inputs[n].parentNode.parentNode;
                    var temp = row.cells[12].innerText;
                    var temp1 = row.cells[13].innerText;
                    Inputs[n].checked = CheckBox.checked;
                    n1++;
                }
            }
            if (n1 != rowCount) { CheckBox.checked = false; }
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }
        function ChildClick(CheckBox, HCheckBox) {
            debugger;
            Counter = parseInt("0");
            var TargetBaseControl = document.getElementById('<%=GridView2.ClientID %>');
            var TargetChildControl = "chkSelectAsset";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var row = CheckBox.parentNode.parentNode;
            var checkStatus = row.cells[12].innerText;
            var temp1 = row.cells[13].innerText;

            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' &&
                    Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    if (Inputs[n].checked)
                        Counter++;
                }
            }
            <%--TotalChkBx = parseInt('<%=this.GridView2.Rows.Count %>');--%>
            var gridgv = document.getElementById('<%=GridView2.ClientID %>');
            TotalChkBx = gridgv.rows.length - 1;
            <%--//TotalChkBx = document.getElementByID('<%=GridView2.ClientID%>').rows.length;--%>
            Counter = HCheckBox.checked ? TotalChkBx : Counter;
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;
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
            Asset IUT Receiving
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table2" runat="server" cellspacing="10" style="width: 100%" align="center">
            <%--<tr>
                <td colspan="4">
                    <asp:Label ID="Label52" Font-Bold="true" Text="* marked fields are mandatory. In bulk upload, please upload excel file with date in dd-MMM-yyyy format. Also set cell as Date Format. Please Set the Cell as Date Format, do not Custom.  " CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>--%>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table1" runat="server" cellspacing="15" style="width: 100%;" align="center">
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="Label21" runat="server" CssClass="label" Text="Remarks"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtRemarks" runat="server" CssClass="multitextbox"
                                            TabIndex="11" Height="50px" Width="470px" ToolTip="Enter remarks" TextMode="MultiLine"
                                            MaxLength="1000"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Receive" Enabled="false" Visible="false" CssClass="button"
                                            TabIndex="15" Width="150px" CausesValidation="true" ToolTip="Receive Asset"
                                            OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                         <asp:Button ID="btnSubmitLostinTransit" runat="server" Text="Lost InTransit" Enabled="false"
                                             Visible="false" CssClass="button" TabIndex="16" Width="150px" CausesValidation="true" ToolTip="Save Lost in Transit" OnClick="btnSubmitLostinTransit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnReject" runat="server" Text="Reject" Enabled="false"
                                            Visible="false" CssClass="button" TabIndex="16" Width="150px" CausesValidation="true" ToolTip="Reject Asset" OnClick="btnReject_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" OnClientClick="ClearFields();" Text="Reset" CssClass="button"
                                            TabIndex="17" Width="60px" CausesValidation="false" ToolTip="Refresh/Reset fields"
                                            OnClick="btnClear_Click" />
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <table>
                                    <tr>
                                        <td colspan="2" style="text-align: right">
                                            <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Report"
                                                ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <tr>
                                    <td colspan="5" align="center">
                                        <table id="tblAssets" runat="server" cellspacing="12" width="100%" style="border: 2px double #006600;"
                                            align="center">
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="Label17" Font-Bold="true" Text="To search the Asset for allocation..." CssClass="ErrorLabel"
                                                        runat="server"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>

                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label32" runat="server" Text="Gate Pass Code :" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left; vertical-align: top">
                                                    <asp:TextBox autocomplete="off" ID="txtGatePassCode" runat="server" CssClass="textbox"
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters Gate Pass Code"></asp:TextBox>
                                                    <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx91" ID="AutoCompleteExtender91"
                                                        TargetControlID="txtGatePassCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetAssetFarTagforAllocation"
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
                                                    <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender9" runat="server" TargetControlID="txtGatePassCode"
                                                        WatermarkText="Search Gate Pass ID..." WatermarkCssClass="watermarked" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label25" runat="server" Text="Site Location" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:DropDownList ID="ddlSiteLocation" Width="200px" runat="server"
                                                        CssClass="dropdownlist"
                                                        TabIndex="3" ToolTip="Select Location">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblFloor" runat="server" Text="Floor" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:DropDownList ID="ddlFloor" Width="200px" runat="server" AutoPostBack="true"
                                                        CssClass="dropdownlist"
                                                        TabIndex="3" ToolTip="Select Floor" OnSelectedIndexChanged="ddlFloor_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>

                                                <td style="text-align: right">
                                                    <asp:Label ID="lblStore" runat="server" Text="Store" CssClass="label"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:DropDownList ID="ddlStore" Width="200px" runat="server" AutoPostBack="true"
                                                        CssClass="dropdownlist"
                                                        TabIndex="3" ToolTip="Select Store">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td align="center" colspan="6">
                                                    <asp:Button ID="btnAllSearch" runat="server" Text="Search" CssClass="button"
                                                        TabIndex="15" Width="150px" CausesValidation="true" ToolTip="Search Asset Details" OnClick="btnAllSearch_Click" />
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
                                                        <asp:CheckBox ID="chkHSelect" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedHeaderChanged" 
                                                            Text=" Select" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectAsset" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gate Pass Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGatePassCode" runat="server" Text='<%#Eval("GATEPASS_CODE") %>'></asp:Label>
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
                                                <asp:TemplateField HeaderText="Asset Far Tag">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFarTag" runat="server" Text='<%#Eval("ASSET_FAR_TAG") %>'></asp:Label>
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
                                                <asp:TemplateField HeaderText="Asset Model">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblam" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
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
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Destination Location">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGatePassOutLoc" runat="server" Text='<%#Eval("GATEPASS_OUT_LOC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pgr"></PagerStyle>
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>


                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmitLostinTransit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReject" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />

                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
