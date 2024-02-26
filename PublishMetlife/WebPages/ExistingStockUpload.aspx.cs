using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using MobiVUE_ATS.MyDLL;

public partial class ExistingStockUpload : System.Web.UI.Page
{

    #region PRIVATE VARIABLES
    int iCnt = 0;
    AssetAcquisition_DAL oDAL;
    AssetAcquisition_PRP oPRP;
    DataTable dtFileData;
    string strFilePath = "";
    string strDupFilePath = @"C:\ATS\";
    #endregion

    public ExistingStockUpload()
    {
        oPRP = new AssetAcquisition_PRP();
    }
    ~ExistingStockUpload()
    {
        oPRP = null;
        oDAL = null;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new AssetAcquisition_DAL(Session["DATABASE"].ToString());
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("ASSET_ACQUISITION", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_ACQUISITION");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                this.Page.Form.Attributes.Add("enctype", "multipart/form-data");
               
                if (!Directory.Exists(strDupFilePath))
                {
                    Directory.CreateDirectory(strDupFilePath);
                }
                if (!Directory.Exists(Request.PhysicalApplicationPath + "UploadedFiles"))
                {
                    Directory.CreateDirectory(Request.PhysicalApplicationPath + "UploadedFiles");
                }
                
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }

    }

    protected void btnUpload_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
         
                Session["ASSET"] = null;
                UploadAssetFile();
            
           
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }


    private void UploadAssetFile()
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
                MyExcel oExcel = new MyExcel();
                dtFileData = oExcel.ReadExcel(strFilePath);
                Session["ASSET"] = dtFileData;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Enter Submit to save/update file data.');", true);
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

    #region ERROR LOGGING
    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Import/Export Data");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Response.Redirect("Error.aspx");
        }
    }
    #endregion
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
           System.Threading.Thread.Sleep(5000);
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }

            if (Session["ASSET"] != null)
            {
                if (rdoITAsset.Checked)
                {
                    new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Upload IT ASSET", "Asset Acquisition", "IT Asset upload by user id" + Session["CURRENTUSER"].ToString());
                    SaveAssetDetails();
                   ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Fresh asset data (except duplicate serial no./IMEI no.) is saved successfully.');", true);
                    string FileName = strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt";
                    clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "BULK_ASSETS_UPLOADED");
                }
                else if (rdoFacilityAsset.Checked)
                {
                    new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Upload Facility ASSET", "Asset Acquisition", "Facility Asset upload by user id" + Session["CURRENTUSER"].ToString());
                    SaveFacilityAssetDetails();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Fresh asset data (except duplicate serial no./IMEI no.) is saved successfully.');", true);
                    string FileName = strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt";
                    clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "BULK_ASSETS_UPLOADED");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : kindly upload file first.');", true);
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    private void SaveAssetDetails()
    {
        try
        {
            int iRslt = 0;
            bool bInValid = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            dtFileData = (DataTable)Session["ASSET"];

            if (dtFileData.Columns.Count != 35)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : invalid file format.');", true);
                bInValid = true;
                return;
            }
            oDAL.BeginTransaction();
            for (iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
                Session["ASSET"] = null;
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }
                if (dtFileData.Rows[iCnt][1].ToString().Trim() != "")
                    oPRP.AssetLocation = dtFileData.Rows[iCnt][1].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : location is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                oPRP.AssetLife = dtFileData.Rows[iCnt][2].ToString().Replace("'", "`").Trim();
                if (dtFileData.Rows[iCnt][3].ToString().Trim() != "")
                    oPRP.AssetType = dtFileData.Rows[iCnt][3].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset type is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                if (dtFileData.Rows[iCnt][4].ToString().Trim() != "")
                {

                    oPRP.AssetMakeName = dtFileData.Rows[iCnt][4].ToString().Replace("'", "`").Trim();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Make is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetModelName = dtFileData.Rows[iCnt][5].ToString().ToUpper().Replace("'", "`").Trim();
                if (oPRP.AssetModelName == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  modal not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetCode = dtFileData.Rows[iCnt][6].ToString().ToUpper().Replace("'", "`").Trim();
                if (oPRP.AssetCode == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  serial no not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.HostName = dtFileData.Rows[iCnt][7].ToString().ToUpper().Replace("'", "`").Trim();
             
                oPRP.AssetID = dtFileData.Rows[iCnt][8].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.AssetTag = dtFileData.Rows[iCnt][9].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.Status = dtFileData.Rows[iCnt][10].ToString().ToUpper().Replace("'", "`").Trim();
                if (oPRP.Status == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : asset status not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.SubStatus = dtFileData.Rows[iCnt][11].ToString().ToUpper().Replace("'", "`").Trim();
                if (oPRP.SubStatus == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : asset sub status not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                oPRP.store = dtFileData.Rows[iCnt][12].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.SERVICE_NOW_TICKET_NO = dtFileData.Rows[iCnt][13].ToString().ToUpper().Replace("'", "`").Trim();
                if (dtFileData.Rows[iCnt][14].ToString().Trim() != "" && dtFileData.Rows[iCnt][14].ToString().Trim() != "NA")
                    oPRP.AllocationDate = Convert.ToDateTime(dtFileData.Rows[iCnt][14].ToString().Trim()).ToString("dd-MMM-yyyy");
                if(dtFileData.Rows[iCnt][15].ToString().Trim()!="" && dtFileData.Rows[iCnt][15].ToString().Trim()!="NA")
                oPRP.ReturnDate = Convert.ToDateTime(dtFileData.Rows[iCnt][15].ToString().Trim()).ToString("dd-MMM-yyyy");
                oPRP.EmpId = dtFileData.Rows[iCnt][16].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.EmpName = dtFileData.Rows[iCnt][17].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.EmpFloor = dtFileData.Rows[iCnt][18].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.SeatNo = dtFileData.Rows[iCnt][19].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.Designation = dtFileData.Rows[iCnt][20].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.Process = dtFileData.Rows[iCnt][21].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.SubLOB = dtFileData.Rows[iCnt][22].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.LOB = dtFileData.Rows[iCnt][23].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.AssetProcessor = dtFileData.Rows[iCnt][24].ToString().ToUpper().Replace("'", "`").Trim(); 
                if (oPRP.AssetProcessor == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Processeor not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetRAM = dtFileData.Rows[iCnt][25].ToString().ToUpper().Replace("'", "`").Trim(); 
                if (oPRP.AssetRAM == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Ram not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetHDD = dtFileData.Rows[iCnt][26].ToString().Trim().ToUpper();
                if (oPRP.AssetHDD == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Hard disk not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AMC_Warranty = dtFileData.Rows[iCnt][27].ToString().ToUpper().Replace("'", "`").Trim(); 
                if (oPRP.AMC_Warranty == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Warranty status not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                oPRP.GRNNo = dtFileData.Rows[iCnt][28].ToString().ToUpper().Replace("'", "`").Trim(); 
                if (oPRP.GRNNo == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Grn no not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                oPRP.VendorCode = dtFileData.Rows[iCnt][29].ToString().Replace("'", "`").Trim(); 
                if (oPRP.VendorCode == "")
                {
                    //if (!oDAL.ValidVendorCode(oPRP.VendorCode, Session["COMPANY"].ToString()))
                    //{
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor  is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    //}
                }
                oPRP.PurchaseOrderNo = dtFileData.Rows[iCnt][30].ToString().Trim().ToUpper();
                if (oPRP.PurchaseOrderNo == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Po no not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.PODate = Convert.ToDateTime( dtFileData.Rows[iCnt][31].ToString().Trim()).ToString("dd-MMM-yyyy");
                if (oPRP.PODate == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Po date not  found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.InvoiceNo = dtFileData.Rows[iCnt][32].ToString().Trim().ToUpper();
                if (oPRP.InvoiceNo == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : invoice no  not  found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                oPRP.InvoiceDate = Convert.ToDateTime(dtFileData.Rows[iCnt][33].ToString().Trim()).ToString("dd-MMM-yyyy");
                if (oPRP.InvoiceDate == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : invoice date  not  found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.Remarks = dtFileData.Rows[iCnt][34].ToString().Trim().ToUpper();
                oPRP.AssetQty = "1";
                //if (oPRP.AssetQty == "")
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :quantity  not  found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                oPRP.CompCode = Session["COMPANY"].ToString();
               // if (oPRP.CompCode == oPRP.AssetLocation)
               // {
                    //int _iMaxACQId = oDAL.GetMaxAcquisitionId(oPRP.AssetCategoryCode, oPRP.CompCode);
                    //oPRP.RunningSerialNo = _iMaxACQId;
                    //string s = oPRP.AssetCategoryCode.ToString().Replace("-00", "");
                    //string str = s.Substring(s.Length - 2, 2);
                    //oPRP.AssetCode = oPRP.CompCode + "-" + ddlAssetType.SelectedValue.ToString() + "-" + str + "-" + _iMaxACQId.ToString().PadLeft(6, '0');

                    if (!oDAL.CheckDuplicateSerialNo(oPRP.AssetCode, oPRP.CompCode) )
                    {
                        iRslt = oDAL.SaveAssetAcquisitionDetails(oPRP);
                        if (iRslt == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details not saved at row no. " + (iCnt + 1).ToString() + ".');", true);
                            bInValid = true;
                            break;
                        }
                    }
                    else
                    {
                        StreamWriter sw;

                      

                            if (File.Exists(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt"))
                            {
                                using (sw = File.AppendText(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt"))
                                    sw.WriteLine(oPRP.AssetCode, oPRP.AssetTag);
                                sw.Close();
                                sw = null;
                            }
                            else
                            {
                                using (sw = new StreamWriter(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt"))
                                    sw.WriteLine(oPRP.AssetCode, oPRP.AssetTag);
                                sw.Close();
                                sw = null;
                            }
                    }
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : You cannot upload assets at another location.');", true);
                //    bInValid = true;
                //    break;
                //}
            }
            oDAL.CommitTransaction();
            if (!bInValid)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Fresh asset data (except duplicate serial no./IMEI no.) is saved successfully.');", true);
        }
        catch (Exception ex)
        {
            oDAL.RollBackTransaction();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : An error occured at row no. " + (iCnt + 1).ToString() + " while uploading asset details, check file data.');", true);
            HandleExceptions(ex);
        }
        finally
        { dtFileData = null; }
    }

    private void SaveFacilityAssetDetails()
    {
        try
        {
            int iRslt = 0;
            bool bInValid = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            dtFileData = (DataTable)Session["ASSET"];
            if (dtFileData.Columns.Count != 16)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : invalid file format.');", true);
                bInValid = true;
               return;
            }
            oDAL.BeginTransaction();
            for (iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }
                if (dtFileData.Rows[iCnt][0].ToString().Trim() != "")
                    oPRP.AssetLocation = dtFileData.Rows[iCnt][0].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : location is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.floor = dtFileData.Rows[iCnt][1].ToString().Replace("'", "`").Trim();
                oPRP.AssetLife = dtFileData.Rows[iCnt][2].ToString().Replace("'", "`").Trim();
                if (dtFileData.Rows[iCnt][3].ToString().Trim() != "")
                    oPRP.AssetType = dtFileData.Rows[iCnt][3].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset type is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                oPRP.AssetCategoryCode = dtFileData.Rows[iCnt][4].ToString().Replace("'", "`").Trim();
                if (oPRP.AssetCategoryCode == "")
                {
                    //if (!oDAL.ValidVendorCode(oPRP.VendorCode, Session["COMPANY"].ToString()))
                    //{
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : asset sub category  is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                    //}
                }
                oPRP.VendorCode = dtFileData.Rows[iCnt][5].ToString().Replace("'", "`").Trim();
                if (oPRP.VendorCode == "")
                {
                    //if (!oDAL.ValidVendorCode(oPRP.VendorCode, Session["COMPANY"].ToString()))
                    //{
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor  is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                    //}
                }

                if (dtFileData.Rows[iCnt][6].ToString().Trim() != "")
                {

                    oPRP.AssetMakeName = dtFileData.Rows[iCnt][6].ToString().Replace("'", "`").Trim();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Make is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetModelName = dtFileData.Rows[iCnt][7].ToString().ToUpper().Replace("'", "`").Trim();


                if (oPRP.AssetModelName == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  modal not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
               
               // oPRP.HostName = dtFileData.Rows[iCnt][7].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.AssetTag = dtFileData.Rows[iCnt][8].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.Asset_FAR_TAG = dtFileData.Rows[iCnt][9].ToString().ToUpper().Replace("'", "`").Trim();
                if (oPRP.Asset_FAR_TAG == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Asset Far Tag not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetCode= dtFileData.Rows[iCnt][9].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.SubStatus = dtFileData.Rows[iCnt][10].ToString().ToUpper().Replace("'", "`").Trim();
                if (oPRP.SubStatus == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : asset working status not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                //oPRP.SubStatus = dtFileData.Rows[iCnt][11].ToString().ToUpper().Replace("'", "`").Trim();
                //if (oPRP.Status == "")
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : asset sub status not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                oPRP.store = dtFileData.Rows[iCnt][11].ToString().ToUpper().Replace("'", "`").Trim();
                oPRP.IdentifierLocation = dtFileData.Rows[iCnt][12].ToString().ToUpper().Replace("'", "`").Trim();
               
              
                oPRP.AMC_Warranty = dtFileData.Rows[iCnt][13].ToString().ToUpper().Replace("'", "`").Trim();
                if (oPRP.AMC_Warranty == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Warranty status not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                //oPRP.GRNNo = dtFileData.Rows[iCnt][14].ToString().ToUpper().Replace("'", "`").Trim();
                //if (oPRP.GRNNo == "")
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Grn no not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

               
                oPRP.PurchaseOrderNo = dtFileData.Rows[iCnt][14].ToString().Trim().ToUpper();
                if (oPRP.PurchaseOrderNo == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Po no not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                //oPRP.PODate = dtFileData.Rows[iCnt][22].ToString().Trim().ToUpper();
                //if (oPRP.PODate == "")
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Po date not  found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}
                if (dtFileData.Rows[iCnt][15].ToString().Trim() != "" && dtFileData.Rows[iCnt][15].ToString().Trim()!="NA")
                    oPRP.AssetInstallDate = Convert.ToDateTime(dtFileData.Rows[iCnt][15].ToString().Trim()).ToString("dd-MMM-yyyy");
                
                //if (oPRP.AssetInstallDate == "")
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Po date not  found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                if (oPRP.floor == "" && oPRP.store == "")
                    oPRP.Status = "STOCK";
                else
                    oPRP.Status = "ALLOCATED";
                //oPRP.InvoiceNo = dtFileData.Rows[iCnt][24].ToString().Trim().ToUpper();
                //if (oPRP.InvoiceNo == "")
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : invoice no  not  found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                //oPRP.InvoiceDate = dtFileData.Rows[iCnt][25].ToString().Trim().ToUpper();
                //if (oPRP.InvoiceDate == "")
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : invoice date  not  found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}
                //oPRP.Asset_FAR_TAG = dtFileData.Rows[iCnt][27].ToString().Trim().ToUpper();
                //oPRP.Remarks = dtFileData.Rows[iCnt][28].ToString().Trim().ToUpper();

                //oPRP.AssetQty = "1";

                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                oPRP.CompCode = Session["COMPANY"].ToString();
              //  if (oPRP.CompCode == oPRP.AssetLocation)
               // {
                    if (!oDAL.CheckDuplicateSerialNo(oPRP.AssetCode, oPRP.CompCode))
                    {
                        iRslt = oDAL.SaveAssetAcquisitionFacilityDetails(oPRP);
                        if (iRslt == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details not saved at row no. " + (iCnt + 1).ToString() + ".');", true);
                            bInValid = true;
                            break;
                        }
                    }
                    else
                    {
                        StreamWriter sw;
                        if (File.Exists(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt"))
                        {
                            sw = File.AppendText(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt");
                            sw.WriteLine(oPRP.AssetCode, oPRP.AssetTag);
                        }
                        else
                        {
                            sw = new StreamWriter(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt");
                            sw.WriteLine(oPRP.AssetCode, oPRP.AssetTag);
                        }
                        sw.Close();
                        sw = null;
                    }
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : You cannot upload assets at another location.');", true);
                //    bInValid = true;
                //    break;
                //}
            }
            oDAL.CommitTransaction();
            if (!bInValid)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note :  asset data (except duplicate serial no.) is saved successfully.');", true);
        }
        catch (Exception ex)
        {
            oDAL.RollBackTransaction();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : An error occured at row no. " + (iCnt + 1).ToString() + " while uploading asset details, check file data.');", true);
            HandleExceptions(ex);
        }
        finally
        { dtFileData = null; }
    }
}