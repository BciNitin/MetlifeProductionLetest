using ClosedXML.Excel;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPages_ImportAssets : System.Web.UI.Page
{
    DataTable dtFileData;
    string strFilePath = "";
    string strDupFilePath = @"C:\ATS\";
    string DatabaseCon = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:onselectionChange(); ", true);
        this.Page.Form.Attributes.Add("enctype", "multipart/form-data");
        ddlImportType.Attributes.Add("onChange", "onselectionChange();");
        ddlImportType.Focus();
        ddlImportType.Enabled = true;
        DatabaseCon = Session["DATABASE"].ToString();
    }

    protected void btnUpload_Click(object sender, ImageClickEventArgs e)
    {
        string strFileName = AssetFileUpload.FileName;
        if (strFileName != "")
        {
            String fileExt = System.IO.Path.GetExtension(AssetFileUpload.FileName);
            if (fileExt.ToUpper() != ".XLSX" && fileExt.ToUpper() != ".XLS")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select an excel file (.xls/.xlsx) only.');", true);
                return;
            }
            if (UploadFile())
            {
                dtFileData = ReadExcelToDatatable(strFilePath);
                Session["ASSET"] = dtFileData;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Enter Submit to save/update file data.');", true);
                ddlImportType.Enabled = false;
                btnSubmit.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Operation Failed! Please Try again.');", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Browse & select excel file only.');", true);
            return;
        }
    }



    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        //bool bInValid = false;
        dtFileData = (DataTable)Session["ASSET"];
        if (dtFileData.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : No Data Found. File is empty.');", true);
            return;
        }

        if (!ValidateHeaders(dtFileData))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Template File Headers are not matached.');", true);
            return;
        }

        if (dtFileData.Rows.Count > 0)
        {
            if (ddlImportType.SelectedValue == "ASSET")
            {
                SaveAssetAcq(dtFileData);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Files Cannot be Empty.');", true);
            return;
        }

    }

    #region Save Asset Acq

    private void SaveAssetAcq(DataTable dt)
    {
        DataTable dtInvalidData = new DataTable();
        dtInvalidData = dt.Clone();
        dtInvalidData.Columns.Add("STATUS");
        dtInvalidData.Columns.Add("ERROR_MESSAGE");
        int Cnt = 0;
        foreach (DataRow dr in dt.Rows) 
        {           
            AssetAcquisition_PRP oPRP = new AssetAcquisition_PRP();
            oPRP.AssetCode = dr.Field<string>("ASSET_CODE");            
            oPRP.AssetType = dr.Field<string>("ASSET_TYPE");
            oPRP.AssetMakeName = dr.Field<string>("ASSET_MAKE");
            oPRP.AssetModelName = dr.Field<string>("MODEL_NAME");
            oPRP.AssetLocation = dr.Field<string>("ASSET_LOCATION");
            oPRP.IdentifierLocation = dr.Field<string>("ASSET_LOCATION");
            oPRP.AssetSerialCode = dr.Field<string>("SERIAL_CODE");
            oPRP.AssetTag = dr.Field<string>("TAG_ID");
            oPRP.AssetRFID = dr.Field<string>("ASSET_RFID");
            oPRP.store = dr.Field<string>("STORE");
            oPRP.AssetProcessor = dr.Field<string>("ASSET_PROCESSOR");
            oPRP.AssetRAM = dr.Field<string>("ASSET_RAM");
            oPRP.AssetHDD = dr.Field<string>("ASSET_HDD");
            oPRP.VendorCode = dr.Field<string>("VENDOR_CODE");
            oPRP.GRNNo = dr.Field<string>("GRN_NO");
            oPRP.AMC_Warranty = dr.Field<string>("AMC_WARRANTY");
            oPRP.ProcurementBudget = dr.Field<string>("PROCUREMENT_BUDGET");

            oPRP.AMC_Wrnty_Start_Date = dr.Field<string>("AMC_WARRANTY_START_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim();
            if (oPRP.AMC_Wrnty_Start_Date != "")
            {
                string date = "";
                if (isValidateDate(oPRP.AMC_Wrnty_Start_Date, out date))
                {
                    oPRP.AMC_Wrnty_Start_Date = date;
                }
                else
                {
                    dtInvalidData.Rows.Add(dr.ItemArray);
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "Warrenty Start Date is not matched.";
                    continue;
                }
            }

            oPRP.AMC_Wrnty_End_Date = dr.Field<string>("AMC_WARRANTY_END_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim();
            if (oPRP.AMC_Wrnty_End_Date != "")
            {
                string date = "";
                if (isValidateDate(oPRP.AMC_Wrnty_End_Date, out date))
                {
                    oPRP.AMC_Wrnty_End_Date = date;
                }
                else
                {
                    dtInvalidData.Rows.Add(dr.ItemArray);
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "Warrenty End Date is not matched.";
                    continue;
                }
            }

            oPRP.PurchaseOrderNo = dr.Field<string>("PO_NUMBER");
            oPRP.PODate = dr.Field<string>("PO_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim();
            if (oPRP.PODate != "")
            {
                string date = "";
                if (isValidateDate(oPRP.PODate, out date))
                {
                    oPRP.PODate = date;
                }
                else
                {
                    dtInvalidData.Rows.Add(dr.ItemArray);
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "PO Date format is not matched.";
                    continue;
                }
            }

            oPRP.InvoiceDate = dr.Field<string>("INVOICE_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim();
            oPRP.InvoiceNo = dr.Field<string>("INVOICE_NO");
            if (oPRP.InvoiceDate != "")
            {
                string date = "";
                if (isValidateDate(oPRP.InvoiceDate, out date))
                {
                    oPRP.InvoiceDate = date;
                }
                else
                {
                    dtInvalidData.Rows.Add(dr.ItemArray);
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "Invoice Date format is not matched.";
                    continue;
                }
            }
            oPRP.Remarks = dr.Field<string>("REMARKS");
            oPRP.SubStatus = "WORKING";
            oPRP.CompCode = Convert.ToString(Session["COMPCODE"]);
            oPRP.CreatedBy = Convert.ToString(Session["CURRENTUSER"]);
            AssetAcq_DAL oDAL = new AssetAcq_DAL(DatabaseCon);
            string message = oDAL.SaveAssetAcquisition(oPRP);
            if (message != "SUCCESS")
            {
                dtInvalidData.Rows.Add(dr.ItemArray);
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = message;
                continue;
            }
            else
                Cnt++;            
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of "+  dt.Rows.Count + ".');", true);
    }

    private bool ValidateHeaders(DataTable dt)
    {
        List<string> headers = new List<string>();
        if (ddlImportType.SelectedValue == "ASSET")
        {
            headers = new List<string>() {
                "ASSET_CODE", "ASSET_TYPE", "ASSET_MAKE", "MODEL_NAME", "ASSET_RFID", "SERIAL_CODE", "TAG_ID", "PROCUREMENT_BUDGET", "STORE",
                "ASSET_PROCESSOR", "ASSET_RAM", "ASSET_HDD", "VENDOR_CODE", "GRN_NO", "AMC_WARRANTY", "AMC_WARRANTY_START_DATE",
                "AMC_WARRANTY_END_DATE", "PO_NUMBER", "PO_DATE", "INVOICE_NO", "INVOICE_DATE", "ASSET_LOCATION", "REMARKS"  };
        }

        List<string> headerList = headers.ToList();
        List<string> columnNames = dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();

        if (!headers.SequenceEqual(columnNames))
            return false;
        return true;
    }

    private bool isValidateDate(string DateValue, out string DateOutput)
    {
        #region
        //double dateS = double.Parse(DateValue);
        //DateValue = DateTime.FromOADate(dateS).ToString("dd-MMM-yyyy");
        //DateValue = DateValue.Substring(0, 11);
        //DateTime dt;
        //if (DateTime.TryParseExact(DateValue,
        //                            "dd-MMM-yyyy",
        //                            CultureInfo.InvariantCulture,
        //                            DateTimeStyles.None,
        //    out dt))
        //{
        //    DateOutput = dt.ToString("dd/MMM/yyyy");
        //    return true;
        //}
        //else
        //{
        //    DateOutput = "";
        //    return false;
        //}
        #endregion
        try
        {
            double dateS = double.Parse(DateValue);
            DateValue = DateTime.FromOADate(dateS).ToString("dd-MMM-yyyy");

            DateValue = DateValue.Substring(0, 11);
            DateTime dt;

            if (DateTime.TryParseExact(DateValue,
                                        "dd-MMM-yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out dt))
            {
                DateOutput = dt.ToString("dd/MMM/yyyy");
                return true;
            }
            else
            {
                DateOutput = "";
                return false;
            }
        }
        catch (Exception ex)
        {
            DateOutput = "";
            return false;
        }
    }

    #endregion

    private DataTable ReadExcelToDatatable(string FilePath)
    {
        DataTable dt = new DataTable();
        using (XLWorkbook workBook = new XLWorkbook(FilePath))
        {
            //Read the first Sheet from Excel file.
            IXLWorksheet workSheet = workBook.Worksheet(1);

            var nonEmptyDataRows = workBook.Worksheet(1).RowsUsed();

            bool firstRow = true;
            foreach (var row in nonEmptyDataRows)
            {
                //Use the first row to add columns to DataTable.
                if (firstRow)
                {
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString());
                    }
                    firstRow = false;
                }
                else
                {
                    dt.Rows.Add();
                    int i = 0;
                    foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                        i++;
                    }
                }

            }
        }
        return dt;
    }

    private bool UploadFile()
    {
        String fileExt;
        bool bRes = false;

        fileExt = System.IO.Path.GetExtension(AssetFileUpload.FileName);
        if (AssetFileUpload.HasFile)
        {
            if (fileExt == ".xls" || fileExt == ".XLS" || fileExt == ".xlsx")
            {
                strFilePath = Request.PhysicalApplicationPath + "UploadedFiles\\" + AssetFileUpload.FileName;
                File.Delete(strFilePath);
                AssetFileUpload.SaveAs(strFilePath);
                bRes = true;
            }
        }
        return bRes;
    }

}
