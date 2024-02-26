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
    /// Summary description for Asset AMC Properties
    /// </summary>
    public class AssetAMC_PRP
    {
        #region ASSET AMC PROPERTIES
        public string AssetLocation
        { get; set; }
        public string AssetMake
        { get; set; }
        public string AssetModelType
        { get; set; }
        public ArrayList Assets
        { get; set; }
        public ArrayList AMC_CODE
        { get; set; }
        public bool AssetsToAdd
        { get; set; }
        public string AMCID
        { get; set; }
        public string AssetID
        { get; set; }
        public string AMCCode
        { get; set; }
        public DateTime PurchaseDate
        { get; set; }
        public DateTime StartDate
        { get; set; }
        public DateTime EndDate
        { get; set; }
        public string CompCode
        { get; set; }
        public string CategoryCode
        { get; set; }
        public string AssetType
        { get; set; }
        public string AMCVendorCode
        { get; set; }
        public string AMCValue
        { get; set; }
        public string RefDocName
        { get; set; }
        public string RespPerson
        { get; set; }
        public bool OperationSuccess
        { get; set; }
        public bool AllAMC
        { get; set; }
        public bool ActiveAMC
        { get; set; }
        public bool ExpiredAMC
        { get; set; }
        public string PONo
        { get; set; }
        public string AssetCode
        { get; set; }
        public string SerialCode
        { get; set; }
        public bool bThisAMC
        { get; set; }
        public string AMCWarranty
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        #endregion
    }
}