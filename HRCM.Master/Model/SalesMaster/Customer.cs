using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HRCM.Master.Model
{
    public class Customer : BaseClass
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
  

        public Customer()
        {
        }

        public Customer(long id)
        {
            ID = id;
        }

    }

    #region Extensions
    public static class CustomerDbContextExtensions
    {

        public static Customer ToEntity(this PostCustomerRequest request)
            => new Customer
            {
                CustomerCode = request.CustomerCode,
                CustomerName = request.CustomerName,
                CR_USR = request.CR_USER ,
                CR_DATE = DateTime.Now
            };

        public static IQueryable<Customer> GetCustomers(this HRCMContext dbContext, int pageSize = 10, int pageNumber = 1, string lastEditedBy = null, int? ID = null)
        {
            // Get query from DbSet
            var query = dbContext.Customer.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);
                        //.Skip(pageSize * (pageNumber - 1));
                        //.Take(pageSize);

            // Filter by: 'ID'
            if (ID.HasValue)
                query = query.Where(item => item.ID == ID);

            return query;
        }

        public static async Task<Customer> GetbyIDAsync(this HRCMContext dbContext, Customer entity)
            => await dbContext.Customer.FirstOrDefaultAsync(item => item.ID == entity.ID);
        
        public static async Task<Customer> GetByNameAsync(this HRCMContext dbContext, Customer entity)
            => await dbContext.Customer.FirstOrDefaultAsync(item => item.CustomerName == entity.CustomerName);
    }

    #endregion

    #region Request Class
    public class DeleteCustomerRequest
    {
        public Int64 ID { get; set; }

        public string User { get; set; }
    }

    public class PostCustomerRequest
    {
        [Required]
        [StringLength(4)]
        public string CustomerCode { get; set; }
        [StringLength(100)]
        public string CustomerName { get; set; }
        public string CR_USER { get; set; }
    }
    public class PutCustomerRequest
    {
        public Int64 ID { get; set; }
        [Required]
        [StringLength(4)]
        public string CustomerCode { get; set; }
        [StringLength(100)]
        public string CustomerName { get; set; }
        [StringLength(40)]
        public string UP_USER { get; set; }

        public DateTime? UP_DATE { get; set; }
    }

    #endregion
}
