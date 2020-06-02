using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class Dept : BaseClass
    {
        public string DE_CODE { get; set; }
        public string DE_DESC { get; set; }

        //public string deCODE { get; set; }
        //public string deDESC { get; set; }


        public Dept()
        {
        }

        public Dept(long id)
        {
            ID = id;
        }

        public Dept(long id, string UserID)
        {
            ID = id;
            CR_USR = UserID;
        }

    }

    #region Extensions
    public static class DeptDbContextExtensions
    {
        #region Devlop
        private static string dev_user = "DEV_USER";
        #endregion

        public static Dept ToEntity(this PostDeptRequest request)
            => new Dept
            {
                DE_CODE = request.DE_CODE,
                DE_DESC = request.DE_DESC,
                CR_USR = request.CR_USER == null ? dev_user : request.CR_USER,
                CR_DATE = DateTime.Now
            };

        public static IQueryable<Dept> GetRecords(this HRCMContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.Dept.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<Dept> GetbyIDAsync(this HRCMContext dbContext, Dept entity)
            => await dbContext.Dept.FirstOrDefaultAsync(item => item.ID == entity.ID);

        public static async Task<Dept> GetByDeptNameAsync(this HRCMContext dbContext, Dept entity)
            => await dbContext.Dept.FirstOrDefaultAsync(item => item.DE_DESC == entity.DE_DESC);
    }

    #endregion

    #region Request Class
    public class DeleteDeptRequest
    {
        public Int64 ID { get; set; }

        public string User { get; set; }
    }

    public class PostDeptRequest
    {
        //public Int64 ID { get; set; }
        [Required]
        [StringLength(40)]
        public string DE_CODE { get; set; }

        [Required]
        [StringLength(200)]
        public string DE_DESC { get; set; }

        [StringLength(40)]
        public string CR_USER { get; set; }
    }
    public class PutDeptRequest
    {
        public Int64 ID { get; set; }

        [Required]
        [StringLength(40)]
        public string DE_CODE { get; set; }

        [Required]
        [StringLength(200)]
        public string DE_DESC { get; set; }

        [StringLength(40)]
        public string UP_USER { get; set; }

        public DateTime? UP_DATE { get; set; }
    }

    #endregion
}
