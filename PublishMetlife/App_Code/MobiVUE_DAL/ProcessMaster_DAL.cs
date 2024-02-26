//************************************************************************************************************************************************
//  Data Access Layer Class For Add/Edit/Update/Delete Process Master
//  Created By : Neeraj Saxena
//  Created On : June 01, 2012
//  Modified By : Neeraj Saxena
//  Modified On : June 03, 2012 
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
    /// Summary description for Process Master Data Access Layer
    /// </summary>
    public class ProcessMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public ProcessMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~ProcessMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Checks Duplicate Process Code
        /// </summary>
        /// <param name="_DeptCode"></param>
        /// <returns>bDup</returns>
        private bool CheckDuplicateProcess(string _ProcessCode, string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM [PROCESS_MASTER] WHERE [PROCESS_CODE]='" + _ProcessCode.Trim().Replace("'", "''") + "'");
                sbQuery.Append(" AND [COMP_CODE]='" + _CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Save/Update Process Master
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns>bResult</returns>
        public bool SaveUpdateProcess(string OpType, ProcessMaster_PRP  oPRP)
        {
            try
            {
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateProcess(oPRP.ProcessCode,oPRP.CompCode))
                    {
                        //Add New Process Master...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [PROCESS_MASTER] ([DEPT_CODE],[PROCESS_CODE],[PROCESS_NAME],[ACTIVE],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES ('" + oPRP.DeptCode + "','" + oPRP.ProcessCode + "','" + oPRP.ProcessName + "','" + oPRP.Active + "',");
                        sbQuery.Append(" '" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update Process Master Information...
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [PROCESS_MASTER] SET [DEPT_CODE] = '" + oPRP.DeptCode + "',[PROCESS_NAME] = '" + oPRP.ProcessName + "',[MODIFIED_BY] = '" + oPRP.ModifiedBy + "',");
                    sbQuery.Append(" [ACTIVE]='" + oPRP.Active + "',[MODIFIED_ON] = GETDATE() WHERE [PROCESS_CODE] = '" + oPRP.ProcessCode + "' AND COMP_CODE='" + oPRP.CompCode + "'");
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
        /// Fetches Process Master Records For GridView Population
        /// </summary>
        /// <returns>DataTable</returns>        
        public DataTable GetProcessDetails(string CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT DM.[DEPT_NAME],PM.[PROCESS_CODE],PM.[PROCESS_NAME],PM.[ACTIVE],PM.[CREATED_BY],");
                sbQuery.Append(" CONVERT(VARCHAR,PM.[CREATED_ON],103) AS CREATED_ON ");
                sbQuery.Append(" FROM [PROCESS_MASTER] PM");
                sbQuery.Append(" INNER JOIN [DEPARTMENT_MASTER] DM");
                sbQuery.Append(" ON PM.DEPT_CODE = DM.DEPT_CODE WHERE PM.[COMP_CODE]='" + CompCode + "'");
                sbQuery.Append(" AND PM.[COMP_CODE] = DM.[COMP_CODE]");
                sbQuery.Append(" ORDER BY PM.[PROCESS_CODE],PM.[PROCESS_NAME]");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Delete A Process From Process Master
        /// </summary>
        /// <param name="_EmpCode"></param>
        /// <returns>bResult</returns>
        public string DeleteProcess(string _ProcessCode,string _CompCode)
        {
            try
            {
                string DelRslt = "";
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS PROCESS FROM EMPLOYEE_MASTER WHERE EMP_PROCESS_CODE='" + _ProcessCode + "' AND COMP_CODE='" + _CompCode + "'");
                DataTable dtRefChk = oDb.GetDataTable(sbQuery.ToString());
                if (dtRefChk.Rows[0]["PROCESS"].ToString() != "0")
                {
                    DelRslt = "PROCESS_MAPPED";
                    return DelRslt;
                }
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS PROCESS FROM ASSET_ACQUISITION WHERE ASSET_PROCESS='" + _ProcessCode + "' AND COMP_CODE='" + _CompCode + "'");
                DataTable dt1 = oDb.GetDataTable(sbQuery.ToString());
                if (dt1.Rows[0]["PROCESS"].ToString() != "0")
                {
                    DelRslt = "ASSET_MAPPED";
                    return DelRslt;
                }
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM [PROCESS_MASTER] WHERE [PROCESS_CODE] = '" + _ProcessCode + "' AND COMP_CODE='" + _CompCode + "'");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    DelRslt = "SUCCESS";
                return DelRslt;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Get employee code/name to be populated into dropdownlist.
        /// </summary>
        /// <returns></returns>
        public DataTable GetEmployee(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT EMPLOYEE_CODE,EMPLOYEE_NAME FROM EMPLOYEE_MASTER WHERE ACTIVE = '1' AND COMP_CODE='" + CompCode + "'");
            sbQuery.Append(" ORDER BY EMPLOYEE_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get department code/name to be populated into dropdownlist.
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepartment(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DEPT_CODE,DEPT_NAME FROM DEPARTMENT_MASTER WHERE ACTIVE = '1' AND COMP_CODE='" + CompCode + "'");
            sbQuery.Append(" ORDER BY DEPT_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}