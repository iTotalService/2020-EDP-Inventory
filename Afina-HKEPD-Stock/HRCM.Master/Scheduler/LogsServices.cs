using HRCM.Master.AppConfig;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HRCM.Master.Scheduler
{
    public class LogsServices : BaseServices
    {
        public LogsServices()
        {
            OpenConnectionbySetting("LogDB");
        }

        #region DB Process
        public void truneLog()
        {
            AppConfiguration ac = new AppConfiguration();

            var dayskeep = Convert.ToInt32(ac.getAttribut("LogMaintenance", "DaysKeep")) * -1;
            SQL = string.Format("delete from APILog where logged <= dateadd( day, {0}, getdate())",dayskeep.ToString());
            Logger.Debug("'{0}' has been invoked and start", nameof(truneLog));
            cmd = new SqlCommand(SQL, conn);
            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(truneLog));

            SQL = string.Format("delete from SystemLog where logged <= dateadd( day, {0}, getdate())", dayskeep.ToString());
            Logger.Debug("'{0}' has been invoked and start", nameof(truneLog));
            cmd = new SqlCommand(SQL, conn);
            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(truneLog));

            SQL = string.Format("delete from SQLLog where logged <= dateadd( day, {0}, getdate())", dayskeep.ToString());
            Logger.Debug("'{0}' has been invoked and start", nameof(truneLog));
            cmd = new SqlCommand(SQL, conn);
            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End ", nameof(truneLog));
        }
        #endregion
    }
}
