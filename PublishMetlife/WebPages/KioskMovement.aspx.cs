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

public partial class WebPages_KioskMovement : System.Web.UI.Page
{
    string _CompCode = "";
    StoreMovemementInOut_DAL oDAL;
    StoreMovementInOut_PRP oPRP;
    bool bTransOpen = false;
    public WebPages_KioskMovement()
    {
        oDAL = new StoreMovemementInOut_DAL("ADMIN");
        oPRP = new StoreMovementInOut_PRP();
    }
    ~WebPages_KioskMovement()
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
                if (!url[0].Contains("KioskMovement"))
                {
                    if (Session["CURRENTUSER"] == null)
                    {
                        Server.Transfer("SessionExpired.aspx");
                    }
                }            
                string[] compCode = url[1].Split(new[] { "~" }, StringSplitOptions.None);
                _CompCode = compCode[1].Replace("%20", " "); 
                Session["COMPANY"] = lblKioskComp.Text = compCode[1].Replace("%20", " ").Trim(); 
                Session["KIOSKVALUE"] = lblKiosk.Text = compCode[0].Replace("%20", " ").Trim();
                lblKioskRefreshTime.Text = ConfigurationManager.AppSettings["KioskRefreshTime"].Trim();                              
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                KioskRefreshGrid();
                KioskTimer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["KioskRefreshTime"]) * 1000;
            }
        }
        catch (Exception ex)
        {
            clsGeneral.LogErrorToLogFile(ex, "Page Load Kiosk Movement");
        }
    }
    private void KioskRefreshGrid()
    {
        try
        {
            lblMsg.Text = "Timer Refreshed at : " + DateTime.Now;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            DataTable dt = oDAL.GetKioskMovementDetails(lblKioskComp.Text.Trim(), "Kiosk", lblKiosk.Text.Trim(), Convert.ToInt32(lblKioskRefreshTime.Text.Trim()));
            if (dt.Rows.Count > 0)
            {
                lblSerialNo.Text = dt.Rows[0]["SERIAL_CODE"].ToString() == null ? "" : dt.Rows[0]["SERIAL_CODE"].ToString();
                lblRFIDTag.Text = dt.Rows[0]["TAG_ID"].ToString() == null ? "" : dt.Rows[0]["TAG_ID"].ToString();
                lblLocation.Text = dt.Rows[0]["Location"].ToString() == null ? "" : dt.Rows[0]["Location"].ToString();
                lblEmployeeTag.Text = dt.Rows[0]["EmployeeTag"].ToString() == null ? "" : dt.Rows[0]["EmployeeTag"].ToString();
                lblAssetType.Text = dt.Rows[0]["ASSET_TYPE"].ToString() == null ? "" : dt.Rows[0]["ASSET_TYPE"].ToString();
                lblAssetSubStatus.Text = dt.Rows[0]["ASSET_SUB_STATUS"].ToString() == null ? "" : dt.Rows[0]["ASSET_SUB_STATUS"].ToString();
                lblAssetStatus.Text = dt.Rows[0]["ASSET_STATUS"].ToString() == null ? "" : dt.Rows[0]["ASSET_STATUS"].ToString();
                lblAssetModel.Text = dt.Rows[0]["MODEL_NAME"].ToString() == null ? "" : dt.Rows[0]["MODEL_NAME"].ToString();
                lblAssetMake.Text = dt.Rows[0]["ASSET_MAKE"].ToString() == null ? "" : dt.Rows[0]["ASSET_MAKE"].ToString();
                lblEmpName.Text = dt.Rows[0]["EMPLOYEE_NAME"].ToString() == null ? "" : dt.Rows[0]["EMPLOYEE_NAME"].ToString();
                lblScanStatus.Text = dt.Rows[0]["ScanStatus"].ToString() == null ? "" : dt.Rows[0]["ScanStatus"].ToString();
                if (lblScanStatus.Text.ToUpper() == "INVALID")
                {
                    lblScanStatus.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblScanStatus.ForeColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                ClearAll();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Data Not Found.');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            clsGeneral.LogErrorToLogFile(ex, "Data grid Load Kiosk Movement");
        }       
    }
    protected void KioskTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            lblMsg.Text = "Timer Refreshed at : " + DateTime.Now;
            KioskRefreshGrid();
        }
        catch (Exception ex)
        {
            clsGeneral.LogErrorToLogFile(ex, " Kiosk Timer Load Kiosk Movement");
        }
    }

    private void ClearAll()
    {
        lblSerialNo.Text = "";
        lblRFIDTag.Text = "";
        lblLocation.Text = "";
        lblEmployeeTag.Text = "";
        lblAssetType.Text = "";
        lblAssetSubStatus.Text = "";
        lblAssetStatus.Text = "";
        lblAssetModel.Text = "";
        lblAssetMake.Text = "";
        lblEmpName.Text = "";
        lblScanStatus.Text = "";
    }
}