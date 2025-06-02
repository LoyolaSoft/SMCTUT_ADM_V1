using CMS.ViewModel.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace CMS.ViewModel.ViewModel
{
    public class TimeTableViewModel
    {
        public List<TimeTable> lstTimeTable = new List<TimeTable>();
        public List<SUP_HOURS> lstHours = new List<SUP_HOURS>();
        public List<SUP_DAY_ORDER> lstDayOrders = new List<SUP_DAY_ORDER>();
        public List<TimeTableSetting> lstTimeTableSetting = new List<TimeTableSetting>();
        public List<TT_DEPARTMENT_WISE_TEMPLATE> lstTimeTableDepartmentWiseTemplate = new List<TT_DEPARTMENT_WISE_TEMPLATE>();
        public List<TimeTableTemplate> lstTimeTableTemplate = new List<TimeTableTemplate>();
        public TimeTable SaveTimeTable = new TimeTable();
        public TimeTableSetting TimeTableSetting = new TimeTableSetting();
        public TimeTableTemplate TimeTableTemplate = new TimeTableTemplate();
        public TT_DEPARTMENT_WISE_TEMPLATE DepartmentWiseTimeTableTemplate = new TT_DEPARTMENT_WISE_TEMPLATE();
        [DisplayName("Class")]
        public SelectList ClassList { get; set; }
        [DisplayName("Staff")]
        public SelectList StaffList { get; set; }
        [DisplayName("Course")]
        public SelectList CourseList { get; set; }
        [DisplayName("Hour Type")]
        public SelectList HourType { get; set; }
        [DisplayName("Semester")]
        public SelectList SemesterList { get; set; }
        [DisplayName("Hour")]
        public SelectList HourList { get; set; }
        [DisplayName("Day Order")]
        public SelectList DayOrderList { get; set; }
        [DisplayName("Graduation Type")]
        public SelectList GraduationType { get; set; }
        [DisplayName("Department")]
        public SelectList Departmentlist { get; set; }
        [DisplayName("Setting")]
        public SelectList SettingList { get; set; }
        [DisplayName("Faculty")]
        public SelectList FacultyLIst { get; set; }
        [DisplayName("Paper Type")]
        public SelectList PaperTypeList { get; set; }
        [DisplayName("Year")]
        public SelectList ClassYearList { get; set; }
    }
}
