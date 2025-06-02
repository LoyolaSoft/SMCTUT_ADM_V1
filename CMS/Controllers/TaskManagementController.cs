using CMS.DAO;
using CMS.SQL.Examination;
using CMS.SQL.TaskManagement;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class TaskManagementController : Controller
    {
        // GET: TaskManagement
        public ActionResult Index()
        {
            return View();
        }
        #region Assignment
        public ActionResult Assignment()
        {
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            TaskManagementViewModel objViewModel = new TaskManagementViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objStaffModel.STAFF_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : "";
                    var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objStaffModel, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchClassList), sAcademicYear).DataSource.Data;
                    if (FetchClass != null && FetchClass.Count > 0)
                        objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Assignment", "TaskManagement", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Assignment", "TaskManagement", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult ListAssignment()
        {
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            TaskManagementViewModel objViewModel = new TaskManagementViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objStaffModel, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchClassList), sAcademicYear).DataSource.Data;
                    List<cp_Course_Root_2017> listCourse = new List<cp_Course_Root_2017>();
                    listCourse.Add(new cp_Course_Root_2017() { COURSE_ROOT_ID = "0", COURSE_TITLE = "---Select---" });
                    objViewModel.CourseList = new SelectList(listCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                    if (FetchClass != null && FetchClass.Count > 0)
                        objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "ListAssignment", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "ListAssignment", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult GetCourseListByClassIdAndStaffId(string sClassId)
        {
            JsonResultData objResult = new JsonResultData();
            CourseWiseStaffList objStaffModel = new CourseWiseStaffList();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sOption = string.Empty;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objStaffModel.CLASS_ID = sClassId;
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var CourseList = (List<stf_Personal_Info>)CMSHandler.FetchData<stf_Personal_Info>(objStaffModel, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseList), sAcademicYear).DataSource.Data;
                    if (CourseList != null && CourseList.Count > 0)
                    {
                        sOption = "<option value='0'>--Select--</option>";
                        foreach (var item in CourseList)
                        {
                            sOption += "<option value='" + item.COURSE_ID + "'>" + item.COURSE_TITLE + "</option>";
                        }
                        objResult.sResult = sOption;
                    }
                }
                else
                {
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "GetCourseListByClassIdAndStaffId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "GetCourseListByClassIdAndStaffId", ex.Message);
                }

            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindAssignmentListByCourseId(string sCourseId, string sClassId)
        {
            TaskManagementViewModel objViewModel = new TaskManagementViewModel();
            AssignmentModel objModel = new AssignmentModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    objModel.COURSE_ID = sCourseId;
                    objModel.CLASS_ID = sClassId;
                    var FetchClass = (List<AssignmentModel>)CMSHandler.FetchData<AssignmentModel>(objModel, TaskManagementSQL.GetTaskManagementSQL(TaskManagementSQLCommands.FetchAssignmentList), sAcademicYear).DataSource.Data;
                    if (FetchClass != null && FetchClass.Count > 0)
                    {
                        foreach (var item in FetchClass)
                        {
                            byte[] bytes = Convert.FromBase64String(item.DESCRIPTION);
                            item.DESCRIPTION = Encoding.UTF8.GetString(bytes);
                        }
                        objViewModel.liTaskManagementModel = FetchClass;
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "BindAssignmentListByCourseId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "BindAssignmentListByCourseId", ex.Message);
                }

            }
            return View(objViewModel);
        }
        public JsonResult SaveAssignment(string Assignment)
        {
            JsonResultData objResult = new JsonResultData();
            TaskManagementViewModel objViewModel = new TaskManagementViewModel();
            AssignmentModel objModel = new AssignmentModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    if (Assignment != null)
                    {
                        var Result = JsonConvert.DeserializeObject<AssignmentModel>(Assignment);
                        Result.DATE_FROM = DateTime.Today.ToString("yyyy-MM-dd");
                        Result.SUBMISSION_DATE = CommonMethods.ConvertdatetoyearFromat(Result.SUBMISSION_DATE).ToString();
                        Result.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                        var SaveAssignment = CMSHandler.SaveOrUpdate(Result, TaskManagementSQL.GetTaskManagementSQL((!string.IsNullOrEmpty(Result.ASSIGNMENT_ID)) ? TaskManagementSQLCommands.UpdateAssignment : TaskManagementSQLCommands.SaveAssignment), sAcademicYear);
                        if (SaveAssignment.Success)
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        else
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.BadRequest;
                        objResult.Message = Common.ErrorMessage.BadRequest;
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "SaveAssignment", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "SaveAssignment", ex.Message);
                }

            }
            return Json(objResult);
        }
        public ActionResult EditAssignment(string id)
        {
            CourseWiseStaffList objStaffModel = new CourseWiseStaffList();
            AssignmentModel objAssignmentModel = new AssignmentModel();
            TaskManagementViewModel objViewModel = new TaskManagementViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objAssignmentModel.ASSIGNMENT_ID = id;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objStaffModel.STAFF_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : "";
                    var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objStaffModel, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchClassList), sAcademicYear).DataSource.Data;
                    if (FetchClass != null && FetchClass.Count > 0)
                        objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                    objViewModel.liTaskManagementModel = (List<AssignmentModel>)CMSHandler.FetchData<AssignmentModel>(objAssignmentModel, TaskManagementSQL.GetTaskManagementSQL(TaskManagementSQLCommands.FetchAssignmentById), sAcademicYear).DataSource.Data;
                    if (objViewModel.liTaskManagementModel != null && objViewModel.liTaskManagementModel.Count > 0)
                    {
                        objViewModel.Assignment.ASSIGNMENT_ID = objViewModel.liTaskManagementModel.FirstOrDefault().ASSIGNMENT_ID;
                        objViewModel.ClassId = objViewModel.liTaskManagementModel.FirstOrDefault().CLASS_ID;
                        objViewModel.CourseId = objViewModel.liTaskManagementModel.FirstOrDefault().COURSE_ID;
                        objViewModel.Assignment.ASSIGNMENT_TITLE = objViewModel.liTaskManagementModel.FirstOrDefault().ASSIGNMENT_TITLE;
                        objViewModel.Assignment.SUBMISSION_DATE = objViewModel.liTaskManagementModel.FirstOrDefault().SUBMISSION_DATE;
                        objViewModel.Assignment.IS_ACTIVE = objViewModel.liTaskManagementModel.FirstOrDefault().IS_ACTIVE;
                        objViewModel.Assignment.IS_FILE_UPLOAD = objViewModel.liTaskManagementModel.FirstOrDefault().IS_FILE_UPLOAD;
                        objStaffModel.CLASS_ID = objViewModel.ClassId;
                        var CourseList = (List<stf_Personal_Info>)CMSHandler.FetchData<stf_Personal_Info>(objStaffModel, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseList), sAcademicYear).DataSource.Data;
                        if (CourseList != null && CourseList.Count > 0)
                            objViewModel.CourseList = new SelectList(CourseList, Common.CP_CLASS_COURSE_2017.COURSE_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                        objViewModel.Assignment.DESCRIPTION = objViewModel.liTaskManagementModel.FirstOrDefault().DESCRIPTION;
                        byte[] bytes = Convert.FromBase64String(objViewModel.Assignment.DESCRIPTION);
                        objViewModel.HiddenId = Encoding.UTF8.GetString(bytes);
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Assignment", "TaskManagement", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Assignment", "TaskManagement", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult DeleteAssignment(string sAssignmentId)
        {
            JsonResultData objResult = new JsonResultData();
            AssignmentModel objModel = new AssignmentModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    objModel.ASSIGNMENT_ID = sAssignmentId;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var DeleteAssignment = CMSHandler.SaveOrUpdate(objModel, TaskManagementSQL.GetTaskManagementSQL(TaskManagementSQLCommands.DeleteAssignment), sAcademicYear);
                    if (DeleteAssignment.Success)
                        objResult.Message = Common.Messages.RecordDeletedSuccessfully;
                    else
                        objResult.Message = Common.Messages.FailedToDeletedTryAgain;
                }
                else
                {
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "DeleteAssignment", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "DeleteAssignment", ex.Message);
                }

            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssignmentSubmittedList()
        {
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            TaskManagementViewModel objViewModel = new TaskManagementViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objStaffModel, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchClassList), sAcademicYear).DataSource.Data;
                    List<cp_Course_Root_2017> listCourse = new List<cp_Course_Root_2017>();
                    List<AssignmentModel> listAssignment = new List<AssignmentModel>();
                    listCourse.Add(new cp_Course_Root_2017() { COURSE_ROOT_ID = "0", COURSE_TITLE = "---Select---" });
                    listAssignment.Add(new AssignmentModel() { ASSIGNMENT_ID = "0", ASSIGNMENT_TITLE = "---Select---" });
                    objViewModel.CourseList = new SelectList(listCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                    if (FetchClass != null && FetchClass.Count > 0)
                        objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "AssignmentSubmittedList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "AssignmentSubmittedList", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult GetAssignmentListByCourseIdIdAndStaffId(string sClassId, string sCourseId)
        {
            JsonResultData objResult = new JsonResultData();
            CourseWiseStaffList objStaffModel = new CourseWiseStaffList();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sOption = string.Empty;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objStaffModel.CLASS_ID = sClassId;
                    objStaffModel.COURSE_ID = sCourseId;
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    var AssignmentList = (List<AssignmentModel>)CMSHandler.FetchData<AssignmentModel>(objStaffModel, TaskManagementSQL.GetTaskManagementSQL(TaskManagementSQLCommands.FetchAssignmentByClassIdAndCourseIdAndStaffId), sAcademicYear).DataSource.Data;
                    if (AssignmentList != null && AssignmentList.Count > 0)
                    {
                        sOption = "<option value='0'>--Select--</option>";
                        foreach (var item in AssignmentList)
                        {
                            sOption += "<option value='" + item.ASSIGNMENT_ID + "'>" + item.ASSIGNMENT_TITLE + "</option>";
                        }
                        objResult.sResult = sOption;
                    }
                }
                else
                {
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "GetAssignmentListByCourseIdIdAndStaffId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "GetAssignmentListByCourseIdIdAndStaffId", ex.Message);
                }

            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindAssignmentSubmittedListByAssignmentId(string sCourseId, string sClassId, string sAssignmentId)
        {
            TaskManagementViewModel objViewModel = new TaskManagementViewModel();
            AssignmentModel objModel = new AssignmentModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    objModel.COURSE_ID = sCourseId;
                    objModel.ASSIGNMENT_ID = sAssignmentId;
                    objModel.CLASS_ID = sClassId;
                    var FetchClass = (List<AssignmentModel>)CMSHandler.FetchData<AssignmentModel>(objModel, TaskManagementSQL.GetTaskManagementSQL(TaskManagementSQLCommands.FetchAssignmentSubmittedList), sAcademicYear).DataSource.Data;
                    if (FetchClass != null && FetchClass.Count > 0)
                    {
                        foreach (var item in FetchClass)
                        {
                            byte[] bytes = Convert.FromBase64String(item.DESCRIPTION);
                            item.DESCRIPTION = Encoding.UTF8.GetString(bytes);
                        }
                        objViewModel.liTaskManagementModel = FetchClass;
                    }
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "BindAssignmentSubmittedListByAssignmentId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "BindAssignmentSubmittedListByAssignmentId", ex.Message);
                }

            }
            return View(objViewModel);
        }
        #endregion
    }
}