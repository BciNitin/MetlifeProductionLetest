<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserGroupMapping.aspx.cs" MasterPageFile="~/WebPages/MobiVUEMaster.master" Inherits="WebPages_UserGroupMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" src="../js/CalendarControl.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        var TotalChkBx;
        var Counter;

        function ClearFields() {
            window.location.href = window.location;
        }
        function noBack() {
            window.history.forward(1);
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
    </script>

    <style type="text/css">
        .style1 {
            width: 113px;
        }

        .style2 {
            width: 110px;
        }

        .style3 {
            height: 143px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebPages/AutoComplete.asmx" />
        </Services>
    </act:ToolkitScriptManager>
    <div id="wrapper">
        <div id="pageTitle">
            USER GROUP MAPPING
        </div>
    </div>
    <div id="wrapper1">
        <table id="Table2" runat="server" cellspacing="10" style="width: 100%" align="center">
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label52" Font-Bold="true" Text="* marked fields are mandatory. " CssClass="ErrorLabel"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table1" runat="server" cellspacing="15" style="width: 100%;" align="center">
                                 <tr>
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
                                        <asp:TextBox ID="txtGroupCode" runat="server" CssClass="textbox" TabIndex="3" 
                                            MaxLength="50" Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvGroupCode" runat="server" ControlToValidate="txtGroupCode" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>

                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label23" runat="server" Text="Group Name" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold;">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtGroupName" runat="server" CssClass="textbox" MaxLength="50" TabIndex="5" Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGroupName" CssClass="validation" ValidationGroup="Submit" Display="Dynamic" ErrorMessage="[Required]"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label25" runat="server" Text="Group Remarks" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtGroupRemarks" runat="server" autocomplete="off" CssClass="textbox" MaxLength="50" TabIndex="6" ToolTip="Enter Group Remarks" Width="200px"></asp:TextBox>
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
                                <tr runat="server" visible="false" id="EmpVisibility">
                                    <td style="text-align: right">
                                        <asp:Label ID="Label19" Font-Bold="true" Text="" CssClass="ErrorLabel" runat="server"></asp:Label>
                                        <asp:Label ID="Label20" runat="server" Text="Employee ID" CssClass="label" ></asp:Label>
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
                                        <asp:Label ID="Label5" runat="server" Text="Select Location" CssClass="label"></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <div style="font-weight: bold">
                                            :</div>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:CheckBoxList ID="CheckBoxListLoc" runat="server" RepeatDirection="Vertical" AutoPostBack="True" >
                                            </asp:CheckBoxList>
                                    
                                        <asp:DropDownList ID="ddlLocation" Visible="false" ToolTip="Select Location Name" CssClass="dropdownlist" runat="server" Width="200px" TabIndex="3">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                         <asp:Button ID="btnUpdateMapLocation"  runat="server" Text="Update User Location Mapping" CssClass="button"
                                             Width="200px" ToolTip="Update User Location Mapping" 
                                        onclick="btnUpdateMapLocation_Click"    />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                 </table>
                            <table id="Table3" runat="server" cellspacing="15" style="width: 100%;" align="center">
                               <tr>   
                                   <td colspan="4">
                                       <div style="width:1200px; overflow:auto">
                                        <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                                            ShowFooter="false" CssClass="mGrid" 
                                                        PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt"
                                            PageSize="500">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Group Code">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGroupCode" runat="server" Text='<%#Eval("GROUP_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Group Name">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("GROUP_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Group Remarks">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGroupRemarks" runat="server" Text='<%#Eval("REMARKS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Created On">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCON" runat="server" Text='<%#Eval("CREATED_ON") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ACTIVE">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("ACTIVE") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="30px">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imagebuttonEdit" 
                                                            ImageUrl="~/images/Edit_16x16.png" runat="server" AutoPostBack="true" OnClick="imagebuttonEdit_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Add Emp" HeaderStyle-Width="30px">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imagebuttonEmpAdd" 
                                                            ImageUrl="~/images/empadd.png" runat="server" AutoPostBack="true" OnClick="imagebuttonEmpAdd_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Manage Emp" HeaderStyle-Width="30px">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imagebuttonEmpManage" 
                                                            ImageUrl="~/images/empManage.png" runat="server" AutoPostBack="true" OnClick="imagebuttonEmpManage_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="30px">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imagebuttonDelete" OnClientClick="return confirm('Are you sure to delete?');"
                                                            ImageUrl="~/images/Delete_16x16.png" runat="server" AutoPostBack="true" OnClick="imagebuttonDelete_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <PagerStyle CssClass="pgr"></PagerStyle>
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        </asp:GridView> 
                                           </div>
                                       </td>
                                </tr>
                                       <tr>
                                    <td colspan="4">
                                        <div style="width:1200px; overflow:auto">
                                        <asp:GridView ID="GVEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            ShowFooter="false" CssClass="mGrid" Visible="false"
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
                                             </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div style="width:1200px; overflow:auto">
                                        <asp:GridView ID="gvGroupRights" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                            OnRowCreated="gvGroupRights_RowCreated" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvGroupRights_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Page Code" HeaderStyle-Width="120px">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkHView" runat="server" onclick="javascript:HeaderViewClick(this);"
                                                            Text="  View" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkView" runat="server" Checked='<%#Eval("VIEW_RIGHTS") %>' OnClick="if(!confirm('Are you sure you want to sign out?'))return false;"
                                                             />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pgr"></PagerStyle>
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        </asp:GridView>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div style="width:1200px; overflow:auto">
                                        <asp:GridView ID="GvGroupEmployee" runat="server" AllowPaging="false" AutoGenerateColumns="false" Visible="false"
                                            OnRowCreated="GvGroupEmployee_RowCreated" ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="GvGroupEmployee_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Group Code" HeaderStyle-Width="50px">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGVGroupEMPGroupCode" runat="server" Text='<%#Eval("GROUP_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Group Name" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGVGroupEMPGroupName" runat="server" Text='<%#Eval("GROUP_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Exist Group Code" HeaderStyle-Width="50px">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExistGroupCode" runat="server" Text='<%#Eval("EXIST_GROUP_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Exist Group Name" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExistGroupName" runat="server" Text='<%#Eval("EXIST_GROUP_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee Code" HeaderStyle-Width="50px">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("EMPLOYEE_CODE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("EMPLOYEE_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Update Location" HeaderStyle-Width="30px">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imageLocationbuttonEdit" 
                                                            ImageUrl="~/images/Edit_16x16.png" runat="server" AutoPostBack="true" OnClick="imageLocationbuttonEdit_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="50px">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkHEmpView" runat="server" AutoPostBack="true" OnCheckedChanged="chkHeaderEmpView_OnCheckedChanged"
                                                            Text="  View" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <%--<ItemTemplate>
                                                        <asp:CheckBox ID="chkEmpView" runat="server" Checked='<%#Convert.ToBoolean(Eval("ASSIGNED")) %>' 
                                                            AutoPostBack="true" OnCheckedChanged="chkEmpView_OnCheckedChanged" OnClick="if(!confirm('Are you sure you want change the Group of the Employee ?'))return false;"/>
                                                    </ItemTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkEmpView" runat="server" Checked='<%#Convert.ToBoolean(Eval("ASSIGNED")) %>' 
                                                            AutoPostBack="true" OnCheckedChanged="chkEmpView_OnCheckedChanged"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pgr"></PagerStyle>
                                            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        </asp:GridView>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="6">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="button"
                                            TabIndex="26" Width="200px" ToolTip="Save User Group Mapping" 
                                            OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnRemoveEmpOnGroup" runat="server" Text="Remove Employees from Group" CssClass="button"
                                            TabIndex="28" Width="200px" ToolTip="Remove User with Mapping" Visible="false"
                                            OnClick="btnRemoveEmpOnGroup_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" OnClientClick="ClearFields();" Text="Reset" CssClass="button"
                                            TabIndex="27" Width="60px" CausesValidation="false" ToolTip="Refresh/Reset fields"/>
                                    </td>
                                </tr>
                          </table>  
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>


        </table>
        <table>
               <tr>
                                    <td colspan="2" style="text-align: right">
                            <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Report" Visible="false"
                                ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click"/>
                        </td>
                                </tr>
        </table>
    </div>
</asp:Content>