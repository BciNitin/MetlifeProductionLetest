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

public partial class DepartmentMaster : System.Web.UI.Page
{
    DeptMaster_DAL oDAL;
    DeptMaster_PRP oPRP;
    public DepartmentMaster()
    {
        oPRP = new DeptMaster_PRP();
    }
    ~DepartmentMaster()
    {
        oDAL = null; oPRP = null;
    }

    #region PAGE EVENTS
    /// <summary>
    /// Navigate to session expired page when user session is expired.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new DeptMaster_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Department master page load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("DEPARTMENT_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "DEPARTMENT_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                GetDepartmentDetails(Session["COMPANY"].ToString());
                txtDeptCode.Focus();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Handle un-handled exceptions and send to error page.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Department Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetDepartmentDetails(string CompCode)
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetDept(CompCode);
        gvDeptMaster.DataSource = Session["DeptMaster"] = dt;
        gvDeptMaster.DataBind();
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Delete a department from department master if not associated with any process.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDeptMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvDeptMaster.Rows[e.RowIndex];
            oPRP.DeptCode = ((Label)gvRow.FindControl("lblDeptCode")).Text.Trim();
            string DelRslt = oDAL.DeleteDept(oPRP.DeptCode, Session["COMPANY"].ToString());
            if (DelRslt == "PROCESS_MAPPED")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg",
                    "ShowErrMsg('Department is mapped with process(s), hence cannot be deleted.');", true);
                return;
            }
            if (DelRslt == "SUCCESS")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Department deleted successfully.');", true);
            GetDepartmentDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get department details into edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDeptMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            gvDeptMaster.EditIndex = e.NewEditIndex;
            GetDepartmentDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDeptMaster_RowDataBound(object sender, GridViewRowEventArgs e)
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
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Update department details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDeptMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvDeptMaster.Rows[e.RowIndex];
            oPRP.DeptCode = ((Label)gvRow.FindControl("lblEDeptCode")).Text.Trim();
            oPRP.DeptName = ((TextBox)gvRow.FindControl("txtEDeptName")).Text.Trim();
            oPRP.Remarks = ((TextBox)gvRow.FindControl("txtERemarks")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oDAL.SaveUpdateDept("UPDATE", oPRP);

            gvDeptMaster.EditIndex = -1;
            GetDepartmentDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Cancel edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDeptMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvDeptMaster.EditIndex = -1;
            GetDepartmentDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Department details grid page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDeptMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvDeptMaster.PageIndex = e.NewPageIndex;
            gvDeptMaster.DataSource = (DataTable)Session["DeptMaster"];
            gvDeptMaster.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region SUBMIT EVENT
    /// <summary>
    /// Save department master details.
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
            btnSubmit.Enabled = false;
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.DeptCode = txtDeptCode.Text.Trim().ToUpper();
            oPRP.DeptName = txtDeptName.Text.Trim();
            oPRP.Remarks = txtRemarks.Text.Trim();
            oPRP.Active = chkSetStatus.Checked;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            bool bResp = oDAL.SaveUpdateDept("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate Department Code.');", true);
                txtDeptCode.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            GetDepartmentDetails(Session["COMPANY"].ToString());
            btnSubmit.Enabled = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export department master data to excel file.
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
            if (gvDeptMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["DeptMaster"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'> Department Master</font></u></b>");
                //hw.WriteLine("<br>");
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=DepartmentMaster.xls");  
                //this.EnableViewState = false;
                //Response.Write(tw.ToString());
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
                //Response.End();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}