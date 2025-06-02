using CMS.ViewModel.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CMS.ViewModel.ViewModel
{
    public class AttendanceViewModel
    {
        public List<ClassModel> Classes { get; set; }
        public List<ATTENDANCE_ENTRY_MODEL> attendanceEntry { get; set; }
        public List<SUP_REASON> reasons { get; set; }
        public List<SUP_ATTENDANCE_TYPE> attendanceTypes { get; set; }
        public SelectList ProgrammeList { get; set; }
        public SelectList ClassList { get; set; }
        public List<STAFF_PERSONAL> staffList { get; set; }
        public List<SUP_HOURS> hourList { get; set; }
        public List<SUP_SPL_ATTENDANCE_TYPE> SplAttendanceType { get; set; }
        public List<Student_Personal_Info> liAttendanceStaus { get; set; }
    }
}
