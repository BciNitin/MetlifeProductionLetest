using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class ViewAssetList : System.Web.UI.Page
{
    string _CompCode = "";
    AssetAcquisition_DAL oDAL;
    AssetAcquisition_PRP oPRP;
    public ViewAssetList()
    {
        oPRP = new AssetAcquisition_PRP();
    }
    ~ViewAssetList()
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
        _CompCode = Session["COMPANY"].ToString();
    }

    /// <summary>
    /// 
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
                string _strRights = clsGeneral.GetRights("VIEW_ASSET_LIST", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "VIEW_ASSET_LIST");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                PopulateLocation();
                //ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
                //lblAssetType.Text = clsGeneral.gStrAssetType;
                PopulateCategory("");
                new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "ASSET List", "View Asset List", "Asset  List view by user id " + Session["CURRENTUSER"].ToString() + "");
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
        clsGeneral.LogErrorToLogFile(ex, "View Asset Acquisition List");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Get acquired assets' details for viewing/editing/deleting.
    /// </summary>
    private void PopulateAssets()
    {
        string[] LocParts = { };
        oPRP.CompCode = Session["COMPANY"].ToString();
        oPRP.AssetCode = txtAssetCode.Text.Trim();
       // oPRP.AssetTag = txtSerialCode.Text.Trim();
        //oPRP.AssetID = txtFAMSId.Text.Trim();
        oPRP.PurchaseOrderNo = txtPONo.Text.Trim();

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

        //if (ddlAssetType.SelectedIndex != 0)
        //    oPRP.AssetType = ddlAssetType.SelectedValue.ToString();
        //else
        //    oPRP.AssetType = clsGeneral.gStrAssetType;

        if (ddlAssetCategory.SelectedIndex != 0)
            oPRP.AssetCategoryCode = ddlAssetCategory.SelectedValue.ToString();
        else
            oPRP.AssetCategoryCode = "";
        if (lblLocCode.Text != "")
        {
            oPRP.AssetLocation = lblLocCode.Text;
            //LocParts = lblLocCode.Text.Trim().Split('-');
            //if (LocParts[2] == "00")
            //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1];
            //else if (LocParts[3] == "00")
            //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
            //else if (LocParts[4] == "00")
            //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
            //else if (LocParts[5] == "00")
            //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
            //else
            //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
        }
        else
            oPRP.AssetLocation = "";

        //if (ddlVerNonVer.SelectedIndex != 0)
        //    oPRP.VerifiableType = ddlVerNonVer.SelectedValue.ToString();
        //else
        //    oPRP.VerifiableType = "";
        //if (ddlAMCWarranty.SelectedIndex != 0)
        //    oPRP.AMC_Warranty = ddlAMCWarranty.SelectedValue.ToString();
        //else
        //    oPRP.AMC_Warranty = "";
        //if (rdoApproved.Checked)
        //    oPRP.Asset_Approved = true;
        //else if (rdoUnApproved.Checked)
        //    oPRP.Asset_Approved = false;

        //if (rdoScrapped.Checked)
        //{ oPRP.Sold_Scrapped = "SCRAPPED"; }
        //else if (rdoSold.Checked)
        //{ oPRP.Sold_Scrapped = "SOLD"; }
        //else
        //    oPRP.Sold_Scrapped = "";

        DataTable dt = new DataTable();
        dt = oDAL.GetViewAssetDetails(oPRP);
        if (dt.Rows.Count > 0)
        {
            gvViewAsset.DataSource = Session["VIEW_ASSETS"] = dt;
            gvViewAsset.DataBind();
            btnBulkDeleteAssets.Visible = true;
            lblAssetCount.Visible = true;
            lblAssetCount.Text = "Total Assets Count : " + dt.Rows.Count.ToString();
        }
        else
        {
            gvViewAsset.DataSource = null;
            gvViewAsset.DataBind();
            btnBulkDeleteAssets.Visible = false;
            lblAssetCount.Text = "Total Assets Count : 0";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Either asset belongs to another location or search criteria is incorrect.');", true);
            return;
        }
    }

    /// <summary>
    /// Populate asset make list when asset category is provided.
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
    #endregion

    #region SELECTEDINDEXCHANGED EVENTS
    /// <summary>
    /// Get Sub location details to be populated into dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlAssetLocation.SelectedIndex != 0)
            //{
            //    int locLevel = int.Parse(lblLocLevel.Text.Trim());
            //    lblLocLevel.Text = (locLevel + 1).ToString();
            //    int iLocLevel = int.Parse(lblLocLevel.Text.Trim());
            //    string sLocCode = ddlAssetLocation.SelectedValue.ToString();
            //    lblLocCode.Text = ddlAssetLocation.SelectedValue.ToString();

            //    ddlAssetLocation.DataSource = null;
            //    DataTable dt = oDAL.GetLocation(_CompCode, sLocCode, iLocLevel);
            //    if (dt.Rows.Count > 0)
            //    {
            //        ddlAssetLocation.DataSource = dt;
            //        ddlAssetLocation.DataValueField = "LOC_CODE";
            //        ddlAssetLocation.DataTextField = "LOC_NAME";
            //        ddlAssetLocation.DataBind();
            //        ddlAssetLocation.Items.Insert(0, "-- Select Location --");
            //        ddlAssetLocation.Focus();
            //    }
            //    else
            //    {
            //        iLocLevel = iLocLevel - 1;
            //        lblLocLevel.Text = iLocLevel.ToString();
                   
            //    }
            //}
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
            DataTable dtNull = new DataTable();
            ddlAssetCategory.DataSource = dtNull;
            ddlAssetCategory.DataBind();
            ddlAssetMake.DataSource = dtNull;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dtNull;
            lstModelName.DataBind();
            dtNull = null;

            //if (ddlAssetType.SelectedValue.ToString() == "ADMIN")
            //{
            //    lblAssetType.Text = "ADMIN";
            //    PopulateCategory(lblAssetType.Text);
            //}
            //else if (ddlAssetType.SelectedValue.ToString() == "IT")
            //{
            //    lblAssetType.Text = "IT";
            //    PopulateCategory(lblAssetType.Text);
            //}
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
                
                //ddlAssetCategory.DataSource = null;
                //DataTable dt = oDAL.PopulateCategory("", sCatCode, iCatLevel);
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
        {
            HandleExceptions(ex);
        }
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
            PopulateLocation();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Fetch asset details.
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
                PopulateAssets();
            }
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
    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            txtAssetCode.Text = "";
            PopulateLocation();
            txtPONo.Text = "";

            DataTable dt = new DataTable();
            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            dt = null;

            gvViewAsset.DataSource = null;
            gvViewAsset.DataBind();
            lblAssetCount.Visible = false;
            btnBulkDeleteAssets.Visible = false;
            PopulateLocation();
            PopulateCategory("");
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[4] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (gvViewAsset.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["VIEW_ASSETS"];
                dt.TableName = "AssetReport";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=ViewAssetList.xls");
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
        {
            HandleExceptions(ex);
        }
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
            if (txtAssetCode.Text.Trim() != "")
            {
                DataTable dt = new DataTable();
                string tagid = txtAssetCode.Text.Trim();
                if (txtAssetCode.Text.Trim().Contains("|"))
                    tagid = txtAssetCode.Text.Trim().Split('|')[0];
                dt = oDAL.GetAssetDetails(tagid, Session["COMPANY"].ToString());
                if (dt.Rows.Count > 0)
                {
                  //  txtSerialCode.Text = dt.Rows[0]["SERIAL_CODE"].ToString();
                   // txtFAMSId.Text = dt.Rows[0]["ASSET_ID"].ToString();
                    txtPONo.Text = dt.Rows[0]["PO_NUMBER"].ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Either asset code belongs to another location or asset code does not exist.');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Bulk deletion of assets as per given filters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBulkDeleteAssets_Click(object sender, EventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                string[] LocParts = { };
                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.AssetCode = txtAssetCode.Text.Trim();
               // oPRP.AssetSerialCode = txtSerialCode.Text.Trim();
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

                //if (ddlAssetType.SelectedIndex != 0)
                //{
                //    if (ddlAssetType.SelectedValue.ToString() == "ADMIN")
                //        oPRP.AssetType = "ADMIN";
                //    if (ddlAssetType.SelectedValue.ToString() == "IT")
                //        oPRP.AssetType = "IT";
                //}
                if (ddlAssetCategory.SelectedIndex != 0)
                    oPRP.AssetCategoryCode = ddlAssetCategory.SelectedValue.ToString();
                else
                    oPRP.AssetCategoryCode = "";
                if (ddlAssetLocation.SelectedIndex>0)
                {
                    oPRP.AssetLocation = ddlAssetLocation.SelectedValue;
                    //LocParts = lblLocCode.Text.Trim().Split('-');
                    //if (LocParts[2] == "00")
                    //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1];
                    //else if (LocParts[3] == "00")
                    //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
                    //else if (LocParts[4] == "00")
                    //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
                    //else if (LocParts[5] == "00")
                    //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
                    //else
                    //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
                }
                else
                    oPRP.AssetLocation = "";

                //if (ddlVerNonVer.SelectedIndex != 0)
                //    oPRP.VerifiableType = ddlVerNonVer.SelectedValue.ToString();
                //else
                //    oPRP.VerifiableType = "";

                //if (ddlAMCWarranty.SelectedIndex != 0)
                //    oPRP.AMC_Warranty = ddlAMCWarranty.SelectedValue.ToString();
                //else
                //    oPRP.AMC_Warranty = "";

                //if (rdoApproved.Checked)
                //    oPRP.Asset_Approved = true;
                //else if (rdoUnApproved.Checked)
                //    oPRP.Asset_Approved = false;

                //if (rdoScrapped.Checked)
                //{ oPRP.Sold_Scrapped = "SCRAPPED"; }
                //else if (rdoSold.Checked)
                //{ oPRP.Sold_Scrapped = "SOLD"; }
                //else
                //    oPRP.Sold_Scrapped = "";
                int iRes = oDAL.DeleteAssetsInBulk(oPRP);
                if (iRes > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Assets are deleted in bulk as per given criteria.');", true);
                    lblAssetCount.Visible = false;
                }
                oPRP = new AssetAcquisition_PRP();
                PopulateAssets();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Asset deletion from asset acquisition if not allocated.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewAsset_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //try
        //{
        //    if (clsGeneral._strRights[3] == "0")
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
        //        return;
        //    }
        //    else
        //    {
        //        GridViewRow gvRow = (GridViewRow)gvViewAsset.Rows[e.RowIndex];
        //        oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
        //        oPRP.CompCode = Session["COMPANY"].ToString();
        //        string DelRslt = oDAL.DeleteAssetDetails(oPRP.AssetCode, oPRP.CompCode);
        //        if (DelRslt == "ALLOCATED")
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Allocated assets cannot be deleted.');", true);
        //            return;
        //        }
        //        if (DelRslt == "SUCCESS")
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset details deleted successfully.');", true);
        //        PopulateAssets();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    HandleExceptions(ex);
        //}
    }

    /// <summary>
    /// View assets gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewAsset_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["VIEW_ASSETS"];
            gvViewAsset.PageIndex = e.NewPageIndex;
            gvViewAsset.DataSource = dt;
            gvViewAsset.DataBind();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Redirect to asset acquisition page for update operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewAsset_RowCommand(object sender, GridViewCommandEventArgs e)
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
    #endregion
}