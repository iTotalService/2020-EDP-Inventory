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
    public class DeptServices : BaseServices
    {
        

        public DeptServices()
        {
            OpenConnection("HRCM");
        }

        public List<ResourceDataSourceModel> getDeptSource()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getDeptSource));
            List<ResourceDataSourceModel> cList = new List<ResourceDataSourceModel>();
            DataTable dt = GetDeptRecords();
            foreach (DataRow dr in dt.Rows)
            {
                cList.Add(new ResourceDataSourceModel()
                {
                    id = Convert.ToInt32(dr["ID"]),
                    text = dr["DE_DESC"].ToString(),
                    color = "#bbdc00"
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getDeptSource));
            return cList;
        }

        public IEnumerable<Dept> getDepts()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getDepts));
            List<Dept> DeptList = new List<Dept>();
            DataTable dt = GetDeptRecords();
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                DeptList.Add(new Dept()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    DE_CODE = dr["DE_CODE"].ToString(),
                    DE_DESC = dr["DE_DESC"].ToString(),
                    CR_USR = dr["CR_USR"].ToString(),
                    CR_DATE = Convert.ToDateTime(dr["CR_DATE"]),
                    UP_USR = dr["UP_USR"].ToString(),
                    UP_DATE = string.IsNullOrEmpty(dr["UP_DATE"].ToString()) ? tmp : Convert.ToDateTime(dr["UP_DATE"])
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getDepts));
            return DeptList; 
        }

        #region DB Process
        public DataTable GetDeptRecords()
        {
            SQL = "select * from DEPT where deleted = 0";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDeptRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDeptRecords), dt.Rows.Count.ToString());
            return dt;
        }
        #endregion
    }
}
