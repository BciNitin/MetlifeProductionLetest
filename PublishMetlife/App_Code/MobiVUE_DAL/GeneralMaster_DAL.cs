//************************************************************************************************************************************************
//  Data Access Layer Class For Add/Edit/Update/Delete General (City/State/Country) Master
//  Created By : Neeraj Saxena
//  Created On : April 28, 2012
//  Modified By : 
//  Modified On : 
//************************************************************************************************************************************************
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
    /// Summary description for GeneralMaster_DAL
    /// </summary>
    public class GeneralMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public GeneralMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~GeneralMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Checks Duplicate City/State/Country
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        private bool CheckDuplicateGeneralMaster(GeneralMaster_PRP oPRP)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM GENERAL_MASTER WHERE GENERAL_NAME='" + oPRP.GenaralName + "' AND STATE_NAME='" + oPRP.StateName + "'");
                sbQuery.Append(" AND COUNTRY_NAME='" + oPRP.CountryName + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Save/Update (City/State/Country) General Master
        /// </summary>
        /// <param name="OpType"></param>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveUpdateGeneralMaster(string OpType, GeneralMaster_PRP oPRP)
        {
            try
            {
                bool bResult = false;
                if (OpType == "SAVE")
                {
                    if (!CheckDuplicateGeneralMaster(oPRP))
                    {
                        //Add New General Master...
                        sbQuery = new StringBuilder();
                        sbQuery.Append("INSERT INTO [GENERAL_MASTER] ([GENERAL_NAME],[STATE_NAME],[COUNTRY_NAME],[REMARKS],[ACTIVE],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                        sbQuery.Append(" VALUES ");
                        sbQuery.Append("('" + oPRP.GenaralName + "','" + oPRP.StateName + "','" + oPRP.CountryName + "','" + oPRP.Remarks + "',");
                        sbQuery.Append("'" + oPRP.Active + "','" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                    }
                }
                if (OpType == "UPDATE")
                {
                    //Update General Master Information...
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE [GENERAL_MASTER] SET [GENERAL_NAME]='" + oPRP.GenaralName + "',[STATE_NAME]='" + oPRP.StateName + "',[COUNTRY_NAME]='" + oPRP.CountryName + "'");
                    sbQuery.Append(" ,[REMARKS]='" + oPRP.Remarks + "',[ACTIVE]='" + oPRP.Active + "'");
                    sbQuery.Append(" ,[MODIFIED_BY]='" + oPRP.ModifiedBy + "',[MODIFIED_ON]=GETDATE()");
                    sbQuery.Append(" WHERE GENERAL_CODE=" + oPRP.GeneralCode + "");
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
        /// Fetches General Master Records For GridView Population
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetGeneralMasterDetails()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT [GENERAL_CODE],[GENERAL_NAME],[STATE_NAME],[COUNTRY_NAME],[REMARKS]");
            sbQuery.Append(",[ACTIVE],[CREATED_BY],CONVERT(VARCHAR,[CREATED_ON],105) AS [CREATED_ON]");
            sbQuery.Append(" FROM [GENERAL_MASTER]");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Deletes A City/State/Country From General Master
        /// </summary>
        /// <param name="_GeneralCode"></param>
        /// <returns>bResult</returns>
        public bool DeleteGeneralMaster(int _GeneralCode)
        {
            try
            {
                bool bResult = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM GENERAL_MASTER WHERE GENERAL_CODE=" + _GeneralCode + "");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Fetches All Country Names
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetCountry()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GENERAL_CODE,GENERAL_NAME FROM GENERAL_MASTER WHERE STATE_NAME='' AND COUNTRY_NAME=''");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetches All State Names In A Country
        /// </summary>
        /// <param name="_CountryName"></param>
        /// <returns>DataTable</returns>
        public DataTable GetState(string _CountryName)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT GENERAL_CODE,GENERAL_NAME FROM GENERAL_MASTER WHERE COUNTRY_NAME='" + _CountryName + "' AND STATE_NAME=''");
            return oDb.GetDataTable(sbQuery.ToString());
        }
    }
}