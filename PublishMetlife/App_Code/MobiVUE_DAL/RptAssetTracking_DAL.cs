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
    /// Summary description for Asset Tracking Report Data Access Layer
    /// </summary>
    public class RptAssetTracking_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public RptAssetTracking_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~RptAssetTracking_DAL()
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
        public DataTable GetLocation(string _CompCode,string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT LOC_CODE, LOC_NAME FROM LOCATION_MASTER");
            sbQuery.Append(" WHERE PARENT_LOC_CODE LIKE '" + _ParentLocCode + "%' AND LOC_LEVEL=" + _LocLevel + "");
            sbQuery.Append(" AND COMP_CODE='" + _CompCode + "' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch category code/name details to be populated into dropdownlist.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateCategory(string _AssetType, string _ParentCategory, int _CatLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT CATEGORY_CODE,CATEGORY_NAME FROM CATEGORY_MASTER");
            sbQuery.Append(" WHERE ASSET_TYPE='" + _AssetType + "' AND PARENT_CATEGORY='" + _ParentCategory + "'");
            sbQuery.Append(" AND CATEGORY_LEVEL=" + _CatLevel + " AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateAssetMake(string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [ASSET_MAKE] FROM [ASSET_ACQUISITION] WHERE [CATEGORY_CODE] = '" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateModelName(string AssetMake, string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [MODEL_NAME] FROM [ASSET_ACQUISITION] WHERE [ASSET_MAKE]='" + AssetMake + "'");
            sbQuery.Append(" AND [CATEGORY_CODE]='" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get Asset tracking report details.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetAssetTrackingReport(RptAssetTracking_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_ACQUISITION.ASSET_CODE, ASSET_ACQUISITION.SERIAL_CODE, ASSET_ACQUISITION.ASSET_MAKE,");
            sbQuery.Append(" ASSET_ACQUISITION.MODEL_NAME, ASSET_ACQUISITION.ASSET_TYPE, CATEGORY_MASTER.CATEGORY_NAME,");
            sbQuery.Append(" LOCATION_MASTER.LOC_NAME, COMPANY_MASTER.COMP_NAME, VENDOR_MASTER.VENDOR_NAME,");
            sbQuery.Append(" CONVERT(VARCHAR,NULLIF(ASSET_ACQUISITION.INSTALLED_DATE,''), 105) AS INSTALLED_DATE, CONVERT(VARCHAR, NULLIF(ASSET_ACQUISITION.PURCHASED_DATE,''), 105)");
            sbQuery.Append(" AS PURCHASED_DATE, ASSET_ACQUISITION.PURCHASED_AMT, ASSET_ACQUISITION.PO_NUMBER, CONVERT(VARCHAR,NULLIF(ASSET_ACQUISITION.PO_DATE,''), 105) AS PO_DATE,");
            sbQuery.Append(" ASSET_ACQUISITION.INVOICE_NO, ASSET_ACQUISITION.ASSET_ALLOCATED,");
            sbQuery.Append(" ASSET_ACQUISITION.CATEGORY_CODE, ASSET_ACQUISITION.ASSET_LOCATION");
            sbQuery.Append(" FROM ASSET_ACQUISITION INNER JOIN");
            sbQuery.Append(" CATEGORY_MASTER ON ASSET_ACQUISITION.CATEGORY_CODE = CATEGORY_MASTER.CATEGORY_CODE INNER JOIN");
            sbQuery.Append(" LOCATION_MASTER ON ASSET_ACQUISITION.ASSET_LOCATION = LOCATION_MASTER.LOC_CODE LEFT JOIN");
            sbQuery.Append(" VENDOR_MASTER ON ASSET_ACQUISITION.VENDOR_CODE = VENDOR_MASTER.VENDOR_CODE INNER JOIN");
            sbQuery.Append(" COMPANY_MASTER ON ASSET_ACQUISITION.COMP_CODE = COMPANY_MASTER.COMP_CODE");
            sbQuery.Append(" WHERE ASSET_ACQUISITION.ASSET_CODE LIKE '" + oPRP.AssetCode + "' + '%'");
            sbQuery.Append(" AND ASSET_ACQUISITION.SERIAL_CODE LIKE '" + oPRP.SerialCode + "' + '%'");
            sbQuery.Append(" AND ASSET_ACQUISITION.ASSET_TYPE LIKE '" + oPRP.AssetType + "' + '%'");
            sbQuery.Append(" AND ASSET_ACQUISITION.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "' + '%'");
            sbQuery.Append(" AND ASSET_ACQUISITION.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "' + '%' AND ASSET_ACQUISITION.ASSET_MAKE LIKE '" + oPRP.AssetMake + "' + '%'");
            if (oPRP.ModelName != "")
                sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME IN (" + oPRP.ModelName + ")");
            else
                sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            sbQuery.Append(" AND ASSET_ACQUISITION.COMP_CODE = '" + oPRP.CompCode + "' AND ASSET_ACQUISITION.ASSET_APPROVED='True'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(string _AssetCode,string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SERIAL_CODE FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "' AND COMP_CODE='" + _CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}