using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DepriciationMaster : System.Web.UI.Page
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CURRENTUSER"] == null)
        {
            Server.Transfer("UserLogin.aspx");
        }
        else if (Session["CURRENTUSER"].ToString() == "test")
        {
            Server.Transfer("UnauthorizedUser.aspx");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindData();
    }

    /// <summary>
    /// 
    /// </summary>
    private void BindData()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("DEPRECIATION_ID", typeof(string));
        dt.Columns.Add("CATEGORY", typeof(string));
        dt.Columns.Add("EFFECTIVE_DATE", typeof(string));
        dt.Columns.Add("DEPRECIATION_RATE", typeof(string));
        dt.Columns.Add("REMARKS", typeof(string));        

        dr = dt.NewRow();
        dr["DEPRECIATION_ID"] = "1";
        dr["CATEGORY"] = "CAT01";
        dr["EFFECTIVE_DATE"] = "12/07/2012";
        dr["DEPRECIATION_RATE"] = "3.5%";
        dr["REMARKS"] = "Depreciation @ 3.5%";        
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["DEPRECIATION_ID"] = "2";
        dr["CATEGORY"] = "CAT02";
        dr["EFFECTIVE_DATE"] = "16/04/2012";
        dr["DEPRECIATION_RATE"] = "5.6%";
        dr["REMARKS"] = "Depreciation @ 5.6%";        
        dt.Rows.Add(dr);

        gvDepMaster.DataSource = dt;
        gvDepMaster.DataBind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDepMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int iRowIndex = e.RowIndex;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDepMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvDepMaster.EditIndex = e.NewEditIndex;
        BindData();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDepMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {


        //At last...
        gvDepMaster.EditIndex = -1;
        BindData();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDepMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDepMaster.EditIndex = -1;
        BindData();
    }

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
        {
            throw ex;
        }
    }
}
