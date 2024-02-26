using System;
using System.Collections;
using System.Configuration;
using System.Data;
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

public partial class CallLogDetails : System.Web.UI.Page
{
    bool bPending = false;
    CallLog_DAL oDAL;
    CallLog_PRP oPRP;
    public CallLogDetails()
    {
        oPRP = new CallLog_PRP();
    }
    ~CallLogDetails()
    {
        oPRP = null; oDAL = null;
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
    /// Checking user group rights for Call Log view operation.
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
                string _strRights = clsGeneral.GetRights("CALL_LOG", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "CALL_LOG_MANAGEMENT");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                if (Request.QueryString["PENDING"] != null)
                {
                    if (Convert.ToString(Request.QueryString["PENDING"].Trim()) == "1")
                        bPending = true;
                }
                clsGeneral.OpType = "SAVE";
                ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
                lblAssetType.Text = clsGeneral.gStrAssetType;
                PopulateCategory(lblAssetType.Text.Trim());
                PopulateVendor();
                GetCallLogCode();
                GetCallLogDetails(bPending);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Call Log Details");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetCallLogCode()
    {
        oPRP.CompCode = Session["COMPANY"].ToString();
        int _iMaxCLId = oDAL.GetMaxCallLogId(oPRP.CompCode);
        oPRP.RunningSerialNo = _iMaxCLId;
        txtCallLogCode.Text = oPRP.CompCode + "-VCL-" + _iMaxCLId.ToString().PadLeft(6, '0');
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetCallLogDetails(bool bPending)
    {
        gvCallLog.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetCallLogDetails(Session["COMPANY"].ToString(), bPending);
        gvCallLog.DataSource = dt;
        gvCallLog.DataBind();
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
    private void GetAssets()
    {
        oPRP.AssetCode = txtAssetCode.Text.Trim();
        oPRP.SerialCode = txtSerialCode.Text.Trim();
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
            oPRP.AssetType = "";
        if (ddlAssetCategory.SelectedIndex != 0)
            oPRP.CategoryCode = ddlAssetCategory.SelectedValue.ToString();
        else
            oPRP.CategoryCode = "";
        gvAssets.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetAssets(oPRP, Session["COMPANY"].ToString());
        if (dt.Rows.Count > 0)
        {
            gvAssets.DataSource = dt;
            gvAssets.DataBind();
            gvAssets.Visible = true;
            lblAssetCount.Text = "Assets Count : " + dt.Rows.Count.ToString();
        }
        else
        {
            gvAssets.DataSource = null;
            gvAssets.Visible = false;
            lblAssetCount.Text = "Assets Count : 0";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Assets not found in the selected criterian.');", true);
            return;
        }
    }

    /// <summary>
    /// Get Call Log Details to be populated for update.
    /// </summary>
    /// <param name="CallLogCode"></param>
    private void GetCallLogDetailsForUpdate(string CallLogCode)
    {
        clsGeneral.OpType = "UPDATE";
        DataTable dt = oDAL.GetCallLogDetailsForUpdate(CallLogCode, Session["COMPANY"].ToString());
        if (dt.Rows.Count > 0)
        {
            txtCallLogCode.Text = CallLogCode;
            ddlVendor.SelectedValue = dt.Rows[0]["VENDOR_CODE"].ToString();
            txtCallNo.Text = dt.Rows[0]["CALL_NO"].ToString();
            txtCallDate.Text = Convert.ToDateTime(dt.Rows[0]["CALL_DATE"].ToString()).ToString("dd/MMM/yyyy");
            if (dt.Rows[0]["RESPONDED_DATE"].ToString() != "")
                txtRespondedDate.Text = Convert.ToDateTime(dt.Rows[0]["RESPONDED_DATE"].ToString()).ToString("dd/MMM/yyyy");
            ddlCallStatus.SelectedValue = dt.Rows[0]["RESOLVED_STATUS"].ToString();
            if (ddlCallStatus.SelectedValue == "RESOLVED")
                txtResolvedDate.Enabled = true;
            if (dt.Rows[0]["RESOLVED_DATE"].ToString() != "")
                txtResolvedDate.Text = Convert.ToDateTime(dt.Rows[0]["RESOLVED_DATE"].ToString()).ToString("dd/MMM/yyyy");
            txtRemarks.Text = dt.Rows[0]["CALL_LOG_REMARKS"].ToString();
            txtEngrName.Text = dt.Rows[0]["ENGINEER_NAME"].ToString();
            txtActionTaken.Text = dt.Rows[0]["ACTION_TAKEN"].ToString();
            txtContPerson.Text = dt.Rows[0]["VENDOR_CONT_PERSON"].ToString();
            if (dt.Rows[0]["PART_STATUS"].ToString() == "REPAIR")
                rdoRepair.Checked = true;
            else
                rdoReplaced.Checked = true;
            txtReplacedSrlNo.Text = dt.Rows[0]["REPLACED_SERIAL_NO"].ToString();
            txtFRUNo.Text = dt.Rows[0]["FRU_NO"].ToString();
            txtGatePassNo.Text = dt.Rows[0]["GATEPASS_NO"].ToString();
            txtVendorLocation.Text = dt.Rows[0]["VENDOR_LOCATION"].ToString();
        }
        DataTable dtAssets = oDAL.GetCallLogAssets(CallLogCode, Session["COMPANY"].ToString());
        if (dtAssets.Rows.Count > 0)
        {
            gvAssets.DataSource = dtAssets;
            gvAssets.DataBind();
            gvAssets.Visible = true;
            foreach (GridViewRow gvRow in gvAssets.Rows)
                ((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked = true;
        }
        btnSearch.Enabled = false;
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
    /// Save new call log details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            bool bAssetSelected = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            for (int iCnt = 0; iCnt < gvAssets.Rows.Count; iCnt++)
            {
                if (((CheckBox)gvAssets.Rows[iCnt].FindControl("chkSelectAsset")).Checked == true)
                {
                    bAssetSelected = true;
                    break;
                }
            }
            if (!bAssetSelected)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select at least one asset.');", true);
                return;
            }
            oPRP.CallLogCode = txtCallLogCode.Text.Trim();
            oPRP.CallDate = txtCallDate.Text.Trim();
            oPRP.CallNo = txtCallNo.Text.Trim();
            oPRP.RespondedDate = (txtRespondedDate.Text.Trim() != "") ? txtRespondedDate.Text.Trim() : "";
            oPRP.VendorCode = (ddlVendor.SelectedIndex != 0) ? ddlVendor.SelectedValue.ToString() : "";
            oPRP.VendorLocation = txtVendorLocation.Text.Trim();
            oPRP.VendorContPerson = txtContPerson.Text.Trim();
            oPRP.CallDate = (txtCallDate.Text.Trim() != "") ? txtCallDate.Text.Trim() : "";
            oPRP.ResolvedDate = (txtResolvedDate.Text.Trim() != "") ? txtResolvedDate.Text.Trim() : "";
            oPRP.ResolvedStatus = (ddlCallStatus.SelectedValue.ToString() != "SELECT") ? ddlCallStatus.SelectedValue.ToString() : "";
            oPRP.Remarks = txtRemarks.Text.Trim();
            oPRP.PartStatus = rdoRepair.Checked ? "REPAIR" : "REPLACED";
            oPRP.GatePassNo = txtGatePassNo.Text.Trim();
            oPRP.ReplacedSrlNo = txtReplacedSrlNo.Text.Trim();
            oPRP.EngrName = txtEngrName.Text.Trim();
            oPRP.ActionTaken = txtActionTaken.Text.Trim();
            oPRP.FRUNO = txtFRUNo.Text.Trim();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();

            if (oPRP.ResolvedStatus == "RESOLVED")
            {
                if (oPRP.CallDate != "" && oPRP.ResolvedDate != "")
                {
                    int iDate = clsGeneral.CompareDate(oPRP.CallDate, oPRP.ResolvedDate);
                    if (iDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Call Date should be earlier than resolved Date!');", true);
                        return;
                    }
                }
            }
            if (oPRP.CallDate != "" && oPRP.RespondedDate != "")
            {
                int iDate = clsGeneral.CompareDate(oPRP.CallDate, oPRP.RespondedDate);
                if (iDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Call Date should be earlier than Responded Date!');", true);
                    return;
                }
            }

            if (clsGeneral.OpType == "SAVE")
            {
                oPRP.RunningSerialNo = oDAL.GetMaxCallLogId(oPRP.CompCode);
                if (!oDAL.ValidateCallLogCode(txtCallLogCode.Text.Trim(), Session["COMPANY"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Call Log Code already exits.');", true);
                    return;
                }
                foreach (GridViewRow gvRow in gvAssets.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                    {
                        oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                        oPRP.SerialCode = ((Label)gvRow.FindControl("lblSerialCode")).Text.Trim();
                        oPRP.AssetMake = ((Label)gvRow.FindControl("lblAssetMake")).Text.Trim();
                        oPRP.ModelName = ((Label)gvRow.FindControl("lblModelName")).Text.Trim();
                        oDAL.SaveCallLogDetails(oPRP);
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Call log details saved successfully.');", true);
                GetCallLogCode();
            }
            else if (clsGeneral.OpType == "UPDATE")
            {
                if (clsGeneral._strRights[2] == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                    return;
                }
                foreach (GridViewRow gvRow in gvAssets.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                    {
                        oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                        oPRP.SerialCode = ((Label)gvRow.FindControl("lblSerialCode")).Text.Trim();
                        oPRP.AssetMake = ((Label)gvRow.FindControl("lblAssetMake")).Text.Trim();
                        oPRP.ModelName = ((Label)gvRow.FindControl("lblModelName")).Text.Trim();
                        oDAL.UpdateCallLogDetails(oPRP);
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Call log details updated successfully.');", true);
            }
            GetCallLogDetails(bPending);
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get assets list based on search criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            GetAssets();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Clear/reset page fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            GetCallLogCode();
            gvAssets.DataSource = null;
            gvAssets.DataBind();

            PopulateCategory(lblAssetType.Text);
            DataTable dt = new DataTable();
            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            dt = null;

            btnSearch.Enabled = true;
            lblAssetCount.Text = "Assets Count : 0";
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get asset details based on asset code entered.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (txtAssetCode.Text.Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = oDAL.GetAssetDetails(txtAssetCode.Text.Trim());
                if (dt.Rows.Count > 0)
                    txtSerialCode.Text = dt.Rows[0]["SERIAL_CODE"].ToString();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SELECTEDINDEXCHANGED EVENTS
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

    /// <summary>
    /// Populate vendor escalation details based on vendor name selected.
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
                dt = oDAL.GetEscalationDetails(ddlVendor.SelectedValue.ToString(), Session["COMPANY"].ToString());
                txtVendorLocation.Text = dt.Rows[0]["VENDOR_CITY"].ToString();
                txtContPerson.Text = dt.Rows[0]["VENDOR_CONT_PERSON"].ToString();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Populate model name based on asset make selected.
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
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Embed checkbox into asset grid for assets being selected for call log management.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssets_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkSelect = (CheckBox)e.Row.Cells[0].FindControl("chkSelectAsset");
                CheckBox chkHSelect = (CheckBox)this.gvAssets.HeaderRow.FindControl("chkHSelect");
                chkSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkHSelect.ClientID);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Assets grid page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssets.PageIndex = e.NewPageIndex;
            GetAssets();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Page index changing event for gatepass view gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCallLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvCallLog.PageIndex = e.NewPageIndex;
            GetCallLogDetails(bPending);
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Redirects to generate gatepass page in order to view/print gatepass details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCallLog_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "View")
            {
                string CallLogCode = e.CommandArgument.ToString();
                GetCallLogDetailsForUpdate(CallLogCode);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}