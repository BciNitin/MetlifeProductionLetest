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

public partial class WebPages_IUTReceived : System.Web.UI.Page
{
    string _CompCode = "";
    IUTReceiving_DAL oDAL;
    IUTReceiving_PRP oPRP;
    bool bTransOpen = false;
    public WebPages_IUTReceived()
    {
        oPRP = new IUTReceiving_PRP();
    }
    ~WebPages_IUTReceived()
    {
        oDAL = null; oPRP = null;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new IUTReceiving_DAL(Session["DATABASE"].ToString());
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
                string _strRights = clsGeneral.GetRights("IUT_RECEIVING", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_ALLOCATION");
                btnExport.Visible = false;
                btnExport.Enabled = false;
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                PopulateLocation();
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    private void PopulateLocation()
    {
        DataTable dt = new DataTable();
        dt = oDAL.GetSiteLocation(Session["COMPANY"].ToString());
        ddlSiteLocation.DataSource = null;
        ddlSiteLocation.DataSource = dt;
        ddlSiteLocation.DataValueField = "SITE_CODE";
        ddlSiteLocation.DataTextField = "SITE_CODE";
        ddlSiteLocation.DataBind();
        //ddlSiteLocation.Items.Insert(0, "-- Select Location --");
    }

    protected void btnAllSearch_Click(object sender, EventArgs e)
    {
        if(txtGatePassCode.Text == null || txtGatePassCode.Text == string.Empty || txtGatePassCode.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Enter the Gate Pass Code.');", true);
            return;
        }

        if(ddlSiteLocation.SelectedValue.ToString() == "" || ddlSiteLocation.SelectedValue.ToString()==string.Empty || ddlSiteLocation.SelectedValue.ToString()==null)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Select the Site Location.');", true);
            return;
        }
        string SiteLocation = ddlSiteLocation.SelectedValue.ToString().ToUpper() != "" ? ddlSiteLocation.SelectedValue.ToString().ToUpper() : null;
        string GatePassCode = txtGatePassCode.Text.Trim() != "" ? txtGatePassCode.Text.Trim() : null;
        oPRP.CompCode = Session["COMPANY"].ToString();
        DataTable dt = new DataTable();
        dt = oDAL.GetAllSearchAssetDetails(GatePassCode, SiteLocation, "inTransit", oPRP.CompCode);
        //dt = oDAL.GetAllSearchAssetDetails(AssetMake, AssetModel, AssetType, SiteLocation, SerialNo, "STOCK", oPRP.CompCode);
        if (dt.Rows.Count > 0)
        {
            GridView2.DataSource = dt;
            GridView2.DataBind();
            GridView2.Visible = true;
            btnSubmit.Enabled = btnSubmit.Visible = btnSubmitLostinTransit.Enabled = btnSubmitLostinTransit.Visible = btnReject.Enabled = btnReject.Visible = true;
            if (Session["COMPANY"].ToString() == "IT")
            {
                GridView2.Columns[8].Visible = false;
            }
            else
            {
                GridView2.Columns[3].Visible = false;
                GridView2.Columns[10].Visible = false;
            }

            txtGatePassCode.Enabled = ddlSiteLocation.Enabled = false;
        }
        else
        {
            txtGatePassCode.Enabled = ddlSiteLocation.Enabled = true;
            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = false;
            btnSubmit.Enabled = btnSubmit.Visible = btnSubmitLostinTransit.Enabled = btnSubmitLostinTransit.Visible = btnReject.Enabled = btnReject.Visible = false;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Invalid search filters or assets are assigned.');", true);
            return;
        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
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

    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox ddlAgent = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ddlAgent.NamingContainer;
        int i = 0;
        if (row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckBox1 = (CheckBox)row.FindControl("chkSelectAsset");
            Label LblFloor = (Label)row.FindControl("lblFloor");
            Label lblgridStore = (Label)row.FindControl("lblgridStore");

            HiddenField hfFloor = (HiddenField)row.FindControl("hidEFloor");
            HiddenField hfStore = (HiddenField)row.FindControl("hidEStore");

            DropDownList ddlAgentFloor = (DropDownList)row.FindControl("ddlEFloor");
            DataTable dtfloor = new DataTable();
            ddlAgentFloor.DataSource = null;
            dtfloor = oDAL.GetFloor(ddlSiteLocation.SelectedValue, Session["COMPANY"].ToString());
            ddlAgentFloor.DataSource = dtfloor;
            ddlAgentFloor.DataTextField = "FLOOR_CODE";
            ddlAgentFloor.DataValueField = "FLOOR_CODE";
            ddlAgentFloor.DataBind();
            ddlAgentFloor.Items.Insert(0, "-- Select Floor --");
            

            DropDownList ddlAgentStore = (DropDownList)row.FindControl("ddlEStore");
            DataTable dtStore = new DataTable();
            ddlAgentStore.DataSource = null;
            dtStore = oDAL.GetStorewithFloor(ddlSiteLocation.SelectedValue, ddlAgentFloor.SelectedValue, Session["COMPANY"].ToString());
            ddlAgentStore.DataSource = dtStore;
            ddlAgentStore.DataTextField = "STORE_CODE";
            ddlAgentStore.DataValueField = "STORE_CODE";
            ddlAgentStore.DataBind();
            ddlAgentStore.Items.Insert(0, "-- Select Store --");
            
            if (CheckBox1.Checked)
            {
                // Shows your "Edit Template"
                LblFloor.Visible = lblgridStore.Visible = false;
                ddlAgentFloor.Visible = ddlAgentStore.Visible = true;
            }
            else
            {
                ddlAgentFloor.Visible = ddlAgentStore.Visible = false;
                LblFloor.Visible = lblgridStore.Visible = true;
            }
        }

        foreach (GridViewRow rows in GridView2.Rows)
        {
            CheckBox CheckBox1 = (CheckBox)rows.FindControl("chkSelectAsset");
            if (CheckBox1.Checked == true)
            {
                i++;
            }
        }
        if (GridView2.Rows.Count == i)
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
    protected void OnCheckedHeaderChanged(object sender, EventArgs e)
    {
        
        CheckBox CheckBoxHeader = (GridView2.HeaderRow.FindControl("chkHSelect") as CheckBox);
        
        foreach (GridViewRow row in GridView2.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CheckBox1 = (CheckBox)row.FindControl("chkSelectAsset");
                CheckBox1.Checked = CheckBoxHeader.Checked;
            
                Label LblFloor = (Label)row.FindControl("lblFloor");
                Label lblgridStore = (Label)row.FindControl("lblgridStore");

                HiddenField hfFloor = (HiddenField)row.FindControl("hidEFloor");
                HiddenField hfStore = (HiddenField)row.FindControl("hidEStore");


                DropDownList ddlAgentFloor = (DropDownList)row.FindControl("ddlEFloor");
                DataTable dtfloor = new DataTable();
                ddlAgentFloor.DataSource = null;
                dtfloor = oDAL.GetFloor(ddlSiteLocation.SelectedValue, Session["COMPANY"].ToString());
                ddlAgentFloor.DataSource = dtfloor;
                ddlAgentFloor.DataTextField = "FLOOR_CODE";
                ddlAgentFloor.DataValueField = "FLOOR_CODE";
                ddlAgentFloor.DataBind();
                ddlAgentFloor.Items.Insert(0, "-- Select Floor --");
                

                DropDownList ddlAgentStore = (DropDownList)row.FindControl("ddlEStore");
                DataTable dtStore = new DataTable();
                ddlAgentStore.DataSource = null;
                dtStore = oDAL.GetStorewithFloor(ddlSiteLocation.SelectedValue, ddlAgentFloor.SelectedValue, Session["COMPANY"].ToString());
                ddlAgentStore.DataSource = dtStore;
                ddlAgentStore.DataTextField = "STORE_CODE";
                ddlAgentStore.DataValueField = "STORE_CODE";
                ddlAgentStore.DataBind();
                ddlAgentStore.Items.Insert(0, "-- Select Store --");
                
                if (CheckBox1.Checked)
                {
                    LblFloor.Visible = lblgridStore.Visible = false;
                    ddlAgentFloor.Visible = ddlAgentStore.Visible = true;
                }
                else
                {
                    ddlAgentFloor.Visible = ddlAgentStore.Visible = false;
                    LblFloor.Visible = lblgridStore.Visible = true;
                }
            }
        }
    }
    protected void ddlEFloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAgent = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlAgent.NamingContainer;
        if (row != null)
        {
            DropDownList ddl = (DropDownList)row.FindControl("ddlEFloor");
            DropDownList ddlstore = (DropDownList)row.FindControl("ddlEStore");
            DataTable dtstore = new DataTable();
            ddlstore.DataSource = null;
            dtstore = oDAL.GetStorewithFloor(ddlSiteLocation.SelectedValue, ddl.SelectedValue, Session["COMPANY"].ToString());
            ddlstore.DataSource = dtstore;
            ddlstore.DataTextField = "STORE_CODE";
            ddlstore.DataValueField = "STORE_CODE";
            ddlstore.DataBind();
            ddlstore.Items.Insert(0, "-- Select Store --");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int iSubmit = 1;
            foreach (GridViewRow rowcheckedcount in GridView2.Rows)
            {
                if (rowcheckedcount.RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBox1 = (CheckBox)rowcheckedcount.FindControl("chkSelectAsset");
                    DropDownList ddlAgentFloor = (DropDownList)rowcheckedcount.FindControl("ddlEFloor");

                    if (CheckBox1.Checked == true)
                    {
                        if (ddlAgentFloor.SelectedIndex == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Need to Selected the Floor in Row  " + iSubmit + " .');", true);
                            return;
                        }
                    }
                    iSubmit++;
                }
            }


            DataTable dtITIUTReceiving = new DataTable();
            dtITIUTReceiving.Columns.Add("GatePass Code");
            dtITIUTReceiving.Columns.Add("Asset Code");
            dtITIUTReceiving.Columns.Add("GatePass Date");
            dtITIUTReceiving.Columns.Add("Source Location");
            dtITIUTReceiving.Columns.Add("Destination Location");
            dtITIUTReceiving.Columns.Add("Asset Type");
            dtITIUTReceiving.Columns.Add("Asset Make");
            dtITIUTReceiving.Columns.Add("Asset Model");
            dtITIUTReceiving.Columns.Add("Serial Number");
            dtITIUTReceiving.Columns.Add("IUT Status");
            dtITIUTReceiving.Columns.Add("IUT Received Date");


            DataTable dtFacilitiesIUTReceiving = new DataTable();
            dtFacilitiesIUTReceiving.Columns.Add("GatePass Code");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Code");
            dtFacilitiesIUTReceiving.Columns.Add("GatePass Date");
            dtFacilitiesIUTReceiving.Columns.Add("Source Location");
            dtFacilitiesIUTReceiving.Columns.Add("Destination Location");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Type");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Make");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Model");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Far Tag");
            dtFacilitiesIUTReceiving.Columns.Add("IUT Status");
            dtFacilitiesIUTReceiving.Columns.Add("IUT Received Date");

            new IUTReceiving_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "IUT Receiving", "IUT Receiving", "trying to IUT Receiving by user id " + Session["CURRENTUSER"].ToString() + "");
            bool bAssetSelected = false;
            string _LiveGPCode = "";
            if (clsGeneral._strRights[1] == "0")
            {
                btnReject.Enabled = btnSubmitLostinTransit.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }
            
            for (int iCnt = 0; iCnt < GridView2.Rows.Count; iCnt++)
            {
                if (((CheckBox)GridView2.Rows[iCnt].FindControl("chkSelectAsset")).Checked == true)
                {
                    bAssetSelected = true;
                    break;
                }
            }
            if (!bAssetSelected)
            {
                btnReject.Enabled = btnSubmitLostinTransit.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note :  select assets from grid or search asset for gatepass.');", true);
                return;
            }
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.Remarks = txtRemarks.Text;

            oPRP.Site = ddlSiteLocation.SelectedValue;
            
            string bRsp = string.Empty;
            foreach (GridViewRow gvRow in GridView2.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                {
                    oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                    oPRP.GatePassCode = ((Label)gvRow.FindControl("lblGatePassCode")).Text.Trim();
                    oPRP.AssetLocation = ((Label)gvRow.FindControl("lblGatePassOutLoc")).Text.Trim();
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.SerialCode = ((Label)gvRow.FindControl("lblSrno")).Text.Trim();
                    else
                        oPRP.ASSET_FAR_TAG = ((Label)gvRow.FindControl("lblFarTag")).Text.Trim();

                    oPRP.Floor = ((DropDownList)gvRow.FindControl("ddlEFloor")).SelectedValue;
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.Store = (((DropDownList)gvRow.FindControl("ddlEStore")).SelectedIndex == 0 ? string.Empty : ((DropDownList)gvRow.FindControl("ddlEStore")).SelectedValue);
                    else
                        oPRP.Store = string.Empty;

                    bRsp = oDAL.UpdateIUTReceivingDetailsSP("RECEIVE",oPRP);
                    if (bRsp.Contains("SUCCESS"))
                    {
                        if (Session["COMPANY"].ToString() == "IT")
                        {
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string GatePassDate = oDAL.GetGatePassDate(oPRP.GatePassCode, Session["COMPANY"].ToString());
                            string CurrentDate = oDAL.GetIUTCurrentDate();
                            dtITIUTReceiving.Rows.Add(oPRP.GatePassCode, oPRP.AssetCode,GatePassDate, oPRP.AssetLocation, oPRP.Site, AssetType, AssetMake, AssetModel, oPRP.SerialCode,"RECEIVED",CurrentDate);
                            dtITIUTReceiving.AcceptChanges();
                        }
                        else
                        {
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string GatePassDate = oDAL.GetGatePassDate(oPRP.GatePassCode, Session["COMPANY"].ToString());
                            string CurrentDate = oDAL.GetIUTCurrentDate();
                            dtFacilitiesIUTReceiving.Rows.Add(oPRP.GatePassCode, oPRP.AssetCode, GatePassDate, oPRP.AssetLocation, oPRP.Site, AssetType, AssetMake, AssetModel, oPRP.ASSET_FAR_TAG, "RECEIVED", CurrentDate);
                            dtFacilitiesIUTReceiving.AcceptChanges();
                        }
                        //if (Session["COMPANY"].ToString() == "IT")
                        //{
                        //    dtITIUTReceiving.Rows.Add(oPRP.GatePassCode,oPRP.AssetCode, oPRP.SerialCode,"RECEIVED");
                        //    dtITIUTReceiving.AcceptChanges();
                        //}
                        //else
                        //{
                        //    dtFacilitiesIUTReceiving.Rows.Add(oPRP.GatePassCode,oPRP.AssetCode, oPRP.ASSET_FAR_TAG, "RECEIVED");
                        //    dtFacilitiesIUTReceiving.AcceptChanges();
                        //}
                    }
                }
            }
            if (!bRsp.Contains("SUCCESS"))
            {
                btnReject.Enabled = btnSubmitLostinTransit.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Receiving details is not Updated successfully.);", true);
            }
            else
            {
                DataTable dp = oDAL.GetMailTransactionDetails("IUT_RECEIVING", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
                {
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        if (Session["COMPANY"].ToString() == "IT")
                            sendmail.FunctionSendingMailWithAssetData(dtITIUTReceiving, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        else
                            sendmail.FunctionSendingMailWithAssetData(dtFacilitiesIUTReceiving, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));

                        //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    }
                    catch (Exception ee)
                    {
                        
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Received Asset details is Updated successfully.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                
                btnSubmit.Enabled = btnSubmitLostinTransit.Enabled = btnReject.Enabled = btnSubmit.Visible = btnSubmitLostinTransit.Visible = btnReject.Visible = false;
                lblErrorMsg.Text = "Please Note : Assets Received Details are Updated Successfully";
                upSubmit.Update();
                //try
                //{ MailGatepassDetails(oPRP.GatePassCode); }
                //catch (Exception ex) { }
            }
            bTransOpen = true;
            if (bTransOpen)
            {
                bTransOpen = false;
            }
        }
        catch (Exception ex)
        {
            btnReject.Enabled = btnSubmitLostinTransit.Enabled = true;
            bTransOpen = false;
            HandleExceptions(ex);
        }
    }

    protected void btnSubmitLostinTransit_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dtITIUTReceiving = new DataTable();
            dtITIUTReceiving.Columns.Add("GatePass Code");
            dtITIUTReceiving.Columns.Add("Asset Code");
            dtITIUTReceiving.Columns.Add("GatePass Date");
            dtITIUTReceiving.Columns.Add("Source Location");
            dtITIUTReceiving.Columns.Add("Destination Location");
            dtITIUTReceiving.Columns.Add("Asset Type");
            dtITIUTReceiving.Columns.Add("Asset Make");
            dtITIUTReceiving.Columns.Add("Asset Model");
            dtITIUTReceiving.Columns.Add("Serial Number");
            dtITIUTReceiving.Columns.Add("IUT Status");
            dtITIUTReceiving.Columns.Add("IUT Received Date");


            DataTable dtFacilitiesIUTReceiving = new DataTable();
            dtFacilitiesIUTReceiving.Columns.Add("GatePass Code");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Code");
            dtFacilitiesIUTReceiving.Columns.Add("GatePass Date");
            dtFacilitiesIUTReceiving.Columns.Add("Source Location");
            dtFacilitiesIUTReceiving.Columns.Add("Destination Location");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Type");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Make");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Model");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Far Tag");
            dtFacilitiesIUTReceiving.Columns.Add("IUT Status");
            dtFacilitiesIUTReceiving.Columns.Add("IUT Received Date");

            btnSubmit.Enabled = btnReject.Enabled = false;
            new IUTReceiving_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "IUT Receiving", "IUT Receiving", "trying to IUT Receiving by user id " + Session["CURRENTUSER"].ToString() + "");
            bool bAssetSelected = false;
            string _LiveGPCode = "";
            if (clsGeneral._strRights[1] == "0")
            {
                btnSubmit.Enabled = btnReject.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }

            for (int iCnt = 0; iCnt < GridView2.Rows.Count; iCnt++)
            {
                if (((CheckBox)GridView2.Rows[iCnt].FindControl("chkSelectAsset")).Checked == true)
                {
                    bAssetSelected = true;
                    break;
                }
            }
            if (!bAssetSelected)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note :  select assets from grid or search asset for gatepass.');", true);
                btnSubmit.Enabled = btnReject.Enabled = false;
                return;
            }
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.Remarks = txtRemarks.Text;

            oPRP.Site = ddlSiteLocation.SelectedValue;
            
            //oPRP.AssetLocation = txtSiteLocation.Text.Trim().ToUpper();
            //oPRP.GatePassCode = txtGatePassCode.Text.Trim();
            // lblLocCode.Text.Trim();
            //bool bRsp = false;
            string bRsp = string.Empty;
            foreach (GridViewRow gvRow in GridView2.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                {
                    oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                    oPRP.GatePassCode = ((Label)gvRow.FindControl("lblGatePassCode")).Text.Trim();
                    oPRP.AssetLocation = ((Label)gvRow.FindControl("lblGatePassOutLoc")).Text.Trim();
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.SerialCode = ((Label)gvRow.FindControl("lblSrno")).Text.Trim();
                    else
                        oPRP.ASSET_FAR_TAG = ((Label)gvRow.FindControl("lblFarTag")).Text.Trim();

                    oPRP.Floor = ((DropDownList)gvRow.FindControl("ddlEFloor")).SelectedValue;
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.Store = (((DropDownList)gvRow.FindControl("ddlEStore")).SelectedIndex == 0 ? string.Empty : ((DropDownList)gvRow.FindControl("ddlEStore")).SelectedValue);
                    else
                        oPRP.Store = string.Empty;

                    //_LiveGPCode = oDAL.ChkLiveGP(oPRP.AssetCode);
                    //if (_LiveGPCode != "")
                    //{
                    //    lblErrorMsg.Text = "Selected asset is already part of Gate Pass No: " + _LiveGPCode + " and is not returned yet.";
                    //    return;
                    //}
                    bRsp = oDAL.UpdateIUTReceivingDetailsSP("LOST_IN_TRANSIT",oPRP);
                    if (bRsp.Contains("SUCCESS"))
                    {
                        if (Session["COMPANY"].ToString() == "IT")
                        {
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string GatePassDate = oDAL.GetGatePassDate(oPRP.GatePassCode, Session["COMPANY"].ToString());
                            string CurrentDate = oDAL.GetIUTCurrentDate();
                            dtITIUTReceiving.Rows.Add(oPRP.GatePassCode, oPRP.AssetCode,GatePassDate, oPRP.AssetLocation, oPRP.Site, AssetType, AssetMake, AssetModel, oPRP.SerialCode,"LOST IN TRANSIT",CurrentDate);
                            dtITIUTReceiving.AcceptChanges();
                        }
                        else
                        {
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string GatePassDate = oDAL.GetGatePassDate(oPRP.GatePassCode, Session["COMPANY"].ToString());
                            string CurrentDate = oDAL.GetIUTCurrentDate();
                            dtFacilitiesIUTReceiving.Rows.Add(oPRP.GatePassCode, oPRP.AssetCode, GatePassDate, oPRP.AssetLocation, oPRP.Site, AssetType, AssetMake, AssetModel, oPRP.ASSET_FAR_TAG, "LOST IN TRANSIT", CurrentDate);
                            dtFacilitiesIUTReceiving.AcceptChanges();
                        }
                    }
                }
            }
            if (!bRsp.Contains("SUCCESS"))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Receiving details is not Updated successfully.);", true);
                btnReject.Enabled = btnSubmitLostinTransit.Enabled = true;
            }
            else
            {
                DataTable dp = oDAL.GetMailTransactionDetails("IUT_RECEIVING", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
                {
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        if (Session["COMPANY"].ToString() == "IT")
                            sendmail.FunctionSendingMailWithAssetData(dtITIUTReceiving, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        else
                            sendmail.FunctionSendingMailWithAssetData(dtFacilitiesIUTReceiving, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));

                        //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    }
                    catch (Exception ee)
                    {

                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Lost In Transit Asset details is Updated successfully.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                btnReject.Enabled = btnSubmitLostinTransit.Enabled = btnSubmit.Enabled = btnReject.Visible = btnSubmitLostinTransit.Visible = btnSubmit.Visible = false;
                lblErrorMsg.Text = "Please Note : Assets Lost In Transit Details are Updated Successfully";
                upSubmit.Update();
                //try
                //{ MailGatepassDetails(oPRP.GatePassCode); }
                //catch (Exception ex) { }
            }
            bTransOpen = true;
            if (bTransOpen)
            {
                bTransOpen = false;
            }
        }
        catch (Exception ex)
        {
            btnSubmit.Enabled = btnReject.Enabled = true;
            bTransOpen = false;
            HandleExceptions(ex);
        }
    }

    private void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "IUT Received");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Server.Transfer("Error.aspx");
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dtITIUTReceiving = new DataTable();
            dtITIUTReceiving.Columns.Add("GatePass Code");
            dtITIUTReceiving.Columns.Add("Asset Code");
            dtITIUTReceiving.Columns.Add("GatePass Date");
            dtITIUTReceiving.Columns.Add("Source Location");
            dtITIUTReceiving.Columns.Add("Destination Location");
            dtITIUTReceiving.Columns.Add("Asset Type");
            dtITIUTReceiving.Columns.Add("Asset Make");
            dtITIUTReceiving.Columns.Add("Asset Model");
            dtITIUTReceiving.Columns.Add("Serial Number");
            dtITIUTReceiving.Columns.Add("IUT Status");
            dtITIUTReceiving.Columns.Add("IUT Received Date");


            DataTable dtFacilitiesIUTReceiving = new DataTable();
            dtFacilitiesIUTReceiving.Columns.Add("GatePass Code");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Code");
            dtFacilitiesIUTReceiving.Columns.Add("GatePass Date");
            dtFacilitiesIUTReceiving.Columns.Add("Source Location");
            dtFacilitiesIUTReceiving.Columns.Add("Destination Location");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Type");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Make");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Model");
            dtFacilitiesIUTReceiving.Columns.Add("Asset Far Tag");
            dtFacilitiesIUTReceiving.Columns.Add("IUT Status");
            dtFacilitiesIUTReceiving.Columns.Add("IUT Received Date");

            btnSubmit.Enabled = btnSubmitLostinTransit.Enabled = false;
            new IUTReceiving_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "IUT Receiving", "IUT Receiving", "trying to IUT Receiving by user id " + Session["CURRENTUSER"].ToString() + "");
            bool bAssetSelected = false;
            string _LiveGPCode = "";
            if (clsGeneral._strRights[1] == "0")
            {
                btnSubmit.Enabled = btnSubmitLostinTransit.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
                return;
            }

            for (int iCnt = 0; iCnt < GridView2.Rows.Count; iCnt++)
            {
                if (((CheckBox)GridView2.Rows[iCnt].FindControl("chkSelectAsset")).Checked == true)
                {
                    bAssetSelected = true;
                    break;
                }
            }
            if (!bAssetSelected)
            {
                btnSubmit.Enabled = btnSubmitLostinTransit.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note :  select assets from grid or search asset for gatepass.');", true);
                return;
            }
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
            oPRP.Remarks = txtRemarks.Text.Trim();

            oPRP.Site = ddlSiteLocation.SelectedValue;
           
            //oPRP.AssetLocation = txtSiteLocation.Text.Trim().ToUpper();
            //oPRP.GatePassCode = txtGatePassCode.Text.Trim();
            // lblLocCode.Text.Trim();
            //bool bRsp = false;
            string bRsp = string.Empty;
            foreach (GridViewRow gvRow in GridView2.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                {
                    oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                    oPRP.GatePassCode = ((Label)gvRow.FindControl("lblGatePassCode")).Text.Trim();
                    oPRP.AssetLocation = ((Label)gvRow.FindControl("lblGatePassOutLoc")).Text.Trim();
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.SerialCode = ((Label)gvRow.FindControl("lblSrno")).Text.Trim();
                    else
                        oPRP.ASSET_FAR_TAG = ((Label)gvRow.FindControl("lblFarTag")).Text.Trim();

                    oPRP.Floor = ((DropDownList)gvRow.FindControl("ddlEFloor")).SelectedValue;
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.Store = (((DropDownList)gvRow.FindControl("ddlEStore")).SelectedIndex == 0 ? string.Empty : ((DropDownList)gvRow.FindControl("ddlEStore")).SelectedValue);
                    else
                        oPRP.Store = string.Empty;

                    //_LiveGPCode = oDAL.ChkLiveGP(oPRP.AssetCode);
                    //if (_LiveGPCode != "")
                    //{
                    //    lblErrorMsg.Text = "Selected asset is already part of Gate Pass No: " + _LiveGPCode + " and is not returned yet.";
                    //    return;
                    //}
                    bRsp = oDAL.UpdateIUTReceivingDetailsSP("REJECT", oPRP);
                    if (bRsp.Contains("SUCCESS"))
                    {
                        if (Session["COMPANY"].ToString() == "IT")
                        {
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string GatePassDate = oDAL.GetGatePassDate(oPRP.GatePassCode, Session["COMPANY"].ToString());
                            string CurrentDate = oDAL.GetIUTCurrentDate();
                            dtITIUTReceiving.Rows.Add(oPRP.GatePassCode, oPRP.AssetCode, GatePassDate, oPRP.AssetLocation, oPRP.Site, AssetType, AssetMake, AssetModel, oPRP.SerialCode,"REJECT", CurrentDate);
                            dtITIUTReceiving.AcceptChanges();
                        }
                        else
                        {
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string GatePassDate = oDAL.GetGatePassDate(oPRP.GatePassCode, Session["COMPANY"].ToString());
                            string CurrentDate = oDAL.GetIUTCurrentDate();
                            dtFacilitiesIUTReceiving.Rows.Add(oPRP.GatePassCode, oPRP.AssetCode, GatePassDate, oPRP.AssetLocation, oPRP.Site, AssetType, AssetMake, AssetModel, oPRP.ASSET_FAR_TAG,"REJECT", CurrentDate);
                            dtFacilitiesIUTReceiving.AcceptChanges();
                        }
                    }
                }
            }
            if (!bRsp.Contains("SUCCESS"))
            {
                btnSubmit.Enabled = btnSubmitLostinTransit.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Receiving details is not Updated successfully.);", true);
            }
            else
            {
                DataTable dp = oDAL.GetMailTransactionDetails("IUT_RECEIVING", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
                {
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        if (Session["COMPANY"].ToString() == "IT")
                            sendmail.FunctionSendingMailWithAssetData(dtITIUTReceiving, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        else
                            sendmail.FunctionSendingMailWithAssetData(dtFacilitiesIUTReceiving, dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                        //sendmail.SendMailForTransaction(dp.Rows[0].Field<string>("TO_MAIL_ID"), dp.Rows[0].Field<string>("CC_MAIL_ID"), dp.Rows[0].Field<string>("MAIL_SUBJECT"), dp.Rows[0].Field<string>("MAIL_BODY"));
                    }
                    catch (Exception ee)
                    {
                        
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Rejected details is Updated successfully.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);
                //lblErrorMsg.Visible = true;
                //lblErrorMsg.Text = "Please Note : Asset Rejected Details are Updated Successfully";
                btnSubmit.Enabled = btnSubmitLostinTransit.Enabled = btnReject.Enabled = btnSubmit.Visible = btnSubmitLostinTransit.Visible = btnReject.Visible = false;
                upSubmit.Update();
                //try
                //{ MailGatepassDetails(oPRP.GatePassCode); }
                //catch (Exception ex) { }
            }
            bTransOpen = true;
            if (bTransOpen)
            {
                bTransOpen = false;
            }
        }
        catch (Exception ex)
        {
            btnSubmit.Enabled = btnSubmitLostinTransit.Enabled = true;
            bTransOpen = false;
            HandleExceptions(ex);
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