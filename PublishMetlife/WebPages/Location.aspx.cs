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
using System.DirectoryServices;

public partial class Home : System.Web.UI.Page
{
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
            Server.Transfer("UserLogin.aspx");
        }
       // lblLoggedUser.Text = Session["CURRENTUSER"].ToString();


        //if (Session["COMP_NAME"] != null)
        //    lblLoggedLocation.Text = Session["COMP_NAME"].ToString();
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
                PopulateCompany();
                //GetPendingCallLogs();
                //GetPendingGatePass();
                //GetExpiredAMCCount();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    private void PopulateCompany()
    {

        Session["DATABASE"] = "ADMIN";
        UserMaster_DAL oDAL = new UserMaster_DAL(Session["DATABASE"].ToString());
      
        ddlCompany.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetCompanyLocation();
        ddlCompany.DataSource = dt;
        ddlCompany.DataTextField = "COMP_NAME";
        ddlCompany.DataValueField = "COMP_CODE";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, "-- Select Company --");
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Get Amc list about to expire.
    /// </summary>
    private void GetExpiredAMCCount()
    {
        AssetAMC_DAL oDAL = new AssetAMC_DAL(Session["DATABASE"].ToString());
        try
        {
        //    int iExpAmcCount = oDAL.GetExpiredAMCCount(30, Session["COMPANY"].ToString());
        //    if (iExpAmcCount > 0)
        //    {
        //        pnlEXAMC.Enabled = true;
        //        lblExpAmcCount.Text = iExpAmcCount.ToString();
        //    }
        //    else
        //        pnlEXAMC.Enabled = false;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
        finally
        { oDAL = null; }
    }

    /// <summary>
    /// Get pending call log list.
    /// </summary>
    private void GetPendingGatePass()
    {
        GatePassGeneration_DAL oDAL = new GatePassGeneration_DAL(Session["DATABASE"].ToString());
        try
        {
            //lblPendingAsset.Text = oDAL.GetPendingGatePass(Session["COMPANY"].ToString());
            //if (lblPendingAsset.Text == "0")
            //    pnlEXGP.Enabled = false;
            //else
            //    pnlEXGP.Enabled = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
        finally
        { oDAL = null; }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetPendingCallLogs()
    {
        CallLog_DAL oDAL = new CallLog_DAL(Session["DATABASE"].ToString());
        try
        {
            //lblPendingCall.Text = oDAL.GetPendingCallLogs(Session["COMPANY"].ToString());
            //if (lblPendingCall.Text == "0")
            //    pnlPCL.Enabled = false;
            //else
            //    pnlPCL.Enabled = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
        finally
        { oDAL = null; }
    }
    
    /// <summary>
    /// Cathes exception for the entier page operations.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Home Page");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Response.Redirect("Error.aspx", false);
        }
    }

    /// <summary>
    /// Active Directory access point.
    /// </summary>
    private void GetDirectoryServicesAccess()
    {
        string EntityCode, LoggedInUserId = "";
        DirectoryEntry DE = new DirectoryEntry();
        DE.Path = "LDAP://FNBCIL.COM";
        DirectorySearcher DS = new DirectorySearcher();
        DS.PropertiesToLoad.Add("departmantNumber");
        DS.Filter = "(&(objectClass=User)(sAMAccountName=" + LoggedInUserId.Substring(LoggedInUserId.LastIndexOf(':') + 1) + "))";

        SearchResult result = DS.FindOne();
        if (result != null && result.Properties["departmantNumber"] != null && result.Properties["departmantNumber"].Count > 0)
        {
            EntityCode = result.Properties["departmantNumber"][0].ToString().Substring(result.Properties["departmantNumber"][0].ToString().Length - 4);
        }
    }
    #endregion

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedIndex > 0)
        {
            Session["COMPANY"] = ddlCompany.SelectedValue.ToString();
            Session["COMP_NAME"] = ddlCompany.SelectedItem.Text.Trim();
            Response.Redirect("~/Webpages/Home.aspx", false);
        }
    }
}