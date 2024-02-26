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
using MobiVUE_ATS.DAL;

/// <summary>
/// Summary description for AssetReconcilation
/// </summary>
public class AssetReconcilation_DAL
{
    clsDb oDb;
    StringBuilder sbQuery;
    public AssetReconcilation_DAL(string DatabaseType)
    {
        oDb = new clsDb();
        if (DatabaseType != "")
            oDb.Connect(DatabaseType);
    }
    ~AssetReconcilation_DAL()
    {
        oDb.Disconnect();
        oDb = null;
        sbQuery = null;
    }

    public DataTable ReconcilationData(string RecId, string Location, string Floor, string Store, string Fromdate, string ToDate,string Operator, string Type, string CompCode)
    {
        sbQuery = new StringBuilder();
        sbQuery.AppendLine(" EXEC [sp_ReconcilationData]  '" + RecId + "' , '" + Location + "', '" + Floor + "', '" + Store + "', ");
        sbQuery.AppendLine(" '" + Fromdate + "' , '" + ToDate + "', '" + Operator + "', '" + Type + "', '" + CompCode + "' ");
        return oDb.GetDataTable(sbQuery.ToString());
    }
    public DataTable GetSite(string _CompCode)
    {
        try
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SITE_CODE, SITE_ADDRESS from SITE_MASTER WHERE ACTIVE = 1 AND SITE_CODE<>'ALL' AND COMP_CODE = '" + _CompCode + "' ");

            return oDb.GetDataTable(sbQuery.ToString());
        }
        catch (Exception ex)
        { throw ex; }
    }

    public DataTable GetFloor(string SiteCode, string CompCode)
    {
        try
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT FLOOR_CODE,FLOOR_NAME FROM FLOOR_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        catch (Exception ex)
        { throw ex; }
    }
    public DataTable GetStore(string SiteCode,string Floor, string CompCode)
    {
        try
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT STORE_CODE,STORE_NAME FROM store_master WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND FLOOR_CODE='"+ Floor + "' AND COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        catch (Exception ex)
        { throw ex; }
    }
    /// <summary>
    /// 
    /// </summary>
    public bool UploadFileData(string Code, string LocCode, string CodeType, string ReconciledBy, string ReconcileDate, string CompCode)
    {
        int iRes = 0;
        bool bResult = false;
        if (CodeType == "ASSET")
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("INSERT INTO [RECONCILED_ASSET_CODES] ([ASSET_CODE],[LOC_CODE],[RECONCILE_DATE],[RECONCILE_TIME],[RECONCILED_BY],[COMP_CODE])");
            sbQuery.AppendLine("VALUES ");
            sbQuery.AppendLine("('" + Code.Trim() + "','" + LocCode + "','" + ReconcileDate + "',CONVERT(DATETIME,GETDATE(),108),'" + ReconciledBy + "','" + CompCode + "')");
            iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResult = true;
        }
        else
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("INSERT INTO [RECONCILED_SERIAL_CODES] ([SERIAL_CODES],[LOC_CODE],[RECONCILE_DATE],[RECONCILE_TIME],[RECONCILED_BY],[COMP_CODE])");
            sbQuery.AppendLine("VALUES ");
            sbQuery.AppendLine("('" + Code.Trim() + "','" + LocCode + "','" + ReconcileDate + "',CONVERT(DATETIME,GETDATE(),108),'" + ReconciledBy + "','" + CompCode + "')");
            iRes = oDb.ExecuteQuery(sbQuery.ToString());
            if (iRes > 0)
                bResult = true;
        }
        return bResult;
    }

    /// <summary>
    /// 
    /// </summary>
    public DataSet GetFileData(string Date)
    {
        sbQuery = new StringBuilder();
        sbQuery.AppendLine("SELECT DISTINCT RAC.TAG_ID,LM.SITE_CODE AS LOC_NAME,RAC.RECONCILED_BY FROM RECONCILED_ASSET_CODES RAC");
        sbQuery.AppendLine(" INNER JOIN SITE_MASTER LM ON RAC.LOC_CODE = LM.SITE_CODE ");
        sbQuery.AppendLine(" WHERE RAC.RECONCILE_TIME = '" + Date + "' ; ");

        //sbQuery.AppendLine("SELECT DISTINCT RSC.SERIAL_CODES, LM.LOC_NAME, CONVERT(VARCHAR,RSC.RECONCILE_DATE,106) AS RECONCILE_DATE,RSC.RECONCILED_BY");
        //sbQuery.AppendLine("FROM RECONCILED_SERIAL_CODES RSC INNER JOIN LOCATION_MASTER LM ON RSC.LOC_CODE = LM.LOC_CODE");
        //sbQuery.AppendLine("WHERE RSC.RECONCILE_DATE = '" + DateTime.Now.ToString("dd/MMM/yyyy") + "'");
        return oDb.GetDataSet(sbQuery.ToString());
    }

    /// <summary>
    /// Get assets reconciled data from the asset acquisition table.
    /// </summary>
    public DataTable GetReconsolidatedData(string CompCode, string AssetType)
    {
        sbQuery = new StringBuilder();
        sbQuery.AppendLine("SELECT Tag_ID,ASSET_CODE,LOC_NAME,[STATUS],COMP_CODE FROM");
        sbQuery.AppendLine("(SELECT Tag_ID,ASSET_CODE,LOC_NAME,COMP_CODE, CASE WHEN RECO IS NULL THEN 'NOT SCANNED YET'");
        sbQuery.AppendLine("WHEN Tag_ID IS NULL THEN 'NOT FOUND IN SYSTEM' ELSE 'SCANNED' END AS [STATUS] FROM");
        sbQuery.AppendLine("(SELECT A.Tag_ID AS RECO, B.TAG_ID, B.ASSET_CODE, X.SITE_CODE AS LOC_NAME, B.COMP_CODE FROM RECONCILED_ASSET_CODES AS A LEFT JOIN ASSET_ACQUISITION AS B INNER JOIN SITE_MASTER AS X ON B.ASSET_LOCATION = X.SITE_CODE");
        sbQuery.AppendLine("ON A.TAG_ID = B.TAG_ID WHERE B.SOLD_SCRAPPED_STATUS IS NULL UNION SELECT B.TAG_ID AS RECO ,A.TAG_ID,A.ASSET_CODE,Z.SITE_CODE AS LOC_NAME,");
        sbQuery.AppendLine("A.COMP_CODE FROM ASSET_ACQUISITION AS A LEFT JOIN RECONCILED_ASSET_CODES AS B ON A.TAG_ID = B.TAG_ID INNER JOIN SITE_MASTER AS Z ON A.ASSET_LOCATION = Z.SITE_CODE) AS X) AS Y");
        sbQuery.AppendLine("WHERE COMP_CODE='" + CompCode + "'  ORDER BY [STATUS] DESC");
        return oDb.GetDataTable(sbQuery.ToString());
    }

    /// <summary>
    /// Get complete asset reconciled data
    /// </summary>
    public DataTable GetPreReconciledData(string CompCode, string AssetType, string ReconcileDate)
    {
        sbQuery = new StringBuilder();

        sbQuery.AppendLine("SELECT ASSET_CODE,SERIAL_CODE,ASSET_ID,LOC_NAME,PORT_NO,[STATUS],COMP_CODE FROM");
        sbQuery.AppendLine("(SELECT ASSET_CODE,RECONCILE_DATE,SERIAL_CODE,ASSET_ID,LOC_NAME,PORT_NO,COMP_CODE,ASSET_TYPE, CASE WHEN RECO IS NULL THEN 'NOT SCANNED YET'");
        sbQuery.AppendLine("WHEN ASSET_CODE IS NULL THEN 'NOT FOUND IN SYSTEM' ELSE 'OK' END AS [STATUS] FROM");
        sbQuery.AppendLine("(SELECT A.ASSET_CODE AS RECO,A.RECONCILE_DATE, B.SERIAL_CODE, B.ASSET_CODE, B.ASSET_ID, X.LOC_NAME, B.PORT_NO,B.COMP_CODE,B.ASSET_TYPE FROM RECONCILED_ASSET_CODES AS A LEFT JOIN ASSET_ACQUISITION AS B INNER JOIN LOCATION_MASTER AS X ON B.ASSET_LOCATION = X.LOC_CODE");
        sbQuery.AppendLine("ON A.ASSET_CODE = B.ASSET_CODE WHERE B.SOLD_SCRAPPED_STATUS IS NULL UNION SELECT B.ASSET_CODE AS RECO,B.RECONCILE_DATE,A.SERIAL_CODE,A.ASSET_CODE,A.ASSET_ID,Z.LOC_NAME,A.PORT_NO,");
        sbQuery.AppendLine("A.COMP_CODE,A.ASSET_TYPE FROM ASSET_ACQUISITION AS A LEFT JOIN RECONCILED_ASSET_CODES AS B ON A.ASSET_CODE = B.ASSET_CODE INNER JOIN LOCATION_MASTER AS Z ON A.ASSET_LOCATION = Z.LOC_CODE) AS X) AS Y");
        sbQuery.AppendLine("WHERE COMP_CODE='" + CompCode + "' AND (RECONCILE_DATE IS NULL OR RECONCILE_DATE='" + ReconcileDate + "') ORDER BY [STATUS] DESC");
        return oDb.GetDataTable(sbQuery.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    public DataTable GetMaxReconcileDate(string CompCode)
    {
        sbQuery = new StringBuilder();
        sbQuery.AppendLine("SELECT ISNULL(MAX(RECONCILE_DATE),'') AS MAXRECONDATE FROM RECONCILED_ASSET_CODES WHERE COMP_CODE='" + CompCode + "'");
        return oDb.GetDataTable(sbQuery.ToString());
    }
    public string ConfirmReconcileData(string CompCode, string ReconcilationDate)
    {
        string status = "";
        sbQuery = new StringBuilder();
        sbQuery.Append("EXEC Usp_ReConcileData '" + ReconcilationDate + "','" + CompCode + "'");
        DataTable dt = oDb.GetDataTable(sbQuery.ToString());
        if (dt.Rows.Count > 0)
        {
            status = Convert.ToString(dt.Rows[0][0]);
        }
        return status;

    }
    /// <summary>
    /// Checks duplicate asset code for the same date reconciliation.
    /// </summary>
    public bool bChkDuplicate(string AssetCode)
    {
        bool bDuplicate = false;
        sbQuery = new StringBuilder();
        sbQuery.AppendLine("SELECT COUNT(*) AS DUP FROM [RECONCILED_ASSET_CODES] WHERE [TAG_ID]='" + AssetCode + "'");
        sbQuery.AppendLine("AND [RECONCILE_DATE]='" + DateTime.Now.ToString("dd/MMM/yyyy") + "'");
        DataTable dt = oDb.GetDataTable(sbQuery.ToString());
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["DUP"].ToString() != "0")
                bDuplicate = true;
        }
        return bDuplicate;
    }
}