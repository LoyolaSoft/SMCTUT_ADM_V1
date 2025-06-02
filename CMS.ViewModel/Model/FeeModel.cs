using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CMS.ViewModel.Model
{

    public class FEE_PAYU_REQUEST
    {
        public string request_id { get; set; }
        public string key { get; set; }
        public string txnid { get; set; }
        public string amount { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string lastname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string pg { get; set; }
        public string hash { get; set; }

    }
    public class FEE_MERCHANT_ACCOUNT_INFO
    {
        public string MERCHANT_ACCOUNT_INFO { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string KEY { get; set; }
        public string SALT { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string BASE_URL { get; set; }
        public string AUTHORIZATION { get; set; }
        public string API_TYPE { get; set; }
        public string HASH_SEQUENCE { get; set; }
        public string ACCOUNT_TYPE { get; set; }
        public string STUDENT_ID { get; set; }
        public string REGISTER_NO { get; set; }
        public string ROLL_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string STU_EMAILID { get; set; }
        public string STU_MOBILENO { get; set; }
        public string curl { get; set; }
        public string furl { get; set; }
        public string surl { get; set; }
        public string BANK_ACCOUNT_ID { get; set; }
    }
    public class fee_payu_response
    {
        public List<FEE_STUDENT_ACCOUNT> liStuAccount { get; set; }
        public string transaction_amount { get; set; }
        public string PAYU_RESPONSE_ID { get; set; }
        public string key { get; set; }
        [Display(Name = "Transaction Id")]
        public string txnid { get; set; }
        [Display(Name = "Amount")]
        public string amount { get; set; }
        [Display(Name = "Description")]
        public string productinfo { get; set; }
        [Display(Name = "Name")]

        public string firstname { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Phone")]
        public string phone { get; set; }
        public string lastname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string udf1 { get; set; }
        [Display(Name = "Frequency")]
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string udf6 { get; set; }
        public string udf7 { get; set; }
        public string udf8 { get; set; }
        public string udf9 { get; set; }
        public string udf10 { get; set; }
        public string hash { get; set; }
        public string payment_source { get; set; }
        public string PG_TYPE { get; set; }
        [Display(Name = "Bank ref_no")]
        public string bank_ref_num { get; set; }
        public string bankcode { get; set; }
        public string error { get; set; }
        public string error_Message { get; set; }
        public string name_on_card { get; set; }
        public string cardnum { get; set; }
        public string mode { get; set; }
        public string mihpayid { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Date")]
        public string addedon { get; set; }
        public string unmappedstatus { get; set; }
        public string additionalCharges { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string APPLICATION_NO { get; set; }
        public string request_date { get; set; }
        public string RECEIPT_NO { get; set; }

    }

    public class PayUPost
    {

        public string key { get; set; }
        public string txnid { get; set; }
        [Display(Name = "Amount")]
        public string amount { get; set; }
        public string productinfo { get; set; }
        [Display(Name = "Name")]
        public string firstname { get; set; }
        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string email { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string udf6 { get; set; }
        public string udf7 { get; set; }
        public string udf8 { get; set; }
        public string udf9 { get; set; }
        public string udf10 { get; set; }
        public string salt { get; set; }
        public string hash { get; set; }
        public string BASE_URL { get; set; }
        public string sMessage { get; set; }
        [Required]
        [Display(Name = "Mobile")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        public string phone { get; set; }
        public string surl { get; set; }
        public string furl { get; set; }
        public string curl { get; set; }
        public string city { get; set; }
        public string address2 { get; set; }
        public string address1 { get; set; }
        public string lastname { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pg { get; set; }
        public string zipcode { get; set; }
    }
    public class FeeMainHead
    {
        public string FEE_MAIN_HEAD_ID { get; set; }
        public string MAIN_HEAD_ID { get; set; }
        public string HEAD_ID { get; set; }
        public string FEE_ROOT { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string IS_DELETED { get; set; }
        public string MAIN_HEAD { get; set; }
        public string HEAD { get; set; }
        public string STATUS { get; set; }
        public string SHIFT_NAME { get; set; }
        public string SHIFT { get; set; }
        public string FREQUENCY_TYPE { get; set; }
        public string FREQUENCY_NAME { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string FEE_ROOT_ID { get; set; }
        public string BANK_ACCOUNT_ID { get; set; }
        public string PASSBOOK_NO { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string FEE_CATEGORY_ID { get; set; }
        public string FEE_CATEGORY { get; set; }
        public string EXAM_HEAD_ID { get; set; }
        public string COST { get; set; }
        public string EXAM_SETTING_ID { get; set; }
        public string PROGRAMME_LEVEL_ID { get; set; }
    }
    public class FEE_STRUCTURE_LIST
    {
        public List<FEE_STRUCTURE> FEE_STRUCTURE { get; set; }
        public List<FeeMainHead> JSON_FEE_MAIN_HEAD { get; set; }
    }
    public class JsonFeeStudentAccountList
    {
        public List<FEE_STUDENT_ACCOUNT> StudentAccountList { get; set; }
    }
    public class JsonFeeTransactionList
    {
        public List<FEE_TRANSACTION> FeeTransactionList { get; set; }
    }

    public class FEE_FREQUENCY_FEE_MODE
    {
        public string FEE_FREQUENCY_FEE_MODE_ID { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string FEE_MODE { get; set; }
        public string FREQUENCY_TYPE { get; set; }
        public string FREQUENCY_NAME { get; set; }
    }
    public class RecipientSettlement
    {
        public string id { get; set; }
        public string entity { get; set; }
        public string amount { get; set; }
        public string status { get; set; }
        public string fees { get; set; }
        public string tax { get; set; }
        public string utr { get; set; }
        public string created_at { get; set; }
    }
    public class FEE_RAZORPAY_TRANSFER_INFO
    {
        public string RAZORPAY_TRANSFER_ID { get; set; }
        public string ID { get; set; }
        public string ENTITY { get; set; }
        public string SOURCE { get; set; }
        public string RECIPIENT { get; set; }
        public string AMOUNT { get; set; }
        public string CURRENCY { get; set; }
        public string AMOUNT_REVERSED { get; set; }
        public string FEES { get; set; }
        public string TAX { get; set; }
        public string ON_HOLD { get; set; }
        public string ON_HOLD_UNTIL { get; set; }
        public string RECIPIENT_SETTLEMENT_ID { get; set; }
        public string CREATED_AT { get; set; }
        public string LINKED_ACCOUNT_NOTES { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
        public string UDF6 { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class FEE_RAZORPAY_SETTLEMENT_INFO
    {
        public string SETTLEMENT_ID { get; set; }
        public string ID { get; set; }
        public string ENTITY { get; set; }
        public string AMOUNT { get; set; }
        public string STATUS { get; set; }
        public string FEES { get; set; }
        public string TAX { get; set; }
        public string UTR { get; set; }
        public string CREATED_AT { get; set; }
        public string RECIPIENT { get; set; }
    }
    public class TransferResponse
    {
        public string entity { get; set; }
        public int count { get; set; }
        public List<Item> items { get; set; }
    }
    public class Item
    {
        public string id { get; set; }
        public string entity { get; set; }
        public string source { get; set; }
        public string recipient { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public int amount_reversed { get; set; }
        public Notes notes { get; set; }
        public int fees { get; set; }
        public int? tax { get; set; }
        public bool on_hold { get; set; }
        public object on_hold_until { get; set; }
        public object recipient_settlement_id { get; set; }
        public int created_at { get; set; }
        public string utr { get; set; }
        public List<object> linked_account_notes { get; set; }
        public RecipientSettlement recipient_settlement { get; set; }
        // public string id { get; set; }
        // public string entity { get; set; }
        // public int amount { get; set; }
        // public string currency { get; set; }
        public string status { get; set; }
        public string order_id { get; set; }
        public object invoice_id { get; set; }
        public bool international { get; set; }
        public string method { get; set; }
        public int amount_refunded { get; set; }
        public object refund_status { get; set; }
        public bool captured { get; set; }
        public string description { get; set; }
        public string card_id { get; set; }
        public object bank { get; set; }
        public object wallet { get; set; }
        public string vpa { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        // public Notes notes { get; set; }
        public int fee { get; set; }
        // public int? tax { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }
        public string payment_id { get; set; }
        public object receipt { get; set; }
        public AcquirerData acquirer_data { get; set; }


        //public int created_at { get; set; }
    }
    public class TransferItemResponse
    {

        public string id { get; set; }
        public string entity { get; set; }
        public string source { get; set; }
        public string recipient { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string amount_reversed { get; set; }
        public Notes notes { get; set; }
        public string fees { get; set; }
        public string tax { get; set; }
        public string on_hold { get; set; }
        public string on_hold_until { get; set; }
        public string recipient_settlement_id { get; set; }
        public RecipientSettlement recipient_settlement { get; set; }
        public int created_at { get; set; }
        public List<object> linked_account_notes { get; set; }
        //  public string id { get; set; }
        //  public string entity { get; set; }
        //  public int amount { get; set; }
        //  public string currency { get; set; }
        public string status { get; set; }
        public string order_id { get; set; }
        public object invoice_id { get; set; }
        public bool international { get; set; }
        public string method { get; set; }
        public int amount_refunded { get; set; }
        public object refund_status { get; set; }
        public bool captured { get; set; }
        public string description { get; set; }
        public string card_id { get; set; }
        public object bank { get; set; }
        public object wallet { get; set; }
        public object vpa { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        //  public Notes notes { get; set; }
        public int fee { get; set; }
        // public int? tax { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }
        //  public int created_at { get; set; }
    }
    public class FEE_STUDENT_ACCOUNT
    {
        public string PROGRAMME_MODE { get; set; }
        public string RAZORPAY_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string STUDENT_AC_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string FREQUENCY { get; set; }
        public string FREQUENCY_NAME { get; set; }
        public string FREQUENCY_TYPE { get; set; }
        public string HEAD { get; set; }
        public string HEAD_ID { get; set; }
        public string HEAD_NAME { get; set; }
        public string CREDIT { get; set; }
        public string DEBIT { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string BRS_ID { get; set; }
        public string DISCOUNT_ID { get; set; }
        public string SPECIAL_FEES_ID { get; set; }
        public string IS_DELETED { get; set; }
        public string TRANSACTION_ID { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string IS_UPDATED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
        public string BANK { get; set; }
        public string CLASS_ID { get; set; }
        public string TEMP_ID { get; set; }
        public string IS_REFUND { get; set; }
        public string SPONSOR_ID { get; set; }
        public string TO_BE_UPLOADED { get; set; }
        public string F_STUDENT_AC_ID { get; set; }
        public string WAIVE_ID { get; set; }
        public string FINE_DATE { get; set; }
        public string IS_CANCELLED_HEAD { get; set; }
        public string BALANCE { get; set; }
        public string RECEIPT_NO { get; set; }
        public string PAID_AMOUNT { get; set; }
        public string FIRST_NAME { get; set; }
        public string CLASS_CODE { get; set; }
        public string ROLL_NO { get; set; }
        public string REGISTER_NO { get; set; }
        public string STU_EMAILID { get; set; }
        public string STU_MOBILENO { get; set; }
        public string STATUS { get; set; }
        public string MAIN_HEAD { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string NO_OF_STUDENTS { get; set; }
        public string FEE_ROOT_ID { get; set; }
        public string PAYU_RESPONSE_ID { get; set; }
        public string PAYMENT_DATE { get; set; }
        public string COLLECTED { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string FREQUENCY_TYPE_ID { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string ID { get; set; }
        public string MAIN_HEAD_ID { get; set; }
        public string BANK_ACCOUNT_ID { get; set; }
        public string HOSTEL_ID { get; set; }
        public string HOSTEL_NAME { get; set; }
        public string FEE_MAIN_HEAD_ID { get; set; }
        public string UDF2 { get; set; }
        public string UDF6 { get; set; }
        public string AMOUNT { get; set; }
        public string FEE_STRUCTURE_ID { get; set; }
        public string PROGRAMME { get; set; }
        public string APPLICATION_NO { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string ERROR_MESSAGE { get; set; }
        public string STATUS_NAME { get; set; }
        public List<FEE_STUDENT_ACCOUNT> StudentAccountList { get; set; }
    }
    public class FEE_REPORT : FEE_TRANSACTION
    {
        public string ADMISSION_NO { get; set; }
        public string PAYURESPONSE_ID { get; set; }
        public string CLASS_NAME { get; set; }
        public string NAME { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string ACCOUNT_NAME { get; set; }
        public string BANK_ACCOUNT_ID { get; set; }
        public string ID { get; set; }
        //  public string ORDER_ID { get; set; }
        public string CREATED_AT { get; set; }
        public string RECIPIENT_SETTLEMENT_ID { get; set; }
        public string AMOUNT { get; set; }
        public string SETTLEMENT_DATE { get; set; }

    }
    public class Notes
    {
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string udf6 { get; set; }

    }
    public class OrderIdResponse
    {
        public string id { get; set; }
        public string entity { get; set; }
        public string amount { get; set; }
        public string amount_paid { get; set; }
        public string amount_due { get; set; }
        public string currency { get; set; }
        public string receipt { get; set; }
        public string offer_id { get; set; }
        public string status { get; set; }
        public string attempts { get; set; }
        public Notes notes = new Notes();
        public string created_at { get; set; }
        public string key { get; set; }
        public string secret { get; set; }
    }
    public class FEE_RAZORPAY_ACCOUNTS
    {
        public string RAZORPAY_ACCOUNT_ID { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string ACCOUNT_NAME { get; set; }
        public string ACCOUNT_EMAIL_ID { get; set; }
        public string ACTIVATED_AT { get; set; }
        public string CREATED_AT { get; set; }
        public string BANK_ACCOUNT_ID { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string RAZORPAY_ACCOUNT_TYPE_ID { get; set; }
        public string KEY { get; set; }
        public string SECRET_KEY { get; set; }
    }
    public class PaymentResponse
    {
        public string id { get; set; }
        public string entity { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string order_id { get; set; }
        public string invoice_id { get; set; }
        public string international { get; set; }
        public string method { get; set; }
        public string amount_refunded { get; set; }
        public string refund_status { get; set; }
        public string captured { get; set; }
        public string description { get; set; }
        public string card_id { get; set; }
        public string bank { get; set; }
        public string wallet { get; set; }
        public string vpa { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public Notes notes { get; set; }
        public string fee { get; set; }
        public string tax { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }
        public string created_at { get; set; }
    }
    public class FEE_RAZORPAY_PAYMENT_INFO
    {
        public string RAZORPAY_PAMENT_ID { get; set; }
        public string ID { get; set; }
        public string ENTITY { get; set; }
        public string AMOUNT { get; set; }
        public string CURRENCY { get; set; }
        public string STATUS { get; set; }
        public string ORDER_ID { get; set; }
        public string INVOICE_ID { get; set; }
        public string INTERNATIONAL { get; set; }
        public string METHOD { get; set; }
        public string AMOUNT_REFUNDED { get; set; }
        public string REFUND_STATUS { get; set; }
        public string CAPTURED { get; set; }
        public string DESCRIPTION { get; set; }
        public string CARD_ID { get; set; }
        public string BANK { get; set; }
        public string WALLET { get; set; }
        public string VPA { get; set; }
        public string EMAIL { get; set; }
        public string CONTACT { get; set; }
        public string FEE { get; set; }
        public string TAX { get; set; }
        public string ERROR_CODE { get; set; }
        public string ERROR_DESCRIPTION { get; set; }
        public string CREATED_AT { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
        public string UDF6 { get; set; }
        public string RECEIPT_NO { get; set; }

        public string IS_DELETED { get; set; }
    }
    public class FEE_RAZORPAY_ORDER_INFO
    {
        public string RAZORPAY_ORDER_ID { get; set; }
        public string ID { get; set; }
        public string ENTITY { get; set; }
        public string AMOUNT { get; set; }
        public string AMOUNT_PAID { get; set; }
        public string AMOUNT_DUE { get; set; }
        public string CURRENCY { get; set; }
        public string RECEIPT { get; set; }
        public string OFFER_ID { get; set; }
        public string STATUS { get; set; }
        public string ATTEMPTS { get; set; }
        public string CREATED_AT { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
        public string UDF6 { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
    }
    public class FEE_TRANSACTION
    {
        public string TRANSACTION_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string FREQUENCY { get; set; }
        public string CLASS { get; set; }
        public string CLASS_LEVEL { get; set; }
        public string CLASS_YEAR { get; set; }
        public string PAYMENT_DATE { get; set; }
        public string RECEIPT_NO { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string DD_CHEQUE_NO { get; set; }
        public string COLLECTED { get; set; }
        public string DISCOUNT { get; set; }
        public string DEDUCT_STUDENT_ACCOUNT { get; set; }
        public string RECEIPT_NARRATION { get; set; }
        public string USERNAME { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string ACC_YEAR { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_AMOUNT_COLLECTED { get; set; }
        public string IS_ACCOUNTANT_COLLECTED { get; set; }
        public string IS_ADVANCE { get; set; }
        public string UPLOAD_FLAG { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string IS_UPDATED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
        public string FEE_TRANSACTION_COUNTER { get; set; }
        public string FEE_TRANSACTION_BANK { get; set; }
        public string TEMP_ID { get; set; }
        public string EXCESS_AMT { get; set; }
        public string F_TRANSACTION_ID { get; set; }
        public string FREQUENCY_TO { get; set; }
        public string CHALLAN_NO { get; set; }
        public string BALANCE { get; set; }
        public string CREDIT { get; set; }
        public string DEBIT { get; set; }
        public string REGISTER_NO { get; set; }
        public string ROLL_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string TOTAL_AMOUNT { get; set; }
        public string DATE_FROM { get; set; }
        public string DATE_TO { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string MONTH { get; set; }
        public string FREQUENCY_NAME { get; set; }
        public string PayUResponse_Id { get; set; }
        public string FREQUENCY_TYPE { get; set; }
        public string DAY_ORDER_DATE { get; set; }
        public string DAY_ORDER_MONTH { get; set; }
        public string SHIFT { get; set; }
        public string HEAD { get; set; }
        public string MAIN_HEAD { get; set; }
        public string MAIN_HEAD_ID { get; set; }
        public string HEAD_ID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string PAID_AMOUNT { get; set; }
        public string RAZORPAY_ID { get; set; }
        public string HOSTEL_ID { get; set; }
        public string HOSTEL_NAME { get; set; }
        public string STATUS { get; set; }
        public string ORDER_ID { get; set; }
        public string RAZOR_PAY_ID { get; set; }
        public string FEE_ROOT_ID { get; set; }
        public string FEE_CATEGORY_ID { get; set; }
        public string PASSBOOK_NO { get; set; }
        public string FEE_MODE { get; set; }
        public string APPLICATION_NO { get; set; }
        public string FEE_MAIN_HEAD_ID { get; set; }
        public string FREQUENCY_TYPE_ID { get; set; }
        public string RECEIVE_ID { get; set; }
        public string ID { get; set; }
        public string ACCOUNT_NAME { get; set; }
        public string AMOUNT { get; set; }
        public string ISSUED_ID { get; set; }
        public string APPTYPE_ID { get; set; }
        public string COUNT { get; set; }
        public string IS_REFUND { get; set; }
        public string REFUND_TYPE { get; set; }
        public string REFUND_DATE { get; set; }
        public string UDF6 { get; set; }

        public string FREQUENCY_ID { get; set; }



    }
    public class FEE_DISCOUNT
    {
        public string DISCOUNT_ID { get; set; }
        public string DISCOUNT_NAME { get; set; }
        public string IS_DELETED { get; set; }
        public string DISCOUNT_TYPE { get; set; }
        public string FREQUENCY_TYPE { get; set; }
        public string DISCOUNT_VALUE { get; set; }
        public string IS_USED { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string IS_UPDATED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
    }
    public class JSON_FEE_DISCOUNT_ALLOTMENT
    {
        public List<FEE_DISCOUNT_ALLOTMENT> DiscountAllotmentList { get; set; }
    }
    public class FEE_DISCOUNT_ALLOTMENT
    {
        public string DISCOUNT_ALLOTMENT_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string ACCADEMIC_YEAR { get; set; }
        public string CLASS_ID { get; set; }
        public string DISCOUNT_ID { get; set; }
        public string DISCOUNT_NAME { get; set; }
        public string DISCOUNT_VALUE { get; set; }
        public string FIRST_NAME { get; set; }
        public string ROLL_NO { get; set; }
        public string IS_DELETED { get; set; }
        public string UPLOAD_FLAG { get; set; }
        public string IS_UPDATED { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
        public string FREQUENCY { get; set; }
        public string FEE_CATEGORY { get; set; }
        public string HEAD { get; set; }
        public string PROGRAMME { get; set; }
    }
    public class FEE_STRUCTURE
    {
        public string FEE_STRUCTURE_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string FREQUENCY { get; set; }
        public string FREQUENCY_NAME { get; set; }
        public string CLASS { get; set; }
        public string CLASS_NAME { get; set; }
        public string HEAD { get; set; }
        public string HEAD_NAME { get; set; }
        public string HEAD_ID { get; set; }
        public string AMOUNT { get; set; }
        public string IS_DELETED { get; set; }
        public string UPLOAD_FLAG { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string IS_UPDATED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
        public string FEE_MODE { get; set; }
        public string FEE_CATEGORY { get; set; }
        public string FEE_CATEGORY_ID { get; set; }
        public string PROGRAMME { get; set; }
        public string TEMP_ID { get; set; }
        public string ISSUE_ID { get; set; }
        public string RECEIVE_ID { get; set; }
        public string PROGRAMME_MODE { get; set; }
        public string SHIFT { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string CREDIT { get; set; }
        public string STUDENT_ID { get; set; }
        public string SELECTED { get; set; }
        public string HEAD_CODE { get; set; }
        public string MAIN_HEAD_ID { get; set; }
        public string MAIN_HEAD { get; set; }
        public string FEE_MAIN_HEAD_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string IS_USED { get; set; }
        public string FIRST_NAME { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string CLASS_YEAR_ID { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string FEE_CATEGORY_NAME { get; set; }
        public string BANK_ACCOUNT_ID { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string PROGRAMME_MODE_ID { get; set; }
        public string FEE_ROOT_ID { get; set; }
        public string SHIFT_NAME { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string PASSBOOK_NO { get; set; }
    }
    public class FEE_DISCOUNT_FREQUENCY
    {
        public string DISCOUNT_FRE_ID { get; set; }
        public string DISCOUNT_ID { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string IS_UPDATED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
    }
    public class FEE_DISCOUNT_HEAD
    {
        public string DISCOUNT_HEAD_ID { get; set; }
        public string DISCOUNT_ID { get; set; }
        public string DISCOUNT_NAME { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string FREQUENCY_NAME { get; set; }
        public string HEAD_ID { get; set; }
        public string HEAD { get; set; }
        public string IS_DOWNLOADED { get; set; }
        public string IS_UPDATED { get; set; }
        public string DOWNLOAD_TIME { get; set; }
    }
    public class FEE_ADVANCE_PAYMENT
    {
        public string ADVANCE_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string STUDENT_NAME { get; set; }
        public string GENDER { get; set; }
        public string FATHER_NAME { get; set; }
        public string MOTHER_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string APPLIED_GRADE { get; set; }
        public string AMOUNT { get; set; }
        public string IS_DELETED { get; set; }
        public string IS_USED { get; set; }
        public string IS_ADMITTED { get; set; }
        public string PAYMENT_DATE { get; set; }
        public string BRS_ID { get; set; }
        public string IS_AMOUNT_COLLECTED { get; set; }
        public string IS_ACCOUNTANT_COLLECTED { get; set; }
        public string PAYMENT_MODE { get; set; }
    }

    // Fetch Program Transfer
    public class PayUStatus
    {
        public string PAYU_RESPONSE_ID { get; set; }
        public string TXNID { get; set; }
        public string FIRSTNAME { get; set; }
        public string STATUS { get; set; }
        public string ERROR_MESSAGE { get; set; }
        public string UNMAPPEDSTATUS { get; set; }
        public string APPLICATION_TYPE_ID { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string ISSUE_ID { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
    }
    public class FEE_COLLECTION_DELETED
    {
        public string DELETED_ID { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string DATE { get; set; }
        public string RECEIPT_NO { get; set; }
        public string ADMISSION_NO { get; set; }
        public string TRANSACTION_ID { get; set; }
        public string NAME { get; set; }
        public string CLASS { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string AMOUNT { get; set; }
        public string REMARK { get; set; }
        public string TEMP_ID { get; set; }
    }

    public class FEE_RECEIPT_GENERATION
    {
        public string ISSUE_ID { get; set; }
        public string RECEIVE_ID { get; set; }
        public string PROGRAMME_ID { get; set; }
        public string PROGRAMME_NAME { get; set; }
        public string ISSUE_DATE { get; set; }
        public string APPLICATION_NO { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string CONTACT_NO { get; set; }
        public string SHIFT { get; set; }
        public string DOB { get; set; }
        public string HSC_NO { get; set; }
        public string CLASS_NAME { get; set; }
        public string PROGRAMME_GROUP_ID { get; set; }
        public string TRANSACTION_ID { get; set; }
        public string STUDENT_ID { get; set; }
        public string PAYMENT_DATE { get; set; }
        public string RECEIPT_NO { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string DD_CHEQUE_NO { get; set; }
        public string COLLECTED { get; set; }
        public string DISCOUNT { get; set; }
        public string DEDUCT_STUDENT_ACCOUNT { get; set; }
        public string PayUResponse_Id { get; set; }
        public string FREQUENCY { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string FREQUENCY_NAME { get; set; }
        public string FREQUENCY_ID { get; set; }
        public string SMS_MOBILE_NO { get; set; }
        public string SETTLEMENT_DATE { get; set; }
    }

    public class AcquirerData
    {
        public string rrn { get; set; }

    }
    public class RefundResponse
    {
        public string id { get; set; }
        public string entity { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string payment_id { get; set; }
        //public List<object> notes { get; set; }
        public Notes notes { get; set; }
        public object receipt { get; set; }
        public AcquirerData acquirer_data { get; set; }
        public int created_at { get; set; }
        public string status { get; set; }
        public string speed_processed { get; set; }
        public string speed_requested { get; set; }
    }

    public class ReversalResponse
    {
        public string id { get; set; }
        public string entity { get; set; }
        public string transfer_id { get; set; }
        public int amount { get; set; }
        public int fee { get; set; }
        public int tax { get; set; }
        public string currency { get; set; }
        public List<Notes> notes { get; set; }
        public string initiator_id { get; set; }
        public object customer_refund_id { get; set; }
        public int created_at { get; set; }

    }
    public class FEE_RAZORPAY_REFUND_INFO
    {
        public string RAZORPAY_REFUND_ID { get; set; }
        public string ID { get; set; }
        public string ENTITY { get; set; }
        public string AMOUNT { get; set; }
        public string CURRENCY { get; set; }
        public string PAYMENT_ID { get; set; }
        public string RECEIPT { get; set; }
        public string RRN { get; set; }
        public string CREATED_AT { get; set; }
        public string STATUS { get; set; }
        public string SPEED_PROCESSED { get; set; }
        public string SPEED_REQUESTED { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
        public string UDF6 { get; set; }
        public string IS_DELETED { get; set; }
    }


    public class FEE_RAZORPAY_REVERSAL_INFO
    {
        public string RAZORPAY_REVERSAL_ID { get; set; }
        public string ID { get; set; }
        public string ENTITY { get; set; }
        public string TRANSFER_ID { get; set; }
        public string AMOUNT { get; set; }
        public string FEES { get; set; }
        public string TAX { get; set; }
        public string CURRENCY { get; set; }
        public string INITIATOR_ID { get; set; }
        public string CUSTOMER_REFUND_ID { get; set; }
        public string CREATED_AT { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
        public string UDF6 { get; set; }
        public string IS_DELETED { get; set; }
    }
}
