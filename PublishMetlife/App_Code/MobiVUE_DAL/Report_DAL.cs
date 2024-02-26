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
/// <summary>
/// Summary description for Report_DAL
/// </summary>
/// 
namespace MobiVUE_ATS.DAL
{
    public class Report_DAL
    {

        clsDb oDb;
        StringBuilder sbQuery;
        public Report_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~Report_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        public DataTable GetReportDetails(Report_PRP rpt)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("EXEC USP_REPORT '" + rpt.Type + "','" + rpt.FromDate + "','" + rpt.ToDate + "','" + rpt.CompCode + "','"+rpt.TagID + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        //
        // TODO: Add constructor logic here
        //

        public DataTable GetReportDetails(int ReportID)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = oDb.ExecuteSPWithOutput("SP_GET_REPORT_DETAILS", new SqlParameter("REPORT_ID", ReportID));
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetReportAssetStockinHomePage(string _CompCode)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = oDb.ExecuteSPWithOutput("sp_ReportGetAssetStockReportInHomePage", new SqlParameter("COMP_CODE", _CompCode));
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetReportAssetAllocatedinHomePage(string _CompCode)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = oDb.ExecuteSPWithOutput("sp_ReportGetAssetAllocatedReportInHomePage", new SqlParameter("COMP_CODE", _CompCode));
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetReportAssetTransferinHomePage(string _CompCode)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = oDb.ExecuteSPWithOutput("sp_ReportGetAssetTransferReportInHomePage", new SqlParameter("COMP_CODE", _CompCode));
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetReportAssetScrappedinHomePage(string _CompCode)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = oDb.ExecuteSPWithOutput("sp_ReportGetAssetScrappedReportInHomePage", new SqlParameter("COMP_CODE", _CompCode));
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetStoreReportDetails(string _ReportName,string _CompCode)
        {
            DataTable dt = new DataTable();
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT ASSET_CODE AS [Asset Code],SERIAL_CODE AS [Serial Number],ASSET_LOCATION AS [Site Location],FLOOR as [Floor],STORE AS [Store],STATUS as [Status],ASSET_SUB_STATUS as [Sub Status] FROM ASSET_ACQUISITION AA INNER JOIN STORE_MASTER SM ON SM.STORE_NAME = AA.STORE WHERE STORE_NAME ='" + _ReportName + "' AND SM.COMP_CODE = '"+ _CompCode + "' ");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataTableInTransaction(string SqlQuery)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = oDb.GetDataTableInTransaction(SqlQuery);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable GetReportData(int ReportID, DataTable dt)
        {
            try
            {
                return oDb.ExecuteSPWithOutput("SP_REPORT_DATA",
                       new SqlParameter("REPORT", dt),
                       new SqlParameter("Report_ID", ReportID));
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

    }
}