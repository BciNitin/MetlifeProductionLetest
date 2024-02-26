using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BarGraph_PRP
/// </summary>
public class BarGraphLocationWithStatus_PRP
{
    public int Count
    { get; set; }
    public string Location
    { get; set; }
    public string Stock
    { get; set; }
    public string Allocated
    { get; set; }
    public string InTransit
    { get; set; }
    public string LostInTransit
    { get; set; }
    public string Scrap
    { get; set; }
}