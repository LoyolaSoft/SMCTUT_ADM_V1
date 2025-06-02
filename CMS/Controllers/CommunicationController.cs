using CMS.DAO;
using CMS.Models;
using CMS.SQL.Communication;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CMS.Controllers
{
    public class CommunicationController : Controller
    {
        JsonResultData ObjJsonData = new JsonResultData();
        ResultArgs resultArgs = new ResultArgs();
        string sTemplateId = string.Empty;
        string sTransactionId = string.Empty;
        string sJsonString = string.Empty;
        string sBaseUrl = string.Empty;
        string sFormat = string.Empty;
        string sMethod = string.Empty;
        string sAPI = string.Empty;

        string sAPIKey = string.Empty;
        Int32 nSMSTimeOut = 0;
        int iReceivedCredits = 0;
        int iSMSLimit = 0;
        int iSMSSent = 0;
        int iUsedCredit = 0;
        int iTotalSendCreditCount = 0;
        int nLogcount = 0;

        string sStartTime { get; set; }
        public string sCreditCount { get; set; }
        public string sSender { get; set; }
        public string sUser { get; set; }
        public string sMobilenos { get; set; }
        public string sAccusage { get; set; }
        string sResult = string.Empty;
        public string sSmsMode = string.Empty;
        public string sClassIds { get; set; }
        public string sReceipientNo { get; set; }
        public string sTextMessage { get; set; }
        public string sTemplateid { get; set; }
        public int sSmsCount = 0;
        public List<MessageResult.sms> liSMS = new List<MessageResult.sms>();
        public List<MessageResult.listsms> listbulkSMS = new List<MessageResult.listsms>();
        public List<MessageResult.sms> liActualSMS = new List<MessageResult.sms>();
        List<SMS_SETTING> liSMSSetting = new List<SMS_SETTING>();
        CommunicationViewModel objViewModel = new CommunicationViewModel();


        #region SMS
        [UserRights("Admin")]
        public ActionResult GeneralMessage()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    var Status = (List<SUP_STATUS>)CMSHandler.FetchData<SUP_STATUS>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupStatus), sAcademicYear).DataSource.Data;
                    var StaffCategoryList = (List<Sup_SubCategory>)CMSHandler.FetchData<Sup_SubCategory>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSubCategory), sAcademicYear).DataSource.Data;
                    var ProgrammeMode = (List<SupProgrammeMode>)CMSHandler.FetchData<SupProgrammeMode>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSupProgrammeMode), sAcademicYear).DataSource.Data;
                    if (ProgrammeMode != null && ProgrammeMode.Count > 0)
                    {
                        objViewModel.ProgrammeMode = new SelectList(ProgrammeMode, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE_ID, Common.SUP_PROGRAMME_MODE.PROGRAMME_MODE);
                    }
                    if (Status != null && Status.Count > 0)
                    {
                        objViewModel.Status = new SelectList(Status, Common.SUP_STATUS.STATUS_ID, Common.SUP_STATUS.STATUS);
                    }
                    if (StaffCategoryList != null && StaffCategoryList.Count > 0)
                    {
                        objViewModel.StaffCategoryList = new SelectList(StaffCategoryList, Common.sup_staff_subcategory.STF_CATEGORY_ID, Common.sup_staff_subcategory.STF_CATEGORY);
                    }

                    var liShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                    if (liShift != null && liShift.Count > 0)
                    {
                        objViewModel.Shift = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    }
                    var liApp_Type = (List<ADM_APPLICATION_TYPE>)CMSHandler.FetchData<ADM_APPLICATION_TYPE>(null, SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchApplicationType)).DataSource.Data;
                    if (liApp_Type != null && liApp_Type.Count > 0)
                    {
                        objViewModel.Application_Type = new SelectList(liApp_Type, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE_ID, Common.ADM_APPLICATION_TYPE.APPLICATION_TYPE);
                    }
                    return View(objViewModel);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("CommunicationController", "GeneralMessage", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("CommunicationController", "GeneralMessage", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return RedirectToAction("ErrorMessage", "Error");

        }
        // To send Test Message
        public JsonResult TestMessage(string JsonTestMessage, string sContent, string CreditCount)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var Result = serializer.Deserialize<MessageResult.ComposeMessageList>(JsonTestMessage);
                    // var Result = JsonConvert.DeserializeObject<TestSMS>(JsonTestMessage);
                    if (!string.IsNullOrEmpty(sContent) && Result != null)
                    {
                        sSmsMode = "4";
                        sTextMessage = sContent;
                        sCreditCount = CreditCount;
                        foreach (var item in Result.sms)
                        {
                            liSMS.Add(new MessageResult.sms() { to = item.to, custom = "0", custom1 = "0", custom2 = "0", CreditCount = sCreditCount });
                        }
                        SendMessage();
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("CommunicationController", "TestMessage", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("CommunicationController", "TestMessage", ex.Message);
                    }
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }

            }
            else
            {
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        // Choose Option For Send Message ...
        public void SendMessage()
        {

            try
            {
                var livendordetails = (List<SMS_VENDOR_DETAILS>)CMSHandler.FetchData<SMS_VENDOR_DETAILS>(null, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchActiveSMSvendors)).DataSource.Data;
                liSMSSetting = (List<SMS_SETTING>)CMSHandler.FetchData<SMS_SETTING>(new SMS_SETTING() { API_METHOD = Common.API_METHOD.API_2 }, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchDovesoftSettings)).DataSource.Data;
                if (liActualSMS != null && liActualSMS.Count > 0)
                {
                    foreach (var item in liActualSMS)
                    {
                        liSMS.Add(new MessageResult.sms() { to = item.to, message = item.message, custom = "0", custom1 = "0", custom2 = "0" });
                        if (liSMSSetting != null && liSMSSetting.Count > 0)
                        {
                            listbulkSMS.Add(new MessageResult.listsms() { mobiles = item.to, sms = item.message, senderid = liSMSSetting.FirstOrDefault().SENDER, clientSMSID = "1947692308", accountusagetypeid = "1" });
                        }
                    }
                    //---------------SMS------------              
                    if (livendordetails != null && livendordetails.Count > 0)
                    {
                        if (livendordetails.FirstOrDefault().SMS_VENDOR_ID == Common.sms_vendor.Solutioninfini)
                        {
                            sSmsMode = Common.MessageType.OTP;
                            int length = Convert.ToInt32(liSMS.FirstOrDefault().message.Length);

                            string sCount = CommonMethods.ValidateCreditCount(length);
                            if (sCount == "0")
                                sCount = "1";
                            sCreditCount = sCount;
                            sSmsCount = 1;
                            SendBulkSMSFromSolutionInfi();
                        }
                        if (livendordetails.FirstOrDefault().SMS_VENDOR_ID == Common.sms_vendor.DoveSoft)
                        {
                            SendSMSFromDovesoft();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("CommunicationController", "SendMessage", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("CommunicationController", "SendMessage", ex.Message);
                }
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
            }

        }
        private void SendSMSFromMvaayoo()
        {
        }
        public void SendBulkSMSFromDovesoft(string isTamil)
        {
            try
            {
                liSMSSetting = (List<SMS_SETTING>)CMSHandler.FetchData<SMS_SETTING>(new SMS_SETTING() { API_METHOD = Common.API_METHOD.API_1 }, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchDovesoftSettings)).DataSource.Data;
                if (liSMSSetting != null && liSMSSetting.Count > 0)
                {
                    sAPIKey = liSMSSetting.FirstOrDefault().API_KEY;
                    sBaseUrl = liSMSSetting.FirstOrDefault().BASE_URL;
                    sFormat = liSMSSetting.FirstOrDefault().FORMAT;
                    sMethod = liSMSSetting.FirstOrDefault().METHOD;
                    sSender = liSMSSetting.FirstOrDefault().SENDER;
                    sUser = liSMSSetting.FirstOrDefault().USER;
                    sAccusage = liSMSSetting.FirstOrDefault().ACCUSAGE;
                    string sbPostData = "";
                    if (isTamil == "1")
                    {
                        sbPostData = "user=" + sUser + "&key=" + sAPIKey + "&mobile=" + sMobilenos + "&message=" + sTextMessage + "&senderid=" + sSender + "&accusage=1&clientSMSID=1947692308&entityid=1201159497239380601&tempid=" + sTemplateid + "&unicode=1";

                    }
                    else
                    {
                        sbPostData = "user=" + sUser + "&key=" + sAPIKey + "&mobile=" + sMobilenos + "&message=" + sTextMessage + "&senderid=" + sSender + "&accusage=1&clientSMSID=1947692308&entityid=1201159497239380601&tempid=" + sTemplateid + "";

                    }
                    //Call Send SMS API
                    string sendSMSUri = sBaseUrl;
                    //Create HTTPWebrequest
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                    //Prepare and Add URL Encoded data
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] data = encoding.GetBytes(sbPostData.ToString());
                    //Specify post method
                    httpWReq.Method = sMethod;
                    httpWReq.ContentType = "application/x-www-form-urlencoded";
                    httpWReq.ContentLength = data.Length;
                    using (Stream stream = httpWReq.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    //Get the response
                    HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseString = reader.ReadToEnd();
                    var res = responseString.Split(',');
                    sResult = res[0];
                    //Close the response
                    reader.Close();
                    response.Close();

                }

            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("CommunicationController", "SendBulkSMSFromDovesoft", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("CommunicationController", "SendBulkSMSFromDovesoft", ex.Message);
                }
                ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
        }
        public void SendSMSFromDovesoft()
        {
            bool bFlag = true;
            int ncount = 0;
            sSmsCount = listbulkSMS.Count;
            var list = new List<MessageResult.listsms>();
            try
            {
                liSMSSetting = (List<SMS_SETTING>)CMSHandler.FetchData<SMS_SETTING>(new SMS_SETTING() { API_METHOD = Common.API_METHOD.API_2 }, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchDovesoftSettings)).DataSource.Data;
                if (liSMSSetting != null && liSMSSetting.Count > 0)
                {
                    sAPIKey = liSMSSetting.FirstOrDefault().API_KEY;
                    sBaseUrl = liSMSSetting.FirstOrDefault().BASE_URL;
                    sFormat = liSMSSetting.FirstOrDefault().FORMAT;
                    sMethod = liSMSSetting.FirstOrDefault().METHOD;
                    sSender = liSMSSetting.FirstOrDefault().SENDER;
                    sUser = liSMSSetting.FirstOrDefault().USER;
                    sAccusage = liSMSSetting.FirstOrDefault().ACCUSAGE;
                    int nSMSCount = Convert.ToInt32(ConfigurationManager.AppSettings["SMSCount"]);
                    while (bFlag)
                    {
                        // Checking Record Count ...
                        if (ncount < listbulkSMS.Count)
                        {
                            list = listbulkSMS.Skip(ncount).Take(nSMSCount).ToList();
                            ncount = ncount + list.Count;
                        }
                        else
                        {
                            bFlag = false;
                            list.Clear();
                        }

                        // Checking Record Count To Send Meessages ....
                        if (list != null && list.Count > 0)
                        {

                            // Assign Values To Send Bulk SMS ....
                            var composeClass = new MessageResult.SMSRequestCompose();
                            composeClass.listsms = listbulkSMS;
                            composeClass.user = sUser;
                            composeClass.password = sAPIKey;

                            // Convert Records To Json , Encoding ....
                            sJsonString = new JavaScriptSerializer().Serialize(composeClass);


                            //Call Send SMS API
                            string sendSMSUri = sBaseUrl;
                            //Create HTTPWebrequest
                            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                            //Prepare and Add URL Encoded data
                            UTF8Encoding encoding = new UTF8Encoding();
                            byte[] data = encoding.GetBytes(sJsonString);
                            //Specify post method
                            httpWReq.Method = sMethod;
                            httpWReq.ContentType = "application/json";
                            httpWReq.ContentLength = data.Length;
                            using (Stream stream = httpWReq.GetRequestStream())
                            {
                                stream.Write(data, 0, data.Length);
                            }
                            //Get the response
                            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                            StreamReader reader = new StreamReader(response.GetResponseStream());
                            string responseString = reader.ReadToEnd();
                            //Close the response
                            reader.Close();
                            response.Close();

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("CommunicationController", "SendBulkSMSFromDovesoft", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("CommunicationController", "SendBulkSMSFromDovesoft", ex.Message);
                }
                ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
        }

        public void SendBulkSMSFromSolutionInfi()
        {
            try
            {
                int ncount = 0;

                var list = new List<MessageResult.sms>();
                // Getting Timeout Setting from AppSetting  ...
                nSMSTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["SMSTimeOut"]);
                // Getting Current Time ....
                sStartTime = DateTime.Now.ToString(Common.Formats.TimeFormat);
                // Checking Count Of Record ...
                sSmsCount = liSMS.Count;
                // Validate SMS ....
                ValidationToSendSMS();
                if (sResult == Common.ErrorCode.Ok)
                {
                    var ObjSmsSetting = new SMS_SETTING();
                    ObjSmsSetting.API_MODE = "1";
                    liSMSSetting = (List<SMS_SETTING>)CMSHandler.FetchData<SMS_SETTING>(ObjSmsSetting, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchSolutionInfiApi)).DataSource.Data;
                    // SMS Setting Details ...
                    if (liSMSSetting != null && liSMSSetting.Count > 0)
                    {
                        sAPIKey = liSMSSetting.FirstOrDefault().API_KEY;
                        sBaseUrl = liSMSSetting.FirstOrDefault().BASE_URL;
                        sFormat = liSMSSetting.FirstOrDefault().FORMAT;
                        sMethod = liSMSSetting.FirstOrDefault().METHOD;
                        sSender = liSMSSetting.FirstOrDefault().SENDER;
                        // sAPI = string.Concat(Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.METHOD, Common.Delimiter.EQUAL, sMethod, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.FORMAT, Common.Delimiter.EQUAL, sFormat, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.JSON, Common.Delimiter.EQUAL).ToLower();
                        //  sAPI = string.Concat(sBaseUrl, sAPI);
                        bool bFlag = true;
                        int nSMSCount = Convert.ToInt32(ConfigurationManager.AppSettings["SMSCount"]);
                        while (bFlag)
                        {
                            // Checking Record Count ...
                            if (ncount < liSMS.Count)
                            {
                                list = liSMS.Skip(ncount).Take(nSMSCount).ToList();
                                ncount = ncount + list.Count;
                            }
                            else
                            {
                                bFlag = false;
                                list.Clear();
                            }

                            // Checking Record Count To Send Meessages ....
                            if (list != null && list.Count > 0)
                            {
                                // Assign Values To Send Bulk SMS ....
                                var composeClass = new MessageResult.ComposeMessageList();
                                composeClass.unicode = 0;
                                composeClass.flash = 0;
                                composeClass.message = (!string.IsNullOrEmpty(sTextMessage)) ? sTextMessage : string.Empty;
                                composeClass.sender = sSender;
                                composeClass.sms = liSMS;

                                // Convert Records To Json , Encoding ....
                                sJsonString = new JavaScriptSerializer().Serialize(composeClass);
                                // sJsonString = HttpUtility.UrlEncode(sJsonString);
                                // sAPI = string.Concat(sAPI, sJsonString);
                                sAPI = string.Concat(Common.SolutionInfiSMSSettingFields.API_KEY.ToLower(), Common.Delimiter.EQUAL, sAPIKey, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.METHOD.ToLower(), Common.Delimiter.EQUAL, sMethod, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.FORMAT.ToLower(), Common.Delimiter.EQUAL, sFormat, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.JSON.ToLower(), Common.Delimiter.EQUAL, sJsonString);
                                var data = Encoding.ASCII.GetBytes(sAPI);
                                // Sending Message Via Web API .....                             
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                                request.Method = "POST";
                                request.ContentType = "application/x-www-form-urlencoded";
                                request.ContentLength = data.Length;
                                //request.KeepAlive = false;
                                // request.Timeout = nSMSTimeOut;
                                //  request.ReadWriteTimeout = nSMSTimeOut;
                                using (var stream = request.GetRequestStream())
                                {
                                    stream.Write(data, 0, data.Length);
                                }
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                Stream s = (Stream)response.GetResponseStream();
                                StreamReader readStream = new StreamReader(s);
                                string dataString = readStream.ReadToEnd();
                                MessageResult.ResponseFromSolutionInfi responseClass = new MessageResult.ResponseFromSolutionInfi();
                                SaveSendSMSList(dataString);
                                responseClass = JsonConvert.DeserializeObject<MessageResult.ResponseFromSolutionInfi>(dataString);
                                response.Close();
                                s.Close();
                                readStream.Close();
                                nLogcount++;
                            }
                        }


                    }
                    else
                    {
                        ObjJsonData.ErrorCode = Common.ErrorCode.MethodNotAllowed;
                        ObjJsonData.Message = Common.Messages.PleaseConfigureMessageSetting;
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("CommunicationController", "SendSMSFromSolutionInfi", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("CommunicationController", "SendSMSFromSolutionInfi", ex.Message);
                }
                ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
        }
        // to validate the sms to send
        private void ValidationToSendSMS()
        {
            var liBalance = (List<SMSBalence>)CMSHandler.FetchData<SMSBalence>(null, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchSMSBalance)).DataSource.Data;
            if (liBalance != null && liBalance.Count > 0)
            {
                iReceivedCredits = Convert.ToInt32(liBalance.FirstOrDefault().RECEIVED_CREDITS);
                iSMSLimit = Convert.ToInt32(liBalance.FirstOrDefault().SMS_LIMIT);
                iUsedCredit = Convert.ToInt32(liBalance.FirstOrDefault().USED_CREDITS);
                iSMSSent = (string.IsNullOrEmpty(sCreditCount)) ? (1 * Convert.ToInt16(sSmsCount)) : (Convert.ToInt16(sCreditCount) * Convert.ToInt16(sSmsCount));
                iTotalSendCreditCount = iSMSSent + iUsedCredit;
                if (iSMSLimit >= iTotalSendCreditCount && iSMSLimit != 0)
                    sResult = Common.ErrorCode.Ok;
                else
                    ObjJsonData.Message = Common.Messages.LowMessageLimit;
            }
            else
                ObjJsonData.Message = Common.Messages.PleaseConfigureMessageSetting;
        }
        // to save sms list
        private void SaveSendSMSList(string JsonResponse)
        {
            AccountViewModel objAccount = new AccountViewModel();
            try
            {
                DataTable dtActiveYear = new DataTable();
                dtActiveYear = objAccount.FetchActiveYear().DataSource.Table;
                string sAcademicYear = dtActiveYear.Rows[0][Common.ACADEMIC_YEAR.AC_YEAR].ToString();

                if (!string.IsNullOrEmpty(sAcademicYear))
                {
                    // Convert Form Json To Model ....
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var ObjSMS = serializer.Deserialize<MessageResult.ComposeResponcesms>(JsonResponse);
                    // Checking SMS Count ....
                    if (ObjSMS.data != null && ObjSMS.data.Count > 0)
                    {
                        ObjJsonData.Message = ObjSMS.message;
                        var ObjUpdateBls = new UpdateSMSBalance();
                        ObjUpdateBls.BALANCE_CREDITS = Convert.ToString(iSMSLimit - iTotalSendCreditCount);
                        ObjUpdateBls.USED_CREDITS = Convert.ToString(iSMSSent + iUsedCredit);
                        ObjUpdateBls.IS_ACTIVE = Common.IsActiveFalg.IsActive;
                        var resultArgs = CMSHandler.SaveOrUpdate(ObjUpdateBls, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.UpdateSMSBalance));
                        //ObjJsonData.Message = ObjSMS.message;
                        //foreach (var item in ObjSMS.data)
                        //{
                        //    sClassIds += item.customid1 + ",";
                        //    sReceipientNo += item.mobile + ",";
                        //}
                        sClassIds = string.Join(",", ObjSMS.data.Select(O => O.customid1)).ToString();
                        sReceipientNo = string.Join(",", ObjSMS.data.Select(O => O.mobile)).ToString();
                        var ObjSaveRecord = new SENT_SMS_2017();
                        ObjSaveRecord.CLASS_IDS = sClassIds;
                        ObjSaveRecord.START_TIME = sStartTime;
                        ObjSaveRecord.END_TIME = DateTime.Now.ToString(Common.Formats.TimeFormat);
                        if (sSmsMode == Common.MessageType.ATTENDANCE)
                        {
                            ObjSaveRecord.CONTENT = "Absentees List";
                            sCreditCount = "1";
                        }
                        else
                        {
                            ObjSaveRecord.CONTENT = sTextMessage;
                        }
                        ObjSaveRecord.SMS_COUNT = Convert.ToString(sSmsCount);
                        ObjSaveRecord.STATUS = ObjSMS.data.FirstOrDefault().status;
                        ObjSaveRecord.TRANSACTION_ID = ObjSMS.data.FirstOrDefault().id;
                        ObjSaveRecord.CREDIT_COUNT = sCreditCount;
                        ObjSaveRecord.SMS_MODE = sSmsMode;
                        ObjSaveRecord.RECEIPIENT_NOS = sReceipientNo;
                        ObjSaveRecord.ACADEMIC_YEAR = sAcademicYear;
                        ObjSaveRecord.USER_TYPE = ObjSMS.data.FirstOrDefault().customid2;
                        string lstInsertedId = string.Empty;
                        if (nLogcount == 0)
                        {
                            var sresultArgs = CMSHandler.SaveOrUpdate(ObjSaveRecord, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.SaveSentMessages), sAcademicYear, true);
                            lstInsertedId = (sresultArgs.Success) ? sresultArgs.RowUniqueId.ToString() : string.Empty;
                        }

                        string sTempMessage = string.Empty;
                        if (!string.IsNullOrEmpty(lstInsertedId))
                        {
                            string sSQL = string.Empty;
                            sSQL = @"INSERT INTO SENT_SMS_LIST_?AC_YEAR (" + Common.SENT_SMS_LIST.SENT_ITEM_ID + "," + Common.SENT_SMS_LIST.RECEIPIENT_ID + ","
                                   + Common.SENT_SMS_LIST.CLASS_ID + "," + Common.SENT_SMS_LIST.CONTENT + "," + Common.SENT_SMS_LIST.IS_STAFF + ","
                                   + Common.SENT_SMS_LIST.MOBILE_NO + "," + Common.SENT_SMS_LIST.TRANSACTION_ID + ","
                                   + Common.SENT_SMS_LIST.CREDIT_COUNT + ")VALUES";
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                            foreach (var item in ObjSMS.data)
                            {
                                if (liSMS != null && liSMS.Count > 0)
                                {
                                    var liSave = liSMS.Where(s => s.to == item.mobile).ToList();
                                    if (liSave != null && liSave.Count > 0)
                                    {
                                        foreach (var lstsave in liSave)
                                        {
                                            sTempMessage = (!string.IsNullOrEmpty(sTextMessage)) ? sTextMessage : lstsave.message;
                                            sSQL += @"('" + lstInsertedId + "','" + lstsave.custom + "','" + lstsave.custom1 + "',\"" + sTempMessage + "\",'" + lstsave.custom2 + "','" + lstsave.to + "','" + item.id + "','" + ObjSaveRecord.CREDIT_COUNT + "'),";
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(item.customid))
                                            item.customid = "0";
                                        sSQL += @"('" + lstInsertedId + "','" + item.customid + "','" + item.customid1 + "',\"" + sTextMessage + "\",'" + item.customid2 + "','" + item.mobile + "','" + item.id + "','" + ObjSaveRecord.CREDIT_COUNT + "'),";
                                    }
                                }
                                else
                                {
                                    ObjJsonData.Message = Common.Messages.NoMobileNumbersAvailable;
                                }

                            }
                            sSQL = sSQL.TrimEnd(',');
                            using (MySQLHandler ObjHandler = new MySQLHandler())
                            {
                                var resultArg = ObjHandler.ExecuteAsScripts(sSQL);
                            }
                        }
                    }
                    else
                    {
                        ObjJsonData.Message = Common.Messages.FailedToSendMessage;
                    }
                }
                else
                {
                    ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
                    ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("CommunicationController", "SaveSendSMSList", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("CommunicationController", "SaveSendSMSList", ex.Message);
                }
                ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
        }
        public ActionResult BindClassWiseInfoForIndividual(string sClassIds)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sClassIds))
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR]).ToString() : string.Empty;
                    string sSql = string.Empty;
                    try
                    {
                        sSql = CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchStudentInfoForIndividualSMS).Replace(Common.Delimiter.QUS + Common.STU_CLASS.CLASS_ID, sClassIds);
                        objViewModel.LIStudentDetails = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(null, sSql, sAcademicYear).DataSource.Data;
                        return View(objViewModel);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("CommunicationController", "BindClassWiseInfoForIndividual", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("CommunicationController", "BindClassWiseInfoForIndividual", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult BindDepartmentWiseInfoForIndividual(string sDepartmentIds)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sClassIds))
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR]).ToString() : string.Empty;
                    string sSql = string.Empty;
                    try
                    {
                        sSql = CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchStudentInfoForIndividualSMS).Replace(Common.Delimiter.QUS + Common.STU_CLASS.CLASS_ID, sClassIds);
                        objViewModel.LIStudentDetails = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(null, sSql).DataSource.Data;
                        return View(objViewModel);
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("CommunicationController", "BindClassWiseInfoForIndividual", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("CommunicationController", "BindClassWiseInfoForIndividual", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult FetchClasswiseRecordsCount(string sClassIds)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sClassIds))
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR]).ToString() : string.Empty;
                    string sSql = string.Empty;
                    try
                    {
                        sSql = CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchClasswiseRecordsCount).Replace(Common.Delimiter.QUS + Common.STU_CLASS.CLASS_ID, sClassIds);
                        var LiStudentMoblileCount = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(null, sSql, sAcademicYear).DataSource.Data;
                        if (LiStudentMoblileCount != null && LiStudentMoblileCount.Count > 0)
                        {
                            ObjJsonData.sResult = LiStudentMoblileCount.SingleOrDefault().RECORDSCOUNT;
                            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("CommunicationController", "FetchClasswiseRecordsCount", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("CommunicationController", "FetchClasswiseRecordsCount", ex.Message);
                        }
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    }
                }
                else
                {
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                }
            }
            else
            {
                ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindStaffInfoForIndividual(string sStaffCategoryId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(sStaffCategoryId))
                    {
                        Sup_SubCategory objModel = new Sup_SubCategory();
                        objModel.STF_CATEGORY_ID = sStaffCategoryId;
                        objViewModel.LIStaffDetails = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(objModel, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchStaffInfoForIndividualSMS)).DataSource.Data;
                        return View(objViewModel);
                    }
                    else
                    {
                        ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                        ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("CommunicationController", "BindStaffInfoForIndividual", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("CommunicationController", "BindStaffInfoForIndividual", ex.Message);
                    }
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ObjJsonData.Message = Common.ErrorMessage.RequestTimeout;
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SendBulkMessageForSMS(string JsonMessage, string sSMSMode, string sContent, string ChStaff, string sCredit, string sStfCategoryId)
        {
            string sSql = string.Empty;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    sSmsMode = sSMSMode;
                    sCreditCount = sCredit;
                    sTextMessage = sContent;
                    var Result = JsonConvert.DeserializeObject<SubmissionAndPaidInfo>(JsonMessage);
                    string sSQL = string.Empty;
                    Result.ACADEMIC_YEAR = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    if (ChStaff.Equals("0"))
                    {
                        var liApplicant = new List<ADM_ISSUE_APPLICATION_2018>();
                        if (Result.STATUS == Common.STATUS.Registered || Result.STATUS == Common.STATUS.Submitted)
                        {
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchRegisteredAndSubmittedStudentInfoWithoutSelectedTable);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAMME_GROUP_ID);
                            liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(Result, sSQL, sAcademicYear).DataSource.Data;
                        }
                        else
                        {
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentInfoWithoutRegisteredAndSubmittedByStatus);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAMME_GROUP_ID);
                            liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(Result, sSQL, sAcademicYear).DataSource.Data;
                        }
                        if (liApplicant != null && liApplicant.Count > 0)
                        {
                            foreach (var item in liApplicant)
                            {
                                liActualSMS.Add(new MessageResult.sms() { to = item.SMS_MOBILE_NO, message = sTextMessage });
                                //liSMS.Add(new MessageResult.sms() { to = item.SMS_MOBILE_NO, custom = item.ISSUED_ID, custom1 =item.PROGRAMME_GROUP_ID, custom2 = "1", CreditCount = sCreditCount });
                            }
                        }
                        else
                            ObjJsonData.Message = Common.Messages.NoMobileNumbersAvailable;

                        SendMessage();
                    }
                    else if (ChStaff.Equals("1"))
                    {
                        sSQL = CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchStaffMobileNumbers).Replace(Common.Delimiter.QUS + Common.STF_PERSONAL_INFO.STF_CATEGORY_ID, sStfCategoryId);
                        var liStaff = (List<FetchClassStaffNumber>)CMSHandler.FetchData<FetchClassStaffNumber>(null, sSQL).DataSource.Data;
                        if (liStaff != null && liStaff.Count > 0)
                        {
                            foreach (var item in liStaff)
                            {
                                liActualSMS.Add(new MessageResult.sms() { to = item.MOBILE, message = sTextMessage });
                                //liSMS.Add(new MessageResult.sms() { to = item.MOBILE, custom = item.RECEIPIENT_ID, custom1 = "0", custom2 = "2", CreditCount = sCreditCount });
                            }
                            SendMessage();
                        }
                        else
                            ObjJsonData.Message = Common.Messages.NoMobileNumbersAvailable;

                    }
                    //else
                    //{
                    //    var liAllMobileNumber = (List<FetchAllMobileNumber>)CMSHandler.FetchData<FetchAllMobileNumber>(null, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchStaffAndStudentMobileNumbers)).DataSource.Data;
                    //    if (liAllMobileNumber != null && liAllMobileNumber.Count > 0)
                    //    {
                    //        foreach (var item in liAllMobileNumber)
                    //        {
                    //            liSMS.Add(new MessageResult.sms() { to = item.MOBILE, custom = item.RECEIPIENT_ID, custom1 = item.CLASS_ID, custom2 = item.IS_STAFF, CreditCount = sCreditCount });
                    //        }
                    //       // SendMessage();
                    //    }
                    //    else
                    //        ObjJsonData.Message = Common.Messages.NoMobileNumbersAvailable;
                    //}
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("CommunicationController", "SendBulkMessage", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("CommunicationController", "SendBulkMessage", ex.Message);
                    }
                    ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                    ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                }
            }
            else
            {
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchStaffRecordsCount(string sStaffCategoryId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(sStaffCategoryId))
                    {
                        string sSQL = CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchStaffRecordsCount).Replace(Common.Delimiter.QUS + Common.STF_PERSONAL_INFO.STF_CATEGORY_ID, sStaffCategoryId);
                        var LiStaffMoblileCount = (List<stf_Personal_Info>)CMSHandler.FetchData<stf_Personal_Info>(null, sSQL).DataSource.Data;
                        if (LiStaffMoblileCount != null && LiStaffMoblileCount.Count > 0)
                            ObjJsonData.sResult = LiStaffMoblileCount.SingleOrDefault().RECORDSCOUNT;
                    }
                    else
                    {
                        ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                        ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    }
                    return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("CommunicationController", "FetchStaffRecordsCount", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("CommunicationController", "FetchStaffRecordsCount", ex.Message);
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
        public JsonResult FetchStaffAndStudentRecordsCount()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR]).ToString() : string.Empty;
                try
                {
                    var BothCount = (List<Student_Personal_Info>)CMSHandler.FetchData<Student_Personal_Info>(null, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchStaffAndStudentRecordsCount), sAcademicYear).DataSource.Data;
                    if (BothCount != null && BothCount.Count > 0)
                    {
                        ObjJsonData.sResult = BothCount.SingleOrDefault().RECORDSCOUNT;
                        return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("CommunicationController", "FetchStaffAndStudentRecordsCount", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("CommunicationController", "FetchStaffAndStudentRecordsCount", ex.Message);
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
        [UserRights("Admin")]
        public ActionResult SentMessage()
        {
            SENT_SMS_2017 liSentMessage = new SENT_SMS_2017();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR]).ToString() : string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                IList<SENT_SMS_LIST_2017> liSentMessages = new List<SENT_SMS_LIST_2017>();
                liSentMessage.ACADEMIC_YEAR = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                objViewModel.LISentMessage = (List<SENT_SMS_2017>)CMSHandler.FetchData<SENT_SMS_2017>(liSentMessage, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchSentMessages), sAcademicYear).DataSource.Data;
            }
            else
                return RedirectToAction("ErrorMessage", "Error");
            return View(objViewModel);
        }

        public ActionResult SentMessageList(string SentSMSId, string sSmsMode)
        {
            int rcount = 0;
            SENT_SMS_LIST_2017 objSentsmsList = new SENT_SMS_LIST_2017();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR]).ToString() : string.Empty;
            var ListSentMessages = new List<FetchSentItemIds>();
            objSentsmsList.SENT_ITEM_ID = SentSMSId;
            objSentsmsList.SMS_MODE = sSmsMode;
            var checkStaff = (List<FetchSentItemIds>)CMSHandler.FetchData<FetchSentItemIds>(objSentsmsList, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchSentListBySentId), sAcademicYear).DataSource.Data;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                ListSentMessages = (List<FetchSentItemIds>)CMSHandler.FetchData<FetchSentItemIds>(objSentsmsList, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchTransactionIdBySentId), sAcademicYear).DataSource.Data;

                if (ListSentMessages != null && ListSentMessages.Count > 0)
                {
                    if (ListSentMessages.Count > Convert.ToInt32(ConfigurationManager.AppSettings["SMSResponseCount"]))
                    {
                        if (rcount < ListSentMessages.Count)
                        {
                            while (rcount != ListSentMessages.Count)
                            {
                                var liResponse = ListSentMessages.Skip(rcount).Take(Convert.ToInt32(ConfigurationManager.AppSettings["SMSResponseCount"])).ToList();
                                sTransactionId = string.Join(",", liResponse.Select(O => O.TRANSACTION_ID)).ToString();
                                GetRequestByTransactionId();
                                rcount = rcount + liResponse.Count;
                                liResponse.Clear();
                                sTransactionId = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        sTransactionId = string.Join(",", ListSentMessages.Select(O => O.TRANSACTION_ID)).ToString();
                        GetRequestByTransactionId();
                    }
                }
                if (objSentsmsList.SMS_MODE == Common.MessageType.COMMON_ANNOUNCEMENT)
                {
                    if (checkStaff.FirstOrDefault().IS_STAFF == "1")
                    {
                        objViewModel.LISentMessageList = (List<SENT_SMS_LIST_2017>)CMSHandler.FetchData<SENT_SMS_LIST_2017>(objSentsmsList, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchStudentSentItemForViewMode), sAcademicYear).DataSource.Data;
                    }
                    else if (checkStaff.FirstOrDefault().IS_STAFF == "2")
                    {
                        objViewModel.LISentMessageList = (List<SENT_SMS_LIST_2017>)CMSHandler.FetchData<SENT_SMS_LIST_2017>(objSentsmsList, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchStaffSentItemForViewMode), sAcademicYear).DataSource.Data;
                    }
                }
                else
                {
                    objViewModel.LISentMessageList = (List<SENT_SMS_LIST_2017>)CMSHandler.FetchData<SENT_SMS_LIST_2017>(objSentsmsList, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchOthersSentItemForViewMode), sAcademicYear).DataSource.Data;
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
            return View(objViewModel);
        }
        // Getting Request By TransactionId ...
        private void GetRequestByTransactionId()
        {
            try
            {
                var ObjSmsSetting = new SMS_SETTING();
                ObjSmsSetting.API_MODE = "1";
                liSMSSetting = (List<SMS_SETTING>)CMSHandler.FetchData<SMS_SETTING>(ObjSmsSetting, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchSolutionInfiApi)).DataSource.Data;
                // SMS Setting Details ...
                if (liSMSSetting != null && liSMSSetting.Count > 0)
                {
                    sBaseUrl = liSMSSetting.FirstOrDefault().BASE_URL;
                    sFormat = liSMSSetting.FirstOrDefault().FORMAT;
                    sMethod = liSMSSetting.FirstOrDefault().REQUEST_METHOD;
                    sSender = liSMSSetting.FirstOrDefault().SENDER;
                    sAPIKey = liSMSSetting.FirstOrDefault().API_KEY;
                    sAPI = string.Concat(Common.SolutionInfiSMSSettingFields.API_KEY, Common.Delimiter.EQUAL, sAPIKey, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.METHOD, Common.Delimiter.EQUAL, sMethod, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.ID, Common.Delimiter.EQUAL, sTransactionId).ToLower();
                    var data = Encoding.ASCII.GetBytes(sAPI);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;
                    //request.KeepAlive = false;
                    //request.Timeout = nSMSTimeOut;
                    //request.ReadWriteTimeout = nSMSTimeOut;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream s = (Stream)response.GetResponseStream();
                    StreamReader readStream = new StreamReader(s);
                    string dataString = readStream.ReadToEnd();
                    MessageResult.ResponseFromSolutionInfi responseClass = new MessageResult.ResponseFromSolutionInfi();
                    UpdateReplyStatus(dataString);
                    responseClass = JsonConvert.DeserializeObject<MessageResult.ResponseFromSolutionInfi>(dataString);
                    response.Close();
                    s.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("CommunicationController", "GetRequestByTransactionId", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("CommunicationController", "GetRequestByTransactionId", ex.Message);
                }
                ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
        }
        // Update Delivery Report ...
        private ResultArgs UpdateReplyStatus(string JsonResponse)
        {
            try
            {
                // Convert Form Json To Model ....
                string IsUpdated = string.Empty;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var ObjSMS = serializer.Deserialize<MessageResult.ComposeResponcesms>(JsonResponse);
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR]).ToString() : string.Empty;
                if (ObjSMS.data != null && ObjSMS.data.Count > 0)
                {
                    string sSQLscript = string.Empty;
                    sSQLscript = @"SET SQL_SAFE_UPDATES=0;";
                    if (ObjSMS.data != null && ObjSMS.data.Count > 0)
                    {
                        foreach (var item in ObjSMS.data)
                        {
                            var sTime = (!string.IsNullOrEmpty(item.senttime)) ? item.senttime.Split(' ')[1] : string.Empty;
                            var dTime = (!string.IsNullOrEmpty(item.dlrtime)) ? item.dlrtime.Split(' ')[1] : string.Empty;
                            var sDate = (!string.IsNullOrEmpty(item.senttime)) ? item.senttime : "0000:00:00";
                            var dDate = (!string.IsNullOrEmpty(item.dlrtime)) ? item.dlrtime : "00000:00:00";
                            if (item.status != "DELIVRD")
                                IsUpdated = "0";
                            else
                                IsUpdated = "1";
                            sSQLscript += @" UPDATE SENT_SMS_LIST_?AC_YEAR SET COMPOSED_TIME='" + sTime + "',SENT_TIME='" + dTime + "',STATUS ='" + item.status + "',DELIVERED_DATE='" + dDate + "',SENT_DATE='" + sDate + "',IS_UPDATED='" + IsUpdated + "'  WHERE TRANSACTION_ID ='" + item.id + "'; ";
                            //sSQLscript += @" UPDATE SENT_SMS_LIST_?AC_YEAR SET `COMPOSED_TIME`='" + item.senttime.Split(' ')[1] + "',`SENT_TIME`='" + item.dlrtime.Split(' ')[1] + "',`STATUS` ='" + item.status + "',`DELIVERED_DATE`='" + item.dlrtime.Split(' ')[0] + "',`SENT_DATE`='" + item.senttime.Split(' ')[0] + "'  WHERE TRANSACTION_ID ='" + item.id + "'; ";
                        }
                        sSQLscript = sSQLscript.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                    }
                    sSQLscript += @"SET SQL_SAFE_UPDATES=1;";

                    using (MySQLHandler ObjHandler = new MySQLHandler())
                    {
                        var resultArgs = ObjHandler.ExecuteAsScripts(sSQLscript);
                    }
                }

            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("CommunicationController", "UpdateReplyStatus", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("CommunicationController", "UpdateReplyStatus", ex.Message);
                }
                ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
            }
            return resultArgs;
        }
        public JsonResult Resend(string SentListItemId)
        {
            var ObjSentItem = new SENT_SMS_LIST_2017();
            ObjSentItem.SENT_SMS_LIST_ID = SentListItemId;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR]).ToString() : string.Empty;
            var liResend = (List<SENT_SMS_LIST_2017>)CMSHandler.FetchData<SENT_SMS_LIST_2017>(ObjSentItem, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchSenderInfoForResend), sAcademicYear).DataSource.Data;
            if (liResend != null && liResend.Count > 0)
            {
                sSmsMode = Common.MessageType.RESENT;
                foreach (var item in liResend)
                {
                    int count = Convert.ToInt32(item.CONTENT.Length) / 153;
                    sCreditCount = Convert.ToString(count);
                    if (sCreditCount == "0")
                        sCreditCount = "1";
                    liSMS.Add(new MessageResult.sms() { to = item.MOBILE_NO, message = item.CONTENT, custom = item.RECEIPIENT_ID, custom1 = item.CLASS, custom2 = item.IS_STAFF });
                }
                SendMessage();
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }

        // Fetch Submitted And Paid List
        public JsonResult FetchSubmissionAndPaidInfo(string sJsonSubmissionList)
        {
            string sSQL = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                var Result = JsonConvert.DeserializeObject<SubmissionAndPaidInfo>(sJsonSubmissionList);
                if (!string.IsNullOrEmpty(Result.APPLICATION_TYPE_ID) && !string.IsNullOrEmpty(Result.PROGRAM_ID) && !string.IsNullOrEmpty(Result.SHIFT))
                {
                    try
                    {
                        var liApplicant = new List<ADM_ISSUE_APPLICATION_2018>();
                        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        if (Result.STATUS == Common.STATUS.Registered || Result.STATUS == Common.STATUS.Submitted)
                        {
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOverallApplicantListSubmittedAndPaid);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAM_ID);
                            liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(Result, sSQL, sAcademicYear).DataSource.Data;
                        }
                        else
                        {
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOverallApplicantListSubmittedAndPaid);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAM_ID);
                            liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(Result, sSQL, sAcademicYear).DataSource.Data;
                        }

                        //if (Result.IS_SUBMITTED == Common.IsActiveFalg.IsActive && Result.IS_FEE_PAID == Common.IsActiveFalg.IsActive)
                        //{
                        //    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOverallApplicantListSubmittedAndPaid);
                        //    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAM_ID);
                        //    liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(Result, sSQL, sAcademicYear).DataSource.Data;
                        //}
                        //if (Result.IS_SUBMITTED == Common.IsActiveFalg.IsActive && Result.IS_FEE_PAID == Common.IsActiveFalg.IsNotActive)
                        //{
                        //    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOverallApplicantListSubmittedAndUnPaid);
                        //    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAM_ID);
                        //    liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(Result, sSQL, sAcademicYear).DataSource.Data;
                        //}
                        //if (Result.IS_SUBMITTED == Common.IsActiveFalg.IsNotActive && Result.IS_FEE_PAID == Common.IsActiveFalg.IsNotActive)
                        //{
                        //    Result.IS_SUBMITTED = Common.IsActiveFalg.IsActive;
                        //    sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchOverallApplicantListNotSubmittedAndUnPaid);
                        //    sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAM_ID);
                        //    liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(Result, sSQL, sAcademicYear).DataSource.Data;
                        //}
                        if (liApplicant != null && liApplicant.Count > 0)
                            ObjJsonData.sResult = Convert.ToString(liApplicant.Count);
                        else
                            ObjJsonData.sResult = Common.IsActiveFalg.IsNotActive;
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("CommunicationController", "FetchSubmissionAndPaidInfo", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("CommunicationController", "FetchSubmissionAndPaidInfo", ex.Message);
                        }
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    }
                }
                else
                {
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                }
            }
            else
            {
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FetchStudentInfoCountForRecordCount(string sJsonSubmissionList)
        {
            string sSQL = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                var Result = JsonConvert.DeserializeObject<SubmissionAndPaidInfo>(sJsonSubmissionList);
                if (!string.IsNullOrEmpty(Result.PROGRAMME_GROUP_ID) && !string.IsNullOrEmpty(Result.STATUS))
                {
                    try
                    {
                        var liApplicant = new List<ADM_ISSUE_APPLICATION_2018>();
                        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        if (Result.STATUS == Common.STATUS.Registered || Result.STATUS == Common.STATUS.Submitted)
                        {
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchRegisteredAndSubmittedStudentInfoWithoutSelectedTable);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAMME_GROUP_ID);
                            liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(Result, sSQL, sAcademicYear).DataSource.Data;
                        }
                        else
                        {
                            sSQL = SQL.Admission.AdmissionSQL.GetAdmissionSQL(AdmissionSQLCommands.FetchStudentInfoWithoutRegisteredAndSubmittedByStatus);
                            sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ADM_ISSUE_APPLICATION_2018.PROGRAMME_GROUP_ID, Result.PROGRAMME_GROUP_ID);
                            liApplicant = (List<ADM_ISSUE_APPLICATION_2018>)CMSHandler.FetchData<ADM_ISSUE_APPLICATION_2018>(Result, sSQL, sAcademicYear).DataSource.Data;
                        }
                        if (liApplicant != null && liApplicant.Count > 0)
                            ObjJsonData.sResult = Convert.ToString(liApplicant.Count);
                        else
                            ObjJsonData.sResult = Common.IsActiveFalg.IsNotActive;
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("CommunicationController", "FetchSubmissionAndPaidInfo", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("CommunicationController", "FetchSubmissionAndPaidInfo", ex.Message);
                        }
                        ObjJsonData.ErrorCode = Common.ErrorCode.InternalServerError;
                        ObjJsonData.Message = Common.ErrorMessage.InternalServerError;
                    }
                }
                else
                {
                    ObjJsonData.ErrorCode = Common.ErrorCode.BadRequest;
                    ObjJsonData.Message = Common.ErrorMessage.BadRequest;
                }
            }
            else
            {
                ObjJsonData.ErrorCode = Common.ErrorCode.RequestTimeout;
                ObjJsonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
            }
            return Json(ObjJsonData, JsonRequestBehavior.AllowGet);
        }

        public string sendBulkMessage(string sBaseUrl, string sParameter)
        {

            string sResult = string.Empty;
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
                SaveSendSMSList(responseString);
            }
            return sResult;
        }
        #endregion
    }
}