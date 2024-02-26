using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using MobiVUE_ATS.DAL;
using Newtonsoft.Json;

/// <summary>
/// Summary description for AutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AutoComplete : System.Web.Services.WebService
{
    string strConnectionString = string.Empty;
    public AutoComplete()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    //[WebMethod]
    //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    //public string GetBarChartData()
    //{
    //    var data = new List<BarGraph_PRP>();
    //    JavaScriptSerializer js = new JavaScriptSerializer();
    //    try
    //    {
    //        AssetAcq_DAL oDAL = new AssetAcq_DAL("ADMIN");
    //        data = oDAL.GetBarChartValues();
    //        return js.Serialize(data);
    //    }
    //    catch (Exception ex)
    //    {
    //        return js.Serialize(data);
    //    }
    //}
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string GetBarChartData(string CompCode, string QueryType)
    {
        var data = new List<BarGraph_PRP>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            AssetAcq_DAL oDAL = new AssetAcq_DAL("ADMIN");
            data = oDAL.GetBarChartValues(CompCode, QueryType);
            //return js.Serialize(data);
            return JsonConvert.SerializeObject(value: data);
        }
        catch (Exception ex)
        {
            return js.Serialize(data);
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string GetBarChartDataLocationWithStatus(string CompCode, string QueryType)
    {
        var data = new List<BarGraphLocationWithStatus_PRP>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            AssetAcq_DAL oDAL = new AssetAcq_DAL("ADMIN");
            data = oDAL.GetBarChartValuesLocationWithstatus(CompCode, QueryType);
            //return js.Serialize(data);
            return JsonConvert.SerializeObject(value: data);
        }
        catch (Exception ex)
        {
            return js.Serialize(data);
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string GetBarChartforStock(string CompCode, string QueryType, string Location)
    {
        var data = new List<BarGraphStock_PRP>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            AssetAcq_DAL oDAL = new AssetAcq_DAL("ADMIN");
            data = oDAL.GetBarChartValuesWithStock(CompCode, QueryType, Location);
            //return js.Serialize(data);
            return JsonConvert.SerializeObject(value: data);
        }
        catch (Exception ex)
        {
            return js.Serialize(data);
        }
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string GetBarChartforAllocated(string CompCode, string QueryType)
    {
        var data = new List<BarGraphAllocated_PRP>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            AssetAcq_DAL oDAL = new AssetAcq_DAL("ADMIN");
            data = oDAL.GetBarChartValuesWithAllocated(CompCode, QueryType);
            return JsonConvert.SerializeObject(value: data);
        }
        catch (Exception ex)
        {
            return js.Serialize(data);
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string GetBarChartforInTransit(string CompCode, string QueryType)
    {
        var data = new List<BarGraphInTransit_PRP>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            AssetAcq_DAL oDAL = new AssetAcq_DAL("ADMIN");
            data = oDAL.GetBarChartValuesWithInTransit(CompCode, QueryType);
            return JsonConvert.SerializeObject(value: data);
        }
        catch (Exception ex)
        {
            return js.Serialize(data);
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string GetPieChartData(string CompCode, string QueryType, string LocationCode)
    {
        var data = new List<PieGraph_PRP>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            AssetAcq_DAL oDAL = new AssetAcq_DAL("ADMIN");
            data = oDAL.GetPieChartValues(CompCode, QueryType, LocationCode);
            //return js.Serialize(data);
            return JsonConvert.SerializeObject(value: data);
        }
        catch (Exception ex)
        {
            return js.Serialize(data);
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string GetDonutChartData(string CompCode, string QueryType, string LocationCode, string FloorCode)
    {
        var data = new List<DonutGraph_PRP>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            AssetAcq_DAL oDAL = new AssetAcq_DAL("ADMIN");
            data = oDAL.GetDonutChartValues(CompCode, QueryType, LocationCode, FloorCode);
            //return js.Serialize(data);
            return JsonConvert.SerializeObject(value: data);
        }
        catch (Exception ex)
        {
            return js.Serialize(data);
        }
    }
    
    [WebMethod]
    public static string GetChart()
    {
        string values = string.Empty;
        string constr = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();

        using (SqlConnection con = new SqlConnection(constr))
        {
            string query = string.Format("SELECT Month, Turnover FROM Sales");
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[");
                    sb.Append("{");
                    sb.Append("labels: [");
                    while (sdr.Read())
                    {
                        sb.Append(string.Format("'{0}',", sdr[0]));
                        values += (string.Format("{0},", sdr[1]));
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    values = values.Remove(values.Length - 1, 1);
                    sb.Append("]");
                    sb.Append(",");
                    sb.Append("datasets: [");
                    sb.Append("{");
                    sb.Append(@"label: ""My First dataset"",
                            fillColor: ""rgba(220,220,220,0.5)"",
                            strokeColor: ""rgba(220,220,220,0.8)"",
                            highlightFill: ""rgba(220,220,220,0.75)"",
                            highlightStroke: ""rgba(220,220,220,1)"",");

                    sb.Append("data:[");
                    sb.Append(values);
                    sb.Append("]");
                    sb.Append("}");
                    sb.Append("]");
                    sb.Append("}");
                    sb.Append("]");
                    con.Close();
                    return sb.ToString();
                }
            }
        }
    }
    [WebMethod]
    public string[] GetAssetCode(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  Tag_ID + ' | '+ ASSET_CODE +' | '+ ASSET_MAKE +' | '+ MODEL_NAME FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append(" (ASSET_CODE LIKE '" + prefixText + "%' OR Tag_ID LIKE '" + prefixText + "%' OR ASSET_MAKE LIKE '" + prefixText + "%' OR MODEL_NAME LIKE '" + prefixText + "%' ) AND SOLD_SCRAPPED_STATUS IS NULL And  ISNULL(Tag_ID, '') <> '' and Status='STOCK'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetALLAssetCode(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  Tag_ID + ' | '+ ASSET_CODE FROM ASSET_ACQUISITION WHERE (ASSET_CODE LIKE '" + prefixText + "%'");
            sbQuery.Append(" OR Tag_ID LIKE  '" + prefixText + "%')  And  Tag_ID is not null and Tag_ID <>''");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAllocatedAssetValue(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT   ASSET_CODE FROM ASSET_ACQUISITION WHERE ASSET_CODE LIKE '" + prefixText + "%'");
            sbQuery.Append("  and   Status='Allocated'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetAssetRFIDTag(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  Tag_ID  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append("  Tag_ID LIKE  '" + prefixText + "%' And  Tag_ID is not null and Tag_ID <>'' and   Status='Allocated'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetGRNNo(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  GRN_NO  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append("  GRN_NO LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetID(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  ASSET_ID  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append("  ASSET_ID LIKE  '" + prefixText + "%' and   Status='Allocated'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetEmployeeId(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct [EMPLOYEE_CODE]  FROM [EMPLOYEE_MASTER] WHERE ");
            sbQuery.Append("  [EMPLOYEE_CODE] LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetEmployeeName(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct [EMPLOYEE_NAME]  FROM [EMPLOYEE_MASTER] WHERE ");
            sbQuery.Append(" [EMPLOYEE_NAME] LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetFloor(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct [FLOOR_CODE]  FROM [FLOOR_MASTER] WHERE ");
            sbQuery.Append(" [FLOOR_CODE] LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetSeatNo(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  distinct [SEAT_NO]  FROM [ASSET_ACQUISITION] WHERE ");
            sbQuery.Append(" [SEAT_NO] LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetDesignation(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  distinct [Designation]  FROM [ASSET_ACQUISITION] WHERE ");
            sbQuery.Append(" [Designation] LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetProcessName(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  distinct [PROCESS_NAME]  FROM [ASSET_ACQUISITION] WHERE ");
            sbQuery.Append(" [PROCESS_NAME] LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetAssetSubStatus(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  distinct [ASSET_SUB_STATUS]  FROM [ASSET_ACQUISITION] WHERE ");
            sbQuery.Append(" [ASSET_SUB_STATUS] LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetAllocationStatus(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  distinct [STATUS]  FROM [ASSET_ACQUISITION] WHERE ");
            sbQuery.Append(" [STATUS] LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetIndentifierLocation(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  distinct [Identifier_Location]  FROM [ASSET_ACQUISITION] WHERE ");
            sbQuery.Append(" [Identifier_Location] LIKE  '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetAllocatedAssetCode(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  Tag_ID + ' | '+ ASSET_CODE FROM ASSET_ACQUISITION WHERE (ASSET_CODE LIKE '" + prefixText + "%'");
            sbQuery.Append(" OR Tag_ID LIKE  '" + prefixText + "%') And  Tag_ID is not null and Tag_ID <>'' and   Status='Allocated'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetAssetModel(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct  MODEL_NAME  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append(" MODEL_NAME LIKE '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetProcessor(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct  ASSET_PROCESSOR  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append(" ASSET_PROCESSOR LIKE '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [WebMethod]
    public string[] GetAssetRAM(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct  ASSET_RAM  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append(" ASSET_RAM LIKE '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetHDD(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct  ASSET_HDD  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append(" ASSET_HDD LIKE '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetDomain(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct  ASSET_DOMAIN  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append(" ASSET_DOMAIN LIKE '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetMake(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct  ASSET_MAKE  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append(" ASSET_MAKE LIKE '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetAssetSubCategory(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct  ASSET_MAKE  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append(" ASSET_MAKE LIKE '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetAssetType(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct  ASSET_TYPE  FROM ASSET_ACQUISITION WHERE ");
            sbQuery.Append(" ASSET_TYPE LIKE '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetAssetSerialNo(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT SERIAL_CODE FROM ASSET_ACQUISITION WHERE SERIAL_CODE LIKE '" + prefixText + "%'");
            sbQuery.Append(" AND SOLD_SCRAPPED_STATUS IS NULL");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetSerialNumber(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct SERIAL_CODE FROM ASSET_ACQUISITION WHERE SERIAL_CODE LIKE '" + prefixText + "%'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetSiteLocation(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT distinct  [SITE_CODE]  FROM [SITE_MASTER] WHERE ");
            sbQuery.Append(" [SITE_CODE] LIKE '" + prefixText + "%' ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public string[] GetAssetWorkStationNo(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT WORKSTATION_NO FROM ASSET_ACQUISITION WHERE WORKSTATION_NO LIKE '" + prefixText + "%'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetPortNo(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT PORT_NO FROM ASSET_ACQUISITION WHERE PORT_NO LIKE '" + prefixText + "%'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetFAMSId(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT ASSET_ID FROM ASSET_ACQUISITION WHERE ASSET_ID LIKE '" + prefixText + "%'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetFarTagforAllocation(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT ASSET_FAR_TAG FROM ASSET_ACQUISITION WHERE ASSET_FAR_TAG LIKE '" + prefixText + "%'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetInvoiceNo(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT INVOICE_NO FROM ASSET_ACQUISITION WHERE INVOICE_NO LIKE '" + prefixText + "%'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetPONo(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT PO_NUMBER FROM ASSET_ACQUISITION WHERE PO_NUMBER LIKE '" + prefixText + "%'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetAssetVendor(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT VENDOR_NAME FROM ASSET_ACQUISITION AA INNER JOIN VENDOR_MASTER VM ON AA.VENDOR_CODE = VM.VENDOR_CODE WHERE VM.VENDOR_NAME LIKE '" + prefixText + "%'");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string[] GetGroupCode(string prefixText)
    {
        try
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT  GROUP_CODE FROM GROUP_MASTER WHERE ");
            sbQuery.Append(" (GROUP_CODE LIKE '" + prefixText + "%') ");
            if (clsGeneral.gStrAssetType == "IT")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrIT"].ToString();
            if (clsGeneral.gStrAssetType == "ADMIN")
                strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
            SqlDataAdapter da = new SqlDataAdapter(sbQuery.ToString(), strConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int iCnt = 0;
            if (dt.Rows.Count > 100)
                iCnt = 100;
            else
                iCnt = dt.Rows.Count;
            string[] items = new string[iCnt];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr[0].ToString(), i);
                i++;
                if (i > iCnt - 1)
                    break;
            }
            return items;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}