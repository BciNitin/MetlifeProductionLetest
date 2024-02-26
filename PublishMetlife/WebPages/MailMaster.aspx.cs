using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPages_MailMaster : System.Web.UI.Page
{

    #region PAGE CONSTRUCTOR & DECLARATIONS

    MailMaster_DAL oDAL;
    MailMaster_PRP oPRP;
    public WebPages_MailMaster()
    {
        oPRP = new MailMaster_PRP();
    }
    ~WebPages_MailMaster()
    {
        oPRP = null; oDAL = null;
    }

    #endregion

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
        oDAL = new MailMaster_DAL(Session["DATABASE"].ToString());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("MAIL_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "MAIL_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
            }
            LoadGrid();
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
        clsGeneral.LogErrorToLogFile(ex, "Mail Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }

    public void LoadGrid()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetMailTransactionDetails(Convert.ToString(Session["COMP_NAME"]));
        gvMailMaster.DataSource = dt;
        gvMailMaster.DataBind();
    }

    #endregion

    #region CONTROL EVENTS

    protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTransactionType.SelectedIndex > 0) 
        {
           DataTable dt = oDAL.GetMailTransactionDetails(ddlTransactionType.SelectedValue, Convert.ToString(Session["COMP_NAME"]));
            if (dt.Rows.Count > 0)
            {
                txtToMailID.Text = dt.Rows[0].Field<string>("TO_MAIL_ID");
                txtCCMailID.Text = dt.Rows[0].Field<string>("CC_MAIL_ID");
                txtMailSubject.Text = dt.Rows[0].Field<string>("MAIL_SUBJECT");
                txtMailBody.Text = dt.Rows[0].Field<string>("MAIL_BODY");
                txtRemarks.Text= dt.Rows[0].Field<string>("REMARKS");
                btnSubmit.Text = "Update";
            }
            else 
            {
                txtToMailID.Text = null;
                txtCCMailID.Text = null;
                txtMailSubject.Text = null;
                txtMailBody.Text = null;
                txtRemarks.Text = null;
                btnSubmit.Text = "Save";
            }
        }
    }

    #endregion

    #region BUTTON EVENTS

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            oPRP = new MailMaster_PRP();
            oPRP.TransactionType = ddlTransactionType.SelectedValue;
            oPRP.ToMailAddress = txtToMailID.Text;
            oPRP.CCMailAddress = txtCCMailID.Text;
            oPRP.MailSubject = txtMailSubject.Text;
            oPRP.MailBody = txtMailBody.Text;
            oPRP.Remarks = txtRemarks.Text;
            oPRP.CompCode = Convert.ToString(Session["COMP_NAME"]);
            oPRP.UserID = Convert.ToString(Session["CURRENTUSER"]);
            string message = oDAL.SaveMailMaster(oPRP);
            if (message == "SUCCESS") {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            else {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : '" + message + ");", true);
                ddlTransactionType.Focus();
            }
        }
        catch (Exception ex) { HandleExceptions(ex); }
    }

    #endregion


}
