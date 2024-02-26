using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public partial class GenerateGatePass : System.Web.UI.Page
{
    GatePassGeneration_DAL oDAL;
    GatePassGeneration_PRP oPRP;
    bool bTransOpen = false;
    public GenerateGatePass()
    {
        oPRP = new GatePassGeneration_PRP();
    }
    ~GenerateGatePass()
    {
        oDAL = null; oPRP = null;
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
        oDAL = new GatePassGeneration_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking user group rights for gate pass generation operation.
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
                string _strRights = clsGeneral.GetRights("GATEPASS_GENERATION", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "GATEPASS_GENERATION");
                btnExport.Visible = false;
                btnExport.Enabled = false;
                PopulateLocationFilter();
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                //   ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
                // lblAssetType.Text = clsGeneral.gStrAssetType;
                //PopulateCategory("");
                PopulateLocation();
                if ((string)Request.QueryString["GatePassCode"] != null)
                {
                    string _GatePassCode = Request.QueryString["GatePassCode"].ToString();
                    GetGatePassDetails(_GatePassCode);
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Populate asset details based on filters supplied.
    /// </summary>
    //private void PopulateAssetDetails()
    //{
    //    gvAssets.DataSource = null;
    //    DataTable dt = new DataTable();
    //    oPRP.AssetCode = "";

    //    string[] LocParts = { };
    //    if (lblLocCode.Text != "0")
    //    {

    //        oPRP.AssetLocation = lblLocCode.Text;
    //        //LocParts = lblLocCode.Text.Trim().Split('-');
    //        //if (LocParts[2] == "00")
    //        //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1];
    //        //else if (LocParts[3] == "00")
    //        //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
    //        //else if (LocParts[4] == "00")
    //        //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
    //        //else if (LocParts[5] == "00")
    //        //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
    //        //else
    //        //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
    //    }
    //    else
    //        oPRP.AssetLocation = "";

    //    if (ddlAssetMake.SelectedIndex != 0)
    //        oPRP.AssetMake = ddlAssetMake.SelectedValue.ToString();
    //    else
    //        oPRP.AssetMake = "";
    //    for (int iCnt = 0; iCnt < lstModelName.Items.Count; iCnt++)
    //    {
    //        if (lstModelName.Items[iCnt].Selected)
    //            oPRP.ModelName += lstModelName.Items[iCnt].Value.ToString() + ",";
    //    }
    //    if (oPRP.ModelName != null)
    //    {
    //        oPRP.ModelName = oPRP.ModelName.TrimEnd(',');
    //        oPRP.ModelName = oPRP.ModelName.Replace(",", "','");
    //        oPRP.ModelName = "'" + oPRP.ModelName + "'";
    //    }
    //    else
    //        oPRP.ModelName = "";
    //    if (txtAssetCode.Text != "")
    //    {
    //        if (txtAssetCode.Text.Contains("|"))
    //            oPRP.AssetTag = (txtAssetCode.Text.Split('|')[0].Trim());
    //        else
    //            oPRP.AssetTag = txtAssetCode.Text.Trim();
    //    }
    //    else
    //    {
    //        oPRP.AssetTag = txtAssetCode.Text.Trim();
    //    }
    //    if (ddlAssetCategory.SelectedIndex != 0)
    //        oPRP.CategoryCode = ddlAssetCategory.SelectedValue.ToString();
    //    else
    //        oPRP.CategoryCode = "";
    //    oPRP.CompCode = Session["COMPANY"].ToString();
    //    dt = oDAL.GetAssetsWithLocation(oPRP);
    //    if (dt.Rows.Count > 0)
    //    {
    //        gvAssets.DataSource = dt;
    //        gvAssets.DataBind();
    //        gvAssets.Visible = true;
    //        lblAssetCount.Text = "Assets Count : " + dt.Rows.Count.ToString();
    //    }
    //    else
    //    {
    //        gvAssets.DataSource = null;
    //        gvAssets.Visible = false;
    //        lblAssetCount.Text = "Assets Count : 0";
    //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Assets not found in the selected criterian.');", true);
    //        return;
    //    }
    //}

    /// <summary>
    /// populate gate pass details into gridview.
    /// </summary>
    /// 
    private void PopulateLocationFilter()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetSiteLocation(Session["COMPANY"].ToString());
        ddlSiteLocationFilter.DataSource = null;
        ddlSiteLocationFilter.DataSource = dt;
        ddlSiteLocationFilter.DataValueField = "SITE_CODE";
        ddlSiteLocationFilter.DataTextField = "SITE_CODE";
        ddlSiteLocationFilter.DataBind();
        //ddlSiteLocationFilter.Items.Insert(0, "-- Select Location --");
    }
    private void GetGatePassDetails(string _GatePassCode)
    {
        txtGatePassNo.Text = _GatePassCode.ToString().Trim();
        DataSet ds = oDAL.GetGatePassDetails(_GatePassCode);
        DataTable dtGPG = ds.Tables[0];
        DataTable dtGPA = ds.Tables[1];

        if (dtGPG.Rows.Count > 0)
        {
            clsGeneral.gStrApproveStatus = bool.Parse(dtGPG.Rows[0]["APPROVE_GATEPASS"].ToString());
            txtGatePassNo.Text = dtGPG.Rows[0]["GATEPASS_CODE"].ToString();
            ddlGatePassType.SelectedValue = dtGPG.Rows[0]["GATEPASS_TYPE"].ToString();
            ddlGatePassType.Enabled = false;
            txtGatePassDate.Text = Convert.ToDateTime(dtGPG.Rows[0]["GATEPASS_DATE"].ToString()).ToString("dd/MMM/yyyy");
            txtGatePassDate.Enabled = false;
            txtDocumentNo.Text = dtGPG.Rows[0]["DOCUMENT_NO"].ToString();
            txtDocumentNo.Enabled = false;
            //if (dtGPG.Rows[0]["GATEPASS_VENDOR_CODE"].ToString() != "")
            //{
            //    ddlGatePassFor.SelectedIndex = 2;
            //    ddlGatePassFor_SelectedIndexChanged(this, EventArgs.Empty);
            //    ddlGatePassForName.SelectedValue = dtGPG.Rows[0]["GATEPASS_VENDOR_CODE"].ToString();
            //}
            //else if (dtGPG.Rows[0]["GATEPASS_EMPLOYEE_CODE"].ToString() != "")
            //{
            //    ddlGatePassFor.SelectedIndex = 1;
            //    ddlGatePassFor_SelectedIndexChanged(this, EventArgs.Empty);
            //    ddlGatePassForName.SelectedValue = dtGPG.Rows[0]["GATEPASS_EMPLOYEE_CODE"].ToString();
            //}
            // ddlGPLocation.SelectedValue = dtGPG.Rows[0]["ASSET_LOCATION"].ToString();
            ddlDestLocation.SelectedValue = dtGPG.Rows[0]["DESTINATION_LOCATION"].ToString();
            //ddlGPLocation.Enabled = false;
            ddlDestLocation.Enabled = false;
            //  txtGPCarrier.Text = dtGPG.Rows[0]["GATEPASS_CARRIER_NAME"].ToString();
            //  txtGPBearerName.Text = dtGPG.Rows[0]["GATEPASS_BEARER_NAME"].ToString();
            if (ddlGatePassType.SelectedValue == "RETURNABLE")
            {
                if (Convert.ToDateTime(dtGPA.Rows[0]["EXP_RETURN_DATE"].ToString()).ToString("dd/MMM/yyyy") != "01/Jan/1900")
                    txtReturnableDate.Text = Convert.ToDateTime(dtGPA.Rows[0]["EXP_RETURN_DATE"].ToString()).ToString("dd/MMM/yyyy");
                txtReturnableDate.Enabled = false;
            }
            txtGPRemarks.Text = dtGPG.Rows[0]["PURPOSE"].ToString();
            txtGPRemarks.Visible = false;

        }
        if (dtGPA.Rows.Count > 0)
        {
            gvAssets.DataSource = null;
            gvAssets.DataSource = dtGPA;
            gvAssets.DataBind();
            foreach (GridViewRow gvRow in gvAssets.Rows)
                ((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked = true;
            gvAssets.Enabled = true;
            btnSearch.Enabled = false;
        }
        btnSubmit.Enabled = false;
        //  btnClear.Enabled = false;
       // btnRefreshLocation.Enabled = false;
        btnSearch.Enabled = false;
        btnPrintGatePass.Enabled = true;
    }

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Generate GatePass");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Response.Redirect("Error.aspx");
        }
    }

    /// <summary>
    /// Get location code/name for location selection.
    /// </summary>
    private void PopulateLocation()
    {
        //lblLocCode.Text = "0";
        //ddlGPLocation.DataSource = null;
        //DataTable dt = new DataTable();
        //dt = oDAL.GetLocation(Session["COMPANY"].ToString(), "", 1);
        //ddlGPLocation.DataSource = dt;
        //ddlGPLocation.DataValueField = "LOC_CODE";
        //ddlGPLocation.DataTextField = "LOC_NAME";
        //ddlGPLocation.DataBind();
        //ddlGPLocation.Items.Insert(0, "-- Select Location --");

        DataTable dt = new DataTable();
        lblDestCode.Text = "0";
        ddlDestLocation.DataSource = null;
        dt = oDAL.GetLocation(Session["COMPANY"].ToString(), "", 1);
        ddlDestLocation.DataSource = dt;
        ddlDestLocation.DataValueField = "LOC_CODE";
        ddlDestLocation.DataTextField = "LOC_CODE";
        ddlDestLocation.DataBind();
        ddlDestLocation.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    private void MailGatepassDetails(string GatePassCode)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SENDER"].ToString(), "ATS");
        smtpClient.Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
        message.From = fromAddress;
        message.To.Add(Session["EMAIL"].ToString());
        message.Subject = "BCIL : ATS - New GatePass For Approval";
        message.IsBodyHtml = false;
        StringBuilder sbMsg = new StringBuilder();

        DataTable dt = new DataTable();
        dt = oDAL.GetPrintGatepassDetails(GatePassCode, clsGeneral.gStrApproveStatus);
        DataRow dr = dt.Rows[0];

        sbMsg.AppendLine("Please Note,");
        sbMsg.AppendLine("A new gatepass has been generated for being approved.");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Gatepass Code : " + dr["GATEPASS_CODE"].ToString());
        sbMsg.AppendLine("Gatepass Date : " + dr["GATEPASS_DATE"].ToString());
        sbMsg.AppendLine("Bearer Name : " + dr["GATEPASS_BEARER_NAME"].ToString());
        sbMsg.AppendLine("Carrier Name : " + dr["GATEPASS_CARRIER_NAME"].ToString());

        if (dr["GATEPASS_EMPLOYEE_CODE"].ToString().Trim() != "")
        {
            sbMsg.AppendLine("");
            sbMsg.AppendLine("Emloyee Code : " + dr["GATEPASS_EMPLOYEE_CODE"].ToString());
            sbMsg.AppendLine("Emloyee Name : " + dr["GATE_PASS_FOR"].ToString().Split(';')[1].Trim());
        }
        else
        {
            sbMsg.AppendLine("");
            sbMsg.AppendLine("Vendor Code : " + dr["VENDOR_CODE"].ToString());
            sbMsg.AppendLine("Vendor Name : " + dr["VENDOR_NAME"].ToString());
        }
        if (dr["ASSET_LOCATION"].ToString().Trim() == "")
            sbMsg.AppendLine("");
        else
            sbMsg.AppendLine("Asset Location : " + dr["ASSET_LOCATION"].ToString());
        sbMsg.AppendLine("");
        sbMsg.AppendLine("ASSET CODE          SERIAL No.          ASSET MAKE          MODEL TYPE          RETURNABLE DATE");
        for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
        {
            sbMsg.AppendLine(dt.Rows[iCnt]["ASSET_CODE"].ToString() + "  " + dt.Rows[iCnt]["SERIAL_CODE"].ToString() + "  " + dt.Rows[iCnt]["ASSET_MAKE"].ToString() + "  " + dt.Rows[iCnt]["MODEL_NAME"].ToString() + "  " + dt.Rows[iCnt]["EXP_RETURN_DATE"].ToString());
        }
        sbMsg.AppendLine("Total Assets : " + dr["TOTAL"].ToString());
        sbMsg.AppendLine("");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("http://10.164.91.191/FIS_ATS/WEBPAGES/UserLogin.aspx");
        message.Body = sbMsg.ToString();
        smtpClient.Send(message);
    }

    /// <summary>
    /// Fetch category details to be populated in dropdownlist.
    /// </summary>
    //private void PopulateCategory(string AssetType)
    //{
    //    lblCatCode.Text = "0";
    //    lblCatLevel.Text = "1";
    //    ddlAssetCategory.DataSource = null;
    //    DataTable dt = new DataTable();
    //    dt = oDAL.PopulateCategory(Convert.ToString(Session["COMPANY"]));
    //    ddlAssetCategory.DataSource = dt;
    //    ddlAssetCategory.DataTextField = "CATEGORY_NAME";
    //    ddlAssetCategory.DataValueField = "CATEGORY_CODE";
    //    ddlAssetCategory.DataBind();
    //    ddlAssetCategory.Items.Insert(0, "-- Select Category --");
    //}

    /// <summary>
    /// 
    /// </summary>
    //private void PopulateAssetMake(string CategoryCode)
    //{
    //    ddlAssetMake.DataSource = null;
    //    DataTable dt = new DataTable();
    //    dt = oDAL.PopulateAssetMake(CategoryCode, Session["COMPANY"].ToString());
    //    ddlAssetMake.DataSource = dt;
    //    ddlAssetMake.DataTextField = "ASSET_MAKE";
    //    ddlAssetMake.DataValueField = "ASSET_MAKE";
    //    ddlAssetMake.DataBind();
    //    ddlAssetMake.Items.Insert(0, "-- Select Make --");
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AssetMake"></param>
    //private void PopulateModelName(string AssetMake, string CategoryCode)
    //{
    //    lstModelName.DataSource = null;
    //    DataTable dt = new DataTable();
    //    dt = oDAL.PopulateModelName(AssetMake, CategoryCode, Session["COMPANY"].ToString());
    //    lstModelName.DataSource = dt;
    //    lstModelName.DataTextField = "MODEL_NAME";
    //    lstModelName.DataValueField = "MODEL_NAME";
    //    lstModelName.DataBind();
    //    lstModelName.Items.Insert(0, "-- Select Model --");
    //}
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Refresh/reset category to top level.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshCategory_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //PopulateCategory("");
            DataTable dtNull = new DataTable();
            //ddlAssetMake.DataSource = dtNull;
            //ddlAssetMake.DataBind();
            //lstModelName.DataSource = dtNull;
            //lstModelName.DataBind();
            dtNull = null;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Save Gate Pass generation details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtITIUTLocation = new DataTable();
            dtITIUTLocation.Columns.Add("GatePass Code");
            dtITIUTLocation.Columns.Add("Asset Code");
            dtITIUTLocation.Columns.Add("GatePass Date");
            dtITIUTLocation.Columns.Add("Document No");
            dtITIUTLocation.Columns.Add("Source Location");
            dtITIUTLocation.Columns.Add("Destination Location");
            dtITIUTLocation.Columns.Add("Asset Type");
            dtITIUTLocation.Columns.Add("Asset Make");
            dtITIUTLocation.Columns.Add("Asset Model");
            dtITIUTLocation.Columns.Add("Serial Number");
            dtITIUTLocation.Columns.Add("PO No");
            dtITIUTLocation.Columns.Add("PO Date");
            dtITIUTLocation.Columns.Add("Invoice No");
            dtITIUTLocation.Columns.Add("Invoice Date");

            DataTable dtFacilitiesIUTLocation = new DataTable();
            dtFacilitiesIUTLocation.Columns.Add("GatePass Code");
            dtFacilitiesIUTLocation.Columns.Add("Asset Code");
            dtFacilitiesIUTLocation.Columns.Add("GatePass Date");
            dtFacilitiesIUTLocation.Columns.Add("Document No");
            dtFacilitiesIUTLocation.Columns.Add("Source Location");
            dtFacilitiesIUTLocation.Columns.Add("Destination Location");
            dtFacilitiesIUTLocation.Columns.Add("Asset Type");
            dtFacilitiesIUTLocation.Columns.Add("Asset Make");
            dtFacilitiesIUTLocation.Columns.Add("Asset Model");
            dtFacilitiesIUTLocation.Columns.Add("Asset Far Tag");
            dtFacilitiesIUTLocation.Columns.Add("PO No");
            dtFacilitiesIUTLocation.Columns.Add("PO Date");
            dtFacilitiesIUTLocation.Columns.Add("Invoice No");
            dtFacilitiesIUTLocation.Columns.Add("Invoice Date");

            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Generate GatePass", "Generate GatePass", "trying to generate gatepass by user id " + Session["CURRENTUSER"].ToString() + "");
            bool bAssetSelected = false;
            //oDAL.Transaction("BEGIN");
            string _LiveGPCode = "";
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
           
            if (lblDestCode.Text == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Destination Location cannot be blank.');", true);
                return;
            }

           
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
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note :  select assets from grid or search asset for gatepass.');", true);
                return;
            }
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.DocumentNo = txtDocumentNo.Text;
            oPRP.BearerName = txtGPBearerName.Text.Trim();
            oPRP.CarrierName = txtGPCarrier.Text.Trim();
            oPRP.DestinationLocation = lblDestCode.Text.Trim();
            //oPRP.GatePassDate = Convert.ToDateTime(txtGatePassDate.Text.Trim());
            oPRP.GatePassDate = txtGatePassDate.Text.Trim();
            oPRP.GatePassType =(ddlGatePassType.SelectedIndex>0? ddlGatePassType.SelectedValue.ToString():"");

            oPRP.AssetLocation = (ddlSiteLocationFilter.SelectedIndex>0? ddlSiteLocationFilter.SelectedValue :""); // lblLocCode.Text.Trim();
            if(oPRP.AssetLocation == oPRP.DestinationLocation)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('From Location & Destination Location should not be same.');", true);
                return;
            }
            oPRP.Remarks = txtGPRemarks.Text.Trim();
            if (ddlGatePassFor.SelectedValue.ToString() == "EMPLOYEE")
                oPRP.EmpCode = ddlGatePassForName.SelectedValue.ToString();
            else if (ddlGatePassFor.SelectedValue.ToString() == "VENDOR")
                oPRP.VendorCode = ddlGatePassForName.SelectedValue.ToString();
            oPRP.Approve_GatePass = true;

            int _iMaxACQId = oDAL.GetMaxRunningSrlNo(Session["COMPANY"].ToString());
            oPRP.GatePassCode = oPRP.CompCode + "-" + "GP" + "-" + _iMaxACQId.ToString().PadLeft(6, '0');
            oPRP.Running_Serial_No = _iMaxACQId;
            bool bResp = oDAL.SaveGatePassDetails(oPRP);
            bool bRsp = false;
            foreach (GridViewRow gvRow in gvAssets.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                {
                    oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                    oPRP.AssetTag = ((Label)gvRow.FindControl("lblRafID")).Text.Trim();
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.SerialCode = ((Label)gvRow.FindControl("lblSrNo")).Text.Trim();
                    else
                        oPRP.ASSET_FAR_TAG = ((Label)gvRow.FindControl("lblAssetFarTag")).Text.Trim();

                    _LiveGPCode = oDAL.ChkLiveGP(oPRP.AssetCode);
                    if (_LiveGPCode != "")
                    {
                        lblErrorMsg.Text = "Selected asset is already part of Gate Pass No: " + _LiveGPCode + " and is not returned yet.";
                        return;
                    }
                    if (txtReturnableDate.Text.Trim() != "")
                    {
                        //oPRP.ExpReturnDate = Convert.ToDateTime(txtReturnableDate.Text.Trim());
                        oPRP.ExpReturnDate = txtReturnableDate.Text.Trim();
                        //if (oPRP.ExpReturnDate < Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy")))
                        if (Convert.ToDateTime(oPRP.ExpReturnDate) < Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy")))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Expected return Date should be greater than current date.');", true);
                            return;
                        }
                    }
                    else
                        oPRP.ExpReturnDate = string.Empty; //"01/Jan/1900";
                    //oPRP.ExpReturnDate = Convert.ToDateTime("01/Jan/1900");
                    bRsp = oDAL.SaveGatePassAssets(oPRP);
                    if(bRsp)
                    {
                        if (Session["COMPANY"].ToString() == "IT")
                        {
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string PONumber = (dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == null || dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == "" || dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("PO_NUMBER").Trim();
                            string PODate = (dtAssetDet.Rows[0].Field<string>("PO_DATE") == null || dtAssetDet.Rows[0].Field<string>("PO_DATE") == "" || dtAssetDet.Rows[0].Field<string>("PO_DATE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("PO_DATE").Trim();
                            string InvoiceNo = (dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == null || dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == "" || dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("INVOICE_NO").Trim();
                            string InvoiceDate = (dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == null || dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == "" || dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("INVOICE_DATE").Trim();

                            dtITIUTLocation.Rows.Add(oPRP.GatePassCode, oPRP.AssetCode, oPRP.GatePassDate, oPRP.DocumentNo,oPRP.AssetLocation,oPRP.DestinationLocation, AssetType, AssetMake, AssetModel, oPRP.SerialCode,PONumber,PODate,InvoiceNo,InvoiceDate);
                            dtITIUTLocation.AcceptChanges();
                        }
                        else
                        {
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string PONumber = (dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == null || dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == "" || dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("PO_NUMBER").Trim();
                            string PODate = (dtAssetDet.Rows[0].Field<string>("PO_DATE") == null || dtAssetDet.Rows[0].Field<string>("PO_DATE") == "" || dtAssetDet.Rows[0].Field<string>("PO_DATE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("PO_DATE").Trim();
                            string InvoiceNo = (dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == null || dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == "" || dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("INVOICE_NO").Trim();
                            string InvoiceDate = (dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == null || dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == "" || dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("INVOICE_DATE").Trim();

                            dtFacilitiesIUTLocation.Rows.Add(oPRP.GatePassCode, oPRP.AssetCode, oPRP.GatePassDate, oPRP.DocumentNo, oPRP.AssetLocation, oPRP.DestinationLocation, AssetType, AssetMake, AssetModel, oPRP.ASSET_FAR_TAG, PONumber, PODate, InvoiceNo, InvoiceDate);
                            dtFacilitiesIUTLocation.AcceptChanges();
                        }
                    }
                }
            }
            if (!bRsp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Gate Pass details not saved successfully.);", true);
            }
            else
            {
                DataTable dp = oDAL.GetMailTransactionDetails("GATEPASS_GENERATION", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
                {
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        if (Session["COMPANY"].ToString() == "IT")
                            sendmail.FunctionSendingMailWithAssetData(dtITIUTLocation, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        else
                            sendmail.FunctionSendingMailWithAssetData(dtFacilitiesIUTLocation, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    }
                    catch (Exception ee)
                    {

                    }
                }
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                lblErrorMsg.Text = "Please Note : New GatePass Generated With GatePass No. As : " + oPRP.GatePassCode;
                txtGPRemarks.Text = string.Empty;
                txtDocumentNo.Text = string.Empty;
                gvAssets.DataSource = null;
                gvAssets.DataBind();
                txtGatePassNo.Text = oPRP.GatePassCode;
                btnPrintGatePass.Visible = true;
                upSubmit.Update();
                //try
                //{ MailGatepassDetails(oPRP.GatePassCode); }
                //catch (Exception ex) { }
            }
            bTransOpen = true;
            if (bTransOpen)
            {
                //oDAL.Transaction("COMMIT");
                bTransOpen = false;
            }
            //PopulateAssetDetails();
        }
        catch (Exception ex)
        {
            //oDAL.Transaction("ROLLBACK");
            bTransOpen = false;
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Get Assets based on asset code, serial code, asset make, model name and location code criterian.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    //PopulateAssetDetails();
        //}
        //catch (Exception ex)
        //{
        //    HandleExceptions(ex);
        //}
        if(ddlSiteLocationFilter.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please enter Site Location');", true);
            return;
        }

        try
        {
            string AssetMake = txtAssetMake.Text.Trim() != "" ? txtAssetMake.Text.Trim() : null;
            string AssetModel = txtAssetModel.Text.Trim() != "" ? txtAssetModel.Text.Trim() : null;
            string SerialNo = txtAssetSerialNo.Text.Trim() != "" ? txtAssetSerialNo.Text.Trim() : null;
            string AssetType = txtAssetType.Text.Trim() != "" ? txtAssetType.Text.Trim() : null;
            string SiteLocation = ddlSiteLocationFilter.SelectedValue != "" ? ddlSiteLocationFilter.SelectedValue : null;
            string FilterStatus = txtAllocationStatus.Text.Trim() != "" ? txtAllocationStatus.Text.Trim() : null;
            string filterSubStatus = txtAllocationSubStatus.Text.Trim() != "" ? txtAllocationSubStatus.Text.Trim() : null;
            string FilterAssetFarTag = txtAssetFarTag.Text.Trim() != "" ? txtAssetFarTag.Text.Trim() : null;
            string FilterAssetDomain = txtAssetDomain.Text.Trim() != "" ? txtAssetDomain.Text.Trim() : null;
            string FilterAssetHDD = txtAssetHDD.Text.Trim() != "" ? txtAssetHDD.Text.Trim() : null;
            string FilterAssetProcessor = txtAssetProcessor.Text.Trim() != "" ? txtAssetProcessor.Text.Trim() : null;
            string FilterAssetRAM = txtAssetRAM.Text.Trim() != "" ? txtAssetRAM.Text.Trim() : null;
            string FilterAssetPONumber = txtPONumber.Text.Trim() != "" ? txtPONumber.Text.Trim() : null;
            string FilterAssetVendor = txtAssetVendor.Text.Trim() != "" ? txtAssetVendor.Text.Trim() : null;
            string FilterAssetInvoiceNumber = txtAssetInvoiceNo.Text.Trim() != "" ? txtAssetInvoiceNo.Text.Trim() : null;
            string FilterAssetRFIDTag = txtRFIDTag.Text.Trim() != "" ? txtRFIDTag.Text.Trim() : null;
            string FilterAssetGRNNo = txtGRNNumber.Text.Trim() != "" ? txtGRNNumber.Text.Trim() : null;
            oPRP.CompCode = Session["COMPANY"].ToString();
            DataTable dt = new DataTable();
            dt = oDAL.GetAllSearchAssetDetails(AssetMake, AssetModel, AssetType, SiteLocation, SerialNo, "LocationToLocation", oPRP.CompCode, FilterStatus, filterSubStatus, FilterAssetFarTag, FilterAssetDomain, FilterAssetHDD, FilterAssetProcessor, FilterAssetRAM, FilterAssetPONumber, FilterAssetVendor, FilterAssetInvoiceNumber, FilterAssetRFIDTag, FilterAssetGRNNo);
            //dt = oDAL.GetAllSearchAssetDetails(AssetMake, AssetModel, AssetType, SiteLocation, SerialNo, "STOCK", oPRP.CompCode);
            if (dt.Rows.Count > 0)
            {
                gvAssets.DataSource = dt;
                gvAssets.DataBind();
                gvAssets.Visible = true;
                btnSubmit.Enabled = true;
                btnSubmit.Visible = true;
                if (Session["COMPANY"].ToString() == "IT")
                {
                    gvAssets.Columns[5].Visible = false;
                }
                else
                {
                    gvAssets.Columns[4].Visible = false;
                }
                lblAssetCount.Text = "Assets Count : " + dt.Rows.Count.ToString();
            }
            else
            {
                gvAssets.DataSource = null;
                gvAssets.DataBind();
                gvAssets.Visible = false;
                btnSubmit.Enabled = false;
                btnSubmit.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Invalid search filters or assets are assigned.');", true);
                return;
            }

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Reset/clear page fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //gvAssets.DataSource = null;
            //gvAssets.DataBind();

            ////PopulateCategory("");
            //DataTable dtNull = new DataTable();
            ////ddlAssetMake.DataSource = dtNull;
            ////ddlAssetMake.DataBind();
            ////lstModelName.DataSource = dtNull;
            ////lstModelName.DataBind();
            //dtNull = null;
            //txtGatePassNo.Text = "";
            //txtDocumentNo.Text = "";
            //txtReturnableDate.Text = "";
            //txtGatePassNo.Text = "";
            ////lblLocCode.Text = "0";
            //lblAssetCount.Text = "Assets Count : 0";
            ////ddlGPLocation.DataSource = null;
            //DataTable dt = oDAL.GetLocation(Session["COMPANY"].ToString(), "", 1);
            ////ddlGPLocation.DataSource = dt;
            //////ddlGPLocation.DataTextField = "LOC_NAME";
            ////ddlGPLocation.DataTextField = "LOC_CODE";
            ////ddlGPLocation.DataValueField = "LOC_CODE";
            ////ddlGPLocation.DataBind();
            ////ddlGPLocation.Items.Insert(0, "-- Select Location --");
            //ddlDestLocation.DataSource = null;
            //ddlDestLocation.DataSource = dt;
            ////ddlDestLocation.DataTextField = "LOC_NAME";
            //ddlDestLocation.DataTextField = "LOC_CODE";
            //ddlDestLocation.DataValueField = "LOC_CODE";
            //ddlDestLocation.DataBind();
            //ddlDestLocation.Items.Insert(0, "-- Select Location --");
            //ddlGatePassForName.DataSource = null;
            //btnPrintGatePass.Enabled = false;
            //if (bTransOpen)
            //{
            //    oDAL.Transaction("ROLLBACK", Session["DATABASE"].ToString());
            //    bTransOpen = false;
            //}
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Refresh/reset location details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnRefreshLocation_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        //lblLocLevel.Text = "1";
    //        PopulateLocation();
    //    }
    //    catch (Exception ex)
    //    {
    //        HandleExceptions(ex);
    //    }
    //}

    /// <summary>
    /// Navigate to print gate pass page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrintGatePass_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtGatePassNo.Text != "")
            {
                string URL = "PrintGatePass.aspx?GPNO=" + txtGatePassNo.Text.Trim() + "&APPROVE_STATUS=" + clsGeneral.gStrApproveStatus.ToString();
                string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=832,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no');";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Gate Pass Code cannot be blank.);", true);
                return;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    protected void btnRePrintGatePass_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtGatePassNoVerify.Text != "")
            {
                string URL = "PrintGatePass.aspx?GPNO=" + txtGatePassNoVerify.Text.Trim() + "&APPROVE_STATUS=" + clsGeneral.gStrApproveStatus.ToString();
                string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=832,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no');";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('GatePass No Cannot be Blank.');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    protected void txtGatePassNoVerify_TextChanged(object sender, EventArgs e)
    {
        string GatePassNo = txtGatePassNoVerify.Text;
        if (oDAL.isGatePassNoExist(GatePassNo, Session["COMPANY"].ToString()))
            txtGatePassNoVerify.Enabled = false;
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Invalid Gate Pass No.');", true);
            txtGatePassNoVerify.Text = string.Empty;
            return;
        }
    }
    #endregion

    #region SELECTEDINDEXCHANGED EVENTS
    /// <summary>
    /// Fetch list of categories based on asset type selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void ddlAssetMake_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlAssetMake.SelectedIndex != 0)
    //            PopulateModelName(ddlAssetMake.SelectedValue.ToString(), lblCatCode.Text.Trim());
    //    }
    //    catch (Exception ex)
    //    { HandleExceptions(ex); }
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlAssetCategory.SelectedIndex > 0)
    //        {
    //            DataTable dtNull = new DataTable();
    //            lstModelName.DataSource = dtNull;
    //            lstModelName.DataBind();
    //            dtNull = null;

    //            PopulateAssetMake(ddlAssetCategory.SelectedValue.ToString());
    //            int CatLevel = int.Parse(lblCatLevel.Text.Trim());
    //            lblCatLevel.Text = (CatLevel + 1).ToString();
    //            int iCatLevel = int.Parse(lblCatLevel.Text.Trim());
    //            string sCatCode = ddlAssetCategory.SelectedValue.ToString();
    //            lblCatCode.Text = sCatCode;

    //            //ddlAssetCategory.DataSource = null;
    //            //DataTable dt = oDAL.PopulateCategory("", sCatCode, iCatLevel);
    //            //if (dt.Rows.Count > 0)
    //            //{
    //            //    ddlAssetCategory.DataSource = dt;
    //            //    ddlAssetCategory.DataValueField = "CATEGORY_CODE";
    //            //    ddlAssetCategory.DataTextField = "CATEGORY_NAME";
    //            //    ddlAssetCategory.DataBind();
    //            //    ddlAssetCategory.Items.Insert(0, "-- Select Category --");
    //            //    ddlAssetCategory.Focus();
    //            //}
    //            //else
    //            //{
    //            //    iCatLevel = iCatLevel - 1;
    //            //    lblCatLevel.Text = iCatLevel.ToString();
    //            //    ddlAssetMake.Focus();
    //            //}
    //        }
    //    }
    //    catch (Exception ex)
    //    { HandleExceptions(ex); }
    //}

    /// <summary>
    /// Get location code/name based on parent location name selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void ddlGPLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlGPLocation.SelectedIndex != 0)
    //        {
    //            int locLevel = int.Parse(lblLocLevel.Text.Trim());
    //            lblLocLevel.Text = (locLevel + 1).ToString();
    //            int iLocLevel = int.Parse(lblLocLevel.Text.Trim());
    //            string sLocCode = ddlGPLocation.SelectedValue.ToString();
    //            lblLocCode.Text = sLocCode;

    //            //ddlGPLocation.DataSource = null;
    //            //DataTable dt = oDAL.GetLocation(Session["COMPANY"].ToString(), sLocCode, iLocLevel);
    //            //if (dt.Rows.Count > 0)
    //            //{
    //            //    ddlGPLocation.DataSource = dt;
    //            //    ddlGPLocation.DataValueField = "LOC_CODE";
    //            //    ddlGPLocation.DataTextField = "LOC_NAME";
    //            //    ddlGPLocation.DataBind();
    //            //    ddlGPLocation.Items.Insert(0, "-- Select Location --");
    //            //    ddlGPLocation.Focus();
    //            //}
    //            //else
    //            //{
    //            //    iLocLevel = iLocLevel - 1;
    //            //    lblLocLevel.Text = iLocLevel.ToString();
    //            //    ddlGatePassFor.Focus();
    //            //}
    //        }
    //    }
    //    catch (Exception ex)
    //    { HandleExceptions(ex); }
    //}

    /// <summary>
    /// Get employee/vendor code/name based on gate pass for selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlGatePassFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlGatePassFor.SelectedIndex != 0)
            {
                if (ddlGatePassFor.SelectedValue.ToString() == "EMPLOYEE")
                {
                    ddlGatePassForName.DataSource = null;
                    DataTable dt = new DataTable();
                    dt = oDAL.GetEmployee(Session["COMPANY"].ToString());
                    ddlGatePassForName.DataSource = dt;
                    ddlGatePassForName.DataTextField = "EMPLOYEE_NAME";
                    ddlGatePassForName.DataValueField = "EMPLOYEE_CODE";
                    ddlGatePassForName.DataBind();
                    ddlGatePassForName.Items.Insert(0, "-- Select Employee --");
                }
                else if (ddlGatePassFor.SelectedValue.ToString() == "VENDOR")
                {
                    ddlGatePassForName.DataSource = null;
                    DataTable dt = new DataTable();
                    dt = oDAL.GetVendor(Session["COMPANY"].ToString());
                    ddlGatePassForName.DataSource = dt;
                    ddlGatePassForName.DataTextField = "VENDOR_NAME";
                    ddlGatePassForName.DataValueField = "VENDOR_CODE";
                    ddlGatePassForName.DataBind();
                    ddlGatePassForName.Items.Insert(0, "-- Select Vendor --");
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// 
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
    /// Asset details gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssets.PageIndex = e.NewPageIndex;
            //PopulateAssetDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlDestLocation.SelectedIndex != 0)
            {
                string sLocCode = ddlDestLocation.SelectedValue.ToString();
                lblDestCode.Text = sLocCode;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }


    #region File Upload
    private Tuple<bool, string, List<GatePassGeneration_PRP>> ValidateFile(DataTable dt)
    {
        bool res = true;
        string msg = "success";
        string[] ReturnType = new string[] { "RETURNABLE", "NON RETURNABLE" };
        int i = 0;
        List<GatePassGeneration_PRP> oPRPList = new List<GatePassGeneration_PRP>();
        foreach (DataRow dr in dt.Rows)
        {
            GatePassGeneration_PRP oPRP = new GatePassGeneration_PRP();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.DocumentNo = (dr.Field<string>("DOCUMENT_NO") == null || dr.Field<string>("DOCUMENT_NO") == string.Empty || dr.Field<string>("DOCUMENT_NO") == "") ? string.Empty : dr.Field<string>("DOCUMENT_NO").Trim();
            oPRP.DestinationLocation = (dr.Field<string>("DESTINATION_LOCATION") == null || dr.Field<string>("DESTINATION_LOCATION") == string.Empty || dr.Field<string>("DESTINATION_LOCATION") == "") ? string.Empty : dr.Field<string>("DESTINATION_LOCATION").Trim().ToUpper();
            oPRP.GatePassType = (dr.Field<string>("GATEPASS_TYPE") == null || dr.Field<string>("GATEPASS_TYPE") == string.Empty || dr.Field<string>("GATEPASS_TYPE") == "") ? string.Empty : dr.Field<string>("GATEPASS_TYPE").Trim().ToUpper();

            if (dr.Field<string>("GATEPASS_DATE") == null || dr.Field<string>("GATEPASS_DATE") == "" || dr.Field<string>("GATEPASS_DATE") == string.Empty)
                oPRP.GatePassDate = string.Empty;
            else
                oPRP.GatePassDate = dr.Field<string>("GATEPASS_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim();

            oPRP.Remarks = dr.Field<string>("GATEPASS_REMARKS").Trim();

            if (oPRP.DocumentNo.Trim() == "" || string.IsNullOrEmpty(oPRP.DocumentNo) ||
                oPRP.DestinationLocation.Trim() == "" || string.IsNullOrEmpty(oPRP.DestinationLocation) ||
                oPRP.GatePassType.Trim() == "" || string.IsNullOrEmpty(oPRP.GatePassType) ||
                oPRP.GatePassDate.Trim() == "" || string.IsNullOrEmpty(oPRP.GatePassDate)
                )
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter the mandatory fields.";
                res = false;
                break;
            }
            if(Session["COMPANY"].ToString()=="IT")
            {
                if (dr.Field<string>("SERIAL_NUMBER").Trim() == "" || dr.Field<string>("SERIAL_NUMBER").Trim() == null || string.IsNullOrEmpty(dr.Field<string>("SERIAL_NUMBER")))
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter the mandatory fields.";
                    res = false;
                    break;
                }
            }
            else
            {
                if (dr.Field<string>("ASSET_FAR_TAG").Trim() == "" || dr.Field<string>("ASSET_FAR_TAG").Trim() == null || string.IsNullOrEmpty(dr.Field<string>("ASSET_FAR_TAG")))
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter the mandatory fields.";
                    res = false;
                    break;
                }
            }

            if (!ReturnType.Contains(oPRP.GatePassType.ToUpper()))
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Gatepass Type.";
                res = false;
                break;
            }
            
            var dtDLocation = oDAL.GetLocation(oPRP.DestinationLocation,Session["COMPANY"].ToString());
            if (dtDLocation.Rows.Count <= 0)
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + " Destination location is invalid.";
                res = false;
                break;
            }

            oPRP.Approve_GatePass = true;

            if (!String.IsNullOrEmpty(oPRP.GatePassDate ))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.GatePassDate, out date))
                {
                    oPRP.GatePassDate = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid Gatepass date.";
                    res = false;
                    break;
                }
            }
            if(Session["COMPANY"].ToString() == "IT")
            { 
                oPRP.SerialCode = (dr.Field<string>("SERIAL_NUMBER") == null || dr.Field<string>("SERIAL_NUMBER") == string.Empty || dr.Field<string>("SERIAL_NUMBER") == "") ? string.Empty : dr.Field<string>("SERIAL_NUMBER").Trim();
                oPRP.ASSET_FAR_TAG = string.Empty;
            }
            else
            {
                oPRP.SerialCode = string.Empty;
                oPRP.ASSET_FAR_TAG = (dr.Field<string>("ASSET_FAR_TAG") == null || dr.Field<string>("ASSET_FAR_TAG") == string.Empty || dr.Field<string>("ASSET_FAR_TAG") == "") ? string.Empty : dr.Field<string>("ASSET_FAR_TAG").Trim();
            }

            oPRP.AssetCode = oDAL.GetAssetCode(Session["COMPANY"].ToString() == "IT" ? dr.Field<string>("SERIAL_NUMBER"): dr.Field<string>("ASSET_FAR_TAG"), Session["COMPANY"].ToString());
            if (string.IsNullOrEmpty(oPRP.AssetCode) || oPRP.AssetCode == "")
            {
                if(Session["COMPANY"].ToString() == "IT")
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Serial Code.";
                else
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Far Tag.";
                //msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Far Tag.";
                res = false;
                break;
            }
            oPRP.AssetTag = oDAL.GetRFIDTag(Session["COMPANY"].ToString() == "IT" ? dr.Field<string>("SERIAL_NUMBER") : dr.Field<string>("ASSET_FAR_TAG"), Session["COMPANY"].ToString());

            oPRP.AssetLocation = oDAL.GetLocationwithAssetCode(oPRP.AssetCode).ToUpper();
            if (oPRP.AssetLocation.ToUpper() == oPRP.DestinationLocation.ToUpper())
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + " Source and destination location should not be same.";
                res = false;
                break;
            }

            string _LiveGPCode = oDAL.ChkLiveGP(oPRP.AssetCode);
            if (_LiveGPCode != "")
            {
                lblErrorMsg.Text = "Selected asset is already part of Gate Pass No: " + _LiveGPCode + " and is not returned yet.";
                res = false;
                break;
            }

            if (oPRP.GatePassType.ToUpper() == "RETURNABLE")
            {
                if (dr.Field<string>("EXP_RETURN_DATE") == null || dr.Field<string>("EXP_RETURN_DATE") == "" || dr.Field<string>("EXP_RETURN_DATE") == string.Empty)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " if Gate Pass type is Returnable then Please enter Exp Return date.";
                    res = false;
                    break;
                }
                else
                    oPRP.ExpReturnDate = dr.Field<string>("EXP_RETURN_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim();

                if (!String.IsNullOrEmpty(oPRP.ExpReturnDate))
                {
                    string date = "";
                    if (ConvertToExcel.isValidateDate(oPRP.ExpReturnDate, out date))
                    {
                        oPRP.ExpReturnDate = date;
                        if (Convert.ToDateTime(oPRP.ExpReturnDate) < Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy")))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Expected return Date should be greater than current date.');", true);
                            msg = "Please Note : Row Number " + (i+1).ToString() + " Expected return Date should be greater than current date.";
                            res = false;
                            break;
                        }
                    }
                    else
                    {
                        msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid Exp Return date.";
                        res = false;
                        break;
                    }
                }
            }
            else
                oPRP.ExpReturnDate = string.Empty;

            oPRPList.Add(oPRP);
        }

        if (oPRPList.Count > 0)
        {
            int data = oPRPList.GroupBy(s => new { s.DocumentNo, s.GatePassType, s.ExpReturnDate, s.AssetLocation, s.DestinationLocation, s.Remarks }).Select(d => new { d }).Count();
            if (data > 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please enter unique gatepass records.');", true);
                msg = "Please Note : Enter unique gatepass records .";
                res = false;
            }
        }

        if (!res)
            oPRPList = new List<GatePassGeneration_PRP>();

        return new Tuple<bool, string, List<GatePassGeneration_PRP>>(res, msg, oPRPList);
    }

    #endregion


    protected void btnUploadFile_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtITIUTLocation = new DataTable();
            dtITIUTLocation.Columns.Add("GatePass Code");
            dtITIUTLocation.Columns.Add("Asset Code");
            dtITIUTLocation.Columns.Add("GatePass Date");
            dtITIUTLocation.Columns.Add("Document No");
            dtITIUTLocation.Columns.Add("Source Location");
            dtITIUTLocation.Columns.Add("Destination Location");
            dtITIUTLocation.Columns.Add("Asset Type");
            dtITIUTLocation.Columns.Add("Asset Make");
            dtITIUTLocation.Columns.Add("Asset Model");
            dtITIUTLocation.Columns.Add("Serial Number");
            dtITIUTLocation.Columns.Add("PO No");
            dtITIUTLocation.Columns.Add("PO Date");
            dtITIUTLocation.Columns.Add("Invoice No");
            dtITIUTLocation.Columns.Add("Invoice Date");

            DataTable dtFacilitiesIUTLocation = new DataTable();
            dtFacilitiesIUTLocation.Columns.Add("GatePass Code");
            dtFacilitiesIUTLocation.Columns.Add("Asset Code");
            dtFacilitiesIUTLocation.Columns.Add("GatePass Date");
            dtFacilitiesIUTLocation.Columns.Add("Document No");
            dtFacilitiesIUTLocation.Columns.Add("Source Location");
            dtFacilitiesIUTLocation.Columns.Add("Destination Location");
            dtFacilitiesIUTLocation.Columns.Add("Asset Type");
            dtFacilitiesIUTLocation.Columns.Add("Asset Make");
            dtFacilitiesIUTLocation.Columns.Add("Asset Model");
            dtFacilitiesIUTLocation.Columns.Add("Asset Far Tag");
            dtFacilitiesIUTLocation.Columns.Add("PO No");
            dtFacilitiesIUTLocation.Columns.Add("PO Date");
            dtFacilitiesIUTLocation.Columns.Add("Invoice No");
            dtFacilitiesIUTLocation.Columns.Add("Invoice Date");

            Session["ERRORDATA"] = null;
            DataTable dtInvalidData = new DataTable();
            DataRow dataRow;
            ConvertToExcel convertToExcel = new ConvertToExcel();
            string GatepassNo = string.Empty;

            var response = convertToExcel.ValidateFileReaded(fuBulkUpload, Session["COMPANY"].ToString() == "IT"?"ITTRANSFER":"FACILITIESTRANSFER");
            if (response.Item1)
            {
                DataTable dt = response.Item2;
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('No Data in the file');", true);
                    return;
                }
                dtInvalidData = dt.Clone();
                dtInvalidData.Columns.Add("STATUS");
                dtInvalidData.Columns.Add("ERROR_MESSAGE");
                var validate = ValidateFile(dt);
                if (validate.Item1)
                {
                    int Cnt = 0;int i = 0;int j = 0; bool isSave = false;
                    int _iMaxACQId = oDAL.GetMaxRunningSrlNo(Session["COMPANY"].ToString());
                    foreach (var s in validate.Item3)
                    {
                        GatepassNo =  s.GatePassCode = s.CompCode + "-" + "GP" + "-" + _iMaxACQId.ToString().PadLeft(6, '0');
                        s.Running_Serial_No = _iMaxACQId;
                        if (i == 0)
                        {
                           isSave = oDAL.SaveGatePassDetails(s);
                        }
                        if (isSave && oDAL.SaveGatePassAssets(s))
                        {
                            if (Session["COMPANY"].ToString() == "IT")
                            {
                                DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(s.AssetCode, Session["COMPANY"].ToString());
                                string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                                string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                                string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                                string PONumber = (dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == null || dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == "" || dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("PO_NUMBER").Trim();
                                string PODate = (dtAssetDet.Rows[0].Field<string>("PO_DATE") == null || dtAssetDet.Rows[0].Field<string>("PO_DATE") == "" || dtAssetDet.Rows[0].Field<string>("PO_DATE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("PO_DATE").Trim();
                                string InvoiceNo = (dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == null || dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == "" || dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("INVOICE_NO").Trim();
                                string InvoiceDate = (dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == null || dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == "" || dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("INVOICE_DATE").Trim();

                                dtITIUTLocation.Rows.Add(GatepassNo, s.AssetCode, s.GatePassDate, s.DocumentNo, s.AssetLocation, s.DestinationLocation, AssetType, AssetMake, AssetModel, s.SerialCode, PONumber, PODate, InvoiceNo, InvoiceDate);
                                dtITIUTLocation.AcceptChanges();
                            }
                            else
                            {
                                DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(s.AssetCode, Session["COMPANY"].ToString());
                                string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                                string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                                string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                                string PONumber = (dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == null || dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == "" || dtAssetDet.Rows[0].Field<string>("PO_NUMBER") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("PO_NUMBER").Trim();
                                string PODate = (dtAssetDet.Rows[0].Field<string>("PO_DATE") == null || dtAssetDet.Rows[0].Field<string>("PO_DATE") == "" || dtAssetDet.Rows[0].Field<string>("PO_DATE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("PO_DATE").Trim();
                                string InvoiceNo = (dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == null || dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == "" || dtAssetDet.Rows[0].Field<string>("INVOICE_NO") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("INVOICE_NO").Trim();
                                string InvoiceDate = (dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == null || dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == "" || dtAssetDet.Rows[0].Field<string>("INVOICE_DATE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("INVOICE_DATE").Trim();

                                dtFacilitiesIUTLocation.Rows.Add(GatepassNo, s.AssetCode, s.GatePassDate, s.DocumentNo, s.AssetLocation, s.DestinationLocation, AssetType, AssetMake, AssetModel, s.ASSET_FAR_TAG, PONumber, PODate, InvoiceNo, InvoiceDate);
                                dtFacilitiesIUTLocation.AcceptChanges();
                            }
                            //if (Session["COMPANY"].ToString() == "IT")
                            //{
                            //    dtITIUTLocationUpload.Rows.Add(GatepassNo,s.AssetCode, s.SerialCode);
                            //    dtITIUTLocationUpload.AcceptChanges();
                            //}
                            //else
                            //{
                            //    dtFacilitiesIUTLocationUpload.Rows.Add(GatepassNo,s.AssetCode, s.ASSET_FAR_TAG);
                            //    dtFacilitiesIUTLocationUpload.AcceptChanges();
                            //}
                            Cnt++;
                            //continue;
                        }
                        else
                        {
                            if(Session["COMPANY"].ToString()=="IT")
                            {
                                dataRow = dtInvalidData.NewRow();
                                dataRow["DOCUMENT_NO"] = s.DocumentNo;
                                dataRow["GATEPASS_DATE"] = updatedatevalue(s.GatePassDate);
                                dataRow["GATEPASS_TYPE"] = s.GatePassType;
                                dataRow["EXP_RETURN_DATE"] = updatedatevalue(s.ExpReturnDate);
                                dataRow["DESTINATION_LOCATION"] = s.DestinationLocation;
                                dataRow["GATEPASS_REMARKS"] = s.Remarks;
                                dataRow["SERIAL_NUMBER"] = s.SerialCode;
                                dataRow["STATUS"] = "ERROR";
                                dataRow["ERROR_MESSAGE"] = "Duplicate/Data is inproper, Please Check";
                                dtInvalidData.Rows.Add(dataRow);
                                Session["ERRORDATA"] = dtInvalidData;
                                j++;
                            }
                            else
                            {
                                dataRow = dtInvalidData.NewRow();
                                dataRow["DOCUMENT_NO"] = s.DocumentNo;
                                dataRow["GATEPASS_DATE"] = updatedatevalue(s.GatePassDate);
                                dataRow["GATEPASS_TYPE"] = s.GatePassType;
                                dataRow["EXP_RETURN_DATE"] = updatedatevalue(s.ExpReturnDate);
                                dataRow["DESTINATION_LOCATION"] = s.DestinationLocation;
                                dataRow["GATEPASS_REMARKS"] = s.Remarks;
                                dataRow["ASSET_FAR_TAG"] = s.ASSET_FAR_TAG;
                                dataRow["STATUS"] = "ERROR";
                                dataRow["ERROR_MESSAGE"] = "Duplicate/Data is inproper, Please Check";
                                dtInvalidData.Rows.Add(dataRow);
                                Session["ERRORDATA"] = dtInvalidData;
                                j++;
                            }
                            
                        }
                        i++;
                    }
                    if(Cnt > 0)
                    {
                        txtGatePassNo.Text = GatepassNo;
                        DataTable dp = oDAL.GetMailTransactionDetails("GATEPASS_GENERATION", Convert.ToString(Session["COMP_NAME"]));
                        if (dp.Rows.Count > 0)
                        {
                            try
                            {
                                SendmailAlert sendmail = new SendmailAlert();
                                if (Session["COMPANY"].ToString() == "IT")
                                    sendmail.FunctionSendingMailWithAssetData(dtITIUTLocation, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                                else
                                    sendmail.FunctionSendingMailWithAssetData(dtFacilitiesIUTLocation, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));

                                //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                            }
                            catch (Exception ee)
                            {

                            }
                        }
                    }
                    if (j == 0)
                    {
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                        txtGatePassNo.Text = GatepassNo;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                    }
                    else
                    {
                        btnExport.Visible = true;
                        btnExport.Enabled = true;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('Some Of The Rows Are Not Inserted Please Download The Report To Find The Error');", true);
                    }
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('" + validate.Item2 + "');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('" + response.Item3 + "');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    private string updatedatevalue(string datevalue)
    {
        if (datevalue == null || datevalue == string.Empty || datevalue == "")
            datevalue = string.Empty;
        else
            datevalue = datevalue.Replace('/', '-');

        return datevalue;
    }
    protected void ddlGatePassType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlGatePassType.SelectedIndex > 0)
        {
            if(ddlGatePassType.SelectedValue == "RETURNABLE")
            {
                txtReturnableDate.Enabled = true;
            }
            else
            {
                txtReturnableDate.Enabled = false;
            }
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //lblLocLevel.Text = "1";
            PopulateLocation();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataTable dt1 = new DataTable();
            if (Session["ERRORDATA"] != null)
            {
                dt1 = (DataTable)Session["ERRORDATA"];
            }
            if (dt1.Rows.Count > 0)
            {
                //Response.Clear();

                dt1.TableName = "ErrorData";
                using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                {
                    wb.Worksheets.Add(dt1);
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
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no data for being exported.');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
}