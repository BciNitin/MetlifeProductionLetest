using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MobiVUE_ATS.PRP;

namespace MobiVUE_ATS.DAL
{

    /// <summary>
    /// Summary description for SiteMaster_DAL
    /// </summary>
    public class SiteMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public SiteMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~SiteMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        private bool CheckDuplicate(string _SiteCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM SITE_MASTER WHERE SITE_CODE = '" + _SiteCode.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool isSiteExistAnywhere(string _SiteCode)
        {
            try
            {
                DataTable dt = new DataTable();
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE ASSET_LOCATION = '" + _SiteCode.Trim().Replace("'", "''") + "' OR ORIGINAL_LOCATION = '" + _SiteCode.Trim().Replace("'", "''") + "' ");
                dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                else
                {
                    bDup = false;
                    dt.Rows.Clear();
                    dt.AcceptChanges();
                    sbQuery = new StringBuilder();
                    sbQuery.Append("SELECT * FROM ASSET_ALLOCATION WHERE SITE = '" + _SiteCode.Trim().Replace("'", "''") + "'");
                    dt = oDb.GetDataTable(sbQuery.ToString());
                    if (dt.Rows.Count > 0)
                        bDup = true;
                    else
                    {
                        bDup = false;
                        dt.Rows.Clear();
                        dt.AcceptChanges();
                        sbQuery = new StringBuilder();
                        sbQuery.Append("SELECT * FROM ASSET_DEALLOCATION WHERE SITE = '" + _SiteCode.Trim().Replace("'", "''") + "'");
                        dt = oDb.GetDataTable(sbQuery.ToString());
                        if (dt.Rows.Count > 0)
                            bDup = true;
                        else
                        {
                            bDup = false;
                            dt.Rows.Clear();
                            dt.AcceptChanges();
                            sbQuery = new StringBuilder();
                            sbQuery.Append("SELECT * FROM GATEPASS_ASSETS WHERE GATEPASS_IN_LOC = '" + _SiteCode.Trim().Replace("'", "''") + "' OR GATEPASS_OUT_LOC = '" + _SiteCode.Trim().Replace("'", "''") + "'");
                            dt = oDb.GetDataTable(sbQuery.ToString());
                            if (dt.Rows.Count > 0)
                                bDup = true;
                            else
                            {
                                bDup = false;
                                dt.Rows.Clear();
                                dt.AcceptChanges();
                                sbQuery = new StringBuilder();
                                sbQuery.Append("SELECT * FROM GATEPASS_GENERATION WHERE ASSET_LOCATION = '" + _SiteCode.Trim().Replace("'", "''") + "' OR DESTINATION_LOCATION = '" + _SiteCode.Trim().Replace("'", "''") + "' ");
                                dt = oDb.GetDataTable(sbQuery.ToString());
                                if (dt.Rows.Count > 0)
                                    bDup = true;
                                else
                                {
                                    bDup = false;
                                    dt.Rows.Clear();
                                    dt.AcceptChanges();
                                    sbQuery = new StringBuilder();
                                    sbQuery.Append("SELECT * FROM TRAINING_AND_MEETING_MASTER WHERE SITE_CODE = '" + _SiteCode.Trim().Replace("'", "''") + "'");
                                    dt = oDb.GetDataTable(sbQuery.ToString());
                                    if (dt.Rows.Count > 0)
                                        bDup = true;
                                    else
                                    {
                                        bDup = false;
                                        dt.Rows.Clear();
                                        dt.AcceptChanges();
                                        sbQuery = new StringBuilder();
                                        sbQuery.Append("SELECT * FROM STORE_MASTER WHERE SITE_CODE = '" + _SiteCode.Trim().Replace("'", "''") + "'");
                                        dt = oDb.GetDataTable(sbQuery.ToString());
                                        if (dt.Rows.Count > 0)
                                            bDup = true;
                                        else
                                        {
                                            bDup = false;
                                            dt.Rows.Clear();
                                            dt.AcceptChanges();
                                            sbQuery = new StringBuilder();
                                            sbQuery.Append("SELECT * FROM FLOOR_MASTER WHERE SITE_CODE = '" + _SiteCode.Trim().Replace("'", "''") + "'");
                                            dt = oDb.GetDataTable(sbQuery.ToString());
                                            if (dt.Rows.Count > 0)
                                                bDup = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                } 
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public bool SaveUpdateSite(string OpType, SiteMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                int iRes = 0;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicate(oPRP.SiteCode))
                    {

                        //Add new Category...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [SITE_MASTER] ([SITE_CODE],[SITE_ADDRESS],[CONTACT_NO],[DESCRIPTION],[ACTIVE],CREATED_ON,COMP_CODE)");
                        // sbQuery.Append(" [REMARKS],[CREATED_BY],[CREATED_ON],[CATEGORY_LEVEL],[ASSET_TYPE],[COMP_CODE],[CATEGORY_INITIALS])");
                        sbQuery.Append(" VALUES('" + oPRP.SiteCode + "','" + oPRP.SiteAddress + "','" + oPRP.ContactNo + "','" + oPRP.Description + "','" + oPRP.Active + "',GETDATE(),'"+oPRP.CompCode + "')");
                        //   sbQuery.Append(" '" + oPRP.Remarks + "','" + oPRP.CreatedBy + "',GETDATE()," + oPRP.CategoryLevel + ",'" + oPRP.AssetType + "','" + oPRP.CompCode + "','" + oPRP.CategoryInitials + "')");
                        iRes = oDb.ExecuteQuery(sbQuery.ToString());
                    }

                }
                else if (OpType == "UPDATE")
                {
                    //  if (!CheckDuplicateName(oPRP.CategoryName, oPRP.AssetType, oPRP.CategoryCode))
                    {
                        //Update Category Information...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("UPDATE [SITE_MASTER] SET [SITE_ADDRESS]='" + oPRP.SiteAddress + "', [CONTACT_NO] = '" + oPRP.ContactNo + "', [DESCRIPTION] = '" + oPRP.Description + "'");
                        //  sbQuery.Append(" ,[ACTIVE] = '" + oPRP.Active + "', [REMARKS] = '" + oPRP.Remarks + "', [MODIFIED_BY] = '" + oPRP.ModifiedBy + "', [ASSET_TYPE] = '" + oPRP.AssetType + "'");
                        sbQuery.Append((" ,[ACTIVE] = '" + oPRP.Active + "' WHERE SITE_CODE='" + oPRP.SiteCode + "'"));
                        iRes = oDb.ExecuteQuery(sbQuery.ToString());
                    }
                }

                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetSite(string CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * from SITE_MASTER WHERE COMP_CODE = '"+CompCode+"' ORDER BY SITE_ID DESC ");
                // sbQuery.Append(" REMARKS,ACTIVE,CREATED_BY,CONVERT(VARCHAR,CREATED_ON,105) AS CREATED_ON FROM CATEGORY_MASTER");
                //   sbQuery.Append(" ORDER BY CATEGORY_CODE,PARENT_CATEGORY");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }
        public string DeleteLocation(string _LocCode, string _CompCode)
        {
            try
            {
                string DelRslt = "";
                sbQuery = new StringBuilder();

                sbQuery.Append("DELETE FROM [SITE_MASTER] WHERE [SITE_CODE] = '" + _LocCode.Trim() + "' ");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    DelRslt = "SUCCESS";
                return DelRslt;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}