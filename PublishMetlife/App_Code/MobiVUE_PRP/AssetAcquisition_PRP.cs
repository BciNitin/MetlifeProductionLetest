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
    /// Summary description for AssetAcquisition_PRP
    /// </summary>
    public class AssetAcquisition_PRP
    {
        #region ASSET ACQUISITION PROPERTIES
        public string AssetID
        { get; set; }
        public string CustomerOrderNo
        { get; set; }
        public string CompCode
        { get; set; }
        public string AssetCode
        { get; set; }
        public string AssetName
        { get; set; }
        public string GRNNo
        { get; set; }
        public string GRNDate
        { get; set; }
        public string AssetType
        { get; set; }
        public int RunningSerialNo
        { get; set; }
        public string SubCategory
        { get; set; }
        public string AssetDomain
        { get; set; }
        public string PurchaseCost
        { get; set; }
        public string AssetSerialCode
        { get; set; }
        public string AssetMakeName
        { get; set; }
        public string AssetModelName
        { get; set; }
        public string AssetCategoryCode
        { get; set; }
        public string AssetLocation
        { get; set; }
        public string AMC_Wrnty_Start_Date
        { get; set; }
        public string AMC_Wrnty_End_Date
        { get; set; }
        public string SecurityGENo
        { get; set; }
        public string SecurityGEDate
        { get; set; }
        public string AssetCompany
        { get; set; }
        public string AssetInstallDate
        { get; set; }
        public string InvoiceNo
        { get; set; }
        public string ExpectedLife
        { get; set; }
        public string PurchaseOrderNo
        { get; set; }
        public string InvoiceDate
        { get; set; }
        public string PODate
        { get; set; }
        public string VendorCode
        { get; set; }
        public string BondedType
        { get; set; }
        public string AssetSaleDate
        { get; set; }
        public string AssetBOE
        { get; set; }
        public string RegisterNo
        { get; set; }
        public string CapitalisedType
        { get; set; }
        public string VerifiableType
        { get; set; }
        public string SecurityClassification
        { get; set; }
        public string AssetProcess
        { get; set; }
        public string ProcessOwner
        { get; set; }
        public string AssetHDD
        { get; set; }
        public string AssetSize
        { get; set; }
        public string AssetVlan
        { get; set; }
        public string AssetProcessor
        { get; set; }
        public string AssetRAM
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ServiceProvider
        { get; set; }
        public string AssetIMEINo
        { get; set; }
        public string AssetPhoneMemory
        { get; set; }
        public string AssetPurchaseValue
        { get; set; }
        public string AssetSaleValue
        { get; set; }
        public string PortNo
        { get; set; }
        public string AssetTag
        { get; set; }
        public string OperationType
        { get; set; }
        public string Remarks
        { get; set; }
        public string ContactName
        { get; set; }
        public string CompanyName
        { get; set; }
        public string CompanyEmail
        { get; set; }
        public string CompanyAddress
        { get; set; }
        public string SoldDate
        { get; set; }
        public string SoldValue
        { get; set; }
        public string ScrapDate
        { get; set; }
        public string RefInvoiceNo
        { get; set; }
        public string AssetRemarks
        { get; set; }
        public string AMC_Warranty
        { get; set; }
        public string WorkStationNo
        { get; set; }
        public string DeptCode
        { get; set; }
        public string CostCenterNo
        { get; set; }
        public bool Asset_Approved
        { get; set; }
        public string Sold_Scrapped
        { get; set; }

        //AMC details properties
        public bool AllAMC
        { get; set; }
        public bool ActiveAMC
        { get; set; }
        public bool ExpiredAMC
        { get; set; }
        public string AMCVendorCode
        { get; set; }

        //Update Asset Serial No
        public string OldSerialNo
        { get; set; }
        public string NewSerialNo
        { get; set; }

        public string AllocationDate
        { get; set; }
        public string AllocatedToId
        { get; set; }
        public string AllocatedTo
        { get; set; }
        public string TicketNo
        { get; set; }
        public string GatePassNo
        { get; set; }

        public string AssetLife
        { get; set; }

       public string AssetEndLife
        { get; set; }

        public string CompanyCost
        { get; set; }

        public string IncomeTaxCost
        { get; set; }

        public string AssetQty
        { get; set; }

        public string Status
        { get; set; }
        public string ParentAssetCode
        {
            get;
            set;
        }

        public string Custodian
        {
            get;
            set;
        }

        public string CapitalisationDate
        {
            get;
            set;
        }
        public bool IsPhysicalVerified
        {
            get;
            set;
        }

        public int ScrappedQty
        {
            get;
            set;
        }
        public int SoldQty
        {
            get;
            set;
        }

        public string  SubStatus
        {
            get;
            set;
        }

        public string store
        {
            get;
            set;
        }

        public string floor
        {
            get;
            set;
        }


       
        public string EmpFloor
        {
            get;
            set;
        }

        public string EmpId
        {
            get;
            set;
        }
        public string EmpName
        {
            get;
            set;
        }

        public string Process
        {
            get;
            set;
        }

        public string  Designation
        {
            get;
            set;
        }


        public string LOB
        {
            get;
            set;
        }

        public string  SubLOB
        {
            get;
            set;
        }


        public string SeatNo
        {
            get;
            set;
        }

        public string ReturnDate
        {
            get;
            set;
        }

        public string HostName
        {
            get;
            set;
        }

       

        public string SERVICE_NOW_TICKET_NO
        {
            get;
            set;
        }

        public string IdentifierLocation
        {
            get;
            set;
        }

        public string Asset_FAR_TAG
        {
            get;
            set;
        }

        public string AssetRFID
        { get; set; }

        public string ProcurementBudget { get; set; }

        public AssetFileUpload_PRP upload { get; set; }

        #endregion
    }
}