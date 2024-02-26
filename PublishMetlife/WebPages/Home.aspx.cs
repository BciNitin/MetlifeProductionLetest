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
using System.DirectoryServices;
using System.Text;

public partial class Home : System.Web.UI.Page
{
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
            Server.Transfer("UserLogin.aspx");
        }
        lblLoggedUser.Text = Session["CURRENTUSER"].ToString();
        if (Session["COMP_NAME"] != null)
            lblLoggedLocation.Text = Session["COMP_NAME"].ToString();
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
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
                GetPendingCallLogs();
                GetPendingGatePass();
                GetExpiredAMCCount();
                if(Session["COMPANY"].ToString()=="IT")
                    DynamicControls();
                //else { TableStoreList.Visible = false; }
            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    /// <summary>
    /// Get Amc list about to expire.
    /// </summary>
    private void GetExpiredAMCCount()
    {
        AssetAMC_DAL oDAL = new AssetAMC_DAL(Session["DATABASE"].ToString());
        try
        {
            DataTable dt = oDAL.GetDashbordCount(Session["COMPANY"].ToString());
            foreach (DataRow row in dt.Rows)
            {
                string ColumnName = row.Field<string>("OperationName");
                string count = Convert.ToString( row.Field<int>("AssetCount"));

                switch (ColumnName)
                {
                    case "Asset Stock":
                        lblAcquisition.Text = count;
                        break;
                    case "Asset Allocation":
                        lblAllocation.Text = count;
                        break;
                    case "Asset Transfer":
                        lblTransfer.Text = count;
                        break;
                    case "Asset Scrapped":
                        lblScrap.Text = count;
                        break;
                }

            }
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
        finally
        { oDAL = null; }
    }

    /// <summary>
    /// Get pending call log list.
    /// </summary>
    private void GetPendingGatePass()
    {
        GatePassGeneration_DAL oDAL = new GatePassGeneration_DAL(Session["DATABASE"].ToString());
        try
        {
            //lblPendingAsset.Text = oDAL.GetPendingGatePass(Session["COMPANY"].ToString());
            //if (lblPendingAsset.Text == "0")
            //    pnlEXGP.Enabled = false;
            //else
            //    pnlEXGP.Enabled = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
        finally
        { oDAL = null; }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetPendingCallLogs()
    {
        CallLog_DAL oDAL = new CallLog_DAL(Session["DATABASE"].ToString());
        try
        {
            //lblPendingCall.Text = oDAL.GetPendingCallLogs(Session["COMPANY"].ToString());
            //if (lblPendingCall.Text == "0")
            //    pnlPCL.Enabled = false;
            //else
            //    pnlPCL.Enabled = true;
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
        finally
        { oDAL = null; }
    }

    /// <summary>
    /// Cathes exception for the entier page operations.
    /// </summary>
    /// <param name="ex"></param>
    public void HandleExceptions(Exception ex)
    {
        clsGeneral.LogErrorToLogFile(ex, "Home Page");
        if (!ex.Message.ToString().Contains("Thread was being aborted."))
        {
            clsGeneral.ErrMsg = ex.Message.ToString(); try { string[] arrErr = ex.Message.ToString().Split('\n'); Session["ErrMsg"] = arrErr[0].ToString().Trim(); }
            catch { }
            Response.Redirect("Error.aspx", false);
        }
    }

    /// <summary>
    /// Active Directory access point.
    /// </summary>
    private void GetDirectoryServicesAccess()
    {
        string EntityCode, LoggedInUserId = "";
        DirectoryEntry DE = new DirectoryEntry();
        DE.Path = "LDAP://FNBCIL.COM";
        DirectorySearcher DS = new DirectorySearcher();
        DS.PropertiesToLoad.Add("departmantNumber");
        DS.Filter = "(&(objectClass=User)(sAMAccountName=" + LoggedInUserId.Substring(LoggedInUserId.LastIndexOf(':') + 1) + "))";

        SearchResult result = DS.FindOne();
        if (result != null && result.Properties["departmantNumber"] != null && result.Properties["departmantNumber"].Count > 0)
        {
            EntityCode = result.Properties["departmantNumber"][0].ToString().Substring(result.Properties["departmantNumber"][0].ToString().Length - 4);
        }
    }

    public void DynamicControls()
    {
        StoreMaster_DAL ODAL = new StoreMaster_DAL(Session["DATABASE"].ToString());
        System.Data.DataTable dt = ODAL.GetStoreforHome(Session["COMPANY"].ToString());
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            //cell.Attributes.Add("class", "text-right");
            cell.Attributes.Add("class", "pageAnchor");
            HyperLink hl = new HyperLink()
            {
                Text = clsGeneral.CapitalizeFirst(Convert.ToString(dt.Rows[i]["STORE"])),
                NavigateUrl = "Reports.aspx?ReportID=STORE~"+ Convert.ToString(dt.Rows[i]["STORE_NAME"]) + "",
                CssClass = "pageAnchor",
                ToolTip = "add a caption to title attribute"
            };
            cell.Controls.Add(hl);
            //for(int j=0;j<45;j++)
            //{
            //    HtmlTableCell cellspace = new HtmlTableCell();
            //    row.Cells.Add(cellspace);
            //}
            row.Cells.Add(cell);
            TableStoreList.Rows.Add(row);
        }
    }

    //protected void BuildTable(DataTable dt)
    //{
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        //add new row every 5 rows
    //        if (i % 5 == 0)
    //        {
    //            imageRow = new TableRow();
    //            textRow = new TableRow();
    //        }
    //        HyperLink Link1 = new HyperLink();
    //        Link1.ImageUrl = "~/Images/House.jpg";
    //        Button Button1 = new Button();
    //        Button1.BackColor = System.Drawing.Color.Transparent;
    //        Button1.Text = dt.Rows[i][0].ToString();
    //        Button1.BorderStyle = BorderStyle.None;
    //        Button1.Click += new EventHandler(Button1_Click);
    //        TableCell imagecell = new TableCell();
    //        TableCell textcell = new TableCell();

    //        imagecell.Controls.Add(Link1);
    //        textcell.Controls.Add(Button1);
    //        //textcell.Text = dt.Rows[i][0].ToString();
    //        textcell.HorizontalAlign = HorizontalAlign.Center;
    //        imagecell.BorderStyle = BorderStyle.Solid;
    //        textcell.BorderStyle = BorderStyle.Solid;
    //        imagecell.BorderWidth = 3;
    //        textcell.BorderWidth = 3;
    //        imageRow.Cells.Add(imagecell);
    //        textRow.Cells.Add(textcell);
    //        Table1.Rows.Add(imageRow);
    //        Table1.Rows.Add(textRow);
    //        Table1.CellSpacing = 20;
    //        Table1.BorderStyle = BorderStyle.Solid;
    //        Table1.BorderWidth = 2;
    //        Table1.BorderColor = System.Drawing.Color.SteelBlue;
    //    }

    //}


    #endregion
}