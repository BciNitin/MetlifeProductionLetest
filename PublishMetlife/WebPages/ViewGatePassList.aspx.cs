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

public partial class ViewGatePassList : System.Web.UI.Page
{
    GatePassGeneration_DAL oDAL;
    GatePassGeneration_PRP oPRP;
    public ViewGatePassList()
    {
        oPRP = new GatePassGeneration_PRP();
    }
    ~ViewGatePassList()
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
        oDAL = new GatePassGeneration_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user rights for gatepass list viewing.
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
                string _strRights = clsGeneral.GetRights("VIEW_GATEPASS_LIST", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "VIEW_GATEPASS_LIST");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                GetGatePassNo();
                PopulateLocation();
                GetGatePassCount();
                new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "GatePass List", "View gate pass list", "visit gatepass list module by user id " + Session["CURRENTUSER"].ToString() + "");
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// 
    /// </summary>
    private void GetGatePassNo()
    {
        ddlGatePassNo.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetGatePassNo(Session["COMPANY"].ToString());
        ddlGatePassNo.DataSource = dt;
        ddlGatePassNo.DataTextField = "GATEPASS_CODE";
        ddlGatePassNo.DataValueField = "GATEPASS_CODE";
        ddlGatePassNo.DataBind();
        ddlGatePassNo.Items.Insert(0, "-- Select GatePass No. --");
    }

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "View GatePass List");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Get location code/name for location selection.
    /// </summary>
    private void PopulateLocation()
    {
        lblLocCode.Text = "0";
        ddlGPLocation.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetLocation(Session["COMPANY"].ToString(), "", 1);
        ddlGPLocation.DataSource = dt;
        ddlGPLocation.DataValueField = "LOC_CODE";
        ddlGPLocation.DataTextField = "LOC_NAME";
        ddlGPLocation.DataBind();
        ddlGPLocation.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// Populate gatepass details for gatepass printing.
    /// </summary>
    private void GetGatePassDetails()
    {
        string[] LocParts = { };
        //if (lblLocCode.Text != "0")
        //    LocParts = lblLocCode.Text.Trim().Split('-');
        // if (lblLocCode.Text != "")
        //{

        //     //if (LocParts[2] == "00")
        //     //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1];
        //     //else if (LocParts[3] == "00")
        //     //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2];
        //     //else if (LocParts[4] == "00")
        //     //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3];
        //     //else if (LocParts[5] == "00")
        //     //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4];
        //     //else
        //     //    oPRP.AssetLocation = LocParts[0] + "-" + LocParts[1] + "-" + LocParts[2] + "-" + LocParts[3] + "-" + LocParts[4] + "-" + LocParts[5];
        // }
        if (ddlGPLocation.SelectedIndex > 0)
        {
            oPRP.AssetLocation = ddlGPLocation.SelectedValue;
        }
        else
            oPRP.AssetLocation = "";

        oPRP.CompCode = Session["COMPANY"].ToString();
        if (ddlGatePassNo.SelectedIndex != 0)
            oPRP.GatePassCode = ddlGatePassNo.SelectedValue.ToString();
        else
            oPRP.GatePassCode = "";

        if (ddlGatePassType.SelectedIndex != 0)
            oPRP.GatePassType = ddlGatePassType.SelectedValue.ToString();
        else
            oPRP.GatePassType = "";

        if (rdoApproved.Checked)
            oPRP.Approve_GatePass = true;
        else if (rdoUnApproved.Checked)
            oPRP.Approve_GatePass = false;

        gvGatePassList.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetGatePassDetails(oPRP);
        if (dt.Rows.Count > 0)
        {
            if (rdoApproved.Checked)
            {
                btnApprove.Visible = false;
                gvGatePassList.DataSource = Session["GATEPASS_LIST"] = dt;
                gvGatePassList.DataBind();

                //foreach (GridViewRow gvRow in gvGatePassList.Rows)
                //{
                //    ((CheckBox)gvRow.FindControl("chkApprove")).Checked = true;
                //    ((CheckBox)gvRow.FindControl("chkApprove")).Enabled = false;
                //}
                //((CheckBox)gvGatePassList.HeaderRow.FindControl("chkHApprove")).Checked = true;
                //((CheckBox)gvGatePassList.HeaderRow.FindControl("chkHApprove")).Enabled = false;
            }
            else
            {
                btnApprove.Visible = true;
                gvGatePassList.DataSource = Session["GATEPASS_LIST"] = dt;
                gvGatePassList.DataBind();
                lblRecordCount.Text = "Total Record Count : " + dt.Rows.Count.ToString();
            }
        }
        else
        {
            btnApprove.Visible = false;
            gvGatePassList.DataSource = null;
            gvGatePassList.DataBind();
            lblRecordCount.Text = "Total Record Count : 0";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no record found for selected criterian.');", true);
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetGatePassCount()
    {
        string GPCount = oDAL.GetGatePassCount(Session["COMPANY"].ToString());
        string[] str = GPCount.Split('^');
        lblUnApprovedGPCount.Text = str[0];
        lblApprovedGPCount.Text = str[1];
        lblUnApprovedGPCount.Visible = true;
        lblApprovedGPCount.Visible = true;
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Get gatepass details for selected criterian.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GetGatePassCount();
            GetGatePassDetails();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Reset/clear form fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {            
            lblLocLevel.Text = "1";
            PopulateLocation();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Refresh/reset location code/name details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshLocation_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateLocation();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApprove_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            bool bAssetSelected = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            else
            {
                for (int iCnt = 0; iCnt < gvGatePassList.Rows.Count; iCnt++)
                {
                    if (((CheckBox)gvGatePassList.Rows[iCnt].FindControl("chkApprove")).Checked == true)
                    {
                        bAssetSelected = true;
                        break;
                    }
                }
                if (!bAssetSelected)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select at least one gatepass.');", true);
                    return;
                }
                bool bResp = false;
                foreach (GridViewRow gvRow in gvGatePassList.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("chkApprove")).Checked)
                    {
                        oPRP.GatePassCode = ((Label)gvRow.FindControl("lblGPCode")).Text.Trim();
                        bResp = oDAL.ApproveGatePassDetails(oPRP.GatePassCode);
                    }
                }
                if (bResp)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Selected gatepass(s) approved successfully.');", true);
                GetGatePassDetails();
                GetGatePassCount();
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    #region SELECTEDINDEXCHANGED EVENT
    /// <summary>
    /// Get location code/name based on parent location name selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlGPLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlGPLocation.SelectedIndex != 0)
            {
                //int locLevel = int.Parse(lblLocLevel.Text.Trim());
                //lblLocLevel.Text = (locLevel + 1).ToString();
                //int iLocLevel = int.Parse(lblLocLevel.Text.Trim());
                string sLocCode = ddlGPLocation.SelectedValue.ToString();
                lblLocCode.Text = ddlGPLocation.SelectedValue.ToString();

                //ddlGPLocation.DataSource = null;
                //DataTable dt = oDAL.GetLocation(Session["COMPANY"].ToString(), sLocCode, iLocLevel);
                //if (dt.Rows.Count > 0)
                //{
                //    ddlGPLocation.DataSource = dt;
                //    ddlGPLocation.DataValueField = "LOC_CODE";
                //    ddlGPLocation.DataTextField = "LOC_NAME";
                //    ddlGPLocation.DataBind();
                //    ddlGPLocation.Items.Insert(0, "-- Select Location --");
                //    ddlGPLocation.Focus();
                //}
                //else
                //{
                //    iLocLevel = iLocLevel - 1;
                //    lblLocLevel.Text = iLocLevel.ToString();
                //    rdoApproved.Focus();
                //}
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
    /// Page index changing event for gatepass view gridview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvGatePassList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["GATEPASS_LIST"];
            gvGatePassList.PageIndex = e.NewPageIndex;
            gvGatePassList.DataSource = dt;
            gvGatePassList.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Redirects to generate gatepass page in order to view/print gatepass details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvGatePassList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "View")
            {
                string _GatePassCode = e.CommandArgument.ToString();
                Response.Redirect("GenerateGatePass.aspx?GatePassCode=" + _GatePassCode.ToString(), false);
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
    protected void gvGatePassList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            else
            {
                GridViewRow gvRow = (GridViewRow)gvGatePassList.Rows[e.RowIndex];
                oPRP.GatePassCode = ((Label)gvRow.FindControl("lblGPCode")).Text.Trim();
                oPRP.CompCode = Session["COMPANY"].ToString();
                string DelRslt = oDAL.DeleteGatePassDetails(oPRP.GatePassCode, oPRP.CompCode);
                if (DelRslt == "ALLOCATED")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Gate pass cannot be deleted.');", true);
                    return;
                }
                if (DelRslt == "SUCCESS")
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Gate pass details deleted successfully.');", true);
                //GetGatePassNo();
                GetGatePassCount();
                GetGatePassDetails();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Checkbox checked/unchecked javascript event addition
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvGatePassList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //try
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow &&
        //       (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        //    {
        //        CheckBox chkApprove = (CheckBox)e.Row.Cells[6].FindControl("chkApprove");
        //        CheckBox chkHApprove = (CheckBox)this.gvGatePassList.HeaderRow.FindControl("chkHApprove");
        //        chkApprove.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkHApprove.ClientID);
        //    }
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
    }
    #endregion
}