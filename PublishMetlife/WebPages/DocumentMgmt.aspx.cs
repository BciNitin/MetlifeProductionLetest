using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using MyDLL;

public partial class DocumentMgmt : System.Web.UI.Page
{
    string ServerFilePath = "";
    DocumentMgmt_DAL oDAL;
    DocumentMgmt_PRP oPRP;
    public DocumentMgmt()
    {
        oPRP = new DocumentMgmt_PRP();
    }
    ~DocumentMgmt()
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
        oDAL = new DocumentMgmt_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user rights for add/view/delate files operation.
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
                string _strRights = clsGeneral.GetRights("DOCUMENT_MANAGEMENT", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "DOCUMENT_MANAGEMENT");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                if (!Directory.Exists(Request.PhysicalApplicationPath + @"\FILES"))
                    Directory.CreateDirectory(Request.PhysicalApplicationPath + @"\FILES");
                GetFileDetails();
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
        clsGeneral.LogErrorToLogFile(ex, "Asset Acquisition");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Save uploded file on server's physical application location.
    /// </summary>
    /// <param name="FileName"></param>
    private bool SaveFileOnServer(string FileName)
    {
        bool bRes = false;
        if (FileName != "")
        {
            ServerFilePath = Request.PhysicalApplicationPath + @"FILES\" + FileName;
            if (File.Exists(Request.PhysicalApplicationPath + @"FILES\" + FileName))
                File.Delete(ServerFilePath);
            FileUpload.SaveAs(ServerFilePath);
            bRes = true;
        }
        return bRes;
    }

    /// <summary>
    /// Get uploaded files details to be viewed in gridview.
    /// </summary>
    private void GetFileDetails()
    {
        gvViewFiles.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetFileDetails(Session["COMPANY"].ToString());
        if (dt.Rows.Count > 0)
        {
            gvViewFiles.DataSource = dt;
            gvViewFiles.DataBind();
            lblRecordCount.Text = "Files Record Count : " + dt.Rows.Count.ToString();
        }
        else
        {
            gvViewFiles.DataSource = null;
            gvViewFiles.DataBind();
            lblRecordCount.Text = "";
        }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Save uploaded file on server and create an entry in the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            oPRP.Description = txtDescription.Text.Trim().Replace("'", "`");
            oPRP.Category = txtCategory.Text.Trim().Replace("'", "`");
            oPRP.Remarks = txtRemarks.Text.Trim().Replace("'", "`");
            oPRP.AttachFileName = FileUpload.FileName;            
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompanyName = Session["COMP_NAME"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            bool bResp = oDAL.SaveUpdateFileDetails("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate File Name cannot be saved.');", true);
                txtCategory.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                SaveFileOnServer(oPRP.AttachFileName);
                upSubmit.Update();
            }
            GetFileDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Delete Document management details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            bool bResp = false;
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvViewFiles.Rows[e.RowIndex];
            oPRP.SerialNo = int.Parse(((Label)gvRow.FindControl("lblSrlNo")).Text.Trim());
            oPRP.CompCode = ((Label)gvRow.FindControl("lblCompCode")).Text.Trim();
            oPRP.AttachFileName = ((Label)gvRow.FindControl("lblFileName")).Text.Trim();
            if (oPRP.CompCode == Session["COMPANY"].ToString())
            {
                if (File.Exists(Request.PhysicalApplicationPath + @"FILES\" + oPRP.AttachFileName))
                    File.Delete(Request.PhysicalApplicationPath + @"FILES\" + oPRP.AttachFileName);
                bResp = oDAL.DeleteFileDetails(oPRP.SerialNo, oPRP.CompCode);
                if (bResp)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : File details deleted successfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : File uploaded by another location cannot be deleted.');", true);
                return;
            }            
            GetFileDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// View File From Document management details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewFiles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[0] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (e.CommandName == "View")
            {
                string FileName = e.CommandArgument.ToString();
                Response.Clear();
                Response.ContentType = @"application\octet-stream";
                string RootPath = Server.MapPath("~/FILES");
                FileInfo file = new FileInfo(RootPath + "\\" + FileName);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.Flush();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Edit Mode Document management details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewFiles_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvViewFiles.Rows[e.NewEditIndex];
            oPRP.CompCode = ((Label)gvRow.FindControl("lblCompCode")).Text.Trim();
            if (oPRP.CompCode == Session["COMPANY"].ToString())
            {
                gvViewFiles.EditIndex = e.NewEditIndex;
                GetFileDetails();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : File uploaded by another location cannot be updated.');", true);
                return;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Enable/disable edit/delete options as per logged in user rights.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewFiles_RowDataBound(object sender, GridViewRowEventArgs e)
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
    /// Update Document management details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvViewFiles.Rows[e.RowIndex];
            oPRP.SerialNo = int.Parse(((Label)gvRow.FindControl("lblEditSNo")).Text.Trim());
            oPRP.Description = ((TextBox)gvRow.FindControl("txtEditDesc")).Text.Trim().Replace("'", "`");
            oPRP.Category = ((TextBox)gvRow.FindControl("txtEditCategory")).Text.Trim().Replace("'", "`");
            oPRP.Remarks = ((TextBox)gvRow.FindControl("txtEditRemarks")).Text.Trim().Replace("'", "`");
            oPRP.CompCode = Session["COMPANY"].ToString();
            bool bResp = oDAL.SaveUpdateFileDetails("UPDATE", oPRP);
            if (bResp)
            {
                gvViewFiles.EditIndex = -1;
                GetFileDetails();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg",
                    "ShowErrMsg('Please Note : File details updated successfully.');", true);
                return;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Cancel file details edit mode in gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewFiles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvViewFiles.EditIndex = -1;
            GetFileDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// View assets gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["VIEW_FILES"];
            gvViewFiles.PageIndex = e.NewPageIndex;
            gvViewFiles.DataSource = dt;
            gvViewFiles.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}