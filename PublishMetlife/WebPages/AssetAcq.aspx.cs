using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPages_AssetAcq : System.Web.UI.Page
{

    #region PAGE CONSTRUCTOR & DECLARATIONS

    AssetAcq_DAL oDAL;
    AssetAcquisition_PRP oPRP;
    public WebPages_AssetAcq()
    {
        oPRP = new AssetAcquisition_PRP();
    }
    ~WebPages_AssetAcq()
    {
        oPRP = null; oDAL = null;
    }

    #endregion

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
        oDAL = new AssetAcq_DAL(Session["DATABASE"].ToString());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
        ScriptManager.GetCurrent(this).RegisterPostBackControl(this.btnSubmit);
        this.Page.Form.Attributes.Add("enctype", "multipart/form-data");
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
                GetDropdownDetails();
                GetSubStatusDropDown();
                btnExport.Visible = false;
                btnExport.Enabled = false;
                txtAssetCode.Enabled = true;
                txtSerialNumber.Enabled = true;
                txtRFIDTag.Enabled = true;
                btnSubmit.Text = "Save";
                if (!string.IsNullOrEmpty(Request.QueryString["AssetSerialNo"]))
                {
                    GetAssetSerialDetails(Convert.ToString(Request.QueryString["AssetSerialNo"]));
                }

            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    #endregion

    #region PRIVATE FUNCTIONS

    /// <summary>
    /// Catch unhandled exceptions.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Asset Acquisition");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }

    /// <summary>
    /// Get Dropdown details to be populated into dropdown.
    /// </summary>
    private void GetDropdownDetails()
    {
        DataTable dt = new DataTable();
        ddlVendor.DataSource = null;
        dt = oDAL.GetVendorforFacilities(Session["COMPANY"].ToString());
        ddlVendor.DataSource = dt;
        ddlVendor.DataTextField = "VENDOR_CODE";
        ddlVendor.DataValueField = "VENDOR_CODE";
        ddlVendor.DataBind();
        ddlVendor.Items.Insert(0, "-- Select Vendor --");

        ddlStore.DataSource = null;
        ddlStore.DataBind();
        ddlStore.Items.Insert(0, "-- Select Store --");

        ddlFloor.DataSource = null;
        ddlFloor.DataBind();
        ddlFloor.Items.Insert(0, "-- Select Floor --");

        ddlLocation.DataSource = null;
        dt = oDAL.GetLocation(Session["COMPANY"].ToString());
        ddlLocation.DataSource = dt;
        ddlLocation.DataTextField = "SITE_CODE";
        ddlLocation.DataValueField = "SITE_CODE";
        ddlLocation.DataBind();
        ddlLocation.Items.Insert(0, "-- Select Location --");

    }
    private void GetSubStatusDropDown()
    {
        DataTable dt = new DataTable();
        ddlSubStatus.DataSource = null;
        dt = oDAL.GetSubStatus();
        ddlSubStatus.DataSource = dt;
        ddlSubStatus.DataTextField = "SUB_STATUS";
        ddlSubStatus.DataValueField = "SUB_STATUS";
        ddlSubStatus.DataBind();
        //ddlSubStatus.Items.Insert(0, "-- Select Asset Domain --");
    }
    private void GetStoreDropdown(string SiteCode,string FloorCode)
    {
        DataTable dt = new DataTable();
        ddlStore.DataSource = null;
        dt = oDAL.GetStore(SiteCode,FloorCode,Session["COMPANY"].ToString());
        ddlStore.DataSource = dt;
        ddlStore.DataTextField = "STORE_CODE";
        ddlStore.DataValueField = "STORE_CODE";
        ddlStore.DataBind();
        ddlStore.Items.Insert(0, "-- Select Store --");
    }
    private void GetFloorDropdown(string SiteCode)
    {
        DataTable dt = new DataTable();
        ddlFloor.DataSource = null;
        dt = oDAL.GetFloor(SiteCode,Session["COMPANY"].ToString());
        ddlFloor.DataSource = dt;
        ddlFloor.DataTextField = "FLOOR_CODE";
        ddlFloor.DataValueField = "FLOOR_CODE";
        ddlFloor.DataBind();
        ddlFloor.Items.Insert(0, "-- Select Floor --");
    }
    private void GetAssetDetails(string AssetCode)
    {

        if (oDAL.GetCompCodetoVerify(AssetCode, "ASSET_CODE").ToUpper() == "IT")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Asset Code is Exist in IT.');", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            return;
        }

        DataTable dt = oDAL.GetAssetDetails(AssetCode);
        if (dt.Rows.Count > 0)
        {
            var dtSubStatus = oDAL.GetSubStatus(dt.Rows[0].Field<string>("ASSET_SUB_STATUS"));
            if (dtSubStatus.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Current Asset Sub Status is  " + dt.Rows[0].Field<string>("ASSET_SUB_STATUS") + " .');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                return;
            }

            if (dt.Rows[0].Field<string>("STATUS") == "SCRAP")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Current Asset Status is  " + dt.Rows[0].Field<string>("STATUS") + " .');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                //txtSerialNumber.Text = string.Empty;
                return;
            }

            if (dt.Rows[0].Field<string>("STATUS") == "ALLOCATED" || dt.Rows[0].Field<string>("STATUS") == "IN TRANSIT")
            {
                ddlLocation.Enabled = ddlFloor.Enabled = ddlStore.Enabled = ddlSubStatus.Enabled = false;
            }
            else
            {
                ddlLocation.Enabled = ddlFloor.Enabled = ddlStore.Enabled = ddlSubStatus.Enabled = true;
            }

            GetDropdownDetails();
            if (dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == null || dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == string.Empty || dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == "")
                ddlSubStatus.SelectedIndex = 0;
            else
                ddlSubStatus.SelectedValue = oDAL.GetSubStatusCodeFilterforAcqusition(dt.Rows[0].Field<string>("ASSET_SUB_STATUS"), Session["COMPANY"].ToString());

            txtAssetCode.Text = (dt.Rows[0].Field<string>("ASSET_CODE") == null || dt.Rows[0].Field<string>("ASSET_CODE") == string.Empty || dt.Rows[0].Field<string>("ASSET_CODE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_CODE").Trim();
            txtAssetCode.Enabled = false;
            txtSerialNumber.Enabled = false;
            txtAssetFARTag.Enabled = txtRFIDTag.Enabled = true;
            //txtAssetFARTag.Enabled = false;
            //txtRFIDTag.Enabled = false;
            txtAssetType.Text = (dt.Rows[0].Field<string>("ASSET_TYPE") == null || dt.Rows[0].Field<string>("ASSET_TYPE") == string.Empty || dt.Rows[0].Field<string>("ASSET_TYPE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_TYPE").Trim();
            txtAssetMake.Text = (dt.Rows[0].Field<string>("ASSET_MAKE") == null || dt.Rows[0].Field<string>("ASSET_MAKE") == string.Empty || dt.Rows[0].Field<string>("ASSET_MAKE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_MAKE").Trim();
            txtAssetModel.Text = (dt.Rows[0].Field<string>("MODEL_NAME") == null || dt.Rows[0].Field<string>("MODEL_NAME") == string.Empty || dt.Rows[0].Field<string>("MODEL_NAME") == "") ? string.Empty : dt.Rows[0].Field<string>("MODEL_NAME").Trim();

            if (dt.Rows[0].Field<string>("SITE_CODE") == null || dt.Rows[0].Field<string>("SITE_CODE") == string.Empty || dt.Rows[0].Field<string>("SITE_CODE") == "")
                ddlLocation.SelectedIndex = 0;
            else
                ddlLocation.SelectedValue = oDAL.GetSiteCodeFilterforAcqusition(dt.Rows[0].Field<string>("SITE_CODE"), Session["COMPANY"].ToString());

            GetFloorDropdown(ddlLocation.SelectedValue);
            if (dt.Rows[0].Field<string>("FLOOR") == null || dt.Rows[0].Field<string>("FLOOR") == string.Empty || dt.Rows[0].Field<string>("FLOOR") == "")
                ddlFloor.SelectedIndex = 0;
            else
                ddlFloor.SelectedValue = oDAL.GetFloorCodeFilterforAcqusition(dt.Rows[0].Field<string>("FLOOR"), Session["COMPANY"].ToString());

            GetStoreDropdown(ddlLocation.SelectedValue, ddlFloor.SelectedValue);
            if (dt.Rows[0].Field<string>("STORE") == null || dt.Rows[0].Field<string>("STORE") == string.Empty || dt.Rows[0].Field<string>("STORE") == "")
                ddlStore.SelectedIndex = 0;
            else
                ddlStore.SelectedValue = oDAL.GetStoreCodeFilterforAcqusition(dt.Rows[0].Field<string>("STORE"), Session["COMPANY"].ToString());

            txtSerialNumber.Text = (dt.Rows[0].Field<string>("SERIAL_CODE") == null || dt.Rows[0].Field<string>("SERIAL_CODE") == string.Empty || dt.Rows[0].Field<string>("SERIAL_CODE") == "") ? string.Empty : dt.Rows[0].Field<string>("SERIAL_CODE").Trim();
            txtAssetTag.Text = (dt.Rows[0].Field<string>("ASSET_ID") == null || dt.Rows[0].Field<string>("ASSET_ID") == string.Empty || dt.Rows[0].Field<string>("ASSET_ID") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_ID").Trim();
            txtRFIDTag.Text = (dt.Rows[0].Field<string>("TAG_ID") == null || dt.Rows[0].Field<string>("TAG_ID") == string.Empty || dt.Rows[0].Field<string>("TAG_ID") == "") ? string.Empty : dt.Rows[0].Field<string>("TAG_ID").Trim();

            txtSubCategory.Text = (dt.Rows[0].Field<string>("SUB_CATEGORY") == null || dt.Rows[0].Field<string>("SUB_CATEGORY") == string.Empty || dt.Rows[0].Field<string>("SUB_CATEGORY") == "") ? string.Empty : dt.Rows[0].Field<string>("SUB_CATEGORY").Trim();
            txtProcessor.Text = (dt.Rows[0].Field<string>("ASSET_PROCESSOR") == null || dt.Rows[0].Field<string>("ASSET_PROCESSOR") == string.Empty || dt.Rows[0].Field<string>("ASSET_PROCESSOR") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_PROCESSOR").Trim();
            txtRam.Text = (dt.Rows[0].Field<string>("ASSET_RAM") == null || dt.Rows[0].Field<string>("ASSET_RAM") == string.Empty || dt.Rows[0].Field<string>("ASSET_RAM") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_RAM").Trim();
            txtHHD.Text = (dt.Rows[0].Field<string>("ASSET_HDD") == null || dt.Rows[0].Field<string>("ASSET_HDD") == string.Empty || dt.Rows[0].Field<string>("ASSET_HDD") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_HDD").Trim();

            if (dt.Rows[0].Field<string>("VENDOR_CODE") != "" || dt.Rows[0].Field<string>("VENDOR_CODE") != null || dt.Rows[0].Field<string>("VENDOR_CODE") != string.Empty)
            {
                ddlVendor.SelectedValue = oDAL.GetVendorCodeFilterforAcqusition(dt.Rows[0].Field<string>("VENDOR_CODE"), Session["COMPANY"].ToString());
            }
            txtGRNNo.Text = (dt.Rows[0].Field<string>("GRN_NO") == null || dt.Rows[0].Field<string>("GRN_NO") == string.Empty || dt.Rows[0].Field<string>("GRN_NO") == "") ? string.Empty : dt.Rows[0].Field<string>("GRN_NO").Trim();
            txtWarrantyStatus.Text = (dt.Rows[0].Field<string>("AMC_WARRANTY") == null || dt.Rows[0].Field<string>("AMC_WARRANTY") == string.Empty || dt.Rows[0].Field<string>("AMC_WARRANTY") == "") ? string.Empty : dt.Rows[0].Field<string>("AMC_WARRANTY").Trim();
            if (dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != "" || dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != null || dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != string.Empty)
            {
                ddlProcurementBudget.SelectedValue = dt.Rows[0].Field<string>("PROCUREMENT_BUDGET");
            }
            txtWarrantyStartDate.Text = dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_START_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_START_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtWarrantyEndDate.Text = dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_END_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_END_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtPONo.Text = (dt.Rows[0].Field<string>("PO_NUMBER") == null || dt.Rows[0].Field<string>("PO_NUMBER") == string.Empty || dt.Rows[0].Field<string>("PO_NUMBER") == "") ? string.Empty : dt.Rows[0].Field<string>("PO_NUMBER").Trim();
            txtPODate.Text = dt.Rows[0].Field<DateTime?>("PO_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("PO_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtInvoiceNo.Text = (dt.Rows[0].Field<string>("INVOICE_NO") == null || dt.Rows[0].Field<string>("INVOICE_NO") == string.Empty || dt.Rows[0].Field<string>("INVOICE_NO") == "") ? string.Empty : dt.Rows[0].Field<string>("INVOICE_NO").Trim();
            txtInvoiceDate.Text = dt.Rows[0].Field<DateTime?>("INVOICE_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("INVOICE_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtRemarks.Text = (dt.Rows[0].Field<string>("REMARKS") == null || dt.Rows[0].Field<string>("REMARKS") == string.Empty || dt.Rows[0].Field<string>("REMARKS") == "") ? string.Empty : dt.Rows[0].Field<string>("REMARKS").Trim();
            txtAssetFARTag.Text = dt.Rows[0].Field<string>("ASSET_FAR_TAG");
            txtIdentifierLocation.Text = (dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == null || dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == string.Empty || dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == "") ? string.Empty : dt.Rows[0].Field<string>("IDENTIFIER_LOCATION").Trim();
            txtInServiceDate.Text = dt.Rows[0].Field<DateTime?>("SERVICE_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("SERVICE_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtLifeCycle.Text = (dt.Rows[0].Field<string>("ASSET_LIFE") == null || dt.Rows[0].Field<string>("ASSET_LIFE") == string.Empty || dt.Rows[0].Field<string>("ASSET_LIFE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_LIFE").Trim();
            txtAssetEndLife.Text = (dt.Rows[0].Field<string>("ASSET_END_LIFE") == null || dt.Rows[0].Field<string>("ASSET_END_LIFE") == string.Empty || dt.Rows[0].Field<string>("ASSET_END_LIFE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_END_LIFE").Trim();

            btnSubmit.Text = "Update";
        }
        else
        {
            txtRFIDTag.Text = "";
            btnSubmit.Text = "Save";
        }
    }
    private void GetAssetSerialDetails(string AssetSerialNo)
    {
        if (oDAL.GetCompCodetoVerify(AssetSerialNo, "SERIAL_CODE").ToUpper() == "IT")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('This Asset Serial Number is Exist in IT.');", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            return;
        }

        DataTable dt = oDAL.GetAssetSerialDetails(AssetSerialNo);
        if (dt.Rows.Count > 0)
        {
            var dtSubStatus = oDAL.GetSubStatus(dt.Rows[0].Field<string>("ASSET_SUB_STATUS"));
            if (dtSubStatus.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Current Asset Sub Status is  " + dt.Rows[0].Field<string>("ASSET_SUB_STATUS") + " .');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                return;
            }

            if (dt.Rows[0].Field<string>("STATUS") == "SCRAP")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Current Asset Status is  " + dt.Rows[0].Field<string>("STATUS") + " .');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                //txtSerialNumber.Text = string.Empty;
                return;
            }

            if (dt.Rows[0].Field<string>("STATUS") == "ALLOCATED" || dt.Rows[0].Field<string>("STATUS") == "IN TRANSIT")
            {
                ddlLocation.Enabled = ddlFloor.Enabled = ddlStore.Enabled = ddlSubStatus.Enabled = false;
            }
            else
            {
                ddlLocation.Enabled = ddlFloor.Enabled = ddlStore.Enabled = ddlSubStatus.Enabled = true;
            }

            GetDropdownDetails();
            if (dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == null || dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == string.Empty || dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == "")
                ddlSubStatus.SelectedIndex = 0;
            else
                ddlSubStatus.SelectedValue = oDAL.GetSubStatusCodeFilterforAcqusition(dt.Rows[0].Field<string>("ASSET_SUB_STATUS"), Session["COMPANY"].ToString());

            txtAssetCode.Text = (dt.Rows[0].Field<string>("ASSET_CODE") == null || dt.Rows[0].Field<string>("ASSET_CODE") == string.Empty || dt.Rows[0].Field<string>("ASSET_CODE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_CODE").Trim();
            txtAssetCode.Enabled = false;
            txtSerialNumber.Enabled = false;
            //txtRFIDTag.Enabled = false;
            txtAssetFARTag.Enabled = txtRFIDTag.Enabled=txtAssetTag.Enabled=true;
            txtAssetType.Text = (dt.Rows[0].Field<string>("ASSET_TYPE") == null || dt.Rows[0].Field<string>("ASSET_TYPE") == string.Empty || dt.Rows[0].Field<string>("ASSET_TYPE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_TYPE").Trim();
            txtAssetMake.Text = (dt.Rows[0].Field<string>("ASSET_MAKE") == null || dt.Rows[0].Field<string>("ASSET_MAKE") == string.Empty || dt.Rows[0].Field<string>("ASSET_MAKE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_MAKE").Trim();
            txtAssetModel.Text = (dt.Rows[0].Field<string>("MODEL_NAME") == null || dt.Rows[0].Field<string>("MODEL_NAME") == string.Empty || dt.Rows[0].Field<string>("MODEL_NAME") == "") ? string.Empty : dt.Rows[0].Field<string>("MODEL_NAME").Trim();

            if (dt.Rows[0].Field<string>("SITE_CODE") == null || dt.Rows[0].Field<string>("SITE_CODE") == string.Empty || dt.Rows[0].Field<string>("SITE_CODE") == "")
                ddlLocation.SelectedIndex = 0;
            else
                ddlLocation.SelectedValue = oDAL.GetSiteCodeFilterforAcqusition(dt.Rows[0].Field<string>("SITE_CODE"), Session["COMPANY"].ToString());

            GetFloorDropdown(ddlLocation.SelectedValue);
            if (dt.Rows[0].Field<string>("FLOOR") == null || dt.Rows[0].Field<string>("FLOOR") == string.Empty || dt.Rows[0].Field<string>("FLOOR") == "")
                ddlFloor.SelectedIndex = 0;
            else
                ddlFloor.SelectedValue = oDAL.GetFloorCodeFilterforAcqusition(dt.Rows[0].Field<string>("FLOOR"), Session["COMPANY"].ToString());

            GetStoreDropdown(ddlLocation.SelectedValue, ddlFloor.SelectedValue);
            if (dt.Rows[0].Field<string>("STORE") == null || dt.Rows[0].Field<string>("STORE") == string.Empty || dt.Rows[0].Field<string>("STORE") == "")
                ddlStore.SelectedIndex = 0;
            else
                ddlStore.SelectedValue = oDAL.GetStoreCodeFilterforAcqusition(dt.Rows[0].Field<string>("STORE"), Session["COMPANY"].ToString());

            txtSerialNumber.Text = (dt.Rows[0].Field<string>("SERIAL_CODE") == null || dt.Rows[0].Field<string>("SERIAL_CODE") == string.Empty || dt.Rows[0].Field<string>("SERIAL_CODE") == "") ? string.Empty : dt.Rows[0].Field<string>("SERIAL_CODE").Trim();
            txtAssetTag.Text = (dt.Rows[0].Field<string>("ASSET_ID") == null || dt.Rows[0].Field<string>("ASSET_ID") == string.Empty || dt.Rows[0].Field<string>("ASSET_ID") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_ID").Trim();
            txtRFIDTag.Text = (dt.Rows[0].Field<string>("TAG_ID") == null || dt.Rows[0].Field<string>("TAG_ID") == string.Empty || dt.Rows[0].Field<string>("TAG_ID") == "") ? string.Empty : dt.Rows[0].Field<string>("TAG_ID").Trim();

            txtSubCategory.Text = (dt.Rows[0].Field<string>("SUB_CATEGORY") == null || dt.Rows[0].Field<string>("SUB_CATEGORY") == string.Empty || dt.Rows[0].Field<string>("SUB_CATEGORY") == "") ? string.Empty : dt.Rows[0].Field<string>("SUB_CATEGORY").Trim();
            txtProcessor.Text = (dt.Rows[0].Field<string>("ASSET_PROCESSOR") == null || dt.Rows[0].Field<string>("ASSET_PROCESSOR") == string.Empty || dt.Rows[0].Field<string>("ASSET_PROCESSOR") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_PROCESSOR").Trim();
            txtRam.Text = (dt.Rows[0].Field<string>("ASSET_RAM") == null || dt.Rows[0].Field<string>("ASSET_RAM") == string.Empty || dt.Rows[0].Field<string>("ASSET_RAM") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_RAM").Trim();
            txtHHD.Text = (dt.Rows[0].Field<string>("ASSET_HDD") == null || dt.Rows[0].Field<string>("ASSET_HDD") == string.Empty || dt.Rows[0].Field<string>("ASSET_HDD") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_HDD").Trim();

            if (dt.Rows[0].Field<string>("VENDOR_CODE") != "" || dt.Rows[0].Field<string>("VENDOR_CODE") != null || dt.Rows[0].Field<string>("VENDOR_CODE") != string.Empty)
            {
                ddlVendor.SelectedValue = oDAL.GetVendorCodeFilterforAcqusition(dt.Rows[0].Field<string>("VENDOR_CODE"), Session["COMPANY"].ToString());
            }
            txtGRNNo.Text = (dt.Rows[0].Field<string>("GRN_NO") == null || dt.Rows[0].Field<string>("GRN_NO") == string.Empty || dt.Rows[0].Field<string>("GRN_NO") == "") ? string.Empty : dt.Rows[0].Field<string>("GRN_NO").Trim();
            txtWarrantyStatus.Text = (dt.Rows[0].Field<string>("AMC_WARRANTY") == null || dt.Rows[0].Field<string>("AMC_WARRANTY") == string.Empty || dt.Rows[0].Field<string>("AMC_WARRANTY") == "") ? string.Empty : dt.Rows[0].Field<string>("AMC_WARRANTY").Trim();
            if (dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != "" || dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != null || dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != string.Empty)
            {
                ddlProcurementBudget.SelectedValue = dt.Rows[0].Field<string>("PROCUREMENT_BUDGET");
            }
            txtWarrantyStartDate.Text = dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_START_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_START_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtWarrantyEndDate.Text = dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_END_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_END_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtPONo.Text = (dt.Rows[0].Field<string>("PO_NUMBER") == null || dt.Rows[0].Field<string>("PO_NUMBER") == string.Empty || dt.Rows[0].Field<string>("PO_NUMBER") == "") ? string.Empty : dt.Rows[0].Field<string>("PO_NUMBER").Trim();
            txtPODate.Text = dt.Rows[0].Field<DateTime?>("PO_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("PO_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtInvoiceNo.Text = (dt.Rows[0].Field<string>("INVOICE_NO") == null || dt.Rows[0].Field<string>("INVOICE_NO") == string.Empty || dt.Rows[0].Field<string>("INVOICE_NO") == "") ? string.Empty : dt.Rows[0].Field<string>("INVOICE_NO").Trim();
            txtInvoiceDate.Text = dt.Rows[0].Field<DateTime?>("INVOICE_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("INVOICE_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtRemarks.Text = (dt.Rows[0].Field<string>("REMARKS") == null || dt.Rows[0].Field<string>("REMARKS") == string.Empty || dt.Rows[0].Field<string>("REMARKS") == "") ? string.Empty : dt.Rows[0].Field<string>("REMARKS").Trim();
            txtAssetFARTag.Text = dt.Rows[0].Field<string>("ASSET_FAR_TAG");
            txtIdentifierLocation.Text = (dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == null || dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == string.Empty || dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == "") ? string.Empty : dt.Rows[0].Field<string>("IDENTIFIER_LOCATION").Trim();
            txtInServiceDate.Text = dt.Rows[0].Field<DateTime?>("SERVICE_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("SERVICE_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtLifeCycle.Text = (dt.Rows[0].Field<string>("ASSET_LIFE") == null || dt.Rows[0].Field<string>("ASSET_LIFE") == string.Empty || dt.Rows[0].Field<string>("ASSET_LIFE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_LIFE").Trim();
            txtAssetEndLife.Text = (dt.Rows[0].Field<string>("ASSET_END_LIFE") == null || dt.Rows[0].Field<string>("ASSET_END_LIFE") == string.Empty || dt.Rows[0].Field<string>("ASSET_END_LIFE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_END_LIFE").Trim();

            btnSubmit.Text = "Update";
        }
        else
        {
            txtRFIDTag.Text = "";
            btnSubmit.Text = "Save";
        }
    }
    private void GetAssetRFIDTagDetails(string RFIDTag)
    {
        if (oDAL.GetCompCodetoVerify(RFIDTag, "RFID_TAG").ToUpper() == "IT")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('This Asset RFID Tag is Exist in IT.');", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            return;
        }

        DataTable dt = oDAL.GetRFIDTagDetails(RFIDTag);
        if (dt.Rows.Count > 0)
        {
            var dtSubStatus = oDAL.GetSubStatus(dt.Rows[0].Field<string>("ASSET_SUB_STATUS"));
            if (dtSubStatus.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Current Asset Sub Status is  " + dt.Rows[0].Field<string>("ASSET_SUB_STATUS") + " .');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                return;
            }

            if (dt.Rows[0].Field<string>("STATUS") == "SCRAP")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Current Asset Status is  " + dt.Rows[0].Field<string>("STATUS") + " .');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                //txtSerialNumber.Text = string.Empty;
                return;
            }

            if (dt.Rows[0].Field<string>("STATUS") == "ALLOCATED" || dt.Rows[0].Field<string>("STATUS") == "IN TRANSIT")
            {
                ddlLocation.Enabled = ddlFloor.Enabled = ddlStore.Enabled = ddlSubStatus.Enabled = false;
            }
            else
            {
                ddlLocation.Enabled = ddlFloor.Enabled = ddlStore.Enabled = ddlSubStatus.Enabled = true;
            }

            GetDropdownDetails();
            if (dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == null || dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == string.Empty || dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == "")
                ddlSubStatus.SelectedIndex = 0;
            else
                ddlSubStatus.SelectedValue = oDAL.GetSubStatusCodeFilterforAcqusition(dt.Rows[0].Field<string>("ASSET_SUB_STATUS"), Session["COMPANY"].ToString());

            txtAssetCode.Text = (dt.Rows[0].Field<string>("ASSET_CODE") == null || dt.Rows[0].Field<string>("ASSET_CODE") == string.Empty || dt.Rows[0].Field<string>("ASSET_CODE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_CODE").Trim();
            txtAssetCode.Enabled = false;
            txtSerialNumber.Enabled = false;
            //txtRFIDTag.Enabled = false;
            txtRFIDTag.Enabled = false;
            txtAssetFARTag.Enabled = false;
            txtAssetTag.Enabled = true;
            txtAssetType.Text = (dt.Rows[0].Field<string>("ASSET_TYPE") == null || dt.Rows[0].Field<string>("ASSET_TYPE") == string.Empty || dt.Rows[0].Field<string>("ASSET_TYPE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_TYPE").Trim();
            txtAssetMake.Text = (dt.Rows[0].Field<string>("ASSET_MAKE") == null || dt.Rows[0].Field<string>("ASSET_MAKE") == string.Empty || dt.Rows[0].Field<string>("ASSET_MAKE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_MAKE").Trim();
            txtAssetModel.Text = (dt.Rows[0].Field<string>("MODEL_NAME") == null || dt.Rows[0].Field<string>("MODEL_NAME") == string.Empty || dt.Rows[0].Field<string>("MODEL_NAME") == "") ? string.Empty : dt.Rows[0].Field<string>("MODEL_NAME").Trim();

            if (dt.Rows[0].Field<string>("SITE_CODE") == null || dt.Rows[0].Field<string>("SITE_CODE") == string.Empty || dt.Rows[0].Field<string>("SITE_CODE") == "")
                ddlLocation.SelectedIndex = 0;
            else
                ddlLocation.SelectedValue = oDAL.GetSiteCodeFilterforAcqusition(dt.Rows[0].Field<string>("SITE_CODE"), Session["COMPANY"].ToString());

            GetFloorDropdown(ddlLocation.SelectedValue);
            if (dt.Rows[0].Field<string>("FLOOR") == null || dt.Rows[0].Field<string>("FLOOR") == string.Empty || dt.Rows[0].Field<string>("FLOOR") == "")
                ddlFloor.SelectedIndex = 0;
            else
                ddlFloor.SelectedValue = oDAL.GetFloorCodeFilterforAcqusition(dt.Rows[0].Field<string>("FLOOR"), Session["COMPANY"].ToString());

            GetStoreDropdown(ddlLocation.SelectedValue, ddlFloor.SelectedValue);
            if (dt.Rows[0].Field<string>("STORE") == null || dt.Rows[0].Field<string>("STORE") == string.Empty || dt.Rows[0].Field<string>("STORE") == "")
                ddlStore.SelectedIndex = 0;
            else
                ddlStore.SelectedValue = oDAL.GetStoreCodeFilterforAcqusition(dt.Rows[0].Field<string>("STORE"), Session["COMPANY"].ToString());

            txtSerialNumber.Text = (dt.Rows[0].Field<string>("SERIAL_CODE") == null || dt.Rows[0].Field<string>("SERIAL_CODE") == string.Empty || dt.Rows[0].Field<string>("SERIAL_CODE") == "") ? string.Empty : dt.Rows[0].Field<string>("SERIAL_CODE").Trim();
            txtAssetTag.Text = (dt.Rows[0].Field<string>("ASSET_ID") == null || dt.Rows[0].Field<string>("ASSET_ID") == string.Empty || dt.Rows[0].Field<string>("ASSET_ID") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_ID").Trim();
            txtRFIDTag.Text = (dt.Rows[0].Field<string>("TAG_ID") == null || dt.Rows[0].Field<string>("TAG_ID") == string.Empty || dt.Rows[0].Field<string>("TAG_ID") == "") ? string.Empty : dt.Rows[0].Field<string>("TAG_ID").Trim();

            txtSubCategory.Text = (dt.Rows[0].Field<string>("SUB_CATEGORY") == null || dt.Rows[0].Field<string>("SUB_CATEGORY") == string.Empty || dt.Rows[0].Field<string>("SUB_CATEGORY") == "") ? string.Empty : dt.Rows[0].Field<string>("SUB_CATEGORY").Trim();
            txtProcessor.Text = (dt.Rows[0].Field<string>("ASSET_PROCESSOR") == null || dt.Rows[0].Field<string>("ASSET_PROCESSOR") == string.Empty || dt.Rows[0].Field<string>("ASSET_PROCESSOR") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_PROCESSOR").Trim();
            txtRam.Text = (dt.Rows[0].Field<string>("ASSET_RAM") == null || dt.Rows[0].Field<string>("ASSET_RAM") == string.Empty || dt.Rows[0].Field<string>("ASSET_RAM") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_RAM").Trim();
            txtHHD.Text = (dt.Rows[0].Field<string>("ASSET_HDD") == null || dt.Rows[0].Field<string>("ASSET_HDD") == string.Empty || dt.Rows[0].Field<string>("ASSET_HDD") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_HDD").Trim();

            if (dt.Rows[0].Field<string>("VENDOR_CODE") != "" || dt.Rows[0].Field<string>("VENDOR_CODE") != null || dt.Rows[0].Field<string>("VENDOR_CODE") != string.Empty)
            {
                ddlVendor.SelectedValue = oDAL.GetVendorCodeFilterforAcqusition(dt.Rows[0].Field<string>("VENDOR_CODE"), Session["COMPANY"].ToString());
            }
            txtGRNNo.Text = (dt.Rows[0].Field<string>("GRN_NO") == null || dt.Rows[0].Field<string>("GRN_NO") == string.Empty || dt.Rows[0].Field<string>("GRN_NO") == "") ? string.Empty : dt.Rows[0].Field<string>("GRN_NO").Trim();
            txtWarrantyStatus.Text = (dt.Rows[0].Field<string>("AMC_WARRANTY") == null || dt.Rows[0].Field<string>("AMC_WARRANTY") == string.Empty || dt.Rows[0].Field<string>("AMC_WARRANTY") == "") ? string.Empty : dt.Rows[0].Field<string>("AMC_WARRANTY").Trim();
            if (dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != "" || dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != null || dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != string.Empty)
            {
                ddlProcurementBudget.SelectedValue = dt.Rows[0].Field<string>("PROCUREMENT_BUDGET");
            }
            txtWarrantyStartDate.Text = dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_START_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_START_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtWarrantyEndDate.Text = dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_END_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_END_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtPONo.Text = (dt.Rows[0].Field<string>("PO_NUMBER") == null || dt.Rows[0].Field<string>("PO_NUMBER") == string.Empty || dt.Rows[0].Field<string>("PO_NUMBER") == "") ? string.Empty : dt.Rows[0].Field<string>("PO_NUMBER").Trim();
            txtPODate.Text = dt.Rows[0].Field<DateTime?>("PO_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("PO_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtInvoiceNo.Text = (dt.Rows[0].Field<string>("INVOICE_NO") == null || dt.Rows[0].Field<string>("INVOICE_NO") == string.Empty || dt.Rows[0].Field<string>("INVOICE_NO") == "") ? string.Empty : dt.Rows[0].Field<string>("INVOICE_NO").Trim();
            txtInvoiceDate.Text = dt.Rows[0].Field<DateTime?>("INVOICE_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("INVOICE_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtRemarks.Text = (dt.Rows[0].Field<string>("REMARKS") == null || dt.Rows[0].Field<string>("REMARKS") == string.Empty || dt.Rows[0].Field<string>("REMARKS") == "") ? string.Empty : dt.Rows[0].Field<string>("REMARKS").Trim();
            txtAssetFARTag.Text = dt.Rows[0].Field<string>("ASSET_FAR_TAG");
            txtIdentifierLocation.Text = (dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == null || dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == string.Empty || dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == "") ? string.Empty : dt.Rows[0].Field<string>("IDENTIFIER_LOCATION").Trim();
            txtInServiceDate.Text = dt.Rows[0].Field<DateTime?>("SERVICE_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("SERVICE_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtLifeCycle.Text = (dt.Rows[0].Field<string>("ASSET_LIFE") == null || dt.Rows[0].Field<string>("ASSET_LIFE") == string.Empty || dt.Rows[0].Field<string>("ASSET_LIFE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_LIFE").Trim();
            txtAssetEndLife.Text = (dt.Rows[0].Field<string>("ASSET_END_LIFE") == null || dt.Rows[0].Field<string>("ASSET_END_LIFE") == string.Empty || dt.Rows[0].Field<string>("ASSET_END_LIFE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_END_LIFE").Trim();

            btnSubmit.Text = "Update";
        }
        else
        {
            txtRFIDTag.Text = "";
            btnSubmit.Text = "Save";
        }
    }
    private void GetAssetFarTagDetails(string FarTag)
    {

        if (oDAL.GetCompCodetoVerify(FarTag, "ASSET_FAR_TAG").ToUpper() == "IT")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('This Asset Far Tag is Exist in IT.');", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
            return;
        }

        DataTable dt = oDAL.GetAssetFarTagDetails(FarTag);
        if (dt.Rows.Count > 0)
        {
            var dtSubStatus = oDAL.GetSubStatus(dt.Rows[0].Field<string>("ASSET_SUB_STATUS"));
            if (dtSubStatus.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Current Asset Sub Status is  " + dt.Rows[0].Field<string>("ASSET_SUB_STATUS") + " .');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                return;
            }

            if (dt.Rows[0].Field<string>("STATUS") == "SCRAP")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Current Asset Status is  " + dt.Rows[0].Field<string>("STATUS") + " .');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                //txtSerialNumber.Text = string.Empty;
                return;
            }

            if (dt.Rows[0].Field<string>("STATUS") == "ALLOCATED" || dt.Rows[0].Field<string>("STATUS") == "IN TRANSIT")
            {
                ddlLocation.Enabled = ddlFloor.Enabled = ddlStore.Enabled = ddlSubStatus.Enabled = false;
            }
            else
            {
                ddlLocation.Enabled = ddlFloor.Enabled = ddlStore.Enabled = ddlSubStatus.Enabled = true;
            }

            GetDropdownDetails();
            if (dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == null || dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == string.Empty || dt.Rows[0].Field<string>("ASSET_SUB_STATUS") == "")
                ddlSubStatus.SelectedIndex = 0;
            else
                ddlSubStatus.SelectedValue = oDAL.GetSubStatusCodeFilterforAcqusition(dt.Rows[0].Field<string>("ASSET_SUB_STATUS"),Session["COMPANY"].ToString());

            txtAssetCode.Text = (dt.Rows[0].Field<string>("ASSET_CODE") == null || dt.Rows[0].Field<string>("ASSET_CODE") == string.Empty || dt.Rows[0].Field<string>("ASSET_CODE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_CODE").Trim();
            txtAssetCode.Enabled = false;
            txtSerialNumber.Enabled = false;
            //txtRFIDTag.Enabled = false;
            txtAssetFARTag.Enabled = false;
            txtRFIDTag.Enabled = txtAssetTag.Enabled = true;
            txtAssetType.Text = (dt.Rows[0].Field<string>("ASSET_TYPE") == null || dt.Rows[0].Field<string>("ASSET_TYPE") == string.Empty || dt.Rows[0].Field<string>("ASSET_TYPE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_TYPE").Trim();
            txtAssetMake.Text = (dt.Rows[0].Field<string>("ASSET_MAKE") == null || dt.Rows[0].Field<string>("ASSET_MAKE") == string.Empty || dt.Rows[0].Field<string>("ASSET_MAKE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_MAKE").Trim();
            txtAssetModel.Text = (dt.Rows[0].Field<string>("MODEL_NAME") == null || dt.Rows[0].Field<string>("MODEL_NAME") == string.Empty || dt.Rows[0].Field<string>("MODEL_NAME") == "") ? string.Empty : dt.Rows[0].Field<string>("MODEL_NAME").Trim();
            
            if (dt.Rows[0].Field<string>("SITE_CODE") == null || dt.Rows[0].Field<string>("SITE_CODE") == string.Empty || dt.Rows[0].Field<string>("SITE_CODE") == "")
                ddlLocation.SelectedIndex = 0;
            else
                ddlLocation.SelectedValue = oDAL.GetSiteCodeFilterforAcqusition(dt.Rows[0].Field<string>("SITE_CODE"), Session["COMPANY"].ToString());

            GetFloorDropdown(ddlLocation.SelectedValue);
            if (dt.Rows[0].Field<string>("FLOOR") == null || dt.Rows[0].Field<string>("FLOOR") == string.Empty || dt.Rows[0].Field<string>("FLOOR") == "")
                ddlFloor.SelectedIndex = 0;
            else
                ddlFloor.SelectedValue = oDAL.GetFloorCodeFilterforAcqusition(dt.Rows[0].Field<string>("FLOOR"),Session["COMPANY"].ToString()) ;

            GetStoreDropdown(ddlLocation.SelectedValue, ddlFloor.SelectedValue);
            if (dt.Rows[0].Field<string>("STORE") == null || dt.Rows[0].Field<string>("STORE") == string.Empty || dt.Rows[0].Field<string>("STORE") == "")
                ddlStore.SelectedIndex = 0;
            else
                ddlStore.SelectedValue = oDAL.GetStoreCodeFilterforAcqusition(dt.Rows[0].Field<string>("STORE"), Session["COMPANY"].ToString());

            txtSerialNumber.Text = (dt.Rows[0].Field<string>("SERIAL_CODE") == null || dt.Rows[0].Field<string>("SERIAL_CODE") == string.Empty || dt.Rows[0].Field<string>("SERIAL_CODE") == "") ? string.Empty : dt.Rows[0].Field<string>("SERIAL_CODE").Trim();
            txtAssetTag.Text = (dt.Rows[0].Field<string>("ASSET_ID") == null || dt.Rows[0].Field<string>("ASSET_ID") == string.Empty || dt.Rows[0].Field<string>("ASSET_ID") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_ID").Trim();
            txtRFIDTag.Text = (dt.Rows[0].Field<string>("TAG_ID") == null || dt.Rows[0].Field<string>("TAG_ID") == string.Empty || dt.Rows[0].Field<string>("TAG_ID") == "") ? string.Empty : dt.Rows[0].Field<string>("TAG_ID").Trim();
            
            txtSubCategory.Text = (dt.Rows[0].Field<string>("SUB_CATEGORY") == null || dt.Rows[0].Field<string>("SUB_CATEGORY") == string.Empty || dt.Rows[0].Field<string>("SUB_CATEGORY") == "") ? string.Empty : dt.Rows[0].Field<string>("SUB_CATEGORY").Trim();
            txtProcessor.Text = (dt.Rows[0].Field<string>("ASSET_PROCESSOR") == null || dt.Rows[0].Field<string>("ASSET_PROCESSOR") == string.Empty || dt.Rows[0].Field<string>("ASSET_PROCESSOR") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_PROCESSOR").Trim();
            txtRam.Text = (dt.Rows[0].Field<string>("ASSET_RAM") == null || dt.Rows[0].Field<string>("ASSET_RAM") == string.Empty || dt.Rows[0].Field<string>("ASSET_RAM") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_RAM").Trim();
            txtHHD.Text = (dt.Rows[0].Field<string>("ASSET_HDD") == null || dt.Rows[0].Field<string>("ASSET_HDD") == string.Empty || dt.Rows[0].Field<string>("ASSET_HDD") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_HDD").Trim();
            
            if (dt.Rows[0].Field<string>("VENDOR_CODE") != "" || dt.Rows[0].Field<string>("VENDOR_CODE") != null || dt.Rows[0].Field<string>("VENDOR_CODE") != string.Empty)
            {
                ddlVendor.SelectedValue = oDAL.GetVendorCodeFilterforAcqusition(dt.Rows[0].Field<string>("VENDOR_CODE"),Session["COMPANY"].ToString());
            }
            txtGRNNo.Text = (dt.Rows[0].Field<string>("GRN_NO") == null || dt.Rows[0].Field<string>("GRN_NO") == string.Empty || dt.Rows[0].Field<string>("GRN_NO") == "") ? string.Empty : dt.Rows[0].Field<string>("GRN_NO").Trim();
            txtWarrantyStatus.Text = (dt.Rows[0].Field<string>("AMC_WARRANTY") == null || dt.Rows[0].Field<string>("AMC_WARRANTY") == string.Empty || dt.Rows[0].Field<string>("AMC_WARRANTY") == "") ? string.Empty : dt.Rows[0].Field<string>("AMC_WARRANTY").Trim();
            if (dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != "" || dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != null || dt.Rows[0].Field<string>("PROCUREMENT_BUDGET") != string.Empty)
            {
                ddlProcurementBudget.SelectedValue = dt.Rows[0].Field<string>("PROCUREMENT_BUDGET");
            }
            txtWarrantyStartDate.Text = dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_START_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_START_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtWarrantyEndDate.Text = dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_END_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("AMC_WARRANTY_END_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtPONo.Text = (dt.Rows[0].Field<string>("PO_NUMBER") == null || dt.Rows[0].Field<string>("PO_NUMBER") == string.Empty || dt.Rows[0].Field<string>("PO_NUMBER") == "") ? string.Empty : dt.Rows[0].Field<string>("PO_NUMBER").Trim();
            txtPODate.Text = dt.Rows[0].Field<DateTime?>("PO_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("PO_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtInvoiceNo.Text = (dt.Rows[0].Field<string>("INVOICE_NO") == null || dt.Rows[0].Field<string>("INVOICE_NO") == string.Empty || dt.Rows[0].Field<string>("INVOICE_NO") == "") ? string.Empty : dt.Rows[0].Field<string>("INVOICE_NO").Trim();
            txtInvoiceDate.Text = dt.Rows[0].Field<DateTime?>("INVOICE_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("INVOICE_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtRemarks.Text = (dt.Rows[0].Field<string>("REMARKS") == null || dt.Rows[0].Field<string>("REMARKS") == string.Empty || dt.Rows[0].Field<string>("REMARKS") == "") ? string.Empty : dt.Rows[0].Field<string>("REMARKS").Trim();
            txtAssetFARTag.Text = dt.Rows[0].Field<string>("ASSET_FAR_TAG");
            txtIdentifierLocation.Text = (dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == null || dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == string.Empty || dt.Rows[0].Field<string>("IDENTIFIER_LOCATION") == "") ? string.Empty : dt.Rows[0].Field<string>("IDENTIFIER_LOCATION").Trim();
            txtInServiceDate.Text = dt.Rows[0].Field<DateTime?>("SERVICE_DATE").HasValue ? dt.Rows[0].Field<DateTime?>("SERVICE_DATE").Value.ToString("dd-MMM-yyyy") : null;
            txtLifeCycle.Text = (dt.Rows[0].Field<string>("ASSET_LIFE") == null || dt.Rows[0].Field<string>("ASSET_LIFE") == string.Empty || dt.Rows[0].Field<string>("ASSET_LIFE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_LIFE").Trim();
            txtAssetEndLife.Text = (dt.Rows[0].Field<string>("ASSET_END_LIFE") == null || dt.Rows[0].Field<string>("ASSET_END_LIFE") == string.Empty || dt.Rows[0].Field<string>("ASSET_END_LIFE") == "") ? string.Empty : dt.Rows[0].Field<string>("ASSET_END_LIFE").Trim();
            
            btnSubmit.Text = "Update";
        }
        else
        {
            txtRFIDTag.Text = "";
            btnSubmit.Text = "Save";
        }
    }

    #endregion

    #region BUTTON EVENTS

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtassetFacilities = new DataTable();
            dtassetFacilities.Columns.Add("Asset Code");
            dtassetFacilities.Columns.Add("Site Location");
            dtassetFacilities.Columns.Add("Asset Type");
            dtassetFacilities.Columns.Add("Asset Make");
            dtassetFacilities.Columns.Add("Asset Model");
            dtassetFacilities.Columns.Add("Asset Far Tag");
            dtassetFacilities.Columns.Add("GRN No");
            dtassetFacilities.Columns.Add("GRN Date");
            dtassetFacilities.Columns.Add("OEM Vendor");
            dtassetFacilities.Columns.Add("PO No");
            dtassetFacilities.Columns.Add("PO Date");
            dtassetFacilities.Columns.Add("Invoice No");
            dtassetFacilities.Columns.Add("Invoice Date");

            if (clsGeneral._strRights[1] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            if(Convert.ToString(Session["COMPANY"]) == "Facilities" && txtAssetFARTag.Text =="")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please enter the asset far tag.');", true);
                return;
            }
            if (Convert.ToString(Session["COMPANY"]) == "IT" && txtSerialNumber.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please enter the asset serial number.');", true);
                return;
            }
            if(ddlSubStatus.SelectedValue == "OTHERS")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please enter the Select Sub Status Except Others.');", true);
                return;
            }
            oPRP.AssetCode = txtAssetCode.Text.Trim().ToUpper();
            oPRP.AssetType = txtAssetType.Text.Trim();
            oPRP.AssetMakeName = txtAssetMake.Text.Trim();
            oPRP.AssetModelName = txtAssetModel.Text.Trim();
            oPRP.AssetLocation = (ddlLocation.SelectedIndex>0 ? ddlLocation.SelectedValue:"");
            oPRP.IdentifierLocation = txtIdentifierLocation.Text;
            oPRP.AssetSerialCode = txtSerialNumber.Text.Trim().ToUpper();
            oPRP.AssetRFID = txtRFIDTag.Text.Trim().ToUpper();
            oPRP.AssetTag = txtAssetTag.Text;
            oPRP.store = (ddlStore.SelectedIndex>0 ? ddlStore.SelectedValue: "");
            oPRP.floor = (ddlFloor.SelectedIndex > 0 ? ddlFloor.SelectedValue : "");
            oPRP.SubCategory = txtSubCategory.Text;
            oPRP.AssetProcessor = txtProcessor.Text;
            oPRP.AssetRAM = txtRam.Text;
            oPRP.VendorCode = (ddlVendor.SelectedIndex > 0 ? ddlVendor.SelectedValue : "");
            oPRP.AssetHDD = txtHHD.Text;
            oPRP.GRNNo = txtGRNNo.Text;
            oPRP.AssetDomain = string.Empty;
            oPRP.AMC_Warranty = txtWarrantyStatus.Text;
            oPRP.ProcurementBudget = (ddlProcurementBudget.SelectedIndex > 0 ? ddlProcurementBudget.SelectedValue : "");
            oPRP.AssetEndLife = txtAssetEndLife.Text.Trim();
            oPRP.AMC_Wrnty_Start_Date = txtWarrantyStartDate.Text.Trim();
            oPRP.AMC_Wrnty_End_Date = txtWarrantyEndDate.Text.Trim();
            oPRP.PurchaseOrderNo = txtPONo.Text;
            oPRP.PODate = txtPODate.Text.Trim();
            oPRP.InvoiceNo = txtInvoiceNo.Text;
            oPRP.InvoiceDate = txtInvoiceDate.Text.Trim();
            oPRP.SubStatus = ddlSubStatus.SelectedValue;
            oPRP.CompCode = Convert.ToString(Session["COMPANY"]);
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.PurchaseCost = string.Empty;
            oPRP.GRNDate = string.Empty;
            oPRP.Remarks = txtRemarks.Text.Trim();
            oPRP.Asset_FAR_TAG = txtAssetFARTag.Text;
            oPRP.SecurityGEDate = string.IsNullOrEmpty(txtInServiceDate.Text) ? string.Empty : txtInServiceDate.Text.Trim();
            txtLifeCycle.Text = string.IsNullOrEmpty(txtLifeCycle.Text) ? "0" : txtLifeCycle.Text;
            txtAssetEndLife.Text = string.IsNullOrEmpty(txtAssetEndLife.Text) ? "0" : txtAssetEndLife.Text;
            oPRP.AssetLife = txtLifeCycle.Text;
            oPRP.AssetEndLife = txtAssetEndLife.Text;
            string bResp = oDAL.SaveAssetAcquisition(oPRP);
            if (bResp.Contains("SUCCESS"))
            {
                dtassetFacilities.Rows.Add(bResp.Split('~')[0], oPRP.AssetLocation, oPRP.AssetType, oPRP.AssetMakeName, oPRP.AssetModelName, oPRP.Asset_FAR_TAG, oPRP.GRNNo, oPRP.GRNDate, oPRP.VendorCode, oPRP.PurchaseOrderNo, oPRP.PODate, oPRP.InvoiceNo, oPRP.InvoiceDate);
                dtassetFacilities.AcceptChanges();

                string strFileName = AssetFileUpload.FileName;
                if (!string.IsNullOrEmpty(strFileName))
                {

                    string subPath = Request.PhysicalApplicationPath + "DocumentUpload\\AcquisitionFileUpload\\";
                    if (!Directory.Exists(subPath))
                        Directory.CreateDirectory(subPath);
                    string strFilePath = subPath + "\\" + AssetFileUpload.FileName;
                    File.Delete(strFilePath);
                    AssetFileUpload.SaveAs(strFilePath);
                    oPRP.upload = new AssetFileUpload_PRP() { FileName = strFilePath, Process = "ACQUISITION", User = Convert.ToString(Session["CURRENTUSER"]), ID = bResp.Split('~')[0] };
                    oDAL.InvoiceFileUpload(oPRP);
                }
                else
                {
                    oPRP.upload = null;
                }
                if (btnSubmit.Text == "Save")
                {
                    DataTable dt = oDAL.GetMailTransactionDetails("ASSET_ACQUISITION", Convert.ToString(Session["COMP_NAME"]));
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            SendmailAlert sendmail = new SendmailAlert();
                            //sendmail.SendMailForTransaction(dt.Rows[0].Field<string>("TO_MAIL_ID"), dt.Rows[0].Field<string>("CC_MAIL_ID"), dt.Rows[0].Field<string>("MAIL_SUBJECT"), dt.Rows[0].Field<string>("MAIL_BODY"));
                            sendmail.FunctionSendingMailWithAssetData(dtassetFacilities, dt.Rows[0].Field<string>("TO_MAIL_ID"), dt.Rows[0].Field<string>("CC_MAIL_ID"), dt.Rows[0].Field<string>("MAIL_SUBJECT"), dt.Rows[0].Field<string>("MAIL_BODY"));
                        }
                        catch (Exception ee)
                        {

                        }
                    }
                }
                
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Asset Details saved successfully.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            else
            {
                string msg = bResp.ToString().Replace("'", "");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('" + msg + "');", true);
                txtAssetCode.Focus();
            }

        }
        catch (Exception ex) { HandleExceptions(ex); }
    }

    #endregion

    #region CUSTOM VALIDATION

    //protected void cvAssetCode_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    if (txtAssetCode.Enabled)
    //    {
    //        args.IsValid = !oDAL.CheckDuplicate(txtAssetCode.Text.Trim().ToUpper());
    //        if(!args.IsValid)
    //        {
    //            GetAssetDetails(Convert.ToString(txtAssetCode.Text.Trim().ToUpper()));
    //        }
    //    }
    //    else
    //    {
    //        args.IsValid = true;
    //    }
    //}

    protected void txtAssetCode_TextChanged(object sender, EventArgs e)
    {
        //cvAssetCode.Validate();
        if (txtAssetCode.Enabled)
        {
            bool IsValid = oDAL.CheckDuplicate(txtAssetCode.Text.Trim().ToUpper());
            if (IsValid)
            {
                GetAssetDetails(Convert.ToString(txtAssetCode.Text.Trim().ToUpper()));
            }
        }
    }

    //protected void cvAssetSerial_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    if (txtSerialNumber.Enabled)
    //    {
    //        args.IsValid = !oDAL.CheckSerialDuplicate(txtSerialNumber.Text.Trim().ToUpper());
    //        if (!args.IsValid)
    //        {
    //            GetAssetSerialDetails(Convert.ToString(txtSerialNumber.Text.Trim().ToUpper()));
    //        }
    //    }
    //    else
    //    {
    //        args.IsValid = true;
    //    }
    //}

    protected void txtSerialNumber_TextChanged(object sender, EventArgs e)
    {
        //cvAssetSerial.Validate();

        if (txtSerialNumber.Enabled)
        {
            bool IsValid = oDAL.CheckSerialDuplicate(txtSerialNumber.Text.Trim().ToUpper());
            if (IsValid)
            {
                GetAssetSerialDetails(Convert.ToString(txtSerialNumber.Text.Trim().ToUpper()));
            }
        }
    }

    //protected void cvRFIDTag_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    if (txtRFIDTag.Enabled)
    //    {
    //        args.IsValid = !oDAL.CheckRFIDDuplicate(txtRFIDTag.Text.Trim().ToUpper());
    //        if (!args.IsValid)
    //        {
    //            GetAssetRFIDTagDetails(Convert.ToString(txtRFIDTag.Text.Trim().ToUpper()));
    //        }
    //    }
    //    else
    //    {
    //        args.IsValid = true;
    //    }
    //}

    protected void txtRFIDTag_TextChanged(object sender, EventArgs e)
    {
        if (txtRFIDTag.Text != "" || txtRFIDTag.Text != null || txtRFIDTag.Text != string.Empty)
        {
            //cvRFIDTag.Validate();
            if (txtRFIDTag.Enabled)
            {
                bool IsValid = oDAL.CheckRFIDDuplicate(txtRFIDTag.Text.Trim().ToUpper());
                if (IsValid)
                {
                    GetAssetRFIDTagDetails(Convert.ToString(txtRFIDTag.Text.Trim().ToUpper()));
                }
            }
        }
    }

    protected void txtAssetFARTag_TextChanged(object sender, EventArgs e)
    {
        if (txtAssetFARTag.Text != "" || txtAssetFARTag.Text != null || txtAssetFARTag.Text != string.Empty)
        {
            //cvAssetFarTag.Validate();
            if (txtAssetFARTag.Enabled)
            {
                bool IsValid = oDAL.CheckAssetFarTagDuplicate(txtAssetFARTag.Text.Trim().ToUpper());
                if (IsValid)
                {
                    GetAssetFarTagDetails(Convert.ToString(txtAssetFARTag.Text.Trim().ToUpper()));
                }
            }
        }
    }

    //protected void cvAssetFarTag_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    if (txtAssetFARTag.Enabled)
    //    {
    //        args.IsValid = !oDAL.CheckAssetFarTagDuplicate(txtAssetFARTag.Text.Trim().ToUpper());
    //        if (!args.IsValid)
    //        {
    //            GetAssetFarTagDetails(Convert.ToString(txtAssetFARTag.Text.Trim().ToUpper()));
    //        }
    //    }
    //    else
    //    {
    //        args.IsValid = true;
    //    }
    //}

    #endregion

    #region CONTROL EVENTS

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLocation.SelectedIndex > 0)
        {
            GetFloorDropdown(ddlLocation.SelectedValue);
            GetStoreDropdown(ddlLocation.SelectedValue, ddlFloor.SelectedValue);
        }
    }

    #endregion

    #region File Upload
    private Tuple<bool, string, List<AssetAcquisition_PRP>> ValidateFile(DataTable dt)
    {
        bool res = true;
        string msg = "success";
        //string[] SubStatus = new string[] {  "WORKING", "IN SERVICE","LOST","RETIRED","DAMAGE","DEPLOYED","FAULTY" };
        List<AssetAcquisition_PRP> oPRPList = new List<AssetAcquisition_PRP>();
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            AssetAcquisition_PRP oPRP = new AssetAcquisition_PRP();
            oPRP.AssetCode = string.Empty;
            oPRP.AssetSerialCode = string.Empty;
            oPRP.AssetTag = string.Empty;
            oPRP.store = string.Empty;
            oPRP.AssetProcessor = string.Empty;
            oPRP.AssetRAM = string.Empty;
            oPRP.AssetHDD = string.Empty;
            oPRP.GRNDate = string.Empty;
            oPRP.AssetDomain = string.Empty;
            oPRP.PurchaseCost = string.Empty;
            oPRP.ProcurementBudget = (dr.Field<string>("PROCUREMENT_BUDGET") == null || dr.Field<string>("PROCUREMENT_BUDGET") == string.Empty || dr.Field<string>("PROCUREMENT_BUDGET") == "") ? string.Empty : dr.Field<string>("PROCUREMENT_BUDGET").Trim();
            oPRP.GRNNo = (dr.Field<string>("GRN_NO") == null || dr.Field<string>("GRN_NO") == string.Empty || dr.Field<string>("GRN_NO") == "") ? string.Empty : dr.Field<string>("GRN_NO").Trim();
            oPRP.InvoiceNo = (dr.Field<string>("INVOICE_NO") == null || dr.Field<string>("INVOICE_NO") == string.Empty || dr.Field<string>("INVOICE_NO") == "") ? string.Empty : dr.Field<string>("INVOICE_NO").Trim();
            oPRP.AssetType = (dr.Field<string>("ASSET_TYPE") == null || dr.Field<string>("ASSET_TYPE") == string.Empty || dr.Field<string>("ASSET_TYPE") == "") ? string.Empty : dr.Field<string>("ASSET_TYPE").Trim().ToUpper();
            oPRP.AssetMakeName = (dr.Field<string>("ASSET_MAKE") == null || dr.Field<string>("ASSET_MAKE") == string.Empty || dr.Field<string>("ASSET_MAKE") == "") ? string.Empty : dr.Field<string>("ASSET_MAKE").Trim().ToUpper();
            oPRP.AssetModelName = (dr.Field<string>("MODEL_NAME") == null || dr.Field<string>("MODEL_NAME") == string.Empty || dr.Field<string>("MODEL_NAME") == "") ? string.Empty : dr.Field<string>("MODEL_NAME").Trim().ToUpper();
            oPRP.AssetLocation = (dr.Field<string>("ASSET_LOCATION") == null || dr.Field<string>("ASSET_LOCATION") == string.Empty || dr.Field<string>("ASSET_LOCATION") == "") ? string.Empty : dr.Field<string>("ASSET_LOCATION").Trim().ToUpper();
            oPRP.IdentifierLocation = (dr.Field<string>("IDENTIFIER_LOCATION") == null || dr.Field<string>("IDENTIFIER_LOCATION") == string.Empty || dr.Field<string>("IDENTIFIER_LOCATION") == "") ? string.Empty : dr.Field<string>("IDENTIFIER_LOCATION").Trim().ToUpper(); 
            oPRP.AssetRFID = (dr.Field<string>("ASSET_RFID") == null || dr.Field<string>("ASSET_RFID") == string.Empty || dr.Field<string>("ASSET_RFID") == "") ? string.Empty : dr.Field<string>("ASSET_RFID").Trim();
            oPRP.floor = (dr.Field<string>("FLOOR") == null || dr.Field<string>("FLOOR") == string.Empty || dr.Field<string>("FLOOR") == "") ? string.Empty : dr.Field<string>("FLOOR").Trim().ToUpper();
            oPRP.SubCategory = (dr.Field<string>("ASSET_SUB_CATEGORY") == null || dr.Field<string>("ASSET_SUB_CATEGORY") == string.Empty || dr.Field<string>("ASSET_SUB_CATEGORY") == "") ? string.Empty : dr.Field<string>("ASSET_SUB_CATEGORY").Trim();
            oPRP.SubStatus = (dr.Field<string>("ASSET_WORKING_STATUS") == null || dr.Field<string>("ASSET_WORKING_STATUS") == string.Empty || dr.Field<string>("ASSET_WORKING_STATUS") == "") ? string.Empty : dr.Field<string>("ASSET_WORKING_STATUS").Trim();
            oPRP.VendorCode = (dr.Field<string>("OEM_VENDOR") == null || dr.Field<string>("OEM_VENDOR") == string.Empty || dr.Field<string>("OEM_VENDOR") == "") ? string.Empty : dr.Field<string>("OEM_VENDOR").Trim().ToUpper();
            oPRP.AMC_Warranty = (dr.Field<string>("WARRANTY_STATUS") == null || dr.Field<string>("WARRANTY_STATUS") == string.Empty || dr.Field<string>("WARRANTY_STATUS") == "") ? string.Empty : dr.Field<string>("WARRANTY_STATUS").Trim();
            oPRP.AssetLife = (dr.Field<string>("ASSET_FAR_LIFE") == null || dr.Field<string>("ASSET_FAR_LIFE") == string.Empty || dr.Field<string>("ASSET_FAR_LIFE") == "") ? string.Empty : dr.Field<string>("ASSET_FAR_LIFE").Trim();
            oPRP.AssetEndLife = (dr.Field<string>("ASSET_END_LIFE") == null || dr.Field<string>("ASSET_END_LIFE") == string.Empty || dr.Field<string>("ASSET_END_LIFE") == "") ? string.Empty : dr.Field<string>("ASSET_END_LIFE").Trim();
            oPRP.Asset_FAR_TAG = (dr.Field<string>("ASSET_FAR_TAG") == null || dr.Field<string>("ASSET_FAR_TAG") == string.Empty || dr.Field<string>("ASSET_FAR_TAG") == "") ? string.Empty : dr.Field<string>("ASSET_FAR_TAG").Trim();
            //oPRP.SecurityGEDate = ((dr.Field<string>("IN_SERVICE_DATE") == null || dr.Field<string>("IN_SERVICE_DATE") == "" || dr.Field<string>("IN_SERVICE_DATE") == string.Empty) ? string.Empty : ((dt.Columns.Contains("IN_SERVICE_DATE")) ? dr.Field<string>("IN_SERVICE_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty));
            if (dr.Field<string>("IN_SERVICE_DATE") == null || dr.Field<string>("IN_SERVICE_DATE") == "" || dr.Field<string>("IN_SERVICE_DATE") == string.Empty)
                oPRP.SecurityGEDate = string.Empty;
            else
                oPRP.SecurityGEDate = (dt.Columns.Contains("IN_SERVICE_DATE")) ? dr.Field<string>("IN_SERVICE_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            oPRP.PurchaseOrderNo = (dr.Field<string>("PO_NUMBER") == null || dr.Field<string>("PO_NUMBER") == string.Empty || dr.Field<string>("PO_NUMBER") == "") ? string.Empty : dr.Field<string>("PO_NUMBER").Trim();
            if (dr.Field<string>("PO_DATE") == null || dr.Field<string>("PO_DATE") == "" || dr.Field<string>("PO_DATE") == string.Empty)
                oPRP.PODate = string.Empty;
            else
                oPRP.PODate = (dt.Columns.Contains("PO_DATE")) ? dr.Field<string>("PO_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            if (dr.Field<string>("INVOICE_DATE") == null || dr.Field<string>("INVOICE_DATE") == "" || dr.Field<string>("INVOICE_DATE") == string.Empty)
                oPRP.InvoiceDate = string.Empty;
            else
                oPRP.InvoiceDate = (dt.Columns.Contains("INVOICE_DATE")) ? dr.Field<string>("INVOICE_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            oPRP.InvoiceNo = (dr.Field<string>("INVOICE_NO") == null || dr.Field<string>("INVOICE_NO") == string.Empty || dr.Field<string>("INVOICE_NO") == "") ? string.Empty : dr.Field<string>("INVOICE_NO").Trim();

            if (dr.Field<string>("REMARKS")=="" || dr.Field<string>("REMARKS") == string.Empty || dr.Field<string>("REMARKS") == null)
            {
                oPRP.Remarks = "NA";
            }
            else
            {
                oPRP.Remarks = dr.Field<string>("REMARKS").Trim();
            }

            if (//string.IsNullOrWhiteSpace(oPRP.AssetCode) ||
                string.IsNullOrWhiteSpace(oPRP.AssetType) ||
                string.IsNullOrWhiteSpace(oPRP.AssetMakeName) ||
                string.IsNullOrWhiteSpace(oPRP.floor) ||
                string.IsNullOrWhiteSpace(oPRP.AssetLocation) ||
                string.IsNullOrWhiteSpace(oPRP.VendorCode) ||
                string.IsNullOrWhiteSpace(oPRP.SubCategory) ||
                string.IsNullOrWhiteSpace(oPRP.SecurityGEDate) ||
                string.IsNullOrWhiteSpace(oPRP.Asset_FAR_TAG) ||
                string.IsNullOrWhiteSpace(oPRP.AssetLife) ||
                string.IsNullOrWhiteSpace(oPRP.SubStatus) ||
                string.IsNullOrWhiteSpace(oPRP.AssetEndLife) ||
                string.IsNullOrEmpty(oPRP.AssetType) ||
                string.IsNullOrEmpty(oPRP.AssetMakeName) ||
                string.IsNullOrEmpty(oPRP.floor) ||
                string.IsNullOrEmpty(oPRP.AssetLocation) ||
                string.IsNullOrEmpty(oPRP.VendorCode) ||
                string.IsNullOrEmpty(oPRP.SubCategory) ||
                string.IsNullOrEmpty(oPRP.SecurityGEDate) ||
                string.IsNullOrEmpty(oPRP.Asset_FAR_TAG) ||
                string.IsNullOrEmpty(oPRP.AssetLife) ||
                string.IsNullOrEmpty(oPRP.SubStatus) ||
                string.IsNullOrEmpty(oPRP.AssetEndLife)
                )
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter the mandatory fields.";
                res = false;
                break;
            }

            if (oPRP.SubStatus.ToUpper() == "OTHERS")
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Sub Status.";
                res = false;
                break;
            }
            else
            {
                var dtSubStatus = oDAL.GetSubStatus(oPRP.SubStatus.ToUpper());
                if (dtSubStatus.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Sub Status.";
                    res = false;
                    break;
                }
            }
            if (oPRP.Asset_FAR_TAG==""|| oPRP.Asset_FAR_TAG==string.Empty)
            {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Asset Far Tag is already Exists.";
                    res = false;
                    break;
                
            }
            var dtSLocation = oDAL.GetLocation(oPRP.AssetLocation,Session["COMPANY"].ToString());
            if (dtSLocation.Rows.Count <= 0)
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + " Asset Location is invalid.";
                res = false;
                break;
            }
            var dtfloor = oDAL.GetFloor(oPRP.AssetLocation, oPRP.floor,Session["COMPANY"].ToString());
            if (dtfloor.Rows.Count <= 0)
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + " floor Code is invalid.";
                res = false;
                break;
            }
            if (oPRP.VendorCode != "")
            {
                var dtVendor = oDAL.GetVendor(oPRP.VendorCode, Convert.ToString(Session["COMPANY"]));
                if (dtVendor.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Vendor Code is invalid.";
                    res = false;
                    break;
                }
            }
            
            if (oPRP.PODate != "" || !string.IsNullOrEmpty(oPRP.PODate))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.PODate, out date))
                {
                    oPRP.PODate = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid PO date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }
            if (oPRP.SecurityGEDate != "" || !string.IsNullOrEmpty(oPRP.SecurityGEDate))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.SecurityGEDate, out date))
                {
                    oPRP.SecurityGEDate = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid in service date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }

            if (oPRP.InvoiceDate != "" || !string.IsNullOrEmpty(oPRP.InvoiceDate))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.InvoiceDate, out date))
                {
                    oPRP.InvoiceDate = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid PO date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }
            if (dr.Field<string>("AMC_WARRANTY_START_DATE") == null || dr.Field<string>("AMC_WARRANTY_START_DATE") == "" || dr.Field<string>("AMC_WARRANTY_START_DATE") == string.Empty)
                oPRP.AMC_Wrnty_Start_Date = string.Empty;
            else
                oPRP.AMC_Wrnty_Start_Date = (dt.Columns.Contains("AMC_WARRANTY_START_DATE")) ? dr.Field<string>("AMC_WARRANTY_START_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            if (oPRP.AMC_Wrnty_Start_Date != "" || !string.IsNullOrEmpty(oPRP.AMC_Wrnty_Start_Date))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.AMC_Wrnty_Start_Date, out date))
                {
                    oPRP.AMC_Wrnty_Start_Date = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid PO date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }

            if (dr.Field<string>("AMC_WARRANTY_END_DATE") == null || dr.Field<string>("AMC_WARRANTY_END_DATE") == "" || dr.Field<string>("AMC_WARRANTY_END_DATE") == string.Empty)
                oPRP.AMC_Wrnty_End_Date = string.Empty;
            else
                oPRP.AMC_Wrnty_End_Date = (dt.Columns.Contains("AMC_WARRANTY_END_DATE")) ? dr.Field<string>("AMC_WARRANTY_END_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            if (oPRP.AMC_Wrnty_End_Date != "" || !string.IsNullOrEmpty(oPRP.AMC_Wrnty_End_Date))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.AMC_Wrnty_End_Date, out date))
                {
                    oPRP.AMC_Wrnty_End_Date = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid PO date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }

            //oPRP.Remarks = dt.Columns.Contains("REMARKS") ? dr.Field<string>("REMARKS") : string.Empty;
            oPRP.CompCode = Convert.ToString(Session["COMPANY"]);
            oPRP.CreatedBy = Convert.ToString(Session["CURRENTUSER"]);
            oPRP.SubStatus = oPRP.SubStatus.ToUpper();
            oPRPList.Add(oPRP);
            i++;
        }

        if (!res)
            oPRPList = new List<AssetAcquisition_PRP>();

        return new Tuple<bool, string, List<AssetAcquisition_PRP>>(res, msg, oPRPList);
    }

    private Tuple<bool, string, List<AssetAcquisition_PRP>> ValidateBulkUpdateFile(DataTable dt)
    {
        bool res = true;
        string msg = "success";
        //string[] SubStatus = new string[] {  "WORKING", "IN SERVICE","LOST","RETIRED","DAMAGE","DEPLOYED","FAULTY" };
        List<AssetAcquisition_PRP> oPRPList = new List<AssetAcquisition_PRP>();
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            AssetAcquisition_PRP oPRP = new AssetAcquisition_PRP();
            oPRP.AssetCode = string.Empty;
            oPRP.AssetSerialCode = string.Empty;
            oPRP.AssetTag = string.Empty;
            oPRP.store = string.Empty;
            oPRP.AssetProcessor = string.Empty;
            oPRP.AssetRAM = string.Empty;
            oPRP.AssetHDD = string.Empty;
            oPRP.GRNDate = string.Empty;
            oPRP.AssetDomain = string.Empty;
            oPRP.PurchaseCost = string.Empty;
            oPRP.ProcurementBudget = (dr.Field<string>("PROCUREMENT_BUDGET") == null || dr.Field<string>("PROCUREMENT_BUDGET") == string.Empty || dr.Field<string>("PROCUREMENT_BUDGET") == "") ? string.Empty : dr.Field<string>("PROCUREMENT_BUDGET").Trim();
            oPRP.GRNNo = (dr.Field<string>("GRN_NO") == null || dr.Field<string>("GRN_NO") == string.Empty || dr.Field<string>("GRN_NO") == "") ? string.Empty : dr.Field<string>("GRN_NO").Trim();
            oPRP.InvoiceNo = (dr.Field<string>("INVOICE_NO") == null || dr.Field<string>("INVOICE_NO") == string.Empty || dr.Field<string>("INVOICE_NO") == "") ? string.Empty : dr.Field<string>("INVOICE_NO").Trim();
            oPRP.AssetType = (dr.Field<string>("ASSET_TYPE") == null || dr.Field<string>("ASSET_TYPE") == string.Empty || dr.Field<string>("ASSET_TYPE") == "") ? string.Empty : dr.Field<string>("ASSET_TYPE").Trim().ToUpper();
            oPRP.AssetMakeName = (dr.Field<string>("ASSET_MAKE") == null || dr.Field<string>("ASSET_MAKE") == string.Empty || dr.Field<string>("ASSET_MAKE") == "") ? string.Empty : dr.Field<string>("ASSET_MAKE").Trim().ToUpper();
            oPRP.AssetModelName = (dr.Field<string>("MODEL_NAME") == null || dr.Field<string>("MODEL_NAME") == string.Empty || dr.Field<string>("MODEL_NAME") == "") ? string.Empty : dr.Field<string>("MODEL_NAME").Trim().ToUpper();
            oPRP.AssetLocation = (dr.Field<string>("ASSET_LOCATION") == null || dr.Field<string>("ASSET_LOCATION") == string.Empty || dr.Field<string>("ASSET_LOCATION") == "") ? string.Empty : dr.Field<string>("ASSET_LOCATION").Trim().ToUpper();
            oPRP.IdentifierLocation = (dr.Field<string>("IDENTIFIER_LOCATION") == null || dr.Field<string>("IDENTIFIER_LOCATION") == string.Empty || dr.Field<string>("IDENTIFIER_LOCATION") == "") ? string.Empty : dr.Field<string>("IDENTIFIER_LOCATION").Trim().ToUpper();
            oPRP.AssetRFID = (dr.Field<string>("ASSET_RFID") == null || dr.Field<string>("ASSET_RFID") == string.Empty || dr.Field<string>("ASSET_RFID") == "") ? string.Empty : dr.Field<string>("ASSET_RFID").Trim();
            oPRP.floor = (dr.Field<string>("FLOOR") == null || dr.Field<string>("FLOOR") == string.Empty || dr.Field<string>("FLOOR") == "") ? string.Empty : dr.Field<string>("FLOOR").Trim().ToUpper();
            oPRP.SubCategory = (dr.Field<string>("ASSET_SUB_CATEGORY") == null || dr.Field<string>("ASSET_SUB_CATEGORY") == string.Empty || dr.Field<string>("ASSET_SUB_CATEGORY") == "") ? string.Empty : dr.Field<string>("ASSET_SUB_CATEGORY").Trim();
            oPRP.SubStatus = (dr.Field<string>("ASSET_WORKING_STATUS") == null || dr.Field<string>("ASSET_WORKING_STATUS") == string.Empty || dr.Field<string>("ASSET_WORKING_STATUS") == "") ? string.Empty : dr.Field<string>("ASSET_WORKING_STATUS").Trim();
            oPRP.VendorCode = (dr.Field<string>("OEM_VENDOR") == null || dr.Field<string>("OEM_VENDOR") == string.Empty || dr.Field<string>("OEM_VENDOR") == "") ? string.Empty : dr.Field<string>("OEM_VENDOR").Trim().ToUpper();
            oPRP.AMC_Warranty = (dr.Field<string>("WARRANTY_STATUS") == null || dr.Field<string>("WARRANTY_STATUS") == string.Empty || dr.Field<string>("WARRANTY_STATUS") == "") ? string.Empty : dr.Field<string>("WARRANTY_STATUS").Trim();
            oPRP.AssetLife = (dr.Field<string>("ASSET_FAR_LIFE") == null || dr.Field<string>("ASSET_FAR_LIFE") == string.Empty || dr.Field<string>("ASSET_FAR_LIFE") == "") ? string.Empty : dr.Field<string>("ASSET_FAR_LIFE").Trim();
            oPRP.AssetEndLife = (dr.Field<string>("ASSET_END_LIFE") == null || dr.Field<string>("ASSET_END_LIFE") == string.Empty || dr.Field<string>("ASSET_END_LIFE") == "") ? string.Empty : dr.Field<string>("ASSET_END_LIFE").Trim();
            oPRP.Asset_FAR_TAG = (dr.Field<string>("ASSET_FAR_TAG") == null || dr.Field<string>("ASSET_FAR_TAG") == string.Empty || dr.Field<string>("ASSET_FAR_TAG") == "") ? string.Empty : dr.Field<string>("ASSET_FAR_TAG").Trim();


            //oPRP.SecurityGEDate = ((dr.Field<string>("IN_SERVICE_DATE") == null || dr.Field<string>("IN_SERVICE_DATE") == "" || dr.Field<string>("IN_SERVICE_DATE") == string.Empty) ? string.Empty : ((dt.Columns.Contains("IN_SERVICE_DATE")) ? dr.Field<string>("IN_SERVICE_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty));
            if (dr.Field<string>("IN_SERVICE_DATE") == null || dr.Field<string>("IN_SERVICE_DATE") == "" || dr.Field<string>("IN_SERVICE_DATE") == string.Empty)
                oPRP.SecurityGEDate = string.Empty;
            else
                oPRP.SecurityGEDate = (dt.Columns.Contains("IN_SERVICE_DATE")) ? dr.Field<string>("IN_SERVICE_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            oPRP.PurchaseOrderNo = (dr.Field<string>("PO_NUMBER") == null || dr.Field<string>("PO_NUMBER") == string.Empty || dr.Field<string>("PO_NUMBER") == "") ? string.Empty : dr.Field<string>("PO_NUMBER").Trim();
            if (dr.Field<string>("PO_DATE") == null || dr.Field<string>("PO_DATE") == "" || dr.Field<string>("PO_DATE") == string.Empty)
                oPRP.PODate = string.Empty;
            else
                oPRP.PODate = (dt.Columns.Contains("PO_DATE")) ? dr.Field<string>("PO_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            if (dr.Field<string>("INVOICE_DATE") == null || dr.Field<string>("INVOICE_DATE") == "" || dr.Field<string>("INVOICE_DATE") == string.Empty)
                oPRP.InvoiceDate = string.Empty;
            else
                oPRP.InvoiceDate = (dt.Columns.Contains("INVOICE_DATE")) ? dr.Field<string>("INVOICE_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            oPRP.InvoiceNo = (dr.Field<string>("INVOICE_NO") == null || dr.Field<string>("INVOICE_NO") == string.Empty || dr.Field<string>("INVOICE_NO") == "") ? string.Empty : dr.Field<string>("INVOICE_NO").Trim();

            if (dr.Field<string>("REMARKS") == "" || dr.Field<string>("REMARKS") == string.Empty || dr.Field<string>("REMARKS") == null)
            {
                oPRP.Remarks = "NA";
            }
            else
            {
                oPRP.Remarks = dr.Field<string>("REMARKS");
            }

            if (string.IsNullOrWhiteSpace(oPRP.Asset_FAR_TAG) ||
                string.IsNullOrEmpty(oPRP.Asset_FAR_TAG))
            {
                msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter the mandatory fields.";
                res = false;
                break;
            }
            oPRP.AssetCode = oDAL.GetAssetCodeforAcqusition(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
            if(oPRP.AssetCode == "" || oPRP.AssetCode == null || oPRP.AssetCode == string.Empty)
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Asset Far Tag.";
                res = false;
                break;
            }

            if (!string.IsNullOrEmpty(oPRP.SubStatus))
            {
                if (oPRP.SubStatus.ToUpper() == "OTHERS")
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Sub Status.";
                    res = false;
                    break;
                }
                else
                {
                    var dtSubStatus = oDAL.GetSubStatus(oPRP.SubStatus.ToUpper());
                    if (dtSubStatus.Rows.Count <= 0)
                    {
                        msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Sub Status.";
                        res = false;
                        break;
                    }
                }
            }

            bool Bverify = oDAL.ChkisAssetFarTagWithLocationExists(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
            if (!Bverify)
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + "Asset Far Tag does not exist.";
                res = false;
                break;
            }

            DataTable dtverifyforSitefloorstore = oDAL.GetAssetCurrentRequiredDetailsforFacilities(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
            if (dtverifyforSitefloorstore.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(oPRP.AssetLocation))
                    oPRP.AssetLocation = (dtverifyforSitefloorstore.Rows[0].Field<string>("ASSET_LOCATION") == null || dtverifyforSitefloorstore.Rows[0].Field<string>("ASSET_LOCATION") == string.Empty || dtverifyforSitefloorstore.Rows[0].Field<string>("ASSET_LOCATION") == "") ? string.Empty : dtverifyforSitefloorstore.Rows[0].Field<string>("ASSET_LOCATION").Trim().ToUpper();
                if (string.IsNullOrEmpty(oPRP.floor))
                    oPRP.floor = (dtverifyforSitefloorstore.Rows[0].Field<string>("FLOOR") == null || dtverifyforSitefloorstore.Rows[0].Field<string>("FLOOR") == string.Empty || dtverifyforSitefloorstore.Rows[0].Field<string>("FLOOR") == "") ? string.Empty : dtverifyforSitefloorstore.Rows[0].Field<string>("FLOOR").Trim().ToUpper();
                if (string.IsNullOrEmpty(oPRP.store))
                    oPRP.store = (dtverifyforSitefloorstore.Rows[0].Field<string>("STORE") == null || dtverifyforSitefloorstore.Rows[0].Field<string>("STORE") == string.Empty || dtverifyforSitefloorstore.Rows[0].Field<string>("STORE") == "") ? string.Empty : dtverifyforSitefloorstore.Rows[0].Field<string>("STORE").Trim().ToUpper();
            }

            if (!string.IsNullOrEmpty(oPRP.AssetLocation))
            {
                var dtSLocation = oDAL.GetLocation(oPRP.AssetLocation, Session["COMPANY"].ToString());
                if (dtSLocation.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Asset Location is invalid.";
                    res = false;
                    break;
                }
            }
            
            //bool ExistedFloor = oDAL.GetFloorforAcqusition(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
            //if (ExistedFloor)
            //{
            //    var dtfloor = oDAL.GetFloor(oPRP.AssetLocation, oPRP.floor, Session["COMPANY"].ToString());
            //    if (dtfloor.Rows.Count <= 0)
            //    {
            //        msg = "Please Note : Row Number " + (i + 1).ToString() + " floor Code is invalid.";
            //        res = false;
            //        break;
            //    }
            //}

            if (!string.IsNullOrEmpty(oPRP.floor))
            {
                var dtfloor = oDAL.GetFloor(oPRP.AssetLocation, oPRP.floor, Session["COMPANY"].ToString());
                if (dtfloor.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " floor Code is invalid.";
                    res = false;
                    break;
                }
            }

            DataTable dtnew = oDAL.GetAssetCurrentRequiredDetailsforFacilities(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
            if (dtnew.Rows.Count > 0)
            {
                oPRP.Status = dtnew.Rows[0].Field<string>("STATUS").ToUpper();
                if (dtnew.Rows[0].Field<string>("STATUS").ToUpper() == "ALLOCATED" || dtnew.Rows[0].Field<string>("STATUS").ToUpper() == "IN TRANSIT")
                {
                    oPRP.SubStatus = (dtnew.Rows[0].Field<string>("ASSET_SUB_STATUS") == null || dtnew.Rows[0].Field<string>("ASSET_SUB_STATUS") == string.Empty || dtnew.Rows[0].Field<string>("ASSET_SUB_STATUS") == "") ? string.Empty : dtnew.Rows[0].Field<string>("ASSET_SUB_STATUS").Trim().ToUpper();
                    oPRP.AssetLocation = (dtnew.Rows[0].Field<string>("ASSET_LOCATION") == null || dtnew.Rows[0].Field<string>("ASSET_LOCATION") == string.Empty || dtnew.Rows[0].Field<string>("ASSET_LOCATION") == "") ? string.Empty : dtnew.Rows[0].Field<string>("ASSET_LOCATION").Trim().ToUpper();
                    oPRP.floor = (dtnew.Rows[0].Field<string>("FLOOR") == null || dtnew.Rows[0].Field<string>("FLOOR") == string.Empty || dtnew.Rows[0].Field<string>("FLOOR") == "") ? string.Empty : dtnew.Rows[0].Field<string>("FLOOR").Trim().ToUpper();
                    oPRP.store = (dtnew.Rows[0].Field<string>("STORE") == null || dtnew.Rows[0].Field<string>("STORE") == string.Empty || dtnew.Rows[0].Field<string>("STORE") == "") ? string.Empty : dtnew.Rows[0].Field<string>("STORE").Trim().ToUpper();
                }
            }

            if (oPRP.VendorCode != "" || !string.IsNullOrEmpty(oPRP.VendorCode))
            {
                var dtVendor = oDAL.GetVendor(oPRP.VendorCode, Convert.ToString(Session["COMPANY"]));
                if (dtVendor.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Vendor Code is invalid.";
                    res = false;
                    break;
                }
            }

            if (oPRP.PODate != "" || !string.IsNullOrEmpty(oPRP.PODate))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.PODate, out date))
                {
                    oPRP.PODate = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid PO date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }
            if (oPRP.SecurityGEDate != "" || !string.IsNullOrEmpty(oPRP.SecurityGEDate))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.SecurityGEDate, out date))
                {
                    oPRP.SecurityGEDate = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid in service date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }

            if (oPRP.InvoiceDate != "" || !string.IsNullOrEmpty(oPRP.InvoiceDate))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.InvoiceDate, out date))
                {
                    oPRP.InvoiceDate = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid PO date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }
            if (dr.Field<string>("AMC_WARRANTY_START_DATE") == null || dr.Field<string>("AMC_WARRANTY_START_DATE") == "" || dr.Field<string>("AMC_WARRANTY_START_DATE") == string.Empty)
                oPRP.AMC_Wrnty_Start_Date = string.Empty;
            else
                oPRP.AMC_Wrnty_Start_Date = (dt.Columns.Contains("AMC_WARRANTY_START_DATE")) ? dr.Field<string>("AMC_WARRANTY_START_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            if (oPRP.AMC_Wrnty_Start_Date != "" || !string.IsNullOrEmpty(oPRP.AMC_Wrnty_Start_Date))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.AMC_Wrnty_Start_Date, out date))
                {
                    oPRP.AMC_Wrnty_Start_Date = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid PO date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }

            if (dr.Field<string>("AMC_WARRANTY_END_DATE") == null || dr.Field<string>("AMC_WARRANTY_END_DATE") == "" || dr.Field<string>("AMC_WARRANTY_END_DATE") == string.Empty)
                oPRP.AMC_Wrnty_End_Date = string.Empty;
            else
                oPRP.AMC_Wrnty_End_Date = (dt.Columns.Contains("AMC_WARRANTY_END_DATE")) ? dr.Field<string>("AMC_WARRANTY_END_DATE").Trim().Replace(" 00:00", "").Replace("'", "`").Trim() : string.Empty;

            if (oPRP.AMC_Wrnty_End_Date != "" || !string.IsNullOrEmpty(oPRP.AMC_Wrnty_End_Date))
            {
                string date = "";
                if (ConvertToExcel.isValidateDate(oPRP.AMC_Wrnty_End_Date, out date))
                {
                    oPRP.AMC_Wrnty_End_Date = date;
                }
                else
                {
                    msg = "Please Note : Row Number " + (i+1).ToString() + " Please enter valid PO date in dd-MMM-yyyy format. Also set cell as Date Format.";
                    res = false;
                    break;
                }
            }

            //oPRP.Remarks = dt.Columns.Contains("REMARKS") ? dr.Field<string>("REMARKS") : string.Empty;
            oPRP.CompCode = Convert.ToString(Session["COMPANY"]);
            oPRP.CreatedBy = Convert.ToString(Session["CURRENTUSER"]);
            oPRP.SubStatus = oPRP.SubStatus.ToUpper();
            oPRPList.Add(oPRP);
            i++;
        }

        if (!res)
            oPRPList = new List<AssetAcquisition_PRP>();

        return new Tuple<bool, string, List<AssetAcquisition_PRP>>(res, msg, oPRPList);
    }

    #endregion

    protected void btnUploadFile_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtforAssetFacilities = new DataTable();
            dtforAssetFacilities.Columns.Add("Asset Code");
            dtforAssetFacilities.Columns.Add("Site Location");
            dtforAssetFacilities.Columns.Add("Asset Type");
            dtforAssetFacilities.Columns.Add("Asset Make");
            dtforAssetFacilities.Columns.Add("Asset Model");
            dtforAssetFacilities.Columns.Add("Asset Far Tag");
            dtforAssetFacilities.Columns.Add("GRN No");
            dtforAssetFacilities.Columns.Add("GRN Date");
            dtforAssetFacilities.Columns.Add("OEM Vendor");
            dtforAssetFacilities.Columns.Add("PO No");
            dtforAssetFacilities.Columns.Add("PO Date");
            dtforAssetFacilities.Columns.Add("Invoice No");
            dtforAssetFacilities.Columns.Add("Invoice Date");

            Session["ERRORDATA"] = null;
            DataTable dtInvalidData = new DataTable();
            DataRow dataRow;
            ConvertToExcel convertToExcel = new ConvertToExcel();

            string AssetType = Convert.ToString(Session["COMPANY"]) == "IT" ? "ITASSET" : "FACILITIESASSET";

            var response = convertToExcel.ValidateFileReaded(fuBulkUpload, AssetType);
            if (response.Item1)
            {
                DataTable dt = response.Item2;
                if(dt.Rows.Count==0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('No Data Exist in the file');", true);
                    return;
                }
                dtInvalidData = dt.Clone();
                dtInvalidData.Columns.Add("STATUS");
                dtInvalidData.Columns.Add("ERROR_MESSAGE");
                var validate = ValidateFile(dt);
                if (validate.Item1)
                {
                    int Cnt = 0;
                    int i = 0;
                    foreach (var s in validate.Item3)
                    {
                        string result = string.Empty;
                        if (s.Asset_FAR_TAG != "")
                        {
                            bool bChk = false;
                            bChk = oDAL.ChkActiveAssetFarTagExists(s.Asset_FAR_TAG, Session["COMPANY"].ToString());
                            if (bChk)
                                result = "Asset Far Tag Exist Already";
                            else
                                result = "SUCCESS";
                        }
                        if(result == "SUCCESS")
                            result = oDAL.SaveBulkAssetAcquisition(s);

                        if (result.Contains("SUCCESS"))
                        {
                            dtforAssetFacilities.Rows.Add(result.Split('~')[0], s.AssetLocation, s.AssetType, s.AssetMakeName, s.AssetModelName, s.Asset_FAR_TAG, s.GRNNo, s.GRNDate, s.VendorCode, s.PurchaseOrderNo, s.PODate, s.InvoiceNo, s.InvoiceDate);
                            dtforAssetFacilities.AcceptChanges();
                            Cnt++;
                            continue;
                        }
                        else
                        {
                            dataRow = dtInvalidData.NewRow();
                            dataRow["ASSET_LOCATION"] = s.AssetLocation;
                            dataRow["FLOOR"] = s.floor;
                            dataRow["ASSET_FAR_LIFE"] = s.AssetLife;
                            dataRow["ASSET_TYPE"] = s.AssetType;
                            dataRow["ASSET_SUB_CATEGORY"] = s.SubCategory;
                            dataRow["PROCUREMENT_BUDGET"] = s.ProcurementBudget;
                            dataRow["GRN_NO"] = s.GRNNo;
                            dataRow["OEM_VENDOR"] = s.VendorCode;
                            dataRow["ASSET_MAKE"] = s.AssetMakeName;
                            dataRow["MODEL_NAME"] = s.AssetModelName;
                            dataRow["ASSET_RFID"] =s.AssetRFID;
                            dataRow["ASSET_FAR_TAG"] = s.Asset_FAR_TAG;
                            dataRow["ASSET_WORKING_STATUS"] = s.SubStatus;
                            dataRow["IDENTIFIER_LOCATION"] = s.IdentifierLocation;
                            dataRow["WARRANTY_STATUS"] = s.AMC_Warranty;
                            dataRow["AMC_WARRANTY_START_DATE"] =updatedatevalue(s.AMC_Wrnty_Start_Date);
                            dataRow["AMC_WARRANTY_END_DATE"] = updatedatevalue(s.AMC_Wrnty_End_Date);
                            dataRow["PO_NUMBER"] = s.PurchaseOrderNo;
                            dataRow["PO_DATE"] = updatedatevalue(s.PODate);
                            dataRow["INVOICE_NO"] = s.InvoiceNo;
                            dataRow["INVOICE_DATE"] = updatedatevalue(s.InvoiceDate);
                            dataRow["IN_SERVICE_DATE"] = updatedatevalue(s.SecurityGEDate);
                            dataRow["ASSET_END_LIFE"] = s.AssetEndLife;
                            dataRow["REMARKS"] = s.Remarks;
                            dataRow["STATUS"] = "ERROR";
                            dataRow["ERROR_MESSAGE"] = result;
                            dtInvalidData.Rows.Add(dataRow);
                            Session["ERRORDATA"] = dtInvalidData;
                            i++;
                            continue;
                        }
                    }
                    if (Cnt > 0)
                    {
                        DataTable dp = oDAL.GetMailTransactionDetails("ASSET_ACQUISITION", Convert.ToString(Session["COMP_NAME"]));
                        if (dp.Rows.Count > 0)
                        {
                            try
                            {
                                SendmailAlert sendmail = new SendmailAlert();
                                //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                                sendmail.FunctionSendingMailWithAssetData(dtforAssetFacilities, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                            }
                            catch (Exception ee)
                            {
                                //HandleExceptions(ee);
                            }
                        }
                    }
                    
                    if (i == 0)
                    {
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                    }
                    else
                    {
                        btnExport.Visible = true;
                        btnExport.Enabled = true;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('Some Of The Rows Are Not Inserted Please Download The Report To Find The Error');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('" + validate.Item2 + "');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('" + response.Item3 + "');", true);
                return;
            }

        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    private string updatedatevalue(string datevalue) 
    {
        if (datevalue == null || datevalue == string.Empty || datevalue == "")
            datevalue = string.Empty;
        else
            datevalue = datevalue.Replace('/', '-');

        return datevalue;
    }
    protected void btnBulkUpdateFile_Click(object sender, EventArgs e)
    {
        try
        {
            Session["ERRORDATA"] = null;
            DataTable dtInvalidData = new DataTable();
            DataRow dataRow;
            ConvertToExcel convertToExcel = new ConvertToExcel();

            string AssetType = Convert.ToString(Session["COMPANY"]) == "IT" ? "ITASSET" : "FACILITIESASSET";

            var response = convertToExcel.ValidateFileReaded(fuBulkUpload, AssetType);
            if (response.Item1)
            {
                DataTable dt = response.Item2;
                if (dt.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('No Data Exist in the file');", true);
                    return;
                }
                dtInvalidData = dt.Clone();
                dtInvalidData.Columns.Add("STATUS");
                dtInvalidData.Columns.Add("ERROR_MESSAGE");
                var validate = ValidateBulkUpdateFile(dt);
                if (validate.Item1)
                {
                    int Cnt = 0;
                    int i = 0;
                    foreach (var s in validate.Item3)
                    {
                        string result = string.Empty;
                        if (s.AssetCode != "" || s.AssetCode != null || s.AssetCode != string.Empty)
                        {
                            bool bChk = false;
                            bChk = oDAL.ChkActiveAssetFarTagisScrappedinFacilities(s.Asset_FAR_TAG, Session["COMPANY"].ToString());
                            if (bChk)
                                result = "The Current Status of this Asset is Scrapped ";
                            else
                                result = "SUCCESS";
                        }

                        if(result=="SUCCESS")
                        {
                            if (!string.IsNullOrEmpty(s.AssetRFID))
                            {
                                bool isRFIDExist = oDAL.CheckRFIDDuplicate(s.AssetRFID);
                                if (isRFIDExist)
                                {
                                    bool IsRFIDExistwiththeCurrentAsset = oDAL.CheckRFIDDuplicatetoVerifybulkUpdate(s.AssetRFID, s.AssetCode);
                                    if (!IsRFIDExistwiththeCurrentAsset)
                                    {
                                        result = "Asset RFID Tag is Exist with another Asset";
                                    }
                                }
                            }
                        }
                        
                        if(result=="SUCCESS")
                        {
                            if (!string.IsNullOrEmpty(s.AssetID))
                            {
                                bool isAssetTagExist = oDAL.CheckAssetTagDuplicate(s.AssetID);
                                if (isAssetTagExist)
                                {
                                    bool isAssetTagExistwiththeCurrentAsset = oDAL.CheckAssetTagDuplicatetoVerifybulkUpdate(s.AssetID, s.AssetCode);
                                    if (!isAssetTagExistwiththeCurrentAsset)
                                    {
                                        result = "Asset Tag is Exist with another Asset";
                                    }
                                }
                            }
                        }

                        if (result == "SUCCESS")
                            result = oDAL.SaveBulkUpdateAssetAcquisition(s);

                        if (result == "SUCCESS")
                        {
                            Cnt++;
                            continue;
                        }
                        else
                        {
                            dataRow = dtInvalidData.NewRow();
                            dataRow["ASSET_LOCATION"] = s.AssetLocation;
                            dataRow["FLOOR"] = s.floor;
                            dataRow["ASSET_FAR_LIFE"] = s.AssetLife;
                            dataRow["ASSET_TYPE"] = s.AssetType;
                            dataRow["ASSET_SUB_CATEGORY"] = s.SubCategory;
                            dataRow["PROCUREMENT_BUDGET"] = s.ProcurementBudget;
                            dataRow["GRN_NO"] = s.GRNNo;
                            dataRow["OEM_VENDOR"] = s.VendorCode;
                            dataRow["ASSET_MAKE"] = s.AssetMakeName;
                            dataRow["MODEL_NAME"] = s.AssetModelName;
                            dataRow["ASSET_RFID"] = s.AssetRFID;
                            dataRow["ASSET_FAR_TAG"] = s.Asset_FAR_TAG;
                            dataRow["ASSET_WORKING_STATUS"] = s.SubStatus;
                            dataRow["IDENTIFIER_LOCATION"] = s.IdentifierLocation;
                            dataRow["WARRANTY_STATUS"] = s.AMC_Warranty;
                            dataRow["AMC_WARRANTY_START_DATE"] = s.AMC_Wrnty_Start_Date;
                            dataRow["AMC_WARRANTY_END_DATE"] = s.AMC_Wrnty_End_Date;
                            dataRow["PO_NUMBER"] = s.PurchaseOrderNo;
                            dataRow["PO_DATE"] = s.PODate;
                            dataRow["INVOICE_NO"] = s.InvoiceNo;
                            dataRow["INVOICE_DATE"] = s.InvoiceDate;
                            dataRow["IN_SERVICE_DATE"] = s.SecurityGEDate;
                            dataRow["ASSET_END_LIFE"] = s.AssetEndLife;
                            dataRow["REMARKS"] = s.Remarks;
                            dataRow["STATUS"] = "ERROR";
                            dataRow["ERROR_MESSAGE"] = result;
                            dtInvalidData.Rows.Add(dataRow);
                            Session["ERRORDATA"] = dtInvalidData;
                            i++;
                            continue;
                        }
                    }
                    if (Cnt > 0)
                    {
                        DataTable dp = oDAL.GetMailTransactionDetails("ASSET_ACQUISITION", Convert.ToString(Session["COMP_NAME"]));
                        if (dp.Rows.Count > 0)
                        {
                            try
                            {
                                SendmailAlert sendmail = new SendmailAlert();
                                sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                            }
                            catch (Exception ee)
                            {
                                //HandleExceptions(ee);
                            }
                        }
                    }

                    if (i == 0)
                    {
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
                    }
                    else
                    {
                        btnExport.Visible = true;
                        btnExport.Enabled = true;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('Some Of The Rows Are Not Inserted Please Download The Report To Find The Error');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('" + validate.Item2 + "');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('" + response.Item3 + "');", true);
                return;
            }

        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    
    protected void ddlSubStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSubStatus.SelectedValue == "OTHERS")
        {
            txtSubStatusNew.Visible = true;
            btnsubStatusAdd.Visible = true;
        }
        else
        {
            txtSubStatusNew.Visible = false;
            btnsubStatusAdd.Visible = false;
        }
    }

    protected void btnsubStatusAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtSubStatusNew.Text) && txtSubStatusNew.Visible == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Enter New Asset Sub Status.');", true);
            return;
        }
        else if (oDAL.CheckDuplicateAssetSubStatus(txtSubStatusNew.Text.Trim()))
        {
            txtSubStatusNew.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Entered Asset Sub Status Already Exists.');", true);
            return;
        }
        else
        {
            bool bResp = false;
            bResp = oDAL.SaveAssetSubStatusDetails(txtSubStatusNew.Text.Trim().ToUpper(), Session["CURRENTUSER"].ToString());
            if (bResp)
            {
                GetSubStatusDropDown();
                txtSubStatusNew.Visible = false;
                btnsubStatusAdd.Visible = false;
                txtSubStatusNew.Text = string.Empty;
            }
            else
            {
                txtSubStatusNew.Visible = true;
                btnsubStatusAdd.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Asset Domain is not Saved.');", true);
                return;
            }
        }
    }

    protected void btnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataTable dt1 = new DataTable();
            if (Session["ERRORDATA"] != null)
            {
                dt1 = (DataTable)Session["ERRORDATA"];
            }
            if (dt1.Rows.Count > 0)
            {
                //Response.Clear();

                dt1.TableName = "ErrorData";
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
}