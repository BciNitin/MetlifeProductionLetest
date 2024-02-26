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
    /// Summary description for AssetMaster_DAL
    /// </summary>
    public class AssetMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public AssetMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~AssetMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveAssetMaster(AssetMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                if (!CheckDuplicateAsset(oPRP.AssetCode))
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("INSERT INTO [ASSET_MASTER] ([ASSET_CODE],[ASSET_NAME],[CATEGORY_CODE],[LOCATION_CODE],[BRAND_NAME],[MAKE_NAME]");
                    sbQuery.Append(" ,[MODEL_NAME],[SERIAL_CODE],[FINANCE_ASSET_TAG],[ALLOCATED_EMP_NAME],[CREATED_BY],[CREATED_ON]");
                    sbQuery.Append(" ,[ASSET_TYPE],[ASSET_SUB_TYPE],[PROCESS_NAME],[W/S#],[HDD],[RAM],[PROCESSOR],[AMC_DATE]");
                    sbQuery.Append(" ,[CARTRIDGE_TONER_NO],[P_D_NO],[IMEI_NO],[DEPT_CODE],[ASSET_PIN],[ASSET_COMMENTS],[SERVER_NAME]");
                    sbQuery.Append(" ,[CPU],[SPEED],[IMPORT_REG_NO],[WAREHOUSE_DATE],[SERVER_TYPE],[SERVER_REMARKS],[SEVICE_PROVIDER]");
                    sbQuery.Append(" ,[OWNER],[SECURITY_CLASSIFICATION])");
                    sbQuery.Append(" VALUES");
                    sbQuery.Append(" ('" + oPRP.AssetCode + "','" + oPRP.AssetName + "','" + oPRP.AssetCategoryCode + "','" + oPRP.AssetLocationCode + "','" + oPRP.AssetBrandName + "','" + oPRP.AssetMakeName + "',");
                    sbQuery.Append(" '" + oPRP.AssetModelName + "','" + oPRP.AssetSerialNo + "','" + oPRP.FinanceAssetTag + "','" + oPRP.AssetAllocatedTo + "','" + oPRP.CreatedBy + "',GETDATE(),");
                    sbQuery.Append(" '" + oPRP.AssetType + "','" + oPRP.AssetSubType + "','" + oPRP.AssetProcess + "','" + oPRP.AssetWSNo + "','" + oPRP.AssetHDD + "','" + oPRP.AssetRAM + "','" + oPRP.AssetProcessor + "','" + oPRP.AssetAMCDate + "'");
                    sbQuery.Append(" ,'" + oPRP.CartridgeTonerNo + "','" + oPRP.PDNo + "','" + oPRP.IMEINo + "','" + oPRP.DeptCode + "','" + oPRP.AssetPIN + "','" + oPRP.Comments + "','" + oPRP.ServerName + "'");
                    sbQuery.Append(" ,'" + oPRP.ServerCPU + "','" + oPRP.ServerSpeed + "','" + oPRP.ServerImpRegNo + "','" + oPRP.ServerWHDate + "','" + oPRP.ServerType + "','" + oPRP.ServerRemarks + "','" + oPRP.ServiceProvider + "'");
                    sbQuery.Append(" ,'" + oPRP.AssetOwner + "','" + oPRP.AssetSecurityClass + "')");

                    int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                    if (iRes > 0)
                        bResult = true;
                }
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <returns></returns>
        private bool CheckDuplicateAsset(string _AssetCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_MASTER WHERE ASSET_CODE = '" + _AssetCode.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateCategory()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT CATEGORY_CODE,CATEGORY_NAME FROM dbo.CATEGORY_MASTER");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns></returns>
        public DataTable PopulateLocation(string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT LOC_CODE, LOC_NAME FROM dbo.LOCATION_MASTER");
            sbQuery.Append(" WHERE PARENT_LOC_CODE LIKE '" + _ParentLocCode + "%' AND LOC_LEVEL=" + _LocLevel + "");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssetType()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT ASSET_TYPE FROM dbo.ASSET_TYPE");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_AssetType"></param>
        /// <returns></returns>
        public DataTable GetAssetSubType(string _AssetType)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_TYPE_CODE,ASSET_SUB_TYPE FROM ASSET_TYPE WHERE ASSET_TYPE='" + _AssetType + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepartment()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DEPT_CODE,DEPT_NAME FROM dbo.DEPARTMENT_MASTER");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}