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
    /// Summary description for Asset Replacement Data Access Layer
    /// </summary>
    public class AssetReplacement_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public AssetReplacement_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~AssetReplacement_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }
        public DataTable GetMailTransactionDetails(string TransactionType, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM MAIL_MASTER WHERE TRANSACTION_TYPE='" + TransactionType + "' AND COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());

        }
        /// <summary>
        /// Save asset replacement details
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveUpdateAssetReplacementDetails(AssetReplacement_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO [ASSET_REPLACEMENT] ([ACTIVE_IN_ASSET_CODE],[SERIAL_CODE],[ASSET_TAG],[FAULTY_OUT_SERIAL_CODE],[REPLACEMENT_DATE],[DOCUMENT_NUMBER]");
                sbQuery.Append(",[CREATED_BY],[CREATED_ON],[REMARKS],[COMP_CODE],[ASSET_FAR_TAG],[FAULTY_OUT_ASSET_FAR_TAG],[REPLACEMENT_STATUS],[REPLACEMENT_ASSET_SUB_STATUS])");
                sbQuery.Append(" VALUES ");
                sbQuery.Append("('" + oPRP.AssetCode + "','" + oPRP.ActiveInAssetCode + "','" + oPRP.AssetTag + "','" + oPRP.FaultyOutSerialCode + "','" + oPRP.Replacement_Date + "','" + oPRP.DocumentNo + "'");
                sbQuery.Append(",'" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.ReplaceRemarks + "','" + oPRP.CompCode + "','" + oPRP.Asset_FAR_TAG + "','" + oPRP.FaultyOutAssetFarTagOld + "','STOCK','WORKING')");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());

                int iRs = 0;
                if (oPRP.CompCode == "IT")
                { 
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [ASSET_ACQUISITION] SET [SERIAL_CODE]='" + oPRP.ActiveInAssetCode + "'");
                    sbQuery.Append(",[ASSET_APPROVED] = '1',REMARKS='" + oPRP.ReplaceRemarks + "' WHERE [SERIAL_CODE] = '" + oPRP.FaultyOutSerialCode + "' ");
                    iRs = oDb.ExecuteQuery(sbQuery.ToString());
                }
                else
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [ASSET_ACQUISITION] SET [ASSET_FAR_TAG]='" + oPRP.Asset_FAR_TAG + "'");
                    sbQuery.Append(",[ASSET_APPROVED] = '1',REMARKS='" + oPRP.ReplaceRemarks + "' WHERE [ASSET_FAR_TAG] = '" + oPRP.FaultyOutAssetFarTagOld + "' ");
                    iRs = oDb.ExecuteQuery(sbQuery.ToString());
                }

                if (oPRP.upload != null)
                    oDb.ExecuteSP("sp_Save_File_Uploads", new SqlParameter("ID", oPRP.upload.ID), new SqlParameter("Process", oPRP.upload.Process), new SqlParameter("FileName", oPRP.upload.FileName), new SqlParameter("Createdby", oPRP.upload.User));

                if (iRes > 0 && iRs > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetAllSearchAssetDetails(string _AssetMake, string Model, string AssetType, string SiteLocation, string SerialNo, string Status, string Compcode, string filterStatus, string filterSubStatus, string FilterAssetFarTag, string FilterAssetDomain, string FilterAssetHDD, string FilterAssetProcessor, string FilterAssetRAM, string FilterAssetPoNumber, string FilterAssetVendor, string FilterInvoiceNo, string FilterAssetRFIDTag, string FilterAssetGRNNo)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("EXEC sp_GetScrapAssetDetails '" + _AssetMake + "','" + Model + "','" + AssetType + "','" + SiteLocation + "','" + SerialNo + "', '" + Status + "','" + Compcode + "','" + filterStatus + "','" + filterSubStatus + "','" + FilterAssetFarTag + "','" + FilterAssetDomain + "','" + FilterAssetHDD + "','" + FilterAssetProcessor + "','" + FilterAssetRAM + "','" + FilterAssetPoNumber + "','" + FilterAssetVendor + "','" + FilterInvoiceNo + "','" + FilterAssetRFIDTag + "','" + FilterAssetGRNNo + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetSiteLocation(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select distinct SITE_CODE from SITE_MASTER WHERE ACTIVE=1  AND COMP_CODE ='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable ValidateAssetInScrap(string AssetCode, string AssetTag)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append(" select * from ASSET_SOLD_DETAILS where ASSET_CODE = '" + AssetCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetAssetDetailsforEmail(string _AssetCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_LOCATION,ASSET_TYPE,ASSET_MAKE,MODEL_NAME,ASSET_PROCESSOR,ASSET_RAM,ASSET_HDD,PO_NUMBER,CONVERT(VARCHAR,NULLIF(PO_DATE,''),105) AS 'PO_DATE',INVOICE_NO,CONVERT(VARCHAR,NULLIF(INVOICE_DATE,''),105) AS 'INVOICE_DATE' FROM ASSET_ACQUISITION WHERE ASSET_CODE = '" + _AssetCode.Trim() + "' AND COMP_CODE='" + _CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public bool SaveUpdateScrapDetails(AssetReplacement_PRP oPRP)
        {
            try
            {
                DataTable dt = ValidateAssetInScrap(oPRP.AssetCode, oPRP.AssetTag);
                bool bResult = false;
                if (dt.Rows.Count == 0)
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("INSERT INTO [ASSET_SOLD_DETAILS] ([ASSET_CODE],[ASSET_TAG],[SCRAP_DATE]");
                    sbQuery.Append(",[CREATED_BY],[CREATED_ON],[REMARKS],[COMP_CODE],[Status])");
                    sbQuery.Append(" VALUES ");
                    sbQuery.Append("('" + oPRP.AssetCode + "','" + oPRP.AssetTag + "','" + oPRP.ScrapDate + "'");
                    sbQuery.Append(",'" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.ScrapRemark + "','" + oPRP.CompCode + "','SCRAP')");
                    int iRes = oDb.ExecuteQuery(sbQuery.ToString());

                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [ASSET_ACQUISITION] SET STATUS = 'SCRAP', SOLD_SCRAPPED_STATUS='SCRAP',ASSET_SUB_STATUS='RETIRED' ");
                    //sbQuery.Append(" WHERE [TAG_ID] = '" + oPRP.AssetTag + "' AND COMP_CODE='" + oPRP.CompCode + "' ");
                    sbQuery.Append(" WHERE [ASSET_CODE] = '" + oPRP.AssetCode + "' AND COMP_CODE='" + oPRP.CompCode + "' ");
                    int iRs = oDb.ExecuteQuery(sbQuery.ToString());

                    if (iRes > 0 && iRs > 0)
                        bResult = true;
                }
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Get Asset Replacement details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssetReplacementDetails(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append(" SELECT AR.ACTIVE_IN_ASSET_CODE,AR.ASSET_TAG,AR.FAULTY_OUT_SERIAL_CODE,AR.DOCUMENT_Number,NULLIF(AR.REPLACEMENT_DATE,'') AS REPLACEMENT_DATE,");
            sbQuery.Append(" AR.ACTIVE_SEC_GE_NO,NULLIF(AR.ACTIVE_SEC_GE_DATE,'') AS ACTIVE_SEC_GE_DATE,AR.BONDED_TYPE,AR.CREATED_BY,AR.REMARKS,AA.SERIAL_CODE,AA.STATUS,AA.ASSET_SUB_STATUS,AA.ASSET_FAR_TAG,AR.FAULTY_OUT_ASSET_FAR_TAG ");
            sbQuery.Append(" FROM ASSET_REPLACEMENT AR INNER JOIN ASSET_ACQUISITION AA ON AR.ACTIVE_IN_ASSET_CODE = AA.ASSET_CODE");
            sbQuery.Append(" WHERE AR.COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return dt;
        }


        public DataTable GetSoldScrappedDetails(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append(" SELECT ASS.ASSET_CODE, ASS.ASSET_TAG, NULLIF(ASS.SCRAP_DATE,'') AS SCRAP_DATE ,");
            sbQuery.Append(" ASS.CREATED_BY,ASS.REMARKS");
            sbQuery.Append(" FROM ASSET_SOLD_DETAILS ASS INNER JOIN ASSET_ACQUISITION AA ON ASS.ASSET_TAG = AA.TAG_ID");
            sbQuery.Append(" WHERE ASS.COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssetCode()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return dt;
        }

        public string GetAssetCode(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' ");
            else
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK'  ");
            //return oDb.GetDataTable(sbQuery.ToString());
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_CODE"].ToString();
            else return "";
        }

        public string GetReceivedBy(string _CompUser)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT USER_NAME from USER_ACCOUNTS WHERE USER_ID = '" + _CompUser.Trim() + "' ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["USER_NAME"].ToString();
            else return "";
        }

        public string GetAssetCodeScrap(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' ");
            else
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' ");
            //return oDb.GetDataTable(sbQuery.ToString());
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_CODE"].ToString();
            else return "";
        }

        public DataTable GetSubStatus(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE ASSET_CODE = '" + _AssetCode.Trim() + "' AND STATUS='STOCK' ");
            return oDb.GetDataTable(sbQuery.ToString());    
        }

        public string GetRFIDTag(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' ");
            else
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK'  ");
            //sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "'");
            //return oDb.GetDataTable(sbQuery.ToString());
            //ASSET_TAG
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TAG_ID"].ToString();
            else return "";
        }

        public string GetRFIDTagSCRAP(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' ");
            else
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TAG_ID"].ToString();
            else return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(string _AssetCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_CODE,ASSET_MAKE,MODEL_NAME, TAG_ID, AQ.VENDOR_CODE,SERIAL_CODE, VENDOR_NAME,INVOICE_NO, PO_NUMBER FROM ASSET_ACQUISITION AQ left JOIN VENDOR_MASTER  VM on AQ.VENDOR_CODE=VM.VENDOR_CODE");
            //sbQuery.Append(" WHERE   (TAG_ID='" + _AssetCode + "' or ASSET_CODE='" + _AssetCode + "' ) AND STATUS='STOCK' AND ASSET_SUB_STATUS='WORKING' AND SOLD_SCRAPPED_STATUS is null and (AQ.TAG_ID != null or AQ.TAG_ID !='')  ");
            sbQuery.Append(" WHERE   ASSET_CODE='" + _AssetCode + "' AND STATUS='STOCK' AND ASSET_SUB_STATUS='WORKING' AND SOLD_SCRAPPED_STATUS is null ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetDetailwithSerialNumber(string _SerialNumber, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_CODE,ASSET_MAKE,MODEL_NAME, TAG_ID, AQ.VENDOR_CODE,AQ.SERIAL_CODE, VENDOR_NAME,INVOICE_NO, PO_NUMBER,AQ.STATUS,AQ.ASSET_SUB_STATUS,AQ.ASSET_FAR_TAG  FROM ASSET_ACQUISITION AQ left JOIN VENDOR_MASTER  VM on AQ.VENDOR_CODE=VM.VENDOR_CODE");
            //sbQuery.Append(" WHERE   (TAG_ID='" + _AssetCode + "' or ASSET_CODE='" + _AssetCode + "' ) AND STATUS='STOCK' AND ASSET_SUB_STATUS='WORKING' AND SOLD_SCRAPPED_STATUS is null and (AQ.TAG_ID != null or AQ.TAG_ID !='')  ");
            sbQuery.Append(" WHERE   SERIAL_CODE='" + _SerialNumber + "' AND AQ.COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' AND SOLD_SCRAPPED_STATUS is null ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetDetailwithAssetFARTag(string _AssetFarTag, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_CODE,ASSET_MAKE,MODEL_NAME, TAG_ID, AQ.VENDOR_CODE,AQ.SERIAL_CODE, VENDOR_NAME,INVOICE_NO, PO_NUMBER,AQ.STATUS,AQ.ASSET_SUB_STATUS,AQ.ASSET_FAR_TAG FROM ASSET_ACQUISITION AQ left JOIN VENDOR_MASTER  VM on AQ.VENDOR_CODE=VM.VENDOR_CODE");
            //sbQuery.Append(" WHERE   (TAG_ID='" + _AssetCode + "' or ASSET_CODE='" + _AssetCode + "' ) AND STATUS='STOCK' AND ASSET_SUB_STATUS='WORKING' AND SOLD_SCRAPPED_STATUS is null and (AQ.TAG_ID != null or AQ.TAG_ID !='')  ");
            sbQuery.Append(" WHERE   ASSET_FAR_TAG='" + _AssetFarTag + "' AND AQ.COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK'  AND SOLD_SCRAPPED_STATUS is null ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable ValidateScrapAssetDetails(string _AssetCode,string TagId, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_CODE,ASSET_MAKE,MODEL_NAME, TAG_ID, AQ.VENDOR_CODE, VENDOR_NAME,INVOICE_NO, PO_NUMBER FROM ASSET_ACQUISITION AQ left JOIN VENDOR_MASTER  VM on AQ.VENDOR_CODE=VM.VENDOR_CODE");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "'  AND STATUS='STOCK' AND SOLD_SCRAPPED_STATUS is null ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool ChkFaultySerialNoExists(string FaultySerialNo,string CompCode)
        {
            bool bExists = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) AS CSN FROM ASSET_REPLACEMENT");
            sbQuery.Append(" WHERE FAULTY_OUT_SERIAL_CODE='" + FaultySerialNo + "' AND COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows[0]["CSN"].ToString() != "0")
                bExists = true;
            return bExists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActiveSerialNo"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public bool ChkActiveSerialNoExists(string ActiveSerialNo, string CompCode)
        {
            bool bExists = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) AS CSN FROM [ASSET_ACQUISITION]");
            sbQuery.Append(" WHERE SERIAL_CODE='" + ActiveSerialNo + "' AND COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows[0]["CSN"].ToString() != "0")
                bExists = true;
            return bExists;
        }
        public bool ChkActiveAssetFarTagExists(string ActiveAssetFarTag, string CompCode)
        {
            bool bExists = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) AS CSN FROM [ASSET_ACQUISITION]");
            sbQuery.Append(" WHERE ASSET_FAR_TAG='" + ActiveAssetFarTag + "' AND COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows[0]["CSN"].ToString() != "0")
                bExists = true;
            return bExists;
        }
        //public DataTable GetSerialNoExists(string AssetTag)
        //{
        //    sbQuery = new StringBuilder();
        //    sbQuery.Append("SELECT SERIAL_CODE FROM [ASSET_ACQUISITION]");
        //    sbQuery.Append(" WHERE TAG_ID='" + AssetTag + "' ");
        //    DataTable dt = oDb.GetDataTable(sbQuery.ToString());
        //    return dt;
        //}
        public DataTable GetSerialNoExists(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SERIAL_CODE FROM [ASSET_ACQUISITION]");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "' ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return dt;
        }
    }

}