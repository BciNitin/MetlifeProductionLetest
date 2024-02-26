using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPages_InvoiceFileUpload : System.Web.UI.Page
{
    AssetImageUpload_DAL oDAL;

    #region PAGE EVENTS

    /// <summary>
    /// Navigates to session expired page in case of user logs off/session expired.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new AssetImageUpload_DAL(Session["DATABASE"].ToString());
    }

    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Invoice File Upload");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Response.Redirect("Error.aspx", false);
        }
    }

    /// Page Load Event  Test the page rights and initial setup
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");

        try
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                string _strRights = clsGeneral.GetRights("INVOICE_FILE_UPLOAD", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "INVOICE_FILE_UPLOAD");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    #endregion

    #region Load GridView 

    private void LoadData(int i = 0)
    {
        gvAssetData.DataSource = null;
        int display = 0;
        //if(string.IsNullOrEmpty(txtAssetInvoiceNo.Text) && string.IsNullOrEmpty(txtPONumber.Text))
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Invalid search filters or assets are assigned.');", true);
        //    return;
        //}
        string FilterAssetPONumber = txtPONumber.Text.Trim() != "" ? txtPONumber.Text.Trim() : null;
        string FilterAssetInvoiceNumber = txtAssetInvoiceNo.Text.Trim() != "" ? txtAssetInvoiceNo.Text.Trim() : null;

        if (rdbDownload.Checked)
            display = 1;

        gvAssetData.DataSource = oDAL.GetAssertPendingUploadImage(FilterAssetPONumber, FilterAssetInvoiceNumber, Session["COMPANY"].ToString(), display);
        gvAssetData.DataBind();
        if (display == 1)
        {
            gvAssetData.Columns[1].Visible = false;
            gvAssetData.Columns[2].Visible = true;
        }
        else
        {
            gvAssetData.Columns[1].Visible = true;
            gvAssetData.Columns[2].Visible = false;
        }
    }

    #endregion

    #region Grid Review

    protected void fileUploadedComplete(object sender, AsyncFileUploadEventArgs e)
    {
        DataTable dtITInvoiceUpload = new DataTable();
        dtITInvoiceUpload.Columns.Add("INVOICE_NO");
        dtITInvoiceUpload.Columns.Add("DOCUMENT_NAME");
        AsyncFileUpload fu = (AsyncFileUpload)sender;
        GridViewRow row = (GridViewRow)fu.NamingContainer;
        string idx = row.RowIndex.ToString();
        //string assetCode = (row.Cells[0].FindControl("lbleid") as Label).Text;
        //string PONumber = (row.Cells[0].FindControl("lblPONumber") as Label).Text;
        string InvoiceNo = (row.Cells[0].FindControl("lblInvoiceNo") as Label).Text;
        if (fu != null && fu.HasFile)
        {
            string subPath = Request.PhysicalApplicationPath + "DocumentUpload\\AcquisitionFileUpload\\";
            //string subPath = Request.PhysicalApplicationPath + "TempFolderForUpload\\AssetAcq\\" + assetCode.Replace("/", "_");
            if (!Directory.Exists(subPath))
            {
                Directory.CreateDirectory(subPath);
            }
            string strFilePath = subPath + "\\" + fu.FileName;
            File.Delete(strFilePath);
            fu.SaveAs(strFilePath);
            string bResp = oDAL.UpdateAssetImage(InvoiceNo, strFilePath, Session["COMPANY"].ToString(), Session["CURRENTUSER"].ToString());
            if (bResp.Contains("SUCCESS"))
            {
                dtITInvoiceUpload.Rows.Add(InvoiceNo, fu.FileName);
                dtITInvoiceUpload.AcceptChanges();
                DataTable dp = oDAL.GetMailTransactionDetails("INVOICE_FILE_UPLOAD", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
                {
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        sendmail.FunctionSendingMailWithAssetData(dtITInvoiceUpload, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    }
                    catch (Exception ee)
                    {

                    }
                }
            }
            else
            {
                string msg = bResp.ToString().Replace("'", "");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('" + msg + "');", true);
                return;
            }
        }
    }

    protected void DownloadFile(object sender, EventArgs e)
    {
        try
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    #endregion

    #region Control Events

    //protected void ddlAssetLocation_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlAssetLocation.Text != "")
    //        {
    //            DataTable dt = oDAL.PopulateLocationName(ddlAssetLocation.Text.ToString());
    //            if (dt.Rows.Count > 0)
    //            {
    //                txtLocationName.Text = dt.Rows[0]["WH_NAME"].ToString();
    //            }
    //            else
    //            {
    //                txtLocationName.Text = null;
    //            }


    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        HandleExceptions(ex);
    //    }
    //}

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtPONumber.Text) && string.IsNullOrEmpty(txtAssetInvoiceNo.Text))
        {
            lblErrorMsg.Text = "Please Enter the Valid Details to filter";
            gvAssetData.DataSource = null;
            gvAssetData.DataBind();
            return;
        }
        else
        {
            lblErrorMsg.Text = null;
            LoadData();
        }
    }

    #endregion


    protected void gvAssetData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssetData.PageIndex = e.NewPageIndex;
            LoadData();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
}