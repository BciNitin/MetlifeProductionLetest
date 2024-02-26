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

public partial class LogOut : System.Web.UI.Page
{
    #region PAGE EVENTS
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["CURRENTUSER"] = null;
        Session.Clear();
        Session.Abandon();
        
        Response.CacheControl = "no-cache";
        Response.Cache.SetExpires(System.DateTime.UtcNow.AddMinutes(-1));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();

        clsGeneral.gStrAssetType = string.Empty;
        clsGeneral.gStrSessionID = string.Empty;

        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
        Session.Abandon();
        Response.Redirect("UserLogin.aspx", false);
    }
    #endregion
}