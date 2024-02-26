using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using System.Web.UI.WebControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.IO;

public partial class AssetTransfer : System.Web.UI.Page
{
    int iAssetCount = 0;
    string TransferedAssets = "";
    AssetTransfer_DAL oDAL;
    AssetTransfer_PRP oPRP;
    public AssetTransfer()
    {
        oPRP = new AssetTransfer_PRP();
    }
    ~AssetTransfer()
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
        oDAL = new AssetTransfer_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// 
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
                string _strRights = clsGeneral.GetRights("ASSET_TRANSFER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_TRANSFER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                if (Session["GROUP"].ToString().ToUpper() == "SYSADMIN")
                {
                    txtTransferDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                    txtTransferDate.Enabled = true;
                }
                else
                {
                    txtTransferDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                    txtTransferDate.Enabled = false;
                }
                ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
                lblAssetType.Text = clsGeneral.gStrAssetType;
                PopulateCategory(lblAssetType.Text.Trim());
                PopulateProcess();
                PopulateToProcess();
                PopulateFromLocation();
                PopulateToLocation();
                PopulateLocation();
                PopulateInterLocation();
                GetAssetTransferDetails();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Catch unhandled exceptions and show on the error page.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Asset Transfer");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Populate country-wide/inter-location trasferred assets details for being viewed.
    /// </summary>
    private void GetAssetTransferDetails()
    {
        gvAssetTransfer.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetAssetTransferDetails(Session["COMPANY"].ToString());
        if (dt.Rows.Count > 0)
        {
            gvAssetTransfer.DataSource = Session["TRANSFER"] = dt;
            gvAssetTransfer.DataBind();
            btnExport.Visible = true;
        }
        else
        {
            gvAssetTransfer.DataSource = null;
            gvAssetTransfer.DataBind();
            btnExport.Visible = false;
        }
    }

    /// <summary>
    /// Populate location code/name for tranferred assets export.
    /// </summary>
    private void PopulateLocation()
    {
        lblLocCode.Text = "";
        lblLocLevel.Text = "1";
        ddlLocation.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetToLocation(Session["COMPANY"].ToString(), "", 1);
        ddlLocation.DataSource = dt;
        ddlLocation.DataTextField = "LOC_NAME";
        ddlLocation.DataValueField = "LOC_CODE";
        ddlLocation.DataBind();
        ddlLocation.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// Populate location code/name for inter location transfer.
    /// </summary>
    private void PopulateInterLocation()
    {
        lblInterLocCode.Text = "0";
        lblInterLocLevel.Text = "1";
        ddlInterToLocation.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetLocation(Session["COMPANY"].ToString(), "", 1);
        ddlInterToLocation.DataSource = dt;
        ddlInterToLocation.DataTextField = "LOC_NAME";
        ddlInterToLocation.DataValueField = "LOC_CODE";
        ddlInterToLocation.DataBind();
        ddlInterToLocation.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// Populate from location details to be populated in dropdownlist.
    /// </summary>
    private void PopulateFromLocation()
    {
        //lblFromLocCode.Text = "";
        //lblLocLevel1.Text = "1";
        //ddlFromLocation.DataSource = null;
        //DataTable dt = new DataTable();
        //dt = oDAL.GetFromLocation(Session["COMPANY"].ToString(), "", 1);
        //ddlFromLocation.DataSource = dt;
        //ddlFromLocation.DataTextField = "LOC_NAME";
        //ddlFromLocation.DataValueField = "LOC_CODE";
        //ddlFromLocation.DataBind();
        //ddlFromLocation.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// Populate to location details to be populated in dropdownlist.
    /// </summary>
    private void PopulateToLocation()
    {
        lblToLocCode.Text = "0";
        lblLocLevel2.Text = "1";
        ddlToLocation.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetToLocation(Session["COMPANY"].ToString(), "", 1);
        ddlToLocation.DataSource = dt;
        ddlToLocation.DataTextField = "LOC_NAME";
        ddlToLocation.DataValueField = "LOCATION_CODE";
        ddlToLocation.DataBind();
        ddlToLocation.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// Populate department code/name into department dropdownlist.
    /// </summary>
    private void PopulateToProcess()
    {
        ddlInterToProcess.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetProcess(Session["COMPANY"].ToString());
        ddlInterToProcess.DataSource = dt;
        ddlInterToProcess.DataValueField = "PROCESS_CODE";
        ddlInterToProcess.DataTextField = "PROCESS_NAME";
        ddlInterToProcess.DataBind();
        ddlInterToProcess.Items.Insert(0, "-- Select Process --");
    }

    /// <summary>
    /// Populate asset model names based on asset make and category name selected.
    /// </summary>
    /// <param name="AssetMake"></param>
    private void PopulateModelName(string AssetMake, string CategoryCode)
    {
        lstModelName.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateModelName(AssetMake, CategoryCode, Session["COMPANY"].ToString());
        lstModelName.DataSource = dt;
        lstModelName.DataTextField = "MODEL_NAME";
        lstModelName.DataValueField = "MODEL_NAME";
        lstModelName.DataBind();
        lstModelName.Items.Insert(0, "-- Select Model --");
    }

    /// <summary>
    /// Populate asset make based on category name selected.
    /// </summary>
    private void PopulateAssetMake(string CategoryCode)
    {
        ddlAssetMake.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateAssetMake(CategoryCode, Session["COMPANY"].ToString());
        ddlAssetMake.DataSource = dt;
        ddlAssetMake.DataTextField = "ASSET_MAKE";
        ddlAssetMake.DataValueField = "ASSET_MAKE";
        ddlAssetMake.DataBind();
        ddlAssetMake.Items.Insert(0, "-- Select Make --");
    }

    /// <summary>
    /// Populate inter location process code/name.
    /// </summary>
    private void PopulateProcess()
    {
        ddlProcessName.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateProcess(Session["COMPANY"].ToString());
        ddlProcessName.DataSource = dt;
        ddlProcessName.DataTextField = "PROCESS_NAME";
        ddlProcessName.DataValueField = "PROCESS_CODE";
        ddlProcessName.DataBind();
        ddlProcessName.Items.Insert(0, "-- Select Process --");
    }

    /// <summary>
    /// Fetch category details to be populated in dropdownlist.
    /// </summary>
    private void PopulateCategory(string AssetType)
    {
        lblCatCode.Text = "0";
        lblCatLevel.Text = "1";
        ddlAssetCategory.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateCategory(AssetType.Trim(), "", 1);
        ddlAssetCategory.DataSource = dt;
        ddlAssetCategory.DataTextField = "CATEGORY_NAME";
        ddlAssetCategory.DataValueField = "CATEGORY_CODE";
        ddlAssetCategory.DataBind();
        ddlAssetCategory.Items.Insert(0, "-- Select Category --");
    }

    /// <summary>
    /// Getting list of assets for being transferred.
    /// </summary>
    private void GetAssetsForTransfer()
    {
        oPRP.CompCode = Session["COMPANY"].ToString();
        oPRP.AssetCode = txtAssetCode.Text.Trim();
        oPRP.AssetSerialCode = txtSerialNo.Text.Trim();
        if (ddlAssetMake.SelectedIndex != 0)
            oPRP.AssetMakeName = ddlAssetMake.SelectedValue.ToString();
        else
            oPRP.AssetMakeName = "";
        for (int iCnt = 0; iCnt < lstModelName.Items.Count; iCnt++)
        {
            if (lstModelName.Items[iCnt].Selected)
                oPRP.AssetModelName += lstModelName.Items[iCnt].Value.ToString() + ",";
        }
        if (oPRP.AssetModelName != null)
        {
            oPRP.AssetModelName = oPRP.AssetModelName.TrimEnd(',');
            oPRP.AssetModelName = oPRP.AssetModelName.Replace(",", "','");
            oPRP.AssetModelName = "'" + oPRP.AssetModelName + "'";
        }
        else
            oPRP.AssetModelName = "";

        if (ddlAssetType.SelectedIndex != 0)
        {
            if (ddlAssetType.SelectedValue.ToString() == "AD")
                oPRP.AssetType = "ADMIN";
            if (ddlAssetType.SelectedValue.ToString() == "IT")
                oPRP.AssetType = "IT";
        }
        if (ddlAssetCategory.SelectedIndex != 0)
            oPRP.AssetCategoryCode = ddlAssetCategory.SelectedValue.ToString();
        else
            oPRP.AssetCategoryCode = "";
        if (ddlProcessName.SelectedIndex != 0)
            oPRP.AssetProcess = ddlProcessName.SelectedValue.ToString();
        else
            oPRP.AssetProcess = "";
        oPRP.PortNo = txtPortNo.Text.Trim();

        DataTable dt = new DataTable();
        dt = oDAL.GetAssetsForTransfer(oPRP);
        if (dt.Rows.Count > 0)
        {
            lblAssetCount.Text = "Assets Count : " + dt.Rows.Count.ToString();
            gvAssets.DataSource = Session["ASSETS"] = dt;
            gvAssets.DataBind();
        }
        else
        {
            lblAssetCount.Text = "Assets Count : 0";
            gvAssets.DataSource = null;
            gvAssets.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no record found for selected criterian.');", true);
            return;
        }
    }

    /// <summary>
    /// Send mail for approval for asset being transferred from within same location.
    /// </summary>
    /// <param name="AssetCode"></param>
    /// <param name="FromInterLocation"></param>
    /// <param name="ToInterLocation"></param>
    /// <param name="AssetProcess"></param>
    /// <param name="ToInterProcessCode"></param>
    /// <param name="FromWorkStation"></param>
    /// <param name="ToWorkStation"></param>
    /// <param name="FromPort"></param>
    /// <param name="ToPort"></param>
    private void SendMailForApproval(string AssetCode, string FromInterLocation, string ToInterLocation, string AssetProcess, string ToInterProcessCode, 
                                     string FromWorkStation, string ToWorkStation, string FromPort, string ToPort, string LoginUser)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SENDER"].ToString(), "ATS");
        smtpClient.Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
        message.From = fromAddress;
        message.To.Add(Session["EMAIL"].ToString());
        message.Subject = "BCIL : ATS - Approval For Inter Location Asset Transfer";
        message.IsBodyHtml = false;
        StringBuilder sbMsg = new StringBuilder();
        sbMsg.AppendLine("Please Note,");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Assets have been transferred (inter-location) with asset code as : " + AssetCode + " by '" + LoginUser + "'.");
        sbMsg.AppendLine("Transferred from inter location : " + FromInterLocation + ".");
        sbMsg.AppendLine("Transferred to inter location : " + ToInterLocation + ".");
        sbMsg.AppendLine("Transferred from process : " + AssetProcess + ".");
        sbMsg.AppendLine("Transferred to process : " + ToInterProcessCode + ".");
        sbMsg.AppendLine("Transferred from workstation : " + FromWorkStation + ".");
        sbMsg.AppendLine("Transferred to workstation : " + ToWorkStation + ".");
        sbMsg.AppendLine("Transferred from port no. : " + FromPort + ".");
        sbMsg.AppendLine("Transferred to port no. : " + ToPort + ".");
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
    /// Send mail for approval for asset being transferred from one location to another.
    /// </summary>
    /// <param name="AssetCode"></param>
    /// <param name="FromLocation"></param>
    /// <param name="ToLocation"></param>
    private void SendMailForApproval(string AssetCode, string FromLocation, string ToLocation, string LoginUser)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SENDER"].ToString(), "ATS");
        smtpClient.Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
        message.From = fromAddress;
        message.To.Add(Session["EMAIL"].ToString());
        message.Subject = "BCIL : ATS - Approval For Country Wide Asset Transfer";
        message.IsBodyHtml = false;
        StringBuilder sbMsg = new StringBuilder();
        sbMsg.AppendLine("Please Note,");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Assets have been transferred (country-wide) with asset code as : " + AssetCode + " by '" + LoginUser + "'.");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Transferred from location : " + FromLocation + ".");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Transferred to other location : " + ToLocation + ".");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Kindly approve through the link given below.");
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
    /// Refresh/reset category code/name details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshCategory_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateCategory(lblAssetType.Text);
            DataTable dtNull = new DataTable();           
            ddlAssetMake.DataSource = dtNull;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dtNull;
            lstModelName.DataBind();
            dtNull = null;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Save/update asset transfer details based on transfer type selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            iAssetCount = 0;
            bool bResp = false;
            TransferedAssets = "";
            bool bAssetSelected = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (txtTransferDate.Text.Trim() != "")
            {
                int iDate = clsGeneral.CompareDate(txtTransferDate.Text.Trim(), DateTime.Now.ToString("dd/MMM/yyyy"));
                if (iDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Transfer date cannot be later than current date!');", true);
                    txtTransferDate.Focus();
                    return;
                }
            }
            oPRP.TransferDate = txtTransferDate.Text.Trim();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.IUTStatus = (ddlAssetIUT.SelectedIndex != 0) ? ddlAssetIUT.SelectedValue.ToString() : "";
            for (int iCnt = 0; iCnt < gvAssets.Rows.Count; iCnt++)
            {
                if (((CheckBox)gvAssets.Rows[iCnt].FindControl("chkSelectAsset")).Checked == true)
                {
                    bAssetSelected = true;
                    break;
                }
            }
            if (!bAssetSelected)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Click on Get Assets button to select assets.');", true);
                return;
            }
            oPRP.TransferRemarks = txtRemarks.Text.Trim().Replace("'", "`");
            foreach (GridViewRow gvRow in gvAssets.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)                
                    iAssetCount++;
            }
            if (iAssetCount > 25)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Maximum 25 assets can be selected for transfer.');", true);
                return;
            }
            if (rdoCntryTransfer.Checked)
            {
                oPRP.TransferType = "COUNTRY";
                if (lblToLocCode.Text.Trim() == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Transfer To location cannot be blank.');", true);
                    return;
                }
                oPRP.ToLocationCode = lblToLocCode.Text.Trim().Split(';')[0];
                oPRP.CompCode = lblToLocCode.Text.Trim().Split(';')[1];
                foreach (GridViewRow gvRow in gvAssets.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                    {
                        oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                        oPRP.SerialNo = ((Label)gvRow.FindControl("lblSerialCode")).Text.Trim();
                        oPRP.AssetModel = ((Label)gvRow.FindControl("lblModelName")).Text.Trim();
                        oPRP.AssetProcess = ((Label)gvRow.FindControl("lblProcess")).Text.Trim();
                        oPRP.FromLocationCode = ((Label)gvRow.FindControl("lblFromLocationCode")).Text.Trim();
                        bResp = oDAL.SaveAssetTransferDetails(oPRP);
                        TransferedAssets += "," + oPRP.AssetCode;
                    }
                }
                TransferedAssets = TransferedAssets.TrimStart(',');
                try
                { SendMailForApproval(TransferedAssets, oPRP.FromLocationCode, oPRP.ToLocationCode, Session["CURRENTUSER"].ToString()); }
                catch (Exception ex) { }
            }
            if (rdoInterTransfer.Checked)
            {
                oPRP.TransferType = "INTER";
                if (txtToPort.Text.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Transfer To port no. cannot be blank.');", true);
                    txtToPort.Focus();
                    return;
                }
                if (lblInterLocCode.Text.Trim() == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Transfer To location cannot be blank.');", true);
                    ddlInterToLocation.Focus();
                    return;
                }
                if (ddlInterToProcess.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Transfer To process cannot be blank.');", true);
                    ddlInterToProcess.Focus();
                    return;
                }
                oPRP.ToInterLocationCode = lblInterLocCode.Text.Trim();
                oPRP.ToInterProcessCode = ddlInterToProcess.SelectedValue.ToString();
                oPRP.ToWorkStation = txtToWorkStation.Text.Trim();
                oPRP.ToPort = txtToPort.Text.Trim();
                foreach (GridViewRow gvRow in gvAssets.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                    {
                        oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                        oPRP.SerialNo = ((Label)gvRow.FindControl("lblSerialCode")).Text.Trim();
                        oPRP.AssetModel = ((Label)gvRow.FindControl("lblModelName")).Text.Trim();
                        oPRP.AssetProcess = ((Label)gvRow.FindControl("lblProcess")).Text.Trim();
                        oPRP.FromPort = ((Label)gvRow.FindControl("lblPortNo")).Text.Trim();
                        oPRP.FromWorkStation = ((Label)gvRow.FindControl("lblWorkStationNo")).Text.Trim();
                        oPRP.FromInterLocationCode = ((Label)gvRow.FindControl("lblFromLocationCode")).Text.Trim();
                        if (oPRP.FromWorkStation == oPRP.ToWorkStation)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Transfer from workstation and to workstation cannot be same.');", true);
                            break;
                        }
                        if (oPRP.FromPort == oPRP.ToPort)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Transfer from port and to port cannot be same.');", true);
                            break;
                        }
                        bResp = oDAL.SaveAssetTransferDetails(oPRP);
                        TransferedAssets += "," + oPRP.AssetCode;
                    }
                }
                TransferedAssets = TransferedAssets.TrimStart(',');
                try
                { SendMailForApproval(TransferedAssets, oPRP.FromInterLocationCode, oPRP.ToInterLocationCode, oPRP.AssetProcess, oPRP.ToInterProcessCode, oPRP.FromWorkStation, oPRP.ToWorkStation, oPRP.FromPort, oPRP.ToPort, Session["CURRENTUSER"].ToString()); }
                catch (Exception ex) { }
            }
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset Transfer Details not saved.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset transfer details saved successfully. Transferred assets need to be approved.');", true);
            }
            GetAssetsForTransfer();
            GetAssetTransferDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Based on asset code, asset details are populated into form controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (txtAssetCode.Text.Trim() != "")
            {
                bool ChkAssetTransferred = oDAL.ChkAssetTransferred(txtAssetCode.Text.Trim(), Session["COMPANY"].ToString());
                if (!ChkAssetTransferred)
                {
                    DataTable dt = new DataTable();
                    dt = oDAL.GetAssetDetails(txtAssetCode.Text.Trim(), Session["COMPANY"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        txtSerialNo.Text = dt.Rows[0]["SERIAL_CODE"].ToString();
                        ddlProcessName.SelectedValue = dt.Rows[0]["ASSET_PROCESS"].ToString();
                        lblFromLocCode.Text = dt.Rows[0]["ASSET_LOCATION"].ToString();
                        lblFromLocationName.Text = dt.Rows[0]["LOC_NAME"].ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset not found/Asset not approved.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset is already transferred to another location.');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Reset/Crear fields and set location details refreshed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateCategory(lblAssetType.Text);
            DataTable dt = new DataTable();
            PopulateFromLocation();
            lblLocLevel2.Text = "1";
            PopulateInterLocation();
            lblFromLocationName.Text = "";
            PopulateToLocation();
            PopulateToProcess();
            gvAssets.DataSource = null;
            gvAssets.DataBind();

            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            dt = null;
            lblAssetCount.Text = "Assets Count : 0";
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh/reset location code/name into dropdowns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreahLocation_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateLocation();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh/reset location code/name into dropdowns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnRefreshLocation1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateFromLocation();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh/reset location/sub location/floor no. dropdowns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnRefreshLocation2_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateToLocation();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export asset trasnfer history data into excel file.
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
            if (gvAssetTransfer.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["TRANSFER"];
                dt.TableName = "TransferReport";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
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

    /// <summary>
    /// Export transferred asset data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGetTransferredData_Click(object sender, EventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[4] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                if (lblLocCode.Text.Trim() == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select asset transferred location.');", true);
                    return;
                }
                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.FromDate = txtFromDate.Text.Trim();
                oPRP.ToDate = txtToDate.Text.Trim();

                string[] LocParts = { };
                if (lblLocCode.Text != "0")
                    LocParts = lblLocCode.Text.Trim().Split('-');
                if (lblLocLevel.Text.Trim() == "2")
                    oPRP.TransferLocation = LocParts[0] + "-" + LocParts[1];
                else if (lblLocLevel.Text.Trim() == "3")
                    oPRP.TransferLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
                else if (lblLocLevel.Text.Trim() == "4")
                    oPRP.TransferLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
                else if (lblLocLevel.Text.Trim() == "5")
                    oPRP.TransferLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
                else if (lblLocLevel.Text.Trim() == "6")
                    oPRP.TransferLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
                else
                    oPRP.TransferLocation = "";

                DataTable dt = oDAL.GetTransferredAssets(oPRP);
                if (dt.Rows.Count > 0)
                {
                    Response.Clear();
                    DataSet dsExport = new DataSet();
                    System.IO.StringWriter tw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                    DataGrid dgGrid = new DataGrid();
                    dgGrid.DataSource = dt;
                    dgGrid.HeaderStyle.Font.Bold = true;
                    dgGrid.DataBind();
                    dgGrid.RenderControl(hw);
                    Response.ContentType = "application/vnd.ms-excel";
                    this.EnableViewState = false;
                    Response.Write(tw.ToString());
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no data to be exported.');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh/reset form fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            PopulateLocation();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get assets for transfer based on search criteria provided.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            GetAssetsForTransfer();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh/reset location/sub location/floor no. dropdowns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnRefreshLocation_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateInterLocation();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SELECTED INDEX CHANGED EVENTS
    /// <summary>
    /// Populate sub-locations based on parent location selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLocation.SelectedIndex > 0)
            {
                int locLevel = int.Parse(lblLocLevel.Text.Trim());
                lblLocLevel.Text = (locLevel + 1).ToString();
                int iLocLevel = int.Parse(lblLocLevel.Text.Trim());
                string sLocCode = ddlLocation.SelectedValue.ToString();
                lblLocCode.Text = sLocCode;

                ddlLocation.DataSource = null;
                DataTable dt = oDAL.GetToLocation(Session["COMPANY"].ToString(),sLocCode, iLocLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlLocation.DataSource = dt;
                    ddlLocation.DataValueField = "LOC_CODE";
                    ddlLocation.DataTextField = "LOC_NAME";
                    ddlLocation.DataBind();
                    ddlLocation.Items.Insert(0, "-- Select Location --");
                    ddlLocation.Focus();
                }
                else
                {
                    iLocLevel = iLocLevel - 1;
                    lblLocLevel.Text = iLocLevel.ToString();
                    btnGetTransferredData.Focus();
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
    //protected void ddlFromLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlFromLocation.SelectedIndex > 0)
    //        {
    //            int locLevel = int.Parse(lblLocLevel1.Text.Trim());
    //            lblLocLevel1.Text = (locLevel + 1).ToString();
    //            int iLocLevel = int.Parse(lblLocLevel1.Text.Trim());
    //            string sLocCode = ddlFromLocation.SelectedValue.ToString();
    //            lblFromLocCode.Text = sLocCode;

    //            ddlFromLocation.DataSource = null;
    //            DataTable dt = oDAL.GetFromLocation(Session["COMPANY"].ToString(),sLocCode, iLocLevel);
    //            if (dt.Rows.Count > 0)
    //            {
    //                ddlFromLocation.DataSource = dt;
    //                ddlFromLocation.DataValueField = "LOC_CODE";
    //                ddlFromLocation.DataTextField = "LOC_NAME";
    //                ddlFromLocation.DataBind();
    //                ddlFromLocation.Items.Insert(0, "-- Select Location --");
    //            }
    //            else
    //            {
    //                iLocLevel = iLocLevel - 1;
    //                lblLocLevel.Text = iLocLevel.ToString();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        HandleExceptions(ex);
    //    }
    //}

    /// <summary>
    /// Populate locations for country-wide asset transfer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlToLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlToLocation.SelectedIndex > 0)
            {
                int locLevel = int.Parse(lblLocLevel2.Text.Trim());
                lblLocLevel2.Text = (locLevel + 1).ToString();
                int iLocLevel = int.Parse(lblLocLevel2.Text.Trim());
                string sLocCode = ddlToLocation.SelectedValue.ToString().Split(';')[0].ToString();
                lblToLocCode.Text = ddlToLocation.SelectedValue.ToString();  //sLocCode;

                ddlToLocation.DataSource = null;
                DataTable dt = oDAL.GetToLocation(Session["COMPANY"].ToString(), sLocCode, iLocLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlToLocation.DataSource = dt;
                    ddlToLocation.DataTextField = "LOC_NAME";
                    ddlToLocation.DataValueField = "LOCATION_CODE";
                    ddlToLocation.DataBind();
                    ddlToLocation.Items.Insert(0, "-- Select Location --");
                    ddlToLocation.Focus();
                }
                else
                {
                    iLocLevel = iLocLevel - 1;
                    lblLocLevel.Text = iLocLevel.ToString();
                    txtRemarks.Focus();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get location code/name to be populated into dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlInterToLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlInterToLocation.SelectedIndex != 0)
            {
                int locLevel = int.Parse(lblInterLocLevel.Text.Trim());
                lblInterLocLevel.Text = (locLevel + 1).ToString();
                int iLocLevel = int.Parse(lblInterLocLevel.Text.Trim());
                string sLocCode = ddlInterToLocation.SelectedValue.ToString();
                lblInterLocCode.Text = sLocCode;

                ddlInterToLocation.DataSource = null;
                DataTable dt = oDAL.GetLocation(Session["COMPANY"].ToString(), sLocCode, iLocLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlInterToLocation.DataSource = dt;
                    ddlInterToLocation.DataValueField = "LOC_CODE";
                    ddlInterToLocation.DataTextField = "LOC_NAME";
                    ddlInterToLocation.DataBind();
                    ddlInterToLocation.Items.Insert(0, "-- Select Location --");
                    ddlInterToLocation.Focus();
                }
                else
                {
                    iLocLevel = iLocLevel - 1;
                    lblInterLocLevel.Text = iLocLevel.ToString();
                    txtToWorkStation.Focus();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Fetch list of categories based on asset type selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtNull = new DataTable();
            ddlAssetCategory.DataSource = dtNull;
            ddlAssetCategory.DataBind();
            ddlAssetMake.DataSource = dtNull;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dtNull;
            lstModelName.DataBind();
            dtNull = null;

            if (ddlAssetType.SelectedValue.ToString() == "AD")
            {
                lblAssetType.Text = "ADMIN";
                PopulateCategory(lblAssetType.Text);
            }
            else if (ddlAssetType.SelectedValue.ToString() == "IT")
            {
                lblAssetType.Text = "IT";
                PopulateCategory(lblAssetType.Text);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Populate asset model name based on asset make selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetMake.SelectedIndex != 0)
                PopulateModelName(ddlAssetMake.SelectedValue.ToString(), lblCatCode.Text.Trim());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Populate sub-category based on parent category selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetCategory.SelectedIndex > 0)
            {
                DataTable dtNull = new DataTable();
                lstModelName.DataSource = dtNull;
                lstModelName.DataBind();
                dtNull = null;

                PopulateAssetMake(ddlAssetCategory.SelectedValue.ToString());
                int CatLevel = int.Parse(lblCatLevel.Text.Trim());
                lblCatLevel.Text = (CatLevel + 1).ToString();
                int iCatLevel = int.Parse(lblCatLevel.Text.Trim());
                string sCatCode = ddlAssetCategory.SelectedValue.ToString();
                lblCatCode.Text = sCatCode;

                ddlAssetCategory.DataSource = null;
                DataTable dt = oDAL.PopulateCategory(lblAssetType.Text, sCatCode, iCatLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlAssetCategory.DataSource = dt;
                    ddlAssetCategory.DataValueField = "CATEGORY_CODE";
                    ddlAssetCategory.DataTextField = "CATEGORY_NAME";
                    ddlAssetCategory.DataBind();
                    ddlAssetCategory.Items.Insert(0, "-- Select Category --");
                    ddlAssetCategory.Focus();
                }
                else
                {
                    iCatLevel = iCatLevel - 1;
                    lblCatLevel.Text = iCatLevel.ToString();
                    ddlAssetMake.Focus();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Embed checkbox into asset grid for asset selection for being transferred.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssets_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkSelect = (CheckBox)e.Row.Cells[0].FindControl("chkSelectAsset");
                CheckBox chkHSelect = (CheckBox)this.gvAssets.HeaderRow.FindControl("chkHSelect");
                chkSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkHSelect.ClientID);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Asset deletion from asset acquisition if not allocated.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetTransfer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
            }
            else
            {
                GridViewRow gvRow = (GridViewRow)gvAssetTransfer.Rows[e.RowIndex];
                oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                oPRP.TransferType = ((Label)gvRow.FindControl("lblTransferType")).Text.Trim();
                oPRP.CompCode = Session["COMPANY"].ToString();
                oDAL.DeleteTransferDetails(oPRP.AssetCode, oPRP.CompCode, oPRP.TransferType);
                GetAssetTransferDetails();
                upGrid.Update();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Assets gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssets.PageIndex = e.NewPageIndex;
            gvAssets.DataSource = (DataTable)Session["ASSETS"];
            gvAssets.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Asset transfer history grid page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssetTransfer.PageIndex = e.NewPageIndex;
            GetAssetTransferDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region CHECKED CHANGED EVENTS
    /// <summary>
    /// Country-wide asset transfer fields enabling.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoCntryTransfer_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoCntryTransfer.Checked)
            {
                PopulateToLocation();
                ddlToLocation.Enabled = true;
                ibtnRefreshLocation2.Enabled = true;

                PopulateInterLocation();
                PopulateToProcess();
                txtToWorkStation.Text = "";
                txtToPort.Text = "";

                ddlInterToLocation.Enabled = false;
                ibtnRefreshLocation.Enabled = false;
                ddlInterToProcess.Enabled = false;
                txtToWorkStation.Enabled = false;
                txtToPort.Enabled = false;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Inter-location asset transfer fields enabling.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoInterTransfer_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoInterTransfer.Checked)
            {
                PopulateToLocation();
                ddlToLocation.Enabled = false;
                ibtnRefreshLocation2.Enabled = false;

                PopulateInterLocation();
                PopulateToProcess();
                txtToWorkStation.Text = "";
                txtToPort.Text = "";

                ddlInterToLocation.Enabled = true;
                ibtnRefreshLocation.Enabled = true;
                ddlInterToProcess.Enabled = true;
                txtToWorkStation.Enabled = true;
                txtToPort.Enabled = true;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}