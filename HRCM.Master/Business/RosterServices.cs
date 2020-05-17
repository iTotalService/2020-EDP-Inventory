using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using HRCM.Master.AppConfig;
using System.Data;
using Microsoft.Extensions.Logging;
using HRCM.Master.Model;
using HRCM.Master.Model.Common;

namespace HRCM.Master.Business
{
    public class RosterServices : BaseServices
    {
        

        public RosterServices()
        {
            OpenConnection("HRCM");
        }

        public List<ScheduleData> getRosterSource()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRosterSource));
            List<ScheduleData> cList = new List<ScheduleData>();
            DateTime? dateNull = null;
            DataTable dt = GetRosterRecords();
            foreach (DataRow dr in dt.Rows)
            {
                cList.Add(new ScheduleData()
                {
                    Id = Convert.ToInt32(dr["ID"]),
                    Name = dr["Name"].ToString(),
                    RosterStartTime = string.IsNullOrEmpty(dr["RosterStartTime"].ToString()) ? dateNull : Convert.ToDateTime(dr["RosterStartTime"]),
                    RosterEndTime = string.IsNullOrEmpty(dr["RosterEndTime"].ToString()) ? dateNull : Convert.ToDateTime(dr["RosterEndTime"]),
                    AttendStartTime = string.IsNullOrEmpty(dr["AttendStartTime"].ToString()) ? dateNull : Convert.ToDateTime(dr["AttendStartTime"]),
                    AttendEndTime = string.IsNullOrEmpty(dr["AttendEndTime"].ToString()) ? dateNull : Convert.ToDateTime(dr["AttendEndTime"]),
                    ConsultantID = Convert.ToInt32(dr["employeeID"]),
                    DepartmentID = Convert.ToInt32(dr["posiID"]),
                    Description = dr["Description"].ToString(),
                    CategoryColor = string.IsNullOrEmpty(dr["AttendStartTime"].ToString()) ? "#bbdc00" : "9e5fff",
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getRosterSource));
            return cList;
        }

        public IEnumerable<Dept> getRosters()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRosters));
            List<Dept> DeptList = new List<Dept>();
            //DataTable dt = GetRosterRecords();
            //DateTime? tmp = null;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    DeptList.Add(new Dept()
            //    {
            //        ID = Convert.ToInt64(dr["ID"]),
            //        DE_CODE = dr["DE_CODE"].ToString(),
            //        DE_DESC = dr["DE_DESC"].ToString(),
            //        CR_USR = dr["CR_USR"].ToString(),
            //        CR_DATE = Convert.ToDateTime(dr["CR_DATE"]),
            //        UP_USR = dr["UP_USR"].ToString(),
            //        UP_DATE = string.IsNullOrEmpty(dr["UP_DATE"].ToString()) ? tmp : Convert.ToDateTime(dr["UP_DATE"])
            //    });
            //}
            //Logger.Debug("'{0}' has been invoked and End", nameof(getRosters));
            return DeptList; 
        }

        #region DB Process
        public DataTable GetRosterRecords()
        {
            SQL = "select tat.ID, w.WO_CODE [Name], w.WO_DESC [Description], employeeID, dbo.getEmployeeAttribute(employeeID, 'POSI') posiID,";
            SQL += " cast(tat.tatDate as datetime) + cast(w.WO_T1 as datetime) RosterStartTime,";
            SQL += " cast(tat.tatDate as datetime) + cast(w.WO_T2 as datetime) RosterEndTime, ";
            SQL += " tatT1 AttendStartTime, tatT2 AttendEndTime";
            SQL += " From tattd tat";
            SQL += " join worktime w on tat.roster = w.id";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetRosterRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetRosterRecords), dt.Rows.Count.ToString());
            return dt;
        }
        #endregion
    }
}
