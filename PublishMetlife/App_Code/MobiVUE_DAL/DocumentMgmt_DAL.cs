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
    /// Summary description for DocumentMgmt_DAL
    /// </summary>
    public class DocumentMgmt_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public DocumentMgmt_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~DocumentMgmt_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Save/update document management details.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveUpdateFileDetails(string OpType, DocumentMgmt_PRP oPRP)
        {
            bool bResult = false;
            if (OpType == "SAVE")
            {
                if (!CheckDuplicateFileName(oPRP.AttachFileName))
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("INSERT INTO [DOCUMENT_MANAGEMENT] ([DESCRIPTION],[CATEGORY],[REMARKS],[ATTACH_FILE_NAME],[CREATED_BY],[CREATED_ON],[COMP_CODE],[COMPANY_NAME])");
                    sbQuery.Append("VALUES ('" + oPRP.Description + "','" + oPRP.Category + "','" + oPRP.Remarks + "'");
                    sbQuery.Append(",'" + oPRP.AttachFileName + "','" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.CompCode + "','" + oPRP.CompanyName + "')");
                }
            }
            if (OpType == "UPDATE")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE [DOCUMENT_MANAGEMENT] SET [DESCRIPTION]='" + oPRP.Description + "',[CATEGORY]='" + oPRP.Category + "',[REMARKS]='" + oPRP.Remarks + "'");
                sbQuery.Append(" WHERE SERIAL_NO = " + oPRP.SerialNo + " AND COMP_CODE = '" + oPRP.CompCode + "'");
            }
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResult = true;
            return bResult;
        }

        /// <summary>
        /// Checking duplicate file name to be saved.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CheckDuplicateFileName(string FileName)
        {
            bool bDup = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM [DOCUMENT_MANAGEMENT] WHERE [ATTACH_FILE_NAME]='" + FileName + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                bDup = true;
            return bDup;
        }

        /// <summary>
        /// Getting document management details to be populated for being viewed.
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable GetFileDetails(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT [SERIAL_NO],[COMP_CODE],[COMPANY_NAME],[DESCRIPTION],[CATEGORY],[REMARKS],[ATTACH_FILE_NAME]");
            sbQuery.Append(" FROM [DOCUMENT_MANAGEMENT]");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Deleting uploaded file and document management details as well.
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public bool DeleteFileDetails(int SerialNo, string CompCode)
        {
            bool bResp=false;
            sbQuery = new StringBuilder();
            sbQuery.Append("DELETE FROM [DOCUMENT_MANAGEMENT] WHERE [SERIAL_NO]=" + SerialNo + " AND COMP_CODE='" + CompCode + "'");
            int iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResp = true;
            return bResp;
        }
    }
}