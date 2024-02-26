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
    /// Summary description for GroupMaster_DAL
    /// </summary>
    public class GroupMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public GroupMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~GroupMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Checking duplicate group name in a company.
        /// </summary>
        /// <param name="_GroupCode"></param>
        /// <returns></returns>
        private bool CheckDuplicateGroup(string _GroupCode, string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM GROUP_MASTER WHERE GROUP_CODE='" + _GroupCode.Trim().Replace("'", "''") + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool CheckSelectedGroupEmpCount(string _GroupCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM USER_ACCOUNTS WHERE GROUP_CODE='" + _GroupCode.Trim().Replace("'", "''") + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool CheckDuplicateGroup(string _GroupCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM GROUP_MASTER WHERE GROUP_CODE='" + _GroupCode.Trim().Replace("'", "''") + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        /// <summary>
        /// Save/update group details into group master table.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveUpdateGroup(string OpType, GroupMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateGroup(oPRP.GroupCode, oPRP.CompCode))
                    {
                        //Add new Group...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [GROUP_MASTER]([GROUP_CODE],[GROUP_NAME],[REMARKS],[ACTIVE],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES ('" + oPRP.GroupCode.Trim() + "','" + oPRP.GroupName.Trim() + "','" + oPRP.Remarks.Trim() + "','" + oPRP.Active + "', ");
                        sbQuery.Append(" '" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE());");
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update Group Information...
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [GROUP_MASTER] SET [GROUP_NAME] = '" + oPRP.GroupName + "',[REMARKS] = '" + oPRP.Remarks + "',[ACTIVE] = '" + oPRP.Active + "', ");
                    sbQuery.Append("[MODIFIED_BY] = '" + oPRP.ModifiedBy + "',[MODIFIED_ON] = GETDATE()");
                    sbQuery.Append(" WHERE [GROUP_CODE] = '" + oPRP.GroupCode + "' ");
                }
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0 && OpType == "SAVE")
                {
                    sbQuery = null;
                    sbQuery = new StringBuilder();
                    sbQuery.Append("INSERT INTO GROUP_RIGHTS");
                    sbQuery.Append(" SELECT '" + oPRP.CompCode + "' AS CMP_CODE, '" + oPRP.GroupCode + "' AS GCODE,PAGE_CODE,PAGE_NAME,0,0,0,0,0 FROM PAGE_MASTER");
                    oDb.ExecuteQuery(sbQuery.ToString());
                }
                bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool UpdateEmployeetoGroup(GroupMaster_PRP oPRP)
        {
            bool result = false;
            sbQuery = new StringBuilder();
            sbQuery.Append(" UPDATE USER_ACCOUNTS SET GROUP_CODE = '"+oPRP.GroupCode+"' WHERE GROUP_CODE = '"+oPRP.isExistGroupCode+"' AND EMP_ID='"+oPRP.EmployeeID+"' ");
            int iResi = oDb.ExecuteQuery(sbQuery.ToString());
            if (iResi > 0)
                result = true;
            return result;
        }
        public bool RemoveEmployeetoGroup(GroupMaster_PRP oPRP)
        {
            bool result = false;
            sbQuery = new StringBuilder();
            sbQuery.Append(" DELETE FROM USER_ACCOUNTS WHERE GROUP_CODE = '" + oPRP.isExistGroupCode + "' AND EMP_ID='" + oPRP.EmployeeID + "' ");
            int iResi = oDb.ExecuteQuery(sbQuery.ToString());
            if (iResi > 0)
                result = true;
            return result;
        }

        public bool RemoveGroup(string GroupCode)
        {
            bool result = false;
            sbQuery = new StringBuilder();
            sbQuery.Append(" INSERT INTO GROUP_MASTER_HISTORY SELECT * FROM GROUP_MASTER WHERE GROUP_CODE = '" + GroupCode + "' ");
            int iResi = oDb.ExecuteQuery(sbQuery.ToString());
            if (iResi > 0)
                result = true;

            sbQuery = new StringBuilder();
            sbQuery.Append(" DELETE FROM GROUP_MASTER WHERE GROUP_CODE = '" + GroupCode + "' ");
            int iRes2 = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes2 > 0)
                result = true;
            return result;
        }


        /// <summary>
        /// Get group master details to be populated for being viewed.
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetGroup(string CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT GROUP_CODE,GROUP_NAME,REMARKS,ACTIVE,CREATED_BY,CONVERT(VARCHAR,CREATED_ON,105) AS CREATED_ON FROM GROUP_MASTER WHERE COMP_CODE = '"+CompCode+"' ");
               // sbQuery.Append(" WHERE COMP_CODE = '" + CompCode + "'");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataTable GetGroupDetails(string GroupCode,string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GROUP_CODE, GROUP_NAME, REMARKS FROM GROUP_MASTER WHERE GROUP_CODE ='" + GroupCode + "' AND COMP_CODE = '"+CompCode+"' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetUserDetails(string UserID)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM USER_ACCOUNTS ");
                sbQuery.Append("WHERE USER_ID = '" + UserID + "'");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataTable GetGroupMasterDetails(string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GROUP_CODE, GROUP_NAME, REMARKS FROM GROUP_MASTER WHERE ACTIVE=1 AND COMP_CODE ='"+_CompCode+"' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetGroupRights(string GroupCode, string CompCode)
        {
            return oDb.ExecuteSPWithOutput("SP_GET_GROUP_RIGHTS", new SqlParameter("GroupCode", GroupCode), new SqlParameter("Comp_Code", CompCode));
        }

        /// <summary>
        /// Delete group master details based on certain conditions.
        /// </summary>
        /// <param name="_GroupCode"></param>
        /// <param name="_CompCode"></param>
        /// <returns></returns>
        public string DeleteGroup(string _GroupCode,string _CompCode)
        {
            try
            {
                string DelRslt = "";
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS GRP FROM USER_ACCOUNTS WHERE GROUP_CODE='" + _GroupCode + "' ");
                DataTable dtRefChk = oDb.GetDataTable(sbQuery.ToString());
                if (dtRefChk.Rows[0]["GRP"].ToString() != "0")
                {
                    DelRslt = "GROUP_IN_USE";
                    return DelRslt;
                }
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM GROUP_MASTER WHERE GROUP_CODE='" + _GroupCode + "' ");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                {
                    sbQuery = null;
                    sbQuery = new StringBuilder();
                    sbQuery.Append("DELETE FROM GROUP_RIGHTS WHERE GROUP_CODE='" + _GroupCode + "' ");
                    oDb.ExecuteQuery(sbQuery.ToString());
                    DelRslt = "SUCCESS";
                }
                return DelRslt;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetSiteExceptAll(string GroupCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT SITE_CODE, SITE_ADDRESS from SITE_MASTER WHERE ACTIVE = 1 AND SITE_CODE = 'ALL' AND COMP_CODE='" + GroupCode + "' ");

                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetSite(string GroupCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT SITE_CODE, SITE_ADDRESS from SITE_MASTER WHERE ACTIVE = 1 AND COMP_CODE='"+GroupCode+"' ");

                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetEmpDetails(string empid)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select EMPLOYEE_CODE, EMPLOYEE_NAME, EMP_EMAIL ,EMP_TAG, Designation, SeatNo, ProcessName, Lob,SubLOB,Emp_Floor from [dbo].[EMPLOYEE_MASTER]  where EMPLOYEE_CODE ='" + empid + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetEmpDetailsUserGroupMapping(string empid)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select EMPLOYEE_CODE, EMPLOYEE_NAME, EMP_EMAIL ,EMP_TAG, Designation, SeatNo, ProcessName, Lob,SubLOB,Emp_Floor from EMPLOYEE_MASTER EM LEFT JOIN USER_ACCOUNTS UA ON EM.EMPLOYEE_CODE = UA.EMP_ID WHERE EMPLOYEE_CODE ='" + empid + "' AND UA.GROUP_CODE IS NULL OR UA.GROUP_CODE='' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable CheckEmployeeGroupMapping(string empid)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select EMPLOYEE_CODE, EMPLOYEE_NAME, EMP_EMAIL ,EMP_TAG, Designation, SeatNo, ProcessName, Lob,SubLOB,Emp_Floor from EMPLOYEE_MASTER EM LEFT JOIN USER_ACCOUNTS UA ON EM.EMPLOYEE_CODE = UA.EMP_ID WHERE EMPLOYEE_CODE ='" + empid + "' AND (UA.GROUP_CODE IS NOT NULL OR UA.GROUP_CODE<>'') ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable SaveUserLocationMapping(string empid, string Location, string CompCode, string useroperator, int Active, string Type)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append(" Exec sp_UserLocationMappingData '"+ Location + "','"+ empid + "',"+ Active + ",'"+ useroperator + "','"+ Type + "','"+ CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public string SaveUpdateGroupRights(DataTable dt, GroupMaster_PRP oPRP)
        {
            try
            {
                string msg = "";
                if(oPRP.BtnStatus== "Save" || oPRP.BtnStatus == "Update")
                {
                    DataTable gdt = oDb.ExecuteSPWithOutput("SP_SaveGroupRights", new SqlParameter("GroupRights", dt),
                    new SqlParameter("Group_Code", oPRP.GroupCode),
                    new SqlParameter("Group_Name", oPRP.GroupName),
                    new SqlParameter("Comp_Code", oPRP.CompCode),
                    new SqlParameter("Remarks", oPRP.GroupRemarks),
                    new SqlParameter("CREATED_BY", oPRP.CreatedBy), new SqlParameter("ACTIVE", oPRP.Active));

                    msg= gdt.Rows[0].Field<string>("ERR_MSG");
                }
                else if(oPRP.BtnStatus== "Add Employee")
                {
                    if (oPRP.LocationCode != "ALL")
                    {
                        DataTable dataTable = oDb.ExecuteSPWithOutput("SP_SaveUserMaster", new SqlParameter("Username", oPRP.UserName),
                            new SqlParameter("UserID", oPRP.UserID),
                            new SqlParameter("EmailID", oPRP.UserEmail),
                            new SqlParameter("SiteCode", oPRP.LocationCode),
                            new SqlParameter("GroupCode", oPRP.GroupCode),
                            new SqlParameter("EmployeeID", oPRP.EmployeeID),
                            new SqlParameter("Active", oPRP.Active),
                            new SqlParameter("COMPCODE", oPRP.CompCode),
                            new SqlParameter("CREATED_BY", oPRP.CreatedBy));

                        msg = dataTable.Rows[0].Field<string>("ERR_MSG");
                    }
                    else
                    {
                        DataTable dl = GetSiteExceptAll(oPRP.CompCode);
                        for (int i = 0; i < dl.Rows.Count; i++)
                        {
                            DataTable dataTable = oDb.ExecuteSPWithOutput("SP_SaveUserMaster", new SqlParameter("Username", oPRP.UserName),
                            new SqlParameter("UserID", oPRP.UserID),
                            new SqlParameter("EmailID", oPRP.UserEmail),
                            new SqlParameter("SiteCode", dl.Rows[i]["SITE_CODE"].ToString()),
                            new SqlParameter("GroupCode", oPRP.GroupCode),
                            new SqlParameter("EmployeeID", oPRP.EmployeeID),
                            new SqlParameter("Active", oPRP.Active),
                            new SqlParameter("COMPCODE", oPRP.CompCode),
                            new SqlParameter("CREATED_BY", oPRP.CreatedBy));

                            msg = dataTable.Rows[0].Field<string>("ERR_MSG");
                        }
                    }
                }
                return msg;
                #region
                //if (gdt.Rows.Count > 0 && gdt.Rows[0].Field<string>("ERR_MSG") == "SUCCESS")
                //{
                //    if (oPRP.LocationCode != "ALL")
                //    {
                //        DataTable dataTable = oDb.ExecuteSPWithOutput("SP_SaveUserMaster", new SqlParameter("Username", oPRP.UserName),
                //            new SqlParameter("UserID", oPRP.UserID),
                //            new SqlParameter("EmailID", oPRP.UserEmail),
                //            new SqlParameter("SiteCode", oPRP.LocationCode),
                //            new SqlParameter("GroupCode", oPRP.GroupCode),
                //            new SqlParameter("EmployeeID", oPRP.EmployeeID),
                //            new SqlParameter("Active", oPRP.Active),
                //            new SqlParameter("COMPCODE", oPRP.CompCode),
                //            new SqlParameter("CREATED_BY", oPRP.CreatedBy));

                //        msg = dataTable.Rows[0].Field<string>("ERR_MSG");
                //    }
                //    else
                //    {
                //        DataTable dl = GetSiteExceptAll();
                //        for (int i = 0; i < dl.Rows.Count; i++)
                //        {
                //            DataTable dataTable = oDb.ExecuteSPWithOutput("SP_SaveUserMaster", new SqlParameter("Username", oPRP.UserName),
                //            new SqlParameter("UserID", oPRP.UserID),
                //            new SqlParameter("EmailID", oPRP.UserEmail),
                //            new SqlParameter("SiteCode", dl.Rows[i]["SITE_CODE"].ToString()),
                //            new SqlParameter("GroupCode", oPRP.GroupCode),
                //            new SqlParameter("EmployeeID", oPRP.EmployeeID),
                //            new SqlParameter("Active", oPRP.Active),
                //            new SqlParameter("COMPCODE", oPRP.CompCode),
                //            new SqlParameter("CREATED_BY", oPRP.CreatedBy));

                //            msg = dataTable.Rows[0].Field<string>("ERR_MSG");
                //        }
                //    }
                //    return msg;
                //}
                //else
                //{
                //    return gdt.Rows[0].Field<string>("ERR_MSG");
                //}
                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Get page master details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllPages()
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT PAGE_CODE,PAGE_NAME FROM PAGE_MASTER");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetGroupwithEmployee(string _GroupCode,string _GroupName,string CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                //sbQuery.Append(" select GM.GROUP_CODE as [GROUP_CODE],GM.GROUP_NAME AS [GROUP_NAME],EMPLOYEE_CODE AS [EMPLOYEE_CODE],EMPLOYEE_NAME AS [EMPLOYEE_NAME],CASE WHEN GM.GROUP_CODE='" + _GroupCode + "' THEN 1 ELSE 0 END AS [ASSIGNED] from EMPLOYEE_MASTER EM INNER JOIN USER_ACCOUNTS UA  ON EM.EMPLOYEE_CODE = UA.EMP_ID LEFT JOIN GROUP_MASTER GM ON GM.GROUP_CODE = UA.GROUP_CODE ");
                sbQuery.Append(" select GM.GROUP_CODE as [EXIST_GROUP_CODE],GM.GROUP_NAME AS [EXIST_GROUP_NAME], '" + _GroupCode+"' as [GROUP_CODE],'"+_GroupName+"' AS [GROUP_NAME],EMPLOYEE_CODE AS [EMPLOYEE_CODE],EMPLOYEE_NAME AS [EMPLOYEE_NAME],CASE WHEN GM.GROUP_CODE='" + _GroupCode + "' THEN 1 ELSE 0 END AS [ASSIGNED] from EMPLOYEE_MASTER EM INNER JOIN USER_ACCOUNTS UA  ON EM.EMPLOYEE_CODE = UA.EMP_ID INNER JOIN GROUP_MASTER GM ON GM.GROUP_CODE = UA.GROUP_CODE WHERE GM.COMP_CODE = '"+ CompCode + "' ");
                return oDb.GetDataTable(sbQuery.ToString());
        
            }
            catch(Exception ex)
            { throw ex; }
        }
        public DataTable GetGroupwithEmployee()
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append(" select GM.GROUP_CODE,GM.GROUP_NAME,EMPLOYEE_CODE,EMPLOYEE_NAME, 0 AS ASSIGNED from EMPLOYEE_MASTER EM INNER JOIN USER_ACCOUNTS UA  ON EM.EMPLOYEE_CODE = UA.EMP_ID LEFT JOIN GROUP_MASTER GM ON GM.GROUP_CODE = UA.GROUP_CODE ");
                return oDb.GetDataTable(sbQuery.ToString());

            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool IsValidEmployee(string empid)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM EMPLOYEE_MASTER where EMPLOYEE_CODE = '"+empid+"' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }

        }
    }
}