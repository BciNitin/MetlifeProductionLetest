using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.Text;
using System.IO;
using System.Globalization;

public partial class WebPages_AssetDeAllocation : System.Web.UI.Page
{
    string _CompCode = "";
    AssetAllocation_DAL oDAL;
    AssetAllocation_PRP oPRP;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                string _strRights = clsGeneral.GetRights("ASSET_DEALLOCATION", (DataTable)Session["UserRights"]);
                clsGeneral._strRights = _strRights.Split('^');
                clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "ASSET_DEALLOCATION");
                PopulateLocationFilter();
                if (clsGeneral._strRights[0] == "0")
                {
                    Response.Redirect("UnauthorizedUser.aspx", false);
                }
                btnExport.Visible = false;
                btnExport.Enabled = false;
                Session["ERRORDATA"] = null;
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
        oDAL = new AssetAllocation_DAL(Session["DATABASE"].ToString());
        _CompCode = Session["COMPANY"].ToString();
    }
    public WebPages_AssetDeAllocation()
    {
        oPRP = new AssetAllocation_PRP();
    }
    ~WebPages_AssetDeAllocation()
    {
        oDAL = null; oPRP = null;
    }

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


    #region BUTTON EVENTS
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
                    DropDownList ddlSubStatus = (DropDownList)rowcheckedcount.FindControl("ddlESubStatus");

                    if (CheckBox1.Checked == true)
                    {
                        if(ddlSubStatus.SelectedValue.ToString().ToUpper() !="LOST" && ddlSubStatus.SelectedValue.ToString().ToUpper() != "LOST IN-TRANSIT")
                        {
                            if (ddlAgentFloor.SelectedIndex == 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Need to Selected the Floor in Row  " + iSubmit + " .');", true);
                                return;
                            }
                        }
                        
                    }
                    iSubmit++;
                }
            }

            DataTable dtforEmail = new DataTable();
            dtforEmail.Columns.Add("Asset Code");
            dtforEmail.Columns.Add("Site Location");
            dtforEmail.Columns.Add("Employee Name");
            dtforEmail.Columns.Add("Employee Code");
            dtforEmail.Columns.Add("Emp Email");
            dtforEmail.Columns.Add("Asset Type");
            dtforEmail.Columns.Add("Asset Make");
            dtforEmail.Columns.Add("Asset Model");
            dtforEmail.Columns.Add("Serial Number");
            dtforEmail.Columns.Add("RFID Tag Id");
            dtforEmail.Columns.Add("SubStatus");

            DataTable dtfloorcountonly = new DataTable();
            dtfloorcountonly.Columns.Add("Asset Code");
            dtfloorcountonly.Columns.Add("Site Location");
            dtfloorcountonly.Columns.Add("Floor");
            dtfloorcountonly.Columns.Add("Asset Type");
            dtfloorcountonly.Columns.Add("Asset Make");
            dtfloorcountonly.Columns.Add("Asset Model");
            dtfloorcountonly.Columns.Add("Serial Number");
            dtfloorcountonly.Columns.Add("RFID Tag Id");
            dtfloorcountonly.Columns.Add("SubStatus");

            DataTable dtFacilitiesForEmail = new DataTable();
            dtFacilitiesForEmail.Columns.Add("Asset Code");
            dtFacilitiesForEmail.Columns.Add("Site Location");
            dtFacilitiesForEmail.Columns.Add("Floor");
            dtFacilitiesForEmail.Columns.Add("Asset Type");
            dtFacilitiesForEmail.Columns.Add("Asset Make");
            dtFacilitiesForEmail.Columns.Add("Asset Model");
            dtFacilitiesForEmail.Columns.Add("Asset Far Tag");
            dtFacilitiesForEmail.Columns.Add("RFID Tag Id");
            dtFacilitiesForEmail.Columns.Add("SubStatus");


            new AssetAcquisition_DAL(Session["DATABASE"].ToString()).SaveAuditLogin(Session["CURRENTUSER"].ToString(), Session["COMPANY"].ToString(), "Asset De-Allocation", "Asset De-Allocation", "Asset  De-Allocation done by user id" + Session["CURRENTUSER"].ToString());
            bool bAssetSelected = false;
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();
            oPRP.ActualReturnDate = (txtDeAllocateDate.Text.Trim() != "") ? txtDeAllocateDate.Text.Trim() : "01/Jan/1900";
            if (Convert.ToDateTime(oPRP.ActualReturnDate) > Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy")))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Deallocation Date should not be greater than current date.');", true);
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
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Click on Get Assets button/search to select assets.');", true);
                return;
            }
            oPRP.AssetAllocated = false;
            oPRP.AssetLocation = (ddlSiteLocationFilter.SelectedIndex > 0 ? ddlSiteLocationFilter.SelectedValue : "");
            bool bResp = false;
            foreach (GridViewRow gvRow in GridView2.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSelectAsset")).Checked)
                {
                    oPRP.AssetCode = ((Label)gvRow.FindControl("lblAssetCode")).Text.Trim();
                    oPRP.SerialCode = ((Label)gvRow.FindControl("lblSrno")).Text.Trim();
                    oPRP.AssetTagID = ((Label)gvRow.FindControl("lblRfTag")).Text.Trim();
                    oPRP.SubStatus = ((DropDownList)gvRow.FindControl("ddlESubStatus")).SelectedItem.Text;
                    oPRP.Site = ((Label)gvRow.FindControl("lblSite")).Text.Trim();
                    oPRP.Floor = ((DropDownList)gvRow.FindControl("ddlEFloor")).SelectedItem.Text;
                    if (Session["COMPANY"].ToString() == "IT")
                        oPRP.Store = (((DropDownList)gvRow.FindControl("ddlEStore")).SelectedIndex == 0 ? string.Empty : ((DropDownList)gvRow.FindControl("ddlEStore")).SelectedItem.Text);
                    else
                        oPRP.Store = string.Empty;

                    oPRP.DeallocationRemarks = ((TextBox)gvRow.FindControl("txtERemarks")).Text.Trim();
                    oPRP.AssetMake = ((Label)gvRow.FindControl("lblmk")).Text.Trim();
                    oPRP.RFIDTag = ((Label)gvRow.FindControl("lblRfTag")).Text.Trim();
                    oPRP.Asset_FAR_TAG = ((Label)gvRow.FindControl("lblFarTag")).Text.Trim();
                    oPRP.AssetType = ((Label)gvRow.FindControl("lblat")).Text.Trim();
                    oPRP.HostName = ((Label)gvRow.FindControl("lblHostName")).Text.Trim();
                    oPRP.AllocationType = ((Label)gvRow.FindControl("lblAllocationType")).Text.Trim();
                    string dateString = null;
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    dateString = ((Label)gvRow.FindControl("lblAllocationDate")).Text.Trim().ToString();
                    DateTime dateTime13 = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider); // 10/22/2015 12:00:00 AM  
                    string temp = dateTime13.ToString();
                    oPRP.AllocationDate = dateTime13.ToString("dd/MMM/yyyy");
                    string dateString1 = null;
                    dateString1 = ((Label)gvRow.FindControl("lblExpectedReturnDate")).Text.Trim().ToString();
                    DateTime dateTime134 = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider); // 10/22/2015 12:00:00 AM  
                    oPRP.ExpReturnDate = dateTime134.ToString("dd/MMM/yyyy");
                    oPRP.TicketNo = ((Label)gvRow.FindControl("lblServiceTicket")).Text.Trim();
                    oPRP.EmpCode = ((Label)gvRow.FindControl("lblEmployeeID")).Text.Trim();
                    oPRP.EmpName = ((Label)gvRow.FindControl("lblEmployeeName")).Text.Trim();
                    oPRP.IdentifierLocation = ((Label)gvRow.FindControl("lblIdentifierLocation")).Text.Trim();
                    oPRP.Status = ((Label)gvRow.FindControl("lblAs")).Text.Trim();
                    oPRP.AllocationId = oDAL.GetAllocationId(oPRP.AssetCode, Session["COMPANY"].ToString());
                    //oPRP.SubStatus = ((Label)gvRow.FindControl("lblAss")).Text.Trim();
                    bResp = oDAL.SaveAssetAllocation("RETURN", oPRP);
                    if (bResp)
                    {
                        if (!string.IsNullOrEmpty(oPRP.EmpCode))
                        {
                            string EmailId = oDAL.GetEmpEmail(oPRP.EmpCode);
                            DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                            string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                            string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                            string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                            string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                            dtforEmail.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.EmpName, oPRP.EmpCode, EmailId, AssetType, AssetMake, AssetModel, oPRP.SerialCode, RFIDTag, oPRP.SubStatus);
                            dtforEmail.AcceptChanges();
                        }
                        else
                        {
                            if (Session["COMPANY"].ToString() == "IT")
                            {
                                DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                                string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                                string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                                string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                                string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                                dtfloorcountonly.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.Floor, AssetType, AssetMake, AssetModel, oPRP.SerialCode, RFIDTag,oPRP.SubStatus);
                                dtfloorcountonly.AcceptChanges();
                            }
                            else
                            {
                                DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                                string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                                string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                                string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                                string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                                dtFacilitiesForEmail.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.Floor, AssetType, AssetMake, AssetModel, oPRP.Asset_FAR_TAG, RFIDTag,oPRP.SubStatus);
                                dtFacilitiesForEmail.AcceptChanges();
                            }

                        }
                    }
                }
            }
            if (bResp)
            {
                if (dtfloorcountonly.Rows.Count > 0 || dtFacilitiesForEmail.Rows.Count > 0)
                {
                    DataTable dp = oDAL.GetMailTransactionDetails("ASSET_DEALLOCATION", Convert.ToString(Session["COMP_NAME"]));
                    if (dp.Rows.Count > 0)
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
                            clsGeneral.LogErrorToLogFile(ee, "Asset DeAllocation");
                            clsGeneral.LogErrorToLogFile(ee.InnerException, "Asset DeAllocation");
                        }
                    }
                }

                if (dtforEmail.Rows.Count > 0)
                {
                    DataTable dp1 = oDAL.GetMailTransactionDetails("ASSET_DEALLOCATION", Convert.ToString(Session["COMP_NAME"]));
                    string Subject = "Assets Deallocated";
                    string MailBody = "";
                    string CCAddress = "";
                    if (dp1.Rows.Count > 0)
                    {
                        CCAddress = (dp1.Rows[0].Field<string>("CC_MAIL_ID") == null || dp1.Rows[0].Field<string>("CC_MAIL_ID") == "" || dp1.Rows[0].Field<string>("CC_MAIL_ID") == string.Empty) ? "" : dp1.Rows[0].Field<string>("CC_MAIL_ID").Trim();
                        Subject = (dp1.Rows[0].Field<string>("MAIL_SUBJECT") == null || dp1.Rows[0].Field<string>("MAIL_SUBJECT") == "" || dp1.Rows[0].Field<string>("MAIL_SUBJECT") == string.Empty) ? "" : dp1.Rows[0].Field<string>("MAIL_SUBJECT").Trim();
                        MailBody = (dp1.Rows[0].Field<string>("MAIL_BODY") == null || dp1.Rows[0].Field<string>("MAIL_BODY") == "" || dp1.Rows[0].Field<string>("MAIL_BODY") == string.Empty) ? "" : dp1.Rows[0].Field<string>("MAIL_BODY").Trim();
                    }
                    var dataRows = dtforEmail.AsEnumerable().GroupBy(row => new
                    {
                        EmailId = row.Field<string>("Emp Email")
                    }).Select(G => G.First()).CopyToDataTable(); 
                    foreach (DataRow row in dataRows.Rows)
                    {
                        var assetRows = dtforEmail.Select("[Emp Email]='" + Convert.ToString(row["Emp Email"]) + "'");
                        try
                        {
                            SendmailAlert sendmail = new SendmailAlert();
                            sendmail.FunctionSendingAqcuisitionMail(assetRows.CopyToDataTable(), Convert.ToString(row["Emp Email"]), CCAddress, Subject, MailBody);
                        }
                        catch (Exception ee)
                        {
                            clsGeneral.LogErrorToLogFile(ee, "Employee Asset DeAllocation");
                            clsGeneral.LogErrorToLogFile(ee.InnerException, "Employee Asset DeAllocation");
                        }
                    }

                    //if (EmailId != "" || EmailId != string.Empty)
                    //{
                    //    try
                    //    {
                    //        SendmailAlert sendmail = new SendmailAlert();
                    //        sendmail.FunctionSendingAqcuisitionMail(dtforEmail, EmailId.Trim(), CCAddress, Subject, MailBody);
                    //    }
                    //    catch (Exception ee) { }
                    //}
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Assets Deallocated successfully.');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClearFields", "ClearFields();", true);

            }
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

    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        //... Your code ...
        // Here we find the controls tha we will handle
        //CheckBox chkAll = (GridView2.HeaderRow.FindControl("chkHSelect") as CheckBox);
        //chkAll.Checked = true;
        int verifygrid = 0;
        int checkedCount = 0;

        foreach (GridViewRow rowcheckedcount in GridView2.Rows)
        {
            CheckBox CheckBox1 = (CheckBox)rowcheckedcount.FindControl("chkSelectAsset");
            if (CheckBox1.Checked == true)
                checkedCount++;
        }

        CheckBox ddlAgentverify = (CheckBox)sender;
        GridViewRow rowVerify = (GridViewRow)ddlAgentverify.NamingContainer;
        int indexverify = Convert.ToInt32(rowVerify.RowIndex.ToString());

        int jverify = 0;
        foreach (GridViewRow rowcheck in GridView2.Rows)
        {
            if (rowcheck.RowType == DataControlRowType.DataRow)
            {
                CheckBox CheckBox1 = (CheckBox)rowcheck.FindControl("chkSelectAsset");
                if (CheckBox1.Checked == true)
                {
                    if (rowcheck.RowIndex != rowVerify.RowIndex)
                    {
                        if (checkedCount > 0)
                        {
                            DropDownList ddlAgentFloor = (DropDownList)rowcheck.FindControl("ddlEFloor");
                            DropDownList ddlSubStatus = (DropDownList)rowcheck.FindControl("ddlESubStatus");
                            if(ddlSubStatus.SelectedValue.ToString().ToUpper() !="LOST" && ddlSubStatus.SelectedValue.ToString().ToUpper() != "LOST IN-TRANSIT")
                            {
                                if (ddlAgentFloor.SelectedIndex == 0)
                                {
                                    jverify++;
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Need to Selected the Floor in Row  " + verifygrid + " .');", true);
                                    break;
                                }
                            }
                            verifygrid++;
                        }
                    }

                }
                else
                {
                    Label lblSite = (Label)rowcheck.FindControl("lblSite");
                    Label LblFloor = (Label)rowcheck.FindControl("lblFloor");
                    Label lblgridStore = (Label)rowcheck.FindControl("lblgridStore");

                    DropDownList ddlAgentFloor = (DropDownList)rowcheck.FindControl("ddlEFloor");
                    DropDownList ddlAgentStore = (DropDownList)rowcheck.FindControl("ddlEStore");

                    ddlAgentFloor.Visible = ddlAgentStore.Visible = false;
                    LblFloor.Visible = lblgridStore.Visible = true;
                }
                verifygrid++;
            }
        }


        CheckBox ddlAgent = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ddlAgent.NamingContainer;
        int i = 0;
        if (row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CheckBox1 = (CheckBox)row.FindControl("chkSelectAsset");
            if (jverify > 0)
            {
                CheckBox1.Checked = false;
                return;
            }


            HiddenField hfAgentID = (HiddenField)row.FindControl("hidESubStatus");
            DropDownList ddlgridSubStatus = (DropDownList)row.FindControl("ddlESubStatus");
            DataTable dt = new DataTable();
            ddlgridSubStatus.DataSource = null;
            dt = oDAL.GetSubStatusGrid();
            ddlgridSubStatus.DataSource = dt;
            ddlgridSubStatus.DataTextField = "SUB_STATUS";
            ddlgridSubStatus.DataValueField = "SUB_STATUS";
            ddlgridSubStatus.DataBind();
            ddlgridSubStatus.SelectedValue = hfAgentID.Value.ToUpper();
            //txtDeAllocateDate.Text = hfAgentID.Value;
            TextBox txtremarks = (TextBox)row.FindControl("txtERemarks");
            Label Lblsusbstatus = (Label)row.FindControl("lblAss");

            Label lblSite = (Label)row.FindControl("lblSite");
            Label LblFloor = (Label)row.FindControl("lblFloor");
            Label lblgridStore = (Label)row.FindControl("lblgridStore");


            HiddenField hfFloor = (HiddenField)row.FindControl("hidEFloor");
            HiddenField hfStore = (HiddenField)row.FindControl("hidEStore");

            DropDownList ddlAgentFloor = (DropDownList)row.FindControl("ddlEFloor");
            DataTable dtfloor = new DataTable();
            ddlAgentFloor.DataSource = null;
            dtfloor = oDAL.GetFloor(lblSite.Text.ToString().ToUpper(), Session["COMPANY"].ToString());
            ddlAgentFloor.DataSource = dtfloor;
            ddlAgentFloor.DataTextField = "FLOOR_CODE";
            ddlAgentFloor.DataValueField = "FLOOR_CODE";
            ddlAgentFloor.DataBind();
            ddlAgentFloor.Items.Insert(0, "-- Select Floor --");

            DropDownList ddlAgentStore = (DropDownList)row.FindControl("ddlEStore");
            DataTable dtStore = new DataTable();
            ddlAgentStore.DataSource = null;
            dtStore = oDAL.GetStorewithFloor(lblSite.Text.ToString().ToUpper(), ddlAgentFloor.SelectedValue, Session["COMPANY"].ToString());
            ddlAgentStore.DataSource = dtStore;
            ddlAgentStore.DataTextField = "STORE_CODE";
            ddlAgentStore.DataValueField = "STORE_CODE";
            ddlAgentStore.DataBind();
            ddlAgentStore.Items.Insert(0, "-- Select Store --");
            if (CheckBox1.Checked)
            {
                Lblsusbstatus.Visible = LblFloor.Visible = lblgridStore.Visible = false;
                ddlgridSubStatus.Visible = txtremarks.Visible = ddlAgentFloor.Visible = ddlAgentStore.Visible = true;
                ddlgridSubStatus.SelectedValue = hfAgentID.Value.ToUpper();
            }
            else
            {
                ddlgridSubStatus.Visible = txtremarks.Visible = ddlAgentFloor.Visible = ddlAgentStore.Visible = false;
                Lblsusbstatus.Visible = LblFloor.Visible = lblgridStore.Visible = true;
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
        //... Your code ...
        // Here we find the controls tha we will handle
        CheckBox CheckBoxHeader = (GridView2.HeaderRow.FindControl("chkHSelect") as CheckBox);
        //CheckBox CheckBoxHeader = (CheckBox)row.FindControl("chkSelectAsset");
        //chkAll.Checked = true;
        foreach (GridViewRow row in GridView2.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CheckBox1 = (CheckBox)row.FindControl("chkSelectAsset");
                CheckBox1.Checked = CheckBoxHeader.Checked;
                HiddenField hfAgentID = (HiddenField)row.FindControl("hidESubStatus");
                DropDownList ddlgridSubStatus = (DropDownList)row.FindControl("ddlESubStatus");
                DataTable dt = new DataTable();
                ddlgridSubStatus.DataSource = null;
                dt = oDAL.GetSubStatusGrid();
                ddlgridSubStatus.DataSource = dt;
                ddlgridSubStatus.DataTextField = "SUB_STATUS";
                ddlgridSubStatus.DataValueField = "SUB_STATUS";
                ddlgridSubStatus.DataBind();
                ddlgridSubStatus.SelectedValue = hfAgentID.Value.ToUpper();
                TextBox txtremarks = (TextBox)row.FindControl("txtERemarks");
                Label Lblsusbstatus = (Label)row.FindControl("lblAss");

                Label lblSite = (Label)row.FindControl("lblSite");
                Label LblFloor = (Label)row.FindControl("lblFloor");
                Label lblgridStore = (Label)row.FindControl("lblgridStore");

                HiddenField hfFloor = (HiddenField)row.FindControl("hidEFloor");
                HiddenField hfStore = (HiddenField)row.FindControl("hidEStore");

                DropDownList ddlAgentFloor = (DropDownList)row.FindControl("ddlEFloor");
                DataTable dtfloor = new DataTable();
                ddlAgentFloor.DataSource = null;
                dtfloor = oDAL.GetFloor(lblSite.Text.ToString().ToUpper(), Session["COMPANY"].ToString());
                ddlAgentFloor.DataSource = dtfloor;
                ddlAgentFloor.DataTextField = "FLOOR_CODE";
                ddlAgentFloor.DataValueField = "FLOOR_CODE";
                ddlAgentFloor.DataBind();
                ddlAgentFloor.Items.Insert(0, "-- Select Floor --");


                DropDownList ddlAgentStore = (DropDownList)row.FindControl("ddlEStore");
                DataTable dtStore = new DataTable();
                ddlAgentStore.DataSource = null;
                dtStore = oDAL.GetStorewithFloor(lblSite.Text.ToString().ToUpper(), ddlAgentFloor.SelectedValue, Session["COMPANY"].ToString());
                ddlAgentStore.DataSource = dtStore;
                ddlAgentStore.DataTextField = "STORE_CODE";
                ddlAgentStore.DataValueField = "STORE_CODE";
                ddlAgentStore.DataBind();
                ddlAgentStore.Items.Insert(0, "-- Select Store --");
                if (CheckBox1.Checked)
                {
                    Lblsusbstatus.Visible = LblFloor.Visible = lblgridStore.Visible = false;
                    ddlgridSubStatus.Visible = txtremarks.Visible = ddlAgentFloor.Visible = ddlAgentStore.Visible = true;
                    ddlgridSubStatus.SelectedValue = hfAgentID.Value.ToUpper();
                }
                else
                {
                    ddlgridSubStatus.Visible = txtremarks.Visible = ddlAgentFloor.Visible = ddlAgentStore.Visible = false;
                    Lblsusbstatus.Visible = LblFloor.Visible = lblgridStore.Visible = true;
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
            Label lblSite = (Label)row.FindControl("lblSite");
            DropDownList ddl = (DropDownList)row.FindControl("ddlEFloor");
            DropDownList ddlstore = (DropDownList)row.FindControl("ddlEStore");
            DataTable dtstore = new DataTable();
            ddlstore.DataSource = null;
            dtstore = oDAL.GetStorewithFloor(lblSite.Text.ToString().ToUpper(), ddl.SelectedValue, Session["COMPANY"].ToString());
            ddlstore.DataSource = dtstore;
            ddlstore.DataTextField = "STORE_CODE";
            ddlstore.DataValueField = "STORE_CODE";
            ddlstore.DataBind();
            ddlstore.Items.Insert(0, "-- Select Store --");
        }
    }

    #endregion BUTTON EVENTS

    #region File Upload
    private Tuple<bool, string> ValidateDeallocationFile(DataTable dt)
    {
        bool res = true;
        string msg = "success";
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            i++;
            oPRP.SubStatus = (dr.Field<string>("ASSET_SUB_STATUS") == null || dr.Field<string>("ASSET_SUB_STATUS") == "" || dr.Field<string>("ASSET_SUB_STATUS") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_SUB_STATUS").Trim().ToUpper();
            oPRP.Site = (dr.Field<string>("SITE") == null || dr.Field<string>("SITE") == "" || dr.Field<string>("SITE") == string.Empty) ? string.Empty : dr.Field<string>("SITE").Trim().ToUpper();
            oPRP.Floor = (dr.Field<string>("FLOOR") == null || dr.Field<string>("FLOOR") == "" || dr.Field<string>("FLOOR") == string.Empty) ? string.Empty : dr.Field<string>("FLOOR").Trim().ToUpper();

            if (dr.Field<string>("DEALLOCATION_DATE") == null || dr.Field<string>("DEALLOCATION_DATE") == "" || dr.Field<string>("DEALLOCATION_DATE") == string.Empty)
                oPRP.ActualReturnDate = string.Empty;
            else
                oPRP.ActualReturnDate = dr.Field<string>("DEALLOCATION_DATE").Trim();

            if (dr.Field<string>("REMARKS") == "" || dr.Field<string>("REMARKS") == string.Empty || dr.Field<string>("REMARKS") == null)
            {
                oPRP.DeallocationRemarks = string.Empty;
            }
            else
            {
                oPRP.DeallocationRemarks = dr.Field<string>("REMARKS").Trim();
            }
            oPRP.SerialCode = string.Empty;
            oPRP.Asset_FAR_TAG = string.Empty;
            if (Session["COMPANY"].ToString() == "IT")
            {
                oPRP.SerialCode = (dr.Field<string>("SERIAL_NUMBER") == null || dr.Field<string>("SERIAL_NUMBER") == "" || dr.Field<string>("SERIAL_NUMBER") == string.Empty) ? string.Empty : dr.Field<string>("SERIAL_NUMBER").Trim();
                oPRP.Store = (dr.Field<string>("STORE") == null || dr.Field<string>("STORE") == "" || dr.Field<string>("STORE") == string.Empty) ? string.Empty : dr.Field<string>("STORE").Trim().ToUpper();
                oPRP.AssetCode = oDAL.GetAssetCodeforDeallocation(oPRP.SerialCode, Session["COMPANY"].ToString());
                oPRP.AssetTagID = oDAL.GetRFIDTagforDeallocation(oPRP.SerialCode, Session["COMPANY"].ToString());

                if (oPRP.SerialCode.Trim() == "" || oPRP.SerialCode == string.Empty ||
                    oPRP.SubStatus.Trim() == "" || oPRP.SubStatus == string.Empty ||
                    oPRP.Site.Trim() == "" || oPRP.Site == string.Empty ||
                    oPRP.Floor.Trim() == "" || oPRP.Floor == string.Empty ||
                    oPRP.ActualReturnDate == string.Empty || oPRP.ActualReturnDate == "")
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Please enter the mandatory fields.";
                    res = false;
                    break;
                }

                var dtSLocation = oDAL.GetLocation(oPRP.Site, Session["COMPANY"].ToString());
                if (dtSLocation.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Asset Location is invalid.";
                    res = false;
                    break;
                }
                var dtfloor = oDAL.GetFloorVerify(oPRP.Site, oPRP.Floor, Session["COMPANY"].ToString());
                if (dtfloor.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " floor Code is invalid.";
                    res = false;
                    break;
                }

                if (!string.IsNullOrEmpty(oPRP.Store))
                {
                    var dtstore = oDAL.GetStoreVerify(oPRP.Site, oPRP.Floor, oPRP.Store, Session["COMPANY"].ToString());
                    if (dtfloor.Rows.Count <= 0)
                    {
                        msg = "Please Note : Row Number " + (i + 1).ToString() + " Store Code is invalid.";
                        res = false;
                        break;
                    }
                }

                //DataTable dy = oDAL.GetDeallocationAssetDetails(oPRP.AssetCode.Trim(), oPRP.AssetTagID.Trim());
                //if (dy.Rows.Count == 0)
                //{
                //    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Serial code.";
                //    res = false;
                //    break;
                //}
                if (string.IsNullOrEmpty(oPRP.AssetCode) || oPRP.AssetCode == "")
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Serial Code.";
                    res = false;
                    break;
                }

            }
            else
            {
                oPRP.Asset_FAR_TAG = (dr.Field<string>("ASSET_FAR_TAG") == null || dr.Field<string>("ASSET_FAR_TAG") == "" || dr.Field<string>("ASSET_FAR_TAG") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_FAR_TAG").Trim();
                oPRP.AssetCode = oDAL.GetAssetCodeforDeallocation(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());
                oPRP.AssetTagID = oDAL.GetRFIDTagforDeallocation(oPRP.Asset_FAR_TAG, Session["COMPANY"].ToString());

                if (oPRP.Asset_FAR_TAG.Trim() == "" || oPRP.Asset_FAR_TAG.Trim() == string.Empty ||
                    oPRP.SubStatus.Trim() == "" || oPRP.SubStatus == string.Empty ||
                    oPRP.Site.Trim() == "" || oPRP.Site == string.Empty ||
                    oPRP.Floor.Trim() == "" || oPRP.Floor == string.Empty ||
                    oPRP.ActualReturnDate == string.Empty || oPRP.ActualReturnDate == "")
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Please enter the mandatory fields.";
                    res = false;
                    break;
                }


                if (string.IsNullOrEmpty(oPRP.AssetCode) || oPRP.AssetCode == "")
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Asset Far Tag.";
                    res = false;
                    break;
                }

                var dtSLocation = oDAL.GetLocation(oPRP.Site, Session["COMPANY"].ToString());
                if (dtSLocation.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " Asset Location is invalid.";
                    res = false;
                    break;
                }
                var dtfloor = oDAL.GetFloorVerify(oPRP.Site, oPRP.Floor, Session["COMPANY"].ToString());
                if (dtfloor.Rows.Count <= 0)
                {
                    msg = "Please Note : Row Number " + (i + 1).ToString() + " floor Code is invalid.";
                    res = false;
                    break;
                }
                //DataTable dy = oDAL.GetDeallocationAssetDetails(oPRP.AssetCode.Trim(), oPRP.AssetTagID.Trim());
                //if (dy.Rows.Count == 0)
                //{
                //    msg = "Please Note : Row Number " + (i+1).ToString() + " Invalid Asset Far Tag.";
                //    res = false;
                //    break;
                //}
            }

            var dtSubStatus = oDAL.GetSubStatus(oPRP.SubStatus.ToUpper());
            if (dtSubStatus.Rows.Count <= 0)
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Asset Sub Status.";
                res = false;
                break;
            }
            string _date = "";
            if (!isValidateDate(oPRP.ActualReturnDate.Trim().Replace(" 00:00", "").Trim(), out _date))
            {
                msg = "Please Note : Row Number " + (i + 1).ToString() + " Invalid Deallocation Date.";
                res = false;
                break;
            }
        }
        return new Tuple<bool, string>(res, msg);
    }
    private void SaveAssetDeallocation(DataTable dt)
    {
        Session["ERRORDATA"] = null;

        DataTable dtforEmail = new DataTable();
        dtforEmail.Columns.Add("Asset Code");
        dtforEmail.Columns.Add("Site Location");
        dtforEmail.Columns.Add("Employee Name");
        dtforEmail.Columns.Add("Employee Code");
        dtforEmail.Columns.Add("Emp Email");
        dtforEmail.Columns.Add("Asset Type");
        dtforEmail.Columns.Add("Asset Make");
        dtforEmail.Columns.Add("Asset Model");
        dtforEmail.Columns.Add("Serial Number");
        dtforEmail.Columns.Add("RFID Tag Id");
        dtforEmail.Columns.Add("SubStatus");

        DataTable dtfloorcountonly = new DataTable();
        dtfloorcountonly.Columns.Add("Asset Code");
        dtfloorcountonly.Columns.Add("Site Location");
        dtfloorcountonly.Columns.Add("Floor");
        dtfloorcountonly.Columns.Add("Asset Type");
        dtfloorcountonly.Columns.Add("Asset Make");
        dtfloorcountonly.Columns.Add("Asset Model");
        dtfloorcountonly.Columns.Add("Serial Number");
        dtfloorcountonly.Columns.Add("RFID Tag Id");
        dtfloorcountonly.Columns.Add("SubStatus");

        DataTable dtFacilitiesForEmail = new DataTable();
        dtFacilitiesForEmail.Columns.Add("Asset Code");
        dtFacilitiesForEmail.Columns.Add("Site Location");
        dtFacilitiesForEmail.Columns.Add("Floor");
        dtFacilitiesForEmail.Columns.Add("Asset Type");
        dtFacilitiesForEmail.Columns.Add("Asset Make");
        dtFacilitiesForEmail.Columns.Add("Asset Model");
        dtFacilitiesForEmail.Columns.Add("Asset Far Tag");
        dtFacilitiesForEmail.Columns.Add("RFID Tag Id");
        dtFacilitiesForEmail.Columns.Add("SubStatus");

        DataTable dtInvalidData = new DataTable();
        dtInvalidData = dt.Clone();
        dtInvalidData.Columns.Add("STATUS");
        dtInvalidData.Columns.Add("ERROR_MESSAGE");
        int Cnt = 0;
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            oPRP.SubStatus = (dr.Field<string>("ASSET_SUB_STATUS") == null || dr.Field<string>("ASSET_SUB_STATUS") == "" || dr.Field<string>("ASSET_SUB_STATUS") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_SUB_STATUS").Trim().ToUpper();
            if (dr.Field<string>("DEALLOCATION_DATE") == null || dr.Field<string>("DEALLOCATION_DATE") == "" || dr.Field<string>("DEALLOCATION_DATE") == string.Empty)
                oPRP.ActualReturnDate = string.Empty;
            else
                oPRP.ActualReturnDate = dr.Field<string>("DEALLOCATION_DATE").Trim();
            //oPRP.DeallocationRemarks = dr.Field<string>("REMARKS").Trim() != "" ? dr.Field<string>("REMARKS").Trim() : null;

            if (dr.Field<string>("REMARKS") == "" || dr.Field<string>("REMARKS") == string.Empty || dr.Field<string>("REMARKS") == null)
            {
                oPRP.DeallocationRemarks = " ";
            }
            else
            {
                oPRP.DeallocationRemarks = dr.Field<string>("REMARKS");
            }
            string _date = "";
            if (isValidateDate(oPRP.ActualReturnDate.Trim().Replace(" 00:00", "").Trim(), out _date))
            {
                oPRP.ActualReturnDate = _date;
            }

            if (Session["COMPANY"].ToString() == "IT")
                oPRP.SerialCode = (dr.Field<string>("SERIAL_NUMBER") == null || dr.Field<string>("SERIAL_NUMBER") == "" || dr.Field<string>("SERIAL_NUMBER") == string.Empty) ? string.Empty : dr.Field<string>("SERIAL_NUMBER").Trim();
            else
                oPRP.Asset_FAR_TAG = (dr.Field<string>("ASSET_FAR_TAG") == null || dr.Field<string>("ASSET_FAR_TAG") == "" || dr.Field<string>("ASSET_FAR_TAG") == string.Empty) ? string.Empty : dr.Field<string>("ASSET_FAR_TAG").Trim();

            oPRP.AssetCode = oDAL.GetAssetCodeforDeallocation(Session["COMPANY"].ToString() == "IT" ? dr.Field<string>("SERIAL_NUMBER").Trim() : dr.Field<string>("ASSET_FAR_TAG").Trim(), Session["COMPANY"].ToString());
            oPRP.AssetTagID = oDAL.GetRFIDTagforDeallocation(Session["COMPANY"].ToString() == "IT" ? dr.Field<string>("SERIAL_NUMBER").Trim() : dr.Field<string>("ASSET_FAR_TAG").Trim(), Session["COMPANY"].ToString());

            //oPRP.CompCode = Convert.ToString(Session["COMPCODE"]);
            //oPRP.ModifiedBy = Convert.ToString(Session["CURRENTUSER"]);
            oPRP.CompCode = Session["COMPANY"].ToString();
            oPRP.CreatedBy = oPRP.ModifiedBy = Session["CURRENTUSER"].ToString();

            oPRP.Site = (dr.Field<string>("SITE") == null || dr.Field<string>("SITE") == "" || dr.Field<string>("SITE") == string.Empty) ? string.Empty : dr.Field<string>("SITE").Trim().ToUpper();
            oPRP.Floor = (dr.Field<string>("FLOOR") == null || dr.Field<string>("FLOOR") == "" || dr.Field<string>("FLOOR") == string.Empty) ? string.Empty : dr.Field<string>("FLOOR").Trim().ToUpper();

            if (Session["COMPANY"].ToString() == "IT")
                oPRP.Store = (dr.Field<string>("STORE") == null || dr.Field<string>("STORE") == "" || dr.Field<string>("STORE") == string.Empty) ? string.Empty : dr.Field<string>("STORE").Trim().ToUpper();
            else
                oPRP.Store = string.Empty;


            oPRP.AssetAllocated = false;
            bool bResp = false;
            if (string.IsNullOrEmpty(oPRP.AssetCode) || oPRP.AssetCode == "")
            { bResp = false; }
            else
            {
                DataTable dtAllocationDetails = oDAL.GetAssetAllocationGridDetails(oPRP.AssetCode);
                oPRP.SerialCode = dtAllocationDetails.Rows[0]["SERIAL_CODE"].ToString();
                oPRP.AssetMake = dtAllocationDetails.Rows[0]["ASSET_MAKE"].ToString();
                oPRP.RFIDTag = dtAllocationDetails.Rows[0]["ASSET_TAG"].ToString();
                oPRP.Asset_FAR_TAG = dtAllocationDetails.Rows[0]["ASSET_FAR_TAG"].ToString();
                oPRP.AssetType = dtAllocationDetails.Rows[0]["ASSET_TYPE"].ToString();
                oPRP.HostName = dtAllocationDetails.Rows[0]["ASSET_HOST_NAME"].ToString();
                oPRP.AllocationType = dtAllocationDetails.Rows[0]["ALLOCATION_TYPE"].ToString();
                string dateString = null;
                CultureInfo provider = CultureInfo.InvariantCulture;
                dateString = dtAllocationDetails.Rows[0]["ASSET_ALLOCATION_DATE"].ToString();
                DateTime dateTime13 = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider); // 10/22/2015 12:00:00 AM  
                string temp = dateTime13.ToString();
                oPRP.AllocationDate = dateTime13.ToString("dd/MMM/yyyy");
                string dateString1 = null;
                dateString1 = dtAllocationDetails.Rows[0]["EXPECTED_RTN_DATE"].ToString();
                DateTime dateTime134 = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider); // 10/22/2015 12:00:00 AM  
                oPRP.ExpReturnDate = dateTime134.ToString("dd/MMM/yyyy");
                oPRP.TicketNo = dtAllocationDetails.Rows[0]["TICKET_NO"].ToString();
                oPRP.EmpCode = dtAllocationDetails.Rows[0]["ALLOCATED_EMP_ID"].ToString();
                oPRP.EmpName = dtAllocationDetails.Rows[0]["ASSET_ALLOCATED_EMP"].ToString();
                oPRP.IdentifierLocation = dtAllocationDetails.Rows[0]["Identifier_Location"].ToString();
                oPRP.Status = dtAllocationDetails.Rows[0]["ALLOCATED_STATUS"].ToString();
                oPRP.AllocationId = oDAL.GetAllocationId(oPRP.AssetCode, Session["COMPANY"].ToString());
                bResp = oDAL.SaveAssetAllocation("RETURN", oPRP);
            }

            if (!bResp)
            {
                dtInvalidData.Rows.Add(dr.ItemArray);
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["STATUS"] = "ERROR";
                dtInvalidData.Rows[dtInvalidData.Rows.Count - 1]["ERROR_MESSAGE"] = "Duplicate / Not proper Data";
                i++;
                continue;
            }
            else
            {
                Cnt++;
                if (!string.IsNullOrEmpty(oPRP.EmpCode))
                {
                    string EmailId = oDAL.GetEmpEmail(oPRP.EmpCode);
                    DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                    string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                    string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                    string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                    string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                    dtforEmail.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.EmpName, oPRP.EmpCode, EmailId, AssetType, AssetMake, AssetModel, oPRP.SerialCode, RFIDTag, oPRP.SubStatus);
                    dtforEmail.AcceptChanges();

                }
                else
                {
                    if (Session["COMPANY"].ToString() == "IT")
                    {
                        DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                        string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                        string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                        string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                        string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                        dtfloorcountonly.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.Floor, AssetType, AssetMake, AssetModel, oPRP.SerialCode, RFIDTag, oPRP.SubStatus);
                        dtfloorcountonly.AcceptChanges();
                    }
                    else
                    {
                        DataTable dtAssetDet = oDAL.GetAssetDetailsforEmail(oPRP.AssetCode, Session["COMPANY"].ToString());
                        string AssetType = (dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_TYPE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_TYPE").Trim();
                        string AssetMake = (dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == null || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == "" || dtAssetDet.Rows[0].Field<string>("ASSET_MAKE") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("ASSET_MAKE").Trim();
                        string AssetModel = (dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == null || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == "" || dtAssetDet.Rows[0].Field<string>("MODEL_NAME") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("MODEL_NAME").Trim();
                        string RFIDTag = (dtAssetDet.Rows[0].Field<string>("TAG_ID") == null || dtAssetDet.Rows[0].Field<string>("TAG_ID") == "" || dtAssetDet.Rows[0].Field<string>("TAG_ID") == string.Empty) ? "" : dtAssetDet.Rows[0].Field<string>("TAG_ID").Trim();
                        dtFacilitiesForEmail.Rows.Add(oPRP.AssetCode, oPRP.Site, oPRP.Floor, AssetType, AssetMake, AssetModel, oPRP.Asset_FAR_TAG, RFIDTag, oPRP.SubStatus);
                        dtFacilitiesForEmail.AcceptChanges();
                    }
                }

            }

        }
        if (Cnt > 0)
        {
            if (dtfloorcountonly.Rows.Count > 0 || dtFacilitiesForEmail.Rows.Count > 0)
            {
                //dtfloorforIT - IT - Facilities - this datatable contains Floor related assetcode and SerialCode / dtfloorforFacilities - this datatable contains Floor related assetcode and Asset Far Tag
                DataTable dp = oDAL.GetMailTransactionDetails("ASSET_DEALLOCATION", Convert.ToString(Session["COMP_NAME"]));
                if (dp.Rows.Count > 0)
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
                        clsGeneral.LogErrorToLogFile(ee, "Asset DeAllocation");
                        clsGeneral.LogErrorToLogFile(ee.InnerException, "Asset DeAllocation");
                    }
                }
            }

            if(dtforEmail.Rows.Count>0)
            {
                DataTable dp1 = oDAL.GetMailTransactionDetails("ASSET_DEALLOCATION", Convert.ToString(Session["COMP_NAME"]));
                string Subject = "Assets Deallocated";
                string MailBody = "";
                string CCAddress = "";
                if (dp1.Rows.Count > 0)
                {
                    CCAddress = (dp1.Rows[0].Field<string>("CC_MAIL_ID") == null || dp1.Rows[0].Field<string>("CC_MAIL_ID") == "" || dp1.Rows[0].Field<string>("CC_MAIL_ID") == string.Empty) ? "" : dp1.Rows[0].Field<string>("CC_MAIL_ID").Trim();
                    Subject = (dp1.Rows[0].Field<string>("MAIL_SUBJECT") == null || dp1.Rows[0].Field<string>("MAIL_SUBJECT") == "" || dp1.Rows[0].Field<string>("MAIL_SUBJECT") == string.Empty) ? "" : dp1.Rows[0].Field<string>("MAIL_SUBJECT").Trim();
                    MailBody = (dp1.Rows[0].Field<string>("MAIL_BODY") == null || dp1.Rows[0].Field<string>("MAIL_BODY") == "" || dp1.Rows[0].Field<string>("MAIL_BODY") == string.Empty) ? "" : dp1.Rows[0].Field<string>("MAIL_BODY").Trim();
                }
                var dataRows = dtforEmail.AsEnumerable().GroupBy(row => new
                {
                    EmailId = row.Field<string>("Emp Email")
                }).Select(G => G.First()).CopyToDataTable();
                foreach (DataRow row in dataRows.Rows)
                {
                    var assetRows = dtforEmail.Select("[Emp Email]='" + Convert.ToString(row["Emp Email"]) + "'");
                    try
                    {
                        SendmailAlert sendmail = new SendmailAlert();
                        sendmail.FunctionSendingAqcuisitionMail(assetRows.CopyToDataTable(), Convert.ToString(row["Emp Email"]), CCAddress, Subject, MailBody);
                    }
                    catch (Exception ee)
                    {
                        clsGeneral.LogErrorToLogFile(ee, "Employee Asset DeAllocation");
                        clsGeneral.LogErrorToLogFile(ee.InnerException, "Employee Asset DeAllocation");
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

    protected void btnAllSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string AssetMake = txtAssetMake.Text.Trim() != "" ? txtAssetMake.Text.Trim() : null;
            string AssetModel = txtAssetModel.Text.Trim() != "" ? txtAssetModel.Text.Trim() : null;
            string SerialNo = txtAssetSerialNo.Text.Trim() != "" ? txtAssetSerialNo.Text.Trim() : null;
            string AssetType = txtAssetType.Text.Trim() != "" ? txtAssetType.Text.Trim() : null;
            string SiteLocation = string.Empty;
            if (ddlSiteLocationFilter.SelectedIndex == 0 || ddlSiteLocationFilter.SelectedValue == "ALL")
                SiteLocation = string.Empty;
            else
                SiteLocation = ddlSiteLocationFilter.SelectedValue;
            //string SiteLocation = ddlSiteLocationFilter.SelectedValue != "" ? ddlSiteLocationFilter.SelectedValue : null;
            string AssetID = txtAssetID.Text.Trim() != "" ? txtAssetID.Text.Trim() : null;
            string AssetCode = txtAssetCode.Text.Trim() != "" ? txtAssetCode.Text.Trim() : null;
            string TagId = txtRFIDTag.Text.Trim() != "" ? txtRFIDTag.Text.Trim() : null;
            string EmpID = txtEmployeeId.Text.Trim() != "" ? txtEmployeeId.Text.Trim() : null;
            string EMPName = txtEmployeeName.Text.Trim() != "" ? txtEmployeeName.Text.Trim() : null;
            string Floor = txtFloor.Text.Trim() != "" ? txtFloor.Text.Trim() : null;
            string SeatNo = txtSeatNo.Text.Trim() != "" ? txtSeatNo.Text.Trim() : null;
            string Designation = txtDesignation.Text.Trim() != "" ? txtDesignation.Text.Trim() : null;
            string ProcessName = txtProcessName.Text.Trim() != "" ? txtProcessName.Text.Trim() : null;
            string AssetSubStatus = txtAssetSubStatus.Text.Trim() != "" ? txtAssetSubStatus.Text.Trim() : null;
            string FilterAssetFarTag = txtAssetFarTag.Text.Trim() != "" ? txtAssetFarTag.Text.Trim() : null;
            string FilterAssetDomain = txtAssetDomain.Text.Trim() != "" ? txtAssetDomain.Text.Trim() : null;
            string FilterAssetHDD = txtAssetHDD.Text.Trim() != "" ? txtAssetHDD.Text.Trim() : null;
            string FilterAssetProcessor = txtAssetProcessor.Text.Trim() != "" ? txtAssetProcessor.Text.Trim() : null;
            string FilterAssetRAM = txtAssetRAM.Text.Trim() != "" ? txtAssetRAM.Text.Trim() : null;
            string FilterAssetPONumber = txtPONumber.Text.Trim() != "" ? txtPONumber.Text.Trim() : null;
            string FilterAssetVendor = txtAssetVendor.Text.Trim() != "" ? txtAssetVendor.Text.Trim() : null;
            string FilterAssetInvoiceNumber = txtAssetInvoiceNo.Text.Trim() != "" ? txtAssetInvoiceNo.Text.Trim() : null;
            //string FilterAssetRFIDTag = txtRFIDTag.Text.Trim() != "" ? txtRFIDTag.Text.Trim() : null;
            string FilterAssetGRNNo = txtGRNNumber.Text.Trim() != "" ? txtGRNNumber.Text.Trim() : null;


            oPRP.CompCode = Session["COMPANY"].ToString();
            DataTable dt = new DataTable();
            dt = oDAL.GetAssetDetailsForDeAllocation(AssetID, AssetCode, TagId, EmpID, EMPName, Floor, SeatNo, Designation, ProcessName, AssetSubStatus, AssetMake, AssetModel, AssetType, SiteLocation, SerialNo, "Deallocation", oPRP.CompCode, FilterAssetFarTag, FilterAssetDomain, FilterAssetHDD, FilterAssetProcessor, FilterAssetRAM, FilterAssetPONumber, FilterAssetVendor, FilterAssetInvoiceNumber, FilterAssetGRNNo);
            if (dt.Rows.Count > 0)
            {
                GridView2.DataSource = dt;
                GridView2.DataBind();
                GridView2.Visible = true;
                btnSubmit.Enabled = true;
                if (Session["COMPANY"].ToString() == "IT")
                {
                    GridView2.Columns[8].Visible = false;
                }
                else
                {
                    GridView2.Columns[2].Visible = false;
                    GridView2.Columns[15].Visible = false;
                    GridView2.Columns[16].Visible = false;
                    GridView2.Columns[17].Visible = false;
                    GridView2.Columns[18].Visible = false;
                    GridView2.Columns[22].Visible = false;
                    GridView2.Columns[27].Visible = false;
                    GridView2.Columns[28].Visible = false;
                }
            }
            else
            {
                GridView2.DataSource = null;
                GridView2.DataBind();
                GridView2.Visible = false;
                btnSubmit.Enabled = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Invalid search filters or assets are assigned.');", true);
                return;
            }

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void btnUploadFile_Click(object sender, EventArgs e)
    {
        try
        {
            ConvertToExcel convertToExcel = new ConvertToExcel();
            var response = convertToExcel.ValidateFileReaded(AssetFileUpload, Session["COMPANY"].ToString() == "IT" ? "ITDeallocation" : "FacilitiesDeallocation");
            if (response.Item1)
            {
                DataTable dt = response.Item2;
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('No Data in the file');", true);
                    return;
                }
                var validate = ValidateDeallocationFile(dt);
                if (validate.Item1)
                {
                    SaveAssetDeallocation(dt);
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