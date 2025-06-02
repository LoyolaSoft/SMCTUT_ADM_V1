using CMS.DAO;
using CMS.SQL.UserAccount;
using CMS.Utility;
using CMS.ViewModel.Model;
using System;

namespace CMS.ViewModel.ViewModel
{
    public class AccountViewModel : IDisposable
    {
        ResultArgs resultArgs = new ResultArgs();
        DataValue dv = new DataValue();
        string sSQL = string.Empty;
        public LoginViewModel objLoginViewModel { get; set; }


        public static bool ValidateAction(string sController, string sAction, string sUserId)
        {
            ResultArgs resultArgs = new ResultArgs();
            DataValue dv = new DataValue();
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                string sSQL = string.Empty;
                bool bResult = false;
                dv.Clear();
                dv.Add(Common.MENU_ITEMS.ACTION, sAction, EnumCommand.DataType.String);
                dv.Add(Common.MENU_ITEMS.CONTROLLER, sController, EnumCommand.DataType.String);
                dv.Add("ROLE_NAME", sUserId, EnumCommand.DataType.String);
                sSQL = UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.ValidateUserRole);
                resultArgs = objHandler.ExecuteScalar(dv, sSQL);
                if (resultArgs.Success)
                {
                    bResult = string.Equals(resultArgs.StringResult, "1");
                }
                return bResult;
            }
        }
        public ResultArgs FetchUserCredential(DataValue dvParam)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserCredentials);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dvParam);
            }
            return resultArgs;
        }

        public ResultArgs FetchMenuItemsByRole(DataValue dvParam)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchMenuItemsByRole);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dvParam);
            }
            return resultArgs;
        }
        public ResultArgs FetchCollegeInfo()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchCollegeInfo);
                resultArgs = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArgs;
        }
        public ResultArgs FetchActiveYear()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchActiveYear);
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
