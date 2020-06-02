using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using HRCM.Master.AppConfig;
using System.Data;
using Microsoft.Extensions.Logging;
using HRCM.Master.Model;
using HRCM.Master.Model.Common;

namespace HRCM.Master.Business
{
    public class StaffServices : BaseServices
    {
        //public StaffServices(ILogger<StaffServices> _logger) : base(_logger)
        public StaffServices()
        {
            OpenConnection("HRCM");
        }

        public List<EmployeeDataSourceModel> getEmployeeSource()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getEmployeeSource));
            List<EmployeeDataSourceModel> cList = new List<EmployeeDataSourceModel>();
            DataTable dt = GetStaffRecords();
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr["STAFF_POSITION"].ToString()))
                {
                    cList.Add(new EmployeeDataSourceModel()
                    {
                        id = Convert.ToInt32(dr["ID"]),
                        text = dr["STAFF_NAME"].ToString(),
                        groupId = Convert.ToInt32(dr["STAFF_POSITION"]),
                        color = "#bbdc00",
                        designation = ""
                    });
                }
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getEmployeeSource));
            return cList;
        }

        public IEnumerable<StaffList> getStaffLists()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getStaffLists));
            List<StaffList> staffList = new List<StaffList>();
            DataTable dt = GetStaffRecords();
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                staffList.Add(new StaffList()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    GUID = dr["EMPLOYEE_GUID"].ToString(),
                    StaffNo = dr["STAFF_NO"].ToString(),
                    Name = dr["STAFF_NAME"].ToString(),
                    NickName = dr["STAFF_NICKNAME"].ToString(),
                    ChineseName = dr["STAFF_CHINESE_NAME"].ToString(),
                    Company = dr["STAFF_COMPANY"].ToString(),
                    Department = dr["STAFF_DEPARTMENT"].ToString(),
                    Position = dr["STAFF_POSITION"].ToString(),
                    JoinDate = dr["STAFF_JOIN_DATE"].ToString(),
                    //TerminatedDate = dr["STAFF_TERMINATED_DATE"].ToString(),
                    Ranking = dr["STAFF_RANKING"].ToString(),
                    CR_USR = dr["CR_USR"].ToString(),
                    CR_DATE = Convert.ToDateTime(dr["CR_DATE"]),
                    UP_USR = dr["UP_USR"].ToString(),
                    UP_DATE = string.IsNullOrEmpty(dr["UP_DATE"].ToString()) ? tmp : Convert.ToDateTime(dr["UP_DATE"])
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(getStaffLists));
            return staffList; 
        }

        public IEnumerable<StaffAttribute> GetStaffAttribute(string EMPLOYEE_GUID,string section)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(GetStaffAttribute));
            List<StaffAttribute> StaffAttribute = new List<StaffAttribute>();
            DataTable dt = GetStaffAttributeRecords(EMPLOYEE_GUID, section);
            foreach (DataRow dr in dt.Rows)
            {
                StaffAttribute.Add(new StaffAttribute()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    EMPLOYEE_ID = Convert.ToInt64(dr["EMPLOYEE_ID"]),
                    ATTRIBUTE_DESC = dr["ATTRIBUTE_DESC"].ToString(),
                    ATTRIBUTE_NAME = dr["ATTRIBUTE_NAME"].ToString(),
                    ATTRIBUTE_VALUE = dr["ATTRIBUTE_VALUE"].ToString()//,
                    //ATTRIBUTE_CATEGORY = dr["ATTRIBUTE_CATEGORY"].ToString(),
                    //ATTRIBUTE_CTRL_TYPE = dr["ATTRIBUTE_CTRL_TYPE"].ToString(),
                    //CR_USR = dr["CR_USR"].ToString(),
                    //CR_DATE = Convert.ToDateTime(dr["CR_DATE"]),
                    //UP_USR = dr["UP_USR"].ToString(),
                    //UP_DATE = string.IsNullOrEmpty(dr["UP_DATE"].ToString()) ? tmp : Convert.ToDateTime(dr["UP_DATE"])
                });
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(GetStaffAttribute));
            return StaffAttribute;
        }

        public IEnumerable<Staff> GetStaffAttributeSingle(string EMPLOYEE_GUID)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(GetStaffAttributeSingle));
            List<Staff> StaffAttribute = new List<Staff>();
            DataTable dt = GetStaffAttributeRecords(EMPLOYEE_GUID);
            DateTime? tmp = null;
            foreach (DataRow dr in dt.Rows)
            {
                StaffAttribute.Add( new Staff()
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    GUID = dr["EMPLOYEE_GUID"].ToString(),
                    EM_NO = dr["EM_NO"].ToString(),
                    EM_NAME = dr["EM_NAME"].ToString(),
                    EM_NNAME = dr["EM_NNAME"].ToString(),

                    EM_SECTION = dr["EM_SECTION"].ToString(),
                    EM_DEPT = dr["EM_DEPT"].ToString(),
                    EM_POSI = dr["EM_POSI"].ToString(),
                    EM_COMP = dr["EM_COMP"].ToString(),

                    EM_ID = dr["EM_ID"].ToString(),
                    EM_SEX = dr["EM_SEX"].ToString(),

                    JOIN_DATE = string.IsNullOrEmpty(dr["JOIN_DATE"].ToString()) ? tmp : Convert.ToDateTime(dr["JOIN_DATE"]),
                    TERMINATE_DATE = string.IsNullOrEmpty(dr["TERMINATE_DATE"].ToString()) ? tmp : Convert.ToDateTime(dr["TERMINATE_DATE"]),
                    EM_BIRTH = string.IsNullOrEmpty(dr["EM_BIRTH"].ToString()) ? tmp : Convert.ToDateTime(dr["EM_BIRTH"]),

                    EM_TEL = dr["EM_TEL"].ToString(),
                    EM_MOBILE = dr["EM_MOBILE"].ToString(),
                    EM_MARIT = dr["EM_MARIT"].ToString(),
                    EM_SPOUSE_ID = dr["EM_SPOUSE_ID"].ToString(),
                    EM_SPOUSE_NAME = dr["EM_SPOUSE_NAME"].ToString(),

                    LEAVE_PLAN = dr["LEAVE_PLAN"].ToString(),
                    HOILDAY_PLAN = dr["HOILDAY_PLAN"].ToString(),

                    LOGIN_ID = dr["LOGIN_ID"].ToString(),
                    LOGIN_PWD = dr["LOGIN_PWD"].ToString(),
                    EM_RANKING = dr["EM_RANKING"].ToString(),
                    CR_USR = dr["CR_USR"].ToString(),
                    CR_DATE = Convert.ToDateTime(dr["CR_DATE"]),
                    UP_USR = dr["UP_USR"].ToString(),
                    UP_DATE = string.IsNullOrEmpty(dr["UP_DATE"].ToString()) ? tmp : Convert.ToDateTime(dr["UP_DATE"])
                });
                //StaffAttribute.Add(new StaffAttribute()
                //{
                //    ID = Convert.ToInt64(dr["ID"]),
                //    ATTRIBUTE_DESC = dr["ATTRIBUTE_DESC"].ToString(),
                //    ATTRIBUTE_NAME = dr["ATTRIBUTE_NAME"].ToString(),
                //    ATTRIBUTE_VALUE = dr["ATTRIBUTE_VALUE"].ToString(),
                //    ATTRIBUTE_CATEGORY = dr["ATTRIBUTE_CATEGORY"].ToString(),
                //    ATTRIBUTE_CTRL_TYPE = dr["ATTRIBUTE_CTRL_TYPE"].ToString(),
                //    CR_USR = dr["CR_USR"].ToString(),
                //    CR_DATE = Convert.ToDateTime(dr["CR_DATE"]),
                //    UP_USR = dr["UP_USR"].ToString(),
                //    UP_DATE = string.IsNullOrEmpty(dr["UP_DATE"].ToString()) ? tmp : Convert.ToDateTime(dr["UP_DATE"])
                //});
            }
            Logger.Debug("'{0}' has been invoked and End", nameof(GetStaffAttributeSingle));
            return StaffAttribute;
        }

        #region DB Process
        public DataTable GetStaffRecords()
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(GetStaffRecords));
            SQL = "select ID, EMPLOYEE_GUID,  ";
            SQL += " dbo.getEmployeeAttribute(ID, 'EMPLOYEE_NO') STAFF_NO,";
            SQL += " dbo.getEmployeeAttribute(ID, 'EMPLOYEE_NAME') STAFF_NAME,";
            SQL += " dbo.getEmployeeAttribute(ID, 'EM_NNAME') STAFF_NICKNAME,";
            //            SQL += " dbo.getEmployeeName(ID) STAFF_NAME,";
            SQL += " dbo.getEmployeeAttribute(ID, 'EM_CNAME') STAFF_CHINESE_NAME,";
            SQL += " dbo.getEmployeeAttribute(ID, 'COMPANY') STAFF_COMPANY,";
            SQL += " dbo.getEmployeeAttribute(ID, 'DEPT') STAFF_DEPARTMENT,";
            SQL += " dbo.getEmployeeAttribute(ID, 'POSI') STAFF_POSITION,";
            SQL += " Convert(varchar(10), dbo.getEmployeeAttribute(ID, 'JOIN_DATE'), 121) STAFF_JOIN_DATE,";
            //SQL += " Convert(varchar(10), dbo.getEmployeeAttribute(ID, 'TERMINATE_DATE'), 121) STAFF_TERMINATED_DATE,";
            SQL += " dbo.getEmployeeAttribute(ID, 'EM_RANKING') STAFF_RANKING,";
            SQL += " CR_USR, CR_DATE, UP_USR, UP_DATE";
            SQL += "  from Employee e;";
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetStaffRecords), dt.Rows.Count.ToString());
            return dt;
        }

        public DataTable GetStaffAttributeRecords(string EMPLOYEE_GUID)
        {
            return GetStaffAttributeRecords(EMPLOYEE_GUID, "");
        }
        public DataTable GetStaffAttributeRecords(string EMPLOYEE_GUID, string section)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(GetStaffAttributeRecords));
            SQL = "select isnull(ea.ID,0) ID, e.ID EMPLOYEE_ID, af.ATTRIBUTE_NAME, ATTRIBUTE_CTRL_TYPE, ATTRIBUTE_DESC, ATTRIBUTE_VALUE, ";
            SQL += " ATTRIBUTE_CATEGORY, ea.CR_USR, ea.CR_DATE, ea.UP_USR, ea.UP_DATE";
            SQL += " from AttributeFactor af";
            SQL += " cross join Employee e";
            SQL += " left outer join EmployeeAttribute ea on af.ATTRIBUTE_NAME = ea.ATTRIBUTE_NAME and e.ID = ea.EMPLOYEE_ID";
            SQL += " where EMPLOYEE_GUID = '" + EMPLOYEE_GUID.ToString() + "'";
            SQL += " and ATTRIBUTE_CATEGORY in ('PERSONAL', 'SALARY', 'ATTEND', 'WORKPLACE')";
            if (section != "") SQL += " and ATTRIBUTE_CATEGORY in ('" + section + "')";
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(GetStaffAttributeRecords), dt.Rows.Count.ToString());
            return dt;
        }

        public string getNewStaff(PostStaffListRequest rec)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(getNewStaff));
            EmployeeAttribute ea = new EmployeeAttribute();
            SQL = "insert into Employee (CR_USR) values ('" + rec.CR_USER + "')";
            cmd = new SqlCommand(SQL, conn);
            RunSQL(cmd);
            SQL = "select cast(ID as varchar(20)) from employee where id = (SELECT IDENT_CURRENT('Employee'))";
            cmd = new SqlCommand(SQL, conn);
            var EMPLOYEE_ID = getValue(cmd);
            //Insert EMPLOYEE_NO
            ea.EMPLOYEE_ID = Convert.ToInt64(EMPLOYEE_ID);
            ea.ATTRIBUTE_NAME = "EMPLOYEE_NO";
            ea.ATTRIBUTE_VALUE = rec.StaffNo;
            ea.CR_USR = rec.CR_USER;
            ea.ID = 0;
            updateAttribute(ea);
            //Insert EMPLOYEE_NAME
            ea.EMPLOYEE_ID = Convert.ToInt64(EMPLOYEE_ID);
            ea.ATTRIBUTE_NAME = "EMPLOYEE_NAME";
            ea.ATTRIBUTE_VALUE = rec.Name;
            ea.CR_USR = rec.CR_USER;
            ea.ID = 0;
            updateAttribute(ea);
            //Insert EM_CNAME
            ea.EMPLOYEE_ID = Convert.ToInt64(EMPLOYEE_ID);
            ea.ATTRIBUTE_NAME = "EM_CNAME";
            ea.ATTRIBUTE_VALUE = rec.ChineseName;
            ea.CR_USR = rec.CR_USER;
            ea.ID = 0;
            updateAttribute(ea);
            //Insert EM_NNAME
            ea.EMPLOYEE_ID = Convert.ToInt64(EMPLOYEE_ID);
            ea.ATTRIBUTE_NAME = "EM_NNAME";
            ea.ATTRIBUTE_VALUE = rec.NickName;
            ea.CR_USR = rec.CR_USER;
            ea.ID = 0;
            updateAttribute(ea);
            //Insert DEPT
            ea.EMPLOYEE_ID = Convert.ToInt64(EMPLOYEE_ID);
            ea.ATTRIBUTE_NAME = "DEPT";
            ea.ATTRIBUTE_VALUE = rec.Department;
            ea.CR_USR = rec.CR_USER;
            ea.ID = 0;
            updateAttribute(ea);
            //Insert POSI
            ea.EMPLOYEE_ID = Convert.ToInt64(EMPLOYEE_ID);
            ea.ATTRIBUTE_NAME = "POSI";
            ea.ATTRIBUTE_VALUE = rec.Position;
            ea.CR_USR = rec.CR_USER;
            ea.ID = 0;
            updateAttribute(ea);
            //Insert Join Date
            ea.EMPLOYEE_ID = Convert.ToInt64(EMPLOYEE_ID);
            ea.ATTRIBUTE_NAME = "JOIN_DATE";
            ea.ATTRIBUTE_VALUE = rec.JoinDate;
            ea.CR_USR = rec.CR_USER;
            ea.ID = 0;
            updateAttribute(ea);
            //Insert Ranking
            ea.EMPLOYEE_ID = Convert.ToInt64(EMPLOYEE_ID);
            ea.ATTRIBUTE_NAME = "EM_RANKING";
            ea.ATTRIBUTE_VALUE = rec.Ranking;
            ea.CR_USR = rec.CR_USER;
            ea.ID = 0;
            updateAttribute(ea);
            // Insert other attribute

            SQL = "insert into EmployeeAttribute ( EMPLOYEE_ID, ATTRIBUTE_NAME, CR_USR ) ";
            SQL += " select " + EMPLOYEE_ID + ", af.ATTRIBUTE_NAME, '" + rec.CR_USER + "'";
            SQL += " from AttributeFactor af";
            SQL += " where ATTRIBUTE_CATEGORY in ('PERSONAL', 'SALARY', 'ATTEND', 'WORKPLACE')";
            SQL += " and not exists(select 1 from EmployeeAttribute ea where EMPLOYEE_ID = " + EMPLOYEE_ID + " and ea.ATTRIBUTE_NAME = af.ATTRIBUTE_NAME)";
            cmd = new SqlCommand(SQL, conn);
            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End with Employee ID : (1)", nameof(getNewStaff), EMPLOYEE_ID);
            return EMPLOYEE_ID;
        }


        public void updateAttribute(EmployeeAttribute rec)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(updateAttribute));
            if (!chkAttributeExist(rec))
            {
                SQL = "insert into EMployeeAttribute (";
                SQL += " EMPLOYEE_ID, ATTRIBUTE_VALUE, ATTRIBUTE_NAME, CR_USR";
                SQL += ") values (";
                SQL += " @EMPLOYEE_ID, @ATTRIBUTE_VALUE, @ATTRIBUTE_NAME, @UID";
                SQL += ")";
            }
            else
            {
                SQL = "update EmployeeAttribute set";
                SQL += " ATTRIBUTE_VALUE = @ATTRIBUTE_VALUE,";
                SQL += " UP_USR = @UID,";
                SQL += " UP_DATE = getdate()";
                SQL += " where ID = @ID";
            }
            cmd = new SqlCommand(SQL, conn);

            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = rec.ID;
            cmd.Parameters.Add("@EMPLOYEE_ID", SqlDbType.BigInt).Value = rec.EMPLOYEE_ID;
            cmd.Parameters.Add("@UID", SqlDbType.NVarChar, 40).Value = rec.CR_USR;
            cmd.Parameters.Add("@ATTRIBUTE_VALUE", SqlDbType.NVarChar, 1000).Value = rec.ATTRIBUTE_VALUE == null ? (object)DBNull.Value : rec.ATTRIBUTE_VALUE;
            cmd.Parameters.Add("@ATTRIBUTE_NAME", SqlDbType.NVarChar, 30).Value = rec.ATTRIBUTE_NAME ;

            RunSQL(cmd);
            Logger.Debug("'{0}' has been invoked and End", nameof(updateAttribute));
        }

        public bool chkAttributeExist(EmployeeAttribute rec)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(chkAttributeExist));
            SQL = "select ID from EmployeeAttribute where EMPLOYEE_ID = " + rec.EMPLOYEE_ID + " and deleted = 0 and ATTRIBUTE_NAME = '" + rec.ATTRIBUTE_NAME + "'";
            cmd = new SqlCommand(SQL, conn);
            dt = GetDataTable(cmd);
            Logger.Debug("'{0}' has been invoked and End - Total Record retrieved : (1)", nameof(chkAttributeExist), dt.Rows.Count.ToString());
            return dt.Rows.Count > 0;
        }

        public string getGUIDbyName(string name)
        {
            Logger.Debug("'{0}' has been invoked and start", nameof(chkAttributeExist));
            SQL = "select dbo.getEmployeeGUIDbyName('" + name + "')";
            cmd = new SqlCommand(SQL, conn);
            var rtn = getValue(cmd);
            Logger.Debug("'{0}' has been invoked and End - GUID retrieved : (1)", nameof(chkAttributeExist), rtn);
            return rtn;
        }

        #endregion
    }
}
