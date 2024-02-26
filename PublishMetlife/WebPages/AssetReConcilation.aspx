<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master"
    AutoEventWireup="true" CodeFile="AssetReConcilation.aspx.cs" Inherits="AssetReConcilation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ClearFields() {

          
           document.getElementById('<%= lblErrorMsg.ClientID%>').value = "";
            document.getElementById('<%= txtReConcileDate.ClientID%>').value = "";
           
            document.getElementById('<%= txtReConcileDate.ClientID%>').disabled = false;
            window.location.href = window.location;
            
        }
        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            ASSET RECONCILIATION
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table1" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <table id="Table2" runat="server" style="width: 100%;" cellspacing="10" align="center">
                        <tr style="display:none;">
                            <td style="text-align: right">
                                <asp:Label ID="Label1" runat="server" Enabled="false" CssClass="label" Text="Select Asset Type"></asp:Label>
                            </td>
                            <td style="text-align: center">
                                <div style="font-weight: bold">
                                    :</div>
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlAssetType" Enabled="false" Width="200px" ToolTip="Select asset type"
                                    runat="server" TabIndex="5" CssClass="dropdownlist">
                                    <asp:ListItem Text="-- Select Type --" Value="SELECT"></asp:ListItem>
                                    <asp:ListItem Text="ADMIN ASSET" Value="ADMIN"></asp:ListItem>
                                    <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFV_ASSET_TYPE" runat="server" ErrorMessage="[Required]"
                                    ControlToValidate="ddlAssetType" CssClass="validation" InitialValue="SELECT"
                                    ValidationGroup="Submit"></asp:RequiredFieldValidator>
                            </td>
                            <td style="text-align: right">
                            </td>
                            <td style="text-align: left">
                                <asp:Button ID="btnNewReconcile" CssClass="button" Width="100px" runat="server" Text="New Reconcile" 
                                    OnClick="btnNewReconcile_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="Label3" runat="server" CssClass="label" Text="Reconciliation Date"></asp:Label>
                            </td>
                            <td style="text-align: center">
                                <div style="font-weight: bold">
                                    :</div>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox autocomplete="off" ID="txtReConcileDate" CssClass="textbox" runat="server"
                                    onfocus="showCalendarControl(this);" ToolTip="Enter/select asset reconciliation date"
                                    Width="200px" TabIndex="4"></asp:TextBox>
                                 <asp:Button ID="btnGetData" runat="server" Text="Get Data" ToolTip="Get reconciled assets list"
                                    OnClick="btnGetData_Click" />
                            </td>
                          
                           
                        </tr>
                        <tr>
                         

                           
                            <td style="text-align: center" colspan="3">
                                  <asp:Button ID="btnSubmit" runat="server"  Text="ReConcilation Data"  CssClass="button"
                                            TabIndex="26" Width="150px" ToolTip="ReConcile" style="margin-bottom:20px;"
                                            OnClick="ibtnUpload_Click" />
                                 <asp:ImageButton ID="btnClear" runat="server" TabIndex="13" OnClientClick="ClearFields();"
                                    ToolTip="Reset/clear fields" ImageUrl="~/images/Reset.png" Height="35px" Width="75px"
                                    CausesValidation="false" />
                                 <asp:Button ID="btnReconcilationConfirmation" runat="server"  visible="false" Text="Confirm ReConcilation"  CssClass="button"
                                            TabIndex="26" Width="150px" ToolTip="ReConcile" style="margin-bottom:20px;"
                                            OnClick="Button1_Click" />
                               
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left" colspan="6">
                                <table id="tblAssets" runat="server" cellspacing="10" width="100%" style="border: 2px double #006600;"
                                    align="center">
                                    <tr style="border: 2px double #006600;">
                                        <td style="vertical-align: top">
                                            <asp:GridView ID="gvCodes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                PageSize="50" OnPageIndexChanging="gvCodes_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tag ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("TAG_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Found At Location">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reconcile By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("RECONCILED_BY") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="pgr"></PagerStyle>
                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </td>
                                        <td style="vertical-align: top">
                                            <asp:GridView ID="gvInvalidCodes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                PageSize="50" OnPageIndexChanging="gvInvalidCodes_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Invalid Serial No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("SERIAL_CODES") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Location">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLoc" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                               <%--     <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("RECONCILE_DATE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Reconcile By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("RECONCILED_BY") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="pgr"></PagerStyle>
                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:GridView ID="gvReconciledData" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                PageSize="100" OnPageIndexChanging="gvReconciledData_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Asset Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCodes" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="TAG_ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSerial" runat="server" Text='<%#Eval("TAG_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actual Location">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLocName" runat="server" Text='<%#Eval("LOC_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("STATUS") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="pgr"></PagerStyle>
                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export assets list into excel file"
                                                ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click"
                                                Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
