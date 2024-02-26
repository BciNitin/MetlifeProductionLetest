﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrainingorMeetingMaster.aspx.cs" Inherits="WebPages_TrainingorMeetingMaster" 
    MasterPageFile="~/WebPages/MobiVUEMaster.master" Title="BCIL : ATS - Training or Meeting Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            document.getElementById('<%= txtMasterName.ClientID%>').value = "";
            document.getElementById('<%= ddlFloor.ClientID%>').value = 0;
            document.getElementById('<%= ddlSite.ClientID%>').value = 0;
            document.getElementById('<%= ddlMasterType.ClientID%>').value = 0;
            document.getElementById('<%= txtRemarks.ClientID %>').value = "";
            document.getElementById('<%= chkSetStatus.ClientID %>').checked = false;
            document.getElementById('<%= lblErrorMsg.ClientID %>').innerHTML = '';
            window.location.href = window.location;

        }
        function ShowSelfRptToMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowErrMsg(Message) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to execute this operation!');
        }
        <%--function Validate() {
            var x = document.getElementById('<%= txtPhone.ClientID%>').value;
            if (isNaN(x) || x.indexOf(" ") != -1) {
                alert("Enter numeric value");
                document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Enter Numeric Value for Mobile No.';
                return false;
            }
            if (x.length < 10) {
                alert("Enter Min. 10 Digits");
                document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Enter minimum 10 digits';
                return false;
            }
            if (x.charAt(0) != "9" || x.charAt(0) != "8" || x.charAt(0) != "7") {
                alert("it should start with 7,8 or 9");
                return false
            }
        }--%>
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
            Training/Meeting Master
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label11" Font-Bold="true" Text="* marked fields are mandatory." CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                                <tr>
                                    <td style="text-align: right" class="auto-style2">&nbsp;</td>
                                    <td style="text-align: center" class="auto-style2">&nbsp;</td>
                                    <td style="text-align: left" class="auto-style2"></td>
                                    <td style="text-align: right" class="auto-style2">&nbsp;</td>
                                    <td style="text-align: center" class="auto-style2">&nbsp;</td>
                                    <td style="text-align: left" class="auto-style2"></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">&nbsp;</td>
                                    <td style="text-align: center">&nbsp;</td>
                                    <td style="text-align: left">&nbsp;</td>
                                    <td style="text-align: right">&nbsp;</td>
                                    <td style="text-align: center">&nbsp;</td>
                                    <td style="text-align: left">&nbsp;</td>
                                </tr>
                                <tr>
                                    <%--<td style="text-align: right">
                                        <asp:Label ID="Label10" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label7" runat="server" Text="Store Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtStoreCode" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="1" ToolTip="Enter Store Code" Width="200px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revStoreCode" runat="server" Display="dynamic"
                                            ControlToValidate="txtStoreCode" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[0-9a-zA-Z ]+$" ErrorMessage="[Alphanumeric]">
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvStoreCode" runat="server" ControlToValidate="txtStoreCode" ValidationGroup="Submit" CssClass="validation"
                                            ErrorMessage="[Required]">
                                        </asp:RequiredFieldValidator>
                                    </td>--%>
                                    
                                    <td style="text-align: right" class="auto-style1">
                                        <asp:Label ID="Label3" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Text="Site Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" class="auto-style1">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left" class="auto-style1">
                                        <asp:DropDownList runat="server" ID="ddlSite" CssClass="dropdownlist" Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddlSite_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="tfvSite" runat="server" ControlToValidate="ddlSite" InitialValue="-- Select Site --" ValidationGroup="Submit" CssClass="validation" Display="Dynamic"
                                            ErrorMessage="[Required]">
                                        </asp:RequiredFieldValidator>
                                    </td>

                                    <td style="text-align: right">
                                        <asp:Label ID="Label570" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Text="Floor" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList runat="server" ID="ddlFloor" CssClass="dropdownlist"  Width="200" TabIndex="9" ToolTip="Select Floor"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvFloor" runat="server" ControlToValidate="ddlFloor" CssClass="validation" InitialValue="-- Select Floor --" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label29" runat="server" Text="Master Type" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlMasterType" Width="200px" runat="server" 
                                            CssClass="dropdownlist" TabIndex="3" ToolTip="Master Type">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvMasterType" runat="server" ControlToValidate="ddlMasterType" InitialValue="-- Select Master Type --" ValidationGroup="Submit" CssClass="validation" Display="Dynamic"
                                            ErrorMessage="[Required]">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label9" runat="server" Text="Master Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtMasterName" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="2" ToolTip="Enter Master Name" Width="200px"></asp:TextBox>
                                        <%--<asp:RegularExpressionValidator ID="revStoreName" runat="server" Display="dynamic"
                                            ControlToValidate="txtMasterName" ValidationGroup="Submit" CssClass="validation"
                                            ValidationExpression="^[a-zA-Z ]+$" ErrorMessage="[Alphabet]">
                                        </asp:RegularExpressionValidator>--%>
                                        <asp:RequiredFieldValidator ID="rfvStoreName" runat="server" ControlToValidate="txtMasterName" ValidationGroup="Submit" CssClass="validation"
                                            ErrorMessage="[Required]">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top" class="auto-style1">
                                        <asp:Label ID="Label12" runat="server" Text="Set Status" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top" class="auto-style1">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left" valign="top" class="auto-style1">
                                        <asp:CheckBox ID="chkSetStatus" runat="server" Text="  Active" ToolTip="Set Store Status"
                                            Checked="true" CssClass="label" TabIndex="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" valign="top">
                                        <asp:Label ID="Label13" runat="server" Text="Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center" valign="top">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td colspan="4" style="text-align: left" valign="top">
                                        <asp:TextBox ID="txtRemarks" runat="server" autocomplete="off" CssClass="multitextbox" Height="60px" MaxLength="200" TabIndex="5" TextMode="MultiLine" ToolTip="Enter Remarks" ValidationGroup="Submit" Width="676px">
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
                                            CausesValidation="false" OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
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
                <td colspan="4" align="center">
                    <div id="DivGrid">
                        <asp:UpdatePanel ID="upGrid" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvTrainingorMeetingMaster" runat="server" AllowPaging="True" OnRowDeleting="gvTrainingorMeetingMaster_RowDeleting"
                                    OnRowEditing="gvTrainingorMeetingMaster_RowEditing" OnRowUpdating="gvTrainingorMeetingMaster_RowUpdating"
                                    OnRowCancelingEdit="gvTrainingorMeetingMaster_RowCancelingEdit" AutoGenerateColumns="False"
                                    OnRowDataBound="gvTrainingorMeetingMaster_RowDataBound" ShowFooter="false" CssClass="mGrid"
                                    PageSize="50" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvTrainingorMeetingMaster_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Master Code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMasterCode" runat="server" Text='<%#Eval("MASTER_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>
                                                <asp:Label ID="lblEMasterCode" runat="server" Text='<%#Eval("MASTER_CODE") %>'></asp:Label>
                                            </EditItemTemplate>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Site Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>
                                                <asp:HiddenField ID="hidESite" runat="server" Value='<%#Eval("SITE_CODE") %>' />
                                                <asp:DropDownList ID="ddlESite" CssClass="dropdownlist" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlESite_SelectedIndexChanged"
                                                    ValidationGroup="Grid" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Floor">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFloor" runat="server" Text='<%#Eval("FLOOR_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                           <%-- <EditItemTemplate>
                                                <asp:HiddenField ID="hidEFloor" runat="server" Value='<%#Eval("FLOOR_CODE") %>' />
                                                <asp:DropDownList ID="ddlEFloor" CssClass="dropdownlist" Width="100px"
                                                    ValidationGroup="Grid" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Master Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMasterType" runat="server" Text='<%#Eval("MASTER_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>
                                                <asp:HiddenField ID="hidEMasterType" runat="server" Value='<%#Eval("MASTER_TYPE") %>' />
                                                <asp:DropDownList ID="ddlEMasterType" CssClass="dropdownlist" Width="100px"
                                                    ValidationGroup="Grid" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Master Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMasterName" runat="server" Text='<%#Eval("MASTER_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtEMasterName" CssClass="textbox" Width="100px"
                                                    ValidationGroup="Grid" MaxLength="50" runat="server" Text='<%#Eval("MASTER_NAME") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEEmpName" runat="server" ControlToValidate="txtEMasterName"
                                                    CssClass="validation" ErrorMessage="[Required]" ValidationGroup="Grid" />
                                            </EditItemTemplate>--%>
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox autocomplete="off" ID="txtERemarks" CssClass="textbox" Width="500px" 
                                                    ValidationGroup="Grid" MaxLength="200" runat="server" Text='<%#Eval("REMARKS") %>'></asp:TextBox>

                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Created On">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCON" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblECON" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ACTIVE">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("ACTIVE") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%#Eval("ACTIVE") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit/Delete">
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
                                <asp:AsyncPostBackTrigger ControlID="gvTrainingorMeetingMaster" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvTrainingorMeetingMaster" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvTrainingorMeetingMaster" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvTrainingorMeetingMaster" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvTrainingorMeetingMaster" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Store Master Data Into Excel File"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" Visible="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
