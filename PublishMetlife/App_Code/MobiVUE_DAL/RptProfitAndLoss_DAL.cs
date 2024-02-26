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
using System.Data.SqlClient;

namespace MobiVUE_ATS.DAL
{
    /// <summary>
    /// Summary description for Asset Tracking Report Data Access Layer
    /// </summary>
    public class RptProfitAndLoss_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;

        public RptProfitAndLoss_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }

        ~RptProfitAndLoss_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        public DataTable GetProfitAndLossData(string CompCode, DateTime? ToDate)
        {
            try
            {
                DataTable tbl;
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "USP_ProfitAndLossReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ToDate", ToDate);
                    command.Parameters.AddWithValue("@COMP_CODE", CompCode);
                    tbl = oDb.GetDataTable(command);
                }
                return tbl;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}