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
    /// Summary description for Asset Allocation Report Properties
    /// </summary>
    public class RptAssetAllocation_PRP
    {
        #region ASSET ALLOCATION REPORT PROPERTIES
        public string CategoryCode
        { get; set; }
        public string AssetType
        { get; set; }
        public string AssetLocation
        { get; set; }
        public string AssetMake
        { get; set; }
        public string ModelName
        { get; set; }
        public string CompCode
        { get; set; }
        public string ProcessCode
        { get; set; }
        public string EmpCode
        { get; set; }
        public string FromDate
        { get; set; }
        public string ToDate
        { get; set; }
        public string AllocationType
        { get; set; }
        public string DateSearchBy
        { get; set; }
        #endregion
    }
}