using System;

public partial class UnauthorizedUser : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
    }
}