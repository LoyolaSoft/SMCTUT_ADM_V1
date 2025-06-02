using System.Collections.Generic;

namespace CMS.ViewModel.Model
{

    public class PROGRAMME_HEADS_AMOUNT
    {
        public string HEAD { get; set; }
        public string HEAD_CODE { get; set; }
        public string HEAD_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string BATCH_ID { get; set; }
        public string AMOUNT { get; set; }
        public string PROGRAMME_HEAD_ID { get; set; }
        public string SEMESTER { get; set; }
        public string SUBJECT_TYPE { get; set; }

    }
    public class EXAM_COURSE_WISE_HEADS
    {
        public string COURSE_HEAD_ID { get; set; }
        public string COURSE_CODE { get; set; }
        public string PROGRAMME_HEAD_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string BATCH_ID { get; set; }
        public string BATCH { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string SEMESTER { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string AMOUNT { get; set; }
        public string ACCOUNT { get; set; }
        public string HEAD { get; set; }
        public string EXAM_SETTING_ID { get; set; }
        public string HEAD_ID { get; set; }

    }
    public class JsonProgrammeHeads
    {
        public List<EXAM_PROGRAMME_WISE_HEADS> ProgrammeHeads { get; set; }

    }
    public class EXAM_PROGRAMME_WISE_HEADS
    {
        public string PROGRAMME_HEAD_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string BATCH_ID { get; set; }
        public string AMOUNT { get; set; }
        public string HEAD_ID { get; set; }
        public string IS_USED { get; set; }
        public string IS_DELETED { get; set; }
        public string SEMESTER { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string HEAD { get; set; }
        public string STATUS { get; set; }
        public string PROGRAMME { get; set; }
        public string HEAD_CODE { get; set; }
        public string ACCOUNT { get; set; }
        public string SUBJECT_TYPE { get; set; }
        public string BATCH { get; set; }
        public string EXAM_SETTING_ID { get; set; }


    }

    public class CourseList
    {

        public string COURSE_ID { get; set; }
        public string COURSE_ROOT_ID { get; set; }
        public string COURSE_TITLE { get; set; }
        public string COURSE_CODE { get; set; }
        public string STAFF_ID { get; set; }
        public string STUDENT_ID { get; set; }
    }

    public class CourseInfo
    {
        public string COURSE_ROOT_ID { get; set; }
        public string COURSE_TITLE { get; set; }
        public string COURSE_CODE { get; set; }
        public string COURSE_TYPE { get; set; }
        public string TOTAL_STUDENT { get; set; }
        public string COURSE_TYPE_ID { get; set; }
        public string CLASS_NAME { get; set; }
        public string CLASS_ID { get; set; }
        public string SEMESTER_ID { get; set; }
    }
    public class CourseComponents
    {
        public string CIA_GROUP_COMPONENT_ID { get; set; }
        public string MAX_MARK { get; set; }
        public string COMPONENT { get; set; }
        public string CIA_GROUP_ID { get; set; }
        public string COURSE_TYPE_ID { get; set; }
        public string GROUP_MARK { get; set; }
    }
    public class CIAMarks
    {
        public string COURSE_ROOT_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string NAME { get; set; }
        public string REGISTER_NO { get; set; }
        public string CIA_C1 { get; set; }
        public string CIA_C2 { get; set; }
        public string CIA_C3 { get; set; }
        public string CIA_C4 { get; set; }
        public string CIA_C5 { get; set; }
        public string CIA_C6 { get; set; }
        public string CIA_C7 { get; set; }
        public string CIA_C8 { get; set; }
        public string CIA_TOTAL { get; set; }
        public string MARK_ID { get; set; }
        public string CLASS_ID { get; set; }

    }
    public class ClassList
    {
        public string CLASS_ID { get; set; }
        public string CLASS_NAME { get; set; }
        public string STAFF_ID { get; set; }
        public string STUDENT_ID { get; set; }
    }
    public class CIA_COMPONENT_MARK
    {
        public string CIA_GORUP_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string CIA_GROUP_MARKS { get; set; }
        public string COMPONENT_MARK_ID { get; set; }
    }
    public class JSON_CIA_COMPONENT_MARKS
    {
        public List<CIA_COMPONENT_MARK> CIA_COMPONENT_MARKS { get; set; }
    }
    public class CIA_MARKS
    {
        public string CLASS_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string COURSE_ROOT_ID { get; set; }
        public string CIA_C1 { get; set; }
        public string CIA_C2 { get; set; }
        public string CIA_C3 { get; set; }
        public string CIA_C4 { get; set; }
        public string CIA_C5 { get; set; }
        public string CIA_C6 { get; set; }
        public string CIA_C7 { get; set; }
        public string CIA_C8 { get; set; }
        public string SEMESTER_ID { get; set; }
        public string CIA_TOTAL { get; set; }
        public string COURSE_ID { get; set; }
        public string MARK_ID { get; set; }
    }
    public class JSON_CIA_MARKS
    {
        public List<CIA_MARKS> CIA_MARKS { get; set; }
        public List<SAVE_CIA_MARKS> SAVE_CIA_MARKS { get; set; }
    }
    public class SAVE_CIA_MARKS
    {
        public string CIA_GROUP_MARK_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string COURSE_GROUP_ID { get; set; }
        public string COURSE_GROUP_MARK { get; set; }
        public string SEMESTER_ID { get; set; }
    }
    public class CIA_TOTAL_MARKS
    {
        public string ADMISSION_NO { get; set; }
        public string REGISTER_NO { get; set; }
        public string NAME { get; set; }
        public string CIA_TOTAL { get; set; }
    }
    public class CIA_COMPONENTS_MARK
    {

        public string ADMISSION_NO { get; set; }
        public string REGISTER_NO { get; set; }
        public string NAME { get; set; }
        public string CIA_C1 { get; set; }
        public string CIA_C2 { get; set; }
        public string CIA_C3 { get; set; }
        public string CIA_C4 { get; set; }
        public string CIA_C5 { get; set; }
        public string CIA_C6 { get; set; }
        public string CIA_C7 { get; set; }
        public string CIA_C8 { get; set; }
        public string CIA_TOTAL { get; set; }
    }
    public class CIA_COURSE_AND_CLASS_INFO
    {
        public string DEPARTMENT { get; set; }
        public string CLASS_NAME { get; set; }
        public string COURSE_CODE { get; set; }
        public string SEMESTER_ID { get; set; }
        public string COURSE_TITLE { get; set; }
        public string STAFF_NAME { get; set; }
        public string STAFF_ID { get; set; }
        public string COURSE_TYPE { get; set; }
        public string INTERNAL { get; set; }
    }

    // CIA Entry By Jeevan ... 
    public class CIA_FETCH_COURSE_INFO
    {
        public string COURSE_ROOT_ID { get; set; }
        public string COURSE_TITLE { get; set; }
        public string COURSE_CODE { get; set; }
        public string TOTAL_STUDENT { get; set; }
        public string COURSE_TYPE_ID { get; set; }
        public string COURSE_TYPE { get; set; }
        public string CLASS_NAME { get; set; }
        public string SEMESTER_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string DATE_FROM { get; set; }
        public string DATE_TO { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string COURSE_ID { get; set; }

    }
    public class CIA_MARKS_INFO
    {
        public string CIA_GROUP_MARK_ID { get; set; }
        public string NAME { get; set; }
        public string REGISTER_NO { get; set; }
        public string ADMISSION_NO { get; set; }
        public string COURSE_ROOT_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string MAX_MARK { get; set; }
        public string ORDER_ID { get; set; }
        public string CIA_GROUP_ID { get; set; }
        public string COMPONENT_ID { get; set; }
        public string COMPONENT { get; set; }
        public string COURSE_GROUP_MARK { get; set; }
        public string SEMESTER_ID { get; set; }
        public string COURSE_GROUP_ID { get; set; }
    }
    public class CIA_COMPONENT
    {
        public string CIA_GROUP_COMPONENT_ID { get; set; }
        public string MAX_MARK { get; set; }
        public string COMPONENT { get; set; }
        public string COURSE_TYPE_ID { get; set; }
        public string GROUP_MARK { get; set; }
        public string CIA_GROUP_ID { get; set; }
        public string ORDER_ID { get; set; }
        public string INTERNAL { get; set; }
        public string DISABLED { get; set; }
    }
    public class CIA_TOTAL
    {
        public string STUDENT_ID { get; set; }
        public string TOTAL { get; set; }
        public string COURSE_ID { get; set; }
        public string ADMISSION_NO { get; set; }
        public string REGISTER_NO { get; set; }
        public string NAME { get; set; }
    }

    // CIA Marks List ..
    public class CIA_STAFF_INFO
    {
        public string DEPARTMENT { get; set; }
        public string CLASS_NAME { get; set; }
        public string COURSE_CODE { get; set; }
        public string SEMESTER_ID { get; set; }
        public string COURSE_TITLE { get; set; }
        public string INTERNAL { get; set; }
        public string STAFF_NAME { get; set; }
        public string STAFF_ID { get; set; }
        public string COURSE_TYPE { get; set; }
        public string COURSE_ID { get; set; }
        public string CLASS_ID { get; set; }
    }
    // Exam Registration ...
    public class EXAM_REGISTRATION
    {
        public string EXAM_SETTING_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string ROLL_NO { get; set; }
        public string NAME { get; set; }
        public string REGISTER_NO { get; set; }
        public string COURSE_ROOT_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string COURSE_CODE { get; set; }
        public string COURSE_TITLE { get; set; }
        public string RESULT { get; set; }
        public string IS_COMPULSORY { get; set; }
        public string SEMESTER_NAME { get; set; }
        public string DEPARTMENT { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string MOBILE_NO { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string PROGRAMME { get; set; }
        public string BATCH { get; set; }
        public string IS_ACTIVE { get; set; }
        public string DURATION_UNIT { get; set; }
        public string SEMESTER_ID { get; set; }
        public string SEMESTER { get; set; }
        public string AC_YEAR { get; set; }
        public string YEAR { get; set; }
        public string DATE { get; set; }
        public string MONTH { get; set; }
        public string SUBJECT_TYPE { get; set; }
        public string SUBJECT_TYPE_ID { get; set; }
        public string STATUS { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string TOTAL { get; set; }
        public string PAID { get; set; }
        public string BALANCE { get; set; }

    }
    public class EXAM_REGISTRATION_LIST
    {
        public string STUDENT_ID { get; set; }
        public string COURSE_ROOT_ID { get; set; }
        public string COURSE_CODE { get; set; }
        public string COURSE_TITLE { get; set; }
        public string IS_COMPULSORY { get; set; }
    }
    public class GET_ACTIVE_SEMESTER_BATCH
    {
        public string BATCH { get; set; }
        public string ACADEMIC_YEAR_ID { get; set; }
        public string AC_YEAR { get; set; }
        public string SEMESTER { get; set; }
        public string IS_ACTIVE { get; set; }
        public string STUDENT_ID { get; set; }
        public string PROGRAMME { get; set; }
    }
    public class GET_PROGRAMME_AND_BATCH
    {
        public string STUDENT_ID { get; set; }
        public string PROGRAMME { get; set; }
        public string BATCH { get; set; }
        public string SEMESTER_ID { get; set; }
    }
    public class SEMESTER_RESULT
    {
        public string COURSE_ROOT_ID { get; set; }
        public string COURSE_CODE { get; set; }
        public string COURSE_TITLE { get; set; }
        public string RESULT { get; set; }
        public string STATUS { get; set; }
        public string STUDENT_ID { get; set; }
        public string SEMESTER { get; set; }
        public string AC_YEAR { get; set; }
        public string WRITTEN_SEMESTER { get; set; }
        public string SUBJECT_TYPE_ID { get; set; }
        public string SUBJECT_TYPE { get; set; }
        public string EXAM_SETTING_ID { get; set; }
        public string AMOUNT { get; set; }

    }

    // Save Subject Details ..
    public class SaveExamDetails
    {
        public List<SEMESTER_RESULT> SubjectDetails { get; set; }
    }
    public class EXAM_REGISTRATION_2017
    {
        public string REGISTRATION_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string ROLL_NO { get; set; }
        public string REGISTER_NO { get; set; }
        public string COURSE_ID { get; set; }
        public string COURSE_TITLE { get; set; }
        public string COURSE_CODE { get; set; }
        public string STATUS { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string EXAM_SEMESTER { get; set; }
        public string EXAM_SETTING_ID { get; set; }
        public string TOTAL_COUNT { get; set; }
        public string EXAM_NAME { get; set; }
        public string CLASS_ID { get; set; }
        public string CLASS_NAME { get; set; }

    }
    public class ExamRegistrationModel
    {
        public string REGISTRATION_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string STATUS { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }

    }
    public class FetchHallTicket
    {
        public string REGISTRATION_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string SEMESTER_ID { get; set; }
        public string SEMESTER { get; set; }
        public string COURSE_CODE { get; set; }
        public string COURSE_TITLE { get; set; }
        public string STATUS { get; set; }
        public string AC_YEAR { get; set; }
        public string COURSE_ROOT_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string MONTH_YEAR { get; set; }

    }
    public class HallTicketDetails
    {
        public string DEPARTMENT { get; set; }
        public string REGISTER_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string DEGREE { get; set; }
        public string SEMESTER_NAME { get; set; }
        public string SEMESTER { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string YEAR { get; set; }
    }

    public class EXAM_SETTING
    {
        public string EXAM_SETTING_ID { get; set; }
        public string EXAM_NAME { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string M_EXAM { get; set; }
        public string M_PASS { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string SEMESTER { get; set; }
        public string LAST_DATE_FOR_EXAM_FEE { get; set; }
    }
    public class CollegeDetails
    {
        public string COLLEGE_NAME { get; set; }
        public string UNIVERSITY { get; set; }
        public string GRADE { get; set; }
        public string COLLEGE_TYPE { get; set; }
        public string PLACE { get; set; }
        public string COLLEGE_LOGO { get; set; }
    }
    public class ExamControllerPrint
    {
        public string ROLL_NO { get; set; }
        public string REGISTER_NO { get; set; }
        public string COURSE_CODE { get; set; }
        public string COURSE_TITLE { get; set; }
        public string STATUS { get; set; }
        public string BATCH { get; set; }
        public string CLASS_ID { get; set; }
    }

    // Online Quiz ..
    public class LIST_QUIZ
    {
        public string QUESTION { get; set; }
        public string QUESTION_ID { get; set; }
        public string ANSWER { get; set; }
        public string CLASS_NAME { get; set; }
        public string FIRST_NAME { get; set; }
    }
    public class QUIZ_QUESTIONS_2017
    {
        //public string PRIMARY_ID { get; set; }
        public string QUESTION_ID { get; set; }
        public string QUESTION { get; set; }
        public string ANSWER { get; set; }
        public string CLASS_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string COURSE { get; set; }
        public string STAFF_ID { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string OPTION_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string OPTIONS { get; set; }
    }
    public class QUIZ_OPTIONS_2017
    {
        public string OPTION_ID { get; set; }
        public string OPTIONS { get; set; }
        public string QUESTION_ID { get; set; }
        public string QUESTION { get; set; }
        public string ANSWER { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class JSON_QUIZ_OPTIONS
    {
        public List<QUIZ_OPTIONS_2017> liQuizOption { get; set; }
    }
    public class QUIZ_2017
    {
        public string QUIZ_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string QUESTION_ID { get; set; }
        public string SELECTED_ANSWER { get; set; }
        public string DATE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class JSON_SAVE_QUIZ
    {
        public List<QUIZ_2017> ObjSaveQuizAnswer { get; set; }
    }
    public class QUIZ_RESULT
    {
        public string SELECTED { get; set; }
        public string COUNT { get; set; }
        public string CHECKED { get; set; }
        public string QUESTION_ID { get; set; }
        public string QUESTION { get; set; }
        public string ANSWER { get; set; }
        public string SELECTED_ANSWER { get; set; }
        public string FIRST_NAME { get; set; }
        public string COURSE_TITLE { get; set; }
        public string COURSE_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string STATUS { get; set; }
        public string DATE { get; set; }
    }
}
