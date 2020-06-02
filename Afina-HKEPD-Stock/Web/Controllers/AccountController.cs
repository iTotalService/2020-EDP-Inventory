using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
#pragma warning disable CS1591
    public class AccountController : BaseController
    {
        public AccountController(IConfiguration config, IHttpContextAccessor accessor, ILogger<AccountController> logger, IHostingEnvironment hostingEnvironment) : base(config, accessor, logger, hostingEnvironment)
        {
            _config = config;
            _accessor = accessor;
            Logger = logger;
        }

        /// <summary>
        /// 登入頁
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            ViewBag.ReturnUrl = "/Master/Dept";
            Logger?.LogInformation("'{0}' has been loaded", nameof(Login));
            return View();
        }

        /// <summary>
        /// 表單post提交，準備登入
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string UserID, string Password, string ReturnUrl)
        {//未登入者想進入必須登入的頁面，他會被導頁至/Home/Login，網址後面會加上QueryString:ReturnUrl(原始要求網址)

            await HttpContext.SignOutAsync();

            //從自己的DB檢查帳&密，輸入是否正確
            if ((UserID == "shadow" && Password == "shadow") == false)
            {
                //帳&密不正確
                ViewBag.errMsg = "帳號或密碼輸入錯誤";
                Logger?.LogInformation(UserID + " login failed.");
                return View();//流程不往下執行
            }

            var token = Guid.NewGuid().ToString();
            NLog.GlobalDiagnosticsContext.Set("Token", token);
            //帳密都輸入正確，ASP.net Core要多寫三行程式碼 
            Claim[] claims = new[] { new Claim("Account", UserID), new Claim("Token", token) }; //取名Account，在登入後的頁面，讀取登入者的帳號會用得到，自己先記在大腦
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);//Scheme必填
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            //執行登入，相當於以前的FormsAuthentication.SetAuthCookie()
            //從組態讀取登入逾時設定
            double loginExpireMinute = Convert.ToDouble(_config["AppSettings:LoginExpireMinute"]);
            await HttpContext.SignInAsync(principal,
                new AuthenticationProperties()
                {
                    IsPersistent = false, //IsPersistent = false，瀏覽器關閉即刻登出
                                          //用戶頁面停留太久，逾期時間，在此設定的話會覆蓋Startup.cs裡的逾期設定
                    ExpiresUtc = DateTime.Now.AddMinutes(loginExpireMinute)
                });
            Logger?.LogInformation(UserID +" has been Login Successfully.");

            //加上 Url.IsLocalUrl 防止Open Redirect漏洞
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);//導到原始要求網址
            }
            else
            {
                return RedirectToAction("Master", "Dept");//到登入後的第一頁，自行決定
            }

        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        //登出 Action 記得別加上[Authorize]，不管用戶是否登入，都可以執行Logout
        public async Task<IActionResult> Logout()
        {
            LoadUserInfo();
            Logger?.LogInformation(UserID + " has been Logout Successfully.");
            await HttpContext.SignOutAsync();

            return RedirectToAction("Master", "Dept");//到登入後的第一頁，自行決定
        }
    }
#pragma warning restore CS1591
}