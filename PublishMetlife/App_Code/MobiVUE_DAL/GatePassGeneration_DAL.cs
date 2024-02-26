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

namespace MobiVUE_ATS.DAL
{
    /// <summary>
    /// Summary description for GatePassGeneration_DAL
    /// </summary>
    public class GatePassGeneration_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public GatePassGeneration_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~GatePassGeneration_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        public void Transaction(string Process, string DatabaseType)
        {
            switch (Process)
            {
                case "BEGIN":
                    if (!oDb.IsConnected) oDb.Connect(DatabaseType);
                    oDb.BeginTrans();
                    break;
                case "COMMIT":
                    oDb.CommitTrans();
                    break;
                case "ROLLBACK":
                    oDb.RollBack();
                    break;
            }
        }
        public DataTable GetMailTransactionDetails(string TransactionType, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM MAIL_MASTER WHERE TRANSACTION_TYPE='" + TransactionType + "' AND COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());

        }
        public DataTable GetAssetDetailsforEmail(string _AssetCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_LOCATION,ASSET_TYPE,ASSET_MAKE,MODEL_NAME,ASSET_PROCESSOR,ASSET_RAM,ASSET_HDD,PO_NUMBER,CONVERT(VARCHAR,NULLIF(PO_DATE,''),105) AS 'PO_DATE',INVOICE_NO,CONVERT(VARCHAR,NULLIF(INVOICE_DATE,''),105) AS 'INVOICE_DATE' FROM ASSET_ACQUISITION WHERE ASSET_CODE = '" + _AssetCode.Trim() + "' AND COMP_CODE='" + _CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        /// <summary>
        /// Fetch Locations details for mapping with user id
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns>DataTable</returns>
        public DataTable GetLocation(string CompCode, string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  SITE_CODE LOC_CODE, SITE_ADDRESS LOC_NAME FROM SITE_MASTER");
            sbQuery.Append(" WHERE ACTIVE='1' AND SITE_CODE <> 'ALL'  AND COMP_CODE ='" + CompCode + "' ");
            // sbQuery.Append("  ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetLocation(string _ParentLocCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  SITE_CODE, SITE_ADDRESS FROM SITE_MASTER");
            sbQuery.Append(" WHERE ACTIVE='1' AND SITE_CODE = '" + _ParentLocCode + "' AND SITE_CODE <> 'ALL'  AND COMP_CODE ='" + CompCode + "' ");
            // sbQuery.Append("  ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        //public DataTable GetLocationwithSerialNumber(string _ParentLocCode)
        //{
        //    sbQuery = new StringBuilder();
        //    sbQuery.Append("SELECT  SITE_CODE LOC_CODE, SITE_ADDRESS LOC_NAME FROM SITE_MASTER");
        //    sbQuery.Append(" WHERE ACTIVE='1' AND SITE_CODE = '" + _ParentLocCode + "' ");
        //    sbQuery.Append("  ACTIVE='1'");
        //    return oDb.GetDataTable(sbQuery.ToString());
        //}

        public DataTable GetEmployee(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT EMPLOYEE_CODE,EMPLOYEE_NAME FROM EMPLOYEE_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendor(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT VENDOR_CODE,VENDOR_NAME FROM VENDOR_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        public string GetUserLocationCode(string _UserID)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT LOCATION_CODE FROM USER_ACCOUNTS WHERE USER_ID='" + _UserID + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return dt.Rows[0]["LOCATION_CODE"].ToString();
        }

        public string GetLocationwithAssetCode(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_LOCATION FROM ASSET_ACQUISITION WHERE ASSET_CODE='" + _AssetCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return dt.Rows[0]["ASSET_LOCATION"].ToString();
        }

        /// <summary>
        /// Fetch category code/name details to be populated into dropdownlist.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateCategory(string Comp_Code)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT Distinct  ASSET_TYPE CATEGORY_CODE, ASSET_TYPE CATEGORY_NAME FROM ASSET_ACQUISITION WHERE COMP_CODE = '" + Comp_Code + "' AND STATUS ='STOCK'");
            // sbQuery.Append(" WHERE ACTIVE=1 ");

            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateAssetMake(string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [ASSET_MAKE] FROM [ASSET_ACQUISITION] WHERE [ASSET_Type] = '" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateModelName(string AssetMake, string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [MODEL_NAME] FROM [ASSET_ACQUISITION] WHERE [ASSET_MAKE]='" + AssetMake + "'");
            sbQuery.Append(" AND [ASSET_TYPE]='" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetAssetsWithLocation(GatePassGeneration_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ACQ.ASSET_CODE, TAG_ID,ACQ.ASSET_ID,ACQ.SERIAL_CODE,ACQ.ASSET_MAKE,ACQ.MODEL_NAME,ACQ.ASSET_LOCATION,VENDOR_CODE FROM ASSET_ACQUISITION ACQ");
            //  sbQuery.Append(" INNER JOIN LOCATION_MASTER LM ON ACQ.ASSET_LOCATION = LM.LOC_CODE");
            //sbQuery.Append(" WHERE /*ACQ.TAG_ID LIKE '" + oPRP.AssetTag + "%'*/ ");
            sbQuery.Append(" WHERE ACQ.ASSET_CODE LIKE '" + oPRP.AssetCode + "%'");
            if (oPRP.AssetMake != "")
                sbQuery.Append(" AND ASSET_MAKE LIKE '" + oPRP.AssetMake + "%'");
            if (oPRP.CategoryCode != "")
                sbQuery.Append(" AND ACQ.ASSET_TYPE LIKE '" + oPRP.CategoryCode + "%'");
            if (oPRP.ModelName != "")            
                sbQuery.Append(" AND MODEL_NAME IN (" + oPRP.ModelName + ")");            
            sbQuery.Append(" AND ACQ.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND ACQ.COMP_CODE='" + oPRP.CompCode + "'  AND SOLD_SCRAPPED_STATUS IS NULL AND STATUS='STOCK'");
            sbQuery.Append(" AND ACQ.ASSET_CODE NOT IN (SELECT GA.ASSET_CODE FROM GATEPASS_ASSETS GA where GA.GATEPASS_IN_DATE IS NULL)");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAllSearchAssetDetails(string _AssetMake, string Model, string AssetType, string SiteLocation, string SerialNo, string Status, string Compcode, string filterStatus, string filterSubStatus, string FilterAssetFarTag, string FilterAssetDomain, string FilterAssetHDD, string FilterAssetProcessor, string FilterAssetRAM, string FilterAssetPoNumber, string FilterAssetVendor, string FilterInvoiceNo, string FilterAssetRFIDTag, string FilterAssetGRNNo)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("EXEC sp_GetAssetLocationtoLocation '" + _AssetMake + "','" + Model + "','" + AssetType + "','" + SiteLocation + "','" + SerialNo + "', '" + Status + "','" + Compcode + "','" + filterStatus + "','" + filterSubStatus + "','" + FilterAssetFarTag + "','" + FilterAssetDomain + "','" + FilterAssetHDD + "','" + FilterAssetProcessor + "','" + FilterAssetRAM + "','" + FilterAssetPoNumber + "','" + FilterAssetVendor + "','" + FilterInvoiceNo + "','" + FilterAssetRFIDTag + "','" + FilterAssetGRNNo + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetAssetDataTable(string AssetCode,string AssetTag, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ACQ.ASSET_CODE, TAG_ID, ACQ.ASSET_ID, ACQ.SERIAL_CODE, ACQ.ASSET_MAKE, ACQ.MODEL_NAME, ACQ.ASSET_LOCATION, VENDOR_CODE FROM ASSET_ACQUISITION ACQ ");
            sbQuery.Append(" WHERE ACQ.COMP_CODE = '" + CompCode + "'  AND  SOLD_SCRAPPED_STATUS IS NULL AND STATUS = 'STOCK' AND ");
            sbQuery.Append("(ACQ.ASSET_CODE = '" + AssetCode + "' AND ACQ.TAG_ID='"+ AssetTag + "' AND ACQ.TAG_ID NOT IN(SELECT GA.ASSET_TAG FROM GATEPASS_ASSETS GA where GA.GATEPASS_IN_DATE IS NULL))");
            return oDb.GetDataTable(sbQuery.ToString());
        }



        /// <summary>
        /// Get live gate pass asset code if found any.
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        //public string ChkLiveGP(string _AssetCode)
        //{
        //    sbQuery = new StringBuilder();
        //    sbQuery.Append("SELECT GG.GATEPASS_CODE FROM GATEPASS_GENERATION GG INNER JOIN GATEPASS_ASSETS GA ON GG.GATEPASS_CODE=GA.GATEPASS_CODE ");
        //    sbQuery.Append(" WHERE GA.ASSET_CODE='" + _AssetCode + "' AND GG.GATEPASS_IN_DATE IS NULL");
        //    DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
        //    if (dt.Rows.Count > 0)
        //        return dt.Rows[0]["GATEPASS_CODE"].ToString();
        //    else return "";
        //}

        public string ChkLiveGP(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT GATEPASS_CODE FROM GATEPASS_ASSETS G INNER JOIN ASSET_ACQUISITION A ON A.ASSET_CODE = G.ASSET_CODE WHERE A.STATUS = 'IN TRANSIT' AND G.ASSET_CODE = '"+_AssetCode+"'");
            DataTable dt = oDb.GetDataTableInTransaction(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["GATEPASS_CODE"].ToString();
            else return "";
        }


        public string GetAssetCode(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '"+_CompCode+"' AND STATUS='STOCK'  ");
            else
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' ");
            //return oDb.GetDataTable(sbQuery.ToString());
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_CODE"].ToString();
            else return "";
        }

        public string GetRFIDTag(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' ");
            else
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' ");
            //sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "'");
            //return oDb.GetDataTable(sbQuery.ToString());
            //ASSET_TAG
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TAG_ID"].ToString();
            else return "";
        }
        /// <summary>
        /// Get new running serial no from asset acquisition data table.
        /// </summary>
        /// <returns></returns>
        public int GetMaxRunningSrlNo(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ISNULL(MAX(RUNNING_SERIAL_NO),0) + 1 AS RSN FROM GATEPASS_GENERATION WHERE [COMP_CODE]='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return int.Parse(dt.Rows[0]["RSN"].ToString());
        }

        /// <summary>
        /// Save new gate pass details.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveGatePassDetails(GatePassGeneration_PRP oPRP)
        {
            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("INSERT INTO [GATEPASS_GENERATION] ([GATEPASS_CODE],[RUNNING_SERIAL_NO],[COMP_CODE],[GATEPASS_DATE],[GATEPASS_TYPE],[GATEPASS_VENDOR_CODE]");
            sbQuery.Append(",[GATEPASS_EMPLOYEE_CODE],[ASSET_LOCATION],[GATEPASS_BEARER_NAME],[GATEPASS_CARRIER_NAME],[GATEPASS_REMARKS]");
            sbQuery.Append(",[GATEPASS_IN_BY],[GATEPASS_IN_DATE],[GATEPASS_IN_STATUS],[GATEPASS_IN_LOC_CODE],[GATEPASS_OUT_BY],[GATEPASS_OUT_DATE]");
            sbQuery.Append(",[GATEPASS_OUT_LOC_CODE],[CREATED_BY],[CREATED_ON],[PURPOSE],[APPROVE_GATEPASS],[DOCUMENT_NO],[DESTINATION_LOCATION])");
            sbQuery.Append(" VALUES ");
            sbQuery.Append("('" + oPRP.GatePassCode + "','" + oPRP.Running_Serial_No + "','" + oPRP.CompCode + "','" + oPRP.GatePassDate + "','" + oPRP.GatePassType + "','" + oPRP.VendorCode + "'");
            sbQuery.Append(",'" + oPRP.EmpCode + "','" + oPRP.AssetLocation + "','" + oPRP.BearerName + "','" + oPRP.CarrierName + "','" + oPRP.Remarks + "'");
            sbQuery.Append(", NULL, NULL, NULL, NULL, NULL, NULL");
            sbQuery.Append(",'" + oPRP.DestinationLocation + "','" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.Remarks + "','" + oPRP.Approve_GatePass + "','" + oPRP.DocumentNo + "','" + oPRP.DestinationLocation + "')");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResult = true;

            return bResult;
        }

        /// <summary>
        /// Save Gate pass assets details.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveGatePassAssets(GatePassGeneration_PRP oPRP)
        {
            bool bResult = false;
            if(!CheckDuplicateAsset(oPRP))
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO [GATEPASS_ASSETS] ([GATEPASS_CODE],[ASSET_CODE],[EXP_RETURN_DATE],[TRANSFER_ITEM]");
                sbQuery.Append(",[GATEPASS_IN_LOC],[GATEPASS_IN_DATE],[GATEPASS_IN_BY],[GATEPASS_OUT_LOC],[GATEPASS_OUT_DATE],[GATEPASS_OUT_BY]");
                sbQuery.Append(",[CREATED_BY],[CREATED_ON],[COMP_CODE],[ASSET_TAG])");
                sbQuery.Append(" VALUES");
                sbQuery.Append("('" + oPRP.GatePassCode + "','" + oPRP.AssetCode + "','" + oPRP.ExpReturnDate + "','" + oPRP.TransferItem + "'");
                sbQuery.Append(",'" + oPRP.AssetLocation + "','" + oPRP.GatePassDate + "','" + oPRP.CreatedBy + "','" + oPRP.DestinationLocation + "', NULL, NULL");
                sbQuery.Append(",'" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.CompCode + "','" + oPRP.AssetTag + "')");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;


                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE ASSET_ACQUISITION SET Status='IN TRANSIT' WHERE  ASSET_CODE='" + oPRP.AssetCode + "'");
                int iRs = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRs > 0)
                    bResult = true;
            }

            return bResult;
        }

        private bool CheckDuplicateAsset(GatePassGeneration_PRP oPRP)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM GATEPASS_ASSETS WHERE GATEPASS_CODE = '" + oPRP.GatePassCode + "' AND ASSET_CODE = '"+ oPRP.AssetCode + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        /// <summary>
        /// Get Gate pass details to be populated into grid for viewing.
        /// </summary>
        /// <returns></returns>
        public DataTable GetGatePassDetails(GatePassGeneration_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GG.GATEPASS_CODE,CONVERT(VARCHAR,GG.GATEPASS_DATE,106) AS GATEPASS_DATE,GG.GATEPASS_TYPE,ASSET_LOCATION LOC_NAME ");
            sbQuery.Append("  FROM GATEPASS_GENERATION GG");
            //sbQuery.Append(" LEFT OUTER JOIN VENDOR_MASTER VM ON GG.GATEPASS_VENDOR_CODE = VM.VENDOR_CODE AND GG.COMP_CODE = VM.COMP_CODE");
            //sbQuery.Append(" LEFT OUTER JOIN EMPLOYEE_MASTER EM ON GG.GATEPASS_EMPLOYEE_CODE = EM.EMPLOYEE_CODE AND GG.COMP_CODE = EM.COMP_CODE");
            // sbQuery.Append(" INNER JOIN LOCATION_MASTER LM ON GG.ASSET_LOCATION = LM.LOC_CODE");
            sbQuery.Append(" WHERE GG.GATEPASS_TYPE LIKE '" + oPRP.GatePassType + "%' AND GG.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%'");
            sbQuery.Append(" AND GG.GATEPASS_CODE LIKE '" + oPRP.GatePassCode + "%' AND GG.COMP_CODE='" + oPRP.CompCode + "' ");
            //sbQuery.Append(" AND GG.COMP_CODE = LM.COMP_CODE");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get gate pass details for printing gate pass.
        /// </summary>
        /// <param name="_GatePassCode"></param>
        /// <returns></returns>
        public DataTable GetPrintGatepassDetails(string GatePassCode, bool ApproveStatus)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT (SELECT COUNT(1) FROM vw_GetPrintGatepassDetails WHERE GATEPASS_CODE = '" + GatePassCode + "') AS TOTAL,");
            sbQuery.Append(" VWPGP.GATEPASS_CODE,VWPGP.ASSET_CODE,VWPGP.TAG_ID,VWPGP.ASSET_MAKE,VWPGP.MODEL_NAME,CONVERT(VARCHAR,NULLIF(VWPGP.GATEPASS_DATE,''),106) AS GATEPASS_DATE,");
            sbQuery.Append(" CONVERT(VARCHAR,NULLIF(VWPGP.EXP_RETURN_DATE,''),106) AS EXP_RETURN_DATE,VWPGP.GATEPASS_TYPE,ASSET_LOCATION AS ASSET_LOCATION,");
            sbQuery.Append(" DESTINATION_LOCATION AS DEST_LOCATION, VWPGP.GATEPASS_BEARER_NAME,VWPGP.GATEPASS_CARRIER_NAME,VWPGP.CREATED_BY,VWPGP.CREATED_ON,VWPGP.GATE_PASS_FOR,");
            sbQuery.Append(" VWPGP.GATEPASS_EMPLOYEE_CODE,VM.VENDOR_CODE,VM.VENDOR_NAME,(VM.VENDOR_ADDRESS + ', ' + VM.VENDOR_CITY + ', ' + VM.VENDOR_STATE + ', ' +");
            sbQuery.Append(" VM.VENDOR_COUNTRY) AS VENDOR_ADDRESS,VWPGP.PURPOSE,VWPGP.SERIAL_CODE,VWPGP.ASSET_FAR_TAG,VWPGP.ASSET_ID ");
            sbQuery.Append(" FROM vw_GetPrintGatepassDetails VWPGP LEFT OUTER JOIN VENDOR_MASTER VM");
            sbQuery.Append(" ON VWPGP.GATEPASS_VENDOR_CODE = VM.VENDOR_CODE AND VWPGP.COMP_CODE = VM.COMP_CODE");
            //sbQuery.Append(" INNER JOIN LOCATION_MASTER LM ON VWPGP.ASSET_LOCATION = LM.LOC_CODE AND VWPGP.COMP_CODE = LM.COMP_CODE");
            //  sbQuery.Append(" INNER JOIN LOCATION_MASTER LOM ON VWPGP.DESTINATION_LOCATION = LOM.LOC_CODE AND VWPGP.COMP_CODE = LM.COMP_CODE");
            sbQuery.Append(" WHERE VWPGP.GATEPASS_CODE='" + GatePassCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get gate pass details for printing gate pass.
        /// </summary>
        /// <param name="_GatePassCode"></param>
        /// <returns></returns>
        public DataTable MailGatepassDetails(string _GatePassCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT (SELECT COUNT(1) FROM vw_GetPrintGatepassDetails WHERE GATEPASS_CODE = '" + _GatePassCode + "') AS TOTAL,");
            sbQuery.Append(" VWPGP.GATEPASS_CODE,VWPGP.ASSET_CODE,VWPGP.SERIAL_CODE,VWPGP.ASSET_MAKE,VWPGP.MODEL_NAME,CONVERT(VARCHAR,VWPGP.GATEPASS_DATE,106) AS GATEPASS_DATE,");
            sbQuery.Append(" CONVERT(VARCHAR,VWPGP.EXP_RETURN_DATE,106) AS EXP_RETURN_DATE,VWPGP.GATEPASS_TYPE,VWPGP.ASSET_LOCATION,");
            sbQuery.Append(" VWPGP.GATEPASS_BEARER_NAME,VWPGP.GATEPASS_CARRIER_NAME,VWPGP.CREATED_BY,VWPGP.CREATED_ON,VWPGP.GATE_PASS_FOR,");
            sbQuery.Append(" VWPGP.GATEPASS_EMPLOYEE_CODE,VM.VENDOR_CODE,VM.VENDOR_NAME,(VM.VENDOR_ADDRESS + ', ' + VM.VENDOR_CITY + ', ' + VM.VENDOR_STATE + ', ' +");
            sbQuery.Append(" VM.VENDOR_COUNTRY) AS VENDOR_ADDRESS,VWPGP.PURPOSE ");
            sbQuery.Append(" FROM vw_GetPrintGatepassDetails VWPGP LEFT JOIN VENDOR_MASTER VM");
            sbQuery.Append(" ON VWPGP.GATEPASS_VENDOR_CODE = VM.VENDOR_CODE");
            sbQuery.Append(" WHERE VWPGP.GATEPASS_CODE='" + _GatePassCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetches Company details for mapping with user id
        /// </summary>
        /// <returns></returns>
        public DataTable GetCompany()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COMP_CODE,COMP_NAME FROM COMPANY_MASTER WHERE COMP_OWNER=0");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get gatepass details based on gatepass code given.
        /// </summary>
        /// <param name="_GatePassCode"></param>
        /// <returns></returns>
        public DataSet GetGatePassDetails(string _GatePassCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM GATEPASS_GENERATION WHERE GATEPASS_CODE = '" + _GatePassCode + "'; ");
            sbQuery.Append("SELECT GPA.GATEPASS_CODE,GPA.EXP_RETURN_DATE,AA.ASSET_CODE,AA.ASSET_ID,AA.TAG_ID ,AA.ASSET_MAKE,AA.MODEL_NAME,AA.Vendor_Code,");
            sbQuery.Append(" AA.ASSET_LOCATION,AA.PORT_NO FROM GATEPASS_ASSETS GPA INNER JOIN ASSET_ACQUISITION AA");
            sbQuery.Append(" ON GPA.ASSET_CODE = AA.ASSET_CODE WHERE GPA.GATEPASS_CODE = '" + _GatePassCode + "'");
            return oDb.GetDataSet(sbQuery.ToString());
        }

        public DataTable GetSiteLocation(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select distinct SITE_CODE from SITE_MASTER WHERE SITE_CODE<>'ALL' AND COMP_CODE ='" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get Gatepass no. to be populated into dropdown selection.
        /// </summary>
        /// <returns></returns>
        public DataTable GetGatePassNo(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GATEPASS_CODE FROM GATEPASS_GENERATION WHERE COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public bool isGatePassNoExist(string GatePassNo,string CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM GATEPASS_ASSETS WHERE GATEPASS_CODE = '" + GatePassNo.Trim() + "' AND COMP_CODE = '" + CompCode + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(string _AssetCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_ID,ASSET_MAKE,SERIAL_CODE,MODEL_NAME,ASSET_TYPE FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "' AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPendingGatePass(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(ASSET_CODE) AS EXGP FROM GATEPASS_ASSETS WHERE EXP_RETURN_DATE IS NOT NULL AND");
            sbQuery.Append(" EXP_RETURN_DATE < GETDATE() AND GATEPASS_IN_DATE IS NULL AND GATEPASS_OUT_DATE IS NOT NULL AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString()).Rows[0]["EXGP"].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GatePassCode"></param>
        /// <returns></returns>
        public bool ApproveGatePassDetails(string GatePassCode)
        {
            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE GATEPASS_GENERATION SET APPROVE_GATEPASS='True'");
            sbQuery.Append(" WHERE GATEPASS_CODE='" + GatePassCode + "'");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResult = true;
            return bResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetGatePassCount(string CompCode)
        {
            string AssetCount = "";
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) AS UNAPPROVED FROM GATEPASS_GENERATION WHERE COMP_CODE='" + CompCode + "' AND APPROVE_GATEPASS='False';");
            sbQuery.Append("SELECT COUNT(*) AS APPROVED FROM GATEPASS_GENERATION WHERE COMP_CODE='" + CompCode + "' AND APPROVE_GATEPASS='True'");
            DataSet ds = oDb.GetDataSet(sbQuery.ToString());
            AssetCount = ds.Tables[0].Rows[0]["UNAPPROVED"].ToString() + "^" + ds.Tables[1].Rows[0]["APPROVED"].ToString();
            return AssetCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GatePassCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public string DeleteGatePassDetails(string GatePassCode, string CompCode)
        {
            string DelRslt = "";
            sbQuery = new StringBuilder();
            sbQuery.Append("DELETE FROM [GATEPASS_GENERATION] WHERE [GATEPASS_CODE]='" + GatePassCode + "' AND COMP_CODE='" + CompCode + "'");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.Append("DELETE FROM [GATEPASS_ASSETS] WHERE [GATEPASS_CODE]='" + GatePassCode + "' AND COMP_CODE='" + CompCode + "'");
            int iRs = oDb.ExecuteQuery(sbQuery.ToString());

            return DelRslt;
        }
    }
}