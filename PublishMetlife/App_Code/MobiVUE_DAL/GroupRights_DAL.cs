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
    /// Summary description for GroupRights_DAL
    /// </summary>
    public class GroupRights_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public GroupRights_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~GroupRights_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Fetching group master details for group population.
        /// </summary>
        /// <returns></returns>
        public DataTable GetGroup(string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GROUP_CODE,GROUP_NAME FROM GROUP_MASTER WHERE ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetching group rights details to be populated.
        /// </summary>
        /// <param name="_GroupCode"></param>
        /// <returns></returns>
        public DataTable GetGroupRights(string _GroupCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GR.*,GM.ASSET_TYPE,PM.PAGE_DESCRIPTION,GM.ASSET_TYPE FROM GROUP_RIGHTS GR");
            sbQuery.Append(" INNER JOIN PAGE_MASTER PM ON GR.PAGE_CODE = PM.PAGE_CODE");
            sbQuery.Append(" INNER JOIN GROUP_MASTER GM ON GR.GROUP_CODE = GM.GROUP_CODE");
            sbQuery.Append(" WHERE GR.GROUP_CODE='" + _GroupCode + "'");
            sbQuery.Append("  ORDER BY GR.PAGE_CODE");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Update group rights for a page into GROUP_RIGHTS data table.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SaveUpdateGroupRights(DataTable dt, GroupRights_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE GROUP_RIGHTS SET VIEW_RIGHTS='" + dt.Rows[iCnt]["VIEW_RIGHTS"] + "', SAVE_RIGHTS='" + dt.Rows[iCnt]["SAVE_RIGHTS"] + "', EDIT_RIGHTS='" + dt.Rows[iCnt]["EDIT_RIGHTS"] + "',");
                    sbQuery.Append(" DELETE_RIGHTS='" + dt.Rows[iCnt]["DELETE_RIGHTS"] + "', EXPORT_RIGHTS='" + dt.Rows[iCnt]["EXPORT_RIGHTS"] + "'");
                    sbQuery.Append(" WHERE GROUP_CODE='" + dt.Rows[iCnt]["GROUP_CODE"] + "' AND PAGE_CODE='" + dt.Rows[iCnt]["PAGE_CODE"] + "' AND PAGE_NAME='" + dt.Rows[iCnt]["PAGE_NAME"] + "'");
                   // sbQuery.Append(" ");
                    int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                    if (iRes > 0)
                        bResult = true;
                }
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE GROUP_MASTER SET ASSET_TYPE='" + oPRP.AssetType + "' WHERE GROUP_CODE='" + oPRP.GroupCode + "'");
                int iRs = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRs > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Fetching Page master and group rights details to be displayed to the user.
        /// </summary>
        /// <returns></returns>
        public DataTable GetPageGroupDetails()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GR.PAGE_CODE,PM.PAGE_NAME,PM.PAGE_DESCRIPTION,GM.GROUP_NAME,GR.VIEW_RIGHTS,GR.SAVE_RIGHTS,GR.EDIT_RIGHTS,GR.DELETE_RIGHTS,GR.EXPORT_RIGHTS");
            sbQuery.Append(" FROM GROUP_RIGHTS GR INNER JOIN GROUP_MASTER GM ON GR.GROUP_CODE=GM.GROUP_CODE");
            sbQuery.Append(" INNER JOIN PAGE_MASTER PM ON GR.PAGE_CODE=PM.PAGE_CODE");
            sbQuery.Append(" ORDER BY GR.PAGE_CODE");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Save/Update Page Master details into PAGE_MASTER & GROUP_RIGHTS data tables.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveUpdatePageGroup(string OpType, GroupRights_PRP oPRP)
        {
            try
            {
                int PM_ID, iRes = 0;
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicatePageMaster(oPRP.PageName))
                    {
                        //Add New Page Master...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [PAGE_MASTER] ([PAGE_NAME],[PAGE_DESCRIPTION],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES ('" + oPRP.PageName + "','" + oPRP.PageDesc + "','" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                        iRes = oDb.ExecuteQuery(sbQuery.ToString());
                        if (iRes > 0)
                        {
                            //Getting last saved identity value.
                            sbQuery = new StringBuilder();
                            sbQuery.Append("SELECT IDENT_CURRENT('[PAGE_MASTER]') AS PM_ID");
                            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                            PM_ID = int.Parse(dt.Rows[0]["PM_ID"].ToString());

                            //Save Page Master details into Group Rights.
                            sbQuery = new StringBuilder();
                            sbQuery.Append("INSERT INTO [GROUP_RIGHTS] ([GROUP_CODE],[PAGE_CODE],[PAGE_NAME],[VIEW_RIGHTS],[SAVE_RIGHTS],[EDIT_RIGHTS]");
                            sbQuery.Append(" ,[DELETE_RIGHTS],[EXPORT_RIGHTS]) VALUES");
                            sbQuery.Append(" ('" + oPRP.GroupCode + "'," + PM_ID + ",'" + oPRP.PageName + "','" + oPRP.ViewRight + "',");
                            sbQuery.Append(" '" + oPRP.SaveRight + "','" + oPRP.EditRight + "','" + oPRP.DeleteRight + "','" + oPRP.ExportRight + "')");
                            iRes = oDb.ExecuteQuery(sbQuery.ToString());
                            if (iRes > 0)
                                bResult = true;
                        }
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update Page Master Information.
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [PAGE_MASTER] SET [PAGE_NAME]='" + oPRP.PageName + "',[PAGE_DESCRIPTION]='" + oPRP.PageDesc + "',");
                    sbQuery.Append(" MODIFIED_BY='" + oPRP.ModifiedBy + "',MODIFIED_ON = GETDATE() ");
                    sbQuery.Append(" WHERE PAGE_CODE=" + oPRP.PageCode + "");
                    iRes = oDb.ExecuteQuery(sbQuery.ToString());

                    //Update Page Master details into Group Rights.
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE GROUP_RIGHTS SET [GROUP_CODE]='" + oPRP.GroupCode + "',[PAGE_NAME]='" + oPRP.PageName + "'");
                    sbQuery.Append(" WHERE [PAGE_CODE]=" + oPRP.PageCode + "");
                    iRes = oDb.ExecuteQuery(sbQuery.ToString());

                    if (iRes > 0)
                        bResult = true;
                }
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Checking duplicate page name in the PAGE_MASTER data table.
        /// </summary>
        /// <param name="_PageName"></param>
        /// <returns></returns>
        private bool CheckDuplicatePageMaster(string _PageName)
        {
            bool bDup = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM PAGE_MASTER WHERE PAGE_NAME='" + _PageName + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                bDup = true;
            return bDup;
        }

        /// <summary>
        /// Delete Page details from PAGE_MASTER & GROUP_RIGHTS data tables.
        /// </summary>
        /// <param name="_PageCode"></param>
        /// <returns></returns>
        public bool DeletePageMaster(int _PageCode)
        {
            try
            {
                int iRes = 0;
                bool bResult = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM PAGE_MASTER WHERE PAGE_CODE=" + _PageCode + "");
                iRes = oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM GROUP_RIGHTS WHERE PAGE_CODE=" + _PageCode + "");
                iRes = oDb.ExecuteQuery(sbQuery.ToString());

                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}