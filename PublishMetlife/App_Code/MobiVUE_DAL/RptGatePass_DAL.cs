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
    /// Summary description for Gate Pass Report Data Access Layer.
    /// </summary>
    public class RptGatePass_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public RptGatePass_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~RptGatePass_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        //// <summary>
        /// Fetch Locations details for mapping with user id
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns>DataTable</returns>
        public DataTable GetLocation(string _CompCode,string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT Site_CODE LOC_CODE, Site_Address LOC_NAME FROM Site_MASTER");
            sbQuery.Append(" WHERE ACTIVE=1 AND SITE_CODE <> 'ALL' ");
            //sbQuery.Append(" ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get gatepass no to be populated for dropdown selection.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateGatePassNo(string _CompCode)
        {
            sbQuery=new StringBuilder();
            sbQuery.Append("SELECT DISTINCT GATEPASS_CODE FROM GATEPASS_GENERATION WHERE COMP_CODE='" + _CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get gatepass report data based of filters selected.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetGatePassReport(RptGatePass_PRP oPRP, bool bExpire)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM vw_ReportGatePass WHERE ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND");
            sbQuery.Append(" GATEPASS_CODE LIKE '" + oPRP.GatePassCode + "%' AND COMP_CODE='" + oPRP.CompCode + "'");
            if (oPRP.AllType)
                sbQuery.Append(" AND GATEPASS_TYPE IN ('RETURNABLE','NOTRETURNABLE')");
            if (oPRP.Returnable)
            {
                sbQuery.Append(" AND GATEPASS_TYPE = 'RETURNABLE'");
                if (oPRP.RtnDateExpired)
                    sbQuery.Append(" AND EXP_RTN_DATE IS NOT NULL AND EXP_RTN_DATE < GETDATE()");
            }
            if (oPRP.NonReturnable)
                sbQuery.Append(" AND GATEPASS_TYPE = 'NOTRETURNABLE'");
            if (oPRP.LiveGatePass)
                sbQuery.Append(" AND ((GATEPASS_TYPE = 'RETURNABLE' AND GATEPASS_IN_DATE IS NULL) OR (GATEPASS_TYPE = 'NOTRETURNABLE' AND GATEPASS_OUT_DATE IS NULL))");
            if (!bExpire)
            {
                sbQuery.Append(" AND GP_DATE >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
                sbQuery.Append(" AND GP_DATE <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            }
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}