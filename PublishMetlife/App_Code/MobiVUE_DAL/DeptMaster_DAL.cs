//************************************************************************************************************************************************
//  Data Access Layer Class For Add/Edit/Update/Delete Department Master
//  Created By : Neeraj Saxena
//  Created On : April 27, 2012
//  Modified By : 
//  Modified On : 
//************************************************************************************************************************************************
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
    /// Summary description for DeptMaster_DAL
    /// </summary>
    public class DeptMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public DeptMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~DeptMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Checks Duplicate Department Code
        /// </summary>
        /// <param name="_DeptCode"></param>
        /// <returns>bDup</returns>
        private bool CheckDuplicateDept(string _DeptCode, string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM DEPARTMENT_MASTER WHERE DEPT_CODE = '" + _DeptCode.Trim() + "'");
                sbQuery.Append(" AND COMP_CODE='" + _CompCode.Trim() + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Fetches Company details
        /// </summary>
        /// <returns></returns>
        public DataTable GetCompany()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COMP_CODE,COMP_NAME FROM COMPANY_MASTER WHERE ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Save/Update Department Master
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns>bResult</returns>
        public bool SaveUpdateDept(string OpType, DeptMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateDept(oPRP.DeptCode, oPRP.CompCode))
                    {
                        //Add new Dept...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [DEPARTMENT_MASTER]([DEPT_CODE],[DEPT_NAME],[REMARKS],[ACTIVE],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES ('" + oPRP.DeptCode.Trim() + "','" + oPRP.DeptName.Trim() + "','" + oPRP.Remarks.Trim() + "','" + oPRP.Active + "',");
                        sbQuery.Append(" '" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update Dept Information...
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [DEPARTMENT_MASTER] SET [DEPT_NAME] = '" + oPRP.DeptName + "',[REMARKS] = '" + oPRP.Remarks + "',[ACTIVE] = '" + oPRP.Active + "',");
                    sbQuery.Append("[MODIFIED_BY] = '" + oPRP.ModifiedBy + "',[MODIFIED_ON] = GETDATE()");
                    sbQuery.Append("WHERE [Dept_CODE] = '" + oPRP.DeptCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
                }
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Fetches Department Records For GridView Population
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetDept(string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT DEPT_CODE,DEPT_NAME,REMARKS,ACTIVE,CREATED_BY,CONVERT(VARCHAR,CREATED_ON,105) AS CREATED_ON,");
                sbQuery.Append(" MODIFIED_BY,CONVERT(VARCHAR,MODIFIED_ON,105) AS MODIFIED_ON FROM DEPARTMENT_MASTER");
                sbQuery.Append(" WHERE COMP_CODE='" + _CompCode + "'");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Delete A Department From Department Master
        /// </summary>
        /// <param name="_EmpCode"></param>
        /// <returns>bResult</returns>
        public string DeleteDept(string _DeptCode,string _CompCode)
        {
            try
            {
                string DelRslt = "";
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS DEPT FROM PROCESS_MASTER WHERE DEPT_CODE='" + _DeptCode + "' AND COMP_CODE='" + _CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                {
                    int iChild = 0;
                    int.TryParse(dt.Rows[0]["DEPT"].ToString(), out iChild);
                    if (iChild > 0)
                    {
                        DelRslt = "PROCESS_MAPPED";
                        return DelRslt;
                    }
                }
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM DEPARTMENT_MASTER WHERE DEPT_CODE='" + _DeptCode + "' AND COMP_CODE='" + _CompCode + "'");
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