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

public partial class CompanyMaster : System.Web.UI.Page
{
    CompanyMaster_DAL oDAL;
    CompanyMaster_PRP oPRP;
    public CompanyMaster()
    {
        oPRP = new CompanyMaster_PRP();
    }
    ~CompanyMaster()
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
        oDAL = new CompanyMaster_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking user group rights for company master details 
    /// save/edit/update/delete operations.
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
                string _strRights = clsGeneral.GetRights("COMPANY_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "COMPANY_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                GetCompanyDetails();
                txtCompCode.Focus();
            }
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
        clsGeneral.LogErrorToLogFile(ex, "Company Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Populate company details into gridview for viewing.
    /// </summary>
    private void GetCompanyDetails()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetComp();
        gvCompMaster.DataSource = dt;
        gvCompMaster.DataBind();
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Company details deleting event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCompMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvCompMaster.Rows[e.RowIndex];
            oPRP.CompCode = ((Label)gvRow.FindControl("lblCompCode")).Text.Trim();
            bool bResp = oDAL.DeleteComp(oPRP.CompCode);
            if (bResp)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Entire details of the company deleted successfully.');", true);
            GetCompanyDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Company details editing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCompMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            gvCompMaster.EditIndex = e.NewEditIndex;
            GetCompanyDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCompMaster_RowDataBound(object sender, GridViewRowEventArgs e)
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
    /// Company details updating event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCompMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvCompMaster.Rows[e.RowIndex];
            oPRP.CompCode = ((Label)gvRow.FindControl("lblEditCompCode")).Text.Trim();
            oPRP.CompName = ((TextBox)gvRow.FindControl("txtEditCompName")).Text.Trim();
            oPRP.Remarks = ((TextBox)gvRow.FindControl("txtERemarks")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oDAL.SaveUpdateComp("UPDATE", oPRP);

            gvCompMaster.EditIndex = -1;
            GetCompanyDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Company details edit/update cancelling event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCompMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvCompMaster.EditIndex = -1;
            GetCompanyDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Company master gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCompMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvCompMaster.PageIndex = e.NewPageIndex;
            GetCompanyDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SUBMIT EVENT
    /// <summary>
    /// Save new company details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            oPRP.CompCode = txtCompCode.Text.Trim();
            oPRP.CompName = txtCompName.Text.Trim();
            oPRP.Remarks = txtRemarks.Text.Trim();
            oPRP.Active = chkSetStatus.Checked;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            bool bResp = oDAL.SaveUpdateComp("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg();", true);
                txtCompCode.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            GetCompanyDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}