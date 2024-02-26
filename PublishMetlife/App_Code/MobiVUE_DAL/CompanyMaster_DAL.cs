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
    /// Summary description for CompanyMaster_DAL
    /// </summary>
    public class CompanyMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public CompanyMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~CompanyMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        private bool CheckDuplicateComp(string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM COMPANY_MASTER WHERE COMP_CODE = '" + _CompCode.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool SaveUpdateComp(string OpType, CompanyMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateComp(oPRP.CompCode))
                    {
                        //Add new Comp...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [COMPANY_MASTER]([COMP_CODE],[COMP_NAME],[REMARKS],[ACTIVE],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES ('" + oPRP.CompCode.Trim() + "','" + oPRP.CompName.Trim() + "','" + oPRP.Remarks.Trim() + "','" + oPRP.Active + "', ");
                        sbQuery.Append("'" + oPRP.CreatedBy + "',GETDATE())");
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update Comp Information...
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [COMPANY_MASTER] SET [COMP_NAME] = '" + oPRP.CompName + "',[REMARKS] = '" + oPRP.Remarks + "',[ACTIVE] = '" + oPRP.Active + "', ");
                    sbQuery.Append("[MODIFIED_BY]='" + oPRP.ModifiedBy + "',[MODIFIED_ON]=GETDATE()");
                    sbQuery.Append(" WHERE [COMP_CODE] = '" + oPRP.CompCode + "'");
                }
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true; 
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataTable GetComp()
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COMP_CODE, COMP_NAME, REMARKS, ACTIVE, CREATED_BY, CONVERT(VARCHAR,CREATED_ON,105) AS CREATED_ON FROM COMPANY_MASTER");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool DeleteComp(string _CompCode)
        {
            try
            {
                bool bResult = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("EXEC sp_DeleteAllDataOfCompany '" + _CompCode + "'");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}