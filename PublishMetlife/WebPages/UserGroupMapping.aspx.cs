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
using System.Text;
using System.IO;
using System.Globalization;

public partial class WebPages_UserGroupMapping : System.Web.UI.Page
{
    string _CompCode = "";
    GroupMaster_DAL oDAL;
    GroupMaster_PRP oPRP;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                string _strRights = clsGeneral.GetRights("USER_GROUP_MAPPING", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "GROUP_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                //GetGroupDetails(Session["COMPANY"].ToString());
                GetGroupMasterDetails();
                GetGroupDetails(Session["COMPANY"].ToString());
                PopulateLocation();
                //txtGroupCode.Focus();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new GroupMaster_DAL(Session["DATABASE"].ToString());
        _CompCode = Session["COMPANY"].ToString();
    }
    public WebPages_UserGroupMapping()
    {
        oPRP = new GroupMaster_PRP();
    }
    ~WebPages_UserGroupMapping()
    {
        oDAL = null; oPRP = null;
    }

    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "User Group Mapping");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }
    private void PopulateLocation()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetSite(Session["COMPANY"].ToString());
        if (dt.Rows.Count > 0) 
        {
            CheckBoxListLoc.Items.Clear();
            CheckBoxListLoc.DataSource = dt;
            CheckBoxListLoc.DataValueField = "SITE_CODE"; //column name in DT
            CheckBoxListLoc.DataTextField = "SITE_CODE";
            CheckBoxListLoc.DataBind();
        }
        ddlLocation.DataSource = null;
        ddlLocation.DataSource = dt;
        ddlLocation.DataValueField = "SITE_CODE";
        ddlLocation.DataTextField = "SITE_CODE";
        ddlLocation.DataBind();
        ddlLocation.Items.Insert(0, "-- Select Location --");
    }
    private void MakeEmptyString()
    {
        oPRP.UserName = string.Empty;
        oPRP.UserEmail = string.Empty;
        oPRP.GroupCode = string.Empty;
        oPRP.CompCode = Session["COMPANY"].ToString();
        oPRP.EmployeeID = string.Empty;
        //oPRP.Active = chkSetStatus.Checked;
        oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
        oPRP.GroupName = string.Empty;
        oPRP.GroupRemarks = string.Empty;
        oPRP.LocationCode = string.Empty;
    }

    #region BUTTON EVENTS
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        MakeEmptyString();
        string bResp = string.Empty;
        //if (clsGeneral._strRights[1] == "0")
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
        //    return;
        //}
        if(btnSubmit.Text == "Manage Employees to Group")
        {
            bool result = false;int i = 0; int Cnt = 0;
            foreach (GridViewRow gvRow in GvGroupEmployee.Rows)
            {
                if (gvRow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkEmpView = ((CheckBox)gvRow.FindControl("chkEmpView"));
                    if (chkEmpView.Checked)
                    {
                        oPRP.GroupCode = ((Label)gvRow.FindControl("lblGVGroupEMPGroupCode")).Text.Trim();
                        oPRP.GroupName = ((Label)gvRow.FindControl("lblGVGroupEMPGroupName")).Text.Trim();
                        oPRP.isExistGroupCode = ((Label)gvRow.FindControl("lblExistGroupCode")).Text.Trim();
                        oPRP.isExistGroupName = ((Label)gvRow.FindControl("lblExistGroupName")).Text.Trim();
                        oPRP.EmployeeID = ((Label)gvRow.FindControl("lblEmpCode")).Text.Trim();
                        result = oDAL.UpdateEmployeetoGroup(oPRP);
                        if (!result)
                        {
                            i++;
                            continue;
                        }
                        else
                            Cnt++;
                    }
                }
            }
            if (i == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertCommon", "ShowAlertCommon('Please Note : " + Cnt + " Employees Mapped with this Group.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : " + i + " Employees with Group is not mapped.');", true);
            }
        }
        else
        {
            int iCnt = 0;
            bool bView = false;
            DataTable dt = (DataTable)Session["Rights"];
            int iRowCnt = gvGroupRights.Rows.Count;
            int totalCount = gvGroupRights.Rows.Cast<GridViewRow>()
                            .Count(r => ((CheckBox)r.FindControl("chkView")).Checked);
            if(btnSubmit.Text!="Add Employee")
            {
                if (txtGroupCode.Text == "" || txtGroupCode.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Enter Group Code.');", true);
                    return;
                }
                if (txtGroupName.Text == "" || txtGroupName.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Enter Group Name.');", true);
                    return;
                }
                if (totalCount == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : No rights is assigned.');", true);
                    return;
                }
                if (iRowCnt == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Group Details are not available.');", true);
                    return;
                }
            }
            else
            {
                foreach (ListItem li in CheckBoxListLoc.Items)
                {
                    bool res = false;
                    // If the listitem is selected
                    if (li.Selected)
                    {
                        res = true;
                        break;
                    }
                    if(!res)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select Location.');", true);
                        return;
                    }
                }
                //if (ddlLocation.SelectedIndex == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select Location.');", true);
                //    return;
                //}
            }
            oPRP.GroupCode = txtGroupCode.Text.Trim().ToUpper();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.Active = chkSetStatus.Checked;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.GroupName = txtGroupName.Text.Trim().ToUpper();
            oPRP.GroupRemarks = txtGroupRemarks.Text.Trim();
            oPRP.BtnStatus = btnSubmit.Text;

            if (btnSubmit.Text == "Add Employee")
            {
                if (GVEmployee.Visible)
                {
                    iRowCnt = GVEmployee.Rows.Count;
                    if (iRowCnt == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee Details are not available.');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee Details are not available. Click on go button to find Employee Details.');", true);
                    return;
                }
                foreach (ListItem li in CheckBoxListLoc.Items)
                {
                    int Active = 0;
                    // If the listitem is selected
                    if (li.Selected)
                    {
                        oPRP.LocationCode = li.Value;
                        Active = 1;
                    }
                    DataTable dl = oDAL.SaveUserLocationMapping(txtEmployeeID.Text.Trim(), li.Value.Trim(), Session["COMPANY"].ToString(), Session["CURRENTUSER"].ToString(), Active, "Save");
                    if (dl.Rows.Count > 0)
                    {
                        string msg = dl.Rows[0][0].ToString();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : " + msg + " ');", true);
                    }
                }
                oPRP.UserID = txtEmployeeID.Text.Trim();
                //oPRP.LocationCode = ddlLocation.SelectedValue;
                oPRP.UserName = ((Label)GVEmployee.Rows[0].FindControl("lblEmpName")).Text;
                oPRP.UserEmail = ((Label)GVEmployee.Rows[0].FindControl("lblEmpEMail")).Text;
                oPRP.GroupCode = txtGroupCode.Text.Trim().ToUpper();
                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.EmployeeID = ((Label)GVEmployee.Rows[0].FindControl("lblEmpID")).Text.Trim();
                oPRP.Active = chkSetStatus.Checked;
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                oPRP.GroupName = txtGroupName.Text.Trim().ToUpper();
                oPRP.GroupRemarks = txtGroupRemarks.Text.Trim();
            }
            if (btnSubmit.Text == "Update")
            {
                if (oPRP.GroupCode == "SUPER ADMIN - IT" || txtGroupCode.Text == "SUPER ADMIN - IT" || oPRP.GroupCode == "SUPER ADMIN - CRE" || txtGroupCode.Text == "SUPER ADMIN - CRE")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to Update this group.');", true);
                    return;
                }
            }
            //if (oPRP.UserID != oPRP.EmployeeID)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee ID is not matched with employee details.');", true);
            //    return;
            //}
            if (btnSubmit.Text == "Update")
            {
                if (oPRP.GroupCode != Convert.ToString(dt.Rows[0]["GROUP_CODE"]))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Group Code is not matched with group rights details.');", true);
                    return;
                }
            }
            if (btnSubmit.Text == "Update" || btnSubmit.Text == "Save")
            {
                foreach (GridViewRow gvRow in gvGroupRights.Rows)
                {
                    if (gvRow.RowType == DataControlRowType.DataRow)
                    {
                        bView = ((CheckBox)gvRow.FindControl("chkView")).Checked;
                        dt.Rows[iCnt]["GROUP_CODE"] = txtGroupCode.Text;
                        dt.Rows[iCnt]["VIEW_RIGHTS"] = bView;
                        dt.Rows[iCnt]["SAVE_RIGHTS"] = bView;
                        dt.Rows[iCnt]["EDIT_RIGHTS"] = bView;
                        dt.Rows[iCnt]["DELETE_RIGHTS"] = bView;
                        dt.Rows[iCnt]["EXPORT_RIGHTS"] = bView;
                        iCnt++;
                        dt.AcceptChanges();
                    }
                }
            }
            
            if(btnSubmit.Text=="Save")
            {
                bool ChkDuplicate = oDAL.CheckDuplicateGroup(txtGroupCode.Text);
                if(ChkDuplicate)
                { ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertCommon", "ShowAlertCommon('Group Already Exist.');", true); return; }
            }
            
            bResp = oDAL.SaveUpdateGroupRights(dt, oPRP);
            if (bResp != "SUCCESS")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : '" + bResp + "'.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertCommon", "ShowAlertCommon('Saved Successfully.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            }
        }
        
    }
    protected void btnRemoveEmpOnGroup_Click(object sender, EventArgs e)
    {
        try
        {
            bool result = false; int i = 0; int Cnt = 0;
            foreach (GridViewRow gvRow in GvGroupEmployee.Rows)
            {
                if (gvRow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkEmpView = ((CheckBox)gvRow.FindControl("chkEmpView"));
                    if (chkEmpView.Checked)
                    {
                        oPRP.GroupCode = ((Label)gvRow.FindControl("lblGVGroupEMPGroupCode")).Text.Trim();
                        oPRP.GroupName = ((Label)gvRow.FindControl("lblGVGroupEMPGroupName")).Text.Trim();
                        oPRP.isExistGroupCode = ((Label)gvRow.FindControl("lblExistGroupCode")).Text.Trim();
                        oPRP.isExistGroupName = ((Label)gvRow.FindControl("lblExistGroupName")).Text.Trim();
                        oPRP.EmployeeID = ((Label)gvRow.FindControl("lblEmpCode")).Text.Trim();
                        result = oDAL.RemoveEmployeetoGroup(oPRP);
                        if (!result)
                        {
                            i++;
                            continue;
                        }
                        else
                            Cnt++;
                    }
                }
            }
            if (i == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertCommon", "ShowAlertCommon('Please Note : " + Cnt + " Employees Removed with this Group.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : " + i + " Employees cannot Remove from this Group.');", true);
            }
        }
        catch(Exception ex)
        { HandleExceptions(ex); }
    }

    private void GetEmployeeDetails(string empid)
    {
        lblErrorMsg.Text = "";
        GVEmployee.DataSource = null;
        GVEmployee.DataBind();
        DataTable dt = ADSearch.GetEmpDetails(Session["LDAPUser"].ToString(), Session["LDAPPassword"].ToString(), empid, Session["COMPANY"].ToString());
            dt = oDAL.GetEmpDetails(empid);
            if (dt.Rows.Count > 0)
            {
                GVEmployee.DataSource = dt;
                GVEmployee.DataBind();
                GVEmployee.Visible = true;
            }
    }

    private void GetGroupDetails(string GroupCode)
    {
        if (!string.IsNullOrWhiteSpace(GroupCode))
        {
            DataTable dt = oDAL.GetGroupDetails(GroupCode,Session["COMPANY"].ToString());
            if (dt.Rows.Count > 0)
            {
                txtGroupName.Text = dt.Rows[0].Field<string>("GROUP_NAME");
                txtGroupRemarks.Text = dt.Rows[0].Field<string>("Remarks");
                
                txtGroupName.Enabled = txtGroupCode.Enabled = txtGroupRemarks.Enabled = false;
            }
            else
            {
                txtGroupName.Enabled = txtGroupCode.Enabled = txtGroupRemarks.Enabled = true;
            }

            gvGroupRights.DataSource = Session["Rights"] = oDAL.GetGroupRights(GroupCode, Session["COMPANY"].ToString());
            gvGroupRights.DataBind();
        }
        else
        {
            txtGroupName.Text = null;
            txtGroupRemarks.Text = null;
            txtGroupName.Enabled = false;
            txtGroupRemarks.Enabled = false;
            gvGroupRights.DataSource = Session["Rights"] = null;
            gvGroupRights.DataBind();
        }
    }

    private void GetGroupMasterDetails()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetGroup(Session["COMPANY"].ToString());
        GridView2.DataSource = Session["GROUPMASTER"] = dt;
        GridView2.DataBind();
    }
    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataTable dt1 = new DataTable();
            if (Session["ERRORDATA"] != null)
            {
                dt1 = (DataTable)Session["ERRORDATA"];
            }
            if (dt1.Rows.Count > 0)
            {
                //Response.Clear();

                dt1.TableName = "ErrorData";
                using (ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
                {
                    wb.Worksheets.Add(dt1);
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
    protected void imagebuttonEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton imgbtn = (ImageButton)sender;
            GridViewRow gvRow = (GridViewRow)imgbtn.NamingContainer;
            PopulateLocation();
            gvGroupRights.Visible = true;
            btnRemoveEmpOnGroup.Visible = false;
            GVEmployee.Visible = GvGroupEmployee.Visible = false;
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                txtGroupCode.Text = ((Label)gvRow.FindControl("lblGroupCode")).Text.Trim();
                txtGroupName.Text = ((Label)gvRow.FindControl("lblGroupName")).Text.Trim();
                txtGroupRemarks.Text = ((Label)gvRow.FindControl("lblGroupRemarks")).Text.Trim();
                chkSetStatus.Checked = ((CheckBox)gvRow.FindControl("chkActive")).Checked;
                GetGroupDetails(txtGroupCode.Text);
                int i = 0;
                foreach (GridViewRow rows in gvGroupRights.Rows)
                {
                    CheckBox CheckBox1 = (CheckBox)rows.FindControl("chkView");
                    if (CheckBox1.Checked == true)
                    {
                        i++;
                    }
                }
                if (gvGroupRights.Rows.Count == i)
                {
                    CheckBox CheckBoxHeader = (gvGroupRights.HeaderRow.FindControl("chkHView") as CheckBox);
                    CheckBoxHeader.Checked = true;
                }
                else
                {
                    CheckBox CheckBoxHeader = (gvGroupRights.HeaderRow.FindControl("chkHView") as CheckBox);
                    CheckBoxHeader.Checked = false;
                }
            }
            btnSubmit.Text = "Update";
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    protected void imagebuttonEmpAdd_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnUpdateMapLocation.Visible = false;
            GVEmployee.DataSource = null;
            GVEmployee.DataBind();
            ImageButton imgbtn = (ImageButton)sender;
            GridViewRow gvRow = (GridViewRow)imgbtn.NamingContainer;
            PopulateLocation();
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                txtGroupCode.Text = ((Label)gvRow.FindControl("lblGroupCode")).Text.Trim();
                txtGroupName.Text = ((Label)gvRow.FindControl("lblGroupName")).Text.Trim();
                txtGroupRemarks.Text = ((Label)gvRow.FindControl("lblGroupRemarks")).Text.Trim();
                chkSetStatus.Checked = ((CheckBox)gvRow.FindControl("chkActive")).Checked;
                //string GroupCode = ((Label)gvRow.FindControl("lblGroupCode")).Text.Trim();
                gvGroupRights.Visible = GvGroupEmployee.Visible= false;
                txtGroupCode.Enabled = txtGroupName.Enabled = txtGroupRemarks.Enabled = false;
                chkSetStatus.Enabled = false;
                EmpVisibility.Visible = true;
                btnRemoveEmpOnGroup.Visible = false;
                //GetGroupDetails(txtGroupCode.Text);
            }
            btnSubmit.Text = "Add Employee";
            txtEmployeeID.Text = "";
            txtEmployeeID.Enabled = true;
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    protected void imagebuttonDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            bool bResult = false;
            string GroupCode = string.Empty;
            ImageButton imgbtn = (ImageButton)sender;
            GridViewRow gvRow = (GridViewRow)imgbtn.NamingContainer;
            PopulateLocation();
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                GroupCode = ((Label)gvRow.FindControl("lblGroupCode")).Text.ToString().Trim();
                if (GroupCode == "SUPER ADMIN - IT" ||GroupCode == "SUPER ADMIN - CRE")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You are not authorised to Delete this group.');", true);
                    return;
                }
                if (oDAL.CheckSelectedGroupEmpCount(GroupCode))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertCommon", "ShowAlertCommon('Please Note:One or More Employees mapped under this Group.');", true);
                    return;
                }
                else
                {
                    bResult = oDAL.RemoveGroup(GroupCode);
                    if(bResult)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertCommon", "ShowAlertCommon('Successfully Group is deleted.');", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertCommon", "ShowAlertCommon('Group is Not Deleted.');", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    protected void imagebuttonEmpManage_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateLocation();
            ImageButton imgbtn = (ImageButton)sender;
            GridViewRow gvRow = (GridViewRow)imgbtn.NamingContainer;

            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                txtGroupCode.Text = ((Label)gvRow.FindControl("lblGroupCode")).Text.Trim();
                txtGroupName.Text = ((Label)gvRow.FindControl("lblGroupName")).Text.Trim();
                txtGroupRemarks.Text = ((Label)gvRow.FindControl("lblGroupRemarks")).Text.Trim();
                chkSetStatus.Checked = ((CheckBox)gvRow.FindControl("chkActive")).Checked;
                gvGroupRights.DataSource = GVEmployee.DataSource = null;
                gvGroupRights.Visible = false;
                GVEmployee.Visible = false;
                txtGroupCode.Enabled = txtGroupName.Enabled = txtGroupRemarks.Enabled = false;
                chkSetStatus.Enabled = false;
                EmpVisibility.Visible = false;
                GvGroupEmployee.Visible = true;
                GvGroupEmployee.DataSource = null;
                
                DataTable dt = oDAL.GetGroupwithEmployee(txtGroupCode.Text,txtGroupName.Text,Session["COMPANY"].ToString());
                if(dt.Rows.Count>0)
                {
                    GvGroupEmployee.DataSource = dt;
                    GvGroupEmployee.DataBind();
                    GVEmployee.Visible = false;
                    gvGroupRights.Visible = false;
                    btnSubmit.Text = "Manage Employees to Group";
                    btnRemoveEmpOnGroup.Visible = true;
                    EmpVisibility.Visible = false;

                    foreach (GridViewRow row in GvGroupEmployee.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox CheckBox1 = (CheckBox)row.FindControl("chkEmpView");
                            if(CheckBox1.Checked)
                            {
                                Label LblEmpcode = (Label)row.FindControl("lblEmpCode");
                                Label LblEmpName = (Label)row.FindControl("lblEmpName");
                                if (LblEmpcode.Text.ToString() == Session["CURRENTUSER"].ToString())
                                {
                                    CheckBox1.Checked = false;
                                }
                            }
                        }
                    }

                    int totalCount = GvGroupEmployee.Rows.Cast<GridViewRow>()
                        .Count(r => ((CheckBox)r.FindControl("chkEmpView")).Checked);
                    if (GvGroupEmployee.Rows.Count == totalCount)
                    {
                        CheckBox CheckBoxHeader = (GvGroupEmployee.HeaderRow.FindControl("chkHEmpView") as CheckBox);
                        CheckBoxHeader.Checked = true;
                    }
                    else
                    {
                        CheckBox CheckBoxHeader = (GvGroupEmployee.HeaderRow.FindControl("chkHEmpView") as CheckBox);
                        CheckBoxHeader.Checked = false;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : No Employee with this Group.');", true);
                    txtGroupName.Text = txtGroupCode.Text = txtGroupRemarks.Text = string.Empty;
                    GvGroupEmployee.DataSource = null;
                    GvGroupEmployee.DataBind();
                    gvGroupRights.Visible = true;
                    GetGroupMasterDetails();
                    GetGroupDetails(Session["COMPANY"].ToString());
                    EmpVisibility.Visible = false;
                    btnSubmit.Text = "Save";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    protected void btnGet_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GetEmployeeDetails(txtEmployeeID.Text.Trim());
            DataTable dt = oDAL.CheckEmployeeGroupMapping(txtEmployeeID.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee is Already Mapped to a Group.');", true);
                txtGroupName.Text = txtGroupCode.Text = txtGroupRemarks.Text = string.Empty;
                //GvGroupEmployee.DataSource = GVEmployee.DataSource = null;
                //GvGroupEmployee.DataBind();
                //GVEmployee.DataBind();
                gvGroupRights.Visible = true;
                txtEmployeeID.Text = string.Empty;
                GetGroupMasterDetails();
                GetGroupDetails(Session["COMPANY"].ToString());
                EmpVisibility.Visible = false;
                btnRemoveEmpOnGroup.Visible = false;
                btnSubmit.Text = "Save";
                return;
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    #endregion BUTTON EVENTS

    #region GRID EVENTS
    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridView2.PageIndex = e.NewPageIndex;
            GetGroupDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void gvGroupRights_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkView = (CheckBox)e.Row.Cells[2].FindControl("chkView");
                CheckBox chkHView = (CheckBox)this.gvGroupRights.HeaderRow.FindControl("chkHView");
                chkView.Attributes["onclick"] = string.Format("javascript:ChildViewClick(this,'{0}');", chkHView.ClientID);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void gvGroupRights_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Rights"];
            gvGroupRights.PageIndex = e.NewPageIndex;
            gvGroupRights.DataSource = dt;
            gvGroupRights.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void GvGroupEmployee_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //try
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow &&
        //       (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        //    {
        //        CheckBox chkEmpView = (CheckBox)e.Row.Cells[2].FindControl("chkEmpView");
        //        CheckBox chkHEmpView = (CheckBox)this.GvGroupEmployee.HeaderRow.FindControl("chkHEmpView");
        //        chkEmpView.Attributes["onclick"] = string.Format("javascript:ChildViewClick(this,'{0}');", chkHEmpView.ClientID);
        //    }
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
    }
    protected void GvGroupEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = oDAL.GetGroupwithEmployee();
            GvGroupEmployee.PageIndex = e.NewPageIndex;
            GvGroupEmployee.DataSource = dt;
            GvGroupEmployee.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void chkEmpView_OnCheckedChanged(object sender, EventArgs e)
    {
        #region
        //bool result = false;
        //CheckBox ddlAgent = (CheckBox)sender;
        //GridViewRow row = (GridViewRow)ddlAgent.NamingContainer;
        //if (row.RowType == DataControlRowType.DataRow)
        //{
        //    CheckBox CheckBox1 = (CheckBox)row.FindControl("chkEmpView");
        //    if(CheckBox1.Checked)
        //    {
        //        oPRP.GroupCode= ((Label)row.FindControl("lblGVGroupEMPGroupCode")).Text.Trim();
        //        oPRP.GroupName= ((Label)row.FindControl("lblGVGroupEMPGroupName")).Text.Trim();
        //        oPRP.isExistGroupCode = ((Label)row.FindControl("lblExistGroupCode")).Text.Trim();
        //        oPRP.isExistGroupName = ((Label)row.FindControl("lblExistGroupName")).Text.Trim();
        //        oPRP.EmployeeID = ((Label)row.FindControl("lblEmpCode")).Text.Trim();
        //        result = oDAL.UpdateEmployeetoGroup(oPRP);
        //        if(result)
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Successfully Updated the Group with Employee.');", true);
        //            return;
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Employee with Group is not mapped.');", true);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        if (CheckBox1.Checked == false) { CheckBox1.Checked = true; }
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Unmapping Cannot be Done in any group.');", true);
        //        return;
        //    }
        //}
        #endregion
        CheckBox ddlAgent = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ddlAgent.NamingContainer;
        int i = 0;
        if (row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckBox1 = (CheckBox)row.FindControl("chkEmpView");
            Label LblEmpcode = (Label)row.FindControl("lblEmpCode");
            Label LblEmpName = (Label)row.FindControl("lblEmpName");
            if(LblEmpcode.Text.ToString()==Session["CURRENTUSER"].ToString())
            {
                CheckBox1.Checked = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertCommon", "ShowAlertCommon('Selected Employee and Logged In User Cannot be same.');", true);
            }
        }
        foreach (GridViewRow rows in GvGroupEmployee.Rows)
        {
            CheckBox CheckBox1 = (CheckBox)rows.FindControl("chkEmpView");
            if (CheckBox1.Checked == true)
            {
                i++;
            }
        }
        if (GvGroupEmployee.Rows.Count == i)
        {
            CheckBox CheckBoxHeader = (GvGroupEmployee.HeaderRow.FindControl("chkHEmpView") as CheckBox);
            CheckBoxHeader.Checked = true;
        }
        else
        {
            CheckBox CheckBoxHeader = (GvGroupEmployee.HeaderRow.FindControl("chkHEmpView") as CheckBox);
            CheckBoxHeader.Checked = false;
        }
    }
    protected void chkHeaderEmpView_OnCheckedChanged(object sender, EventArgs e)
    {
        #region
        //bool result = false;
        //CheckBox ddlAgent = (CheckBox)sender;
        //GridViewRow row = (GridViewRow)ddlAgent.NamingContainer;
        //if (row.RowType == DataControlRowType.DataRow)
        //{
        //    CheckBox CheckBox1 = (CheckBox)row.FindControl("chkEmpView");
        //    if (CheckBox1.Checked)
        //    {

        //        oPRP.GroupCode = ((Label)row.FindControl("lblGVGroupEMPGroupCode")).Text.Trim();
        //        oPRP.GroupName = ((Label)row.FindControl("lblGVGroupEMPGroupName")).Text.Trim();
        //        oPRP.isExistGroupCode = ((Label)row.FindControl("lblExistGroupCode")).Text.Trim();
        //        oPRP.isExistGroupName = ((Label)row.FindControl("lblExistGroupName")).Text.Trim();
        //        oPRP.EmployeeID = ((Label)row.FindControl("lblEmpCode")).Text.Trim();
        //        result = oDAL.UpdateEmployeetoGroup(oPRP);
        //        if (result)
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Successfully Updated the Group with Employee.');", true);
        //            return;
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Employee with Group is not mapped.');", true);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        if (CheckBox1.Checked == false) { CheckBox1.Checked = true; }
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Unmapping Cannot be Done in any group.');", true);
        //        return;
        //    }
        //}
        #endregion
        //... Your code ...
        // Here we find the controls tha we will handle
        CheckBox CheckBoxHeader = (GvGroupEmployee.HeaderRow.FindControl("chkHEmpView") as CheckBox);
        //CheckBox CheckBoxHeader = (CheckBox)row.FindControl("chkSelectAsset");
        //chkAll.Checked = true;
        foreach (GridViewRow row in GvGroupEmployee.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CheckBox1 = (CheckBox)row.FindControl("chkEmpView");
                if(CheckBox1.Checked)
                {
                    Label LblEmpcode = (Label)row.FindControl("lblEmpCode");
                    Label LblEmpName = (Label)row.FindControl("lblEmpName");
                    if (LblEmpcode.Text.ToString() == Session["CURRENTUSER"].ToString())
                    {
                        CheckBox1.Checked = false;
                    }
                }
            }
        }
        int totalCount = GvGroupEmployee.Rows.Cast<GridViewRow>()
                        .Count(r => ((CheckBox)r.FindControl("chkEmpView")).Checked);
        if (GvGroupEmployee.Rows.Count == totalCount)
        {
            CheckBoxHeader = (GvGroupEmployee.HeaderRow.FindControl("chkHEmpView") as CheckBox);
            CheckBoxHeader.Checked = true;
        }
        else
        {
            CheckBoxHeader = (GvGroupEmployee.HeaderRow.FindControl("chkHEmpView") as CheckBox);
            CheckBoxHeader.Checked = false;
        }
    }
    #endregion



    protected void btnUpdateMapLocation_Click(object sender, EventArgs e)
    {
        try
        {
            if(txtEmployeeID.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Enter Employee code.');", true);
                return;
            }
            //DataTable dp = oDAL.SaveUserLocationMapping(txtEmployeeID.Text.Trim(), "", Session["COMPANY"].ToString(), Session["CURRENTUSER"].ToString(), 0, "GetData");
            //if (dp.Rows.Count == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee is not mapped to any location previously.');", true);
            //    return;
            //}
            foreach (ListItem li in CheckBoxListLoc.Items)
            {
                int Active = 0;
                // If the listitem is selected
                if (li.Selected)
                {
                    Active = 1;
                }
                DataTable dt = oDAL.SaveUserLocationMapping(txtEmployeeID.Text.Trim(), li.Value.Trim(), Session["COMPANY"].ToString(), Session["CURRENTUSER"].ToString(), Active, "Update");
                if (dt.Rows.Count > 0)
                {
                    string msg = dt.Rows[0][0].ToString();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : "+ msg + " ');", true);
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    protected void imageLocationbuttonEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            EmpVisibility.Visible = true;
            ImageButton imgbtn = (ImageButton)sender;
            GridViewRow gvRow = (GridViewRow)imgbtn.NamingContainer;
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                btnUpdateMapLocation.Visible = true;
                txtEmployeeID.Text = ((Label)gvRow.FindControl("lblEmpCode")).Text.Trim();
                txtEmployeeID.Enabled = false;
                DataTable dp = oDAL.SaveUserLocationMapping(txtEmployeeID.Text.Trim(), "", Session["COMPANY"].ToString(), Session["CURRENTUSER"].ToString(), 0, "GetData");
                for (int i = 0; i < dp.Rows.Count; i++)
                {
                    foreach (ListItem li in CheckBoxListLoc.Items)
                    {
                        if(dp.Rows[i]["SITE_CODE"].ToString() == li.Value.Trim())
                        {
                            li.Selected = true;
                        }
                        else
                        {
                            li.Selected = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
}