using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MobiVUE_ATS.DAL
{
    /// <summary>
    /// Summary description for MailMaster_DAL
    /// </summary>
    public class MailMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public MailMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~MailMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        public DataTable GetMailTransactionDetails(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM MAIL_MASTER WHERE  COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetMailTransactionDetails(string TransactionType, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM MAIL_MASTER WHERE TRANSACTION_TYPE='" + TransactionType + "' AND COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        
        }

        public string SaveMailMaster(MailMaster_PRP oPRP)
        {
            try
            {                
                DataTable dt = oDb.ExecuteSPWithOutput("SP_SAVE_MAIL_MASTER", new SqlParameter("TRANSACTION_TYPE", oPRP.TransactionType),
                            new SqlParameter("TO_MAIL_ID", oPRP.ToMailAddress),
                            new SqlParameter("CC_MAIL_ID", oPRP.CCMailAddress),
                            new SqlParameter("MAIL_SUBJECT", oPRP.MailSubject),
                            new SqlParameter("MAIL_BODY", oPRP.MailBody),
                            new SqlParameter("COMP_CODE", oPRP.CompCode),
                            new SqlParameter("REMARKS", oPRP.Remarks),
                            new SqlParameter("USER_ID", oPRP.UserID));

                return Convert.ToString(dt.Rows[0][0]);
            }
            catch (Exception ex)
            { return ex.Message; }
        }
    }
}