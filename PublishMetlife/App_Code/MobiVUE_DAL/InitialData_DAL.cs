using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MobiVUE_ATS.DAL;
using System.Text;
using MobiVUE_ATS.PRP;

namespace MobiVUE_ATS.DAL
{
    /// <summary>
    /// Summary description for "Create Initial Data" Data Access Layer
    /// </summary>
    public class InitialData_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public InitialData_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~InitialData_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Save initial data for the first time through default user.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool InsertInitialData(InitialData_PRP oPRP)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("EXEC sp_InsertInitialData '" + oPRP.CompCode.Trim() + "','" + oPRP.CompName.Trim() + "',");
                sbQuery.Append("'" + oPRP.LocationCode.Trim() + "','" + oPRP.LocationName.Trim() + "',");
                sbQuery.Append("'" + oPRP.GroupCode.Trim() + "','" + oPRP.GroupName.Trim() + "',");
                sbQuery.Append("'" + oPRP.UserID.Trim() + "','" + oPRP.UserName.Trim() + "','" + oPRP.Password.Trim() + "',");
                sbQuery.Append("'" + oPRP.AdminEmail.Trim() + "','" + oPRP.TechopsEmail.Trim() + "','" + oPRP.SuperUser.Trim() + "';");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}