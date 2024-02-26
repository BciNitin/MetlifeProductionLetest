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
    /// Summary description for GeneralMaster_PRP
    /// </summary>
    public class GeneralMaster_PRP
    {
        #region GENERAL MASTER PROPERTIES
        public int GeneralCode
        { get; set; }
        public string GenaralName
        { get; set; }
        public string StateName
        { get; set; }
        public string CountryName
        { get; set; }
        public string Remarks
        { get; set; }
        public string CompCode
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        public bool Active
        { get; set; }
        #endregion
    }
}