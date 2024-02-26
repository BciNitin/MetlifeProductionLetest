using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class MobiVUEMaster : System.Web.UI.MasterPage
{
    #region PAGE EVENTS
    /// <summary>
    /// Redirects to session expired page if user session is expired.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string uri = HttpContext.Current.Request.Url.AbsoluteUri;
            if(!uri.Contains("StoreMovement") && !uri.Contains("KioskMovement"))
            {
                if (Session["CURRENTUSER"] == null || Convert.ToString(Session["CURRENTUSER"]).Trim() == "")
                {
                    Response.Redirect("SessionExpired.aspx");
                }                
            }
            else
            {
                menu.Visible = false;
                //topnav.Visible = false;
            }

            //else if (Session["COMPANY"] == null || Convert.ToString(Session["COMPANY"]).Trim() == "" || Convert.ToString(Session["COMPANY"]).Trim().Contains("ALL"))
            //{
            //    Response.Redirect("Location.aspx");
            //}
            //if (Session["ALL"] != null && Convert.ToString(Session["ALL"]) != "")
            //    liall.Visible = true;
        }
        catch(Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Master page load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //To Do Nothing.
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Catch unhandled exceptions and show the error message on the error page.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    #endregion
}