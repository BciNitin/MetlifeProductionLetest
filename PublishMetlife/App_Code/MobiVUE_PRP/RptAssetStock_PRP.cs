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
    /// Summary description for Asset Stock Report Properties
    /// </summary>
    public class RptAssetStock_PRP
    {
        #region ASSET REPORT PROPERTIES
        public string AssetType
        { get; set; }
        public string CategoryCode
        { get; set; }
        public string AssetLocation
        { get; set; }
        public string CompCode
        { get; set; }
        public bool ReturnableAssets
        { get; set; }
        public string AssetMake
        { get; set; }
        public string ModelName
        { get; set; }
        #endregion
    }
}