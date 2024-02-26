using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MobiVUE_ATS.PRP;
using System.Data.SqlClient;

namespace MobiVUE_ATS.DAL
{
    /// <summary>
    /// Summary description for AssetAcquisition_DAL
    /// </summary>
    public class AssetAcquisition_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public AssetAcquisition_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~AssetAcquisition_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Fetch Locations details for mapping with user id
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns>DataTable</returns>
        public DataTable GetLocation(string _CompCode, string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT Site_Code LOC_CODE, Site_Address LOC_NAME FROM SITE_MASTER");
          //  sbQuery.AppendLine(" WHERE PARENT_LOC_CODE LIKE '" + _ParentLocCode + "%' AND LOC_LEVEL=" + _LocLevel + "");
            sbQuery.AppendLine(" WHERE ACTIVE=1 AND SITE_CODE <> 'ALL' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAllocationTo()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT ALLOCATION_TO_ID, ALLOCATION_TO FROM ALLOCATION_TO");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetMailTransactionDetails(string TransactionType, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM MAIL_MASTER WHERE TRANSACTION_TYPE='" + TransactionType + "' AND COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());

        }
        public DataTable PopulateCategory(string _AssetType, string _ParentCategory, int _CatLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT  Distinct Asset_TYPE CATEGORY_CODE, Asset_TYPE CATEGORY_NAME FROM ASSET_ACQUISITION");
            //sbQuery.AppendLine(" WHERE ASSET_TYPE='" + _AssetType + "' AND PARENT_CATEGORY='" + _ParentCategory + "'");
            //sbQuery.AppendLine(" AND CATEGORY_LEVEL=" + _CatLevel + " AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get category cpde/name fr update operation.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateCategoryForUpdate()
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT CATEGORY_CODE,CATEGORY_NAME FROM CATEGORY_MASTER");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch vendor code/name to be populated.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateVendor(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT VENDOR_CODE,VENDOR_NAME FROM VENDOR_MASTER");
            sbQuery.AppendLine(" WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "' ORDER BY VENDOR_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Save new asset acquisition details.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveAssetAcquisitionDetails(string OpType, AssetAcquisition_PRP oPRP)
        {
            bool bResult = false;

            sbQuery = new StringBuilder();

           // string AssetClass = GetAssetClass(oPRP);
            
            sbQuery.AppendLine("INSERT INTO [ASSET_ACQUISITION] ([COMP_Code],[ASSET_LOCATION],[Floor],[ASSET_LIFE],[ASSET_TYPE],[ASSET_MAKE],[MODEL_NAME],[ASSET_CODE],[Host_Name],[Tag_ID],[ASSET_ID],");
            sbQuery.AppendLine(" [Status],[ASSET_SUB_STATUS],[STORE],[SERVICE_NOW_TICKET_NO],[Allocation_Date],[ReturnDate],[EMP_ID],[EMP_NAME],");
            sbQuery.AppendLine(" [EMP_FLOOR],[SEAT_NO],[Designation],[PROCESS_NAME],[SUB_LOB],[LOB], ");
            sbQuery.AppendLine(" [ASSET_PROCESSOR],[ASSET_HDD],[ASSET_RAM],[AMC_WARRANTY],[GRN_NO],[VENDOR_CODE],[PO_NUMBER],[PO_DATE],[INVOICE_NO],[INVOICE_DATE],");
            sbQuery.AppendLine(" [REMARKS],[CREATED_BY],[CREATED_ON] )");
            sbQuery.AppendLine(" VALUES");
            sbQuery.AppendLine(" ('" + oPRP.CompCode + "','" + oPRP.AssetLocation + "','" + oPRP.floor + "','" + oPRP.AssetLife + "','" + oPRP.AssetType + "','" + oPRP.AssetMakeName + "','" + oPRP.AssetModelName + "','" + oPRP.AssetCode + "','" + oPRP.HostName + "','" + oPRP.AssetTag + "','" + oPRP.AssetID + "'");
            sbQuery.AppendLine(" ,'" + oPRP.Status + "','" + oPRP.SubStatus + "','" + oPRP.store + "','" + oPRP.SERVICE_NOW_TICKET_NO + "','" + oPRP.AllocationDate + "','" + oPRP.ReturnDate + "','" + oPRP.EmpId + "','" + oPRP.EmpName + "'");
            sbQuery.AppendLine(" ,'" + oPRP.EmpFloor + "','" + oPRP.SeatNo + "','" + oPRP.Designation + "','" + oPRP.Process + "'," + oPRP.SubLOB + ",'" + oPRP.LOB + "'");
            sbQuery.AppendLine(" ,'" + oPRP.AssetProcessor + "','" + oPRP.AssetHDD + "','" + oPRP.AssetRAM + "','" + oPRP.AMC_Warranty + "','" + oPRP.GRNNo + "','" + oPRP.VendorCode + "','" + oPRP.PurchaseOrderNo + "','" + oPRP.PODate + "','" + oPRP.InvoiceNo + "','" + oPRP.InvoiceDate + "'");
            sbQuery.AppendLine(" ,'" + oPRP.Remarks + "','" + oPRP.CreatedBy + "',getdate()");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResult = true;
            return bResult;
        }


        private string GetAssetClass(AssetAcquisition_PRP oPRP)
        {
            decimal dd = 0;
            string AssetClass = "C";
            if (!string.IsNullOrEmpty(oPRP.AssetPurchaseValue) && decimal.TryParse(oPRP.AssetPurchaseValue, out dd))
            {
                if (dd > 1500000)
                {
                    AssetClass = "A";
                }
                else if (dd <= 1500000 && dd > 500000)
                {
                    AssetClass = "B";
                }
                else
                {
                    AssetClass = "C";
                }
            }
            return AssetClass;
        }
        #region SQL TRANSACTION
        public void BeginTransaction()
        { oDb.BeginTrans(); }

        public void CommitTransaction()
        { oDb.CommitTrans(); }

        public void RollBackTransaction()
        { oDb.RollBack(); }
        #endregion

        /// <summary>
        /// Save asset acquisition details through excel file import.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public int SaveAssetAcquisitionDetails(AssetAcquisition_PRP oPRP)
        {
           // string AssetClass = GetAssetClass(oPRP);

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("INSERT INTO [ASSET_ACQUISITION] ([COMP_Code],[ASSET_LOCATION],[Floor],[ASSET_LIFE],[ASSET_TYPE],[ASSET_MAKE],[MODEL_NAME],[ASSET_CODE],[Host_Name],[Tag_ID],[ASSET_ID],");
            sbQuery.AppendLine(" [Status],[ASSET_SUB_STATUS],[STORE],[SERVICE_NOW_TICKET_NO],[Allocation_Date],[ReturnDate],[EMP_ID],[EMP_NAME],");
            sbQuery.AppendLine(" [EMP_FLOOR],[SEAT_NO],[Designation],[PROCESS_NAME],[SUB_LOB],[LOB], ");
            sbQuery.AppendLine(" [ASSET_PROCESSOR],[ASSET_HDD],[ASSET_RAM],[AMC_WARRANTY],[GRN_NO],[VENDOR_CODE],[PO_NUMBER],[PO_DATE],[INVOICE_NO],[INVOICE_DATE],");
            sbQuery.AppendLine(" [REMARKS],[CREATED_BY],[CREATED_ON] )");
            sbQuery.AppendLine(" VALUES");
            sbQuery.AppendLine(" ('" + oPRP.CompCode + "','" + oPRP.AssetLocation + "','" + oPRP.floor + "','" + oPRP.AssetLife + "','" + oPRP.AssetType + "','" + oPRP.AssetMakeName + "','" + oPRP.AssetModelName + "','" + oPRP.AssetCode + "','" + oPRP.HostName + "','" + oPRP.AssetTag + "','" + oPRP.AssetID + "'");
            sbQuery.AppendLine(" ,'" + oPRP.Status + "','" + oPRP.SubStatus + "','" + oPRP.store + "','" + oPRP.SERVICE_NOW_TICKET_NO + "','" + oPRP.AllocationDate + "','" + oPRP.ReturnDate + "','" + oPRP.EmpId + "','" + oPRP.EmpName + "'");
            sbQuery.AppendLine(" ,'" + oPRP.EmpFloor + "','" + oPRP.SeatNo + "','" + oPRP.Designation + "','" + oPRP.Process + "','" + oPRP.SubLOB + "','" + oPRP.LOB + "'");
            sbQuery.AppendLine(" ,'" + oPRP.AssetProcessor + "','" + oPRP.AssetHDD + "','" + oPRP.AssetRAM + "','" + oPRP.AMC_Warranty + "','" + oPRP.GRNNo + "','" + oPRP.VendorCode + "','" + oPRP.PurchaseOrderNo + "', '"+oPRP.PODate+"','" + oPRP.InvoiceNo + "','"+oPRP.InvoiceDate + "'");
            sbQuery.AppendLine(" ,'" + oPRP.Remarks + "','" + oPRP.CreatedBy + "',getdate())");

            int iRslt = oDb.ExecuteQuery(sbQuery.ToString());
            return iRslt;
        }



        public int SaveAssetAcquisitionFacilityDetails(AssetAcquisition_PRP oPRP)
        {
            // string AssetClass = GetAssetClass(oPRP);
            
            sbQuery = new StringBuilder();
            if (oPRP.AssetInstallDate == null || oPRP.AssetInstallDate == "")
            {
                sbQuery.AppendLine("INSERT INTO [ASSET_ACQUISITION] ([COMP_Code],[ASSET_LOCATION],[Floor],[ASSET_LIFE],[ASSET_TYPE],[CATEGORY_CODE],[ASSET_MAKE],[MODEL_NAME],[ASSET_CODE],[Tag_ID],[ASSET_ID],");
                sbQuery.AppendLine(" [Status],[ASSET_SUB_STATUS],[STORE],[Identifier_Location],");
                //sbQuery.AppendLine(" [EMP_FLOOR],[SEAT_NO],[Designation],[PROCESS_NAME],[SUB_LOB],[LOB], ");
                sbQuery.AppendLine(" [AMC_WARRANTY],[VENDOR_CODE],[PO_NUMBER], ");
                sbQuery.AppendLine("  [ASSET_FAR_TAG],[REMARKS],[CREATED_BY],[CREATED_ON] )");
                sbQuery.AppendLine(" VALUES");
                sbQuery.AppendLine(" ('" + oPRP.CompCode + "','" + oPRP.AssetLocation + "','" + oPRP.floor + "','" + oPRP.AssetLife + "','" + oPRP.AssetType + "','" + oPRP.AssetCategoryCode + "','" + oPRP.AssetMakeName + "','" + oPRP.AssetModelName + "','" + oPRP.AssetCode + "','" + oPRP.AssetTag + "','" + oPRP.AssetID + "'");
                sbQuery.AppendLine(" ,'" + oPRP.Status + "','" + oPRP.SubStatus + "','" + oPRP.store + "','" + oPRP.IdentifierLocation + "'");
                //sbQuery.AppendLine(" ,'" + oPRP.EmpFloor + "','" + oPRP.SeatNo + "','" + oPRP.Designation + "','" + oPRP.Process + "'," + oPRP.SubLOB + ",'" + oPRP.LOB + "'");
                sbQuery.AppendLine(" ,'" + oPRP.AMC_Warranty + "','" + oPRP.VendorCode + "','" + oPRP.PurchaseOrderNo + "','" + oPRP.Asset_FAR_TAG + "'");
                sbQuery.AppendLine(" ,'" + oPRP.Remarks + "','" + oPRP.CreatedBy + "',getdate())");
            }
            else
            {
                sbQuery.AppendLine("INSERT INTO [ASSET_ACQUISITION] ([COMP_Code],[ASSET_LOCATION],[Floor],[ASSET_LIFE],[ASSET_TYPE],[CATEGORY_CODE],[ASSET_MAKE],[MODEL_NAME],[ASSET_CODE],[Tag_ID],[ASSET_ID],");
                sbQuery.AppendLine(" [Status],[ASSET_SUB_STATUS],[STORE],[Identifier_Location],");
                //sbQuery.AppendLine(" [EMP_FLOOR],[SEAT_NO],[Designation],[PROCESS_NAME],[SUB_LOB],[LOB], ");
                sbQuery.AppendLine(" [AMC_WARRANTY],[VENDOR_CODE],[PO_NUMBER],[INSTALLED_DATE], ");
                sbQuery.AppendLine("  [ASSET_FAR_TAG], [REMARKS],[CREATED_BY],[CREATED_ON] )");
                sbQuery.AppendLine(" VALUES");
                sbQuery.AppendLine(" ('" + oPRP.CompCode + "','" + oPRP.AssetLocation + "','" + oPRP.floor + "','" + oPRP.AssetLife + "','" + oPRP.AssetType + "','" + oPRP.AssetCategoryCode + "','" + oPRP.AssetMakeName + "','" + oPRP.AssetModelName + "','" + oPRP.AssetCode + "','" + oPRP.AssetTag + "','" + oPRP.AssetID + "'");
                sbQuery.AppendLine(" ,'" + oPRP.Status + "','" + oPRP.SubStatus + "','" + oPRP.store + "','" + oPRP.IdentifierLocation + "'");
                //sbQuery.AppendLine(" ,'" + oPRP.EmpFloor + "','" + oPRP.SeatNo + "','" + oPRP.Designation + "','" + oPRP.Process + "'," + oPRP.SubLOB + ",'" + oPRP.LOB + "'");
                sbQuery.AppendLine(" ,'" + oPRP.AMC_Warranty + "','" + oPRP.VendorCode + "','" + oPRP.PurchaseOrderNo + "','" + oPRP.AssetInstallDate + "','" + oPRP.Asset_FAR_TAG + "'");
                sbQuery.AppendLine(" ,'" + oPRP.Remarks + "','" + oPRP.CreatedBy + "',getdate())");
            }

            int iRslt = oDb.ExecuteQuery(sbQuery.ToString());
            return iRslt;
        }


        public  void  SaveAuditLogin(string UserID,string Location, string Module_Name, string ActionName,string Desc)
        {
            // string AssetClass = GetAssetClass(oPRP);

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("INSERT INTO [Audit_History] ([UserID],[Location],[Module_Name],[Action],[TransTime],[Description])");
            sbQuery.AppendLine(" VALUES ");
            sbQuery.AppendLine(" ('" + UserID + "','" + Location + "','" + Module_Name + "','" + ActionName + "',getdate(),'" + Desc + "')");
            int iRslt = oDb.ExecuteQuery(sbQuery.ToString());
            
        }
        /// <summary>
        /// Bulk update asset acquisition records.
        /// </summary>
        /// <param name="oPRP"></param>
        public int UpdateAssetAcquisitionDetails(AssetAcquisition_PRP oPRP)
        {
            string AssetClass = GetAssetClass(oPRP);

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("UPDATE [ASSET_ACQUISITION] SET [ASSET_ID] = '" + oPRP.AssetID + "',[GRN_NO] = '" + oPRP.GRNNo + "',[CATEGORY_CODE] = '" + oPRP.AssetCategoryCode + "',[ASSET_LOCATION] = '" + oPRP.AssetLocation + "',[VENDOR_CODE] = '" + oPRP.VendorCode + "',");
            sbQuery.AppendLine("[INSTALLED_DATE] = " + oPRP.AssetPurchaseValue + "+ISNULL(SCRAPPED_AMT,0),[INVOICE_DATE] = '" + oPRP.InvoiceDate + "',[PURCHASED_AMT] = " + oPRP.AssetPurchaseValue + ",[PO_NUMBER] = '" + oPRP.PurchaseOrderNo + "',[PO_DATE] = '" + oPRP.PODate + "',[INVOICE_NO] = '" + oPRP.InvoiceNo + "'");
            sbQuery.AppendLine(",[SALE_DATE] = '" + oPRP.AssetSaleDate + "',[SALE_AMT] = " + oPRP.AssetSaleValue + ",[ASSET_MAKE] = '" + oPRP.AssetMakeName + "',[MODEL_NAME] = '" + oPRP.AssetModelName + "',[ASSET_PROCESS] = '" + oPRP.AssetProcess + "'");
            sbQuery.AppendLine(",[SECURITY_CLASSIFICATION] = '" + oPRP.SecurityClassification + "',[ASSET_SIZE] = '" + oPRP.AssetSize + "',[ASSET_VLAN] = '" + oPRP.AssetVlan + "',[ASSET_HDD] = '" + oPRP.AssetHDD + "',[ASSET_PROCESSOR] = '" + oPRP.AssetProcessor + "'");
            sbQuery.AppendLine(",[ASSET_RAM] = '" + oPRP.AssetRAM + "',[ASSET_IMEI_NO] = '" + oPRP.AssetIMEINo + "',[ASSET_PHONE_MEMORY] = '" + oPRP.AssetPhoneMemory + "',[ASSET_SERVICE_PROVIDER] = '" + oPRP.ServiceProvider + "',[ASSET_NAME] = '" + oPRP.AssetName + "'");
            sbQuery.AppendLine(",[ASSET_TYPE] = '" + oPRP.AssetType + "',[ASSET_BOE] = '" + oPRP.AssetBOE + "',[ASSET_REGISTER_NO] = '" + oPRP.RegisterNo + "',[BONDED_TYPE] = '" + oPRP.BondedType + "',[CAPITALISED_STATUS] = '" + oPRP.CapitalisedType + "'");
            sbQuery.AppendLine(",[VERIFIABLE_STATUS] = '" + oPRP.VerifiableType + "',[PORT_NO] = '" + oPRP.PortNo + "',[SECURITY_GATE_ENTRY_NO] = '" + oPRP.SecurityGENo + "',[SECURITY_GATE_ENTRY_DATE] = '" + oPRP.SecurityGEDate + "',[CREATED_BY] = '" + oPRP.CreatedBy + "',[CREATED_ON] = GETDATE()");
            sbQuery.AppendLine(",[AMC_WARRANTY_START_DATE] = '" + oPRP.AMC_Wrnty_Start_Date + "',[AMC_WARRANTY_END_DATE] = '" + oPRP.AMC_Wrnty_End_Date + "',[AMC_WARRANTY] = '" + oPRP.AMC_Warranty + "',[WORKSTATION_NO] = '" + oPRP.WorkStationNo + "',[COST_CENTER_NO] = '" + oPRP.CostCenterNo + "',[COMPANY_NAME] = '" + oPRP.CompanyName + "',[DEPARTMENT] = '" + oPRP.DeptCode + "',[REMARKS] = '" + oPRP.Remarks + "'");  //,[ASSET_ALLOCATED] = 'False'
            sbQuery.AppendLine(",[ASSET_APPROVED] = '0',  ASSET_LIFE  ='" + oPRP.AssetLife + "', ASSET_QTY = " + oPRP.AssetQty + "+ISNULL(SCRAPPED_QTY,0) , INCOMETAX_COST = '" + oPRP.IncomeTaxCost + "'  , COMPANY_COST ='" + oPRP.CompanyCost + "' , AssetClass = '" + AssetClass + "' ,  ParentAsset ='" + oPRP.ParentAssetCode + "' , Custodian='" + oPRP.Custodian + "' ,  CapitalisationDate ='" + oPRP.CapitalisationDate + "'  , PHYSICAL_VERIFIED='" + oPRP.IsPhysicalVerified + "'    WHERE [ASSET_CODE] = '" + oPRP.AssetCode + "' AND [COMP_CODE] = '" + oPRP.CompCode + "'");
            int iRslt = oDb.ExecuteQuery(sbQuery.ToString());
            return iRslt;
        }

        /// <summary>
        /// Update asset acquisition details.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool UpdateAssetAcquisitionDetails(string OpType, AssetAcquisition_PRP oPRP)
        {
            string AssetClass = GetAssetClass(oPRP);

            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("UPDATE [ASSET_ACQUISITION] SET [SERIAL_CODE] = '" + oPRP.AssetSerialCode + "',[ASSET_ID] = '" + oPRP.AssetID + "',[CATEGORY_CODE] = '" + oPRP.AssetCategoryCode + "'");
            sbQuery.AppendLine(",[ASSET_LOCATION] = '" + oPRP.AssetLocation + "',[AMC_WARRANTY] = '" + oPRP.AMC_Warranty + "',[AMC_WARRANTY_START_DATE] = '" + oPRP.AMC_Wrnty_Start_Date + "',[AMC_WARRANTY_END_DATE] = '" + oPRP.AMC_Wrnty_End_Date + "',[VENDOR_CODE] = '" + oPRP.VendorCode + "',[INSTALLED_DATE] = '" + oPRP.AssetInstallDate + "',[INVOICE_DATE] = '" + oPRP.InvoiceDate + "'");
            sbQuery.AppendLine(",[PURCHASED_AMT] = " + oPRP.AssetPurchaseValue + "+ISNULL(SCRAPPED_AMT,0),[PO_NUMBER] = '" + oPRP.PurchaseOrderNo + "',[PO_DATE] = '" + oPRP.PODate + "',[INVOICE_NO] = '" + oPRP.InvoiceNo + "',[SALE_DATE] = '" + oPRP.AssetSaleDate + "'");
            sbQuery.AppendLine(",[SALE_AMT] = " + oPRP.AssetSaleValue + ",[ASSET_MAKE] = '" + oPRP.AssetMakeName + "',[MODEL_NAME] = '" + oPRP.AssetModelName + "',[ASSET_PROCESS] = '" + oPRP.AssetProcess + "',[SECURITY_CLASSIFICATION] = '" + oPRP.SecurityClassification + "'");
            sbQuery.AppendLine(",[ASSET_SIZE] = '" + oPRP.AssetSize + "',[ASSET_VLAN] = '" + oPRP.AssetVlan + "',[ASSET_HDD] = '" + oPRP.AssetHDD + "',[ASSET_PROCESSOR] = '" + oPRP.AssetProcessor + "',[ASSET_RAM] = '" + oPRP.AssetRAM + "'");
            sbQuery.AppendLine(",[ASSET_IMEI_NO] = '" + oPRP.AssetIMEINo + "',[ASSET_PHONE_MEMORY] = '" + oPRP.AssetPhoneMemory + "',[ASSET_SERVICE_PROVIDER] = '" + oPRP.ServiceProvider + "',[ASSET_TYPE] = '" + oPRP.AssetType + "',[ASSET_NAME] = '" + oPRP.AssetName + "'");
            sbQuery.AppendLine(",[ASSET_BOE] = '" + oPRP.AssetBOE + "',[ASSET_REGISTER_NO] = '" + oPRP.RegisterNo + "',[BONDED_TYPE] = '" + oPRP.BondedType + "',[CAPITALISED_STATUS] = '" + oPRP.CapitalisedType + "',[VERIFIABLE_STATUS] = '" + oPRP.VerifiableType + "'");
            sbQuery.AppendLine(",[SECURITY_GATE_ENTRY_NO] = '" + oPRP.SecurityGENo + "',[SECURITY_GATE_ENTRY_DATE] = '" + oPRP.SecurityGEDate + "',[REMARKS] = '" + oPRP.AssetRemarks.Replace("'", "`") + "',[WORKSTATION_NO] = '" + oPRP.WorkStationNo + "',[COST_CENTER_NO] = '" + oPRP.CostCenterNo + "'");
            sbQuery.AppendLine(",[COMPANY_NAME] = '" + oPRP.CompanyName + "',[DEPARTMENT] = '" + oPRP.DeptCode + "',[CREATED_BY] = '" + oPRP.CreatedBy + "',[CREATED_ON] = GETDATE()");
            sbQuery.AppendLine(",[PORT_NO] = '" + oPRP.PortNo + "',[ASSET_APPROVED] = '0' , ASSET_LIFE  ='" + oPRP.AssetLife + "', ASSET_QTY = " + oPRP.AssetQty + "+ISNULL(SCRAPPED_QTY,0) , INCOMETAX_COST = '" + oPRP.IncomeTaxCost + "'  , COMPANY_COST ='" + oPRP.CompanyCost + "' , AssetClass = '" + AssetClass + "' ,  ParentAsset ='" + oPRP.ParentAssetCode + "' , Custodian='" + oPRP.Custodian + "' ,  CapitalisationDate ='" + oPRP.CapitalisationDate + "'  , PHYSICAL_VERIFIED='" + oPRP.IsPhysicalVerified + "'   WHERE [ASSET_CODE] = '" + oPRP.AssetCode + "' AND [COMP_CODE] = '" + oPRP.CompCode + "'");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResult = true;
            return bResult;
        }

        /// <summary>
        /// Save asset acquisition details through excel file import (save in asset acquisition table).
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public int ImportFreshAssetsAcquisition(AssetAcquisition_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("INSERT INTO [ASSET_ACQUISITION] ([ASSET_CODE],[ASSET_ID],[SERIAL_CODE],[CATEGORY_CODE],[ASSET_LOCATION],[AMC_WARRANTY],[AMC_WARRANTY_START_DATE],[AMC_WARRANTY_END_DATE],[COMP_CODE]");
            sbQuery.AppendLine(",[VENDOR_CODE],[INSTALLED_DATE],[INVOICE_DATE],[PURCHASED_AMT]");
            sbQuery.AppendLine(",[PO_NUMBER],[PO_DATE],[INVOICE_NO],[SALE_DATE],[SALE_AMT],[CREATED_BY],[CREATED_ON],[ASSET_MAKE]");
            sbQuery.AppendLine(",[MODEL_NAME],[ASSET_PROCESS],[SECURITY_CLASSIFICATION],[ASSET_SIZE],[ASSET_VLAN]");
            sbQuery.AppendLine(",[ASSET_HDD],[ASSET_PROCESSOR],[ASSET_RAM],[ASSET_IMEI_NO],[ASSET_PHONE_MEMORY],[ASSET_SERVICE_PROVIDER]");
            sbQuery.AppendLine(",[ASSET_TYPE],[ASSET_BOE],[ASSET_REGISTER_NO],[BONDED_TYPE],[CAPITALISED_STATUS],[VERIFIABLE_STATUS],[PORT_NO],[ASSET_ALLOCATED],[RUNNING_SERIAL_NO],[SECURITY_GATE_ENTRY_NO],[SECURITY_GATE_ENTRY_DATE],[ASSET_APPROVED],[WORKSTATION_NO],[COST_CENTER_NO],[COMPANY_NAME],[GRN_NO],[DEPARTMENT])");
            sbQuery.AppendLine(" VALUES");
            sbQuery.AppendLine(" ('" + oPRP.AssetCode + "','" + oPRP.AssetID + "','" + oPRP.AssetSerialCode + "','" + oPRP.AssetCategoryCode + "','" + oPRP.AssetLocation + "','" + oPRP.AMC_Warranty + "','" + oPRP.AMC_Wrnty_Start_Date + "','" + oPRP.AMC_Wrnty_End_Date + "','" + oPRP.CompCode + "'");
            sbQuery.AppendLine(",'" + oPRP.VendorCode + "','" + oPRP.AssetInstallDate + "','" + oPRP.InvoiceDate + "'," + oPRP.AssetPurchaseValue + "");
            sbQuery.AppendLine(",'" + oPRP.PurchaseOrderNo + "','" + oPRP.PODate + "','" + oPRP.InvoiceNo + "','" + oPRP.AssetSaleDate + "'," + oPRP.AssetSaleValue + ",'" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.AssetMakeName + "'");
            sbQuery.AppendLine(",'" + oPRP.AssetModelName + "','" + oPRP.AssetProcess + "','" + oPRP.SecurityClassification + "','" + oPRP.AssetSize + "','" + oPRP.AssetVlan + "'");
            sbQuery.AppendLine(",'" + oPRP.AssetHDD + "','" + oPRP.AssetProcessor + "','" + oPRP.AssetRAM + "','" + oPRP.AssetIMEINo + "','" + oPRP.AssetPhoneMemory + "','" + oPRP.ServiceProvider + "'");
            sbQuery.AppendLine(",'" + oPRP.AssetType + "','" + oPRP.AssetBOE + "','" + oPRP.RegisterNo + "','" + oPRP.BondedType + "','" + oPRP.CapitalisedType + "','" + oPRP.VerifiableType + "','" + oPRP.PortNo + "','True'," + oPRP.RunningSerialNo.ToString() + ",'" + oPRP.SecurityGENo + "','" + oPRP.SecurityGEDate + "','True','" + oPRP.WorkStationNo + "','" + oPRP.CostCenterNo + "','" + oPRP.CompanyName + "','" + oPRP.GRNNo + "','" + oPRP.DeptCode + "')");
            int iRslt = oDb.ExecuteQueryTran(sbQuery.ToString());
            return iRslt;
        }

        /// <summary>
        /// Bulk allocate fresh assets (save in asset allocation table).
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public int AllocateFreshAssets(AssetAcquisition_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("INSERT INTO [ASSET_ALLOCATION] ([ASSET_CODE],[ASSET_ALLOCATION_DATE],");
            sbQuery.AppendLine(" [ASSET_ALLOCATED_EMP],[ALLOCATED_EMP_ID],[ALLOCATED_DEPARTMENT],[ALLOCATED_PROCESS],");
            sbQuery.AppendLine(" [ASSET_LOCATION],[COMP_CODE],[PORT_NO],[VLAN],[WORKSTATION_NO],[CREATED_BY],[CREATED_ON])");
            sbQuery.AppendLine(" VALUES ");
            sbQuery.AppendLine("('" + oPRP.AssetCode + "','" + oPRP.AllocationDate + "',");
            sbQuery.AppendLine(" '" + oPRP.AllocatedTo + "','" + oPRP.AllocatedToId + "','" + oPRP.DeptCode + "','" + oPRP.AssetProcess + "',");
            sbQuery.AppendLine(" '" + oPRP.AssetLocation + "','" + oPRP.CompCode + "','" + oPRP.PortNo + "','" + oPRP.AssetVlan + "','" + oPRP.WorkStationNo + "','" + oPRP.CreatedBy + "',GETDATE())");
            int iRes = oDb.ExecuteQueryTran(sbQuery.ToString());
            return iRes;
        }

        /// <summary>
        /// Get new running serial no from asset acquisition data table.
        /// </summary>
        /// <returns></returns>
        public int GetMaxAcquisitionId(string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT ISNULL(MAX(RUNNING_SERIAL_NO),0)+1 AS RSN FROM ASSET_ACQUISITION");
            sbQuery.AppendLine(" WHERE CATEGORY_CODE = '" + CategoryCode.Trim() + "' AND COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            return int.Parse(dt.Rows[0]["RSN"].ToString());
        }

        /// <summary>
        /// Get asset details for being viewed/edited/deleted.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetViewAssetDetails(AssetAcquisition_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT  [ASSET_LOCATION] AS Location,[Floor] ,[ASSET_LIFE],[ASSET_TYPE],[ASSET_MAKE],[MODEL_NAME],[ASSET_CODE] AS Serial_NO,[Host_Name],[Tag_ID] RFID_TAG,[ASSET_ID] AS ASSET_TAG,");
            sbQuery.AppendLine(" [Status],[ASSET_SUB_STATUS],[STORE],[SERVICE_NOW_TICKET_NO],[Allocation_Date],[ReturnDate],[EMP_ID],[EMP_NAME], ");
            sbQuery.AppendLine(" [EMP_FLOOR],[SEAT_NO],[Designation],[PROCESS_NAME],[SUB_LOB],[LOB],");
           sbQuery.AppendLine(" [ASSET_PROCESSOR],[ASSET_HDD],[ASSET_RAM],[AMC_WARRANTY],[GRN_NO],[VENDOR_CODE],[PO_NUMBER],[PO_DATE],[INVOICE_NO],[INVOICE_DATE], [REMARKS] FROM ASSET_ACQUISITION ");
          // sbQuery.AppendLine(" ON AA.CATEGORY_CODE = CM.CATEGORY_CODE");
           // sbQuery.AppendLine(" LEFT JOIN LOCATION_MASTER LM ON AA.Store = LM.LOC_CODE");
            sbQuery.AppendLine(" WHERE  (ASSET_CODE LIKE '" + oPRP.AssetCode + "%' OR  TAG_ID  LIKE '" + oPRP.AssetTag + "%')");
            if(oPRP.AssetMakeName!="")
            sbQuery.AppendLine("  AND ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
            if(oPRP.PurchaseOrderNo!="")
                  sbQuery.AppendLine("  AND PO_NUMBER LIKE '" + oPRP.PurchaseOrderNo + "%'");

            if (oPRP.AssetModelName != "")
            {
                if(oPRP.AssetModelName.Contains(','))
                    sbQuery.AppendLine(" AND [MODEL_NAME] IN (" + oPRP.AssetModelName + ")");
            else
                sbQuery.AppendLine(" AND [MODEL_NAME] LIKE '" + oPRP.AssetModelName + "%'");
            }

            if (oPRP.AssetCategoryCode != "")
                sbQuery.AppendLine(" AND Asset_Type LIKE '" + oPRP.AssetCategoryCode + "%'");
            //if (oPRP.AssetLocation != "")
            //    sbQuery.AppendLine("  AND( Store LIKE '" + oPRP.AssetLocation + "%' or Floor LIKE '" + oPRP.AssetLocation + "%') ");
          
            sbQuery.AppendLine(" AND COMP_CODE='" + oPRP.CompCode.Trim() + "'  ORDER BY ASSET_CODE");
          
              
            return oDb.GetDataTable(sbQuery.ToString());
        }



        /// <summary>
        /// Get asset list for being approved after being newly added/updated/swapped/replaced/transferred (internally).
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetAssetDetailsForApproval(AssetAcquisition_PRP oPRP)
        {
            if (oPRP.OperationType == "")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT AA.*,CM.CATEGORY_NAME,LM.LOC_NAME FROM ASSET_ACQUISITION AA INNER JOIN CATEGORY_MASTER CM");
                sbQuery.AppendLine(" ON AA.CATEGORY_CODE = CM.CATEGORY_CODE");
                sbQuery.AppendLine(" INNER JOIN LOCATION_MASTER LM ON AA.ASSET_LOCATION = LM.LOC_CODE");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE = '" + oPRP.AssetType + "' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
                sbQuery.AppendLine(" AND AA.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND AA.SERIAL_CODE LIKE '" + oPRP.AssetSerialCode + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.[MODEL_NAME] IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.[MODEL_NAME] LIKE '" + oPRP.AssetModelName + "%'");
                sbQuery.AppendLine(" AND AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%'");
                sbQuery.AppendLine(" AND AA.COMP_CODE='" + oPRP.CompCode + "' AND AA.ASSET_APPROVED = 'False' AND AA.SOLD_SCRAPPED_STATUS IS NULL ORDER BY AA.ASSET_CODE,AA.PORT_NO");
            }
            else if (oPRP.OperationType == "REPLACED")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT AA.*,CM.CATEGORY_NAME,LM.LOC_NAME");
                sbQuery.AppendLine(" FROM ASSET_ACQUISITION AA");
                sbQuery.AppendLine(" INNER JOIN CATEGORY_MASTER CM ON AA.CATEGORY_CODE = CM.CATEGORY_CODE");
                sbQuery.AppendLine(" INNER JOIN LOCATION_MASTER LM ON AA.ASSET_LOCATION = LM.LOC_CODE");
                sbQuery.AppendLine(" INNER JOIN ASSET_REPLACEMENT AR ON AA.ASSET_CODE = AR.ACTIVE_IN_ASSET_CODE");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE = '" + oPRP.AssetType + "' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%' AND AA.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND AA.SERIAL_CODE LIKE '" + oPRP.AssetSerialCode + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.[MODEL_NAME] IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.[MODEL_NAME] LIKE '" + oPRP.AssetModelName + "%'");
                sbQuery.AppendLine(" AND AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND AA.COMP_CODE='" + oPRP.CompCode + "' AND");
                sbQuery.AppendLine(" AA.ASSET_APPROVED = 'False' AND AA.SOLD_SCRAPPED_STATUS IS NULL ORDER BY AA.ASSET_CODE,AA.PORT_NO");
            }
            else if (oPRP.OperationType == "TRANSFERRED")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT AA.*,CM.CATEGORY_NAME,LM.LOC_NAME");
                sbQuery.AppendLine(" FROM ASSET_ACQUISITION AA");
                sbQuery.AppendLine(" INNER JOIN CATEGORY_MASTER CM ON AA.CATEGORY_CODE = CM.CATEGORY_CODE");
                sbQuery.AppendLine(" INNER JOIN LOCATION_MASTER LM ON AA.ASSET_LOCATION = LM.LOC_CODE");
                sbQuery.AppendLine(" INNER JOIN ASSET_TRANSFER AT ON AA.ASSET_CODE = AT.ASSET_CODE");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE = '" + oPRP.AssetType + "' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%' AND AA.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND AA.SERIAL_CODE LIKE '" + oPRP.AssetSerialCode + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.[MODEL_NAME] IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.[MODEL_NAME] LIKE '" + oPRP.AssetModelName + "%'");
                sbQuery.AppendLine(" AND AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' -- AND AA.COMP_CODE='" + oPRP.CompCode + "'");
                sbQuery.AppendLine(" AND AA.ASSET_APPROVED = 'False' AND AA.SOLD_SCRAPPED_STATUS IS NULL ORDER BY AA.ASSET_CODE,AA.PORT_NO");
            }
            else if (oPRP.OperationType == "SWAPPED")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT AA.*,CM.CATEGORY_NAME,LM.LOC_NAME");
                sbQuery.AppendLine(" FROM ASSET_ACQUISITION AA");
                sbQuery.AppendLine(" INNER JOIN CATEGORY_MASTER CM ON AA.CATEGORY_CODE = CM.CATEGORY_CODE");
                sbQuery.AppendLine(" INNER JOIN LOCATION_MASTER LM ON AA.ASSET_LOCATION = LM.LOC_CODE");
                sbQuery.AppendLine(" INNER JOIN ASSET_SWAP_HISTORY SH1 ON AA.ASSET_CODE = SH1.ASSET_CODE1 AND SH1.COMP_CODE='" + oPRP.CompCode + "' AND AA.ASSET_APPROVED='0'");
                sbQuery.AppendLine(" UNION");
                sbQuery.AppendLine(" SELECT AA.*,CM.CATEGORY_NAME,LM.LOC_NAME");
                sbQuery.AppendLine(" FROM ASSET_ACQUISITION AA");
                sbQuery.AppendLine(" INNER JOIN CATEGORY_MASTER CM ON AA.CATEGORY_CODE = CM.CATEGORY_CODE");
                sbQuery.AppendLine(" INNER JOIN LOCATION_MASTER LM ON AA.ASSET_LOCATION = LM.LOC_CODE");
                sbQuery.AppendLine(" INNER JOIN ASSET_SWAP_HISTORY SH2 ON AA.ASSET_CODE = SH2.ASSET_CODE2 AND SH2.COMP_CODE='" + oPRP.CompCode + "' AND AA.ASSET_APPROVED='0'");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE = '" + oPRP.AssetType + "' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%' AND AA.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND AA.SERIAL_CODE LIKE '" + oPRP.AssetSerialCode + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.[MODEL_NAME] IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.[MODEL_NAME] LIKE '" + oPRP.AssetModelName + "%'");
                sbQuery.AppendLine(" AND AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND AA.COMP_CODE='" + oPRP.CompCode + "'");
                sbQuery.AppendLine(" AND AA.SOLD_SCRAPPED_STATUS IS NULL ORDER BY AA.ASSET_CODE,AA.PORT_NO");
            }
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset details to be populated for being updated.
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetAssetDetailsForUpdate(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT AA.*,CM.CATEGORY_NAME,LM.LOC_NAME FROM ASSET_ACQUISITION AA INNER JOIN CATEGORY_MASTER CM");
            sbQuery.AppendLine(" ON AA.CATEGORY_CODE = CM.CATEGORY_CODE");
            sbQuery.AppendLine(" INNER JOIN LOCATION_MASTER LM");
            sbQuery.AppendLine(" ON AA.ASSET_LOCATION = LM.LOC_CODE");
            sbQuery.AppendLine(" WHERE AA.ASSET_CODE = '" + _AssetCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get process list under a department in a company/location.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateProcess(string DeptCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER WHERE");
            sbQuery.AppendLine(" DEPT_CODE = '" + DeptCode + "' AND COMP_CODE='" + CompCode + "' AND ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get process list in a company/location.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateProcess(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER WHERE");
            sbQuery.AppendLine(" COMP_CODE='" + CompCode + "' AND ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get process-wise owner name.
        /// </summary>
        /// <param name="ProcessCode"></param>
        /// <returns></returns>
        public DataTable GetProcessOwner(string ProcessCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT PROCESS_OWNER FROM PROCESS_MASTER WHERE PROCESS_CODE='" + ProcessCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset details for update sold/scrapped status.
        /// </summary>
        /// <param name="AssetType"></param>
        /// <param name="AssetCode"></param>
        /// <param name="AssetID"></param>
        /// <param name="SerialCode"></param>
        /// <returns></returns>
        public DataTable GetAssetDetailsForSoldScrapped(string StatusType, AssetAcquisition_PRP oPRP)
        {
            if (StatusType == "SCRAPPED")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT '' as REF_INV_NO,(IsNULL(ACQ.ASSET_QTY,0) -ISNULL(ACQ.SCRAPPED_QTY,0)) AS[ASSET_QTY] , ACQ.ASSET_CODE, ACQ.ASSET_ID, ACQ.SERIAL_CODE, ACQ.ASSET_MAKE, ACQ.MODEL_NAME, CM.CATEGORY_NAME, LM.LOC_NAME,");
                sbQuery.AppendLine(" ACQ.ASSET_PROCESS FROM ASSET_ACQUISITION ACQ");
                sbQuery.AppendLine(" INNER JOIN CATEGORY_MASTER CM ON ACQ.CATEGORY_CODE = CM.CATEGORY_CODE");
                sbQuery.AppendLine(" INNER JOIN LOCATION_MASTER LM ON ACQ.ASSET_LOCATION = LM.LOC_CODE");
                sbQuery.AppendLine(" WHERE ACQ.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND ACQ.SERIAL_CODE LIKE '" + oPRP.AssetSerialCode + "%'");
                sbQuery.AppendLine(" AND ACQ.ASSET_TYPE LIKE '" + oPRP.AssetType + "%' AND ACQ.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND ACQ.ASSET_MAKE lIKE '" + oPRP.AssetMakeName + "%' ");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND ACQ.MODEL_NAME IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND ACQ.MODEL_NAME LIKE '" + oPRP.AssetModelName + "%'");
                sbQuery.AppendLine(" AND ACQ.COMP_CODE='" + oPRP.CompCode + "' AND ACQ.ASSET_PROCESS LIKE '" + oPRP.AssetProcess + "%' AND ACQ.SOLD_SCRAPPED_STATUS IS NULL AND ASSET_APPROVED = 'True'");
            }
            else if (StatusType == "SOLD")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT ASD.REFERENCE_INVOICE_NO as REF_INV_NO,ISNULL(ASD.SCRAPPED_QTY,0) AS [ASSET_QTY], ACQ.ASSET_CODE, ACQ.ASSET_ID, ACQ.SERIAL_CODE, ACQ.ASSET_MAKE, ACQ.MODEL_NAME, CM.CATEGORY_NAME, LM.LOC_NAME,");
                sbQuery.AppendLine(" ACQ.ASSET_PROCESS FROM ASSET_ACQUISITION ACQ ");
                sbQuery.AppendLine(" INNER JOIN ASSET_SOLD_DETAILS ASD ON ASD.ASSET_CODE = ACQ.ASSET_CODE ");
                sbQuery.AppendLine(" INNER JOIN CATEGORY_MASTER CM ON ACQ.CATEGORY_CODE = CM.CATEGORY_CODE");
                sbQuery.AppendLine(" INNER JOIN LOCATION_MASTER LM ON ACQ.ASSET_LOCATION = LM.LOC_CODE");
                sbQuery.AppendLine(" WHERE ACQ.ASSET_TYPE LIKE '" + oPRP.AssetType + "%' AND ACQ.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND ACQ.SERIAL_CODE LIKE '" + oPRP.AssetSerialCode + "%'");
                sbQuery.AppendLine(" AND ACQ.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND ACQ.ASSET_MAKE lIKE '" + oPRP.AssetMakeName + "%' ");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND ACQ.MODEL_NAME IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND ACQ.MODEL_NAME LIKE '" + oPRP.AssetModelName + "%'");
                //sbQuery.AppendLine(" AND ACQ.COMP_CODE='" + oPRP.CompCode + "' AND ACQ.ASSET_PROCESS LIKE '" + oPRP.AssetProcess + "%' AND ACQ.SOLD_SCRAPPED_STATUS = 'SCRAPPED' AND ASSET_APPROVED = 'True'");
                sbQuery.AppendLine(" AND ACQ.COMP_CODE='" + oPRP.CompCode + "' AND ACQ.ASSET_PROCESS LIKE '" + oPRP.AssetProcess + "%' AND ASD.STATUS = 'SCRAPPED' AND ASSET_APPROVED = 'True' AND ISNULL(ASD.SOLD_QTY,0)=0");
            }
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Save/update assets details as SCRAPPED/SOLD status.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <param name="OpType"></param>
        /// <returns></returns>
        public bool SaveAssetSoldScrapped(AssetAcquisition_PRP oPRP, string OpType)
        {
            int iRes = 0;
            bool bResult = false;
            if (OpType == "SOLD")
            {
                ////sbQuery = new StringBuilder();
                ////sbQuery.AppendLine("INSERT INTO [ASSET_SOLD_DETAILS] ([REFERENCE_INVOICE_NO],[VENDOR_CODE],[CONTACT_NAME],[COMPANY_EMAIL],[COMPANY_ADDRESS],[SOLD_DATE]");
                ////sbQuery.AppendLine(",[ASSET_CODE],[ASSET_ID],[SERIAL_CODE],[REMARKS],[STATUS],[COMP_CODE])");
                ////sbQuery.AppendLine(" VALUES ");
                ////sbQuery.AppendLine("('" + oPRP.RefInvoiceNo + "','" + oPRP.VendorCode + "','" + oPRP.ContactName + "','" + oPRP.CompanyEmail + "','" + oPRP.CompanyAddress + "','" + oPRP.SoldDate + "'");
                ////sbQuery.AppendLine(",'" + oPRP.AssetCode + "','" + oPRP.AssetID + "','" + oPRP.AssetSerialCode + "','" + oPRP.Remarks + "','SOLD','" + oPRP.CompCode + "')");

                //sbQuery = new StringBuilder();
                //sbQuery.AppendLine("UPDATE [ASSET_SOLD_DETAILS] SET [REFERENCE_INVOICE_NO] = '" + oPRP.RefInvoiceNo + "',[VENDOR_CODE] = '" + oPRP.VendorCode + "',[CONTACT_NAME] = '" + oPRP.ContactName + "'");
                //sbQuery.AppendLine(",[COMPANY_ADDRESS] = '" + oPRP.CompanyAddress + "',[SOLD_DATE] = '" + oPRP.SoldDate + "',[REMARKS] = '" + oPRP.Remarks + "',[SOLD_VALUE] = '" + oPRP.SoldValue + "',[STATUS] = 'SOLD',[COMPANY_EMAIL] = '" + oPRP.CompanyEmail + "'");

                //sbQuery.AppendLine(", Sold_Qty ='" + oPRP.SoldQty + "'");

                //sbQuery.AppendLine(" WHERE [ASSET_CODE] = '" + oPRP.AssetCode + "' AND [COMP_CODE] = '" + oPRP.CompCode + "'");
                //iRes = oDb.ExecuteQuery(sbQuery.ToString());

                //if ((oPRP.ScrappedQty - oPRP.SoldQty) <= 0)
                //{
                //    sbQuery = new StringBuilder();
                //    sbQuery.AppendLine("UPDATE ASSET_ACQUISITION SET SOLD_SCRAPPED_STATUS='SOLD'");
                //    sbQuery.AppendLine(" WHERE ASSET_CODE='" + oPRP.AssetCode + "' AND ASSET_ID='" + oPRP.AssetID + "' AND SERIAL_CODE='" + oPRP.AssetSerialCode + "'");
                //    iRes = oDb.ExecuteQuery(sbQuery.ToString());
                //}
                //if (iRes > 0)
                //    bResult = true;

                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.CommandText = "USP_SoldAsset";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@REFERENCE_INVOICE_NO", oPRP.RefInvoiceNo);
                cmd.Parameters.AddWithValue("@ASSET_CODE", oPRP.AssetCode);
                cmd.Parameters.AddWithValue("@ASSET_ID", oPRP.AssetID);
                cmd.Parameters.AddWithValue("@SERIAL_CODE", oPRP.AssetSerialCode);
                cmd.Parameters.AddWithValue("@REMARKS", oPRP.Remarks);
                cmd.Parameters.AddWithValue("@VENDOR_CODE", oPRP.VendorCode);
                cmd.Parameters.AddWithValue("@CONTACT_NAME", oPRP.ContactName);
                cmd.Parameters.AddWithValue("@COMP_ADDRS", oPRP.CompanyAddress);
                cmd.Parameters.AddWithValue("@COMP_EMAIL", oPRP.CompanyEmail);
                cmd.Parameters.AddWithValue("@SOLD_DATE", oPRP.SoldDate);
                cmd.Parameters.AddWithValue("@STATUS", "SOLD");
                cmd.Parameters.AddWithValue("@ASSET_QTY", oPRP.AssetQty);
                cmd.Parameters.AddWithValue("@SCRAPPED_QTY", oPRP.ScrappedQty);
                cmd.Parameters.AddWithValue("@SOLD_QTY", oPRP.SoldQty);
                cmd.Parameters.AddWithValue("@SOLD_VALUE", oPRP.SoldValue);
                cmd.Parameters.AddWithValue("@COMP_CODE", oPRP.CompCode);

                iRes = oDb.ExecuteCommand(cmd);

                if (iRes > 0)
                    bResult = true;
            }
            if (OpType == "SCRAPPED")
            {
                //sbQuery = new StringBuilder();
                //sbQuery.AppendLine("INSERT INTO [ASSET_SOLD_DETAILS] ([REFERENCE_INVOICE_NO],[ASSET_CODE],[ASSET_ID],");
                //sbQuery.AppendLine("[SERIAL_CODE],[REMARKS],[SCRAP_DATE],[STATUS],[COMP_CODE])");
                //sbQuery.AppendLine(" VALUES ");
                //sbQuery.AppendLine("('" + oPRP.RefInvoiceNo + "','" + oPRP.AssetCode + "','" + oPRP.AssetID + "',");
                //sbQuery.AppendLine("'" + oPRP.AssetSerialCode + "','" + oPRP.Remarks + "','" + oPRP.ScrapDate + "','SCRAPPED','" + oPRP.CompCode + "')");
                //iRes = oDb.ExecuteQuery(sbQuery.ToString());

                //sbQuery = new StringBuilder();
                //sbQuery.AppendLine("UPDATE ASSET_ACQUISITION SET SOLD_SCRAPPED_STATUS='SCRAPPED'");
                //sbQuery.AppendLine(" WHERE ASSET_CODE='" + oPRP.AssetCode + "' AND ASSET_ID='" + oPRP.AssetID + "' AND SERIAL_CODE='" + oPRP.AssetSerialCode + "'");
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.CommandText = "USP_ScrappedAsset";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@REFERENCE_INVOICE_NO", oPRP.RefInvoiceNo);
                cmd.Parameters.AddWithValue("@ASSET_CODE", oPRP.AssetCode);
                cmd.Parameters.AddWithValue("@ASSET_ID", oPRP.AssetID);
                cmd.Parameters.AddWithValue("@SERIAL_CODE", oPRP.AssetSerialCode);
                cmd.Parameters.AddWithValue("@REMARKS", oPRP.Remarks);
                cmd.Parameters.AddWithValue("@SCRAP_DATE", oPRP.ScrapDate);
                cmd.Parameters.AddWithValue("@STATUS", "SCRAPPED");
                cmd.Parameters.AddWithValue("@ASSET_QTY", oPRP.AssetQty);
                cmd.Parameters.AddWithValue("@SCRAPPED_QTY", oPRP.ScrappedQty);
                cmd.Parameters.AddWithValue("@COMP_CODE", oPRP.CompCode);

                iRes = oDb.ExecuteCommand(cmd);

                if (iRes > 0)
                    bResult = true;
            }
            return bResult;
        }

        /// <summary>
        /// Deleting asset sold/scrapped details and updating asset acquisition table.
        /// </summary>
        /// <param name="oPRP"></param>
        public void DeleteSoldScrappedDetails(AssetAcquisition_PRP oPRP)
        {
            using(SqlCommand cmd =new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "USP_RemoveSoldScrape";
                cmd.Parameters.AddWithValue("@AssetCode", oPRP.AssetCode);
                cmd.Parameters.AddWithValue("@SoldScrpeStatus", oPRP.Sold_Scrapped);
                cmd.Parameters.AddWithValue("@CompCode", oPRP.CompCode);
                cmd.Parameters.AddWithValue("@RefInvNo", oPRP.RefInvoiceNo);
                oDb.ExecuteCommand(cmd);
            }

            //sbQuery = new StringBuilder();
            //sbQuery.AppendLine("DELETE FROM [ASSET_SOLD_DETAILS] WHERE [ASSET_CODE]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            //oDb.ExecuteQuery(sbQuery.ToString());

            //sbQuery = new StringBuilder();
            //sbQuery.AppendLine("UPDATE [ASSET_ACQUISITION] SET [SOLD_SCRAPPED_STATUS] = NULL");
            //sbQuery.AppendLine(" WHERE [ASSET_CODE]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            //oDb.ExecuteQuery(sbQuery.ToString());
        }

        /// <summary>
        /// Get PRN file for asset barcode printing.
        /// </summary>
        /// <returns></returns>
        public string GetPRNFile()
        {
            string strPRN = "";
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT PRN_FILE_DATA FROM dbo.AIS_LABEL_PRINTING WHERE PRN_NAME = 'AIS_PRN'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            strPRN = dt.Rows[0]["PRN_FILE_DATA"].ToString();
            return strPRN;
        }

        /// <summary>
        /// Get FAMS ID,Serial No., Model Name etc. as per asset code selected.
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(string _AssetCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT ASSET_ID,SERIAL_CODE,ASSET_ID,PO_NUMBER,MODEL_NAME,ASSET_TYPE FROM ASSET_ACQUISITION");
            sbQuery.AppendLine(" WHERE TAG_ID='" + _AssetCode + "' AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Delete asset details from asset acquisition table (if not allocated).
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string DeleteAssetDetails(string _AssetCode, string _CompCode)
        {
            string DelRslt = "";
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM ASSET_ACQUISITION WHERE ASSET_CODE='" + _AssetCode + "' AND COMP_CODE='" + _CompCode + "' AND ASSET_ALLOCATED='0'");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [ASSET_TRANSFER] WHERE [ASSET_CODE]='" + _AssetCode + "' AND [COMP_CODE]='" + _CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [ASSET_SWAP_HISTORY] WHERE [ASSET_CODE1]='" + _AssetCode + "' AND [COMP_CODE]='" + _CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [ASSET_ALLOCATION] WHERE [ASSET_CODE]='" + _AssetCode + "' AND [COMP_CODE]='" + _CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [ASSET_SOLD_DETAILS] WHERE [ASSET_CODE]='" + _AssetCode + "' AND [COMP_CODE]='" + _CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [ASSET_REPLACEMENT] WHERE [ACTIVE_IN_ASSET_CODE]='" + _AssetCode + "' AND [COMP_CODE]='" + _CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [CALL_LOG_MGMT] WHERE [ASSET_CODE]='" + _AssetCode + "' AND [COMP_CODE]='" + _CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [GATEPASS_ASSETS] WHERE [ASSET_CODE]='" + _AssetCode + "' AND [COMP_CODE]='" + _CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [RECONCILED_ASSET_CODES] WHERE [ASSET_CODE]='" + _AssetCode + "' AND [COMP_CODE]='" + _CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());
            }
            if (iRes > 0)
                DelRslt = "SUCCESS";
            else
                DelRslt = "ALLOCATED";
            return DelRslt;
        }

        /// <summary>
        /// Approve asset details in order to use asset for further process.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool ApproveAssetDetails(AssetAcquisition_PRP oPRP)
        {
            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("UPDATE ASSET_ACQUISITION SET ASSET_APPROVED = 'True'");
            sbQuery.AppendLine(" WHERE ASSET_CODE = '" + oPRP.AssetCode + "' AND SERIAL_CODE = '" + oPRP.AssetSerialCode + "'");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResult = true;
            return bResult;
        }

        /// <summary>
        /// Approve asset details in order to use asset for further process.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool ApproveAssetDetails(AssetAcquisition_PRP oPRP, string ApproveAll)
        {
            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("UPDATE ASSET_ACQUISITION SET ASSET_APPROVED = 'True'");
            sbQuery.AppendLine(" WHERE ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND SERIAL_CODE LIKE '" + oPRP.AssetSerialCode + "%'");
            sbQuery.AppendLine(" AND ASSET_TYPE = '" + oPRP.AssetType + "' AND ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
            if (oPRP.AssetModelName != "")
                sbQuery.AppendLine(" AND [MODEL_NAME] IN (" + oPRP.AssetModelName + ")");
            else
                sbQuery.AppendLine(" AND [MODEL_NAME] LIKE '" + oPRP.AssetModelName + "%'");
            sbQuery.AppendLine(" AND CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%'");
            sbQuery.AppendLine(" AND COMP_CODE='" + oPRP.CompCode + "'");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResult = true;
            return bResult;
        }

        /// <summary>
        /// Get asset count (unapproved/approved) based on search criteria.
        /// </summary>
        /// <returns></returns>
        public string GetAssetsCount(AssetAcquisition_PRP oPRP)
        {
            string AssetCount = "";
            if (oPRP.OperationType == "")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT COUNT(*) UNAPPROVED FROM ASSET_ACQUISITION WHERE ASSET_TYPE='" + oPRP.AssetType + "'");
                sbQuery.AppendLine(" AND COMP_CODE='" + oPRP.CompCode + "' AND CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND ASSET_APPROVED='False'");
                sbQuery.AppendLine(" AND ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND MODEL_NAME IN (" + oPRP.AssetModelName + ");");
                else
                    sbQuery.AppendLine(" AND MODEL_NAME LIKE '" + oPRP.AssetModelName + "%';");

                sbQuery.AppendLine(" SELECT COUNT(*) APPROVED FROM ASSET_ACQUISITION WHERE ASSET_TYPE='" + oPRP.AssetType + "'");
                sbQuery.AppendLine(" AND COMP_CODE='" + oPRP.CompCode + "' AND CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND ASSET_APPROVED='True'");
                sbQuery.AppendLine(" AND ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND MODEL_NAME IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND MODEL_NAME LIKE '" + oPRP.AssetModelName + "%'");
            }
            else if (oPRP.OperationType == "REPLACED")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT COUNT(*) UNAPPROVED FROM ASSET_ACQUISITION AA INNER JOIN ASSET_REPLACEMENT AR");
                sbQuery.AppendLine(" ON AA.ASSET_CODE = AR.ACTIVE_IN_ASSET_CODE");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE='" + oPRP.AssetType + "' AND AA.COMP_CODE='" + oPRP.CompCode + "' AND");
                sbQuery.AppendLine(" AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND AA.ASSET_APPROVED='False' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.MODEL_NAME IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.MODEL_NAME LIKE '" + oPRP.AssetModelName + "%';");
                sbQuery.AppendLine(" SELECT COUNT(*) APPROVED FROM ASSET_ACQUISITION AA INNER JOIN ASSET_REPLACEMENT AR");
                sbQuery.AppendLine(" ON AA.ASSET_CODE = AR.ACTIVE_IN_ASSET_CODE");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE='" + oPRP.AssetType + "' AND AA.COMP_CODE='" + oPRP.CompCode + "'");
                sbQuery.AppendLine(" AND AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%'  AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND AA.ASSET_APPROVED='True' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.MODEL_NAME IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.MODEL_NAME LIKE '" + oPRP.AssetModelName + "%'");
            }
            else if (oPRP.OperationType == "TRANSFERRED")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT COUNT(*) UNAPPROVED FROM ASSET_ACQUISITION AA INNER JOIN ASSET_TRANSFER AT");
                sbQuery.AppendLine(" ON AA.ASSET_CODE = AT.ASSET_CODE");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE='" + oPRP.AssetType + "' AND AA.COMP_CODE='" + oPRP.CompCode + "' AND");
                sbQuery.AppendLine(" AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND AA.ASSET_APPROVED='False' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.MODEL_NAME IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.MODEL_NAME LIKE '" + oPRP.AssetModelName + "%';");
                sbQuery.AppendLine(" SELECT COUNT(*) APPROVED FROM ASSET_ACQUISITION AA INNER JOIN ASSET_TRANSFER AT");
                sbQuery.AppendLine(" ON AA.ASSET_CODE = AT.ASSET_CODE");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE='" + oPRP.AssetType + "' AND AA.COMP_CODE='" + oPRP.CompCode + "'");
                sbQuery.AppendLine(" AND AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%'  AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND AA.ASSET_APPROVED='True' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.MODEL_NAME IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.MODEL_NAME LIKE '" + oPRP.AssetModelName + "%'");
            }
            else if (oPRP.OperationType == "SWAPPED")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT COUNT(*) UNAPPROVED FROM");
                sbQuery.AppendLine("(SELECT AA.* FROM ASSET_ACQUISITION AA");
                sbQuery.AppendLine(" INNER JOIN ASSET_SWAP_HISTORY SH1 ON AA.ASSET_CODE = SH1.ASSET_CODE1 AND SH1.COMP_CODE='" + oPRP.CompCode + "'");
                sbQuery.AppendLine(" AND AA.ASSET_APPROVED='0' UNION");
                sbQuery.AppendLine(" SELECT AA.* FROM ASSET_ACQUISITION AA");
                sbQuery.AppendLine(" INNER JOIN ASSET_SWAP_HISTORY SH2 ON AA.ASSET_CODE = SH2.ASSET_CODE2 AND SH2.COMP_CODE='" + oPRP.CompCode + "' AND AA.ASSET_APPROVED='0'");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE = '" + oPRP.AssetType + "' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.MODEL_NAME IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.MODEL_NAME LIKE '" + oPRP.AssetModelName + "%'");
                sbQuery.AppendLine(" AND AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND AA.COMP_CODE='" + oPRP.CompCode + "'");
                sbQuery.AppendLine(" AND AA.SOLD_SCRAPPED_STATUS IS NULL");
                sbQuery.AppendLine(" ) A;");
                sbQuery.AppendLine(" SELECT COUNT(*) APPROVED FROM");
                sbQuery.AppendLine(" (SELECT AA.* FROM ASSET_ACQUISITION AA");
                sbQuery.AppendLine(" INNER JOIN ASSET_SWAP_HISTORY SH1 ON AA.ASSET_CODE = SH1.ASSET_CODE1 AND SH1.COMP_CODE = '" + oPRP.CompCode + "'");
                sbQuery.AppendLine(" AND AA.ASSET_APPROVED='1' UNION");
                sbQuery.AppendLine(" SELECT AA.* FROM ASSET_ACQUISITION AA");
                sbQuery.AppendLine(" INNER JOIN ASSET_SWAP_HISTORY SH2 ON AA.ASSET_CODE = SH2.ASSET_CODE2 AND SH2.COMP_CODE = '" + oPRP.CompCode + "' AND AA.ASSET_APPROVED='1'");
                sbQuery.AppendLine(" WHERE AA.ASSET_TYPE = '" + oPRP.AssetType + "' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
                if (oPRP.AssetModelName != "")
                    sbQuery.AppendLine(" AND AA.MODEL_NAME IN (" + oPRP.AssetModelName + ")");
                else
                    sbQuery.AppendLine(" AND AA.MODEL_NAME LIKE '" + oPRP.AssetModelName + "%'");
                sbQuery.AppendLine(" AND AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND AA.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND AA.COMP_CODE='" + oPRP.CompCode + "'");
                sbQuery.AppendLine(" AND AA.SOLD_SCRAPPED_STATUS IS NULL");
                sbQuery.AppendLine(" ) B");
            }
            DataSet ds = oDb.GetDataSet(sbQuery.ToString());
            AssetCount = ds.Tables[0].Rows[0]["UNAPPROVED"].ToString() + "^" + ds.Tables[1].Rows[0]["APPROVED"].ToString();
            return AssetCount;
        }

        /// <summary>
        /// Check if serial no. of the asset already exists in asset acquisition.
        /// </summary>
        /// <param name="SerialCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public bool CheckDuplicateSerialNo(string SerialCode, string CompCode)
        {
            bool bExists = false;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS SC FROM ASSET_ACQUISITION WHERE ASSET_CODE = '" + SerialCode + "' AND COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows[0]["SC"].ToString() != "0")
                bExists = true;
            return bExists;
        }

        /// <summary>
        /// Check if IMEI no. of the asset already exists in asset acquisition.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool CheckDuplicateIMEINo(string IMEINO, string CompCode)
        {
            bool bExists = false;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS IMEI FROM ASSET_ACQUISITION WHERE ASSET_IMEI_NO = '" + IMEINO + "' AND COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows[0]["IMEI"].ToString() != "0")
                bExists = true;
            return bExists;
        }

        /// <summary>
        /// Get assets list for being exported into excel sheet.
        /// </summary>
        /// <param name="AssetType"></param>
        /// <returns></returns>
        public DataTable GetAssetsForExport(string AssetType, string AssetCategoryCode, string AssetMakeName, string AssetModelName, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT [ASSET_CODE],[ASSET_ID] AS [FAMS_ID],[SERIAL_CODE],[CATEGORY_CODE],[ASSET_LOCATION],[AMC_WARRANTY],CONVERT(VARCHAR,NULLIF([AMC_WARRANTY_START_DATE],''),101) AS [AMC_WARRANTY_START_DATE],");
            sbQuery.AppendLine("CONVERT(VARCHAR,NULLIF([AMC_WARRANTY_END_DATE],''),101) AS [AMC_WARRANTY_END_DATE],[VENDOR_CODE],CONVERT(VARCHAR,NULLIF([INSTALLED_DATE],''),101) AS [INSTALLED_DATE],CONVERT(VARCHAR,NULLIF([PURCHASED_DATE],''),101) AS [PURCHASED_DATE],[PURCHASED_AMT] AS [PURCHASED_AMT],[PO_NUMBER],");
            sbQuery.AppendLine("CONVERT(VARCHAR,NULLIF([PO_DATE],''),101) AS [PO_DATE],[INVOICE_NO],CONVERT(VARCHAR,NULLIF([SALE_DATE],''),101) AS [SALE_DATE],[SALE_AMT],[ASSET_MAKE],[MODEL_NAME],[ASSET_PROCESS],[SECURITY_CLASSIFICATION],");
            sbQuery.AppendLine("[ASSET_SIZE],[ASSET_VLAN],[ASSET_HDD],[ASSET_PROCESSOR],[ASSET_RAM],[ASSET_IMEI_NO],[ASSET_PHONE_MEMORY],");
            sbQuery.AppendLine("[ASSET_SERVICE_PROVIDER],[ASSET_TYPE],[ASSET_BOE],[ASSET_REGISTER_NO],[BONDED_TYPE],[CAPITALISED_STATUS],");
            sbQuery.AppendLine("[VERIFIABLE_STATUS],[PORT_NO],[WORKSTATION_NO],[COST_CENTER_NO],[SECURITY_GATE_ENTRY_NO],");
            sbQuery.AppendLine("CONVERT(VARCHAR,NULLIF([SECURITY_GATE_ENTRY_DATE],''),101) AS [SECURITY_GATE_ENTRY_DATE],[COMP_CODE],[COMPANY_NAME],[CUSTOMER_ORDER_NO],[DEPARTMENT]");
            sbQuery.AppendLine(@" , CONVERT(VARCHAR , NULLIF(ASSET_LIFE,''),101)
,COMPANY_COST
,INCOMETAX_COST
,ASSET_QTY	
,AssetClass	
,ParentAsset	
,CUSTODIAN	
,PHYSICAL_VERIFIED	
,CapitalisationDate	
,SCRAPPED_QTY	
,SCRAPPED_AMT ");
            sbQuery.AppendLine(" FROM [ASSET_ACQUISITION] WHERE [ASSET_TYPE]='" + AssetType + "' AND [CATEGORY_CODE] LIKE '" + AssetCategoryCode + "%'");
            sbQuery.AppendLine(" AND [ASSET_MAKE] LIKE '" + AssetMakeName + "%'");
            if (AssetModelName != "")
                sbQuery.AppendLine(" AND [MODEL_NAME] IN (" + AssetModelName + ")");
            else
                sbQuery.AppendLine(" AND [MODEL_NAME] LIKE '" + AssetModelName + "%'");
            sbQuery.AppendLine(" AND [COMP_CODE]='" + CompCode + "' AND [ASSET_APPROVED]='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get company/location based on login location.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateCompany(string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COMP_CODE,COMP_NAME FROM COMPANY_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + _CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get department list based on login location.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateDepartment(string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT DEPT_CODE,DEPT_NAME FROM DEPARTMENT_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + _CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset SCRAPPED/SOLD details for being viewed.
        /// </summary>
        /// <returns></returns>
        public DataTable GetSoldScrappedDetails(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT [REFERENCE_INVOICE_NO],[VENDOR_CODE],[CONTACT_NAME],SCRAPPED_QTY AS[Asset_Qty],[COMPANY_ADDRESS],CONVERT(VARCHAR,NULLIF([SOLD_DATE],''),105) AS [SOLD_DATE]");
            sbQuery.AppendLine(",[ASSET_CODE],[ASSET_ID],[SERIAL_CODE],[SOLD_VALUE],CONVERT(VARCHAR,NULLIF([SCRAP_DATE],''),105) AS [SCRAP_DATE],[STATUS]");
            sbQuery.AppendLine(" FROM [ASSET_SOLD_DETAILS] WHERE [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Update asset acquisition (AMC/Warranty) details when AMC details are updated.
        /// </summary>
        /// <param name="AMC_WARRANTY"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="AssetCode"></param>
        /// <param name="SerialCode"></param>
        /// <param name="CompCode"></param>
        public void UpdateAssetAMCWarrantyDetails(string AMC_WARRANTY, DateTime StartDate, DateTime EndDate, string AssetCode, string SerialCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("UPDATE [ASSET_ACQUISITION] SET [AMC_WARRANTY]='" + AMC_WARRANTY + "', [AMC_WARRANTY_START_DATE]='" + StartDate + "', [AMC_WARRANTY_END_DATE]='" + EndDate + "'");
            sbQuery.AppendLine(" WHERE [ASSET_CODE]='" + AssetCode + "' AND [SERIAL_CODE]='" + SerialCode + "' AND [COMP_CODE]='" + CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());
        }

        /// <summary>
        /// Check whether the asset is being transferred to another location.
        /// </summary>
        /// <param name="AssetCode"></param>
        /// <returns></returns>
        public bool ChkAssetTransferred(string AssetCode, string CompCode)
        {
            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS TRANS FROM [ASSET_TRANSFER] AT WHERE AT.[ASSET_CODE]='" + AssetCode + "'");
            sbQuery.AppendLine(" AND AT.[COMP_CODE]='" + CompCode + "' AND AT.[TO_LOCATION] IN ");
            sbQuery.AppendLine("(SELECT [LOC_CODE] FROM [LOCATION_MASTER] WHERE [COMP_CODE] <> '" + CompCode + "')");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["TRANS"].ToString() != "0")
                    bResult = true;
            }
            return bResult;
        }

        /// <summary>
        /// Check location code validity when asset is being saved/updated.
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public bool ValidLocationCode(string LocationCode, string CompCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS LOC FROM [LOCATION_MASTER] WHERE [LOC_CODE]='" + LocationCode + "'");
            sbQuery.AppendLine(" AND [COMP_CODE]='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LOC"].ToString() == "0")
                    bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Check process code validity when asset is being saved/updated.
        /// </summary>
        /// <param name="ProcessCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public bool ValidProcessCode(string ProcessCode, string CompCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS PRC FROM [PROCESS_MASTER] WHERE [PROCESS_CODE]='" + ProcessCode + "'");
            sbQuery.AppendLine(" AND [COMP_CODE]='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["PRC"].ToString() == "0")
                    bResult = false;
            }
            return bResult;
        }



        public bool ValidParentAssetCode(string ParentAssetCode, string CompCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();

            sbQuery.AppendLine("SELECT COUNT(*) AS PRC FROM ASSET_ACQUISITION  WHERE [ASSET_CODE]='" + ParentAssetCode + "'");
            sbQuery.AppendLine(" AND [COMP_CODE]='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["PRC"].ToString() == "0")
                    bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Check department code validity when asset is being saved/updated.
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public bool ValidDepartmentCode(string DeptCode, string CompCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS DPT FROM [DEPARTMENT_MASTER] WHERE [DEPT_CODE]='" + DeptCode + "'");
            sbQuery.AppendLine(" AND [COMP_CODE]='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["DPT"].ToString() == "0")
                    bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Get a valid employee name based on employee code provided.
        /// </summary>
        /// <returns></returns>
        public string ValidEmployeeCode(string EmpCode, string CompCode)
        {
            string EmpName = "";
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT [EMPLOYEE_NAME] FROM [EMPLOYEE_MASTER] WHERE [EMPLOYEE_CODE]='" + EmpCode + "'");
            sbQuery.AppendLine(" AND [COMP_CODE]='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                EmpName = dt.Rows[0]["EMPLOYEE_NAME"].ToString().Trim();
            else
                EmpName = "NOT_FOUND";
            return EmpName;
        }

        /// <summary>
        /// Get asset make based on category selected.
        /// </summary>
        /// <param name="CategoryCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateAssetMake(string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT DISTINCT [ASSET_MAKE] FROM [ASSET_ACQUISITION] WHERE [ASSET_TYPE] = '" + CategoryCode + "' AND [COMP_CODE] = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset model name based on category nad make selected.
        /// </summary>
        /// <param name="AssetMake"></param>
        /// <param name="CategoryCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateModelName(string AssetMake, string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT DISTINCT [MODEL_NAME] FROM [ASSET_ACQUISITION] WHERE [ASSET_MAKE]='" + AssetMake + "'");
            sbQuery.AppendLine(" AND [ASSET_TYPE]='" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Check whether serial code/no. exists in case of asset details updation.
        /// </summary>
        /// <param name="SerialCode"></param>
        /// <returns></returns>
        public bool ValidSerialNo(string SerialCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS SNO FROM [ASSET_ACQUISITION] WHERE [SERIAL_CODE]='" + SerialCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SNO"].ToString() == "0")
                    bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Check whether the category is valid while uploading asset through excel sheet.
        /// </summary>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        public bool ValidCategoryCode(string CategoryCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS CAT FROM [CATEGORY_MASTER] WHERE [CATEGORY_CODE]='" + CategoryCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["CAT"].ToString() == "0")
                    bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Category code cannot be changed when asset details are updated.
        /// </summary>
        /// <param name="AssetCode"></param>
        /// <param name="SerialCode"></param>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        public bool CategoryCodeChanged(string AssetCode, string CategoryCode, string CompCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT ASSET_TYPE CATEGORY_CODE FROM ASSET_ACQUISITION WHERE ASSET_CODE='" + AssetCode + "' AND COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["CATEGORY_CODE"].ToString().Trim() == CategoryCode.Trim())
                    bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Check whether the vendor exists on the location.
        /// </summary>
        /// <param name="VendorCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public bool ValidVendorCode(string VendorCode, string CompCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS VENDOR FROM [VENDOR_MASTER] WHERE [VENDOR_CODE]='" + VendorCode + "'");
            sbQuery.AppendLine(" AND [COMP_CODE]='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["VENDOR"].ToString() == "0")
                    bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Bulk deletion of assets from each database table as per criteria provided.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public int DeleteAssetsInBulk(AssetAcquisition_PRP oPRP)
        {
            int iResult = 0;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [ASSET_ACQUISITION] ");
            sbQuery.AppendLine(" WHERE ASSET_TYPE LIKE '" + oPRP.AssetType + "%' AND ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND SERIAL_CODE LIKE '" + oPRP.AssetSerialCode + "%' AND ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
            if (oPRP.AssetModelName != "")
                sbQuery.AppendLine(" AND [MODEL_NAME] IN (" + oPRP.AssetModelName + ")");
            else
                sbQuery.AppendLine(" AND [MODEL_NAME] LIKE '" + oPRP.AssetModelName + "%'");
            sbQuery.AppendLine(" AND CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%'");
            sbQuery.AppendLine(" AND AMC_WARRANTY LIKE '" + oPRP.AMC_Warranty + "%' AND VERIFIABLE_STATUS LIKE '" + oPRP.VerifiableType + "%'");
            sbQuery.AppendLine(" AND ASSET_APPROVED = '" + oPRP.Asset_Approved + "' AND COMP_CODE='" + oPRP.CompCode + "'");
            if (oPRP.Sold_Scrapped == "")
                sbQuery.AppendLine(" AND SOLD_SCRAPPED_STATUS IS NULL");
            else
                sbQuery.AppendLine(" AND SOLD_SCRAPPED_STATUS LIKE '" + oPRP.Sold_Scrapped + "%'");
            iResult = oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [ASSET_TRANSFER] WHERE [ASSET_CODE]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [ASSET_SWAP_HISTORY] WHERE [ASSET_CODE1]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [ASSET_ALLOCATION] WHERE [ASSET_CODE]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [ASSET_SOLD_DETAILS] WHERE [ASSET_CODE]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [ASSET_REPLACEMENT] WHERE [ACTIVE_IN_ASSET_CODE]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [CALL_LOG_MGMT] WHERE [ASSET_CODE]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [GATEPASS_ASSETS] WHERE [ASSET_CODE]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [RECONCILED_ASSET_CODES] WHERE [ASSET_CODE]='" + oPRP.AssetCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.AppendLine("DELETE FROM [RECONCILED_SERIAL_CODES] WHERE [SERIAL_CODES]='" + oPRP.AssetSerialCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            return iResult;
        }

        /// <summary>
        /// Get vendor master details
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendor(string CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("SELECT VENDOR_CODE,VENDOR_NAME FROM VENDOR_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "' ORDER BY VENDOR_NAME");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Get Vendor details based on vendor name selected in sold/scrapped assets.
        /// </summary>
        /// <param name="VendorCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetVendorDetails(string VendorCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT [VENDOR_CONT_PERSON],[VENDOR_ADDRESS],[VENDOR_EMAIL] FROM [VENDOR_MASTER]");
            sbQuery.AppendLine(" WHERE [VENDOR_CODE]='" + VendorCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetches AMC details based on filter criteria (all/active/expired).
        /// </summary>
        /// <param name="_AMCType"></param>
        /// <returns></returns>
        public DataTable GetAMCDetails(AssetAcquisition_PRP oPRP, bool _VendorWiseAMC)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT CONVERT(VARCHAR,NULLIF(AACQ.AMC_WARRANTY_START_DATE,''),105) AS AMC_WARRANTY_START_DATE,");
            sbQuery.AppendLine(" CONVERT(VARCHAR,NULLIF(AACQ.AMC_WARRANTY_END_DATE,''),105) AS AMC_WARRANTY_END_DATE,AACQ.ASSET_CODE,");
            sbQuery.AppendLine(" AACQ.SERIAL_CODE,AACQ.PO_NUMBER,LM.LOC_NAME,AACQ.ASSET_MAKE,AACQ.MODEL_NAME,ISNULL(VM.VENDOR_NAME,'') AS VENDOR_NAME");
            sbQuery.AppendLine(" FROM ASSET_ACQUISITION AACQ LEFT OUTER JOIN VENDOR_MASTER VM");
            sbQuery.AppendLine(" ON AACQ.VENDOR_CODE = VM.VENDOR_CODE");
            sbQuery.AppendLine(" INNER JOIN LOCATION_MASTER LM ON AACQ.ASSET_LOCATION = LM.LOC_CODE");
            sbQuery.AppendLine(" WHERE AACQ.AMC_WARRANTY = 'AMC' AND AACQ.COMP_CODE='" + oPRP.CompCode + "' AND AACQ.COMP_CODE = LM.COMP_CODE");
            if (_VendorWiseAMC)
            {
                sbQuery.AppendLine(" AND AACQ.VENDOR_CODE='" + oPRP.AMCVendorCode + "'");
            }
            if (oPRP.ExpiredAMC == true)
            {
                sbQuery.AppendLine(" AND AACQ.AMC_WARRANTY_END_DATE < GETDATE()");
            }
            else if (oPRP.ActiveAMC == true)
            {
                sbQuery.AppendLine(" AND AACQ.AMC_WARRANTY_END_DATE >= GETDATE()");
            }
            sbQuery.AppendLine(" ORDER BY ASSET_CODE");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get Process code/name details to be populated in dropdownlist.
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetProcess(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER");
            sbQuery.AppendLine(" WHERE COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Update asset serial no. based on old serial code and asset code.
        /// </summary>
        /// <param name="oPRP"></param>
        public void UpdateAssetSerialNo(AssetAcquisition_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("UPDATE [ASSET_ACQUISITION] SET [SERIAL_CODE] = '" + oPRP.NewSerialNo + "',[CREATED_BY] = '" + oPRP.CreatedBy + "',[CREATED_ON] = GETDATE()");
            sbQuery.AppendLine(" WHERE [ASSET_CODE] = '" + oPRP.AssetCode + "' AND [SERIAL_CODE] = '" + oPRP.OldSerialNo + "' AND [COMP_CODE] = '" + oPRP.CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());
        }

        /// <summary>
        ///  Allocate assets in bulk mode.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public int AllocateExistingAssets(AssetAcquisition_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("INSERT INTO [ASSET_ALLOCATION] ([ASSET_CODE],[ASSET_ALLOCATION_DATE],");
            sbQuery.AppendLine(" [ASSET_ALLOCATED_EMP],[ALLOCATED_EMP_ID],[ALLOCATED_DEPARTMENT],[ALLOCATED_PROCESS],");
            sbQuery.AppendLine(" [ASSET_LOCATION],[COMP_CODE],[PORT_NO],[VLAN],");
            sbQuery.AppendLine(" [TICKET_NO],[GATEPASS_NO],[WORKSTATION_NO],[CREATED_BY],[CREATED_ON])");
            sbQuery.AppendLine(" VALUES ");
            sbQuery.AppendLine("('" + oPRP.AssetCode + "','" + oPRP.AllocationDate + "',");
            sbQuery.AppendLine(" '" + oPRP.AllocatedTo + "','" + oPRP.AllocatedToId + "','" + oPRP.DeptCode + "','" + oPRP.AssetProcess + "',");
            sbQuery.AppendLine(" '" + oPRP.AssetLocation + "','" + oPRP.CompCode + "','" + oPRP.PortNo + "','" + oPRP.AssetVlan + "',");
            sbQuery.AppendLine(" '" + oPRP.TicketNo + "','" + oPRP.GatePassNo + "','" + oPRP.WorkStationNo + "','" + oPRP.CreatedBy + "',GETDATE())");
            return oDb.ExecuteQueryTran(sbQuery.ToString());
        }

        /// <summary>
        /// update asset acquisition for bulk asset allocation.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public int UpdateAssetAcquisition(AssetAcquisition_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("UPDATE ASSET_ACQUISITION SET ASSET_ALLOCATED='True',ASSET_PROCESS='" + oPRP.AssetProcess + "',DEPARTMENT='" + oPRP.DeptCode + "',");
            sbQuery.AppendLine(" PORT_NO='" + oPRP.PortNo + "' WHERE ASSET_CODE='" + oPRP.AssetCode + "'");
            return oDb.ExecuteQueryTran(sbQuery.ToString());
        }

        /// <summary>
        /// Check department code validity when asset is being saved/updated.
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public bool ValidateRefInvoiceNo(string RefInvoiceNo, string CompCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS RefInvoiceNo FROM [ASSET_SOLD_DETAILS] WHERE [REFERENCE_INVOICE_NO]='" + RefInvoiceNo + "'");
            sbQuery.AppendLine(" AND [COMP_CODE]='" + CompCode + "'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["RefInvoiceNo"].ToString() == "0")
                    bResult = false;
            }
            return bResult;
        }
    }
}