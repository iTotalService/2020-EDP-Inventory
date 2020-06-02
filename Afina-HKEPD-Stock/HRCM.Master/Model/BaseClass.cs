using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace HRCM.Master.Model
{
    public class BaseClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CR_DATE { get; set; }
        public string CR_USR { get; set; }
        public DateTime? UP_DATE { get; set; }
        public string UP_USR { get; set; }
        public DateTime? DEL_DATE { get; set; }
        public string DEL_USR { get; set; }
        public bool DELETED { get; set; }
    }

    public static class IQueryableExtensions
    {
        public static IQueryable<TModel> Paging<TModel>(this IQueryable<TModel> query, int pageSize = 0, int pageNumber = 0) where TModel : class
            => pageSize > 0 && pageNumber > 0 ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize) : query;
    }

    public class BaseRequest
    {
        public Int64 ID { get; set; }
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
