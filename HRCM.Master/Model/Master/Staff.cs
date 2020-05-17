using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRCM.Master.Model
{
    public class Staff : BaseClass
    {
        public string GUID { get; set; }
        public string EM_NO { get; set; }
        public string EM_NAME { get; set; }
        public string EM_NNAME { get; set; }
        public string EM_MARIT { get; set; }
        public string EM_ID { get; set; }
        public DateTime? EM_BIRTH { get; set; }
        public string EM_SEX { get; set; }
        public string EM_MOBILE { get; set; }
        public string EM_SECTION { get; set; }
        public string EM_SPOUSE_ID { get; set; }
        public string EM_SPOUSE_NAME { get; set; }
        public string EM_TEL { get; set; }
        public string LEAVE_PLAN { get; set; }
        public string HOILDAY_PLAN { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public string LOGIN_ID { get; set; }
        public string LOGIN_PWD { set; get; }
        public string EM_POSI { get; set; }
        public string EM_COMP { get; set; }
        public string EM_RANKING { get; set; }
        public DateTime? TERMINATE_DATE { get; set; }
        public string EM_DEPT { get; set; }
    }
    public class StaffAttribute : BaseClass
    {
        public string GUID { set; get; }
        public long EMPLOYEE_ID { set; get; }
        public string ATTRIBUTE_NAME { set; get; }
        public string ATTRIBUTE_DESC { set; get; }
        public string ATTRIBUTE_VALUE { set; get; }
        public string ATTRIBUTE_CATEGORY { set; get; }
        public string ATTRIBUTE_CTRL_TYPE { set; get; }
    }

    public class EmployeeAttribute : BaseClass
    {
        public long EMPLOYEE_ID { set; get; }
        public string ATTRIBUTE_NAME { set; get; }
        public string ATTRIBUTE_VALUE { set; get; }

    }

    public class PostStaffListRequest
    {
        public string StaffNo { set; get; }
        public string Name { set; get; }
        public string NickName { set; get; }
        public string ChineseName { set; get; }
        public string Company { set; get; }
        public string Department { set; get; }
        public string Position { set; get; }
        public string JoinDate { set; get; }
        public string Ranking { set; get; }
        [StringLength(40)]
        public string CR_USER { get; set; }
        [StringLength(40)]
        public string UP_USER { get; set; }
        [StringLength(40)]
        public string DEL_USER { get; set; }

    }
    public class StaffList : BaseClass
    {
        public string GUID { set; get; }
        public string StaffNo { set; get; }
        public string Name { set; get; }
        public string NickName { set; get; }
        public string ChineseName {set; get;}
        public string Company { set; get; }
        public string Department { set; get; }
        public string Position { set; get; }
        public string JoinDate { set; get; }
        public string TerminatedDate { set; get; }
        public string Ranking { set; get; }
    }

    public class PostStaffAttributeRequest : BaseRequest
    {
        [Required]
        public List<EmployeeAttribute> EmployeeAttributes { get; set; }
    }
}
