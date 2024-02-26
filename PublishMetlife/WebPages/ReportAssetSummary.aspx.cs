using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class ReportAssetSummary : System.Web.UI.Page
{
    RptAssetSummary_DAL oDAL;
    RptAssetSummary_PRP oPRP;
    public ReportAssetSummary()
    {
        oPRP = new RptAssetSummary_PRP();
    }
    ~ReportAssetSummary()
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
        oDAL = new RptAssetSummary_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Get user rights for the page and populate location & category dropdowns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                string _strRights = clsGeneral.GetRights("ASSET_SUMMARY_REPORT", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_SUMMARY_REPORT");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                PopulateLocation();
                ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
                lblAssetType.Text = clsGeneral.gStrAssetType;
                PopulateCategory(lblAssetType.Text.Trim());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Cathes exception for the entier page operations.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "View Asset Summary Report");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Get department code/name for all locations.
    /// </summary>
    private void PopulateDepartment(string CompCode)
    {
        ddlDepartment.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateDepartment(CompCode);
        ddlDepartment.DataSource = dt;
        ddlDepartment.DataTextField = "DEPT_NAME";
        ddlDepartment.DataValueField = "DEPT_CODE";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, "-- Select Department --");
    }

    /// <summary>
    /// Get asset model names as per asset make and category selected.
    /// </summary>
    /// <param name="AssetMake"></param>
    private void PopulateModelName(string AssetMake, string CategoryCode, string CompCode)
    {
        lstModelName.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateModelName(AssetMake, CategoryCode, CompCode);
        lstModelName.DataSource = dt;
        lstModelName.DataTextField = "MODEL_NAME";
        lstModelName.DataValueField = "MODEL_NAME";
        lstModelName.DataBind();
        lstModelName.Items.Insert(0, "-- Select Model --");
    }

    /// <summary>
    /// Get asset make details as per category selected.
    /// </summary>
    private void PopulateAssetMake(string CategoryCode, string CompCode)
    {
        ddlAssetMake.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateAssetMake(CategoryCode, CompCode);
        ddlAssetMake.DataSource = dt;
        ddlAssetMake.DataTextField = "ASSET_MAKE";
        ddlAssetMake.DataValueField = "ASSET_MAKE";
        ddlAssetMake.DataBind();
        ddlAssetMake.Items.Insert(0, "-- Select Make --");
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
    /// Fetch location details to be populated in dropdownlist.
    /// </summary>
    private void PopulateLocation()
    {
        lblLocLevel.Text = "1";
        lblLocCode.Text = "0";
        ddlAssetLocation.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetLocation("", 1);
        ddlAssetLocation.DataSource = dt;
        ddlAssetLocation.DataTextField = "LOC_NAME";
        ddlAssetLocation.DataValueField = "LOC_CODE";
        ddlAssetLocation.DataBind();
        ddlAssetLocation.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// Get process code/name as per department name selected.
    /// </summary>
    /// <param name="DeptCode"></param>
    private void PopulateProcess(string DeptCode,string CompCode)
    {
        ddlProcess.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateProcess(DeptCode, CompCode);
        ddlProcess.DataSource = dt;
        ddlProcess.DataTextField = "PROCESS_NAME";
        ddlProcess.DataValueField = "PROCESS_CODE";
        ddlProcess.DataBind();
        ddlProcess.Items.Insert(0, "-- Select Process --");
    }

    /// <summary>
    /// Get asset summary report details as per filter criteria provided.
    /// </summary>
    private void GetAssetSummaryReport()
    {
        string[] LocParts = { };
        if (lblLocCode.Text != "0")
        {
            LocParts = lblLocCode.Text.Trim().Split('-');
            if (LocParts[2] == "00")
                oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1];
            else if (LocParts[3] == "00")
                oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
            else if (LocParts[4] == "00")
                oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
            else if (LocParts[5] == "00")
                oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
            else
                oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
        }
        else
            oPRP.AssetLocation = "";

        if (ddlAssetMake.SelectedIndex != 0)
            oPRP.AssetMake = ddlAssetMake.SelectedValue.ToString();
        else
            oPRP.AssetMake = "";

        for (int iCnt = 0; iCnt < lstModelName.Items.Count; iCnt++)
        {
            if (lstModelName.Items[iCnt].Selected)
                oPRP.ModelName += lstModelName.Items[iCnt].Value.ToString() + ",";
        }
        if (oPRP.ModelName != null)
        {
            btnGetAssets.Visible = true;
            oPRP.ModelName = oPRP.ModelName.TrimEnd(',');
            oPRP.ModelName = oPRP.ModelName.Replace(",", "','");
            oPRP.ModelName = "'" + oPRP.ModelName + "'";
        }
        else
        {
            oPRP.ModelName = "";
            btnGetAssets.Visible = false;
        }
        if (ddlDepartment.SelectedIndex != 0)
            oPRP.Department = ddlDepartment.SelectedValue.ToString();
        else
            oPRP.Department = "";

        if (ddlProcess.SelectedIndex != 0)
            oPRP.ProcessCode = ddlProcess.SelectedValue.ToString();
        else
            oPRP.ProcessCode = "";

        if (ddlPortNo.SelectedIndex != 0)
            oPRP.PortNo = ddlPortNo.SelectedValue.ToString();
        else
            oPRP.PortNo = "";

        if (ddlAssetCategory.SelectedIndex > 0)
            oPRP.CategoryCode = ddlAssetCategory.SelectedValue.ToString();
        else
            oPRP.CategoryCode = "";

        DataTable dt = new DataTable();
        dt = oDAL.GetAssetSummaryReport(oPRP);
        if (dt.Rows.Count > 0)
        {
            gvAssetSummary.DataSource = Session["ASSET_SUMMARY"] = dt;
            gvAssetSummary.DataBind();
        }
        else
        {
            gvAssetSummary.DataSource = null;
            gvAssetSummary.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no record found for selected criteria.');", true);
            return;
        }
    }

    /// <summary>
    /// Get asset details as per summary report gridview row clicked.
    /// </summary>
    /// <param name="oPRP"></param>
    private void GetAssetDetails(RptAssetSummary_PRP oPRP)
    {
        gvAssetDetails.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetAssetDetails(oPRP);
        if (dt.Rows.Count > 0)
        {
            gvAssetDetails.DataSource = Session["ASSET_DETAILS"] = dt;
            gvAssetDetails.DataBind();
            btnExportDetails.Visible = true;
            lblAssetCount.Visible = true;
            lblAssetCount.Text = "Total Assets Count : " + dt.Rows.Count.ToString();
        }
    }

    /// <summary>
    /// Get asset details model wise.
    /// </summary>
    /// <param name="oPRP"></param>
    private void GetAssetDetails(RptAssetSummary_PRP oPRP, string ModelWise)
    {
        gvAssetDetails.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetAssetDetails(oPRP, ModelWise);
        if (dt.Rows.Count > 0)
        {
            gvAssetDetails.DataSource = Session["ASSET_DETAILS"] = dt;
            gvAssetDetails.DataBind();
            btnExportDetails.Visible = true;
            lblAssetCount.Text = "Total Assets Count : " + dt.Rows.Count.ToString();
        }
    }

    /// <summary>
    /// Get port No. based on department name selected and process name selected in a location.
    /// </summary>
    /// <param name="ProcessCode"></param>
    /// <param name="DeptCode"></param>
    /// <param name="CompCode"></param>
    private void PopulatePortNo(string ProcessCode, string DeptCode, string CompCode)
    {
        ddlPortNo.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulatePortNo(ProcessCode, DeptCode, CompCode);
        ddlPortNo.DataSource = dt;
        ddlPortNo.DataTextField = "PORT_NO";
        ddlPortNo.DataValueField = "PORT_NO";
        ddlPortNo.DataBind();
        ddlPortNo.Items.Insert(0, "-- Select Port --");
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Refresh/reset Category details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshCategory_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateCategory(lblAssetType.Text);
            DataTable dt = new DataTable();
            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            dt = null;
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Reset/refresh location details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnRefreshLocation_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblCompCode.Text = "";
            PopulateLocation();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get asset summary report details.
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
            GetAssetSummaryReport();
            gvAssetDetails.DataSource = null;
            gvAssetDetails.DataBind();
            btnExportDetails.Visible = false;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Reset/Crear fields and set location details refreshed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblCompCode.Text = "";
            PopulateLocation();
            btnGetAssets.Visible = false;
            PopulateCategory(lblAssetType.Text);
            DataTable dt = new DataTable();
            ddlDepartment.DataSource = dt;
            ddlDepartment.DataBind();
            ddlProcess.DataSource = dt;
            ddlProcess.DataBind();
            ddlPortNo.DataSource = dt;
            ddlPortNo.DataBind();
            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            gvAssetSummary.DataSource = null;
            gvAssetSummary.DataBind();
            gvAssetDetails.DataSource = null;
            gvAssetDetails.DataBind();
            btnExportDetails.Visible = false;
            lblAssetCount.Visible = false;
            dt = null;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export asset count report from gridview to excel sheet.
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
            if (gvAssetSummary.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["ASSET_SUMMARY"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=ReportAssetSummary.xls");
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

    /// <summary>
    /// Export asset details for the count displayed in a particular department, process,
    /// asset make and model type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportDetails_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[4] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (gvAssetDetails.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["ASSET_DETAILS"];
                DataSet dsExport = new DataSet();
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.HeaderStyle.Font.Bold = true;
                dgGrid.DataBind();
                dgGrid.RenderControl(hw);
                Response.ContentType = "application/vnd.ms-excel";
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
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

    /// <summary>
    /// Get assets details for seelcted models from listbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGetAssets_Click(object sender, EventArgs e)
    {
        try
        {
            string[] LocParts = { };
            if (lblLocCode.Text != "0")
            {
                LocParts = lblLocCode.Text.Trim().Split('-');
                if (LocParts[2] == "00")
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1];
                else if (LocParts[3] == "00")
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
                else if (LocParts[4] == "00")
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
                else if (LocParts[5] == "00")
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
                else
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
            }
            else
                oPRP.AssetLocation = "";

            if (ddlAssetCategory.SelectedIndex != 0)
                oPRP.CategoryCode = ddlAssetCategory.SelectedValue.ToString();
            else
                oPRP.CategoryCode = "";

            if (ddlDepartment.SelectedIndex != 0)
                oPRP.Department = ddlDepartment.SelectedValue.ToString();
            else
                oPRP.Department = "";

            if (ddlProcess.SelectedIndex != 0)
                oPRP.ProcessCode = ddlProcess.SelectedValue.ToString();
            else
                oPRP.ProcessCode = "";

            if (ddlPortNo.SelectedIndex != 0)
                oPRP.PortNo = ddlPortNo.SelectedValue.ToString();
            else
                oPRP.PortNo = "";

            if (ddlAssetMake.SelectedIndex != 0)
                oPRP.AssetMake = ddlAssetMake.SelectedValue.ToString();
            else
                oPRP.AssetMake = "";

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
            GetAssetDetails(oPRP, "MODELWISE");
            btnExportDetails.Visible = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SELECTEDINDEXCHANGED EVENTS
    /// <summary>
    /// Get process code/name as per department name selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlDepartment.SelectedIndex != 0)
                PopulateProcess(ddlDepartment.SelectedValue.ToString(), lblCompCode.Text.Trim());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }    

    /// <summary>
    /// Get a list of port nos based on department name and process name provided.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProcess.SelectedIndex != 0)
                PopulatePortNo(ddlProcess.SelectedValue.ToString(), ddlDepartment.SelectedValue.ToString(), lblCompCode.Text.Trim());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get asset model names as per asset make selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetMake.SelectedIndex != 0)
                PopulateModelName(ddlAssetMake.SelectedValue.ToString(), lblCatCode.Text.Trim(), lblCompCode.Text.Trim());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get Sub location details to be populated into dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetLocation.SelectedIndex != 0)
            {
                lblCompCode.Text = oDAL.GetCompanyCode(ddlAssetLocation.SelectedValue.ToString());
                PopulateDepartment(lblCompCode.Text);
                int locLevel = int.Parse(lblLocLevel.Text.Trim());
                lblLocLevel.Text = (locLevel + 1).ToString();
                int iLocLevel = int.Parse(lblLocLevel.Text.Trim());
                string sLocCode = ddlAssetLocation.SelectedValue.ToString();
                lblLocCode.Text = ddlAssetLocation.SelectedValue.ToString();

                ddlAssetLocation.DataSource = null;
                DataTable dt = oDAL.GetLocation(sLocCode, iLocLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlAssetLocation.DataSource = dt;
                    ddlAssetLocation.DataValueField = "LOC_CODE";
                    ddlAssetLocation.DataTextField = "LOC_NAME";
                    ddlAssetLocation.DataBind();
                    ddlAssetLocation.Items.Insert(0, "-- Select Location --");
                    ddlAssetLocation.Focus();
                }
                else
                {
                    iLocLevel = iLocLevel - 1;
                    lblLocLevel.Text = iLocLevel.ToString();
                    ddlDepartment.Focus();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get sub categories and asset make as per category name selected.
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

                PopulateAssetMake(ddlAssetCategory.SelectedValue.ToString(),lblCompCode.Text.Trim());
                int CatLevel = int.Parse(lblCatLevel.Text.Trim());
                lblCatLevel.Text = (CatLevel + 1).ToString();
                int iCatLevel = int.Parse(lblCatLevel.Text.Trim());
                string sCatCode = ddlAssetCategory.SelectedValue.ToString();
                lblCatCode.Text = sCatCode;

                ddlAssetCategory.DataSource = null;
                DataTable dt = oDAL.PopulateCategory(lblAssetType.Text, sCatCode, iCatLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlAssetCategory.DataSource = dt;
                    ddlAssetCategory.DataValueField = "CATEGORY_CODE";
                    ddlAssetCategory.DataTextField = "CATEGORY_NAME";
                    ddlAssetCategory.DataBind();
                    ddlAssetCategory.Items.Insert(0, "-- Select Category --");
                    ddlAssetCategory.Focus();
                }
                else
                {
                    iCatLevel = iCatLevel - 1;
                    lblCatLevel.Text = iCatLevel.ToString();
                    ddlAssetMake.Focus();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get list of asset categories based on asset type selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetType.SelectedIndex != 0)
            {
                DataTable dtNull = new DataTable();
                ddlAssetCategory.DataSource = dtNull;
                ddlAssetCategory.DataBind();
                ddlAssetMake.DataSource = dtNull;
                ddlAssetMake.DataBind();
                lstModelName.DataSource = dtNull;
                lstModelName.DataBind();
                dtNull = null;

                lblAssetType.Text = ddlAssetType.SelectedValue.ToString();
                PopulateCategory(lblAssetType.Text);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Asset summary report gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ASSET_SUMMARY"];
            gvAssetSummary.PageIndex = e.NewPageIndex;
            gvAssetSummary.DataSource = dt;
            gvAssetSummary.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Asset details gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ASSET_DETAILS"];
            gvAssetDetails.PageIndex = e.NewPageIndex;
            gvAssetDetails.DataSource = dt;
            gvAssetDetails.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    
    /// <summary>
    /// Get asset details as per summary report gridview row button is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetSummary_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridViewRow gvRow = (GridViewRow)gvAssetSummary.Rows[e.RowIndex];
            oPRP.CategoryCode = ((Label)gvRow.FindControl("lblCatCode")).Text.Trim();
            oPRP.AssetLocation = ((Label)gvRow.FindControl("lblLocationCode")).Text.Trim();
            oPRP.Department = ((Label)gvRow.FindControl("lblDeptCode")).Text.Trim();
            oPRP.ProcessCode = ((Label)gvRow.FindControl("lblProcessCode")).Text.Trim();
            oPRP.PortNo = ((Label)gvRow.FindControl("lblPortNo")).Text.Trim();
            oPRP.AssetMake = ((Label)gvRow.FindControl("lblAssetMake")).Text.Trim();
            oPRP.ModelName = ((Label)gvRow.FindControl("lblModel")).Text.Trim();
            GetAssetDetails(oPRP);
            btnExportDetails.Visible = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}