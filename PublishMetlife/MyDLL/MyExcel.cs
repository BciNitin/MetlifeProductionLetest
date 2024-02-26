using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.OleDb;
using Microsoft.Win32;
using System.Drawing;
using System.Data;

namespace MyDLL
{
    public class MyExcel
    {
        #region "variable Declaration"

        public bool IsProtected;                       // Is the Sheet is Protected
        public bool IsHeaderLocked;                     // Is the Headers of File should be Locked
        public bool IsCellLocked;                       // Is the Cells of File should be Locked
        public string ExcelPassword;                    // If Header or Cell will be Locked then Password for Sheet will be. 
        public bool IsCellBold;                         // Is the Cells should be Bold.
        public bool IsHeaderBold;                       // Is the Headers should be Bold.
        public bool IsBorder;                           // Is Excel file contains Border
        public System.Drawing.Color HeaderColor;        //Color of Header Background
        public System.Drawing.Color CellColor;          //Color of Rows Background
        public System.Data.DataTable DataTableToWrite;  //data table to Write in Excel
        public string Path;                             //Path of the file

        #endregion
        public void WriteInExcel(System.Data.DataTable dt, string xlsFilePath, bool IsProtected, bool IsHeaderLocked, bool IsCellLocked, bool IsCellBold, bool IsHeaderBold, bool IsBorder, string ExcelPassword, System.Drawing.Color HeaderColor, System.Drawing.Color CellColor)
        {
            Excel.Application exc = null;
            try
            {
                exc = new Excel.Application();
                Excel.Workbooks workbooks = exc.Workbooks;
                Excel._Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                _Worksheet worksheet = (_Worksheet)workbook.Worksheets[1];
                Range range;
                Object[] data;
                int i, j;
                for (j = 0; j < dt.Columns.Count; j++)
                {
                    range = worksheet.get_Range(Convert.ToChar(65 + j) + 1.ToString(), System.Reflection.Missing.Value);
                    data = new Object[] { dt.Columns[j].Caption };
                    if (IsHeaderBold == true)
                    {
                        range.Font.Bold = true;
                    }
                    if (IsHeaderLocked == true)
                    {
                        range.Select();
                        range.Locked = true;
                    }
                    if (!HeaderColor.IsEmpty)
                    {
                        range.Select();
                        range.Interior.Color = HeaderColor.ToArgb();
                    }
                    if (IsBorder == true)
                    {
                        range.Borders.Color = Color.Black.ToArgb();
                    }
                    range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    for (j = 0; j < dt.Columns.Count; j++)
                    {
                        range = worksheet.get_Range(Convert.ToChar(65 + j) + Convert.ToString(2 + i), System.Reflection.Missing.Value);
                        data = new Object[] { dt.Rows[i][j] };
                        if (!HeaderColor.IsEmpty)
                        {
                            range.Select();
                            range.Interior.Color = CellColor.ToArgb();
                        }
                        if (IsCellBold == true)
                        {
                            range.Font.Bold = true;
                        }
                        if (IsBorder == true)
                        {
                            range.Borders.Color = Color.Black.ToArgb();
                        }
                        if (IsCellLocked == true)
                        {
                            range.Select();
                            range.Locked = true;
                        }
                        else
                        {
                            range.Select();
                            range.Locked = false;
                        }
                        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                    }
                }
                range = worksheet.get_Range("A1", "B1");
                range.Select();
                worksheet.Columns.AutoFit();
                if (IsCellLocked == true || IsHeaderLocked == true)
                {
                    worksheet.Protect(ExcelPassword,
                        worksheet.ProtectDrawingObjects,
                        true, worksheet.ProtectScenarios,
                        worksheet.ProtectionMode,
                        true,
                        true,
                        true,
                         worksheet.Protection.AllowInsertingColumns,
                        worksheet.Protection.AllowInsertingRows,
                        worksheet.Protection.AllowInsertingHyperlinks,
                        worksheet.Protection.AllowDeletingColumns,
                        worksheet.Protection.AllowDeletingRows,
                        worksheet.Protection.AllowSorting,
                        true,
                        worksheet.Protection.AllowUsingPivotTables);
                }
                workbook.Close(true, xlsFilePath, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (exc != null)
                    exc.Quit();
            }

        }


        /// <summary>
        /// To Write an Excel File.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="xlsFilePath"></param>
        public void WriteInExcel(System.Data.DataTable dt, string xlsFilePath)
        {
            Excel.Application exc = null;
            try
            {
                exc = new Excel.Application();
                Excel.Workbooks workbooks = exc.Workbooks;
                Excel._Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                _Worksheet worksheet = (_Worksheet)workbook.Worksheets[1];
                Range range;
                Object[] data;
                int i, j;
                for (j = 0; j < dt.Columns.Count; j++)
                {
                    range = worksheet.get_Range(Convert.ToChar(65 + j) + 1.ToString(), System.Reflection.Missing.Value);
                    data = new Object[] { dt.Columns[j].Caption };
                    if (IsHeaderBold == true)
                    {
                        range.Font.Bold = true;
                    }
                    if (IsHeaderLocked == true)
                    {
                        range.Select();
                        range.Locked = true;
                    }
                    if (!HeaderColor.IsEmpty)
                    {
                        range.Select();
                        range.Interior.Color = HeaderColor.ToArgb();
                    }
                    if (IsBorder == true)
                    {
                        range.Borders.Color = Color.Black.ToArgb();
                    }
                    range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    for (j = 0; j < dt.Columns.Count; j++)
                    {
                        range = worksheet.get_Range(Convert.ToChar(65 + j) + Convert.ToString(2 + i), System.Reflection.Missing.Value);
                        data = new Object[] { dt.Rows[i][j] };
                        if (!HeaderColor.IsEmpty)
                        {
                            range.Select();
                            range.Interior.Color = CellColor.ToArgb();
                        }
                        if (IsCellBold == true)
                        {
                            range.Font.Bold = true;
                        }
                        if (IsBorder == true)
                        {
                            range.Borders.Color = Color.Black.ToArgb();
                        }
                        if (IsCellLocked == true)
                        {
                            range.Select();
                            range.Locked = true;
                        }
                        else
                        {
                            range.Select();
                            range.Locked = false;
                        }
                        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                    }
                }
                range = worksheet.get_Range("A1", "B1");
                range.Select();
                worksheet.Columns.AutoFit();
                if (IsCellLocked == true || IsHeaderLocked == true)
                {
                    worksheet.Protect(ExcelPassword,
                        worksheet.ProtectDrawingObjects,
                        true, worksheet.ProtectScenarios,
                        worksheet.ProtectionMode,
                        true,
                        true,
                        true,
                         worksheet.Protection.AllowInsertingColumns,
                        worksheet.Protection.AllowInsertingRows,
                        worksheet.Protection.AllowInsertingHyperlinks,
                        worksheet.Protection.AllowDeletingColumns,
                        worksheet.Protection.AllowDeletingRows,
                        worksheet.Protection.AllowSorting,
                        true,
                        worksheet.Protection.AllowUsingPivotTables);
                }
                workbook.Close(true, xlsFilePath, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (exc != null)
                    exc.Quit();
            }
        }


        /// <summary>
        /// To Write an Excel File.
        /// </summary>
        public void WriteInExcel()
        {
            Excel.Application exc = null;
            try
            {
                exc = new Excel.Application();
                Excel.Workbooks workbooks = exc.Workbooks;
                Excel._Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                _Worksheet worksheet = (_Worksheet)workbook.Worksheets[1];
                Range range;
                Object[] data;
                int i, j;
                for (j = 0; j < DataTableToWrite.Columns.Count; j++)
                {
                    range = worksheet.get_Range(Convert.ToChar(65 + j) + 1.ToString(), System.Reflection.Missing.Value);
                    data = new Object[] { DataTableToWrite.Columns[j].Caption };
                    if (IsHeaderBold == true)
                    {
                        range.Font.Bold = true;
                    }
                    if (IsHeaderLocked == true)
                    {
                        range.Select();
                        range.Locked = true;
                    }
                    if (!HeaderColor.IsEmpty)
                    {
                        range.Select();
                        range.Interior.Color = HeaderColor.ToArgb();
                    }
                    if (IsBorder == true)
                    {
                        range.Borders.Color = Color.Black.ToArgb();
                    }
                    range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                }
                for (i = 0; i < DataTableToWrite.Rows.Count; i++)
                {
                    for (j = 0; j < DataTableToWrite.Columns.Count; j++)
                    {
                        range = worksheet.get_Range(Convert.ToChar(65 + j) + Convert.ToString(2 + i), System.Reflection.Missing.Value);
                        data = new Object[] { DataTableToWrite.Rows[i][j] };
                        if (!HeaderColor.IsEmpty)
                        {
                            range.Select();
                            range.Interior.Color = CellColor.ToArgb();
                        }
                        if (IsCellBold == true)
                        {
                            range.Font.Bold = true;
                        }
                        if (IsBorder == true)
                        {
                            range.Borders.Color = Color.Black.ToArgb();
                        }
                        if (IsCellLocked == true)
                        {
                            range.Select();
                            range.Locked = true;
                        }
                        else
                        {
                            range.Select();
                            range.Locked = false;
                        }
                        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                    }
                }
                range = worksheet.get_Range("A1", "B1");
                range.Select();
                worksheet.Columns.AutoFit();
                if (IsCellLocked == true || IsHeaderLocked == true)
                {
                    worksheet.Protect(ExcelPassword,
                        worksheet.ProtectDrawingObjects,
                        true, worksheet.ProtectScenarios,
                        worksheet.ProtectionMode,
                        true,
                        true,
                        true,
                         worksheet.Protection.AllowInsertingColumns,
                        worksheet.Protection.AllowInsertingRows,
                        worksheet.Protection.AllowInsertingHyperlinks,
                        worksheet.Protection.AllowDeletingColumns,
                        worksheet.Protection.AllowDeletingRows,
                        worksheet.Protection.AllowSorting,
                        true,
                        worksheet.Protection.AllowUsingPivotTables);
                }
                workbook.Close(true, Path, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (exc != null)
                    exc.Quit();
            }
        }


        /// <summary>
        /// To Read an Excel File.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public System.Data.DataTable ReadExcel(string FilePath)
        {
            try
            {
                string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Extended Properties=Excel 12.0; Data Source=" + FilePath;
                OleDbConnection connection = new OleDbConnection(excelConnectionString);
                Console.WriteLine("Connecting to Excel File.");
                OleDbCommand command = new OleDbCommand("Select * FROM [Sheet1$]", connection);
                OleDbCommand count = new OleDbCommand("Select count(*) FROM [Sheet1$]", connection);
                DataSet dataset = new DataSet();
                OleDbConnection conn = new OleDbConnection(excelConnectionString);
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = new OleDbCommand("Select * from [Sheet1$]", connection);
                adapter.Fill(dataset);
                System.Data.DataTable dt = dataset.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// To Read an Excel File.
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable ReadExcel()
        {
            try
            {
                string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Extended Properties=Excel 12.0; Data Source=" + Path;
                OleDbConnection connection = new OleDbConnection(excelConnectionString);
                Console.WriteLine("Connecting to Excel File.");
                OleDbCommand command = new OleDbCommand("Select * FROM [Sheet1$]", connection);
                OleDbCommand count = new OleDbCommand("Select count(*) FROM [Sheet1$]", connection);
                DataSet dataset = new DataSet();
                OleDbConnection conn = new OleDbConnection(excelConnectionString);
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = new OleDbCommand("Select * from [Sheet1$]", connection);
                adapter.Fill(dataset);
                System.Data.DataTable dt = dataset.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void WriteInExcelWithFormat(System.Data.DataTable dt, string xlsFilePath, bool IsProtected, bool IsHeaderLocked, bool IsCellLocked, bool IsCellBold, bool IsHeaderBold, bool IsBorder, string ExcelPassword, System.Drawing.Color HeaderColor, System.Drawing.Color CellColor,string ReportName,string UserID)
        {
            Excel.Application exc = null;
            try
            {
                exc = new Excel.Application();
                Excel.Workbooks workbooks = exc.Workbooks;
                Excel._Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                _Worksheet worksheet = (_Worksheet)workbook.Worksheets[1];
                Range range;
                Object[] data;

                range = worksheet.get_Range(Convert.ToChar(65 + 0) + Convert.ToString(1 + 0), System.Reflection.Missing.Value);
                data = new Object[] { "MAKE MY TRIP" };
                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                range.WrapText = false;
                range.Borders.Color = Color.Black.ToArgb();
                range.VerticalAlignment = XlVAlign.xlVAlignTop;
                range.Select();
                //------------------------------------------------------------------------------------------------------------------
                range = worksheet.get_Range(Convert.ToChar(65 + 0) + Convert.ToString(2 + 0), System.Reflection.Missing.Value);
                data = new Object[] { "REPORT NAME" };
                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                range.VerticalAlignment = XlVAlign.xlVAlignTop;
                range.WrapText = false;
                range.Borders.Color = Color.Black.ToArgb();
              
                range = worksheet.get_Range(Convert.ToChar(65 + 1) + Convert.ToString(2 + 0), System.Reflection.Missing.Value);            
                data = new Object[] { ReportName };
                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                range.VerticalAlignment = XlVAlign.xlVAlignTop;
                range.WrapText = false;
                range.Borders.Color = Color.Black.ToArgb();
                //------------------------------------------------------------------------------------------------------------------

                range = worksheet.get_Range(Convert.ToChar(65 + 0) + Convert.ToString(3 + 0), System.Reflection.Missing.Value);
                data = new Object[] { "GENERATED ON" };
                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                range.Borders.Color = Color.Black.ToArgb();
                range.WrapText = false;
                range.VerticalAlignment = XlVAlign.xlVAlignTop;

                string strDate = DateTime.Today.ToString("dd/MM/yyyy");
                range = worksheet.get_Range(Convert.ToChar(65 + 1) + Convert.ToString(3 + 0), System.Reflection.Missing.Value);
                data = new Object[] { "'" + strDate };
                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                range.WrapText = false;
                range.Borders.Color = Color.Black.ToArgb();
                range.VerticalAlignment = XlVAlign.xlVAlignTop;
                //------------------------------------------------------------------------------------------------------------------

                range = worksheet.get_Range(Convert.ToChar(65 + 0) + Convert.ToString(4 + 0), System.Reflection.Missing.Value);
                data = new Object[] { "GENERATED BY" };
                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                range.Borders.Color = Color.Black.ToArgb();
                range.WrapText = false;
                range.VerticalAlignment = XlVAlign.xlVAlignTop;


                range = worksheet.get_Range(Convert.ToChar(65 + 1) + Convert.ToString(4 + 0), System.Reflection.Missing.Value);
                data = new Object[] { UserID };
                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                range.VerticalAlignment = XlVAlign.xlVAlignTop;
                range.WrapText = false;
                range.Borders.Color = Color.Black.ToArgb();


                //------------------------------------------------------------------------------------------------------------------

              
          




                int i, j;
                for (j = 0; j < dt.Columns.Count; j++)
                {
                    range = worksheet.get_Range(Convert.ToChar(65 + j) + 6.ToString(), System.Reflection.Missing.Value);
                    data = new Object[] { dt.Columns[j].Caption };
                    if (IsHeaderBold == true)
                    {
                        range.Font.Bold = true;
                    }
                    if (IsHeaderLocked == true)
                    {
                        range.Select();
                        range.Locked = true;
                    }
                    if (!HeaderColor.IsEmpty)
                    {
                        range.Select();
                        range.Interior.Color = HeaderColor.ToArgb();
                    }
                    if (IsBorder == true)
                    {
                        range.Borders.Color = Color.Black.ToArgb();
                    }
                    range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    for (j = 0; j < dt.Columns.Count; j++)
                    {
                        range = worksheet.get_Range(Convert.ToChar(65 + j) + Convert.ToString(7 + i), System.Reflection.Missing.Value);
                        data = new Object[] { dt.Rows[i][j] };
                        if (!HeaderColor.IsEmpty)
                        {
                            range.Select();
                            range.Interior.Color = CellColor.ToArgb();
                        }
                        if (IsCellBold == true)
                        {
                            range.Font.Bold = true;
                        }
                        if (IsBorder == true)
                        {
                            range.Borders.Color = Color.Black.ToArgb();
                        }
                        if (IsCellLocked == true)
                        {
                            range.Select();
                            range.Locked = true;
                        }
                        else
                        {
                            range.Select();
                            range.Locked = false;
                        }
                        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
                    }
                }
                range = worksheet.get_Range("A1", "B1");
                range.Select();
                worksheet.Columns.AutoFit();
                if (IsCellLocked == true || IsHeaderLocked == true)
                {
                    worksheet.Protect(ExcelPassword,
                        worksheet.ProtectDrawingObjects,
                        true, worksheet.ProtectScenarios,
                        worksheet.ProtectionMode,
                        true,
                        true,
                        true,
                         worksheet.Protection.AllowInsertingColumns,
                        worksheet.Protection.AllowInsertingRows,
                        worksheet.Protection.AllowInsertingHyperlinks,
                        worksheet.Protection.AllowDeletingColumns,
                        worksheet.Protection.AllowDeletingRows,
                        worksheet.Protection.AllowSorting,
                        true,
                        worksheet.Protection.AllowUsingPivotTables);
                }
                workbook.Close(true, xlsFilePath, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (exc != null)
                    exc.Quit();
            }

        }


    }
}