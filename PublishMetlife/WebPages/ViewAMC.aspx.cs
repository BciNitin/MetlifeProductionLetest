using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
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

public partial class ViewAMC : System.Web.UI.Page
{
    AssetAcquisition_DAL oDAL;
    AssetAcquisition_PRP oPRP;
    public ViewAMC()
    {
        oPRP = new AssetAcquisition_PRP();
    }
    ~ViewAMC()
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
        oDAL = new AssetAcquisition_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking user group rights for viewing (all/active/expired) AMC.
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
                this.Page.Form.Attributes.Add("enctype", "multipart/form-data");
                string _strRights = clsGeneral.GetRights("VIEW_AMC", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "VIEW_ASSET_AMC");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                if (Request.QueryString["EXPIRE"] != null)
                {
                    if (Convert.ToString(Request.QueryString["EXPIRE"]).Trim() == "1")
                    {
                        rdoExpiredAMC.Checked = false;
                        rdoActiveAMC.Checked = true;
                        rdoAllAMC.Checked = false;
                        GetAMCDetails();
                    }
                }
                PopulateVendor();
            }
        }
        catch (Exception ex)
        { HandleException(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    private void HandleException(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "View Asset AMC Details");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Response.Redirect("Error.aspx");
        }
    }

    /// <summary>
    /// Get vendor code/name details.
    /// </summary>
    private void PopulateVendor()
    {
        ddlVendor.DataSource = null; 
        DataTable dt = new DataTable();
        dt = oDAL.GetVendor(Session["COMPANY"].ToString());
        ddlVendor.DataSource = dt;
        ddlVendor.DataValueField = "VENDOR_CODE";
        ddlVendor.DataTextField = "VENDOR_NAME";
        ddlVendor.DataBind();
        ddlVendor.Items.Insert(0, "-- Select Vendor --");
    }

    /// <summary>
    /// Get AMC details based on selected criterian.
    /// </summary>
    private void GetAMCDetails()
    {
        oPRP.AllAMC = rdoAllAMC.Checked;
        oPRP.ActiveAMC = rdoActiveAMC.Checked;
        oPRP.ExpiredAMC = rdoExpiredAMC.Checked;
        oPRP.CompCode = Session["COMPANY"].ToString();
        DataTable dt = new DataTable();
        if (ddlVendor.SelectedIndex > 0)
        {
            oPRP.AMCVendorCode = ddlVendor.SelectedValue.ToString();
            dt = oDAL.GetAMCDetails(oPRP, true);
            if (dt.Rows.Count > 0)
            {
                Session["VIEW_AMC"] = dt;
                gvViewAMC.DataSource = dt;
                gvViewAMC.DataBind();
                lblAssetCount.Text = "AMC Records Count : " + dt.Rows.Count.ToString();
            }
            else
            {
                gvViewAMC.DataSource = null;
                gvViewAMC.DataBind();
                lblAssetCount.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There are no records found for selected criterian.');", true);
                return;
            }
        }
        else
        {
            dt = oDAL.GetAMCDetails(oPRP, false);
            if (dt.Rows.Count > 0)
            {
                Session["VIEW_AMC"] = dt;
                gvViewAMC.DataSource = dt;
                gvViewAMC.DataBind();
                lblAssetCount.Text = "AMC Records Count : " + dt.Rows.Count.ToString();
            }
            else
            {
                gvViewAMC.DataSource = null;
                gvViewAMC.DataBind();
                lblAssetCount.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There are no records found for selected criterian.');", true);
                return;
            }
        }
    }
    #endregion

    #region SUBMIT EVENTS
    /// <summary>
    /// Get (all/active/expired) AMC details based on filter criteria.
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
            GetAMCDetails();
        }
        catch (Exception ex)
        { HandleException(ex); }
    }

    /// <summary>
    /// Export report data from gridview to excel worksheet.
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
            if (gvViewAMC.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["VIEW_AMC"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'> Asset AMC Details</font></u></b>");
                //hw.WriteLine("<br>");
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=ViewAMC.xls");
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
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no data for being exported.');", true);
                return;
            }
        }
        catch (Exception ex)
        { HandleException(ex); }
    }

    /// <summary>
    /// Refresh/Reset form fields and gridview details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblAssetCount.Text = "";
            gvViewAMC.DataSource = null;
            gvViewAMC.DataBind();
        }
        catch (Exception ex)
        { HandleException(ex); }
    }   
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewAMC_PageIndexChanging(object sender,GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["VIEW_AMC"];
            gvViewAMC.PageIndex = e.NewPageIndex;
            gvViewAMC.DataSource = dt;
            gvViewAMC.DataBind();
        }
        catch (Exception ex)
        { HandleException(ex); }
    }

    /// <summary>
    /// Redirects to AssetAMC.aspx page to view/edit AMC details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewAMC_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "View")
            {
                string AMCCode = e.CommandArgument.ToString();
                Response.Redirect("AssetAMC.aspx?AMCCode=" + AMCCode.ToString().Trim());
            }
        }
        catch (Exception ex)
        { HandleException(ex); }
    }
    #endregion
}