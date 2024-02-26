using MobiVUE_ATS.PRP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace MobiVUE_ATS.DAL
{
    public class AssetAcq_DAL
    {
        clsDb oDb;
        StringBuilder sbQuery;
        public AssetAcq_DAL(string DatabaseType)
        {
            oDb = new clsDb();
            if (DatabaseType != "")
                oDb.Connect(DatabaseType);
        }
        ~AssetAcq_DAL()
        {
            oDb.Disconnect();
            oDb = null;
            sbQuery = null;
        }

        public List<BarGraph_PRP> GetBarChartValues(string CompCode, string QueryType)
        {
            List<BarGraph_PRP> data = new List<BarGraph_PRP>();
            DataTable dt = GetAssetLocationWithcount(CompCode, QueryType, string.Empty, string.Empty);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = (from p in dt.Rows.Cast<DataRow>()
                        group p by new
                        {
                            Count = Convert.ToInt32(p["AssetCount"]),
                            Location = Convert.ToString(p["ASSET_LOCATION"])
                        } into g
                        select new BarGraph_PRP
                        {
                            Count = g.Key.Count,
                            Location = g.Key.Location
                        }).ToList();

            }
            return data;
        }
        public List<BarGraphLocationWithStatus_PRP> GetBarChartValuesLocationWithstatus(string CompCode, string QueryType)
        {
            List<BarGraphLocationWithStatus_PRP> data = new List<BarGraphLocationWithStatus_PRP>();
            DataTable dt = GetAssetLocationWithcount(CompCode, QueryType, string.Empty, string.Empty);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = (from p in dt.Rows.Cast<DataRow>()
                        group p by new
                        {
                            Location = Convert.ToString(p["ASSET_LOCATION"]),
                            Stock = Convert.ToString(p["STOCK"]),
                            Allocated = Convert.ToString(p["ALLOCATED"]),
                            InTransit = Convert.ToString(p["IN TRANSIT"]),
                            LostInTransit = Convert.ToString(p["LOST IN TRANSIT"]),
                            Scrap = Convert.ToString(p["SCRAP"]),
                        } into g
                        select new BarGraphLocationWithStatus_PRP
                        {
                            Location = g.Key.Location,
                            Stock = g.Key.Stock,
                            Allocated = g.Key.Allocated,
                            InTransit = g.Key.InTransit,
                            LostInTransit = g.Key.LostInTransit,
                            Scrap = g.Key.Scrap,
                        }).ToList();

            }
            return data;
        }
        public List<BarGraphStock_PRP> GetBarChartValuesWithStock(string CompCode, string QueryType, string Location)
        {
            List<BarGraphStock_PRP> data = new List<BarGraphStock_PRP>();
            DataTable dt = GetAssetLocationWithcount(CompCode, QueryType, Location, string.Empty);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = (from p in dt.Rows.Cast<DataRow>()
                        group p by new
                        {
                            Count = Convert.ToInt32(p["AssetCount"]),
                            Location = Convert.ToString(p["ASSET_LOCATION"]),
                            Store = Convert.ToString(p["STORE"])
                        } into g
                        select new BarGraphStock_PRP
                        {
                            Count = g.Key.Count,
                            Location = g.Key.Location,
                            Store = g.Key.Store
                        }).ToList();

            }
            return data;
        }
        public List<BarGraphAllocated_PRP> GetBarChartValuesWithAllocated(string CompCode, string QueryType)
        {
            List<BarGraphAllocated_PRP> data = new List<BarGraphAllocated_PRP>();
            DataTable dt = GetAssetLocationWithcount(CompCode, QueryType, string.Empty, string.Empty);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = (from p in dt.Rows.Cast<DataRow>()
                        group p by new
                        {
                            Location = Convert.ToString(p["ASSET_LOCATION"]),
                            Employee = Convert.ToString(p["EMPLOYEE"]),
                            Floor = Convert.ToString(p["FLOOR"])

                        } into g
                        select new BarGraphAllocated_PRP
                        {
                            Location = g.Key.Location,
                            Employee = g.Key.Employee,
                            Floor = g.Key.Floor
                        }).ToList();

            }
            return data;
        }
        public List<BarGraphInTransit_PRP> GetBarChartValuesWithInTransit(string CompCode, string QueryType)
        {
            List<BarGraphInTransit_PRP> data = new List<BarGraphInTransit_PRP>();
            DataTable dt = GetAssetLocationWithcount(CompCode, QueryType, string.Empty, string.Empty);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = (from p in dt.Rows.Cast<DataRow>()
                        group p by new
                        {
                            Count = Convert.ToInt32(p["AssetCount"]),
                            Location = Convert.ToString(p["ASSET_LOCATION"])
                        } into g
                        select new BarGraphInTransit_PRP
                        {
                            Count = g.Key.Count,
                            Location = g.Key.Location
                        }).ToList();

            }
            return data;
        }
        public List<PieGraph_PRP> GetPieChartValues(string CompCode, string QueryType, string LocationCode)
        {
            List<PieGraph_PRP> data = new List<PieGraph_PRP>();
            DataTable dt = GetAssetLocationWithcount(CompCode, QueryType, LocationCode, string.Empty);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = (from p in dt.Rows.Cast<DataRow>()
                        group p by new
                        {
                            Count = Convert.ToInt32(p["AssetCount"]),
                            Location = Convert.ToString(p["ASSET_LOCATION"]),
                            Floor = Convert.ToString(p["FLOOR"])
                        } into g
                        select new PieGraph_PRP
                        {
                            Count = g.Key.Count,
                            Location = g.Key.Location,
                            Floor = g.Key.Floor
                        }).ToList();

            }
            return data;
        }
        public List<DonutGraph_PRP> GetDonutChartValues(string CompCode, string QueryType, string LocationCode, string FloorCode)
        {
            List<DonutGraph_PRP> data = new List<DonutGraph_PRP>();
            DataTable dt = GetAssetLocationWithcount(CompCode, QueryType, LocationCode, FloorCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = (from p in dt.Rows.Cast<DataRow>()
                        group p by new
                        {
                            Count = Convert.ToInt32(p["AssetCount"]),
                            Location = Convert.ToString(p["ASSET_LOCATION"]),
                            Floor = Convert.ToString(p["FLOOR"]),
                            Store = Convert.ToString(p["STORE"])
                        } into g
                        select new DonutGraph_PRP
                        {
                            Count = g.Key.Count,
                            Location = g.Key.Location,
                            Floor = g.Key.Floor,
                            Store = g.Key.Store
                        }).ToList();

            }
            return data;
        }
        public DataTable GetAssetLocationWithcount(string Compcode, string QueryType, string Location, string Floor)
        {
            return oDb.ExecuteSPWithOutput("sp_GetAssetLocationWithCount", new System.Data.SqlClient.SqlParameter("Compcode", Compcode),
                new System.Data.SqlClient.SqlParameter("QueryType", QueryType),
                new System.Data.SqlClient.SqlParameter("Location", Location),
                new System.Data.SqlClient.SqlParameter("Floor", Floor));
        }
        public DataTable GetBarChartDBData()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT [Id], [Male], [Female], [Others], [Country], [MonthAndYear] from Population ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
              
        public bool CheckDuplicate(string _AssetCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE ASSET_CODE = '" + _AssetCode.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool CheckDuplicateAssetDomain(string _AssetDomain)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_DOMAIN_MASTER WHERE ASSET_DOMAIN_CODE = '" + _AssetDomain.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool CheckDuplicateAssetSubStatus(string _AssetSubStatus)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM SUB_STATUS_MASTER WHERE SUB_STATUS = '" + _AssetSubStatus.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool CheckSerialDuplicate(string _AssetSerial)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _AssetSerial.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool ChkActiveSerialNoExists(string ActiveSerialNo, string CompCode)
        {
            try
            {
                bool bExists = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COUNT(*) AS CSN FROM [ASSET_ACQUISITION]");
                sbQuery.Append(" WHERE SERIAL_CODE='" + ActiveSerialNo + "' AND COMP_CODE='" + CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows[0]["CSN"].ToString() != "0")
                    bExists = true;
                return bExists;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool ChkActiveSerialNoisScrappedinIT(string ActiveSerialNo, string CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + ActiveSerialNo.Trim().Replace("'", "''") + "' AND STATUS='SCRAP' AND COMP_CODE='" + CompCode + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool ChkActiveAssetFarTagisScrappedinFacilities(string AssetFarTag, string CompCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + AssetFarTag.Trim().Replace("'", "''") + "' AND STATUS='SCRAP' AND COMP_CODE='" + CompCode + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool CheckRFIDDuplicate(string RFIDTag)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE TAG_ID = '" + RFIDTag.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool CheckRFIDDuplicatetoVerifybulkUpdate(string RFIDTag,string AssetCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE TAG_ID = '" + RFIDTag.Trim().Replace("'", "''") + "' AND ASSET_CODE = '"+AssetCode+"' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool CheckAssetTagDuplicatetoVerifybulkUpdate(string RFIDTag, string AssetCode)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE TAG_ID = '" + RFIDTag.Trim().Replace("'", "''") + "' AND ASSET_CODE = '" + AssetCode + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public bool CheckAssetFarTagDuplicate(string AssetFarTag)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE [ASSET_FAR_TAG] = '" + AssetFarTag.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool CheckAssetTagDuplicate(string _AssetTAg)
        {
            try
            {
                bool bDup = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE ASSET_ID = '" + _AssetTAg.Trim().Replace("'", "''") + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bDup = true;
                return bDup;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataTable GetStore(string SiteCode,string FloorCode,string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT STORE_CODE,STORE_NAME FROM STORE_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND FLOOR_CODE='" + FloorCode + "' AND COMP_CODE='" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetFloor(string SiteCode,string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT FLOOR_CODE,FLOOR_NAME FROM FLOOR_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND COMP_CODE = '"+CompCode+"' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetFloor(string SiteCode, string Floor,string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT FLOOR_CODE,FLOOR_NAME FROM FLOOR_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' and FLOOR_CODE='"+ Floor + "' AND COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetStorewithFloorandSite(string SiteCode, string FloorCode, string StoreCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT STORE_CODE,STORE_NAME FROM STORE_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' and FLOOR_CODE='" + FloorCode + "' AND STORE_CODE = '" + StoreCode + "'  AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetStorewithFloor(string SiteCode, string FloorCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT STORE_CODE,STORE_NAME FROM STORE_MASTER WHERE ACTIVE=1 AND SITE_CODE='" + SiteCode + "' AND FLOOR_CODE = '" + FloorCode + "'  AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetLocation(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SITE_CODE,SITE_ADDRESS FROM SITE_MASTER WHERE ACTIVE=1 AND SITE_CODE <> 'ALL' AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetAssetDomain()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_DOMAIN_CODE,ASSET_DOMAIN_NAME FROM ASSET_DOMAIN_MASTER WHERE ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetSubStatus()
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SUB_STATUS_CODE,SUB_STATUS FROM SUB_STATUS_MASTER WHERE ACTIVE=1");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetSubStatus(string SubStatus)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SUB_STATUS_CODE,SUB_STATUS FROM SUB_STATUS_MASTER WHERE ACTIVE=1 AND SUB_STATUS = '"+SubStatus+"' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable CheckAssetDomain(string _AssetDomain)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM ASSET_DOMAIN_MASTER WHERE ASSET_DOMAIN_CODE = '" + _AssetDomain.Trim().Replace("'", "''") + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetCurrentRequiredDetailsforIT(string _SerialCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialCode.Trim().Replace("'", "''") + "' AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetCurrentRequiredDetailsforFacilities(string _AssetFarTag, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _AssetFarTag.Trim().Replace("'", "''") + "' AND COMP_CODE = '" + CompCode + "'  ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetLocation(string _ParentLocCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  SITE_CODE LOC_CODE, SITE_ADDRESS LOC_NAME FROM SITE_MASTER");
            sbQuery.Append(" WHERE ACTIVE='1' AND SITE_CODE = '" + _ParentLocCode + "' AND COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public bool ChkActiveAssetFarTagExists(string ActiveAssetFarTag, string CompCode)
        {
            bool bExists = false;
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT COUNT(*) AS CSN FROM [ASSET_ACQUISITION]");
            sbQuery.Append(" WHERE ASSET_FAR_TAG='" + ActiveAssetFarTag + "' AND COMP_CODE='" + CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows[0]["CSN"].ToString() != "0")
                bExists = true;
            return bExists;
        }

        public bool ChkisSerialCodeWithLocationExists(string SerialCode, string CompCode)
        {
            try {
                bool bExists = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM [ASSET_ACQUISITION]");
                sbQuery.Append(" WHERE SERIAL_CODE='" + SerialCode + "' AND COMP_CODE='" + CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bExists = true;
                return bExists;
            } 
            catch(Exception EX) { throw EX; }
            
        }

        public bool ChkisAssetFarTagWithLocationExists(string AssetFarTag, string CompCode)
        {
            try
            {
                bool bExists = false;
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT * FROM [ASSET_ACQUISITION]");
                sbQuery.Append(" WHERE ASSET_FAR_TAG='" + AssetFarTag + "' AND COMP_CODE='" + CompCode + "'");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    bExists = true;
                return bExists;
            }
            catch (Exception EX) { throw EX; }

        }

        public DataTable GetVendorforIT(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT VENDOR_CODE, VENDOR_NAME FROM VENDOR_MASTER WHERE ACTIVE=1 and COMP_CODE = '"+CompCode+"' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetVendorforFacilities(string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT VENDOR_CODE, VENDOR_NAME FROM VENDOR_MASTER WHERE ACTIVE=1 and COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetVendor(string VendorCode, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT VENDOR_CODE, VENDOR_NAME FROM VENDOR_MASTER WHERE ACTIVE=1 AND VENDOR_CODE='" + VendorCode + "' and COMP_CODE = '" + CompCode + "' ");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetDetails(string AssetCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select A.*,S.SITE_CODE   from [ASSET_ACQUISITION] A inner join ");
            sbQuery.Append("SITE_MASTER S ON A.ASSET_LOCATION = S.SITE_CODE WHERE A.ASSET_CODE='" + AssetCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }



        public string GetAssetCodeforAcqusition(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "'");
            else
                sbQuery.Append("SELECT ASSET_CODE FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_CODE"].ToString();
            else return "";
        }

        public string GetVendorCodeFilterforAcqusition(string _VendorCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT VENDOR_CODE FROM VENDOR_MASTER WHERE VENDOR_CODE = '" + _VendorCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["VENDOR_CODE"].ToString();
            else return "";
        }

        public string GetSiteCodeFilterforAcqusition(string _SiteCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SITE_CODE FROM SITE_MASTER WHERE SITE_CODE = '" + _SiteCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["SITE_CODE"].ToString();
            else return "";
        }

        public string GetFloorCodeFilterforAcqusition(string _FloorCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT FLOOR_CODE FROM FLOOR_MASTER WHERE FLOOR_CODE = '" + _FloorCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["FLOOR_CODE"].ToString();
            else return "";
        }

        public string GetStoreCodeFilterforAcqusition(string _StoreCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT STORE_CODE FROM STORE_MASTER WHERE STORE_CODE = '" + _StoreCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "'");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["STORE_CODE"].ToString();
            else return "";
        }

        public string GetSubStatusCodeFilterforAcqusition(string _SubStatusCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SUB_STATUS_CODE FROM SUB_STATUS_MASTER WHERE SUB_STATUS_CODE = '" + _SubStatusCode.Trim() + "' ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["SUB_STATUS_CODE"].ToString();
            else return "";
        }

        public string GetAssetDomainCodeFilterforAcqusition(string _AssetDomainCode, string _CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ASSET_DOMAIN_CODE FROM ASSET_DOMAIN_MASTER WHERE ASSET_DOMAIN_CODE = '" + _AssetDomainCode.Trim() + "' ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ASSET_DOMAIN_CODE"].ToString();
            else return "";
        }

        public string GetCompCodetoVerify(string VerifyString, string VerifyType)
        {
            string ReturnValue = string.Empty;
            if (VerifyType == "ASSET_CODE")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COMP_CODE FROM ASSET_ACQUISITION WHERE ASSET_CODE = '" + VerifyString.Trim() + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                   ReturnValue =  dt.Rows[0]["COMP_CODE"].ToString();
            }
            if (VerifyType == "SERIAL_CODE")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COMP_CODE FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + VerifyString.Trim() + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    ReturnValue = dt.Rows[0]["COMP_CODE"].ToString();
            }
            if (VerifyType == "ASSET_TAG")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COMP_CODE FROM ASSET_ACQUISITION WHERE ASSET_ID = '" + VerifyString.Trim() + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    ReturnValue = dt.Rows[0]["COMP_CODE"].ToString();
            }
            if (VerifyType == "RFID_TAG")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COMP_CODE FROM ASSET_ACQUISITION WHERE TAG_ID = '" + VerifyString.Trim() + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    ReturnValue = dt.Rows[0]["COMP_CODE"].ToString();
            }
            if (VerifyType == "ASSET_FAR_TAG")
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT COMP_CODE FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + VerifyString.Trim() + "' ");
                DataTable dt = oDb.GetDataTable(sbQuery.ToString());
                if (dt.Rows.Count > 0)
                    ReturnValue = dt.Rows[0]["COMP_CODE"].ToString();
            }

            return ReturnValue;
        }

        public bool GetFloorforAcqusition(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            bool bRes = false;
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND FLOOR <> '' AND FLOOR IS NOT NULL ");
            else
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND FLOOR <> '' AND FLOOR IS NOT NULL ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                bRes = true;

            return bRes;
        }
        public bool GetStoreforAcqusition(string _SerialOrAssetFarTagCode, string _CompCode)
        {
            bool bRes = false;
            sbQuery = new StringBuilder();
            if (_CompCode == "IT")
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE SERIAL_CODE = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STORE <> '' AND STORE IS NOT NULL ");
            else
                sbQuery.Append("SELECT * FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG = '" + _SerialOrAssetFarTagCode.Trim() + "' AND COMP_CODE = '" + _CompCode + "' AND STORE <> '' AND STORE IS NOT NULL ");
            DataTable dt = oDb.GetDataTable(sbQuery.ToString());
            if (dt.Rows.Count > 0)
                bRes = true;

            return bRes;
        }
        public DataTable GetAssetSerialDetails(string AssetSerial)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select A.*,S.SITE_CODE   from [ASSET_ACQUISITION] A inner join ");
            sbQuery.Append("SITE_MASTER S ON A.ASSET_LOCATION = S.SITE_CODE WHERE A.SERIAL_CODE='" + AssetSerial + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }

        public DataTable GetAssetTaglDetails(string AssetTag)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select A.*,S.SITE_CODE   from [ASSET_ACQUISITION] A inner join ");
            sbQuery.Append("SITE_MASTER S ON A.ASSET_LOCATION = S.SITE_CODE WHERE A.ASSET_ID='" + AssetTag + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetRFIDTagDetails(string RFIDTag)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select A.*,S.SITE_CODE   from [ASSET_ACQUISITION] A inner join ");
            sbQuery.Append("SITE_MASTER S ON A.ASSET_LOCATION = S.SITE_CODE WHERE A.TAG_ID='" + RFIDTag + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetAssetFarTagDetails(string AssetFarTag)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("select A.*,S.SITE_CODE   from [ASSET_ACQUISITION] A inner join ");
            sbQuery.Append("SITE_MASTER S ON A.ASSET_LOCATION = S.SITE_CODE WHERE A.[ASSET_FAR_TAG]='" + AssetFarTag + "'");
            return oDb.GetDataTable(sbQuery.ToString());
        }
        public DataTable GetMailTransactionDetails(string TransactionType, string CompCode)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM MAIL_MASTER WHERE TRANSACTION_TYPE='" + TransactionType + "' AND COMP_CODE = '" + CompCode + "'");
            return oDb.GetDataTable(sbQuery.ToString());

        }
        public bool SaveAssetDomainDetails(string _AssetDomain,string _CreatedBy)
        {
            try
            {
                bool bResult = false;
               
                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO [ASSET_DOMAIN_MASTER] ([ASSET_DOMAIN_CODE],[ASSET_DOMAIN_NAME],[ACTIVE]");
                sbQuery.Append(",[CREATED_BY],[CREATED_ON])");
                sbQuery.Append(" VALUES ");
                sbQuery.Append("('" + _AssetDomain + "','" + _AssetDomain + "','1' ");
                sbQuery.Append(",'" + _CreatedBy + "',GETDATE())");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                        bResult = true;
                
                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool SaveAssetSubStatusDetails(string _AssetSubStatus, string _CreatedBy)
        {
            try
            {
                bool bResult = false;

                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO [SUB_STATUS_MASTER] ([SUB_STATUS_CODE],[SUB_STATUS],[ACTIVE]");
                sbQuery.Append(",[CREATED_BY],[CREATED_ON])");
                sbQuery.Append(" VALUES ");
                sbQuery.Append("('" + _AssetSubStatus + "','" + _AssetSubStatus + "','1' ");
                sbQuery.Append(",'" + _CreatedBy + "',GETDATE())");
                int iRes = oDb.ExecuteQuery(sbQuery.ToString());
                if (iRes > 0)
                    bResult = true;

                return bResult;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public string SaveAssetAcquisition(AssetAcquisition_PRP oPRP)
        {
            try
            {
                //string message = "";
                DataTable dt = oDb.ExecuteSPWithOutput("SP_SaveAssetAcquisition", new SqlParameter("ASSET_CODE", oPRP.AssetCode),
                            new SqlParameter("ASSET_TYPE", oPRP.AssetType),
                            new SqlParameter("ASSET_MAKE", oPRP.AssetMakeName),
                            new SqlParameter("MODEL_NAME", oPRP.AssetModelName),
                            new SqlParameter("IDENTIFIER_LOCATION", oPRP.AssetLocation),
                            new SqlParameter("SERIAL_CODE", oPRP.AssetSerialCode),
                            new SqlParameter("TAG_ID", oPRP.AssetTag),
                            new SqlParameter("ASSET_RFID", oPRP.AssetRFID),
                            new SqlParameter("PROCUREMENT_BUDGET", oPRP.ProcurementBudget),
                            new SqlParameter("FLOOR", oPRP.floor),
                            new SqlParameter("STORE", oPRP.store),
                            new SqlParameter("SUB_CATEGORY", oPRP.SubCategory),
                            new SqlParameter("ASSET_PROCESSOR", oPRP.AssetProcessor),
                            new SqlParameter("ASSET_RAM", oPRP.AssetRAM),
                            new SqlParameter("ASSET_HDD", oPRP.AssetHDD),
                            new SqlParameter("VENDOR_CODE", oPRP.VendorCode),
                            new SqlParameter("GRN_NO", oPRP.GRNNo),
                            new SqlParameter("AMC_WARRANTY", oPRP.AMC_Warranty),
                            new SqlParameter("AMC_WARRANTY_START_DATE", oPRP.AMC_Wrnty_Start_Date),
                            new SqlParameter("AMC_WARRANTY_END_DATE", oPRP.AMC_Wrnty_End_Date),
                            new SqlParameter("PO_NUMBER", oPRP.PurchaseOrderNo),
                            new SqlParameter("PO_DATE", oPRP.PODate),
                            new SqlParameter("INVOICE_NO", oPRP.InvoiceNo),
                            new SqlParameter("INVOICE_DATE", oPRP.InvoiceDate),
                            new SqlParameter("ASSET_LOCATION", oPRP.AssetLocation),
                            new SqlParameter("ASSET_SUB_STATUS", oPRP.SubStatus),
                            new SqlParameter("COMPCODE", oPRP.CompCode),
                            new SqlParameter("REMARKS", oPRP.Remarks),
                            new SqlParameter("CREATED_BY", oPRP.CreatedBy),
                            new SqlParameter("AssetLife", oPRP.AssetLife),
                            new SqlParameter("Asset_FAR_TAG", oPRP.Asset_FAR_TAG),
                            new SqlParameter("ServiceDate", oPRP.SecurityGEDate),
                            new SqlParameter("ASSET_DOMAIN", oPRP.AssetDomain),
                            new SqlParameter("PURCHASE_COST", oPRP.PurchaseCost),
                            new SqlParameter("GRN_DATE", oPRP.GRNDate),
                            new SqlParameter("AssetEndLife", oPRP.AssetEndLife));
                
                return Convert.ToString(dt.Rows[0][0]);
            }
            catch (Exception ex)
            { return ex.Message; }
        }

        public void InvoiceFileUpload(AssetAcquisition_PRP oPRP)
        {
            if (oPRP.upload != null)
                oDb.ExecuteSP("sp_Save_File_Uploads", new SqlParameter("ID", oPRP.upload.ID), new SqlParameter("Process", oPRP.upload.Process), new SqlParameter("FileName", oPRP.upload.FileName), new SqlParameter("Createdby", oPRP.upload.User));
        }

        public string SaveBulkAssetAcquisition(AssetAcquisition_PRP oPRP)
        {
            try
            {
                //string message = "";
                DataTable dt = oDb.ExecuteSPWithOutput("SP_SaveBulkAssetAcquisition", new SqlParameter("ASSET_CODE", oPRP.AssetCode),
                            new SqlParameter("ASSET_TYPE", oPRP.AssetType),
                            new SqlParameter("ASSET_MAKE", oPRP.AssetMakeName),
                            new SqlParameter("MODEL_NAME", oPRP.AssetModelName),
                            new SqlParameter("IDENTIFIER_LOCATION", oPRP.AssetLocation),
                            new SqlParameter("SERIAL_CODE", oPRP.AssetSerialCode),
                            new SqlParameter("TAG_ID", oPRP.AssetTag),
                            new SqlParameter("ASSET_RFID", oPRP.AssetRFID),
                            new SqlParameter("PROCUREMENT_BUDGET", oPRP.ProcurementBudget),
                            new SqlParameter("FLOOR", oPRP.floor),
                            new SqlParameter("STORE", oPRP.store),
                            new SqlParameter("SUB_CATEGORY", oPRP.SubCategory),
                            new SqlParameter("ASSET_PROCESSOR", oPRP.AssetProcessor),
                            new SqlParameter("ASSET_RAM", oPRP.AssetRAM),
                            new SqlParameter("ASSET_HDD", oPRP.AssetHDD),
                            new SqlParameter("VENDOR_CODE", oPRP.VendorCode),
                            new SqlParameter("GRN_NO", oPRP.GRNNo),
                            new SqlParameter("AMC_WARRANTY", oPRP.AMC_Warranty),
                            new SqlParameter("AMC_WARRANTY_START_DATE", oPRP.AMC_Wrnty_Start_Date),
                            new SqlParameter("AMC_WARRANTY_END_DATE", oPRP.AMC_Wrnty_End_Date),
                            new SqlParameter("PO_NUMBER", oPRP.PurchaseOrderNo),
                            new SqlParameter("PO_DATE", oPRP.PODate),
                            new SqlParameter("INVOICE_NO", oPRP.InvoiceNo),
                            new SqlParameter("INVOICE_DATE", oPRP.InvoiceDate),
                            new SqlParameter("ASSET_LOCATION", oPRP.AssetLocation),
                            new SqlParameter("ASSET_SUB_STATUS", oPRP.SubStatus),
                            new SqlParameter("COMPCODE", oPRP.CompCode),
                            new SqlParameter("REMARKS", oPRP.Remarks),
                            new SqlParameter("CREATED_BY", oPRP.CreatedBy),
                            new SqlParameter("AssetLife", oPRP.AssetLife),
                            new SqlParameter("Asset_FAR_TAG", oPRP.Asset_FAR_TAG),
                            new SqlParameter("ServiceDate", oPRP.SecurityGEDate),
                            new SqlParameter("ASSET_DOMAIN", oPRP.AssetDomain),
                            new SqlParameter("PURCHASE_COST", oPRP.PurchaseCost),
                            new SqlParameter("GRN_DATE", oPRP.GRNDate),
                            new SqlParameter("AssetEndLife", oPRP.AssetEndLife));

                if (oPRP.upload != null)
                    oDb.ExecuteSP("sp_Save_File_Uploads", new SqlParameter("ID", oPRP.upload.ID), new SqlParameter("Process", oPRP.upload.Process), new SqlParameter("FileName", oPRP.upload.FileName), new SqlParameter("Createdby", oPRP.upload.User));

                return Convert.ToString(dt.Rows[0][0]);
            }
            catch (Exception ex)
            { return ex.Message; }
        }

        public string SaveBulkUpdateAssetAcquisition(AssetAcquisition_PRP oPRP)
        {
            try
            {
                //string message = "";
                DataTable dt = oDb.ExecuteSPWithOutput("SP_SaveBulkUpdateAssetAcquisition", new SqlParameter("ASSET_CODE", oPRP.AssetCode),
                            new SqlParameter("ASSET_TYPE", oPRP.AssetType),
                            new SqlParameter("ASSET_MAKE", oPRP.AssetMakeName),
                            new SqlParameter("MODEL_NAME", oPRP.AssetModelName),
                            new SqlParameter("IDENTIFIER_LOCATION", oPRP.AssetLocation),
                            new SqlParameter("SERIAL_CODE", oPRP.AssetSerialCode),
                            new SqlParameter("TAG_ID", oPRP.AssetTag),
                            new SqlParameter("ASSET_RFID", oPRP.AssetRFID),
                            new SqlParameter("PROCUREMENT_BUDGET", oPRP.ProcurementBudget),
                            new SqlParameter("FLOOR", oPRP.floor),
                            new SqlParameter("STORE", oPRP.store),
                            new SqlParameter("SUB_CATEGORY", oPRP.SubCategory),
                            new SqlParameter("ASSET_PROCESSOR", oPRP.AssetProcessor),
                            new SqlParameter("ASSET_RAM", oPRP.AssetRAM),
                            new SqlParameter("ASSET_HDD", oPRP.AssetHDD),
                            new SqlParameter("VENDOR_CODE", oPRP.VendorCode),
                            new SqlParameter("GRN_NO", oPRP.GRNNo),
                            new SqlParameter("AMC_WARRANTY", oPRP.AMC_Warranty),
                            new SqlParameter("AMC_WARRANTY_START_DATE", oPRP.AMC_Wrnty_Start_Date),
                            new SqlParameter("AMC_WARRANTY_END_DATE", oPRP.AMC_Wrnty_End_Date),
                            new SqlParameter("PO_NUMBER", oPRP.PurchaseOrderNo),
                            new SqlParameter("PO_DATE", oPRP.PODate),
                            new SqlParameter("INVOICE_NO", oPRP.InvoiceNo),
                            new SqlParameter("INVOICE_DATE", oPRP.InvoiceDate),
                            new SqlParameter("ASSET_LOCATION", oPRP.AssetLocation),
                            new SqlParameter("ASSET_SUB_STATUS", oPRP.SubStatus),
                            new SqlParameter("COMPCODE", oPRP.CompCode),
                            new SqlParameter("REMARKS", oPRP.Remarks),
                            new SqlParameter("CREATED_BY", oPRP.CreatedBy),
                            new SqlParameter("AssetLife", oPRP.AssetLife),
                            new SqlParameter("Asset_FAR_TAG", oPRP.Asset_FAR_TAG),
                            new SqlParameter("ServiceDate", oPRP.SecurityGEDate),
                            new SqlParameter("ASSET_DOMAIN", oPRP.AssetDomain),
                            new SqlParameter("PURCHASE_COST", oPRP.PurchaseCost),
                            new SqlParameter("GRN_DATE", oPRP.GRNDate),
                            new SqlParameter("AssetEndLife", oPRP.AssetEndLife));

                if (oPRP.upload != null)
                    oDb.ExecuteSP("sp_Save_File_Uploads", new SqlParameter("ID", oPRP.upload.ID), new SqlParameter("Process", oPRP.upload.Process), new SqlParameter("FileName", oPRP.upload.FileName), new SqlParameter("Createdby", oPRP.upload.User));

                return Convert.ToString(dt.Rows[0][0]);
            }
            catch (Exception ex)
            { return ex.Message; }
        }

    }
}