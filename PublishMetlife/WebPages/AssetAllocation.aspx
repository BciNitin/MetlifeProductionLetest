<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="AssetAllocation.aspx.cs" Inherits="AssetAllocation" Title="ATS - Asset Allocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        var TotalChkBx;
        var Counter;

        function ClearFields() {
            window.location.href = window.location;
           <%-- document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= ddlAllocRtrn.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlEmployee.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlStatus.ClientID%>').selectedIndex = 0;
            
            document.getElementById('<%=ddlEmployee.ClientID%>').disabled = false;
            document.getElementById('<%= txtAssetCode.ClientID%>').value = "";
            document.getElementById('<%= txtTicketNo.ClientID%>').value = "";
            document.getElementById('<%= txtExpRtnDate.ClientID%>').value = "";
            document.getElementById('<%=txtExpRtnDate.ClientID%>').disabled = false;
            document.getElementById('<%= txtAllocationDate.ClientID%>').value = "";
            document.getElementById('<%= txtStore.ClientID%>').value = "";
            document.getElementById('<%= enternoofduedate.ClientID%>').value = "";
            document.getElementById('<%= txtStore.ClientID%>').value = "";
             document.getElementById('<%= txtStore.ClientID%>').disabled = false;
            document.getElementById('<%= txtRemarks.ClientID%>').value = "";
             document.getElementById('<%= GridView1.ClientID%>').visible = false;--%>
           <%-- document.getElementById('<%= ddlFromDept.ClientID%>').selectedIndex = 0;--%>
           <%-- document.getElementById('<%= ddlToDept.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlFromProcess.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlToProcess.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlAllocatedTo.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtAllocatedToId.ClientID%>').value = "";
            document.getElementById('<%= ddlRequestedBy.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtRequestedById.ClientID%>').value = "";
            document.getElementById('<%= ddlApprovedBy.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtApprovedById.ClientID%>').value = "";
            document.getElementById('<%= txtAllocatedDate.ClientID%>').value = "";
            document.getElementById('<%= txtExpRtnDate.ClientID%>').value = "";
            document.getElementById('<%= txtWorkStationNo.ClientID%>').value = "";
            //document.getElementById('<%= ddlAssetType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtPortNo.ClientID%>').value = "";
            document.getElementById('<%= txtVlan.ClientID%>').value = "";
            
            document.getElementById('<%= txtGatePassNo.ClientID%>').value = "";
            document.getElementById('<%= ddlAllocRtrn.ClientID%>').focus();--%>
        }
       <%-- function onselectionChange(evt) {
            var ddlval = $("[id*='ddlAllocRtrn'] :selected").val();
            if (ddlval == 'RETURN') {
                document.getElementById('<%=txtReturnDate.ClientID%>').disabled = false;
            }
            else if (ddlval == 'ALLOCATE' || ddlval == 'SELECT') {
                document.getElementById('<%=txtReturnDate.ClientID%>').disabled = true;
            }
        }--%>


        function ShowAllocTypeMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Asset Allocate/Return Type is not selected.';
            document.getElementById('<%= ddlAllocRtrn.ClientID%>').focus();
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
            var gridgv = document.getElementById('<%=GridView3.ClientID %>');
            var rowCount = gridgv.rows.length - 1;
            var TargetBaseControl = document.getElementById('<%= this.GridView3.ClientID %>');
            var TargetChildControl = "chkSubGridSelectAsset";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox' &&
                    Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                    var row = Inputs[n].parentNode.parentNode;
                    var temp = row.cells[3].innerText;
                    var temp1 = row.cells[4].innerText;
                    temp1 = temp1.toString().toUpperCase();
                    if (temp == 'STOCK' && temp1 == 'WORKING') {
                        Inputs[n].checked = CheckBox.checked;
                        n1++;
                    }
                    else {
                        /*CheckBox.checked = false;*/
                    }
                }
            }
            if (n1 != rowCount) { CheckBox.checked = false;}
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }
        function ChildClick(CheckBox, HCheckBox) {
            debugger;
            Counter = parseInt("0");
            var TargetBaseControl = document.getElementById('<%=GridView3.ClientID %>');
            var TargetChildControl = "chkSubGridSelectAsset";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var row = CheckBox.parentNode.parentNode;
            var checkStatus = row.cells[3].innerText;
            var temp1 = row.cells[4].innerText;
            temp1 = temp1.toString().toUpperCase();
            if (checkStatus == 'STOCK' && temp1 == 'WORKING') {
                for (var n = 0; n < Inputs.length; ++n) {
                    if (Inputs[n].type == 'checkbox' &&
                        Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                        if (Inputs[n].checked)
                            Counter++;
                    }
                }
            <%--TotalChkBx = parseInt('<%=this.GridView3.Rows.Count %>');--%>
                var gridgv = document.getElementById('<%=GridView3.ClientID %>');
                TotalChkBx = gridgv.rows.length - 1;
            <%--//TotalChkBx = document.getElementByID('<%=GridView3.ClientID%>').rows.length;--%>
                Counter = HCheckBox.checked ? TotalChkBx : Counter;
                var HeaderCheckBox = document.getElementById(HCheckBox);
                //if (CheckBox.checked && Counter < TotalChkBx)
                //    Counter++;
                //else if (Counter > 0)
                //    Counter--;
                if (Counter < TotalChkBx)
                    HeaderCheckBox.checked = false;
                else if (Counter == TotalChkBx)
                    HeaderCheckBox.checked = true;
            }
            else {
                CheckBox.checked = false;
                alert("status not belongs to STOCK & Sub Status not belongs to Working");
                return false;
            }
            
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
            Asset Allocation
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
                                    <td style="text-align: right">
                                        <asp:Label ID="Label13" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label14" runat="server" Text="Allocation To" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left" >
                                        <asp:DropDownList ID="ddlAllocTo" Width="200px" runat="server" CssClass="dropdownlist"
                                            TabIndex="1" ToolTip="Select Allocation to" AutoPostBack="true" OnSelectedIndexChanged="ddlAllocTo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label29" runat="server" Text="Sub Allocation To" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlSubAllocTo" Width="200px" runat="server"  AutoPostBack="true"
                                            CssClass="dropdownlist" TabIndex="3" ToolTip="Select Sub Allocation To" OnSelectedIndexChanged="ddlSubAllocTo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label22" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Text="Allocation Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlAllocRtrn" Width="200px" runat="server" CssClass="dropdownlist" AutoPostBack="true"
                                            TabIndex="2" ToolTip="Select Allocation type" OnSelectedIndexChanged="ddlAllocRtrn_SelectedIndexChanged">
                                            <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                            <asp:ListItem Text="Permanent" Value="Permanent"></asp:ListItem>
                                            <asp:ListItem Text="Temporary" Value="Temporary"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddlAllocRtrn"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>
                                    </td>

                                    <td style="text-align: right">
                                        <asp:Label ID="Label26" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label25" runat="server" Text="Site Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlSiteLocation" Width="200px" runat="server"  AutoPostBack="true"
                                            CssClass="dropdownlist" OnSelectedIndexChanged="ddlSiteLocation_SelectedIndexChanged"
                                            TabIndex="3" ToolTip="Select Location">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="Floor" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlFacilitiesFloor" Width="200px" runat="server" CssClass="dropdownlist" Enabled="false"
                                            TabIndex="4" ToolTip="Select Store" AutoPostBack="true" OnSelectedIndexChanged="ddlFacilitiesFloor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label24" runat="server" Text="Meeting/Training Room" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlMeetingRoom" Width="200px" runat="server" CssClass="dropdownlist"
                                            TabIndex="1" ToolTip="Select Meeting/Training Room">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr  class="all-asset-display-none IT-display">
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" Font-Bold="true" Text="" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label5" runat="server" Text="Employee" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtEmployeeID" Style="margin-left: 0px;" runat="server" CssClass="textbox" TabIndex="4"
                                            MaxLength="50" Width="200px" ToolTip="Enter employee id for fetch Details"></asp:TextBox>
                                        <asp:ImageButton ID="btnGet" runat="server" TabIndex="21" ImageUrl="~/images/go_button.png"
                                            OnClick="btnGet_Click" ToolTip="Enter serial no."
                                            Height="25px" Width="25px" />
                                    </td>

                                    <td style="text-align: right">
                                       <%-- <asp:Label ID="Label15" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>--%>
                                        <asp:Label ID="Label16" runat="server" Text="Host Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                      <td style="text-align: left">
                                        <asp:TextBox ID="txtHostName" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="11" ToolTip="Enter Host Name" Width="200px"></asp:TextBox>
                                    </td>

                                    
                                </tr>
                                <tr class="all-asset-display-none IT-display">
                                    <td style="text-align: right">
                                        <asp:Label ID="Label7" runat="server" CssClass="label" Text="Exp. Return Date"></asp:Label>
                                        <asp:Label ID="lblExpRtnDate1" Visible="false" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" Style="margin-left: 0px !important;" ID="txtExpRtnDate" TabIndex="7" runat="server" CssClass="textbox"
                                            onfocus="showCalendarControl(this);" Text="" ToolTip="Enter asset's expected return date"
                                            Width="200px" Enabled="false"></asp:TextBox>
                                       <%-- <asp:RequiredFieldValidator ID="rfvExpRtnDate" runat="server" ControlToValidate="txtExpRtnDate" Enabled="false"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" runat="server" CssClass="label" Text="No of Due Days"></asp:Label>
                                        <asp:Label ID="lblenternoofduedate" Visible="false" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="enternoofduedate" TabIndex="8" runat="server" CssClass="textbox"
                                            Text="" Enabled="false"
                                            Width="200px"></asp:TextBox>
                                        <%--<asp:RegularExpressionValidator ID="rfvPhoneNo" runat="server" ControlToValidate="enternoofduedate"
                                            ErrorMessage="[Numeric Only]" ValidationExpression="^[0-9]$"
                                             CssClass="validation">
                                        </asp:RegularExpressionValidator>--%>
                                        <%--<asp:RequiredFieldValidator ID="rfventernoofduedate" runat="server" ControlToValidate="enternoofduedate" Enabled="false"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>

                                </tr>
                                <%--<tr>

                                   <td style="text-align: right">
                                        <asp:Label ID="Label9" runat="server" CssClass="label" Text="RFID Tag"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left; vertical-align: center">
                                        <asp:TextBox autocomplete="off" ID="txtAssetCode" runat="server" CssClass="textbox" TabIndex="9"
                                            MaxLength="50" Width="200px" ToolTip="Enter first 5 characters of serial no /tag to search"></asp:TextBox>
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
                                            WatermarkText="Search RFID Tag..." WatermarkCssClass="watermarked" />
                                        &nbsp;
                                                    <asp:ImageButton ID="btnGo" runat="server" TabIndex="21" ImageUrl="~/images/go_button.png"
                                                        OnClick="btnGo_Click" ToolTip="Enter serial no."
                                                        Height="25px" Width="25px" />
                                    </td>

                                    
                                </tr>--%>
                                <tr>                                 
                                    <td style="text-align: right">
                                        <asp:Label ID="Label11" runat="server" CssClass="label" Text="Identifier Location"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtIdentifierlocation" TabIndex="10" runat="server" CssClass="textbox"
                                            Text=""
                                            Width="200px"></asp:TextBox>

                                    </td>

                                    <td style="text-align: right">
                                        <asp:Label ID="Label12" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Allocation Date " CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtAllocationDate" runat="server"
                                            onfocus="showCalendarControl(this);" ToolTip="Enter/select asset return date"
                                            Width="200px" TabIndex="5" CssClass="textbox"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr class="all-asset-display-none IT-display">                                   
                                     <td style="text-align: right" >
                                        <asp:Label ID="Label6" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label10" runat="server" Text="Ticket No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" >
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                  <td style="text-align: left" >
                                        <asp:TextBox autocomplete="off" ID="txtTicketNo" TabIndex="6" runat="server" MaxLength="50"
                                            CssClass="textbox" Width="200px" ToolTip="Enter ticket no."></asp:TextBox>
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTicketNo"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>
                                     <td style="text-align: right">
                                        <asp:Label ID="Label27" runat="server" Text="Seat No" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtSeatNo1" TabIndex="3" runat="server" MaxLength="50"
                                            CssClass="textbox" Width="200px" Enabled="false" ToolTip="Enter Seat No."></asp:TextBox>
                                    </td>
                                </tr>
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

                                    <td style="text-align: right"></td>
                                    <td style="text-align: center"></td>
                                    <td style="text-align: left"></td>
                                </tr>
                                <%--<table id="Table4" cellspacing="15" style="width: 100%;" align="center">
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
                                    <td colspan="6">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label28" runat="server" Text="Select Asset Allocation Data File :" CssClass="label"></asp:Label>
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
                                                <asp:PostBackTrigger ControlID="btnUploadFile" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <td style="text-align: center" class="all-asset-display-none Facilities-display">
                                            <a href="DownloadSample/Facilities_Allocation.xlsx">
                                                <input type="button" class="button btn btn-secondary" value="Download Template" />
                                            </a>
                                        </td>
                                         <td style="text-align: center" class="all-asset-display-none IT-display">
                                            <a href="DownloadSample/IT_Allocation.xlsx">
                                                <input type="button" class="button btn btn-secondary" value="Download Template" />
                                            </a>
                                        </td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <table id="tblAssets" runat="server" cellspacing="12" width="100%" style="border: 2px double #006600;"
                                            align="center">
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="Label17" Font-Bold="true" Text="To search the Asset for allocation..." CssClass="ErrorLabel"
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
                                                    <asp:Label ID="Label19" runat="server" Enabled="true" CssClass="label" Text="Asset Model : "></asp:Label>
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
                                                        MaxLength="50" Width="200px" ToolTip="Enter first 3 characters of Invoice No to search"></asp:TextBox>
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
                                        <asp:GridView ID="GridView1" runat="server" Visible="false" AllowPaging="True" AutoGenerateColumns="False"
                                            ShowFooter="false" CssClass="mGrid"
                                            AlternatingRowStyle-CssClass="alt"
                                            PageSize="50">
                                            <Columns>
                                                <asp:TemplateField>

                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TagID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpTag" runat="server" Text='<%#Eval("EMP_TAG") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="EMP ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpID" runat="server" Text='<%#Eval("EMPLOYEE_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Emp Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpMail" runat="server" Text='<%#Eval("EMP_EMAIL") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="Floor">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpFloor" runat="server" Text='<%#Eval("Emp_Floor") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Seat No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblseatno" runat="server" Text='<%#Eval("SeatNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Designation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblde" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Process Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProcess" runat="server" Text='<%#Eval("ProcessName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="LOB">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbllob" runat="server" Text='<%#Eval("Lob") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SUB LOB">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsub" runat="server" Text='<%#Eval("SubLOB") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <PagerStyle CssClass="pgr"></PagerStyle>
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>
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
                                                            Text=" Select" /> <%--onclick="javascript:HeaderViewClick(this);"--%>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectAsset" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
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
                                                <asp:TemplateField HeaderText="Serial No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSrno" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
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
                                                <asp:TemplateField HeaderText="HDD">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHdd" runat="server" Text='<%#Eval("ASSET_HDD") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RAM">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRam" runat="server" Text='<%#Eval("ASSET_RAM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PROCESSOR">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProcessor" runat="server" Text='<%#Eval("ASSET_PROCESSOR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pgr"></PagerStyle>
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="GridView3" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            ShowFooter="false" CssClass="mGrid" Visible="false"
                                            PagerStyle-CssClass="pgr" OnRowCreated="GridView3_RowCreated"
                                            AlternatingRowStyle-CssClass="alt"
                                            PageSize="500">
                                            <Columns>
                                                <asp:TemplateField>

                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkHSubGridSelect" runat="server" onclick="javascript:HeaderViewClick(this);"
                                                            Text=" Select" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSubGridSelectAsset" runat="server" />
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
                                                <asp:TemplateField HeaderText="Serial No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSrno" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
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
                                                <asp:TemplateField HeaderText="HDD">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHdd" runat="server" Text='<%#Eval("ASSET_HDD") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RAM">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRam" runat="server" Text='<%#Eval("ASSET_RAM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PROCESSOR">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProcessor" runat="server" Text='<%#Eval("ASSET_PROCESSOR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pgr"></PagerStyle>
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="center" colspan="6">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" Text="Save Asset Allocation" Enabled="false" Visible="false" CssClass="button"
                                            TabIndex="15" Width="150px" CausesValidation="true" ToolTip="Save Asset Allocation Details"
                                            OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" OnClientClick="ClearFields();" Text="Reset" CssClass="button"
                                            TabIndex="16" Width="60px" CausesValidation="false" ToolTip="Refresh/Reset fields"
                                            OnClick="btnClear_Click" />
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
            <tr>
                <td colspan="4" align="center">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvAssetAllocation" runat="server" AllowPaging="True" OnRowDeleting="gvAssetAllocation_RowDeleting"
                                    AutoGenerateColumns="false" ShowFooter="false" CssClass="mGrid" Visible="false" PagerStyle-CssClass="pgr"
                                    PageSize="50" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvAssetAllocation_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Allocated Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAllocationDate" runat="server" Text='<%#Eval("ASSET_ALLOCATION_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetTag" runat="server" Text='<%#Eval("ASSET_TAG") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Allocation Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetLocation" runat="server" Text='<%#Eval("ALLOCATION_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Allocated To">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAllocatedTo" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Allocated Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAllocatedTag" runat="server" Text='<%#Eval("ALLOCATED_EMP_TAGID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Exp. Return Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpRtnDate" runat="server" Text='<%# Eval("EXPECTED_RTN_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAssetAllocation" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvAssetAllocation" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
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
