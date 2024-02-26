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
    /// Summary description for LocationMaster_DAL
    /// </summary>
    public class LocationMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public LocationMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~LocationMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Save/Update location master details.
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveUpdateLocation(string OpType, LocationMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateLocation(oPRP.LocName.Trim(), oPRP.CompCode.Trim()))
                    {
                        //Add new Location...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO dbo.LOCATION_MASTER ([LOC_CODE],[LOC_NAME],[DESCRIPTION],[COMP_CODE],[LOC_LEVEL],[ACTIVE],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES ");
                        sbQuery.Append("('" + oPRP.LocCode.Trim() + "','" + oPRP.LocName.Trim() + "','" + oPRP.LocDesc.Trim() + "','" + oPRP.CompCode+"',1,");
                        sbQuery.Append(" '" + oPRP.Active + "', '" + oPRP.CreatedBy.Trim() + "', GETDATE())");
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update Location Details...

                  

                    
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE dbo.LOCATION_MASTER SET [LOC_NAME] = '" + oPRP.LocName.Trim() + "',");
                    sbQuery.Append(" [COMP_CODE] = '" + oPRP.CompCode.Trim() + "',[DESCRIPTION] = '" + oPRP.LocDesc.Trim() + "',");
                    sbQuery.Append(" [MODIFIED_BY] = '" + oPRP.ModifiedBy.Trim() + "',[MODIFIED_ON]=GETDATE(),[ACTIVE]='" + oPRP.Active + "'");
                    sbQuery.Append(" WHERE [LOC_CODE] = '" + oPRP.LocCode.Trim() + "' and [COMP_CODE]='"+oPRP.CompCode.Trim()+"' ");
                }
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Check duplicate location code.
        /// </summary>
        /// <param name="_LocCode"></param>
        /// <returns></returns>
        private bool CheckDuplicateLocation(string _LocCode,string _CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM dbo.LOCATION_MASTER WHERE LOC_CODE='" + _LocCode + "' and COMP_CODE='"+ _CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Get location code and level from selected location name.
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns></returns>
        public DataTable GetLocationCode(string _CompCode, string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT LOC_CODE, LOC_NAME FROM dbo.LOCATION_MASTER");
            sbQuery.Append(" WHERE COMP_CODE='" + _CompCode + "' AND PARENT_LOC_CODE='" + _ParentLocCode + "' AND LOC_LEVEL=" + _LocLevel + "");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get all existing locations to be populated in the grid.
        /// </summary>
        /// <returns></returns>
        public DataTable GetLocation(string _CompCode)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT LM.LOC_CODE,LM.LOC_NAME,DESCRIPTION,LM.ACTIVE,");
                sbQuery.Append("LM.CREATED_BY,CONVERT(VARCHAR,LM.CREATED_ON,105) AS CREATED_ON FROM LOCATION_MASTER LM");
               // sbQuery.Append(" INNER JOIN COMPANY_MASTER CM ON LM.COMP_CODE = CM.COMP_CODE");
                sbQuery.Append(" WHERE LM.COMP_CODE='" + _CompCode + "'");
               // sbQuery.Append("SELECT  * from mLocation");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Fetches Company Details For DropDownList Population
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetCompany()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COMP_CODE,COMP_NAME FROM COMPANY_MASTER WHERE ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetches Company details for mapping with user id
        /// </summary>
        /// <returns></returns>
        //public string GetCompany()
        //{
        //    string _CompCode = "";
        //    sbQuery = new StringBuilder();
        //    sbQuery.Append("SELECT COMP_CODE,COMP_NAME FROM COMPANY_MASTER");
        //    DataTable dt = oDb.GetDataTable(sbQuery.ToString());
        //    if (dt.Rows.Count > 0)
        //    {
        //        _CompCode = dt.Rows[0]["COMP_CODE"].ToString();
        //    }
        //    return _CompCode;
        //}

        /// <summary>
        /// Delete a particular location from data table.
        /// </summary>
        /// <param name="_LocCode"></param>
        /// <returns></returns>
        public string DeleteLocation(string _LocCode, string _CompCode)
        {
            try
            {
                string DelRslt = "";
                //sbQuery = new StringBuilder();
                //sbQuery.Append("SELECT COUNT(*) AS CHLOC FROM LOCATION_MASTER");
                //sbQuery.Append(" WHERE PARENT_LOC_CODE='" + _LocCode.Trim() + "'");
                //DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                //if (dt.Rows.Count > 0)
                //{
                //    int iChild = 0;
                //    int.TryParse(dt.Rows[0]["CHLOC"].ToString(), out iChild);
                //    if (iChild > 0)
                //    {
                //        DelRslt = "CHILD_FOUND";
                //        return DelRslt;
                //    }
                //}
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS LOCFOUND FROM ASSET_ACQUISITION WHERE ASSET_LOCATION='" + _LocCode + "'");
                DataTable dtRefChk = oDb.GetDataTable(sbQuery.ToString());
                if (dtRefChk.Rows[0]["LOCFOUND"].ToString() != "0")
                {
                    DelRslt = "LOCATION_IN_USE";
                    return DelRslt;
                }
                else
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("DELETE FROM LOCATION_MASTER WHERE  [LOC_CODE] = '" + _LocCode.Trim() + "' and [COMP_CODE]='" + _CompCode.Trim() + "'");
                    int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                    if (iRes > 0)
                        DelRslt = "SUCCESS";
                }
                return DelRslt;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Get Location level for parent child relationship (maximum 6 levels).
        /// </summary>
        /// <param name="_LocCode"></param>
        /// <returns></returns>
        public DataTable GetLocationCodeLevel(string _LocCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT LOC_CODE,LOC_LEVEL FROM LOCATION_MASTER");
            sbQuery.Append(" WHERE LOC_CODE LIKE '" + _LocCode + "%'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}