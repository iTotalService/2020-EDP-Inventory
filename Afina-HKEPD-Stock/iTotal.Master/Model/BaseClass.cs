using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace iTotal.Master.Model
{
    public class BaseClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool DELETED { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public static class IQueryableExtensions
    {
        public static IQueryable<TModel> Paging<TModel>(this IQueryable<TModel> query, int pageSize = 0, int pageNumber = 0) where TModel : class
            => pageSize > 0 && pageNumber > 0 ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize) : query;
    }

    public class BaseRequest
    {
        public long ID { get; set; }
        [StringLength(40)]
        public string UP_USER { get; set; }
        public DateTime? UP_DATE { get; set; }
        [StringLength(40)]
        public string CR_USER { get; set; }

    }

    public class DeleteRequest : BaseRequest
    {
        public string User { get; set; }
    }
}
