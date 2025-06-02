using CMS.DAO;
using CMS.SQL.Feedback;
using CMS.Utility;
using CMS.ViewModel.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CMS.ViewModel.ViewModel
{
    public class FeedbackViewModel
    {
        private string sSQL { get; set; }
        ResultArgs resultArg = new ResultArgs();
        DataValue dv = new DataValue();
        //public IEnumerable<SelectListItem> StaffList { get; set; }
        public string sValidateStaff { get; set; }
        public FBCLASS_WISE_STAFF objFBStaff { get; set; }
        public SelectList ClasswiseStaffList { get; set; }
        public SelectList CourseList { get; set; }
        public SelectList ClassList { get; set; }
        public SelectList StudentCourseList { get; set; }
        public List<FBOBJECTIVES> ObjectiveList { get; set; }
        public List<FBFEEDBACK_QUESTIONS> QuestionList { get; set; }
        public List<FBQUESTIONS_GROUP> QuestionGroup { get; set; }
        public List<FBRATING_BY_STAFF> StaffwiseRatingList { get; set; }

        public SelectList DepartmentList { get; set; }
        public SelectList ShiftList { get; set; }
        public SelectList StaffListByDepartmet { get; set; }

        //THIS PROPERTIES FOR STAFF RESULT

        public FBCOURSE_AND_CLASS_INFO objCourseAndClassInfo { get; set; }
        public FBFEEDBACK_SETTINGS objFBSetting { get; set; }
        public List<FBRATING_BY_STAFF> objRating { get; set; }


        #region Methods
        public ResultArgs FetchClassWiseStaffList(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchFBClasswiseStaff);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchCourseListByStaffId(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchCourseListByStaffId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs FetchFBClassesByStaffId(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchFBClassesByStaffId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs FetchCourseWiseStaffList(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchFBCoursewiseStaff);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;

        }

        public ResultArgs FetchObjective()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                dv.Clear();
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchFBObjectives);
                dv.Add(Common.FBOBJECTIVES.QUESTION_TYPE, "2", EnumCommand.DataType.String);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchQuestion(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.CheckCourseTypeByCourseId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteScalar(dv, sSQL);
                if (resultArg.Success)
                {
                    sSQL = string.Empty;
                    sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FectchFBQuestions);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.FBQUESTIONS_GROUP.QUESTION_GROUP_ID, (resultArg.StringResult == "1") ? Common.FBQUESTION_GROUP_IDS.ForParacticals : Common.FBQUESTION_GROUP_IDS.NotForParacticals);
                }
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs FetchStudentCourseListByClassId(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchStudentCourseListByClassId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs FetchStudentInfoById(string sStudentId, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                dv.Clear();
                dv.Add("STUDENT_ID", sStudentId, EnumCommand.DataType.String);
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchStudentInfo);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs SaveFeedback(string sSQL)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                resultArg = objHandler.ExecuteCommand(sSQL, false);
            }
            return resultArg;
        }

        public ResultArgs FetchOverallRatingByStaffId(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchOverallRatingByStaffId);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs FetchCoursesByStudentId(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchCoursesByStudentId);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs ValidateExisitingCourseList(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.ValidateCourseByStudentId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.ExecuteScalar(dv, sSQL);
            }
            return resultArg;
        }

        public ResultArgs FetchFBSettingInfo()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchFeedbackInfo);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }
        public ResultArgs FetchFBClassCourseInfo(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchCourseAndClassInfoByCourse);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchFBResultByCourseId(DataValue dv, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchQuestionWiseRatingByStaffId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        public ResultArgs FetchDepartmentList(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchDepartmentList);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchShiftList()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchShiftList);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchDepartmentByShiftId(DataValue objDV, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchDepartmentByShiftId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, objDV);
            }
            return resultArg;
        }
        public ResultArgs FetchStaffListByDepartment(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchStaffListByDepartment);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchQuestionwiseReportByStaffIds(string sStaffIds, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchQuestionWiseReportByStaffIds);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.FBEVALUATION2017.ASSESSEE, sStaffIds);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        public ResultArgs FetchQuestionwiseReportByDepartmentsIds(string sDepartIds, string sShiftId, string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.DEPARTMENT, sDepartIds, EnumCommand.DataType.String);
                dv.Add(Common.CP_CLASSES_2017.SHIFT, sShiftId, EnumCommand.DataType.String);

                sSQL = FeedbackSQL.GetFeedbackSQL(FeedbackSQLCommands.FetchQuestionwiseReportByDepartmentsIds);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                //   sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.STF_PERSONAL_INFO.DEPARTMENT, sDepartIds);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        #endregion


    }
}
