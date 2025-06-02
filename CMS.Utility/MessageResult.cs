using System.Collections.Generic;
using System.Data;

namespace CMS.Utility
{

    public class Common_SMS_SETTING
    {
        public string SMS_SETTING_ID { get; set; }
        public string BASE_URL { get; set; }
        public string API_KEY { get; set; }
        public string METHOD { get; set; }
        public string FORMAT { get; set; }
        public string UNICODE { get; set; }
        public string FLASH { get; set; }
        public string DLRURL { get; set; }
        public string IS_ACTIVE { get; set; }
        public string IS_DELETED { get; set; }
        public string ACADEMIC_YEAR { get; set; }
        public string API_MODE { get; set; }
        public string SENDER { get; set; }
        public string REQUEST_METHOD { get; set; }

    }

    public class MessageResult
    {



        public class ResponseFromSolutionInfi
        {
            public string status { get; set; }
            public DataTable data { get; set; }
            public string message { get; set; }
        }
        public class sms
        {
            public string to { get; set; }
            public string msgid { get; set; }
            public string message { get; set; }
            public string CreditCount { get; set; }
            public string custom { get; set; }
            public string custom1 { get; set; }
            public string custom2 { get; set; }
            public string sender { get; set; }
        }
        public class ComposeMessageList
        {
            public string message { get; set; }
            public List<sms> sms { get; set; }
            public int unicode { get; set; }
            public string sender { get; set; }
            public int flash { get; set; }
            public string dlrurl { get; set; }

        }

        public class listsms
        {
            public string sms { get; set; }
            public string mobiles { get; set; }
            public string senderid { get; set; }
            public string clientSMSID { get; set; }
            public string accountusagetypeid { get; set; }
            public string tempid { get; set; }

        }

        public class SMSRequestCompose
        {
            public List<listsms> listsms { get; set; }
            public string password { get; set; }
            public string user { get; set; }
        }

        public class Responcesms
        {
            public string id { get; set; }
            public string mobile { get; set; }
            public string senttime { get; set; }
            public string dlrtime { get; set; }
            public string customid { get; set; }
            public string customid1 { get; set; }
            public string customid2 { get; set; }
            public string status { get; set; }
            public string sender { get; set; }
            public string provider { get; set; }
            public string location { get; set; }
        }
        public class ComposeResponcesms
        {
            public string status { get; set; }
            public List<Responcesms> data { get; set; }
            public string message { get; set; }
        }
    }
}
