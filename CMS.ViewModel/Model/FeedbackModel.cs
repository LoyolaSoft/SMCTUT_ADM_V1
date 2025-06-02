using System.ComponentModel.DataAnnotations;

namespace CMS.ViewModel.Model
{
    public class FBCLASS_WISE_STAFF
    {
        public string FB_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public string STAFF_ID { get; set; }
        public string FEEDBACK_ID { get; set; }
        public string STAFF_NAME { get; set; }


    }
    public class StudentCourseList
    {
        public string COURSE_ID { get; set; }
        public string COURSE_TITLE { get; set; }

    }
    public class Department
    {
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT { get; set; }

    }

    public class Shift
    {
        public string SHIFT_ID { get; set; }
        public string SHIFT { get; set; }

    }

    public class FBStaffList
    {
        public string STAFF_ID { get; set; }
        public string STAFF_NAME { get; set; }
    }
    public class FBObjective
    {
        public string OBJECTIVE_ID { get; set; }
        public string OBJECTIVE_NAME { get; set; }
        public string OBJECTIVE_DESC { get; set; }
    }
    public class FBCLASSES
    {
        public string FBCLASSID { get; set; }
        public string CLASS_ID { get; set; }
        public string FEEDBACK_ID { get; set; }
    }

    public class FBEVALUATION
    {
        public string FBSTUDENT_ID { get; set; }
        public string ASSESSOR { get; set; }
        public string CLASS_ID { get; set; }
        public string ASSESSEE { get; set; }
        public string FEEDBACK_ID { get; set; }
        public string QUESTION { get; set; }
        public string ANSWER { get; set; }
        public string ISDELETED { get; set; }
    }

    public class FBFEEDBACK_QUESTIONS
    {
        [Required(ErrorMessage = "Required !!!")]
        public string QUESTION_ID { get; set; }
        public string FEEDBACK_ID { get; set; }
        public string QUESTION { get; set; }
        public string QUESTION_TYPE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class FBFEEDBACK_SETTINGS
    {
        public string FEEDBACK_ID { get; set; }
        public string FEEDBACK_NAME { get; set; }
        public string DATE_FROM { get; set; }
        public string DATE_TO { get; set; }
        public string QUESTION_TYPE_ID { get; set; }
        public string NO_OF_QUESTION { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_DELETED { get; set; }
        public string CLASS_ID { get; set; }
        public string IS_ACTIVE { get; set; }
        public string FEEDBACK_TYPE { get; set; }
        public string ASSESSOR { get; set; }
        public string ASSESSEE { get; set; }
        public string MANUAL_STAFF_SELECTION { get; set; }
        public string WITH_IN_DEPARTMENT { get; set; }
        public string ASSESSOR_CATEGORY { get; set; }
        public string ASSESSEE_CATEGORY { get; set; }
    }

    public class FBOBJECTIVES
    {
        public string OBJECTIVE_ID { get; set; }
        public string QUESTION_TYPE { get; set; }
        public string OBJECTIVES { get; set; }
        public string IS_DELETED { get; set; }
        public string RATING { get; set; }
        public string OBJECTIVESHORTTERM { get; set; }
        public string OBJECTIVE_ORDER { get; set; }
    }

    public class FBQUESTIONS_GROUP
    {
        public string QUESTION_GROUP_ID { get; set; }
        public string GROUP_TITLE { get; set; }
        public string IS_DELETED { get; set; }

    }

    public class FBSTUDENT_ENTRY
    {
        public string STUDENT_ENTRY_ID { get; set; }
        public string ENTRY_STATUS { get; set; }
        public string STUDENT_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public string IS_DELETED { get; set; }
        public string FEEDBACK_ID { get; set; }
    }
    public class FBCLASSES_BY_STAFF
    {
        public string CLASS_ID { get; set; }
        public string CLASS_DESCRIPTION { get; set; }
    }


    public class FBRATING_BY_STAFF
    {
        public string ASSESSOR { get; set; }
        public string QUESTION { get; set; }
        public string GROUP_TITLE { get; set; }
        public string ANSWER { get; set; }
        [Display(Name = "VERY GOOD")]
        public string VERY_GOOD { get; set; }
        public string GOOD { get; set; }
        public string SATISFACTORY { get; set; }
        public string POOR { get; set; }
        [Display(Name = "VERY POOR")]
        public string VERY_POOR { get; set; }

    }

    public class FBCOURSE_AND_CLASS_INFO
    {
        public string TOTAL_STUDENTS { get; set; }
        public string COURSE_TITLE { get; set; }
        public string CLASS_NAME { get; set; }
        public string COURSE_TYPE { get; set; }
        public string COURSE_CODE { get; set; }
        public string DEPARTMENT { get; set; }
    }


}
