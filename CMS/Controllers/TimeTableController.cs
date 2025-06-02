using CMS.DAO;
using CMS.SQL.Examination;
using CMS.SQL.SupportData;
using CMS.SQL.TimeTable;
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
    public class TimeTableController : Controller
    {
        // GET: TimeTable
        public ActionResult Index()
        {
            GenerateTimeTableForStaticHours();
            return View();
        }
        #region Methods
        private void GenerateTimeTableForStaticHours()
        {
            //Declaration
            string sAcademicYear = string.Empty;
            sAcademicYear = (string.IsNullOrEmpty(Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()) ? string.Empty : Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString());
            var settings = new List<TimeTableSetting>();
            TimeTableSetting objSetting = new TimeTableSetting();
            var departmentWiseTemplate = new TT_DEPARTMENT_WISE_TEMPLATE();
            var departmentWiseTemplates = new List<TT_DEPARTMENT_WISE_TEMPLATE>();
            var programme = new CP_PROGRAMME_2017();
            var programmes = new List<CP_PROGRAMME_2017>();
            var academicCurriculum = new ACADEMIC_CURRICULUM_2017();
            var academicCurriculums = new List<ACADEMIC_CURRICULUM_2017>();
            var academicBatch = new AcademicBatches();
            var academicBatches = new List<AcademicBatches>();
            var timeTableTemplate = new TimeTableTemplate();
            var timeTableTemplates = new List<TimeTableTemplate>();
            var objClass = new CP_CLASSES();
            var Classes = new List<CP_CLASSES>();

            var paperTypes = (List<SupPaperType>)CMSHandler.FetchData<SupPaperType>(null, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchPaparType)).DataSource.Data;
            academicBatches = (List<AcademicBatches>)CMSHandler.FetchData<AcademicBatches>(null, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchAcademicBatches)).DataSource.Data;

            objSetting.ACADEMIC_YEAR = sAcademicYear;

            settings = (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(objSetting, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTimeTableSettings)).DataSource.Data;
            if (settings != null && settings.Count() > 0)
            {
                departmentWiseTemplate.Academic_Year = sAcademicYear;

                foreach (var setting in settings)
                {
                    departmentWiseTemplate.SETTING_ID = setting.SETTING_ID;
                    departmentWiseTemplates = (List<TT_DEPARTMENT_WISE_TEMPLATE>)CMSHandler.FetchData<TT_DEPARTMENT_WISE_TEMPLATE>(departmentWiseTemplate, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchDepartmentBySettingId)).DataSource.Data;
                    if (departmentWiseTemplates != null && departmentWiseTemplates.Count() > 0)
                    {
                        foreach (var template in departmentWiseTemplates)
                        {
                            programme.DEPARTMENT = template.DEPARTMENT_ID;
                            programme.PROGRAMME_LEVEL = setting.GRADUATION_TYPE;
                            programmes = (List<CP_PROGRAMME_2017>)CMSHandler.FetchData<CP_PROGRAMME_2017>(programme, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchProgrammesByDepartmentAndGraduationType), sAcademicYear).DataSource.Data;
                            foreach (var prog in programmes)
                            {
                                foreach (var batch in academicBatches)
                                {
                                    academicCurriculum.BATCH = batch.BATCH_ID;
                                    academicCurriculum.PROGRAMME = prog.PROGRAMME_ID;
                                    academicCurriculums = (List<ACADEMIC_CURRICULUM_2017>)CMSHandler.FetchData<ACADEMIC_CURRICULUM_2017>(academicCurriculum, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchAcademicCurrilum), sAcademicYear).DataSource.Data;
                                    timeTableTemplate.Setting_Id = setting.SETTING_ID;
                                    timeTableTemplate.Year = batch.BATCH_YEAR;
                                    timeTableTemplates = (List<TimeTableTemplate>)CMSHandler.FetchData<TimeTableTemplate>(timeTableTemplate, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTemplateOnlyStaticHours), sAcademicYear).DataSource.Data;

                                    objClass.PROGRAMME = programme.PROGRAMME_ID;
                                    Classes = (List<CP_CLASSES>)CMSHandler.FetchData<CP_CLASSES>(objClass, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchClassesByProgrammeId), sAcademicYear).DataSource.Data;

                                    if (Classes != null && Classes.Count() > 0)
                                    {
                                        if (academicCurriculums != null && academicCurriculums.Count() > 0)
                                        {
                                            if (timeTableTemplates != null && timeTableTemplates.Count() > 0)
                                            {
                                                foreach (var cls in Classes)
                                                {
                                                    foreach (var paperType in paperTypes)
                                                    {
                                                        var temp = (List<TimeTableTemplate>)timeTableTemplates.Where(o => o.Paper_Type_Id.Equals(paperType.PAPER_TYPE_ID));
                                                        if (temp != null && temp.Count() > 0)
                                                        {

                                                            var objTimeTable = new TimeTable();

                                                            var objCourses = academicCurriculums.Where(o => o.PAPER_TYPE == paperType.PAPER_TYPE_ID);
                                                            foreach (var tt in temp)
                                                            {
                                                                if (objCourses != null && objCourses.Count() > 0)
                                                                {
                                                                    foreach (var course in objCourses)
                                                                    {
                                                                        objTimeTable.Class_Id = cls.CLASS_ID;
                                                                        objTimeTable.Course_Id = course.COURSE_ID;
                                                                        objTimeTable.Day_Order_Id = tt.Day_Order_Id;
                                                                        objTimeTable.Hour_Id = tt.Hour_Id;
                                                                        objTimeTable.Is_Active = tt.Is_Active;
                                                                        objTimeTable.Semester_Id = course.SEMESTER;
                                                                        CMSHandler.SaveOrUpdate(objTimeTable, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }


                                }
                            }

                        }
                    }
                }

            }



            //     var staticTamplate = CMSHandler.FetchData<TimeTableTemplate>(timeTableTemplate, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTemplateOnlyStaticHours)).DataSource.Data;

        }

        #endregion
        #region Time Table Crude
        public ActionResult TimeTableClassWiseList()
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var FetchClass = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
            objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            return View(objViewModel);
        }
        public ActionResult BindTimeTableInfo(string sClassId)
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            TimeTable objModel = new TimeTable();
            var BindStudentCourseInfo = new List<TimeTable>();
            objModel.Class_Id = sClassId;
            BindStudentCourseInfo = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTimeTableClassWiseListByClassId), sAcademicYear).DataSource.Data;
            var FetchHours = (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(null, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchHoursByTimeTableHours), sAcademicYear).DataSource.Data;
            var FetchDayOrders = (List<SUP_DAY_ORDER>)CMSHandler.FetchData<SUP_DAY_ORDER>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDayOrders)).DataSource.Data;
            objViewModel.lstHours = FetchHours;
            objViewModel.lstTimeTable = BindStudentCourseInfo;
            objViewModel.lstDayOrders = FetchDayOrders;
            return View(objViewModel);
        }
        public JsonResult EditClassWiseTimeTable(string sClassId, string sHourId, string sDayId, string sHourType, string sSemesterId, string sCourseId, string sStaffId)
        {
            string sOption = string.Empty;
            TimeTable objModel = new TimeTable();
            objModel.Class_Id = sClassId;
            objModel.Hour_Id = sHourId;
            objModel.hourType = sHourType;
            objModel.Day_Order_Id = sDayId;
            objModel.Course_Id = sCourseId;
            objModel.Staff_Id = sStaffId;
            objModel.Semester_Id = sSemesterId;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var CheckIsRegularType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.CheckIsRegularPaper), sAcademicYear).DataSource.Data;
            if (CheckIsRegularType != null && CheckIsRegularType.Count > 0)
            {
                objModel.Time_Table_Id = CheckIsRegularType.SingleOrDefault().Time_Table_Id;
                var FetchRegularPaperTimeTableById = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTimeTableById), sAcademicYear).DataSource.Data;

                //objModel.Time_Table_Id = FetchRegularPaperTimeTableById.FirstOrDefault().Time_Table_Id;
                objModel.CLASS_CODE = FetchRegularPaperTimeTableById.FirstOrDefault().CLASS_CODE;
                //objModel.Course_Id = FetchRegularPaperTimeTableById.FirstOrDefault().Course_Id;
                //objModel.Staff_Id = FetchRegularPaperTimeTableById.FirstOrDefault().Staff_Id;
            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        public string GetCourseAndStaff(string sClassId)
        {
            string sCourse = string.Empty;
            string sHourType = string.Empty;
            string sSemester = string.Empty;
            string sStaff = string.Empty;
            string JsonData = string.Empty;
            TimeTable objModel = new TimeTable();
            objModel.Class_Id = sClassId;
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                objModel.Staff_Id = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                var FetchStaff = (List<stf_Personal_Info>)CMSHandler.FetchData<stf_Personal_Info>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStaff), sAcademicYear).DataSource.Data;
                var FetchCourse = (List<CourseWiseStaffInfo>)CMSHandler.FetchData<CourseWiseStaffInfo>(objModel, ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseList), sAcademicYear).DataSource.Data;
                var HourType = (List<SUP_HOUR_TYPE>)CMSHandler.FetchData<SUP_HOUR_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHourType)).DataSource.Data;
                //var Semester = (List<SupSemester>)CMSHandler.FetchData<SupSemester>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemester)).DataSource.Data;
                if (FetchStaff != null && FetchStaff.Count > 0)
                {
                    foreach (var item in FetchStaff)
                        sStaff += "<option value='" + item.STAFF_ID + "'>" + item.FIRST_NAME + "</option>";
                }
                if (FetchCourse != null && FetchCourse.Count > 0)
                {
                    foreach (var list in FetchCourse)
                    {
                        sCourse += "<option value='" + list.COURSE_ID + "'>" + list.COURSE_TITLE + "</option>";
                        sSemester = list.SEMESTER_ID;
                    }
                }
                if (HourType != null && HourType.Count > 0)
                {
                    foreach (var DataItem in HourType)
                        sHourType += "<option value='" + DataItem.hour_type_id + "'>" + DataItem.hourType + "</option>";
                }
                var objJsonData = new TimeTable() { Course_Id = sCourse, Staff_Id = sStaff, hourType = sHourType, Semester_Id = sSemester };
                JsonData = JsonConvert.SerializeObject(objJsonData);
            }
            return JsonData;
        }
        public string GetStaffByCourseId(string sCourseId, string sClassId)
        {
            string sOption = string.Empty;
            string JsonData = string.Empty;
            TimeTable objModel = new TimeTable();
            objModel.Course_Id = sCourseId;
            objModel.Class_Id = sClassId;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var CheckIsOptionalPaper = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.CheckIsOptionalPaper), sAcademicYear).DataSource.Data;
            if (CheckIsOptionalPaper != null && CheckIsOptionalPaper.Count > 0)
            {
                var FetchStaff = (List<stf_Personal_Info>)CMSHandler.FetchData<stf_Personal_Info>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchStaffByCourseId), sAcademicYear).DataSource.Data;
                if (FetchStaff != null && FetchStaff.Count > 0)
                {
                    foreach (var item in FetchStaff)
                        sOption += "<option value='" + item.STAFF_ID + "'>" + item.FIRST_NAME + "</option>";
                }
            }
            else
            {
                var FetchStaff = (List<stf_Personal_Info>)CMSHandler.FetchData<stf_Personal_Info>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchStaffByCourseIdAndClassId), sAcademicYear).DataSource.Data;
                if (FetchStaff != null && FetchStaff.Count > 0)
                {
                    foreach (var item in FetchStaff)
                        sOption += "<option value='" + item.STAFF_ID + "'>" + item.FIRST_NAME + "</option>";
                }
            }
            var objJsonData = new TimeTable() { Staff_Id = sOption };
            JsonData = JsonConvert.SerializeObject(objJsonData);
            return JsonData;
        }
        public string GetCourseByClassId(string sClassId)
        {
            string sOption = string.Empty;
            string JsonData = string.Empty;
            TimeTable objModel = new TimeTable();
            objModel.Class_Id = sClassId;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var FetchCourse = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchCourseByClassId), sAcademicYear).DataSource.Data;
            if (FetchCourse != null && FetchCourse.Count > 0)
            {
                foreach (var list in FetchCourse)
                    sOption += "<option value='" + list.COURSE_ROOT_ID + "'>" + list.COURSE_TITLE + "</option>";
            }
            var objJsonData = new TimeTable() { Course_Id = sOption };
            JsonData = JsonConvert.SerializeObject(objJsonData);
            return JsonData;
        }
        public string UpdateClassWiseTimeTable(string sCourseId, string sClassId, string sTimeTableId, string sStaffId, string sDayOrderId, string sHourId, string sHourType, string sSemesterId)
        {
            string sOption = string.Empty;
            TimeTable objModel = new TimeTable();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {

                    objModel.Course_Id = sCourseId;
                    objModel.Class_Id = sClassId;
                    objModel.Staff_Id = sStaffId;
                    objModel.Time_Table_Id = sTimeTableId;
                    objModel.Day_Order_Id = sDayOrderId;
                    objModel.Hour_Id = sHourId;
                    objModel.hourType = sHourType;
                    objModel.Semester_Id = sSemesterId;
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                    if (!string.IsNullOrEmpty(objModel.Staff_Id))
                    {
                        var FetchIsStaticHourAndPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchIsStaticAndPaperTypeByClassId), sAcademicYear).DataSource.Data;
                        if (FetchIsStaticHourAndPaperType.FirstOrDefault().IS_STATIC == Common.IsActiveFalg.IsNotActive)
                        {
                            ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.UpdateTimeTableInfo), sAcademicYear);
                            if (ResultArgs.Success)
                            {
                                sOption = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                sOption = Common.Messages.FailedToSaveRecords;
                            }
                        }
                        else
                        {
                            var FetchPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objModel, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchPaperType), sAcademicYear).DataSource.Data;
                            var FetchHourType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchHourType), sAcademicYear).DataSource.Data;
                            if (FetchPaperType != null && FetchPaperType.Count > 0 && FetchHourType != null && FetchHourType.Count > 0)
                            {
                                objModel.PAPER_TYPE = FetchPaperType.SingleOrDefault().PAPER_TYPE;
                                objModel.Paper_Type_Id = FetchHourType.SingleOrDefault().Paper_Type_Id;
                                if (objModel.PAPER_TYPE == objModel.Paper_Type_Id)
                                {
                                    ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.UpdateTimeTableInfo), sAcademicYear);
                                    if (ResultArgs.Success)
                                    {
                                        sOption = Common.Messages.RecordsSavedSuccessfully;
                                    }
                                    else
                                    {
                                        sOption = Common.Messages.FailedToSaveRecords;
                                    }
                                }
                                else
                                {
                                    sOption = "This Course is not eligible for this hour...!!!";
                                }
                            }
                            else
                            {
                                sOption = "No paper type and hour type for this course...!!!";
                            }
                        }
                    }
                    else
                    {
                        sOption = Common.Messages.FailedToSaveRecords;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("TimeTableController", "UpdateClassWiseTimeTable", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("TimeTableController", "UpdateClassWiseTimeTable", ex.Message);
                    }
                }
            }
            else
            {
                sOption = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return sOption;
        }
        public async Task<ActionResult> ClassWiseTimeTableInfo()
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            var Class = new List<cp_Classes_2017>();
            var Course = new List<cp_Course_Root_2017>();
            var Staff = new List<stf_Personal_Info>();
            var HourType = new List<SUP_HOUR_TYPE>();
            var Hour = new List<SUP_HOURS>();
            var Semester = new List<SupSemester>();
            var DayOrder = new List<SUP_DAY_ORDER>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Class = await Task.Run(() => (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data);
            Course = await Task.Run(() => (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOnlyRegularCoursesList), sAcademicYear).DataSource.Data);
            Staff = await Task.Run(() => (List<stf_Personal_Info>)CMSHandler.FetchData<stf_Personal_Info>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchStaff), sAcademicYear).DataSource.Data);
            HourType = await Task.Run(() => (List<SUP_HOUR_TYPE>)CMSHandler.FetchData<SUP_HOUR_TYPE>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHourType)).DataSource.Data);
            //Hour = await Task.Run(() => (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHours)).DataSource.Data);
            Hour = await Task.Run(() => (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(null, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchHoursByTimeTableHours), sAcademicYear).DataSource.Data);
            Semester = await Task.Run(() => (List<SupSemester>)CMSHandler.FetchData<SupSemester>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemester)).DataSource.Data);
            DayOrder = await Task.Run(() => (List<SUP_DAY_ORDER>)CMSHandler.FetchData<SUP_DAY_ORDER>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDayOrders)).DataSource.Data);
            objViewModel.ClassList = new SelectList(Class, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            objViewModel.StaffList = new SelectList(Staff, Common.STF_PERSONAL_INFO.STAFF_ID, Common.STF_PERSONAL_INFO.FIRST_NAME);
            objViewModel.HourType = new SelectList(HourType, Common.SUP_HOUR_TYPE.HOUR_TYPE_ID, Common.SUP_HOUR_TYPE.HOURTYPE);
            objViewModel.HourList = new SelectList(Hour, Common.SUP_HOURS.HOUR_ID, Common.SUP_HOURS.HOUR);
            objViewModel.CourseList = new SelectList(Course, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
            objViewModel.SemesterList = new SelectList(Semester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
            objViewModel.DayOrderList = new SelectList(DayOrder, Common.SUP_DAY_ORDERS.DAY_ORDER_ID, Common.SUP_DAY_ORDERS.DAY);
            return View(objViewModel);
        }
        public string SaveClassWiseTimeTable(string TimeTableInfo)
        {
            string sOption = string.Empty;
            if (TimeTableInfo != null)
            {
                var Result = JsonConvert.DeserializeObject<TimeTable>(TimeTableInfo);
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                var FetchSelectedClassId = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchSelectedClassIdByCourseIdAndClassId), sAcademicYear).DataSource.Data;
                if (FetchSelectedClassId != null && FetchSelectedClassId.Count > 0)
                {
                    Result.CLASS_CODE = Result.Class_Id;
                    Result.Class_Id = FetchSelectedClassId.FirstOrDefault().S_CLASS_ID;
                    var FetchExistingTimeTable = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.IsTimeTableInfoExistingForOptionalPaper), sAcademicYear).DataSource.Data;
                    if (FetchExistingTimeTable != null && FetchExistingTimeTable.Count > 0)
                    {
                        sOption = "This Hour Is Already Alloted...!!!";
                    }
                    else
                    {
                        var FetchAllotedStaff = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.CheckIsStaffAlloted), sAcademicYear).DataSource.Data;
                        if (FetchAllotedStaff != null && FetchAllotedStaff.Count > 0)
                        {
                            sOption = "This Staff Already Alloted To Another Class";
                        }
                        else
                        {
                            Result.Class_Id = Result.CLASS_CODE;
                            var FetchPaperType = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(Result, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchPaperType), sAcademicYear).DataSource.Data;
                            var FetchHourType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchHourType), sAcademicYear).DataSource.Data;
                            if (FetchPaperType != null && FetchPaperType.Count > 0 && FetchHourType != null && FetchHourType.Count > 0)
                            {
                                Result.PAPER_TYPE = FetchPaperType.SingleOrDefault().PAPER_TYPE;
                                Result.Paper_Type_Id = FetchHourType.SingleOrDefault().Paper_Type_Id;
                                if (Result.PAPER_TYPE == Result.Paper_Type_Id)
                                {
                                    ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                    if (ResultArgs.Success)
                                    {
                                        sOption = Common.Messages.RecordsSavedSuccessfully;
                                    }
                                    else
                                    {
                                        sOption = Common.Messages.FailedToSaveRecords;
                                    }
                                }
                                else
                                {
                                    sOption = "This Course is not eligible to assign to this hour...!!!";
                                }
                            }
                            else
                            {
                                sOption = "No paper type and hour type for this course...!!!";
                            }
                        }
                    }

                }
                else
                {
                    var FetchExistingTimeTable = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.IsTimeTableInfoExisting), sAcademicYear).DataSource.Data;
                    if (FetchExistingTimeTable != null && FetchExistingTimeTable.Count > 0)
                    {
                        sOption = "This Hour Is Already Alloted...!!!";
                    }
                    else
                    {
                        var FetchAllotedStaff = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.CheckIsStaffAlloted), sAcademicYear).DataSource.Data;
                        if (FetchAllotedStaff != null && FetchAllotedStaff.Count > 0)
                        {
                            sOption = "This Staff Already Alloted To Another Class";
                        }
                        else
                        {
                            if (Result.Course_Id == "0")
                            {
                                var FetchIsStaticHourAndPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchIsStaticAndPaperTypeByClassId), sAcademicYear).DataSource.Data;
                                if (FetchIsStaticHourAndPaperType.FirstOrDefault().IS_STATIC == "0")
                                {
                                    ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                    if (ResultArgs.Success)
                                    {
                                        sOption = Common.Messages.RecordsSavedSuccessfully;
                                    }
                                    else
                                    {
                                        sOption = Common.Messages.FailedToSaveRecords;
                                    }
                                }
                            }
                            else
                            {
                                var FetchIsStaticHourAndPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchIsStaticAndPaperTypeByClassId), sAcademicYear).DataSource.Data;
                                if (FetchIsStaticHourAndPaperType != null && FetchIsStaticHourAndPaperType.Count > 0)
                                {
                                    if (FetchIsStaticHourAndPaperType.FirstOrDefault().IS_STATIC == "0")
                                    {
                                        ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                        if (ResultArgs.Success)
                                        {
                                            sOption = Common.Messages.RecordsSavedSuccessfully;
                                        }
                                        else
                                        {
                                            sOption = Common.Messages.FailedToSaveRecords;
                                        }
                                    }
                                    else
                                    {
                                        var FetchPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchPaperType), sAcademicYear).DataSource.Data;
                                        var FetchHourType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchHourType), sAcademicYear).DataSource.Data;
                                        if (FetchPaperType != null && FetchPaperType.Count > 0 && FetchHourType != null && FetchHourType.Count > 0)
                                        {
                                            Result.PAPER_TYPE = FetchPaperType.SingleOrDefault().PAPER_TYPE;
                                            Result.Paper_Type_Id = FetchHourType.SingleOrDefault().Paper_Type_Id;
                                            if (Result.PAPER_TYPE == Result.Paper_Type_Id)
                                            {
                                                ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                                if (ResultArgs.Success)
                                                {
                                                    sOption = Common.Messages.RecordsSavedSuccessfully;
                                                }
                                                else
                                                {
                                                    sOption = Common.Messages.FailedToSaveRecords;
                                                }
                                            }
                                            else
                                            {
                                                sOption = "This Course is not eligible to assign to this hour...!!!";
                                            }
                                        }
                                        else
                                        {
                                            sOption = "No paper type and hour type for this course...!!!";
                                        }
                                    }
                                }
                                else
                                {
                                    sOption = "Please Assign The Hour In TimeTable Template...!!!";
                                }
                            }

                        }
                    }
                }
            }
            return sOption;
        }
        #endregion
        #region Time Table Setting
        public async Task<ActionResult> TimeTableSettingList()
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var TimeTableSetting = new TimeTableSetting();
            var ListTimeTableSettings = await Task.Run(() => (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(null, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchAllTimeTableSettings), sAcademicYear).DataSource.Data);
            objViewModel.lstTimeTableSetting = ListTimeTableSettings;
            return View(objViewModel);
        }
        public async Task<ActionResult> TimeTableSetting()
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            var Semester = new List<SupSemester>();
            var Hours = new List<SUP_HOURS>();
            var Days = new List<SUP_DAY_ORDER>();
            var GraduationType = new List<SupClassLevel>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Semester = await Task.Run(() => (List<SupSemester>)CMSHandler.FetchData<SupSemester>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemester)).DataSource.Data);
            Days = await Task.Run(() => (List<SUP_DAY_ORDER>)CMSHandler.FetchData<SUP_DAY_ORDER>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDayOrders)).DataSource.Data);
            Hours = await Task.Run(() => (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHours)).DataSource.Data);
            GraduationType = await Task.Run(() => (List<SupClassLevel>)CMSHandler.FetchData<SupClassLevel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassLevel)).DataSource.Data);
            objViewModel.SemesterList = new SelectList(Semester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
            objViewModel.HourList = new SelectList(Hours, Common.SUP_HOURS.HOUR_ID, Common.SUP_HOURS.HOUR);
            objViewModel.DayOrderList = new SelectList(Days, Common.SUP_DAY_ORDERS.DAY_ORDER_ID, Common.SUP_DAY_ORDERS.DAY);
            objViewModel.GraduationType = new SelectList(GraduationType, Common.SUP_CLASS_LEVEL.CLASSLEVELID, Common.SUP_CLASS_LEVEL.CLASSLEVEL);
            return View(objViewModel);
        }
        public string SaveTimeTableSetting(string TimeTableSetting)
        {
            string sOption = string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            if (TimeTableSetting != null)
            {
                var Result = JsonConvert.DeserializeObject<TimeTableSetting>(TimeTableSetting);
                var FetchExistingSetting = (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.IsTimeTableSettingExisting), sAcademicYear).DataSource.Data;
                if (FetchExistingSetting != null)
                {
                    sOption = "This Setting Is Existing Already...!";
                }
                else
                {
                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTableSettings), sAcademicYear);
                    if (ResultArg.Success)
                    {
                        sOption = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sOption = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            return sOption;
        }
        public async Task<ActionResult> EditTimeTableSetting(string id)
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            Session[Common.SESSION_VARIABLES.SETTING_ID] = id;
            var Semester = new List<SupSemester>();
            var Hours = new List<SUP_HOURS>();
            var Days = new List<SUP_DAY_ORDER>();
            var GraduationType = new List<SupClassLevel>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Semester = await Task.Run(() => (List<SupSemester>)CMSHandler.FetchData<SupSemester>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemester)).DataSource.Data);
            Days = await Task.Run(() => (List<SUP_DAY_ORDER>)CMSHandler.FetchData<SUP_DAY_ORDER>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDayOrders)).DataSource.Data);
            Hours = await Task.Run(() => (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHours)).DataSource.Data);
            GraduationType = await Task.Run(() => (List<SupClassLevel>)CMSHandler.FetchData<SupClassLevel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassLevel)).DataSource.Data);
            objViewModel.SemesterList = new SelectList(Semester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
            objViewModel.HourList = new SelectList(Hours, Common.SUP_HOURS.HOUR_ID, Common.SUP_HOURS.HOUR);
            objViewModel.DayOrderList = new SelectList(Days, Common.SUP_DAY_ORDERS.DAY_ORDER_ID, Common.SUP_DAY_ORDERS.DAY);
            objViewModel.GraduationType = new SelectList(GraduationType, Common.SUP_CLASS_LEVEL.CLASSLEVELID, Common.SUP_CLASS_LEVEL.CLASSLEVEL);
            return View(objViewModel);
        }
        public JsonResult FetchTimeTableSettingById(string sSettingId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            TimeTableSetting objModel = new TimeTableSetting();
            objModel.SETTING_ID = sSettingId;
            var FetchTimeTableSetting = (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTimeTableSettingById), sAcademicYear).DataSource.Data;
            return Json(FetchTimeTableSetting);
        }
        public string UpdateTimeTableSetting(string TimeTableSetting)
        {
            string sOption = string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            if (TimeTableSetting != null)
            {
                var Result = JsonConvert.DeserializeObject<TimeTableSetting>(TimeTableSetting);
                Result.SETTING_ID = (Session[Common.SESSION_VARIABLES.SETTING_ID] != null) ? Session[Common.SESSION_VARIABLES.SETTING_ID].ToString() : "";
                var FetchExistingSetting = (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.IsTimeTableSettingExisting), sAcademicYear).DataSource.Data;
                if (FetchExistingSetting != null)
                {
                    sOption = "This Setting Is Existing Already...!";
                }
                else
                {
                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.UpdateTimeTableSettings), sAcademicYear);
                    if (ResultArg.Success)
                    {
                        sOption = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sOption = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            return sOption;
        }
        public string DeleteTimeTableSetting(string sSettingId)
        {
            string sOption = string.Empty;
            TimeTableSetting objModel = new TimeTableSetting();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            objModel.SETTING_ID = sSettingId;
            ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.DeleteTimeTableSettings), sAcademicYear);
            if (ResultArg.Success)
            {
                sOption = Common.Messages.RecordDeletedSuccessfully;
            }
            else
            {
                sOption = Common.Messages.FailedToDeletedTryAgain;
            }
            return sOption;
        }
        #endregion
        #region  Department Wise TimeTable Template
        public async Task<ActionResult> TimeTableDepartmentWiseTemplateList()
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            //var TimeTableSetting = new TT_DEPARTMENT_WISE_TEMPLATE();
            var ListDeparmentWiseTemplate = await Task.Run(() => (List<TT_DEPARTMENT_WISE_TEMPLATE>)CMSHandler.FetchData<TT_DEPARTMENT_WISE_TEMPLATE>(null, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchDepartmentWiseTimetableTemplate), sAcademicYear).DataSource.Data);
            objViewModel.lstTimeTableDepartmentWiseTemplate = ListDeparmentWiseTemplate;
            return View(objViewModel);
        }
        public string DeleteDepartmentWiseTimeTableTemplate(string sDepatmentWiseTemplateId)
        {
            string sOption = string.Empty;
            TT_DEPARTMENT_WISE_TEMPLATE objModel = new TT_DEPARTMENT_WISE_TEMPLATE();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            objModel.DEPARTMENT_TEMPLATE_ID = sDepatmentWiseTemplateId;
            ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.DeleteDepartmentWiseTimeTableTemplate), sAcademicYear);
            if (ResultArg.Success)
            {
                sOption = Common.Messages.RecordDeletedSuccessfully;
            }
            else
            {
                sOption = Common.Messages.FailedToDeletedTryAgain;
            }
            return sOption;
        }
        public async Task<ActionResult> DepartmentWiseTimeTableTemplate()
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            var Setting = new List<TimeTableSetting>();
            var Department = new List<SupDepartment>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Setting = await Task.Run(() => (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchTimeTableSetting)).DataSource.Data);
            Department = await Task.Run(() => (List<SupDepartment>)CMSHandler.FetchData<SupDepartment>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDepartment), sAcademicYear).DataSource.Data);
            objViewModel.SettingList = new SelectList(Setting, Common.TT_TEMPLATE_SETTING.SETTING_ID, Common.TT_TEMPLATE_SETTING.SETTING_NAME);
            objViewModel.Departmentlist = new SelectList(Department, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
            return View(objViewModel);
        }
        public string SaveDepartmentWiseTimeTableTemplate(string DepartmentWiseTimeTableTemplate)
        {
            string sOption = string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            if (DepartmentWiseTimeTableTemplate != null)
            {
                var Result = JsonConvert.DeserializeObject<TT_DEPARTMENT_WISE_TEMPLATE>(DepartmentWiseTimeTableTemplate);
                var FetchExistingDepartmentWiseTimeTableTemplate = (List<TT_DEPARTMENT_WISE_TEMPLATE>)CMSHandler.FetchData<TT_DEPARTMENT_WISE_TEMPLATE>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.IsDepartmentWiseTimeTableTemplateExisting), sAcademicYear).DataSource.Data;
                if (FetchExistingDepartmentWiseTimeTableTemplate != null)
                {
                    sOption = "This Department Template Is Existing Already...!";
                }
                else
                {
                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveDepartmentWiseTimeTableTemplate), sAcademicYear);
                    if (ResultArg.Success)
                    {
                        sOption = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sOption = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            return sOption;
        }
        public async Task<ActionResult> EditDepartmentWiseTimeTableTemplate(string id)
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            Session[Common.SESSION_VARIABLES.DEPARTMENT_TEMPLATE_ID] = id;
            var Setting = new List<TimeTableSetting>();
            var Department = new List<SupDepartment>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Setting = await Task.Run(() => (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchTimeTableSetting)).DataSource.Data);
            Department = await Task.Run(() => (List<SupDepartment>)CMSHandler.FetchData<SupDepartment>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDepartment), sAcademicYear).DataSource.Data);
            objViewModel.SettingList = new SelectList(Setting, Common.TT_TEMPLATE_SETTING.SETTING_ID, Common.TT_TEMPLATE_SETTING.SETTING_NAME);
            objViewModel.Departmentlist = new SelectList(Department, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
            return View(objViewModel);
        }
        public JsonResult FetchDepartmentmentWiseTimeTableTemplateById(string sDepartmentWiseTimeTableTemplateId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            TT_DEPARTMENT_WISE_TEMPLATE objModel = new TT_DEPARTMENT_WISE_TEMPLATE();
            objModel.DEPARTMENT_TEMPLATE_ID = sDepartmentWiseTimeTableTemplateId;
            var FetchTimeTableSetting = (List<TT_DEPARTMENT_WISE_TEMPLATE>)CMSHandler.FetchData<TT_DEPARTMENT_WISE_TEMPLATE>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchDepartmentWiseTimetableTemplateById), sAcademicYear).DataSource.Data;
            return Json(FetchTimeTableSetting);
        }
        public string UpdateDepartmentWiseTimeTableTemplate(string DepartmentWiseTimeTableTemplate)
        {
            string sOption = string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            if (DepartmentWiseTimeTableTemplate != null)
            {
                var Result = JsonConvert.DeserializeObject<TT_DEPARTMENT_WISE_TEMPLATE>(DepartmentWiseTimeTableTemplate);
                Result.DEPARTMENT_TEMPLATE_ID = (Session[Common.SESSION_VARIABLES.DEPARTMENT_TEMPLATE_ID] != null) ? Session[Common.SESSION_VARIABLES.DEPARTMENT_TEMPLATE_ID].ToString() : "";
                var FetchExistingDepartmentWiseTimeTableTemplate = (List<TT_DEPARTMENT_WISE_TEMPLATE>)CMSHandler.FetchData<TT_DEPARTMENT_WISE_TEMPLATE>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.IsDepartmentWiseTimeTableTemplateExisting), sAcademicYear).DataSource.Data;
                if (FetchExistingDepartmentWiseTimeTableTemplate != null)
                {
                    sOption = "This Department Template Is Existing Already...!";
                }
                else
                {
                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.UpdateDepartmentWiseTimeTableTemplate), sAcademicYear);
                    if (ResultArg.Success)
                    {
                        sOption = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sOption = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            return sOption;
        }
        #endregion
        #region TimeTable Template
        public async Task<ActionResult> TimeTableTemplateList()
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            //var TimeTableSetting = new TT_DEPARTMENT_WISE_TEMPLATE();
            var ListTimeTableTemplate = await Task.Run(() => (List<TimeTableTemplate>)CMSHandler.FetchData<TimeTableTemplate>(null, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTimeTableTemplate), sAcademicYear).DataSource.Data);
            objViewModel.lstTimeTableTemplate = ListTimeTableTemplate;
            return View(objViewModel);
        }
        public async Task<ActionResult> TimeTableTemplate()
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            var Setting = new List<TimeTableSetting>();
            var Hours = new List<SUP_HOURS>();
            var Days = new List<SUP_DAY_ORDER>();
            var GraduationType = new List<SupClassLevel>();
            var Faculty = new List<SupFaculty>();
            var PaperType = new List<SupPaperType>();
            var Year = new List<SubClassYear>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Days = await Task.Run(() => (List<SUP_DAY_ORDER>)CMSHandler.FetchData<SUP_DAY_ORDER>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDayOrders)).DataSource.Data);
            Hours = await Task.Run(() => (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHours)).DataSource.Data);
            GraduationType = await Task.Run(() => (List<SupClassLevel>)CMSHandler.FetchData<SupClassLevel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassLevel)).DataSource.Data);
            Setting = await Task.Run(() => (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchTimeTableSetting)).DataSource.Data);
            Faculty = await Task.Run(() => (List<SupFaculty>)CMSHandler.FetchData<SupFaculty>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFaculty), sAcademicYear).DataSource.Data);
            PaperType = await Task.Run(() => (List<SupPaperType>)CMSHandler.FetchData<SupPaperType>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllPaperType)).DataSource.Data);
            Year = await Task.Run(() => (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear)).DataSource.Data);
            objViewModel.SettingList = new SelectList(Setting, Common.TT_TEMPLATE_SETTING.SETTING_ID, Common.TT_TEMPLATE_SETTING.SETTING_NAME);
            objViewModel.HourList = new SelectList(Hours, Common.SUP_HOURS.HOUR_ID, Common.SUP_HOURS.HOUR);
            objViewModel.DayOrderList = new SelectList(Days, Common.SUP_DAY_ORDERS.DAY_ORDER_ID, Common.SUP_DAY_ORDERS.DAY);
            objViewModel.GraduationType = new SelectList(GraduationType, Common.SUP_CLASS_LEVEL.CLASSLEVELID, Common.SUP_CLASS_LEVEL.CLASSLEVEL);
            objViewModel.FacultyLIst = new SelectList(Faculty, Common.CP_FACULTY_2017.FACULTY_ID, Common.CP_FACULTY_2017.FACULTY);
            objViewModel.PaperTypeList = new SelectList(PaperType, Common.SUP_PAPER_TYPE.PAPER_TYPE_ID, Common.SUP_PAPER_TYPE.PAPER_TYPE);
            objViewModel.ClassYearList = new SelectList(Year, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.SUP_CLASS_YEAR.CLASS_YEAR);
            return View(objViewModel);
        }
        public string SaveTimeTableTemplate(string TimeTableTemplate)
        {
            string sOption = string.Empty;
            if (TimeTableTemplate != null)
            {
                var Result = JsonConvert.DeserializeObject<TimeTableTemplate>(TimeTableTemplate);
                var FecthTimeTableTemplate = (List<TimeTableTemplate>)CMSHandler.FetchData<TimeTableTemplate>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.IsTimeTableTemplateExisting)).DataSource.Data;
                if (FecthTimeTableTemplate != null)
                {
                    sOption = "This TimeTable Template Is Existing Already...!";
                }
                else
                {
                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTableTemplate));
                    if (ResultArg.Success)
                    {
                        sOption = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sOption = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            return sOption;
        }
        public string DeleteTimeTableTemplate(string sTemplateId)
        {
            string sOption = string.Empty;
            TimeTableTemplate objModel = new TimeTableTemplate();
            objModel.TT_Temp_Id = sTemplateId;
            ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.DeleteTimeTableTemplate));
            if (ResultArg.Success)
            {
                sOption = Common.Messages.RecordDeletedSuccessfully;
            }
            else
            {
                sOption = Common.Messages.FailedToDeletedTryAgain;
            }
            return sOption;
        }
        public async Task<ActionResult> EditTimeTableTemplate(string id)
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            Session[Common.SESSION_VARIABLES.TT_TEMP_ID] = id;
            var Setting = new List<TimeTableSetting>();
            var Hours = new List<SUP_HOURS>();
            var Days = new List<SUP_DAY_ORDER>();
            var GraduationType = new List<SupClassLevel>();
            var Faculty = new List<SupFaculty>();
            var PaperType = new List<SupPaperType>();
            var Year = new List<SubClassYear>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Days = await Task.Run(() => (List<SUP_DAY_ORDER>)CMSHandler.FetchData<SUP_DAY_ORDER>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDayOrders)).DataSource.Data);
            Hours = await Task.Run(() => (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchHours)).DataSource.Data);
            GraduationType = await Task.Run(() => (List<SupClassLevel>)CMSHandler.FetchData<SupClassLevel>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassLevel)).DataSource.Data);
            Setting = await Task.Run(() => (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchTimeTableSetting)).DataSource.Data);
            Faculty = await Task.Run(() => (List<SupFaculty>)CMSHandler.FetchData<SupFaculty>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchFaculty), sAcademicYear).DataSource.Data);
            PaperType = await Task.Run(() => (List<SupPaperType>)CMSHandler.FetchData<SupPaperType>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAllPaperType)).DataSource.Data);
            Year = await Task.Run(() => (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear)).DataSource.Data);
            objViewModel.SettingList = new SelectList(Setting, Common.TT_TEMPLATE_SETTING.SETTING_ID, Common.TT_TEMPLATE_SETTING.SETTING_NAME);
            objViewModel.HourList = new SelectList(Hours, Common.SUP_HOURS.HOUR_ID, Common.SUP_HOURS.HOUR);
            objViewModel.DayOrderList = new SelectList(Days, Common.SUP_DAY_ORDERS.DAY_ORDER_ID, Common.SUP_DAY_ORDERS.DAY);
            objViewModel.GraduationType = new SelectList(GraduationType, Common.SUP_CLASS_LEVEL.CLASSLEVELID, Common.SUP_CLASS_LEVEL.CLASSLEVEL);
            objViewModel.FacultyLIst = new SelectList(Faculty, Common.CP_FACULTY_2017.FACULTY_ID, Common.CP_FACULTY_2017.FACULTY);
            objViewModel.PaperTypeList = new SelectList(PaperType, Common.SUP_PAPER_TYPE.PAPER_TYPE_ID, Common.SUP_PAPER_TYPE.PAPER_TYPE);
            objViewModel.ClassYearList = new SelectList(Year, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.SUP_CLASS_YEAR.CLASS_YEAR);
            return View(objViewModel);
        }
        public JsonResult FetchTimeTableTemplateById(string sTemplateId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            TimeTableTemplate objModel = new TimeTableTemplate();
            objModel.TT_Temp_Id = sTemplateId;
            var FetchTimeTableTemplate = (List<TimeTableTemplate>)CMSHandler.FetchData<TimeTableTemplate>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTimeTableTemplateById), sAcademicYear).DataSource.Data;
            return Json(FetchTimeTableTemplate);
        }
        public string UpdateTimeTableTemplate(string TimeTableTemplate)
        {
            string sOption = string.Empty;
            if (TimeTableTemplate != null)
            {
                var Result = JsonConvert.DeserializeObject<TimeTableTemplate>(TimeTableTemplate);
                Result.TT_Temp_Id = Session[Common.SESSION_VARIABLES.TT_TEMP_ID].ToString();
                var FecthTimeTableTemplate = (List<TimeTableTemplate>)CMSHandler.FetchData<TimeTableTemplate>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.IsTimeTableTemplateExisting)).DataSource.Data;
                if (FecthTimeTableTemplate != null)
                {
                    sOption = "This TimeTable Template Is Existing Already...!";
                }
                else
                {
                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.UpdateTimeTableTemplate));
                    if (ResultArg.Success)
                    {
                        sOption = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sOption = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            return sOption;
        }
        #endregion
        #region TimeTable Registration For Staff
        public ActionResult TimeTableEntryForStaff()
        {
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
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
                    objHandler.WriteError("TimeTableEntryForStaff", "TimeTable", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TimeTableEntryForStaff", "TimeTable", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public ActionResult BindTimeTableForStaffByClassId(string sClassId)
        {
            TimeTableViewModel objViewModel = new TimeTableViewModel();
            stf_Personal_Info objStaffModel = new stf_Personal_Info();
            TimeTableSetting objSettingModel = new TimeTableSetting();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    objStaffModel.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    objStaffModel.CLASS_ID = sClassId;
                    var FetchNoOfDaysAndHoursByClassId = (List<TimeTableSetting>)CMSHandler.FetchData<TimeTableSetting>(objStaffModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTotalHoursAndDaysByClassId), sAcademicYear).DataSource.Data;
                    if (FetchNoOfDaysAndHoursByClassId != null && FetchNoOfDaysAndHoursByClassId.Count > 0)
                        objSettingModel.NO_OF_HOURS = FetchNoOfDaysAndHoursByClassId.FirstOrDefault().NO_OF_HOURS;
                    string sSql = TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchHoursByNoOfHours).Replace(Common.Delimiter.QUS + Common.TT_TEMPLATE_SETTING.NO_OF_HOURS, objSettingModel.NO_OF_HOURS);
                    objViewModel.lstHours = (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(null, sSql, sAcademicYear).DataSource.Data;
                    //objViewModel.lstHours = (List<SUP_HOURS>)CMSHandler.FetchData<SUP_HOURS>(objSettingModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchHoursByNoOfHours), sAcademicYear).DataSource.Data;
                    objViewModel.lstDayOrders = (List<SUP_DAY_ORDER>)CMSHandler.FetchData<SUP_DAY_ORDER>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDayOrders)).DataSource.Data;
                    // Fetch Regular Hours
                    objViewModel.lstTimeTable = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objStaffModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTimeTableInfoForStaffByClassId), sAcademicYear).DataSource.Data;
                    if (objViewModel.lstTimeTable == null)
                        objViewModel.lstTimeTable = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objStaffModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTimeTableInfoForStaffByClassIdOnlyOptionalPapers), sAcademicYear).DataSource.Data;
                }
                else
                    return RedirectToAction("ErrorMessage", "error");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindTimeTableForStaff", "TimeTable", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindTimeTableForStaff", "TimeTable", ex.Message);
                }
            }
            return View(objViewModel);
        }
        public JsonResult EditTimeTableForStaff(string sClassId, string sHourId, string sDayId, string sHourType, string sSemesterId, string sCourseId, string sStaffId)
        {
            string sOption = string.Empty;
            JsonResultData ObjJsonData = new JsonResultData();
            TimeTable objModel = new TimeTable();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                objModel.Class_Id = sClassId;
                objModel.Hour_Id = sHourId;
                objModel.hourType = sHourType;
                objModel.Day_Order_Id = sDayId;
                objModel.Course_Id = sCourseId;
                objModel.Staff_Id = sStaffId;
                objModel.Semester_Id = sSemesterId;
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                var CheckIsRegularType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.CheckIsRegularPaperByStaffId), sAcademicYear).DataSource.Data;
                if (CheckIsRegularType != null && CheckIsRegularType.Count > 0)
                {
                    objModel.Time_Table_Id = CheckIsRegularType.SingleOrDefault().Time_Table_Id;
                    var FetchRegularPaperTimeTableById = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchTimeTableById), sAcademicYear).DataSource.Data;

                    //objModel.Time_Table_Id = FetchRegularPaperTimeTableById.FirstOrDefault().Time_Table_Id;
                    objModel.CLASS_CODE = FetchRegularPaperTimeTableById.FirstOrDefault().CLASS_CODE;
                    //objModel.Course_Id = FetchRegularPaperTimeTableById.FirstOrDefault().Course_Id;
                    //objModel.Staff_Id = FetchRegularPaperTimeTableById.FirstOrDefault().Staff_Id;
                }
            }
            else
            {
                ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTimeTable(string sTimeTableId)
        {
            JsonResultData ObjJsonData = new JsonResultData();
            TimeTable objModel = new TimeTable();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR]).ToString() : string.Empty;
                try
                {
                    objModel.Time_Table_Id = sTimeTableId;
                    var DeleteTimeTable = CMSHandler.SaveOrUpdate(objModel, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.DeleteTimeTable), sAcademicYear);
                    if (DeleteTimeTable.Success)
                    {
                        ObjJsonData.Message = Common.Messages.RecordDeletedSuccessfully;
                        ObjJsonData.ErrorCode = Common.ErrorCode.Accepted;
                    }
                    else
                    {
                        ObjJsonData.Message = Common.Messages.FailedToDeletedTryAgain;
                        ObjJsonData.ErrorCode = Common.ErrorCode.ExpectationFailed;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("TimeTableController", "DeleteTimeTable", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("TimeTableController", "DeleteTimeTable", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                }
            }
            else
            {
                ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveTimeTableStaffEntry(string TimeTableInfo)
        {
            JsonResultData ObjJsonData = new JsonResultData();
            string sOption = string.Empty;
            if (TimeTableInfo != null)
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    try
                    {
                        var Result = JsonConvert.DeserializeObject<TimeTable>(TimeTableInfo);
                        string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                        if (Result.Course_Id != "0")
                        {
                            var CheckIsOptionalPaper = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOptionalPaperByCourseId), sAcademicYear).DataSource.Data;
                            var FetchAllotedStaff = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.CheckIsStaffAlloted), sAcademicYear).DataSource.Data;
                            if (FetchAllotedStaff != null && FetchAllotedStaff.Count > 1)
                            {
                                ObjJsonData.Message = "There Are Duplicate Data, Please Meet Admin...!!!";
                                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                            }
                            if (CheckIsOptionalPaper != null && CheckIsOptionalPaper.Count > 0)
                            {

                                if (FetchAllotedStaff != null && FetchAllotedStaff.Count > 0)
                                {
                                    foreach (var item in FetchAllotedStaff)
                                    {

                                        if (CheckIsOptionalPaper.FirstOrDefault().Course_Id == item.Course_Id)
                                        {
                                            ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                            if (ResultArgs.Success)
                                            {
                                                ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                                            }
                                            else
                                            {
                                                ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                                            }
                                        }
                                        else
                                        {
                                            ObjJsonData.Message = "This Staff Already Alloted To Another Class...!!!";
                                            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                }
                                else
                                {
                                    var FetchIsStaticHourAndPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchIsStaticAndPaperTypeByClassId), sAcademicYear).DataSource.Data;
                                    if (FetchIsStaticHourAndPaperType != null && FetchIsStaticHourAndPaperType.Count > 0)
                                    {
                                        if (FetchIsStaticHourAndPaperType.FirstOrDefault().IS_STATIC == Common.IsActiveFalg.IsNotActive)
                                        {
                                            ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                            if (ResultArgs.Success)
                                            {
                                                ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                                            }
                                            else
                                            {
                                                ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                                            }
                                        }
                                        else
                                        {
                                            var FetchPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchPaperType), sAcademicYear).DataSource.Data;
                                            var FetchHourType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchHourType), sAcademicYear).DataSource.Data;
                                            if (FetchPaperType != null && FetchPaperType.Count > 0 && FetchHourType != null && FetchHourType.Count > 0)
                                            {
                                                Result.PAPER_TYPE = FetchPaperType.SingleOrDefault().PAPER_TYPE;
                                                Result.Paper_Type_Id = FetchHourType.SingleOrDefault().Paper_Type_Id;
                                                if (Result.PAPER_TYPE == Result.Paper_Type_Id)
                                                {
                                                    ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                                    if (ResultArgs.Success)
                                                    {
                                                        ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                                                    }
                                                    else
                                                    {
                                                        ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                                                    }
                                                }
                                                else
                                                {
                                                    ObjJsonData.Message = "This Course is not eligible for this hour...!!!";
                                                }
                                            }
                                            else
                                            {
                                                ObjJsonData.Message = "No paper type and hour type for this course...!!!";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ObjJsonData.Message = "Please check the template and No paper type and hour type for this course...!!!";
                                    }
                                }
                            }
                            else
                            {
                                if (FetchAllotedStaff != null && FetchAllotedStaff.Count > 0)
                                {
                                    ObjJsonData.Message = "This Staff Already Alloted To Another Class...!!!";
                                }
                                else
                                {
                                    var FetchIsStaticHourAndPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchIsStaticAndPaperTypeByClassId), sAcademicYear).DataSource.Data;
                                    if (FetchIsStaticHourAndPaperType != null && FetchIsStaticHourAndPaperType.Count > 0)
                                    {
                                        if (FetchIsStaticHourAndPaperType.FirstOrDefault().IS_STATIC == Common.IsActiveFalg.IsNotActive)
                                        {
                                            ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                            if (ResultArgs.Success)
                                            {
                                                ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                                            }
                                            else
                                            {
                                                ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                                            }
                                        }
                                        else
                                        {
                                            var FetchPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchPaperType), sAcademicYear).DataSource.Data;
                                            var FetchHourType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchHourType), sAcademicYear).DataSource.Data;
                                            if (FetchPaperType != null && FetchPaperType.Count > 0 && FetchHourType != null && FetchHourType.Count > 0)
                                            {
                                                Result.PAPER_TYPE = FetchPaperType.SingleOrDefault().PAPER_TYPE;
                                                Result.Paper_Type_Id = FetchHourType.SingleOrDefault().Paper_Type_Id;
                                                if (Result.PAPER_TYPE == Result.Paper_Type_Id)
                                                {
                                                    ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                                    if (ResultArgs.Success)
                                                    {
                                                        ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                                                    }
                                                    else
                                                    {
                                                        ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                                                    }
                                                }
                                                else
                                                {
                                                    ObjJsonData.Message = "This Course is not eligible for this hour...!!!";
                                                }
                                            }
                                            else
                                            {
                                                ObjJsonData.Message = "No paper type and hour type for this course...!!!";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ObjJsonData.Message = "Please check the template and No paper type and hour type for this course...!!!";
                                    }
                                }
                            }
                        }
                        else
                        {
                            var FetchIsStaticHourAndPaperType = (List<TimeTable>)CMSHandler.FetchData<TimeTable>(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.FetchIsStaticAndPaperTypeByClassId), sAcademicYear).DataSource.Data;
                            if (FetchIsStaticHourAndPaperType.FirstOrDefault().IS_STATIC == "0")
                            {
                                ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(Result, TimeTableSQL.GetTimeTableSQL(TimeTableSQLCommands.SaveTimeTable), sAcademicYear);
                                if (ResultArgs.Success)
                                {
                                    ObjJsonData.Message = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    ObjJsonData.Message = Common.Messages.FailedToSaveRecords;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("TimeTableController", "SaveTimeTableStaffEntry", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("TimeTableController", "SaveTimeTableStaffEntry", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    }
                }
            }
            else
            {
                ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}