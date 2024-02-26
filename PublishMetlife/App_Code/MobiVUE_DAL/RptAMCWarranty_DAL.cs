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
    /// Summary description for RptAMCWarranty_DAL
    /// </summary>
    public class RptAMCWarranty_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public RptAMCWarranty_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~RptAMCWarranty_DAL()
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
        public DataTable PopulateModelName(string AssetMake, string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [MODEL_NAME] FROM [ASSET_ACQUISITION] WHERE [ASSET_MAKE]='" + AssetMake + "'");
            sbQuery.Append(" AND [CATEGORY_CODE]='" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
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
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetAssetHistoryReport(RptAMCWarranty_PRP oPRP)
        {
            string AgeType = "";
            if (oPRP.AgeType == "YEAR")
                AgeType = "YY";
            else if (oPRP.AgeType == "MONTH")
                AgeType = "MM";
            else if (oPRP.AgeType == "DAY")
                AgeType = "DD";
            sbQuery = new StringBuilder();
            if (AgeType == "YY" || AgeType == "MM" || AgeType == "DD")
            {
                sbQuery.Append("SELECT AA.[ASSET_CODE],AA.[ASSET_ID],AA.[SERIAL_CODE],AA.[ASSET_MAKE],AA.[MODEL_NAME],");
				sbQuery.Append("CONVERT(VARCHAR,NULLIF(AA.[PURCHASED_DATE],''),105) AS PURCHASED_DATE,AA.[AMC_WARRANTY],");
				sbQuery.Append("CONVERT(VARCHAR,NULLIF(AA.[AMC_WARRANTY_START_DATE],''),105) AS AMC_WARRANTY_START_DATE,");
				sbQuery.Append("CONVERT(VARCHAR,NULLIF(AA.[AMC_WARRANTY_END_DATE],''),105) AS AMC_WARRANTY_END_DATE,");
                sbQuery.Append("PM.[PROCESS_NAME],DM.[DEPT_NAME],LM.[LOC_NAME],DATEDIFF(" + AgeType + ",NULLIF(AA.[PURCHASED_DATE],''),GETDATE()) AS NO_OF_YRS_OLD");
				sbQuery.Append(" FROM [ASSET_ACQUISITION] AA ");
				sbQuery.Append(" INNER JOIN [PROCESS_MASTER] PM ON AA.[ASSET_PROCESS] = PM.[PROCESS_CODE]");
				sbQuery.Append(" INNER JOIN [DEPARTMENT_MASTER] DM ON AA.[DEPARTMENT] = DM.[DEPT_CODE]");
				sbQuery.Append(" INNER JOIN [LOCATION_MASTER] LM ON AA.[ASSET_LOCATION] = LM.[LOC_CODE]");
				sbQuery.Append(" WHERE AA.[COMP_CODE] = PM.[COMP_CODE] AND AA.[COMP_CODE] = DM.[COMP_CODE] AND AA.[COMP_CODE] = LM.[COMP_CODE]");
                sbQuery.Append(" AND AA.[ASSET_TYPE] = '" + oPRP.AssetType + "'  AND AA.[AMC_WARRANTY] LIKE '" + oPRP.AMC_Warranty + "%'");
                sbQuery.Append(" AND AA.[ASSET_MAKE] LIKE '" + oPRP.AssetMake + "%'");
                if (oPRP.ModelName != "")
                    sbQuery.Append(" AND AA.[MODEL_NAME] IN (" + oPRP.ModelName + ")");
                else
                    sbQuery.Append(" AND AA.[MODEL_NAME] LIKE '" + oPRP.ModelName + "%'");
                sbQuery.Append(" AND AA.[CATEGORY_CODE] LIKE '" + oPRP.CategoryCode + "%' AND AA.[ASSET_LOCATION] LIKE '" + oPRP.AssetLocation + "%'");
                sbQuery.Append(" AND AA.[PURCHASED_DATE] >= '" + oPRP.PurchaseDateFrom + "' AND AA.[PURCHASED_DATE] <= '" + oPRP.PurchaseDateTo + "'");
                if (oPRP.AgeCriteria == "GTET")
                    sbQuery.Append(" AND DATEDIFF(" + AgeType + ",NULLIF(AA.[PURCHASED_DATE],''),GETDATE()) >= " + oPRP.NoOfYearsOld + "");
                else if (oPRP.AgeCriteria == "LTET")
                    sbQuery.Append(" AND DATEDIFF(" + AgeType + ",NULLIF(AA.[PURCHASED_DATE],''),GETDATE()) < " + oPRP.NoOfYearsOld + "");
                sbQuery.Append(" ORDER BY NO_OF_YRS_OLD");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            else
            {
                sbQuery.Append(" SELECT AA.[ASSET_CODE],AA.[ASSET_ID],AA.[SERIAL_CODE],AA.[ASSET_MAKE],AA.[MODEL_NAME],");
				sbQuery.Append(" CONVERT(VARCHAR,NULLIF(AA.[PURCHASED_DATE],''),105) AS PURCHASED_DATE,AA.[AMC_WARRANTY],");
				sbQuery.Append(" CONVERT(VARCHAR,NULLIF(AA.[AMC_WARRANTY_START_DATE],''),105) AS AMC_WARRANTY_START_DATE,");
				sbQuery.Append(" CONVERT(VARCHAR,NULLIF(AA.[AMC_WARRANTY_END_DATE],''),105) AS AMC_WARRANTY_END_DATE,");
				sbQuery.Append(" PM.[PROCESS_NAME],DM.[DEPT_NAME],LM.[LOC_NAME],DATEDIFF(YY,NULLIF(AA.[PURCHASED_DATE],''),GETDATE()) AS NO_OF_YRS_OLD");
				sbQuery.Append(" FROM [ASSET_ACQUISITION] AA");
				sbQuery.Append(" INNER JOIN [PROCESS_MASTER] PM ON AA.[ASSET_PROCESS] = PM.[PROCESS_CODE]");
				sbQuery.Append(" INNER JOIN [DEPARTMENT_MASTER] DM ON AA.[DEPARTMENT] = DM.[DEPT_CODE]");
				sbQuery.Append(" INNER JOIN [LOCATION_MASTER] LM ON AA.[ASSET_LOCATION] = LM.[LOC_CODE]");
				sbQuery.Append(" WHERE AA.[COMP_CODE] = PM.[COMP_CODE] AND AA.[COMP_CODE] = DM.[COMP_CODE] AND AA.[COMP_CODE] = LM.[COMP_CODE]");
                sbQuery.Append(" AND AA.[ASSET_TYPE] = '" + oPRP.AssetType + "'  AND AA.[AMC_WARRANTY] LIKE '" + oPRP.AMC_Warranty + "%'");
                sbQuery.Append(" AND AA.[ASSET_MAKE] LIKE '" + oPRP.AssetMake + "%'");
                if (oPRP.ModelName != "")
                    sbQuery.Append(" AND AA.[MODEL_NAME] IN (" + oPRP.ModelName + ")");
                else
                    sbQuery.Append(" AND AA.[MODEL_NAME] LIKE '" + oPRP.ModelName + "%'");
                sbQuery.Append(" AND AA.[CATEGORY_CODE] LIKE '" + oPRP.CategoryCode + "%' AND AA.[ASSET_LOCATION] LIKE '" + oPRP.AssetLocation + "%'");
                sbQuery.Append(" AND AA.[PURCHASED_DATE] >= '" + oPRP.PurchaseDateFrom + "' AND AA.[PURCHASED_DATE] <= '" + oPRP.PurchaseDateTo + "'");
                if (oPRP.AgeCriteria == "GTET")
                    sbQuery.Append(" AND DATEDIFF(YY,AA.[PURCHASED_DATE],GETDATE()) >= " + oPRP.NoOfYearsOld + "");
                else if (oPRP.AgeCriteria == "LTET")
                    sbQuery.Append(" AND DATEDIFF(YY,AA.[PURCHASED_DATE],GETDATE()) < " + oPRP.NoOfYearsOld + "");
                sbQuery.Append(" ORDER BY NO_OF_YRS_OLD");
            }
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}