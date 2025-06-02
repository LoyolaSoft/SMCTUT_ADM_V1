using CMS.ViewModel.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CMS.ViewModel.ViewModel
{

    public class FeeMainHeadViewModel
    {
        public List<FeeMainHead> FeeMainHeadList { get; set; }
        public List<SUP_FEE_MAIN_HEAD> MainHeads { get; set; }
        public List<SUP_HEAD> SubHeads { get; set; }
        public List<SUP_FEE_FREQUENCY> SubFeeFrequency { get; set; }
        public List<Sup_Shift> SupShift { get; set; }
        public List<SUP_FEE_ROOT> LiSupFeeRoot { get; set; }
        public List<SUP_APPLICATION_TYPE> LiApplicationType { get; set; }
        public List<SUP_BANK_ACCOUNT> LiSupBankAccount { get; set; }
        public List<SupProgrammeMode> SupProgrammeMode { get; set; }
        public List<SUP_FEE_FREQUENCY> SupFrequency { get; set; }
        public List<Sup_Fee_Category> FeeCategory { get; set; }
    }

    public class AdmissionFeeViewModel
    {

        public List<Sup_Shift> shiftList { get; set; }
        public List<ADM_APPTYPE_PROG_GROUPS> programmeList { get; set; }
        public SelectList programmeMode { get; set; }
        public SelectList ClassList { get; set; }
        public SelectList AcademicyearList { get; set; }
        public SelectList FrequencyList { get; set; }


    }
    public class FeeViewModel
    {
        public SelectList liSupFeeFrequency;

        public SUP_HEAD ObjSup_Head { get; set; }
        public List<SUP_HEAD> liSup_Head { get; set; }
        public Sup_Fee_Category ObjFee_Category { get; set; }
        public List<Sup_Fee_Category> liFee_Category { get; set; }

        public List<FEE_COLLECTION_DELETED> liFeeRefunded = new List<FEE_COLLECTION_DELETED>();
    }
    public class FeeTransactionViewModel
    {
        public SelectList liSupFeeFrequency { get; set; }
        public SelectList liStudentPersonalInfo { get; set; }
        public SelectList liClassList { get; set; }
        public SelectList liClassLevel { get; set; }
        public SelectList liClassYear { get; set; }
        public SelectList liHead { get; set; }
        public SelectList liMainHead { get; set; }
        public SelectList liShift { get; set; }
        public SelectList liProgrammeModeList { get; set; }
        public SelectList liAcademicYear { get; set; }
        public SelectList liProgrammeGroup { get; set; }
        public SelectList liFrequencyType { get; set; }
        public SelectList liApplicationType { get; set; }
        public SelectList FeeRoot { get; set; }
        public SelectList FeeCategory { get; set; }
        public SelectList liBankAccount { get; set; }
        public FEE_TRANSACTION FeeTransaction = new FEE_TRANSACTION();
        public List<SUP_FEE_FREQUENCY> lstSup_Fee_Frequency { get; set; }
        public List<FEE_STUDENT_ACCOUNT> liFeeStudentAccount { get; set; }
        public List<FEE_STUDENT_ACCOUNT> liCreditAndDebit { get; set; }
        public List<FEE_STUDENT_ACCOUNT> liStatusVerification { get; set; }
        public List<FEE_STRUCTURE> liFeeStructure { get; set; }
        public List<FEE_TRANSACTION> liFeeTransaction { get; set; }
        public List<FEE_TRANSACTION> liMonth { get; set; }
        public List<CollegeDetails> liCollegeDetails { get; set; }
        public List<DayOrderCalender> liDate { get; set; }
        public List<Student_Personal_Info> lstStudentPersonalInfo { get; set; }
        public List<ADM_RECEIVE_APPLICATION> LiReceivedStudentInfo { get; set; }
        public List<HOSTEL_STUDENT_INFO> lstHostelStudentInfo { get; set; }
        public List<FEE_RECEIPT_GENERATION> liFeeReceiptGeneration { get; set; }
        public List<FEE_RECEIPT_GENERATION> liFeeReceiptInfo { get; set; }
        public List<SUP_HEAD> lstSupHead { get; set; }
        public List<FEE_RAZORPAY_ACCOUNTS> liFeeAccount { get; set; }
        public List<SUP_FEE_MAIN_HEAD> lstSupMainHead { get; set; }
        public List<FEE_REPORT> liFeeReport { get; set; }
        public List<SUP_FEE_ROOT> LiSupFeeRoot { get; set; }
        public OrderIdResponse objOrderId { get; set; }
        public string sMessage { get; set; }

    }
    public class JsonFeeStructureHeads
    {
        public List<FEE_STRUCTURE> FeeStructureHeads { get; set; }

    }
    public class FeeStructureViewModel
    {
        public SelectList ClassList { get; set; }
        public SelectList FrequencyList { get; set; }
        public SelectList AcademicyearList { get; set; }
        public List<Sup_Fee_Category> lstFeeCategory { get; set; }
        public FEE_STRUCTURE FeeStructure = new FEE_STRUCTURE();
        public SelectList ClassYearList { get; set; }
        public SelectList MainHeadList { get; set; }
        public SelectList ShiftList { get; set; }
        public SelectList ProgrammeModeList { get; set; }
        public List<FEE_STRUCTURE> LiFeeStructure { get; set; }
        public List<Sup_Fee_Category> FeeCategory { get; set; }
        public List<SUP_BANK_ACCOUNT> Bank { get; set; }
        public SelectList ProgrammeGroupList { get; set; }
        public List<FEE_STRUCTURE> lstFeeStructure { get; set; }
    }
    public class FeeDiscountViewModel
    {
        public List<FEE_DISCOUNT> lstFeeDiscount { get; set; }
        public FEE_DISCOUNT FeeDiscount = new FEE_DISCOUNT();
    }
    public class FeeDiscountAllotmentviewModel
    {
        public SelectList AcademicyearList { get; set; }
        public SelectList ClassList { get; set; }
        public List<FEE_DISCOUNT_ALLOTMENT> lstFeeDiscountAllotment { get; set; }
        public FEE_DISCOUNT_ALLOTMENT FeeDiscountAllotment = new FEE_DISCOUNT_ALLOTMENT();
    }
    public class FeeDiscountHeadViewModel
    {
        public List<FEE_DISCOUNT_HEAD> lstFeeDiscountHead { get; set; }
        public FEE_DISCOUNT_HEAD FeeDiscountHead = new FEE_DISCOUNT_HEAD();
    }
    public class FeeAdvancePaymentViewModel
    {
        public List<FEE_ADVANCE_PAYMENT> lstFeeAdvancePayment { get; set; }
        public FEE_ADVANCE_PAYMENT FeeAdvancePayment = new FEE_ADVANCE_PAYMENT();
        public SelectList StudentList { get; set; }
        public SelectList ClassList { get; set; }
    }

    // PayUStaus View Model .
    public class FeePayUStatusViewModel
    {
        public List<PayUStatus> liPayUStatus = new List<PayUStatus>();
        public List<fee_payu_response> liPayUResponse = new List<fee_payu_response>();
        [Display(Name = "Application Type")]
        public SelectList Application_Type { get; set; }
        [Display(Name = "Program")]
        public SelectList Program { get; set; }
        [Display(Name = "Shift")]
        public SelectList Shift { get; set; }
        public string Application_No { get; set; }

    }
}
