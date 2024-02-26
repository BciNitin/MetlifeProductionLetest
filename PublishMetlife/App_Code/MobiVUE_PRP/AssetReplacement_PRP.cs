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
    /// Summary description for Asset Replacement Properties
    /// </summary>
    public class AssetReplacement_PRP
    {
        #region ASSET REPLACEMENT PROPERTIES
        public string ActiveInAssetCode
        { get; set; }

        public string AssetTag
        { get; set; }

        public string Asset_FAR_TAG
        { get; set; }

        public string DocumentNo
        { get; set; }

        public string AssetCode
        { get; set; }
        public string Status
        { get; set; }
        public string Replacement_Date
        { get; set; }
        public string FaultyOutSerialCode
        { get; set; }
        public string FaultyOutAssetFarTagOld
        { get; set; }
        //public string AssetFarTag
        //{ get; set; }
        public string SerialCode
        { get; set; }
        public string AssetMake
        { get; set; }
        public string AssetModel
        { get; set; }
        public string FaultySecurityGENo
        { get; set; }
        public string FaultySecurityGEDate
        { get; set; }
        public string ActiveSecurityGENo
        { get; set; }
        public string ActiveSecurityGEDate
        { get; set; }
        public string BondedType
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ReplaceRemarks
        { get; set; }
        public string CompCode
        { get; set; }

        public string ScrapRemark
        { get; set; }

        public string ScrapDate
        { get; set; }

        public AssetFileUpload_PRP upload { get; set; }

        #endregion
    }
}