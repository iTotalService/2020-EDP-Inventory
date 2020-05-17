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

        public IEnumerable<Stock> getRecords()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRecords));
            List<Stock> sList = new List<Stock>();
            DataTable dt = GetDBRecords();
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                sList.Add(new Stock()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    BarCode = dr["BarCode"].ToString(),
                    Description = dr["Description"].ToString(),
                    Status = Convert.ToInt64(dr["Status"]),
                    Remarks = dr["Remarks"].ToString(),
                    MinQty = Convert.ToInt32(dr["MinQty"]),
                    MaxQty = Convert.ToInt32(dr["MaxQty"]),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                    LastUpdatedDate = string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()) ? tmp : Convert.ToDateTime(dr["LastUpdatedDate"])
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getRecords));
            return sList;
        }
        public Stock getRecords(long ID)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRecords));
            Stock sList = new Stock();
            DataTable dt = GetDBRecords(ID);
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                sList = new Stock()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    BarCode = dr["BarCode"].ToString(),
                    Description = dr["Description"].ToString(),
                    Status = Convert.ToInt64(dr["Status"]),
                    Remarks = dr["Remarks"].ToString(),
                    MinQty = Convert.ToInt32(dr["MinQty"]),
                    MaxQty = Convert.ToInt32(dr["MaxQty"]),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                    LastUpdatedDate = string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()) ? tmp : Convert.ToDateTime(dr["LastUpdatedDate"])
                };
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getRecords));
            return sList;
        }
        public Stock getRecords(string code)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getRecords));
            Stock sList = new Stock();
            DataTable dt = GetDBRecords(code);
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                sList = new Stock()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    BarCode = dr["BarCode"].ToString(),
                    Description = dr["Description"].ToString(),
                    Status = Convert.ToInt64(dr["Status"]),
                    Remarks = dr["Remarks"].ToString(),
                    MinQty = Convert.ToInt32(dr["MinQty"]),
                    MaxQty = Convert.ToInt32(dr["MaxQty"]),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                    LastUpdatedDate = string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()) ? tmp : Convert.ToDateTime(dr["LastUpdatedDate"])
                };
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getRecords));
            return sList;
        }

        public IEnumerable<StockTotal2> getStockTotal()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getStockTotal));
            List<StockTotal2> sList = new List<StockTotal2>();
            DataTable dt = GetTotalRecords();
            foreach (DataRow dr in dt.Rows)
            {
                StockTotal2 stock = new StockTotal2();
                stock.ID = Convert.ToInt64(dr["ID"]);
                stock.BarCode = dr["BarCode"].ToString();
                stock.Description = dr["Description"].ToString();
                stock.Quantity = Convert.ToInt32(dr["Quantity"]);
                stock.MinQty = Convert.ToInt32(dr["MinQty"]);
                stock.MaxQty = Convert.ToInt32(dr["MaxQty"]);
                stock.Location = dr["Location"].ToString();
                stock.Section = dr["Section"].ToString();

                sList.Add(stock);
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getStockTotal));
            return sList;
        }

        public IEnumerable<mStock> getStockServerTotal()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getStockServerTotal));
            List<mStock> sList = new List<mStock>();
            DataTable dt = GetStockTotalRecords();
            foreach (DataRow dr in dt.Rows)
            {
                mStock stock = new mStock();
                stock.ID = Convert.ToInt64(dr["ID"]);
                stock.BarCode = dr["BarCode"].ToString();
                stock.Description = dr["Description"].ToString();
                stock.ServerQty = Convert.ToInt32(dr["Quantity"]);
                stock.MinQty = Convert.ToInt32(dr["MinQty"]);
                stock.MaxQty = Convert.ToInt32(dr["MaxQty"]);

                sList.Add(stock);
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getStockServerTotal));
            return sList;
        }

        #region DB Process
        public DataTable GetStockNames()
        {
            SQL = "select ID, Description from StockMaster where Deleted2 = 0";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetStockNames));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetStockNames), dt.Rows.Count.ToString());
            return dt;
        }

        public DataTable GetStockTotalRecords()
        {
            SQL = "select sm.ID, Barcode, sm.Description, minQty,MaxQty, isnull(sum(tx.Quantity),0) Quantity ";
            SQL += "from StockMaster sm ";
            SQL += "left outer join StockTX tx on sm.id = tx.StockID  and tx.Deleted2 = 0 ";
            //SQL += "join location l on l.id = tx.location and l.Deleted2 = 0";
            //SQL += "join section s on s.id = tx.section and s.Deleted2 = 0";
            SQL += "where sm.deleted2 = 0";
            SQL += "group by Barcode, sm.Description, sm.id,minQty,MaxQty";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetStockTotalRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetStockTotalRecords), dt.Rows.Count.ToString());
            return dt;
        }

        public DataTable GetTotalRecords()
        {
            SQL = "select sm.ID, Barcode, sm.Description, l.Description location, s.Description section, minQty,MaxQty, isnull(sum(tx.Quantity),0) Quantity ";
            SQL += "from StockMaster sm ";
            SQL += "left outer join StockTX tx on sm.id = tx.StockID  and tx.Deleted2 = 0 ";
            SQL += "join location l on l.id = tx.location and l.Deleted2 = 0";
            SQL += "join section s on s.id = tx.section and s.Deleted2 = 0";
            SQL += "where sm.deleted2 = 0";
            SQL += "group by Barcode, sm.Description,l.Description, s.Description,sm.id,minQty,MaxQty";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetTotalRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetTotalRecords), dt.Rows.Count.ToString());
            return dt;
        }

        public DataTable GetDBRecords()
        {
            SQL = "select * from StockMaster where deleted2 = 0";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecords), dt.Rows.Count.ToString());
            return dt;
        }
        public DataTable GetDBRecords(long ID)
        {
            SQL = "select * from StockMaster where deleted2 = 0 and ID = " + ID.ToString();
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecords), dt.Rows.Count.ToString());
            return dt;
        }
        public DataTable GetDBRecords(string code)
        {
            SQL = "select * from StockMaster where deleted2 = 0 and barCode = " + code.ToString();
            Logger.Debug("'{0}' has been invoked and start", nameof(GetDBRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetDBRecords), dt.Rows.Count.ToString());
            return dt;
        }
        public void Update(Stock record)
        {
            SQL = "update StockMaster set";
            SQL += " Barcode = @Barcode, ";
            SQL += " Description = @Description, ";
            SQL += " Status = @Status, ";
            SQL += " Remarks = @Remarks, ";
            SQL += " MinQty = @MinQty, ";
            SQL += " MaxQty = @MaxQty, ";
            SQL += " LastUpdatedDate = getdate()";
            SQL += " where ID = @ID ";

            Logger.Debug("'{0}' has been invoked and start", nameof(Update));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = record.ID;
            cmd.Parameters.Add("@Barcode", SqlDbType.NVarChar, 20).Value = record.BarCode;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 60).Value = record.Description;
            cmd.Parameters.Add("@Status", SqlDbType.BigInt).Value = record.Status;
            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 60).Value = record.Remarks;
            cmd.Parameters.Add("@MinQty", SqlDbType.Int).Value = record.MinQty;
            cmd.Parameters.Add("@MaxQty", SqlDbType.Int).Value = record.MaxQty;


            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Update));

        }
        public void Insert(Stock record)
        {
            SQL = "insert into StockMaster (";
            SQL += " Barcode,Description, Status, Remarks, MinQty, MaxQty, Deleted2 ";
            SQL += " ) values (";
            SQL += " @Barcode,@Description, @Status, @Remarks, @MinQty, @MaxQty, 0 ";
            SQL += ")";

            Logger.Debug("'{0}' has been invoked and start", nameof(Insert));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@Barcode", SqlDbType.NVarChar, 20).Value = record.BarCode;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 60).Value = record.Description;
            cmd.Parameters.Add("@Status", SqlDbType.BigInt).Value = record.Status;
            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 60).Value = record.Remarks;
            cmd.Parameters.Add("@MinQty", SqlDbType.Int).Value = record.MinQty;
            cmd.Parameters.Add("@MaxQty", SqlDbType.Int).Value = record.MaxQty;

            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Insert));

        }

        public void Delete(string key)
        {
            SQL = "update StockMaster set";
            SQL += " DELETED2 = 1, ";
            SQL += " DeletedDate = getdate()";
            SQL += " where ID = @ID ";

            Logger.Debug("'{0}' has been invoked and start", nameof(Delete));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = key;


            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Delete));
        }

        public bool chkRecord(Stock record)
        {
            SQL = "select * from StockMaster " +
                "where BarCode = '" + record.BarCode + "' and DELETED2 = 0";
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(Update), dt.Rows.Count.ToString());
            return dt.Rows.Count > 0;
        }
        #endregion
    }
}
