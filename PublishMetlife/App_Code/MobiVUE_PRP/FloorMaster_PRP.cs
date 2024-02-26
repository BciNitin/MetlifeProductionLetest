using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FloorMaster_PRP
/// </summary>
namespace MobiVUE_ATS.PRP
{

    public class FloorMaster_PRP
    {
        public string FloorCode
        { get; set; }
        public string FloorName
        { get; set; }
        public string SiteCode
        { get; set; }
        public string SiteName
        { get; set; }
        public string Remarks
        { get; set; }
        public bool Active
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        public string CompCode
        { get; set; }
    }
}