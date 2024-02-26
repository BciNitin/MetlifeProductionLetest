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

public partial class ReportAssetAllocation : System.Web.UI.Page
{
    RptAssetAllocation_DAL oDAL;
    RptAssetAllocation_PRP oPRP;
    public ReportAssetAllocation()
    {
        oPRP = new RptAssetAllocation_PRP();
    }
    ~ReportAssetAllocation()
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
                string _strRights = clsGeneral.GetRights("ASSET_ALLOCATION_REPORT", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_ALLOCATION_REPORT");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
               // PopulateProcess();
               // PopulateLocation();
               
                PopulateCategory("");
                PopulateProcessEmployee("", Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SELECTEDINDEXCHANGED EVENTS
    /// <summary>
    /// Get/populate employee based on department name selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlProcess.SelectedIndex > 0)
            //{
            //    PopulateProcessEmployee(ddlProcess.SelectedValue.ToString(), Session["COMPANY"].ToString());
            //}
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get child/sub location based on parent location selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void ddlAssetLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlAssetLocation.SelectedIndex != 0)
    //        {
    //            int locLevel = int.Parse(lblLocLevel.Text.Trim());
    //            lblLocLevel.Text = (locLevel + 1).ToString();
    //            int iLocLevel = int.Parse(lblLocLevel.Text.Trim());
    //            string sLocCode = ddlAssetLocation.SelectedValue.ToString();
    //            lblLocCode.Text = ddlAssetLocation.SelectedValue.ToString();

    //            ddlAssetLocation.DataSource = null;
    //            DataTable dt = oDAL.GetLocation(Session["COMPANY"].ToString(), sLocCode, iLocLevel);
    //            if (dt.Rows.Count > 0)
    //            {
    //                ddlAssetLocation.DataSource = dt;
    //                ddlAssetLocation.DataValueField = "LOC_CODE";
    //                ddlAssetLocation.DataTextField = "LOC_NAME";
    //                ddlAssetLocation.DataBind();
    //                ddlAssetLocation.Items.Insert(0, "-- Select Location --");
    //                ddlAssetLocation.Focus();
    //            }
    //            else
    //            {
    //                iLocLevel = iLocLevel - 1;
    //                lblLocLevel.Text = iLocLevel.ToString();
    //                ddlAssetType.Focus();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    { HandleExceptions(ex); }
    //}

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
                //int CatLevel = int.Parse(lblCatLevel.Text.Trim());
                //lblCatLevel.Text = (CatLevel + 1).ToString();
                //int iCatLevel = int.Parse(lblCatLevel.Text.Trim());
                string sCatCode = ddlAssetCategory.SelectedValue.ToString();
                lblCatCode.Text = sCatCode;

                //ddlAssetCategory.DataSource = null;
                //DataTable dt = oDAL.PopulateCategory(lblAssetType.Text, sCatCode, iCatLevel);
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

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Catch unhandled exceptions and show the errors on error page.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Asset Allocation Report");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Get location code/name populated for dropdown selection.
    /// </summary>
    private void PopulateLocation()
    {
        //lblLocCode.Text = "0";
        //DataTable dt = new DataTable();
        //dt = oDAL.GetLocation(Session["COMPANY"].ToString(), "", 1);
        //ddlAssetLocation.DataSource = null;
        //ddlAssetLocation.DataSource = dt;
        //ddlAssetLocation.DataTextField = "LOC_NAME";
        //ddlAssetLocation.DataValueField = "LOC_CODE";
        //ddlAssetLocation.DataBind();
        //ddlAssetLocation.Items.Insert(0, "-- Select Location --");
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
    /// Populate asse model names (for one or more model name selection) as per asset make and category provided.
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
    /// Populate asset make as per asset category is provided.
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
        //ddlProcess.DataSource = null;
        //DataTable dt = new DataTable();q3333
        //dt = oDAL.GetProcess(Session["COMPANY"].ToString());
        //ddlProcess.DataSource = dt;
        //ddlProcess.DataValueField = "PROCESS_CODE";
        //ddlProcess.DataTextField = "PROCESS_NAME";
        //ddlProcess.DataBind();
        //ddlProcess.Items.Insert(0, "-- Select Process --");
    }

    /// <summary>
    /// Populate employee code/name into employee dropdownlist based on 
    /// department selected through department dropdownlist.
    /// </summary>
    /// <param name="_DeptCode"></param>
    private void PopulateProcessEmployee(string _ProcessCode,string _CompCode)
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

    #region BUTTON EVENTS
    /// <summary>
    /// Refresh/reset category, asset make dropdownlists and model name listbox for fresh selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnRefreshCategory_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        PopulateCategory(lblAssetType.Text);
    //        DataTable dt = new DataTable();
    //        ddlAssetMake.DataSource = dt;
    //        ddlAssetMake.DataBind();
    //        lstModelName.DataSource = dt;
    //        lstModelName.DataBind();
    //        dt = null;
    //    }
    //    catch (Exception ex)
    //    { HandleExceptions(ex); }
    //}

    /// <summary>
    /// Refresh/reset location dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnRefreshLocation_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblLocLevel.Text = "1";
    //        PopulateLocation();
    //    }
    //    catch (Exception ex)
    //    { HandleExceptions(ex); }
    //}

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
                Response.Redirect("UnauthorizedUser.aspx");
            }

            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Asset Allocation", "Asset Allocation Report", "Asset Allocation Report generate by user id" + Session["CURRENTUSER"].ToString());
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

            string[] LocParts = { };
            //if (lblLocCode.Text != "0")
            //{
            //    LocParts = lblLocCode.Text.Trim().Split('-');
            //    if (LocParts[2] == "00")
            //        oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1];
            //    else if (LocParts[3] == "00")
            //        oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
            //    else if (LocParts[4] == "00")
            //        oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
            //    else if (LocParts[5] == "00")
            //        oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
            //    else
            //        oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
            //}
            //else
            //    oPRP.AssetLocation = "";

            //if (ddlAllocRtrn.SelectedIndex != 0)
            //    oPRP.AllocationType = ddlAllocRtrn.SelectedValue.ToString();
            //else
            //    oPRP.AllocationType = "";



            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.FromDate = (txtFromDate.Text.Trim() != "") ? txtFromDate.Text.Trim() : "01/Jan/1900";
            oPRP.ToDate = (txtToDate.Text.Trim() != "") ? txtToDate.Text.Trim() : DateTime.Now.ToString("dd/MMM/yyyy");

            gvRptAssetAllocation.DataSource = null;
            DataTable dt = new DataTable();
            if (rdoITAsset.Checked == true)
            {
                dt = oDAL.GetAssetAllocationITReport(oPRP);
            }
            else
            {
                dt = oDAL.GetAssetAllocationFacilityReport(oPRP);
            }
            if (dt.Rows.Count > 0)
            {
                gvRptAssetAllocation.DataSource = Session["RptAssetAllocation"] = dt;
                gvRptAssetAllocation.DataBind();
                gvRptAssetAllocation.Visible = true;
                btnExport.Enabled = true;
            }
            else
            {
                gvRptAssetAllocation.DataSource = null;
                gvRptAssetAllocation.DataBind();
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
            gvRptAssetAllocation.DataSource = null;
            gvRptAssetAllocation.Visible = false;
           // lblLocLevel.Text = "1";
            PopulateLocation();
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
    /// Export asset allocation report data into excel file.
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
            if (gvRptAssetAllocation.Rows.Count > 0)
            {
                DataTable dt = (DataTable)Session["RptAssetAllocation"];
                dt.TableName = "RptAssetAllocation";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=ReportAssetAllocation.xls");
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
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no data for being exported.');", true);
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
    protected void gvRptAssetAllocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["RptAssetAllocation"];
            gvRptAssetAllocation.PageIndex = e.NewPageIndex;
            gvRptAssetAllocation.DataSource = dt;
            gvRptAssetAllocation.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    protected void grdFacility_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
}