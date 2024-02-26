using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.IO;

public partial class SwapAsset : System.Web.UI.Page
{
    AssetSwapping_DAL oDAL;
    AssetSwapping_PRP oPRP;
    public SwapAsset()
    {
        oPRP = new AssetSwapping_PRP();
    }
    ~SwapAsset()
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
        oDAL = new AssetSwapping_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking user group rights for add new asset allocation operation.
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
                string _strRights = clsGeneral.GetRights("SWAPPING_ASSET", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "SWAPPING_ASSET");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                if (Session["GROUP"].ToString().ToUpper() == "SYSADMIN")
                {
                    txtSwapDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                    txtSwapDate.Enabled = true;
                }
                else
                {
                    txtSwapDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                    txtSwapDate.Enabled = false;
                }
                PopulateLocation1();
                PopulateLocation2();
                GetAssetSwapHistory();
            }
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
        clsGeneral.LogErrorToLogFile(ex, "Swappings Assets");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Populate assets which are to be swapped.
    /// </summary>
    private void GetAssetDetails1()
    {
        gvAssetSwap1.DataSource = null;
        oPRP.AssetCode = txtAssetCode1.Text.Trim();
        oPRP.PortNo = txtPortNo1.Text.Trim();
        oPRP.SerialCode = txtSerialCode1.Text.Trim();
        oPRP.AllocatedAssets = ChkAllocatedAssets1.Checked;
        oPRP.CompCode = Session["COMPANY"].ToString();

        string[] LocParts = { };
        if (lblLocCode1.Text != "0")
        {
            LocParts = lblLocCode1.Text.Trim().Split('-');
            if (LocParts[2] == "00")
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1];
            else if (LocParts[3] == "00")
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
            else if (LocParts[4] == "00")
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
            else if (LocParts[5] == "00")
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
            else
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
        }
        else
            oPRP.LocationCode = "";
        
        DataTable dt = new DataTable();
        dt = oDAL.GetAssetsForSwapping(oPRP);
        if (dt.Rows.Count > 0)
        {
            gvAssetSwap1.DataSource = dt;
            gvAssetSwap1.DataBind();
            lblAssetCount1.Text = "Assets Count : " + dt.Rows.Count.ToString();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset not found for the given criteria.');", true);
            gvAssetSwap1.DataSource = dt;
            gvAssetSwap1.DataBind();
            lblAssetCount1.Text = "Assets Count : 0";
            return;
        }
    }

    /// <summary>
    /// Populate assets which are to be swapped.
    /// </summary>
    private void GetAssetDetails2()
    {
        gvAssetSwap2.DataSource = null;
        oPRP.AssetCode = txtAssetCode2.Text.Trim();
        oPRP.PortNo = txtPortNo2.Text.Trim();
        oPRP.SerialCode = txtSerialCode2.Text.Trim();
        oPRP.AllocatedAssets = ChkAllocatedAssets2.Checked;
        oPRP.CompCode = Session["COMPANY"].ToString();

        string[] LocParts = { };
        if (lblLocCode2.Text != "0")
        {
            LocParts = lblLocCode2.Text.Trim().Split('-');
            if (LocParts[2] == "00")
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1];
            else if (LocParts[3] == "00")
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
            else if (LocParts[4] == "00")
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
            else if (LocParts[5] == "00")
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
            else
                oPRP.LocationCode = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
        }
        else
            oPRP.LocationCode = "";
        
        DataTable dt = new DataTable();
        dt = oDAL.GetAssetsForSwapping(oPRP);
        if (dt.Rows.Count > 0)
        {
            gvAssetSwap2.DataSource = dt;
            gvAssetSwap2.DataBind();
            lblAssetCount2.Text = "Assets Count : " + dt.Rows.Count.ToString();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset not found for the given criteria.');", true);
            gvAssetSwap2.DataSource = dt;
            gvAssetSwap2.DataBind();
            lblAssetCount2.Text = "Assets Count : 0";
            return;
        }
    }

    /// <summary>
    /// Fetces Location for asset replacement.
    /// </summary>
    private void PopulateLocation1()
    {
        lblLocLevel1.Text = "1";
        ddlAssetLocation1.DataSource = null;
        ddlAssetLocation1.Items.Clear();
        DataTable dt = new DataTable();
        dt = oDAL.GetLocation(Session["COMPANY"].ToString(),"", 1);
        ddlAssetLocation1.DataSource = dt;
        ddlAssetLocation1.DataValueField = "LOC_CODE";
        ddlAssetLocation1.DataTextField = "LOC_NAME";
        ddlAssetLocation1.DataBind();
        ddlAssetLocation1.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// Fetces Location for asset replacement.
    /// </summary>
    private void PopulateLocation2()
    {
        lblLocLevel2.Text = "1";
        ddlAssetLocation2.DataSource = null;
        ddlAssetLocation2.Items.Clear();
        DataTable dt = new DataTable();
        dt = oDAL.GetLocation(Session["COMPANY"].ToString(),"", 1);
        ddlAssetLocation2.DataSource = dt;
        ddlAssetLocation2.DataValueField = "LOC_CODE";
        ddlAssetLocation2.DataTextField = "LOC_NAME";
        ddlAssetLocation2.DataBind();
        ddlAssetLocation2.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// Get asset swap history details for being viewed in gridview.
    /// </summary>
    private void GetAssetSwapHistory()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetAssetSwapHistory(Session["COMPANY"].ToString());
        if (dt.Rows.Count > 0)
        {
            gvSwapHistory.DataSource = Session["SWAP_HISTORY"] = dt;
            gvSwapHistory.DataBind();
        }
    }

    /// <summary>
    /// Send mail for asset approval for assets being swapped.
    /// </summary>
    /// <param name="AssetCode1"></param>
    /// <param name="AssetCode2"></param>
    /// <param name="LocationCode1"></param>
    /// <param name="LocationCode2"></param>
    /// <param name="ProcessCode1"></param>
    /// <param name="ProcessCode2"></param>
    /// <param name="PortNo1"></param>
    /// <param name="PortNo2"></param>
    private void SendMailForApproval(string AssetCode1, string AssetCode2, string LocationCode1, string LocationCode2, string ProcessCode1, string ProcessCode2, string PortNo1, string PortNo2, string LoginUser)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SENDER"].ToString(), "ATS");
        smtpClient.Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
        message.From = fromAddress;
        message.To.Add(Session["EMAIL"].ToString());
        message.Subject = "BCIL : ATS - Approval For Asset Swapped";
        message.IsBodyHtml = false;
        StringBuilder sbMsg = new StringBuilder();
        sbMsg.AppendLine("Please Note,");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("An asset with asset code as : " + AssetCode1 + " has been swapped with asset code as : " + AssetCode2 + " by " + LoginUser + ",");
        sbMsg.AppendLine("location : " + LocationCode1 + "  swapped with location : " + LocationCode2 + ",");
        sbMsg.AppendLine("process : " + ProcessCode1 + "  swapped with process : " + ProcessCode2 + ",");
        sbMsg.AppendLine("port no. : " + PortNo1 + "  swapped with port no. : " + PortNo2 + ",");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Kindly approve through the link given below.");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("http://10.164.91.191/BCIL_ATS/WebPages/UserLogin.aspx");
        message.Body = sbMsg.ToString();
        smtpClient.Send(message);
    }

    /// <summary>
    /// Send mail for asset approval for allocated assets being swapped.
    /// </summary>
    /// <param name="AssetCode1"></param>
    /// <param name="AssetCode2"></param>
    /// <param name="LocationCode1"></param>
    /// <param name="LocationCode2"></param>
    /// <param name="EmpName1"></param>
    /// <param name="EmpCode1"></param>
    /// <param name="EmpName2"></param>
    /// <param name="EmpCode2"></param>
    /// <param name="ProcessCode1"></param>
    /// <param name="ProcessCode2"></param>
    /// <param name="PortNo1"></param>
    /// <param name="PortNo2"></param>
    private void SendMailForApproval(string AssetCode1, string AssetCode2, string LocationCode1, string LocationCode2, string EmpName1, string EmpCode1, string EmpName2, string EmpCode2, string ProcessCode1, string ProcessCode2, string PortNo1, string PortNo2, string LoginUser)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SENDER"].ToString(), "ATS");
        smtpClient.Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
        message.From = fromAddress;
        message.To.Add(Session["EMAIL"].ToString());
        message.Subject = "BCIL : ATS - Approval For Asset Swapped";
        message.IsBodyHtml = false;
        StringBuilder sbMsg = new StringBuilder();
        sbMsg.AppendLine("Please Note,");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("An asset with asset code as : " + AssetCode1 + " has been swapped with asset code as : " + AssetCode2 + " by " + LoginUser + ",");
        sbMsg.AppendLine("location : " + LocationCode1 + "  swapped with location : " + LocationCode2 + ",");
        sbMsg.AppendLine("allocated employee : " + EmpName1 + "  swapped with employee : " + EmpName2 + ",");
        sbMsg.AppendLine("allocated employee code : " + EmpCode1 + "  swapped with employee code : " + EmpCode2 + ",");
        sbMsg.AppendLine("process : " + ProcessCode1 + "  swapped with process : " + ProcessCode2 + ",");
        sbMsg.AppendLine("port no. : " + PortNo1 + "  swapped with port no. : " + PortNo2 + ",");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("http://10.164.91.191/FIS_ATS/WebPages/UserLogin.aspx");
        message.Body = sbMsg.ToString();
        smtpClient.Send(message);
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGo1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (txtAssetCode1.Text.Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = oDAL.GetAssetDetails(txtAssetCode1.Text.Trim(), Session["COMPANY"].ToString());
                if (dt.Rows.Count > 0)
                {
                    txtSerialCode1.Text = dt.Rows[0]["SERIAL_CODE"].ToString();
                    txtPortNo1.Text = dt.Rows[0]["PORT_NO"].ToString();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGo2_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (txtAssetCode2.Text.Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = oDAL.GetAssetDetails(txtAssetCode2.Text.Trim(), Session["COMPANY"].ToString());
                if (dt.Rows.Count > 0)
                {
                    txtSerialCode2.Text = dt.Rows[0]["SERIAL_CODE"].ToString();
                    txtPortNo2.Text = dt.Rows[0]["PORT_NO"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Search assets based on asset code, serial code and asset allocated filters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch1_Click(object sender, EventArgs e)
    {
        try
        {
            GetAssetDetails1();       
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Search assets based on asset code, serial code and asset allocated filters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch2_Click(object sender, EventArgs e)
    {
        try
        {
            GetAssetDetails2();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Save/Update asset swapping details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            oPRP.CompCode = Session["COMPANY"].ToString();
            bool bAssetSelected1 = false;
            bool bAssetSelected2 = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (txtSwapDate.Text.Trim() != "")
            {
                int iDate = clsGeneral.CompareDate(txtSwapDate.Text.Trim(), DateTime.Now.ToString("dd/MMM/yyyy"));
                if (iDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Swapping Date cannot be later than current date!');", true);
                    txtSwapDate.Focus();
                    return;
                }
            }
            oPRP.SwapDate = txtSwapDate.Text.Trim();
            for (int iCnt = 0; iCnt < gvAssetSwap1.Rows.Count; iCnt++)
            {
                if (((CheckBox)gvAssetSwap1.Rows[iCnt].FindControl("chkSelectAsset1")).Checked == true)
                {
                    bAssetSelected1 = true;
                    break;
                }
            }
            if (!bAssetSelected1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Click on search assets button and select asset for swapping.');", true);
                return;
            }
            for (int iCnt = 0; iCnt < gvAssetSwap2.Rows.Count; iCnt++)
            {
                if (((CheckBox)gvAssetSwap2.Rows[iCnt].FindControl("chkSelectAsset2")).Checked == true)
                {
                    bAssetSelected2 = true;
                    break;
                }
            }
            if (!bAssetSelected2)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Click on search assets button and select asset for swapping.');", true);
                return;
            }
            if ((ChkAllocatedAssets1.Checked && !ChkAllocatedAssets2.Checked) || (!ChkAllocatedAssets1.Checked && ChkAllocatedAssets2.Checked))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Either both assets should be allocated or both not allocated.');", true);
                return;
            }

            //Swapping not allocated assets...
            if (!ChkAllocatedAssets1.Checked && !ChkAllocatedAssets2.Checked)
            {
                foreach (GridViewRow gvRow in gvAssetSwap1.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("chkSelectAsset1")).Checked)
                    {
                        oPRP.AssetCode1 = ((Label)gvRow.FindControl("lblAsset1")).Text.Trim();
                        oPRP.AssetID1 = ((Label)gvRow.FindControl("lblAssetID1")).Text.Trim();
                        oPRP.SerialCode1 = ((Label)gvRow.FindControl("lblSerial1")).Text.Trim();
                        oPRP.LocationCode1 = ((Label)gvRow.FindControl("lblLoc1")).Text.Trim();
                        oPRP.ProcessCode1 = ((Label)gvRow.FindControl("lblAllProc1")).Text.Trim();
                        oPRP.WorkStation1 = ((Label)gvRow.FindControl("lblWS1")).Text.Trim();
                        oPRP.PortNo1 = ((Label)gvRow.FindControl("lblPN1")).Text.Trim();
                    }
                }
                foreach (GridViewRow gvRow in gvAssetSwap2.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("chkSelectAsset2")).Checked)
                    {
                        oPRP.AssetCode2 = ((Label)gvRow.FindControl("lblAsset2")).Text.Trim();
                        oPRP.AssetID2 = ((Label)gvRow.FindControl("lblAssetID2")).Text.Trim();
                        oPRP.SerialCode2 = ((Label)gvRow.FindControl("lblSerial2")).Text.Trim();
                        oPRP.LocationCode2 = ((Label)gvRow.FindControl("lblLoc2")).Text.Trim();
                        oPRP.ProcessCode2 = ((Label)gvRow.FindControl("lblAllProc2")).Text.Trim();
                        oPRP.WorkStation2 = ((Label)gvRow.FindControl("lblWS2")).Text.Trim();
                        oPRP.PortNo2 = ((Label)gvRow.FindControl("lblPN2")).Text.Trim();
                    }
                }
                if ((oPRP.AssetCode1 == oPRP.AssetCode2) || (oPRP.SerialCode1 == oPRP.SerialCode2))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Assets selected for swapping are same.');", true);
                    return;
                }
                oPRP.SwappingRemarks = txtRemarks.Text.Trim().Replace("'", "`");
                bool bResp = oDAL.SwapNotAllocatedAssets(oPRP);
                bool bHistory = oDAL.SaveAssetSwapHistory(oPRP);
                if (bResp && bHistory)
                {
                    try
                    { SendMailForApproval(oPRP.AssetCode1, oPRP.AssetCode2, oPRP.LocationCode1, oPRP.LocationCode2, oPRP.ProcessCode1, oPRP.ProcessCode2, oPRP.PortNo1, oPRP.PortNo2, Session["CURRENTUSER"].ToString()); }
                    catch (Exception ex) { }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Assets are swapped successfully. Swapped assets need to be approved.');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            }

            //Swapping allocated assets...
            if (ChkAllocatedAssets1.Checked && ChkAllocatedAssets2.Checked)
            {
                foreach (GridViewRow gvRow in gvAssetSwap1.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("chkSelectAsset1")).Checked)
                    {
                        oPRP.AssetCode1 = ((Label)gvRow.FindControl("lblAsset1")).Text.Trim();
                        oPRP.AssetID1 = ((Label)gvRow.FindControl("lblAssetID1")).Text.Trim();
                        oPRP.SerialCode1 = ((Label)gvRow.FindControl("lblSerial1")).Text.Trim();
                        oPRP.LocationCode1 = ((Label)gvRow.FindControl("lblLoc1")).Text.Trim();
                        oPRP.EmpName1 = ((Label)gvRow.FindControl("lblEmp1")).Text.Trim();
                        oPRP.EmpCode1 = ((Label)gvRow.FindControl("lblEmpCode1")).Text.Trim();
                        oPRP.ProcessCode1 = ((Label)gvRow.FindControl("lblAllProc1")).Text.Trim();
                        oPRP.WorkStation1 = ((Label)gvRow.FindControl("lblWS1")).Text.Trim();
                        oPRP.PortNo1 = ((Label)gvRow.FindControl("lblPN1")).Text.Trim();
                    }
                }
                foreach (GridViewRow gvRow in gvAssetSwap2.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("chkSelectAsset2")).Checked)
                    {
                        oPRP.AssetCode2 = ((Label)gvRow.FindControl("lblAsset2")).Text.Trim();
                        oPRP.AssetID2 = ((Label)gvRow.FindControl("lblAssetID2")).Text.Trim();
                        oPRP.SerialCode2 = ((Label)gvRow.FindControl("lblSerial2")).Text.Trim();
                        oPRP.LocationCode2 = ((Label)gvRow.FindControl("lblLoc2")).Text.Trim();
                        oPRP.EmpName2 = ((Label)gvRow.FindControl("lblEmp2")).Text.Trim();
                        oPRP.EmpCode2 = ((Label)gvRow.FindControl("lblEmpCode2")).Text.Trim();
                        oPRP.ProcessCode2 = ((Label)gvRow.FindControl("lblAllProc2")).Text.Trim();
                        oPRP.WorkStation2 = ((Label)gvRow.FindControl("lblWS2")).Text.Trim();
                        oPRP.PortNo2 = ((Label)gvRow.FindControl("lblPN2")).Text.Trim();
                    }
                }
                if ((oPRP.AssetCode1 == oPRP.AssetCode2) || (oPRP.SerialCode1 == oPRP.SerialCode2))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Assets selected for swapping are same.');", true);
                    return;
                }
                oPRP.SwappingRemarks = txtRemarks.Text.Trim().Replace("'", "`");
                bool bResp = oDAL.SwapAllocatedAssets(oPRP);
                bool bHistory = oDAL.SaveAssetSwapHistory(oPRP);
                if (bResp && bHistory)
                {
                    try
                    { SendMailForApproval(oPRP.AssetCode1, oPRP.AssetCode2, oPRP.LocationCode1, oPRP.LocationCode2, oPRP.EmpName1, oPRP.EmpCode1, oPRP.EmpName2, oPRP.EmpCode2, oPRP.ProcessCode1, oPRP.ProcessCode2, oPRP.PortNo1, oPRP.PortNo2, Session["CURRENTUSER"].ToString()); }
                    catch (Exception ex) { }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Assets are swapped successfully. Swapped assets need to be approved.');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            }
            GetAssetDetails1();
            GetAssetDetails2();
            GetAssetSwapHistory();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Referesh/reset location.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshLocation1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblLocCode1.Text = "0";
            lblLocLevel1.Text = "1";
            PopulateLocation1();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Referesh/reset location.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshLocation2_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblLocCode2.Text = "0";
            lblLocLevel2.Text = "1";
            PopulateLocation2();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Reset/Clear fields and gets assets gridviews refreshed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            gvAssetSwap1.DataSource = null;
            gvAssetSwap1.DataBind();
            gvAssetSwap2.DataSource = null;
            gvAssetSwap2.DataBind();

            lblAssetCount1.Text = "Assets Count : 0";
            lblAssetCount2.Text = "Assets Count : 0";
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
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
            if (gvSwapHistory.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["SWAP_HISTORY"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=SwapAsset.xls");
                //this.EnableViewState = false;
                //Response.Write(tw.ToString());
                //Response.End();
                using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    wb.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=REPORT" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetSwap1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssetSwap1.PageIndex = e.NewPageIndex;
            GetAssetDetails1();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetSwap2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssetSwap2.PageIndex = e.NewPageIndex;
            GetAssetDetails2();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvSwapHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSwapHistory.PageIndex = e.NewPageIndex;
            GetAssetSwapHistory();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SELECTED INDEX CHANGED EVENTS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetLocation1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetLocation1.SelectedIndex > 0)
            {
                int locLevel = int.Parse(lblLocLevel1.Text.Trim());
                lblLocLevel1.Text = (locLevel + 1).ToString();
                int iLocLevel = int.Parse(lblLocLevel1.Text.Trim());
                string sLocCode = ddlAssetLocation1.SelectedValue.ToString();
                lblLocCode1.Text = sLocCode;

                ddlAssetLocation1.DataSource = null;
                DataTable dt = oDAL.GetLocation(Session["COMPANY"].ToString(), sLocCode, iLocLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlAssetLocation1.DataSource = dt;
                    ddlAssetLocation1.DataValueField = "LOC_CODE";
                    ddlAssetLocation1.DataTextField = "LOC_NAME";
                    ddlAssetLocation1.DataBind();
                    ddlAssetLocation1.Items.Insert(0, "-- Select Location --");
                }
                else
                {
                    iLocLevel = iLocLevel - 1;
                    lblLocLevel1.Text = iLocLevel.ToString();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetLocation2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetLocation2.SelectedIndex > 0)
            {
                int locLevel = int.Parse(lblLocLevel2.Text.Trim());
                lblLocLevel2.Text = (locLevel + 1).ToString();
                int iLocLevel = int.Parse(lblLocLevel2.Text.Trim());
                string sLocCode = ddlAssetLocation2.SelectedValue.ToString();
                lblLocCode2.Text = sLocCode;

                ddlAssetLocation2.DataSource = null;
                DataTable dt = oDAL.GetLocation(Session["COMPANY"].ToString(), sLocCode, iLocLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlAssetLocation2.DataSource = dt;
                    ddlAssetLocation2.DataValueField = "LOC_CODE";
                    ddlAssetLocation2.DataTextField = "LOC_NAME";
                    ddlAssetLocation2.DataBind();
                    ddlAssetLocation2.Items.Insert(0, "-- Select Location --");
                }
                else
                {
                    iLocLevel = iLocLevel - 1;
                    lblLocLevel2.Text = iLocLevel.ToString();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}