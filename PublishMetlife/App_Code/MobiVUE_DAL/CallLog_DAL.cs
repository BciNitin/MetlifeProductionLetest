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
    /// Summary description for Call Log Data Access Layer
    /// </summary>
    public class CallLog_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public CallLog_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~CallLog_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Get assets for call log management.
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <param name="_SerialCode"></param>
        /// <param name="_AssetMake"></param>
        /// <param name="_ModelName"></param>
        /// <returns></returns>
        public DataTable GetAssets(CallLog_PRP oPRP, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ACQ.ASSET_CODE,ACQ.SERIAL_CODE,ACQ.ASSET_ID,ACQ.ASSET_MAKE,ACQ.MODEL_NAME,ACQ.WORKSTATION_NO FROM ASSET_ACQUISITION ACQ");
            sbQuery.Append(" WHERE ACQ.ASSET_TYPE LIKE '" + oPRP.AssetType + "%' AND ACQ.ASSET_MAKE LIKE '" + oPRP.AssetMake + "%' AND ACQ.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND ACQ.SERIAL_CODE LIKE '" + oPRP.SerialCode + "%'");
            if (oPRP.ModelName != "")
                sbQuery.Append(" AND ACQ.[MODEL_NAME] IN (" + oPRP.ModelName + ")");
            else
                sbQuery.Append(" AND ACQ.[MODEL_NAME] LIKE '" + oPRP.ModelName + "%'");
            sbQuery.Append(" AND ACQ.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "%' AND ACQ.COMP_CODE='" + CompCode + "'");
            sbQuery.Append(" AND ACQ.ASSET_APPROVED = 'True' AND ACQ.SOLD_SCRAPPED_STATUS IS NULL");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get new running serial no from asset acquisition data table.
        /// </summary>
        /// <returns></returns>
        public int GetMaxCallLogId(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ISNULL(MAX(RUNNING_SERIAL_NO),0) + 1 AS RSN FROM [CALL_LOG_MGMT]");
            sbQuery.Append(" WHERE COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return int.Parse(dt.Rows[0]["RSN"].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CallLogCode"></param>
        /// <returns></returns>
        public DataTable GetCallLogAssets(string CallLogCode,string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ACQ.ASSET_CODE,ACQ.SERIAL_CODE,ACQ.ASSET_ID,ACQ.ASSET_MAKE,ACQ.MODEL_NAME,ACQ.WORKSTATION_NO FROM ASSET_ACQUISITION ACQ");
            sbQuery.Append(" INNER JOIN CALL_LOG_MGMT CLM ON ACQ.ASSET_CODE = CLM.ASSET_CODE");
            sbQuery.Append(" WHERE CLM.CALL_LOG_CODE='" + CallLogCode + "' AND ACQ.COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch vendor code/name to be populated.
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendor(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT VENDOR_CODE,VENDOR_NAME FROM VENDOR_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "' ORDER BY VENDOR_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch category code/name details to be populated into dropdownlist.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateCategory(string _AssetType, string _ParentCategory, int _CatLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT CATEGORY_CODE,CATEGORY_NAME FROM CATEGORY_MASTER");
            sbQuery.Append(" WHERE ASSET_TYPE='" + _AssetType + "' AND PARENT_CATEGORY='" + _ParentCategory + "'");
            sbQuery.Append(" AND CATEGORY_LEVEL=" + _CatLevel + " AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_ID,ASSET_MAKE,SERIAL_CODE,MODEL_NAME,ASSET_TYPE FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "'");
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
            sbQuery.Append("SELECT DISTINCT [ASSET_MAKE] FROM [ASSET_ACQUISITION] WHERE [CATEGORY_CODE] = '" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
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
            sbQuery.Append(" AND [CATEGORY_CODE]='" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_VendorCode"></param>
        /// <returns></returns>
        public DataTable GetEscalationDetails(string VendorCode,string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT VENDOR_CITY,VENDOR_CONT_PERSON FROM [VENDOR_MASTER] ");
            sbQuery.Append(" WHERE [VENDOR_CODE]='" + VendorCode + "' AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(string _AssetCode,string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_MAKE,SERIAL_CODE,MODEL_NAME,ASSET_TYPE FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "' AND COMP_CODE='" + _CompCode + "' AND ASSET_APPROVED='True'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Save call log details.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveCallLogDetails(CallLog_PRP oPRP)
        {
            bool bResp = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("INSERT INTO [CALL_LOG_MGMT] ([CALL_LOG_CODE],[RUNNING_SERIAL_NO],[ASSET_CODE],[SERIAL_CODE],[ASSET_MAKE],[MODEL_NAME],[VENDOR_CODE]");
            sbQuery.Append(",[VENDOR_LOCATION],[ENGINEER_NAME],[CALL_NO],[CALL_DATE],[RESPONDED_DATE],[RESOLVED_STATUS],[RESOLVED_DATE],[CREATED_BY],[CREATED_ON]");
            sbQuery.Append(",[CALL_LOG_REMARKS],[VENDOR_CONT_PERSON],[COMP_CODE],[PART_STATUS],[REPLACED_SERIAL_NO]");
            sbQuery.Append(",[FRU_NO],[GATEPASS_NO],[ACTION_TAKEN])");
            sbQuery.Append(" VALUES ('" + oPRP.CallLogCode + "'," + oPRP.RunningSerialNo + ",'" + oPRP.AssetCode + "','" + oPRP.SerialCode + "','" + oPRP.AssetMake + "','" + oPRP.ModelName + "','" + oPRP.VendorCode + "'");
            sbQuery.Append(",'" + oPRP.VendorLocation + "','" + oPRP.EngrName + "','" + oPRP.CallNo + "','" + oPRP.CallDate + "','" + oPRP.RespondedDate + "','" + oPRP.ResolvedStatus + "','" + oPRP.ResolvedDate + "','" + oPRP.CreatedBy + "',GETDATE()");
            sbQuery.Append(",'" + oPRP.Remarks + "','" + oPRP.VendorContPerson + "','" + oPRP.CompCode + "','" + oPRP.PartStatus + "','" + oPRP.ReplacedSrlNo + "'");
            sbQuery.Append(",'" + oPRP.FRUNO + "','" + oPRP.GatePassNo + "','" + oPRP.ActionTaken + "')");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResp = true;
            return bResp;
        }

        /// <summary>
        /// Update call log details.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool UpdateCallLogDetails(CallLog_PRP oPRP)
        {
            bool bResp = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE [CALL_LOG_MGMT] SET [ASSET_CODE] = '" + oPRP.AssetCode + "',[SERIAL_CODE] = '" + oPRP.SerialCode + "',[ASSET_MAKE] = '" + oPRP.AssetMake + "',[MODEL_NAME] = '" + oPRP.ModelName + "'");
            sbQuery.Append(",[VENDOR_CODE] = '" + oPRP.VendorCode + "',[VENDOR_LOCATION] = '" + oPRP.VendorLocation + "',[ENGINEER_NAME] = '" + oPRP.EngrName + "',[CALL_NO] = '" + oPRP.CallNo + "',[CALL_DATE] = '" + oPRP.CallDate + "',[RESPONDED_DATE] = '" + oPRP.RespondedDate + "',[RESOLVED_STATUS] = '" + oPRP.ResolvedStatus + "'");
            sbQuery.Append(",[RESOLVED_DATE] = '" + oPRP.ResolvedDate + "',[MODIFIED_BY] = '" + oPRP.ModifiedBy + "',[MODIFIED_ON] = GETDATE(),[CALL_LOG_REMARKS] = '" + oPRP.Remarks + "',[VENDOR_CONT_PERSON] = '" + oPRP.VendorContPerson + "'");
            sbQuery.Append(",[COMP_CODE] = '" + oPRP.CompCode + "',[PART_STATUS] = '" + oPRP.PartStatus + "',[REPLACED_SERIAL_NO] = '" + oPRP.ReplacedSrlNo + "',[FRU_NO] = '" + oPRP.FRUNO + "',[GATEPASS_NO] = '" + oPRP.GatePassNo + "',[ACTION_TAKEN] = '" + oPRP.ActionTaken + "'");
            sbQuery.Append(" WHERE [CALL_LOG_CODE] = '" + oPRP.CallLogCode + "'");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResp = true;
            return bResp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetCallLogDetails(string CompCode, bool bPending)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT CLM.CALL_LOG_CODE,CLM.ASSET_CODE,CLM.SERIAL_CODE,VM.VENDOR_CONT_PERSON,VM.VENDOR_CODE,VM.VENDOR_NAME,CLM.CALL_NO,CONVERT(VARCHAR,NULLIF(CLM.CALL_DATE,''),105) AS CALL_DATE,");
            sbQuery.Append(" CONVERT(VARCHAR,NULLIF(CLM.RESPONDED_DATE,''),105) AS RESPONDED_DATE,CONVERT(VARCHAR,NULLIF(CLM.RESOLVED_DATE,''),105) AS RESOLVED_DATE,CLM.RESOLVED_STATUS");
            sbQuery.Append(" FROM CALL_LOG_MGMT CLM INNER JOIN VENDOR_MASTER VM");
            sbQuery.Append(" ON CLM.VENDOR_CODE = VM.VENDOR_CODE AND CLM.COMP_CODE='" + CompCode + "'");
            if (bPending)
            {
                sbQuery.Append(" AND CLM.RESOLVED_STATUS='PENDING'");
            }
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Checking duplicate call log code value.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool ValidateCallLogCode(string _CallLogCode,string _CompCode)
        {
            bool bValidate = true;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) AS CLC FROM CALL_LOG_MGMT WHERE CALL_LOG_CODE='" + _CallLogCode + "' AND COMP_CODE='" + _CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows[0]["CLC"].ToString() != "0")
                bValidate = false;
            return bValidate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CallLogCode"></param>
        /// <returns></returns>
        public DataTable GetCallLogDetailsForUpdate(string _CallLogCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT CLM.CALL_LOG_CODE,CLM.ASSET_CODE,CLM.SERIAL_CODE,CLM.VENDOR_CODE,CLM.CALL_NO,CLM.CALL_DATE,NULLIF(CLM.RESPONDED_DATE,'') AS RESPONDED_DATE,");
            sbQuery.Append(" CLM.RESOLVED_STATUS,NULLIF(CLM.RESOLVED_DATE,'') AS RESOLVED_DATE,VM.VENDOR_CONT_PERSON,CLM.VENDOR_LOCATION,CLM.ENGINEER_NAME,");
            sbQuery.Append(" CLM.ACTION_TAKEN,CLM.REPLACED_SERIAL_NO,CLM.PART_STATUS,CLM.FRU_NO,CLM.GATEPASS_NO,CLM.CALL_LOG_REMARKS FROM CALL_LOG_MGMT CLM INNER JOIN VENDOR_MASTER VM");
            sbQuery.Append(" ON CLM.VENDOR_CODE = VM.VENDOR_CODE AND CLM.COMP_CODE = VM.COMP_CODE");
            sbQuery.Append(" WHERE CALL_LOG_CODE='" + _CallLogCode + "' AND CLM.COMP_CODE='" + _CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPendingCallLogs(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) AS PCL FROM CALL_LOG_MGMT WHERE COMP_CODE='" + CompCode + "' AND RESOLVED_STATUS='PENDING'");
            return oDb.GetDataTable(sbQuery.ToString()).Rows[0]["PCL"].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetCallNo(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [CALL_NO] FROM [CALL_LOG_MGMT] WHERE [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetCallLogCode(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [CALL_LOG_CODE] FROM [CALL_LOG_MGMT] WHERE [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public DataTable GetVendorDetails(string VendorCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT [CALL_LOG_CODE],[CALL_NO] FROM [CALL_LOG_MGMT]");
            sbQuery.Append(" WHERE [VENDOR_CODE] ='" + VendorCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetCallLogReport(CallLog_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT VM.VENDOR_NAME, CLM.VENDOR_LOCATION,CLM.CALL_NO,CLM.ASSET_MAKE,CLM.MODEL_NAME,");
            sbQuery.Append(" CLM.ENGINEER_NAME,CLM.SERIAL_CODE,CONVERT(VARCHAR,NULLIF(CLM.CALL_DATE,''),105) AS CALL_DATE,");
            sbQuery.Append(" CONVERT(VARCHAR,NULLIF(CLM.RESPONDED_DATE,''),105) AS RESPONDED_DATE,CONVERT(VARCHAR,NULLIF(CLM.RESOLVED_DATE,''),105) AS RESOLVED_DATE,");
            sbQuery.Append(" RESOLUTION_DAYS = CASE WHEN CONVERT(VARCHAR,CLM.RESOLVED_DATE,103) != '01/01/1900' THEN");
            sbQuery.Append(" DATEDIFF(DD,CLM.CALL_DATE,CLM.RESOLVED_DATE) ELSE DATEDIFF(DD,CLM.CALL_DATE,GETDATE()) END,");
            sbQuery.Append(" CLM.RESOLVED_STATUS,CLM.PART_STATUS,CLM.FRU_NO,CLM.GATEPASS_NO,CLM.VENDOR_CODE");
            sbQuery.Append(" FROM [CALL_LOG_MGMT] CLM INNER JOIN [VENDOR_MASTER] VM ON CLM.[VENDOR_CODE] = VM.[VENDOR_CODE]");
            sbQuery.Append(" WHERE CLM.VENDOR_CODE LIKE '" + oPRP.VendorCode + "%' AND CLM.CALL_LOG_CODE LIKE '" + oPRP.CallLogCode + "%' AND CLM.CALL_NO LIKE '" + oPRP.CallNo + "%'");
            sbQuery.Append(" AND CLM.RESOLVED_STATUS LIKE '" + oPRP.ResolvedStatus + "%' AND CLM.PART_STATUS LIKE '" + oPRP.PartStatus + "%'");
            sbQuery.Append(" AND CLM.ASSET_MAKE LIKE '" + oPRP.AssetMake + "%' AND CLM.COMP_CODE = '" + oPRP.CompCode + "'");
            if (oPRP.ModelName != "")
                sbQuery.Append(" AND CLM.MODEL_NAME IN (" + oPRP.ModelName + ")");
            else
                sbQuery.Append(" AND CLM.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            sbQuery.Append(" AND CLM.CALL_DATE >= CONVERT(DATETIME,'" + oPRP.CallDateFrom + "',105)");
            sbQuery.Append(" AND CLM.CALL_DATE <= CONVERT(DATETIME,'" + oPRP.CallDateTo + "',105)");
            //sbQuery.Append(" AND CLM.RESOLVED_DATE >= CONVERT(DATETIME,'" + oPRP.ResolvedDateFrom + "',105)");
            //sbQuery.Append(" AND CLM.RESOLVED_DATE <= CONVERT(DATETIME,'" + oPRP.ResolvedDateTo + "',105)");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}