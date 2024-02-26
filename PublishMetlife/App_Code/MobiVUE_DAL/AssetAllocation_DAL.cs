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
using System.Data.SqlClient;
using MobiVUE_ATS.PRP;

namespace MobiVUE_ATS.DAL
{
    /// <summary>
    /// Summary description for Asset Allocation Data Access Layer
    /// </summary>
    public class AssetAllocation_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public AssetAllocation_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~AssetAllocation_DAL()
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
        //public DataTable GetLocation(string _CompCode, string _ParentLocCode, int _LocLevel)
        //{
        //    sbQuery = new StringBuilder();
        //    sbQuery.Append("SELECT LOC_CODE, LOC_NAME FROM LOCATION_MASTER");
        //    sbQuery.Append(" WHERE PARENT_LOC_CODE LIKE '" + _ParentLocCode + "%' AND LOC_LEVEL=" + _LocLevel + "");
        //    sbQuery.Append(" AND COMP_CODE='" + _CompCode + "' AND ACTIVE=1");
        //    return oDb.GetDataTable(sbQuery.ToString());
        //}
        public DataTable GetMailTransactionDetails(string TransactionType, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM MAIL_MASTER WHERE TRANSACTION_TYPE='" + TransactionType + "' AND COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());

        }
        public string GetAssetSiteLocation(string Assetcode,string _compcode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [ASSET_LOCATION] FROM [ASSET_ACQUISITION] WHERE [ASSET_CODE] = '" + Assetcode + "' AND [COMP_CODE] = '" + _compcode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_LOCATION"].ToString();
            else return string.Empty;
        }
        public string GetAssetFloorLocation(string Assetcode, string _compcode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [FLOOR] FROM [ASSET_ACQUISITION] WHERE [ASSET_CODE] = '" + Assetcode + "' AND [COMP_CODE] = '" + _compcode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["FLOOR"].ToString();
            else return string.Empty;
        }
        public string GetAssetStoreLocation(string Assetcode, string _compcode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [STORE] FROM [ASSET_ACQUISITION] WHERE [ASSET_CODE] = '" + Assetcode + "' AND [COMP_CODE] = '" + _compcode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["STORE"].ToString();
            else return string.Empty;
        }
        public DataTable GetMaildetails(string mailType)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT CC, BCC,Text,IsHtml FROM MailConfiguration WHERE Module='" + mailType + "' ");
            //  sbQuery.Append(" WHERE PARENT_LOC_CODE LIKE '" + _ParentLocCode + "%' AND LOC_LEVEL=" + _LocLevel + "");
            // sbQuery.Append(" AND COMP_CODE='" + _CompCode + "' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch all department name within a location.
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetDepartment(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DEPT_CODE,DEPT_NAME FROM DEPARTMENT_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "' ORDER BY DEPT_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }


        public DataTable GetEmpDetails(string empid)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select EMPLOYEE_CODE, EMPLOYEE_NAME, EMP_EMAIL ,EMP_TAG, Designation, SeatNo, ProcessName, Lob,SubLOB,Emp_Floor from [dbo].[EMPLOYEE_MASTER]  where EMPLOYEE_CODE ='" + empid + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public string GetEmpEmail(string empid)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select EMPLOYEE_CODE, EMPLOYEE_NAME, EMP_EMAIL ,EMP_TAG, Designation, SeatNo, ProcessName, Lob,SubLOB,Emp_Floor from [dbo].[EMPLOYEE_MASTER]  where EMPLOYEE_CODE ='" + empid + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["EMP_EMAIL"].ToString();
            else return "";
        }

        public DataTable GetSubAllocationTo()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT SUB_ALLOCATION_TO FROM SUB_ALLOCATION_TO_MASTERS");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetMeetingRoom(string ddlid, string ddlSite, string ddlfloor, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT MASTER_NAME AS MEETING_ROOM FROM TRAINING_AND_MEETING_MASTER WHERE MASTER_TYPE = '" + ddlid + "' AND SITE_CODE = '" + ddlSite + "' AND FLOOR_CODE = '" + ddlfloor + "' AND COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        /// <summary>
        /// Fetch all department code/name from department master.
        /// </summary>
        /// <returns></returns>
        public DataTable GetProcess(string DeptCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER WHERE ACTIVE='1' AND DEPT_CODE='" + DeptCode + "' AND COMP_CODE='" + CompCode + "' ORDER BY PROCESS_NAME");
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
        /// Get asset make list when asset category is provided.
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateAssetMake(string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT [ASSET_MAKE] FROM [ASSET_ACQUISITION] WHERE [CATEGORY_CODE] = '" + CategoryCode + "' AND [COMP_CODE] = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset model name list when asset make and category is provided.
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
        /// Fetch all employee code/name from employee master based on 
        /// department selected.
        /// </summary>
        /// <param name="_DeptCode"></param>
        /// <returns></returns>
        public DataTable GetDeptEmployee(string _DeptCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT EMPLOYEE_CODE,EMPLOYEE_NAME FROM EMPLOYEE_MASTER");
            sbQuery.Append(" WHERE EMP_DEPT_CODE='" + _DeptCode + "' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get employee code/name within a process.
        /// </summary>
        /// <param name="_DeptCode"></param>
        /// <returns></returns>
        public DataTable GetProcessEmployee(string ProcessCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT EMPLOYEE_CODE,EMPLOYEE_NAME FROM EMPLOYEE_MASTER");
            sbQuery.Append(" WHERE EMP_PROCESS_CODE='" + ProcessCode + "' AND COMP_CODE='" + CompCode + "' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get employee code/name from employee master.
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetEmployee(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT EMPLOYEE_CODE,EMPLOYEE_NAME FROM [dbo].[EMPLOYEE_MASTER] ");
            sbQuery.Append(" WHERE   ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetSiteLocation(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select distinct SITE_CODE from SITE_MASTER WHERE ACTIVE='1' AND COMP_CODE ='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetSubStatus(string SubStatus)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SUB_STATUS_CODE,SUB_STATUS FROM SUB_STATUS_MASTER WHERE ACTIVE=1 AND SUB_STATUS = '" + SubStatus + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetSubStatusGrid()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SUB_STATUS_CODE,SUB_STATUS FROM SUB_STATUS_MASTER WHERE ACTIVE=1 ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetLocation(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SITE_CODE,SITE_ADDRESS FROM SITE_MASTER WHERE ACTIVE=1 AND SITE_CODE <> 'ALL' AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetFloor(string SiteCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT FLOOR_CODE,FLOOR_NAME FROM FLOOR_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetFloorVerify(string SiteCode, string Floor, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT FLOOR_CODE,FLOOR_NAME FROM FLOOR_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND FLOOR_CODE='" + Floor + "' AND COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetStorewithFloor(string SiteCode, string FloorCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT STORE_CODE,STORE_NAME FROM STORE_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND FLOOR_CODE = '" + FloorCode + "'  AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetStoreVerify(string SiteCode, string FloorCode, string Store, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT STORE_CODE,STORE_NAME FROM STORE_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND FLOOR_CODE = '" + FloorCode + "' AND STORE_CODE = '" + Store + "'  AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetSiteLocationWithoutfilter(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select distinct SITE_CODE from SITE_MASTER WHERE ACTIVE='1' AND SITE_CODE <> 'ALL'  AND COMP_CODE ='" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetFloorLocation(string Loc, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select distinct FLOOR_CODE from FLOOR_MASTER where SITE_CODE='" + Loc + "' AND COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable ValidateFloorLocation(string Site, string Floor, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select distinct FLOOR_CODE from FLOOR_MASTER where SITE_CODE='" + Site + "' and FLOOR_CODE='" + Floor + "' and COMP_CODE='" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable ValidateMeetingRoom(string subAllocationTo, string MeetingRoom)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select distinct MEETING_ROOM from SUB_ALLOCATION_TO_MASTERS where SUB_ALLOCATION_TO ='" + subAllocationTo + "' AND MEETING_ROOM = '" + MeetingRoom + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetStoreLocation(string Loc)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select distinct STORE_CODE from STORE_MASTER where SITE_CODE='" + Loc + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable ValidateStoreLocation(string Site, string Store)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select distinct STORE_CODE from STORE_MASTER where SITE_CODE='" + Site + "' and STORE_CODE='" + Store + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetAssetCode()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset serial no when asset code is provided.
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetSerialCode(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SERIAL_CODE FROM ASSET_ACQUISITION WHERE ASSET_CODE = '" + _AssetCode.Trim() + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public string GetAsseTSubStatus(string _AssetCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_SUB_STATUS FROM ASSET_ACQUISITION WHERE ASSET_CODE = '" + _AssetCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_SUB_STATUS"].ToString();
            else return "";
        }

        public string GetAllocationId(string _AssetCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT MAX(ALLOCATION_ID) AS ALLOCATION_ID FROM ASSET_ALLOCATION WHERE ASSET_CODE = '" + _AssetCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ALLOCATION_ID"].ToString();
            else return "";
        }
        public string GetAssetCode(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' AND ASSET_SUB_STATUS = 'WORKING' ");
            else
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' AND ASSET_SUB_STATUS = 'WORKING' ");
            //return oDb.GetDataTable(sbQuery.ToString());
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_CODE"].ToString();
            else return "";
        }

        public string GetAcquisitionLocation(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT ASSET_LOCATION FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' AND ASSET_SUB_STATUS = 'WORKING' ");
            else
                sbQuery.Append("SELECT ASSET_LOCATION FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' AND ASSET_SUB_STATUS = 'WORKING' ");
            //return oDb.GetDataTable(sbQuery.ToString());
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_LOCATION"].ToString();
            else return "";
        }

        public string GetRFIDTag(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' AND ASSET_SUB_STATUS = 'WORKING' ");
            else
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='STOCK' AND ASSET_SUB_STATUS = 'WORKING' ");
            //sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "'");
            //return oDb.GetDataTable(sbQuery.ToString());
            //ASSET_TAG
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TAG_ID"].ToString();
            else return "";
        }

        public string GetAssetCodeforDeallocation(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='ALLOCATED' ");
            else
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='ALLOCATED' ");
            //return oDb.GetDataTable(sbQuery.ToString());
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_CODE"].ToString();
            else return "";
        }

        public string GetRFIDTagforDeallocation(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='ALLOCATED' ");
            else
                sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STATUS='ALLOCATED' ");
            //sbQuery.Append("SELECT TAG_ID FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "'");
            //return oDb.GetDataTable(sbQuery.ToString());
            //ASSET_TAG
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TAG_ID"].ToString();
            else return "";
        }


        /// <summary>
        /// Get assets from asset acquisition table for assets to be allocated.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable GetAssets(AssetAllocation_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_CODE,ASSET_ID,SERIAL_CODE,ASSET_MAKE,MODEL_NAME,TAG_ID, ASSET_PROCESS,ASSET_LOCATION,PORT_NO FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE ASSET_TYPE LIKE '" + oPRP.AssetType + "%' AND ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND SERIAL_CODE LIKE '" + oPRP.SerialCode + "%'");
            if (oPRP.AssetMake != "")
                sbQuery.Append(" AND ASSET_MAKE LIKE '" + oPRP.AssetMake + "%'");
            if (oPRP.FromProcessCode != "")
                sbQuery.Append(" AND ASSET_PROCESS LIKE '" + oPRP.FromProcessCode + "%'");
            if (oPRP.CategoryCode != "")
                sbQuery.Append(" AND CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "%'");
            if (oPRP.ModelName != "")
                sbQuery.Append(" AND MODEL_NAME IN (" + oPRP.ModelName + ")");
            else
                sbQuery.Append(" AND MODEL_NAME LIKE '" + oPRP.ModelName + "%'");
            sbQuery.Append("  AND COMP_CODE='" + oPRP.CompCode + "'  AND SOLD_SCRAPPED_STATUS IS NULL AND STATUS='STOCK'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset details which are to be returned.
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        public DataTable GetAssetToReturnReAllocate(AssetAllocation_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ACQ.ASSET_CODE,ACQ.ASSET_ID,ACQ.SERIAL_CODE,ACQ.ASSET_MAKE,ACQ.MODEL_NAME,ACQ.ASSET_PROCESS,ACQ.ASSET_LOCATION,AAL.WORKSTATION_NO,");
            sbQuery.Append(" AAL.ASSET_ALLOCATION_DATE,AAL.REQUESTED_BY_ID,");
            sbQuery.Append(" AAL.APPROVED_BY_ID,AAL.ASSET_ALLOCATED_EMP,AAL.ALLOCATED_EMP_ID,NULLIF(AAL.EXPECTED_RTN_DATE,'') AS EXPECTED_RTN_DATE,AAL.PORT_NO,AAL.VLAN,");
            sbQuery.Append(" AAL.TICKET_NO,AAL.GATEPASS_NO");
            sbQuery.Append(" FROM ASSET_ACQUISITION ACQ INNER JOIN ASSET_ALLOCATION AAL ON ACQ.ASSET_CODE = AAL.ASSET_CODE");
            sbQuery.Append(" WHERE ACQ.ASSET_TYPE LIKE '" + oPRP.AssetType + "%' AND ACQ.CATEGORY_CODE LIKE '" + oPRP.CategoryCode + "%'");
            sbQuery.Append(" AND ACQ.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND ACQ.SERIAL_CODE LIKE '" + oPRP.SerialCode + "%'");
            sbQuery.Append(" AND ACQ.ASSET_MAKE LIKE '" + oPRP.AssetMake + "%' AND AAL.ALLOCATED_PROCESS LIKE '" + oPRP.FromProcessCode + "%'");    //-- AND AAL.ALLOCATED_EMP_ID LIKE '" + oPRP.AllocatedToId + "%'
            if (oPRP.ModelName != "")
                sbQuery.Append(" AND ACQ.[MODEL_NAME] IN (" + oPRP.ModelName + ")");
            else
                sbQuery.Append(" AND ACQ.[MODEL_NAME] LIKE '" + oPRP.ModelName + "%'");
            sbQuery.Append(" AND AAL.ACTUAL_RTN_DATE IS NULL AND ACQ.ASSET_ALLOCATED = '" + oPRP.AssetAllocated + "' AND AAL.COMP_CODE='" + oPRP.CompCode + "' AND ASSET_APPROVED = 'True' AND SOLD_SCRAPPED_STATUS IS NULL");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public string ChkLiveGP(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GG.GATEPASS_CODE FROM GATEPASS_GENERATION GG INNER JOIN GATEPASS_ASSETS GA ON GG.GATEPASS_CODE=GA.GATEPASS_CODE");
            sbQuery.Append(" WHERE GA.ASSET_TAG='" + _AssetCode + "' AND GA.GATEPASS_IN_DATE IS NULL");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["GATEPASS_CODE"].ToString();
            else return "";
        }
        public bool CheckAssetCode(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ALLOCATION WHERE ASSET_CODE = '" + _AssetCode.Trim() + "' ");
            //return oDb.GetDataTable(sbQuery.ToString());
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return true;
            else return false;
        }

        public DataTable GetAssetAllocationGridDetails(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT AC.SERIAL_CODE,AC.ASSET_SUB_STATUS,AC.ASSET_MAKE,AA.ASSET_TAG,AC.ASSET_FAR_TAG,AC.ASSET_TYPE,AA.ASSET_HOST_NAME,AA.ALLOCATION_TYPE, ");
            sbQuery.Append("CONVERT(VARCHAR,NULLIF(AA.ASSET_ALLOCATION_DATE,''),105) AS ASSET_ALLOCATION_DATE, CONVERT(VARCHAR,NULLIF(AA.EXPECTED_RTN_DATE,''),105) AS EXPECTED_RTN_DATE,AA.TICKET_NO,AA.ALLOCATED_EMP_ID,AA.ASSET_ALLOCATED_EMP,AC.Identifier_Location,AA.ALLOCATED_STATUS ");
            sbQuery.Append(" FROM ASSET_ALLOCATION AA INNER JOIN ASSET_ACQUISITION AC ON AC.ASSET_CODE = AA.ASSET_CODE WHERE AC.STATUS='ALLOCATED' AND AA.ASSET_CODE = '" + _AssetCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetDetailsforEmail(string _AssetCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_LOCATION,ASSET_TYPE,ASSET_MAKE,MODEL_NAME,ASSET_PROCESSOR,ASSET_RAM,ASSET_HDD,TAG_ID FROM ASSET_ACQUISITION WHERE ASSET_CODE = '" + _AssetCode.Trim() + "' AND COMP_CODE='"+_CompCode+"' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetLocation(string _ParentLocCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  SITE_CODE LOC_CODE, SITE_ADDRESS LOC_NAME FROM SITE_MASTER");
            sbQuery.Append(" WHERE ACTIVE='1' AND SITE_CODE = '" + _ParentLocCode + "' AND COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }


        public DataTable GetMeetingRoomwithFloorandSite(string SiteCode, string FloorCode, string SubAllocationTo, string MeetingRoom, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM TRAINING_AND_MEETING_MASTER WHERE MASTER_NAME = '" + MeetingRoom.Trim().Replace("'", "''") + "' AND FLOOR_CODE = '" + FloorCode.Trim().Replace("'", "''") + "' AND SITE_CODE = '" + SiteCode.Trim().Replace("'", "''") + "' AND MASTER_TYPE = '" + SubAllocationTo.Trim().Replace("'", "''") + "' AND COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public string SaveAssetAllocationSP(AssetAllocation_PRP oPRP)
        {
            try
            {
                //string message = "";
                DataTable dt = oDb.ExecuteSPWithOutput("SP_SaveAssetAllocationSP", new SqlParameter("ASSET_CODE", oPRP.AssetCode),
                            new SqlParameter("ASSET_ALLOCATION_DATE", oPRP.AllocationDate),
                            new SqlParameter("REQUESTED_BY", oPRP.RequestedBy),
                            new SqlParameter("REQUESTED_BY_ID", oPRP.RequestedById),
                            new SqlParameter("APPROVED_BY", oPRP.ApprovedBy),
                            new SqlParameter("APPROVED_BY_ID", oPRP.ApprovedById),
                            new SqlParameter("ASSET_ALLOCATION_TO", oPRP.AssetAllocationTo),
                            new SqlParameter("ASSET_ALLOCATED_EMP", oPRP.AllocatedTo),
                            new SqlParameter("ALLOCATED_EMP_ID", oPRP.AllocatedToId),
                            new SqlParameter("ALLOCATED_PROCESS", oPRP.Process),
                            new SqlParameter("STORE", oPRP.Store),
                            new SqlParameter("FLOOR", oPRP.Floor),
                            new SqlParameter("EXPECTED_RTN_DATE", oPRP.ExpReturnDate),
                            new SqlParameter("TICKET_NO", oPRP.TicketNo),
                            new SqlParameter("VLAN", oPRP.Vlan),
                            new SqlParameter("ASSET_TAG", oPRP.AssetTagID),
                            new SqlParameter("SITE", oPRP.Site),
                            new SqlParameter("ALLOCATION_TYPE", oPRP.AllocationType),
                            new SqlParameter("ALLOCATED_EMP_TAGID", oPRP.EmpTagID),
                            new SqlParameter("STATUS", oPRP.Status),
                            new SqlParameter("NO_OF_DUE_DAYS", oPRP.NoofDueDate),
                            new SqlParameter("MeetingRoom", oPRP.MeetingRoom),
                            new SqlParameter("CREATED_BY", oPRP.CreatedBy),
                            //new SqlParameter("CREATED_ON", DateTime.Now),
                            new SqlParameter("REMARKS", oPRP.AllocationRemarks), //insert query until here
                            new SqlParameter("ASSET_HOST_NAME", oPRP.HostName),
                            new SqlParameter("COMP_CODE", oPRP.CompCode),
                            new SqlParameter("SUB_ASSET_ALLOCATION_TO", oPRP.SubAssetAllocationTo),
                            new SqlParameter("HOST_NAME", oPRP.HostName),
                            new SqlParameter("ASSET_ALLOCATED", oPRP.AssetAllocated),
                            new SqlParameter("SERVICE_NOW_TICKET_NO", oPRP.TicketNo),
                            new SqlParameter("Allocation_Date", oPRP.AllocationDate),
                            new SqlParameter("ReturnDate", oPRP.ExpReturnDate),
                            new SqlParameter("Identifier_Location", oPRP.AssetLocation),
                            new SqlParameter("EMP_ID", oPRP.EmpCode),
                            new SqlParameter("EMP_TAG", oPRP.EmpTagID),
                            new SqlParameter("EMP_NAME", oPRP.EmpName),
                            new SqlParameter("Emp_Floor", oPRP.EmpFloor),
                            new SqlParameter("SEAT_NO", oPRP.SeatNo),
                            new SqlParameter("PROCESS_NAME", oPRP.Process),
                            new SqlParameter("Designation", oPRP.Designation),
                            new SqlParameter("LOB", oPRP.LOB),
                            new SqlParameter("SUB_LOB", oPRP.SubLOB), 
                            new SqlParameter("SUB_STATUS", oPRP.SubStatus));
                return Convert.ToString(dt.Rows[0][0]);
                //return true;
            }
            catch (Exception ex)
            {
                return ex.Message;
                //return false;
            }
        }
        public bool SaveAssetAllocation(string OpType, AssetAllocation_PRP oPRP)
        {
            bool bResult = false;
            if (OpType == "ALLOCATE")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO [ASSET_ALLOCATION] ([ASSET_CODE],[ASSET_ALLOCATION_DATE],[REQUESTED_BY],[REQUESTED_BY_ID],");
                sbQuery.Append(" [APPROVED_BY],[APPROVED_BY_ID],[ASSET_ALLOCATION_TO],[ASSET_ALLOCATED_EMP],[ALLOCATED_EMP_ID],[ALLOCATED_PROCESS],");
                sbQuery.Append(" [Store],[Floor],[EXPECTED_RTN_DATE],TICKET_NO,[VLAN],[ASSET_TAG],");
                sbQuery.Append(" [SITE],[ALLOCATION_TYPE],[ALLOCATED_EMP_TAGID],[STATUS],[NO_OF_DUE_DAYS],[MeetingRoom],[CREATED_BY],[CREATED_ON],[REMARKS], [ASSET_HOST_NAME],COMP_CODE,SUB_ASSET_ALLOCATION_TO)");
                sbQuery.Append(" VALUES ");
                sbQuery.Append("('" + oPRP.AssetCode + "','" + oPRP.AllocationDate + "','" + oPRP.RequestedBy + "','" + oPRP.RequestedById + "',");
                sbQuery.Append(" '" + oPRP.ApprovedBy + "','" + oPRP.ApprovedById + "','" + oPRP.AssetAllocationTo + "','" + oPRP.AllocatedTo + "','" + oPRP.AllocatedToId + "','" + oPRP.Process + "',");
                sbQuery.Append(" '" + oPRP.Store + "','" + oPRP.Floor + "','" + oPRP.ExpReturnDate + "','" + oPRP.TicketNo + "','" + oPRP.Vlan + "','" + oPRP.AssetTagID + "',");
                sbQuery.Append(" '" + oPRP.Site + "', '" + oPRP.AllocationType + "','" + oPRP.EmpTagID + "','" + oPRP.Status + "','" + oPRP.NoofDueDate + "','" + oPRP.MeetingRoom + "','" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.AllocationRemarks + "', '" + oPRP.HostName + "','" + oPRP.CompCode + "','" + oPRP.SubAssetAllocationTo + "')");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;

                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE ASSET_ACQUISITION SET HOST_NAME = '" + oPRP.HostName + "', ASSET_ALLOCATED='" + oPRP.AssetAllocated + "', Status='ALLOCATED', Floor='" + oPRP.Floor + "',Store='" + oPRP.Store + "', SERVICE_NOW_TICKET_NO='" + oPRP.TicketNo + "',Allocation_Date='" + oPRP.AllocationDate + "',ReturnDate='" + oPRP.ExpReturnDate + "',");
                sbQuery.Append(" Identifier_Location ='" + oPRP.AssetLocation + "',EMP_ID='" + oPRP.EmpCode + "', EMP_TAG='" + oPRP.EmpTagID + "',EMP_NAME='" + oPRP.EmpName + "', Emp_Floor ='" + oPRP.EmpFloor + "',SEAT_NO='" + oPRP.SeatNo + "',PROCESS_NAME='" + oPRP.Process + "',Designation='" + oPRP.Designation + "',LOB='" + oPRP.LOB + "',SUB_LOB='" + oPRP.SubLOB + "' WHERE  ASSET_CODE='" + oPRP.AssetCode + "'");
                int iRs = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRs > 0)
                    bResult = true;
            }
            if (OpType == "RETURN")
            {
                //sbQuery = new StringBuilder();
                //sbQuery.Append("UPDATE ASSET_ALLOCATION SET ACTUAL_RTN_DATE='" + oPRP.ActualReturnDate + "', ALLOCATED_PROCESS='STOCK',MODIFIED_BY='" + oPRP.ModifiedBy + "',");
                //sbQuery.Append(" MODIFIED_ON=GETDATE()  WHERE ASSET_CODE='" + oPRP.AssetCode + "'   ");
                //int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                //if (iRes > 0)
                //    bResult = true;

                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE ASSET_ACQUISITION SET ASSET_ALLOCATED='" + oPRP.AssetAllocated + "',STATUS='STOCK',ASSET_SUB_STATUS='" + oPRP.SubStatus + "',ASSET_LOCATION ='" + oPRP.Site + "', Floor='" + oPRP.Floor + "',Store='" + oPRP.Store + "',SERVICE_NOW_TICKET_NO=null,Allocation_Date=null,ReturnDate=null,");
                sbQuery.Append("Identifier_Location =null,EMP_ID=null, EMP_TAG=null,EMP_NAME=null, Emp_Floor =null,SEAT_NO=null,PROCESS_NAME=null,Designation=null,LOB=null,SUB_LOB=null WHERE ASSET_CODE='" + oPRP.AssetCode + "' ");
                int iRs = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRs > 0)
                    bResult = true;

                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO [ASSET_DEALLOCATION] ([SERIAL_CODE],[ASSET_CODE],[ALLOCATION_TYPE],[RFID_TAG],[ASSET_MAKE],");
                sbQuery.Append(" [ASSET_FAR_TAG],[ASSET_TYPE],[SITE],[DEALLOCATED_STATUS],[REMARKS],[ASSET_HOST_NAME],[ALLOCATION_DATE],[DEALLOCATED_ASSET_SUB_STATUS],");
                sbQuery.Append(" [EMP_ID],[EMP_NAME],[IDENTIFIER_LOCATION],[TICKET_NO],[DEALLOCATION_DATE],[CREATED_BY],");
                sbQuery.Append(" [CREATED_ON],[COMP_CODE],[EXPECTED_RTN_DATE],[ALLOCATION_ID])");
                sbQuery.Append(" VALUES ");
                sbQuery.Append("('" + oPRP.SerialCode + "','" + oPRP.AssetCode + "','" + oPRP.AllocationType + "','" + oPRP.RFIDTag + "','" + oPRP.AssetMake + "',");
                sbQuery.Append(" '" + oPRP.Asset_FAR_TAG + "','" + oPRP.AssetType + "','" + oPRP.AssetLocation + "','DEALLOCATED','" + oPRP.DeallocationRemarks + "','" + oPRP.HostName + "','" + oPRP.AllocationDate + "','" + oPRP.SubStatus + "',");
                sbQuery.Append(" '" + oPRP.EmpCode + "','" + oPRP.EmpName + "','" + oPRP.IdentifierLocation + "','" + oPRP.TicketNo + "','" + oPRP.ActualReturnDate + "','" + oPRP.CreatedBy + "',");
                sbQuery.Append(" GETDATE(),'" + oPRP.CompCode + "','" + oPRP.ExpReturnDate + "','" + oPRP.AllocationId + "')");
                int iResi = oDb.ExecuteQuery(sbQuery.ToString());
                if (iResi > 0)
                    bResult = true;

                sbQuery = new StringBuilder();
                sbQuery.Append(" INSERT INTO ASSET_ALLOCATION_HISTORY SELECT *,'" + oPRP.CreatedBy + "' AS HISTORY_CREATED_BY,GETDATE() AS HISTORY_CREATED_ON FROM ASSET_ALLOCATION WHERE ASSET_CODE='" + oPRP.AssetCode + "' ");
                int iRes2 = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes2 > 0)
                    bResult = true;

                sbQuery = new StringBuilder();
                sbQuery.Append(" DELETE FROM ASSET_ALLOCATION WHERE ASSET_CODE='" + oPRP.AssetCode + "' ");
                int iRes3 = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes3 > 0)
                    bResult = true;
            }
            if (OpType == "REALLOCATE")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE [ASSET_ALLOCATION] SET [ASSET_ALLOCATION_DATE] = '" + oPRP.AllocationDate + "',[REQUESTED_BY] = '" + oPRP.RequestedBy + "',[REQUESTED_BY_ID] = '" + oPRP.RequestedById + "'");
                sbQuery.Append(",[APPROVED_BY] = '" + oPRP.ApprovedBy + "',[APPROVED_BY_ID] = '" + oPRP.ApprovedById + "',[ASSET_ALLOCATED_EMP] = '" + oPRP.AllocatedTo + "',[ALLOCATED_EMP_ID] = '" + oPRP.AllocatedToId + "',[ALLOCATED_DEPARTMENT] = '" + oPRP.ToDeptCode + "'");
                sbQuery.Append(",[ALLOCATED_PROCESS] = '" + oPRP.ToProcessCode + "',[ASSET_LOCATION] = '" + oPRP.AssetLocation + "',[EXPECTED_RTN_DATE] = '" + oPRP.ExpReturnDate + "'");
                sbQuery.Append(",[PORT_NO] = '" + oPRP.PortNo + "',[VLAN] = '" + oPRP.Vlan + "',[TICKET_NO] = '" + oPRP.TicketNo + "',[GATEPASS_NO] = '" + oPRP.GatePassNo + "',[WORKSTATION_NO] = '" + oPRP.WorkStationNo + "',[MODIFIED_BY] = '" + oPRP.ModifiedBy + "',[MODIFIED_ON] = GETDATE()");
                sbQuery.Append(",[REMARKS] = '" + oPRP.AllocationRemarks + "' WHERE [ASSET_CODE] = '" + oPRP.AssetCode + "' AND [COMP_CODE] = '" + oPRP.CompCode + "'");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;

                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE ASSET_ACQUISITION SET ASSET_ALLOCATED='" + oPRP.AssetAllocated + "',ASSET_LOCATION='" + oPRP.AssetLocation + "',DEPARTMENT='" + oPRP.ToDeptCode + "',ASSET_PROCESS='" + oPRP.ToProcessCode + "',");
                sbQuery.Append(" PORT_NO='" + oPRP.PortNo + "' WHERE ASSET_CODE='" + oPRP.AssetCode + "' AND COMP_CODE='" + oPRP.CompCode + "'");
                int iRs = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRs > 0)
                    bResult = true;
            }
            return bResult;
        }

        /// <summary>
        /// Re-allocate assets to another employee/process.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool UpdateAssetAllocation(string OpType, AssetAllocation_PRP oPRP)
        {
            bool bResult = false;
            if (OpType == "REALLOCATE")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE [ASSET_ALLOCATION] SET [ASSET_ALLOCATION_DATE] = '" + oPRP.AllocationDate + "',[REQUESTED_BY] = '" + oPRP.RequestedBy + "',[REQUESTED_BY_ID] = '" + oPRP.RequestedById + "'");
                sbQuery.Append(",[APPROVED_BY] = '" + oPRP.ApprovedBy + "',[APPROVED_BY_ID] = '" + oPRP.ApprovedById + "',[ASSET_ALLOCATED_EMP] = '" + oPRP.AllocatedTo + "',[ALLOCATED_EMP_ID] = '" + oPRP.AllocatedToId + "',[ALLOCATED_PROCESS] = '" + oPRP.ToProcessCode + "'");
                sbQuery.Append(",[ASSET_LOCATION] = '" + oPRP.AssetLocation + "',[EXPECTED_RTN_DATE] = '" + oPRP.ExpReturnDate + "',[ACTUAL_RTN_DATE] = '" + oPRP.ActualReturnDate + "',[COMP_CODE] = '" + oPRP.CompCode + "',[PORT_NO] = '" + oPRP.PortNo + "'");
                sbQuery.Append(",[VLAN] = '" + oPRP.Vlan + "',[TICKET_NO] = '" + oPRP.TicketNo + "',[GATEPASS_NO] = '" + oPRP.GatePassNo + "',[WORKSTATION_NO] = '" + oPRP.WorkStationNo + "',[CREATED_BY] = '" + oPRP.CreatedBy + "',[CREATED_ON] = GETDATE()");
                sbQuery.Append(",[REMARKS] = '" + oPRP.AllocationRemarks + "'");
                sbQuery.Append(" WHERE [ASSET_CODE] = '" + oPRP.AssetCode + "'");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
            }
            return bResult;
        }

        /// <summary>
        /// Get asset allocation details.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetAssetAllocationDetails(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT AAL.ASSET_CODE,AAL.ASSET_TAG,AAL.SERIAL_CODE, ALLOCATION_TYPE ,CONVERT(VARCHAR,NULLIF(AAL.ASSET_ALLOCATION_DATE,''),105) AS ASSET_ALLOCATION_DATE,EM.EMPLOYEE_NAME,AAL.ALLOCATED_EMP_TAGID,");
            sbQuery.Append("CONVERT(VARCHAR,NULLIF(AAL.EXPECTED_RTN_DATE,''),105) AS EXPECTED_RTN_DATE");
            sbQuery.Append(" FROM ASSET_ALLOCATION AAL INNER JOIN ASSET_ACQUISITION AA ON AAL.ASSET_TAG = AA.TAG_ID AND AAL.COMP_CODE = AA.COMP_CODE");
            sbQuery.Append(" INNER JOIN CATEGORY_MASTER CM ON AA.CATEGORY_CODE = CM.CATEGORY_CODE LEFT JOIN EMPLOYEE_MASTER EM");
            sbQuery.Append(" ON AAL.ALLOCATED_EMP_ID = EM.EMPLOYEE_CODE AND AAL.COMP_CODE = EM.COMP_CODE");
            sbQuery.Append(" WHERE AAL.COMP_CODE='" + CompCode + "' ORDER BY AAL.MODIFIED_ON DESC");

            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetDetails(string _AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_ID,ASSET_CODE,Tag_ID, ASSET_MAKE,MODEL_NAME,ASSET_TYPE,ASSET_FAR_TAG,SERIAL_CODE, Status,ASSET_SUB_STATUS,ASSET_HDD,ASSET_PROCESSOR,ASSET_RAM FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "' and STATUS='STOCK'  and SOLD_SCRAPPED_STATUS is null ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable ValidateAssetDetailsForAllocation(string _AssetCode, string AssetTag)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_ID,ASSET_CODE,Tag_ID, ASSET_MAKE,MODEL_NAME,ASSET_TYPE,SERIAL_CODE, Status,ASSET_SUB_STATUS,ASSET_HDD,ASSET_PROCESSOR,ASSET_RAM FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "' and STATUS='STOCK' AND ASSET_SUB_STATUS ='WORKING' and SOLD_SCRAPPED_STATUS is null ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetDeallocationAssetDetails(string _AssetCode, string AssetTag)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_ID,ASSET_CODE,Tag_ID, ASSET_MAKE,MODEL_NAME,ASSET_TYPE,SERIAL_CODE, Status,ASSET_SUB_STATUS,ASSET_HDD,ASSET_PROCESSOR,ASSET_RAM FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "' and STATUS='ALLOCATED' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetAllSearchAssetDetails(string _AssetMake, string Model, string AssetType, string SiteLocation, string SerialNo, string Status, string Compcode, string filterStatus, string filterSubStatus, string FilterAssetFarTag, string FilterAssetDomain, string FilterAssetHDD, string FilterAssetProcessor, string FilterAssetRAM, string FilterAssetPoNumber, string FilterAssetVendor, string FilterInvoiceNo, string FilterAssetRFIDTag, string FilterAssetGRNNo)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("EXEC sp_GetAssetAllocation '" + _AssetMake + "','" + Model + "','" + AssetType + "','" + SiteLocation + "','" + SerialNo + "', '" + Status + "','" + Compcode + "','" + filterStatus + "','" + filterSubStatus + "','" + FilterAssetFarTag + "','" + FilterAssetDomain + "','" + FilterAssetHDD + "','" + FilterAssetProcessor + "','" + FilterAssetRAM + "','" + FilterAssetPoNumber + "','" + FilterAssetVendor + "','" + FilterInvoiceNo + "','" + FilterAssetRFIDTag + "','" + FilterAssetGRNNo + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetAssetDetailsForDeAllocation(string AssetID, string AssetCode, string TagId, string EmpID, string EMPName, string Floor, string SeatNo, string Designation, string ProcessName, string AssetSubStatus, string _AssetMake, string Model, string AssetType, string SiteLocation, string SerialNo, string Status, string Compcode, string FilterAssetFarTag, string FilterAssetDomain, string FilterAssetHDD, string FilterAssetProcessor, string FilterAssetRAM, string FilterAssetPoNumber, string FilterAssetVendor, string FilterInvoiceNo, string FilterAssetGRNNo)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("EXEC sp_GetAssetDeAllocation  ");
            sbQuery.Append(" '" + AssetID + "','" + AssetCode + "','" + TagId + "','" + EmpID + "','" + EMPName + "', '" + Floor + "','" + SeatNo + "', ");
            sbQuery.Append(" '" + Designation + "','" + ProcessName + "','" + AssetSubStatus + "', ");
            sbQuery.Append(" '" + _AssetMake + "','" + Model + "','" + AssetType + "','" + SiteLocation + "','" + SerialNo + "', '" + Status + "','" + Compcode + "', ");
            sbQuery.Append(" '" + FilterAssetFarTag + "','" + FilterAssetDomain + "','" + FilterAssetHDD + "','" + FilterAssetProcessor + "','" + FilterAssetRAM + "','" + FilterAssetPoNumber + "','" + FilterAssetVendor + "','" + FilterInvoiceNo + "','" + FilterAssetGRNNo + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetAllocatedAssetDetails(string TagID)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_ACQUISITION.ASSET_CODE,ASSET_ACQUISITION.ASSET_ID,ASSET_ACQUISITION.TAG_ID , ASSET_ACQUISITION.STATUS , ASSET_ALLOCATION_TO AS ALLOCATION_TYPE,CONVERT(VARCHAR,NULLIF(ALLOCATION_DATE,''),105) AS ASSET_ALLOCATION_DATE,ASSET_ACQUISITION.EMP_TAG, ");
            sbQuery.Append(" CONVERT(VARCHAR,NULLIF(ReturnDATE,''),105) AS EXPECTED_RTN_DATE,ASSET_ACQUISITION.STORE,ASSET_ACQUISITION.FLOOR, ASSET_ACQUISITION.ASSET_SUB_Status,SERVICE_NOW_TICKET_NO, ASSET_ACQUISITION.EMP_ID,Identifier_Location ");
            sbQuery.Append(" FROM ASSET_ACQUISITION LEFT  Join ASSET_ALLOCATION on ASSET_ACQUISITION.ASSET_CODE= ASSET_ALLOCATION.ASSET_CODE AND ASSET_ACQUISITION.TAG_ID=ASSET_ALLOCATION.ASSET_TAG  and ACTUAL_RTN_DATE is null");
            sbQuery.Append(" WHERE ASSET_ACQUISITION.STATUS='ALLOCATED'  AND (ASSET_ACQUISITION.TAG_ID='" + TagID + "' or ASSET_ACQUISITION.ASSET_CODE='" + TagID + "')");

            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}