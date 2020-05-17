using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace iTotal.Master.Model
{
    public class Status : BaseClass
    {
        public string StatusCode { get; set; }
        public string Description { get; set; }
 
        public Status()
        {
        }

        public Status(long id, string UserID)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class StatusDbContextExtensions
    {
        public static Status ToEntity(this PostStatusRequest request)
            => new Status
            {
                StatusCode = request.StatusCode,
                Description = request.Description,
                Deleted2 = 0,
                CreateDate = DateTime.Now
            };

        public static IQueryable<Status> GetStatuss(this InvContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.StockStatus.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.Deleted2 == 0);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<Status> GetbyIDAsync(this InvContext dbContext, Status entity)
            => await dbContext.StockStatus.FirstOrDefaultAsync(item => item.ID == entity.ID);
        
        public static async Task<Status> GetByNameAsync(this InvContext dbContext, Status entity)
            => await dbContext.StockStatus.FirstOrDefaultAsync(item => item.Description == entity.Description);
    }

    #endregion

    #region Request Class
    public class DeleteStatusRequest
    {
        public long ID { get; set; }

        public string User { get; set; }
    }

    public class PostStatusRequest
    {
        [StringLength(20)]
        public string StatusCode { get; set; }

        [Required]
        [StringLength(60)]
        public string Description { get; set; }
    }
    public class PutStatusRequest
    {
        public long ID { get; set; }

        [StringLength(20)]
        public string StatusCode { get; set; }

        [Required]
        [StringLength(60)]
        public string Description { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }

    #endregion
}
