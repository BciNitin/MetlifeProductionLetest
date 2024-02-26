<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reports.aspx.cs" Inherits="WebPages_Reports" MasterPageFile="~/WebPages/MobiVUEMaster.master" Title="BCIL : ATS - Reports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/CalendarControl.css" type="text/css" rel="Stylesheet" />




    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript" src="../js/dropmenu.js"></script>

    <script src="../js/CalendarControl.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ClearFields() {
            var cnt = 0;
            $(".ErrorLabel").remove();
            $(".textbox").each(function () {
                $(this).val('');
            });

            $("select").each(function () {
                $(this).val('-- Please Select --');
            });
            return false;
        }
        function pageLoad(sender, args) {
            $(document).ready(function () {

                $(".Date").each(function () {
                    $(this).focus(function () {
                        showCalendarControl(this);
                    });
                });

                $("#<%= btnSearch.ClientID%>").click(function () {
                    var cnt = 0;
                    $(".ErrorLabel").remove();
                    $(".Mandatory").each(function () {
                        if (!$(this).val() || $(this).val() == "-- Please Select --") {
                            $(this).parent().append("<p class='ErrorLabel'>Please enter the " + $(this).parent().attr("data-class") + ". </p>");
                            cnt++;
                        }
                    });
                    if (cnt == 0)
                        return true;
                    else
                        return false;
                });

                <%--$("#<%= btnClear.ClientID%>").click(function () {
                    var cnt = 0;
                    $(".ErrorLabel").remove();
                    $(".textbox").each(function () {
                        $(this).val('');
                    });

                    $("select").each(function () {
                        $(this).val('-- Please Select --');
                    });
                    return false;
                });--%>

            });

        }

        function ShowErrMsg(msg) {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = msg.toString();
        }
        function ShowUnAuthorisedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : You are not authorised to execute this operation!';
        }
        function ShowAssetNotSelectedMsg() {
            document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Please Note : Asset has not been selected.';
        }
        function ShowAlert() {
            alert('Please Note : You are not authorised to execute this operation!');
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

    <style>
        .text-right {
            text-align: right;
        }

        .grid-data {
            margin: 30px;
            overflow: auto;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/WebPages/AutoComplete.asmx" />
        </Services>
    </act:ToolkitScriptManager>


    <div id="clear">
    </div>
    <div id="wrapper">
        <div id="pageTitle">
            Reports
        </div>
    </div>

    <div id="wrapper1">

        <asp:UpdatePanel ID="upSubmit" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hdnReportID" runat="server" />
                 <table id="Table2" runat="server" cellspacing="10" style="width: 100%" align="center">
                <tr>
                    <td colspan="4">
                        <asp:Label ID="Label19" Font-Bold="true" Text="Note: Please Enter date in dd-MMM-yyyy format." CssClass="ErrorLabel"
                            runat="server"></asp:Label>
                    </td>
                </tr>
              </table>
                <div class="form-control" style="width: 100%; text-align: center">
                    <h4>
                        <asp:Label runat="server" ID="lblReportName"></asp:Label>
                    </h4>
                </div>

                <table id="Table1" runat="server" cellspacing="15" style="width: 100%;" align="center">
                </table>
                <div style="width: 100%; text-align: center">
                    <asp:Label ID="lblErrorMsg" Font-Bold="true" CssClass="ErrorLabel" runat="server"></asp:Label>
                </div>
                <div style="width: 100%; text-align: center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button"
                        Width="70px" ToolTip="Search Reports" OnClick="btnSearch_Click" />

                    <asp:Button ID="btnClear" runat="server" Width="70px" Text="Clear" CssClass="button" OnClientClick="ClearFields();" OnClick="btnClear_Click"
                        ToolTip="Clear" />
                </div>
                <div class="grid-data">
                   <%-- <asp:GridView ID="gvReportData" runat="server" PageSize="50" CssClass="mGrid">
                    </asp:GridView>--%>
                     <asp:GridView ID="gvReportData" runat="server" AllowPaging="True" OnPageIndexChanging="gvReportData_PageIndexChanging"
                                                ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                PageSize="50" >
                                             
                                                <PagerStyle CssClass="pgr"></PagerStyle>
                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                            </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <table>
            <tr>
                <td></td>
                <td></td>
                <td colspan="2" style="text-align: right">
                    <asp:ImageButton ID="btnExport" runat="server" TabIndex="13" Enabled="true" ToolTip="Export Report"
                        ImageUrl="~/images/Excel-icon (2).png" CausesValidation="false" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
