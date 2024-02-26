using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class ProcessMaster : System.Web.UI.Page
{
    ProcessMaster_DAL oDAL;
    ProcessMaster_PRP oPRP;
    public ProcessMaster()
    {
        oPRP = new ProcessMaster_PRP();
    }
    ~ProcessMaster()
    {
        oDAL = null; oPRP = null;
    }

    #region PAGE EVENTS
    /// <summary>
    /// Navigates to session expired page in case of user logs off
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new ProcessMaster_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user rights for process master operations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["CURRENTUSER"].ToString() != "test")
                {
                    string _strRights = clsGeneral.GetRights("PROCESS_MASTER", (DataTable)Session["UserRights"]);
                    clsGeneral._strRights = _strRights.Split('^');
                    clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "PROCESS_MASTER");
                    if (clsGeneral._strRights[0] == "0")
                    {
                        Response.Redirect("UnauthorizedUser.aspx", false);
                    }
                }
                //PopulateEmployee(Session["COMPANY"].ToString());
                PopulateDepartment(Session["COMPANY"].ToString());
                GetProcessDetails(Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Populate employee code/name for process owner selection.
    /// </summary>
    private void PopulateEmployee(string CompCode)
    {
        //ddlProcOwner.DataSource = null;
        //DataTable dt = new DataTable();
        //dt = oDAL.GetEmployee(CompCode);
        //ddlProcOwner.DataSource = dt;
        //ddlProcOwner.DataValueField = "EMPLOYEE_CODE";
        //ddlProcOwner.DataTextField = "EMPLOYEE_NAME";
        //ddlProcOwner.DataBind();
        //ddlProcOwner.Items.Insert(0, "-- Select Owner --");
    }

    /// <summary>
    /// Populate department code/name from dropdown selection.
    /// </summary>
    private void PopulateDepartment(string CompCode)
    {
        ddlDepartment.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetDepartment(CompCode);
        ddlDepartment.DataSource = dt;
        ddlDepartment.DataValueField = "DEPT_CODE";
        ddlDepartment.DataTextField = "DEPT_NAME";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, "-- Select Department --");
    }

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Process Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Populate process master details into gridview.
    /// </summary>
    private void GetProcessDetails(string CompCode)
    {
        gvProcessMaster.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetProcessDetails(CompCode);
        gvProcessMaster.DataSource = Session["PROCESS"] = dt;
        gvProcessMaster.DataBind();
    }
    #endregion

    #region SUBMIT EVENT
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (ddlDepartment.SelectedIndex != 0)
                oPRP.DeptCode = ddlDepartment.SelectedValue.ToString();
            oPRP.ProcessCode = txtProcessCode.Text.Trim().ToUpper();
            oPRP.ProcessName = txtProcessName.Text.Trim();
            //oPRP.ProcessOwner = txtProcessOwner.Text.Trim();
            oPRP.Active = chkSetStatus.Checked;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            bool bResp = oDAL.SaveUpdateProcess("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate Process Code cannot be saved.');", true);
                txtProcessCode.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            GetProcessDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
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
            if (gvProcessMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["PROCESS"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'> Process Master</font></u></b>");
                //hw.WriteLine("<br>");
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
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Gets Process details in delete mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvProcessMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = gvProcessMaster.Rows[e.RowIndex];
            oPRP.ProcessCode = ((Label)(gvRow.FindControl("lblProcCode"))).Text.Trim();
            string DelRslt = oDAL.DeleteProcess(oPRP.ProcessCode, Session["COMPANY"].ToString());
            if (DelRslt == "PROCESS_MAPPED")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg",
                   "ShowErrMsg('Please Note : Process is mapped with employees, hence cannot be deleted.');", true);
                return;
            }
            if (DelRslt == "ASSET_MAPPED")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg",
                   "ShowErrMsg('Please Note : Process is mapped with assets, hence cannot be deleted.');", true);
                return;
            }
            if (DelRslt == "SUCCESS")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg",
                       "ShowErrMsg('Please Note : Process deleted successfully.');", true);
            GetProcessDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Gets Process details in edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvProcessMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvProcessMaster.Rows[e.NewEditIndex];
            ViewState["DEPT"] = ((Label)gvRow.FindControl("lblDeptCode")).Text.Trim();
            gvProcessMaster.EditIndex = e.NewEditIndex;
            GetProcessDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    
    /// <summary>
    /// Process details can be updated
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvProcessMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = gvProcessMaster.Rows[e.RowIndex];
            oPRP.DeptCode = ((DropDownList)(gvRow.FindControl("ddlEDeptCode"))).SelectedValue.ToString();
            oPRP.ProcessCode = ((Label)(gvRow.FindControl("lblEProcCode"))).Text.Trim();
            oPRP.ProcessName = ((TextBox)(gvRow.FindControl("txtEProcName"))).Text.Trim();
            //if (((DropDownList)(gvRow.FindControl("ddlEProcOwner"))).SelectedValue.ToString() != "-- Select Owner --")
            //    oPRP.ProcessOwner = ((DropDownList)(gvRow.FindControl("ddlEProcOwner"))).SelectedValue.ToString();
            //else
            //    oPRP.ProcessOwner = "";
            oPRP.Active = ((CheckBox)(gvRow.FindControl("chkEditActive"))).Checked;
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oDAL.SaveUpdateProcess("UPDATE", oPRP);

            gvProcessMaster.EditIndex = -1;
            GetProcessDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Process details are cancelled from edit/update/delete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvProcessMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvProcessMaster.EditIndex = -1;
            GetProcessDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    
    /// <summary>
    /// populate Process owner dropdownlist inside gridview for edit operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvProcessMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlEProcOwner = (DropDownList)e.Row.FindControl("ddlEProcOwner");
                if (ddlEProcOwner != null)
                {
                    ddlEProcOwner.DataSource = oDAL.GetEmployee(Session["COMPANY"].ToString());
                    ddlEProcOwner.DataTextField = "EMPLOYEE_NAME";
                    ddlEProcOwner.DataValueField = "EMPLOYEE_CODE";
                    ddlEProcOwner.DataBind();
                    ddlEProcOwner.Items.Insert(0, "-- Select Owner --");
                    if (ViewState["OWNER"].ToString() != "")
                        ddlEProcOwner.SelectedIndex = ddlEProcOwner.Items.IndexOf(ddlEProcOwner.Items.FindByText(ViewState["OWNER"].ToString()));
                    else
                    {
                        ddlEProcOwner.SelectedIndex = ddlEProcOwner.Items.IndexOf(ddlEProcOwner.Items.FindByText("-- Select Owner --"));
                    }
                }

                DropDownList ddlEDeptCode = (DropDownList)e.Row.FindControl("ddlEDeptCode");
                if (ddlEDeptCode != null)
                {
                    ddlEDeptCode.DataSource = oDAL.GetDepartment(Session["COMPANY"].ToString());
                    ddlEDeptCode.DataTextField = "DEPT_NAME";
                    ddlEDeptCode.DataValueField = "DEPT_CODE";
                    ddlEDeptCode.DataBind();
                    ddlEDeptCode.Items.Insert(0, "-- Select Department --");
                    if (ViewState["DEPT"].ToString() != "")
                        ddlEDeptCode.SelectedIndex = ddlEDeptCode.Items.IndexOf(ddlEDeptCode.Items.FindByText(ViewState["DEPT"].ToString()));
                    else
                    {
                        ddlEDeptCode.SelectedIndex = ddlEDeptCode.Items.IndexOf(ddlEDeptCode.Items.FindByText("-- Select Owner --"));
                    }
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
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Process master gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvProcessMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvProcessMaster.PageIndex = e.NewPageIndex;
            GetProcessDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}