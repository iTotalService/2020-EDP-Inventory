using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Extensions.Logging;
using iTotal.Master.Model;

namespace iTotal.Master.Business
{
    public class SectionServices : BaseServices
    {
        

        public SectionServices()
        {
            OpenConnection("sys");
        }

        public IEnumerable<Section> getRecords()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRecords));
            List<Section> sList = new List<Section>();
            DataTable dt = GetDBRecords();
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                sList.Add(new Section()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    SectionCode = dr["SectionCode"].ToString(),
                    Description = dr["Description"].ToString(),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                    LastUpdatedDate = string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()) ? tmp : Convert.ToDateTime(dr["LastUpdatedDate"])
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getRecords));
            return sList;
        }
        public Section getRecords(long ID)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRecords));
            Section sList = new Section();
            DataTable dt = GetDBRecords(ID);
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                sList = new Section()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    SectionCode = dr["SectionCode"].ToString(),
                    Description = dr["Description"].ToString(),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                    LastUpdatedDate = string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()) ? tmp : Convert.ToDateTime(dr["LastUpdatedDate"])
                };
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getRecords));
            return sList;
        }
        public Section getRecords(string code)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRecords));
            Section sList = new Section();
            DataTable dt = GetDBRecords(code);
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                sList = new Section()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    SectionCode = dr["SectionCode"].ToString(),
                    Description = dr["Description"].ToString(),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                    LastUpdatedDate = string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()) ? tmp : Convert.ToDateTime(dr["LastUpdatedDate"])
                };
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getRecords));
            return sList;
        }
        public long getRecordsbyName(string name)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRecords));
            Section sList = new Section();
            return Convert.ToInt64(GetDBRecordbyName(name));
        }

        #region DB Process
        public string GetDBRecordbyName(string name)
        {
            SQL = "select cast(ID as varchar(20)) from Section where deleted2 = 0 and Description = '" + name + "'";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecordbyName));
            cmd = new SqlCommand(SQL, conn);
            var rtn = getValue(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecordbyName), rtn.ToString());
            return rtn;
        }
        public DataTable GetDBRecords()
        {
            SQL = "select * from Section where deleted2 = 0";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecords), dt.Rows.Count.ToString());
            return dt;
        }
        public DataTable GetDBRecords(string code)
        {
            SQL = "select * from Section where deleted2 = 0 and SectionCode = '" + code + "'";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecords), dt.Rows.Count.ToString());
            return dt;
        }
        public DataTable GetDBRecords(long ID)
        {
            SQL = "select * from Section where deleted2 = 0 and id = " + ID.ToString();
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecords), dt.Rows.Count.ToString());
            return dt;
        }
        public void Update(Section record)
        {
            SQL = "update Section set";
            SQL += " SectionCode = @SectionCode, ";
            SQL += " Description = @Description, ";
            SQL += " LastUpdatedDate = getdate()";
            SQL += " where ID = @ID ";

            Logger.Debug("'{0}' has been invoked and start", nameof(Update));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = record.ID;
            cmd.Parameters.Add("@SectionCode", SqlDbType.NVarChar, 20).Value = record.SectionCode;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 60).Value = record.Description;


            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Update));

        }
        public void Insert(Section record)
        {
            SQL = "insert into Section (";
            SQL += " SectionCode,Description, deleted2 ";
            SQL += " ) values (";
            SQL += " @SectionCode,@Description,0 ";
            SQL += ")";

            Logger.Debug("'{0}' has been invoked and start", nameof(Insert));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@SectionCode", SqlDbType.NVarChar, 20).Value = record.SectionCode;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 60).Value = record.Description;
            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Insert));

        }

        public void Delete(string key)
        {
            SQL = "update Section set";
            SQL += " DELETED2 = 1, ";
            SQL += " DeletedDate = getdate()";
            SQL += " where ID = @ID ";

            Logger.Debug("'{0}' has been invoked and start", nameof(Delete));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = key;


            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Delete));
        }

        public bool chkRecord(Section record)
        {
            SQL = "select * from Section " +
                "where SectionCode = '" + record.SectionCode + "' and DELETED2 = 0";
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(Update), dt.Rows.Count.ToString());
            return dt.Rows.Count > 0;
        }
        #endregion
    }
}
