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
    /// Summary description for RptAssetDashBoard_DAL
    /// </summary>
    public class RptAssetDashBoard_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public RptAssetDashBoard_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~RptAssetDashBoard_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Fetch Locations details for mapping with user id
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns>DataTable</returns>
        public DataTable GetLocation(string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT LOC_CODE, LOC_NAME FROM LOCATION_MASTER");
            sbQuery.Append(" WHERE PARENT_LOC_CODE='" + _ParentLocCode + "' AND LOC_LEVEL=" + _LocLevel + "");
            sbQuery.Append(" AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get Dashboard details for assets of each category.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetDashBoardDetails(RptAssetDashBoard_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("EXEC sp_ReportAssetDashBoard '" + oPRP.AssetLocation + "','" + oPRP.CompanyCode + "','" + oPRP.Type + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}