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

public partial class PageMaster : System.Web.UI.Page
{
    GroupRights_DAL oDAL;
    GroupRights_PRP oPRP;
    public PageMaster()
    {
        oPRP = new GroupRights_PRP();
    }
    ~PageMaster()
    {
        oDAL = null; oPRP = null;
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
        oDAL = new GroupRights_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking group rights for page master details to be viewed.
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
                string _strRights = clsGeneral.GetRights("PAGE_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                if (clsGeneral._strRights[0] == "0" || clsGeneral._strRights[1] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx");
                }
                PopulateGroup(Session["COMPANY"].ToString());
                GetPageGroupDetails();
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message.ToString();
            if (!ex.Message.ToString().Contains("Thread was being aborted."))
            {
                clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
                catch { } Server.Transfer("Error.aspx");
            }
        }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Fetch group details to be populated.
    /// </summary>
    private void PopulateGroup(string _CompCode)
    {
        ddlGroup.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetGroup(_CompCode);
        ddlGroup.DataSource = dt;
        ddlGroup.DataValueField = "GROUP_CODE";
        ddlGroup.DataTextField = "GROUP_NAME";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, "SELECT");
    }

    /// <summary>
    /// Fetch page master and corresponding group details.
    /// </summary>
    private void GetPageGroupDetails()
    {
        gvPageMaster.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetPageGroupDetails();
        gvPageMaster.DataSource = ViewState["PageMaster"] = dt;
        gvPageMaster.DataBind();
    }
    #endregion

    #region SUBMIT EVENT
    /// <summary>
    /// Save page master details into database table.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            bool bResp = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.PageName = txtPageName.Text.Trim().ToUpper();
                oPRP.PageDesc = txtPageDesc.Text.Trim();
                oPRP.GroupCode = ddlGroup.SelectedValue.ToString();
                oPRP.ViewRight = rdoView.Checked;
                oPRP.SaveRight = rdoSave.Checked;
                oPRP.EditRight = rdoEdit.Checked;
                oPRP.DeleteRight = rdoDelete.Checked;
                oPRP.ExportRight = rdoExport.Checked;
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                bResp = oDAL.SaveUpdatePageGroup("SAVE", oPRP);
                if (!bResp)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                    upSubmit.Update();
                }
                GetPageGroupDetails();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Populate Group code/name while in edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPageMaster_RowDataBound(object sender,GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlEGroup = (DropDownList)e.Row.FindControl("ddlEGroup");
                if (ddlEGroup != null)
                {
                    ddlEGroup.DataSource = oDAL.GetGroup(Session["COMPANY"].ToString());
                    ddlEGroup.DataTextField = "GROUP_NAME";
                    ddlEGroup.DataValueField = "GROUP_CODE";
                    ddlEGroup.DataBind();
                    ddlEGroup.Items.Insert(0, "SELECT");
                    ddlEGroup.SelectedIndex = ddlEGroup.Items.IndexOf(ddlEGroup.Items.FindByText(ViewState["GROUP"].ToString()));
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Delete a page master record from data tables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPageMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                GridViewRow gvRow = (GridViewRow)gvPageMaster.Rows[e.RowIndex];
                oPRP.PageCode = int.Parse(((Label)gvRow.FindControl("lblPageCode")).Text.Trim());
                bool bRslt = oDAL.DeletePageMaster(oPRP.PageCode);
                if (!bRslt)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPageNotDeletedMsg", "ShowPageNotDeletedMsg();", true);
                }
                GetPageGroupDetails();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get page master details in gridview into edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPageMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                GridViewRow gvRow = (GridViewRow)gvPageMaster.Rows[e.NewEditIndex];
                ViewState["GROUP"] = ((Label)gvRow.FindControl("lblGroup")).Text.Trim();

                gvPageMaster.EditIndex = e.NewEditIndex;
                GetPageGroupDetails();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Update page master details through gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPageMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                GridViewRow gvRow = (GridViewRow)gvPageMaster.Rows[e.RowIndex];
                oPRP.PageCode = int.Parse(((Label)gvRow.FindControl("lblEPageCode")).Text.Trim());
                oPRP.PageName = ((TextBox)gvRow.FindControl("txtEPageName")).Text.Trim();
                oPRP.PageDesc = ((TextBox)gvRow.FindControl("txtEPageDesc")).Text.Trim();
                oPRP.GroupCode = ((DropDownList)gvRow.FindControl("ddlEGroup")).SelectedValue.ToString();
                oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
                oDAL.SaveUpdatePageGroup("UPDATE", oPRP);

                gvPageMaster.EditIndex = -1;
                GetPageGroupDetails();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Page master details cancel edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPageMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvPageMaster.EditIndex = -1;
            GetPageGroupDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Group rights page index changing event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPageMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["PageMaster"];
            gvPageMaster.PageIndex = e.NewPageIndex;
            gvPageMaster.DataSource = dt;
            gvPageMaster.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Catch unhandled excpetions.
    /// </summary>
    /// <param name="ex"></param>
    private void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Page Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    #endregion
}