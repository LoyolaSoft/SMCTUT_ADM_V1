using CMS.DAO;
using CMS.SQL.Dashboard;
using CMS.Utility;
using CMS.ViewModel.Model;
using System;
using System.Collections.Generic;

namespace CMS.ViewModel.ViewModel
{
    public class DashboardViewModel : IDisposable
    {
        private string sSQL { get; set; }
        ResultArgs resultArg = new ResultArgs();
        DataValue dv = new DataValue();
        public List<AssignmentModel> liAssignment { get; set; }
        public List<stf_Personal_Info> liStaff { get; set; }
        public List<ADM_ISSUE_APPLICATION_2018> liIssueApplication { get; set; }
        public string sNoOfStudentsFeedbackPosted { get; set; }

        public ResultArgs FetchStudentInfoById(DataValue dvParam)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = DashboardSQL.GetDashboardSQL(DashboardSQLCommands.FetchStudentInfoByUserId);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dvParam);
                return resultArg;
            }
        }
        public ResultArgs FetchStaffInfoById(DataValue dvParam)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = DashboardSQL.GetDashboardSQL(DashboardSQLCommands.FetchStaffInfoByUserId);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dvParam);
                return resultArg;
            }
        }

        public ResultArgs GetOverallFeedbackCountByStudent()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = DashboardSQL.GetDashboardSQL(DashboardSQLCommands.GetFeedbackCountByStudent);
                resultArg = objHandler.ExecuteScalar(sSQL);
                return resultArg;
            }

        }

        // count
        public ResultArgs GetOverallReceivedApplicationCount()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = DashboardSQL.GetDashboardSQL(DashboardSQLCommands.GetReceivedAppCount);
                resultArg = objHandler.ExecuteScalar(sSQL);
                return resultArg;
            }

        }
        public ResultArgs GetOverallIssuedApplicationCount()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = DashboardSQL.GetDashboardSQL(DashboardSQLCommands.GetIssuedAppCount);
                resultArg = objHandler.ExecuteScalar(sSQL);
                return resultArg;
            }

        }

        public ResultArgs GetOverallSelectedApplicationCount()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = DashboardSQL.GetDashboardSQL(DashboardSQLCommands.GetSelectedAppCount);
                resultArg = objHandler.ExecuteScalar(sSQL);
                return resultArg;
            }

        }
        public ResultArgs GetOverallAdmittedApplicationCount()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = DashboardSQL.GetDashboardSQL(DashboardSQLCommands.GetAdmittedAppCount);
                resultArg = objHandler.ExecuteScalar(sSQL);
                return resultArg;
            }

        }
        public ResultArgs GetOverallVerifiedApplicationCount()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = DashboardSQL.GetDashboardSQL(DashboardSQLCommands.GetVerifiedAppCount);
                resultArg = objHandler.ExecuteScalar(sSQL);
                return resultArg;
            }

        }
        public ResultArgs NoOfStudents()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = DashboardSQL.GetDashboardSQL(DashboardSQLCommands.GetTotalNoOfStudents);
                resultArg = objHandler.ExecuteScalar(sSQL);
                return resultArg;
            }

        }
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
