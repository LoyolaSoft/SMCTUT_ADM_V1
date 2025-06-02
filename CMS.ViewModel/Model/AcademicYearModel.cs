using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;


namespace CMS.ViewModel.Model
{
    #region Academic Year
    public class AcademicYearModel
    {
        public string ACADEMIC_YEAR_ID { get; set; }
        [DisplayName("Academic Year")]
        public string AC_YEAR { get; set; }
        [DisplayName("Date From")]
        public string DATE_FROM { get; set; }
        [DisplayName("Date To")]
        public string DATE_TO { get; set; }
        [DisplayName("Active Year")]
        public string ACTIVE_YEAR { get; set; }
        [DisplayName("Academic Name")]
        public string ACADEMIC_NAME { get; set; }
    }
    #endregion
    #region Course Root
    public class CourseRootModel
    {
        public string COURSE_ROOT_ID { get; set; }
        [DisplayName("Year From")]
        public string YEAR_FROM { get; set; }
        [DisplayName("Year To")]
        public string YEAR_TO { get; set; }
        [DisplayName("Department")]
        public SelectList DEPARTMENT { get; set; }
        [DisplayName("Year")]
        public string YEAR { get; set; }
        [DisplayName("Semester From")]
        public SelectList SEMESTER_FROM { get; set; }
        [DisplayName("Semester To")]
        public SelectList SEMESTER_TO { get; set; }
        [DisplayName("Part")]
        public SelectList PART { get; set; }
        [DisplayName("Course Type")]
        public SelectList COURSE_TYPE { get; set; }
        [DisplayName("Hours Per Week")]
        public string HRS_PER_WEEK { get; set; }
        [DisplayName("Credits")]
        public string CREDITS { get; set; }
        [DisplayName("Option Name")]
        public SelectList OPTION_NAME { get; set; }
        [DisplayName("Paper Code")]
        public SelectList PAPER_CODE { get; set; }
        [DisplayName("Course Title")]
        public string COURSE_TITLE { get; set; }
        [DisplayName("Course Code")]
        public string COURSE_CODE { get; set; }
        [DisplayName("Semester")]
        public SelectList SEMESTER { get; set; }
        [DisplayName("Is NME Subject")]
        public bool IS_NME_SUBJECT { get; set; }
        [DisplayName("Is Allied Subject")]
        public bool IS_ALLIED_SUBJECT { get; set; }
        [DisplayName("Course Order")]
        public string COURSE_ORDER { get; set; }
        [DisplayName("Subjects")]
        public string SUBJECTS { get; set; }
        [DisplayName("Is Compulsory Subject")]
        public bool IS_COMPULSORY_SUBJECT { get; set; }
        [DisplayName("UGC Course Type")]
        public SelectList UGC_COURSE_TYPE { get; set; }
        [DisplayName("Paper Type")]
        public SelectList PAPER_TYPE { get; set; }
        [DisplayName("Subject Type")]
        public SelectList SUBJECT_TYPE { get; set; }
    }
    public class CourseRootInfo
    {
        public string COURSE_ROOT_ID { get; set; }
        [DisplayName("Year From")]
        public string YEAR_FROM { get; set; }
        [DisplayName("Year To")]
        public string YEAR_TO { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT { get; set; }
        [DisplayName("Academic Year")]
        public string YEAR { get; set; }
        [DisplayName("Semester From")]
        public string SEMESTER_FROM { get; set; }
        [DisplayName("Semester To")]
        public string SEMESTER_TO { get; set; }
        [DisplayName("Part")]
        public string PART { get; set; }
        [DisplayName("Course Type")]
        public string COURSE_TYPE { get; set; }
        [DisplayName("Department")]
        public string HRS_PER_WEEK { get; set; }
        [DisplayName("Credits")]
        public string CREDITS { get; set; }
        [DisplayName("Option Name")]
        public string OPTION_NAME { get; set; }
        public string PAPER_CODE { get; set; }
        [DisplayName("Course Title")]
        public string COURSE_TITLE { get; set; }
        [DisplayName("Course Code")]
        public string COURSE_CODE { get; set; }
        [DisplayName("Semester")]
        public string SEMESTER { get; set; }
        [DisplayName("Is NME")]
        public string IS_NME_SUBJECT { get; set; }
        [DisplayName("Is Allied")]
        public string IS_ALLIED_SUBJECT { get; set; }
        public string COURSE_ORDER { get; set; }
        public string SUBJECTS { get; set; }
        [DisplayName("Is Compulsory")]
        public string IS_COMPULSORY_SUBJECT { get; set; }
        public string UGC_COURSE_TYPE { get; set; }
        public string PAPER_TYPE { get; set; }
        public string SUBJECT_TYPE { get; set; }
    }
    #endregion
    #region Course Wise Staff

    public class CourseWiseStaffModel
    {
        [DisplayName("Department")]
        public SelectList DEPARTMENT { get; set; }
        [DisplayName("Class")]
        public SelectList CLASS_ID { get; set; }
        [DisplayName("Class Type")]
        public SelectList CLASS_TYPE { get; set; }
        [DisplayName("Course")]
        public SelectList COURSE_ID { get; set; }
        [DisplayName("Staff Name")]
        public SelectList STAFF_ID { get; set; }
        [DisplayName("Hourse Per Week")]
        public string HRS_WEEK { get; set; }
        [DisplayName("Hours Per Term")]
        public string HRS_TERM { get; set; }
        [DisplayName("Shift")]
        public SelectList SHIFT { get; set; }
        [DisplayName("Semester")]
        public SelectList SEMESTER_ID { get; set; }
    }

    public class CourseWiseStaffList
    {
        public string CLASS_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string STAFF_ID { get; set; }
        public string SHIFT { get; set; }
        public string SEMESTER_ID { get; set; }
    }

    public class JsonCourseWiseStaff
    {
        public List<CourseWiseStaffList> CourseWiseStaffList { get; set; }
    }
    public class CourseWiseStaffInfo
    {
        public string CLSCRSSTF_ID { get; set; }
        [DisplayName("Class")]
        public string CLASS_NAME { get; set; }
        public string CLASS_ID { get; set; }
        [DisplayName("Course")]
        public string COURSE_ID { get; set; }
        [DisplayName("Course Code")]
        public string COURSE_CODE { get; set; }
        [DisplayName("Course Title")]
        public string COURSE_TITLE { get; set; }
        [DisplayName("Staff Name")]
        public string STAFF_ID { get; set; }
        public string STAFF_COUNT { get; set; }
        public string STF_ALLOTMENT_TYPE { get; set; }
        public string HRS_WEEK { get; set; }
        public string HRS_TERM { get; set; }
        [DisplayName("Shift")]
        public string SHIFT { get; set; }
        public string IS_PRIMARY_STAFF { get; set; }
        [DisplayName("Semester")]
        public string SEMESTER_ID { get; set; }
        public string SEMESTER { get; set; }
        public string PROGRAMME { get; set; }
    }
    #endregion
    #region AcademicCurriculumModel
    public class AcademicBatches
    {
        public string AC_BATCH_ID { get; set; }
        public string BATCH_ID { get; set; }
        public string ACADEMIC_YEAR_ID { get; set; }
        public string BATCH_YEAR { get; set; }

    }

    public class AcademicCurriculumModel
    {
        [DisplayName("Programme")]
        public SelectList PROGRAMME { get; set; }
        [DisplayName("Batch")]
        public SelectList BATCH { get; set; }
        [DisplayName("Course List")]
        public SelectList COURSE_LIST { get; set; }
        [DisplayName("Serial Number")]
        public SelectList S_NO { get; set; }
        [DisplayName("Semester")]
        public SelectList SEMESTER { get; set; }
        [DisplayName("IS ACTIVE")]
        public bool IS_ACTIVE { get; set; }
    }
    public class AcademicCurriculumInfo
    {
        public string CURRICULUM_ID { get; set; }
        [DisplayName("Programme")]
        public string PROGRAMME { get; set; }
        [DisplayName("Batch")]
        public string BATCH { get; set; }
        [DisplayName("Course Title")]
        public string COURSE_ID { get; set; }
        [DisplayName("Serial Number")]
        public string S_NO { get; set; }
        [DisplayName("Semester")]
        public string SEMESTER { get; set; }
        //[DisplayName("Batch")]
        //public SelectList sBATCH { get; set; }
    }

    public class SaveAcademicCurriculumInfo
    {

        public string BATCH { get; set; }
        public string PROGRAMME { get; set; }
        public string COURSE_ID { get; set; }
        public string IS_ACTIVE { get; set; }
    }
    public class CurriculumCourse
    {
        public string BATCH { get; set; }
        public string PROGRAMME { get; set; }
        public string COURSE_ID { get; set; }
        public string IS_ACTIVE { get; set; }
    }
    public class JSonCurriculumCourse
    {
        public List<CurriculumCourse> CurriculumCourses { get; set; }
    }

    #endregion
    #region Semester Root
    public class JsonSemesterRoot
    {
        public List<SemesterRootInfo> SemesterRoot { get; set; }
    }
    public class SemesterRootModel
    {
        [DisplayName("Semester")]
        public string SEMESTER_ID { get; set; }
        [DisplayName("Batch")]
        public SelectList BATCH { get; set; }
        [DisplayName("Programme")]
        public SelectList PROGRAMME { get; set; }
        [DisplayName("Semester")]
        public SelectList SEMESTER { get; set; }
        [DisplayName("Date From")]
        public string DATE_FROM { get; set; }
        [DisplayName("Date To")]
        public string DATE_TO { get; set; }
        [DisplayName("Active Semester")]
        public bool IS_ACTIVE { get; set; }

    }
    public class SemesterRootInfo
    {
        [DisplayName("Semester")]
        public string SEMESTER_ID { get; set; }
        [DisplayName("Batch")]
        public string BATCH { get; set; }
        [DisplayName("Programme")]
        public string PROGRAMME { get; set; }
        [DisplayName("Semester")]
        public string SEMESTER { get; set; }
        [DisplayName("Date From")]
        public string DATE_FROM { get; set; }
        [DisplayName("Date To")]
        public string DATE_TO { get; set; }
        [DisplayName("Active Semester")]
        public string IS_ACTIVE { get; set; }
        public string ACC_SEMESTER_ID { get; set; }


    }
    #endregion
    #region ClassCourse
    public class ClassCourse
    {
        public string CLASS_COURSE_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string COURSE_ID { get; set; }
        [DisplayName("Course Type")]
        public string COURSE_TYPE { get; set; }
        [DisplayName("Course Title")]
        public string COURSE_TITLE { get; set; }
        [DisplayName("Course Code")]
        public string COURSE_CODE { get; set; }
        [DisplayName("Is NME")]
        public string IS_NME_SUBJECT { get; set; }
        [DisplayName("Is Allied")]
        public string IS_ALLIED_SUBJECT { get; set; }
        [DisplayName("Class Name")]
        public string CLASS_NAME { get; set; }
        [DisplayName("Shift")]
        public string SHIFT_NAME { get; set; }
        [DisplayName("Semester")]
        public string SEMESTER_NAME { get; set; }

    }
    public class ClassCourseDDL
    {
        public string sClassesList { get; set; }
        public string sCourseList { get; set; }
        public string sClassType { get; set; }

    }

    public class JsonClassCourse
    {
        public string ClassCourseId { get; set; }
        public string sClassId { get; set; }
        public string sCourseId { get; set; }
    }

    public class JsonClassCourseRoot
    {
        public List<JsonClassCourse> JsonClassCourse { get; set; }
        public string sAcademicYear { get; set; }
    }
    #endregion

    #region TestModel

    public class ACADEMIC_YEAR
    {
        public string ACADEMIC_YEAR_ID { get; set; }
        public string AC_YEAR { get; set; }
        public string DATE_FROM { get; set; }
        public string DATE_TO { get; set; }
        public string ACTIVE_YEAR { get; set; }
        public string ACADEMIC_NAME { get; set; }


    }
    #endregion
    #region Department
    public class DepartmentModel
    {
        public string DEPARTMENT_ID { get; set; }
        [DisplayName("Department Code")]
        public string DEPARTMENT_CODE { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT { get; set; }
        [DisplayName("Department Order")]
        public string DEPARTMENT_ORDER { get; set; }
        [DisplayName("Faculty")]
        public SelectList FACULTY { get; set; }
        [DisplayName("Year Of Publishment")]
        public string YEAR_OF_PUBLISHMENT { get; set; }
        [DisplayName("Is Active")]
        public bool IS_ACTIVE { get; set; }
    }
    public class DepartmentInfo
    {
        public string DEPARTMENT_ID { get; set; }
        [DisplayName("Department Code")]
        public string DEPARTMENT_CODE { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT { get; set; }
        [DisplayName("Department Order")]
        public string DEPARTMENT_ORDER { get; set; }
        [DisplayName("Faculty")]
        public string FACULTY { get; set; }
        [DisplayName("Year Of Publishment")]
        public string YEAR_OF_PUBLISHMENT { get; set; }
        [DisplayName("Is Active")]
        public string IS_ACTIVE { get; set; }
    }
    #endregion
    #region Faculty
    public class FacultyModel
    {
        public string FACULTY_ID { get; set; }
        [DisplayName("Faculty Code")]
        public string FACULTY_CODE { get; set; }
        [DisplayName("Faculty")]
        public string FACULTY { get; set; }
        [DisplayName("Is Active")]
        public bool IS_ACTIVE { get; set; }
        [DisplayName("Faculty Order")]
        public string FACULTY_ORDER { get; set; }
        [DisplayName("Faculty Series")]
        public string FAC_SERIES { get; set; }
    }
    public class FacultyInfo
    {
        public string FACULTY_ID { get; set; }
        [DisplayName("Faculty")]
        public string FACULTY { get; set; }
        [DisplayName("Faculty Code")]
        public string FACULTY_CODE { get; set; }
        [DisplayName("Is Active")]
        public string IS_ACTIVE { get; set; }
        [DisplayName("Faculty Order")]
        public string FACULTY_ORDER { get; set; }
        [DisplayName("Faculty Series")]
        public string FAC_SERIES { get; set; }
    }
    #endregion
    #region Class
    public class ClassModel
    {
        public string CLASS_ID { get; set; }
        [DisplayName("Class Code")]
        public string CLASS_CODE { get; set; }
        [DisplayName("Class Name")]
        public string CLASS_NAME { get; set; }
        [DisplayName("Class Description")]
        public string CLASS_DESCRIPTION { get; set; }
        [DisplayName("Class Type")]
        public SelectList CLASS_TYPE { get; set; }
        [DisplayName("Class Level")]
        public SelectList CLASS_LEVEL { get; set; }
        [DisplayName("Class Year")]
        public SelectList CLASS_YEAR { get; set; }
        [DisplayName("Programme")]
        public SelectList PROGRAMME { get; set; }
        [DisplayName("Department")]
        public SelectList DEPARTMENT { get; set; }
        [DisplayName("Language")]
        public SelectList LANGUAGE { get; set; }
        [DisplayName("Is Choice")]
        public bool IS_CHOICE { get; set; }
        [DisplayName("Class Order")]
        public string CLASS_ORDER { get; set; }
        //[DisplayName("Class Promotion Group")]
        //public string CLS_PROMOTION_GROUP { get; set; }
        //[DisplayName("Class promotion Levl")]
        //public string CLS_PROMOTION_LEVEL { get; set; }
        [DisplayName("Shift")]
        public SelectList SHIFT { get; set; }
        [DisplayName("Course Type")]
        public SelectList COURSE_TYPE { get; set; }
        [DisplayName("Course")]
        public SelectList COURSE_ID { get; set; }
    }
    public class ClassInfo
    {
        public string CLASS_ID { get; set; }
        [DisplayName("Class Code")]
        public string CLASS_CODE { get; set; }
        [DisplayName("Class Name")]
        public string CLASS_NAME { get; set; }
        [DisplayName("Class Description")]
        public string CLASS_DESCRIPTION { get; set; }
        [DisplayName("Class Type")]
        public string CLASS_TYPE { get; set; }
        [DisplayName("Class Level")]
        public string CLASS_LEVEL { get; set; }
        [DisplayName("Class Year")]
        public string CLASS_YEAR { get; set; }
        [DisplayName("Programme")]
        public string PROGRAMME { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT { get; set; }
        [DisplayName("Language")]
        public string LANGUAGE { get; set; }
        [DisplayName("Is Choice")]
        public string IS_CHOICE { get; set; }
        [DisplayName("Class Order")]
        public string CLASS_ORDER { get; set; }
        //[DisplayName("Class Promotion Group")]
        // public string CLS_PROMOTION_GROUP { get; set; }
        //[DisplayName("Class promotion Levl")]
        //public string CLS_PROMOTION_LEVEL { get; set; }
        [DisplayName("Shift")]
        public string SHIFT { get; set; }
        [DisplayName("Course Type")]
        public string COURSE_TYPE { get; set; }
        [DisplayName("Course")]
        public string COURSE_ID { get; set; }
    }
    #endregion
    #region Batch
    public class JsonBatchInfo
    {
        public List<BatchInfo> BatchList { get; set; }
    }
    public class BatchModel
    {
        public string AC_BATCH_ID { get; set; }
        [DisplayName("Batch")]
        public SelectList BATCH_ID { get; set; }
        [DisplayName("Academic Year")]
        public SelectList ACADEMIC_YEAR_ID { get; set; }
    }
    public class BatchInfo
    {
        public string AC_BATCH_ID { get; set; }
        [DisplayName("Batch")]
        public string BATCH_ID { get; set; }
        [DisplayName("Academic Year")]
        public string ACADEMIC_YEAR_ID { get; set; }
    }
    #endregion
    #region Course Type
    public class CourseTypeModel
    {
        public string COURSE_TYPE_ID { get; set; }
        [DisplayName("Course Type")]
        public string COURSE_TYPE { get; set; }
        [DisplayName("No Of Components")]
        public string NO_OF_COMPONENTS { get; set; }
        [DisplayName("Course Type Order")]
        public string COURSE_TYPE_ORDER { get; set; }
        [DisplayName("Is Stu Based Selection")]
        public bool IS_STU_BASED_SELECTION { get; set; }
        [DisplayName("Internal")]
        public string INTERNAL { get; set; }
        [DisplayName("External")]
        public string EXTERNAL { get; set; }
        [DisplayName("Total")]
        public string TOTAL { get; set; }
        [DisplayName("Hours")]
        public string HOURS { get; set; }
        [DisplayName("Part")]
        public SelectList PART_ID { get; set; }
        [DisplayName("Programme Level")]
        public SelectList PROGRAMME_LEVEL { get; set; }
        [DisplayName("Credits")]
        public string CREDITS { get; set; }
    }
    public class CourseTypeInfo
    {
        public string COURSE_TYPE_ID { get; set; }
        [DisplayName("Course Type")]
        public string COURSE_TYPE { get; set; }
        [DisplayName("No Of Components")]
        public string NO_OF_COMPONENTS { get; set; }
        [DisplayName("Course Type Order")]
        public string COURSE_TYPE_ORDER { get; set; }
        [DisplayName("Is Stu Based Selection")]
        public string IS_STU_BASED_SELECTION { get; set; }
        [DisplayName("Internal")]
        public string INTERNAL { get; set; }
        [DisplayName("External")]
        public string EXTERNAL { get; set; }
        [DisplayName("Total")]
        public string TOTAL { get; set; }
        [DisplayName("Hours")]
        public string HOURS { get; set; }
        [DisplayName("Part")]
        public string PART_ID { get; set; }
        [DisplayName("Programme Level")]
        public string PROGRAMME_LEVEL { get; set; }
        [DisplayName("Credits")]
        public string CREDITS { get; set; }
    }
    #endregion
    #region Programme
    public class ProgrammeModel
    {
        public string PROGRAMME_ID { get; set; }
        [DisplayName("Programme Code")]
        public string PROGRAMME_CODE { get; set; }
        [DisplayName("Programme Name")]
        public string PROGRAMME_NAME { get; set; }
        [DisplayName("Programme Description")]
        public string PROGRAMME_DESCRIPTION { get; set; }
        [DisplayName("Department")]
        public SelectList DEPARTMENT { get; set; }
        [DisplayName("Degree")]
        public SelectList DEGREE { get; set; }
        [DisplayName("Programme Order")]
        public string PROGRAMME_ORDER { get; set; }
        [DisplayName("Duration")]
        public string DURATION { get; set; }
        [DisplayName("Duration Unit")]
        public SelectList DURATION_UNIT { get; set; }
        [DisplayName("No Of Semester")]
        public string NO_OF_SEMESTER { get; set; }
        [DisplayName("Channel")]
        public SelectList CHANNEL { get; set; }
        [DisplayName("Is Aided")]
        public bool IS_AIDED { get; set; }
        [DisplayName("Programme Series Roll No")]
        public string PRO_SERIES_ROLLNO { get; set; }
        [DisplayName("Non Aided")]
        public bool NON_AIDED { get; set; }
        [DisplayName("Is Regular")]
        public bool IS_REGULAR { get; set; }
        [DisplayName("Subjects")]
        public SelectList SUBJECTS { get; set; }
        [DisplayName("Programme Series Register Number")]
        public string PRO_SERIES_REGNO { get; set; }
        [DisplayName("Programme Series Admission Number")]
        public string PRO_SERIES_ADMNO { get; set; }
        [DisplayName("Medium Of Instruction")]
        public SelectList MEDIUM_OF_INSTRACTION { get; set; }
    }
    public class ProgrammeInfo
    {
        public string PROGRAMME_ID { get; set; }
        [DisplayName("Programme Code")]
        public string PROGRAMME_CODE { get; set; }
        [DisplayName("Programme Name")]
        public string PROGRAMME_NAME { get; set; }
        [DisplayName("Programme Description")]
        public string PROGRAMME_DESCRIPTION { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT { get; set; }
        [DisplayName("Degree")]
        public string DEGREE { get; set; }
        [DisplayName("Programme Order")]
        public string PROGRAMME_ORDER { get; set; }
        [DisplayName("Duration")]
        public string DURATION { get; set; }
        [DisplayName("Duration Unit")]
        public string DURATION_UNIT { get; set; }
        [DisplayName("No Of Semester")]
        public string NO_OF_SEMESTER { get; set; }
        [DisplayName("Channel")]
        public string CHANNEL { get; set; }
        [DisplayName("Is Aided")]
        public string IS_AIDED { get; set; }
        [DisplayName("Programme Series Roll No")]
        public string PRO_SERIES_ROLLNO { get; set; }
        [DisplayName("Non Aided")]
        public string NON_AIDED { get; set; }
        [DisplayName("Is Regular")]
        public string IS_REGULAR { get; set; }
        [DisplayName("Subjects")]
        public string SUBJECTS { get; set; }
        [DisplayName("Programme Series Register Number")]
        public string PRO_SERIES_REGNO { get; set; }
        [DisplayName("Programme Series Admission Number")]
        public string PRO_SERIES_ADMNO { get; set; }
        [DisplayName("Medium Of Instruction")]
        public string MEDIUM_OF_INSTRACTION { get; set; }
    }
    #endregion
    #region Degree
    public class DegreeModel
    {
        public string DEGREE_ID { get; set; }
        [DisplayName("Degree")]
        public string DEGREE { get; set; }
        [DisplayName("Programme Type")]
        public SelectList PROGRAMME_TYPE { get; set; }
        [DisplayName("Programme Level")]
        public SelectList PROGRAMME_LEVEL { get; set; }
        [DisplayName("Degree Order")]
        public string DEGREE_ORDER { get; set; }
        [DisplayName("Faculty")]
        public SelectList FACULTY { get; set; }
        [DisplayName("BOID Name")]
        public string BOS_NAME { get; set; }
        [DisplayName("Prefix")]
        public string PREFIX { get; set; }
        [DisplayName("BOID Series Roll Number")]
        public string BOS_SERIES_ROLLNO { get; set; }
        [DisplayName("BOID Series Register Number")]
        public string BOS_SERIES_REGNO { get; set; }
        [DisplayName("BOID Series Admission Number")]
        public string BOS_SERIES_ADMNO { get; set; }
    }
    public class DegreeInfo
    {
        public string DEGREE_ID { get; set; }
        [DisplayName("Degree")]
        public string DEGREE { get; set; }
        [DisplayName("Programme Type")]
        public string PROGRAMME_TYPE { get; set; }
        [DisplayName("Programme Level")]
        public string PROGRAMME_LEVEL { get; set; }
        [DisplayName("Degree Order")]
        public string DEGREE_ORDER { get; set; }
        [DisplayName("Faculty")]
        public string FACULTY { get; set; }
        [DisplayName("BOID Name")]
        public string BOS_NAME { get; set; }
        [DisplayName("Prefix")]
        public string PREFIX { get; set; }
        [DisplayName("BOID Series Roll Number")]
        public string BOS_SERIES_ROLLNO { get; set; }
        [DisplayName("BOID Series Register Number")]
        public string BOS_SERIES_REGNO { get; set; }
        [DisplayName("BOID Series Admission Number")]
        public string BOS_SERIES_ADMNO { get; set; }
    }
    #endregion
}
