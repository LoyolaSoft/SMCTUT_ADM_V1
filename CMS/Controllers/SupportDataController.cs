using CMS.DAO;
using CMS.Models;
using CMS.SQL.SupportData;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace CMS.Controllers
{
    public class SupportDataController : Controller
    {
        JsonResultData ObjJsonData = new JsonResultData();
        SupportViewModel objViewModel = new SupportViewModel();
        ResultArgs resultArgs = new ResultArgs();
        public SupBankAccount ObjSupBankViewModel { get; private set; }
        #region Sup_Head List
        /// <summary>
        /// This Action is to show Sup head list                        
        /// </summary>
        /// <returns>FeeViewModel</returns>
        /// 
       // [UserRights("EXAMINCHARGE")]
        public ActionResult SupHeadList()
        {
            FeeViewModel ObjFeeViewModel = new FeeViewModel();
            ObjFeeViewModel.liSup_Head = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupHeadList)).DataSource.Data;
            return View(ObjFeeViewModel);
        }
        /// <summary>
        /// This 
        /// </summary>
        /// <returns></returns>
        public JsonResult FetchddlCategory()
        {
            JsonResultData objResult = new JsonResultData();
            string sOption = string.Empty;
            var liFee = new List<Sup_Fee_Category>();
            liFee = (List<Sup_Fee_Category>)CMSHandler.FetchData<Sup_Fee_Category>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchddlCategory)).DataSource.Data;
            if (liFee != null)
            {
                foreach (var item in liFee)
                {
                    sOption += "<option value='" + item.FEE_CATEGORY_ID + "'>" + item.FEE_CATEGORY + "</option>";
                }
                objResult.ErrorCode = Common.ErrorCode.Ok;
                objResult.Message = Common.ErrorMessage.Ok;
                objResult.sResult = sOption;
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.NoContent;
                objResult.Message = Common.ErrorMessage.NoContent;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// this action to fetch suphed info by headid 
        /// </summary>
        /// <param name="HeadId"></param>
        /// <returns></returns>
        public JsonResult EditSupHead(string HeadId)
        {
            var ObjHead = new SUP_HEAD();
            var liHead = new List<SUP_HEAD>();
            ObjHead.HEAD_ID = HeadId;
            liHead = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(ObjHead, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupHeadForEdit)).DataSource.Data;
            return Json(liHead, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// this action to Save Records ...
        /// </summary>
        /// <param name="HeadId"></param>
        /// <returns></returns>
        public JsonResult SaveSupHead(string HeadId, string Head, string category)
        {
            JsonResultData objResult = new JsonResultData();
            if (!string.IsNullOrEmpty(Head) && !string.IsNullOrEmpty(category))
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    var ObjHead = new SUP_HEAD() { HEAD = Head, HEAD_ID = HeadId, FEE_CATEGORY = category };
                    var liHead = new List<SUP_HEAD>();
                    liHead = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(ObjHead, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.IsSupHeadExist)).DataSource.Data;
                    if (liHead == null)
                    {
                        var sresultArgs = CMSHandler.SaveOrUpdate(ObjHead, SupportDataSQL.GetSupportDataSQL((!string.IsNullOrEmpty(HeadId)) ? SupportDataSQLCommands.UpdateSupHead : SupportDataSQLCommands.SaveSupHead));
                        objResult.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.ExpectationFailed;
                        objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                    }
                    else
                    {
                        objResult.Message = "Already Exits ...!";
                    }

                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
            }

            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSupHead(string HeadId)
        {
            JsonResultData ObjResult = new JsonResultData();
            if (!string.IsNullOrEmpty(HeadId))
            {
                var ObjHead = new SUP_HEAD();
                ObjHead.HEAD_ID = HeadId;
                var sresultArgs = CMSHandler.SaveOrUpdate(ObjHead, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.DeleteSupHead));
                ObjResult.sResult = (sresultArgs.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                ObjResult.ErrorCode = Common.ErrorCode.Ok;
                ObjResult.Message = Common.ErrorMessage.Ok;
            }
            else
            {
                ObjResult.ErrorCode = Common.ErrorCode.BadRequest;
                ObjResult.Message = Common.ErrorMessage.ExpectationFailed;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Sup_Main_head

        #endregion
        public ActionResult Index()
        {
            return View();
        }
        #region Fee Sub Fee Main Head
        [UserRights("ADMIN")]
        public async Task<ActionResult> SupFeeMainHeadList()
        {
            SupFeeMainHead objViewModel = new SupFeeMainHead();
            var ListSupFeeMainHead = await Task.Run(() => (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeMainHead)).DataSource.Data);
            objViewModel.lstSupFeeMainHead = ListSupFeeMainHead;
            return View(objViewModel);
        }
        public JsonResult FetchSupFeeMainHeadById(string sMainHeadId)
        {
            string sOption = string.Empty;
            SUP_FEE_MAIN_HEAD objModel = new SUP_FEE_MAIN_HEAD();
            objModel.MAIN_HEAD_ID = sMainHeadId;
            var FetchExistingFeeHead = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeMainHeadById)).DataSource.Data;
            return Json(FetchExistingFeeHead);
        }
        public JsonResult SaveSupFeeMainHead(string sMainHead)
        {
            JsonResultData objResult = new JsonResultData();
            string sOption = string.Empty;
            if (!string.IsNullOrEmpty(sMainHead))
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    SUP_FEE_MAIN_HEAD objModel = new SUP_FEE_MAIN_HEAD();
                    objModel.MAIN_HEAD = sMainHead;
                    var FetchExistingFeeHead = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.IsSupFeeMainHeadExisting)).DataSource.Data;
                    if (FetchExistingFeeHead != null && FetchExistingFeeHead.Count > 0)
                    {
                        objResult.ErrorCode = Common.ErrorCode.Ok;
                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        //objResult.ErrorCode = "This Sup Fee Main Head is Existing...!!!";
                    }
                    else
                    {
                        var ResultArg = CMSHandler.SaveOrUpdate(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.SaveSupFeeMainHead));
                        if (ResultArg.Success)
                        {
                            objResult.ErrorCode = Common.ErrorCode.Ok;
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                        {
                            objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateSupFeeMainHead(string sFeeMainHead)
        {
            JsonResultData objResult = new JsonResultData();
            string sOption = string.Empty;
            if (!string.IsNullOrEmpty(sFeeMainHead))
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    var Result = JsonConvert.DeserializeObject<SUP_FEE_MAIN_HEAD>(sFeeMainHead);
                    var FetchExistingFeeHead = (List<SUP_FEE_MAIN_HEAD>)CMSHandler.FetchData<SUP_FEE_MAIN_HEAD>(Result, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.IsSupFeeMainHeadExisting)).DataSource.Data;
                    if (FetchExistingFeeHead != null && FetchExistingFeeHead.Count > 0)
                    {
                        objResult.ErrorCode = Common.ErrorCode.Ok;
                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        var ResultArg = CMSHandler.SaveOrUpdate(Result, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdateSupFeeMainHead));
                        if (ResultArg.Success)
                        {
                            objResult.ErrorCode = Common.ErrorCode.Ok;
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                        {
                            objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSupFeeMainHead(string sFeeMainHead)
        {
            JsonResultData objResult = new JsonResultData();
            if (sFeeMainHead != null)
            {
                SUP_FEE_MAIN_HEAD objModel = new SUP_FEE_MAIN_HEAD();
                objModel.MAIN_HEAD_ID = sFeeMainHead;
                ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.DeleteSupFeeMainHead));
                if (ResultArg.Success)
                {
                    objResult.ErrorCode = Common.ErrorCode.Ok;
                    objResult.Message = Common.Messages.RecordDeletedSuccessfully;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                    objResult.Message = Common.Messages.FailedToDeletedTryAgain;
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Sup Bank
        [UserRights("EXAMINCHARGE")]
        public ActionResult SupBankList()
        {
            SupBank ObjSupBankViewModel = new SupBank();
            ObjSupBankViewModel.lstSupBank = (List<SUP_BANK>)CMSHandler.FetchData<SUP_BANK>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBank)).DataSource.Data;
            return View(ObjSupBankViewModel);
        }
        public JsonResult DeleteSupBank(string sBankId)
        {
            JsonResultData objResult = new JsonResultData();
            if (sBankId != null)
            {
                SUP_BANK objModel = new SUP_BANK();
                objModel.BANK_ID = sBankId;
                ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.DeleteSupBank));
                if (ResultArg.Success)
                {
                    objResult.ErrorCode = Common.ErrorCode.Ok;
                    objResult.Message = Common.Messages.RecordDeletedSuccessfully;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                    objResult.Message = Common.Messages.FailedToDeletedTryAgain;
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupBank(string sBankId)
        {
            SupBank objViewModel = new SupBank();
            SUP_BANK objModel = new SUP_BANK();
            objModel.BANK_ID = sBankId;
            if (sBankId != null)
            {
                var SupBankList = (List<SUP_BANK>)CMSHandler.FetchData<SUP_BANK>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankById)).DataSource.Data;
                objViewModel.Bank.BANK_NAME = SupBankList.FirstOrDefault().BANK_NAME;
                objViewModel.Bank.ADDRESS = SupBankList.FirstOrDefault().ADDRESS;
                objViewModel.Bank.PHONE = SupBankList.FirstOrDefault().PHONE;
                objViewModel.Bank.BANK_ID = sBankId;
                return View(objViewModel);
            }
            return View();
        }
        public JsonResult SaveOrUpdateSupBank(string sBank)
        {
            JsonResultData objResult = new JsonResultData();
            SUP_BANK objModel = new SUP_BANK();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (!string.IsNullOrEmpty(sBank))
                {
                    var BankDetails = JsonConvert.DeserializeObject<SUP_BANK>(sBank);
                    if (BankDetails != null)
                    {
                        var result = CMSHandler.SaveOrUpdate(BankDetails, SupportDataSQL.GetSupportDataSQL((!string.IsNullOrEmpty(BankDetails.BANK_ID)) ? SupportDataSQLCommands.UpdateSupBank : SupportDataSQLCommands.SaveSupBank));
                        objResult.ErrorCode = Common.ErrorCode.Ok;
                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                    }
                }
            }
            else
            {
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Sup Bank Account

        public ActionResult SupBankAccount(string sAccountBankId)
        {
            var BankId = string.Empty;
            SupBankAccount ObjSupBankViewModel = new SupBankAccount();
            SUP_BANK_ACCOUNT objModel = new SUP_BANK_ACCOUNT();
            objModel.BANK_ACCOUNT_ID = sAccountBankId;
            var BankList = (List<SUP_BANK>)CMSHandler.FetchData<SUP_BANK>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchBankList)).DataSource.Data;
            if (!string.IsNullOrEmpty(objModel.BANK_ACCOUNT_ID))
            {
                var SupBankAccountList = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAcountById)).DataSource.Data;
                if (SupBankAccountList != null && SupBankAccountList.Count > 0)
                {
                    BankId = SupBankAccountList.FirstOrDefault().BANK;
                    ObjSupBankViewModel.BankAccount.BANK = BankId;
                    ObjSupBankViewModel.BankAccount.ACCOUNT_PURPOSE = SupBankAccountList.FirstOrDefault().ACCOUNT_PURPOSE;
                    ObjSupBankViewModel.BankAccount.PASSBOOK_NO = SupBankAccountList.FirstOrDefault().PASSBOOK_NO;
                    ObjSupBankViewModel.BankAccount.STARTED_DATE = SupBankAccountList.FirstOrDefault().STARTED_DATE;
                    ObjSupBankViewModel.BankAccount.CLOSED_DATE = SupBankAccountList.FirstOrDefault().CLOSED_DATE;
                    ObjSupBankViewModel.BankAccount.BANK_ACCOUNT_ID = sAccountBankId;
                    ObjSupBankViewModel.BankList = new SelectList(BankList, Common.SUP_BANK.BANK_ID, Common.SUP_BANK.BANK_NAME, BankId);
                }
            }
            else
            {
                ObjSupBankViewModel.BankList = new SelectList(BankList, Common.SUP_BANK.BANK_ID, Common.SUP_BANK.BANK_NAME);
            }
            return View(ObjSupBankViewModel);
        }
        [UserRights("EXAMINCHARGE")]
        public ActionResult ListSupBankAccount()
        {
            SupBankAccount ObjSupBankViewModel = new SupBankAccount();
            ObjSupBankViewModel.lstSupBankAccount = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAccount)).DataSource.Data;
            return View(ObjSupBankViewModel);
        }
        public JsonResult DeleteSupBankAccount(string sBankAccountId)
        {
            JsonResultData objResult = new JsonResultData();
            if (sBankAccountId != null)
            {
                SUP_BANK_ACCOUNT objModel = new SUP_BANK_ACCOUNT();
                objModel.BANK_ACCOUNT_ID = sBankAccountId;
                ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.DeleteSupBankAccount));
                if (ResultArg.Success)
                {
                    objResult.ErrorCode = Common.ErrorCode.Ok;
                    objResult.Message = Common.Messages.RecordDeletedSuccessfully;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                    objResult.Message = Common.Messages.FailedToDeletedTryAgain;
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveOrUpdateSupBankAccount(string sBankAccount)
        {
            JsonResultData objResult = new JsonResultData();
            SUP_BANK_ACCOUNT objModel = new SUP_BANK_ACCOUNT();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (!string.IsNullOrEmpty(sBankAccount))
                {
                    var AccountDetails = JsonConvert.DeserializeObject<SUP_BANK_ACCOUNT>(sBankAccount);
                    if (AccountDetails != null)
                    {
                        if (string.IsNullOrEmpty(AccountDetails.BANK_ACCOUNT_ID))
                        {
                            AccountDetails.STARTED_DATE = CommonMethods.ConvertdatetoyearFromat(AccountDetails.STARTED_DATE);
                            AccountDetails.CLOSED_DATE = CommonMethods.ConvertdatetoyearFromat(AccountDetails.CLOSED_DATE);
                            var FetchExistingAccount = (List<SUP_BANK_ACCOUNT>)CMSHandler.FetchData<SUP_BANK_ACCOUNT>(AccountDetails, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupBankAcountByAccountId)).DataSource.Data;
                            if (FetchExistingAccount != null && FetchExistingAccount.Count > 0)
                            {
                                objResult.ErrorCode = Common.ErrorCode.Ok;
                                objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                var result = CMSHandler.SaveOrUpdate(AccountDetails, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.SaveSupBankAccount));
                                if (result.Success)
                                {
                                    objResult.ErrorCode = Common.ErrorCode.Ok;
                                    objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    objResult.ErrorCode = Common.ErrorCode.BadRequest;
                                    objResult.Message = Common.Messages.FailedToSaveRecords;
                                }
                            }
                        }
                        else
                        {
                            AccountDetails.STARTED_DATE = CommonMethods.ConvertdatetoyearFromat(AccountDetails.STARTED_DATE);
                            AccountDetails.CLOSED_DATE = CommonMethods.ConvertdatetoyearFromat(AccountDetails.CLOSED_DATE);
                            var result = CMSHandler.SaveOrUpdate(AccountDetails, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdateSupBankAccount));
                            if (result.Success)
                            {
                                objResult.ErrorCode = Common.ErrorCode.Ok;
                                objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                                objResult.Message = Common.Messages.FailedToSaveRecords;
                            }
                        }
                    }
                }
            }
            else
            {
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Frequency Crude
        [UserRights("Admin")]
        public ActionResult SubFrequencyList()
        {
            SubFrequencyViewModel objViewModel = new SubFrequencyViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                objViewModel.lstSupFeeFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSubFeeFrequency)).DataSource.Data;
            }
            else
                return RedirectToAction("ErrorMessage", "Error");
            return View(objViewModel);
        }
        public JsonResult FetchFrequencyType()
        {
            JsonResultData objResult = new JsonResultData();
            string sOption = string.Empty;
            string sSemesterType = string.Empty;
            string sFrequency = string.Empty;
            string JsonData = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                var liFrequencyType = new List<sup_frequency_type>();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                liFrequencyType = (List<sup_frequency_type>)CMSHandler.FetchData<sup_frequency_type>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFrequencytype)).DataSource.Data;
                var liSemesterType = (List<SUP_SEMESTER_TYPE>)CMSHandler.FetchData<SUP_SEMESTER_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemesterType)).DataSource.Data;
                var liFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFrequency), sAcademicYear).DataSource.Data;
                if (liFrequencyType != null)
                {
                    foreach (var item in liFrequencyType)
                        sOption += "<option value='" + item.FREQUENCY_TYPE_ID + "'>" + item.FREQUENCY_TYPE + "</option>";
                }
                if (liSemesterType != null && liSemesterType.Count > 0)
                {
                    foreach (var item in liSemesterType)
                        sSemesterType += "<option value='" + item.SEMESTER_TYPE_ID + "'>" + item.SEMESTER_TYPE + "</option>";
                }
                if (liFrequency != null && liFrequency.Count > 0)
                {
                    foreach (var item in liFrequency)
                        sFrequency += "<option value='" + item.FREQUENCY_ID + "'>" + item.FREQUENCY_NAME + "</option>";
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            var objJsonData = new SUP_FEE_FREQUENCY() { TYPE = sOption, SEMESTER_TYPE = sSemesterType, FREQUENCY_ID = sFrequency };
            JsonData = JsonConvert.SerializeObject(objJsonData);
            return Json(JsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveOrUpdateSubFrequency(string sFrequency)
        {
            JsonResultData objResult = new JsonResultData();
            var liFrequency = new List<SUP_FEE_FREQUENCY>();
            if (sFrequency != null)
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    var Result = JsonConvert.DeserializeObject<SUP_FEE_FREQUENCY>(sFrequency);
                    Result.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(Result.DATE_FROM);
                    Result.DATE_TO = CommonMethods.ConvertdatetoyearFromat(Result.DATE_TO);
                    Result.LAST_DATE_OF_PAYMENT = CommonMethods.ConvertdatetoyearFromat(Result.LAST_DATE_OF_PAYMENT);
                    Result.ACADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    if (string.IsNullOrEmpty(Result.FREQUENCY_ID))
                    {
                        liFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(Result, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.CheckIsFrequencyExisting)).DataSource.Data;
                    }
                    else
                    {
                        var sresultArgs = CMSHandler.SaveOrUpdate(Result, SupportDataSQL.GetSupportDataSQL((!string.IsNullOrEmpty(Result.FREQUENCY_ID)) ? SupportDataSQLCommands.UpdateSupFeeFrequency : SupportDataSQLCommands.SaveSubFeeFrequency));
                        objResult.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.ExpectationFailed;
                        objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                    }
                    if (liFrequency == null)
                    {
                        var sresultArgs = CMSHandler.SaveOrUpdate(Result, SupportDataSQL.GetSupportDataSQL((!string.IsNullOrEmpty(Result.FREQUENCY_ID)) ? SupportDataSQLCommands.UpdateSupFeeFrequency : SupportDataSQLCommands.SaveSubFeeFrequency));
                        objResult.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.ExpectationFailed;
                        objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                    }
                    else
                    {
                        objResult.Message = "Allready Exits ...!";
                    }

                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
            }

            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchSubFrequencyById(string sFrequencyId)
        {
            JsonResultData objResult = new JsonResultData();
            SUP_FEE_FREQUENCY objModel = new SUP_FEE_FREQUENCY();
            if (sFrequencyId != null)
            {
                string sOption = string.Empty;
                objModel.FREQUENCY_ID = sFrequencyId;
                var FetchFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFeeFrequencyById)).DataSource.Data;
                return Json(FetchFrequency);
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
                return Json(objResult, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteSupFrequency(string sFrequencyId)
        {
            JsonResultData ObjResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sFrequencyId))
                {
                    var objModel = new SUP_FEE_FREQUENCY();
                    objModel.FREQUENCY_ID = sFrequencyId;
                    var sresultArgs = CMSHandler.SaveOrUpdate(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.DeleteSupFeeFrequency));
                    ObjResult.sResult = (sresultArgs.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                    ObjResult.ErrorCode = Common.ErrorCode.Ok;
                    ObjResult.Message = Common.ErrorMessage.Ok;
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.BadRequest;
                    ObjResult.Message = Common.ErrorMessage.ExpectationFailed;
                }
            }
            else
            {
                ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fetch Program By ShiftId
        public JsonResult FetchProgramByShiftId(string sShiftId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sShiftId))
                {
                    try
                    {
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        var ObjShift = new Sup_Shift();
                        ObjShift.SHIFT_ID = sShiftId;
                        //var liprogram=(List<CP_PROGRAMME_2017>)CMSHandler.FetchData<>
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("SupportDataController", "FetchProgramByShiftId", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("SupportDataController", "FetchProgramByShiftId", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    }
                }
                else
                {
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region SupSelectionCycle

        public ActionResult SupSelectionCycleList()
        {
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objViewModel.liSelectionCycle = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupSelectionCycle)).DataSource.Data;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("SupportDataController", "SupSelectionCycleList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("SupportDataController", "SupSelectionCycleList", ex.Message);
                }
                return RedirectToAction("Index", "Account");
            }

            return View(objViewModel);
        }

        /// <summary>
        /// this action to fetch suphed info by headid 
        /// </summary>
        /// <param name="sSelectionCycleId"></param>
        /// <returns></returns>
        public JsonResult FetchSupSelectionCycleById(string sSelectionCycleId)
        {
            var ObjSupSelectionCycle = new SUP_SELECTION_CYCLE();
            var lisupSelectionCycle = new List<SUP_SELECTION_CYCLE>();
            ObjSupSelectionCycle.SELECTION_CYCLE_ID = sSelectionCycleId;
            lisupSelectionCycle = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(ObjSupSelectionCycle, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupSelectionCycleById)).DataSource.Data;
            return Json(lisupSelectionCycle, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// this action to Save Records ...
        /// </summary>
        /// <param name="HeadId"></param>
        /// <returns></returns>
        public JsonResult SaveSupSelectionCycle(string sJsonData)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() != null ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            JsonResultData objResultData = new JsonResultData();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<SUP_SELECTION_CYCLE>(sJsonData);
                    if (result != null)
                    {
                        if (result.SELECTION_CYCLE_ID != null && result.SELECTION_CYCLE_ID != "")
                        {
                            // UPDATE setting 
                            resultArgs = CMS.DAO.CMSHandler.SaveOrUpdate(result, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(DAO.SupportDataSQLCommands.UpdateSupSelectionCycle), sAcademicYear);
                            objResultData.Message = resultArgs.Success ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                            objResultData.ErrorCode = resultArgs.Success ? Common.ErrorCode.Ok : Common.ErrorCode.ExpectationFailed;
                        }
                        else
                        {
                            // ADD setting 
                            resultArgs = CMS.DAO.CMSHandler.SaveOrUpdate(result, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(DAO.SupportDataSQLCommands.SaveSupSelectionCycle), sAcademicYear);
                            objResultData.Message = resultArgs.Success ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                            objResultData.ErrorCode = resultArgs.Success ? Common.ErrorCode.Ok : Common.ErrorCode.ExpectationFailed;
                        }

                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog ObjLog = new ErrorLog())
                    {
                        ObjLog.WriteError("SupportData", "SaveSupSelectionCycle", ex.Message);
                    }
                }


            }
            else
            {
                objResultData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                objResultData.ErrorCode = Common.ErrorCode.ServiceUnavailable;
            }
            return Json(objResultData);
        }


        public JsonResult ActivateorDeactivateSupSelectionCycle(string sSelectionCycleId, string sMode)
        {

            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() != null ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            JsonResultData objResultData = new JsonResultData();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {

                if (!string.IsNullOrEmpty(sSelectionCycleId) && !string.IsNullOrEmpty(sMode))
                {
                    // fetch setting 
                    resultArgs = CMS.DAO.CMSHandler.SaveOrUpdate(new SUP_SELECTION_CYCLE() { SELECTION_CYCLE_ID = sSelectionCycleId, IS_ACTIVE = sMode }, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(DAO.SupportDataSQLCommands.ActivateOrDeactivateSupSelectionCycleById), sAcademicYear);
                    objResultData.Message = resultArgs.Success ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                }
            }
            return Json(objResultData);
        }

        public JsonResult IsShowActivateAcitvateOrDeactivateSupSelectionCycle(string sSelectionCycleId, string sMode)
        {

            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() != null ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            JsonResultData objResultData = new JsonResultData();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {

                if (!string.IsNullOrEmpty(sSelectionCycleId) && !string.IsNullOrEmpty(sMode))
                {
                    // fetch setting 
                    resultArgs = CMS.DAO.CMSHandler.SaveOrUpdate(new SUP_SELECTION_CYCLE() { SELECTION_CYCLE_ID = sSelectionCycleId, IS_SHOW_WEBSITE = sMode }, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(DAO.SupportDataSQLCommands.IsActivateOrDeactivateIsShowWebsiteSupSelectionCycleById), sAcademicYear);
                    objResultData.Message = resultArgs.Success ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                }
            }
            return Json(objResultData);
        }

        public JsonResult DeleteSelectionCycleById(string sSelectionCycleId)
        {
            JsonResultData ObjResult = new JsonResultData();
            if (!string.IsNullOrEmpty(sSelectionCycleId))
            {
                var ObjData = new SUP_SELECTION_CYCLE();
                ObjData.SELECTION_CYCLE_ID = sSelectionCycleId;
                ObjData.IS_DELETED = Common.Delimiter.ONE;
                resultArgs = CMSHandler.SaveOrUpdate(ObjData, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.DeleteSupSelectionCycle));
                ObjResult.Message = resultArgs.Success ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;

            }
            else
            {
                ObjResult.ErrorCode = Common.ErrorCode.BadRequest;
                ObjResult.Message = Common.ErrorMessage.ExpectationFailed;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}