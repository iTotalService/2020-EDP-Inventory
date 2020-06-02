using System;
using System.Collections.Generic;
using System.Text;

namespace HRCM.Master.Model.Common
{
    public class ScheduleData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? RosterStartTime { get; set; }
        public DateTime? RosterEndTime { get; set; }
        public DateTime? AttendStartTime { get; set; }
        public DateTime? AttendEndTime { get; set; }
        public string Description { get; set; }
        public int DepartmentID { get; set; }
        public int ConsultantID { get; set; }
        public string CategoryColor { get; set; }
    }
}
