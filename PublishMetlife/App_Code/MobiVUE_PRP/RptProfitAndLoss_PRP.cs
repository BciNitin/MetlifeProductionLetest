using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobiVUE_ATS.PRP
{
    /// <summary>
    /// Summary description for Asset Tracking Report Properties.
    /// </summary>
    public class RptProfitAndLoss_PRP
    {
        #region PROFIT AND LOSS REPORT PROPERTIES
        public string AssetCode
        { get; set; }
        public string AssetType
        { get; set; }
        public string CompCode
        { get; set; }
        public string FromDate
        { get; set; }
        public string ToDate
        { get; set; }
        #endregion
    }
}