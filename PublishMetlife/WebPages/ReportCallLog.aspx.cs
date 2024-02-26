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

public partial class ReportCallLog : System.Web.UI.Page
{
    CallLog_DAL oDAL;
    CallLog_PRP oPRP;
    public ReportCallLog()
    {
        oPRP = new CallLog_PRP();
    }
    ~ReportCallLog()
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
        oDAL = new CallLog_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user rights for call log report view and get a list of vendors, call log nos, call nos and asset category.
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
                string _strRights = clsGeneral.GetRights("CALL_LOG_REPORT", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "CALL_LOG_REPORT");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                PopulateVendor();
                PopulateCallLogNo();
                PopulateCallNo();
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
    /// Populate vendor call nos list.
    /// </summary>
    private void PopulateCallNo()
    {
        ddlCallNo.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetCallNo(Session["COMPANY"].ToString());
        ddlCallNo.DataSource = dt;
        ddlCallNo.DataTextField = "CALL_NO";
        ddlCallNo.DataValueField = "CALL_NO";
        ddlCallNo.DataBind();
        ddlCallNo.Items.Insert(0, "-- Select Call No. --");
    }

    /// <summary>
    /// Populate vendor call log nos list.
    /// </summary>
    private void PopulateCallLogNo()
    {
        ddlCallLogCode.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetCallLogCode(Session["COMPANY"].ToString());
        ddlCallLogCode.DataSource = dt;
        ddlCallLogCode.DataTextField = "CALL_LOG_CODE";
        ddlCallLogCode.DataValueField = "CALL_LOG_CODE";
        ddlCallLogCode.DataBind();
        ddlCallLogCode.Items.Insert(0, "-- Select Call Log No. --");
    }

    /// <summary>
    /// Populate vendors list.
    /// </summary>
    private void PopulateVendor()
    {
        ddlVendor.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetVendor(Session["COMPANY"].ToString());
        ddlVendor.DataSource = dt;
        ddlVendor.DataTextField = "VENDOR_NAME";
        ddlVendor.DataValueField = "VENDOR_CODE";
        ddlVendor.DataBind();
        ddlVendor.Items.Insert(0, "-- Select Vendor --");
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
    /// Populate list of model names based on asset make and category provided.
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
    /// Populate a list of asset make based on asset category provided.
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
    /// Get vendor call log report based on parameters provided.
    /// </summary>
    private void GetCallLogReport()
    {
        oPRP.CompCode = Session["COMPANY"].ToString();
        if (ddlVendor.SelectedIndex != 0)
            oPRP.VendorCode = ddlVendor.SelectedValue.ToString();
        else
            oPRP.VendorCode = "";
        if (ddlCallLogCode.SelectedIndex != 0)
            oPRP.CallLogCode = ddlCallLogCode.SelectedValue.ToString();
        else
            oPRP.CallLogCode = "";
        if (ddlCallNo.SelectedIndex != 0)
            oPRP.CallNo = ddlCallNo.SelectedValue.ToString();
        else
            oPRP.CallNo = "";
        if (ddlAssetMake.SelectedIndex != 0)
            oPRP.AssetMake = ddlAssetMake.SelectedValue.ToString();
        else
            oPRP.AssetMake = "";
        if (ddlCallStatus.SelectedIndex != 0)
            oPRP.ResolvedStatus = ddlCallStatus.SelectedValue.ToString();
        else
            oPRP.ResolvedStatus = "";
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
            oPRP.AssetType = "";

        if (ddlAssetCategory.SelectedIndex != 0)
            oPRP.CategoryCode = ddlAssetCategory.SelectedValue.ToString();
        else
            oPRP.CategoryCode = "";

        oPRP.CallDateFrom = (txtCallDateFrom.Text.Trim() != "") ? txtCallDateFrom.Text.Trim() : "01/Jan/1900";
        oPRP.CallDateTo = (txtCallDateTo.Text.Trim() != "") ? txtCallDateTo.Text.Trim() : DateTime.Now.ToString("dd/MMM/yyyy");
        //oPRP.ResolvedDateFrom = (txtResolvedDateFrom.Text.Trim()!="")?txtResolvedDateFrom.Text.Trim():"01/Jan/1900";
        //oPRP.ResolvedDateTo = (txtResolvedDateTo.Text.Trim() != "") ? txtResolvedDateTo.Text.Trim() : DateTime.Now.ToString("dd/MMM/yyyy");

        if(rdoRepair.Checked)
            oPRP.PartStatus = "REPAIR";
        else if(rdoReplaced.Checked)
            oPRP.PartStatus = "REPLACED";
        else
            oPRP.PartStatus = "";

        DataTable dt = new DataTable();
        dt = oDAL.GetCallLogReport(oPRP);
        if (dt.Rows.Count > 0)
        {
            gvCallLog.DataSource = Session["CALL_LOG"] = dt;
            gvCallLog.DataBind();
        }
        else
        {
            gvCallLog.DataSource = null;
            gvCallLog.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no record found for selected criteria.');", true);
            return;
        }
    }
    #endregion
    
    #region BUTTON EVENTS
    /// <summary>
    /// Refresh/reset asset category, asset make and model name list.
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
    /// Get vendor call log report.
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
                GetCallLogReport();
            }
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
            PopulateCategory(lblAssetType.Text);
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
    /// Export vendor call log report into excel file.
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
            if (gvCallLog.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["CALL_LOG"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=ReportCallLog.xls");
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
    /// Get a list of asset model names based on asset make provided.
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
    /// Get vendor call log code and call no. based on vendor name provided.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlVendor.SelectedIndex != 0)
            {
                DataTable dt = new DataTable();
                dt = oDAL.GetVendorDetails(ddlVendor.SelectedValue.ToString(), Session["COMPANY"].ToString());
                if (dt.Rows.Count > 0)
                {
                    ddlCallLogCode.SelectedValue = dt.Rows[0]["CALL_LOG_CODE"].ToString();
                    ddlCallNo.SelectedValue = dt.Rows[0]["CALL_NO"].ToString();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get a list of asset sub category based on parent category provided.
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
        {
            HandleExceptions(ex);
        }
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
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Vendor call log report gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCallLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["CALL_LOG"];
            gvCallLog.PageIndex = e.NewPageIndex;
            gvCallLog.DataSource = dt;
            gvCallLog.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}