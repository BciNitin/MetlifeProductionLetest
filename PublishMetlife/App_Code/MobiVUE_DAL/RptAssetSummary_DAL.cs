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
    /// Summary description for RptAssetSummary_DAL
    /// </summary>
    public class RptAssetSummary_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public RptAssetSummary_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~RptAssetSummary_DAL()
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


        public string GetCompanyCode(string LocationCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COMP_CODE FROM [LOCATION_MASTER] WHERE LOC_CODE='" + LocationCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            return dt.Rows[0]["COMP_CODE"].ToString();
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
        /// Fetch all department code/name from department master.
        /// </summary>
        /// <returns></returns>
        public DataTable GetProcess(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER WHERE COMP_CODE='" + CompCode + "' AND ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateDepartment(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT DEPT_CODE,DEPT_NAME FROM DEPARTMENT_MASTER WHERE ACTIVE = '1' AND COMP_CODE='" + CompCode + "'");
            sbQuery.Append(" ORDER BY DEPT_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public DataTable PopulateProcess(string DeptCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER WHERE DEPT_CODE='" + DeptCode + "' AND ACTIVE='1'");
            sbQuery.Append(" AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get port nos. list based on department code and process code supplied in a location.
        /// /// </summary>
        /// <param name="ProcessCode"></param>
        /// <param name="DeptCode"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulatePortNo(string ProcessCode, string DeptCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT PORT_NO FROM ASSET_ACQUISITION WHERE DEPARTMENT='" + DeptCode + "'");
            sbQuery.Append(" AND ASSET_PROCESS='" + ProcessCode + "' AND COMP_CODE='" + CompCode + "' ORDER BY PORT_NO");
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
            sbQuery.Append(" AND [CATEGORY_CODE]='" + CategoryCode + "' AND [COMP_CODE] LIKE '" + CompCode + "%'");
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
            sbQuery.Append("SELECT DISTINCT [ASSET_MAKE] FROM [ASSET_ACQUISITION] WHERE [CATEGORY_CODE] = '" + CategoryCode + "'");
            sbQuery.Append(" AND [COMP_CODE] LIKE '" + CompCode + "%'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get assets summary report (for all locations/locatio wise) based on filters provided.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetAssetSummaryReport(RptAssetSummary_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) AS ASSET_COUNT,CM.[CATEGORY_NAME],LM.[LOC_NAME],DM.[DEPT_NAME],");
		    sbQuery.Append(" PM.[PROCESS_NAME],AA.[ASSET_MAKE],AA.[MODEL_NAME],AA.[CATEGORY_CODE],AA.[ASSET_LOCATION],");
            sbQuery.Append(" AA.[DEPARTMENT],AA.[ASSET_PROCESS],AA.[PORT_NO] FROM ");
		    sbQuery.Append(" [ASSET_ACQUISITION] AA INNER JOIN [CATEGORY_MASTER] CM ");
			sbQuery.Append(" ON AA.[CATEGORY_CODE] = CM.[CATEGORY_CODE]");
		    sbQuery.Append(" INNER JOIN [LOCATION_MASTER] LM ON AA.[ASSET_LOCATION] = LM.[LOC_CODE]");
		    sbQuery.Append(" INNER JOIN [DEPARTMENT_MASTER] DM ON AA.[DEPARTMENT] = DM.[DEPT_CODE]");
		    sbQuery.Append(" INNER JOIN [PROCESS_MASTER] PM ON AA.[ASSET_PROCESS] = PM.[PROCESS_CODE]");
            sbQuery.Append(" WHERE AA.[CATEGORY_CODE] LIKE '" + oPRP.CategoryCode + "%' AND AA.[ASSET_LOCATION] LIKE '" + oPRP.AssetLocation + "%'");
            sbQuery.Append(" AND AA.[DEPARTMENT] LIKE '" + oPRP.Department + "%' AND AA.[ASSET_PROCESS] LIKE '" + oPRP.ProcessCode + "%' AND AA.[PORT_NO] LIKE '" + oPRP.PortNo + "%'");
            sbQuery.Append(" AND AA.[ASSET_MAKE] LIKE '" + oPRP.AssetMake + "%'");
            if (oPRP.ModelName != "")
                sbQuery.Append(" AND AA.[MODEL_NAME] IN (" + oPRP.ModelName + ")");
            else
                sbQuery.Append(" AND AA.[MODEL_NAME] LIKE '" + oPRP.ModelName + "%'");
		    sbQuery.Append(" AND AA.[COMP_CODE] = LM.[COMP_CODE] AND AA.[COMP_CODE] = DM.[COMP_CODE] AND AA.[COMP_CODE] = PM.[COMP_CODE]");
            sbQuery.Append(" GROUP BY CM.[CATEGORY_NAME],LM.[LOC_NAME],DM.[DEPT_NAME],PM.[PROCESS_NAME],AA.[PORT_NO],AA.[ASSET_MAKE],AA.[MODEL_NAME],");
            sbQuery.Append(" AA.[CATEGORY_CODE],AA.[ASSET_LOCATION],AA.[DEPARTMENT],AA.[ASSET_PROCESS] ORDER BY LM.[LOC_NAME],AA.[PORT_NO]");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get assets details based on filters provided.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(RptAssetSummary_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT AA.[ASSET_CODE],AA.[ASSET_ID],AA.[SERIAL_CODE],AA.[PO_NUMBER],AA.[ASSET_MAKE],AA.[MODEL_NAME],");
            sbQuery.Append("PM.[PROCESS_NAME],DM.[DEPT_NAME],LM.[LOC_NAME]");
            sbQuery.Append(" FROM [ASSET_ACQUISITION] AA INNER JOIN [PROCESS_MASTER] PM ON AA.[ASSET_PROCESS] = PM.[PROCESS_CODE]");
            sbQuery.Append(" INNER JOIN [DEPARTMENT_MASTER] DM ON AA.[DEPARTMENT] = DM.[DEPT_CODE]");
            sbQuery.Append(" INNER JOIN [LOCATION_MASTER] LM ON AA.[ASSET_LOCATION] = LM.[LOC_CODE]");
            sbQuery.Append(" WHERE [CATEGORY_CODE] = '" + oPRP.CategoryCode + "' AND [ASSET_LOCATION] LIKE '" + oPRP.AssetLocation + "%'");
            sbQuery.Append(" AND [DEPARTMENT] = '" + oPRP.Department + "' AND [ASSET_PROCESS]='" + oPRP.ProcessCode + "' AND [PORT_NO]='" + oPRP.PortNo + "'");
            sbQuery.Append(" AND [ASSET_MAKE]='" + oPRP.AssetMake + "' AND [MODEL_NAME]='" + oPRP.ModelName + "'");
            sbQuery.Append(" AND AA.[COMP_CODE] = PM.[COMP_CODE] AND AA.[COMP_CODE] = DM.[COMP_CODE] AND AA.[COMP_CODE] = LM.[COMP_CODE] ORDER BY AA.[MODEL_NAME]");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get model name wise assets details based on filters provided.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(RptAssetSummary_PRP oPRP, string ModelWise)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT AA.[ASSET_CODE],AA.[ASSET_ID],AA.[SERIAL_CODE],AA.[PO_NUMBER],AA.[ASSET_MAKE],AA.[MODEL_NAME],");
            sbQuery.Append("PM.[PROCESS_NAME],DM.[DEPT_NAME],LM.[LOC_NAME]");
            sbQuery.Append(" FROM [ASSET_ACQUISITION] AA INNER JOIN [PROCESS_MASTER] PM ON AA.[ASSET_PROCESS] = PM.[PROCESS_CODE]");
            sbQuery.Append(" INNER JOIN [DEPARTMENT_MASTER] DM ON AA.[DEPARTMENT] = DM.[DEPT_CODE]");
            sbQuery.Append(" INNER JOIN [LOCATION_MASTER] LM ON AA.[ASSET_LOCATION] = LM.[LOC_CODE]");
            sbQuery.Append(" WHERE [CATEGORY_CODE] LIKE '" + oPRP.CategoryCode + "%' AND [ASSET_LOCATION] LIKE '" + oPRP.AssetLocation + "%'");
            sbQuery.Append(" AND [DEPARTMENT] LIKE '" + oPRP.Department + "%' AND [ASSET_PROCESS] LIKE '" + oPRP.ProcessCode + "%' AND [PORT_NO] LIKE '" + oPRP.PortNo + "%'");
            sbQuery.Append(" AND [ASSET_MAKE] LIKE '" + oPRP.AssetMake + "%' AND [MODEL_NAME] IN (" + oPRP.ModelName + ")");
            sbQuery.Append(" AND AA.[COMP_CODE] = PM.[COMP_CODE] AND AA.[COMP_CODE] = DM.[COMP_CODE] AND AA.[COMP_CODE] = LM.[COMP_CODE] ORDER BY AA.[MODEL_NAME]");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}