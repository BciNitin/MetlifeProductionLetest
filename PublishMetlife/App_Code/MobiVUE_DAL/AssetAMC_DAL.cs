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
    /// Summary description for Asset AMC Data Access Layer
    /// </summary>
    public class AssetAMC_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public AssetAMC_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~AssetAMC_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Get vendor Master details
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendor(string CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT VENDOR_CODE,VENDOR_NAME FROM VENDOR_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "'");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        

        /// <summary>
        /// Get category code/name for category dropdown selection.
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategory(string _AssetType)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT CATEGORY_CODE,CATEGORY_NAME FROM CATEGORY_MASTER");
                sbQuery.Append(" WHERE ASSET_TYPE='" + _AssetType + "' AND ACTIVE=1");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Get new running serial no from asset AMC data table.
        /// </summary>
        /// <returns></returns>
        public int GetMaxAMCRunningNo()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ISNULL(MAX(AMC_RUNNING_NO),0)+1 AS RSN FROM ASSET_AMC");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return int.Parse(dt.Rows[0]["RSN"].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssetsForAMC(AssetAMC_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT AA.ASSET_CODE,AA.ASSET_ID,AA.MODEL_NAME,AA.SERIAL_CODE,AA.PO_NUMBER,LM.LOC_NAME,");
            sbQuery.Append(" CONVERT(VARCHAR,AA.PO_DATE,106) AS PO_DATE,AMC.AMC_CODE,AMC.AMC_RUNNING_NO");
            sbQuery.Append(" FROM ASSET_ACQUISITION AA LEFT JOIN ASSET_AMC AMC");
            sbQuery.Append(" ON AA.ASSET_CODE = AMC.ASSET_CODE");
            sbQuery.Append(" INNER JOIN LOCATION_MASTER LM ON AA.ASSET_LOCATION = LM.LOC_CODE");
            if (oPRP.bThisAMC)
            {
                sbQuery.Append(" WHERE AMC.AMC_CODE = '" + oPRP.AMCCode + "'");
                sbQuery.Append(" AND AA.SERIAL_CODE LIKE '" + oPRP.SerialCode + "%' AND AA.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "%'");
            }
            else
            {
                sbQuery.Append(" WHERE AMC.AMC_CODE IS NULL");
                sbQuery.Append(" AND AA.SERIAL_CODE LIKE '" + oPRP.SerialCode + "%' AND AA.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "%'");
            }
            sbQuery.Append(" AND AA.ASSET_ID LIKE '" + oPRP.AssetID + "%' AND AA.ASSET_TYPE LIKE '" + oPRP.AssetType + "%' AND AA.PO_NUMBER LIKE '" + oPRP.PONo + "%' AND ASSET_APPROVED = 'True' AND SOLD_SCRAPPED_STATUS IS NULL");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPRP"></param>
        /// <param name="OpType"></param>
        /// <returns></returns>
        public bool SaveUpdateAMCDetails(AssetAMC_PRP oPRP, string OpType)
        {
            int iRes = 0;
            bool bResult = false;
            if (OpType == "SAVE")
            {
                foreach (string AssetCode in oPRP.Assets)
                {
                    oPRP.AssetCode = AssetCode;
                    sbQuery = new StringBuilder();
                    sbQuery.Append("INSERT INTO [ASSET_AMC] ([AMC_CODE],[AMC_WARRANTY],[AMC_PURCHASE_DATE],[AMC_START_DATE],[AMC_END_DATE],[ASSET_CODE],[AMC_VALUE]");
                    sbQuery.Append(",[AMC_VENDOR_CODE],[AMC_DOCUMENT],[AMC_RESP_PERSON],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                    sbQuery.Append(" VALUES ");
                    sbQuery.Append("('" + oPRP.AMCCode + "','" + oPRP.AMCWarranty + "','" + oPRP.PurchaseDate + "','" + oPRP.StartDate + "','" + oPRP.EndDate + "','" + oPRP.AssetCode + "','" + oPRP.AMCValue + "'");
                    sbQuery.Append(",'" + oPRP.AMCVendorCode + "','" + oPRP.RefDocName + "','" + oPRP.RespPerson + "','" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                    iRes = oDb.ExecuteQuery(sbQuery.ToString());
                }
                if (iRes > 0)
                    bResult = true;
            }
            if (OpType == "UPDATE")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE [ASSET_AMC] SET [AMC_WARRANTY] = '" + oPRP.AMCWarranty + "',[AMC_PURCHASE_DATE] = '" + oPRP.PurchaseDate + "',[AMC_START_DATE] = '" + oPRP.StartDate + "'");
                sbQuery.Append(",[AMC_END_DATE] = '" + oPRP.EndDate + "',[AMC_VALUE] = '" + oPRP.AMCValue + "',[AMC_VENDOR_CODE] = '" + oPRP.AMCVendorCode + "',[AMC_DOCUMENT] = '" + oPRP.RefDocName + "'");
                sbQuery.Append(",[AMC_RESP_PERSON] = '" + oPRP.RespPerson + "',[COMP_CODE] = '" + oPRP.CompCode + "',[MODIFIED_BY] = '" + oPRP.ModifiedBy + "',[MODIFIED_ON] = GETDATE()");
                sbQuery.Append(" WHERE [AMC_CODE] = '" + oPRP.AMCCode + "'");
                iRes = oDb.ExecuteQuery(sbQuery.ToString());

                if (oPRP.AssetsToAdd)
                {
                    foreach (string AssetCode in oPRP.Assets)
                    {
                        oPRP.AssetCode = AssetCode;
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [ASSET_AMC] ([AMC_CODE],[AMC_WARRANTY],[AMC_PURCHASE_DATE],[AMC_START_DATE],[AMC_END_DATE],[ASSET_CODE],[AMC_VALUE]");
                        sbQuery.Append(",[AMC_VENDOR_CODE],[AMC_DOCUMENT],[AMC_RESP_PERSON],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES ");
                        sbQuery.Append("('" + oPRP.AMCCode + "','" + oPRP.AMCWarranty + "','" + oPRP.PurchaseDate + "','" + oPRP.StartDate + "','" + oPRP.EndDate + "','" + oPRP.AssetCode + "','" + oPRP.AMCValue + "'");
                        sbQuery.Append(",'" + oPRP.AMCVendorCode + "','" + oPRP.RefDocName + "','" + oPRP.RespPerson + "','" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                        iRes = oDb.ExecuteQuery(sbQuery.ToString());
                    }
                }
            }
            if (iRes > 0)
                bResult = true;
            return bResult;
        }

        /// <summary>
        /// Upload AMC details from excel file.
        /// </summary>
        /// <param name="oPRP"></param>
        public void UploadAMCDetails(AssetAMC_PRP oPRP)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO [ASSET_AMC] ([AMC_CODE],[AMC_WARRANTY],[AMC_PURCHASE_DATE],[AMC_START_DATE],[AMC_END_DATE],[ASSET_CODE],[AMC_VALUE]");
                sbQuery.Append(",[AMC_VENDOR_CODE],[AMC_DOCUMENT],[AMC_RESP_PERSON],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                sbQuery.Append(" VALUES ");
                sbQuery.Append("('" + oPRP.AMCCode + "','" + oPRP.AMCWarranty + "','" + oPRP.PurchaseDate + "','" + oPRP.StartDate + "','" + oPRP.EndDate + "','" + oPRP.AssetCode + "','" + oPRP.AMCValue + "'");
                sbQuery.Append(",'" + oPRP.AMCVendorCode + "','" + oPRP.RefDocName + "','" + oPRP.RespPerson + "','" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                oDb.ExecuteQuery(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Get AMC details for edit/update operation.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable GetAMCDetailsForUpdate(string _AMCCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT AMC_WARRANTY,AMC_PURCHASE_DATE,AMC_START_DATE,AMC_END_DATE,");
            sbQuery.Append(" AMC_VALUE,AMC_VENDOR_CODE,AMC_DOCUMENT,AMC_RESP_PERSON");
            sbQuery.Append(" FROM ASSET_AMC AMC WHERE AMC.AMC_CODE = '" + _AMCCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset details based on serial code provided.
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(string _SerialCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_ID,PO_NUMBER FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE SERIAL_CODE='" + _SerialCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Upload/save bulk AMC details.
        /// </summary>
        /// <param name="oPRP"></param>
        public void UploadNewAMCDetails(AssetAMC_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_CODE,ASSET_LOCATION,ASSET_MAKE,MODEL_NAME FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE SERIAL_CODE = '" + oPRP.SerialCode + "' AND COMP_CODE='" + oPRP.CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                oPRP.AssetCode = dt.Rows[0]["ASSET_CODE"].ToString();
                oPRP.AssetLocation = dt.Rows[0]["ASSET_LOCATION"].ToString();
                oPRP.AssetMake = (dt.Rows[0]["ASSET_MAKE"].ToString() != "") ? dt.Rows[0]["ASSET_MAKE"].ToString() : "";
                oPRP.AssetModelType = (dt.Rows[0]["MODEL_NAME"].ToString() != "") ? dt.Rows[0]["MODEL_NAME"].ToString() : "";
            }

            sbQuery = new StringBuilder();
            sbQuery.Append("INSERT INTO [ASSET_AMC] ([AMC_CODE],[AMC_WARRANTY],[AMC_START_DATE],[AMC_END_DATE],[ASSET_CODE],[AMC_VALUE]");
            sbQuery.Append(",[AMC_VENDOR_CODE],[AMC_RESP_PERSON],[COMP_CODE],[CREATED_BY],[CREATED_ON],[SERIAL_CODE]");
            sbQuery.Append(",[ASSET_LOCATION],[ASSET_MAKE],[MODEL_NAME])");
            sbQuery.Append(" VALUES ");
            sbQuery.Append("('" + oPRP.AMCCode + "','" + oPRP.AMCWarranty + "','" + oPRP.StartDate + "','" + oPRP.EndDate + "','" + oPRP.AssetCode + "','" + oPRP.AMCValue + "'");
            sbQuery.Append(",'" + oPRP.AMCVendorCode + "','" + oPRP.RespPerson + "','" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.SerialCode + "'");
            sbQuery.Append(",'" + oPRP.AssetLocation + "','" + oPRP.AssetMake + "','" + oPRP.AssetModelType + "')");
            oDb.ExecuteQuery(sbQuery.ToString());
        }

        /// <summary>
        /// Validate asset serial code within a location.
        /// </summary>
        /// <param name="SerialCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public bool ValidSerialCode(string SerialCode, string CompCode)
        {
            bool bResult = true;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) AS SERIAL FROM [ASSET_ACQUISITION] WHERE [SERIAL_CODE]='" + SerialCode + "'");
            sbQuery.Append(" AND [COMP_CODE]='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SERIAL"].ToString() == "0")
                    bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Upload/update bulk AMC details.
        /// </summary>
        /// <param name="oPRP"></param>
        public void UpdateAMCDetails(AssetAMC_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE [ASSET_AMC] SET [AMC_WARRANTY] = '" + oPRP.AMCWarranty + "'");
            sbQuery.Append(",[AMC_START_DATE] = '" + oPRP.StartDate + "',[AMC_END_DATE] = '" + oPRP.EndDate + "',[AMC_VALUE] = '" + oPRP.AMCValue + "',[AMC_VENDOR_CODE] = '" + oPRP.AMCVendorCode + "'");
            sbQuery.Append(",[AMC_RESP_PERSON] = '" + oPRP.RespPerson + "',[COMP_CODE] = '" + oPRP.CompCode + "',[MODIFIED_BY] = '" + oPRP.ModifiedBy + "'");
            sbQuery.Append(",[MODIFIED_ON] = GETDATE(),[ASSET_LOCATION] = '" + oPRP.AssetLocation + "',[ASSET_MAKE] = '" + oPRP.AssetMake + "',[MODEL_NAME] = '" + oPRP.AssetModelType + "'");
            sbQuery.Append(" WHERE [AMC_CODE] = '" + oPRP.AMCCode + "' AND [ASSET_CODE] = '" + oPRP.AssetCode + "' AND [SERIAL_CODE] = '" + oPRP.SerialCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());

            //sbQuery = new StringBuilder();
            //sbQuery.Append("");
        }

        /// <summary>
        /// Get count of expired AMC on the home page.
        /// </summary>
        /// <param name="DAYS"></param>
        /// <returns></returns>
        public int GetExpiredAMCCount(int DAYS, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) EXAMC FROM (SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE AMC_WARRANTY_END_DATE >= GETDATE() AND");
            sbQuery.Append(" AMC_WARRANTY_END_DATE < DATEADD(DAY," + DAYS + ",GETDATE()) AND AMC_WARRANTY_END_DATE IS NOT NULL AND");
            sbQuery.Append(" RTRIM(COALESCE(ASSET_CODE,'')) <> '' AND AMC_WARRANTY='AMC' AND COMP_CODE='" + CompCode + "' GROUP BY ASSET_CODE) A");
            return int.Parse(oDb.GetDataTable(sbQuery.ToString()).Rows[0]["EXAMC"].ToString());
        }

        /// <summary>
        /// Get AMC data fro export.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAMCForExport(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT AMC_CODE,AMC_WARRANTY,NULLIF(AMC_START_DATE,'') AS AMC_START_DATE,NULLIF(AMC_END_DATE,'') AS AMC_END_DATE,AMC_VALUE,");
            sbQuery.Append("AMC_VENDOR_CODE,AMC_RESP_PERSON,COMP_CODE,ASSET_CODE,");
            sbQuery.Append("SERIAL_CODE,ASSET_LOCATION,ASSET_MAKE,MODEL_NAME");
            sbQuery.Append(" FROM ASSET_AMC WHERE COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Update AMC/Warranty details when asset details are updated.
        /// </summary>
        /// <param name="AMC_WARRANTY"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="AssetCode"></param>
        /// <param name="SerialCode"></param>
        /// <param name="CompCode"></param>
        public void UpdateAMCDetails(string AMC_WARRANTY, string StartDate, string EndDate, string AssetCode, string SerialCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE [ASSET_AMC] SET [AMC_WARRANTY]='" + AMC_WARRANTY + "',[AMC_START_DATE]='" + StartDate + "',[AMC_END_DATE]='" + EndDate + "'");
            sbQuery.Append(" WHERE [ASSET_CODE]='" + AssetCode + "' AND [SERIAL_CODE]='" + SerialCode + "' AND [COMP_CODE]='" + CompCode + "'");
            oDb.ExecuteQuery(sbQuery.ToString());
        }

        public DataTable GetDashbordCount(string CompCode) 
        {
            return oDb.ExecuteSPWithOutput("SP_DASHBOARD_REPORT_COUNT", new System.Data.SqlClient.SqlParameter("COMP_CODE", CompCode));            
        }

    }
}