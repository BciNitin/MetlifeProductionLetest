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
    /// Summary description for RptAssetDashBoard_PRP
    /// </summary>
    public class RptAssetDashBoard_PRP
    {
        #region ASSET DASHBOARD REPORT PROPERTIES
        public string AssetLocation
        { get; set; }
        public string CompanyCode
        { get; set; }
        public string Type
        { get; set; }
        #endregion
    }
}