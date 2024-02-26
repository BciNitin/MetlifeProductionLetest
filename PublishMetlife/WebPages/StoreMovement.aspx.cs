using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.Data;
using System.Configuration;
using System.Timers;

public partial class WebPages_StoreMovement : System.Web.UI.Page
{
    string _CompCode = "";
    StoreMovemementInOut_DAL oDAL;
    StoreMovementInOut_PRP oPRP;
    bool bTransOpen = false;
    public WebPages_StoreMovement()
    {
        oDAL = new StoreMovemementInOut_DAL("ADMIN");
        oPRP = new StoreMovementInOut_PRP();
    }
    ~WebPages_StoreMovement()
    {
        oDAL = null; oPRP = null;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string[] url = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { "q=" }, StringSplitOptions.None);
                if (!url[0].Contains("StoreMovement"))
                {
                    if (Session["CURRENTUSER"] == null)
                    {
                        Server.Transfer("SessionExpired.aspx");
                    }
                }
                
                string[] compCode = url[1].Split(new[] { "~" }, StringSplitOptions.None);
                _CompCode = compCode[1].Replace("%20"," ");
                Session["COMPANY"] = lblComp.Text = compCode[1].Replace("%20", " ").Trim();
                Session["STOREVALUE"] = lblStore.Text = compCode[0].Replace("%20", " ").Trim();
                lblRefreshTime.Text = ConfigurationManager.AppSettings["RefreshTime"].Trim();
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                RefreshGrid();
                StoreTimer.Interval= Convert.ToInt32(ConfigurationManager.AppSettings["RefreshTime"])*1000;
            }
        }
        catch (Exception ex)
        {
            clsGeneral.LogErrorToLogFile(ex, "Page Load Store Movement");
        }
    }
    protected void imagebuttonDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string bResult = string.Empty;
            ImageButton imgbtn = (ImageButton)sender;
            string status = string.Empty;
            int id = 0;
            string AssetTag = string.Empty;
            GridViewRow gvRow = (GridViewRow)imgbtn.NamingContainer;
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                status = ((Label)gvRow.FindControl("lblStatus")).Text.ToString().Trim();
                id = Convert.ToInt32(((Label)gvRow.FindControl("lblId")).Text.ToString().Trim());
                AssetTag = ((Label)gvRow.FindControl("lblAssetTag")).Text.ToString().Trim();
                bResult = oDAL.RemoveAssetTag(id, AssetTag, status);
                if (bResult.Contains("SUCCESS"))
                {
                    lblErrorMSg.Text = "Asset is Successfully Deleted";
                    RefreshGrid();
                }
                else
                {
                    lblErrorMSg.Text = "Asset is Not Deleted";
                    RefreshGrid();
                }
            }
        }
        catch (Exception ex)
        {
            clsGeneral.LogErrorToLogFile(ex, "On Delete Store Movement");
        }

    }


    protected void StoreTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            RefreshGrid();
        }
        catch (Exception ex) 
        {
            clsGeneral.LogErrorToLogFile(ex, "Timer Store Movement");
        }
    }


    public void RefreshGrid()
    {
        try
        {
            lblMsg.Text = "";
            lblMsg.Text = "Timer Refreshed at : " + DateTime.Now;
            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.DataSource = oDAL.GetStoreMovementDetails(lblComp.Text.Trim(), "IN&OUT", string.Empty, string.Empty, lblStore.Text.Trim(), Convert.ToInt32(lblRefreshTime.Text.Trim()));
            GridView2.DataBind();
            if (Session["COMPANY"].ToString() == "IT")
                GridView2.Columns[11].Visible = false;
            else
                GridView2.Columns[10].Visible = false;
            GridView2.DataBind();
        }
        catch (Exception ex)
        {
            clsGeneral.LogErrorToLogFile(ex, "Load Data Store Movement");
        }
       
    }

}