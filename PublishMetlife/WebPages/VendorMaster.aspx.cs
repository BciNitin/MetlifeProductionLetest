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
using MobiVUE_ATS.PRP;
using MobiVUE_ATS.DAL;
using System.IO;

public partial class VendorMaster : System.Web.UI.Page
{
    VendorMaster_DAL oDAL;
    VendorMaster_PRP oPRP;
    public VendorMaster()
    {
        oPRP = new VendorMaster_PRP();
    }
    ~VendorMaster()
    {
        oDAL = null; oPRP = null;
    }

    #region PAGE EVENTS
    /// <summary>
    /// Navigate to session expired page in case of user session is expired.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new VendorMaster_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user rights for vnedor master page and get vendor details for being viewed.
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
                string _strRights = clsGeneral.GetRights("VENDOR_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "VENDOR_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                GetVendorDetails(Session["COMPANY"].ToString());
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
    /// Get vendor details to be populated in gridview
    /// </summary>
    private void GetVendorDetails(string _CompCode)
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetVendorDetails(_CompCode);
        gvVendMaster.DataSource = Session["VENDOR"] = dt;
        gvVendMaster.DataBind();
    }

    /// <summary>
    /// Get country details
    /// </summary>
    private void GetCountry()
    {
        //ddlState.DataSource = null;
        //DataTable dt = new DataTable();
        //dt = oDAL.GetCountry();
        //ddlCountry.DataSource = dt;
        //ddlCountry.DataValueField = "GENERAL_CODE";
        //ddlCountry.DataTextField = "GENERAL_NAME";
        //ddlCountry.DataBind();
        //ddlCountry.Items.Insert(0, "SELECT");
    }

    /// <summary>
    /// Get state names inside a country
    /// </summary>
    /// <param name="_CountryName"></param>
    private void GetState(string _CountryName)
    {
        //ddlCountry.DataSource = null;
        //DataTable dt = new DataTable();
        //dt = oDAL.GetState(_CountryName);
        //ddlState.DataSource = dt;
        //ddlState.DataValueField = "GENERAL_CODE";
        //ddlState.DataTextField = "GENERAL_NAME";
        //ddlState.DataBind();
        //ddlState.Items.Insert(0, "SELECT");
    }

    /// <summary>
    /// Get city names inside a state
    /// </summary>
    /// <param name="_StateName"></param>
    private void GetCity(string _StateName)
    {
        //ddlCountry.DataSource = null;
        //DataTable dt = new DataTable();
        //dt = oDAL.GetCity(_StateName);
        //ddlCity.DataSource = dt;
        //ddlCity.DataValueField = "GENERAL_CODE";
        //ddlCity.DataTextField = "GENERAL_NAME";
        //ddlCity.DataBind();
        //ddlCity.Items.Insert(0, "SELECT");
    }

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    private void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Vendor Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Response.Redirect("Error.aspx");
        }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Save new vendor details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Vendor Master", "Vendor Master", "Vendor created by user id" + Session["CURRENTUSER"].ToString());
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.VendorCode = txtVendorCode.Text.Trim().ToUpper();
            oPRP.VendorName = txtVendorName.Text.Trim().ToUpper();
            oPRP.VendorAddress = txtAddress.Text.Trim();
            oPRP.VendorCountry = txtCountry.Text.Trim();
            oPRP.VendorSate = txtState.Text.Trim();
            oPRP.VendorCity = txtCity.Text.Trim();
            oPRP.VendorPIN = txtPIN.Text.Trim();
            oPRP.VendorContPerson = txtContPerson.Text.Trim();
            oPRP.VendorEmail = txtEmailID.Text.Trim();
            oPRP.VendorPhone = txtPhone.Text.Trim();
            oPRP.Remarks = txtRemarks.Text.Trim();
            oPRP.Active = chkSetStatus.Checked;
            oPRP.WorkCatagory = ddlWorkCategory.SelectedValue;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            bool bResp = oDAL.SaveUpdateVendorMaster("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Duplicate Vendor Code not Allowed!');", true);
                txtVendorName.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Vendor details are saved successfully.');", true);
                upSubmit.Update();
            }
            GetVendorDetails(Session["COMPANY"].ToString());
            btnSubmit.Enabled = true;
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Export gridview data to excel sheet.
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
            if (gvVendMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["VENDOR"];
                dt.TableName = "VendorReport";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=VendorMaster.xls");
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
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Get dropdowns populated while gridview comes into edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVendMaster_RowDataBound(object sender,GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlECountry = (DropDownList)e.Row.FindControl("ddlECountry");
                if (ddlECountry != null)
                {
                    ddlECountry.DataSource = oDAL.GetCountry();
                    ddlECountry.DataTextField = "GENERAL_NAME";
                    ddlECountry.DataValueField = "GENERAL_CODE";
                    ddlECountry.DataBind();
                    ddlECountry.Items.Insert(0, "SELECT");
                    ddlECountry.SelectedIndex = ddlECountry.Items.IndexOf(ddlECountry.Items.FindByText(ViewState["COUNTRY"].ToString()));
                }
                DropDownList ddlEState = (DropDownList)e.Row.FindControl("ddlEState");
                if (ddlEState != null)
                {
                    ddlEState.DataSource = oDAL.GetState(ViewState["COUNTRY"].ToString());
                    ddlEState.DataTextField = "GENERAL_NAME";
                    ddlEState.DataValueField = "GENERAL_CODE";
                    ddlEState.DataBind();
                    ddlEState.Items.Insert(0, "SELECT");
                    ddlEState.SelectedIndex = ddlEState.Items.IndexOf(ddlEState.Items.FindByText(ViewState["STATE"].ToString()));
                }
                DropDownList ddlECity = (DropDownList)e.Row.FindControl("ddlECity");
                if (ddlECity != null)
                {
                    ddlECity.DataSource = oDAL.GetCity(ViewState["STATE"].ToString());
                    ddlECity.DataTextField = "GENERAL_NAME";
                    ddlECity.DataValueField = "GENERAL_CODE";
                    ddlECity.DataBind();
                    ddlECity.Items.Insert(0, "SELECT");
                    ddlECity.SelectedIndex = ddlECity.Items.IndexOf(ddlECity.Items.FindByText(ViewState["CITY"].ToString()));
                }
                ImageButton imagebuttonEdit = (ImageButton)e.Row.FindControl("imagebuttonEdit");
                ImageButton imagebuttonDelete = (ImageButton)e.Row.FindControl("imagebuttonDelete");
                if (imagebuttonEdit != null)
                {
                    if (clsGeneral._strRights[2] == "0")
                        imagebuttonEdit.Enabled = false;
                    else
                        imagebuttonEdit.Enabled = true;
                }
                if (imagebuttonDelete != null)
                {
                    if (clsGeneral._strRights[3] == "0")
                        imagebuttonDelete.Enabled = false;
                    else
                        imagebuttonDelete.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Vendor details delete event through gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVendMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvVendMaster.Rows[e.RowIndex];
            oPRP.VendorCode = ((Label)gvRow.FindControl("lblEVendCode")).Text.Trim();
            oPRP.CompCode = Session["COMPANY"].ToString();
            bool bRslt = oDAL.DeleteVendor(oPRP.VendorCode, oPRP.CompCode);
            if (!bRslt)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Vendor details are not deleted.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Vendor details deleted successfully.');", true);
            }
            GetVendorDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Get vendor master details in gridview into edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVendMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvVendMaster.Rows[e.NewEditIndex];
            ViewState["COUNTRY"] = ((Label)gvRow.FindControl("lblCountry")).Text.Trim();
            ViewState["STATE"] = ((Label)gvRow.FindControl("lblState")).Text.Trim();
            ViewState["CITY"] = ((Label)gvRow.FindControl("lblCity")).Text.Trim();

            gvVendMaster.EditIndex = e.NewEditIndex;
            GetVendorDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Update vendor master details through gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVendMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvVendMaster.Rows[e.RowIndex];
            oPRP.VendorCode = ((Label)gvRow.FindControl("lblEVendCode")).Text.Trim().ToUpper();
            oPRP.VendorName = ((TextBox)gvRow.FindControl("txtEVendName")).Text.Trim().ToUpper();
            oPRP.VendorAddress = ((TextBox)gvRow.FindControl("txtEAdd")).Text.Trim();
            oPRP.VendorCountry = ((TextBox)gvRow.FindControl("txtECountry")).Text.Trim();
            oPRP.VendorSate = ((TextBox)gvRow.FindControl("txtEState")).Text.Trim();
            oPRP.VendorCity = ((TextBox)gvRow.FindControl("txtECity")).Text.Trim();
            oPRP.VendorPIN = ((TextBox)gvRow.FindControl("txtEPIN")).Text.Trim();
            oPRP.VendorContPerson = ((TextBox)gvRow.FindControl("txtEContPerson")).Text.Trim();
            oPRP.VendorPhone = ((TextBox)gvRow.FindControl("txtEPhone")).Text.Trim();
            oPRP.VendorEmail = ((TextBox)gvRow.FindControl("txtEEMail")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oDAL.SaveUpdateVendorMaster("UPDATE", oPRP);

            gvVendMaster.EditIndex = -1;
            GetVendorDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Vendor master cancel edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVendMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            gvVendMaster.EditIndex = -1;
            GetVendorDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Vendor master gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVendMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvVendMaster.PageIndex = e.NewPageIndex;
            GetVendorDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    #region SELECTEDINDEXCHANGED EVENTS
    /// <summary>
    /// Fetch state names based on country name selected through dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlECountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlECountry = sender as DropDownList;
            foreach (GridViewRow gvRow in gvVendMaster.Rows)
            {
                Control ddlCtrl = gvRow.FindControl("ddlECountry") as DropDownList;
                if (ddlCtrl != null)
                {
                    DropDownList ddlCntry = (DropDownList)ddlCtrl;
                    if (ddlECountry.ClientID == ddlCntry.ClientID)
                    {
                        DropDownList ddlEState = gvRow.FindControl("ddlEState") as DropDownList;
                        DataTable dt = new DataTable();
                        dt = oDAL.GetState(ddlCntry.SelectedItem.Text.Trim());
                        ddlEState.DataSource = dt;
                        ddlEState.DataTextField = "GENERAL_NAME";
                        ddlEState.DataValueField = "GENERAL_CODE";
                        ddlEState.DataBind();
                        ddlEState.Items.Insert(0, "SELECT");
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
    /// Fetch city names based on state name selected through dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlEState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlEState = sender as DropDownList;
            foreach (GridViewRow gvRow in gvVendMaster.Rows)
            {
                Control ddlCtrl = gvRow.FindControl("ddlEState") as DropDownList;
                if (ddlCtrl != null)
                {
                    DropDownList ddlState = (DropDownList)ddlCtrl;
                    if (ddlEState.ClientID == ddlState.ClientID)
                    {
                        DropDownList ddlECity = gvRow.FindControl("ddlECity") as DropDownList;
                        DataTable dt = new DataTable();
                        dt = oDAL.GetCity(ddlState.SelectedItem.Text.Trim());
                        ddlECity.DataSource = dt;
                        ddlECity.DataTextField = "GENERAL_NAME";
                        ddlECity.DataValueField = "GENERAL_CODE";
                        ddlECity.DataBind();
                        ddlECity.Items.Insert(0, "SELECT");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion
}