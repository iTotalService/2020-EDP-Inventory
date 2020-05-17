﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Extensions.Logging;
using iTotal.Master.Model;

namespace iTotal.Master.Business
{
    public class LocationServices : BaseServices
    {
        

        public LocationServices()
        {
            OpenConnection("sys");
        }

        public IEnumerable<Location> getRecords()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRecords));
            List<Location> sList = new List<Location>();
            DataTable dt = GetDBRecords();
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                sList.Add(new Location()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    LocationCode = dr["LocationCode"].ToString(),
                    Description = dr["Description"].ToString(),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                    LastUpdatedDate = string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()) ? tmp : Convert.ToDateTime(dr["LastUpdatedDate"])
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getRecords));
            return sList;
        }
        public Location getLocation(long ID)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getLocation));
            Location sList = new Location();
            DataTable dt = GetDBRecord(ID);
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                sList = new Location()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    LocationCode = dr["LocationCode"].ToString(),
                    Description = dr["Description"].ToString(),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                    LastUpdatedDate = string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()) ? tmp : Convert.ToDateTime(dr["LastUpdatedDate"])
                };
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getLocation));
            return sList;
        }
        public Location getLocation(string code)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getLocation));
            Location sList = new Location();
            DataTable dt = GetDBRecord(code);
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                sList = new Location()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    LocationCode = dr["LocationCode"].ToString(),
                    Description = dr["Description"].ToString(),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                    LastUpdatedDate = string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()) ? tmp : Convert.ToDateTime(dr["LastUpdatedDate"])
                };
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getLocation));
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
            SQL = "select cast(ID as varchar(20)) from Location where deleted2 = 0 and Description = '" + name + "'";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecordbyName));
            cmd = new SqlCommand(SQL, conn);
            var rtn = getValue(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecordbyName), rtn.ToString());
            return rtn;
        }
        public DataTable GetDBRecord(string code)
        {
            SQL = "select * from Location where deleted2 = 0 and LocationCode = " + code.ToString();
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecord));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecord), dt.Rows.Count.ToString());
            return dt;
        }
        public DataTable GetDBRecord(long ID)
        {
            SQL = "select * from Location where deleted2 = 0 and ID = " + ID.ToString();
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecord));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecord), dt.Rows.Count.ToString());
            return dt;
        }
        public DataTable GetDBRecords()
        {
            SQL = "select * from Location where deleted2 = 0";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecords), dt.Rows.Count.ToString());
            return dt;
        }
        public void Update(Location record)
        {
            SQL = "update Location set";
            SQL += " LocationCode = @LocationCode, ";
            SQL += " Description = @Description, ";
            SQL += " LastUpdatedDate = getdate()";
            SQL += " where ID = @ID ";

            Logger.Debug("'{0}' has been invoked and start", nameof(Update));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = record.ID;
            cmd.Parameters.Add("@LocationCode", SqlDbType.NVarChar, 20).Value = record.LocationCode;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 60).Value = record.Description;


            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Update));

        }
        public void Insert(Location record)
        {
            SQL = "insert into Location (";
            SQL += " LocationCode,Description, deleted2 ";
            SQL += " ) values (";
            SQL += " @LocationCode,@Description,0 ";
            SQL += ")";

            Logger.Debug("'{0}' has been invoked and start", nameof(Insert));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@LocationCode", SqlDbType.NVarChar, 20).Value = record.LocationCode;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 60).Value = record.Description;
            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Insert));

        }

        public void Delete(string key)
        {
            SQL = "update Location set";
            SQL += " DELETED2 = 1, ";
            SQL += " DeletedDate = getdate()";
            SQL += " where ID = @ID ";

            Logger.Debug("'{0}' has been invoked and start", nameof(Delete));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = key;


            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Delete));
        }

        public bool chkRecord(Location record)
        {
            SQL = "select * from Location " +
                "where LocationCode = '" + record.LocationCode + "' and DELETED2 = 0";
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(Update), dt.Rows.Count.ToString());
            return dt.Rows.Count > 0;
        }
        #endregion
    }
}