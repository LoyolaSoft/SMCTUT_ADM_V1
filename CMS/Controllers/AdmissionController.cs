using CMS.DAO;
using CMS.Models;
using CMS.SQL.Communication;
using CMS.SQL.Dashboard;
using CMS.SQL.Examination;
using CMS.SQL.Fee;
using CMS.SQL.SupportData;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class AdmissionController : Controller
    {
        JsonResultData ObjJsonData = new JsonResultData();
        AdmissionViewModel ObjViewModel = new AdmissionViewModel();
        #region HccAdmissionWebsite
        public ActionResult HccIndex()
        {
            var academicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchActiveYear)).DataSource.Data;
            Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] = academicYear.FirstOrDefault().AC_YEAR;

            // Fetch selection cycle active and is show website 
            ObjViewModel.liSupSelectionCycle = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycleByActiveAndIsShowwebsite)).DataSource.Data;

            return View(ObjViewModel);
        }

        public ActionResult ListSelectedStudents()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                //  string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sAcademicYear = "2024";
                sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectedApplicants);
                objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(null, sSQL, sAcademicYear).DataSource.Data;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListSelectedStudents", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListSelectedStudents", ex.Message);
                }
            }
            return View(objViewModel);
        }

        public ActionResult ListSelectedStudentsSSC()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                //  string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sAcademicYear = "2024";
                sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectedApplicants);
                objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(null, sSQL, sAcademicYear).DataSource.Data;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListSelectedStudents", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListSelectedStudents", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        // GET: Admission
        [UserRights("APPLICANT")]
        public async Task<ActionResult> Index()
        {
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            var issuedApplication = await Task.Run(() => (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAppliedApplicationInfo), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data);
            if (issuedApplication == null)
            {
                ViewBag.Message = "Please Apply Programme and pay !";
            }
            else
            {
                ViewBag.Message = string.Empty;
                ViewBag.STATUS = issuedApplication.FirstOrDefault().STATUS_ID;
                ViewBag.IS_BLOCKED = issuedApplication.FirstOrDefault().IS_BLOCKED;
            }
            return View();
        }

        public ActionResult AdditionalCourses()
        {
            // ViewBag.QRcode = CommonMethods.GetEncodeQRCodeWithLogo("erp.stmaryscollege.edu.in");
            return View();
        }

        [UserRights("ADMISSION ADMIN")]
        public ActionResult IssuedList()
        {

            return View();
        }
        public async Task<JsonResult> FetchAdmissionIssueApplicationById()
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_RECEIVE_APPLICATION objModel = new ADM_RECEIVE_APPLICATION();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.RECEIVE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    admissionViewModel.liApplicant = await Task.Run(() => (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionIssueApplicationById), sAcademicYear).DataSource.Data);
                    if (admissionViewModel.liApplicant != null && admissionViewModel.liApplicant.Count > 0)
                    {
                        Session[Common.SESSION_VARIABLES.HSC_NO] = admissionViewModel.liApplicant.FirstOrDefault().HSC_NO;
                        Session[Common.SESSION_VARIABLES.RECEIVE_ID] = admissionViewModel.liApplicant.FirstOrDefault().RECEIVE_ID;
                        Session[Common.SESSION_VARIABLES.APPLICATION_NO] = admissionViewModel.liApplicant.FirstOrDefault().APPLICATION_NO;
                        Session[Common.SESSION_VARIABLES.APPLICATION_TYPE_ID] = admissionViewModel.liApplicant.FirstOrDefault().APPLICATION_TYPE_ID;
                        Session[Common.SESSION_VARIABLES.PROGRAMME_ID] = admissionViewModel.liApplicant.FirstOrDefault().PROGRAMME_ID;
                        Session[Common.SESSION_VARIABLES.PHOTO_PATH] = admissionViewModel.liApplicant.FirstOrDefault().PHOTO_PATH;
                        return Json(admissionViewModel.liApplicant, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.NoContent;
                        objResult.Message = Common.ErrorMessage.NoContent;
                        return Json(objResult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "Index", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "Index", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public async Task<ActionResult> FetchSubejectsByHSSGroupId(string sHSSGroupId)
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objIssueModel = new ADM_ISSUE_APPLICATION_2018();
            ADM_STU_SUBMARKS objModel = new ADM_STU_SUBMARKS();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.RECEIVE_STUID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    objModel.ACADEMIC_YEAR = sAcademicYear;
                    objModel.HSS_GROUP = sHSSGroupId;

                    //objIssueModel.PROGRAMME_GROUP_ID = Session[Common.SESSION_VARIABLES.PROGRAMME_GROUP_ID].ToString();
                    objIssueModel.RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    admissionViewModel.liApplicant = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(objIssueModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchTotalByIssueId), sAcademicYear).DataSource.Data;
                    //if (string.IsNullOrEmpty(sHSSGroupId) && admissionViewModel.liApplicant!=null && admissionViewModel.liApplicant.Count>0)
                    //{
                    //    if (admissionViewModel.liApplicant.FirstOrDefault().APPLICATION_TYPE_ID=="1")
                    //    {
                    //        objModel.HSS_GROUP = "139";
                    //    }
                    //    else
                    //    {
                    //        objModel.HSS_GROUP = "136";
                    //    }
                    //}
                    //admissionViewModel.liEligibility = (List<ADM_ELEGIBILITIES>)CMSHandler.FetchData<ADM_ELEGIBILITIES>(objIssueModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmEligibilityByProgrammeGroupIdId), sAcademicYear).DataSource.Data;
                    admissionViewModel.liADMStudentMarks = await Task.Run(() => (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHssSubjectsByReceiveId), sAcademicYear).DataSource.Data);
                    admissionViewModel.liADMStudentMarksFor11th = await Task.Run(() => (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHssSubjectsFor11ThByReceiveId), sAcademicYear).DataSource.Data);
                    var lisubjects = await Task.Run(() => (List<SUP_HSS_SUBJECTS>)CMSHandler.FetchData<SUP_HSS_SUBJECTS>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllSubjects), sAcademicYear).DataSource.Data);
                    if (lisubjects != null && lisubjects.Count > 0)
                    {
                        admissionViewModel.lisubjects = lisubjects;
                    }
                    var markTemplateByGroup = await Task.Run(() => (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHssSubjectsByHSSGroup), sAcademicYear).DataSource.Data);

                    if (admissionViewModel.liADMStudentMarksFor11th == null)
                    {
                        admissionViewModel.liADMStudentMarksFor11th = markTemplateByGroup;
                    }
                    if (admissionViewModel.liADMStudentMarks != null)
                        return View(admissionViewModel);
                    else
                    {
                        admissionViewModel.liADMStudentMarks = markTemplateByGroup;
                        return View(admissionViewModel);
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchSubejectsByHSSGroupId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchSubejectsByHSSGroupId", ex.Message);
                    return View(admissionViewModel);
                }
            }

        }
        public async Task<ActionResult> FetchUploadedFiles()
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_UPLOADED_FILES_2018 objFileModel = new ADM_UPLOADED_FILES_2018();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objFileModel.RECEIVE_STU_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    admissionViewModel.liUploadedFile = await Task.Run(() => (List<ADM_UPLOADED_FILES_2018>)CMSHandler.FetchData<ADM_UPLOADED_FILES_2018>(objFileModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUploadedImagesById), sAcademicYear).DataSource.Data);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchUploadedFiles", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchUploadedFiles", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            return View(admissionViewModel);
        }
        public async Task<ActionResult> FetchSubjectsById(string sHSSGroupId)
        {
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_STU_SUBMARKS objModel = new ADM_STU_SUBMARKS();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.RECEIVE_STUID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    if (!string.IsNullOrEmpty(objModel.RECEIVE_STUID))
                    {
                        objModel.ACADEMIC_YEAR = DateTime.Now.Year.ToString();
                        objModel.HSS_GROUP = sHSSGroupId;
                        admissionViewModel.liADMStudentMarks = await Task.Run(() => (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHssSubjectsByReceiveId), sAcademicYear).DataSource.Data);
                        if (admissionViewModel.liADMStudentMarks != null)
                            return View(admissionViewModel);
                        else
                        {
                            objModel.ACADEMIC_YEAR = DateTime.Now.Year.ToString();
                            admissionViewModel.liADMStudentMarks = await Task.Run(() => (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHssSubjectsByHSSGroup), sAcademicYear).DataSource.Data);
                            return View(admissionViewModel);
                        }
                    }
                    else
                    {
                        return View(admissionViewModel);
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchSubjectsById", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchSubjectsById", ex.Message);
                    return View(admissionViewModel);
                }
            }
        }
        public JsonResult DeleteUploadedFile(string sFileId, string sFilePath)
        {
            JsonResultData ObjResult = new JsonResultData();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sFileId))
                {
                    var objModel = new ADM_UPLOADED_FILES_2018();
                    objModel.FILE_ID = sFileId;
                    var FetchFilePathById = (List<ADM_UPLOADED_FILES_2018>)CMSHandler.FetchData<ADM_UPLOADED_FILES_2018>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchFilePathById), sAcademicYear).DataSource.Data;
                    if (FetchFilePathById != null && FetchFilePathById.Count > 0)
                        objModel.FILE_PATH = FetchFilePathById.SingleOrDefault().FILE_PATH;
                    objModel.FILE_PATH = objModel.FILE_PATH.Remove(0, 1);
                    objModel.FILE_PATH = AppDomain.CurrentDomain.BaseDirectory + objModel.FILE_PATH;
                    FileInfo file = new FileInfo(objModel.FILE_PATH);
                    if (file.Exists)
                        file.Delete();
                    var sresultArgs = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.DeleteFileUploads), sAcademicYear);
                    ObjResult.Message = (sresultArgs.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                    ObjResult.ErrorCode = Common.ErrorCode.Ok;
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
        public async Task<JsonResult> SaveOrUpdateStudentMarks(string sMarkjson, string sLastStudiedClass, string sMarkjsonFot11th, string sEleventhPass)
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            ADM_STU_SUBMARKS objModel = new ADM_STU_SUBMARKS();
            ADM_ISSUE_APPLICATION_2018 objIssueModel = new ADM_ISSUE_APPLICATION_2018();
            decimal Total = 0;
            decimal MaxTotal = 0;
            decimal CutOff = 0;
            decimal Percentage = 0;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JSON_ADM_STU_SUBMARKS objJson = serializer.Deserialize<JSON_ADM_STU_SUBMARKS>(sMarkjson);
                    objIssueModel.LAST_STUDIED_CLASS = sLastStudiedClass;
                    objIssueModel.RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    objIssueModel.IS_ELEVENTH_PASS = sEleventhPass;
                    var ResultArg = CMSHandler.SaveOrUpdate(objIssueModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.SaveorUpdateIsEleventhPass));

                    if (objJson.SAVE_ADM_SUB_STU_MARKS != null && objJson.SAVE_ADM_SUB_STU_MARKS.Count > 0)
                    {
                        foreach (var item in objJson.SAVE_ADM_SUB_STU_MARKS)
                        {
                            Total = Total + (Convert.ToDecimal(item.MARK));
                            MaxTotal = MaxTotal + (Convert.ToDecimal(item.MAX_MARK));
                            if (item.PART == "3")
                            {
                                CutOff = CutOff + (Convert.ToDecimal(item.MARK));
                            }
                            item.RECEIVE_STUID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                            var FetchExistingMarkBySubjectIdandReceiveId = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuMarksBySubjectIdAndReceiveId), sAcademicYear).DataSource.Data;
                            if (FetchExistingMarkBySubjectIdandReceiveId != null && FetchExistingMarkBySubjectIdandReceiveId.Count > 0)
                            {
                                var SaveStuMarks = await Task.Run(() => CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateAdmissionStudentSubMarks), sAcademicYear));
                                if (SaveStuMarks.Success)
                                    objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                else
                                    objResult.Message = Common.Messages.FailedToSaveRecords;
                            }
                            else
                            {
                                var UpdateStuMarks = await Task.Run(() => CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmissionStudentSubMarks), sAcademicYear));
                                if (UpdateStuMarks.Success)
                                    objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                else
                                    objResult.Message = Common.Messages.FailedToSaveRecords;
                            }
                            //var result = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL((!string.IsNullOrEmpty(item.MARK)) ? AdmissionSQLCommands.UpdateAdmissionStudentSubMarks : AdmissionSQLCommands.SaveAdmissionStudentSubMarks), sAcademicYear);
                        }
                        objIssueModel.HSTOTAL = Convert.ToString(Total);
                        objIssueModel.HS_MAX_MARK = Convert.ToString(MaxTotal);
                        objIssueModel.TOTAL_CUT_OFF_MARK = Convert.ToString(CutOff);
                        Percentage = Math.Round((decimal)(Total * 100) / MaxTotal, 2);
                        objIssueModel.HSPERCENTAGE = Convert.ToString(Percentage);
                        var UpdateTotal = await Task.Run(() => CMSHandler.SaveOrUpdate(objIssueModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateReceiveTotal), sAcademicYear));
                        if (UpdateTotal.Success)
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        else
                            objResult.Message = Common.Messages.FailedToSaveRecords;

                        //11th marks updatation
                        objJson = serializer.Deserialize<JSON_ADM_STU_SUBMARKS>(sMarkjsonFot11th);
                        if (objJson.SAVE_ADM_SUB_STU_MARKS != null && objJson.SAVE_ADM_SUB_STU_MARKS.Count > 0)
                        {
                            foreach (var item in objJson.SAVE_ADM_SUB_STU_MARKS)
                            {
                                if (string.IsNullOrEmpty(item.MARK))
                                {
                                    continue;
                                }
                                item.RECEIVE_STUID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                                var FetchExistingMarkBySubjectIdandReceiveId = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuMarksBySubjectIdAndReceiveIdFor11th), sAcademicYear).DataSource.Data;
                                if (FetchExistingMarkBySubjectIdandReceiveId != null && FetchExistingMarkBySubjectIdandReceiveId.Count > 0)
                                {
                                    var SaveStuMarks = await Task.Run(() => CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateAdmissionStudentSubMarksFor11th), sAcademicYear));
                                }
                                else
                                {
                                    var UpdateStuMarks = await Task.Run(() => CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmissionStudentSubMarksFor11th), sAcademicYear));
                                }
                                //var result = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL((!string.IsNullOrEmpty(item.MARK)) ? AdmissionSQLCommands.UpdateAdmissionStudentSubMarks : AdmissionSQLCommands.SaveAdmissionStudentSubMarks), sAcademicYear);
                            }
                        }
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.BadRequest;
                        objResult.Message = Common.ErrorMessage.ExpectationFailed;
                        return Json(objResult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateStudentMarks", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateStudentMarks", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDropzoneFile()
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            ADM_UPLOADED_FILES_2018 objFileModel = new ADM_UPLOADED_FILES_2018();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string filePath = string.Empty;
                    string isSubmitClick = string.Empty;
                    string sImagePath = string.Empty;
                    string _sDirectorypath = string.Empty;
                    string sContent = string.Empty;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();

                    objModel.RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();

                    _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_APPLICANT_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    if (!Directory.Exists(_sDirectorypath))
                    {
                        Directory.CreateDirectory(_sDirectorypath);
                    }
                    //string Status = Request.Form["Status"];
                    //if (Status != "0" && !string.IsNullOrEmpty(Status))
                    //{
                    //    if (Status == Common.STATUS.Registered)
                    //        objModel.STATUS = (objModel.IS_SUBMITTED != "1") ? Common.STATUS.Registered : Common.STATUS.Submitted;
                    //    else
                    //        objModel.STATUS = Status;
                    //}
                    //else
                    //    objModel.STATUS = (objModel.IS_SUBMITTED != "1") ? Common.STATUS.Registered : Common.STATUS.Submitted;
                    //if (isSubmitClick.Equals("1"))
                    //{
                    //    var UpdateSubmitted = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateApplicationSubmittion));
                    //    if (UpdateSubmitted.Success)
                    //    {
                    //        var FetchFeeStudentAccount = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    //        if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                    //        {
                    //            var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Submitted }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                    //            sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, FetchFeeStudentAccount.FirstOrDefault().FIRST_NAME);
                    //            //sContent = "Dear " + FetchFeeStudentAccount.FirstOrDefault().FIRST_NAME + ",\n\nGreetings,\n Your Application has been submitted Successfully. \n Note:\n If your application is selected you will receive a message and email for your confirmation.  \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.\nPh:0461-2320946. ";
                    //            SendEmailAndTextMessage(sContent, FetchFeeStudentAccount.FirstOrDefault().SMS_MOBILE_NO, FetchFeeStudentAccount.FirstOrDefault().EMAIL_ID, "Application is submitted successfully.");
                    //        }

                    //    }
                    //}
                    objFileModel.RECEIVE_STU_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    var FetchImageUploadsCount = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objFileModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFileUploadsCount), sAcademicYear).DataSource.Data;
                    var count = Convert.ToInt32(FetchImageUploadsCount.FirstOrDefault().FILE_COUNT);
                    var i = 0;
                    if (Request.Files.Count > 0)
                    {
                        foreach (var item in Request.Files)
                        {
                            var File = Request.Files[i];
                            count++;
                            string ImgName = File.FileName;
                            var Img = ImgName.Split('.');
                            string fileName = ImgName.Replace(Img[0], Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() + "_" + count);

                            if (!string.IsNullOrEmpty(fileName))
                            {
                                filePath = _sDirectorypath + "\\" + fileName;
                               sImagePath = "\\" + Common.URLPages.STUDENT_APPLICANT_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + fileName;
                                File.SaveAs(filePath);
                                objFileModel.FILE_PATH = sImagePath;
                            }
                            i++;
                            var SaveFileUploads = CMSHandler.SaveOrUpdate(objFileModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveFileUploads), sAcademicYear);
                            if (SaveFileUploads.Success)
                                objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                            else
                                objResult.Message = Common.Messages.FailedToSaveRecords;
                        }

                    }
                    else
                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveUploadedFiles", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveUploadedFiles", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveUploadedFiles()
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            ADM_UPLOADED_FILES_2018 objFileModel = new ADM_UPLOADED_FILES_2018();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string filePath = string.Empty;
                    string isSubmitClick = string.Empty;
                    string sImagePath = string.Empty;
                    string _sDirectorypath = string.Empty;
                    string sContent = string.Empty;
                    string sContentTamil = string.Empty;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.IS_SUBMITTED = Request.Form["IS_SUBMITTED"];
                    objModel.IS_HAVE_CONSOLIDATE_MARKSHEET = Request.Form["ConsolidateMarkSheet"];
                    isSubmitClick = Request.Form["Is_Submit_Click"];
                    objModel.RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    //string ClassName = Session[Common.SESSION_VARIABLES.COURSE_CODE].ToString();
                    //ClassName = ClassName.Replace(".", "");
                    _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_APPLICANT_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    if (!Directory.Exists(_sDirectorypath))
                    {
                        Directory.CreateDirectory(_sDirectorypath);
                    }
                    string Status = Request.Form["Status"];
                    if (Status != "0" && !string.IsNullOrEmpty(Status))
                    {
                        if (Status == Common.STATUS.Registered)
                            objModel.STATUS = (objModel.IS_SUBMITTED != "1") ? Common.STATUS.Registered : Common.STATUS.Submitted;
                        else
                            objModel.STATUS = Status;
                    }
                    else
                        objModel.STATUS = (objModel.IS_SUBMITTED != "1") ? Common.STATUS.Registered : Common.STATUS.Submitted;
                    if (isSubmitClick.Equals("1"))
                    {
                        var UpdateSubmitted = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateApplicationSubmittion));
                        if (UpdateSubmitted.Success)
                        {
                            var FetchFeeStudentAccount = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                            if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                            {
                                var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Submitted }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, FetchFeeStudentAccount.FirstOrDefault().FIRST_NAME);
                                // tamil

                                var MessageContentTamil = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Submitted_Tamil }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                sContentTamil = MessageContentTamil.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, FetchFeeStudentAccount.FirstOrDefault().FIRST_NAME);

                                SendEmailAndTextMessage(sContent, FetchFeeStudentAccount.FirstOrDefault().SMS_MOBILE_NO, FetchFeeStudentAccount.FirstOrDefault().EMAIL_ID, MessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Application is submitted successfully.", "0");
                                SendEmailAndTextMessage(sContentTamil, FetchFeeStudentAccount.FirstOrDefault().SMS_MOBILE_NO, FetchFeeStudentAccount.FirstOrDefault().EMAIL_ID, MessageContentTamil.FirstOrDefault().SMS_TEMPLATE_ID, "Application is submitted successfully.", "1");
                            }
                        }
                    }
                    objFileModel.RECEIVE_STU_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    var FetchImageUploadsCount = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objFileModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFileUploadsCount), sAcademicYear).DataSource.Data;
                    var count = Convert.ToInt32(FetchImageUploadsCount.FirstOrDefault().FILE_COUNT);
                    var i = 0;
                    if (Request.Files.Count > 0)
                    {
                        foreach (var item in Request.Files)
                        {
                            var File = Request.Files[i];
                            count++;
                            string ImgName = File.FileName;
                            var Img = ImgName.Split('.');
                            string fileName = ImgName.Replace(Img[0], Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() + "_" + count);

                            if (!string.IsNullOrEmpty(fileName))
                            {
                                filePath = _sDirectorypath + "\\" + fileName;
                                sImagePath = "\\" + Common.URLPages.STUDENT_APPLICANT_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + fileName;
                                File.SaveAs(filePath);
                                objFileModel.FILE_PATH = sImagePath;
                            }
                            i++;
                            var SaveFileUploads = CMSHandler.SaveOrUpdate(objFileModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveFileUploads), sAcademicYear);
                            if (SaveFileUploads.Success)
                                objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                            else
                                objResult.Message = Common.Messages.FailedToSaveRecords;
                        }

                    }
                    else
                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveUploadedFiles", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveUploadedFiles", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        // Save & Update .....
        public JsonResult SaveOrUpdateAdmissionIssueApplicationAndReceiveApplication()
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_STU_SUBMARKS objModel = new ADM_STU_SUBMARKS();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string filePath = string.Empty;
                    string sImagePath = string.Empty;
                    string _sDirectorypath = string.Empty;
                    var SaveInfo = Request.Form["SaveAdmIssueApplication"];
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var Result = JsonConvert.DeserializeObject<ADM_ISSUE_APPLICATION_2018>(SaveInfo);
                    if (Result != null)
                    {
                        Result.DATE_OF_BIRTH = CommonMethods.ConvertdatetoyearFromat(Result.DOB);
                        Result.PHOTO_PATH = Session[Common.SESSION_VARIABLES.PHOTO_PATH].ToString();
                        //string ClassName = Session[Common.SESSION_VARIABLES.COURSE_CODE].ToString();
                        // ClassName = ClassName.Replace(".", "");
                        _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        if (!Directory.Exists(_sDirectorypath))
                        {
                            Directory.CreateDirectory(_sDirectorypath);
                        }
                        Result.ACADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        Result.SMS_MOBILE_NO = Result.MOBILE_NO;
                        Result.RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                        //Result.PROGRAMME_GROUP_ID = Session[Common.SESSION_VARIABLES.PROGRAMME_GROUP_ID].ToString();
                        // Result.ISSUE_ID = Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString();
                        Result.APPLICATION_TYPE_ID = Session[Common.SESSION_VARIABLES.APPLICATION_TYPE_ID].ToString();
                        // Result.PROGRAMME_ID = Session[Common.SESSION_VARIABLES.PROGRAMME_ID].ToString();

                        var PhyFile = Request.Files["physicaly"];
                        if (PhyFile != null)
                        {
                            string fileName = PhyFile.FileName; ;
                            filePath = _sDirectorypath + "\\" + fileName;
                            sImagePath = "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + fileName;
                            PhyFile.SaveAs(filePath);
                            Result.PHYSICALY_CHALLENGED_PROOF = sImagePath;
                            var Resultarg = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatePhysicalyChallangedProof), sAcademicYear);
                        }

                        //var NRIFile = Request.Files["nri"];
                        //if (NRIFile != null)
                        //{
                        //    string fileName = NRIFile.FileName; ;
                        //    filePath = _sDirectorypath + "\\" + fileName;
                        //    sImagePath = "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + fileName;
                        //    NRIFile.SaveAs(filePath);
                        //    Result.NRI_PROOF = sImagePath;
                        //    var Resultarg = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateNRIProof), sAcademicYear);
                        //}
                        var ExserviceFile = Request.Files["exservice"];
                        if (ExserviceFile != null)
                        {
                            string fileName = ExserviceFile.FileName; ;
                            filePath = _sDirectorypath + "\\" + fileName;
                            sImagePath = "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + fileName;
                            ExserviceFile.SaveAs(filePath);
                            Result.EX_SERVICEMAN_PROOF = sImagePath;
                            var Resultarg = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateExservicemanProof), sAcademicYear);
                        }
                        var Community = Request.Files["community"];
                        if (Community != null)
                        {
                            string fileName = Community.FileName; ;
                            filePath = _sDirectorypath + "\\" + fileName;
                            sImagePath = "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + fileName;
                            Community.SaveAs(filePath);
                            Result.COMMUNITY_PROOF = sImagePath;
                            var Resultarg = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateCommunityProof), sAcademicYear);
                        }
                        var Orphant = Request.Files["orphant"];
                        if (Orphant != null)
                        {
                            string fileName = Orphant.FileName; ;
                            filePath = _sDirectorypath + "\\" + fileName;
                            sImagePath = "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + fileName;
                            Orphant.SaveAs(filePath);
                            Result.SINGLE_PARENT_PROOF = sImagePath;
                            var Resultarg = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateSingleParentProof), sAcademicYear);
                        }
                        var File = Request.Files["photo"];
                        if (File != null)
                        {
                            string ImgName = File.FileName;
                            var Img = ImgName.Split('.');
                            string fileName = ImgName.Replace(Img[0], Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString());

                            if (!string.IsNullOrEmpty(fileName))
                            {
                                filePath = _sDirectorypath + "\\" + fileName;
                                sImagePath = "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + fileName;
                                File.SaveAs(filePath);
                                Result.PHOTO_PATH = sImagePath;
                            }

                        }
                        string Status = Request.Form["Status"];
                        if (Status != "0" && !string.IsNullOrEmpty(Status))
                        {
                            if (Status == Common.STATUS.Registered)
                                Result.STATUS = (Result.IS_SUBMITTED != "1") ? Common.STATUS.Registered : Common.STATUS.Submitted;
                            else
                                Result.STATUS = Status;
                        }
                        else
                            Result.STATUS = (Result.IS_SUBMITTED != "1") ? Common.STATUS.Registered : Common.STATUS.Submitted;

                        var sresultArgs = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateReceiveAplicationById), sAcademicYear, true);
                        if (sresultArgs.Success)
                        {
                            objResult.ErrorCode = Common.ErrorCode.Ok;
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                            return Json(objResult, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            objResult.ErrorCode = Common.ErrorCode.BadRequest;
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                            return Json(objResult, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.BadRequest;
                        objResult.Message = Common.Messages.FailedToSaveRecords;
                        return Json(objResult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateAdmissionIssueApplicationAndReceiveApplication", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateAdmissionIssueApplicationAndReceiveApplication", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// fetch all drop down fields for application form
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> FetchSupportDataForDropDown()
        {
            JsonResultData objResult = new JsonResultData();
            Student_Personal_Info objModel = new Student_Personal_Info();
            string sMotherTongue = string.Empty;
            string sReligion = string.Empty;
            string sCaste = string.Empty;
            string sCountry = string.Empty;
            string sGender = string.Empty;
            string sHSSGroups = string.Empty;
            string sOccupation = string.Empty;
            string sClass = string.Empty;
            string sProgrammeGroups = string.Empty;
            string sBloodGroup = string.Empty;
            string sUniversity = string.Empty;
            string sState = string.Empty;
            string sMainCommunity = string.Empty;
            string sEducationboard = string.Empty;
            string sPhysicalychallangedtype = string.Empty;
            string sSemester = string.Empty;
            string JsonData = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                //objModel.CLASS = sClassId;
                var FetchMotherTongue = await Task.Run(() => (List<SupMotherTongue>)CMSHandler.FetchData<SupMotherTongue>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMotherTongue)).DataSource.Data);
                var FetchReligion = await Task.Run(() => (List<SUP_RELIGION>)CMSHandler.FetchData<SUP_RELIGION>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchReligion), sAcademicyear).DataSource.Data);
                var FetchCaste = await Task.Run(() => (List<SupCommunity>)CMSHandler.FetchData<SupCommunity>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCommunity), sAcademicyear).DataSource.Data);
                var FetchCountry = await Task.Run(() => (List<SUP_COUNTRY>)CMSHandler.FetchData<SUP_COUNTRY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCountry), sAcademicyear).DataSource.Data);
                var FetchGender = await Task.Run(() => (List<SupGender>)CMSHandler.FetchData<SupGender>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchGender), sAcademicyear).DataSource.Data);
                var FetchHSSGroups = await Task.Run(() => (List<SUP_HSS_GROUPS>)CMSHandler.FetchData<SUP_HSS_GROUPS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHSSGroups), sAcademicyear).DataSource.Data);
                var FetchOccupation = await Task.Run(() => (List<Sup_Occupation>)CMSHandler.FetchData<Sup_Occupation>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOccupation), sAcademicyear).DataSource.Data);
                var FetchProgrammeGroups = await Task.Run(() => (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicyear).DataSource.Data);
                var Fetchclass = await Task.Run(() => (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchADMClass), sAcademicyear).DataSource.Data);
                var FetchBloodGroup = await Task.Run(() => (List<SupBloodGroup>)CMSHandler.FetchData<SupBloodGroup>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchBloodGroup), sAcademicyear).DataSource.Data);
                var FetchUniversity = await Task.Run(() => (List<SUP_UNIVERSITY>)CMSHandler.FetchData<SUP_UNIVERSITY>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchUniversity), sAcademicyear).DataSource.Data);
                var FetchState = await Task.Run(() => (List<SUP_STATE>)CMSHandler.FetchData<SUP_STATE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchState), sAcademicyear).DataSource.Data);
                var FetchMainCommunity = await Task.Run(() => (List<MainCommunity>)CMSHandler.FetchData<MainCommunity>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMainCommunity), sAcademicyear).DataSource.Data);
                var FetchEducationboard = await Task.Run(() => (List<SUP_EDUCATION_BOARD>)CMSHandler.FetchData<SUP_EDUCATION_BOARD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupEducationBoard), sAcademicyear).DataSource.Data);
                var FetchSemester = await Task.Run(() => (List<SUP_SEMESTER>)CMSHandler.FetchData<SUP_SEMESTER>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupSemester), sAcademicyear).DataSource.Data);
                var FetchPhysicalytype = await Task.Run(() => (List<SUP_PHYSICALY_CHALLENGED_TYPES>)CMSHandler.FetchData<SUP_PHYSICALY_CHALLENGED_TYPES>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchPhysicalychallangedtype), sAcademicyear).DataSource.Data);
                if (FetchMotherTongue != null && FetchMotherTongue.Count > 0)
                {
                    foreach (var item in FetchMotherTongue)
                        sMotherTongue += "<option value='" + item.MOTHER_TONGUE_ID + "'>" + item.MOTHER_TONGUE_NAME + "</option>";
                }
                if (FetchReligion != null && FetchReligion.Count > 0)
                {
                    foreach (var item in FetchReligion)
                        sReligion += "<option value='" + item.RELIGIONID + "'>" + item.RELIGION + "</option>";
                }
                if (FetchCaste != null && FetchCaste.Count > 0)
                {
                    foreach (var item in FetchCaste)
                        sCaste += "<option value='" + item.COMMUNITYID + "'>" + item.COMMUNITY + "</option>";
                }
                if (FetchCountry != null && FetchCountry.Count > 0)
                {
                    foreach (var item in FetchCountry)
                        sCountry += "<option value='" + item.COUNTRY_ID + "'>" + item.COUNTRY_NAME + "</option>";
                }
                if (FetchGender != null && FetchGender.Count > 0)
                {
                    foreach (var item in FetchGender)
                        sGender += "<option value='" + item.GENDER_ID + "'>" + item.GENDER_NAME + "</option>";
                }
                if (FetchHSSGroups != null && FetchHSSGroups.Count > 0)
                {
                    foreach (var item in FetchHSSGroups)
                        sHSSGroups += "<option value='" + item.HSS_GROUP_ID + "'>" + item.HSS_GROUP + "</option>";
                }
                if (FetchOccupation != null && FetchOccupation.Count > 0)
                {
                    foreach (var item in FetchOccupation)
                        sOccupation += "<option value='" + item.OCCUPATION_ID + "'>" + item.OCCUPATION_NAME + "</option>";
                }
                if (FetchProgrammeGroups != null && FetchProgrammeGroups.Count > 0)
                {
                    foreach (var item in FetchProgrammeGroups)
                        sProgrammeGroups += "<option value='" + item.PRO_GROUPS_ID + "'>" + item.PROGRAMME_NAME + "</option>";
                }
                if (Fetchclass != null && Fetchclass.Count > 0)
                {
                    foreach (var item in Fetchclass)
                        sClass += "<option value='" + item.CLASS_ID + "'>" + item.CLASS_NAME + "</option>";
                }
                if (FetchBloodGroup != null && FetchBloodGroup.Count > 0)
                {
                    foreach (var item in FetchBloodGroup)
                        sBloodGroup += "<option value='" + item.BLOOD_GROUP_ID + "'>" + item.BLOOD_GROUP_NAME + "</option>";
                }
                if (FetchUniversity != null && FetchUniversity.Count > 0)
                {
                    foreach (var item in FetchUniversity)
                        sUniversity += "<option value='" + item.UNIVERSITY_ID + "'>" + item.UNIVERSITY + "</option>";
                }
                if (FetchState != null && FetchState.Count > 0)
                {
                    foreach (var item in FetchState)
                        sState += "<option value='" + item.STATE_ID + "'>" + item.STATE + "</option>";
                }
                if (FetchMainCommunity != null && FetchMainCommunity.Count > 0)
                {
                    foreach (var item in FetchMainCommunity)
                        sMainCommunity += "<option value='" + item.MAIN_COMMUNITY_ID + "'>" + item.MAIN_COMMUNITY + "</option>";
                }
                if (FetchEducationboard != null && FetchEducationboard.Count > 0)
                {
                    foreach (var item in FetchEducationboard)
                        sEducationboard += "<option value='" + item.BOARD_ID + "'>" + item.BOARD_NAME + "</option>";
                }
                if (FetchPhysicalytype != null && FetchPhysicalytype.Count > 0)
                {
                    foreach (var item in FetchPhysicalytype)
                        sPhysicalychallangedtype += "<option value='" + item.TYPE_ID + "' >" + item.TYPE + "</option>";
                }
                if (FetchSemester != null && FetchSemester.Count > 0)
                {
                    foreach (var item in FetchSemester)
                        sSemester += "<option value='" + item.SUP_SEMESTER_ID + "' >" + item.SEMESTER_NAME + "</option>";
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            var objJsonData = new ADM_ISSUE_APPLICATION_2018() { MOTHER_TONGUE = sMotherTongue, RELIGION_ID = sReligion, CASTE = sCaste, CCOUNTRY = sCountry, GENDER = sGender, HSS_GROUP_ID = sHSSGroups, OCCUPATION = sOccupation, PROGRAMME_GROUP_ID = sProgrammeGroups, CLASS_ID = sClass, BLOOD_GROUP = sBloodGroup, UNIVERSITY = sUniversity, CSTATE = sState, MAIN_COMMUNITY = sMainCommunity, EDUCATION_BOARD_ID = sEducationboard, PHYSICALY_CHALLANGED_TYPE = sPhysicalychallangedtype, UG_TOTAL_SEMESTER = sSemester };
            JsonData = JsonConvert.SerializeObject(objJsonData);
            return Json(JsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Home()
        {
            // Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] = "2018";
            return View();
        }
        public ActionResult Courses()
        {
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            //  Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] = "2018";
            string sSql = string.Empty;
            //string sAcademicYear = "2018";
            //var liCourse = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCoursesOffered), sAcademicYear).DataSource.Data;
            admissionViewModel.programmeType = (List<SupProgrammeLevel>)CMSHandler.FetchData<SupProgrammeLevel>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeLevel)).DataSource.Data;
            admissionViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchTempCourse)).DataSource.Data;
            return View(admissionViewModel);
        }
        public ActionResult Prospectus()
        {
            return View();
        }
        public ActionResult Admissionprocess()
        {
            return View();
        }
        public ActionResult contact()
        {
            return View();
        }
        public ActionResult HostelAdmission()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public string ValidateOTP(string sJson)
        {

            bool sResult = false;
            string sTemp_id = string.Empty;
            string sTemp_idTamil = string.Empty;
            var issueApplation = JsonConvert.DeserializeObject<ADM_ISSUE_APPLICATION_2018>(sJson);
            //issueApplation.DOB = CommonMethods.ConvertdatetoyearFromat(issueApplation.DOB);


            if (Session[Common.SESSION_VARIABLES.OTP] != null)
            {
                string sOTP = issueApplation.OTP;
                string sSessionOTP = CommonMethods.CreateMD5(Session[Common.SESSION_VARIABLES.OTP].ToString());
                string sContent = string.Empty;
                string sTContent = string.Empty;
                string sTContentTamil = string.Empty;
                string sUserAccountId = string.Empty;
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (string.Equals(sSessionOTP.ToUpper(), CommonMethods.CreateMD5(sOTP.ToUpper())))
                {
                    sResult = true;
                    Session[Common.SESSION_VARIABLES.OTP] = null;
                    var result = CMSHandler.SaveOrUpdate(issueApplation, CMS.SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveIssueApplications), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString(), true);
                    if (result.Success)
                    {
                        var lastInsertedInfo = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018 { EMAIL_ID = issueApplation.EMAIL_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.IsExistEmailId)).DataSource.Data;
                        //UserAccount Creation.
                        var isExistingUser = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(new USER_ACCOUNT_INFO() { USER_ID = issueApplation.RECEIVE_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUserAccountByUserId), sAcademicYear).DataSource.Data;
                        sUserAccountId = (isExistingUser != null && isExistingUser.Count > 0) ? isExistingUser.FirstOrDefault().USER_ACCOUNT_ID : string.Empty;

                        var update = CMSHandler.SaveOrUpdate(new USER_ACCOUNT_INFO() { USERNAME = issueApplation.EMAIL_ID.Trim(), PASSWORD = CommonMethods.GetSha256FromString(issueApplation.PASSWORD), USER_ID = lastInsertedInfo.FirstOrDefault().RECEIVE_ID, ROLE = Common.USER_ROLE_IDS.APPLICANT, EMAIL = issueApplation.EMAIL_ID, MOBILE = issueApplation.SMS_MOBILE_NO, NAME = issueApplation.FIRST_NAME }, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(string.IsNullOrEmpty(sUserAccountId) ? UserAccountSQLCommands.SaveUserAccount : UserAccountSQLCommands.UpdateUserAccount), sAcademicYear);
                        if (update.Success)
                        {
                            sContent = string.Empty;
                            var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Validate_OTP }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                            sTemp_id = MessageContent.FirstOrDefault().SMS_TEMPLATE_ID;
                            sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, issueApplation.FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.EMAIL_ID, issueApplation.EMAIL_ID).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.PASSWORD, issueApplation.PASSWORD);
                            sTContentTamil = string.Empty;
                            var TamilMessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Validate_OTP_Tamil }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                            sTemp_idTamil = TamilMessageContent.FirstOrDefault().SMS_TEMPLATE_ID;
                            sTContentTamil = TamilMessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, issueApplation.FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.EMAIL_ID, issueApplation.EMAIL_ID).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.PASSWORD, issueApplation.PASSWORD);
                        }
                        else
                        {
                            sContent = @"Dear " + issueApplation.FIRST_NAME + ",\n\nSorry for the inconvenience,Failed to create account.\nFor any query use help lines:\n\nAdmission:0461-2320946\n\nEmail:admission@stmaryscollege.edu.in \n\nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.";

                            sTContentTamil = @"Dear " + issueApplation.FIRST_NAME + ",\n\nSorry for the inconvenience,Failed to create account.\nFor any query use help lines:\n\nAdmission:0461-2320946\n\nEmail:admission@stmaryscollege.edu.in \n\nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.";

                        }
                        SendEmailAndTextMessage(sContent, issueApplation.SMS_MOBILE_NO.Trim(), issueApplation.EMAIL_ID.Trim(), sTemp_id, "Account Details with St. Mary's College.", "0");
                        SendEmailAndTextMessage(sTContentTamil, issueApplation.SMS_MOBILE_NO.Trim(), issueApplation.EMAIL_ID.Trim(), sTemp_idTamil, "Account Details with St. Mary's College.", "1");
                    }

                }
            }

            return sResult.ToString().ToLower();
        }
        public ActionResult OnlineApplication()
        {
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            var academicYear = (List<AcademicYearModel>)CMSHandler.FetchData<AcademicYearModel>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchActiveYear)).DataSource.Data;
            Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] = academicYear.FirstOrDefault().AC_YEAR;
            Session[Common.SESSION_VARIABLES.ISSUE_ID] = null;
            //CommonMethods.SendMailFromGmailSMTP("hccadmission@stmaryscollege.edu.ac.in", "erp@admsn","expedite.paul@gmail.com","Test","Test Content");
            //admissionViewModel.shift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
            admissionViewModel.applicationType = (List<ADM_APPLICATION_TYPE>)CMSHandler.FetchData<ADM_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;


            return View(admissionViewModel);
        }
        public JsonResult FetchGenderByAppType(string sApptype)
        {
            string sOption = string.Empty;
            try
            {
                var ligender = (List<SupGender>)CMSHandler.FetchData<SupGender>(new SUP_APPLICATION_TYPE() { APPLICATION_TYPE_ID = sApptype }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchGenderByApptypeId)).DataSource.Data;
                if (ligender != null && ligender.Count > 0)
                {
                    foreach (var item in ligender)
                    {
                        sOption += "<option value=" + item.GENDER_ID + ">" + item.GENDER_NAME + "</option>";
                    }
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchGenderByAppType", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchGenderByAppType", ex.Message);
                }
            }
            return Json(sOption, JsonRequestBehavior.AllowGet);

        }

        public JsonResult FetchProgrammeByAppType(string sApptype)
        {
            string sOption = string.Empty;
            try
            {
                var liProgramme = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(new SUP_APPLICATION_TYPE() { APPLICATION_TYPE_ID = sApptype }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeByApptypeId)).DataSource.Data;
                if (liProgramme != null && liProgramme.Count > 0)
                {
                    foreach (var item in liProgramme)
                    {
                        sOption += "<option value=" + item.PROGRAMME_GROUP_ID + ">" + item.PROGRAMME_NAME + "</option>";
                    }
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchProgrammeByAppType", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchProgrammeByAppType", ex.Message);
                }
            }
            return Json(sOption, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ForgotApplicationNo()
        {

            return View();
        }

        public ActionResult GetForgotApplicationNo(string sSHC, string sDOB)
        {

            AdmissionViewModel admissionViewModel = new AdmissionViewModel();

            //  admissionViewModel.liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { HSC_NO = sSHC, DOB = CommonMethods.ConvertdatetoyearFromat(sDOB) }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchForgotApplicationNo), "2018").DataSource.Data;
            return View(admissionViewModel);
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult Help()
        {
            return View();
        }

        public ActionResult PayURepsponse(FormCollection payUResponse)
        {
            fee_payu_response feePayUResponse = new fee_payu_response();
            List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();

            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    if (payUResponse != null)
                    {
                        JsonResultData objResult = new JsonResultData();
                        string PayUResponse_Id = string.Empty;
                        string status = payUResponse["status"].ToString(); feePayUResponse = FormCollectionToClass(payUResponse);
                        objResult.oResult = feePayUResponse;
                        var isExisting = (List<fee_payu_response>)CMSHandler.FetchData<fee_payu_response>(feePayUResponse, FeeSQL.GetFeeSQL(FeeSQLCommands.IsPayUResponseExist), sAcademicYear).DataSource.Data;
                        if (isExisting != null && isExisting.Count > 0)
                        {
                            feePayUResponse.PAYU_RESPONSE_ID = isExisting.FirstOrDefault().PAYU_RESPONSE_ID;
                            var updateState = CMSHandler.SaveOrUpdate(feePayUResponse, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdatePayUResponse), sAcademicYear);
                            return View(feePayUResponse);
                        }
                        var result = CMSHandler.SaveOrUpdate(feePayUResponse, FeeSQL.GetFeeSQL(FeeSQLCommands.SavePayUResponse), sAcademicYear, true);
                        var lastInsertId = (List<fee_payu_response>)CMSHandler.FetchData<fee_payu_response>(new fee_payu_response() { txnid = feePayUResponse.txnid }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchPayUresponseByTxnid), sAcademicYear).DataSource.Data;
                        PayUResponse_Id = (lastInsertId != null && lastInsertId.Count > 0) ? lastInsertId.FirstOrDefault().PAYU_RESPONSE_ID : string.Empty;
                        if (status.Equals("success"))
                        {
                            if (payUResponse["udf2"].ToString().Equals(Common.FrequencyType.AdmissionApplicationFee))
                            {
                                feePayUResponse = ApplicationFeeCollection(feePayUResponse, payUResponse, PayUResponse_Id, sAcademicYear);
                                Session.Abandon();
                            }
                            else if (payUResponse["udf2"].ToString().Equals(Common.FrequencyType.AdmissionFee))
                            {
                                var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                                if (FetchFrequency != null && FetchFrequency.Count > 0)
                                {
                                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = payUResponse["udf1"].ToString(), FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfoByReceiveId), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                    if (studentAcountList != null && studentAcountList.Count > 0)
                                    {
                                        feePayUResponse = AdmissionFeeCollection(feePayUResponse, payUResponse, PayUResponse_Id, sAcademicYear);
                                        studentAcountList.FirstOrDefault().STATUS = feePayUResponse.status;
                                        studentAcountList.FirstOrDefault().RECEIPT_NO = feePayUResponse.RECEIPT_NO;

                                    }
                                }
                                // feePayUResponse = AdmissionFeeCollection(feePayUResponse, payUResponse, PayUResponse_Id, sAcademicYear);
                                // return RedirectToAction("AdmissionFeeStatus", feePayUResponse);
                                string sJson = JsonConvert.SerializeObject(studentAcountList);
                                Session[Common.SESSION_VARIABLES.PAYMENT_RESPONSE] = sJson;
                                return RedirectToAction("PaymentStatus");
                            }
                            else if (payUResponse["udf2"].ToString().Equals(Common.FrequencyType.HostelFee))
                            {

                                var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.HostelFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                                if (FetchFrequency != null && FetchFrequency.Count > 0)
                                {
                                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = payUResponse["udf1"].ToString(), FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfoByReceiveId), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                    if (studentAcountList != null && studentAcountList.Count > 0)
                                    {
                                        feePayUResponse = AdmissionFeeCollection(feePayUResponse, payUResponse, PayUResponse_Id, sAcademicYear);
                                        studentAcountList.FirstOrDefault().STATUS = feePayUResponse.status;
                                        studentAcountList.FirstOrDefault().RECEIPT_NO = feePayUResponse.RECEIPT_NO;

                                    }
                                }
                                // feePayUResponse = AdmissionFeeCollection(feePayUResponse, payUResponse, PayUResponse_Id, sAcademicYear);
                                // return RedirectToAction("AdmissionFeeStatus", feePayUResponse);
                                string sJson = JsonConvert.SerializeObject(studentAcountList);
                                Session[Common.SESSION_VARIABLES.PAYMENT_RESPONSE] = sJson;
                                return RedirectToAction("PaymentStatus");
                                // feePayUResponse = AdmissionFeeCollection(feePayUResponse, payUResponse, PayUResponse_Id, sAcademicYear);
                                //return RedirectToAction("AdmissionFeeStatus", feePayUResponse);
                            }
                            else
                            {
                                feePayUResponse.status = "Pending";
                                feePayUResponse.error_Message = "If Money is debited from your account, Please contact us with your Transaction ID";
                            }
                        }
                        else
                        {
                            feePayUResponse.status = "Failed";
                        }
                    }
                    else
                    {
                        feePayUResponse.status = "Failed";
                        feePayUResponse.error_Message = "Null response, If Money is debited from your account, Please contact us with your Transaction ID ";
                    }
                }
                else
                {
                    feePayUResponse.status = "Failed";
                    feePayUResponse.error_Message = "Session Expired,If Money is debited from your account, Please contact us with your Transaction ID ";
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    var LineNumber = new System.Diagnostics.StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                    objHandler.WriteError("Fee", "PayURepsponse", "Err MSg: " + ex.Message, "LineNumber:" + LineNumber);
                }
                feePayUResponse.error_Message = "Sorry for the trouble,Please note Transaction Id and enquire with admin !!";
                Session[Common.SESSION_VARIABLES.ISSUE_ID] = null;
                return View(feePayUResponse);

            }
            return View(feePayUResponse);
        }

        public ActionResult PaymentStatus()
        {
            List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();
            if (Session[Common.SESSION_VARIABLES.PAYMENT_RESPONSE] != null)
            {
                if (!string.IsNullOrEmpty(Session[Common.SESSION_VARIABLES.PAYMENT_RESPONSE].ToString()))
                {
                    studentAcountList = JsonConvert.DeserializeObject<List<FEE_STUDENT_ACCOUNT>>(Session[Common.SESSION_VARIABLES.PAYMENT_RESPONSE].ToString());
                }
            }
            return View(studentAcountList);
        }

        public ActionResult AdmissionFeeStatus(fee_payu_response feePayUResponse)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            feePayUResponse.liStuAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = feePayUResponse.txnid }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHeadByReceiveId), sAcademicYear).DataSource.Data;
            return View(feePayUResponse);
        }
        //this method will make entry for application fee
        private fee_payu_response ApplicationFeeCollection(fee_payu_response feePayUResponse, FormCollection payUResponse, string PayUResponse_Id, string sAcademicYear)
        {

            string[] merc_hash_vars_seq;
            string merc_hash_string = string.Empty;
            string merc_hash = string.Empty;
            string sStudentId = payUResponse["udf1"].ToString();
            string sFrequencyId = payUResponse["udf3"].ToString();
            string sApplicationNo = string.Empty;
            string sContent = string.Empty;
            string sUserAccountId = string.Empty;
            var FetchFeeStudentAccount = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { ISSUE_ID = Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
            var payUPaymentInfo = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { ACCOUNT_TYPE = Common.FrequencyType.AdmissionApplicationFee, API_TYPE = Common.PayUMoneyAPIsType.BaseUrl }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchMerchantInfo)).DataSource.Data;
            if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
            {
                if (payUPaymentInfo != null && payUPaymentInfo.Count > 0)
                {
                    merc_hash_vars_seq = payUPaymentInfo.FirstOrDefault().HASH_SEQUENCE.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    if (payUResponse.AllKeys.Contains("additionalCharges"))
                    {
                        merc_hash_string = payUResponse["additionalCharges"].ToString() + "|";
                    }
                    merc_hash_string += payUPaymentInfo.FirstOrDefault().SALT + "|" + payUResponse["status"].ToString();
                    foreach (var merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (payUResponse[merc_hash_var] != null ? payUResponse[merc_hash_var] : "");
                    }
                    merc_hash = CommonMethods.Generatehash512(merc_hash_string).ToLower();
                    if (merc_hash == payUResponse["hash"])
                    {
                        var applicationNo = CMSHandler.SaveOrUpdate(new ADM_APPLICATIONSERIALNO() { ACADEMIC_YEAR = sAcademicYear, Issue_Id = Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString(), DEPT_CODE = FetchFeeStudentAccount.FirstOrDefault().DEPARTMENT_CODE }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveApplicationNo), sAcademicYear, true);
                        if (applicationNo.Success && !string.IsNullOrEmpty(applicationNo.RowUniqueId.ToString()))
                        {
                            var issueApplation = FetchFeeStudentAccount.FirstOrDefault();
                            issueApplation.APPLICATION_NO = GetApplicationNo(applicationNo.RowUniqueId.ToString(), issueApplation.DEPARTMENT_CODE);
                            issueApplation.PAYU_RESPONSE_ID = PayUResponse_Id;
                            sApplicationNo = issueApplation.APPLICATION_NO;
                            feePayUResponse.PROGRAMME_NAME = issueApplation.PROGRAMME_NAME;
                            var update = CMSHandler.SaveOrUpdate(issueApplation, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatePaidStatusInIssueApplication), sAcademicYear);

                            var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Application_Fee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                            sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, issueApplation.FIRST_NAME).Replace(Common.Delimiter.AT + Common.APP_FEE, issueApplation.APP_FEE).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO, sApplicationNo);

                            //sContent = "Dear " + issueApplation.FIRST_NAME + ",\n\nGreetings,\nRs " + issueApplation.APP_FEE + " is credited successfully.\n\nPlease note your application no:" + sApplicationNo + ". \n \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.\nPh:0461-2320946. ";
                            SendEmailAndTextMessage(sContent, issueApplation.CONTACT_NO, issueApplation.EMAIL_ID, "Application No");
                            var isExistingUser = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(new USER_ACCOUNT_INFO() { USER_ID = issueApplation.ISSUE_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUserAccountByUserId), sAcademicYear).DataSource.Data;
                            sUserAccountId = (isExistingUser != null && isExistingUser.Count > 0) ? isExistingUser.FirstOrDefault().USER_ACCOUNT_ID : string.Empty;

                            update = CMSHandler.SaveOrUpdate(new USER_ACCOUNT_INFO() { USERNAME = issueApplation.APPLICATION_NO.Trim(), PASSWORD = CommonMethods.GetSha256FromString(issueApplation.DOB), USER_ID = issueApplation.ISSUE_ID, ROLE = "12", EMAIL = issueApplation.EMAIL_ID, MOBILE = issueApplation.CONTACT_NO, NAME = issueApplation.FIRST_NAME }, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(string.IsNullOrEmpty(sUserAccountId) ? UserAccountSQLCommands.SaveUserAccount : UserAccountSQLCommands.UpdateUserAccount), sAcademicYear);
                            if (update.Success)
                            {
                                sContent = string.Empty;
                                var MessageContents = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Application_Fee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                sContent = MessageContents.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, issueApplation.FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.DOB, issueApplation.DOB).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO, issueApplation.APPLICATION_NO);

                                //sContent = @"Dear " + issueApplation.FIRST_NAME + ",\n\nThank you for applying/verifying at St. Mary's College\nYou may now login by clicking the link below.\nhttp://admission.stmaryscollege.edu.in/account/index\n\nIf clicking the link doesn't work, you can copy and paste the link into your browser's address window, or retype it there,\n\n Use this Login credential\n\n Username:" + issueApplation.APPLICATION_NO +
                                //    "\n\nPassword:" + issueApplation.DOB + "\n\n For any query use help lines:\n \nAdmission:0461-2320946\nEmail:admission@stmaryscollege.edu.in \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.";
                            }
                            else
                            {
                                sContent = string.Empty;
                                sContent = @"Dear " + issueApplation.FIRST_NAME + ",\n\nSorry for the inconvenience,Failed to create account.\n\nYour Transaction Id and Application number as follows \nTransactionId:" + feePayUResponse.txnid + "\n ApplicationNo:" + sApplicationNo + " \nFor any query use help lines:\n\nAdmission:0461-2320946\n\nEmail:admission@stmaryscollege.edu.in \n\nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.";
                            }
                            SendEmailAndTextMessage(sContent, issueApplation.CONTACT_NO.Trim(), issueApplation.EMAIL_ID.Trim(), "Account Details with St. Mary's College.");
                            feePayUResponse.APPLICATION_NO = issueApplation.APPLICATION_NO;
                        }
                        else
                        {
                            sContent = @"Dear " + feePayUResponse.firstname + ",\n\nSorry for the inconvenience,Failed to create account(Missing Application No).\nYour Transaction Id and Application number as follows \nTransactionId:" + feePayUResponse.txnid + "\n ApplicationNo:" + sApplicationNo + " \nFor any query use help lines:\nOnline Payment:\nAdmission:0461-2320946\nEmail:admission@stmaryscollege.edu.in \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.";
                            SendEmailAndTextMessage(sContent, feePayUResponse.phone, feePayUResponse.email, "Account Details with St. Mary's College.");
                            feePayUResponse.status = "Pending";
                            feePayUResponse.error_Message = "If Money is debited from your account, Please contact us with your Transaction ID";
                        }
                    }
                    else
                    {
                        feePayUResponse.status = "Pending";
                        feePayUResponse.error_Message = "If Money is debited from your account, Please contact us with your Transaction ID";
                    }
                }
                else
                {
                    feePayUResponse.status = "Pending";
                    feePayUResponse.error_Message = "If Money is debited from your account, Please contact us with your Transaction ID";
                }
            }
            else
            {
                feePayUResponse.status = "Pending";
                feePayUResponse.error_Message = "If Money is debited from your account, Please contact us with your Transaction ID";
            }
            return feePayUResponse;
        }
        //this method will make entry for admission fee
        private fee_payu_response AdmissionFeeCollection(fee_payu_response feePayUResponse, FormCollection payUResponse, string PayUResponse_Id, string sAcademicYear)
        {
            string[] merc_hash_vars_seq;
            string merc_hash_string = string.Empty;
            string merc_hash = string.Empty;
            string sStudentId = payUResponse["udf1"].ToString();
            string sFrequencyId = payUResponse["udf3"].ToString();
            string sFrequencyType = payUResponse["udf2"].ToString();
            string sContent = string.Empty;


            var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { FREQUENCY_ID = sFrequencyId, STUDENT_ID = sStudentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
            var payUPaymentInfo = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { ACCOUNT_TYPE = Common.FrequencyType.AdmissionFee, API_TYPE = Common.PayUMoneyAPIsType.BaseUrl }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchMerchantInfo)).DataSource.Data;
            if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
            {
                if (payUPaymentInfo != null && payUPaymentInfo.Count > 0)
                {
                    merc_hash_vars_seq = payUPaymentInfo.FirstOrDefault().HASH_SEQUENCE.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    if (payUResponse.AllKeys.Contains("additionalCharges"))
                    {
                        merc_hash_string = payUResponse["additionalCharges"].ToString() + "|";
                    }
                    merc_hash_string += payUPaymentInfo.FirstOrDefault().SALT + "|" + payUResponse["status"].ToString();
                    foreach (var merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (payUResponse[merc_hash_var] != null ? payUResponse[merc_hash_var] : "");
                    }
                    merc_hash = CommonMethods.Generatehash512(merc_hash_string).ToLower();
                    string s = payUResponse["hash"];
                    if (merc_hash == payUResponse["hash"])
                    {
                        int ReceiptNo = 0;
                        string Prefix = "00";
                        string TransactionId = string.Empty;
                        FEE_STUDENT_ACCOUNT objStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                        FEE_TRANSACTION objFeeTransactionModel = new FEE_TRANSACTION();
                        AUTO_GENERATE_NUMBERS objAutoGenerateNumber = new AUTO_GENERATE_NUMBERS();
                        if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                        {
                            if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                            {
                                decimal balance = 0;
                                decimal.TryParse(FetchFeeStudentAccount.FirstOrDefault().BALANCE, out balance);
                                if (balance > 0)
                                {
                                    objFeeTransactionModel.STUDENT_ID = sStudentId;
                                    objFeeTransactionModel.CLASS = string.Empty;
                                    objFeeTransactionModel.FREQUENCY = sFrequencyId;
                                    objFeeTransactionModel.FREQUENCY_TO = sFrequencyId;
                                    objFeeTransactionModel.PAYMENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                                    objFeeTransactionModel.USERNAME = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                                    objFeeTransactionModel.COLLECTED = FetchFeeStudentAccount.Sum(o => Convert.ToDecimal(o.BALANCE)).ToString();
                                    var FetchReceiptNo = (List<AUTO_GENERATE_NUMBERS>)CMSHandler.FetchData<AUTO_GENERATE_NUMBERS>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAutoGenerateNumber)).DataSource.Data;
                                    if (FetchReceiptNo != null && FetchReceiptNo.Count > 0)
                                        ReceiptNo = Convert.ToInt32(FetchReceiptNo.FirstOrDefault().exam_receipt_no);
                                    else
                                    {
                                        // objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                        //   objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                    }
                                    objAutoGenerateNumber.exam_receipt_no = Convert.ToString(ReceiptNo + 1);
                                    objAutoGenerateNumber.exam_receipt_no = Prefix + objAutoGenerateNumber.exam_receipt_no;
                                    var UpdateReceiptNo = CMSHandler.SaveOrUpdate(objAutoGenerateNumber, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateAutoGenerateNumber));
                                    if (UpdateReceiptNo.Success)
                                    {
                                        objFeeTransactionModel.PAYMENT_MODE = Common.PaymentMode.Online;
                                        objFeeTransactionModel.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                        feePayUResponse.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                        objFeeTransactionModel.PayUResponse_Id = PayUResponse_Id;
                                        var SaveFeeTransaction = CMSHandler.SaveOrUpdate(objFeeTransactionModel, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeTransaction), sAcademicYear, true);
                                        if (SaveFeeTransaction.Success)
                                        {
                                            TransactionId = SaveFeeTransaction.RowUniqueId.ToString();
                                            //   feePayUResponse.txnid = TransactionId;
                                            decimal dAmount = FetchFeeStudentAccount.Sum(o => Convert.ToDecimal(o.BALANCE));
                                            decimal dCredit = 0;
                                            foreach (var item in FetchFeeStudentAccount)
                                            {
                                                if (dAmount > 0 && Convert.ToDecimal(item.BALANCE) > 0)
                                                {
                                                    dCredit = Convert.ToDecimal(item.CREDIT);
                                                    item.DEBIT = (dAmount >= dCredit) ? item.CREDIT : (dCredit - dAmount).ToString();
                                                    dAmount = dAmount - dCredit;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                                item.FREQUENCY = item.FREQUENCY_ID;
                                                item.TRANSACTION_ID = TransactionId;
                                                item.PAID_AMOUNT = item.DEBIT;
                                                item.RECEIPT_NO = objFeeTransactionModel.RECEIPT_NO;
                                                item.HEAD = item.HEAD_ID;

                                                var SaveFeeCollection = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeCollection));
                                                if (SaveFeeCollection.Success)
                                                {
                                                    var SaveStudentAccount = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeStudentAccount), sAcademicYear);
                                                    if (SaveStudentAccount.Success)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        //objResult.Message = Common.Messages.FailedToSaveRecords;
                                                    }
                                                }
                                            }
                                            if (sFrequencyType == Common.FrequencyType.AdmissionFee)
                                            {
                                                var MessageContents = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Admission_Fee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                                sContent = MessageContents.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, feePayUResponse.firstname).Replace(Common.Delimiter.AT + Common.txnid, feePayUResponse.txnid).Replace(Common.Delimiter.AT + Common.amount, feePayUResponse.amount);

                                                //sContent = @"Dear " + feePayUResponse.firstname + ",\n\nThank you for using online payment.Your transaction id is " + feePayUResponse.txnid + "\nRs." + feePayUResponse.amount + " is credited successfully. \n,"
                                                //                                             + "\n For any query use help lines:\n\nAdmission:0461-2320946\nEmail:admission@stmaryscollege.edu.in \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.";
                                                SendEmailAndTextMessage(sContent, feePayUResponse.phone.Trim(), feePayUResponse.email.Trim(), "Admission Fee Status From St. Mary's College,Thoothukudi.", "0");

                                                new MySQLHandler().ExecuteAsScripts(SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAdmissionFeeUpdates).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, sStudentId).Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear));
                                            }
                                            else if (sFrequencyType == Common.FrequencyType.HostelFee)
                                            {
                                                var MessageContents = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Hostel_Admission_Fee }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                                sContent = MessageContents.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, feePayUResponse.firstname).Replace(Common.Delimiter.AT + Common.txnid, feePayUResponse.txnid).Replace(Common.Delimiter.AT + Common.amount, feePayUResponse.amount);
                                                //sContent = @"Dear " + feePayUResponse.firstname + ",\n\nThank you for using online payment.Your transaction id is " + feePayUResponse.txnid + "\nRs." + feePayUResponse.amount + " is credited successfully. \n,"
                                                //                                             + "\n For any query use help lines:\n \nAdmission:0461-2320946\nEmail:admission@stmaryscollege.edu.in \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.";
                                                SendEmailAndTextMessage(sContent, feePayUResponse.phone.Trim(), feePayUResponse.email.Trim(), "Hostel Fee Status From St. Mary's College,Thoothukudi.", "0");

                                                new MySQLHandler().ExecuteAsScripts(SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.HostelFeeUpdates).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, sStudentId).Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear));
                                            }


                                            payUResponse.Clear();
                                        }
                                        else
                                        {
                                            // objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                            //  objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                        }
                                    }
                                }
                                else
                                {
                                    feePayUResponse.status = "Pending";
                                    feePayUResponse.error_Message = "Please note Transaction Id and enquire with admin !!";
                                }
                            }
                            else
                            {
                                feePayUResponse.status = "Pending";
                                feePayUResponse.error_Message = "Registration failed";
                            }

                        }
                        else
                        {
                            feePayUResponse.status = Common.ErrorCode.RequestTimeout;
                            feePayUResponse.error_Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                        }

                    }
                    else
                    {
                        feePayUResponse.status = "Pending";
                        feePayUResponse.error_Message = "Please note Transaction Id and enquire with admin !!";
                    }
                }
                else
                {
                    feePayUResponse.status = "Pending";
                    feePayUResponse.error_Message = "Please note Transaction Id and enquire with admin !!";
                }
            }
            else
            {
                feePayUResponse.status = "Pending";
                feePayUResponse.error_Message = "Please note Transaction Id and enquire with admin !!";
            }


            return feePayUResponse;
        }

        public ActionResult PayApplicationFee()
        {
            PayUPost objPayUPost = new PayUPost();
            if (Session[Common.SESSION_VARIABLES.ISSUE_ID] != null)
            {
                var objResult = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { ISSUE_ID = Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                if (objResult != null && objResult.Count > 0)
                {
                    var person = objResult.FirstOrDefault();
                    var payUPaymentInfo = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { ACCOUNT_TYPE = Common.FrequencyType.AdmissionApplicationFee, API_TYPE = Common.PayUMoneyAPIsType.BaseUrl }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchMerchantInfo)).DataSource.Data;
                    var objFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionApplicationFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    string sFrequencyId = (objFrequency != null && objFrequency.Count > 0) ? objFrequency.FirstOrDefault().FREQUENCY_ID : string.Empty;
                    if (payUPaymentInfo != null && payUPaymentInfo.Count > 0)
                    {
                        objPayUPost.productinfo = "Application Fee";
                        objPayUPost.key = payUPaymentInfo.FirstOrDefault().KEY;
                        objPayUPost.salt = payUPaymentInfo.FirstOrDefault().SALT;
                        objPayUPost.udf1 = person.ISSUE_ID;
                        objPayUPost.email = person.EMAIL_ID;
                        objPayUPost.udf5 = person.CONTACT_NO;
                        objPayUPost.firstname = person.FIRST_NAME;
                        objPayUPost.udf3 = sFrequencyId;
                        objPayUPost.udf4 = person.HSC_NO;
                        objPayUPost.udf2 = Common.FrequencyType.AdmissionApplicationFee;
                        objPayUPost.txnid = CommonMethods.CommonTransactionId(person.ISSUE_ID);
                        objPayUPost.amount = Convert.ToDecimal(person.APP_FEE).ToString("g29");
                        bool sLoop = true;
                        while (sLoop)
                        {
                            var fresult = (List<FEE_PAYU_REQUEST>)CMSHandler.FetchData<FEE_PAYU_REQUEST>(new FEE_PAYU_REQUEST() { txnid = objPayUPost.txnid }, FeeSQL.GetFeeSQL(FeeSQLCommands.IsPayURequestExist), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                            if (fresult != null && fresult.Count > 0)
                            {
                                objPayUPost.txnid = CommonMethods.CommonTransactionId(Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString());
                            }
                            else
                            {
                                var result = CMSHandler.SaveOrUpdate(objPayUPost, FeeSQL.GetFeeSQL(FeeSQLCommands.SavePayURequest), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
                                if (result.Success)
                                    sLoop = false;
                            }
                        }
                        objPayUPost.hash = GetHashValue(payUPaymentInfo.FirstOrDefault().HASH_SEQUENCE, payUPaymentInfo.FirstOrDefault().KEY, objPayUPost);
                        objPayUPost.surl = payUPaymentInfo.FirstOrDefault().surl;
                        objPayUPost.furl = payUPaymentInfo.FirstOrDefault().furl;
                        objPayUPost.curl = payUPaymentInfo.FirstOrDefault().curl;
                        objPayUPost.phone = person.CONTACT_NO;
                        objPayUPost.BASE_URL = payUPaymentInfo.FirstOrDefault().BASE_URL.Trim();
                        return View(objPayUPost);
                    }
                }
                else
                {
                    objPayUPost.sMessage = "Sorry for the inconvenience,Please Close your browser and register again. ";
                }
            }
            else
            {
                objPayUPost.sMessage = "Sorry for the inconvenience,Time out error , Please Close your browser and register again.";
            }

            return View(objPayUPost);
        }

        public ActionResult GetIssuedList(string from, string to)
        {

            from = CommonMethods.ConvertdatetoyearFromat(from);
            to = CommonMethods.ConvertdatetoyearFromat(to);
            AdmissionViewModel adminssionView = new AdmissionViewModel();
            // adminssionView.liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ISSUED_LIST() { FROM_DATE = from, TO_DATE = to }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssuedApplications)).DataSource.Data;

            return View(adminssionView);
        }

        public string ResendOTP(string sContactNo, string sEmailId, string sFirstName)
        {
            string sResult = string.Empty;
            if (Session[Common.SESSION_VARIABLES.OTP] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sOTP = string.Empty;
                sOTP = Session[Common.SESSION_VARIABLES.OTP].ToString();
                var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.OTP_Verification }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                string sOTPContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, sFirstName).Replace(Common.Delimiter.AT + Common.sOTP, sOTP);
                //string sOTPContent = "Dear " + sFirstName + ",\n\t Your verification code is " + sOTP + ".\n By \n Admission Team,\n St. Mary's College.";
                SendEmailAndTextMessage(sOTPContent, sContactNo, sEmailId, MessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Verification from St. Mary's College,Thoothukudi.", "0");

                // TAMIL
                var TamilMessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.OTP_VerificationTamil }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                string sOTPContentTamil = TamilMessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, sFirstName).Replace(Common.Delimiter.AT + Common.sOTP, sOTP);
                //string sOTPContent = "Dear " + sFirstName + ",\n\t Your verification code is " + sOTP + ".\n By \n Admission Team,\n St. Mary's College.";
                SendEmailAndTextMessage(sOTPContentTamil, sContactNo, sEmailId, TamilMessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Verification from St. Mary's College,Thoothukudi.", "1");
                sResult = "Your verification code is sent";
            }
            else
            {
                sResult = "Session out,Register Again";
            }

            return sResult;
        }
        public string SendOTP(string sJson)
        {
            string sResult = string.Empty;
            string sOTP = string.Empty;
            string sOTPContent = string.Empty;
            string sOTPContentTamil = string.Empty;
            var issueApplation = JsonConvert.DeserializeObject<ADM_ISSUE_APPLICATION_2018>(sJson);
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var result = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018 { EMAIL_ID = issueApplation.EMAIL_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.IsExistEmailId)).DataSource.Data;
            // var isExisting = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { PROGRAMME_GROUP_ID = issueApplation.PROGRAMME_GROUP_ID, HSC_NO = issueApplation.HSC_NO }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.IsExistingUser)).DataSource.Data;
            var IsHscnoExisting = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { HSC_NO = issueApplation.HSC_NO }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.IsHscNoExist)).DataSource.Data;
            if (result != null && result.Count > 0 && result.FirstOrDefault().STATUS != "0" && IsHscnoExisting != null && IsHscnoExisting.Count > 0 && IsHscnoExisting.FirstOrDefault().STATUS != "0")
            {
                return sResult = "This E-Mail id and HscNo is existing already.";
            }
            else
            {
                if (result != null && result.Count > 0 && result.FirstOrDefault().STATUS != "0")
                {
                    return sResult = "This E-Mail id is existing already.";
                }
                if (IsHscnoExisting != null && IsHscnoExisting.Count > 0 && IsHscnoExisting.FirstOrDefault().STATUS != "0")
                {
                    return sResult = "This HscNo is existing already.";
                }
            }

            if (result != null && result.Count > 0)
            {
                if (result.FirstOrDefault().STATUS == "1")
                {
                    if (string.Equals(Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString(), result.FirstOrDefault().ISSUE_ID))
                    {
                        return sResult;
                    }
                }

                if (Session[Common.SESSION_VARIABLES.OTP] != null)
                    sOTP = Session[Common.SESSION_VARIABLES.OTP].ToString();
                else
                {
                    sOTP = CommonMethods.GenerateOTP();
                    Session[Common.SESSION_VARIABLES.OTP] = sOTP;
                }
                var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.OTP_Verification }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                sOTPContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, issueApplation.FIRST_NAME).Replace(Common.Delimiter.AT + Common.sOTP, sOTP);
                SendEmailAndTextMessage(sOTPContent, issueApplation.CONTACT_NO, issueApplation.EMAIL_ID, MessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Verification from St. Mary's College,Thoothukudi.", "0");

                var MessageContentTamil = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.OTP_VerificationTamil }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                sOTPContentTamil = MessageContentTamil.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, issueApplation.FIRST_NAME + "  ").Replace(Common.Delimiter.AT + Common.sOTP, sOTP);
                SendEmailAndTextMessage(sOTPContentTamil, issueApplation.CONTACT_NO, issueApplation.EMAIL_ID, MessageContentTamil.FirstOrDefault().SMS_TEMPLATE_ID, "Verification from St. Mary's College,Thoothukudi.", "1");
            }
            else
            {
                sResult = "Something went wrong try again!!!";
            }
            return sResult;
        }
        public string FetchCoursesForAdmission(string sApplicationType, string sShift)
        {
            string sResult = string.Empty;
            ADM_APPTYPE_PROG_GROUPS applicatonGroup = new ADM_APPTYPE_PROG_GROUPS();
            string SsQL = string.Empty;
            SsQL = CMS.SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApplicationTypeAndShift).Replace(Common.Delimiter.QUS + Common.SUP_APPTYPE_PROG_GROUPS.SHIFT, sShift);
            SsQL = SsQL.Replace(Common.Delimiter.QUS + Common.SUP_APPTYPE_PROG_GROUPS.APPTYPE_ID, sApplicationType);
            var Courses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SsQL, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty).DataSource.Data;
            if (Courses != null && Courses.Count > 0)
            {
                foreach (var item in Courses)
                {
                    sResult += "<option value='" + item.PROGRAMME_GROUP_ID + "'>" + item.PROGRAMME_NAME + "</option>";
                }
            }
            return sResult;
        }
        public string FetchAdmissionProgramme(string sApplicationType, string sProgrammeMode, string sShift)
        {
            string sResult = string.Empty;
            string sSQL = string.Empty;
            if (!string.IsNullOrEmpty(sApplicationType) && !string.IsNullOrEmpty(sProgrammeMode) && !string.IsNullOrEmpty(sShift))
            {
                ADM_APPTYPE_PROG_GROUPS applicatonGroup = new ADM_APPTYPE_PROG_GROUPS();
                sSQL = CMS.SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionProgrammeByApplicationTypeAndShiftAndProgrammemode).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.APPTYPE_ID, sApplicationType);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.PROGRAMME_MODE, sProgrammeMode);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.SHIFT, sShift);
                var Courses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(applicatonGroup, sSQL).DataSource.Data;
                if (Courses != null && Courses.Count > 0)
                {
                    foreach (var item in Courses)
                    {
                        sResult += "<option value='" + item.PROGRAMME_GROUP_ID + "'>" + item.PROGRAMME_NAME + "</option>";
                    }
                }
            }
            else
            {
                sResult += "<option value='0'>--Select--</option>";
            }
            return sResult;
        }
        public bool SaveOrUpdateForIssueApplicaiton(string sJson)
        {
            bool bResult = false;
            string sLastInsertedId = string.Empty;
            string sOTP = string.Empty;
            var issueApplation = JsonConvert.DeserializeObject<ADM_ISSUE_APPLICATION_2018>(sJson);
            issueApplation.DOB = CommonMethods.ConvertdatetoyearFromat(issueApplation.DOB);

            sLastInsertedId = CMSHandler.SaveOrUpdate(issueApplation, CMS.SQL.Admission.AdmissionSQL.GetAdmissionSQL((Session[Common.SESSION_VARIABLES.ISSUE_ID] == null) ? AdmissionSQLCommands.SaveIssueApplication : AdmissionSQLCommands.UpdateIssueApplication), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString(), true).RowUniqueId.ToString();

            if (!string.IsNullOrEmpty(sLastInsertedId))
            {
                Session[Common.SESSION_VARIABLES.ISSUE_ID] = sLastInsertedId;
            }

            return bResult;
        }

        public JsonResult FetchApplicantInfo()
        {
            JsonResultData result = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ISSUE_ID] != null)
            {
                var objResult = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { ISSUE_ID = Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                if (objResult != null && objResult.Count > 0)
                {
                    var person = objResult.FirstOrDefault();
                    result.oResult = person;
                    result.ErrorCode = Common.ErrorCode.Ok;
                }
                else
                {
                    result.ErrorCode = Common.ErrorCode.FailedDependency;
                    result.Message = Common.ErrorMessage.FailedDependency;
                }
            }
            else
            {
                result.ErrorCode = Common.ErrorCode.RequestTimeout;
                result.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private JsonResultData ConstructPayURequest(ADM_ISSUE_APPLICATION_2018 issueApplication)
        {
            JsonResultData oResult = new JsonResultData();
            PayUPost objPayUPost = new PayUPost();
            var payUPaymentInfo = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { ACCOUNT_TYPE = Common.FrequencyType.AdmissionApplicationFee, API_TYPE = Common.PayUMoneyAPIsType.BaseUrl }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchMerchantInfo)).DataSource.Data;

            if (payUPaymentInfo != null && payUPaymentInfo.Count > 0)
            {
                objPayUPost.productinfo = "Application Fee";
                objPayUPost.key = payUPaymentInfo.FirstOrDefault().KEY;
                objPayUPost.salt = payUPaymentInfo.FirstOrDefault().SALT;
                objPayUPost.udf1 = payUPaymentInfo.FirstOrDefault().STUDENT_ID;
                objPayUPost.email = issueApplication.EMAIL_ID.Trim();
                objPayUPost.udf5 = string.Empty;
                objPayUPost.firstname = issueApplication.FIRST_NAME;
                objPayUPost.udf3 = string.Empty;
                objPayUPost.udf4 = string.Empty;
                objPayUPost.udf2 = string.Empty;
                objPayUPost.txnid = CommonMethods.CommonTransactionId(Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString());
                objPayUPost.phone = issueApplication.CONTACT_NO.Trim();
                objPayUPost.amount = Convert.ToDecimal(Convert.ToDecimal(issueApplication.APP_FEE)).ToString("g29");
                objPayUPost.hash = GetHashValue(payUPaymentInfo.FirstOrDefault().HASH_SEQUENCE, payUPaymentInfo.FirstOrDefault().KEY, objPayUPost);

                bool sLoop = true;
                while (sLoop)
                {
                    var fresult = (List<FEE_PAYU_REQUEST>)CMSHandler.FetchData<FEE_PAYU_REQUEST>(new FEE_PAYU_REQUEST() { txnid = objPayUPost.txnid }, FeeSQL.GetFeeSQL(FeeSQLCommands.IsPayURequestExist)).DataSource.Data;
                    if (fresult != null && fresult.Count > 0)
                    {
                        objPayUPost.txnid = CommonMethods.CommonTransactionId(Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString());
                    }
                    else
                    {
                        var result = CMSHandler.SaveOrUpdate(objPayUPost, FeeSQL.GetFeeSQL(FeeSQLCommands.SavePayURequest));
                        if (result.Success)
                        {
                            oResult.oResult = objPayUPost;
                            oResult.ErrorCode = Common.ErrorCode.Ok;
                        }
                        else
                        {
                            oResult.oResult = objPayUPost;
                            oResult.ErrorCode = Common.ErrorCode.FailedDependency;
                            oResult.Message = Common.Messages.FailedToLoadTryAgain;
                        }
                        sLoop = false;
                    }
                }

            }
            else
            {
                oResult.ErrorCode = Common.ErrorCode.FailedDependency;
                oResult.Message = Common.Messages.FailedToLoadTryAgain;
            }
            return oResult;

        }

        private string GetHashValue(string hashSeq, string sMerchant_key, PayUPost objPost)
        {
            string hash_string = string.Empty;
            string hash = string.Empty;
            string[] hashVarsSeq = hashSeq.Split('|'); // spliting hash sequence from config       
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = hash_string + sMerchant_key;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txnid")
                {
                    hash_string = hash_string + objPost.txnid;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "amount")
                {
                    hash_string = hash_string + Convert.ToDecimal(objPost.amount).ToString("g29");
                    hash_string = hash_string + '|';
                }
                else
                {
                    Type t = objPost.GetType();
                    hash_string = hash_string + (objPost.GetType().GetProperty(hash_var).GetValue(objPost, null) != null ? objPost.GetType().GetProperty(hash_var).GetValue(objPost, null) : "");// isset if else
                    hash_string = hash_string + '|';
                }
            }

            hash_string += objPost.salt;// appending SALT

            hash = CommonMethods.Generatehash512(hash_string).ToLower();         //generating hash
            return hash;
        }

        private fee_payu_response FormCollectionToClass(FormCollection collection)
        {

            fee_payu_response obj = new fee_payu_response();
            foreach (var key in collection.AllKeys)
            {
                Type t = obj.GetType();
                System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(obj, collection[key], null);
                }

            }
            return obj;
        }

        private string GetApplicationNo(string sLastInsertedId, string sPrefix)
        {
            string sResult = string.Empty;
            Int16 number = 0;
            if (string.IsNullOrEmpty(sLastInsertedId))
            {
                return sResult = string.Concat(sPrefix, new Random().Next(5000, 9999).ToString());
            }
            else
            {
                number = Convert.ToInt16(sLastInsertedId);
                if (number <= 9 && number >= 0)
                {
                    sResult = string.Concat(sPrefix, "000", number);
                }
                else if (number >= 10 && number <= 99)
                {
                    sResult = string.Concat(sPrefix, "00", number);
                }
                else if (number >= 100 && number <= 999)
                {
                    sResult = string.Concat(sPrefix, "0", number);
                }
                else
                {
                    sResult = string.Concat(sPrefix, number);
                }
            }
            return sResult;
        }

        public JsonResultData FeeRefundRequest()
        {
            JsonResultData objResult = new JsonResultData();
            string sHash = string.Empty;
            var APIs = (List<FEE_MERCHANT_ACCOUNT_INFO>)CMSHandler.FetchData<FEE_MERCHANT_ACCOUNT_INFO>(new FEE_MERCHANT_ACCOUNT_INFO() { ACCOUNT_TYPE = Common.FrequencyType.AdmissionApplicationFee, API_TYPE = Common.PayUMoneyAPIsType.RefundURL }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchPayUAPI)).DataSource.Data;
            if (APIs != null && APIs.Count > 0)
            {

                //403993715517626168
                string TransactionId = CommonMethods.CommonTransactionId("1000");
                sHash = CommonMethods.GetHashValueForRefund(APIs.FirstOrDefault().HASH_SEQUENCE, APIs.FirstOrDefault().KEY, TransactionId, "6927335727", "100.00", APIs.FirstOrDefault().curl, APIs.FirstOrDefault().SALT);
                var value = new Dictionary<string, string>
                                              {
                                                 { "key", APIs.FirstOrDefault().KEY },
                                                 { "command", APIs.FirstOrDefault().curl },
                                                { "hash", sHash },
                                               { "var1", "6927335727" },
                                                { "var2",TransactionId  },
                                                 { "var3","100.00"  }
                                             };
                var apiResponse = CommonMethods.HttpPostMethod(APIs.FirstOrDefault().BASE_URL, value);
            }
            return objResult;
        }

        private void SendEmailAndTextMessage(string sContent, string sToNumber = "", string sToEmail = "", string sTemplateId = "", string sSubTitle = "", string isTamil = "")
        {
            if (!string.IsNullOrEmpty(sContent))
            {
                List<MessageResult.sms> liSMS = new List<MessageResult.sms>();
                List<MessageResult.listsms> libulkSMS = new List<MessageResult.listsms>();
                List<SMS_SETTING> liSMSSetting = new List<SMS_SETTING>();
                string sFromEmail = ConfigurationManager.AppSettings["AdmissionFromMail"].ToString();
                string sPasswordEmail = ConfigurationManager.AppSettings["AdmissionPassword"].ToString();
                string sEmailOTPContent = string.Empty;
                string sEmailContent = string.Empty;
                //Fetch Active SMS Vendor
                var livendordetails = (List<SMS_VENDOR_DETAILS>)CMSHandler.FetchData<SMS_VENDOR_DETAILS>(null, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchActiveSMSvendors)).DataSource.Data;
                liSMSSetting = (List<SMS_SETTING>)CMSHandler.FetchData<SMS_SETTING>(new SMS_SETTING() { API_METHOD = Common.API_METHOD.API_1 }, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchDovesoftSettings)).DataSource.Data;
                if (!string.IsNullOrEmpty(sToNumber))
                {
                    //---------------SMS------------
                    CommunicationController communication = new CommunicationController();
                    if (livendordetails != null && livendordetails.Count > 0)
                    {
                        liSMS.Add(new MessageResult.sms() { to = sToNumber, message = sContent, custom = "0", custom1 = "0", custom2 = "0" });
                        if (liSMSSetting != null && liSMSSetting.Count > 0)
                        {
                            libulkSMS.Add(new MessageResult.listsms() { mobiles = sToNumber, sms = sContent, senderid = liSMSSetting.FirstOrDefault().SENDER, clientSMSID = "1947692308", accountusagetypeid = "1", tempid = sTemplateId });
                        }

                        if (livendordetails.FirstOrDefault().SMS_VENDOR_ID == Common.sms_vendor.Solutioninfini)
                        {
                            communication.sSmsMode = Common.MessageType.OTP;
                            int length = Convert.ToInt32(sContent.Length);

                            string sCount = CommonMethods.ValidateCreditCount(length);
                            if (sCount == "0")
                                sCount = "1";
                            communication.sCreditCount = sCount;
                            communication.liSMS = liSMS;
                            communication.sSmsCount = 1;
                            communication.SendBulkSMSFromSolutionInfi();
                        }
                        if (livendordetails.FirstOrDefault().SMS_VENDOR_ID == Common.sms_vendor.DoveSoft)
                        {
                            communication.sTextMessage = sContent;
                            communication.sTemplateid = sTemplateId;
                            communication.sMobilenos = string.Join(",", liSMS.Select(o => o.to));
                            //communication.listbulkSMS = libulkSMS;
                            //communication.SendBulkSMSFromDovesoft1();
                            communication.SendBulkSMSFromDovesoft(isTamil);

                        }
                    }

                }
                //-----------end sms--------------
                //-----------Email----------------   
                if (!string.IsNullOrEmpty(sToEmail))
                {
                    // CommonMethods.SendMailFromGmailSMTP(sFromEmail, sPasswordEmail, sToEmail, sSubTitle, sContent);
                }
                //----------Ending Email---------
            }

        }

        #region Selection Process
        [UserRights("STAFF")]
        public ActionResult ListReceivedApplication()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objStaffModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeInchargeByStaffId), sAcademicYear).DataSource.Data;
                    var CasteList = (List<SupCommunity>)CMSHandler.FetchData<SupCommunity>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCommunity)).DataSource.Data;
                    var MainCommunityList = (List<MainCommunity>)CMSHandler.FetchData<MainCommunity>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMainCommunity)).DataSource.Data;
                    var ApplicantTypeList = (List<SUP_APPLICANT_TYPE>)CMSHandler.FetchData<SUP_APPLICANT_TYPE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchApplicantType)).DataSource.Data;
                    var SubjectList = (List<SUP_HSS_SUBJECTS>)CMSHandler.FetchData<SUP_HSS_SUBJECTS>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHssSubjects)).DataSource.Data;

                    if (CycleList != null && CycleList.Count > 0)
                        objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    if (MainCommunityList != null && MainCommunityList.Count > 0)
                        objViewModel.CasteList = new SelectList(MainCommunityList, Common.SUP_MAIN_COMMUNITY.MAIN_COMMUNITY_ID, Common.SUP_MAIN_COMMUNITY.MAIN_COMMUNITY);
                    if (ApplicantTypeList != null && ApplicantTypeList.Count > 0)
                        objViewModel.ApplicantTypeList = new SelectList(ApplicantTypeList, Common.SUP_APPLICANT_TYPE.APPLICANT_TYPE_ID, Common.SUP_APPLICANT_TYPE.APPLICANT_TYPE);
                    if (SubjectList != null && SubjectList.Count > 0)
                        objViewModel.Subjects = new SelectList(SubjectList, Common.SUP_HSS_SUBJECTS.HSS_SUBJECT_ID, Common.SUP_HSS_SUBJECTS.HSS_SUBJECT);

                    //Selection Setting Process
                    var Lidateexpiredselectedapplicant = new List<ADM_SELECTION_PROCESS>();
                    var liselectionsetting = (List<ADM_SELECTION_SETTING>)CMSHandler.FetchData<ADM_SELECTION_SETTING>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectionSettings), sAcademicYear).DataSource.Data;
                    if (liselectionsetting != null && liselectionsetting.Count > 0)
                    {
                        foreach (var item in liselectionsetting)
                        {
                            var liapplicant = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new ADM_SELECTION_SETTING() { INTERVAL_DAY = item.INTERVAL_DAY, STATUS = Common.ADM_STATUS.ADMITTED }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchDateExpiredSelectedApplicant), sAcademicYear).DataSource.Data;
                            if (item.IS_AUTO_CANCEL == Common.IsActiveFalg.IsActive)
                            {
                                if (liapplicant != null && liapplicant.Count > 0)
                                {
                                    foreach (var applicant in liapplicant)
                                    {
                                        applicant.STATUS = Common.STATUS.Pending;
                                        var Result = CMSHandler.SaveOrUpdate(applicant, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusofDateExpiredSelectedApplicant));
                                        var Resultarg = CMSHandler.SaveOrUpdate(applicant, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateAdmissionCancelinSelectionprocess), sAcademicYear);
                                    }
                                }
                            }
                            else
                            {
                                if (liapplicant != null && liapplicant.Count > 0)
                                {
                                    Lidateexpiredselectedapplicant = Lidateexpiredselectedapplicant.Concat(liapplicant).ToList();
                                }
                            }
                        }
                    }
                    objViewModel.LiDateExpiredSelectedApplicant = Lidateexpiredselectedapplicant;
                    ViewBag.SelectedApplicant = Lidateexpiredselectedapplicant;

                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListReceivedApplication", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListReceivedApplication", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindReceivedApplicantList(string sProgrammeId, string sApplicantTypeId, string sCasteId, string sIs_Minority, string sHostel, string sFilterJson)
            {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            List<SUP_HOSTEL> LiHostel = new List<SUP_HOSTEL>();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    //string[] applicantTypeList = sApplicantTypeId.Split(',');
                    //if(applicantTypeList.Count() >= 1) {
                    //    objModel.RELIGION_ID = "";
                    //    foreach (string applicant in applicantTypeList)
                    //    {
                    //        if (applicant == Common.IsActiveFalg.IsActive)
                    //            objModel.RELIGION_ID = Common.IsActiveFalg.IsActive;
                    //        else
                    //        {
                    //            if (objModel.RELIGION_ID != "")
                    //                objModel.RELIGION_ID = objModel.RELIGION_ID + "," + "2,3,4";
                    //             else
                    //                objModel.RELIGION_ID = "2,3,4";
                    //        }      
                    //    }
                    //}

                    if (sApplicantTypeId == Common.IsActiveFalg.IsActive)
                    {
                        objModel.RELIGION_ID = Common.IsActiveFalg.IsActive;
                        objModel.IS_ROMAN_CATHOLIC = Common.IsActiveFalg.IsActive;
                    }

                    else if (sApplicantTypeId == "2")
                    {
                        objModel.RELIGION_ID = "1,3";
                        objModel.IS_ROMAN_CATHOLIC = Common.IsActiveFalg.IsNotActive;
                    }
                    else
                    {
                        objModel.RELIGION_ID = "1,2,3,4";
                        objModel.IS_ROMAN_CATHOLIC = Common.IsActiveFalg.IsNotActive + Common.Delimiter.COMMA + Common.IsActiveFalg.IsActive;
                    }


                    //if (sIs_Minority != "0")
                    //{
                    //    objModel.IS_ROMAN_CATHOLIC = Common.IsActiveFalg.IsNotActive + Common.Delimiter.COMMA + Common.IsActiveFalg.IsActive;
                    //}

                    objModel.STATUS = Common.STATUS.Submitted;
                    //sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionReceivedApplicationByApplicantTypeIdAndProgrammeIdAndCasteId);
                    // TO FETCH ENGLISH MARK AS CUTOFF FOR ENGLISH STUDENTS
                    if (sProgrammeId == "15" || sProgrammeId == "16")
                    {
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionReceivedApplicationForEnglish);
                    }
                    else
                    {
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionReceivedApplication);
                    }

                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RELIGION_ID, objModel.RELIGION_ID);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_COMMUNITY.COMMUNITYID, sCasteId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.IS_ROMAN_CATHOLIC, objModel.IS_ROMAN_CATHOLIC);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.HOSTEL_FACILITY, sHostel);

                    objViewModel.liSelectionProccess = (List<SELECTIONPROCCESS>)CMSHandler.FetchData<SELECTIONPROCCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    sSQL = String.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeEligibilityByProgrammeId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liProgrammeEligibility = (List<ADM_PROG_ELIGIBILITY>)CMSHandler.FetchData<ADM_PROG_ELIGIBILITY>(null, sSQL, sAcademicYear).DataSource.Data;
                    objViewModel.liMonth = (List<SELECTIONPROCCESS>)CMSHandler.FetchData<SELECTIONPROCCESS>(null, "SELECT text1 AS APPLICATION_NO, TEXT2 AS RECEIVE_ID FROM demo_test_table_1;").DataSource.Data;
                    if (objViewModel.liSelectionProccess != null && objViewModel.liSelectionProccess.Count > 0)
                    {
                        var ActualList = objViewModel.liSelectionProccess.Select(l => l.RECEIVE_ID).ToArray();

                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectionProcessByHSCNo);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, string.Join(",", ActualList));
                        var SelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(null, sSQL, sAcademicYear).DataSource.Data;
                        if (SelectionProcess != null && SelectionProcess.Count > 0)
                        {
                            foreach (var item in SelectionProcess)
                            {
                                objViewModel.liSelectionProccess.Remove(objViewModel.liSelectionProccess.SingleOrDefault(S => S.RECEIVE_ID == item.RECEIVE_ID && S.PROGRAMME_MODE == item.PROGRAMME_MODE));
                            }
                        }
                        if (objViewModel.liSelectionProccess != null && objViewModel.liSelectionProccess.Count > 0)
                        {
                            var ReceiveIdList = objViewModel.liSelectionProccess.Select(l => l.RECEIVE_ID).ToArray();
                            var SelectedReceiveIds = string.Join("','", ReceiveIdList);
                            SelectedReceiveIds = "'" + SelectedReceiveIds + "'";
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuSubMarksByReceivedId);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_STU_SUBMARKS.RECEIVE_STUID, SelectedReceiveIds);
                            objViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(null, sSQL, sAcademicYear).DataSource.Data;
                            var liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(null, sSQL, sAcademicYear).DataSource.Data;
                            var groupbyreceiveid = liADMStudentMarks.GroupBy(o => o.RECEIVE_STUID).ToList();
                            if (!string.IsNullOrEmpty(sFilterJson))
                            {
                                var FilterInfo = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sFilterJson);
                                int count = 0;
                                string Ineligibleapplicants = string.Empty;
                                if (FilterInfo.SAVE_ADM_SUB_STU_MARKS != null && FilterInfo.SAVE_ADM_SUB_STU_MARKS.Count > 0)
                                {
                                    foreach (var student in groupbyreceiveid)
                                    {
                                        var studentmarks = liADMStudentMarks.Where(o => o.RECEIVE_STUID == student.FirstOrDefault().RECEIVE_STUID).ToList();
                                        if (studentmarks != null && studentmarks.Count > 0)
                                        {
                                            foreach (var marks in studentmarks)
                                            {
                                                foreach (var item in FilterInfo.SAVE_ADM_SUB_STU_MARKS)
                                                {
                                                    if (item.HSS_SUBJECT_ID == marks.SUBJECT_ID && Convert.ToInt32(marks.MARK) >= Convert.ToInt32(item.MIN_MARK) && Convert.ToInt32(marks.MARK) <= Convert.ToInt32(item.MAX_MARK))
                                                    {
                                                        count++;
                                                    }
                                                }
                                            }
                                            if (count != Convert.ToInt32(FilterInfo.SAVE_ADM_SUB_STU_MARKS.Count))
                                            {

                                                Ineligibleapplicants += student.FirstOrDefault().RECEIVE_STUID + ",";

                                            }
                                        }
                                        count = 0;
                                    }
                                }
                                var IneligibleReceiveIds = Ineligibleapplicants.Split(',');
                                if (IneligibleReceiveIds != null)
                                {
                                    foreach (var value in IneligibleReceiveIds)
                                    {
                                        objViewModel.liSelectionProccess.RemoveAll(o => o.RECEIVE_ID == value);
                                    }
                                }
                            }
                            var SelectedList = objViewModel.liSelectionProccess.Select(l => l.RECEIVE_ID).ToArray();
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCoursesAndProgrammeModeAppliedByHSCNo);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, string.Join(",", SelectedList));
                            objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, sSQL, sAcademicYear).DataSource.Data;
                        }



                    }
                    // CASETEWISE_QUOTA ....
                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramWiseQuata).Replace(Common.Delimiter.QUS + Common.SUP_COMMUNITY.COMMUNITYID, sCasteId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liProgram_Quata = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;

                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCasteWiseQuata).Replace(Common.Delimiter.QUS + Common.SUP_COMMUNITY.COMMUNITYID, sCasteId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liCasteWise_Quata = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;

                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchMinorityWise).Replace(Common.Delimiter.QUS + Common.SUP_COMMUNITY.COMMUNITYID, sCasteId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liMinorityWise = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;

                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchMinorityWiseAdmitted).Replace(Common.Delimiter.QUS + Common.SUP_COMMUNITY.COMMUNITYID, sCasteId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liMinorityWiseAdmitted = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;

                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCasteWiseAdmitted).Replace(Common.Delimiter.QUS + Common.SUP_COMMUNITY.COMMUNITYID, sCasteId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liCasteWiseAdmitted = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;
                    objViewModel.liMainCommunity = (List<MainCommunity>)CMSHandler.FetchData<MainCommunity>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMainCommunity)).DataSource.Data;

                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOcWise).Replace(Common.Delimiter.QUS + Common.SUP_COMMUNITY.COMMUNITYID, sCasteId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liOccategoryWise = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;

                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOcWiseAdmitted).Replace(Common.Delimiter.QUS + Common.SUP_COMMUNITY.COMMUNITYID, sCasteId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liOccategoryWiseAdmitted = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;
                    //Fetch Hostel Total str,selected,paid

                    var HostelDetails = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllHostelDetails)).DataSource.Data;
                    objModel.STATUS = Common.STATUS.Selected;
                    var HostelSelected = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllHostelSelected), sAcademicYear).DataSource.Data;
                    objModel.STATUS = Common.STATUS.Admitted;
                    var HostelAdmitted = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllHostelAdmitted), sAcademicYear).DataSource.Data;
                    if (HostelDetails != null && HostelDetails.Count > 0)
                    {
                        foreach (var item in HostelDetails)
                        {
                            var admitted = HostelAdmitted.Where(o => o.HOSTEL_ID == item.HOSTEL_ID).ToList();
                            var selected = HostelSelected.Where(o => o.HOSTEL_ID == item.HOSTEL_ID).ToList();
                            if (admitted != null && admitted.Count > 0 && selected != null && selected.Count > 0)
                            {
                                LiHostel.Add(new SUP_HOSTEL() { HOSTEL_NAME = item.HOSTEL_NAME, TOTAL_STRENGTH = item.TOTAL_STRENGTH, ADMITTED = admitted.FirstOrDefault().ADMITTED, SELECTED = selected.FirstOrDefault().SELECTED });
                            }
                        }
                        objViewModel.liHostel = LiHostel;
                    }


                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindReceivedApplicantList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindReceivedApplicantList", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult SaveOrUpdateSelectionWaitingProcess(string sSelectionProcess)
        {
            JsonResultData objResult = new JsonResultData();
            string sContent = string.Empty;
            int success = 0, fail = 0;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JSON_ADM_STU_SUBMARKS objJson = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sSelectionProcess);
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    foreach (var item in objJson.SAVE_SELECTION_PROCESS)
                    {

                        item.SELECTED_BY = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                        item.STATUS = Common.STATUS.Submitted;
                        var SaveSelectionWaitingProcess = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveSelectionWaitingProcess), sAcademicYear);
                        if (SaveSelectionWaitingProcess.Success)
                        {
                            success++;
                        }
                        else
                            fail++;


                    }
                    objResult.Message = string.Format("{0} Records Saved Successfully{1}{2} Failed To SaveRecords ", success, Environment.NewLine, fail);
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateSelectionWaitingProcess", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateSelectionWaitingProcess", ex.Message);
                }
            }

            return Json(objResult);
        }

        public ActionResult ListSelectedApplication()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objStaffModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeInchargeByStaffId), sAcademicYear).DataSource.Data;

                    if (CycleList != null && CycleList.Count > 0)
                        objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);

                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListReceivedApplication", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListReceivedApplication", ex.Message);
                }
            }

            return View(objViewModel);
        }
        public JsonResult SaveOrUpdateSelectionProcess(string sSelectionProcess)
        {
            JsonResultData objResult = new JsonResultData();
            string sContent = string.Empty;
            int success = 0, fail = 0;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JSON_ADM_STU_SUBMARKS objJson = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sSelectionProcess);
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    foreach (var item in objJson.SAVE_SELECTION_PROCESS)
                    {

                        item.SELECTED_BY = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                        var SaveSelectionProcess = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveSelectionProcess), sAcademicYear);
                        if (SaveSelectionProcess.Success)
                        {
                            new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                            var UpdateStatus = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = item.RECEIVE_ID, STATUS = Common.STATUS.Selected, PROGRAMME_ID = item.PROGRAMME_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusByReceiveId));
                            new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                            if (UpdateStatus.Success)
                            {
                                var StudentAccount = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = item.RECEIVE_ID, PROGRAMME_ID = item.PROGRAMME_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                if (StudentAccount != null && StudentAccount.Count > 0)
                                {

                                    var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.College_Selection }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                    sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_NAME, StudentAccount.FirstOrDefault().PROGRAMME_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO, StudentAccount.FirstOrDefault().APPLICATION_NO);

                                    //sContent = "Dear " + StudentAccount.FirstOrDefault().FIRST_NAME + ",\n\nCongratulations,\n You are selected for " + StudentAccount.FirstOrDefault().PROGRAMME_NAME + " and your  Application No is  " + StudentAccount.FirstOrDefault().APPLICATION_NO + ".\n Please go for certificate verification to concern department at St. Mary's College with in two days. After the verfication you are requested to pay fee only through online fee collection. No offline fee collection. \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.\nPh:0461-2320946. ";
                                    ////need to use bulk sms api. and remove email.
                                   //  SendEmailAndTextMessage(sContent, StudentAccount.FirstOrDefault().CONTACT_NO, string.Empty, MessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Congratulations ,You are selected for " + StudentAccount.FirstOrDefault().PROGRAMME_NAME + ".");
                                }

                            }

                            success++;
                        }
                        else
                            fail++;


                    }
                    objResult.Message = string.Format("{0} Records Saved Successfully{1}{2} Failed To SaveRecords ", success, Environment.NewLine, fail);
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateSelectionProcess", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateSelectionProcess", ex.Message);
                }
            }
            return Json(objResult);
        }

        public JsonResult SaveSelectionProcess(string sSelectionProcess)
        {
            JsonResultData objResult = new JsonResultData();
            string sContent = string.Empty;
            string sTContent = string.Empty;
            int success = 0, fail = 0;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JSON_ADM_STU_SUBMARKS objJson = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sSelectionProcess);
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    foreach (var item in objJson.SAVE_SELECTION_PROCESS)
                    {
                        var SaveSelectionProcess = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveSelectionProcess), sAcademicYear);
                        if (SaveSelectionProcess.Success)
                        {
                            new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                            var UpdateStatusInselectedApp = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = item.RECEIVE_ID, STATUS = Common.STATUS.Selected, PROGRAMME_ID = item.PROGRAMME_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateSelectedStatusByReceiveId), sAcademicYear);
                            if (UpdateStatusInselectedApp.Success)
                            {
                                var UpdateStatus = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = item.RECEIVE_ID, STATUS = Common.STATUS.Selected, PROGRAMME_ID = item.PROGRAMME_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusByReceiveId));
                                new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                if (UpdateStatus.Success)
                                {
                                    var StudentAccount = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = item.RECEIVE_ID, PROGRAMME_ID = item.PROGRAMME_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                    if (StudentAccount != null && StudentAccount.Count > 0)
                                    {
                                        // get verification date for offline verification by programme and selection_cycle

                                       // var liDate= (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() {PROGRAMME_ID = item.PROGRAMME_ID ,SELECTION_CYCLE=item.SELECTION_CYCLE}, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetVerificationDateByProgrammeAndCycle), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;

                                       // var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.College_Selection }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                       // //var tMessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Selection_Tamil }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;

                                       // if (liDate != null && liDate.Count > 0)
                                       // {
                                       //     sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_NAME, StudentAccount.FirstOrDefault().PROGRAMME_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO, StudentAccount.FirstOrDefault().APPLICATION_NO).Replace(Common.Delimiter.AT+ Common.ADM_ISSUE_APPLICATION_2018.VERIFICATION_DATE,liDate.FirstOrDefault().DAY);
                                       //    // sTContent = tMessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_NAME, StudentAccount.FirstOrDefault().PROGRAMME_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO, StudentAccount.FirstOrDefault().APPLICATION_NO).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.VERIFICATION_DATE, liDate.FirstOrDefault().DAY);

                                       // }
                                       // else
                                       // { 
                                       //     sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_NAME, StudentAccount.FirstOrDefault().PROGRAMME_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO, StudentAccount.FirstOrDefault().APPLICATION_NO).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.VERIFICATION_DATE, DateTime.Now.AddDays(1).ToString("dd-MM-yyyy"));
                                       //    // sTContent = tMessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_NAME, StudentAccount.FirstOrDefault().PROGRAMME_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO, StudentAccount.FirstOrDefault().APPLICATION_NO).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.VERIFICATION_DATE, DateTime.Now.AddDays(1).ToString("dd-MM-yyyy"));
                                       // }
                                          
                                       // // replace verification date on msg
                                       // SendEmailAndTextMessage(sContent, StudentAccount.FirstOrDefault().CONTACT_NO, string.Empty, MessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Congratulations ,You are selected for " + StudentAccount.FirstOrDefault().PROGRAMME_NAME + ".","0");
                                       //// SendEmailAndTextMessage(sTContent, StudentAccount.FirstOrDefault().CONTACT_NO, string.Empty, tMessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Congratulations ,You are selected for " + StudentAccount.FirstOrDefault().PROGRAMME_NAME + ".", "1");

                                        success++;
                                    }
                                    else
                                    {
                                        fail++;
                                    }

                                }

                            }
                        }
                      
                    }
                    objResult.Message = string.Format("{0} Records Saved Successfully{1}{2} Failed To SaveRecords ", success, Environment.NewLine, fail);
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateSelectionProcess", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateSelectionProcess", ex.Message);
                }
            }
            return Json(objResult);
        }


        public JsonResult SaveDateExpiredSelectedApplicant(string sJsonData)
        {
            JsonResultData ObjResult = new JsonResultData();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();

            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sJsonData))
                {
                    var liapplicant = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sJsonData);
                    if (liapplicant.SAVE_SELECTION_PROCESS != null)
                    {
                        foreach (var item in liapplicant.SAVE_SELECTION_PROCESS)
                        {
                            item.STATUS = Common.STATUS.Pending;
                            var Result = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusofDateExpiredSelectedApplicant));
                            var Resultarg = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateAdmissionCancelinSelectionprocess), sAcademicYear);
                            if (Result.Success && Resultarg.Success)
                            {
                                ObjResult.ErrorCode = Common.ErrorCode.Ok;
                                ObjResult.Message = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                ObjResult.Message = Common.Messages.RecordsSavedSuccessfully;

                            }
                        }
                    }

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
        //Releasing Admission Hold Process
        public ActionResult ReleasingAdmissionHold()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = string.Empty;
                try
                {
                    if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                    {
                        sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;

                        if (CycleList != null && CycleList.Count > 0)
                            objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);

                    }
                    else
                        return RedirectToAction("ErrorMessage", "error");
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "ReleasingAdmissionHold", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "ReleasingAdmissionHold", ex.Message);
                    }
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindAdmissionHoldedApplicant(string sCycleId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            string sSQL = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                try
                {
                    if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                    {
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionHoldedApplicantByCycleId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.SELECTION_CYCLE, sCycleId);
                        objViewModel.LiDateExpiredSelectedApplicant = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new ADM_SELECTION_PROCESS() { STATUS = Common.ADM_STATUS.PENDING }, sSQL, sAcademicyear).DataSource.Data;

                    }
                    else
                        return RedirectToAction("ErrorMessage", "error");
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindAdmissionHoldedApplicant", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindAdmissionHoldedApplicant", ex.Message);
                    }
                }
            }
            return View(objViewModel);
        }
        public JsonResult SaveReleasingAdmissionHolded(string sJsonData)
        {
            int success = 0, fail = 0;
            ResultArgs Resultargs = new ResultArgs();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JSON_ADM_STU_SUBMARKS objJson = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sJsonData);
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.SupFrequencyType.AdmissionFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                    foreach (var item in objJson.SAVE_SELECTION_PROCESS)
                    {
                        //Check Is Admission Fee Created

                        var CheckIsAccountExist = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STRUCTURE() { FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID, RECEIVE_ID = item.RECEIVE_ID, PROGRAMME = item.PROGRAMME }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.CheckIsStudentAccountCreated), sAcademicYear).DataSource.Data;
                        if (CheckIsAccountExist != null && CheckIsAccountExist.Count > 0)
                        {
                            Resultargs = CMSHandler.SaveOrUpdate(new ADM_ISSUED_APPLICATIONS() { ISSUED_ID = item.ISSUED_ID, STATUS = Common.STATUS.Verified }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateAdmissionHoldedStatusByIssuedId), sAcademicYear);

                        }
                        else
                        {
                            Resultargs = CMSHandler.SaveOrUpdate(new ADM_ISSUED_APPLICATIONS() { ISSUED_ID = item.ISSUED_ID, STATUS = Common.STATUS.Selected }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateAdmissionHoldedStatusByIssuedId), sAcademicYear);
                        }
                        if (Resultargs.Success)
                            success++;
                        else
                            fail++;

                    }
                    ObjJsonData.Message = string.Format("{0} Records Saved Successfully !{1} {2} Failed to SaveRecords...!", success, Environment.NewLine, fail);
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveAdmissionCancel", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveAdmissionCancel", ex.Message);
                }
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        //Admission Cancel
        public ActionResult AdmissionCancel()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {

                string sAcademicYear = string.Empty;
                try
                {
                    if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                    {

                        objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                        sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                        var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objStaffModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeInchargeByStaffId), sAcademicYear).DataSource.Data;
                        var CasteList = (List<SupCommunity>)CMSHandler.FetchData<SupCommunity>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCommunity)).DataSource.Data;
                        var MainCommunityList = (List<MainCommunity>)CMSHandler.FetchData<MainCommunity>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMainCommunity)).DataSource.Data;
                        var ApplicantTypeList = (List<SUP_APPLICANT_TYPE>)CMSHandler.FetchData<SUP_APPLICANT_TYPE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchApplicantType)).DataSource.Data;
                        if (CycleList != null && CycleList.Count > 0)
                            objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                        if (ProgrammeList != null && ProgrammeList.Count > 0)
                            objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                        if (MainCommunityList != null && MainCommunityList.Count > 0)
                            objViewModel.CasteList = new SelectList(MainCommunityList, Common.SUP_MAIN_COMMUNITY.MAIN_COMMUNITY_ID, Common.SUP_MAIN_COMMUNITY.MAIN_COMMUNITY);
                        if (ApplicantTypeList != null && ApplicantTypeList.Count > 0)
                            objViewModel.ApplicantTypeList = new SelectList(ApplicantTypeList, Common.SUP_APPLICANT_TYPE.APPLICANT_TYPE_ID, Common.SUP_APPLICANT_TYPE.APPLICANT_TYPE);


                    }
                    else
                        return RedirectToAction("ErrorMessage", "error");
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "AdmissionCancel", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "AdmissionCancel", ex.Message);
                    }
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindSelectedApplicantList(string sCycleId, string sProgrammeId, string sApplicantTypeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            string sSQL = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicyear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                try
                {
                    if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                    {
                        objModel.SELECTED_BY = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                        objModel.SELECTION_CYCLE = sCycleId;
                        objModel.APPLICATION_TYPE_ID = sApplicantTypeId;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectedApplicantByProgrammeandApplicatType);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liSelectionProccess = (List<SELECTIONPROCCESS>)CMSHandler.FetchData<SELECTIONPROCCESS>(objModel, sSQL, sAcademicyear).DataSource.Data;
                        if (objViewModel.liSelectionProccess != null && objViewModel.liSelectionProccess.Count > 0)
                        {
                            var ReceiveIdList = objViewModel.liSelectionProccess.Select(l => l.RECEIVE_ID).ToArray();
                            var SelectedReceiveIds = string.Join("','", ReceiveIdList);
                            SelectedReceiveIds = "'" + SelectedReceiveIds + "'";
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuSubMarksByReceivedId);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_STU_SUBMARKS.RECEIVE_STUID, SelectedReceiveIds);
                            objViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(null, sSQL, sAcademicyear).DataSource.Data;
                            var SelectedList = objViewModel.liSelectionProccess.Select(l => l.RECEIVE_ID).ToArray();
                            //var SelectedHSCNo = string.Join("','", SelectedList);
                            //SelectedHSCNo = "'" + SelectedHSCNo + "'";
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCoursesAppliedByHSCNo);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, string.Join(",", SelectedList));
                            objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, sSQL, sAcademicyear).DataSource.Data;
                        }
                    }
                    else
                        return RedirectToAction("ErrorMessage", "error");
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "AdmissionCancel", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "AdmissionCancel", ex.Message);
                    }
                }
            }
            return View(objViewModel);
        }
        public JsonResult SaveAdmissionCancelProcess(string sCancelProcess)
        {

            string sContent = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JSON_ADM_STU_SUBMARKS objJson = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sCancelProcess);
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    foreach (var item in objJson.SAVE_SELECTION_PROCESS)
                    {

                        var StudentAccount = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = item.RECEIVE_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                        if (StudentAccount != null && StudentAccount.Count > 0)
                        {
                            var AdmissionCancel = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UdateDeleteByRecieveId), sAcademicYear);
                            if (AdmissionCancel.Success)
                            {
                                //sContent = "Dear " + StudentAccount.FirstOrDefault().FIRST_NAME + ",\n\nCongratulations,\n You are selected for " + StudentAccount.FirstOrDefault().PROGRAMME_NAME + " and your  Application No is  " + StudentAccount.FirstOrDefault().APPLICATION_NO + ".\n Please go for certificate verification to concern department at St. Mary's College after the verfication you are requested to pay fee within two days or else your admission will be cancelled. \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.\nPh:0461-2320946. ";
                                ////need to use bulk sms api. and remove email.
                                //SendEmailAndTextMessage(sContent, StudentAccount.FirstOrDefault().CONTACT_NO, string.Empty, "Congratulations ,You are selected for " + StudentAccount.FirstOrDefault().PROGRAMME_NAME + ".");
                            }
                            ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                            ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveAdmissionCancel", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveAdmissionCancel", ex.Message);
                }
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        //Verification
        [UserRights("STAFF")]
        public ActionResult ListSelectionProcess()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objStaffModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeInchargeByStaffId), sAcademicYear).DataSource.Data;
                    if (CycleList != null && CycleList.Count > 0)
                        objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListSelectionProcess", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListSelectionProcess", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindSelectionProcessApplicantList(string sCycleId, string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.SELECTION_CYCLE = sCycleId;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectionProcessByProgrammeIdAndCycle);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessApplicantList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessApplicantList", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult ViewApplicantDetails(string id, string sProgrammeId, string sVerified)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.RECEIVE_ID = id;

                    var List = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionIssuededApplicationById), sAcademicYear).DataSource.Data;

                    objViewModel.liApplicant = List.Where(x => x.PROGRAMME_GROUP_ID == sProgrammeId).ToList();
                    var ListProgramme = new List<SELECTIONPROCCESS>();
                    ListProgramme.Add(new SELECTIONPROCCESS() { PROGRAMME_ID = sProgrammeId, IS_VERIFIED = sVerified });
                    objViewModel.liSelectionProccess = ListProgramme;
                    var ListDocuments = (List<ADM_DOCUMENTS_UPLOADED>)CMSHandler.FetchData<ADM_DOCUMENTS_UPLOADED>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupDocuments), sAcademicYear).DataSource.Data;
                    if (ListDocuments != null)
                    {
                        objViewModel.Documents = new SelectList(ListDocuments, Common.ADM_DOCUMENTS_UPLOADED.DOCUMENT_ID, Common.ADM_DOCUMENTS_UPLOADED.DOCUMENT_NAME);

                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ViewApplicantDetails", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ViewApplicantDetails", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult FetchUploadedFilesByReceiveId(string sReceiveId)
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_UPLOADED_FILES_2018 objFileModel = new ADM_UPLOADED_FILES_2018();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objFileModel.RECEIVE_STU_ID = sReceiveId;
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    admissionViewModel.liUploadedFile = (List<ADM_UPLOADED_FILES_2018>)CMSHandler.FetchData<ADM_UPLOADED_FILES_2018>(objFileModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUploadedImagesById), sAcademicYear).DataSource.Data;
                    if (admissionViewModel.liUploadedFile != null && admissionViewModel.liUploadedFile.Count > 0)
                    {
                        admissionViewModel.PHOTO_PATH = admissionViewModel.liUploadedFile.FirstOrDefault().PHOTO_PATH;
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchUploadedFilesByReceiveId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchUploadedFilesByReceiveId", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            return View(admissionViewModel);
        }
        public ActionResult FetchSubejectMarksByReceiveId(string sReceiveId, string sApplicationTypeId)
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objIssueModel = new ADM_ISSUE_APPLICATION_2018();
            ADM_STU_SUBMARKS objModel = new ADM_STU_SUBMARKS();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.RECEIVE_STUID = sReceiveId;
                    objModel.ACADEMIC_YEAR = sAcademicYear;
                    admissionViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHssSubjectsByReceiveId), sAcademicYear).DataSource.Data;
                    if (sApplicationTypeId == "1")
                    {
                        admissionViewModel.liEleventhMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchElevathMark), sAcademicYear).DataSource.Data;
                    }
                    return View(admissionViewModel);
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchSubejectMarksByReceiveId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchSubejectMarksByReceiveId", ex.Message);
                    return View(admissionViewModel);
                }
            }
        }
        public JsonResult UpdateVerificationIdByReceiveId(string sReceiveId, string sProgrammeId, string DocumentId)
        {
            JsonResultData ObjResult = new JsonResultData();
            ResultArgs Resultargs = new ResultArgs();
            List<FEE_FREQUENCY_FEE_MODE> FetchFrequency = new List<FEE_FREQUENCY_FEE_MODE>();
            string sContent = string.Empty;
            string sTContent = string.Empty;
            string sResult = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (!string.IsNullOrEmpty(sReceiveId))
                {
                    var objModel = new ADM_SELECTION_PROCESS();
                    objModel.RECEIVE_ID = sReceiveId;
                    objModel.PROGRAMME_GROUP_ID = sProgrammeId;
                    objModel.VERIFIED_BY = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var FetchSelectionId = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetSelectionIdByReceiveId), sAcademicYear).DataSource.Data;
                    if (FetchSelectionId != null && FetchSelectionId.Count > 0)
                        objModel.SELECTION_ID = FetchSelectionId.FirstOrDefault().SELECTION_ID;
                    var sresultArgs = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateVerificationByReceiveId), sAcademicYear);
                    //var FetchIssueId = (List<SELECTIONPROCCESS>)CMSHandler.FetchData<SELECTIONPROCCESS>(SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueIdByReceiveIdandProgrammeId)).DataSource.Data;
                    objModel.RECEIVE_ID = sReceiveId;
                    var document = DocumentId.Split(',');
                    foreach (var item in document)
                    {
                        objModel.DOCUMENT_ID = item;
                        sresultArgs = CMSHandler.SaveOrUpdate(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.InsertUploadedDocuments), sAcademicYear);
                    }
                    if (sresultArgs.Success)
                    {
                        new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                        var UpdateStatus = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = sReceiveId, STATUS = Common.STATUS.Verified, PROGRAMME_ID = sProgrammeId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusByReceiveId));
                        new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                        if (UpdateStatus.Success)
                        {
                            // fetch programme mode by programme id 
                            var FetchProgrammeMode = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(new CP_PROGRAMME_GROUP() { PROGRAMME_GROUP_ID = sProgrammeId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetProgammeMode), sAcademicYear).DataSource.Data;
                            if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                            {
                                // seperate ssc and regular frequency
                                if (FetchProgrammeMode.FirstOrDefault().PROGRAMME_MODE == "1")
                                {
                                    FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                                        objModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                                    var FetchStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = objModel.RECEIVE_ID, FREQUENCY_ID = objModel.FREQUENCY_ID }, FeeSQL.GetFeeSQL(FeeSQLCommands.CheckIsExistingAccount), sAcademicYear).DataSource.Data;
                                    if (FetchStudentAccount == null)
                                    {
                                        SaveAdmissionFeeStudentAccount(objModel.RECEIVE_ID, objModel.FREQUENCY_ID, objModel.PROGRAMME_GROUP_ID);
                                        var StudentAccount = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { PROGRAMME_ID = sProgrammeId, RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                        if (StudentAccount != null && StudentAccount.Count > 0)
                                        {
                                            var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Verification }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                            sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT+Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_NAME,StudentAccount.FirstOrDefault().PROGRAMME_NAME).Replace(Common.Delimiter.AT + Common.day, DateTime.Now.AddDays(3).ToString("dd-MM-yyyy"));

                                            var TMessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Verification_Tamil }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                            sTContent = TMessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_NAME, StudentAccount.FirstOrDefault().PROGRAMME_NAME).Replace(Common.Delimiter.AT + Common.day, DateTime.Now.AddDays(3).ToString("dd-MM-yyyy"));

                                            //sContent = "Dear " + StudentAccount.FirstOrDefault().FIRST_NAME + ",\n\nCongratulations,\n Your certificates are verified successfully. \n Please make online payment to confirm your admission. Your are requested pay fee with in " + DateTime.Now.AddDays(2).ToShortDateString() + " or else your admission will be cancelled. \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.\nPh:0461-2320946. ";
                                            SendEmailAndTextMessage(sContent, StudentAccount.FirstOrDefault().CONTACT_NO, StudentAccount.FirstOrDefault().EMAIL_ID, MessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Please make online payment to confirm your admission. ","0");

                                            SendEmailAndTextMessage(sTContent, StudentAccount.FirstOrDefault().CONTACT_NO, StudentAccount.FirstOrDefault().EMAIL_ID, TMessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Please make online payment to confirm your admission. ","1");

                                        }
                                        ObjResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                    }
                                    else
                                    {
                                        ObjResult.Message = "Account is existing already !!";
                                    }
                                }
                                else
                                {
                                    FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFeeSSC }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                                        objModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                                    var FetchStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = objModel.RECEIVE_ID, FREQUENCY_ID = objModel.FREQUENCY_ID }, FeeSQL.GetFeeSQL(FeeSQLCommands.CheckIsExistingAccount), sAcademicYear).DataSource.Data;
                                    if (FetchStudentAccount == null)
                                    {
                                        SaveAdmissionFeeStudentAccount(objModel.RECEIVE_ID, objModel.FREQUENCY_ID, objModel.PROGRAMME_GROUP_ID);
                                        var StudentAccount = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { PROGRAMME_ID = sProgrammeId, RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                                        if (StudentAccount != null && StudentAccount.Count > 0)
                                        {
                                            var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Verification }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                            sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.day, DateTime.Now.AddDays(3).ToShortDateString());

                                            //sContent = "Dear " + StudentAccount.FirstOrDefault().FIRST_NAME + ",\n\nCongratulations,\n Your certificates are verified successfully. \n Please make online payment to confirm your admission. Your are requested pay fee with in " + DateTime.Now.AddDays(2).ToShortDateString() + " or else your admission will be cancelled. \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.\nPh:0461-2320946. ";
                                            SendEmailAndTextMessage(sContent, StudentAccount.FirstOrDefault().CONTACT_NO, StudentAccount.FirstOrDefault().EMAIL_ID, MessageContent.FirstOrDefault().SMS_TEMPLATE_ID, "Please make online payment to confirm your admission. ");
                                        }
                                        ObjResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                    }
                                    else
                                    {
                                        ObjResult.Message = "Account is existing already !!";
                                    }
                                }



                                // CHECK HOSTEL
                                var FetchReceivedApplication = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetHostelFacilityByReceiveId), sAcademicYear).DataSource.Data;
                                if (FetchReceivedApplication.FirstOrDefault().HOSTEL_FACILITY == "1")
                                {
                                    var Hostelapplicationfees = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new sup_frequency_type() { FREQUENCY_TYPE_ID = Common.FrequencyType.HostelApplication }, DashboardSQL.GetDashboardSQL(DashboardSQLCommands.FeeHostelApplicationfee),sAcademicYear).DataSource.Data;
                                    //fEES iNSERT
                                    if (Hostelapplicationfees != null)
                                    {
                                        foreach (var item in Hostelapplicationfees)
                                        {
                                            item.STUDENT_ID = objModel.RECEIVE_ID;
                                            Resultargs = CMSHandler.SaveOrUpdate(item, DashboardSQL.GetDashboardSQL(DashboardSQLCommands.SaveHostelApplicationfee));
                                        }
                                    }

                                    ObjResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    ObjResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                }
                            }
                        }
                        else
                            ObjResult.Message = Common.Messages.FailedToSaveRecords;
                    }
                    else
                        ObjResult.Message = Common.Messages.FailedToSaveRecords;
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

        public JsonResult UpdateCancelinVerfication(string sReceiveId, string sProgrammeId, string sVerified)
        {
            JsonResultData ObjResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (!string.IsNullOrEmpty(sReceiveId))
                {
                    var objModel = new ADM_SELECTION_PROCESS();
                    objModel.RECEIVE_ID = sReceiveId;
                    objModel.VERIFIED_BY = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var FetchSelectionId = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetSelectionIdByReceiveId), sAcademicYear).DataSource.Data;
                    if (FetchSelectionId != null && FetchSelectionId.Count > 0)
                        objModel.SELECTION_ID = FetchSelectionId.FirstOrDefault().SELECTION_ID;
                    var sresultArgs = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateNotInterestBySelectionId), sAcademicYear);

                    if (sresultArgs.Success)
                    {
                        if (!string.IsNullOrEmpty(sVerified) && sVerified == "1")
                        {
                            var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                            if (FetchFrequency != null && FetchFrequency.Count > 0)
                                objModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                            new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                            var DeleteFee = CMSHandler.SaveOrUpdate(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeStudentAccountByReceiveIdandFrequencyId));
                            new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                        }
                        new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                        var cancelSelectedApp = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { PROGRAMME_ID = sProgrammeId, RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.CancelSelectionByReceiveIdandProgrammeId), sAcademicYear);
                        var UpdateStatus = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = sReceiveId, STATUS = Common.STATUS.Cancelled, PROGRAMME_ID = sProgrammeId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusByReceiveId));
                        new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                        if (UpdateStatus.Success)
                        {
                            ObjResult.ErrorCode = Common.ErrorCode.Ok;
                            ObjResult.Message = Common.Messages.FailedToSaveRecords;
                        }
                        else
                        {
                            ObjResult.ErrorCode = Common.ErrorCode.NotAcceptable;
                            ObjResult.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                    else
                        ObjResult.ErrorCode = Common.ErrorCode.NotAcceptable;
                    ObjResult.Message = Common.Messages.FailedToSaveRecords;
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

        public string SaveStudentAccount(string ReceiveId, string FrequencyId)
        {
            string sResult;
            ResultArgs sresultArgs = new ResultArgs();
            try
            {
                StuPersonalInfo student = new StuPersonalInfo();
                FEE_STRUCTURE ObjFeeStructure = new FEE_STRUCTURE();
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                student.TYPE = Common.SupFrequencyType.AdmissionFee;
                student.RECEIVE_ID = ReceiveId;
                student.FREQUENCY = FrequencyId;
                var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;

                var StudentInfo = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = ReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationByReceivedId), sAcademicYear).DataSource.Data;
                sresultArgs = CMSHandler.SaveOrUpdate(student, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SelectInsertForStudentAccountByReceiveIdAndFrequencyId), sAcademicYear);

                if (FetchFrequency != null && FetchFrequency.Count > 0 && FetchFrequency.FirstOrDefault().FREQUENCY_ID.Equals(FrequencyId))
                {
                    var FetchOtherFees = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(new StuPersonalInfo() { RECEIVE_ID = ReceiveId, FREQUENCY = FrequencyId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOtherFeeForInsert), sAcademicYear).DataSource.Data;
                    if (StudentInfo != null && StudentInfo.Count() > 0 && FetchOtherFees != null && FetchOtherFees.Count > 0)
                    {
                        if (StudentInfo.FirstOrDefault().APPLICATION_TYPE.ToLower().Equals("pg") || StudentInfo.FirstOrDefault().APPLICATION_TYPE.ToLower().Equals("mca") || StudentInfo.FirstOrDefault().APPLICATION_TYPE.ToLower().Equals("mca later entry") || StudentInfo.FirstOrDefault().APPLICATION_TYPE.ToLower().Equals("m.phil"))
                        {
                            if (!string.IsNullOrEmpty(StudentInfo.FirstOrDefault().UNIVERSITY) && StudentInfo.FirstOrDefault().UNIVERSITY.ToUpper().Equals("OTHERS"))
                            {
                                var select = FetchOtherFees.Where(s => s.HEAD == "83");
                                ObjFeeStructure.HEAD = select.FirstOrDefault().HEAD;
                                ObjFeeStructure.AMOUNT = select.FirstOrDefault().AMOUNT;
                                ObjFeeStructure.STUDENT_ID = ReceiveId;
                                ObjFeeStructure.FREQUENCY = FrequencyId;
                                ObjFeeStructure.FEE_MAIN_HEAD_ID = select.FirstOrDefault().FEE_MAIN_HEAD_ID;
                                ObjFeeStructure.FEE_STRUCTURE_ID = select.FirstOrDefault().FEE_STRUCTURE_ID; ;
                                ObjFeeStructure.FEE_ROOT_ID = select.FirstOrDefault().FEE_ROOT_ID;
                                sresultArgs = CMSHandler.SaveOrUpdate(ObjFeeStructure, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertOtherFeeForSpecialStudents), sAcademicYear);
                            }
                        }
                        else
                        {
                            var select = FetchOtherFees.Where(s => s.HEAD == "81");
                            ObjFeeStructure.HEAD = select.FirstOrDefault().HEAD;
                            ObjFeeStructure.AMOUNT = select.FirstOrDefault().AMOUNT;
                            ObjFeeStructure.STUDENT_ID = ReceiveId;
                            ObjFeeStructure.FREQUENCY = FrequencyId;
                            ObjFeeStructure.FEE_MAIN_HEAD_ID = select.FirstOrDefault().FEE_MAIN_HEAD_ID;
                            ObjFeeStructure.FEE_STRUCTURE_ID = select.FirstOrDefault().FEE_STRUCTURE_ID; ;
                            ObjFeeStructure.FEE_ROOT_ID = select.FirstOrDefault().FEE_ROOT_ID;
                            if (!string.IsNullOrEmpty(StudentInfo.FirstOrDefault().EDUCATION_BOARD_ID) && (StudentInfo.FirstOrDefault().EDUCATION_BOARD_ID) != "1")
                            {
                                sresultArgs = CMSHandler.SaveOrUpdate(ObjFeeStructure, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertOtherFeeForSpecialStudents), sAcademicYear);
                            }
                        }
                    }
                }
                if (sresultArgs.Success)

                    sResult = Common.Messages.RecordsSavedSuccessfully;
                else
                    sResult = Common.Messages.FailedToSaveRecords;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveStudentAccount", "Err MSg: " + ex.Message, "Query is empty!");
                }
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return ReceiveId;
        }



        public string SaveAdmissionFeeStudentAccount(string ReceiveId, string FrequencyId, string ProgrammeID)
        {
            string sResult;
            ResultArgs sresultArgs = new ResultArgs();
            try
            {
                StuPersonalInfo student = new StuPersonalInfo();
                FEE_STRUCTURE ObjFeeStructure = new FEE_STRUCTURE();
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                student.TYPE = Common.SupFrequencyType.AdmissionFee;
                student.RECEIVE_ID = ReceiveId;
                student.FREQUENCY = FrequencyId;

                // fetch programme mode for frequency
                // fetch programme mode by programme id 
                var FetchProgrammeMode = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(new CP_PROGRAMME_GROUP() { PROGRAMME_GROUP_ID = ProgrammeID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetProgammeMode), sAcademicYear).DataSource.Data;
                List<FEE_FREQUENCY_FEE_MODE> FetchFrequency = new List<FEE_FREQUENCY_FEE_MODE>();
                if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                {
                    // seperate ssc and regular frequency
                    if (FetchProgrammeMode.FirstOrDefault().PROGRAMME_MODE == "1")
                    {
                        FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;

                    }
                    else
                    {
                        FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFeeSSC }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;

                    }
                }
                var StudentInfo = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = ReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationByReceivedId), sAcademicYear).DataSource.Data;
                sresultArgs = CMSHandler.SaveOrUpdate(student, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SelectInsertForStudentAccountByReceiveIdAndFrequencyId), sAcademicYear);

                if (FetchFrequency != null && FetchFrequency.Count > 0 && FetchFrequency.FirstOrDefault().FREQUENCY_ID.Equals(FrequencyId))
                {
                    var FetchOtherFees = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(new StuPersonalInfo() { RECEIVE_ID = ReceiveId, FREQUENCY = FrequencyId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOtherFeeForInsert), sAcademicYear).DataSource.Data;
                    if (StudentInfo != null && StudentInfo.Count() > 0 && FetchOtherFees != null && FetchOtherFees.Count > 0)
                    {
                        if (StudentInfo.FirstOrDefault().APPLICATION_TYPE.ToLower().Equals("pg") || StudentInfo.FirstOrDefault().APPLICATION_TYPE.ToLower().Equals("mca") || StudentInfo.FirstOrDefault().APPLICATION_TYPE.ToLower().Equals("mca later entry") || StudentInfo.FirstOrDefault().APPLICATION_TYPE.ToLower().Equals("m.phil"))
                        {
                            if (!string.IsNullOrEmpty(StudentInfo.FirstOrDefault().UNIVERSITY) && StudentInfo.FirstOrDefault().UNIVERSITY.ToUpper().Equals("OTHERS"))
                            {
                                var select = FetchOtherFees.Where(s => s.HEAD == "83");
                                ObjFeeStructure.HEAD = select.FirstOrDefault().HEAD;
                                ObjFeeStructure.AMOUNT = select.FirstOrDefault().AMOUNT;
                                ObjFeeStructure.STUDENT_ID = ReceiveId;
                                ObjFeeStructure.FREQUENCY = FrequencyId;
                                ObjFeeStructure.FEE_MAIN_HEAD_ID = select.FirstOrDefault().FEE_MAIN_HEAD_ID;
                                ObjFeeStructure.FEE_STRUCTURE_ID = select.FirstOrDefault().FEE_STRUCTURE_ID; ;
                                ObjFeeStructure.FEE_ROOT_ID = select.FirstOrDefault().FEE_ROOT_ID;
                                sresultArgs = CMSHandler.SaveOrUpdate(ObjFeeStructure, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertOtherFeeForSpecialStudents), sAcademicYear);
                            }
                        }
                        else
                        {
                            var select = FetchOtherFees.Where(s => s.HEAD == "81");
                            ObjFeeStructure.HEAD = select.FirstOrDefault().HEAD;
                            ObjFeeStructure.AMOUNT = select.FirstOrDefault().AMOUNT;
                            ObjFeeStructure.STUDENT_ID = ReceiveId;
                            ObjFeeStructure.FREQUENCY = FrequencyId;
                            ObjFeeStructure.FEE_MAIN_HEAD_ID = select.FirstOrDefault().FEE_MAIN_HEAD_ID;
                            ObjFeeStructure.FEE_STRUCTURE_ID = select.FirstOrDefault().FEE_STRUCTURE_ID; ;
                            ObjFeeStructure.FEE_ROOT_ID = select.FirstOrDefault().FEE_ROOT_ID;
                            if (!string.IsNullOrEmpty(StudentInfo.FirstOrDefault().EDUCATION_BOARD_ID) && (StudentInfo.FirstOrDefault().EDUCATION_BOARD_ID) != "1")
                            {
                                sresultArgs = CMSHandler.SaveOrUpdate(ObjFeeStructure, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertOtherFeeForSpecialStudents), sAcademicYear);
                            }
                        }
                    }
                }
                if (sresultArgs.Success)

                    sResult = Common.Messages.RecordsSavedSuccessfully;
                else
                    sResult = Common.Messages.FailedToSaveRecords;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveStudentAccount", "Err MSg: " + ex.Message, "Query is empty!");
                }
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return ReceiveId;
        }
        public string SaveHostelStudentAccount(string ReceiveId, string FrequencyId)
        {
            string sResult;
            string sSql = string.Empty;
            ResultArgs sresultArgs = new ResultArgs();
            try
            {
                StuPersonalInfo student = new StuPersonalInfo();
                FEE_STRUCTURE ObjFeeStructure = new FEE_STRUCTURE();
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                student.TYPE = Common.SupFrequencyType.AdmissionFee;
                student.RECEIVE_ID = ReceiveId;
                student.FREQUENCY = FrequencyId;
                var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.HostelFee }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                var StudentInfo = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = ReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationByReceivedId), sAcademicYear).DataSource.Data;
                sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SelectInsertForHostelStudentAccountByReceiveIdAndFrequencyId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, ReceiveId);
                sresultArgs = CMSHandler.SaveOrUpdate(student, sSql, sAcademicYear);
                if (sresultArgs.Success)
                    sResult = Common.Messages.RecordsSavedSuccessfully;
                else
                    sResult = Common.Messages.FailedToSaveRecords;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveStudentAccount", "Err MSg: " + ex.Message, "Query is empty!");
                }
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return ReceiveId;
        }
        #endregion
        #region Reminder SMS & Auto Admission Hold
        public JsonResult SendReminderSMS()
        {
            string sContent = string.Empty;
            string sAcademicYear = string.Empty;
            CommunicationController com = new CommunicationController();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    //Selection Setting Process
                    var Lidateexpiredselectedapplicant = new List<ADM_SELECTION_PROCESS>();
                    var liselectionsetting = (List<ADM_SELECTION_SETTING>)CMSHandler.FetchData<ADM_SELECTION_SETTING>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectionSettings), sAcademicYear).DataSource.Data;
                    if (liselectionsetting != null && liselectionsetting.Count > 0)
                    {
                        //Auto Admission hold
                        foreach (var item in liselectionsetting)
                        {
                            var liapplicant = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new ADM_SELECTION_SETTING() { INTERVAL_DAY = item.INTERVAL_DAY, STATUS = Common.ADM_STATUS.ADMITTED }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchDateExpiredSelectedApplicant), sAcademicYear).DataSource.Data;
                            if (item.IS_AUTO_CANCEL == Common.IsActiveFalg.IsActive)
                            {
                                if (liapplicant != null && liapplicant.Count > 0)
                                {
                                    foreach (var applicant in liapplicant)
                                    {
                                        applicant.STATUS = Common.STATUS.Pending;
                                        var Result = CMSHandler.SaveOrUpdate(applicant, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusofDateExpiredSelectedApplicant));
                                        var Resultarg = CMSHandler.SaveOrUpdate(applicant, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateAdmissionCancelinSelectionprocess), sAcademicYear);
                                    }
                                }
                            }
                            //Send Reminder to Selected  for Verification
                            var liselectedapplicant = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new ADM_SELECTION_SETTING() { INTERVAL_DAY = (Convert.ToInt32(item.INTERVAL_DAY) - (Convert.ToInt32(item.INTERVAL_DAY) - 2)).ToString(), STATUS = Common.ADM_STATUS.SELECTED }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicantforReminderByStatus), sAcademicYear).DataSource.Data;
                            if (liselectedapplicant != null && liselectedapplicant.Count > 0)
                            {
                                foreach (var selected in liselectedapplicant)
                                {
                                    var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Verification_Reminder }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                    sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, selected.FIRST_NAME);

                                    com.liActualSMS.Add(new MessageResult.sms() { to = selected.SMS_MOBILE_NO, message = sContent });
                                }
                            }
                            //Send Reminder to Verified for Payment
                            var liverifiedapplicant = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new ADM_SELECTION_SETTING() { INTERVAL_DAY = (Convert.ToInt32(item.INTERVAL_DAY) - 1).ToString(), STATUS = Common.ADM_STATUS.VERIFIED }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicantforReminderByStatus), sAcademicYear).DataSource.Data;
                            if (liverifiedapplicant != null && liverifiedapplicant.Count > 0)
                            {
                                foreach (var verified in liverifiedapplicant)
                                {
                                    var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.Fee_Payment_Reminder }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                    sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, verified.FIRST_NAME);
                                    com.liActualSMS.Add(new MessageResult.sms() { to = verified.SMS_MOBILE_NO, message = sContent });
                                }
                            }
                        }
                        com.SendMessage();

                    }

                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SendReminderSMS", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SendReminderSMS", ex.Message);
                }
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Over All Applicant List
        [UserRights("Admin")]
        public ActionResult OverAllApplicantList()
        {
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            var Shift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
            var ApplicationType = (List<ADM_APPLICATION_TYPE>)CMSHandler.FetchData<ADM_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
            if (Shift != null && Shift.Count > 0)
                admissionViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            if (ApplicationType != null && ApplicationType.Count > 0)
                admissionViewModel.ApplicationTypeList = new SelectList(ApplicationType, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE);
            return View(admissionViewModel);
        }
        public ActionResult BindOverallApplicantList(string sOverallApplicans)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    var Result = JsonConvert.DeserializeObject<ADM_ISSUE_APPLICATION_2018>(sOverallApplicans);
                    objModel.APPLICATION_TYPE_ID = Result.APPLICATION_TYPE_ID;
                    objModel.SHIFT = Result.SHIFT;
                    objModel.IS_SUBMITTED = Result.IS_SUBMITTED;
                    objModel.IS_FEE_PAID = Result.IS_PAID;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    if (objModel.IS_SUBMITTED == Common.IsActiveFalg.IsActive && objModel.IS_FEE_PAID == Common.IsActiveFalg.IsActive)
                    {
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOverallApplicantListSubmittedAndPaid);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAMME_GROUP_ID);
                        // objViewModel.liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    }
                    if (objModel.IS_SUBMITTED == Common.IsActiveFalg.IsActive && objModel.IS_FEE_PAID == Common.IsActiveFalg.IsNotActive)
                    {
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOverallApplicantListSubmittedAndUnPaid);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAMME_GROUP_ID);
                        //   objViewModel.liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    }
                    if (objModel.IS_SUBMITTED == Common.IsActiveFalg.IsNotActive && objModel.IS_FEE_PAID == Common.IsActiveFalg.IsNotActive)
                    {
                        objModel.IS_SUBMITTED = Common.IsActiveFalg.IsActive;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOverallApplicantListNotSubmittedAndUnPaid);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAMME_GROUP_ID);
                        // objViewModel.liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessApplicantList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessApplicantList", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region ApplicantTransfer
        [UserRights("Staff")]
        public ActionResult ApplicantProgrammeTransfer()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objStaffModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeInchargeByStaffId), sAcademicYear).DataSource.Data;
                    if (CycleList != null && CycleList.Count > 0)
                        objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ApplicantProgrammeTransfer", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ApplicantProgrammeTransfer", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindApplicantProgrammeTransfer(string sCycleId, string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.SELECTION_CYCLE = sCycleId;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicantsToTransfer);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessApplicantList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessApplicantList", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult SendRequestToTransferApplicant(string ReceiveId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ResultArgs sresultArgs = new ResultArgs();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    objModel.RECEIVE_ID = ReceiveId;
                    objModel.STUDENT_ID = ReceiveId;
                    objModel.REQUEST_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var ApplicantInfo = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicantsInfoForTransfer), sAcademicYear).DataSource.Data;
                    if (ApplicantInfo != null && ApplicantInfo.Count > 0)
                    {
                        objModel.PROGRAMME_MODE = ApplicantInfo.FirstOrDefault().PROGRAMME_MODE_ID;
                        objModel.SHIFT = ApplicantInfo.FirstOrDefault().SHIFT;
                    }
                    objViewModel.liFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchFrequencyByReceiveId), sAcademicYear).DataSource.Data;
                    var Programme = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeGroupByProgrammeId), sAcademicYear).DataSource.Data;
                    if (objViewModel.liFrequency != null)
                    {
                        foreach (var item in objViewModel.liFrequency)
                        {
                            objModel.FREQUENCY_ID = item.FREQUENCY_ID;
                            var FetchCreditAndBalance = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchCreditByStudentId), sAcademicYear).DataSource.Data;
                            var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
                            var FetchFeeMainHead = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeMainHeadByFrequencyId), sAcademicYear).DataSource.Data;
                            if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                            {
                                if (objViewModel.liFeeStudentAccount == null)
                                    objViewModel.liFeeStudentAccount = FetchFeeStudentAccount;
                                else
                                    objViewModel.liFeeStudentAccount.AddRange(FetchFeeStudentAccount);
                            }
                            if (FetchCreditAndBalance != null && FetchCreditAndBalance.Count > 0)
                            {
                                if (objViewModel.liCreditAndDebit == null)
                                    objViewModel.liCreditAndDebit = FetchCreditAndBalance;
                                else
                                    objViewModel.liCreditAndDebit.AddRange(FetchCreditAndBalance);
                            }
                            if (FetchFeeMainHead != null && FetchFeeMainHead.Count > 0)
                            {
                                if (objViewModel.liFeeStructure == null)
                                    objViewModel.liFeeStructure = FetchFeeMainHead;
                                else
                                    objViewModel.liFeeStructure.AddRange(FetchFeeMainHead);
                            }
                        }
                    }
                    if (ApplicantInfo != null && ApplicantInfo.Count > 0)
                        // objViewModel.liApplicant = ApplicantInfo;
                        if (Programme != null && Programme.Count > 0)
                            objViewModel.ProgrammeList = new SelectList(Programme, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SendRequestToTransferApplicant", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SendRequestToTransferApplicant", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult ApplicantProgrammeTransferApprove()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var ProgrammeList = (List<cp_Program_2017>)CMSHandler.FetchData<cp_Program_2017>(objStaffModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeInchargeByStaffId), sAcademicYear).DataSource.Data;
                    if (CycleList != null && CycleList.Count > 0)
                        objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ApplicantProgrammeTransferApprove", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ApplicantProgrammeTransferApprove", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindApplicantProgrammeTransferToApprove(string sCycleId, string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.SELECTION_CYCLE = sCycleId;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectionProcessByProgrammeIdAndCycle);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindApplicantProgrammeTransferToApprove", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindApplicantProgrammeTransferToApprove", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult ApproveRequestToTransferApplicant(string ReceiveId)
        {
            JsonResultData ObjResult = new JsonResultData();
            ResultArgs sresultArgs = new ResultArgs();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    objModel.RECEIVE_ID = ReceiveId;
                    objModel.REQUEST_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sresultArgs = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SendRequestToTransferApplicant), sAcademicYear);
                    if (sresultArgs.Success)
                        ObjResult.Message = Common.Messages.RecordsSavedSuccessfully;
                    else
                        ObjResult.Message = Common.Messages.FailedToSaveRecords;
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    ObjResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ApproveRequestToTransferApplicant", "Err MSg: " + ex.Message, "Query is empty!");
                }
                ObjResult.Message = Common.Messages.FailedToSaveRecords;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindFeeDetailsByProgramme(string sProgrammeGroupId, string sShiftId, string sProgrammeMode)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            FEE_STRUCTURE objModel = new FEE_STRUCTURE();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.PROGRAMME = sProgrammeGroupId;
                    objModel.SHIFT = sShiftId;
                    objModel.PROGRAMME_MODE = sProgrammeMode;
                    objViewModel.liFrequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchFrequencyByProgrammeGroupId), sAcademicYear).DataSource.Data;
                    objViewModel.liFeeStructure = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchFeeDetailsByProgrammeGroupId), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindFeeDetailsByProgramme", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindFeeDetailsByProgramme", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult SaveAdmProgrammeTransfer(string sADMTransfer)
        {
            JsonResultData ObjResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    var Result = JsonConvert.DeserializeObject<ADM_TRANSFER>(sADMTransfer);
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    Result.REQUEST_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var sresultArgs = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmTransfer), sAcademicYear);
                    if (sresultArgs.Success)
                        ObjResult.Message = Common.Messages.RecordsSavedSuccessfully;
                    else
                        ObjResult.Message = Common.Messages.FailedToSaveRecords;
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    ObjResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveAdmProgrammeTransfer", "Err MSg: " + ex.Message, "Query is empty!");
                }
                ObjResult.Message = Common.Messages.FailedToSaveRecords;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Transfer Approve By Principal
        public ActionResult TransferApproveByPrincipal()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    //var Applicantlist = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicantByProgrammeId), sAcademicYear).DataSource.Data;
                    //if (Applicantlist != null && Applicantlist.Count > 0)
                    //    objViewModel.ApplicantList = new SelectList(Applicantlist, Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO);

                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ApplicantProgrammeTransferApprove", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ApplicantProgrammeTransferApprove", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult FetchStudentByProgrammeId(string sProgrammeId)
        {
            string sOPtion = string.Empty;
            string sPorgramme = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {

                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                try
                {
                    var Applicantlist = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION { PROGRAMME_ID = sProgrammeId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicantByProgrammeId), sAcademicYear).DataSource.Data;
                    if (Applicantlist != null && Applicantlist.Count > 0)
                    {
                        foreach (var item in Applicantlist)
                        {
                            sOPtion += "<option value='" + item.RECEIVE_ID + "'>" + item.APPLICATION_NO + "</option>";
                        }
                    }
                    var FetchProgrammdetails = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(new ADM_APPTYPE_PROG_GROUPS() { PROGRAMME_ID = sProgrammeId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchPorgrammedetails)).DataSource.Data;
                    if (FetchProgrammdetails != null)
                    {

                        var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(new ADM_APPTYPE_PROG_GROUPS() { APPTYPE_ID = FetchProgrammdetails.FirstOrDefault().APPTYPE_ID, SHIFT = FetchProgrammdetails.FirstOrDefault().SHIFT, PROGRAMME_MODE = FetchProgrammdetails.FirstOrDefault().PROGRAMME_MODE }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchToProgramme), sAcademicYear).DataSource.Data;
                        if (ProgrammeList != null && ProgrammeList.Count > 0)
                        {
                            foreach (var item in ProgrammeList)
                            {
                                sPorgramme += "<option value='" + item.PROGRAMME_GROUP_ID + "'>" + item.PROGRAMME_NAME + "</option>";
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "FetchStudentByProgrammeId", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "FetchStudentByProgrammeId", ex.Message);
                    }
                }
            }
            string JsonData = string.Empty;
            var data = new ADM_RECEIVE_APPLICATION() { RECEIVE_ID = sOPtion, PROGRAMME_ID = sPorgramme };
            JsonData = JsonConvert.SerializeObject(data);
            return Json(JsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindStudentForTransfer(string sProgrammeId, string sReceiveId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sPorgramme = string.Empty;
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentsForTransfer);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objModel.RECEIVE_ID = sReceiveId;
                    objViewModel.liTransfer = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    var FetchProgrammdetails = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(new ADM_APPTYPE_PROG_GROUPS() { PROGRAMME_ID = sProgrammeId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchPorgrammedetails)).DataSource.Data;
                    if (FetchProgrammdetails != null)
                    {
                        var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(new ADM_APPTYPE_PROG_GROUPS() { APPTYPE_ID = FetchProgrammdetails.FirstOrDefault().APPTYPE_ID, SHIFT = FetchProgrammdetails.FirstOrDefault().SHIFT, PROGRAMME_MODE = FetchProgrammdetails.FirstOrDefault().PROGRAMME_MODE }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchToProgramme), sAcademicYear).DataSource.Data;
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindStudentForTransfer", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindStudentForTransfer", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult SaveTransferList(string sTransfer)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_TRANSFER objModel = new ADM_TRANSFER();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    var Transferdetails = JsonConvert.DeserializeObject<ADM_TRANSFER>(sTransfer);
                    Transferdetails.APPROVE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var status = Transferdetails.STATUS;
                    var Updatestaus = new ResultArgs();
                    var SaveTransfer = new ResultArgs();
                    var Result = new ResultArgs();
                    var liadmissioncancel = new List<ADM_TRANSFER>();
                    var LiFetchIssueId = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssuedIdByProgrammeIdandReceiveId), sAcademicYear).DataSource.Data;
                    var IsProgrammeExisit = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.IsProgrammeExistByReceiveId)).DataSource.Data;
                    var IssueNo = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueNobyProgrameID)).DataSource.Data;
                    switch (status)
                    {
                        //Registered
                        case Common.CONST_STATUS.Registered:
                            if (LiFetchIssueId != null & LiFetchIssueId.Count > 0)
                            {

                                if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                                {
                                    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                    Transferdetails.STATUS_ID = Common.STATUS.Registered; Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString();
                                    Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                    Result = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertFromProgrammeByIssuedId));
                                }
                                else
                                {
                                    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                    Transferdetails.STATUS_ID = Common.STATUS.Registered; Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString();
                                    Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                    Updatestaus = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatePorgrammeByIssueId));
                                }
                                if (Updatestaus.Success || Result.Success)
                                {
                                    SaveTransfer = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmTransfer), sAcademicYear);

                                }
                            }
                            break;
                        //Submitted
                        case Common.CONST_STATUS.Submitted:
                            if (LiFetchIssueId != null & LiFetchIssueId.Count > 0)
                            {
                                if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                                {
                                    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                    Transferdetails.STATUS_ID = Common.STATUS.Submitted;
                                    Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString();
                                    Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                    Result = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertFromProgrammeByIssuedId));
                                    Transferdetails.ISSUE_NO = IssueNo.LastOrDefault().ISSUE_NO;
                                }
                                else
                                {
                                    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                    Transferdetails.STATUS_ID = Common.STATUS.Submitted;
                                    Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString();
                                    Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                    Updatestaus = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatePorgrammeByIssueId));
                                    Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(Transferdetails.ISSUE_NO) + 1).ToString();
                                }
                                if (Updatestaus.Success || Result.Success)
                                {
                                    var UpdateProgrammeCount = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatePorgrammeIssueCount));
                                    SaveTransfer = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmTransfer), sAcademicYear);
                                }

                            }
                            break;
                        //Selected
                        case Common.CONST_STATUS.Selected:
                            if (LiFetchIssueId != null && LiFetchIssueId.Count > 0)
                            {
                                if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                                {
                                    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                    Transferdetails.STATUS_ID = Common.STATUS.Submitted;
                                    Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString();
                                    Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                    Result = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertFromProgrammeByIssuedId));
                                }
                                else
                                {

                                    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                    Transferdetails.STATUS_ID = Common.STATUS.Submitted;
                                    Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString();
                                    Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                    Updatestaus = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatePorgrammeByIssueId));
                                }
                                if (Updatestaus.Success || Result.Success)
                                {
                                    new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                                    var SelectionUpdate = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.DeleteApplicantByProgrammeAndReceiveId), sAcademicYear);
                                    new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                    if (SelectionUpdate.Success)
                                    {
                                        SaveTransfer = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmTransfer), sAcademicYear);

                                    }
                                }

                            }
                            break;
                        //Verified
                        case Common.CONST_STATUS.Verified:
                            if (LiFetchIssueId != null && LiFetchIssueId.Count > 0)
                            {
                                if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                                {
                                    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                    Transferdetails.STATUS_ID = Common.STATUS.Submitted;
                                    Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString();
                                    Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                    Result = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertFromProgrammeByIssuedId));
                                }
                                else
                                {

                                    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                    Transferdetails.STATUS_ID = Common.STATUS.Selected;
                                    Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString();
                                    Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                    Updatestaus = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatePorgrammeByIssueId));
                                }
                            }
                            if (Updatestaus.Success || Result.Success)
                            {
                                var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                                if (FetchFrequency != null && FetchFrequency.Count > 0)
                                    Transferdetails.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                                new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                                var DeleteFee = CMSHandler.SaveOrUpdate(Transferdetails, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeStudentAccountByReceiveIdandFrequencyId));
                                new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                if (DeleteFee.Success)
                                {
                                    new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                                    var SelectionUpdate = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateSelectionByReceiveIdandProgrammeId), sAcademicYear);
                                    new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                    if (SelectionUpdate.Success)
                                    {
                                        SaveTransfer = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmTransfer), sAcademicYear);

                                    }
                                }
                            }
                            break;
                        //Admitted
                        case Common.CONST_STATUS.Admitted:
                            if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                            {
                                Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                Transferdetails.STATUS_ID = Common.STATUS.Submitted; Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString(); 
                                Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                Result = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertFromProgrammeByIssuedId));
                                if (Result.Success)
                                {
                                    SaveTransfer = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmTransfer), sAcademicYear);
                                }
                            }
                            new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                            var UpdateSelection = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.DeleteandUpdateCancelByReceiveandProgrammeId), sAcademicYear);
                            new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                            if (UpdateSelection.Success)
                            {
                                Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                Transferdetails.STATUS_ID = Common.STATUS.Cancelled;
                                Transferdetails.ISSUE_NO = Convert.ToInt32(Convert.ToInt32(IssueNo.LastOrDefault().ISSUE_NO) + 1).ToString(); 
                                Transferdetails.APPLICATION_NO = IssueNo.LastOrDefault().APPLICATION_NO;
                                Updatestaus = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateCancel));
                                objViewModel.liAdmissionCancel = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCanceldetails), sAcademicYear).DataSource.Data;
                                if (Result.Success)
                                {
                                    objViewModel.liTransferExist = IsProgrammeExisit;
                                }
                                return View(objViewModel);
                            }
                            break;

                    }

                    if (SaveTransfer.Success)
                    {
                        objViewModel.liTransfer = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchTransferDetails), sAcademicYear).DataSource.Data;
                    }
                    if (Result.Success)
                    {
                        objViewModel.liTransferExist = IsProgrammeExisit;

                    }

                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "UpdateADMApproveById", "Err MSg: " + ex.Message, "Query is empty!");
                }
            }
            return View(objViewModel);
        }
        public JsonResult UpdateADMApproveById(string sTransferId, string sReceiveId)
        {
            JsonResultData ObjResult = new JsonResultData();
            ADM_TRANSFER objModel = new ADM_TRANSFER();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    objModel.APPROVE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    objModel.TRANSFER_ID = sTransferId;
                    var sresultArgs = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateApproveIdByTransferId), sAcademicYear);
                    if (sresultArgs.Success)
                    {
                        var UpdateStatus = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = sReceiveId, STATUS = Common.STATUS.Transfer }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusByReceiveId));
                        ObjResult.Message = (UpdateStatus.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                    }
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    ObjResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "UpdateADMApproveById", "Err MSg: " + ex.Message, "Query is empty!");
                }
                ObjResult.Message = Common.Messages.FailedToSaveRecords;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Student Direct Selection
        [UserRights("Principal")]
        public ActionResult StudnetSelection()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    var ObjViewModel = new AdmissionViewModel();
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var liProgram = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (liProgram != null)
                    {
                        ObjViewModel.ProgrammeList = new SelectList(liProgram, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    }
                    return View(ObjViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "StudnetSelection", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "StudnetSelection", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }

        }

        public ActionResult StudentSelectionList(string sJsonProgramId)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                if (!string.IsNullOrEmpty(sJsonProgramId))
                {
                    try
                    {
                        var ObjViewModel = new AdmissionViewModel();
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        string sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetSubmittedStudentForPrincipal).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sJsonProgramId);
                        ObjViewModel.liIssueApplication = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(null, sSql, sAcademicYear).DataSource.Data;
                        return View(ObjViewModel);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("AdmissionController", "StudnetSelection", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("AdmissionController", "StudnetSelection", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult SaveSelectionProccessByReceiveId(string sReceiveId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sReceiveId))
                {
                    try
                    {
                        var ObjViewModel = new AdmissionViewModel();
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        var liSelectionProccess = (List<SELECTIONPROCCESS>)CMSHandler.FetchData<SELECTIONPROCCESS>(new ADM_ISSUE_APPLICATION_2018 { RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetSubmittedStudentsForPrincipalByReceiveId), sAcademicYear).DataSource.Data;
                        if (liSelectionProccess != null && liSelectionProccess.Count > 0)
                        {
                            string UserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                            var ObjSelectionProccess = new ADM_SELECTION_PROCESS() { APPLICATION_NO = liSelectionProccess.FirstOrDefault().APPLICATION_NO, APPLICATION_TYPE_ID = liSelectionProccess.FirstOrDefault().APPLICATION_TYPE_ID, PROGRAMME_ID = liSelectionProccess.FirstOrDefault().PROGRAMME_ID, SELECTION_CYCLE = Common.IsActiveFalg.IsActive, TOTAL_CUT_OFF_MARK = liSelectionProccess.FirstOrDefault().TOTAL_CUT_OFF_MARK, TOTAL_SECURED = liSelectionProccess.FirstOrDefault().HSTOTAL, MAX_TOTAL = liSelectionProccess.FirstOrDefault().HS_MAX_MARK, SELECTED_BY = UserId, RECEIVE_ID = sReceiveId };
                            var SaveSelectionProcess = CMSHandler.SaveOrUpdate(ObjSelectionProccess, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveSelectionProcess), sAcademicYear);
                            if (SaveSelectionProcess.Success)
                            {
                                // string sContent = "TEST";
                                var UpdateStatus = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = sReceiveId, STATUS = Common.STATUS.Selected }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusByReceiveId));
                                if (UpdateStatus.Success)
                                {
                                    var FetchReceiveToCommunicate = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchReceiveToSendMailAndMessage)).DataSource.Data;
                                    if (FetchReceiveToCommunicate != null && FetchReceiveToCommunicate.Count > 0)
                                    {
                                        // SendEmailAndTextMessage(sContent, FetchReceiveToCommunicate.FirstOrDefault().SMS_MOBILE_NO, FetchReceiveToCommunicate.FirstOrDefault().EMAIL_ID, "Verification from St. Mary's College,Thoothukudi.");
                                        ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                                    }
                                    else
                                        ObjJsonData.Message = Common.Messages.NoRecordsfound;
                                }
                                else
                                    ObjJsonData.Message = Common.Messages.FailedToSaveRecords;

                            }
                            else
                                ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                        }
                        else
                        {
                            ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                            ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                        }
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("AdmissionController", "StudnetSelection", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("AdmissionController", "StudnetSelection", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region For Principal View
        public ActionResult AllApplicationViewPrincipal()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    //var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var programmes = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (programmes != null)
                    {
                        objViewModel.ProgrammeList = new SelectList(programmes, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    }
                    var CasteList = (List<SupCommunity>)CMSHandler.FetchData<SupCommunity>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCommunity)).DataSource.Data;
                    var ApplicantTypeList = (List<SUP_APPLICANT_TYPE>)CMSHandler.FetchData<SUP_APPLICANT_TYPE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchApplicantType)).DataSource.Data;
                    if (CasteList != null && CasteList.Count > 0)
                        objViewModel.CasteList = new SelectList(CasteList, Common.SUP_COMMUNITY.COMMUNITYID, Common.SUP_COMMUNITY.COMMUNITY);
                    if (ApplicantTypeList != null && ApplicantTypeList.Count > 0)
                        objViewModel.ApplicantTypeList = new SelectList(ApplicantTypeList, Common.SUP_APPLICANT_TYPE.APPLICANT_TYPE_ID, Common.SUP_APPLICANT_TYPE.APPLICANT_TYPE);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "AllApplicationViewPrincipal", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "AllApplicationViewPrincipal", ex.Message);
                }
            }
            return View(objViewModel);
        }

        #endregion


        // this is not in use
        #region Hostel Registration
        [UserRights("Applicant")]
        public ActionResult HostelRegistration()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
                AdmissionViewModel admissionViewModel = new AdmissionViewModel();
                objModel.ISSUE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                objModel.STATUS = Common.STATUS.Admitted;
                var LiHostelAuthendicate = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.CheckHostelEligibility)).DataSource.Data;
                if (LiHostelAuthendicate != null && LiHostelAuthendicate.Count > 0)
                {
                    //  admissionViewModel.liApplicant = LiHostelAuthendicate;
                }
                return View(admissionViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }

        }
        public ActionResult FetchRelationsInfo()
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_STU_RELATIONS objModel = new ADM_STU_RELATIONS();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.ACADEMIC_YEAR = sAcademicYear;
                    objModel.RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    admissionViewModel.liADMStudentStudentRelations = (List<ADM_STU_RELATIONS>)CMSHandler.FetchData<ADM_STU_RELATIONS>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentRelationsByReceiveId), sAcademicYear).DataSource.Data;
                    admissionViewModel.liRelation = (List<SUP_RELATION>)CMSHandler.FetchData<SUP_RELATION>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchRelaion)).DataSource.Data;
                    admissionViewModel.liOccupation = (List<Sup_Occupation>)CMSHandler.FetchData<Sup_Occupation>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOccupation)).DataSource.Data;
                    return View(admissionViewModel);
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchRelationsInfo", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchRelationsInfo", ex.Message);
                    return View(admissionViewModel);
                }
            }
        }
        public JsonResult FetchHostelRegistrationById()
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            cp_Classes_2017 objClassModel = new cp_Classes_2017();
            Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] = "2018";
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    //objModel.ISSUE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    //admissionViewModel.liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionIssueApplicationById), sAcademicYear).DataSource.Data;
                    //if (admissionViewModel.liApplicant != null && admissionViewModel.liApplicant.Count > 0)
                    //{
                    //    Session[Common.SESSION_VARIABLES.HSC_NO] = admissionViewModel.liApplicant.FirstOrDefault().HSC_NO;
                    //    Session[Common.SESSION_VARIABLES.RECEIVE_ID] = admissionViewModel.liApplicant.FirstOrDefault().RECEIVE_ID;
                    //    Session[Common.SESSION_VARIABLES.APPLICATION_NO] = admissionViewModel.liApplicant.FirstOrDefault().APPLICATION_NO;
                    //    Session[Common.SESSION_VARIABLES.APPLICATION_TYPE_ID] = admissionViewModel.liApplicant.FirstOrDefault().APPLICATION_TYPE_ID;
                    //    Session[Common.SESSION_VARIABLES.PROGRAMME_ID] = admissionViewModel.liApplicant.FirstOrDefault().PROGRAMME_ID;
                    //    Session[Common.SESSION_VARIABLES.CLASS_NAME] = admissionViewModel.liApplicant.FirstOrDefault().PROGRAMME_NAME;
                    //    Session[Common.SESSION_VARIABLES.COURSE_CODE] = admissionViewModel.liApplicant.FirstOrDefault().PROGRAMME_CODE;
                    //    Session[Common.SESSION_VARIABLES.PROGRAMME_GROUP_ID] = admissionViewModel.liApplicant.FirstOrDefault().PROGRAMME_GROUP_ID;
                    //    Session[Common.SESSION_VARIABLES.PHOTO_PATH] = admissionViewModel.liApplicant.FirstOrDefault().PHOTO_PATH;
                    //    objClassModel.PROGRAMME = admissionViewModel.liApplicant.FirstOrDefault().PROGRAMME_GROUP_ID;
                    //    var ClassId = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objClassModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionClassesByProgrammeId), sAcademicYear).DataSource.Data;
                    //    if (ClassId != null && ClassId.Count > 0)
                    //    {
                    //        admissionViewModel.liApplicant.FirstOrDefault().CLASS_ID = ClassId.FirstOrDefault().CLASS_ID;
                    //    }
                    //    return Json(admissionViewModel.liApplicant, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    //    objResult.ErrorCode = Common.ErrorCode.NoContent;
                    //    objResult.Message = Common.ErrorMessage.NoContent;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                    //}
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "Index", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "Index", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult SaveOrUpdateHostelPersonalInfo(string sHostelPersonal)
        {
            JsonResultData ObjResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    var Result = JsonConvert.DeserializeObject<ADM_ISSUE_APPLICATION_2018>(sHostelPersonal);
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    if (Session[Common.SESSION_VARIABLES.HOSTEL_REGISTRATION_ID] != null)
                        Result.HOSTEL_REGISTRATION_ID = Session[Common.SESSION_VARIABLES.HOSTEL_REGISTRATION_ID].ToString();
                    var sresultArgs = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL((!string.IsNullOrEmpty(Result.HOSTEL_REGISTRATION_ID)) ? AdmissionSQLCommands.UpdateHostelRegistration : AdmissionSQLCommands.SaveHostelRegistration), sAcademicYear, true);
                    if (sresultArgs.Success)
                    {
                        if (string.IsNullOrEmpty(Result.HOSTEL_REGISTRATION_ID))
                            Session[Common.SESSION_VARIABLES.HOSTEL_REGISTRATION_ID] = sresultArgs.RowUniqueId.ToString();
                        var SaveReceiveApplication = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateReceiveApplicationByReceiveId), sAcademicYear);
                        ObjResult.Message = (SaveReceiveApplication.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                    }
                    else
                        ObjResult.Message = Common.Messages.FailedToSaveRecords;
                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    ObjResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateHostelPersonalInfo", "Err MSg: " + ex.Message, "Query is empty!");
                }
                ObjResult.Message = Common.Messages.FailedToSaveRecords;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveOrUpdateFamilyDetails(string sRelationJson)
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            ADM_STU_SUBMARKS objModel = new ADM_STU_SUBMARKS();
            ADM_ISSUE_APPLICATION_2018 objIssueModel = new ADM_ISSUE_APPLICATION_2018();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JSON_ADM_STU_SUBMARKS objJson = serializer.Deserialize<JSON_ADM_STU_SUBMARKS>(sRelationJson);
                    if (objJson.SAVE_STU_RELATIONS != null && objJson.SAVE_STU_RELATIONS.Count > 0)
                    {
                        foreach (var item in objJson.SAVE_STU_RELATIONS)
                        {
                            var sresultArgs = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL((!string.IsNullOrEmpty(item.RELATION_ID)) ? AdmissionSQLCommands.UpdateRelationsDetails : AdmissionSQLCommands.SaveRelationsDetails), sAcademicYear, true);
                            objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : objResult.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.BadRequest;
                        objResult.Message = Common.ErrorMessage.ExpectationFailed;
                        return Json(objResult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateFamilyDetails", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveOrUpdateFamilyDetails", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteStudentRelation(string sRelationId)
        {
            JsonResultData objResult = new JsonResultData();
            SUP_RELATION obModel = new SUP_RELATION();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(sRelationId))
                    {
                        obModel.RELATION_ID = sRelationId;
                        var sresultArgs = CMSHandler.SaveOrUpdate(obModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.DeleteStudentRelation));
                        objResult.Message = (sresultArgs.Success) ? Common.ErrorCode.Ok : objResult.Message = Common.Messages.FailedToSaveRecords;
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.BadRequest;
                        objResult.Message = Common.ErrorMessage.ExpectationFailed;
                        return Json(objResult, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "DeleteStudentRelation", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "DeleteStudentRelation", ex.Message);
                        return Json(objResult, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveOrUpdatedPhotoUploads()
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string filePath = string.Empty;
                    string isSubmitClick = string.Empty;
                    string sImagePath = string.Empty;
                    string _sDirectorypath = string.Empty;
                    string sContent = string.Empty;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.IS_SUBMITTED = Request.Form["IS_SUBMITTED"];
                    isSubmitClick = Request.Form["Is_Submit_Click"];
                    objModel.RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    string ClassName = Session[Common.SESSION_VARIABLES.COURSE_CODE].ToString();
                    ClassName = ClassName.Replace(".", "");
                    _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_HOSTEL_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + ClassName;
                    if (!Directory.Exists(_sDirectorypath))
                    {
                        Directory.CreateDirectory(_sDirectorypath);
                    }
                    if (isSubmitClick.Equals("1"))
                    {
                        objModel.STATUS = Common.STATUS.Submitted;
                        var UpdateSubmitted = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateApplicationSubmittionForHostel));
                        if (UpdateSubmitted.Success)
                        {
                            //var FetchFeeStudentAccount = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { ISSUE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                            //if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                            //{
                            //    sContent = "Dear " + FetchFeeStudentAccount.FirstOrDefault().FIRST_NAME + ",\n\nGreetings,\n Your Application has been submitted Successfully and the submitted Application No is  " + FetchFeeStudentAccount.FirstOrDefault().APPLICATION_NO + ". \n Note:\n If your application is selected you will receive a message and email for your confirmation.  \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.\nPh:0461-2320946. ";
                            //    SendEmailAndTextMessage(sContent, FetchFeeStudentAccount.FirstOrDefault().CONTACT_NO, FetchFeeStudentAccount.FirstOrDefault().EMAIL_ID, "Application is submitted successfully.");
                            //}

                        }
                    }
                    if (Request.Files.Count > 0)
                    {
                        foreach (var item in Request.Files)
                        {
                            var File = Request.Files[0];
                            string ImgName = File.FileName;
                            var Img = ImgName.Split('.');
                            string fileName = ImgName.Replace(Img[0], Session[Common.SESSION_VARIABLES.APPLICATION_NO].ToString());

                            if (!string.IsNullOrEmpty(fileName))
                            {
                                filePath = _sDirectorypath + "\\" + fileName;
                                sImagePath = "\\" + Common.URLPages.STUDENT_HOSTEL_PATH + "\\" + Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() + "\\" + ClassName + "\\" + fileName;
                                File.SaveAs(filePath);
                                objModel.FAMILY_PHOTO = sImagePath;
                            }
                            string sSQL = string.Empty;
                            sSQL = "SET SQL_SAFE_UPDATES=0;";
                            new MySQLHandler().ExecuteAsScripts(sSQL);
                            var resultArgs = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateFamilyPhoto), sAcademicYear);
                            sSQL = "SET SQL_SAFE_UPDATES=1;";
                            new MySQLHandler().ExecuteAsScripts(sSQL);
                            if (resultArgs.Success)
                                objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                            else
                                objResult.Message = Common.Messages.FailedToSaveRecords;
                        }

                    }
                    else
                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveOrUpdatedPhotoUploads", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveOrUpdatedPhotoUploads", ex.Message);
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion
        public ActionResult HostelApplication()
        {
            string sUserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sUserId))
            {
                try
                {
                    // check fees paid or not 
                    ObjViewModel.liFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT { STUDENT_ID = sUserId, FREQUENCY_ID = "3", }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelFeePaidStatus), sAcademicYear).DataSource.Data;
                    if (ObjViewModel.liFeeStudentAccount.Count() > 0 && ObjViewModel.liFeeStudentAccount != null)
                    {
                        ObjViewModel.LiHostelStudentExtracurricular = (List<HOSTEL_STUDENT_EXTRACURRICULAR>)CMSHandler.FetchData<HOSTEL_STUDENT_EXTRACURRICULAR>(new HOSTEL_STUDENT_EXTRACURRICULAR { STUDENT_ID = sUserId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentExtracurricularchk), sAcademicYear).DataSource.Data;
                        ObjViewModel.LiHostelStudentCertificate = (List<HOSTEL_STUDENT_CERTIFICATE>)CMSHandler.FetchData<HOSTEL_STUDENT_CERTIFICATE>(new HOSTEL_STUDENT_CERTIFICATE { STUDENT_ID = sUserId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentCertificatechk), sAcademicYear).DataSource.Data;
                        ObjViewModel.LiHostelStudentGadget = (List<HOSTEL_STUDENT_GADGETS>)CMSHandler.FetchData<HOSTEL_STUDENT_GADGETS>(new HOSTEL_STUDENT_GADGETS { STUDENT_ID = sUserId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentGadgetschk), sAcademicYear).DataSource.Data;
                        ObjViewModel.LiHostelStudentSport = (List<HOSTEL_STUDENT_SPORTS>)CMSHandler.FetchData<HOSTEL_STUDENT_SPORTS>(new HOSTEL_STUDENT_SPORTS { STUDENT_ID = sUserId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentSportschk), sAcademicYear).DataSource.Data;
                        ObjViewModel.liApplicationForm = (List<APPLICATION_FORM>)CMSHandler.FetchData<APPLICATION_FORM>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = sUserId },
                           SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationFormByReceiveIdForHostel), sAcademicYear).DataSource.Data;
                        ObjViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(new ADM_STU_SUBMARKS() { RECEIVE_STUID = sUserId },
                            SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuSubMarksByReceivedId), sAcademicYear).DataSource.Data;
                        ViewBag.Message = string.Empty;
                    }
                    else
                    {
                        ViewBag.Message = "Please pay your Application fee..!";
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "HostelApplication", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(ObjViewModel);
        }
        public JsonResult SaveHostelApplication(string schkgad, string schksport, string schke, string schkcertificate, string DoorNo, string Street, string City, string PinCode, string District, string PhoneNo, string Name, string Occupation, string Income, string Name1, string Occupation1, string Income1, string Name2, string Occupation2, string Income2, string Mobile, string Mobile1, string Mobile2)
        {
            HOSTEL_STUDENT_GADGETS objGadget = new HOSTEL_STUDENT_GADGETS();
            HOSTEL_STUDENT_SPORTS objSport = new HOSTEL_STUDENT_SPORTS();
            HOSTEL_STUDENT_EXTRACURRICULAR objExtracurricular = new HOSTEL_STUDENT_EXTRACURRICULAR();
            HOSTEL_STUDENT_CERTIFICATE ObjCertificate = new HOSTEL_STUDENT_CERTIFICATE();

            string sUserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sUserId))
            {
                try
                {
                    string sQuery = "UPDATE adm_receive_application SET LDOORDETAIL='" + DoorNo + "',LSTREET='" + Street + "',LCITY='" + City + "',LDISTRICT='" + District + "',LPINCODE='" + PinCode + "',LPHONENO='" + PhoneNo + "',BSNAME='" + Name + "',BSOCCUPATION='" + Occupation + "',BSINCOME='" + Income + "',BSNAME1='" + Name1 + "',BSOCCUPATION1='" + Occupation1 + "',BSINCOME1='" + Income1 + "',BSNAME2='" + Name2 + "',BSOCCUPATION2='" + Occupation2 + "',BSINCOME2='" + Income2 + "',BSMOBILE='" + Mobile + "',BSMOBILE1='" + Mobile1 + "',BSMOBILE2='" + Mobile2 + "'  WHERE RECEIVE_ID='" + sUserId + "';";
                    var sresultArgs1 = CMSHandler.SaveOrUpdate(null, sQuery, sAcademicYear);

                    // Gadget
                    objGadget.STUDENT_ID = sUserId;
                    var vschkgad = schkgad.Split(',');
                    foreach (var item in vschkgad)
                    {
                        objGadget.GADGETS_ID = item;
                        var ChkisExist = (List<HOSTEL_STUDENT_GADGETS>)CMSHandler.FetchData<HOSTEL_STUDENT_GADGETS>(new HOSTEL_STUDENT_GADGETS() { STUDENT_ID = sUserId, GADGETS_ID = item }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentGadgetschkIsExist), sAcademicYear).DataSource.Data;
                        if (ChkisExist != null && ChkisExist.Count > 0)
                        {
                            objGadget.HS_GADGETS_ID = ChkisExist.FirstOrDefault().HS_GADGETS_ID;
                            var sresultArgs = CMSHandler.SaveOrUpdate(objGadget, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateHostelStudentGadgets), sAcademicYear);
                        }
                        else
                        {
                            var sresultArgs = CMSHandler.SaveOrUpdate(objGadget, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertHostelStudentGadgets), sAcademicYear);
                        }
                    }

                    // Sport
                    objSport.STUDENT_ID = sUserId;
                    var vschksport = schksport.Split(',');
                    foreach (var item in vschksport)
                    {
                        objSport.SPORTS_ID = item;
                        var ChkisExist = (List<HOSTEL_STUDENT_SPORTS>)CMSHandler.FetchData<HOSTEL_STUDENT_SPORTS>(new HOSTEL_STUDENT_SPORTS() { STUDENT_ID = sUserId, SPORTS_ID = item }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentSportschkIsExist), sAcademicYear).DataSource.Data;
                        if (ChkisExist != null && ChkisExist.Count > 0)
                        {
                            objSport.HS_SPORTS_ID = ChkisExist.FirstOrDefault().HS_SPORTS_ID;
                            var sresultArgs = CMSHandler.SaveOrUpdate(objSport, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateHostelStudentSports), sAcademicYear);
                        }
                        else
                        {
                            var sresultArgs = CMSHandler.SaveOrUpdate(objSport, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertHostelStudentSports), sAcademicYear);
                        }
                    }


                    // ExtraCurricular
                    objExtracurricular.STUDENT_ID = sUserId;
                    var vschke = schke.Split(',');
                    foreach (var item in vschke)
                    {
                        objExtracurricular.EXTRACURRICULAR_ID = item;
                        var ChkisExist = (List<HOSTEL_STUDENT_EXTRACURRICULAR>)CMSHandler.FetchData<HOSTEL_STUDENT_EXTRACURRICULAR>(new HOSTEL_STUDENT_EXTRACURRICULAR() { STUDENT_ID = sUserId, EXTRACURRICULAR_ID = item }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentExtracurricularchkIsExist), sAcademicYear).DataSource.Data;
                        if (ChkisExist != null && ChkisExist.Count > 0)
                        {
                            objExtracurricular.HS_EXTRACURRICULAR_ID = ChkisExist.FirstOrDefault().HS_EXTRACURRICULAR_ID;
                            var sresultArgs = CMSHandler.SaveOrUpdate(objExtracurricular, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateHostelStudentExtracurricular), sAcademicYear);
                        }
                        else
                        {
                            var sresultArgs = CMSHandler.SaveOrUpdate(objExtracurricular, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertHostelStudentExtracurricular), sAcademicYear);
                        }
                    }

                    ObjCertificate.STUDENT_ID = sUserId;
                    var vschkcertificate = schkcertificate.Split(',');
                    foreach (var item in vschkcertificate)
                    {
                        ObjCertificate.CERTIFICATE_ID = item;
                        var ChkisExist = (List<HOSTEL_STUDENT_CERTIFICATE>)CMSHandler.FetchData<HOSTEL_STUDENT_CERTIFICATE>(new HOSTEL_STUDENT_CERTIFICATE() { STUDENT_ID = sUserId, CERTIFICATE_ID = item }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentCertificatechkIsExist), sAcademicYear).DataSource.Data;
                        if (ChkisExist != null && ChkisExist.Count > 0)
                        {
                            ObjCertificate.HS_CERTIFICATE_ID = ChkisExist.FirstOrDefault().HS_CERTIFICATE_ID;
                            var sresultArgs = CMSHandler.SaveOrUpdate(ObjCertificate, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateHostelStudentCertificate), sAcademicYear);
                            ObjJsonData.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                            ObjJsonData.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.Ok : ObjJsonData.ErrorCode = Common.ErrorCode.NotAcceptable;
                        }
                        else
                        {
                            var sresultArgs = CMSHandler.SaveOrUpdate(ObjCertificate, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertHostelStudentCertificate), sAcademicYear);
                            ObjJsonData.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                            ObjJsonData.ErrorCode = (sresultArgs.Success) ? Common.ErrorCode.Ok : ObjJsonData.ErrorCode = Common.ErrorCode.NotAcceptable;
                        }
                    }





                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveHostelApplication", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HostelApplicationPrint()
        {
            string sUserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sUserId))
            {
                try
                {
                    ObjViewModel.LiHostelStudentExtracurricular = (List<HOSTEL_STUDENT_EXTRACURRICULAR>)CMSHandler.FetchData<HOSTEL_STUDENT_EXTRACURRICULAR>(new HOSTEL_STUDENT_EXTRACURRICULAR { STUDENT_ID = sUserId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentExtracurricularchk), sAcademicYear).DataSource.Data;
                    ObjViewModel.LiHostelStudentCertificate = (List<HOSTEL_STUDENT_CERTIFICATE>)CMSHandler.FetchData<HOSTEL_STUDENT_CERTIFICATE>(new HOSTEL_STUDENT_CERTIFICATE { STUDENT_ID = sUserId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentCertificatechk), sAcademicYear).DataSource.Data;
                    ObjViewModel.LiHostelStudentGadget = (List<HOSTEL_STUDENT_GADGETS>)CMSHandler.FetchData<HOSTEL_STUDENT_GADGETS>(new HOSTEL_STUDENT_GADGETS { STUDENT_ID = sUserId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentGadgetschk), sAcademicYear).DataSource.Data;
                    ObjViewModel.LiHostelStudentSport = (List<HOSTEL_STUDENT_SPORTS>)CMSHandler.FetchData<HOSTEL_STUDENT_SPORTS>(new HOSTEL_STUDENT_SPORTS { STUDENT_ID = sUserId, }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentSportschk), sAcademicYear).DataSource.Data;
                    ObjViewModel.liApplicationForm = (List<APPLICATION_FORM>)CMSHandler.FetchData<APPLICATION_FORM>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = sUserId },
                       SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationFormByReceiveIdForHostel), sAcademicYear).DataSource.Data;
                    ObjViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(new ADM_STU_SUBMARKS() { RECEIVE_STUID = sUserId },
                        SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuSubMarksByReceivedId), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "HostelApplication", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(ObjViewModel);
        }

        public ActionResult HostelApplicationForOffice()
        {
            string sUserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sUserId))
            {
                try
                {
                    ObjViewModel.LiHostelStudentExtracurricular = (List<HOSTEL_STUDENT_EXTRACURRICULAR>)CMSHandler.FetchData<HOSTEL_STUDENT_EXTRACURRICULAR>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentExtracurricular), sAcademicYear).DataSource.Data;
                    ObjViewModel.LiHostelStudentCertificate = (List<HOSTEL_STUDENT_CERTIFICATE>)CMSHandler.FetchData<HOSTEL_STUDENT_CERTIFICATE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentCertificate), sAcademicYear).DataSource.Data;
                    ObjViewModel.LiHostelStudentGadget = (List<HOSTEL_STUDENT_GADGETS>)CMSHandler.FetchData<HOSTEL_STUDENT_GADGETS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentGadgets), sAcademicYear).DataSource.Data;
                    ObjViewModel.LiHostelStudentSport = (List<HOSTEL_STUDENT_SPORTS>)CMSHandler.FetchData<HOSTEL_STUDENT_SPORTS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStudentSports), sAcademicYear).DataSource.Data;
                    ObjViewModel.liApplicationForm = (List<APPLICATION_FORM>)CMSHandler.FetchData<APPLICATION_FORM>(null,
                       SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelApplication), sAcademicYear).DataSource.Data;
                    ObjViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(null,
                        SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuSubMarksByReceivedId), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "HostelApplicationForOffice", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(ObjViewModel);
        }


        #region HostelSelection
        public ActionResult HostelRegisteredList()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                AdmissionViewModel objViewModel = new AdmissionViewModel();
                ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                try
                {
                    var ApplicantType = (List<SUP_APPLICANT_TYPE>)CMSHandler.FetchData<SUP_APPLICANT_TYPE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchApplicantType)).DataSource.Data;
                    var Hostel = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHostel)).DataSource.Data;
                    var Religion = (List<SUP_RELIGION>)CMSHandler.FetchData<SUP_RELIGION>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchReligion)).DataSource.Data;
                    var ApplicationType = (List<SUP_APPLICATION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                    if (ApplicantType != null && ApplicantType.Count > 0)
                    {
                        objViewModel.ApplicantTypeList = new SelectList(ApplicantType, Common.SUP_APPLICANT_TYPE.APPLICANT_TYPE_ID, Common.SUP_APPLICANT_TYPE.APPLICANT_TYPE);
                    }
                    if (Religion != null && Religion.Count > 0)
                    {
                        objViewModel.Religion = new SelectList(Religion, Common.SUP_RELIGION.RELIGIONID, Common.SUP_RELIGION.RELIGION);
                    }
                    if (Hostel != null && Hostel.Count > 0)
                    {
                        objViewModel.HostelList = new SelectList(Hostel, Common.SUP_HOSTEL.HOSTEL_ID, Common.SUP_HOSTEL.HOSTEL_NAME);
                    }
                    if (ApplicationType != null && ApplicationType.Count > 0)
                    {
                        objViewModel.ApplicationTypeList = new SelectList(ApplicationType, Common.SUP_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.SUP_APPLICATION_TYPE.APPLICATION_TYPE);
                    }
                    return View(objViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "HostelRegisteredList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "HostelRegisteredList", ex.Message);
                        return View(objViewModel);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
        }
        public ActionResult BindHostelRegisteredList(string sReligion, string sHostel, string sApplicantType, string sApplicationType)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                AdmissionViewModel objViewModel = new AdmissionViewModel();
                List<SUP_HOSTEL> LiHostel = new List<SUP_HOSTEL>();
                ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
                string sSql = string.Empty;
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (!string.IsNullOrEmpty(sHostel) && !string.IsNullOrEmpty(sApplicantType) && !string.IsNullOrEmpty(sApplicationType))
                {
                    try
                    {
                        objModel.RELIGION_ID = sReligion;
                        objModel.HOSTEL_ID = sHostel;
                        objModel.IS_ROMAN_CATHOLIC = sApplicantType;
                        objModel.APPLICATION_TYPE_ID = sApplicationType;
                        objModel.STATUS = Common.STATUS.Selected;
                        if (sApplicantType == "1")
                        {
                            objViewModel.liApplicant = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentListByMinorityForHostelSelection), sAcademicYear).DataSource.Data;
                        }
                        else
                        {
                            sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentListByReligionForHostelSelection).Replace(Common.Delimiter.QUS + Common.SUP_RELIGION.RELIGION_ID, sReligion);
                            objViewModel.liApplicant = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(objModel, sSql, sAcademicYear).DataSource.Data;
                        }
                        //Fetch Hostel Total str,selected,paid
                        var HostelDetails = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(new SUP_HOSTEL() { HOSTEL_ID = sHostel }, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHostelDetails)).DataSource.Data;
                        var HostelSelected = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHostelSelected), sAcademicYear).DataSource.Data;
                        objModel.STATUS = Common.STATUS.Admitted;
                        var HostelAdmitted = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHostelAdmitted), sAcademicYear).DataSource.Data;
                        if (HostelDetails != null && HostelDetails.Count > 0)
                        {
                            LiHostel.Add(new SUP_HOSTEL() { HOSTEL_NAME = HostelDetails.FirstOrDefault().HOSTEL_NAME, TOTAL_STRENGTH = HostelDetails.FirstOrDefault().TOTAL_STRENGTH, ADMITTED = HostelAdmitted.FirstOrDefault().ADMITTED, SELECTED = HostelSelected.FirstOrDefault().SELECTED });
                        }
                        objViewModel.liHostel = LiHostel;

                        return View(objViewModel);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("AdmissionController", "HostelRegisteredList", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("AdmissionController", "HostelRegisteredList", ex.Message);
                            return View(objViewModel);
                        }
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.BadRequest;
                    objResult.Message = Common.ErrorMessage.BadRequest;
                    return Json(objResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
                return Json(objResult, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveHostelSelection(string sSelectionProcess)
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_RECEIVE_APPLICATION objModel = new ADM_RECEIVE_APPLICATION();
            string sContent = string.Empty;
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JSON_ADM_STU_SUBMARKS objJson = serializer.Deserialize<JSON_ADM_STU_SUBMARKS>(sSelectionProcess);
                try
                {
                    if (objJson.SAVE_HOSTEL_SELECTION != null && objJson.SAVE_HOSTEL_SELECTION.Count > 0)
                    {
                        foreach (var item in objJson.SAVE_HOSTEL_SELECTION)
                        {
                            var HostelDetails = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(new SUP_HOSTEL() { HOSTEL_ID = item.HOSTEL_ID }, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHostelDetails)).DataSource.Data;
                            item.SELECTED_BY = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                            item.STATUS = Common.STATUS.Selected;
                            var sresultArgs = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertHostelSelection), sAcademicYear);
                            objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : objResult.Message = Common.Messages.FailedToSaveRecords;
                            if (sresultArgs.Success)
                            {
                                var StudentAccount = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = item.RECEIVE_ID, PROGRAMME_GROUP_ID = item.PROGRAMME_GROUP_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelSelectedStudentInfo), sAcademicYear).DataSource.Data;
                                if (StudentAccount != null && StudentAccount.Count > 0)
                                {
                                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.HostelFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                                        item.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                                    var FetchStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = objModel.RECEIVE_ID, FREQUENCY_ID = item.FREQUENCY_ID }, FeeSQL.GetFeeSQL(FeeSQLCommands.CheckIsExistingAccount), sAcademicYear).DataSource.Data;
                                    var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.HostelSelection }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                    if (FetchStudentAccount == null)
                                    {
                                        if (HostelDetails.FirstOrDefault().IS_OUTSIDE != "1")
                                        {
                                            SaveHostelStudentAccount(item.RECEIVE_ID, item.FREQUENCY_ID);
                                        }
                                        sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, StudentAccount.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.SUP_HOSTEL.HOSTEL_NAME, StudentAccount.FirstOrDefault().HOSTEL_NAME);
                                        SendEmailAndTextMessage(sContent, StudentAccount.FirstOrDefault().SMS_MOBILE_NO);
                                    }
                                    else
                                    {
                                        objResult.Message = "Account is existing already !!";
                                    }
                                }
                                objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else objResult.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.BadRequest;
                        objResult.Message = Common.ErrorMessage.ExpectationFailed;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveHostelRegistered", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveHostelRegistered", ex.Message);
                        return Json(objResult, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
                return Json(objResult, JsonRequestBehavior.AllowGet);
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Selection Process Status List
        [UserRights("ADMIN")]
        public ActionResult ListSelectionProcessStatus()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();

            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (CycleList != null && CycleList.Count > 0)
                        objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListSelectionProcessStatus", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListSelectionProcessStatus", ex.Message);
                }
            }
            return View(objViewModel);
        }

        public ActionResult BindSelectedStudentsToCofirm(string sCycleId, string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.SELECTION_CYCLE = sCycleId;
                    objModel.STATUS = Common.STATUS.Submitted;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectedApplicationByProgrammeId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;

                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsToCofirm", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsToCofirm", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindSelectedStudents(string sCycleId, string sProgrammeId, string sPhone)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.SELECTION_CYCLE = sCycleId;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssuedApplicationByProgrammeId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    //sSQL = string.Empty;
                    //sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId);
                    //sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    //objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, sSQL, sAcademicYear).DataSource.Data;


                    if (!string.IsNullOrEmpty(sPhone))
                    {
                        ViewBag.isPhone = sPhone;
                    }

                }

                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectedStudents", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectedStudents", ex.Message);
                }
            }
            return View(objViewModel);
        }
        //public ActionResult BindSelectedStudentsForPrint(string sCycleId, string sProgrammeId)
        //{
        //    AdmissionViewModel objViewModel = new AdmissionViewModel();
        //    ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
        //    var FetchCollegeDetails = new List<CollegeDetails>();
        //    string sSQL = string.Empty;
        //    try
        //    {
        //        if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
        //        {
        //            objModel.SELECTION_CYCLE = sCycleId;
        //            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
        //            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicantsToTransfer);
        //            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
        //            objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
        //            sSQL = string.Empty;
        //            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId);
        //            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
        //            objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, sSQL, sAcademicYear).DataSource.Data;

        //            FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
        //            if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
        //                objViewModel.liCollegeDetails = FetchCollegeDetails;
        //        }
        //        else
        //            return RedirectToAction("ErrorMessage", "error");
        //    }
        //    catch (Exception ex)
        //    {

        //        using (ErrorLog objHandler = new ErrorLog())
        //        {
        //            objHandler.WriteError("AdmissionController", "BindSelectedStudentsForPrint", "Err MSg: " + ex.Message, "Query is empty!");
        //            objHandler.WriteError("AdmissionController", "BindSelectedStudentsForPrint", ex.Message);
        //        }
        //    }
        //    return View(objViewModel);
        //}
        #endregion

        #region Admitted Student List
        //[UserRights("ADMIN")]
        public ActionResult AdmittedStudentList()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    //var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objStaffModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    // if (CycleList != null && CycleList.Count > 0)
                    //   objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "AdmittedStudentList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "AdmittedStudentList", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindAdmittedStudents(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                objModel.STATUS = Common.STATUS.Admitted;
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        objModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentListReport);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                        sSQL = string.Empty;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudents", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudents", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindAdmittedStudentsForPrint(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            var FetchCollegeDetails = new List<CollegeDetails>();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.STATUS = Common.STATUS.Admitted;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        objModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentListReport);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                        sSQL = string.Empty;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, sSQL, sAcademicYear).DataSource.Data;

                        FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                        if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                            objViewModel.liCollegeDetails = FetchCollegeDetails;
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudentsForPrint", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudentsForPrint", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region Admitted Student List For Cancel
        //[UserRights("ADMIN")]
        public ActionResult ListAdmittedStudentList()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objStaffModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (CycleList != null && CycleList.Count > 0)
                        objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListAdmittedStudentList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListAdmittedStudentList", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindAdmittedStudent(string sCycleId, string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.STATUS = Common.STATUS.Admitted;
                    objModel.SELECTION_CYCLE = sCycleId;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentList);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liApplicant = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(objModel, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudent", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudent", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult AdmittedStudentView(string sIssueId, string sReceiveId)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                if (!string.IsNullOrEmpty(sIssueId))
                {
                    try
                    {
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        ObjViewModel.liIssueApplication = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { ISSUE_ID = sIssueId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentInfoByIssueId), sAcademicYear).DataSource.Data;
                        var liPaidStatus = (List<ISSUED_LIST>)CMSHandler.FetchData<ISSUED_LIST>(new ISSUED_LIST() { RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchFeePaidInfo), sAcademicYear).DataSource.Data;
                        if (liPaidStatus != null && liPaidStatus.Count > 0)
                        {
                            ObjViewModel.AMOUNT = liPaidStatus.FirstOrDefault().AMOUNT;
                            ObjViewModel.PAID = liPaidStatus.FirstOrDefault().PAID;
                        }
                        return View(ObjViewModel);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("AdmissionController", "AdmittedStudentView", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("AdmissionController", "AdmittedStudentView", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }

        }
        public JsonResult SaveAdmissionCancel(string sIssueId, string sReason, string sHostel)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    string sSQL = string.Empty;

                    ResultArgs sresultArgs = new ResultArgs();
                    var ObjCancel = new ADM_CANCEL_ADMISSION_2018();
                    if (!string.IsNullOrEmpty(sIssueId) && !string.IsNullOrEmpty(sReason))
                    {
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        var liIssueInfo = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { ISSUE_ID = sIssueId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentInfoByIssueId), sAcademicYear).DataSource.Data;
                        if (liIssueInfo != null && liIssueInfo.Count > 0)
                        {
                            ObjCancel.ACADEMIC_YEAR = sAcademicYear;
                            ObjCancel.APPLICATION_NO = liIssueInfo.FirstOrDefault().APPLICATION_NO;
                            ObjCancel.PROGRAMME_ID = liIssueInfo.FirstOrDefault().PROGRAMME_GROUP_ID;
                            ObjCancel.RECEIVE_ID = liIssueInfo.FirstOrDefault().RECEIVE_ID;
                            ObjCancel.REASON = sReason;
                            if (!string.IsNullOrEmpty(sHostel))
                                ObjCancel.IS_HOSTEL_CANCEL = sHostel;
                            else
                                ObjCancel.IS_HOSTEL_CANCEL = "0";

                            if (liIssueInfo != null & liIssueInfo.Count > 0)
                            {
                                sresultArgs = CMSHandler.SaveOrUpdate(ObjCancel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveCancelStudent), sAcademicYear, true);
                                if (sresultArgs.Success)
                                {
                                    var FetchSelection = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = ObjCancel.RECEIVE_ID, PROGRAMME_ID = ObjCancel.PROGRAMME_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectionIdByReceiveIdandProgrammeId), sAcademicYear).DataSource.Data;
                                    var FetchHostelSelection = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new ADM_SELECTION_PROCESS() { RECEIVE_ID = ObjCancel.RECEIVE_ID, PROGRAMME_ID = ObjCancel.PROGRAMME_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelSelectionIdByReceiveIdandProgrammeId), sAcademicYear).DataSource.Data;
                                    if (!string.IsNullOrEmpty(sHostel))
                                    {
                                        if (FetchHostelSelection != null && FetchHostelSelection.Count > 0)
                                        {
                                            sresultArgs = CMSHandler.SaveOrUpdate(new ADM_SELECTION_PROCESS() { HOSTEL_SELECTION_ID = FetchHostelSelection.FirstOrDefault().HOSTEL_SELECTION_ID, STATUS = Common.STATUS.Cancelled }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateHostelSelectionBySelectionId), sAcademicYear);

                                            sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FEE_MODE, String.Concat(Common.FrequencyType.HostelFee));
                                            var Fetcfrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(null, sSQL, sAcademicYear).DataSource.Data;
                                            if (Fetcfrequency != null && Fetcfrequency.Count > 0)
                                            {
                                                var FrequencyId = string.Join(",", Fetcfrequency.Select(o => o.FREQUENCY_ID));
                                                sSQL = string.Empty;
                                                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateFeeStudentAc).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, FrequencyId);
                                                sresultArgs = CMSHandler.SaveOrUpdate(ObjCancel, sSQL, sAcademicYear);
                                                sSQL = string.Empty;
                                                sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTranscationIds).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, FrequencyId);
                                                var FetchTranscation = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(ObjCancel, sSQL, sAcademicYear).DataSource.Data;
                                                if (FetchTranscation != null && FetchTranscation.Count > 0)
                                                {
                                                    var TrascationIds = string.Join(",", FetchTranscation.Select(o => o.TRANSACTION_ID));
                                                    sSQL = string.Empty;
                                                    sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateTranscation).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.TRANSACTION_ID, TrascationIds);
                                                    sresultArgs = CMSHandler.SaveOrUpdate(null, sSQL, sAcademicYear);
                                                    sSQL = string.Empty;
                                                    sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateFeecollection).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.TRANSACTION_ID, TrascationIds);
                                                    sresultArgs = CMSHandler.SaveOrUpdate(null, sSQL, sAcademicYear);
                                                }

                                            }
                                        }
                                    }
                                    sSQL = string.Empty;
                                    sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FEE_MODE, String.Concat(Common.FrequencyType.AdmissionFee));
                                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(null, sSQL, sAcademicYear).DataSource.Data;
                                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                                    {
                                        var FrequencyId = string.Join(",", FetchFrequency.Select(o => o.FREQUENCY_ID));
                                        sSQL = string.Empty;
                                        sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateFeeStudentAc).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, FrequencyId);
                                        sresultArgs = CMSHandler.SaveOrUpdate(ObjCancel, sSQL, sAcademicYear);
                                        sSQL = string.Empty;
                                        sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.FetchTranscationIds).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.FREQUENCY_ID, FrequencyId);
                                        var FetchTranscation = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(ObjCancel, sSQL, sAcademicYear).DataSource.Data;
                                        if (FetchTranscation != null && FetchTranscation.Count > 0)
                                        {
                                            var TrascationIds = string.Join(",", FetchTranscation.Select(o => o.TRANSACTION_ID));
                                            sSQL = string.Empty;
                                            sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateTranscation).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.TRANSACTION_ID, TrascationIds);
                                            sresultArgs = CMSHandler.SaveOrUpdate(null, sSQL, sAcademicYear);
                                            sSQL = string.Empty;
                                            sSQL = FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateFeecollection).Replace(Common.Delimiter.QUS + Common.SUP_FEE_FREQUENCY.TRANSACTION_ID, TrascationIds);
                                            sresultArgs = CMSHandler.SaveOrUpdate(null, sSQL, sAcademicYear);
                                        }

                                    }
                                    if (FetchSelection != null & FetchSelection.Count > 0)
                                    {
                                        sresultArgs = CMSHandler.SaveOrUpdate(new ADM_ISSUED_APPLICATIONS() { SELECTION_ID = FetchSelection.FirstOrDefault().SELECTION_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateSelectionBySelectionId), sAcademicYear);

                                        sresultArgs = CMSHandler.SaveOrUpdate(new ADM_ISSUED_APPLICATIONS() { ISSUED_ID = sIssueId, STATUS = Common.STATUS.Cancelled }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateAdmIssueByIssueId));
                                        ObjJsonData.sResult = sReason;
                                    }
                                }

                            }

                            if (sresultArgs.Success)
                            {
                                ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                                ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                            }

                        }
                        else
                        {
                            ObjJsonData.Message = Common.Messages.NoRecordsfound;
                            ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                        }
                    }
                    else
                    {
                        ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                        ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveAdmissionCancel", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveAdmissionCancel", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
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

        #region CasteWiseQuata
        public ActionResult CasteWiseQuata()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        ObjViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);

                    return View(ObjViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "CasteWiseQuata", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "CasteWiseQuata", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return RedirectToAction("ErrorMessage", "Error");
        }

        public ActionResult CasteWiseQuataList(string sProgramIds)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    string sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramWiseQuata).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgramIds);
                    string sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCasteWiseQuata).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgramIds);
                    ObjViewModel.liProgram_Quata = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;
                    ObjViewModel.liCasteWise_Quata = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSql, sAcademicYear).DataSource.Data;
                    ObjViewModel.liCaste = (List<SupCommunity>)CMSHandler.FetchData<SupCommunity>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCommunity)).DataSource.Data;
                    return View(ObjViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "CasteWiseQuataList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "CasteWiseQuataList", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Admission Programme Incharge
        //[UserRights("Admin")]
        public ActionResult ProgrammeIncharge()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                AdmissionViewModel objViewModel = new AdmissionViewModel();
                ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                try
                {
                    var programmes = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (programmes != null)
                    {
                        objViewModel.ProgrammeList = new SelectList(programmes, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    }
                    objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionProgrammeIncharge), sAcademicYear).DataSource.Data;
                    return View(objViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "ProgrammeIncharge", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "ProgrammeIncharge", ex.Message);
                        return View(objViewModel);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
        }
        public JsonResult FetchStaffByProgramme(string sProgrammeGroupId)
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            ADM_APPTYPE_PROG_GROUPS ObjModel = new ADM_APPTYPE_PROG_GROUPS();
            string sOption = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sProgrammeGroupId))
                {
                    try
                    {
                        ObjModel.PROGRAMME_GROUP_ID = sProgrammeGroupId;
                        var listaff = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(ObjModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStaffListByProgramme)).DataSource.Data;
                        if (listaff != null && listaff.Count > 0)
                        {
                            foreach (var item in listaff)
                            {
                                sOption += "<option value='" + item.STAFF_ID + "'" + item.SELECTED + ">" + item.FIRST_NAME + "</option>";
                            }
                            objResult.sResult = sOption;
                        }
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("AdmissionController", "FetchStaffByProgramme", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("AdmissionController", "FetchStaffByProgramme", ex.Message);
                            return Json(objResult, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.BadRequest;
                    objResult.Message = Common.ErrorMessage.BadRequest;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveProgrammeIncharge(string sStaffIds, string sProgrammeGroupId, string sIsActive)
        {
            JsonResultData objResult = new JsonResultData();
            ADM_APPTYPE_PROG_GROUPS ObjModel = new ADM_APPTYPE_PROG_GROUPS();
            USER_ACCOUNT_INFO objUserAccount = new USER_ACCOUNT_INFO();
            List<ADM_APPTYPE_PROG_GROUPS> liProgrammechange = new List<ADM_APPTYPE_PROG_GROUPS>();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    if (!string.IsNullOrEmpty(sStaffIds) && !string.IsNullOrEmpty(sProgrammeGroupId) && !string.IsNullOrEmpty(sIsActive))
                    {
                        ObjModel.PROGRAMME_GROUP_ID = sProgrammeGroupId;
                        ObjModel.IS_ACTIVE = sIsActive;
                        ObjModel.ACADEMIC_YEAR = sAcademicYear;
                        var sStaffId = sStaffIds.Split(',');
                        var OFF = new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                        var sresultArgs = CMSHandler.SaveOrUpdate(ObjModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateInActiveByProgramme));
                        var On = new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                        if (sresultArgs.Success)
                        {
                            foreach (var item in sStaffId)
                            {
                                ObjModel.STAFF_ID = item;
                                var StaffExist = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(ObjModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStaffByProgrammeGroupId)).DataSource.Data;
                                if (StaffExist != null && StaffExist.Count > 0)
                                {
                                    ObjModel.INCHARGE_ID = StaffExist.FirstOrDefault().INCHARGE_ID;
                                    sresultArgs = CMSHandler.SaveOrUpdate(ObjModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateActiveByInChargeId));
                                }
                                else
                                {
                                    sresultArgs = CMSHandler.SaveOrUpdate(ObjModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertAdmissionIncharge));
                                    if (sresultArgs.Success)
                                    {
                                        objUserAccount.USER_ID = item;
                                        var StaffUserAccount = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(objUserAccount, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.CheckStaffExistInUserAccount)).DataSource.Data;
                                        if (StaffUserAccount != null && StaffUserAccount.Count > 0)
                                        {
                                        }
                                        else
                                        {
                                            var StaffInfo = (List<stf_Personal_Info>)CMSHandler.FetchData<stf_Personal_Info>(ObjModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStaffInfoForUserAccount)).DataSource.Data;
                                            if (StaffInfo != null && StaffInfo.Count > 0)
                                            {
                                                objUserAccount.NAME = StaffInfo.FirstOrDefault().FIRST_NAME;
                                                objUserAccount.USERNAME = StaffInfo.FirstOrDefault().STAFF_CODE.ToUpper();
                                                objUserAccount.EMAIL = "";
                                                objUserAccount.MOBILE = "";
                                                objUserAccount.USER_ID = StaffInfo.FirstOrDefault().STAFF_ID;
                                                objUserAccount.PASSWORD = CommonMethods.GetSha256FromString(objUserAccount.USERNAME + "@" + StaffInfo.FirstOrDefault().DATE_OF_BIRTH.Substring(0, 4));
                                                objUserAccount.ROLE = "4";
                                                objUserAccount.USER_TYPE = "4";
                                                sresultArgs = CMSHandler.SaveOrUpdate(objUserAccount, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.SaveUserAccount));
                                            }
                                        }
                                    }
                                }
                                objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                            }
                            objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                        }
                        objResult.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.BadRequest;
                        objResult.Message = Common.ErrorMessage.BadRequest;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveProgrammeIncharge", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveProgrammeIncharge", ex.Message);
                        objResult.ErrorCode = Common.ErrorCode.InternalServerError;
                        objResult.Message = Common.ErrorMessage.InternalServerError;
                        return Json(objResult, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateActiveProgrammeIncharge(string sInchargeId, string sActive)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sInchargeId) && !string.IsNullOrEmpty(sActive))
                {
                    ADM_APPTYPE_PROG_GROUPS ObjModel = new ADM_APPTYPE_PROG_GROUPS();
                    ObjModel.INCHARGE_ID = sInchargeId;
                    if (sActive == "0")
                        ObjModel.IS_ACTIVE = Common.IsActiveFalg.IsActive;
                    else if (sActive == "1")
                        ObjModel.IS_ACTIVE = Common.IsActiveFalg.IsNotActive;
                    try
                    {
                        var sresultArgs = CMSHandler.SaveOrUpdate(ObjModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateActiveByInChargeId));
                        objResult.Message = (sresultArgs.Success) ? "Active State Changed Successfully..!" : "Failed to Change Active State..!";
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("AdmissionController", "UpdateActiveProgrammeIncharge", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("AdmissionController", "UpdateActiveProgrammeIncharge", ex.Message);
                        }
                        return Json(objResult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.BadRequest;
                    objResult.Message = Common.ErrorMessage.BadRequest;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Hostel Application Print View
        public ActionResult HostelRegistrationPrintView()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.ISSUE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    objModel.RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    // objViewModel.liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentDetailsForPrintView), sAcademicYear).DataSource.Data;
                    objViewModel.liADMStudentStudentRelations = (List<ADM_STU_RELATIONS>)CMSHandler.FetchData<ADM_STU_RELATIONS>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchRelastionsInfoForPrint), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "HostelRegistrationPrintView", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "HostelRegistrationPrintView", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region Transfered Refund and Transactions
        [UserRights("ADMISSION ADMIN")]
        public ActionResult TransferedListForTransaction()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objViewModel.liTransfer = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchTransferedStudentsForTransaction), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "TransferedListForTransaction", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "TransferedListForTransaction", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult ViewStudentFeeDetailsForTransfer(string sReceiveId, string sTransferId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ResultArgs sresultArgs = new ResultArgs();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            ADM_TRANSFER objTransferModel = new ADM_TRANSFER();

            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    objTransferModel.TRANSFER_ID = sTransferId;
                    objViewModel.Transfer.TRANSFER_ID = sTransferId;
                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                        objModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                    var Transfer = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(objTransferModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchTransferById), sAcademicYear).DataSource.Data;
                    if (Transfer != null && Transfer.Count > 0)
                        objModel.PROGRAMME_ID = Transfer.FirstOrDefault().PROGRAMME_TO;
                    objViewModel.liFeeDetails = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchFeeDetailsByProgramme), sAcademicYear).DataSource.Data;
                    var Total = (objViewModel.liFeeDetails.Select(x => Convert.ToInt32(x.AMOUNT)).Sum());
                    objViewModel.Issue.HSTOTAL = Convert.ToString(Total);
                    objModel.RECEIVE_ID = sReceiveId;
                    objModel.STUDENT_ID = sReceiveId;
                    // objViewModel.liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicantsInfoForTransfer), sAcademicYear).DataSource.Data;
                    objViewModel.liFeeStructure = (List<FEE_STRUCTURE>)CMSHandler.FetchData<FEE_STRUCTURE>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentId), sAcademicYear).DataSource.Data;
                    objViewModel.liCreditAndDebit = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(objModel, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchCreditAndDebitByStudentId), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ViewStudentFeeDetailsForTransfer", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ViewStudentFeeDetailsForTransfer", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult FeeRefundAndCreateStudentAccount(string sTransfer)
        {
            JsonResultData objResult = new JsonResultData();
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            ADM_TRANSFER objTransferModel = new ADM_TRANSFER();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var Result = JsonConvert.DeserializeObject<ADM_TRANSFER>(sTransfer);
                    int Paid = 0;
                    if (Result != null)
                    {
                        objTransferModel.TRANSFER_ID = Result.TRANSFER_ID;
                        var Transfer = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(objTransferModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchTransferById), sAcademicYear).DataSource.Data;
                        if (Transfer != null && Transfer.Count > 0)
                            objModel.PROGRAMME_GROUP_ID = Transfer.FirstOrDefault().PROGRAMME_TO;
                        var FetchProgrammeAndClass = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeAndClassByProgrammeGroupId), sAcademicYear).DataSource.Data;
                        if (FetchProgrammeAndClass != null && FetchProgrammeAndClass.Count > 0)
                        {
                            objModel.RECEIVE_ID = Result.RECEIVE_ID;
                            objModel.ISSUE_ID = Result.ISSUE_ID;
                            objModel.CLASS_ID = FetchProgrammeAndClass.FirstOrDefault().CLASS_ID;
                            objModel.PROGRAMME_ID = FetchProgrammeAndClass.FirstOrDefault().PROGRAMME_ID;
                            // UPDATE PROGRAMME AND CLASS
                            var UpdateIssue = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateIssueProgrammeAndProgrammeGroupIdForTransfer));
                            var UpdateReceive = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateReceiveProgrammeAndProgrammeGroupIdForTransfer), sAcademicYear);
                            var UpdateSelectionProcess = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateSelectionProcessdProgrammeGroupIdForTransfer), sAcademicYear);
                            var UpdateHostelRegistration = CMSHandler.SaveOrUpdate(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateHostelRegistrationClassForTransfer), sAcademicYear);
                            // FETCH FEE TRANSACTION
                            var Transaction = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(Result, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeTransactionIdByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
                            if (Transaction != null && Transaction.Count > 0)
                            {
                                foreach (var item in Transaction)
                                {
                                    // DELETED TRANSACTION
                                    var DeleteFeeTransaction = CMSHandler.SaveOrUpdate(item, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeTransaction), sAcademicYear);
                                    var DeleteFeeCollection = CMSHandler.SaveOrUpdate(item, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeCollection), sAcademicYear);
                                    var DeleteFeeStudentAccount = CMSHandler.SaveOrUpdate(item, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeStudentAccount), sAcademicYear);
                                    // Save Deleted Account
                                    if (DeleteFeeStudentAccount.Success)
                                    {
                                        FEE_COLLECTION_DELETED objDeletedAccountModel = new FEE_COLLECTION_DELETED();
                                        var FeeTransaction = (List<FEE_COLLECTION_DELETED>)CMSHandler.FetchData<FEE_COLLECTION_DELETED>(item, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeTransactionById), sAcademicYear).DataSource.Data;
                                        if (FeeTransaction != null && FeeTransaction.Count > 0)
                                        {
                                            objDeletedAccountModel.NAME = FeeTransaction.FirstOrDefault().NAME;
                                            objDeletedAccountModel.TRANSACTION_ID = FeeTransaction.FirstOrDefault().TRANSACTION_ID;
                                            objDeletedAccountModel.RECEIPT_NO = FeeTransaction.FirstOrDefault().RECEIPT_NO;
                                            objDeletedAccountModel.CLASS = objModel.CLASS_ID;
                                            objDeletedAccountModel.AMOUNT = FeeTransaction.FirstOrDefault().AMOUNT;
                                            objDeletedAccountModel.PAYMENT_MODE = FeeTransaction.FirstOrDefault().PAYMENT_MODE;
                                            var SaveDeletedAccount = CMSHandler.SaveOrUpdate(objDeletedAccountModel, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.SaveDeletedAccount), sAcademicYear);
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            var DeleteFeeStudentAccountOnlyCredit = CMSHandler.SaveOrUpdate(Result, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeStudentAccountOnlyCredit), sAcademicYear);
                        }
                        var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                        if (FetchFrequency != null && FetchFrequency.Count > 0)
                            Result.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                        SaveStudentAccount(Result.RECEIVE_ID, Result.FREQUENCY_ID);
                        Paid = Convert.ToInt32(Result.AMOUNT);
                        if (Paid > 0)
                        {
                            var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { FREQUENCY_ID = Result.FREQUENCY_ID, STUDENT_ID = Result.RECEIVE_ID }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
                            if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                            {
                                int ReceiptNo = 0;
                                string Prefix = "00";
                                FEE_STUDENT_ACCOUNT objStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                                FEE_TRANSACTION objFeeTransactionModel = new FEE_TRANSACTION();
                                AUTO_GENERATE_NUMBERS objAutoGenerateNumber = new AUTO_GENERATE_NUMBERS();
                                objFeeTransactionModel.STUDENT_ID = Result.RECEIVE_ID;
                                objFeeTransactionModel.CLASS = objModel.CLASS_ID;
                                objFeeTransactionModel.FREQUENCY = Result.FREQUENCY_ID;
                                objFeeTransactionModel.FREQUENCY_TO = Result.FREQUENCY_ID;
                                objFeeTransactionModel.PAYMENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                                objFeeTransactionModel.USERNAME = Transfer.FirstOrDefault().APPLICATION_NO;
                                objFeeTransactionModel.COLLECTED = Paid.ToString();
                                var FetchReceiptNo = (List<AUTO_GENERATE_NUMBERS>)CMSHandler.FetchData<AUTO_GENERATE_NUMBERS>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAutoGenerateNumber)).DataSource.Data;
                                if (FetchReceiptNo != null && FetchReceiptNo.Count > 0)
                                    ReceiptNo = Convert.ToInt32(FetchReceiptNo.FirstOrDefault().exam_receipt_no);
                                else
                                {
                                    // objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                    //   objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                }
                                objAutoGenerateNumber.exam_receipt_no = Convert.ToString(ReceiptNo + 1);
                                objAutoGenerateNumber.exam_receipt_no = Prefix + objAutoGenerateNumber.exam_receipt_no;
                                var UpdateReceiptNo = CMSHandler.SaveOrUpdate(objAutoGenerateNumber, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateAutoGenerateNumber));
                                if (UpdateReceiptNo.Success)
                                {
                                    objFeeTransactionModel.PAYMENT_MODE = Common.PaymentMode.Online;
                                    objFeeTransactionModel.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                    var SaveFeeTransaction = CMSHandler.SaveOrUpdate(objFeeTransactionModel, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeTransaction), sAcademicYear, true);
                                    if (SaveFeeTransaction.Success)
                                    {
                                        decimal dAmount = Convert.ToDecimal(Paid);
                                        decimal dCredit = 0;
                                        string sTransactionId = SaveFeeTransaction.RowUniqueId.ToString();
                                        foreach (var item in FetchFeeStudentAccount)
                                        {
                                            if (dAmount > 0 && Convert.ToDecimal(item.BALANCE) > 0)
                                            {
                                                dCredit = Convert.ToDecimal(item.CREDIT);
                                                item.DEBIT = (dAmount >= dCredit) ? item.CREDIT : (dAmount).ToString();
                                                dAmount = dAmount - dCredit;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                            item.TRANSACTION_ID = sTransactionId;
                                            item.FREQUENCY = item.FREQUENCY_ID;
                                            item.PAID_AMOUNT = item.DEBIT;
                                            item.RECEIPT_NO = objFeeTransactionModel.RECEIPT_NO;
                                            item.HEAD = item.HEAD_ID;

                                            var SaveFeeCollection = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeCollection));
                                            if (SaveFeeCollection.Success)
                                            {
                                                var SaveStudentAccount = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeStudentAccount), sAcademicYear);
                                                if (SaveStudentAccount.Success)
                                                {

                                                }
                                                else
                                                {
                                                    //objResult.Message = Common.Messages.FailedToSaveRecords;
                                                }
                                            }
                                        }
                                        new MySQLHandler().ExecuteAsScripts(SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAdmissionFeeUpdates).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, Result.RECEIVE_ID).Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FeeRefundAndCreateStudentAccount", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FeeRefundAndCreateStudentAccount", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region OfflineCollection
        public ActionResult BindFeeCollectionByReceivedId(string sReceiveId, string sFrequencyId)
        {
            List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                if (!string.IsNullOrEmpty(sReceiveId) && !string.IsNullOrEmpty(sFrequencyId))
                {
                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = sReceiveId, FREQUENCY_ID = sFrequencyId }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfoByReceiveIdAndFrequencyIdByMainHead), sAcademicYear).DataSource.Data;
                }
            }
            return View(studentAcountList);
        }
        public ActionResult PayInOffline(string sReceiveId, string sFrequencyId)
        {
            List<FEE_STUDENT_ACCOUNT> studentAcountList = new List<FEE_STUDENT_ACCOUNT>();
            FEE_STUDENT_ACCOUNT ObjModel = new FEE_STUDENT_ACCOUNT();
            string sStudentId = string.Empty;
            string sProgrammeGroupId = string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                if (!string.IsNullOrEmpty(sReceiveId) && !string.IsNullOrEmpty(sFrequencyId))
                {
                    studentAcountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = sReceiveId, FREQUENCY_ID = sFrequencyId }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfoByReceiveIdAndFrequencyId), sAcademicYear).DataSource.Data;
                    ObjModel.StudentAccountList = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { STUDENT_ID = sReceiveId, FREQUENCY_ID = sFrequencyId }, SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchStudentAccountFeeInfoByReceiveIdAndFrequencyIdByMainHead), sAcademicYear).DataSource.Data;
                    if (studentAcountList != null && studentAcountList.Count > 0)
                    {
                        sStudentId = sReceiveId;
                        var FetchFeeStudentAccount = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_STUDENT_ACCOUNT() { FREQUENCY_ID = sFrequencyId, STUDENT_ID = sStudentId }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId), sAcademicYear).DataSource.Data;
                        if (FetchFeeStudentAccount != null && FetchFeeStudentAccount.Count > 0)
                        {
                            int ReceiptNo = 0;
                            string Prefix = "00";
                            string TransactionId = string.Empty;
                            string sContent = string.Empty;
                            FEE_STUDENT_ACCOUNT objStudentAccountModel = new FEE_STUDENT_ACCOUNT();
                            FEE_TRANSACTION objFeeTransactionModel = new FEE_TRANSACTION();
                            AUTO_GENERATE_NUMBERS objAutoGenerateNumber = new AUTO_GENERATE_NUMBERS();

                            decimal balance = 0;
                            decimal.TryParse(FetchFeeStudentAccount.FirstOrDefault().BALANCE, out balance);
                            if (balance > 0)
                            {
                                objFeeTransactionModel.STUDENT_ID = sStudentId;
                                objFeeTransactionModel.CLASS = studentAcountList.FirstOrDefault().PROGRAMME_GROUP_ID;
                                sProgrammeGroupId = studentAcountList.FirstOrDefault().PROGRAMME_GROUP_ID;
                                objFeeTransactionModel.PAYMENT_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                                objFeeTransactionModel.USERNAME = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                                objFeeTransactionModel.FREQUENCY = sFrequencyId;
                                objFeeTransactionModel.FREQUENCY_TO = sFrequencyId;
                                objFeeTransactionModel.COLLECTED = FetchFeeStudentAccount.Sum(o => Convert.ToDecimal(o.BALANCE)).ToString();
                                var FetchReceiptNo = (List<AUTO_GENERATE_NUMBERS>)CMSHandler.FetchData<AUTO_GENERATE_NUMBERS>(null, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAutoGenerateNumber)).DataSource.Data;
                                if (FetchReceiptNo != null && FetchReceiptNo.Count > 0)
                                    ReceiptNo = Convert.ToInt32(FetchReceiptNo.FirstOrDefault().exam_receipt_no);
                                else
                                {
                                    // objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                    //   objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                }
                                objAutoGenerateNumber.exam_receipt_no = Convert.ToString(ReceiptNo + 1);
                                objAutoGenerateNumber.exam_receipt_no = Prefix + objAutoGenerateNumber.exam_receipt_no;
                                var UpdateReceiptNo = CMSHandler.SaveOrUpdate(objAutoGenerateNumber, FeeSQL.GetFeeSQL(FeeSQLCommands.UpdateAutoGenerateNumber));
                                if (UpdateReceiptNo.Success)
                                {
                                    objFeeTransactionModel.PAYMENT_MODE = Common.PaymentMode.Cash;
                                    objFeeTransactionModel.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                    studentAcountList.FirstOrDefault().RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                    ObjModel.RECEIPT_NO = objAutoGenerateNumber.exam_receipt_no;
                                    var SaveFeeTransaction = CMSHandler.SaveOrUpdate(objFeeTransactionModel, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveOfflineFeeTransaction), sAcademicYear, true);
                                    if (SaveFeeTransaction.Success)
                                    {
                                        TransactionId = SaveFeeTransaction.RowUniqueId.ToString();
                                        //   feePayUResponse.txnid = TransactionId;
                                        decimal dAmount = FetchFeeStudentAccount.Sum(o => Convert.ToDecimal(o.BALANCE));
                                        decimal dCredit = 0;
                                        foreach (var item in FetchFeeStudentAccount)
                                        {
                                            if (dAmount > 0 && Convert.ToDecimal(item.BALANCE) > 0)
                                            {
                                                dCredit = Convert.ToDecimal(item.BALANCE);
                                                item.DEBIT = (dAmount >= dCredit) ? item.BALANCE : (dCredit - dAmount).ToString();
                                                dAmount = dAmount - dCredit;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                            item.FREQUENCY = item.FREQUENCY_ID;
                                            item.TRANSACTION_ID = TransactionId;
                                            item.PAID_AMOUNT = item.DEBIT;
                                            item.RECEIPT_NO = objFeeTransactionModel.RECEIPT_NO;
                                            item.HEAD = item.HEAD_ID;
                                            item.FEE_MAIN_HEAD_ID = item.FEE_MAIN_HEAD_ID;
                                            var SaveFeeCollection = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeCollection));
                                            if (SaveFeeCollection.Success)
                                            {
                                                var SaveStudentAccount = CMSHandler.SaveOrUpdate(item, FeeSQL.GetFeeSQL(FeeSQLCommands.SaveFeeStudentAccount), sAcademicYear);
                                                if (SaveStudentAccount.Success)
                                                {

                                                }
                                                else
                                                {
                                                    //objResult.Message = Common.Messages.FailedToSaveRecords;
                                                }
                                            }
                                        }
                                        sContent = @"Dear " + studentAcountList.FirstOrDefault().FIRST_NAME + ",\n\nThank you for using offline payment.\nRs." + FetchFeeStudentAccount.Sum(o => Convert.ToDecimal(o.BALANCE)).ToString() + " is credited successfully. \nYou have to be present with parents on the respective departments at St. Mary's College by tomorrow. with available original certificate to complete admission procedure.,"
                                                                                         + "\n For any query use help lines:\n \nAdmission:0461-2320946\nEmail:admission@stmaryscollege.edu.in \nBest Wishes,\nAdmission Team,\nSt. Mary's College,Thoothukudi.";
                                        //SendEmailAndTextMessage(sContent, studentAcountList.FirstOrDefault().SMS_MOBILE_NO, string.Empty, "Admission Fee Status From St. Mary's College,Thoothukudi.");
                                        if (sFrequencyId == Common.FrequencyType.AdmissionFee)
                                        {
                                            new MySQLHandler().ExecuteAsScripts(SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.FetchAdmissionFeeUpdates).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, sStudentId).Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sProgrammeGroupId));
                                        }
                                        else
                                        {
                                            new MySQLHandler().ExecuteAsScripts(SQL.Fee.FeeSQL.GetFeeSQL(FeeSQLCommands.HostelFeeUpdates).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, sStudentId).Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear));
                                        }


                                        studentAcountList.FirstOrDefault().STATUS = "SUCCESS";
                                        ObjModel.STATUS = "SUCCESS";
                                        studentAcountList.FirstOrDefault().ERROR_MESSAGE = "Please note Transaction Id and enquire with admin !!";
                                    }
                                    else
                                    {
                                        // objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                        //  objResult.Message = Common.ErrorMessage.ExpectationFailed;
                                    }
                                }
                            }
                            else
                            {
                                studentAcountList.FirstOrDefault().STATUS = "Pending";
                                ObjModel.STATUS = "Pending";
                                studentAcountList.FirstOrDefault().ERROR_MESSAGE = "Please note Transaction Id and enquire with admin !!";
                            }
                        }
                        else
                        {
                            studentAcountList.FirstOrDefault().STATUS = "Pending";
                            ObjModel.STATUS = "Pending";
                            studentAcountList.FirstOrDefault().ERROR_MESSAGE = "Account is not created, please meet ERP admin !!";
                        }
                    }
                    else
                    {
                        studentAcountList.FirstOrDefault().STATUS = "Failed";
                        ObjModel.STATUS = "Failed";
                        studentAcountList.FirstOrDefault().ERROR_MESSAGE = "Account is not created, please meet ERP admin !!";
                    }
                }
                else
                {
                    studentAcountList.FirstOrDefault().STATUS = "Failed";
                    ObjModel.STATUS = "Failed";
                    studentAcountList.FirstOrDefault().ERROR_MESSAGE = "Missing Dependancy";
                }
            }
            return View(ObjModel);
        }
        #endregion

        #region UnPaidStudentList
        [UserRights("ADMIN")]
        public ActionResult UnPaidStudent()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    var liCycle = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    if (liCycle != null && liCycle.Count > 0)
                        ObjViewModel.CycleList = new SelectList(liCycle, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    return View(ObjViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "ListSelectionProcessStatus", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "ListSelectionProcessStatus", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }

        public ActionResult FetchUnPaidStudentList(string sCycle)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                if (!string.IsNullOrEmpty(sCycle))
                {
                    try
                    {
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        ObjViewModel.liSelectionProccess = (List<SELECTIONPROCCESS>)CMSHandler.FetchData<SELECTIONPROCCESS>(new SELECTIONPROCCESS() { SELECTION_CYCLE = sCycle }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUnPaidStudentListByCycle), sAcademicYear).DataSource.Data;
                        return View(ObjViewModel);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("AdmissionController", "ListSelectionProcessStatus", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("AdmissionController", "ListSelectionProcessStatus", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdatePendingStatus(string sReceiveId)
        {
            sReceiveId = sReceiveId.TrimEnd(',');
            string sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusByReceiveId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, sReceiveId);
            var UpdatePendingStatus = CMSHandler.SaveOrUpdate(new ADM_ISSUE_APPLICATION_2018() { RECEIVE_ID = sReceiveId, STATUS = Common.STATUS.Pending }, sSql);
            ObjJsonData.Message = (UpdatePendingStatus.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Overall Status For Admission       
        public ActionResult OverAllStatusForAdmission()
        {
            var ObjViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var ProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode), sAcademicYear).DataSource.Data;
                    if (ProgrammeMode != null && ProgrammeMode.Count > 0)
                    {
                        ObjViewModel.ProgrammeMode = new SelectList(ProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                    }
                    var liShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                    if (liShift != null && liShift.Count > 0)
                    {
                        ObjViewModel.ShiftList = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    }
                    var liApp_Type = (List<ADM_APPLICATION_TYPE>)CMSHandler.FetchData<ADM_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                    if (liApp_Type != null && liApp_Type.Count > 0)
                    {
                        ObjViewModel.ApplicationTypeList = new SelectList(liApp_Type, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE);
                    }
                    return View(ObjViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Admission", "OverAllStatusForAdmission", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("Admission", "OverAllStatusForAdmission", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return RedirectToAction("ErrorMessage", "Error");
        }
        public ActionResult BindOverAllAdmissionStatus(string sJsonProgramId)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                if (!string.IsNullOrEmpty(sJsonProgramId))
                {
                    try
                    {
                        var ObjViewModel = new AdmissionViewModel();
                        ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

                        // Registered List ...
                        objModel.STATUS = string.Empty;
                        objModel.STATUS = Common.STATUS.Registered;
                        string sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetOverAllStatusByProgrammeId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sJsonProgramId);
                        ObjViewModel.liRegistered = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSql, sAcademicYear).DataSource.Data;

                        // Submitted List ...
                        objModel.STATUS = string.Empty;
                        objModel.STATUS = Common.STATUS.Submitted;
                        sSql = string.Empty;
                        sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetOverAllStatusByProgrammeId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sJsonProgramId);
                        ObjViewModel.liSubmitted = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSql, sAcademicYear).DataSource.Data;

                        // Selected List ...
                        objModel.STATUS = string.Empty;
                        objModel.STATUS = Common.STATUS.Selected;
                        sSql = string.Empty;
                        sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetOverAllStatusByProgrammeId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sJsonProgramId);
                        ObjViewModel.liSelected = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSql, sAcademicYear).DataSource.Data;

                        // Verified List ...
                        objModel.STATUS = string.Empty;
                        objModel.STATUS = Common.STATUS.Verified;
                        sSql = string.Empty;
                        sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetOverAllStatusByProgrammeId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sJsonProgramId);
                        ObjViewModel.liVerified = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSql, sAcademicYear).DataSource.Data;

                        // Admitted List ...
                        objModel.STATUS = string.Empty;
                        objModel.STATUS = Common.STATUS.Admitted;
                        sSql = string.Empty;
                        sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetOverAllStatusByProgrammeId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sJsonProgramId);
                        ObjViewModel.liAdmitted = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSql, sAcademicYear).DataSource.Data;

                        // Transfered List ...
                        objModel.STATUS = string.Empty;
                        objModel.STATUS = Common.STATUS.Transfer;
                        sSql = string.Empty;
                        sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetOverAllStatusByProgrammeId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sJsonProgramId);
                        ObjViewModel.liTransfered = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSql, sAcademicYear).DataSource.Data;

                        // Pending List ...
                        objModel.STATUS = string.Empty;
                        objModel.STATUS = Common.STATUS.Pending;
                        sSql = string.Empty;
                        sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetOverAllStatusByProgrammeId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, sJsonProgramId);
                        ObjViewModel.liPending = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, sSql, sAcademicYear).DataSource.Data;

                        // Fetch Program For DropDown List ....
                        sSql = string.Empty;
                        sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sJsonProgramId);
                        ObjViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, sSql, sAcademicYear).DataSource.Data;
                        return View(ObjViewModel);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("AdmissionController", "BindOverAllAdmissionStatus", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("AdmissionController", "BindOverAllAdmissionStatus", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);

            }

        }
        #endregion

        #region Date Wise Fee Paid and Verified Status
        public ActionResult DateWiseVerifiedAndPaidStatus()
        {
            return View();
        }
        public ActionResult BindDateWiseVerifiedAndPaidStatus(string sDateFrom, string sDateTo)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var objFeeStudentAccountModel = new SELECTIONPROCCESS();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objFeeStudentAccountModel.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(sDateFrom);
                    objFeeStudentAccountModel.DATE_TO = CommonMethods.ConvertdatetoyearFromat(sDateTo);
                    var FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                    var FetchFeePaidAndVerifiedList = (List<SELECTIONPROCCESS>)CMSHandler.FetchData<SELECTIONPROCCESS>(objFeeStudentAccountModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchDateWiseFeePaidAndVerifiedStatus), sAcademicYear).DataSource.Data;
                    if (FetchFeePaidAndVerifiedList != null && FetchFeePaidAndVerifiedList.Count > 0)
                        objViewModel.liSelectionProccess = FetchFeePaidAndVerifiedList;
                    if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                        objViewModel.liCollegeDetails = FetchCollegeDetails;
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindDateWiseVerifiedAndPaidStatus", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindDateWiseVerifiedAndPaidStatus", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        #endregion
        #region ApplicationTransfer
        public ActionResult ApplicationTransfer()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            //  stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    // objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    //var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    //if (CycleList != null && CycleList.Count > 0)
                    //    objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ApplicantProgrammeTransfer", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ApplicantProgrammeTransfer", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult FetchSubmittedStudentByProgrammeGroup(string ProgrammeGroupId)
        {
            ADM_ISSUE_APPLICATION_2018 ObjModel = new ADM_ISSUE_APPLICATION_2018();
            JsonResultData objResult = new JsonResultData();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    ObjModel.PROGRAMME_GROUP_ID = ProgrammeGroupId;
                    ObjModel.STATUS = Common.STATUS.Submitted;
                    var Student = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(ObjModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSubmittedStudentForTransfer), sAcademicYear).DataSource.Data;
                    if (Student != null && Student.Count > 0)
                    {
                        foreach (var item in Student)
                            objResult.sResult += "<option value='" + item.RECEIVE_ID + "'>" + item.FIRST_NAME + "</option>";
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FetchFrequencyByAcademicYear", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FetchFrequencyByAcademicYear", "Fee", ex.Message);
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BindApplicantionProgrammeTransfer(string sReceiveId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            ADM_APPTYPE_PROG_GROUPS objProgrammes = new ADM_APPTYPE_PROG_GROUPS();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    //objModel.RECEIVE_ID = sReceiveId;
                    //string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    //objViewModel.liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssueApplicationByReceivedId), sAcademicYear).DataSource.Data;
                    //if (objViewModel.liApplicant != null && objViewModel.liApplicant.Count > 0)
                    //{
                    //    objModel.HSC_NO = objViewModel.liApplicant.FirstOrDefault().HSC_NO;
                    //    objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetAppliedProgrammeByHSCNO), sAcademicYear).DataSource.Data;
                    //    objProgrammes.SHIFT = objViewModel.liApplicant.FirstOrDefault().SHIFT;
                    //    objProgrammes.PROGRAMME_MODE = objViewModel.liApplicant.FirstOrDefault().PROGRAMME_MODE;
                    //    objProgrammes.APPTYPE_ID = objViewModel.liApplicant.FirstOrDefault().APPLICATION_TYPE_ID;
                    //    var Programme = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objProgrammes, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeGroupByApplicationTypeAndProgrammeMode), sAcademicYear).DataSource.Data;
                    //    if (Programme != null && Programme.Count > 0)
                    //        objViewModel.ProgrammeList = new SelectList(Programme, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    //    var StudentExist = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectionProcessByHSCNo), sAcademicYear).DataSource.Data;
                    //    if (StudentExist != null && StudentExist.Count > 0)
                    //    {
                    //        objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetSelectedProgrammeByHSCNO), sAcademicYear).DataSource.Data;
                    //    }
                    //}
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessApplicantList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessApplicantList", ex.Message);
                }
            }
            return View(objViewModel);
        }


        public JsonResult SaveAdmissionProgrammeTransfer(string sADMTransfer)
        {
            JsonResultData ObjResult = new JsonResultData();
            ADM_APPTYPE_PROG_GROUPS objModel = new ADM_APPTYPE_PROG_GROUPS();
            ADM_TRANSFER objTransfer = new ADM_TRANSFER();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    var Result = JsonConvert.DeserializeObject<ADM_TRANSFER>(sADMTransfer);
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    Result.REQUEST_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var sresultArgs = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmTransfer), sAcademicYear);
                    if (sresultArgs.Success)
                    {
                        objTransfer.PROGRAMME_ID = Result.PROGRAMME_TO;
                        var ProgramByProgramGroupId = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId), sAcademicYear).DataSource.Data;
                        if (ProgramByProgramGroupId != null && ProgramByProgramGroupId.Count > 0)
                        {
                            Result.PROGRAMME_ID = ProgramByProgramGroupId.FirstOrDefault().PROGRAMME_ID;
                            Result.PROGRAMME_GROUP_ID = Result.PROGRAMME_TO;
                            Result.PROGGROUP_ID = Result.PROGRAMME_TO;
                            sresultArgs = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateProgrammeForTransferByIssueId));
                            sresultArgs = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateProgrammeForTransferByReceiveId));
                        }
                        if (sresultArgs.Success)
                            ObjResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        else
                            ObjResult.Message = Common.Messages.FailedToSaveRecords;
                    }

                }
                else
                {
                    ObjResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    ObjResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveAdmProgrammeTransfer", "Err MSg: " + ex.Message, "Query is empty!");
                }
                ObjResult.Message = Common.Messages.FailedToSaveRecords;
            }
            return Json(ObjResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Overall Status Fee Paid And Verified
        [UserRights("ADMIN")]
        public ActionResult OverAllStatusForFeePaidAndVerified()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    var ObjViewModel = new AdmissionViewModel();
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var liProgram = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (liProgram != null)
                    {
                        ObjViewModel.ProgrammeList = new SelectList(liProgram, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    }
                    return View(ObjViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "OverAllStatusForFeePaidAndVerified", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "OverAllStatusForFeePaidAndVerified", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }
        public ActionResult BindOverAllStatusForFeePaidAndVerified(string sJsonProgramId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var objFeeStudentAccountModel = new SELECTIONPROCCESS();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    var FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                    string sSql = string.Empty;
                    sSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOverAllFeePaidAndVerifiedStatus).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sJsonProgramId);
                    objViewModel.liSelectionProccess = (List<SELECTIONPROCCESS>)CMSHandler.FetchData<SELECTIONPROCCESS>(objFeeStudentAccountModel, sSql, sAcademicYear).DataSource.Data;
                    if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                        objViewModel.liCollegeDetails = FetchCollegeDetails;
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindOverAllStatusForFeePaidAndVerified", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindOverAllStatusForFeePaidAndVerified", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        #endregion

        #region Selected List For Hostel      
        public ActionResult ListSelectionProcessStatusForHostel()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var HostelList = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHostel)).DataSource.Data;
                    if (HostelList != null && HostelList.Count > 0)
                        objViewModel.HostelList = new SelectList(HostelList, Common.SUP_HOSTEL.HOSTEL_ID, Common.SUP_HOSTEL.HOSTEL_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListSelectionProcessStatusForHostel", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListSelectionProcessStatusForHostel", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindSelectedStudentsForHostel(string sHostelId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.STATUS = Common.STATUS.Submitted;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectedStudentsForHostel);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_HOSTEL.HOSTEL_ID, sHostelId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsForHostel", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsForHostel", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindSelectedStudentsForHostelForPrint(string sHostelId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            var FetchCollegeDetails = new List<CollegeDetails>();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objModel.STATUS = Common.STATUS.Submitted;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectedStudentsForHostel);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_HOSTEL.HOSTEL_ID, sHostelId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    sSQL = string.Empty;
                    sSQL = SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHostelByHostelIds);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.SUP_HOSTEL.HOSTEL_ID, sHostelId);
                    objViewModel.liHostel = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    //FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                    //if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                    //    objViewModel.liCollegeDetails = FetchCollegeDetails;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsForHostelForPrint", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsForHostelForPrint", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region ProgrammeMode Wise Admitted Student List
        [UserRights("ADMIN")]
        public ActionResult ProgrammeModeWiseAdmittedStudentList()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode)).DataSource.Data;
                    var FetchShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                    if (FetchProgrammeMode != null && FetchProgrammeMode.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(FetchProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                    if (FetchShift != null && FetchShift.Count > 0)
                        objViewModel.ShiftList = new SelectList(FetchShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ProgrammeModeWiseAdmittedStudentList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ProgrammeModeWiseAdmittedStudentList", ex.Message);
                }
            }
            return View();
        }
        #endregion
        #region Hostel Selected Student list By Programme 
        [UserRights("ADMIN")]
        public ActionResult ListSelectedStudentListForHostelByProgramme()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objStaffModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListSelectedStudentListForHostelByProgramme", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListSelectedStudentListForHostelByProgramme", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindSelectedStudentsForHostelByProgramme(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectedStudentsForHostelByProgramme);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsForHostelByProgramme", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsForHostelByProgramme", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindSelectedStudentsForPrintForHostel(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            var FetchCollegeDetails = new List<CollegeDetails>();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectedStudentsForHostelByProgramme);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, sSQL, sAcademicYear).DataSource.Data;

                    FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                    if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                        objViewModel.liCollegeDetails = FetchCollegeDetails;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsForPrintForHostel", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectedStudentsForPrintForHostel", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion
        #region Hostel Selected Student list By Programme 
        [UserRights("ADMIN")]
        public ActionResult ListAdmittedStudentListForHostelByProgramme()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objStaffModel, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PRO_GROUPS_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListAdmittedStudentListForHostelByProgramme", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListAdmittedStudentListForHostelByProgramme", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindAdmittedStudentsForHostelByProgramme(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.HostelFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        objModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentsForHostelByProgramme);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                        sSQL = string.Empty;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudentsForHostelByProgramme", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudentsForHostelByProgramme", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindAdmittedStudentsForPrintForHostel(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            var FetchCollegeDetails = new List<CollegeDetails>();
            string sSQL = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.HostelFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        objModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentsForHostelByProgramme);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                        sSQL = string.Empty;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgramByProGroupId);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, sSQL, sAcademicYear).DataSource.Data;

                        FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
                        if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                            objViewModel.liCollegeDetails = FetchCollegeDetails;
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudentsForHostelByProgramme", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudentsForHostelByProgramme", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region Admission Cancel List
        [UserRights("ADMIN")]
        public ActionResult AdmissionCanceledList()
        {
            AdmissionViewModel ObjViewModel = new AdmissionViewModel();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    ObjViewModel.liAdmCancelAdmission = (List<ADM_CANCEL_ADMISSION>)CMSHandler.FetchData<ADM_CANCEL_ADMISSION>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCanceledAdmissionList), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "AdmissionCanceledList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "AdmissionCanceledList", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
            return View(ObjViewModel);
        }
        #endregion

        #region Bank Application
        // Bank Application Details ....
        public ActionResult BankApplication()
        {
            AdmissionViewModel ObjViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
            string sUserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sUserId))
            {
                try
                {
                    // Fetch Gender ....
                    var liGender = (List<SupGender>)CMSHandler.FetchData<SupGender>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchGender)).DataSource.Data;
                    if (liGender != null && liGender.Count > 0)
                        ObjViewModel.GENDER = new SelectList(liGender, Common.SUP_GENDER.GENDER_ID, Common.SUP_GENDER.GENDER_NAME);
                    var liMaritalStatus = (List<SupMaritalStatus>)CMSHandler.FetchData<SupMaritalStatus>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMarritalStatus)).DataSource.Data;
                    if (liMaritalStatus != null && liMaritalStatus.Count > 0)
                        ObjViewModel.MARITAL_STATUS = new SelectList(liMaritalStatus, Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID, Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME);
                    var liSalutation = (List<SUP_SALUTATION>)CMSHandler.FetchData<SUP_SALUTATION>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSalutation)).DataSource.Data;
                    if (liSalutation != null && liSalutation.Count > 0)
                        ObjViewModel.SALUTATION = new SelectList(liSalutation, Common.SUP_SALUTATION.SALUTATION_ID, Common.SUP_SALUTATION.SALUTATION);
                    var liRelation = (List<SUP_RELATION>)CMSHandler.FetchData<SUP_RELATION>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchRelaion)).DataSource.Data;
                    if (liRelation != null && liRelation.Count > 0)
                        ObjViewModel.RELATION = new SelectList(liRelation, Common.SUP_RELATION.RELATION_ID, Common.SUP_RELATION.RELATION);
                    var liMedium = (List<SUP_OPTION>)CMSHandler.FetchData<SUP_OPTION>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOption)).DataSource.Data;
                    if (liMedium != null && liMedium.Count > 0)
                        ObjViewModel.MEDIUM = new SelectList(liMedium, Common.SUP_OPTION.OPTION_ID, Common.SUP_OPTION.OPTION_NAME);

                    var liFetchBankApplication = (List<FETCH_BANK_APPLICATION>)CMSHandler.FetchData<FETCH_BANK_APPLICATION>(new FETCH_BANK_APPLICATION() { ISSUE_ID = sUserId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchBankApplicationByIssueId)).DataSource.Data;
                    if (liFetchBankApplication != null && liFetchBankApplication.Count > 0)
                    {
                        string sStudentId = liFetchBankApplication.FirstOrDefault().RECEIVE_ID;
                        var liBankApplication = (List<ADM_BANK_APPLICATION>)CMSHandler.FetchData<ADM_BANK_APPLICATION>(new ADM_BANK_APPLICATION() { STUDENT_ID = sStudentId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchBankApplicationByStudentId)).DataSource.Data;
                        if (liBankApplication != null && liBankApplication.Count > 0)
                        {
                            ObjViewModel.ObjBankApplication.STUDENT_ID = liBankApplication.FirstOrDefault().STUDENT_ID;
                            ObjViewModel.ObjBankApplication.BANK_APPLICATION_ID = liBankApplication.FirstOrDefault().BANK_APPLICATION_ID;
                            ObjViewModel.ObjBankApplication.COLLEGE_NAME = liBankApplication.FirstOrDefault().COLLEGE_NAME;
                            ObjViewModel.ObjBankApplication.DESIGNATION = liBankApplication.FirstOrDefault().DESIGNATION;
                            ObjViewModel.ObjBankApplication.SALUTATION = liBankApplication.FirstOrDefault().SALUTATION;
                            ObjViewModel.ObjBankApplication.FIRST_NAME = liBankApplication.FirstOrDefault().FIRST_NAME;
                            ObjViewModel.ObjBankApplication.LAST_NAME = liBankApplication.FirstOrDefault().LAST_NAME;
                            ObjViewModel.ObjBankApplication.GENDER_ID = liBankApplication.FirstOrDefault().GENDER_ID;
                            ObjViewModel.ObjBankApplication.MARITUAL_STATUS = liBankApplication.FirstOrDefault().MARITUAL_STATUS;
                            ObjViewModel.ObjBankApplication.FATHER_NAME = liBankApplication.FirstOrDefault().FATHER_NAME;
                            ObjViewModel.ObjBankApplication.MOTHER_NAME = liBankApplication.FirstOrDefault().MOTHER_NAME;
                            ObjViewModel.ObjBankApplication.SPOUSE_NAME = liBankApplication.FirstOrDefault().SPOUSE_NAME;
                            ObjViewModel.ObjBankApplication.DOB = liBankApplication.FirstOrDefault().DOB;
                            ObjViewModel.ObjBankApplication.PERMANENT_ADDRESS1 = liBankApplication.FirstOrDefault().PERMANENT_ADDRESS1;
                            ObjViewModel.ObjBankApplication.PERMANENT_ADDRESS2 = liBankApplication.FirstOrDefault().PERMANENT_ADDRESS2;
                            ObjViewModel.ObjBankApplication.PERMANENT_ADDRESS3 = liBankApplication.FirstOrDefault().PERMANENT_ADDRESS3;
                            ObjViewModel.ObjBankApplication.PERMANENT_CITY = liBankApplication.FirstOrDefault().PERMANENT_CITY;
                            ObjViewModel.ObjBankApplication.PERMANENT_STATE = liBankApplication.FirstOrDefault().PERMANENT_STATE;
                            ObjViewModel.ObjBankApplication.PERMANENT_PINCODE = liBankApplication.FirstOrDefault().PERMANENT_PINCODE;
                            ObjViewModel.ObjBankApplication.MOBILE_NUMBER = liBankApplication.FirstOrDefault().MOBILE_NUMBER;
                            ObjViewModel.ObjBankApplication.EMAIL = liBankApplication.FirstOrDefault().EMAIL;
                            ObjViewModel.ObjBankApplication.ID_TYPE = liBankApplication.FirstOrDefault().ID_TYPE;
                            ObjViewModel.ObjBankApplication.NUMBER = liBankApplication.FirstOrDefault().NUMBER;
                            ObjViewModel.ObjBankApplication.STUDENT_ID_CARD_NUMBER = liBankApplication.FirstOrDefault().STUDENT_ID_CARD_NUMBER;
                            ObjViewModel.ObjBankApplication.NOMINEE_NAME = liBankApplication.FirstOrDefault().NOMINEE_NAME;
                            ObjViewModel.ObjBankApplication.NOMINEE_AGE = liBankApplication.FirstOrDefault().NOMINEE_AGE;
                            ObjViewModel.ObjBankApplication.NOMINEE_RELATION = liBankApplication.FirstOrDefault().NOMINEE_RELATION;
                            ObjViewModel.ObjBankApplication.NOMINEE_ADDRESS1 = liBankApplication.FirstOrDefault().NOMINEE_ADDRESS1;
                            ObjViewModel.ObjBankApplication.NOMINEE_ADDRESS2 = liBankApplication.FirstOrDefault().NOMINEE_ADDRESS2;
                            ObjViewModel.ObjBankApplication.NOMINEE_STATE = liBankApplication.FirstOrDefault().NOMINEE_STATE;
                            ObjViewModel.ObjBankApplication.NOMINEE_CITY = liBankApplication.FirstOrDefault().NOMINEE_CITY;
                            ObjViewModel.ObjBankApplication.NOMINNE_PINCODE = liBankApplication.FirstOrDefault().NOMINNE_PINCODE;
                            ObjViewModel.ObjBankApplication.NOMINEE_PHONE = liBankApplication.FirstOrDefault().NOMINEE_PHONE;
                            ObjViewModel.ObjBankApplication.MEDIUM = liBankApplication.FirstOrDefault().MEDIUM;
                            ObjViewModel.ObjBankApplication.COMMUNICATION_STATE = liBankApplication.FirstOrDefault().COMMUNICATION_STATE;
                        }
                        else if (liFetchBankApplication != null && liFetchBankApplication.Count > 0)
                        {
                            ObjViewModel.ObjBankApplication.STUDENT_ID = liFetchBankApplication.FirstOrDefault().RECEIVE_ID;
                            ObjViewModel.ObjBankApplication.FIRST_NAME = liFetchBankApplication.FirstOrDefault().FIRST_NAME;
                            ObjViewModel.ObjBankApplication.LAST_NAME = liFetchBankApplication.FirstOrDefault().LAST_NAME;
                            ObjViewModel.ObjBankApplication.GENDER_ID = liFetchBankApplication.FirstOrDefault().GENDER;
                            ObjViewModel.ObjBankApplication.MOBILE_NUMBER = liFetchBankApplication.FirstOrDefault().MOBILE;
                            ObjViewModel.ObjBankApplication.EMAIL = liFetchBankApplication.FirstOrDefault().EMAIL_ID;
                            ObjViewModel.ObjBankApplication.DOB = liFetchBankApplication.FirstOrDefault().DOB;
                            ObjViewModel.ObjBankApplication.PERMANENT_STATE = liFetchBankApplication.FirstOrDefault().STATE;
                            ObjViewModel.ObjBankApplication.PERMANENT_CITY = liFetchBankApplication.FirstOrDefault().CITY;
                            ObjViewModel.ObjBankApplication.FATHER_NAME = liFetchBankApplication.FirstOrDefault().FATHER_NAME;
                        }

                        var liCollegeInfo = (List<COLLEGE_DETAILS>)CMSHandler.FetchData<COLLEGE_DETAILS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCollegeInfo)).DataSource.Data;
                        if (liCollegeInfo != null && liCollegeInfo.Count > 0)
                        {
                            ObjViewModel.ObjBankApplication.COMMUNICATION_ADDRESS1 = liCollegeInfo.FirstOrDefault().COLLEGE_NAME;
                            ObjViewModel.ObjBankApplication.COMMUNICATION_ADDRESS2 = liCollegeInfo.FirstOrDefault().ADDRESS_TWO;
                            ObjViewModel.ObjBankApplication.COMMUNICATION_ADDRESS3 = liCollegeInfo.FirstOrDefault().ADDRESS_ONE;
                            ObjViewModel.ObjBankApplication.COMMUNICATION_CITY = liCollegeInfo.FirstOrDefault().CITY;
                            ObjViewModel.ObjBankApplication.COMMUNICATION_PINCODE = liCollegeInfo.FirstOrDefault().PINCODE;
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BankApplication", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BankApplication", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(ObjViewModel);
        }

        // Save Bank Application ...
        public JsonResult SaveBankApplication(string sBankApplication)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
            string sUserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sUserId))
            {
                try
                {
                    var Result = JsonConvert.DeserializeObject<ADM_BANK_APPLICATION>(sBankApplication);
                    Result.DOB = CommonMethods.ConvertdatetoyearFromat(Result.DOB);
                    //Result.COLLEGE_NAME = (Session[Common.SESSION_VARIABLES.COLLEGE_INFO] != null) ? Session[Common.SESSION_VARIABLES.COLLEGE_INFO].ToString() : string.Empty;
                    var liExist = (List<ADM_BANK_APPLICATION>)CMSHandler.FetchData<ADM_BANK_APPLICATION>(new ADM_BANK_APPLICATION() { STUDENT_ID = Result.STUDENT_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchBankApplicationByStudentId)).DataSource.Data;
                    if (liExist != null && liExist.Count > 0)
                    {
                        Result.BANK_APPLICATION_ID = liExist.FirstOrDefault().BANK_APPLICATION_ID;
                        var UpdateBankApplication = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatebankApplication));
                        if (UpdateBankApplication.Success)
                        {
                            ObjJsonData.Message = Common.ErrorCode.Ok;
                        }
                        else
                        {
                            ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                    else
                    {
                        var SaveBankApplication = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertBankApplication), "", true);
                        if (SaveBankApplication.Success)
                        {
                            ObjJsonData.Message = Common.ErrorCode.Ok;
                            // ObjJsonData.sResult = SaveBankApplication.RowUniqueId.ToString();
                        }
                        else
                        {
                            ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveBankApplication", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveBankApplication", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Programme Description


        public ActionResult ListProgrammeDescription()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    ObjViewModel.liProgarammeDescription = (List<ADM_PROGRAMME_DESCRIPTION>)CMSHandler.FetchData<ADM_PROGRAMME_DESCRIPTION>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeDescriptionForList), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "ListProgrammeDescription", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "ListProgrammeDescription", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(ObjViewModel);
        }
        public ActionResult AddProgrammeDescription()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var Programme = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByProgrammeGroup), sAcademicYear).DataSource.Data;
                    if (Programme != null && Programme.Count > 0)
                    {
                        ObjViewModel.ProgrammeList = new SelectList(Programme, Common.CP_PROGRAMME_GROUP.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "ListProgrammeDescription", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "ListProgrammeDescription", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(ObjViewModel);
        }
        public ActionResult EditProgrammeDescription(string sProgrammeDescriptionId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
            ADM_PROGRAMME_DESCRIPTION ObjModel = new ADM_PROGRAMME_DESCRIPTION();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var Programme = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByProgrammeGroup), sAcademicYear).DataSource.Data;
                    if (Programme != null && Programme.Count > 0)
                    {
                        ObjViewModel.ProgrammeList = new SelectList(Programme, Common.CP_PROGRAMME_GROUP.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    }
                    ObjViewModel.PROGRAMME_DESCRIPTION_ID = sProgrammeDescriptionId;
                    //var LiDescription = (List<ADM_PROGRAMME_DESCRIPTION>)CMSHandler.FetchData<ADM_PROGRAMME_DESCRIPTION>(new ADM_PROGRAMME_DESCRIPTION() { PROGRAMME_DESCRIPTION_ID = sProgrammeDescriptionId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL
                    //      (AdmissionSQLCommands.FetchProgrammeDescriptionByDescriptionId), sAcademicYear).DataSource.Data;
                    //if (LiDescription != null && LiDescription.Count > 0)
                    //{
                    //    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_DESCRIPTION))
                    //    {
                    //        string Answer = LiDescription.FirstOrDefault().PROGRAMME_DESCRIPTION;
                    //        byte[] bytes = Convert.FromBase64String(Answer);
                    //        ObjModel.PROGRAMME_DESCRIPTION = Encoding.UTF8.GetString(bytes);
                    //    }
                    //    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().COURSE_OBJECTIVE))
                    //    {
                    //        string Answer = LiDescription.FirstOrDefault().COURSE_OBJECTIVE;
                    //        byte[] bytes = Convert.FromBase64String(Answer);
                    //        ObjModel.COURSE_OBJECTIVE = Encoding.UTF8.GetString(bytes);
                    //    }
                    //    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_OUTCOME))
                    //    {
                    //        string Answer = LiDescription.FirstOrDefault().PROGRAMME_OUTCOME;
                    //        byte[] bytes = Convert.FromBase64String(Answer);
                    //        ObjModel.PROGRAMME_OUTCOME = Encoding.UTF8.GetString(bytes);
                    //    }
                    //    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_CONTENT))
                    //    {
                    //        string Answer = LiDescription.FirstOrDefault().PROGRAMME_CONTENT;
                    //        byte[] bytes = Convert.FromBase64String(Answer);
                    //        ObjModel.PROGRAMME_CONTENT = Encoding.UTF8.GetString(bytes);
                    //    }
                    //    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_ELIGIBLITY))
                    //    {
                    //        string Answer = LiDescription.FirstOrDefault().PROGRAMME_ELIGIBLITY;
                    //        byte[] bytes = Convert.FromBase64String(Answer);
                    //        ObjModel.PROGRAMME_ELIGIBLITY = Encoding.UTF8.GetString(bytes);
                    //    }
                    //    ObjModel.SYLLABUS_PATH = LiDescription.FirstOrDefault().SYLLABUS_PATH;
                    //ObjModel.PROGRAMME_DESCRIPTION_ID = LiDescription.FirstOrDefault().PROGRAMME_DESCRIPTION_ID;
                    //}                   
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "ListProgrammeDescription", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "ListProgrammeDescription", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(ObjViewModel);
        }
        public JsonResult InsertProgrammeDescription()
        {
            //string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sAcademicYear = "2018";
            var ObjJsonData = new JsonResultData();
            var ObjModel = new ADM_PROGRAMME_DESCRIPTION();
            string _sDirectorypath = string.Empty;
            string filePath = string.Empty;
            string filedbPath = string.Empty;
            string fileName = string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    //var File = Request.Files[0];
                    string sJsonString = Request.Form["sJsonProgrammeDescription"];
                    var Result = JsonConvert.DeserializeObject<ADM_PROGRAMME_DESCRIPTION>(sJsonString);
                    if (true)
                    {
                        var LiDescription = (List<ADM_PROGRAMME_DESCRIPTION>)CMSHandler.FetchData<ADM_PROGRAMME_DESCRIPTION>(new ADM_PROGRAMME_DESCRIPTION() { PROGRAMME_GROUP_ID = Result.PROGRAMME_GROUP_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL
                          (AdmissionSQLCommands.CheckProgrammeDescriptionExistByProgrammeGroupId), sAcademicYear).DataSource.Data;
                        if (LiDescription != null && LiDescription.Count > 0)
                        {
                            ObjJsonData.Message = Common.Messages.RecordAlreadyExist;
                        }
                        else
                        {
                            // fileName = Result.PROGRAMME_NAME;
                            //_sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.PROGRAMME_SYLLABUS_PATH + "\\" + sAcademicYear;
                            //if (!Directory.Exists(_sDirectorypath))
                            //{
                            //    Directory.CreateDirectory(_sDirectorypath);
                            //}
                            //filePath = _sDirectorypath + "\\" + fileName;
                            //filedbPath = "\\" + Common.URLPages.PROGRAMME_SYLLABUS_PATH + "\\" + sAcademicYear + "\\" + fileName;
                            //File.SaveAs(filePath);
                            // Result.SYLLABUS_PATH = filedbPath;
                            Result.ACADEMIC_YEAR = sAcademicYear;
                            var InsertProgrammeDescription = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertProgrammeDescription), sAcademicYear);
                            ObjJsonData.Message = InsertProgrammeDescription.Success ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                        }

                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog ObjLog = new ErrorLog())
                    {
                        ObjLog.WriteError("AdmissionController", "SaveSupRoom", "Err MSg: " + ex.Message, "Query is empty!");
                        ObjLog.WriteError("AdmissionController", "SaveSupRoom", ex.Message);
                    }
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateProgrammeDescription()
        {
            //string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sAcademicYear = "2018";
            var ObjJsonData = new JsonResultData();
            var ObjModel = new ADM_PROGRAMME_DESCRIPTION();
            string _sDirectorypath = string.Empty;
            string filePath = string.Empty;
            string filedbPath = string.Empty;
            string fileName = string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    //var File = Request.Files[0];
                    string sJsonString = Request.Form["sJsonProgrammeDescription"];
                    //string sExtension = Request.Form["extension"];
                    var Result = JsonConvert.DeserializeObject<ADM_PROGRAMME_DESCRIPTION>(sJsonString);
                    if (true)
                    {
                        //fileName = Result.PROGRAMME_NAME;
                        //_sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.PROGRAMME_SYLLABUS_PATH + "\\" + sAcademicYear;
                        //if (!Directory.Exists(_sDirectorypath))
                        //{
                        //    Directory.CreateDirectory(_sDirectorypath);
                        //}
                        //filePath = _sDirectorypath + "\\" + fileName+"."+ sExtension;
                        //filedbPath = "\\" + Common.URLPages.PROGRAMME_SYLLABUS_PATH + "\\" + sAcademicYear + "\\" + fileName + "." + sExtension;
                        //File.SaveAs(filePath);
                        //Result.SYLLABUS_PATH = filedbPath;
                        Result.ACADEMIC_YEAR = sAcademicYear;
                        var InsertProgrammeDescription = CMSHandler.SaveOrUpdate(Result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateProgrammeDescription), sAcademicYear);
                        ObjJsonData.Message = InsertProgrammeDescription.Success ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog ObjLog = new ErrorLog())
                    {
                        ObjLog.WriteError("AdmissionController", "SaveSupRoom", "Err MSg: " + ex.Message, "Query is empty!");
                        ObjLog.WriteError("AdmissionController", "SaveSupRoom", ex.Message);
                    }
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchProgrammeDescriptionById(string sProgrammeDescriptionId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjJsonData = new JsonResultData();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var LiDescription = (List<ADM_PROGRAMME_DESCRIPTION>)CMSHandler.FetchData<ADM_PROGRAMME_DESCRIPTION>(new ADM_PROGRAMME_DESCRIPTION() { PROGRAMME_DESCRIPTION_ID = sProgrammeDescriptionId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL
                          (AdmissionSQLCommands.FetchProgrammeDescriptionByDescriptionId), sAcademicYear).DataSource.Data;
                    if (LiDescription != null && LiDescription.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_DESCRIPTION))
                        {
                            string Answer = LiDescription.FirstOrDefault().PROGRAMME_DESCRIPTION;
                            byte[] bytes = Convert.FromBase64String(Answer);
                            LiDescription.FirstOrDefault().PROGRAMME_DESCRIPTION = Encoding.UTF8.GetString(bytes);
                        }
                        if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().COURSE_OBJECTIVE))
                        {
                            string Answer = LiDescription.FirstOrDefault().COURSE_OBJECTIVE;
                            byte[] bytes = Convert.FromBase64String(Answer);
                            LiDescription.FirstOrDefault().COURSE_OBJECTIVE = Encoding.UTF8.GetString(bytes);
                        }
                        if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_OUTCOME))
                        {
                            string Answer = LiDescription.FirstOrDefault().PROGRAMME_OUTCOME;
                            byte[] bytes = Convert.FromBase64String(Answer);
                            LiDescription.FirstOrDefault().PROGRAMME_OUTCOME = Encoding.UTF8.GetString(bytes);
                        }
                        if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_CONTENT))
                        {
                            string Answer = LiDescription.FirstOrDefault().PROGRAMME_CONTENT;
                            byte[] bytes = Convert.FromBase64String(Answer);
                            LiDescription.FirstOrDefault().PROGRAMME_CONTENT = Encoding.UTF8.GetString(bytes);
                        }
                        if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_ELIGIBLITY))
                        {
                            string Answer = LiDescription.FirstOrDefault().PROGRAMME_ELIGIBLITY;
                            byte[] bytes = Convert.FromBase64String(Answer);
                            LiDescription.FirstOrDefault().PROGRAMME_ELIGIBLITY = Encoding.UTF8.GetString(bytes);
                        }
                        return Json(LiDescription, JsonRequestBehavior.AllowGet);
                    }
                    else
                        ObjJsonData.Message = Common.Messages.NoRecordsfound;
                }
                catch (Exception ex)
                {
                    using (ErrorLog ObjLog = new ErrorLog())
                    {
                        ObjLog.WriteError("AdmissionController", "FetchProgrammeDescriptionById", "Err MSg: " + ex.Message, "Query is empty!");
                        ObjLog.WriteError("AdmissionController", "FetchProgrammeDescriptionById", ex.Message);
                    }
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteProgrammeDescription(string sProgrammeDescriptionId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var ObjJsonData = new JsonResultData();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var sDeleteRoom = CMSHandler.SaveOrUpdate(new ADM_PROGRAMME_DESCRIPTION() { PROGRAMME_DESCRIPTION_ID = sProgrammeDescriptionId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.DeleteProgrammeDescription), sAcademicYear);
                    ObjJsonData.Message = (sDeleteRoom.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                }
                catch (Exception ex)
                {
                    using (ErrorLog ObjLog = new ErrorLog())
                    {
                        ObjLog.WriteError("AdmissionController", "DeleteProgrammeDescription", "Err MSg: " + ex.Message, "Query is empty!");
                        ObjLog.WriteError("AdmissionController", "DeleteProgrammeDescription", ex.Message);
                    }
                }
            }
            else
            {
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AdmissionCourses()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                //string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
                AdmissionViewModel ObjViewModel = new AdmissionViewModel();
                ObjViewModel.LiApplictionType = (List<SUP_APPLICATION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                //ObjViewModel.LiProgrammeGroup = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByProgrammeGroup), sAcademicYear).DataSource.Data;
                ObjViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeForCourses), sAcademicYear).DataSource.Data;
                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("hccindex");
            }
        }

        public ActionResult SelectedListCourses()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                //string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
                AdmissionViewModel ObjViewModel = new AdmissionViewModel();
                ObjViewModel.LiApplictionType = (List<SUP_APPLICATION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                //ObjViewModel.LiProgrammeGroup = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByProgrammeGroup), sAcademicYear).DataSource.Data;
                ObjViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeForCourses), sAcademicYear).DataSource.Data;
                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("hccindex");
            }

        }
        public ActionResult ProgrammeDescription(string sProgrammeGroupId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
                AdmissionViewModel ObjViewModel = new AdmissionViewModel();
                ObjViewModel.LiApplictionType = (List<SUP_APPLICATION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                var LiDescription = (List<ADM_PROGRAMME_DESCRIPTION>)CMSHandler.FetchData<ADM_PROGRAMME_DESCRIPTION>(new ADM_PROGRAMME_DESCRIPTION { PROGRAMME_GROUP_ID = sProgrammeGroupId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeDescriptionByProgrammeGroupId), sAcademicYear).DataSource.Data;
                if (LiDescription != null && LiDescription.Count > 0)
                {
                    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_DESCRIPTION))
                    {
                        string Answer = LiDescription.FirstOrDefault().PROGRAMME_DESCRIPTION;
                        byte[] bytes = Convert.FromBase64String(Answer);
                        LiDescription.FirstOrDefault().PROGRAMME_DESCRIPTION = Encoding.UTF8.GetString(bytes);
                    }
                    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().COURSE_OBJECTIVE))
                    {
                        string Answer = LiDescription.FirstOrDefault().COURSE_OBJECTIVE;
                        byte[] bytes = Convert.FromBase64String(Answer);
                        LiDescription.FirstOrDefault().COURSE_OBJECTIVE = Encoding.UTF8.GetString(bytes);
                    }
                    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_OUTCOME))
                    {
                        string Answer = LiDescription.FirstOrDefault().PROGRAMME_OUTCOME;
                        byte[] bytes = Convert.FromBase64String(Answer);
                        LiDescription.FirstOrDefault().PROGRAMME_OUTCOME = Encoding.UTF8.GetString(bytes);
                    }
                    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_CONTENT))
                    {
                        string Answer = LiDescription.FirstOrDefault().PROGRAMME_CONTENT;
                        byte[] bytes = Convert.FromBase64String(Answer);
                        LiDescription.FirstOrDefault().PROGRAMME_CONTENT = Encoding.UTF8.GetString(bytes);
                    }
                    if (!string.IsNullOrEmpty(LiDescription.FirstOrDefault().PROGRAMME_ELIGIBLITY))
                    {
                        string Answer = LiDescription.FirstOrDefault().PROGRAMME_ELIGIBLITY;
                        byte[] bytes = Convert.FromBase64String(Answer);
                        LiDescription.FirstOrDefault().PROGRAMME_ELIGIBLITY = Encoding.UTF8.GetString(bytes);
                    }
                    ObjViewModel.liProgarammeDescription = LiDescription;
                }
                return View(ObjViewModel);
            }
            else
            {
                return RedirectToAction("hccindex");
            }

        }
        public ActionResult IssuedApplication()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sAcademicYearId = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString() : string.Empty;
                try
                {
                    var LiIssuedApplication = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchDepartmentWiseIssuedApplication), sAcademicYear).DataSource.Data;
                    objViewModel.liCourses = LiIssuedApplication;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "IssuedApplication", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "IssuedApplication", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region Admission Support
        public JsonResult CheckIsUserExit(string semail, string sHscno)
        {
            if (!string.IsNullOrEmpty(semail))
            {
                var Result = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(new USER_ACCOUNT_INFO() { USERNAME = semail, HSC_NO = sHscno }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.CheckIsUserExist)).DataSource.Data;
                if (Result == null)
                {
                    ObjJsonData.Message = "User Email Id or Hsc No is not Exist..!";

                }
                else
                {
                    ObjJsonData.sResult = Result.FirstOrDefault().USER_ACCOUNT_ID;
                }
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SavePasswordReset(string sPassword, string sAcid)
        {
            if (!string.IsNullOrEmpty(sPassword))
            {
                var Result = CMSHandler.SaveOrUpdate(new USER_ACCOUNT_INFO() { PASSWORD = sPassword, USER_ACCOUNT_ID = sAcid }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatePasswordByUserAcId));
                if (Result.Success)
                    ObjJsonData.Message = "Password Changed Successfully..!";
                else
                    ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CasteWiseStatus()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var Programme = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByProgrammeGroup), sAcademicYear).DataSource.Data;
                    if (Programme != null && Programme.Count > 0)
                    {
                        objViewModel.ProgrammeList = new SelectList(Programme, Common.CP_PROGRAMME_GROUP.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "CasteWiseStatus", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "CasteWiseStatus", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindProgrammeWiseandCasteStatus(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sSQL;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    objViewModel.LiApplicantType = (List<SUP_APPLICANT_TYPE>)CMSHandler.FetchData<SUP_APPLICANT_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchApplicantType), sAcademicYear).DataSource.Data;
                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeWiseQutoa).Replace((Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID), sProgrammeId);
                    objViewModel.liProgram_Quata = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;
                    objViewModel.liMainCommunity = (List<MainCommunity>)CMSHandler.FetchData<MainCommunity>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMainCommunity)).DataSource.Data;

                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchMinorityCount).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liMinorityWise = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;

                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOthersCasteWise).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liCasteWise_Quata = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;

                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOcCasteWise).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liOcCasteWise_Quata = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindProgrammeWiseandCasteStatus", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindProgrammeWiseandCasteStatus", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        public ActionResult SelectionCancel()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                try
                {
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SelectionCancel", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SelectionCancel", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindStudentSelectedList(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sSQL = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                try
                {
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSelectedListByProgrammeId);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(null, sSQL, sAcademicYear).DataSource.Data;

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindStudentSelectedList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindStudentSelectedList", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        public JsonResult SaveSelectionCacel(string sSelectionId, string sReceiveId, string sProgrammeId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                try
                {
                    var CancelSelection = CMSHandler.SaveOrUpdate(new ADM_SELECTION_PROCESS() { SELECTION_ID = sSelectionId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.CancelSelectedBySelectionId), sAcademicYear);
                    if (CancelSelection.Success)
                    {
                        new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                        var RevertStatus = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { PROGRAMME_ID = sProgrammeId, RECEIVE_ID = sReceiveId, STATUS = Common.STATUS.Submitted }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertStatusByReceiveIdandProgrammeId));
                        var cancelSelectedApp = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { PROGRAMME_ID = sProgrammeId, RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.CancelSelectionByReceiveIdandProgrammeId), sAcademicYear);
                        new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                        ObjJsonData.Message = "Selection Canceled Successfully..!";
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindStudentSelectedList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindStudentSelectedList", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }

            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Rollno
        public ActionResult RollnoCreation()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "RollnoCreation", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "RollnoCreation", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(objViewModel);
        }
        public ActionResult BindStudents(string sProgrammeId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    objViewModel.liApplicant = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new CP_PROGRAMME_GROUP() { PROGRAMME_ID = sProgrammeId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentsByProgrammeId), sAcademicYear).DataSource.Data;

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindStudents", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindStudents", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }

            return View(objViewModel);
        }
        public JsonResult FetchClassess(string sProgrammeId)
        {
            string sSQL = string.Empty;
            string sOption = string.Empty;
            try
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassessByProgrammeId).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                var Result = (List<CP_CLASSES>)CMSHandler.FetchData<CP_CLASSES>(new CP_PROGRAMME_GROUP { PROGRAMME_GROUP_ID = sProgrammeId }, sSQL).DataSource.Data;
                if (Result != null && Result.Count > 0)
                {
                    foreach (var item in Result)
                    {
                        sOption += "<option value=" + item.CLASS_ID + ">" + item.CLASS_NAME + "</option>";
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchClassess", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchClassess", ex.Message);
                }
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
            return Json(sOption, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateStudentClass(string sStudentDetails, string sClassId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string classid = string.Empty;
                string programmeid = string.Empty;
                JSON_ADM_STU_SUBMARKS objJson = new JSON_ADM_STU_SUBMARKS();

                if (!string.IsNullOrEmpty(sStudentDetails))
                {

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    objJson = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sStudentDetails);
                    programmeid = objJson.JSON_ADM_RECEIVE.FirstOrDefault().PROGRAMME_ID;
                    foreach (var item in objJson.JSON_ADM_RECEIVE)
                    {
                        var UpdateClass = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateClassByReceiveId), sAcademicYear);

                    }
                }
                classid = sClassId;
                objViewModel.liApplicant = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { CLASS = classid }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentsByClassId), sAcademicYear).DataSource.Data;

            }
            return View(objViewModel);
        }
        public JsonResult SaveStudentRollno(string sStudentDetails)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JSON_ADM_STU_SUBMARKS objJson = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sStudentDetails);
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                string lastruningid = objJson.JSON_ADM_RECEIVE.FirstOrDefault().RUN_ID;
                string shift = objJson.JSON_ADM_RECEIVE.FirstOrDefault().SHIFT;
                string programmemode = objJson.JSON_ADM_RECEIVE.FirstOrDefault().PROGRAMME_MODE;
                var Fetchprogrammegoupid = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(new CP_PROGRAMME_GROUP() { PROGRAMME_MODE = programmemode, SHIFT = shift }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeGroupByShiftAndProgrammeMode)).DataSource.Data;
                string sSQL = string.Empty;
                var programmegroupids = string.Join(",", Fetchprogrammegoupid.Select(t => t.PROGRAMME_GROUP_ID));
                sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateLastRuningId).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.PROGRAMME_GROUP_ID, programmegroupids);
                var Updateruningid = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { RUN_ID = lastruningid, SHIFT = shift, PROGRAMME_MODE = programmemode }, sSQL);
                if (Updateruningid.Success)
                {
                    foreach (var item in objJson.JSON_ADM_RECEIVE)
                    {
                        var UpdateRollno = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateRollnoByReceiveId), sAcademicYear);
                        if (UpdateRollno.Success)
                            ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                        else
                            ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                    }
                }
                else
                    ObjJsonData.Message = Common.Messages.FailedToSaveRecords;

            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveStudentRollno", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveStudentRollno", ex.Message);
                }
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PromoteStudent(string sStudentDetails)
        {
            JSON_ADM_STU_SUBMARKS objJson = new JSON_ADM_STU_SUBMARKS();
            string ReceiveId = string.Empty;
            string sSQL = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sAcademicYearId = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString();
                string sBatchId = string.Empty;
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                objJson = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sStudentDetails);
                foreach (var item in objJson.JSON_ADM_RECEIVE)
                {
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.IsStudentExists).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                    var IsExist = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { ROLL_NO = item.ROLL_NO }, sSQL).DataSource.Data;
                    if (IsExist != null)
                    {
                        sSQL = string.Empty;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStuPersonal).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                        var UpdateStuPersonal = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { STUDENT_ID = item.STUDENT_ID }, sSQL);
                        if (UpdateStuPersonal.Success)
                        {
                            sSQL = string.Empty;
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStuClass).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                            var UpdateStuClass = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { STUDENT_ID = item.STUDENT_ID }, sSQL);
                        }
                        sSQL = string.Empty;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UseraccountExisit).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                        var UseracExist = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { ROLL_NO = IsExist.FirstOrDefault().ROLL_NO, STUDENT_ID = IsExist.FirstOrDefault().STUDENT_ID }, sSQL).DataSource.Data;
                        if (UseracExist != null && UseracExist.Count > 0)
                        {

                        }
                        else
                        {
                            sSQL = string.Empty;
                            sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfo).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                            var FetchStuInfo = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { ROLL_NO = item.ROLL_NO }, sSQL).DataSource.Data;
                            sSQL = string.Empty;
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertUseraccount).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                            var Insertuseraccount = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { FIRST_NAME = FetchStuInfo.FirstOrDefault().FIRST_NAME, ROLL_NO = FetchStuInfo.FirstOrDefault().ROLL_NO, STUDENT_ID = FetchStuInfo.FirstOrDefault().STUDENT_ID, STU_EMAILID = FetchStuInfo.FirstOrDefault().STU_EMAILID, STU_MOBILENO = FetchStuInfo.FirstOrDefault().STU_MOBILENO }, sSQL);
                        }

                        sSQL = string.Empty;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UserRolesByAcademicYearExist).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                        var UserRolesByAcademicYearExist = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { STUDENT_ID = IsExist.FirstOrDefault().STUDENT_ID }, sSQL, sAcademicYear).DataSource.Data;
                        if (UserRolesByAcademicYearExist != null && UserRolesByAcademicYearExist.Count > 0)
                        {

                        }
                        else
                        {
                            sSQL = string.Empty;
                            sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfo).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                            var FetchStuInfo = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { ROLL_NO = item.ROLL_NO }, sSQL).DataSource.Data;
                            sSQL = string.Empty;
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertUserRolesByAcademicYear).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                            var InsertuserRolesByAcademicYear = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { STUDENT_ID = FetchStuInfo.FirstOrDefault().STUDENT_ID }, sSQL, sAcademicYear);

                        }
                    }
                    else
                    {
                        sSQL = string.Empty;
                        sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchBatchIdByAc).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                        var BatchId = (List<AcademicBatches>)CMSHandler.FetchData<AcademicBatches>(new AcademicBatches() { ACADEMIC_YEAR_ID = sAcademicYearId }, sSQL).DataSource.Data;
                        if (BatchId != null && BatchId.Count > 0)
                        {
                            sBatchId = BatchId.FirstOrDefault().BATCH_ID;

                        }
                        else
                            sBatchId = "10";

                        sSQL = string.Empty;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertStupersonalInfo).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                        var InsertStupersonal = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { DEPARTMENT = item.DEPARTMENT, PROGRAMME_ID = item.PROGRAMME_GROUP_ID, BATCH_ID = sBatchId, PROGRAMME_MODE = item.PROGRAMME_MODE, SHIFT = item.SHIFT, RECEIVE_ID = item.RECEIVE_ID, ACADEMIC_YEAR = sAcademicYear, APPLICATION_NO = item.APPLICATION_NO }, sSQL);
                        if (InsertStupersonal.Success)
                        {
                            sSQL = string.Empty;
                            sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentInfo).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                            var FetchStuInfo = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { ROLL_NO = item.ROLL_NO }, sSQL).DataSource.Data;
                            if (FetchStuInfo != null && FetchStuInfo.Count > 0)
                            {
                                sSQL = string.Empty;
                                sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertStuclass).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                                var InsertStuclass = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { STUDENT_ID = FetchStuInfo.FirstOrDefault().STUDENT_ID, CLASS_ID = FetchStuInfo.FirstOrDefault().CLASS_ID, BATCH_ID = FetchStuInfo.FirstOrDefault().BATCH_ID, PROGRAMME_GROUP_ID = FetchStuInfo.FirstOrDefault().PROGRAMME_ID, ACADEMIC_YEAR = sAcademicYear }, sSQL);
                                if (InsertStuclass.Success)
                                {
                                    sSQL = string.Empty;
                                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertUseraccount).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                                    var Insertuseraccount = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { FIRST_NAME = FetchStuInfo.FirstOrDefault().FIRST_NAME, ROLL_NO = FetchStuInfo.FirstOrDefault().ROLL_NO, STUDENT_ID = FetchStuInfo.FirstOrDefault().STUDENT_ID, STU_EMAILID = FetchStuInfo.FirstOrDefault().STU_EMAILID, STU_MOBILENO = FetchStuInfo.FirstOrDefault().STU_MOBILENO }, sSQL);


                                    if (Insertuseraccount.Success)
                                    {
                                        sSQL = string.Empty;
                                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.InsertUserRolesByAcademicYear).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                                        var InsertuserRolesByAcademicYear = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { STUDENT_ID = FetchStuInfo.FirstOrDefault().STUDENT_ID }, sSQL, sAcademicYear);
                                    }


                                }
                                ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                            }
                        }

                    }
                    ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;

                }



            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RollnoNotCreatedList()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var ShiftList = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift), sAcademicYear).DataSource.Data;
                    if (ShiftList != null && ShiftList.Count > 0)
                        objViewModel.ShiftList = new SelectList(ShiftList, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    var Programmemode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode), sAcademicYear).DataSource.Data;
                    if (Programmemode != null && Programmemode.Count > 0)
                        objViewModel.ProgrammeMode = new SelectList(Programmemode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "RollnoNotCreatedList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "RollnoNotCreatedList", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }

            return View(objViewModel);
        }
        public JsonResult FetchProgrammesBymodeandShift(string sShift, string sProgmode)
        {
            string sOption = string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var ListProgramme = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(new ADM_APPTYPE_PROG_GROUPS() { SHIFT = sShift, PROGRAMME_MODE = sProgmode }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeBymodeandShift)).DataSource.Data;
                    if (ListProgramme != null && ListProgramme.Count > 0)
                    {
                        foreach (var item in ListProgramme)
                        {
                            sOption += "<option value='" + item.PROGRAMME_GROUP_ID + "'>" + item.PROGRAMME_NAME + "</option>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "FetchProgrammesBymodeandShift", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "FetchProgrammesBymodeandShift", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return Json(sOption, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindNotCreatedList(string sProgId)
        {
            string sSQL = string.Empty;
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchRollnoNotCreatedStudent).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_ID, sProgId);
                    objViewModel.liApplicant = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(null, sSQL, sAcademicYear).DataSource.Data;

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindNotCreatedList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindNotCreatedList", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        #endregion
        #region Reports
        public ActionResult SelectionProcessStatus()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sAcademicYear))
                {
                    var CycleList = (List<SUP_SELECTION_CYCLE>)CMSHandler.FetchData<SUP_SELECTION_CYCLE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSelectionCycle)).DataSource.Data;
                    var Shift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                    var ApplicationType = (List<ADM_APPLICATION_TYPE>)CMSHandler.FetchData<ADM_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                    if (Shift != null && Shift.Count > 0)
                        objViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    if (ApplicationType != null && ApplicationType.Count > 0)
                        objViewModel.ApplicationTypeList = new SelectList(ApplicationType, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE);
                    if (CycleList != null && CycleList.Count > 0)
                        objViewModel.CycleList = new SelectList(CycleList, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE_ID, Common.SUP_SELECTION_CYCLE.SELECTION_CYCLE);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SelectionProcessStatus", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SelectionProcessStatus", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindSelectionProcessStatus(string sCycle, string sStatus, string sProgramme)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sSQL = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sAcademicYear))
                {
                    if (sStatus == Common.ADM_STATUS.VERIFIED)
                    {
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmSelectedStudentByStatusForVerificationProgrammeAndCycle).Replace(Common.Delimiter.QUS + Common.SUP_APPTYPE_PROG_GROUPS.PROGRAMME_GROUP_ID, sProgramme);
                    }
                    else
                    {
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmSelectedStudentByStatusAndProgrammeAndCycle).Replace(Common.Delimiter.QUS + Common.SUP_APPTYPE_PROG_GROUPS.PROGRAMME_GROUP_ID, sProgramme);
                    }
                    objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(new ADM_SELECTION_PROCESS { STATUS = sStatus, SELECTION_CYCLE = sCycle }, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessStatus", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessStatus", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult IssuedApplicationForStaffByStatus()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sAcademicYear))
                {
                    var Shift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                    var ApplicationType = (List<ADM_APPLICATION_TYPE>)CMSHandler.FetchData<ADM_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                    if (Shift != null && Shift.Count > 0)
                        objViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    if (ApplicationType != null && ApplicationType.Count > 0)
                        objViewModel.ApplicationTypeList = new SelectList(ApplicationType, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SelectionProcessStatus", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SelectionProcessStatus", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindIssuedApplicationForStaffByStatus(string sStatus, string sProgramme)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sSQL = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sAcademicYear))
                {
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmRegisteredApplicationByProgrammeAndStatusForStaff).Replace(Common.Delimiter.QUS + Common.SUP_APPTYPE_PROG_GROUPS.PROGRAMME_GROUP_ID, sProgramme);
                    objViewModel.liApplicant = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_SELECTION_PROCESS { STATUS = sStatus }, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessStatus", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessStatus", ex.Message);
                }
            }
            return View(objViewModel);
        }

        public ActionResult IssuedApplicationForReport()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sSQL = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sAcademicYear))
                {
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIssuedApplicationForReport);
                    objViewModel.liApplicant = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(null, sSQL, sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessStatus", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindSelectionProcessStatus", ex.Message);
                }
            }
            return View(objViewModel);
        }

        public ActionResult QuataWiseReportByProgramme()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var Shift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                    var ApplicationType = (List<ADM_APPLICATION_TYPE>)CMSHandler.FetchData<ADM_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                    if (Shift != null && Shift.Count > 0)
                        objViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    if (ApplicationType != null && ApplicationType.Count > 0)
                        objViewModel.ApplicationTypeList = new SelectList(ApplicationType, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "QuataWiseReportByProgramme", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "QuataWiseReportByProgramme", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindQuataWiseReportByProgramme(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sSQL;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeInProgrammeGroupByProgramme).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, sProgrammeId);
                    objViewModel.liCourses = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, sSQL, sAcademicYear).DataSource.Data;
                    sSQL = string.Empty;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchQuataWiseReportByProgrammeAndCaste).Replace(Common.Delimiter.QUS + Common.adm_apptype_prog_groups.PROGRAMME_ID, sProgrammeId);
                    objViewModel.liCasteWise_Quata = (List<ADM_CASTEWISE_QUATA>)CMSHandler.FetchData<ADM_CASTEWISE_QUATA>(null, sSQL, sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindQuataWiseReportByProgramme", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindQuataWiseReportByProgramme", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }

        public ActionResult StudentInfo()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var Fields = (List<SUP_FIELDS>)CMSHandler.FetchData<SUP_FIELDS>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFields)).DataSource.Data;
                    var ApplicationType = (List<ADM_APPLICATION_TYPE>)CMSHandler.FetchData<ADM_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationtype)).DataSource.Data;
                    if (ApplicationType != null && ApplicationType.Count > 0)
                        objViewModel.ApplicationTypeList = new SelectList(ApplicationType, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE);
                    if (Fields != null && Fields.Count > 0)
                        objViewModel.Fields = new SelectList(Fields, Common.SUP_FIELDS.ALICE_NAME, Common.SUP_FIELDS.FIELD_NAME);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "StudentInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "StudentInfo", ex.Message);
                    }

                }
            }
            return View(objViewModel);
        }
        public JsonResult FetchProgrammesByAppType(string sApptypeId)
        {
            string sOption = string.Empty;
            string sFields = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sApptypeId))
                {
                    var ProgrammeList = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(new CP_PROGRAMME_GROUP() { APPTYPE_ID = sApptypeId }, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammesByAppType)).DataSource.Data;
                    var Fields = (List<SUP_FIELDS>)CMSHandler.FetchData<SUP_FIELDS>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupFields)).DataSource.Data;
                    foreach (var field in Fields)
                    {
                        sFields += "<option value='" + field.ALICE_NAME + "' field='" + field.ALICE_NAME + "'  property='" + field.PROPERTY_NAME + "' >" + field.FIELD_NAME + "</option>";
                    }
                    foreach (var item in ProgrammeList)
                    {
                        sOption += "<option value='" + item.PROGRAMME_GROUP_ID + "'>" + item.PROGRAMME_NAME + "</option>";
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchProgrammesByAppType", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchProgrammesByAppType", ex.Message);
                }
            }
            string JsonData = string.Empty;
            var data = new SUP_FIELDS() { FIELD_ID = sFields, ALICE_NAME = sOption };
            JsonData = JsonConvert.SerializeObject(data);
            return Json(JsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindStudentInfo(string sProgramme, string sFields, string sProperty, string sColumns)
        {

            AdmissionViewModel objViewModel = new AdmissionViewModel();
            List<SUP_FIELDS> lifields = new List<SUP_FIELDS>();
            List<SUP_FIELDS> lifield = new List<SUP_FIELDS>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sSQL = string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var propertys = sProperty.Split(',');
                    var columns = sColumns.Split(',');
                    foreach (var property in propertys)
                    {
                        lifields.Add(new SUP_FIELDS() { PROPERTY_NAME = property });
                    }
                    foreach (var column in columns)
                    {
                        lifield.Add(new SUP_FIELDS() { PROPERTY_NAME = column });
                    }
                    objViewModel.LiFields = lifields;
                    objViewModel.LiColumns = lifield;
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentInfo).Replace(Common.Delimiter.QUS + Common.FIELDS, sFields);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.PROGRAMME_GROUP_ID, sProgramme);
                    var StudentInfo = (List<StudentModel>)CMSHandler.FetchData<StudentModel>(null, sSQL, sAcademicYear).DataSource.Data;
                    if (StudentInfo != null && StudentInfo.Count > 0)
                    {
                        objViewModel.LiStudentInfo = StudentInfo;


                    }
                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindStudentInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindStudentInfo", ex.Message);
                    }
                }
            }
            return View(objViewModel);
        }
        public ActionResult ListDateWiseSMSDelivered()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "ListDateWiseSMSDelivered", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "ListDateWiseSMSDelivered", ex.Message);
                    }

                }
            }
            return View(objViewModel);
        }
        public ActionResult BindDateWiseSMS(string sDateFrom, string sDateTo)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();

            if (!string.IsNullOrEmpty(sDateFrom) && !string.IsNullOrEmpty(sDateTo))
            {
                try
                {

                    objViewModel.Lisms = (List<SENT_SMS_LIST_2017>)CMSHandler.FetchData<SENT_SMS_LIST_2017>(new SENT_SMS_LIST_2017() { FROM_DATE = CommonMethods.ConvertdatetoyearFromat(sDateFrom), TO_DATE = CommonMethods.ConvertdatetoyearFromat(sDateTo) }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDatewiseSMS)).DataSource.Data;

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "StudentInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "StudentInfo", ex.Message);
                    }

                }
            }
            return View(objViewModel);
        }
        #endregion
        #region ID Card Process
        public ActionResult IDcardProcess()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var ProgrammeList = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByProgrammeGroup), sAcademicYear).DataSource.Data;
                    objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.CP_PROGRAMME_GROUP.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_GROUP.PROGRAMME_NAME);
                    var LiApplication = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudent), sAcademicYear).DataSource.Data;
                    objViewModel.Application = new SelectList(LiApplication, Common.ADM_ISSUE_APPLICATION_2018.ISSUED_ID, Common.ADM_ISSUE_APPLICATION_2018.APPLICATION_NO);

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "IDcardProcess", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "IDcardProcess", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindStudentDetails(string sApplicationno)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUE_APPLICATION_2018 objModel = new ADM_ISSUE_APPLICATION_2018();
            string sOption = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sApplicationno))
                {
                    var Listudentdetails = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_RECEIVE_APPLICATION() { ISSUED_ID = sApplicationno }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentByApplicationNo)).DataSource.Data;
                    if (Listudentdetails != null && Listudentdetails.Count > 0)
                    {
                        // objViewModel.liIssueApplication = Listudentdetails;
                        objModel.FIRST_NAME = Listudentdetails.FirstOrDefault().FIRST_NAME;
                        objModel.DOB = Listudentdetails.FirstOrDefault().DOB;
                        objModel.PROGRAMME_NAME = Listudentdetails.FirstOrDefault().PROGRAMME_NAME;
                        objModel.AADHAR_NUMBER = Listudentdetails.FirstOrDefault().AADHAR_NUMBER;
                        objModel.CDOORDETAIL = Listudentdetails.FirstOrDefault().CDOORDETAIL;
                        objModel.CSTREET = Listudentdetails.FirstOrDefault().CSTREET;
                        objModel.CTALUK_CITY = Listudentdetails.FirstOrDefault().CTALUK_CITY;
                        objModel.CDISTRICT = Listudentdetails.FirstOrDefault().CDISTRICT;
                        objModel.CPINCODE = Listudentdetails.FirstOrDefault().CPINCODE;
                        objModel.CVILLAGE_AREA = Listudentdetails.FirstOrDefault().CVILLAGE_AREA;

                        objModel.IMAGE_PATH = Listudentdetails.FirstOrDefault().IMAGE_PATH;
                        objModel.SMS_MOBILE_NO = Listudentdetails.FirstOrDefault().SMS_MOBILE_NO;
                        var Listbloodgroup = (List<SupBloodGroup>)CMSHandler.FetchData<SupBloodGroup>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchBloodGroup)).DataSource.Data;
                        if (Listbloodgroup != null && Listbloodgroup.Count > 0)
                        {
                            foreach (var item in Listbloodgroup)
                            {
                                if (item.BLOOD_GROUP_ID.ToString() == Listudentdetails.FirstOrDefault().BLOOD_GROUP.ToString())
                                {
                                    sOption += "<option value=" + item.BLOOD_GROUP_ID + " selected>" + item.BLOOD_GROUP_NAME + "</option>";
                                }
                                else
                                {
                                    sOption += "<option value=" + item.BLOOD_GROUP_ID + ">" + item.BLOOD_GROUP_NAME + "</option>";
                                }
                            }

                        }
                        objModel.BLOOD_GROUP = sOption;

                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindStudentDetails", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindStudentDetails", ex.Message);
                }
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
            return View(objModel);
        }
        public JsonResult FetchApplicant(string sProgrammeId)
        {
            string sSQL = string.Empty;
            string sOption = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                try
                {
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudent).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.PROGRAMME_ID, sProgrammeId);

                    var LiApplication = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(null, sSQL, sAcademicYear).DataSource.Data;
                    foreach (var app in LiApplication)
                    {
                        sOption += "<option value='" + app.ISSUED_ID + "'>" + app.APPLICATION_NO + "</option>";
                    }

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "FetchApplicant", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "FetchApplicant", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return Json(sOption, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveImage(string baseImage, string sExtension, string Programmecode, string fileName)
        {
            string Message = string.Empty;
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

                    string sImagePath = string.Empty;
                    //this is to rename baseimgae64 formart.
                    string sToReplaceBaseFormat = string.Format("data:image/{0};base64,", (sExtension == "jpeg") ? "jpeg" : sExtension);
                    string strBaseimage64 = string.Empty;
                    string filePath = string.Empty;
                    //actual replacement 
                    strBaseimage64 = baseImage.Replace(sToReplaceBaseFormat, string.Empty);

                    string _sDirectorypath = string.Empty;
                    // bool _bSuccess = true;
                    _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_IMGAE_PATH;

                    if (!Directory.Exists(_sDirectorypath))
                    {
                        Directory.CreateDirectory(_sDirectorypath);

                    }
                    _sDirectorypath = string.Empty;
                    _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + sAcademicYear;
                    if (!Directory.Exists(_sDirectorypath))
                    {
                        Directory.CreateDirectory(_sDirectorypath);
                    }
                    //_sDirectorypath = string.Empty;
                    //_sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages. +"\\"+sAcademicYear+"\\"+Programmecode;
                    //if (!Directory.Exists(_sDirectorypath))
                    //{
                    //    Directory.CreateDirectory(_sDirectorypath);
                    //}
                    //Convert Base64 Encoded string to Byte Array.
                    string convert = strBaseimage64.Replace("data:image/jpeg;base64,", String.Empty);
                    byte[] imageBytes = Convert.FromBase64String(convert);

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        filePath = _sDirectorypath + "\\" + fileName + "." + sExtension;
                        sImagePath = string.Empty;
                        sImagePath = "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + sAcademicYear + "\\" + fileName + "." + sExtension;
                    }
                    //Save the Byte Array as Image File.and replace file if exists
                    System.IO.File.WriteAllBytes(filePath, imageBytes);

                    var ReceiveId = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_RECEIVE_APPLICATION() { RECEIVE_ID = fileName }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchReceiveIdByAppno)).DataSource.Data;
                    if (ReceiveId != null && ReceiveId.Count > 0)
                    {
                        var Result = CMSHandler.SaveOrUpdate(new ADM_RECEIVE_APPLICATION() { RECEIVE_ID = ReceiveId.FirstOrDefault().RECEIVE_ID, PHOTO_PATH = sImagePath }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdateImagePath));
                        if (Result.Success)
                            Message = Common.Messages.RecordsSavedSuccessfully;
                        else
                            Message = Common.Messages.FailedToSaveRecords;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveImage", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveImage", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveVerified(string sIssuedId, string baseImage, string sExtension, string fileName, string JsonData)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                string sSQL = string.Empty;
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(sIssuedId))
                {
                    var FetchProgrammeCode = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(new ADM_ISSUED_APPLICATIONS() { ISSUED_ID = sIssuedId }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgramme)).DataSource.Data;
                    var FetchRollno = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS() { ISSUED_ID = sIssuedId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchRollnoByIssuedId)).DataSource.Data;
                    if (FetchRollno != null && FetchRollno.Count > 0)
                    {
                        SaveImage(baseImage, sExtension, FetchProgrammeCode.FirstOrDefault().PROGRAMME_NAME, FetchRollno.FirstOrDefault().RECEIVE_ID);
                    }
                    if (!string.IsNullOrEmpty(JsonData))
                    {
                        var studentdetails = JsonConvert.DeserializeObject<ADM_ISSUE_APPLICATION_2018>(JsonData);


                        var FetchReceiveId = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS() { ISSUED_ID = sIssuedId }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchReceiveByIssueId)).DataSource.Data;
                        if (FetchReceiveId != null && FetchReceiveId.Count > 0)
                        {
                            studentdetails.RECEIVE_ID = FetchReceiveId.FirstOrDefault().RECEIVE_ID;
                            studentdetails.ROLL_NO = FetchReceiveId.FirstOrDefault().ROLL_NO;
                            studentdetails.DATE_OF_BIRTH = CommonMethods.ConvertdatetoyearFromat(studentdetails.DATE_OF_BIRTH);
                            var Result = CMSHandler.SaveOrUpdate(studentdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateVerified));

                            if (Result.Success)
                            {
                                sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.Updatestupersonalinfo).Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                                var Results = CMSHandler.SaveOrUpdate(studentdetails, sSQL, sAcademicYear);
                                ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                                ObjJsonData.Message = Common.Messages.FailedToSaveRecords;

                        }
                    }
                }
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IDcardStatusReport()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                try
                {
                    var ProgrammeList = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByProgrammeGroup), sAcademicYear).DataSource.Data;
                    objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.CP_PROGRAMME_GROUP.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_GROUP.PROGRAMME_NAME);
                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "IDcardStatusReport", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "IDcardStatusReport", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindStatusWiseStudent(string sProgramme, string sSatatus)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sSQL = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(sProgramme) && !string.IsNullOrEmpty(sSatatus))
                    {

                        if (sSatatus == "3")
                        {
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAllstudent).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.PROGRAMME_ID, sProgramme);
                            objViewModel.liIssueApplication = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(null, sSQL).DataSource.Data;
                        }
                        else
                        {
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStatusWiseStudent).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.PROGRAMME_ID, sProgramme);
                            objViewModel.liIssueApplication = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUED_APPLICATIONS() { STATUS = sSatatus }, sSQL).DataSource.Data;
                        }

                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindStatusWiseStudent", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindStatusWiseStudent", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        public ActionResult IDBankReport()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                try
                {
                    var ProgrammeList = (List<CP_PROGRAMME_GROUP>)CMSHandler.FetchData<CP_PROGRAMME_GROUP>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByProgrammeGroup), sAcademicYear).DataSource.Data;
                    objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.CP_PROGRAMME_GROUP.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_GROUP.PROGRAMME_NAME);
                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "IDBankReport", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "IDBankReport", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindVerifiedStudent(string sProgramme)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sSQL = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                try
                {
                    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchIDVerifiedStudent).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.PROGRAMME_ID, sProgramme);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.TODB, CommonMethods.ToDB);
                    objViewModel.liIssueApplication = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(null, sSQL, sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindVerifiedStudent", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindVerifiedStudent", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindStuWiseReport(string sProgramme, string sSatatus)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sSQL = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(sProgramme) && !string.IsNullOrEmpty(sSatatus))
                    {
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStatusWiseStudent).Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_GROUP.PROGRAMME_ID, sProgramme);
                        objViewModel.liIssueApplication = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUED_APPLICATIONS() { STATUS = sSatatus }, sSQL).DataSource.Data;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindStatusWiseStudent", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindStatusWiseStudent", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(objViewModel);
        }
        public ActionResult AccountnoAssign()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {


                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "AccountnoAssign", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "AccountnoAssign", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View();
        }
        public ActionResult BindAccountstatusWiseStudent(string sStatus)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(sStatus))
                    {
                        if (sStatus == "1")
                            ObjViewModel.liIssueApplication = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUED_APPLICATIONS() { STATUS = sStatus }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAccountWisestatus)).DataSource.Data;
                        else if (sStatus == "0")
                            ObjViewModel.liIssueApplication = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUED_APPLICATIONS() { STATUS = sStatus }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchNotcreatedac)).DataSource.Data;
                    }
                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindAccountstatusWiseStudent", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindAccountstatusWiseStudent", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View(ObjViewModel);
        }
        public JsonResult SaveAssignedAccountno(string sJsonData)
        {
            int count = 0;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    JSON_ADM_STU_SUBMARKS objJson = JsonConvert.DeserializeObject<JSON_ADM_STU_SUBMARKS>(sJsonData);
                    if (objJson != null)
                    {
                        foreach (var item in objJson.JSON_ADM_RECEIVE)
                        {
                            var Result = CMSHandler.SaveOrUpdate(item, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateAccountno)).DataSource.Data;
                            count++;
                        }
                        ObjJsonData.Message = count + " " + Common.Messages.RecordsSavedSuccessfully;
                    }

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveAssignedAccountno", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveAssignedAccountno", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ImageCorrection
        public ActionResult Imagecorrection()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "AccountnoAssign", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "AccountnoAssign", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return View();
        }
        public JsonResult SaveStudentImages(string sPath)
        {
            var ObjJsonData = new JsonResultData();
            var ObjStudent = new ADM_RECEIVE_APPLICATION();
            var UserAccount = new USER_ACCOUNT_INFO();
            string _sDirectorypath = string.Empty;
            int COUNT = 0;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                string path = sPath;
                DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles(); //Getting Text files

                var IsExist = new List<ADM_RECEIVE_APPLICATION>();
                var liStudent = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(null,
                     SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentForImagecorrection), sAcademicYear).DataSource.Data;
                string sStudentQuery = string.Empty;
                string sUserQuery = string.Empty;
                foreach (FileInfo file in Files)
                {
                    var sImage = file.Name.Split('.');
                    // Save Image By Roll No 
                    IsExist = liStudent.Where(o => o.ROLL_NO == sImage[0]).ToList();

                    if (IsExist != null && IsExist.Count > 0)
                    {
                        _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_IMGAE_PATH;
                        if (!Directory.Exists(_sDirectorypath))
                        {
                            Directory.CreateDirectory(_sDirectorypath);
                        }
                        _sDirectorypath = string.Empty;
                        _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + sAcademicYear;
                        if (!Directory.Exists(_sDirectorypath))
                        {
                            Directory.CreateDirectory(_sDirectorypath);
                        }
                        string sFilePath = _sDirectorypath + "\\" + file.Name.Replace(sImage[0], IsExist.FirstOrDefault().RECEIVE_ID);
                        System.Drawing.Bitmap b = new System.Drawing.Bitmap(file.DirectoryName + "\\" + file.Name);

                        b.Save(sFilePath);

                        // Save Personal Info Image.
                        ObjStudent.PHOTO_PATH = "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + sAcademicYear + "\\" + file.Name.Replace(sImage[0], IsExist.FirstOrDefault().RECEIVE_ID);
                        ObjStudent.RECEIVE_ID = IsExist.FirstOrDefault().RECEIVE_ID;
                        var UpdatePersonalInfo = CMSHandler.SaveOrUpdate(ObjStudent, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.Updateimageinadmreceiveapplication), sAcademicYear);
                        sStudentQuery += "UPDATE ADM_RECEIVE_APPLICATION SET PHOTO ='" + ObjStudent.PHOTO_PATH + "' WHERE RECEIVE_ID =" + ObjStudent.RECEIVE_ID + ";";
                        COUNT++;

                    }
                }
                var Lisstudent = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentForImagecorrection)).DataSource.Data;
                if (Lisstudent != null && Lisstudent.Count > 0)
                {
                    string s = string.Empty;
                    string s1 = string.Empty;

                    foreach (var item in Lisstudent)
                    {
                        if (!string.IsNullOrEmpty(item.PHOTO_PATH.ToString()))
                        {
                            s = string.Concat(AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", ""), item.PHOTO_PATH.ToString().Trim());
                            if (System.IO.File.Exists(s))
                            {
                                Image img = Image.FromFile(s);
                                _sDirectorypath = string.Empty;
                                _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.ID_IMAGE_PATH;
                                if (!Directory.Exists(_sDirectorypath))
                                {
                                    Directory.CreateDirectory(_sDirectorypath);
                                }
                                _sDirectorypath = string.Empty;
                                _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.ID_IMAGE_PATH + "\\" + sAcademicYear;
                                if (!Directory.Exists(_sDirectorypath))
                                {
                                    Directory.CreateDirectory(_sDirectorypath);
                                }
                                string sFilePath = _sDirectorypath + "\\" + item.ROLL_NO.ToString().Trim() + ".jpg";
                                img.Save(sFilePath);
                            }
                        }
                    }
                }

            }
            ObjJsonData.Message = "(" + COUNT + ")" + "Request Completed Successfully :) !.";
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveStudentImage()
        {
            var ObjJsonData = new JsonResultData();
            var ObjStudent = new ADM_RECEIVE_APPLICATION();
            var UserAccount = new USER_ACCOUNT_INFO();
            string _sDirectorypath = string.Empty;
            int COUNT = 0;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {

                DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/Image/New_images"));//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles(); //Getting Text files

                var IsExist = new List<ADM_RECEIVE_APPLICATION>();
                var liStudent = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(null,
                     SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentForImagecorrection), sAcademicYear).DataSource.Data;
                string sStudentQuery = string.Empty;
                string sUserQuery = string.Empty;
                foreach (FileInfo file in Files)
                {
                    var sImage = file.Name.Split('.');
                    // Save Image By Roll No 
                    IsExist = liStudent.Where(o => o.ROLL_NO == sImage[0]).ToList();

                    if (IsExist != null && IsExist.Count > 0)
                    {
                        _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_IMGAE_PATH;
                        if (!Directory.Exists(_sDirectorypath))
                        {
                            Directory.CreateDirectory(_sDirectorypath);
                        }
                        _sDirectorypath = string.Empty;
                        _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + sAcademicYear;
                        if (!Directory.Exists(_sDirectorypath))
                        {
                            Directory.CreateDirectory(_sDirectorypath);
                        }
                        string sFilePath = _sDirectorypath + "\\" + file.Name.Replace(sImage[0], IsExist.FirstOrDefault().RECEIVE_ID);
                        System.Drawing.Bitmap b = new System.Drawing.Bitmap(file.DirectoryName + "\\" + file.Name);

                        b.Save(sFilePath);

                        // Save Personal Info Image.
                        ObjStudent.PHOTO_PATH = "\\" + Common.URLPages.STUDENT_IMGAE_PATH + "\\" + sAcademicYear + "\\" + file.Name.Replace(sImage[0], IsExist.FirstOrDefault().RECEIVE_ID);
                        ObjStudent.RECEIVE_ID = IsExist.FirstOrDefault().RECEIVE_ID;
                        var UpdatePersonalInfo = CMSHandler.SaveOrUpdate(ObjStudent, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.Updateimageinadmreceiveapplication), sAcademicYear);
                        sStudentQuery += "UPDATE ADM_RECEIVE_APPLICATION SET PHOTO ='" + ObjStudent.PHOTO_PATH + "' WHERE RECEIVE_ID =" + ObjStudent.RECEIVE_ID + ";";
                        COUNT++;

                    }
                }
                var Lisstudent = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentForImagecorrection)).DataSource.Data;
                if (Lisstudent != null && Lisstudent.Count > 0)
                {
                    string s = string.Empty;
                    string s1 = string.Empty;

                    foreach (var item in Lisstudent)
                    {
                        if (!string.IsNullOrEmpty(item.PHOTO_PATH.ToString()))
                        {
                            s = string.Concat(AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", ""), item.PHOTO_PATH.ToString().Trim());
                            if (System.IO.File.Exists(s))
                            {
                                Image img = Image.FromFile(s);
                                _sDirectorypath = string.Empty;
                                _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.ID_IMAGE_PATH;
                                if (!Directory.Exists(_sDirectorypath))
                                {
                                    Directory.CreateDirectory(_sDirectorypath);
                                }
                                _sDirectorypath = string.Empty;
                                _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.ID_IMAGE_PATH + "\\" + sAcademicYear;
                                if (!Directory.Exists(_sDirectorypath))
                                {
                                    Directory.CreateDirectory(_sDirectorypath);
                                }
                                string sFilePath = _sDirectorypath + "\\" + item.ROLL_NO.ToString().Trim() + ".jpg";
                                img.Save(sFilePath);
                            }
                        }
                    }
                }

            }
            ObjJsonData.Message = "(" + COUNT + ")" + "Request Completed Successfully :) !.";
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ImagesByReceiveId()
        {
            AdmissionViewModel ObjViewModel = new AdmissionViewModel();
            try
            {
                var Fetchstudent = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAlladmittedList)).DataSource.Data;
                ObjViewModel.liApplicant = Fetchstudent;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ImagesByReceiveId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ImagesByReceiveId", ex.Message);
                }
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
            return View(ObjViewModel);
        }
        public ActionResult SaveStudentImagesByReceiveId(string sRollno)
        {
            string sSQL = string.Empty;
            AdmissionViewModel ObjViewModel = new AdmissionViewModel();
            string _sDirectorypath = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sRollno))
                {
                    sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStudentsByRollno).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.ROLL_NO, sRollno);
                    var Fetchstudent = (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(null, sSQL).DataSource.Data;
                    ObjViewModel.liApplicant = Fetchstudent;

                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "SaveStudentImagesByReceiveId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "SaveStudentImagesByReceiveId", ex.Message);
                }
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
            return View(ObjViewModel);
        }
        #endregion

        #region Entrance Exam MarkEntry

        public ActionResult EntranceExamMarkEntry()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    string sUserId = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(new stf_Personal_Info() { STAFF_ID = sUserId, }, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeInchargeByStaffId), sAcademicYear).DataSource.Data;
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "EntranceExamMarkEntry", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "EntranceExamMarkEntry", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
            return View(objViewModel);
        }


        public ActionResult BindStudentEntranceExamMarkEntry(string sProgrammeGroupId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    objViewModel.liIssuedApplictionsInfo = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS() { PROGRAMME_GROUP_ID = sProgrammeGroupId, STATUS = Common.STATUS.Submitted }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSubmittedStudentForEntranceExam), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindStudentEntranceExamMarkEntry", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindStudentEntranceExamMarkEntry", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
            return View(objViewModel);
        }

        public JsonResult SaveStudentEntranceExamMarkEntry(string sJsonData)
        {
            JsonResultData objResultData = new JsonResultData();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            int sCount = 0;
            int fcount = 0;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JSON_ADM_STU_SUBMARKS objJson = serializer.Deserialize<JSON_ADM_STU_SUBMARKS>(sJsonData);
                    if (objJson.JSON_ISSUED_APPLICATIONS != null && objJson.JSON_ISSUED_APPLICATIONS.Count > 0)
                    {
                        foreach (var result in objJson.JSON_ISSUED_APPLICATIONS)
                        {
                            if (!string.IsNullOrEmpty(result.ENTRANCE_MARK))
                            {
                                ResultArgs resultArgs = CMSHandler.SaveOrUpdate(result, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateEntranceExamMarks), sAcademicYear);
                                if (resultArgs.Success)
                                {
                                    sCount++;
                                }

                            }
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Examination", "AddStaffForExamSeating", ex.Message);
                    }
                }
                if (sCount != 0)
                {
                    objResultData.ErrorCode = Common.ErrorCode.Accepted;
                    objResultData.Message = Common.Delimiter.OPENBRACKET + sCount + Common.Delimiter.CLOSEBRACKET + Common.Messages.RecordsSavedSuccessfully;
                }
                else
                {
                    objResultData.ErrorCode = Common.ErrorCode.NotAcceptable;
                    objResultData.Message = Common.Messages.FailedToSaveRecords;
                }
            }
            return Json(objResultData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintStudentEntranceExamMark(string sProgrammeGroupId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    objViewModel.liIssuedApplictionsInfo = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS() { PROGRAMME_GROUP_ID = sProgrammeGroupId, STATUS = Common.STATUS.Submitted }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchSubmittedStudentForEntranceExam), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "BindStudentEntranceExamMarkEntry", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "BindStudentEntranceExamMarkEntry", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
            return View(objViewModel);
        }
        #endregion

        #region Admission Transfer
        //Transfer Applicant Request
        public async Task<ActionResult> TrasnsferRequest()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sReceiveId = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    objViewModel.liIssuedApplictionsInfo = await Task.Run(() => (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAppliedApplicationInfoForTransferRequest), sAcademicYear).DataSource.Data);

                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "TrasnsferRequest", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "TrasnsferRequest", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
            return View(objViewModel);
        }

        public JsonResult FetchProgrammesForTransferRequest()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sOption = string.Empty;
            try
            {
                var Applicantinfo = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS { RECEIVE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.ApproveTransferApplicant), sAcademicYear).DataSource.Data;
                if (Applicantinfo != null && Applicantinfo.Count > 0)
                {

                    string sProgrammeMode = string.Join(",", Applicantinfo.GroupBy(x => x.PROGRAMME_MODE).Select(x => x.First()).ToString());
                    string sShift = string.Join(",", Applicantinfo.GroupBy(x => x.SHIFT).Select(x => x.First()).ToString());
                    var programme = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS { RECEIVE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.ApproveTransferApplicant), sAcademicYear).DataSource.Data;
                    if (programme != null && programme.Count > 0)
                    {
                        foreach (var item in programme)
                        {
                            sOption += "<option value=" + item.PROGRAMME_GROUP_ID + ">" + item.PROGRAMME_NAME + "</option>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "FetchProgrammesForTransferRequest", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "FetchProgrammesForTransferRequest", ex.Message);
                }
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
            return Json(sOption, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveTrasnsferRequest(ADM_TRANSFER_REQUEST objtransferRequest)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var issuedApplication = (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS { ISSUED_ID = objtransferRequest.ISSUED_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.ApproveTransferApplicant), sAcademicYear).DataSource.Data;
                    if (issuedApplication != null && issuedApplication.Count > 0)
                    {
                        objtransferRequest.RECEIVE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                        objtransferRequest.ISSUED_ID = issuedApplication.FirstOrDefault().ISSUED_ID;
                        objtransferRequest.ISSUED_STATUS = issuedApplication.FirstOrDefault().STATUS;
                        var Save = CMSHandler.SaveOrUpdate(objtransferRequest, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.DeleteProgrammeDescription), sAcademicYear);
                        ObjJsonData.Message = (Save.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveTrasnsferRequest", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveTrasnsferRequest", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteTrasnsferRequest(string sTransferRequestId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    if (!string.IsNullOrEmpty(sTransferRequestId))
                    {
                        var sresultArgs = CMSHandler.SaveOrUpdate(new ADM_TRANSFER_REQUEST { TRANSFER_REQUEST_ID = sTransferRequestId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionClasses));
                        ObjJsonData.Message = (sresultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                    }
                    else
                    {
                        ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                        ObjJsonData.Message = Common.ErrorMessage.ExpectationFailed;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "DeleteTrasnsferRequest", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "DeleteTrasnsferRequest", ex.Message);
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Admission tranfer By Office
        public ActionResult AdmissionApplicationTranfer()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    objViewModel.LiTransferapprovalApplicant = (List<ADM_TRANSFER_APPROVAL>)CMSHandler.FetchData<ADM_TRANSFER_APPROVAL>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApprovedTransferForTransfer), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "AdmissionApplicationTranfer", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "AdmissionApplicationTranfer", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
            return View(objViewModel);
        }

        public JsonResult SaveAdmissionApplicationTranfer(string sTransferRequestId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var objTransferApproval = new ADM_TRANSFER_APPROVAL();
            //var objAdmTransfer = new ADM_REFUND_STUDENT_ACCOUNT();
            var objAdmTransfer = new ADM_TRANSFER();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var TransferApproval = (List<ADM_TRANSFER_APPROVAL>)CMSHandler.FetchData<ADM_TRANSFER_APPROVAL>(new ADM_TRANSFER_APPROVAL { TRANSFER_REQUEST_ID = sTransferRequestId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchTransferApprovalByTransferRequestId), sAcademicYear).DataSource.Data;
                    if (TransferApproval != null && TransferApproval.Count > 0)
                    {
                        var Updatestaus = new ResultArgs();
                        var SaveTransfer = new ResultArgs();
                        var Result = new ResultArgs();
                        var status = TransferApproval.FirstOrDefault().ISSUED_STATUS;
                        var IsProgrammeExisit = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(objTransferApproval, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.IsProgrammeExistByReceiveId)).DataSource.Data;
                        switch (status)
                        {
                            //Registered
                            case Common.CONST_STATUS.Registered:
                                //if (LiFetchIssueId != null & LiFetchIssueId.Count > 0)
                                //{

                                if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                                {
                                    objAdmTransfer.ISSUE_ID = TransferApproval.FirstOrDefault().ISSUED_ID;
                                    objAdmTransfer.STATUS_ID = Common.STATUS.Registered;
                                    Result = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusForTransfer));
                                }
                                else
                                {
                                    objAdmTransfer.ISSUE_ID = TransferApproval.FirstOrDefault().ISSUED_ID;
                                    objAdmTransfer.STATUS_ID = Common.STATUS.Registered;
                                    Updatestaus = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertProgrammeIdForTransfer));
                                }
                                if (Updatestaus.Success || Result.Success)
                                {
                                    SaveTransfer = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateIsTransferedForTransfer), sAcademicYear);

                                }
                                //  }
                                break;
                            //Submitted
                            case Common.CONST_STATUS.Submitted:
                                //  if (LiFetchIssueId != null & LiFetchIssueId.Count > 0)
                                // {
                                if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                                {
                                    objAdmTransfer.ISSUE_ID = TransferApproval.FirstOrDefault().ISSUED_ID;
                                    objAdmTransfer.STATUS_ID = Common.STATUS.Submitted;
                                    Result = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusForTransfer));
                                }
                                else
                                {
                                    objAdmTransfer.ISSUE_ID = TransferApproval.FirstOrDefault().ISSUED_ID;
                                    objAdmTransfer.STATUS_ID = Common.STATUS.Submitted;
                                    Updatestaus = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertProgrammeIdForTransfer));
                                }
                                if (Updatestaus.Success || Result.Success)
                                {
                                    SaveTransfer = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateIsTransferedForTransfer), sAcademicYear);
                                }

                                //}
                                break;
                            //Selected
                            case Common.CONST_STATUS.Selected:
                                //  if (LiFetchIssueId != null && LiFetchIssueId.Count > 0)
                                //{
                                if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                                {
                                    objAdmTransfer.ISSUE_ID = TransferApproval.FirstOrDefault().ISSUED_ID;
                                    objAdmTransfer.STATUS_ID = Common.STATUS.Submitted;
                                    Result = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateStatusForTransfer));
                                }
                                else
                                {

                                    objAdmTransfer.ISSUE_ID = TransferApproval.FirstOrDefault().ISSUED_ID;
                                    objAdmTransfer.STATUS_ID = Common.STATUS.Submitted;
                                    Updatestaus = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertProgrammeIdForTransfer));
                                }
                                if (Updatestaus.Success || Result.Success)
                                {
                                    new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                                    var SelectionUpdate = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.DeleteApplicantByProgrammeAndReceiveId), sAcademicYear);
                                    new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                    if (SelectionUpdate.Success)
                                    {
                                        SaveTransfer = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateIsTransferedForTransfer), sAcademicYear);

                                    }
                                }

                                //}
                                break;
                            //Verified
                            case Common.CONST_STATUS.Verified:
                                //if (LiFetchIssueId != null && LiFetchIssueId.Count > 0)
                                //{
                                if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                                {
                                    objAdmTransfer.ISSUE_ID = TransferApproval.FirstOrDefault().ISSUED_ID;
                                    objAdmTransfer.STATUS_ID = Common.STATUS.Submitted;
                                    Result = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertFromProgrammeByIssuedId));
                                }
                                else
                                {
                                    objAdmTransfer.ISSUE_ID = TransferApproval.FirstOrDefault().ISSUED_ID;
                                    objAdmTransfer.STATUS_ID = Common.STATUS.Selected;
                                    Updatestaus = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdatePorgrammeByIssueId));
                                }
                                //}
                                if (Updatestaus.Success || Result.Success)
                                {
                                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                                        objAdmTransfer.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                                    new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                                    var DeleteFee = CMSHandler.SaveOrUpdate(objAdmTransfer, FeeSQL.GetFeeSQL(FeeSQLCommands.DeleteFeeStudentAccountByReceiveIdandFrequencyId));
                                    new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                    if (DeleteFee.Success)
                                    {
                                        new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                                        var SelectionUpdate = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateSelectionByReceiveIdandProgrammeId), sAcademicYear);
                                        new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                        if (SelectionUpdate.Success)
                                        {
                                            SaveTransfer = CMSHandler.SaveOrUpdate(objAdmTransfer, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateIsTransferedForTransfer), sAcademicYear);
                                        }
                                    }
                                }
                                break;
                            //Admitted
                            case Common.CONST_STATUS.Admitted:
                                //if (IsProgrammeExisit != null && IsProgrammeExisit.Count > 0)
                                //{
                                //    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                //    Transferdetails.STATUS_ID = Common.STATUS.Submitted;
                                //    Result = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.RevertFromProgrammeByIssuedId));
                                //    if (Result.Success)
                                //    {
                                //        SaveTransfer = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.SaveAdmTransfer), sAcademicYear);
                                //    }
                                //}
                                //new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateoff));
                                //var UpdateSelection = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.DeleteandUpdateCancelByReceiveandProgrammeId), sAcademicYear);
                                //new MySQLHandler().ExecuteAsScripts(SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.UpdatesafeUpdateon));
                                //if (UpdateSelection.Success)
                                //{
                                //    Transferdetails.ISSUE_ID = LiFetchIssueId.FirstOrDefault().ISSUE_ID;
                                //    Transferdetails.STATUS_ID = Common.STATUS.Cancelled;
                                //    Updatestaus = CMSHandler.SaveOrUpdate(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.UpdateCancel));
                                //    objViewModel.liAdmissionCancel = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCanceldetails), sAcademicYear).DataSource.Data;
                                //    if (Result.Success)
                                //    {
                                //        objViewModel.liTransferExist = IsProgrammeExisit;
                                //    }
                                //    return View(objViewModel);
                                //}
                                break;

                        }

                        //if (SaveTransfer.Success)
                        //{
                        //    objViewModel.liTransfer = (List<ADM_TRANSFER>)CMSHandler.FetchData<ADM_TRANSFER>(Transferdetails, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchTransferDetails), sAcademicYear).DataSource.Data;
                        //}
                        //if (Result.Success)
                        //{
                        //    objViewModel.liTransferExist = IsProgrammeExisit;

                        //}
                    }
                    else
                    {
                        ObjJsonData.ErrorCode = Common.ErrorCode.ExpectationFailed;
                        ObjJsonData.Message = Common.ErrorMessage.ExpectationFailed;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveAdmissionApplicationTranfer", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveAdmissionApplicationTranfer", ex.Message);
                    }
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FetchStudentAccountForRefund(string sTransferRequestId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var TransferApproval = (List<ADM_TRANSFER_APPROVAL>)CMSHandler.FetchData<ADM_TRANSFER_APPROVAL>(new ADM_TRANSFER_APPROVAL { TRANSFER_REQUEST_ID = sTransferRequestId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchTransferApprovalByTransferRequestId), sAcademicYear).DataSource.Data;
                    if (TransferApproval != null && TransferApproval.Count > 0)
                    {
                        var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                        var FetchFeeTransaction = (List<FEE_TRANSACTION>)CMSHandler.FetchData<FEE_TRANSACTION>(new FEE_TRANSACTION() { STUDENT_ID = TransferApproval.FirstOrDefault().RECEIVE_ID, FREQUENCY = FetchFrequency.FirstOrDefault().FREQUENCY_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchFeeTransactionForRefundByStudentId), sAcademicYear).DataSource.Data;
                        objViewModel.LiRefundStudentAccount = (List<ADM_REFUND_STUDENT_ACCOUNT>)CMSHandler.FetchData<ADM_REFUND_STUDENT_ACCOUNT>(new ADM_REFUND_STUDENT_ACCOUNT { STUDENT_ID = TransferApproval.FirstOrDefault().RECEIVE_ID, TRANSACTION_ID = FetchFeeTransaction.FirstOrDefault().TRANSACTION_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentPaidAmountForRefund), sAcademicYear).DataSource.Data;
                        objViewModel.LiRefundStudentAccount.ForEach(O => O.FREQUENCY_ID = FetchFeeTransaction.FirstOrDefault().FREQUENCY);
                        objViewModel.LiRefundStudentAccount.ForEach(O => O.ISSUED_ID = TransferApproval.FirstOrDefault().ISSUED_ID);
                    }

                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "AdmissionApplicationTranfer", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "AdmissionApplicationTranfer", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
            return View(objViewModel);
        }



        public JsonResult SaveRefundStuentAccount(string sJsonData)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sUserId = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            bool refundresult = false;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JSON_ADM_STU_SUBMARKS objJson = serializer.Deserialize<JSON_ADM_STU_SUBMARKS>(sJsonData);
                    if (objJson.JSON_ADM_REFUND_STUDENT_ACCOUNT != null && objJson.JSON_ADM_REFUND_STUDENT_ACCOUNT.Count > 0)
                    {
                        string InsertQuery = @"INSERT INTO ADM_REFUND_STUDENT_ACCOUNT (STUDENT_ID, ISSUED_ID, FREQUENCY_ID, HEAD, AMOUNT, FEE_MAIN_HEAD_ID, FEE_STRUCTURE_ID, FEE_ROOT_ID, TRANSACTION_ID, REFUND_TYPE, REFUND_BY, ACADEMIC_YEAR) VALUES";
                        foreach (var result in objJson.JSON_ADM_REFUND_STUDENT_ACCOUNT)
                        {
                            objJson.JSON_ADM_REFUND_STUDENT_ACCOUNT.FirstOrDefault().REFUND_BY = sUserId;
                            InsertQuery += @"('" + result.STUDENT_ID + "','" + result.ISSUED_ID + "','" + result.FREQUENCY_ID + "','" + result.HEAD + "','" + result.AMOUNT + "','" + result.FEE_MAIN_HEAD_ID + "','" + result.FEE_STRUCTURE_ID + "','" + result.FEE_ROOT_ID + "','" + result.TRANSACTION_ID + "','" + result.STUDENT_ID + "','" + sUserId + "','" + sAcademicYear + "'),";
                        }
                        InsertQuery = InsertQuery.TrimEnd(',');
                        var resultArgs = CMSHandler.SaveOrUpdate(null, InsertQuery, sAcademicYear);
                        if (resultArgs.Success)
                        {
                            using (FeeController objfee = new FeeController())
                            {
                                refundresult = objfee.FeeRazorpayRefund(objJson.JSON_ADM_REFUND_STUDENT_ACCOUNT.FirstOrDefault().RAZORPAY_ID);
                                if (refundresult)
                                {
                                    ObjJsonData.ErrorCode = Common.ErrorCode.Ok;
                                    ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                                }
                            }
                        }
                        else
                        {
                            ObjJsonData.ErrorCode = Common.ErrorCode.ExpectationFailed;
                            ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveRefundStuentAccount", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveRefundStuentAccount", ex.Message);
                    }
                }
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Admission approval

        public async Task<ActionResult> TrasnsferApproval()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sReceiveId = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    objViewModel.liAdmTransferRequest = await Task.Run(() => (List<ADM_TRANSFER_REQUEST>)CMSHandler.FetchData<ADM_TRANSFER_REQUEST>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAllApplicantTransForRequest), sAcademicYear).DataSource.Data);

                }
                catch (Exception ex)
                {

                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "TrasnsferApproval", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "TrasnsferApproval", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "error");
            }
            return View(objViewModel);
        }


        public JsonResult FetchAllTransferRequestApplicationsByReceiveId(string sReceiveId)
        {
            JsonResultData objResultData = new JsonResultData();
            string sTransferStatus = "";
            string sApprovedContent = "";
            if (!string.IsNullOrEmpty(sReceiveId))
            {
                objResultData.oResult = (List<ADM_TRANSFER_REQUEST>)CMSHandler.FetchData<ADM_TRANSFER_REQUEST>(new ADM_TRANSFER_REQUEST() { RECEIVE_ID = sReceiveId }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAllApplicantTransForRequestByReceiveId)).DataSource.Data;
            }

            // adm transfer status

            var liTransferData = (List<SUP_ADM_TRANSFER_STATUS>)CMSHandler.FetchData<SUP_ADM_TRANSFER_STATUS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupAdmTransferStatus)).DataSource.Data;

            if (liTransferData != null && liTransferData.Count > 0)
            {
                sTransferStatus += "<select class='show-menu - arrow show - tick drop - down form - control  input - sm' data-live-search='true' id='ddlTransferStatus' name='TransferStatus' style='display: none;'><option value=''>--select--</option>";
                foreach (var item in liTransferData)
                {
                    sTransferStatus += "<option value=" + item.ADM_TRANSFER_STATUS_ID + ">" + item.STATUS_NAME + "</option>";
                }
                sTransferStatus += "</select>";
                objResultData.sResultStringArray.Add(sTransferStatus);
            }

            // adm approval content

            var liApprovedContent = (List<SUP_ADM_APPROVED_CONTENT>)CMSHandler.FetchData<SUP_ADM_APPROVED_CONTENT>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupAdmApprovedContent)).DataSource.Data;

            if (liApprovedContent != null && liApprovedContent.Count > 0)
            {
                sApprovedContent += "<select class='show-menu - arrow show - tick drop - down form - control  input - sm' data-live-search='true' id='ddlApprovedContent' name='ApprovedContent' style='display: none;'><option value=''>--select--</option>";
                foreach (var item in liApprovedContent)
                {
                    sApprovedContent += "<option value=" + item.ADM_APPROVED_CONTENT_ID + ">" + item.CONTENT_NAME + "</option>";
                }
                sApprovedContent += "</select>";
                objResultData.sResultStringArray.Add(sApprovedContent);
            }

            return Json(objResultData, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objtransferApproval"></param>
        /// <returns></returns>

        public JsonResult SaveTrasnsferApproval(ADM_TRANSFER_APPROVAL objtransferApproval)
        {
            JsonResultData objResultData = new JsonResultData();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sUserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    //fetch adm transfer requeste 
                    var sTransfer = (List<ADM_TRANSFER_REQUEST>)CMSHandler.FetchData<ADM_TRANSFER_REQUEST>(new ADM_TRANSFER_REQUEST() { TRANSFER_REQUEST_ID = objtransferApproval.TRANSFER_REQUEST_ID }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmTransferRequestByTransferRequestId), sAcademicYear).DataSource.Data;

                    if (sTransfer != null && sTransfer.Count > 0)
                    {
                        objtransferApproval.RECEIVE_ID = sTransfer.FirstOrDefault().RECEIVE_ID;
                        objtransferApproval.ISSUED_ID = sTransfer.FirstOrDefault().ISSUED_ID;
                        objtransferApproval.APPLICATION_NO = sTransfer.FirstOrDefault().APPLICATION_NO;
                        objtransferApproval.PROGRAMME_FROM = sTransfer.FirstOrDefault().PROGRAMME_FROM;
                        objtransferApproval.PROGRAMME_TO = sTransfer.FirstOrDefault().PROGRAMME_TO;
                        objtransferApproval.ISSUED_STATUS = sTransfer.FirstOrDefault().ISSUED_STATUS;
                        objtransferApproval.APPROVED_BY = sUserId;


                        // save adm transfer approval
                        var result = CMS.DAO.CMSHandler.SaveOrUpdate(objtransferApproval, SQL.Admission.AdmissionSQL.GetAdmissionSQL(DAO.AdmissionSQLCommands.SaveAdmTransferApproved), sAcademicYear);
                        if (result.Success)
                        {
                            // save adm transfer requested 
                            var result1 = CMS.DAO.CMSHandler.SaveOrUpdate(new ADM_TRANSFER_REQUEST() { TRANSFER_REQUEST_ID = objtransferApproval.TRANSFER_REQUEST_ID, TRANSFER_STATUS = objtransferApproval.TRANSFER_STATUS }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(DAO.AdmissionSQLCommands.UpdateAdmTransferRequetedForApproved), sAcademicYear);

                            if (result1.Success)
                            {
                                objResultData.Message = Common.Messages.RecordsSavedSuccessfully;


                                //send sms

                                var MessageContent = (List<SMS_USERDEFINED_TEMPLATE>)CMSHandler.FetchData<SMS_USERDEFINED_TEMPLATE>(new SMS_USERDEFINED_TEMPLATE() { TEMPLATE_ID = Common.SMS_TEMPLATE.TransferApprove }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchsmsTemplateByTemplateId), sAcademicYear).DataSource.Data;
                                string sContent = MessageContent.FirstOrDefault().TEMPLATE_TEXT.Replace(Common.Delimiter.AT + Common.ADM_ISSUE_APPLICATION_2018.FIRST_NAME, sTransfer.FirstOrDefault().FIRST_NAME).Replace(Common.Delimiter.AT + Common.ADM_TRANSFER_REQUEST.PROGRAMME_FROM, sTransfer.FirstOrDefault().PROGRAMME_FROM_NAME).Replace(Common.Delimiter.AT + Common.ADM_TRANSFER_REQUEST.PROGRAMME_TO, sTransfer.FirstOrDefault().PROGRAMME_TO_NAME).Replace(Common.Delimiter.AT + Common.ADM_TRANSFER_REQUEST.TRANSFER_STATUS, objtransferApproval.TRANSFER_STATUS_MESSAGE).Replace(Common.Delimiter.AT + Common.ADM_TRANSFER_REQUEST.APPROVED_CONTENT, objtransferApproval.APPROVED_CONTENT_MESSAGE);
                                SendEmailAndTextMessage(sContent, sTransfer.FirstOrDefault().SMS_MOBILE_NO, "", "");
                            }
                            else
                            {
                                objResultData.Message = objResultData.Message = Common.Messages.FailedToSaveRecords;
                            }
                        }
                        else
                        {
                            objResultData.Message = Common.Messages.FailedToSaveRecords;
                        }
                    }
                    else
                    {
                        objResultData.Message = "Something went wrong!";
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "SaveTrasnsferApproval", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "SaveTrasnsferApproval", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            return Json(objResultData, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Name Correction


        public ActionResult ApplicantNameCorrection()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AdmissionController", "RollnoCreation", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("AdmissionController", "RollnoCreation", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(objViewModel);
        }

        #endregion
        #region Application Form Print
        public ActionResult ApplicationFormForApplicant()
        {
            var ObjViewModel = new AdmissionViewModel();
            //string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            //string sUserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            //string sReceiveId = (Session[Common.SESSION_VARIABLES.RECEIVE_ID] != null) ? Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() : string.Empty;

            //if (!string.IsNullOrEmpty(sAcademicYear))
            //{
            //    try
            //    {
            //        ObjViewModel.liApplicationForm = (List<APPLICATION_FORM>)CMSHandler.FetchData<APPLICATION_FORM>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = sReceiveId },
            //            SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationFormByReceiveId), sAcademicYear).DataSource.Data;
            //        ObjViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(new ADM_STU_SUBMARKS() { RECEIVE_STUID = sReceiveId },
            //            SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuSubMarksByReceivedId), sAcademicYear).DataSource.Data;
            //    }
            //    catch (Exception ex)
            //    {
            //        using (ErrorLog ObjLog = new ErrorLog())
            //        {
            //            ObjLog.WriteError("AdmissionController", "ApplicationFormForApplicant", ex.Message);
            //        }
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("ErrorMessage", "Error");
            //}
            return View(ObjViewModel);
        }

        public ActionResult ApplicationPrintView()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sUserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            string sReceiveId = (Session[Common.SESSION_VARIABLES.RECEIVE_ID] != null) ? Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() : string.Empty;
            var ObjViewModel = new AdmissionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    ObjViewModel.liApplicationForm = (List<APPLICATION_FORM>)CMSHandler.FetchData<APPLICATION_FORM>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = sReceiveId },
                        SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationFormByReceiveId), sAcademicYear).DataSource.Data;
                    //if (ObjViewModel.liApplicationForm!=null && ObjViewModel.liApplicationForm.Count>0)
                    //{
                    //    byte[] smybytes = System.Convert.FromBase64String(ObjViewModel.liApplicationForm.FirstOrDefault().NAME_IN_NATIVE);
                    //    ObjViewModel.liApplicationForm.FirstOrDefault().NAME_IN_NATIVE  = System.Text.Encoding.UTF8.GetString(smybytes);

                    //    byte[] fmybytes = System.Convert.FromBase64String(ObjViewModel.liApplicationForm.FirstOrDefault().FATHER_NAME_IN_NATIVE);
                    //    ObjViewModel.liApplicationForm.FirstOrDefault().FATHER_NAME_IN_NATIVE = System.Text.Encoding.UTF8.GetString(fmybytes);

                    //    byte[] mmybytes = System.Convert.FromBase64String(ObjViewModel.liApplicationForm.FirstOrDefault().MOTHER_NAME_IN_NATIVE);
                    //    ObjViewModel.liApplicationForm.FirstOrDefault().MOTHER_NAME_IN_NATIVE = System.Text.Encoding.UTF8.GetString(mmybytes);

                    //}
                    ObjViewModel.liADMStudentMarksFor11th = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(new ADM_STU_SUBMARKS() { RECEIVE_STUID = sReceiveId }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHssSubjectsFor11ThByReceiveId), sAcademicYear).DataSource.Data;
                    ObjViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(new ADM_STU_SUBMARKS() { RECEIVE_STUID = sReceiveId },
                        SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuSubMarksByReceivedId), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog ObjLog = new ErrorLog())
                    {
                        ObjLog.WriteError("AdmissionController", "ApplicationFormForApplicant", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(ObjViewModel);
        }

        public ActionResult ApplicationPrintViewByReceiveId(string id, string sProgrammeId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

            string sReceiveId = id;
            var ObjViewModel = new AdmissionViewModel();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    ObjViewModel.liApplicationForm = (List<APPLICATION_FORM>)CMSHandler.FetchData<APPLICATION_FORM>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = sReceiveId, PROGRAMME_GROUP_ID = sProgrammeId },
                        SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationFormByProgrammeId), sAcademicYear).DataSource.Data;
                    //if (ObjViewModel.liApplicationForm!=null && ObjViewModel.liApplicationForm.Count>0)
                    //{
                    //    byte[] smybytes = System.Convert.FromBase64String(ObjViewModel.liApplicationForm.FirstOrDefault().NAME_IN_NATIVE);
                    //    ObjViewModel.liApplicationForm.FirstOrDefault().NAME_IN_NATIVE  = System.Text.Encoding.UTF8.GetString(smybytes);

                    //    byte[] fmybytes = System.Convert.FromBase64String(ObjViewModel.liApplicationForm.FirstOrDefault().FATHER_NAME_IN_NATIVE);
                    //    ObjViewModel.liApplicationForm.FirstOrDefault().FATHER_NAME_IN_NATIVE = System.Text.Encoding.UTF8.GetString(fmybytes);

                    //    byte[] mmybytes = System.Convert.FromBase64String(ObjViewModel.liApplicationForm.FirstOrDefault().MOTHER_NAME_IN_NATIVE);
                    //    ObjViewModel.liApplicationForm.FirstOrDefault().MOTHER_NAME_IN_NATIVE = System.Text.Encoding.UTF8.GetString(mmybytes);

                    //}
                    ObjViewModel.liADMStudentMarksFor11th = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(new ADM_STU_SUBMARKS() { RECEIVE_STUID = sReceiveId }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHssSubjectsFor11ThByReceiveId), sAcademicYear).DataSource.Data;
                    ObjViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(new ADM_STU_SUBMARKS() { RECEIVE_STUID = sReceiveId },
                        SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStuSubMarksByReceivedId), sAcademicYear).DataSource.Data;
                }
                catch (Exception ex)
                {
                    using (ErrorLog ObjLog = new ErrorLog())
                    {
                        ObjLog.WriteError("AdmissionController", "ApplicationFormForApplicant", ex.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
            return View(ObjViewModel);
        }

        public ActionResult ListAdmittedStudents()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    if (ProgrammeList != null && ProgrammeList.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "ListAdmittedStudents", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "ListAdmittedStudents", ex.Message);
                }
            }
            return View(objViewModel);

        }

        public ActionResult BindAdmittedStudentsApplication(string sProgrammeId)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_SELECTION_PROCESS objModel = new ADM_SELECTION_PROCESS();
            string sSQL = string.Empty;
            try
            {
                objModel.STATUS = Common.STATUS.Admitted;
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var FetchFrequency = (List<FEE_FREQUENCY_FEE_MODE>)CMSHandler.FetchData<FEE_FREQUENCY_FEE_MODE>(new FEE_FREQUENCY_FEE_MODE() { FEE_MODE = Common.FrequencyType.AdmissionFee }, FeeSQL.GetFeeSQL(FeeSQLCommands.FetchFrequencyIdByFeeMode), sAcademicYear).DataSource.Data;
                    if (FetchFrequency != null && FetchFrequency.Count > 0)
                    {
                        objModel.FREQUENCY_ID = FetchFrequency.FirstOrDefault().FREQUENCY_ID;
                        sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmittedStudentListByProgrammeId);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId);
                        objViewModel.liSelectionProcess = (List<ADM_SELECTION_PROCESS>)CMSHandler.FetchData<ADM_SELECTION_PROCESS>(objModel, sSQL, sAcademicYear).DataSource.Data;
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudents", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindAdmittedStudents", ex.Message);
                }
            }
            return View(objViewModel);
        }
        #endregion

        #region Admission final report
        public ActionResult AdmissionReport()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var ApplicationTypeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType), sAcademicYear).DataSource.Data;
                    // var ProgrammeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchProgrammeByApprogramme), sAcademicYear).DataSource.Data;
                    var AppSubmissionTypeList = (List<SUP_APPLICATION_SUBMISSION_TYPE>)CMSHandler.FetchData<SUP_APPLICATION_SUBMISSION_TYPE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupApplicationSubmissionType)).DataSource.Data;
                    if (ApplicationTypeList != null && ApplicationTypeList.Count > 0)
                        objViewModel.ApplicationTypeList = new SelectList(ApplicationTypeList, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE);

                    //if (ProgrammeList != null && ProgrammeList.Count > 0)
                    //    objViewModel.ProgrammeList = new SelectList(ProgrammeList, Common.adm_apptype_prog_groups.PROGRAMME_GROUP_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    if (AppSubmissionTypeList != null && AppSubmissionTypeList.Count > 0)
                        objViewModel.AppSubmissionTypeList = new SelectList(AppSubmissionTypeList, Common.SUP_APP_SUBMISSION_TYPE.APP_TYPE_ID, Common.SUP_APP_SUBMISSION_TYPE.APPLICATION_TYPE);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "AdmissionReport", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "AdmissionReport", ex.Message);
                }
            }

            return View(objViewModel);
        }
        public ActionResult BindIssuedApplication(string sProgrammeId, string sSubmissionTypeId, string sAppType)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUED_APPLICATION_RANKLIST objRanklist = new ADM_ISSUED_APPLICATION_RANKLIST();

            objRanklist.PROGRAMME_ID = sProgrammeId;
            objRanklist.ISSUED_ID = "1537";
            try
            {

                if (sAppType == Common.ApplicationType.UG)
                {
                    objViewModel.liIssuedApplication = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCoursesbyReceiveID)).DataSource.Data;

                    if (sSubmissionTypeId == "1" && sProgrammeId != "15" && sProgrammeId != "16")
                    {
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGPersonalInfoforRankList)).DataSource.Data;
                         objViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGMarksForRankList)).DataSource.Data;
                        objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "0";

                    }
                    else if (sSubmissionTypeId == "1" && (sProgrammeId == "15" || sProgrammeId == "16"))
                    {
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGEnglishPersonalInfoforRankList)).DataSource.Data;
                        objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "0";
                    }
                    else if (sSubmissionTypeId == "2" && sProgrammeId != "15" && sProgrammeId != "16")
                    {
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGLateApplication)).DataSource.Data;
                        objViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGLateApplicationMarksForRankList)).DataSource.Data;
                        objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "1";
                    }
                    else
                    {
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGEnglishLateApplication)).DataSource.Data;
                        objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "1";
                    }

                }

                else
                {
                    //// FOR PG 
                    objRanklist.ISSUED_ID = "1537";
                    if (sSubmissionTypeId == "1")
                    {
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchPGPersonalInfoforRankList)).DataSource.Data;
                        // objViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGMarksForRankList)).DataSource.Data;
                        objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "0";

                    }

                    else
                    {
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchPGPersonalInfoforSecondRankList)).DataSource.Data;
                        objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "1";
                    }
                }

                ViewBag.AppType = sAppType;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindIssuedApplication", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindIssuedApplication", ex.Message);
                }
            }

            return View(objViewModel);
        }
        public JsonResult BindProgrammeGroupByApptypeID(string sAppType)
        {
            string sOption = string.Empty;
            CP_PROGRAMME_GROUP objModel = new CP_PROGRAMME_GROUP();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objModel.APPTYPE_ID = sAppType;
                    var Programme = (List<ADM_APPTYPE_PROG_GROUPS>)CMS.DAO.CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeGroupByAppType)).DataSource.Data;
                    if (Programme != null && Programme.Count > 0)
                    {
                        foreach (var item in Programme)
                        {
                            sOption += "<option value='" + item.PROGRAMME_GROUP_ID + "' >" + item.PROGRAMME_NAME + "</option>";
                        }
                    }
                }
                else
                    return Json(sOption);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindProgrammeGroupByShiftAndProgrammeMode", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindProgrammeGroupByShiftAndProgrammeMode", "Fee", ex.Message);
                }
            }
            return Json(sOption);
        }

        // Date Wise Issued Report
        public ActionResult DateWiseAdmissionReport()
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            string sAcademicYear = string.Empty;
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var ApplicationTypeList = (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType), sAcademicYear).DataSource.Data;
                    if (ApplicationTypeList != null && ApplicationTypeList.Count > 0)
                        objViewModel.ApplicationTypeList = new SelectList(ApplicationTypeList, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE); }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "DateWiseAdmissionReport", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "DateWiseAdmissionReport", ex.Message);
                }
            }

            return View(objViewModel);
        }
        public JsonResult FetchStudentListByDate(string ToDate, string FromDate, string ProgrammeList)
        {
            string sOption = string.Empty;
            ADM_ISSUED_APPLICATIONS objModel = new ADM_ISSUED_APPLICATIONS();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objModel.FROM_DATE = CommonMethods.ConvertdatetoyearFromat(FromDate);
                    objModel.TO_DATE = CommonMethods.ConvertdatetoyearFromat(ToDate);
                    objModel.PROGRAMME_GROUP_ID = ProgrammeList;
                    var StudentList = (List<ADM_ISSUED_APPLICATIONS>)CMS.DAO.CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.IssuedStudentlistByDate)).DataSource.Data;
                    if (StudentList != null && StudentList.Count > 0)
                    {
                        foreach (var item in StudentList)
                        {
                            sOption += "<option value='" + item.RECEIVE_ID + "' >" + item.FIRST_NAME + "</option>";
                        }
                    }
                }
                else
                    return Json(sOption);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindProgrammeGroupByShiftAndProgrammeMode", "Fee", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindProgrammeGroupByShiftAndProgrammeMode", "Fee", ex.Message);
                }
            }
            return Json(sOption);
        }
        public ActionResult BindDateWiseIssuedApplication(string sProgrammeId, string LiStudent, string sAppType)
        {
            AdmissionViewModel objViewModel = new AdmissionViewModel();
            ADM_ISSUED_APPLICATION_RANKLIST objRanklist = new ADM_ISSUED_APPLICATION_RANKLIST();

            objRanklist.PROGRAMME_ID = sProgrammeId;
            objRanklist.RECEIVE_ID = LiStudent;
            objRanklist.ISSUED_ID = "1537";
            var sSubmissionTypeId = "1";
            try
            {

                if (sAppType == Common.ApplicationType.UG)
                {
                    objViewModel.liIssuedApplication = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchlistByReceiveid)).DataSource.Data;

                    if (sSubmissionTypeId == "1" && sProgrammeId != "15" && sProgrammeId != "16")
                    {
                        var Sql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGPersonalInfoforRankListByReceiveId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, LiStudent);
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, Sql).DataSource.Data;
                        var sql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGMarksForRankListByReceiveId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, LiStudent);
                         objViewModel.liADMStudentMarks = (List<ADM_STU_SUBMARKS>)CMSHandler.FetchData<ADM_STU_SUBMARKS>(objRanklist, sql).DataSource.Data;
                        objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "0";

                    }
                    if (sSubmissionTypeId == "1" && (sProgrammeId == "15" || sProgrammeId == "16"))
                    {
                        var sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchUGEnglishPersonalInfoforRankListByReceiveId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, LiStudent);
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, sSQL).DataSource.Data;
                        objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "0";
                    }

                }

                else
                {
                    //// FOR PG 
                    objRanklist.ISSUED_ID = "1537";
                    if (sSubmissionTypeId == "1")
                    {
                        var Ssql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchPGPersonalInfoforRankListByReceiveId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, LiStudent);
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, Ssql).DataSource.Data;
                         objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "0";
                    }
                    else
                    {
                        var SSql = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchPGPersonalInfoforSecondRankListByReceiveId).Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.RECEIVE_ID, LiStudent);
                        objViewModel.liIssuedApplicationRank = (List<ADM_ISSUED_APPLICATION_RANKLIST>)CMSHandler.FetchData<ADM_ISSUED_APPLICATION_RANKLIST>(objRanklist, SSql).DataSource.Data;
                        objViewModel.liIssuedApplicationRank.FirstOrDefault().IS_LATE_APPLICATION = "1";
                    }
                }

                ViewBag.AppType = sAppType;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AdmissionController", "BindIssuedApplication", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AdmissionController", "BindIssuedApplication", ex.Message);
                }
            }

            return View(objViewModel);
        }
        #endregion
    }
}