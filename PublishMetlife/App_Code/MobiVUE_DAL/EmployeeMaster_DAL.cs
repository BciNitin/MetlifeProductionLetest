//************************************************************************************************************************************************
//  Data Access Layer Class For Add/Edit/Update/Delete Employee Master Details
//  Created By : Neeraj Saxena
//  Created On : April 27, 2012
//  Modified By : Neeraj Saxena 
//  Modified On : Sept 11, 2012
//************************************************************************************************************************************************
using System;
using System.Data;
using System.Text;

namespace MobiVUE_ATS.DAL
{
    /// <summary>
    /// Summary description for EmployeeMaster_DAL
    /// </summary>
    public class EmployeeMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public EmployeeMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~EmployeeMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Save/Update Employee Master
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns>bResult</returns>
        public bool SaveUpdateEmployee(string OpType, EmployeeMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateEmployee(oPRP.EmpCode, oPRP.CompCode))
                    {
                        //Add new Employee...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [EMPLOYEE_MASTER] ([EMPLOYEE_CODE],[EMPLOYEE_NAME]");
                        sbQuery.Append(",[EMP_EMAIL],[Designation],[SeatNo],[ACTIVE],[ProcessName],[LOB],[SubLOB],COMP_CODE,[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append("VALUES ('" + oPRP.EmpCode + "','" + oPRP.EmpName.Replace("'","`") + "','" + oPRP.EmpEmail.Replace("'", "`") + "','" + oPRP.Designation.Replace("'", "`") + "','" + oPRP.seatno.Replace("'", "`") + "',1");
                        sbQuery.Append(",'" + oPRP.Process.Replace("'", "`") + "','" + oPRP.Lob.Replace("'", "`") + "','" + oPRP.SubLob.Replace("'", "`") + "','" + oPRP.CompCode.Replace("'", "`") + "','" + oPRP.CreatedBy.Replace("'", "`") + "',GETDATE())");
                        int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                        if (iRes > 0)
                            bResult = true;
                    }
                    else
                    {
                        //Update Employee Information...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("UPDATE [EMPLOYEE_MASTER] SET [EMPLOYEE_NAME]='" + oPRP.EmpName.Replace("'", "`") + "',");
                        sbQuery.Append(" [EMP_EMAIL]='" + oPRP.EmpEmail.Replace("'", "`") + "',[Designation]= '"+ oPRP.Designation.Replace("'", "`") + "',[SeatNo]='"+ oPRP.seatno.Replace("'", "`") + "',[ProcessName]= '"+ oPRP.Process.Replace("'", "`") + "',[LOB]= '"+ oPRP.Lob.Replace("'", "`") + "',[SubLOB]= '"+ oPRP.SubLob.Replace("'", "`") + "',  ");
                        sbQuery.Append(" [MODIFIED_BY]='" + oPRP.ModifiedBy.Replace("'", "`") + "',[MODIFIED_ON]=GETDATE() ");
                        sbQuery.Append(" WHERE [EMPLOYEE_CODE]='" + oPRP.EmpCode + "' ");
                        int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                        if (iRes > 0)
                            bResult = true;
                    }
                }
                else if (OpType == "UPDATE")
                {
                    //Update Employee Information...
                    //sbQuery = new StringBuilder();
                    //sbQuery.Append("UPDATE [EMPLOYEE_MASTER] SET [EMPLOYEE_NAME]='" + oPRP.EmpName + "',[EMP_COMP_CODE]='" + oPRP.EmpCompCode + "'");
                    //sbQuery.Append(",[EMP_PROCESS_CODE]='" + oPRP.EmpProcCode + "',[EMP_REPORTING_TO]='" + oPRP.EmpReprotTo + "',[EMP_EMAIL]='" + oPRP.EmpEmail + "'");
                    //sbQuery.Append(",[EMP_PHONE]='" + oPRP.EmpPhone + "',[ACTIVE] = '" + oPRP.Active + "',[REMARKS] = '" + oPRP.EmpRemarks + "',[MODIFIED_BY]='" + oPRP.ModifiedBy + "',[MODIFIED_ON]=GETDATE()");
                    //sbQuery.Append(" WHERE [EMPLOYEE_CODE]='" + oPRP.EmpCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
                    //int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                    //if (iRes > 0)
                    //    bResult = true;
                }
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Upload employee details from excel file.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public void UploadEmployeeDetails(EmployeeMaster_PRP oPRP)
        {
            try
            {
                sbQuery = new StringBuilder();
                //sbQuery.Append("INSERT INTO [EMPLOYEE_MASTER] ([EMPLOYEE_CODE],[EMPLOYEE_NAME]");
                //sbQuery.Append(",[EMP_EMAIL],[Designation],[SeatNo],[ACTIVE],[ProcessName],[LOB],[SubLOB],[CREATED_BY],[CREATED_ON])");
                //sbQuery.Append("VALUES ('" + oPRP.EmpCode + "','" + oPRP.EmpName + "','" + oPRP.EmpProcCode + "','" + oPRP.EmpEmail + "'");
                //sbQuery.Append(",'" + oPRP.EmpDOJ + "','" + oPRP.EmpPhone + "','" + oPRP.Active + "','" + oPRP.EmpRemarks + "','" + oPRP.CreatedBy + "',GETDATE())");
                //oDb.ExecuteQuery(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Checks Duplicate Employee Code
        /// </summary>
        /// <param name="_EmpCode"></param>
        /// <returns>bDup</returns>
        public bool CheckDuplicateEmployee(string _EmpCode, string _CompCode)
        {
            bool bDup = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM EMPLOYEE_MASTER WHERE EMPLOYEE_CODE='" + _EmpCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                bDup = true;
            return bDup;
        }

        /// <summary>
        /// Checks Duplicate Email ID on update. 
        /// </summary>
        /// <param name="_EmpCode"></param>
        /// <returns>bDup</returns>
        public bool CheckDuplicateEmailID(string EmailID, string EmpCode, string CompCode)
        {
            bool bDup = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM EMPLOYEE_MASTER WHERE EMP_EMAIL='" + EmailID.Trim() + "' AND EMPLOYEE_CODE!='" + EmpCode + "' AND COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                bDup = true;
            return bDup;
        }

        /// <summary>
        /// Fetches Company Details For DropDownList Population
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetCompany()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COMP_CODE,COMP_NAME FROM COMPANY_MASTER WHERE ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetches Department Details For DropDownList Population
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetProcess(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetches Empoyee Details For DropDownList (Reporting To) Population
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetEmpoyee(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT EMPLOYEE_CODE,EMPLOYEE_NAME FROM EMPLOYEE_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetches Empoyee Records For GridView Population
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetEmpoyeeDetails(string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM dbo.vw_EmployeeDetails WHERE COMP_CODE='" + _CompCode + "' ORDER BY EMPLOYEE_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Delete An Employee From Employee Master.
        /// </summary>
        /// <param name="_EmpCode"></param>
        /// <returns>bResult</returns>
        public string DeleteEmployee(string _EmpCode, string _CompCode)
        {
            try
            {
                string DelRslt = "";
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS CNTEMP FROM EMPLOYEE_MASTER");
                sbQuery.Append(" WHERE EMP_REPORTING_TO='" + _EmpCode.Trim() + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                {
                    int iChild = 0;
                    int.TryParse(dt.Rows[0]["CNTEMP"].ToString(), out iChild);
                    if (iChild > 0)
                    {
                        DelRslt = "CHILD_FOUND";
                        return DelRslt;
                    }
                }
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS EMPALLOC FROM ASSET_ALLOCATION AL INNER JOIN ASSET_ACQUISITION AA");
                sbQuery.Append(" ON AL.ASSET_CODE = AA.ASSET_CODE WHERE AL.ALLOCATED_EMP_ID='" + _EmpCode + "' AND AA.ASSET_ALLOCATED='1'");
                sbQuery.Append(" AND AL.ACTUAL_RTN_DATE IS NULL AND AL.COMP_CODE='" + _CompCode + "'");
                DataTable dtALL = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                {
                    int iAll = 0;
                    int.TryParse(dtALL.Rows[0]["EMPALLOC"].ToString(), out iAll);
                    if (iAll > 0)
                    {
                        DelRslt = "ASSET_ALLOCATED";
                        return DelRslt;
                    }
                }
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM EMPLOYEE_MASTER WHERE EMPLOYEE_CODE = '" + _EmpCode + "' AND COMP_CODE='" + _CompCode + "'");
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

//server