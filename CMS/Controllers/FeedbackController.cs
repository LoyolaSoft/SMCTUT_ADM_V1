using CMS.Models;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
//using Rotativa;


namespace CMS.Controllers
{
    public class FeedbackController : Controller
    {
        #region Actions

        #endregion
        ResultArgs resultArgs = new ResultArgs();
        DataValue dv = new DataValue();
        // GET: Feedback
        [UserRights("STUDENT")]
        public async Task<ActionResult> Index()
        {
            try
            {
                FeedbackViewModel objFeedback = new FeedbackViewModel();
                objFeedback = await FeedbackIndexAsync();
                return View(objFeedback);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "Index", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "Index", ex.Message);
                }
                return RedirectToAction("Index", "Account");
            }

        }

        //public ActionResult ExportToPDF()
        //{
        //    ActionAsPdf result = new ActionAsPdf("PrintView")
        //    {
        //        FileName = Server.MapPath("~/PrintedFile/" + Session[Common.SESSION_VARIABLES.USER_CODE] + "/" + DateTime.Now.ToLongDateString())
        //    };
        //    return result;
        //}



        public async Task<FeedbackViewModel> FeedbackIndexAsync()
        {

            string sCourseIds = string.Empty;
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtClasswiseStaffList = new DataTable();
            DataTable dtObjectiveList = new DataTable();
            DataTable dtQuestionList = new DataTable();
            DataTable dtStudentCourseList = new DataTable();
            DataTable dtExistingCourse = new DataTable();
            FeedbackViewModel objFeedback = new FeedbackViewModel();
            List<FBStaffList> StaffList = new List<FBStaffList>();
            List<FBOBJECTIVES> ObjectiveList = new List<FBOBJECTIVES>();
            List<FBFEEDBACK_QUESTIONS> QuestoinList = new List<FBFEEDBACK_QUESTIONS>();
            List<StudentCourseList> StudentList = new List<StudentCourseList>();

            //  objFeedback.StaffList =  (from DataRow dr in dtCourseType.Rows select new SelectListItem { Value = dr["ID"].ToString(), Text = dr["Student"].ToString() });
            //  objFeedback.StaffList=new SelectList()
            dv.Clear();
            dv.Add("CLASS_ID", (Session[Common.SESSION_VARIABLES.CLASS_ID] != null) ? Session[Common.SESSION_VARIABLES.CLASS_ID].ToString() : "0", EnumCommand.DataType.String);
            //  dtClasswiseStaffList = objFeedback.FetchClassWiseStaffList(dv).DataSource.Table;
            dtObjectiveList = await Task.Run(() => objFeedback.FetchObjective().DataSource.Table);
            //  dtQuestionList = objFeedback.FetchQuestion().DataSource.Table;
            dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : "0", EnumCommand.DataType.String);
            dtStudentCourseList = await Task.Run(() => objFeedback.FetchStudentCourseListByClassId(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);
            if (dtStudentCourseList != null && dtStudentCourseList.Rows.Count > 0)
            {
                var selectedValues = string.Join(",", dtStudentCourseList.AsEnumerable().Select(s => s.Field<int>("COURSE_ID").ToString()).ToArray());
                sCourseIds = string.Join(",", selectedValues);
            }
            else
                sCourseIds = "0";

            if (dtStudentCourseList != null && dtStudentCourseList.Rows.Count > 0)
            {
                StudentList = await Task.Run(() => ((from DataRow dr in dtStudentCourseList.Rows
                                                     select new StudentCourseList()
                                                     {
                                                         COURSE_ID = dr["COURSE_ID"].ToString(),
                                                         COURSE_TITLE = dr["COURSE_TITLE"].ToString()
                                                     }).ToList()));
            }
            else
            {
                //SelectListItem item = new SelectListItem() { Text = "---Select Course---", Value = "0", Selected = true };

                //objFeedback.StudentCourseList = new SelectList(item,);
            }
            objFeedback.StudentCourseList = new SelectList(StudentList, "COURSE_ID", "COURSE_TITLE", "---Select---");
            if (dtClasswiseStaffList != null && dtClasswiseStaffList.Rows.Count > 0)
            {
                StaffList = await Task.Run(() => (from DataRow dr in dtClasswiseStaffList.Rows
                                                  select new FBStaffList()
                                                  {
                                                      STAFF_ID = dr["STAFF_ID"].ToString(),
                                                      STAFF_NAME = dr["STAFF_NAME"].ToString()
                                                  }).ToList());

            }
            objFeedback.ClasswiseStaffList = new SelectList(StaffList, "STAFF_ID", "STAFF_NAME", "--Select Staff--");

            if (dtObjectiveList != null && dtObjectiveList.Rows.Count > 0)
            {
                ObjectiveList = await Task.Run(() => (from DataRow dr in dtObjectiveList.Rows
                                                      select new FBOBJECTIVES()
                                                      {
                                                          OBJECTIVESHORTTERM = dr[Common.FBOBJECTIVES.OBJECTIVESHORTTERM].ToString(),
                                                          OBJECTIVES = dr[Common.FBOBJECTIVES.OBJECTIVES].ToString(),
                                                          RATING = dr[Common.FBOBJECTIVES.RATING].ToString(),
                                                          OBJECTIVE_ID = dr[Common.FBOBJECTIVES.OBJECTIVE_ID].ToString()
                                                      }
                             ).ToList());
                objFeedback.ObjectiveList = ObjectiveList;
            }
            if (dtQuestionList != null && dtQuestionList.Rows.Count > 0)
            {
                QuestoinList = await Task.Run(() => (from DataRow dr in dtQuestionList.Rows
                                                     select new FBFEEDBACK_QUESTIONS()
                                                     {
                                                         QUESTION = dr[Common.FBFEEDBACK_QUESTIONS.QUESTION].ToString(),
                                                         QUESTION_ID = dr[Common.FBFEEDBACK_QUESTIONS.QUESTION_ID].ToString(),
                                                         QUESTION_TYPE = dr[Common.FBFEEDBACK_QUESTIONS.QUESTION_TYPE].ToString()

                                                     }
                        ).ToList());
                objFeedback.QuestionList = QuestoinList;
            }

            return objFeedback;
        }


        public async Task<ActionResult> QuestionView(string sInputs, string sAssessee)
        {
            try
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtClasswiseStaffList = new DataTable();
                DataTable dtObjectiveList = new DataTable();
                DataTable dtQuestionList = new DataTable();
                DataTable dtExistingCourse = new DataTable();
                List<FBStaffList> StaffList = new List<FBStaffList>();
                List<FBOBJECTIVES> ObjectiveList = new List<FBOBJECTIVES>();
                List<FBFEEDBACK_QUESTIONS> QuestoinList = new List<FBFEEDBACK_QUESTIONS>();


                string str = Session[Common.SESSION_VARIABLES.USER_ROLE_NAME].ToString();
                FeedbackViewModel objFeedback = new FeedbackViewModel();

                dv.Clear();
                dv.Add(Common.FBEVALUATION2017.COURSE_ID, sInputs, EnumCommand.DataType.String);
                dv.Add("ASSESSOR", (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : "0", EnumCommand.DataType.String);
                dv.Add(Common.FBEVALUATION2017.ASSESSEE, sAssessee, EnumCommand.DataType.String);
                objFeedback.sValidateStaff = await Task.Run(() => objFeedback.ValidateExisitingCourseList(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").StringResult);
                if (!string.IsNullOrEmpty(objFeedback.sValidateStaff) && objFeedback.sValidateStaff != "1")
                {
                    // dv.Clear();
                    // dtClasswiseStaffList = objFeedback.FetchClassWiseStaffList().DataSource.Table;
                    dtObjectiveList = objFeedback.FetchObjective().DataSource.Table;
                    dv.Clear();
                    dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, sInputs, EnumCommand.DataType.String);
                    dtQuestionList = objFeedback.FetchQuestion(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                    ObjectiveList = await Task.Run(() => (from DataRow dr in dtObjectiveList.Rows
                                                          select new FBOBJECTIVES()
                                                          {
                                                              OBJECTIVESHORTTERM = dr[Common.FBOBJECTIVES.OBJECTIVESHORTTERM].ToString(),
                                                              OBJECTIVES = dr[Common.FBOBJECTIVES.OBJECTIVES].ToString(),
                                                              RATING = dr[Common.FBOBJECTIVES.RATING].ToString(),
                                                              OBJECTIVE_ID = dr[Common.FBOBJECTIVES.OBJECTIVE_ID].ToString()
                                                          }
                                 ).ToList());
                    objFeedback.ObjectiveList = ObjectiveList;

                    QuestoinList = await Task.Run(() => (from DataRow dr in dtQuestionList.Rows
                                                         select new FBFEEDBACK_QUESTIONS()
                                                         {
                                                             QUESTION = dr[Common.FBFEEDBACK_QUESTIONS.QUESTION].ToString(),
                                                             QUESTION_ID = dr[Common.FBFEEDBACK_QUESTIONS.QUESTION_ID].ToString(),
                                                             QUESTION_TYPE = dr[Common.FBFEEDBACK_QUESTIONS.QUESTION_TYPE].ToString()
                                                         }
                            ).ToList());
                    objFeedback.QuestionList = QuestoinList;
                }
                return View(objFeedback);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "QuestionView", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "QuestionView", ex.Message);
                }
                return RedirectToAction("Index", "Account");
            }

        }
        [HttpPost]
        public async Task<ActionResult> Save(FormCollection objCollection)
        {
            try
            {


                resultArgs.Success = await Task.Run(() => SaveFeedbackAsync(ref objCollection));
                //resultArgs.Success =  SaveFeedbackAsync(objCollection);
                if (resultArgs.Success)
                {
                    TempData["Success"] = "Saved Successfully !!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Failed"] = "Failed to Save Records !!!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "QuestionView", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "QuestionView", ex.Message);
                }
                TempData["Failed"] = "Failed to Save Records !!!";
                return RedirectToAction("Index");
            }

        }

        public bool SaveFeedbackAsync(ref FormCollection objCollection)
        {
            bool sResult = false;

            string sSQL = string.Empty;
            string sStudentId = string.Empty;
            string sStaffId = string.Empty;
            string sClassId = string.Empty;
            string sAnswer = string.Empty;
            string sQuestionId = string.Empty;
            string sCourseId = string.Empty;
            DataTable dtStudentInfo = new DataTable();
            FeedbackViewModel objFeedback = new FeedbackViewModel();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                sStudentId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : "0";
                sCourseId = objCollection["StudentCourseList"].ToString();
                sClassId = (Session[Common.SESSION_VARIABLES.FBCLASS_ID] != null) ? Session[Common.SESSION_VARIABLES.FBCLASS_ID].ToString() : "0";

                sSQL = @"INSERT INTO FBEVALUATION2017(ASSESSOR,CLASS_ID,ASSESSEE,COURSE_ID,FEEDBACK_ID,QUESTION,ANSWER)VALUES";
                foreach (var item in objCollection.Keys)
                {
                    if (item.ToString() != "ClasswiseStaffList" && item.ToString() != "__RequestVerificationToken" && item.ToString() != "StudentCourseList")
                    {
                        sSQL += "('" + sStudentId + "','" + sClassId + "','" + objCollection["ClasswiseStaffList"] + "','" + sCourseId + "','1','" + item.ToString() + "','" + objCollection[item.ToString()] + "'),";
                    }

                }
                sSQL = sSQL.Trim(',') + ";";
                //  resultArgs = await Task.Run(() => objFeedback.SaveFeedback(sSQL));
                resultArgs = objFeedback.SaveFeedback(sSQL);
                sResult = resultArgs.Success;

            }


            return sResult;

        }

        public async Task<ActionResult> Result()
        {
            try
            {
                FeedbackViewModel objFeedback = new FeedbackViewModel();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    List<FBCLASSES_BY_STAFF> iFbClass = new List<FBCLASSES_BY_STAFF>();
                    List<StudentCourseList> iFbList = new List<StudentCourseList>();
                    DataTable dtCourse = new DataTable();
                    DataTable dtClass = new DataTable();
                    dv.Clear();
                    dv.Add(Common.FBCLASS_WISE_STAFF2017.STAFF_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                    dtClass = await Task.Run(() => objFeedback.FetchFBClassesByStaffId(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);
                    // dtCourse = await Task.Run(() => objFeedback.FetchCourseListByStaffId(dv).DataSource.Table);
                    //  iFbList = await Task.Run(() => (from DataRow dr in dtCourse.Rows select new StudentCourseList() { COURSE_ID = dr["COURSE_ID"].ToString(), COURSE_TITLE = dr["COURSE_TITLE"].ToString() }).ToList());
                    iFbClass = await Task.Run(() => (from DataRow dr in dtClass.Rows select new FBCLASSES_BY_STAFF() { CLASS_ID = dr["CLASS_ID"].ToString(), CLASS_DESCRIPTION = dr["CLASS_DESCRIPTION"].ToString() }).ToList());
                    iFbList.Add(new StudentCourseList { COURSE_ID = "0", COURSE_TITLE = "---Select---" });
                    objFeedback.CourseList = new SelectList(iFbList, "COURSE_ID", "COURSE_TITLE");
                    objFeedback.ClassList = new SelectList(iFbClass, "CLASS_ID", "CLASS_DESCRIPTION");
                }
                else
                {

                }
                return View(objFeedback);
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "Result", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "Result", ex.Message);
                }
                return RedirectToAction("Index", "Account");
            }

        }
        public ActionResult PrintView()
        {
            return View("Hello world......");
        }

        public async Task<ActionResult> LoadFeedbackResult(string CourseId, string ClassId)
        {
            FeedbackViewModel objFeedback = new FeedbackViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    Session[Common.SESSION_VARIABLES.COURSE_ID] = CourseId;
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    List<FBRATING_BY_STAFF> rating = new List<FBRATING_BY_STAFF>();
                    DataTable dtCourseClassInfo = new DataTable();
                    DataTable dtStaffRatting = new DataTable();
                    DataTable dtFeedbackSetting = new DataTable();

                    dv.Clear();
                    dv.Add(Common.FBEVALUATION2017.COURSE_ID, CourseId, EnumCommand.DataType.String);
                    dv.Add(Common.FBCLASS_WISE_STAFF2017.STAFF_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                    dv.Add(Common.FBEVALUATION2017.CLASS_ID, ClassId, EnumCommand.DataType.String);

                    dtCourseClassInfo = await Task.Run(() => objFeedback.FetchFBClassCourseInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);
                    dtFeedbackSetting = await Task.Run(() => objFeedback.FetchFBSettingInfo().DataSource.Table);
                    dv.Clear();
                    dv.Add(Common.FBEVALUATION2017.COURSE_ID, CourseId, EnumCommand.DataType.String);
                    dv.Add(Common.FBEVALUATION2017.ASSESSEE, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                    dv.Add(Common.FBEVALUATION2017.CLASS_ID, ClassId, EnumCommand.DataType.String);
                    dtStaffRatting = await Task.Run(() => objFeedback.FetchFBResultByCourseId(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);
                    if (dtFeedbackSetting != null && dtFeedbackSetting.Rows.Count > 0)
                    {
                        objFeedback.objFBSetting = new FBFEEDBACK_SETTINGS()
                        {
                            FEEDBACK_NAME = dtFeedbackSetting.Rows[0][Common.FBFEEDBACK_SETTINGS.FEEDBACK_NAME].ToString()
                        ,
                            ACADEMIC_YEAR = dtFeedbackSetting.Rows[0][Common.FBFEEDBACK_SETTINGS.ACADEMIC_YEAR].ToString()
                        };

                    }
                    if (dtCourseClassInfo != null && dtCourseClassInfo.Rows.Count > 0)
                    {
                        objFeedback.objCourseAndClassInfo = new FBCOURSE_AND_CLASS_INFO()
                        {
                            CLASS_NAME = dtCourseClassInfo.Rows[0][Common.COURSE_AND_COURSE_INFO_BY_STAFF_ID.CLASS_NAME].ToString(),
                            COURSE_CODE = dtCourseClassInfo.Rows[0][Common.COURSE_AND_COURSE_INFO_BY_STAFF_ID.COURSE_CODE].ToString(),
                            COURSE_TITLE = dtCourseClassInfo.Rows[0][Common.COURSE_AND_COURSE_INFO_BY_STAFF_ID.COURSE_TITLE].ToString(),
                            COURSE_TYPE = dtCourseClassInfo.Rows[0][Common.COURSE_AND_COURSE_INFO_BY_STAFF_ID.COURSE_TYPE].ToString(),
                            DEPARTMENT = dtCourseClassInfo.Rows[0][Common.COURSE_AND_COURSE_INFO_BY_STAFF_ID.DEPARTMENT].ToString(),
                            TOTAL_STUDENTS = dtCourseClassInfo.Rows[0][Common.COURSE_AND_COURSE_INFO_BY_STAFF_ID.TOTAL_STUDENTS].ToString()
                        };
                    }
                    if (dtStaffRatting != null && dtStaffRatting.Rows.Count > 0)
                    {

                        rating = await Task.Run(() => (from DataRow dr in dtStaffRatting.Rows
                                                       select new FBRATING_BY_STAFF()
                                                       {
                                                           ASSESSOR = dr[Common.FBRATING_BY_STAFF_BY_COURSE_ID.ASSESSOR].ToString(),
                                                           GOOD = dr[Common.FBRATING_BY_STAFF_BY_COURSE_ID.GOOD].ToString(),
                                                           GROUP_TITLE = dr[Common.FBRATING_BY_STAFF_BY_COURSE_ID.GROUP_TITLE].ToString(),
                                                           POOR = dr[Common.FBRATING_BY_STAFF_BY_COURSE_ID.POOR].ToString(),
                                                           QUESTION = dr[Common.FBRATING_BY_STAFF_BY_COURSE_ID.QUESTION].ToString(),
                                                           SATISFACTORY = dr[Common.FBRATING_BY_STAFF_BY_COURSE_ID.SATISFACTORY].ToString(),
                                                           VERY_GOOD = dr[Common.FBRATING_BY_STAFF_BY_COURSE_ID.VERY_GOOD].ToString(),
                                                           VERY_POOR = dr[Common.FBRATING_BY_STAFF_BY_COURSE_ID.VERY_POOR].ToString()
                                                       }).ToList());


                    }
                    objFeedback.objRating = rating;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "LoadFeedbackResult", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "LoadFeedbackResult", ex.Message);
                }
            }

            return View(objFeedback);
        }

        public ActionResult StaffRating(string sStaffId)
        {
            try
            {
                FeedbackViewModel objFeedbackViewModel = new FeedbackViewModel();
                DataTable dtRattingByStaffId = new DataTable();
                List<FBRATING_BY_STAFF> FBRatingList = new List<FBRATING_BY_STAFF>();
                DataValue dv = new DataValue();

                dv.Add(Common.FBEVALUATION.ASSESSEE, sStaffId, EnumCommand.DataType.String);

                dtRattingByStaffId = new FeedbackViewModel().FetchOverallRatingByStaffId(dv).DataSource.Table;
                if (dtRattingByStaffId != null && dtRattingByStaffId.Rows.Count > 0)
                {
                    //FBRatingList = (from DataRow dr in dtRattingByStaffId.Rows
                    //                select new FBRATING_BY_STAFF()
                    //                {
                    //                    ASSESSOR = dr[Common.FBRATING_BY_STAFF.ASSESSOR].ToString(),
                    //                    CLASS_CODE = dr[Common.FBRATING_BY_STAFF.CLASS_CODE].ToString(),
                    //                    FIRST_NAME = dr[Common.FBRATING_BY_STAFF.FIRST_NAME].ToString(),
                    //                    OBJECTIVES_COUNT = dr[Common.FBRATING_BY_STAFF.OBJECTIVES_COUNT].ToString(),
                    //                    OBJECTIVES = dr[Common.FBRATING_BY_STAFF.OBJECTIVES].ToString(),
                    //                    STUDENT_COUNT = dr[Common.FBRATING_BY_STAFF.STUDENT_COUNT].ToString(),
                    //                    QUESTION = dr[Common.FBRATING_BY_STAFF.QUESTION].ToString()

                    //                }
                    //    ).ToList();
                    objFeedbackViewModel.StaffwiseRatingList = FBRatingList;
                }

            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "StaffRating", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "StaffRating", ex.Message);
                }
            }

            return View();
        }

        public ActionResult PrintOptions()
        {
            return View();
        }



        #region PrincipalView
        [UserRights("PRINCIPAL")]
        public async Task<ActionResult> FeedbackPricipalView()
        {
            FeedbackViewModel objFeedback = new FeedbackViewModel();
            try
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    List<Department> objDepartment = new List<Department>();
                    List<FBStaffList> objStaffList = new List<FBStaffList>();
                    DataTable dtDepartmentList = new DataTable();


                    dtDepartmentList = await Task.Run(() => objFeedback.FetchDepartmentList((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);
                    if (dtDepartmentList != null && dtDepartmentList.Rows.Count > 0)
                    {
                        objDepartment = await Task.Run(() => (from DataRow dr in dtDepartmentList.Rows
                                                              select new Department()
                                                              {
                                                                  DEPARTMENT = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString(),
                                                                  DEPARTMENT_ID = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT_ID].ToString()
                                                              }).ToList());
                        ;
                    }
                    else
                    {
                        objDepartment.Add(new Department() { DEPARTMENT = "---Select---", DEPARTMENT_ID = "0" });
                    }

                    objFeedback.DepartmentList = new SelectList(objDepartment, Common.CP_DEPARTMENT_2017.DEPARTMENT_ID, Common.CP_DEPARTMENT_2017.DEPARTMENT);
                    objFeedback.StaffListByDepartmet = new SelectList(objStaffList, Common.STF_PERSONAL_INFO.STAFF_ID, "STAFF_NAME", "---Select---");
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "FeedbackPricipalView", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "FeedbackPricipalView", ex.Message);
                }
            }

            return View(objFeedback);
        }
        [UserRights("PRINCIPAL")]
        public async Task<ActionResult> FeedbackPricipalViewForDepartment()
        {
            FeedbackViewModel objFeedback = new FeedbackViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
                {
                    List<Department> objDepartment = new List<Department>();
                    List<Sup_Shift> objShift = new List<Sup_Shift>();

                    DataTable dtDepartmentList = new DataTable();
                    DataTable dtShift = new DataTable();


                    dtShift = await Task.Run(() => objFeedback.FetchShiftList().DataSource.Table);
                    if (dtShift != null && dtShift.Rows.Count > 0)
                    {
                        objShift = await Task.Run(() => (from DataRow dr in dtShift.Rows
                                                         select new Sup_Shift()
                                                         {
                                                             SHIFT_ID = dr[Common.SUP_SHIFT.SHIFT_ID].ToString(),
                                                             SHIFT_NAME = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString()
                                                         }).ToList());
                        ;
                    }
                    else
                    {
                        objShift.Add(new Sup_Shift() { SHIFT_ID = "---Select---", SHIFT_NAME = "0" });
                    }

                    objFeedback.ShiftList = new SelectList(objShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    objFeedback.DepartmentList = new SelectList(objDepartment, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME, "---Select---");
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "FeedbackPricipalViewForDepartment", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "FeedbackPricipalViewForDepartment", ex.Message);
                }
            }

            return View(objFeedback);
        }

        #endregion
        public async Task<string> StaffListBySubjectId(string sCourseId)
        {
            List<FBStaffList> StaffList = new List<FBStaffList>();
            DataTable dtStaffList = new DataTable();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            FeedbackViewModel objFeedbackViewModel = new FeedbackViewModel();
            string option = "<option value='0' >---Select Staff---</option>";
            try
            {
                dv.Clear();
                dv.Add("COURSE_ID", sCourseId, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : "0", EnumCommand.DataType.String);
                resultArgs = await Task.Run(() => objFeedbackViewModel.FetchCourseWiseStaffList(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : ""));
                dtStaffList = resultArgs.DataSource.Table;
                if (dtStaffList != null && dtStaffList.Rows.Count > 0)
                {
                    Session[Common.SESSION_VARIABLES.FBCLASS_ID] = dtStaffList.Rows[0]["CLASS_ID"].ToString();
                }


                foreach (DataRow item in dtStaffList.Rows)
                {
                    option += "<option value='" + item["STAFF_ID"].ToString() + "' >" + item["STAFF_NAME"] + "</option>";
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "StaffListBySubjectId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "StaffListBySubjectId", ex.Message);
                }
            }

            return option;
        }
        public async Task<string> StaffListByDepartment(string sDepartmentId)
        {
            List<FBStaffList> StaffList = new List<FBStaffList>();
            DataTable dtStaffList = new DataTable();
            FeedbackViewModel objFeedbackViewModel = new FeedbackViewModel();
            string option = string.Empty;
            try
            {
                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.DEPARTMENT, sDepartmentId, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.CATEGORY, "1", EnumCommand.DataType.String);
                resultArgs = await Task.Run(() => objFeedbackViewModel.FetchStaffListByDepartment(dv));
                dtStaffList = resultArgs.DataSource.Table;
                //string option = "<option value='0' >---Select Staff---</option>";


                if (dtStaffList != null && dtStaffList.Rows.Count > 0)
                {
                    foreach (DataRow item in dtStaffList.Rows)
                    {
                        option += "<option value='" + item["STAFF_ID"].ToString() + "' >" + item["STAFF_NAME"] + "</option>";
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "StaffListByDepartment", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "StaffListByDepartment", ex.Message);
                }
            }

            return option;
        }

        public async Task<string> DepartmentListByShift(string sShiftId)
        {
            List<Department> objDepartment = new List<Department>();
            DataTable dtDepartmentList = new DataTable();
            FeedbackViewModel objFeedbackViewModel = new FeedbackViewModel();
            string option = string.Empty;
            try
            {
                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.SHIFT, sShiftId, EnumCommand.DataType.String);
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                dtDepartmentList = await Task.Run(() => objFeedbackViewModel.FetchDepartmentByShiftId(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);
                if (dtDepartmentList != null && dtDepartmentList.Rows.Count > 0)
                {
                    foreach (DataRow item in dtDepartmentList.Rows)
                    {
                        option += "<option value='" + item[Common.CP_DEPARTMENT_2017.DEPARTMENT_ID].ToString() + "' >" + item[Common.CP_DEPARTMENT_2017.DEPARTMENT] + "</option>";
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("FeedbackController", "DepartmentListByShift", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("FeedbackController", "DepartmentListByShift", ex.Message);
                }
            }

            return option;
        }

        public async Task<string> GetCourseListByClassId(string sClassId)
        {
            string option = string.Empty;
            DataTable dtCourseList = new DataTable();
            DataTable dtClassList = new DataTable();

            using (ExaminationViewModel objExamination = new ExaminationViewModel())
            {
                try
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objExamination.dv.Clear();
                    objExamination.dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.STAFF_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                    objExamination.dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
                    dtCourseList = await Task.Run(() => objExamination.FetchCourseList((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);

                    foreach (DataRow item in dtCourseList.Rows)
                    {
                        option += "<option value='" + item[Common.CP_CLASS_COURSE_2017.COURSE_ID].ToString() + "' >" + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString() + "</option>";
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("FeedbackController", "GetCourseListByClassId", "Err MSg: " + ex.Message, "");
                        objHandler.WriteError("FeedbackController", "GetCourseListByClassId", ex.Message);
                    }

                }
                return option;

            }


        }
    }
}