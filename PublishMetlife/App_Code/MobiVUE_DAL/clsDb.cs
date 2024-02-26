using System;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace MobiVUE_ATS.DAL
{
    public class clsDb
    {
        SqlConnection m_Con;
        SqlTransaction sqlTrans;
        SqlCommand com;           
        #region PUBLIC PROPERTY
        private string m_DataSource;
        public string DataSource
        {
            get { return m_DataSource; }
            set { m_DataSource = value; }
        }       
        private string m_UserID;
        public string UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }
        private string m_Password;
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }       
        private string m_InitialCatalog;
        public string InitialCatalog
        {
            get { return m_InitialCatalog; }
            set { m_InitialCatalog = value; }
        }
      
        private bool m_IsConnected;
        public bool IsConnected
        {
            get { return m_IsConnected; }
            set { m_IsConnected = value; }
        }       
        #endregion

        public clsDb()
        {
            //ReadSetting();
            m_Con = new SqlConnection();            
        }

        void ReadSetting()
        {            
            //StreamReader sr = new StreamReader(Application.StartupPath + "\\DBSettings.txt");
            //this.DataSource = sr.ReadLine();
            //this.InitialCatalog = sr.ReadLine();
            //this.UserID = sr.ReadLine();
            //this.Password = sr.ReadLine();
            //sr.Close();
        }

        /// <summary>
        /// Connect to Database as per asset type provided.
        /// </summary>
        public void Connect(string DatabaseType)
        {
            try
            {
                string strConnectionString = string.Empty;
                if (DatabaseType == "IT")
                {
                    clsGeneral.gStrAssetType = "IT";
                    strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
                }
                if (DatabaseType == "ADMIN")
                {
                    clsGeneral.gStrAssetType = "ADMIN";
                    strConnectionString = ConfigurationManager.AppSettings["ConnStrAdmin"].ToString();
                }
                m_Con.ConnectionString = strConnectionString;
                m_Con.Open();
                com = new SqlCommand();
                com = m_Con.CreateCommand();
                m_IsConnected = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        /// <summary>
        /// Begin Transection
        /// </summary>
        public void BeginTrans()
        {
            //sqlTrans = new SqlTransaction();
            try
            {
                sqlTrans = m_Con.BeginTransaction(IsolationLevel.ReadCommitted);
                com.Transaction = sqlTrans;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Commit Transection
        /// </summary>
        public void CommitTrans()
        {
            try
            {
                sqlTrans.Commit();
                sqlTrans.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Rollback trasection
        /// </summary>
        ///       
        public void RollBack()
        {
            try
            {
                sqlTrans.Rollback();
                sqlTrans.Dispose();
            }
            catch(Exception )
            {
                throw;
            }
        }

        /// <summary>
        /// Disconnect database connection
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (m_Con != null && m_Con.State == ConnectionState.Open)
                {
                    com.Dispose();
                    m_Con.Close();
                    m_IsConnected = false;
                }
            }
            catch (Exception ex)
            { 
                //throw ex;
            }
        }

        /// <summary>
        /// Fetch data from data set 
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string strSql)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                ds = new DataSet();
                    da = new SqlDataAdapter(strSql, m_Con);
                    da.Fill(ds);
                    da.Dispose();
                    //if (m_Con.State == ConnectionState.Open)
                    //    Disconnect();
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
            return ds;
        }

        public DataTable GetDataTable(string strSql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                dt = new DataTable();
                da = new SqlDataAdapter(strSql, m_Con);
                da.Fill(dt);
                da.Dispose();

                //if (m_Con.State == ConnectionState.Open)
                //    Disconnect();

            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
            return dt;
        }

        public DataTable GetDataTableInTransaction(string strSql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                dt = new DataTable();
                da = new SqlDataAdapter(strSql, m_Con);
                da.SelectCommand.Transaction = sqlTrans;
                da.Fill(dt);
                da.Dispose();

                //if (m_Con.State == ConnectionState.Open)
                //    Disconnect();

            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
            return dt;
        }
        
        public DataSet GetDataSetInTransaction(string strSql)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                ds = new DataSet();
                    da = new SqlDataAdapter(strSql, m_Con);
                    da.SelectCommand.Transaction = sqlTrans;
                    da.Fill(ds);
                    da.Dispose();
                    
                //if (m_Con.State == ConnectionState.Open)
                //        Disconnect();
               
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
            return ds;
        }

        public int ExecuteQuery(string StrSql)
        {            
            int result = 0;
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                com.CommandText = StrSql;
                        result = com.ExecuteNonQuery();
                        //if (m_Con.State == ConnectionState.Open)
                        //    Disconnect();               
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
            return result;
        }
        
        public int ExecuteQueryTran(string StrSql)
        {
            int result = 0;
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                com.Transaction = sqlTrans;
                com.CommandText = StrSql;
                result = com.ExecuteNonQuery();
                //if (m_Con.State == ConnectionState.Open)
                //    Disconnect();
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
            return result;
        }
        
        public int ExecuteCommand(SqlCommand cmd)
        {
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                cmd.Connection = m_Con;
                   return  cmd.ExecuteNonQuery();
                //if (m_Con.State == ConnectionState.Open)
                //    Disconnect();
            }
            catch (Exception)
            { throw; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
        }

        
        public DataTable GetDataTable(SqlCommand command)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                dt = new DataTable();
                da = new SqlDataAdapter();
                da.SelectCommand = command;
                command.Connection = this.m_Con;
                da.Fill(dt);
                da.Dispose();

            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
            return dt;
        }

        public DataTable ExecuteSPWithOutput(string SPName, params SqlParameter[] sqlParam)
        {
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                DataTable table = new DataTable();
                using (var cmd = new SqlCommand(SPName, m_Con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (sqlParam.Length > 0)
                        cmd.Parameters.AddRange(sqlParam);
                    da.Fill(table);
                }
                return table;
            }
            catch (SqlException ex)
            { throw; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
        }

        public bool ExecuteSP(string SPName, params SqlParameter[] sqlParam)
        {
            try
            {
                if (m_Con.State == ConnectionState.Closed)
                    Connect("ADMIN");
                using (var cmd = new SqlCommand(SPName, m_Con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (sqlParam.Length > 0)
                    {
                        cmd.Parameters.AddRange(sqlParam);
                    }
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException ex)
            { return false; }
            finally
            {
                if (m_Con.State == ConnectionState.Open)
                    Disconnect();
            }
        }

    }
}