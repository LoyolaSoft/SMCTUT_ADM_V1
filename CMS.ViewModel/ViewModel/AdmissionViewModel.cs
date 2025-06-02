using CMS.ViewModel.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CMS.ViewModel.ViewModel
{
    public class AdmissionViewModel
    {
        public List<ADM_ISSUED_APPLICATION_RANKLIST> liIssuedApplicationRank { get; set; }
        public List<ADM_ISSUED_APPLICATION_RANKLIST> liIssuedApplication { get; set; }
        public List<Sup_Shift> shift { get; set; }
        public List<SUP_FIELDS> LiFields { get; set; }
        public List<SUP_FIELDS> LiColumns { get; set; }

        public List<ADM_APPLICATION_TYPE> applicationType { get; set; }
        public List<ADM_APPTYPE_PROG_GROUPS> liCourses { get; set; }
        public List<ADM_ISSUED_APPLICATIONS> liIssuedApplictionsInfo { get; set; }

        public List<ADM_TRANSFER_REQUEST> liAdmTransferRequest { get; set; }

        public List<ADM_RECEIVE_APPLICATION> liApplicant { get; set; }
        public List<ADM_PROG_ELIGIBILITY> liProgrammeEligibility { get; set; }
        public List<ADM_SELECTION_PROCESS> liSelectionProcess { get; set; }
        public List<ADM_TRANSFER> liTransfer { get; set; }
        public List<ADM_TRANSFER> liTransferExist { get; set; }
        public List<ADM_TRANSFER> liAdmissionCancel { get; set; }
        public List<ADM_ELEGIBILITIES> liEligibility { get; set; }
        public List<ADM_STU_SUBMARKS> liADMStudentMarks { get; set; }
        public List<ADM_STU_SUBMARKS> liEleventhMarks { get; set; }
        public List<ADM_STU_SUBMARKS> liADMStudentMarksFor11th { get; set; }
        public List<ADM_STU_RELATIONS> liADMStudentStudentRelations { get; set; }
        public List<SUP_RELATION> liRelation { get; set; }
        public List<Sup_Occupation> liOccupation { get; set; }
        public ADM_ISSUE_APPLICATION_2018 Issue = new ADM_ISSUE_APPLICATION_2018();
        public SELECTIONPROCCESS SelectionProcess = new SELECTIONPROCCESS();
        public ADM_PROGRAMME_DESCRIPTION ProgrammeDescription = new ADM_PROGRAMME_DESCRIPTION();
        public ADM_TRANSFER Transfer = new ADM_TRANSFER();
        public List<FEE_STUDENT_ACCOUNT> liFeeStudentAccount { get; set; }
        public List<FEE_STUDENT_ACCOUNT> liCreditAndDebit { get; set; }
        public List<FEE_STRUCTURE> liFeeStructure { get; set; }
        public List<FEE_STRUCTURE> liFeeDetails { get; set; }
        public List<SupProgrammeLevel> programmeType { get; set; }
        public List<ADM_UPLOADED_FILES_2018> liUploadedFile { get; set; }
        public List<SUP_FEE_FREQUENCY> liFrequency { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> liIssueApplication { get; set; }
        public List<SELECTIONPROCCESS> liSelectionProccess { get; set; }
        public List<SELECTIONPROCCESS> liMonth { get; set; }
        public List<CollegeDetails> liCollegeDetails { get; set; }
        public List<ADM_CASTEWISE_QUATA> liCasteWise_Quata { get; set; }
        public List<ADM_CASTEWISE_QUATA> liOcCasteWise_Quata { get; set; }
        public List<ADM_CASTEWISE_QUATA> liProgram_Quata { get; set; }
        public List<ADM_CASTEWISE_QUATA> liCasteWiseAdmitted { get; set; }
        public List<ADM_CASTEWISE_QUATA> liMinorityWise { get; set; }
        public List<ADM_CASTEWISE_QUATA> liOccategoryWise { get; set; }
        public List<ADM_CASTEWISE_QUATA> liOccategoryWiseAdmitted { get; set; }
        public List<ADM_CASTEWISE_QUATA> liMinorityWiseAdmitted { get; set; }
        public List<ADM_SELECTION_PROCESS> LiDateExpiredSelectedApplicant = new List<ADM_SELECTION_PROCESS>();
        public List<SUP_SELECTION_CYCLE> liSupSelectionCycle { get; set; }
        public List<SupCommunity> liCaste { get; set; }
        public List<MainCommunity> liMainCommunity { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> liRegistered { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> liSubmitted { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> liSelected { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> liVerified { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> liAdmitted { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> liTransfered { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> liPending { get; set; }
        public List<ADM_PROGRAMME_DESCRIPTION> liProgarammeDescription { get; set; }
        public List<SUP_HOSTEL> liHostel { get; set; }
        public List<ADM_TRANSFER_REQUEST> LiTransferRequestApplicant { get; set; }
        public List<ADM_TRANSFER_APPROVAL> LiTransferapprovalApplicant { get; set; }
        public List<ADM_REFUND_STUDENT_ACCOUNT> LiRefundStudentAccount { get; set; }
        public List<HOSTEL_STUDENT_EXTRACURRICULAR> LiHostelStudentExtracurricular { get; set; }
        public List<HOSTEL_STUDENT_CERTIFICATE> LiHostelStudentCertificate { get; set; }
        public List<HOSTEL_STUDENT_GADGETS> LiHostelStudentGadget { get; set; }
        public List<HOSTEL_STUDENT_SPORTS> LiHostelStudentSport { get; set; }


        public SelectList LiStudent { get; set; }
        public SelectList ProgrammeList { get; set; }
        public SelectList Documents { get; set; }
        public SelectList CycleList { get; set; }
        public SelectList CasteList { get; set; }
        public SelectList ShiftList { get; set; }
        public SelectList HostelList { get; set; }
        public SelectList Religion { get; set; }
        public SelectList StaffList { get; set; }
        public SelectList ApplicationTypeList { get; set; }
        public SelectList ApplicantTypeList { get; set; }
        public SelectList ProgrammeMode { get; set; }
        public SelectList Fields { get; set; }
        public SelectList Application { get; set; }
        public SelectList Subjects { get; set; }
        public SelectList AppSubmissionTypeList { get; set; }
        public string Status { get; set; }
        public string CancelAdmissionId { get; set; }
        public string PAID { get; set; }
        public string AMOUNT { get; set; }
        public string PHOTO_PATH { get; set; }

        public string PROGRAMME_DESCRIPTION_ID { get; set; }
        public List<SUP_HSS_SUBJECTS> lisubjects = new List<SUP_HSS_SUBJECTS>();
        public List<ADM_CANCEL_ADMISSION> liAdmCancelAdmission = new List<ADM_CANCEL_ADMISSION>();
        public List<ADM_BANK_APPLICATION> liBankApplication = new List<ADM_BANK_APPLICATION>();
        public List<FETCH_BANK_APPLICATION> liFetchBankApplication = new List<FETCH_BANK_APPLICATION>();
        public ADM_BANK_APPLICATION ObjBankApplication = new ADM_BANK_APPLICATION();

        public SelectList GENDER { get; set; }
        public SelectList MARITAL_STATUS { get; set; }
        public SelectList SALUTATION { get; set; }
        public SelectList RELATION { get; set; }
        public SelectList MEDIUM { get; set; }
        public SelectList ApplicantList { get; set; }
        public List<SUP_APPLICATION_TYPE> LiApplictionType = new List<SUP_APPLICATION_TYPE>();
        public List<SUP_APPLICANT_TYPE> LiApplicantType = new List<SUP_APPLICANT_TYPE>();
        public List<CP_PROGRAMME_GROUP> LiProgrammeGroup = new List<CP_PROGRAMME_GROUP>();
        public List<StudentModel> LiStudentInfo = new List<StudentModel>();
        public List<SENT_SMS_LIST_2017> Lisms = new List<SENT_SMS_LIST_2017>();
        public List<APPLICATION_FORM> liApplicationForm = new List<APPLICATION_FORM>();

    }

    public class OfflineCollectionViewModel
    {
        public List<ADM_ISSUE_APPLICATION_2018> VerifiedList { get; set; }
        public List<sup_frequency_type> FeeTypeList { get; set; }
    }
    public class APPLICATION_FORM
    {
        public string LDOORDETAIL { get; set; }
        public string PLACE_OF_BIRTH { get; set; }
        public string NATIVE_DISTRICT { get; set; }
        public string COUNTRY_NAME { get; set; }
        public string LSTREET { get; set; }
        public string LCITY { get; set; }
        public string LDISTRICT { get; set; }
        public string LPINCODE { get; set; }
        public string LPHONENO { get; set; }
        public string BSNAME { get; set; }
        public string BSOCCUPATION { get; set; }
        public string BSINCOME { get; set; }
        public string BSNAME1 { get; set; }
        public string BSOCCUPATION1 { get; set; }
        public string BSINCOME1 { get; set; }
        public string BSNAME2 { get; set; }
        public string BSOCCUPATION2 { get; set; }
        public string BSINCOME2 { get; set; }
        public string BSMOBILE { get; set; }
        public string BSMOBILE1 { get; set; }
        public string BSMOBILE2 { get; set; }

        public string RECEIVE_ID { get; set; }
        public string AGE { get; set; }
        public string APPLICATION_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string GENDER_NAME { get; set; }
        public string BLOOD_GROUP_NAME { get; set; }
        public string COMMUNITY { get; set; }
        public string COMMUNITY_ID { get; set; }
        public string IS_ROMAN_CATHOLIC { get; set; }
        public string MOTHER_INCOME { get; set; }
        public string MOTHER_OCCUPATION { get; set; }
        public string FATHER_MOBILE_NUMBER { get; set; }
        public string MOTHER_MOBILE_NUMBER { get; set; }

        public string RELIGION { get; set; }
        public string DALIT { get; set; }
        public string NATIONALITY_NAME { get; set; }
        public string AADHAR_NUMBER { get; set; }
        public string LANGUAGE_NAME { get; set; }
        public string UG_TOTAL_SEMESTER { get; set; }

        public string STATE { get; set; }
        public string CDISTRICT1 { get; set; }
        public string CPOST_PLACE_TOWN1 { get; set; }
        public string CVILLAGE_AREA1 { get; set; }
        public string IS_NRI { get; set; }
        public string PASSPORT_NUMBER { get; set; }
        public string REFUGEE { get; set; }
        public string ORPHAN { get; set; }
        public string SEMI_ORPHAN { get; set; }
        public string FATHER_NAME { get; set; }
        public string OCCUPATION_NAME { get; set; }
        public string OCCUPATION { get; set; }

        public string ANNUAL_INCOME { get; set; }
        public string MOTHER_NAME { get; set; }
        public string IS_FIRST_GENERATION { get; set; }
        public string EXSERVICE_MAN { get; set; }
        public string EXSERVICE_MAN_APPLICABLE { get; set; }
        public string LAST_STUDIED_PLACE { get; set; }
        public string LAST_STUDIED_SCHOOL { get; set; }
        public string EDUCATION_BOARD { get; set; }
        public string HSC_NO { get; set; }
        public string HSPERCENTAGE { get; set; }
        public string TOTAL_CUT_OFF_MARK { get; set; }
        public string HS_MAX_MARK { get; set; }
        public string HSTOTAL { get; set; }
        public string CDOORDETAIL { get; set; }
        public string CVILLAGE_AREA { get; set; }
        public string CDISTRICT { get; set; }
        public string CPINCODE { get; set; }
        public string CPHONENO { get; set; }
        public string CMOBILENO { get; set; }
        public string PDOORDETAIL { get; set; }
        public string PVILLAGE_AREA { get; set; }
        public string PDISTRICT { get; set; }
        public string PPINCODE { get; set; }
        public string PMOBILENO { get; set; }
        public string PPHONENO { get; set; }
        public string EMAIL_ID { get; set; }
        public string HSS_GROUP { get; set; }
        public string EXTRA_CURRICULAR { get; set; }
        public string SPECIALLYABLED { get; set; }
        public string LAST_STUDIED_CLASS { get; set; }
        public string HOSTEL_FACILITY { get; set; }
        public string MARITAL_STATUS { get; set; }
        public string PHOTO_PATH { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string NO_OF_SEMESTERS { get; set; }
        public string IS_ARREAR { get; set; }
        public string NAME_IN_NATIVE { get; set; }
        public string FATHER_NAME_IN_NATIVE { get; set; }
        public string MOTHER_NAME_IN_NATIVE { get; set; }
        public string MEDIUM_OF_STUDY { get; set; }
        public string SPORTS { get; set; }
        public string PARISHI_FRC { get; set; }
        public string SPECIALLYABLED_TYPE { get; set; }

        public string SMS_MOBILE_NO { get; set; }

    }
}
