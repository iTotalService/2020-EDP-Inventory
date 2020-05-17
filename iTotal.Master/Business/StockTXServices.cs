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

        public List<vStockTXDisplay2> getDisplayStockTX(long stockID)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getDisplayStockTX));
            List<vStockTXDisplay2> cList = new List<vStockTXDisplay2>();
            DataTable dt = GetStockTXRecords(stockID);
            foreach (DataRow dr in dt.Rows)
            {
                cList.Add(new vStockTXDisplay2()
                {
                    StockID = Convert.ToInt64(dr["StockID"]),
                    Quantity = Convert.ToInt64(dr["Quantity"]),
                    Section = dr["Section"].ToString(),
                    Location = dr["Location"].ToString(),
                    Remarks = dr["Remarks"].ToString(),
                    Description = dr["Description"].ToString(),
                    Designation = dr["Designation"].ToString(),
                    BarCode = dr["BarCode"].ToString(),
                    CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getDisplayStockTX));
            return cList;
        }

        public IEnumerable<StockTX> getStockTXs()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getStockTXs));
            List<StockTX> DeptList = new List<StockTX>();
            DataTable dt = GetStockTXRecords(0);
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
        public DataTable GetStockTXRecords(long stockID)
        {
            SQL = "select StockID, Quantity, l.Description Location, s.Description Section, x.Remarks, Designation, sm.Description, sm.BarCode, x.CreateDate" +
                " from StockTX x join stockmaster st on st.id = x.stockid " +
                " join Stockmaster sm on x.stockID = sm.ID" +
                " join Location l on l.ID = x.location and l.deleted2 = 0" +
                " join Section s on s.ID = x.Section and s.deleted2 = 0" +
                " where x.deleted2 = 0 and st.deleted2= 0";
            if (stockID > 0) SQL += " and x.stockid = " + stockID.ToString();
            Logger.Debug("'{0}' has been invoked and start", nameof(GetStockTXRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetStockTXRecords), dt.Rows.Count.ToString());
            return dt;
        }

        public void Insert(StockTX record)
        {
            SQL = "insert into StockTX (";
            SQL += " UploadedDate,Location, Section, UploadedID, CreateDate, Designation ";
            SQL += " ) values (";
            SQL += " getdate(),@Location, @Section, @UploadedID, @CreateDate, @Designation";
            SQL += ")";

            Logger.Debug("'{0}' has been invoked and start", nameof(Insert));
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@Location", SqlDbType.NVarChar, 20).Value = record.Location;
            cmd.Parameters.Add("@Section", SqlDbType.NVarChar, 60).Value = record.Section;
            cmd.Parameters.Add("@UploadedID", SqlDbType.BigInt).Value = record.UploadedID;
            cmd.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = record.CreateDate;
            cmd.Parameters.Add("@Designation", SqlDbType.NVarChar, 60).Value = record.Designation;
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = record.Quantity;

            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(Insert));

        }

        #endregion    
    }
}
