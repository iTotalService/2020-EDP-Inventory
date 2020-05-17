using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace iTotal.Master.Model
{
    public class Location : BaseClass
    {
        public string LocationCode { get; set; }
        public string Description { get; set; }

        public Location()
        {
        }

        public Location(long id, string UserID)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class LocationDbContextExtensions
    {
        public static Location ToEntity(this PostLocationRequest request)
            => new Location
            {
                LocationCode = request.LocationCode,
                Description = request.Description,
                Deleted2 = 0,
                CreateDate = DateTime.Now
            };

        public static IQueryable<Location> GetLocations(this InvContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.Location.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.Deleted2 == 0);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<Location> GetbyIDAsync(this InvContext dbContext, Location entity)
            => await dbContext.Location.FirstOrDefaultAsync(item => item.ID == entity.ID);
        
        public static async Task<Location> GetByNameAsync(this InvContext dbContext, Location entity)
            => await dbContext.Location.FirstOrDefaultAsync(item => item.Description == entity.Description);
    }

    #endregion

    #region Request Class
    public class DeleteLocationRequest
    {
        public long ID { get; set; }

        public string User { get; set; }
    }

    public class PostLocationRequest
    {
        [Required]
        [StringLength(20)]
        public string LocationCode { get; set; }

        [StringLength(60)]
        public string Description { get; set; }
    }
    public class PutLocationRequest
    {
        public long ID { get; set; }

        [Required]
        [StringLength(20)]
        public string LocationCode { get; set; }

        [StringLength(60)]
        public string Description { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }

    #endregion
}
