using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.Sql;
using System.IO;
using System.Linq;
using MobiVUE_ATS.MyDLL;

public partial class TagMaster : System.Web.UI.Page
{
    TagMaster_DAL oDAL;
    LocationMaster_PRP oPRP;

     public TagMaster()
    {
        oPRP = new LocationMaster_PRP();
    }
     ~TagMaster()
    {
        oDAL = null; oPRP = null;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        GetTagDetails();
    }
    protected void btnUpload_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string _strRights = clsGeneral.GetRights("TAG_MASTER", (DataTable)Session["UserRights"]);
            clsGeneral._strRights = _strRights.Split('^');
            clsGeneral.LogUserOperationToLogFile(Session["CURRENTUSER"].ToString(), Session["COMP_NAME"].ToString(), "TAG_MASTER");
            if (clsGeneral._strRights[0] == "0")
            {
                Response.Redirect("UnauthorizedUser.aspx", false);
            }
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);

            //if (clsGeneral._strRights[1] == "0")
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
            //    return;
            //}
            Session["Tag"] = null;
            
            UploadTagFile();
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }
    }

    private void UploadTagFile()
    {
        string strFilePath = TagMasterFileUpload.FileName;
        if (strFilePath != "")
        {
            String fileExt = System.IO.Path.GetExtension(TagMasterFileUpload.FileName);
            if (fileExt.ToUpper() != ".XLSX" && fileExt.ToUpper() != ".XLS")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Select an excel file (.xls/.xlsx) only.');", true);
                return;
            }
          

            if (TagMasterFileUpload.HasFile)
            {
                
                   strFilePath = Request.PhysicalApplicationPath + "UploadedFiles\\" + TagMasterFileUpload.FileName;
                    File.Delete(strFilePath);
                TagMasterFileUpload.SaveAs(strFilePath);
                   
            }
                MyExcel oExcel = new MyExcel();
               DataTable  dtFileData = oExcel.ReadExcel(strFilePath);
                Session["Tag"] = dtFileData;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Enter Submit to save/update file data.');", true);
                btnSubmit.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Operation Failed! Please Try again.');", true);
                return;
            }
        }

    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
            //if (clsGeneral._strRights[1] == "0")
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
            //    return;
            //}
            if (Session["Tag"] != null)
            {


                SaveTagDetails();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : tag Master is saved successfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Upload tag master file first.');", true);
            }
        }
        catch (Exception ex)
        {
            HandleExceptions(ex);
        }

    }

    private void SaveTagDetails()
    {
        TagMaster_DAL oDAL = new TagMaster_DAL(Session["DATABASE"].ToString());
        TagMaster_PRP oPRP = new TagMaster_PRP();
        try
        {
            bool bInValid = false;
            //if (clsGeneral._strRights[1] == "0")
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowUnAuthorisedMsg", "ShowUnAuthorisedMsg();", true);
            //    return;
            //}
         DataTable   dtFileData = (DataTable)Session["Tag"];

            Session["Tag"] = null;
            if (dtFileData.Columns.Count != 2)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Invalid File format.", true);
                bInValid = true;
                return;
            }
            for (int iCnt = 0; iCnt < dtFileData.Rows.Count; iCnt++)
            {
          
                if (iCnt + 1 == dtFileData.Rows.Count)
                {
                    if (dtFileData.Rows[iCnt][0].ToString().Trim() == "")
                        break;
                }

                if (dtFileData.Rows[iCnt][0].ToString().Trim() != "")
                    oPRP.SerialNo = dtFileData.Rows[iCnt][0].ToString().Trim().ToUpper();
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : Serial No is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }

                //var data = from c in dtFileData.AsEnumerable()
                //           where c.Field<string>("SerialNo").ToUpper().Trim() == Convert.ToString(dtFileData.Rows[iCnt][0]).ToUpper().Trim()
                //           select c;
                //if (data.AsDataView().Count > 0)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlertShowAlert('Please Note : duplicate serial no " + (iCnt + 1).ToString() + ".');", true);
                //    bInValid = true;
                //    break;
                //}
                if (dtFileData.Rows[iCnt][1].ToString().Trim() != "")
                    oPRP.Active = Convert.ToBoolean(dtFileData.Rows[iCnt][1]);
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlertShowAlert('Please Note : Active is left blank at row no. " + (iCnt + 1).ToString() + ".');", true);
                    bInValid = true;
                    break;
                }
                
                oPRP.CreatedBy = Session["CURRENTUSER"].ToString();
                oDAL.UploadTAgDetails(oPRP);
            }
            if (!bInValid)
            {
                GetTagDetails();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowErrMsg", "ShowErrMsg('Please Note : Tag Master data is saved successfully.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowAlert", "ShowAlert('Please Note : An error has occured while uploading tag details, check file data.');", true);
            HandleExceptions(ex);
        }
        finally
        { oPRP = null; oDAL = null;  }
    }

    private void GetTagDetails()
    {
        DataTable dt = new DataTable();
        oDAL = new TagMaster_DAL(Session["DATABASE"].ToString());
        dt = oDAL.GetTags();
        gvRptAssetStock.DataSource = Session["TagMaster"] = dt;
        gvRptAssetStock.DataBind();
    }

    protected void gvRptAssetStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["TagMaster"];
            gvRptAssetStock.PageIndex = e.NewPageIndex;
            gvRptAssetStock.DataSource = dt;
            gvRptAssetStock.DataBind();
        }
        catch (Exception ex)
        { HandleExceptions(ex); }
    }

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


}