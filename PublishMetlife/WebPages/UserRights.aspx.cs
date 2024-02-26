using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPages_UserRights : System.Web.UI.Page
{


    #region PAGE CONSTRUCTOR & DECLARATIONS

    UserRights_DAL oDAL;
    UserMaster_PRP oPRP;
    public WebPages_UserRights()
    {
        oPRP = new UserMaster_PRP();
    }
    ~WebPages_UserRights()
    {
        oPRP = null;
        oDAL = null;
    }

    #endregion

    #region PAGE EVENTS

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new UserRights_DAL(Session["DATABASE"].ToString());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("USER_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "USER_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                GetSiteDetails();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    #endregion

    #region PRIVATE FUNCTIONS

    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "User Management");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }

    private string EncryptPassword(string password)
    {
        string EncryptedPswd = string.Empty;
        byte[] encode = new byte[password.Length];
        encode = Encoding.UTF8.GetBytes(password);
        EncryptedPswd = Convert.ToBase64String(encode);
        return EncryptedPswd;
    }

    private void GetEmployeeDetails(string empid)
    {
        lblErrorMsg.Text = "";
        gvGroupRights.DataSource = null;
        gvGroupRights.DataBind();
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
            DataTable dt = oDAL.GetGroupDetails(GroupCode);
            if (dt.Rows.Count > 0)
            {
                txtGroupName.Text = dt.Rows[0].Field<string>("GROUP_NAME");
                txtGroupRemarks.Text = dt.Rows[0].Field<string>("Remarks");
                txtGroupName.Enabled = false;
                txtGroupRemarks.Enabled = false;
            }
            else
            {
                txtGroupName.Enabled = true;
                txtGroupRemarks.Enabled = true;
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

    private void GetSiteDetails()
    {
        DataTable dt = new DataTable();
        ddlLocation.DataSource = null;
        dt = oDAL.GetSite();
        ddlLocation.DataSource = dt;
        ddlLocation.DataTextField = "SITE_CODE";
        ddlLocation.DataValueField = "SITE_CODE";
        ddlLocation.DataBind();
        //ddlLocation.Items.Remove("ALL");
        ddlLocation.Items.Insert(0, "-- Select Site --");
    }

    #endregion

    #region GRID EVENTS


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

    //protected void txtGroupCode_TextChanged(object sender, EventArgs e)
    //{
    //    txtGroupName.Text = null;
    //    txtGroupRemarks.Text = null;
    //    txtGroupName.Enabled = false;
    //    txtGroupRemarks.Enabled = false;
    //    gvGroupRights.DataSource = Session["Rights"] = null;
    //    gvGroupRights.DataBind();
    //}


    #endregion

    #region BUTTON EVENTS

    protected void btnGet_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(txtEmployeeID.Text))
            {
                ddlLocation.SelectedIndex = -1;
                txtGroupCode.Text = null;
                chkSetStatus.Checked = true;
                btnSubmit.Text = "Save";
                gvGroupRights.DataSource = null;
                gvGroupRights.DataBind();
                GVEmployee.DataSource = null;
                GVEmployee.DataBind();
                GetEmployeeDetails(txtEmployeeID.Text.Trim());
                DataTable dt = oDAL.GetUserDetails(txtEmployeeID.Text);
                if (dt.Rows.Count > 0)
                {
                    ddlLocation.SelectedValue = dt.Rows[0].Field<string>("LOCATION_CODE");
                    txtGroupCode.Text = dt.Rows[0].Field<string>("GROUP_CODE");
                    chkSetStatus.Checked = dt.Rows[0].Field<bool>("ACTIVE");
                    btnSubmit.Text = "Update";
                }
                else
                {
                    ddlLocation.SelectedIndex = -1;
                    txtGroupCode.Text = null;
                    chkSetStatus.Checked = true;
                    btnSubmit.Text = "Save";
                }
                GetGroupDetails(txtGroupCode.Text);
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    protected void btnGo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GetGroupDetails(txtGroupCode.Text);
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if ((txtGroupCode.Text == "SUPER ADMIN" || txtGroupName.Text == "SUPER ADMIN") && btnSubmit.Text == "Update")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlertCommon", "ShowAlertCommon('Please Note : You are not allowed to Update the Details.');", true);
            return;
        }

        string bResp = string.Empty;
        //if (clsGeneral._strRights[1] == "0")
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
        //    return;
        //}

        int iCnt = 0;
        bool bView = false;
        DataTable dt = (DataTable)Session["Rights"];
        int iRowCnt = gvGroupRights.Rows.Count;
        if (iRowCnt == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Group Details are not available.');", true);
            return;
        }

        iRowCnt = GVEmployee.Rows.Count;
        if (iRowCnt == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee Details are not available.');", true);
            return;
        }

        oPRP.UserID = txtEmployeeID.Text.Trim();
        oPRP.UserName = ((Label)GVEmployee.Rows[0].FindControl("lblEmpName")).Text;
        oPRP.UserEmail = ((Label)GVEmployee.Rows[0].FindControl("lblEmpEMail")).Text;
        oPRP.LocationCode = ddlLocation.SelectedValue;
        oPRP.GroupCode = txtGroupCode.Text;
        oPRP.CompCode = Session["COMPANY"].ToString();
        oPRP.EmployeeID = ((Label)GVEmployee.Rows[0].FindControl("lblEmpID")).Text.Trim();
        oPRP.Active = chkSetStatus.Checked;
        oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
        oPRP.GroupName = txtGroupName.Text.Trim();
        oPRP.GroupRemarks = txtGroupRemarks.Text.Trim();

        if (oPRP.UserID != oPRP.EmployeeID)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee ID is not matched with employee details.');", true);
            return;
        }
        if (oPRP.GroupCode != Convert.ToString(dt.Rows[0]["GROUP_CODE"]))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Group Code is not matched with group rights details.');", true);
            return;
        }


        foreach (GridViewRow gvRow in gvGroupRights.Rows)
        {
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                bView = ((CheckBox)gvRow.FindControl("chkView")).Checked;

                dt.Rows[iCnt]["VIEW_RIGHTS"] = bView;
                dt.Rows[iCnt]["SAVE_RIGHTS"] = bView;
                dt.Rows[iCnt]["EDIT_RIGHTS"] = bView;
                dt.Rows[iCnt]["DELETE_RIGHTS"] = bView;
                dt.Rows[iCnt]["EXPORT_RIGHTS"] = bView;
                iCnt++;
                dt.AcceptChanges();
            }
        }
        bResp = oDAL.SaveUpdateGroupRights(dt, oPRP);
        if (bResp != "SUCCESS")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : '" + bResp + "'.');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
        }
    }

    #endregion

    protected void txtGroupCode_TextChanged(object sender, EventArgs e)
    {
        txtGroupName.Text = null;
        txtGroupRemarks.Text = null;
        gvGroupRights.DataSource = Session["Rights"] = null;
        gvGroupRights.DataBind();
    }
}