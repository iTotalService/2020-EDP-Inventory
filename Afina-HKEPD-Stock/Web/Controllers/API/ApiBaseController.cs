using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using iTotal.Master.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Extensions;

namespace WebApi.Controllers
{
#pragma warning disable CS1591

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ApiBaseController : ControllerBase
    {
        protected  ILogger Logger;
        protected  InvContext _context;
        protected IHttpContextAccessor _accessor;
        protected static readonly string ClaimType = "Account";
        public ApiBaseController(ILogger<ApiBaseController> logger, InvContext context, IHttpContextAccessor accessor)
        {
            Logger = logger;
            _context = context;
            _accessor = accessor;
            NLog.GlobalDiagnosticsContext.Set("IpAddress", _accessor.HttpContext.Connection.RemoteIpAddress);
        }

        public string UserID
        {
            get
            {
                return User.GetUserName();
            }
        }

        protected void LoadUserInfo()
        {
            NLog.GlobalDiagnosticsContext.Set("UserID", User.GetUserName());
            NLog.GlobalDiagnosticsContext.Set("Token", User.GetToken());
            NLog.GlobalDiagnosticsContext.Set("Response", "");
        }

    }
#pragma warning restore CS1591
}