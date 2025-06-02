using CMS.DAO;
using CMS.Models;
using CMS.SQL.Dashboard;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS.Controllers
{

    public class DashBoardController : Controller
    {
        JsonResultData ObjsonData = new JsonResultData();
        ResultArgs Resultargs = new ResultArgs();
        // GET: DashBoard
        [UserRights("SUPER ADMIN")]
        public ActionResult SuperAdmin()
        {
            return View();
        }
        [UserRights("ADMIN")]
        public ActionResult Admin()
        {
            return View();
        }
        [UserRights("PRINCIPAL")]
        public ActionResult Principal()
        {
            DashboardViewModel objDashboard = new DashboardViewModel();
            ResultArgs resultReceived = new ResultArgs();
            ResultArgs resultIssued = new ResultArgs();
            ResultArgs resultSelected = new ResultArgs();
            ResultArgs resultVerified = new ResultArgs();
            ResultArgs resultAdmitted = new ResultArgs();

            resultReceived = objDashboard.GetOverallReceivedApplicationCount();
            ViewBag.ReceivedApplicationTotal = (string.IsNullOrEmpty(resultReceived.StringResult) ? 0 : Convert.ToDouble(resultReceived.StringResult));
            resultIssued = objDashboard.GetOverallIssuedApplicationCount();
            ViewBag.TotalIssuedApplication = (string.IsNullOrEmpty(resultIssued.StringResult) ? 0 : Convert.ToDouble(resultIssued.StringResult));
            resultSelected = objDashboard.GetOverallSelectedApplicationCount();
            ViewBag.TotalSelectedApplication = (string.IsNullOrEmpty(resultSelected.StringResult) ? 0 : Convert.ToDouble(resultSelected.StringResult));
            resultVerified = objDashboard.GetOverallVerifiedApplicationCount();
            ViewBag.TotalVerifiedApplication = (string.IsNullOrEmpty(resultVerified.StringResult) ? 0 : Convert.ToDouble(resultVerified.StringResult));
            resultAdmitted = objDashboard.GetOverallAdmittedApplicationCount();
            ViewBag.TotalAdmitedApplication = (string.IsNullOrEmpty(resultAdmitted.StringResult) ? 0 : Convert.ToDouble(resultAdmitted.StringResult));







            return View(objDashboard);
            // return View();
        }
        [UserRights("STAFF")]
        public ActionResult Staff()
        {
            try
            {
                ResultArgs resultArgs = new ResultArgs();
                DataValue dv = new DataValue();
                string sUserId = string.Empty;
                string sAcademicYear = string.Empty;
                using (DashboardViewModel objDashboard = new DashboardViewModel())
                {
                    // need to use this block for dashbord tasks

                    if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                    {
                        sUserId = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                        //sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();   academic year need to handle paul
                        //   sAcademicYear = "2017";
                        Session[Common.SESSION_VARIABLES.SEMESTER_ID] = "1,3,5";
                        dv.Add(Common.STUDENT_INFO.STUDENT_ID, sUserId, EnumCommand.DataType.String);
                        //  resultArgs = objDashboard.FetchStaffInfoById(dv);
                        if (resultArgs.Success && resultArgs.DataSource.Table != null && resultArgs.DataSource.Table.Rows.Count > 0)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("DashBoardController", "Staff", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AccountController", "Staff", ex.Message);
                }

            }

            return View();
        }
        [UserRights("STUDENT")]
        public ActionResult Student()
        {
            DashboardViewModel objViewModel = new DashboardViewModel();
            try
            {
                ResultArgs resultArgs = new ResultArgs();
                DataValue dv = new DataValue();
                string sUserId = string.Empty;
                string sAcademicYear = string.Empty;
                string sDayOrder = string.Empty;
                AssignmentModel objAssignmentModel = new AssignmentModel();
                using (DashboardViewModel objDashboard = new DashboardViewModel())
                {
                    if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                    {
                        sUserId = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                        sDayOrder = Session[Common.SESSION_VARIABLES.DAY_ORDER].ToString();
                        sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        //sAcademicYear = "2017";
                        dv.Add(Common.STUDENT_INFO.ACADEMIC_YEAR, sAcademicYear, EnumCommand.DataType.String);
                        dv.Add(Common.STUDENT_INFO.STUDENT_ID, sUserId, EnumCommand.DataType.String);
                        resultArgs = objDashboard.FetchStudentInfoById(dv);
                        if (resultArgs.Success)
                        {
                            if (resultArgs.DataSource.Table != null && resultArgs.DataSource.Table.Rows.Count > 0)
                            {
                                Session[Common.SESSION_VARIABLES.USER_INFO] = resultArgs.DataSource.Table;
                                Session[Common.SESSION_VARIABLES.CLASS_ID] = resultArgs.DataSource.Table.Rows[0][Common.USER_INFO.CLASS_ID];
                                Session[Common.SESSION_VARIABLES.DEPARTMENT_ID] = resultArgs.DataSource.Table.Rows[0][Common.USER_INFO.DEPT_ID];
                                Session[Common.SESSION_VARIABLES.SHIFT_ID] = resultArgs.DataSource.Table.Rows[0][Common.USER_INFO.SHIFT_ID];
                                Session[Common.SESSION_VARIABLES.CLASS_NAME] = resultArgs.DataSource.Table.Rows[0][Common.USER_INFO.CLASS_NAME];
                                Session[Common.SESSION_VARIABLES.PHOTO] = resultArgs.DataSource.Table.Rows[0][Common.USER_INFO.PHOTO];
                            }
                        }
                        objAssignmentModel.CLASS_ID = Session[Common.SESSION_VARIABLES.CLASS_ID].ToString();
                        objAssignmentModel.STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                        objViewModel.liAssignment = (List<AssignmentModel>)CMSHandler.FetchData<AssignmentModel>(objAssignmentModel, DashboardSQL.GetDashboardSQL(DashboardSQLCommands.FetchAssignmentInfo), sAcademicYear).DataSource.Data;
                        if (objViewModel.liAssignment != null && objViewModel.liAssignment.Count > 0)
                        {
                            foreach (var item in objViewModel.liAssignment)
                            {
                                byte[] bytes = Convert.FromBase64String(item.DESCRIPTION);
                                item.DESCRIPTION = Encoding.UTF8.GetString(bytes);
                            }
                        }
                        objViewModel.liStaff = (List<stf_Personal_Info>)CMSHandler.FetchData<stf_Personal_Info>(objAssignmentModel, DashboardSQL.GetDashboardSQL(DashboardSQLCommands.FetchStaffInfoByStudentId), sAcademicYear).DataSource.Data;
                    }
                    else
                        return RedirectToAction("ErrorMessage", "error");
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("DashBoardController", "Student", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AccountController", "Student", ex.Message);
                }
            }

            return View(objViewModel);
        }
        [UserRights("OFFICEASSISTANT")]
        public ActionResult OfficeAssistant()
        {
            return View();
        }
        [UserRights("EXAMINCHARGE")]

        public ActionResult EXAMINCHARGE()
        {
            return View();
        }
        [UserRights("ASSISTANTEXAMINCHARGE")]

        public ActionResult ASSISTANTEXAMINCHARGE()
        {
            return View();
        }
        public async Task<ActionResult> APPLICANT()
        {

            AdmissionViewModel admissionViewModel = new AdmissionViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    Session[Common.SESSION_VARIABLES.RECEIVE_ID] = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    //Session[Common.SESSION_VARIABLES.ISSUE_ID] = Session[Common.SESSION_VARIABLES.APPLICATION_TYPE_ID].ToString();
                    //objViewModel.liIssueApplication= (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(new ADM_ISSUE_APPLICATION_2018() { ISSUE_ID = Session[Common.SESSION_VARIABLES.ISSUE_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAdmissionIssueApplicationById),"2018").DataSource.Data;
                    // Session[Common.SESSION_VARIABLES.APPLICATION_NO] = objViewModel.liIssueApplication.FirstOrDefault().APPLICATION_NO;
                    var issuedApplication = await Task.Run(() => (List<ADM_ISSUED_APPLICATIONS>)CMSHandler.FetchData<ADM_ISSUED_APPLICATIONS>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchAppliedApplicationInfo), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data);
                    var HostelStatus = await Task.Run(() => (List<ADM_RECEIVE_APPLICATION>)CMSHandler.FetchData<ADM_RECEIVE_APPLICATION>(new ADM_ISSUED_APPLICATIONS() { RECEIVE_ID = Session[Common.SESSION_VARIABLES.RECEIVE_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchHostelStatusForDashboard), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data);
                    var applicationType = await Task.Run(() => (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(new ADM_RECEIVE_APPLICATION { RECEIVE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString() }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.GetApplicationTypeById), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data);
                    if (applicationType != null && applicationType.Count > 0)
                    {
                        Session[Common.SESSION_VARIABLES.APPLICATION_TYPE_ID] = applicationType.FirstOrDefault().APPLICATION_TYPE_ID;

                        admissionViewModel.liCourses = await Task.Run(() => (List<ADM_APPTYPE_PROG_GROUPS>)CMSHandler.FetchData<ADM_APPTYPE_PROG_GROUPS>(new ADM_APPTYPE_PROG_GROUPS { APPLICATION_TYPE_ID = applicationType.FirstOrDefault().APPLICATION_TYPE_ID, GENDER = applicationType.FirstOrDefault().GENDER }, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchCoursesByApplicationTypeId)).DataSource.Data);
                    }
                    if (issuedApplication != null && issuedApplication.Count > 0)
                    {
                        admissionViewModel.liIssuedApplictionsInfo = issuedApplication;
                    }
                    if (HostelStatus != null && HostelStatus.Count > 0)
                    {
                        admissionViewModel.liApplicant = HostelStatus;
                    }
                    admissionViewModel.liHostel = (List<SUP_HOSTEL>)CMSHandler.FetchData<SUP_HOSTEL>(new SUP_HOSTEL() { RECEIVE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString() }, DashboardSQL.GetDashboardSQL(DashboardSQLCommands.FetchHostelApplied)).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("DashBoardController", "APPLICANT", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AccountController", "APPLICANT", ex.Message);
                }
            }
            return View(admissionViewModel);
        }

        public ActionResult DashboardIndex()
        {
            return View();
        }
        public ActionResult ADMISSIONADMIN()
        {

            return View();
        }
        public ActionResult MESSADMIN()
        {

            return View();
        }
        public ActionResult BANK()
        {
            return View();
        }
        public ActionResult AppDownloaded()
        {
            return View();
        }
        public JsonResult SaveHostelFees(string sValue)
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                SUP_HOSTEL objModel = new SUP_HOSTEL();
                try
                {

                    objModel.RECEIVE_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    //objModel.ROLE_ID = Session[Common.SESSION_VARIABLES.USER_ROLE_ID].ToString();
                    objModel.ACADEMIC_YEAR = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.HOSTEL_FACILITY = sValue;
                    //  var Hostelapplicationfees = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new sup_frequency_type() { FREQUENCY_TYPE_ID = Common.FrequencyType.HostelApplication }, DashboardSQL.GetDashboardSQL(DashboardSQLCommands.FeeHostelApplicationfee)).DataSource.Data;

                    Resultargs = CMSHandler.SaveOrUpdate(objModel, DashboardSQL.GetDashboardSQL(DashboardSQLCommands.Savehostelinfo));
                    if (Resultargs.Success)
                    {
                        //if (objModel.HOSTEL_FACILITY=="1")
                        //{
                        //    ////fEES iNSERT
                        //    //if (Hostelapplicationfees != null)
                        //    //{
                        //    //    foreach (var item in Hostelapplicationfees)
                        //    //    {
                        //    //        item.STUDENT_ID = objModel.RECEIVE_ID;
                        //    //        Resultargs = CMSHandler.SaveOrUpdate(item, DashboardSQL.GetDashboardSQL(DashboardSQLCommands.SaveHostelApplicationfee));
                        //    //    }
                        //    //}

                        //    ObjsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                        //}
                        //else
                        //{
                        //    ObjsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                        //}
                        ObjsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                        ObjsonData.Message = Common.Messages.FailedToSaveRecords;
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("DashBoardController", "SaveHostelFees", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("DashBoardController", "SaveHostelFees", ex.Message);
                    }

                }
            }
            else
            {
                ObjsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjsonData, JsonRequestBehavior.AllowGet);
        }
    }
}