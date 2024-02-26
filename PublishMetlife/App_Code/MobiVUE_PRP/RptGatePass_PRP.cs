using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobiVUE_ATS.PRP
{
    /// <summary>
    /// Summary description for RptGatePass_PRP
    /// </summary>
    public class RptGatePass_PRP
    {
        #region GATEPASS REPORT PROPERTIES
        public string GatePassCode
        { get; set; }
        public string AssetLocation
        { get; set; }
        public string CompCode
        { get; set; }
        public bool RtnDateExpired
        { get; set; }
        public bool AllType
        { get; set; }
        public bool Returnable
        { get; set; }
        public bool NonReturnable
        { get; set; }
        public bool LiveGatePass
        { get; set; }
        public string FromDate
        { get; set; }
        public string ToDate
        { get; set; }
        #endregion
    }
}