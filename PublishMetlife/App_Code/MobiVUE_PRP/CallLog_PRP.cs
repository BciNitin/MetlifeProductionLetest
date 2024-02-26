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
    /// Summary description for Call Log Properties
    /// </summary>
    public class CallLog_PRP
    {
        #region CALL LOG PROPERTIES
        public int RunningSerialNo
        { get; set; }
        public string CallLogCode
        { get; set; }
        public string CallNo
        { get; set; }
        public string CallDate
        { get; set; }
        public string RespondedDate
        { get; set; }
        public string ResolvedStatus
        { get; set; }
        public string ResolvedDate
        { get; set; }
        public string AssetCode
        { get; set; }
        public string SerialCode
        { get; set; }
        public string AssetType
        { get; set; }
        public string CategoryCode
        { get; set; }
        public string CompCode
        { get; set; }
        public string AssetMake
        { get; set; }
        public string PartStatus
        { get; set; }
        public string ModelName
        { get; set; }
        public string VendorCode
        { get; set; }
        public string VendorLocation
        { get; set; }
        public string EngrName
        { get; set; }
        public string VendorContPerson
        { get; set; }
        public string ReplacedSrlNo
        { get; set; }
        public string FRUNO
        { get; set; }
        public string ActionTaken
        { get; set; }
        public string GatePassNo
        { get; set; }
        public string Remarks
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        public string CallDateFrom
        { get; set; }
        public string CallDateTo
        { get; set; }
        public string ResolvedDateFrom
        { get; set; }
        public string ResolvedDateTo
        { get; set; }
        #endregion
    }
}