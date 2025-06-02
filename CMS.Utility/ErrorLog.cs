using System;
using System.IO;



namespace CMS.Utility
{
    /// <summary>
    /// Summary description for ErrorLog
    /// </summary>
    public class ErrorLog : IDisposable
    {
        #region Properties
        DataValue dvLog = new DataValue();//To hold the log parameter
        static string slogfilepath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\SMS_Log");
        static string ImportFileName = "StudentImportInfo.txt";
        #endregion
        public ErrorLog()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Method used to write the exception passed an an arugement to targetted text file
        /// in a new line
        /// </summary>
        /// <param name="ex"></param>
        public void WriteError(Exception ex)
        {
            try
            {
                //Open the file
                WriteLog("-----------------------------------------------------------------------------------------");
                WriteLog("Date      :" + DateTime.Now.ToShortDateString());
                WriteLog("Time      :" + DateTime.Now.ToShortTimeString());
                WriteLog("Exception :" + ex.ToString());
                WriteLog("Source    :" + ex.Source);
                WriteLog("StackTrace:" + ex.StackTrace);
                WriteLog("-----------------------------------------------------------------------------------------");
            }
            catch (Exception msg)
            {


            }
        }
        /// <summary>
        /// Method used to write the exception passed an an arugement to targetted text file
        /// in a new line
        /// </summary>
        /// <param name="ex"></param>
        public void importError(string sName, string sAdmissionNo, string ClassId, string errMessage)
        {
            ImportLog("-----------------------------------------------------------------------------------------");
            ImportLog("Date      :" + DateTime.Now.ToShortDateString());
            ImportLog("Time      :" + DateTime.Now.ToShortTimeString());
            ImportLog("Student Name :" + sName);
            ImportLog("Admission No :" + sAdmissionNo);
            ImportLog("ErrorMessage :" + errMessage);
            ImportLog("-----------------------------------------------------------------------------------------");
        }

        private static void ImportLog(string sMessage)
        {
            string _sDirectorypath = string.Empty;
            bool _bSuccess = true;
            _sDirectorypath = string.Concat(Common.FilePath.SERVER_PATH, "\\", "HCC", "\\", Common.FilePath.IMPORT_DATA_PATH, System.DateTime.Now.Year.ToString(), "\\"); ;
            try
            {
                StreamWriter sw = File.AppendText((_sDirectorypath + "\\" + ImportFileName + ""));
                sMessage = ("::" + sMessage);
                sw.WriteLine(sMessage);
                sw.Close();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// This method is to write the passed error message and other details of error occurence
        /// </summary>
        /// <param name="sMessage">string - Error message</param>
        private static void WriteLog(string sMessage)
        {
            try
            {
                string filename = string.Empty;
                //check if the directory exists
                if (!Directory.Exists(slogfilepath))
                {
                    Directory.CreateDirectory(slogfilepath);
                }
                filename = string.Concat(slogfilepath, "\\", "SMS", DateTime.Today.ToShortDateString().Replace("/", "_"), ".log"); //create log file daily basis if exception is thorwn
                StreamWriter sw = File.AppendText(filename);
                sMessage = ("::" + sMessage);
                sw.WriteLine(sMessage);
                sw.Close();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Method used to write the exception passed an an arugement to targetted text file
        /// in a new line
        /// </summary>
        /// <param name="ex"></param>
        public void WriteError(string className, string methodName, string query, string errMessage)
        {
            try
            {
                WriteLog("-----------------------------------------------------------------------------------------");
                WriteLog("Date      :" + DateTime.Now.ToShortDateString());
                WriteLog("Time      :" + DateTime.Now.ToShortTimeString());
                WriteLog("ClassName :" + className);
                WriteLog("MethodName :" + methodName);
                if (!string.IsNullOrEmpty(query))
                    WriteLog("Query :" + query);
                WriteLog("ErrorMessage :" + errMessage);
                WriteLog("-----------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// Method used to write the exception passed an an arugement to targetted text file
        /// in a new line
        /// </summary>
        /// <param name="ex"></param>
        public void WriteError(string className, string methodName, string errMessage)
        {
            try
            {
                WriteLog("-----------------------------------------------------------------------------------------");
                WriteLog("Date      :" + DateTime.Now.ToShortDateString());
                WriteLog("Time      :" + DateTime.Now.ToShortTimeString());
                WriteLog("ClassName :" + className);
                WriteLog("MethodName :" + methodName);
                WriteLog("ErrorMessage :" + errMessage);
                WriteLog("-----------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {

            }
        }

        public void Dispose()
        {
            GC.Collect();
        }

    }
}
