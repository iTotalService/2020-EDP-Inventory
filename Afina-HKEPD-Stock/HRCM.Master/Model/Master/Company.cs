using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class CMPY : BaseClass
    {
        public string CO_CODE { get; set; }
        public string CO_NAME { get; set; }
        public string CO_ADDRESS { get; set; }
        public string CO_DESIG { get; set; }
        public string CO_SNAME { get; set; }
        public string CO_TAXNO { get; set; }
        public string CO_LOGO { get; set; }
        public string CO_MPF { get; set; }
        public string CO_BANKAC { get; set; }
        public string CO_COMPANY { get; set; }

        public CMPY()
        {
        }

        public CMPY(long id, string UserID)
        {
            ID = id;
            CR_USR = UserID;
        }

    }

    #region Extensions
    public static class CompanyDbContextExtensions
    {
        public static CMPY ToEntity(this PostCmpyRequest request)
            => new CMPY
            {
                CO_CODE = request.CO_CODE,
                CO_NAME = request.CO_NAME,
                CO_ADDRESS = request.CO_ADDRESS,
                CO_DESIG = request.CO_DESIG,
                CO_SNAME = request.CO_SNAME,
                CO_TAXNO = request.CO_TAXNO,
                CO_LOGO = request.CO_LOGO,
                CO_MPF = request.CO_MPF,
                CO_BANKAC = request.CO_BANKAC,
                CO_COMPANY = request.CO_COMPANY,
                CR_USR = request.CR_USER,
                UP_USR = request.UP_USER,
                DEL_USR = request.DEL_USER,
                CR_DATE = DateTime.Now
            };

        public static IQueryable<CMPY> GetCompanys(this HRCMContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.Cmpy.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<CMPY> GetbyIDAsync(this HRCMContext dbContext, CMPY entity)
            => await dbContext.Cmpy.FirstOrDefaultAsync(item => item.ID == entity.ID);
        
        public static async Task<CMPY> GetByNameAsync(this HRCMContext dbContext, CMPY entity)
            => await dbContext.Cmpy.FirstOrDefaultAsync(item => item.CO_NAME == entity.CO_NAME);
    }

    #endregion

    #region Request Class
    public class DeleteCmpyRequest
    {
        public Int64 ID { get; set; }

        public string User { get; set; }
    }

    public class PostCmpyRequest
    {
        //public Int64 ID { get; set; }
        [Required]
        [StringLength(2)]
        public string CO_CODE { get; set; }

        [StringLength(50)]
        public string CO_NAME { get; set; }
        [StringLength(100)]
        public string CO_ADDRESS { get; set; }
        [StringLength(50)]
        public string CO_DESIG { get; set; }
        [StringLength(20)]
        public string CO_TAXNO { get; set; }
        [StringLength(100)]
        public string CO_LOGO { get; set; }
        [StringLength(20)]
        public string CO_MPF { get; set; }
        [StringLength(20)]
        public string CO_BANKAC { get; set; }
        [StringLength(10)]
        public string CO_COMPANY { get; set; }
        [StringLength(50)]
        public string CO_SNAME { get; set; }
        [StringLength(40)]
        public string CR_USER { get; set; }
        [StringLength(40)]
        public string UP_USER { get; set; }
        [StringLength(40)]
        public string DEL_USER { get; set; }
    }
    public class PutCmpyRequest
    {
        public Int64 ID { get; set; }

        [Required]
        [StringLength(2)]
        public string CO_CODE { get; set; }

        [StringLength(50)]
        public string CO_NAME { get; set; }
        [StringLength(100)]
        public string CO_ADDRESS { get; set; }
        [StringLength(50)]
        public string CO_DESIG { get; set; }
        [StringLength(20)]
        public string CO_TAXNO { get; set; }
        [StringLength(100)]
        public string CO_LOGO { get; set; }
        [StringLength(20)]
        public string CO_MPF { get; set; }
        [StringLength(20)]
        public string CO_BANKAC { get; set; }
        [StringLength(10)]
        public string CO_COMPANY { get; set; }
        [StringLength(50)]
        public string CO_SNAME { get; set; }
        [StringLength(40)]
        public string UP_USER { get; set; }

        public DateTime? UP_DATE { get; set; }
    }

    #endregion
}
