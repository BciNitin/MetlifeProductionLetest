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

public partial class ReportAMCWarranty : System.Web.UI.Page
{
    RptAMCWarranty_DAL oDAL;
    RptAMCWarranty_PRP oPRP;
    public ReportAMCWarranty()
    {
        oPRP = new RptAMCWarranty_PRP();
    }
    ~ReportAMCWarranty()
    {
        oDAL = null;
        oPRP = null;
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
        oDAL = new RptAMCWarranty_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Get user rights for the page and populate category and location dropdowns.
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
                string _strRights = clsGeneral.GetRights("AMC_WARRANTY_REPORT", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "AMC_WARRANTY_REPORT");
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
    /// Get model name based on asset make and category selected.
    /// </summary>
    /// <param name="AssetMake"></param>
    private void PopulateModelName(string AssetMake,string CategoryCode)
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
    /// Get asset make based on category selected.
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
    /// Get asset history report based on filters provided.
    /// </summary>
    private void GetAssetHistoryReport()
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

        if (ddlAMCWarranty.SelectedIndex != 0)
            oPRP.AMC_Warranty = ddlAMCWarranty.SelectedValue.ToString();
        else
            oPRP.AMC_Warranty = "";

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

        if (ddlAssetType.SelectedIndex != 0)
            oPRP.AssetType = ddlAssetType.SelectedValue.ToString();
        else
            oPRP.AssetType = "IT";

        if (ddlAssetCategory.SelectedIndex != 0)
            oPRP.CategoryCode = ddlAssetCategory.SelectedValue.ToString();
        else
            oPRP.CategoryCode = "";

        if (ddlAgeType.SelectedIndex != 0)
            oPRP.AgeType = ddlAgeType.SelectedValue.ToString();
        else
            oPRP.AgeType = "YY";
        
        oPRP.AgeCriteria = rblAgeCriteria.SelectedValue.ToString();

        if (txtNoOfYrsOld.Text.Trim() != "")
            oPRP.NoOfYearsOld = int.Parse(txtNoOfYrsOld.Text.Trim());
        else
            oPRP.NoOfYearsOld = 0;

        if (txtFromDate.Text.Trim() != "")
            oPRP.PurchaseDateFrom = txtFromDate.Text.Trim();
        else
            oPRP.PurchaseDateFrom = "01/Jan/1900";

        if (txtToDate.Text.Trim() != "")
            oPRP.PurchaseDateTo = txtToDate.Text.Trim();
        else
            oPRP.PurchaseDateTo = DateTime.Now.ToString("dd/MMM/yyyy");

        DataTable dt = new DataTable();
        dt = oDAL.GetAssetHistoryReport(oPRP);
        if (dt.Rows.Count > 0)
        {
            gvAssetHistory.DataSource = Session["ASSET_HISTORY"] = dt;
            gvAssetHistory.DataBind();
            lblAssetCount.Visible = true;
            btnExport.Visible = true;
            lblAssetCount.Text = "Total Assets Count : " + dt.Rows.Count.ToString();
        }
        else
        {
            gvAssetHistory.DataSource = null;
            gvAssetHistory.DataBind();
            lblAssetCount.Visible = false;
            btnExport.Visible = false;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no record found for selected criteria.');", true);
            return;
        }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Refresh/reset category, asset make dropdowns and model name listbox for fresh selection.
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
    /// Get asset history report based on filters provided and disply in the gridview.
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
            else
            {
                GetAssetHistoryReport();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Reset/Clear fields and set location,category,make and model name details refreshed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateLocation();
            PopulateCategory(lblAssetType.Text);
            
            DataTable dt = new DataTable();
            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();

            gvAssetHistory.DataSource = null;
            gvAssetHistory.DataBind();
            lblAssetCount.Visible = false;
            dt = null;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
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
            PopulateLocation();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export asset count report from gridview into excel file.
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
            if (gvAssetHistory.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["ASSET_HISTORY"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=ReportAMCWarranty.xls");
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

    #region SELECTEDINDEXCHANGED EVENTS
    /// <summary>
    /// Populate model name based on asset make and category selected.
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
                    txtNoOfYrsOld.Focus();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Populate sub-category based on parent category selected.
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
    /// Fetch list of categories based on asset type selected.
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
    /// Asset history report gridview page changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ASSET_HISTORY"];
            gvAssetHistory.PageIndex = e.NewPageIndex;
            gvAssetHistory.DataSource = dt;
            gvAssetHistory.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}