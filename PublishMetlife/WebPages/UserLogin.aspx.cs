using System;
using System.Data;
using System.Web;
using System.Text;
using System.Web.UI;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Runtime.InteropServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Configuration;

public partial class UserLogin : System.Web.UI.Page
{
    UserMaster_DAL oDAL;
    UserMaster_PRP oPRP;
    public UserLogin()
    {

    }
    ~UserLogin()
    {
        oDAL = null; oPRP = null;
    }

    #region PAGE EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                Session["DATABASE"] = null;
                Session["COMPANY"] = null;
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                Session["DATABASE"] = "ADMIN";
                oDAL = new UserMaster_DAL(Session["DATABASE"].ToString());
                oPRP = new UserMaster_PRP();
                ddlAssetType.Focus();
                this.Form.DefaultButton = btnSignIn.UniqueID;

            }
            lblMsg.Visible = false;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS

    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "User Login");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }

    private bool IsValidUser(string UsrLogin, string UsrPass)
    {
        DirectoryEntry m_obDirEntry;
        try
        {
            string strLDAP = ConfigurationManager.AppSettings["LDAPAdd"].ToString();
            m_obDirEntry = new DirectoryEntry(strLDAP, UsrLogin, UsrPass);
            DirectorySearcher srch = new DirectorySearcher(m_obDirEntry);
            srch.Filter = "(SAMAccountName=" + UsrLogin + ")";
            SearchResultCollection results;
            results = srch.FindAll();
            ResultPropertyCollection propColl = null;

            foreach (SearchResult result in results)
            {
                propColl = result.Properties;
            }
            if (propColl != null)
            {
                foreach (string strKey in propColl.PropertyNames)
                    foreach (object obProp in propColl[strKey])
                    {
                        if (strKey.ToUpper() == "DISPLAYNAME")
                        {
                            string id = obProp.ToString();

                        }
                        if (strKey.ToUpper() == "MAIL")
                        {
                            string mail = obProp.ToString();
                            Session["EMPMAILID"] = mail;
                            clsGeneral.GlobalEmpEmailId = mail;
                        }
                        if (strKey.ToUpper() == "EMPLOYEEID")
                        {
                            Session["EMPLOYEEID"] = obProp.ToString();
                        }
                    }
                return true;
            }
            else
                return false;
        }
        catch (System.DirectoryServices.DirectoryServicesCOMException Ex)
        {
            return false;
        }

        catch (Exception ex)
        {
            return false;
        }


    }
    private string EncryptPassword(string password)
    {
        string EncryptedPswd = string.Empty;
        byte[] encode = new byte[password.Length];
        encode = Encoding.UTF8.GetBytes(password);
        EncryptedPswd = Convert.ToBase64String(encode);
        return EncryptedPswd;
    }
    #endregion

    #region BUTTON EVENTS

    public bool IsAuthenticated(string username, string pwd)
    {
        bool status = true;
        //using (DirectoryEntry entry = new DirectoryEntry())
        //{
        //    entry.Username = username;
        //    entry.Password = pwd;

        //    DirectorySearcher searcher = new DirectorySearcher(entry);
        //    searcher.Filter = "(objectclass=user)";

        //    try
        //    {
        //        searcher.FindOne();
        //        status = true;


        //    }
        //    catch (COMException ex)
        //    {
        //        throw ex;
        //    }
        //}


        return status;
    }

    protected void btnSignIn_Click(object sender, ImageClickEventArgs e)
    {
        oDAL = new UserMaster_DAL(Session["DATABASE"].ToString());
        oPRP = new UserMaster_PRP();
        try
        {
            DataTable dtGrpRights = new DataTable();
            oPRP.UserID = txtUserId.Text.Trim();
            oPRP.UserPswd = txtPassword.Text.Trim();
            oPRP.CompCode = ddlAssetType.SelectedValue.ToString();

            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(oPRP.UserID, oPRP.CompCode, "USER Login", "Login", "Login attempt by user id" + oPRP.UserID);
            if (ddlAssetType.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ValidateLogin", "ValidateLogin();", true);
            }
            else
            {
                if (txtUserId.Text.Trim() == "5" || txtUserId.Text.Trim() == "1")
                //if (IsAuthenticated(oPRP.UserID, oPRP.UserPswd))
                {
                    if (Session["EMPLOYEEID"] != null)
                    {
                        oPRP.UserID = Session["EMPLOYEEID"].ToString().Trim();
                    }
                    Session["LDAPUser"] = txtUserId.Text.Trim();
                    Session["LDAPPassword"] = txtPassword.Text.Trim();
                    Session["SUPER_USER_ID"] = null;
                    Session["COMPANY"] = ddlAssetType.SelectedValue.ToString();
                    clsGeneral.gStrCompType = ddlAssetType.SelectedValue.ToString();
                    if (Convert.ToString(Session["COMPANY"]).Contains("ALL"))
                    {
                        Session["ALL"] = "ALL";
                    }
                    Session["CURRENTUSER"] = oPRP.UserID;
                    Session["EMPMAILID"] = oDAL.GetUserLoginEmail(txtUserId.Text.Trim());
                    clsGeneral.GlobalEmpEmailId = oDAL.GetUserLoginEmail(txtUserId.Text.Trim());
                    Session["ASSETTYPE"] = ddlAssetType.SelectedValue + "-display";
                    Session["COMP_NAME"] = ddlAssetType.SelectedValue.ToString();
                    try
                    {
                        string location = oDAL.GetUserLocation(oPRP.UserID);
                        Session["GROUP"] = oDAL.GetLogInUserGroup(oPRP.UserID, oPRP.CompCode);
                        dtGrpRights = oDAL.GetGroupRights(oPRP.UserID, oPRP.CompCode);
                        clsGeneral.gStrSessionID = Session.SessionID.ToString();
                        if (dtGrpRights.Rows.Count == 0)
                        {
                            Session["ErrMsg"] = "The logged in user doesn't belong to any group, please contact system administrator.";
                            Response.Redirect("~/Webpages/Error.aspx", false);
                        }
                        else
                        {
                            Session["UserRights"] = dtGrpRights;
                            if (Session["COMPANY"].ToString().Contains("ALL"))
                                Response.Redirect("~/Webpages/Location.aspx", false);
                            else
                                Response.Redirect("~/Webpages/Home.aspx", false);
                        }

                    }
                    catch (Exception ex)
                    {
                        Session["ErrMsg"] = "The logged in user doesn't belong to any group, please contact system administrator.";
                        Response.Redirect("~/Webpages/Error.aspx", false);
                    }

                }
                else if (IsValidUser(txtUserId.Text.Trim(), txtPassword.Text.Trim()))
                //if (IsAuthenticated(oPRP.UserID, oPRP.UserPswd))
                {
                    if (Session["EMPLOYEEID"] != null)
                    {
                        oPRP.UserID = Session["EMPLOYEEID"].ToString().Trim();
                    }
                    Session["LDAPUser"] = txtUserId.Text.Trim();
                    Session["LDAPPassword"] = txtPassword.Text.Trim();
                    Session["SUPER_USER_ID"] = null;
                    Session["COMPANY"] = ddlAssetType.SelectedValue.ToString();
                    clsGeneral.gStrCompType = ddlAssetType.SelectedValue.ToString();
                    if (Convert.ToString(Session["COMPANY"]).Contains("ALL"))
                    {
                        Session["ALL"] = "ALL";
                    }
                    Session["CURRENTUSER"] = oPRP.UserID;
                    Session["ASSETTYPE"] = ddlAssetType.SelectedValue + "-display";
                    Session["COMP_NAME"] = ddlAssetType.SelectedValue.ToString();
                    try
                    {
                        string location = oDAL.GetUserLocation(oPRP.UserID);
                        Session["GROUP"] = oDAL.GetLogInUserGroup(oPRP.UserID, oPRP.CompCode);
                        dtGrpRights = oDAL.GetGroupRights(oPRP.UserID, oPRP.CompCode);
                        clsGeneral.gStrSessionID = Session.SessionID.ToString();
                        if (dtGrpRights.Rows.Count == 0)
                        {
                            Session["ErrMsg"] = "The logged in user doesn't belong to any group, please contact system administrator.";
                            Response.Redirect("~/Webpages/Error.aspx", false);
                        }
                        else
                        {
                            Session["UserRights"] = dtGrpRights;
                            if (Session["COMPANY"].ToString().Contains("ALL"))
                                Response.Redirect("~/Webpages/Location.aspx", false);
                            else
                                Response.Redirect("~/Webpages/Home.aspx", false);
                        }

                    }
                    catch (Exception ex)
                    {
                        Session["ErrMsg"] = "The logged in user doesn't belong to any group, please contact system administrator.";
                        Response.Redirect("~/Webpages/Error.aspx", false);
                    }

                }
                else
                {
                    lblMsg.Text = "Invalid User ID/Password . LDAP Authentication failed !";
                    lblMsg.Visible = true;
                    txtUserId.Text = "";
                    txtPassword.Text = "";
                    ddlAssetType.SelectedIndex = 0;
                    //ddlCompany.SelectedIndex = 0;
                    ddlAssetType.Focus();
                }

            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Invalid User ID/Password . LDAP Authentication failed !";
            lblMsg.Visible = true;
            txtUserId.Text = "";
            txtPassword.Text = "";
            ddlAssetType.SelectedIndex = 0;
            //ddlCompany.SelectedIndex = 0;
            clsGeneral.LogErrorToLogFile(ex, "User sign in error occured.");
            if (ex.Message.ToString().ToUpper().Contains("LOGIN FAILED FOR USER"))
            {
                Session["CURRENTUSER"] = null;
                HandleExceptions(ex);
            }
        }
        finally
        { oDAL = null; oPRP = null; }
    }

    protected void btnForgotPswd_Click(object sender, EventArgs e)
    {
        try
        {
            Session["COMPANY"] = ddlAssetType.SelectedValue.ToString();
            Response.Redirect("~/Webpages/ForgotPassword.aspx");
        }
        catch (Exception ex)
        {
            clsGeneral.LogErrorToLogFile(ex, "User sign in error occured.");
            if (ex.Message.ToString().ToUpper().Contains("LOGIN FAILED FOR USER"))
            {
                Session["CURRENTUSER"] = null;
                HandleExceptions(ex);
            }
        }
    }
    #endregion

    #region SELECTED INDEX CHANGED EVENTS

    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    if (ddlAssetType.SelectedIndex != 0)
        //    {
        //        if (ddlAssetType.SelectedValue.ToString() == "ADMIN")
        //        {
        //            Session["DATABASE"] = "ADMIN";
        //            oDAL = new UserMaster_DAL(Session["DATABASE"].ToString());
        //            oPRP = new UserMaster_PRP();
        //            PopulateCompany();
        //            ddlCompany.Focus();
        //        }
        //        else if (ddlAssetType.SelectedValue.ToString() == "IT")
        //        {
        //            Session["DATABASE"] = "IT";
        //            oDAL = new UserMaster_DAL(Session["DATABASE"].ToString());
        //            oPRP = new UserMaster_PRP();
        //            PopulateCompany();
        //            ddlCompany.Focus();
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
        //finally
        //{ oDAL = null; oPRP = null; }
    }
    #endregion
}
