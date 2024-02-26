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
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class ReportAssetDashBoard : System.Web.UI.Page
{
    RptAssetDashBoard_DAL oDAL;
    RptAssetDashBoard_PRP oPRP;
    public ReportAssetDashBoard()
    {
        oPRP = new RptAssetDashBoard_PRP();
    }
    ~ReportAssetDashBoard()
    {
        oPRP = null;
        oDAL = null;
    }

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
        oDAL = new RptAssetDashBoard_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Get user rights for the page and populate location dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                string _strRights = clsGeneral.GetRights("ASSET_DASHBOARD", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_DASHBOARD");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                pnlDashboard.Visible = false;
                PopulateLocation();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Cathes exception for the entier page operations.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "View Asset Dashboard");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Fetch location details to be populated in dropdownlist.
    /// </summary>
    private void PopulateLocation()
    {
        lblLocLevel.Text = "1";
        lblLocCode.Text = "0";
        ddlAssetLocation.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetLocation("", 1);
        ddlAssetLocation.DataSource = dt;
        ddlAssetLocation.DataTextField = "LOC_NAME";
        ddlAssetLocation.DataValueField = "LOC_CODE";
        ddlAssetLocation.DataBind();
        ddlAssetLocation.Items.Insert(0, "-- Select Location --");
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Reset/refresh location details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnRefreshLocation_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateLocation();
            lblLocationName.Text = "";
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get asset dashboard details for each category.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[0] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (lblLocCode.Text.Trim() == "0")
            {
                if (!chkAllLocations.Checked)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Location is not selected.');", true);
                    ddlAssetLocation.Focus();
                    pnlDashboard.Visible = false;
                    return;
                }
            }
            if (!chkAllLocations.Checked)
            {
                string[] LocParts = { };
                if (lblLocCode.Text != "0")
                    LocParts = lblLocCode.Text.Trim().Split('-');
                if (LocParts[2] == "00")
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1];
                else if (LocParts[3] == "00")
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
                else if (LocParts[4] == "00")
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
                else if (LocParts[5] == "00")
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
                else if (LocParts[5] != "00")
                    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
                else
                    oPRP.AssetLocation = "";
                oPRP.CompanyCode = LocParts[0].ToString();
            }
            else
            {
                oPRP.AssetLocation = "";
                oPRP.CompanyCode = "";
                lblLocationName.Text = "All Locations Selected";
            }

            oPRP.Type = "PRODUCTION";
            DataTable dtProd = oDAL.GetDashBoardDetails(oPRP);
            oPRP.Type = "STOCK";
            DataTable dtStok = oDAL.GetDashBoardDetails(oPRP);
            oPRP.Type = "FAULTY";
            DataTable dtFlty = oDAL.GetDashBoardDetails(oPRP);
            oPRP.Type = "SCRAPPED";
            DataTable dtScrp = oDAL.GetDashBoardDetails(oPRP);
            oPRP.Type = "SOLD";
            DataTable dtSold = oDAL.GetDashBoardDetails(oPRP);

            //Populating Asset Production Count...

            lblDTProd.Text = dtProd.Rows[0]["DT_PROD"].ToString();
            lblLTProd.Text = dtProd.Rows[0]["LT_PROD"].ToString();
            lblMNProd.Text = dtProd.Rows[0]["MN_PROD"].ToString();
            lblMCProd.Text = dtProd.Rows[0]["MC_PROD"].ToString();
            lblSTProd.Text = dtProd.Rows[0]["ST_PROD"].ToString();
            lblSVProd.Text = dtProd.Rows[0]["SV_PROD"].ToString();
            lblTCProd.Text = dtProd.Rows[0]["TC_PROD"].ToString();
            lblIPProd.Text = dtProd.Rows[0]["IP_PROD"].ToString();
            lblBBProd.Text = dtProd.Rows[0]["BB_PROD"].ToString();
            lblPRProd.Text = dtProd.Rows[0]["PR_PROD"].ToString();
            lblPTProd.Text = dtProd.Rows[0]["PT_PROD"].ToString();
            lblPPProd.Text = dtProd.Rows[0]["PP_PROD"].ToString();
            lblSWProd.Text = dtProd.Rows[0]["SW_PROD"].ToString();
            lblRTProd.Text = dtProd.Rows[0]["RT_PROD"].ToString();
            lblFWProd.Text = dtProd.Rows[0]["FW_PROD"].ToString();
            lblBMProd.Text = dtProd.Rows[0]["BM_PROD"].ToString();

            //Populating Asset Stock Count...

            lblDTStk.Text = dtStok.Rows[0]["DT_STK"].ToString();
            lblLTStk.Text = dtStok.Rows[0]["LT_STK"].ToString();
            lblMNStk.Text = dtStok.Rows[0]["MN_STK"].ToString();
            lblMCStk.Text = dtStok.Rows[0]["MC_STK"].ToString();
            lblSTStk.Text = dtStok.Rows[0]["ST_STK"].ToString();
            lblSVStk.Text = dtStok.Rows[0]["SV_STK"].ToString();
            lblTCStk.Text = dtStok.Rows[0]["TC_STK"].ToString();
            lblIPStk.Text = dtStok.Rows[0]["IP_STK"].ToString();
            lblBBStk.Text = dtStok.Rows[0]["BB_STK"].ToString();
            lblPRStk.Text = dtStok.Rows[0]["PR_STK"].ToString();
            lblPTStk.Text = dtStok.Rows[0]["PT_STK"].ToString();
            lblPPStk.Text = dtStok.Rows[0]["PP_STK"].ToString();
            lblSWStk.Text = dtStok.Rows[0]["SW_STK"].ToString();
            lblRTStk.Text = dtStok.Rows[0]["RT_STK"].ToString();
            lblFWStk.Text = dtStok.Rows[0]["FW_STK"].ToString();
            lblBMStk.Text = dtStok.Rows[0]["BM_STK"].ToString();

            //Populating Asset Faulty Count...

            lblDTFlty.Text = dtFlty.Rows[0]["DT_FLT"].ToString();
            lblLTFlty.Text = dtFlty.Rows[0]["LT_FLT"].ToString();
            lblMNFlty.Text = dtFlty.Rows[0]["MN_FLT"].ToString();
            lblMCFlty.Text = dtFlty.Rows[0]["MC_FLT"].ToString();
            lblSTFlty.Text = dtFlty.Rows[0]["ST_FLT"].ToString();
            lblSVFlty.Text = dtFlty.Rows[0]["SV_FLT"].ToString();
            lblTCFlty.Text = dtFlty.Rows[0]["TC_FLT"].ToString();
            lblIPFlty.Text = dtFlty.Rows[0]["IP_FLT"].ToString();
            lblBBFlty.Text = dtFlty.Rows[0]["BB_FLT"].ToString();
            lblPRFlty.Text = dtFlty.Rows[0]["PR_FLT"].ToString();
            lblPTFlty.Text = dtFlty.Rows[0]["PT_FLT"].ToString();
            lblPPFlty.Text = dtFlty.Rows[0]["PP_FLT"].ToString();
            lblSWFlty.Text = dtFlty.Rows[0]["SW_FLT"].ToString();
            lblRTFlty.Text = dtFlty.Rows[0]["RT_FLT"].ToString();
            lblFWFlty.Text = dtFlty.Rows[0]["FW_FLT"].ToString();
            lblBMFlty.Text = dtFlty.Rows[0]["BM_FLT"].ToString();

            //Populating Asset Scrapped Count...

            lblDTScrapped.Text = dtScrp.Rows[0]["DT_SCRP"].ToString();
            lblLTScrapped.Text = dtScrp.Rows[0]["LT_SCRP"].ToString();
            lblMNScrapped.Text = dtScrp.Rows[0]["MN_SCRP"].ToString();
            lblMCScrapped.Text = dtScrp.Rows[0]["MC_SCRP"].ToString();
            lblSTScrapped.Text = dtScrp.Rows[0]["ST_SCRP"].ToString();
            lblSVScrapped.Text = dtScrp.Rows[0]["SV_SCRP"].ToString();
            lblTCScrapped.Text = dtScrp.Rows[0]["TC_SCRP"].ToString();
            lblIPScrapped.Text = dtScrp.Rows[0]["IP_SCRP"].ToString();
            lblBBScrapped.Text = dtScrp.Rows[0]["BB_SCRP"].ToString();
            lblPRScrapped.Text = dtScrp.Rows[0]["PR_SCRP"].ToString();
            lblPTScrapped.Text = dtScrp.Rows[0]["PT_SCRP"].ToString();
            lblPPScrapped.Text = dtScrp.Rows[0]["PP_SCRP"].ToString();
            lblSWScrapped.Text = dtScrp.Rows[0]["SW_SCRP"].ToString();
            lblRTScrapped.Text = dtScrp.Rows[0]["RT_SCRP"].ToString();
            lblFWScrapped.Text = dtScrp.Rows[0]["FW_SCRP"].ToString();
            lblBMScrapped.Text = dtScrp.Rows[0]["BM_SCRP"].ToString();

            //Populating Asset Sold Count...

            lblDTSold.Text = dtSold.Rows[0]["DT_SOLD"].ToString();
            lblLTSold.Text = dtSold.Rows[0]["LT_SOLD"].ToString();
            lblMNSold.Text = dtSold.Rows[0]["MN_SOLD"].ToString();
            lblMCSold.Text = dtSold.Rows[0]["MC_SOLD"].ToString();
            lblSTSold.Text = dtSold.Rows[0]["ST_SOLD"].ToString();
            lblSVSold.Text = dtSold.Rows[0]["SV_SOLD"].ToString();
            lblTCSold.Text = dtSold.Rows[0]["TC_SOLD"].ToString();
            lblIPSold.Text = dtSold.Rows[0]["IP_SOLD"].ToString();
            lblBBSold.Text = dtSold.Rows[0]["BB_SOLD"].ToString();
            lblPRSold.Text = dtSold.Rows[0]["PR_SOLD"].ToString();
            lblPTSold.Text = dtSold.Rows[0]["PT_SOLD"].ToString();
            lblPPSold.Text = dtSold.Rows[0]["PP_SOLD"].ToString();
            lblSWSold.Text = dtSold.Rows[0]["SW_SOLD"].ToString();
            lblRTSold.Text = dtSold.Rows[0]["RT_SOLD"].ToString();
            lblFWSold.Text = dtSold.Rows[0]["FW_SOLD"].ToString();
            lblBMSold.Text = dtSold.Rows[0]["BM_SOLD"].ToString();

            //Populating Asset Total Count Caregory wise...

            lblDTTotal.Text = (int.Parse(lblDTProd.Text.Trim()) + int.Parse(lblDTStk.Text.Trim()) +
                               int.Parse(lblDTFlty.Text.Trim()) + int.Parse(lblDTScrapped.Text.Trim()) +
                               int.Parse(lblDTSold.Text.Trim())).ToString();

            lblLTTotal.Text = (int.Parse(lblLTProd.Text.Trim()) + int.Parse(lblLTStk.Text.Trim()) +
                               int.Parse(lblLTFlty.Text.Trim()) + int.Parse(lblLTScrapped.Text.Trim()) +
                               int.Parse(lblLTSold.Text.Trim())).ToString();

            lblMNTotal.Text = (int.Parse(lblMNProd.Text.Trim()) + int.Parse(lblMNStk.Text.Trim()) +
                               int.Parse(lblMNFlty.Text.Trim()) + int.Parse(lblMNScrapped.Text.Trim()) +
                               int.Parse(lblMNSold.Text.Trim())).ToString();

            lblMCTotal.Text = (int.Parse(lblMCProd.Text.Trim()) + int.Parse(lblMCStk.Text.Trim()) +
                               int.Parse(lblMCFlty.Text.Trim()) + int.Parse(lblMCScrapped.Text.Trim()) +
                               int.Parse(lblMCSold.Text.Trim())).ToString();

            lblSTTotal.Text = (int.Parse(lblSTProd.Text.Trim()) + int.Parse(lblSTStk.Text.Trim()) +
                               int.Parse(lblSTFlty.Text.Trim()) + int.Parse(lblSTScrapped.Text.Trim()) +
                               int.Parse(lblSTSold.Text.Trim())).ToString();

            lblSVTotal.Text = (int.Parse(lblSVProd.Text.Trim()) + int.Parse(lblSVStk.Text.Trim()) +
                               int.Parse(lblSVFlty.Text.Trim()) + int.Parse(lblSVScrapped.Text.Trim()) +
                               int.Parse(lblSVSold.Text.Trim())).ToString();

            lblTCTotal.Text = (int.Parse(lblTCProd.Text.Trim()) + int.Parse(lblTCStk.Text.Trim()) +
                               int.Parse(lblTCFlty.Text.Trim()) + int.Parse(lblTCScrapped.Text.Trim()) +
                               int.Parse(lblTCSold.Text.Trim())).ToString();

            lblIPTotal.Text = (int.Parse(lblIPProd.Text.Trim()) + int.Parse(lblIPStk.Text.Trim()) +
                               int.Parse(lblIPFlty.Text.Trim()) + int.Parse(lblIPScrapped.Text.Trim()) +
                               int.Parse(lblIPSold.Text.Trim())).ToString();

            lblBBTotal.Text = (int.Parse(lblBBProd.Text.Trim()) + int.Parse(lblBBStk.Text.Trim()) +
                               int.Parse(lblBBFlty.Text.Trim()) + int.Parse(lblBBScrapped.Text.Trim()) +
                               int.Parse(lblBBSold.Text.Trim())).ToString();

            lblPRTotal.Text = (int.Parse(lblPRProd.Text.Trim()) + int.Parse(lblPRStk.Text.Trim()) +
                               int.Parse(lblPRFlty.Text.Trim()) + int.Parse(lblPRScrapped.Text.Trim()) +
                               int.Parse(lblPRSold.Text.Trim())).ToString();

            lblPTTotal.Text = (int.Parse(lblPTProd.Text.Trim()) + int.Parse(lblPTStk.Text.Trim()) +
                               int.Parse(lblPTFlty.Text.Trim()) + int.Parse(lblPTScrapped.Text.Trim()) +
                               int.Parse(lblPTSold.Text.Trim())).ToString();

            lblPPTotal.Text = (int.Parse(lblPPProd.Text.Trim()) + int.Parse(lblPPStk.Text.Trim()) +
                               int.Parse(lblPPFlty.Text.Trim()) + int.Parse(lblPPScrapped.Text.Trim()) +
                               int.Parse(lblPPSold.Text.Trim())).ToString();

            lblSWTotal.Text = (int.Parse(lblSWProd.Text.Trim()) + int.Parse(lblSWStk.Text.Trim()) +
                               int.Parse(lblSWFlty.Text.Trim()) + int.Parse(lblSWScrapped.Text.Trim()) +
                               int.Parse(lblSWSold.Text.Trim())).ToString();

            lblRTTotal.Text = (int.Parse(lblRTProd.Text.Trim()) + int.Parse(lblRTStk.Text.Trim()) +
                               int.Parse(lblRTFlty.Text.Trim()) + int.Parse(lblRTScrapped.Text.Trim()) +
                               int.Parse(lblRTSold.Text.Trim())).ToString();

            lblFWTotal.Text = (int.Parse(lblFWProd.Text.Trim()) + int.Parse(lblFWStk.Text.Trim()) +
                               int.Parse(lblFWFlty.Text.Trim()) + int.Parse(lblFWScrapped.Text.Trim()) +
                               int.Parse(lblFWSold.Text.Trim())).ToString();

            lblBMTotal.Text = (int.Parse(lblBMProd.Text.Trim()) + int.Parse(lblBMStk.Text.Trim()) +
                               int.Parse(lblBMFlty.Text.Trim()) + int.Parse(lblBMScrapped.Text.Trim()) +
                               int.Parse(lblBMSold.Text.Trim())).ToString();

            lblTotalAssets.Text = "TOTAL : " + (int.Parse(lblDTTotal.Text.Trim()) + int.Parse(lblLTTotal.Text.Trim()) + int.Parse(lblMNTotal.Text.Trim()) + int.Parse(lblMCTotal.Text.Trim()) +
                                  int.Parse(lblSTTotal.Text.Trim()) + int.Parse(lblSVTotal.Text.Trim()) + int.Parse(lblTCTotal.Text.Trim()) + int.Parse(lblIPTotal.Text.Trim()) +
                                  int.Parse(lblBBTotal.Text.Trim()) + int.Parse(lblPRTotal.Text.Trim()) + int.Parse(lblPTTotal.Text.Trim()) + int.Parse(lblPPTotal.Text.Trim()) +
                                  int.Parse(lblSWTotal.Text.Trim()) + int.Parse(lblRTTotal.Text.Trim()) + int.Parse(lblFWTotal.Text.Trim()) + int.Parse(lblBMTotal.Text.Trim())).ToString();
            pnlDashboard.Visible = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Reset/Clear fields and set location details refreshed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateLocation();
            lblLocationName.Text = "";
            pnlDashboard.Visible = false;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export asset count report from gridview to excel sheet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[4] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            //if (gvAssetSummary.Rows.Count > 0)
            //{
            //    Response.Clear();
            //    DataTable dt = (DataTable)Session["ASSET_SUMMARY"];
            //    DataSet dsExport = new DataSet();
            //    System.IO.StringWriter tw = new System.IO.StringWriter();
            //    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            //    DataGrid dgGrid = new DataGrid();
            //    dgGrid.DataSource = dt;
            //    dgGrid.HeaderStyle.Font.Bold = true;
            //    dgGrid.DataBind();
            //    dgGrid.RenderControl(hw);
            //    Response.ContentType = "application/vnd.ms-excel";
            //    this.EnableViewState = false;
            //    Response.Write(tw.ToString());
            //    Response.End();
            //}
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SELECTED INDEX CHANGED EVENTS
    /// <summary>
    /// Get Sub location details to be populated into dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetLocation.SelectedIndex != 0)
            {
                int locLevel = int.Parse(lblLocLevel.Text.Trim());
                lblLocLevel.Text = (locLevel + 1).ToString();
                int iLocLevel = int.Parse(lblLocLevel.Text.Trim());
                string sLocCode = ddlAssetLocation.SelectedValue.ToString();
                lblLocCode.Text = ddlAssetLocation.SelectedValue.ToString();
                lblLocationName.Text = ddlAssetLocation.SelectedItem.Text.Trim();

                ddlAssetLocation.DataSource = null;
                DataTable dt = oDAL.GetLocation(sLocCode, iLocLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlAssetLocation.DataSource = dt;
                    ddlAssetLocation.DataValueField = "LOC_CODE";
                    ddlAssetLocation.DataTextField = "LOC_NAME";
                    ddlAssetLocation.DataBind();
                    ddlAssetLocation.Items.Insert(0, "-- Select Location --");
                }
                else
                {
                    iLocLevel = iLocLevel - 1;
                    lblLocLevel.Text = iLocLevel.ToString();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}