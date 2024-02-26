    using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class LocationMaster : System.Web.UI.Page
{
    LocationMaster_DAL oDAL;
    LocationMaster_PRP oPRP;
        public LocationMaster()
        {
            oPRP = new LocationMaster_PRP();
        }
        ~LocationMaster()
        {
            oDAL = null; oPRP = null;
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
        oDAL = new LocationMaster_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user group rights for location master view operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("LOCATION_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "LOCATION_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
            //    PopulateLocation(Session["COMPANY"].ToString());
                GetLocationDetails(Session["COMPANY"].ToString());
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Catch unhandles exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Location Master");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Populate location name and code for parent location name 
    /// selection through parent location dropdownlist.
    /// </summary>
    private void PopulateLocation(string _CompCode)
    {
      //  txtParentLocCode.Text = "";
      //  lblLocLevel.Text = "1";
        DataTable dt = new DataTable();
        if (ddlParentLocCode.SelectedIndex == -1)
            dt = oDAL.GetLocationCode(_CompCode, "", 1);
        else
            dt = oDAL.GetLocationCode(_CompCode, (ddlParentLocCode.SelectedIndex != 0) ? ddlParentLocCode.SelectedItem.Value.ToString().Trim() : "", 1);
        ddlParentLocCode.DataSource = null;
        ddlParentLocCode.DataSource = dt;
        ddlParentLocCode.DataTextField = "LOC_NAME";
        ddlParentLocCode.DataValueField = "LOC_CODE";
        ddlParentLocCode.DataBind();
        ddlParentLocCode.Items.Insert(0, "-- Select Parent Location --");
    }

    /// <summary>
    /// Get location master details for gridview populaion.
    /// </summary>
    private void GetLocationDetails(string _CompCode)
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetLocation(_CompCode);
        gvLocMaster.DataSource = Session["LOCMASTER"] = dt;
        gvLocMaster.DataBind();
    }
    #endregion

    #region GRIDVIEW EVENTS
    /// <summary>
    /// Delete location details through gridview row delete event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLocMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvLocMaster.Rows[e.RowIndex];
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.LocCode = ((Label)gvRow.FindControl("lblLocCode")).Text.Trim();
      //      oPRP.ParentLocCode = ((Label)gvRow.FindControl("lblParentLocCode")).Text.Trim();
      //      oPRP.LocLevel = int.Parse(((Label)gvRow.FindControl("lblLoclevel")).Text.Trim());
            string DelRslt = oDAL.DeleteLocation(oPRP.LocCode, oPRP.CompCode);
            //if (DelRslt == "CHILD_FOUND")
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg",
            //    "ShowErrMsg('Please Note : You cannot delete this location, child locations exist.');", true);
            //    return;
            //}
            if (DelRslt == "LOCATION_IN_USE")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg",
                "ShowErrMsg('Please Note : You cannot delete this location, since the location is mapped with assets.');", true);
                return;
            }
           else if (DelRslt == "SUCCESS")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Location is deleted successfully.');", true);
            GetLocationDetails(Session["COMPANY"].ToString());
            PopulateLocation(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Get location master gridview details into edit mode.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLocMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            gvLocMaster.EditIndex = e.NewEditIndex;
            GetLocationDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Enable/disable edit/delete options as per logged in user rights.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLocMaster_RowDataBound(object sender, GridViewRowEventArgs e)
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
    /// Update location master through gridview row updation event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLocMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvLocMaster.Rows[e.RowIndex];
            oPRP.LocCode = ((Label)gvRow.FindControl("lblLocCode")).Text.Trim();
            oPRP.LocName = ((TextBox)gvRow.FindControl("txtEditLocName")).Text.Trim();
         //   oPRP.ParentLocCode = ((Label)gvRow.FindControl("lblEditParentLocCode")).Text.Trim();
      //      oPRP.LocLevel = int.Parse(((Label)gvRow.FindControl("lblEditLoclevel")).Text.Trim());
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
            oPRP.LocDesc = ((TextBox)gvRow.FindControl("txtEditDesc")).Text.Replace("'", "`").Trim();
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oDAL.SaveUpdateLocation("UPDATE", oPRP);

            gvLocMaster.EditIndex = -1;
            GetLocationDetails(Session["COMPANY"].ToString());
            PopulateLocation(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Location master gridview cancel edit mode event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLocMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvLocMaster.EditIndex = -1;
            GetLocationDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Location master gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLocMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvLocMaster.PageIndex = e.NewPageIndex;
            GetLocationDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Save new location details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string _LocCode = "";
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            oPRP.CompCode = Session["COMPANY"].ToString();
            //if (txtParentLocCode.Text.Trim() != "")
            //{
            //    oPRP.LocCode = txtParentLocCode.Text.Trim().ToUpper();
            //    DataTable dt = oDAL.GetLocationCodeLevel(oPRP.LocCode);
            //    if (dt.Rows.Count > 0)
            //    {
            //        string[] _LocParts = dt.Rows[0]["LOC_CODE"].ToString().Trim().Split('-');
            //        if (Convert.ToInt16(dt.Rows[0]["LOC_LEVEL"]) == 1)
            //        {
            //            _LocCode = _LocParts[0].Trim() + "-" + _LocParts[1].Trim() + "-" + txtLocationCode.Text.Trim().ToUpper() + "-00-00-00";
            //            oPRP.LocLevel = 2;
            //        }
            //        else if (Convert.ToInt16(dt.Rows[0]["LOC_LEVEL"]) == 2)
            //        {
            //            _LocCode = _LocParts[0].Trim() + "-" + _LocParts[1].Trim() + "-" + _LocParts[2].Trim() + "-" + txtLocationCode.Text.Trim().ToUpper() + "-00-00";
            //            oPRP.LocLevel = 3;
            //        }
            //        else if (Convert.ToInt16(dt.Rows[0]["LOC_LEVEL"]) == 3)
            //        {
            //            _LocCode = _LocParts[0].Trim() + "-" + _LocParts[1].Trim() + "-" + _LocParts[2].Trim() + "-" + _LocParts[3].Trim() + "-" + txtLocationCode.Text.Trim().ToUpper() + "-00";
            //            oPRP.LocLevel = 4;
            //        }
            //        else if (Convert.ToInt16(dt.Rows[0]["LOC_LEVEL"]) == 4)
            //        {
            //            _LocCode = _LocParts[0].Trim() + "-" + _LocParts[1].Trim() + "-" + _LocParts[2].Trim() + "-" + _LocParts[3].Trim() + "-" + _LocParts[4].Trim() + "-" + txtLocationCode.Text.Trim().ToUpper();
            //            oPRP.LocLevel = 5;
            //        }
            //        else if (Convert.ToInt16(dt.Rows[0]["LOC_LEVEL"]) == 5)
            //        {
            //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : You cannot add location under this parent location, the selected parent location is of Level 5.');", true);
            //            return;
            //        }
            //    }
            //}
           // else
            {
                _LocCode = txtLocationCode.Text.Trim();
                oPRP.LocLevel = 1;
            }
           oPRP.LocCode = _LocCode;
            oPRP.LocName = txtLocationName.Text.Replace("'", "`").Trim();
         //   oPRP.ParentLocCode = ViewState["ParentLocCode"] != null ? (string)ViewState["ParentLocCode"] : "";
            oPRP.LocDesc = txtLocationDesc.Text.Replace("'", "`").Trim();
        //    oPRP.LocLevel = Convert.ToInt32(lblLocLevel.Text.Trim());
            oPRP.Active = chkSetStatus.Checked;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            bool bResp = oDAL.SaveUpdateLocation("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Location details are not saved.');", true);
                txtLocationCode.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            GetLocationDetails(Session["COMPANY"].ToString());
            PopulateLocation(Session["COMPANY"].ToString());
            btnClear_Click(sender, e);
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Clear/Reset location details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //  txtParentLocCode.Text = "";
            txtLocationCode.Text = "";
            txtParentLocName.Text = "";
          //  lblLocLevel.Text = "1";
            ViewState["ParentLocCode"] = "";
            ddlParentLocCode.SelectedIndex = -1;

            ddlParentLocCode.DataSource = null;
            DataTable dt = oDAL.GetLocationCode(Session["COMPANY"].ToString(),"", 1);
            ddlParentLocCode.DataSource = dt;
            ddlParentLocCode.DataTextField = "LOC_NAME";
            ddlParentLocCode.DataValueField = "LOC_CODE";
            ddlParentLocCode.DataBind();
            ddlParentLocCode.Items.Insert(0, "-- Select Parent Location --");
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Refresh location.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnRefreshLocation_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ddlParentLocCode.DataSource = null;
            ddlParentLocCode.Items.Clear();
            txtParentLocName.Text = "";
       //     txtParentLocCode.Text = "";
         //   lblLocLevel.Text = "1";
            PopulateLocation(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Export report data from gridview to excel worksheet.
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
            if (gvLocMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["LOCMASTER"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'> Location Master</font></u></b>");
                //hw.WriteLine("<br>");
                //dgGrid.HeaderStyle.Font.Bold = true;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=LocationMaster.xls");  
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

    #region SELECTED INDEX CHANGED EVENT
    /// <summary>
    /// Get parent location code for each location selected from location dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlParentLocCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlParentLocCode.SelectedIndex > 0)
            {
             //   int locLevel = int.Parse(lblLocLevel.Text.Trim());
             //   txtParentLocCode.Text = Convert.ToString(ddlParentLocCode.SelectedValue.ToString());
             //   lblLocLevel.Text = (locLevel + 1).ToString();
               // oPRP.LocLevel = int.Parse(lblLocLevel.Text.Trim());
               // oPRP.LocCode = txtParentLocCode.Text.Trim();
            //    txtParentLocName.Text = ddlParentLocCode.SelectedItem.Text.Trim();
             //   ViewState["ParentLocCode"] = ddlParentLocCode.SelectedValue.ToString();

                ddlParentLocCode.DataSource = null;
                DataTable dt = oDAL.GetLocationCode(Session["COMPANY"].ToString(), oPRP.LocCode, oPRP.LocLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlParentLocCode.DataSource = dt;
                    ddlParentLocCode.DataTextField = "LOC_NAME";
                    ddlParentLocCode.DataValueField = "LOC_CODE";
                    ddlParentLocCode.DataBind();
                    ddlParentLocCode.Items.Insert(0, "-- Select Parent Location --");
                    ddlParentLocCode.Focus();
                }
                else
                {
                    txtLocationDesc.Focus();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}