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
    /// Summary description for Vendoe Escalation Matrix Data Access Layer
    /// </summary>
    public class VendorEscalationMatrix_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public VendorEscalationMatrix_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~VendorEscalationMatrix_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Save/Update Vendor Escalation Matrix details
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns>bResult</returns>
        public bool SaveUpdateVendorEscalationMatrix(string OpType, VendorEscalationMatrix_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateVEM(oPRP.VEMVendorCode, oPRP.CompCode))
                    {
                        //Save new vendor escalation matrix details...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [VENDOR_ESCALATION_MATRIX] ([VENDOR_CODE],[VEM_PERSON_NAME],[VEM_EMAIL],[VEM_MOBILE],[VEM_ADDRESS]");
                        sbQuery.Append(",[VEM_LEVEL],[VEM_REMARKS],[VEM_SUPPORT_TYPE],[VEM_ACTIVE],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES ");
                        sbQuery.Append("('" + oPRP.VEMVendorCode + "','" + oPRP.VEMPersonName + "','" + oPRP.VEMEmail + "','" + oPRP.VEMMobile + "','" + oPRP.VEMAddress + "'");
                        sbQuery.Append(",'" + oPRP.VEMLevel + "','" + oPRP.VEMRemarks + "','" + oPRP.VEMSupportType + "','" + oPRP.VEMActive + "','" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update vendor escalation matrix details...
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [VENDOR_ESCALATION_MATRIX] SET [VENDOR_CODE] = '" + oPRP.VEMVendorCode + "',[VEM_PERSON_NAME] = '" + oPRP.VEMPersonName + "',[VEM_EMAIL] = '" + oPRP.VEMEmail + "',[VEM_MOBILE] = '" + oPRP.VEMMobile + "'");
                    sbQuery.Append(",[VEM_ADDRESS] = '" + oPRP.VEMAddress + "',[VEM_LEVEL] = '" + oPRP.VEMLevel + "',[VEM_SUPPORT_TYPE] = '" + oPRP.VEMSupportType + "'");
                    sbQuery.Append(",[VEM_ACTIVE] = '" + oPRP.VEMActive + "',[MODIFIED_BY] = '" + oPRP.ModifiedBy + "',[MODIFIED_ON] = GETDATE()");
                    sbQuery.Append(" WHERE VEM_CODE=" + oPRP.VEMCode + "");
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
        /// Check duplicate vendoe escalation matrix code
        /// </summary>
        /// <param name="_VEMCode"></param>
        /// <returns></returns>
        private bool CheckDuplicateVEM(string _VEMVendorCode,string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM VENDOR_ESCALATION_MATRIX WHERE VENDOR_CODE='" + _VEMVendorCode + "' AND COMP_CODE='" + _CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Get vendor details
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendor(string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT VENDOR_CODE,VENDOR_NAME FROM VENDOR_MASTER WHERE COMP_CODE='" + _CompCode + "' AND ACTIVE='1' ORDER BY VENDOR_NAME");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Get vendoe escalation matrix details
        /// </summary>
        /// <returns></returns>
        public DataTable GetVenEscMatrixDetails(string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT VEM.[VEM_CODE],VM.VENDOR_NAME,VEM.[VEM_PERSON_NAME],VEM.[VEM_EMAIL],VEM.[VEM_MOBILE],VEM.[VEM_ADDRESS],VEM.[VEM_LEVEL]");
                sbQuery.Append(",VEM.[VEM_REMARKS],VEM.[VEM_SUPPORT_TYPE],VEM.[VEM_ACTIVE]");
                sbQuery.Append(" FROM [VENDOR_ESCALATION_MATRIX] VEM INNER JOIN VENDOR_MASTER VM");
                sbQuery.Append(" ON VEM.VENDOR_CODE = VM.VENDOR_CODE WHERE VEM.[COMP_CODE]='" + _CompCode + "'");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Delete Vendor Escalation Matrix details.
        /// </summary>
        /// <param name="VEMCode"></param>
        /// <returns></returns>
        public bool DeleteVEMDetails(int VEMCode,string CompCode)
        {
            try
            {
                bool bResult = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM VENDOR_ESCALATION_MATRIX WHERE VEM_CODE=" + VEMCode + " AND COMP_CODE='" + CompCode + "'");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataTable GetVendorDetails(string VendorCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT VENDOR_PHONE,VENDOR_CONT_PERSON FROM [VENDOR_MASTER] ");
            sbQuery.Append(" WHERE [VENDOR_CODE]='" + VendorCode + "' AND COMP_CODE='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}