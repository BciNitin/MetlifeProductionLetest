using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.Text;
using System.Globalization;

public partial class AssetAllocation : System.Web.UI.Page
{
    string _CompCode = "";
    AssetAllocation_DAL oDAL;
    AssetAllocation_PRP oPRP;
    public AssetAllocation()
    {
        oPRP = new AssetAllocation_PRP();
    }
    ~AssetAllocation()
    {
        oDAL = null; oPRP = null;
    }

    #region PAGE EVENTS
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new AssetAllocation_DAL(Session["DATABASE"].ToString());
        _CompCode = Session["COMPANY"].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                string _strRights = clsGeneral.GetRights("ASSET_ALLOCATION", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_ALLOCATION");
                btnExport.Visible = false;
                btnExport.Enabled = false;
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                PopulateLocation();
                PopulateLocationFilter();
                PopulateAllocationTo();
                PopulateSubAllocationTo();
                PopulateMeetingRoom();
                PopulateFloor(ddlSiteLocation.SelectedValue);
                if (_CompCode == "Facilities")
                {
                    ddlAllocRtrn.Items.Remove("Temporary");
                    ddlAllocRtrn.DataBind();
                }
                if (AssetFileUpload.HasFile)
                {

                }
                if (ddlAllocRtrn.SelectedIndex > 0 && ddlAllocRtrn.SelectedValue.ToString() == "Temporary")
                {
                    txtExpRtnDate.Enabled = true;
                    enternoofduedate.Enabled = true;
                    lblenternoofduedate.Visible = true;
                    lblExpRtnDate1.Visible = true;
                }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    private void makeEmptyallFields()
    {
        oPRP.AssetCode = string.Empty;
        oPRP.AllocationDate = string.Empty;
        oPRP.RequestedBy = string.Empty;
        oPRP.RequestedById = string.Empty;
        oPRP.ApprovedBy = string.Empty;
        oPRP.ApprovedById = string.Empty;
        oPRP.AssetAllocationTo = string.Empty;
        oPRP.AllocatedTo = string.Empty;
        oPRP.AllocatedToId = string.Empty;
        oPRP.Process = string.Empty;
        oPRP.Store = string.Empty;
        oPRP.Floor = string.Empty;
        oPRP.ExpReturnDate = string.Empty;
        oPRP.TicketNo = string.Empty;
        oPRP.Vlan = string.Empty;
        oPRP.AssetTagID = string.Empty;
        oPRP.Site = string.Empty;
        oPRP.AllocationType = string.Empty;
        oPRP.EmpTagID = string.Empty;
        oPRP.Status = string.Empty;
        oPRP.NoofDueDate = string.Empty;
        oPRP.MeetingRoom = string.Empty;
        oPRP.CreatedBy = string.Empty;
        oPRP.AllocationRemarks = string.Empty;
        oPRP.HostName = string.Empty;
        oPRP.CompCode = string.Empty;
        oPRP.SubAssetAllocationTo = string.Empty;
        oPRP.HostName = string.Empty;
        //oPRP.AssetAllocated = string.Empty;
        oPRP.TicketNo = string.Empty;
        oPRP.AllocationDate = string.Empty;
        oPRP.ExpReturnDate = string.Empty;
        oPRP.AssetLocation = string.Empty;
        oPRP.EmpCode = string.Empty;
        oPRP.EmpTagID = string.Empty;
        oPRP.EmpName = string.Empty;
        oPRP.EmpFloor = string.Empty;
        oPRP.SeatNo = string.Empty;
        oPRP.Process = string.Empty;
        oPRP.Designation = string.Empty;
        oPRP.LOB = string.Empty;
        oPRP.SubLOB = string.Empty;
    }

    #region PRIVATE FUNCTIONS

    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Asset Allocation");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }

    private void PopulateEmployee()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetEmployee(Session["COMPANY"].ToString());

    }
    private void PopulateAllocationTo()
    {
        ddlAllocTo.DataSource = null;
        ddlAllocTo.DataBind();

        if (Session["COMPANY"].ToString() == "IT")
        {
            ddlAllocTo.Items.Insert(0, "-- Select Allocation To --");
            ddlAllocTo.Items.Insert(1, "EMPLOYEE");
            ddlAllocTo.Items.Insert(2, "FLOOR");

        }
        else if (Session["COMPANY"].ToString() == "Facilities")
        {
            ddlAllocTo.Items.Insert(0, "-- Select Allocation To --");
            ddlAllocTo.Items.Insert(1, "FLOOR");
        }
    }
    private void PopulateSubAllocationTo()
    {
        DataTable dt = new DataTable();
        ddlSubAllocTo.DataSource = null;
        ddlSubAllocTo.DataBind();
        ddlSubAllocTo.Items.Insert(0, "-- Sub Allocation To --");
        ddlSubAllocTo.Items.Insert(1, "TRAINING ROOM");
        ddlSubAllocTo.Items.Insert(2, "MEETING ROOM");
        ddlSubAllocTo.Items.Insert(3, "PHONE BOOTH");
        ddlSubAllocTo.Items.Insert(4, "CAFETERIA");
    }

    private void PopulateMeetingRoom()
    {
        ddlMeetingRoom.Items.Clear();
        DataTable dt = new DataTable();
        ddlMeetingRoom.DataSource = null;
        dt = oDAL.GetMeetingRoom(ddlSubAllocTo.SelectedItem.Text, ddlSiteLocation.SelectedValue, ddlFacilitiesFloor.SelectedValue, Session["COMPANY"].ToString());
        ddlMeetingRoom.DataSource = dt;
        ddlMeetingRoom.DataTextField = "MEETING_ROOM";
        ddlMeetingRoom.DataValueField = "MEETING_ROOM";
        ddlMeetingRoom.DataBind();
        //ddlMeetingRoom.Items.Insert(0, "-- Training/Meeting Room--");
        if (ddlSubAllocTo.SelectedValue.ToString() == "PHONE BOOTH" || ddlSubAllocTo.SelectedValue.ToString() == "CAFETERIA")
        {
            ddlMeetingRoom.Items.Clear();
            ddlMeetingRoom.Items.Insert(0, "-- Training/Meeting Room--");
        }
        else if (ddlSubAllocTo.SelectedValue == "TRAINING ROOM")
        {
            ddlMeetingRoom.Items.Insert(0, "-- Training Room--");
        }
        else if (ddlSubAllocTo.SelectedValue == "MEETING ROOM")
        {
            ddlMeetingRoom.Items.Insert(0, "-- Meeting Room--");
        }
        else if (ddlSubAllocTo.SelectedIndex == 0)
        { ddlMeetingRoom.Items.Insert(0, "-- Training/Meeting Room--"); }
    }

    private void PopulateLocation()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetSiteLocationWithoutfilter(Session["COMPANY"].ToString());
        ddlSiteLocation.DataSource = null;
        ddlSiteLocation.DataSource = dt;
        ddlSiteLocation.DataValueField = "SITE_CODE";
        ddlSiteLocation.DataTextField = "SITE_CODE";
        ddlSiteLocation.DataBind();
        ddlSiteLocation.Items.Insert(0, "-- Select Location --");
    }

    private void PopulateLocationFilter()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetSiteLocation(Session["COMPANY"].ToString());
        ddlSiteLocationFilter.DataSource = null;
        ddlSiteLocationFilter.DataSource = dt;
        ddlSiteLocationFilter.DataValueField = "SITE_CODE";
        ddlSiteLocationFilter.DataTextField = "SITE_CODE";
        ddlSiteLocationFilter.DataBind();
        ddlSiteLocationFilter.Items.Insert(0, "-- Select Location --");
    }
    private void PopulateFloor(string Loc)
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetFloorLocation(Loc, Session["COMPANY"].ToString());
        ddlFacilitiesFloor.DataSource = null;
        ddlFacilitiesFloor.DataSource = dt;
        ddlFacilitiesFloor.DataValueField = "FLOOR_CODE";
        ddlFacilitiesFloor.DataTextField = "FLOOR_CODE";
        ddlFacilitiesFloor.DataBind();
        ddlFacilitiesFloor.Items.Insert(0, "-- Select Floor --");
    }
    //private void PopulateStore(string Loc)
    //{
    //    DataTable dt = new DataTable();
    //    dt = oDAL.GetStoreLocation(Loc);
    //    ddlStore.DataSource = null;
    //    ddlStore.DataSource = dt;
    //    ddlStore.DataValueField = "STORE_CODE";
    //    ddlStore.DataTextField = "STORE_CODE";
    //    ddlStore.DataBind();
    //    ddlStore.Items.Insert(0, "-- Select Store --");
    //}
    private void GetAssetAllocationDetails(string CompCode)
    {
        gvAssetAllocation.DataSource = null;
        DataTable dt = new DataTable();
        dt = oDAL.GetAssetAllocationDetails(CompCode);
        gvAssetAllocation.DataSource = dt;
        gvAssetAllocation.DataBind();
    }



    #endregion

    #region BUTTON EVENTS
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if(ddlAllocTo.SelectedValue.ToString().ToUpper() == "FLOOR")
        {
            if (ddlSiteLocation.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Select Site Location.');", true);
                return;
            }
        }
        
        makeEmptyallFields();
        DataTable dtforEmail = new DataTable();
        dtforEmail.Columns.Add("Asset Code");
        dtforEmail.Columns.Add("Site Location");
        dtforEmail.Columns.Add("Allocation Date");
        dtforEmail.Columns.Add("Employee Name");
        dtforEmail.Columns.Add("Employee Code");
        dtforEmail.Columns.Add("Emp Email");
        dtforEmail.Columns.Add("Asset Type");
        dtforEmail.Columns.Add("Asset Make");
        dtforEmail.Columns.Add("Asset Model");
        dtforEmail.Columns.Add("Serial Number");
        dtforEmail.Columns.Add("Processor");
        dtforEmail.Columns.Add("RAM");
        dtforEmail.Columns.Add("HDD");
        dtforEmail.Columns.Add("RFID Tag Id");

        DataTable dtfloorcountonly = new DataTable();
        dtfloorcountonly.Columns.Add("Asset Code");
        dtfloorcountonly.Columns.Add("Site Location");
        dtfloorcountonly.Columns.Add("Floor");
        dtfloorcountonly.Columns.Add("Allocation Date");
        dtfloorcountonly.Columns.Add("Asset Type");
        dtfloorcountonly.Columns.Add("Asset Make");
        dtfloorcountonly.Columns.Add("Asset Model");
        dtfloorcountonly.Columns.Add("Serial Number");
        dtfloorcountonly.Columns.Add("Processor");
        dtfloorcountonly.Columns.Add("RAM");
        dtfloorcountonly.Columns.Add("HDD");
        dtfloorcountonly.Columns.Add("Seat No.");
        dtfloorcountonly.Columns.Add("Identifier Location");
        dtfloorcountonly.Columns.Add("RFID Tag Id");

        DataTable dtFacilitiesForEmail = new DataTable();
        dtFacilitiesForEmail.Columns.Add("Asset Code");
        dtFacilitiesForEmail.Columns.Add("Site Location");
        dtFacilitiesForEmail.Columns.Add("Floor");
        dtFacilitiesForEmail.Columns.Add("Allocation Date");
        dtFacilitiesForEmail.Columns.Add("Asset Type");
        dtFacilitiesForEmail.Columns.Add("Asset Make");
        dtFacilitiesForEmail.Columns.Add("Asset Model");
        dtFacilitiesForEmail.Columns.Add("Asset Far Tag");
        dtFacilitiesForEmail.Columns.Add("RFID Tag Id");

        try
        {
            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Asset Allocation", "Asset Allocation", "Asset Allocation done by user id" + Session["CURRENTUSER"].ToString() /*+ " Asset Serial" + txtAssetCode.Text*/);

            bool bGPStatus = false;
            string _LiveGPCode = "";
            if (clsGeneral._strRights[1] == "0")
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('You are not authorised to execute this operation.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('You are not authorised to execute this operation.');", true);
                return;
            }
            bool bAssetSelected = false;
            for (int iCnt = 0; iCnt < GridView3.Rows.Count; iCnt++)
            {
                if (((CheckBox)GridView3.Rows[iCnt].FindControl("chkSubGridSelectAsset")).Checked == true)
                {
                    bAssetSelected = true;
                    break;
                }
            }
            if (!bAssetSelected)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Click on Get Assets button/search to select assets.');", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Click on Get Assets button/search to select assets.');", true);
                return;
            }
            foreach (GridViewRow gvRow in GridView3.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSubGridSelectAsset")).Checked)
                {
                    oPRP.SerialCode = ((Label)gvRow.FindControl("lblSrno")).Text.Trim();
                    oPRP.AssetTagID = ((Label)gvRow.FindControl("lblRfTag")).Text.Trim();
                    oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                    _LiveGPCode = oDAL.ChkLiveGP(oPRP.AssetTagID);
                    if (!String.IsNullOrEmpty(_LiveGPCode))
                    {
                        bGPStatus = true;
                        break;
                    }
                }
            }
            if (bGPStatus)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Asset is already part of Gate Pass No: " + _LiveGPCode + " and is not returned yet.');", true);
                return;
            }
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.HostName = txtHostName.Text.Trim();
            oPRP.AllocationDate = txtAllocationDate.Text.Trim();
            //if (Convert.ToDateTime(oPRP.AllocationDate) >= Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy")))
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Allocated Date should not be greater than current date.');", true);
            //    return;
            //}
            oPRP.ExpReturnDate = (txtExpRtnDate.Text.Trim() != "") ? txtExpRtnDate.Text.Trim() : "";
            if (!String.IsNullOrEmpty(txtExpRtnDate.Text.Trim()))
            {
                if (Convert.ToDateTime(oPRP.AllocationDate) > Convert.ToDateTime(oPRP.ExpReturnDate))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Allocated Date should not be later than expected return date.');", true);
                    return;
                }
            }

            oPRP.MeetingRoom = (ddlMeetingRoom.SelectedIndex > 0 ? ddlMeetingRoom.SelectedValue : "");
            oPRP.SubAssetAllocationTo = (ddlSubAllocTo.SelectedIndex > 0 ? ddlSubAllocTo.SelectedItem.Text : string.Empty);
            oPRP.Site = (ddlSiteLocation.SelectedIndex > 0 ? ddlSiteLocation.SelectedValue : "");
            oPRP.AssetAllocated = true;
            oPRP.AssetAllocationTo = (ddlAllocTo.SelectedIndex > 0 ? ddlAllocTo.SelectedValue : "");
            //oPRP.SubAssetAllocationTo = ddlSubAllocTo.SelectedValue;
            oPRP.AllocationType = (ddlAllocRtrn.SelectedIndex > 0 ? ddlAllocRtrn.SelectedValue : "");
            if (Session["COMPANY"].ToString() == "IT")
            {
                if (txtTicketNo.Text.Trim() == "" || txtTicketNo.Text.Trim() == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Enter Ticket No.');", true);
                    return;
                }
            }


            if (txtAllocationDate.Text.Trim() == "" || txtAllocationDate.Text.Trim() == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Enter Allocation Date.');", true);
                return;
            }

            oPRP.TicketNo = txtTicketNo.Text.Trim();
            string floor = string.Empty;

            if (oPRP.CompCode != "Facilities")
            {
                if (ddlAllocTo.SelectedIndex > 0)
                {
                    if (ddlAllocTo.SelectedItem.Text == "FLOOR")
                    {
                        if (ddlFacilitiesFloor.SelectedIndex <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Select Floor is Mandatory.');", true);
                            return;
                        }
                    }

                    if (ddlAllocTo.SelectedItem.Text == "EMPLOYEE")
                    {
                        if (GridView1.Rows.Count <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Employee Code is mandatory.');", true);
                            return;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Select Allocation To is Mandatory.');", true);
                    return;
                }
            }
            else
            {
                if (ddlAllocTo.SelectedIndex > 0)
                {

                    if (ddlAllocTo.SelectedItem.Text == "FLOOR")
                    {
                        if (ddlFacilitiesFloor.SelectedIndex <=0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Select Floor is Mandatory');", true);
                            return;
                        }
                    }

                    if (ddlSubAllocTo.SelectedIndex <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Select Sub Allocation is Mandatory');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Select Allocation To is Mandatory.');", true);
                    return;
                }
            }

            if (ddlAllocRtrn.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Select Allocation Type is Mandatory');", true);
                return;
            }

            if (oPRP.CompCode != "Facilities")
                floor = ddlAllocTo.SelectedItem.Text;
            else
                floor = ddlAllocTo.SelectedItem.Text;

            if (floor == "FLOOR")
            {
                if (floor == "FLOOR")
                {
                    oPRP.Floor = (ddlFacilitiesFloor.SelectedIndex > 0 ? ddlFacilitiesFloor.SelectedValue : "");
                    //oPRP.AllocatedTo = (ddlAllocTo.SelectedIndex > 0 ? ddlAllocTo.SelectedValue : "");
                    //oPRP.AllocatedToId = txtEmployeeID.Text.Trim();
                    oPRP.SeatNo = txtSeatNo1.Text.Trim();
                }
                if (oPRP.CompCode == "Facilities")
                {
                    oPRP.Floor = (ddlFacilitiesFloor.SelectedIndex > 0 ? ddlFacilitiesFloor.SelectedValue : "");
                }
            }
            else
            {
                if (oPRP.CompCode != "Facilities" && (ddlAllocTo.SelectedItem.Text.ToString() == "EMPLOYEE"))
                {
                    if (GridView1.Rows.Count > 0)
                    {
                        foreach (GridViewRow r in GridView1.Rows)
                        {
                            oPRP.EmpTagID = ((Label)r.FindControl("lblEmpTag")).Text.Trim();
                            oPRP.EmailID = ((Label)r.FindControl("lblEmpMail")).Text.Trim();
                            oPRP.EmpCode = ((Label)r.FindControl("lblEmpID")).Text.Trim();
                            oPRP.EmpName = ((Label)r.FindControl("lblEmpName")).Text.Trim();
                            oPRP.Designation = ((Label)r.FindControl("lblde")).Text.Trim();
                            oPRP.Process = ((Label)r.FindControl("lblProcess")).Text.Trim();
                            oPRP.SubLOB = ((Label)r.FindControl("lblsub")).Text.Trim();
                            oPRP.LOB = ((Label)r.FindControl("lbllob")).Text.Trim();
                            oPRP.AllocatedToId = oPRP.EmpCode;
                            oPRP.AllocatedTo = oPRP.EmpName;

                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Search Employee Details.');", true);
                        return;
                    }
                }
            }
            oPRP.Status = "ALLOCATED";
            oPRP.NoofDueDate = enternoofduedate.Text.Trim();
            oPRP.AssetLocation = txtIdentifierlocation.Text.Trim();
            //bool bResp = true;
            string bResp = string.Empty;
            oPRP.AllocationRemarks = txtRemarks.Text.Trim().Replace("'", "`");
            foreach (GridViewRow gvRow in GridView3.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSubGridSelectAsset")).Checked)
                {
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.SerialCode = ((Label)gvRow.FindControl("lblSrno")).Text.Trim();
                    else
                        oPRP.Asset_FAR_TAG = ((Label)gvRow.FindControl("lblFarTag")).Text.Trim();

                    oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                    oPRP.AssetTagID = ((Label)gvRow.FindControl("lblRfTag")).Text.Trim();
                    oPRP.SubStatus = ((Label)gvRow.FindControl("lblAss")).Text.Trim();
                    if(ddlSiteLocation.SelectedIndex==0)
                    {
                        oPRP.Site = oDAL.GetAssetSiteLocation(oPRP.AssetCode,Session["COMPANY"].ToString());
                        oPRP.Floor = oDAL.GetAssetFloorLocation(oPRP.AssetCode, Session["COMPANY"].ToString());
                        if (oPRP.Floor == null || oPRP.Floor == "" || oPRP.Floor == string.Empty)
                            oPRP.Floor = string.Empty;
                        else
                            oPRP.Floor = oPRP.Floor;
                    }
                    else
                    {
                        if(ddlFacilitiesFloor.SelectedIndex==0)
                        {
                            oPRP.Site = oDAL.GetAssetSiteLocation(oPRP.AssetCode, Session["COMPANY"].ToString());
                            oPRP.Floor = oDAL.GetAssetFloorLocation(oPRP.AssetCode, Session["COMPANY"].ToString());
                            if (oPRP.Floor == null || oPRP.Floor == "" || oPRP.Floor == string.Empty)
                                oPRP.Floor = string.Empty;
                            else
                                oPRP.Floor = oPRP.Floor;
                        }
                        else
                        {
                            oPRP.Site = ddlSiteLocation.SelectedValue;
                            oPRP.Floor = ddlFacilitiesFloor.SelectedValue;
                            oPRP.Store = string.Empty;
                        }
                    }
                    
                    //bResp = oDAL.SaveAssetAllocation("ALLOCATE",oPRP);
                    bResp = oDAL.SaveAssetAllocationSP(oPRP);
                    if (bResp.Contains("SUCCESS"))
                    {
                        if (Session["COMPANY"].ToString() == "IT")
                        {
                            if (oPRP.AssetAllocationTo.ToString().ToUpper() == "EMPLOYEE")
                            {
                                foreach (GridViewRow r in GridView1.Rows)
                                {
                                    oPRP.EmailID = ((Label)r.FindControl("lblEmpMail")).Text.Trim();
                                    oPRP.EmpName = ((Label)r.FindControl("lblEmpName")).Text.Trim();
                                    oPRP.EmpCode = ((Label)r.FindControl("lblEmpID")).Text.Trim();
                                    DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                                    string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                                    string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                                    string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                                    string AssetProcessor = (dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == null || dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR").Trim();
                                    string AssetHDD = (dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == null || dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_RAM").Trim();
                                    string AssetRAM = (dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == null || dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_HDD").Trim();
                                    string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                                    dtforEmail.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.AllocationDate, oPRP.EmpName, oPRP.EmpCode, oPRP.EmailID, AssetType, AssetMake, AssetModel, oPRP.SerialCode, AssetProcessor, AssetHDD, AssetRAM, RFIDTag);
                                    dtforEmail.AcceptChanges();
                                }

                            }
                            else
                            {
                                DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                                string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                                string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                                string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                                string AssetProcessor = (dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == null || dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR").Trim();
                                string AssetHDD = (dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == null || dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_RAM").Trim();
                                string AssetRAM = (dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == null || dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_HDD").Trim();
                                string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                                dtfloorcountonly.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.Floor, oPRP.AllocationDate, AssetType, AssetMake, AssetModel, oPRP.SerialCode, AssetProcessor, AssetHDD, AssetRAM,oPRP.SeatNo, oPRP.AssetLocation, RFIDTag);
                                dtfloorcountonly.AcceptChanges();
                            }
                        }
                        else
                        {
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                            dtFacilitiesForEmail.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.Floor, oPRP.AllocationDate, AssetType, AssetMake, AssetModel, oPRP.Asset_FAR_TAG, RFIDTag);
                            dtFacilitiesForEmail.AcceptChanges();
                        }
                            

                        
                    }
                    //dtforEmail.Rows.Add(oPRP.AssetCode,oPRP.SerialCode);
                }
            }
            //if (Session["COMPANY"].ToString() == "IT")
            //    dtforEmail.AcceptChanges();
            //else
            //    dtFacilitiesForEmail.AcceptChanges();

            if (bResp.Contains("SUCCESS"))
            {
                if (ddlAllocTo.SelectedValue == "EMPLOYEE")
                {
                    DataTable dp1 = oDAL.GetMailTransactionDetails("ASSET_ALLOCATION", Convert.ToString(Session["COMP_NAME"]));
                    string Subject = "Assets Allocated";
                    string MailBody = "";
                    string CCAddress = "";
                    if (dp1.Rows.Count > 0)
                    {
                        CCAddress = (dp1.Rows[0].Field<string>("CC_MAIL_ID") == null || dp1.Rows[0].Field<string>("CC_MAIL_ID") == "" || dp1.Rows[0].Field<string>("CC_MAIL_ID") == string.Empty) ? "" : dp1.Rows[0].Field<string>("CC_MAIL_ID").Trim();
                        Subject = (dp1.Rows[0].Field<string>("MAIL_SUBJECT") == null || dp1.Rows[0].Field<string>("MAIL_SUBJECT") == "" || dp1.Rows[0].Field<string>("MAIL_SUBJECT") == string.Empty) ? "" : dp1.Rows[0].Field<string>("MAIL_SUBJECT").Trim();
                        MailBody = (dp1.Rows[0].Field<string>("MAIL_BODY") == null || dp1.Rows[0].Field<string>("MAIL_BODY") == "" || dp1.Rows[0].Field<string>("MAIL_BODY") == string.Empty) ? "" : dp1.Rows[0].Field<string>("MAIL_BODY").Trim();
                    }
                    foreach (GridViewRow r in GridView1.Rows)
                    {
                        oPRP.EmailID = ((Label)r.FindControl("lblEmpMail")).Text.Trim();
                        try
                        {
                            SendmailAlert sendmail1 = new SendmailAlert();
                            sendmail1.FunctionSendingAqcuisitionMail(dtforEmail, oPRP.EmailID, CCAddress, Subject, MailBody);
                        }
                        catch (Exception ee)
                        {
                            clsGeneral.LogErrorToLogFile(ee, "Employee Asset Allocation");
                            clsGeneral.LogErrorToLogFile(ee.InnerException, "Employee Asset Allocation");
                        }

                    }
                }
                DataTable dp = oDAL.GetMailTransactionDetails("ASSET_ALLOCATION", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
                {
                    try
                    {
                        //if allocation to is floor --> dtforEmail FOR IT / dtFacilitiesForEmail for Facilities the datatable contains asset data (Asset code, Serial code/ Asset Far TAg)
                        if (ddlAllocTo.SelectedValue != "EMPLOYEE")
                        {
                            try
                            {
                                SendmailAlert sendmail = new SendmailAlert();
                                if (Session["COMPANY"].ToString() == "IT")
                                    sendmail.FunctionSendingMailWithAssetData(dtfloorcountonly, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                                else
                                    sendmail.FunctionSendingMailWithAssetData(dtFacilitiesForEmail, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                                //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                            }
                            catch (Exception ee) 
                            {
                                clsGeneral.LogErrorToLogFile(ee, "Asset Allocation");
                                clsGeneral.LogErrorToLogFile(ee.InnerException, "Asset Allocation");
                            }

                        }

                    }
                    catch (Exception ee)
                    {

                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Selected Assets allocated successfully.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                upSubmit.Update();
            }
            GetAssetAllocationDetails(Session["COMPANY"].ToString());

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void btnRefreshCategory_Click(object sender, ImageClickEventArgs e)
    {
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }
    #endregion
    #region SELECTEDINDEXCHANGED EVENTS
    protected void ddlAllocTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridView2.DataSource = null;
            GridView2.DataBind();
            //PopulateSubAllocationTo();
            if (ddlAllocTo.SelectedIndex > 0)
            {
                if (ddlAllocTo.SelectedValue.ToString() == "FLOOR")
                {
                    Label26.Visible = true;
                    ddlSiteLocation.Enabled = true;
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    ddlFacilitiesFloor.Enabled = true;
                    txtSeatNo1.Enabled = true;
                    txtEmployeeID.Enabled = false;
                    txtEmployeeID.Text = string.Empty;
                    txtExpRtnDate.Enabled = false;
                    GridView1.Visible = false;
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    enternoofduedate.Enabled = false;
                    txtIdentifierlocation.Enabled = true;
                }
                else
                {
                    Label26.Visible = false;
                    ddlFacilitiesFloor.Enabled = false;
                    ddlSiteLocation.SelectedIndex = 0;
                    ddlSiteLocation.Enabled = false;
                    PopulateFloor(ddlSiteLocation.SelectedValue.Trim());
                    txtEmployeeID.Enabled = true;
                    txtExpRtnDate.Enabled = true;
                    enternoofduedate.Enabled = true;
                    txtIdentifierlocation.Enabled = false;

                }

            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void ddlAllocRtrn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAllocRtrn.SelectedIndex > 0)
            {
                txtExpRtnDate.Text = null;
                enternoofduedate.Text = null;
                if (ddlAllocRtrn.SelectedValue.ToString() == "Temporary")
                {
                    txtExpRtnDate.Enabled = true;
                    enternoofduedate.Enabled = true;
                    //rfventernoofduedate.Enabled = true;
                    //rfvExpRtnDate.Enabled = true;
                    lblenternoofduedate.Visible = true;
                    lblExpRtnDate1.Visible = true;
                }
                else
                {
                    txtExpRtnDate.Enabled = false;
                    enternoofduedate.Enabled = false;
                    //rfventernoofduedate.Enabled = false;
                    //rfvExpRtnDate.Enabled = false;
                    lblenternoofduedate.Visible = false;
                    lblExpRtnDate1.Visible = false;
                }

            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
    #region GRIDVIEW EVENTS
    /// <summary>
    /// Embed checkbox into the assets grid as the assets bound to it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssets_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //try
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow &&
        //       (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        //    {
        //        CheckBox chkSelect = (CheckBox)e.Row.Cells[0].FindControl("chkSelectAsset");
        //       // CheckBox chkHSelect = (CheckBox)this.gvAssets.HeaderRow.FindControl("chkHSelect");
        //        chkSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkHSelect.ClientID);
        //    }
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
    }

    /// <summary>
    /// Assets gridview page index changing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //try
        //{
        //    gvAssets.PageIndex = e.NewPageIndex;
        //    gvAssets.DataSource = (DataTable)Session["Assets"];
        //    gvAssets.DataBind();
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
    }

    /// <summary>
    /// Asset allocation delete event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetAllocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (clsGeneral._strRights[3] == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('You are not authorised to execute this operation.');", true);
            }
            else
            {

            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    /// <summary>
    /// Asset allocation history page index changed event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssetAllocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssetAllocation.PageIndex = e.NewPageIndex;
            GetAssetAllocationDetails(Session["COMPANY"].ToString());
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region File Upload
    private Tuple<bool, string> ValidateITFile(DataTable dt)
    {
        bool res = true;
        string msg = "success";
        string[] AllocationTo = new string[] { "EMPLOYEE", "FLOOR" };
        string[] SubAllocationTo = new string[] { "MEETING ROOM", "PHONE BOOTH", "TRAINING ROOM", "CAFETERIA" };
        string[] AllocationType = new string[] { "Permanent", "Temporary" };
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            i++;
            oPRP.HostName = (dr.Field<string>("HOST_NAME") == null || dr.Field<string>("HOST_NAME") == "" || dr.Field<string>("HOST_NAME") == string.Empty) ? string.Empty : dr.Field<string>("HOST_NAME").Trim();

            if (dr.Field<string>("ALLOCATION_DATE") == null || dr.Field<string>("ALLOCATION_DATE") == "" || dr.Field<string>("ALLOCATION_DATE") == string.Empty)
                oPRP.AllocationDate = string.Empty;
            else
                oPRP.AllocationDate = dr.Field<string>("ALLOCATION_DATE").Trim();

            if (dr.Field<string>("EXPECTED_RETURN_DATE") == null || dr.Field<string>("EXPECTED_RETURN_DATE") == "" || dr.Field<string>("EXPECTED_RETURN_DATE") == string.Empty)
                oPRP.ExpReturnDate = string.Empty;
            else
                oPRP.ExpReturnDate = dr.Field<string>("EXPECTED_RETURN_DATE").Trim();

            oPRP.Site = (dr.Field<string>("SITE_LOCATION") == null || dr.Field<string>("SITE_LOCATION") == "" || dr.Field<string>("SITE_LOCATION") == string.Empty) ? string.Empty : dr.Field<string>("SITE_LOCATION").Trim().ToUpper();
            oPRP.AssetAllocationTo = (dr.Field<string>("ASSET_ALLOCATION_TO") == null || dr.Field<string>("ASSET_ALLOCATION_TO") == "" || dr.Field<string>("ASSET_ALLOCATION_TO") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_ALLOCATION_TO").Trim().ToUpper();
            oPRP.SubAssetAllocationTo = (dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == null || dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == "" || dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == string.Empty) ? string.Empty : dr.Field<string>("SUB_ASSET_ALLOCATION_TO").Trim().ToUpper();
            oPRP.AllocationType = (dr.Field<string>("ALLOCATION_TYPE") == null || dr.Field<string>("ALLOCATION_TYPE") == "" || dr.Field<string>("ALLOCATION_TYPE") == string.Empty) ? string.Empty : dr.Field<string>("ALLOCATION_TYPE").Trim();
            oPRP.TicketNo = (dr.Field<string>("TICKET_NO") == null || dr.Field<string>("TICKET_NO") == "" || dr.Field<string>("TICKET_NO") == string.Empty) ? string.Empty : dr.Field<string>("TICKET_NO").Trim();
            oPRP.Floor = (dr.Field<string>("FLOOR") == null || dr.Field<string>("FLOOR") == "" || dr.Field<string>("FLOOR") == string.Empty) ? string.Empty : dr.Field<string>("FLOOR").Trim().ToUpper();
            oPRP.SeatNo = (dr.Field<string>("SEAT_NO") == null || dr.Field<string>("SEAT_NO") == "" || dr.Field<string>("SEAT_NO") == string.Empty) ? string.Empty : dr.Field<string>("SEAT_NO").Trim().ToUpper();
            oPRP.EmpCode = (dr.Field<string>("EMPLOYEE_CODE") == null || dr.Field<string>("EMPLOYEE_CODE") == "" || dr.Field<string>("EMPLOYEE_CODE") == string.Empty) ? string.Empty : dr.Field<string>("EMPLOYEE_CODE").Trim();
            oPRP.NoofDueDate = (dr.Field<string>("NO_OF_DUE_DATE") == null || dr.Field<string>("NO_OF_DUE_DATE") == "" || dr.Field<string>("NO_OF_DUE_DATE") == string.Empty) ? string.Empty : dr.Field<string>("NO_OF_DUE_DATE").Trim();
            oPRP.AssetLocation = (dr.Field<string>("IDENTIFIER_LOCATION") == null || dr.Field<string>("IDENTIFIER_LOCATION") == "" || dr.Field<string>("IDENTIFIER_LOCATION") == string.Empty) ? string.Empty : dr.Field<string>("IDENTIFIER_LOCATION").Trim();
            oPRP.SerialCode = (dr.Field<string>("SERIAL_NUMBER") == null || dr.Field<string>("SERIAL_NUMBER") == "" || dr.Field<string>("SERIAL_NUMBER") == string.Empty) ? string.Empty : dr.Field<string>("SERIAL_NUMBER").Trim();
            oPRP.MeetingRoom = (dr.Field<string>("MEETING_ROOM") == null || dr.Field<string>("MEETING_ROOM") == "" || dr.Field<string>("MEETING_ROOM") == string.Empty) ? string.Empty : dr.Field<string>("MEETING_ROOM").Trim().ToUpper();
            oPRP.AssetCode = oDAL.GetAssetCode(oPRP.SerialCode, Session["COMPANY"].ToString());
            oPRP.AssetTagID = oDAL.GetRFIDTag(oPRP.SerialCode, Session["COMPANY"].ToString());

            if (string.IsNullOrEmpty(oPRP.AssetCode) || oPRP.AssetCode == "")
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Serial Code.";
                res = false;
                break;
            }
            //DataTable dy = oDAL.ValidateAssetDetailsForAllocation(oPRP.AssetCode.Trim(), oPRP.AssetTagID.Trim());
            //if (dy.Rows.Count == 0)
            //{
            //    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Serial Number.";
            //    res = false;
            //    break;
            //}

            if (oPRP.AllocationType.Trim() == "" ||
                oPRP.AssetAllocationTo.Trim() == "" ||
                oPRP.TicketNo.Trim() == "" ||
                oPRP.AllocationDate.Trim() == "" ||
                oPRP.SerialCode.Trim() == "" 
                )
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Please enter the mandatory fields.";
                res = false;
                break;
            }

            string _date = "";
            if (!string.IsNullOrEmpty(oPRP.ExpReturnDate))
            {
                if (oPRP.ExpReturnDate.Trim() != "")
                {
                    if (!isValidateDate(oPRP.ExpReturnDate.Trim().Replace(" 00:00", "").Trim(), out _date))
                    {
                        msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Expected Return Date.";
                        res = false;
                        break;
                    }
                }
            }

            if (!isValidateDate(oPRP.AllocationDate.Trim().Replace(" 00:00", "").Trim(), out _date))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Allocation Date.";
                res = false;
                break;
            }
            if (!AllocationTo.Contains(oPRP.AssetAllocationTo))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Allocation To.";
                res = false;
                break;
            }

            if (!string.IsNullOrEmpty(oPRP.SubAssetAllocationTo))
            {
                if (!SubAllocationTo.Contains(oPRP.SubAssetAllocationTo))
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Sub Allocation To.";
                    res = false;
                    break;
                }
            }
            if (!AllocationType.Contains(oPRP.AllocationType))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Allocation Type.";
                res = false;
                break;
            }
            if (oPRP.AssetAllocationTo == "EMPLOYEE" && (oPRP.EmpCode.Trim() == ""))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Please mention employee code, if Allocation to is employee.";
                res = false;
                break;
            }
            //if (!String.IsNullOrEmpty(oPRP.EmpCode))
            //{
            //    DataTable de = oDAL.GetEmpDetails(oPRP.EmpCode);
            //    if (de.Rows.Count == 0)
            //    {
            //        msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid employee code.";
            //        res = false;
            //        break;
            //    }
            //}
            if ((oPRP.AllocationType == "Temporary") && ((oPRP.NoofDueDate.Trim() == "") || (oPRP.ExpReturnDate.Trim() == "")))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Please mention No. of Due Date/Expected Return Date, if Allocation Type is Temporary.";
                res = false;
                break;
            }
            if (oPRP.AssetAllocationTo == "EMPLOYEE")
            {
                oPRP.MeetingRoom = string.Empty;
                oPRP.Floor = string.Empty;
                if (oPRP.AllocationType == "Permanent")
                {
                    oPRP.NoofDueDate = "0";
                    oPRP.SeatNo = string.Empty;
                    oPRP.IdentifierLocation = string.Empty;
                    oPRP.ExpReturnDate = string.Empty;
                }
                else
                {
                    oPRP.SeatNo = string.Empty;
                    oPRP.IdentifierLocation = string.Empty;
                }
            }
            else
            {

                if (oPRP.AssetAllocationTo == "FLOOR" && (oPRP.Floor.Trim() == "" || string.IsNullOrEmpty(oPRP.Floor)))
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Please mention floor, if Allocation to is floor type.";
                    res = false;
                    break;
                }
                oPRP.EmpCode = string.Empty;
                if (oPRP.AllocationType == "Permanent")
                {
                    oPRP.NoofDueDate = "0";
                    oPRP.IdentifierLocation = string.Empty;
                }
            }
            if (oPRP.AllocationType == "Permanent")
            {
                oPRP.NoofDueDate = "0";
            }

            if (dr.Field<string>("REMARKS") == "" || dr.Field<string>("REMARKS") == string.Empty || dr.Field<string>("REMARKS") == null)
            {
                oPRP.AllocationRemarks = "NA";
            }
            else
            {
                oPRP.AllocationRemarks = dr.Field<string>("REMARKS");
            }

            if (oPRP.AssetAllocationTo == "Store" && (oPRP.Store.Trim() == ""))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Please mention store, if Allocation to is store type.";
                res = false;
                break;
            }
            //if (!String.IsNullOrEmpty(oPRP.Site.Trim()))
            //{
            //    var dtSLocation = oDAL.GetLocation(oPRP.Site, Session["COMPANY"].ToString());
            //    if (dtSLocation.Rows.Count <= 0)
            //    {
            //        msg = "Please Note : Row Number " + (i+1).ToString() + " Asset Location is invalid.";
            //        res = false;
            //        break;
            //    }
            //}
            if (oPRP.AssetAllocationTo == "FLOOR") 
            {
                var dtSLocation = oDAL.GetLocation(oPRP.Site, Session["COMPANY"].ToString());
                if (dtSLocation.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Asset Location is invalid.";
                    res = false;
                    break;
                }
            }
            

            if (oPRP.AssetAllocationTo == "FLOOR")
            {
                if (oPRP.Site.ToString().ToUpper() != oDAL.GetAcquisitionLocation(oPRP.SerialCode, Session["COMPANY"].ToString()).ToUpper())
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " The location you have entered is not matching with original Asset Location.";
                    res = false;
                    break;
                }
            }
            if (!String.IsNullOrEmpty(oPRP.Floor.Trim()))
            {
                DataTable de = oDAL.ValidateFloorLocation(oPRP.Site.Trim(), oPRP.Floor.Trim(), Session["COMPANY"].ToString());
                if (de.Rows.Count == 0)
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Site and Floor Combination.";
                    res = false;
                    break;
                }
            }

            if (oPRP.SubAssetAllocationTo == "CAFETERIA" || oPRP.SubAssetAllocationTo == "PHONE BOOTH")
            {
                if (oPRP.MeetingRoom == null || oPRP.MeetingRoom == string.Empty || oPRP.MeetingRoom == "")
                {
                    oPRP.MeetingRoom = "";
                }
                else
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Value in Meeting Room.";
                    res = false;
                    break;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oPRP.MeetingRoom.Trim()))
                {
                    var dtMeetingRoom = oDAL.GetMeetingRoomwithFloorandSite(oPRP.Site, oPRP.Floor, oPRP.SubAssetAllocationTo, oPRP.MeetingRoom, Session["COMPANY"].ToString()); //GetStorewithFloorandSite
                    if (dtMeetingRoom.Rows.Count <= 0)
                    {
                        msg = "Please Note : Row Number " + (i + 1).ToString() + " Meeting Room is invalid.";
                        res = false;
                        break;
                    }
                }
            }

        }
        return new Tuple<bool, string>(res, msg);
    }
    private void SaveITAssetAllocation(DataTable dt)
    {
        makeEmptyallFields();
        DataTable dtforemail = new DataTable();
        dtforemail.Columns.Add("Asset Code");
        dtforemail.Columns.Add("Site Location");
        dtforemail.Columns.Add("Allocation Date");
        dtforemail.Columns.Add("Employee Name");
        dtforemail.Columns.Add("Employee Code");
        dtforemail.Columns.Add("Emp Email");
        dtforemail.Columns.Add("Asset Type");
        dtforemail.Columns.Add("Asset Make");
        dtforemail.Columns.Add("Asset Model");
        dtforemail.Columns.Add("Serial Number");
        dtforemail.Columns.Add("Processor");
        dtforemail.Columns.Add("RAM");
        dtforemail.Columns.Add("HDD");
        dtforemail.Columns.Add("RFID Tag Id");

        DataTable dtfloorcountonly = new DataTable();
        dtfloorcountonly.Columns.Add("Asset Code");
        dtfloorcountonly.Columns.Add("Site Location");
        dtfloorcountonly.Columns.Add("Floor");
        dtfloorcountonly.Columns.Add("Allocation Date");
        dtfloorcountonly.Columns.Add("Asset Type");
        dtfloorcountonly.Columns.Add("Asset Make");
        dtfloorcountonly.Columns.Add("Asset Model");
        dtfloorcountonly.Columns.Add("Serial Number");
        dtfloorcountonly.Columns.Add("Processor");
        dtfloorcountonly.Columns.Add("RAM");
        dtfloorcountonly.Columns.Add("HDD");
        dtfloorcountonly.Columns.Add("Seat No.");
        dtfloorcountonly.Columns.Add("Identifier Location");
        dtfloorcountonly.Columns.Add("RFID Tag Id");

        Session["ERRORDATA"] = null;
        DataTable dtInvalidData = new DataTable();
        dtInvalidData = dt.Clone();
        dtInvalidData.Columns.Add("STATUS");
        dtInvalidData.Columns.Add("ERROR_MESSAGE");
        int Cnt = 0;
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            oPRP.HostName = (dr.Field<string>("HOST_NAME") == null || dr.Field<string>("HOST_NAME") == "" || dr.Field<string>("HOST_NAME") == string.Empty) ? string.Empty : dr.Field<string>("HOST_NAME").Trim();

            if (dr.Field<string>("ALLOCATION_DATE") == null || dr.Field<string>("ALLOCATION_DATE") == "" || dr.Field<string>("ALLOCATION_DATE") == string.Empty)
                oPRP.AllocationDate = string.Empty;
            else
                oPRP.AllocationDate = dr.Field<string>("ALLOCATION_DATE").Trim();

            if (dr.Field<string>("EXPECTED_RETURN_DATE") == null || dr.Field<string>("EXPECTED_RETURN_DATE") == "" || dr.Field<string>("EXPECTED_RETURN_DATE") == string.Empty)
                oPRP.ExpReturnDate = string.Empty;
            else
                oPRP.ExpReturnDate = dr.Field<string>("EXPECTED_RETURN_DATE").Trim();

            oPRP.Site = (dr.Field<string>("SITE_LOCATION") == null || dr.Field<string>("SITE_LOCATION") == "" || dr.Field<string>("SITE_LOCATION") == string.Empty) ? string.Empty : dr.Field<string>("SITE_LOCATION").Trim().ToUpper();
            oPRP.AssetAllocationTo = (dr.Field<string>("ASSET_ALLOCATION_TO") == null || dr.Field<string>("ASSET_ALLOCATION_TO") == "" || dr.Field<string>("ASSET_ALLOCATION_TO") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_ALLOCATION_TO").Trim().ToUpper(); ;
            oPRP.SubAssetAllocationTo = (dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == null || dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == "" || dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == string.Empty) ? string.Empty : dr.Field<string>("SUB_ASSET_ALLOCATION_TO").Trim().ToUpper();
            oPRP.AllocationType = (dr.Field<string>("ALLOCATION_TYPE") == null || dr.Field<string>("ALLOCATION_TYPE") == "" || dr.Field<string>("ALLOCATION_TYPE") == string.Empty) ? string.Empty : dr.Field<string>("ALLOCATION_TYPE").Trim();
            oPRP.TicketNo = (dr.Field<string>("TICKET_NO") == null || dr.Field<string>("TICKET_NO") == "" || dr.Field<string>("TICKET_NO") == string.Empty) ? string.Empty : dr.Field<string>("TICKET_NO").Trim();
            oPRP.Floor = (dr.Field<string>("FLOOR") == null || dr.Field<string>("FLOOR") == "" || dr.Field<string>("FLOOR") == string.Empty) ? string.Empty : dr.Field<string>("FLOOR").Trim().ToUpper();
            oPRP.SeatNo = (dr.Field<string>("SEAT_NO") == null || dr.Field<string>("SEAT_NO") == "" || dr.Field<string>("SEAT_NO") == string.Empty) ? string.Empty : dr.Field<string>("SEAT_NO").Trim().ToUpper();
            oPRP.EmpCode = (dr.Field<string>("EMPLOYEE_CODE") == null || dr.Field<string>("EMPLOYEE_CODE") == "" || dr.Field<string>("EMPLOYEE_CODE") == string.Empty) ? string.Empty : dr.Field<string>("EMPLOYEE_CODE").Trim();
            oPRP.NoofDueDate = (dr.Field<string>("NO_OF_DUE_DATE") == null || dr.Field<string>("NO_OF_DUE_DATE") == "" || dr.Field<string>("NO_OF_DUE_DATE") == string.Empty) ? string.Empty : dr.Field<string>("NO_OF_DUE_DATE").Trim();
            oPRP.AssetLocation = (dr.Field<string>("IDENTIFIER_LOCATION") == null || dr.Field<string>("IDENTIFIER_LOCATION") == "" || dr.Field<string>("IDENTIFIER_LOCATION") == string.Empty) ? string.Empty : dr.Field<string>("IDENTIFIER_LOCATION").Trim();
            oPRP.SerialCode = (dr.Field<string>("SERIAL_NUMBER") == null || dr.Field<string>("SERIAL_NUMBER") == "" || dr.Field<string>("SERIAL_NUMBER") == string.Empty) ? string.Empty : dr.Field<string>("SERIAL_NUMBER").Trim();
            oPRP.MeetingRoom = (dr.Field<string>("MEETING_ROOM") == null || dr.Field<string>("MEETING_ROOM") == "" || dr.Field<string>("MEETING_ROOM") == string.Empty) ? string.Empty : dr.Field<string>("MEETING_ROOM").Trim().ToUpper();

            oPRP.AssetCode = oDAL.GetAssetCode(oPRP.SerialCode, Session["COMPANY"].ToString());
            oPRP.AssetTagID = oDAL.GetRFIDTag(oPRP.SerialCode, Session["COMPANY"].ToString());

            if (oPRP.AssetAllocationTo == "EMPLOYEE")
            {
                oPRP.Site = oDAL.GetAssetSiteLocation(oPRP.AssetCode, Session["COMPANY"].ToString());
                oPRP.Floor = oDAL.GetAssetFloorLocation(oPRP.AssetCode, Session["COMPANY"].ToString());
            }

            oPRP.CompCode = Convert.ToString(Session["COMPCODE"]);
            oPRP.CreatedBy = Convert.ToString(Session["CURRENTUSER"]);

            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.AssetAllocated = true;

            oPRP.AllocatedTo = string.Empty; // oprp.floor
            oPRP.AllocatedToId = string.Empty; // oprp.floor

            string _date = "";
            if (!String.IsNullOrEmpty(oPRP.ExpReturnDate))
            {
                if (isValidateDate(oPRP.ExpReturnDate.Trim().Replace(" 00:00", "").Trim(), out _date))
                {
                    oPRP.ExpReturnDate = _date;
                }
            }
            if (isValidateDate(oPRP.AllocationDate.Trim().Replace(" 00:00", "").Trim(), out _date))
            {
                oPRP.AllocationDate = _date;
            }

            if (oPRP.AssetAllocationTo == "EMPLOYEE")
            {
                oPRP.MeetingRoom = string.Empty;
                oPRP.Floor = string.Empty;
                oPRP.Store = string.Empty;
                if (oPRP.AllocationType == "Permanent")
                {
                    oPRP.NoofDueDate = "0";
                    oPRP.SeatNo = string.Empty;
                    oPRP.IdentifierLocation = string.Empty;
                    oPRP.ExpReturnDate = string.Empty;
                }
                else
                {
                    oPRP.SeatNo = string.Empty;
                    oPRP.IdentifierLocation = string.Empty;
                }
            }
            else
            {
                oPRP.EmpCode = string.Empty;
                oPRP.EmpName = string.Empty;
                oPRP.EmpTagID = string.Empty;
                if (oPRP.AllocationType == "Permanent")
                {
                    oPRP.NoofDueDate = "0";
                    oPRP.IdentifierLocation = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(oPRP.EmpCode))
            {
                //DataTable de = oDAL.GetEmpDetails(oPRP.EmpCode);
                DataTable de = GetEmployeeDetailsforBulkUpload(oPRP.EmpCode);
                if (de.Rows.Count > 0)
                {
                    oPRP.EmpTagID = de.Rows[0]["EMP_TAG"].ToString().Trim();
                    oPRP.EmailID = de.Rows[0]["EMP_EMAIL"].ToString().Trim();
                    oPRP.EmpName = de.Rows[0]["EMPLOYEE_NAME"].ToString().Trim();
                    oPRP.Designation = de.Rows[0]["Designation"].ToString().Trim();
                    oPRP.Process = de.Rows[0]["ProcessName"].ToString().Trim();
                    oPRP.SubLOB = de.Rows[0]["SubLOB"].ToString().Trim();
                    oPRP.LOB = de.Rows[0]["Lob"].ToString().Trim();
                    oPRP.AllocatedToId = oPRP.EmpCode;
                    oPRP.AllocatedTo = oPRP.EmpName;
                }
            }

            if (oPRP.Status == "" || oPRP.Status == null)
            {
                oPRP.Status = "ALLOCATED";
            }

            oPRP.SubStatus = oDAL.GetAsseTSubStatus(oPRP.AssetCode, Session["COMPANY"].ToString());

            if (oPRP.SubAssetAllocationTo == "CAFETERIA" || oPRP.SubAssetAllocationTo == "PHONE BOOTH")
            {
                if (oPRP.MeetingRoom == null || oPRP.MeetingRoom == string.Empty || oPRP.MeetingRoom == "")
                {
                    oPRP.MeetingRoom = "";
                }
            }
            bool isAssetCodeExists = oDAL.CheckAssetCode(oPRP.AssetCode);
            if (isAssetCodeExists)
            {
                i++;
                dtInvalidData.Rows.Add(dr.ItemArray);
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "AssetCode Exist Already";
                continue;
            }
            else
            {
                //bool bResp = oDAL.SaveAssetAllocation("ALLOCATE", oPRP);
                string bResp = oDAL.SaveAssetAllocationSP(oPRP);
                if (bResp.Contains("SUCCESS"))
                {
                    if (oPRP.AssetAllocationTo.ToString().ToUpper() == "EMPLOYEE")
                    {
                        string EmailId = oDAL.GetEmpEmail(oPRP.EmpCode);
                        
                        DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                        string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                        string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                        string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                        string AssetProcessor = (dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == null || dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR").Trim();
                        string AssetHDD = (dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == null || dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_RAM").Trim();
                        string AssetRAM = (dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == null || dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_HDD").Trim();
                        string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                        dtforemail.Rows.Add(oPRP.AssetCode,oPRP.Site,oPRP.AllocationDate,oPRP.EmpName,oPRP.EmpCode,EmailId, AssetType,AssetMake,AssetModel,oPRP.SerialCode,AssetProcessor, AssetHDD,AssetRAM, RFIDTag);
                        dtforemail.AcceptChanges();
                    }
                    else
                    {
                        DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                        string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                        string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                        string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                        string AssetProcessor = (dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == null || dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_PROCESSOR").Trim();
                        string AssetHDD = (dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == null || dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_RAM") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_RAM").Trim();
                        string AssetRAM = (dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == null || dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_HDD") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_HDD").Trim();
                        string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                        dtfloorcountonly.Rows.Add(oPRP.AssetCode, oPRP.Site,oPRP.Floor, oPRP.AllocationDate, AssetType, AssetMake, AssetModel, oPRP.SerialCode, AssetProcessor, AssetHDD, AssetRAM,oPRP.SeatNo,oPRP.AssetLocation, RFIDTag);
                        dtfloorcountonly.AcceptChanges();
                    }

                    Cnt++;
                    continue;
                }
                else
                {
                    dtInvalidData.Rows.Add(dr.ItemArray);
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "Not Saved";
                    i++;
                    continue;
                }

            }
        }
        if (Cnt > 0)
        {
            if(dtforemail.Rows.Count>0)
            {
                DataTable dp1 = oDAL.GetMailTransactionDetails("ASSET_ALLOCATION", Convert.ToString(Session["COMP_NAME"]));
                string Subject = "Assets Allocated";
                string MailBody = "";
                string CCAddress = "";
                if (dp1.Rows.Count > 0)
                {
                    CCAddress = (dp1.Rows[0].Field<string>("CC_MAIL_ID") == null || dp1.Rows[0].Field<string>("CC_MAIL_ID") == "" || dp1.Rows[0].Field<string>("CC_MAIL_ID") == string.Empty) ? "" : dp1.Rows[0].Field<string>("CC_MAIL_ID").Trim();
                    Subject = (dp1.Rows[0].Field<string>("MAIL_SUBJECT") == null || dp1.Rows[0].Field<string>("MAIL_SUBJECT") == "" || dp1.Rows[0].Field<string>("MAIL_SUBJECT") == string.Empty) ? "" : dp1.Rows[0].Field<string>("MAIL_SUBJECT").Trim();
                    MailBody = (dp1.Rows[0].Field<string>("MAIL_BODY") == null || dp1.Rows[0].Field<string>("MAIL_BODY") == "" || dp1.Rows[0].Field<string>("MAIL_BODY") == string.Empty) ? "" : dp1.Rows[0].Field<string>("MAIL_BODY").Trim();
                }
                var dataRows = dtforemail.AsEnumerable().GroupBy(row => new {
                    EmailId = row.Field<string>("Emp Email")
                }).Select(G => G.First()).CopyToDataTable(); foreach (DataRow row in dataRows.Rows)
                {
                    var assetRows = dtforemail.Select("[Emp Email]='"+Convert.ToString(row["Emp Email"])+"'");
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        sendmail.FunctionSendingAqcuisitionMail(assetRows.CopyToDataTable(), Convert.ToString(row["Emp Email"]).Trim(), CCAddress, Subject, MailBody);
                    }
                    catch (Exception ee)
                    {
                        clsGeneral.LogErrorToLogFile(ee, "Employee Asset Allocation");
                        clsGeneral.LogErrorToLogFile(ee.InnerException, "Employee Asset Allocation");
                    }
                }
            }
            //dtfloorcountonly - this datatable contains Floor related assetcode and Serial Code for 
            if (dtfloorcountonly.Rows.Count > 0)
            {
                DataTable dp = oDAL.GetMailTransactionDetails("ASSET_ALLOCATION", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
                {
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        sendmail.FunctionSendingMailWithAssetData(dtfloorcountonly, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    }
                    catch (Exception ex)
                    {
                        clsGeneral.LogErrorToLogFile(ex, "Floor Asset Allocation");
                        clsGeneral.LogErrorToLogFile(ex.InnerException, "Floor Asset Allocation");
                    }
                }
            }

        }
        if (i == 0)
        {
            Session["ERRORDATA"] = null;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
        }
        else
        {
            btnExport.Visible = true;
            btnExport.Enabled = true;
            Session["ERRORDATA"] = dtInvalidData;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Some Of The Rows Are Not Inserted Please Download The Report To Find The Error');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('File Containing Data is not Correct, For Further information Please click Export Button');", true);
        }
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
    }
    private Tuple<bool, string> ValidateStoreFile(DataTable dt)
    {
        bool res = true;
        string msg = "success";
        string[] AllocationTo = new string[] { "FLOOR" };
        string[] SubAllocationTo = new string[] { "MEETING ROOM", "PHONE BOOTH", "TRAINING ROOM", "CAFETERIA" };
        string[] AllocationType = new string[] { "Permanent", "Temporary" };
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            i++;
            oPRP.Asset_FAR_TAG = (dr.Field<string>("ASSET_FAR_TAG") == null || dr.Field<string>("ASSET_FAR_TAG") == "" || dr.Field<string>("ASSET_FAR_TAG") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_FAR_TAG").Trim();
            oPRP.AssetAllocationTo = (dr.Field<string>("ASSET_ALLOCATION_TO") == null || dr.Field<string>("ASSET_ALLOCATION_TO") == "" || dr.Field<string>("ASSET_ALLOCATION_TO") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_ALLOCATION_TO").Trim().ToUpper();
            oPRP.SubAssetAllocationTo = (dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == null || dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == "" || dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == string.Empty) ? string.Empty : dr.Field<string>("SUB_ASSET_ALLOCATION_TO").Trim().ToUpper();
            oPRP.AllocationType = (dr.Field<string>("ALLOCATION_TYPE") == null || dr.Field<string>("ALLOCATION_TYPE") == "" || dr.Field<string>("ALLOCATION_TYPE") == string.Empty) ? string.Empty : dr.Field<string>("ALLOCATION_TYPE").Trim();
            oPRP.Site = (dr.Field<string>("SITE_LOCATION") == null || dr.Field<string>("SITE_LOCATION") == "" || dr.Field<string>("SITE_LOCATION") == string.Empty) ? string.Empty : dr.Field<string>("SITE_LOCATION").Trim().ToUpper();
            oPRP.Floor = (dr.Field<string>("FLOOR") == null || dr.Field<string>("FLOOR") == "" || dr.Field<string>("FLOOR") == string.Empty) ? string.Empty : dr.Field<string>("FLOOR").Trim().ToUpper();
            oPRP.MeetingRoom = (dr.Field<string>("MEETING_ROOM") == null || dr.Field<string>("MEETING_ROOM") == "" || dr.Field<string>("MEETING_ROOM") == string.Empty) ? string.Empty : dr.Field<string>("MEETING_ROOM").Trim().ToUpper();
            oPRP.NoofDueDate = "0";
            oPRP.AssetLocation = (dr.Field<string>("IDENTIFIER_LOCATION") == null || dr.Field<string>("IDENTIFIER_LOCATION") == "" || dr.Field<string>("IDENTIFIER_LOCATION") == string.Empty) ? string.Empty : dr.Field<string>("IDENTIFIER_LOCATION").Trim();

            if (dr.Field<string>("ALLOCATION_DATE") == null || dr.Field<string>("ALLOCATION_DATE") == "" || dr.Field<string>("ALLOCATION_DATE") == string.Empty)
                oPRP.AllocationDate = string.Empty;
            else
                oPRP.AllocationDate = dr.Field<string>("ALLOCATION_DATE").Trim();

            oPRP.AssetCode = oDAL.GetAssetCode(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
            oPRP.AssetTagID = oDAL.GetRFIDTag(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
            oPRP.ExpReturnDate = string.Empty;
            oPRP.EmpCode = string.Empty;
            oPRP.TicketNo = string.Empty;
            oPRP.Store = string.Empty;
            oPRP.SeatNo = string.Empty;

            if (string.IsNullOrEmpty(oPRP.AssetCode) || oPRP.AssetCode == "")
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Asset Far Tag.";
                res = false;
                break;
            }
            //DataTable dy = oDAL.ValidateAssetDetailsForAllocation(oPRP.AssetCode.Trim(), oPRP.AssetTagID.Trim());
            //if (dy.Rows.Count == 0)
            //{
            //    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Far Tag.";
            //    res = false;
            //    break;
            //}

            if (oPRP.AllocationType.Trim() == "" ||
                oPRP.AssetAllocationTo.Trim() == "" ||
                oPRP.AllocationDate.Trim() == "" ||
                oPRP.Asset_FAR_TAG.Trim() == "" || oPRP.Site == ""
                )
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Please enter the mandatory fields.";
                res = false;
                break;
            }

            string _date = "";

            if (!isValidateDate(oPRP.AllocationDate.Trim().Replace(" 00:00", "").Trim(), out _date))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Allocation Date.";
                res = false;
                break;
            }

            if (!AllocationTo.Contains(oPRP.AssetAllocationTo))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Allocation To.";
                res = false;
                break;
            }
            if (!string.IsNullOrEmpty(oPRP.SubAssetAllocationTo))
            {
                if (!SubAllocationTo.Contains(oPRP.SubAssetAllocationTo))
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Sub Allocation To.";
                    res = false;
                    break;
                }
            }

            if (!AllocationType.Contains(oPRP.AllocationType))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Allocation Type.";
                res = false;
                break;
            }
            if (oPRP.AssetAllocationTo == "EMPLOYEE" && (oPRP.EmpCode.Trim() == ""))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Please mention employee code, if Allocation to is employee.";
                res = false;
                break;
            }
            if (!String.IsNullOrEmpty(oPRP.EmpCode))
            {
                DataTable de = oDAL.GetEmpDetails(oPRP.EmpCode);
                if (de.Rows.Count == 0)
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid employee code.";
                    res = false;
                    break;
                }
            }

            if (oPRP.AssetAllocationTo == "FLOOR" && (oPRP.Floor.Trim() == ""))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Please mention floor, if Allocation to is floor type.";
                res = false;
                break;
            }
            if (oPRP.AssetAllocationTo == "FLOOR")
            {
                if (oPRP.Site.Trim().ToString().ToUpper() != oDAL.GetAcquisitionLocation(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString()).ToUpper())
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " The location you have entered is not matching with original Asset Location.";
                    res = false;
                    break;
                }
            }
            var dtSLocation = oDAL.GetLocation(oPRP.Site, Session["COMPANY"].ToString());
            if (dtSLocation.Rows.Count <= 0)
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Asset Location is invalid.";
                res = false;
                break;
            }

            if (!String.IsNullOrEmpty(oPRP.Floor.Trim()))
            {
                DataTable de = oDAL.ValidateFloorLocation(oPRP.Site.Trim(), oPRP.Floor.Trim(), Session["COMPANY"].ToString());
                if (de.Rows.Count == 0)
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Site and Floor Combination.";
                    res = false;
                    break;
                }
            }
            if (oPRP.SubAssetAllocationTo == "CAFETERIA" || oPRP.SubAssetAllocationTo == "PHONE BOOTH")
            {
                if (oPRP.MeetingRoom == null || oPRP.MeetingRoom == string.Empty || oPRP.MeetingRoom == "")
                {
                    oPRP.MeetingRoom = "";
                }
                else
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Value in Meeting Room.";
                    res = false;
                    break;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oPRP.MeetingRoom.Trim()))
                {
                    var dtMeetingRoom = oDAL.GetMeetingRoomwithFloorandSite(oPRP.Site, oPRP.Floor, oPRP.SubAssetAllocationTo, oPRP.MeetingRoom, Session["COMPANY"].ToString()); //GetStorewithFloorandSite
                    if (dtMeetingRoom.Rows.Count <= 0)
                    {
                        msg = "Please Note : Row Number " + (i + 1).ToString() + " Meeting Room is invalid.";
                        res = false;
                        break;
                    }
                }
            }

            if (dr.Field<string>("REMARKS") == "" || dr.Field<string>("REMARKS") == string.Empty || dr.Field<string>("REMARKS") == null)
            {
                oPRP.AllocationRemarks = "NA";
            }
            else
            {
                oPRP.AllocationRemarks = dr.Field<string>("REMARKS");
            }

        }
        return new Tuple<bool, string>(res, msg);
    }
    private void SaveFacilitiesAssetAllocation(DataTable dt)
    {
        makeEmptyallFields();
        Session["ERRORDATA"] = null;

        DataTable dtforemail = new DataTable();
        dtforemail.Columns.Add("Asset Code");
        dtforemail.Columns.Add("Site Location");
        dtforemail.Columns.Add("Floor");
        dtforemail.Columns.Add("Allocation Date");
        dtforemail.Columns.Add("Asset Type");
        dtforemail.Columns.Add("Asset Make");
        dtforemail.Columns.Add("Asset Model");
        dtforemail.Columns.Add("Asset Far Tag");
        dtforemail.Columns.Add("RFID Tag Id");

        DataTable dtInvalidData = new DataTable();
        dtInvalidData = dt.Clone();
        dtInvalidData.Columns.Add("STATUS");
        dtInvalidData.Columns.Add("ERROR_MESSAGE");
        int Cnt = 0;
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            oPRP.ExpReturnDate = string.Empty;
            oPRP.Asset_FAR_TAG = (dr.Field<string>("ASSET_FAR_TAG") == null || dr.Field<string>("ASSET_FAR_TAG") == "" || dr.Field<string>("ASSET_FAR_TAG") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_FAR_TAG").Trim();
            oPRP.AssetAllocationTo = (dr.Field<string>("ASSET_ALLOCATION_TO") == null || dr.Field<string>("ASSET_ALLOCATION_TO") == "" || dr.Field<string>("ASSET_ALLOCATION_TO") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_ALLOCATION_TO").Trim().ToUpper();
            oPRP.SubAssetAllocationTo = (dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == null || dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == "" || dr.Field<string>("SUB_ASSET_ALLOCATION_TO") == string.Empty) ? string.Empty : dr.Field<string>("SUB_ASSET_ALLOCATION_TO").Trim().ToUpper();
            oPRP.AllocationType = (dr.Field<string>("ALLOCATION_TYPE") == null || dr.Field<string>("ALLOCATION_TYPE") == "" || dr.Field<string>("ALLOCATION_TYPE") == string.Empty) ? string.Empty : dr.Field<string>("ALLOCATION_TYPE").Trim();
            oPRP.Site = (dr.Field<string>("SITE_LOCATION") == null || dr.Field<string>("SITE_LOCATION") == "" || dr.Field<string>("SITE_LOCATION") == string.Empty) ? string.Empty : dr.Field<string>("SITE_LOCATION").Trim().ToUpper();
            oPRP.Floor = (dr.Field<string>("FLOOR") == null || dr.Field<string>("FLOOR") == "" || dr.Field<string>("FLOOR") == string.Empty) ? string.Empty : dr.Field<string>("FLOOR").Trim().ToUpper();
            oPRP.MeetingRoom = (dr.Field<string>("MEETING_ROOM") == null || dr.Field<string>("MEETING_ROOM") == "" || dr.Field<string>("MEETING_ROOM") == string.Empty) ? string.Empty : dr.Field<string>("MEETING_ROOM").Trim().ToUpper();
            oPRP.NoofDueDate = "0";
            oPRP.AssetLocation = (dr.Field<string>("IDENTIFIER_LOCATION") == null || dr.Field<string>("IDENTIFIER_LOCATION") == "" || dr.Field<string>("IDENTIFIER_LOCATION") == string.Empty) ? string.Empty : dr.Field<string>("IDENTIFIER_LOCATION").Trim();

            if (dr.Field<string>("ALLOCATION_DATE") == null || dr.Field<string>("ALLOCATION_DATE") == "" || dr.Field<string>("ALLOCATION_DATE") == string.Empty)
                oPRP.AllocationDate = string.Empty;
            else
                oPRP.AllocationDate = dr.Field<string>("ALLOCATION_DATE").Trim();

            oPRP.AssetCode = oDAL.GetAssetCode(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
            oPRP.AssetTagID = oDAL.GetRFIDTag(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());

            oPRP.CompCode = Convert.ToString(Session["COMPCODE"]);
            oPRP.CreatedBy = Convert.ToString(Session["CURRENTUSER"]);

            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.AssetAllocated = true;

            oPRP.AllocatedTo = string.Empty; //oPRP.Store;
            oPRP.AllocatedToId = string.Empty; //oPRP.Store;
            oPRP.EmpCode = string.Empty;
            if (!string.IsNullOrEmpty(oPRP.EmpCode))
            {
                DataTable de = oDAL.GetEmpDetails(oPRP.EmpCode);
                if (de.Rows.Count > 0)
                {
                    oPRP.EmpTagID = de.Rows[0]["EMP_TAG"].ToString().Trim();
                    oPRP.EmailID = de.Rows[0]["EMP_EMAIL"].ToString().Trim();
                    oPRP.EmpName = de.Rows[0]["EMPLOYEE_NAME"].ToString().Trim();
                    oPRP.Designation = de.Rows[0]["Designation"].ToString().Trim();
                    oPRP.Process = de.Rows[0]["ProcessName"].ToString().Trim();
                    oPRP.SubLOB = de.Rows[0]["SubLOB"].ToString().Trim();
                    oPRP.LOB = de.Rows[0]["Lob"].ToString().Trim();
                    oPRP.AllocatedToId = oPRP.EmpCode;
                    oPRP.AllocatedTo = oPRP.EmpName;
                }
            }

            if (oPRP.Status == "" || oPRP.Status == null || string.IsNullOrEmpty(oPRP.Status))
            {
                oPRP.Status = "ALLOCATED";
            }
            if (dr.Field<string>("MEETING_ROOM") == "" || dr.Field<string>("MEETING_ROOM") == null || dr.Field<string>("MEETING_ROOM") == string.Empty)
            {
                oPRP.MeetingRoom = "";
            }
            if (oPRP.SubAssetAllocationTo == "CAFETERIA" || oPRP.SubAssetAllocationTo == "PHONE BOOTH")
            {
                oPRP.MeetingRoom = "";
            }
            string _date = "";
            if (isValidateDate(oPRP.AllocationDate.Trim().Replace(" 00:00", "").Trim(), out _date))
            {
                oPRP.AllocationDate = _date;
            }

            oPRP.SubStatus = oDAL.GetAsseTSubStatus(oPRP.AssetCode, Session["COMPANY"].ToString());

            bool isAssetCodeExists = oDAL.CheckAssetCode(oPRP.AssetCode);
            if (isAssetCodeExists)
            {
                i++;
                dtInvalidData.Rows.Add(dr.ItemArray);
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "AssetCode Exist Already";
                continue;
            }
            else
            {
                //bool bResp = oDAL.SaveAssetAllocation("ALLOCATE", oPRP);
                string bResp = oDAL.SaveAssetAllocationSP(oPRP);
                if (bResp.Contains("SUCCESS"))
                {
                    DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                    string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                    string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                    string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                    string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                    dtforemail.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.Floor, oPRP.AllocationDate, AssetType, AssetMake, AssetModel, oPRP.Asset_FAR_TAG, RFIDTag);
                    dtforemail.AcceptChanges();

                    Cnt++;
                    continue;
                }
                else
                {
                    i++;
                    dtInvalidData.Rows.Add(dr.ItemArray);
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                    dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "Not Saved";
                    continue;
                }

            }

        }
        if (Cnt > 0)
        {
            //dtforemail - this datatable contains Floor related assetcode and Asset Far Tag
            DataTable dp = oDAL.GetMailTransactionDetails("ASSET_ALLOCATION", Convert.ToString(Session["COMP_NAME"]));
            if (dp.Rows.Count > 0)
            {
                try
                {
                    SendmailAlert sendmail = new SendmailAlert();
                    sendmail.FunctionSendingMailWithAssetData(dtforemail, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                }
                catch (Exception ee)
                {

                }
            }
        }
        if (i == 0)
        {
            Session["ERRORDATA"] = null;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Asset details are updated  " + Cnt + " out of " + dt.Rows.Count + ".');", true);
        }
        else
        {
            btnExport.Visible = true;
            btnExport.Enabled = true;
            Session["ERRORDATA"] = dtInvalidData;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Some Of The Rows Are Not Inserted Please Download The Report To Find The Error');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", "ShowAlert('File Containing Data is not Correct, For Further information Please click Export Button');", true);
        }

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
    private void GetEmployeeDetails(string empid)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        DataTable dt =  ADSearch.GetEmpDetails(Session["LDAPUser"].ToString(), Session["LDAPPassword"].ToString(), empid, Session["COMPANY"].ToString());
        dt = oDAL.GetEmpDetails(empid);
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Visible = true;
            }

    }
    private DataTable GetEmployeeDetailsforBulkUpload(string empid)
    {
        DataTable dt  = ADSearch.GetEmpDetails(Session["LDAPUser"].ToString(), Session["LDAPPassword"].ToString(), empid, Session["COMPANY"].ToString());
            dt = oDAL.GetEmpDetails(empid);
        return dt;
    }
    protected void btnGet_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
            if (txtEmployeeID.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Enter Employee ID and search');", true);
                return;
            }
            else
            {
                GetEmployeeDetails(txtEmployeeID.Text.Trim());
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }
    protected void btnAllSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlAllocTo.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Select Allocation To Value');", true);
                return;
            }
            else
            {
                if (ddlAllocTo.SelectedItem.Text.ToString().ToUpper() == "FLOOR")
                {
                    if (ddlSiteLocation.SelectedIndex == 0 && ddlSiteLocationFilter.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Select Site Location & Location Filter');", true);
                        return;
                    }
                    else if (ddlSiteLocation.SelectedValue != ddlSiteLocationFilter.SelectedValue)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Both the Locations should be same when Allocation to is FLOOR');", true);
                        return;
                    }
                    ddlAllocTo.Enabled = ddlSiteLocation.Enabled = ddlSiteLocationFilter.Enabled = false;
                }
            }

            string AssetMake = txtAssetMake.Text.Trim() != "" ? txtAssetMake.Text.Trim() : string.Empty;
            string AssetModel = txtAssetModel.Text.Trim() != "" ? txtAssetModel.Text.Trim() : string.Empty;
            string SerialNo = txtAssetSerialNo.Text.Trim() != "" ? txtAssetSerialNo.Text.Trim() : string.Empty;
            string AssetType = txtAssetType.Text.Trim() != "" ? txtAssetType.Text.Trim() : string.Empty;
            string SiteLocation = string.Empty;
            if (ddlSiteLocationFilter.SelectedIndex == 0 || ddlSiteLocationFilter.SelectedValue == "ALL")
                SiteLocation = string.Empty;
            else
                SiteLocation = ddlSiteLocationFilter.SelectedValue;
            string FilterStatus = txtAllocationStatus.Text.Trim() != "" ? txtAllocationStatus.Text.Trim() : string.Empty;
            string filterSubStatus = txtAllocationSubStatus.Text.Trim() != "" ? txtAllocationSubStatus.Text.Trim() : string.Empty;
            string FilterAssetFarTag = txtAssetFarTag.Text.Trim() != "" ? txtAssetFarTag.Text.Trim() : string.Empty;
            string FilterAssetDomain = txtAssetDomain.Text.Trim() != "" ? txtAssetDomain.Text.Trim() : string.Empty;
            string FilterAssetHDD = txtAssetHDD.Text.Trim() != "" ? txtAssetHDD.Text.Trim() : string.Empty;
            string FilterAssetProcessor = txtAssetProcessor.Text.Trim() != "" ? txtAssetProcessor.Text.Trim() : string.Empty;
            string FilterAssetRAM = txtAssetRAM.Text.Trim() != "" ? txtAssetRAM.Text.Trim() : string.Empty;
            string FilterAssetPONumber = txtPONumber.Text.Trim() != "" ? txtPONumber.Text.Trim() : string.Empty;
            string FilterAssetVendor = txtAssetVendor.Text.Trim() != "" ? txtAssetVendor.Text.Trim() : string.Empty;
            string FilterAssetInvoiceNumber = txtAssetInvoiceNo.Text.Trim() != "" ? txtAssetInvoiceNo.Text.Trim() : string.Empty;
            string FilterAssetRFIDTag = txtRFIDTag.Text.Trim() != "" ? txtRFIDTag.Text.Trim() : string.Empty;
            string FilterAssetGRNNo = txtGRNNumber.Text.Trim() != "" ? txtGRNNumber.Text.Trim() : string.Empty;
            oPRP.CompCode = Session["COMPANY"].ToString();
            DataTable dt = new DataTable();
            dt = oDAL.GetAllSearchAssetDetails(AssetMake, AssetModel, AssetType, SiteLocation, SerialNo, "Allocation", oPRP.CompCode, FilterStatus, filterSubStatus, FilterAssetFarTag, FilterAssetDomain, FilterAssetHDD, FilterAssetProcessor, FilterAssetRAM, FilterAssetPONumber, FilterAssetVendor, FilterAssetInvoiceNumber, FilterAssetRFIDTag, FilterAssetGRNNo);
            //dt = oDAL.GetAllSearchAssetDetails(AssetMake, AssetModel, AssetType, SiteLocation, SerialNo, "STOCK", oPRP.CompCode);
            if (dt.Rows.Count > 0)
            {
                GridView2.DataSource = dt;
                GridView2.DataBind();
                //GridView2.Columns[0]["ASSET_FAR_TAG"].Visible = false;
                if (Session["COMPANY"].ToString() == "IT")
                {
                    GridView2.Columns[9].Visible = false;
                }
                else
                {
                    GridView2.Columns[5].Visible = false;
                    GridView2.Columns[12].Visible = false;
                    GridView2.Columns[13].Visible = false;
                    GridView2.Columns[14].Visible = false;
                }
                GridView2.Visible = true;
                btnSubmit.Enabled = true;
                btnSubmit.Visible = true;
            }
            else
            {
                GridView2.DataSource = null;
                GridView2.DataBind();
                GridView2.Visible = false;
                btnSubmit.Enabled = false;
                btnSubmit.Visible = false;
                ddlSiteLocation.Enabled = ddlSiteLocationFilter.Enabled = ddlAllocTo.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Invalid search filters or assets are assigned.');", true);
                return;
            }

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //try
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow &&
        //       (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        //    {
        //        CheckBox chkSelect = (CheckBox)e.Row.Cells[0].FindControl("chkSelectAsset");
        //        CheckBox chkHSelect = (CheckBox)this.GridView2.HeaderRow.FindControl("chkHSelect");
        //        chkSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkHSelect.ClientID);
        //    }
        //}
        //catch (Exception ex)
        //{ HandleExceptions(ex); }
    }
    protected void GridView3_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkSubGridSelect = (CheckBox)e.Row.Cells[0].FindControl("chkSubGridSelectAsset");
                CheckBox chkHSubGridSelect = (CheckBox)this.GridView3.HeaderRow.FindControl("chkHSubGridSelect");
                chkSubGridSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkHSubGridSelect.ClientID);
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void OnCheckedHeaderChanged(object sender, EventArgs e)
    {
        int i = 0;
        int Headerverify = 0;
        CheckBox CheckBoxHeader = (GridView2.HeaderRow.FindControl("chkHSelect") as CheckBox);
        foreach (GridViewRow row in GridView2.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CheckBox = (CheckBox)row.FindControl("chkSelectAsset");
                CheckBox.Checked = CheckBoxHeader.Checked;
                Label lblStatus = (Label)row.FindControl("lblAs");
                Label lblSubStatus = (Label)row.FindControl("lblAss");
                if (lblStatus.Text.ToString().ToUpper() == "STOCK" && lblSubStatus.Text.ToString().ToUpper() == "WORKING")
                {
                    if (CheckBox.Checked)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ASSET_CODE");
                        dt.Columns.Add("Status");
                        dt.Columns.Add("ASSET_SUB_STATUS");
                        dt.Columns.Add("SERIAL_CODE");
                        dt.Columns.Add("ASSET_ID");
                        dt.Columns.Add("Tag_ID");
                        dt.Columns.Add("ASSET_MAKE");
                        dt.Columns.Add("ASSET_FAR_TAG");
                        dt.Columns.Add("MODEL_NAME");
                        dt.Columns.Add("ASSET_TYPE");
                        dt.Columns.Add("ASSET_HDD");
                        dt.Columns.Add("ASSET_RAM");
                        dt.Columns.Add("ASSET_PROCESSOR");

                        string AssetCode = ((Label)row.FindControl("lblAssetCode")).Text.Trim();
                        string status = ((Label)row.FindControl("lblAs")).Text.Trim();
                        string SubStatus = ((Label)row.FindControl("lblAss")).Text.Trim();
                        string SerialNo = ((Label)row.FindControl("lblSrno")).Text.Trim();
                        string AssetTag = ((Label)row.FindControl("lblAssettag")).Text.Trim();
                        string RFIDTag = ((Label)row.FindControl("lblRfTag")).Text.Trim();
                        string assetmake = ((Label)row.FindControl("lblmk")).Text.Trim();
                        string AssetFarTag = ((Label)row.FindControl("lblFarTag")).Text.Trim();
                        string assetmodel = ((Label)row.FindControl("lblam")).Text.Trim();
                        string assettype = ((Label)row.FindControl("lblat")).Text.Trim();
                        string AssetHDD = ((Label)row.FindControl("lblHdd")).Text.Trim();
                        string AssetRAM = ((Label)row.FindControl("lblRam")).Text.Trim();
                        string AssetProcessor = ((Label)row.FindControl("lblProcessor")).Text.Trim();

                        dt.Rows.Add(AssetCode, status, SubStatus, SerialNo, AssetTag, RFIDTag, assetmake, AssetFarTag, assetmodel, assettype, AssetHDD, AssetRAM, AssetProcessor);
                        dt.AcceptChanges();
                        if (GridView3.Rows.Count == 0)
                        {
                            GridView3.DataSource = dt;
                            GridView3.DataBind();
                            GridView3.Visible = true;

                            if (Session["COMPANY"].ToString() == "IT")
                            {
                                GridView3.Columns[9].Visible = false;
                            }
                            else
                            {
                                GridView3.Columns[5].Visible = false;
                                GridView3.Columns[12].Visible = false;
                                GridView3.Columns[13].Visible = false;
                                GridView3.Columns[14].Visible = false;
                            }
                        }
                        else
                        {
                            int igridverify = 0;
                            DataTable dtgrid = new DataTable();
                            dtgrid.Columns.Add("ASSET_CODE");
                            dtgrid.Columns.Add("Status");
                            dtgrid.Columns.Add("ASSET_SUB_STATUS");
                            dtgrid.Columns.Add("SERIAL_CODE");
                            dtgrid.Columns.Add("ASSET_ID");
                            dtgrid.Columns.Add("Tag_ID");
                            dtgrid.Columns.Add("ASSET_MAKE");
                            dtgrid.Columns.Add("ASSET_FAR_TAG");
                            dtgrid.Columns.Add("MODEL_NAME");
                            dtgrid.Columns.Add("ASSET_TYPE");
                            dtgrid.Columns.Add("ASSET_HDD");
                            dtgrid.Columns.Add("ASSET_RAM");
                            dtgrid.Columns.Add("ASSET_PROCESSOR");

                            foreach (GridViewRow gvRowdt in GridView3.Rows)
                            {
                                string AssetCode1 = ((Label)gvRowdt.FindControl("lblAssetCode")).Text.Trim();
                                string status1 = ((Label)gvRowdt.FindControl("lblAs")).Text.Trim();
                                string SubStatus1 = ((Label)gvRowdt.FindControl("lblAss")).Text.Trim();
                                string SerialNo1 = ((Label)gvRowdt.FindControl("lblSrno")).Text.Trim();
                                string AssetTag1 = ((Label)gvRowdt.FindControl("lblAssettag")).Text.Trim();
                                string RFIDTag1 = ((Label)gvRowdt.FindControl("lblRfTag")).Text.Trim();
                                string assetmake1 = ((Label)gvRowdt.FindControl("lblmk")).Text.Trim();
                                string AssetFarTag1 = ((Label)gvRowdt.FindControl("lblFarTag")).Text.Trim();
                                string assetmodel1 = ((Label)gvRowdt.FindControl("lblam")).Text.Trim();
                                string assettype1 = ((Label)gvRowdt.FindControl("lblat")).Text.Trim();
                                string AssetHDD1 = ((Label)gvRowdt.FindControl("lblHdd")).Text.Trim();
                                string AssetRAM1 = ((Label)gvRowdt.FindControl("lblRam")).Text.Trim();
                                string AssetProcessor1 = ((Label)gvRowdt.FindControl("lblProcessor")).Text.Trim();
                                dtgrid.Rows.Add(AssetCode1, status1, SubStatus1, SerialNo1, AssetTag1, RFIDTag1, assetmake1, AssetFarTag1, assetmodel1, assettype1, AssetHDD1, AssetRAM1, AssetProcessor1);
                                dtgrid.AcceptChanges();
                            }

                            // dtgrid = (DataTable)GridView3.DataSource;
                            for (i = 0; i < dtgrid.Rows.Count; i++)
                            {
                                if (AssetCode == dtgrid.Rows[i].Field<string>("ASSET_CODE").ToString())
                                {
                                    igridverify++;
                                }
                            }
                            if (igridverify == 0)
                            {
                                dtgrid.Rows.Add(AssetCode, status, SubStatus, SerialNo, AssetTag, RFIDTag, assetmake, AssetFarTag, assetmodel, assettype, AssetHDD, AssetRAM, AssetProcessor);
                                dtgrid.AcceptChanges();
                                GridView3.DataSource = null;
                                GridView3.DataBind();
                                GridView3.DataSource = dtgrid;
                                GridView3.DataBind();
                                GridView3.Visible = true;

                                if (Session["COMPANY"].ToString() == "IT")
                                {
                                    GridView3.Columns[9].Visible = false;
                                }
                                else
                                {
                                    GridView3.Columns[5].Visible = false;
                                    GridView3.Columns[12].Visible = false;
                                    GridView3.Columns[13].Visible = false;
                                    GridView3.Columns[14].Visible = false;
                                }
                            }
                            else
                            {
                                
                            }
                        }
                    }
                }
                else
                {
                    CheckBox.Checked = false;
                }
            }
        }

        foreach (GridViewRow rows in GridView2.Rows)
        {
            CheckBox CheckBox1 = (CheckBox)rows.FindControl("chkSelectAsset");
            if (CheckBox1.Checked == true)
            {
                Headerverify++;
            }
        }
        if (GridView2.Rows.Count == Headerverify)
        {
            CheckBoxHeader.Checked = true;
        }
        else
        {
            CheckBoxHeader.Checked = false;
        }
    }
    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox ddlAgent = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ddlAgent.NamingContainer;
        int i = 0;
        int Headerverify = 0;
        if (row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckBox = (CheckBox)row.FindControl("chkSelectAsset");
            Label lblStatus = (Label)row.FindControl("lblAs");
            Label lblSubStatus = (Label)row.FindControl("lblAss");
            if (lblStatus.Text.ToString().ToUpper() == "STOCK" && lblSubStatus.Text.ToString().ToUpper() == "WORKING")
            {
                if (CheckBox.Checked)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ASSET_CODE");
                    dt.Columns.Add("Status");
                    dt.Columns.Add("ASSET_SUB_STATUS");
                    dt.Columns.Add("SERIAL_CODE");
                    dt.Columns.Add("ASSET_ID");
                    dt.Columns.Add("Tag_ID");
                    dt.Columns.Add("ASSET_MAKE");
                    dt.Columns.Add("ASSET_FAR_TAG");
                    dt.Columns.Add("MODEL_NAME");
                    dt.Columns.Add("ASSET_TYPE");
                    dt.Columns.Add("ASSET_HDD");
                    dt.Columns.Add("ASSET_RAM");
                    dt.Columns.Add("ASSET_PROCESSOR");

                    string AssetCode = ((Label)row.FindControl("lblAssetCode")).Text.Trim();
                    string status = ((Label)row.FindControl("lblAs")).Text.Trim();
                    string SubStatus = ((Label)row.FindControl("lblAss")).Text.Trim();
                    string SerialNo = ((Label)row.FindControl("lblSrno")).Text.Trim();
                    string AssetTag = ((Label)row.FindControl("lblAssettag")).Text.Trim();
                    string RFIDTag = ((Label)row.FindControl("lblRfTag")).Text.Trim();
                    string assetmake = ((Label)row.FindControl("lblmk")).Text.Trim();
                    string AssetFarTag = ((Label)row.FindControl("lblFarTag")).Text.Trim();
                    string assetmodel = ((Label)row.FindControl("lblam")).Text.Trim();
                    string assettype = ((Label)row.FindControl("lblat")).Text.Trim();
                    string AssetHDD = ((Label)row.FindControl("lblHdd")).Text.Trim();
                    string AssetRAM = ((Label)row.FindControl("lblRam")).Text.Trim();
                    string AssetProcessor = ((Label)row.FindControl("lblProcessor")).Text.Trim();

                    dt.Rows.Add(AssetCode, status, SubStatus, SerialNo, AssetTag, RFIDTag, assetmake, AssetFarTag, assetmodel, assettype, AssetHDD, AssetRAM, AssetProcessor);
                    dt.AcceptChanges();
                    if (GridView3.Rows.Count == 0)
                    {
                        GridView3.DataSource = dt;
                        GridView3.DataBind();
                        GridView3.Visible = true;

                        if (Session["COMPANY"].ToString() == "IT")
                        {
                            GridView3.Columns[9].Visible = false;
                        }
                        else
                        {
                            GridView3.Columns[5].Visible = false;
                            GridView3.Columns[12].Visible = false;
                            GridView3.Columns[13].Visible = false;
                            GridView3.Columns[14].Visible = false;
                        }
                    }
                    else
                    {
                        int igridverify = 0;
                        DataTable dtgrid = new DataTable();
                        dtgrid.Columns.Add("ASSET_CODE");
                        dtgrid.Columns.Add("Status");
                        dtgrid.Columns.Add("ASSET_SUB_STATUS");
                        dtgrid.Columns.Add("SERIAL_CODE");
                        dtgrid.Columns.Add("ASSET_ID");
                        dtgrid.Columns.Add("Tag_ID");
                        dtgrid.Columns.Add("ASSET_MAKE");
                        dtgrid.Columns.Add("ASSET_FAR_TAG");
                        dtgrid.Columns.Add("MODEL_NAME");
                        dtgrid.Columns.Add("ASSET_TYPE");
                        dtgrid.Columns.Add("ASSET_HDD");
                        dtgrid.Columns.Add("ASSET_RAM");
                        dtgrid.Columns.Add("ASSET_PROCESSOR");

                        foreach (GridViewRow gvRowdt in GridView3.Rows)
                        {
                            string AssetCode1 = ((Label)gvRowdt.FindControl("lblAssetCode")).Text.Trim();
                            string status1 = ((Label)gvRowdt.FindControl("lblAs")).Text.Trim();
                            string SubStatus1 = ((Label)gvRowdt.FindControl("lblAss")).Text.Trim();
                            string SerialNo1 = ((Label)gvRowdt.FindControl("lblSrno")).Text.Trim();
                            string AssetTag1 = ((Label)gvRowdt.FindControl("lblAssettag")).Text.Trim();
                            string RFIDTag1 = ((Label)gvRowdt.FindControl("lblRfTag")).Text.Trim();
                            string assetmake1 = ((Label)gvRowdt.FindControl("lblmk")).Text.Trim();
                            string AssetFarTag1 = ((Label)gvRowdt.FindControl("lblFarTag")).Text.Trim();
                            string assetmodel1 = ((Label)gvRowdt.FindControl("lblam")).Text.Trim();
                            string assettype1 = ((Label)gvRowdt.FindControl("lblat")).Text.Trim();
                            string AssetHDD1 = ((Label)gvRowdt.FindControl("lblHdd")).Text.Trim();
                            string AssetRAM1 = ((Label)gvRowdt.FindControl("lblRam")).Text.Trim();
                            string AssetProcessor1 = ((Label)gvRowdt.FindControl("lblProcessor")).Text.Trim();
                            dtgrid.Rows.Add(AssetCode1, status1, SubStatus1, SerialNo1, AssetTag1, RFIDTag1, assetmake1, AssetFarTag1, assetmodel1, assettype1, AssetHDD1, AssetRAM1, AssetProcessor1);
                            dtgrid.AcceptChanges();
                        }

                        // dtgrid = (DataTable)GridView3.DataSource;
                        for (i = 0; i < dtgrid.Rows.Count; i++)
                        {
                            if (AssetCode == dtgrid.Rows[i].Field<string>("ASSET_CODE").ToString())
                            {
                                igridverify++;
                            }
                        }
                        if (igridverify == 0)
                        {
                            dtgrid.Rows.Add(AssetCode, status, SubStatus, SerialNo, AssetTag, RFIDTag, assetmake, AssetFarTag, assetmodel, assettype, AssetHDD, AssetRAM, AssetProcessor);
                            dtgrid.AcceptChanges();
                            GridView3.DataSource = null;
                            GridView3.DataBind();
                            GridView3.DataSource = dtgrid;
                            GridView3.DataBind();
                            GridView3.Visible = true;

                            if (Session["COMPANY"].ToString() == "IT")
                            {
                                GridView3.Columns[9].Visible = false;
                            }
                            else
                            {
                                GridView3.Columns[5].Visible = false;
                                GridView3.Columns[12].Visible = false;
                                GridView3.Columns[13].Visible = false;
                                GridView3.Columns[14].Visible = false;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('This Asset Details is already Available');", true);
                            return;
                        }
                    }
                }
            }
            else
            {
                CheckBox.Checked = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('status not belongs to STOCK & Sub Status not belongs to Working.');", true);
                return;
            }
        }

        foreach (GridViewRow rows in GridView2.Rows)
        {
            CheckBox CheckBox1 = (CheckBox)rows.FindControl("chkSelectAsset");
            if (CheckBox1.Checked == true)
            {
                Headerverify++;
            }
        }
        if (GridView2.Rows.Count == Headerverify)
        {
            CheckBox CheckBoxHeader = (GridView2.HeaderRow.FindControl("chkHSelect") as CheckBox);
            CheckBoxHeader.Checked = true;
        }
        else
        {
            CheckBox CheckBoxHeader = (GridView2.HeaderRow.FindControl("chkHSelect") as CheckBox);
            CheckBoxHeader.Checked = false;
        }
    }
    protected void ddlSiteLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlSiteLocation.SelectedIndex > 0)
            //{
            //    PopulateFloor(ddlSiteLocation.SelectedValue.Trim());
            //    //PopulateStore(ddlSiteLocation.SelectedValue.Trim());
            //}
            PopulateFloor(ddlSiteLocation.SelectedValue.Trim());
            PopulateMeetingRoom();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    protected void btnUploadFile_Click(object sender, EventArgs e)
    {
        try
        {
            makeEmptyallFields();
            ConvertToExcel convertToExcel = new ConvertToExcel();
            if (Session["COMPANY"].ToString() == "IT")
            {
                var response = convertToExcel.ValidateFileReaded(AssetFileUpload, "ITAllocation");
                if (response.Item1)
                {
                    DataTable dt = response.Item2;
                    if (dt.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('No Data in the file');", true);
                        return;
                    }
                    var validate = ValidateITFile(dt);
                    if (validate.Item1)
                    {
                        SaveITAssetAllocation(dt);
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
            else if (Session["COMPANY"].ToString() == "Facilities")
            {
                var response = convertToExcel.ValidateFileReaded(AssetFileUpload, "FacilitiesAllocation");
                if (response.Item1)
                {
                    DataTable dt = response.Item2;
                    if (dt.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('No Data in the file');", true);
                        return;
                    }
                    var validate = ValidateStoreFile(dt);
                    if (validate.Item1)
                    {
                        SaveFacilitiesAssetAllocation(dt);
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
    protected void ddlSubAllocTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PopulateMeetingRoom();
            if (ddlSubAllocTo.SelectedItem.Text.ToString() == "TRAINING ROOM")
                Label24.Text = "Training Room";
            else if (ddlSubAllocTo.SelectedItem.Text.ToString() == "MEETING ROOM")
                Label24.Text = "Meeting Room";
            else
                Label24.Text = "Meeting/Training Room";
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void ddlFacilitiesFloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PopulateMeetingRoom();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    //private void SendBulkMailForAcquisition(DataTable dt, string LoginUser, string process, string Subject, string Message)
    //{
    //    try
    //    {
    //        var dataRows = dt.AsEnumerable().GroupBy(row => new {
    //            ASSET_LOCATION = row.Field<string>("ASSET_LOCATION"),
    //            ASSET_TYPE = row.Field<string>("ASSET_TYPE"),
    //            UserLevelForMail = row.Field<string>("UserLevelForMail")
    //        }).Select(G => G.First()).CopyToDataTable(); foreach (DataRow row in dataRows.Rows)
    //        {
    //            var assetRows = dt.Select("ASSET_LOCATION = '" + Convert.ToString(row["ASSET_LOCATION"]) + "' AND ASSET_TYPE = '" + Convert.ToString(row["ASSET_TYPE"]) + "' AND UserLevelForMail = '" + Convert.ToString(row["UserLevelForMail"]) + "'");
    //            try
    //            {
    //                FunctionSendingAqcuisitionMail(assetRows.CopyToDataTable(), Convert.ToString(row["ASSET_LOCATION"]), LoginUser, Convert.ToString(row["UserLevelForMail"]), Convert.ToString(row["ASSET_TYPE"]), process, Subject, Message);
    //            }
    //            catch (Exception ee)
    //            {
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        HandleExceptions(ex);
    //    }
    //}


}