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

public partial class ImportAssetDetails : System.Web.UI.Page
{
    #region PRIVATE VARIABLES
    int iCnt = 0;
    AssetAcquisition_DAL oDAL;
    AssetAcquisition_PRP oPRP;
    DataTable dtFileData;
    string strFilePath = "";
    string strDupFilePath = @"C:\ATS\";
    #endregion

    public ImportAssetDetails()
    {
        oPRP = new AssetAcquisition_PRP();
    }
    ~ImportAssetDetails()
    {
        oPRP = null;
        oDAL = null;
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
        oDAL = new AssetAcquisition_DAL(Session["DATABASE"].ToString());
    }

    /// <summary>
    /// Check user permissions in order to import asset details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                ddlImportType.Attributes.Add("onChange", "onselectionChange();");
                ddlImportType.Focus();
                if (Session["GROUP"].ToString().ToUpper() == "SYSADMIN")
                    rdoUpdateSerialNo.Enabled = true;
                else
                    rdoUpdateSerialNo.Enabled = false;
                if (!Directory.Exists(strDupFilePath))
                {
                    Directory.CreateDirectory(strDupFilePath);
                }
                if (!Directory.Exists(Request.PhysicalApplicationPath + "UploadedFiles"))
                {
                    Directory.CreateDirectory(Request.PhysicalApplicationPath + "UploadedFiles");
                }
                lblAssetType.Text = clsGeneral.gStrAssetType;
                ddlAssetType.SelectedValue = clsGeneral.gStrAssetType;
                ddlAssetType2.SelectedValue = clsGeneral.gStrAssetType;
                PopulateCategory(clsGeneral.gStrAssetType);
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// Upload excel file from a location and populate datatable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (ddlImportType.SelectedValue.ToString() == "ASSET")
            {
                Session["ASSET"] = null;
                UploadAssetFile();
            }
            else if (ddlImportType.SelectedValue.ToString() == "ALLOCATE")
            {
                Session["ALLOCATE"] = null;
                UploadAllocationFile();
            }
            else if (ddlImportType.SelectedValue.ToString() == "VENDOR")
            {
                Session["VENDOR"] = null;
                UploadVendorFile();
            }
            else if (ddlImportType.SelectedValue.ToString() == "EMPLOYEE")
            {
                Session["EMPLOYEE"] = null;
                UploadEmployeeFile();
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Save uploaded file data into respective data table as per type selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
            if (ddlImportType.SelectedValue.ToString() == "ASSET")
            {
                if (rdoUploadAsset.Checked)
                {
                    SaveAssetDetails();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Fresh asset data (except duplicate serial no./IMEI no.) is saved successfully.');", true);
                    string FileName = strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt";
                    clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "BULK_ASSETS_UPLOADED");
                }
                else if (rdoUpdateAsset.Checked)
                {
                    UpdateAssetDetails();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset data is updated successfully.');", true);
                    clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "BULK_ASSETS_UPDATED");
                }
                else if (rdoUpdateSerialNo.Checked)
                {
                    UpdateAssetSerialNo();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset serial nos. are updated successfully.');", true);
                    clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_SERIAL_NO_UPDATED");
                }
            }
            else if (ddlImportType.SelectedValue.ToString() == "ALLOCATE")
            {
                if (rdoAllocateFreshAssets.Checked)
                {
                    AllocateFreshAssets();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Fresh asset are allocated successfully.');", true);
                    clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "FRESH_ASSETS_ALLOCATED");
                }
                else if (rdoAllocateExistingAssets.Checked)
                {
                    AllocateExistingAssets();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Existing asset are allocated successfully.');", true);
                    clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "EXISTING_ASSETS_ALLOCATED");
                }
            }
            else if (ddlImportType.SelectedValue.ToString() == "VENDOR")
            {
                SaveVendorDetails();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor data is saved successfully.');", true);
            }
            else if (ddlImportType.SelectedValue.ToString() == "EMPLOYEE")
            {
                SaveEmployeeDetails();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Employee data is saved successfully.');", true);
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    /// <summary>
    /// Export gridview data to excel sheet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportAsset_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[4] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if (Session["EXPDATA"] != null)
            {
                Response.Clear();
                DataTable dt = (DataTable)Session["EXPDATA"];
                DataSet dsExport = new DataSet();
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.HeaderStyle.Font.Bold = true;
                dgGrid.DataBind();
                dgGrid.RenderControl(hw);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=ImportAssetDetails.xls");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : There is no data for being exported.');", true);
                return;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
        finally
        { }
    }

    /// <summary>
    /// Export assets as per given filters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGetExpAsset_Click(object sender, EventArgs e)
    {
        try
        {
            string AssetType = "", AssetCategoryCode = "", AssetMakeName = "", AssetModelName = null;
            AssetType = clsGeneral.gStrAssetType;
            if (ddlAssetCategory.SelectedIndex != 0)
                AssetCategoryCode = ddlAssetCategory.SelectedValue.ToString();
            else
                AssetCategoryCode = "";

            if (ddlAssetMake.SelectedIndex != 0)
                AssetMakeName = ddlAssetMake.SelectedValue.ToString();
            else
                AssetMakeName = "";

            for (int iCnt = 0; iCnt < lstModelName.Items.Count; iCnt++)
            {
                if (lstModelName.Items[iCnt].Selected)
                    AssetModelName += lstModelName.Items[iCnt].Value.ToString() + ",";
            }
            if (AssetModelName != null)
            {
                AssetModelName = AssetModelName.TrimEnd(',');
                AssetModelName = AssetModelName.Replace(",", "','");
                AssetModelName = "'" + AssetModelName + "'";
            }
            else
                AssetModelName = "";

            DataTable dt = new DataTable();
            dt = oDAL.GetAssetsForExport(AssetType, AssetCategoryCode, AssetMakeName, AssetModelName, Session["COMPANY"].ToString());
            if (dt.Rows.Count > 0)
            {
                Session["EXPDATA"] = dt;
                btnExportAsset.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Export Asstes details now.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Either Assets are not approved or not found as per given filters.');", true);
                return;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
        finally
        { oDAL = null; }
    }

    /// <summary>
    /// Refresh category.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshCategory_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            PopulateCategory(clsGeneral.gStrAssetType);
            DataTable dt = new DataTable();
            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            dt = null;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Reset/clear form fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateCategory(lblAssetType.Text);
            ddlAssetType2.SelectedValue = clsGeneral.gStrAssetType;
            DataTable dt = new DataTable();
            PopulateCategory(clsGeneral.gStrAssetType);
            ddlAssetMake.DataSource = dt;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dt;
            lstModelName.DataBind();
            dt = null;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region UPLOAD EMPLOYEE DETAILS
    /// <summary>
    /// Bulk "upload/save" employee details into EMPLOYEE_MASTER datatable.
    /// </summary>
    private void SaveEmployeeDetails()
    {
        EmployeeMaster_DAL oDAL = new EmployeeMaster_DAL(Session["DATABASE"].ToString());
        EmployeeMaster_PRP oPRP = new EmployeeMaster_PRP();
        try
        {
            bool bInValid = false;
            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            dtFileData = (DataTable)Session["EMPLOYEE"];
            for (iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
                Session["EMPLOYEE"] = null;
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }
                if (dtFileData.Rows[iCnt][0].ToString().Trim() != "")
                    oPRP.EmpCode = dtFileData.Rows[iCnt][0].ToString().Trim().ToUpper();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Employee Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][1].ToString().Trim() != "")
                    oPRP.EmpName = dtFileData.Rows[iCnt][1].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Employee Name is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.EmpCompCode = dtFileData.Rows[iCnt][2].ToString().Trim().ToUpper();
                oPRP.EmpProcCode = dtFileData.Rows[iCnt][3].ToString().Replace("'", "`").Trim();
                oPRP.EmpReprotTo = dtFileData.Rows[iCnt][4].ToString().Replace("'", "`").Trim();
                if (dtFileData.Rows[iCnt][5].ToString().Trim() != "")
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(dtFileData.Rows[iCnt][5].ToString().Trim(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") == true)
                        oPRP.EmpEmail = dtFileData.Rows[iCnt][5].ToString().Replace("'", "`").Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Employee E-mail format is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Employee E-mail is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.EmpDOJ = dtFileData.Rows[iCnt][6].ToString().Trim();
                if (dtFileData.Rows[iCnt][7].ToString().Trim() != "")
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(dtFileData.Rows[iCnt][7].ToString().Trim(), @"^([6789]{1})([0-9]{1})([0-9]{8})$") == true)
                        oPRP.EmpPhone = dtFileData.Rows[iCnt][7].ToString().Replace("'", "`").Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Employee Mobile No. format is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                    oPRP.EmpPhone = dtFileData.Rows[iCnt][7].ToString().Replace("'", "`").Trim();
                oPRP.CompCode = Session["COMPANY"].ToString().ToUpper();
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                oPRP.Active = true;
                oDAL.UploadEmployeeDetails(oPRP);
            }
            if (!bInValid)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Employee data is saved successfully.');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : An error has occured while uploading employee details, check file data.');", true);
            HandleExceptions(ex);
        }
        finally
        { oPRP = null; oDAL = null; }
    }
    #endregion

    #region UPLOAD VENDOR DETAILS
    /// <summary>
    /// Bulk "upload/save" Vendor details into VENDOR_MASTER datatable.
    /// </summary>
    private void SaveVendorDetails()
    {
        VendorMaster_DAL oDAL = new VendorMaster_DAL(Session["DATABASE"].ToString());
        VendorMaster_PRP oPRP = new VendorMaster_PRP();
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
                    oPRP.VendorCode = dtFileData.Rows[iCnt][0].ToString().Trim().ToUpper();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][1].ToString().Trim() != "")
                    oPRP.VendorName = dtFileData.Rows[iCnt][1].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlertShowAlert('Please Note : Vendor Name is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.VendorAddress = dtFileData.Rows[iCnt][2].ToString().Replace("'", "`").Trim();
                oPRP.VendorCountry = dtFileData.Rows[iCnt][3].ToString().Replace("'", "`").Trim();
                oPRP.VendorSate = dtFileData.Rows[iCnt][4].ToString().Replace("'", "`").Trim();
                if (dtFileData.Rows[iCnt][5].ToString().Trim() != "")
                    oPRP.VendorCity = dtFileData.Rows[iCnt][5].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor City Name is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.VendorPIN = dtFileData.Rows[iCnt][6].ToString().Trim();
                oPRP.VendorContPerson = dtFileData.Rows[iCnt][7].ToString().Replace("'", "`").Trim();
                if (dtFileData.Rows[iCnt][8].ToString().Trim() != "")
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(dtFileData.Rows[iCnt][8].ToString().Trim(), @"^([6789]{1})([0-9]{1})([0-9]{8})$") == true)
                        oPRP.VendorPhone = dtFileData.Rows[iCnt][8].ToString().Replace("'", "`").Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor Mobile No. format is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                    oPRP.VendorPhone = dtFileData.Rows[iCnt][8].ToString().Replace("'", "`").Trim();
                if (dtFileData.Rows[iCnt][9].ToString().Trim() != "")
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(dtFileData.Rows[iCnt][9].ToString().Trim(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") == true)
                        oPRP.VendorEmail = dtFileData.Rows[iCnt][9].ToString().Replace("'", "`").Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor E-mail format is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                oPRP.CompCode = Session["COMPANY"].ToString();
                oPRP.Active = true;
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                oDAL.UploadVendorDetails(oPRP);
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
    #endregion

    #region IMPORT FRESH ASSETS/UPDATE EXISTING ASSETS
    /// <summary>
    /// Update asset serial no. against asset code and old serial no.
    /// </summary>
    private void UpdateAssetSerialNo()
    {
        try
        {
            bool bInValid = false;
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            dtFileData = (DataTable)Session["ASSET"];
            oDAL.BeginTransaction();
            for (iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
                Session["ASSET"] = null;
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }
                if (dtFileData.Rows[iCnt][0].ToString().Trim() != "")
                    oPRP.AssetCode = dtFileData.Rows[iCnt][0].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][1].ToString().Trim() != "")
                {
                    if (oDAL.ValidSerialNo(dtFileData.Rows[iCnt][1].ToString().Trim()))
                        oPRP.OldSerialNo = dtFileData.Rows[iCnt][1].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Old Serial No. is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Old Serial No. is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][2].ToString().Trim() != "")
                    oPRP.NewSerialNo = dtFileData.Rows[iCnt][2].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : New Serial No. is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][3].ToString().Trim() != "")
                    oPRP.CompCode = dtFileData.Rows[iCnt][3].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Company Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                if (oPRP.CompCode == Session["COMPANY"].ToString())
                {
                    oDAL.UpdateAssetSerialNo(oPRP);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : You cannot update another location asset serial nos.');", true);
                    bInValid = true;
                    break;
                }
            }
            oDAL.CommitTransaction();
            if (!bInValid)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset serial nos. are updated successfully.');", true);
        }
        catch (Exception ex)
        {
            oDAL.RollBackTransaction();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : An error occured at row no. " + (iCnt + 1).ToString() + " while updating serial no., check file data.');", true);
            HandleExceptions(ex);
        }
        finally
        { dtFileData = null; }
    }

    /// <summary>
    /// Bulk "upload/save" asset details into ASSET_ACQUISITION datatable.
    /// </summary>
    private void SaveAssetDetails()
    {
        StreamWriter sw;
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
            oDAL.BeginTransaction();
            for (iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
                Session["ASSET"] = null;
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }
                oPRP.AssetID = dtFileData.Rows[iCnt][0].ToString().Trim();
                if (dtFileData.Rows[iCnt][1].ToString().Trim() != "")
                    oPRP.AssetSerialCode = dtFileData.Rows[iCnt][1].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Serial No. is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][2].ToString().Trim() != "")
                {
                    if (oDAL.ValidCategoryCode(dtFileData.Rows[iCnt][2].ToString().Trim()))
                        oPRP.AssetCategoryCode = dtFileData.Rows[iCnt][2].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Category Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Category Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][3].ToString().Trim() != "")
                {
                    if (oDAL.ValidLocationCode(dtFileData.Rows[iCnt][3].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.AssetLocation = dtFileData.Rows[iCnt][3].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Location Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Location Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AMC_Warranty = dtFileData.Rows[iCnt][4].ToString().Trim().ToUpper();
                if (oPRP.AMC_Warranty == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : AMC/WARRANTY/NONE not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (oPRP.AMC_Warranty == "AMC" || oPRP.AMC_Warranty == "WARRANTY")
                {
                    if (dtFileData.Rows[iCnt][5].ToString() == "" || dtFileData.Rows[iCnt][6].ToString() == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : AMC/WARRANTY start date/end date is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                    else
                    {
                        oPRP.AMC_Wrnty_Start_Date = dtFileData.Rows[iCnt][5].ToString();
                        oPRP.AMC_Wrnty_End_Date = dtFileData.Rows[iCnt][6].ToString();
                    }
                }
                else if (oPRP.AMC_Warranty == "NONE")
                {
                    oPRP.AMC_Wrnty_Start_Date = "01/Jan/1900";
                    oPRP.AMC_Wrnty_End_Date = "01/Jan/1900";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : AMC/WARRANTY/NONE has invalid value at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.VendorCode = dtFileData.Rows[iCnt][7].ToString().Trim();
                if (oPRP.VendorCode != "")
                {
                    if (!oDAL.ValidVendorCode(oPRP.VendorCode, Session["COMPANY"].ToString()))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                oPRP.AssetInstallDate = dtFileData.Rows[iCnt][8].ToString().Trim();
               // oPRP.PurchasedDate = dtFileData.Rows[iCnt][9].ToString().Trim();
                oPRP.InvoiceDate = dtFileData.Rows[iCnt][9].ToString().Trim();
                oPRP.AssetPurchaseValue = dtFileData.Rows[iCnt][10].ToString() != "" ? dtFileData.Rows[iCnt][10].ToString().Trim() : "0";
                oPRP.PurchaseOrderNo = dtFileData.Rows[iCnt][11].ToString().Replace("'", "`").Trim();
                oPRP.PODate = dtFileData.Rows[iCnt][12].ToString().Trim();
                oPRP.InvoiceNo = dtFileData.Rows[iCnt][13].ToString().Replace("'", "`").Trim();
                oPRP.AssetSaleDate = dtFileData.Rows[iCnt][14].ToString().Trim();
                oPRP.AssetSaleValue = dtFileData.Rows[iCnt][15].ToString() != "" ? dtFileData.Rows[iCnt][15].ToString().Trim() : "0";
                oPRP.AssetMakeName = dtFileData.Rows[iCnt][16].ToString().Replace("'", "`").Trim();
                oPRP.AssetModelName = dtFileData.Rows[iCnt][17].ToString().Replace("'", "`").Trim();

                if (dtFileData.Rows[iCnt][18].ToString().Trim() != "")
                {
                    if (oDAL.ValidProcessCode(dtFileData.Rows[iCnt][18].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.AssetProcess = dtFileData.Rows[iCnt][18].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Program name is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Program name is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                oPRP.SecurityClassification = dtFileData.Rows[iCnt][19].ToString().Replace("'", "`").Trim();
                oPRP.AssetSize = dtFileData.Rows[iCnt][20].ToString().Replace("'", "`").Trim();
                oPRP.AssetVlan = dtFileData.Rows[iCnt][21].ToString().Replace("'", "`").Trim();
                oPRP.AssetHDD = dtFileData.Rows[iCnt][22].ToString().Replace("'", "`").Trim();
                oPRP.AssetProcessor = dtFileData.Rows[iCnt][23].ToString().Replace("'", "`").Trim();
                oPRP.AssetRAM = dtFileData.Rows[iCnt][24].ToString().Replace("'", "`").Trim();
                oPRP.AssetIMEINo = dtFileData.Rows[iCnt][25].ToString().Replace("'", "`").Trim();
                oPRP.AssetPhoneMemory = dtFileData.Rows[iCnt][26].ToString().Replace("'", "`").Trim();
                oPRP.ServiceProvider = dtFileData.Rows[iCnt][27].ToString().Replace("'", "`").Trim();

                if (dtFileData.Rows[iCnt][28].ToString().Trim() == "IT" || dtFileData.Rows[iCnt][28].ToString().Trim() == "ADMIN")
                    oPRP.AssetType = dtFileData.Rows[iCnt][28].ToString().Trim().ToUpper();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Type (IT/ADMIN) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetBOE = dtFileData.Rows[iCnt][29].ToString().Replace("'", "`").Trim();
                oPRP.RegisterNo = dtFileData.Rows[iCnt][30].ToString().Replace("'", "`").Trim();
                oPRP.BondedType = dtFileData.Rows[iCnt][31].ToString().Trim();
                //if (dtFileData.Rows[iCnt][31].ToString().Trim() == "CBD" || dtFileData.Rows[iCnt][31].ToString().Trim() == "NCBD")
                //    oPRP.BondedType = dtFileData.Rows[iCnt][31].ToString().Trim();
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Bonded Type (CBD/NCBD) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}
                //if (oPRP.BondedType == "CBD")
                //{
                //    if (oPRP.AssetBOE == "")
                //    {
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : For asset bonded type as CBD, asset BOE no. not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //        bInValid = true;
                //        break;
                //    }
                //}
                if (dtFileData.Rows[iCnt][32].ToString().Trim() == "CAP" || dtFileData.Rows[iCnt][32].ToString().Trim() == "NCAP")
                    oPRP.CapitalisedType = dtFileData.Rows[iCnt][32].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Capitalised Status (CAP/NCAP) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                if (dtFileData.Rows[iCnt][33].ToString().Trim() == "VER" || dtFileData.Rows[iCnt][33].ToString().Trim() == "NVER")
                    oPRP.VerifiableType = dtFileData.Rows[iCnt][33].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Verifiable Status (VER/NVER) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                oPRP.PortNo = dtFileData.Rows[iCnt][34].ToString().Replace("'", "`").Trim();
                //if (dtFileData.Rows[iCnt][34].ToString().Trim() != "")
                //    oPRP.PortNo = dtFileData.Rows[iCnt][34].ToString().Replace("'", "`").Trim();
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Port No. is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}
                oPRP.WorkStationNo = dtFileData.Rows[iCnt][35].ToString().Replace("'", "`").Trim();
                oPRP.CostCenterNo = dtFileData.Rows[iCnt][36].ToString().Replace("'", "`").Trim();
                oPRP.SecurityGENo = dtFileData.Rows[iCnt][37].ToString().Replace("'", "`").Trim();
                oPRP.SecurityGEDate = dtFileData.Rows[iCnt][38].ToString().Trim();
                if (dtFileData.Rows[iCnt][39].ToString().Trim() == "" || dtFileData.Rows[iCnt][40].ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Company Code/Company Name is/are left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                else
                {
                    oPRP.CompCode = dtFileData.Rows[iCnt][39].ToString().Trim().ToUpper();
                    oPRP.CompanyName = dtFileData.Rows[iCnt][40].ToString().Replace("'", "`").Trim();
                }
                oPRP.CustomerOrderNo = dtFileData.Rows[iCnt][41].ToString().Trim();
                if (dtFileData.Rows[iCnt][42].ToString().Trim() != "")
                {
                    if (oDAL.ValidDepartmentCode(dtFileData.Rows[iCnt][42].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.DeptCode = dtFileData.Rows[iCnt][42].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Department Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Department Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.Remarks = dtFileData.Rows[iCnt][43].ToString().Trim();
                oPRP.AssetLife = dtFileData.Rows[iCnt][49].ToString().Trim();
                oPRP.CompanyCost = dtFileData.Rows[iCnt][50].ToString().Trim();
                oPRP.IncomeTaxCost = dtFileData.Rows[iCnt][51].ToString().Trim();
                oPRP.AssetQty = string.IsNullOrEmpty(dtFileData.Rows[iCnt][52].ToString().Trim()) ? "0" :    dtFileData.Rows[iCnt][52].ToString().Trim();
                //--testing code---------------------
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Capitalisation date should not be blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                //break;
                //-----end-------------------

                oPRP.ParentAssetCode = dtFileData.Rows[iCnt][53].ToString().Trim();
                oPRP.Custodian = dtFileData.Rows[iCnt][54].ToString().Trim();


               
                oPRP.CapitalisationDate = dtFileData.Rows[iCnt][56].ToString().Trim();



                decimal dd = 0;

                //if (string.IsNullOrEmpty(oPRP.AssetLife))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Life should not be blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;

                //}
                //if (!string.IsNullOrEmpty(oPRP.AssetLife) && clsGeneral.ToDate(oPRP.AssetLife) == default(DateTime?))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Life should be  valid date format (dd/mmm/yyyy) blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                //dd = 0;
                //if (string.IsNullOrEmpty(oPRP.CapitalisationDate))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Capitalisation date should not be blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;

                //}
                //if (!string.IsNullOrEmpty(oPRP.CapitalisationDate) && clsGeneral.ToDate(oPRP.CapitalisationDate) == default(DateTime?))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Capitalisation date should be  valid date format (dd/mmm/yyyy) blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                
                
                
                
                //if (string.IsNullOrEmpty(oPRP.CompanyCost) || !decimal.TryParse(oPRP.CompanyCost, out dd))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Company cost should not be blank and only numeric value at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                //if (string.IsNullOrEmpty(oPRP.IncomeTaxCost) || !decimal.TryParse(oPRP.IncomeTaxCost, out dd))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Incometax cost should not be blank and numeric value invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}
                //if (string.IsNullOrEmpty(oPRP.AssetQty) || !decimal.TryParse(oPRP.AssetQty, out dd))
                //{
                //    if (dd==0)
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Assset Qty should not be blank and numeric value >0 at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                //if (!string.IsNullOrEmpty(oPRP.ParentAssetCode) && !oDAL.ValidParentAssetCode(oPRP.ParentAssetCode, Session["COMPANY"].ToString()))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Invalid ParentAssetCode value  at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                //try
                //{
                //    oPRP.IsPhysicalVerified = Convert.ToBoolean(dtFileData.Rows[iCnt][55].ToString().Trim());
                //}
                //catch
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : PhysicalVerified value should be true or false at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();

                if (oPRP.CompCode == Session["COMPANY"].ToString())
                {
                    int _iMaxACQId = oDAL.GetMaxAcquisitionId(oPRP.AssetCategoryCode, oPRP.CompCode);
                    oPRP.RunningSerialNo = _iMaxACQId;
                    string s = oPRP.AssetCategoryCode.ToString().Replace("-00", "");
                    string str = s.Substring(s.Length - 2, 2);
                    oPRP.AssetCode = oPRP.CompCode + "-" + ddlAssetType.SelectedValue.ToString() + "-" + str + "-" + _iMaxACQId.ToString().PadLeft(6, '0');

                    if (!oDAL.CheckDuplicateSerialNo(oPRP.AssetSerialCode, oPRP.CompCode) || !oDAL.CheckDuplicateIMEINo(oPRP.AssetIMEINo, oPRP.CompCode))
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
                       // StreamWriter sw;
                        if (File.Exists(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt"))
                        {
                            sw = File.AppendText(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt");
                            sw.WriteLine(oPRP.AssetSerialCode, oPRP.AssetIMEINo);
                        }
                        else
                        {
                            sw = new StreamWriter(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt");
                            sw.WriteLine(oPRP.AssetSerialCode, oPRP.AssetIMEINo);
                        }
                        sw.Close();
                        sw = null;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : You cannot upload assets at another location.');", true);
                    bInValid = true;
                    break;
                }
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
        {

            //if (sw != null)
            //    sw = null;
            dtFileData = null; }
    }

    /// <summary>
    /// Asset details bulk update into ASSET_ACQUISITION table.
    /// </summary>
    private void UpdateAssetDetails()
    {
        try
        {
            int iRslt = 0;
            bool bInValid = false;
            if (clsGeneral._strRights[2] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            dtFileData = (DataTable)Session["ASSET"];
            oDAL.BeginTransaction();
            for (iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
                Session["ASSET"] = null;
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }
                if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                    break;
                if (dtFileData.Rows[iCnt][0].ToString().Trim() != "")
                    oPRP.AssetCode = dtFileData.Rows[iCnt][0].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetID = dtFileData.Rows[iCnt][1].ToString().Trim();

                if (dtFileData.Rows[iCnt][2].ToString().Trim() != "")
                    oPRP.AssetSerialCode = dtFileData.Rows[iCnt][2].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Serial No. is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                if (dtFileData.Rows[iCnt][3].ToString().Trim() != "")
                {
                    if (oDAL.ValidCategoryCode(dtFileData.Rows[iCnt][3].ToString().Trim()))
                        oPRP.AssetCategoryCode = dtFileData.Rows[iCnt][3].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Category Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Category Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (oDAL.CategoryCodeChanged(oPRP.AssetCode, oPRP.AssetCategoryCode, Session["COMPANY"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Category Code cannot be changed in asset updation, check row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                if (dtFileData.Rows[iCnt][4].ToString().Trim() != "")
                {
                    if (oDAL.ValidLocationCode(dtFileData.Rows[iCnt][4].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.AssetLocation = dtFileData.Rows[iCnt][4].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Location Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Location Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AMC_Warranty = dtFileData.Rows[iCnt][5].ToString().Trim().ToUpper();
                if (oPRP.AMC_Warranty == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : AMC/WARRANTY/NONE not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (oPRP.AMC_Warranty == "AMC" || oPRP.AMC_Warranty == "WARRANTY")
                {
                    if (dtFileData.Rows[iCnt][6].ToString() == "" || dtFileData.Rows[iCnt][7].ToString() == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : AMC/WARRANTY start date/end date is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                    else
                    {
                        oPRP.AMC_Wrnty_Start_Date = dtFileData.Rows[iCnt][6].ToString();
                        oPRP.AMC_Wrnty_End_Date = dtFileData.Rows[iCnt][7].ToString();
                    }
                }
                else if (oPRP.AMC_Warranty == "NONE")
                {
                    oPRP.AMC_Wrnty_Start_Date = "01/Jan/1900";
                    oPRP.AMC_Wrnty_End_Date = "01/Jan/1900";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : AMC/WARRANTY/NONE has invalid value at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.VendorCode = dtFileData.Rows[iCnt][8].ToString().Trim();
                if (oPRP.VendorCode != "")
                {
                    if (!oDAL.ValidVendorCode(oPRP.VendorCode, Session["COMPANY"].ToString()))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                oPRP.AssetInstallDate = dtFileData.Rows[iCnt][9].ToString().Trim();
           //     oPRP.PurchasedDate = dtFileData.Rows[iCnt][10].ToString().Trim();
                oPRP.InvoiceDate = dtFileData.Rows[iCnt][10].ToString().Trim();
                oPRP.AssetPurchaseValue = dtFileData.Rows[iCnt][11].ToString() != "" ? dtFileData.Rows[iCnt][11].ToString().Trim() : "0";
                oPRP.PurchaseOrderNo = dtFileData.Rows[iCnt][12].ToString().Replace("'", "`").Trim();
                oPRP.PODate = dtFileData.Rows[iCnt][13].ToString().Trim();
                oPRP.InvoiceNo = dtFileData.Rows[iCnt][14].ToString().Replace("'", "`").Trim();
                oPRP.AssetSaleDate = dtFileData.Rows[iCnt][15].ToString().Trim();
                oPRP.AssetSaleValue = dtFileData.Rows[iCnt][16].ToString() != "" ? dtFileData.Rows[iCnt][16].ToString().Trim() : "0";
                oPRP.AssetMakeName = dtFileData.Rows[iCnt][17].ToString().Replace("'", "`").Trim();
                oPRP.AssetModelName = dtFileData.Rows[iCnt][18].ToString().Replace("'", "`").Trim();

                if (dtFileData.Rows[iCnt][19].ToString().Trim() != "")
                {
                    if (oDAL.ValidProcessCode(dtFileData.Rows[iCnt][19].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.AssetProcess = dtFileData.Rows[iCnt][19].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Program name is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Program name is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.SecurityClassification = dtFileData.Rows[iCnt][20].ToString().Replace("'", "`").Trim();
                oPRP.AssetSize = dtFileData.Rows[iCnt][21].ToString().Replace("'", "`").Trim();
                oPRP.AssetVlan = dtFileData.Rows[iCnt][22].ToString().Replace("'", "`").Trim();
                oPRP.AssetHDD = dtFileData.Rows[iCnt][23].ToString().Replace("'", "`").Trim();
                oPRP.AssetProcessor = dtFileData.Rows[iCnt][24].ToString().Replace("'", "`").Trim();
                oPRP.AssetRAM = dtFileData.Rows[iCnt][25].ToString().Replace("'", "`").Trim();
                oPRP.AssetIMEINo = dtFileData.Rows[iCnt][26].ToString().Replace("'", "`").Trim();
                oPRP.AssetPhoneMemory = dtFileData.Rows[iCnt][27].ToString().Replace("'", "`").Trim();
                oPRP.ServiceProvider = dtFileData.Rows[iCnt][28].ToString().Replace("'", "`").Trim();

                if (dtFileData.Rows[iCnt][29].ToString().Trim() == "IT" || dtFileData.Rows[iCnt][29].ToString().Trim() == "ADMIN")
                    oPRP.AssetType = dtFileData.Rows[iCnt][29].ToString().Trim().ToUpper();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Type (IT/ADMIN) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetBOE = dtFileData.Rows[iCnt][30].ToString().Replace("'", "`").Trim();
                oPRP.RegisterNo = dtFileData.Rows[iCnt][31].ToString().Replace("'", "`").Trim();
                oPRP.BondedType = dtFileData.Rows[iCnt][32].ToString().Trim();
                //if (dtFileData.Rows[iCnt][32].ToString().Trim() == "CBD" || dtFileData.Rows[iCnt][32].ToString().Trim() == "NCBD")
                //    oPRP.BondedType = dtFileData.Rows[iCnt][32].ToString().Trim();
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Bonded Type (CBD/NCBD) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}
                //if (oPRP.BondedType == "CBD")
                //{
                //    if (oPRP.AssetBOE == "")
                //    {
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : For asset bonded type as CBD, asset BOE no. not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                //        bInValid = true;
                //        break;
                //    }
                //}
                if (dtFileData.Rows[iCnt][33].ToString().Trim() == "CAP" || dtFileData.Rows[iCnt][33].ToString().Trim() == "NCAP")
                    oPRP.CapitalisedType = dtFileData.Rows[iCnt][33].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Capitalised Status (CAP/NCAP) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                if (dtFileData.Rows[iCnt][34].ToString().Trim() == "VER" || dtFileData.Rows[iCnt][34].ToString().Trim() == "NVER")
                    oPRP.VerifiableType = dtFileData.Rows[iCnt][34].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Verifiable Status (VER/NVER) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                oPRP.PortNo = dtFileData.Rows[iCnt][35].ToString().Replace("'", "`").Trim();
                //if (dtFileData.Rows[iCnt][35].ToString().Trim() != "")
                //    oPRP.PortNo = dtFileData.Rows[iCnt][35].ToString().Replace("'", "`").Trim();
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Port No. is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}
                oPRP.WorkStationNo = dtFileData.Rows[iCnt][36].ToString().Replace("'", "`").Trim();
                oPRP.CostCenterNo = dtFileData.Rows[iCnt][37].ToString().Replace("'", "`").Trim();
                oPRP.SecurityGENo = dtFileData.Rows[iCnt][38].ToString().Replace("'", "`").Trim();
                oPRP.SecurityGEDate = dtFileData.Rows[iCnt][39].ToString().Trim();
                if (dtFileData.Rows[iCnt][40].ToString().Trim() == "" || dtFileData.Rows[iCnt][41].ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Company Code/Company Name is/are left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                else
                {
                    oPRP.CompCode = dtFileData.Rows[iCnt][40].ToString().Trim().ToUpper();
                    oPRP.CompanyName = dtFileData.Rows[iCnt][41].ToString().Replace("'", "`").Trim();
                }
                oPRP.CustomerOrderNo = dtFileData.Rows[iCnt][42].ToString().Trim();
                if (dtFileData.Rows[iCnt][43].ToString().Trim() != "")
                {
                    if (oDAL.ValidDepartmentCode(dtFileData.Rows[iCnt][43].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.DeptCode = dtFileData.Rows[iCnt][43].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Department Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Department Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.Remarks = dtFileData.Rows[iCnt][44].ToString().Trim();
                oPRP.AssetLife = dtFileData.Rows[iCnt][50].ToString().Trim();
                oPRP.CompanyCost = dtFileData.Rows[iCnt][51].ToString().Trim();
                oPRP.IncomeTaxCost = dtFileData.Rows[iCnt][52].ToString().Trim();
                oPRP.AssetQty = string.IsNullOrEmpty(dtFileData.Rows[iCnt][53].ToString().Trim()) ? "0" : dtFileData.Rows[iCnt][53].ToString().Trim();

                oPRP.ParentAssetCode = dtFileData.Rows[iCnt][54].ToString().Trim();
                oPRP.Custodian = dtFileData.Rows[iCnt][55].ToString().Trim();



                oPRP.CapitalisationDate = dtFileData.Rows[iCnt][57].ToString().Trim();



                decimal dd = 0;

                if (string.IsNullOrEmpty(oPRP.AssetLife))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Life should not be blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;

                }
                //if (!string.IsNullOrEmpty(oPRP.AssetLife) && clsGeneral.ToDate(oPRP.AssetLife) == default(DateTime?))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Life should be  valid date format (dd/mmm/yyyy) blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}

                dd = 0;
                if (string.IsNullOrEmpty(oPRP.CapitalisationDate))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Capitalisation date should not be blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;

                }
                //if (!string.IsNullOrEmpty(oPRP.CapitalisationDate) && clsGeneral.ToDate(oPRP.CapitalisationDate) == default(DateTime?))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Capitalisation date should be  valid date format (dd/mmm/yyyy) blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}





                if (string.IsNullOrEmpty(oPRP.CompanyCost) || !decimal.TryParse(oPRP.CompanyCost, out dd))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Company cost should not be blank and only numeric value at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                if (string.IsNullOrEmpty(oPRP.IncomeTaxCost) || !decimal.TryParse(oPRP.IncomeTaxCost, out dd))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Incometax cost should not be blank and numeric value invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (string.IsNullOrEmpty(oPRP.AssetQty) || !decimal.TryParse(oPRP.AssetQty, out dd))
                {
                    if (dd == 0)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Assset Qty should not be blank and numeric value >0 at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                if (!string.IsNullOrEmpty(oPRP.ParentAssetCode) && !oDAL.ValidParentAssetCode(oPRP.ParentAssetCode, Session["COMPANY"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Invalid ParentAssetCode value  at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                try
                {
                    oPRP.IsPhysicalVerified = Convert.ToBoolean(dtFileData.Rows[iCnt][56].ToString().Trim());
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : PhysicalVerified value should be true or false at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }


                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                if (oPRP.CompCode == Session["COMPANY"].ToString())
                {
                    iRslt = oDAL.UpdateAssetAcquisitionDetails(oPRP);
                    if (iRslt == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details not updated at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : You cannot update another location asset details.');", true);
                    bInValid = true;
                    break;
                }
            }
            oDAL.CommitTransaction();
            if (!bInValid)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Asset data is updated successfully.');", true);
        }
        catch (Exception ex)
        {
            oDAL.RollBackTransaction();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : An error occured at row no. " + (iCnt + 1).ToString() + " while updating asset details, check file data.');", true);
            HandleExceptions(ex);
        }
        finally
        { dtFileData = null; }
    }
    #endregion

    #region ALLOCATE FRESH ASSETS/ALLOCATE EXISTING ASSETS
    /// <summary>
    /// Import fresh assets and allocate to employees in bulk.
    /// </summary>
    private void AllocateFreshAssets()
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
            dtFileData = (DataTable)Session["ALLOCATE"];
            oDAL.BeginTransaction();
            for (iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
                Session["ALLOCATE"] = null;
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }
                oPRP.AssetID = dtFileData.Rows[iCnt][0].ToString().Trim();
                if (dtFileData.Rows[iCnt][1].ToString().Trim() != "")
                    oPRP.AssetSerialCode = dtFileData.Rows[iCnt][1].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Serial No. is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][2].ToString().Trim() != "")
                {
                    if (oDAL.ValidCategoryCode(dtFileData.Rows[iCnt][2].ToString().Trim()))
                        oPRP.AssetCategoryCode = dtFileData.Rows[iCnt][2].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Category Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Category Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][3].ToString().Trim() != "")
                {
                    if (oDAL.ValidLocationCode(dtFileData.Rows[iCnt][3].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.AssetLocation = dtFileData.Rows[iCnt][3].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Location Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Location Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AMC_Warranty = dtFileData.Rows[iCnt][4].ToString().Trim().ToUpper();
                if (oPRP.AMC_Warranty == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : AMC/WARRANTY/NONE not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (oPRP.AMC_Warranty == "AMC" || oPRP.AMC_Warranty == "WARRANTY")
                {
                    if (dtFileData.Rows[iCnt][5].ToString() == "" || dtFileData.Rows[iCnt][6].ToString() == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : AMC/WARRANTY start date/end date is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                    else
                    {
                        oPRP.AMC_Wrnty_Start_Date = dtFileData.Rows[iCnt][5].ToString();
                        oPRP.AMC_Wrnty_End_Date = dtFileData.Rows[iCnt][6].ToString();
                    }
                }
                else if (oPRP.AMC_Warranty == "NONE")
                {
                    oPRP.AMC_Wrnty_Start_Date = "01/Jan/1900";
                    oPRP.AMC_Wrnty_End_Date = "01/Jan/1900";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : AMC/WARRANTY/NONE has invalid value at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.VendorCode = dtFileData.Rows[iCnt][7].ToString().Trim();
                if (oPRP.VendorCode != "")
                {
                    if (!oDAL.ValidVendorCode(oPRP.VendorCode, Session["COMPANY"].ToString()))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Vendor Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                oPRP.AssetInstallDate = dtFileData.Rows[iCnt][8].ToString().Trim();
              //  oPRP.PurchasedDate = dtFileData.Rows[iCnt][9].ToString().Trim();
                oPRP.InvoiceDate = dtFileData.Rows[iCnt][9].ToString().Trim();
                oPRP.AssetPurchaseValue = dtFileData.Rows[iCnt][10].ToString() != "" ? dtFileData.Rows[iCnt][10].ToString().Trim() : "0";
                oPRP.PurchaseOrderNo = dtFileData.Rows[iCnt][11].ToString().Replace("'", "`").Trim();
                oPRP.PODate = dtFileData.Rows[iCnt][12].ToString().Trim();
                oPRP.InvoiceNo = dtFileData.Rows[iCnt][13].ToString().Replace("'", "`").Trim();
                oPRP.AssetSaleDate = dtFileData.Rows[iCnt][14].ToString().Trim();
                oPRP.AssetSaleValue = dtFileData.Rows[iCnt][15].ToString() != "" ? dtFileData.Rows[iCnt][15].ToString().Trim() : "0";
                oPRP.AssetMakeName = dtFileData.Rows[iCnt][16].ToString().Replace("'", "`").Trim();
                oPRP.AssetModelName = dtFileData.Rows[iCnt][17].ToString().Replace("'", "`").Trim();

                if (dtFileData.Rows[iCnt][18].ToString().Trim() != "")
                {
                    if (oDAL.ValidProcessCode(dtFileData.Rows[iCnt][18].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.AssetProcess = dtFileData.Rows[iCnt][18].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Process Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Process Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.SecurityClassification = dtFileData.Rows[iCnt][19].ToString().Replace("'", "`").Trim();
                oPRP.AssetSize = dtFileData.Rows[iCnt][20].ToString().Replace("'", "`").Trim();
                oPRP.AssetVlan = dtFileData.Rows[iCnt][21].ToString().Replace("'", "`").Trim();
                oPRP.AssetHDD = dtFileData.Rows[iCnt][22].ToString().Replace("'", "`").Trim();
                oPRP.AssetProcessor = dtFileData.Rows[iCnt][23].ToString().Replace("'", "`").Trim();
                oPRP.AssetRAM = dtFileData.Rows[iCnt][24].ToString().Replace("'", "`").Trim();
                oPRP.AssetIMEINo = dtFileData.Rows[iCnt][25].ToString().Replace("'", "`").Trim();
                oPRP.AssetPhoneMemory = dtFileData.Rows[iCnt][26].ToString().Replace("'", "`").Trim();
                oPRP.ServiceProvider = dtFileData.Rows[iCnt][27].ToString().Replace("'", "`").Trim();

                if (dtFileData.Rows[iCnt][28].ToString().Trim() == "IT" || dtFileData.Rows[iCnt][28].ToString().Trim() == "ADMIN")
                    oPRP.AssetType = dtFileData.Rows[iCnt][28].ToString().Trim().ToUpper();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Type (IT/ADMIN) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetBOE = dtFileData.Rows[iCnt][29].ToString().Replace("'", "`").Trim();
                oPRP.RegisterNo = dtFileData.Rows[iCnt][30].ToString().Replace("'", "`").Trim();

                if (dtFileData.Rows[iCnt][31].ToString().Trim() == "CBD" || dtFileData.Rows[iCnt][31].ToString().Trim() == "NCBD")
                    oPRP.BondedType = dtFileData.Rows[iCnt][31].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Bonded Type (CBD/NCBD) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (oPRP.BondedType == "CBD")
                {
                    if (oPRP.AssetBOE == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : For asset bonded type as CBD, asset BOE no. not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                if (dtFileData.Rows[iCnt][32].ToString().Trim() == "CAP" || dtFileData.Rows[iCnt][32].ToString().Trim() == "NCAP")
                    oPRP.CapitalisedType = dtFileData.Rows[iCnt][32].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Capitalised Status (CAP/NCAP) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                if (dtFileData.Rows[iCnt][33].ToString().Trim() == "VER" || dtFileData.Rows[iCnt][33].ToString().Trim() == "NVER")
                    oPRP.VerifiableType = dtFileData.Rows[iCnt][33].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Verifiable Status (VER/NVER) not found at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                if (dtFileData.Rows[iCnt][34].ToString().Trim() != "")
                    oPRP.PortNo = dtFileData.Rows[iCnt][34].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Port No. is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.WorkStationNo = dtFileData.Rows[iCnt][35].ToString().Replace("'", "`").Trim();
                oPRP.CostCenterNo = dtFileData.Rows[iCnt][36].ToString().Replace("'", "`").Trim();
                oPRP.SecurityGENo = dtFileData.Rows[iCnt][37].ToString().Replace("'", "`").Trim();
                oPRP.SecurityGEDate = dtFileData.Rows[iCnt][38].ToString().Trim();
                if (dtFileData.Rows[iCnt][39].ToString().Trim() == "" || dtFileData.Rows[iCnt][40].ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Company Code/Company Name is/are left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                else
                {
                    oPRP.CompCode = dtFileData.Rows[iCnt][39].ToString().Trim().ToUpper();
                    oPRP.CompanyName = dtFileData.Rows[iCnt][40].ToString().Replace("'", "`").Trim();
                }
                oPRP.CustomerOrderNo = dtFileData.Rows[iCnt][41].ToString().Trim();
                if (dtFileData.Rows[iCnt][42].ToString().Trim() != "")
                {
                    if (oDAL.ValidDepartmentCode(dtFileData.Rows[iCnt][42].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.DeptCode = dtFileData.Rows[iCnt][42].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Department Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Department Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][43].ToString().Trim() != "")
                {
                    string EmpName = oDAL.ValidEmployeeCode(dtFileData.Rows[iCnt][43].ToString().Trim(), Session["COMPANY"].ToString());
                    if (EmpName != "NOT_FOUND")
                    {
                        oPRP.AllocatedToId = dtFileData.Rows[iCnt][43].ToString().Trim();
                        oPRP.AllocatedTo = EmpName.ToString().Trim();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Employee Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Allocated To Employee Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][44].ToString().Trim() != "")
                    oPRP.AllocationDate = dtFileData.Rows[iCnt][44].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Allocation Date is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetVlan = dtFileData.Rows[iCnt][45].ToString().Replace("'", "`").Trim();
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                if (oPRP.CompCode == Session["COMPANY"].ToString())
                {
                    int _iMaxACQId = oDAL.GetMaxAcquisitionId(oPRP.AssetCategoryCode, oPRP.CompCode);
                    oPRP.RunningSerialNo = _iMaxACQId;
                    string s = oPRP.AssetCategoryCode.ToString().Replace("-00", "");
                    string str = s.Substring(s.Length - 2, 2);
                    oPRP.AssetCode = oPRP.CompCode + "-" + ddlAssetType.SelectedValue.ToString() + "-" + str + "-" + _iMaxACQId.ToString().PadLeft(6, '0');

                    if (!oDAL.CheckDuplicateSerialNo(oPRP.AssetSerialCode, oPRP.CompCode) || !oDAL.CheckDuplicateIMEINo(oPRP.AssetIMEINo, oPRP.CompCode))
                    {
                        iRslt = oDAL.ImportFreshAssetsAcquisition(oPRP);
                        int iAllocate = oDAL.AllocateFreshAssets(oPRP);
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
                            sw.WriteLine(oPRP.AssetSerialCode, oPRP.AssetIMEINo);
                        }
                        else
                        {
                            sw = new StreamWriter(strDupFilePath + DateTime.Today.ToString("dd_MMM_yyyy") + "_" + Session["COMPANY"].ToString() + ".txt");
                            sw.WriteLine(oPRP.AssetSerialCode, oPRP.AssetIMEINo);
                        }
                        sw.Close();
                        sw = null;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : You cannot upload/allocate another location assets.');", true);
                    bInValid = true;
                    break;
                }
            }
            oDAL.CommitTransaction();
            if (!bInValid)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Fresh assets (except duplicate serial no./IMEI no.) are allocated successfully.');", true);
        }
        catch (Exception ex)
        {
            oDAL.RollBackTransaction();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : An error occured at row no. " + (iCnt + 1).ToString() + " while saving/allocating assets, check file data.');", true);
            HandleExceptions(ex);
        }
        finally
        { }
    }

    /// <summary>
    /// Allocate existing assets in bulk to respective employees.
    /// </summary>
    private void AllocateExistingAssets()
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
            dtFileData = (DataTable)Session["ALLOCATE"];
            oDAL.BeginTransaction();
            for (iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
                Session["ALLOCATE"] = null;
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }
                if (dtFileData.Rows[iCnt][0].ToString().Trim() != "")
                    oPRP.AssetCode = dtFileData.Rows[iCnt][0].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Barcode is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][1].ToString().Trim() != "")
                    oPRP.AllocationDate = dtFileData.Rows[iCnt][1].ToString().Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset allocation date is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][2].ToString().Trim() != "")
                {
                    string EmpName = oDAL.ValidEmployeeCode(dtFileData.Rows[iCnt][2].ToString().Trim(), Session["COMPANY"].ToString());
                    if (EmpName != "NOT_FOUND")
                    {
                        oPRP.AllocatedToId = dtFileData.Rows[iCnt][2].ToString().Trim();
                        oPRP.AllocatedTo = EmpName.ToString().Trim();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Employee Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Allocated To Employee Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][3].ToString().Trim() != "")
                {
                    if (oDAL.ValidDepartmentCode(dtFileData.Rows[iCnt][3].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.DeptCode = dtFileData.Rows[iCnt][3].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Department Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Department Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][4].ToString().Trim() != "")
                {
                    if (oDAL.ValidProcessCode(dtFileData.Rows[iCnt][4].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.AssetProcess = dtFileData.Rows[iCnt][4].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Process Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Process Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][5].ToString().Trim() != "")
                {
                    if (oDAL.ValidLocationCode(dtFileData.Rows[iCnt][5].ToString().Trim(), Session["COMPANY"].ToString()))
                        oPRP.AssetLocation = dtFileData.Rows[iCnt][5].ToString().Trim();
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Location Code is invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
                        bInValid = true;
                        break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Location Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][6].ToString().Trim() != "")
                    oPRP.CompCode = dtFileData.Rows[iCnt][6].ToString().Trim().ToUpper();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Company Code is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                if (dtFileData.Rows[iCnt][7].ToString().Trim() != "")
                    oPRP.PortNo = dtFileData.Rows[iCnt][7].ToString().Replace("'", "`").Trim();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Port No. is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                oPRP.AssetVlan = dtFileData.Rows[iCnt][8].ToString().Replace("'", "`").Trim();
                oPRP.TicketNo = dtFileData.Rows[iCnt][9].ToString().Replace("'", "`").Trim();
                oPRP.GatePassNo = dtFileData.Rows[iCnt][10].ToString().Replace("'", "`").Trim();
                oPRP.WorkStationNo = dtFileData.Rows[iCnt][11].ToString().Replace("'", "`").Trim();
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                if (oPRP.CompCode == Session["COMPANY"].ToString())
                {
                    iRslt = oDAL.AllocateExistingAssets(oPRP);
                    int iResult = oDAL.UpdateAssetAcquisition(oPRP);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : You cannot allocate another location assets.');", true);
                    bInValid = true;
                    break;
                }
            }
            oDAL.CommitTransaction();
            if (!bInValid)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Existing assets are allocated successfully.');", true);
        }
        catch (Exception ex)
        {
            oDAL.RollBackTransaction();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : An error occured at row no. " + (iCnt + 1).ToString() + " while uploading asset details, check file data.');", true);
            HandleExceptions(ex);
        }
        finally
        { }
    }
    #endregion

    #region FILE UPLOAD FUNCTIONS
    /// <summary>
    /// Upload Employee data file.
    /// </summary>
    private void UploadEmployeeFile()
    {
        string strFileName = EmployeeFileUpload.FileName;
        if (strFileName != "")
        {
            String fileExt = System.IO.Path.GetExtension(EmployeeFileUpload.FileName);
            if (fileExt.ToUpper() != ".XLSX" && fileExt.ToUpper() != ".XLS")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select an excel file (.xls/.xlsx) only.');", true);
                return;
            }
            if (UploadFile())
            {
                MyExcel oExcel = new MyExcel();
                dtFileData = oExcel.ReadExcel(strFilePath);
                Session["EMPLOYEE"] = dtFileData;
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

    /// <summary>
    /// Uppload Vendor data file.
    /// </summary>
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

    /// <summary>
    /// Upload Asset data file.
    /// </summary>
    private void UploadAssetFile()
    {
        if (ddlAssetType.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select Asset Type.');", true);
            return;
        }
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

    /// <summary>
    /// Upload asset allocation file.
    /// </summary>
    private void UploadAllocationFile()
    {
        string strFileName = AllocationFileUpload.FileName;
        if (strFileName != "")
        {
            String fileExt = System.IO.Path.GetExtension(AllocationFileUpload.FileName);
            if (fileExt.ToUpper() != ".XLSX" && fileExt.ToUpper() != ".XLS")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select an excel file (.xls/.xlsx) only.');", true);
                return;
            }
            if (UploadFile())
            {
                MyExcel oExcel = new MyExcel();
                dtFileData = oExcel.ReadExcel(strFilePath);
                Session["ALLOCATE"] = dtFileData;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Enter Submit to save assets allocation.');", true);
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
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Browse & select excel file.');", true);
            return;
        }
    }

    /// <summary>
    /// Check file extension and saves file to local path.
    /// </summary>
    /// <returns></returns>
    private bool UploadFile()
    {
        String fileExt;
        bool bRes = false;
        if (ddlImportType.SelectedValue.ToString() == "ASSET")
        {
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
        }
        if (ddlImportType.SelectedValue.ToString() == "ALLOCATE")
        {
            fileExt = System.IO.Path.GetExtension(AllocationFileUpload.FileName);
            if (AllocationFileUpload.HasFile)
            {
                if (fileExt == ".xls" || fileExt == ".XLS" || fileExt == ".xlsx")
                {
                    strFilePath = Request.PhysicalApplicationPath + "UploadedFiles\\" + AllocationFileUpload.FileName;
                    File.Delete(strFilePath);
                    AllocationFileUpload.SaveAs(strFilePath);
                    bRes = true;
                }
            }
        }
        if (ddlImportType.SelectedValue.ToString() == "VENDOR")
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
        if (ddlImportType.SelectedValue.ToString() == "EMPLOYEE")
        {
            fileExt = System.IO.Path.GetExtension(EmployeeFileUpload.FileName);
            if (EmployeeFileUpload.HasFile)
            {
                if (fileExt == ".xls" || fileExt == ".XLS" || fileExt == ".xlsx")
                {
                    strFilePath = Request.PhysicalApplicationPath + "UploadedFiles\\" + EmployeeFileUpload.FileName;
                    File.Delete(strFilePath);
                    EmployeeFileUpload.SaveAs(strFilePath);
                    bRes = true;
                }
            }
        }
        return bRes;
    }
    #endregion

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
            catch { } Response.Redirect("Error.aspx");
        }
    }
    #endregion

    #region EXPORT ASSETS FUNCTIONS
    private void PopulateAssetMake(string CategoryCode)
    {
        ddlAssetMake.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateAssetMake(CategoryCode, Session["COMPANY"].ToString());
        ddlAssetMake.DataSource = dt;
        ddlAssetMake.DataTextField = "ASSET_MAKE";
        ddlAssetMake.DataValueField = "ASSET_MAKE";
        ddlAssetMake.DataBind();
        ddlAssetMake.Items.Insert(0, "-- Select Make --");
    }

    /// <summary>
    /// Fetch category details to be populated in dropdownlist.
    /// </summary>
    private void PopulateCategory(string AssetType)
    {
        lblCatCode.Text = "0";
        lblCatLevel.Text = "1";
        ddlAssetCategory.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateCategory(AssetType.Trim(), "", 1);
        ddlAssetCategory.DataSource = dt;
        ddlAssetCategory.DataTextField = "CATEGORY_NAME";
        ddlAssetCategory.DataValueField = "CATEGORY_CODE";
        ddlAssetCategory.DataBind();
        ddlAssetCategory.Items.Insert(0, "-- Select Category --");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AssetMake"></param>
    private void PopulateModelName(string AssetMake, string CategoryCode)
    {
        lstModelName.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.PopulateModelName(AssetMake, CategoryCode, Session["COMPANY"].ToString());
        lstModelName.DataSource = dt;
        lstModelName.DataTextField = "MODEL_NAME";
        lstModelName.DataValueField = "MODEL_NAME";
        lstModelName.DataBind();
        lstModelName.Items.Insert(0, "-- Select Model --");
    }
    #endregion

    #region SELECTED INDEX CHANGED EVENTS
    /// <summary>
    /// Fetch list of categories based on asset type selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetType2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtNull = new DataTable();
            ddlAssetCategory.DataSource = dtNull;
            ddlAssetCategory.DataBind();
            ddlAssetMake.DataSource = dtNull;
            ddlAssetMake.DataBind();
            lstModelName.DataSource = dtNull;
            lstModelName.DataBind();
            dtNull = null;

            if (ddlAssetType2.SelectedValue.ToString() == "ADMIN")
            {
                lblAssetType.Text = "ADMIN";
                PopulateCategory(lblAssetType.Text);
            }
            else if (ddlAssetType2.SelectedValue.ToString() == "IT")
            {
                lblAssetType.Text = "IT";
                PopulateCategory(lblAssetType.Text);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetMake.SelectedIndex != 0)
                PopulateModelName(ddlAssetMake.SelectedValue.ToString(), lblCatCode.Text.Trim());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetCategory.SelectedIndex > 0)
            {
                DataTable dtNull = new DataTable();
                lstModelName.DataSource = dtNull;
                lstModelName.DataBind();
                dtNull = null;

                PopulateAssetMake(ddlAssetCategory.SelectedValue.ToString());
                int CatLevel = int.Parse(lblCatLevel.Text.Trim());
                lblCatLevel.Text = (CatLevel + 1).ToString();
                int iCatLevel = int.Parse(lblCatLevel.Text.Trim());
                string sCatCode = ddlAssetCategory.SelectedValue.ToString();
                lblCatCode.Text = sCatCode;

                ddlAssetCategory.DataSource = null;
                DataTable dt = oDAL.PopulateCategory(lblAssetType.Text, sCatCode, iCatLevel);
                if (dt.Rows.Count > 0)
                {
                    ddlAssetCategory.DataSource = dt;
                    ddlAssetCategory.DataValueField = "CATEGORY_CODE";
                    ddlAssetCategory.DataTextField = "CATEGORY_NAME";
                    ddlAssetCategory.DataBind();
                    ddlAssetCategory.Items.Insert(0, "-- Select Category --");
                    ddlAssetCategory.Focus();
                }
                else
                {
                    iCatLevel = iCatLevel - 1;
                    lblCatLevel.Text = iCatLevel.ToString();
                    ddlAssetMake.Focus();
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion


    private bool Validate(DataTable dtFileData, int iCnt)
    {
        bool bInValid = default(bool);

        oPRP.AssetLife = dtFileData.Rows[iCnt][49].ToString().Trim();
        oPRP.CompanyCost = dtFileData.Rows[iCnt][50].ToString().Trim();
        oPRP.IncomeTaxCost = dtFileData.Rows[iCnt][51].ToString().Trim();
        oPRP.AssetQty = dtFileData.Rows[iCnt][52].ToString().Trim();


        decimal dd = 0;

        if (string.IsNullOrEmpty(oPRP.AssetLife))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Life should not be blank at row no. " + (iCnt + 1).ToString() + ".');", true);
            bInValid = true;

        }
        if (!string.IsNullOrEmpty(oPRP.AssetLife) && clsGeneral.ToDate(oPRP.AssetLife) == default(DateTime?))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset Life should be  valid date format (dd/mmm/yyyy) blank at row no. " + (iCnt + 1).ToString() + ".');", true);
            bInValid = true;

        }

        if (string.IsNullOrEmpty(oPRP.CompanyCost) || !decimal.TryParse(oPRP.CompanyCost, out dd))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Company cost should not be blank and only numeric value at row no. " + (iCnt + 1).ToString() + ".');", true);
            bInValid = true;

        }

        if (string.IsNullOrEmpty(oPRP.IncomeTaxCost) || !decimal.TryParse(oPRP.CompanyCost, out dd))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Incometax cost should not be blank and numeric value invalid at row no. " + (iCnt + 1).ToString() + ".');", true);
            bInValid = true;

        }
        if (string.IsNullOrEmpty(oPRP.AssetQty) || !decimal.TryParse(oPRP.AssetQty, out dd))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Assset Qty should not be blank and numeric value  at row no. " + (iCnt + 1).ToString() + ".');", true);
            bInValid = true;

        }

        return bInValid;

    }


}