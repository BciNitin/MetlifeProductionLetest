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

    public class TrainingorMeetingMaster_DAL
    {

        clsDb oDb;
        StringBuilder sbQuery;
        public TrainingorMeetingMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~TrainingorMeetingMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }
        public bool CheckDuplicatateMeetingorTrainingFloorSite(string _MasterName,string _MasterType,string _FloorCode, string _SiteCode,string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM TRAINING_AND_MEETING_MASTER WHERE MASTER_NAME = '" + _MasterName.Trim().Replace("'", "''") + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool SaveUpdateTrainingorMeetingMaster(string OpType, TrainingorMeetingMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                int iRes = 0;
                if (OpType == "SAVE")
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("INSERT INTO [TRAINING_AND_MEETING_MASTER] ([MASTER_CODE],[MASTER_NAME],[MASTER_TYPE],[FLOOR_CODE],[SITE_CODE],[REMARKS],[ACTIVE],[CREATED_BY],[CREATED_ON],[COMP_CODE])");
                    sbQuery.Append(" VALUES('" + oPRP.MasterCode + "','" + oPRP.MasterName + "','" + oPRP.MasterType + "', '" + oPRP.Floor + "', '" + oPRP.SiteCode + "', '" + oPRP.Remarks + "','" + oPRP.Active + "','" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.CompCode + "')");
                    iRes = oDb.ExecuteQuery(sbQuery.ToString());

                }
                else if (OpType == "UPDATE")
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [TRAINING_AND_MEETING_MASTER] SET [REMARKS] = '" + oPRP.Remarks + "'");
                    sbQuery.Append((" ,[ACTIVE] = '" + oPRP.Active + "', [UPDATED_BY]='" + oPRP.CreatedBy + "', [UPDATED_ON] = GETDATE() WHERE MASTER_CODE='" + oPRP.MasterCode + "' AND MASTER_NAME = '"+oPRP.MasterName+"' AND MASTER_TYPE = '"+oPRP.MasterType+"' AND FLOOR_CODE = '"+oPRP.Floor+"' AND SITE_CODE = '"+oPRP.SiteCode+"' AND COMP_CODE = '"+oPRP.CompCode+"' "));
                    iRes = oDb.ExecuteQuery(sbQuery.ToString());
                }

                if (iRes > 0)
                    bResult = true;
                return bResult;
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
        public DataTable GetTrainingandMeetingType(string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT MASTER_ID,MASTER_CODE,MASTER_NAME,REMARKS,ACTIVE,CREATED_BY,CONVERT(VARCHAR,CREATED_ON,105) AS CREATED_ON,SITE_CODE,FLOOR_CODE,MASTER_TYPE,COMP_CODE from TRAINING_AND_MEETING_MASTER WHERE COMP_CODE = '" + _CompCode+"' ORDER BY MASTER_ID DESC ");

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

        public DataTable GetFloor(string SiteCode,string CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT FLOOR_CODE,FLOOR_NAME FROM FLOOR_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND COMP_CODE = '"+ CompCode + "' ");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public string DeleteTrainingandMeetingMaster(TrainingorMeetingMaster_PRP oPRP)
        {
            try
            {
                string DelRslt = "";
                sbQuery = new StringBuilder();

                sbQuery.Append("DELETE FROM [TRAINING_AND_MEETING_MASTER] WHERE [MASTER_CODE] = '" + oPRP.MasterCode + "' AND MASTER_NAME = '" + oPRP.MasterName + "' AND MASTER_TYPE = '" + oPRP.MasterType + "' AND FLOOR_CODE = '" + oPRP.Floor + "' AND SITE_CODE = '" + oPRP.SiteCode + "' AND COMP_CODE = '" + oPRP.CompCode + "' ");
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