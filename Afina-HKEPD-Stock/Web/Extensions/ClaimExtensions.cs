using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public static class ClaimsExtension
    {
        public static string GetUserName(this IPrincipal user)
        {
            if (user != null)
            {
                var identity = (ClaimsIdentity)user.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                return claims.FirstOrDefault(s => s.Type == "Account")?.Value;
            }
            return null;
        }

        public static string GetToken(this IPrincipal user)
        {
            if (user != null)
            {
                var identity = (ClaimsIdentity)user.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                return claims.FirstOrDefault(s => s.Type == "Token")?.Value;
            }
            return null;
        }
    }

}
