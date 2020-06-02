using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Syncfusion.Licensing;
using Hangfire;
using Hangfire.SqlServer;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Models.nLog;
using NLog;
using WebApi.Controllers;
using iTotal.Master.Model;
using iTotal.Master.Scheduler;

namespace Web
{
    public class Startup
    {
        private readonly ILoggerFactory _factory;
        public IConfiguration Configuration { get; }
        public IHostingEnvironment env { get; }
        public IHttpContextAccessor _httpContextAccessor { get; set; }
        //public InvContext _InvContext { get; set; }

        public Startup(IConfiguration configuration, ILoggerFactory factory, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            env = hostingEnvironment;
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            SyncfusionLicenseProvider.RegisterLicense("MTkwNjk1QDMxMzcyZTM0MmUzMEdUNEU1bitFaGgvTStmTFU0ZjUvTk9vRmxKdU9LaG5sZHRrTmNsMTRTMnM9");
            //GlobalDiagnosticsContext.Set("configDir", "C:\\temp\\Logs");
            GlobalDiagnosticsContext.Set("connectionString", Configuration["AppSettings:LogDB"]);

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            #endregion

            #region Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "HRCM API", Version = "v1" });

                // Get xml comments path
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Set xml path
                options.IncludeXmlComments(xmlPath);
            });
            #endregion

            #region Registor Context
            var conn = Configuration["AppSettings:sys"];
            services.AddDbContext<InvContext>(options =>
            {
                options.UseSqlServer(conn);
                options.UseLoggerFactory(_factory);
            });
            #endregion

            #region get remoteIP
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<nLogFilterExtension>();
            //services.AddDirectoryBrowser();
            services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
               .AddJsonOptions(options =>
               {
                   options.SerializerSettings.ContractResolver
                       = new DefaultContractResolver();
               });

            //Added on 2019-10-10 for Login secuity 
            //getting form configuration
            double loginExpireMinute = Convert.ToDouble(Configuration["AppSettings:LoginExpireMinute"]);
            //register CookieAuthentication，Scheme (must have)
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                //option.LoginPath = new PathString("/Account/Login");//login page
                //option.LogoutPath = new PathString("/Account/Logout");// logout Action
                //                                                      //time out 
                //option.ExpireTimeSpan = TimeSpan.FromMinutes(loginExpireMinute);//defalut 14 days
            });

            #region Register Menu service
            services.Add(new ServiceDescriptor(typeof(MenuData), new MenuData(env, Configuration, _httpContextAccessor)));
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHangfireDashboard();

            #region HangFire Job Define
            RecurringJob.RemoveIfExists("Purge Log File");
            if (Configuration["LogMaintenance:Enable"] =="true")
                RecurringJob.AddOrUpdate<LogsServices>("Purge Log File", x => x.truneLog(), Configuration["LogMaintenance:schedule"], TimeZoneInfo.Local);
            #endregion

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            // Add authentication to request pipeline
            //app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRCM API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action}/{id?}",
            //        defaults: new { controller = "Home", action = "Index" }
            //        );
            //});
            
            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")))
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"scripts", @"ej2.min.js"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2", @"ej2.min.js"));
                }

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"bootstrap.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"material.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"material.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"highcontrast.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"highcontrast.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"fabric.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"fabric.css"));
                }
            }
        }
    }
}
