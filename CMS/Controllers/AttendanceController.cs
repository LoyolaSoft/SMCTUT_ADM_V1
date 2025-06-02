using CMS.DAO;
using CMS.Models;
using CMS.SQL.SupportData;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CMS.Controllers
{
    public class AttendanceController : Controller
    {
        string sSQL = string.Empty;
        // GET: Attendance
        //    [UserRights("STAFF")]
        [UserRights("STAFF")]
        public ActionResult Index()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    stf_Personal_Info staff = new stf_Personal_Info();
                    AttendanceViewModel objAttendanceViewModel = new AttendanceViewModel();
                    staff.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    sSQL = SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassesByStaffIdAndActiveSemester).Replace(Common.Delimiter.QUS + Common.CP_COURSE_ROOT_2017.SEMESTER_ID, (Session[Common.SESSION_VARIABLES.ACTIVE_SEMESTER_FOR_STAFF] != null) ? Session[Common.SESSION_VARIABLES.ACTIVE_SEMESTER_FOR_STAFF].ToString() : string.Empty);
                    objAttendanceViewModel.Classes = (List<ClassModel>)CMSHandler.FetchData<ClassModel>(staff, sSQL, sAcademicYear).DataSource.Data;
                    return View(objAttendanceViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("AttendanceController", "Index", "Err MSg: " + ex.Message, "Query is empty!");
                    }
                    return RedirectToAction("ErrorMessage", "Error");
                }

            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        public ActionResult OverAllAttendance()
        {
            string sAcademicYear = string.Empty;

            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                AttendanceViewModel attendanceViewModel = new AttendanceViewModel();
                attendanceViewModel.staffList = (List<STAFF_PERSONAL>)CMSHandler.FetchData<STAFF_PERSONAL>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStaff)).DataSource.Data;
                attendanceViewModel.hourList = (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHours)).DataSource.Data;
                attendanceViewModel.SplAttendanceType = (List<SUP_SPL_ATTENDANCE_TYPE>)CMSHandler.FetchData<SUP_SPL_ATTENDANCE_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupSplAttendanceType)).DataSource.Data;
                attendanceViewModel.Classes = (List<ClassModel>)CMSHandler.FetchData<ClassModel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOnlyRegularClasses), sAcademicYear).DataSource.Data;
                return View(attendanceViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }


        }

        public ActionResult LoadStudentsForOverAll(string objInput)
        {
            var obj = new JavaScriptSerializer().Deserialize<OVER_ALL_ATTENDANCE>(objInput);
            AttendanceViewModel objAttendanceViewModel = new AttendanceViewModel();
            TT_TIMETABLE_2017 timeTable = new TT_TIMETABLE_2017();
            CALENDER_2017 Calender = new CALENDER_2017();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    timeTable.STAFF_ID = obj.StaffId;
                    timeTable.CLASS_ID = obj.ClassId;
                    timeTable.HOUR_ID = obj.HourId;
                    timeTable.ATTENDANCE_DATE = CommonMethods.ConvertdatetoyearFromat(obj.AttendanceDate);
                    if (obj.Flag.Equals("1"))
                    {
                        objAttendanceViewModel.attendanceEntry = (List<ATTENDANCE_ENTRY_MODEL>)CMSHandler.FetchData<ATTENDANCE_ENTRY_MODEL>(timeTable, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchOptionalCourseStudentListForOverAllAttendance), sAcademicYear).DataSource.Data;
                        if (objAttendanceViewModel.attendanceEntry == null)
                            objAttendanceViewModel.attendanceEntry = (List<ATTENDANCE_ENTRY_MODEL>)CMSHandler.FetchData<ATTENDANCE_ENTRY_MODEL>(timeTable, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchStudentListForOverAllAttendance), sAcademicYear).DataSource.Data;
                        if (objAttendanceViewModel.attendanceEntry == null)
                            objAttendanceViewModel.attendanceEntry = (List<ATTENDANCE_ENTRY_MODEL>)CMSHandler.FetchData<ATTENDANCE_ENTRY_MODEL>(timeTable, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchStudentListForAttendanceByCourseIdAndStaffIdForSpecialAttendance), sAcademicYear).DataSource.Data;
                    }
                    else
                    {
                        Calender.DAY_ORDER_DATE = timeTable.ATTENDANCE_DATE;
                        var FetchDayOrder = (List<CALENDER_2017>)CMSHandler.FetchData<CALENDER_2017>(Calender, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchdDayOrderByAttendanceDate), sAcademicYear).DataSource.Data;
                        if (FetchDayOrder != null && FetchDayOrder.Count > 0)
                        {
                            objAttendanceViewModel.attendanceEntry = (List<ATTENDANCE_ENTRY_MODEL>)CMSHandler.FetchData<ATTENDANCE_ENTRY_MODEL>(timeTable, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchStudentListForAttendanceByClassIdForSpecialAttendance), sAcademicYear).DataSource.Data;
                        }
                        else
                            objAttendanceViewModel.attendanceEntry = (List<ATTENDANCE_ENTRY_MODEL>)CMSHandler.FetchData<ATTENDANCE_ENTRY_MODEL>(timeTable, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchStudentListForAttendanceByClassId), sAcademicYear).DataSource.Data;
                    }
                    objAttendanceViewModel.attendanceTypes = (List<SUP_ATTENDANCE_TYPE>)CMSHandler.FetchData<SUP_ATTENDANCE_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAttendanceTypes)).DataSource.Data;
                    objAttendanceViewModel.reasons = (List<SUP_REASON>)CMSHandler.FetchData<SUP_REASON>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchReasonForAttendance)).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Attendance", "LoadStudentsForOverAll", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Attendance", "LoadStudentsForOverAll", Ex.Message);
                }

            }
            return PartialView("LoadStudentsForOverAll", objAttendanceViewModel);
        }
        public JsonResult FetchDayOrderByDate(string sAttendanceDate)
        {
            JsonResultData objResult = new JsonResultData();
            CALENDER_2017 Calender = new CALENDER_2017();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                Calender.DAY_ORDER_DATE = CommonMethods.ConvertdatetoyearFromat(sAttendanceDate);
                var FetchDayOrder = (List<CALENDER_2017>)CMSHandler.FetchData<CALENDER_2017>(Calender, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchdDayOrderByAttendanceDate), sAcademicYear).DataSource.Data;
                if (FetchDayOrder != null && FetchDayOrder.Count > 0)
                    objResult.sResult = FetchDayOrder.FirstOrDefault().DAY_ORDER;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        [UserRights("STAFF")]
        public JsonResult AttendanceEntry(string sJsonString)
        {
            JsonResultData objResult = new JsonResultData();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            TimeTableSetting TimeTable = new TimeTableSetting();
            int SuccessCount = 0;
            int FailedCount = 0;
            if (!string.IsNullOrEmpty(sJsonString))
            {
                var absentees = serializer.Deserialize<ABSENTEE_LIST>(sJsonString);
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                foreach (var item in absentees.ATT_ABSENTEE_LIST)
                {
                    item.ATTENDENCE_DATE = CommonMethods.ConvertdatetoyearFromat(item.ATTENDENCE_DATE);
                    //if (!string.IsNullOrEmpty(item.COURSE_ID))
                    //{
                    //    if (!string.IsNullOrEmpty(item.HOUR_FROM))
                    //    {
                    //        item.CLASS_ID = item.S_CLASS_ID;
                    //        item.ENTRY_ID = (Session[Common.SESSION_VARIABLES.USER_ID].ToString());
                    //        if (CMSHandler.SaveOrUpdate(item, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(string.IsNullOrEmpty(item.ATTENDANCE_ID) ? AttendanceSQLCommands.SaveAbsenteeEntryForSpecialAttendance : AttendanceSQLCommands.UpdateAbsenteeEntryForSpecialAttendance), sAcademicYear).Success)
                    //        {
                    //            SuccessCount++;
                    //            objResult.ErrorCode = Common.ErrorCode.Ok;
                    //            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                    //        }
                    //        else
                    //        {
                    //            FailedCount++;
                    //            objResult.ErrorCode = Common.ErrorCode.BadRequest;
                    //            objResult.Message = Common.Messages.FailedToSaveRecords;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var FetchNoOfHoursByClassId = (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(item, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchNoOfHours), sAcademicYear).DataSource.Data;
                    //        if (FetchNoOfHoursByClassId != null && FetchNoOfHoursByClassId.Count > 0)
                    //        {
                    //            var TotalDays = Convert.ToInt32(FetchNoOfHoursByClassId.FirstOrDefault().NO_OF_DAYS);
                    //            item.ATTENDENCE_DATE = CommonMethods.ConvertdatetoyearFromat(item.ATTENDENCE_DATE);
                    //            for (int i = 1; i <= TotalDays; i++)
                    //            {
                    //                item.HOUR_FROM = Convert.ToString(i);
                    //                item.HOUR_TO = Convert.ToString(i);
                    //                item.ENTRY_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    //                item.CLASS_ID = item.S_CLASS_ID;
                    //                var IsAbsenteeEntryExisting = (List<ATT_ABSENTEE_LIST_2017>)CMSHandler.FetchData<ATT_ABSENTEE_LIST_2017>(item, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.IsAbsenteeEntryExisting), sAcademicYear).DataSource.Data;
                    //                if (IsAbsenteeEntryExisting != null && IsAbsenteeEntryExisting.Count > 0)
                    //                    item.ATTENDANCE_ID = IsAbsenteeEntryExisting.FirstOrDefault().ATTENDANCE_ID;
                    //                if (CMSHandler.SaveOrUpdate(item, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(string.IsNullOrEmpty(item.ATTENDANCE_ID) ? AttendanceSQLCommands.SaveAbsenteeEntryForSpecialAttendance : AttendanceSQLCommands.UpdateAbsenteeEntryForSpecialAttendance), sAcademicYear).Success)
                    //                {
                    //                    SuccessCount++;
                    //                    objResult.ErrorCode = Common.ErrorMessage.Ok;
                    //                    objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                    //                }
                    //                else
                    //                {
                    //                    FailedCount++;
                    //                    objResult.ErrorCode = Common.ErrorCode.BadRequest;
                    //                    objResult.Message = Common.Messages.FailedToSaveRecords;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //item.ATTENDENCE_DATE = CommonMethods.ConvertdatetoyearFromat(item.ATTENDENCE_DATE);
                    if (CMSHandler.SaveOrUpdate(item, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(string.IsNullOrEmpty(item.ATTENDANCE_ID) ? AttendanceSQLCommands.SaveAbsenteeEntryByCourseStaff : AttendanceSQLCommands.UpdateAbsenteeEntryByCourseStaff), sAcademicYear).Success)
                    {
                        SuccessCount++;
                        objResult.ErrorCode = Common.ErrorMessage.Ok;
                        objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        FailedCount++;
                        objResult.ErrorCode = Common.ErrorCode.BadRequest;
                        objResult.Message = Common.Messages.FailedToSaveRecords;
                    }
                    //}
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
            }

            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        [UserRights("STAFF")]
        public ActionResult AbsenteesList()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                var absenteesList = (List<ATTENDANCE_ENTRY_MODEL>)CMSHandler.FetchData<ATTENDANCE_ENTRY_MODEL>(new TT_TIMETABLE_2017() { STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString() }, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchAbsentListByStaffId), sAcademicYear).DataSource.Data;
                return View(absenteesList);
            }
            else
                return RedirectToAction("ErrorMessage", "Error");
        }

        public ActionResult LoadStudentsForAttendance(string sClassId, string sHourId, string sAttendanceDate)
        {
            JsonResultData objResult = new JsonResultData();
            if (!string.IsNullOrEmpty(sClassId) && !string.IsNullOrEmpty(sHourId))
            {
                AttendanceViewModel objAttendanceViewModel = new AttendanceViewModel();
                TT_TIMETABLE_2017 timeTable = new TT_TIMETABLE_2017();

                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                string sStafId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty; ;
                timeTable.STAFF_ID = sStafId;
                timeTable.CLASS_ID = sClassId;
                timeTable.HOUR_ID = sHourId;
                timeTable.ATTENDANCE_DATE = CommonMethods.ConvertdatetoyearFromat(sAttendanceDate);
                // FETCH STUDENTS FOR OPTIONAL COURSE
                objAttendanceViewModel.attendanceEntry = (List<ATTENDANCE_ENTRY_MODEL>)CMSHandler.FetchData<ATTENDANCE_ENTRY_MODEL>(timeTable, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchStudentListForAttendanceByOptionalCourseIdAndStaffId), sAcademicYear).DataSource.Data;
                if (objAttendanceViewModel.attendanceEntry == null)
                    // FETCH STUDENTS FOR REGULAR COURSE
                    objAttendanceViewModel.attendanceEntry = (List<ATTENDANCE_ENTRY_MODEL>)CMSHandler.FetchData<ATTENDANCE_ENTRY_MODEL>(timeTable, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchStudentListForAttendanceByCourseIdAndStaffId), sAcademicYear).DataSource.Data;
                if (objAttendanceViewModel.attendanceEntry == null)
                    // FETCH STUDENTS FOR SPECIAL HOURS
                    objAttendanceViewModel.attendanceEntry = (List<ATTENDANCE_ENTRY_MODEL>)CMSHandler.FetchData<ATTENDANCE_ENTRY_MODEL>(timeTable, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchStudentListForAttendanceByCourseIdAndStaffIdForSpecialAttendance), sAcademicYear).DataSource.Data;
                objAttendanceViewModel.attendanceTypes = (List<SUP_ATTENDANCE_TYPE>)CMSHandler.FetchData<SUP_ATTENDANCE_TYPE>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAttendanceTypes)).DataSource.Data;
                objAttendanceViewModel.reasons = (List<SUP_REASON>)CMSHandler.FetchData<SUP_REASON>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchReasonForAttendance)).DataSource.Data;
                return PartialView("LoadStudentsForAttendance", objAttendanceViewModel);
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.BadRequest;
                objResult.Message = Common.ErrorMessage.BadRequest;
                return Json(objResult, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult FetchHoursByStaffId(string sClassId, string sAttendanceDate)
        {
            JsonResultData objResult = new JsonResultData();
            string sOption = string.Empty;
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                List<TT_TIMETABLE_2017> timeTables = new List<TT_TIMETABLE_2017>();
                TT_TIMETABLE_2017 timeTable = new TT_TIMETABLE_2017();
                string sStaffId, sDayOrderId = string.Empty;
                sStaffId = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                sDayOrderId = Session[Common.SESSION_VARIABLES.DAY_ORDER].ToString();
                timeTable.DAY_ORDER_ID = sDayOrderId;
                timeTable.STAFF_ID = sStaffId;
                timeTable.CLASS_ID = sClassId;
                timeTable.ATTENDANCE_DATE = CommonMethods.ConvertdatetoyearFromat(sAttendanceDate);
                timeTables = (List<TT_TIMETABLE_2017>)CMSHandler.FetchData<TT_TIMETABLE_2017>(timeTable, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchHoursByStaffIdAndDayOrder), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                if (timeTables != null)
                {
                    foreach (var item in timeTables)
                    {
                        sOption += "<option value='" + item.HOUR_ID + "'>" + item.HOUR_ID + "</option>";
                    }
                    objResult.ErrorCode = Common.ErrorCode.NotFound;
                    objResult.Message = Common.Messages.NoRecordsfound;
                    objResult.sResult = sOption;
                }
                objResult.ErrorCode = Common.ErrorCode.Ok;
                objResult.Message = Common.Messages.Success;
                objResult.sResult = sOption;

            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                objResult.sResult = sOption;
            }

            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        #region Attendance Admin View
        public ActionResult StudentAttendanceViewForAdmin()
        {
            AttendanceViewModel objViewModel = new AttendanceViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var FetchProgramme = (List<cp_Program_2017>)CMSHandler.FetchData<cp_Program_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgram), sAcademicYear).DataSource.Data;
                    if (FetchProgramme != null && FetchProgramme.Count > 0)
                        objViewModel.ProgrammeList = new SelectList(FetchProgramme, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    List<cp_Classes_2017> listClass = new List<cp_Classes_2017>();
                    listClass.Add(new cp_Classes_2017() { CLASS_ID = "0", CLASS_NAME = "---Select---" });
                    objViewModel.ClassList = new SelectList(listClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");

            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Attendance", "StudentAttendanceViewForAdmin", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Attendance", "StudentAttendanceViewForAdmin", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public JsonResult GetStudentListByClassId(string sProgrammeId)
        {
            JsonResultData objResult = new JsonResultData();
            cp_Classes_2017 objClassModel = new cp_Classes_2017();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sOption = string.Empty;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objClassModel.PROGRAMME = sProgrammeId;
                    var ClassList = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objClassModel, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchClassesByProgrammeId), sAcademicYear).DataSource.Data;
                    if (ClassList != null && ClassList.Count > 0)
                    {
                        sOption = "<option value='0'>--Select--</option>";
                        foreach (var item in ClassList)
                        {
                            sOption += "<option value='" + item.CLASS_ID + "'>" + item.CLASS_NAME + "</option>";
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
                    objHandler.WriteError("Attendance", "GetStudentListByClassId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Attendance", "GetStudentListByClassId", ex.Message);
                }

            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindAttendanceStatusByClassIdAndProgrammeId(string sProgrammeId, string sClassId)
        {
            AttendanceViewModel objViewModel = new AttendanceViewModel();
            Student_Personal_Info objModel = new Student_Personal_Info();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objModel.CLASS_ID = sClassId;
                    var FetchAttendanceStatus = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objModel, SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchAttendanceStatusByClassId), sAcademicYear).DataSource.Data;
                    if (FetchAttendanceStatus != null && FetchAttendanceStatus.Count > 0)
                        objViewModel.liAttendanceStaus = FetchAttendanceStatus;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Attendance", "BindAttendanceStatusByClassIdAndProgrammeId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("Attendance", "BindAttendanceStatusByClassIdAndProgrammeId", ex.Message);
                }

            }
            return View(objViewModel);
        }
        #endregion
    }
}