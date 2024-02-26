using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.Windows.Forms;
using System.IO;

public partial class AssetReplacement : System.Web.UI.Page
{
    AssetReplacement_DAL oDAL;
    AssetReplacement_PRP oPRP;
    public AssetReplacement()
    {
        oPRP = new AssetReplacement_PRP();
    }
    ~AssetReplacement()
    {
        oPRP = null; oDAL = null;
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
        oDAL = new AssetReplacement_DAL(Session["DATABASE"].ToString());
        txtActiveInAssetCode.Focus();
    }

    /// <summary>
    /// Checking user group rights for Asset replacement operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
        ScriptManager.GetCurrent(this).RegisterPostBackControl(this.btnSubmit);

        this.Page.Form.Attributes.Add("enctype", "multipart/form-data");
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("ASSET_REPLACEMENT", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_REPLACEMENT");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                if (Session["GROUP"].ToString().ToUpper() == "SYSADMIN")
                {
                    txtSecurityGEDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                    txtSecurityGEDate.Enabled = true;
                }
                else
                {
                    txtSecurityGEDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                    txtSecurityGEDate.Enabled = false;
                }
               // GetAssetReplacementDetails(Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Asset Replacement");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Populate Asset replacement details into gridview for viewing.
    /// </summary>
    private void GetAssetReplacementDetails(string CompCode)
    {
        gvAssetReplacement.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetAssetReplacementDetails(CompCode);
        gvAssetReplacement.DataSource = Session["REPLACEMENT"] = dt;
        gvAssetReplacement.DataBind();
    }

    /// <summary>
    /// Send e-mail to concerned person for asset approval after replacement .
    /// </summary>
    /// <param name="ActiveInAssetCode"></param>
    /// <param name="FaultyOutSerialCode"></param>
    /// <param name="SerialCode"></param>
    private void SendMailForApproval(string ActiveInAssetCode, string FaultyOutSerialCode, string SerialCode)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();
        MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SENDER"].ToString(), "ATS");
        smtpClient.Host = ConfigurationManager.AppSettings["SMTP_HOST"].ToString();
        smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
        message.From = fromAddress;
        message.To.Add(Session["EMAIL"].ToString());
        message.Subject = "BCIL : ATS - Approval For Asset Replacement";
        message.IsBodyHtml = false;
        StringBuilder sbMsg = new StringBuilder();
        sbMsg.AppendLine("Please Note,");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("An asset has been replaced with asset code as : " + ActiveInAssetCode + ".");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Asset faulty out serial no. : " + FaultyOutSerialCode + ".");
        sbMsg.AppendLine("");
        sbMsg.AppendLine("Asset active in serial no. : " + SerialCode + ".");
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

    #region SUBMIT EVENT

    /// <summary>
    /// Save Asset replacement details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtemailforIt = new DataTable();
            dtemailforIt.Columns.Add("Asset Code");
            dtemailforIt.Columns.Add("Site Location");
            dtemailforIt.Columns.Add("Replacement Date");
            dtemailforIt.Columns.Add("Asset Type");
            dtemailforIt.Columns.Add("Asset Make");
            dtemailforIt.Columns.Add("Asset Model");
            dtemailforIt.Columns.Add("Old Serial No");
            dtemailforIt.Columns.Add("New Serial No");
            dtemailforIt.Columns.Add("Replaced By");

            DataTable dtemailforFacilities = new DataTable();
            dtemailforFacilities.Columns.Add("Asset Code");
            dtemailforFacilities.Columns.Add("Site Location");
            dtemailforFacilities.Columns.Add("Replacement Date");
            dtemailforFacilities.Columns.Add("Asset Type");
            dtemailforFacilities.Columns.Add("Asset Make");
            dtemailforFacilities.Columns.Add("Asset Model");
            dtemailforFacilities.Columns.Add("Old Asset Far Tag");
            dtemailforFacilities.Columns.Add("New Asset Far Tag");
            dtemailforFacilities.Columns.Add("Replaced By");
            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Asset Replacement", "Asset Replacement", "Asset  Replacement done by user id" + Session["CURRENTUSER"].ToString() + " Asset Serial No" + txtActiveInAssetCode.Text);
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            string CreatedByName = oDAL.GetReceivedBy(Session["CURRENTUSER"].ToString());

            bool bChk = false;
            
            if (oPRP.CompCode == "IT")
            {
                if(txtActiveInAssetCode.Text.Trim()!="")
                { 
                    if (txtSerialCode.Text.Trim() != "")
                    {
                        bChk = oDAL.ChkActiveSerialNoExists(txtSerialCode.Text.Trim(), Session["COMPANY"].ToString());
                        if (bChk)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Asset serial code already exists in asset acquisition.');", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please Enter the New Serial code.');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please enter the Old Serial Number.');", true);
                    return;
                }
                oPRP.AssetCode = oDAL.GetAssetCode(txtActiveInAssetCode.Text,oPRP.CompCode);
                oPRP.AssetTag = oDAL.GetRFIDTag(txtActiveInAssetCode.Text, oPRP.CompCode);
                oPRP.FaultyOutSerialCode = txtActiveInAssetCode.Text.Trim();
                oPRP.ActiveInAssetCode = txtSerialCode.Text.Trim();
                oPRP.FaultyOutAssetFarTagOld = null;
                oPRP.Asset_FAR_TAG = null;
                if (gvAssets.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please Note : No Assets Found with the Given old Serial Number !');", true);
                    txtRemarks.Focus();
                    return;
                }
                DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                string AssetLocation = (dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == null || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION").Trim();
                dtemailforIt.Rows.Add(oPRP.AssetCode, AssetLocation, txtSecurityGEDate.Text, AssetType, AssetMake, AssetModel, oPRP.FaultyOutSerialCode, oPRP.ActiveInAssetCode,CreatedByName);
                dtemailforIt.AcceptChanges();
            }
            else
            {
                if (txtAssetFARTagOld.Text.Trim() != "")
                {
                    if (txtAssetFarTagNew.Text.Trim() != "")
                    {
                        bChk = oDAL.ChkActiveAssetFarTagExists(txtAssetFarTagNew.Text.Trim(), Session["COMPANY"].ToString());
                        if (bChk)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Asset Far Tag already exists in asset acquisition.');", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please Enter the New Asset Far Tag.');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please enter the Old Asset Far Tag.');", true);
                    return;
                }
                oPRP.AssetCode = oDAL.GetAssetCode(txtAssetFARTagOld.Text, oPRP.CompCode);
                oPRP.AssetTag = oDAL.GetRFIDTag(txtAssetFARTagOld.Text, oPRP.CompCode);
                oPRP.FaultyOutSerialCode = null;
                oPRP.ActiveInAssetCode = null;
                oPRP.FaultyOutAssetFarTagOld = txtAssetFARTagOld.Text.Trim();
                oPRP.Asset_FAR_TAG = txtAssetFarTagNew.Text.Trim();
                if (gvAssets.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please Note : No Assets Found with the Given old Asset Far Tag Number !');", true);
                    txtRemarks.Focus();
                    return;
                }

                DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                string AssetLocation = (dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == null || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION").Trim();
                dtemailforFacilities.Rows.Add(oPRP.AssetCode, AssetLocation, txtSecurityGEDate.Text, AssetType, AssetMake, AssetModel, oPRP.FaultyOutAssetFarTagOld, oPRP.Asset_FAR_TAG, CreatedByName);
                dtemailforFacilities.AcceptChanges();
            }

            if (txtSecurityGEDate.Text.Trim() != "")
            {
                int iDate = clsGeneral.CompareDate(txtSecurityGEDate.Text.Trim(), DateTime.Now.ToString("dd/MMM/yyyy"));
                if (iDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please Note : Replacement date cannot be later than current date!');", true);
                    txtSecurityGEDate.Focus();
                    return;
                }
            }
            if (txtRemarks.Text == "" || txtRemarks.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please Note : Enter the Remarks or Reason!');", true);
                txtRemarks.Focus();
                return;
            }
           
            oPRP.DocumentNo = txtSecurityGENo.Text.Trim();
            oPRP.Replacement_Date = txtSecurityGEDate.Text.Trim();
            oPRP.ReplaceRemarks = txtRemarks.Text.Trim();
            string strFileName = AssetFileUpload.FileName;
            if (!string.IsNullOrEmpty(strFileName))
            {

                string subPath = Request.PhysicalApplicationPath + "DocumentUpload\\TransferFileUpload\\";
                if (!Directory.Exists(subPath))
                    Directory.CreateDirectory(subPath);
                string strFilePath = subPath + "\\" + AssetFileUpload.FileName;
                File.Delete(strFilePath);
                AssetFileUpload.SaveAs(strFilePath);                
                oPRP.upload = new AssetFileUpload_PRP() { FileName = strFilePath, Process = "REPLACEMENT", User = Convert.ToString(Session["CURRENTUSER"]), ID = oPRP.ActiveInAssetCode };
            }
            else
            {
                oPRP.upload = null;
            }
            
            bool bResp = oDAL.SaveUpdateAssetReplacementDetails(oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Asset Replacement details not saved.');", true);
                return;
            }
            else
            {

                DataTable dp = oDAL.GetMailTransactionDetails("ASSET_REPLACEMENT", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
                {
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        if (Session["COMPANY"].ToString() == "IT")
                            sendmail.FunctionSendingMailWithAssetData(dtemailforIt,dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        else
                            sendmail.FunctionSendingMailWithAssetData(dtemailforFacilities, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));

                        //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    }
                    catch (Exception ee)
                    {
                        
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Asset Replacement details saved successfully.');", true);
                lblErrorMsg.Text = "Asset Replacement details saved successfully.";
            }
           // GetAssetReplacementDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get asset serial no./model name/asset make based on asset code entered.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGo_Click(object sender, EventArgs e)
    {
        #region
        //try
        //{
        //    DataTable dt = new DataTable();
        //    if (txtActiveInAssetCode.Text.Trim() != "")
        //    {
        //        if (txtActiveInAssetCode.Text.Contains("|"))
        //        {

        //            dt = oDAL.GetAssetDetails(txtActiveInAssetCode.Text.Split('|')[0].Trim(), Session["COMPANY"].ToString());
        //            if (dt.Rows.Count > 0)
        //            {
        //                gvAssets.DataSource = dt;
        //                gvAssets.DataBind();
        //                gvAssets.Visible = true;
        //                btnSubmit.Enabled = true;
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Asset not found.');", true);
        //            }
        //        }
        //        else
        //        {
        //            dt = oDAL.GetAssetDetails(txtActiveInAssetCode.Text.Split('|')[0].Trim(), Session["COMPANY"].ToString());
        //            if (dt.Rows.Count > 0)
        //            {
        //                txtActiveInAssetCode.Text = Convert.ToString(dt.Rows[0]["Tag_ID"]) + '|' + Convert.ToString(dt.Rows[0]["ASSET_CODE"]);
        //                gvAssets.DataSource = dt;
        //                gvAssets.DataBind();
        //                gvAssets.Visible = true;
        //                btnSubmit.Enabled = true;
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Asset not found.');", true);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please enter Asset serial.');", true);
        //    }
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
        #endregion

        try
        {
            DataTable dt = new DataTable();
            if (txtActiveInAssetCode.Text.Trim() != "")
            {
                dt = oDAL.GetAssetDetailwithSerialNumber(txtActiveInAssetCode.Text.Trim(), Session["COMPANY"].ToString());
                if (dt.Rows.Count > 0)
                {
                    gvAssets.DataSource = dt;
                    gvAssets.DataBind();
                    gvAssets.Visible = true;
                    btnSubmit.Enabled = true;
                    if (Session["COMPANY"].ToString() == "IT")
                    {
                        gvAssets.Columns[9].Visible = false;
                    }
                    else
                    {
                        gvAssets.Columns[8].Visible = false;
                    }
                }
                else
                {
                    txtActiveInAssetCode.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Asset not found.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please enter Asset Far Tag.');", true);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export asset replacement history into excel file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[4] == "0")
            {
                Response.Redirect("UnauthorizedUser.aspx");
            }
            if (gvAssetReplacement.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["REPLACEMENT"];
                dt.TableName = "ReplacementReport";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //System.Web.UI.WebControls.DataGrid dgGrid = new System.Web.UI.WebControls.DataGrid();
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
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('There is no data to export.');", true);
                return;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Asset replacement details gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetReplacement_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssetReplacement.PageIndex = e.NewPageIndex;
           // GetAssetReplacementDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    protected void btnGoAssetFarTag_Click(object sender, ImageClickEventArgs e)
    {
        #region
        //try
        //{
        //    DataTable dt = new DataTable();
        //    if (txtAssetFARTagOld.Text.Trim() != "")
        //    {
        //        if (txtAssetFARTagOld.Text.Contains("|"))
        //        {

        //            dt = oDAL.GetAssetDetails(txtAssetFARTagOld.Text.Split('|')[0].Trim(), Session["COMPANY"].ToString());
        //            if (dt.Rows.Count > 0)
        //            {
        //                gvAssets.DataSource = dt;
        //                gvAssets.DataBind();
        //                gvAssets.Visible = true;
        //                btnSubmit.Enabled = true;
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Asset not found.');", true);
        //            }
        //        }
        //        else
        //        {
        //            dt = oDAL.GetAssetDetails(txtAssetFARTagOld.Text.Split('|')[0].Trim(), Session["COMPANY"].ToString());
        //            if (dt.Rows.Count > 0)
        //            {
        //                txtActiveInAssetCode.Text = Convert.ToString(dt.Rows[0]["Tag_ID"]) + '|' + Convert.ToString(dt.Rows[0]["ASSET_CODE"]);
        //                gvAssets.DataSource = dt;
        //                gvAssets.DataBind();
        //                gvAssets.Visible = true;
        //                btnSubmit.Enabled = true;
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Asset not found.');", true);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please enter Asset serial.');", true);
        //    }
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
        #endregion

        try
        {
            DataTable dt = new DataTable();
            if (txtAssetFARTagOld.Text.Trim() != "")
            {
                dt = oDAL.GetAssetDetailwithAssetFARTag(txtAssetFARTagOld.Text.Trim(), Session["COMPANY"].ToString());
                if (dt.Rows.Count > 0)
                {
                    gvAssets.DataSource = dt;
                    gvAssets.DataBind();
                    gvAssets.Visible = true;
                    btnSubmit.Enabled = true;
                }
                else
                {
                    txtAssetFARTagOld.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Asset not found.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertN", "ShowAlertN('Please enter Asset Far Tag.');", true);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
}