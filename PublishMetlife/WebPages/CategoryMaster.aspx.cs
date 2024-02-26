using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class CategoryMaster : System.Web.UI.Page
{
    string _AssetType = "";
    CategoryMaster_PRP oPRP;
    CategoryMaster_DAL oDAL;
    public CategoryMaster()
    {
        oPRP = new CategoryMaster_PRP();
    }
    ~CategoryMaster()
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
        oDAL = new CategoryMaster_DAL(Session["DATABASE"].ToString());
        txtCategoryCode.Focus();
    }

    /// <summary>
    /// Checking user group rights for category master view operation, populating dropdownlist 
    /// with existing categories and populating gridview with category master details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("CATEGORY_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "CATEGORY_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                PopulateCategory();
                GetCategoryDetails();
            }
            txtCategoryCode.Focus();
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
        clsGeneral.LogErrorToLogFile(ex, "Category Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Populate all existing categories in order to create parent category
    /// for the new created category.
    /// </summary>
    private void PopulateCategory()
    {
        lblCatCode.Text = "0";
        lblCatLevel.Text = "1";
        ddlParentCategory.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetCategoryCode(rdoITAsset.Checked ? "IT" : "ADMIN", "", 1);
        ddlParentCategory.DataSource = dt;
        ddlParentCategory.DataTextField = "CATEGORY_NAME";
        ddlParentCategory.DataValueField = "CATEGORY_CODE";
        ddlParentCategory.DataBind();
        ddlParentCategory.Items.Insert(0, "-- Select Category --");
    }

    /// <summary>
    /// Fetch and populate gridview with category master details.
    /// </summary>
    private void GetCategoryDetails()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetCategory();
        gvCategoryMaster.DataSource = Session["CategoryMaster"] = dt;
        gvCategoryMaster.DataBind();
    }
    #endregion

    #region GRID EVENTS
    /// <summary>
    /// Delete a category.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCategoryMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvCategoryMaster.Rows[e.RowIndex];
            oPRP.CategoryCode = ((Label)gvRow.FindControl("lblCatCode")).Text.Trim();
            string DelRslt = oDAL.DeleteCategory(oPRP.CategoryCode);
            if (DelRslt == "CHILD_FOUND")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Category has child categories, hence cannot be deleted.');", true);
                return;
            }
            if (DelRslt == "CATEGORY_IN_USE")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Category is associated with assets, hence cannot be deleted.');", true);
                return;
            }
            if (DelRslt == "SUCCESS")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Category deleted successfully.');", true);
            _AssetType = rdoAdminAsset.Checked ? "ADMIN" : "IT";
            PopulateCategory();
            GetCategoryDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get category master details in edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCategoryMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            gvCategoryMaster.EditIndex = e.NewEditIndex;
            GetCategoryDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCategoryMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imagebuttonEdit = (ImageButton)e.Row.FindControl("imagebuttonEdit");
                ImageButton imagebuttonDelete = (ImageButton)e.Row.FindControl("imagebuttonDelete");
                if (imagebuttonEdit != null)
                {
                    if (clsGeneral._strRights[2] == "0")
                        imagebuttonEdit.Enabled = false;
                    else
                        imagebuttonEdit.Enabled = true;
                }
                if (imagebuttonDelete != null)
                {
                    if (clsGeneral._strRights[3] == "0")
                        imagebuttonDelete.Enabled = false;
                    else
                        imagebuttonDelete.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Update category master details through gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCategoryMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvCategoryMaster.Rows[e.RowIndex];
            oPRP.CategoryCode = ((Label)gvRow.FindControl("lblECatCode")).Text.Trim();
            oPRP.CategoryName = ((TextBox)gvRow.FindControl("txtECatName")).Text.Trim();
            oPRP.ParentCategory = ((Label)gvRow.FindControl("lblEParentCategory")).Text.Trim();
            oPRP.CategoryType = "";
            oPRP.CategoryLevel = int.Parse(((Label)gvRow.FindControl("lblECatLevel")).Text.Trim());
            oPRP.AssetType = ((Label)gvRow.FindControl("lblEAssetType")).Text.Trim();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oPRP.Remarks = ((TextBox)gvRow.FindControl("txtERemarks")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
            bool bResp = oDAL.SaveUpdateCategory("UPDATE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUpdateErrMsg", "ShowUpdateErrMsg();", true);
                txtCategoryCode.Focus();
            }
            gvCategoryMaster.EditIndex = -1;
            GetCategoryDetails();
            _AssetType = rdoAdminAsset.Checked ? "ADMIN" : "IT";
            PopulateCategory();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Cancel category master details from edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCategoryMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            gvCategoryMaster.EditIndex = -1;
            GetCategoryDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Category master gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPageIndex(object sender, GridViewPageEventArgs e)
    {
        gvCategoryMaster.PageIndex = e.NewPageIndex;
        GetCategoryDetails();
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Save category master details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string _AssetTypeCode = "";
            string _CatCode = "";
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (oDAL.CheckCategoryInitials(txtCategoryCode.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Category initials already exists.');", true);
                return;
            }
            oPRP.CategoryInitials = txtCategoryCode.Text.ToUpper().Trim();
            oPRP.CompCode = Session["COMPANY"].ToString();
            _AssetTypeCode = rdoAdminAsset.Checked ? "AD" : "IT";
            if (lblCatCode.Text.Trim() != "0")
            {
                oPRP.CategoryCode = lblCatCode.Text.Trim().ToUpper();
                DataTable dt = oDAL.GetCategoryCodeLevel(oPRP.CategoryCode);
                if (dt.Rows.Count > 0)
                {
                    string[] _CatParts = Convert.ToString(dt.Rows[0]["CATEGORY_CODE"]).Trim().Split('-');
                    if (Convert.ToInt16(dt.Rows[0]["CATEGORY_LEVEL"]) == 1)
                    {
                        _CatCode = _CatParts[0].Trim() + "-" + _CatParts[1].Trim() + "-" + txtCategoryCode.Text.Trim().ToUpper() + "-00";
                        oPRP.CategoryLevel = 2;
                    }
                    else if (Convert.ToInt16(dt.Rows[0]["CATEGORY_LEVEL"]) == 2)
                    {
                        _CatCode = _CatParts[0].Trim() + "-" + _CatParts[1].Trim() + "-" + _CatParts[2].Trim() + "-" + txtCategoryCode.Text.Trim().ToUpper();
                        oPRP.CategoryLevel = 3;
                    }
                    else if (Convert.ToInt16(dt.Rows[0]["CATEGORY_LEVEL"]) == 3)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowLevelMsg", "ShowLevelMsg();", true);
                        return;
                    }
                }
            }
            else
            {
                _CatCode = _AssetTypeCode + "-" + txtCategoryCode.Text.Trim().ToUpper() + "-00-00";
                oPRP.CategoryLevel = 1;
            }
            oPRP.CategoryCode = _CatCode;
            oPRP.CategoryName = txtCategoryName.Text.Trim();
            oPRP.CategoryType = "";
            oPRP.ParentCategory = ViewState["ParentCatCode"] != null ? (string)ViewState["ParentCatCode"] : "";
            oPRP.AssetType = rdoAdminAsset.Checked ? "ADMIN" : "IT";
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.Remarks = txtCategoryRemarks.Text.Trim();
            oPRP.Active = chkSetStatus.Checked;
            bool bResp = oDAL.SaveUpdateCategory("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Category details are not saved.');", true);
                txtCategoryCode.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                _AssetType = rdoAdminAsset.Checked ? "ADMIN" : "IT";
                PopulateCategory();
            }
            ViewState["ParentCatCode"] = null;
            PopulateCategory();
            GetCategoryDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export category master grid data into excel file.
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
            if (gvCategoryMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["CategoryMaster"];
                dt.TableName = "CategoryMasterReport";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'> Category Master</font></u></b>");
                //hw.WriteLine("<br>");
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=CategoryMaster.xls");  
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
    /// Clear/reset page fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ParentCatCode"] = null;
            if (clsGeneral.gStrAssetType == "IT")
                rdoITAsset.Checked = true;
            else
                rdoAdminAsset.Checked = true;
            PopulateCategory();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh/reset category to top level in dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshCategory_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateCategory();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SELECTED INDEX CHANGED EVENTS
    /// <summary>
    /// Populate sub-categories based on parent ctegory selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlParentCategory.SelectedIndex > 0)
            {
                int CatLevel = int.Parse(lblCatLevel.Text.Trim());
                lblCatLevel.Text = (CatLevel + 1).ToString();
                int iCatLevel = int.Parse(lblCatLevel.Text.Trim());
                string sCatCode = ddlParentCategory.SelectedValue.ToString();
                ViewState["ParentCatCode"] = sCatCode;
                lblCatCode.Text = sCatCode;

                ddlParentCategory.DataSource = null;
                DataTable dt = oDAL.GetCategoryCode(rdoAdminAsset.Checked ? "ADMIN" : "IT", sCatCode, iCatLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlParentCategory.DataSource = dt;
                    ddlParentCategory.DataValueField = "CATEGORY_CODE";
                    ddlParentCategory.DataTextField = "CATEGORY_NAME";
                    ddlParentCategory.DataBind();
                    ddlParentCategory.Items.Insert(0, "-- Select Category --");
                    ddlParentCategory.Focus();
                }
                else
                {
                    iCatLevel = iCatLevel - 1;
                    lblCatLevel.Text = iCatLevel.ToString();
                    txtCategoryRemarks.Focus();
                }
            }
        }
        catch(Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region CHECKEDCHANGED EVENTS
    /// <summary>
    /// populate category when ADMIN asset radio button is checked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoAdminAsset_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoAdminAsset.Checked)
            {
                PopulateCategory();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// populate category when IT asset radio button is checked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoITAsset_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoITAsset.Checked)
            {
                PopulateCategory();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}