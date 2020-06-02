using HRCM.Master.AppConfig;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HRCM.Master
{
    public class BaseServices
    {
        private SqlConnection _conn = new SqlConnection();
        private SqlTransaction _trans;
        private SqlCommand _command = new SqlCommand();
        protected string SQL = string.Empty;
        protected DataTable dt = null;
        protected Logger Logger;
        protected DateTime startTime;
        protected DateTime endTime;

        public BaseServices()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region Basic Config DB Function
        protected void OpenConnection(String Module)
        {
            AppConfiguration ac = new AppConfiguration();
            _conn.ConnectionString = ac.ConnectionString(Module);
            _conn.Open();
        }

        protected void OpenConnectionbySetting(String Module)
        {
            AppConfiguration ac = new AppConfiguration();
            _conn.ConnectionString = ac.appSetting(Module);
            _conn.Open();
        }
        #endregion

        #region Database Prop
        public SqlTransaction Trans
        {
            get { return _trans; }
            set { _trans = value; }
        }
        public SqlConnection conn
        {
            get { return _conn; }
            set { _conn = value; }
        }
        public SqlCommand cmd
        {
            get { return _command; }
            set { _command = value; }
        }
        #endregion

        #region DataBase Function

        public string getValue(SqlCommand cmd)
        {
            SqlConnection conn = cmd.Connection;
            string txtID = "";

            try
            {
                startTime = DateTime.Now;
                if (_trans != null) { cmd.Transaction = _trans; }
                if (conn.State != ConnectionState.Open) { conn.Open(); }
                //Logger.Debug("SQL Statement Trace          [" + cmd.CommandText + "]");
                txtID = (String)cmd.ExecuteScalar();
                endTime = DateTime.Now;
                TimeSpan ts = endTime - startTime;
                NLog.GlobalDiagnosticsContext.Set("SQLText", cmd.CommandText);
                NLog.GlobalDiagnosticsContext.Set("TimeElapsed", ts.TotalMilliseconds.ToString() + "ms.");
                Logger.Debug("SQL Command [{0}] has been invoked", nameof(getValue));
            }
            catch (SqlException e)
            {
                NLog.GlobalDiagnosticsContext.Set("SQLText", cmd.CommandText);
                Logger.Error("Error found in SQL Command [{0}]", nameof(getValue));
            }
            finally
            {
                if (_trans == null)
                {
                    conn.Close();
                }
            }
            return txtID;

        }

        public int RunSQL(SqlCommand cmd)
        {
            int result = 0;
            SqlTransaction lTran;
            try
            {
                startTime = DateTime.Now;
                cmd.Connection = _conn;
                if (_trans != null)
                {
                    cmd.Transaction = _trans;
                }
                //Add by Kenneth Tang 2008-10-26 for make sure all update should have begin tran and commit
                else
                {
                    if (_conn.State != ConnectionState.Open) { _conn.Open(); }
                    lTran = cmd.Connection.BeginTransaction();
                    cmd.Transaction = lTran;
                }
                if (_conn.State != ConnectionState.Open) { _conn.Open(); }
                result = cmd.ExecuteNonQuery();
                endTime = DateTime.Now;
                TimeSpan ts = endTime - startTime;
                NLog.GlobalDiagnosticsContext.Set("SQLText", cmd.CommandText);
                NLog.GlobalDiagnosticsContext.Set("TimeElapsed", ts.TotalMilliseconds.ToString() + "ms.");
                Logger.Debug("SQL Command [{0}] has been invoked", nameof(RunSQL));

            }
            catch (SqlException e)
            {
                NLog.GlobalDiagnosticsContext.Set("SQLText", cmd.CommandText);
                Logger.Error("Error found in SQL Command [{0}]", nameof(RunSQL));
            }
            catch (Exception e)
            {
                NLog.GlobalDiagnosticsContext.Set("SQLText", cmd.CommandText);
                Logger.Error("Error found in SQL Command [{0}]", nameof(RunSQL));
            }
            finally
            {
                if (_trans == null)
                {
                    cmd.Transaction.Commit();
                    _conn.Close();
                }
            }
            return result;
        }

        public DataTable GetDataTable(SqlCommand cmd)
        {
            if (_trans != null) { cmd.Transaction = _trans; }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                startTime = DateTime.Now;
                if (conn.State != ConnectionState.Open) { conn.Open(); }
                da.Fill(ds, "dt");
                dt = ds.Tables["dt"];
                endTime = DateTime.Now;
                TimeSpan ts = endTime - startTime;
                NLog.GlobalDiagnosticsContext.Set("SQLText", cmd.CommandText);
                NLog.GlobalDiagnosticsContext.Set("TimeElapsed", ts.TotalMilliseconds.ToString() + "ms.");
                Logger.Debug("SQL Command [{0}] has been invoked", nameof(GetDataTable));
            }
            catch (SqlException e)
            {
                NLog.GlobalDiagnosticsContext.Set("SQLText", cmd.CommandText);
                Logger.Error("Error found in SQL Command [{0}]", nameof(GetDataTable));
            }
            finally
            {
                if (_trans == null)
                {
                    conn.Close();
                }
            }
            return dt;
        }

        #endregion

    }
}
