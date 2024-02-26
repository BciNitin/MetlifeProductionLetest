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
    /// Summary description for AssetMaster_PRP
    /// </summary>
    public class AssetMaster_PRP
    {
        #region ASSET MASTER PROPERTIES
        public string AssetCode
        { get; set; }
        public string CompCode
        { get; set; }
        public string AssetSerialCode
        { get; set; }
        public string AssetCategoryCode
        { get; set; }
        public string AssetLocationCode
        { get; set; }
        public string AssetCompanyCode
        { get; set; }
        public string AssetVendorCode
        { get; set; }
        public DateTime AssetInstalledDate
        { get; set; }
        public DateTime AssetPurchasedDate
        { get; set; }
        public double AssetPurchasedAmt
        { get; set; }
        public string AssetExpectedLife
        { get; set; }
        public DateTime AssetWarrantyDate
        { get; set; }
        public string AssetPONo
        { get; set; }
        public DateTime AssetPODate
        { get; set; }
        public string AssetInvoiceNo
        { get; set; }
        public DateTime AssetSaleDate
        { get; set; }
        public double AssetSaleAmt
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string AssetName
        { get; set; }
        public string AssetType
        { get; set; }
        public string AssetSubType
        { get; set; }
        public string AssetBrandName
        { get; set; }
        public string AssetMakeName
        { get; set; }
        public string AssetModelName
        { get; set; }
        public string AssetHDD
        { get; set; }
        public string AssetSerialNo
        { get; set; }
        public string FinanceAssetTag
        { get; set; }
        public string AssetAllocatedTo
        { get; set; }
        public string AssetRAM
        { get; set; }
        public DateTime AssetAMCDate
        { get; set; }
        public string AssetProcessor
        { get; set; }
        public string AssetOwner
        { get; set; }
        public string AssetSecurityClass
        { get; set; }
        public string AssetWSNo
        { get; set; }
        public string AssetProcess
        { get; set; }
        public string CartridgeTonerNo
        { get; set; }
        public string PDNo
        { get; set; }
        public string IMEINo
        { get; set; }
        public string Comments
        { get; set; }
        public string ServiceProvider
        { get; set; }
        public string AssetPIN
        { get; set; }
        public string ServerName
        { get; set; }
        public string ServerType
        { get; set; }
        public string ServerCPU
        { get; set; }
        public string ServerSpeed
        { get; set; }
        public string ServerRAM
        { get; set; }
        public string ServerHDD
        { get; set; }
        public string ServerImpRegNo
        { get; set; }
        public DateTime ServerWHDate
        { get; set; }
        public string ServerRemarks
        { get; set; }
        public string DeptCode
        { get; set; }
        #endregion
    }
}