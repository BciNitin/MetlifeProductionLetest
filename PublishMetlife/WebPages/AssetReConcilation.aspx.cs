using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using MobiVUE_ATS.DAL;

public partial class AssetReConcilation : System.Web.UI.Page
{
    AssetReconcilation_DAL oDAL;
    string filePath = "";
    public AssetReConcilation()
    {
        
    }
    ~AssetReConcilation()
    {
        oDAL = null;
    }

    #region PAGE EVENTS
    /// <summary>
    /// Navigates to session expired page in case of user logs off/session expired.
    /// </summary>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new AssetReconcilation_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user rights for asset acquisition save/update operation.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (!Directory.Exists(Server.MapPath("UploadedCSVFiles")))
                    Directory.CreateDirectory(Server.MapPath("UploadedCSVFiles"));
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                string _strRights = clsGeneral.GetRights("ASSET_RECONCILIATION", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_RECONCILIATION");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
               // ibtnUpload.Enabled = true;
              //  GetMaxReconcileDate();
              //  ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
               // ibtnUpload.Enabled = true;
              //  txtReConcileDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                //txtReConcileDate.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetMaxReconcileDate()
    {
        //DataTable dt = oDAL.GetMaxReconcileDate(Session["COMPANY"].ToString());
        //DateTime MaxReconDate = Convert.ToDateTime(dt.Rows[0]["MAXRECONDATE"].ToString());
        //if (MaxReconDate.ToString("dd/MMM/yyyy") != "01/Jan/1900")
        //    txtReConcileDate.Text = MaxReconDate.ToString("dd/MMM/yyyy");
        //else
        txtReConcileDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        txtReConcileDate.Enabled = false;
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Cathes exception for the entier page operations.
    /// </summary>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Asset Reconciliation");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Read scanned file.
    /// </summary>
    private object ReadToEnd(string filePath)
    {
        DataTable dtDataSource = new DataTable();
        string[] fileContent = File.ReadAllLines(filePath);
        if (fileContent.Count() > 0)
        {
            dtDataSource.Columns.Add("LOC_CODE");
            dtDataSource.Columns.Add("ASSET_CODE");
            for (int i = 0; i < fileContent.Count(); i++)
            {
                string[] rowData = fileContent[i].Split(',');
                dtDataSource.Rows.Add(rowData[0], rowData[1]);
            }
        }
        return dtDataSource;
    }

    /// <summary>
    /// Upload scanned file data to database as reconciled assets.
    /// </summary>
    private bool UploadFileToDatabase(DataTable dt)
    {
        bool bDuplicate = false;
        string Code = "";
        string LocCode = "";
        string CodeType = "";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            LocCode = dt.Rows[i][0].ToString();
            Code = dt.Rows[i][1].ToString();
            if (!string.IsNullOrEmpty(Code))
            {
                if (Code.Substring(0, 2).ToUpper() == Session["COMPANY"].ToString())
                    CodeType = "ASSET";
                else
                    CodeType = "SERIAL";
                oDAL.UploadFileData(Code, LocCode, CodeType, Session["CURRENTUSER"].ToString(), txtReConcileDate.Text.Trim(), Session["COMPANY"].ToString());
            }
        }
        return bDuplicate;
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Scanned assets are reconciled here and invalid scanned data is saved separately.
    /// </summary>
    protected void ibtnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
                DataTable dt = new DataTable();
                dt = oDAL.GetReconsolidatedData(Session["COMPANY"].ToString(), "ADMIN");
                gvReconciledData.DataSource = dt;
                gvReconciledData.DataBind();
                Session["RECONCILE"] = dt;
               // btnReconcilationConfirmation.Visible = true;
                lblErrorMsg.Visible = false;
                btnExport.Visible = true;
           
           
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// On new reconcile, current date is displayed for reconciliation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNewReconcile_Click(object sender, EventArgs e)
    {
        try
        {
            //ibtnUpload.Enabled = true;
            txtReConcileDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
          //  txtReConcileDate.Enabled = false;
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Valid scanned assets can be exported from here in order to cross check the details.
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
            if (gvReconciledData.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["RECONCILE"];
                dt.TableName = "ReconcilationReport";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'>Asset Reconciliation Report</font></u></b>");
                //hw.WriteLine("<br>");
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=AssetReConcilation.xls");
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
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Get Previously reconciled data based on reconciliation date provided.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGetData_Click(object sender, EventArgs e)
    {
        try
        {
           
            if (txtReConcileDate.Text.Trim() != "")
            {
                DataTable dt = oDAL.GetFileData(txtReConcileDate.Text).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    gvCodes.DataSource = oDAL.GetFileData(txtReConcileDate.Text).Tables[0].DefaultView;
                    gvCodes.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Reconciliation date data not avilable.');", true);
                    // txtDate.Focus();
                    return;
                }
               
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Reconciliation date not avilable.');", true);
               // txtDate.Focus();
                return;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW_PAGE_INDEX CHANGING
    /// <summary>
    /// Asset code grid page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCodes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvCodes.PageIndex = e.NewPageIndex;
            gvCodes.DataSource = oDAL.GetFileData(txtReConcileDate.Text).Tables[0].DefaultView;
            gvCodes.DataBind();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex); 
        }
    }

    /// <summary>
    /// Invalid barcode scanned grid page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvInvalidCodes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //try
        //{
        //    gvInvalidCodes.PageIndex = e.NewPageIndex;
        //    gvInvalidCodes.DataSource = oDAL.GetFileData().Tables[1].DefaultView;
        //    gvInvalidCodes.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    HandleExceptions(ex);
        //}
    }

    /// <summary>
    /// Complete list of assets grid page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvReconciledData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvReconciledData.PageIndex = e.NewPageIndex;
            gvReconciledData.DataSource = oDAL.GetReconsolidatedData(Session["COMPANY"].ToString(), Session["ASSET_TYPE"].ToString());
            gvReconciledData.DataBind();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtReConcileDate.Text.Trim() != "")
            {
                string result= oDAL.ConfirmReconcileData(txtReConcileDate.Text, Session["COMPANY"].ToString());
                if (result.Contains("SUCCESS"))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Reconciliation date data save successfully.');", true);
                    // txtDate.Focus();
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Reconciliation date data saving error.');", true);
                    // txtDate.Focus();
                    return;
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Please enter Reconciliation date .');", true);
                // txtDate.Focus();
                return;
            }
           

        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
        
    }
}