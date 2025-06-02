using CMS.DAO;
using CMS.Models;
using CMS.SQL.Examination;
using CMS.SQL.SupportData;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class ExaminationController : Controller
    {
        DataValue dv = new DataValue();
        // GET: Examination
        [UserRights("STAFF")]

        #region Cia_Entry

        public async Task<ActionResult> InternalAssessment()
        {
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    using (ExaminationViewModel objExamination = new ExaminationViewModel())
                    {
                        List<CourseList> listCourse = new List<CourseList>();
                        List<ClassList> listClass = new List<ClassList>();
                        CourseInfo objCourseInfo = new CourseInfo();
                        //CourseComponent
                        //CIAMarks
                        DataTable dtCourseList = new DataTable();
                        DataTable dtClassList = new DataTable();

                        objExamination.dv.Clear();
                        objExamination.dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.STAFF_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                        //dtCourseList = objExamination.FetchCourseList().DataSource.Table;

                        dtClassList = await Task.Run(() => objExamination.FetchClassList((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);

                        if (dtCourseList != null & dtCourseList.Rows.Count > 0)
                        {
                            listCourse = await Task.Run(() => (from DataRow dr in dtCourseList.Rows
                                                               select new CourseList()
                                                               {
                                                                   COURSE_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID].ToString(),
                                                                   COURSE_TITLE = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString()
                                                               }).ToList());

                            objExamination.CourseList = new SelectList(listCourse, Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                        }
                        else
                        {
                            listCourse.Add(new CourseList() { COURSE_ID = "0", COURSE_TITLE = "---Select---" });
                            objExamination.CourseList = new SelectList(listCourse, Common.CP_CLASS_COURSE_2017.COURSE_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                        }

                        if (dtClassList != null && dtClassList.Rows.Count > 0)
                        {
                            listClass = await Task.Run(() => (from DataRow dr in dtClassList.Rows
                                                              select new ClassList()
                                                              {
                                                                  CLASS_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID].ToString()
                          ,
                                                                  CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                                                              }).ToList());
                            objExamination.ClassList = new SelectList(listClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);

                        }
                        else
                        {
                            //Need to add select set here 
                        }
                        return View(objExamination);
                    }
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "error");
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ExaminationController", "InternalAssessment", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ExaminationController", "InternalAssessment", ex.Message);
                }
                return RedirectToAction("Index", "Account");
            }


        }

        public async Task<ActionResult> BindStudentsByCourseId(string sCourseId, string sClassId)
        {
            DataTable dtCourseInfo = new DataTable();
            DataTable dtCourseComponents = new DataTable();
            DataTable dtCIAMarks = new DataTable();
            string sSQL = string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            using (ExaminationViewModel objExamination = new ExaminationViewModel())
            {
                try
                {
                    var CIA_COURSE_INFO = new CIA_FETCH_COURSE_INFO();
                    //var CIA_TOTAL = new CIA_TOTAL();
                    CIA_COURSE_INFO.CLASS_ID = sClassId;
                    //CIA_COURSE_INFO.COURSE_ROOT_ID = sCourseId;
                    CIA_COURSE_INFO.COURSE_ID = sCourseId;
                    // Fetch Course Details ....
                    objExamination.liCIACourse_Info = await Task.Run(() => (List<CIA_FETCH_COURSE_INFO>)CMSHandler.FetchData<CIA_FETCH_COURSE_INFO>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseInfoByClassAndCourseId), sAcademicYear).DataSource.Data);
                    if (objExamination.liCIACourse_Info != null && objExamination.liCIACourse_Info.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(objExamination.liCIACourse_Info.FirstOrDefault().COURSE_TYPE_ID) && !objExamination.liCIACourse_Info.FirstOrDefault().COURSE_TYPE_ID.Equals("0"))
                        {
                            CIA_COURSE_INFO.COURSE_TYPE_ID = objExamination.liCIACourse_Info.FirstOrDefault().COURSE_TYPE_ID;
                            CIA_COURSE_INFO.SEMESTER_ID = objExamination.liCIACourse_Info.FirstOrDefault().SEMESTER_ID;
                            // Fetch Component Details ...
                            objExamination.liCIA_Component = await Task.Run(() => (List<CIA_COMPONENT>)CMSHandler.FetchData<CIA_COMPONENT>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseComponentByCourseTypeId), sAcademicYear).DataSource.Data);
                            // FetchMarks ...
                            // objExamination.liCIA_Marks_Info = await Task.Run(() => (List<CIA_MARKS_INFO>)CMSHandler.FetchData<CIA_MARKS_INFO>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIA_Marks), sAcademicYear).DataSource.Data);
                            var componentInfo = (List<CIA_MARKS_INFO>)CMSHandler.FetchData<CIA_MARKS_INFO>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIAComponentsByCourseId), sAcademicYear).DataSource.Data;
                            var markInfo = (List<CIA_MARKS_INFO>)CMSHandler.FetchData<CIA_MARKS_INFO>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIA_Marks_Info), sAcademicYear).DataSource.Data;
                            foreach (var item in componentInfo)
                            {
                                var temp = markInfo.Where(o => o.STUDENT_ID == item.STUDENT_ID && o.COURSE_GROUP_ID == item.COURSE_GROUP_ID).ToList();
                                if (temp != null && temp.Count > 0) { item.COURSE_GROUP_MARK = temp.FirstOrDefault().COURSE_GROUP_MARK; item.CIA_GROUP_MARK_ID = temp.FirstOrDefault().CIA_GROUP_MARK_ID; }
                            }
                            componentInfo.ForEach(o => o.COURSE_GROUP_MARK = string.Equals(o.COURSE_GROUP_MARK, "0") ? "" : o.COURSE_GROUP_MARK);

                            var student = componentInfo.GroupBy(o => o.STUDENT_ID).Select(x => new CIA_TOTAL() { STUDENT_ID = x.FirstOrDefault().STUDENT_ID }).ToList();
                            //var StartDate= await Task.Run(() => (List<CIA_FETCH_COURSE_INFO>)CMSHandler.FetchData<CIA_FETCH_COURSE_INFO>(CIA_COURSE_INFO, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemesterStartDateByClassId), sAcademicYear).DataSource.Data);
                            //CIA_COURSE_INFO.DATE_FROM = StartDate.FirstOrDefault().DATE_FROM;
                            //var EndDate = await Task.Run(() => (List<CIA_FETCH_COURSE_INFO>)CMSHandler.FetchData<CIA_FETCH_COURSE_INFO>(CIA_COURSE_INFO,SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemesterEndDateByClassId), sAcademicYear).DataSource.Data);
                            //CIA_COURSE_INFO.DATE_TO = EndDate.FirstOrDefault().DATE_TO;
                            //sSQL = SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchTotalHoursByClassIdAndCourseId).Replace(Common.Delimiter.QUS + Common.CP_SEMESTER_ROOT_2017.DATE_FROM, CIA_COURSE_INFO.DATE_FROM);
                            //sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_SEMESTER_ROOT_2017.DATE_TO, CIA_COURSE_INFO.DATE_TO);
                            //var FetchTotalHours = await Task.Run(() => (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(CIA_COURSE_INFO,sSQL, sAcademicYear).DataSource.Data);
                            //sSQL = SQL.Attendance.AttendanceSQL.GetAttendanceSQL(AttendanceSQLCommands.FetchTotalAbsentHoursByClassIdAndCourseId).Replace(Common.Delimiter.QUS + Common.CP_SEMESTER_ROOT_2017.DATE_FROM, CIA_COURSE_INFO.DATE_FROM);
                            //sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_SEMESTER_ROOT_2017.DATE_TO, CIA_COURSE_INFO.DATE_TO);
                            //var FetchAbsentHours = await Task.Run(() => (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(CIA_COURSE_INFO,sSQL, sAcademicYear).DataSource.Data);
                            //if (FetchTotalHours != null && FetchTotalHours.Count > 0 && FetchAbsentHours != null && FetchAbsentHours.Count > 0)
                            //{
                            //    var TotalHoursToUpdate = FetchTotalHours.FirstOrDefault().TOTAL_HOURS;
                            //    var AttendanceMarksList = new List<Student_Personal_Info>();
                            //    foreach (var item in FetchAbsentHours)
                            //    {
                            //        item.ACTUAL_PERCENTAGE = Math.Round((Convert.ToDouble(TotalHoursToUpdate) - (Convert.ToDouble(item.ABSENT_HOURS))) / Convert.ToDouble(TotalHoursToUpdate)) * 100;
                            //        item.PERCENTAGE = Convert.ToString((item.ACTUAL_PERCENTAGE));
                            //        //var AttendancePercentageInfo = liCIA_Marks_Info.Where(o => o.STUDENT_ID == item.STUDENT_ID).ToList();
                            //        AttendanceMarksList.Add(new Student_Personal_Info() { STUDENT_ID = item.STUDENT_ID, PERCENTAGE = item.PERCENTAGE, COURSE_ID = item.COURSE_ID });
                            //        //objExamination.liCIA_Marks_Info.Add( new CIA_MARKS_INFO() { STUDENT_ID=item.STUDENT_ID,NAME=item.STUDENT_ID, PERCENTAGE = Convert.ToString(item.ACTUAL_PERCENTAGE)});
                            //    }
                            //    objExamination.liAttendanceInfo = AttendanceMarksList;
                            //}
                            objExamination.liCIA_Marks_Info = componentInfo;

                            // string SQL = string.Join(",", componentInfo.Select(o => o.CIA_GROUP_MARK_ID));

                            objExamination.liCIA_Total = student;


                            // check existing marks ..
                            // if (objExamination.liCIA_Marks_Info == null)
                            //   objExamination.liCIA_Marks_Info = await Task.Run(() => (List<CIA_MARKS_INFO>)CMSHandler.FetchData<CIA_MARKS_INFO>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIA_Empty_Marks), sAcademicYear).DataSource.Data);
                            // FetchCIATotal ...
                            // objExamination.liCIA_Total = await Task.Run(() => (List<CIA_TOTAL>)CMSHandler.FetchData<CIA_TOTAL>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIATotal), sAcademicYear).DataSource.Data);
                            return View(objExamination);
                        }
                        else
                        {
                            return Json("Missing Course Type For This Course Please Meet Admin!!!", JsonRequestBehavior.AllowGet);

                        }
                    }
                    else
                    {
                        return Json("Missing Course Please Meet Admin!!!", JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("ExaminationController", "BindStudentsByCourseId", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("ExaminationController", "BindStudentsByCourseId", ex.Message);
                    }
                    return View(objExamination);
                }
            }
        }
        [UserRights("STAFF")]
        public async Task<ActionResult> CIAMarksReports()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                using (ExaminationViewModel objExamination = new ExaminationViewModel())
                {
                    try
                    {
                        List<CourseList> listCourse = new List<CourseList>();
                        List<ClassList> listClass = new List<ClassList>();
                        CourseInfo objCourseInfo = new CourseInfo();
                        //CourseComponent
                        //CIAMarks
                        DataTable dtCourseList = new DataTable();
                        DataTable dtClassList = new DataTable();
                        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        objExamination.dv.Clear();
                        objExamination.dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.STAFF_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                        //dtCourseList = objExamination.FetchCourseList().DataSource.Table;

                        dtClassList = await Task.Run(() => objExamination.FetchClassList((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);

                        if (dtCourseList != null & dtCourseList.Rows.Count > 0)
                        {
                            listCourse = await Task.Run(() => (from DataRow dr in dtCourseList.Rows
                                                               select new CourseList()
                                                               {
                                                                   COURSE_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID].ToString(),
                                                                   COURSE_TITLE = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString()
                                                               }).ToList());

                            objExamination.CourseList = new SelectList(listCourse, Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                        }
                        else
                        {
                            listCourse.Add(new CourseList() { COURSE_ID = "0", COURSE_TITLE = "---Select---" });
                            objExamination.CourseList = new SelectList(listCourse, Common.CP_CLASS_COURSE_2017.COURSE_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                        }

                        if (dtClassList != null && dtClassList.Rows.Count > 0)
                        {
                            listClass = await Task.Run(() => (from DataRow dr in dtClassList.Rows
                                                              select new ClassList()
                                                              {
                                                                  CLASS_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID].ToString()
                          ,
                                                                  CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                                                              }).ToList());
                            objExamination.ClassList = new SelectList(listClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);

                        }
                        else
                        {
                            //Need to add select set here 
                        }
                        return View(objExamination);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("ExaminationController", "CIAMarksReports", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("ExaminationController", "CIAMarksReports", ex.Message);
                        }
                        return View();
                    }

                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }


            //  return View();
        }
        [UserRights("STAFF")]
        public async Task<ActionResult> CIAComponentMarks()
        {

            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                using (ExaminationViewModel objExamination = new ExaminationViewModel())
                {
                    try
                    {
                        List<CourseList> listCourse = new List<CourseList>();
                        List<ClassList> listClass = new List<ClassList>();
                        CourseInfo objCourseInfo = new CourseInfo();
                        //CourseComponent
                        //CIAMarks
                        DataTable dtCourseList = new DataTable();
                        DataTable dtClassList = new DataTable();
                        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        objExamination.dv.Clear();
                        objExamination.dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.STAFF_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                        //dtCourseList = objExamination.FetchCourseList().DataSource.Table;

                        dtClassList = await Task.Run(() => objExamination.FetchClassList((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);

                        if (dtCourseList != null & dtCourseList.Rows.Count > 0)
                        {
                            listCourse = await Task.Run(() => (from DataRow dr in dtCourseList.Rows
                                                               select new CourseList()
                                                               {
                                                                   COURSE_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID].ToString(),
                                                                   COURSE_TITLE = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString()
                                                               }).ToList());

                            objExamination.CourseList = new SelectList(listCourse, Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                        }
                        else
                        {
                            listCourse.Add(new CourseList() { COURSE_ID = "0", COURSE_TITLE = "---Select---" });
                            objExamination.CourseList = new SelectList(listCourse, Common.CP_CLASS_COURSE_2017.COURSE_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                        }

                        if (dtClassList != null && dtClassList.Rows.Count > 0)
                        {
                            listClass = await Task.Run(() => (from DataRow dr in dtClassList.Rows
                                                              select new ClassList()
                                                              {
                                                                  CLASS_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID].ToString()
                          ,
                                                                  CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                                                              }).ToList());
                            objExamination.ClassList = new SelectList(listClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);

                        }
                        else
                        {
                            //Need to add select set here 
                        }
                    }
                    catch (Exception ex)
                    {

                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("ExaminationController", "CIAComponentMarks", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("ExaminationController", "CIAComponentMarks", ex.Message);
                        }
                    }

                    return View(objExamination);
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }

        }
        public async Task<ActionResult> BindCIAMarkList(string sCourseId, string sClassId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            DataTable dtCourseInfo = new DataTable();
            DataTable dtCourseComponents = new DataTable();
            DataTable dtCIAMarks = new DataTable();
            // string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            using (ExaminationViewModel objExamination = new ExaminationViewModel())
            {
                try
                {

                    var ObjCIA_Component_Info = new CIA_COMPONENT();
                    var ObjCIA_Staff_Info = new CIA_STAFF_INFO();
                    ObjCIA_Staff_Info.CLASS_ID = sClassId;
                    ObjCIA_Staff_Info.COURSE_ID = sCourseId;

                    //objExamination.liCIACourse_Info = await Task.Run(() => (List<CIA_FETCH_COURSE_INFO>)CMSHandler.FetchData<CIA_FETCH_COURSE_INFO>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseInfoByClassAndCourseId), sAcademicYear).DataSource.Data);

                    objExamination.liCIA_Staff_Info = await Task.Run(() => (List<CIA_STAFF_INFO>)CMSHandler.FetchData<CIA_STAFF_INFO>(ObjCIA_Staff_Info, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseAndClassInfoByClassAndCourseId), sAcademicYear).DataSource.Data);
                    ObjCIA_Component_Info.COURSE_TYPE_ID = objExamination.liCIA_Staff_Info.FirstOrDefault().COURSE_TYPE;
                    objExamination.liCIA_Component = await Task.Run(() => (List<CIA_COMPONENT>)CMSHandler.FetchData<CIA_COMPONENT>(ObjCIA_Component_Info, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseComponentByCourseTypeId), sAcademicYear).DataSource.Data);

                    objExamination.liCIA_Total = await Task.Run(() => (List<CIA_TOTAL>)CMSHandler.FetchData<CIA_TOTAL>(ObjCIA_Staff_Info, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIATotal), sAcademicYear).DataSource.Data);
                    objExamination.liCIA_Total.ForEach(o => o.TOTAL = Math.Round(Convert.ToDecimal((o.TOTAL).ToUpper().Equals("A") ? "0" : o.TOTAL), MidpointRounding.AwayFromZero).ToString());

                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("ExaminationController", "BindCIAMarkList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("ExaminationController", "BindCIAMarkList", ex.Message);
                    }
                }
                return View(objExamination);
            }
        }

        public async Task<ActionResult> BindCIAComponentMarkList(string sCourseId, string sClassId)
        {
            DataTable dtCourseInfo = new DataTable();
            DataTable dtCourseComponents = new DataTable();
            DataTable dtCIAMarks = new DataTable();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            using (ExaminationViewModel objExamination = new ExaminationViewModel())
            {
                try
                {
                    var CIA_COURSE_INFO = new CIA_FETCH_COURSE_INFO();
                    //var CIA_TOTAL = new CIA_TOTAL();
                    CIA_COURSE_INFO.CLASS_ID = sClassId;
                    //CIA_COURSE_INFO.COURSE_ROOT_ID = sCourseId;
                    CIA_COURSE_INFO.COURSE_ID = sCourseId;
                    // Fetch Course Details ....
                    objExamination.liCIA_Staff_Info = await Task.Run(() => (List<CIA_STAFF_INFO>)CMSHandler.FetchData<CIA_STAFF_INFO>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseAndClassInfoByClassAndCourseId), sAcademicYear).DataSource.Data);
                    objExamination.liCIACourse_Info = await Task.Run(() => (List<CIA_FETCH_COURSE_INFO>)CMSHandler.FetchData<CIA_FETCH_COURSE_INFO>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseInfoByClassAndCourseId), sAcademicYear).DataSource.Data);
                    CIA_COURSE_INFO.COURSE_TYPE_ID = objExamination.liCIACourse_Info.FirstOrDefault().COURSE_TYPE_ID;
                    CIA_COURSE_INFO.SEMESTER_ID = objExamination.liCIACourse_Info.FirstOrDefault().SEMESTER_ID;
                    // Fetch Component Details ...
                    objExamination.liCIA_Component = await Task.Run(() => (List<CIA_COMPONENT>)CMSHandler.FetchData<CIA_COMPONENT>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseComponentByCourseTypeId), sAcademicYear).DataSource.Data);
                    // FetchMarks ...
                    objExamination.liCIA_Marks_Info = await Task.Run(() => (List<CIA_MARKS_INFO>)CMSHandler.FetchData<CIA_MARKS_INFO>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIA_Marks), sAcademicYear).DataSource.Data);

                    // FetchCIATotal ...
                    objExamination.liCIA_Total = await Task.Run(() => (List<CIA_TOTAL>)CMSHandler.FetchData<CIA_TOTAL>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIATotal), sAcademicYear).DataSource.Data);
                    objExamination.liCIA_Total.ForEach(o => o.TOTAL = Math.Round(Convert.ToDecimal((o.TOTAL).ToUpper().Equals("A") ? "0" : o.TOTAL), MidpointRounding.AwayFromZero).ToString());
                    // check Existing Total ...
                    //if(objExamination.liCIA_Total==null)
                    //objExamination.liCIA_Total = await Task.Run(() => (List<CIA_TOTAL>)CMSHandler.FetchData<CIA_TOTAL>(CIA_COURSE_INFO, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIA_Empty_Total), sAcademicYear).DataSource.Data);
                    return View(objExamination);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("ExaminationController", "BindCIAComponentMarkList", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("ExaminationController", "BindCIAComponentMarkList", ex.Message);
                    }
                }
                //objExamination.CourseList
                return View(objExamination);
            }
        }

        public async Task<string> GetCourseListByClassIdAndStaffId(string sClassId)
        {
            string sOption = string.Empty;
            DataTable dtCourseList = new DataTable();
            DataTable dtClassList = new DataTable();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            using (ExaminationViewModel objExamination = new ExaminationViewModel())
            {
                try
                {
                    objExamination.dv.Clear();
                    objExamination.dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.STAFF_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                    objExamination.dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
                    dtCourseList = await Task.Run(() => objExamination.FetchCourseList((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);

                    foreach (DataRow item in dtCourseList.Rows)
                    {
                        sOption += "<option value='" + item[Common.CP_CLASS_COURSE_2017.COURSE_ID].ToString() + "' >" + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString() + "</option>";
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("ExaminationController", "GetCourseListByClassIdAndStaffId", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("ExaminationController", "GetCourseListByClassIdAndStaffId", ex.Message);
                    }

                }
            }

            return sOption;
        }

        [HttpPost]
        public async Task<string> SaveCIAMarks(string jsonCIA_MARKS)
        {
            string sResult = string.Empty;
            try
            {
                sResult = await SaveCIAAsync(jsonCIA_MARKS);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ExaminationController", "SaveCIAMarks", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ExaminationController", "SaveCIAMarks", ex.Message);
                }
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return sResult;
        }

        private async Task<string> SaveCIAAsync(string jsonCIA_MARKS)
        {
            string sResult = string.Empty;
            bool insertFlag = false;
            bool updateFlag = false;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                string sInsertQuery = string.Empty;
                string sInsertQueryCom = string.Empty;
                string sUpdateQuery = string.Empty;
                ResultArgs resultArgs = new ResultArgs();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JSON_CIA_MARKS objJson = serializer.Deserialize<JSON_CIA_MARKS>(jsonCIA_MARKS);
                if (objJson.SAVE_CIA_MARKS != null && objJson.SAVE_CIA_MARKS.Count > 0)
                {
                    sInsertQuery = @"INSERT INTO CIA_GROUP_MARKS_?AC_YEAR (
                                STUDENT_ID,
                                COURSE_ID,
                                COURSE_GROUP_ID,
                                COURSE_GROUP_MARK,
                                SEMESTER_ID)VALUES";
                    sInsertQueryCom = sInsertQuery;
                    foreach (var item in objJson.SAVE_CIA_MARKS)
                    {
                        if (string.IsNullOrEmpty(item.COURSE_GROUP_MARK))
                            item.COURSE_GROUP_MARK = "0";

                        if (string.IsNullOrEmpty(item.CIA_GROUP_MARK_ID))
                        {
                            sInsertQuery += "('" + item.STUDENT_ID.Trim() + "','" + item.COURSE_ID.Trim() + "','" + item.COURSE_GROUP_ID.Trim() + "','" + item.COURSE_GROUP_MARK.Trim() + "','" + item.SEMESTER_ID.Trim() + "'),";
                            insertFlag = true;
                        }
                        else
                        {
                            sUpdateQuery += @"UPDATE CIA_GROUP_MARKS_?AC_YEAR SET STUDENT_ID='" + item.STUDENT_ID.Trim() + "', COURSE_ID='" + item.COURSE_ID.Trim() + "', COURSE_GROUP_ID='" + item.COURSE_GROUP_ID.Trim() + "', COURSE_GROUP_MARK='" + item.COURSE_GROUP_MARK.Trim() + "', SEMESTER_ID='" + item.SEMESTER_ID.Trim() + "' WHERE CIA_GROUP_MARK_ID='" + item.CIA_GROUP_MARK_ID.Trim() + "';";
                            updateFlag = true;
                        }
                    }
                    if (insertFlag)
                    {
                        sInsertQuery = sInsertQuery.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                        sInsertQuery = sInsertQuery.TrimEnd(',');
                        sInsertQuery = sInsertQuery + ";";
                    }

                    using (ExaminationViewModel objExamination = new ExaminationViewModel())
                    {
                        if (updateFlag)
                        {
                            sUpdateQuery = sUpdateQuery.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                            resultArgs = objExamination.ExcuteCIA_Marks(sUpdateQuery);

                        }
                        if (insertFlag)
                        {
                            resultArgs = objExamination.ExcuteCIA_Marks(sInsertQuery);
                        }
                        sResult = resultArgs.Success ? "Marks have been updated successfully...!" : Common.Messages.FailedToSaveRecords;
                    }
                }
                else
                {
                    sResult = "No changes made !!";
                }

            }
            else
            {
                sResult = "Sorry Your Session is expaired.Please Login again !!";
            }

            return sResult;
        }

        #endregion

        #region Exam Registration
        [UserRights("EXAMINCHARGE")]
        public ActionResult ArrearCourseList()
        {
            JsonResult objResult = new JsonResult();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                ExaminationViewModel examViewModel = new ExaminationViewModel();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                var batches = (List<sup_Batch>)CMSHandler.FetchData<sup_Batch>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicBatches)).DataSource.Data;
                var programmes = (List<CP_PROGRAMME_2017>)CMSHandler.FetchData<CP_PROGRAMME_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgram), sAcademicYear).DataSource.Data;
                if (batches != null)
                {
                    examViewModel.Batch = new SelectList(batches, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
                }
                if (programmes != null)
                {
                    examViewModel.Program = new SelectList(programmes, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                return View(examViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }

        }
        public JsonResult SaveArrearFee(string jsonInput)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                EXAM_COURSE_WISE_HEADS objCourses = new EXAM_COURSE_WISE_HEADS();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                var sjsonCourseHeads = JsonConvert.DeserializeObject<List<EXAM_COURSE_WISE_HEADS>>(jsonInput);
                if (sjsonCourseHeads != null && sjsonCourseHeads.Count > 0)
                {
                    var activeSemester = (List<SemesterRootInfo>)CMSHandler.FetchData<SemesterRootInfo>(new SemesterRootInfo() { PROGRAMME = sjsonCourseHeads.FirstOrDefault().PROGRAMME_ID, BATCH = sjsonCourseHeads.FirstOrDefault().BATCH_ID }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchActiveSemesterByProgrammeAndBatch), sAcademicYear).DataSource.Data;
                    if (activeSemester != null && activeSemester.Count > 0)
                    {
                        foreach (var item in sjsonCourseHeads)
                        {
                            if (item.PROGRAMME_HEAD_ID != null && item.PROGRAMME_HEAD_ID != "0")
                            {
                                item.SEMESTER = activeSemester.FirstOrDefault().SEMESTER;
                                item.ACADEMIC_YEAR = sAcademicYear;
                                var result = CMSHandler.SaveOrUpdate(item, ExaminationSQL.GetExaminationSQL((!string.IsNullOrEmpty(item.COURSE_HEAD_ID)) ? ExaminationSQLCommands.UpdateCourseHeads : ExaminationSQLCommands.SaveCourseHeads), sAcademicYear);
                                if (result.Success)
                                {
                                    objResult.ErrorCode = Common.ErrorCode.Accepted;
                                    objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    objResult.ErrorCode = Common.ErrorCode.ExpectationFailed;
                                    objResult.Message = Common.Messages.FailedToSaveRecords;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(objResult.Message))
                                {
                                    objResult.Message = "Head should not be empty !!";
                                }
                            }

                        }
                    }
                    else
                    {
                        objResult.ErrorCode = Common.ErrorCode.FailedDependency;
                        objResult.Message = Common.ErrorMessage.FailedDependency;
                    }

                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);

        }
        public ActionResult BindArrearList(string sProgramme, string sBatch)
        {
            JsonResultData objResult = new JsonResultData();
            ProgrammeHeadsAmount objView = new ProgrammeHeadsAmount();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sSQL = string.Empty;
                SemesterRootInfo semester = new SemesterRootInfo();
                semester.PROGRAMME = sProgramme;
                semester.BATCH = sBatch;
                PROGRAMME_HEADS_AMOUNT programme = new PROGRAMME_HEADS_AMOUNT();
                List<ACADEMIC_CURRICULUM_2017> AcademicCurriculum = new List<ACADEMIC_CURRICULUM_2017>();
                objView.SubjectType = (List<SUP_SUBJECT_TYPE>)CMSHandler.FetchData<SUP_SUBJECT_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FecthSubjectType)).DataSource.Data;
                programme.BATCH_ID = sBatch;
                programme.PROGRAMME_ID = sProgramme;
                var programeList = (List<EXAM_PROGRAMME_WISE_HEADS>)CMSHandler.FetchData<EXAM_PROGRAMME_WISE_HEADS>(programme, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchExamFeeStructureByProgrammeIdAndBatch), sAcademicYear).DataSource.Data;
                objView.programeList = programeList;
                var CourseHeadsList = (List<EXAM_COURSE_WISE_HEADS>)CMSHandler.FetchData<EXAM_COURSE_WISE_HEADS>(programme, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseHeadList), sAcademicYear).DataSource.Data;
                var activeSemester = (List<SemesterRootInfo>)CMSHandler.FetchData<SemesterRootInfo>(new SemesterRootInfo() { PROGRAMME = programme.PROGRAMME_ID, BATCH = programme.BATCH_ID }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchActiveSemesterByProgrammeAndBatch), sAcademicYear).DataSource.Data;
                var liActiveSemester = (List<GET_ACTIVE_SEMESTER_BATCH>)CMSHandler.FetchData<GET_ACTIVE_SEMESTER_BATCH>(new SemesterRootInfo() { PROGRAMME = programme.PROGRAMME_ID, BATCH = programme.BATCH_ID }, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchActiveSemesterWithBatches), sAcademicYear).DataSource.Data;
                if (activeSemester != null && activeSemester.Count > 0)
                {
                    CommonMethods.SemestersInComma(Convert.ToInt16(activeSemester.FirstOrDefault().SEMESTER));
                    sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchArrearCourseByProgrammeIdAndBatch).Replace(Common.Delimiter.QUS + Common.CP_SEMESTER_ROOT_2017.SEMESTER, CommonMethods.SemestersInComma(Convert.ToInt16(activeSemester.FirstOrDefault().SEMESTER)));
                }
                if (liActiveSemester != null && liActiveSemester.Count > 0)
                {
                    foreach (var item in liActiveSemester)
                    {
                        var TempAcademicCurriculum = (List<ACADEMIC_CURRICULUM_2017>)CMSHandler.FetchData<ACADEMIC_CURRICULUM_2017>(programme, sSQL, item.AC_YEAR).DataSource.Data;
                        if (TempAcademicCurriculum != null && TempAcademicCurriculum.Count > 0)
                        {
                            AcademicCurriculum.AddRange(TempAcademicCurriculum);
                        }
                    }
                }
                if (AcademicCurriculum != null && AcademicCurriculum.Count > 0)
                {
                    if (CourseHeadsList != null && CourseHeadsList.Count > 0)
                    {
                        foreach (var item in CourseHeadsList)
                        {
                            AcademicCurriculum.Where(o => o.COURSE_CODE == item.COURSE_CODE).ToList().ForEach(k => k.AMOUNT = item.AMOUNT);
                            AcademicCurriculum.Where(o => o.COURSE_CODE == item.COURSE_CODE).ToList().ForEach(k => k.HEAD = item.PROGRAMME_HEAD_ID);
                            AcademicCurriculum.Where(o => o.COURSE_CODE == item.COURSE_CODE).ToList().ForEach(k => k.COURSE_HEAD_ID = item.COURSE_HEAD_ID);
                        }
                    }
                    if (programeList != null && programeList.Count > 0)
                    {
                        foreach (var item in programeList)
                        {
                            AcademicCurriculum.Where(o => string.IsNullOrEmpty(o.AMOUNT) && o.SUBJECT_TYPE_ID == item.SUBJECT_TYPE).ToList().ForEach(k => k.AMOUNT = item.AMOUNT);
                        }

                    }
                    objView.academicCurriculum = AcademicCurriculum;
                    return PartialView("BindArrearList", objView);
                }
                else
                {
                    return PartialView("BindArrearList", objView);
                }

            }
            else
            {
                return Json(Common.Messages.SessionIsExpiredPleaseLoginAgain, JsonRequestBehavior.AllowGet);
            }
        }
        [UserRights("EXAMINCHARGE")]
        public ActionResult ExamFeeList()
        {
            JsonResult objResult = new JsonResult();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                ExaminationViewModel examViewModel = new ExaminationViewModel();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                var batches = (List<sup_Batch>)CMSHandler.FetchData<sup_Batch>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicBatches)).DataSource.Data;
                var programmes = (List<CP_PROGRAMME_2017>)CMSHandler.FetchData<CP_PROGRAMME_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgram), sAcademicYear).DataSource.Data;
                if (batches != null)
                {
                    examViewModel.Batch = new SelectList(batches, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
                }
                if (programmes != null)
                {
                    examViewModel.Program = new SelectList(programmes, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                return View(examViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }


        public JsonResult DeleteCourseHead(string sCourseHeadId)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                EXAM_COURSE_WISE_HEADS examCourseHead = new EXAM_COURSE_WISE_HEADS();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                examCourseHead.COURSE_HEAD_ID = sCourseHeadId;
                var result = CMSHandler.SaveOrUpdate(examCourseHead, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.DeleteCourseHeads), sAcademicYear);
                if (result.Success)
                {
                    objResult.ErrorCode = Common.ErrorCode.Ok;
                    objResult.Message = Common.Messages.RecordDeletedSuccessfully;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.NotAcceptable;
                    objResult.Message = Common.Messages.FailedToDeletedTryAgain;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        [UserRights("EXAMINCHARGE")]
        public ActionResult CourseHeadsList()
        {
            JsonResult objResult = new JsonResult();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                ExaminationViewModel examViewModel = new ExaminationViewModel();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                var batches = (List<sup_Batch>)CMSHandler.FetchData<sup_Batch>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicBatches)).DataSource.Data;
                var programmes = (List<CP_PROGRAMME_2017>)CMSHandler.FetchData<CP_PROGRAMME_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgram), sAcademicYear).DataSource.Data;
                if (batches != null)
                {
                    examViewModel.Batch = new SelectList(batches, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
                }
                if (programmes != null)
                {
                    examViewModel.Program = new SelectList(programmes, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                return View(examViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }


        }


        public ActionResult BindCourseHeads(string sProgramme, string sBatch)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                ProgrammeHeadsAmount objView = new ProgrammeHeadsAmount();
                objView.CourseHeadsList = (List<EXAM_COURSE_WISE_HEADS>)CMSHandler.FetchData<EXAM_COURSE_WISE_HEADS>(new EXAM_COURSE_WISE_HEADS() { PROGRAMME_ID = sProgramme, BATCH_ID = sBatch }, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseHeadList), sAcademicYear).DataSource.Data;
                return PartialView("BindCourseHeads", objView);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }
        public JsonResult GetProgrameId(string sProgramme, string sBatch, string sCourseCode)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                EXAM_COURSE_WISE_HEADS objCourse = new EXAM_COURSE_WISE_HEADS();
                objCourse.ACADEMIC_YEAR = sAcademicYear;
                objCourse.BATCH_ID = sBatch;
                objCourse.COURSE_CODE = sCourseCode;
                objCourse.PROGRAMME_ID = sProgramme;
                var programmeId = (List<EXAM_COURSE_WISE_HEADS>)CMSHandler.FetchData<EXAM_COURSE_WISE_HEADS>(objCourse, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.GetProgrammeIdByCourseCodeAndBatch)).DataSource.Data;
                if (programmeId != null)
                {
                    objResult.sResultStringArray.Add(programmeId.FirstOrDefault().PROGRAMME_HEAD_ID);
                    objResult.sResultStringArray.Add(programmeId.FirstOrDefault().COURSE_HEAD_ID);
                }
            }
            else
            {
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveOrUpdateCourseHeads(string sJsonCourseHeads)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (!string.IsNullOrEmpty(sJsonCourseHeads))
                {
                    var courseHead = JsonConvert.DeserializeObject<EXAM_COURSE_WISE_HEADS>(sJsonCourseHeads);
                    if (courseHead != null)
                    {
                        var activeSemester = (List<SemesterRootInfo>)CMSHandler.FetchData<SemesterRootInfo>(new SemesterRootInfo() { PROGRAMME = courseHead.PROGRAMME_ID, BATCH = courseHead.BATCH_ID }, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchActiveSemesterByProgrammeAndBatch), sAcademicYear).DataSource.Data;
                        if (activeSemester != null)
                        {
                            courseHead.SEMESTER = activeSemester.FirstOrDefault().SEMESTER;
                        }
                        courseHead.ACADEMIC_YEAR = sAcademicYear;
                        var result = CMSHandler.SaveOrUpdate(courseHead, ExaminationSQL.GetExaminationSQL((!string.IsNullOrEmpty(courseHead.COURSE_HEAD_ID)) ? ExaminationSQLCommands.UpdateCourseHeads : ExaminationSQLCommands.SaveCourseHeads), sAcademicYear);
                        if (result.Success)
                        {
                            objResult.ErrorCode = Common.ErrorCode.Ok;
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                        {
                            objResult.ErrorCode = Common.ErrorCode.FailedDependency;
                            objResult.Message = Common.Messages.FailedToSaveRecords;
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

        public ActionResult SetHeadsForCourse()
        {
            JsonResult objResult = new JsonResult();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                ExaminationViewModel examViewModel = new ExaminationViewModel();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                var batches = (List<sup_Batch>)CMSHandler.FetchData<sup_Batch>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicBatches)).DataSource.Data;
                var programmes = (List<CP_PROGRAMME_2017>)CMSHandler.FetchData<CP_PROGRAMME_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgram), sAcademicYear).DataSource.Data;
                var heads = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHead)).DataSource.Data;
                examViewModel.examSetting = (List<EXAM_SETTING>)CMSHandler.FetchData<EXAM_SETTING>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchExamSettingByAcitveId), sAcademicYear).DataSource.Data;

                if (heads != null)
                {
                    examViewModel.Heads = new SelectList(heads, Common.SUP_HEAD.HEAD_ID, Common.SUP_HEAD.HEAD);
                }
                if (batches != null)
                {
                    examViewModel.Batch = new SelectList(batches, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
                }
                if (programmes != null)
                {
                    examViewModel.Program = new SelectList(programmes, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                return View(examViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }

        }

        public JsonResult LoadCourseForHeads(string sProgramme, string sBatch)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sCourseOptions = string.Empty;
                string sHeadOptions = string.Empty;
                AcademicCurriculumInfo academicCurriculumInfo = new AcademicCurriculumInfo();
                academicCurriculumInfo.PROGRAMME = sProgramme;
                academicCurriculumInfo.BATCH = sBatch;
                var Courses = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(academicCurriculumInfo, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCourseListByProgrammeAndBatch), sAcademicYear).DataSource.Data;
                var ProgrammeHeads = (List<EXAM_PROGRAMME_WISE_HEADS>)CMSHandler.FetchData<EXAM_PROGRAMME_WISE_HEADS>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchProgramHeadsList), sAcademicYear).DataSource.Data;

                if (Courses != null)
                {
                    foreach (var item in Courses)
                    {
                        sCourseOptions += "<option value='" + item.COURSE_CODE + "'>" + item.COURSE_TITLE + "</option>";
                    }
                    objResult.sResultStringArray.Add(sCourseOptions);
                    if (ProgrammeHeads != null)
                    {
                        var ProgrammeHeadsByBatch = ProgrammeHeads.Where(o => o.PROGRAMME_ID == sProgramme && o.BATCH_ID == sBatch);
                        if (ProgrammeHeadsByBatch != null)
                        {
                            foreach (var item in ProgrammeHeadsByBatch)
                            {
                                sHeadOptions += "<option value='" + item.PROGRAMME_HEAD_ID + "'>" + item.HEAD + "</option>";
                            }
                            objResult.sResultStringArray.Add(sHeadOptions);
                        }
                    }

                    objResult.ErrorCode = Common.ErrorCode.Ok;
                    objResult.Message = Common.ErrorMessage.Ok;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        [UserRights("EXAMINCHARGE")]
        public ActionResult ListProgramList()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                ProgrammeHeadsAmount objView = new ProgrammeHeadsAmount();
                objView.programeList = (List<EXAM_PROGRAMME_WISE_HEADS>)CMSHandler.FetchData<EXAM_PROGRAMME_WISE_HEADS>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchProgramHeadsList), sAcademicYear).DataSource.Data;
                return View(objView);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }

        }
        public JsonResult DeleteProgramHead(string sProgrameHeadId)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                EXAM_PROGRAMME_WISE_HEADS examProgramHead = new EXAM_PROGRAMME_WISE_HEADS();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                examProgramHead.PROGRAMME_HEAD_ID = sProgrameHeadId;
                var result = CMSHandler.SaveOrUpdate(examProgramHead, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.DeleteProgrameHeads), sAcademicYear);
                if (result.Success)
                {
                    objResult.ErrorCode = Common.ErrorCode.Ok;
                    objResult.Message = Common.Messages.RecordDeletedSuccessfully;
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.NotAcceptable;
                    objResult.Message = Common.Messages.FailedToDeletedTryAgain;
                }
            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveProgrammeHeads(string sJsonProgrammeHeads)
        {
            JsonResultData objResult = new JsonResultData();
            int sCount = 0, fCount = 0;

            if (!string.IsNullOrEmpty(sJsonProgrammeHeads))
            {
                var programmeHeads = JsonConvert.DeserializeObject<JsonProgrammeHeads>(sJsonProgrammeHeads);
                if (programmeHeads.ProgrammeHeads != null)
                {
                    foreach (var item in programmeHeads.ProgrammeHeads)
                    {
                        var result = CMSHandler.SaveOrUpdate(item, ExaminationSQL.GetExaminationSQL((!string.IsNullOrEmpty(item.PROGRAMME_HEAD_ID)) ? ExaminationSQLCommands.UpdateProgrammeHeads : ExaminationSQLCommands.SaveProgrammeHeads));
                        if (result.Success)
                            sCount++;
                        else
                            fCount++;
                    }
                    objResult.ErrorCode = Common.ErrorCode.Ok;
                    objResult.Message = sCount + " " + Common.Messages.RecordsSavedSuccessfully + " \n" + fCount + Common.Messages.FailedToSaveRecords;
                }
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BindExamFee(string sProgramme, string sBatch)
        {
            JsonResultData objResult = new JsonResultData();
            ProgrammeHeadsAmount objView = new ProgrammeHeadsAmount();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sSQL = string.Empty;
                SemesterRootInfo semester = new SemesterRootInfo();
                semester.PROGRAMME = sProgramme;
                semester.BATCH = sBatch;
                PROGRAMME_HEADS_AMOUNT programme = new PROGRAMME_HEADS_AMOUNT();
                objView.SubjectType = (List<SUP_SUBJECT_TYPE>)CMSHandler.FetchData<SUP_SUBJECT_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FecthSubjectType)).DataSource.Data;
                programme.BATCH_ID = sBatch;
                programme.PROGRAMME_ID = sProgramme;
                var programeList = (List<EXAM_PROGRAMME_WISE_HEADS>)CMSHandler.FetchData<EXAM_PROGRAMME_WISE_HEADS>(programme, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchExamFeeStructureByProgrammeIdAndBatch), sAcademicYear).DataSource.Data;
                var CourseHeadsList = (List<EXAM_COURSE_WISE_HEADS>)CMSHandler.FetchData<EXAM_COURSE_WISE_HEADS>(programme, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseHeadList), sAcademicYear).DataSource.Data;
                var AcademicCurriculum = (List<ACADEMIC_CURRICULUM_2017>)CMSHandler.FetchData<ACADEMIC_CURRICULUM_2017>(programme, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchAcademicCurriculumByProgrammeIdAndBatch), sAcademicYear).DataSource.Data;

                if (AcademicCurriculum != null && AcademicCurriculum.Count > 0)
                {
                    if (CourseHeadsList != null && CourseHeadsList.Count > 0)
                    {
                        foreach (var item in CourseHeadsList)
                        {
                            AcademicCurriculum.Where(o => o.COURSE_CODE == item.COURSE_CODE).ToList().ForEach(k => k.AMOUNT = item.AMOUNT);
                        }
                    }
                    if (programeList != null && programeList.Count > 0)
                    {
                        foreach (var item in programeList)
                        {
                            AcademicCurriculum.Where(o => string.IsNullOrEmpty(o.AMOUNT) && o.SUBJECT_TYPE_ID == item.SUBJECT_TYPE).ToList().ForEach(k => k.AMOUNT = item.AMOUNT);
                            if (item.SUBJECT_TYPE == "0")
                            {
                                AcademicCurriculum.Add(new ACADEMIC_CURRICULUM_2017() { HEAD = item.HEAD, AMOUNT = item.AMOUNT });
                            }
                        }

                    }
                    objView.academicCurriculum = AcademicCurriculum;
                    return PartialView("BindExamFee", objView);
                }
                else
                {
                    return PartialView("BindExamFee", objView);
                }

            }
            else
            {
                return Json(Common.Messages.SessionIsExpiredPleaseLoginAgain, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BindProngrammeHeads(string sProgramme, string sBatch, string sHeads)
        {
            ProgrammeHeadsAmount objView = new ProgrammeHeadsAmount();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            string sSQL = string.Empty;
            SemesterRootInfo semester = new SemesterRootInfo();
            semester.PROGRAMME = sProgramme;
            semester.BATCH = sBatch;
            PROGRAMME_HEADS_AMOUNT programme = new PROGRAMME_HEADS_AMOUNT();
            objView.SubjectType = (List<SUP_SUBJECT_TYPE>)CMSHandler.FetchData<SUP_SUBJECT_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FecthSubjectType)).DataSource.Data;
            programme.BATCH_ID = sBatch;
            programme.PROGRAMME_ID = sProgramme;
            var currentSemester = (List<SemesterRootInfo>)CMSHandler.FetchData<SemesterRootInfo>(semester, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchActiveSemesterByProgrammeAndBatch), sAcademicYear).DataSource.Data;
            programme.SEMESTER = currentSemester.FirstOrDefault().SEMESTER;
            sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchProgrammeheadsWithAmount).Replace(Common.Delimiter.QUS + Common.EXAM_PROGRAMME_WISE_HEADS.HEAD_ID, sHeads);
            objView.programme = (List<PROGRAMME_HEADS_AMOUNT>)CMSHandler.FetchData<PROGRAMME_HEADS_AMOUNT>(programme, sSQL, sAcademicYear).DataSource.Data;
            objView.sSemester = currentSemester.FirstOrDefault().SEMESTER;
            return PartialView("BindProngrammeHeads", objView);
        }

        public JsonResult LoadHeads(string sProgramme, string sBatch)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                string sOptions = string.Empty;
                EXAM_PROGRAMME_WISE_HEADS examPrograme = new EXAM_PROGRAMME_WISE_HEADS();
                examPrograme.PROGRAMME_ID = sProgramme;
                examPrograme.BATCH_ID = sBatch;
                var Heads = (List<EXAM_PROGRAMME_WISE_HEADS>)CMSHandler.FetchData<EXAM_PROGRAMME_WISE_HEADS>(examPrograme, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchProgramHeads), sAcademicYear).DataSource.Data;
                if (Heads != null)
                {
                    foreach (var item in Heads)
                    {
                        sOptions += "<option value='" + item.HEAD_ID + "' " + item.STATUS + ">" + item.HEAD + "</option>";
                    }
                    objResult.ErrorCode = Common.ErrorCode.Ok;
                    objResult.sResult = sOptions;
                    objResult.Message = Common.ErrorMessage.Ok;
                }


            }
            else
            {
                objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }

            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetProgrammeHeads()

        {
            JsonResult objResult = new JsonResult();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                ExaminationViewModel examViewModel = new ExaminationViewModel();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                var batches = (List<sup_Batch>)CMSHandler.FetchData<sup_Batch>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicBatches)).DataSource.Data;
                var programmes = (List<CP_PROGRAMME_2017>)CMSHandler.FetchData<CP_PROGRAMME_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgram), sAcademicYear).DataSource.Data;
                examViewModel.examSetting = (List<EXAM_SETTING>)CMSHandler.FetchData<EXAM_SETTING>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchExamSettingByAcitveId), sAcademicYear).DataSource.Data;
                if (batches != null)
                {
                    examViewModel.Batch = new SelectList(batches, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
                }
                if (programmes != null)
                {
                    examViewModel.Program = new SelectList(programmes, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                return View(examViewModel);
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }


        }

        [UserRights("STUDENT")]
        public async Task<ActionResult> ExamRegistration()
        {
            string sSql = string.Empty;
            int nSemester = 0;
            string sSemesters = string.Empty;
            ExaminationViewModel ExamRegistrastionViewModel = new ExaminationViewModel();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    var objExamViewModel = new List<EXAM_REGISTRATION>();
                    var ObjExamRegistration = new EXAM_REGISTRATION();
                    var liBatchAndProgramme = new List<GET_PROGRAMME_AND_BATCH>();
                    var liSemesterResult = new List<SEMESTER_RESULT>();
                    var SemesterResult = new SEMESTER_RESULT();
                    var CollegeDetails = new CollegeDetails();
                    // Fetch College Details ....
                    ExamRegistrastionViewModel.liCollegeDetails = await Task.Run(() => (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails)).DataSource.Data);
                    ObjExamRegistration.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                    List<EXAM_REGISTRATION> ObjExamRegistrationList = new List<EXAM_REGISTRATION>();
                    GET_ACTIVE_SEMESTER_BATCH objActiveSemester = new GET_ACTIVE_SEMESTER_BATCH();
                    List<GET_ACTIVE_SEMESTER_BATCH> liActiveSemester = new List<GET_ACTIVE_SEMESTER_BATCH>();
                    GET_PROGRAMME_AND_BATCH ObjProgramAndBatch = new GET_PROGRAMME_AND_BATCH();

                    // Fetch Program and Batch By Student Id .....
                    ObjProgramAndBatch.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    liBatchAndProgramme = await Task.Run(() => (List<GET_PROGRAMME_AND_BATCH>)CMSHandler.FetchData<GET_PROGRAMME_AND_BATCH>(ObjProgramAndBatch, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchProgramAndBatchByStudentID)).DataSource.Data);

                    // Fetch Active Semester By Batch Id .....
                    objActiveSemester.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                    objActiveSemester.BATCH = liBatchAndProgramme.FirstOrDefault().BATCH;
                    objActiveSemester.PROGRAMME = liBatchAndProgramme.FirstOrDefault().PROGRAMME;
                    liActiveSemester = await Task.Run(() => (List<GET_ACTIVE_SEMESTER_BATCH>)CMSHandler.FetchData<GET_ACTIVE_SEMESTER_BATCH>(objActiveSemester, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchActiveSemesterWithBatches), sAcademicYear).DataSource.Data);

                    // Get Student Information ...
                    ExamRegistrastionViewModel.liExamRegistrationModel = await Task.Run(() => (List<EXAM_REGISTRATION>)CMSHandler.FetchData<EXAM_REGISTRATION>(ObjExamRegistration, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchStudentDetailsForExamRegistration), sAcademicYear).DataSource.Data);

                    // Get All The Written Papers By Active Semester and Academic Year and Student Id .....

                    int.TryParse(liActiveSemester.Where(o => o.AC_YEAR == sAcademicYear).SingleOrDefault().SEMESTER, out nSemester);
                    sSemesters = CommonMethods.SemestersInComma(nSemester);
                    foreach (var item in liActiveSemester)
                    {
                        item.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                        int.TryParse(item.SEMESTER, out nSemester);
                        item.SEMESTER = sSemesters;
                        var tempSemesterResult = new List<SEMESTER_RESULT>();
                        sSql = string.Empty;
                        sSql = await Task.Run(() => ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchSemesterResult).Replace(Common.Delimiter.QUS + Common.SEMESTER_MARKS_2017.SEMESTER, sSemesters));
                        tempSemesterResult = await Task.Run(() => (List<SEMESTER_RESULT>)CMSHandler.FetchData<SEMESTER_RESULT>(item, sSql, item.AC_YEAR).DataSource.Data);
                        if (tempSemesterResult != null)
                        {
                            foreach (var sem in tempSemesterResult)
                            {
                                sem.AC_YEAR = item.AC_YEAR;
                                liSemesterResult.Add(sem);
                            }
                        }
                    }
                    var arrearList = new List<SEMESTER_RESULT>();
                    var objGroup = new List<SEMESTER_RESULT>();
                    var objGp = new List<SEMESTER_RESULT>();

                    // Get Only Arrear Papers group By Course Code ....
                    var result = liSemesterResult.GroupBy(u => u.COURSE_CODE).ToList();

                    for (int i = 0; i < result.Count(); i++)
                    {
                        var code = result[i].Key.ToString();
                        objGp = (List<SEMESTER_RESULT>)liSemesterResult.Where(o => o.COURSE_CODE == code).ToList();
                        if (objGp.FindAll(o => o.RESULT == "PASS").Count() == 0)
                            arrearList.Add(objGp.FirstOrDefault());
                    }
                    //ExamRegistrastionViewModel.liSemesterList = arrearList;
                    var liArearList = new List<SEMESTER_RESULT>();
                    // Get All The Regular Papers By Student_Id ...
                    ExamRegistrastionViewModel.liExamRegistrationList = await Task.Run(() => (List<EXAM_REGISTRATION>)CMSHandler.FetchData<EXAM_REGISTRATION>(objActiveSemester, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchExamRegistration), sAcademicYear).DataSource.Data);
                    // Filter Arear papers by Reguler papers 
                    foreach (var item in arrearList)
                    {
                        if (ExamRegistrastionViewModel.liExamRegistrationList.FirstOrDefault().SEMESTER_ID != item.WRITTEN_SEMESTER)
                        {
                            liArearList.Add(item);
                        }
                    }
                    ExamRegistrastionViewModel.liSemesterList = liArearList;
                    return View(ExamRegistrastionViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("ExaminationController", "ExamRegistration", "Err MSg: " + ex.Message, "Query is empty!");
                    }
                    return RedirectToAction("ErrorMessage", "Error");
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }
        }
        public JsonResult SaveExamSubjects(string ExamDetails)
        {
            string sResult = string.Empty;
            string sErrorCode = string.Empty;
            var obj = new { sResult, sErrorCode };
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                try
                {
                    ResultArgs sresultArgs = new ResultArgs();
                    string sStudentId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                    sResult = ValidateStudent();
                    if (sResult != "1")
                    {
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        string sActiveSemesterId = (Session[Common.SESSION_VARIABLES.ACTIVE_SEMESTER_FOR_STUDENT] != null) ? Session[Common.SESSION_VARIABLES.ACTIVE_SEMESTER_FOR_STUDENT].ToString() : string.Empty;
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string sLastDate = string.Empty;
                        SaveExamDetails objJson = serializer.Deserialize<SaveExamDetails>(ExamDetails);
                        //var Result=JsonConvert.DeserializeObject<SaveExamDetails>(ExamDetails);
                        var ObjSubjectDetails = new EXAM_REGISTRATION_2017();
                        var courseInfo = new List<SupCourseInfo>();
                        var tempSubjectDetails = new List<SEMESTER_RESULT>();
                        //this below statement for student credit account
                        List<FEE_STUDENT_ACCOUNT> studentAccounts = new List<FEE_STUDENT_ACCOUNT>();
                        FEE_STUDENT_ACCOUNT studentAccount;
                        StuPersonalInfo student = new StuPersonalInfo();
                        student.TYPE = Common.SupFrequencyType.ExamFee;
                        student.STUDENT_ID = sStudentId;
                        var frequency = (List<SUP_FEE_FREQUENCY>)CMSHandler.FetchData<SUP_FEE_FREQUENCY>(student, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFrequencyIdForExam), sAcademicYear).DataSource.Data;
                        var heads = (List<SUP_HEAD>)CMSHandler.FetchData<SUP_HEAD>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupHeadList), sAcademicYear).DataSource.Data;
                        var HeadsWithAmount = (List<EXAM_PROGRAMME_WISE_HEADS>)CMSHandler.FetchData<EXAM_PROGRAMME_WISE_HEADS>(student, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchExamProgrameHeadsWithAmountByStudentId), sAcademicYear).DataSource.Data;
                        var CourseHeads = (List<EXAM_COURSE_WISE_HEADS>)CMSHandler.FetchData<EXAM_COURSE_WISE_HEADS>(student, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchExamCourseHeadsByStudentId), sAcademicYear).DataSource.Data;
                        foreach (var item in objJson.SubjectDetails)
                        {
                            courseInfo = (List<SupCourseInfo>)CMSHandler.FetchData<SupCourseInfo>(item, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCourseInfoByCourseCode), sAcademicYear).DataSource.Data;
                            if (courseInfo != null && courseInfo.Count > 0)
                            {
                                item.COURSE_ROOT_ID = courseInfo.FirstOrDefault().COURSE_ROOT_ID;
                                item.AC_YEAR = sAcademicYear;
                            }
                            else
                            {
                                tempSubjectDetails.Add(item);
                            }
                        }
                        //setting default amount to course by course code
                        if (CourseHeads != null)
                        {
                            foreach (var item in CourseHeads)
                            {
                                objJson.SubjectDetails.Where(O => O.COURSE_CODE == item.COURSE_CODE).ToList().ForEach(k => k.AMOUNT = item.AMOUNT);
                            }
                        }
                        if (frequency != null && frequency.Count > 0)
                        {
                            sLastDate = frequency.FirstOrDefault().LAST_DATE_OF_PAYMENT;
                            if (!string.IsNullOrEmpty(sLastDate))
                            {
                                //setting amount by subject type
                                if (HeadsWithAmount != null && HeadsWithAmount.Count() > 0)
                                {
                                    foreach (var item in HeadsWithAmount)
                                    {
                                        objJson.SubjectDetails.Where(o => string.IsNullOrEmpty(o.AMOUNT) && o.SUBJECT_TYPE_ID == item.SUBJECT_TYPE).ToList().ForEach(k => k.AMOUNT = item.AMOUNT);
                                    }
                                    DateTime dtTemp = DateTime.ParseExact(sLastDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);

                                    if (DateTime.Compare(dtTemp, DateTime.Now.Date) >= 0)
                                    {
                                        string FineHeadId = (heads.Exists(t => t.HEAD.ToLower().Equals("fine") || t.HEAD.ToLower().Equals("examination fine"))) ? heads.Where(o => o.HEAD.ToLower().Equals("fine") || o.HEAD.ToLower().Equals("examination fine")).FirstOrDefault().HEAD_ID : "";
                                        HeadsWithAmount.RemoveAll(o => o.HEAD_ID == FineHeadId);
                                    }
                                    if (objJson.SubjectDetails.Where(o => string.IsNullOrEmpty(o.AMOUNT) || o.AMOUNT == "0").Count() == 0)
                                    {
                                        if (tempSubjectDetails == null || tempSubjectDetails.Count == 0)
                                        {
                                            foreach (var item in objJson.SubjectDetails)
                                            {
                                                ObjSubjectDetails.STUDENT_ID = sStudentId;
                                                ObjSubjectDetails.COURSE_ID = item.COURSE_ROOT_ID;
                                                ObjSubjectDetails.STATUS = item.STATUS;
                                                ObjSubjectDetails.ACADEMIC_YEAR = item.AC_YEAR;
                                                ObjSubjectDetails.EXAM_SEMESTER = item.SEMESTER;
                                                ObjSubjectDetails.EXAM_SETTING_ID = item.EXAM_SETTING_ID;
                                                sresultArgs = CMSHandler.SaveOrUpdate(ObjSubjectDetails, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.SaveExamDetails), sAcademicYear);
                                            }
                                            if (sresultArgs.Success)
                                            {
                                                studentAccount = new FEE_STUDENT_ACCOUNT();
                                                var arrearList = objJson.SubjectDetails.Where(o => o.STATUS != "R");
                                                var regularList = objJson.SubjectDetails.Where(o => o.STATUS == "R");
                                                //need to add fine head .
                                                //this is to sum only arrears amount                        
                                                if (arrearList != null && arrearList.Count() > 0)
                                                {
                                                    studentAccount = new FEE_STUDENT_ACCOUNT();
                                                    studentAccount.HEAD = (heads.Exists(t => t.HEAD.ToLower().Equals("arrears") || t.HEAD.ToLower().Equals("arrear"))) ? heads.Where(o => o.HEAD.ToLower().Equals("arrears") || o.HEAD.ToLower().Equals("arrear")).FirstOrDefault().HEAD_ID : "0";
                                                    studentAccount.CREDIT = arrearList.Sum(o => Convert.ToInt32(o.AMOUNT)).ToString();
                                                    studentAccount.ACADEMIC_YEAR = sAcademicYear;
                                                    studentAccount.FREQUENCY_ID = frequency.FirstOrDefault().FREQUENCY_ID;
                                                    studentAccount.STUDENT_ID = arrearList.FirstOrDefault().STUDENT_ID;
                                                    studentAccounts.Add(studentAccount);
                                                }
                                                if (regularList != null && regularList.Count() > 0)
                                                {
                                                    //This is for extra credit
                                                    string sExtraCredit = string.Empty;
                                                    sExtraCredit = (heads.Exists(t => t.HEAD.ToLower().Equals("extra credit"))) ? heads.Where(o => o.HEAD.ToLower().Equals("extra credit")).FirstOrDefault().HEAD_ID : string.Empty;
                                                    if (!string.IsNullOrEmpty(sExtraCredit) && sExtraCredit != "0")
                                                    {
                                                        if (HeadsWithAmount.Where(o => o.HEAD_ID == sExtraCredit).Count() > 0)
                                                        {
                                                            var tempExtraCredit = regularList.Where(o => o.SUBJECT_TYPE_ID == HeadsWithAmount.Where(k => k.HEAD_ID == sExtraCredit).FirstOrDefault().SUBJECT_TYPE).ToList();
                                                            if (tempExtraCredit != null && tempExtraCredit.Count > 0)
                                                            {
                                                                studentAccount = new FEE_STUDENT_ACCOUNT();
                                                                studentAccount.HEAD = sExtraCredit;
                                                                studentAccount.CREDIT = tempExtraCredit.Sum(o => Convert.ToInt32(o.AMOUNT)).ToString();
                                                                studentAccount.ACADEMIC_YEAR = sAcademicYear;
                                                                studentAccount.FREQUENCY_ID = frequency.FirstOrDefault().FREQUENCY_ID;
                                                                studentAccount.STUDENT_ID = regularList.FirstOrDefault().STUDENT_ID;
                                                                studentAccounts.Add(studentAccount);
                                                                string headid = HeadsWithAmount.Where(k => k.HEAD_ID == sExtraCredit).FirstOrDefault().SUBJECT_TYPE;
                                                                regularList = regularList.Where(o => o.SUBJECT_TYPE_ID != headid);

                                                            }
                                                        }
                                                    }

                                                    //this is to sum only regular amount
                                                    studentAccount = new FEE_STUDENT_ACCOUNT();
                                                    studentAccount.HEAD = (heads.Exists(t => t.HEAD.ToLower().Equals("examination fees") || t.HEAD.ToLower().Equals("examination fee"))) ? heads.Where(o => o.HEAD.ToLower().Equals("examination fees") || o.HEAD.ToLower().Equals("examination fee")).FirstOrDefault().HEAD_ID : "0";
                                                    studentAccount.CREDIT = regularList.Sum(o => Convert.ToInt32(o.AMOUNT)).ToString();
                                                    studentAccount.ACADEMIC_YEAR = sAcademicYear;
                                                    studentAccount.FREQUENCY_ID = frequency.FirstOrDefault().FREQUENCY_ID;
                                                    studentAccount.STUDENT_ID = regularList.FirstOrDefault().STUDENT_ID;
                                                    studentAccounts.Add(studentAccount);

                                                    foreach (var item in HeadsWithAmount)
                                                    {
                                                        if (item.SUBJECT_TYPE == "0")
                                                        {
                                                            studentAccount = new FEE_STUDENT_ACCOUNT();
                                                            studentAccount.HEAD = item.HEAD_ID;
                                                            studentAccount.CREDIT = item.AMOUNT;
                                                            studentAccount.ACADEMIC_YEAR = sAcademicYear;
                                                            studentAccount.FREQUENCY_ID = frequency.FirstOrDefault().FREQUENCY_ID;
                                                            studentAccount.STUDENT_ID = regularList.FirstOrDefault().STUDENT_ID;
                                                            studentAccounts.Add(studentAccount);
                                                        }
                                                    }
                                                    if (studentAccounts != null && studentAccounts.Count() > 0)
                                                    {
                                                        foreach (var item in studentAccounts)
                                                        {
                                                            sresultArgs = CMSHandler.SaveOrUpdate(item, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.SaveExamCreditAmountToStudentAccount), sAcademicYear);
                                                        }

                                                        if (sresultArgs.Success)
                                                        {
                                                            sResult = Common.Messages.RecordsSavedSuccessfully;
                                                        }
                                                        else
                                                        {//need to use trasaction's revart method 
                                                            sErrorCode = "2";
                                                            sResult = Common.Messages.FailedToSaveRecords;
                                                        }
                                                    }
                                                    else
                                                    {//need to use trasaction's revart method 
                                                        sresultArgs.Success = false;
                                                        sErrorCode = "2";
                                                        sResult = Common.Messages.FailedToSaveRecords;
                                                        obj = new { sResult, sErrorCode };
                                                    }
                                                }
                                                else
                                                {
                                                    sresultArgs.Success = false;
                                                    sErrorCode = "2";
                                                    sResult = Common.Messages.CoursesAreNotAlloted;
                                                    obj = new { sResult, sErrorCode };
                                                }
                                            }
                                            else
                                            {
                                                sErrorCode = "2";
                                                sResult = Common.Messages.FailedToSaveRecords;
                                            }
                                        }
                                        else
                                        {
                                            sresultArgs.Success = false;
                                            sErrorCode = "2";
                                            sResult = Common.Messages.MissingCoursePleaseMeetAdmin;
                                            obj = new { sResult, sErrorCode };
                                            return Json(obj);
                                        }
                                    }
                                    else
                                    {

                                        sresultArgs.Success = false;
                                        sErrorCode = "2";
                                        sResult = Common.Messages.LedgersAreNotMapped;
                                        obj = new { sResult, sErrorCode };
                                    }
                                }
                                else
                                {
                                    sresultArgs.Success = false;
                                    sErrorCode = "2";
                                    sResult = Common.Messages.LedgersAreNotMapped;
                                    obj = new { sResult, sErrorCode };
                                }
                            }
                            else
                            {
                                sresultArgs.Success = false;
                                sErrorCode = "2";
                                sResult = "Missing Last Date !,Please Meet Admin";
                                obj = new { sResult, sErrorCode };
                            }
                        }
                        else
                        {
                            sresultArgs.Success = false;
                            sErrorCode = "2";
                            sResult = "Exam Frequency is Required !,Please Meet Admin";
                            obj = new { sResult, sErrorCode };
                        }

                    }
                    else
                    {
                        sErrorCode = sResult;
                        sResult = " You have Registered Already ...!";
                    }
                    obj = new { sResult, sErrorCode };
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("ExaminationController", "SaveExamSubjects", "Err MSg: " + ex.Message, "Query is empty!");
                    }
                    sErrorCode = "2";
                    sResult = Common.Messages.FailedToSaveRecords;
                    obj = new { sResult, sErrorCode };
                }
            }
            else
            {
                sErrorCode = "2";
                sResult = "Session out please login again!!";
                obj = new { sResult, sErrorCode };
            }

            return Json(obj);
        }

        public string ValidateStudent()
        {
            string sResult = string.Empty;
            if ((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null))
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                ResultArgs resultArgs = new ResultArgs();
                var ObjValidate = new GET_ACADEMIC_YEAR_BY_ID();
                var liValidate = new List<GET_ACADEMIC_YEAR_BY_ID>();
                ObjValidate.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                liValidate = (List<GET_ACADEMIC_YEAR_BY_ID>)CMSHandler.FetchData<GET_ACADEMIC_YEAR_BY_ID>(ObjValidate, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.ValidateStudentForExamRegistration), sAcademicYear).DataSource.Data;
                if (liValidate != null && liValidate.Count > 0)
                {
                    sResult = liValidate.FirstOrDefault().RESULT;
                }
                else
                {
                    sResult = "0";
                }
            }
            else
            {
                sResult = "404";
            }

            return sResult;
        }

        public JsonResult BindDDLBatch(string sProgramId)
        {
            string sOption = string.Empty;
            try
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                //Batch ...
                DataTable dtBatch = new DataTable();
                dv.Clear();
                dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.PROGRAMME, sProgramId, EnumCommand.DataType.String);
                dtBatch = SupportDataMethod.FetchBatchByProgramId(dv, sAcademicYear).DataSource.Table;
                List<sup_Batch> liBatch = new List<sup_Batch>();
                if (dtBatch != null && dtBatch.Rows.Count > 0)
                {
                    foreach (DataRow item in dtBatch.Rows)
                    {
                        sOption += "<option value='" + item[Common.SUP_BATCHES.BATCH_ID].ToString() + "' >" + item[Common.SUP_BATCHES.BATCH].ToString() + "</option>";
                    }
                }
                return Json(sOption);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ExaminationController", "BindDDLBatch", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ExaminationController", "BindDDLBatch", ex.Message);
                }
                return Json(sOption);
            }
        }

        public ActionResult PrintExamRegistrationForm()
        {
            return View();
        }
        #endregion

        #region Hall Ticket
        [UserRights("STUDENT")]
        public ActionResult HallTicket()
        {
            return View();
        }
        public async Task<ActionResult> ExamRegistrationPriview()
        {
            var sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var objActiveSemester = new GET_ACTIVE_SEMESTER_BATCH();
            var ExamRegistrastionViewModel = new ExaminationViewModel();
            try
            {
                objActiveSemester.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                // Get ExamRegistrationPriview ...
                ExamRegistrastionViewModel.liFetchHallTicket = await Task.Run(() => (List<FetchHallTicket>)CMSHandler.FetchData<FetchHallTicket>(objActiveSemester, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchHallTicketList), sAcademicYear).DataSource.Data);
                // Get All Student Information ...
                ExamRegistrastionViewModel.liExamRegistrationModel = (List<EXAM_REGISTRATION>)CMSHandler.FetchData<EXAM_REGISTRATION>(objActiveSemester, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchStudentDetailsForExamRegistration), sAcademicYear).DataSource.Data;
                // Get College Infomation ...
                ExamRegistrastionViewModel.liCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails)).DataSource.Data;
                ExamRegistrastionViewModel.feeTransactions = (List<FEE_STUDENT_ACCOUNT>)CMSHandler.FetchData<FEE_STUDENT_ACCOUNT>(new FEE_TRANSACTION() { ACADEMIC_YEAR = sAcademicYear, STUDENT_ID = objActiveSemester.STUDENT_ID }, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchExamFeeStatus), sAcademicYear).DataSource.Data;

                return View(ExamRegistrastionViewModel);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ExaminationController", "ExamRegistrationPriview", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ExaminationController", "ExamRegistrationPriview", ex.Message);
                }
                return View(ExamRegistrastionViewModel);
            }

        }
        public async Task<ActionResult> LoadSubjectsForHallTicket()
        {
            ExaminationViewModel ExamRegistrastionViewModel = new ExaminationViewModel();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                ExamRegistrastionViewModel.sessionFlag = true;
                try
                {
                    string sStudentId = string.Empty;
                    List<EXAM_REGISTRATION> ObjExamRegistrationList = new List<EXAM_REGISTRATION>();
                    GET_ACTIVE_SEMESTER_BATCH objActiveSemester = new GET_ACTIVE_SEMESTER_BATCH();
                    List<GET_ACTIVE_SEMESTER_BATCH> liActiveSemester = new List<GET_ACTIVE_SEMESTER_BATCH>();
                    GET_PROGRAMME_AND_BATCH ObjProgramAndBatch = new GET_PROGRAMME_AND_BATCH();
                    var liBatchAndProgramme = new List<GET_PROGRAMME_AND_BATCH>();
                    sStudentId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                    objActiveSemester.STUDENT_ID = sStudentId;
                    // Fetch Program and Batch By Student Id .....
                    ObjProgramAndBatch.STUDENT_ID = sStudentId;
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    liBatchAndProgramme = (List<GET_PROGRAMME_AND_BATCH>)CMSHandler.FetchData<GET_PROGRAMME_AND_BATCH>(ObjProgramAndBatch, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchProgramAndBatchByStudentID)).DataSource.Data;
                    // Fetch Active Semester By Batch Id .....
                    objActiveSemester.BATCH = liBatchAndProgramme.FirstOrDefault().BATCH;
                    objActiveSemester.PROGRAMME = liBatchAndProgramme.FirstOrDefault().PROGRAMME;
                    liActiveSemester = (List<GET_ACTIVE_SEMESTER_BATCH>)CMSHandler.FetchData<GET_ACTIVE_SEMESTER_BATCH>(objActiveSemester, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchActiveSemesterWithBatches), sAcademicYear).DataSource.Data;
                    var ActiveSemesterItem = liActiveSemester.Where(o => o.AC_YEAR == sAcademicYear).SingleOrDefault();
                    ActiveSemesterItem.STUDENT_ID = sStudentId;
                    List<FetchHallTicket> liHallTickets = new List<FetchHallTicket>();
                    var objExamRegistraion = await Task.Run(() => (List<FetchHallTicket>)CMSHandler.FetchData<FetchHallTicket>(ActiveSemesterItem, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchHallTicketList), sAcademicYear).DataSource.Data);
                    foreach (var item in objExamRegistraion)
                    {
                        var tempCourse = ((List<FetchHallTicket>)CMSHandler.FetchData<FetchHallTicket>(item, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseInfoByAcademicYear), item.ACADEMIC_YEAR).DataSource.Data).SingleOrDefault();
                        tempCourse.AC_YEAR = item.ACADEMIC_YEAR;
                        tempCourse.STATUS = item.STATUS;
                        liHallTickets.Add(tempCourse);
                    }
                    //------
                    //var liExamRegistration = new FetchHallTicket();
                    ExamRegistrastionViewModel.liFetchHallTicket = liHallTickets;
                    objActiveSemester.STUDENT_ID = sStudentId;
                    ExamRegistrastionViewModel.sSemester = objExamRegistraion.FirstOrDefault().SEMESTER_ID;
                    ExamRegistrastionViewModel.Month_Year = objExamRegistraion.FirstOrDefault().MONTH_YEAR;
                    ExamRegistrastionViewModel.liRegistration = (List<HallTicketDetails>)CMSHandler.FetchData<HallTicketDetails>(objActiveSemester, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchHallTicket), sAcademicYear).DataSource.Data;
                    return View(ExamRegistrastionViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("ExaminationController", "LoadSubjectsForHallTicket", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("ExaminationController", "LoadSubjectsForHallTicket", ex.Message);
                    }
                    return View(ExamRegistrastionViewModel);
                }
            }
            else
            {
                return View(ExamRegistrastionViewModel);
            }
        }
        #endregion

        #region Examination Contoller Print
        public ActionResult ExamControllerView()
        {
            ExaminationViewModel ObjExaminationViewModel = new ExaminationViewModel();
            try
            {
                string sAcademicYear = string.Empty;

                sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;


                List<Sup_Shift> liShift = new List<Sup_Shift>();
                liShift = (List<Sup_Shift>)CMS.DAO.CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                ObjExaminationViewModel.Shift = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                // Program ...
                DataTable dtProgram = new DataTable();
                dtProgram = SupportDataMethod.FetchProgram(sAcademicYear).DataSource.Table;
                List<cp_Program_2017> liProgram = new List<cp_Program_2017>();
                if (dtProgram != null && dtProgram.Rows.Count > 0)
                {
                    liProgram = (from DataRow dr in dtProgram.Rows
                                 select new cp_Program_2017()
                                 {
                                     PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                     PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                                 }).ToList();
                    ObjExaminationViewModel.Program = new SelectList(liProgram, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }
                // Class ...
                DataTable dtClass = new DataTable();

                dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
                List<cp_Classes_2017> liClass = new List<cp_Classes_2017>();
                if (dtClass != null && dtClass.Rows.Count > 0)
                {
                    liClass = (from DataRow dr in dtClass.Rows
                               select new cp_Classes_2017()
                               {
                                   CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                                   CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                               }).ToList();
                    ObjExaminationViewModel.ClassList = new SelectList(liClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objErrorLog = new ErrorLog())
                {
                    objErrorLog.WriteError("ExaminationController", "ExamControllerView", "Err.Msg:" + ex.Message, "Qury is Empty !");
                    objErrorLog.WriteError("ExaminationController", "ExamControllerView", "Err.Msg:" + ex.Message);
                }
            }
            return View(ObjExaminationViewModel);
        }
        public ActionResult ExaminationControllerPrint(string sClassId)
        {
            ExamControllerPrint objExamContollerPrint = new ExamControllerPrint();
            string sSQL = string.Empty;
            IList<ExamControllerPrint> liExaminationControllerPrint = new List<ExamControllerPrint>();
            try
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                objExamContollerPrint.CLASS_ID = sClassId;
                // liExaminationControllerPrint = (List<ExamControllerPrint>)CMSHandler.FetchData<ExamControllerPrint>(objExamContollerPrint,ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.ExamControllerPrint), sAcademicYear).DataSource.Data;
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.ExamControllerPrint).Replace("?" + Common.CP_CLASSES_2017.CLASS_ID, sClassId);
                liExaminationControllerPrint = (List<ExamControllerPrint>)CMSHandler.FetchData<ExamControllerPrint>(objExamContollerPrint, sSQL, sAcademicYear).DataSource.Data;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ExaminationController", "ExaminationControllerPrint", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ExaminationController", "ExaminationControllerPrint", ex.Message);
                }
            }
            return View(liExaminationControllerPrint);
        }

        #endregion

        #region Exam Registration
        public async Task<ActionResult> ExamRegisteredList()
        {
            ExaminationViewModel objViewModel = new ExaminationViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            //var NMESettings = new EXAM_REGISTRATION_2017();
            var ListExamRegistration = await Task.Run(() => (List<EXAM_REGISTRATION_2017>)CMSHandler.FetchData<EXAM_REGISTRATION_2017>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchExamRegistrationList), sAcademicYear).DataSource.Data);
            objViewModel.liExamRegisteredList = ListExamRegistration;
            return View(objViewModel);
        }
        #endregion

        #region Online - Quiz
        public ActionResult OnlineQuiz()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            //CourseList ObjCourse = new CourseList();
            ClassList ObjClass = new ClassList();
            List<ClassList> liClass = new List<ClassList>();
            ObjClass.STAFF_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            using (ExaminationViewModel ObjViewModel = new ExaminationViewModel())
            {
                liClass = (List<ClassList>)CMSHandler.FetchData<ClassList>(ObjClass, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchClassByStaffId), sAcademicYear).DataSource.Data;
                ObjViewModel.ClassList = new SelectList(liClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);

                List<CourseList> liCourse = new List<CourseList>();
                liCourse.Add(new CourseList() { COURSE_TITLE = "---Select---", COURSE_ROOT_ID = "0" });
                ObjViewModel.CourseList = new SelectList(liCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);

                return View(ObjViewModel);
            }
        }

        public JsonResult FethCourseByClassId(string Class_Id)
        {
            string sOption = string.Empty;
            ClassList ObjClass = new ClassList();
            List<CourseList> liCourse = new List<CourseList>();
            ObjClass.CLASS_ID = Class_Id;
            ObjClass.STAFF_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

            liCourse = (List<CourseList>)CMSHandler.FetchData<CourseList>(ObjClass, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseByStaffId), sAcademicYear).DataSource.Data;
            foreach (var item in liCourse)
            {
                sOption += "<option value='" + item.COURSE_ROOT_ID + "'>" + item.COURSE_TITLE + "</option>";
            }
            return Json(sOption);
        }

        public async Task<JsonResult> SaveOnlineQuiz(string QuizQuestion, string QuizOption)
        {
            string sResult = string.Empty;
            string QuestionId = string.Empty;

            try
            {
                QUIZ_OPTIONS_2017 ObjOption = new QUIZ_OPTIONS_2017();
                List<QUIZ_OPTIONS_2017> liOption = new List<QUIZ_OPTIONS_2017>();
                ResultArgs sresultArgs = new ResultArgs();
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

                var Result = JsonConvert.DeserializeObject<QUIZ_QUESTIONS_2017>(QuizQuestion);
                Result.STAFF_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                string sSql = string.Empty;
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JSON_QUIZ_OPTIONS objJson = serializer.Deserialize<JSON_QUIZ_OPTIONS>(QuizOption);
                Result.QUESTION = HttpUtility.UrlEncode(Result.QUESTION);
                sresultArgs = await Task.Run(() => CMSHandler.SaveOrUpdate(Result, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.SaveQuizQuestion), sAcademicYear, true));
                if (sresultArgs.Success)
                {
                    QuestionId = sresultArgs.RowUniqueId.ToString();
                    ObjOption.QUESTION_ID = QuestionId;
                    Result.QUESTION_ID = QuestionId;
                    foreach (var item in objJson.liQuizOption)
                    {
                        ObjOption.OPTIONS = HttpUtility.UrlEncode(item.OPTIONS);
                        sresultArgs = await Task.Run(() => CMSHandler.SaveOrUpdate(ObjOption, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.SaveQuizOption), sAcademicYear, true));
                    }
                    Result.ANSWER = HttpUtility.UrlEncode(Result.ANSWER);
                    liOption = (List<QUIZ_OPTIONS_2017>)CMSHandler.FetchData<QUIZ_OPTIONS_2017>(Result, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchOptionIdByOption), sAcademicYear).DataSource.Data;
                    Result.ANSWER = liOption.FirstOrDefault().OPTION_ID;
                    sresultArgs = await Task.Run(() => CMSHandler.SaveOrUpdate(Result, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.UpdateQuizQuestion), sAcademicYear));
                    sResult = Common.Messages.RecordsSavedSuccessfully;
                }
                else
                {
                    sResult = Common.Messages.FailedToSaveRecords;
                }

                return Json(sResult);

            }
            catch (Exception ex)
            {
                using (ErrorLog objErrorLog = new ErrorLog())
                {
                    objErrorLog.WriteError("ExaminationController", "SaveOnlineQuiz", "Err.Msg:" + ex.Message, "Qury is Empty !");
                    objErrorLog.WriteError("ExaminationController", "SaveOnlineQuiz", "Err.Msg:" + ex.Message);
                }
                return Json(ex.Message);
            }
            // Obj = new { sResult, QuestionId };

        }

        public ActionResult ListQuiz()
        {
            return View();
        }

        public async Task<ActionResult> ListQuizPartial()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            List<LIST_QUIZ> liQuizList = new List<LIST_QUIZ>();
            IList<LIST_QUIZ> liQuiz = new List<LIST_QUIZ>();
            QUIZ_QUESTIONS_2017 ObjQuizQuestion = new QUIZ_QUESTIONS_2017();
            ObjQuizQuestion.STAFF_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            liQuizList = await Task.Run(() => (List<LIST_QUIZ>)CMSHandler.FetchData<LIST_QUIZ>(ObjQuizQuestion, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.ListQuiz), sAcademicYear).DataSource.Data);
            if (liQuizList != null)
            {
                foreach (var item in liQuizList)
                {
                    item.QUESTION = HttpUtility.UrlDecode(item.QUESTION);
                    item.ANSWER = HttpUtility.UrlDecode(item.ANSWER);
                    liQuiz.Add(item);
                }
            }

            return View(liQuiz);
        }

        public async Task<ActionResult> EditQuizQuestion(string Id)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            QUIZ_QUESTIONS_2017 ObjQuizQuestion = new QUIZ_QUESTIONS_2017();
            var liQuestion = new List<QUIZ_QUESTIONS_2017>();
            var liOptions = new List<QUIZ_OPTIONS_2017>();
            ObjQuizQuestion.QUESTION_ID = Id;
            ClassList ObjClass = new ClassList();
            List<ClassList> liClass = new List<ClassList>();
            ObjClass.STAFF_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            using (ExaminationViewModel ObjViewModel = new ExaminationViewModel())
            {
                ObjViewModel.liQuizQuestion = await Task.Run(() => (List<QUIZ_QUESTIONS_2017>)CMSHandler.FetchData<QUIZ_QUESTIONS_2017>(ObjQuizQuestion, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchQuizByQuestionId), sAcademicYear).DataSource.Data);
                ObjViewModel.liQuizOption = await Task.Run(() => (List<QUIZ_OPTIONS_2017>)CMSHandler.FetchData<QUIZ_OPTIONS_2017>(ObjQuizQuestion, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchOptionIdByQuestionId), sAcademicYear).DataSource.Data);
                liClass = (List<ClassList>)CMSHandler.FetchData<ClassList>(ObjClass, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchClassByStaffId), sAcademicYear).DataSource.Data;
                ObjViewModel.ClassList = new SelectList(liClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                ObjClass.CLASS_ID = ObjViewModel.liQuizQuestion.FirstOrDefault().CLASS_ID;
                ObjClass.STAFF_ID = ObjViewModel.liQuizQuestion.FirstOrDefault().STAFF_ID;
                ObjViewModel.liCourse = (List<CourseList>)CMSHandler.FetchData<CourseList>(ObjClass, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseByStaffId), sAcademicYear).DataSource.Data;
                ObjViewModel.CourseList = new SelectList(ObjViewModel.liCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);

                foreach (var item in ObjViewModel.liQuizQuestion)
                {
                    item.QUESTION = HttpUtility.UrlDecode(item.QUESTION);
                    liQuestion.Add(item);
                }
                ObjViewModel.liQuizQuestion = liQuestion;
                foreach (var item in ObjViewModel.liQuizOption)
                {
                    item.OPTIONS = HttpUtility.UrlDecode(item.OPTIONS);
                    liOptions.Add(item);
                }
                ObjViewModel.liQuizOption = liOptions;
                return View(ObjViewModel);
            }
        }

        public async Task<JsonResult> UpdateQuizQuestion(string QuizQuestion, string QuizOption, string OptionId)
        {
            string sResult = string.Empty;
            var Option_Id = OptionId.Split(',');
            var count = Option_Id.Count() - 1;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            try
            {
                ResultArgs sresultArgs = new ResultArgs();
                var Result = JsonConvert.DeserializeObject<QUIZ_QUESTIONS_2017>(QuizQuestion);
                Result.STAFF_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                var liOptions = new List<QUIZ_OPTIONS_2017>();
                QUIZ_OPTIONS_2017 ObjOption = new QUIZ_OPTIONS_2017();
                int i = 0;
                ObjOption.QUESTION_ID = Result.QUESTION_ID;
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JSON_QUIZ_OPTIONS objJson = serializer.Deserialize<JSON_QUIZ_OPTIONS>(QuizOption);
                foreach (var item in objJson.liQuizOption)
                {
                    ObjOption.OPTIONS = HttpUtility.UrlEncode(item.OPTIONS);
                    ObjOption.OPTION_ID = Option_Id[i];
                    sresultArgs = await Task.Run(() => CMSHandler.SaveOrUpdate(ObjOption, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.UpdateQuizOptions), sAcademicYear));
                    i++;
                }
                Result.ANSWER = HttpUtility.UrlEncode(Result.ANSWER);
                Result.QUESTION = HttpUtility.UrlEncode(Result.QUESTION);
                liOptions = (List<QUIZ_OPTIONS_2017>)CMSHandler.FetchData<QUIZ_OPTIONS_2017>(Result, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchOptionIdByOption), sAcademicYear).DataSource.Data;
                Result.ANSWER = liOptions.FirstOrDefault().OPTION_ID;

                sresultArgs = await Task.Run(() => CMSHandler.SaveOrUpdate(Result, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.UpdateQuizQuestion), sAcademicYear));
                if (sresultArgs.Success)
                    sResult = "Record Updated Successfully ...!";
                else
                    sResult = Common.Messages.FailedToSaveRecords;
                return Json(sResult);
            }
            catch (Exception ex)
            {
                using (ErrorLog objErrorLog = new ErrorLog())
                {
                    objErrorLog.WriteError("ExaminationController", "SaveOnlineQuiz", "Err.Msg:" + ex.Message, "Qury is Empty !");
                    objErrorLog.WriteError("ExaminationController", "SaveOnlineQuiz", "Err.Msg:" + ex.Message);
                    return Json(ex.Message);
                }
            }

        }

        public string DeleteQuizQuestion(string QuestionId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            ResultArgs sresultArgs = new ResultArgs();
            string sResult = string.Empty;
            QUIZ_QUESTIONS_2017 ObjQuizQuestions = new QUIZ_QUESTIONS_2017();
            ObjQuizQuestions.QUESTION_ID = QuestionId;
            sresultArgs = CMSHandler.SaveOrUpdate(ObjQuizQuestions, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.DeleteQuiz), sAcademicYear);
            if (sresultArgs.Success)
                sResult = Common.Messages.RecordDeletedSuccessfully;
            else
                sResult = Common.Messages.FailedToDeletedTryAgain;
            return sResult;
        }

        public async Task<ActionResult> StuOnlineQuiz()
        {
            string sAademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            using (ExaminationViewModel ObjViewModel = new ExaminationViewModel())
            {
                List<CourseList> liCourse = new List<CourseList>();

                CourseList ObjCourseList = new CourseList();
                ObjCourseList.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;

                ObjViewModel.liCourse = await Task.Run(() => (List<CourseList>)CMSHandler.FetchData<CourseList>(ObjCourseList, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseForQuiz), sAademicYear).DataSource.Data);
                if (ObjViewModel.liCourse == null)
                {
                    liCourse.Add(new CourseList() { COURSE_TITLE = "---Select---", COURSE_ROOT_ID = "0" });
                    ObjViewModel.CourseList = new SelectList(liCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                }
                else
                {
                    ObjViewModel.CourseList = new SelectList(ObjViewModel.liCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
                }

                return View(ObjViewModel);
            }

        }

        public async Task<ActionResult> StuQuestionAnswer(string sCourseId)
        {
            ExaminationViewModel ObjViewModel = new ExaminationViewModel();
            string sResult = string.Empty;
            sResult = await QuizValidate(sCourseId);
            if (sResult != "1")
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                var liQuestion = new List<QUIZ_QUESTIONS_2017>();
                var liOptions = new List<QUIZ_OPTIONS_2017>();
                QUIZ_QUESTIONS_2017 ObjQuizQestion = new QUIZ_QUESTIONS_2017();
                List<ClassList> liClass = new List<ClassList>();
                ObjQuizQestion.COURSE_ID = sCourseId;
                ObjQuizQestion.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                liClass = (List<ClassList>)CMSHandler.FetchData<ClassList>(ObjQuizQestion, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchClassByStudentId)).DataSource.Data;
                ObjQuizQestion.CLASS_ID = liClass.FirstOrDefault().CLASS_ID;
                ObjViewModel.liQuizQuestion = await Task.Run(() => (List<QUIZ_QUESTIONS_2017>)CMSHandler.FetchData<QUIZ_QUESTIONS_2017>(ObjQuizQestion, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchQuizQuestionByCourseId), sAcademicYear).DataSource.Data);
                ObjViewModel.liQuizOption = await Task.Run(() => (List<QUIZ_OPTIONS_2017>)CMSHandler.FetchData<QUIZ_OPTIONS_2017>(ObjQuizQestion, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchQuizOptionByCourseId), sAcademicYear).DataSource.Data);
                foreach (var item in ObjViewModel.liQuizQuestion)
                {
                    item.QUESTION = HttpUtility.UrlDecode(item.QUESTION);
                    item.ANSWER = HttpUtility.UrlDecode(item.ANSWER);
                    liQuestion.Add(item);
                }
                ObjViewModel.liQuizQuestion = liQuestion;
                foreach (var item in ObjViewModel.liQuizOption)
                {
                    item.OPTIONS = HttpUtility.UrlDecode(item.OPTIONS);
                    liOptions.Add(item);
                }
                ObjViewModel.liQuizOption = liOptions;
                return View(ObjViewModel);
            }
            else
            {
                return Json(sResult);
            }

        }

        public async Task<JsonResult> SaveQuestionAnswer(string QuizAnswer)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sStudentId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            string sInsertQuery = string.Empty;
            string sUpdateQuery = string.Empty;
            //string sInsertQueryCom = string.Empty;
            string sResult = string.Empty;
            try
            {
                ResultArgs sresultArgs = new ResultArgs();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JSON_SAVE_QUIZ objJson = serializer.Deserialize<JSON_SAVE_QUIZ>(QuizAnswer);
                sInsertQuery = @"INSERT INTO QUIZ_?AC_YEAR(STUDENT_ID,CLASS_ID,QUESTION_ID,SELECTED_ANSWER,DATE)VALUES";
                //sInsertQueryCom = sInsertQuery;
                foreach (var item in objJson.ObjSaveQuizAnswer)
                {
                    item.DATE = CommonMethods.ConvertdatetoyearFromat(item.DATE);
                    item.SELECTED_ANSWER = HttpUtility.UrlDecode(item.SELECTED_ANSWER);
                    sInsertQuery += "('" + sStudentId + "','" + item.CLASS_ID + "','" + item.QUESTION_ID + "','" + item.SELECTED_ANSWER + "','" + item.DATE + "'),";
                }

                using (ExaminationViewModel objExamination = new ExaminationViewModel())
                {
                    sInsertQuery = sInsertQuery.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                    sInsertQuery = sInsertQuery.TrimEnd(',');
                    sInsertQuery = sInsertQuery + ";";
                    sresultArgs = await Task.Run(() => objExamination.ExcuteCIA_Marks(sInsertQuery));
                    if (sresultArgs.Success)
                    {
                        sResult = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sResult = Common.Messages.FailedToSaveRecords;
                    }

                }
                return Json(sResult);
            }
            catch (Exception ex)
            {
                using (ErrorLog objErrorLog = new ErrorLog())
                {
                    objErrorLog.WriteError("ExaminationController", "SaveOnlineQuiz", "Err.Msg:" + ex.Message, "Qury is Empty !");
                    objErrorLog.WriteError("ExaminationController", "SaveOnlineQuiz", "Err.Msg:" + ex.Message);
                }
                return Json(ex.Message);
            }

        }

        public async Task<string> QuizValidate(string CourseId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string validate = string.Empty;
            QUIZ_2017 ObjQuiz = new QUIZ_2017();
            List<QUIZ_2017> liQuiz = new List<QUIZ_2017>();
            ObjQuiz.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            ObjQuiz.COURSE_ID = CourseId;
            liQuiz = await Task.Run(() => (List<QUIZ_2017>)CMSHandler.FetchData<QUIZ_2017>(ObjQuiz, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.QuizValidate), sAcademicYear).DataSource.Data);
            if (liQuiz != null)
                validate = "1";
            else
                validate = "0";
            return validate;
        }

        public async Task<ActionResult> QuizResult(string CourseId)
        {
            ExaminationViewModel ObjViewModel = new ExaminationViewModel();
            string sResult = string.Empty;
            sResult = await QuizValidate(CourseId);
            if (sResult != "0")
            {
                try
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    QUIZ_RESULT ObjQuizResult = new QUIZ_RESULT();
                    var liQuiz = new List<QUIZ_RESULT>();
                    ObjQuizResult.STUDENT_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
                    ObjQuizResult.COURSE_ID = CourseId;
                    ObjViewModel.liQuizAnswer = await Task.Run(() => (List<QUIZ_RESULT>)CMSHandler.FetchData<QUIZ_RESULT>(ObjQuizResult, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.QuizResult), sAcademicYear).DataSource.Data);
                    //ObjViewModel.liQuizOption = await Task.Run(() => (List<QUIZ_OPTIONS_2017>)CMSHandler.FetchData<QUIZ_OPTIONS_2017>(ObjQuizResult, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchQuizOptionByStudentId), sAcademicYear).DataSource.Data);
                    int i = 0; int j = 0; int count = 0;
                    foreach (var item in ObjViewModel.liQuizAnswer)
                    {
                        item.SELECTED_ANSWER = HttpUtility.UrlDecode(item.SELECTED_ANSWER);
                        item.ANSWER = HttpUtility.UrlDecode(item.ANSWER);
                        item.QUESTION = HttpUtility.UrlDecode(item.QUESTION);
                        liQuiz.Add(item);
                        if (item.STATUS != "1")
                        {
                            i++;
                            ObjViewModel.Wrong_Answer = i;
                        }
                        else
                        {
                            j++;
                            ObjViewModel.Correct_Answer = j;
                        }
                        count++;
                        ObjViewModel.Count = count;
                    }
                    ObjViewModel.liQuizAnswer = liQuiz;
                    return View(ObjViewModel);

                }
                catch (Exception ex)
                {
                    using (ErrorLog objErrorLog = new ErrorLog())
                    {
                        objErrorLog.WriteError("ExaminationController", "QuizResult", "Err.Msg:" + ex.Message, "Qury is Empty !");
                        objErrorLog.WriteError("ExaminationController", "QuizResult", "Err.Msg:" + ex.Message);
                    }
                    return Json(ex.Message);
                }
            }
            else
            {
                return View(ObjViewModel);
            }

        }
        #endregion
        #region Exame Registered Student Count List
        [UserRights("EXAMINCHARGE")]

        public ActionResult ListExamRegisteredStudent()
        {
            ExaminationViewModel objViewModel = new ExaminationViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var FetchExamList = (List<EXAM_SETTING>)CMSHandler.FetchData<EXAM_SETTING>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchExamSetting), sAcademicYear).DataSource.Data;
                    if (FetchExamList != null && FetchExamList.Count > 0)
                        objViewModel.ExamList = new SelectList(FetchExamList, Common.EXAM_SETTING.EXAM_SETTING_ID, Common.EXAM_SETTING.EXAM_NAME);
                }
                else
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objResult.Message = Common.ErrorMessage.RequestTimeout;
                        objHandler.WriteError("Error", "ErrorMessage", "Query is empty!");
                        objHandler.WriteError("Error", "ErrorMessage", objResult.Message);
                    }
                }
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Examination", "ListExamRegisteredStudent", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Examination", "ListExamRegisteredStudent", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindExamRegisteredStudentCount(string sExamSettingId)
        {
            ExaminationViewModel objViewModel = new ExaminationViewModel();
            EXAM_REGISTRATION_2017 objModel = new EXAM_REGISTRATION_2017();
            objModel.EXAM_SETTING_ID = sExamSettingId;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
            var ListExamRegistration = (List<EXAM_REGISTRATION_2017>)CMSHandler.FetchData<EXAM_REGISTRATION_2017>(objModel, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.ListExamRegisteredStudentCount), sAcademicYear).DataSource.Data;
            if (ListExamRegistration != null && ListExamRegistration.Count > 0)
                objViewModel.liExamRegisteredList = ListExamRegistration;
            if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                objViewModel.liCollegeDetails = FetchCollegeDetails;
            return View(objViewModel);
        }
        #endregion
        #region Student Arrear Registered List
        [UserRights("EXAMINCHARGE")]
        public ActionResult ListArrearRegisteredStudent()
        {
            ExaminationViewModel objViewModel = new ExaminationViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var FetchExamList = (List<EXAM_SETTING>)CMSHandler.FetchData<EXAM_SETTING>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchExamSetting), sAcademicYear).DataSource.Data;
                    var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
                    if (FetchExamList != null && FetchExamList.Count > 0)
                        objViewModel.ExamList = new SelectList(FetchExamList, Common.EXAM_SETTING.EXAM_SETTING_ID, Common.EXAM_SETTING.EXAM_NAME);
                    if (FetchClass != null && FetchClass.Count > 0)
                        objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");
            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Examination", "ListArrearRegisteredStudent", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Examination", "ListArrearRegisteredStudent", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindArrearRegisteredStudent(string sExamSettingId, string sClassId)
        {
            string sSQL = string.Empty;
            ExaminationViewModel objViewModel = new ExaminationViewModel();
            EXAM_REGISTRATION_2017 objModel = new EXAM_REGISTRATION_2017();
            objModel.EXAM_SETTING_ID = sExamSettingId;
            objModel.CLASS_ID = sClassId;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
            sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchArrearExamRegisteredStudent);
            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_CLASSES_2017.CLASS_ID, objModel.CLASS_ID);
            //sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.EXAM_SETTING.EXAM_SETTING_ID, objModel.EXAM_SETTING_ID);
            var listArrearRegisteredStudent = (List<EXAM_REGISTRATION_2017>)CMSHandler.FetchData<EXAM_REGISTRATION_2017>(objModel, sSQL, sAcademicYear).DataSource.Data;
            if (listArrearRegisteredStudent != null && listArrearRegisteredStudent.Count > 0)
                objViewModel.liExamRegisteredList = listArrearRegisteredStudent;
            if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                objViewModel.liCollegeDetails = FetchCollegeDetails;
            return View(objViewModel);
        }
        #endregion
        #region Student Regular Registered List
        [UserRights("EXAMINCHARGE")]
        public ActionResult ListRegularRegisteredStudent()
        {
            ExaminationViewModel objViewModel = new ExaminationViewModel();
            JsonResultData objResult = new JsonResultData();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    var FetchExamList = (List<EXAM_SETTING>)CMSHandler.FetchData<EXAM_SETTING>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchExamSetting), sAcademicYear).DataSource.Data;
                    var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
                    if (FetchExamList != null && FetchExamList.Count > 0)
                        objViewModel.ExamList = new SelectList(FetchExamList, Common.EXAM_SETTING.EXAM_SETTING_ID, Common.EXAM_SETTING.EXAM_NAME);
                    if (FetchClass != null && FetchClass.Count > 0)
                        objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                else
                    return RedirectToAction("ErrorMessage", "Error");

            }
            catch (Exception Ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("Examination", "ListRegularRegisteredStudent", "Err MSg: " + Ex.Message, "Query is empty!");
                    objHandler.WriteError("Examination", "ListRegularRegisteredStudent", Ex.Message);
                }

            }
            return View(objViewModel);
        }
        public ActionResult BindRegularRegisteredStudent(string sExamSettingId, string sClassId)
        {
            string sSQL = string.Empty;
            ExaminationViewModel objViewModel = new ExaminationViewModel();
            EXAM_REGISTRATION_2017 objModel = new EXAM_REGISTRATION_2017();
            objModel.EXAM_SETTING_ID = sExamSettingId;
            objModel.CLASS_ID = sClassId;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var FetchCollegeDetails = (List<CollegeDetails>)CMSHandler.FetchData<CollegeDetails>(null, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCollegeDetails), sAcademicYear).DataSource.Data;
            sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchRegularExamRegisteredStudent);
            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_CLASSES_2017.CLASS_ID, objModel.CLASS_ID);
            var listRegularRegisteredStudent = (List<EXAM_REGISTRATION_2017>)CMSHandler.FetchData<EXAM_REGISTRATION_2017>(objModel, sSQL, sAcademicYear).DataSource.Data;
            if (listRegularRegisteredStudent != null && listRegularRegisteredStudent.Count > 0)
                objViewModel.liExamRegisteredList = listRegularRegisteredStudent;
            if (FetchCollegeDetails != null && FetchCollegeDetails.Count > 0)
                objViewModel.liCollegeDetails = FetchCollegeDetails;
            return View(objViewModel);
        }
        #endregion
    }
}