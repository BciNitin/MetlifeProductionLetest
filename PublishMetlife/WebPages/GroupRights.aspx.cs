using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class GroupRights : System.Web.UI.Page
{
    GroupRights_DAL oDAL;
    GroupRights_PRP oPRP;
    public GroupRights()
    {
        oPRP = new GroupRights_PRP();
    }
    ~GroupRights()
    {
        oDAL = null;
        oPRP = null;
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
        oDAL = new GroupRights_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Checking group rights for viewing group rights details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("GROUP_RIGHTS", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "GROUP_RIGHTS");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                GetGroups(Session["COMPANY"].ToString());
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
        clsGeneral.LogErrorToLogFile(ex, "Group Rights");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Populate existing user groups and their corresponding group code.
    /// </summary>
    private void GetGroups(string CompCode)
    {
        DataTable dt = new DataTable();
        ddlGroup.DataSource = null;
        dt = oDAL.GetGroup(CompCode);
        ddlGroup.DataSource = dt;
        ddlGroup.DataTextField = "GROUP_NAME";
        ddlGroup.DataValueField = "GROUP_CODE";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, "-- Select Group --");
    }

    /// <summary>
    /// Fetching group rights details for the selected user group.
    /// </summary>
    /// <param name="_GroupCode"></param>
    private void GetGroupRights(string GroupCode, string CompCode)
    {
        DataTable dt = new DataTable();
        if (GroupCode == "")
            gvGroupRights.DataSource = null;
        else
        {
            gvGroupRights.DataSource = null;
            dt = oDAL.GetGroupRights(GroupCode,CompCode);
            if (dt.Rows.Count > 0)
            {
                gvGroupRights.DataSource = Session["GROUP_RIGHTS"] = dt;
                gvGroupRights.DataBind();
                //if (GroupCode.ToUpper() == "SYSADMIN")
                //{
                //    gvGroupRights.Enabled = false;
                //    btnSubmit.Enabled = false;
                //}
                //else
                //{
                //    gvGroupRights.Enabled = true;
                //    btnSubmit.Enabled = true;
                //}
                Session["Rights"] = dt;
                Session["ASSET_TYPE"] = dt.Rows[0]["ASSET_TYPE"].ToString();
                if (Session["ASSET_TYPE"].ToString().Contains(","))
                {
                    rdoAdminAssetType.Checked = true;
                    rdoITAssetType.Checked = true;
                }
                else if (dt.Rows[0]["ASSET_TYPE"].ToString() == "ADMIN")
                {
                    rdoAdminAssetType.Checked = true;
                    rdoITAssetType.Checked = false;
                }
                else if (dt.Rows[0]["ASSET_TYPE"].ToString() == "IT")
                {
                    rdoAdminAssetType.Checked = false;
                    rdoITAssetType.Checked = true;
                }
                else
                {
                    rdoAdminAssetType.Checked = false;
                    rdoITAssetType.Checked = false;
                }
            }
            else
            {
                gvGroupRights.DataSource = null;
                gvGroupRights.DataBind();
            }
        }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Submit request for fetching group rights details for the selected user group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool bResp = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Group Rights", "Group Rights", "Group Rights by user id" + Session["CURRENTUSER"].ToString());
            int iCnt = 0;
            bool bView, bSave, bEdit, bDelete, bExport = false;
            DataTable dt = (DataTable)Session["Rights"];
            int iRowCnt = gvGroupRights.Rows.Count;
            if (iRowCnt == 0 || dt == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowNullMsg", "ShowNullMsg();", true);
                return;
            }
            foreach (GridViewRow gvRow in gvGroupRights.Rows)
            {
                if (gvRow.RowType == DataControlRowType.DataRow)
                {
                    bView = ((CheckBox)gvRow.FindControl("chkView")).Checked;
                    bSave = ((CheckBox)gvRow.FindControl("chkSave")).Checked;
                    bEdit = ((CheckBox)gvRow.FindControl("chkEdit")).Checked;
                    bDelete = ((CheckBox)gvRow.FindControl("chkDelete")).Checked;
                    bExport = ((CheckBox)gvRow.FindControl("chkExport")).Checked;

                    dt.Rows[iCnt]["VIEW_RIGHTS"] = bView;
                    dt.Rows[iCnt]["SAVE_RIGHTS"] = bSave;
                    dt.Rows[iCnt]["EDIT_RIGHTS"] = bEdit;
                    dt.Rows[iCnt]["DELETE_RIGHTS"] = bDelete;
                    dt.Rows[iCnt]["EXPORT_RIGHTS"] = bExport;
                    iCnt++;
                    dt.AcceptChanges();
                }
            }
            if (rdoAdminAssetType.Checked)
                oPRP.AssetType = "ADMIN";
            if (rdoITAssetType.Checked)
                oPRP.AssetType = "IT";
            if (rdoAdminAssetType.Checked && rdoITAssetType.Checked)
                oPRP.AssetType = "ADMIN,IT";
            oPRP.GroupCode = ddlGroup.SelectedValue.ToString();
            oPRP.CompCode = Session["COMPANY"].ToString();

            bResp = oDAL.SaveUpdateGroupRights(dt, oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : An error occured. Please try again.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSuccessMsg", "ShowSuccessMsg();", true);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh/reset gridview data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            gvGroupRights.DataSource = null;
            gvGroupRights.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Group rights details for a group to be exported into excel sheet.
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
            if (gvGroupRights.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["GROUP_RIGHTS"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=GroupRights.xls");  
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

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Checkbox checked/unchecked javascript event addition
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

                CheckBox chkSave = (CheckBox)e.Row.Cells[3].FindControl("chkSave");
                CheckBox chkHSave = (CheckBox)this.gvGroupRights.HeaderRow.FindControl("chkHSave");
                chkSave.Attributes["onclick"] = string.Format("javascript:ChildSaveClick(this,'{0}');", chkHSave.ClientID);

                CheckBox chkEdit = (CheckBox)e.Row.Cells[4].FindControl("chkEdit");
                CheckBox chkHEdit = (CheckBox)this.gvGroupRights.HeaderRow.FindControl("chkHEdit");
                chkEdit.Attributes["onclick"] = string.Format("javascript:ChildEditClick(this,'{0}');", chkHEdit.ClientID);

                CheckBox chkDelete = (CheckBox)e.Row.Cells[5].FindControl("chkDelete");
                CheckBox chkHDelete = (CheckBox)this.gvGroupRights.HeaderRow.FindControl("chkHDelete");
                chkDelete.Attributes["onclick"] = string.Format("javascript:ChildDeleteClick(this,'{0}');", chkHDelete.ClientID);

                CheckBox chkExport = (CheckBox)e.Row.Cells[6].FindControl("chkExport");
                CheckBox chkHExport = (CheckBox)this.gvGroupRights.HeaderRow.FindControl("chkHExport");
                chkExport.Attributes["onclick"] = string.Format("javascript:ChildExportClick(this,'{0}');", chkHExport.ClientID);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Group rights page index changing event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    #endregion

    #region SELECTEDINDEXCHANGED EVENT
    /// <summary>
    /// Page level rights are displayed based on group selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlGroup.SelectedIndex != 0)
                GetGroupRights(ddlGroup.SelectedValue.ToString().Trim(), Session["COMPANY"].ToString());
            //else
            //{
            //    rdoAdminAssetType.Checked = true;
               
            //}
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion
}