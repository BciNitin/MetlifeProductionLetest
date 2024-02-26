using MobiVUE_ATS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
/// <summary>
/// Summary description for AssetImageUpload_DAL
/// </summary>
public class AssetImageUpload_DAL
{

    clsDb oDb;
    StringBuilder sbQuery;
    public AssetImageUpload_DAL(string DatabaseType)
    {
        oDb = new clsDb();
        if (DatabaseType != "")
            oDb.Connect(DatabaseType);
    }
    ~AssetImageUpload_DAL()
    {
        oDb.Disconnect();
        oDb = null;
        sbQuery = null;
    }

    public DataTable GetAssertPendingUploadImage(string AssetPONumber, string AssetInvoiceNo,string CompCode,int displaytype)
    {
        DataTable dt = new DataTable();
        try
        {
            ////dt = oDb.ExecuteSPWithOutput("SP_ASSET_ACQUISITION_PENDING", new SqlParameter("PO_NUMBER", AssetPONumber),
            ////    new SqlParameter("INVOICE_NO", AssetInvoiceNo), new SqlParameter("Compcode", CompCode));
            //return dt;
            sbQuery = new StringBuilder();
            sbQuery.Append("EXEC SP_ASSET_ACQUISITION_PENDING '" + AssetPONumber + "','" + AssetInvoiceNo + "','" + displaytype + "','" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetMailTransactionDetails(string TransactionType, string CompCode)
    {
        sbQuery = new StringBuilder();
        sbQuery.Append("SELECT * FROM MAIL_MASTER WHERE TRANSACTION_TYPE='" + TransactionType + "' AND COMP_CODE = '" + CompCode + "'");
        return oDb.GetDataTable(sbQuery.ToString());

    }
    public DataTable PopulateLocationName(string LocationCode)
    {
        sbQuery = new StringBuilder();
        sbQuery.AppendLine("SELECT DISTINCT SITE_CODE from SITE_MASTER WHERE [SITE_CODE] ='" + LocationCode + "' AND  ACTIVE='1'");
        return oDb.GetDataTable(sbQuery.ToString());
    }

    public DataTable GetSiteLocation()
    {
        sbQuery = new StringBuilder();
        sbQuery.Append("select distinct SITE_CODE from SITE_MASTER WHERE SITE_CODE<>'ALL' ");
        return oDb.GetDataTable(sbQuery.ToString());
    }

    public string UpdateAssetImage(string InvoiceNumber,string FilePath, string CompCode,string CreatedBy)
    {
        try
        {
            DataTable dt = oDb.ExecuteSPWithOutput("SP_UPDATE_ASSET_ACQUISITION_IMAGE",  
                new SqlParameter("@INVOICE_NO", InvoiceNumber),
                new SqlParameter("FILEPATH", FilePath), 
                new SqlParameter("@Compcode", CompCode),
                new SqlParameter("@CreatedBy", CreatedBy));
            return Convert.ToString(dt.Rows[0][0]);
        }
        catch (Exception ex)
        { return ex.Message; }
    }
}