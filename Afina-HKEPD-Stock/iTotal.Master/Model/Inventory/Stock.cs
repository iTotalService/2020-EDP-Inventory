using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace iTotal.Master.Model
{
    public class Stock : BaseClass
    {
        public string BarCode { get; set; }
        public string Description { get; set; }
        public string Section { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        

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
                Section = request.Section,
                Location = request.Location,
                Status = request.Status,
                Remarks = request.Remarks,
                CreateDate = DateTime.Now
            };

        public static IQueryable<Stock> GetItems(this InvContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.StockMaster.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<Stock> GetbyIDAsync(this InvContext dbContext, Stock entity)
            => await dbContext.StockMaster.FirstOrDefaultAsync(item => item.ID == entity.ID);
        
        public static async Task<Stock> GetByNameAsync(this InvContext dbContext, Stock entity)
            => await dbContext.StockMaster.FirstOrDefaultAsync(item => item.Description == entity.Description);
    }

    #endregion

    #region Request Class
    public class DeleteCmpyRequest
    {
        public long ID { get; set; }

        public string User { get; set; }
    }

    public class PostStockRequest
    {
        [Required]
        [StringLength(20)]
        public string BarCode { get; set; }

        [StringLength(60)]
        public string Description { get; set; }
        [StringLength(30)]
        public string Section { get; set; }
        [StringLength(30)]
        public string Location { get; set; }
        [StringLength(30)]
        public string Status { get; set; }
        [StringLength(60)]
        public string Remarks { get; set; }
    }
    public class PutStockRequest
    {
        public long ID { get; set; }

        [Required]
        [StringLength(20)]
        public string BarCode { get; set; }

        [StringLength(60)]
        public string Description { get; set; }
        [StringLength(30)]
        public string Section { get; set; }
        [StringLength(30)]
        public string Location { get; set; }
        [StringLength(30)]
        public string Status { get; set; }
        [StringLength(60)]
        public string Remarks { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
    }

    #endregion
}
