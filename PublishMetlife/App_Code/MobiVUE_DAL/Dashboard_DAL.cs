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
    /// Summary description for Dashboard_DAL
    /// </summary>
    public class Dashboard_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public Dashboard_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }

        public Dashboard_DAL()
        {
        }

        ~Dashboard_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        public void Transaction(string Process, string DatabaseType)
        {
            switch (Process)
            {
                case "BEGIN":
                    if (!oDb.IsConnected) oDb.Connect(DatabaseType);
                    oDb.BeginTrans();
                    break;
                case "COMMIT":
                    oDb.CommitTrans();
                    break;
                case "ROLLBACK":
                    oDb.RollBack();
                    break;
            }
        }
        public DataTable GetAssetMovement(string GridType)
        {
            sbQuery = new StringBuilder();
            if(GridType == "IN")
                sbQuery.Append(" SELECT [Id],[EmployeeTag],[AssetTag],[Status],[ScannedDateTime],[Location] FROM AssetMovement WHERE ScannedDateTime >= DATEADD(SECOND,-10,GETDATE()) and IsDeleted=0 AND Status='IN' ");
            else
                sbQuery.Append(" SELECT [Id],[EmployeeTag],[AssetTag],[Status],[ScannedDateTime],[Location] FROM AssetMovement WHERE ScannedDateTime >= DATEADD(SECOND,-10,GETDATE()) and IsDeleted=0 AND Status IN('OUT','INVALID') ");

            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetLocationWithcount(string Compcode,string QueryType)
        {
            return oDb.ExecuteSPWithOutput("sp_GetAssetLocationWithCount", new System.Data.SqlClient.SqlParameter("Compcode", Compcode),
                new System.Data.SqlClient.SqlParameter("QueryType", QueryType));
        }

    }
}