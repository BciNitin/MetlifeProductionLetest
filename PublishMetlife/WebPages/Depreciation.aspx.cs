using MobiVUE_ATS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPages_Depreciation : System.Web.UI.Page
{
    Depreciation_DAL oDAL = default(Depreciation_DAL);
    private string CompCode
    {
        get
        {
            return HttpContext.Current.Session["COMPANY"] == null ? string.Empty : Convert.ToString(HttpContext.Current.Session["COMPANY"]);
        }
    }

    public string DataBaseName
    {
        get
        {
            return Session["DATABASE"] == null ? string.Empty : Convert.ToString(Session["DATABASE"]);
        }
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            oDAL = new Depreciation_DAL(DataBaseName);
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("DEPARTMENT_MASTER", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "DEPARTMENT_MASTER");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }

            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }

    }

    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;

            if (string.IsNullOrEmpty(txtToDate.Text.Trim()))
            {
                lblErrorMsg.Text = "To date is required !";
                return;
            }
            else if (!this.IsValidDate(txtToDate.Text.Trim()))
            {
                lblErrorMsg.Text = "Enter valid date format dd/MMM/yyyy";
                return;
            }

            DateTime? ToDate = this.GetDate(txtToDate.Text.Trim());

            DataTable tbl = oDAL.GetData(CompCode, ToDate);
            this.gvDepreciation.DataSource = Session["Depreciation"] = tbl;
            this.gvDepreciation.DataBind();
            this.lblRecordCount.Text = string.Format("Total record found: {0}", tbl.Rows.Count);
            btnExport.Enabled = tbl.Rows.Count > 0;



        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
            HandleExceptions(ex);
        }
    }
    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

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
            if (clsGeneral._strRights[4] == "0")
            {
                Response.Redirect("UnauthorizedUser.aspx");
            }

            if (gvDepreciation.Rows.Count > 0)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["Depreciation"];
                //DataSet dsExport = new DataSet();
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.DataBind();
                //dgGrid.RenderControl(hw);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=Depreciation.xls");
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


    private DateTime? GetDate(string Value)
    {
        DateTime _date = default(DateTime);
        try
        {
            bool x = DateTime.TryParseExact(Value, new string[] { "dd/MMM/yyyy" },
                                 CultureInfo.InvariantCulture,
                                 DateTimeStyles.None,
                                 out _date);
            if (x == true)
            {
                return (DateTime?)_date;
            }
            else
            {
                return default(DateTime?);
            }
        }
        catch (Exception)
        {
            return default(DateTime?);
        }
    }

    private bool IsValidDate(string Value)
    {
        try
        {
            string[] formats = { "dd/MMM/yyyy" };
            DateTime dateValue;

            if (DateTime.TryParseExact(Value, formats,
                               CultureInfo.InvariantCulture,
                              DateTimeStyles.None,
                              out dateValue))
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Asset Allocation");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    protected void gvDepreciation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvDepreciation.PageIndex = e.NewPageIndex;
            btnSubmit_Click(null, null);

        }
        catch (Exception)
        {

            throw;
        }
    }
}