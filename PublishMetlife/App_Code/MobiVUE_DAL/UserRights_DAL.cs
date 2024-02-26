using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MobiVUE_ATS.DAL
{

    /// <summary>
    /// Summary description for UserRights_DAL
    /// </summary>
    public class UserRights_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public UserRights_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~UserRights_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        public DataTable GetSite()
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT SITE_CODE, SITE_ADDRESS from SITE_MASTER WHERE ACTIVE = 1");

                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetSiteExceptAll()
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT SITE_CODE, SITE_ADDRESS from SITE_MASTER WHERE ACTIVE = 1 AND SITE_CODE = 'ALL' ");

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

        public DataTable GetGroupDetails(string GroupCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GROUP_CODE, GROUP_NAME, REMARKS FROM GROUP_MASTER WHERE GROUP_CODE ='" + GroupCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetGroupRights(string GroupCode, string CompCode)
        {
            return oDb.ExecuteSPWithOutput("SP_GET_GROUP_RIGHTS", new SqlParameter("GroupCode", GroupCode), new SqlParameter("Comp_Code", CompCode));
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

        public string SaveUpdateGroupRights(DataTable dt, UserMaster_PRP oPRP)
        {
            try
            {
                string msg = "";
                DataTable gdt = oDb.ExecuteSPWithOutput("SP_SaveGroupRights", new SqlParameter("GroupRights", dt),
                    new SqlParameter("Group_Code", oPRP.GroupCode),
                    new SqlParameter("Group_Name", oPRP.GroupName),
                    new SqlParameter("Comp_Code", oPRP.CompCode),
                    new SqlParameter("Remarks", oPRP.GroupRemarks),
                    new SqlParameter("CREATED_BY", oPRP.CreatedBy));

                if (gdt.Rows.Count > 0 && gdt.Rows[0].Field<string>("ERR_MSG") == "SUCCESS")
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
                        DataTable dl = GetSiteExceptAll();
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
                    return msg;
                }
                else
                {
                    return gdt.Rows[0].Field<string>("ERR_MSG");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}


//server