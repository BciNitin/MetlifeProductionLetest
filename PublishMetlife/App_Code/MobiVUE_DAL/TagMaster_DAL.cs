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
    public class TagMaster_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public TagMaster_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~TagMaster_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }


        public void UploadTAgDetails(TagMaster_PRP oPRP)
        {
            try
            {
                sbQuery = new StringBuilder();
                if (!CheckDuplicateTag(oPRP.SerialNo.Trim()))
                {
                    sbQuery.Append("INSERT INTO [TagMaster] ([SerialNumber],[Active],[UploadedOn],[UploadedBy])");
                    //       sbQuery.Append(",[VENDOR_CITY],[VENDOR_PIN],[VENDOR_EMAIL],[ACTIVE],[REMARKS],[COMP_CODE],[CREATED_BY],[CREATED_ON])");
                    sbQuery.Append(" VALUES");
                    sbQuery.Append(" ('" + oPRP.SerialNo + "','" + oPRP.Active + "',GETDATE(),'" + oPRP.CreatedBy + "')");
                }
                else
                {
                    sbQuery.Append("Update [TagMaster] SET Active='"+ oPRP.Active + "', [UploadedOn] =GETDATE(), [UploadedBy] ='" + oPRP.CreatedBy + "' WHERE SerialNumber='"+ oPRP.SerialNo + "' ");
                }
            //    sbQuery.Append(" '" + oPRP.VendorCity + "','" + oPRP.VendorPIN + "','" + oPRP.VendorContPerson + "','" + oPRP.VendorPhone + "','" + oPRP.VendorEmail + "','" + oPRP.Active + "','" + oPRP.Remarks + "','" + oPRP.CompCode + "','" + oPRP.CreatedBy + "',GETDATE())");
                oDb.ExecuteQuery(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        private bool CheckDuplicateTag(string TagID)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM [TagMaster] WHERE SerialNumber='" + TagID + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataTable GetTags()
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT SerialNumber, ACTIVE,");
                sbQuery.Append("UploadedBy,CONVERT(VARCHAR,UploadedOn,105) AS UploadedOn FROM TagMaster");
                
                // sbQuery.Append("SELECT  * from mLocation");
                return oDb.GetDataTable(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// Upload AssetTagPerosnalization details from excel file.
        /// </summary>
        /// <param name="oPRP"></param>
        public void UploadAssetTagDetails(TagMaster_PRP oPRP)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO [AssetTagMapping] ([AssetCode],[TagSerialNo],[HostName],[AssetTag]");
                sbQuery.Append(",[CREATED_BY],[CREATED_ON])");
                sbQuery.Append(" VALUES");
                sbQuery.Append(" ('" + oPRP.AssetCode + "','" + oPRP.TagSerialNo + "','" + oPRP.HostName + "','" + oPRP.AssetTag + "',");
                sbQuery.Append(" '" + oPRP.CreatedBy + "',GETDATE())");
                oDb.ExecuteQuery(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }


        /// <summary>
        /// Upload Employeetag Persosnalization from excel file.
        /// </summary>
        /// <param name="oPRP"></param>
        public void UploadEmployeeTagDetails(TagMaster_PRP oPRP)
        {
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO [EmployeeTagMapping] ([EmpCode],[TagSerialNo],[EmpName],[EmailID],[Designation]");
                sbQuery.Append(",[SeatNo],[ProcessName],[LOB],[SubLOB],[CREATED_BY],[CREATED_ON])");
                sbQuery.Append(" VALUES");
                sbQuery.Append(" ('" + oPRP.EmpCode + "','" + oPRP.TagSerialNo + "','" + oPRP.EmpName + "','" + oPRP.EmailID + "','" + oPRP.Designation + "',");
                sbQuery.Append(" '" + oPRP.SeatNo + "','" + oPRP.ProcessName + "','" + oPRP.LOB + "','" + oPRP.SubLOB + "','" + oPRP.CreatedBy + "',GETDATE())");
                oDb.ExecuteQuery(sbQuery.ToString());
            }
            catch (Exception ex)
            { throw ex; }
        }

    }
}