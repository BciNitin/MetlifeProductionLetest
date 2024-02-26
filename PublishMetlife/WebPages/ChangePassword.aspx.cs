using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class ChangePassword : System.Web.UI.Page
{
    UserMaster_DAL oDAL;
    UserMaster_PRP oPRP;
    public ChangePassword()
    {
        oPRP = new UserMaster_PRP();
    }
    ~ChangePassword()
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
        oDAL = new UserMaster_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking user group rights for change password operation.
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
                string _strRights = clsGeneral.GetRights("CHANGE_PASSWORD", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "CHANGE_PASSWORD");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                txtUserID.Text = Session["CURRENTUSER"].ToString();
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
        clsGeneral.LogErrorToLogFile(ex, "Change User Password");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Encrypt user password in order to check login credentials.
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    private string EncryptPassword(string password)
    {
        string EncryptedPswd = string.Empty;
        byte[] encode = new byte[password.Length];
        encode = Encoding.UTF8.GetBytes(password);
        EncryptedPswd = Convert.ToBase64String(encode);
        return EncryptedPswd;
    }
    #endregion

    #region SUBMIT EVENT
    /// <summary>
    /// Save changed password for logged in user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            oPRP.UserID = txtUserID.Text.Trim();
            oPRP.UserPswd = EncryptPassword(txtOldPswd.Text.Trim());
            oPRP.NewPswd = EncryptPassword(txtNewPswd.Text.Trim());
            oPRP.CompCode = Session["COMPANY"].ToString();
            bool bValid = oDAL.ValidateUserLogin(oPRP);
            if (!bValid)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg();", true);
                txtOldPswd.Focus();
                return;
            }
            else
            {
                bool bSave = oDAL.ChangePassword(oPRP);
                if (bSave)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSavedSuccessMsg", "ShowSavedSuccessMsg();", true);
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}