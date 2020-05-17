using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iTotal.Master.Model
{
    public class StockTotal2 : Stock
    {
        public long? Quantity { get; set; }
        public string Section { get; set; }
        public string Location { get; set; }

    }
    public class StockTotal : Stock
    {
        public long? Quantity { get; set; }
        public long? Section { get; set; }
        public long? Location { get; set; }

    }
    #region Extensions
    public static class StockTotalDbContextExtensions
    {
        public static long GetStockBalance(this InvContext dbContext, long stockID = 0, long location = 0, long section = 0)
        {
            var txquery = (from tx in dbContext.StockTX
                           where tx.StockID == stockID && tx.Section == section && tx.Location == location
                           group tx by tx.StockID into n
                           select new { StockID = n.Key, Quantity = n.Sum(o => o.Quantity) }
                           );

            var tmp = txquery.FirstOrDefault();
            if (tmp == null) return 0;
            long ttl = txquery.FirstOrDefault().Quantity;
            return ttl;
        }

        public static IQueryable<StockTotal> GetItemswithQty(this InvContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            var txquery = (from tx in dbContext.StockTX
                           group tx by new
                           {
                                tx.StockID,
                                tx.Location,
                                tx.Section,
                            } into n
                           select new { 
                               StockID = n.Key.StockID, 
                               Location = n.Key.Location, 
                               Section = n.Key.Section,
                               Quantity = n.Sum(o => o.Quantity) 
                           }
                           );

            var query = (from s in dbContext.StockMaster
                         join tx in txquery on s.ID equals tx.StockID //into tmpTX
                         //from tx in tmpTX.DefaultIfEmpty()
                         select new StockTotal()
                         {
                             Deleted = s.Deleted,
                             ID = s.ID,
                             Description = s.Description,
                             Section = tx.Section,
                             BarCode = s.BarCode,
                             Status = s.Status,
                             Location = tx.Location,
                             Quantity = tx.Quantity
                         });

            // filter out all deleted records
            query = query.Where(item => item.Deleted2 == 0);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
            
        }

        public static IQueryable<StockTotal> GetAllItemswithQty(this InvContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            var txquery = (from tx in dbContext.StockTX
                           group tx by new
                           {
                               tx.StockID,
                               tx.Location,
                               tx.Section,
                           } into n
                           select new
                           {
                               StockID = n.Key.StockID,
                               Location = n.Key.Location,
                               Section = n.Key.Section,
                               Quantity = n.Sum(o => o.Quantity)
                           }
                           );

            var query = (from s in dbContext.StockMaster
                        select new StockTotal()
                        {
                            Deleted = s.Deleted,
                            ID = s.ID,
                            Description = s.Description,
                            //Section = s.Section,
                            BarCode = s.BarCode,
                            Status = s.Status,
                            //Location = s.Location,
                            Quantity = 0
                        });
            query = query.Where(item => item.Deleted2 == 0);

            foreach ( var rec in query )
            {
                foreach (var txRec in txquery )
                {
                    if (( rec.ID == txRec.StockID) && ( rec.Location == txRec.Location ) && ( rec.Section == txRec.Section))
                    {
                        rec.Quantity = txRec.Quantity;
                    }
                }
            }

            return query;

        }
    }
    #endregion
}
