using System.Collections.Generic;
using System.ComponentModel;

namespace CMS.ViewModel.Model
{

    public class ADM_APPLICATIONSERIALNO
    {
        public string ApplicationNo { get; set; }
        public string Issue_Id { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string DEPT_CODE { get; set; }

    }
    public class ADM_APPLICATION_TYPE
    {
        public string APPLICATION_TYPE_ID { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string PROGRAMME { get; set; }
        public string APPLICATION_COST { get; set; }
        public string LAST_DATE { get; set; }
        public string IS_DELETED { get; set; }
        public string G_COST { get; set; }
        public string APP_PREFIX { get; set; }
        public string APP_START_NO { get; set; }
        public string APP_FEE { get; set; }
        public string REG_FEE { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string ONLINE_COST { get; set; }
        public string ADDITIONAL_COST { get; set; }
        public string APPLICATION_ADDITIONALCOST { get; set; }
        public string SHIFT { get; set; }
        public string PROGRAMME_MODE { get; set; }

    }
    public class PayURefundResponse
    {
        public string status { get; set; }
        public string msg { get; set; }
        public string request_id { get; set; }
        public string bank_ref_num { get; set; }
        public string mihpayid { get; set; }
        public string error_code { get; set; }
    }
    public class ISSUED_LIST
    {
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string AMOUNT { get; set; }
        public string PAID { get; set; }
        public string RECEIVE_ID { get; set; }

    }
    public class ADM_ISSUED_APPLICATIONS
    {
        public string ISSUED_ID { get; set; }
        public string APPLICATION_NO { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string RECEIVE_ID { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string STATUS { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string AGE { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_PAID { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string RAZORPAY_ID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string NATIVE_DISTRICT { get; set; }

        public string SMS_MOBILE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string SELECTION_ID { get; set; }
        public string ROLL_NO { get; set; }
        public string ENTRANCE_MARK { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string SHIFT { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string STATUS_ID { get; set; }
        public string START_DATE { get; set; }
        public string IS_BLOCKED { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string ISSUE_ID { get; set; }

        public string COLLEGE_DISTRICT { get; set; }
        public string COLLEGE_REGION { get; set; }
        public string AADHAR_NAME { get; set; }
        public string IS_ORPHAN { get; set; }
        public string INCOME_CERT_NO { get; set; }
        public string BANK_NAME { get; set; }
        public string BANK_BRANCH { get; set; }
        public string CITY { get; set; }
        public string IFSC_CODE { get; set; }
        public string ACCOUNT_TYPE { get; set; }
        public string COMMUNITY_CERT_NO { get; set; }
        public string ACCOUNT_NO { get; set; }

    }
    public class ADM_ISSUE_APPLICATION_2018
    {
        public string VERIFICATION_DATE { get; set; }
        public string DAY { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string ISSUED_ID { get; set; }
        public string NATIVE_DISTRICT { get; set; }

        public string PAYU_RESPONSE_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string ISSUE_ID { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string IS_HAVE_CONSOLIDATE_MARKSHEET { get; set; }

        public string PROGRAMME_ID { get; set; }
        public string PROGRAMME_CODE { get; set; }
        public string ISSUE_DATE { get; set; }
        public string APPLICATION_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string FATHER_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string IS_FREE_APPLICATION { get; set; }
        public string STATUS { get; set; }
        public string IS_DELETED { get; set; }
        public string CONTACT_NO { get; set; }
        public string REMARKS { get; set; }
        public string STAFF_ID { get; set; }
        public string IS_ONLINE_APPLICANT { get; set; }
        public string RELIGION_ID { get; set; }
        public string RELIGION { get; set; }
        public string CASTE { get; set; }
        public string IS_RECOMMENDATION { get; set; }
        public string RECOMMENDED_BY { get; set; }
        public string RESIDENCE_TYPE { get; set; }
        public string EMAIL_ID { get; set; }
        public string CDOOR_DETAIL { get; set; }
        public string MOTHER_INCOME { get; set; }
        public string MOTHER_OCCUPATION { get; set; }

        public string CSTREET { get; set; }
        public string CVILLAGE_AREA { get; set; }
        public string CPOST_PLACE_TOWN { get; set; }
        public string CTALUK_CITY { get; set; }
        public string CDISTRICT { get; set; }
        public string CPINCODE { get; set; }
        public string CCOUNTRY { get; set; }
        public string COUNTRY_NAME { get; set; }
        public string CSTATE { get; set; }
        public string PSTATE { get; set; }
        public string SHIFT { get; set; }
        public string IS_PAID { get; set; }
        public string DOB { get; set; }
        public string HSC_NO { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string MOTHER_TONGUE { get; set; }
        public string COMMUNITY_ID { get; set; }
        public string OCCUPATION { get; set; }
        public string ANNUAL_INCOME { get; set; }
        public string MARITAL_STATUS_ID { get; set; }
        public string IS_SINGLE_GIRL_CHILD { get; set; }
        public string EXSERVICE_MAN { get; set; }
        public string EDUCATION_BOARD_ID { get; set; }
        public string SPECIALLYABLED_ID { get; set; }
        public string GENDER { get; set; }
        public string IS_FIRSTGENERATION { get; set; }
        public string CDOORDETAIL { get; set; }
        public string PDOORDETAIL { get; set; }
        public string PSTREET { get; set; }
        public string PPOST_PLACE_TOWN { get; set; }
        public string PTALUK_CITY { get; set; }
        public string PPINCODE { get; set; }
        public string PDISTRICT { get; set; }
        public string PCOUNTRY { get; set; }
        public string PVILLAGE_AREA { get; set; }
        public string CPHONENO { get; set; }
        public string PPHONENO { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string YEAR_OF_PASSING { get; set; }
        public string RECEIVE_ID { get; set; }
        public string IS_ROMAN_CATHOLIC { get; set; }
        public string HOSTEL_FACILITY { get; set; }
        public string HSS_GROUP_ID { get; set; }
        public string AGE { get; set; }
        public string APP_FEE { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string RECEIVE_STU_ID { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string PLACE_OF_BIRTH { get; set; }
        public string EXTRA_CURRICULAR { get; set; }
        public string MOBILE_NO { get; set; }
        public string LAST_STUDIED_SCHOOL { get; set; }
        public string UG_LAST_COLLEGE_STUDIED { get; set; }
        public string UNIVERSITY { get; set; }
        public string IS_REGISTERED_TENANT { get; set; }
        public string IS_SUBMITTED { get; set; }
        public string FILE_PATH { get; set; }
        public string PHOTO_PATH { get; set; }
        public string COMMUNITY { get; set; }
        public string SHIFT_NAME { get; set; }
        public string HSTOTAL { get; set; }
        public string HS_MAX_MARK { get; set; }
        public string HSPERCENTAGE { get; set; }
        public string IS_DALIT { get; set; }
        public string LAST_STUDIED_PLACE { get; set; }
        public string TOTAL_CUT_OFF_MARK { get; set; }
        public string IS_VERIFIED { get; set; }
        public string LAST_STUDIED_CLASS { get; set; }
        public string VERIFIED_BY { get; set; }
        public string SELECTED_BY { get; set; }
        public string IS_FEE_PAID { get; set; }
        public string REQUEST_ID { get; set; }
        public string APPROVE_ID { get; set; }
        public string PROGRAMME_MODE_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string HOSTEL_REGISTRATION_ID { get; set; }
        public string ROOM_NO { get; set; }
        public string CLASS_ID { get; set; }
        public string PARISH_NAME { get; set; }
        public string COLLEGE_RELATION { get; set; }
        public string STUDENT_INFO { get; set; }
        public string FAMILY_PHOTO { get; set; }
        public string STATUS_NAME { get; set; }
        public string SELECTION_CYCLE { get; set; }
        public string HOSTEL_SUBMITTED { get; set; }
        public string BLOOD_GROUP { get; set; }
        public string CLASS_NAME { get; set; }
        public string HOSTEL_ID { get; set; }
        public string HOSTEL_NAME { get; set; }
        public string MAIN_COMMUNITY { get; set; }
        public string SELECTION_DATE { get; set; }
        public string PASSWORD { get; set; }
        public string OTP { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string FATHER_AGE { get; set; }
        public string MOTHER_AGE { get; set; }
        public string IS_NRI { get; set; }
        public string AADHAR_NUMBER { get; set; }
        public string PASSPORT_NUMBER { get; set; }
        public string MOTHER_NAME { get; set; }
        public string IS_ACTIVE { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string RAZORPAY_ID { get; set; }
        public string CASTE_ID { get; set; }
        public string STATUS_ID { get; set; }
        public string APPTYPE_ID { get; set; }
        public string RECEIVE_DATE { get; set; }
        public string BLOOD_GROUP_NAME { get; set; }
        public string IMAGE_PATH { get; set; }
        public string ADDRESS { get; set; }
        public string NOMINEE { get; set; }
        public string NOMINEE_DOORNO { get; set; }

        public string NOMINEE_STREET { get; set; }

        public string NOMINEE_AREA { get; set; }

        public string NOMINEE_CITY { get; set; }
        public string NOMINEE_DISTRICT { get; set; }
        public string NOMINEE_PINCODE { get; set; }

        public string ID_TYPE { get; set; }

        public string NUMBER { get; set; }
        public string MANAGEMENT_NAME { get; set; }
        public string DESIGNATION { get; set; }
        public string MARITAL_STATUS_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public string PADDRESS { get; set; }
        public string CADDRESS { get; set; }
        public string NOMINEE_ADDRESS { get; set; }
        public string NOMINEE_STATE { get; set; }
        public string ACCOUNT_NO { get; set; }
        public string IS_ACCOUNT_CREATED { get; set; }
        public string ROLL_NO { get; set; }
        public string NAME_IN_NATIVE { get; set; }

        public string IS_ELEVENTH_PASS { get; set; }
        public string SINGLE_PARENT_PROOF { get; set; }
        public string PHYSICALY_CHALLENGED_PROOF { get; set; }
        public string EX_SERVICEMAN_PROOF { get; set; }
        public string COMMUNITY_PROOF { get; set; }
        public string NRI_PROOF { get; set; }
        public string IS_SINGLE_PARENT { get; set; }
        public string PHYSICALY_CHALLANGED_TYPE { get; set; }
        public string AMOUNT { get; set; }
        public string UG_TOTAL_SEMESTER { get; set; }
        public string PARISHI_FRC { get; set; }
        public string FATHER_MOBILE_NUMBER { get; set; }
        public string MOTHER_MOBILE_NUMBER { get; set; }

        public string COLLEGE_DISTRICT { get; set; }
        public string COLLEGE_REGION { get; set; }
        public string AADHAR_NAME { get; set; }
        public string IS_ORPHAN { get; set; }
        public string INCOME_CERT_NO { get; set; }
        public string BANK_NAME { get; set; }
        public string BANK_BRANCH { get; set; }
        public string CITY { get; set; }
        public string IFSC_CODE { get; set; }
        public string ACCOUNT_TYPE { get; set; }
        public string COMMUNITY_CERT_NO { get; set; }


    }

    public class ADM_RECEIVE_APPLICATION
    {
        public string SETTLEMENT_DATE { get; set; }
        public string STATUS_ID { get; set; }
        public string PASSPORT_NUMBER { get; set; }
        public string FATHER_AGE { get; set; }
        public string MOTHER_AGE { get; set; }
        public string IS_NRI { get; set; }
        public string NATIVE_DISTRICT { get; set; }

        public string AADHAR_NUMBER { get; set; }
        public string RECEIVE_ID { get; set; }
        public string ISSUE_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string PROGRAMME_LEVEL { get; set; }
        public string BRANCH_OF_STUDY { get; set; }
        public string MOTHER_NAME { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string LAST_STUDIED_STATE { get; set; }
        public string TRANSACTION_MODE { get; set; }
        public string TRANSACTION_ID { get; set; }
        public string DDNUMBER { get; set; }
        public string BRANCH { get; set; }
        public string DDOR_TRANSACTION_DATE { get; set; }
        public string DDTAKEN_PLACE { get; set; }
        public string DDTAKEN_BANK { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string APPLICATION_NO { get; set; }
        public string MODE_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string AGE { get; set; }
        public string GENDER { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string COMMUNITY_ID { get; set; }
        public string CASTE_ID { get; set; }
        public string NATIONALITY_ID { get; set; }
        public string PLACE_OF_BIRTH { get; set; }
        public string RELIGION_ID { get; set; }
        public string MOTHER_TONGUE { get; set; }
        public string MOTHER_INCOME { get; set; }
        public string FATHER_MOBILE_NUMBER { get; set; }
        public string MOTHER_MOBILE_NUMBER { get; set; }
        public string FATHER_NAME { get; set; }
        public string OCCUPATION { get; set; }
        public string ANNUAL_INCOME { get; set; }
        public string ECONOMIC_STATUS_ID { get; set; }
        public string MARITAL_STATUS_ID { get; set; }
        public string PARISHI_FRC { get; set; }
        public string IS_FIRSTGENERATION { get; set; }
        public string SPECIALLYABLED_ID { get; set; }
        public string EXSERVICE_MAN { get; set; }
        public string TAMIL_ORGIN { get; set; }
        public string LIVING_MODE_ID { get; set; }
        public string SPORTS { get; set; }
        public string LEVEL { get; set; }
        public string EXTRA_CURRICULAR { get; set; }
        public string EXAM_PASSED { get; set; }
        public string YEAR_OF_PASSING { get; set; }
        public string MEDIUM { get; set; }
        public string SCHOOL_OR_COLLEGE { get; set; }
        public string LAST_STUDIED_PLACE { get; set; }
        public string MAIN_SUBJECT_PG { get; set; }
        public string HSC_GROUP { get; set; }
        public string NO_OF_ATTEMPT { get; set; }
        public string REMARK { get; set; }
        public string RECEIVE_DATE { get; set; }
        public string EDUCATION_BOARD_ID { get; set; }
        public string VOCATIONAL_GROUP { get; set; }
        public string CDOORDETAIL { get; set; }
        public string CSTREET { get; set; }
        public string CVILLAGE_AREA { get; set; }
        public string CPOST_PLACE_TOWN { get; set; }
        public string CTALUK_CITY { get; set; }
        public string CDISTRICT { get; set; }
        public string CSTATE { get; set; }
        public string CPINCODE { get; set; }
        public string CCOUNTRY { get; set; }
        public string CPHONENO { get; set; }
        public string CMOBILENO { get; set; }
        public string PDOORDETAIL { get; set; }
        public string PSTREET { get; set; }
        public string PVILLAGE_AREA { get; set; }
        public string PPOST_PLACE_TOWN { get; set; }
        public string PTALUK_CITY { get; set; }
        public string PDISTRICT { get; set; }
        public string PSTATE { get; set; }
        public string PPINCODE { get; set; }
        public string PCOUNTRY { get; set; }
        public string PPHONENO { get; set; }
        public string PMOBILENO { get; set; }
        public string HSTOTAL { get; set; }
        public string STAFF_ID { get; set; }
        public string IS_DELETED { get; set; }
        public string AGGREGATE_PERCENTAGE { get; set; }
        public string HONOUR_SPERCENTAGE { get; set; }
        public string PROGGROUP_ID { get; set; }
        public string HSPERCENTAGE { get; set; }
        public string HONOUR_SUBJECT { get; set; }
        public string HOSTEL_FACILITY { get; set; }
        public string ONLINE_USER { get; set; }
        public string IS_ENTRANCE_BASED { get; set; }
        public string IS_SINGLE_GIRL_CHILD { get; set; }
        public string ONLINE_APPLICATION_COST { get; set; }
        public string IS_COURIERSENT { get; set; }
        public string NOTRANS_ATTEMPT { get; set; }
        public string ENTRANCE_ROLLNO { get; set; }
        public string CLUBMEMID { get; set; }
        public string UG_PROGRAMME_ID { get; set; }
        public string UG_TOTAL_MARK { get; set; }
        public string HS_MAX_MARK { get; set; }
        public string IS_SPOT_ADM { get; set; }
        public string HAILING_FROM { get; set; }
        public string EMAIL_ID { get; set; }
        public string ISRC { get; set; }
        public string RCCATEGORY { get; set; }
        public string ROLL_NO { get; set; }
        public string BATCH_ID { get; set; }
        public string LAST_STUDIED_SCHOOL { get; set; }
        public string UG_LAST_COLLEGE_STUDIED { get; set; }
        public string UG_LAST_COLLEGE_PLACE { get; set; }
        public string LAST_STUDIED_CLASS { get; set; }
        public string TOTAL_CUT_OFF_MARK { get; set; }
        public string UG_YEAR_OF_PASSING { get; set; }
        public string BLOOD_GROUP { get; set; }
        public string DIOCESE_OR_CONGREGATION { get; set; }
        public string IS_DALIT { get; set; }
        public string STATUS { get; set; }
        public string UNIVERSITY { get; set; }
        public string MOTHER_OCCUPATION { get; set; }
        public string IS_ROMAN_CATHOLIC { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_REGISTERED_TENANT { get; set; }
        public string PHOTO_PATH { get; set; }
        public string IS_SUBMITTED { get; set; }
        public string HSC_NO { get; set; }
        public string IS_VERIFIED { get; set; }
        public string ISSUED_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string IS_PAID { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string RAZORPAY_ID { get; set; }
        public string SHIFT { get; set; }
        public string DOB { get; set; }
        public string SHIFT_NAME { get; set; }
        public string IS_FEE_PAID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string STUDENT_ID { get; set; }
        public string PAYMENT_DATE { get; set; }
        public string STATUS_NAME { get; set; }
        public string CASTE { get; set; }
        public string RELIGION { get; set; }
        public string MAIN_COMMUNITY { get; set; }
        public string HOSTEL_NAME { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string CLASS_NAME { get; set; }
        public string CLASS { get; set; }
        public string PREFIX { get; set; }
        public string RUN_ID { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string DEPARTMENT { get; set; }
        public string CLASS_ID { get; set; }
        public string ACCOUNT_NO { get; set; }
        public string IS_AC_CREATED { get; set; }
        public string STU_EMAILID { get; set; }
        public string STU_MOBILENO { get; set; }
        public string BOID_IMAGE { get; set; }
        public string IS_ELEVENTH_PASS { get; set; }

        public string SINGLE_PARENT_PROOF { get; set; }
        public string PHYSICALY_CHALLENGED_PROOF { get; set; }
        public string EX_SERVICEMAN_PROOF { get; set; }
        public string COMMUNITY_PROOF { get; set; }
        public string NRI_PROOF { get; set; }
        public string IS_SINGLE_PARENT { get; set; }
        public string PHYSICALY_CHALLANGED_TYPE { get; set; }
        public string UG_TOTAL_SEMESTER { get; set; }
        public string COMMUNITY { get; set; }
        public string IS_BLOCKED { get; set; }

        public string IS_HAVE_CONSOLIDATE_MARKSHEET { get; set; }



    }
    public class ADM_APPTYPE_PROG_GROUPS
    {
        public string PROGRAMME_LEVEL { get; set; }
        public string PROGRAMME_LEVEL_ID { get; set; }
        public string PRO_GROUPS_ID { get; set; }
        public string APPTYPE_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string IS_DELETED { get; set; }
        public string SHIFT { get; set; }
        public string SHIFT_NAME { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string APPLICATION_COST { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string PROGRAMME_MODE_ID { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string HSC_NO { get; set; }
        public string IS_NEW { get; set; }
        public string TIME { get; set; }
        public string CLASS_ID { get; set; }
        public string PROGRAMME { get; set; }
        public string CLASS_NAME { get; set; }
        public string CLASS_YEAR { get; set; }
        public string STAFF_ID { get; set; }
        public string INCHARGE_ID { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string FIRST_NAME { get; set; }
        public string SELECTED { get; set; }
        public string APPLICATION_NO { get; set; }
        public string RECEIVE_ID { get; set; }
        public string ISSUE_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT { get; set; }
        public string STATUS { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string STATUS_NAME { get; set; }
        public string GENDER { get; set; }
    }
    public class ADM_HSS_GROUP_SUBJECTS
    {
        public string GRPSUB_ID { get; set; }
        public string HSS_GROUP { get; set; }
        public string HSS_SUBJECT { get; set; }
        public string IS_DELETED { get; set; }
        public string ACADEMIC_YEAR { get; set; }
    }



    public class ADM_STU_SUBMARKS
    {
        public string MARK_ID { get; set; }
        public string RECEIVE_STUID { get; set; }
        public string GRPSUB_ID { get; set; }
        public string HSS_GROUP { get; set; }
        public string SUBJECT_ID { get; set; }
        public string HSS_SUBJECT { get; set; }
        public string MAX_MARK { get; set; }
        public string MARK { get; set; }
        public string M_PASS { get; set; }
        public string PART { get; set; }
        public string NO_OF_ATTEMPTS { get; set; }
        public string IS_DELETED { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string FILE_COUNT { get; set; }
        public string EDUCATION_BOARD_ID { get; set; }
        public string IS_ELEVENTH_PASS { get; set; }
        public string MIN_MARK { get; set; }
        public string PERSENTAGE { get; set; }
        public string HSS_SUBJECT_ID { set; get; }
        public string REGISTER_NO { set; get; }
        public string SUB_CODE { get; set; }
        public string HSPERCENTAGE { get; set; }

    }
    public class JSON_ADM_STU_SUBMARKS
    {
        public List<ADM_STU_SUBMARKS> SAVE_ADM_SUB_STU_MARKS { get; set; }
        public List<ADM_SELECTION_PROCESS> SAVE_SELECTION_PROCESS { get; set; }
        public List<ADM_STU_RELATIONS> SAVE_STU_RELATIONS { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> SAVE_ADM_ISSUE_APPLICATION_2018 { get; set; }
        public List<ADM_SELECTION_PROCESS> SAVE_HOSTEL_SELECTION { get; set; }
        public List<ADM_RECEIVE_APPLICATION> JSON_ADM_RECEIVE { get; set; }
        public List<ADM_ISSUED_APPLICATIONS> JSON_ISSUED_APPLICATIONS { get; set; }
        public List<ADM_REFUND_STUDENT_ACCOUNT> JSON_ADM_REFUND_STUDENT_ACCOUNT { get; set; }
    }
    public class ADM_UPLOADED_FILES_2018
    {
        public string FILE_ID { get; set; }
        public string RECEIVE_STU_ID { get; set; }
        public string FILE_PATH { get; set; }
        public string IS_UPLOADED { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_ACTIVE { get; set; }
        public string PHOTO_PATH { get; set; }
    }
    public class ADM_ELEGIBILITIES
    {
        public string ELIGIBILITY_ID { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string ELIGIBILITY { get; set; }
        public string MAIN_SUBJECT_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_ACTIVE { get; set; }
    }
    public class ADM_SELECTION_PROCESS
    {
        public string SELECTION_ID { get; set; }
        public string APP_ID { get; set; }
        public string APPLICATION_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string SELECTION_DATE { get; set; }
        public string QUOTA { get; set; }
        public string SELECTION_TYPE { get; set; }
        public string SELECTION_CYCLE { get; set; }
        public string ENTRANCE_MARK { get; set; }
        public string ADVANTAGE_MARK { get; set; }
        public string TOTAL_CUT_OFF_MARK { get; set; }
        public string TOTAL_SECURED { get; set; }
        public string MAX_TOTAL { get; set; }
        public string IS_DELETED { get; set; }
        public string RECEIVE_ID { get; set; }
        public string HSC_NO { get; set; }
        public string ISSUE_ID { get; set; }
        public string IS_VERIFIED { get; set; }
        public string VERIFIED_BY { get; set; }
        public string VERIFIED_DATE { get; set; }
        public string SELECTED_BY { get; set; }
        public string REQUEST_ID { get; set; }
        public string APPROVE_ID { get; set; }
        public string IS_FEE_PAID { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string COMMUNITY { get; set; }
        public string STAFF_ID { get; set; }
        public string HOSTEL_ID { get; set; }
        public string STATUS { get; set; }
        public string CVILLAGE_AREA { get; set; }
        public string STATUS_NAME { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string PROGRAMME { get; set; }
        public string CYCLE { get; set; }
        public string CASTE { get; set; }
        public string HOSTEL_SELECTION_ID { get; set; }
        public string ISSUED_ID { get; set; }
        public string IS_CANCELED { get; set; }
        public string HOSTEL_NAME { get; set; }
        public string MAIN_COMMUNITY { get; set; }
        public string HSTOTAL { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string IS_SPORTS_QUOTA { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string SELECTION_CYCLE_ID { get; set; }
        public string IS_NOT_INTEREST { get; set; }
        public string APPLICANT_TYPE { get; set; }
        public string SUBMITION_NO { get; set; }
        public string STAFF_NAME { get; set; }
        public string MAIN_COMMUNITY_ID { get; set; }
        public string PROGRAMME_MODE_ID { get; set; }
        public string PROGRAMME_MODE { get; set; }


    }
    public class ADM_PROG_ELIGIBILITY
    {
        public string PROG_ELIGIBILITY_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string SUBJECT_ID { get; set; }
        public string CODE { get; set; }
        public string HSS_SUBJECT { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_ACTIVE { get; set; }

    }
    public class ADM_TRANSFER
    {
        public string TRANSFER_ID { get; set; }
        public string RECEIVE_ID { get; set; }
        public string REQUEST_ID { get; set; }
        public string APPLICATION_NO { get; set; }
        public string PROGRAMME_FROM { get; set; }
        public string PROGRAMME_TO { get; set; }
        public string FIRST_NAME { get; set; }
        public string APPROVE_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_APPROVED { get; set; }
        public string IS_DELETED { get; set; }
        public string HSTOTAL { get; set; }
        public string HS_MAX_MARK { get; set; }
        public string FLAG { get; set; }
        public string ISSUE_ID { get; set; }
        public string AMOUNT { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string PROGGROUP_ID { get; set; }
        public string STATUS { get; set; }
        public string STATUS_ID { get; set; }
        public string DATETIME { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string ISSUE_NO { get; set; }

    }

    public class SELECTIONPROCCESS
    {
        public string RECEIVE_ID { get; set; }
        public string APPLICATION_NO { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public string IS_SINGLE_GIRL_CHILD { get; set; }
        public string TOTAL_CUT_OFF_MARK { get; set; }
        public string HSTOTAL { get; set; }
        public string HSPERCENTAGE { get; set; }
        public string HS_MAX_MARK { get; set; }
        public string ISSUE_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string HSC_NO { get; set; }
        public string SHIFT { get; set; }
        public string COMMUNITY_ID { get; set; }
        public string COMMUNITY { get; set; }
        public string LAST_STUDIED_CLASS { get; set; }
        public string LAST_STUDIED_SCHOOL { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string RELIGION { get; set; }
        public string CASTE_ID { get; set; }
        public string SELECTION_CYCLE { get; set; }
        public string DATE_FROM { get; set; }
        public string DATE_TO { get; set; }
        public string NO_OF_SEATS { get; set; }
        public string TOTAL_SELECTION { get; set; }
        public string VERIFIED { get; set; }
        public string PAID { get; set; }
        public string PAID_MINORITY { get; set; }
        public string MINORITY_SELECTED { get; set; }
        public string PAID_OTHERS { get; set; }
        public string OTHERS_SELECTED { get; set; }
        public string SELECTION_DATE { get; set; }
        public string UNIVERSITY { get; set; }
        public string OCCUPATION { get; set; }
        public string ANNUAL_INCOME { get; set; }
        public string EXSERVICE_MAN { get; set; }
        public string SPECIALLYABLED_ID { get; set; }
        public string IS_FIRSTGENERATION { get; set; }
        public string CVILLAGE_AREA { get; set; }
        public string IS_VERIFIED { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string PROGRAMME { get; set; }
        public string STATUS_NAME { get; set; }
        public string CYCLE { get; set; }
        public string CASTE { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string PROGRAMME_MODE { get; set; }
    }
    public class ADM_STU_RELATIONS
    {
        public string RELATION_ID { get; set; }
        public string RELATION_NAME { get; set; }
        public string RECEIVE_ID { get; set; }
        public string RELATION_SHIP { get; set; }
        public string OCCUPATION { get; set; }
        public string INCOME { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }

    public class ADM_CANCEL_ADMISSION_2018
    {
        public string ADMISSION_CANCEL_ID { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string APPLICATION_NO { get; set; }
        public string MODE_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string ADMISSION_NO { get; set; }
        public string ADMISSION_DATE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string GENDER_ID { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string RECEIVE_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string REASON { get; set; }
        public string IS_REFUNDED { get; set; }
        public string IS_HOSTEL_CANCEL { get; set; }
    }

    public class ADM_MAXIMUM_IN_TAKES
    {
        public string INTAKE_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string SHIFT { get; set; }
        public string NO_OF_SEATS { get; set; }
        public string IS_DELETED { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string CQ_SEATS { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string MINORITY { get; set; }
    }

    public class ADM_CASTEWISE_QUATA : ADM_MAXIMUM_IN_TAKES
    {
        public string CASTEWISE_QUATA_ID { get; set; }
        public string CASTE_ID { get; set; }
        public string IS_ACTIVE { get; set; }
        public string SELECTED { get; set; }
        public string VERIFIED { get; set; }
        public string ADMITTED { get; set; }
        public string TOTAL_SELECTED { get; set; }
        public string TOTAL_VERIFIED { get; set; }
        public string TOTAL_ADMITTED { get; set; }
        public string MX_PROGRAMME_ID { get; set; }
        public string MINORITY_SELECTED { get; set; }
        public string MINORITY_VERIFIED { get; set; }
        public string MINORITY_ADMITTED { get; set; }
        public string MINORITY_ALLOATED { get; set; }
        public string OC_SELECTED { get; set; }
        public string OC_VERIFIED { get; set; }
        public string OC_ADMITTED { get; set; }
        public string OC_ALLOATED { get; set; }
        public string BC_SELECTED { get; set; }
        public string BC_VERIFIED { get; set; }
        public string BC_ADMITTED { get; set; }
        public string BC_ALLOATED { get; set; }
        public string SC_ST_SELECTED { get; set; }
        public string SC_ST_VERIFIED { get; set; }
        public string SC_ST_ADMITTED { get; set; }
        public string SC_ST_ALLOATED { get; set; }
        public string MBC_DNC_SELECTED { get; set; }
        public string MBC_DNC_VERIFIED { get; set; }
        public string MBC_DNC_ADMITTED { get; set; }
        public string MBC_DNC_ALLOATED { get; set; }
        public string OTHER_SELECTED { get; set; }
        public string OTHER_VERIFIED { get; set; }
        public string OTHER_ADMITTED { get; set; }
        public string MINORITY { get; set; }
    }

    public class ADM_CANCEL_ADMISSION
    {
        public string CANCEL_ADMISSION_ID { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string NAME { get; set; }
        public string APPLICATION_NO { get; set; }
        public string GENDER_NAME { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string PRO_GROUPS_ID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string ADMISSION_DATE { get; set; }
        public string REASON { get; set; }
        public string ACADEMIC_YEAR { get; set; }
    }

    // BANK APPLICATION ..
    public class ADM_BANK_APPLICATION
    {
        public string BANK_APPLICATION_ID { get; set; }
        [DisplayName("College Name")]
        public string COLLEGE_NAME { get; set; }
        public string STUDENT_ID { get; set; }
        public string DESIGNATION { get; set; }
        public string SALUTATION { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string GENDER_ID { get; set; }
        public string MARITUAL_STATUS { get; set; }
        public string FATHER_NAME { get; set; }
        public string MOTHER_NAME { get; set; }
        public string SPOUSE_NAME { get; set; }
        public string DOB { get; set; }
        public string PERMANENT_ADDRESS1 { get; set; }
        public string PERMANENT_ADDRESS2 { get; set; }
        public string PERMANENT_ADDRESS3 { get; set; }
        public string PERMANENT_CITY { get; set; }
        public string PERMANENT_STATE { get; set; }
        public string PERMANENT_PINCODE { get; set; }
        public string COMMUNICATION_ADDRESS1 { get; set; }
        public string COMMUNICATION_ADDRESS2 { get; set; }
        public string COMMUNICATION_ADDRESS3 { get; set; }
        public string COMMUNICATION_STATE { get; set; }
        public string COMMUNICATION_CITY { get; set; }
        public string COMMUNICATION_PINCODE { get; set; }
        public string MOBILE_NUMBER { get; set; }
        public string EMAIL { get; set; }
        public string ID_TYPE { get; set; }
        public string NUMBER { get; set; }
        public string STUDENT_ID_CARD_NUMBER { get; set; }
        public string NOMINEE_NAME { get; set; }
        public string NOMINEE_AGE { get; set; }
        public string NOMINEE_RELATION { get; set; }
        public string NOMINEE_ADDRESS1 { get; set; }
        public string NOMINEE_ADDRESS2 { get; set; }
        public string NOMINEE_STATE { get; set; }
        public string NOMINEE_CITY { get; set; }
        public string NOMINNE_PINCODE { get; set; }
        public string NOMINEE_PHONE { get; set; }
        public string MEDIUM { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }

    // For Bank Application ....
    public class FETCH_BANK_APPLICATION
    {
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL_ID { get; set; }
        public string DOB { get; set; }
        public string STATE { get; set; }
        public string CITY { get; set; }
        public string FATHER_NAME { get; set; }
        public string GENDER { get; set; }
        public string ISSUE_ID { get; set; }
        public string RECEIVE_ID { get; set; }
    }

    public class COLLEGE_DETAILS
    {
        public string COLLEGE_ID { get; set; }
        public string MANAGEMENT_NAME { get; set; }
        public string COLLEGE_NAME { get; set; }
        public string COLLEGE_TYPE { get; set; }
        public string ADDRESS_ONE { get; set; }
        public string ADDRESS_TWO { get; set; }
        public string PLACE { get; set; }
        public string CITY { get; set; }
        public string DISTRICT { get; set; }
        public string PINCODE { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string COLLEGE_CODE { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string DB_NAME { get; set; }
        public string USERNAME { get; set; }
        public string LICENSE_NO { get; set; }
        public string PASSWORD { get; set; }
        public string APPLICATION_URL { get; set; }
        public string DB_SERVER { get; set; }
        public string PORTAL_URL { get; set; }
        public string WEBSITE_URL { get; set; }
        public string COLLEGE_EMAIL { get; set; }
        public string COLLEGE_EMAIL_PASSWORD { get; set; }
        public string COLLEGE_LOGO { get; set; }
        public string UNIVERSITY { get; set; }
        public string GRADE { get; set; }
    }


    public class ADM_PROGRAMME_DESCRIPTION
    {
        public string PROGRAMME_DESCRIPTION_ID { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string PROGRAMME_DESCRIPTION { get; set; }
        public string COURSE_OBJECTIVE { get; set; }
        public string PROGRAMME_OUTCOME { get; set; }
        public string PROGRAMME_CONTENT { get; set; }
        public string PROGRAMME_ELIGIBLITY { get; set; }
        public string SYLLABUS_PATH { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_ACTIVE { get; set; }
        public string SHIFT_NAME { get; set; }
        public string SHIFT { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string APPLICATION_TYPE { get; set; }
    }
    public class CP_PROGRAMME_GROUP
    {
        public string PROGRAMME_GROUP_ID { get; set; }
        public string APPTYPE_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string IS_DELETED { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_NEW { get; set; }
        public string SHIFT { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string PROGRAMME_NAME { get; set; }
    }

    public class SUP_APPLICATION_TYPE
    {
        public string APPLICATION_TYPE_ID { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string PREFIX { get; set; }
        public string SUFFIX { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class ADM_DOCUMENTS_UPLOADED
    {
        public string UPLOAD_ID { get; set; }
        public string RECEIVE_ID { get; set; }
        public string DOCUMENT_ID { get; set; }
        public string DOCUMENT_NAME { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class ADM_SELECTION_SETTING
    {
        public string SELECTION_SETTING_ID { get; set; }
        public string SELECTION_CYCLE_ID { get; set; }
        public string SELECTION_CYCLE { get; set; }

        public string INTERVAL_DAY { get; set; }
        public string IS_AUTO_CANCEL { get; set; }
        public string STATUS { get; set; }

        public string ACADEMIC_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }

    }

    public class ADM_TRANSFER_REQUEST
    {
        public string TRANSFER_REQUEST_ID { get; set; }
        public string RECEIVE_ID { get; set; }
        public string ISSUED_ID { get; set; }
        public string PROGRAMME_FROM { get; set; }
        public string PROGRAMME_TO { get; set; }
        public string PROGRAMME_FROM_NAME { get; set; }
        public string PROGRAMME_TO_NAME { get; set; }
        public string ISSUED_STATUS { get; set; }
        public string REQUEST_DATE { get; set; }
        public string REQUEST_CONTENT { get; set; }
        public string IS_CANCELLED { get; set; }
        public string IS_DELETED { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string FIRST_NAME { get; set; }
        public string EMAIL_ID { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string APPLICATION_NO { get; set; }
        public string TRANSFER_STATUS { get; set; }
        public string HSC_NO { get; set; }


    }

    public class ADM_TRANSFER_APPROVAL
    {
        public string TRANSFER_APPROVAL_ID { get; set; }
        public string TRANSFER_REQUEST_ID { get; set; }
        public string RECEIVE_ID { get; set; }
        public string ISSUED_ID { get; set; }
        public string APPLICATION_NO { get; set; }
        public string PROGRAMME_FROM { get; set; }
        public string PROGRAMME_TO { get; set; }
        public string APPROVED_BY { get; set; }
        public string ISSUED_STATUS { get; set; }
        public string TRANSFER_STATUS { get; set; }
        public string APPROVED_DATE { get; set; }
        public string IS_APPROVED { get; set; }
        public string IS_REFUND { get; set; }
        public string IS_DELETED { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string FIRST_NAME { get; set; }
        public string EMAIL_ID { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string STATUS_NAME { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string APPROVED_CONTENT { get; set; }
        public string APPROVED_CONTENT_MESSAGE { get; set; }
        public string TRANSFER_STATUS_MESSAGE { get; set; }

    }


    public class ADM_REFUND_STUDENT_ACCOUNT
    {
        public string REFUND_STUDENT_AC_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string ISSUED_ID { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string HEAD { get; set; }
        public string AMOUNT { get; set; }
        public string FEE_MAIN_HEAD_ID { get; set; }
        public string FEE_STRUCTURE_ID { get; set; }
        public string FEE_ROOT_ID { get; set; }
        public string TRANSACTION_ID { get; set; }
        public string REFUND_TYPE { get; set; }
        public string REFUND_DATE { get; set; }
        public string REFUND_BY { get; set; }
        public string IS_DELETED { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string PAYMENT_DATE { get; set; }
        public string RAZORPAY_ID { get; set; }
        public string IS_REFUND { get; set; }
        public string F_STUDENT_AC_ID { get; set; }
        public string MAIN_HEAD { get; set; }
        public string HEAD_ID { get; set; }
        public string PASSBOOK_NO { get; set; }
        public string FEE_CATEGORY { get; set; }
        public string BALANCE { get; set; }
        public string FREQUENCY_NAME { get; set; }
        public string HEAD_NAME { get; set; }
        public string STATUS { get; set; }
        public string UDF6 { get; set; }
        public string RECEIPT_NO { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string MAIN_HEAD_ID { get; set; }
        public string BANK_ACCOUNT_ID { get; set; }
        public string UDF2 { get; set; }
        public string ID { get; set; }
        public string PAYMENT_ID { get; set; }
        public string TRANSFER_ID { get; set; }
    }

    public class HOSTEL_STUDENT_EXTRACURRICULAR
    {
        public string HS_EXTRACURRICULAR_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string EXTRACURRICULAR_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string EXTRACURRICULAR_NAME { get; set; }
        public string CHECKED { get; set; }
    }


    public class HOSTEL_STUDENT_GADGETS
    {
        public string HS_GADGETS_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string GADGETS_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string GADGETS_NAME { get; set; }
        public string CHECKED { get; set; }
    }
    public class HOSTEL_STUDENT_CERTIFICATE
    {
        public string HS_CERTIFICATE_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string CHECKED { get; set; }
        public string STUDENT_ID { get; set; }
        public string CERTIFICATE_NAME { get; set; }
        public string CERTIFICATE_ID { get; set; }
    }

    public class HOSTEL_STUDENT_SPORTS
    {
        public string HS_SPORTS_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string SPORTS_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string SPORTS_NAME { get; set; }
        public string CHECKED { get; set; }
    }

    public class ADM_ISSUED_APPLICATION_RANKLIST
    {
        public string RECEIVE_ID { get; set; }
        public string ISSUED_ID { get; set; }
        public string NAME { get; set; }
        public string RELIGION { get; set; }
        public string COMMUNITY { get; set; }
        public string SUBJECT1 { get; set; }
        public string SUBJECT2 { get; set; }
        public string SUBJECT3 { get; set; }
        public string SUBJECT4 { get; set; }
        public string TOTAL { get; set; }
        public string IS_EXSERVICE_MAN { get; set; }
        public string IS_PHYSICALLY_CHALANGED { get; set; }
        public string IS_SPORTS { get; set; }
        public string APPLICATION_NO { get; set; }
        public string IS_LATE_APPLICATION { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string APP_TYPE_ID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string MARK { get; set; }
        public string MAX_MARK { get; set; }
        public string HSPERCENTAGE { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string EXTRA_CURRICULAR { get; set; }
        public string PROGRAMME_GROUP_CODE { get; set; }
        public string CASTE { get; set; }
        public string PROGRAMME_MODE { get; set; }
    }

}

