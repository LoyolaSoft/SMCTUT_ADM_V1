using System.Collections.Generic;

namespace CMS.ViewModel.Model
{
    public class SUP_SUBJECT_TYPE
    {
        public string SUBJECT_TYPE_ID { get; set; }
        public string SUBJECT_TYPE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class Sup_Fee_Category
    {
        public string FEE_CATEGORY_ID { get; set; }
        public string FEE_CATEGORY { get; set; }
    }
    public class SUP_HEAD
    {
        public string MAIN_HEAD_ID { get; set; }
        public string MAIN_HEAD { get; set; }
        public string HEAD_ID { get; set; }
        public string HEAD { get; set; }
        public string HEAD_CODE { get; set; }
        public string ACCOUNT { get; set; }
        public string HEAD_TYPE { get; set; }
        public string IS_USED { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
        public string IS_UPDATED { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string STATUS { get; set; }
        public string FEE_CATEGORY { get; set; }
        public string TEMP_ID { get; set; }
        public string HEAD_ORDER { get; set; }
        public string IS_REFUNDABLE { get; set; }
        public string SHIFT { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
    }
    public class SUP_FEE_ROOT
    {
        public string FEE_ROOT_ID { get; set; }
        public string FEE_ROOT { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }

    }
    public class SUP_FEE_MAIN_HEAD
    {
        public string MAIN_HEAD_ID { get; set; }
        public string MAIN_HEAD { get; set; }
        public string HEAD { get; set; }
        public string HEAD_ID { get; set; }
        public string IS_USED { get; set; }
        public string IS_DELETED { get; set; }
        public string HEAD_COUNT { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string FEE_MAIN_HEAD_ID { get; set; }
        public string FEE_CATEGORY_ID { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string PROGRAMME_MODE { get; set; }
    }
    public class SUP_BANK
    {
        public string BANK_ID { get; set; }
        public string BANK_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
        public string IS_UPDATED { get; set; }
        public string IS_USED { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class SUP_BANK_ACCOUNT
    {
        public string BANK_ACCOUNT_ID { get; set; }
        public string ACCOUNT_PURPOSE { get; set; }
        public string BANK { get; set; }
        public string BANK_NAME { get; set; }
        public string PASSBOOK_NO { get; set; }
        public string STARTED_DATE { get; set; }
        public string CLOSED_DATE { get; set; }
        public string IS_USED { get; set; }
        public string IS_DELETED { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string ACCOUNT_TYPE { get; set; }
    }
    public class SUP_FEE_FREQUENCY
    {
        public string FREQUENCY_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string FREQUENCY_NAME { get; set; }
        public string DATE_FROM { get; set; }
        public string DATE_TO { get; set; }
        public string STATUS { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_USED { get; set; }
        public string SEMESTER_TYPE { get; set; }
        public string LAST_DATE_OF_PAYMENT { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string IS_UPDATED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
        public string TYPE { get; set; }
        public string FREQUENCY_TYPE { get; set; }
        public string DISCOUNT_ID { get; set; }
        public string FREQUENCY_TYPE_ID { get; set; }
        public string FEE_ROOT_ID { get; set; }
        public string FEE_MODE { get; set; }
        public string MAIN_HEAD { get; set; }
        public string FEE_MAIN_HEAD_ID { get; set; }
        public string MAIN_HEAD_ID { get; set; }

    }
    public class SUP_HOURS
    {
        public string HOUR_ID { get; set; }
        public string HOUR { get; set; }
        public string TIME_FROM { get; set; }
        public string TIME_TO { get; set; }
        public string DESCRIPTION { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }

    }
    public class JsonResultData
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string sResult { get; set; }
        public List<string> sResultStringArray = new List<string>();
        public object oResult { get; set; }

    }

    public class SupCourseInfo
    {
        public string COURSE_ROOT_ID { get; set; }
        public string YEAR_FROM { get; set; }
        public string YEAR_TO { get; set; }
        public string DEPARTMENT { get; set; }
        public string YEAR { get; set; }
        public string SEMESTER_FROM { get; set; }
        public string SEMESTER_TO { get; set; }
        public string PART { get; set; }
        public string PAPER { get; set; }
        public string COURSE_TYPE { get; set; }
        public string HRS_PER_WEEK { get; set; }
        public string CREDITS { get; set; }
        public string OPTION_NAME { get; set; }
        public string PAPER_CODE { get; set; }
        public string COURSE_TITLE { get; set; }
        public string COURSE_CODE { get; set; }
        public string SEMESTER_ID { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_NME_SUBJECT { get; set; }
        public string IS_ALLIED_SUBJECT { get; set; }
        public string COURSE_ORDER { get; set; }
        public string IS_DELETED { get; set; }
        public string SUBJECTS { get; set; }
        public string IS_COMPULSORY { get; set; }
        public string UGC_COURSE_TYPE { get; set; }

    }
    public class SupCourseList
    {

    }
    public class SupPaperType
    {
        public string PAPER_TYPE_ID { get; set; }
        public string PAPER_TYPE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }


    public class SupGender
    {
        public string GENDER_ID { get; set; }
        public string GENDER_NAME { get; set; }
    }
    public class SupBloodGroup
    {
        public string BLOOD_GROUP_ID { get; set; }
        public string BLOOD_GROUP_NAME { get; set; }
    }
    public class SupMotherTongue
    {
        public string MOTHER_TONGUE_ID { get; set; }
        public string MOTHER_TONGUE_NAME { get; set; }
    }
    public class SupNationality
    {
        public string NATIONALITY_ID { get; set; }
        public string NATIONALITY_NAME { get; set; }
    }
    public class SupDepartment
    {
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT { get; set; }
    }
    public class SupCommunity
    {
        public string COMMUNITYID { get; set; }
        public string COMMUNITY { get; set; }
    }
    public class SUP_STATUS
    {
        public string STATUS_ID { get; set; }
        public string STATUS { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class SupMaritalStatus
    {
        public string MARITAL_STAUS_ID { get; set; }
        public string MARITAL_STATUS_NAME { get; set; }
    }
    public class SUP_RELIGION
    {
        public string RELIGIONID { get; set; }
        public string RELIGION { get; set; }
    }
    public class SUP_DEGREE
    {
        public string DEGREE_ID { get; set; }
        public string DEGREE { get; set; }
    }
    public class SUP_DAY_ORDER
    {
        public string DAY_ORDER_ID { get; set; }
        public string DAY { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }

    public class Sup_Duration_Unit
    {
        public string DURATION_UNIT_ID { get; set; }
        public string DURATION_UNIT { get; set; }
    }
    public class Sup_Channel
    {
        public string CHANNEL_ID { get; set; }
        public string CHANNEL { get; set; }
    }
    public class Sup_Subject
    {
        public string SUBJECT_ID { get; set; }
        public string SUBJECT { get; set; }
    }
    public class SUP_MEDIUM_OF_INSTRUCTION
    {
        public string MEDIUM_ID { get; set; }
        public string MEDIUM_OF_INSTRUCTION { get; set; }
    }
    public class Sup_Volume
    {
        public string VOLUME_ID { get; set; }
        public string VOLUME { get; set; }
    }
    public class sup_Level
    {
        public string LEVEL_ID { get; set; }
        public string LEVEL { get; set; }
    }
    public class sup_PublishCategory
    {
        public string PUBLISH_CATEGORY_ID { get; set; }
        public string PUBLISH_CATEGORY { get; set; }
    }
    public class Sup_Shift
    {
        public string SHIFT_ID { get; set; }
        public string SHIFT_NAME { get; set; }
    }
    public class Sup_Qualification
    {
        public string QUALIFICATION_ID { get; set; }
        public string QUALIFICATION { get; set; }
    }
    public class Sup_SubCategory
    {
        public string STF_CATEGORY_ID { get; set; }
        public string STF_CATEGORY { get; set; }
    }
    public class Sup_Designation
    {
        public string DESIGNATIONID { get; set; }
        public string DESIGNATION { get; set; }
    }
    public class SUP_ACHIEVEMENT
    {
        public string ACHIEVEMENT_ID { get; set; }
        public string ACHIEVEMENT_NAME { get; set; }
    }
    public class SUP_PART
    {
        public string PART_ID { get; set; }
        public string PART { get; set; }
    }
    public class SupProgrammeLevel
    {
        public string PROGRAMME_LEVEL_ID { get; set; }
        public string PROGRAMME_LEVEL { get; set; }
    }
    public class SupProgrammeType
    {
        public string PROGRAMME_TYPE_ID { get; set; }
        public string PROGRAMME_TYPE { get; set; }
    }
    public class SupProgrammeMode
    {
        public string PROGRAMME_MODE_ID { get; set; }
        public string PROGRAMME_MODE { get; set; }
    }
    public class SUP_FAMILY
    {
        public string RELATION_ID { get; set; }
        public string RELATION { get; set; }
    }
    public class cp_Classes_2017
    {
        public string CLASS_ID { get; set; }
        public string CLASS_CODE { get; set; }
        public string SHIFT { get; set; }
        public string CLASS_NAME { get; set; }
        public string DEPARTMENT { get; set; }
        public string CLASS_YEAR { get; set; }
        public string CLASS_LEVEL { get; set; }
        public string PROGRAMME { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string BATCH { get; set; }
        public string COURSE_ID { get; set; }
        public string DISCOUNT_ID { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
    }
    public class StuCourseInfo
    {
        public string CLASS_ID { get; set; }
        public string CLASS_CODE { get; set; }
        public string SHIFT { get; set; }
        public string CLASS_NAME { get; set; }
        public string DEPARTMENT { get; set; }
    }

    public class SUP_SEMESTER_TYPE
    {
        public string SEMESTER_TYPE_ID { get; set; }
        public string SEMESTER_TYPE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class SUP_SEMESTER
    {
        public string SUP_SEMESTER_ID { get; set; }
        public string SEMESTER_NAME { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }

    public class sup_Batch
    {
        public string BATCH_ID { get; set; }
        public string BATCH { get; set; }
    }
    public class sup_frequency_type
    {
        public string FREQUENCY_TYPE_ID { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string FEE_NAME { get; set; }
        public string FREQUENCY_TYPE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string FREQUENCY_NAME { get; set; }
    }

    public class SUP_REASON
    {
        public string REASON_ID { get; set; }
        public string REASON { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }

    public class SUP_ATTENDANCE_TYPE
    {
        public string ATTENDANCE_TYPE_ID { get; set; }
        public string ATTENDANCE_TYPE { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_ACTIVE { get; set; }

    }
    public class SUP_SPL_ATTENDANCE_TYPE
    {
        public string SPL_ATTENDANCE_TYPE_ID { get; set; }
        public string SPL_ATTENDANCE_TYPE { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_ACTIVE { get; set; }

    }
    public class cp_Program_2017
    {
        public string PROGRAMME_ID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string PROGRAMME_DESCRIPTION { get; set; }
        public string SHIFT { get; set; }
        public string PROGRAMME_MODE { get; set; }



    }
    public class cp_Department_2017
    {
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT { get; set; }
    }
    public class sup_Second_language
    {
        public string SECOND_LANGUAGE_ID { get; set; }
        public string SECOND_LANGUAGE_NAME { get; set; }
    }
    public class Sup_Spoken_Language
    {
        public string SPOKEN_LANGUAGE_ID { get; set; }
        public string SPOKEN_LANGUAGE_NAME { get; set; }
    }
    public class Sup_Language
    {
        public string LANGUAGE_ID { get; set; }
        public string LANGUAGE_NAME { get; set; }
    }
    public class SUP_ACTIVITY_TYPE
    {
        public string ACTIVITY_TYPE_ID { get; set; }
        public string ACTIVITY_TYPE { get; set; }
    }
    public class SUP_ACADEMIC_YEAR_LIST
    {
        public string ACADEMIC_YEAR_ID { get; set; }
        public string AC_YEAR { get; set; }
        public string ACTIVE_YEAR { get; set; }
    }
    public class Sup_Specially_Abled
    {
        public string SPECIALLY_ABLED_ID { get; set; }
        public string SPECIALLY_ABLED_NAME { get; set; }
    }
    public class Sup_ResidencyType
    {
        public string RESIDENCY_TYPE_ID { get; set; }
        public string RESIDENCY_TYPE_NAME { get; set; }
    }
    public class Sup_Occupation
    {
        public string OCCUPATION_ID { get; set; }
        public string OCCUPATION_NAME { get; set; }
    }
    public class Sup_Education
    {
        public string EDUCATION_ID { get; set; }
        public string EDUCATION_NAME { get; set; }
    }
    public class SUP_EDUCATION_BOARD
    {
        public string BOARD_ID { get; set; }
        public string BOARD_NAME { get; set; }
    }
    public class Sup_Mother_Occupation
    {
        public string OCCUPATION_ID { get; set; }
        public string OCCUPATION_NAME { get; set; }
    }
    public class Sup_Mother_Education
    {
        public string EDUCATION_ID { get; set; }
        public string EDUCATION_NAME { get; set; }
    }
    public class Sup_Father_Nationality
    {
        public string NATIONALITY_ID { get; set; }
        public string NATIONALITY_NAME { get; set; }
    }
    public class Sup_Father_Employer
    {
        public string EMPLOYER_ID { get; set; }
        public string EMPLOYER_NAME { get; set; }
    }
    public class Sup_Father_country_Of_Issue
    {
        public string COUNTRY_OF_ISSUE_ID { get; set; }
        public string COUNTRY_NAME { get; set; }
    }
    public class Sup_Mother_Nationality
    {
        public string NATIONALITY_ID { get; set; }
        public string NATIONALITY_NAME { get; set; }
    }
    public class Sup_Mother_Employer
    {
        public string EMPLOYER_ID { set; get; }
        public string EMPLOYER_NAME { set; get; }
    }
    public class Sup_Mother_country_Of_Issue
    {
        public string COUNTRY_OF_ISSUE_ID { get; set; }
        public string COUNTRY_NAME { get; set; }
    }
    public class Sup_Activity
    {
        public string ACTIVITY_ID { get; set; }
        public string ACTIVITY_NAME { get; set; }
    }
    public class Sup_Week_No
    {
        public string WEEK_NO_ID { get; set; }
        public string WEEK_NAME { get; set; }
    }
    public class SUP_TALENT_AREA
    {
        public string ACTIVITY_LEVEL_ID { get; set; }
        public string ACTIVITY_LEVEL { get; set; }
    }
    public class SUP_EXAMPASSED
    {
        public string EXAM_PASSED_ID { get; set; }
        public string EXAM_PASSED { get; set; }
    }
    public class SUP_OPTION
    {
        public string OPTION_ID { get; set; }
        public string OPTION_NAME { get; set; }
    }
    public class SUP_OVERALL_GRADE
    {
        public string OVERALL_GRADE_ID { get; set; }
        public string OVERALL_GRADE { get; set; }
    }
    public class SUP_COUNTRY
    {
        public string COUNTRY_ID { get; set; }
        public string COUNTRY_NAME { get; set; }
    }
    public class SUP_COURSEBYID
    {
        public string COURSE_ROOT_ID = "COURSE_ROOT_ID";
        public string COURSE_TITLE = "COURSE_TITLE";
    }
    public class SUP_CLASS
    {
        public string CLASS_ID { get; set; }
        public string CLASS_NAME { get; set; }
    }
    public class SUP_CLASS_YEAR
    {
        public string CLASS_YEAR_ID { get; set; }
        public string CLASS_YEAR { get; set; }
    }
    public class SupSemester
    {
        public string SUP_SEMESTER_ID { get; set; }
        public string SEMESTER_NAME { get; set; }
        public bool SELECTED { get; set; }
    }
    public class SupClassType
    {
        public string CLASS_TYPE_ID { get; set; }
        public string CLASS_TYPE { get; set; }

    }
    public class SupClassLevel
    {
        public string CLASSLEVELID { get; set; }
        public string CLASSLEVEL { get; set; }

    }
    public class SupFaculty
    {
        public string FACULTY_ID { get; set; }
        public string FACULTY { get; set; }

    }
    public class SubClassYear
    {
        public string CLASS_YEAR_ID { get; set; }
        public string CLASS_YEAR { get; set; }

    }
    public class SupOptionName
    {
        public string OPTION_ID { get; set; }
        public string OPTION_NAME { get; set; }
    }
    public class SupPaperCode
    {
        public string PAPER_CODE_ID { get; set; }
        public string PAPER_CODE { get; set; }
    }
    public class SupCourseType
    {
        public string COURSE_TYPE_ID { get; set; }
        public string COURSE_TYPE { get; set; }
    }
    public class SupIsNMESubject
    {
        public string IS_NME_ID { get; set; }
        public string IS_NME_NAME { get; set; }
    }
    public class SupIsAlliedSubject
    {
        public string IS_ALLIED_ID { get; set; }
        public string IS_ALLIED_NAME { get; set; }
    }
    public class SupIsCompulsory
    {
        public string IS_COMPULSORY_ID { get; set; }
        public string IS_COMPULSORY_NAME { get; set; }
    }
    public class SupUGCCourseType
    {
        public string UGC_COURSE_TYPE_ID { get; set; }
        public string UGC_COURSE_TYPE { get; set; }
    }
    public class cp_Course_Root_2017
    {
        public string COURSE_ROOT_ID { get; set; }
        public string COURSE_CODE { get; set; }
        public string COURSE_TITLE { get; set; }
        public string IS_NME_SUBJECT { get; set; }
        public string IS_DELETED { get; set; }
        public string SEMESTER_ID { get; set; }
        public string PAPER_TYPE { get; set; }
    }
    public class stf_Personal_Info
    {
        public string STAFF_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string COURSE_TITLE { get; set; }
        public string STAFF_CODE { get; set; }
        public string SEMESTER { get; set; }
        public string SEMESTER_ID { get; set; }
        public string PROGRAMME { get; set; }
        public string PHOTO { get; set; }
        public string FIRST_NAME { get; set; }
        public bool SELECTED { get; set; }
        public string RECORDSCOUNT { get; set; }
        public string CLASS_ID { get; set; }
        public string DATE_OF_BIRTH { get; set; }

    }
    public class SUP_NON_TEACHING_CATEGORY
    {
        public string NON_TEACHING_CATEGORY_ID { get; set; }
        public string NON_TEACHING_CATEGORY_NAME { get; set; }
    }

    public class SUP_BOARD_MEMBER
    {
        public string BOARD_MEMBER_ID { get; set; }
        public string BOARD_MEMBER_NAME { get; set; }
    }
    public class SUP_CONDUCT
    {
        public string CONDUCT_ID { get; set; }
        public string CONDUCT_NAME { get; set; }
    }
    public class NME_Setting
    {
        public string SETTINGS_ID { get; set; }
        public string SETTINGS_NAME { get; set; }
        public string CLASS_ID { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class SUP_ROLE
    {
        public string ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
    }
    public class SUP_USER_TYPE
    {
        public string USER_TYPE_ID { get; set; }
        public string USER_TYPE_NAME { get; set; }
    }

    public class SUP_TEACHING_CATEGORY
    {
        public string TEACHING_CATEGORY_ID { get; set; }
        public string TEACHING_CATEGORY { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class CATEGORY
    {
        public List<SUP_TEACHING_CATEGORY> liTeachingCategory { get; set; }
        public List<SUP_NON_TEACHING_CATEGORY> liNonTeachingCategory { get; set; }
    }
    public class Student_Personal_Info
    {
        public string STUDENT_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string ROLL_NO { get; set; }
        public string REGISTER_NO { get; set; }
        public string COURSE_ID { get; set; }
        public string CLASS_CODE { get; set; }
        public string CLASS_ID { get; set; }
        public string PERCENTAGE { get; set; }
        public double ACTUAL_PERCENTAGE { get; set; }
        public string TOTAL_HOURS { get; set; }
        public string ABSENT_HOURS { get; set; }
        public string SHIFT_NAME { get; set; }
        public string SHIFT_ID { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string RECORDSCOUNT { get; set; }
        public string CLASS { get; set; }
        public string USER_ID { get; set; }
        public string MOBILE { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string STATUS { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string PAYMENT_DATE { get; set; }
        public string STAFF_CODE { get; set; }
        public string SETTLEMENT_DATE { get; set; }
    }
    public class SUP_HOUR_TYPE
    {
        public string hour_type_id { get; set; }
        public string hourType { get; set; }
        public string is_active { get; set; }
        public string is_deleted { get; set; }
    }
    public class AUTO_GENERATE_NUMBERS
    {
        public string auto_generate_numbers_id { get; set; }
        public string exam_receipt_no { get; set; }
    }
    public class SUP_HSS_GROUPS
    {
        public string HSS_GROUP_ID { get; set; }
        public string HSS_GROUP { get; set; }
        public string HSS_GROUP_CODE { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_USED { get; set; }
    }
    public class SUP_HSS_SUBJECTS
    {
        public string HSS_SUBJECT_ID { get; set; }
        public string HSS_SUBJECT { get; set; }
        public string PART { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_USED { get; set; }
    }
    public class SUP_SELECTION_CYCLE
    {
        public string SELECTION_CYCLE_ID { get; set; }
        public string SELECTION_CYCLE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_SHOW_WEBSITE { get; set; }
        public string IS_USED { get; set; }
        public string IS_DELETED { get; set; }

    }


    public class SUP_ADM_APPROVED_CONTENT
    {
        public string ADM_APPROVED_CONTENT_ID { get; set; }
        public string CONTENT_NAME { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }

    }

    public class SUP_ADM_TRANSFER_STATUS
    {
        public string ADM_TRANSFER_STATUS_ID { get; set; }
        public string STATUS_NAME { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }

    }


    public class SUP_APPLICANT_TYPE
    {
        public string APPLICANT_TYPE_ID { get; set; }
        public string APPLICANT_TYPE { get; set; }
    }

    public class SUP_RELATION
    {
        public string RELATION_ID { get; set; }
        public string RELATION { get; set; }
    }
    public class SUP_HOSTEL
    {
        public string HOSTEL_ID { get; set; }
        public string HOSTEL_NAME { get; set; }
        public string HOSTEL_CODE { get; set; }
        public string IS_OUTSIDE { get; set; }
        public string TOTAL_STRENGTH { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string SELECTED { get; set; }
        public string ADMITTED { get; set; }
        public string RECEIVE_ID { get; set; }
        public string HOSTEL_FACILITY { get; set; }
        public string ACADEMIC_YEAR { get; set; }

    }
    public class SUP_UNIVERSITY
    {
        public string UNIVERSITY_ID { get; set; }
        public string UNIVERSITY { get; set; }
    }
    public class SUP_STATE
    {
        public string STATE_ID { get; set; }
        public string STATE { get; set; }
    }
    public class MainCommunity
    {
        public string MAIN_COMMUNITY_ID { get; set; }
        public string MAIN_COMMUNITY { get; set; }
    }

    public class SUP_SALUTATION
    {
        public string SALUTATION_ID { get; set; }
        public string SALUTATION { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class SUP_APPTYPE_PROG_GROUPS
    {
        public string PRO_GROUPS_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string APPTYPE_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_NEW { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string PROGRAMME { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string SHIFT { get; set; }
    }
    public class SMS_USERDEFINED_TEMPLATE
    {
        public string USERDEFINED_TEMPLATE_ID { get; set; }
        public string TEMPLATE_ID { get; set; }
        public string SMS_TEMPLATE_ID { get; set; }
        public string TEMPLATE_TEXT { get; set; }
        public string FIELDS_IDS { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string SMS_TEMPLATE_MODULE_ID { get; set; }
        public string TEMPLATE_NAME { get; set; }
    }
    public class SUP_FIELDS
    {
        public string FIELD_ID { get; set; }
        public string FIELD_NAME { get; set; }
        public string ALICE_NAME { get; set; }
        public string PROPERTY_NAME { get; set; }
    }
    public class SUP_PHYSICALY_CHALLENGED_TYPES
    {
        public string TYPE_ID { get; set; }
        public string TYPE { get; set; }
        public string DESCRIPTION { get; set; }

        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }

    public class SUP_APPLICATION_SUBMISSION_TYPE
    {
        public string APP_TYPE_ID { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }

}
