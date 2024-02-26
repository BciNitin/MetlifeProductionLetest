<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="CustomerMaster.aspx.cs" Inherits="CustomerMaster" Title="BCIL : ATS - Customer Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="..css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="..js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= txtCustomerCode.ClientID%>').value = "";
            document.getElementById('<%= txtCustomerName.ClientID%>').value = "";
            document.getElementById('<%= ddlCompanyID.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtCustomerTag.ClientID%>').value = "";
            document.getElementById('<%= txtAdd1.ClientID%>').value = "";
            document.getElementById('<%= txtAdd2.ClientID%>').value = "";
            document.getElementById('<%= ddlCountry.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlState.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= ddlCity.ClientID%>').selectedIndex = 0;
            document.getElementById('<%= txtPIN.ClientID%>').value = "";
            document.getElementById('<%= txtEffectiveDate.ClientID%>').value = "";
            document.getElementById('<%= txtEmailID.ClientID%>').value = "";
            document.getElementById('<%= txtPhone.ClientID%>').value = "";
            document.getElementById('<%= txtRemarks.ClientID %>').value = "";
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
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
            width: 280px;
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
            Customer Master
        </div>
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
                                        <asp:Label ID="Label1" runat="server" Text="Customer Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtCustomerCode" CssClass="textbox" runat="server" Width="200px"
                                            TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_CustCode" runat="server" ControlToValidate="txtCustomerCode"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="Customer Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtCustomerName" CssClass="textbox" runat="server" Width="200px"
                                            TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_CustName" runat="server" ControlToValidate="txtCustomerName"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label14" runat="server" Text="Company ID" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlCompanyID" runat="server" Width="200px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_CmpId" runat="server" ControlToValidate="ddlCompanyID"
                                            InitialValue="SELECT" ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label15" runat="server" Text="Customer Tag" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtCustomerTag" CssClass="textbox" runat="server" Width="200px"
                                            TabIndex="4"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCustomerTag"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label4" runat="server" Text="Address Line 1" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtAdd1" CssClass="textbox" runat="server" Width="200px" TabIndex="3"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Add1" runat="server" ControlToValidate="txtAdd1"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="Address Line 2" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtAdd2" CssClass="textbox" runat="server" Width="200px" TabIndex="4"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Add2" runat="server" ControlToValidate="txtAdd2"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="Country" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlCountry" CssClass="dropdownlist" runat="server" Width="200px"
                                            TabIndex="5">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_Country" runat="server" ControlToValidate="ddlCountry"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label7" runat="server" Text="State" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlState" CssClass="dropdownlist" runat="server" Width="200px"
                                            TabIndex="6">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_State" runat="server" ControlToValidate="ddlState"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" runat="server" Text="City" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlCity" CssClass="dropdownlist" runat="server" Width="200px"
                                            TabIndex="7">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFV_City" runat="server" ControlToValidate="ddlCity"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" runat="server" Text="PIN Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtPIN" CssClass="textbox" runat="server" Width="200px" TabIndex="8"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_PIN" runat="server" ControlToValidate="txtPIN"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label9" runat="server" Text="Effective Date" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtEffectiveDate" CssClass="textbox" runat="server" Width="200px"
                                            TabIndex="9" onfocus="showCalendarControl(this);">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_EffDate" runat="server" ControlToValidate="txtEffectiveDate"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label10" runat="server" Text="Phone No." CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtPhone" CssClass="textbox" runat="server" Width="200px" TabIndex="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Phone" runat="server" ControlToValidate="txtPhone"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label11" runat="server" Text="E-Mail ID" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtEmailID" CssClass="textbox" runat="server" Width="200px" TabIndex="11">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Email" runat="server" ControlToValidate="txtEmailID"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>&nbsp;
                                        <asp:RegularExpressionValidator ID="REP_Email" runat="server" ControlToValidate="txtEmailID"
                                            ErrorMessage="[Invalid E-Mail]" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
                                            CssClass="validation">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label12" runat="server" Text="Set Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:CheckBox ID="chkSetStatus" runat="server" Text="  Active" Checked="true" CssClass="label"
                                            TabIndex="12" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label13" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold;">
                                            :</div>
                                    </td>
                                    <td colspan="2" style="text-align: left" valign="top">
                                        <asp:TextBox ID="txtRemarks" CssClass="multitextbox" TextMode="MultiLine" TabIndex="13"
                                            runat="server" Height="50px" Width="200px">
                                        </asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RFV_Remarks" runat="server" ControlToValidate="txtRemarks"
                                            ErrorMessage="*" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:ImageButton ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="14"
                                            ImageUrl="~/images/Submit.png" Height="35px" Width="85px" OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="15" OnClientClick="ClearFields();"
                                            ImageUrl="~/images/Reset.png" Height="35px" Width="85px" CausesValidation="False" />
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
                    <div id="dv" style="max-height: 350px; overflow: auto">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvCustMaster" runat="server" AllowPaging="True" OnRowDeleting="gvCustMaster_RowDeleting"
                                    OnRowEditing="gvCustMaster_RowEditing" 
                                    OnRowUpdating="gvCustMaster_RowUpdating" PageSize="50"
                                    OnRowCancelingEdit="gvCustMaster_RowCancelingEdit" AutoGenerateColumns="False"
                                    ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" 
                                    AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Customer Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustCode" runat="server" Text='<%#Eval("CUSTOMER_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECustCode" runat="server" Text='<%#Eval("CUSTOMER_CODE") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustName" runat="server" Text='<%#Eval("CUSTOMER_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtECustName" ValidationGroup="Grid" runat="server" Text='<%#Eval("CUSTOMER_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvECustName" runat="server" ControlToValidate="txtECustName"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompID" runat="server" Text='<%#Eval("COMPANY_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlECompanyID" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvECompID" runat="server" ControlToValidate="ddlECompanyID"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustTag" runat="server" Text='<%#Eval("CUSTOMER_TAG") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtECustTag" ValidationGroup="Grid" runat="server" Text='<%#Eval("CUSTOMER_TAG") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvECustTag" runat="server" ControlToValidate="txtECustTag"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdd" runat="server" Text='<%#Eval("ADDRESS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEAdd" ValidationGroup="Grid" runat="server" Text='<%#Eval("ADDRESS") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEAdd" runat="server" ControlToValidate="txtEAdd"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Country">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("COUNTRY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlECountry" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvECountry" runat="server" ControlToValidate="ddlECountry"
                                                    InitialValue="SELECT" Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="State">
                                            <ItemTemplate>
                                                <asp:Label ID="lblState" runat="server" Text='<%#Eval("STATE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEState" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvEState" runat="server" ControlToValidate="ddlEState"
                                                    InitialValue="SELECT" Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="City">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server" Text='<%#Eval("CITY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlECity" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvECity" runat="server" ControlToValidate="ddlECity"
                                                    InitialValue="SELECT" Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Phone">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhone" runat="server" Text='<%#Eval("PHONE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEPhone" ValidationGroup="Grid" runat="server" Text='<%#Eval("PHONE") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEPhone" runat="server" ControlToValidate="txtEPhone"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="E-Mail">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("EMAIL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEEMail" ValidationGroup="Grid" runat="server" Text='<%#Eval("EMAIL") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEEMail" runat="server" ControlToValidate="txtEEMail"
                                                    Text="*" ValidationGroup="Grid" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ACTIVE">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Eval("ACTIVE") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%#Eval("ACTIVE") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit/Delete">
                                            <EditItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonUpdate" ToolTip="Update" CausesValidation="false"
                                                    CommandName="Update" ImageUrl="~/images/Update_icon.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonCancel" ToolTip="Cancel" CommandName="Cancel" CausesValidation="false"
                                                    ImageUrl="~/images/Cancel_16x16.png" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonEdit" ToolTip="Edit" CommandName="Edit" CausesValidation="false"
                                                    ImageUrl="~/images/Edit_16x16.png" runat="server" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="imagebuttonDelete" ToolTip="Delete" OnClientClick="return confirm('Are you sure to delete?');"
                                                    CommandName="Delete" ImageUrl="~/images/Delete_16x16.png" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvCustMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvCustMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvCustMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvCustMaster" EventName="RowCancelingEdit" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
