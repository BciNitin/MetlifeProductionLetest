<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="InvoiceFileUpload.aspx.cs" Inherits="WebPages_InvoiceFileUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function noBack() {
            window.history.forward(1);
        }

        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }

        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }

        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID %>').value = "";

        }

    </script>
    <style>
        .mGrid tbody tr:nth-child(2) {
            display:none;
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
            Invoice File Upload
        </div>
    </div>

    <div id="wrapper1" onload="noBack();">

        <asp:UpdatePanel ID="upSubmit" runat="server">
            <ContentTemplate>

                <table id="Table1" cellspacing="10" style="width: 100%" align="center">
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
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
                        <td colspan="4" style="text-align:center">
                             <asp:RadioButton runat="server" ID="rdbUpload" GroupName="UserLevel" Text="Upload File" CssClass="label" Checked="true"  />
                             <asp:RadioButton runat="server" ID="rdbDownload" GroupName="UserLevel" Text="Download" CssClass="label" />
                                        
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4" style="text-align:center">
                            <asp:Button runat="server" ID="btnSearch" Text=" Search " CssClass="button" OnClick="btnSearch_Click" />                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="gvAssetData" runat="server" AllowPaging="True" OnPageIndexChanging="gvAssetData_PageIndexChanging"
                                PageSize="40" AutoGenerateColumns="false" CssClass="mGrid">
                                <Columns>
                                    <%--<asp:TemplateField ItemStyle-Width="150">
                                        <HeaderStyle CssClass="hdrow" />
                                        <HeaderTemplate>
                                            <asp:Label ID="hlbleid" runat="server" Text="ASSET CODE"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbleid" runat="server" Text='<%# Eval("ASSET_CODE") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="150">
                                        <HeaderStyle CssClass="hdrow" />
                                        <HeaderTemplate>
                                            <asp:Label ID="hlbPONumber" runat="server" Text="PO Number"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPONumber" runat="server" Text='<%# Eval("PO_NUMBER") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField ItemStyle-Width="150">
                                        <HeaderStyle CssClass="hdrow" />
                                        <HeaderTemplate>
                                            <asp:Label ID="hlblInvoiceNo" runat="server" Text="Invoice No"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("INVOICE_NO") %>'>  
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="ASSET_NAME" HeaderText="ASSET NAME" ItemStyle-Width="150" />
                                    <asp:BoundField DataField="ASSET_DESC" HeaderText="ASSET DESC" ItemStyle-Width="150" />
                                    <asp:BoundField DataField="ASSET_TYPE" HeaderText="ASSET TYPE" ItemStyle-Width="150" />
                                    <asp:BoundField DataField="ASSET_LOCATION" HeaderText="ASSET LOCATION" ItemStyle-Width="150" />--%>
                                    <asp:TemplateField ItemStyle-Width="150">
                                        <HeaderTemplate>
                                            <asp:Label ID="hlbleid" runat="server" Text="Upload Asset"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <act:AsyncFileUpload ID="fileUpload" Mode="Auto"  runat="server" ClientIDMode="AutoID" OnUploadedComplete="fileUploadedComplete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="150">
                                        <HeaderTemplate>
                                            <asp:Label ID="hlbleid" runat="server" Text="Download Asset"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("FILE_NAME") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                </Columns>
                            </asp:GridView>

                        </td>
                    </tr>
                </table>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvAssetData" /> 

            </Triggers>

        </asp:UpdatePanel>
    </div>
</asp:Content>

