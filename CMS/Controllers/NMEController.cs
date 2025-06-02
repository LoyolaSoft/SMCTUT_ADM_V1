using CMS.DAO;
using CMS.Models;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class NMEController : Controller
    {
        // GET: NME
        [UserRights("STUDENT")]
        public async Task<ActionResult> NMERegistration()
        {
            return View();
        }
        public async Task<string> CheckNMEAvailability(string sClassId, string sCourseId, string sSettingId)
        {
            string sResult = string.Empty;
            try
            {
                DataValue dv = new DataValue();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                ResultArgs resultArgs = new ResultArgs();
                dv.Clear();
                dv.Add(Common.NME_STUDENT_REGISTRATION_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
                dv.Add(Common.NME_STUDENT_REGISTRATION_2017.COURSE_ID, sCourseId, EnumCommand.DataType.String);
                dv.Add(Common.NME_COURSE_REGISTRATION_2017.SETTINGS_ID, sSettingId, EnumCommand.DataType.String);
                NMEViewModel objNME = new NMEViewModel();
                resultArgs = objNME.CheckNMEAvailability(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "");
                sResult = resultArgs.StringResult;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("CheckNMEAvailability", "NMEController", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("CheckNMEAvailability", "NMEController", ex.Message);
                }
            }
            return sResult;
        }

        public async Task<string> RegisterNMECourse(string sClassId, string sCourseId, string sSettingId, string semester, string sSClassId)
        {
            string sResult = string.Empty;
            string sCheckCourse = string.Empty;
            DataValue dv = new DataValue();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            ResultArgs resultArgs = new ResultArgs();
            try
            {
                dv.Clear();
                dv.Add(Common.NME_STUDENT_REGISTRATION_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
                dv.Add(Common.NME_STUDENT_REGISTRATION_2017.COURSE_ID, sCourseId, EnumCommand.DataType.String);
                sCheckCourse = await Task.Run(() => CheckNMEAvailability(sClassId, sCourseId, sSettingId));
                dv.Add(Common.NME_SETTINGS.SETTINGS_ID, sSettingId, EnumCommand.DataType.String);
                dv.Add(Common.NME_STUDENT_REGISTRATION_2017.STUDENT_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                dv.Add(Common.NME_STUDENT_REGISTRATION_2017.SEMESTER, semester, EnumCommand.DataType.String);
                dv.Add(Common.NME_STUDENT_REGISTRATION_2017.S_CLASS_ID, sSClassId, EnumCommand.DataType.String);
                NMEViewModel objNME = new NMEViewModel();

                if (sCheckCourse.Equals("1"))
                {
                    resultArgs = await Task.Run(() => objNME.RegisterNMECourse(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : ""));
                    if (resultArgs.Success)
                    {
                        resultArgs.StringResult = "1";
                    }
                    else
                    {
                        resultArgs.StringResult = "0";
                    }
                }
                else
                {
                    resultArgs.Success = false;
                    resultArgs.StringResult = "3";
                }
            }
            catch (Exception ex)
            {

                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("RegisterNMECourse", "NMEController", "Err MSg : " + ex.Message, "Query is empty!");
                    objHandler.WriteError("RegisterNMECourse", "NMEController", ex.Message);
                }
            }
            sResult = resultArgs.StringResult;
            return sResult;
        }

        public async Task<string> LoadNMECourses()
        {
            ResultArgs resultArgs = new ResultArgs();
            string sResult = string.Empty;
            try
            {
                sResult = await ValidateStudent();
                if (sResult != "1")
                {
                    if (Session[Common.SESSION_VARIABLES.CLASS_ID] != null)
                    {
                        DataValue dv = new DataValue();
                        DataTable dt = new DataTable();
                        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        string sClassId = Session[Common.SESSION_VARIABLES.CLASS_ID].ToString();
                        string sShiftId = Session[Common.SESSION_VARIABLES.SHIFT_ID].ToString();
                        dv.Clear();
                        dv.Add(Common.NME_SETTINGS.CLASS_ID, sClassId, EnumCommand.DataType.String);
                        dv.Add(Common.STU_PERSONAL_INFO.SHIFT_ID, sShiftId, EnumCommand.DataType.String);
                        NMEViewModel objNME = new NMEViewModel();
                        resultArgs = await Task.Run(() => objNME.LoadNMECourses(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : ""));
                        if (resultArgs.Success)
                        {
                            dt = resultArgs.DataSource.Table;
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                sResult = string.Empty;
                                // CLASS_ID, NC.COURSE_ID, NC.SETTINGS_ID, CR.COURSE_TITLE
                                foreach (DataRow item in dt.Rows)
                                {
                                    sResult += "<div class='form-group'> <input type='radio' cms-semester='" + item[Common.CP_COURSE_ROOT_2017.SEMESTER_ID].ToString() + "' cms-classid='" + item[Common.NME_CLASS_COURSE_2017.CLASS_ID].ToString() + "' cms-nmeclassid='" + item["NME_CLASS"].ToString() + "' cms-courseid='" + item[Common.NME_CLASS_COURSE_2017.COURSE_ID].ToString() + "'  name='NMECourse' id='" + item[Common.NME_CLASS_COURSE_2017.COURSE_ID].ToString() + "' value='" + item[Common.NME_CLASS_COURSE_QUOTA_2017.SETTINGS_ID] + "' > <label for='" + item[Common.NME_CLASS_COURSE_2017.COURSE_ID].ToString() + "'> " + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString() + "</lable></div>";
                                }
                            }
                            else
                            {
                                sResult = string.Empty;
                            }

                        }
                    }
                }
                else
                {
                    sResult = "2";
                }

            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("LoadNMECourses", "NMEController", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("LoadNMECourses", "NMEController", ex.Message);
                }


            }
            return sResult;
        }

        public async Task<string> ValidateStudent()
        {
            string sResult = string.Empty;

            string sCheckCourse = string.Empty;
            DataValue dv = new DataValue();
            ResultArgs resultArgs = new ResultArgs();
            try
            {
                dv.Clear();
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                dv.Add(Common.NME_STUDENT_REGISTRATION_2017.STUDENT_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                NMEViewModel objNME = new NMEViewModel();
                resultArgs = await Task.Run(() => objNME.ValidateStudents(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : ""));
                sResult = resultArgs.StringResult;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ValidateStudent", "NMEController", "Err MSg : " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ValidateStudent", "NMEController", ex.Message);
                }
            }
            return sResult;

        }

        public async Task<ActionResult> NMEStudentList()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            try
            {
                DataTable dtNMEclassList = new DataTable();
                List<NMEClassList> iClassList = new List<NMEClassList>();
                dtNMEclassList = await Task.Run(() => objViewModel.FetchNMEClassList((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);
                iClassList = await Task.Run(() => (from DataRow dr in dtNMEclassList.Rows select new NMEClassList() { CLASS_ID = dr["CLASS_ID"].ToString(), CLASS_NAME = dr["CLASS_NAME"].ToString() }).ToList());
                objViewModel.NMEClassList = new SelectList(iClassList, "CLASS_ID", "CLASS_NAME");
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("ValidateStudent", "NMEController", "Err MSg : " + ex.Message, "Query is empty!");
                    objHandler.WriteError("ValidateStudent", "NMEController", ex.Message);
                }

            }
            return View(objViewModel);
        }

        public async Task<ActionResult> BindNMEStudentList(string sClassID)
        {
            DataValue dv = new DataValue();
            NMEViewModel objViewModel = new NMEViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            try
            {
                DataTable dtStudentList = new DataTable();
                List<NMEClassWiseStudent> iClassList = new List<NMEClassWiseStudent>();
                dv.Add(Common.CP_CLASSES_2017.CLASS_ID, sClassID, EnumCommand.DataType.String);
                dtStudentList = await Task.Run(() => objViewModel.FetchNMEStudentsByClassId(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table);
                iClassList = await Task.Run(() => (from DataRow dr in dtStudentList.Rows
                                                   select new NMEClassWiseStudent()
                                                   {
                                                       ADMISSION_NO = dr[Common.STU_PERSONAL_INFO.ADMISSION_NO].ToString(),
                                                       CLASS_NAME = dr["CLASS_NAME"].ToString(),
                                                       COURSE_TITLE = dr[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString(),
                                                       FIRST_NAME = dr[Common.STU_PERSONAL_INFO.FIRST_NAME].ToString(),
                                                       REGISTER_NO = dr[Common.STU_PERSONAL_INFO.REGISTER_NO].ToString()
                                                   }).ToList());
                objViewModel.NMEClassWiseStudents = iClassList;
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("BindNMEStudentList", "NMEController", "Err MSg : " + ex.Message, "Query is empty!");
                    objHandler.WriteError("BindNMEStudentList", "NMEController", ex.Message);
                }

            }
            return View(objViewModel);
        }
        #region NME Settings
        public async Task<ActionResult> NMESettingsList()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var NMESettings = new NMESettings();
            var ListNMESettings = await Task.Run(() => (List<NMESettings>)CMSHandler.FetchData<NMESettings>(null, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMESettingsList), sAcademicYear).DataSource.Data);
            objViewModel.lstNMESettings = ListNMESettings;
            return View(objViewModel);
        }
        public async Task<ActionResult> NMESettings()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            var Class = new List<cp_Classes_2017>();
            var Semester = new List<SupSemester>();
            var Shift = new List<Sup_Shift>();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            Class = await Task.Run(() => (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.Fetchclass), sAcademicYear).DataSource.Data);
            Semester = await Task.Run(() => (List<SupSemester>)CMSHandler.FetchData<SupSemester>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemester)).DataSource.Data);
            Shift = await Task.Run(() => (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data);
            objViewModel.ClassList = new SelectList(Class, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            objViewModel.SemesterList = new SelectList(Semester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
            objViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            return View(objViewModel);
        }
        public string GetClassIdByShiftId(string sShiftId)
        {
            string sOption = string.Empty;
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            NMEViewModel objViewModel = new NMEViewModel();
            SaveNMESettings objModel = new SaveNMESettings();
            objModel.SHIFT = sShiftId;
            var Class = new List<cp_Classes_2017>();
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByShiftId), sAcademicYear).DataSource.Data;
            if (Class != null && Class.Count > 0)
            {
                foreach (var List in Class)
                    sOption += "<option value='" + List.CLASS_ID + "' >" + List.CLASS_NAME + "</option>";
            }
            return sOption;
        }
        public string SaveNMESetting(string NMESettings)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var sResult = string.Empty;
            if (NMESettings != null)
            {
                var Result = JsonConvert.DeserializeObject<SaveNMESettings>(NMESettings);
                Result.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(Result.DATE_FROM);
                Result.DATE_TO = CommonMethods.ConvertdatetoyearFromat(Result.DATE_TO);
                Result.IS_ALLOWED = "1";
                var FetchExistingSetting = (List<NMESettings>)CMSHandler.FetchData<NMESettings>(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.IsNMESettingExisting), sAcademicYear).DataSource.Data;
                if (FetchExistingSetting != null)
                {
                    sResult = "This Setting Is Existing Already...!";
                }
                else
                {
                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.SaveNMESettings), sAcademicYear);
                    if (ResultArg.Success)
                    {
                        sResult = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sResult = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            return sResult;
        }
        public async Task<ActionResult> EditNMESettings(int id)
        {
            NMEViewModel objViewModel = new NMEViewModel();
            Session[Common.SESSION_VARIABLES.SETTINGS_ID] = id;
            var Class = new List<cp_Classes_2017>();
            var Semester = new List<SupSemester>();
            var Shift = new List<Sup_Shift>();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            Class = await Task.Run(() => (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.Fetchclass), sAcademicYear).DataSource.Data);
            Semester = await Task.Run(() => (List<SupSemester>)CMSHandler.FetchData<SupSemester>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemester)).DataSource.Data);
            Shift = await Task.Run(() => (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data);
            objViewModel.ClassList = new SelectList(Class, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            objViewModel.SemesterList = new SelectList(Semester, Common.SUP_SEMESTER.SUP_SEMESTER_ID, Common.SUP_SEMESTER.SEMESTER_NAME);
            objViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            return View(objViewModel);
        }
        public JsonResult FetchSettingsById(string sSettingsId)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            SaveNMESettings objModel = new SaveNMESettings();
            objModel.SETTINGS_ID = sSettingsId;
            var ListNMESettings = (List<SaveNMESettings>)CMSHandler.FetchData<SaveNMESettings>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMESettingsById), sAcademicYear).DataSource.Data;
            return Json(ListNMESettings);
        }
        public string UpdateRegistration(string sSettingsId)
        {
            string Result = string.Empty;
            if (sSettingsId != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                NMESettings objModel = new NMESettings();
                objModel.SETTINGS_ID = sSettingsId;
                ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.NMESettingsRegistrationClose), sAcademicYear);
                if (ResultArg.Success)
                {
                    Result = "Registration has been closed successfully";
                }
                else
                {
                    Result = "Failed to close Registration !!!";
                }
            }
            else
            {
                Result = "Failed to close Registration !!!";
            }
            return Result;
        }
        public ActionResult UpdateNMESettings(string NMESettings)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var sResult = string.Empty;
            if (NMESettings != null)
            {
                var Result = JsonConvert.DeserializeObject<SaveNMESettings>(NMESettings);
                Result.SETTINGS_ID = (Session[Common.SESSION_VARIABLES.SETTINGS_ID] != null) ? Session[Common.SESSION_VARIABLES.SETTINGS_ID].ToString() : "";
                Result.DATE_FROM = CommonMethods.ConvertdatetoyearFromat(Result.DATE_FROM);
                Result.DATE_TO = CommonMethods.ConvertdatetoyearFromat(Result.DATE_TO);
                var FetchExistingSetting = (List<NMESettings>)CMSHandler.FetchData<NMESettings>(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.IsNMESettingExisting), sAcademicYear).DataSource.Data;
                if (FetchExistingSetting != null)
                {
                    sResult = "This Setting Is Existing Already...!";
                }
                else
                {
                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.UpdateNMESettings), sAcademicYear);
                    if (ResultArg.Success)
                    {
                        sResult = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sResult = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            return Json(sResult);
        }
        public async Task<ActionResult> DeleteNMESettings(int id)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DeleteNMESettings objModel = new DeleteNMESettings();
            objModel.SETTINGS_ID = Convert.ToString(id);
            var sResult = string.Empty;
            ResultArgs ResultArg = await Task.Run(() => CMSHandler.SaveOrUpdate(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.DeleteNMESettings), sAcademicYear));
            sResult = ResultArg.Success.ToString();
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("NMESettingsList", "NME");
        }
        #endregion
        #region NME Course Registration
        public async Task<ActionResult> NMECourseRegistrationList()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var NMECourseRegistration = new NMECourseRegistration();
            var ListNMECourseRegistration = await Task.Run(() => (List<NMECourseRegistration>)CMSHandler.FetchData<NMECourseRegistration>(null, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMECourseRegistration), sAcademicYear).DataSource.Data);
            objViewModel.lstNMECourseRegistration = ListNMECourseRegistration;
            return View(objViewModel);
        }
        public string NMECourseRegistration()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            string sOption = string.Empty;
            string objJsonData = string.Empty;
            var Course = new List<cp_Course_Root_2017>();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            Course = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOnlyNMECoursesList), sAcademicYear).DataSource.Data;
            if (Course != null && Course.Count > 0)
            {
                foreach (var item in Course)
                {
                    sOption += "<option value='" + item.COURSE_ROOT_ID + "'>" + item.COURSE_TITLE + "</option>";
                }
            }
            var JsonData = new SaveNMECourseRegistration() { COURSE_ID = sOption };
            objJsonData = JsonConvert.SerializeObject(JsonData);
            return objJsonData;
        }
        public JsonResult GetClassId(string sCourseId)
        {
            string sOption = string.Empty;
            string sCourseTypeId = string.Empty;
            string sCourseType = string.Empty;
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            NMEViewModel objViewModel = new NMEViewModel();
            NMECourseRegistration objModel = new NMECourseRegistration();
            objModel.COURSE_ROOT_ID = sCourseId;
            var NMECourseType = new List<NMECourseRegistration>();
            NMECourseType = (List<NMECourseRegistration>)CMSHandler.FetchData<NMECourseRegistration>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.GetNMECourseTypeByCourseId), sAcademicYear).DataSource.Data;
            sCourseTypeId = NMECourseType[0].COURSE_TYPE_ID;
            sCourseType = NMECourseType[0].COURSE_TYPE;
            FetchNMECourseRegistration objFetchModel = new FetchNMECourseRegistration() { COURSE_TYPE_ID = sCourseTypeId, COURSE_TYPE = sCourseType };
            //    NMEClass = (List<NMEClassCourse>)CMSHandler.FetchData<NMEClassCourse>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.GetSelectedNMEClassId), sAcademicYear).DataSource.Data;
            return Json(objFetchModel, JsonRequestBehavior.AllowGet);
        }
        public string SaveNMECourseRegistration(string NMECourseRegistration)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var sResult = string.Empty;
            if (NMECourseRegistration != null)
            {
                var Result = JsonConvert.DeserializeObject<SaveNMECourseRegistration>(NMECourseRegistration);
                string[] sArray = Result.COURSE_ID.Split(',');
                for (int i = 0; i < sArray.Length; i++)
                {
                    Result.COURSE_ID = sArray[i];
                    Result.COURSE_ROOT_ID = sArray[i];
                    var FetchNMEClass = (List<NMECourseRegistration>)CMSHandler.FetchData<NMECourseRegistration>(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMEClassByCourseId), sAcademicYear).DataSource.Data;
                    if (FetchNMEClass != null && FetchNMEClass.Count > 0)
                    {
                        var FetchNMECoruseRegistration = (List<NMECourseRegistration>)CMSHandler.FetchData<NMECourseRegistration>(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.IsNMECourseRegistrationExisting), sAcademicYear).DataSource.Data;
                        if (FetchNMECoruseRegistration != null)
                        {
                            ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.UpdateNMECourseRegistrationByCourseId), sAcademicYear);
                            if (ResultArg.Success)
                            {
                                sResult = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                sResult = Common.Messages.FailedToSaveRecords;
                            }
                        }
                        else
                        {
                            var FetchCoursetype = (List<NMECourseRegistration>)CMSHandler.FetchData<NMECourseRegistration>(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchCourseTypeByCourseId), sAcademicYear).DataSource.Data;
                            foreach (var item in FetchCoursetype)
                                Result.COURSE_TYPE = item.COURSE_TYPE;
                            ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.SaveNMECourseRegistration), sAcademicYear);
                            if (ResultArg.Success)
                            {
                                sResult = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                sResult = Common.Messages.FailedToSaveRecords;
                            }
                        }
                    }
                    else
                    {
                        sResult = "Please Add This Course In Classes As Optional Class...!!!";
                    }
                }

            }
            return sResult;
        }
        public ActionResult UpdateNMECourseRegistration(string NMECourseRegistration)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var sResult = string.Empty;
            if (NMECourseRegistration != null)
            {
                var Result = JsonConvert.DeserializeObject<SaveNMECourseRegistration>(NMECourseRegistration);
                Result.REGISTRATION_ID = (Session[Common.SESSION_VARIABLES.REGISTRATION_ID] != null) ? Session[Common.SESSION_VARIABLES.REGISTRATION_ID].ToString() : "";
                var FetchNMEClass = (List<NMECourseRegistration>)CMSHandler.FetchData<NMECourseRegistration>(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMEClassByCourseId), sAcademicYear).DataSource.Data;
                if (FetchNMEClass != null && FetchNMEClass.Count > 0)
                {
                    var FetchNMECoruseRegistration = (List<NMECourseRegistration>)CMSHandler.FetchData<NMECourseRegistration>(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.IsNMECourseRegistrationExisting), sAcademicYear).DataSource.Data;
                    if (FetchNMECoruseRegistration != null)
                    {
                        sResult = "This Course Is Existing Already...!";
                    }
                    else
                    {
                        ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.UpdateNMECourseRegistration), sAcademicYear);
                        if (ResultArg.Success)
                        {
                            sResult = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                        {
                            sResult = Common.Messages.FailedToSaveRecords;
                        }
                    }
                }
                else
                {
                    sResult = "Please Add This Course In Classes As Optional Class...!!!";
                }

            }
            return Json(sResult);
        }
        public async Task<ActionResult> DeleteNMECourseRegistration(int id)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DeleteNMECourseRegistration objModel = new DeleteNMECourseRegistration();
            objModel.REGISTRATION_ID = Convert.ToString(id);
            var sResult = string.Empty;
            ResultArgs ResultArg = await Task.Run(() => CMSHandler.SaveOrUpdate(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.DeleteNMECourseRegistration), sAcademicYear));
            sResult = ResultArg.Success.ToString();
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("NMECourseRegistrationList", "NME");
        }
        public async Task<ActionResult> EditNMECoruseRegistration(int id)
        {
            NMEViewModel objViewModel = new NMEViewModel();
            var Course = new List<cp_Course_Root_2017>();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            Course = await Task.Run(() => (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOnlyNMECoursesList), sAcademicYear).DataSource.Data);
            objViewModel.NMECourseList = new SelectList(Course, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
            return View(objViewModel);
        }
        public JsonResult FetchNMECourseRegistrationById(string sRegistrationId)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            Session[Common.SESSION_VARIABLES.REGISTRATION_ID] = sRegistrationId;
            SaveNMECourseRegistration objModel = new SaveNMECourseRegistration();
            objModel.REGISTRATION_ID = sRegistrationId;
            var ListNMECourseRegistration = (List<SaveNMECourseRegistration>)CMSHandler.FetchData<SaveNMECourseRegistration>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMECourseRegistrationById), sAcademicYear).DataSource.Data;
            foreach (var item in ListNMECourseRegistration)
            {
                objModel.COURSE_ID = item.COURSE_ID;
                objModel.COURSE_TYPE = item.COURSE_TYPE;
                objModel.COURSE_TYPE_ID = item.COURSE_TYPE_ID;
            }
            return Json(objModel);
        }
        #endregion
        #region NME Class Course
        public async Task<ActionResult> NMEClassCourseList()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var NMEClassCourse = new NMEClassCourse();
            var ListClassCourse = await Task.Run(() => (List<NMEClassCourse>)CMSHandler.FetchData<NMEClassCourse>(null, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMEClassCourseList), sAcademicYear).DataSource.Data);
            objViewModel.lstNMEClassCourse = ListClassCourse;
            return View(objViewModel);
        }
        public ActionResult NMEClassCourse()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            var NMESetting = new List<NME_Setting>();
            var Course = new List<cp_Course_Root_2017>();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            NMESetting = (List<NME_Setting>)CMSHandler.FetchData<NME_Setting>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchNMESetting)).DataSource.Data;
            Course = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOnlyNMECoursesList), sAcademicYear).DataSource.Data;
            objViewModel.NMESettingList = new SelectList(NMESetting, Common.NME_SETTINGS.SETTINGS_ID, Common.NME_SETTINGS.SETTINGS_NAME);
            objViewModel.NMECourseList = new SelectList(Course, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
            return View(objViewModel);
        }
        public ActionResult GetNMEClassCourseWithQuota(string sSettingId, string SClassId, string SCourseId)
        {
            NMEViewModel objViewModel = new NMEViewModel();
            BindNMEClassCourseQuota objBindClassCourseQuotaModel = new BindNMEClassCourseQuota();
            DeleteNMEClassCourse objDeleteBulkNMEClassCourse = new DeleteNMEClassCourse();
            objDeleteBulkNMEClassCourse.CLASS_ID = SClassId;
            DeleteNMEClassCourseQuota objDeleteBulkNMEClassCourseQuota = new DeleteNMEClassCourseQuota();
            objDeleteBulkNMEClassCourseQuota.CLASS_ID = SClassId;
            objBindClassCourseQuotaModel.CLASS_ID = SClassId;
            objBindClassCourseQuotaModel.COURSE_ROOT_ID = SCourseId;
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var NMEClassCourse = new NMEClassCourse();
            string sClassCourseQuotaSQL = SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.BindNMEClassCourseQuota);
            string sDeleteBulkNMEClassCourse = SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.DeleteBulkNMEClassCourse);
            string sDeleteBulkNMEClassCourseQuota = SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.DeleteBulkNMEClassCourseQuota);
            sClassCourseQuotaSQL = sClassCourseQuotaSQL.Replace(Common.Delimiter.QUS + Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, SCourseId);
            sDeleteBulkNMEClassCourse = sDeleteBulkNMEClassCourse.Replace(Common.Delimiter.QUS + Common.NME_CLASS_COURSE_2017.COURSE_ID, SCourseId);
            sDeleteBulkNMEClassCourseQuota = sDeleteBulkNMEClassCourseQuota.Replace(Common.Delimiter.QUS + Common.NME_CLASS_COURSE_QUOTA_2017.COURSE_ID, SCourseId);
            var ListClassCourse = (List<BindNMEClassCourseQuota>)CMSHandler.FetchData<BindNMEClassCourseQuota>(objBindClassCourseQuotaModel, sClassCourseQuotaSQL, sAcademicYear).DataSource.Data;
            ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objDeleteBulkNMEClassCourse, sDeleteBulkNMEClassCourse, sAcademicYear);
            ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(objDeleteBulkNMEClassCourseQuota, sDeleteBulkNMEClassCourseQuota, sAcademicYear);
            objViewModel.lstBindNMEClassCourse = ListClassCourse;
            return View(objViewModel);
        }
        public string SaveNMEClassCourse(string sClassCourse)
        {
            string sResult = string.Empty;
            try
            {
                if (sClassCourse != null)
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JsonNMEClassCourse objJson = JsonConvert.DeserializeObject<JsonNMEClassCourse>(sClassCourse);
                    foreach (var item in objJson.NMEClassCourse)
                    {

                        //NMEViewModel objViewModel = new NMEViewModel();
                        //var NMEClassCourse = new NMEClassCourse();
                        var ListClassCourse = (List<SaveNMEClassCourse>)CMSHandler.FetchData<SaveNMEClassCourse>(item, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.IsNMEClassCourseExisting), sAcademicYear).DataSource.Data;
                        var ListClassCourseQuota = (List<SaveNMEClassCourseQuota>)CMSHandler.FetchData<SaveNMEClassCourseQuota>(item, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.IsNMEClassCourseQuotaExisting), sAcademicYear).DataSource.Data;
                        if (ListClassCourse != null)
                        {
                            foreach (var List in ListClassCourse)
                            {
                                item.NME_CLASS_COURSE_ID = List.NME_CLASS_COURSE_ID;
                                ResultArgs ResultArg = CMSHandler.SaveOrUpdate(item, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.UpdateNMEClassCourse), sAcademicYear);
                                //ResultArgs Resultarg = CMSHandler.SaveOrUpdate(item, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.UpdateNMEClassCourseQuota), sAcademicYear);
                                if (ResultArg.Success)
                                {
                                    sResult = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    sResult = Common.Messages.FailedToSaveRecords;
                                }
                            }
                            foreach (var dataitem in ListClassCourseQuota)
                            {
                                item.NME_CLASS_COURSE_QUOTA_ID = dataitem.NME_CLASS_COURSE_QUOTA_ID;
                                //ResultArgs ResultArg = CMSHandler.SaveOrUpdate(item, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.UpdateNMEClassCourse), sAcademicYear);
                                ResultArgs Resultarg = CMSHandler.SaveOrUpdate(item, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.UpdateNMEClassCourseQuota), sAcademicYear);
                                if (Resultarg.Success)
                                {
                                    sResult = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    sResult = Common.Messages.FailedToSaveRecords;
                                }
                            }
                        }
                        else
                        {
                            ResultArgs ResultArg = CMSHandler.SaveOrUpdate(item, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.SaveNMEClassCourse), sAcademicYear);
                            ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(item, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.SaveNMEClassCourseQuota), sAcademicYear);
                            if (ResultArg.Success)
                            {

                                if (ResultArgs.Success)
                                {
                                    sResult = Common.Messages.RecordsSavedSuccessfully;
                                }
                                else
                                {
                                    sResult = Common.Messages.FailedToSaveRecords;
                                }
                            }
                            else
                            {
                                sResult = Common.Messages.FailedToSaveRecords;
                            }
                        }
                    }
                }
                else
                {
                    sResult = Common.Messages.FailedToSaveRecords;
                }
            }
            catch (Exception e)
            {

            }
            return sResult;
        }
        public JsonResult GetCourseId(string sSettingId)
        {
            string sOption = string.Empty;
            string sClass = string.Empty;
            string sClassId = string.Empty;
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            NMEViewModel objViewModel = new NMEViewModel();
            NMEClassCourse objModel = new NMEClassCourse();
            objModel.SETTINGS_ID = sSettingId;
            var NMECourse = new List<NMEClassCourse>();
            var Class = new List<NMEClassCourse>();
            var Course = new List<cp_Course_Root_2017>();
            Course = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.GetRegisteredCoursesFromCourseAllotment), sAcademicYear).DataSource.Data;
            NMECourse = (List<NMEClassCourse>)CMSHandler.FetchData<NMEClassCourse>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.GetSelectedNMECourseId), sAcademicYear).DataSource.Data;
            Class = (List<NMEClassCourse>)CMSHandler.FetchData<NMEClassCourse>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.GetSelectedClassIDBySettingsId), sAcademicYear).DataSource.Data;
            if (Class != null && Class.Count > 0)
            {
                sClassId = Class[0].CLASS_ID;
                sClass = Class[0].CLASS_CODE;
            }
            //    NMEClass = (List<NMEClassCourse>)CMSHandler.FetchData<NMEClassCourse>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.GetSelectedNMEClassId), sAcademicYear).DataSource.Data;
            if (NMECourse != null && NMECourse.Count > 0)
            {
                objViewModel.SelectedNMECourseList = new SelectList(NMECourse, Common.NME_CLASS_COURSE_2017.COURSE_ID, Common.CP_COURSE_ROOT_2017.COURSE_CODE, objModel.SELECTED);
                foreach (var List in NMECourse)
                {
                    sOption += "<option value='" + List.COURSE_ID + "' " + List.SELECTED + " >" + List.COURSE_CODE + "</option>";
                    Course.Remove(Course.SingleOrDefault(o => o.COURSE_ROOT_ID == List.COURSE_ID));
                }
                sClassId = NMECourse[0].CLASS_ID;
                sClass = NMECourse[0].CLASS_CODE;
            }
            if (Course != null && Course.Count > 0)
            {
                foreach (var List in Course)
                    sOption += "<option value='" + List.COURSE_ROOT_ID + "' >" + List.COURSE_TITLE + "</option>";
            }
            FetchNMEModel objFetchModel = new FetchNMEModel() { CLASS = sClass, COURSE = sOption, CLASS_ID = sClassId };
            return Json(objFetchModel, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> DeleteNMEClassCourse(int id)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DeleteNMEClassCourse objModel = new DeleteNMEClassCourse();
            DeleteNMEClassCourseQuota objCourseAllotment = new DeleteNMEClassCourseQuota();
            NMEClassCourse objClassCourse = new NMEClassCourse();
            objModel.NME_CLASS_COURSE_ID = Convert.ToString(id);
            var sResult = string.Empty;
            ResultArgs ResultArg = await Task.Run(() => CMSHandler.SaveOrUpdate(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.DeleteNMEClassCourse), sAcademicYear));
            var FetchNMEClassCourse = (List<NMEClassCourse>)CMSHandler.FetchData<NMEClassCourse>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMEClassCourseById), sAcademicYear).DataSource.Data;
            objClassCourse.CLASS_ID = FetchNMEClassCourse.FirstOrDefault().CLASS_ID;
            objClassCourse.COURSE_ID = FetchNMEClassCourse.FirstOrDefault().COURSE_ID;
            objClassCourse.SETTINGS_ID = FetchNMEClassCourse.FirstOrDefault().SETTINGS_ID;
            var ListNMEClassCourseQuota = (List<NMEClassCourseQuota>)CMSHandler.FetchData<NMEClassCourseQuota>(objClassCourse, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchClassCourseQuotaByClassIdAndCourseIdAndSettingId), sAcademicYear).DataSource.Data;
            if (ListNMEClassCourseQuota != null)
            {
                objCourseAllotment.NME_CLASS_COURSE_QUOTA_ID = ListNMEClassCourseQuota[0].NME_CLASS_COURSE_QUOTA_ID;
                ResultArgs ResultArgs = await Task.Run(() => CMSHandler.SaveOrUpdate(objCourseAllotment, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.DeleteNMEClassCourseQuotaFromClassCourse), sAcademicYear));
                sResult = ResultArg.Success.ToString();
            }
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("NMEClassCourseList", "NME");
        }
        #endregion
        #region NME Course Allotment
        public async Task<ActionResult> NMECourseAllotmentList()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var NMECourseAllotment = new NMECourseAllotment();
            var ListNMECourseAllotment = await Task.Run(() => (List<NMECourseAllotment>)CMSHandler.FetchData<NMECourseAllotment>(null, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMECourseAllotment), sAcademicYear).DataSource.Data);
            objViewModel.lstNMECourseAllotment = ListNMECourseAllotment;
            return View(objViewModel);
        }
        public ActionResult NMECourseAllotment()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            var Shift = new List<Sup_Shift>();
            var Course = new List<cp_Course_Root_2017>();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            Shift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
            Course = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(null, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.GetRegisteredCoursesFromCourseRegistration), sAcademicYear).DataSource.Data;
            objViewModel.NMECourseList = new SelectList(Course, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
            objViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            return View(objViewModel);
        }
        public string SaveNMECourseAllotment(string NMECourseAllotment)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var sResult = string.Empty;
            if (NMECourseAllotment != null)
            {
                var Result = JsonConvert.DeserializeObject<SaveNMECourseAllotment>(NMECourseAllotment);
                var FetchExistingCourse = (List<NMECourseAllotment>)CMSHandler.FetchData<NMECourseAllotment>(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.IsNMECourseAllotmentExisting), sAcademicYear).DataSource.Data;
                if (FetchExistingCourse != null)
                {
                    sResult = "This Course Is Existing Already...!";
                }
                else
                {
                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.SaveNMECourseAllotment), sAcademicYear);
                    if (ResultArg.Success)
                    {
                        sResult = Common.Messages.RecordsSavedSuccessfully;
                    }
                    else
                    {
                        sResult = Common.Messages.FailedToSaveRecords;
                    }
                }
            }
            return sResult;
        }
        public ActionResult EditNMECourseAllotment(int id)
        {
            NMEViewModel objViewModel = new NMEViewModel();
            Session[Common.SESSION_VARIABLES.ALLOTMENT_ID] = id;
            var Shift = new List<Sup_Shift>();
            var Course = new List<cp_Course_Root_2017>();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            Shift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
            Course = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOnlyNMECoursesList), sAcademicYear).DataSource.Data;
            objViewModel.NMECourseList = new SelectList(Course, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
            objViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            return View(objViewModel);
        }
        public async Task<JsonResult> FetchNMECourseAllotmentById(string sCourseAllotmentId)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            SaveNMECourseAllotment objModel = new SaveNMECourseAllotment();
            objModel.ALLOTMENT_ID = sCourseAllotmentId;
            var ListNMECourseAllotment = await Task.Run(() => (List<SaveNMECourseAllotment>)CMSHandler.FetchData<SaveNMECourseAllotment>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMECourseAllotmentById), sAcademicYear).DataSource.Data);
            return Json(ListNMECourseAllotment);
        }
        public ActionResult UpdateNMECourseAllotment(string NMECourseAllotment)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var sResult = string.Empty;
            if (NMECourseAllotment != null)
            {
                var Result = JsonConvert.DeserializeObject<SaveNMECourseAllotment>(NMECourseAllotment);
                Result.ALLOTMENT_ID = (Session[Common.SESSION_VARIABLES.ALLOTMENT_ID] != null) ? Session[Common.SESSION_VARIABLES.ALLOTMENT_ID].ToString() : "";
                ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.UpdateNMECourseAllotment), sAcademicYear);
                if (ResultArg.Success)
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
        public async Task<ActionResult> DeleteNMECourseAllotment(int id)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DeleteNMECourseAllotment objModel = new DeleteNMECourseAllotment();
            objModel.ALLOTMENT_ID = Convert.ToString(id);
            var sResult = string.Empty;
            ResultArgs ResultArg = await Task.Run(() => CMSHandler.SaveOrUpdate(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.DeleteNMECourseAllotment), sAcademicYear));
            sResult = ResultArg.Success.ToString();
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("NMECourseAllotmentList", "NME");
        }
        #endregion
        #region NME Registered Student
        public ActionResult NMERegisteredStudentList()
        {
            NMEViewModel objViewModel = new NMEViewModel();
            var Shift = new List<Sup_Shift>();
            var Class = new List<cp_Classes_2017>();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            Shift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchOnlyOrdinaryClass), sAcademicYear).DataSource.Data;
            objViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            objViewModel.ClassList = new SelectList(Class, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_CODE);
            return View(objViewModel);
        }
        public async Task<ActionResult> BindNMERegisteredStudent(string sClassId)
        {
            NMEViewModel objViewModel = new NMEViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var NMERegisteredStudent = new NMERegisteredStudent();
            NMERegisteredStudent.CLASS_ID = sClassId;
            var ListNMERegisteredStudent = await Task.Run(() => (List<NMERegisteredStudent>)CMSHandler.FetchData<NMERegisteredStudent>(NMERegisteredStudent, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMERegisteredStudentByClassId), sAcademicYear).DataSource.Data);
            objViewModel.lstNMERegisteredStudent = ListNMERegisteredStudent;
            return View(objViewModel);
        }
        public JsonResult FetchNMERegisteredStudentById(string sNMERegiseteredStudent)
        {
            NMEViewModel objViewModel = new NMEViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            Session[Common.SESSION_VARIABLES.REGISTRATION_ID] = sNMERegiseteredStudent;
            NMERegisteredStudent objModel = new NMERegisteredStudent();
            objModel.REGISTRATION_ID = sNMERegiseteredStudent;
            var FetchNMERegisteredStudent = (List<NMERegisteredStudent>)CMSHandler.FetchData<NMERegisteredStudent>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMERegisteredStudentById), sAcademicYear).DataSource.Data;
            foreach (var item in FetchNMERegisteredStudent)
            {
                objModel.CLASS_ID = item.CLASS_ID;
                objModel.CLASS_CODE = item.CLASS_CODE;
                objModel.STUDENT_ID = item.STUDENT_ID;
                objModel.FIRST_NAME = item.FIRST_NAME;
                objModel.ROLL_NO = item.ROLL_NO;
                objModel.COURSE_ID = item.COURSE_ID;
                objModel.SUMOFQUTOA = item.SUMOFQUTOA;
                objModel.REGISTRATION_ID = item.REGISTRATION_ID;
            }
            //var Course = new List<NMERegisteredStudent>();
            //Course = (List<NMERegisteredStudent>)CMSHandler.FetchData<NMERegisteredStudent>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMECourseByClassId), sAcademicYear).DataSource.Data;
            //objViewModel.NMECourseList = new SelectList(Course, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_CODE);
            return Json(objModel);
        }
        public string GetNMECourseListByClassId(string sClassId)
        {
            string sOption = string.Empty;
            string objJsonData = string.Empty;
            NMEViewModel objViewModel = new NMEViewModel();
            NMERegisteredStudent objModel = new NMERegisteredStudent();
            objModel.CLASS_ID = sClassId;
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var Course = new List<NMERegisteredStudent>();
            Course = (List<NMERegisteredStudent>)CMSHandler.FetchData<NMERegisteredStudent>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMECourseByClassId), sAcademicYear).DataSource.Data;
            //objViewModel.NMECourseList = new SelectList(Course, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_CODE);
            if (Course != null && Course.Count > 0)
            {
                foreach (var item in Course)
                {
                    sOption += "<option value='" + item.COURSE_ID + "'>" + item.COURSE_CODE + "</option>";
                }
            }
            var JsonData = new SaveNMECourseRegistration() { COURSE_ID = sOption };
            objJsonData = JsonConvert.SerializeObject(JsonData);
            return objJsonData;
        }
        public JsonResult GetNMEClassCourseQuotaByCourseId(string sCourseId, string sClassId)
        {
            string sOption = string.Empty;
            NMEViewModel objViewModel = new NMEViewModel();
            NMERegisteredStudent objModel = new NMERegisteredStudent();
            objModel.COURSE_ID = sCourseId;
            objModel.CLASS_ID = sClassId;
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var SumOfClassCourseQuota = new List<NMERegisteredStudent>();
            SumOfClassCourseQuota = (List<NMERegisteredStudent>)CMSHandler.FetchData<NMERegisteredStudent>(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchSumOfNMEClassQuotaByClassIdAndCourseId), sAcademicYear).DataSource.Data;
            if (SumOfClassCourseQuota != null && SumOfClassCourseQuota.Count > 0)
            {
                foreach (var item in SumOfClassCourseQuota)
                {
                    objModel.SUMOFQUTOA = item.SUMOFQUTOA;
                    objModel.ALLOTED_SEATS = item.ALLOTED_SEATS;
                }
            }
            return Json(objModel);
        }
        public ActionResult UpdateNMERegisteredStudent(string NMERegisteredStudent)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var sResult = string.Empty;
            if (NMERegisteredStudent != null)
            {
                var Result = JsonConvert.DeserializeObject<NMERegisteredStudent>(NMERegisteredStudent);
                var FetchSelectedClassByStudentIdAndCourseId = (List<NMERegisteredStudent>)CMSHandler.FetchData<NMERegisteredStudent>(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchSelectedClassIdByStudentIdAndCourseId), sAcademicYear).DataSource.Data;
                if (FetchSelectedClassByStudentIdAndCourseId != null && FetchSelectedClassByStudentIdAndCourseId.Count > 0)
                {
                    foreach (var item in FetchSelectedClassByStudentIdAndCourseId)
                        Result.S_CLASS_ID = item.CLASS_ID;
                }
                ResultArgs ResultArg = CMSHandler.SaveOrUpdate(Result, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.UpdateNMERegisteredStudent), sAcademicYear);
                if (ResultArg.Success)
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
        public async Task<ActionResult> DeleteNMERegisteredStudent(int id)
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DeleteNMECourseRegistration objModel = new DeleteNMECourseRegistration();
            objModel.REGISTRATION_ID = Convert.ToString(id);
            var sResult = string.Empty;
            ResultArgs ResultArg = await Task.Run(() => CMSHandler.SaveOrUpdate(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.DeleteNMECourseRegisteredStudent), sAcademicYear));
            sResult = ResultArg.Success.ToString();
            if (sResult == "True")
            {
                TempData["DeleteSuccess"] = "Record deleted successfully ...!";
            }
            else
            {
                TempData["DeleteError"] = "Record not deleted Try again ...!";
            }
            return RedirectToAction("NMERegisteredStudentList", "NME");
        }
        public string FetchNMERegisteredStudentAndUpdateStuCourseInfo()
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            string sResult = string.Empty;
            SaveNMERegisteredStudent objModel = new SaveNMERegisteredStudent();
            var FetchNMERegisteredStudentToPush = (List<NMERegisteredStudent>)CMSHandler.FetchData<NMERegisteredStudent>(null, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMERegisteredStudentToPush), sAcademicYear).DataSource.Data;
            if (FetchNMERegisteredStudentToPush != null && FetchNMERegisteredStudentToPush.Count > 0)
            {
                var FetchNMESettingsAcademicYear = (List<NMERegisteredStudent>)CMSHandler.FetchData<NMERegisteredStudent>(null, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchNMESettingsAcademicYear), sAcademicYear).DataSource.Data;
                if (FetchNMESettingsAcademicYear != null && FetchNMESettingsAcademicYear.Count > 0)
                {
                    objModel.ACADEMIC_YEAR = FetchNMESettingsAcademicYear.FirstOrDefault().ACADEMIC_YEAR;
                    foreach (var item in FetchNMERegisteredStudentToPush)
                    {
                        objModel.CLASS_ID = item.CLASS_ID;
                        objModel.COURSE_ID = item.COURSE_ID;
                        objModel.STUDENT_ID = item.STUDENT_ID;
                        objModel.S_CLASS_ID = item.S_CLASS_ID;
                        objModel.SEMESTER = item.SEMESTER;
                        var FetchStuCourseInfo = (List<NMERegisteredStudent>)CMSHandler.FetchData<NMERegisteredStudent>(null, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.FetchStuCourseInfo), sAcademicYear).DataSource.Data;
                        if (FetchStuCourseInfo == null && FetchStuCourseInfo.Count == 0)
                        {
                            ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objModel, SQL.NME.NMESQL.GetNMESQL(NMESQLCommands.PushNMERegisteredStudentToStuCourseInfo), sAcademicYear);
                            if (ResultArg.Success)
                            {
                                sResult = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                sResult = Common.Messages.FailedToSaveRecords;
                            }
                        }
                        else
                        {
                            sResult = "Can not push again...!!!";
                        }
                    }
                }
            }

            return sResult;
        }
        #endregion
    }
}