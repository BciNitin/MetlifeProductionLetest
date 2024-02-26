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
    /// Summary description for Asset Swapping Data Access Layer.
    /// </summary>
    public class AssetSwapping_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public AssetSwapping_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~AssetSwapping_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Get assets from asset acquisition/allocation table for assets to be swapped.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable GetAssetsForSwapping(AssetSwapping_PRP oPRP)
        {
            if (oPRP.AllocatedAssets)
            {
                sbQuery = new StringBuilder();
                sbQuery.Append(" SELECT ACQ.ASSET_CODE,ACQ.ASSET_ID,ACQ.SERIAL_CODE,ACQ.ASSET_LOCATION,");
                sbQuery.Append(" EM.EMPLOYEE_NAME,PM.PROCESS_NAME,AAL.ASSET_LOCATION,AAL.ASSET_ALLOCATED_EMP,AAL.ALLOCATED_EMP_ID,AAL.ALLOCATED_PROCESS,ACQ.WORKSTATION_NO,ACQ.PORT_NO");
                sbQuery.Append(" FROM ASSET_ACQUISITION ACQ LEFT OUTER JOIN ASSET_ALLOCATION AAL ON ACQ.ASSET_CODE = AAL.ASSET_CODE");
                sbQuery.Append(" INNER JOIN EMPLOYEE_MASTER EM ON AAL.ALLOCATED_EMP_ID = EM.EMPLOYEE_CODE");
                sbQuery.Append(" INNER JOIN PROCESS_MASTER PM ON AAL.ALLOCATED_PROCESS = PM.PROCESS_CODE");
                sbQuery.Append(" WHERE ACQ.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND ACQ.SERIAL_CODE LIKE '" + oPRP.SerialCode + "%' AND ACQ.PORT_NO LIKE '" + oPRP.PortNo + "%'");
                sbQuery.Append(" AND ACQ.ASSET_LOCATION LIKE '" + oPRP.LocationCode + "%' AND ACQ.COMP_CODE='" + oPRP.CompCode + "' AND ACQ.COMP_CODE = AAL.COMP_CODE AND AAL.COMP_CODE = EM.COMP_CODE AND AAL.COMP_CODE = PM.COMP_CODE");
                sbQuery.Append(" AND ACQ.ASSET_ALLOCATED = 'True' AND ACQ.ASSET_APPROVED = 'True' AND ACQ.SOLD_SCRAPPED_STATUS IS NULL ORDER BY ACQ.PORT_NO");
            }
            else
            {
                sbQuery = new StringBuilder();
                sbQuery.Append(" SELECT ACQ.ASSET_CODE,ACQ.ASSET_ID,ACQ.SERIAL_CODE,ACQ.ASSET_LOCATION,'' AS EMPLOYEE_NAME,PM.PROCESS_NAME,");
                sbQuery.Append(" ACQ.ASSET_LOCATION,'' AS ASSET_ALLOCATED_EMP,'' AS ALLOCATED_EMP_ID,ACQ.ASSET_PROCESS AS ALLOCATED_PROCESS,ACQ.WORKSTATION_NO,ACQ.PORT_NO");
                sbQuery.Append(" FROM ASSET_ACQUISITION ACQ INNER JOIN PROCESS_MASTER PM ON ACQ.ASSET_PROCESS = PM.PROCESS_CODE");
                sbQuery.Append(" WHERE ACQ.ASSET_LOCATION LIKE '" + oPRP.LocationCode + "%' AND ACQ.ASSET_CODE LIKE '" + oPRP.AssetCode + "%' AND ACQ.PORT_NO LIKE '" + oPRP.PortNo + "%'");
                sbQuery.Append(" AND ACQ.SERIAL_CODE LIKE '" + oPRP.SerialCode + "%' AND ACQ.COMP_CODE='" + oPRP.CompCode + "' AND ACQ.COMP_CODE = PM.COMP_CODE");
                sbQuery.Append(" AND ACQ.ASSET_ALLOCATED = 'False' AND ACQ.ASSET_APPROVED = 'True' AND ACQ.SOLD_SCRAPPED_STATUS IS NULL ORDER BY ACQ.PORT_NO");
            }
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Swap assets which are not allocated to any department/process/employee.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SwapNotAllocatedAssets(AssetSwapping_PRP oPRP)
        {
            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.Append(" UPDATE ASSET_ACQUISITION SET ASSET_LOCATION='" + oPRP.LocationCode2 + "',ASSET_PROCESS='" + oPRP.ProcessCode2 + "',WORKSTATION_NO='" + oPRP.WorkStation2 + "',PORT_NO='" + oPRP.PortNo2 + "',ASSET_APPROVED='0',REMARKS='" + oPRP.SwappingRemarks + "'");
            sbQuery.Append(" WHERE ASSET_CODE='" + oPRP.AssetCode1 + "' AND ASSET_ID='" + oPRP.AssetID1 + "' AND SERIAL_CODE='" + oPRP.SerialCode1 + "' AND COMP_CODE='" + oPRP.CompCode + "'");
            int iRes1 = oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.Append(" UPDATE ASSET_ACQUISITION SET ASSET_LOCATION='" + oPRP.LocationCode1 + "',ASSET_PROCESS='" + oPRP.ProcessCode1 + "',WORKSTATION_NO='" + oPRP.WorkStation1 + "',PORT_NO='" + oPRP.PortNo1 + "',ASSET_APPROVED='0',REMARKS='" + oPRP.SwappingRemarks + "'");
            sbQuery.Append(" WHERE ASSET_CODE='" + oPRP.AssetCode2 + "' AND ASSET_ID='" + oPRP.AssetID2 + "' AND SERIAL_CODE='" + oPRP.SerialCode2 + "' AND COMP_CODE='" + oPRP.CompCode + "'");
            int iRes2 = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes1 > 0 && iRes2 > 0)
                bResult = true;
            return bResult;
        }

        /// <summary>
        /// Swap assets which are already allocated to some department/process/employee.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SwapAllocatedAssets(AssetSwapping_PRP oPRP)
        {
            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.Append(" UPDATE ASSET_ALLOCATION SET ASSET_ALLOCATED_EMP='" + oPRP.EmpName2 + "',ALLOCATED_EMP_ID='" + oPRP.EmpCode2 + "', ALLOCATED_PROCESS='" + oPRP.ProcessCode2 + "',WORKSTATION_NO='" + oPRP.WorkStation2 + "',PORT_NO='" + oPRP.PortNo2 + "',REMARKS='" + oPRP.SwappingRemarks + "',");
            sbQuery.Append(" ASSET_LOCATION='" + oPRP.LocationCode2 + "' WHERE ASSET_CODE='" + oPRP.AssetCode1 + "' AND COMP_CODE='" + oPRP.CompCode + "'");
            int iRes1 = oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.Append(" UPDATE ASSET_ACQUISITION SET ASSET_LOCATION='" + oPRP.LocationCode2 + "',ASSET_PROCESS='" + oPRP.ProcessCode2 + "',WORKSTATION_NO='" + oPRP.WorkStation2 + "',ASSET_APPROVED='0',REMARKS='" + oPRP.SwappingRemarks + "',");
            sbQuery.Append(" PORT_NO='" + oPRP.PortNo2 + "' WHERE ASSET_CODE='" + oPRP.AssetCode1 + "' AND COMP_CODE='" + oPRP.CompCode + "'");
            int iRes2 = oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.Append(" UPDATE ASSET_ALLOCATION SET ASSET_ALLOCATED_EMP='" + oPRP.EmpName1 + "',ALLOCATED_EMP_ID='" + oPRP.EmpCode1 + "', ALLOCATED_PROCESS='" + oPRP.ProcessCode1 + "',WORKSTATION_NO='" + oPRP.WorkStation1 + "',PORT_NO='" + oPRP.PortNo1 + "',REMARKS='" + oPRP.SwappingRemarks + "',");
            sbQuery.Append(" ASSET_LOCATION='" + oPRP.LocationCode1 + "' WHERE ASSET_CODE='" + oPRP.AssetCode2 + "' AND COMP_CODE='" + oPRP.CompCode + "'");
            int iRes3 = oDb.ExecuteQuery(sbQuery.ToString());

            sbQuery = new StringBuilder();
            sbQuery.Append(" UPDATE ASSET_ACQUISITION SET ASSET_LOCATION='" + oPRP.LocationCode1 + "',ASSET_PROCESS='" + oPRP.ProcessCode1 + "',WORKSTATION_NO='" + oPRP.WorkStation1 + "',ASSET_APPROVED='0',REMARKS='" + oPRP.SwappingRemarks + "',");
            sbQuery.Append(" PORT_NO='" + oPRP.PortNo1 + "' WHERE ASSET_CODE='" + oPRP.AssetCode2 + "' AND COMP_CODE='" + oPRP.CompCode + "'");
            int iRes4 = oDb.ExecuteQuery(sbQuery.ToString());

            if (iRes1 > 0 && iRes2 > 0 && iRes3 > 0 && iRes4 > 0)
                bResult = true;
            return bResult;
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
            sbQuery.Append(" WHERE PARENT_LOC_CODE LIKE '" + _ParentLocCode + "%' AND LOC_LEVEL=" + _LocLevel + "");
            sbQuery.Append(" AND COMP_CODE='" + _CompCode + "' AND ACTIVE='1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable GetAssetDetails(string _AssetCode,string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SERIAL_CODE,PORT_NO FROM ASSET_ACQUISITION");
            sbQuery.Append(" WHERE ASSET_CODE='" + _AssetCode + "' AND COMP_CODE='" + _CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Save asset swap history details.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveAssetSwapHistory(AssetSwapping_PRP oPRP)
        {
            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("INSERT INTO [ASSET_SWAP_HISTORY] ([ASSET_CODE1],[SERIAL_CODE1],[LOCATION2],[PROCESS2],[WORKSTATION2],[PORT2],[EMPLOYEE_NAME2],[EMPLOYEE_CODE2]");
            sbQuery.Append(" ,[ASSET_CODE2],[SERIAL_CODE2],[LOCATION1],[PROCESS1],[WORKSTATION1],[PORT1],[EMPLOYEE_NAME1],[EMPLOYEE_CODE1]");
            sbQuery.Append(" ,[COMP_CODE],[SWAP_DATE],[REMARKS])");
            sbQuery.Append(" VALUES");
            sbQuery.Append("('" + oPRP.AssetCode1 + "','" + oPRP.SerialCode1 + "','" + oPRP.LocationCode2 + "','" + oPRP.ProcessCode2 + "','" + oPRP.WorkStation2 + "','" + oPRP.PortNo2 + "','" + oPRP.EmpName2 + "','" + oPRP.EmpCode2 + "'");
            sbQuery.Append(",'" + oPRP.AssetCode2 + "','" + oPRP.SerialCode2 + "','" + oPRP.LocationCode1 + "','" + oPRP.ProcessCode1 + "','" + oPRP.WorkStation1 + "','" + oPRP.PortNo1 + "','" + oPRP.EmpName1 + "','" + oPRP.EmpCode1 + "'");
            sbQuery.Append(",'" + oPRP.CompCode + "','" + oPRP.SwapDate + "','" + oPRP.SwappingRemarks + "')");
            int iRes=oDb.ExecuteQuery(sbQuery.ToString());
                if(iRes>0)
                    bResult=true;
            return bResult;
        }

        /// <summary>
        /// Get asset swap history details for being viewed.
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetAssetSwapHistory(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SH.[ASSET_CODE1],SH.[SERIAL_CODE1],LM1.[LOC_NAME] AS LOCATION2,PM1.[PROCESS_NAME] AS PROCESS2,SH.[WORKSTATION2],SH.[PORT2],SH.[EMPLOYEE_NAME2],");
            sbQuery.Append("SH.[ASSET_CODE2],SH.[SERIAL_CODE2],LM2.[LOC_NAME] AS LOCATION1,PM2.[PROCESS_NAME] AS PROCESS1,SH.[WORKSTATION1],SH.[PORT1],SH.[EMPLOYEE_NAME1],SH.[SWAP_DATE],SH.[REMARKS]");
            sbQuery.Append(" FROM [ASSET_SWAP_HISTORY] SH");
            sbQuery.Append(" INNER JOIN [PROCESS_MASTER] PM1 ON SH.[PROCESS2] = PM1.[PROCESS_CODE] AND SH.[COMP_CODE] = PM1.[COMP_CODE]");
            sbQuery.Append(" INNER JOIN [PROCESS_MASTER] PM2 ON SH.[PROCESS1] = PM2.[PROCESS_CODE] AND SH.[COMP_CODE] = PM2.[COMP_CODE]");
            sbQuery.Append(" INNER JOIN [LOCATION_MASTER] LM1 ON SH.[LOCATION2] = LM1.[LOC_CODE] AND SH.[COMP_CODE] = LM1.[COMP_CODE]");
            sbQuery.Append(" INNER JOIN [LOCATION_MASTER] LM2 ON SH.[LOCATION1] = LM2.[LOC_CODE] AND SH.[COMP_CODE] = LM2.[COMP_CODE] AND");
            sbQuery.Append(" SH.[COMP_CODE] = LM2.[COMP_CODE] INNER JOIN [ASSET_ACQUISITION] AA1 ON SH.[ASSET_CODE1] = AA1.[ASSET_CODE] AND");
            sbQuery.Append(" SH.[COMP_CODE] = AA1.[COMP_CODE] INNER JOIN [ASSET_ACQUISITION] AA2 ON SH.[ASSET_CODE2] = AA2.[ASSET_CODE] AND");
            sbQuery.Append(" SH.[COMP_CODE] = AA2.[COMP_CODE] WHERE SH.[COMP_CODE] = '" + CompCode + "'");  // AND AA1.[ASSET_APPROVED] = '1'
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}