using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace iTotal.Master.Model
{
    public class Section : BaseClass
    {
        public string SectionCode { get; set; }
        public string Description { get; set; }
 
        public Section()
        {
        }

        public Section(long id, string UserID)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class SectionDbContextExtensions
    {
        public static Section ToEntity(this PostSectionRequest request)
            => new Section
            {
                SectionCode = request.SectionCode,
                Description = request.Description,
                CreateDate = DateTime.Now
            };

        public static IQueryable<Section> GetSections(this InvContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.Section.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.Deleted2 == 0);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<Section> GetbyIDAsync(this InvContext dbContext, Section entity)
            => await dbContext.Section.FirstOrDefaultAsync(item => item.ID == entity.ID);
        
        public static async Task<Section> GetByNameAsync(this InvContext dbContext, Section entity)
            => await dbContext.Section.FirstOrDefaultAsync(item => item.Description == entity.Description);
    }

    #endregion

    #region Request Class
    public class DeleteSectionRequest
    {
        public long ID { get; set; }

        public string User { get; set; }
    }

    public class PostSectionRequest
    {
        [Required]
        [StringLength(20)]
        public string SectionCode { get; set; }

        [StringLength(60)]
        public string Description { get; set; }
    }
    public class PutSectionRequest
    {
        public long ID { get; set; }

        [Required]
        [StringLength(20)]
        public string SectionCode { get; set; }

        [StringLength(60)]
        public string Description { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }

    #endregion
}
