using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Extensions.Logging;
using iTotal.Master.Model;

namespace iTotal.Master.Business
{
    public class sysServices : BaseServices
    {
        

        public sysServices()
        {
            OpenConnection("sys");
        }

        #region DB Process
        public string getConfigValue(string type)
        {
            SQL = "select configvalue from defconfig " +
                "where configtype = '" + type + "' and DELETED2 = 0";
            cmd = new SqlCommand(SQL, conn);
            var rtn = getValue(cmd);
            Logger.Debug("'{0}' has been invoked and End - record value =  (1)", nameof(getConfigValue), rtn);
            return rtn;
        }
        #endregion
    }
}
