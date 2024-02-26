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
using System.Text;
using System.Globalization;
using System.Net.Mail;
using System.IO;

public partial class WebPages_SoldScrappedAssets : System.Web.UI.Page
{

    AssetReplacement_DAL oDAL;
    AssetReplacement_PRP oPRP;
    public WebPages_SoldScrappedAssets()
    {
        oPRP = new AssetReplacement_PRP();
    }
    ~WebPages_SoldScrappedAssets()
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
        //txtActiveInAssetCode.Focus();
    }
    #endregion
    /// <summary>
    /// Checking user group rights for Asset replacement operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.btnAllSearch);
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("SCRAPPED_ASSET", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "SCRAPPED_ASSET");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                if (Session["GROUP"].ToString().ToUpper() == "SYSADMIN")
                {
                    txtSecurityGEDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    txtSecurityGEDate.Enabled = true;
                }
                else
                {
                    txtSecurityGEDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    txtSecurityGEDate.Enabled = true;
                }
                PopulateLocationFilter();
                GetSoldScrappedDetails(Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    #region PRIVATE FUNCTIONS
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

    private void GetSoldScrappedDetails(string CompCode)
    {
        //gvAssetReplacement.DataSource = null;
        //DataTable dt = new DataTable();
        //dt = oDAL.GetSoldScrappedDetails(CompCode);
        //gvAssetReplacement.DataSource = Session["SCRAPPED"] = dt;
        //gvAssetReplacement.DataBind();
    }

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

    #region File Upload
    private Tuple<bool, string> ValidateScrapFile(DataTable dt)
    {
        bool res = true;
        string msg = "success";
        int i = 0;
        //string[] StatusType = new string[] { "Repair", "Scrap" };
        foreach (DataRow dr in dt.Rows)
        {
            i++;
            oPRP.AssetCode = string.Empty;
            oPRP.AssetTag = string.Empty;
            //oPRP.Status = dr.Field<string>("Status").Trim();
            oPRP.ScrapRemark = (dr.Field<string>("REMARKS") == null || dr.Field<string>("REMARKS") == "" || dr.Field<string>("REMARKS") == string.Empty) ? string.Empty : dr.Field<string>("REMARKS").Trim();
            if (dr.Field<string>("SCRAP_DATE") == null || dr.Field<string>("SCRAP_DATE") == "" || dr.Field<string>("SCRAP_DATE") == string.Empty)
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Please enter the mandatory fields.";
                res = false;
                break;
            }
            else
                oPRP.ScrapDate = (dt.Columns.Contains("SCRAP_DATE")) ? dr.Field<string>("SCRAP_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            string date = "";
            if (ConvertToExcel.isValidateDate(oPRP.ScrapDate, out date))
            {
                oPRP.ScrapDate = date;
            }
            else
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Please enter valid scrap date in dd-MMM-yyyy format. Also set cell as Date Format.";
                res = false;
                break;
            }

            if (Session["COMPANY"].ToString() == "IT")
            {
                oPRP.SerialCode = (dr.Field<string>("SERIAL_NUMBER") == null || dr.Field<string>("SERIAL_NUMBER") == "" || dr.Field<string>("SERIAL_NUMBER") == string.Empty) ? string.Empty : dr.Field<string>("SERIAL_NUMBER").Trim();
                oPRP.AssetCode = oDAL.GetAssetCodeScrap(oPRP.SerialCode, Session["COMPANY"].ToString());
                oPRP.AssetTag = oDAL.GetRFIDTagSCRAP(oPRP.SerialCode, Session["COMPANY"].ToString());

                if (oPRP.SerialCode.Trim() == "" || oPRP.SerialCode.Trim() == string.Empty || oPRP.ScrapRemark.Trim() == "" || oPRP.ScrapRemark.Trim() == string.Empty)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter the mandatory fields.";
                    res = false;
                    break;
                }

                if (string.IsNullOrEmpty(oPRP.AssetCode) || oPRP.AssetCode == "" || oPRP.AssetCode == null)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Serial Code.";
                    res = false;
                    break;
                }

                DataTable dy = oDAL.ValidateScrapAssetDetails(oPRP.AssetCode, oPRP.AssetTag, Session["COMPANY"].ToString());
                if (dy.Rows.Count == 0)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Serial Code.";
                    res = false;
                    break;
                }
            }
            else
            {
                oPRP.Asset_FAR_TAG = (dr.Field<string>("ASSET_FAR_TAG") == null || dr.Field<string>("ASSET_FAR_TAG") == "" || dr.Field<string>("ASSET_FAR_TAG") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_FAR_TAG").Trim();
                oPRP.AssetCode = oDAL.GetAssetCodeScrap(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
                oPRP.AssetTag = oDAL.GetRFIDTagSCRAP(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());

                if (oPRP.Asset_FAR_TAG.Trim() == "" || oPRP.Asset_FAR_TAG.Trim() == string.Empty || oPRP.ScrapRemark.Trim() == "" || oPRP.ScrapRemark.Trim() == string.Empty)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter the mandatory fields.";
                    res = false;
                    break;
                }

                if (string.IsNullOrEmpty(oPRP.AssetCode) || oPRP.AssetCode == "" || oPRP.AssetCode == null)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Far Tag.";
                    res = false;
                    break;
                }

                DataTable dy = oDAL.ValidateScrapAssetDetails(oPRP.AssetCode, oPRP.AssetTag, Session["COMPANY"].ToString());
                if (dy.Rows.Count == 0)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Far Tag.";
                    res = false;
                    break;
                }
            }

            DataTable dtsubstatus =  oDAL.GetSubStatus(oPRP.AssetCode);
            if (dtsubstatus.Rows.Count == 0)
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + "Asset Status is not in Stock Condition.";
                res = false;
                break;
            }

        }
        return new Tuple<bool, string>(res, msg);
    }
    private void SaveAssetScrapDetails(DataTable dt)
    {
        DataTable dtITScrap = new DataTable();
        dtITScrap.Columns.Add("Asset Code");
        dtITScrap.Columns.Add("Site Location");
        dtITScrap.Columns.Add("Scrap Date");
        dtITScrap.Columns.Add("Asset Type");
        dtITScrap.Columns.Add("Asset Make");
        dtITScrap.Columns.Add("Asset Model");
        dtITScrap.Columns.Add("Serial Number");
        dtITScrap.Columns.Add("PO No");
        dtITScrap.Columns.Add("PO Date");
        dtITScrap.Columns.Add("Invoice No");
        dtITScrap.Columns.Add("Invoice Date");

        DataTable dtFacilitiesScrap = new DataTable();
        dtFacilitiesScrap.Columns.Add("Asset Code");
        dtFacilitiesScrap.Columns.Add("Site Location");
        dtFacilitiesScrap.Columns.Add("Scrap Date");
        dtFacilitiesScrap.Columns.Add("Asset Type");
        dtFacilitiesScrap.Columns.Add("Asset Make");
        dtFacilitiesScrap.Columns.Add("Asset Model");
        dtFacilitiesScrap.Columns.Add("Asset Far Tag");
        dtFacilitiesScrap.Columns.Add("PO No");
        dtFacilitiesScrap.Columns.Add("PO Date");
        dtFacilitiesScrap.Columns.Add("Invoice No");
        dtFacilitiesScrap.Columns.Add("Invoice Date");

        DataTable dtInvalidData = new DataTable();
        dtInvalidData = dt.Clone();
        dtInvalidData.Columns.Add("SAVE_STATUS");
        dtInvalidData.Columns.Add("ERROR_MESSAGE");
        int Cnt = 0; int i=0;
        foreach (DataRow dr in dt.Rows)
        {
            oPRP.ScrapRemark = (dr.Field<string>("REMARKS") == null || dr.Field<string>("REMARKS") == "" || dr.Field<string>("REMARKS") == string.Empty) ? string.Empty : dr.Field<string>("REMARKS").Trim();
            if (dr.Field<string>("SCRAP_DATE") == null || dr.Field<string>("SCRAP_DATE") == "" || dr.Field<string>("SCRAP_DATE") == string.Empty)
                oPRP.ScrapDate = string.Empty;
            else
                oPRP.ScrapDate = (dt.Columns.Contains("SCRAP_DATE")) ? dr.Field<string>("SCRAP_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            string date = "";
            if (ConvertToExcel.isValidateDate(oPRP.ScrapDate, out date))
            {
                oPRP.ScrapDate = date;
            }

            oPRP.ScrapRemark = (dr.Field<string>("REMARKS") == null || dr.Field<string>("REMARKS") == "" || dr.Field<string>("REMARKS") == string.Empty) ? string.Empty : dr.Field<string>("REMARKS").Trim();
            if (Session["COMPANY"].ToString() == "IT")
                oPRP.SerialCode = (dr.Field<string>("SERIAL_NUMBER") == null || dr.Field<string>("SERIAL_NUMBER") == "" || dr.Field<string>("SERIAL_NUMBER") == string.Empty) ? string.Empty : dr.Field<string>("SERIAL_NUMBER").Trim();
            else
                oPRP.Asset_FAR_TAG = (dr.Field<string>("ASSET_FAR_TAG") == null || dr.Field<string>("ASSET_FAR_TAG") == "" || dr.Field<string>("ASSET_FAR_TAG") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_FAR_TAG").Trim();

            oPRP.AssetCode = oDAL.GetAssetCodeScrap(Session["COMPANY"].ToString() == "IT" ? oPRP.SerialCode : oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
            oPRP.AssetTag = oDAL.GetRFIDTagSCRAP(Session["COMPANY"].ToString() == "IT" ? oPRP.SerialCode : oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());

            oPRP.AssetCode = oPRP.AssetCode.Trim() != "" ? oPRP.AssetCode.Trim() : null;
            oPRP.AssetTag = oPRP.AssetTag.Trim() != "" ? oPRP.AssetTag.Trim() : null;

            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            bool bResp = false;
            if (oPRP.AssetCode == "" || oPRP.AssetCode == null || oPRP.AssetCode == string.Empty || string.IsNullOrEmpty(oPRP.AssetCode))
                bResp = false;
            else
                bResp = oDAL.SaveUpdateScrapDetails(oPRP);

            if (!bResp)
            {
                dtInvalidData.Rows.Add(dr.ItemArray);
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "Duplicate / Not proper Data";
                i++;
                continue;
            }
            else
            {
                //if (Session["COMPANY"].ToString() == "IT")
                //{
                //    dtITScrapUpload.Rows.Add(oPRP.AssetCode, oPRP.SerialCode);
                //    dtITScrapUpload.AcceptChanges();
                //}
                //else
                //{
                //    dtFacilitiesScrapUpload.Rows.Add(oPRP.AssetCode, oPRP.Asset_FAR_TAG);
                //    dtFacilitiesScrapUpload.AcceptChanges();
                //}
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
                    string AssetLocation = (dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == null || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION").Trim();
                    dtITScrap.Rows.Add(oPRP.AssetCode, AssetLocation, oPRP.ScrapDate, AssetType, AssetMake, AssetModel, oPRP.SerialCode, PONumber, PODate, InvoiceNo, InvoiceDate);
                    dtITScrap.AcceptChanges();
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
                    string AssetLocation = (dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == null || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION").Trim();
                    dtFacilitiesScrap.Rows.Add(oPRP.AssetCode, AssetLocation, oPRP.ScrapDate, AssetType, AssetMake, AssetModel, oPRP.Asset_FAR_TAG, PONumber, PODate, InvoiceNo, InvoiceDate);
                    dtFacilitiesScrap.AcceptChanges();
                }

                Cnt++;
            }
                
        }
        if(Cnt > 0)
        {
            DataTable dp = oDAL.GetMailTransactionDetails("SCRAPPED_ASSET", Convert.ToString(Session["COMP_NAME"]));
            if (dp.Rows.Count > 0)
            {
                try
                {
                    SendmailAlert sendmail = new SendmailAlert();
                    if (Session["COMPANY"].ToString() == "IT")
                        sendmail.FunctionSendingMailWithAssetData(dtITScrap, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    else
                        sendmail.FunctionSendingMailWithAssetData(dtFacilitiesScrap, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                }
                catch (Exception ee)
                {

                }
            }
        }
        if (i == 0)
        {
            Session["ERRORDATA"] = null;
            lblErrorMsg.Text = "Please Note: Asset details are scrapped  " + Cnt + " out of " + dt.Rows.Count + ".";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertNew", "ShowAlertNew('Please Note : Asset details are scrapped  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
        }
        else
        {
            btnExport.Visible = true;
            btnExport.Enabled = true;
            Session["ERRORDATA"] = dtInvalidData;
            lblErrorMsg.Text = "Some Of The Rows Are Not Inserted Please Download The Report To Find The Error.";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertNew", "ShowAlertNew('Some Of The Rows Are Not Inserted Please Download The Report To Find The Error');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('File Containing Data is not Correct, For Further information Please click Export Button');", true);
        }
        //lblErrorMsg.Text = "Please Note: Asset details are scrapped  " + Cnt + " out of " + dt.Rows.Count + ".";
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertMessage", "ShowAlertMessage('Please Note : Asset details are scrapped  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
    }
    private bool isValidateDate(string DateValue, out string DateOutput)
    {
        #region
        //double dateS = double.Parse(DateValue);
        //DateValue = DateTime.FromOADate(dateS).ToString("dd-MMM-yyyy");
        //DateValue = DateValue.Substring(0, 11);
        //DateTime dt;
        //if (DateTime.TryParseExact(DateValue,
        //                            "dd-MMM-yyyy",
        //                            CultureInfo.InvariantCulture,
        //                            DateTimeStyles.None,
        //    out dt))
        //{
        //    DateOutput = dt.ToString("dd/MMM/yyyy");
        //    return true;
        //}
        //else
        //{
        //    DateOutput = "";
        //    return false;
        //}
        #endregion
        try
        {
            double dateS = double.Parse(DateValue);
            DateValue = DateTime.FromOADate(dateS).ToString("dd-MMM-yyyy");

            DateValue = DateValue.Substring(0, 11);
            DateTime dt;

            if (DateTime.TryParseExact(DateValue,
                                        "dd-MMM-yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out dt))
            {
                DateOutput = dt.ToString("dd/MMM/yyyy");
                return true;
            }
            else
            {
                DateOutput = "";
                return false;
            }
        }
        catch (Exception ex)
        {
            DateOutput = "";
            return false;
        }
    }
    #endregion
    #region SUBMIT EVENT
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtITScrap = new DataTable();
            dtITScrap.Columns.Add("Asset Code");
            dtITScrap.Columns.Add("Site Location");
            dtITScrap.Columns.Add("Scrap Date");
            dtITScrap.Columns.Add("Asset Type");
            dtITScrap.Columns.Add("Asset Make");
            dtITScrap.Columns.Add("Asset Model");
            dtITScrap.Columns.Add("Serial Number");
            dtITScrap.Columns.Add("PO No");
            dtITScrap.Columns.Add("PO Date");
            dtITScrap.Columns.Add("Invoice No");
            dtITScrap.Columns.Add("Invoice Date");

            DataTable dtFacilitiesScrap = new DataTable();
            dtFacilitiesScrap.Columns.Add("Asset Code");
            dtFacilitiesScrap.Columns.Add("Site Location");
            dtFacilitiesScrap.Columns.Add("Scrap Date");
            dtFacilitiesScrap.Columns.Add("Asset Type");
            dtFacilitiesScrap.Columns.Add("Asset Make");
            dtFacilitiesScrap.Columns.Add("Asset Model");
            dtFacilitiesScrap.Columns.Add("Asset Far Tag");
            dtFacilitiesScrap.Columns.Add("PO No");
            dtFacilitiesScrap.Columns.Add("PO Date");
            dtFacilitiesScrap.Columns.Add("Invoice No");
            dtFacilitiesScrap.Columns.Add("Invoice Date");

            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Asset Scrapped", "Asset Scrapped", "Asset  Scrapped done by user id" + Session["CURRENTUSER"].ToString() /*+ " Asset Serial " + txtActiveInAssetCode.Text.Trim()*/);

            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            //if (ddlStatus.SelectedValue == "")
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Please select the status.');", true);
            //    ddlStatus.Focus();
            //    return;
            //}
            if (txtSecurityGEDate.Text.Trim() != "")
            {
                int iDate = clsGeneral.CompareDate(txtSecurityGEDate.Text.Trim(), DateTime.Now.ToString("dd/MMM/yyyy"));
                if (iDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Scrap date cannot be later than current date!');", true);
                    txtSecurityGEDate.Focus();
                    return;
                }
            }
            bool bAssetSelected = false;
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
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Click on Get Assets button/search to select assets.');", true);
                return;
            }
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            //if (txtActiveInAssetCode.Text != "")
            //{
            //    oPRP.FaultyOutSerialCode = txtActiveInAssetCode.Text.Trim().Split('|')[1].Trim();
            //    oPRP.AssetTag = txtActiveInAssetCode.Text.Trim().Split('|')[0].Trim();
            //}
            //oPRP.Status = ddlStatus.SelectedValue.Trim();
            oPRP.ScrapDate = txtSecurityGEDate.Text.Trim();
            oPRP.ScrapRemark = txtRemarks.Text.Trim();
            bool bResp = false;
            foreach (GridViewRow gvRow in gvAssets.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                {
                    oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.SerialCode = ((Label)gvRow.FindControl("lblSrNo")).Text.Trim();
                    else
                        oPRP.Asset_FAR_TAG= ((Label)gvRow.FindControl("lblFarTag")).Text.Trim();
                    oPRP.AssetTag = ((Label)gvRow.FindControl("lblAssetID")).Text.Trim();
                    bResp = oDAL.SaveUpdateScrapDetails(oPRP);
                    if(bResp)
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
                            string AssetLocation = (dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == null || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION").Trim();
                            dtITScrap.Rows.Add(oPRP.AssetCode, AssetLocation, oPRP.ScrapDate, AssetType, AssetMake, AssetModel, oPRP.SerialCode, PONumber, PODate, InvoiceNo, InvoiceDate);
                            dtITScrap.AcceptChanges();
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
                            string AssetLocation = (dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == null || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_LOCATION").Trim();
                            dtFacilitiesScrap.Rows.Add(oPRP.AssetCode, AssetLocation,oPRP.ScrapDate, AssetType, AssetMake, AssetModel, oPRP.Asset_FAR_TAG, PONumber, PODate, InvoiceNo, InvoiceDate);
                            dtFacilitiesScrap.AcceptChanges();
                        }
                    }
                }
            }

            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertMessage", "ShowAlertMessage('Asset scrapped details not saved.');", true);

                return;
            }
            else
            {
                DataTable dp = oDAL.GetMailTransactionDetails("SCRAPPED_ASSET", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
                {
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        if (Session["COMPANY"].ToString() == "IT")
                            sendmail.FunctionSendingMailWithAssetData(dtITScrap, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        else
                            sendmail.FunctionSendingMailWithAssetData(dtFacilitiesScrap, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    }
                    catch (Exception ee)
                    {
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertMessage", "ShowAlertMessage('Asset scrapped details saved successfully.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            }
            GetSoldScrappedDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    //protected void btnGo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (txtActiveInAssetCode.Text.Trim() != "")
    //        {
    //            if (txtActiveInAssetCode.Text.Contains("|"))
    //            {
    //                DataTable dt = new DataTable();
    //                dt = oDAL.GetAssetDetails(txtActiveInAssetCode.Text.Split('|')[0].Trim(), Session["COMPANY"].ToString());
    //                if (dt.Rows.Count > 0)
    //                {
    //                    gvAssets.DataSource = dt;
    //                    gvAssets.DataBind();
    //                    gvAssets.Visible = true;
    //                    btnSubmit.Enabled = true;
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Asset not found.');", true);
    //                }
    //            }
    //            else
    //            {
    //                DataTable dt = new DataTable();
    //                dt = oDAL.GetAssetDetails(txtActiveInAssetCode.Text.Trim(), Session["COMPANY"].ToString());
    //                if (dt.Rows.Count > 0)
    //                {
    //                    txtActiveInAssetCode.Text = Convert.ToString(dt.Rows[0]["Tag_ID"]) + '|' + Convert.ToString(dt.Rows[0]["ASSET_CODE"]);
    //                    gvAssets.DataSource = dt;
    //                    gvAssets.DataBind();
    //                    gvAssets.Visible = true;
    //                    btnSubmit.Enabled = true;

    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Asset not found or asset is scrapped.');", true);
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    { HandleExceptions(ex); }
    //}

    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        #region
        //try
        //{
        //    if (clsGeneral._strRights[1] == "0")
        //    {
        //        Response.Redirect("UnauthorizedUser.aspx");
        //    }
        //    if (gvAssetReplacement.Rows.Count > 0)
        //    {
        //        Response.Clear();
        //        DataTable dt = (DataTable)Session["SCRAPPED"];
        //        //DataSet dsExport = new DataSet();
        //        //System.IO.StringWriter tw = new System.IO.StringWriter();
        //        //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        //        //System.Web.UI.WebControls.DataGrid dgGrid = new System.Web.UI.WebControls.DataGrid();
        //        //dgGrid.DataSource = dt;
        //        //dgGrid.HeaderStyle.Font.Bold = true;
        //        //dgGrid.DataBind();
        //        //dgGrid.RenderControl(hw);
        //        //Response.ContentType = "application/vnd.ms-excel";
        //        //this.EnableViewState = false;
        //        //Response.Write(tw.ToString());
        //        //Response.End();
        //        using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
        //        {
        //            wb.Worksheets.Add(dt);
        //            wb.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
        //            wb.Style.Font.Bold = true;

        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.Charset = "";
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;filename=REPORT" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");

        //            using (MemoryStream MyMemoryStream = new MemoryStream())
        //            {
        //                wb.SaveAs(MyMemoryStream);
        //                MyMemoryStream.WriteTo(Response.OutputStream);
        //                Response.Flush();
        //                Response.End();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('There is no data to export.');", true);
        //        return;
        //    }
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
        #endregion
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
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertNew", "ShowAlertNew('Please Note : There is no data for being exported.');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    protected void gvAssets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssets.DataSource = (DataTable)Session["GRIDDATA"];
            gvAssets.DataBind();
            gvAssets.PageIndex = e.NewPageIndex;
            gvAssets.DataBind();
            //GetSoldScrappedDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

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

    protected void btnAllSearch_Click(object sender, EventArgs e)
    {
        try
        {

            string AssetMake = txtAssetMake.Text.Trim() != "" ? txtAssetMake.Text.Trim() : null;
            string AssetModel = txtAssetModel.Text.Trim() != "" ? txtAssetModel.Text.Trim() : null;
            string SerialNo = txtAssetSerialNo.Text.Trim() != "" ? txtAssetSerialNo.Text.Trim() : null;
            string AssetType = txtAssetType.Text.Trim() != "" ? txtAssetType.Text.Trim() : null;
            string SiteLocation = string.Empty;
            if (ddlSiteLocationFilter.SelectedIndex == 0 || ddlSiteLocationFilter.SelectedValue == "ALL")
                SiteLocation = string.Empty;
            else
                SiteLocation = ddlSiteLocationFilter.SelectedValue;
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
            //dt = oDAL.GetAllSearchAssetDetails(AssetMake, AssetModel, AssetType, SiteLocation, SerialNo, "Scrap", oPRP.CompCode);

            dt = oDAL.GetAllSearchAssetDetails(AssetMake, AssetModel, AssetType, SiteLocation, SerialNo, "Scrap", oPRP.CompCode, FilterStatus, filterSubStatus, FilterAssetFarTag, FilterAssetDomain, FilterAssetHDD, FilterAssetProcessor, FilterAssetRAM, FilterAssetPONumber, FilterAssetVendor, FilterAssetInvoiceNumber, FilterAssetRFIDTag, FilterAssetGRNNo);
            Session["GRIDDATA"] = dt;

            if (dt.Rows.Count > 0)
            {
                gvAssets.DataSource = null;
                gvAssets.DataSource = dt;
                gvAssets.DataBind();
                gvAssets.Visible = true;
                btnSubmit.Enabled = true;
                if (Session["COMPANY"].ToString() == "IT")
                {
                    gvAssets.Columns[5].Visible = false;
                }
                else
                {
                    gvAssets.Columns[4].Visible = false;
                }
            }
            else
            {
                gvAssets.DataSource = null;
                gvAssets.DataBind();
                gvAssets.Visible = false;
                btnSubmit.Enabled = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Invalid search filters or assets are scrapped.');", true);
                return;
            }

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    private void PopulateLocationFilter()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetSiteLocation(Session["COMPANY"].ToString());
        ddlSiteLocationFilter.DataSource = null;
        ddlSiteLocationFilter.DataSource = dt;
        ddlSiteLocationFilter.DataValueField = "SITE_CODE";
        ddlSiteLocationFilter.DataTextField = "SITE_CODE";
        ddlSiteLocationFilter.DataBind();
        ddlSiteLocationFilter.Items.Insert(0, "-- Select Location --");
    }
    protected void btnUploadFile_Click(object sender, EventArgs e)
    {
        try
        {
            ConvertToExcel convertToExcel = new ConvertToExcel();
            var response = convertToExcel.ValidateFileReaded(AssetFileUpload, Session["COMPANY"].ToString() == "IT" ? "ITScrap" : "FacilitiesScrap");
            if (response.Item1)
            {
                DataTable dt = response.Item2;
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertMessage", "ShowAlertMessage('No Data in the file');", true);
                    return;
                }
                var validate = ValidateScrapFile(dt);
                if (validate.Item1)
                {
                    SaveAssetScrapDetails(dt);
                }
                else
                {
                    //lblErrorMsg.Text = validate.Item2;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('" + validate.Item2 + "');", true);
                    return;
                }
            }
            else
            {
                //lblErrorMsg.Text = response.Item3;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('" + response.Item3 + "');", true);
                return;
            }

        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
}

