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
    /// Summary description for Asset Summary Report Properties
    /// </summary>
    public class RptAssetSummary_PRP
    {
        #region ASSET SUMMARY REPORT PROPERTIES
        public string CategoryCode
        { get; set; }
        public string AssetLocation
        { get; set; }
        public string CompCode
        { get; set; }
        public string ProcessCode
        { get; set; }
        public string EmpCode
        { get; set; }
        public string PurchaseDateFrom
        { get; set; }
        public string PurchaseDateTo
        { get; set; }
        public string AssetMake
        { get; set; }
        public string ModelName
        { get; set; }
        public string Department
        { get; set; }
        public string AMC_Warranty
        { get; set; }
        public string PortNo
        { get; set; }
        #endregion
    }
}