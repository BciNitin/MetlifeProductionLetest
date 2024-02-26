using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomerMaster : System.Web.UI.Page
{
    #region PAGE EVENTS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("SessionExpired.aspx");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
                BindData();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// 
    /// </summary>
    private void BindData()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("CUSTOMER_CODE", typeof(string));
        dt.Columns.Add("CUSTOMER_NAME", typeof(string));
        dt.Columns.Add("COMPANY_ID", typeof(string));
        dt.Columns.Add("CUSTOMER_TAG", typeof(string));
        dt.Columns.Add("ADDRESS", typeof(string));
        dt.Columns.Add("COUNTRY", typeof(string));
        dt.Columns.Add("STATE", typeof(string));
        dt.Columns.Add("CITY", typeof(string));
        dt.Columns.Add("PHONE", typeof(string));
        dt.Columns.Add("EMAIL", typeof(string));
        dt.Columns.Add("ACTIVE", typeof(bool));

        dr = dt.NewRow();
        dr["CUSTOMER_CODE"] = "CU001";
        dr["CUSTOMER_NAME"] = "Bar Code India Ltd.";
        dr["COMPANY_ID"] = "BCIL";
        dr["CUSTOMER_TAG"] = "Software";
        dr["ADDRESS"] = "Udyog Vihar";
        dr["COUNTRY"] = "India";
        dr["STATE"] = "Haryana";
        dr["CITY"] = "Gurgaon";
        dr["PHONE"] = "9934543553";
        dr["EMAIL"] = "test@barcodeindia.com";
        dr["ACTIVE"] = true;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["CUSTOMER_CODE"] = "CU002";
        dr["CUSTOMER_NAME"] = "Fidelity Information System";
        dr["COMPANY_ID"] = "BCIL";
        dr["CUSTOMER_TAG"] = "Vendor";
        dr["ADDRESS"] = "DLF - 4";
        dr["COUNTRY"] = "India";
        dr["STATE"] = "New Delhi";
        dr["CITY"] = "New Delhi";
        dr["PHONE"] = "9912324249";
        dr["EMAIL"] = "services@fisglobal.com";
        dr["ACTIVE"] = false;
        dt.Rows.Add(dr);
        gvCustMaster.DataSource = dt;
        gvCustMaster.DataBind();
    }

    public void HandleExceptions(Exception ex)
    {
        lblErrorMsg.Text = ex.Message.ToString();
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            //oBL_ClsLog.SaveLog(Convert.ToString(Session["CURRENTUSER"]).Trim(), "Exception", ex.Message.ToString(), "GROUP MASTER");
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { } Server.Transfer("Error.aspx");
        }
    }
    #endregion

    #region BUTTON EVENTS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region GRIDVIEW EVENTS
    protected void gvCustMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvCustMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            gvCustMaster.EditIndex = e.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvCustMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            //At last...
            gvCustMaster.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

    protected void gvCustMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvCustMaster.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion
}