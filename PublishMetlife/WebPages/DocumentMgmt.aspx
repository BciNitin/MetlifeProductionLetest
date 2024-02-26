<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="DocumentMgmt.aspx.cs" Inherits="DocumentMgmt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtDescription.ClientID%>').value = "";
            document.getElementById('<%= txtCategory.ClientID%>').value = "";
            document.getElementById('<%= FileUpload.ClientID%>').value = "";
            document.getElementById('<%= txtRemarks.ClientID%>').value = "";
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = "Please Note : You are not authorised to execute this operation.";
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .style1
        {
            width: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            DOCUMENT MANAGEMENT
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" style="width: 100%;" align="center">
            <tr>
                <td colspan="6">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="15" align="center">
                                <tr>
                                    <td style="text-align: right; width: 16%">
                                        <asp:Label ID="Label4" runat="server" Text="Enter Description" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; width: 2%">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; width: 33%">
                                        <asp:TextBox autocomplete="off" ID="txtDescription" CssClass="textbox" MaxLength="50"
                                            runat="server" ToolTip="Enter Description" Width="200px" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescription"
                                            ErrorMessage="[Mandatory]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right; width: 15%">
                                        <asp:Label ID="Label5" runat="server" Text="Enter Category" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; width: 2%">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; width: 32%">
                                        <asp:TextBox autocomplete="off" ID="txtCategory" CssClass="textbox" MaxLength="50"
                                            runat="server" ToolTip="Enter Category" Width="200px" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCategory"
                                            ErrorMessage="[Mandatory]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label2" runat="server" Text="Attach File" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" colspan="4">
                                        <asp:FileUpload ID="FileUpload" CssClass="textbox" TabIndex="3" runat="server" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="FileUpload"
                                            ErrorMessage="[Mandatory]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; vertical-align: top">
                                        <asp:Label ID="Label1" runat="server" Text="Enter Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" colspan="4">
                                        <asp:TextBox autocomplete="off" ID="txtRemarks" CssClass="textbox" MaxLength="500"
                                            runat="server" ToolTip="Enter Remarks" TextMode="MultiLine" Width="670px" Height="200px"
                                            TabIndex="4"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRemarks"
                                            ErrorMessage="[Mandatory]" CssClass="validation" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ImageUrl="~/images/Submit.png"
                                            TabIndex="5" Height="35px" Width="85px" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="~/images/Reset.png"
                                            TabIndex="6" Height="35px" Width="85px" CausesValidation="false" />
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="text-align: center" colspan="6">
                    <div id="DivGrid">
                        <%--<asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>--%>
                        <asp:GridView ID="gvViewFiles" runat="server" AllowPaging="True" PageSize="40" AutoGenerateColumns="False"
                            ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            OnRowEditing="gvViewFiles_RowEditing" OnRowUpdating="gvViewFiles_RowUpdating" OnRowDataBound="gvViewFiles_RowDataBound"
                            OnPageIndexChanging="gvViewFiles_PageIndexChanging" OnRowCommand="gvViewFiles_RowCommand"
                            OnRowDeleting="gvViewFiles_RowDeleting" OnRowCancelingEdit="gvViewFiles_RowCancelingEdit">
                            <Columns>
                                <asp:TemplateField Visible="false" HeaderText="Serial No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrlNo" runat="server" Text='<%#Eval("SERIAL_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditSNo" runat="server" Text='<%#Eval("SERIAL_NO") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="Company Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompCode" runat="server" Text='<%#Eval("COMP_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditCompCode" runat="server" Text='<%#Eval("COMP_CODE") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Uploaded In">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompName" runat="server" Text='<%#Eval("COMPANY_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditCompName" runat="server" Text='<%#Eval("COMPANY_NAME") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("DESCRIPTION") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditDesc" TabIndex="1" MaxLength="50" runat="server" Text='<%#Eval("DESCRIPTION") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEditDesc"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("CATEGORY") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditCategory" TabIndex="2" MaxLength="50" runat="server" Text='<%#Eval("CATEGORY") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEditCategory"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditRemarks" TabIndex="3" MaxLength="500" Width="300px" Height="50px"
                                            TextMode="MultiLine" runat="server" Text='<%#Eval("REMARKS") %>'></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEditRemarks"
                                            ErrorMessage="[Required]" CssClass="validation" ValidationGroup="Grid"></asp:RequiredFieldValidator>--%>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("ATTACH_FILE_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View" HeaderStyle-Width="50px">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imagebuttonView" ToolTip="View" CommandName="View" ImageUrl="~/images/View_16x16.png"
                                            runat="server" CommandArgument='<%#Eval("ATTACH_FILE_NAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="40px">
                                    <EditItemTemplate>
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                            ImageUrl="~/images/Update_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CommandName="Cancel" CausesValidation="false"
                                            ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" ImageUrl="~/images/Edit_16x16.png"
                                            runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" OnClientClick="return confirm('Are you sure to delete file details?');"
                                            CommandName="Delete" ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="pgr"></PagerStyle>
                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                        </asp:GridView>
                        <%--</ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViewFiles" EventName="PageIndexChanging" />
                                <asp:AsyncPostBackTrigger ControlID="gvViewFiles" EventName="RowCommand" />
                                <asp:AsyncPostBackTrigger ControlID="gvViewFiles" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvViewFiles" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvViewFiles" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvViewFiles" EventName="RowDeleting" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
