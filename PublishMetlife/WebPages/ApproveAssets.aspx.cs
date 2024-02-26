using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class ApproveAssets : System.Web.UI.Page
{
    string _CompCode = "";
    AssetAcquisition_DAL oDAL;
    AssetAcquisition_PRP oPRP;
    public ApproveAssets()
    {
        oPRP = new AssetAcquisition_PRP();
    }
    ~ApproveAssets()
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
        oDAL = new AssetAcquisition_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking group rights for viewing group rights details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("APPROVE_ASSETS", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "APPROVE_ASSETS");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
                lblAssetType.Text = clsGeneral.gStrAssetType;
                PopulateCategory(lblAssetType.Text);
                PopulateLocation();
                if (Session["GROUP"].ToString().ToUpper() == "SYSADMIN")
                    chkApproveAllAssets.Enabled = true;
                else
                    chkApproveAllAssets.Enabled = false;
                _CompCode = Session["COMPANY"].ToString();
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
    /// Get approved/unapproved assets counter.
    /// </summary>
    private void GetAssetsCount(string CompCode)
    {
        oPRP.CompCode = CompCode;
        if (ddlAssetMake.SelectedIndex != 0)
            oPRP.AssetMakeName = ddlAssetMake.SelectedValue.ToString();
        else
            oPRP.AssetMakeName = "";
        oPRP.AssetModelName = null;
        for (int iCnt = 0; iCnt < lstModelName.Items.Count; iCnt++)
        {
            if (lstModelName.Items[iCnt].Selected)
                oPRP.AssetModelName += lstModelName.Items[iCnt].Value.ToString() + ",";
        }
        if (oPRP.AssetModelName != null)
        {
            oPRP.AssetModelName = oPRP.AssetModelName.TrimEnd(',');
            oPRP.AssetModelName = oPRP.AssetModelName.Replace(",", "','");
            oPRP.AssetModelName = "'" + oPRP.AssetModelName + "'";
        }
        else
            oPRP.AssetModelName = "";
        if (ddlAssetType.SelectedIndex != 0)
            oPRP.AssetType = ddlAssetType.SelectedValue.ToString();
        else
            oPRP.AssetType = clsGeneral.gStrAssetType;
        if (ddlAssetCategory.SelectedIndex != 0)
            oPRP.AssetCategoryCode = ddlAssetCategory.SelectedValue.ToString();
        else
            oPRP.AssetCategoryCode = "";
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
        if (ddlOperationType.SelectedIndex != 0)
            oPRP.OperationType = ddlOperationType.SelectedValue.ToString();
        else
            oPRP.OperationType = "";

        string AssetCount = oDAL.GetAssetsCount(oPRP);
        string[] str = AssetCount.Split('^');
        lblUnApprovedAssets.Text = str[0];
        lblApprovedAssets.Text = str[1];
        lblUnApprovedAssets.Visible = true;
        lblApprovedAssets.Visible = true;
        lblUnApproved.Visible = true;
        lblApproved.Visible = true;
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
        dt = oDAL.GetLocation(Session["COMPANY"].ToString(), "", 1);
        ddlAssetLocation.DataSource = dt;
        ddlAssetLocation.DataTextField = "LOC_NAME";
        ddlAssetLocation.DataValueField = "LOC_CODE";
        ddlAssetLocation.DataBind();
        ddlAssetLocation.Items.Insert(0, "-- Select Location --");
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
    /// Get asset acquisition details for viewing.
    /// </summary>
    private void PopulateAssets()
    {
        oPRP.AssetModelName = null;
        oPRP.CompCode = Session["COMPANY"].ToString();
        oPRP.AssetCode = txtAssetCode.Text.Trim();
        oPRP.AssetSerialCode = txtSerialCode.Text.Trim();
        if (ddlAssetMake.SelectedIndex != 0)
            oPRP.AssetMakeName = ddlAssetMake.SelectedValue.ToString();
        else
            oPRP.AssetMakeName = "";
        for (int iCnt = 0; iCnt < lstModelName.Items.Count; iCnt++)
        {
            if (lstModelName.Items[iCnt].Selected)
                oPRP.AssetModelName += lstModelName.Items[iCnt].Value.ToString() + ",";
        }
        if (oPRP.AssetModelName != null)
        {
            oPRP.AssetModelName = oPRP.AssetModelName.TrimEnd(',');
            oPRP.AssetModelName = oPRP.AssetModelName.Replace(",", "','");
            oPRP.AssetModelName = "'" + oPRP.AssetModelName + "'";
        }
        else
            oPRP.AssetModelName = "";
        if (ddlAssetType.SelectedIndex != 0)
            oPRP.AssetType = ddlAssetType.SelectedValue.ToString();
        else
            oPRP.AssetType = clsGeneral.gStrAssetType;
        if (ddlAssetCategory.SelectedIndex != 0)
            oPRP.AssetCategoryCode = ddlAssetCategory.SelectedValue.ToString();
        else
            oPRP.AssetCategoryCode = "";
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
        if (ddlOperationType.SelectedIndex != 0)
            oPRP.OperationType = ddlOperationType.SelectedValue.ToString();
        else
            oPRP.OperationType = "";

        DataTable dt = new DataTable();
        dt = oDAL.GetAssetDetailsForApproval(oPRP);
        if (dt.Rows.Count > 0)
        {
            btnApprove.Visible = true;
            gvApproveAsset.DataSource = Session["ASSETS"] = dt;
            gvApproveAsset.DataBind();
        }
        else
        {
            btnApprove.Visible = false;
            gvApproveAsset.DataSource = null;
            gvApproveAsset.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset Not Found/Asset already approved.');", true);
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ex"></param>
    private void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Approve Assets");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Geting Asset Code and redirecting to asset acquisition page to show details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvApproveAsset_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "View")
            {
                string AssetCode = e.CommandArgument.ToString();
                Response.Redirect("AssetAcquisition.aspx?AssetCode=" + AssetCode.Trim());
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Checkbox checked/unchecked javascript event addition
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvApproveAsset_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkApprove = (CheckBox)e.Row.Cells[6].FindControl("chkApprove");
                CheckBox chkHApprove = (CheckBox)this.gvApproveAsset.HeaderRow.FindControl("chkHApprove");
                chkApprove.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkHApprove.ClientID);
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Group rights page index changing event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvApproveAsset_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ASSETS"];
            gvApproveAsset.PageIndex = e.NewPageIndex;
            gvApproveAsset.DataSource = dt;
            gvApproveAsset.DataBind();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message.ToString();
            if (!ex.Message.ToString().Contains("Thread was being aborted."))
            {
                //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
                clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
                catch { } Server.Transfer("Error.aspx");
            }
        }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Refresh/reset asset category.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshCategory_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateCategory(lblAssetType.Text.Trim());
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
    /// Reset/refresh location details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnRefreshLocation_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnSubmit.Enabled = true;
            lblLocLevel.Text = "1";
            PopulateLocation();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Reset/Crear fields and set location details refreshed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            btnApprove.Visible = false;
            btnSubmit.Enabled = true;
            lblLocLevel.Text = "1";
            PopulateLocation();
            PopulateCategory(lblAssetType.Text);

            DataTable dt = new DataTable();
            PopulateCategory(lblAssetType.Text.Trim());
            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            dt = null;

            gvApproveAsset.DataSource = null;
            gvApproveAsset.DataBind();
            GetAssetsCount(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get a list of unapproved assets based on search criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[0] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
            }
            else
            {
                PopulateAssets();
                GetAssetsCount(Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Approve assets.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            bool bAssetSelected = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation.');", true);
                return;
            }
            else
            {
                if (chkApproveAllAssets.Checked)
                {
                    bool bResp = false;
                    oPRP.CompCode = Session["COMPANY"].ToString();
                    oPRP.AssetCode = txtAssetCode.Text.Trim();
                    oPRP.AssetSerialCode = txtSerialCode.Text.Trim();
                    if (ddlAssetMake.SelectedIndex != 0)
                        oPRP.AssetMakeName = ddlAssetMake.SelectedValue.ToString();
                    else
                        oPRP.AssetMakeName = "";
                    for (int iCnt = 0; iCnt < lstModelName.Items.Count; iCnt++)
                    {
                        if (lstModelName.Items[iCnt].Selected)
                            oPRP.AssetModelName += lstModelName.Items[iCnt].Value.ToString() + ",";
                    }
                    if (oPRP.AssetModelName != null)
                    {
                        oPRP.AssetModelName = oPRP.AssetModelName.TrimEnd(',');
                        oPRP.AssetModelName = oPRP.AssetModelName.Replace(",", "','");
                        oPRP.AssetModelName = "'" + oPRP.AssetModelName + "'";
                    }
                    else
                        oPRP.AssetModelName = "";
                    if (ddlAssetType.SelectedIndex != 0)
                        oPRP.AssetType = ddlAssetType.SelectedValue.ToString();
                    else
                        oPRP.AssetType = clsGeneral.gStrAssetType;
                    if (ddlAssetCategory.SelectedIndex != 0)
                        oPRP.AssetCategoryCode = ddlAssetCategory.SelectedValue.ToString();
                    else
                    {
                        if (lblCatCode.Text.Trim() == "0")
                            oPRP.AssetCategoryCode = "";
                        else
                            oPRP.AssetCategoryCode = lblCatCode.Text.Trim();
                    }
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
                    bResp = oDAL.ApproveAssetDetails(oPRP, "APPROVE_ALL");
                    if (bResp)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Selected assets approved successfully.');", true);
                }
                else
                {
                    for (int iCnt = 0; iCnt < gvApproveAsset.Rows.Count; iCnt++)
                    {
                        if (((CheckBox)gvApproveAsset.Rows[iCnt].FindControl("chkApprove")).Checked == true)
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
                    bool bResp = false;
                    foreach (GridViewRow gvRow in gvApproveAsset.Rows)
                    {
                        if (((CheckBox)gvRow.FindControl("chkApprove")).Checked)
                        {
                            oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                            oPRP.AssetSerialCode = ((Label)gvRow.FindControl("lblAssetSrlCode")).Text.Trim();
                            bResp = oDAL.ApproveAssetDetails(oPRP);
                        }
                    }
                    if (bResp)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Selected assets approved successfully.');", true);
                }
            }
            PopulateAssets();
            GetAssetsCount(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get asset details baed on asset code provided.
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
                dt = oDAL.GetAssetDetails(txtAssetCode.Text.Trim(), Session["COMPANY"].ToString());
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
                DataTable dt = oDAL.GetLocation(Session["COMPANY"].ToString(), sLocCode, iLocLevel);
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
                    ddlOperationType.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
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
}