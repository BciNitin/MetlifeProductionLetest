using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public class ConvertToExcel
{
    public Tuple<bool, DataTable, string> ValidateFileReaded(FileUpload AssetFileUpload, string Operation)
    {
        DataTable dt = new DataTable();
        string FilePath = "";
        try
        {
            string strFileName = AssetFileUpload.FileName;
            if (strFileName != "")
            {
                String fileExt = System.IO.Path.GetExtension(AssetFileUpload.FileName);
                if (fileExt.ToUpper() != ".XLSX" && fileExt.ToUpper() != ".XLS")
                {
                    return new Tuple<bool, DataTable, string>(false, dt, "Invalid File extension. Please Upload excel file.");
                }
                else
                {
                    if (AssetFileUpload.HasFile)
                    {
                        if (fileExt == ".xls" || fileExt == ".XLS" || fileExt == ".xlsx")
                        {
                            FilePath = HttpContext.Current.Server.MapPath("~\\UploadedFiles\\" + AssetFileUpload.FileName);
                            File.Delete(FilePath);
                            AssetFileUpload.SaveAs(FilePath);
                        }
                    }
                    else
                    {
                        dt = new DataTable();
                        return new Tuple<bool, DataTable, string>(false, dt, "There is no file attached.");
                    }
                }
            }
            else
            {
                dt = new DataTable();
                return new Tuple<bool, DataTable, string>(false, dt, "There is no file attached.");
            }
            dt = ReadExcelToDatatable(FilePath);
            bool res = ValidateHeaders(dt, Operation);
            if (res)
            {
                return new Tuple<bool, DataTable, string>(res, dt, "success");
            }
            else
            {
                dt = new DataTable();
                return new Tuple<bool, DataTable, string>(false, dt, "Invalid File Header");
            }
        }
        catch (Exception ex)
        {
            dt = new DataTable();
            return new Tuple<bool, DataTable, string>(false, dt, ex.ToString());
        }
    }
    public static DataTable ReadExcelToDatatable(string FilePath)
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
    public static bool ValidateHeaders(DataTable dt, string Methods)
    {
        List<string> headers = new List<string>();
        if (Methods == "ITASSET")
        {
            headers = new List<string>() {
                "SERIAL_NUMBER","ASSET_LOCATION","ASSET_DOMAIN","ASSET_TYPE","ASSET_MAKE","MODEL_NAME","ASSET_SUB_STATUS","ASSET_TAG","ASSET_RFID","FLOOR",
                "ASSET_PROCESSOR","STORE","ASSET_RAM","ASSET_HDD","AMC_WARRANTY","AMC_WARRANTY_START_DATE","AMC_WARRANTY_END_DATE","GRN_NO","GRN_DATE",
                "PO_NUMBER","PO_DATE","INVOICE_NO","INVOICE_DATE","SCOPE","PURCHASE_COST","OEM_VENDOR","REMARKS"
            };
        }
        else if (Methods == "FACILITIESASSET")
        {
            headers = new List<string>() {
                "ASSET_FAR_TAG","ASSET_LOCATION","FLOOR","ASSET_FAR_LIFE","ASSET_TYPE","ASSET_SUB_CATEGORY","PROCUREMENT_BUDGET","GRN_NO","OEM_VENDOR","ASSET_MAKE","MODEL_NAME",
                "ASSET_RFID","ASSET_WORKING_STATUS","IDENTIFIER_LOCATION","WARRANTY_STATUS","AMC_WARRANTY_START_DATE","AMC_WARRANTY_END_DATE","PO_NUMBER",
                "PO_DATE","INVOICE_NO","INVOICE_DATE","IN_SERVICE_DATE","ASSET_END_LIFE","REMARKS"
            };

        }
        else if (Methods == "ITASSETUPDATE")
        {
            headers = new List<string>() {
                "ASSET_LOCATION","FLOOR","ASSET_FAR_LIFE","ASSET_TYPE","ASSET_SUB_CATEGORY","PROCUREMENT_BUDGET","GRN_NO","OEM_VENDOR","ASSET_MAKE","MODEL_NAME",
                "ASSET_RFID","ASSET_FAR_TAG","ASSET_WORKING_STATUS","IDENTIFIER_LOCATION","WARRANTY_STATUS","AMC_WARRANTY_START_DATE","AMC_WARRANTY_END_DATE","PO_NUMBER",
                "PO_DATE","INVOICE_NO","INVOICE_DATE","IN_SERVICE_DATE","ASSET_END_LIFE","REMARKS"
            };

        }
        else if (Methods == "FACILITIESASSETUPDATE")
        {
            headers = new List<string>() {
                "ASSET_LOCATION","FLOOR","ASSET_FAR_LIFE","ASSET_TYPE","ASSET_SUB_CATEGORY","PROCUREMENT_BUDGET","GRN_NO","OEM_VENDOR","ASSET_MAKE","MODEL_NAME",
                "ASSET_RFID","ASSET_FAR_TAG","ASSET_WORKING_STATUS","IDENTIFIER_LOCATION","WARRANTY_STATUS","AMC_WARRANTY_START_DATE","AMC_WARRANTY_END_DATE","PO_NUMBER",
                "PO_DATE","INVOICE_NO","INVOICE_DATE","IN_SERVICE_DATE","ASSET_END_LIFE","REMARKS"
            };

        }
        else if (Methods == "ITAllocation")
        {
            headers = new List<string>() {
                "SERIAL_NUMBER","ASSET_ALLOCATION_TO","SUB_ASSET_ALLOCATION_TO","ALLOCATION_TYPE","SITE_LOCATION","FLOOR","MEETING_ROOM","EMPLOYEE_CODE","HOST_NAME",
                "EXPECTED_RETURN_DATE","NO_OF_DUE_DATE","IDENTIFIER_LOCATION","ALLOCATION_DATE","TICKET_NO","SEAT_NO","REMARKS"
            //"SERIAL_NUMBER","ASSET_ALLOCATION_TO","ALLOCATION_TYPE","SITE_LOCATION","FLOOR","SEAT_NO","EMPLOYEE_CODE",
            //    "HOST_NAME","EXPECTED_RETURN_DATE","NO_OF_DUE_DATE","IDENTIFIER_LOCATION","REMARKS","ALLOCATION_DATE","TICKET_NO"
            };
        }
        else if (Methods == "FacilitiesAllocation")
        {
            headers = new List<string>() {
                // "ASSETCODE","RFIDTAG","EXPECTEDRETURNDATE","SITELOCATION","ASSETALLOCATIONTO",
                //"ALLOCATIONTYPE","FLOOR","MEETINGROOM","EMPLOYEECODE","STATUS","NOOFDUEDATE","IDENTIFIERLOCATION","REMARKS","ALLOCATIONDATE"
            //"ASSET_FAR_TAG","ASSET_ALLOCATION_TO","SUB_ASSET_ALLOCATION_TO","ALLOCATION_TYPE","SITE_LOCATION","FLOOR","MEETING_ROOM",
            //    "EXPECTED_RETURN_DATE","NO_OF_DUE_DATE","IDENTIFIER_LOCATION","ALLOCATION_DATE","REMARKS"
            "ASSET_FAR_TAG","ASSET_ALLOCATION_TO","SUB_ASSET_ALLOCATION_TO","ALLOCATION_TYPE","SITE_LOCATION","FLOOR","MEETING_ROOM","IDENTIFIER_LOCATION","ALLOCATION_DATE","REMARKS"
            };
        }
        else if (Methods == "ITDeallocation")
        {
            headers = new List<string>() {
                "SERIAL_NUMBER", "DEALLOCATION_DATE","ASSET_SUB_STATUS","REMARKS","SITE","FLOOR","STORE"  };
        }
        else if (Methods == "FacilitiesDeallocation")
        {
            headers = new List<string>() {
                "ASSET_FAR_TAG", "DEALLOCATION_DATE","ASSET_SUB_STATUS","REMARKS","SITE","FLOOR"  };
        }
        else if (Methods == "ITScrap")
        {
            headers = new List<string>() {
                "SERIAL_NUMBER","REMARKS","SCRAP_DATE"  };
        }
        else if (Methods == "FacilitiesScrap")
        {
            headers = new List<string>() {
                "ASSET_FAR_TAG","REMARKS","SCRAP_DATE"  };
        }
        else if (Methods == "ITTRANSFER")
        {
            headers = new List<string>() {
                "DOCUMENT_NO", "GATEPASS_DATE",  "GATEPASS_TYPE", "EXP_RETURN_DATE","DESTINATION_LOCATION", "GATEPASS_REMARKS", "SERIAL_NUMBER"  };
        }

        else if (Methods == "FACILITIESTRANSFER")
        {
            headers = new List<string>() {
                "DOCUMENT_NO", "GATEPASS_DATE",  "GATEPASS_TYPE", "EXP_RETURN_DATE", "DESTINATION_LOCATION", "GATEPASS_REMARKS", "ASSET_FAR_TAG" };
        }

        List<string> headerList = headers.ToList();
        List<string> columnNames = dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName.ToUpper())
                                 .ToList();

        if (!headers.SequenceEqual(columnNames))
            return false;
        return true;
    }

    public static bool isValidateDate(string DateValue, out string DateOutput)
    {
        //if(DateValue.Length>11)
        //{
        //    DateTime dateTime13 = Convert.ToDateTime(DateValue);
        //    //DateValue = DateValue.Substring(0, 11);
        //    //CultureInfo provider = CultureInfo.InvariantCulture;
        //    //dateTime13 = DateTime.ParseExact(DateValue, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, provider, DateTimeStyles.None);
        //    string temp = dateTime13.ToString("dd-MMM-yyyy");
        //}
        //else
        //{
        //    double dateS = double.Parse(DateValue);
        //    DateValue = DateTime.FromOADate(dateS).ToString("dd-MMM-yyyy");
        //}
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
        catch(Exception ex)
        {
            DateOutput = "";
            return false;
        }
        
    }

}