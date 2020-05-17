using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class Location : BaseClass
    {
        public string LocationCode { get; set; }
        public string Description { get; set; }
  

        public Location()
        {
        }

        public Location(long id)
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
                CR_USR = request.CR_USER ,
                CR_DATE = DateTime.Now
            };

        public static IQueryable<Location> GetLocations(this HRCMContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.Location.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<Location> GetbyIDAsync(this HRCMContext dbContext, Location entity)
            => await dbContext.Location.FirstOrDefaultAsync(item => item.ID == entity.ID);
        
        public static async Task<Location> GetByNameAsync(this HRCMContext dbContext, Location entity)
            => await dbContext.Location.FirstOrDefaultAsync(item => item.Description == entity.Description);
    }

    #endregion

    #region Request Class
    public class DeleteLocationRequest
    {
        public Int64 ID { get; set; }

        public string User { get; set; }
    }

    public class PostLocationRequest
    {
        [Required]
        [StringLength(4)]
        public string LocationCode { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public string CR_USER { get; set; }
    }
    public class PutLocationRequest
    {
        public Int64 ID { get; set; }
        [Required]
        [StringLength(4)]
        public string LocationCode { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [StringLength(40)]
        public string UP_USER { get; set; }

        public DateTime? UP_DATE { get; set; }
    }

    #endregion
}
