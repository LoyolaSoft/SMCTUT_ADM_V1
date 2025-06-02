using System.Collections.Generic;
using System.ComponentModel;

namespace CMS.ViewModel.Model
{
    public class StaffInfoForStudentAttendance
    {
        public string STAFF_ID { get; set; }
        public string STAFFNO { get; set; }
        public string STAFF_CODE { get; set; }
    }

    public class TT_TIMETABLE_2017
    {
        public string TIME_TABLE_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string DAY_ORDER_ID { get; set; }
        public string STAFF_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string HOUR_ID { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string SEMESTER_ID { get; set; }
        public string ATTENDANCE_DATE { get; set; }

    }

    public class ATT_ABSENTEE_LIST_2017
    {
        public string ATTENDANCE_ID { get; set; }
        public string ATTENDENCE_DATE { get; set; }
        public string STAFF_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string HOUR_FROM { get; set; }
        public string HOUR_TO { get; set; }
        public string STUDENT_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string ENTRY_ID { get; set; }
        public string ENTRY_DATE { get; set; }
        public string IS_DELETED { get; set; }
        public string S_CLASS_ID { get; set; }
        public string IS_PROCESSED { get; set; }
        public string ATTENDANCE_TYPE_ID { get; set; }
        public string REASON_ID { get; set; }
    }
    public class ABSENTEE_LIST
    {
        public List<ATT_ABSENTEE_LIST_2017> ATT_ABSENTEE_LIST { get; set; }
    }
    public class ATT_AUTHORIZED_ABSENCE_2017
    {

        public string ABSENT_ID { get; set; }
        public string SUBMISSION_DATE { get; set; }
        public string STUDENT_ID { get; set; }
        public string ABSENCE_TYPE { get; set; }
        public string HOUR_FROM { get; set; }
        public string DATE_FROM { get; set; }
        public string HR_TO { get; set; }
        public string DATE_TO { get; set; }
        public string REASON { get; set; }
        public string APPROVER_ID { get; set; }
        public string IS_PROCESSED { get; set; }
    }
    public class ATT_OVERALL_ATTENDANCE_2017
    {
        public string ATT_ID { get; set; }
        public string ATT_DATE { get; set; }
        public string STUDENT_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string ABS_HRS { get; set; }
        public string LEAVE_HRS { get; set; }
        public string LONG_ABS_HRS { get; set; }
        public string PERMIT_HRS { get; set; }
        public string OD_HRS { get; set; }
        public string SUSPEND_HRS { get; set; }
    }

    public class CALENDER_2017
    {
        public string DAY_ID { get; set; }
        public string DAY_ORDER_DATE { get; set; }
        public string DAY_TYPE { get; set; }
        public string DAY_ORDER { get; set; }
        public string DAY_ORDER_MONTH { get; set; }
        public string IS_HOLIDAY { get; set; }
        public string IS_DELETED { get; set; }
        public string DAY_ORDER_END_DATE { get; set; }
    }

    public class OVER_ALL_ATTENDANCE
    {
        public string AttendanceDate { get; set; }
        public string ClassId { get; set; }
        public string HourId { get; set; }
        public string StaffId { get; set; }
        public string Flag { get; set; }

    }
    public class ATTENDANCE_ENTRY_MODEL
    {
        public string ATTENDANCE_ID { get; set; }
        [DisplayName("ROLL NO")]
        public string ROLL_NO { get; set; }
        [DisplayName("REGISTER NO")]
        public string REGISTER_NO { get; set; }
        [DisplayName("NAME")]
        public string FIRST_NAME { get; set; }
        [DisplayName("COURSE CODE")]
        public string COURSE_CODE { get; set; }
        public string COURSE_ID { get; set; }
        [DisplayName("COURSE ")]
        public string COURSE_TITLE { get; set; }
        public string S_CLASS_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string ENTRY_ID { get; set; }
        public string TYPE { get; set; }
        public string SPECIAL_EVENT_ID { get; set; }
        [DisplayName("ENTRY DATE")]
        public string ENTRY_DATE { get; set; }
        public string REASON { get; set; }
        [DisplayName("CLASS NAME")]
        public string CLASS_NAME { get; set; }
        public string REASON_ID { get; set; }
        public string ATTENDANCE_TYPE_ID { get; set; }
        public string STUDENT_ID { get; set; }
        [DisplayName("HOUR")]

        public string HOUR_FROM { get; set; }
        [DisplayName("ATTENDENCE DATE")]
        public string ATTENDENCE_DATE { get; set; }

    }
}
