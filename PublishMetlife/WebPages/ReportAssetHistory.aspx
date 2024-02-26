<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="ReportAssetHistory.aspx.cs" Inherits="Report_AuditHistory" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            window.location.href = window.location;
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
          Asset History Report</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                        
                       
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="Label2" runat="server" Text="Serial No./ RFID Tag" CssClass="label"></asp:Label>
                            </td>
                            <td style="text-align: center">
                                <div style="font-weight: bold">
                                    :</div>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtActiveInAssetCode" autocomplete="off" CssClass="textbox" MaxLength="30"
                                                            ValidationGroup="Submit" runat="server" Width="200px" TabIndex="1" ToolTip="Enter first 5 characters of Asset Code to search"></asp:TextBox>
                                                        <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                                                            TargetControlID="txtActiveInAssetCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetALLAssetCode"
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
                                                            WatermarkText="Search Asset Code..." WatermarkCssClass="watermarked" />
                                                        &nbsp;
                                                       
                                                   
                            </td>
                           <td>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtActiveInAssetCode"
                                            Display="Dynamic" ErrorMessage="[Asset Code Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                           </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="center">
                                <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                    ToolTip="Get Report" ImageUrl="~/images/Submit.png"
                                    Height="35px" Width="85px" OnClick="btnSubmit_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                    ToolTip="Reset/clear fields" ImageUrl="~/images/Reset.png" Height="35px" Width="85px"
                                    CausesValidation="false" OnClick="btnClear_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="false" ToolTip="Export gate pass report into excel file"
                                    ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="text-align: left">
                                <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblRecordCount" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4" runat="server">
                    <div id="DivGrid" runat="server">
                        <asp:GridView ID="gvRptGatePass" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            OnPageIndexChanging="gvRptGatePass_PageIndexChanging" ShowFooter="false" CssClass="mGrid"
                            PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:TemplateField HeaderText="Serial No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPCode" runat="server" Text='<%#Eval("Asset_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RFID TAG">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompCode" runat="server" Text='<%#Eval("TAG_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Store">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPDate" runat="server" Text='<%#Eval("Store") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Floor">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGPType" runat="server" Text='<%#Eval("FLOOR") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EMP ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpRtnDate" runat="server" Text='<%#Eval("EMP_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EMP NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("EMP_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Allocation Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("Allocation_Date") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Return Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ReturnDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("Asset_sub_status") %>'></asp:Label>
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
    </div>
</asp:Content>

