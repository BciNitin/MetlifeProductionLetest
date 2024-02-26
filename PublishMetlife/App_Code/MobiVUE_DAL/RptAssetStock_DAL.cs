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
    /// Summary description for RptAssetStock_DAL
    /// </summary>
    public class RptAssetStock_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public RptAssetStock_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~RptAssetStock_DAL()
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
            sbQuery.Append("SELECT  Distinct ASSET_TYPE CATEGORY_CODE, ASSET_TYPE CATEGORY_NAME FROM  ASSET_ACQUISITION ");
            //sbQuery.Append(" WHERE ASSET_TYPE='" + _AssetType + "' AND PARENT_CATEGORY='" + _ParentCategory + "'");
            //sbQuery.Append(" AND CATEGORY_LEVEL=" + _CatLevel + " AND ACTIVE=1");
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
            sbQuery.Append("SELECT DISTINCT [ASSET_MAKE] FROM [ASSET_ACQUISITION] WHERE [ASSET_TYPE] = '" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
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
            sbQuery.Append(" AND [ASSET_TYPE]='" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset stock report details.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        //public DataTable GetAssetStockReport(RptAssetStock_PRP oPRP)
        //{
        //    sbQuery = new StringBuilder();
        //    sbQuery.Append("SELECT COUNT(A.TAG_ID) AS TOTAL,A.ASSET_CODE,A.CATEGORY_NAME,A.TAG_ID,A.LOC_NAME,A.ASSET_MAKE,A.MODEL_NAME,A.COMP_CODE FROM");
        //    sbQuery.Append("(SELECT ASSET_ACQUISITION.ASSET_CODE, ASSET_ACQUISITION.TAG_ID, A ASSET_ACQUISITION.ASSET_MAKE,");
        //    sbQuery.Append(" ASSET_ACQUISITION.MODEL_NAME, ASSET_ACQUISITION.COMP_CODE, CATEGORY_MASTER.CATEGORY_NAME,");
        //    sbQuery.Append(" LOCATION_MASTER.LOC_NAME, COMPANY_MASTER.COMP_NAME, VENDOR_MASTER.VENDOR_NAME, CONVERT(VARCHAR,");
        //    sbQuery.Append(" NULLIF(ASSET_ACQUISITION.INSTALLED_DATE,''), 105) AS INSTALLED_DATE, CONVERT(VARCHAR, NULLIF(ASSET_ACQUISITION.PURCHASED_DATE,''), 105)");
        //    sbQuery.Append(" AS PURCHASED_DATE,  ASSET_ACQUISITION.PO_NUMBER, CONVERT(VARCHAR,");
        //    sbQuery.Append(" ASSET_ACQUISITION.PO_DATE, 105) AS PO_DATE, ASSET_ACQUISITION.INVOICE_NO, ASSET_ACQUISITION.ASSET_ALLOCATED,");
        //    sbQuery.Append(" ASSET_ACQUISITION.CATEGORY_CODE, ASSET_ACQUISITION.ASSET_LOCATION,ASSET_ACQUISITION.ASSET_PROCESS,ASSET_ACQUISITION.SOLD_SCRAPPED_STATUS FROM ASSET_ACQUISITION INNER JOIN");
        //    sbQuery.Append(" CATEGORY_MASTER ON ASSET_ACQUISITION.CATEGORY_CODE = CATEGORY_MASTER.CATEGORY_CODE INNER JOIN");
        //    sbQuery.Append(" LOCATION_MASTER ON ASSET_ACQUISITION.ASSET_LOCATION = LOCATION_MASTER.LOC_CODE LEFT JOIN");
        //    sbQuery.Append(" VENDOR_MASTER ON ASSET_ACQUISITION.VENDOR_CODE = VENDOR_MASTER.VENDOR_CODE INNER JOIN");
        //    sbQuery.Append(" COMPANY_MASTER ON ASSET_ACQUISITION.COMP_CODE = COMPANY_MASTER.COMP_CODE) A");
        //    sbQuery.Append(" WHERE  A.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "%' AND A.ASSET_MAKE LIKE '" + oPRP.AssetMake + "%' AND A.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "%' AND A.COMP_CODE = '" + oPRP.CompCode + "'");
        //    if (oPRP.ModelName != "")
        //        sbQuery.Append(" AND A.MODEL_NAME IN (" + oPRP.ModelName + ")");
        //    else
        //        sbQuery.Append(" AND A.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
        //    sbQuery.Append(" AND A.ASSET_APPROVED = 'True' AND A.ASSET_PROCESS = 'STOCK' AND A.SOLD_SCRAPPED_STATUS IS NULL");
        //    sbQuery.Append(" GROUP BY A.ASSET_CODE,A.CATEGORY_NAME,A.TAG_ID,,A.LOC_NAME,A.LOC_NAME,A.ASSET_MAKE,A.MODEL_NAME,A.COMP_CODE ORDER BY A.MODEL_NAME");
        //    return oDb.GetDataTable(sbQuery.ToString());
        //}




        public DataTable GetAssetSTOCKITReport(RptAssetStock_PRP oPRP)
        {
            sbQuery = new StringBuilder();

            sbQuery = new StringBuilder();
            sbQuery.AppendLine(" SELECT   [ASSET_LOCATION] AS [Location],[Floor] [Floor Name],[ASSET_TYPE],[ASSET_MAKE],[MODEL_NAME],[ASSET_CODE] AS [Serial No],[Tag_ID] AS [RFID Tag],[ASSET_ID] AS [Asset Tag],");
            sbQuery.AppendLine(" [Status] [Asset Status],[ASSET_SUB_STATUS] [Sub Status],[STORE] [Store Name],[SERVICE_NOW_TICKET_NO] [ServiceNow Ticket no],[Allocation_Date] [Allocation Date],[ReturnDate] [Return Date], [EMP_TAG],[EMP_ID][Employee ID],[EMP_NAME] [Employee Name],");
            sbQuery.AppendLine(" [EMP_FLOOR] [Floor],[SEAT_NO][Seat No],[Designation],[PROCESS_NAME] [Process Name],[SUB_LOB] [Sub LOB],[LOB], ");
            sbQuery.AppendLine(" [ASSET_PROCESSOR][Processor],[ASSET_HDD][HDD],[ASSET_RAM][RAM],[AMC_WARRANTY] [Warranty Status] FROM ASSET_ACQUISITION WHERE Status='STOCK' and ( ASSET_FAR_TAG is null or ASSET_FAR_TAG='')");
            if (oPRP.CategoryCode != "")
                sbQuery.Append("  AND ASSET_ACQUISITION.ASSET_TYPE LIKE '" + oPRP.CategoryCode + "' + '%'");
            if (oPRP.AssetMake != "")
                sbQuery.AppendLine(" AND ASSET_ACQUISITION.ASSET_MAKE LIKE '" + oPRP.AssetMake + "' + '%'");
            if (oPRP.ModelName != "")
            {
                if (oPRP.ModelName.Contains(','))
                    sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME IN (" + oPRP.ModelName + ")");
                else
                    sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            }
            //sbQuery.Append(" AND Emp_ID LIKE '" + oPRP.EmpCode + "' + '%'");
            //if (oPRP.FromDate != "")
            //    sbQuery.Append(" AND Allocation_Date >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
            //if (oPRP.ToDate != "")
            //    sbQuery.Append(" AND Allocation_Date <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            sbQuery.Append(" AND ASSET_LOCATION = '" + oPRP.CompCode + "'");
            //  sbQuery.AppendLine(" [REMARKS],[CREATED_BY],[CREATED_ON] )");
            //if (oPRP.AllocationType == )
            //{
            //sbQuery = new StringBuilder();
            //sbQuery.Append("SELECT DEPARTMENT_MASTER.DEPT_NAME, ASSET_ALLOCATION.ASSET_CODE,ASSET_ACQUISITION.SERIAL_CODE,");
            //sbQuery.Append(" CONVERT(VARCHAR,ASSET_ALLOCATION.ASSET_ALLOCATION_DATE, 105) AS ASSET_ALLOCATION_DATE, ASSET_ACQUISITION.ASSET_ALLOCATED,");
            //sbQuery.Append(" PROCESS_MASTER.PROCESS_NAME, EMPLOYEE_MASTER.EMPLOYEE_NAME, LOCATION_MASTER.LOC_NAME, CONVERT(VARCHAR,NULLIF(ASSET_ALLOCATION.EXPECTED_RTN_DATE,''), 105) AS EXPECTED_RTN_DATE,");
            //sbQuery.Append(" CONVERT(VARCHAR, NULLIF(ASSET_ALLOCATION.ACTUAL_RTN_DATE,''),105) AS ACTUAL_RTN_DATE,");
            //sbQuery.Append(" CATEGORY_MASTER.CATEGORY_NAME, CATEGORY_MASTER.ASSET_TYPE,");
            //sbQuery.Append(" ASSET_ALLOCATION.ASSET_ALLOCATED_EMP, ASSET_ALLOCATION.ALLOCATED_PROCESS,");
            //sbQuery.Append(" ASSET_ALLOCATION.ASSET_LOCATION, CATEGORY_MASTER.CATEGORY_CODE,");
            //sbQuery.Append(" ASSET_ALLOCATION.ASSET_ALLOCATION_DATE,ASSET_ALLOCATION.PORT_NO,ASSET_ALLOCATION.VLAN FROM ASSET_ALLOCATION INNER JOIN");
            //sbQuery.Append(" PROCESS_MASTER ON ASSET_ALLOCATION.ALLOCATED_PROCESS = PROCESS_MASTER.PROCESS_CODE AND");
            //sbQuery.Append(" ASSET_ALLOCATION.COMP_CODE = PROCESS_MASTER.COMP_CODE INNER JOIN");
            //sbQuery.Append(" LOCATION_MASTER ON ASSET_ALLOCATION.ASSET_LOCATION = LOCATION_MASTER.LOC_CODE INNER JOIN");
            //sbQuery.Append(" EMPLOYEE_MASTER ON ASSET_ALLOCATION.ALLOCATED_EMP_ID = EMPLOYEE_MASTER.EMPLOYEE_CODE AND ");
            //sbQuery.Append(" PROCESS_MASTER.COMP_CODE = EMPLOYEE_MASTER.COMP_CODE AND");
            //sbQuery.Append(" LOCATION_MASTER.COMP_CODE = EMPLOYEE_MASTER.COMP_CODE INNER JOIN");
            //sbQuery.Append(" ASSET_ACQUISITION ON ASSET_ALLOCATION.ASSET_CODE = ASSET_ACQUISITION.ASSET_CODE INNER JOIN");
            //sbQuery.Append(" CATEGORY_MASTER ON ASSET_ACQUISITION.CATEGORY_CODE = CATEGORY_MASTER.CATEGORY_CODE INNER JOIN");
            //sbQuery.Append(" DEPARTMENT_MASTER ON ASSET_ALLOCATION.ALLOCATED_DEPARTMENT = DEPARTMENT_MASTER.DEPT_CODE AND ASSET_ALLOCATION.COMP_CODE = DEPARTMENT_MASTER.COMP_CODE");
            //sbQuery.Append(" WHERE ASSET_ACQUISITION.ASSET_TYPE LIKE '" + oPRP.AssetType + "' + '%' AND ASSET_ACQUISITION.ASSET_MAKE LIKE '" + oPRP.AssetMake + "' + '%'");
            //sbQuery.Append(" AND CATEGORY_MASTER.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "' + '%' AND ASSET_ALLOCATION.ALLOCATED_PROCESS LIKE '" + oPRP.ProcessCode + "' + '%'");
            //sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "' + '%'");
            //if (oPRP.ModelName != "")
            //    sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME IN (" + oPRP.ModelName + ")");
            //else
            //    sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            //sbQuery.Append(" AND ASSET_ALLOCATION.ALLOCATED_EMP_ID LIKE '" + oPRP.EmpCode + "' + '%'");
            //sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
            //sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            //sbQuery.Append(" AND ASSET_ALLOCATION.COMP_CODE = '" + oPRP.CompCode + "' AND ASSET_ALLOCATION.ACTUAL_RTN_DATE IS NULL");
            //}
            //else if (oPRP.AllocationType == "RETURNED")
            //{
            //    sbQuery = new StringBuilder();
            //    sbQuery.Append("SELECT DEPARTMENT_MASTER.DEPT_NAME, ASSET_ALLOCATION.ASSET_CODE,ASSET_ACQUISITION.SERIAL_CODE,");
            //    sbQuery.Append(" CONVERT(VARCHAR,ASSET_ALLOCATION.ASSET_ALLOCATION_DATE, 105) AS ASSET_ALLOCATION_DATE, ASSET_ACQUISITION.ASSET_ALLOCATED,");
            //    sbQuery.Append(" PROCESS_MASTER.PROCESS_NAME, EMPLOYEE_MASTER.EMPLOYEE_NAME, LOCATION_MASTER.LOC_NAME,");
            //    sbQuery.Append(" CONVERT(VARCHAR,NULLIF(ASSET_ALLOCATION.EXPECTED_RTN_DATE,''), 105) AS EXPECTED_RTN_DATE, CONVERT(VARCHAR,NULLIF(ASSET_ALLOCATION.ACTUAL_RTN_DATE,''),105) AS ACTUAL_RTN_DATE,");
            //    sbQuery.Append(" CATEGORY_MASTER.CATEGORY_NAME, CATEGORY_MASTER.ASSET_TYPE,");
            //    sbQuery.Append(" ASSET_ALLOCATION.ASSET_ALLOCATED_EMP, ASSET_ALLOCATION.ALLOCATED_PROCESS,");
            //    sbQuery.Append(" ASSET_ALLOCATION.ASSET_LOCATION, CATEGORY_MASTER.CATEGORY_CODE,");
            //    sbQuery.Append(" ASSET_ALLOCATION.ASSET_ALLOCATION_DATE,ASSET_ALLOCATION.PORT_NO,ASSET_ALLOCATION.VLAN FROM ASSET_ALLOCATION INNER JOIN");
            //    sbQuery.Append(" PROCESS_MASTER ON ASSET_ALLOCATION.ALLOCATED_PROCESS = PROCESS_MASTER.PROCESS_CODE AND");
            //    sbQuery.Append(" ASSET_ALLOCATION.COMP_CODE = PROCESS_MASTER.COMP_CODE INNER JOIN");
            //    sbQuery.Append(" LOCATION_MASTER ON ASSET_ALLOCATION.ASSET_LOCATION = LOCATION_MASTER.LOC_CODE INNER JOIN");
            //    sbQuery.Append(" EMPLOYEE_MASTER ON ASSET_ALLOCATION.ALLOCATED_EMP_ID = EMPLOYEE_MASTER.EMPLOYEE_CODE AND");
            //    sbQuery.Append(" PROCESS_MASTER.COMP_CODE = EMPLOYEE_MASTER.COMP_CODE AND");
            //    sbQuery.Append(" LOCATION_MASTER.COMP_CODE = EMPLOYEE_MASTER.COMP_CODE INNER JOIN");
            //    sbQuery.Append(" ASSET_ACQUISITION ON ASSET_ALLOCATION.ASSET_CODE = ASSET_ACQUISITION.ASSET_CODE INNER JOIN");
            //    sbQuery.Append(" CATEGORY_MASTER ON ASSET_ACQUISITION.CATEGORY_CODE = CATEGORY_MASTER.CATEGORY_CODE INNER JOIN");
            //    sbQuery.Append(" DEPARTMENT_MASTER ON ASSET_ALLOCATION.ALLOCATED_DEPARTMENT = DEPARTMENT_MASTER.DEPT_CODE AND ASSET_ALLOCATION.COMP_CODE = DEPARTMENT_MASTER.COMP_CODE");
            //    sbQuery.Append(" WHERE ASSET_ACQUISITION.ASSET_TYPE LIKE '" + oPRP.AssetType + "' + '%' AND ASSET_ACQUISITION.ASSET_MAKE LIKE '" + oPRP.AssetMake + "' + '%'");
            //    sbQuery.Append(" AND CATEGORY_MASTER.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "' + '%' AND ASSET_ALLOCATION.ALLOCATED_PROCESS LIKE '" + oPRP.ProcessCode + "' + '%'");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "' + '%'");
            //    if (oPRP.ModelName != "")
            //        sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME IN (" + oPRP.ModelName + ")");
            //    else
            //        sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.ALLOCATED_EMP_ID LIKE '" + oPRP.EmpCode + "' + '%'");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.COMP_CODE = '" + oPRP.CompCode + "' AND ASSET_ALLOCATION.ACTUAL_RTN_DATE IS NOT NULL");
            //}
            //sbQuery.Append("sp_ReportAssetAllocation '" + oPRP.AssetType + "' , '" + oPRP.CategoryCode + "' , '" + oPRP.ProcessCode + "' ,");
            //sbQuery.Append(" '" + oPRP.AssetLocation + "' , '" + oPRP.EmpCode + "' , '" + oPRP.AllocationType + "' , '" + oPRP.CompCode + "' ,");
            //sbQuery.Append(" '" + oPRP.FromDate + "' , '" + oPRP.ToDate + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetStockFacilityReport(RptAssetStock_PRP oPRP)
        {
            sbQuery = new StringBuilder();

            sbQuery = new StringBuilder();
            sbQuery.AppendLine(" SELECT [ASSET_LOCATION] AS [Location],[Floor] [Floor Name],[ASSET_TYPE],[CATEGORY_CODE] [Asset Sub Category],[ASSET_MAKE],[MODEL_NAME],[Tag_ID] AS [RFID Tag],ASSET_FAR_TAG[Asset FAR tag],");
            sbQuery.AppendLine(" [ASSET_SUB_STATUS] [Working Status],[STORE] [Store Name],[Identifier_Location] [Asset Identifier location],[Allocation_Date] [Allocation Date],");

            sbQuery.AppendLine(" [AMC_WARRANTY] [Warranty Status] FROM ASSET_ACQUISITION WHERE Status='STOCK' and ASSET_FAR_TAG is not null and ASSET_FAR_TAG<>''");
            if (oPRP.CategoryCode != "")
                sbQuery.Append("  AND ASSET_ACQUISITION.ASSET_TYPE LIKE '" + oPRP.CategoryCode + "' + '%'");
            if (oPRP.AssetMake != "")
                sbQuery.AppendLine(" AND ASSET_ACQUISITION.ASSET_MAKE LIKE '" + oPRP.AssetMake + "' + '%'");
            if (oPRP.ModelName != "")
            {
                if (oPRP.ModelName.Contains(','))
                    sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME IN (" + oPRP.ModelName + ")");
                else
                    sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            }
            //sbQuery.Append(" AND Emp_ID LIKE '" + oPRP.EmpCode + "' + '%'");
            //if (oPRP.FromDate != "")
            //    sbQuery.Append(" AND Allocation_Date >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
            //if (oPRP.ToDate != "")
            //    sbQuery.Append(" AND Allocation_Date <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            sbQuery.Append(" AND ASSET_LOCATION = '" + oPRP.CompCode + "'");
            //  sbQuery.AppendLine(" [REMARKS],[CREATED_BY],[CREATED_ON] )");
            //if (oPRP.AllocationType == )
            //{
            //sbQuery = new StringBuilder();
            //sbQuery.Append("SELECT DEPARTMENT_MASTER.DEPT_NAME, ASSET_ALLOCATION.ASSET_CODE,ASSET_ACQUISITION.SERIAL_CODE,");
            //sbQuery.Append(" CONVERT(VARCHAR,ASSET_ALLOCATION.ASSET_ALLOCATION_DATE, 105) AS ASSET_ALLOCATION_DATE, ASSET_ACQUISITION.ASSET_ALLOCATED,");
            //sbQuery.Append(" PROCESS_MASTER.PROCESS_NAME, EMPLOYEE_MASTER.EMPLOYEE_NAME, LOCATION_MASTER.LOC_NAME, CONVERT(VARCHAR,NULLIF(ASSET_ALLOCATION.EXPECTED_RTN_DATE,''), 105) AS EXPECTED_RTN_DATE,");
            //sbQuery.Append(" CONVERT(VARCHAR, NULLIF(ASSET_ALLOCATION.ACTUAL_RTN_DATE,''),105) AS ACTUAL_RTN_DATE,");
            //sbQuery.Append(" CATEGORY_MASTER.CATEGORY_NAME, CATEGORY_MASTER.ASSET_TYPE,");
            //sbQuery.Append(" ASSET_ALLOCATION.ASSET_ALLOCATED_EMP, ASSET_ALLOCATION.ALLOCATED_PROCESS,");
            //sbQuery.Append(" ASSET_ALLOCATION.ASSET_LOCATION, CATEGORY_MASTER.CATEGORY_CODE,");
            //sbQuery.Append(" ASSET_ALLOCATION.ASSET_ALLOCATION_DATE,ASSET_ALLOCATION.PORT_NO,ASSET_ALLOCATION.VLAN FROM ASSET_ALLOCATION INNER JOIN");
            //sbQuery.Append(" PROCESS_MASTER ON ASSET_ALLOCATION.ALLOCATED_PROCESS = PROCESS_MASTER.PROCESS_CODE AND");
            //sbQuery.Append(" ASSET_ALLOCATION.COMP_CODE = PROCESS_MASTER.COMP_CODE INNER JOIN");
            //sbQuery.Append(" LOCATION_MASTER ON ASSET_ALLOCATION.ASSET_LOCATION = LOCATION_MASTER.LOC_CODE INNER JOIN");
            //sbQuery.Append(" EMPLOYEE_MASTER ON ASSET_ALLOCATION.ALLOCATED_EMP_ID = EMPLOYEE_MASTER.EMPLOYEE_CODE AND ");
            //sbQuery.Append(" PROCESS_MASTER.COMP_CODE = EMPLOYEE_MASTER.COMP_CODE AND");
            //sbQuery.Append(" LOCATION_MASTER.COMP_CODE = EMPLOYEE_MASTER.COMP_CODE INNER JOIN");
            //sbQuery.Append(" ASSET_ACQUISITION ON ASSET_ALLOCATION.ASSET_CODE = ASSET_ACQUISITION.ASSET_CODE INNER JOIN");
            //sbQuery.Append(" CATEGORY_MASTER ON ASSET_ACQUISITION.CATEGORY_CODE = CATEGORY_MASTER.CATEGORY_CODE INNER JOIN");
            //sbQuery.Append(" DEPARTMENT_MASTER ON ASSET_ALLOCATION.ALLOCATED_DEPARTMENT = DEPARTMENT_MASTER.DEPT_CODE AND ASSET_ALLOCATION.COMP_CODE = DEPARTMENT_MASTER.COMP_CODE");
            //sbQuery.Append(" WHERE ASSET_ACQUISITION.ASSET_TYPE LIKE '" + oPRP.AssetType + "' + '%' AND ASSET_ACQUISITION.ASSET_MAKE LIKE '" + oPRP.AssetMake + "' + '%'");
            //sbQuery.Append(" AND CATEGORY_MASTER.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "' + '%' AND ASSET_ALLOCATION.ALLOCATED_PROCESS LIKE '" + oPRP.ProcessCode + "' + '%'");
            //sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "' + '%'");
            //if (oPRP.ModelName != "")
            //    sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME IN (" + oPRP.ModelName + ")");
            //else
            //    sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            //sbQuery.Append(" AND ASSET_ALLOCATION.ALLOCATED_EMP_ID LIKE '" + oPRP.EmpCode + "' + '%'");
            //sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
            //sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            //sbQuery.Append(" AND ASSET_ALLOCATION.COMP_CODE = '" + oPRP.CompCode + "' AND ASSET_ALLOCATION.ACTUAL_RTN_DATE IS NULL");
            //}
            //else if (oPRP.AllocationType == "RETURNED")
            //{
            //    sbQuery = new StringBuilder();
            //    sbQuery.Append("SELECT DEPARTMENT_MASTER.DEPT_NAME, ASSET_ALLOCATION.ASSET_CODE,ASSET_ACQUISITION.SERIAL_CODE,");
            //    sbQuery.Append(" CONVERT(VARCHAR,ASSET_ALLOCATION.ASSET_ALLOCATION_DATE, 105) AS ASSET_ALLOCATION_DATE, ASSET_ACQUISITION.ASSET_ALLOCATED,");
            //    sbQuery.Append(" PROCESS_MASTER.PROCESS_NAME, EMPLOYEE_MASTER.EMPLOYEE_NAME, LOCATION_MASTER.LOC_NAME,");
            //    sbQuery.Append(" CONVERT(VARCHAR,NULLIF(ASSET_ALLOCATION.EXPECTED_RTN_DATE,''), 105) AS EXPECTED_RTN_DATE, CONVERT(VARCHAR,NULLIF(ASSET_ALLOCATION.ACTUAL_RTN_DATE,''),105) AS ACTUAL_RTN_DATE,");
            //    sbQuery.Append(" CATEGORY_MASTER.CATEGORY_NAME, CATEGORY_MASTER.ASSET_TYPE,");
            //    sbQuery.Append(" ASSET_ALLOCATION.ASSET_ALLOCATED_EMP, ASSET_ALLOCATION.ALLOCATED_PROCESS,");
            //    sbQuery.Append(" ASSET_ALLOCATION.ASSET_LOCATION, CATEGORY_MASTER.CATEGORY_CODE,");
            //    sbQuery.Append(" ASSET_ALLOCATION.ASSET_ALLOCATION_DATE,ASSET_ALLOCATION.PORT_NO,ASSET_ALLOCATION.VLAN FROM ASSET_ALLOCATION INNER JOIN");
            //    sbQuery.Append(" PROCESS_MASTER ON ASSET_ALLOCATION.ALLOCATED_PROCESS = PROCESS_MASTER.PROCESS_CODE AND");
            //    sbQuery.Append(" ASSET_ALLOCATION.COMP_CODE = PROCESS_MASTER.COMP_CODE INNER JOIN");
            //    sbQuery.Append(" LOCATION_MASTER ON ASSET_ALLOCATION.ASSET_LOCATION = LOCATION_MASTER.LOC_CODE INNER JOIN");
            //    sbQuery.Append(" EMPLOYEE_MASTER ON ASSET_ALLOCATION.ALLOCATED_EMP_ID = EMPLOYEE_MASTER.EMPLOYEE_CODE AND");
            //    sbQuery.Append(" PROCESS_MASTER.COMP_CODE = EMPLOYEE_MASTER.COMP_CODE AND");
            //    sbQuery.Append(" LOCATION_MASTER.COMP_CODE = EMPLOYEE_MASTER.COMP_CODE INNER JOIN");
            //    sbQuery.Append(" ASSET_ACQUISITION ON ASSET_ALLOCATION.ASSET_CODE = ASSET_ACQUISITION.ASSET_CODE INNER JOIN");
            //    sbQuery.Append(" CATEGORY_MASTER ON ASSET_ACQUISITION.CATEGORY_CODE = CATEGORY_MASTER.CATEGORY_CODE INNER JOIN");
            //    sbQuery.Append(" DEPARTMENT_MASTER ON ASSET_ALLOCATION.ALLOCATED_DEPARTMENT = DEPARTMENT_MASTER.DEPT_CODE AND ASSET_ALLOCATION.COMP_CODE = DEPARTMENT_MASTER.COMP_CODE");
            //    sbQuery.Append(" WHERE ASSET_ACQUISITION.ASSET_TYPE LIKE '" + oPRP.AssetType + "' + '%' AND ASSET_ACQUISITION.ASSET_MAKE LIKE '" + oPRP.AssetMake + "' + '%'");
            //    sbQuery.Append(" AND CATEGORY_MASTER.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "' + '%' AND ASSET_ALLOCATION.ALLOCATED_PROCESS LIKE '" + oPRP.ProcessCode + "' + '%'");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_LOCATION LIKE '" + oPRP.AssetLocation + "' + '%'");
            //    if (oPRP.ModelName != "")
            //        sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME IN (" + oPRP.ModelName + ")");
            //    else
            //        sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.ALLOCATED_EMP_ID LIKE '" + oPRP.EmpCode + "' + '%'");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            //    sbQuery.Append(" AND ASSET_ALLOCATION.COMP_CODE = '" + oPRP.CompCode + "' AND ASSET_ALLOCATION.ACTUAL_RTN_DATE IS NOT NULL");
            //}
            //sbQuery.Append("sp_ReportAssetAllocation '" + oPRP.AssetType + "' , '" + oPRP.CategoryCode + "' , '" + oPRP.ProcessCode + "' ,");
            //sbQuery.Append(" '" + oPRP.AssetLocation + "' , '" + oPRP.EmpCode + "' , '" + oPRP.AllocationType + "' , '" + oPRP.CompCode + "' ,");
            //sbQuery.Append(" '" + oPRP.FromDate + "' , '" + oPRP.ToDate + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReturnableDateExpired"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetReturnableAssetReport(bool ReturnableDateExpired, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT AA.ASSET_CODE,AA.SERIAL_CODE, GG.GATEPASS_CODE, CONVERT(VARCHAR, NULLIF(GA.GATEPASS_IN_DATE,''), 105) AS GATEPASS_IN_DATE,");
            sbQuery.Append(" CONVERT(VARCHAR, NULLIF(GA.EXP_RETURN_DATE,''), 105) AS EXP_RETURN_DATE FROM GATEPASS_ASSETS GA INNER JOIN ASSET_ACQUISITION AA");
            sbQuery.Append(" ON GA.ASSET_CODE = AA.ASSET_CODE INNER JOIN GATEPASS_GENERATION GG ON GA.GATEPASS_CODE = GG.GATEPASS_CODE");
            sbQuery.Append(" WHERE GA.EXP_RETURN_DATE IS NOT NULL ");
            sbQuery.Append((ReturnableDateExpired == true ? " AND GA.EXP_RETURN_DATE < GETDATE()" : ""));
            sbQuery.Append(" AND GG.COMP_CODE='" + CompCode + "' AND AA.COMP_CODE = GG.COMP_CODE");
            //sbQuery.Append(" AND GA.GATEPASS_IN_DATE IS NULL AND GA.GATEPASS_OUT_DATE IS NOT NULL");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}