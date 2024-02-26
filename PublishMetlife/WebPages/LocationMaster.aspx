<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="LocationMaster.aspx.cs" Inherits="LocationMaster" Title="BCIL : ATS - Location Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtLocationCode.ClientID %>').value = "";
            document.getElementById('<%= txtLocationName.ClientID %>').value = "";
            document.getElementById('<%= txtLocationDesc.ClientID %>').value = "";
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
         //   document.getElementById('<%= txtParentLocName.ClientID %>').value = "";
          //  document.getElementById('<%= txtLocationCode.ClientID %>').focus();
        }
        function ShowErrMsg(Message) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowLevelMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You cannot add the location under this parent location, the selected parent location is level 6.';
        }
        function ShowLocNotDeletedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You cannot delete this location, since it has child locations mapped.';
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
            width: 225px;
        }
        .style3
        {
            width: 260px;
        }
        .auto-style1 {
            width: 225px;
            height: 28px;
        }
        .auto-style2 {
            height: 28px;
        }
        .auto-style3 {
            width: 225px;
            height: 32px;
        }
        .auto-style4 {
            height: 32px;
        }
        .auto-style5 {
            height: 25px;
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
            Location Master</div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label19" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right" class="auto-style3">
                                        <asp:Label ID="Label8" Font-Bold="true" Text="*"  CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text="Location Code" CssClass="label" ></asp:Label>
                                    </td>
                                    <td style="text-align: center" class="auto-style4">
                                        <div style="font-weight: bold">
                                            </div>
                                    </td>
                                    <td class="auto-style4">
                                        <asp:TextBox autocomplete="off" ID="txtLocationCode" CssClass="textbox" MaxLength="50"
                                            runat="server" ToolTip="Enter Location Code" Width="200px" TabIndex="1" ></asp:TextBox>
                                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="dynamic"
                                            ControlToValidate="txtLocationCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z_-]+$" ErrorMessage="[ Alphanumeric only ]" ></asp:RegularExpressionValidator>--%>
                                    </td>
                                    <td rowspan="7" align="right" valign="top">
                                        <img src="../images/Location-Master.jpg" width="300px" height="200px" alt="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style2">
                                        <asp:Label ID="Label5" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Location Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td>
                                        <asp:TextBox autocomplete="off" ID="txtLocationName" MaxLength="25" CssClass="textbox"
                                            runat="server" ToolTip="Enter Location Name" Width="200px" TabIndex="2"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="dynamic"
                                            ControlToValidate="txtLocationName" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[ Alphanumeric only ]" Visible="False"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td style="text-align: right; vertical-align: top;" class="style2">
                                        <asp:Label ID="Label2" runat="server" Text="Description" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top;">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:TextBox autocomplete="off" ID="txtLocationDesc" style="margin-left:15px;" CssClass="multitextbox" ToolTip="Enter Location Description"
                                            MaxLength="500" runat="server" Width="200px" TabIndex="4" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style2">
                                        <asp:Label ID="Label6" runat="server" Text="Set Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td colspan="2" style="text-align: left">
                                        <asp:CheckBox ID="chkSetStatus" runat="server" Text="  Active" Checked="true" ToolTip="Set Location Status"
                                            CssClass="label" TabIndex="5" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="auto-style5">
                                        <asp:RequiredFieldValidator ID="RFV_Code" runat="server" ControlToValidate="txtLocationCode"
                                            Display="Dynamic" ErrorMessage="[Location Initials Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFV_Name" runat="server" ControlToValidate="txtLocationName"
                                            Display="Dynamic" ErrorMessage="[Location Name Required]" CssClass="validation"
                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save Location" CssClass="button" TabIndex="6"
                                            ToolTip="Save Location Details" Width="100px" ValidationGroup="Submit"
                                            OnClick="btnSubmit_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" Width="60px" Text="Reset" CssClass="button"
                                            ToolTip="Reset/Clear Fields" OnClick="btnClear_Click" OnClientClick="ClearFields();"
                                            TabIndex="7" CausesValidation="False" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="text-align: right; vertical-align: top;" class="style2">
                                        <asp:Label ID="Label4" runat="server" Text="Parent Location" CssClass="label" Visible="False"></asp:Label>
                                    </td>
                                    <td style="text-align: center; vertical-align: top;">
                                        <div style="font-weight: bold">
                                            </div>
                                    </td>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:DropDownList ID="ddlParentLocCode" TabIndex="3" AutoPostBack="true" ToolTip="Select Parent Location"
                                            OnSelectedIndexChanged="ddlParentLocCode_SelectedIndexChanged" CssClass="dropdownlist"
                                            Width="200px" runat="server" Visible="False">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="ibtnRefreshLocation" Visible="false" ToolTip="Refresh/reset parent location"
                                            runat="server" ImageUrl="~/images/Refresh_16x16.png" OnClick="ibtnRefreshLocation_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="auto-style1">
                                        <asp:Label ID="Label7" runat="server" Text="Parent Location Name" CssClass="label" Visible="False"></asp:Label>
                                    </td>
                                    <td class="auto-style2">
                                        <div style="font-weight: bold">
                                            </div>
                                    </td>
                                    <td style="text-align: left" class="auto-style2">
                                        <asp:TextBox autocomplete="off" ID="txtParentLocName" runat="server" ToolTip="Parent Location Name"
                                            CssClass="readonlytext" Width="200px" TabIndex="-1" ReadOnly="true" Wrap="False" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style2">
                                    </td>
                                    <td>
                                    </td>
                                    <td style="text-align: left">
                                        &nbsp;</td>
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
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvLocMaster" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    OnRowDataBound="gvLocMaster_RowDataBound" ShowFooter="false" CssClass="mGrid"
                                    PagerStyle-CssClass="pgr" OnRowDeleting="gvLocMaster_RowDeleting" PageSize="50"
                                    OnRowEditing="gvLocMaster_RowEditing" OnRowUpdating="gvLocMaster_RowUpdating"
                                    OnPageIndexChanging="gvLocMaster_PageIndexChanging" OnRowCancelingEdit="gvLocMaster_RowCancelingEdit"
                                    AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                
                                         <asp:TemplateField HeaderText="Location Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocCode" runat="server" Text='<%#Eval("LOC_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>
                                
                                        <asp:TemplateField HeaderText="Location Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocName" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" MaxLength="25" ID="txtEditLocName" runat="server"
                                                    Text='<%#Eval("LOC_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFV_LOCNAME" runat="server" ControlToValidate="txtEditLocName"
                                                    Text="*" ValidationGroup="Grid"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldesc" runat="server" Text='<%#Eval("DESCRIPTION") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEditDesc" MaxLength="400" runat="server" Text='<%#Eval("DESCRIPTION") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                     
                                        <asp:TemplateField HeaderText="ACTIVE">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" Enabled="false" runat="server" Checked='<%#Eval("ACTIVE") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%#Eval("ACTIVE") %>' />
                                            </EditItemTemplate>
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
                                                <asp:ImageButton ID="imagebuttonEdit" CausesValidation="false" ToolTip="Edit" CommandName="Edit"
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
                                <asp:AsyncPostBackTrigger ControlID="gvLocMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvLocMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvLocMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvLocMaster" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvLocMaster" EventName="PageIndexChanging" />
                                <asp:AsyncPostBackTrigger ControlID="gvLocMaster" EventName="RowDataBound" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="8" Enabled="true" ToolTip="Export location master data into excel file"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
