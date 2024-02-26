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
    /// Summary description for UserMaster_DAL
    /// </summary>
    public class UserMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public UserMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~UserMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Checks User Login Credentials When User Logins.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns>bValid</returns>
        public bool ValidateUserLogin(UserMaster_PRP oPRP)
        {
            try
            {
                bool bValid = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM USER_ACCOUNTS WHERE USER_ID='" + oPRP.UserID.Trim().Replace("'", "''") + "' AND ");
                sbQuery.Append("PASSWORD='" + oPRP.UserPswd.Trim().Replace("'", "''") + "'  AND ACTIVE=1");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                {
                    bValid = true;
                }
                return bValid;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Update session state into user accounts table to avoid multiple 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CompCode"></param>
        /// <param name="SessionID"></param>
        public bool SaveLoggedInSessionID(string UserID, string CompCode, string SessionID)
        {
            bool bResp=false;
            sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE USER_ACCOUNTS SET USER_SESSION_ID='" + SessionID + "' WHERE USER_ID='" + UserID + "'");
            sbQuery.Append("  AND USER_SESSION_ID IS NULL");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResp = true;
            return bResp;
        }

        /// <summary>
        /// Update session state as null into user accounts table when user logs out.
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CompCode"></param>
        public void UpdateLoggedInSessionID(string UserID, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE USER_ACCOUNTS SET USER_SESSION_ID=NULL WHERE USER_ID='" + UserID + "' ");
            oDb.ExecuteQuery(sbQuery.ToString());
        }

        /// <summary>
        /// Update session state as null into user accounts table when user logs out.
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CompCode"></param>
        public string GetLoggedInSessionID(string UserID, string CompCode)
        {
            string SessionID = "";
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT USER_SESSION_ID FROM USER_ACCOUNTS WHERE USER_ID='" + UserID + "' ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                SessionID = dt.Rows[0]["USER_SESSION_ID"].ToString();
            }
            return SessionID;
        }

        /// <summary>
        /// Fetches User Rights For View/Save/Edit/Delete/Export Operations Throughout The Application Scope.
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetGroupRights(string _UserID, string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM vw_UserGroupRights WHERE USER_ID= '" + _UserID.Trim().Replace("'", "''") + "' AND COMP_CODE = '" + _CompCode +"'");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save/Update User Details.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns>bResult</returns>
        public bool SaveUpdateUser(string OpType, UserMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;                
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateUser(oPRP.UserID.Trim(), oPRP.CompCode))
                    {
                        //Add new User...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO USER_ACCOUNTS (USER_ID,USER_NAME,PASSWORD,USER_EMAIL,COMP_CODE,LOCATION_CODE,GROUP_CODE,ACTIVE,CREATED_BY,CREATED_ON,TECHOPS_EMAIL) ");
                        sbQuery.Append(" VALUES ");
                        sbQuery.Append("('" + oPRP.UserID.Trim().Replace("'", "''") + "','" + oPRP.UserName.Trim().Replace("'", "''") + "','" + oPRP.UserPswd.Trim().Replace("'", "''") + "','" + oPRP.UserEmail + "',");
                        sbQuery.Append(" '" + oPRP.CompCode + "','" + oPRP.LocationCode + "', '" + oPRP.GroupCode + "', '" + oPRP.Active + "','" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.TechOpsEmail + "')");
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update User Information...
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE USER_ACCOUNTS SET USER_NAME='" + oPRP.UserName.Trim().Replace("'", "''") + "',");
                    sbQuery.Append(" ACTIVE='" + oPRP.Active + "',USER_EMAIL='" + oPRP.UserEmail.Trim().Replace("'", "''") + "',"); //, PASSWORD='" + oPRP.UserPswd.Trim().Replace("'", "''") + "'
                    sbQuery.Append(" LOCATION_CODE='" + oPRP.LocationCode.Trim().Replace("'", "''") + "', COMP_CODE='" + oPRP.LocationCode.Trim().Replace("'", "''") + "', GROUP_CODE='" + oPRP.GroupCode.Trim() + "',");
                    sbQuery.Append(" MODIFIED_BY='" + oPRP.ModifiedBy + "', MODIFIED_ON = GETDATE(),TECHOPS_EMAIL='" + oPRP.TechOpsEmail.Trim() + "'");
                    sbQuery.Append(" WHERE USER_ID='" + oPRP.UserID.Trim().Replace("'", "''") + "'");
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
        /// User Password Can Be Changed Through This Function
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns>bResult</returns>
        public bool ChangePassword(UserMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE USER_ACCOUNTS SET PASSWORD='" + oPRP.NewPswd.Trim().Replace("'", "''") + "'");
                sbQuery.Append(" WHERE USER_ID='" + oPRP.UserID.Trim().Replace("'", "''") + "' AND PASSWORD='" + oPRP.UserPswd.Trim().Replace("'", "''") + "'");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Checks For Duplicate User ID
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>bDup</returns>
        private bool CheckDuplicateUser(string _UserID,string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM USER_ACCOUNTS WHERE USER_ID = '" + _UserID.Trim().Replace("'", "''") + "'");
               // sbQuery.Append(" AND COMP_CODE='" + _CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Fetches User Details To Be Populated In GridView
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetUserDetails(string CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT UA.USER_ID,UA.USER_NAME,UA.USER_EMAIL,NULL AS COMP_NAME,NULL AS LOC_NAME,GM.GROUP_NAME,GM.GROUP_CODE,UA.ACTIVE,UA.LOCATION_CODE,");
                sbQuery.Append(" UA.TECHOPS_EMAIL FROM USER_ACCOUNTS UA INNER JOIN GROUP_MASTER GM");
                sbQuery.Append(" ON UA.GROUP_CODE = GM.GROUP_CODE ");
               // sbQuery.Append(" INNER JOIN LOCATION_MASTER LM ON UA.LOCATION_CODE = LM.LOC_CODE");
              //  sbQuery.Append(" WHERE UA.COMP_CODE = '" + CompCode + "'");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Delete A Particular User
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns>bResult</returns>
        public bool DeleteUser(UserMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM USER_ACCOUNTS WHERE USER_ID = '" + oPRP.UserID.Trim().Replace("'", "''") + "'");
                sbQuery.Append(" AND COMP_CODE='" + oPRP.CompCode + "' AND GROUP_CODE != 'SYSADMIN'");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Gets Existing Password For An Existing User
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_UserEmail"></param>
        /// <returns>_UserPswd</returns>
        public string GetUserPassword(string _UserID,string _CompCode, string _UserEmail)
        {
            try
            {
                string _UserPswd = "";
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT PASSWORD FROM dbo.USER_ACCOUNTS");
                sbQuery.Append(" WHERE USER_ID='" + _UserID + "' AND USER_EMAIL='" + _UserEmail + "' AND COMP_CODE='" + _CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                {
                    _UserPswd = dt.Rows[0][0].ToString();
                }
                else                
                    _UserPswd = "NOT_FOUND";
                return _UserPswd;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Fetch Locations details for mapping with user id
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns>DataTable</returns>
        public DataTable GetLocation(string _CompCode, string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT Site_Code LOC_CODE, Site_Address LOC_NAME FROM Site_MASTER");
            sbQuery.Append(" WHERE ACTIVE=1  AND SITE_CODE <> 'ALL' ");
            //sbQuery.Append(" AND COMP_CODE='" + _CompCode + "' AND ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch Group details for mapping with user id
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetGroup(string CompanyCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GROUP_CODE,GROUP_NAME FROM GROUP_MASTER WHERE  ACTIVE = 1;");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetches Company details
        /// </summary>
        /// <returns></returns>
        public DataTable GetCompany()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  Site_CODE AS  COMP_CODE, SITE_ADDRESS COMP_NAME FROM SITE_MASTER WHERE ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetCompanyLocation()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  Site_CODE AS  COMP_CODE, SITE_ADDRESS COMP_NAME FROM SITE_MASTER WHERE ACTIVE='1' and SITE_ADDRESS <>'ALL' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable Uploaduser(string emp,string location,string Username, string userID, string userEmail,Boolean Status, string CreatedBy)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = "USP_USERMaster";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmpID", emp);
                command.Parameters.AddWithValue("@location", location);
                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@userEmail", userEmail);
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                return oDb.GetDataTable(command);
            }
           
        }

        /// <summary>
        /// Fetches Company details for mapping with user id
        /// </summary>
        /// <returns></returns>
        //public DataTable GetCompany()
        //{
        //    sbQuery = new StringBuilder();
        //    sbQuery.Append("SELECT COMP_CODE,COMP_NAME FROM COMPANY_MASTER WHERE ACTIVE='1' AND COMP_OWNER=0");
        //    return oDb.GetDataTable(sbQuery.ToString());
        //}

        /// <summary>
        /// Get logged in user email id.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string GetUserEmailID(string UserID,string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT TECHOPS_EMAIL FROM USER_ACCOUNTS WHERE USER_ID='" + UserID + "' AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString()).Rows[0]["TECHOPS_EMAIL"].ToString();
        }

        public string GetUserLocation(string UserID)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT LOCATION_CODE FROM USER_ACCOUNTS WHERE USER_ID='" + UserID + "' ");
            return oDb.GetDataTable(sbQuery.ToString()).Rows[0]["LOCATION_CODE"].ToString();
        }

        public string GetUserLoginEmail(string UserID)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT USER_EMAIL FROM USER_ACCOUNTS WHERE USER_ID='" + UserID + "' ");
            return oDb.GetDataTable(sbQuery.ToString()).Rows[0]["USER_EMAIL"].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public string GetLogInUserGroup(string UserID, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GROUP_CODE FROM USER_ACCOUNTS WHERE USER_ID='" + UserID + "' ");
            return oDb.GetDataTable(sbQuery.ToString()).Rows[0]["GROUP_CODE"].ToString();
        }
    }
}