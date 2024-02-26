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

public partial class EmployeeTagPersonalization : System.Web.UI.Page
{

    #region PRIVATE VARIABLES
    int iCnt = 0;
    TagMaster_DAL oDAL;
    TagMaster_PRP oPRP;
    DataTable dtFileData;
    string strFilePath = "";
    string strDupFilePath = @"C:\ATS\";
    #endregion

     public EmployeeTagPersonalization()
    {
        oPRP = new TagMaster_PRP();
    }
     ~EmployeeTagPersonalization()
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
         oDAL = new TagMaster_DAL(Session["DATABASE"].ToString());
     }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string _strRights = clsGeneral.GetRights("IMPORT_ASSET", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "IMPORT_EXPORT_DATA");
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                this.Page.Form.Attributes.Add("enctype", "multipart/form-data");
                ////   ddlImportType.Attributes.Add("onChange", "onselectionChange();");
                //    ddlImportType.Focus();
                //if (Session["GROUP"].ToString().ToUpper() == "SYSADMIN")
                //    rdoUpdateSerialNo.Enabled = true;
                //else
                //    rdoUpdateSerialNo.Enabled = false;
                if (!Directory.Exists(strDupFilePath))
                {
                    Directory.CreateDirectory(strDupFilePath);
                }
                if (!Directory.Exists(Request.PhysicalApplicationPath + "UploadedFiles"))
                {
                    Directory.CreateDirectory(Request.PhysicalApplicationPath + "UploadedFiles");
                }
                //lblAssetType.Text = clsGeneral.gStrAssetType;
                //ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
                //ddlAssetType2.SelectedValue = clsGeneral.gStrAssetType;
                //PopulateCategory(clsGeneral.gStrAssetType);
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



            {
                Session["VENDOR"] = null;
                UploadVendorFile();
            }

        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    private void UploadVendorFile()
    {
        string strFileName = VendorFileUpload.FileName;
        if (strFileName != "")
        {
            String fileExt = System.IO.Path.GetExtension(VendorFileUpload.FileName);
            if (fileExt.ToUpper() != ".XLSX" && fileExt.ToUpper() != ".XLS")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select an excel file (.xls/.xlsx) only.');", true);
                return;
            }
            if (UploadFile())
            {
                MyExcel oExcel = new MyExcel();
                dtFileData = oExcel.ReadExcel(strFilePath);
                Session["VENDOR"] = dtFileData;
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


        //  if (ddlImportType.SelectedValue.ToString() == "VENDOR")
        {
            fileExt = System.IO.Path.GetExtension(VendorFileUpload.FileName);
            if (VendorFileUpload.HasFile)
            {
                if (fileExt == ".xls" || fileExt == ".XLS" || fileExt == ".xlsx")
                {
                    strFilePath = Request.PhysicalApplicationPath + "UploadedFiles\\" + VendorFileUpload.FileName;
                    File.Delete(strFilePath);
                    VendorFileUpload.SaveAs(strFilePath);
                    bRes = true;
                }
            }
        }

        return bRes;
    }
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Import/Export Data");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Response.Redirect("Error.aspx");
        }
    }
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



            SaveVendorDetails();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note :  Data is saved successfully.');", true);


        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    private void SaveVendorDetails()
    {
        TagMaster_DAL oDAL = new TagMaster_DAL(Session["DATABASE"].ToString());
        TagMaster_PRP oPRP = new TagMaster_PRP();
        try
        {
            bool bInValid = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            dtFileData = (DataTable)Session["VENDOR"];
            for (iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
                Session["VENDOR"] = null;
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }
                if (dtFileData.Rows[iCnt][0].ToString().Trim() != "")
                    oPRP.AssetCode = dtFileData.Rows[iCnt][0].ToString().Trim().ToUpper();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Employee Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][1].ToString().Trim() != "")
                    oPRP.TagSerialNo = dtFileData.Rows[iCnt][1].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlertShowAlert('Please Note :TagSerialNo is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.EmpName = dtFileData.Rows[iCnt][2].ToString().Replace("'", "`").Trim();
                oPRP.EmailID = dtFileData.Rows[iCnt][3].ToString().Replace("'", "`").Trim();
                oPRP.Designation = dtFileData.Rows[iCnt][4].ToString().Replace("'", "`").Trim();
                oPRP.SeatNo = dtFileData.Rows[iCnt][5].ToString().Replace("'", "`").Trim();
                oPRP.ProcessName = dtFileData.Rows[iCnt][6].ToString().Replace("'", "`").Trim();
                oPRP.LOB = dtFileData.Rows[iCnt][7].ToString().Replace("'", "`").Trim();
                oPRP.SubLOB = dtFileData.Rows[iCnt][8].ToString().Replace("'", "`").Trim();



                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.Active = true;
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                oDAL.UploadEmployeeTagDetails(oPRP);
            }
            if (!bInValid)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Vendor data is saved successfully.');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : An error has occured while uploading vendor details, check file data.');", true);
            HandleExceptions(ex);
        }
        finally
        { oPRP = null; oDAL = null; }
    }
}