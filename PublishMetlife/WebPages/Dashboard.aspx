<%@ Page Title="" Language="C#" MasterPageFile="~/WebPages/MobiVUEMaster.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="WebPages_Dashboard" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://d3js.org/d3.v5.min.js" charset="utf-8"></script>
    <script src="../js/c3.js"></script>
    <style>
        /*.c3-axis-x text {
            font-size: 10px;
        }*/
    </style>
    <script type="text/javascript"> 
        <%--<%= Convert.ToString(Session["COMPANY"]) %>--%>
        $(document).ready(function () {
            
            $('#ctl00_ContentPlaceHolder1_tdBarStock').hide();
            var CompCodes = '<%= Convert.ToString(Session["COMPANY"])%>';
            var QueryType = 'Location';
            var obj = {};
            obj.CompCode = '<%= Convert.ToString(Session["COMPANY"])%>';
            obj.QueryType = 'Location';
            $.ajax({
                type: "POST",
                url: "AutoComplete.asmx/GetBarChartData",
                data: '{CompCode: \'' + CompCodes + '\', QueryType: \'' + QueryType + '\'}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                cache: false,
                success: function (response) {
                    
                    var msg = JSON.parse(response.d);
                    successFunc(msg);
                    LocationWithStatus('<%= Convert.ToString(Session["COMPANY"])%>', 'StatusWiseLocation');
                },
                error: function (err) {
                    console.log(err);
                }
            });
            function LocationWithStatus(CompCode, QueryType) {
                $.ajax({
                    type: "POST",
                    url: "AutoComplete.asmx/GetBarChartDataLocationWithStatus",
                    data: '{CompCode: \'' + CompCode + '\', QueryType: \'' + QueryType + '\'}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (response) {
                        
                        var msg = JSON.parse(response.d);
                        GenerateLocationWithStatus(msg);
                        BarGraphAllocated('<%= Convert.ToString(Session["COMPANY"])%>', 'AllocatedWithLocation');
                        
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            }
            function BarGraphStock(CompCode, QueryType,Location) {
                $.ajax({
                    type: "POST",
                    url: "AutoComplete.asmx/GetBarChartforStock",
                    data: '{CompCode: \'' + CompCode + '\', QueryType: \'' + QueryType + '\', Location: \'' + Location + '\'}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (response) {
                        
                        var msg = JSON.parse(response.d);
                        if (msg.length > 0) {
                            $('#ctl00_ContentPlaceHolder1_tdBarStock').show();
                            GenerateStoreWiseStockReport(msg);
                        }
                        else
                            $('#ctl00_ContentPlaceHolder1_tdBarStock').hide();

                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            }
            function BarGraphAllocated(CompCode, QueryType) {
                $.ajax({
                    type: "POST",
                    url: "AutoComplete.asmx/GetBarChartforAllocated",
                    data: '{CompCode: \'' + CompCode + '\', QueryType: \'' + QueryType + '\'}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (response) {
                        
                        var msg = JSON.parse(response.d);
                        if ('<%= Convert.ToString(Session["COMPANY"])%>' == 'IT')
                            GenerateLocationWiseAllocationReport(msg);
                        else
                            GenerateLocationWiseAllocationReportFacilities(msg);
                        BarGraphinTransit('<%= Convert.ToString(Session["COMPANY"])%>', 'InTransitWithLocation');
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            }
            function GenerateLocationWiseAllocationReportFacilities(jsondata) {
                
                var chart = c3.generate({
                    bindto: '#BarchartforAllocated',
                    data: {
                        json: jsondata,
                        keys: {
                            x: 'Location',
                            value: ['Floor'],
                        },
                        type: 'bar',
                        onclick: function (d, element) {
                        },
                    },
                    size: {
                        width: 650,
                    },
                    axis: {
                        x: {
                            type: 'category'
                        }
                    },
                    bar: {
                        width: {
                            ratio: 0.5
                        }
                    },

                    color: {
                        pattern: ['#1568d4', '#aec7e8', '#ff7f0e', '#ffbb78', '#1d6dc2', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
                    },
                });
            }
            function GenerateLocationWiseAllocationReport(jsondata) {
                
                var chart = c3.generate({
                    bindto: '#BarchartforAllocated',
                    data: {
                        json: jsondata,
                        keys: {
                            x: 'Location',
                            value: ['Employee', 'Floor'],
                        },
                        type: 'bar',
                        onclick: function (d, element) {
                        },
                    },
                    size: {
                        width: 650,
                    },
                    axis: {
                        x: {
                            type: 'category'
                        }
                    },
                    bar: {
                        width: {
                            ratio: 0.25
                        }
                    },

                    color: {
                        pattern: ['#1568d4', '#aec7e8', '#ff7f0e', '#ffbb78', '#1d6dc2', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
                    },
                });
            }
            function BarGraphinTransit(CompCode, QueryType) {
                $.ajax({
                    type: "POST",
                    url: "AutoComplete.asmx/GetBarChartforInTransit",
                    data: '{CompCode: \'' + CompCode + '\', QueryType: \'' + QueryType + '\'}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (response) {
                        
                        var msg = JSON.parse(response.d);
                        if (msg.length > 0) {
                            GenerateLocationWiseInTransitReport(msg);
                        }
                        else {
                            $('#ctl00_ContentPlaceHolder1_tblIntransit').hide();
                        }
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            }
            function GenerateLocationWiseInTransitReport(jsondata) {
                
                var chart = c3.generate({
                    bindto: '#BarchartforInTransit',
                    data: {
                        json: jsondata,
                        keys: {
                            x: 'Location',
                            value: ['Count'],
                        },
                        type: 'bar',
                        onclick: function (d, element) {
                        },
                    },
                    size: {
                        width: 650,
                    },
                    padding: {
                        bottom: 10 //adjust chart padding bottom
                    },
                    axis: {
                        x: {
                            type: 'category',
                            tick: {
                                /*rotate:25,*/
                                multiline: true,
                            }
                        }
                    },
                    bar: {
                        width: {
                            ratio: 0.25
                        }
                    },
                    color: {
                        pattern: ['#ffff05', '#aec7e8', '#ff7f0e', '#ffbb78', '#1d6dc2', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
                    },
                });
                $('text').each(function () {
                    $(this).attr("font-size", "7"); //
                    $(this).attr("font-weight", "bold");
                    $(this).attr("font-family", "Arial, Helvetica, sans-serif");
                });
            }
            function fixLabels() {
                d3.selectAll("text[y='9'] tspan:not(.added)").each(function () {
                    // Get the original label including splitter character(s)
                    text_to_split = this.innerHTML;
                    // Split into an array
                    text_array = text_to_split.split('~');
                    // Set first tspan to original label date
                    this.innerHTML = text_array[0];
                    // Set first tspan to bold text
                    d3.select(this).style("font-weight", 600);
                    // Add additional tspans as necessary
                    for (i = 1; i < text_array.length; i++) {
                        d3.select(d3.select(this).node().parentNode)
                            .append("tspan")
                            .attr('x', 0)
                            .attr('dy', '1em')
                            .attr('dx', 0)
                            .attr('class', 'added')
                            .attr('font-style', 'italic')
                            .text(text_array[i]);
                    }
                });
            }
            function GenerateStoreWiseStockReport(jsondata) {
                
                console.log(jsondata[0].Location);
                $("#<%=lblFloorOrStore.ClientID %>").text(jsondata[0].Location.substr(0, 1).toUpperCase() + jsondata[0].Location.substr(1).toLowerCase() + ' - Location Wise Asset Stock Count');
               
                var chart = c3.generate({
                    bindto: '#BarchartforStock',
                    data: {
                        json: jsondata,
                        keys: {
                            x: 'Store',
                            value: ['Count']
                        },
                        type: 'bar',
                        onclick: function (d, element) {
                        },
                    },
                    size: {
                        width: 650,
                    },
                    line: {
                        connectNull: true,
                    },
                    axis: {
                        x: {
                            tick: {

                            },
                            type: 'category',
                            categories: 'Location',   //console.log(jsondata[d.x].Location);
                            height: 50
                        }
                    },
                    bar: {
                        width: {
                            ratio: 0.25
                        }
                    },
                    color: {
                        pattern: ['#45d419', '#aec7e8', '#ff7f0e', '#ffbb78', '#1d6dc2', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
                    },
                });


            }
            function successFunc(jsondata) {
                
                var chart = c3.generate({
                    bindto: '#Barchart',
                    data: {
                        json: jsondata,
                        keys: {
                            x: 'Location',
                            value: ['Count'],
                        },
                        type: 'bar',
                        onclick: function (d, element) {
                        },
                    },
                    size: {
                        width: 650,
                    },
                    axis: {
                        x: {
                            type: 'category'
                        }
                    },
                    bar: {
                        width: {
                            ratio: 0.25
                        }
                    },
                    color: {
                        pattern: ['#1c148f', '#aec7e8', '#ff7f0e', '#ffbb78', '#1d6dc2', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
                    },
                });

                d3.select(".c3-axis-x-label")
                    .style("font-weight", "bold")
                    .style("font-size", "30px");
            }
            function GenerateLocationWithStatus(jsondata) {
                
                var chart = c3.generate({
                    bindto: '#BarchartWithStatus',
                    data: {
                        json: jsondata,
                        keys: {
                            x: 'Location',
                            value: ['Stock', 'Allocated', 'InTransit', 'LostInTransit', 'Scrap'],
                        },
                        type: 'bar',
                        onclick: function (d, element) {
                            //console.log("onclick", d, element);
                            //console.log(d.id);
                            //console.log(jsondata[d.x].Location);
                            //BarGraphCommon(d.id, jsondata[d.x].Location);
                            BarGraphStock('<%= Convert.ToString(Session["COMPANY"])%>', 'StockWiseStoreWithLocation', jsondata[d.x].Location);
                        },
                    },
                    size: {
                        width: 650,
                    },
                    axis: {
                        x: {
                            type: 'category'
                        }
                    },
                    bar: {
                        width: {
                            ratio: 0.5
                        }
                    },
                    color: {
                        pattern: ['#45d419', '#1568d4', '#ffff05', '#ba142b', '#d1940f', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
                    },
                });
            }
        });

    </script>

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
            Dashboard
        </div>
    </div>
    <div id="wrapper1">

        <table id="Table4" runat="server" cellspacing="16" style="width: 100%; border: 2px double #006600;" align="center">
            <tr>
                <td colspan="6" style="text-align: center; border: 2px double #006600;">
                     <asp:Label ID="Label5" runat="server" Text="Total Asset Count" CssClass="label"></asp:Label>
                    <div id="Barchart" style="max-width: 350px; align-content: center; max-height: 350px;">
                    </div>
                </td>
                <td colspan="6" style="text-align: center; border: 2px double #006600;">
                     <asp:Label ID="Label1" runat="server" Text="Asset Count Based On Status" CssClass="label"></asp:Label>
                    <div id="BarchartWithStatus" style="max-width: 350px; align-content: center; max-height: 350px;">
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: center; border: 2px double #006600;">
                    <asp:Label ID="Label2" runat="server" Text="Allocated Asset Count" CssClass="label"></asp:Label>
                    <div id="BarchartforAllocated" style="max-width: 350px; align-content: center; max-height: 350px;">
                    </div>
                </td>
                 <td colspan="6" style="text-align: center; border: 2px double #006600;" id="tdBarStock">                     
                     <asp:Label ID="lblFloorOrStore" runat="server" Text="" CssClass="label"></asp:Label>
                    <div id="BarchartforStock" style="max-width: 350px; align-content: center; max-height: 350px;">
                    </div>
                </td>
            </tr>

        </table>
        <table id="tblIntransit" runat="server" cellspacing="16" style="width: 100%; border: 2px double #006600;" align="center">
            <tr>
                <td id="IntransittdID" style="text-align: center;border: 2px double #006600;">
                    <asp:Label ID="Label3" runat="server" Text="In-Transit Asset Count" CssClass="label"></asp:Label>
                    <div id="BarchartforInTransit" style="max-width: 100%; align-content: center; max-height: 550px;">
                    </div>
                </td>
            </tr>
        </table>

    </div>
</asp:Content>
