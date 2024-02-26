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
    /// Summary description for AssetTransfer_DAL
    /// </summary>
    public class AssetTransfer_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public AssetTransfer_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~AssetTransfer_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        /// <summary>
        /// Fetch Locations details for mapping with user id
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns>DataTable</returns>
        public DataTable GetFromLocation(string _CompCode, string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT LOC_CODE, LOC_NAME FROM LOCATION_MASTER");
            sbQuery.AppendLine(" WHERE PARENT_LOC_CODE LIKE '" + _ParentLocCode + "%' AND LOC_LEVEL=" + _LocLevel + "");
            sbQuery.AppendLine(" AND COMP_CODE='" + _CompCode + "' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetMailTransactionDetails(string TransactionType, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM MAIL_MASTER WHERE TRANSACTION_TYPE='" + TransactionType + "' AND COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());

        }
        /// <summary>
        /// Fetch Locations details for mapping with user id
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns>DataTable</returns>
        public DataTable GetToLocation(string _CompCode, string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT LOC_CODE, (LOC_CODE + ';' + COMP_CODE) AS LOCATION_CODE, LOC_NAME FROM LOCATION_MASTER");
            sbQuery.AppendLine(" WHERE PARENT_LOC_CODE LIKE '" + _ParentLocCode + "%' AND LOC_LEVEL=" + _LocLevel + "");
            sbQuery.AppendLine(" AND COMP_CODE != '" + _CompCode + "' AND ACTIVE = '1'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch all department code/name from department master.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetProcess(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "' ORDER BY PROCESS_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch Locations details for mapping with user id
        /// </summary>
        /// <param name="_ParentLocCode"></param>
        /// <param name="_LocLevel"></param>
        /// <returns>DataTable</returns>
        public DataTable GetLocation(string _CompCode, string _ParentLocCode, int _LocLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT LOC_CODE, LOC_NAME FROM LOCATION_MASTER");
            sbQuery.AppendLine(" WHERE PARENT_LOC_CODE LIKE '" + _ParentLocCode + "%' AND LOC_LEVEL=" + _LocLevel + "");
            sbQuery.AppendLine(" AND COMP_CODE='" + _CompCode + "' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch category code/name details to be populated into dropdownlist.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateCategory(string _AssetType)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT CATEGORY_CODE,CATEGORY_NAME FROM CATEGORY_MASTER");
            sbQuery.AppendLine(" WHERE ASSET_TYPE='" + _AssetType + "' AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get asset details for asset transfer based on filters provided.
        /// </summary>
        /// <param name="_AssetCode"></param>
        /// <param name="_CompCode"></param>
        /// <returns>DataTable</returns>
        public DataTable GetAssetDetails(string _AssetCode,string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT ACQ.SERIAL_CODE,ACQ.ASSET_MAKE,ACQ.MODEL_NAME,ACQ.ASSET_PROCESS,ACQ.ASSET_LOCATION,LM.LOC_NAME,ACQ.WORKSTATION_NO,ACQ.PORT_NO"); 
            sbQuery.AppendLine(" FROM ASSET_ACQUISITION ACQ INNER JOIN LOCATION_MASTER LM ON ACQ.ASSET_LOCATION = LM.LOC_CODE");
            sbQuery.AppendLine(" WHERE ACQ.ASSET_CODE='" + _AssetCode + "' AND ACQ.COMP_CODE='" + _CompCode + "' AND ACQ.ASSET_APPROVED = 'True' AND ACQ.SOLD_SCRAPPED_STATUS IS NULL");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssetCode"></param>
        /// <returns></returns>
        public bool ChkAssetTransferred(string AssetCode, string CompCode)
        {
            bool bResult = false;
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT COUNT(*) AS TRANS FROM [ASSET_TRANSFER] AT WHERE AT.[ASSET_CODE]='" + AssetCode + "'");
            sbQuery.AppendLine(" AND AT.[COMP_CODE]='" + CompCode + "' AND AT.[TO_LOCATION] IN ");
            sbQuery.AppendLine("(SELECT [LOC_CODE] FROM [LOCATION_MASTER] WHERE [COMP_CODE] <> '" + CompCode + "')");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["TRANS"].ToString() != "0")
                    bResult = true;
            }
            return bResult;
        }

        /// <summary>
        /// Save asset transfer details and update asset location in asset acquisition.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public bool SaveAssetTransferDetails(AssetTransfer_PRP oPRP)
        {
            int iRes, iRs = 0;
            bool bResult = false;
            if (oPRP.TransferType == "INTER")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("INSERT INTO [ASSET_TRANSFER] ([ASSET_CODE],[SERIAL_CODE],[MODEL_NAME],[ASSET_PROCESS]");
                sbQuery.AppendLine(",[IUT_STATUS],[REMARKS],[CREATED_BY],[CREATED_ON],[TRANSFER_DATE]");
                sbQuery.AppendLine(",[FROM_WORKSTATION],[TO_WORKSTATION],[FROM_PORT],[TO_PORT],[INTER_FROM_LOCATION],[INTER_TO_LOCATION]");
                sbQuery.AppendLine(",[INTER_TO_PROCESS],[COMP_CODE],[TRANSFER_TYPE])");
                sbQuery.AppendLine(" VALUES ");
                sbQuery.AppendLine("('" + oPRP.AssetCode + "','" + oPRP.SerialNo + "','" + oPRP.AssetModel + "','" + oPRP.AssetProcess + "'");
                sbQuery.AppendLine(",'" + oPRP.IUTStatus + "','" + oPRP.TransferRemarks + "','" + oPRP.CreatedBy + "',GETDATE(),'" + oPRP.TransferDate + "'");
                sbQuery.AppendLine(",'" + oPRP.FromWorkStation + "','" + oPRP.ToWorkStation + "','" + oPRP.FromPort + "','" + oPRP.ToPort + "','" + oPRP.FromInterLocationCode + "','" + oPRP.ToInterLocationCode + "'");
                sbQuery.AppendLine(",'" + oPRP.ToInterProcessCode + "','" + oPRP.CompCode + "','" + oPRP.TransferType + "')");
                iRes = oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("UPDATE ASSET_ACQUISITION SET WORKSTATION_NO='" + oPRP.ToWorkStation + "',PORT_NO='" + oPRP.ToPort + "',");
                sbQuery.AppendLine("ASSET_LOCATION='" + oPRP.ToInterLocationCode + "',ASSET_PROCESS='" + oPRP.ToInterProcessCode + "',");
                sbQuery.AppendLine("ASSET_APPROVED='0',REMARKS='" + oPRP.TransferRemarks + "' WHERE ASSET_CODE='" + oPRP.AssetCode + "' AND SERIAL_CODE='" + oPRP.SerialNo + "'");
                iRs = oDb.ExecuteQuery(sbQuery.ToString());

                if (iRes > 0 && iRs > 0)
                    bResult = true;
            }
            if (oPRP.TransferType == "COUNTRY")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine("INSERT INTO [ASSET_TRANSFER] ([ASSET_CODE],[SERIAL_CODE],[MODEL_NAME],[ASSET_PROCESS],[FROM_LOCATION]");
                sbQuery.AppendLine(",[TO_LOCATION],[IUT_STATUS],[REMARKS],[CREATED_BY],[CREATED_ON]");
                sbQuery.AppendLine(",[TRANSFER_DATE],[COMP_CODE],[TRANSFER_TYPE])");
                sbQuery.AppendLine(" VALUES ");
                sbQuery.AppendLine("('" + oPRP.AssetCode + "','" + oPRP.SerialNo + "','" + oPRP.AssetModel + "','" + oPRP.AssetProcess + "','" + oPRP.FromLocationCode + "'");
                sbQuery.AppendLine(",'" + oPRP.ToLocationCode + "','" + oPRP.IUTStatus + "','" + oPRP.TransferRemarks + "','" + oPRP.CreatedBy + "',GETDATE()");
                sbQuery.AppendLine(",'" + oPRP.TransferDate + "','" + oPRP.CompCode + "','" + oPRP.TransferType + "')");
                iRes = oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("UPDATE ASSET_ACQUISITION SET ASSET_LOCATION='" + oPRP.ToLocationCode + "',REMARKS='" + oPRP.TransferRemarks + "',ASSET_APPROVED='0',");
                sbQuery.AppendLine("COMP_CODE='" + oPRP.CompCode + "' WHERE ASSET_CODE='" + oPRP.AssetCode + "' AND SERIAL_CODE='" + oPRP.SerialNo + "'");
                iRs = oDb.ExecuteQuery(sbQuery.ToString());

                if (iRes > 0)
                    bResult = true;
            }
            return bResult;
        }

        /// <summary>
        /// Get transferred assets details for being viewed.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssetTransferDetails(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT AT.ASSET_CODE,AT.SERIAL_CODE,AT.MODEL_NAME,AT.ASSET_PROCESS,");
            sbQuery.AppendLine(" LM.LOC_NAME AS FROM_LOCATION,LM1.LOC_NAME AS TO_LOCATION,AT.FROM_WORKSTATION,AT.TO_WORKSTATION,");
            sbQuery.AppendLine(" AT.FROM_PORT,AT.TO_PORT,LM2.LOC_NAME AS INTER_FROM_LOCATION,LM3.LOC_NAME AS INTER_TO_LOCATION,AT.INTER_TO_PROCESS,AT.REMARKS,AT.TRANSFER_TYPE,");
            sbQuery.AppendLine(" AT.TRANSFER_DATE FROM ASSET_TRANSFER AT LEFT JOIN LOCATION_MASTER LM");
            sbQuery.AppendLine(" ON AT.FROM_LOCATION = LM.LOC_CODE LEFT JOIN LOCATION_MASTER LM1 ON AT.TO_LOCATION = LM1.LOC_CODE");
            sbQuery.AppendLine(" LEFT JOIN LOCATION_MASTER LM2 ON AT.INTER_FROM_LOCATION = LM2.LOC_CODE");
            sbQuery.AppendLine(" LEFT JOIN LOCATION_MASTER LM3 ON AT.INTER_TO_LOCATION = LM3.LOC_CODE");
            sbQuery.AppendLine(" INNER JOIN ASSET_ACQUISITION AA ON AT.ASSET_CODE = AA.ASSET_CODE");
            sbQuery.AppendLine(" WHERE AA.ASSET_APPROVED='1'"); //AT.[COMP_CODE]='" + CompCode + "' AND
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateAssetMake(string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT DISTINCT [ASSET_MAKE] FROM [ASSET_ACQUISITION] WHERE [CATEGORY_CODE] = '" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Fetch category code/name details to be populated into dropdownlist.
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateCategory(string _AssetType, string _ParentCategory, int _CatLevel)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT CATEGORY_CODE,CATEGORY_NAME FROM CATEGORY_MASTER");
            sbQuery.AppendLine(" WHERE ASSET_TYPE='" + _AssetType + "' AND PARENT_CATEGORY='" + _ParentCategory + "'");
            sbQuery.AppendLine(" AND CATEGORY_LEVEL=" + _CatLevel + " AND ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompCode"></param>
        /// <returns></returns>
        public DataTable PopulateModelName(string AssetMake, string CategoryCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT DISTINCT [MODEL_NAME] FROM [ASSET_ACQUISITION] WHERE [ASSET_MAKE]='" + AssetMake + "'");
            sbQuery.AppendLine(" AND [CATEGORY_CODE]='" + CategoryCode + "' AND [COMP_CODE]='" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateProcess(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER WHERE ACTIVE='1' AND COMP_CODE='" + CompCode + "' ORDER BY PROCESS_NAME");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get list of country-wide transferred assets for being exported.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetTransferredAssets(AssetTransfer_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT AA.[ASSET_CODE],AA.[ASSET_ID],AA.[SERIAL_CODE],AA.[CATEGORY_CODE],AA.[ASSET_LOCATION],AA.[AMC_WARRANTY],NULLIF(AA.[AMC_WARRANTY_START_DATE],'') [AMC_WARRANTY_START_DATE],");
            sbQuery.AppendLine("NULLIF(AA.[AMC_WARRANTY_END_DATE],'') [AMC_WARRANTY_END_DATE],AA.[VENDOR_CODE],NULLIF(AA.[INSTALLED_DATE],'') [INSTALLED_DATE],NULLIF(AA.[PURCHASED_DATE],'') [PURCHASED_DATE],AA.[PURCHASED_AMT],AA.[PO_NUMBER],");
            sbQuery.AppendLine("NULLIF(AA.[PO_DATE],'') [PO_DATE],AA.[INVOICE_NO],NULLIF(AA.[SALE_DATE],'') [SALE_DATE],AA.[SALE_AMT],AA.[ASSET_MAKE],AA.[MODEL_NAME],AA.[ASSET_PROCESS],AA.[SECURITY_CLASSIFICATION],");
            sbQuery.AppendLine("AA.[ASSET_SIZE],AA.[ASSET_VLAN],AA.[ASSET_HDD],AA.[ASSET_PROCESSOR],AA.[ASSET_RAM],AA.[ASSET_IMEI_NO],AA.[ASSET_PHONE_MEMORY],");
            sbQuery.AppendLine("AA.[ASSET_SERVICE_PROVIDER],AA.[ASSET_TYPE],AA.[ASSET_BOE],AA.[ASSET_REGISTER_NO],AA.[BONDED_TYPE],AA.[CAPITALISED_STATUS],");
            sbQuery.AppendLine("AA.[VERIFIABLE_STATUS],AA.[PORT_NO],AA.[WORKSTATION_NO],AA.[COST_CENTER_NO],AA.[SECURITY_GATE_ENTRY_NO],");
            sbQuery.AppendLine("NULLIF(AA.[SECURITY_GATE_ENTRY_DATE],'') [SECURITY_GATE_ENTRY_DATE],AA.[COMP_CODE],AA.[COMPANY_NAME],AA.[CUSTOMER_ORDER_NO],AA.[DEPARTMENT]");
            sbQuery.AppendLine(" FROM [ASSET_ACQUISITION] AA INNER JOIN [ASSET_TRANSFER] AT ON AA.[ASSET_CODE] = AT.[ASSET_CODE]");
            sbQuery.AppendLine(" WHERE AT.[TRANSFER_TYPE] = 'COUNTRY' AND AT.[TRANSFER_DATE] >= CONVERT(DATETIME,'" + oPRP.FromDate + "',105)");
            sbQuery.AppendLine(" AND AT.[TRANSFER_DATE] <= CONVERT(DATETIME,'" + oPRP.ToDate + "',105)");
            sbQuery.AppendLine(" AND AA.[ASSET_LOCATION] LIKE '" + oPRP.TransferLocation + "%' AND AA.[COMP_CODE]='" + oPRP.CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Get assets for being transferred.
        /// </summary>
        /// <param name="oPRP"></param>
        /// <returns></returns>
        public DataTable GetAssetsForTransfer(AssetTransfer_PRP oPRP)
        {
            sbQuery = new StringBuilder();
            sbQuery.AppendLine("SELECT AA.ASSET_CODE,AA.SERIAL_CODE,AA.ASSET_MAKE,AA.MODEL_NAME,AA.ASSET_PROCESS,AA.ASSET_LOCATION,");
            sbQuery.AppendLine(" AA.PORT_NO,AA.WORKSTATION_NO,CM.CATEGORY_NAME FROM ASSET_ACQUISITION AA INNER JOIN CATEGORY_MASTER CM");
            sbQuery.AppendLine(" ON AA.CATEGORY_CODE = CM.CATEGORY_CODE");
            sbQuery.AppendLine(" WHERE AA.ASSET_TYPE LIKE '" + oPRP.AssetType + "%' AND AA.ASSET_CODE LIKE '" + oPRP.AssetCode + "%'");
            sbQuery.AppendLine(" AND AA.SERIAL_CODE LIKE '" + oPRP.AssetSerialCode + "%' AND AA.ASSET_MAKE LIKE '" + oPRP.AssetMakeName + "%'");
            if (oPRP.AssetModelName != "")
                sbQuery.AppendLine(" AND AA.[MODEL_NAME] IN (" + oPRP.AssetModelName + ")");
            else
                sbQuery.AppendLine(" AND AA.[MODEL_NAME] LIKE '" + oPRP.AssetModelName + "%'");
            sbQuery.AppendLine(" AND AA.CATEGORY_CODE LIKE '" + oPRP.AssetCategoryCode + "%' AND AA.ASSET_LOCATION LIKE '" + oPRP.CompCode + "%'");
            sbQuery.AppendLine(" AND AA.ASSET_PROCESS LIKE '" + oPRP.AssetProcess + "%' AND AA.PORT_NO LIKE '" + oPRP.PortNo + "%'");
            sbQuery.AppendLine(" AND AA.ASSET_APPROVED = 'True' AND AA.COMP_CODE='" + oPRP.CompCode + "' AND AA.SOLD_SCRAPPED_STATUS IS NULL ORDER BY AA.ASSET_CODE,AA.PORT_NO");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        /// <summary>
        /// Delete/revert inter-location/country-wide asset transfer details.
        /// </summary>
        /// <param name="AssetCode"></param>
        /// <param name="CompCode"></param>
        public void DeleteTransferDetails(string AssetCode, string CompCode, string TransferType)
        {
            if (TransferType == "COUNTRY")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine(" UPDATE [ASSET_ACQUISITION] SET [ASSET_LOCATION] = AT.[FROM_LOCATION]");
                sbQuery.AppendLine(" FROM [ASSET_ACQUISITION] AA INNER JOIN [ASSET_TRANSFER] AT");
                sbQuery.AppendLine(" ON AA.[ASSET_CODE] = AT.[ASSET_CODE]");
                sbQuery.AppendLine(" WHERE AT.[ASSET_CODE] = '" + AssetCode + "' AND AT.[COMP_CODE] = '" + CompCode + "'");
                sbQuery.AppendLine(" AND AT.[TRANSFER_TYPE] = 'COUNTRY' AND AA.[COMP_CODE] = AT.[COMP_CODE]");
                oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [ASSET_TRANSFER] WHERE [ASSET_CODE]='" + AssetCode + "' AND [COMP_CODE]='" + CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());
            }
            if (TransferType == "INTER")
            {
                sbQuery = new StringBuilder();
                sbQuery.AppendLine(" UPDATE [ASSET_ACQUISITION] SET [WORKSTATION_NO] = AT.[FROM_WORKSTATION],[PORT_NO] = AT.[FROM_PORT],[ASSET_LOCATION] = AT.[INTER_FROM_LOCATION],");
                sbQuery.AppendLine(" [ASSET_PROCESS] = AT.[ASSET_PROCESS] FROM [ASSET_ACQUISITION] AA INNER JOIN [ASSET_TRANSFER] AT");
                sbQuery.AppendLine(" ON AA.[ASSET_CODE] = AT.[ASSET_CODE]");
                sbQuery.AppendLine(" WHERE AT.[ASSET_CODE] = '" + AssetCode + "' AND AT.[COMP_CODE] = '" + CompCode + "'");
                sbQuery.AppendLine(" AND AT.[TRANSFER_TYPE] = 'INTER' AND AA.[COMP_CODE] = AT.[COMP_CODE]");
                oDb.ExecuteQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.AppendLine("DELETE FROM [ASSET_TRANSFER] WHERE [ASSET_CODE]='" + AssetCode + "' AND [COMP_CODE]='" + CompCode + "'");
                oDb.ExecuteQuery(sbQuery.ToString());
            }
        }
    }
}