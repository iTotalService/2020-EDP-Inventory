using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class Posi : BaseClass
    {
        public string PO_CODE { get; set; }
        public string PO_DESC { get; set; }

        public Posi()
        {
        }

        public Posi(long id)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class PosiDbContextExtensions
    {

        #region Devlop
        private static string dev_user = "DEV_USER";
        #endregion

        public static Posi ToEntity(this PostPosiRequest request)
            => new Posi
            {
                PO_CODE = request.PO_CODE,
                PO_DESC = request.PO_DESC,
                CR_USR = request.CR_USER == null ? dev_user : request.CR_USER
            };

        public static IQueryable<Posi> GetPositions(this HRCMContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.Posi.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<Posi> GetbyIDAsync(this HRCMContext dbContext, Posi entity)
            => await dbContext.Posi.FirstOrDefaultAsync(item => item.ID == entity.ID);

        public static async Task<Posi> GetByNameAsync(this HRCMContext dbContext, Posi entity)
            => await dbContext.Posi.FirstOrDefaultAsync(item => item.PO_CODE == entity.PO_CODE);
    }

    #endregion

    #region Request Class
    public class PostPosiRequest : BaseRequest
    {
        [Required]
        [StringLength(40)]
        public string PO_CODE { get; set; }

        [Required]
        [StringLength(200)]
        public string PO_DESC { get; set; }

    }
    public class PutPosiRequest :BaseRequest
    {
        [Required]
        [StringLength(40)]
        public string PO_CODE { get; set; }
        [Required]
        [StringLength(200)]
        public string PO_DESC { get; set; }
    }

    #endregion
}
