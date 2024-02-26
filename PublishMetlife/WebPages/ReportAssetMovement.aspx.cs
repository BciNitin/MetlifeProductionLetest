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

public partial class UploadedFiles_WebPages_ReportAssetMovement : System.Web.UI.Page
{
    bool bExpire = false;
    Report_DAL oDAL;
    Report_PRP oPRP;
    public UploadedFiles_WebPages_ReportAssetMovement()
    {
        oPRP = new Report_PRP();
    }
    ~UploadedFiles_WebPages_ReportAssetMovement()
    {
        oPRP = null;
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
        oDAL = new Report_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user permissions for gatepass report view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("ASSET_MOVEMENT_REPORT", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_MOVEMENT_REPORT");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
               
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Get gatepass generation report
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //if (clsGeneral._strRights[0] == "0")
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
            //    return;
            //}

            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Asset Allocation", "Asset Movement Report", "Asset Movement Report generate by user id" + Session["CURRENTUSER"].ToString() );
            int iDate = clsGeneral.CompareDate(txtFromDate.Text.Trim(), txtToDate.Text.Trim());
            if (iDate > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : From Date should be earlier than To Date.');", true);
                return;
            }
            GetReports();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
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
            gvRptGatePass.DataSource = null;
            gvRptGatePass.Visible = false;
            txtFromDate.Text = "";
            txtToDate.Text = "";
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export report data from gridview to excel worksheet.
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
            if (gvRptGatePass.Rows.Count > 0)
            {
                DataTable dt = (DataTable)Session["RptAM"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'>Asset Movement Report</font></u></b>");
                //hw.WriteLine("<br>");
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=AssetMovement.xls");
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
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh/reset location dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshLocation_Click(object sender, ImageClickEventArgs e)
    {
        //try
        //{
        //    lblLocLevel.Text = "1";
        //    PopulateLocation();
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Get gate pass report based on parameters provided.
    /// </summary>
    private void GetReports()
    {
        

        if (txtFromDate.Text != "")
            oPRP.FromDate = txtFromDate.Text.Trim();
        else
            oPRP.FromDate = "01/Jan/1900";

        if (txtToDate.Text != "")
            oPRP.ToDate = txtToDate.Text.Trim();
        else
            oPRP.ToDate = DateTime.Now.ToString("dd/MMM/yyyy");

        oPRP.CompCode = Session["COMPANY"].ToString();
        oPRP.Type = "ASSETMOVEMENT";
        gvRptGatePass.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetReportDetails(oPRP);
        if (dt.Rows.Count > 0)
        {
            gvRptGatePass.DataSource = Session["RptAM"] = dt;
            gvRptGatePass.DataBind();
            gvRptGatePass.Visible = true;
            btnExport.Enabled = true;
            lblRecordCount.Text = "Total Record Count : " + dt.Rows.Count.ToString();
        }
        else
        {
            gvRptGatePass.DataSource = null;
            gvRptGatePass.DataBind();
            lblRecordCount.Text = "Total Record Count : 0";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no data found for selected criteria.');", true);
            return;
        }
    }

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "GatePass Generation Report");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Get location code/name populated for dropdown selection.
    /// </summary>

    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvRptGatePass_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["RptAM"];
            gvRptGatePass.PageIndex = e.NewPageIndex;
            gvRptGatePass.DataSource = dt;
            gvRptGatePass.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}