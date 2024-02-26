using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace MobiVUE_ATS.PRP
{
    /// <summary>
    /// Summary description for SiteMaster_PRP
    /// </summary>
    public class SiteMaster_PRP
    {
        public string SiteCode
        { get; set; }
        public string SiteAddress
        { get; set; }
        public string ContactNo
        { get; set; }
        public string Description
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