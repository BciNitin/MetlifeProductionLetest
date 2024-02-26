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

public partial class WebPages_StoreMaster : System.Web.UI.Page
{

    #region PAGE CONSTRUCTOR & DECLARATIONS

    StoreMaster_DAL oDAL;
    StoreMaster_PRP oPRP;
    public WebPages_StoreMaster()
    {
        oPRP = new StoreMaster_PRP();
    }
    ~WebPages_StoreMaster()
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
        oDAL = new StoreMaster_DAL(Session["DATABASE"].ToString());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("STORE_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "STORE_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                GetStoreDetails();
                GetSiteDetails();
                Loadfloor();
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
    private void GetStoreDetails()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetStore(Session["COMPANY"].ToString());
        gvStoreMaster.DataSource = Session["STOREMASTER"] = dt;
        gvStoreMaster.DataBind();
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
        dt = oDAL.GetFloor(SiteCode,Session["COMPANY"].ToString());
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
            oPRP.StoreCode= oPRP.StoreName = txtStoreName.Text.Trim().ToUpper();
            oPRP.SiteCode = ddlSite.SelectedValue;
            oPRP.Floor = ddlFloor.SelectedValue;
            oPRP.Remarks = txtRemarks.Text.Trim();
            oPRP.CreatedBy = Convert.ToString(Session["CURRENTUSER"]);
            oPRP.CompCode = Convert.ToString(Session["COMPANY"]);

            oPRP.Active = chkSetStatus.Checked;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();

            if (oDAL.CheckDuplicatateStoreFloorSite(oPRP.StoreCode, oPRP.Floor, oPRP.SiteCode,oPRP.CompCode))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate Store Code.');", true);
                txtStoreName.Focus();
                return;
            }
            btnSubmit.Enabled = false;
            bool bResp = oDAL.SaveUpdateStore("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate Store Code.');", true);
                txtStoreName.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            GetStoreDetails();
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
            if (gvStoreMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["STOREMASTER"];
                dt.TableName = "StoreMasterReport";
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

    protected void gvStoreMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvStoreMaster.Rows[e.RowIndex];            
            oPRP.StoreCode = ((Label)gvRow.FindControl("lblStoreCode")).Text.Trim();
            oPRP.StoreName = ((Label)gvRow.FindControl("lblStoreName")).Text.Trim();
            oPRP.Floor = ((Label)gvRow.FindControl("lblFloor")).Text.Trim();
            oPRP.SiteCode = ((Label)gvRow.FindControl("lblSiteName")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkActive")).Checked;
            oPRP.Remarks = ((Label)gvRow.FindControl("lblDescription")).Text.Trim();
            oPRP.CompCode = Session["COMPANY"].ToString();
            string DelRslt = oDAL.DeleteStore(oPRP);

            if (DelRslt == "SUCCESS")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Store is deleted successfully.');", true);
            GetStoreDetails();

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvStoreMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            gvStoreMaster.EditIndex = e.NewEditIndex;
            GetStoreDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvStoreMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvStoreMaster.Rows[e.RowIndex];
            oPRP.StoreCode = ((Label)gvRow.FindControl("lblEStoreCode")).Text.Trim().ToUpper();
            oPRP.StoreName = ((Label)gvRow.FindControl("lblStoreName")).Text.Trim().ToUpper();
            oPRP.Floor = ((Label)gvRow.FindControl("lblFloor")).Text.Trim();
            oPRP.SiteCode = ((Label)gvRow.FindControl("lblSiteName")).Text.Trim();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
            oPRP.Remarks = ((TextBox)gvRow.FindControl("txtERemarks")).Text.Replace("'", "`").Trim();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oPRP.CompCode= Session["COMPANY"].ToString();
            oDAL.SaveUpdateStore("UPDATE", oPRP);

            gvStoreMaster.EditIndex = -1;
            GetStoreDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvStoreMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvStoreMaster.EditIndex = -1;
            GetStoreDetails();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvStoreMaster_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    //        ddlAgent.SelectedValue = hfAgentID.Value;

                    //        DropDownList ddlAgentFloor = (DropDownList)gvRow.FindControl("ddlEFloor");
                    //        DataTable dtfloor = new DataTable();
                    //        ddlAgentFloor.DataSource = null;
                    //        dtfloor = oDAL.GetFloor(ddlAgent.SelectedValue);
                    //        ddlAgentFloor.DataSource = dtfloor;
                    //        ddlAgentFloor.DataTextField = "FLOOR_CODE";
                    //        ddlAgentFloor.DataValueField = "FLOOR_CODE";
                    //        ddlAgentFloor.DataBind();
                    //        ddlAgentFloor.Items.Insert(0, "-- Select Site --");
                    //        ddlAgentFloor.SelectedValue = hfAgentIDFloor.Value;
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
        //TextBox box1 = (DropDownList)gvStoreMaster.Rows[gvStoreMaster.Rows.].Cells[2].FindControl("TextBox2");
        //DropDownList dl = (DropDownList)gvStoreMaster.Rows[e.currentrow].Cells[2].FindControl("DropDownProducts");

        //box1.Text = dl.SelectedValue.ToString();
    }
    protected void gvStoreMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvStoreMaster.PageIndex = e.NewPageIndex;
            GetStoreDetails();
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