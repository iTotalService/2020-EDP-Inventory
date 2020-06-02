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
    public class StockTXServices : BaseServices
    {
        

        public StockTXServices()
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

        public IEnumerable<StockTX> getStockTXs()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getStockTXs));
            List<StockTX> DeptList = new List<StockTX>();
            DataTable dt = GetStockTXRecords();
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                DeptList.Add(new StockTX()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getStockTXs));
            return DeptList; 
        }

        #region DB Process
        public DataTable GetStockTXRecords()
        {
            SQL = "select * from StockTX where deleted = 0";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetStockTXRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetStockTXRecords), dt.Rows.Count.ToString());
            return dt;
        }
        #endregion
    }
}
