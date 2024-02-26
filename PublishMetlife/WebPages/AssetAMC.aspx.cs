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

public partial class AssetAMC : System.Web.UI.Page
{
    AssetAMC_DAL oDAL;
    AssetAMC_PRP oPRP;
    public AssetAMC()
    {
        oPRP = new AssetAMC_PRP();
    }
    ~AssetAMC()
    {
        oPRP = null;
        oDAL = null;
    }

    #region PAGE EVENTS
    /// <summary>
    /// Navigates to session expired page in case of user logs off
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new AssetAMC_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking user group rights for creating AMC for selected assets.
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
                string _strRights = clsGeneral.GetRights("GENERATE_AMC", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx");
                }
                PopulateVendor();
                GetAssetsForAMC();
                clsGeneral.OpType = "SAVE";
                if (Request.QueryString["AMCCode"] != null)
                {
                    btnClear.Enabled = false;
                    clsGeneral.OpType = "UPDATE";
                    string AMCCode = Request.QueryString["AMCCode"].ToString();
                    PopulateAMCDetails(AMCCode);
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Popuate AMC details for edit/update operation.
    /// </summary>
    /// <param name="AMCCode"></param>
    private void PopulateAMCDetails(string AMCCode)
    {
        oPRP.AMCCode = AMCCode.Trim();
        txtAMCCode.Text = AMCCode.Trim();
        CollapsiblePanelExtender1.Collapsed = false;
        CollapsiblePanelExtender1.ClientState = "false";
        DataTable dt = oDAL.GetAMCDetailsForUpdate(oPRP.AMCCode);
        if (dt.Rows.Count > 0)
        {
            ddlVendor.SelectedValue = dt.Rows[0]["AMC_VENDOR_CODE"].ToString();
            txtAMCValue.Text = dt.Rows[0]["AMC_VALUE"].ToString();
            txtStartDate.Text = Convert.ToDateTime(dt.Rows[0]["AMC_START_DATE"].ToString()).ToString("dd/MMM/yyyy");
            txtPurchaseDate.Text = Convert.ToDateTime(dt.Rows[0]["AMC_PURCHASE_DATE"].ToString()).ToString("dd/MMM/yyyy");
            txtEndDate.Text = Convert.ToDateTime(dt.Rows[0]["AMC_END_DATE"].ToString()).ToString("dd/MMM/yyyy");
            txtRespPerson.Text = dt.Rows[0]["AMC_RESP_PERSON"].ToString();
            txtWarranty.Text = dt.Rows[0]["AMC_WARRANTY"].ToString();
            txtAMCDoc.Text = dt.Rows[0]["AMC_DOCUMENT"].ToString();
            
            if (txtAMCDoc.Text != "")
            {
                txtAMCDoc.Enabled = true;
                btnDownload.Enabled = true;
            }
            else
            {
                txtAMCDoc.Enabled = false;
                UploadRefDoc.Enabled = false;
                btnDownload.Enabled = false;
            }
        }
        rdoInAMC.Checked = true;
        rdoNotInAMC.Checked = false;
        GetAssetsForAMC();
    }
    
    /// <summary>
    /// Get vendor code/name for vendor selection.
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
    /// Get category code/name as per asset type selection.
    /// </summary>
    private void PopulateCategory(string AssetType)
    {
        ddlAssetCategory.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetCategory(AssetType);
        ddlAssetCategory.DataSource = dt;
        ddlAssetCategory.DataTextField = "CATEGORY_NAME";
        ddlAssetCategory.DataValueField = "CATEGORY_CODE";
        ddlAssetCategory.DataBind();
        ddlAssetCategory.Items.Insert(0, "-- Select Category --");
    }

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Generate Asset AMC Details");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Get assets to be included into the AMC.
    /// </summary>
    private void GetAssetsForAMC()
    {
        if (ddlAssetCategory.SelectedIndex != 0)
            oPRP.CategoryCode = ddlAssetCategory.SelectedValue.ToString();
        if (ddlAssetType.SelectedIndex != 0)
        {
            if (ddlAssetType.SelectedValue.ToString() == "AD")
                oPRP.AssetType = "ADMIN";
            else if (ddlAssetType.SelectedValue.ToString() == "IT")
                oPRP.AssetType = "IT";
        }
        oPRP.AssetID = txtAssetID.Text.Trim();
        oPRP.SerialCode = txtSerialCode.Text.Trim();
        oPRP.PONo = txtAssetPONo.Text.Trim();
        oPRP.AMCCode = txtAMCCode.Text.Trim();
        gvAMCAssets.DataSource = null;
        if (rdoInAMC.Checked)
            oPRP.bThisAMC = true;
        else
            oPRP.bThisAMC = false;
        DataTable dt = oDAL.GetAssetsForAMC(oPRP);
        if (dt.Rows.Count > 0)
        {
            gvAMCAssets.DataSource = dt;
            gvAMCAssets.DataBind();
            if (oPRP.AMCCode != "" && rdoInAMC.Checked)
            {
                ((CheckBox)gvAMCAssets.HeaderRow.Cells[0].FindControl("chkHSelect")).Checked = true;
                foreach (GridViewRow gvRow in gvAMCAssets.Rows)
                    ((CheckBox)gvRow.FindControl("chkSelect")).Checked = true;
            }
        }
        else
        {
            gvAMCAssets.DataSource = null;
            gvAMCAssets.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There are no assets found for given criterian.');", true);
            return;
        }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Save new asset AMC details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            bool bRslt = false;
            int iDate = clsGeneral.CompareDate(txtStartDate.Text.Trim(), txtEndDate.Text.Trim());
            if (iDate > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Start Date cannot be greater than end date.');", true);
                return;
            }
            oPRP.AMCVendorCode = ddlVendor.SelectedValue.ToString();
            oPRP.AMCValue = txtAMCValue.Text.Trim();
            oPRP.AMCWarranty = txtWarranty.Text.Trim();
            oPRP.PurchaseDate = Convert.ToDateTime(txtPurchaseDate.Text.Trim());
            oPRP.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
            oPRP.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
            oPRP.RespPerson = txtRespPerson.Text.Trim();
            oPRP.RefDocName = UploadRefDoc.FileName;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.AssetsToAdd = rdoNotInAMC.Checked;
            oPRP.Assets = new ArrayList();

            foreach (GridViewRow gvRow in gvAMCAssets.Rows)
            {
                if (rdoNotInAMC.Checked)
                {
                    if (((CheckBox)gvRow.FindControl("chkSelect")).Checked)
                    {
                        oPRP.Assets.Add(((Label)gvRow.FindControl("lblAssetCode")).Text.Trim());
                    }
                }
                else
                {
                    if (((CheckBox)gvRow.FindControl("chkSelect")).Checked == false)
                    {
                        oPRP.AMC_CODE.Add(((Label)gvRow.FindControl("lblAMCCode")).Text.Trim());
                    }
                }
            }
            if (clsGeneral.OpType == "SAVE")
            {
                if (oPRP.Assets.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There are no assets selected for the AMC.');", true);
                    return;
                }
                int _iMaxRunningNo = oDAL.GetMaxAMCRunningNo();
                oPRP.AMCCode = oPRP.CompCode + "-AMC-" + _iMaxRunningNo.ToString().PadLeft(6, '0');
                bRslt = oDAL.SaveUpdateAMCDetails(oPRP, clsGeneral.OpType);
            }
            if (clsGeneral.OpType == "UPDATE")
            {
                if (clsGeneral._strRights[2] == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                    return;
                }
                oPRP.AMCCode = txtAMCCode.Text.Trim();
                oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
                bRslt = oDAL.SaveUpdateAMCDetails(oPRP, clsGeneral.OpType);
            }
            if (bRslt)
            {
                Response.Redirect("ViewAMC.aspx");
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Search for assets to be included with a new AMC.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GetAssetsForAMC();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (txtSerialCode.Text.Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = oDAL.GetAssetDetails(txtSerialCode.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    txtAssetID.Text = dt.Rows[0]["ASSET_ID"].ToString();
                    txtAssetPONo.Text = dt.Rows[0]["PO_NUMBER"].ToString();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Download amc document attached with particular amc.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDownload_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAMCAssets_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkSelect = (CheckBox)e.Row.Cells[0].FindControl("chkSelect");
                CheckBox chkHSelect = (CheckBox)this.gvAMCAssets.HeaderRow.FindControl("chkHSelect");
                chkSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkHSelect.ClientID);
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
    protected void gvAMCAssets_PageIndexChanging(object sender,GridViewPageEventArgs e)
    {
        try
        {
            gvAMCAssets.PageIndex = e.NewPageIndex;
            GetAssetsForAMC();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SELECTEDINDEXCHANGED EVENT
    /// <summary>
    /// Get category based on selection of asset type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetType.SelectedValue.ToString() == "AD")
            {
                oPRP.AssetType = "ADMIN";
                PopulateCategory("ADMIN");
            }
            else if (ddlAssetType.SelectedValue.ToString() == "IT")
            {
                oPRP.AssetType = "IT";
                PopulateCategory("IT");
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region CHECKEDCHANGED EVENTS
    /// <summary>
    /// Get assets which are not included in the selected AMC.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoNotInAMC_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GetAssetsForAMC();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    
    /// <summary>
    /// Get assets which are included in the selected AMC.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoInAMC_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GetAssetsForAMC();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}