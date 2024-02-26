using MobiVUE_ATS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Depreciation_DAL
/// </summary>
public class Depreciation_DAL
{
    clsDb oDb;
    StringBuilder sbQuery;

    public Depreciation_DAL(string DatabaseType)
    {
        oDb = new clsDb();
        if (DatabaseType != "")
        {
            oDb.Connect(DatabaseType);
        }
    }
    ~Depreciation_DAL()
    {
        oDb.Disconnect();
        oDb = null;
        sbQuery = null;
    }

    public DataTable GetData(string CompCode , DateTime? ToDate)
    {
        try
        {
            DataTable tbl;
           using(SqlCommand  command =new SqlCommand ())
           {
               command.CommandText = "USP_DepreciationReport";
               command.CommandType = CommandType.StoredProcedure;
               command.Parameters.AddWithValue("@ToDate", ToDate);
               command.Parameters.AddWithValue("@CompCode", CompCode);
              tbl = oDb.GetDataTable(command);
           }
           return tbl ;
        }
        catch (Exception)
        {
            throw;
        }
    }




}