using MobiVUE_ATS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;
using MobiVUE_ATS.PRP;
using MobiVUE_ATS.MyDLL;
using AjaxControlToolkit;
//using System.Windows.Forms;


public partial class WebPages_Reports : System.Web.UI.Page
{
    Report_DAL oDAL;
    public static DataTable reportdatatable;
    public string ReportIdFromtheHomePage = string.Empty;
    #region PAGE EVENT

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new Report_DAL(Session["DATABASE"].ToString());
    }

    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Report");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Response.Redirect("Error.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
        string RptId = Request.QueryString["ReportID"].ToString();
        ReportIdFromtheHomePage = RptId;
        string RptIDExact = string.Empty;
        int ReportID = 0;
        if (!IsPostBack)
        {
            if (Request.QueryString["ReportID"] != null)
            {
                if(Request.QueryString["ReportID"].ToString().Contains("STORE"))
                {
                    RptIDExact = Request.QueryString["ReportID"].ToString();
                    RptIDExact = RptIDExact.Remove(0, 6).Trim();
                }
                else
                {
                    int.TryParse(Request.QueryString["ReportID"].ToString(), out ReportID);
                    hdnReportID.Value = ReportID.ToString();
                }
                //btnExport.Enabled = false;
            }
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
            if (RptId.Contains("STORE"))
            {
                gvReportData.DataSource = null;
                DataTable dt = new DataTable();
                dt = oDAL.GetStoreReportDetails(RptIDExact, Session["COMPANY"].ToString());
                gvReportData.DataSource = Session["REPORT"] = reportdatatable = dt;
                gvReportData.DataBind();
                btnSearch.Visible = btnClear.Visible = false;
            }

            if (ReportID==2)
            {
                gvReportData.DataSource = null;
                DataTable dt = new DataTable();
                dt = oDAL.GetReportAssetStockinHomePage(Session["COMPANY"].ToString());
                gvReportData.DataSource = Session["REPORT"] = reportdatatable = dt;
                gvReportData.DataBind();
                btnSearch.Visible = btnClear.Visible = false;
            }

            if (ReportID == 3)
            {
                gvReportData.DataSource = null;
                DataTable dt = new DataTable();
                dt = oDAL.GetReportAssetAllocatedinHomePage(Session["COMPANY"].ToString());
                gvReportData.DataSource = Session["REPORT"] = reportdatatable = dt;
                gvReportData.DataBind();
                btnSearch.Visible = btnClear.Visible = false;
            }

            if (ReportID == 4)
            {
                gvReportData.DataSource = null;
                DataTable dt = new DataTable();
                dt = oDAL.GetReportAssetTransferinHomePage(Session["COMPANY"].ToString());
                gvReportData.DataSource = Session["REPORT"] = reportdatatable = dt;
                gvReportData.DataBind();
                btnSearch.Visible = btnClear.Visible = false;
            }

            if (ReportID == 5)
            {
                gvReportData.DataSource = null;
                DataTable dt = new DataTable();
                dt = oDAL.GetReportAssetScrappedinHomePage(Session["COMPANY"].ToString());
                gvReportData.DataSource = Session["REPORT"] = reportdatatable = dt;
                gvReportData.DataBind();
                btnSearch.Visible = btnClear.Visible = false;
            }
        }
        if (ReportIdFromtheHomePage.Contains("STORE") || Convert.ToInt32(ReportIdFromtheHomePage) ==2 || Convert.ToInt32(ReportIdFromtheHomePage) == 3
            || Convert.ToInt32(ReportIdFromtheHomePage) == 4 || Convert.ToInt32(ReportIdFromtheHomePage) == 5)
        {
            Session["REPORT"] = reportdatatable;
        }
        int.TryParse(hdnReportID.Value, out ReportID);
        DynamicControls(ReportID);


    }

    #endregion

    #region PRIVATE FUNCTION

    public void DynamicControls(int ReportID)
    {
        System.Data.DataTable dt = oDAL.GetReportDetails(ReportID);
        lblReportName.Text = dt.Rows.Count > 0 ? Convert.ToString(dt.Rows[0]["REPORT_NAME"]) : null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string InputType = Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]);
            if (InputType == "DEFAULT" || InputType == "SESSION")
            {
                string ClassName = Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                HiddenField txt = new HiddenField();
                txt.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                txt.Value = InputType == "DEFAULT" ? ClassName : Convert.ToString(Session[ClassName]).Replace("'","");                
                upSubmit.ContentTemplateContainer.Controls.Add(txt);
            }
            else
            {
                HtmlTableRow row = new HtmlTableRow();
                HtmlTableCell cell = new HtmlTableCell();
                cell.Attributes.Add("class", "text-right");

                Label lbl = new Label();
                lbl.ID = "lbl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                lbl.Text = Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]) + ": ";

                cell.Controls.Add(lbl);
                row.Cells.Add(cell);

                HtmlTableCell cell1 = new HtmlTableCell();

                if (Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "LIST_QUERY")
                {
                    DropDownList txt = new DropDownList();
                    txt.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                    txt.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                    string ddlQuery = Convert.ToString(dt.Rows[i]["DDL_TYPE_QUERY"]).Replace(":USERID:", "'" + Convert.ToString(Session["CURRENTUSER"]) + "'");
                    txt.DataSource = oDAL.GetDataTableInTransaction(ddlQuery);
                    txt.DataTextField = "D_TEXT";
                    txt.DataValueField = "D_VALUE";
                    txt.DataBind();
                    txt.Items.Insert(0, "-- Please Select --");
                    cell1.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                    cell1.Controls.Add(txt);
                }
                else if (Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "LIST")
                {
                    DropDownList txt = new DropDownList();
                    txt.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                    txt.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                    string ddlQuery = Convert.ToString(dt.Rows[i]["DDL_TYPE_QUERY"]).Replace(":USERID:", "'" + Convert.ToString(Session["CURRENTUSER"]) + "'");
                    txt.DataSource = Convert.ToString(dt.Rows[i]["DDL_TYPE_QUERY"]).Split(',').ToList();//oDAL.GetDataTableInTransaction(ddlQuery);
                                                                                                        //txt.DataTextField = "D_TEXT";
                                                                                                        //txt.DataValueField = "D_VALUE";
                    txt.DataBind();
                    txt.Items.Insert(0, "-- Please Select --");
                    cell1.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                    cell1.Controls.Add(txt);
                }
                else if ((Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "LOC_SEARCH") || (Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "LOC_TO"))
                {
                    TextBox txt = new TextBox();
                    txt.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                    txt.Text = "";
                    txt.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                    txt.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]));
                    cell1.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                    cell1.Controls.Add(txt);
                }
                else if ((Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "DATETIME"))
                {
                    TextBox txt = new TextBox();
                    txt.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                    txt.Text = "";
                    txt.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                    txt.Attributes.Add("onFocus", "showCalendarControl(this)");
                    cell1.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                    cell1.Controls.Add(txt);
                }
                else
                {   
                    TextBox txt = new TextBox();
                    txt.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                    txt.Text = "";
                    txt.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                    cell1.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                    cell1.Controls.Add(txt);
                }
                row.Cells.Add(cell1);



                i++;

                if (i < dt.Rows.Count)
                {
                    InputType = Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]);
                    if (InputType == "DEFAULT" || InputType == "SESSION")
                    {
                        string ClassName = Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                        HiddenField txt = new HiddenField();
                        txt.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                        txt.Value = InputType == "DEFAULT" ? ClassName : Convert.ToString(Session[ClassName]);
                        upSubmit.ContentTemplateContainer.Controls.Add(txt);
                    }
                    else
                    {
                        HtmlTableCell cell2 = new HtmlTableCell();
                        HtmlTableCell cell3 = new HtmlTableCell();
                        cell2.Attributes.Add("class", "text-right");

                        Label lbl2 = new Label();
                        lbl2.ID = "lbl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                        lbl2.Text = Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]) + ": ";
                        cell2.Controls.Add(lbl2);
                        if (Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "LIST_QUERY")
                        {
                            DropDownList txt = new DropDownList();
                            txt.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                            txt.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                            string ddlQuery = Convert.ToString(dt.Rows[i]["DDL_TYPE_QUERY"]).Replace(":USERID:", "'" + Convert.ToString(Session["CURRENTUSER"]) + "'");
                            txt.DataSource = oDAL.GetDataTableInTransaction(ddlQuery);
                            txt.DataTextField = "D_TEXT";
                            txt.DataValueField = "D_VALUE";
                            txt.DataBind();
                            txt.Items.Insert(0, "-- Please Select --");
                            cell3.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                            cell3.Controls.Add(txt);
                        }
                        else if (Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "LIST")
                        {
                            DropDownList txt = new DropDownList();
                            txt.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                            txt.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                            // string ddlQuery = Convert.ToString(dt.Rows[i]["DDL_TYPE_QUERY"]).Replace(":USERID:", "'" + Convert.ToString(Session["CURRENTUSER"]) + "'");
                            txt.DataSource = Convert.ToString(dt.Rows[i]["DDL_TYPE_QUERY"]).Split(',').ToList();//oDAL.GetDataTableInTransaction(ddlQuery);
                                                                                                                //txt.DataTextField = "D_TEXT";
                                                                                                                //txt.DataValueField = "D_VALUE";
                            txt.DataBind();
                            txt.Items.Insert(0, "-- Please Select --");
                            cell3.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                            cell3.Controls.Add(txt);
                        }
                        else if ((Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "LOC_SEARCH") || (Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "LOC_TO"))
                        {
                            TextBox txt1 = new TextBox();
                            txt1.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                            txt1.Text = "";
                            txt1.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                            txt1.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]));
                            cell3.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                            cell3.Controls.Add(txt1);
                        }
                        else if ((Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_TYPE"]) == "DATETIME"))
                        {
                            TextBox txt1 = new TextBox();
                            txt1.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                            txt1.Text = "";
                            txt1.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                            txt1.Attributes.Add("onFocus", "showCalendarControl(this)");
                            cell3.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                            cell3.Controls.Add(txt1);
                        }
                        else
                        {
                            TextBox txt1 = new TextBox();
                            txt1.ID = "ctrl" + Convert.ToString(dt.Rows[i]["REPORT_DTLS_ID"]);
                            txt1.Text = "";
                            txt1.CssClass = "textbox " + Convert.ToString(dt.Rows[i]["INPUT_CLASS_NAME"]);
                            cell3.Attributes.Add("data-class", Convert.ToString(dt.Rows[i]["INPUT_DISPLAY_NAME"]));
                            cell3.Controls.Add(txt1);
                        }
                        row.Cells.Add(cell2);
                        row.Cells.Add(cell3);
                    }
                }
                Table1.Rows.Add(row);
            }
        }
    }

    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            btnExport.Enabled = false;
            gvReportData.DataSource = Session["REPORT"] = null;
            gvReportData.DataBind();

            lblErrorMsg.Text = "";
            int ReportID = 0;
            int.TryParse(hdnReportID.Value, out ReportID);

            DataTable dt = oDAL.GetReportDetails(ReportID);

            DataTable dtable = new DataTable();
            dtable.Columns.Add("REPORT_DTLS_ID");
            dtable.Columns.Add("REPORT_ID");
            dtable.Columns.Add("INPUT_PARAM_NAME");
            dtable.Columns.Add("INPUT_PARAM_TYPE");
            dtable.Columns.Add("INPUT_PARAM_SIZE");
            dtable.Columns.Add("INPUT_PARAM_VALUE");

            string[] stringDataType = new string[] { "INT", "DECIMAL", "MONEY","DATETIME" };
            foreach (DataRow dr in dt.Rows)
            {
                int ReportDetailID = Convert.ToInt32(dr["REPORT_DTLS_ID"]);
                string txtValue = null;
                string txtType = Convert.ToString(dr["INPUT_PARAM_TYPE"]);

                Control ctrl = upSubmit.FindControl("ctrl" + Convert.ToString(dr["REPORT_DTLS_ID"]));
                if (ctrl is TextBox)
                {
                    TextBox txt = ((TextBox)ctrl);
                    if (!string.IsNullOrEmpty(txt.Text))
                    {
                        if (stringDataType.Contains(txtType))
                        {
                            txtValue = txt.Text;
                        }
                        else
                        {
                            txtValue = "'" + txt.Text + "'";
                        }
                    }
                }
                else if (ctrl is DropDownList)
                {
                    DropDownList ddl = ((DropDownList)ctrl);
                    if (ddl != null && ddl.SelectedIndex > 0)
                    {
                        txtValue = "'" + ddl.SelectedValue + "'";
                    }
                }
                else if (ctrl is HiddenField) 
                {
                    HiddenField txt = ((HiddenField)ctrl);
                    if (!string.IsNullOrEmpty(txt.Value))
                    {
                        if (stringDataType.Contains(txtType))
                        {
                            txtValue = txt.Value;
                        }
                        else
                        {
                            txtValue = "'" + txt.Value + "'";
                        }
                    }
                }
                dtable.Rows.Add(ReportDetailID, ReportID, Convert.ToString(dr["INPUT_PARAM_NAME"]), txtType, dr["INPUT_PARAM_SIZE"], txtValue);
            }

            DataTable dtReport = new DataTable();
            
            dtReport = oDAL.GetReportData(ReportID, dtable);

            if (dtReport.Rows.Count == 0)
            {
                lblErrorMsg.Text = "No Data Found.";
            }
            else 
            {
                btnExport.Enabled = true;
                gvReportData.AutoGenerateColumns = true;
                gvReportData.DataSource = Session["REPORT"] = dtReport;
                gvReportData.DataBind();
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
   
    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //if(gvReportData.Rows.Count==0)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no data for being exported.');", true);
            //    return;
            //}
            DataTable dt1 = new DataTable();
            if (Session["REPORT"] != null) {
                dt1 = (DataTable)Session["REPORT"];
            }
            if (dt1.Rows.Count > 0)
            {
                dt1.TableName = "Report";
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


    protected void gvReportData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReportData.PageIndex = e.NewPageIndex;
        gvReportData.DataSource = Session["REPORT"];
        gvReportData.DataBind();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        gvReportData.DataSource = null;
        gvReportData.DataBind();
    }
}