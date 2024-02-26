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
    /// Summary description for Asset Allocation Report.
    /// </summary>
    public class RptAssetAllocation_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public RptAssetAllocation_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~RptAssetAllocation_DAL()
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
        public DataTable GetLocation(string _CompCode, string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT LOC_CODE, LOC_NAME FROM LOCATION_MASTER");
            sbQuery.Append(" WHERE PARENT_LOC_CODE='" + _ParentLocCode + "' AND LOC_LEVEL=" + _LocLevel + "");
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
            sbQuery.Append("SELECT DISTINCT ASSET_TYPE CATEGORY_CODE, ASSET_TYPE CATEGORY_NAME FROM ASSET_ACQUISITION");
            //sbQuery.Append(" WHERE ASSET_TYPE='" + _AssetType + "' AND PARENT_CATEGORY='" + _ParentCategory + "'");
           // sbQuery.Append(" AND CATEGORY_LEVEL=" + _CatLevel + " AND ACTIVE=1");
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
        /// Fetch all employee code/name from employee master based on
        /// process name selected.
        /// </summary>
        /// <param name="_DeptCode"></param>
        /// <returns></returns>
        public DataTable GetProcEmployee(string _ProcessCode,string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT EMPLOYEE_CODE,EMPLOYEE_NAME FROM EMPLOYEE_MASTER");
            sbQuery.Append(" WHERE  COMP_CODE='" + _CompCode + "' AND ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset allocation report details.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetAssetAllocationITReport(RptAssetAllocation_PRP oPRP)
        {
            sbQuery = new StringBuilder();

            sbQuery = new StringBuilder();
            sbQuery.AppendLine(" SELECT  [ASSET_LOCATION] AS [Location],[Floor] [Floor Name],[ASSET_TYPE],[ASSET_MAKE],[MODEL_NAME],[ASSET_CODE] AS [Serial No],[Tag_ID] AS [RFID Tag],[ASSET_ID] AS [Asset Tag],");
            sbQuery.AppendLine(" [Status] [Asset Status],[ASSET_SUB_STATUS] [Sub Status],[STORE] [Store Name],[SERVICE_NOW_TICKET_NO] [ServiceNow Ticket no],[Allocation_Date] [Allocation Date],[ReturnDate] [Return Date], [EMP_TAG],[EMP_ID][Employee ID],[EMP_NAME] [Employee Name],");
            sbQuery.AppendLine(" [EMP_FLOOR] [Floor],[SEAT_NO][Seat No],[Designation],[PROCESS_NAME] [Process Name],[SUB_LOB] [Sub LOB],[LOB], ");
            sbQuery.AppendLine(" [ASSET_PROCESSOR][Processor],[ASSET_HDD][HDD],[ASSET_RAM][RAM],[AMC_WARRANTY] [Warranty Status] FROM ASSET_ACQUISITION WHERE Status='Allocated' and ( ASSET_FAR_TAG is null or ASSET_FAR_TAG='')");
            if (oPRP.CategoryCode != "")
                sbQuery.Append("  AND ASSET_ACQUISITION.ASSET_TYPE LIKE '" + oPRP.CategoryCode + "' + '%'");
            if (oPRP.AssetMake != "")
                sbQuery.AppendLine(" AND ASSET_ACQUISITION.ASSET_MAKE LIKE '" + oPRP.AssetMake + "' + '%'");
            if (oPRP.ModelName != "")
            {
                if(oPRP.ModelName.Contains(','))
                sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME IN (" + oPRP.ModelName + ")");
             else
                sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            }
            if(oPRP.EmpCode!="")
            sbQuery.Append(" AND Emp_ID LIKE '" + oPRP.EmpCode + "' + '%'");
            if (oPRP.FromDate != "" && oPRP.FromDate.Trim()!= "01/Jan/1900")
                sbQuery.Append(" AND Allocation_Date >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
            if (oPRP.ToDate != "" && oPRP.ToDate.Trim()!=DateTime.Now.ToString("dd/MMM/yyyy"))
                sbQuery.Append(" AND Allocation_Date <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            sbQuery.Append(" AND Comp_CODE = '" + oPRP.CompCode + "'");
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

        public DataTable GetAssetAllocationFacilityReport(RptAssetAllocation_PRP oPRP)
        {
            sbQuery = new StringBuilder();

            sbQuery = new StringBuilder();
            sbQuery.AppendLine(" SELECT  [ASSET_LOCATION] AS [Location],[Floor] [Floor Name],[ASSET_TYPE],[CATEGORY_CODE] [Asset Sub Category],[ASSET_MAKE],[MODEL_NAME],[Tag_ID] AS [RFID Tag],ASSET_FAR_TAG[Asset FAR tag],");
            sbQuery.AppendLine(" [ASSET_SUB_STATUS] [Working Status],[STORE] [Store Name],[Identifier_Location] [Asset Identifier location],[Allocation_Date] [Allocation Date],");
     
            sbQuery.AppendLine(" [AMC_WARRANTY] [Warranty Status] FROM ASSET_ACQUISITION WHERE Status='Allocated' and ASSET_FAR_TAG is not null and ASSET_FAR_TAG<>''");
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
            if(oPRP.EmpCode!="")
            sbQuery.Append(" AND Emp_ID LIKE '" + oPRP.EmpCode + "' + '%'");
            if (oPRP.FromDate != "" && oPRP.FromDate.Trim() != "01/Jan/1900")
                sbQuery.Append(" AND Allocation_Date >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
            if (oPRP.ToDate != "" && oPRP.ToDate.Trim() != DateTime.Now.ToString("dd/MMM/yyyy"))
                sbQuery.Append(" AND Allocation_Date <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            sbQuery.Append(" AND Comp_CODE = '" + oPRP.CompCode + "'");
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
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetAllocatedAssetsStatus(RptAssetAllocation_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_ALLOCATION.ASSET_CODE,  ASSET_ACQUISITION.ASSET_ID, ASSET_ACQUISITION.TAG_ID,  CONVERT(VARCHAR,ASSET_ALLOCATION.ASSET_ALLOCATION_DATE, 105) AS ASSET_ALLOCATION_DATE,");
            sbQuery.Append("   ASSET_ACQUISITION.STATUS,");
            sbQuery.Append("  EMPLOYEE_MASTER.EMPLOYEE_NAME, ASSET_ACQUISITION.ASSET_MAKE,");
            sbQuery.Append(" CONVERT(VARCHAR,NULLIF(ASSET_ALLOCATION.EXPECTED_RTN_DATE,''), 105) AS EXPECTED_RTN_DATE,"); 
            sbQuery.Append(" CONVERT(VARCHAR, NULLIF(ASSET_ALLOCATION.ACTUAL_RTN_DATE,''), 105) AS ACTUAL_RTN_DATE,"); 
            sbQuery.Append(" ASSET_TYPE , ASSET_ALLOCATION.ASSET_ALLOCATED_EMP,"); 
            sbQuery.Append(" ASSET_ACQUISITION.MODEL_NAME,"); 
            sbQuery.Append(" ASSET_ALLOCATION.ASSET_ALLOCATION_DATE,ASSET_ALLOCATION.ACTUAL_RTN_DATE FROM ASSET_ALLOCATION ");
           // sbQuery.Append(" PROCESS_MASTER ON ASSET_ALLOCATION.ALLOCATED_PROCESS = PROCESS_MASTER.PROCESS_CODE AND");
          //  sbQuery.Append(" ASSET_ALLOCATION.COMP_CODE = PROCESS_MASTER.COMP_CODE INNER JOIN");
            sbQuery.Append("  LEFT JOIN EMPLOYEE_MASTER ON ASSET_ALLOCATION.ALLOCATED_EMP_ID = EMPLOYEE_MASTER.EMPLOYEE_CODE ");
          //  sbQuery.Append(" PROCESS_MASTER.COMP_CODE = EMPLOYEE_MASTER.COMP_CODE INNER JOIN");
            sbQuery.Append(" INNER JOIN ASSET_ACQUISITION ON ASSET_ALLOCATION.ASSET_CODE = ASSET_ACQUISITION.ASSET_CODE ");
           // sbQuery.Append(" CATEGORY_MASTER ON ASSET_ACQUISITION.CATEGORY_CODE = CATEGORY_MASTER.CATEGORY_CODE");
            sbQuery.Append(" WHERE ASSET_ACQUISITION.ASSET_MAKE LIKE '" + oPRP.AssetMake + "' + '%'");
            sbQuery.Append(" AND ASSET_TYPE LIKE '" + oPRP.AssetType + "' + '%'");
            if (oPRP.ModelName != "")
                sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME IN (" + oPRP.ModelName + ")");
            else
                sbQuery.Append(" AND ASSET_ACQUISITION.MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            sbQuery.Append(" AND ASSET_ALLOCATION.ALLOCATED_EMP_ID LIKE '" + oPRP.EmpCode + "' + '%'");
            if (oPRP.DateSearchBy == "ALLOCATE")
            {
                sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
                sbQuery.Append(" AND ASSET_ALLOCATION.ASSET_ALLOCATION_DATE <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            }
            else if (oPRP.DateSearchBy == "RETURN")
            {
                sbQuery.Append(" AND ASSET_ALLOCATION.ACTUAL_RTN_DATE >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
                sbQuery.Append(" AND ASSET_ALLOCATION.ACTUAL_RTN_DATE <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            }
            sbQuery.Append(" AND ASSET_ALLOCATION.COMP_CODE = '" + oPRP.CompCode + "' AND ASSET_ALLOCATION.ACTUAL_RTN_DATE is not Null");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}