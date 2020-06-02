using HRCM.Master.AppConfig;
using HRCM.Master.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRCM.Master.Security
{
    public class UserMasterRepository : IDisposable
    {
        HRCMContext context;

        public UserMasterRepository()
        {
            AppConfiguration ac = new AppConfiguration();
            context = new HRCMContext(ac.ConnectionString("HRCM"));
        }

        //This method is used to check and validate the user credentials
        public User ValidateUser(string username, string password)
        {
            return context.UserMaster.FirstOrDefault(user =>
            user.sysUserName.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.sysUserPassword == password);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
