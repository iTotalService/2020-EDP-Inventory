using iTotal.BAPComponent.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace iTotal.BAP.Helpers
{
    public class WebSiteHelper
    {
        private static AccessServices _accessManager;

        public static bool isAdmin
        {
            get
            {
                var httpContext = HttpContext.Current;
                var identity = httpContext.User.Identity as FormsIdentity;

                if (identity == null)
                {
                    return false;
                }
                else
                {
                    var userID = identity.Name;
                    return chkAdmin(userID);
                }
            }
        }
        public static string CurrentUserID
        {
            get
            {
                var httpContext = HttpContext.Current;
                var identity = httpContext.User.Identity as FormsIdentity;

                if (identity == null)
                {
                    return string.Empty;
                }
                else
                {
                    return identity.Name;
                }
            }
        }
        public static string CurrentUserName
        {
            get
            {
                var httpContext = HttpContext.Current;
                var identity = httpContext.User.Identity as FormsIdentity;

                if (identity == null)
                {
                    return string.Empty;
                }
                else
                {
                    var userID = identity.Name;
                    return SystemUserName(userID);
                }
            }
        }

        /// <summary>
        /// Systems the name of the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string SystemUserName(Object id)
        {
            string userName = string.Empty;

            Guid systemUserID;

            if (!Guid.TryParse(id.ToString(), out systemUserID))
            {
                return userName;
            }
            if (systemUserID.Equals(Guid.Empty))
            {
                userName = "系統預設";
            }
            else
            {
                _accessManager = new AccessServices();

                var user = _accessManager.getUserbyID(systemUserID.ToString());
                userName = (user == null) ? string.Empty : user.UserName;
            }
            return userName;
        }

        public static bool chkAdmin(Object id)
        {
            string userName = string.Empty;

            Guid systemUserID;

            if (!Guid.TryParse(id.ToString(), out systemUserID))
            {
                return false;
            }
            else
            {
                _accessManager = new AccessServices();

                return  _accessManager.isAdmin(systemUserID.ToString());
            }
        }
    }
}
