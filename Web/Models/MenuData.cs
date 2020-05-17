using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using iTotal.Master;
using iTotal.Master.Model;

namespace WebApi.Controllers
{
    public class MenuData : BaseServices
    {
        private IHostingEnvironment _hostingEnvironment;
        private IConfiguration _configuration;
        protected Logger Logger;
        protected IHttpContextAccessor _accessor;
        protected static readonly string ClaimType = "Account";

        public MenuData(IHostingEnvironment environment,IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = environment;
            _configuration = configuration;
            _accessor = httpContextAccessor;
            Logger = LogManager.GetCurrentClassLogger();
            OpenConnection("sys");
        }
        public string getMenuData()
        {
            GlobalDiagnosticsContext.Set("IpAddress", _accessor.HttpContext.Connection.RemoteIpAddress);
            var list = getMenu();
            string MenuJson = JsonConvert.SerializeObject(list.ToList());
            GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(list.ToList()));
            Logger.Info("Menu have been retrieved successfully.");
            return MenuJson;
        }
        public string getReportServerUrl()
        {
            return getConfigValue("ReportServer.url");
        }

        public IEnumerable<sysMenu> getMenu()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getMenu));
            List<sysMenu> list = new List<sysMenu>();
            DataTable dt = GetMenuRecords();
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                DataTable iDT = getMenuItem(Convert.ToInt32(dr["uid"]));
                List<sysMenuItem> ilist = new List<sysMenuItem>();
                foreach (DataRow idr in iDT.Rows)
                {
                    ilist.Add(new sysMenuItem
                    {
                        uid = idr["uid"].ToString(),
                        name = idr["Name"].ToString(),
                        category = idr["Category"].ToString(),
                        dir = idr["directory"].ToString(),
                        component = idr["directory"].ToString(),
                        type = string.IsNullOrEmpty(idr["type"].ToString()) ? "" : idr["type"].ToString(),
                        url = string.IsNullOrEmpty(idr["url"].ToString()) ? "" : idr["url"].ToString(),
                        order = Convert.ToInt32(idr["order"]),
                        hidden = Convert.ToBoolean(idr["hidden"]),
                        parentId = idr["parentId"].ToString()
                    });
                }
                list.Add(new sysMenu()
                {
                    uid = dr["uid"].ToString(),
                    name = dr["Name"].ToString(),
                    category = dr["Category"].ToString(),
                    directory = dr["directory"].ToString(),
                    type = string.IsNullOrEmpty(dr["type"].ToString()) ? "" : dr["type"].ToString(),
                    order = Convert.ToInt32(dr["order"]),
                    samples = ilist
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getMenu));
            return list;
        }

        #region DB Process
        public string getConfigValue(string config)
        {
            SQL = "select configValue from defconfig where configType = '" + config + "'";
            Logger.Debug("'{0}' has been invoked and start", nameof(getConfigValue));
            cmd = new SqlCommand(SQL, conn);
            string rtn = getValue(cmd);
            Logger.Debug("'{0}' has been invoked and End - Return Value : (1)", nameof(getConfigValue), rtn);
            return rtn;
        }
        public DataTable getMenuItem(Int32 parentID)
        {
            SQL = "select * from sysMenu where parentId = " + parentID.ToString();
            Logger.Debug("'{0}' has been invoked and start", nameof(getMenuItem));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(getMenuItem), dt.Rows.Count.ToString());
            return dt;
        }
        public DataTable GetMenuRecords()
        {
            SQL = "select * from sysMenu where parentId = 0";
            Logger.Debug("'{0}' has been invoked and start", nameof(GetMenuRecords));
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetMenuRecords), dt.Rows.Count.ToString());
            return dt;
        }
        #endregion
    }
#pragma warning restore CS1591
}
