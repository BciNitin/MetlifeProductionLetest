using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using Excel = Microsoft.Office.Interop.Excel;

public partial class ReportReturnableAssets : System.Web.UI.Page
{
    RptAssetStock_DAL oDAL;
    public ReportReturnableAssets()
    {
        
    }
    ~ReportReturnableAssets()
    {
        oDAL = null;
    }

    #region PAGE EVENTS
    /// <summary>
    /// Navigates to session expired page in case of user logs off/session expired.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new RptAssetStock_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user permissions in order to view asset allocation report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("RETURNABLE_ASSET_REPORT", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "RETURNABLE_ASSET_REPORT");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                ChkRtnDateExpired.Checked = false;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Returnable Asset Report");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    #endregion

    #region SUBMIT EVENT
    /// <summary>
    /// Get returnable asset report details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            gvRptReturnAsset.DataSource = null;
            DataTable dt = new DataTable();
            dt = oDAL.GetReturnableAssetReport(ChkRtnDateExpired.Checked, Session["COMPANY"].ToString());
            if (dt.Rows.Count > 0)
            {
                gvRptReturnAsset.DataSource = Session["RptReturnAsset"] = dt;
                gvRptReturnAsset.DataBind();
                gvRptReturnAsset.Visible = true;
                btnExport.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no record for selected criteria.');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Reset/Clear page fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            gvRptReturnAsset.DataSource = null;
            gvRptReturnAsset.Visible = false;
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Export gridview report data into excel sheet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[4] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (gvRptReturnAsset.Rows.Count > 0)
            {
                DataTable dt = (DataTable)Session["RptReturnAsset"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //this.EnableViewState = false;
                //Response.Write(tw.ToString());
                //Response.End();
                using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    wb.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=REPORT" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no data to export.');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvRptReturnAsset_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["RptReturnAsset"];
            gvRptReturnAsset.PageIndex = e.NewPageIndex;
            gvRptReturnAsset.DataSource = dt;
            gvRptReturnAsset.DataBind();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion
}
