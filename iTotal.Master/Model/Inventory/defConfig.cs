using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace iTotal.Master.Model
{
    public class defConfig 
    {
        [Key]
        public string configType { get; set; }
        public string configValue { get; set; }

        public defConfig()
        {
        }
    }

    #region Extensions
    public static class defConfigDbContextExtensions
    {
        public static async Task<defConfig> GetConfigValueAsync(this InvContext dbContext, string configType)
            => await dbContext.defConfig.FirstOrDefaultAsync(item => item.configType == configType);
    }

    #endregion
}
