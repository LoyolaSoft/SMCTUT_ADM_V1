using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CMS.ViewModel.Model
{
    public class StudentModel
    {
        [Key]
        public string STUDENT_ID { get; set; }
        [Required(ErrorMessage = "First name is required !!!")]
        [Display(Name = "FIRST NAME")]
        [StringLength(maximumLength: 40, ErrorMessage = "First name should be minimum 3 and maximum 40", MinimumLength = 3)]
        public string FIRST_NAME { get; set; }
        [Display(Name = "LAST NAME")]
        public string LAST_NAME { get; set; }
        [Display(Name = "GENDER")]
        public string GENDER_ID { get; set; }
        [Display(Name = "COURSE")]
        public string COURSE_ID { get; set; }
        [Display(Name = "DEPARTMENT")]
        public string DEPARTMENT_ID { get; set; }
        [Display(Name = "DOB")]

        public string DOB { get; set; }
        [Display(Name = "E-MAIL")]
        public string EMAIL { get; set; }
        [Display(Name = "BLOOD GROUP")]
        public string BLOOD_GROUP_ID { get; set; }
        [Display(Name = "PLACE OF BIRTH")]
        public string BIRTH_PLACE { get; set; }
        [Display(Name = "RELIGION")]
        public string RELIGION_ID { get; set; }
        [Display(Name = "MOBILE")]
        public string STUDENT_MOBILE { get; set; }
        [Display(Name = "ADMISSION DATE")]
        public string ADMISSION_DATE { get; set; }
        public string STATUS { get; set; }
        public string APPLICATION_NO { get; set; }
        [Display(Name = "REGISTER NO")]
        public string REGISTER_NO { get; set; }
        [Display(Name = "BATCH")]
        public string BATCH_ID { get; set; }
        public string GRADUCATE_YEAR { get; set; }
        [Display(Name = "PERMANENT ADDRESS")]
        public string PERMANENT_ADDRESS { get; set; }
        [Display(Name = "PERMANENT ADDRESS")]

        public string PRESENT_ADDRESS { get; set; }
        [Display(Name = "COUNTRY")]

        public string COUNTRY { get; set; }
        [Display(Name = "STATE")]

        public string STATE { get; set; }
        [Display(Name = "CITY")]

        public string CITY { get; set; }
        public string PINCODE { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        [Display(Name = "FATHER'S NAME")]

        public string FATHER_NAME { get; set; }
        [Display(Name = "MOTHER'S NAME")]

        public string MOTHER_NAME { get; set; }
        [Display(Name = "GUARDIAN'S NAME")]

        public string GUARDIAN_NAME { get; set; }
        [Display(Name = "IS GUARDIAN AVAILABLE?")]

        public string IS_GUARDIAN_AVAILABLE { get; set; }
        [Display(Name = "FATHER'S MOBILE NO")]

        public string FATHER_MOBILE { get; set; }
        [Display(Name = "MOTHER'S MOBILE NO")]

        public string MOTHER_MOBILE { get; set; }
        [Display(Name = "GAURDIAN'S MOBILE NO ")]

        public string GUARDIAN_MOBILE { get; set; }
        [Display(Name = "FATHER'S E-MAIL")]

        public string FARTHER_EMAIL { get; set; }
        [Display(Name = "MOTHER'S E-MAIL")]

        public string MOTHER_EMAIL { get; set; }
        [Display(Name = "GUARDIAN'S E-MAIL")]

        public string GUARDIAN_EMAIL { get; set; }
        [Display(Name = "IS STUDENT FIRST LEARNER ?")]

        public string IS_FIRST_LEARNER { get; set; }
        [Display(Name = "IS PHYSICALLY CHALLENGED ?")]

        public string IS_PHYSICALLY_CHALLENGED { get; set; }
        [Display(Name = "ANNUAL INCOME")]
        public string ANNUAL_INCOME { get; set; }
        [Display(Name = "PREVIOUS SCHOOL/COLLEGE")]

        public string LAST_STUDIED_SCHOOL { get; set; }
        public string ADMITTED_COURSE { get; set; }
        [Display(Name = "PERSONAL MARKS OF IDENTIFICATION")]

        public string PERSONAL_MARK1 { get; set; }
        [Display(Name = "PERSONAL MARKS OF IDENTIFICATION")]

        public string PERSONAL_MARK2 { get; set; }
        public string LEAVING_DATE { get; set; }
        public string LEAVING_REASON { get; set; }
        public string TC_ISSUED_DATE { get; set; }
        [Display(Name = "CONDUCT AND CHARACTER")]
        public string CONDUCT_INFO { get; set; }
        [Display(Name = "IMAGE")]
        public string IMG_PATH { get; set; }
        public string FINGER_PRINT { get; set; }
        public string IS_DELETED { get; set; }
        public string ADMINSSION_NO { get; set; }
        [Display(Name = "ROLL NO")]
        public string ROLL_NO { get; set; }
        [Display(Name = "COURSE TYPE")]
        public string COURSE_TYPE { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string GENDER_NAME { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string RELIGION { get; set; }
        public string MOTHER_TONGUE_NAME { get; set; }

        public string OCCUPATION_NAME { get; set; }

        public string IS_FIRSTGENERATION { get; set; }

        public string SPECIALLYABLED_ID { get; set; }
        public string EXSERVICE_MAN { get; set; }
        public string EXTRA_CURRICULAR { get; set; }
        public string LAST_STUDIED_PLACE { get; set; }
        public string EDUCATION_BOARD_ID { get; set; }
        public string CVILLAGE_AREA { get; set; }
        public string CPOST_PLACE_TOWN { get; set; }
        public string CTALUK_CITY { get; set; }
        public string CDISTRICT { get; set; }

        public string COUNTRY_NAME { get; set; }
        public string HSTOTAL { get; set; }
        public string HSPERCENTAGE { get; set; }
        public string IS_SINGLE_GIRL_CHILD { get; set; }
        public string LAST_STUDIED_CLASS { get; set; }
        public string TOTAL_CUT_OFF_MARK { get; set; }
        public string BLOOD_GROUP_NAME { get; set; }

        public string IS_REGISTERED_TENANT { get; set; }
        public string HSC_NO { get; set; }
        public string FATHER_AGE { get; set; }
        public string MOTHER_AGE { get; set; }
        public string IS_NRI { get; set; }
        public string IS_DALIT { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string SHIFT_NAME { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string UNIVERSITY { get; set; }
        public string COMMUNITY { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string STATUS_NAME { get; set; }
        public string AADHAR_NUMBER { get; set; }
        public string IS_ROMAN_CATHOLIC { get; set; }
        public string EMAIL_ID { get; set; }
        public string BOARD_NAME { get; set; }


        public SelectList TestList { get; set; }

    }
    public class ListStudents
    {
        public string STUDENT_ID { get; set; }
        public string ROLL_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string DEPT_ID { get; set; }
        public string GENDER { get; set; }
        public string SHIFT_ID { get; set; }
    }
    public class AcademicDetails
    {
        public string RegisterNo { get; set; }
        public string RollNo { get; set; }
        public string URollNo { get; set; }
        public string AdmissionNo { get; set; }
        public string AdmissionDate { get; set; }
        public string AdmittedClass { get; set; }
        public string sRegisterDate { get; set; }
        public string Remarks { get; set; }
        public string TCSerialNo { get; set; }
        public string Class { get; set; }
        public string Department { get; set; }
        public string Program { get; set; }
        public string Batch { get; set; }
        public string Shift { get; set; }
        public string Stu_Email { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string STUDENT_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string STU_CLASS_ID { get; set; }
        public string Role { get; set; }
        public string UserType { get; set; }
        public string IsLeft { get; set; }

    }
    public class IF_STUDENT_EXITS
    {
        public string STUDENT_ID { get; set; }
    }
    // Privious Qualification ..
    public class PriviousQualification
    {
        public string LSchool { get; set; }
        public string LStudied { get; set; }
        public string ExamPassed { get; set; }
        public string LstudiedClass { get; set; }
    }
    // Address Details ...
    public class StudentAddress
    {
        public string CDoorDetail { get; set; }

        public string CStreet { get; set; }

        public string CVillage { get; set; }

        public string CPostPlace { get; set; }

        public string CTaluk { get; set; }

        public string CCity { get; set; }

        public string CDistrict { get; set; }

        public string Cpincode { get; set; }

        public string CCountry { get; set; }

        public string PDoorDetails { get; set; }

        public string PStreet { get; set; }

        public string PVillage { get; set; }

        public string PPostPlace { get; set; }

        public string PTaluk { get; set; }

        public string PCity { get; set; }

        public string PDistrict { get; set; }

        public string PPincode { get; set; }

        public string PCountry { get; set; }
    }

    // Student Parents Details...
    public class ParentsDetails
    {
        public string FatherName { get; set; }

        public string FDateOfBirth { get; set; }

        public string MotherName { get; set; }

        public string MDateOfBirth { get; set; }

        public string fBusinessContact { get; set; }

        public string FMobile { get; set; }

        public string FPassport { get; set; }

        public string mBusinessContact { get; set; }

        public string MMobile { get; set; }

        public string MPassport { get; set; }

        public string FPhoto { get; set; }

        public string MPhoto { get; set; }

        public string FIncome { get; set; }

        public string MIncome { get; set; }

        public string FDateOfExpire { get; set; }

        public string MDateOfExpire { get; set; }

        public string FEmail { get; set; }

        public string MEmail { get; set; }

        public string FOccupation { get; set; }

        public string FEducation { get; set; }

        public string MOccupation { get; set; }

        public string MEducation { get; set; }

        public string FNationality { get; set; }

        public string MNationality { get; set; }
    }

    // Student personal Info
    public class StudentPersonal
    {

        public string firstName { get; set; }

        public string LastName { get; set; }

        public string Caste { get; set; }

        public string DateOfBirth { get; set; }

        public string AnnulIncome { get; set; }

        public string ResidencyNo { get; set; }

        public string PossportNo { get; set; }

        public string PhotoPath { get; set; }

        public string VisaIssueDate { get; set; }

        public string VisaExpiryDate { get; set; }

        public string VisaSponser { get; set; }

        public string VisaNumber { get; set; }

        public string Identity1 { get; set; }

        public string Identity2 { get; set; }

        public string HomePhone { get; set; }

        public string AadharNumber { get; set; }

        public string StuMobileNo { get; set; }

        public string PlaceOfBirth { get; set; }

        public string MaritalStatus { get; set; }

        public string Gender { get; set; }

        public string Community { get; set; }

        public string MotherTongue { get; set; }

        public string SecondLanguage { get; set; }

        public string BloodGroup { get; set; }

        public string Religion { get; set; }

        public string Nationality { get; set; }

        public string ModeOfTransport { get; set; }

        public string SpokenLanguage { get; set; }

        public string FirstLanguage { get; set; }

        public string SpecillyAblled { get; set; }

        public string ResidencyType { get; set; }
        public string Role { get; set; }
        public string UserType { get; set; }
    }
    // Student Activity ..
    public class StudentActivity
    {
        public string PostHeld { get; set; }
        public string ActivityId { get; set; }

        public string InitiativeShown { get; set; }

        public string Participation { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public string ExtraActivity { get; set; }

        public string positionObtained { get; set; }

        public string StuActivity { get; set; }

        public string ActivityImg1 { get; set; }

        public string ActivityImg2 { get; set; }

        public string ActivityImg3 { get; set; }
    }
    // Stuudent  Certificate ...
    public class StudentCertificate
    {
        //public string CERTIFICATE_ID { get; set; }
        //public string STUDENT_ID { get; set; }

        public string CertificateNo { get; set; }
        public string CeretificateID { get; set; }

        public string RecieveDate { get; set; }

        public string GivenDate { get; set; }

        public string Archive { get; set; }

        public string Purpose { get; set; }

        public string RegisterNo { get; set; }

        public string CertificateName { get; set; }
    }
    // Student Courses ...
    public class StudentCourses
    {
        public string program { get; set; }
        public string Class { get; set; }
        public string Part { get; set; }
        public string MainSubject { get; set; }
        public string Allied1 { get; set; }
        public string Allied2 { get; set; }
        public string Allied3 { get; set; }
        public string Allied4 { get; set; }
        public string Elective1 { get; set; }
        public string Elective2 { get; set; }
        public string Elective3 { get; set; }
        public string Elective4 { get; set; }
        public string SplSub { get; set; }
    }
    // Account Info
    public class StudentAccountInfo
    {
        public string AccountNo { get; set; }

        public string IfscCode { get; set; }

        public string MicrCode { get; set; }
    }
    // Student Counseling ...
    public class StudentCounseling
    {
        public string CounsellingId { get; set; }
        public string Dateofcounseling { get; set; }

        public string Duration { get; set; }

        public string Remarks { get; set; }

        public string Commands { get; set; }

        public string Batch { get; set; }
    }
    // Student Incident ..
    public class StudentIncident
    {
        public string IncidentId { get; set; }
        public string DateOfIncident { get; set; }
        public string TimeOfIncident { get; set; }
        public string PlaceOfIncident { get; set; }
        public string FirstAidDone { get; set; }
        public string InformedToParents { get; set; }
        public string DateInformed { get; set; }
        public string IncidentDetails { get; set; }
    }

    // Student Sibling .....
    public class StudentSibling
    {
        public string SiblingId { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string InstituteName { get; set; }
        public string Program { get; set; }
        public string Qualification { get; set; }
        public string Employed { get; set; }
        public string DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public string MonthlyIncome { get; set; }
        public string ProgName { get; set; }
        public string Is_Same_Collage { get; set; }
    }

    // Student Talents ...
    public class StudentTalents
    {
        public string TalentsId { get; set; }
        public string Date { get; set; }
        public string TalentArea { get; set; }
        public string TalentActivityType { get; set; }
        public string TalentDiscription { get; set; }
        public string Status { get; set; }
        public string Grade { get; set; }
        public string Remarks { get; set; }
    }

    // Student College History ....
    public class StudentClgHistry
    {
        public string SchoolName { get; set; }
        public string Grade { get; set; }
        public string EntryDate { get; set; }
        public string ExitDate { get; set; }
        public string Age { get; set; }
        public string AcademicLevels { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string OfficialWebsite { get; set; }
        public string Curriculum { get; set; }
        public string Reasonforwidthdrawel { get; set; }
        public string LastAttendenceDate { get; set; }
    }

    // Student IssueEtc ...
    public class StudentIssueEtc
    {
        public string sTCIssueId { get; set; }
        public string TCproduceNumber { get; set; }
        public string TCProduceDate { get; set; }
        public string TCIssueNumber { get; set; }
        public string TCIssueDate { get; set; }
    }

    // Student Achievements ...
    public class StudentAchievements
    {
        public string Achievements { get; set; }
        public string Class { get; set; }
        public string Date { get; set; }
        public string ImgPath { get; set; }
        public string Activity { get; set; }
    }

    // Student Medical ...
    public class StudentMedical
    {
        public string sMedicalId { get; set; }
        public string Allergies { get; set; }
        public string DocterNote { get; set; }
        public string MedicalDate { get; set; }
        public string MedicalCondition { get; set; }
        public string Vaccination { get; set; }
    }

    // Student Orivious Qualification ..
    public class StudentPriviousQualification
    {
        [Display(Name = "Privious School/College")]
        public string LAST_SCHOOL_OR_COLLEGE { get; set; }
        [Display(Name = "Privious Studied Place")]
        public string LAST_STUDIED_PLACE { get; set; }
        [Display(Name = "Exam Passed")]
        public SelectList EXAM_PASSED { get; set; }
        [Display(Name = "Privious Studied Class")]
        public string LAST_STUDIED_CLASS { get; set; }

    }

    // TransferCertificate ...
    public class StudentTransferCertificate
    {
        public string StudentId { get; set; }
        public string SerialNo { get; set; }
        public string AdmissionNo { get; set; }
        [Display(Name = "1. a) Name of the College")]
        public string CollegeName { get; set; }
        public string ClassId { get; set; }
        [Display(Name = " b) Name of the District")]
        public string DistrictName { get; set; }
        [Display(Name = "2. Name of the student(in BLOCK LETTERS) as entered in +2 or equivalent certificate")]
        public string StudentName { get; set; }
        [Display(Name = "3. Name of the Parent / Guardian")]
        public string ParentName { get; set; }
        public string Guardian { get; set; }
        [Display(Name = "4. Nationality and Religion")]
        public string Nationality { get; set; }
        public string NationalityId { get; set; }
        public string Religion { get; set; }
        public string ReligionId { get; set; }
        [Display(Name = "5. Caste and Community")]
        public string Caste { get; set; }
        public string Community { get; set; }
        public string CommunityId { get; set; }
        [Display(Name = "6. Sex")]
        public string Sex { get; set; }
        public string SexId { get; set; }
        [Display(Name = "7. Date of Birth(in figures & words) as entered in Admission Register")]
        public string DateOfBirth { get; set; }
        [Display(Name = "8. Personal Marks of Identification")]
        public string IdentificationMark1 { get; set; }
        public string IdentificationMark2 { get; set; }
        [Display(Name = "9. Date of Admission & Class which is admitted")]
        public string AdmissionDate { get; set; }
        public string AdmittedClass { get; set; }
        [Display(Name = "10. a) Class at the time of leaving")]
        public string LeavingTime { get; set; }
        [Display(Name = "b) Courses Offered - Main")]
        public string MainCourse { get; set; }
        [Display(Name = "Allied")]
        public string AlliedCourse { get; set; }
        [Display(Name = "11. Whether the student haas paid all the fees due to the College")]
        public string FeesPaid { get; set; }
        [Display(Name = "12. Whether the student was in receipt of any scholarship or any educational concessions")]
        public string StuScholarship { get; set; }
        [Display(Name = "13. Date on which the student left the College")]
        public string LeavingDate { get; set; }
        [Display(Name = "14. The Student's Contact and Character")]
        public string Conduct { get; set; }
        [Display(Name = "15. Date on which appication for Transfer Certificate was made by the sudent")]
        public string StuTCApplyDate { get; set; }
        [Display(Name = "16. Date of the Transfer Certificate")]
        public string TCIssueDate { get; set; }
        [Display(Name = "17. Course of Study")]
        public string AcademicYears { get; set; }
        public string ClassesStudied { get; set; }
        public string FirstLanguage { get; set; }
        public string DateToWords { get; set; }
        public string CertificateId { get; set; }
        public string sResult { get; set; }

    }

    // TreansferCertificate ..
    public class UpdateTransferCertirficate
    {
        public string CERTIFICATE_ID { get; set; }
        public string SERIAL_NO { get; set; }
        public string ADMISSION_NO { get; set; }
        public string STUDENT_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string GENDER { get; set; }
        public string RELIGION { get; set; }
        public string NATIONALITY { get; set; }
        public string CASTE { get; set; }
        public string COMMUNITY { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string ADMISSION_DATE { get; set; }
        public string ADMITTED_CLASS { get; set; }
        public string IDENTIFICATION_MARK1 { get; set; }
        public string IDENTIFICATION_MARK2 { get; set; }
        public string MAIN_COURSE { get; set; }
        public string ALLIED { get; set; }
        public string FEE_PAID { get; set; }
        public string SCHOLAR_SHIP { get; set; }
        public string LEAVING_DATE { get; set; }
        public string REASON_FOR_LEAVING { get; set; }
        public string LEAVING_CLASS { get; set; }
        public string CONDUCT { get; set; }
        public string TC_APPLIED_DATE { get; set; }
        public string TC_GIVEN_DATE { get; set; }
        public string FATHER_NAME { get; set; }
        public string GUARDIAN_NAME { get; set; }
        public string ACADEMIC_YEARS { get; set; }
        public string CLASSES_STUDEIED { get; set; }
        public string FIRST_LANGUAGE { get; set; }
        public string DATETOWORDS { get; set; }
    }

    // Conduct Certificate ..
    public class StudentConductCertificate
    {
        [Display(Name = "Program")]
        public string ProgramId { get; set; }
        [Display(Name = "Class")]
        public string ClassId { get; set; }
        [Display(Name = "Student")]
        public string StudentId { get; set; }
    }

    // Course Certificate ..
    public class StudentCourseCertificate
    {
        [Display(Name = "Program")]
        public string ProgramId { get; set; }
        [Display(Name = "Class")]
        public string ClassId { get; set; }
        [Display(Name = "Student")]
        public string StudentId { get; set; }
    }

    // Bonafide Certificate ..
    public class StudentBonafideCertificate
    {
        public string ProgramId { get; set; }
        public string ClassId { get; set; }
        public string StudentId { get; set; }
    }

    // DAY ORDER CALENDER
    public class DayOrderCalender
    {
        public string DAY_ID { get; set; }
        public string DAY_ORDER_ID { get; set; }
        public string DAY_TYPE { get; set; }
        public string DAY_ORDER { get; set; }
        public string DAY_ORDER_MONTH { get; set; }
        public string IS_HOLIDAY { get; set; }
        public string IS_DELETED { get; set; }
        public string COLOR { get; set; }
        public string DAY_ORDER_DATE { get; set; }
        public string DAY_ORDER_END_DATE { get; set; }

    }
    // Fetch Events ...
    public class FETCH_CALENDER_EVENTS
    {
        public string EVENT_ID { get; set; }
        public string DAY_ID { get; set; }
        public string SYEAR { get; set; }
        public string SMONTH { get; set; }
        public string SDAY { get; set; }
        public string EYEAR { get; set; }
        public string EMONTH { get; set; }
        public string EDAY { get; set; }
        public string DAY_ORDER { get; set; }
        public string RESPONSIBLE_STUDENT { get; set; }
        public string RESPONSIBLE_STAFF { get; set; }
        public string ACCESS_CATEGORY { get; set; }
        public string VISIBILITY_TYPE { get; set; }
        public string COLOR { get; set; }
        public string STAFF_ID { get; set; }
    }
    public class CALENDER_EVENTS
    {
        //Event Get By Single Query
        public string EVENT_ID { get; set; }
        public string EVENT_NAME { get; set; }
        public string DAY_ID { get; set; }
        public string EVENT_END_DATE { get; set; }
        public string EVENT_START_DATE { get; set; }
        public string EVENT_TYPE { get; set; }
        public string DAY_ORDER { get; set; }
        public string EVENT_DESCRIPTION { get; set; }
        public string EVENT_DEPARTMENT { get; set; }
        public string USER_ID { get; set; }
        public string RESPONSIBLE_STAFF { get; set; }
        public string RESPONSIBLE_STUDENT { get; set; }
        public string EVENT_REPORT { get; set; }
        public string EVENT_PARTICIPANTS { get; set; }
        public string EVENT_STATUS { get; set; }
        public string EVENT_BUDGET { get; set; }
        public string AMOUNT_SPENT { get; set; }
        public string HOLIDAY_TYPE { get; set; }
        public string DAY_ORDER_MONTH { get; set; }
        public string IS_HOLIDAY { get; set; }
        public string COLOR { get; set; }
        public string STAFF_CATEGORY { get; set; }
        public string ACCESS_VISIBILITY { get; set; }
        public string VIEW_MANAGE { get; set; }
        public string STAFF_ID { get; set; }
    }

    // Fetch Student/Staff
    public class FetchStudnetOrStaff
    {
        public string STUDENT_ID { get; set; }
        public string STAFF_ID { get; set; }
    }
    // Calender ..
    public class Calender
    {
        public string CALENDER_ID { get; set; }
        public string EVENT_TITLE { get; set; }
        public string SYEAR { get; set; }
        public string SMONTH { get; set; }
        public string SDAY { get; set; }
        public string EYEAR { get; set; }
        public string EMONTH { get; set; }
        public string EDAY { get; set; }
        public string DESCRIPTION { get; set; }
        //public string IS_ACTIVE{ get; set; }
        //public string IS_DELETED{ get; set; }
        public string COLOR { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    /// <summary>
    /// Student UI Class
    /// </summary>
    // Academic Details ...
    public class StuAcadamicInfo
    {
        [Display(Name = "Register No")]
        public string REGISTER_NO { get; set; }
        [Display(Name = "Roll No")]
        public string ROLL_NO { get; set; }
        [Display(Name = "Examination Roll No")]
        public string UNIVERSITY_ROLLNO { get; set; }
        [Display(Name = "Student E-Mail")]
        public string STU_EMAIL { get; set; }
        [Display(Name = "Admission No")]
        public string ADMINSSION_NO { get; set; }
        [Display(Name = "Admission Date")]
        public string ADMISSIONDATE { get; set; }
        [Display(Name = "Admitted Class")]
        public SelectList ADMITTED_CLASS { get; set; }
        [Display(Name = "Register Date")]
        public string STUDENTREGISTEREDDATE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "TC Serial No")]
        public string TC_SERIAL_NUMBER { get; set; }
        [Display(Name = "Class")]
        public SelectList CLASS_ID { get; set; }
        [Display(Name = "Department")]
        public SelectList DEPT_ID { get; set; }
        [Display(Name = "Program")]
        public SelectList PROGRAM_ID { get; set; }
        [Display(Name = "Batch")]
        public SelectList BATCH { get; set; }
        [Display(Name = "Academic Mentor")]
        public SelectList ACADEMIC_MENTOR { get; set; }
        [Display(Name = "Shift")]
        public SelectList SHIFT { get; set; }
        public SelectList Role { get; set; }
        public SelectList UserType { get; set; }
        [Display(Name = "Is Left")]
        public string IsLeft { get; set; }
    }

    // Personal Details ...
    public class StuPersonalInfo
    {
        public string STUDENT_ID { get; set; }
        [Display(Name = "First Name")]
        public string FIRSTNAME { get; set; }
        [Display(Name = "Last Name")]
        public string LASTNAME { get; set; }
        [Display(Name = "Caste")]
        public string CASTE { get; set; }
        [Display(Name = "Date Of Birth")]
        public string DATEOFBIRTH { get; set; }
        [Display(Name = "Annual Income")]
        public string ANNUALINCOME { get; set; }
        [Display(Name = "Residence No")]
        public string RESIDENCENO { get; set; }
        [Display(Name = "Passport No")]
        public string PASSPORTNUMBER { get; set; }
        [Display(Name = "Photo")]
        public string PHOTOPATH { get; set; }
        [Display(Name = "Role")]
        public SelectList Role { get; set; }
        [Display(Name = "User Type")]
        public SelectList UserType { get; set; }
        [Display(Name = "Visa Issue Date")]
        public string VISAISSUEDATE { get; set; }
        [Display(Name = "Visa Expiry Date")]
        public string VISAEXPIRYDATE { get; set; }
        [Display(Name = "Visa Sponsor")]
        public string VISASPONSOR { get; set; }
        [Display(Name = "Visa No")]
        public string VISANUMBER { get; set; }
        [Display(Name = "Idendification Mark1")]
        public string IDENTIFICATIONMARK1 { get; set; }
        [Display(Name = "Idendification Mark2")]
        public string IDENTIFICATIONMARK2 { get; set; }
        [Display(Name = "Contact Number")]
        public string HOMEPHONE { get; set; }
        [Display(Name = "Aadhar No")]
        public string AADHARNUMBER { get; set; }
        [Display(Name = "Student Mobile No")]
        public string STUMOBILENO { get; set; }
        [Display(Name = "Place Of Birth")]
        public string PLACEOFBIRTH { get; set; }
        [Display(Name = "Marital Status")]
        public SelectList MARITALSTATUS { get; set; }
        [Display(Name = "Gender")]
        public SelectList GENDERID { get; set; }
        [Display(Name = "Community")]
        public SelectList COMMUNITY { get; set; }
        [Display(Name = "Mother Tongue")]
        public SelectList MOTHERTONGUE { get; set; }
        [Display(Name = "Second Language")]
        public SelectList SECONDLANGUAGE { get; set; }
        [Display(Name = "Blood Group")]
        public SelectList BLOODGROUP { get; set; }
        [Display(Name = "Religion")]
        public SelectList RELIGION { get; set; }
        [Display(Name = "Nationality")]
        public SelectList NATIONALITY { get; set; }
        [Display(Name = "Mode OF Transport")]
        public SelectList MODEOFTRANSPORT { get; set; }
        [Display(Name = "Spoken Language")]
        public SelectList SPOKENLANGUAGE { get; set; }
        [Display(Name = "First Language")]
        public SelectList FIRSTLANGUAGE { get; set; }
        [Display(Name = "Specially Abled")]
        public SelectList SPECIALLYABLED { get; set; }
        [Display(Name = "Residency Type")]
        public SelectList RESIDENCYTYPE { get; set; }
        public string TYPE { get; set; }
        public string RECEIVE_ID { get; set; }
        public string FREQUENCY { get; set; }
        public string DOCUMENT_ID { get; set; }
    }

    // Parents Details ...
    public class StuParentsInfo
    {
        [Display(Name = "Father's Name")]
        public string FATHER_NAME { get; set; }
        [Display(Name = "Date Of Birth")]
        public string FDATEOFBIRTH { get; set; }
        [Display(Name = "Mother's Name")]
        public string MOTHER_NAME { get; set; }
        [Display(Name = "Date Of Birth")]
        public string MDATEOFBIRTH { get; set; }
        [Display(Name = "Office Contact")]
        public string FR_BUSINESS_PHONE { get; set; }
        [Display(Name = "Contact Number")]
        public string FR_MOBILE { get; set; }
        [Display(Name = "Father Passport No")]
        public string FR_PASSPORT_NUMBER { get; set; }
        [Display(Name = "Office Contact")]
        public string MO_BUSINESS_PHONE { get; set; }
        [Display(Name = "Contact Number")]
        public string MO_MOBILE { get; set; }
        [Display(Name = "Passport No")]
        public string MO_PASSPORT_NUMBER { get; set; }
        [Display(Name = "Father Photo")]
        public string FATHER_PHOTO { get; set; }
        [Display(Name = "Mother Photo")]
        public string MOTHER_PHOTO { get; set; }
        [Display(Name = "FATHER BUSINESS POST BOX")]
        public string FR_BUSINESS_PO_BOX { get; set; }
        [Display(Name = "MOTHER BUSINESS POST BOX")]
        public string MO_BUSINESS_PO_BOX { get; set; }

        [Display(Name = "Annual Income")]
        public string FR_INCOME { get; set; }
        [Display(Name = "Annual Income")]
        public string MO_INCOME { get; set; }
        [Display(Name = "Date Of Expiry")]
        public string FRDATEOFEXPIRY { get; set; }
        [Display(Name = "Date Of Expiry")]
        public string MODATEOFEXPIRY { get; set; }

        [Display(Name = "Father's Email")]
        public string FR_EMAIL { get; set; }
        [Display(Name = "Mother's Email")]
        public string MO_EMAIL { get; set; }
        [Display(Name = "Father's Occupation")]
        public SelectList FR_OCCUPATION { get; set; }
        [Display(Name = "Father Education")]
        public SelectList FR_EDUCATION { get; set; }
        [Display(Name = "Mother Occupation")]
        public SelectList MO_OCCUPATION { get; set; }
        [Display(Name = "Mother Education")]
        public SelectList MO_EDUCATION { get; set; }
        [Display(Name = "Father Nationality")]
        public SelectList FR_NATIONALITY { get; set; }
        [Display(Name = "Mother Nationality")]
        public SelectList MO_NATIONALITY { get; set; }
    }

    // Address Details ..
    public class StuAddressInfo
    {
        [Display(Name = "Door Details")]
        public string C_DOOR_DETAIL { get; set; }
        [Display(Name = "Street")]
        public string C_STREET { get; set; }
        [Display(Name = "Village")]
        public string C_VILLAGE_AREA { get; set; }
        [Display(Name = "Post Area")]
        public string C_POST_PLACE_TOWN { get; set; }
        [Display(Name = "Taulk")]
        public string C_TAULK { get; set; }
        [Display(Name = "City")]
        public string C_CITY { get; set; }
        [Display(Name = "District")]
        public String C_DISTRICT { get; set; }
        [Display(Name = "PinCode")]
        public string C_PINCODE { get; set; }
        [Display(Name = "Country")]
        public SelectList C_COUNTRY { get; set; }
        [Display(Name = "Door Details")]
        public string P_DOOR_DETAIL { get; set; }
        [Display(Name = "Street")]
        public string P_STREET { get; set; }
        [Display(Name = "Village")]
        public string P_VILLAGE_AREA { get; set; }
        [Display(Name = "Post Area")]
        public string P_POST_PLACE_TOWN { get; set; }
        [Display(Name = "Taluk")]
        public string P_TAULK { get; set; }
        [Display(Name = "City")]
        public string P_CITY { get; set; }
        [Display(Name = "District")]
        public string P_DISTRICT { get; set; }
        [Display(Name = "PinCode")]
        public string P_PINCODE { get; set; }
        [Display(Name = "Country")]
        public SelectList P_COUNTRY { get; set; }
    }

    // Activities Details ..
    public class StuActivitiesInfo
    {
        [Display(Name = "Post Held")]
        public string POST_HELD { get; set; }
        [Display(Name = "Initiative Shown")]
        public string INITIATIVE_SHOWN { get; set; }
        [Display(Name = "Participation")]
        public string PARTICIPATION { get; set; }
        [Display(Name = "Date From")]
        public string DATE_FROM { get; set; }
        [Display(Name = "Date To")]
        public string DATE_TO { get; set; }
        [Display(Name = "Extra Activity")]
        public string EXTRA_ACTIVITY { get; set; }
        [Display(Name = "Position Obtained")]
        public string POSITION_OBTAINED { get; set; }
        [Display(Name = "Activity")]
        public SelectList ACTIVITY { get; set; }
        [Display(Name = "Activity Image 1")]
        public string ACTIVITY_IMAGE1 { get; set; }
        [Display(Name = "Activity Image 2")]
        public string ACTIVITY_IMAGE2 { get; set; }
        [Display(Name = "Activity Image 3")]
        public string ACTIVITY_IMAGE3 { get; set; }
    }

    // Certificate Details ...
    public class StuCertificateInfo
    {
        [Display(Name = "CERTIFICATE NUMBER")]
        public string CERTIFICATE_NO { get; set; }
        [Display(Name = "RECEIVED DATE")]
        public string RECEIVEDDATE { get; set; }
        [Display(Name = "GIVEN DATE")]
        public string GIVENDATE { get; set; }
        [Display(Name = "ARCHIVE")]
        public SelectList ARCHIVE { get; set; }
        [Display(Name = "PURPOSE")]
        public string PURPOSE { get; set; }
        [Display(Name = "REGISTER NUMBER")]
        public string REGISTER_NUMBER { get; set; }
        [Display(Name = "CERTIFICATE NAME")]
        public string CERTIFICATE_NAME { get; set; }
    }

    // Account Details ...
    public class StuAccountInfo
    {
        [Display(Name = "Account No")]
        public string ACCOUNT_NO { get; set; }
        [Display(Name = "Ifsc Code")]
        public string IFSC_CODE { get; set; }
        [Display(Name = "Micr Code")]
        public string MICR_CODE { get; set; }
    }

    //Counselling Details ...
    public class StuCounsellingInfo
    {
        [Display(Name = "Date of Counseling")]
        public string DOC { get; set; }
        [Display(Name = "Duration")]
        public string DURATION { get; set; }
        [Display(Name = "Remarks")]
        public string CREMARKS { get; set; }
        [Display(Name = "Comments")]
        public string COMMENTS { get; set; }
        [Display(Name = "Batch")]
        public SelectList CBATCH { get; set; }
    }

    // Incident Details ..
    public class StuIncidentInfo
    {
        [Display(Name = "Date Of Incident")]
        public string DATEOFINCIDENT { get; set; }
        [Display(Name = "Time Of Incident")]
        public string TIME_OF_INCIDENT { get; set; }
        [Display(Name = "Place Of Incident")]
        public string PLACE_OF_INCIDENT { get; set; }
        [Display(Name = "FirstAid Done")]
        public string FIRST_AID_DONE { get; set; }
        [Display(Name = "Informed To Parents")]
        public SelectList INFORMED_TO_PARENTS { get; set; }
        [Display(Name = "Date Of Informed")]
        public string DATEINFORMED { get; set; }
        [Display(Name = "Incident Details")]
        public string INCIDENT_DETAILS { get; set; }
    }

    // Sibling Details ...
    public class StuSiblingInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Institute_Name { get; set; }
        public SelectList Program { get; set; }
        public string Qualification { get; set; }
        public SelectList Employed { get; set; }
        public string DateofBirth { get; set; }
        [Display(Name = "Gender")]
        public SelectList Gender { get; set; }
        public SelectList Occupation { get; set; }
        public string Monthly_Income { get; set; }
        public string ProgName { get; set; }
        public SelectList Is_Same_Collage { get; set; }
    }

    // Talents Detais .,,
    public class StuTalentsInfo
    {
        public string Date { get; set; }
        [Display(Name = "Talent Area")]
        public SelectList TalentArea { get; set; }
        [Display(Name = "Talent Activity Type")]
        public SelectList TalentActivityType { get; set; }
        [Display(Name = "Talent Discription")]
        public string TalentDiscription { get; set; }
        public string Status { get; set; }
        public SelectList Grade { get; set; }
        public string Remarks { get; set; }
    }

    // College History Details ..
    public class StuClgHistoryInfo
    {
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }
        [Display(Name = "Grade")]
        public SelectList CGrade { get; set; }
        [Display(Name = "Entry Date")]
        public string EntryDate { get; set; }
        [Display(Name = "Exit Date")]
        public string ExitDate { get; set; }
        [Display(Name = "Age")]
        public int CAge { get; set; }
        [Display(Name = "Academic Levels")]
        public string AcademicLevels { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Country")]
        public SelectList Country { get; set; }
        [Display(Name = "Official Website")]
        public string OfficialWebsite { get; set; }
        [Display(Name = "Curriculum")]
        public string Curriculum { get; set; }
        [Display(Name = "Reason For Widthdrawel")]
        public string Reasonforwidthdrawel { get; set; }
        [Display(Name = "Last Attendentce Date")]
        public string LastAttendenceDate { get; set; }
    }

    // Tc issue etc Details ...
    public class TcIssueEtcInfo
    {
        [Display(Name = "TC Produce Number")]
        public string TCproduceNumber { get; set; }
        [Display(Name = "TC Produce Date")]
        public string TCProduceDate { get; set; }
        [Display(Name = "TC Issue Number")]
        public string TCIssueNumber { get; set; }
        [Display(Name = "TC Produce Date")]
        public string TCIssueDate { get; set; }
    }

    // Achivement Details ...
    public class StuAchivementInfo
    {
        [Display(Name = "Achievements")]
        public string ACHIEVEMENTS { get; set; }
        [Display(Name = "Class")]
        public SelectList ACLASSID { get; set; }
        [Display(Name = "Date")]
        public string DATE { get; set; }
        [Display(Name = "Img Path")]
        public string IMAGE_PATH { get; set; }
        [Display(Name = "Activity")]
        public SelectList AACTIVITY { get; set; }
    }

    // Medical Details ....
    public class StuMedicalInfo
    {
        public string Allergies { get; set; }
        public string DocterNote { get; set; }
        public string MedicalDate { get; set; }
        public string MedicalCondition { get; set; }
        public string Vaccination { get; set; }
    }

    // TransferCertificate ...
    public class TransferCertificate
    {
        [Display(Name = "Program")]
        public SelectList ProgramName { get; set; }
        [Display(Name = "Class")]
        public SelectList ClassName { get; set; }
        [Display(Name = "Shift")]
        public SelectList ShiftList { get; set; }
        [Display(Name = "Student")]
        public SelectList StuName { get; set; }
        public string SerialNo { get; set; }
        public string AdmissionNo { get; set; }
        [Display(Name = "1. a) Name of the College")]
        public string CollegeName { get; set; }
        [Display(Name = " b) Name of the District")]
        public string DistrictName { get; set; }
        [Display(Name = "2. Name of the student(in BLOCK LETTERS) as entered in +2 or equivalent certificate")]
        public string StudentName { get; set; }
        [Display(Name = "3. Name of the Parent / Guardian")]
        public string ParentName { get; set; }
        public string Guardian { get; set; }
        [Display(Name = "4. Nationality and Religion")]
        public SelectList Nationality { get; set; }
        public SelectList Religion { get; set; }
        [Display(Name = "5. Caste and Community")]
        public string Caste { get; set; }
        public SelectList Community { get; set; }
        [Display(Name = "6. Sex")]
        public SelectList Sex { get; set; }
        [Display(Name = "7. Date of Birth(in figures & words) as entered in Admission Register")]
        public string DateOfBirth { get; set; }
        public string DateToWords { get; set; }
        [Display(Name = "8. Personal Marks of Identification")]
        public string IdentificationMark1 { get; set; }
        public string IdentificationMark2 { get; set; }
        [Display(Name = "9. Date of Admission & Class which is admitted")]
        public string AdmissionDate { get; set; }
        public string AdmittedClass { get; set; }
        [Display(Name = "10. a) Class at the time of leaving")]
        public string LeavingTime { get; set; }
        [Display(Name = "b) Courses Offered - Main")]
        public string MainCourse { get; set; }
        [Display(Name = "Allied")]
        public string AlliedCourse { get; set; }
        public string AlliedCourse2 { get; set; }
        [Display(Name = "11. Whether the student haas paid all the fees due to the College")]
        public SelectList FeesPaid { get; set; }
        [Display(Name = "12. Whether the student was in receipt of any scholarship or any educational concessions")]
        public SelectList StuScholarship { get; set; }
        [Display(Name = "13. Date on which the student left the College")]
        public string LeavingDate { get; set; }
        [Display(Name = "14. The Student's Contact and Character")]
        public SelectList Conduct { get; set; }
        [Display(Name = "15. Date on which appication for Transfer Certificate was made by the sudent")]
        public string StuTCApplyDate { get; set; }
        [Display(Name = "16. Date of the Transfer Certificate")]
        public string TCIssueDate { get; set; }
        [Display(Name = "Academic Years")]
        public string AcademicYears { get; set; }
        [Display(Name = "Class Studied")]
        public string ClassesStudied { get; set; }
        [Display(Name = "First Language")]
        public string FirstLanguage { get; set; }
    }

    // Conduct Certificate ..
    public class ConductCertificate
    {
        [Display(Name = "Shift")]
        public SelectList ShiftList { get; set; }
        [Display(Name = "Program")]
        public SelectList ProgramId { get; set; }
        [Display(Name = "Class")]
        public SelectList ClassId { get; set; }
        [Display(Name = "Student")]
        public SelectList StudentId { get; set; }
    }

    // Course Certificate ..
    public class CourseCertificate
    {
        [Display(Name = "Shift")]
        public SelectList ShiftList { get; set; }
        [Display(Name = "Program")]
        public SelectList ProgramId { get; set; }
        [Display(Name = "Class")]
        public SelectList ClassId { get; set; }
        [Display(Name = "Student")]
        public SelectList StudentId { get; set; }
    }

    // Bonafide Certificate ..
    public class BonafideCertificate
    {
        [Display(Name = "Roll No")]
        public string ROLL_NO { get; set; }
        [Display(Name = "Name")]
        public string NAME { get; set; }
        [Display(Name = "Class")]
        public string CLASS { get; set; }
        [Display(Name = "Father Name")]
        public string FATHER_NAME { get; set; }
        [Display(Name = "During Year")]
        public string DURING_YEAR { get; set; }
        public string BONAFIDE_ID { get; set; }
        public string Student_Id { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string sResult { get; set; }
    }

    // Student Details ...
    public class StudentInfo
    {
        // Student List Details ..
        public string STUDENTID { get; set; }
        public string ROLLNO { get; set; }
        public string FIRSTNAME { get; set; }
        public string DEPTID { get; set; }
        public string GENDER { get; set; }
        public string SHIFTID { get; set; }
        public string CLASS_NAME { get; set; }
        public string IS_LEFT { get; set; }




        // Student Courses ...
        [Display(Name = "Program Name")]
        public SelectList PROGRAMID { get; set; }
        [Display(Name = "Class")]
        public SelectList CLASSID { get; set; }
        [Display(Name = "Part")]
        public SelectList PART { get; set; }
        [Display(Name = "Main Subject")]
        public string MAIN_SUBJECT { get; set; }
        [Display(Name = "Allied1")]
        public string ALLIED1 { get; set; }
        [Display(Name = "Allied2")]
        public string ALLIED2 { get; set; }
        [Display(Name = "Allied3")]
        public string ALLIED3 { get; set; }
        [Display(Name = "Allied4")]
        public string ALLIED4 { get; set; }
        [Display(Name = "Elecctive1")]
        public string ELECTIVE1 { get; set; }
        [Display(Name = "Elecctive2")]
        public string ELECTIVE2 { get; set; }
        [Display(Name = "Elecctive3")]
        public string ELECTIVE3 { get; set; }
        [Display(Name = "Elecctive4")]
        public string ELECTIVE4 { get; set; }
        [Display(Name = "Special Subject")]
        public string SPECIAL_SUBJECT { get; set; }


        // Student Report Template
        public string REPORT_ID { get; set; }
        public string MODULE { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_ON { get; set; }
        public string REPORT_NAME { get; set; }
        public string FIELD_LIST { get; set; }
        public string CONDITION { get; set; }
        public string ORDER_BY_FIELD { get; set; }
        public string GROUP_BY_FIELD { get; set; }
        public string STATISSTIC_FIELD { get; set; }
        public string COUNT_FIELD { get; set; }
        public string COUNT_CAPTION { get; set; }

        // Student Leave Letter ...
        public string LEAVE_LETTER_TITLE { get; set; }
        public string LEAVE_LETTER_FORMAT { get; set; }
        public string LETTER_FOR { get; set; }
    }

    // GET ACADEMIC YEAR ..
    public class GET_ACADEMIC_YEAR_BY_ID
    {
        public string STUDENT_ID { get; set; }
        public string ACADEMIC_YEAR_ID { get; set; }
        public string AC_YEAR { get; set; }
        public string SEMESTER { get; set; }
        public string IS_ACTIVE { get; set; }
        public string RESULT { get; set; }
    }

    public class Get_Allied_Subjects
    {
        public string COURSE_CODE { get; set; }
        public string COURSE_TITLE { get; set; }
        public string CLASS_ID { get; set; }
        public string IS_ALLIED_SUBJECT { get; set; }
    }
    #region Student Promotion
    public class JsonStudentPromotionList
    {
        public List<StudentPromotionList> StudentPromotionList { get; set; }
    }
    public class StudentPromotionList
    {
        public string SHIFT { get; set; }
        public string CLASS_ID { get; set; }
        public string cCLASS_ID { get; set; }
        public string pCLASS_ID { get; set; }
        public string CLASS_CODE { get; set; }
        public string COURSE_INFO_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string DEPARTMENT { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string ROLL_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string STU_CLASS_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string cACADEMIC_YEAR { get; set; }
        public string pACADEMIC_YEAR { get; set; }
        public string ACADEMIC_YEAR_ID { get; set; }
        public string PROGRAMME { get; set; }
        public string cPROGRAMME { get; set; }
        public string tPROGRAMME { get; set; }
        public string CLASS_YEAR { get; set; }
        public string CLASS_LEVEL { get; set; }
    }
    #endregion
    #region Stu Course Info
    public class JsonStuCourseInfo
    {
        public List<Stu_Course_Info> SelectedStudentList { get; set; }
    }
    public class Stu_Course_Info
    {
        public string COURSE_INFO_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string ROLL_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string COURSE_ID { get; set; }
        public string SEMESTER { get; set; }
        public string COURSE_TITLE { get; set; }
        public string SHIFT { get; set; }
        public string S_CLASS_ID { get; set; }
        public string PROGRAMME { get; set; }
        public string BATCH { get; set; }
        public string ACADEMIC_YEAR { get; set; }
    }
    #endregion
    #region ASSIGNMENT SUBMISSION
    public class ASSIGNMENT_SUBMISSION
    {
        public string ASS_SUBMISSION_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string ASSIGNMENT_ID { get; set; }
        public string ASSIGNMENT_NAME { get; set; }
        public string FILE_PATH { get; set; }
        public string SUBMITTED_DATE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string STAFF_ID { get; set; }
        public string COURSE_CODE { get; set; }
    }
    #endregion
}
