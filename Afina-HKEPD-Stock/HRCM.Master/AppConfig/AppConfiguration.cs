using Microsoft.Extensions.Configuration;
using System.IO;

namespace HRCM.Master.AppConfig
{
    public class AppConfiguration
    {
        public readonly string _connectionString = string.Empty;
        protected IConfigurationRoot root = null;
        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            root = configurationBuilder.Build();
        }
        //public string ConnectionString
        //{
        //    get => _connectionString;
        //}
        public string ConnectionString(string parm)
        {
            return root.GetSection("ConnectionStrings").GetSection(parm).Value;
        }

        public string appSetting(string parm)
        {
            return root.GetSection("AppSettings").GetSection(parm).Value;
        }

        public string getAttribut(string section, string parm)
        {
            return root.GetSection(section).GetSection(parm).Value;
        }
    }
}
