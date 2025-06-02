using CMS.Models;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;



namespace CMS.Controllers
{
    public class AcademicsController : Controller
    {
        ResultArgs resultArg = new ResultArgs();
        DataValue dv = new DataValue();
        string sResult = string.Empty;
        // GET: Academics
        #region Academic Year
        public ActionResult AcademicYearInfo()
        {
            return View();
        }
        [UserRights("ADMIN")]
        public ActionResult ListAcademicDetails()
        {
            AcademicsViewModel objAcademicyear = new AcademicsViewModel();
            DataTable dtAcademicYearList = new DataTable();
            dtAcademicYearList = objAcademicyear.FetchAcademicYearDetails().DataSource.Table;
            IList<AcademicYearModel> liList = new List<AcademicYearModel>();
            liList = (from DataRow dr in dtAcademicYearList.Rows
                      select new AcademicYearModel
                      {
                          ACADEMIC_YEAR_ID = dr[Common.ACC_YEAR.ACADEMIC_YEAR_ID].ToString(),
                          AC_YEAR = dr[Common.ACC_YEAR.AC_YEAR].ToString(),
                          DATE_FROM = dr[Common.ACC_YEAR.DATE_FROM].ToString(),
                          DATE_TO = dr[Common.ACC_YEAR.DATE_TO].ToString(),
                          ACTIVE_YEAR = dr[Common.ACC_YEAR.ACTIVE_YEAR].ToString(),
                          ACADEMIC_NAME = dr[Common.ACC_YEAR.ACADEMIC_NAME].ToString(),
                      }).ToList();
            return View(liList);
        }
        public string UpdateActiveAcademic(string sActiveYearId)
        {
            if (sActiveYearId != null)
            {
                DataTable dt = new DataTable();
                dv.Clear();
                dv.Add(Common.ACC_YEAR.ACADEMIC_YEAR_ID, sActiveYearId, EnumCommand.DataType.String);
                using (AcademicsViewModel objAcademicYearView = new AcademicsViewModel())
                {
                    DataTable dtActiveYear = new DataTable();
                    resultArg = objAcademicYearView.UpdateActiveAcademic(dv);
                    resultArg = objAcademicYearView.UpdateActiveYear(dv);
                    dt = objAcademicYearView.FetchAcademicYearDetails().DataSource.Table;
                    dtActiveYear = objAcademicYearView.FetchActiveYear().DataSource.Table;
                    sResult = resultArg.Success.ToString();
                    Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] = dtActiveYear.Rows[0][Common.ACADEMIC_YEAR.AC_YEAR].ToString();
                    Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_NAME] = dtActiveYear.Rows[0][Common.ACADEMIC_YEAR.ACADEMIC_NAME].ToString();
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    AcademicYearModel objAcademicYear = new AcademicYearModel()
                    {
                        AC_YEAR = dt.Rows[0][Common.ACC_YEAR.AC_YEAR].ToString(),
                        DATE_FROM = dt.Rows[0][Common.ACC_YEAR.DATE_FROM].ToString(),
                        DATE_TO = dt.Rows[0][Common.ACC_YEAR.DATE_TO].ToString(),
                        ACTIVE_YEAR = dt.Rows[0][Common.ACC_YEAR.ACTIVE_YEAR].ToString(),
                        ACADEMIC_NAME = dt.Rows[0][Common.ACC_YEAR.ACADEMIC_NAME].ToString(),
                    };
                }
            }

            return sResult;
        }
        public string InsertAcademicYearInfo(string str)
        {
            if (str != null)
            {
                var Result = JsonConvert.DeserializeObject<AcademicYearModel>(str);
                dv.Clear();
                dv.Add(Common.ACC_YEAR.AC_YEAR, Result.AC_YEAR, EnumCommand.DataType.String);
                dv.Add(Common.ACC_YEAR.DATE_FROM, CommonMethods.ConvertdatetoyearFromat(Result.DATE_FROM), EnumCommand.DataType.String);
                dv.Add(Common.ACC_YEAR.DATE_TO, CommonMethods.ConvertdatetoyearFromat(Result.DATE_TO), EnumCommand.DataType.String);
                dv.Add(Common.ACC_YEAR.ACADEMIC_NAME, Result.ACADEMIC_NAME, EnumCommand.DataType.String);

                using (AcademicsViewModel objAcademicView = new AcademicsViewModel())
                {
                    resultArg = objAcademicView.InsertAcademicYearInfo(dv);
                    sResult = resultArg.Success.ToString();
                }
            }
            return sResult;
        }
        public ActionResult EditAcademicYearInfo(int id)
        {
            string sAcademicYeadId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] = sAcademicYeadId;
            dv.Clear();
            dv.Add(Common.ACC_YEAR.ACADEMIC_YEAR_ID, sAcademicYeadId, EnumCommand.DataType.String);
            DataTable dtFetchRecordById = new DataTable();
            using (AcademicsViewModel objAcademicyear = new AcademicsViewModel())
            {
                dtFetchRecordById = objAcademicyear.FetchAcademicYearById(dv).DataSource.Table;
            }
            //List<AcademicYearModel> liList = new List<AcademicYearModel>();
            //liList = (from DataRow dr in dtFetchRecordById.Rows
            AcademicYearModel objAcademicYearView = new AcademicYearModel()
            {
                AC_YEAR = dtFetchRecordById.Rows[0][Common.ACC_YEAR.AC_YEAR].ToString(),
                DATE_FROM = dtFetchRecordById.Rows[0][Common.ACC_YEAR.DATE_FROM].ToString(),
                DATE_TO = dtFetchRecordById.Rows[0][Common.ACC_YEAR.DATE_TO].ToString(),
                ACTIVE_YEAR = dtFetchRecordById.Rows[0][Common.ACC_YEAR.ACTIVE_YEAR].ToString(),
                ACADEMIC_NAME = dtFetchRecordById.Rows[0][Common.ACC_YEAR.ACADEMIC_NAME].ToString(),
            };
            return View(objAcademicYearView);
        }
        public string UpdateAcademicYearInfo(string str)
        {
            var sAcademicYearId = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID].ToString();
            if (str != null)
            {
                var Result = JsonConvert.DeserializeObject<AcademicYearModel>(str);
                dv.Clear();
                dv.Add(Common.ACC_YEAR.AC_YEAR, Result.AC_YEAR, EnumCommand.DataType.String);
                dv.Add(Common.ACC_YEAR.DATE_FROM, CommonMethods.ConvertdatetoyearFromat(Result.DATE_FROM), EnumCommand.DataType.String);
                dv.Add(Common.ACC_YEAR.DATE_TO, CommonMethods.ConvertdatetoyearFromat(Result.DATE_TO), EnumCommand.DataType.String);
                dv.Add(Common.ACC_YEAR.ACADEMIC_NAME, Result.ACADEMIC_NAME, EnumCommand.DataType.String);
                dv.Add(Common.ACC_YEAR.ACADEMIC_YEAR_ID, sAcademicYearId, EnumCommand.DataType.String);
                using (AcademicsViewModel objAcademicYearView = new AcademicsViewModel())
                {
                    resultArg = objAcademicYearView.UpdateAcademicYearInfo(dv);
                    sResult = resultArg.Success.ToString();
                }
            }
            return sResult;
        }
        public ActionResult DeleteAcademicYearInfo(string id)
        {
            if (id != null)
            {
                using (AcademicsViewModel objAcademicYearView = new AcademicsViewModel())
                {
                    dv.Clear();
                    dv.Add(Common.ACC_YEAR.ACADEMIC_YEAR_ID, id, EnumCommand.DataType.String);
                    resultArg = objAcademicYearView.DeleteAcademicYearInfo(dv);
                    sResult = resultArg.Success.ToString();
                }
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = Common.Messages.RecordDeletedSuccessfully;
            }
            else
            {
                TempData["DeleteError"] = Common.Messages.FailedToDeletedTryAgain;
            }
            return RedirectToAction("ListAcademicDetails", "Academics");
        }
        #endregion
        #region Course root
        public ActionResult CourseRootInfo()

        {
            CourseRootModel objCourseRoot = new CourseRootModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtDepartment = new DataTable();
            DataTable dtPart = new DataTable();
            DataTable dtCourseType = new DataTable();
            DataTable dtPaperCode = new DataTable();
            DataTable dtSemester = new DataTable();
            DataTable dtUGCCourseType = new DataTable();
            DataTable dtPaperType = new DataTable();
            DataTable dtSubjectType = new DataTable();

            dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            dtPart = SupportDataMethod.FetchPart().DataSource.Table;
            dtCourseType = SupportDataMethod.FetchCourseItems((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            dtPaperCode = SupportDataMethod.FetchPaperCode().DataSource.Table;
            dtSemester = SupportDataMethod.FetchSemester().DataSource.Table;
            dtUGCCourseType = SupportDataMethod.FetchUGCCoursetype().DataSource.Table;
            dtPaperType = SupportDataMethod.FetchPaperType().DataSource.Table;
            dtSubjectType = SupportDataMethod.FetchSubjectType().DataSource.Table;

            List<SupDepartment> liDepartment = new List<SupDepartment>();
            List<SUP_PART> liPart = new List<SUP_PART>();
            List<SupCourseType> liCourseType = new List<SupCourseType>();
            List<SupPaperCode> liPaperCode = new List<SupPaperCode>();
            List<SupSemester> liSemester = new List<SupSemester>();
            List<SupIsAlliedSubject> liIsAllied = new List<SupIsAlliedSubject>();
            List<SupIsCompulsory> liIsCompulsory = new List<SupIsCompulsory>();
            List<SupUGCCourseType> liUGCCourseType = new List<SupUGCCourseType>();
            List<SupPaperType> liPaperType = new List<SupPaperType>();
            List<SUP_SUBJECT_TYPE> liSupbjectType = new List<SUP_SUBJECT_TYPE>();

            if (dtDepartment != null && dtDepartment.Rows.Count > 0)
            {
                liDepartment = (from DataRow dr in dtDepartment.Rows
                                select new SupDepartment()
                                {
                                    DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                    DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                }).ToList();
                objCourseRoot.DEPARTMENT = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
            }

            if (dtPart != null && dtPart.Rows.Count > 0)
            {
                liPart = (from DataRow dr in dtPart.Rows
                          select new SUP_PART()
                          {
                              PART_ID = dr[Common.SUP_PART.PART_ID].ToString(),
                              PART = dr[Common.SUP_PART.PART].ToString()
                          }).ToList();
                objCourseRoot.PART = new SelectList(liPart, Common.SUP_PART.PART_ID, Common.SUP_PART.PART);
            }
            if (dtCourseType != null && dtCourseType.Rows.Count > 0)
            {
                liCourseType = (from DataRow dr in dtCourseType.Rows
                                select new SupCourseType()
                                {
                                    COURSE_TYPE_ID = dr[Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ID].ToString(),
                                    COURSE_TYPE = dr[Common.CP_COURSE_TYPE_2017.COURSE_TYPE].ToString()
                                }).ToList();
                objCourseRoot.COURSE_TYPE = new SelectList(liCourseType, Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ID, Common.CP_COURSE_TYPE_2017.COURSE_TYPE);
            }

            if (dtPaperCode != null && dtPaperCode.Rows.Count > 0)
            {
                liPaperCode = (from DataRow dr in dtPaperCode.Rows
                               select new SupPaperCode()
                               {
                                   PAPER_CODE_ID = dr[Common.SUP_PAPER_CODE.PAPER_CODE_ID].ToString(),
                                   PAPER_CODE = dr[Common.SUP_PAPER_CODE.PAPER_CODE].ToString()
                               }).ToList();
                objCourseRoot.PAPER_CODE = new SelectList(liPaperCode, Common.SUP_PAPER_CODE.PAPER_CODE_ID, Common.SUP_PAPER_CODE.PAPER_CODE);
            }
            if (dtSemester != null && dtSemester.Rows.Count > 0)
            {
                liSemester = (from DataRow dr in dtSemester.Rows
                              select new SupSemester()
                              {
                                  SUP_SEMESTER_ID = dr[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString(),
                                  SEMESTER_NAME = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString()
                              }).ToList();
                objCourseRoot.SEMESTER = new SelectList(liSemester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
            }

            if (dtUGCCourseType != null && dtUGCCourseType.Rows.Count > 0)
            {
                liUGCCourseType = (from DataRow dr in dtUGCCourseType.Rows
                                   select new SupUGCCourseType()
                                   {
                                       UGC_COURSE_TYPE_ID = dr[Common.CP_UGC_COURSE_TYPE.UGC_COURSE_TYPE_ID].ToString(),
                                       UGC_COURSE_TYPE = dr[Common.CP_UGC_COURSE_TYPE.UGC_COURSE_TYPE].ToString()
                                   }).ToList();
                objCourseRoot.UGC_COURSE_TYPE = new SelectList(liUGCCourseType, Common.CP_UGC_COURSE_TYPE.UGC_COURSE_TYPE_ID, Common.CP_UGC_COURSE_TYPE.UGC_COURSE_TYPE);
            }
            if (dtPaperType != null && dtPaperType.Rows.Count > 0)
            {
                liPaperType = (from DataRow dr in dtPaperType.Rows
                               select new SupPaperType()
                               {
                                   PAPER_TYPE_ID = dr[Common.SUP_PAPER_TYPE.PAPER_TYPE_ID].ToString(),
                                   PAPER_TYPE = dr[Common.SUP_PAPER_TYPE.PAPER_TYPE].ToString()
                               }).ToList();
                objCourseRoot.PAPER_TYPE = new SelectList(liPaperType, Common.SUP_PAPER_TYPE.PAPER_TYPE_ID, Common.SUP_PAPER_TYPE.PAPER_TYPE);
            }
            if (dtSubjectType != null && dtSubjectType.Rows.Count > 0)
            {
                liSupbjectType = (from DataRow dr in dtSubjectType.Rows
                                  select new SUP_SUBJECT_TYPE()
                                  {
                                      SUBJECT_TYPE_ID = dr[Common.SUP_SUBJECT_TYPE.SUBJECT_TYPE_ID].ToString(),
                                      SUBJECT_TYPE = dr[Common.SUP_SUBJECT_TYPE.SUBJECT_TYPE].ToString()
                                  }).ToList();
                objCourseRoot.SUBJECT_TYPE = new SelectList(liSupbjectType, Common.SUP_SUBJECT_TYPE.SUBJECT_TYPE_ID, Common.SUP_SUBJECT_TYPE.SUBJECT_TYPE);
            }
            return View(objCourseRoot);
        }
        [UserRights("ADMIN")]
        public ActionResult ListCourseInfo()
        {
            AcademicsViewModel objCourseRootView = new AcademicsViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtCourseRootInfo = new DataTable();
            dtCourseRootInfo = objCourseRootView.FetchCourseRootInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<CourseRootInfo> liCourseRoot = new List<CourseRootInfo>();
            if (dtCourseRootInfo != null && dtCourseRootInfo.Rows.Count > 0)
            {
                liCourseRoot = (from DataRow dr in dtCourseRootInfo.Rows
                                select new CourseRootInfo
                                {
                                    COURSE_ROOT_ID = dr[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID].ToString(),
                                    DEPARTMENT = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString(),
                                    YEAR = dr[Common.CP_COURSE_ROOT_2017.YEAR].ToString(),
                                    PART = dr[Common.SUP_PART.PART].ToString(),
                                    COURSE_TYPE = dr[Common.CP_COURSE_TYPE_2017.COURSE_TYPE].ToString(),
                                    COURSE_TITLE = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString(),
                                    COURSE_CODE = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString(),
                                    SEMESTER = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString(),
                                    IS_NME_SUBJECT = dr[Common.SUP_IS_NME_SUBJECT.IS_NME_NAME].ToString(),
                                    IS_ALLIED_SUBJECT = dr[Common.SUP_IS_ALLIED_SUBJECT.IS_ALLIED_NAME].ToString()
                                }).ToList();
            }
            return View(liCourseRoot);
        }
        public JsonResult InsertCourseRootInfo(string str)
        {
            if (str != null)
            {
                DataTable dtCourseRoot = new DataTable();
                var Result = JsonConvert.DeserializeObject<CourseRootInfo>(str);
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                dv.Clear();
                dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_CODE, Result.COURSE_CODE, EnumCommand.DataType.String);
                using (AcademicsViewModel objCourseRoot = new AcademicsViewModel())
                {
                    dtCourseRoot = objCourseRoot.CheckCourseCode(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                    if (dtCourseRoot != null && dtCourseRoot.Rows.Count == 0)
                    {
                        dv.Clear();
                        dv.Add(Common.CP_COURSE_ROOT_2017.DEPARTMENT, Result.DEPARTMENT, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.YEAR_FROM, Result.YEAR_FROM, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.YEAR_TO, Result.YEAR_TO, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.YEAR, Result.YEAR, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.PART, Result.PART, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_TYPE, Result.COURSE_TYPE, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.HRS_PER_WEEK, Result.HRS_PER_WEEK, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.CREDITS, Result.CREDITS, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.PAPER_CODE, Result.PAPER_CODE, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_TITLE, Result.COURSE_TITLE, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_CODE, Result.COURSE_CODE, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.SEMESTER_ID, Result.SEMESTER, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.IS_NME_SUBJECT, Result.IS_NME_SUBJECT, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.IS_ALLIED_SUBJECT, Result.IS_ALLIED_SUBJECT, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_ORDER, Result.COURSE_ORDER, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.SUBJECTS, Result.SUBJECTS, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.IS_COMPULSORY, Result.IS_COMPULSORY_SUBJECT, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.UGC_COURSE_TYPE, Result.UGC_COURSE_TYPE, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.PAPER_TYPE, Result.PAPER_TYPE, EnumCommand.DataType.String);
                        dv.Add(Common.CP_COURSE_ROOT_2017.SUBJECT_TYPE, Result.SUBJECT_TYPE, EnumCommand.DataType.String);
                        resultArg = objCourseRoot.InsertCourseRootInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                        sResult = resultArg.Success.ToString();


                        if (sResult == "True")
                        {
                            sResult = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                        {
                            sResult = "Enter Correct Values";
                        }
                    }
                    else
                    {
                        sResult = "Course Code is already exist..!";
                    }

                }
            }
            return Json(sResult);
        }
        public ActionResult DeleteCourseRoot(string id)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            using (AcademicsViewModel objCourseRoot = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, id, EnumCommand.DataType.String);
                resultArg = objCourseRoot.DeleteCourseRoot(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("ListCourseInfo", "Academics");
        }
        public ActionResult EditCourseRootInfo(string id)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            dv.Clear();
            dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, id, EnumCommand.DataType.String);
            DataTable dtFetchCourseRootById = new DataTable();
            Session[Common.SESSION_VARIABLES.COURSE_ROOT_ID] = id;
            CourseRootModel objCourseRoot = new CourseRootModel();

            DataTable dtDepartment = new DataTable();
            DataTable dtPart = new DataTable();
            DataTable dtCourseType = new DataTable();
            DataTable dtPaperCode = new DataTable();
            DataTable dtSemester = new DataTable();
            DataTable dtUGCCourseType = new DataTable();
            DataTable dtPaperType = new DataTable();
            DataTable dtSubjectType = new DataTable();

            dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            dtPart = SupportDataMethod.FetchPart().DataSource.Table;
            dtCourseType = SupportDataMethod.FetchCourseItems((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            dtPaperCode = SupportDataMethod.FetchPaperCode().DataSource.Table;
            dtSemester = SupportDataMethod.FetchSemester().DataSource.Table;
            dtUGCCourseType = SupportDataMethod.FetchUGCCoursetype().DataSource.Table;
            dtPaperType = SupportDataMethod.FetchPaperType().DataSource.Table;
            dtSubjectType = SupportDataMethod.FetchSubjectType().DataSource.Table;

            List<SupDepartment> liDepartment = new List<SupDepartment>();
            List<SUP_PART> liPart = new List<SUP_PART>();
            List<SupCourseType> liCourseType = new List<SupCourseType>();
            List<SupPaperCode> liPaperCode = new List<SupPaperCode>();
            List<SupSemester> liSemester = new List<SupSemester>();
            List<SupUGCCourseType> liUGCCourseType = new List<SupUGCCourseType>();
            List<SupPaperType> liPaperType = new List<SupPaperType>();
            List<SUP_SUBJECT_TYPE> liSupbjectType = new List<SUP_SUBJECT_TYPE>();

            if (dtDepartment != null && dtDepartment.Rows.Count > 0)
            {
                liDepartment = (from DataRow dr in dtDepartment.Rows
                                select new SupDepartment()
                                {
                                    DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                    DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                }).ToList();
                objCourseRoot.DEPARTMENT = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
            }

            if (dtPart != null && dtPart.Rows.Count > 0)
            {
                liPart = (from DataRow dr in dtPart.Rows
                          select new SUP_PART()
                          {
                              PART_ID = dr[Common.SUP_PART.PART_ID].ToString(),
                              PART = dr[Common.SUP_PART.PART].ToString()
                          }).ToList();
                objCourseRoot.PART = new SelectList(liPart, Common.SUP_PART.PART_ID, Common.SUP_PART.PART);
            }
            if (dtCourseType != null && dtCourseType.Rows.Count > 0)
            {
                liCourseType = (from DataRow dr in dtCourseType.Rows
                                select new SupCourseType()
                                {
                                    COURSE_TYPE_ID = dr[Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ID].ToString(),
                                    COURSE_TYPE = dr[Common.CP_COURSE_TYPE_2017.COURSE_TYPE].ToString()
                                }).ToList();
                objCourseRoot.COURSE_TYPE = new SelectList(liCourseType, Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ID, Common.CP_COURSE_TYPE_2017.COURSE_TYPE);
            }

            if (dtPaperCode != null && dtPaperCode.Rows.Count > 0)
            {
                liPaperCode = (from DataRow dr in dtPaperCode.Rows
                               select new SupPaperCode()
                               {
                                   PAPER_CODE_ID = dr[Common.SUP_PAPER_CODE.PAPER_CODE_ID].ToString(),
                                   PAPER_CODE = dr[Common.SUP_PAPER_CODE.PAPER_CODE].ToString()
                               }).ToList();
                objCourseRoot.PAPER_CODE = new SelectList(liPaperCode, Common.SUP_PAPER_CODE.PAPER_CODE_ID, Common.SUP_PAPER_CODE.PAPER_CODE);
            }
            if (dtSemester != null && dtSemester.Rows.Count > 0)
            {
                liSemester = (from DataRow dr in dtSemester.Rows
                              select new SupSemester()
                              {
                                  SUP_SEMESTER_ID = dr[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString(),
                                  SEMESTER_NAME = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString()
                              }).ToList();
                objCourseRoot.SEMESTER = new SelectList(liSemester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
            }

            if (dtUGCCourseType != null && dtUGCCourseType.Rows.Count > 0)
            {
                liUGCCourseType = (from DataRow dr in dtUGCCourseType.Rows
                                   select new SupUGCCourseType()
                                   {
                                       UGC_COURSE_TYPE_ID = dr[Common.CP_UGC_COURSE_TYPE.UGC_COURSE_TYPE_ID].ToString(),
                                       UGC_COURSE_TYPE = dr[Common.CP_UGC_COURSE_TYPE.UGC_COURSE_TYPE].ToString()
                                   }).ToList();
                objCourseRoot.UGC_COURSE_TYPE = new SelectList(liUGCCourseType, Common.CP_UGC_COURSE_TYPE.UGC_COURSE_TYPE_ID, Common.CP_UGC_COURSE_TYPE.UGC_COURSE_TYPE);
            }
            if (dtPaperType != null && dtPaperType.Rows.Count > 0)
            {
                liPaperType = (from DataRow dr in dtPaperType.Rows
                               select new SupPaperType()
                               {
                                   PAPER_TYPE_ID = dr[Common.SUP_PAPER_TYPE.PAPER_TYPE_ID].ToString(),
                                   PAPER_TYPE = dr[Common.SUP_PAPER_TYPE.PAPER_TYPE].ToString()
                               }).ToList();
                objCourseRoot.PAPER_TYPE = new SelectList(liPaperType, Common.SUP_PAPER_TYPE.PAPER_TYPE_ID, Common.SUP_PAPER_TYPE.PAPER_TYPE);
            }
            if (dtSubjectType != null && dtSubjectType.Rows.Count > 0)
            {
                liSupbjectType = (from DataRow dr in dtSubjectType.Rows
                                  select new SUP_SUBJECT_TYPE()
                                  {
                                      SUBJECT_TYPE_ID = dr[Common.SUP_SUBJECT_TYPE.SUBJECT_TYPE_ID].ToString(),
                                      SUBJECT_TYPE = dr[Common.SUP_SUBJECT_TYPE.SUBJECT_TYPE].ToString()
                                  }).ToList();
                objCourseRoot.SUBJECT_TYPE = new SelectList(liSupbjectType, Common.SUP_SUBJECT_TYPE.SUBJECT_TYPE_ID, Common.SUP_SUBJECT_TYPE.SUBJECT_TYPE);
            }

            return View(objCourseRoot);
        }
        public JsonResult FetchCourseRootInfo(string id)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            dv.Clear();
            dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, id, EnumCommand.DataType.String);
            DataTable dtFetchCourseRootById = new DataTable();
            AcademicsViewModel objCourseRootView = new AcademicsViewModel();
            dtFetchCourseRootById = objCourseRootView.FetchCourseRootInfoById(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            if (dtFetchCourseRootById != null && dtFetchCourseRootById.Rows.Count > 0)
            {
                CourseRootInfo objCourseRoot = new CourseRootInfo()
                {
                    DEPARTMENT = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.DEPARTMENT].ToString(),
                    YEAR_FROM = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.YEAR_FROM].ToString(),
                    YEAR_TO = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.YEAR_TO].ToString(),
                    SEMESTER_FROM = dtFetchCourseRootById.Rows[0]["SEMESTER_FROM"].ToString(),
                    SEMESTER_TO = dtFetchCourseRootById.Rows[0]["SEMESTER_TO"].ToString(),
                    YEAR = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.YEAR].ToString(),
                    PART = dtFetchCourseRootById.Rows[0]["PART"].ToString(),
                    COURSE_TYPE = dtFetchCourseRootById.Rows[0]["COURSE_TYPE"].ToString(),
                    HRS_PER_WEEK = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.HRS_PER_WEEK].ToString(),
                    CREDITS = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.CREDITS].ToString(),
                    OPTION_NAME = dtFetchCourseRootById.Rows[0]["OPTION_NAME"].ToString(),
                    PAPER_CODE = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.PAPER_CODE].ToString(),
                    COURSE_TITLE = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString(),
                    COURSE_CODE = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString(),
                    SEMESTER = dtFetchCourseRootById.Rows[0]["SEMESTER_ID"].ToString(),
                    IS_NME_SUBJECT = dtFetchCourseRootById.Rows[0]["IS_NME_SUBJECT"].ToString(),
                    IS_ALLIED_SUBJECT = dtFetchCourseRootById.Rows[0]["IS_ALLIED_SUBJECT"].ToString(),
                    COURSE_ORDER = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.COURSE_ORDER].ToString(),
                    SUBJECTS = dtFetchCourseRootById.Rows[0][Common.CP_COURSE_ROOT_2017.SUBJECTS].ToString(),
                    IS_COMPULSORY_SUBJECT = dtFetchCourseRootById.Rows[0]["IS_COMPULSORY"].ToString(),
                    UGC_COURSE_TYPE = dtFetchCourseRootById.Rows[0]["UGC_COURSE_TYPE"].ToString(),
                    PAPER_TYPE = dtFetchCourseRootById.Rows[0][Common.SUP_PAPER_TYPE.PAPER_TYPE].ToString(),
                    SUBJECT_TYPE = dtFetchCourseRootById.Rows[0][Common.SUP_SUBJECT_TYPE.SUBJECT_TYPE].ToString(),
                };
                return Json(objCourseRoot);
            }
            else
            {
                return Json(sResult);
            }

        }
        public ActionResult UpdateCourseRootInfo(string id)
        {
            JsonResultData objResult = new JsonResultData();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                var sCourseRootId = Session[Common.SESSION_VARIABLES.COURSE_ROOT_ID].ToString();
                if (id != null)
                {
                    var Result = JsonConvert.DeserializeObject<CourseRootInfo>(id);
                    dv.Clear();
                    dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, sCourseRootId, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.DEPARTMENT, Result.DEPARTMENT, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.YEAR_FROM, Result.YEAR_FROM, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.YEAR_TO, Result.YEAR_TO, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.YEAR, Result.YEAR, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.PART, Result.PART, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_TYPE, Result.COURSE_TYPE, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.HRS_PER_WEEK, Result.HRS_PER_WEEK, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.CREDITS, Result.CREDITS, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.PAPER_CODE, Result.PAPER_CODE, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_TITLE, Result.COURSE_TITLE, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_CODE, Result.COURSE_CODE, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.SEMESTER_ID, Result.SEMESTER, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.IS_NME_SUBJECT, Result.IS_NME_SUBJECT, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.IS_ALLIED_SUBJECT, Result.IS_ALLIED_SUBJECT, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.COURSE_ORDER, Result.COURSE_ORDER, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.SUBJECTS, Result.SUBJECTS, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.IS_COMPULSORY, Result.IS_COMPULSORY_SUBJECT, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.UGC_COURSE_TYPE, Result.UGC_COURSE_TYPE, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.PAPER_TYPE, Result.PAPER_TYPE, EnumCommand.DataType.String);
                    dv.Add(Common.CP_COURSE_ROOT_2017.SUBJECT_TYPE, Result.SUBJECT_TYPE, EnumCommand.DataType.String);
                    using (AcademicsViewModel objCourseRoot = new AcademicsViewModel())
                    {
                        resultArg = objCourseRoot.UpdateCourseRootInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                        sResult = resultArg.Success.ToString();
                        if (resultArg.Success)
                        {
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                    }
                }
                else
                {
                    objResult.Message = Common.ErrorMessage.NoContent;
                }

            }
            else
            {
                objResult.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(objResult, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region Course Wise Staff
        public ActionResult CourseWiseStaffInfo()
        {
            // var Genders = new List<ACADEMIC_YEAR>();
            //  var ac = new ACADEMIC_YEAR();
            // var result = DAO.CMSHandler.SaveOrUpdate(ac, AcademicsSQL.GetAcademicSQL(DAO.AcademicSQLCommands.UpdateAcademicInfo), string.Empty, false);
            //  Genders = new AcademicsViewModel().testFetchList();
            // Genders = (List<ACADEMIC_YEAR>)CMS.DAO.CMSHandler.FetchData<ACADEMIC_YEAR>(null, AcademicsSQL.GetAcademicSQL(DAO.AcademicSQLCommands.FetchAcademicYearInfo)).DataSource.Data;

            // ACADEMIC_YEAR obj = Genders.Where(u => u.ACADEMIC_YEAR_ID.Equals("1")).FirstOrDefault();
            //Genders = (List<ACADEMIC_YEAR>)cont;
            //(null,AcademicsSQL.GetAcademicSQL(DAO.AcademicSQLCommands.FetchClassCourseList));
            CourseWiseStaffModel objCourseWiseStaff = new CourseWiseStaffModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtDepartment = new DataTable();
                DataTable dtClass = new DataTable();
                DataTable dtCourse = new DataTable();
                DataTable dtStaff = new DataTable();
                DataTable dtShift = new DataTable();
                DataTable dtSemester = new DataTable();
                DataTable dtClasstype = new DataTable();

                dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
                dtCourse = SupportDataMethod.FetchCourses((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                dtStaff = SupportDataMethod.FetchStaff().DataSource.Table;
                dtShift = SupportDataMethod.FetchShift().DataSource.Table;
                dtSemester = SupportDataMethod.FetchSemester().DataSource.Table;
                dtClasstype = SupportDataMethod.FetchClassType().DataSource.Table;

                List<SupDepartment> liDepartment = new List<SupDepartment>();
                List<cp_Classes_2017> liClass = new List<cp_Classes_2017>();
                List<cp_Course_Root_2017> liCourse = new List<cp_Course_Root_2017>();
                List<stf_Personal_Info> liStaff = new List<stf_Personal_Info>();
                List<Sup_Shift> liShit = new List<Sup_Shift>();
                List<SupSemester> liSemester = new List<SupSemester>();
                List<SupClassType> liClassType = new List<SupClassType>();

                if (dtDepartment != null && dtDepartment.Rows.Count > 0)
                {
                    liDepartment = (from DataRow dr in dtDepartment.Rows
                                    select new SupDepartment()
                                    {
                                        DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                        DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                    }).ToList();
                    objCourseWiseStaff.DEPARTMENT = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
                }

                if (dtClass != null && dtClass.Rows.Count > 0)
                {
                    liClass = (from DataRow dr in dtClass.Rows
                               select new cp_Classes_2017()
                               {
                                   CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                                   CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                               }).ToList();
                    objCourseWiseStaff.CLASS_ID = new SelectList(liClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                if (dtClasstype != null && dtClasstype.Rows.Count > 0)
                {
                    liClassType = (from DataRow dr in dtClasstype.Rows
                                   select new SupClassType()
                                   {
                                       CLASS_TYPE_ID = dr[Common.SUP_CLASS_TYPE.CLASS_TYPE_ID].ToString(),
                                       CLASS_TYPE = dr[Common.SUP_CLASS_TYPE.CLASS_TYPE].ToString()
                                   }).ToList();
                    objCourseWiseStaff.CLASS_TYPE = new SelectList(liClassType, Common.SUP_CLASS_TYPE.CLASS_TYPE_ID, Common.SUP_CLASS_TYPE.CLASS_TYPE);
                }
                if (dtCourse != null && dtCourse.Rows.Count > 0)
                {
                    liCourse = (from DataRow dr in dtCourse.Rows
                                select new cp_Course_Root_2017()
                                {
                                    COURSE_ROOT_ID = dr[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID].ToString(),
                                    COURSE_CODE = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString()
                                }).ToList();
                    objCourseWiseStaff.COURSE_ID = new SelectList(liCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_CODE);
                }
                if (dtStaff != null && dtStaff.Rows.Count > 0)
                {
                    liStaff = (from DataRow dr in dtStaff.Rows
                               select new stf_Personal_Info()
                               {
                                   STAFF_ID = dr[Common.STF_PERSONAL_INFO.STAFF_ID].ToString(),
                                   STAFF_CODE = dr["STAFF_NAME"].ToString()
                               }).ToList();
                    objCourseWiseStaff.STAFF_ID = new SelectList(liStaff, Common.STF_PERSONAL_INFO.STAFF_ID, Common.STF_PERSONAL_INFO.STAFF_CODE);
                }
                if (dtShift != null && dtShift.Rows.Count > 0)
                {
                    liShit = (from DataRow dr in dtShift.Rows
                              select new Sup_Shift()
                              {
                                  SHIFT_ID = dr[Common.SUP_SHIFT.SHIFT_ID].ToString(),
                                  SHIFT_NAME = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString()
                              }).ToList();
                    objCourseWiseStaff.SHIFT = new SelectList(liShit, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                }
                if (dtSemester != null && dtSemester.Rows.Count > 0)
                {
                    liSemester = (from DataRow dr in dtSemester.Rows
                                  select new SupSemester()
                                  {
                                      SUP_SEMESTER_ID = dr[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString(),
                                      SEMESTER_NAME = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString()
                                  }).ToList();
                    objCourseWiseStaff.SEMESTER_ID = new SelectList(liSemester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Error");
            }

            return View(objCourseWiseStaff);
        }
        [UserRights("ADMIN")]
        public ActionResult ListCourseWiseStaffInfo()
        {
            AcademicsViewModel objCourseWiseStaff = new AcademicsViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtCourseWiseStaff = new DataTable();
            dtCourseWiseStaff = objCourseWiseStaff.FetchCourseWiseStaff((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<CourseWiseStaffInfo> liCourseWiseStaff = new List<CourseWiseStaffInfo>();
            if (dtCourseWiseStaff != null && dtCourseWiseStaff.Rows.Count > 0)
            {
                liCourseWiseStaff = (from DataRow dr in dtCourseWiseStaff.Rows
                                     select new CourseWiseStaffInfo()
                                     {
                                         CLSCRSSTF_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.CLSCRSSTF_ID].ToString(),
                                         CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                                         CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString(),
                                         COURSE_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID].ToString(),
                                         COURSE_CODE = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString(),
                                         COURSE_TITLE = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString(),
                                         STAFF_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.STAFF_NAME].ToString(),
                                         STAFF_COUNT = dr[Common.CP_CLASS_COURSE_STAFF_2017.STAFF_COUNT].ToString(),
                                         SEMESTER_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.SEMESTER_ID].ToString(),
                                         SHIFT = dr[Common.CP_CLASS_COURSE_STAFF_2017.SHIFT].ToString(),
                                     }).ToList();
            }
            return View(liCourseWiseStaff);
        }
        public JsonResult InsertCourseWiseStaff(string sJsonCourseWiseStaff)
        {
            string Result = string.Empty;
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            JsonCourseWiseStaff objJson = JsonConvert.DeserializeObject<JsonCourseWiseStaff>(sJsonCourseWiseStaff);
            AcademicsViewModel objCourseWiseStaff = new AcademicsViewModel();
            resultArg = objCourseWiseStaff.BulkSaveCourseWiseStaff(objJson, Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
            if (resultArg.Success)
            {
                sResult = Common.Messages.RecordsSavedSuccessfully;
            }
            else
            {
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return Json(sResult);
        }
        public ActionResult DeleteCourseWiseStaffInfo(string id)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            using (AcademicsViewModel objCourseWiseStaff = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLSCRSSTF_ID, id, EnumCommand.DataType.String);
                resultArg = objCourseWiseStaff.DeleteCourseWiseStaffINfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("ListCourseWiseStaffInfo", "Academics");
        }
        public string GetClassListByClassTypeId(string sDepartment, string sShift, string sClassType)
        {
            string sOption = string.Empty;
            DataTable dtClassList = new DataTable();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            using (AcademicsViewModel objAcademicsview = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_CLASSES_2017.DEPARTMENT, sDepartment, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.SHIFT, sShift, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.CLASS_TYPE, sClassType, EnumCommand.DataType.String);
                dtClassList = objAcademicsview.GetClassListByClassTypeId(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                foreach (DataRow item in dtClassList.Rows)
                {
                    sOption += "<option value='" + item[Common.CP_CLASSES_2017.CLASS_ID].ToString() + "' >" + item[Common.CP_CLASSES_2017.CLASS_CODE].ToString() + "</option>";
                }
            }
            return sOption;
        }
        public string GetClassTypeByShiftId(string sShift)
        {
            string sOption = string.Empty;
            DataTable dtClassType = new DataTable();
            using (AcademicsViewModel objAcademicsview = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_CLASSES_2017.DEPARTMENT, sShift, EnumCommand.DataType.String);
                dtClassType = objAcademicsview.GetClassTypeByShiftId(dv).DataSource.Table;
                foreach (DataRow item in dtClassType.Rows)
                {
                    sOption += "<option value='" + item[Common.SUP_CLASS_TYPE.CLASS_TYPE_ID].ToString() + "' >" + item[Common.SUP_CLASS_TYPE.CLASS_TYPE].ToString() + "</option>";
                }
            }
            return sOption;
        }
        public ActionResult EditCourseWiseStaff(string id)
        {
            CourseWiseStaffModel objCourseWiseStaff = new CourseWiseStaffModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                dv.Clear();
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLSCRSSTF_ID, id, EnumCommand.DataType.String);
                DataTable dtFetchCourseWiseStaffInfoById = new DataTable();
                Session[Common.SESSION_VARIABLES.CLSCRSSTF_ID] = id;

                DataTable dtClass = new DataTable();
                DataTable dtCourse = new DataTable();
                DataTable dtStaff = new DataTable();
                DataTable dtShift = new DataTable();
                DataTable dtSemester = new DataTable();

                dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
                dtCourse = SupportDataMethod.FetchCourses((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                dtStaff = SupportDataMethod.FetchStaff().DataSource.Table;
                dtShift = SupportDataMethod.FetchShift().DataSource.Table;
                dtSemester = SupportDataMethod.FetchSemester().DataSource.Table;

                List<cp_Classes_2017> liClass = new List<cp_Classes_2017>();
                List<cp_Course_Root_2017> liCourse = new List<cp_Course_Root_2017>();
                List<stf_Personal_Info> liStaff = new List<stf_Personal_Info>();
                List<Sup_Shift> liShit = new List<Sup_Shift>();
                List<SupSemester> liSemester = new List<SupSemester>();

                if (dtClass != null && dtClass.Rows.Count > 0)
                {
                    liClass = (from DataRow dr in dtClass.Rows
                               select new cp_Classes_2017()
                               {
                                   CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                                   CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                               }).ToList();
                    objCourseWiseStaff.CLASS_ID = new SelectList(liClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                if (dtCourse != null && dtCourse.Rows.Count > 0)
                {
                    liCourse = (from DataRow dr in dtCourse.Rows
                                select new cp_Course_Root_2017()
                                {
                                    COURSE_ROOT_ID = dr[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID].ToString(),
                                    COURSE_CODE = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString()
                                }).ToList();
                    objCourseWiseStaff.COURSE_ID = new SelectList(liCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_CODE);
                }
                if (dtStaff != null && dtStaff.Rows.Count > 0)
                {
                    liStaff = (from DataRow dr in dtStaff.Rows
                               select new stf_Personal_Info()
                               {
                                   STAFF_ID = dr[Common.STF_PERSONAL_INFO.STAFF_ID].ToString(),
                                   STAFF_CODE = dr["STAFF_NAME"].ToString()
                               }).ToList();
                    objCourseWiseStaff.STAFF_ID = new SelectList(liStaff, Common.STF_PERSONAL_INFO.STAFF_ID, Common.STF_PERSONAL_INFO.STAFF_CODE);
                }
                if (dtShift != null && dtShift.Rows.Count > 0)
                {
                    liShit = (from DataRow dr in dtShift.Rows
                              select new Sup_Shift()
                              {
                                  SHIFT_ID = dr[Common.SUP_SHIFT.SHIFT_ID].ToString(),
                                  SHIFT_NAME = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString()
                              }).ToList();
                    objCourseWiseStaff.SHIFT = new SelectList(liShit, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                }
                if (dtSemester != null && dtSemester.Rows.Count > 0)
                {
                    liSemester = (from DataRow dr in dtSemester.Rows
                                  select new SupSemester()
                                  {
                                      SUP_SEMESTER_ID = dr[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString(),
                                      SEMESTER_NAME = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString()
                                  }).ToList();
                    objCourseWiseStaff.SEMESTER_ID = new SelectList(liSemester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
                }
            }

            return View(objCourseWiseStaff);
        }
        public JsonResult FetchCourseWiseStaffById(string CourseWiseStaffId)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            dv.Clear();
            dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLSCRSSTF_ID, CourseWiseStaffId, EnumCommand.DataType.String);
            Session[Common.SESSION_VARIABLES.CLSCRSSTF_ID] = CourseWiseStaffId;
            DataTable dtFetchClassWiseStaffById = new DataTable();
            AcademicsViewModel objCourseWiseStaff = new AcademicsViewModel();
            dtFetchClassWiseStaffById = objCourseWiseStaff.FetchCourseWiseStaffById(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            if (dtFetchClassWiseStaffById != null && dtFetchClassWiseStaffById.Rows.Count > 0)
            {
                CourseWiseStaffInfo objCourseWiseStaffInfo = new CourseWiseStaffInfo()
                {
                    CLSCRSSTF_ID = dtFetchClassWiseStaffById.Rows[0][Common.CP_CLASS_COURSE_STAFF_2017.CLSCRSSTF_ID].ToString(),
                    CLASS_ID = dtFetchClassWiseStaffById.Rows[0][Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID].ToString(),
                    COURSE_ID = dtFetchClassWiseStaffById.Rows[0][Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID].ToString(),
                    STAFF_ID = dtFetchClassWiseStaffById.Rows[0][Common.CP_CLASS_COURSE_STAFF_2017.STAFF_ID].ToString(),
                    HRS_WEEK = dtFetchClassWiseStaffById.Rows[0][Common.CP_CLASS_COURSE_STAFF_2017.HRS_WEEK].ToString(),
                    HRS_TERM = dtFetchClassWiseStaffById.Rows[0][Common.CP_CLASS_COURSE_STAFF_2017.HRS_TERM].ToString(),
                    SHIFT = dtFetchClassWiseStaffById.Rows[0][Common.CP_CLASS_COURSE_STAFF_2017.SHIFT].ToString(),
                    SEMESTER_ID = dtFetchClassWiseStaffById.Rows[0][Common.CP_CLASS_COURSE_STAFF_2017.SEMESTER_ID].ToString()
                };
                return Json(objCourseWiseStaffInfo);
            }
            else
            {
                return Json(sResult);
            }
        }
        public ActionResult FetchSelectedCourseWiseStaff(string sCourseId, string sClassId)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            dv.Clear();
            dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID, sCourseId, EnumCommand.DataType.String);
            dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
            DataTable dtFetchSelectedCourseStaff = new DataTable();
            AcademicsViewModel objSelectedCourseWiseStaff = new AcademicsViewModel();
            dtFetchSelectedCourseStaff = objSelectedCourseWiseStaff.FetchSelectedCourseWiseStaff(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<CourseWiseStaffInfo> liSelectedCourseWiseStaff = new List<CourseWiseStaffInfo>();
            liSelectedCourseWiseStaff = (from DataRow dr in dtFetchSelectedCourseStaff.Rows
                                         select new CourseWiseStaffInfo()
                                         {
                                             CLSCRSSTF_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.CLSCRSSTF_ID].ToString(),
                                             CLASS_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID].ToString(),
                                             CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString(),
                                             COURSE_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID].ToString(),
                                             COURSE_CODE = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString(),
                                             COURSE_TITLE = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString(),
                                             STAFF_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.STAFF_NAME].ToString(),
                                             //STAFF_COUNT = dr[Common.CP_CLASS_COURSE_STAFF_2017.STAFF_COUNT].ToString(),
                                             SEMESTER_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.SEMESTER_ID].ToString(),
                                             SHIFT = dr[Common.CP_CLASS_COURSE_STAFF_2017.SHIFT].ToString(),
                                         }).ToList();
            return View(liSelectedCourseWiseStaff);

        }
        public string EditSelectedCourseWiseStaff(string CourseWiseStaffId)
        {
            string sClass = string.Empty;
            string sCourse = string.Empty;
            string sStaff = string.Empty;
            string sShift = string.Empty;
            string sSemester = string.Empty;
            string JsonData = string.Empty;

            CourseWiseStaffModel objSelectedCourseWiseStaff = new CourseWiseStaffModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtClass = new DataTable();
            DataTable dtCourse = new DataTable();
            DataTable dtStaff = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtSemester = new DataTable();

            dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
            dtCourse = SupportDataMethod.FetchCourses((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            dtStaff = SupportDataMethod.FetchStaff().DataSource.Table;
            dtShift = SupportDataMethod.FetchShift().DataSource.Table;
            dtSemester = SupportDataMethod.FetchSemester().DataSource.Table;

            List<cp_Classes_2017> liClass = new List<cp_Classes_2017>();
            List<cp_Course_Root_2017> liCourse = new List<cp_Course_Root_2017>();
            List<stf_Personal_Info> liStaff = new List<stf_Personal_Info>();
            List<Sup_Shift> liShit = new List<Sup_Shift>();
            List<SupSemester> liSemester = new List<SupSemester>();

            if (dtClass != null && dtClass.Rows.Count > 0)
            {
                foreach (DataRow item in dtClass.Rows)
                {
                    sClass += "<option value='" + item[Common.CP_CLASSES_2017.CLASS_ID].ToString() + "'>" + item[Common.CP_CLASSES_2017.CLASS_NAME].ToString() + "</option>";
                }
            }
            if (dtCourse != null && dtCourse.Rows.Count > 0)
            {
                foreach (DataRow item in dtCourse.Rows)
                {
                    sCourse += "<option value='" + item[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID].ToString() + "'>" + item[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString() + "</option>";
                }
            }
            if (dtStaff != null && dtStaff.Rows.Count > 0)
            {
                foreach (DataRow item in dtStaff.Rows)
                {
                    sStaff += "<option value='" + item[Common.STF_PERSONAL_INFO.STAFF_ID].ToString() + "'>" + item[Common.STF_PERSONAL_INFO.STAFF_NAME].ToString() + "</option>";
                }
            }
            if (dtShift != null && dtShift.Rows.Count > 0)
            {
                foreach (DataRow item in dtShift.Rows)
                {
                    sShift += "<option value='" + item[Common.SUP_SHIFT.SHIFT_ID].ToString() + "'>" + item[Common.SUP_SHIFT.SHIFT_NAME].ToString() + "</option>";
                }
            }
            if (dtSemester != null && dtSemester.Rows.Count > 0)
            {
                foreach (DataRow item in dtSemester.Rows)
                {
                    sSemester += "<option value='" + item[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString() + "'>" + item[Common.SUP_SEMESTER.SEMESTER_NAME].ToString() + "</option>";
                }
            }
            var objJsonData = new CourseWiseStaffInfo() { CLASS_ID = sClass, COURSE_ID = sCourse, STAFF_ID = sStaff, SHIFT = sShift, SEMESTER_ID = sSemester };
            JsonData = JsonConvert.SerializeObject(objJsonData);
            return JsonData;
        }
        public ActionResult UpdateSelectedCourseWiseStaffInfo(string jsonSelectedCourseWiseStaff)
        {
            string sSelectedCourseWiseStaffId = Session[Common.SESSION_VARIABLES.CLSCRSSTF_ID].ToString();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            if (jsonSelectedCourseWiseStaff != null)
            {
                var Result = JsonConvert.DeserializeObject<CourseWiseStaffInfo>(jsonSelectedCourseWiseStaff);
                dv.Clear();
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLSCRSSTF_ID, sSelectedCourseWiseStaffId, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID, Result.CLASS_ID, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID, Result.COURSE_ID, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.STAFF_ID, Result.STAFF_ID, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.HRS_WEEK, Result.HRS_WEEK, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.HRS_TERM, Result.HRS_TERM, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.SHIFT, Result.SHIFT, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.SEMESTER_ID, Result.SEMESTER_ID, EnumCommand.DataType.String);
                using (AcademicsViewModel objDepartment = new AcademicsViewModel())
                {
                    resultArg = objDepartment.UpdateSelectedCourseWiseStaffInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                    sResult = resultArg.Success.ToString();
                }
            }
            return RedirectToAction("FetchSelectedCourseWiseStaff", "Academics");
        }
        public ActionResult BindStaffsbyClassId(string sClassId, string sDepartment, string sShift, string sClassType)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtCourseStaff = new DataTable();
            DataTable dtSemester = new DataTable();
            DataTable dtStaffList = new DataTable();
            dtStaffList = SupportDataMethod.FetchStaff().DataSource.Table;
            dtSemester = SupportDataMethod.FetchSemester().DataSource.Table;
            List<stf_Personal_Info> liStaff = new List<stf_Personal_Info>();
            List<SupSemester> liSemester = new List<SupSemester>();
            using (AcademicsViewModel objCourseWiseStaff = new AcademicsViewModel())
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
                    dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT_ID, sDepartment, EnumCommand.DataType.String);
                    dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.SHIFT, sShift, EnumCommand.DataType.String);
                    dv.Add(Common.CP_CLASSES_2017.CLASS_TYPE, sClassType, EnumCommand.DataType.String);
                    dtCourseStaff = objCourseWiseStaff.FetchCourseWiseStaffInfoByClassId(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                    if (dtSemester != null && dtSemester.Rows.Count > 0)
                    {
                        liSemester = (from DataRow dr in dtSemester.Rows
                                      select new SupSemester()
                                      {
                                          SUP_SEMESTER_ID = dr[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString(),
                                          SEMESTER_NAME = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString()
                                      }).ToList();
                        objCourseWiseStaff.Semester_List = liSemester;
                    }
                    if (dtStaffList != null && dtStaffList.Rows.Count > 0)
                    {
                        liStaff = (from DataRow dr in dtStaffList.Rows
                                   select new stf_Personal_Info()
                                   {
                                       STAFF_ID = dr[Common.STF_PERSONAL_INFO.STAFF_ID].ToString(),
                                       STAFF_CODE = dr[Common.STF_PERSONAL_INFO.STAFF_NAME].ToString(),
                                   }).ToList();
                        objCourseWiseStaff.Staff_List = liStaff;
                    }
                    //IList<CourseWiseStaffInfo> liList = new List<CourseWiseStaffInfo>();
                    if (dtCourseStaff != null && dtCourseStaff.Rows.Count > 0)
                    {
                        objCourseWiseStaff.objCourseWiseStaff = (from DataRow dr in dtCourseStaff.Rows
                                                                 select new CourseWiseStaffInfo()
                                                                 {
                                                                     //CLSCRSSTF_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.CLSCRSSTF_ID].ToString(),
                                                                     CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                                                                     //SHIFT = dr["SHIFT_NAME"].ToString(),
                                                                     STAFF_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.STAFF_NAME].ToString(),
                                                                     COURSE_ID = dr[Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID].ToString(),
                                                                     COURSE_CODE = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString(),
                                                                     COURSE_TITLE = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString(),
                                                                     SEMESTER = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString(),
                                                                     SEMESTER_ID = dr[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString(),
                                                                     HRS_WEEK = dr[Common.CP_CLASS_COURSE_STAFF_2017.HRS_WEEK].ToString(),
                                                                     HRS_TERM = dr[Common.CP_CLASS_COURSE_STAFF_2017.HRS_TERM].ToString()
                                                                 }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("Academics", "BindStaffsbyClassId", ex.Message);
                    }
                }

                return View(objCourseWiseStaff);
            }
        }



        #endregion
        #region Academic Curriculum
        [UserRights("ADMIN")]
        public ActionResult ListAcademicCurriculumInfo()
        {

            AcademicCurriculumViewModel objAcademicViewModel = new AcademicCurriculumViewModel();
            string sSelectedValue = string.Empty;
            DataTable dtAcademicYear = new DataTable();

            dtAcademicYear = SupportDataMethod.FetchAcademicYearList().DataSource.Table;
            IList<SUP_ACADEMIC_YEAR_LIST> liSupAcademicYear = new List<SUP_ACADEMIC_YEAR_LIST>();
            liSupAcademicYear = (from DataRow dr in dtAcademicYear.Rows
                                 select new SUP_ACADEMIC_YEAR_LIST()
                                 {
                                     ACADEMIC_YEAR_ID = dr[Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID].ToString(),
                                     ACTIVE_YEAR = dr[Common.ACADEMIC_YEAR.ACTIVE_YEAR].ToString(),
                                     AC_YEAR = dr[Common.ACADEMIC_YEAR.AC_YEAR].ToString()
                                 }).ToList();

            DataRow[] drArray = dtAcademicYear.Select(Common.ACADEMIC_YEAR.ACTIVE_YEAR + "=1");
            sSelectedValue = drArray[0][Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID].ToString();
            objAcademicViewModel.ddlAcademicYearList = new SelectList(liSupAcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR, sSelectedValue);

            return View(objAcademicViewModel);
        }

        public ActionResult listAcademicCurriculum(string sAademicYear)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                AcademicsViewModel objAcademicCurriculumview = new AcademicsViewModel();
                DataTable dtAcademicCurriculum = new DataTable();
                dv.Clear();
                dv.Add(Common.ACADEMIC_YEAR.AC_YEAR, (string.IsNullOrEmpty(sAademicYear) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : sAademicYear), EnumCommand.DataType.String);
                objAcademicCurriculumview.dv = dv;
                dtAcademicCurriculum = objAcademicCurriculumview.FetchAcademicCurriculumInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                IList<AcademicCurriculumInfo> liAcademicCurriculum = new List<AcademicCurriculumInfo>();
                if (dtAcademicCurriculum != null && dtAcademicCurriculum.Rows.Count > 0)
                {
                    liAcademicCurriculum = (from DataRow dr in dtAcademicCurriculum.Rows
                                            select new AcademicCurriculumInfo()
                                            {
                                                CURRICULUM_ID = dr[Common.CP_ACADEMIC_CURRICULUM_2017.CURRICULUM_ID].ToString(),
                                                PROGRAMME = dr["PROGRAMME_CODE"].ToString(),
                                                BATCH = dr[Common.CP_ACADEMIC_CURRICULUM_2017.BATCH].ToString(),
                                                COURSE_ID = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString(),
                                                //S_NO=dr[Common.CP_ACADEMIC_CURRICULUM_2017.S_NO].ToString(),
                                                SEMESTER = dr["SEMESTER_NAME"].ToString()
                                            }).ToList();

                }
                return PartialView(liAcademicCurriculum);
            }
            return RedirectToAction("Index", "Account");
        }
        /// <summary>
        /// this action for add page
        /// </summary>
        /// <returns></returns>
        public ActionResult AcademicCurriculum()
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            AcademicCurriculumModel objAcademicCurriculum = new AcademicCurriculumModel();
            AcademicsViewModel objAcademicViewModel = new AcademicsViewModel();
            DataTable dtBatch = new DataTable();
            DataTable dtProgramme = new DataTable();
            DataTable dtCourseList = new DataTable();
            DataTable dtSemester = new DataTable();

            dtBatch = SupportDataMethod.FetchActiveBatch().DataSource.Table;
            dtProgramme = SupportDataMethod.FetchProgram((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            // dtCourseList = objAcademicViewModel.FetchCourseBySemester()
            dtSemester = SupportDataMethod.FetchSemester().DataSource.Table;

            List<sup_Batch> liBatch = new List<sup_Batch>();
            List<cp_Program_2017> liProgramme = new List<cp_Program_2017>();
            List<cp_Course_Root_2017> liCourse = new List<cp_Course_Root_2017>();
            List<SupSemester> liSemester = new List<SupSemester>();

            if (dtBatch != null && dtBatch.Rows.Count > 0)
            {
                liBatch = (from DataRow dr in dtBatch.Rows
                           select new sup_Batch()
                           {
                               BATCH_ID = dr[Common.SUP_BATCHES.BATCH_ID].ToString(),
                               BATCH = dr[Common.SUP_BATCHES.BATCH].ToString()
                           }).ToList();
                objAcademicCurriculum.BATCH = new SelectList(liBatch, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
            }
            if (dtProgramme != null && dtProgramme.Rows.Count > 0)
            {
                liProgramme = (from DataRow dr in dtProgramme.Rows
                               select new cp_Program_2017()
                               {
                                   PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                   PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                               }).ToList();
                objAcademicCurriculum.PROGRAMME = new SelectList(liProgramme, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
            }

            liCourse.Add(new cp_Course_Root_2017() { COURSE_ROOT_ID = "0", COURSE_CODE = "---Select---" });
            objAcademicCurriculum.COURSE_LIST = new SelectList(liCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_CODE);
            if (dtSemester != null && dtSemester.Rows.Count > 0)
            {
                liSemester = (from DataRow dr in dtSemester.Rows
                              select new SupSemester()
                              {
                                  SUP_SEMESTER_ID = dr[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString(),
                                  SEMESTER_NAME = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString()
                              }).ToList();
                objAcademicCurriculum.SEMESTER = new SelectList(liSemester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
            }
            return View(objAcademicCurriculum);
        }

        public string SaveAcademicCurriculum(string sCurriculumCourse)
        {
            string sResult = string.Empty;
            JSonCurriculumCourse objJSonCurriculumCourse = JsonConvert.DeserializeObject<JSonCurriculumCourse>(sCurriculumCourse);
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                AcademicsViewModel objAcademicsViewModel = new AcademicsViewModel();
                resultArg = objAcademicsViewModel.BulkSaveCurriculum(objJSonCurriculumCourse, Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
                if (resultArg.Success)
                    sResult = Common.Messages.RecordsSavedSuccessfully;
                else
                    sResult = Common.Messages.FailedToSaveRecords;
            }
            else
            {
                sResult = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return sResult;
        }
        public ActionResult BindCoursesById(string sBatchId, string sProgrammeIds)
        {
            DataTable dtCurriculumCourse = new DataTable();
            using (AcademicsViewModel objCurriculum = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.BATCH, sBatchId, EnumCommand.DataType.String);
                dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.PROGRAMME, sProgrammeIds, EnumCommand.DataType.String);
                dtCurriculumCourse = objCurriculum.FetchCurriculumInfoById(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                if (dtCurriculumCourse != null && dtCurriculumCourse.Rows.Count > 0)
                {
                    objCurriculum.objCourseCurriculum = (from DataRow dr in dtCurriculumCourse.Rows
                                                         select new AcademicCurriculumInfo()
                                                         {
                                                             CURRICULUM_ID = dr[Common.CP_ACADEMIC_CURRICULUM_2017.CURRICULUM_ID].ToString(),
                                                             BATCH = dr[Common.CP_ACADEMIC_CURRICULUM_2017.BATCH].ToString(),
                                                             PROGRAMME = dr["PROGRAMME_CODE"].ToString(),
                                                             COURSE_ID = dr["COURSE_CODE"].ToString(),
                                                             SEMESTER = dr["SEMESTER_NAME"].ToString()
                                                         }).ToList();
                }
                return View(objCurriculum);
            }

        }
        public ActionResult DeleteCurriculumInfo(string id)
        {
            string sResult = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                using (AcademicsViewModel objCurriculum = new AcademicsViewModel())
                {
                    dv.Clear();
                    dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.CURRICULUM_ID, id, EnumCommand.DataType.String);
                    resultArg = objCurriculum.DeleteCurriculumInfo(dv, Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
                    if (resultArg.Success)
                    {
                        TempData["DeleteSuccess"] = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        TempData["DeleteSuccess"] = Common.Messages.RecordsSavedSuccessfully;

                    }
                }
            }
            else
            {
                TempData["DeleteSuccess"] = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return RedirectToAction("listacademiccurriculuminfo", "academics");
        }

        public string FetchCourseListBySemester(string sBatchId, string sProgrammeId)
        {
            string sResult = string.Empty;
            DataTable dtCourseList = new DataTable();
            dv.Clear();
            dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.BATCH, sBatchId, EnumCommand.DataType.String);
            dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.PROGRAMME, sProgrammeId, EnumCommand.DataType.String);

            using (AcademicsViewModel objViewModel = new AcademicsViewModel())
            {
                dtCourseList = objViewModel.FetchCourseBySemester(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                if (dtCourseList != null && dtCourseList.Rows.Count > 0)
                {
                    foreach (DataRow item in dtCourseList.Rows)
                    {
                        sResult += "<option value='" + item[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID] + "' " + item["SELECTED"] + " >" + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE] + "</option>";
                    }

                }
            }

            return sResult;

        }
        public ActionResult SaveOrUpdateCurriculum(string str)
        {
            if (str != null)
            {
                string sResult = string.Empty;
                string sInsertQuery = string.Empty;
                string sUpdateQuery = string.Empty;
                string sInsertQuerycom = string.Empty;
                var Result = JsonConvert.DeserializeObject<AcademicCurriculumInfo>(str);
                sInsertQuery = @"INSERT INTO ACADEMIC_CURRICULUM_2017(PROGRAMME,BATCH,COURSE_ID,IS_ACTIVE,SEMESTER)VALUES";
                sInsertQuerycom = sInsertQuery;

            }
            return RedirectToAction("ListAcademicCurriculumInfo", "Academics");
        }
        #endregion
        #region Semester Root
        [UserRights("ADMIN")]
        public ActionResult ListSemesterRoot()
        {
            AcademicsViewModel objAcademicyear = new AcademicsViewModel();
            DataTable dtSemesterRoot = new DataTable();
            dtSemesterRoot = objAcademicyear.FetchSemesterRootInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<SemesterRootInfo> liList = new List<SemesterRootInfo>();
            if (dtSemesterRoot != null && dtSemesterRoot.Rows.Count > 0)
            {
                liList = (from DataRow dr in dtSemesterRoot.Rows
                          select new SemesterRootInfo
                          {
                              SEMESTER_ID = dr[Common.CP_SEMESTER_ROOT_2017.ACC_SEMESTER_ID].ToString(),
                              BATCH = dr[Common.CP_SEMESTER_ROOT_2017.BATCH].ToString(),
                              PROGRAMME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString(),
                              DATE_FROM = dr[Common.CP_SEMESTER_ROOT_2017.DATE_FROM].ToString(),
                              DATE_TO = dr[Common.CP_SEMESTER_ROOT_2017.DATE_TO].ToString(),
                              IS_ACTIVE = dr[Common.CP_SEMESTER_ROOT_2017.IS_ACTIVE].ToString(),
                              SEMESTER = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString(),
                          }).ToList();
            }
            return View(liList);
        }
        //public string UpdateActiveSemesterRoot(string sActiveSemesterId)
        //{
        //    if (sActiveSemesterId != null)
        //    {
        //        DataTable dt = new DataTable();
        //        dv.Clear();
        //        dv.Add(Common.CP_SEMESTER_ROOT_2017.SEMESTER_ID, sActiveSemesterId, EnumCommand.DataType.String);
        //        using (AcademicsViewModel objAcademicYearView = new AcademicsViewModel())
        //        {
        //            resultArg = objAcademicYearView.UpdateActiveSemester(dv);
        //            dt = objAcademicYearView.FetchSemesterRootInfo().DataSource.Table;
        //            sResult = resultArg.Success.ToString();
        //        }
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            SemesterRootInfo objAcademicYear = new SemesterRootInfo()
        //            {
        //                BATCH = dt.Rows[0]["BATCH"].ToString(),
        //                PROGRAMME = dt.Rows[0]["PROGRAMME_NAME"].ToString(),
        //                DATE_FROM = dt.Rows[0][Common.CP_SEMESTER_ROOT_2017.DATE_FROM].ToString(),
        //                DATE_TO = dt.Rows[0][Common.CP_SEMESTER_ROOT_2017.DATE_TO].ToString(),
        //                IS_ACTIVE = dt.Rows[0][Common.CP_SEMESTER_ROOT_2017.IS_ACTIVE].ToString(),
        //                SEMESTER = dt.Rows[0]["SEMESTER_NAME"].ToString()
        //            };
        //        }
        //    }

        //    return sResult;
        //}
        public ActionResult SemesterRootInfo()
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            SemesterRootModel objSemesterRoot = new SemesterRootModel();
            DataTable dtBatch = new DataTable();
            DataTable dtProgramme = new DataTable();
            DataTable dtSemester = new DataTable();

            dtBatch = SupportDataMethod.FetchBatch().DataSource.Table;
            dtProgramme = SupportDataMethod.FetchProgram((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            dtSemester = SupportDataMethod.FetchSemester().DataSource.Table;

            IList<sup_Batch> liBatch = new List<sup_Batch>();
            IList<cp_Program_2017> liProgramme = new List<cp_Program_2017>();
            IList<SupSemester> liSemester = new List<SupSemester>();

            if (dtBatch != null && dtBatch.Rows.Count > 0)
            {
                liBatch = (from DataRow dr in dtBatch.Rows
                           select new sup_Batch()
                           {
                               BATCH_ID = dr[Common.SUP_BATCHES.BATCH_ID].ToString(),
                               BATCH = dr[Common.SUP_BATCHES.BATCH].ToString()
                           }).ToList();
                objSemesterRoot.BATCH = new SelectList(liBatch, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
            }
            if (dtProgramme != null && dtProgramme.Rows.Count > 0)
            {
                liProgramme = (from DataRow dr in dtProgramme.Rows
                               select new cp_Program_2017()
                               {
                                   PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                   PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                               }).ToList();
                objSemesterRoot.PROGRAMME = new SelectList(liProgramme, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
            }
            if (dtSemester != null && dtProgramme.Rows.Count > 0)
            {
                liSemester = (from DataRow dr in dtSemester.Rows
                              select new SupSemester()
                              {
                                  SUP_SEMESTER_ID = dr[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString(),
                                  SEMESTER_NAME = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString()
                              }).ToList();
                objSemesterRoot.SEMESTER = new SelectList(liSemester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
            }
            return View(objSemesterRoot);
        }
        public string GetSemesterId(string sProgramme, string sBatch)
        {
            string sOption = string.Empty;
            DataTable dtSemesterList = new DataTable();
            DataTable dtTempSemester = new DataTable();
            DataTable dtSemester = new DataTable();
            using (AcademicsViewModel objAcademicsview = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_SEMESTER_ROOT_2017.PROGRAMME, sProgramme, EnumCommand.DataType.String);
                dv.Add(Common.CP_SEMESTER_ROOT_2017.BATCH, sBatch, EnumCommand.DataType.String);
                dtSemesterList = objAcademicsview.GetSemesterId(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                dtSemester = SupportDataMethod.FetchSemester().DataSource.Table;
                dtTempSemester = SupportDataMethod.FetchSemester().DataSource.Table;
                if (dtSemesterList != null && dtSemesterList.Rows.Count > 0)
                {
                    foreach (DataRow item in dtSemesterList.Rows)
                    {
                        sOption += "<option value='" + item[Common.SUP_SEMESTER.SUP_SEMESTER_ID] + "' " + item["SELECTED"] + " >" + item[Common.SUP_SEMESTER.SEMESTER_NAME] + "</option>";
                        foreach (DataRow row in dtSemester.Rows)
                            if (item[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString() == row[Common.SUP_SEMESTER.SUP_SEMESTER_ID].ToString())
                            {
                                row.Delete();
                            }
                        dtSemester.AcceptChanges();
                    }
                }
                if (dtSemester != null && dtSemester.Rows.Count > 0)
                {
                    foreach (DataRow item in dtSemester.Rows)
                    {
                        sOption += "<option value='" + item[Common.SUP_SEMESTER.SUP_SEMESTER_ID] + "' >" + item[Common.SUP_SEMESTER.SEMESTER_NAME] + "</option>";
                    }
                }

            }
            return sOption;
        }
        public string InsertSemesterRootInfo(string sSemester)
        {
            string Result = string.Empty;
            //var sResult = JsonConvert.DeserializeObject<SemesterRootInfo>(sSemester);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            JsonSemesterRoot objJson = JsonConvert.DeserializeObject<JsonSemesterRoot>(sSemester);
            AcademicsViewModel objSemesterRoot = new AcademicsViewModel();
            resultArg = objSemesterRoot.BulkSaveSemesterRoot(objJson, Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
            if (resultArg.Success)
            {
                sResult = Common.Messages.RecordsSavedSuccessfully;
            }
            else
            {
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return sResult;
        }
        public ActionResult DeleteSemesterRootInfo(string id)
        {
            string sResult = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                using (AcademicsViewModel objSemesterRoot = new AcademicsViewModel())
                {
                    dv.Clear();
                    dv.Add(Common.CP_SEMESTER_ROOT_2017.ACC_SEMESTER_ID, id, EnumCommand.DataType.String);
                    resultArg = objSemesterRoot.DeleteSemesterRootInfo(dv, Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
                    if (resultArg.Success)
                    {
                        TempData["DeleteSuccess"] = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        TempData["DeleteSuccess"] = Common.Messages.RecordsSavedSuccessfully;

                    }
                }
            }
            else
            {
                TempData["DeleteSuccess"] = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return RedirectToAction("ListSemesterRoot", "Academics");
        }
        #endregion
        #region Department
        public ActionResult DepartmentInfo()
        {
            DepartmentModel objDepartmentModel = new DepartmentModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtFaculty = new DataTable();
                dtFaculty = SupportDataMethod.FetchFaculty((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                IList<SupFaculty> liFaculty = new List<SupFaculty>();
                if (dtFaculty != null && dtFaculty.Rows.Count > 0)
                {
                    liFaculty = (from DataRow dr in dtFaculty.Rows
                                 select new SupFaculty()
                                 {
                                     FACULTY_ID = dr[Common.CP_FACULTY_2017.FACULTY_ID].ToString(),
                                     FACULTY = dr[Common.CP_FACULTY_2017.FACULTY].ToString()
                                 }).ToList();
                    objDepartmentModel.FACULTY = new SelectList(liFaculty, Common.CP_FACULTY_2017.FACULTY_ID, Common.CP_FACULTY_2017.FACULTY);
                }
            }
            return View(objDepartmentModel);
        }
        [UserRights("ADMIN")]
        public ActionResult ListDepartmentInfo()
        {
            AcademicsViewModel objDepartment = new AcademicsViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtDepartmentList = new DataTable();
            dtDepartmentList = objDepartment.FetchDepartmentInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<DepartmentInfo> liDepartment = new List<DepartmentInfo>();
            if (dtDepartmentList != null && dtDepartmentList.Rows.Count > 0)
            {
                liDepartment = (from DataRow dr in dtDepartmentList.Rows
                                select new DepartmentInfo()
                                {
                                    DEPARTMENT_ID = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT_ID].ToString(),
                                    DEPARTMENT_CODE = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT_CODE].ToString(),
                                    DEPARTMENT = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString(),
                                    DEPARTMENT_ORDER = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT_ORDER].ToString(),
                                    FACULTY = dr[Common.CP_FACULTY_2017.FACULTY].ToString(),
                                    YEAR_OF_PUBLISHMENT = dr[Common.CP_DEPARTMENT_2017.YEAR_OF_PUBLISHMENT].ToString(),
                                    IS_ACTIVE = dr[Common.CP_DEPARTMENT_2017.IS_ACTIVE].ToString(),
                                }).ToList();
            }
            return View(liDepartment);
        }
        public JsonResult InsertDepartmentInfo(string sJsonDepartment)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (sJsonDepartment != null)
                {
                    DataTable dtDepartment = new DataTable();
                    var Result = JsonConvert.DeserializeObject<DepartmentInfo>(sJsonDepartment);

                    dv.Clear();
                    dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT_CODE, Result.DEPARTMENT_CODE, EnumCommand.DataType.String);
                    using (AcademicsViewModel objDepartment = new AcademicsViewModel())
                    {
                        dtDepartment = objDepartment.CheckDepartment(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                        if (dtDepartment != null && dtDepartment.Rows.Count == 0)
                        {
                            dv.Clear();
                            dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT, Result.DEPARTMENT, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT_CODE, Result.DEPARTMENT_CODE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT_ORDER, Result.DEPARTMENT_ORDER, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEPARTMENT_2017.FACULTY, Result.FACULTY, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEPARTMENT_2017.YEAR_OF_PUBLISHMENT, Result.YEAR_OF_PUBLISHMENT, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEPARTMENT_2017.IS_ACTIVE, Result.IS_ACTIVE, EnumCommand.DataType.String);
                            resultArg = objDepartment.InsertDepartmentInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                            sResult = resultArg.Success.ToString();
                            if (sResult == "True")
                            {
                                sResult = "Records Saved Successfully";
                            }
                            else
                            {
                                sResult = "Enter Correct Values";
                            }
                        }
                        else
                        {
                            sResult = "Department Already Exists...!";
                        }
                    }
                }
            }
            return Json(sResult);
        }
        public ActionResult EditDepartmentInfo(int id)
        {
            string sDepartmentId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.DEPARTMENT_ID] = sDepartmentId;
            DepartmentModel objDepartmentModel = new DepartmentModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtFaculty = new DataTable();
                dtFaculty = SupportDataMethod.FetchFaculty((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                IList<SupFaculty> liFaculty = new List<SupFaculty>();
                if (dtFaculty != null && dtFaculty.Rows.Count > 0)
                {
                    liFaculty = (from DataRow dr in dtFaculty.Rows
                                 select new SupFaculty()
                                 {
                                     FACULTY_ID = dr[Common.CP_FACULTY_2017.FACULTY_ID].ToString(),
                                     FACULTY = dr[Common.CP_FACULTY_2017.FACULTY].ToString()
                                 }).ToList();
                    objDepartmentModel.FACULTY = new SelectList(liFaculty, Common.CP_FACULTY_2017.FACULTY_ID, Common.CP_FACULTY_2017.FACULTY);
                }
            }
            return View(objDepartmentModel);
        }
        public ActionResult UpdateDepartmentInfo(string JsonDepartment)
        {
            var sDepartmentId = Session[Common.SESSION_VARIABLES.DEPARTMENT_ID].ToString();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            if (JsonDepartment != null)
            {
                var Result = JsonConvert.DeserializeObject<DepartmentInfo>(JsonDepartment);
                dv.Clear();
                dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT_ID, sDepartmentId, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT, Result.DEPARTMENT, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT_CODE, Result.DEPARTMENT_CODE, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT_ORDER, Result.DEPARTMENT_ORDER, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEPARTMENT_2017.FACULTY, Result.FACULTY, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEPARTMENT_2017.YEAR_OF_PUBLISHMENT, Result.YEAR_OF_PUBLISHMENT, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEPARTMENT_2017.IS_ACTIVE, Result.IS_ACTIVE, EnumCommand.DataType.String);
                using (AcademicsViewModel objDepartment = new AcademicsViewModel())
                {
                    resultArg = objDepartment.UpdateDepartmentInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                    sResult = resultArg.Success.ToString();
                }
            }
            return RedirectToAction("ListDepartmentInfo", "Academics");
        }
        public JsonResult FetchDepartmentInfoById(string id)
        {
            string sDepartmentId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.DEPARTMENT_ID] = sDepartmentId;
            dv.Clear();
            dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT_ID, sDepartmentId, EnumCommand.DataType.String);
            //AcademicsViewModel objDepartment = new AcademicsViewModel();
            DataTable dtFetchDepartmentInfoById = new DataTable();
            using (AcademicsViewModel objDepartment = new AcademicsViewModel())
            {
                dtFetchDepartmentInfoById = objDepartment.FetchDepartmentInfoById(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            }
            if (dtFetchDepartmentInfoById != null && dtFetchDepartmentInfoById.Rows.Count > 0)
            {
                DepartmentInfo objDepartment = new DepartmentInfo()
                {
                    DEPARTMENT = dtFetchDepartmentInfoById.Rows[0][Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString(),
                    DEPARTMENT_CODE = dtFetchDepartmentInfoById.Rows[0][Common.CP_DEPARTMENT_2017.DEPARTMENT_CODE].ToString(),
                    DEPARTMENT_ORDER = dtFetchDepartmentInfoById.Rows[0][Common.CP_DEPARTMENT_2017.DEPARTMENT_ORDER].ToString(),
                    FACULTY = dtFetchDepartmentInfoById.Rows[0][Common.CP_DEPARTMENT_2017.FACULTY].ToString(),
                    YEAR_OF_PUBLISHMENT = dtFetchDepartmentInfoById.Rows[0][Common.CP_DEPARTMENT_2017.YEAR_OF_PUBLISHMENT].ToString(),
                    IS_ACTIVE = dtFetchDepartmentInfoById.Rows[0][Common.CP_DEPARTMENT_2017.IS_ACTIVE].ToString(),

                };
                return Json(objDepartment);
            }
            else
            {
                return Json(sResult);
            }

        }
        public ActionResult DeleteDepartmentInfo(string id)
        {
            using (AcademicsViewModel objDepartment = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_DEPARTMENT_2017.DEPARTMENT_ID, id, EnumCommand.DataType.String);
                resultArg = objDepartment.DeleteDepartmentInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("ListDepartmentInfo", "Academics");
        }
        #endregion
        #region Faculty
        public ActionResult FacultyInfo()
        {
            return View();
        }
        [UserRights("ADMIN")]
        public ActionResult ListFacultyInfo()
        {
            AcademicsViewModel objFaculty = new AcademicsViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtFaculty = new DataTable();
            dtFaculty = objFaculty.FetchFAcutlyInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<FacultyInfo> liFaculty = new List<FacultyInfo>();
            if (dtFaculty != null && dtFaculty.Rows.Count > 0)
            {
                liFaculty = (from DataRow dr in dtFaculty.Rows
                             select new FacultyInfo()
                             {
                                 FACULTY_ID = dr[Common.CP_FACULTY_2017.FACULTY_ID].ToString(),
                                 FACULTY = dr[Common.CP_FACULTY_2017.FACULTY].ToString(),
                                 FACULTY_CODE = dr[Common.CP_FACULTY_2017.FACULTY_CODE].ToString(),
                                 FACULTY_ORDER = dr[Common.CP_FACULTY_2017.FACULTY_ORDER].ToString(),
                                 FAC_SERIES = dr[Common.CP_FACULTY_2017.FAC_SERIES].ToString(),
                                 IS_ACTIVE = dr[Common.CP_FACULTY_2017.IS_ACTIVE].ToString(),
                             }).ToList();
            }
            return View(liFaculty);
        }
        public JsonResult InsertFacultyInfo(string sJsonFaculty)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (sJsonFaculty != null)
                {
                    DataTable dtFactulty = new DataTable();
                    var Result = JsonConvert.DeserializeObject<FacultyInfo>(sJsonFaculty);

                    dv.Clear();
                    dv.Add(Common.CP_FACULTY_2017.FACULTY_CODE, Result.FACULTY_CODE, EnumCommand.DataType.String);
                    using (AcademicsViewModel objFaculty = new AcademicsViewModel())
                    {
                        dtFactulty = objFaculty.CheckFaculty(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                        if (dtFactulty != null && dtFactulty.Rows.Count == 0)
                        {
                            dv.Clear();
                            dv.Add(Common.CP_FACULTY_2017.FACULTY, Result.FACULTY, EnumCommand.DataType.String);
                            dv.Add(Common.CP_FACULTY_2017.FACULTY_CODE, Result.FACULTY_CODE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_FACULTY_2017.FACULTY_ORDER, Result.FACULTY_ORDER, EnumCommand.DataType.String);
                            dv.Add(Common.CP_FACULTY_2017.FAC_SERIES, Result.FAC_SERIES, EnumCommand.DataType.String);
                            resultArg = objFaculty.InsertFacultyInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                            sResult = resultArg.Success.ToString();
                            sResult = (resultArg.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                        }
                        else
                        {
                            sResult = "Faculty Already Exists...!";
                        }
                    }
                }
            }
            return Json(sResult);
        }
        public ActionResult EditFacultyInfo(int id)
        {
            string sFacultyId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.FACULTY_ID] = sFacultyId;
            FacultyModel objFacultyModel = new FacultyModel();
            return View(objFacultyModel);
        }
        public JsonResult FetchFacultyInfoById(string sJsonFacultyId)
        {
            string sFacultyId = Convert.ToString(sJsonFacultyId);
            Session[Common.SESSION_VARIABLES.FACULTY_ID] = sFacultyId;
            dv.Clear();
            dv.Add(Common.CP_FACULTY_2017.FACULTY_ID, sFacultyId, EnumCommand.DataType.String);
            //AcademicsViewModel objFacutly = new AcademicsViewModel();
            DataTable dtFaculty = new DataTable();
            using (AcademicsViewModel objFaculty = new AcademicsViewModel())
            {
                dtFaculty = objFaculty.FetchFacultyInfoById(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            }
            if (dtFaculty != null && dtFaculty.Rows.Count > 0)
            {
                FacultyInfo objFaculty = new FacultyInfo()
                {
                    FACULTY_ID = dtFaculty.Rows[0][Common.CP_FACULTY_2017.FACULTY_ID].ToString(),
                    FACULTY = dtFaculty.Rows[0][Common.CP_FACULTY_2017.FACULTY].ToString(),
                    FACULTY_CODE = dtFaculty.Rows[0][Common.CP_FACULTY_2017.FACULTY_CODE].ToString(),
                    FACULTY_ORDER = dtFaculty.Rows[0][Common.CP_FACULTY_2017.FACULTY_ORDER].ToString(),
                    FAC_SERIES = dtFaculty.Rows[0][Common.CP_FACULTY_2017.FAC_SERIES].ToString(),
                };
                return Json(objFaculty);
            }
            else
            {
                return Json(sResult);
            }

        }
        public ActionResult UpdateFacultyInfo(string sJsonFaculty)
        {
            var sFacultyId = Session[Common.SESSION_VARIABLES.FACULTY_ID].ToString();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            if (sJsonFaculty != null)
            {
                var Result = JsonConvert.DeserializeObject<FacultyInfo>(sJsonFaculty);
                dv.Clear();
                dv.Add(Common.CP_FACULTY_2017.FACULTY_ID, sFacultyId, EnumCommand.DataType.String);
                dv.Add(Common.CP_FACULTY_2017.FACULTY, Result.FACULTY, EnumCommand.DataType.String);
                dv.Add(Common.CP_FACULTY_2017.FACULTY_CODE, Result.FACULTY_CODE, EnumCommand.DataType.String);
                dv.Add(Common.CP_FACULTY_2017.FACULTY_ORDER, Result.FACULTY_ORDER, EnumCommand.DataType.String);
                dv.Add(Common.CP_FACULTY_2017.FAC_SERIES, Result.FAC_SERIES, EnumCommand.DataType.String);
                using (AcademicsViewModel objFacutly = new AcademicsViewModel())
                {
                    resultArg = objFacutly.UpdateFacultyInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                    sResult = resultArg.Success.ToString();
                }
            }
            return RedirectToAction("ListFacultyInfo", "Academics");
        }
        public ActionResult DeleteFacultyInfo(string id)
        {
            using (AcademicsViewModel objFaculty = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_FACULTY_2017.FACULTY_ID, id, EnumCommand.DataType.String);
                resultArg = objFaculty.DeleteFacultyInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("ListFacultyInfo", "Academics");
        }
        #endregion
        #region Class
        public ActionResult ClassInfo()
        {
            ClassModel objClass = new ClassModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtClassType = new DataTable();
            DataTable dtClassLevel = new DataTable();
            DataTable dtClassYear = new DataTable();
            DataTable dtProgramme = new DataTable();
            DataTable dtDepartment = new DataTable();
            DataTable dtLanguage = new DataTable();
            DataTable dtShift = new DataTable();
            DataTable dtCourseId = new DataTable();

            dtClassType = SupportDataMethod.FetchClassType().DataSource.Table;
            dtClassLevel = SupportDataMethod.FetchClassLevel().DataSource.Table;
            dtClassYear = SupportDataMethod.FetchClassYear().DataSource.Table;
            dtProgramme = SupportDataMethod.FetchProgram((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            dtLanguage = SupportDataMethod.FetchLanguage().DataSource.Table;
            dtShift = SupportDataMethod.FetchShift().DataSource.Table;
            dtCourseId = SupportDataMethod.FetchNMECourses((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;

            List<SupClassType> liClassType = new List<SupClassType>();
            List<SupClassLevel> liClassLevel = new List<SupClassLevel>();
            List<SubClassYear> liClassYear = new List<SubClassYear>();
            List<cp_Program_2017> liProgramme = new List<cp_Program_2017>();
            List<SupDepartment> liDepartment = new List<SupDepartment>();
            List<Sup_Language> liLanguage = new List<Sup_Language>();
            List<Sup_Shift> liShift = new List<Sup_Shift>();
            List<cp_Course_Root_2017> liCourse = new List<cp_Course_Root_2017>();

            if (dtDepartment != null && dtDepartment.Rows.Count > 0)
            {
                liDepartment = (from DataRow dr in dtDepartment.Rows
                                select new SupDepartment()
                                {
                                    DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                    DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                }).ToList();
                objClass.DEPARTMENT = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
            }

            if (dtClassType != null && dtClassType.Rows.Count > 0)
            {
                liClassType = (from DataRow dr in dtClassType.Rows
                               select new SupClassType()
                               {
                                   CLASS_TYPE_ID = dr[Common.SUP_CLASS_TYPE.CLASS_TYPE_ID].ToString(),
                                   CLASS_TYPE = dr[Common.SUP_CLASS_TYPE.CLASS_TYPE].ToString()
                               }).ToList();
                objClass.CLASS_TYPE = new SelectList(liClassType, Common.SUP_CLASS_TYPE.CLASS_TYPE_ID, Common.SUP_CLASS_TYPE.CLASS_TYPE);
            }
            if (dtClassLevel != null && dtClassLevel.Rows.Count > 0)
            {
                liClassLevel = (from DataRow dr in dtClassLevel.Rows
                                select new SupClassLevel()
                                {
                                    CLASSLEVELID = dr[Common.SUP_CLASS_LEVEL.CLASSLEVELID].ToString(),
                                    CLASSLEVEL = dr[Common.SUP_CLASS_LEVEL.CLASSLEVEL].ToString()
                                }).ToList();
                objClass.CLASS_LEVEL = new SelectList(liClassLevel, Common.SUP_CLASS_LEVEL.CLASSLEVELID, Common.SUP_CLASS_LEVEL.CLASSLEVEL);
            }

            if (dtClassYear != null && dtClassYear.Rows.Count > 0)
            {
                liClassYear = (from DataRow dr in dtClassYear.Rows
                               select new SubClassYear()
                               {
                                   CLASS_YEAR_ID = dr[Common.SUP_CLASS_YEAR.CLASS_YEAR_ID].ToString(),
                                   CLASS_YEAR = dr[Common.SUP_CLASS_YEAR.CLASS_YEAR_ID].ToString()
                               }).ToList();
                objClass.CLASS_YEAR = new SelectList(liClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.SUP_CLASS_YEAR.CLASS_YEAR);
            }
            if (dtProgramme != null && dtProgramme.Rows.Count > 0)
            {
                liProgramme = (from DataRow dr in dtProgramme.Rows
                               select new cp_Program_2017()
                               {
                                   PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                   PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                               }).ToList();
                objClass.PROGRAMME = new SelectList(liProgramme, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
            }

            if (dtLanguage != null && dtLanguage.Rows.Count > 0)
            {
                liLanguage = (from DataRow dr in dtLanguage.Rows
                              select new Sup_Language()
                              {
                                  LANGUAGE_ID = dr[Common.SUP_LANGUAGE.LANGUAGE_ID].ToString(),
                                  LANGUAGE_NAME = dr[Common.SUP_LANGUAGE.LANGUAGE_NAME].ToString()
                              }).ToList();
                objClass.LANGUAGE = new SelectList(liLanguage, Common.SUP_LANGUAGE.LANGUAGE_ID, Common.SUP_LANGUAGE.LANGUAGE_NAME);
            }
            if (dtShift != null && dtShift.Rows.Count > 0)
            {
                liShift = (from DataRow dr in dtShift.Rows
                           select new Sup_Shift()
                           {
                               SHIFT_ID = dr[Common.SUP_SHIFT.SHIFT_ID].ToString(),
                               SHIFT_NAME = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString()
                           }).ToList();
                objClass.SHIFT = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            }
            if (dtCourseId != null && dtCourseId.Rows.Count > 0)
            {
                liCourse = (from DataRow dr in dtCourseId.Rows
                            select new cp_Course_Root_2017()
                            {
                                COURSE_ROOT_ID = dr[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID].ToString(),
                                COURSE_CODE = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString()
                            }).ToList();
                objClass.COURSE_ID = new SelectList(liCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_CODE);
            }
            return View(objClass);
        }
        [UserRights("ADMIN")]
        public ActionResult ListClassInfo()
        {
            AcademicsViewModel objClassList = new AcademicsViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtClassList = new DataTable();
            dtClassList = objClassList.FetchClassInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<ClassInfo> liClass = new List<ClassInfo>();
            if (dtClassList != null && dtClassList.Rows.Count > 0)
            {
                liClass = (from DataRow dr in dtClassList.Rows
                           select new ClassInfo()
                           {
                               CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                               CLASS_CODE = dr[Common.CP_CLASSES_2017.CLASS_CODE].ToString(),
                               CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString(),
                               //CLASS_DESCRIPTION = dr[Common.CP_CLASSES_2017.CLASS_DESCRIPTION].ToString(),
                               //CLASS_LEVEL = dr[Common.SUP_CLASS_LEVEL.CLASSLEVEL].ToString(),
                               CLASS_TYPE = dr[Common.SUP_CLASS_TYPE.CLASS_TYPE].ToString(),
                               //CLASS_YEAR = dr[Common.SUP_CLASS_YEAR.CLASS_YEAR].ToString(),
                               PROGRAMME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString(),
                               DEPARTMENT = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString(),
                               //LANGUAGE = dr[Common.SUP_LANGUAGE.LANGUAGE_NAME].ToString(),
                               //IS_CHOICE = dr[Common.SUP_IS_CHOICE.IS_CHOICE_NAME].ToString(),
                               //CLASS_ORDER = dr[Common.CP_CLASSES_2017.CLASS_ORDER].ToString(),
                               SHIFT = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString(),
                               //COURSE_ID = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString(),
                           }).ToList();
            }
            return View(liClass);
        }
        public JsonResult InsertClassInfo(string sJsonClass)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (sJsonClass != null)
                {
                    DataTable dtClass = new DataTable();
                    var Result = JsonConvert.DeserializeObject<ClassInfo>(sJsonClass);

                    dv.Clear();
                    dv.Add(Common.CP_CLASSES_2017.CLASS_CODE, Result.CLASS_CODE, EnumCommand.DataType.String);
                    using (AcademicsViewModel objClass = new AcademicsViewModel())
                    {
                        dtClass = objClass.CheckClass(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                        if (dtClass != null && dtClass.Rows.Count == 0)
                        {
                            dv.Clear();
                            dv.Add(Common.CP_CLASSES_2017.CLASS_CODE, Result.CLASS_CODE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.CLASS_NAME, Result.CLASS_NAME, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.CLASS_DESCRIPTION, Result.CLASS_DESCRIPTION, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.CLASS_TYPE, Result.CLASS_TYPE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.CLASS_LEVEL, Result.CLASS_LEVEL, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.CLASS_YEAR, Result.CLASS_YEAR, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.PROGRAMME, Result.PROGRAMME, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.DEPARTMENT, Result.DEPARTMENT, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.LANGUAGE, Result.LANGUAGE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.IS_CHOICE, Result.IS_CHOICE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.CLASS_ORDER, Result.CLASS_ORDER, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.SHIFT, Result.SHIFT, EnumCommand.DataType.String);
                            dv.Add(Common.CP_CLASSES_2017.COURSE_ID, Result.COURSE_ID, EnumCommand.DataType.String);
                            resultArg = objClass.InsertClassInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                            sResult = resultArg.Success.ToString();
                            if (sResult == "True")
                            {
                                sResult = "Records Saved Successfully";
                            }
                            else
                            {
                                sResult = "Enter Correct Values";
                            }
                        }
                        else
                        {
                            sResult = "Class Already Exists";
                        }
                    }
                }
            }
            return Json(sResult);
        }
        public ActionResult EditClassInfo(int id)
        {
            string sClassId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.CLASS_ID] = sClassId;

            ClassModel objClass = new ClassModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtClassType = new DataTable();
                DataTable dtClassLevel = new DataTable();
                DataTable dtClassYear = new DataTable();
                DataTable dtProgramme = new DataTable();
                DataTable dtDepartment = new DataTable();
                DataTable dtLanguage = new DataTable();
                DataTable dtShift = new DataTable();
                DataTable dtCourseId = new DataTable();

                dtClassType = SupportDataMethod.FetchClassType().DataSource.Table;
                dtClassLevel = SupportDataMethod.FetchClassLevel().DataSource.Table;
                dtClassYear = SupportDataMethod.FetchClassYear().DataSource.Table;
                dtProgramme = SupportDataMethod.FetchProgram((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                dtLanguage = SupportDataMethod.FetchLanguage().DataSource.Table;
                dtShift = SupportDataMethod.FetchShift().DataSource.Table;
                dtCourseId = SupportDataMethod.FetchNMECourses((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;

                List<SupClassType> liClassType = new List<SupClassType>();
                List<SupClassLevel> liClassLevel = new List<SupClassLevel>();
                List<SubClassYear> liClassYear = new List<SubClassYear>();
                List<cp_Program_2017> liProgramme = new List<cp_Program_2017>();
                List<SupDepartment> liDepartment = new List<SupDepartment>();
                List<Sup_Language> liLanguage = new List<Sup_Language>();
                List<Sup_Shift> liShift = new List<Sup_Shift>();
                List<cp_Course_Root_2017> liCourse = new List<cp_Course_Root_2017>();

                if (dtDepartment != null && dtDepartment.Rows.Count > 0)
                {
                    liDepartment = (from DataRow dr in dtDepartment.Rows
                                    select new SupDepartment()
                                    {
                                        DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                        DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                    }).ToList();
                    objClass.DEPARTMENT = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
                }

                if (dtClassType != null && dtClassType.Rows.Count > 0)
                {
                    liClassType = (from DataRow dr in dtClassType.Rows
                                   select new SupClassType()
                                   {
                                       CLASS_TYPE_ID = dr[Common.SUP_CLASS_TYPE.CLASS_TYPE_ID].ToString(),
                                       CLASS_TYPE = dr[Common.SUP_CLASS_TYPE.CLASS_TYPE].ToString()
                                   }).ToList();
                    objClass.CLASS_TYPE = new SelectList(liClassType, Common.SUP_CLASS_TYPE.CLASS_TYPE_ID, Common.SUP_CLASS_TYPE.CLASS_TYPE);
                }
                if (dtClassLevel != null && dtClassLevel.Rows.Count > 0)
                {
                    liClassLevel = (from DataRow dr in dtClassLevel.Rows
                                    select new SupClassLevel()
                                    {
                                        CLASSLEVELID = dr[Common.SUP_CLASS_LEVEL.CLASSLEVELID].ToString(),
                                        CLASSLEVEL = dr[Common.SUP_CLASS_LEVEL.CLASSLEVEL].ToString()
                                    }).ToList();
                    objClass.CLASS_LEVEL = new SelectList(liClassLevel, Common.SUP_CLASS_LEVEL.CLASSLEVELID, Common.SUP_CLASS_LEVEL.CLASSLEVEL);
                }

                if (dtClassYear != null && dtClassYear.Rows.Count > 0)
                {
                    liClassYear = (from DataRow dr in dtClassYear.Rows
                                   select new SubClassYear()
                                   {
                                       CLASS_YEAR_ID = dr[Common.SUP_CLASS_YEAR.CLASS_YEAR_ID].ToString(),
                                       CLASS_YEAR = dr[Common.SUP_CLASS_YEAR.CLASS_YEAR_ID].ToString()
                                   }).ToList();
                    objClass.CLASS_YEAR = new SelectList(liClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.SUP_CLASS_YEAR.CLASS_YEAR);
                }
                if (dtProgramme != null && dtProgramme.Rows.Count > 0)
                {
                    liProgramme = (from DataRow dr in dtProgramme.Rows
                                   select new cp_Program_2017()
                                   {
                                       PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                       PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                                   }).ToList();
                    objClass.PROGRAMME = new SelectList(liProgramme, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }

                if (dtLanguage != null && dtLanguage.Rows.Count > 0)
                {
                    liLanguage = (from DataRow dr in dtLanguage.Rows
                                  select new Sup_Language()
                                  {
                                      LANGUAGE_ID = dr[Common.SUP_LANGUAGE.LANGUAGE_ID].ToString(),
                                      LANGUAGE_NAME = dr[Common.SUP_LANGUAGE.LANGUAGE_NAME].ToString()
                                  }).ToList();
                    objClass.LANGUAGE = new SelectList(liLanguage, Common.SUP_LANGUAGE.LANGUAGE_ID, Common.SUP_LANGUAGE.LANGUAGE_NAME);
                }
                if (dtShift != null && dtShift.Rows.Count > 0)
                {
                    liShift = (from DataRow dr in dtShift.Rows
                               select new Sup_Shift()
                               {
                                   SHIFT_ID = dr[Common.SUP_SHIFT.SHIFT_ID].ToString(),
                                   SHIFT_NAME = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString()
                               }).ToList();
                    objClass.SHIFT = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                }
                if (dtCourseId != null && dtCourseId.Rows.Count > 0)
                {
                    liCourse = (from DataRow dr in dtCourseId.Rows
                                select new cp_Course_Root_2017()
                                {
                                    COURSE_ROOT_ID = dr[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID].ToString(),
                                    COURSE_CODE = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString()
                                }).ToList();
                    objClass.COURSE_ID = new SelectList(liCourse, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_CODE);
                }
            }
            return View(objClass);
        }
        public JsonResult FetchClassInfoById(string id)
        {
            string sClassId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.CLASS_ID] = sClassId;
            dv.Clear();
            dv.Add(Common.CP_CLASSES_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
            //AcademicsViewModel objDepartment = new AcademicsViewModel();
            DataTable dtFetchclassById = new DataTable();
            using (AcademicsViewModel objClass = new AcademicsViewModel())
            {
                dtFetchclassById = objClass.FetchClassInfoById(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            }
            if (dtFetchclassById != null && dtFetchclassById.Rows.Count > 0)
            {
                ClassInfo objClass = new ClassInfo()
                {
                    CLASS_CODE = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.CLASS_CODE].ToString(),
                    CLASS_NAME = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.CLASS_NAME].ToString(),
                    CLASS_DESCRIPTION = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.CLASS_DESCRIPTION].ToString(),
                    CLASS_TYPE = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.CLASS_TYPE].ToString(),
                    CLASS_LEVEL = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.CLASS_LEVEL].ToString(),
                    CLASS_YEAR = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.CLASS_YEAR].ToString(),
                    PROGRAMME = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.PROGRAMME].ToString(),
                    DEPARTMENT = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.DEPARTMENT].ToString(),
                    LANGUAGE = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.LANGUAGE].ToString(),
                    IS_CHOICE = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.IS_CHOICE].ToString(),
                    CLASS_ORDER = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.CLASS_ORDER].ToString(),
                    SHIFT = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.SHIFT].ToString(),
                    COURSE_ID = dtFetchclassById.Rows[0][Common.CP_CLASSES_2017.COURSE_ID].ToString()
                };
                return Json(objClass);
            }
            else
            {
                return Json(sResult);
            }

        }
        public ActionResult UpdateClassInfo(string sJsonClass)
        {
            var sClassId = Session[Common.SESSION_VARIABLES.CLASS_ID].ToString();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            if (sJsonClass != null)
            {
                var Result = JsonConvert.DeserializeObject<ClassInfo>(sJsonClass);
                dv.Clear();
                dv.Add(Common.CP_CLASSES_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.CLASS_CODE, Result.CLASS_CODE, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.CLASS_NAME, Result.CLASS_NAME, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.CLASS_DESCRIPTION, Result.CLASS_DESCRIPTION, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.CLASS_TYPE, Result.CLASS_TYPE, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.CLASS_LEVEL, Result.CLASS_LEVEL, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.CLASS_YEAR, Result.CLASS_YEAR, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.PROGRAMME, Result.PROGRAMME, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.DEPARTMENT, Result.DEPARTMENT, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.LANGUAGE, Result.LANGUAGE, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.IS_CHOICE, Result.IS_CHOICE, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.CLASS_ORDER, Result.CLASS_ORDER, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.SHIFT, Result.SHIFT, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.COURSE_ID, Result.COURSE_ID, EnumCommand.DataType.String);
                using (AcademicsViewModel objClass = new AcademicsViewModel())
                {
                    resultArg = objClass.UpdateClassInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                    sResult = resultArg.Success.ToString();
                }
            }
            return RedirectToAction("ListClassInfo", "Academics");
        }
        public ActionResult DeleteClassInfo(string id)
        {
            using (AcademicsViewModel objClass = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_CLASSES_2017.CLASS_ID, id, EnumCommand.DataType.String);
                resultArg = objClass.DeleteClassInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("ListClassInfo", "Academics");
        }
        #endregion
        #region Batch
        public ActionResult BatchInfo()
        {
            BatchModel objBatch = new BatchModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtBatch = new DataTable();
                DataTable dtAcademicYear = new DataTable();
                dtBatch = SupportDataMethod.FetchBatch().DataSource.Table;
                dtAcademicYear = SupportDataMethod.FetchAcademicYearList().DataSource.Table;
                IList<sup_Batch> liBatch = new List<sup_Batch>();
                IList<SUP_ACADEMIC_YEAR_LIST> liAcademicYear = new List<SUP_ACADEMIC_YEAR_LIST>();
                if (dtBatch != null && dtBatch.Rows.Count > 0)
                {
                    liBatch = (from DataRow dr in dtBatch.Rows
                               select new sup_Batch()
                               {
                                   BATCH_ID = dr[Common.SUP_BATCHES.BATCH_ID].ToString(),
                                   BATCH = dr[Common.SUP_BATCHES.BATCH].ToString()
                               }).ToList();
                    objBatch.BATCH_ID = new SelectList(liBatch, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
                }
                if (dtAcademicYear != null && dtAcademicYear.Rows.Count > 0)
                {
                    liAcademicYear = (from DataRow dr in dtAcademicYear.Rows
                                      select new SUP_ACADEMIC_YEAR_LIST()
                                      {
                                          ACADEMIC_YEAR_ID = dr[Common.ACC_YEAR.ACADEMIC_YEAR_ID].ToString(),
                                          AC_YEAR = dr[Common.ACC_YEAR.AC_YEAR].ToString()
                                      }).ToList();
                    objBatch.ACADEMIC_YEAR_ID = new SelectList(liAcademicYear, Common.ACC_YEAR.ACADEMIC_YEAR_ID, Common.ACC_YEAR.AC_YEAR);
                }
            }
            return View(objBatch);
        }
        [UserRights("ADMIN")]
        public ActionResult ListBatchInfo()
        {
            AcademicsViewModel objBatch = new AcademicsViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtBatch = new DataTable();
            dtBatch = objBatch.FetchBatchInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<BatchInfo> liBatch = new List<BatchInfo>();
            if (dtBatch != null && dtBatch.Rows.Count > 0)
            {
                liBatch = (from DataRow dr in dtBatch.Rows
                           select new BatchInfo()
                           {
                               AC_BATCH_ID = dr[Common.ACADEMIC_BATCHES.AC_BATCH_ID].ToString(),
                               ACADEMIC_YEAR_ID = dr[Common.ACADEMIC_YEAR.AC_YEAR].ToString(),
                               BATCH_ID = dr[Common.SUP_BATCHES.BATCH].ToString(),
                           }).ToList();
            }
            return View(liBatch);
        }
        public JsonResult InsertBatchInfo(string sJsonBatch)
        {
            string Result = string.Empty;
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            JsonBatchInfo objJson = JsonConvert.DeserializeObject<JsonBatchInfo>(sJsonBatch);
            AcademicsViewModel objBatch = new AcademicsViewModel();
            resultArg = objBatch.BulkSaveBatchInfo(objJson, Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
            if (resultArg.Success)
            {
                sResult = Common.Messages.RecordsSavedSuccessfully;
            }
            else
            {
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return Json(sResult);
        }
        public ActionResult DeleteBatchInfo(string id)
        {
            using (AcademicsViewModel objDepartment = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.ACADEMIC_BATCHES.AC_BATCH_ID, id, EnumCommand.DataType.String);
                resultArg = objDepartment.DeleteDepartmentInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("ListDepartmentInfo", "Academics");
        }
        #endregion
        #region Course Type
        public ActionResult CourseTypeInfo()
        {
            CourseTypeModel objCourseType = new CourseTypeModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtPart = new DataTable();
                DataTable dtProgrammeLevel = new DataTable();

                dtPart = SupportDataMethod.FetchPart().DataSource.Table;
                dtProgrammeLevel = SupportDataMethod.FetchProgrammeLevel().DataSource.Table;

                IList<SUP_PART> liPart = new List<SUP_PART>();
                IList<SupProgrammeLevel> liProgrammeLevel = new List<SupProgrammeLevel>();
                if (dtPart != null && dtPart.Rows.Count > 0)
                {
                    liPart = (from DataRow dr in dtPart.Rows
                              select new SUP_PART()
                              {
                                  PART_ID = dr[Common.SUP_PART.PART_ID].ToString(),
                                  PART = dr[Common.SUP_PART.PART].ToString()
                              }).ToList();
                    objCourseType.PART_ID = new SelectList(liPart, Common.SUP_PART.PART_ID, Common.SUP_PART.PART);
                }
                if (dtProgrammeLevel != null && dtProgrammeLevel.Rows.Count > 0)
                {
                    liProgrammeLevel = (from DataRow dr in dtProgrammeLevel.Rows
                                        select new SupProgrammeLevel()
                                        {
                                            PROGRAMME_LEVEL_ID = dr[Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL_ID].ToString(),
                                            PROGRAMME_LEVEL = dr[Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL].ToString()
                                        }).ToList();
                    objCourseType.PROGRAMME_LEVEL = new SelectList(liProgrammeLevel, Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL_ID, Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL);
                }
            }
            return View(objCourseType);
        }
        [UserRights("ADMIN")]
        public ActionResult ListCoursetype()
        {
            AcademicsViewModel objCourseType = new AcademicsViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtCourseType = new DataTable();
            dtCourseType = objCourseType.FetchCourseTypeInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<CourseTypeInfo> liCourseType = new List<CourseTypeInfo>();
            if (dtCourseType != null && dtCourseType.Rows.Count > 0)
            {
                liCourseType = (from DataRow dr in dtCourseType.Rows
                                select new CourseTypeInfo()
                                {
                                    COURSE_TYPE_ID = dr[Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ID].ToString(),
                                    COURSE_TYPE = dr[Common.CP_COURSE_TYPE_2017.COURSE_TYPE].ToString(),
                                    NO_OF_COMPONENTS = dr[Common.CP_COURSE_TYPE_2017.NO_OF_COMPONENTS].ToString(),
                                    COURSE_TYPE_ORDER = dr[Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ORDER].ToString(),
                                    IS_STU_BASED_SELECTION = dr[Common.CP_COURSE_TYPE_2017.IS_STU_BASED_SELECTION].ToString(),
                                    INTERNAL = dr[Common.CP_COURSE_TYPE_2017.INTERNAL].ToString(),
                                    EXTERNAL = dr[Common.CP_COURSE_TYPE_2017.EXTERNAL].ToString(),
                                    TOTAL = dr[Common.CP_COURSE_TYPE_2017.TOTAL].ToString(),
                                    HOURS = dr[Common.CP_COURSE_TYPE_2017.HOURS].ToString(),
                                    PART_ID = dr[Common.SUP_PART.PART].ToString(),
                                    PROGRAMME_LEVEL = dr[Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL].ToString(),
                                    CREDITS = dr[Common.CP_COURSE_TYPE_2017.CREDITS].ToString(),
                                }).ToList();
            }
            return View(liCourseType);
        }
        public JsonResult InsertCourseTypeInfo(string sJsonCourseType)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (sJsonCourseType != null)
                {
                    DataTable dtCourseType = new DataTable();
                    var Result = JsonConvert.DeserializeObject<CourseTypeInfo>(sJsonCourseType);

                    dv.Clear();
                    dv.Add(Common.CP_COURSE_TYPE_2017.COURSE_TYPE, Result.COURSE_TYPE, EnumCommand.DataType.String);
                    using (AcademicsViewModel objCourseType = new AcademicsViewModel())
                    {
                        dtCourseType = objCourseType.CheckCourseType(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                        if (dtCourseType != null && dtCourseType.Rows.Count == 0)
                        {
                            dv.Clear();
                            dv.Add(Common.CP_COURSE_TYPE_2017.COURSE_TYPE, Result.COURSE_TYPE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.NO_OF_COMPONENTS, Result.NO_OF_COMPONENTS, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ORDER, Result.COURSE_TYPE_ORDER, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.IS_STU_BASED_SELECTION, Result.IS_STU_BASED_SELECTION, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.INTERNAL, Result.INTERNAL, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.EXTERNAL, Result.EXTERNAL, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.TOTAL, Result.TOTAL, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.HOURS, Result.HOURS, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.PART_ID, Result.PART_ID, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.PROGRAMME_LEVEL, Result.PROGRAMME_LEVEL, EnumCommand.DataType.String);
                            dv.Add(Common.CP_COURSE_TYPE_2017.CREDITS, Result.CREDITS, EnumCommand.DataType.String);
                            resultArg = objCourseType.InsertCourseTypeInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                            sResult = resultArg.Success.ToString();
                            if (sResult == "True")
                            {
                                sResult = "Records Saved Successfully";
                            }
                            else
                            {
                                sResult = "Enter Correct Values";
                            }
                        }
                        else
                        {
                            sResult = "Course Type Already Exists...!";
                        }
                    }
                }
            }
            return Json(sResult);
        }
        public ActionResult EditCourseTypeInfo(int id)
        {
            string sCourseTypeId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.COURSE_TYPE_ID] = sCourseTypeId;
            CourseTypeModel objCourseType = new CourseTypeModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtPart = new DataTable();
                DataTable dtProgrammeLevel = new DataTable();

                dtPart = SupportDataMethod.FetchPart().DataSource.Table;
                dtProgrammeLevel = SupportDataMethod.FetchProgrammeLevel().DataSource.Table;

                IList<SUP_PART> liPart = new List<SUP_PART>();
                IList<SupProgrammeLevel> liProgrammeLevel = new List<SupProgrammeLevel>();
                if (dtPart != null && dtPart.Rows.Count > 0)
                {
                    liPart = (from DataRow dr in dtPart.Rows
                              select new SUP_PART()
                              {
                                  PART_ID = dr[Common.SUP_PART.PART_ID].ToString(),
                                  PART = dr[Common.SUP_PART.PART].ToString()
                              }).ToList();
                    objCourseType.PART_ID = new SelectList(liPart, Common.SUP_PART.PART_ID, Common.SUP_PART.PART);
                }
                if (dtProgrammeLevel != null && dtProgrammeLevel.Rows.Count > 0)
                {
                    liProgrammeLevel = (from DataRow dr in dtProgrammeLevel.Rows
                                        select new SupProgrammeLevel()
                                        {
                                            PROGRAMME_LEVEL_ID = dr[Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL_ID].ToString(),
                                            PROGRAMME_LEVEL = dr[Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL].ToString()
                                        }).ToList();
                    objCourseType.PROGRAMME_LEVEL = new SelectList(liProgrammeLevel, Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL_ID, Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL);
                }
            }
            return View(objCourseType);
        }
        public JsonResult FetchCourseTypeInfoById(string id)
        {
            string sCourseTypeId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.COURSE_TYPE_ID] = sCourseTypeId;
            dv.Clear();
            dv.Add(Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ID, sCourseTypeId, EnumCommand.DataType.String);
            //AcademicsViewModel objDepartment = new AcademicsViewModel();
            DataTable dtFetchCourseTypeInfoById = new DataTable();
            using (AcademicsViewModel objCourseType = new AcademicsViewModel())
            {
                dtFetchCourseTypeInfoById = objCourseType.FetchCourseTypeInfoById(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            }
            if (dtFetchCourseTypeInfoById != null && dtFetchCourseTypeInfoById.Rows.Count > 0)
            {
                CourseTypeInfo objDepartment = new CourseTypeInfo()
                {
                    COURSE_TYPE_ID = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.COURSE_TYPE].ToString(),
                    COURSE_TYPE = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.COURSE_TYPE].ToString(),
                    NO_OF_COMPONENTS = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.NO_OF_COMPONENTS].ToString(),
                    COURSE_TYPE_ORDER = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ORDER].ToString(),
                    IS_STU_BASED_SELECTION = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.IS_STU_BASED_SELECTION].ToString(),
                    INTERNAL = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.INTERNAL].ToString(),
                    EXTERNAL = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.EXTERNAL].ToString(),
                    TOTAL = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.TOTAL].ToString(),
                    HOURS = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.HOURS].ToString(),
                    PART_ID = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.PART_ID].ToString(),
                    PROGRAMME_LEVEL = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.PROGRAMME_LEVEL].ToString(),
                    CREDITS = dtFetchCourseTypeInfoById.Rows[0][Common.CP_COURSE_TYPE_2017.CREDITS].ToString(),
                };
                return Json(objDepartment);
            }
            else
            {
                return Json(sResult);
            }

        }
        public ActionResult UpdateCourseTypeInfo(string JsonCourseType)
        {
            var sCourseType = Session[Common.SESSION_VARIABLES.COURSE_TYPE_ID].ToString();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            if (JsonCourseType != null)
            {
                var Result = JsonConvert.DeserializeObject<CourseTypeInfo>(JsonCourseType);
                dv.Clear();
                dv.Add(Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ID, sCourseType, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.COURSE_TYPE, Result.COURSE_TYPE, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.NO_OF_COMPONENTS, Result.NO_OF_COMPONENTS, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ORDER, Result.COURSE_TYPE_ORDER, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.IS_STU_BASED_SELECTION, Result.IS_STU_BASED_SELECTION, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.INTERNAL, Result.INTERNAL, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.EXTERNAL, Result.EXTERNAL, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.TOTAL, Result.TOTAL, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.HOURS, Result.HOURS, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.PART_ID, Result.PART_ID, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.PROGRAMME_LEVEL, Result.PROGRAMME_LEVEL, EnumCommand.DataType.String);
                dv.Add(Common.CP_COURSE_TYPE_2017.CREDITS, Result.CREDITS, EnumCommand.DataType.String);
                using (AcademicsViewModel objDepartment = new AcademicsViewModel())
                {
                    resultArg = objDepartment.UpdateCourseTypeInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                    sResult = resultArg.Success.ToString();
                }
            }
            return RedirectToAction("ListDepartmentInfo", "Academics");
        }
        public ActionResult DeleteCourseTypeInfo(string id)
        {
            using (AcademicsViewModel objCourseType = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_COURSE_TYPE_2017.COURSE_TYPE_ID, id, EnumCommand.DataType.String);
                resultArg = objCourseType.DeleteCourseTypeInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("ListCoursetype", "Academics");
        }
        #endregion
        #region Degree
        public ActionResult DegreeInfo()
        {
            DegreeModel objDegree = new DegreeModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtProgrammetype = new DataTable();
                DataTable dtProgrammeLevel = new DataTable();
                DataTable dtFaculty = new DataTable();

                dtProgrammeLevel = SupportDataMethod.FetchProgrammeLevel().DataSource.Table;
                dtProgrammetype = SupportDataMethod.FetchProgrammeType().DataSource.Table;
                dtFaculty = SupportDataMethod.FetchFaculty((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;

                IList<SupProgrammeLevel> liProgrammeLevel = new List<SupProgrammeLevel>();
                IList<SupProgrammeType> liProgrammeType = new List<SupProgrammeType>();
                IList<SupFaculty> liFaculty = new List<SupFaculty>();

                if (dtFaculty != null && dtFaculty.Rows.Count > 0)
                {
                    liFaculty = (from DataRow dr in dtFaculty.Rows
                                 select new SupFaculty()
                                 {
                                     FACULTY_ID = dr[Common.CP_FACULTY_2017.FACULTY_ID].ToString(),
                                     FACULTY = dr[Common.CP_FACULTY_2017.FACULTY].ToString()
                                 }).ToList();
                    objDegree.FACULTY = new SelectList(liFaculty, Common.CP_FACULTY_2017.FACULTY_ID, Common.CP_FACULTY_2017.FACULTY);
                }
                if (dtProgrammetype != null && dtProgrammetype.Rows.Count > 0)
                {
                    liProgrammeType = (from DataRow dr in dtProgrammetype.Rows
                                       select new SupProgrammeType()
                                       {
                                           PROGRAMME_TYPE_ID = dr[Common.SUP_PROGRAMME_TYPE.PROGRAMME_TYPE_ID].ToString(),
                                           PROGRAMME_TYPE = dr[Common.SUP_PROGRAMME_TYPE.PROGRAMME_TYPE].ToString()
                                       }).ToList();
                    objDegree.PROGRAMME_TYPE = new SelectList(liProgrammeType, Common.SUP_PROGRAMME_TYPE.PROGRAMME_TYPE_ID, Common.SUP_PROGRAMME_TYPE.PROGRAMME_TYPE);
                }
                if (dtProgrammeLevel != null && dtProgrammeLevel.Rows.Count > 0)
                {
                    liProgrammeLevel = (from DataRow dr in dtProgrammeLevel.Rows
                                        select new SupProgrammeLevel()
                                        {
                                            PROGRAMME_LEVEL_ID = dr[Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL_ID].ToString(),
                                            PROGRAMME_LEVEL = dr[Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL].ToString()
                                        }).ToList();
                    objDegree.PROGRAMME_LEVEL = new SelectList(liProgrammeLevel, Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL_ID, Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL);
                }

            }
            return View(objDegree);
        }
        [UserRights("ADMIN")]
        public ActionResult ListDegreeInfo()
        {
            AcademicsViewModel objDegree = new AcademicsViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtDegree = new DataTable();
            dtDegree = objDegree.FetchDegreeInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<DegreeInfo> liDegree = new List<DegreeInfo>();
            if (dtDegree != null && dtDegree.Rows.Count > 0)
            {
                liDegree = (from DataRow dr in dtDegree.Rows
                            select new DegreeInfo()
                            {
                                DEGREE_ID = dr[Common.CP_DEGREE_2017.DEGREE_ID].ToString(),
                                DEGREE = dr[Common.CP_DEGREE_2017.DEGREE].ToString(),
                                PROGRAMME_TYPE = dr[Common.CP_DEGREE_2017.PROGRAMME_TYPE].ToString(),
                                PROGRAMME_LEVEL = dr[Common.CP_DEGREE_2017.PROGRAMME_LEVEL].ToString(),
                                DEGREE_ORDER = dr[Common.CP_DEGREE_2017.DEGREE_ORDER].ToString(),
                                FACULTY = dr[Common.CP_DEGREE_2017.FACULTY].ToString(),
                                BOS_NAME = dr[Common.CP_DEGREE_2017.BOS_NAME].ToString(),
                                PREFIX = dr[Common.CP_DEGREE_2017.PREFIX].ToString(),
                                BOS_SERIES_ROLLNO = dr[Common.CP_DEGREE_2017.BOS_SERIES_ROLLNO].ToString(),
                                BOS_SERIES_REGNO = dr[Common.CP_DEGREE_2017.BOS_SERIES_REGNO].ToString(),
                                BOS_SERIES_ADMNO = dr[Common.CP_DEGREE_2017.BOS_SERIES_ADMNO].ToString(),
                            }).ToList();
            }
            return View(liDegree);
        }
        public JsonResult InsertDegreeInfo(string JsonDegree)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (JsonDegree != null)
                {
                    DataTable dtDegree = new DataTable();
                    var Result = JsonConvert.DeserializeObject<DegreeInfo>(JsonDegree);

                    dv.Clear();
                    dv.Add(Common.CP_DEGREE_2017.PREFIX, Result.PREFIX, EnumCommand.DataType.String);
                    using (AcademicsViewModel objDegree = new AcademicsViewModel())
                    {
                        dtDegree = objDegree.CheckDegree(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                        if (dtDegree != null && dtDegree.Rows.Count == 0)
                        {
                            dv.Clear();
                            dv.Add(Common.CP_DEGREE_2017.DEGREE, Result.DEGREE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEGREE_2017.PROGRAMME_TYPE, Result.PROGRAMME_TYPE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEGREE_2017.PROGRAMME_LEVEL, Result.PROGRAMME_LEVEL, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEGREE_2017.DEGREE_ORDER, Result.DEGREE_ORDER, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEGREE_2017.FACULTY, Result.FACULTY, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEGREE_2017.BOS_NAME, Result.BOS_NAME, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEGREE_2017.PREFIX, Result.PREFIX, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEGREE_2017.BOS_SERIES_ROLLNO, Result.BOS_SERIES_ROLLNO, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEGREE_2017.BOS_SERIES_REGNO, Result.BOS_SERIES_REGNO, EnumCommand.DataType.String);
                            dv.Add(Common.CP_DEGREE_2017.BOS_SERIES_ADMNO, Result.BOS_SERIES_ADMNO, EnumCommand.DataType.String);
                            resultArg = objDegree.InsertDegreeInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                            sResult = resultArg.Success.ToString();
                            if (sResult == "True")
                            {
                                sResult = "Records Saved Successfully";
                            }
                            else
                            {
                                sResult = "Enter Correct Values";
                            }
                        }
                        else
                        {
                            sResult = "Degree Already Exists";
                        }
                    }
                }
            }
            return Json(sResult);
        }
        public ActionResult EditDegreeInfo(int id)
        {
            string sDegreeId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.DEGREE_ID] = sDegreeId;
            DegreeModel objDegree = new DegreeModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtProgrammetype = new DataTable();
                DataTable dtProgrammeLevel = new DataTable();
                DataTable dtFaculty = new DataTable();

                dtProgrammeLevel = SupportDataMethod.FetchProgrammeLevel().DataSource.Table;
                dtProgrammetype = SupportDataMethod.FetchProgrammeType().DataSource.Table;
                dtFaculty = SupportDataMethod.FetchFaculty((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;

                IList<SupProgrammeLevel> liProgrammeLevel = new List<SupProgrammeLevel>();
                IList<SupProgrammeType> liProgrammeType = new List<SupProgrammeType>();
                IList<SupFaculty> liFaculty = new List<SupFaculty>();

                if (dtFaculty != null && dtFaculty.Rows.Count > 0)
                {
                    liFaculty = (from DataRow dr in dtFaculty.Rows
                                 select new SupFaculty()
                                 {
                                     FACULTY_ID = dr[Common.CP_FACULTY_2017.FACULTY_ID].ToString(),
                                     FACULTY = dr[Common.CP_FACULTY_2017.FACULTY].ToString()
                                 }).ToList();
                    objDegree.FACULTY = new SelectList(liFaculty, Common.CP_FACULTY_2017.FACULTY_ID, Common.CP_FACULTY_2017.FACULTY);
                }
                if (dtProgrammetype != null && dtProgrammetype.Rows.Count > 0)
                {
                    liProgrammeType = (from DataRow dr in dtProgrammetype.Rows
                                       select new SupProgrammeType()
                                       {
                                           PROGRAMME_TYPE_ID = dr[Common.SUP_PROGRAMME_TYPE.PROGRAMME_TYPE_ID].ToString(),
                                           PROGRAMME_TYPE = dr[Common.SUP_PROGRAMME_TYPE.PROGRAMME_TYPE].ToString()
                                       }).ToList();
                    objDegree.PROGRAMME_TYPE = new SelectList(liProgrammeType, Common.SUP_PROGRAMME_TYPE.PROGRAMME_TYPE_ID, Common.SUP_PROGRAMME_TYPE.PROGRAMME_TYPE);
                }
                if (dtProgrammeLevel != null && dtProgrammeLevel.Rows.Count > 0)
                {
                    liProgrammeLevel = (from DataRow dr in dtProgrammeLevel.Rows
                                        select new SupProgrammeLevel()
                                        {
                                            PROGRAMME_LEVEL_ID = dr[Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL_ID].ToString(),
                                            PROGRAMME_LEVEL = dr[Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL].ToString()
                                        }).ToList();
                    objDegree.PROGRAMME_LEVEL = new SelectList(liProgrammeLevel, Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL_ID, Common.SUP_PROGRAMME_LEVEL.PROGRAMME_LEVEL);
                }

            }
            return View(objDegree);
        }
        public JsonResult FetchDegreeInfoById(string id)
        {
            string sDegreeId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.DEGREE_ID] = sDegreeId;
            dv.Clear();
            dv.Add(Common.CP_DEGREE_2017.DEGREE_ID, sDegreeId, EnumCommand.DataType.String);
            //AcademicsViewModel objDepartment = new AcademicsViewModel();
            DataTable dtFetchDegreeInfoById = new DataTable();
            using (AcademicsViewModel objDegree = new AcademicsViewModel())
            {
                dtFetchDegreeInfoById = objDegree.FetchDegreeInfoById(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            }
            if (dtFetchDegreeInfoById != null && dtFetchDegreeInfoById.Rows.Count > 0)
            {
                DegreeInfo objDepartment = new DegreeInfo()
                {
                    DEGREE = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.DEGREE].ToString(),
                    PROGRAMME_TYPE = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.PROGRAMME_TYPE].ToString(),
                    PROGRAMME_LEVEL = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.PROGRAMME_LEVEL].ToString(),
                    DEGREE_ORDER = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.DEGREE_ORDER].ToString(),
                    FACULTY = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.FACULTY].ToString(),
                    BOS_NAME = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.BOS_NAME].ToString(),
                    PREFIX = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.PREFIX].ToString(),
                    BOS_SERIES_ROLLNO = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.BOS_SERIES_ROLLNO].ToString(),
                    BOS_SERIES_REGNO = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.BOS_SERIES_REGNO].ToString(),
                    BOS_SERIES_ADMNO = dtFetchDegreeInfoById.Rows[0][Common.CP_DEGREE_2017.BOS_SERIES_ADMNO].ToString(),

                };
                return Json(objDepartment);
            }
            else
            {
                return Json(sResult);
            }

        }
        public ActionResult UpdateDegreeInfo(string JsonDegree)
        {
            var sDegreeId = Session[Common.SESSION_VARIABLES.DEGREE_ID].ToString();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            if (JsonDegree != null)
            {
                var Result = JsonConvert.DeserializeObject<DegreeInfo>(JsonDegree);
                dv.Clear();
                dv.Add(Common.CP_DEGREE_2017.DEGREE_ID, sDegreeId, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.DEGREE, Result.DEGREE, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.PROGRAMME_TYPE, Result.PROGRAMME_TYPE, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.PROGRAMME_LEVEL, Result.PROGRAMME_LEVEL, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.FACULTY, Result.FACULTY, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.DEGREE_ORDER, Result.DEGREE_ORDER, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.BOS_NAME, Result.BOS_NAME, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.PREFIX, Result.PREFIX, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.BOS_SERIES_ROLLNO, Result.BOS_SERIES_ROLLNO, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.BOS_SERIES_REGNO, Result.BOS_SERIES_REGNO, EnumCommand.DataType.String);
                dv.Add(Common.CP_DEGREE_2017.BOS_SERIES_ADMNO, Result.BOS_SERIES_ADMNO, EnumCommand.DataType.String);
                using (AcademicsViewModel objDepartment = new AcademicsViewModel())
                {
                    resultArg = objDepartment.UpdateDegreeInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                    sResult = resultArg.Success.ToString();
                }
            }
            return RedirectToAction("ListDegreeInfo", "Academics");
        }
        public ActionResult DeleteDegreeInfo(string id)
        {
            using (AcademicsViewModel objDegree = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_DEGREE_2017.DEGREE_ID, id, EnumCommand.DataType.String);
                resultArg = objDegree.DeleteDegreeInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("ListDegreeInfo", "Academics");
        }
        #endregion
        #region Programme
        public ActionResult ProgrammeInfo()
        {
            ProgrammeModel objProgramme = new ProgrammeModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtDepartment = new DataTable();
                DataTable dtDegree = new DataTable();
                DataTable dtDurationUnit = new DataTable();
                DataTable dtChannel = new DataTable();
                DataTable dtSubjects = new DataTable();
                DataTable dtMediumOfInstruction = new DataTable();

                dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                dtDegree = SupportDataMethod.FetchDegree().DataSource.Table;
                dtDurationUnit = SupportDataMethod.FetchDurationUnit().DataSource.Table;
                dtChannel = SupportDataMethod.FetchChannel().DataSource.Table;
                dtSubjects = SupportDataMethod.FetchSubjects().DataSource.Table;
                dtMediumOfInstruction = SupportDataMethod.FetchMediumOfInstruction().DataSource.Table;

                IList<SupDepartment> liDepartment = new List<SupDepartment>();
                IList<SUP_DEGREE> liDegree = new List<SUP_DEGREE>();
                IList<Sup_Duration_Unit> liDurationUnit = new List<Sup_Duration_Unit>();
                IList<Sup_Channel> lichannel = new List<Sup_Channel>();
                IList<Sup_Subject> liSubjects = new List<Sup_Subject>();
                IList<SUP_MEDIUM_OF_INSTRUCTION> liMediumOfInstruction = new List<SUP_MEDIUM_OF_INSTRUCTION>();


                if (dtDepartment != null && dtDepartment.Rows.Count > 0)
                {
                    liDepartment = (from DataRow dr in dtDepartment.Rows
                                    select new SupDepartment()
                                    {
                                        DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                        DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                    }).ToList();
                    objProgramme.DEPARTMENT = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
                }
                if (dtDegree != null && dtDegree.Rows.Count > 0)
                {
                    liDegree = (from DataRow dr in dtDegree.Rows
                                select new SUP_DEGREE()
                                {
                                    DEGREE_ID = dr[Common.SUP_DEGREE.DEGREE_ID].ToString(),
                                    DEGREE = dr[Common.SUP_DEGREE.DEGREE].ToString()
                                }).ToList();
                    objProgramme.DEGREE = new SelectList(liDegree, Common.SUP_DEGREE.DEGREE_ID, Common.SUP_DEGREE.DEGREE);
                }
                if (dtDurationUnit != null && dtDurationUnit.Rows.Count > 0)
                {
                    liDurationUnit = (from DataRow dr in dtDurationUnit.Rows
                                      select new Sup_Duration_Unit()
                                      {
                                          DURATION_UNIT_ID = dr[Common.SUP_DURATION_UNIT.DURATION_UNIT_ID].ToString(),
                                          DURATION_UNIT = dr[Common.SUP_DURATION_UNIT.DURATION_UNIT].ToString()
                                      }).ToList();
                    objProgramme.DURATION_UNIT = new SelectList(liDurationUnit, Common.SUP_DURATION_UNIT.DURATION_UNIT_ID, Common.SUP_DURATION_UNIT.DURATION_UNIT);
                }
                if (dtChannel != null && dtChannel.Rows.Count > 0)
                {
                    lichannel = (from DataRow dr in dtChannel.Rows
                                 select new Sup_Channel()
                                 {
                                     CHANNEL_ID = dr[Common.SUP_CHANNEL.CHANNEL_ID].ToString(),
                                     CHANNEL = dr[Common.SUP_CHANNEL.CHANNEL].ToString()
                                 }).ToList();
                    objProgramme.CHANNEL = new SelectList(lichannel, Common.SUP_CHANNEL.CHANNEL_ID, Common.SUP_CHANNEL.CHANNEL);
                }
                if (dtSubjects != null && dtSubjects.Rows.Count > 0)
                {
                    liSubjects = (from DataRow dr in dtSubjects.Rows
                                  select new Sup_Subject()
                                  {
                                      SUBJECT_ID = dr[Common.SUP_SUBJECT.SUBJECT_ID].ToString(),
                                      SUBJECT = dr[Common.SUP_SUBJECT.SUBJECT].ToString()
                                  }).ToList();
                    objProgramme.SUBJECTS = new SelectList(liSubjects, Common.SUP_SUBJECT.SUBJECT_ID, Common.SUP_SUBJECT.SUBJECT);
                }
                if (dtMediumOfInstruction != null && dtMediumOfInstruction.Rows.Count > 0)
                {
                    liMediumOfInstruction = (from DataRow dr in dtMediumOfInstruction.Rows
                                             select new SUP_MEDIUM_OF_INSTRUCTION()
                                             {
                                                 MEDIUM_ID = dr[Common.SUP_MEDIUM_OF_INSTRUCTION.MEDIUM_ID].ToString(),
                                                 MEDIUM_OF_INSTRUCTION = dr[Common.SUP_MEDIUM_OF_INSTRUCTION.MEDIUM_OF_INSTRUCTION].ToString()
                                             }).ToList();
                    objProgramme.MEDIUM_OF_INSTRACTION = new SelectList(liMediumOfInstruction, Common.SUP_MEDIUM_OF_INSTRUCTION.MEDIUM_ID, Common.SUP_MEDIUM_OF_INSTRUCTION.MEDIUM_OF_INSTRUCTION);
                }
            }
            return View(objProgramme);
        }
        [UserRights("ADMIN")]
        public ActionResult listProgrammeInfo()
        {
            AcademicsViewModel objProgramme = new AcademicsViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtProgrammeList = new DataTable();
            dtProgrammeList = objProgramme.FetchProgrammeInfo((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            IList<ProgrammeInfo> liProgramme = new List<ProgrammeInfo>();
            if (dtProgrammeList != null && dtProgrammeList.Rows.Count > 0)
            {
                liProgramme = (from DataRow dr in dtProgrammeList.Rows
                               select new ProgrammeInfo()
                               {
                                   PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                   PROGRAMME_CODE = dr[Common.CP_PROGRAMME_2017.PROGRAMME_CODE].ToString(),
                                   PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString(),
                                   PROGRAMME_DESCRIPTION = dr[Common.CP_PROGRAMME_2017.PROGRAMME_DESCRIPTION].ToString(),
                                   DEPARTMENT = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString(),
                                   DEGREE = dr[Common.CP_DEGREE_2017.DEGREE].ToString(),
                                   PROGRAMME_ORDER = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ORDER].ToString(),
                                   DURATION_UNIT = dr[Common.SUP_DURATION_UNIT.DURATION_UNIT].ToString(),
                                   NO_OF_SEMESTER = dr[Common.CP_PROGRAMME_2017.NO_OF_SEMESTER].ToString(),
                                   CHANNEL = dr[Common.SUP_CHANNEL.CHANNEL].ToString(),
                                   IS_AIDED = dr[Common.SUP_IS_AIDED.IS_AIDED_NAME].ToString(),
                                   PRO_SERIES_ROLLNO = dr[Common.CP_PROGRAMME_2017.PRO_SERIES_ROLLNO].ToString(),
                                   NON_AIDED = dr[Common.SUP_NON_AIDED.NON_AIDED_NAME].ToString(),
                                   IS_REGULAR = dr[Common.SUP_IS_REGULAR.IS_REGULAR_NAME].ToString(),
                                   SUBJECTS = dr[Common.SUP_SUBJECT.SUBJECT].ToString(),
                                   PRO_SERIES_REGNO = dr[Common.CP_PROGRAMME_2017.PRO_SERIES_REGNO].ToString(),
                                   PRO_SERIES_ADMNO = dr[Common.CP_PROGRAMME_2017.PRO_SERIES_ADMNO].ToString(),
                                   MEDIUM_OF_INSTRACTION = dr[Common.SUP_MEDIUM_OF_INSTRUCTION.MEDIUM_OF_INSTRUCTION].ToString(),
                               }).ToList();
            }
            return View(liProgramme);
        }
        public JsonResult InsertProgrammeInfo(string sJsonProgramme)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (sJsonProgramme != null)
                {
                    DataTable dtProgramme = new DataTable();
                    var Result = JsonConvert.DeserializeObject<ProgrammeInfo>(sJsonProgramme);

                    dv.Clear();
                    dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_CODE, Result.PROGRAMME_CODE, EnumCommand.DataType.String);
                    using (AcademicsViewModel objProgramme = new AcademicsViewModel())
                    {
                        dtProgramme = objProgramme.CheckProgramme(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                        if (dtProgramme != null && dtProgramme.Rows.Count == 0)
                        {
                            dv.Clear();
                            dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_CODE, Result.PROGRAMME_CODE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_NAME, Result.PROGRAMME_NAME, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_DESCRIPTION, Result.PROGRAMME_DESCRIPTION, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.DEPARTMENT, Result.DEPARTMENT, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.DEGREE, Result.DEGREE, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_ORDER, Result.PROGRAMME_ORDER, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.DURATION_UNIT, Result.DURATION_UNIT, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.NO_OF_SEMESTER, Result.NO_OF_SEMESTER, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.CHANNEL, Result.CHANNEL, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.IS_AIDED, Result.IS_AIDED, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.PRO_SERIES_ROLLNO, Result.PRO_SERIES_ROLLNO, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.NON_AIDED, Result.NON_AIDED, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.IS_REGULAR, Result.IS_REGULAR, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.SUBJECTS, Result.SUBJECTS, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.PRO_SERIES_REGNO, Result.PRO_SERIES_REGNO, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.PRO_SERIES_ADMNO, Result.PRO_SERIES_ADMNO, EnumCommand.DataType.String);
                            dv.Add(Common.CP_PROGRAMME_2017.MEDIUM_OF_INSTRACTION, Result.MEDIUM_OF_INSTRACTION, EnumCommand.DataType.String);
                            resultArg = objProgramme.InsertProgrammeInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                            sResult = resultArg.Success.ToString();
                            if (sResult == "True")
                            {
                                sResult = "Records Saved Successfully";
                            }
                            else
                            {
                                sResult = "Enter Correct Values";
                            }
                        }
                        else
                        {
                            sResult = "Programme Already Exists...!";
                        }
                    }
                }
            }
            return Json(sResult);
        }
        public ActionResult EditProgrammeInfo(int id)
        {
            string sProgrammeId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.PROGRAMME_ID] = sProgrammeId;
            ProgrammeModel objProgramme = new ProgrammeModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                DataTable dtDepartment = new DataTable();
                DataTable dtDegree = new DataTable();
                DataTable dtDurationUnit = new DataTable();
                DataTable dtChannel = new DataTable();
                DataTable dtSubjects = new DataTable();
                DataTable dtMediumOfInstruction = new DataTable();

                dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                dtDegree = SupportDataMethod.FetchDegree().DataSource.Table;
                dtDurationUnit = SupportDataMethod.FetchDurationUnit().DataSource.Table;
                dtChannel = SupportDataMethod.FetchChannel().DataSource.Table;
                dtSubjects = SupportDataMethod.FetchSubjects().DataSource.Table;
                dtMediumOfInstruction = SupportDataMethod.FetchMediumOfInstruction().DataSource.Table;

                IList<SupDepartment> liDepartment = new List<SupDepartment>();
                IList<SUP_DEGREE> liDegree = new List<SUP_DEGREE>();
                IList<Sup_Duration_Unit> liDurationUnit = new List<Sup_Duration_Unit>();
                IList<Sup_Channel> lichannel = new List<Sup_Channel>();
                IList<Sup_Subject> liSubjects = new List<Sup_Subject>();
                IList<SUP_MEDIUM_OF_INSTRUCTION> liMediumOfInstruction = new List<SUP_MEDIUM_OF_INSTRUCTION>();


                if (dtDepartment != null && dtDepartment.Rows.Count > 0)
                {
                    liDepartment = (from DataRow dr in dtDepartment.Rows
                                    select new SupDepartment()
                                    {
                                        DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                        DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                    }).ToList();
                    objProgramme.DEPARTMENT = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
                }
                if (dtDegree != null && dtDegree.Rows.Count > 0)
                {
                    liDegree = (from DataRow dr in dtDegree.Rows
                                select new SUP_DEGREE()
                                {
                                    DEGREE_ID = dr[Common.SUP_DEGREE.DEGREE_ID].ToString(),
                                    DEGREE = dr[Common.SUP_DEGREE.DEGREE].ToString()
                                }).ToList();
                    objProgramme.DEGREE = new SelectList(liDegree, Common.SUP_DEGREE.DEGREE_ID, Common.SUP_DEGREE.DEGREE);
                }
                if (dtDurationUnit != null && dtDurationUnit.Rows.Count > 0)
                {
                    liDurationUnit = (from DataRow dr in dtDurationUnit.Rows
                                      select new Sup_Duration_Unit()
                                      {
                                          DURATION_UNIT_ID = dr[Common.SUP_DURATION_UNIT.DURATION_UNIT_ID].ToString(),
                                          DURATION_UNIT = dr[Common.SUP_DURATION_UNIT.DURATION_UNIT].ToString()
                                      }).ToList();
                    objProgramme.DURATION_UNIT = new SelectList(liDurationUnit, Common.SUP_DURATION_UNIT.DURATION_UNIT_ID, Common.SUP_DURATION_UNIT.DURATION_UNIT);
                }
                if (dtChannel != null && dtChannel.Rows.Count > 0)
                {
                    lichannel = (from DataRow dr in dtChannel.Rows
                                 select new Sup_Channel()
                                 {
                                     CHANNEL_ID = dr[Common.SUP_CHANNEL.CHANNEL_ID].ToString(),
                                     CHANNEL = dr[Common.SUP_CHANNEL.CHANNEL].ToString()
                                 }).ToList();
                    objProgramme.CHANNEL = new SelectList(lichannel, Common.SUP_CHANNEL.CHANNEL_ID, Common.SUP_CHANNEL.CHANNEL);
                }
                if (dtSubjects != null && dtSubjects.Rows.Count > 0)
                {
                    liSubjects = (from DataRow dr in dtSubjects.Rows
                                  select new Sup_Subject()
                                  {
                                      SUBJECT_ID = dr[Common.SUP_SUBJECT.SUBJECT_ID].ToString(),
                                      SUBJECT = dr[Common.SUP_SUBJECT.SUBJECT].ToString()
                                  }).ToList();
                    objProgramme.SUBJECTS = new SelectList(liSubjects, Common.SUP_SUBJECT.SUBJECT_ID, Common.SUP_SUBJECT.SUBJECT);
                }
                if (dtMediumOfInstruction != null && dtMediumOfInstruction.Rows.Count > 0)
                {
                    liMediumOfInstruction = (from DataRow dr in dtMediumOfInstruction.Rows
                                             select new SUP_MEDIUM_OF_INSTRUCTION()
                                             {
                                                 MEDIUM_ID = dr[Common.SUP_MEDIUM_OF_INSTRUCTION.MEDIUM_ID].ToString(),
                                                 MEDIUM_OF_INSTRUCTION = dr[Common.SUP_MEDIUM_OF_INSTRUCTION.MEDIUM_OF_INSTRUCTION].ToString()
                                             }).ToList();
                    objProgramme.MEDIUM_OF_INSTRACTION = new SelectList(liMediumOfInstruction, Common.SUP_MEDIUM_OF_INSTRUCTION.MEDIUM_ID, Common.SUP_MEDIUM_OF_INSTRUCTION.MEDIUM_OF_INSTRUCTION);
                }
            }
            return View(objProgramme);
        }
        public ActionResult UpdateProgrammeInfo(string JsonProgramme)
        {
            var sProgrammeId = Session[Common.SESSION_VARIABLES.PROGRAMME_ID].ToString();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            if (JsonProgramme != null)
            {
                var Result = JsonConvert.DeserializeObject<ProgrammeInfo>(JsonProgramme);
                dv.Clear();
                dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_CODE, Result.PROGRAMME_CODE, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_NAME, Result.PROGRAMME_NAME, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_DESCRIPTION, Result.PROGRAMME_DESCRIPTION, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.DEPARTMENT, Result.DEPARTMENT, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.DEGREE, Result.DEGREE, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_ORDER, Result.PROGRAMME_ORDER, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.DURATION_UNIT, Result.DURATION_UNIT, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.NO_OF_SEMESTER, Result.NO_OF_SEMESTER, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.CHANNEL, Result.CHANNEL, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.IS_AIDED, Result.IS_AIDED, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.PRO_SERIES_ROLLNO, Result.PRO_SERIES_ROLLNO, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.NON_AIDED, Result.NON_AIDED, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.IS_REGULAR, Result.IS_REGULAR, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.SUBJECTS, Result.SUBJECTS, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.PRO_SERIES_REGNO, Result.PRO_SERIES_REGNO, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.PRO_SERIES_ADMNO, Result.PRO_SERIES_ADMNO, EnumCommand.DataType.String);
                dv.Add(Common.CP_PROGRAMME_2017.MEDIUM_OF_INSTRACTION, Result.MEDIUM_OF_INSTRACTION, EnumCommand.DataType.String);
                using (AcademicsViewModel objDepartment = new AcademicsViewModel())
                {
                    resultArg = objDepartment.UpdateProgrammeInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                    sResult = resultArg.Success.ToString();
                }
            }
            return RedirectToAction("listProgrammeInfo", "Academics");
        }
        public JsonResult FetchProgrammeInfoById(string id)
        {
            string sProgrammeId = Convert.ToString(id);
            Session[Common.SESSION_VARIABLES.PROGRAMME_ID] = sProgrammeId;
            dv.Clear();
            dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_ID, sProgrammeId, EnumCommand.DataType.String);
            //AcademicsViewModel objDepartment = new AcademicsViewModel();
            DataTable dtFetchProgrammeInfoById = new DataTable();
            using (AcademicsViewModel objDepartment = new AcademicsViewModel())
            {
                dtFetchProgrammeInfoById = objDepartment.FetchProgrammeInfoById(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            }
            if (dtFetchProgrammeInfoById != null && dtFetchProgrammeInfoById.Rows.Count > 0)
            {
                ProgrammeInfo objDepartment = new ProgrammeInfo()
                {
                    PROGRAMME_CODE = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.PROGRAMME_CODE].ToString(),
                    PROGRAMME_NAME = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString(),
                    PROGRAMME_DESCRIPTION = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.PROGRAMME_DESCRIPTION].ToString(),
                    DEPARTMENT = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.DEPARTMENT].ToString(),
                    DEGREE = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.DEGREE].ToString(),
                    PROGRAMME_ORDER = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.PROGRAMME_ORDER].ToString(),
                    NON_AIDED = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.NON_AIDED].ToString(),
                    DURATION_UNIT = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.DURATION_UNIT].ToString(),
                    NO_OF_SEMESTER = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.NO_OF_SEMESTER].ToString(),
                    CHANNEL = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.CHANNEL].ToString(),
                    IS_AIDED = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.IS_AIDED].ToString(),
                    PRO_SERIES_ROLLNO = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.PRO_SERIES_ROLLNO].ToString(),
                    IS_REGULAR = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.IS_REGULAR].ToString(),
                    SUBJECTS = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.SUBJECTS].ToString(),
                    PRO_SERIES_REGNO = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.PRO_SERIES_REGNO].ToString(),
                    PRO_SERIES_ADMNO = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.PRO_SERIES_ADMNO].ToString(),
                    MEDIUM_OF_INSTRACTION = dtFetchProgrammeInfoById.Rows[0][Common.CP_PROGRAMME_2017.MEDIUM_OF_INSTRACTION].ToString(),
                };
                return Json(objDepartment);
            }
            else
            {
                return Json(sResult);
            }

        }
        public ActionResult DeleteProgrammeInfo(string id)
        {
            using (AcademicsViewModel objDepartment = new AcademicsViewModel())
            {
                dv.Clear();
                dv.Add(Common.CP_PROGRAMME_2017.PROGRAMME_ID, id, EnumCommand.DataType.String);
                resultArg = objDepartment.DeleteProgrammeInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("listProgrammeInfo", "Academics");
        }
        #endregion
        #region Class Course
        public JsonResult SaveClassCourses(string strJsonClassCourse)
        {
            string sResult = string.Empty;
            AcademicsViewModel objAcademicViewModel = new AcademicsViewModel();
            JsonClassCourseRoot objJsonClassCourseRoot = JsonConvert.DeserializeObject<JsonClassCourseRoot>(strJsonClassCourse);
            resultArg = objAcademicViewModel.SaveClassCourse(objJsonClassCourseRoot);
            sResult = (resultArg.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
            return Json(sResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteClassCourse(string sAcademicYear, string sClassCourse)
        {
            string sResult = string.Empty;
            using (AcademicsViewModel objAcademicViewModel = new AcademicsViewModel())
            {
                resultArg = objAcademicViewModel.DeleteClassCourse(sAcademicYear, sClassCourse);
                sResult = (resultArg.Success) ? Common.Messages.RecordDeletedSuccessfully : Common.Messages.FailedToDeletedTryAgain;
            }
            return Json(sResult, JsonRequestBehavior.AllowGet);
        }
        [UserRights("ADMIN")]
        public ActionResult ClassCourseList()
        {
            AcademicsViewModel objAcademicViewModel = new AcademicsViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();

                DataTable dtAcademicYearList = new DataTable();
                DataTable dtClasses = new DataTable();
                dtAcademicYearList = SupportDataMethod.FetchAcademicYearList().DataSource.Table;
                dtClasses = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;

                if (dtAcademicYearList != null && dtAcademicYearList.Rows.Count > 0)
                {
                    objAcademicViewModel.AcademicYearList = (from DataRow dr in dtAcademicYearList.Rows select new SUP_ACADEMIC_YEAR_LIST() { ACADEMIC_YEAR_ID = dr[Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID].ToString(), ACTIVE_YEAR = dr[Common.ACADEMIC_YEAR.ACTIVE_YEAR].ToString(), AC_YEAR = dr[Common.ACADEMIC_YEAR.AC_YEAR].ToString() }).ToList();
                    objAcademicViewModel.AcademicYearList.Add(new SUP_ACADEMIC_YEAR_LIST() { ACADEMIC_YEAR_ID = "0", ACTIVE_YEAR = "0", AC_YEAR = "--Select Academic Year--" });
                }
                else
                {
                    objAcademicViewModel.AcademicYearList.Add(new SUP_ACADEMIC_YEAR_LIST() { ACADEMIC_YEAR_ID = "0", ACTIVE_YEAR = "1", AC_YEAR = "--Select Academic Year--" });
                }
                if (dtClasses != null && dtClasses.Rows.Count > 0)
                {
                    objAcademicViewModel.ClassList = (from DataRow dr in dtClasses.Rows select new SUP_CLASS() { CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(), CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString() }).ToList();
                    objAcademicViewModel.ClassList.Add(new SUP_CLASS() { CLASS_ID = "0", CLASS_NAME = "--Select Class--" });
                }
                else
                {

                    objAcademicViewModel.ClassList = new List<SUP_CLASS>();

                    objAcademicViewModel.ClassList.Add(new SUP_CLASS() { CLASS_ID = "0", CLASS_NAME = "---Select---" });
                }
            }

            return View(objAcademicViewModel);
        }
        public ActionResult BindClassCourse(string sClassId, string sACYear)
        {
            AcademicsViewModel objAcademicViewModel = new AcademicsViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = string.Empty;
                sAcademicYear = (string.IsNullOrEmpty(sACYear) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : sACYear);
                DataTable dtClassCourseList = new DataTable();
                dtClassCourseList = objAcademicViewModel.FetchClassCourseList(sAcademicYear, sClassId).DataSource.Table;
                if (dtClassCourseList != null && dtClassCourseList.Rows.Count > 0)
                {
                    objAcademicViewModel.ClassCourseList = (from DataRow dr in dtClassCourseList.Rows
                                                            select new ClassCourse()
                                                            {
                                                                CLASS_COURSE_ID = dr[Common.CP_CLASS_COURSE_2017.CLASS_COURSE_ID].ToString(),
                                                                CLASS_ID = dr[Common.CP_CLASS_COURSE_2017.CLASS_ID].ToString(),
                                                                CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString(),
                                                                COURSE_CODE = dr[Common.CP_COURSE_ROOT_2017.COURSE_CODE].ToString(),
                                                                COURSE_ID = dr[Common.CP_CLASS_COURSE_2017.COURSE_ID].ToString(),
                                                                COURSE_TITLE = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString(),
                                                                COURSE_TYPE = dr[Common.CP_COURSE_TYPE_2017.COURSE_TYPE].ToString(),
                                                                IS_ALLIED_SUBJECT = dr[Common.CP_COURSE_ROOT_2017.IS_ALLIED_SUBJECT].ToString(),
                                                                IS_NME_SUBJECT = dr[Common.CP_COURSE_ROOT_2017.IS_NME_SUBJECT].ToString(),
                                                                SEMESTER_NAME = dr[Common.SUP_SEMESTER.SEMESTER_NAME].ToString(),
                                                                SHIFT_NAME = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString()
                                                            }).ToList();

                }
            }
            return View(objAcademicViewModel);
        }

        public JsonResult FetchClassCourseByClassCourseid(string sAcademicYear, string sClassCourse)
        {
            var objJsonClassCourse = new ClassCourseDDL();
            try
            {
                string sCourseId = string.Empty;
                string sClassId = string.Empty;
                AcademicsViewModel objAcademicViewModel = new AcademicsViewModel();
                DataTable dtClassType = new DataTable();
                DataTable dtCourseList = new DataTable();
                DataTable dtClassList = new DataTable();
                DataTable dtClassCourse = new DataTable();

                string sClassType = string.Empty;
                string sClassList = string.Empty;
                string sCourseList = string.Empty;

                dtClassType = SupportDataMethod.FetchClassType().DataSource.Table;
                dtClassList = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
                dtCourseList = SupportDataMethod.FetchAllCourseList(sAcademicYear).DataSource.Table;
                dtClassCourse = objAcademicViewModel.FetchClassCourseByClassCourseId(sAcademicYear, sClassCourse).DataSource.Table;
                if (dtClassCourse != null && dtClassCourse.Rows.Count > 0)
                {
                    sCourseId = dtClassCourse.Rows[0][Common.CP_CLASS_COURSE_2017.COURSE_ID].ToString();
                    sClassId = dtClassCourse.Rows[0][Common.CP_CLASS_COURSE_2017.CLASS_ID].ToString();
                }
                if (dtClassType != null)
                {
                    foreach (DataRow dr in dtClassType.Rows)
                    {
                        sClassType += "<option value='" + dr[Common.SUP_CLASS_TYPE.CLASS_TYPE_ID] + "' >" + dr[Common.SUP_CLASS_TYPE.CLASS_TYPE] + "</option>";
                    }
                }
                if (dtClassList != null)
                {
                    foreach (DataRow dr in dtClassList.Rows)
                    {
                        sClassList += "<option value='" + dr[Common.CP_CLASSES_2017.CLASS_ID] + "' " + (!string.Equals(sClassId, dr[Common.CP_CLASSES_2017.CLASS_ID].ToString()) ? string.Empty : "selected ") + " >" + dr[Common.CP_CLASSES_2017.CLASS_NAME] + "</option>";
                    }
                }
                if (dtCourseList != null)
                {
                    foreach (DataRow dr in dtCourseList.Rows)
                    {
                        sCourseList += "<option value='" + dr[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID] + "' " + (!string.Equals(sCourseId, dr[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID].ToString()) ? string.Empty : "selected ") + " >" + dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE] + "</option>";
                    }
                }
                objJsonClassCourse.sClassesList = sClassList;
                objJsonClassCourse.sCourseList = sCourseList;
                objJsonClassCourse.sClassType = sClassType;

                var obj = JsonConvert.SerializeObject(objJsonClassCourse);
                return Json(objJsonClassCourse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {


            }
            return Json(objJsonClassCourse);
        }

        public JsonResult FetchCourseListByClassId(string sAcademicYear, string sClassId)
        {
            AcademicsViewModel objAcademicViewModel = new AcademicsViewModel();
            var objJson = new ClassCourseDDL();
            string sCourseList = string.Empty;
            DataTable dtCourseList = new DataTable();
            dtCourseList = objAcademicViewModel.FetchCourseListByClassId(sAcademicYear, sClassId).DataSource.Table;

            if (dtCourseList != null && dtCourseList.Rows.Count > 0)

            {
                foreach (DataRow dr in dtCourseList.Rows)
                {
                    sCourseList += "<option value='" + dr[Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID] + "' >" + dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE] + "</option>";
                }
            }
            objJson.sCourseList = sCourseList;
            return Json(objJson, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
