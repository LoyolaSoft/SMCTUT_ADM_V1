using MessagingToolkit.QRCode.Codec;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace CMS.Utility
{
    public abstract class CommonMethods
    {
        public static string GetEncodeQRCodeWithLogo(string Content)
        {
            string sResult = string.Empty;
            try
            {

                QRCodeEncoder encoder = new QRCodeEncoder();
                encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;//30%               
                encoder.QRCodeScale = 5;
                if (Content.Length > 75)
                    encoder.QRCodeScale = 3;
                var img = encoder.Encode(Content);
                System.Drawing.Image logo = System.Drawing.Image.FromFile(string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"//Content//hcc_img//128_128.jpg"));
                int left = (img.Width / 2) - 20;
                int top = (img.Height / 2) - 20;
                Graphics g = Graphics.FromImage(img);
                g.DrawImage(logo, new Point(left, top));
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                sResult = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(byteImage));
                return sResult;
            }
            catch (Exception ex)
            {
                string sErrorMessage = ex.Message;
                return sResult;
            }


        }
        public static string GetMethodForRazorpay(string sBaseUrl, string sKey, string sSecret)
        {
            string sResult = string.Empty;
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                request.Method = "GET";
                request.ContentLength = 0;
                request.ContentType = "application/json";
                string authString = string.Format("{0}:{1}", sKey, sSecret);
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));
                var response = (HttpWebResponse)request.GetResponse();
                sResult = ParseResponse(response);
                return sResult;
            }
            catch (Exception ex)
            {
                string sErrorMessage = ex.Message;
                return sResult;
            }


        }
        private static string ParseResponse(HttpWebResponse response)
        {
            string responseValue = string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseValue = reader.ReadToEnd();
                    }
            }

            return responseValue;
        }
        public static object PropertyMapping<TSource, TDestination>(object oSource, object oDestination) where TSource : new() where TDestination : new()
        {
            // Unix timestamp is seconds past epoch
            TSource s = new TSource();
            TDestination d = new TDestination();
            Type ts = s.GetType();
            Type td = s.GetType();

            PropertyInfo[] propertyItem = ts.GetProperties();

            foreach (var item in propertyItem)
            {


                if (d.GetType().GetProperty(item.Name.ToUpper()) != null)
                {
                    d.GetType().GetProperty(item.Name.ToUpper()).SetValue(d, item.GetValue(oSource, null), null);
                    //item.SetValue(d, item.GetValue(oSource, null), null);
                }

            }

            return d;
        }
        public static string GetHashValueForRefund(string hashSeq, string sMerchant_key, string txids, string payuId, string Amount, string sCommand, string sSalt)
        {
            string hash_string = string.Empty;
            string hash = string.Empty;
            string[] hashVarsSeq = hashSeq.Split('|'); // spliting hash sequence from config       
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = hash_string + sMerchant_key;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "command")
                {
                    hash_string = hash_string + sCommand;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "var1")
                {
                    hash_string = hash_string + payuId;
                    hash_string = hash_string + '|';
                }
                //else if (hash_var == "var2")
                //{
                //    hash_string = hash_string + txids;
                //    hash_string = hash_string + '|';
                //}
                //else if (hash_var == "var3")
                //{
                //    hash_string = hash_string + Amount;
                //    hash_string = hash_string + '|';
                //}


            }
            //hash_string = hash_string + '|';
            hash_string += sSalt;// appending SALT

            hash = CommonMethods.Generatehash512(hash_string).ToLower();         //generating hash
            return hash;
        }

        public static string ValidateCreditCount(int CreditCount)
        {
            string sCount = string.Empty;
            if (CreditCount <= 160)
            {
                sCount = "1";
            }
            else if (CreditCount <= 306)
            {
                sCount = "2";
            }
            else if (CreditCount <= 459)
            {
                sCount = "3";
            }
            else if (CreditCount <= 612)
            {
                sCount = "4";
            }
            else if (CreditCount <= 765)
            {
                sCount = "5";
            }
            else if (CreditCount <= 918)
            {
                sCount = "6";
            }
            else
            {
                sCount = "7";
            }
            return sCount;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string GenerateOTP()
        {
            string sOTP = string.Empty;
            char[] charArr = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[] strrandomArr;
            string strrandom = string.Empty;

            Random objran = new Random();
            int noofcharacters = 6;
            for (int i = 0; i < noofcharacters; i++)
            {
                //It will not allow Repetation of Characters
                int pos = objran.Next(1, charArr.Length);
                if (!strrandom.Contains(charArr.GetValue(pos).ToString()))
                    strrandom += charArr.GetValue(pos);
                else
                    i--;
            }
            strrandomArr = strrandom.ToCharArray();
            foreach (var item in strrandomArr)
            {
                sOTP += ((int)item).ToString();
            }

            return sOTP.Substring(sOTP.Length - 6, noofcharacters);
        }

        public static bool SendMailFromGmailSMTP(string sfromEmail, string sPassword, string sToMail, string sSubject, string sContent, bool bIsHtmlBody = false, string sDomain = "", int sSMTP = 0)
        {
            bool bResult = false;

            try
            {
                bool bSSL = Convert.ToBoolean(ConfigurationManager.AppSettings[Common.AppSettingKeys.SSL].ToString());
                sSMTP = (sSMTP == 0) ? Convert.ToInt16(ConfigurationManager.AppSettings[Common.AppSettingKeys.SMTPPORT].ToString()) : sSMTP;
                sDomain = (string.IsNullOrEmpty(sDomain)) ? ConfigurationManager.AppSettings[Common.AppSettingKeys.Domain].ToString() : sDomain;
                MailMessage message = new MailMessage(sfromEmail, sToMail);
                message.Subject = sSubject;
                message.Body = sContent;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = bIsHtmlBody;
                SmtpClient client = new SmtpClient(sDomain, sSMTP);
                System.Net.NetworkCredential basicCredential = new NetworkCredential(sfromEmail, sPassword);
                client.EnableSsl = bSSL;
                client.UseDefaultCredentials = true;
                client.Credentials = basicCredential;
                client.Send(message);
                bResult = true;
            }
            catch (Exception ex)
            {
                using (Utility.ErrorLog log = new Utility.ErrorLog())
                {
                    log.WriteError("CommonMethod", "SendMailFromGmailSMTP", ex.Message);
                }
            }
            return bResult;

        }
        public static object TEST_HttpPostMethod(string sBaseUrl, string sParameter)
        {
            object oResult = new object();
            if (!string.IsNullOrEmpty(sBaseUrl))
            {
                var request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                var postData = sParameter;
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            return oResult;
        }


        public static object HttpPostMethod(string sBaseUrl, Dictionary<string, string> sParameter)
        {
            object oResult = new object();
            try
            {
                if (!string.IsNullOrEmpty(sBaseUrl))
                {
                    //  var content = new StringContent(sParameter, Encoding.UTF8, "application/json");
                    var content = new FormUrlEncodedContent(sParameter);
                    using (HttpClient c = new HttpClient())
                    {
                        var result = c.PostAsync(sBaseUrl, content).Result;
                        oResult = result.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility.ErrorLog log = new Utility.ErrorLog())
                {
                    log.WriteError("CommonMethod", "HttpPostMethod", ex.Message);

                }
            }

            return oResult;

        }
        public static string CommonTransactionId(string userid)
        {
            string sTransactionId = string.Empty;
            Random rnd = new Random();
            string strHash = Generatehash512(userid + rnd.ToString() + DateTime.Now);
            sTransactionId = strHash.ToString().Substring(0, 14) + userid;
            return sTransactionId;
        }
        public static string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }

        public static string SemestersInComma(int nInput)
        {
            string sResult = string.Empty;
            int i = nInput;
            while (i > 0)
            {
                sResult += i + ",";
                i -= 2;
            }

            return sResult.TrimEnd(',');

        }
        /// <summary>
        /// This is to display the alert msg 
        /// </summary>
        /// <param name="alertmsg"></param>
        /// <param name="Message"></param>
        /// <param name="Show"></param>
        public static void ShowMessage(Literal alertmsg, string sStyle, string Message, bool Show)
        {
            Message = string.Format(sStyle, Message);
            alertmsg.Text = Message;
            alertmsg.Visible = Show;
        }

        /// To bind asp dropdownlist
        /// <param name="dropDownCombo">asp dropdownlist - Control to assign the datasource</param>
        /// <param name="dataSource">DataTable - Data to bind the control</param>
        /// <param name="listField">String - List field is for asp dropdownlist control</param>
        /// <param name="valueField">ValueField - Value field is for asp dropdownlist control</param>
        /// <param name="isAddEmptyItem">bool</param>
        /// <param name="emptyItemName">string-first selected value</param>
        public static void BindDataCombo(System.Web.UI.WebControls.DropDownList dropDownCombo, object dataSource, string listField, string valueField, bool isAddEmptyItem = false, string emptyItemName = "")
        {
            if (dataSource != null)
            {
                dropDownCombo.DataSource = dataSource;
                dropDownCombo.DataTextField = listField;
                dropDownCombo.DataValueField = valueField;
                dropDownCombo.DataBind();
                if (isAddEmptyItem)
                {
                    System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem(emptyItemName, "0");
                    dropDownCombo.Items.Insert(0, lstSelect);
                }
            }
            else
            {
                dropDownCombo.DataSource = null;
                dropDownCombo.DataBind();
            }

        }
        /// <summary>
        /// This is to save the images in the local for binding SIGNATURE in the PDF
        /// </summary>
        public static string SaveImage(FileUpload fuUpload, string FilePath, string FileName)
        {
            string sFileExtension = string.Empty;
            string sImageFilePath = string.Empty;
            string imageName = string.Empty;
            try
            {
                string _sDirectorypathImage = string.Empty;
                string _sDirectorypathSignature = string.Empty;
                string sFileName = string.Empty;
                _sDirectorypathImage = AppDomain.CurrentDomain.BaseDirectory + "\\" + FilePath;
                if (!Directory.Exists(_sDirectorypathImage))
                    Directory.CreateDirectory(_sDirectorypathImage);

                if (fuUpload.HasFile)
                {
                    //Check the manipulation mode for inserting default image in the add mode
                    if (fuUpload.FileBytes.Length > 0)
                    {
                        imageName = _sDirectorypathImage + FileName.Trim() + System.IO.Path.GetExtension(fuUpload.FileName);
                        DeleteImages(imageName);  //Delete the photo if images name exists already for the regno
                        fuUpload.SaveAs(imageName);
                        sImageFilePath = "../../" + FilePath + FileName.Trim() + System.IO.Path.GetExtension(fuUpload.FileName);
                    }
                }

            }
            catch (Exception ex)
            {

                using (Utility.ErrorLog log = new Utility.ErrorLog())
                {
                    log.WriteError("CommonMethod", "SaveImage", ex.Message);
                }
            }
            return sImageFilePath;
        }

        /// <summary>
        /// This is to delete the existing image for the reg no
        /// </summary>
        /// <param name="filename"></param>
        private static void DeleteImages(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                try
                {
                    if (File.Exists(Common.URLPages.SERVER_PATH + Common.URLPages.STORE_PATH + filename))
                        File.Delete(Common.URLPages.SERVER_PATH + Common.URLPages.STORE_PATH + filename);
                }
                catch (Exception ex)
                {
                    using (Utility.ErrorLog log = new Utility.ErrorLog())
                    {
                        log.WriteError("CommonMethod", "DeleteImages", ex.Message);

                    }
                }
            }
        }

        #region Generate Random Password
        /// <summary>
        /// This method is to generate Random Password which is the combination of Alphabets and Numeric of 10 characters
        /// </summary>
        /// <returns>string</returns>
        public static string GetRandomPassword()
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz0123456989".ToCharArray();
            string password = string.Empty;
            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                int x = random.Next(1, chars.Length);
                //Don't Allow repeation of Characters
                if (!password.Contains(chars.GetValue(x).ToString()))
                    password += chars.GetValue(x);
                else
                    i--;
            }
            return password;
        }
        #endregion
        #region Encryption Method
        public static string GetSha256FromString(string strData)
        {
            var Message = Encoding.ASCII.GetBytes(strData);
            SHA256Managed hashString = new SHA256Managed();
            string Hex = string.Empty;
            var Hashvalue = hashString.ComputeHash(Message);
            foreach (byte x in Hashvalue)
            {
                Hex += string.Format("{0:x2}", x);
            }
            return Hex;
        }
        #endregion
        #region Sending Mail

        #endregion
        /// <summary>
        /// Convert date from dd/MM/YYYY to YYYY/MM/dd
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static string ConvertdatetoyearFromat(string dateString)
        {
            if (!string.IsNullOrEmpty(dateString))
            {
                DateTime dt = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                return dt.ToString("yyyy/MM/dd");
            }
            else
                dateString = DBNull.Value.ToString();
            return dateString;
        }
        /// <summary>
        /// Convert date from MM/dd/YYYY to YYYY/MM/dd
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static string Convert_mmdate_toyearFromat(string dateString)
        {
            if (!string.IsNullOrEmpty(dateString))
            {
                DateTime dt = DateTime.ParseExact(dateString, "MM/dd/yyyy",
                                  CultureInfo.InvariantCulture);
                return dt.ToString("yyyy/MM/dd");
            }
            else
                return dateString = DBNull.Value.ToString();

        }

        /// <summary>
        /// This is to get the rights access for the code and field
        /// </summary>
        /// <param name="code">activity or page code</param>
        /// <param name="fieldname">activity or page field to compare</param>
        /// <returns></returns>
        //public static Int64 ApplyRights(string code, string fieldname)
        //{
        //    Int64 enablemenu = 0;
        //    UserProperty objuserpro = new UserProperty();
        //    //Apply the Rights based on Menu Enable and Disable
        //    //Get the Values from Session if it is not Null
        //    if (objuserpro.LoginUserRights != null && objuserpro.LoginUserRights.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            string sfield = string.Empty;

        //            var query = objuserpro.LoginUserRights.AsEnumerable()
        //                        .Where(r => r.Field<string>(fieldname).ToString().ToLower().Equals(code.ToLower()))
        //                        .Select(r => r.Field<Int64>(MessageCatalog.UserRights.Allow)).FirstOrDefault();
        //            enablemenu = (Int64)query;
        //        }
        //        catch (Exception ex)
        //        {
        //            using (ErrorLog log = new ErrorLog())
        //            {
        //                log.WriteError("CommonMethod", "ApplyRights(string code, string fieldname)", ex.Message);
        //            }
        //        }
        //    }
        //    return enablemenu;
        //}



        /// <summary>
        /// This method is to get the exception line no
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>line no of the exception</returns>
        public static string GetExceptionLineNo(Exception ex)
        {
            return ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(Common.Delimiter.COLON) + 5);
        }

        public static void OptimizeNResize(Bitmap filecontents, string imgPath, string imgName, string extension, Int64 imagewidth, Int64 imageHeight)
        {
            try
            {
                Bitmap final_image = null;
                Graphics graphic = null;
                int reqW = Int32.Parse(imagewidth.ToString());
                int reqH = Int32.Parse(imageHeight.ToString());
                final_image = new Bitmap(reqW, reqH);
                graphic = Graphics.FromImage(final_image);
                graphic.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, reqW, reqH));
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic; /* new way */
                graphic.DrawImage(filecontents, 0, 0, reqW, reqH);
                if (graphic != null) graphic.Dispose();
                final_image.Save(HttpContext.Current.Server.MapPath(imgPath + imgName));
                if (filecontents != null) filecontents.Dispose();
                if (final_image != null) final_image.Dispose();
            }
            catch (Exception ex)
            {


            }

        }

        //this method to convert datatabel into html table.
        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

        //this method to convert datatabel into html table.
        public static DataTable ConvertStringToDataTableFormVaayooRespose(string mVaayooRespose)
        {
            DataTable dtTemp = new DataTable();
            string sResponse = string.Empty;
            string[] strReposnseArray;
            strReposnseArray = mVaayooRespose.Split(',');
            //need to code

            return dtTemp;
        }

        /// <summary>
        /// This method is to convert the datatable to list to bind to jqgrid
        /// </summary>
        /// <param name="dsProducts">datatable to be converted</param>
        /// <returns></returns>
        public static List<object> ConvertDataTableToList(DataTable dsProducts)
        {
            List<object> objListOfEmployeeEntity = new List<object>();
            try
            {
                if (dsProducts != null)
                {
                    if (dsProducts.Rows.Count > 0)
                    {
                        foreach (DataRow dRow in dsProducts.Rows)
                        {
                            Hashtable hashtable = new Hashtable();
                            foreach (DataColumn column in dsProducts.Columns)
                            {
                                if (column.DataType == typeof(int))
                                {
                                    if (string.IsNullOrEmpty(dRow[column.ColumnName].ToString()))
                                    {
                                        hashtable.Add(column.ColumnName, string.Empty);
                                    }
                                    else
                                    {
                                        hashtable.Add(column.ColumnName, Convert.ToInt16(dRow[column.ColumnName].ToString()));
                                    }
                                }
                                else
                                    hashtable.Add(column.ColumnName, dRow[column.ColumnName].ToString());
                            }
                            objListOfEmployeeEntity.Add(hashtable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objListOfEmployeeEntity;
        }

        public static void BindDropDownList(DropDownList ddl, DataTable dt, string sDataField, string sValueField, bool bAddSelectItem)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                ddl.DataSource = dt;
                ddl.DataValueField = sValueField;
                ddl.DataTextField = sDataField;
                ddl.DataBind();
                if (bAddSelectItem)
                {
                    ListItem emptyItem = new ListItem();
                    emptyItem.Text = "--Select--";
                    emptyItem.Value = "0";
                    emptyItem.Selected = true;
                    ddl.Items.Add(emptyItem);
                }
            }

        }
        public static void BindListBox(ListBox lbtn, DataTable dt, string sDataField, string sValueField, bool bAddSelectItem)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                lbtn.DataSource = dt;
                lbtn.DataValueField = sValueField;
                lbtn.DataTextField = sDataField;
                lbtn.DataBind();
                //if (bAddSelectItem)
                //{
                //    ListItem emptyItem = new ListItem();
                //    emptyItem.Text = "--Select--";
                //    emptyItem.Value = "0";
                //    emptyItem.Selected = true;
                //    lbtn.Items.Add(emptyItem);
                //}
            }

        }
        public static DataTable ConstructDataTable(string[] sDataColumns)
        {
            DataTable dtTemp = new DataTable();
            for (int i = 0; i < sDataColumns.Length; i++)
            {
                dtTemp.Columns.Add(sDataColumns[i]);
            }
            return dtTemp;
        }
        //DataTable to Json convertor
        public static string DataTableToJSON(DataTable table)
        {
            string JSONstring = string.Empty;
            JSONstring = JsonConvert.SerializeObject(table);
            return JSONstring;
        }
        /// <summary>
        /// this method to convert to text into unicode for mvaayoo api
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string UnicodeForMvaayooAPI(string sText)
        {
            string strTemp = string.Empty;
            foreach (char item in sText)
            {
                strTemp += ((int)item).ToString("x4");
            }
            return strTemp;
        }


        /// <summary>
        /// this method used to convrt in csv format
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sColName"></param>
        /// <returns></returns>
        public static string DataColumnIntoCSV(DataTable dt, string sColName)
        {
            string strCSV = string.Empty;
            foreach (DataRow item in dt.Rows)
            {
                if (!string.IsNullOrEmpty(item[sColName].ToString()))
                {
                    strCSV += item[sColName].ToString() + ",";
                }
            }
            return strCSV.TrimEnd(',');
        }

        public static DataTable CSVToDataTable(string strCSV, string sColName)
        {
            DataTable dtTable = new DataTable();
            if (!string.IsNullOrEmpty(strCSV))
            {
                dtTable.Columns.Add(sColName);
                string[] strArray = strCSV.Split(',');
                for (int i = 0; i < strArray.Length; i++)
                    dtTable.Rows.Add(strArray[i]);
                return dtTable;
            }
            else
            {
                return dtTable = null;
            }
            return dtTable;
        }

        // this method for bulk Insert query construction
        public static string BulkInsertQueryConstruction(string[] strColArray, DataTable dtTemp, string sTableName)
        {
            string strBulkQuery = string.Empty;
            string sColumns = string.Empty;
            string sColumnsWithValues = string.Empty;
            strBulkQuery = "INSERT INTO " + sTableName + "VALUES ";
            sColumns = "(";
            foreach (string strColumns in strColArray)
                sColumns += strColumns + ",";

            foreach (DataRow row in dtTemp.Rows)
            {
                sColumnsWithValues += "(";
                foreach (string colName in strColArray)
                {
                    sColumnsWithValues += row[colName] + ",";

                }
                sColumnsWithValues = sColumnsWithValues.TrimEnd(',');
                sColumnsWithValues += "),";
            }
            return strBulkQuery;
        }

        public static string DateConversion(string sFormats, string sDate)
        {
            string strResult = DBNull.Value.ToString();
            DateTime date = DateTime.Parse(sDate);
            strResult = string.Format("{0:1}", sDate, sFormats);
            return strResult;
        }
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
        public static string ToDB  //for to db name from webconfig
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("TODB");
            }
        }
    }
}
