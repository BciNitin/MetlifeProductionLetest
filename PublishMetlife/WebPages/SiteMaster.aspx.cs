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

public partial class SiteMaster : System.Web.UI.Page
{

    SiteMaster_DAL oDAL;
    SiteMaster_PRP oPRP;
     public SiteMaster()
    {
        oPRP = new SiteMaster_PRP();
    }
     ~SiteMaster()
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
         oDAL = new SiteMaster_DAL(Session["DATABASE"].ToString());
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
                 string _strRights = clsGeneral.GetRights("SITE_MASTER", (DataTable)Session["UserRights"]);
                 clsGeneral._strRights = _strRights.Split('^');
                 clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "SITE_MASTER");
                 if (clsGeneral._strRights[0] == "0")
                 {
                     Response.Redirect("UnauthorizedUser.aspx", false);
                 }
                GetSiteDetails(Session["COMPANY"].ToString());
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
         clsGeneral.LogErrorToLogFile(ex, "Site Master");
         if (!ex.Message.ToString().Contains("Thread was being aborted."))
         {
             clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
             catch { } Server.Transfer("Error.aspx");
         }
     }

    

     /// <summary>
     /// Get employee details to be populated into gridview.
     /// </summary>
     private void GetSiteDetails(string CompCode)
     {
         DataTable dt = new DataTable();
         dt = oDAL.GetSite(CompCode);
         gvEmpMaster.DataSource = Session["SITEMASTER"] = dt;
         gvEmpMaster.DataBind();
     }
     #endregion
   
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
            oPRP.SiteCode = txtSteCode.Text.Trim().ToUpper();
            oPRP.SiteAddress = txtSiteAddress.Text.Trim().ToUpper();
            oPRP.Description = txtDescription.Text.Trim();
            oPRP.ContactNo = txtPhone.Text.Trim();
            oPRP.Active = chkSetStatus.Checked;
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            bool bResp = oDAL.SaveUpdateSite("SAVE", oPRP);
            if (!bResp)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Duplicate Site Code.');", true);
                txtSteCode.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            GetSiteDetails(Session["COMPANY"].ToString());
            btnSubmit.Enabled = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {

    }
    protected void gvEmpMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvEmpMaster.Rows[e.RowIndex];
            oPRP.SiteCode = ((Label)gvRow.FindControl("lblESiteCode")).Text.Trim().ToUpper();
            oPRP.SiteAddress = ((TextBox)gvRow.FindControl("txtESiteAddress")).Text.Trim().ToUpper();
            oPRP.Active = ((CheckBox)gvRow.FindControl("chkEditActive")).Checked;
            oPRP.ContactNo = ((TextBox)gvRow.FindControl("txtEPhone")).Text.Trim();
            oPRP.Description = ((TextBox)gvRow.FindControl("txtEDescription")).Text.Replace("'", "`").Trim();
         //  oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oDAL.SaveUpdateSite("UPDATE", oPRP);

            gvEmpMaster.EditIndex = -1;
            GetSiteDetails(Session["COMPANY"].ToString());
           
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void gvEmpMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            GridViewRow gvRow = (GridViewRow)gvEmpMaster.Rows[e.RowIndex];
         //   oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.SiteCode = ((Label)gvRow.FindControl("lblSiteCode")).Text.Trim();
            //   oPRP.ParentLocCode = ((Label)gvRow.FindControl("lblParentLocCode")).Text.Trim();
            //  oPRP.LocLevel = int.Parse(((Label)gvRow.FindControl("lblLoclevel")).Text.Trim());
            string DelRslt = string.Empty;
            if (oDAL.isSiteExistAnywhere(oPRP.SiteCode))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Not authorised to delete this site because already exist in Current Inventory.');", true);
            }
            else
            {
                DelRslt = oDAL.DeleteLocation(oPRP.SiteCode, Session["COMPANY"].ToString());
            }
           
            if (DelRslt == "SUCCESS")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Site is deleted successfully.');", true);
            GetSiteDetails(Session["COMPANY"].ToString());
           
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void gvEmpMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            gvEmpMaster.EditIndex = e.NewEditIndex;
            GetSiteDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void gvEmpMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
          try
        {
            gvEmpMaster.PageIndex = e.NewPageIndex;
            GetSiteDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void gvEmpMaster_RowDataBound(object sender, GridViewRowEventArgs e)
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
        {
            HandleExceptions(ex);
        }
    }
    protected void gvEmpMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvEmpMaster.EditIndex = -1;
            GetSiteDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void gvEmpMaster_SelectedIndexChanged(object sender, EventArgs e)
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
            if (gvEmpMaster.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["SITEMASTER"];
                dt.TableName = "SiteMasterReport";
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //hw.WriteLine("<b><u><font size='5'> " + Session["COMPANY"].ToString() + "</font></u></b>");
                //hw.WriteLine("<br>");
                //hw.WriteLine("<b><u><font size='4'> Site Master</font></u></b>");
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
}