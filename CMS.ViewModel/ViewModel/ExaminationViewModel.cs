using CMS.DAO;
using CMS.SQL.Examination;
using CMS.Utility;
using CMS.ViewModel.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CMS.ViewModel.ViewModel
{

    public class ProgrammeHeadsAmount
    {

        public List<PROGRAMME_HEADS_AMOUNT> programme { get; set; }
        public string sSemester { get; set; }
        public List<EXAM_PROGRAMME_WISE_HEADS> programeList { get; set; }
        public List<EXAM_COURSE_WISE_HEADS> CourseHeadsList { get; set; }
        public List<SUP_SUBJECT_TYPE> SubjectType { get; set; }
        public List<ACADEMIC_CURRICULUM_2017> academicCurriculum { get; set; }

    }
    public class ExaminationViewModel : IDisposable
    {
        ResultArgs resultArgs = new ResultArgs();
        public DataValue dv = new DataValue();
        string sSQL = string.Empty;
        public string sSemester { get; set; }
        public string Month_Year { get; set; }
        public List<Shift> liShift { get; set; }
        public List<CourseList> liCourse { get; set; }
        public bool sessionFlag { get; set; }
        // Quiz ...
        public int Correct_Answer { get; set; }
        public int Wrong_Answer { get; set; }
        public int Count { get; set; }
        public QUIZ_OPTIONS_2017 ObjQuizOption { get; set; }
        public QUIZ_QUESTIONS_2017 ObjQuizQuestion { get; set; }
        public QUIZ_2017 ObjQuiz { get; set; }
        public List<QUIZ_OPTIONS_2017> liQuizOption { get; set; }
        public List<QUIZ_RESULT> liQuizAnswer { get; set; }
        public List<QUIZ_QUESTIONS_2017> liQuizQuestion { get; set; }
        public List<QUIZ_2017> liQuizResult { get; set; }
        public List<LIST_QUIZ> liListQuiz { get; set; }
        public List<Student_Personal_Info> liAttendanceInfo { get; set; }
        // CIA ....
        public List<CIA_FETCH_COURSE_INFO> liCIACourse_Info { get; set; }
        public List<CIA_MARKS_INFO> liCIA_Marks_Info { get; set; }
        public List<CIA_COMPONENT> liCIA_Component { get; set; }
        public List<CIA_TOTAL> liCIA_Total { get; set; }
        public List<CIA_STAFF_INFO> liCIA_Staff_Info { get; set; }
        // Exam Registration.....
        public EXAM_REGISTRATION objExamRegistration { get; set; }
        public List<EXAM_REGISTRATION> liExamRegistrationList { get; set; }
        public List<EXAM_REGISTRATION_2017> liExamRegisteredList { get; set; }
        public List<EXAM_REGISTRATION> liExamRegistrationModel { get; set; }
        public List<FEE_STUDENT_ACCOUNT> feeTransactions { get; set; }
        public List<SEMESTER_RESULT> liSemesterList { get; set; }
        public List<FetchHallTicket> liFetchHallTicket { get; set; }
        public List<HallTicketDetails> liRegistration { get; set; }
        public List<CollegeDetails> liCollegeDetails { get; set; }
        // public List<EXAM_REGISTRATION_LIST> liExamRegistrationList { get; set; }
        public SelectList Batch { get; set; }
        public SelectList Program { get; set; }
        public SelectList Heads { get; set; }
        public List<EXAM_SETTING> examSetting { get; set; }

        public SelectList Shift { get; set; }
        public SelectList CourseList { get; set; }
        public SelectList ClassList { get; set; }
        public SelectList ExamList { get; set; }
        public List<CourseComponents> objComponents { get; set; }
        public CourseInfo objCourseInfo { get; set; }
        public List<CIAMarks> liCIAMarks { get; set; }
        public List<CIA_TOTAL_MARKS> liCIATotalMarks { get; set; }
        public List<CIA_COMPONENTS_MARK> liCIAComponents { get; set; }

        public CIA_COURSE_AND_CLASS_INFO objCIA_COURSE_AND_CLASS_INFO { get; set; }

        public ResultArgs FetchCourseList(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseList);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                //  sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.CP_CLASS_COURSE_STAFF_2017.SEMESTER_ID, sSemesterId);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }
        public ResultArgs FetchClassList(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchClassList);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }
        public ResultArgs FetchCIAInfoForRegularByClassAndCourseId(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIAInfoForRegularByClassAndCourseId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }
        public ResultArgs FetchCIAInfoForSelectedByClassAndCourseId(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCIAInfoForSelectedByClassAndCourseId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }
        public ResultArgs FetchSclassId(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchSclassId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }

        public ResultArgs FetchCourseInfoByClassAndCourseId(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseInfoByClassAndCourseId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }
        public ResultArgs FetchCourseComponentByCourseTypeId(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseComponentByCourseTypeId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }
        public ResultArgs ExcuteCIA_Marks(string sQuery)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = sQuery;
                resultArgs = objHandler.ExecuteAsScripts(sSQL);
            }
            return resultArgs;
        }

        public ResultArgs FetchCourseWiseCIAMarks(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseWiseCIAMarks);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }
        public ResultArgs FetchCourseWiseCIAMarksForStudents(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseWiseCIAMarksForStudents);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }
        public ResultArgs FetchCourseAndClassInfoByClassAndCourseId(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = ExaminationSQL.GetExaminationSQL(ExaminationSQLCommands.FetchCourseAndClassInfoByClassAndCourseId);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArgs;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
