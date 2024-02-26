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
    /// Summary description for CategoryMaster_DAL
    /// </summary>
    public class CategoryMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public CategoryMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~CategoryMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Check duplicate category code if exists.
        /// </summary>
        /// <param name="_CategoryCode"></param>
        /// <returns></returns>
        private bool CheckDuplicateCategory(string _CategoryCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM CATEGORY_MASTER WHERE CATEGORY_CODE = '" + _CategoryCode.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Check duplicate category initials if exists.
        /// </summary>
        /// <param name="CategoryInitials"></param>
        /// <returns></returns>
        public bool CheckCategoryInitials(string CategoryInitials)
        {
            bool bDup = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM CATEGORY_MASTER WHERE CATEGORY_INITIALS = '" + CategoryInitials.ToUpper() + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                bDup = true;
            return bDup;
        }

        /// <summary>
        /// Check duplicate category name within an asset type.
        /// </summary>
        /// <param name="_CategoryName"></param>
        /// <param name="_AssetType"></param>
        /// <returns></returns>
        private bool CheckDuplicateName(string _CategoryName, string _AssetType, string _CategoryCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM CATEGORY_MASTER WHERE CATEGORY_NAME = '" + _CategoryName.Trim().Replace("'", "''") + "'");
                sbQuery.Append(" AND ASSET_TYPE='" + _AssetType + "' AND CATEGORY_CODE != '" + _CategoryCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Save/update category master details.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveUpdateCategory(string OpType, CategoryMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                int iRes = 0;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateCategory(oPRP.CategoryCode))
                    {
                        if (!CheckDuplicateName(oPRP.CategoryName, oPRP.AssetType, ""))
                        {
                            //Add new Category...
                            sbQuery = new StringBuilder();
                            sbQuery.Append("INSERT INTO [CATEGORY_MASTER] ([CATEGORY_CODE],[CATEGORY_NAME],[CATEGORY_TYPE],[PARENT_CATEGORY],[ACTIVE],");
                            sbQuery.Append(" [REMARKS],[CREATED_BY],[CREATED_ON],[CATEGORY_LEVEL],[ASSET_TYPE],[COMP_CODE],[CATEGORY_INITIALS])");
                            sbQuery.Append(" VALUES('" + oPRP.CategoryCode + "','" + oPRP.CategoryName + "','" + oPRP.CategoryType + "','" + oPRP.ParentCategory + "','" + oPRP.Active + "',");
                            sbQuery.Append(" '" + oPRP.Remarks + "','" + oPRP.CreatedBy + "',GETDATE()," + oPRP.CategoryLevel + ",'" + oPRP.AssetType + "','" + oPRP.CompCode + "','" + oPRP.CategoryInitials + "')");
                            iRes = oDb.ExecuteQuery(sbQuery.ToString());
                        }
                    }
                }
                else if (OpType == "UPDATE")
                {
                    if (!CheckDuplicateName(oPRP.CategoryName, oPRP.AssetType,oPRP.CategoryCode))
                    {
                        //Update Category Information...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("UPDATE [CATEGORY_MASTER] SET [CATEGORY_NAME]='" + oPRP.CategoryName + "', [CATEGORY_TYPE] = '" + oPRP.CategoryType + "', [PARENT_CATEGORY] = '" + oPRP.ParentCategory + "'");
                        sbQuery.Append(" ,[ACTIVE] = '" + oPRP.Active + "', [REMARKS] = '" + oPRP.Remarks + "', [MODIFIED_BY] = '" + oPRP.ModifiedBy + "', [ASSET_TYPE] = '" + oPRP.AssetType + "'");
                        sbQuery.Append(" ,[MODIFIED_ON] = GETDATE(), [CATEGORY_LEVEL] = " + oPRP.CategoryLevel + " WHERE CATEGORY_CODE='" + oPRP.CategoryCode + "'");
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

        /// <summary>
        /// Fetch category code/name details to be populated into dropdownlist.
        /// </summary>
        /// <param name="_AssetType"></param>
        /// <param name="_ParentCategory"></param>
        /// <param name="_CatLevel"></param>
        /// <returns></returns>
        public DataTable GetCategoryCode(string _AssetType, string _ParentCategory, int _CatLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT CATEGORY_CODE,CATEGORY_NAME FROM CATEGORY_MASTER");
            sbQuery.Append(" WHERE ASSET_TYPE='" + _AssetType + "' AND PARENT_CATEGORY='" + _ParentCategory + "'");
            sbQuery.Append(" AND CATEGORY_LEVEL=" + _CatLevel + " AND ACTIVE=1 ORDER BY CATEGORY_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch category master details to be populated into gridview.
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategory()
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT CATEGORY_CODE,CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_LEVEL,ASSET_TYPE,CATEGORY_INITIALS,");
                sbQuery.Append(" REMARKS,ACTIVE,CREATED_BY,CONVERT(VARCHAR,CREATED_ON,105) AS CREATED_ON FROM CATEGORY_MASTER");
                sbQuery.Append(" ORDER BY CATEGORY_CODE,PARENT_CATEGORY");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_CatCode"></param>
        /// <returns></returns>
        public DataTable GetCategoryCodeLevel(string _CatCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT CATEGORY_CODE,CATEGORY_LEVEL FROM CATEGORY_MASTER");
            sbQuery.Append(" WHERE CATEGORY_CODE LIKE '" + _CatCode + "%'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Delete category if not having child/not in use.
        /// </summary>
        /// <param name="_CategoryCode"></param>
        /// <returns></returns>
        public string DeleteCategory(string _CategoryCode)
        {
            try
            {
                string DelRslt = "";
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS CNTCAT FROM dbo.CATEGORY_MASTER");
                sbQuery.Append(" WHERE PARENT_CATEGORY='" + _CategoryCode.Trim() + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                {
                    int iChild = 0;
                    int.TryParse(dt.Rows[0]["CNTCAT"].ToString(), out iChild);
                    if (iChild > 0)
                    {
                        DelRslt = "CHILD_FOUND";
                        return DelRslt;
                    }
                }
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS CATFOUND FROM ASSET_ACQUISITION WHERE CATEGORY_CODE='" + _CategoryCode + "'");
                DataTable dtRefChk = oDb.GetDataTable(sbQuery.ToString());
                if (dtRefChk.Rows[0]["CATFOUND"].ToString() != "0")
                {
                    DelRslt = "CATEGORY_IN_USE";
                    return DelRslt;
                }
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM CATEGORY_MASTER WHERE CATEGORY_CODE='" + _CategoryCode + "'");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    DelRslt = "SUCCESS";
                return DelRslt;
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
                if (ex.Message.Contains("conflicted with the REFERENCE constraint"))
                {
                    str = "You cannot delete this category, it is used somewhere.";
                }
                throw new Exception(str, ex);
            }
        }
    }
}