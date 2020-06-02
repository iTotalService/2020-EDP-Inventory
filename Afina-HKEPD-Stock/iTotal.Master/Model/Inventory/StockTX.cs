using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace iTotal.Master.Model
{
    public class StockTX : BaseClass
    {
        public long StockID { get; set; }
        public long Quantity { get; set; }
        public string Designation { get; set; }


        public string Location { get; set; }

        public long? UploadedID { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string Remarks { get; set; }

        public StockTX()
        {
        }

        public StockTX(long id, string UserID)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class StockTXDbContextExtensions
    {
        public static StockTX ToEntity(this PostStockTXRequest request)
            => new StockTX
            {
                StockID = request.StockID,
                Quantity = request.Quantity,
                Designation = request.Designation,
                CreateDate = DateTime.Now
            };
        public static IQueryable<StockTX> GetTXs(this InvContext dbContext, long stockID = 0, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.StockTX.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false && item.StockID == stockID).OrderByDescending(x => x.CreateDate);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<StockTX> GetbyTXIDAsync(this InvContext dbContext, StockTX entity)
            => await dbContext.StockTX.FirstOrDefaultAsync(item => item.ID == entity.ID);

        //public static async Task<StockTX> GetByNameAsync(this InvContext dbContext, Stock entity)
        //    => await dbContext.StockMaster.FirstOrDefaultAsync(item => item.Stock == entity.Description);
    }

    #endregion

    #region Request Class
    public class DeleteStockTXRequest
    {
        public long ID { get; set; }

        public string User { get; set; }
    }

    public class PostStockTXListRequest
    {
        public List<PostStockTXRequest> Result { get; set; }
    }
    public class PostStockTXRequest
    {
        [Required]
        public long StockID { get; set; }

        [Required]
        public long Quantity { get; set; }
        [StringLength(20)]
        public string Designation { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? Id { get; set; }
        public string Location { get; set; }
    }
    public class PutStockTXRequest
    {
        public long ID { get; set; }

        [Required]
        public long StockID { get; set; }

        [Required]
        public long Quantity { get; set; }
        [StringLength(20)]
        public string Designation { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }

    #endregion
}
