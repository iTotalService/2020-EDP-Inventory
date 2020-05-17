using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace iTotal.Master.Model
{
    public class mStock : Stock
    {
        public long? ServerQty { get; set; }
    }
    public class Stock : BaseClass
    {
        public string BarCode { get; set; }
        public string Description { get; set; }
        //public long? Section { get; set; }
        //public long? Location { get; set; }
        public long Status { get; set; }
        public string Remarks { get; set; }
        public Int32 MinQty { get; set; }
        public Int32 MaxQty { get; set; }

        public Stock()
        {
        }

        public Stock(long id, string UserID)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class StockDbContextExtensions
    {
        public static Stock ToEntity(this PostStockRequest request)
            => new Stock
            {
                BarCode = request.BarCode,
                Description = request.Description,
                //Section = request.Section,
                //Location = request.Location,
                Status = request.Status,
                Remarks = request.Remarks,
                MinQty = request.MinQty,
                MaxQty =request.MaxQty,
                Deleted2 = 0,
                CreateDate = DateTime.Now
            };

        public static IQueryable<Stock> GetItems(this InvContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.StockMaster.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.Deleted2 == 0);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<Stock> GetbyIDAsync(this InvContext dbContext, Stock entity)
            => await dbContext.StockMaster.FirstOrDefaultAsync(item => item.ID == entity.ID && item.Deleted2 == 0);
        
        public static async Task<Stock> GetByNameAsync(this InvContext dbContext, Stock entity)
            => await dbContext.StockMaster.FirstOrDefaultAsync(item => item.Description == entity.Description && item.Deleted2 == 0);

        public static async Task<Stock> GetByBarcodeAsync(this InvContext dbContext, Stock entity)
            => await dbContext.StockMaster.FirstOrDefaultAsync(item => item.BarCode == entity.BarCode && item.Deleted2 == 0);
    }

    #endregion

    #region Request Class
    public class DeleteStockRequest
    {
        public long ID { get; set; }

        public string User { get; set; }
    }

    public class PostStockListRequest
    {
        public List<PostStockRequest> Result { get; set; }
    }

    public class StockErrorcsv
    {
        [Required]
        [StringLength(20)]
        public string BarCode { get; set; }
        public string Status { get; set; }
        [StringLength(60)]
        public string ErrorMsg { get; set; }

    }
    public class PostStockRequest
    {
        [Required]
        [StringLength(20)]
        public string BarCode { get; set; }
        public string Description { get; set; }
        public long Section { get; set; }
        public long Location { get; set; }
        public long Status { get; set; }
        [StringLength(60)]
        public string Remarks { get; set; }
        public Int32 MinQty { get; set; }
        public Int32 MaxQty { get; set; }

    }
    public class PutStockRequest
    {
        public long ID { get; set; }

        [Required]
        [StringLength(20)]
        public string BarCode { get; set; }

        [StringLength(60)]
        public string Description { get; set; }
        public long Section { get; set; }
        public long Location { get; set; }
        public long Status { get; set; }
        [StringLength(60)]
        public string Remarks { get; set; }
        public Int32 MinQty { get; set; }
        public Int32 MaxQty { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
    }

    #endregion
}
