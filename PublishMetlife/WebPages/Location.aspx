<%@ Page Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true"
    CodeFile="Location.aspx.cs" Inherits="Home" Title="BCIL : ATS - Location" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
       
        function noBack() {
            window.history.forward(1);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </act:ToolkitScriptManager>
    <div id="clear">
    </div>
    <div id="wrapper">
       <div id="div11" style="width: 260px; padding-top: 10px; padding-left: 45px; font: normal bold 15px  Helvetica, Arial;
                                    color: White;">
                                    Select Company/Location :
                                </div>
                                <div id="div10" style="width: 260px; padding-top: 5px; padding-left: 45px; font: normal 14px  Helvetica, Arial;
                                    color: White;">
                                    <asp:DropDownList ID="ddlCompany" TabIndex="2" runat="server" CssClass="dropdownlist"
                                        Width="200px" ToolTip="Select Company Name" AutoPostBack="true"  OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br />
                                </div>
    </div>
   
</asp:Content>
