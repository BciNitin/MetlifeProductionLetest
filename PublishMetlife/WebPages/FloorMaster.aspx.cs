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

public partial class WebPages_FloorMaster : System.Web.UI.Page
{

    #region PAGE CONSTRUCTOR & DECLARATIONS

    FloorMaster_DAL oDAL;
    FloorMaster_PRP oPRP;
    public WebPages_FloorMaster()
    {
        oPRP = new FloorMaster_PRP();
    }
    ~WebPages_FloorMaster()
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
        oDAL = new FloorMaster_DAL(Session["DATABASE"].ToString());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("FLOOR_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "FLOOR_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }

                GetFloorDetails();
                GetSiteDetails();
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
        clsGeneral.LogErrorToLogFile(ex, "Floor Master");
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
    private void GetFloorDetails()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetFloor(Session["COMPANY"].ToString());
        gvFloorMaster.DataSource = Session["FLOORMASTER"] = dt;
        gvFloorMaster.DataBind();
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

            oPRP.FloorCode = txtFloorCode.Text.Trim().ToUpper();
            oPRP.FloorName = txtFloorName.Text.Trim().ToUpper();
            oPRP.SiteCode = ddlSite.SelectedValue;
            oPRP.Remarks = txtRemarks.Text.Trim();
            oPRP.CreatedBy = Convert.ToString(Session["CURRENTUSER"]);
            oPRP.Active = chkSetStatus.Checked;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            btnSubmit.Enabled = false;

            bool bResp = oDAL.SaveUpdateFloor("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate Floor Code.');", true);
                txtFloorCode.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            GetFloorDetails();
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
            if (gvFloorMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["FLOORMASTER"];
                dt.TableName = "FloorMasterReport";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                ////hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'> Floor Master</font></u></b>");
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

    protected void gvFloorMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvFloorMaster.Rows[e.RowIndex];
            oPRP.FloorCode = ((Label)gvRow.FindControl("lblFloorCode")).Text.Trim();
            oPRP.FloorName = ((Label)gvRow.FindControl("lblFloorName")).Text.Trim();
            oPRP.SiteCode = ((Label)gvRow.FindControl("lblSiteName")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkActive")).Checked;
            oPRP.Remarks = ((Label)gvRow.FindControl("lblDescription")).Text.Trim();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            string DelRslt = oDAL.DeleteFloor(oPRP);

            if (DelRslt == "SUCCESS")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Floor is deleted successfully.');", true);
            GetFloorDetails();

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvFloorMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            gvFloorMaster.EditIndex = e.NewEditIndex;
            GetFloorDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvFloorMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvFloorMaster.Rows[e.RowIndex];
            oPRP.FloorCode = ((Label)gvRow.FindControl("lblEFloorCode")).Text.Trim().ToUpper();
            oPRP.FloorName = ((Label)gvRow.FindControl("lblFloorName")).Text.Trim().ToUpper();
            oPRP.SiteCode = ((Label)gvRow.FindControl("lblSiteName")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
            oPRP.Remarks = ((TextBox)gvRow.FindControl("txtERemarks")).Text.Replace("'", "`").Trim();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oDAL.SaveUpdateFloor("UPDATE", oPRP);

            gvFloorMaster.EditIndex = -1;
            GetFloorDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvFloorMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvFloorMaster.EditIndex = -1;
            GetFloorDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvFloorMaster_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    //if (hfAgentID != null)
                    //{
                    //    if (e.Row.RowType == DataControlRowType.DataRow)
                    //    {
                    //        DropDownList ddlAgent = (DropDownList)gvRow.FindControl("ddlESite");
                    //        DataTable dt = new DataTable();
                    //        ddlAgent.DataSource = null;
                    //        dt = oDAL.GetSite();
                    //        ddlAgent.DataSource = dt;
                    //        ddlAgent.DataTextField = "SITE_ADDRESS";
                    //        ddlAgent.DataValueField = "SITE_CODE";
                    //        ddlAgent.DataBind();
                    //        //ddlAgent.Items.Insert(0, "-- Select Site --");

                    //        ddlAgent.SelectedValue = hfAgentID.Value;
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

    protected void gvFloorMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvFloorMaster.PageIndex = e.NewPageIndex;
            GetFloorDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    #endregion

}