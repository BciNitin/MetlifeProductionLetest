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
    /// Summary description for GatePassGeneration_DAL
    /// </summary>
    public class StoreMovemementInOut_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public StoreMovemementInOut_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~StoreMovemementInOut_DAL()
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
        public DataTable GetLocation(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SITE_CODE,SITE_ADDRESS FROM SITE_MASTER WHERE ACTIVE=1 AND SITE_CODE <> 'ALL' AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetStorewithFloor(string SiteCode, string FloorCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT STORE_CODE,STORE_NAME FROM STORE_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND FLOOR_CODE = '" + FloorCode + "'  AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetFloor(string SiteCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT FLOOR_CODE,FLOOR_NAME FROM FLOOR_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetStoreMovementDetails(string Compcode, string GridType, string Location, string Floor, string Store,int interval)
        {
            return oDb.ExecuteSPWithOutput("sp_GetStoreMovementDetails", new System.Data.SqlClient.SqlParameter("Compcode", Compcode),
                new System.Data.SqlClient.SqlParameter("GridType", GridType),
                new System.Data.SqlClient.SqlParameter("Location", Location),
                new System.Data.SqlClient.SqlParameter("Floor", Floor),
                new System.Data.SqlClient.SqlParameter("Store", Store),
                new System.Data.SqlClient.SqlParameter("interval", interval));
        }
        public DataTable GetKioskMovementDetails(string Compcode, string Type, string Location, int interval)
        {
            return oDb.ExecuteSPWithOutput("sp_GetKioskMovementDetails", new System.Data.SqlClient.SqlParameter("Compcode", Compcode),
                new System.Data.SqlClient.SqlParameter("Type", Type),
                new System.Data.SqlClient.SqlParameter("Location", Location),
                new System.Data.SqlClient.SqlParameter("interval", interval));
        }
        public string RemoveAssetTag(int ID, string AssetTag, string Status)
        {
            try
            {
                DataTable dt = oDb.ExecuteSPWithOutput("USP_MarkDeletedForAssetMovement",
                    new SqlParameter("@ID", ID),
                    new SqlParameter("@AssetTag", AssetTag),
                    new SqlParameter("@Status", Status));
                return Convert.ToString(dt.Rows[0][0]);
            }
            catch (Exception ex)
            { return ex.Message; }
        }
    }
}