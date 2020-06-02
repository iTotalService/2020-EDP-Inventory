using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class PosiProfile : BaseClass
    {
        public long CustomerID { get; set; }
        public long PosiID { get; set; }
        public decimal SalaryRate { get; set; }

        public PosiProfile()
        {
        }

        public PosiProfile(long id)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class PosiProfileDbContextExtensions
    {

        public static PosiProfile ToEntity(this PostPosiProfileRequest request)
            => new PosiProfile
            {
                CustomerID = request.CustomerID,
                PosiID = request.PosiID,
                SalaryRate = request.SalaryRate,
                CR_USR = request.CR_USER
            };

        public static IQueryable<PosiProfile> GetPosiProfiles(this HRCMContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.PosiProfile.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<PosiProfile> GetbyIDAsync(this HRCMContext dbContext, PosiProfile entity)
            => await dbContext.PosiProfile.FirstOrDefaultAsync(item => item.ID == entity.ID);

        public static async Task<PosiProfile> GetByNameAsync(this HRCMContext dbContext, PosiProfile entity)
            => await dbContext.PosiProfile.FirstOrDefaultAsync(item => item.PosiID == entity.PosiID);
    }

    #endregion

    #region Request Class
    public class PostPosiProfileRequest : BaseRequest
    {
        [Required]
        public long CustomerID { get; set; }
        [Required]
        public long PosiID { get; set; }
        [Required]
        public decimal SalaryRate { get; set; }

    }
    public class PutPosiProfileRequest :BaseRequest
    {
        [Required]
        public long CustomerID { get; set; }
        [Required]
        public long PosiID { get; set; }
        [Required]
        public decimal SalaryRate { get; set; }
    }

    #endregion
}
