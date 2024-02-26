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

public partial class EmployeeMaster : System.Web.UI.Page
{
    EmployeeMaster_DAL oDAL;
    EmployeeMaster_PRP oPRP;
    public EmployeeMaster()
    {
        oPRP = new EmployeeMaster_PRP();
    }
    ~EmployeeMaster()
    {
        oPRP = null; oDAL = null;
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
        oDAL = new EmployeeMaster_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking user group rights for employee master details viewing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("EMPLOYEE_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "EMPLOYEE_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                PopulateProcess(Session["COMPANY"].ToString());
                PopulateEmloyee(Session["COMPANY"].ToString());
                GetEmployeeDetails(Session["COMPANY"].ToString());
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
    //private void PopulateCompany()
    //{
    //    ddlCompany.DataSource = null;
    //    DataTable dt = oDAL.GetCompany();
    //    ddlCompany.DataSource = dt;
    //    ddlCompany.DataTextField = "COMP_NAME";
    //    ddlCompany.DataValueField = "COMP_CODE";
    //    ddlCompany.DataBind();
    //    ddlCompany.Items.Insert(0, "-- Select Company --");
    //}

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Employee Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Get department details.
    /// </summary>
    private void PopulateProcess(string CompCode)
    {
        ddlProcess.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetProcess(CompCode);
        ddlProcess.DataSource = dt;
        ddlProcess.DataValueField = "PROCESS_CODE";
        ddlProcess.DataTextField = "PROCESS_NAME";
        ddlProcess.DataBind();
        ddlProcess.Items.Insert(0, "-- Select Process --");
    }

    /// <summary>
    /// Populate employee details.
    /// </summary>
    private void PopulateEmloyee(string CompCode)
    {
        ddlReportingTo.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetEmpoyee(CompCode);
        ddlReportingTo.DataSource = dt;
        ddlReportingTo.DataValueField = "EMPLOYEE_CODE";
        ddlReportingTo.DataTextField = "EMPLOYEE_NAME";
        ddlReportingTo.DataBind();
        ddlReportingTo.Items.Insert(0, "-- Select Emlpoyee --");
    }

    /// <summary>
    /// Get employee details to be populated into gridview.
    /// </summary>
    private void GetEmployeeDetails(string CompCode)
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetEmpoyeeDetails(CompCode);
        gvEmpMaster.DataSource = Session["EMPLOYEE"] = dt;
        gvEmpMaster.DataBind();
    }
    #endregion

    #region SUBMIT EVENT
    /// <summary>
    /// Save employee details into employee master table.
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
            if (txtDateofJoin.Text.Trim() != "")
            {
                int iDate = clsGeneral.CompareDate(txtDateofJoin.Text.Trim(), DateTime.Now.ToString("dd/MMM/yyyy"));
                if (iDate > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Joining Date cannot be be later than current date!');", true);
                    return;
                }
            }
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.EmpCode = txtEmployeeCode.Text.Trim().ToUpper();
            if (!oDAL.CheckDuplicateEmailID(txtEmailID.Text.Trim(), oPRP.EmpCode, oPRP.CompCode))
            {
                oPRP.EmpName = txtEmployeeName.Text.Trim();
                oPRP.EmpCompCode = Session["COMPANY"].ToString();
                oPRP.EmpProcCode = (ddlProcess.SelectedIndex != 0) ? ddlProcess.SelectedValue.ToString() : "";
                oPRP.EmpReprotTo = ddlReportingTo.SelectedValue.ToString() != "-- Select Emlpoyee --" ? ddlReportingTo.SelectedValue.ToString() : "";
                oPRP.EmpEmail = txtEmailID.Text.Trim();
                oPRP.EmpDOJ = txtDateofJoin.Text.Trim();
                oPRP.EmpPhone = txtPhone.Text.Trim();
                oPRP.Active = chkSetStatus.Checked;
                oPRP.EmpRemarks = txtRemarks.Text.Replace("'", "`").Trim();
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                bool bResp = oDAL.SaveUpdateEmployee("SAVE", oPRP);
                if (!bResp)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate Employee Code');", true);
                    txtEmployeeCode.Focus();
                }
                else
                {
                    GetEmployeeDetails(Session["COMPANY"].ToString());
                    PopulateEmloyee(Session["COMPANY"].ToString());
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Email ID already exists.');", true);
                txtEmailID.Focus();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export employee master gridview data to excel file.
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
            if (gvEmpMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["EMPLOYEE"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=EmployeeMaster.xls");  
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
    /// Delete employee from employee master (if the employee doesn't have reporting to employee).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEmpMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            GridViewRow gvRow = gvEmpMaster.Rows[e.RowIndex];
            oPRP.EmpCode = ((Label)(gvRow.FindControl("lblEmpCode"))).Text.Trim();
            string DelRslt = oDAL.DeleteEmployee(oPRP.EmpCode, Session["COMPANY"].ToString());
            if (DelRslt == "CHILD_FOUND")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : One or more employee(s) report to him/her, hence cannot be deleted.');", true);
                return;
            }
            if (DelRslt == "ASSET_ALLOCATED")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : An asset is allocated to the employee which is not returned yet, hence cannot be deleted.');", true);
                return;
            }
            if (DelRslt == "SUCCESS")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee deleted successfully.');", true);
            GetEmployeeDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get employee details into edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEmpMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvEmpMaster.Rows[e.NewEditIndex];
            ViewState["PROC"] = ((Label)gvRow.FindControl("lblProc")).Text.Trim();
            ViewState["REPTO"] = ((Label)gvRow.FindControl("lblReptTo")).Text.Trim();

            gvEmpMaster.EditIndex = e.NewEditIndex;
            GetEmployeeDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Update employee details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEmpMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to execute this operation!!');", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvEmpMaster.Rows[e.RowIndex];
            if (!oDAL.CheckDuplicateEmailID(((TextBox)gvRow.FindControl("txtEEMail")).Text.Trim(), ((Label)gvRow.FindControl("lblEEmpCode")).Text.Trim(), Session["COMPANY"].ToString()))
            {
                oPRP.EmpCode = ((Label)gvRow.FindControl("lblEEmpCode")).Text.Trim();
                oPRP.EmpName = ((TextBox)gvRow.FindControl("txtEEmpName")).Text.Trim();

                if (((DropDownList)gvRow.FindControl("ddlEProc")).SelectedValue.ToString() != "-- Select Process --")
                    oPRP.EmpProcCode = ((DropDownList)gvRow.FindControl("ddlEProc")).SelectedValue.ToString();
                else
                    oPRP.EmpProcCode = "";
                if (ViewState["REPTO"].ToString() != "")
                {
                    if (((DropDownList)gvRow.FindControl("ddlEReptTo")).SelectedValue.ToString() != "-- Select Employee --")
                        oPRP.EmpReprotTo = ((DropDownList)gvRow.FindControl("ddlEReptTo")).SelectedValue.ToString();
                    else
                        oPRP.EmpReprotTo = "";
                }
                else if (ViewState["REPTO"].ToString() == "")
                {
                    if (((DropDownList)gvRow.FindControl("ddlEReptTo")).SelectedValue.ToString() != "-- Select Employee --")
                        oPRP.EmpReprotTo = ((DropDownList)gvRow.FindControl("ddlEReptTo")).SelectedValue.ToString();
                    else
                        oPRP.EmpReprotTo = "";
                }
                if (oPRP.EmpReprotTo != "")
                {
                    if (oPRP.EmpCode == oPRP.EmpReprotTo)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee cannot report to himself.');", true);
                        return;
                    }
                }
                oPRP.EmpEmail = ((TextBox)gvRow.FindControl("txtEEMail")).Text.Trim();
                oPRP.EmpPhone = ((TextBox)gvRow.FindControl("txtEPhone")).Text.Trim();
                oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
                oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
                oPRP.EmpCompCode = Session["COMPANY"].ToString();
                oPRP.CompCode = Session["COMPANY"].ToString();
                bool bRslt = oDAL.SaveUpdateEmployee("UPDATE", oPRP);
                if (bRslt)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                    gvEmpMaster.EditIndex = -1;
                    GetEmployeeDetails(Session["COMPANY"].ToString());
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Duplicate Email ID.');", true);
                ((TextBox)gvRow.FindControl("txtEEMail")).Focus();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Binding dropdowns inside gridview when in edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEmpMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlECompany = (DropDownList)e.Row.FindControl("ddlECompany");
                if (ddlECompany != null)
                {
                    ddlECompany.DataSource = oDAL.GetCompany();
                    ddlECompany.DataTextField = "COMP_NAME";
                    ddlECompany.DataValueField = "COMP_CODE";
                    ddlECompany.DataBind();
                    ddlECompany.Items.Insert(0, "SELECT");
                    if (ViewState["COMPANY"].ToString() != "")
                        ddlECompany.SelectedIndex = ddlECompany.Items.IndexOf(ddlECompany.Items.FindByText(ViewState["COMPANY"].ToString()));
                    else
                    {
                        ddlECompany.SelectedIndex = ddlECompany.Items.IndexOf(ddlECompany.Items.FindByText("SELECT"));
                    }
                }

                DropDownList ddlEProc = (DropDownList)e.Row.FindControl("ddlEProc");
                if (ddlEProc != null)
                {
                    ddlEProc.DataSource = oDAL.GetProcess(Session["COMPANY"].ToString());
                    ddlEProc.DataTextField = "PROCESS_NAME";
                    ddlEProc.DataValueField = "PROCESS_CODE";
                    ddlEProc.DataBind();
                    ddlEProc.Items.Insert(0, "-- Select Process --");
                    if (ViewState["PROC"].ToString() != "")
                        ddlEProc.SelectedIndex = ddlEProc.Items.IndexOf(ddlEProc.Items.FindByText(ViewState["PROC"].ToString()));
                    else
                    {
                        ddlEProc.SelectedIndex = ddlEProc.Items.IndexOf(ddlEProc.Items.FindByText("SELECT"));
                    }
                }

                DropDownList ddlEReptTo = (DropDownList)e.Row.FindControl("ddlEReptTo");
                if (ddlEReptTo != null)
                {
                    ddlEReptTo.DataSource = oDAL.GetEmpoyee(Session["COMPANY"].ToString());
                    ddlEReptTo.DataTextField = "EMPLOYEE_NAME";
                    ddlEReptTo.DataValueField = "EMPLOYEE_CODE";
                    ddlEReptTo.DataBind();
                    ddlEReptTo.Items.Insert(0, "-- Select Employee --");
                    if (ViewState["REPTO"].ToString() != "")
                        ddlEReptTo.SelectedIndex = ddlEReptTo.Items.IndexOf(ddlEReptTo.Items.FindByValue(ViewState["REPTO"].ToString()));
                    else
                    {
                        ddlEReptTo.SelectedIndex = ddlEReptTo.Items.IndexOf(ddlEReptTo.Items.FindByValue("SELECT"));
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
    /// Cancel employee details from edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEmpMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvEmpMaster.EditIndex = -1;
            GetEmployeeDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Employee master girdview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEmpMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvEmpMaster.PageIndex = e.NewPageIndex;
            GetEmployeeDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}