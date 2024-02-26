<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserRights.aspx.cs" MasterPageFile="~/WebPages/MobiVUEMaster.master" Inherits="WebPages_UserRights" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function ClearFields() {
            window.location.href = window.location;
        }

        function HeaderViewClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.gvGroupRights.ClientID %>');
            var TargetChildControl = "chkView";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                    Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }

        function ChildViewClick(CheckBox, HCheckBox) {
            TotalChkBx = document.getElementById('<%= gvGroupRights.ClientID %>').rows.length;
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;
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
        function ShowAlertCommon(Message) {
            alert(Message);
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = Message.toString();
        }

        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style type="text/css">
        .style1 {
            width: 170px;
        }

        .style2 {
            width: 300px;
        }

        .style3 {
            height: 143px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>--%>
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebPages/AutoComplete.asmx" />
        </Services>
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            User Management
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
                                    <td style="text-align: right">
                                        <asp:Label ID="Label19" Font-Bold="true" Text="" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label20" runat="server" Text="Employee ID" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtEmployeeID" Style="margin-left: 0px;" runat="server" CssClass="textbox" TabIndex="1"
                                            MaxLength="50" Width="200px" ToolTip="Enter employee id for fetch Details"></asp:TextBox>
                                        <asp:ImageButton ID="btnGet" runat="server" TabIndex="2" ImageUrl="~/images/go_button.png"
                                            OnClick="btnGet_Click" ToolTip="Enter Employee Details"
                                            Height="25px" Width="25px" />
                                        <asp:RequiredFieldValidator ID="rfvEmployeeID" runat="server" ControlToValidate="txtEmployeeID" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label13" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label18" runat="server" Text="Group Code" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox autocomplete="off" ID="txtGroupCode" runat="server" CssClass="textbox" TabIndex="3" AutoPostBack="true" OnTextChanged="txtGroupCode_TextChanged"
                                            MaxLength="50" Width="200px" ToolTip="Enter first 1 characters of serial no /tag to search"></asp:TextBox>
                                        <act:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender1"
                                            TargetControlID="txtGroupCode" ServicePath="AutoComplete.asmx" ServiceMethod="GetGroupCode"
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
                                        <act:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtGroupCode"
                                            WatermarkText="Search Group Code..." WatermarkCssClass="watermarked" />
                                        &nbsp;
                                                    <asp:ImageButton ID="btnGo" runat="server" TabIndex="4" ImageUrl="~/images/go_button.png"
                                                        OnClick="btnGo_Click" ToolTip="Get Group Details"
                                                        Height="25px" Width="25px" />
                                        <asp:RequiredFieldValidator ID="rfvGroupCode" runat="server" ControlToValidate="txtGroupCode" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>

                                    </td>

                                </tr>
                                
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label23" runat="server" Text="Group Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtGroupName" runat="server" autocomplete="off" Enabled="false" CssClass="textbox" MaxLength="50" TabIndex="5" ToolTip="Enter Group Name" Width="200px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label25" runat="server" Text="Group Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtGroupRemarks" runat="server" autocomplete="off" Enabled="false" CssClass="textbox" MaxLength="50" TabIndex="6" ToolTip="Enter Group Remarks" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label2" Font-Bold="true" Text="*" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server" Text="Site Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList runat="server" ID="ddlLocation" CssClass="dropdownlist" Width="200" TabIndex="7" ToolTip="Select Site">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" InitialValue="-- Select Site --" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                    </td>
                                    
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
                                            Checked="true" CssClass="label" TabIndex="8" />
                                    </td>
                                </tr>

                            </table>

                            <table id="Table3" runat="server" cellspacing="15" style="width: 100%;" align="center">
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="GVEmployee" runat="server" Visible="false" AllowPaging="True" AutoGenerateColumns="False"
                                            ShowFooter="false" CssClass="mGrid"
                                            AlternatingRowStyle-CssClass="alt"
                                            PageSize="50">
                                            <Columns>
                                                <asp:TemplateField>

                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TagID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetTag" runat="server" Text='<%#Eval("EMP_TAG") %>'></asp:Label>
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
                                                        <asp:Label ID="lblEmpEMail" runat="server" Text='<%#Eval("EMP_EMAIL") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                                        <asp:GridView ID="gvGroupRights" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                            OnRowCreated="gvGroupRights_RowCreated" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvGroupRights_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Page Code" HeaderStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPageCode" runat="server" Text='<%#Eval("PAGE_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Page Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPageName" runat="server" Text='<%#Eval("PAGE_DESCRIPTION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="100px">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkHView" runat="server" onclick="javascript:HeaderViewClick(this);"
                                                            Text="  View" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkView" runat="server" Checked='<%#Eval("VIEW_RIGHTS") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pgr"></PagerStyle>
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="Submit" TabIndex="12"
                                            ToolTip="Save User Details" Text="Save" CssClass="button" OnClick="btnSubmit_Click"
                                            Width="110px" />
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
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Store Master Data Into Excel File"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" Visible="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
