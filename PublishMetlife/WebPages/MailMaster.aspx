<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MailMaster.aspx.cs" Inherits="WebPages_MailMaster" MasterPageFile="~/WebPages/MobiVUEMaster.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

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
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Mail Master
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 80%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label11" Font-Bold="true" Text="* marked fields are mandatory. To enter multiple mail addresses, use ',' - comma to separate." CssClass="ErrorLabel"
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
                                        <asp:Label ID="Label10" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label7" runat="server" Text="Transaction Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList runat="server" ID="ddlTransactionType" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged" Width="200">
                                            <asp:ListItem Value="" Text="-- Please Select --"></asp:ListItem>
                                            <asp:ListItem Value="ASSET_ACQUISITION" Text="ASSET ACQUISITION"></asp:ListItem>
                                            <asp:ListItem Value="ASSET_ALLOCATION" Text="ASSET ALLOCATION"></asp:ListItem>
                                            <asp:ListItem Value="ASSET_DEALLOCATION" Text="ASSET DEALLOCATION"></asp:ListItem>
                                            <asp:ListItem Value="GATEPASS_GENERATION" Text="GATEPASS GENERATION"></asp:ListItem>
                                            <asp:ListItem Value="SCRAPPED_ASSET" Text="ASSET SCRAP"></asp:ListItem>
                                            <asp:ListItem Value="ASSET_REPLACEMENT" Text="ASSET REPLACEMENT"></asp:ListItem>
                                            <asp:ListItem Value="ASSET_RECONCILATION" Text="ASSET RECONCILATION"></asp:ListItem>
                                            <asp:ListItem Value="IUT_RECEIVING" Text="IUT RECEIVING"></asp:ListItem>
                                            <asp:ListItem Value="INVOICE_FILE_UPLOAD" Text="INVOICE FILE UPLOAD"></asp:ListItem>
                                            <asp:ListItem Value="Asset Life Alert" Text="Asset Life Alert"></asp:ListItem>
                                        </asp:DropDownList>
                                         <asp:RequiredFieldValidator ID="RFVddlTransactionType" runat="server" ControlToValidate="ddlTransactionType" ValidationGroup="Submit" CssClass="validation"
                                         InitialValue=""   ErrorMessage="[Required]">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label9" runat="server" Text="To Mail Addresses" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtToMailID" runat="server" autocomplete="off" CssClass="textbox" TabIndex="2" ToolTip="Enter To Mail Addresses" Width="500px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvToMailID" runat="server" ControlToValidate="txtToMailID" ValidationGroup="Submit" CssClass="validation"
                                            ErrorMessage="[Required]">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <%--</tr>
                                <tr>--%>
                                    <td style="text-align: right">

                                        <asp:Label ID="Label3" runat="server" Text="CC Mail Addresses" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtCCMailID" runat="server" autocomplete="off" CssClass="textbox" TabIndex="2" ToolTip="Enter CC Mail Addresses" Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Text="Mail Subject" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtMailSubject" runat="server" autocomplete="off" CssClass="textbox" TabIndex="2" ToolTip="Enter Mail Subject" Width="500px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvMailSubject" runat="server" ControlToValidate="txtMailSubject" ValidationGroup="Submit" CssClass="validation"
                                            ErrorMessage="[Required]">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <%-- </tr>
                                <tr>--%>
                                    <td style="text-align: right" >
                                        <asp:Label ID="Label5" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label13" runat="server" Text="Mail Body" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" >
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td colspan="4" style="text-align: left" valign="top">
                                        <asp:TextBox ID="txtMailBody" runat="server" autocomplete="off" CssClass="multitextbox" Height="60px" TabIndex="5" TextMode="MultiLine" ToolTip="Enter Mail Body" ValidationGroup="Submit" Width="500px">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvMailBody" runat="server" ControlToValidate="txtMailBody" ValidationGroup="Submit" CssClass="validation"
                                            ErrorMessage="[Required]">
                                        </asp:RequiredFieldValidator>
                                    </td>                                   
                                </tr>
                                <tr>
                                     <td style="text-align: right" >                                        
                                        <asp:Label ID="Label8" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" >
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td colspan="4" style="text-align: left" valign="top">
                                        <asp:TextBox ID="txtRemarks" runat="server" autocomplete="off" CssClass="multitextbox" Height="60px" TabIndex="5" TextMode="MultiLine" ToolTip="Enter Remarks" ValidationGroup="Submit" Width="500px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                            ToolTip="Save Store Details" Text="Save" CssClass="button"
                                            Width="110px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
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
                                    <td colspan="6">
                                        <div id="DivGrid1" style="width:1150px; overflow:auto">
                                            <asp:UpdatePanel ID="upGrid" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvMailMaster" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Transaction Type" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransactionType" runat="server" Text='<%#Eval("TRANSACTION_TYPE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Mail Addresses">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblToMailAddresses" runat="server" Text='<%#Eval("TO_MAIL_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CC Mail Addresses">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCCMailAddresses" runat="server" Text='<%#Eval("CC_MAIL_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Mail Subject">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMailSubject" runat="server" Text='<%#Eval("MAIL_SUBJECT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Mail Body">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMailBody" runat="server" Text='<%#Eval("MAIL_BODY") %>'></asp:Label>
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
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
