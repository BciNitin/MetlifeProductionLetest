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

public partial class ReportAllocatedReturnable : System.Web.UI.Page
{
    RptAssetAllocation_DAL oDAL;
    RptAssetAllocation_PRP oPRP;
    public ReportAllocatedReturnable()
    {
        oPRP = new RptAssetAllocation_PRP();
    }
    ~ReportAllocatedReturnable()
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
        oDAL = new RptAssetAllocation_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user permissions in order to view asset allocation returnable report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //string _strRights = clsGeneral.GetRights("ALLOCATED_RETURNABLE_REPORT", (DataTable)Session["UserRights"]);
                //clsGeneral._strRights = _strRights.Split('^');
                //clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ALLOCATED_RETURNABLE_REPORT");
                //if (clsGeneral._strRights[0] == "0")
                //{
                //    Response.Redirect("UnauthorizedUser.aspx", false);
                //}
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
               // PopulateProcess();
                //ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
                //lblAssetType.Text = clsGeneral.gStrAssetType;
                PopulateCategory("");
                PopulateProcessEmployee("", Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Allocated Returnable Report");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Fetch category details to be populated in dropdownlist.
    /// </summary>
    private void PopulateCategory(string AssetType)
    {
        lblCatCode.Text = "0";
        lblCatLevel.Text = "1";
        ddlAssetCategory.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateCategory(AssetType.Trim(), "", 1);
        ddlAssetCategory.DataSource = dt;
        ddlAssetCategory.DataTextField = "CATEGORY_NAME";
        ddlAssetCategory.DataValueField = "CATEGORY_CODE";
        ddlAssetCategory.DataBind();
        ddlAssetCategory.Items.Insert(0, "-- Select Category --");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AssetMake"></param>
    private void PopulateModelName(string AssetMake, string CategoryCode)
    {
        lstModelName.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateModelName(AssetMake, CategoryCode, Session["COMPANY"].ToString());
        lstModelName.DataSource = dt;
        lstModelName.DataTextField = "MODEL_NAME";
        lstModelName.DataValueField = "MODEL_NAME";
        lstModelName.DataBind();
        lstModelName.Items.Insert(0, "-- Select Model --");
    }

    /// <summary>
    /// 
    /// </summary>
    private void PopulateAssetMake(string CategoryCode)
    {
        ddlAssetMake.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateAssetMake(CategoryCode, Session["COMPANY"].ToString());
        ddlAssetMake.DataSource = dt;
        ddlAssetMake.DataTextField = "ASSET_MAKE";
        ddlAssetMake.DataValueField = "ASSET_MAKE";
        ddlAssetMake.DataBind();
        ddlAssetMake.Items.Insert(0, "-- Select Make --");
    }

    /// <summary>
    /// Get Department code/name populated for dropdown selection.
    /// </summary>
    private void PopulateProcess()
    {
        ddlProcess.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetProcess(Session["COMPANY"].ToString());
        ddlProcess.DataSource = dt;
        ddlProcess.DataValueField = "PROCESS_CODE";
        ddlProcess.DataTextField = "PROCESS_NAME";
        ddlProcess.DataBind();
        ddlProcess.Items.Insert(0, "-- Select Process --");
    }

    /// <summary>
    /// Populate employee code/name into employee dropdownlist based on 
    /// department selected through department dropdownlist.
    /// </summary>
    /// <param name="_DeptCode"></param>
    private void PopulateProcessEmployee(string _ProcessCode, string _CompCode)
    {
        ddlEmployee.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetProcEmployee(_ProcessCode, _CompCode);
        ddlEmployee.DataSource = dt;
        ddlEmployee.DataValueField = "EMPLOYEE_CODE";
        ddlEmployee.DataTextField = "EMPLOYEE_NAME";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, "-- Select Employee --");
    }
    #endregion

    #region SELECTED INDEX CHANGED EVENTS
    /// <summary>
    /// Get/populate employee based on department name selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProcess.SelectedIndex > 0)
            {
                PopulateProcessEmployee(ddlProcess.SelectedValue.ToString(), Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetCategory.SelectedIndex > 0)
            {
                DataTable dtNull = new DataTable();
                lstModelName.DataSource = dtNull;
                lstModelName.DataBind();
                dtNull = null;

                PopulateAssetMake(ddlAssetCategory.SelectedValue.ToString());
                int CatLevel = int.Parse(lblCatLevel.Text.Trim());
                lblCatLevel.Text = (CatLevel + 1).ToString();
                int iCatLevel = int.Parse(lblCatLevel.Text.Trim());
                string sCatCode = ddlAssetCategory.SelectedValue.ToString();
                lblCatCode.Text = sCatCode;

               // ddlAssetCategory.DataSource = null;
               // DataTable dt = oDAL.PopulateCategory(lblAssetType.Text, sCatCode, iCatLevel);
                //if (dt.Rows.Count > 0)
                //{
                //    ddlAssetCategory.DataSource = dt;
                //    ddlAssetCategory.DataValueField = "CATEGORY_CODE";
                //    ddlAssetCategory.DataTextField = "CATEGORY_NAME";
                //    ddlAssetCategory.DataBind();
                //    ddlAssetCategory.Items.Insert(0, "-- Select Category --");
                //    ddlAssetCategory.Focus();
                //}
                //else
                //{
                //    iCatLevel = iCatLevel - 1;
                //    lblCatLevel.Text = iCatLevel.ToString();
                //    ddlAssetMake.Focus();
                //}
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get/populate category list based on asset type selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlAssetType.SelectedIndex != 0)
            //{
            //    DataTable dtNull = new DataTable();
            //    ddlAssetCategory.DataSource = dtNull;
            //    ddlAssetCategory.DataBind();
            //    ddlAssetMake.DataSource = dtNull;
            //    ddlAssetMake.DataBind();
            //    lstModelName.DataSource = dtNull;
            //    lstModelName.DataBind();
            //    dtNull = null;

            //    lblAssetType.Text = ddlAssetType.SelectedValue.ToString();
            //    PopulateCategory(lblAssetType.Text);
            //}
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetMake.SelectedIndex != 0)
                PopulateModelName(ddlAssetMake.SelectedValue.ToString(), lblCatCode.Text.Trim());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshCategory_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //PopulateCategory(lblAssetType.Text);
            //DataTable dt = new DataTable();
            //ddlAssetMake.DataSource = dt;
            //ddlAssetMake.DataBind();
            //lstModelName.DataSource = dt;
            //lstModelName.DataBind();
            //dt = null;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get asset allocation report based on filetrs selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[0] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            oPRP.CompCode = Session["COMPANY"].ToString();
            int iDate = clsGeneral.CompareDate(txtFromDate.Text.Trim(), txtToDate.Text.Trim());
            if (iDate > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : From Date should be earlier than To Date.');", true);
                return;
            }
            //if (ddlAssetType.SelectedIndex != 0)
            //    oPRP.AssetType = ddlAssetType.SelectedValue.ToString();
            //else
            //    oPRP.AssetType = "";
            if (ddlAssetMake.SelectedIndex != 0)
                oPRP.AssetMake = ddlAssetMake.SelectedValue.ToString();
            else
                oPRP.AssetMake = "";
            if (ddlAssetCategory.SelectedIndex != 0)
                oPRP.CategoryCode = ddlAssetCategory.SelectedValue.ToString();
            else
                oPRP.CategoryCode = "";
            for (int iCnt = 0; iCnt < lstModelName.Items.Count; iCnt++)
            {
                if (lstModelName.Items[iCnt].Selected)
                    oPRP.ModelName += lstModelName.Items[iCnt].Value.ToString() + ",";
            }
            if (oPRP.ModelName != null)
            {
                oPRP.ModelName = oPRP.ModelName.TrimEnd(',');
                oPRP.ModelName = oPRP.ModelName.Replace(",", "','");
                oPRP.ModelName = "'" + oPRP.ModelName + "'";
            }
            else
                oPRP.ModelName = "";
            //if (ddlProcess.SelectedIndex != 0)
            //    oPRP.ProcessCode = ddlProcess.SelectedValue.ToString();
            //else
            //    oPRP.ProcessCode = "";
            if (ddlEmployee.SelectedIndex != 0)
                oPRP.EmpCode = ddlEmployee.SelectedValue.ToString();
            else
                oPRP.EmpCode = "";

            if (rdoAllocationDate.Checked)
                oPRP.DateSearchBy = "ALLOCATE";
            else if (rdoReturnDate.Checked)
                oPRP.DateSearchBy = "RETURN";
            oPRP.FromDate = (txtFromDate.Text.Trim() != "") ? txtFromDate.Text.Trim() : "01/Jan/1900";
            oPRP.ToDate = (txtToDate.Text.Trim() != "") ? txtFromDate.Text.Trim() : DateTime.Now.ToString("dd/MMM/yyyy");

            DataTable dt = new DataTable();
            dt = oDAL.GetAllocatedAssetsStatus(oPRP);
            if (dt.Rows.Count > 0)
            {
                gvRptReturnable.DataSource = Session["RptAllocReturn"] = dt;
                gvRptReturnable.DataBind();
                gvRptReturnable.Visible = true;
                btnExport.Enabled = true;
            }
            else
            {
                gvRptReturnable.DataSource = null;
                gvRptReturnable.DataBind();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no data found for selected criteria.');", true);
                return;
            }
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
            gvRptReturnable.DataSource = null;
            gvRptReturnable.DataBind();
            PopulateCategory("");
            DataTable dt = new DataTable();
            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            dt = null;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
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
                Response.Redirect("UnauthorizedUser.aspx");
            }
            if (gvRptReturnable.Rows.Count > 0)
            {
                DataTable dt = (DataTable)Session["RptAllocReturn"];
                DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=ReportAllocatedReturnable.xls");
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
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvRptReturnable_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["RptAllocReturn"];
            gvRptReturnable.PageIndex = e.NewPageIndex;
            gvRptReturnable.DataSource = dt;
            gvRptReturnable.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}
