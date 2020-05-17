using HRCM.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTotal.Base
{
    public class AccessServices : BaseServices
    {
        public AccessServices()
        {
            OpenConnection("BAPUser");
        }
        public bool isAdmin(string userID)
        {

            SQL = string.Format("select RoleName from SystemUser su join SystemUserRole sur on su.ID = sur.userID join SystemRole sr on sr.id = sur.roleID where RoleName = 'Admin' and su.id = '{0}'", userID);
            Logger.Info("isAdmin - Retrieve Parm : " + userID);
            cmd = new SqlCommand(SQL,conn);
            dt = GetDataTable(cmd);
            Logger.Info("isAdmin - Total Record retrieved : " + dt.Rows.Count.ToString());
            return dt.Rows.Count > 0;
        }

        public SystemUser getUser(string userID)
        {

            SQL = string.Format("select * from dbo.SystemUser where Account = '{0}'", userID );
            Logger.Info("getUser - Retrieve Parm : " + userID );
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Info("getUser - Total Record retrieved : " + dt.Rows.Count.ToString());
            SystemUser cd = null;
            foreach (DataRow dr in dt.Rows)
            {
                cd = new SystemUser()
                {
                    ID = dr["ID"].ToString(),
                    UserName = dr["UserName"].ToString(),
                    Password = dr["Password"].ToString()
                };
                break;
            }
            return cd;
        }

        public SystemUser getUserbyID(string userID)
        {

            SQL = string.Format("select * from dbo.SystemUser where ID = '{0}'", userID);
            Logger.Info("getUser - Retrieve Parm : " + userID);
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Info("getUser - Total Record retrieved : " + dt.Rows.Count.ToString());
            SystemUser cd = null;
            foreach (DataRow dr in dt.Rows)
            {
                cd = new SystemUser()
                {
                    ID = dr["ID"].ToString(),
                    UserName = dr["UserName"].ToString(),
                    Password = dr["Password"].ToString()
                };
                break;
            }
            return cd;
        }

    }
}
