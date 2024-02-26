using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.IO;
using System.Text;
using BarcodeLib;
using System.Drawing;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;

public partial class PrintGatePass : System.Web.UI.Page
{
    TYPE type;
    Barcode barcode;
    bool ApproveStatus = true;
    GatePassGeneration_DAL oDAL;
    GatePassGeneration_PRP oPRP;
    public PrintGatePass()
    {
        oPRP = new GatePassGeneration_PRP();
        barcode = new Barcode();
        type = TYPE.CODE128;
    }
    ~PrintGatePass()
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
        oDAL = new GatePassGeneration_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Print gate pass details page load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string _GatePassCode = "";            
            if (Convert.ToString(Request.QueryString["GPNO"]).Trim() != "")
            {
                _GatePassCode = Convert.ToString(Request.QueryString["GPNO"]).Trim();
                ApproveStatus = bool.Parse(Request.QueryString["APPROVE_STATUS"].ToString());
            }
            clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "PRINT_GATEPASS");
            System.Drawing.Image myimg = barcode.Encode(type, _GatePassCode, Color.Black, Color.White, 400, 150);
            File.Delete(MapPath("~/Images/GatePassBarcode.temp.jpg"));
            myimg.Save(MapPath("~/Images/GatePassBarcode.temp.jpg"));
            myimg.Dispose();
            imgBarcode.ImageUrl = "~/Images/GatePassBarcode.temp.jpg";

            FillGatePassDetails(_GatePassCode);

            System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>");
            System.Web.HttpContext.Current.Response.Write("window.print();");
            System.Web.HttpContext.Current.Response.Write("</SCRIPT>");
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);   
        }
    }
    #endregion

    #region PRIVATE FUNCTION
    private void FillGatePassDetails(string _GatePassCode)
    {
        gvGatePass.DataSource = null;
        System.Data.DataTable dt = new System.Data.DataTable();
        dt = oDAL.GetPrintGatepassDetails(_GatePassCode, ApproveStatus);
        gvGatePass.DataSource = dt;
        gvGatePass.DataBind();
        if (Session["COMPANY"].ToString() == "IT")
        {
            gvGatePass.Columns[2].Visible = false;
        }
        else
        {
            gvGatePass.Columns[1].Visible = false;
            gvGatePass.Columns[4].Visible = false;
        }

        DataRow dr = dt.Rows[0];
        lblGPNO.Text = dr["GATEPASS_CODE"].ToString();
        lblGPDate.Text = dr["GATEPASS_DATE"].ToString();
        //lblBearer.Text = dr["GATEPASS_BEARER_NAME"].ToString();
        //lblCarrer.Text = dr["GATEPASS_CARRIER_NAME"].ToString();
        //lblPurpose.Text = dr["PURPOSE"].ToString();
        //if (dr["GATEPASS_EMPLOYEE_CODE"].ToString().Trim() != "")
        //{
        //    lblVendEmp.Text = "Employee";
        //    lblVendEmpID.Text = dr["GATEPASS_EMPLOYEE_CODE"].ToString();
        //    lblVendEmpName.Text = dr["GATE_PASS_FOR"].ToString().Split(';')[1].Trim();
        //    lblVendAddress.Text = "";
        //}
        //else if (dr["VENDOR_CODE"].ToString().Trim() != "")
        //{
        //    lblVendEmp.Text = "Vendor";
        //    lblVendEmpID.Text = dr["VENDOR_CODE"].ToString();
        //    lblVendEmpName.Text = dr["VENDOR_NAME"].ToString();
        //    lblVendAddress.Text = dr["VENDOR_ADDRESS"].ToString();
        //}
        if (dr["ASSET_LOCATION"].ToString().Trim() == "")
            lblForLocation.Text = "N/A";
        else
            lblForLocation.Text = dr["ASSET_LOCATION"].ToString();
        if(dr["DEST_LOCATION"].ToString().Trim() == "")
            lblToLocation.Text = "N/A";
        else
            lblToLocation.Text = dr["DEST_LOCATION"].ToString();

        lblTotalAssets.Text = dr["TOTAL"].ToString();
    }

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Print GatePass");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    #endregion

    /*
    protected void btnExportToPdf_Click(object sender, EventArgs e)
    {
        renderPDF = true;
    }

    protected override void Render(HtmlTextWriter writer)
    {
        if (renderPDF == true)
        {
            MemoryStream mem = new MemoryStream();
            StreamWriter twr = new StreamWriter(mem);
            HtmlTextWriter myWriter = new HtmlTextWriter(twr);
            base.Render(myWriter);
            myWriter.Flush();
            myWriter.Dispose();
            StreamReader strmRdr = new StreamReader(mem);
            strmRdr.BaseStream.Position = 0;
            string pageContent = strmRdr.ReadToEnd();
            strmRdr.Dispose();
            mem.Dispose();
            writer.Write(pageContent);
            CreatePDFDocument(pageContent);
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter tw = new HtmlTextWriter(new System.IO.StringWriter(sb));
            base.Render(tw);
            // get the captured markup as a string
            string pageSource = tw.ToString();
            //Get the rendered content
            string sContent = sb.ToString();
            //Now output it to the page, if you want
            writer.Write(sContent);
        }
    }

    public void CreatePDFDocument(string strHtml)
    {
        string strHTMLpath = Server.MapPath("MyHTML.html");
        StreamWriter strWriter = new StreamWriter(strHTMLpath, false, Encoding.UTF8);
        strWriter.Write(strHtml);
        strWriter.Close();
        string strFileName = HttpContext.Current.Server.MapPath("map1.pdf"); //    strFileName    "C:\\Inetpub\\wwwroot\\Test\\map1.pdf" 
        // step 1: creation of a document-object
        Document document = new Document();
        // step 2:
        // we create a writer that listens to the document
        PdfWriter.GetInstance(document, new FileStream(strFileName, FileMode.Create));
        StringReader se = new StringReader(strHtml);
        TextReader tr = new StreamReader(Server.MapPath("MyHTML.html"));

        //add the collection to the document
        document.Open();
        /////////
        iTextSharp.text.html.simpleparser.HTMLWorker worker = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

        worker.StartDocument();

        //// step 5: parse the html into the document
        worker.Parse(tr);

        //// step 6: close the document and the worker
        worker.EndDocument();

        //worker.Parse(tr); // getting error "Illegal characters in path"
        document.Close();
        ShowPdf(strFileName);
    }

    public void ShowPdf(string strFileName)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
        Response.ContentType = "application/pdf";
        Response.WriteFile(strFileName);
        Response.Flush();
        Response.Clear();
    }
    */
}