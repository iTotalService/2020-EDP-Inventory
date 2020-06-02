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
    public class StockServices : BaseServices
    {
        

        public StockServices()
        {
            OpenConnection("sys");
        }

        //public List<ResourceDataSourceModel> getDeptSource()
        //{
        //    Logger.Debug("'{0}' has been invoked and start", nameof(getDeptSource));
        //    List<ResourceDataSourceModel> cList = new List<ResourceDataSourceModel>();
        //    DataTable dt = GetDeptRecords();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        cList.Add(new ResourceDataSourceModel()
        //        {
        //            id = Convert.ToInt32(dr["ID"]),
        //            text = dr["DE_DESC"].ToString(),
        //            color = "#bbdc00"
        //        });
        //    }
        //    Logger.Debug("'{0}' has been invoked and End", nameof(getDeptSource));
        //    return cList;
        //}

        public IEnumerable<Stock> getStockNames()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getStockNames));
            List<Stock> cList = new List<Stock>();
            DataTable dt = GetStockNames();
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                cList.Add(new Stock()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    Description = dr["Description"].ToString(),
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getStockNames));
            return cList; 
        }

        #region DB Process
        public DataTable GetStockNames()
        {
            SQL = "select ID, Description from Stock where deleted = 0";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetStockNames));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetStockNames), dt.Rows.Count.ToString());
            return dt;
        }
        #endregion
    }
}
