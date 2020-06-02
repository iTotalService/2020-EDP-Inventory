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
    public class PosiServices : BaseServices
    {
        

        public PosiServices()
        {
            OpenConnection("HRCM");
        }

        public List<ResourceDataSourceModel> getPosiSource()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getPosis));
            List<ResourceDataSourceModel> cList = new List<ResourceDataSourceModel>();
            DataTable dt = GetPosiRecords();
            foreach (DataRow dr in dt.Rows)
            {
                cList.Add(new ResourceDataSourceModel()
                {
                    id = Convert.ToInt32(dr["ID"]),
                    text = dr["PO_DESC"].ToString(),
                    color = "#bbdc00"
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getPosis));
            return cList;
        }

        public IEnumerable<Posi> getPosis()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getPosis));
            List<Posi> cList = new List<Posi>();
            DataTable dt = GetPosiRecords();
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                cList.Add(new Posi()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    PO_CODE = dr["PO_CODE"].ToString(),
                    PO_DESC = dr["PO_DESC"].ToString(),
                    CR_USR = dr["CR_USR"].ToString(),
                    CR_DATE = Convert.ToDateTime(dr["CR_DATE"]),
                    UP_USR = dr["UP_USR"].ToString(),
                    UP_DATE = string.IsNullOrEmpty(dr["UP_DATE"].ToString()) ? tmp : Convert.ToDateTime(dr["UP_DATE"])
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getPosis));
            return cList; 
        }

        #region DB Process
        public DataTable GetPosiRecords()
        {
            SQL = "select * from POSI where deleted = 0";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetPosiRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetPosiRecords), dt.Rows.Count.ToString());
            return dt;
        }
        #endregion
    }
}
