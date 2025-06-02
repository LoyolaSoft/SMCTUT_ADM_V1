using CMS.Utility;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;


namespace CMS.DAO
{
    public class DapperHandler
    {
        MySqlConnection connection;
        private string rowUniqueParmName = "";
        private bool getRowUniqueId = false;
        private bool bFlag = false;
        private const string PARAMETER_DELIMITER = "?";
        public string CurrentConnectionString = ConfigurationManager.ConnectionStrings["CMS"].ConnectionString;


        public DapperHandler()
        {
            try
            {
                connection = new MySqlConnection(CurrentConnectionString);
            }
            catch (Exception ex)
            {
                using (ErrorLog objlog = new ErrorLog())
                {
                    objlog.WriteError("DapperHandler", ex.Message, "", "Query is empty!");
                }
            }

        }
        public ResultArgs Save(object obj, string sSQL, string sAcademicYear = null)
        {
            ResultArgs result = new ResultArgs();
            try
            {
                if (!string.IsNullOrEmpty(sSQL))
                {
                    if (sAcademicYear != string.Empty) { }
                    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                    sSQL = sSQL.Replace(Common.Delimiter.QUS, "@");

                    //  result.RowsAffected = connection.Execute(sSQL, obj);
                    result.Success = true;
                }
                else
                {
                    using (ErrorLog objlog = new ErrorLog())
                    {
                        objlog.WriteError("DapperHandler", "", "", "Query is empty!");
                    }
                }

            }
            catch (Exception ex)
            {
                using (ErrorLog objlog = new ErrorLog())
                {
                    objlog.WriteError("DapperHandler", ex.Message, "Save(string sSQL,object obj,string sAcademicYear=null)", sSQL);
                }
            }
            return result;
        }
    }
}
