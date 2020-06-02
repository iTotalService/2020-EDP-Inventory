using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class Leav : BaseClass
    {
        public string LE_CODE { get; set; }
        public string LE_DESC { get; set; }
        public decimal LE_FACTOR { get; set; }
        public Leav()
        {
        }

        public Leav(long id)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class LeavDbContextExtensions
    {

        #region Devlop
        private static string dev_user = "DEV_USER";
        #endregion

        public static Leav ToEntity(this PostLeavRequest request)
            => new Leav
            {
                LE_CODE = request.LE_CODE,
                LE_DESC = request.LE_DESC,
                LE_FACTOR = request.LE_FACTOR,
                CR_USR = request.CR_USER == null ? dev_user : request.CR_USER
            };

    public static IQueryable<Leav> GetLeaves(this HRCMContext dbContext)
        {
            // Get query from DbSet
            var query = dbContext.Leav.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            return query;
        }

        public static async Task<Leav> GetbyIDAsync(this HRCMContext dbContext, Leav entity)
            => await dbContext.Leav.FirstOrDefaultAsync(item => item.ID == entity.ID);

        public static async Task<Leav> GetByNameAsync(this HRCMContext dbContext, Leav entity)
            => await dbContext.Leav.FirstOrDefaultAsync(item => item.LE_CODE == entity.LE_CODE);
    }

    #endregion

    #region Request Class
    

    public class PostLeavRequest : BaseRequest
    {
        [Required]
        [StringLength(40)]
        public string LE_CODE { get; set; }

        [Required]
        [StringLength(200)]
        public string LE_DESC { get; set; }
        public int LE_FACTOR { get; set; }
    }
    public class PutLeavRequest : BaseRequest
    {
        [Required]
        [StringLength(40)]
        public string LE_CODE { get; set; }
        [Required]
        [StringLength(200)]
        public string LE_DESC { get; set; }
        public int LE_FACTOR { get; set; }
    }

    #endregion
}
