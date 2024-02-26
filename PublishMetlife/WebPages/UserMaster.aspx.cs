using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    string _CompCode = "";
    UserMaster_DAL oDAL;
    UserMaster_PRP oPRP;

    public _Default()
    {
        oPRP = new UserMaster_PRP();
    }
    ~_Default()
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
        if (Session["COMPANY"].ToString() == string.Empty)
        {
            Server.Transfer("UnauthorizedUser.aspx");
        }
        oDAL = new UserMaster_DAL(Session["DATABASE"].ToString());
        _CompCode = Session["COMPANY"].ToString();
    }

    /// <summary>
    /// Checking user group rights for user accounts/master viewing.
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
                if (Session["CURRENTUSER"].ToString() != null)
                {
                    string _strRights = clsGeneral.GetRights("USER_MASTER", (DataTable)Session["UserRights"]);
                    clsGeneral._strRights = _strRights.Split('^');
                    clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "USER_MASTER");
                    if (clsGeneral._strRights[0] == "0")
                    {
                        Response.Redirect("UnauthorizedUser.aspx", false);
                    }
                } 
                GetUserDetails(Session["COMPANY"].ToString());
                if (_CompCode != null)
                {
                    GetGroup(_CompCode);
                    GetLocation();
                }
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
        clsGeneral.LogErrorToLogFile(ex, "User Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Response.Redirect("Error.aspx", false);
        }
    }

    /// <summary>
    /// Fetches user details to be populated in gridview
    /// </summary>
    private void GetUserDetails(string CompCode)
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetUserDetails(CompCode);
        gvUserMaster.DataSource = Session["UserMaster"] = dt;
        gvUserMaster.DataBind();
    }

    /// <summary>
    /// Fetches Group to be mapped with user id
    /// </summary>
    private void GetGroup(string CompanyCode)
    {
        ddlGroup.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetGroup(CompanyCode);
        ddlGroup.DataSource = dt;
        ddlGroup.DataValueField = "GROUP_CODE";
        ddlGroup.DataTextField = "GROUP_NAME";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, "-- Select Group --");
    }

    /// <summary>
    /// Fetces Location to be mapped with user id
    /// </summary>
    private void GetLocation()
    {
        lblLocCode.Text = "0";
        lblLocLevel.Text = "1";
        txtLocationCode.Text = "";
        ddlLocation.DataSource = null;
        ddlLocation.Items.Clear();
        DataTable dt = new DataTable();
        dt = oDAL.GetLocation(_CompCode, "", 1);
        ddlLocation.DataSource = dt;
        ddlLocation.DataValueField = "LOC_CODE";
        ddlLocation.DataTextField = "LOC_NAME";
        ddlLocation.DataBind();
        ddlLocation.Items.Insert(0, "-- Select Location --");
    }

    /// <summary>
    /// Encrypt user password into UTF8 characters.
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    private string EncryptPassword(string password)
    {
        string EncryptedPswd = string.Empty;
        byte[] encode = new byte[password.Length];
        encode = Encoding.UTF8.GetBytes(password);
        EncryptedPswd = Convert.ToBase64String(encode);
        return EncryptedPswd;
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Gets user details in delete mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvUserMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                GridViewRow gvRow = gvUserMaster.Rows[e.RowIndex];
                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.UserID = ((Label)(gvRow.FindControl("lblUserId"))).Text.Trim();
                if (oPRP.UserID.ToUpper() == Session["CURRENTUSER"].ToString().ToUpper())
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg",
                    "ShowErrMsg('Please Note : Logged in user cannot delete own user id!');", true);
                    return;
                }
                bool bResult = oDAL.DeleteUser(oPRP);
                if(bResult)
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", 
                        "ShowErrMsg('Please Note : User details deleted successfully.');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg",
                        "ShowErrMsg('Please Note : System Administrator cannot be deleted.');", true);
                GetUserDetails(Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Gets user details in edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvUserMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            string UserID = "";
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg",
                "ShowUnAuthorisedMsg('Please Note : You are not authorised to execute this operation!');", true);
                return;
            }
            else
            {
                GridViewRow gvRow = (GridViewRow)gvUserMaster.Rows[e.NewEditIndex];
                UserID = ((Label)gvRow.FindControl("lblUserId")).Text.Trim();
                ViewState["LOC_CODE"] = ((Label)gvRow.FindControl("lblLC")).Text.Trim();
                ViewState["GROUP"] = ((Label)gvRow.FindControl("lblGC")).Text.Trim();
                //ViewState["COMPANY"] = ((Label)gvRow.FindControl("lblCompany")).Text.Trim();
                gvUserMaster.EditIndex = e.NewEditIndex;
                GetUserDetails(Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Gets user location and Group details for user detials updation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvUserMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlEditLocation = (DropDownList)e.Row.FindControl("ddlEditLocation");
                if (ddlEditLocation != null)
                {
                    ddlEditLocation.DataSource = oDAL.GetLocation(_CompCode, "", 1);
                    ddlEditLocation.DataTextField = "LOC_NAME";
                    ddlEditLocation.DataValueField = "LOC_CODE";
                    ddlEditLocation.DataBind();
                    ddlEditLocation.Items.Insert(0, "-- Select Location --");
                    ddlEditLocation.SelectedIndex = ddlEditLocation.Items.IndexOf(ddlEditLocation.Items.FindByText(ViewState["LOC_CODE"].ToString()));
                }
                DropDownList ddlEditGroup = (DropDownList)e.Row.FindControl("ddlEditGroup");
                if (ddlEditGroup != null)
                {
                    ddlEditGroup.DataSource = oDAL.GetGroup(Session["COMPANY"].ToString());
                    ddlEditGroup.DataTextField = "GROUP_NAME";
                    ddlEditGroup.DataValueField = "GROUP_CODE";
                    ddlEditGroup.DataBind();
                    ddlEditGroup.Items.Insert(0, "-- Select Group --");
                    ddlEditGroup.SelectedIndex = ddlEditGroup.Items.IndexOf(ddlEditGroup.Items.FindByValue(ViewState["GROUP"].ToString()));
                }
                DropDownList ddlEditCompany = (DropDownList)e.Row.FindControl("ddlEditCompany");
                if (ddlEditCompany != null)
                {
                    ddlEditCompany.DataSource = oDAL.GetCompany();
                    ddlEditCompany.DataTextField = "COMP_NAME";
                    ddlEditCompany.DataValueField = "COMP_CODE";
                    ddlEditCompany.DataBind();
                    ddlEditCompany.Items.Insert(0, "-- Select Company --");
                    ddlEditCompany.SelectedIndex = ddlEditCompany.Items.IndexOf(ddlEditCompany.Items.FindByText(ViewState["COMPANY"].ToString()));
                }
                ImageButton imagebuttonEdit = (ImageButton)e.Row.FindControl("imagebuttonEdit");
                ImageButton imagebuttonDelete = (ImageButton)e.Row.FindControl("imagebuttonDelete");
                CheckBox chkEditActive = (CheckBox)e.Row.FindControl("chkEditActive");
                if (ViewState["GROUP"] != null)
                {
                    if (ViewState["GROUP"].ToString().ToUpper() == "SYSADMIN")
                    {
                        if (chkEditActive != null)
                            chkEditActive.Enabled = false;
                    }
                    else
                    {
                        if (chkEditActive != null)
                            chkEditActive.Enabled = true;
                    }
                }
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
    /// User details can be updated
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvUserMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
                new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "User Master", "User Master", "User Master  update by user id" + Session["CURRENTUSER"].ToString());
                GridViewRow gvRow = gvUserMaster.Rows[e.RowIndex];
                oPRP.UserID = ((Label)(gvRow.FindControl("lblEditUserId"))).Text.Trim();
                oPRP.UserName = ((TextBox)(gvRow.FindControl("txtEditUserName"))).Text.Trim();
                oPRP.Active = ((CheckBox)(gvRow.FindControl("chkEditActive"))).Checked;
                if (((Label)(gvRow.FindControl("lblELocCode"))).Text.Trim() == "0")
                {
                    oPRP.LocationCode = ((Label)(gvRow.FindControl("lblELC"))).Text.Trim();
                }
                else
                {
                    oPRP.LocationCode = ((Label)(gvRow.FindControl("lblELocCode"))).Text.Trim();
                }
                oPRP.UserEmail = ((TextBox)(gvRow.FindControl("txtEEmail"))).Text.Trim();
                oPRP.TechOpsEmail = ((TextBox)(gvRow.FindControl("txtEditTOEmail"))).Text.Trim();
                oPRP.GroupCode = ((DropDownList)(gvRow.FindControl("ddlEditGroup"))).SelectedValue.ToString();
                string GroupName = ((DropDownList)(gvRow.FindControl("ddlEditGroup"))).SelectedItem.Text.ToString();
                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
                oDAL.SaveUpdateUser("UPDATE", oPRP);
                //if (oPRP.UserID.ToUpper() == Session["CURRENTUSER"].ToString().ToUpper())
                //{
                //    if (oPRP.GroupCode == ViewState["GROUP"].ToString())
                //    {
                //        oDAL.SaveUpdateUser("UPDATE", oPRP);
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You cannot change your own group.');", true);
                //    }
                //}
                //else if (oPRP.UserID.ToUpper() != Session["CURRENTUSER"].ToString().ToUpper() && Session["GROUP"].ToString().ToUpper() == "SYSADMIN")
                //{
                //    oDAL.SaveUpdateUser("UPDATE", oPRP);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You cannot update another user details.');", true);
                //}
                gvUserMaster.EditIndex = -1;
                GetUserDetails(Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// User details are cancelled from edit/update/delete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvUserMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvUserMaster.EditIndex = -1;
            GetUserDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// User master gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvUserMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvUserMaster.PageIndex = e.NewPageIndex;
            GetUserDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Save new user details
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
            else if (lblLocCode.Text.Trim() == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : User must have a location.');", true);
                ddlLocation.Focus();
                return;
            }
            else
            {
                oPRP.UserID = txtUserID.Text.Trim();
                oPRP.UserName = txtUserName.Text.Trim();
                oPRP.UserPswd = EncryptPassword(txtPassword.Text.Trim());
                oPRP.UserEmail = txtEmailAddress.Text.Trim();
                oPRP.LocationCode = lblLocCode.Text.Trim();
                oPRP.GroupCode = ddlGroup.SelectedValue.ToString();
                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.TechOpsEmail = txtTechOpsEMailId.Text.Trim();
                oPRP.Active = chkSetStatus.Checked;
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                bool bResp = oDAL.SaveUpdateUser("SAVE", oPRP);
                if (!bResp)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate User Id cannot be saved!!!');", true);
                    txtUserID.Focus();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                }
                GetLocation();
                GetUserDetails(Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refreshes/clears screen fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            GetLocation();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh/reset location details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshLocation_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblLocCode.Text = "0";
            GetLocation();
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
            if (gvUserMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["UserMaster"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'>User Master</font></u></b>");
                //hw.WriteLine("<br>");
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=UserMaster.xls");
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
        { HandleExceptions(ex); }
    }
    #endregion

    #region SELECTED INDEX CHANGED EVENTS
    /// <summary>
    /// Getting location code based on location name selected (save operation)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLocation_SelectedIndexChanged(object sender,EventArgs e)
    {
        try
        {
            if (ddlLocation.SelectedIndex > 0)
            {
                int locLevel = int.Parse(lblLocLevel.Text.Trim());
                txtLocationCode.Text = ddlLocation.SelectedItem.Text.Trim();
                lblLocLevel.Text = (locLevel + 1).ToString();
                int iLocLevel = int.Parse(lblLocLevel.Text.Trim());
                string sLocCode = ddlLocation.SelectedValue.ToString();
                lblLocCode.Text = ddlLocation.SelectedValue.ToString();

                ddlLocation.DataSource = null;
                DataTable dt = oDAL.GetLocation(_CompCode, sLocCode, iLocLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlLocation.DataSource = dt;
                    ddlLocation.DataValueField = "LOC_CODE";
                    ddlLocation.DataTextField = "LOC_NAME";
                    ddlLocation.DataBind();
                    ddlLocation.Items.Insert(0, "-- Select Location --");
                    ddlLocation.Focus();
                }
                else
                    txtUserName.Focus();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Getting location code based on location name selected (update operation) 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlEditLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlEditLocation = sender as DropDownList;
            foreach (GridViewRow gvRow in gvUserMaster.Rows)
            {
                Control ddlCtrl = gvRow.FindControl("ddlEditLocation") as DropDownList;
                if (ddlCtrl != null)
                {
                    DropDownList ddlLoc = (DropDownList)ddlCtrl;
                    if (ddlEditLocation.ClientID == ddlLoc.ClientID)
                    {
                        //TextBox txtEditLoc = gvRow.FindControl("txtEditLoc") as TextBox;
                        Label lblELoclevel = gvRow.FindControl("lblELoclevel") as Label;
                        Label lblELocCode = gvRow.FindControl("lblELocCode") as Label;
                        if (ddlEditLocation.SelectedIndex > 0)
                        {
                            int locLevel = int.Parse(lblELoclevel.Text.Trim());
                            //txtEditLoc.Text = Convert.ToString(ddlEditLocation.SelectedValue.ToString());
                            lblELoclevel.Text = (locLevel + 1).ToString();
                           // int iLocLevel = int.Parse(lblELoclevel.Text.Trim());
                            string sLocCode = ddlLoc.SelectedValue.ToString();
                            lblELocCode.Text = sLocCode;

                            //ddlEditLocation.DataSource = null;
                            //DataTable dt = oDAL.GetLocation(_CompCode, sLocCode, iLocLevel);
                            //ddlEditLocation.DataSource = dt;
                            //if (dt.Rows.Count > 0)
                            //{
                            //    ddlEditLocation.DataValueField = "LOC_CODE";
                            //    ddlEditLocation.DataTextField = "LOC_NAME";
                            //    ddlEditLocation.DataBind();
                            //    ddlEditLocation.Items.Insert(0, "-- Select Location --");
                            //}
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}