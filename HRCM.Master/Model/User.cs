using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCM.Master.Model
{
    #region User Master
    public class User : BaseClass
    {
        public string sysUserID { get; set; }
        public string sysUserName { get; set; }
        public string sysUserPassword { get; set; }
        public string sysUserEmail { get; set; }

        public User()
        {
        }

        public User(long id)
        {
            ID = id;
        }
    }

    #region Extensions
    public static class UserMasterDbContextExtensions
    {

        public static IQueryable<User> GetUserMasters(this HRCMContext dbContext)
        {
            // Get query from DbSet
            var query = dbContext.UserMaster.AsQueryable();

            // filter out all deleted records
            query = query.Where(item => item.DELETED == false);
            return query;
        }

        public static async Task<User> GetbyIDAsync(this HRCMContext dbContext, User entity)
            => await dbContext.UserMaster.FirstOrDefaultAsync(item => item.ID == entity.ID);

        public static async Task<User> GetByNameAsync(this HRCMContext dbContext, User entity)
            => await dbContext.UserMaster.FirstOrDefaultAsync(item => item.sysUserName == entity.sysUserName);
    }

    #endregion

    #endregion

    public class RoleMaster : BaseClass
    {
        public string sysRoleID { get; set; }
        public string sysRoleName { get; set; }
        public RoleMaster()
        {
        }

        public RoleMaster(long id)
        {
            ID = id;
        }
    }

    public class UserRole : BaseClass
    {
        public int sysUserID { get; set; }
        public int sysRoleID { get; set; }
        public UserRole()
        {
        }

        public UserRole(long id)
        {
            ID = id;
        }
    }
}
