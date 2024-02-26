using System;
using System.Data;
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
    /// Summary description for LocationMaster_PRP
    /// </summary>
    public class LocationMaster_PRP
    {
        #region LOCATION MASTER PROPERTIES
        public string LocCode
        { get; set; }
        public string LocName
        { get; set; }
        public string ParentLocCode
        { get; set; }
        public string CompCode
        { get; set; }
        public string LocDesc
        { get; set; }
        public int LocLevel
        { get; set; }
        public bool Active
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        #endregion
    }
}