using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobiVUE_ATS.PRP
{
    /// <summary>
    /// Summary description for Asset Tracking Report Properties.
    /// </summary>
    public class RptAssetTracking_PRP
    {
        #region ASSET TRACKING REPORT PROPERTIES
        public string AssetCode
        { get; set; }
        public string SerialCode
        { get; set; }
        public string AssetType
        { get; set; }
        public string AssetLocation
        { get; set; }
        public string CompCode
        { get; set; }
        public string CategoryCode
        { get; set; }
        public string FromDate
        { get; set; }
        public string ToDate
        { get; set; }
        public string AssetMake
        { get; set; }
        public string ModelName
        { get; set; }
        #endregion
    }
}