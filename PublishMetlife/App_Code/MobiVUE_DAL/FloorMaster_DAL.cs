using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace MobiVUE_ATS.DAL
{

    public class FloorMaster_DAL
    {

        clsDb oDb;
        StringBuilder sbQuery;
        public FloorMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~FloorMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        private bool CheckDuplicate(string _FloorCode,string _SiteCode,string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM FLOOR_MASTER WHERE FLOOR_CODE = '" + _FloorCode.Trim().Replace("'", "''") + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool SaveUpdateFloor(string OpType, FloorMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                int iRes = 0;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicate(oPRP.FloorCode,oPRP.SiteCode,oPRP.CompCode))
                    {
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [FLOOR_MASTER] ([FLOOR_CODE],[FLOOR_NAME], [SITE_CODE],[REMARKS],[ACTIVE],[CREATED_BY],[CREATED_ON],[COMP_CODE])");
                        sbQuery.Append(" VALUES('" + oPRP.FloorCode + "','" + oPRP.FloorName + "', '" + oPRP.SiteCode + "', '" + oPRP.Remarks + "','" + oPRP.Active + "','" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.CompCode + "')");
                        iRes = oDb.ExecuteQuery(sbQuery.ToString());
                    }

                }
                else if (OpType == "UPDATE")
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [FLOOR_MASTER] SET [REMARKS] = '" + oPRP.Remarks + "'");
                    sbQuery.Append((" ,[ACTIVE] = '" + oPRP.Active + "', [UPDATED_BY]='" + oPRP.ModifiedBy + "', [UPDATED_ON] = GETDATE() WHERE FLOOR_CODE='" + oPRP.FloorCode + "' AND [FLOOR_NAME]='" + oPRP.FloorName + "' AND [SITE_CODE]= '" + oPRP.SiteCode + "' AND [COMP_CODE]= '" + oPRP.CompCode + "' "));
                    iRes = oDb.ExecuteQuery(sbQuery.ToString());
                }

                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetFloor(string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT FLOOR_ID,FLOOR_CODE,FLOOR_NAME,ACTIVE,CREATED_BY,CONVERT(VARCHAR,CREATED_ON,105) AS CREATED_ON,SITE_CODE,COMP_CODE,REMARKS from FLOOR_MASTER WHERE ACTIVE=1 AND COMP_CODE = '" + _CompCode+"' ORDER BY FLOOR_ID DESC ");

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
                sbQuery.Append("SELECT SITE_CODE, SITE_ADDRESS from SITE_MASTER WHERE ACTIVE = 1 AND SITE_CODE <> 'ALL' AND COMP_CODE = '"+ _CompCode + "' ");

                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataTable GetStore()
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * from STORE_MASTER");

                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public string DeleteFloor(FloorMaster_PRP oPRP)
        {
            try
            {
                string DelRslt = "";
                sbQuery = new StringBuilder();

                sbQuery.Append("DELETE FROM [FLOOR_MASTER] WHERE [FLOOR_CODE] = '" + oPRP.FloorCode + "' AND [FLOOR_NAME]='" + oPRP.FloorName + "' AND [SITE_CODE]= '" + oPRP.SiteCode + "' AND [COMP_CODE]= '" + oPRP.CompCode + "' ");
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