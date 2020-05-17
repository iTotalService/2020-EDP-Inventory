using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class WorkTime : BaseClass
    {
        public string WO_CODE { get; set; }
        public string WO_DESC { get; set; }

        public TimeSpan WO_T1 { get; set; }
        public TimeSpan WO_T2 { get; set; }
        public TimeSpan? WO_T3 { get; set; }
        public TimeSpan? WO_T4 { get; set; }


        public WorkTime()
        {
        }

        public WorkTime(long id)
        {
            ID = id;
        }

        public WorkTime(long id, string UserID)
        {
            ID = id;
            CR_USR = UserID;
        }

    }

    #region Extensions
    public static class WorkTimeDbContextExtensions
    {
        static TimeSpan? nullDateTime = null;

        public static WorkTime ToEntity(this PostWorkTimeRequest request)
            => new WorkTime
            {
                WO_CODE = request.WO_CODE,
                WO_DESC = request.WO_DESC,
                WO_T1 = request.WO_T1,
                WO_T2 = request.WO_T2,
                WO_T3 = request.WO_T3 == null ? nullDateTime : request.WO_T3,
                WO_T4 = request.WO_T4 == null ? nullDateTime : request.WO_T4,
                CR_USR = request.CR_USER,
                UP_USR = request.UP_USER,
                DEL_USR = request.DEL_USER,
                CR_DATE = DateTime.Now
            };

        public static IQueryable<WorkTime> GetWorkTimes(this HRCMContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.WorkTime.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<WorkTime> GetbyIDAsync(this HRCMContext dbContext, WorkTime entity)
            => await dbContext.WorkTime.FirstOrDefaultAsync(item => item.ID == entity.ID);

        public static async Task<WorkTime> GetByNameAsync(this HRCMContext dbContext, WorkTime entity)
            => await dbContext.WorkTime.FirstOrDefaultAsync(item => item.WO_DESC == entity.WO_DESC);
    }

    #endregion

    #region Request Class
    public class DeleteWorkTimeRequest
    {
        public Int64 ID { get; set; }

        public string User { get; set; }
    }

    public class PostWorkTimeRequest
    {
        //public Int64 ID { get; set; }
        [Required]
        [StringLength(40)]
        public string WO_CODE { get; set; }

        [Required]
        [StringLength(200)]
        public string WO_DESC { get; set; }
        public TimeSpan WO_T1 { get; set; }
        public TimeSpan WO_T2 { get; set; }
        public TimeSpan? WO_T3 { get; set; }
        public TimeSpan? WO_T4 { get; set; }


        [StringLength(40)]
        public string CR_USER { get; set; }
        [StringLength(40)]
        public string UP_USER { get; set; }
        [StringLength(40)]
        public string DEL_USER { get; set; }

    }
    public class PutWorkTimeRequest
    {
        public Int64 ID { get; set; }

        [Required]
        [StringLength(40)]
        public string WO_CODE { get; set; }

        [Required]
        [StringLength(200)]
        public string WO_DESC { get; set; }
        public TimeSpan WO_T1 { get; set; }
        public TimeSpan WO_T2 { get; set; }
        public TimeSpan? WO_T3 { get; set; }
        public TimeSpan? WO_T4 { get; set; }

        [StringLength(40)]
        public string UP_USER { get; set; }

        public DateTime? UP_DATE { get; set; }
    }

    #endregion
}
