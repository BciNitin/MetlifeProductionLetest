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
    /// Summary description for VendorMaster_DAL
    /// </summary>
    public class VendorMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public VendorMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~VendorMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Check duplicate vendor code.
        /// </summary>
        /// <param name="_VendorCode"></param>
        /// <returns></returns>
        private bool CheckDuplicateVendor(string _VendorCode,string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM VENDOR_MASTER WHERE VENDOR_CODE = '" + _VendorCode.Trim().Replace("'", "''") + "'");
               // sbQuery.Append(" AND COMP_CODE='" + _CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// save/Update vendor details into data tables.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveUpdateVendorMaster(string OpType, VendorMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateVendor(oPRP.VendorCode,oPRP.CompCode))
                    {
                        //Save New Vendor...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [VENDOR_MASTER] ([VENDOR_CODE],[VENDOR_NAME],[VENDOR_ADDRESS],[VENDOR_COUNTRY],[VENDOR_STATE]");
                        sbQuery.Append(",[VENDOR_CITY],[VENDOR_PIN],[VENDOR_CONT_PERSON],[VENDOR_PHONE],[VENDOR_EMAIL],[ACTIVE],[REMARKS],[COMP_CODE],[WORK_CATAGORY],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES");
                        sbQuery.Append(" ('" + oPRP.VendorCode + "','" + oPRP.VendorName + "','" + oPRP.VendorAddress + "','" + oPRP.VendorCountry + "','" + oPRP.VendorSate + "',");
                        sbQuery.Append(" '" + oPRP.VendorCity + "','" + oPRP.VendorPIN + "','" + oPRP.VendorContPerson + "','" + oPRP.VendorPhone + "','" + oPRP.VendorEmail + "','" + oPRP.Active + "','" + oPRP.Remarks + "','" + oPRP.CompCode + "','"+ oPRP.WorkCatagory +"','" + oPRP.CreatedBy + "',GETDATE())");
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update Vendor Information...
                    sbQuery=new StringBuilder();
                    sbQuery.Append("UPDATE [VENDOR_MASTER] SET [VENDOR_NAME] = '" + oPRP.VendorName + "',[VENDOR_ADDRESS] = '" + oPRP.VendorAddress + "'");
                    sbQuery.Append(" ,[VENDOR_COUNTRY] = '" + oPRP.VendorCountry + "',[VENDOR_STATE] = '" + oPRP.VendorSate + "',[VENDOR_CITY] = '" + oPRP.VendorCity + "',[VENDOR_PIN] = '" + oPRP.VendorPIN + "', [WORK_CATAGORY] = '" + oPRP.WorkCatagory + "'");
                    sbQuery.Append(" ,[VENDOR_CONT_PERSON] = '" + oPRP.VendorContPerson + "',[VENDOR_PHONE] = '" + oPRP.VendorPhone + "',[VENDOR_EMAIL] = '" + oPRP.VendorEmail + "',[ACTIVE] = '" + oPRP.Active + "',[REMARKS] = '" + oPRP.Remarks + "'");
                    sbQuery.Append(" ,[MODIFIED_BY]='" + oPRP.ModifiedBy + "',MODIFIED_ON=GETDATE()");
                    sbQuery.Append(" WHERE [VENDOR_CODE] = '" + oPRP.VendorCode + "' AND [COMP_CODE]='" + oPRP.CompCode + "'");
                }
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Upload vendor details from excel file.
        /// </summary>
        /// <param name="oPRP"></param>
        public void UploadVendorDetails(VendorMaster_PRP oPRP)
        {
            try
            {

                if (!CheckDuplicateVendor(oPRP.VendorCode, oPRP.CompCode))
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("INSERT INTO [VENDOR_MASTER] ([VENDOR_CODE],[VENDOR_NAME],[VENDOR_ADDRESS],[VENDOR_COUNTRY],[VENDOR_STATE]");
                    sbQuery.Append(",[VENDOR_CITY],[VENDOR_PIN],[VENDOR_CONT_PERSON],[VENDOR_PHONE],[VENDOR_EMAIL],[ACTIVE],[REMARKS],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                    sbQuery.Append(" VALUES");
                    sbQuery.Append(" ('" + oPRP.VendorCode + "','" + oPRP.VendorName + "','" + oPRP.VendorAddress + "','" + oPRP.VendorCountry + "','" + oPRP.VendorSate + "',");
                    sbQuery.Append(" '" + oPRP.VendorCity + "','" + oPRP.VendorPIN + "','" + oPRP.VendorContPerson + "','" + oPRP.VendorPhone + "','" + oPRP.VendorEmail + "','" + oPRP.Active + "','" + oPRP.Remarks + "','" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                    oDb.ExecuteQuery(sbQuery.ToString());
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Fetch all vendors details.
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendorDetails(string _CompCode)
        {
            sbQuery=new StringBuilder();
            sbQuery.Append("SELECT [VENDOR_CODE],[VENDOR_NAME],[VENDOR_ADDRESS],[VENDOR_COUNTRY],[VENDOR_STATE],[VENDOR_CITY],[VENDOR_PIN]");
            sbQuery.Append(",[VENDOR_CONT_PERSON],[VENDOR_PHONE],[VENDOR_EMAIL],[ACTIVE],[REMARKS],[CREATED_BY],[CREATED_ON]");
            sbQuery.Append("FROM [VENDOR_MASTER] WHERE [COMP_CODE]='" + _CompCode + "' ORDER BY [VENDOR_NAME]");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Delete a particular vendor details.
        /// </summary>
        /// <param name="_VendorCode"></param>
        /// <returns></returns>
        public bool DeleteVendor(string _VendorCode,string _CompCode)
        {
            try
            {
                bool bResult = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM VENDOR_MASTER WHERE VENDOR_CODE='" + _VendorCode + "' AND COMP_CODE='" + _CompCode + "'");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Fetch all country names.
        /// </summary>
        /// <returns></returns>
        public DataTable GetCountry()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GENERAL_CODE,GENERAL_NAME FROM GENERAL_MASTER WHERE STATE_NAME='' AND COUNTRY_NAME='' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch all state names in a country.
        /// </summary>
        /// <param name="_CountryName"></param>
        /// <returns></returns>
        public DataTable GetState(string _CountryName)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GENERAL_CODE,GENERAL_NAME FROM GENERAL_MASTER WHERE COUNTRY_NAME='" + _CountryName + "' AND STATE_NAME='' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch all city names in a state.
        /// </summary>
        /// <param name="_StateName"></param>
        /// <returns></returns>
        public DataTable GetCity(string _StateName)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GENERAL_CODE,GENERAL_NAME FROM GENERAL_MASTER WHERE STATE_NAME='" + _StateName + "' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}