using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class Grad : BaseClass
    {
        public string GR_CODE { get; set; }
        public string GR_DESC { get; set; }

        public Grad()
        {
        }

        public Grad(long id)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class GradDbContextExtensions
    {
        public static Grad ToEntity(this PostGradRequest request)
            => new Grad
            {
                GR_CODE = request.GR_CODE,
                GR_DESC = request.GR_DESC,
                CR_USR = request.CR_USER
            };

    public static IQueryable<Grad> GetGradings(this HRCMContext dbContext)
        {
            // Get query from DbSet
            var query = dbContext.Grad.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            return query;
        }

        public static async Task<Grad> GetbyIDAsync(this HRCMContext dbContext, Grad entity)
            => await dbContext.Grad.FirstOrDefaultAsync(item => item.ID == entity.ID);

        public static async Task<Grad> GetByNameAsync(this HRCMContext dbContext, Grad entity)
            => await dbContext.Grad.FirstOrDefaultAsync(item => item.GR_CODE == entity.GR_CODE);
    }

    #endregion

    #region Request Class
    

    public class PostGradRequest : BaseRequest
    {
        [Required]
        [StringLength(40)]
        public string GR_CODE { get; set; }

        [Required]
        [StringLength(200)]
        public string GR_DESC { get; set; }

    }
    public class PutGradRequest : BaseRequest
    {
        [Required]
        [StringLength(40)]
        public string GR_CODE { get; set; }
        [Required]
        [StringLength(200)]
        public string GR_DESC { get; set; }
    }

    #endregion
}
