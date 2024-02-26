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

public partial class WebPages_CreateInitialData : System.Web.UI.Page
{
    InitialData_DAL oDAL;
    InitialData_PRP oPRP;
    public WebPages_CreateInitialData()
    {
        oPRP = new InitialData_PRP();
    }
    ~WebPages_CreateInitialData()
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
        oDAL = new InitialData_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Creating initial master data for the very first time.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                string _strRights = clsGeneral.GetRights("CREATE_INITIAL_DETAILS", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "CREATE_INITIAL_DETAILS");
                if (Session["GROUP"].ToString() != "SYSADMIN")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Encrypt user password into UTF8 characters.
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

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Create initial data");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    #endregion

    #region SUBMIT EVENTS
    /// <summary>
    /// Save initial details for a new location very first time by System Administrator.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["GROUP"].ToString() != "SYSADMIN")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            oPRP.CompCode = txtCompanyCode.Text.Trim().ToUpper();
            oPRP.CompName = txtCompanyName.Text.Trim();
            oPRP.LocationCode = txtLocationCode.Text.Trim().ToUpper();
            oPRP.LocationName = txtLocationName.Text.Trim();
            oPRP.GroupCode = txtGroupCode.Text.Trim().ToUpper();
            oPRP.GroupName = txtGroupName.Text.Trim();
            oPRP.UserID = txtUserID.Text.Trim();
            oPRP.UserName = txtUserName.Text.Trim();
            oPRP.Password = EncryptPassword(txtPassword.Text.Trim());
            oPRP.AdminEmail = txtAdminEmail.Text.Trim();
            oPRP.TechopsEmail = txtTechopsEmail.Text.Trim();
            oPRP.SuperUser = Session["CURRENTUSER"].ToString();

            bool bResp = oDAL.InsertInitialData(oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Initial details are not saved.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Initial details are saved successfully.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}