using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace MobiVUE_ATS.DAL
{

    public class StoreMaster_DAL
    {

        clsDb oDb;
        StringBuilder sbQuery;
        public StoreMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~StoreMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }
        public bool CheckDuplicatateStoreFloorSite(string _StoreCode,string _FloorCode, string _SiteCode,string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM STORE_MASTER WHERE STORE_CODE = '" + _StoreCode.Trim().Replace("'", "''") + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool SaveUpdateStore(string OpType, StoreMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                int iRes = 0;
                if (OpType == "SAVE")
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("INSERT INTO [STORE_MASTER] ([STORE_CODE],[STORE_NAME],[FLOOR_CODE],[SITE_CODE],[REMARKS],[ACTIVE],[CREATED_BY],[CREATED_ON],[COMP_CODE])");
                    sbQuery.Append(" VALUES('" + oPRP.StoreCode + "','" + oPRP.StoreName + "', '" + oPRP.Floor + "', '" + oPRP.SiteCode + "', '" + oPRP.Remarks + "','" + oPRP.Active + "','" + oPRP.CreatedBy + "',GETDATE(),'"+oPRP.CompCode+"')");
                    iRes = oDb.ExecuteQuery(sbQuery.ToString());
                }
                else if (OpType == "UPDATE")
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [STORE_MASTER] SET [REMARKS] = '" + oPRP.Remarks + "'");
                    sbQuery.Append((" ,[ACTIVE] = '" + oPRP.Active + "', [UPDATED_BY]='" + oPRP.ModifiedBy + "', [UPDATED_ON] = GETDATE() WHERE STORE_CODE='" + oPRP.StoreCode + "' AND [STORE_NAME]='" + oPRP.StoreName + "' AND [FLOOR_CODE] = '" + oPRP.Floor + "' AND [SITE_CODE] = '" + oPRP.SiteCode + "' AND COMP_CODE='" + oPRP.CompCode + "' "));
                    iRes = oDb.ExecuteQuery(sbQuery.ToString());
                }

                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetStore(string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append(" SELECT STORE_ID,STORE_CODE,STORE_NAME,REMARKS,ACTIVE,CREATED_BY,CONVERT(VARCHAR,CREATED_ON,105) AS CREATED_ON,SITE_CODE,FLOOR_CODE,COMP_CODE FROM STORE_MASTER WHERE ACTIVE=1 AND COMP_CODE='" + _CompCode+"' ORDER BY STORE_ID DESC ");

                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetStoreforHome(string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append(" SELECT SM.STORE_NAME AS STORE_NAME,COUNT(AA.ASSET_CODE) AS STORE_COUNT,CONCAT(SM.STORE_NAME,'  :  ',COUNT(AA.ASSET_CODE)) AS STORE FROM STORE_MASTER SM LEFT JOIN ASSET_ACQUISITION AA ON AA.STORE = SM.STORE_NAME WHERE SM.COMP_CODE = '" + _CompCode + "' GROUP BY SM.STORE_NAME ");

                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataTable GetSite(string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT SITE_CODE, SITE_ADDRESS from SITE_MASTER WHERE ACTIVE = 1 AND SITE_CODE<>'ALL' AND COMP_CODE = '"+_CompCode+"' ");

                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataTable GetFloor(string SiteCode, string CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT FLOOR_CODE,FLOOR_NAME FROM FLOOR_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND COMP_CODE = '"+CompCode+"'");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public string DeleteStore(StoreMaster_PRP oPRP)
        {
            try
            {
                string DelRslt = "";
                sbQuery = new StringBuilder();

                sbQuery.Append("DELETE FROM [STORE_MASTER] WHERE [STORE_CODE] = '" + oPRP.StoreCode + "' AND [STORE_NAME]='" + oPRP.StoreName + "' AND [FLOOR_CODE] = '" + oPRP.Floor + "' AND [SITE_CODE] = '" + oPRP.SiteCode + "' AND COMP_CODE='" + oPRP.CompCode + "' ");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    DelRslt = "SUCCESS";
                return DelRslt;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}