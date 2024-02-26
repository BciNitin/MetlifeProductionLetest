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

public partial class Error : System.Web.UI.Page
{
    #region PAGE EVENTS
    /// <summary>
    /// Navigates to session expired page in case of user logs off
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
    }

    /// <summary>
    /// Display application level error/exception messages.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                string DeleteError = "The DELETE statement conflicted with the REFERENCE constraint";
                string strError = "";
                bool b = String.IsNullOrEmpty(Convert.ToString(Session["ErrMsg"]).ToString().Trim());
                if (b == false)
                {
                    strError = Convert.ToString(Session["ErrMsg"]).ToString().Trim();
                    //clsGeneral.Message(ref lblMsg, ex.Message.ToString(), 0);
                    if (strError.ToUpper().Contains(DeleteError.ToUpper()))
                    {
                        lblMsg.Text = "Sorry you cannot delete this record, it is used by some other record !!";
                    }
                    else if (strError == "Value cannot be null.")
                    {
                        lblMsg.Text = "The logged in user doesn't belong to any group, please contact your administator.";
                    }
                    else
                    {
                        lblMsg.Text = strError;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
        }
    }
    #endregion
}