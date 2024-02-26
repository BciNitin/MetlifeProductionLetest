<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintGatePass.aspx.cs" Inherits="PrintGatePass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/gridStyle.css" type="text/css" rel="Stylesheet" />
    <title>Print Gate Pass</title>
    <style type="text/css">
        table, td
        {
            border-color: Black;
            border-style: solid;
            font-family: Arial;
        }
        table
        {
            border-width: 0 0 0 0;
            border-collapse: collapse;
        }
        td
        {
            margin: 0;
            border-width: 1px 1px 1px 1px;
            background-color: Transparent;
        }
        .style9
        {
            font-size: x-large;
            padding: 20px 20px 20px 20px;
            font-family: Arial, Helvetica, sans-serif;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width: 8.27in; height: 1061px">
    <div style="width: 8.27in; height: 10.5in">
        <br />
        <table width="100%" style="border-style: none">
            <tr>
                <td style="border-style: none">
                    <asp:Image ID="imgBarcode" runat="server" Width="170px" Height="40px" ImageUrl="../Images/GatePassBarcode.jpg"
                        Style="margin-left: 0px" />
                </td>
                <td width="100%" style="border-style: none;" align="center">
                    <span class="style9" style="float: none">GATE PASS</span>
                </td>
                <td style="border-style: none; float: right">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/MetLife.png" Height="57px"
                        Width="185px" />&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" style="border-style: none">
                    <br />
                    <table cellspacing="1" width="100%">
                        <tr>
                            <td width="50%" style="padding: 4px 4px 4px 4px">
                                GATE PASS NO :&nbsp;&nbsp;<asp:Label ID="lblGPNO" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                            <td width="50%" style="padding: 4px 4px 4px 4px">
                                GATE PASS DATE :&nbsp;&nbsp;<asp:Label ID="lblGPDate" runat="server" Font-Bold="False"></asp:Label>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td style="padding: 4px 4px 4px 4px">
                                BEARER :&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblBearer" runat="server" Text="lblBearer"
                                    Font-Bold="False"></asp:Label>
                                <br />
                                CARRIER :&nbsp;&nbsp;
                                <asp:Label ID="lblCarrer" runat="server" Text="lblCarrer" Font-Bold="False"></asp:Label>
                            </td>
                            <td valign="top" style="padding: 4px 4px 4px 4px">
                                PURPOSE :&nbsp;&nbsp;<asp:Label ID="lblPurpose" runat="server" Text="lblPurpose"
                                    Font-Bold="False"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                              <td valign="top" style="padding: 4px 4px 4px 4px">
                                FROM LOCATION :&nbsp;&nbsp;<asp:Label ID="lblForLocation" runat="server" Text="lblForLocation"
                                    Font-Bold="False"></asp:Label>
                            </td>
                           <%-- <td style="padding: 4px 4px 4px 4px">
                                <asp:Label ID="lblVendEmp" runat="server" Text="lblVendEmp"></asp:Label>: &nbsp;&nbsp;<asp:Label
                                    ID="lblVendEmpID" runat="server" Text="lblVendEmpID" Font-Bold="False"></asp:Label>
                                ,&nbsp;&nbsp;<asp:Label ID="lblVendEmpName" runat="server" Text="lblVendEmpName"
                                    Font-Bold="False"></asp:Label>
                                <br />
                                <asp:Label ID="lblVendAddress" runat="server" Text="lblVendAddress" Font-Bold="False"></asp:Label>
                            </td>--%>
                            <td valign="top" style="padding: 4px 4px 4px 4px">
                                TO LOCATION :&nbsp;&nbsp;<asp:Label ID="lblToLocation" runat="server" Text="lblToLocation"
                                    Font-Bold="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="border-style: none">
                                <br />
                                <asp:GridView ID="gvGatePass" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    ShowFooter="false" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <HeaderStyle HorizontalAlign="Left" Font-Bold="True" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Asset Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%#Eval("ASSET_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNumber" runat="server" Text='<%#Eval("SERIAL_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Far Tag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetfarTag" runat="server" Text='<%#Eval("ASSET_FAR_TAG") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RFID TAG">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialCode" runat="server" Text='<%#Eval("TAG_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ASSET TAG">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetTag" runat="server" Text='<%#Eval("ASSET_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Make">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetMake" runat="server" Text='<%#Eval("ASSET_MAKE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("MODEL_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Returnable Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("EXP_RETURN_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="border-style: none" align="right">
                                TOTAL ASSETS :&nbsp;&nbsp;<asp:Label ID="lblTotalAssets" runat="server" Text="lblTotalAssets"
                                    Font-Bold="true"></asp:Label>&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table width="100%">
            <tr>
                <td style="border-style: none" align="center">
                    <h4>
                        Received By</h4>
                </td>
                <td style="border-style: none" align="center">
                    <h4>
                        Prepared By</h4>
                </td>
                <td style="border-style: none" align="center">
                    <h4>
                        Authorised By</h4>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
