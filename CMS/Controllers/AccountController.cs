using CMS.DAO;
using CMS.SQL.Account;
using CMS.SQL.Communication;
using CMS.SQL.SupportData;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace CMS.Controllers
{
    public class AccountController : Controller
    {
        JsonResultData ObjJasonData = new JsonResultData();
        // GET: Account
        public ActionResult Index()
        {
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
            }
            return View();
        }

        // GET: Account/Details/5

        public ActionResult SignOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(ViewModel.ViewModel.AccountViewModel collection)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                // TODO: Add insert logic here

                using (AccountViewModel objAccount = new AccountViewModel())
                {
                    ResultArgs resultArgs = new ResultArgs();
                    DataValue dv = new DataValue();
                    string sUserRoles = string.Empty;
                    string sUserRoleId = string.Empty;
                    dv.Clear();
                    dv.Add(Common.USER_ACCOUNT.USERNAME, collection.objLoginViewModel.Username, EnumCommand.DataType.String);
                    string sPassword = CommonMethods.GetSha256FromString(collection.objLoginViewModel.Password);
                    dv.Add(Common.USER_ACCOUNT.PASSWORD, sPassword, EnumCommand.DataType.String);

                    resultArgs = await Task.Run(() => objAccount.FetchUserCredential(dv));
                    //Task<DataTable> getData =  objAccount.FetchUserCredential(dv).DataSource.Table;
                    // await TestAsync();
                    if (resultArgs.Success)
                    {
                        if (resultArgs.DataSource.Table != null && resultArgs.DataSource.Table.Rows.Count > 0)
                        {

                            FormsAuthentication.SetAuthCookie(resultArgs.DataSource.Table.Rows[0][Common.USER_ACCOUNT.USER_ID].ToString(), false);
                            Session[Common.SESSION_VARIABLES.COLLEGE_INFO] = objAccount.FetchCollegeInfo().DataSource.Table;
                            //new AuthorizeAttribute().Roles = "SuperAdmin";
                            var activeSemesterType = await Task.Run(() => (List<SUP_SEMESTER_TYPE>)DAO.CMSHandler.FetchData<SUP_SEMESTER_TYPE>(null, SupportDataSQL.GetSupportDataSQL(DAO.SupportDataSQLCommands.GetActiveSemesterType)).DataSource.Data);
                            if (activeSemesterType != null && activeSemesterType.Count() > 0)
                            {
                                Session[Common.SESSION_VARIABLES.ACTIVE_SEMESTER_TYPE] = activeSemesterType.FirstOrDefault().SEMESTER_TYPE_ID;
                            }
                            DataTable dtActiveYear = new DataTable();
                            dtActiveYear = await Task.Run(() => objAccount.FetchActiveYear().DataSource.Table);
                            Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] = dtActiveYear.Rows[0][Common.ACADEMIC_YEAR.AC_YEAR].ToString();
                            Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_NAME] = dtActiveYear.Rows[0][Common.ACADEMIC_YEAR.ACADEMIC_NAME].ToString();
                            Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR_ID] = dtActiveYear.Rows[0][Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID].ToString();

                            // need to store semester id 
                            Session[Common.SESSION_VARIABLES.USER_ID] = resultArgs.DataSource.Table.Rows[0][Common.USER_ACCOUNT.USER_ID];
                            Session[Common.SESSION_VARIABLES.USERNAME] = resultArgs.DataSource.Table.Rows[0][Common.USER_ACCOUNT.NAME];
                            Session[Common.SESSION_VARIABLES.USER_CODE] = resultArgs.DataSource.Table.Rows[0][Common.USER_ACCOUNT.USERNAME];
                            Session[Common.SESSION_VARIABLES.USER_ROLE_NAME] = resultArgs.DataSource.Table.Rows[0][Common.USER_ROLES_RIGHTS.ROLE_NAME];
                            Session[Common.SESSION_VARIABLES.USER_ROLE_ID] = resultArgs.DataSource.Table.Rows[0][Common.USER_ACCOUNT.ROLE];
                            Session[Common.SESSION_VARIABLES.PHOTO] = resultArgs.DataSource.Table.Rows[0][Common.USER_ACCOUNT.PHOTO];
                            Session[Common.SESSION_VARIABLES.CONTROLLER_NAME] = resultArgs.DataSource.Table.Rows[0][Common.USER_ACCOUNT.CONTROLLER_NAME];
                            Session[Common.SESSION_VARIABLES.ACTION_NAME] = resultArgs.DataSource.Table.Rows[0][Common.USER_ACCOUNT.ACTION_NAME];
                            sUserRoles = (Session[Common.SESSION_VARIABLES.USER_ROLE_NAME] != null) ? Session[Common.SESSION_VARIABLES.USER_ROLE_NAME].ToString() : string.Empty;
                            sUserRoles = sUserRoles.Replace(" ", string.Empty);
                            //var currenrDayOrder = (List<CALENDER_2017>)CMS.DAO.CMSHandler.FetchData<CALENDER_2017>(null, CMS.SQL.SupportData.SupportDataSQL.GetSupportDataSQL(DAO.SupportDataSQLCommands.FetchDayOrderInfoByCurrentDate), dtActiveYear.Rows[0][Common.ACADEMIC_YEAR.AC_YEAR].ToString()).DataSource.Data;
                            //var activeSemesterForStaff = (List<SemesterRootInfo>)CMS.DAO.CMSHandler.FetchData<SemesterRootInfo>(null, CMS.SQL.SupportData.SupportDataSQL.GetSupportDataSQL(DAO.SupportDataSQLCommands.FetchActiveSemesterForStaffByAcademicYear), dtActiveYear.Rows[0][Common.ACADEMIC_YEAR.AC_YEAR].ToString()).DataSource.Data;
                            //if (activeSemesterForStaff != null)
                            //Session[Common.SESSION_VARIABLES.ACTIVE_SEMESTER_FOR_STAFF] = activeSemesterForStaff.FirstOrDefault().SEMESTER;

                            //if (currenrDayOrder != null)
                            // {
                            //    Session[Common.SESSION_VARIABLES.DAY_ORDER] = (currenrDayOrder.FirstOrDefault().DAY_ORDER!=null)? currenrDayOrder.FirstOrDefault().DAY_ORDER:string.Empty;
                            //   Session[Common.SESSION_VARIABLES.DAY_ORDER_DATE] = currenrDayOrder.FirstOrDefault().DAY_ORDER_DATE;
                            // }

                            if (!string.IsNullOrEmpty(sUserRoles))
                            {
                                sUserRoleId = resultArgs.DataSource.Table.Rows[0][Common.USER_ACCOUNT.ROLE].ToString();
                                dv.Clear();
                                dv.Add(Common.USER_ROLES_RIGHTS.ROLE_ID, sUserRoleId, EnumCommand.DataType.String);
                                resultArgs = await Task.Run(() => objAccount.FetchMenuItemsByRole(dv));
                                Session[Common.SESSION_VARIABLES.MENU_ITEMS] = resultArgs.DataSource.Table;
                            }
                            using (LoginLog objHandler = new LoginLog())
                            {
                                objHandler.WriteUerDetails((Session[Common.SESSION_VARIABLES.USERNAME] != null) ? Session[Common.SESSION_VARIABLES.USERNAME].ToString() : "", collection.objLoginViewModel.Username);
                            }
                            if (sUserRoles.ToUpper().Equals("STUDENT"))
                            {
                                StuPersonalInfo studentPersonal = new StuPersonalInfo();
                                studentPersonal.STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                                var activeSemesterForStudent = await Task.Run(() => (List<SemesterRootInfo>)CMS.DAO.CMSHandler.FetchData<SemesterRootInfo>(studentPersonal, CMS.SQL.SupportData.SupportDataSQL.GetSupportDataSQL(DAO.SupportDataSQLCommands.FetchActiveSemesterForStudentByAcademicYear), dtActiveYear.Rows[0][Common.ACADEMIC_YEAR.AC_YEAR].ToString()).DataSource.Data);
                                if (activeSemesterForStudent != null)
                                    Session[Common.SESSION_VARIABLES.ACTIVE_SEMESTER_FOR_STUDENT] = activeSemesterForStudent.FirstOrDefault().SEMESTER;
                            }
                            return RedirectToAction("DashboardIndex", "DashBoard");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = Common.Messages.InvalidUserCredentials;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = Common.Messages.InvalidUserCredentials;
                        return RedirectToAction("Index");
                    }
                }
                //   }
                //   else
                //   {
                //      ModelState.AddModelError("LoginError",Common.Messages.InvalidUserCredentials);
                //  }

            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("AccountController", "Login", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("AccountController", "Login", ex.Message);
                }
                TempData["ErrorMessage"] = Common.Messages.InvalidUserCredentials;
                return RedirectToAction("Index"); ;
            }
            //  return RedirectToAction("Index");
        }
        #region Change Password
        public ActionResult ChangePassword()
        {
            return View();
        }
        public JsonResult ViewDetails(string sid)
        {
            USER_ACCOUNT_INFO objModel = new USER_ACCOUNT_INFO();
            objModel.USERNAME = sid;
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    var role = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(objModel, AccountSQL.GetAccountSQL(AccountSQLCommands.FetchUserRoleDetails)).DataSource.Data;
                        var obj = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(objModel, AccountSQL.GetAccountSQL(AccountSQLCommands.FetchUserDetailsForStudent)).DataSource.Data;
                        if (obj != null && obj.Count > 0)
                        {
                            objModel.USERNAME = obj.FirstOrDefault().USERNAME;
                            objModel.USER_ACCOUNT_ID = obj.FirstOrDefault().USER_ACCOUNT_ID;
                            objModel.NAME = obj.FirstOrDefault().NAME;
                            objModel.MOBILE = obj.FirstOrDefault().MOBILE;
                            objModel.DATE_OF_BIRTH = obj.FirstOrDefault().DATE_OF_BIRTH;
                        }
                }
                catch (Exception ex)
                {
                    using (ErrorLog ObjLog = new ErrorLog())
                    {
                        ObjLog.WriteError("AccountController", "ViewDetails", ex.Message);
                    }
                }
            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditPassword(string NewPassword)
        {
            string sResult = string.Empty;
            try
            {
                USER_ACCOUNT_INFO objModel = new USER_ACCOUNT_INFO();

                var ChangePasswords = new ChangePassword();
                var Result = JsonConvert.DeserializeObject<ChangePassword>(NewPassword);
                Result.PASSWORD = CommonMethods.GetSha256FromString(Result.PASSWORD);
                ResultArgs resultArgs = new ResultArgs();
                objModel.USERNAME = Result.USERNAME;
                var obj = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(objModel, AccountSQL.GetAccountSQL(AccountSQLCommands.FetchUserDetailsByUsername)).DataSource.Data;
                if (obj != null && obj.Count > 0)
                {
                    Result.USER_ACCOUNT_ID = obj.FirstOrDefault().USER_ACCOUNT_ID;
                }
                resultArgs = CMSHandler.SaveOrUpdate(Result, AccountSQL.GetAccountSQL(AccountSQLCommands.ResetPassword));
                if (resultArgs.Success)
                {
                    sResult = "Password Changed Successfully...!";
                }
                else
                {
                    sResult = "Failed to Update Password...!";
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog ObjLog = new ErrorLog())
                {
                    ObjLog.WriteError("AccountController", "EditPassword", ex.Message);
                }
            }
            if (string.IsNullOrEmpty(sResult))
            {
                sResult = "No Changes Made !";
            }
            return Json(sResult);
        }
        #endregion
        #region Forgot Password
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public string SendMail(string email)
        {
            string sResult = string.Empty;


            var FetchEmail = (List<USER_ACCOUNT_INFO>)DAO.CMSHandler.FetchData<USER_ACCOUNT_INFO>(new USER_ACCOUNT_INFO() { EMAIL = email }, AccountSQL.GetAccountSQL(DAO.AccountSQLCommands.FetchEmailAddress)).DataSource.Data;
            if (FetchEmail != null && FetchEmail.Count > 0)
            {
                string sPassword = CommonMethods.GetRandomPassword();
                var result = DAO.CMSHandler.SaveOrUpdate(new USER_ACCOUNT_INFO() { USER_ACCOUNT_ID = FetchEmail.FirstOrDefault().USER_ACCOUNT_ID, PASSWORD = CommonMethods.GetSha256FromString(sPassword) }, AccountSQL.GetAccountSQL(DAO.AccountSQLCommands.UpdatePassword));
                if (result.Success)
                {
                    string sFromEmail = ConfigurationManager.AppSettings["AdmissionFromMail"].ToString();
                    string sPasswordEmail = ConfigurationManager.AppSettings["AdmissionPassword"].ToString();
                    string sContent = @"Dear " + FetchEmail.FirstOrDefault().NAME + ",\n\n Your new password is " + sPassword + ".\n By\nSt. Mary's College";

                    CommonMethods.SendMailFromGmailSMTP(sFromEmail, sPasswordEmail, FetchEmail.FirstOrDefault().EMAIL, "New Password from St. Mary's College", sContent);
                    sResult = "New password has been sent to your registered email.";
                }

            }
            else
            {
                sResult = "Invalid Email-Id";
            }
            return sResult;
        }
        #endregion
        #region userAccountList


        public JsonResult SendPassword(string sIssueIds)
        {
            JsonResultData objResult = new JsonResultData();
            string scontent = string.Empty;
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sIssueIds))
                {
                    string sSQL = AccountSQL.GetAccountSQL(DAO.AccountSQLCommands.FetchUserAccountListByUserIds).Replace(Common.Delimiter.QUS + Common.USER_ACCOUNT.USER_ID, sIssueIds.TrimEnd(','));
                    var userAccountList = (List<USER_ACCOUNT_INFO>)CMS.DAO.CMSHandler.FetchData<USER_ACCOUNT_INFO>(null, sSQL, Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
                    ComposeMessageList composeList = new ComposeMessageList();
                    composeList.sms = new List<sms>();
                    if (userAccountList != null && userAccountList.Count > 0)
                    {
                        var smsSetting = (List<SMS_SETTING>)CMSHandler.FetchData<SMS_SETTING>(new SMS_SETTING() { API_MODE = "1" }, CommunicationSQL.GetCommunicationSQL(CommunicationSQLCommands.FetchSolutionInfiApi)).DataSource.Data;

                        foreach (var item in userAccountList)
                        {
                            var smsItem = new sms();
                            scontent = @"Dear " + item.NAME.ToUpper() + ",\n Thank you for applying/verifying at St. Mary's College(Autonomous) you may now login by clicking the link below. http://admission.stmaryscollege.edu.in/account/index  if clicking the link doesn't work, you can copy and paste the link into your browser's address window, or retype it there, use this login credential \n Application No: " + item.APPLICATION_NO
                                .ToUpper() + " \n username: " + item.USERNAME + "\n password: " + ((CommonMethods.IsValidEmail(item.USERNAME)) ? item.MOBILE : item.DOB) + " \n for any query use help lines: online payment:8012748844 ,admission:0461-2320946 email:admission@hccThoothukudi.ac.in \n best wishes, \n admission team,\n St. Mary's College,\n Thoothukudi.";
                            smsItem.message = scontent;
                            smsItem.custom1 = item.USER_ID;
                            smsItem.to = item.MOBILE;
                            composeList.sms.Add(smsItem);
                        }
                        composeList.sender = smsSetting.FirstOrDefault().SENDER;
                        composeList.flash = 0;
                        composeList.unicode = 0;

                        var sJson = JsonConvert.SerializeObject(composeList);
                        string sParameter = string.Concat(Common.SolutionInfiSMSSettingFields.API_KEY.ToLower(), Common.Delimiter.EQUAL, smsSetting.FirstOrDefault().API_KEY, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.METHOD.ToLower(), Common.Delimiter.EQUAL, smsSetting.FirstOrDefault().METHOD, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.FORMAT.ToLower(), Common.Delimiter.EQUAL, smsSetting.FirstOrDefault().FORMAT, Common.Delimiter.AMPRESAND, Common.SolutionInfiSMSSettingFields.JSON.ToLower(), Common.Delimiter.EQUAL, sJson);
                        CommunicationController communication = new CommunicationController();
                        communication.sendBulkMessage(smsSetting.FirstOrDefault().BASE_URL, sParameter);
                        //var result = CommonMethods.TEST_HttpPostMethod(smsSetting.FirstOrDefault().BASE_URL, sParameter);      
                        ObjJasonData.Message = Common.Messages.SendMessage;
                    }
                    else
                    {
                        ObjJasonData.Message = Common.Messages.NoRecordsfound;
                        ObjJasonData.ErrorCode = Common.ErrorCode.NoContent;
                    }
                }
                else
                {
                    ObjJasonData.Message = Common.ErrorMessage.BadRequest;
                    ObjJasonData.ErrorCode = Common.ErrorCode.BadRequest;
                }

            }
            else
            {
                ObjJasonData.Message = Common.Messages.SessionIsExpiredPleaseLoginAgain;
                ObjJasonData.ErrorCode = Common.ErrorCode.RequestTimeout;
            }
            return Json(ObjJasonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserAccount()
        {
            UserAccountViewModel userAccountViewModel = new UserAccountViewModel();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                userAccountViewModel.userAccountInfo = (List<USER_ACCOUNT_INFO>)CMS.DAO.CMSHandler.FetchData<USER_ACCOUNT_INFO>(null, SQL.Account.AccountSQL.GetAccountSQL(DAO.AccountSQLCommands.FetchUserAccountList), Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString()).DataSource.Data;
            }
            return View(userAccountViewModel);
        }

        #endregion

    }
}
