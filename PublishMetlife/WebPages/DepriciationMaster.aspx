<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="DepriciationMaster.aspx.cs" Inherits="DepriciationMaster" Title="BCIL : ATS - Depreciation Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="..css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="..js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= ddlCategory.ClientID %>').selectedIndex = 0;
            document.getElementById('<%= txtEffDate.ClientID %>').value = "";
            document.getElementById('<%= txtDepRate.ClientID %>').value = "";
            document.getElementById('<%= txtRemarks.ClientID %>').value = "";
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
            width: 26px;
        }
        .style2
        {
            width: 300px;
        }
        .style3
        {
            width: 270px;
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
            Depreciation Master</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="Category" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlCategory" runat="server" Width="200px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_Category" runat="server" ControlToValidate="ddlCategory"
                                            ErrorMessage="*" ValidationGroup="Submit" InitialValue="SELECT"></asp:RequiredFieldValidator>
                                    </td>
                                    <td rowspan="7" valign="top">
                                        <img src="../images/Depreciation.png" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="Effective Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtEffDate" CssClass="textbox" runat="server" Width="200px" TabIndex="2"
                                            onfocus="showCalendarControl(this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_EffDate" runat="server" ControlToValidate="txtEffDate"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="Depriciation Rate" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtDepRate" CssClass="textbox" runat="server" Width="200px"></asp:TextBox>
                                        <asp:Label ID="Label4" runat="server" Text="%" Font-Bold="true" CssClass="label"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RFV_DepRate" runat="server" ControlToValidate="txtDepRate"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label6" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left" valign="top">
                                        <asp:TextBox ID="txtRemarks" CssClass="multitextbox" runat="server" TextMode="MultiLine"
                                            Width="200px" Height="50px" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Remarks" runat="server" ControlToValidate="txtRemarks"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                    </td>
                                    <td align="left" class="style1">
                                    </td>
                                    <td align="left">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" ImageUrl="~/images/Submit.png"
                                            TabIndex="11" Height="35px" Width="85px" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" OnClientClick="ClearFields();" ImageUrl="~/images/Reset.png"
                                            TabIndex="12" Height="35px" Width="85px" CausesValidation="False" />
                                    </td>
                                    <td>
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
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblErrorMsg" CssClass="ErrorLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvDepMaster" runat="server" AllowPaging="True" OnRowDeleting="gvDepMaster_RowDeleting"
                                    OnRowEditing="gvDepMaster_RowEditing" OnRowUpdating="gvDepMaster_RowUpdating"
                                    PageSize="20" OnRowCancelingEdit="gvDepMaster_RowCancelingEdit" AutoGenerateColumns="False"
                                    ShowFooter="True" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="DEPRECIATION ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepID" runat="server" Text='<%#Eval("DEPRECIATION_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEDepID" runat="server" Text='<%#Eval("DEPRECIATION_ID") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("CATEGORY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditCategory" runat="server" Text='<%#Eval("CATEGORY") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFV_EditCat" runat="server" ControlToValidate="txtEditCategory"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffDate" runat="server" Text='<%#Eval("EFFECTIVE_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditEffDate" runat="server" Text='<%#Eval("EFFECTIVE_DATE") %>'
                                                    onfocus="showCalendarControl(this);"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFV_EditEffDate" runat="server" ControlToValidate="txtEditEffDate"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Depreciation Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepRate" runat="server" Text='<%#Eval("DEPRECIATION_RATE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditDepRate" runat="server" Text='<%#Eval("DEPRECIATION_RATE") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFV_EditDepRate" runat="server" ControlToValidate="txtEditDepRate"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtERemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvRditRemarks" runat="server" ControlToValidate="txtERemarks"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="40px">
                                            <EditItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" ValidationGroup="Grid" CommandName="Update"
                                                    ImageUrl="~/images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CausesValidation="false"
                                                    CommandName="Cancel" ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" CausesValidation="false"
                                                    ImageUrl="~/images/Edit_16x16.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" CausesValidation="false"
                                                    OnClientClick="return confirm('Are you sure you want to delete?');" CommandName="Delete"
                                                    ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvDepMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepMaster" EventName="RowCancelingEdit" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
