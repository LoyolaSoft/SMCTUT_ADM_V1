using CMS.DAO;
using CMS.SQL.Academics;
using CMS.Utility;
using CMS.ViewModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;


namespace CMS.ViewModel.ViewModel
{
    public class AcademicsViewModel : IDisposable
    {
        public string sSQL = string.Empty;
        ResultArgs resultArg = new ResultArgs();
        public DataValue dv = new DataValue();
        public List<SupSemester> Semester_List { get; set; }
        public List<stf_Personal_Info> Staff_List { get; set; }
        public List<AcademicCurriculumInfo> objCourseCurriculum { get; set; }
        public List<CourseWiseStaffInfo> objCourseWiseStaff { get; set; }
        AcademicYearModel objAcademicyear = new AcademicYearModel();
        #region Academic Year
        public ResultArgs FetchAcademicYearDetails()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchAcademicYearInfo);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);

            }
            return resultArg;
        }
        public ResultArgs FetchActiveYear()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchActiveYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);

            }
            return resultArg;
        }
        public ResultArgs InsertAcademicYearInfo(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertAcademicYearInfo);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }
        public ResultArgs UpdateAcademicYearInfo(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateAcademicInfo);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs UpdateActiveAcademic(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateActiveAcademic);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs UpdateActiveYear(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateActiveYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs DeleteAcademicYearInfo(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteAcademicInfo);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs CheckCourseCode(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckCourseRootInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchAcademicYearById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchAcademicYearById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        #endregion
        #region Course Root
        public ResultArgs FetchCourseRootInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCourseRootInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs InsertCourseRootInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertCourseRootInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs DeleteCourseRoot(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteCourseRootInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs FetchCourseRootInfoById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCourseRootInfoById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs UpdateCourseRootInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateCourseRootInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region CourseWiseStaff
        public ResultArgs FetchCourseWiseStaff(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCourseWiseStaffInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs FetchSelectedCourseWiseStaff(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchSelectedCourseWiseStaff);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchSelectedCourseWiseStaffById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchSelectedCourseWiseStaffById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs BulkSaveCourseWiseStaff(JsonCourseWiseStaff objJson, string sAcademicYear)
        {
            try
            {
                string sCourseWiseStaffId = string.Empty;
                using (MySQLHandler objHandler = new MySQLHandler())
                {
                    foreach (var item in objJson.CourseWiseStaffList)
                    {
                        dv.Clear();
                        dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLASS_ID, item.CLASS_ID, EnumCommand.DataType.String);
                        dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.COURSE_ID, item.COURSE_ID, EnumCommand.DataType.String);
                        dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.STAFF_ID, item.STAFF_ID, EnumCommand.DataType.String);
                        dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.SHIFT, item.SHIFT, EnumCommand.DataType.String);
                        dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.SEMESTER_ID, item.SEMESTER_ID, EnumCommand.DataType.String);
                        sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.IsCourseWiseStaffExisting);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);

                        sCourseWiseStaffId = objHandler.ExecuteScalar(dv, sSQL).StringResult;
                        if (string.IsNullOrEmpty(sCourseWiseStaffId))
                        {
                            if (item.STAFF_ID != "null")
                            {
                                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.SaveCourseWiseStaff);
                                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
                            }
                        }
                        else
                        {
                            dv.Add(Common.CP_CLASS_COURSE_STAFF_2017.CLSCRSSTF_ID, sCourseWiseStaffId, EnumCommand.DataType.String);
                            sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateCourseWiseStaff);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                            resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }
            return resultArg;
        }
        public ResultArgs DeleteCourseWiseStaffINfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteCourseWiseStaffInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs FetchCourseWiseStaffById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCourseWiseStaffInfoById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs GetClassListByClassTypeId(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.GetClassListByClassTypeId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs GetClassTypeByShiftId(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.GetClassTypeByShiftId);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchCourseWiseStaffInfoByClassId(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCourseWiseStaffInfoByClassId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs UpdateSelectedCourseWiseStaffInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateCourseWiseStaff);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region AcademicCurriculumModel
        public List<ACADEMIC_YEAR> testFetchList()
        {
            List<ACADEMIC_YEAR> Genders = new List<ACADEMIC_YEAR>();
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                var obj = objHandler.FecthDataAsList<ACADEMIC_YEAR>("SELECT ACADEMIC_YEAR_ID, AC_YEAR, DATE_FROM, DATE_TO, ACTIVE_YEAR, ACADEMIC_NAME FROM  ACADEMIC_YEAR;", dv).DataSource.Data;
                Genders = (List<ACADEMIC_YEAR>)obj;
                //Genders =   
            }
            return Genders;
        }

        public ResultArgs CheckActiveCurriculum(string sAcademicYear)
        {

            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckActiveCurriculum);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }

        public ResultArgs FetchAcademicCurriculumInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchAcademicCurriculumInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }

        public ResultArgs FetchCurriculumInfoById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCurriculumCourseInfoById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        /// <summary>
        /// bulk insert / update for academic curriculum.
        /// </summary>
        /// <param name="objJson"></param>
        /// <param name="sAcademicYear"></param>
        /// <returns></returns>
        public ResultArgs BulkSaveCurriculum(JSonCurriculumCourse objJson, string sAcademicYear)
        {
            try
            {
                string sCurriculumId = string.Empty;
                using (MySQLHandler objHandler = new MySQLHandler())
                {
                    if (objJson != null)
                    {
                        if (objJson.CurriculumCourses[0].IS_ACTIVE != "0")
                        {
                            //Deactive process.
                            sSQL = "SET SQL_SAFE_UPDATES = 0; ";
                            sSQL += AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckActiveCurriculum);
                            sSQL += " SET SQL_SAFE_UPDATES = 1;";
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_ACADEMIC_CURRICULUM_2017.BATCH, objJson.CurriculumCourses[0].BATCH);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_ACADEMIC_CURRICULUM_2017.PROGRAMME, objJson.CurriculumCourses[0].PROGRAMME);

                            resultArg = objHandler.ExecuteAsScripts(sSQL);
                        }
                        foreach (var item in objJson.CurriculumCourses)
                        {
                            dv.Clear();
                            dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.PROGRAMME, item.PROGRAMME, EnumCommand.DataType.String);
                            dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.COURSE_ID, item.COURSE_ID, EnumCommand.DataType.String);
                            dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.BATCH, item.BATCH, EnumCommand.DataType.String);
                            dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.IS_ACTIVE, item.IS_ACTIVE, EnumCommand.DataType.String);
                            sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.IsAcademicCurriculumExisting);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);

                            sCurriculumId = objHandler.ExecuteScalar(dv, sSQL).StringResult;
                            if (string.IsNullOrEmpty(sCurriculumId))
                            {
                                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.SaveAcademicCurriculum);
                                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
                            }
                            else
                            {
                                dv.Add(Common.CP_ACADEMIC_CURRICULUM_2017.CURRICULUM_ID, sCurriculumId, EnumCommand.DataType.String);
                                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateAcademicCurriculumInfo);
                                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                resultArg.Success = false;
                using (ErrorLog objlog = new ErrorLog())
                {
                    objlog.WriteError("Academics", "BulkSaveCurriculum", "", ex.Message);
                }
            }
            return resultArg;
        }



        public ResultArgs DeleteCurriculumInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteCurriculumInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs FetchCourseBySemester(DataValue dv, string sAcademiYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCourseBySemester);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademiYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }


        #endregion
        #region Semester Root
        public ResultArgs FetchSemesterRootInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchSemesterRootInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);

            }
            return resultArg;
        }
        public ResultArgs UpdateActiveSemester(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateActiveSemester);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs ExecuteSemesterRoot(string Query)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = Query;
                resultArg = objHandler.ExecuteAsScripts(sSQL);
            }
            return resultArg;
        }
        public ResultArgs GetSemesterId(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.GetSemesterId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs BulkSaveSemesterRoot(JsonSemesterRoot objJson, string sAcademicYear)
        {
            try
            {
                string sSemesterRootId = string.Empty;
                using (MySQLHandler objHandler = new MySQLHandler())
                {
                    if (objJson != null)
                    {
                        if (objJson.SemesterRoot[0].IS_ACTIVE != "0")
                        {
                            sSQL = "SET SQL_SAFE_UPDATES = 0; ";
                            sSQL += AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckActiveSemesterRoot);
                            sSQL += " SET SQL_SAFE_UPDATES = 1;";
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_ACADEMIC_CURRICULUM_2017.BATCH, objJson.SemesterRoot[0].BATCH);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_ACADEMIC_CURRICULUM_2017.PROGRAMME, objJson.SemesterRoot[0].PROGRAMME);
                            resultArg = objHandler.ExecuteAsScripts(sSQL);
                        }
                        foreach (var item in objJson.SemesterRoot)
                        {
                            dv.Clear();
                            dv.Add(Common.CP_SEMESTER_ROOT_2017.PROGRAMME, item.PROGRAMME, EnumCommand.DataType.String);
                            dv.Add(Common.CP_SEMESTER_ROOT_2017.BATCH, item.BATCH, EnumCommand.DataType.String);
                            dv.Add(Common.CP_SEMESTER_ROOT_2017.SEMESTER, item.SEMESTER, EnumCommand.DataType.String);
                            dv.Add(Common.CP_SEMESTER_ROOT_2017.DATE_FROM, CommonMethods.ConvertdatetoyearFromat(item.DATE_FROM), EnumCommand.DataType.String);
                            dv.Add(Common.CP_SEMESTER_ROOT_2017.DATE_TO, CommonMethods.ConvertdatetoyearFromat(item.DATE_TO), EnumCommand.DataType.String);
                            dv.Add(Common.CP_SEMESTER_ROOT_2017.IS_ACTIVE, item.IS_ACTIVE, EnumCommand.DataType.String);
                            sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.IsSemesterRootExisting);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                            sSemesterRootId = objHandler.ExecuteScalar(dv, sSQL).StringResult;
                            if (string.IsNullOrEmpty(sSemesterRootId))
                            {
                                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.SaveSemesterRoot);
                                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
                            }
                            else
                            {
                                dv.Add(Common.CP_SEMESTER_ROOT_2017.ACC_SEMESTER_ID, sSemesterRootId, EnumCommand.DataType.String);
                                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateSemesterRoot);
                                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return resultArg;
        }
        public ResultArgs DeleteSemesterRootInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteSemesterRootInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region ClassCourse
        [DisplayName("Academic Year List")]
        public List<SUP_ACADEMIC_YEAR_LIST> AcademicYearList { set; get; }
        public List<ClassCourse> ClassCourseList { get; set; }
        [DisplayName("Class List")]
        public List<SUP_CLASS> ClassList { get; set; }


        public ResultArgs SaveClassCourse(JsonClassCourseRoot objClassCourseRoot)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                foreach (var item in objClassCourseRoot.JsonClassCourse)
                {
                    dv.Clear();
                    dv.Add(Common.CP_CLASS_COURSE_2017.CLASS_ID, item.sClassId, EnumCommand.DataType.String);
                    dv.Add(Common.CP_CLASS_COURSE_2017.COURSE_ID, item.sCourseId, EnumCommand.DataType.String);
                    dv.Add(Common.CP_CLASS_COURSE_2017.CLASS_COURSE_ID, item.ClassCourseId, EnumCommand.DataType.String);
                    if (item.ClassCourseId == "0")
                    {
                        sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.IsClassCourseExisting).Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, objClassCourseRoot.sAcademicYear);
                        if (objHandler.ExecuteScalar(dv, sSQL).StringResult.Equals("1"))
                        {
                            resultArg.Success = true;
                            continue;

                        }

                    }
                    sSQL = (item.ClassCourseId == "0") ? AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.SaveClassCourseInfo) : AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateClassCourseInfo);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, objClassCourseRoot.sAcademicYear);
                    resultArg = objHandler.ExecuteCommand(dv, sSQL, false);

                }
            }


            return resultArg;
        }

        /// <summary>
        /// This method is to fetch class course by sacademicyear and class id 
        /// </summary>
        /// <param name="sAcademicYear"></param>
        /// <param name="sClassId"></param>
        /// <returns>ResultArgs</returns>
        public ResultArgs FetchClassCourseList(string sAcademicYear, string sClassId)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                dv.Clear();
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchClassCourseList);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                dv.Add(Common.CP_CLASSES_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs FetchClassCourseByClassCourseId(string sAcademicYear, string sClassCourseId)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                dv.Clear();
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchClassCourseByClassCourseId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                dv.Add(Common.CP_CLASS_COURSE_2017.CLASS_COURSE_ID, sClassCourseId, EnumCommand.DataType.String);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs FetchClassesByClassTypeId(string sAcademicYear, string sClassTypeId)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                dv.Clear();
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchClassesByClassTypeId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                dv.Add(Common.CP_CLASS_COURSE_2017.CLASS_COURSE_ID, sClassTypeId, EnumCommand.DataType.String);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchCourseListByClassId(string sAcademicYear, string sClassId)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                dv.Clear();
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCourseListByClassId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                dv.Add(Common.CP_CLASSES_2017.CLASS_ID, sClassId, EnumCommand.DataType.String);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs DeleteClassCourse(string sAcademicYear, string sClassCourseId)
        {

            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteClassCourseInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                dv.Add(Common.CP_CLASS_COURSE_2017.CLASS_COURSE_ID, sClassCourseId, EnumCommand.DataType.String);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region Department
        public ResultArgs FetchDepartmentInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchDepartmentInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs InsertDepartmentInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertDepartmentInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs CheckDepartment(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckDepartment);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs UpdateDepartmentInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateDepartmentInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs FetchDepartmentInfoById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchDepartmentInfoById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs DeleteDepartmentInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteDepartmentInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region Faculty
        public ResultArgs FetchFAcutlyInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchFAcutlyInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs CheckFaculty(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckFaculty);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs InsertFacultyInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertFacultyInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs FetchFacultyInfoById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchFacultyInfoById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs UpdateFacultyInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateFacultyInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs DeleteFacultyInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteFacultyInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region Class
        public ResultArgs FetchClassInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchClassInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs CheckClass(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckClass);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs InsertClassInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertClassInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs FetchClassInfoById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchClassInfoById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs UpdateClassInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateClassInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs DeleteClassInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteClassInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region Batch
        public ResultArgs FetchBatchInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetcAcademichBatchInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs CheckBatch(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckAcademicBatch);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs InsertBatchInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertAcademicBatchInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs BulkSaveBatchInfo(JsonBatchInfo objJson, string sAcademicYear)
        {
            try
            {
                string sAcademicBatchId = string.Empty;
                using (MySQLHandler objHandler = new MySQLHandler())
                {
                    foreach (var item in objJson.BatchList)
                    {
                        dv.Clear();
                        dv.Add(Common.ACADEMIC_BATCHES.ACADEMIC_YEAR_ID, item.ACADEMIC_YEAR_ID, EnumCommand.DataType.String);
                        dv.Add(Common.ACADEMIC_BATCHES.BATCH_ID, item.BATCH_ID, EnumCommand.DataType.String);
                        sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckAcademicBatch);
                        sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);

                        sAcademicBatchId = objHandler.ExecuteScalar(dv, sSQL).StringResult;
                        if (string.IsNullOrEmpty(sAcademicBatchId))
                        {
                            sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertAcademicBatchInfo);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                            resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
                        }
                        else
                        {
                            dv.Add(Common.ACADEMIC_BATCHES.AC_BATCH_ID, sAcademicBatchId, EnumCommand.DataType.String);
                            sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateAcademicBatchInfo);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                            resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }
            return resultArg;
        }
        #endregion
        #region CourseType
        public ResultArgs FetchCourseTypeInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCourseTypeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs CheckCourseType(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckCourseType);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs InsertCourseTypeInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertCourseTypeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs FetchCourseTypeInfoById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchCourseTypeInfoById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs UpdateCourseTypeInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateCourseTypeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs DeleteCourseTypeInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteCourseTypeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region Programme
        public ResultArgs FetchProgrammeInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchProgrammeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs CheckProgramme(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckProgramme);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs InsertProgrammeInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertProgrammeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs FetchProgrammeInfoById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchProgrammeInfoById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs UpdateProgrammeInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateProgrammeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs DeleteProgrammeInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteProgrammeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region Degree
        public ResultArgs FetchDegreeInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchDegreeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs CheckDegree(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.CheckDegree);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs InsertDegreeInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.InsertDegreeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs FetchDegreeInfoById(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.FetchDegreeInfoById);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs UpdateDegreeInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.UpdateDegreeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        public ResultArgs DeleteDegreeInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = AcademicsSQL.GetAcademicSQL(AcademicSQLCommands.DeleteDegreeInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion
        #region DisposeMethod
        public void Dispose()
        {
            GC.Collect();
        }
        #endregion

    }
    public class AcademicCurriculumViewModel
    {
        [DisplayName("Academic Years")]
        public SelectList ddlAcademicYearList { get; set; }


    }
}
