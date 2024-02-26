using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPages_TrainingorMeetingMaster : System.Web.UI.Page
{

    #region PAGE CONSTRUCTOR & DECLARATIONS

    TrainingorMeetingMaster_DAL oDAL;
    TrainingorMeetingMaster_PRP oPRP;
    public WebPages_TrainingorMeetingMaster()
    {
        oPRP = new TrainingorMeetingMaster_PRP();
    }
    ~WebPages_TrainingorMeetingMaster()
    {
        oPRP = null; oDAL = null;
    }

    #endregion

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
        oDAL = new TrainingorMeetingMaster_DAL(Session["DATABASE"].ToString());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("TRAINING_OR_MEETING_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "TRAINING_OR_MEETING_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                GetTrainingandMeetingRoomDetails();
                GetSiteDetails();
                Loadfloor();
                GetMasterTypeDetails();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    #endregion

    #region PRIVATE FUNCTIONS
    
    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Store Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Get employee details to be populated into gridview.
    /// </summary>
    private void GetTrainingandMeetingRoomDetails()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetTrainingandMeetingType(Session["COMPANY"].ToString());
        gvTrainingorMeetingMaster.DataSource = Session["TRAININGORMEETINGMASTER"] = dt;
        gvTrainingorMeetingMaster.DataBind();
    }
    private void GetSiteDetails() 
    {
        DataTable dt = new DataTable();
        ddlSite.DataSource = null;
        dt = oDAL.GetSite(Session["COMPANY"].ToString());
        ddlSite.DataSource = dt;
        ddlSite.DataTextField = "SITE_CODE";
        ddlSite.DataValueField = "SITE_CODE";
        ddlSite.DataBind();
        ddlSite.Items.Insert(0, "-- Select Site --");
    }

    private void GetMasterTypeDetails()
    {
        ddlMasterType.DataSource = null;
        ddlMasterType.DataBind();
        ddlMasterType.Items.Insert(0, "-- Select Master Type --");
        ddlMasterType.Items.Insert(1, "TRAINING ROOM");
        ddlMasterType.Items.Insert(2, "MEETING ROOM");
    }
    private void Loadfloor()
    {
        ddlFloor.DataSource = null;
        ddlFloor.DataBind();
        ddlFloor.Items.Insert(0, "-- Select Floor --");
    }

    private void GetFloorDropdown(string SiteCode)
    {
        DataTable dt = new DataTable();
        ddlFloor.DataSource = null;
        dt = oDAL.GetFloor(SiteCode, Session["COMPANY"].ToString());
        ddlFloor.DataSource = dt;
        ddlFloor.DataTextField = "FLOOR_CODE";
        ddlFloor.DataValueField = "FLOOR_CODE";
        ddlFloor.DataBind();
        ddlFloor.Items.Insert(0, "-- Select Floor --");
    }

    #endregion

    #region BUTTON EVENTS

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            
            
            //oPRP.StoreCode = txtStoreCode.Text.Trim().ToUpper();
            oPRP.MasterCode= oPRP.MasterName = txtMasterName.Text.Trim().ToUpper();
            oPRP.SiteCode = ddlSite.SelectedValue;
            oPRP.Floor = ddlFloor.SelectedValue;
            oPRP.MasterType = ddlMasterType.SelectedItem.Text.ToString();
            oPRP.Remarks = txtRemarks.Text.Trim();
            oPRP.CreatedBy = Convert.ToString(Session["CURRENTUSER"]);

            oPRP.Active = chkSetStatus.Checked;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();

            if (oDAL.CheckDuplicatateMeetingorTrainingFloorSite(oPRP.MasterName,oPRP.MasterType, oPRP.Floor, oPRP.SiteCode,oPRP.CompCode))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate Training or Meeting Room.');", true);
                txtMasterName.Focus();
                return;
            }
            btnSubmit.Enabled = false;
            bool bResp = oDAL.SaveUpdateTrainingorMeetingMaster("SAVE",oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Invalid Training or Meeting Room.');", true);
                txtMasterName.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            GetTrainingandMeetingRoomDetails();
            btnSubmit.Enabled = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {

    }
    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[4] == "0")
            {
                Response.Redirect("UnauthorizedUser.aspx");
            }
            if (gvTrainingorMeetingMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["TRAININGORMEETINGMASTER"];
                dt.TableName = "TrainingOrMeetingMasterReport";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                ////hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'> Store Master</font></u></b>");
                //hw.WriteLine("<br>");
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=SiteMaster.xls");
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

    #endregion

    #region GRID EVENTS

    protected void gvTrainingorMeetingMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvTrainingorMeetingMaster.Rows[e.RowIndex];            
            oPRP.MasterCode = ((Label)gvRow.FindControl("lblMasterCode")).Text.Trim();
            oPRP.MasterName = ((Label)gvRow.FindControl("lblMasterName")).Text.Trim();
            oPRP.MasterType = ((Label)gvRow.FindControl("lblMasterType")).Text.Trim();
            oPRP.Floor = ((Label)gvRow.FindControl("lblFloor")).Text.Trim();
            oPRP.SiteCode = ((Label)gvRow.FindControl("lblSiteName")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkActive")).Checked;
            oPRP.Remarks = ((Label)gvRow.FindControl("lblDescription")).Text.Trim();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            string DelRslt = oDAL.DeleteTrainingandMeetingMaster(oPRP);

            if (DelRslt == "SUCCESS")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Training or Meeting Master is deleted successfully.');", true);
            GetTrainingandMeetingRoomDetails();

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvTrainingorMeetingMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            gvTrainingorMeetingMaster.EditIndex = e.NewEditIndex;
            GetTrainingandMeetingRoomDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvTrainingorMeetingMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvTrainingorMeetingMaster.Rows[e.RowIndex];
            oPRP.MasterCode = ((Label)gvRow.FindControl("lblMasterCode")).Text.Trim().ToUpper();
            //oPRP.MasterCode = ((Label)gvRow.FindControl("lblEMasterCode")).Text.Trim();
            oPRP.MasterName = ((Label)gvRow.FindControl("lblMasterName")).Text.Trim().ToUpper();
            oPRP.MasterType = ((Label)gvRow.FindControl("lblMasterType")).Text.Trim();
            oPRP.Floor = ((Label)gvRow.FindControl("lblFloor")).Text.Trim();
            oPRP.SiteCode = ((Label)gvRow.FindControl("lblSiteName")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
            oPRP.Remarks = ((TextBox)gvRow.FindControl("txtERemarks")).Text.Replace("'", "`").Trim();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oDAL.SaveUpdateTrainingorMeetingMaster("UPDATE", oPRP);

            gvTrainingorMeetingMaster.EditIndex = -1;
            GetTrainingandMeetingRoomDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvTrainingorMeetingMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvTrainingorMeetingMaster.EditIndex = -1;
            GetTrainingandMeetingRoomDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvTrainingorMeetingMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
                if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                {
                    GridViewRow gvRow = e.Row;
                    //HiddenField hfAgentID = (HiddenField)gvRow.FindControl("hidESite");
                    //HiddenField hfAgentIDFloor = (HiddenField)gvRow.FindControl("hidEFloor");
                    //HiddenField hfMasterType = (HiddenField)gvRow.FindControl("hidEMasterType");
                    //if (hfAgentID != null)
                    //{
                    //    if (e.Row.RowType == DataControlRowType.DataRow)
                    //    {
                    //        DropDownList ddlAgent = (DropDownList)gvRow.FindControl("ddlESite");
                    //        DataTable dt = new DataTable();
                    //        ddlAgent.DataSource = null;
                    //        dt = oDAL.GetSite();
                    //        ddlAgent.DataSource = dt;
                    //        ddlAgent.DataTextField = "SITE_CODE";
                    //        ddlAgent.DataValueField = "SITE_CODE";
                    //        ddlAgent.DataBind();
                    //        ddlAgent.SelectedValue = hfAgentID.Value;

                    //        DropDownList ddlAgentFloor = (DropDownList)gvRow.FindControl("ddlEFloor");
                    //        DataTable dtfloor = new DataTable();
                    //        ddlAgentFloor.DataSource = null;
                    //        dtfloor = oDAL.GetFloor(ddlAgent.SelectedValue);
                    //        ddlAgentFloor.DataSource = dtfloor;
                    //        ddlAgentFloor.DataTextField = "FLOOR_CODE";
                    //        ddlAgentFloor.DataValueField = "FLOOR_CODE";
                    //        ddlAgentFloor.DataBind();
                    //        ddlAgentFloor.SelectedValue = hfAgentIDFloor.Value;

                    //        DropDownList ddlAgentMasterType = (DropDownList)gvRow.FindControl("ddlEMasterType");
                    //        ddlAgentMasterType.DataSource = null;
                    //        ddlAgentMasterType.DataBind();
                    //        ddlAgentMasterType.Items.Insert(0, "-- Select Master Type --");
                    //        ddlAgentMasterType.Items.Insert(1, "TRAINING ROOM");
                    //        ddlAgentMasterType.Items.Insert(2, "MEETING ROOM");
                    //        ddlAgentMasterType.SelectedValue = hfMasterType.Value;
                    //    }
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    protected void ddlESite_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAgent = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlAgent.NamingContainer;
        if (row != null)
        {
            DropDownList ddl = (DropDownList)row.FindControl("ddlEFloor");
            DataTable dt = new DataTable();
            ddl.DataSource = null;
            dt = oDAL.GetFloor(ddlAgent.SelectedValue,Session["COMPANY"].ToString());
            ddl.DataSource = dt;
            ddl.DataTextField = "FLOOR_CODE";
            ddl.DataValueField = "FLOOR_CODE";
            ddl.DataBind();
        }
    }
    protected void gvTrainingorMeetingMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvTrainingorMeetingMaster.PageIndex = e.NewPageIndex;
            GetTrainingandMeetingRoomDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    #endregion

    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetFloorDropdown(ddlSite.SelectedValue);
    }
}