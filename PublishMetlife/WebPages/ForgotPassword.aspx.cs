using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net.Mail;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
  
public partial class ForgotPassword : System.Web.UI.Page
{
    UserMaster_DAL oDAL;
    UserMaster_PRP oPRP;
    public ForgotPassword()
    {
        oPRP = new UserMaster_PRP();
    }
    ~ForgotPassword()
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
        if (Session["DATABASE"] == null || Session["COMPANY"] == null)
        {
            Server.Transfer("UserLogin.aspx");
        }
        oDAL = new UserMaster_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Page Load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Nothing to do here...
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SUBMIT EVENT
    /// <summary>
    /// Send forgot password to the user through a registered e-mail address.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            oPRP.UserID = txtUserID.Text.Trim();
            oPRP.UserEmail = txtEmailId.Text.Trim();
            oPRP.CompCode = Session["COMPANY"].ToString();
            string _UserPswd = oDAL.GetUserPassword(oPRP.UserID, oPRP.CompCode, oPRP.UserEmail);
            if (_UserPswd == "NOT_FOUND")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowNotFoundMsg", "ShowNotFoundMsg();", true);
            }
            else
            {
                try
                { SendForgotPassword(DecryptPassword(_UserPswd)); }
                catch (Exception ex)
                { }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SendSuccessMsg", "SendSuccessMsg();", true);
                Session["DATABASE"] = null;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTION
    /// <summary>
    /// Decrypt password in order to send password to the user through e-mail.
    /// </summary>
    /// <param name="EncryptedPswd"></param>
    /// <returns></returns>
    public static string DecryptPassword(string EncryptedPswd)
    {
        byte[] encodedDataAsBytes = System.Convert.FromBase64String(EncryptedPswd);
        string DecryptPswd = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
        return DecryptPswd;
    }

    /// <summary>
    /// Send approval mail message for new asset created.
    /// </summary>
    /// <param name="_AssetCode"></param>
    private void SendForgotPassword(string Password)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SENDER"].ToString(), "ATS");
        smtpClient.Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
        message.From = fromAddress;
        message.To.Add(txtEmailId.Text.Trim());
        message.Subject = "BCIL : ATS - Forgot Password";
        message.IsBodyHtml = false;
        StringBuilder sbMsg = new StringBuilder();
        sbMsg.AppendLine("Please Note,");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Your BCIL ATS Application Login Password is : " + Password + ".");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("http://10.164.91.191/FIS_ATS/WEBPAGES/UserLogin.aspx");
        message.Body = sbMsg.ToString();
        smtpClient.Send(message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Forgot Password");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    #endregion
}