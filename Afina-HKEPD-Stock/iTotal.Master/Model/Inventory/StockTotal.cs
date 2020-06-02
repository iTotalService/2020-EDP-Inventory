using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iTotal.Master.Model
{
    public class StockTotal : Stock
    {
        public long Quantity { get; set; }
    }
    #region Extensions
    public static class StockTotalDbContextExtensions
    {
        public static long GetStockBalance(this InvContext dbContext, long stockID = 0)
        {
            var txquery = (from tx in dbContext.StockTX
                           where tx.StockID == stockID
                           group tx by tx.StockID into n
                           select new { StockID = n.Key, Quantity = n.Sum(o => o.Quantity) }
                           );

            var tmp = txquery.FirstOrDefault();
            long ttl = txquery.FirstOrDefault().Quantity;
            return ttl;
        }

        public static IQueryable<StockTotal> GetItemswithQty(this InvContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            var txquery = (from tx in dbContext.StockTX
                           group tx by tx.StockID into n
                           select new { StockID = n.Key, Quantity = n.Sum(o => o.Quantity) }
                           );

            var query = (from s in dbContext.StockMaster
                         join tx in txquery on s.ID equals tx.StockID //into tmpTX
                         //from tx in tmpTX.DefaultIfEmpty()
                         select new StockTotal()
                         {
                             DELETED = s.DELETED,
                             ID = s.ID,
                             Description = s.Description,
                             BarCode = s.BarCode,
                             Status = s.Status,
                             Location = s.Location,
                             Quantity = tx.Quantity
                         });

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
            
        }

    }
    #endregion
}
