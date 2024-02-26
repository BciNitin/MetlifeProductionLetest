using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using MobiVUE_ATS.PRP;
using MobiVUE_ATS.DAL;

public partial class VendorEscalationMatrix : System.Web.UI.Page
{
    VendorEscalationMatrix_DAL oDAL;
    VendorEscalationMatrix_PRP oPRP;
    public VendorEscalationMatrix()
    {
        oPRP = new VendorEscalationMatrix_PRP();
    }
    ~VendorEscalationMatrix()
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
        oDAL = new VendorEscalationMatrix_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user rights for vendor escalation matrix page and get details for being viewed.
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
                string _strRights = clsGeneral.GetRights("VENDOR_ESCALATION_MATRIX", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "VENDOR_ESCALATION_MATRIX");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                GetVendor();
                GetVenEscMatrixDetails();
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
    /// Populate gridview with vendoe escalation matrix details.
    /// </summary>
    private void GetVenEscMatrixDetails()
    {
        gvVenEscMatrix.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetVenEscMatrixDetails(Session["COMPANY"].ToString());
        gvVenEscMatrix.DataSource = dt;
        gvVenEscMatrix.DataBind();
    }
    
    /// <summary>
    /// Get vendor details to be populated into vendor dropdownlist.
    /// </summary>
    private void GetVendor()
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
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    private void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Vendor Escalation Matrix");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    #endregion

    #region SUBMIT EVENT
    /// <summary>
    /// Save Vendor Escalation Matrix Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                oPRP.VEMVendorCode = ddlVendor.SelectedValue.ToString();
                oPRP.VEMSupportType = ddlSupportType.Text.Trim();
                oPRP.VEMLevel = ddlLevel.SelectedItem.Text.Trim();
                oPRP.VEMEmail = txtVendorEmail.Text.Trim();
                oPRP.VEMPersonName = txtVendorName.Text.Trim();
                oPRP.VEMMobile = txtMobileNo.Text.Trim();
                oPRP.VEMRemarks = txtRemarks.Text.Trim();
                oPRP.VEMActive = chkSetStatus.Checked;
                oPRP.VEMAddress = txtAddress.Text.Trim();
                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                bool bRslt = oDAL.SaveUpdateVendorEscalationMatrix("SAVE", oPRP);
                if (bRslt)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Vendor Escalation details saved successfully.');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Vendor Escalation details not saved.');", true);
                GetVenEscMatrixDetails();
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    #region SELECTED INDEX CHANGED EVENT
    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlVendor.SelectedIndex != 0)
            {
                DataTable dt = new DataTable();
                dt = oDAL.GetVendorDetails(ddlVendor.SelectedValue.ToString(), Session["COMPANY"].ToString());
                txtMobileNo.Text = dt.Rows[0]["VENDOR_PHONE"].ToString();
                txtVendorName.Text = dt.Rows[0]["VENDOR_CONT_PERSON"].ToString();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Get Vendor Escalation Matrix details in edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVenEscMatrix_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                GridViewRow gvRow = (GridViewRow)gvVenEscMatrix.Rows[e.NewEditIndex];
                ViewState["VENDOR"] = ((Label)gvRow.FindControl("lblVendName")).Text.Trim();
                ViewState["SUPP_TYPE"] = ((Label)gvRow.FindControl("lblSuppType")).Text.Trim();
                ViewState["LEVEL"] = ((Label)gvRow.FindControl("lblLevel")).Text.Trim();

                gvVenEscMatrix.EditIndex = e.NewEditIndex;
                GetVenEscMatrixDetails();
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Get Vendor Escalation Matrix details in cancel edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVenEscMatrix_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                gvVenEscMatrix.EditIndex = -1;
                GetVenEscMatrixDetails();
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Delete Vendor Escalation Matrix details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVenEscMatrix_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                bool bRslt = false;
                GridViewRow gvRow = gvVenEscMatrix.Rows[e.RowIndex];
                oPRP.VEMCode = int.Parse(((Label)gvRow.FindControl("lblVEMCode")).Text.Trim());
                bRslt = oDAL.DeleteVEMDetails(oPRP.VEMCode, Session["COMPANY"].ToString());
                if (bRslt)
                {
                    GetVenEscMatrixDetails();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Vendor Escalation Matrix details could not be deleted!!');", true);
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Get Vendor Escalation Matrix details population before update mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVenEscMatrix_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlEVendor = (DropDownList)e.Row.FindControl("ddlEVendor");
                if (ddlEVendor != null)
                {
                    ddlEVendor.DataSource = oDAL.GetVendor(Session["COMPANY"].ToString());
                    ddlEVendor.DataTextField = "VENDOR_NAME";
                    ddlEVendor.DataValueField = "VENDOR_CODE";
                    ddlEVendor.DataBind();
                    ddlEVendor.Items.Insert(0, "-- Select Vendor --");
                    if (ViewState["VENDOR"].ToString() != "")
                        ddlEVendor.SelectedIndex = ddlEVendor.Items.IndexOf(ddlEVendor.Items.FindByText(ViewState["VENDOR"].ToString()));
                    else
                    {
                        ddlEVendor.SelectedIndex = ddlEVendor.Items.IndexOf(ddlEVendor.Items.FindByText("-- Select Vendor --"));
                    }
                }

                DropDownList ddlESuppType = (DropDownList)e.Row.FindControl("ddlESuppType");
                if (ddlESuppType != null)
                {
                    if (ViewState["SUPP_TYPE"].ToString() != "")
                        ddlESuppType.SelectedIndex = ddlESuppType.Items.IndexOf(ddlESuppType.Items.FindByValue(ViewState["SUPP_TYPE"].ToString()));
                    else
                    {
                        ddlESuppType.SelectedIndex = ddlESuppType.Items.IndexOf(ddlESuppType.Items.FindByValue("SELECT"));
                    }
                }

                DropDownList ddlELevel = (DropDownList)e.Row.FindControl("ddlELevel");
                if (ddlELevel != null)
                {
                    if (ViewState["LEVEL"].ToString() != "")
                        ddlELevel.SelectedIndex = ddlELevel.Items.IndexOf(ddlELevel.Items.FindByValue(ViewState["LEVEL"].ToString()));
                    else
                    {
                        ddlELevel.SelectedIndex = ddlELevel.Items.IndexOf(ddlELevel.Items.FindByValue("SELECT"));
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
    /// Vendor Escalation Matrix details update event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVenEscMatrix_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            else
            {
                GridViewRow gvRow = (GridViewRow)gvVenEscMatrix.Rows[e.RowIndex];
                oPRP.VEMCode = int.Parse(((Label)gvRow.FindControl("lblEVEMCode")).Text.Trim());
                oPRP.VEMPersonName = ((TextBox)gvRow.FindControl("txtEPersName")).Text.Trim();
                oPRP.VEMEmail = ((TextBox)gvRow.FindControl("txtEEmail")).Text.Trim();
                if (ViewState["VENDOR"].ToString() != "")
                {
                    if (((DropDownList)gvRow.FindControl("ddlEVendor")).SelectedItem.Text.Trim() == "-- Select Vendor --")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Vendor Name cannot be blank.');", true);
                        return;
                    }
                    else
                        oPRP.VEMVendorCode = ((DropDownList)gvRow.FindControl("ddlEVendor")).SelectedValue.ToString();
                }
                else
                    oPRP.VEMVendorCode = "";

                if (ViewState["SUPP_TYPE"].ToString() != "")
                {
                    if (((DropDownList)gvRow.FindControl("ddlESuppType")).SelectedValue.ToString() == "SELECT")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Support Type cannot be blank.');", true);
                        return;
                    }
                    else
                        oPRP.VEMSupportType = ((DropDownList)gvRow.FindControl("ddlESuppType")).SelectedValue.ToString();
                }
                else
                    oPRP.VEMSupportType = "";

                if (ViewState["LEVEL"].ToString() != "")
                {
                    if (((DropDownList)gvRow.FindControl("ddlELevel")).SelectedValue.ToString() == "SELECT")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Escalation Lavel cannot be blank.');", true);
                        return;
                    }
                    else
                        oPRP.VEMLevel = ((DropDownList)gvRow.FindControl("ddlELevel")).SelectedValue.ToString();
                }
                else
                    oPRP.VEMLevel = "";
                oPRP.VEMMobile = ((TextBox)gvRow.FindControl("txtEMobile")).Text.Trim();
                oPRP.VEMAddress = ((TextBox)gvRow.FindControl("txtEAddress")).Text.Trim();
                oPRP.VEMActive = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
                bool bRslt = oDAL.SaveUpdateVendorEscalationMatrix("UPDATE", oPRP);
                if (bRslt)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                    gvVenEscMatrix.EditIndex = -1;
                    GetVenEscMatrixDetails();
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Vendor escalation matrix gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvVenEscMatrix_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvVenEscMatrix.PageIndex = e.NewPageIndex;
            GetVenEscMatrixDetails();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion
}