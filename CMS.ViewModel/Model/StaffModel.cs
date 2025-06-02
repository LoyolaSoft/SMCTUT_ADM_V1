using System.ComponentModel;
using System.Web.Mvc;

namespace CMS.ViewModel.Model
{
    public class StaffModel
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string StaffCode { get; set; }
        public string NickName { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Maritalstatus { get; set; }
        public string DateofBirth { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string MotherTongue { get; set; }
        public string Department { get; set; }
        public string Photo { get; set; }
        public string DateOfJoin { get; set; }
        public string Community { get; set; }
        public string Nationality { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
        public string UserType { get; set; }

    }
    // Staff Personal Information ...
    public class StaffPersonal
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string NickName { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Maritalstatus { get; set; }
        public string DateofBirth { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string MotherTongue { get; set; }
        public string Department { get; set; }
        public string Photo { get; set; }
        public string DateOfJoin { get; set; }
        public string Community { get; set; }
        public string Nationality { get; set; }
        public string Image { get; set; }
        public string StaffCode { get; set; }
        public string Role { get; set; }
        public string UserType { get; set; }
    }
    // Staff List ...
    public class StaffList
    {
        public string STAFFID { get; set; }
        public string STAFFCODE { get; set; }
        public string FIRSTNAME { get; set; }
        public string GENDERNAME { get; set; }
        public string DEPARTMENT { get; set; }
        public string SHIFTNAME { get; set; }
    }
    // Staff Address
    public class StaffAddress
    {
        public string CDOORDETAILS { get; set; }
        public string CSTREET { get; set; }
        public string CPLACE { get; set; }
        public string CCITY { get; set; }
        public string CPINCODE { get; set; }
        public string CDISTRICT { get; set; }
        public string CVILLAGE { get; set; }
        public string CCOUNTRY { get; set; }
        public string CPHONENO { get; set; }
        public string CCELLNO { get; set; }
        public string CEMAIL { get; set; }
        public string PSTREET { get; set; }
        public string PVILLAGE { get; set; }
        public string PPLACE { get; set; }
        public string PCITY { get; set; }
        public string PPINCODE { get; set; }
        public string PDISTRICT { get; set; }
        public string PCOUNTRY { get; set; }
        public string PPHONENO { get; set; }
        public string PCELLNO { get; set; }
        public string PDOORDETAILS { get; set; }
        public string PEMAIL { get; set; }
    }
    // Staff Service ... 
    public class StaffService
    {
        public string SERVICEID { get; set; }
        public string DATEOFAPPOINT { get; set; }
        public string NAME { get; set; }
        public string APPOINTMENTNATURE { get; set; }
        public string DATEOFTERMINATION { get; set; }
        public string INSTITUTE { get; set; }
        public string REMARKS { get; set; }
        public string Place { get; set; }
    }

    // Staff Couseling ..
    public class StaffCounseling
    {
        public string CounsellingID { get; set; }
        public string DateOfCounsel { get; set; }
        public string DURATION { get; set; }
        public string REASON { get; set; }
        public string GIVEN { get; set; }
        public string ACTIONTAKEN { get; set; }
        public string REMARK { get; set; }
    }

    // Staff Paper
    public class StaffPaper
    {
        public string PAPERID { get; set; }
        public string PAPERNAME { get; set; }
        public string LEVEL { get; set; }
        public string PUBLISHINGCATEGORY { get; set; }
        public string JOURNALNAME { get; set; }
        public string NOOFPAGES { get; set; }
        public string PAGESFROM { get; set; }
        public string PAGESTO { get; set; }
        public string VOLUME { get; set; }
        public string YEARPUBLISHED { get; set; }
    }

    // Staff Publish
    public class StaffPublish
    {
        public string PUBLISHID { get; set; }
        public string BOOKNAME { get; set; }
        public string PLEVEL { get; set; }
        public string PPUBLISHINGCATEGORY { get; set; }
        public string PJOURNALNAME { get; set; }
        public string PVOLUME { get; set; }
        public string PUBLISHER { get; set; }
        public string PYEAR { get; set; }
        public string EDITION { get; set; }
    }

    // Staff Qualification ..
    public class StaffQualification
    {
        public string QualificationId { get; set; }
        public string MainSubject { get; set; }
        public string AilledSubject { get; set; }
        public string Completion { get; set; }
        public string InstituteOfStudy { get; set; }
        public string University { get; set; }
        public string Percentange { get; set; }
        public string CompletionYear { get; set; }
        public string CompletionMonth { get; set; }
        public string Degree { get; set; }
        public string DegreeType { get; set; }
    }

    // Staff Leaving ..
    public class StaffLeaving
    {
        public string LeavingDate { get; set; }
        public string LeavingRemark { get; set; }
        public string LeavingReason { get; set; }
        public string RetirementDate { get; set; }
        public string Department { get; set; }
    }

    // Staff Training ..
    public class StaffTraining
    {
        public string TraininngId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string PROGRAMME { get; set; }
        public string PLACE { get; set; }
        public string LEVEL { get; set; }
        public string COMMENTS { get; set; }
    }

    // Subject Details ...
    public class StaffRoles
    {
        // <!-- Subject --!>
        public string KNOWLEDGEINCOMPUTER { get; set; }
        public string MAINSUBJECT { get; set; }
        public string QUALIFICATION { get; set; }
        public string DESIGNATION { get; set; }
        public string NONTEACHINGCATEGORY { get; set; }
        public string SUBCATEGORY { get; set; }
        public string BOARDMEMBER { get; set; }
        public string SESSIONS { get; set; }
        public string SHIFT { get; set; }
        public string ATTSHIFT { get; set; }
        public string StaffCode { get; set; }
        public string STAFFID { get; set; }
    }

    // Staff Family ..
    public class Family
    {
        public string FamilyId { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public string DateOfBirth { get; set; }
    }

    // Staff Personal Infomation ...
    public class StaffPersonalInfo
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string Lastname { get; set; }
        [DisplayName("Know As")]
        public string KnowAs { get; set; }
        [DisplayName("Place Of Birth")]
        public string PlaceOfBirth { get; set; }
        [DisplayName("Date of Birth")]
        public string DateofBirth { get; set; }
        [DisplayName("Photo")]
        public string Photo { get; set; }
        [DisplayName("Date Of Join")]
        public string DateOfJoin { get; set; }
        [DisplayName("Staff Code")]
        public string StaffCode { get; set; }
        [DisplayName("Marital status")]
        public SelectList Maritalstatus { get; set; }
        [DisplayName("Blood Group")]
        public SelectList BloodGroup { get; set; }
        [DisplayName("Religion")]
        public SelectList Religion { get; set; }
        [DisplayName("Role")]
        public SelectList Role { get; set; }
        [DisplayName("User Type")]
        public SelectList UserType { get; set; }
        [DisplayName("Mother Tongue")]
        public SelectList MotherTongue { get; set; }
        [DisplayName("Department")]
        public SelectList Department { get; set; }
        [DisplayName("Community")]
        public SelectList Community { get; set; }
        [DisplayName("Nationality")]
        public SelectList Nationality { get; set; }
        [DisplayName("Gender")]
        public SelectList Gender { get; set; }
    }

    // Staff Qualification ....
    public class StaffQualificationInfo
    {
        [DisplayName("Main Subject")]
        public string MAINSUBJECT { get; set; }
        [DisplayName("Allied Subject")]
        public string ALLIEDSUBJECT { get; set; }
        [DisplayName("Completion Month")]
        public string COMPLETIONMONTH { get; set; }
        [DisplayName("Institute Of Study")]
        public string INSTITUTEOFSTUDY { get; set; }
        [DisplayName("University")]
        public string UNIVERSITY { get; set; }
        [DisplayName("Percentage")]
        public string PERCENTAGE { get; set; }
        [DisplayName("Completion Year")]
        public string COMPLETIONYEAR { get; set; }
        [DisplayName("Degree")]
        public SelectList DEGREE { get; set; }
        [DisplayName("Degree Type")]
        public SelectList DEGREETYPE { get; set; }
    }

    // Staff Roles ....
    public class StaffRolesInfo
    {
        [DisplayName("Knowledge In Computer")]
        public string KNOWLEDGEINCOMPUTER { get; set; }
        [DisplayName("Main Subject")]
        public string MAINSUBJECT { get; set; }
        [DisplayName("Qualification")]
        public SelectList QUALIFICATION { get; set; }
        [DisplayName("Designation")]
        public SelectList DESIGNATION { get; set; }
        [DisplayName("Sub Category")]
        public SelectList NONTEACHINGCATEGORY { get; set; }
        [DisplayName("Category")]
        public SelectList SUBCATEGORY { get; set; }
        [DisplayName("Board Member")]
        public SelectList BOARDMEMBER { get; set; }
        [DisplayName("Sessions")]
        public string SESSIONS { get; set; }
        [DisplayName("Shift")]
        public SelectList SHIFT { get; set; }
        [DisplayName("AttShift")]
        public string ATTSHIFT { get; set; }
    }

    // Staff Counseling ..
    public class StaffCounselingInfo
    {
        [DisplayName("Date of Counsel")]
        public string DATEOFCOUNSELLING { get; set; }
        [DisplayName("Duration")]
        public string DURATION { get; set; }
        [DisplayName("Reason")]
        public string REASON { get; set; }
        [DisplayName("Given")]
        public string GIVEN { get; set; }
        [DisplayName("Action Taken")]
        public string ACTIONTAKEN { get; set; }
        [DisplayName("Remark")]
        public string REMARK { get; set; }
    }

    // Staff Publish ...
    public class StaffPublishInfo
    {
        //<!-- Publish --!>
        [DisplayName("Book Name")]
        public string BOOKNAME { get; set; }
        [DisplayName("Level")]
        public SelectList PLEVEL { get; set; }
        [DisplayName("Publish Category")]
        public SelectList PPUBLISHINGCATEGORY { get; set; }
        [DisplayName("Journal Name")]
        public string PJOURNALNAME { get; set; }
        [DisplayName("Volume")]
        public SelectList PVOLUME { get; set; }
        [DisplayName("Publisher")]
        public string PUBLISHER { get; set; }
        [DisplayName("Year")]
        public string PUBLISHEDYEAR { get; set; }
        [DisplayName("Edition")]
        public string EDITION { get; set; }
    }

    // Staff Address ..
    public class StaffAddressInfo
    {
        [DisplayName("Door No")]
        public string CDOORDETAILS { get; set; }
        [DisplayName("Street")]
        public string CSTREET { get; set; }
        [DisplayName("Place")]
        public string CPLACE { get; set; }
        [DisplayName("City")]
        public string CCITY { get; set; }
        [DisplayName("Pin Code")]
        public string CPINCODE { get; set; }
        [DisplayName("District")]
        public string CDISTRICT { get; set; }
        [DisplayName("Village")]
        public string CVILLAGE { get; set; }
        [DisplayName("Country")]
        public SelectList CCOUNTRY { get; set; }
        [DisplayName("Phone No")]
        public string CPHONENO { get; set; }
        [DisplayName("Mobile No")]
        public string CCELLNO { get; set; }
        [DisplayName("Email")]
        public string CEMAIL { get; set; }
        [DisplayName("Street")]
        public string PSTREET { get; set; }
        [DisplayName("Village")]
        public string PVILLAGE { get; set; }
        [DisplayName("Place")]
        public string PPLACE { get; set; }
        [DisplayName("City")]
        public string PCITY { get; set; }
        [DisplayName("Pin Code")]
        public string PPINCODE { get; set; }
        [DisplayName("District")]
        public string PDISTRICT { get; set; }
        [DisplayName("Country")]
        public SelectList PCOUNTRY { get; set; }
        [DisplayName("Phone No")]
        public string PPHONENO { get; set; }
        [DisplayName("Mobile No")]
        public string PCELLNO { get; set; }
        [DisplayName("Door No")]
        public string PDOORDETAILS { get; set; }
        [DisplayName("Email")]
        public string PEMAIL { get; set; }
    }

    // Staff paper ...
    public class StaffPaperInfo
    {
        [DisplayName("Paper Name")]
        public string PAPERNAME { get; set; }
        [DisplayName("Level")]
        public SelectList LEVEL { get; set; }
        public string PaperLevel { get; set; }
        [DisplayName("Publish Category")]
        public SelectList PUBLISHINGCATEGORY { get; set; }
        public string PaperPublishCategory { get; set; }
        [DisplayName("Journal Name")]
        public string JOURNALNAME { get; set; }
        [DisplayName("No of Pages")]
        public int NOOFPAGES { get; set; }
        public string PaperNoOfPages { get; set; }
        [DisplayName("Pages From")]
        public int PAGESFROM { get; set; }
        public string PaperPagesFrom { get; set; }
        [DisplayName("Pages To")]
        public int PAGESTO { get; set; }
        public string PaperPagesto { get; set; }
        [DisplayName("Volume")]
        public SelectList VOLUME { get; set; }
        public string PaperVolume { get; set; }
        [DisplayName("Publish Year")]
        public string YEARPUBLISHED { get; set; }
    }

    // Staff Leaving ..
    public class StaffLeavingInfo
    {
        [DisplayName("Leaving Date")]
        public string LEAVINGDATE { get; set; }
        [DisplayName("Leaving Remark")]
        public string LEAVINGREMARKS { get; set; }
        [DisplayName("Leaving Reason")]
        public string LEAVINGREASON { get; set; }
        [DisplayName("Retirement Date")]
        public string RETIREMENTDATE { get; set; }
        [DisplayName("Department")]
        public SelectList DEPARTMENT { get; set; }
    }

    // Staff Services ..
    public class StaffServiceInfo
    {
        [DisplayName("Date of Appoint")]
        public string DATEOFAPPOINT { get; set; }
        [DisplayName("Appointment Name")]
        public string APPOINTMENTNAME { get; set; }
        [DisplayName("Appointment Nature")]
        public SelectList APPOINTMENTNATURE { get; set; }
        [DisplayName("Date of Termination")]
        public string DATEOFTERMINATION { get; set; }
        [DisplayName("Institute")]
        public string INSTITUTE { get; set; }
        [DisplayName("Remark")]
        public string REMARKS { get; set; }
        [DisplayName("Place")]
        public string PLACE { get; set; }
    }

    // Staff Training ..
    public class StaffTrainingInfo
    {
        [DisplayName("Date From")]
        public string DATEFROM { get; set; }
        [DisplayName("Date To")]
        public string DATETO { get; set; }
        [DisplayName("Programme")]
        public string PROGRAMME { get; set; }
        [DisplayName("Place")]
        public string PLACE { get; set; }
        [DisplayName("Level")]
        public string TLEVEL { get; set; }
        [DisplayName("Comments")]
        public string COMMENTS { get; set; }
    }

    // Staff Family ..
    public class StaffFamilyInfo
    {
        public string Name { get; set; }
        public SelectList Relation { get; set; }
        public string DateOfBirth { get; set; }
    }

    public class StaffInfo
    {
        //<Staff List>
        [DisplayName("Staff Id")]
        public string LSTAFFID { get; set; }
        [DisplayName("Staff Code")]
        public string LSTAFFCODE { get; set; }
        [DisplayName("First Name")]
        public string LFIRSTNAME { get; set; }
        [DisplayName("Gender")]
        public string LGENDERNAME { get; set; }
        [DisplayName("Shift")]
        public string LSHIFTNAME { get; set; }
        [DisplayName("Department")]
        public string LDEPARTMENT { get; set; }
    }
    #region Edit Staff Profile
    #region Staff Personal Profile
    // STAFF PERSONAL
    public class STAFF_PERSONAL
    {
        public string STAFF_ID { get; set; }
        [DisplayName("First Name")]
        public string FIRST_NAME { get; set; }
        [DisplayName("Last Name")]
        public string LAST_NAME { get; set; }
        [DisplayName("Know As")]
        public string KNOWN_AS { get; set; }
        [DisplayName("Place Of Birth")]
        public string PLACE_OF_BIRTH { get; set; }
        [DisplayName("Date of Birth")]
        public string DATE_OF_BIRTH { get; set; }
        [DisplayName("Date Of Join")]
        public string DATE_OF_JOIN { get; set; }
        [DisplayName("Staff Code")]
        public string STAFF_CODE { get; set; }
        [DisplayName("Marital status")]
        public string MARITAL_STATUS { get; set; }
        [DisplayName("Blood Group")]
        public string BLOOD_GROUP { get; set; }
        [DisplayName("Religion")]
        public string RELIGION { get; set; }
        [DisplayName("Mother Tongue")]
        public string MOTHER_TONGUE { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT { get; set; }
        [DisplayName("Community")]
        public string COMMUNITY { get; set; }
        [DisplayName("Nationality")]
        public string NATIONALITY { get; set; }
        [DisplayName("Gender")]
        public string GENDER { get; set; }
    }
    #endregion
    #region Edti Staff Address Profile
    public class STAFF_ADDRESS
    {
        public string ADDRESS_NO { get; set; }
        public string STAFFNO { get; set; }
        [DisplayName("Door No")]
        public string CDOOR_DETAILS { get; set; }
        [DisplayName("Street")]
        public string CSTREET { get; set; }
        [DisplayName("Place")]
        public string CPLACE { get; set; }
        [DisplayName("City")]
        public string CCITY { get; set; }
        [DisplayName("Pin Code")]
        public string CPIN_CODE { get; set; }
        [DisplayName("District")]
        public string CDISTRICT { get; set; }
        [DisplayName("Village")]
        public string CVILLAGE { get; set; }
        [DisplayName("Country")]
        public string CCOUNTRY { get; set; }
        [DisplayName("Phone No")]
        public string CPHONE_NO { get; set; }
        [DisplayName("Mobile No")]
        public string CCELL_NO { get; set; }
        [DisplayName("Email")]
        public string CEMAIL { get; set; }
        [DisplayName("Street")]
        public string PSTREET { get; set; }
        [DisplayName("Village")]
        public string PVILLAGE { get; set; }
        [DisplayName("Place")]
        public string PPLACE { get; set; }
        [DisplayName("City")]
        public string PCITY { get; set; }
        [DisplayName("Pin Code")]
        public string PPIN_CODE { get; set; }
        [DisplayName("District")]
        public string PDISTRICT { get; set; }
        [DisplayName("Country")]
        public string PCOUNTRY { get; set; }
        [DisplayName("Phone No")]
        public string PPHONE_NO { get; set; }
        [DisplayName("Mobile No")]
        public string PCELL_NO { get; set; }
        [DisplayName("Door No")]
        public string PDOOR_DETAILS { get; set; }
        [DisplayName("Email")]
        public string PEMAIL { get; set; }
    }
    #endregion
    #region Edit Staff Services Profile
    public class STAFF_SERVICES
    {
        public string SERVICE_NO { get; set; }
        public string STAFF_NO { get; set; }
        [DisplayName("Date of Appoint")]
        public string DATE_OF_APPOINT { get; set; }
        [DisplayName("Appointment Name")]
        public string APPOINTMENT_NAME { get; set; }
        public string APPOINTMENT_NATURE { get; set; }
        [DisplayName("Date of Termination")]
        public string DATE_OF_TERMINATION { get; set; }
        [DisplayName("Institute")]
        public string INSTITUTE { get; set; }
        [DisplayName("Remark")]
        public string REMARKS { get; set; }
        [DisplayName("Place")]
        public string PLACE { get; set; }
    }
    #endregion
    #region View Staff Profile
    public class STAFFPROFILEVIEW
    {
        public string STAFF_ID { get; set; }
        [DisplayName("Name")]
        public string FIRST_NAME { get; set; }
        [DisplayName("Staff code")]
        public string STAFF_CODE { get; set; }
        [DisplayName("Gender")]
        public string GENDER_NAME { get; set; }
        [DisplayName("Date Of Birth")]
        public string DATE_OF_BIRTH { get; set; }
        [DisplayName("Date Of Join")]
        public string DATE_OF_JOIN { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT { get; set; }
        [DisplayName("Nationality")]
        public string NATIONALITY_NAME { get; set; }
        [DisplayName("Qualification")]
        public string QUALIFICATION { get; set; }
        [DisplayName("Staff Category")]
        public string STF_CATEGORY { get; set; }
        [DisplayName("Shift")]
        public string SHIFT_NAME { get; set; }
        public string PHOTO { get; set; }

    }
    #endregion
    #region Staff Papar
    public class STAFF_PAPER
    {
        public string PAPER_ID { get; set; }
        public string STAFF_NO { get; set; }
        public string PAPER_NAME { get; set; }
        public string LEVEL { get; set; }
        public string PUBLISH_CATEGORY { get; set; }
        public string JOURNAL_NAME { get; set; }
        public string NO_OF_PAGES { get; set; }
        public string PAGES_FROM { get; set; }
        public string PAGES_TO { get; set; }
        public string VOLUME { get; set; }
        public string YEAR_PUBLISHED { get; set; }
    }
    #endregion
    #region Staff Publish
    public class STAFF_PUBLISH
    {
        public string PUBLISH_ID { get; set; }
        public string STAFF_NO { get; set; }
        public string BOOK_NAME { get; set; }
        public string LEVEL { get; set; }
        public string PUBLISH_CATEGORY { get; set; }
        public string JOURNAL_NAME { get; set; }
        public string VOLUME { get; set; }
        public string PUBLISHER { get; set; }
        public string PYEAR { get; set; }
        public string EDITION { get; set; }
    }
    #endregion
    #region Staff Training
    public class STAFF_TRAINING
    {
        public string TRAINING_NO { get; set; }
        public string STAFF_NO { get; set; }
        public string DATE_FROM { get; set; }
        public string DATE_TO { get; set; }
        public string PROGRAMME { get; set; }
        public string PLACE { get; set; }
        public string TLEVEL { get; set; }
        public string COMMENTS { get; set; }
    }
    #endregion
    #region Staff Counseling
    public class STAFF_COUNSELING
    {
        public string STF_COUN_ID { get; set; }
        public string STAFF { get; set; }
        public string DATE_OF_COUN { get; set; }
        public string DURATION { get; set; }
        public string REASON { get; set; }
        public string GIVEN { get; set; }
        public string ACTION_TAKEN { get; set; }
        public string REMARK { get; set; }
    }
    #endregion
    #endregion
}
