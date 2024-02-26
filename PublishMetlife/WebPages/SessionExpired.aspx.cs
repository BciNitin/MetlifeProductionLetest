using System;
using System.Web;

public partial class SessionExpired : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
    }
}