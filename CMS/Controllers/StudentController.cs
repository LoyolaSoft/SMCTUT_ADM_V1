using CMS.DAO;
using CMS.Models;
using CMS.SQL.Student;
using CMS.SQL.SupportData;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class StudentController : Controller
    {
        string sResult = string.Empty;
        DataValue dv = new DataValue();
        ResultArgs resultArgs = new ResultArgs();
        List<IF_STUDENT_EXITS> Exits = new List<IF_STUDENT_EXITS>();
        IF_STUDENT_EXITS ObjStudent = new IF_STUDENT_EXITS();
        string sSql = string.Empty;
        // GET: Student
        public ActionResult Index()
        {
            //Load DDL

            return View();
        }
        public ActionResult Add()
        {
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            DataTable dtCourseType = new DataTable();
            DataTable dtCourseItems = new DataTable();
            DataTable dtGender = new DataTable();
            DataTable dtBloodGroup = new DataTable();
            StudentViewModel objStudent = new StudentViewModel();
            objStudent.objStudent = new ViewModel.Model.StudentModel();

            dtCourseType = SupportDataMethod.FetchCourseItems((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            objStudent.CourseTypeItems = (from DataRow dr in dtCourseType.Rows select new SelectListItem { Value = dr[Common.SUP_COURSE_TYPE.COURSE_TYPE_ID].ToString(), Text = dr[Common.SUP_COURSE_TYPE.COURSE_TYPE].ToString() });
            objStudent.objStudent.TestList = new SelectList((from DataRow dr in dtCourseType.Rows select new SelectListItem { Value = dr[Common.SUP_COURSE_TYPE.COURSE_TYPE_ID].ToString(), Text = dr[Common.SUP_COURSE_TYPE.COURSE_TYPE].ToString() }).ToList());
            // objStudent.GenderList = (from DataRow dr in dt.Rows select new SelectListItem { Value = dr["ID"].ToString(), Text = dr["Gender"].ToString() });

            return View(objStudent);
        }
        [HttpPost]
        public ActionResult Save(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                string str = collection["objStudent.GUARDIAN_NAME"];
                string str1 = collection["ddlCourseType"];
                string str2 = collection["txtTest"];

                return View();
            }


            return View("Model is not valid ");
        }

        public ActionResult Test()
        {

            Gender objGender = new Gender();
            return View(objGender);
        }



        #region Personal Information
        // Add PersoanlInfo UI ...

        public ActionResult AddStuPersonalInfo()
        {
            StuPersonalInfo objStuPersonalInfo = new StuPersonalInfo();
            //Gender ...
            DataTable dtGender = new DataTable();
            dtGender = SupportDataMethod.FetchGender().DataSource.Table;
            List<SupGender> liGender = new List<SupGender>();
            if (dtGender != null && dtGender.Rows.Count > 0)
            {
                liGender = (from DataRow dr in dtGender.Rows
                            select new SupGender()
                            {
                                GENDER_ID = dr[Common.SUP_GENDER.GENDER_ID].ToString(),
                                GENDER_NAME = dr[Common.SUP_GENDER.GENDER_NAME].ToString()
                            }).ToList();
                objStuPersonalInfo.GENDERID = new SelectList(liGender, Common.SUP_GENDER.GENDER_ID, Common.SUP_GENDER.GENDER_NAME);
            }

            // Community ...
            DataTable dtCommunity = new DataTable();
            dtCommunity = SupportDataMethod.FetchCommunity().DataSource.Table;
            List<SupCommunity> liCommunity = new List<SupCommunity>();
            if (dtCommunity != null && dtCommunity.Rows.Count > 0)
            {
                liCommunity = (from DataRow dr in dtCommunity.Rows
                               select new SupCommunity()
                               {
                                   COMMUNITYID = dr[Common.SUP_COMMUNITY.COMMUNITYID].ToString(),
                                   COMMUNITY = dr[Common.SUP_COMMUNITY.COMMUNITY].ToString()
                               }).ToList();
                objStuPersonalInfo.COMMUNITY = new SelectList(liCommunity, Common.SUP_COMMUNITY.COMMUNITYID, Common.SUP_COMMUNITY.COMMUNITY);
            }

            // Mother Tongue ...
            DataTable dtMotherTongue = new DataTable();
            dtMotherTongue = SupportDataMethod.FetchMotherTongue().DataSource.Table;
            List<SupMotherTongue> liMotherTongue = new List<SupMotherTongue>();
            if (dtMotherTongue != null && dtMotherTongue.Rows.Count > 0)
            {
                liMotherTongue = (from DataRow dr in dtMotherTongue.Rows
                                  select new SupMotherTongue()
                                  {
                                      MOTHER_TONGUE_ID = dr[Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_ID].ToString(),
                                      MOTHER_TONGUE_NAME = dr[Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_NAME].ToString()
                                  }).ToList();
                objStuPersonalInfo.MOTHERTONGUE = new SelectList(liMotherTongue, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_ID, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_NAME);
            }

            //First Language ..
            DataTable dtLanguage = new DataTable();
            dtLanguage = SupportDataMethod.FetchLanguage().DataSource.Table;
            List<Sup_Language> liLanguage = new List<Sup_Language>();
            if (dtLanguage != null && dtLanguage.Rows.Count > 0)
            {
                liLanguage = (from DataRow dr in dtLanguage.Rows
                              select new Sup_Language()
                              {
                                  LANGUAGE_ID = dr[Common.SUP_LANGUAGE.LANGUAGE_ID].ToString(),
                                  LANGUAGE_NAME = dr[Common.SUP_LANGUAGE.LANGUAGE_NAME].ToString()
                              }).ToList();
                objStuPersonalInfo.FIRSTLANGUAGE = new SelectList(liLanguage, Common.SUP_LANGUAGE.LANGUAGE_ID, Common.SUP_LANGUAGE.LANGUAGE_NAME);
                objStuPersonalInfo.SECONDLANGUAGE = new SelectList(liLanguage, Common.SUP_LANGUAGE.LANGUAGE_ID, Common.SUP_LANGUAGE.LANGUAGE_NAME);
            }

            // Boold Group ...
            DataTable dtBloodGroup = new DataTable();
            dtBloodGroup = SupportDataMethod.FetchBloodGroup().DataSource.Table;
            List<SupBloodGroup> liBloodGroup = new List<SupBloodGroup>();
            if (dtBloodGroup != null && dtBloodGroup.Rows.Count > 0)
            {
                liBloodGroup = (from DataRow dr in dtBloodGroup.Rows
                                select new SupBloodGroup()
                                {
                                    BLOOD_GROUP_ID = dr[Common.SUP_BLOOD_GROUP.BLOOD_GROUP_ID].ToString(),
                                    BLOOD_GROUP_NAME = dr[Common.SUP_BLOOD_GROUP.BLOOD_GROUP_NAME].ToString()
                                }).ToList();
                objStuPersonalInfo.BLOODGROUP = new SelectList(liBloodGroup, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_ID, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_NAME);
            }

            // Religion ..
            DataTable dtReligion = new DataTable();
            dtReligion = SupportDataMethod.FetchReligion().DataSource.Table;
            List<SUP_RELIGION> liReligion = new List<SUP_RELIGION>();
            if (dtReligion != null && dtReligion.Rows.Count > 0)
            {
                liReligion = (from DataRow dr in dtReligion.Rows
                              select new SUP_RELIGION()
                              {
                                  RELIGIONID = dr[Common.SUP_RELIGION.RELIGIONID].ToString(),
                                  RELIGION = dr[Common.SUP_RELIGION.RELIGION].ToString()
                              }).ToList();
                objStuPersonalInfo.RELIGION = new SelectList(liReligion, Common.SUP_RELIGION.RELIGIONID, Common.SUP_RELIGION.RELIGION);
            }

            //Nationality ...
            DataTable dtNationality = new DataTable();
            dtNationality = SupportDataMethod.FetchNationality().DataSource.Table;
            List<SupNationality> liNationality = new List<SupNationality>();
            if (dtNationality != null && dtNationality.Rows.Count > 0)
            {
                liNationality = (from DataRow dr in dtNationality.Rows
                                 select new SupNationality()
                                 {
                                     NATIONALITY_ID = dr[Common.SUP_NATIONALITY.NATIONALITY_ID].ToString(),
                                     NATIONALITY_NAME = dr[Common.SUP_NATIONALITY.NATIONALITY_NAME].ToString()
                                 }).ToList();
                objStuPersonalInfo.NATIONALITY = new SelectList(liNationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);
            }

            // SpeciallyAbled ..
            DataTable dtSpeciallyAbled = new DataTable();
            dtSpeciallyAbled = SupportDataMethod.FetchSpeciallyAbled().DataSource.Table;
            List<Sup_Specially_Abled> liSpeciallyAbled = new List<Sup_Specially_Abled>();
            if (dtSpeciallyAbled != null && dtSpeciallyAbled.Rows.Count > 0)
            {
                liSpeciallyAbled = (from DataRow dr in dtSpeciallyAbled.Rows
                                    select new Sup_Specially_Abled()
                                    {
                                        SPECIALLY_ABLED_ID = dr[Common.SUP_SPECIALLY_ABLED.SPECIALLY_ABLED_ID].ToString(),
                                        SPECIALLY_ABLED_NAME = dr[Common.SUP_SPECIALLY_ABLED.SPECIALLY_ABLED_NAME].ToString()
                                    }).ToList();
                objStuPersonalInfo.SPECIALLYABLED = new SelectList(liSpeciallyAbled, Common.SUP_SPECIALLY_ABLED.SPECIALLY_ABLED_ID, Common.SUP_SPECIALLY_ABLED.SPECIALLY_ABLED_NAME);
            }

            // ResidencyType ...
            DataTable dtResidencyType = new DataTable();
            dtResidencyType = SupportDataMethod.FetchResidencyType().DataSource.Table;
            List<Sup_ResidencyType> liResidencyType = new List<Sup_ResidencyType>();
            if (dtResidencyType != null && dtResidencyType.Rows.Count > 0)
            {
                liResidencyType = (from DataRow dr in dtResidencyType.Rows
                                   select new Sup_ResidencyType()
                                   {
                                       RESIDENCY_TYPE_ID = dr[Common.SUP_RESIDENCY_TYPE.RESIDENCY_TYPE_ID].ToString(),
                                       RESIDENCY_TYPE_NAME = dr[Common.SUP_RESIDENCY_TYPE.RESIDENCY_TYPE_NAME].ToString()
                                   }).ToList();
                objStuPersonalInfo.RESIDENCYTYPE = new SelectList(liResidencyType, Common.SUP_RESIDENCY_TYPE.RESIDENCY_TYPE_ID, Common.SUP_RESIDENCY_TYPE.RESIDENCY_TYPE_NAME);
            }

            //Marital Status ..
            DataTable dtMaritalStatus = new DataTable();
            dtMaritalStatus = SupportDataMethod.FetchMaritalStatus().DataSource.Table;
            List<SupMaritalStatus> liMaritalStatus = new List<SupMaritalStatus>();
            if (dtMaritalStatus != null && dtMaritalStatus.Rows.Count > 0)
            {
                liMaritalStatus = (from DataRow dr in dtMaritalStatus.Rows
                                   select new SupMaritalStatus()
                                   {
                                       MARITAL_STAUS_ID = dr[Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID].ToString(),
                                       MARITAL_STATUS_NAME = dr[Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME].ToString()
                                   }).ToList();
                objStuPersonalInfo.MARITALSTATUS = new SelectList(liMaritalStatus, Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID, Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME);
            }

            // Sup_Role ...
            var liRole = new List<SUP_ROLE>();
            liRole = (List<SUP_ROLE>)CMSHandler.FetchData<SUP_ROLE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchSupRole)).DataSource.Data;
            objStuPersonalInfo.Role = new SelectList(liRole, Common.SUP_ROLE.ROLE_ID, Common.SUP_ROLE.ROLE_NAME);

            // UserType ...
            var liUserType = new List<SUP_USER_TYPE>();
            liUserType = (List<SUP_USER_TYPE>)CMSHandler.FetchData<SUP_USER_TYPE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserType)).DataSource.Data;
            objStuPersonalInfo.UserType = new SelectList(liUserType, Common.SUP_USER_TYPE.USER_TYPE_ID, Common.SUP_USER_TYPE.USER_TYPE_NAME);
            return View(objStuPersonalInfo);
        }

        // Insert Personal Details .. 
        public JsonResult InsertPersonalInfo(string str)
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentPersonal>(str);
                string sDateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.DateOfBirth);
                dv.Clear();
                dv.Add(Common.STU_PERSONAL_INFO.FIRST_NAME, Result.firstName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.LAST_NAME, Result.LastName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.CASTE, Result.Caste, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.DATE_OF_BIRTH, sDateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.ANNUAL_INCOME, Result.AnnulIncome, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.RESIDENCE_NO, Result.ResidencyNo, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.PASSPORT_Number, Result.PossportNo, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.VISA_SPONSOR, Result.VisaSponser, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.VISA_Number, Result.VisaNumber, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK1, Result.Identity1, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK2, Result.Identity2, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.HOME_PHONE, Result.HomePhone, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.AADHAR_NUMBER, Result.AadharNumber, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STU_MOBILENO, Result.StuMobileNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.PLACE_OF_BIRTH, Result.PlaceOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MARITAL_STATUS, Result.MaritalStatus, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.GENDER, Result.Gender, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.COMMUNITY, Result.Community, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MOTHER_TONGUE, Result.MotherTongue, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.BLOOD_GROUP, Result.BloodGroup, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.RELIGION, Result.Religion, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.NATIONALITY, Result.Nationality, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FIRST_LANGUAGE, Result.FirstLanguage, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.SECOND_LANGUAGE, Result.SecondLanguage, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.SPECIALLY_ABLED, Result.SpecillyAblled, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.RESIDENCY_TYPE, Result.ResidencyType, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                using (StudentViewModel objModel = new StudentViewModel())
                {
                    resultArgs = objModel.UpdatePersonalInfo(dv);
                    if (resultArgs.Success)
                    {
                        var objAccountInfo = new USER_ACCOUNT_INFO();
                        var liAccountInfo = new List<USER_ACCOUNT_INFO>();
                        var liSuPAccountDetails = new List<SUP_USER>();

                        objAccountInfo.NAME = Result.firstName;
                        objAccountInfo.USER_ID = sStudentID;
                        objAccountInfo.MOBILE = Result.StuMobileNo;

                        objAccountInfo.USERNAME = (Session[Common.SESSION_VARIABLES.USER_NAME] != null) ? Session[Common.SESSION_VARIABLES.USER_NAME].ToString() : string.Empty;
                        liAccountInfo = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserAccountByName)).DataSource.Data;

                        objAccountInfo.USER_ACCOUNT_ID = liAccountInfo.FirstOrDefault().USER_ACCOUNT_ID;
                        objAccountInfo.EMAIL = liAccountInfo.FirstOrDefault().EMAIL;
                        objAccountInfo.ROLE = liAccountInfo.FirstOrDefault().ROLE;
                        objAccountInfo.USER_TYPE = liAccountInfo.FirstOrDefault().USER_TYPE;
                        objAccountInfo.PASSWORD = liAccountInfo.FirstOrDefault().PASSWORD;

                        resultArgs = CMSHandler.SaveOrUpdate(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.UpdateUserAccount));

                        sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                    }
                    else
                    {
                        sResult = Common.ErrorCode.NotAcceptable;
                    }
                }
            }
            return Json(sResult);
        }

        // Fetch Personal Details ...
        public JsonResult FetchPersonalInfo()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                dv.Clear();
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                DataTable dtPersonalInfo = new DataTable();
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    dtPersonalInfo = objViewModel.PersonalInfo(dv).DataSource.Table;
                }
                if (dtPersonalInfo != null && dtPersonalInfo.Rows.Count > 0)
                {
                    StudentPersonal objPersonal = new StudentPersonal()
                    {
                        firstName = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.FIRST_NAME].ToString(),
                        LastName = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.LAST_NAME].ToString(),
                        Gender = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.GENDER].ToString(),
                        Community = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.COMMUNITY].ToString(),
                        Nationality = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.NATIONALITY].ToString(),
                        MaritalStatus = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.MARITAL_STATUS].ToString(),
                        MotherTongue = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.MOTHER_TONGUE].ToString(),
                        AadharNumber = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.AADHAR_NUMBER].ToString(),
                        AnnulIncome = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.ANNUAL_INCOME].ToString(),
                        BloodGroup = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.BLOOD_GROUP].ToString(),
                        Caste = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.CASTE].ToString(),
                        DateOfBirth = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.DATE_OF_BIRTH].ToString(),
                        HomePhone = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.HOME_PHONE].ToString(),
                        Identity1 = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK1].ToString(),
                        Identity2 = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK2].ToString(),
                        PlaceOfBirth = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.PLACE_OF_BIRTH].ToString(),
                        PossportNo = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.PASSPORT_Number].ToString(),
                        Religion = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.RELIGION].ToString(),
                        ResidencyNo = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.RESIDENCE_NO].ToString(),
                        StuMobileNo = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.STU_MOBILENO].ToString(),
                        VisaNumber = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.VISA_Number].ToString(),
                        VisaSponser = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.VISA_SPONSOR].ToString(),
                        FirstLanguage = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.FIRST_LANGUAGE].ToString(),
                        SecondLanguage = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.SECOND_LANGUAGE].ToString(),
                        SpecillyAblled = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.SPECIALLY_ABLED].ToString(),
                        ResidencyType = dtPersonalInfo.Rows[0][Common.STU_PERSONAL_INFO.RESIDENCY_TYPE].ToString()
                    };
                    return Json(objPersonal);
                }
                else
                {
                    sResult = "No Records Available ...!";
                    return Json(sResult);
                }
            }
            else
            {
                sResult = "Session Has Expired Please Login And Try Again ...!";
                return Json(sResult);
            }
        }


        // Edit PersoanlInfo UI ...
        public ActionResult EditStuPersonalInfo()
        {
            StuPersonalInfo objStuPersonalInfo = new StuPersonalInfo();
            //Gender ...
            DataTable dtGender = new DataTable();
            dtGender = SupportDataMethod.FetchGender().DataSource.Table;
            List<SupGender> liGender = new List<SupGender>();
            if (dtGender != null && dtGender.Rows.Count > 0)
            {
                liGender = (from DataRow dr in dtGender.Rows
                            select new SupGender()
                            {
                                GENDER_ID = dr[Common.SUP_GENDER.GENDER_ID].ToString(),
                                GENDER_NAME = dr[Common.SUP_GENDER.GENDER_NAME].ToString()
                            }).ToList();
                objStuPersonalInfo.GENDERID = new SelectList(liGender, Common.SUP_GENDER.GENDER_ID, Common.SUP_GENDER.GENDER_NAME);
            }

            // Community ...
            DataTable dtCommunity = new DataTable();
            dtCommunity = SupportDataMethod.FetchCommunity().DataSource.Table;
            List<SupCommunity> liCommunity = new List<SupCommunity>();
            if (dtCommunity != null && dtCommunity.Rows.Count > 0)
            {
                liCommunity = (from DataRow dr in dtCommunity.Rows
                               select new SupCommunity()
                               {
                                   COMMUNITYID = dr[Common.SUP_COMMUNITY.COMMUNITYID].ToString(),
                                   COMMUNITY = dr[Common.SUP_COMMUNITY.COMMUNITY].ToString()
                               }).ToList();
                objStuPersonalInfo.COMMUNITY = new SelectList(liCommunity, Common.SUP_COMMUNITY.COMMUNITYID, Common.SUP_COMMUNITY.COMMUNITY);
            }

            // Mother Tongue ...
            DataTable dtMotherTongue = new DataTable();
            dtMotherTongue = SupportDataMethod.FetchMotherTongue().DataSource.Table;
            List<SupMotherTongue> liMotherTongue = new List<SupMotherTongue>();
            if (dtMotherTongue != null && dtMotherTongue.Rows.Count > 0)
            {
                liMotherTongue = (from DataRow dr in dtMotherTongue.Rows
                                  select new SupMotherTongue()
                                  {
                                      MOTHER_TONGUE_ID = dr[Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_ID].ToString(),
                                      MOTHER_TONGUE_NAME = dr[Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_NAME].ToString()
                                  }).ToList();
                objStuPersonalInfo.MOTHERTONGUE = new SelectList(liMotherTongue, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_ID, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_NAME);
            }

            //First Language ..
            DataTable dtLanguage = new DataTable();
            dtLanguage = SupportDataMethod.FetchLanguage().DataSource.Table;
            List<Sup_Language> liLanguage = new List<Sup_Language>();
            if (dtLanguage != null && dtLanguage.Rows.Count > 0)
            {
                liLanguage = (from DataRow dr in dtLanguage.Rows
                              select new Sup_Language()
                              {
                                  LANGUAGE_ID = dr[Common.SUP_LANGUAGE.LANGUAGE_ID].ToString(),
                                  LANGUAGE_NAME = dr[Common.SUP_LANGUAGE.LANGUAGE_NAME].ToString()
                              }).ToList();
                objStuPersonalInfo.FIRSTLANGUAGE = new SelectList(liLanguage, Common.SUP_LANGUAGE.LANGUAGE_ID, Common.SUP_LANGUAGE.LANGUAGE_NAME);
                objStuPersonalInfo.SECONDLANGUAGE = new SelectList(liLanguage, Common.SUP_LANGUAGE.LANGUAGE_ID, Common.SUP_LANGUAGE.LANGUAGE_NAME);
            }

            // Boold Group ...
            DataTable dtBloodGroup = new DataTable();
            dtBloodGroup = SupportDataMethod.FetchBloodGroup().DataSource.Table;
            List<SupBloodGroup> liBloodGroup = new List<SupBloodGroup>();
            if (dtBloodGroup != null && dtBloodGroup.Rows.Count > 0)
            {
                liBloodGroup = (from DataRow dr in dtBloodGroup.Rows
                                select new SupBloodGroup()
                                {
                                    BLOOD_GROUP_ID = dr[Common.SUP_BLOOD_GROUP.BLOOD_GROUP_ID].ToString(),
                                    BLOOD_GROUP_NAME = dr[Common.SUP_BLOOD_GROUP.BLOOD_GROUP_NAME].ToString()
                                }).ToList();
                objStuPersonalInfo.BLOODGROUP = new SelectList(liBloodGroup, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_ID, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_NAME);
            }

            // Religion ..
            DataTable dtReligion = new DataTable();
            dtReligion = SupportDataMethod.FetchReligion().DataSource.Table;
            List<SUP_RELIGION> liReligion = new List<SUP_RELIGION>();
            if (dtReligion != null && dtReligion.Rows.Count > 0)
            {
                liReligion = (from DataRow dr in dtReligion.Rows
                              select new SUP_RELIGION()
                              {
                                  RELIGIONID = dr[Common.SUP_RELIGION.RELIGIONID].ToString(),
                                  RELIGION = dr[Common.SUP_RELIGION.RELIGION].ToString()
                              }).ToList();
                objStuPersonalInfo.RELIGION = new SelectList(liReligion, Common.SUP_RELIGION.RELIGIONID, Common.SUP_RELIGION.RELIGION);
            }

            //Nationality ...
            DataTable dtNationality = new DataTable();
            dtNationality = SupportDataMethod.FetchNationality().DataSource.Table;
            List<SupNationality> liNationality = new List<SupNationality>();
            if (dtNationality != null && dtNationality.Rows.Count > 0)
            {
                liNationality = (from DataRow dr in dtNationality.Rows
                                 select new SupNationality()
                                 {
                                     NATIONALITY_ID = dr[Common.SUP_NATIONALITY.NATIONALITY_ID].ToString(),
                                     NATIONALITY_NAME = dr[Common.SUP_NATIONALITY.NATIONALITY_NAME].ToString()
                                 }).ToList();
                objStuPersonalInfo.NATIONALITY = new SelectList(liNationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);
            }

            // SpeciallyAbled ..
            DataTable dtSpeciallyAbled = new DataTable();
            dtSpeciallyAbled = SupportDataMethod.FetchSpeciallyAbled().DataSource.Table;
            List<Sup_Specially_Abled> liSpeciallyAbled = new List<Sup_Specially_Abled>();
            if (dtSpeciallyAbled != null && dtSpeciallyAbled.Rows.Count > 0)
            {
                liSpeciallyAbled = (from DataRow dr in dtSpeciallyAbled.Rows
                                    select new Sup_Specially_Abled()
                                    {
                                        SPECIALLY_ABLED_ID = dr[Common.SUP_SPECIALLY_ABLED.SPECIALLY_ABLED_ID].ToString(),
                                        SPECIALLY_ABLED_NAME = dr[Common.SUP_SPECIALLY_ABLED.SPECIALLY_ABLED_NAME].ToString()
                                    }).ToList();
                objStuPersonalInfo.SPECIALLYABLED = new SelectList(liSpeciallyAbled, Common.SUP_SPECIALLY_ABLED.SPECIALLY_ABLED_ID, Common.SUP_SPECIALLY_ABLED.SPECIALLY_ABLED_NAME);
            }

            // ResidencyType ...
            DataTable dtResidencyType = new DataTable();
            dtResidencyType = SupportDataMethod.FetchResidencyType().DataSource.Table;
            List<Sup_ResidencyType> liResidencyType = new List<Sup_ResidencyType>();
            if (dtResidencyType != null && dtResidencyType.Rows.Count > 0)
            {
                liResidencyType = (from DataRow dr in dtResidencyType.Rows
                                   select new Sup_ResidencyType()
                                   {
                                       RESIDENCY_TYPE_ID = dr[Common.SUP_RESIDENCY_TYPE.RESIDENCY_TYPE_ID].ToString(),
                                       RESIDENCY_TYPE_NAME = dr[Common.SUP_RESIDENCY_TYPE.RESIDENCY_TYPE_NAME].ToString()
                                   }).ToList();
                objStuPersonalInfo.RESIDENCYTYPE = new SelectList(liResidencyType, Common.SUP_RESIDENCY_TYPE.RESIDENCY_TYPE_ID, Common.SUP_RESIDENCY_TYPE.RESIDENCY_TYPE_NAME);
            }

            //Marital Status ..
            DataTable dtMaritalStatus = new DataTable();
            dtMaritalStatus = SupportDataMethod.FetchMaritalStatus().DataSource.Table;
            List<SupMaritalStatus> liMaritalStatus = new List<SupMaritalStatus>();
            if (dtMaritalStatus != null && dtMaritalStatus.Rows.Count > 0)
            {
                liMaritalStatus = (from DataRow dr in dtMaritalStatus.Rows
                                   select new SupMaritalStatus()
                                   {
                                       MARITAL_STAUS_ID = dr[Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID].ToString(),
                                       MARITAL_STATUS_NAME = dr[Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME].ToString()
                                   }).ToList();
                objStuPersonalInfo.MARITALSTATUS = new SelectList(liMaritalStatus, Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID, Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME);
            }
            return View(objStuPersonalInfo);
        }

        //Edit Personal Info ....
        public JsonResult EditPersoalInfo(string str)
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentPersonal>(str);
                string DateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.DateOfBirth);
                dv.Clear();

                dv.Add(Common.STU_PERSONAL_INFO.FIRST_NAME, Result.firstName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.LAST_NAME, Result.LastName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.CASTE, Result.Caste, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.DATE_OF_BIRTH, DateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.ANNUAL_INCOME, Result.AnnulIncome, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.RESIDENCE_NO, Result.ResidencyNo, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.PASSPORT_Number, Result.PossportNo, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.VISA_SPONSOR, Result.VisaSponser, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.VISA_Number, Result.VisaNumber, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK1, Result.Identity1, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK2, Result.Identity2, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.HOME_PHONE, Result.HomePhone, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.AADHAR_NUMBER, Result.AadharNumber, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STU_MOBILENO, Result.StuMobileNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.PLACE_OF_BIRTH, Result.PlaceOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MARITAL_STATUS, Result.MaritalStatus, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.GENDER, Result.Gender, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.COMMUNITY, Result.Community, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MOTHER_TONGUE, Result.MotherTongue, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.BLOOD_GROUP, Result.BloodGroup, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.RELIGION, Result.Religion, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.NATIONALITY, Result.Nationality, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FIRST_LANGUAGE, Result.FirstLanguage, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.SECOND_LANGUAGE, Result.SecondLanguage, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.SPECIALLY_ABLED, Result.SpecillyAblled, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.RESIDENCY_TYPE, Result.ResidencyType, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                using (StudentViewModel objModel = new StudentViewModel())
                {
                    resultArgs = objModel.UpdatePersonalInfo(dv);
                    if (resultArgs.Success)
                    {
                        //Update Account Details .....
                        var objAccountInfo = new USER_ACCOUNT_INFO();
                        var liAccountInfo = new List<USER_ACCOUNT_INFO>();
                        //var liSuPAccountDetails = new List<SUP_USER>();
                        objAccountInfo.NAME = Result.firstName;
                        objAccountInfo.USER_ID = sStudentID;
                        objAccountInfo.MOBILE = Result.StuMobileNo;
                        objAccountInfo.USERNAME = (Session[Common.SESSION_VARIABLES.USER_NAME] != null) ? Session[Common.SESSION_VARIABLES.USER_NAME].ToString() : string.Empty;
                        liAccountInfo = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserAccountByName)).DataSource.Data;
                        if (liAccountInfo != null && liAccountInfo.Count > 0)
                        {
                            objAccountInfo.USER_ACCOUNT_ID = liAccountInfo.FirstOrDefault().USER_ACCOUNT_ID;
                            objAccountInfo.EMAIL = liAccountInfo.FirstOrDefault().EMAIL;
                            objAccountInfo.ROLE = liAccountInfo.FirstOrDefault().ROLE;
                            objAccountInfo.USER_TYPE = liAccountInfo.FirstOrDefault().USER_TYPE;
                            objAccountInfo.PASSWORD = liAccountInfo.FirstOrDefault().PASSWORD;
                            resultArgs = CMSHandler.SaveOrUpdate(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.UpdateUserAccount));
                        }
                        sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                    }
                    else
                    {
                        sResult = Common.ErrorCode.NotAcceptable;
                    }
                }
            }
            return Json(sResult);
        }
        #endregion

        #region Academic Information

        // Add Student  Academic UI ....
        public ActionResult AddStuAcademicInfo()
        {
            StuAcadamicInfo objAcademicInfo = new StuAcadamicInfo();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();

                //Batch ...
                DataTable dtBatch = new DataTable();
                dtBatch = SupportDataMethod.FetchBatch().DataSource.Table;
                List<sup_Batch> liBatch = new List<sup_Batch>();
                if (dtBatch != null && dtBatch.Rows.Count > 0)
                {
                    liBatch = (from DataRow dr in dtBatch.Rows
                               select new sup_Batch()
                               {
                                   BATCH_ID = dr[Common.SUP_BATCHES.BATCH_ID].ToString(),
                                   BATCH = dr[Common.SUP_BATCHES.BATCH].ToString()
                               }).ToList();
                    objAcademicInfo.BATCH = new SelectList(liBatch, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
                }

                // Department ...
                DataTable dtDepartment = new DataTable();
                dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                List<cp_Department_2017> liDepartment = new List<cp_Department_2017>();
                if (dtDepartment != null && dtDepartment.Rows.Count > 0)
                {
                    liDepartment = (from DataRow dr in dtDepartment.Rows
                                    select new cp_Department_2017()
                                    {
                                        DEPARTMENT_ID = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT_ID].ToString(),
                                        DEPARTMENT = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString()
                                    }).ToList();
                    objAcademicInfo.DEPT_ID = new SelectList(liDepartment, Common.CP_DEPARTMENT_2017.DEPARTMENT_ID, Common.CP_DEPARTMENT_2017.DEPARTMENT);
                }

                // Program ..
                DataTable dtProgram = new DataTable();
                dtProgram = SupportDataMethod.FetchProgram((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                List<cp_Program_2017> liProgram = new List<cp_Program_2017>();
                if (dtProgram != null && dtProgram.Rows.Count > 0)
                {
                    liProgram = (from DataRow dr in dtProgram.Rows
                                 select new cp_Program_2017()
                                 {
                                     PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                     PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                                 }).ToList();
                    objAcademicInfo.PROGRAM_ID = new SelectList(liProgram, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }

                // Shift ...
                DataTable dtFetchShift = new DataTable();
                dtFetchShift = SupportDataMethod.FetchShift().DataSource.Table;
                List<Sup_Shift> liShift = new List<Sup_Shift>();
                if (dtFetchShift != null && dtFetchShift.Rows.Count > 0)
                {
                    liShift = (from DataRow dr in dtFetchShift.Rows
                               select new Sup_Shift()
                               {
                                   SHIFT_ID = dr[Common.SUP_SHIFT.SHIFT_ID].ToString(),
                                   SHIFT_NAME = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString()
                               }).ToList();
                    objAcademicInfo.SHIFT = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                }
                //Class ...
                DataTable dtClass = new DataTable();
                dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
                List<cp_Classes_2017> liClasses = new List<cp_Classes_2017>();
                if (dtClass != null && dtClass.Rows.Count > 0)
                {
                    liClasses = (from DataRow dr in dtClass.Rows
                                 select new cp_Classes_2017()
                                 {
                                     CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                                     CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                                 }).ToList();
                    objAcademicInfo.CLASS_ID = new SelectList(liClasses, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                    objAcademicInfo.ADMITTED_CLASS = new SelectList(liClasses, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                // Sup_Role ...
                var liRole = new List<SUP_ROLE>();
                liRole = (List<SUP_ROLE>)CMSHandler.FetchData<SUP_ROLE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchSupRole)).DataSource.Data;
                objAcademicInfo.Role = new SelectList(liRole, Common.SUP_ROLE.ROLE_ID, Common.SUP_ROLE.ROLE_NAME);

                // UserType ...
                var liUserType = new List<SUP_USER_TYPE>();
                liUserType = (List<SUP_USER_TYPE>)CMSHandler.FetchData<SUP_USER_TYPE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserType)).DataSource.Data;
                objAcademicInfo.UserType = new SelectList(liUserType, Common.SUP_USER_TYPE.USER_TYPE_ID, Common.SUP_USER_TYPE.USER_TYPE_NAME);
            }

            return View(objAcademicInfo);
        }

        // Insert Acadamic Details ...
        public JsonResult InsertAcadamicInfo(string str)
        {
            try
            {
                var Result = JsonConvert.DeserializeObject<AcademicDetails>(str);
                if (!string.IsNullOrEmpty(Result.RollNo))
                {
                    var objAccountInfo = new USER_ACCOUNT_INFO();
                    var liAccountInfo = new List<USER_ACCOUNT_INFO>();
                    string sRegisterDate = CommonMethods.ConvertdatetoyearFromat(Result.sRegisterDate);
                    string sAdmissionDate = CommonMethods.ConvertdatetoyearFromat(Result.AdmissionDate);
                    dv.Clear();
                    dv.Add(Common.STU_PERSONAL_INFO.REGISTER_NO, Result.RegisterNo, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.ROLL_NO, Result.RollNo, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.UNIVERSITY_ROLLNO, Result.URollNo, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.ADMISSION_DATE, sAdmissionDate, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.ADMISSION_NO, Result.AdmissionNo, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.ADMITTED_CLASS, Result.AdmittedClass, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.STUDENT_REGISTERDED_DATE, sRegisterDate, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.REMARKS, Result.Remarks, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.TC_SERIAL_NO, Result.TCSerialNo, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.CLASS_ID, Result.Class, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.PROGRAM_ID, Result.Program, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.BATCH, Result.Batch, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.DEPT_ID, Result.Department, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.SHIFT_ID, Result.Shift, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.STU_EMAILID, Result.Stu_Email, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.IS_LEFT, Result.IsLeft, EnumCommand.DataType.String);
                    dv.Add(Common.USER_ACCOUNT.ROLE, Result.Role, EnumCommand.DataType.String);
                    dv.Add(Common.USER_ACCOUNT.USER_TYPE, Result.UserType, EnumCommand.DataType.String);

                    using (StudentViewModel objStudent = new StudentViewModel())
                    {
                        DataTable dtIfExit = new DataTable();
                        dtIfExit = objStudent.AcadamicIfExits(dv).DataSource.Table;
                        if (dtIfExit != null && dtIfExit.Rows.Count > 0)
                        {
                            sResult = Common.ErrorCode.Created;
                            // Update Statement ....
                            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                            {
                                resultArgs = objStudent.UpdateAcadamic(dv);
                                string sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
                                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                                resultArgs = objStudent.UpdateAcadamic(dv);

                                // update Stu_Class ...
                                Result.CLASS_ID = Result.Class;
                                Result.ACADEMIC_YEAR = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                                Result.STUDENT_ID = sStudentID;
                                var result = (List<AcademicDetails>)CMSHandler.FetchData<AcademicDetails>(Result, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuclassByStudentIdAndAcademicYear)).DataSource.Data;
                                Result.STU_CLASS_ID = result.FirstOrDefault().STU_CLASS_ID;
                                resultArgs = CMSHandler.SaveOrUpdate(Result, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuclass));

                                // Update User  Account ...
                                objAccountInfo.USER_ID = sStudentID;
                                objAccountInfo.EMAIL = Result.Stu_Email;
                                objAccountInfo.USERNAME = Result.RollNo.ToUpper();
                                objAccountInfo.PASSWORD = CommonMethods.GetSha256FromString(Result.RollNo.ToUpper());
                                objAccountInfo.ROLE = Result.Role;
                                objAccountInfo.USER_TYPE = Result.UserType;
                                liAccountInfo = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserAccountByRoleAndId)).DataSource.Data;
                                if (liAccountInfo != null && liAccountInfo.Count > 0)
                                {
                                    objAccountInfo.USER_ACCOUNT_ID = liAccountInfo.FirstOrDefault().USER_ACCOUNT_ID;
                                    objAccountInfo.MOBILE = liAccountInfo.FirstOrDefault().MOBILE;
                                    objAccountInfo.NAME = liAccountInfo.FirstOrDefault().NAME;
                                }
                                resultArgs = CMSHandler.SaveOrUpdate(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.UpdateUserAccount));
                                sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                            }
                        }
                        else
                        {
                            // Insert Statement ....
                            resultArgs = objStudent.InsertAcadamicDetails(dv);

                            if (resultArgs.Success)
                            {
                                Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] = resultArgs.RowUniqueId.ToString();

                                // insert Stu_class 
                                Result.CLASS_ID = Result.Class;
                                Result.ACADEMIC_YEAR = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                                Result.STUDENT_ID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
                                resultArgs = CMSHandler.SaveOrUpdate(Result, StudentSQL.GetStudentSQL(StudentSQLCommads.SaveStuClass));

                                // Insert User Account ...
                                objAccountInfo.NAME = "";
                                objAccountInfo.USERNAME = Result.RollNo.ToUpper();
                                Session[Common.SESSION_VARIABLES.USER_NAME] = Result.RollNo.ToUpper();
                                objAccountInfo.EMAIL = Result.Stu_Email;
                                objAccountInfo.MOBILE = "";
                                objAccountInfo.ROLE = Result.Role;
                                objAccountInfo.USER_TYPE = Result.UserType;
                                objAccountInfo.USER_ID = Result.STUDENT_ID;
                                objAccountInfo.PASSWORD = CommonMethods.GetSha256FromString(Result.RollNo.ToUpper());
                                //objAccountInfo.PASSWORD =Result.RollNo;
                                resultArgs = CMSHandler.SaveOrUpdate(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.SaveUserAccount));
                                sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                            }
                            else
                            {
                                sResult = Common.ErrorCode.NotAcceptable;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("StudentController", "InsertAcadamicInfo", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("StudentController", "InsertAcadamicInfo", ex.Message);
                }
                sResult = Common.ErrorCode.NotAcceptable;
            }
            return Json(sResult);
        }

        // Fetch AcadamicInfo ...
        public JsonResult FetchAcadamicInfo()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
            DataTable dtFetchAcadamicInfo = new DataTable();
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtFetchAcadamicInfo = objViewModel.AcadamicInfo(dv).DataSource.Table;
            }
            if (dtFetchAcadamicInfo != null && dtFetchAcadamicInfo.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.USER_NAME] = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.ROLL_NO].ToString();
                AcademicDetails objAcadamic = new AcademicDetails()
                {
                    RollNo = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.ROLL_NO].ToString(),
                    URollNo = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.UNIVERSITY_ROLLNO].ToString(),
                    AdmissionNo = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.ADMISSION_NO].ToString(),
                    AdmissionDate = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.ADMISSION_DATE].ToString(),
                    AdmittedClass = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.ADMITTED_CLASS].ToString(),
                    RegisterNo = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.REGISTER_NO].ToString(),
                    Remarks = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.REMARKS].ToString(),
                    sRegisterDate = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.STUDENT_REGISTERDED_DATE].ToString(),
                    Batch = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.BATCH].ToString(),
                    Department = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.DEPT_ID].ToString(),
                    Program = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.PROGRAM_ID].ToString(),
                    Class = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.CLASS_ID].ToString(),
                    TCSerialNo = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.TC_SERIAL_NO].ToString(),
                    Shift = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.SHIFT_ID].ToString(),
                    Stu_Email = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.STU_EMAILID].ToString(),
                    IsLeft = dtFetchAcadamicInfo.Rows[0][Common.STU_PERSONAL_INFO.IS_LEFT].ToString(),
                    Role = dtFetchAcadamicInfo.Rows[0][Common.USER_ACCOUNT.ROLE].ToString(),
                    UserType = dtFetchAcadamicInfo.Rows[0][Common.USER_ACCOUNT.USER_TYPE].ToString()
                };
                return Json(objAcadamic);
            }
            else
            {
                return Json(sResult);
            }

        }

        // Edit Student  Academic UI ....
        public ActionResult EditStuAcademicInfo()
        {
            StuAcadamicInfo objAcademicInfo = new StuAcadamicInfo();
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                //Batch ...
                DataTable dtBatch = new DataTable();
                dtBatch = SupportDataMethod.FetchBatch().DataSource.Table;
                List<sup_Batch> liBatch = new List<sup_Batch>();
                if (dtBatch != null && dtBatch.Rows.Count > 0)
                {
                    liBatch = (from DataRow dr in dtBatch.Rows
                               select new sup_Batch()
                               {
                                   BATCH_ID = dr[Common.SUP_BATCHES.BATCH_ID].ToString(),
                                   BATCH = dr[Common.SUP_BATCHES.BATCH].ToString()
                               }).ToList();
                    objAcademicInfo.BATCH = new SelectList(liBatch, Common.SUP_BATCHES.BATCH_ID, Common.SUP_BATCHES.BATCH);
                }

                // Department ...
                DataTable dtDepartment = new DataTable();
                dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                List<cp_Department_2017> liDepartment = new List<cp_Department_2017>();
                if (dtDepartment != null && dtDepartment.Rows.Count > 0)
                {
                    liDepartment = (from DataRow dr in dtDepartment.Rows
                                    select new cp_Department_2017()
                                    {
                                        DEPARTMENT_ID = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT_ID].ToString(),
                                        DEPARTMENT = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString()
                                    }).ToList();
                    objAcademicInfo.DEPT_ID = new SelectList(liDepartment, Common.CP_DEPARTMENT_2017.DEPARTMENT_ID, Common.CP_DEPARTMENT_2017.DEPARTMENT);
                }

                // Program ..
                DataTable dtProgram = new DataTable();
                dtProgram = SupportDataMethod.FetchProgram((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                List<cp_Program_2017> liProgram = new List<cp_Program_2017>();
                if (dtProgram != null && dtProgram.Rows.Count > 0)
                {
                    liProgram = (from DataRow dr in dtProgram.Rows
                                 select new cp_Program_2017()
                                 {
                                     PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                     PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                                 }).ToList();
                    objAcademicInfo.PROGRAM_ID = new SelectList(liProgram, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                }

                // Shift ...
                DataTable dtFetchShift = new DataTable();
                dtFetchShift = SupportDataMethod.FetchShift().DataSource.Table;
                List<Sup_Shift> liShift = new List<Sup_Shift>();
                if (dtFetchShift != null && dtFetchShift.Rows.Count > 0)
                {
                    liShift = (from DataRow dr in dtFetchShift.Rows
                               select new Sup_Shift()
                               {
                                   SHIFT_ID = dr[Common.SUP_SHIFT.SHIFT_ID].ToString(),
                                   SHIFT_NAME = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString()
                               }).ToList();
                    objAcademicInfo.SHIFT = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                }
                //Class ...
                DataTable dtClass = new DataTable();
                dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
                List<cp_Classes_2017> liClasses = new List<cp_Classes_2017>();
                if (dtClass != null && dtClass.Rows.Count > 0)
                {
                    liClasses = (from DataRow dr in dtClass.Rows
                                 select new cp_Classes_2017()
                                 {
                                     CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                                     CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                                 }).ToList();
                    objAcademicInfo.CLASS_ID = new SelectList(liClasses, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                    objAcademicInfo.ADMITTED_CLASS = new SelectList(liClasses, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                }
                // Sup_Role ...
                var liRole = new List<SUP_ROLE>();
                liRole = (List<SUP_ROLE>)CMSHandler.FetchData<SUP_ROLE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchSupRole)).DataSource.Data;
                objAcademicInfo.Role = new SelectList(liRole, Common.SUP_ROLE.ROLE_ID, Common.SUP_ROLE.ROLE_NAME);

                // UserType ...
                var liUserType = new List<SUP_USER_TYPE>();
                liUserType = (List<SUP_USER_TYPE>)CMSHandler.FetchData<SUP_USER_TYPE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserType)).DataSource.Data;
                objAcademicInfo.UserType = new SelectList(liUserType, Common.SUP_USER_TYPE.USER_TYPE_ID, Common.SUP_USER_TYPE.USER_TYPE_NAME);
            }
            return View(objAcademicInfo);
        }

        // Update AcadamicInfo ...
        public JsonResult EditAcadamicInfo(string str)
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;

            try
            {
                var Result = JsonConvert.DeserializeObject<AcademicDetails>(str);
                string sRegisterDate = CommonMethods.ConvertdatetoyearFromat(Result.sRegisterDate);
                string sAdmissionDate = CommonMethods.ConvertdatetoyearFromat(Result.AdmissionDate);
                dv.Clear();
                dv.Add(Common.STU_PERSONAL_INFO.REGISTER_NO, Result.RegisterNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.ROLL_NO, Result.RollNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.UNIVERSITY_ROLLNO, Result.URollNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.ADMISSION_DATE, sAdmissionDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.ADMISSION_NO, Result.AdmissionNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.ADMITTED_CLASS, Result.AdmittedClass, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_REGISTERDED_DATE, sRegisterDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.REMARKS, Result.Remarks, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.TC_SERIAL_NO, Result.TCSerialNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.CLASS_ID, Result.Class, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.PROGRAM_ID, Result.Program, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.BATCH, Result.Batch, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.DEPT_ID, Result.Department, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.SHIFT_ID, Result.Shift, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STU_EMAILID, Result.Stu_Email, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.IS_LEFT, Result.IsLeft, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                dv.Add(Common.USER_ACCOUNT.ROLE, Result.Role, EnumCommand.DataType.String);
                dv.Add(Common.USER_ACCOUNT.USER_TYPE, Result.UserType, EnumCommand.DataType.String);
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.UpdateAcadamic(dv);
                    if (resultArgs.Success)
                    {
                        var objAccountInfo = new USER_ACCOUNT_INFO();
                        var liAccountInfo = new List<USER_ACCOUNT_INFO>();

                        // update Stu_Class ...
                        Result.CLASS_ID = Result.Class;
                        Result.ACADEMIC_YEAR = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                        Result.STUDENT_ID = sStudentID;
                        var result = (List<AcademicDetails>)CMSHandler.FetchData<AcademicDetails>(Result, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuclassByStudentIdAndAcademicYear)).DataSource.Data;
                        Result.STU_CLASS_ID = result.FirstOrDefault().STU_CLASS_ID;
                        resultArgs = CMSHandler.SaveOrUpdate(Result, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuclass));

                        // Update User  Account ...
                        objAccountInfo.USER_ID = sStudentID;
                        objAccountInfo.EMAIL = Result.Stu_Email;
                        objAccountInfo.USERNAME = Result.RollNo.ToUpper();
                        objAccountInfo.PASSWORD = CommonMethods.GetSha256FromString(Result.RollNo.ToUpper());
                        objAccountInfo.ROLE = Result.Role;
                        objAccountInfo.USER_TYPE = Result.UserType;
                        liAccountInfo = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserAccountByRoleAndId)).DataSource.Data;
                        if (liAccountInfo != null && liAccountInfo.Count > 0)
                        {
                            objAccountInfo.USER_ACCOUNT_ID = liAccountInfo.FirstOrDefault().USER_ACCOUNT_ID;
                            objAccountInfo.MOBILE = liAccountInfo.FirstOrDefault().MOBILE;
                            objAccountInfo.NAME = liAccountInfo.FirstOrDefault().NAME;
                        }
                        resultArgs = CMSHandler.SaveOrUpdate(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.UpdateUserAccount));
                        sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                    }
                    else
                    {
                        sResult = Common.ErrorCode.NotAcceptable;
                    }
                    return Json(sResult);
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("StudentController", "EditAcadamicInfo", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("StudentController", "EditAcadamicInfo", ex.Message);
                }
                sResult = Common.ErrorCode.NotAcceptable;
                return Json(sResult);
            }
        }
        #endregion

        #region Adderess Information
        // Add Student Address UI ...

        public ActionResult AddStuAddressInfo()
        {
            StuAddressInfo objStuAddress = new StuAddressInfo();
            //Country ...
            DataTable dtCountry = new DataTable();
            dtCountry = SupportDataMethod.FetchCountry().DataSource.Table;
            List<SUP_COUNTRY> liCountry = new List<SUP_COUNTRY>();
            if (dtCountry != null && dtCountry.Rows.Count > 0)
            {
                liCountry = (from DataRow dr in dtCountry.Rows
                             select new SUP_COUNTRY()
                             {
                                 COUNTRY_ID = dr[Common.SUP_COUNTRY.COUNTRY_ID].ToString(),
                                 COUNTRY_NAME = dr[Common.SUP_COUNTRY.COUNTRY_NAME].ToString()
                             }).ToList();
                objStuAddress.C_COUNTRY = new SelectList(liCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);
                objStuAddress.P_COUNTRY = new SelectList(liCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);
            }
            return View(objStuAddress);
        }

        // Insert Address Details ..
        public JsonResult InsertStuAddress(string JsonAddress)
        {
            if (JsonAddress != null)
            {
                string sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
                var Result = JsonConvert.DeserializeObject<StudentAddress>(JsonAddress);
                dv.Clear();
                dv.Add(Common.STU_ADDRESS.C_DOOR_DETAIL, Result.CDoorDetail, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_STREET, Result.CStreet, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_VILLAGE_AREA, Result.CVillage, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_POST_PLACE_TOWN, Result.CPostPlace, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_TALUK, Result.CTaluk, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_CITY, Result.CCity, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_DISTRICT, Result.CDistrict, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_PINCODE, Result.Cpincode, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_COUNTRY, Result.CCountry, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_DOOR_DETAIL, Result.PDoorDetails, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_STREET, Result.PStreet, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_VILLAGE_AREA, Result.PVillage, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_POST_PLACE_TOWN, Result.PPostPlace, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_TALUK, Result.PTaluk, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_CITY, Result.PCity, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_DISTRICT, Result.PDistrict, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_PINCODE, Result.PPincode, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_COUNTRY, Result.PCountry, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                using (StudentViewModel objStudent = new StudentViewModel())
                {

                    ObjStudent.STUDENT_ID = sStudentID;
                    sSql = StudentSQL.GetStudentSQL(StudentSQLCommads.IfStudentExitsByMultiTable);
                    sSql = sSql.Replace(Common.Delimiter.QUS + "TABLE_NAME", "STU_ADDRESS");
                    Exits = (List<IF_STUDENT_EXITS>)CMSHandler.FetchData<IF_STUDENT_EXITS>(ObjStudent, sSql).DataSource.Data;
                    if (Exits == null)
                    {
                        resultArgs = objStudent.InsertStudentAddressDetails(dv);
                        sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                    }
                    else
                    {
                        sResult = Common.ErrorCode.Created;
                    }


                }
            }
            return Json(sResult);
        }

        // FetchAddress Details ...
        public JsonResult FetchAddress()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STU_ADDRESS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
            DataTable dtAddress = new DataTable();
            StudentViewModel objViewModel = new StudentViewModel();
            dtAddress = objViewModel.FetchAddress(dv).DataSource.Table;
            if (dtAddress != null && dtAddress.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtAddress.Rows[0][Common.STU_ADDRESS.ADDRESS_ID].ToString();
                StudentAddress objAddress = new StudentAddress()
                {
                    CDoorDetail = dtAddress.Rows[0][Common.STU_ADDRESS.C_DOOR_DETAIL].ToString(),
                    CCity = dtAddress.Rows[0][Common.STU_ADDRESS.C_CITY].ToString(),
                    CCountry = dtAddress.Rows[0][Common.STU_ADDRESS.C_COUNTRY].ToString(),
                    CDistrict = dtAddress.Rows[0][Common.STU_ADDRESS.C_DISTRICT].ToString(),
                    Cpincode = dtAddress.Rows[0][Common.STU_ADDRESS.C_PINCODE].ToString(),
                    CPostPlace = dtAddress.Rows[0][Common.STU_ADDRESS.C_POST_PLACE_TOWN].ToString(),
                    CStreet = dtAddress.Rows[0][Common.STU_ADDRESS.C_STREET].ToString(),
                    CTaluk = dtAddress.Rows[0][Common.STU_ADDRESS.C_TALUK].ToString(),
                    CVillage = dtAddress.Rows[0][Common.STU_ADDRESS.C_VILLAGE_AREA].ToString(),
                    PDoorDetails = dtAddress.Rows[0][Common.STU_ADDRESS.P_DOOR_DETAIL].ToString(),
                    PCity = dtAddress.Rows[0][Common.STU_ADDRESS.P_CITY].ToString(),
                    PCountry = dtAddress.Rows[0][Common.STU_ADDRESS.P_COUNTRY].ToString(),
                    PDistrict = dtAddress.Rows[0][Common.STU_ADDRESS.P_DISTRICT].ToString(),
                    PPincode = dtAddress.Rows[0][Common.STU_ADDRESS.P_PINCODE].ToString(),
                    PPostPlace = dtAddress.Rows[0][Common.STU_ADDRESS.P_POST_PLACE_TOWN].ToString(),
                    PStreet = dtAddress.Rows[0][Common.STU_ADDRESS.P_STREET].ToString(),
                    PTaluk = dtAddress.Rows[0][Common.STU_ADDRESS.P_TALUK].ToString(),
                    PVillage = dtAddress.Rows[0][Common.STU_ADDRESS.P_VILLAGE_AREA].ToString()
                };
                return Json(objAddress);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Edit Student Address UI ...

        public ActionResult EditStuAddressInfo()
        {
            StuAddressInfo objStuAddress = new StuAddressInfo();
            //Country ...
            DataTable dtCountry = new DataTable();
            dtCountry = SupportDataMethod.FetchCountry().DataSource.Table;
            List<SUP_COUNTRY> liCountry = new List<SUP_COUNTRY>();
            if (dtCountry != null && dtCountry.Rows.Count > 0)
            {
                liCountry = (from DataRow dr in dtCountry.Rows
                             select new SUP_COUNTRY()
                             {
                                 COUNTRY_ID = dr[Common.SUP_COUNTRY.COUNTRY_ID].ToString(),
                                 COUNTRY_NAME = dr[Common.SUP_COUNTRY.COUNTRY_NAME].ToString()
                             }).ToList();
                objStuAddress.C_COUNTRY = new SelectList(liCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);
                objStuAddress.P_COUNTRY = new SelectList(liCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);
            }
            return View(objStuAddress);
        }

        // Edit Address ... 
        public JsonResult UpdateAddress(string JsonAddress)
        {
            string sAddressID = string.Empty;
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {

                var Result = JsonConvert.DeserializeObject<StudentAddress>(JsonAddress);
                dv.Clear();
                dv.Add(Common.STU_ADDRESS.C_DOOR_DETAIL, Result.CDoorDetail, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_STREET, Result.CStreet, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_VILLAGE_AREA, Result.CVillage, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_POST_PLACE_TOWN, Result.CPostPlace, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_TALUK, Result.CTaluk, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_CITY, Result.CCity, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_DISTRICT, Result.CDistrict, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_PINCODE, Result.Cpincode, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.C_COUNTRY, Result.CCountry, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_DOOR_DETAIL, Result.PDoorDetails, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_STREET, Result.PStreet, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_VILLAGE_AREA, Result.PVillage, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_POST_PLACE_TOWN, Result.PPostPlace, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_TALUK, Result.PTaluk, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_CITY, Result.PCity, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_DISTRICT, Result.PDistrict, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_PINCODE, Result.PPincode, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.P_COUNTRY, Result.PCountry, EnumCommand.DataType.String);
                dv.Add(Common.STU_ADDRESS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    sAddressID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STU_ADDRESS.ADDRESS_ID, sAddressID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateAddress(dv);
                        sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                    }
                }
                else
                {
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertStudentAddressDetails(dv);
                        sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                    }
                }
            }
            return Json(sResult);
        }
        #endregion

        #region Parents Information
        // Add parents UI ...

        public ActionResult AddStuParentsInfo()
        {
            StuParentsInfo objParentsInfo = new StuParentsInfo();
            //Nationality ...
            DataTable dtNationality = new DataTable();
            dtNationality = SupportDataMethod.FetchNationality().DataSource.Table;
            List<SupNationality> liNationality = new List<SupNationality>();
            if (dtNationality != null && dtNationality.Rows.Count > 0)
            {
                liNationality = (from DataRow dr in dtNationality.Rows
                                 select new SupNationality()
                                 {
                                     NATIONALITY_ID = dr[Common.SUP_NATIONALITY.NATIONALITY_ID].ToString(),
                                     NATIONALITY_NAME = dr[Common.SUP_NATIONALITY.NATIONALITY_NAME].ToString()
                                 }).ToList();
                objParentsInfo.MO_NATIONALITY = new SelectList(liNationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);
                objParentsInfo.FR_NATIONALITY = new SelectList(liNationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);
            }
            //Occupation ...
            DataTable dtOccupation = new DataTable();
            dtOccupation = SupportDataMethod.Occupation().DataSource.Table;
            List<Sup_Occupation> liOccupation = new List<Sup_Occupation>();
            if (dtOccupation != null && dtOccupation.Rows.Count > 0)
            {
                liOccupation = (from DataRow dr in dtOccupation.Rows
                                select new Sup_Occupation()
                                {
                                    OCCUPATION_ID = dr[Common.SUP_OCCUPATION.OCCUPATION_ID].ToString(),
                                    OCCUPATION_NAME = dr[Common.SUP_OCCUPATION.OCCUPATION_NAME].ToString()
                                }).ToList();
                objParentsInfo.FR_OCCUPATION = new SelectList(liOccupation, Common.SUP_OCCUPATION.OCCUPATION_ID, Common.SUP_OCCUPATION.OCCUPATION_NAME);
                objParentsInfo.MO_OCCUPATION = new SelectList(liOccupation, Common.SUP_OCCUPATION.OCCUPATION_ID, Common.SUP_OCCUPATION.OCCUPATION_NAME);
            }
            //Education ...
            DataTable dtEducation = new DataTable();
            dtEducation = SupportDataMethod.Education().DataSource.Table;
            List<Sup_Education> liEducation = new List<Sup_Education>();
            if (dtEducation != null && dtEducation.Rows.Count > 0)
            {
                liEducation = (from DataRow dr in dtEducation.Rows
                               select new Sup_Education()
                               {
                                   EDUCATION_ID = dr[Common.SUP_EDUCATION.EDUCATION_ID].ToString(),
                                   EDUCATION_NAME = dr[Common.SUP_EDUCATION.EDUCATION_NAME].ToString()
                               }).ToList();
                objParentsInfo.FR_EDUCATION = new SelectList(liEducation, Common.SUP_EDUCATION.EDUCATION_ID, Common.SUP_EDUCATION.EDUCATION_NAME);
                objParentsInfo.MO_EDUCATION = new SelectList(liEducation, Common.SUP_EDUCATION.EDUCATION_ID, Common.SUP_EDUCATION.EDUCATION_NAME);
            }

            return View(objParentsInfo);
        }

        // Insert Parents Details ..
        public JsonResult InsertParentsDetails(string JsonParents)
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<ParentsDetails>(JsonParents);
                string sFDateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.FDateOfBirth);
                string sFDateOfExpire = CommonMethods.ConvertdatetoyearFromat(Result.FDateOfExpire);
                string sMDateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.MDateOfBirth);
                string sMDateOfExpire = CommonMethods.ConvertdatetoyearFromat(Result.MDateOfExpire);
                dv.Clear();
                dv.Add(Common.STU_PERSONAL_INFO.FATHER_NAME, Result.FatherName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.F_DATE_OF_BIRTH, sFDateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_OCCUPATION, Result.FOccupation, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FATHER_EDUCATION, Result.FEducation, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_NATIONALITY, Result.FNationality, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_BUSINESS_PHONE, Result.fBusinessContact, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_MOBILE, Result.FMobile, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.FR_PASSPORT_number, Result.FPassport, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_INCOME, Result.FIncome, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_DATE_OF_EXPIRY, sFDateOfExpire, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_EMAIL, Result.FEmail, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MOTHER_NAME, Result.MotherName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.M_DATE_OF_BIRTH, sMDateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_OCCUPATION, Result.MOccupation, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MOTHER_EDUCATION, Result.MEducation, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_NATIONALITY, Result.MNationality, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_BUSINESS_PHONE, Result.mBusinessContact, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_MOBILE, Result.MMobile, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.MO_PASSPORT_number, Result.MPassport, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_INCOME, Result.MIncome, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_DATE_OF_EXPIRY, sMDateOfExpire, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_EMAIL, Result.MEmail, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);

                using (StudentViewModel objViewModel = new StudentViewModel())
                {

                    resultArgs = objViewModel.InsertParentsInfo(dv);
                    sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                }
            }
            return Json(sResult);
        }

        // Fetch Parents Details ...
        public JsonResult FetchParentsDetails()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
            DataTable dtFetchParentsDetails = new DataTable();
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtFetchParentsDetails = objViewModel.ParentsInfo(dv).DataSource.Table;
            }
            if (dtFetchParentsDetails != null && dtFetchParentsDetails.Rows.Count > 0)
            {
                ParentsDetails objParents = new ParentsDetails()
                {
                    FatherName = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FATHER_NAME].ToString(),
                    FDateOfBirth = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.F_DATE_OF_BIRTH].ToString(),
                    fBusinessContact = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FR_BUSINESS_PHONE].ToString(),
                    FDateOfExpire = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FR_DATE_OF_EXPIRY].ToString(),
                    FEducation = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FATHER_EDUCATION].ToString(),
                    FEmail = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FR_EMAIL].ToString(),
                    FIncome = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FR_INCOME].ToString(),
                    FMobile = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FR_MOBILE].ToString(),
                    FNationality = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FR_NATIONALITY].ToString(),
                    FOccupation = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FR_OCCUPATION].ToString(),
                    FPassport = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FR_PASSPORT_number].ToString(),
                    // FPhoto = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.FATHER_PHOTO].ToString(),
                    MotherName = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MOTHER_NAME].ToString(),
                    mBusinessContact = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MO_BUSINESS_PHONE].ToString(),
                    MDateOfBirth = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.M_DATE_OF_BIRTH].ToString(),
                    MDateOfExpire = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MO_DATE_OF_EXPIRY].ToString(),
                    MEducation = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MOTHER_EDUCATION].ToString(),
                    MEmail = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MO_EMAIL].ToString(),
                    MIncome = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MO_INCOME].ToString(),
                    MMobile = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MO_MOBILE].ToString(),
                    MNationality = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MO_NATIONALITY].ToString(),
                    MOccupation = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MO_OCCUPATION].ToString(),
                    MPassport = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MO_PASSPORT_number].ToString(),
                    //MPhoto = dtFetchParentsDetails.Rows[0][Common.STU_PERSONAL_INFO.MOTHER_PHOTO].ToString(),

                };
                return Json(objParents);
            }
            else
            {
                return Json(sResult);
            }
        }

        // Edit parents UI ...

        public ActionResult EditStuParentsInfo()
        {
            StuParentsInfo objParentsInfo = new StuParentsInfo();
            //Nationality ...
            DataTable dtNationality = new DataTable();
            dtNationality = SupportDataMethod.FetchNationality().DataSource.Table;
            List<SupNationality> liNationality = new List<SupNationality>();
            if (dtNationality != null && dtNationality.Rows.Count > 0)
            {
                liNationality = (from DataRow dr in dtNationality.Rows
                                 select new SupNationality()
                                 {
                                     NATIONALITY_ID = dr[Common.SUP_NATIONALITY.NATIONALITY_ID].ToString(),
                                     NATIONALITY_NAME = dr[Common.SUP_NATIONALITY.NATIONALITY_NAME].ToString()
                                 }).ToList();
                objParentsInfo.MO_NATIONALITY = new SelectList(liNationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);
                objParentsInfo.FR_NATIONALITY = new SelectList(liNationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);
            }
            //Occupation ...
            DataTable dtOccupation = new DataTable();
            dtOccupation = SupportDataMethod.Occupation().DataSource.Table;
            List<Sup_Occupation> liOccupation = new List<Sup_Occupation>();
            if (dtOccupation != null && dtOccupation.Rows.Count > 0)
            {
                liOccupation = (from DataRow dr in dtOccupation.Rows
                                select new Sup_Occupation()
                                {
                                    OCCUPATION_ID = dr[Common.SUP_OCCUPATION.OCCUPATION_ID].ToString(),
                                    OCCUPATION_NAME = dr[Common.SUP_OCCUPATION.OCCUPATION_NAME].ToString()
                                }).ToList();
                objParentsInfo.FR_OCCUPATION = new SelectList(liOccupation, Common.SUP_OCCUPATION.OCCUPATION_ID, Common.SUP_OCCUPATION.OCCUPATION_NAME);
                objParentsInfo.MO_OCCUPATION = new SelectList(liOccupation, Common.SUP_OCCUPATION.OCCUPATION_ID, Common.SUP_OCCUPATION.OCCUPATION_NAME);
            }
            //Education ...
            DataTable dtEducation = new DataTable();
            dtEducation = SupportDataMethod.Education().DataSource.Table;
            List<Sup_Education> liEducation = new List<Sup_Education>();
            if (dtEducation != null && dtEducation.Rows.Count > 0)
            {
                liEducation = (from DataRow dr in dtEducation.Rows
                               select new Sup_Education()
                               {
                                   EDUCATION_ID = dr[Common.SUP_EDUCATION.EDUCATION_ID].ToString(),
                                   EDUCATION_NAME = dr[Common.SUP_EDUCATION.EDUCATION_NAME].ToString()
                               }).ToList();
                objParentsInfo.FR_EDUCATION = new SelectList(liEducation, Common.SUP_EDUCATION.EDUCATION_ID, Common.SUP_EDUCATION.EDUCATION_NAME);
                objParentsInfo.MO_EDUCATION = new SelectList(liEducation, Common.SUP_EDUCATION.EDUCATION_ID, Common.SUP_EDUCATION.EDUCATION_NAME);
            }
            return View(objParentsInfo);
        }

        // Edit Parents Details ...
        public JsonResult UpdateParentInfo(string JsonParents)
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<ParentsDetails>(JsonParents);
                string sFDateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.FDateOfBirth);
                string sFDateOfExpire = CommonMethods.ConvertdatetoyearFromat(Result.FDateOfExpire);
                string sMDateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.MDateOfBirth);
                string sMDateOfExpire = CommonMethods.ConvertdatetoyearFromat(Result.MDateOfExpire);
                dv.Clear();
                dv.Add(Common.STU_PERSONAL_INFO.FATHER_NAME, Result.FatherName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.F_DATE_OF_BIRTH, sFDateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_OCCUPATION, Result.FOccupation, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FATHER_EDUCATION, Result.FEducation, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_NATIONALITY, Result.FNationality, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_BUSINESS_PHONE, Result.fBusinessContact, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_MOBILE, Result.FMobile, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.FR_PASSPORT_number, Result.FPassport, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_INCOME, Result.FIncome, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_DATE_OF_EXPIRY, sFDateOfExpire, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FR_EMAIL, Result.FEmail, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MOTHER_NAME, Result.MotherName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.M_DATE_OF_BIRTH, sMDateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_OCCUPATION, Result.MOccupation, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MOTHER_EDUCATION, Result.MEducation, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_NATIONALITY, Result.MNationality, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_BUSINESS_PHONE, Result.mBusinessContact, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_MOBILE, Result.MMobile, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.MO_PASSPORT_number, Result.MPassport, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_INCOME, Result.MIncome, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_DATE_OF_EXPIRY, sMDateOfExpire, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.MO_EMAIL, Result.MEmail, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);

                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.UpdateParentsDetails(dv);
                    sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                }
            }
            return Json(sResult);
        }
        #endregion

        #region Privious Qualification
        // Add Privious Qualification UI ..

        public ActionResult AddStuPriviousQualificationInfo()
        {
            StudentPriviousQualification objQualification = new StudentPriviousQualification();
            DataTable dtExamPassed = new DataTable();
            dtExamPassed = SupportDataMethod.FetchExamPassed().DataSource.Table;
            List<SUP_EXAMPASSED> liExamPassed = new List<SUP_EXAMPASSED>();
            if (dtExamPassed != null && dtExamPassed.Rows.Count > 0)
            {
                liExamPassed = (from DataRow dr in dtExamPassed.Rows
                                select new SUP_EXAMPASSED()
                                {
                                    EXAM_PASSED_ID = dr[Common.SUP_EXAMPASSED.EXAM_PASSED_ID].ToString(),
                                    EXAM_PASSED = dr[Common.SUP_EXAMPASSED.EXAM_PASSED].ToString()
                                }).ToList();
                objQualification.EXAM_PASSED = new SelectList(liExamPassed, Common.SUP_EXAMPASSED.EXAM_PASSED_ID, Common.SUP_EXAMPASSED.EXAM_PASSED);
            }
            return View(objQualification);
        }

        // Edit Privious Qualification UI ..

        public ActionResult EditStuPriviousQualificationInfo()
        {
            StudentPriviousQualification objQualification = new StudentPriviousQualification();
            DataTable dtExamPassed = new DataTable();
            dtExamPassed = SupportDataMethod.FetchExamPassed().DataSource.Table;
            List<SUP_EXAMPASSED> liExamPassed = new List<SUP_EXAMPASSED>();
            if (dtExamPassed != null && dtExamPassed.Rows.Count > 0)
            {
                liExamPassed = (from DataRow dr in dtExamPassed.Rows
                                select new SUP_EXAMPASSED()
                                {
                                    EXAM_PASSED_ID = dr[Common.SUP_EXAMPASSED.EXAM_PASSED_ID].ToString(),
                                    EXAM_PASSED = dr[Common.SUP_EXAMPASSED.EXAM_PASSED].ToString()
                                }).ToList();
                objQualification.EXAM_PASSED = new SelectList(liExamPassed, Common.SUP_EXAMPASSED.EXAM_PASSED_ID, Common.SUP_EXAMPASSED.EXAM_PASSED);
            }
            return View(objQualification);
        }

        // Fetch Student Privious Qualification ...
        public JsonResult FetchPriviousQualification()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
            DataTable dtPriviousQualification = new DataTable();
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtPriviousQualification = objViewModel.FetchPriviousQualification(dv).DataSource.Table;
            }
            if (dtPriviousQualification != null && dtPriviousQualification.Rows.Count > 0)
            {
                PriviousQualification objModel = new PriviousQualification()
                {
                    LSchool = dtPriviousQualification.Rows[0][Common.STU_PERSONAL_INFO.LAST_SCHOOL_OR_COLLEGE].ToString(),
                    LStudied = dtPriviousQualification.Rows[0][Common.STU_PERSONAL_INFO.LAST_STUDIED_PLACE].ToString(),
                    LstudiedClass = dtPriviousQualification.Rows[0][Common.STU_PERSONAL_INFO.LAST_STUDIED_CLASS].ToString(),
                    ExamPassed = dtPriviousQualification.Rows[0][Common.STU_PERSONAL_INFO.EXAM_PASSED].ToString(),
                };
                return Json(objModel);
            }
            return Json(sResult);
        }

        // Edit Student Privious Qualification ..
        public JsonResult UpdatePriviousQualification(string JsonPriviousQualification)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null)
            {
                sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
            }
            else
            {
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            }

            if (JsonPriviousQualification != null)
            {
                var Result = JsonConvert.DeserializeObject<PriviousQualification>(JsonPriviousQualification);
                dv.Clear();
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.LAST_SCHOOL_OR_COLLEGE, Result.LSchool, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.LAST_STUDIED_CLASS, Result.LstudiedClass, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.LAST_STUDIED_PLACE, Result.LStudied, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.EXAM_PASSED, Result.ExamPassed, EnumCommand.DataType.String);
                using (StudentViewModel objModel = new StudentViewModel())
                {
                    resultArgs = objModel.UpdatepriviousQualification(dv);
                    if (resultArgs.Success)
                    {
                        sResult = "Record Updated Successfully ...!";
                    }
                    else
                    {
                        sResult = "Record not updated try again ...!";
                    }
                }
            }
            return Json(sResult);
        }
        #endregion

        #region Activity Information
        // Add Student Activity UI ...

        public ActionResult AddStuActivityInfo()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_ACTIVITY.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    DataTable dtActivity = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtActivity = objViewModel.BindActivity(dv).DataSource.Table;
                    }
                    IList<StudentActivity> liStuActivity = new List<StudentActivity>();
                    if (dtActivity != null && dtActivity.Rows.Count > 0)
                    {
                        liStuActivity = (from DataRow dr in dtActivity.Rows
                                         select new StudentActivity
                                         {
                                             ActivityId = dr[Common.STU_ACTIVITY.ACTIVITY_ID].ToString(),
                                             DateFrom = dr[Common.STU_ACTIVITY.DATE_FROM].ToString(),
                                             DateTo = dr[Common.STU_ACTIVITY.DATE_TO].ToString(),
                                             ExtraActivity = dr[Common.STU_ACTIVITY.EXTRA_ACTIVITY].ToString(),
                                             InitiativeShown = dr[Common.STU_ACTIVITY.INITIATIVE_SHOWN].ToString(),
                                             positionObtained = dr[Common.STU_ACTIVITY.POSITION_OBTAINED].ToString(),
                                             PostHeld = dr[Common.STU_ACTIVITY.POST_HELD].ToString(),
                                             Participation = dr[Common.STU_ACTIVITY.PARTICIPATION].ToString(),
                                             StuActivity = dr[Common.SUP_ACTIVITY.ACTIVITY_NAME].ToString(),
                                             ActivityImg1 = dr[Common.STU_ACTIVITY.ACTIVITY_IMAGE1].ToString(),
                                             ActivityImg2 = dr[Common.STU_ACTIVITY.ACTIVITY_IMAGE2].ToString(),
                                             ActivityImg3 = dr[Common.STU_ACTIVITY.ACTIVITY_IMAGE3].ToString()
                                         }).ToList();
                        return View(liStuActivity);
                    }
                    else
                    {
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "AddStuActivityInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "AddStuActivityInfo", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult);
            }
        }

        // Insert Student Activity ...
        public JsonResult InsertStudentActivity(string str)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            }
            else
            {
                sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            }

            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    var Result = JsonConvert.DeserializeObject<StudentActivity>(str);
                    string DateFrom = CommonMethods.ConvertdatetoyearFromat(Result.DateFrom);
                    string DateTo = CommonMethods.ConvertdatetoyearFromat(Result.DateTo);
                    dv.Clear();
                    dv.Add(Common.STU_ACTIVITY.POST_HELD, "0", EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.DATE_FROM, DateFrom, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.DATE_TO, DateTo, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.EXTRA_ACTIVITY, Result.ExtraActivity, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.ACTIVITY, Result.StuActivity, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.INITIATIVE_SHOWN, Result.InitiativeShown, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.POSITION_OBTAINED, "0", EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.PARTICIPATION, Result.Participation, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.ACTIVITY_IMAGE1, Result.ActivityImg1, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.ACTIVITY_IMAGE2, Result.ActivityImg2, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.ACTIVITY_IMAGE3, Result.ActivityImg3, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ACTIVITY.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    using (StudentViewModel objStudent = new StudentViewModel())
                    {
                        resultArgs = objStudent.InsertStuActivity(dv);
                        sResult = (resultArgs.Success) ? "Record saved successfully ...!" : "Record not saved try again ...!";
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "InsertStudentActivity", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "InsertStudentActivity", ex.Message);
                    }
                }
            }
            else
            {
                sResult = "Session Has Expired Login Again ...!";
            }
            return Json(sResult);
        }

        // Fetch StudentActivity ...
        public JsonResult FetchActivity(string sStudentID)
        {
            dv.Clear();
            dv.Add(Common.STU_ACTIVITY.ACTIVITY_ID, sStudentID, EnumCommand.DataType.String);
            DataTable dtActivity = new DataTable();
            StudentViewModel objViewModel = new StudentViewModel();
            dtActivity = objViewModel.FetchActivity(dv).DataSource.Table;
            if (dtActivity != null && dtActivity.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtActivity.Rows[0][Common.STU_ACTIVITY.ACTIVITY_ID].ToString();
                StudentActivity objActivity = new StudentActivity()
                {
                    DateFrom = dtActivity.Rows[0][Common.STU_ACTIVITY.DATE_FROM].ToString(),
                    DateTo = dtActivity.Rows[0][Common.STU_ACTIVITY.DATE_TO].ToString(),
                    StuActivity = dtActivity.Rows[0][Common.STU_ACTIVITY.ACTIVITY].ToString(),
                    ExtraActivity = dtActivity.Rows[0][Common.STU_ACTIVITY.EXTRA_ACTIVITY].ToString(),
                    positionObtained = dtActivity.Rows[0][Common.STU_ACTIVITY.POSITION_OBTAINED].ToString(),
                    PostHeld = dtActivity.Rows[0][Common.STU_ACTIVITY.POST_HELD].ToString(),
                    InitiativeShown = dtActivity.Rows[0][Common.STU_ACTIVITY.INITIATIVE_SHOWN].ToString(),
                    Participation = dtActivity.Rows[0][Common.STU_ACTIVITY.PARTICIPATION].ToString(),
                    ActivityImg1 = dtActivity.Rows[0][Common.STU_ACTIVITY.ACTIVITY_IMAGE1].ToString(),
                    ActivityImg2 = dtActivity.Rows[0][Common.STU_ACTIVITY.ACTIVITY_IMAGE2].ToString(),
                    ActivityImg3 = dtActivity.Rows[0][Common.STU_ACTIVITY.ACTIVITY_IMAGE3].ToString()
                };
                return Json(objActivity);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }

        }

        // Bind DropDown List ...
        public string BindActivityDDL()
        {
            string sOption = string.Empty;
            DataTable dtActivity = new DataTable();
            dtActivity = SupportDataMethod.FetchActvity().DataSource.Table;
            if (dtActivity != null && dtActivity.Rows.Count > 0)
            {
                foreach (DataRow item in dtActivity.Rows)
                {
                    sOption += "<option value='" + item[Common.SUP_ACTIVITY.ACTIVITY_ID].ToString() + "'>" + item[Common.SUP_ACTIVITY.ACTIVITY_NAME] + "</option>";

                }
            }
            return sOption;
        }

        // Edit Student Activity UI ...

        public ActionResult EditStuActivityInfo()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                dv.Clear();
                dv.Add(Common.STU_ACTIVITY.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                DataTable dtActivity = new DataTable();
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    dtActivity = objViewModel.BindActivity(dv).DataSource.Table;
                }
                IList<StudentActivity> liStuActivity = new List<StudentActivity>();
                if (dtActivity != null && dtActivity.Rows.Count > 0)
                {
                    liStuActivity = (from DataRow dr in dtActivity.Rows
                                     select new StudentActivity
                                     {
                                         ActivityId = dr[Common.STU_ACTIVITY.ACTIVITY_ID].ToString(),
                                         DateFrom = dr[Common.STU_ACTIVITY.DATE_FROM].ToString(),
                                         DateTo = dr[Common.STU_ACTIVITY.DATE_TO].ToString(),
                                         ExtraActivity = dr[Common.STU_ACTIVITY.EXTRA_ACTIVITY].ToString(),
                                         InitiativeShown = dr[Common.STU_ACTIVITY.INITIATIVE_SHOWN].ToString(),
                                         positionObtained = dr[Common.STU_ACTIVITY.POSITION_OBTAINED].ToString(),
                                         PostHeld = dr[Common.STU_ACTIVITY.POST_HELD].ToString(),
                                         Participation = dr[Common.STU_ACTIVITY.PARTICIPATION].ToString(),
                                         StuActivity = dr[Common.SUP_ACTIVITY.ACTIVITY_NAME].ToString(),
                                         ActivityImg1 = dr[Common.STU_ACTIVITY.ACTIVITY_IMAGE1].ToString(),
                                         ActivityImg2 = dr[Common.STU_ACTIVITY.ACTIVITY_IMAGE2].ToString(),
                                         ActivityImg3 = dr[Common.STU_ACTIVITY.ACTIVITY_IMAGE3].ToString()
                                     }).ToList();
                    return View(liStuActivity);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult);
            }
        }

        // Edit Student Activity ...
        public JsonResult EditActivity(string str)
        {
            string sActivityID = string.Empty;
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null)
                sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentActivity>(str);
                string DateFrom = CommonMethods.ConvertdatetoyearFromat(Result.DateFrom);
                string DateTo = CommonMethods.ConvertdatetoyearFromat(Result.DateTo);
                dv.Clear();
                dv.Add(Common.STU_ACTIVITY.DATE_FROM, DateFrom, EnumCommand.DataType.String);
                dv.Add(Common.STU_ACTIVITY.DATE_TO, DateTo, EnumCommand.DataType.String);
                dv.Add(Common.STU_ACTIVITY.EXTRA_ACTIVITY, Result.ExtraActivity, EnumCommand.DataType.String);
                dv.Add(Common.STU_ACTIVITY.INITIATIVE_SHOWN, Result.InitiativeShown, EnumCommand.DataType.String);
                dv.Add(Common.STU_ACTIVITY.PARTICIPATION, Result.Participation, EnumCommand.DataType.String);
                dv.Add(Common.STU_ACTIVITY.POSITION_OBTAINED, "0", EnumCommand.DataType.String);
                dv.Add(Common.STU_ACTIVITY.POST_HELD, "0", EnumCommand.DataType.String);
                dv.Add(Common.STU_ACTIVITY.ACTIVITY, Result.StuActivity, EnumCommand.DataType.String);
                dv.Add(Common.STU_ACTIVITY.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    sActivityID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STU_ACTIVITY.ACTIVITY_ID, sActivityID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateActivity(dv);
                        sResult = (resultArgs.Success) ? "Record updated successfully ....!" : "Record not updated try again....!";
                    }
                }
                else
                {
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertStuActivity(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record Insertd successfully ....!";
                        }
                        else
                        {
                            sResult = "Record not Insertd try again....!";
                        }
                    }
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult);
            }
            return Json(sResult);
        }

        // Delete Student Activity ..
        public JsonResult DeleteActivity(string sActivityId)
        {
            dv.Clear();
            dv.Add(Common.STU_ACTIVITY.ACTIVITY_ID, sActivityId, EnumCommand.DataType.String);
            dv.Add(Common.STU_ACTIVITY.IS_DELETED, "1", EnumCommand.DataType.String);
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                resultArgs = objViewModel.DeleteActivity(dv);
                sResult = (resultArgs.Success) ? "Record deleted successfully ..!" : "Record not deleted try again..!";
            }
            return Json(sResult);
        }
        #endregion

        #region Certificate Information
        // Add Student Cirtificate UI ...

        public ActionResult AddStuCertificateInfo()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                dv.Clear();
                dv.Add(Common.STU_ADDRESS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                DataTable dtCertificate = new DataTable();
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    dtCertificate = objViewModel.FetchCertificate(dv).DataSource.Table;
                }
                IList<StudentCertificate> liCertificate = new List<StudentCertificate>();
                if (dtCertificate != null && dtCertificate.Rows.Count > 0)
                {
                    liCertificate = (from DataRow dr in dtCertificate.Rows
                                     select new StudentCertificate
                                     {
                                         CeretificateID = dr[Common.STU_CERTIFICATE.CERTIFICATE_ID].ToString(),
                                         Archive = dr[Common.SUP_ACHIEVEMENT.ACHIEVEMENT_NAME].ToString(),
                                         CertificateName = dr[Common.STU_CERTIFICATE.CERTIFICATE_NAME].ToString(),
                                         CertificateNo = dr[Common.STU_CERTIFICATE.CERTIFICATE_NO].ToString(),
                                         GivenDate = dr[Common.STU_CERTIFICATE.GIVEN_DATE].ToString(),
                                         Purpose = dr[Common.STU_CERTIFICATE.PURPOSE].ToString(),
                                         RecieveDate = dr[Common.STU_CERTIFICATE.RECEIVED_DATE].ToString(),
                                         RegisterNo = dr[Common.STU_CERTIFICATE.REGISTER_NUMBER].ToString()
                                     }).ToList();
                    return View(liCertificate);
                }
                else
                    return View();
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult);
            }
        }

        // Insert Student Certificate ...
        public JsonResult InsertStudentCertificate(string str)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentCertificate>(str);
                string ReceiveDate = CommonMethods.ConvertdatetoyearFromat(Result.RecieveDate);
                string GivenDate = CommonMethods.ConvertdatetoyearFromat(Result.GivenDate);
                dv.Clear();
                dv.Add(Common.STU_CERTIFICATE.CERTIFICATE_NAME, Result.CertificateName, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.CERTIFICATE_NO, Result.CertificateNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.RECEIVED_DATE, ReceiveDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.GIVEN_DATE, GivenDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.PURPOSE, Result.Purpose, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.REGISTER_NUMBER, Result.RegisterNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.ARCHIVE, Result.Archive, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                using (StudentViewModel objStudent = new StudentViewModel())
                {
                    resultArgs = objStudent.InsertStuCertificate(dv);
                    sResult = (resultArgs.Success) ? "Record saved successfully ...!" : "Record not saved try again ...!";
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
            }
            return Json(sResult);
        }

        // Fetch Student Certificate ...
        public JsonResult FetchCertificate(string sStudentID)
        {
            dv.Clear();
            dv.Add(Common.STU_ADDRESS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
            DataTable dtCertificate = new DataTable();
            StudentViewModel objViewModel = new StudentViewModel();
            dtCertificate = objViewModel.BindCertificate(dv).DataSource.Table;
            if (dtCertificate != null && dtCertificate.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtCertificate.Rows[0][Common.STU_CERTIFICATE.CERTIFICATE_ID].ToString();
                StudentCertificate objCertificate = new StudentCertificate()
                {
                    Archive = dtCertificate.Rows[0][Common.STU_CERTIFICATE.ARCHIVE].ToString(),
                    CertificateName = dtCertificate.Rows[0][Common.STU_CERTIFICATE.CERTIFICATE_NAME].ToString(),
                    CertificateNo = dtCertificate.Rows[0][Common.STU_CERTIFICATE.CERTIFICATE_NO].ToString(),
                    GivenDate = dtCertificate.Rows[0][Common.STU_CERTIFICATE.GIVEN_DATE].ToString(),
                    Purpose = dtCertificate.Rows[0][Common.STU_CERTIFICATE.PURPOSE].ToString(),
                    RecieveDate = dtCertificate.Rows[0][Common.STU_CERTIFICATE.RECEIVED_DATE].ToString(),
                    RegisterNo = dtCertificate.Rows[0][Common.STU_CERTIFICATE.REGISTER_NUMBER].ToString()
                };
                return Json(objCertificate);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                sResult = Common.Messages.NoRecordsfound;
                return Json(sResult);
            }
        }

        // Bind DDL Achivement ..
        public string BindDDLAchivement()
        {
            string sOption = string.Empty;
            DataTable dtAchievement = new DataTable();
            dtAchievement = SupportDataMethod.FetchAchievement().DataSource.Table;
            if (dtAchievement != null && dtAchievement.Rows.Count > 0)
            {
                foreach (DataRow item in dtAchievement.Rows)
                {
                    sOption += "<option value='" + item[Common.SUP_ACHIEVEMENT.ACHIEVEMENT_ID].ToString() + "'>" + item[Common.SUP_ACHIEVEMENT.ACHIEVEMENT_NAME].ToString() + "</option>";
                }
            }
            return sOption;
        }

        // Bind Achivement ...
        public JsonResult BindCertificate(string sCertificateId)
        {
            dv.Clear();
            dv.Add(Common.STU_CERTIFICATE.CERTIFICATE_ID, sCertificateId, EnumCommand.DataType.String);
            DataTable dtCertificate = new DataTable();
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtCertificate = objViewModel.BindCertificate(dv).DataSource.Table;
            }
            if (dtCertificate != null && dtCertificate.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtCertificate.Rows[0][Common.STU_CERTIFICATE.CERTIFICATE_ID].ToString();
                StudentCertificate objStuCertificate = new StudentCertificate()
                {
                    Archive = dtCertificate.Rows[0][Common.STU_CERTIFICATE.ARCHIVE].ToString(),
                    CeretificateID = dtCertificate.Rows[0][Common.STU_CERTIFICATE.CERTIFICATE_ID].ToString(),
                    CertificateName = dtCertificate.Rows[0][Common.STU_CERTIFICATE.CERTIFICATE_NAME].ToString(),
                    CertificateNo = dtCertificate.Rows[0][Common.STU_CERTIFICATE.CERTIFICATE_NO].ToString(),
                    GivenDate = dtCertificate.Rows[0][Common.STU_CERTIFICATE.GIVEN_DATE].ToString(),
                    Purpose = dtCertificate.Rows[0][Common.STU_CERTIFICATE.PURPOSE].ToString(),
                    RecieveDate = dtCertificate.Rows[0][Common.STU_CERTIFICATE.RECEIVED_DATE].ToString(),
                    RegisterNo = dtCertificate.Rows[0][Common.STU_CERTIFICATE.REGISTER_NUMBER].ToString()
                };
                return Json(objStuCertificate);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                sResult = Common.Messages.NoRecordsfound;
                return Json(sResult);
            }
        }

        // Edit Student Cirtificate UI ...

        public ActionResult EditStuCertificateInfo()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                dv.Clear();
                dv.Add(Common.STU_ADDRESS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                DataTable dtCertificate = new DataTable();
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    dtCertificate = objViewModel.FetchCertificate(dv).DataSource.Table;
                }
                IList<StudentCertificate> liCertificate = new List<StudentCertificate>();
                if (dtCertificate != null && dtCertificate.Rows.Count > 0)
                {
                    liCertificate = (from DataRow dr in dtCertificate.Rows
                                     select new StudentCertificate
                                     {
                                         CeretificateID = dr[Common.STU_CERTIFICATE.CERTIFICATE_ID].ToString(),
                                         Archive = dr[Common.SUP_ACHIEVEMENT.ACHIEVEMENT_NAME].ToString(),
                                         CertificateName = dr[Common.STU_CERTIFICATE.CERTIFICATE_NAME].ToString(),
                                         CertificateNo = dr[Common.STU_CERTIFICATE.CERTIFICATE_NO].ToString(),
                                         GivenDate = dr[Common.STU_CERTIFICATE.GIVEN_DATE].ToString(),
                                         Purpose = dr[Common.STU_CERTIFICATE.PURPOSE].ToString(),
                                         RecieveDate = dr[Common.STU_CERTIFICATE.RECEIVED_DATE].ToString(),
                                         RegisterNo = dr[Common.STU_CERTIFICATE.REGISTER_NUMBER].ToString()
                                     }).ToList();
                    return View(liCertificate);
                }
                else
                    return View();
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult);
            }
        }

        // Edit Student Certificate ..
        public JsonResult UpdateCertificate(string str)
        {
            string sCertificateID = string.Empty;
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null)
                sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentCertificate>(str);
                string ReceiveDate = CommonMethods.ConvertdatetoyearFromat(Result.RecieveDate);
                string GivenDate = CommonMethods.ConvertdatetoyearFromat(Result.GivenDate);
                dv.Clear();
                dv.Add(Common.STU_CERTIFICATE.CERTIFICATE_NAME, Result.CertificateName, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.CERTIFICATE_NO, Result.CertificateNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.RECEIVED_DATE, ReceiveDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.GIVEN_DATE, GivenDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.PURPOSE, Result.Purpose, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.REGISTER_NUMBER, Result.RegisterNo, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.ARCHIVE, Result.Archive, EnumCommand.DataType.String);
                dv.Add(Common.STU_CERTIFICATE.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    sCertificateID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STU_CERTIFICATE.CERTIFICATE_ID, sCertificateID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateCertificate(dv);
                        sResult = (resultArgs.Success) ? "Record update successfully ...!" : "Record not update try again ...!";
                    }
                }
                else
                {
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertStuCertificate(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record Insertd successfully ...!";
                        }
                        else
                            sResult = "Record not Insertd try again ...!";
                    }
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
            }
            return Json(sResult);
        }

        // Delete Student Certificate ...
        public JsonResult DeleteCertificate(string sCertificateId)
        {
            dv.Clear();
            dv.Add(Common.STU_CERTIFICATE.CERTIFICATE_ID, sCertificateId, EnumCommand.DataType.String);
            dv.Add(Common.STU_CERTIFICATE.IS_DELETED, "1", EnumCommand.DataType.String);
            using (StudentViewModel objvieModel = new StudentViewModel())
            {
                resultArgs = objvieModel.DeleteCertificate(dv);
                sResult = (resultArgs.Success) ? "Record deleted successfully ...!" : "Record not deleted try again ..!";
            }
            return Json(sResult);
        }
        #endregion

        #region Account Information
        // Add Student Account UI ...
        public ActionResult AddStuAccountInfo()
        {
            return View();
        }

        // Insert Account Details ...
        public JsonResult InsertAccountDetails(string JsonAccountInfo)
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    var Result = JsonConvert.DeserializeObject<StudentAccountInfo>(JsonAccountInfo);
                    dv.Clear();
                    dv.Add(Common.STU_PERSONAL_INFO.ACCOUNT_NO, Result.AccountNo, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.IFSC_CODE, Result.IfscCode, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.MICR_CODE, Result.MicrCode, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    using (StudentViewModel objStudent = new StudentViewModel())
                    {
                        resultArgs = objStudent.UpdateAccountInfo(dv);
                        sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "InsertAccountDetails", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "InsertAccountDetails", ex.Message);
                    }
                    sResult = Common.ErrorCode.NotAcceptable;
                    return Json(sResult);
                }
            }
            else
            {
                sResult = Common.ErrorCode.RequestTimeout;
            }
            return Json(sResult);
        }

        // Fetch Student AccounrtInfo ...
        public JsonResult FetchAccountInfo()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    DataTable dtAccountInfo = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtAccountInfo = objViewModel.FetchAccountInfo(dv).DataSource.Table;
                    }
                    if (dtAccountInfo != null && dtAccountInfo.Rows.Count > 0)
                    {
                        StudentAccountInfo objAccount = new StudentAccountInfo()
                        {
                            AccountNo = dtAccountInfo.Rows[0][Common.STU_PERSONAL_INFO.ACCOUNT_NO].ToString(),
                            IfscCode = dtAccountInfo.Rows[0][Common.STU_PERSONAL_INFO.IFSC_CODE].ToString(),
                            MicrCode = dtAccountInfo.Rows[0][Common.STU_PERSONAL_INFO.MICR_CODE].ToString()
                        };

                        return Json(objAccount);
                    }
                    else
                    {
                        Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                        sResult = Common.ErrorCode.NoContent;
                        return Json(sResult);
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "FetchAccountInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "FetchAccountInfo", ex.Message);
                    }
                    sResult = Common.ErrorCode.NoContent;
                    return Json(sResult);
                }
            }
            else
            {
                sResult = Common.ErrorCode.RequestTimeout;
                return Json(sResult);
            }
        }

        // Edit Student Account UI ...
        public ActionResult EditStuAccountInfo()
        {
            return View();
        }

        // Edit Student Account Info ...
        public JsonResult UpdateAccountInfo(string JsonAccountInfo)
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    var Result = JsonConvert.DeserializeObject<StudentAccountInfo>(JsonAccountInfo);
                    dv.Clear();
                    dv.Add(Common.STU_PERSONAL_INFO.ACCOUNT_NO, Result.AccountNo, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.IFSC_CODE, Result.IfscCode, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.MICR_CODE, Result.MicrCode, EnumCommand.DataType.String);
                    dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateAccountInfo(dv);
                        sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.ErrorCode.NotAcceptable;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "UpdateAccountInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "UpdateAccountInfo", ex.Message);
                    }
                    sResult = Common.ErrorCode.NotAcceptable;
                    return Json(sResult);
                }
            }
            else
            {
                sResult = Common.ErrorCode.RequestTimeout;
            }
            return Json(sResult);
        }
        #endregion

        #region Counselling Information
        // Add Counselling UI ...

        public ActionResult AddStuCounsellingInfo()
        {
            string sStudentId = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentId))
            {
                dv.Clear();
                dv.Add(Common.STU_COUNSELING.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
                DataTable dtCounsellingInfo = new DataTable();
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    dtCounsellingInfo = objViewModel.FetchCounselling(dv).DataSource.Table;
                }
                IList<StudentCounseling> liStuCounselling = new List<StudentCounseling>();
                if (dtCounsellingInfo != null && dtCounsellingInfo.Rows.Count > 0)
                {
                    liStuCounselling = (from DataRow dr in dtCounsellingInfo.Rows
                                        select new StudentCounseling
                                        {
                                            CounsellingId = dr[Common.STU_COUNSELING.SC_ID].ToString(),
                                            Batch = dr[Common.STU_COUNSELING.BATCH].ToString(),
                                            Commands = dr[Common.STU_COUNSELING.COMMENTS].ToString(),
                                            Dateofcounseling = dr[Common.STU_COUNSELING.DOC].ToString(),
                                            Duration = dr[Common.STU_COUNSELING.DURATION].ToString(),
                                            Remarks = dr[Common.STU_COUNSELING.REMARKS].ToString()
                                        }).ToList();
                    return View(liStuCounselling);
                }
                else
                    return View();
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult);
            }
        }

        // Insert Counseling Details ...
        public JsonResult InsertCounseling(string JsonCounselling)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentCounseling>(JsonCounselling);
                string sCounsellingDate = CommonMethods.ConvertdatetoyearFromat(Result.Dateofcounseling);
                dv.Clear();
                dv.Add(Common.STU_COUNSELING.DOC, sCounsellingDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_COUNSELING.DURATION, Result.Duration, EnumCommand.DataType.String);
                dv.Add(Common.STU_COUNSELING.REMARKS, Result.Remarks, EnumCommand.DataType.String);
                dv.Add(Common.STU_COUNSELING.COMMENTS, Result.Commands, EnumCommand.DataType.String);
                dv.Add(Common.STU_COUNSELING.BATCH, Result.Batch, EnumCommand.DataType.String);
                dv.Add(Common.STU_COUNSELING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.InsertCounseling(dv);
                    sResult = (resultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ....!";
            return Json(sResult);
        }

        // Fetch Student Counselling ...
        public JsonResult FetchCounselling(string sStudentID)
        {
            dv.Clear();
            dv.Add(Common.STU_COUNSELING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
            DataTable dtCounseling = new DataTable();
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtCounseling = objViewModel.FetchCounselling(dv).DataSource.Table;
            }
            if (dtCounseling != null && dtCounseling.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtCounseling.Rows[0][Common.STU_COUNSELING.SC_ID].ToString();
                StudentCounseling objCouselling = new StudentCounseling()
                {
                    Batch = dtCounseling.Rows[0][Common.STU_COUNSELING.BATCH].ToString(),
                    Commands = dtCounseling.Rows[0][Common.STU_COUNSELING.COMMENTS].ToString(),
                    Dateofcounseling = dtCounseling.Rows[0][Common.STU_COUNSELING.DOC].ToString(),
                    Duration = dtCounseling.Rows[0][Common.STU_COUNSELING.DURATION].ToString(),
                    Remarks = dtCounseling.Rows[0][Common.STU_COUNSELING.REMARKS].ToString()
                };
                return Json(objCouselling);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                sResult = Common.Messages.NoRecordsfound;
                return Json(sResult);
            }
        }

        // Bind Counselling DDL ...
        public string BindCounsellingDDL()
        {
            string sOption = string.Empty;
            DataTable dtBatch = new DataTable();
            dtBatch = SupportDataMethod.FetchBatch().DataSource.Table;
            if (dtBatch != null && dtBatch.Rows.Count > 0)
            {
                foreach (DataRow item in dtBatch.Rows)
                {
                    sOption += "<option value='" + item[Common.SUP_BATCHES.BATCH_ID].ToString() + "'>" + item[Common.SUP_BATCHES.BATCH].ToString() + "</option>";
                }
            }
            return sOption;
        }

        // Bind Values To Controls ..
        public JsonResult BindCounsellingInfo(string sCounsellingId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sCounsellingId))
                {
                    try
                    {
                        dv.Clear();
                        dv.Add(Common.STU_COUNSELING.SC_ID, sCounsellingId, EnumCommand.DataType.String);
                        DataTable dtFetchCounselling = new DataTable();
                        using (StudentViewModel objViewModel = new StudentViewModel())
                        {
                            dtFetchCounselling = objViewModel.BindCounselling(dv).DataSource.Table;
                        }
                        if (dtFetchCounselling != null && dtFetchCounselling.Rows.Count > 0)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchCounselling.Rows[0][Common.STU_COUNSELING.SC_ID].ToString();
                            StudentCounseling objCounselling = new StudentCounseling()
                            {
                                Batch = dtFetchCounselling.Rows[0][Common.STU_COUNSELING.BATCH].ToString(),
                                Commands = dtFetchCounselling.Rows[0][Common.STU_COUNSELING.COMMENTS].ToString(),
                                CounsellingId = dtFetchCounselling.Rows[0][Common.STU_COUNSELING.SC_ID].ToString(),
                                Dateofcounseling = dtFetchCounselling.Rows[0][Common.STU_COUNSELING.DOC].ToString(),
                                Duration = dtFetchCounselling.Rows[0][Common.STU_COUNSELING.DURATION].ToString(),
                                Remarks = dtFetchCounselling.Rows[0][Common.STU_COUNSELING.REMARKS].ToString()
                            };
                            return Json(objCounselling);
                        }
                        else
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                            sResult = Common.Messages.NoRecordsfound;
                            return Json(sResult);
                        }
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("StudentController", "BindCounsellingInfo", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("StudentController", "BindCounsellingInfo", ex.Message);
                        }
                        sResult = Common.Messages.NoRecordsfound;
                        return Json(sResult);
                    }
                }
                else
                {
                    sResult = Common.ErrorMessage.BadRequest;
                    return Json(sResult);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult);
            }
        }

        // Edit Counselling UI ...
        public ActionResult EditStuCounsellingInfo()
        {
            string sStudentId = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentId))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_COUNSELING.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
                    DataTable dtCounsellingInfo = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtCounsellingInfo = objViewModel.FetchCounselling(dv).DataSource.Table;
                    }
                    IList<StudentCounseling> liStuCounselling = new List<StudentCounseling>();
                    if (dtCounsellingInfo != null && dtCounsellingInfo.Rows.Count > 0)
                    {
                        liStuCounselling = (from DataRow dr in dtCounsellingInfo.Rows
                                            select new StudentCounseling
                                            {
                                                CounsellingId = dr[Common.STU_COUNSELING.SC_ID].ToString(),
                                                Batch = dr[Common.STU_COUNSELING.BATCH].ToString(),
                                                Commands = dr[Common.STU_COUNSELING.COMMENTS].ToString(),
                                                Dateofcounseling = dr[Common.STU_COUNSELING.DOC].ToString(),
                                                Duration = dr[Common.STU_COUNSELING.DURATION].ToString(),
                                                Remarks = dr[Common.STU_COUNSELING.REMARKS].ToString()
                                            }).ToList();
                        return View(liStuCounselling);
                    }
                    else
                        return View();
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "EditStuCounsellingInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "EditStuCounsellingInfo", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Edit Student Couselling...
        public JsonResult UpdateCouselling(string JsonCounselling)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null)
                sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    var Result = JsonConvert.DeserializeObject<StudentCounseling>(JsonCounselling);
                    string sCounsellingDate = CommonMethods.ConvertdatetoyearFromat(Result.Dateofcounseling);
                    dv.Clear();
                    dv.Add(Common.STU_COUNSELING.DOC, sCounsellingDate, EnumCommand.DataType.String);
                    dv.Add(Common.STU_COUNSELING.DURATION, Result.Duration, EnumCommand.DataType.String);
                    dv.Add(Common.STU_COUNSELING.REMARKS, Result.Remarks, EnumCommand.DataType.String);
                    dv.Add(Common.STU_COUNSELING.COMMENTS, Result.Commands, EnumCommand.DataType.String);
                    dv.Add(Common.STU_COUNSELING.BATCH, Result.Batch, EnumCommand.DataType.String);
                    dv.Add(Common.STU_COUNSELING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                    {
                        string sCounselingID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                        dv.Add(Common.STU_COUNSELING.SC_ID, sCounselingID, EnumCommand.DataType.String);
                        using (StudentViewModel objViewModel = new StudentViewModel())
                        {
                            resultArgs = objViewModel.UpdateCounseling(dv);
                            sResult = (resultArgs.Success) ? "Record updated successfully ...!" : "Record not updated try again ...!";
                        }
                    }
                    else
                    {
                        using (StudentViewModel objViewModel = new StudentViewModel())
                        {
                            resultArgs = objViewModel.InsertCounseling(dv);
                            if (resultArgs.Success)
                            {
                                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                                sResult = "Record saved successfully ...!";
                            }
                            else
                                sResult = "Record not saved try again ...!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "UpdateCouselling", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "UpdateCouselling", ex.Message);
                    }
                    sResult = "Record not saved try again ...!";
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return Json(sResult);
        }

        // Delete Student Counselling ...
        public JsonResult DeleteCounselling(string sCounsellingId)
        {
            if (!string.IsNullOrEmpty(sCounsellingId))
            {
                dv.Clear();
                dv.Add(Common.STU_COUNSELING.SC_ID, sCounsellingId, EnumCommand.DataType.String);
                dv.Add(Common.STU_COUNSELING.IS_DELETED, "1", EnumCommand.DataType.String);
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.DeleteCounselling(dv);
                    sResult = (resultArgs.Success) ? "Record deleted successfully ...!" : "Record not deleted try again ...!";
                }
            }
            else
            {
                sResult = Common.ErrorMessage.BadRequest;
            }


            return Json(sResult);
        }
        #endregion

        #region Incident Information
        // Add Incident UI ...

        public ActionResult AddStuincidentInfo()
        {
            string sStudentId = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentId))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_INCIDENT.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
                    DataTable dtStuIncident = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtStuIncident = objViewModel.FetchIncident(dv).DataSource.Table;
                    }
                    IList<StudentIncident> liIncident = new List<StudentIncident>();
                    if (dtStuIncident != null && dtStuIncident.Rows.Count > 0)
                    {
                        liIncident = (from DataRow dr in dtStuIncident.Rows
                                      select new StudentIncident
                                      {
                                          IncidentId = dr[Common.STU_INCIDENT.INCIDENT_ID].ToString(),
                                          DateInformed = dr[Common.STU_INCIDENT.DATE_INFORMED].ToString(),
                                          DateOfIncident = dr[Common.STU_INCIDENT.DATE_OF_INCIDENT].ToString(),
                                          FirstAidDone = dr[Common.STU_INCIDENT.FIRST_AID_DONE].ToString(),
                                          IncidentDetails = dr[Common.STU_INCIDENT.INCIDENT_DETAILS].ToString(),
                                          InformedToParents = dr[Common.SUP_OPTION.OPTION_NAME].ToString(),
                                          PlaceOfIncident = dr[Common.STU_INCIDENT.PLACE_OF_INCIDENT].ToString(),
                                          TimeOfIncident = dr[Common.STU_INCIDENT.TIME_OF_INCIDENT].ToString()
                                      }).ToList();
                        return View(liIncident);
                    }
                    else
                        return View();
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "AddStuincidentInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "AddStuincidentInfo", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult);
            }
        }

        // Insert Incident ...
        public JsonResult InsertIncident(string JsonIncident)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    var Result = JsonConvert.DeserializeObject<StudentIncident>(JsonIncident);
                    string DateInformed = CommonMethods.ConvertdatetoyearFromat(Result.DateInformed);
                    string DateOfIncident = CommonMethods.ConvertdatetoyearFromat(Result.DateOfIncident);
                    dv.Clear();
                    dv.Add(Common.STU_INCIDENT.DATE_INFORMED, DateInformed, EnumCommand.DataType.String);
                    dv.Add(Common.STU_INCIDENT.DATE_OF_INCIDENT, DateOfIncident, EnumCommand.DataType.String);
                    dv.Add(Common.STU_INCIDENT.PLACE_OF_INCIDENT, Result.PlaceOfIncident, EnumCommand.DataType.String);
                    dv.Add(Common.STU_INCIDENT.TIME_OF_INCIDENT, Result.TimeOfIncident, EnumCommand.DataType.String);
                    dv.Add(Common.STU_INCIDENT.INFORMED_TO_PARENTS, Result.InformedToParents, EnumCommand.DataType.String);
                    dv.Add(Common.STU_INCIDENT.INCIDENT_DETAILS, Result.IncidentDetails, EnumCommand.DataType.String);
                    dv.Add(Common.STU_INCIDENT.FIRST_AID_DONE, Result.FirstAidDone, EnumCommand.DataType.String);
                    dv.Add(Common.STU_INCIDENT.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertStuIncident(dv);
                        sResult = (resultArgs.Success) ? "Record saved successfully ....!" : "Record not saved try again ...!";
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "InsertIncident", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "InsertIncident", ex.Message);
                    }
                    sResult = "Record not saved try again ...!";
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return Json(sResult);
        }

        // Fetch Student Incident ...
        public JsonResult FetchIncident(string sStudentID)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sStudentID))
                {
                    try
                    {
                        dv.Clear();
                        dv.Add(Common.STU_INCIDENT.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                        DataTable dtIncident = new DataTable();
                        using (StudentViewModel objViewModel = new StudentViewModel())
                        {
                            dtIncident = objViewModel.FetchIncident(dv).DataSource.Table;
                        }
                        if (dtIncident != null && dtIncident.Rows.Count > 0)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtIncident.Rows[0][Common.STU_INCIDENT.INCIDENT_ID].ToString();
                            StudentIncident objIncident = new StudentIncident()
                            {
                                DateInformed = dtIncident.Rows[0][Common.STU_INCIDENT.DATE_INFORMED].ToString(),
                                DateOfIncident = dtIncident.Rows[0][Common.STU_INCIDENT.DATE_OF_INCIDENT].ToString(),
                                FirstAidDone = dtIncident.Rows[0][Common.STU_INCIDENT.FIRST_AID_DONE].ToString(),
                                IncidentDetails = dtIncident.Rows[0][Common.STU_INCIDENT.INCIDENT_DETAILS].ToString(),
                                InformedToParents = dtIncident.Rows[0][Common.STU_INCIDENT.INFORMED_TO_PARENTS].ToString(),
                                PlaceOfIncident = dtIncident.Rows[0][Common.STU_INCIDENT.PLACE_OF_INCIDENT].ToString(),
                                TimeOfIncident = dtIncident.Rows[0][Common.STU_INCIDENT.TIME_OF_INCIDENT].ToString()
                            };
                            return Json(objIncident, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                            sResult = Common.Messages.NoRecordsfound;
                            return Json(sResult, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("StudentController", "FetchIncident", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("StudentController", "FetchIncident", ex.Message);
                        }
                        sResult = Common.Messages.NoRecordsfound;
                        return Json(sResult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    sResult = Common.ErrorMessage.BadRequest;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }

        }

        // Bind DDLIncident ...
        public string BindDDLIncident()
        {
            string sOption = string.Empty;
            DataTable dtOption = new DataTable();
            dtOption = SupportDataMethod.FetchOption().DataSource.Table;
            if (dtOption != null && dtOption.Rows.Count > 0)
            {
                foreach (DataRow item in dtOption.Rows)
                {
                    sOption += "<option value='" + item[Common.SUP_OPTION.OPTION_ID].ToString() + "'>" + item[Common.SUP_OPTION.OPTION_NAME].ToString() + "</option>";
                }
            }
            return sOption;
        }

        // Bind  Student Incident ..
        public JsonResult BindIncident(string sIncidentId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sIncidentId))
                {
                    try
                    {
                        dv.Clear();
                        dv.Add(Common.STU_INCIDENT.INCIDENT_ID, sIncidentId, EnumCommand.DataType.String);
                        DataTable dtIncident = new DataTable();
                        using (StudentViewModel objViewModel = new StudentViewModel())
                        {
                            dtIncident = objViewModel.BindStuincident(dv).DataSource.Table;
                        }
                        if (dtIncident != null && dtIncident.Rows.Count > 0)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtIncident.Rows[0][Common.STU_INCIDENT.INCIDENT_ID].ToString();
                            StudentIncident objIncident = new StudentIncident()
                            {
                                DateInformed = dtIncident.Rows[0][Common.STU_INCIDENT.DATE_INFORMED].ToString(),
                                DateOfIncident = dtIncident.Rows[0][Common.STU_INCIDENT.DATE_OF_INCIDENT].ToString(),
                                FirstAidDone = dtIncident.Rows[0][Common.STU_INCIDENT.FIRST_AID_DONE].ToString(),
                                IncidentDetails = dtIncident.Rows[0][Common.STU_INCIDENT.INCIDENT_DETAILS].ToString(),
                                InformedToParents = dtIncident.Rows[0][Common.STU_INCIDENT.INFORMED_TO_PARENTS].ToString(),
                                PlaceOfIncident = dtIncident.Rows[0][Common.STU_INCIDENT.PLACE_OF_INCIDENT].ToString(),
                                TimeOfIncident = dtIncident.Rows[0][Common.STU_INCIDENT.TIME_OF_INCIDENT].ToString()
                            };
                            return Json(objIncident, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                            sResult = Common.Messages.NoRecordsfound;
                            return Json(sResult, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("StudentController", "BindIncident", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("StudentController", "BindIncident", ex.Message);
                        }
                        sResult = Common.Messages.NoRecordsfound;
                        return Json(sResult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    sResult = Common.ErrorMessage.BadRequest;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ....!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Edit Incident UI ...

        public ActionResult EditStuincidentInfo()
        {
            string sStudentId = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentId))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_INCIDENT.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
                    DataTable dtStuIncident = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtStuIncident = objViewModel.FetchIncident(dv).DataSource.Table;
                    }
                    IList<StudentIncident> liIncident = new List<StudentIncident>();
                    if (dtStuIncident != null && dtStuIncident.Rows.Count > 0)
                    {
                        liIncident = (from DataRow dr in dtStuIncident.Rows
                                      select new StudentIncident
                                      {
                                          IncidentId = dr[Common.STU_INCIDENT.INCIDENT_ID].ToString(),
                                          DateInformed = dr[Common.STU_INCIDENT.DATE_INFORMED].ToString(),
                                          DateOfIncident = dr[Common.STU_INCIDENT.DATE_OF_INCIDENT].ToString(),
                                          FirstAidDone = dr[Common.STU_INCIDENT.FIRST_AID_DONE].ToString(),
                                          IncidentDetails = dr[Common.STU_INCIDENT.INCIDENT_DETAILS].ToString(),
                                          InformedToParents = dr[Common.SUP_OPTION.OPTION_NAME].ToString(),
                                          PlaceOfIncident = dr[Common.STU_INCIDENT.PLACE_OF_INCIDENT].ToString(),
                                          TimeOfIncident = dr[Common.STU_INCIDENT.TIME_OF_INCIDENT].ToString()
                                      }).ToList();
                        return View(liIncident);
                    }
                    else
                        return View();
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "EditStuincidentInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "EditStuincidentInfo", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ....!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Edit Student Incident ..
        public JsonResult UpdateIncident(string JsonIncident)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentIncident>(JsonIncident);
                string DateInformed = CommonMethods.ConvertdatetoyearFromat(Result.DateInformed);
                string DateOfIncident = CommonMethods.ConvertdatetoyearFromat(Result.DateOfIncident);
                dv.Clear();
                dv.Add(Common.STU_INCIDENT.DATE_INFORMED, DateInformed, EnumCommand.DataType.String);
                dv.Add(Common.STU_INCIDENT.DATE_OF_INCIDENT, DateOfIncident, EnumCommand.DataType.String);
                dv.Add(Common.STU_INCIDENT.PLACE_OF_INCIDENT, Result.PlaceOfIncident, EnumCommand.DataType.String);
                dv.Add(Common.STU_INCIDENT.TIME_OF_INCIDENT, Result.TimeOfIncident, EnumCommand.DataType.String);
                dv.Add(Common.STU_INCIDENT.INFORMED_TO_PARENTS, Result.InformedToParents, EnumCommand.DataType.String);
                dv.Add(Common.STU_INCIDENT.INCIDENT_DETAILS, Result.IncidentDetails, EnumCommand.DataType.String);
                dv.Add(Common.STU_INCIDENT.FIRST_AID_DONE, Result.FirstAidDone, EnumCommand.DataType.String);
                dv.Add(Common.STU_INCIDENT.STUDENT_ID, sStudentID, EnumCommand.DataType.String);

                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    string sIncidentID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STU_INCIDENT.INCIDENT_ID, sIncidentID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateStuIncident(dv);
                        if (resultArgs.Success)
                            sResult = (resultArgs.Success) ? "Record updated successfully ...!" : "Record not updated try again ...!";
                    }
                }
                else
                {
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertStuIncident(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record saved successfully ...!";
                        }
                        else
                            sResult = "Record not saved try again ...!";
                    }
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return Json(sResult, JsonRequestBehavior.AllowGet);
        }

        // Delete Student Incident ..
        public JsonResult DeleteIncident(string sIncidentId)
        {
            if (!string.IsNullOrEmpty(sIncidentId))
            {
                dv.Clear();
                dv.Add(Common.STU_INCIDENT.INCIDENT_ID, sIncidentId, EnumCommand.DataType.String);
                dv.Add(Common.STU_INCIDENT.IS_DELETED, "1", EnumCommand.DataType.String);
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.DeleteIncident(dv);
                    sResult = (resultArgs.Success) ? "Record deleted successfully ..!" : "Record not deleted try again ..!";
                }
            }
            else
                sResult = Common.ErrorMessage.BadRequest;
            return Json(sResult);
        }
        #endregion

        #region Sibling Information
        // Add Sibling UI ..

        public ActionResult AddStuSiblingInfo()
        {
            string sStudentID = string.Empty;
            sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_SIBLING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    DataTable dtSibling = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        dtSibling = objViewModel.FetchSibling(dv, sAcademicYear).DataSource.Table;
                    }
                    IList<StudentSibling> liSibling = new List<StudentSibling>();
                    if (dtSibling != null && dtSibling.Rows.Count > 0)
                    {
                        liSibling = (from DataRow dr in dtSibling.Rows
                                     select new StudentSibling()
                                     {
                                         SiblingId = dr[Common.STU_SIBLING.SIBLING_ID].ToString(),
                                         Age = dr[Common.STU_SIBLING.AGE].ToString(),
                                         DateofBirth = dr[Common.STU_SIBLING.DATE_OF_BIRTH].ToString(),
                                         Employed = dr[Common.STU_SIBLING.EMPLOYED].ToString(),
                                         Gender = dr[Common.SUP_GENDER.GENDER_NAME].ToString(),
                                         InstituteName = dr[Common.STU_SIBLING.INSTITUTE_NAME].ToString(),
                                         Is_Same_Collage = dr[Common.STU_SIBLING.IS_SAME_COLLEGE].ToString(),
                                         MonthlyIncome = dr[Common.STU_SIBLING.MONTHLY_INCOME].ToString(),
                                         Name = dr[Common.STU_SIBLING.NAME].ToString(),
                                         Occupation = dr[Common.SUP_OCCUPATION.OCCUPATION_NAME].ToString(),
                                         ProgName = dr[Common.STU_SIBLING.PROGNAME].ToString(),
                                         Program = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString(),
                                         Qualification = dr[Common.STU_SIBLING.QUALIFICATION].ToString()
                                     }).ToList();
                        return View(liSibling);
                    }
                    else
                        return View();
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "AddStuSiblingInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "AddStuSiblingInfo", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ....!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }

        }

        // Insert Student Sibling ...
        public JsonResult InsertSibling(string JsonSibling)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentSibling>(JsonSibling);
                string sDOB = CommonMethods.ConvertdatetoyearFromat(Result.DateofBirth);
                dv.Clear();
                dv.Add(Common.STU_SIBLING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.NAME, Result.Name, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.AGE, Result.Age, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.PROGRAM, Result.Program, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.PROGNAME, Result.ProgName, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.OCCUPATION, Result.Occupation, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.GENDER, Result.Gender, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.EMPLOYED, Result.Employed, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.DATE_OF_BIRTH, sDOB, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.INSTITUTE_NAME, Result.InstituteName, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.MONTHLY_INCOME, Result.MonthlyIncome, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.IS_SAME_COLLEGE, Result.Is_Same_Collage, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.QUALIFICATION, Result.Qualification, EnumCommand.DataType.String);
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.InsertStuSibling(dv);
                    sResult = (resultArgs.Success) ? "Record saved successfully ....!" : "Record not saved try again ...!";
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return Json(sResult);
        }

        // Fetch Student Sibling ...
        public JsonResult FetchSibling(string sStudentID)
        {
            dv.Clear();
            dv.Add(Common.STU_SIBLING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
            DataTable dtSibling = new DataTable();
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                dtSibling = objViewModel.FetchSibling(dv, sAcademicYear).DataSource.Table;
            }
            if (dtSibling != null && dtSibling.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtSibling.Rows[0][Common.STU_SIBLING.SIBLING_ID].ToString();
                StudentSibling objsibling = new StudentSibling()
                {
                    Name = dtSibling.Rows[0][Common.STU_SIBLING.NAME].ToString(),
                    Age = dtSibling.Rows[0][Common.STU_SIBLING.AGE].ToString(),
                    DateofBirth = dtSibling.Rows[0][Common.STU_SIBLING.DATE_OF_BIRTH].ToString(),
                    Employed = dtSibling.Rows[0][Common.STU_SIBLING.EMPLOYED].ToString(),
                    Gender = dtSibling.Rows[0][Common.STU_SIBLING.GENDER].ToString(),
                    InstituteName = dtSibling.Rows[0][Common.STU_SIBLING.INSTITUTE_NAME].ToString(),
                    Is_Same_Collage = dtSibling.Rows[0][Common.STU_SIBLING.IS_SAME_COLLEGE].ToString(),
                    MonthlyIncome = dtSibling.Rows[0][Common.STU_SIBLING.MONTHLY_INCOME].ToString(),
                    Occupation = dtSibling.Rows[0][Common.STU_SIBLING.OCCUPATION].ToString(),
                    ProgName = dtSibling.Rows[0][Common.STU_SIBLING.PROGNAME].ToString(),
                    Program = dtSibling.Rows[0][Common.STU_SIBLING.PROGRAM].ToString(),
                    Qualification = dtSibling.Rows[0][Common.STU_SIBLING.QUALIFICATION].ToString()
                };
                return Json(objsibling);

            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                sResult = Common.Messages.NoRecordsfound;
                return Json(sResult);
            }

        }

        // Bind DDLSibling...
        public JsonResult BindDDLSibling()
        {
            string sOption = string.Empty;
            string sProgram = string.Empty;
            string sGender = string.Empty;
            string sOccupation = string.Empty;
            string JsonData = string.Empty;
            string sAcademicYear = string.Empty;

            sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                DataTable dtOption = new DataTable();
                dtOption = SupportDataMethod.FetchOption().DataSource.Table;
                if (dtOption != null && dtOption.Rows.Count > 0)
                {
                    foreach (DataRow item in dtOption.Rows)
                    {
                        sOption += "<option value='" + item[Common.SUP_OPTION.OPTION_ID].ToString() + "'>" + item[Common.SUP_OPTION.OPTION_NAME].ToString() + "</option>";
                    }
                }
                DataTable dtProgram = new DataTable();
                dtProgram = SupportDataMethod.FetchProgram(sAcademicYear).DataSource.Table;
                if (dtProgram != null && dtProgram.Rows.Count > 0)
                {
                    foreach (DataRow item in dtProgram.Rows)
                    {
                        sProgram += "<option value='" + item[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString() + "'>" + item[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString() + "</option>";
                    }
                }
                DataTable dtGender = new DataTable();
                dtGender = SupportDataMethod.FetchGender().DataSource.Table;
                if (dtGender != null && dtGender.Rows.Count > 0)
                {
                    foreach (DataRow item in dtGender.Rows)
                    {
                        sGender += "<option value='" + item[Common.SUP_GENDER.GENDER_ID].ToString() + "'>" + item[Common.SUP_GENDER.GENDER_NAME].ToString() + "</option>";
                    }
                }
                DataTable dtOccupation = new DataTable();
                dtOccupation = SupportDataMethod.Occupation().DataSource.Table;
                if (dtOccupation != null && dtOccupation.Rows.Count > 0)
                {
                    foreach (DataRow item in dtOccupation.Rows)
                    {
                        sOccupation += "<option value='" + item[Common.SUP_OCCUPATION.OCCUPATION_ID].ToString() + "'>" + item[Common.SUP_OCCUPATION.OCCUPATION_NAME].ToString() + "</option>";
                    }
                }
                var objJsonData = new DDLForSibling() { Option = sOption, Gender = sGender, Occupation = sOccupation, Program = sProgram };
                JsonData = JsonConvert.SerializeObject(objJsonData);
                return Json(JsonData);
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Bind Sibling Details ...
        public JsonResult BindSibling(string sSiblingId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sSiblingId))
                {
                    try
                    {
                        dv.Clear();
                        dv.Add(Common.STU_SIBLING.SIBLING_ID, sSiblingId, EnumCommand.DataType.String);
                        DataTable dtSibling = new DataTable();
                        using (StudentViewModel objViewModel = new StudentViewModel())
                        {
                            dtSibling = objViewModel.BindSibling(dv).DataSource.Table;
                        }
                        if (dtSibling != null && dtSibling.Rows.Count > 0)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtSibling.Rows[0][Common.STU_SIBLING.SIBLING_ID].ToString();
                            StudentSibling objSibling = new StudentSibling()
                            {
                                Age = dtSibling.Rows[0][Common.STU_SIBLING.AGE].ToString(),
                                DateofBirth = dtSibling.Rows[0][Common.STU_SIBLING.DATE_OF_BIRTH].ToString(),
                                Employed = dtSibling.Rows[0][Common.STU_SIBLING.EMPLOYED].ToString(),
                                Gender = dtSibling.Rows[0][Common.STU_SIBLING.GENDER].ToString(),
                                InstituteName = dtSibling.Rows[0][Common.STU_SIBLING.INSTITUTE_NAME].ToString(),
                                Is_Same_Collage = dtSibling.Rows[0][Common.STU_SIBLING.IS_SAME_COLLEGE].ToString(),
                                MonthlyIncome = dtSibling.Rows[0][Common.STU_SIBLING.MONTHLY_INCOME].ToString(),
                                Name = dtSibling.Rows[0][Common.STU_SIBLING.NAME].ToString(),
                                Occupation = dtSibling.Rows[0][Common.STU_SIBLING.OCCUPATION].ToString(),
                                ProgName = dtSibling.Rows[0][Common.STU_SIBLING.PROGRAM].ToString(),
                                Program = dtSibling.Rows[0][Common.STU_SIBLING.PROGRAM].ToString(),
                                Qualification = dtSibling.Rows[0][Common.STU_SIBLING.QUALIFICATION].ToString(),
                                SiblingId = dtSibling.Rows[0][Common.STU_SIBLING.SIBLING_ID].ToString()
                            };
                            return Json(objSibling, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                            sResult = Common.Messages.NoRecordsfound;
                            return Json(sResult, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("StudentController", "BindSibling", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("StudentController", "BindSibling", ex.Message);
                        }
                        sResult = Common.Messages.NoRecordsfound;
                        return Json(sResult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    sResult = Common.ErrorMessage.BadRequest;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }

        }

        // Edit Sibling UI ..

        public ActionResult EditStuSiblingInfo()
        {
            string sStudentID = string.Empty;
            sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_SIBLING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    DataTable dtSibling = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                        dtSibling = objViewModel.FetchSibling(dv, sAcademicYear).DataSource.Table;
                    }
                    IList<StudentSibling> liSibling = new List<StudentSibling>();
                    if (dtSibling != null && dtSibling.Rows.Count > 0)
                    {
                        liSibling = (from DataRow dr in dtSibling.Rows
                                     select new StudentSibling()
                                     {
                                         SiblingId = dr[Common.STU_SIBLING.SIBLING_ID].ToString(),
                                         Age = dr[Common.STU_SIBLING.AGE].ToString(),
                                         DateofBirth = dr[Common.STU_SIBLING.DATE_OF_BIRTH].ToString(),
                                         Employed = dr[Common.STU_SIBLING.EMPLOYED].ToString(),
                                         Gender = dr[Common.SUP_GENDER.GENDER_NAME].ToString(),
                                         InstituteName = dr[Common.STU_SIBLING.INSTITUTE_NAME].ToString(),
                                         Is_Same_Collage = dr[Common.STU_SIBLING.IS_SAME_COLLEGE].ToString(),
                                         MonthlyIncome = dr[Common.STU_SIBLING.MONTHLY_INCOME].ToString(),
                                         Name = dr[Common.STU_SIBLING.NAME].ToString(),
                                         Occupation = dr[Common.SUP_OCCUPATION.OCCUPATION_NAME].ToString(),
                                         ProgName = dr[Common.STU_SIBLING.PROGNAME].ToString(),
                                         Program = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString(),
                                         Qualification = dr[Common.STU_SIBLING.QUALIFICATION].ToString()
                                     }).ToList();
                        return View(liSibling);
                    }
                    else
                        return View();
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "BindCounsellingInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "BindCounsellingInfo", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Edit Student Sibling ..
        public JsonResult UpdateSibling(string JsonSibling)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null)
                sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentSibling>(JsonSibling);
                string sDOB = CommonMethods.ConvertdatetoyearFromat(Result.DateofBirth);
                dv.Clear();
                dv.Add(Common.STU_SIBLING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.NAME, Result.Name, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.AGE, Result.Age, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.PROGRAM, Result.Program, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.PROGNAME, Result.ProgName, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.OCCUPATION, Result.Occupation, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.GENDER, Result.Gender, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.EMPLOYED, Result.Employed, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.DATE_OF_BIRTH, sDOB, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.INSTITUTE_NAME, Result.InstituteName, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.MONTHLY_INCOME, Result.MonthlyIncome, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.IS_SAME_COLLEGE, Result.Is_Same_Collage, EnumCommand.DataType.String);
                dv.Add(Common.STU_SIBLING.QUALIFICATION, Result.Qualification, EnumCommand.DataType.String);

                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    string sSiblingID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STU_SIBLING.SIBLING_ID, sSiblingID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateStuSibling(dv);
                        sResult = (resultArgs.Success) ? "Record updated successfully ...!" : "Record not updated try again ...!";
                    }
                }
                else
                {
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertStuSibling(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record saved successfully ...!";
                        }
                        else
                            sResult = "Record not saved try again ...!";
                    }
                }
            }
            else
                sResult = "Session Has Expired Login Again ...!";
            return Json(sResult);
        }

        // Delete Student Sibling ..
        public JsonResult DeleteSibling(string sSiblingId)
        {
            dv.Clear();
            dv.Add(Common.STU_SIBLING.SIBLING_ID, sSiblingId, EnumCommand.DataType.String);
            dv.Add(Common.STU_SIBLING.IS_DELETED, "1", EnumCommand.DataType.String);
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                resultArgs = objViewModel.DeleteStuSibling(dv);
                sResult = (resultArgs.Success) ? "Record deleted successfully ...!" : "Record not deleted try again ...!";
            }
            return Json(sResult);
        }
        #endregion

        #region Talents Information
        // Add Talents UI ...

        public ActionResult AddStuTalentsInfo()
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    string sStudentId = string.Empty;
                    sStudentId = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
                    dv.Clear();
                    dv.Add(Common.STU_TALENTS.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
                    DataTable dtTalents = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtTalents = objViewModel.FetchTalents(dv).DataSource.Table;
                    }
                    IList<StudentTalents> liTalents = new List<StudentTalents>();
                    if (dtTalents != null && dtTalents.Rows.Count > 0)
                    {
                        liTalents = (from DataRow dr in dtTalents.Rows
                                     select new StudentTalents()
                                     {
                                         Date = dr[Common.STU_TALENTS.DATE].ToString(),
                                         Grade = dr[Common.SUP_OVERALL_GRADE.OVERALL_GRADE].ToString(),
                                         Remarks = dr[Common.STU_TALENTS.REMARKS].ToString(),
                                         Status = dr[Common.STU_TALENTS.STATUS].ToString(),
                                         TalentActivityType = dr[Common.SUP_ACTIVITY_TYPE.ACTIVITY_TYPE].ToString(),
                                         TalentArea = dr[Common.SUP_ACTIVITY_LEVEL.ACTIVITY_LEVEL].ToString(),
                                         TalentDiscription = dr[Common.STU_TALENTS.TALENT_DESCRIPTION].ToString(),
                                         TalentsId = dr[Common.STU_TALENTS.TALENT_ID].ToString()
                                     }).ToList();
                        return View(liTalents);
                    }
                    else
                        return View();
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "AddStuTalentsInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "AddStuTalentsInfo", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Insert Student Talents ...
        public JsonResult InsertTalents(string JsonTalents)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentTalents>(JsonTalents);
                string Date = CommonMethods.ConvertdatetoyearFromat(Result.Date);
                dv.Clear();
                dv.Add(Common.STU_SIBLING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.DATE, Date, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.GRADE, Result.Grade, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.REMARKS, Result.Remarks, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.STATUS, Result.Status, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.TALENT_ACTIVITY_TYPE, Result.TalentActivityType, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.TALENT_AREA, Result.TalentArea, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.TALENT_DESCRIPTION, Result.TalentDiscription, EnumCommand.DataType.String);

                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.InsertTalents(dv);
                    sResult = (resultArgs.Success) ? "Record saved successfully ....!" : "Record not saved successfully ...!";
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return Json(sResult, JsonRequestBehavior.AllowGet);
        }

        // Fetch Student Talents ...
        public JsonResult FetchTalents(string sStudentID)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_TALENTS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    DataTable dtTalents = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtTalents = objViewModel.FetchTalents(dv).DataSource.Table;
                    }
                    if (dtTalents != null && dtTalents.Rows.Count > 0)
                    {
                        Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtTalents.Rows[0][Common.STU_TALENTS.TALENT_ID].ToString();
                        StudentTalents objTalents = new StudentTalents()
                        {
                            Date = dtTalents.Rows[0][Common.STU_TALENTS.DATE].ToString(),
                            Grade = dtTalents.Rows[0][Common.STU_TALENTS.GRADE].ToString(),
                            Remarks = dtTalents.Rows[0][Common.STU_TALENTS.REMARKS].ToString(),
                            Status = dtTalents.Rows[0][Common.STU_TALENTS.STATUS].ToString(),
                            TalentActivityType = dtTalents.Rows[0][Common.STU_TALENTS.TALENT_ACTIVITY_TYPE].ToString(),
                            TalentArea = dtTalents.Rows[0][Common.STU_TALENTS.TALENT_AREA].ToString(),
                            TalentDiscription = dtTalents.Rows[0][Common.STU_TALENTS.TALENT_DESCRIPTION].ToString()
                        };
                        return Json(objTalents, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                        sResult = Common.Messages.NoRecordsfound;
                        return Json(sResult, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "FetchTalents", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "FetchTalents", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Bind DDLTalents ...
        public JsonResult BindDDLTalents()
        {
            string sGrade = string.Empty;
            string sTalentType = string.Empty;
            string sTalentArea = string.Empty;
            string JsonData = string.Empty;
            DataTable dtGrade = new DataTable();
            dtGrade = SupportDataMethod.FetchGrade().DataSource.Table;
            if (dtGrade != null && dtGrade.Rows.Count > 0)
            {
                foreach (DataRow item in dtGrade.Rows)
                {
                    sGrade += "<option value='" + item[Common.SUP_OVERALL_GRADE.OVERALL_GRADE_ID].ToString() + "'>" + item[Common.SUP_OVERALL_GRADE.OVERALL_GRADE].ToString() + "</option>";
                }
            }
            DataTable dtTalent = new DataTable();
            dtTalent = SupportDataMethod.FetchTalentActivityType().DataSource.Table;
            if (dtTalent != null && dtTalent.Rows.Count > 0)
            {
                foreach (DataRow item in dtTalent.Rows)
                {
                    sTalentType += "<option value='" + item[Common.SUP_ACTIVITY_TYPE.ACTIVITY_TYPE_ID].ToString() + "'>" + item[Common.SUP_ACTIVITY_TYPE.ACTIVITY_TYPE].ToString() + "</option>";
                }
            }
            DataTable dtTalentArea = new DataTable();
            dtTalentArea = SupportDataMethod.FetchActivityArea().DataSource.Table;
            if (dtTalentArea != null && dtTalentArea.Rows.Count > 0)
            {
                foreach (DataRow item in dtTalentArea.Rows)
                {
                    sTalentArea += "<option value='" + item[Common.SUP_TALENT_AREA.ACTIVITY_LEVEL_ID].ToString() + "'>" + item[Common.SUP_TALENT_AREA.ACTIVITY_LEVEL].ToString() + "</option>";
                }
            }
            var objJsonData = new DDLForTalents() { Grade = sGrade, TalentActivity = sTalentType, TalentArea = sTalentArea };
            JsonData = JsonConvert.SerializeObject(objJsonData);
            return Json(JsonData);
        }

        // Bind Talent Details ...
        public JsonResult BindTalent(string sTalentsId)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_TALENTS.TALENT_ID, sTalentsId, EnumCommand.DataType.String);
                    DataTable dtTalents = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtTalents = objViewModel.BindTalents(dv).DataSource.Table;
                    }
                    if (dtTalents != null && dtTalents.Rows.Count > 0)
                    {
                        Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtTalents.Rows[0][Common.STU_TALENTS.TALENT_ID].ToString();
                        StudentTalents objTalents = new StudentTalents()
                        {
                            Date = dtTalents.Rows[0][Common.STU_TALENTS.DATE].ToString(),
                            Grade = dtTalents.Rows[0][Common.STU_TALENTS.GRADE].ToString(),
                            Remarks = dtTalents.Rows[0][Common.STU_TALENTS.REMARKS].ToString(),
                            Status = dtTalents.Rows[0][Common.STU_TALENTS.STATUS].ToString(),
                            TalentActivityType = dtTalents.Rows[0][Common.STU_TALENTS.TALENT_ACTIVITY_TYPE].ToString(),
                            TalentArea = dtTalents.Rows[0][Common.STU_TALENTS.TALENT_AREA].ToString(),
                            TalentDiscription = dtTalents.Rows[0][Common.STU_TALENTS.TALENT_DESCRIPTION].ToString()
                        };
                        return Json(objTalents, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                        sResult = Common.Messages.NoRecordsfound;
                        return Json(sResult, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "BindTalent", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "BindTalent", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Edit Talents UI ...

        public ActionResult EditStuTalentsInfo()
        {
            string sStudentId = string.Empty;
            sStudentId = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentId))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_TALENTS.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
                    DataTable dtTalents = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtTalents = objViewModel.FetchTalents(dv).DataSource.Table;
                    }
                    IList<StudentTalents> liTalents = new List<StudentTalents>();
                    if (dtTalents != null && dtTalents.Rows.Count > 0)
                    {
                        liTalents = (from DataRow dr in dtTalents.Rows
                                     select new StudentTalents()
                                     {
                                         Date = dr[Common.STU_TALENTS.DATE].ToString(),
                                         Grade = dr[Common.SUP_OVERALL_GRADE.OVERALL_GRADE].ToString(),
                                         Remarks = dr[Common.STU_TALENTS.REMARKS].ToString(),
                                         Status = dr[Common.STU_TALENTS.STATUS].ToString(),
                                         TalentActivityType = dr[Common.SUP_ACTIVITY_TYPE.ACTIVITY_TYPE].ToString(),
                                         TalentArea = dr[Common.SUP_ACTIVITY_LEVEL.ACTIVITY_LEVEL].ToString(),
                                         TalentDiscription = dr[Common.STU_TALENTS.TALENT_DESCRIPTION].ToString(),
                                         TalentsId = dr[Common.STU_TALENTS.TALENT_ID].ToString()
                                     }).ToList();
                        return View(liTalents);
                    }
                    else
                        return View();
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "EditStuTalentsInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "EditStuTalentsInfo", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Update Student Talents ..
        public JsonResult UpdateTalents(string JsonTalents)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null)
                sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentTalents>(JsonTalents);
                string Date = CommonMethods.ConvertdatetoyearFromat(Result.Date);
                dv.Clear();
                dv.Add(Common.STU_SIBLING.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.DATE, Date, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.GRADE, Result.Grade, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.REMARKS, Result.Remarks, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.STATUS, Result.Status, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.TALENT_ACTIVITY_TYPE, Result.TalentActivityType, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.TALENT_AREA, Result.TalentArea, EnumCommand.DataType.String);
                dv.Add(Common.STU_TALENTS.TALENT_DESCRIPTION, Result.TalentDiscription, EnumCommand.DataType.String);

                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    string sTalentID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STU_TALENTS.TALENT_ID, sTalentID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateTalents(dv);
                        sResult = (resultArgs.Success) ? "Record updated successfully ...!" : "Record not updated try again ...!";
                    }
                }
                else
                {
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertTalents(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record saved successfully ...!";
                        }
                        else
                            sResult = "Record not saved try again ...!";
                    }
                }
            }
            else
                sResult = "Session Has Expired Login Again  ...!";
            return Json(sResult, JsonRequestBehavior.AllowGet);
        }

        // Delete Student Talents ...
        public JsonResult DeleteTalents(string sTalentsId)
        {
            dv.Clear();
            dv.Add(Common.STU_TALENTS.TALENT_ID, sTalentsId, EnumCommand.DataType.String);
            dv.Add(Common.STU_TALENTS.IS_DELETED, "1", EnumCommand.DataType.String);
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                resultArgs = objViewModel.DeleteTalents(dv);
                sResult = (resultArgs.Success) ? "Record deleted successfully ...!" : "Record not deleted try again...!";
            }
            return Json(sResult);
        }
        #endregion

        #region TC Issue Information
        // Add TC Issue UI...

        public ActionResult AddStuTcIssueInfo()
        {
            string sStudentID = string.Empty;
            sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_ISSUE_ETC.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    DataTable dtIssueEtc = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtIssueEtc = objViewModel.FetchIssueEtc(dv).DataSource.Table;
                    }
                    IList<StudentIssueEtc> liIssue = new List<StudentIssueEtc>();
                    if (dtIssueEtc != null && dtIssueEtc.Rows.Count > 0)
                    {
                        liIssue = (from DataRow dr in dtIssueEtc.Rows
                                   select new StudentIssueEtc()
                                   {
                                       sTCIssueId = dr[Common.STU_ISSUE_ETC.TC_ID].ToString(),
                                       TCIssueDate = dr[Common.STU_ISSUE_ETC.TC_ISSUED_DATE].ToString(),
                                       TCIssueNumber = dr[Common.STU_ISSUE_ETC.TC_ISSUED_NUMBER].ToString(),
                                       TCProduceDate = dr[Common.STU_ISSUE_ETC.TC_PRODUCE_DATE].ToString(),
                                       TCproduceNumber = dr[Common.STU_ISSUE_ETC.TC_PRODUCED_NUMBER].ToString()
                                   }).ToList();
                        return View(liIssue);
                    }
                    else
                        return View();
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "AddStuTcIssueInfo", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "AddStuTcIssueInfo", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Insert Student Issue Ect ...
        public JsonResult InsertIssueEtc(string JsonTCIssue)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    var Result = JsonConvert.DeserializeObject<StudentIssueEtc>(JsonTCIssue);
                    string ProduceDate = CommonMethods.ConvertdatetoyearFromat(Result.TCProduceDate);
                    string IssueDate = CommonMethods.ConvertdatetoyearFromat(Result.TCIssueDate);
                    dv.Clear();
                    dv.Add(Common.STU_ISSUE_ETC.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ISSUE_ETC.TC_PRODUCE_DATE, ProduceDate, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ISSUE_ETC.TC_PRODUCED_NUMBER, Result.TCproduceNumber, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ISSUE_ETC.TC_ISSUED_NUMBER, Result.TCIssueNumber, EnumCommand.DataType.String);
                    dv.Add(Common.STU_ISSUE_ETC.TC_ISSUED_DATE, IssueDate, EnumCommand.DataType.String);

                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertIssueEtc(dv);
                        sResult = (resultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                    }
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "InternalAssessment", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "InternalAssessment", ex.Message);
                    }
                    sResult = Common.Messages.FailedToSaveRecords;
                }
            }
            else
                sResult = "Session  Has Expired Login And Try Again ...!";
            return Json(sResult, JsonRequestBehavior.AllowGet);

        }

        // Fetch Student IssueEtc ...
        public JsonResult FetchIssueEtc(string sStudentID)
        {
            if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
            {
                if (!string.IsNullOrEmpty(sStudentID))
                {
                    try
                    {
                        dv.Clear();
                        dv.Add(Common.STU_ISSUE_ETC.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                        DataTable dtIssueEtc = new DataTable();
                        using (StudentViewModel objViewModel = new StudentViewModel())
                        {
                            dtIssueEtc = objViewModel.FetchIssueEtc(dv).DataSource.Table;
                        }
                        if (dtIssueEtc != null && dtIssueEtc.Rows.Count > 0)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_ID].ToString();
                            StudentIssueEtc objIssueEtc = new StudentIssueEtc()
                            {
                                TCIssueDate = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_ISSUED_DATE].ToString(),
                                TCIssueNumber = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_ISSUED_NUMBER].ToString(),
                                TCProduceDate = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_PRODUCED_NUMBER].ToString(),
                                TCproduceNumber = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_PRODUCE_DATE].ToString()
                            };
                            return Json(objIssueEtc, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                            sResult = Common.Messages.NoRecordsfound;
                            return Json(sResult, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLog objHandler = new ErrorLog())
                        {
                            objHandler.WriteError("StudentController", "FetchIssueEtc", "Err MSg: " + ex.Message, "Query is empty!");
                            objHandler.WriteError("StudentController", "FetchIssueEtc", ex.Message);
                        }
                        sResult = Common.Messages.NoRecordsfound;
                        return Json(sResult, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    sResult = Common.ErrorMessage.BadRequest;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Bind Student IssueEtc ...
        public JsonResult BindIssueEtc(string sTCIssueId)
        {
            dv.Clear();
            dv.Add(Common.STU_ISSUE_ETC.TC_ID, sTCIssueId, EnumCommand.DataType.String);
            DataTable dtIssueEtc = new DataTable();
            using (StudentViewModel objHandler = new StudentViewModel())
            {
                dtIssueEtc = objHandler.BindIssueEtc(dv).DataSource.Table;
            }
            if (dtIssueEtc != null && dtIssueEtc.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_ID].ToString();
                StudentIssueEtc objIssueEtc = new StudentIssueEtc()
                {
                    TCIssueDate = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_ISSUED_DATE].ToString(),
                    TCIssueNumber = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_ISSUED_NUMBER].ToString(),
                    TCProduceDate = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_PRODUCE_DATE].ToString(),
                    TCproduceNumber = dtIssueEtc.Rows[0][Common.STU_ISSUE_ETC.TC_PRODUCED_NUMBER].ToString()
                };
                return Json(objIssueEtc);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                sResult = Common.Messages.NoRecordsfound;
                return Json(sResult);
            }
        }

        // Edit TC Issue UI...

        public ActionResult EditStuTcIssueInfo()
        {
            string sStudentID = string.Empty;
            sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                try
                {
                    dv.Clear();
                    dv.Add(Common.STU_ISSUE_ETC.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                    DataTable dtIssueEtc = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtIssueEtc = objViewModel.FetchIssueEtc(dv).DataSource.Table;
                    }
                    IList<StudentIssueEtc> liIssue = new List<StudentIssueEtc>();
                    if (dtIssueEtc != null && dtIssueEtc.Rows.Count > 0)
                    {
                        liIssue = (from DataRow dr in dtIssueEtc.Rows
                                   select new StudentIssueEtc()
                                   {
                                       sTCIssueId = dr[Common.STU_ISSUE_ETC.TC_ID].ToString(),
                                       TCIssueDate = dr[Common.STU_ISSUE_ETC.TC_ISSUED_DATE].ToString(),
                                       TCIssueNumber = dr[Common.STU_ISSUE_ETC.TC_ISSUED_NUMBER].ToString(),
                                       TCProduceDate = dr[Common.STU_ISSUE_ETC.TC_PRODUCE_DATE].ToString(),
                                       TCproduceNumber = dr[Common.STU_ISSUE_ETC.TC_PRODUCED_NUMBER].ToString()
                                   }).ToList();
                        return View(liIssue);
                    }
                    else
                        return View();
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "InternalAssessment", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "InternalAssessment", ex.Message);
                    }
                    sResult = Common.Messages.NoRecordsfound;
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Edit Student IssueEtc ..
        public JsonResult UpdateIssueEtc(string JsonTCIssue)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null)
                sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentIssueEtc>(JsonTCIssue);
                string ProduceDate = CommonMethods.ConvertdatetoyearFromat(Result.TCProduceDate);
                string IssueDate = CommonMethods.ConvertdatetoyearFromat(Result.TCIssueDate);
                dv.Clear();
                dv.Add(Common.STU_ISSUE_ETC.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                dv.Add(Common.STU_ISSUE_ETC.TC_PRODUCE_DATE, ProduceDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_ISSUE_ETC.TC_PRODUCED_NUMBER, Result.TCproduceNumber, EnumCommand.DataType.String);
                dv.Add(Common.STU_ISSUE_ETC.TC_ISSUED_NUMBER, Result.TCIssueNumber, EnumCommand.DataType.String);
                dv.Add(Common.STU_ISSUE_ETC.TC_ISSUED_DATE, IssueDate, EnumCommand.DataType.String);

                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    string sTCID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STU_ISSUE_ETC.TC_ID, sTCID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateIssueEtc(dv);
                        sResult = (resultArgs.Success) ? "Record updated successfully ...!" : "Record not updated try again ...!";
                    }
                }
                else
                {
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertIssueEtc(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record saved successfully ...!";
                        }
                        else
                            sResult = "Record not saved try again ...!";
                    }
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return Json(sResult, JsonRequestBehavior.AllowGet);
        }

        // Delete Student IssueEtc ..
        public JsonResult DeleteIssueEtc(string sTCIssueId)
        {
            dv.Clear();
            dv.Add(Common.STU_ISSUE_ETC.TC_ID, sTCIssueId, EnumCommand.DataType.String);
            dv.Add(Common.STU_ISSUE_ETC.IS_DELETED, "1", EnumCommand.DataType.String);
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                resultArgs = objViewModel.DeleteIssueEtc(dv);
                sResult = (resultArgs.Success) ? "Record deleted successfully ...!" : "Record not deleted tey again ...!";
            }
            return Json(sResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Medical Information
        // Add Medical UI ...

        public ActionResult AddStuMedicalInfo()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                dv.Clear();
                dv.Add(Common.STU_MEDICAL.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                DataTable dtMedical = new DataTable();
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    dtMedical = objViewModel.FetchMedical(dv).DataSource.Table;
                }
                IList<StudentMedical> liMedical = new List<StudentMedical>();
                if (dtMedical != null && dtMedical.Rows.Count > 0)
                {
                    liMedical = (from DataRow dr in dtMedical.Rows
                                 select new StudentMedical()
                                 {
                                     sMedicalId = dr[Common.STU_MEDICAL.MEDICAL_ID].ToString(),
                                     Allergies = dr[Common.STU_MEDICAL.ALLERGIES].ToString(),
                                     DocterNote = dr[Common.STU_MEDICAL.DOCTOR_NOTE].ToString(),
                                     MedicalCondition = dr[Common.STU_MEDICAL.MEDICAL_CONDITION].ToString(),
                                     MedicalDate = dr[Common.STU_MEDICAL.MEDICAL_DATE].ToString(),
                                     Vaccination = dr[Common.STU_MEDICAL.VACCINATION].ToString()
                                 }).ToList();
                    return View(liMedical);
                }
                else
                    return View();
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Insert Student  Medical...
        public JsonResult InsertMedical(string JsonMedical)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentMedical>(JsonMedical);
                string sDate = CommonMethods.ConvertdatetoyearFromat(Result.MedicalDate);
                dv.Clear();
                dv.Add(Common.STU_MEDICAL.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.DOCTOR_NOTE, Result.DocterNote, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.MEDICAL_CONDITION, Result.MedicalCondition, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.MEDICAL_DATE, sDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.VACCINATION, Result.Vaccination, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.ALLERGIES, Result.Allergies, EnumCommand.DataType.String);
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.InsertMedical(dv);
                    sResult = (resultArgs.Success) ? "Record saved successfully ....!" : "Record not saved try again ...!";
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return Json(sResult, JsonRequestBehavior.AllowGet);

        }

        // Fetch Student Medical ...
        public JsonResult FetchMedical(string sStudentId)
        {
            dv.Clear();
            dv.Add(Common.STU_MEDICAL.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
            DataTable dtMedical = new DataTable();
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtMedical = objViewModel.FetchMedical(dv).DataSource.Table;
            }
            if (dtMedical != null && dtMedical.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtMedical.Rows[0][Common.STU_MEDICAL.MEDICAL_ID].ToString();
                StudentMedical objMedical = new StudentMedical()
                {
                    Allergies = dtMedical.Rows[0][Common.STU_MEDICAL.ALLERGIES].ToString(),
                    DocterNote = dtMedical.Rows[0][Common.STU_MEDICAL.DOCTOR_NOTE].ToString(),
                    MedicalCondition = dtMedical.Rows[0][Common.STU_MEDICAL.MEDICAL_CONDITION].ToString(),
                    MedicalDate = dtMedical.Rows[0][Common.STU_MEDICAL.MEDICAL_DATE].ToString(),
                    Vaccination = dtMedical.Rows[0][Common.STU_MEDICAL.VACCINATION].ToString()
                };
                return Json(objMedical, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                sResult = Common.Messages.NoRecordsfound;
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }

        }

        // Bind Student Medical ...
        public JsonResult BindMedical(string sMedicalId)
        {
            dv.Clear();
            dv.Add(Common.STU_MEDICAL.MEDICAL_ID, sMedicalId, EnumCommand.DataType.String);
            DataTable dtMedical = new DataTable();
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtMedical = objViewModel.BindMedical(dv).DataSource.Table;
            }
            if (dtMedical != null && dtMedical.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtMedical.Rows[0][Common.STU_MEDICAL.MEDICAL_ID].ToString();
                StudentMedical objMedical = new StudentMedical()
                {
                    Allergies = dtMedical.Rows[0][Common.STU_MEDICAL.ALLERGIES].ToString(),
                    DocterNote = dtMedical.Rows[0][Common.STU_MEDICAL.DOCTOR_NOTE].ToString(),
                    MedicalCondition = dtMedical.Rows[0][Common.STU_MEDICAL.MEDICAL_CONDITION].ToString(),
                    MedicalDate = dtMedical.Rows[0][Common.STU_MEDICAL.MEDICAL_DATE].ToString(),
                    Vaccination = dtMedical.Rows[0][Common.STU_MEDICAL.VACCINATION].ToString()
                };
                return Json(objMedical);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                sResult = Common.Messages.NoRecordsfound;
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Edit Medical UI ...

        public ActionResult EditStuMedicalInfo()
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStudentID))
            {
                dv.Clear();
                dv.Add(Common.STU_MEDICAL.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                DataTable dtMedical = new DataTable();
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    dtMedical = objViewModel.FetchMedical(dv).DataSource.Table;
                }
                IList<StudentMedical> liMedical = new List<StudentMedical>();
                if (dtMedical != null && dtMedical.Rows.Count > 0)
                {
                    liMedical = (from DataRow dr in dtMedical.Rows
                                 select new StudentMedical()
                                 {
                                     sMedicalId = dr[Common.STU_MEDICAL.MEDICAL_ID].ToString(),
                                     Allergies = dr[Common.STU_MEDICAL.ALLERGIES].ToString(),
                                     DocterNote = dr[Common.STU_MEDICAL.DOCTOR_NOTE].ToString(),
                                     MedicalCondition = dr[Common.STU_MEDICAL.MEDICAL_CONDITION].ToString(),
                                     MedicalDate = dr[Common.STU_MEDICAL.MEDICAL_DATE].ToString(),
                                     Vaccination = dr[Common.STU_MEDICAL.VACCINATION].ToString()
                                 }).ToList();
                    return View(liMedical);
                }
                else
                    return View();
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Edit Student Medical ..
        public JsonResult UpdateMedical(string JsonMedical)
        {
            string sStudentID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null)
                sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
            else
                sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStudentID))
            {
                var Result = JsonConvert.DeserializeObject<StudentMedical>(JsonMedical);
                string sDate = CommonMethods.ConvertdatetoyearFromat(Result.MedicalDate);
                dv.Clear();
                dv.Add(Common.STU_MEDICAL.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.DOCTOR_NOTE, Result.DocterNote, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.MEDICAL_CONDITION, Result.MedicalCondition, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.MEDICAL_DATE, sDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.VACCINATION, Result.Vaccination, EnumCommand.DataType.String);
                dv.Add(Common.STU_MEDICAL.ALLERGIES, Result.Allergies, EnumCommand.DataType.String);

                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    string sMedicalID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STU_MEDICAL.MEDICAL_ID, sMedicalID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateMedical(dv);
                        sResult = (resultArgs.Success) ? "Record updated successfully ...!" : "Record not updated try again ...!";
                    }
                }
                else
                {
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertMedical(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record saved successfully ...!";
                        }
                        else
                            sResult = "Record not saved try again ...!";
                    }
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return Json(sResult);
        }

        // Delete Student Medical ...
        public JsonResult DeleteMedical(string sMedicalId)
        {
            dv.Clear();
            dv.Add(Common.STU_MEDICAL.MEDICAL_ID, sMedicalId, EnumCommand.DataType.String);
            dv.Add(Common.STU_MEDICAL.IS_DELETED, "1", EnumCommand.DataType.String);
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                resultArgs = objViewModel.DeleteMedical(dv);
                sResult = (resultArgs.Success) ? "Record deleted successfully ...!" : "Record not deleted try again...!";
            }
            return Json(sResult);
        }
        #endregion

        #region Achivement Information
        ////Add Achivement UI ...
        //public ActionResult AddStuAchivementInfo()
        //{
        //    return View();
        //}

        //// Insert Student Achievement ...
        //public JsonResult InsertAchievement(string str)
        //{
        //    string sStudentID =  (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
        //    if (str != null)
        //    {
        //        var Result = JsonConvert.DeserializeObject<StudentAchievements>(str);
        //        dv.Clear();
        //        dv.Add(Common.STU_ACHIEVEMENTS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.CLASS_ID, Result.Class, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.ACHIEVEMENTS, Result.Achievements, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.DATE, Result.Date, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.ACTIVITY, Result.Activity, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.IMAGE_PATH, Result.ImgPath, EnumCommand.DataType.String);

        //        using (StudentViewModel objViewModel = new StudentViewModel())
        //        {
        //            resultArgs = objViewModel.InsertAchievements(dv);
        //            if (resultArgs.Success)
        //            {
        //                sResult = "Record saved successfully ....!";
        //            }
        //            else
        //            {
        //                sResult = "Record not saved successfully ...!";
        //            }
        //        }
        //    }
        //    return Json(sResult);

        //}

        //// Fetch Student Achievements ...
        //public JsonResult FetchAchievements(string sStudentID)
        //{
        //    dv.Clear();
        //    dv.Add(Common.STU_ACHIEVEMENTS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
        //    DataTable dtAchievement = new DataTable();
        //    using (StudentViewModel objViewModel = new StudentViewModel())
        //    {
        //        dtAchievement = objViewModel.FetchAchievement(dv).DataSource.Table;
        //    }
        //    if (dtAchievement != null && dtAchievement.Rows.Count > 0)
        //    {
        //        Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtAchievement.Rows[0][Common.STU_ACHIEVEMENTS.ACHIEVE_ID].ToString();
        //        StudentAchievements objAchievements = new StudentAchievements()
        //        {
        //            Achievements = dtAchievement.Rows[0][Common.STU_ACHIEVEMENTS.ACHIEVEMENTS].ToString(),
        //            Activity = dtAchievement.Rows[0][Common.STU_ACHIEVEMENTS.ACTIVITY].ToString(),
        //            Class = dtAchievement.Rows[0][Common.STU_ACHIEVEMENTS.CLASS_ID].ToString(),
        //            Date = dtAchievement.Rows[0][Common.STU_ACHIEVEMENTS.DATE].ToString(),
        //            ImgPath = dtAchievement.Rows[0][Common.STU_ACHIEVEMENTS.IMAGE_PATH].ToString()
        //        };
        //        return Json(objAchievements);
        //    }
        //    else
        //    {
        //        Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
        //        return Json(sResult);
        //    }

        //}


        ////Edit Achivement UI ...
        //public ActionResult EditStuAchivementInfo()
        //{
        //    return View();
        //}

        //// Edit Student Achievements ..
        //public JsonResult EditAchievements(string str)
        //{
        //    string sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
        //    if (str != null)
        //    {
        //        var Result = JsonConvert.DeserializeObject<StudentAchievements>(str);
        //        dv.Clear();
        //        dv.Add(Common.STU_ACHIEVEMENTS.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.CLASS_ID, Result.Class, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.ACHIEVEMENTS, Result.Achievements, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.DATE, Result.Date, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.ACTIVITY, Result.Activity, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_ACHIEVEMENTS.IMAGE_PATH, Result.ImgPath, EnumCommand.DataType.String);


        //        if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
        //        {
        //            string sAchieveID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
        //            dv.Add(Common.STU_ACHIEVEMENTS.ACHIEVE_ID, sAchieveID, EnumCommand.DataType.String);
        //            using (StudentViewModel objViewModel = new StudentViewModel())
        //            {
        //                resultArgs = objViewModel.UpdateAchievements(dv);
        //                if (resultArgs.Success)
        //                {
        //                    sResult = "Record updated successfully ...!";
        //                }
        //                else
        //                {
        //                    sResult = "Record not updated try again ...!";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            using (StudentViewModel objViewModel = new StudentViewModel())
        //            {
        //                resultArgs = objViewModel.InsertAchievements(dv);
        //                if (resultArgs.Success)
        //                {
        //                    Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
        //                    sResult = "Record saved successfully ...!";
        //                }
        //                else
        //                {
        //                    sResult = "Record not saved try again ...!";
        //                }
        //            }
        //        }
        //    }
        //    return Json(sResult);
        //}
        #endregion

        #region ClgHistory Information
        //// Add ClgHistory UI ..
        //public ActionResult AddStuClgHistoryInfo()
        //{
        //    return View();
        //}

        //// Insert Student College History ...
        //public JsonResult InsertClgHistory(string str)
        //{
        //    string sStudentID =  (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
        //    if (str != null)
        //    {
        //        var Result = JsonConvert.DeserializeObject<StudentClgHistry>(str);
        //        dv.Clear();
        //        dv.Add(Common.STU_COLLEGE_HISTORY.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.SCHOOL_NAME, Result.SchoolName, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.GRADE, Result.Grade, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.EXIT_DATE, Result.ExitDate, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.ENTRY_DATE, Result.EntryDate, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.LAST_ATTENDANCE_DATE, Result.LastAttendenceDate, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.OFFICIAL_WEBSITE, Result.OfficialWebsite, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.AGE, Result.Age, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.CITY, Result.City, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.COUNTRY, Result.Country, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.CURRICULUM, Result.Curriculum, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.REASON_FOR_WITHDRAWAL, Result.Reasonforwidthdrawel, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.ACADEMIC_LEVELS, Result.AcademicLevels, EnumCommand.DataType.String);

        //        using (StudentViewModel objViewModel = new StudentViewModel())
        //        {
        //            resultArgs = objViewModel.InsertClghistory(dv);
        //            if (resultArgs.Success)
        //            {
        //                sResult = "Record saved successfully ....!";
        //            }
        //            else
        //            {
        //                sResult = "Record not saved successfully ...!";
        //            }
        //        }
        //    }
        //    return Json(sResult);

        //}

        //// Fetch Student College History ...
        //public JsonResult FetchClgHistory(string sStudentID)
        //{
        //    dv.Clear();
        //    dv.Add(Common.STU_COLLEGE_HISTORY.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
        //    DataTable dtClgHistory = new DataTable();
        //    using (StudentViewModel objViewModel = new StudentViewModel())
        //    {
        //        dtClgHistory = objViewModel.FetchClgHistory(dv).DataSource.Table;
        //    }
        //    if (dtClgHistory != null && dtClgHistory.Rows.Count > 0)
        //    {
        //        Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.SCHOOL_ID].ToString();
        //        StudentClgHistry objClgHistory = new StudentClgHistry()
        //        {
        //            AcademicLevels = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.ACADEMIC_LEVELS].ToString(),
        //            Grade = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.GRADE].ToString(),
        //            Age = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.AGE].ToString(),
        //            City = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.CITY].ToString(),
        //            Country = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.COUNTRY].ToString(),
        //            Curriculum = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.CURRICULUM].ToString(),
        //            EntryDate = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.ENTRY_DATE].ToString(),
        //            ExitDate = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.EXIT_DATE].ToString(),
        //            LastAttendenceDate = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.LAST_ATTENDANCE_DATE].ToString(),
        //            OfficialWebsite = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.OFFICIAL_WEBSITE].ToString(),
        //            Reasonforwidthdrawel = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.REASON_FOR_WITHDRAWAL].ToString(),
        //            SchoolName = dtClgHistory.Rows[0][Common.STU_COLLEGE_HISTORY.SCHOOL_NAME].ToString()
        //        };
        //        return Json(objClgHistory);
        //    }
        //    else
        //    {
        //        Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
        //        return Json(sResult);
        //    }

        //}

        //// Edit ClgHistory UI ..
        //public ActionResult EditStuClgHistoryInfo()
        //{
        //    return View();
        //}

        //// Edit Student College History ..
        //public JsonResult EditClgHistory(string str)
        //{
        //    string sStudentID = Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString();
        //    if (str != null)
        //    {
        //        var Result = JsonConvert.DeserializeObject<StudentClgHistry>(str);
        //        dv.Clear();
        //        dv.Add(Common.STU_COLLEGE_HISTORY.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.SCHOOL_NAME, Result.SchoolName, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.GRADE, Result.Grade, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.EXIT_DATE, Result.ExitDate, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.ENTRY_DATE, Result.EntryDate, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.LAST_ATTENDANCE_DATE, Result.LastAttendenceDate, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.OFFICIAL_WEBSITE, Result.OfficialWebsite, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.AGE, Result.Age, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.CITY, Result.City, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.COUNTRY, Result.Country, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.CURRICULUM, Result.Curriculum, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.REASON_FOR_WITHDRAWAL, Result.Reasonforwidthdrawel, EnumCommand.DataType.String);
        //        dv.Add(Common.STU_COLLEGE_HISTORY.ACADEMIC_LEVELS, Result.AcademicLevels, EnumCommand.DataType.String);

        //        if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
        //        {
        //            string sSchoolID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
        //            dv.Add(Common.STU_COLLEGE_HISTORY.SCHOOL_ID, sSchoolID, EnumCommand.DataType.String);
        //            using (StudentViewModel objViewModel = new StudentViewModel())
        //            {
        //                resultArgs = objViewModel.UpdateClgHistory(dv);
        //                if (resultArgs.Success)
        //                {
        //                    sResult = "Record updated successfully ...!";
        //                }
        //                else
        //                {
        //                    sResult = "Record not updated try again ...!";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            using (StudentViewModel objViewModel = new StudentViewModel())
        //            {
        //                resultArgs = objViewModel.InsertClghistory(dv);
        //                if (resultArgs.Success)
        //                {
        //                    Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
        //                    sResult = "Record saved successfully ...!";
        //                }
        //                else
        //                {
        //                    sResult = "Record not saved try again ...!";
        //                }
        //            }
        //        }
        //    }
        //    return Json(sResult);
        //}
        #endregion

        #region Transfer Certificate
        // Add Details For TC UI ....

        public ActionResult TransferCertificate()
        {
            string sAcademicYear = string.Empty;

            sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                try
                {
                    TransferCertificate objTransferCertificate = new TransferCertificate();
                    //Gender ...
                    DataTable dtGender = new DataTable();
                    dtGender = SupportDataMethod.FetchGender().DataSource.Table;
                    List<SupGender> liGender = new List<SupGender>();
                    if (dtGender != null && dtGender.Rows.Count > 0)
                    {
                        liGender = (from DataRow dr in dtGender.Rows
                                    select new SupGender()
                                    {
                                        GENDER_ID = dr[Common.SUP_GENDER.GENDER_ID].ToString(),
                                        GENDER_NAME = dr[Common.SUP_GENDER.GENDER_NAME].ToString()
                                    }).ToList();
                        objTransferCertificate.Sex = new SelectList(liGender, Common.SUP_GENDER.GENDER_ID, Common.SUP_GENDER.GENDER_NAME);
                    }

                    // Community ...
                    DataTable dtCommunity = new DataTable();
                    dtCommunity = SupportDataMethod.FetchCommunity().DataSource.Table;
                    List<SupCommunity> liCommunity = new List<SupCommunity>();
                    if (dtCommunity != null && dtCommunity.Rows.Count > 0)
                    {
                        liCommunity = (from DataRow dr in dtCommunity.Rows
                                       select new SupCommunity()
                                       {
                                           COMMUNITYID = dr[Common.SUP_COMMUNITY.COMMUNITYID].ToString(),
                                           COMMUNITY = dr[Common.SUP_COMMUNITY.COMMUNITY].ToString()
                                       }).ToList();
                        objTransferCertificate.Community = new SelectList(liCommunity, Common.SUP_COMMUNITY.COMMUNITYID, Common.SUP_COMMUNITY.COMMUNITY);
                    }

                    // Religion ..
                    DataTable dtReligion = new DataTable();
                    dtReligion = SupportDataMethod.FetchReligion().DataSource.Table;
                    List<SUP_RELIGION> liReligion = new List<SUP_RELIGION>();
                    if (dtReligion != null && dtReligion.Rows.Count > 0)
                    {
                        liReligion = (from DataRow dr in dtReligion.Rows
                                      select new SUP_RELIGION()
                                      {
                                          RELIGIONID = dr[Common.SUP_RELIGION.RELIGIONID].ToString(),
                                          RELIGION = dr[Common.SUP_RELIGION.RELIGION].ToString()
                                      }).ToList();
                        objTransferCertificate.Religion = new SelectList(liReligion, Common.SUP_RELIGION.RELIGIONID, Common.SUP_RELIGION.RELIGION);
                    }

                    //Nationality ...
                    DataTable dtNationality = new DataTable();
                    dtNationality = SupportDataMethod.FetchNationality().DataSource.Table;
                    List<SupNationality> liNationality = new List<SupNationality>();
                    if (dtNationality != null && dtNationality.Rows.Count > 0)
                    {
                        liNationality = (from DataRow dr in dtNationality.Rows
                                         select new SupNationality()
                                         {
                                             NATIONALITY_ID = dr[Common.SUP_NATIONALITY.NATIONALITY_ID].ToString(),
                                             NATIONALITY_NAME = dr[Common.SUP_NATIONALITY.NATIONALITY_NAME].ToString()
                                         }).ToList();
                        objTransferCertificate.Nationality = new SelectList(liNationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);
                    }

                    // Option
                    DataTable dtOption = new DataTable();
                    dtOption = SupportDataMethod.FetchOption().DataSource.Table;
                    List<SUP_OPTION> liOption = new List<SUP_OPTION>();
                    if (dtOption != null && dtOption.Rows.Count > 0)
                    {
                        liOption = (from DataRow dr in dtOption.Rows
                                    select new SUP_OPTION()
                                    {
                                        OPTION_ID = dr[Common.SUP_OPTION.OPTION_ID].ToString(),
                                        OPTION_NAME = dr[Common.SUP_OPTION.OPTION_NAME].ToString()
                                    }).ToList();
                        objTransferCertificate.FeesPaid = new SelectList(liOption, Common.SUP_OPTION.OPTION_ID, Common.SUP_OPTION.OPTION_NAME);
                        objTransferCertificate.StuScholarship = new SelectList(liOption, Common.SUP_OPTION.OPTION_ID, Common.SUP_OPTION.OPTION_NAME);
                    }

                    // Conduct ..
                    DataTable dtConduct = new DataTable();
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        dtConduct = objViewModel.FetchConduct().DataSource.Table;
                    }
                    List<SUP_CONDUCT> liConduct = new List<SUP_CONDUCT>();
                    if (dtConduct != null && dtConduct.Rows.Count > 0)
                    {
                        liConduct = (from DataRow dr in dtConduct.Rows
                                     select new SUP_CONDUCT()
                                     {
                                         CONDUCT_ID = dr[Common.SUP_CONDUCT.CONDUCT_ID].ToString(),
                                         CONDUCT_NAME = dr[Common.SUP_CONDUCT.CONDUCT_NAME].ToString()
                                     }).ToList();
                        objTransferCertificate.Conduct = new SelectList(liConduct, Common.SUP_CONDUCT.CONDUCT_ID, Common.SUP_CONDUCT.CONDUCT_NAME);
                    }

                    List<Sup_Shift> liShift = new List<Sup_Shift>();
                    liShift = (List<Sup_Shift>)CMS.DAO.CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
                    objTransferCertificate.ShiftList = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
                    // Program ...
                    DataTable dtProgram = new DataTable();
                    dtProgram = SupportDataMethod.FetchProgram(sAcademicYear).DataSource.Table;
                    List<cp_Program_2017> liProgram = new List<cp_Program_2017>();
                    if (dtProgram != null && dtProgram.Rows.Count > 0)
                    {
                        liProgram = (from DataRow dr in dtProgram.Rows
                                     select new cp_Program_2017()
                                     {
                                         PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                         PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                                     }).ToList();
                        objTransferCertificate.ProgramName = new SelectList(liProgram, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
                    }

                    // Fetch class ....
                    DataTable dtClass = new DataTable();

                    dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
                    List<cp_Classes_2017> liClass = new List<cp_Classes_2017>();
                    if (dtClass != null && dtClass.Rows.Count > 0)
                    {
                        liClass = (from DataRow dr in dtClass.Rows
                                   select new cp_Classes_2017()
                                   {
                                       CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                                       CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                                   }).ToList();
                        objTransferCertificate.ClassName = new SelectList(liClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
                    }
                    return View(objTransferCertificate);
                }
                catch (Exception ex)
                {
                    using (ErrorLog objHandler = new ErrorLog())
                    {
                        objHandler.WriteError("StudentController", "TransferCertificate", "Err MSg: " + ex.Message, "Query is empty!");
                        objHandler.WriteError("StudentController", "TransferCertificate", ex.Message);
                    }
                    sResult = "Fail To Fetch Record(s) ...!";
                    return Json(sResult, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        // Get DDL For Transfer Certificate ..
        public JsonResult BindDDLClass(string sProgramId, string sShiftId)
        {
            string sClass = string.Empty;
            dv.Clear();
            dv.Add(Common.CP_CLASSES_2017.PROGRAMME, sProgramId, EnumCommand.DataType.String);
            dv.Add(Common.CP_CLASSES_2017.SHIFT, sShiftId, EnumCommand.DataType.String);
            DataTable dtClass = new DataTable();
            string sAcademicYear = string.Empty;
            sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtClass = objViewModel.FetchClass(dv, sAcademicYear).DataSource.Table;
            }
            if (dtClass != null && dtClass.Rows.Count > 0)
            {
                foreach (DataRow item in dtClass.Rows)
                {
                    sClass += "<option value='" + item[Common.CP_CLASSES_2017.CLASS_ID].ToString() + "'>" + item[Common.CP_CLASSES_2017.CLASS_NAME].ToString() + "</option>";
                }
            }
            return Json(sClass);
        }

        // Fetch Student Drop Down...
        public JsonResult FetchStudents(string sClassId, string sProgramId)
        {
            string sOption = string.Empty;
            DataTable dtStudent = new DataTable();
            dv.Clear();
            dv.Add(Common.STU_PERSONAL_INFO.CLASS_ID, sClassId, EnumCommand.DataType.String);
            dv.Add(Common.STU_PERSONAL_INFO.PROGRAM_ID, sProgramId, EnumCommand.DataType.String);
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtStudent = objViewModel.FetchStudent(dv).DataSource.Table;
            }
            if (dtStudent != null && dtStudent.Rows.Count > 0)
            {
                foreach (DataRow item in dtStudent.Rows)
                {
                    sOption += "<option value='" + item[Common.STU_PERSONAL_INFO.STUDENT_ID].ToString() + "' >" + item[Common.STU_PERSONAL_INFO.FIRST_NAME].ToString() + "</option>";
                }
            }
            return Json(sOption);
        }

        // Fetch Student Controls...
        public JsonResult FetchStudentForTC(string sStudentId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] == null) ? string.Empty : Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            try
            {
                DataTable dtStudent = new DataTable();
                DataTable dtStudentTC = new DataTable();
                dv.Clear();
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
                StudentTransferCertificate objTransferCertificate = new StudentTransferCertificate();
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    dtStudentTC = objViewModel.FetchStudentsForTCEdit(dv, sAcademicYear).DataSource.Table;
                    dtStudent = objViewModel.BindStudentDetailsForTC(dv, sAcademicYear).DataSource.Table;
                }
                if (dtStudentTC != null && dtStudentTC.Rows.Count > 0)
                {

                    objTransferCertificate.CertificateId = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.CERTIFICATE_ID].ToString();
                    objTransferCertificate.StudentName = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.FIRST_NAME].ToString();
                    objTransferCertificate.Guardian = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.GUARDIAN_NAME].ToString();
                    objTransferCertificate.ParentName = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.FATHER_NAME].ToString();
                    objTransferCertificate.Caste = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.CASTE].ToString();
                    objTransferCertificate.DateToWords = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.DATETOWORDS].ToString();
                    objTransferCertificate.IdentificationMark1 = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.IDENTIFICATION_MARK1].ToString();
                    objTransferCertificate.IdentificationMark2 = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.IDENTIFICATION_MARK2].ToString();
                    objTransferCertificate.AdmittedClass = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.ADMITTED_CLASS].ToString();
                    objTransferCertificate.LeavingTime = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.LEAVING_CLASS].ToString();
                    objTransferCertificate.MainCourse = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.MAIN_COURSE].ToString();
                    objTransferCertificate.AlliedCourse = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.ALLIED].ToString();
                    objTransferCertificate.LeavingDate = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.LEAVING_DATE].ToString();
                    objTransferCertificate.StuTCApplyDate = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.TC_APPLIED_DATE].ToString();
                    objTransferCertificate.TCIssueDate = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.TC_GIVEN_DATE].ToString();
                    objTransferCertificate.AcademicYears = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.ACADEMIC_YEARS].ToString();
                    objTransferCertificate.ClassesStudied = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.CLASSES_STUDEIED].ToString();
                    objTransferCertificate.FirstLanguage = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.FIRST_LANGUAGE].ToString();
                    objTransferCertificate.AdmissionDate = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.ADMISSION_DATE].ToString();
                    objTransferCertificate.DateOfBirth = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.DATE_OF_BIRTH].ToString();
                    objTransferCertificate.FeesPaid = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.FEE_PAID].ToString();
                    objTransferCertificate.StuScholarship = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.SCHOLAR_SHIP].ToString();
                    objTransferCertificate.Conduct = dtStudentTC.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.CONDUCT].ToString();
                    objTransferCertificate.Religion = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.RELIGION].ToString();
                    objTransferCertificate.Nationality = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.NATIONALITY].ToString();
                    objTransferCertificate.Religion = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.RELIGION].ToString();
                    objTransferCertificate.Caste = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.CASTE].ToString();
                    objTransferCertificate.Community = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.COMMUNITY].ToString();
                    objTransferCertificate.Sex = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.GENDER].ToString();
                    return Json(objTransferCertificate);
                }
                else
                {
                    if (dtStudent != null && dtStudent.Rows.Count > 0)
                    {

                        objTransferCertificate.StudentName = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.FIRST_NAME].ToString();
                        objTransferCertificate.ParentName = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.FATHER_NAME].ToString();
                        objTransferCertificate.Guardian = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.GUARDIAN_NAME].ToString();
                        objTransferCertificate.Nationality = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.NATIONALITY].ToString();
                        objTransferCertificate.Religion = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.RELIGION].ToString();
                        objTransferCertificate.Caste = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.CASTE].ToString();
                        objTransferCertificate.Community = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.COMMUNITY].ToString();
                        objTransferCertificate.Sex = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.GENDER].ToString();
                        objTransferCertificate.DateOfBirth = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.DATE_OF_BIRTH].ToString();
                        objTransferCertificate.IdentificationMark1 = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK1].ToString();
                        objTransferCertificate.IdentificationMark2 = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK2].ToString();
                        objTransferCertificate.AdmissionDate = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.ADMISSION_DATE].ToString();
                        objTransferCertificate.AdmittedClass = dtStudent.Rows[0][Common.CP_CLASSES_2017.CLASS_NAME].ToString();
                        objTransferCertificate.LeavingTime = dtStudent.Rows[0][Common.CP_CLASSES_2017.CLASS_NAME].ToString();
                        objTransferCertificate.MainCourse = dtStudent.Rows[0][Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString();
                        objTransferCertificate.LeavingDate = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.LEAVIN_GDATE].ToString();
                        objTransferCertificate.StuTCApplyDate = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.TC_APPLIED_DATE].ToString();
                        objTransferCertificate.TCIssueDate = dtStudent.Rows[0][Common.STU_PERSONAL_INFO.TC_GIVEN_DATE].ToString();

                        return Json(objTransferCertificate);
                    }
                    else
                    {
                        sResult = "No Records Found ...!";
                        return Json(sResult, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("StudentController", "FetchStudentForTC", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("StudentController", "FetchStudentForTC", ex.Message);
                }
                sResult = "Fail To Fetch Record(s) ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        // Fetch Students For Update ..
        public JsonResult FetchStudentsForUpdate(string sStudentId)
        {
            dv.Clear();
            dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
            DataTable dtStudentDetails = new DataTable();
            using (StudentViewModel objviewmodel = new StudentViewModel())
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                dtStudentDetails = objviewmodel.FetchStudentsForTCEdit(dv, sAcademicYear).DataSource.Table;
            }
            if (dtStudentDetails != null & dtStudentDetails.Rows.Count > 0)
            {
                StudentTransferCertificate objTransferCertificate = new StudentTransferCertificate()
                {
                    CertificateId = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.CERTIFICATE_ID].ToString(),
                    StudentName = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.FIRST_NAME].ToString(),
                    Guardian = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.GUARDIAN_NAME].ToString(),
                    ParentName = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.FATHER_NAME].ToString(),
                    Caste = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.CASTE].ToString(),
                    DateToWords = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.DATETOWORDS].ToString(),
                    IdentificationMark1 = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.IDENTIFICATION_MARK1].ToString(),
                    IdentificationMark2 = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.IDENTIFICATION_MARK2].ToString(),
                    AdmittedClass = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.ADMITTED_CLASS].ToString(),
                    LeavingTime = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.LEAVING_CLASS].ToString(),
                    MainCourse = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.MAIN_COURSE].ToString(),
                    AlliedCourse = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.ALLIED].ToString(),
                    LeavingDate = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.LEAVING_DATE].ToString(),
                    StuTCApplyDate = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.TC_APPLIED_DATE].ToString(),
                    TCIssueDate = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.TC_GIVEN_DATE].ToString(),
                    AcademicYears = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.ACADEMIC_YEARS].ToString(),
                    ClassesStudied = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.CLASSES_STUDEIED].ToString(),
                    FirstLanguage = dtStudentDetails.Rows[0][Common.STU_TRANSFER_CERTIFICATE_2017.FIRST_LANGUAGE].ToString()
                };
                return Json(objTransferCertificate, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }

        //// Check Student If Exits ...
        //public JsonResult StudentIfExits(string sStudentId)
        //{
        //    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
        //    StudentTransferCertificate objTransferCertificate = new StudentTransferCertificate();
        //    dv.Clear();
        //    dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.STUDENT_ID,sStudentId,EnumCommand.DataType.String);
        //    using (StudentViewModel objViewModel=new StudentViewModel())
        //    {
        //        resultArgs = objViewModel.StudentIfExits(dv, sAcademicYear);
        //    }
        //    return Json(sResult);
        //}
        // Insert Transfer Certificate ...
        public JsonResult InsertTransferCertificate(string JsonTCIssue)
        {
            string sLastInsertedId = string.Empty;
            StudentTransferCertificate objTransferCertificate = new StudentTransferCertificate();
            if (JsonTCIssue != null)
            {
                var Result = JsonConvert.DeserializeObject<StudentTransferCertificate>(JsonTCIssue);
                string DateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.DateOfBirth);
                string LeavingDate = CommonMethods.ConvertdatetoyearFromat(Result.LeavingDate);
                string AdmissionDate = CommonMethods.ConvertdatetoyearFromat(Result.AdmissionDate);
                string TCApplyDate = CommonMethods.ConvertdatetoyearFromat(Result.StuTCApplyDate);
                string TcIssueDate = CommonMethods.ConvertdatetoyearFromat(Result.TCIssueDate);
                dv.Clear();
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.STUDENT_ID, Result.StudentId, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.SERIAL_NO, "0", EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.ADMISSION_NO, "0", EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.FIRST_NAME, Result.StudentName, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.FATHER_NAME, Result.ParentName, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.GUARDIAN_NAME, Result.Guardian, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.NATIONALITY, Result.Nationality, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.RELIGION, Result.Religion, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.CASTE, Result.Caste, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.COMMUNITY, Result.Community, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.GENDER, Result.Sex, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.DATE_OF_BIRTH, DateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.IDENTIFICATION_MARK1, Result.IdentificationMark1, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.IDENTIFICATION_MARK2, Result.IdentificationMark2, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.ADMISSION_DATE, AdmissionDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.ADMITTED_CLASS, Result.AdmittedClass, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.LEAVING_CLASS, Result.LeavingTime, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.MAIN_COURSE, Result.MainCourse, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.ALLIED, Result.AlliedCourse, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.FEE_PAID, Result.FeesPaid, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.SCHOLAR_SHIP, Result.StuScholarship, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.LEAVING_DATE, LeavingDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.CONDUCT, Result.Conduct, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.TC_APPLIED_DATE, TCApplyDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.TC_GIVEN_DATE, TcIssueDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.ACADEMIC_YEARS, Result.AcademicYears, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.CLASSES_STUDEIED, Result.ClassesStudied, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.FIRST_LANGUAGE, Result.FirstLanguage, EnumCommand.DataType.String);
                dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.DATETOWORDS, Result.DateToWords, EnumCommand.DataType.String);
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                    DataTable dtIfExits = new DataTable();
                    // dtIfExits = objViewModel.StudentIfExits(dv, sAcademicYear).DataSource.Table;

                    if (string.IsNullOrEmpty(Result.CertificateId))
                    {
                        resultArgs = objViewModel.InsertTransferCertificate(dv, sAcademicYear);
                        if (resultArgs.RowUniqueId != null)
                        {
                            objTransferCertificate.CertificateId = resultArgs.RowUniqueId.ToString();
                        }
                        objTransferCertificate.sResult = (resultArgs.Success) ? Common.Messages.RecordsSavedSuccessfully : Common.Messages.FailedToSaveRecords;
                    }
                    else
                    {
                        dv.Add(Common.STU_TRANSFER_CERTIFICATE_2017.CERTIFICATE_ID, Result.CertificateId, EnumCommand.DataType.String);
                        resultArgs = objViewModel.UpdateTransferCertificate(dv, sAcademicYear);
                        objTransferCertificate.sResult = (resultArgs.Success) ? "Record Updated Successfully ..!" : Common.Messages.FailedToSaveRecords;
                    }
                }

                // Update Personal Table
                dv.Clear();
                dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, Result.StudentId, EnumCommand.DataType.String);
                //dv.Add(Common.STU_PERSONAL_INFO.ADMISSION_NO,"0", EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FIRST_NAME, Result.StudentName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.FATHER_NAME, Result.ParentName, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.GUARDIAN_NAME, Result.Guardian, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.NATIONALITY, Result.NationalityId, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.RELIGION, Result.ReligionId, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.CASTE, Result.Caste, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.COMMUNITY, Result.CommunityId, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.GENDER, Result.SexId, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.DATE_OF_BIRTH, DateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK1, Result.IdentificationMark1, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.IDENTIFICATION_MARK2, Result.IdentificationMark2, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.ADMISSION_DATE, AdmissionDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.ADMITTED_CLASS, Result.ClassId, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.LEAVING_CLASS, Result.LeavingTime, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.LEAVIN_GDATE, LeavingDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.TC_APPLIED_DATE, TCApplyDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.TC_GIVEN_DATE, TcIssueDate, EnumCommand.DataType.String);
                dv.Add(Common.STU_PERSONAL_INFO.CONDUCT, Result.Conduct, EnumCommand.DataType.String);
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.UpdatepersonalInfoForTC(dv);
                }
            }

            return Json(objTransferCertificate);
        }

        // Fetch Record For Print ...        
        public ActionResult TCPrintView()
        {
            string sAcademicYear = string.Empty;

            sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            TransferCertificate objTransferCertificate = new TransferCertificate();
            List<Sup_Shift> liShift = new List<Sup_Shift>();
            liShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
            objTransferCertificate.ShiftList = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            // Program ...
            DataTable dtProgram = new DataTable();
            dtProgram = SupportDataMethod.FetchProgram(sAcademicYear).DataSource.Table;
            List<cp_Program_2017> liProgram = new List<cp_Program_2017>();
            if (dtProgram != null && dtProgram.Rows.Count > 0)
            {
                liProgram = (from DataRow dr in dtProgram.Rows
                             select new cp_Program_2017()
                             {
                                 PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                 PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                             }).ToList();
                objTransferCertificate.ProgramName = new SelectList(liProgram, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
            }
            // Fetch class ....
            DataTable dtClass = new DataTable();

            dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
            List<cp_Classes_2017> liClass = new List<cp_Classes_2017>();
            if (dtClass != null && dtClass.Rows.Count > 0)
            {
                liClass = (from DataRow dr in dtClass.Rows
                           select new cp_Classes_2017()
                           {
                               CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                               CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                           }).ToList();
                objTransferCertificate.ClassName = new SelectList(liClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            }
            return View(objTransferCertificate);
        }

        public JsonResult GetProgramByShift(string sShiftId)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sOption = string.Empty;
            var liProgram = new List<CP_PROGRAMME_2017>();
            var ObjShift = new Sup_Shift();
            ObjShift.SHIFT_ID = sShiftId;
            liProgram = (List<CP_PROGRAMME_2017>)CMSHandler.FetchData<CP_PROGRAMME_2017>(ObjShift, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchProgrammeByShiftId), sAcademicYear).DataSource.Data;
            if (liProgram != null && liProgram.Count > 0)
            {
                foreach (var item in liProgram)
                {
                    sOption += "<option value='" + item.PROGRAMME_ID + "'>" + item.PROGRAMME_NAME + "</option>";
                }
            }

            return Json(sOption);
        }
        #endregion

        #region Conduct Certificate
        // Conduct Certificate for Student UI ...

        public ActionResult ConductCertificate()
        {
            string sAcademicYear = string.Empty;

            sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            ConductCertificate objConductCertificate = new ConductCertificate();
            // Program ...
            DataTable dtProgram = new DataTable();
            dtProgram = SupportDataMethod.FetchProgram(sAcademicYear).DataSource.Table;
            List<cp_Program_2017> liProgram = new List<cp_Program_2017>();
            if (dtProgram != null && dtProgram.Rows.Count > 0)
            {
                liProgram = (from DataRow dr in dtProgram.Rows
                             select new cp_Program_2017()
                             {
                                 PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                 PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                             }).ToList();
                objConductCertificate.ProgramId = new SelectList(liProgram, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
            }
            // Shift ..
            List<Sup_Shift> liShift = new List<Sup_Shift>();
            liShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
            objConductCertificate.ShiftList = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            // Fetch class ....
            DataTable dtClass = new DataTable();

            dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
            List<cp_Classes_2017> liClass = new List<cp_Classes_2017>();
            if (dtClass != null && dtClass.Rows.Count > 0)
            {
                liClass = (from DataRow dr in dtClass.Rows
                           select new cp_Classes_2017()
                           {
                               CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                               CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                           }).ToList();
                objConductCertificate.ClassId = new SelectList(liClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            }
            return View(objConductCertificate);
        }
        #endregion

        #region Course Certificate
        // Course Certificate For Student UI ...

        public ActionResult CourseCertificate()
        {
            string sAcademicYear = string.Empty;

            sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;

            CourseCertificate objCourseCertificate = new CourseCertificate();
            // Program ...
            DataTable dtProgram = new DataTable();
            dtProgram = SupportDataMethod.FetchProgram(sAcademicYear).DataSource.Table;
            List<cp_Program_2017> liProgram = new List<cp_Program_2017>();
            if (dtProgram != null && dtProgram.Rows.Count > 0)
            {
                liProgram = (from DataRow dr in dtProgram.Rows
                             select new cp_Program_2017()
                             {
                                 PROGRAMME_ID = dr[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString(),
                                 PROGRAMME_NAME = dr[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString()
                             }).ToList();
                objCourseCertificate.ProgramId = new SelectList(liProgram, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_NAME);
            }

            // Shift ..
            List<Sup_Shift> liShift = new List<Sup_Shift>();
            liShift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
            objCourseCertificate.ShiftList = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);

            // Fetch class ....
            DataTable dtClass = new DataTable();

            dtClass = SupportDataMethod.FetchClass(sAcademicYear).DataSource.Table;
            List<cp_Classes_2017> liClass = new List<cp_Classes_2017>();
            if (dtClass != null && dtClass.Rows.Count > 0)
            {
                liClass = (from DataRow dr in dtClass.Rows
                           select new cp_Classes_2017()
                           {
                               CLASS_ID = dr[Common.CP_CLASSES_2017.CLASS_ID].ToString(),
                               CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString()
                           }).ToList();
                objCourseCertificate.ClassId = new SelectList(liClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            }
            return View(objCourseCertificate);
        }
        #endregion

        #region BonafideCertificate
        // BonaFide Certificate for Student UI ...

        public ActionResult BonafideCertificate()
        {

            return View();
        }

        public JsonResult InsertBonafideCertificate(string BonafideCertificate)
        {
            var Result = JsonConvert.DeserializeObject<BonafideCertificate>(BonafideCertificate);
            string Student_Id = string.Empty;
            string sAcademicYear = string.Empty;
            sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            BonafideCertificate objBonafideCertificate = new BonafideCertificate();
            var liBonafideCertificate = new List<BonafideCertificate>();
            objBonafideCertificate.ROLL_NO = Result.ROLL_NO;
            objBonafideCertificate.FATHER_NAME = Result.FATHER_NAME;
            objBonafideCertificate.CLASS = Result.CLASS;
            objBonafideCertificate.DURING_YEAR = Result.DURING_YEAR;
            objBonafideCertificate.NAME = Result.NAME;
            liBonafideCertificate = (List<BonafideCertificate>)CMSHandler.FetchData<BonafideCertificate>(objBonafideCertificate, StudentSQL.GetStudentSQL(StudentSQLCommads.CheckBonafideIsExits), sAcademicYear).DataSource.Data;

            if (liBonafideCertificate != null)
            {
                objBonafideCertificate.BONAFIDE_ID = liBonafideCertificate.FirstOrDefault().BONAFIDE_ID;
                resultArgs = CMSHandler.SaveOrUpdate(objBonafideCertificate, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateBonafideCertificate), sAcademicYear, true);
                if (resultArgs.Success)
                {
                    objBonafideCertificate.Student_Id = liBonafideCertificate.FirstOrDefault().BONAFIDE_ID;
                    objBonafideCertificate.sResult = Common.Messages.RecordsSavedSuccessfully;
                }
                else
                {
                    objBonafideCertificate.sResult = Common.Messages.FailedToSaveRecords;
                }
            }
            else
            {
                resultArgs = CMSHandler.SaveOrUpdate(objBonafideCertificate, StudentSQL.GetStudentSQL(StudentSQLCommads.InsertBonafideCertificate), sAcademicYear, true);
                if (resultArgs.Success)
                {
                    objBonafideCertificate.Student_Id = resultArgs.RowUniqueId.ToString();
                    objBonafideCertificate.sResult = Common.Messages.RecordsSavedSuccessfully;
                }
                else
                {
                    objBonafideCertificate.sResult = Common.Messages.FailedToSaveRecords;
                }
            }


            return Json(objBonafideCertificate);
        }
        #endregion

        #region Calender
        // Calender For College ..
        public ActionResult Calender()
        {
            return View();
        }

        // Get Calender ...
        public JsonResult GetCalender()
        {
            var DayOrderList = new List<DayOrderCalender>();
            DayOrderList = (List<DayOrderCalender>)CMSHandler.FetchData<DayOrderCalender>(null, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchCalender)).DataSource.Data;
            return new JsonResult { Data = DayOrderList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        // Get Calender Events ...
        public JsonResult GetCalenderEvent()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            FetchStudnetOrStaff ObjInfo = new FetchStudnetOrStaff();
            var CalenderEvents = new List<FETCH_CALENDER_EVENTS>();
            var RoleId = (Session[Common.SESSION_VARIABLES.USER_ROLE_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ROLE_ID].ToString() : string.Empty;
            var UserId = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            if (RoleId == "5")
            {
                CalenderEvents = (List<FETCH_CALENDER_EVENTS>)CMSHandler.FetchData<FETCH_CALENDER_EVENTS>(null, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchCalender), sAcademicYear).DataSource.Data;
            }
            else
            {
                ObjInfo.STAFF_ID = UserId;
                CalenderEvents = (List<FETCH_CALENDER_EVENTS>)CMSHandler.FetchData<FETCH_CALENDER_EVENTS>(null, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchCalender), sAcademicYear).DataSource.Data;
            }

            return new JsonResult { Data = CalenderEvents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        // FetchCalenderBy ID
        public JsonResult FetchCalenderById(string sCalenderId)
        {

            return Json(sResult);
        }

        // FetchDepartment ..
        public string FetchDepartment()
        {
            string sDepartment = string.Empty;
            DataTable dtDepartment = new DataTable();
            dtDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            List<cp_Department_2017> liDepartment = new List<cp_Department_2017>();
            if (dtDepartment != null && dtDepartment.Rows.Count > 0)
            {
                foreach (DataRow item in dtDepartment.Rows)
                {
                    sDepartment += "<option value='" + item[Common.CP_DEPARTMENT_2017.DEPARTMENT_ID].ToString() + "'>" + item[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString() + "</option>";
                }
            }

            return sDepartment;
        }

        // Insert Calender ..
        public JsonResult InsertCalender(string CalenderJson)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var Result = JsonConvert.DeserializeObject<CALENDER_EVENTS>(CalenderJson);
            Result.EVENT_START_DATE = CommonMethods.ConvertdatetoyearFromat(Result.EVENT_START_DATE);
            Result.EVENT_END_DATE = CommonMethods.ConvertdatetoyearFromat(Result.EVENT_END_DATE);
            Result.USER_ID = (Session[Common.SESSION_VARIABLES.USER_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ID].ToString() : string.Empty;
            var RoleId = (Session[Common.SESSION_VARIABLES.USER_ROLE_ID] != null) ? Session[Common.SESSION_VARIABLES.USER_ROLE_ID].ToString() : string.Empty;
            if (RoleId == "5")
            {
                Result.RESPONSIBLE_STUDENT = (Session[Common.SESSION_VARIABLES.USERNAME] != null) ? Session[Common.SESSION_VARIABLES.USERNAME].ToString() : string.Empty;
            }
            else
            {
                Result.RESPONSIBLE_STAFF = (Session[Common.SESSION_VARIABLES.USERNAME] != null) ? Session[Common.SESSION_VARIABLES.USERNAME].ToString() : string.Empty;
            }
            var Retrun = CMSHandler.SaveOrUpdate(Result, StudentSQL.GetStudentSQL(StudentSQLCommads.InsertCalender), sAcademicYear).DataSource.Data;

            return Json(sResult);
        }

        // UpdateDayOrder ..
        public JsonResult UpdateDayOrder(string Event)
        {
            var Result = JsonConvert.DeserializeObject<DayOrderCalender>(Event);
            Result.DAY_ORDER_DATE = CommonMethods.ConvertdatetoyearFromat(Result.DAY_ORDER_DATE);
            if (Result.DAY_ORDER_END_DATE != null)
            {
                Result.DAY_ORDER_END_DATE = CommonMethods.ConvertdatetoyearFromat(Result.DAY_ORDER_END_DATE);
            }
            var Return = CMSHandler.SaveOrUpdate(Result, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateDayOrder), "").DataSource.Data;
            if (Return != null)
            {
                sResult = Common.Messages.RecordsSavedSuccessfully;
            }
            else
            {
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return Json(sResult);
        }

        // UpdateEvent ..
        public JsonResult UpdateEvent(string Event)
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            var Result = JsonConvert.DeserializeObject<CALENDER_EVENTS>(Event);
            Result.EVENT_START_DATE = CommonMethods.ConvertdatetoyearFromat(Result.EVENT_START_DATE);
            if (Result.EVENT_END_DATE != null)
            {
                Result.EVENT_END_DATE = CommonMethods.ConvertdatetoyearFromat(Result.EVENT_END_DATE);
            }
            // var CalenderEvents =new  List<CALENDER_EVENTS>();
            var Return = CMSHandler.SaveOrUpdate(Result, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateCalender), sAcademicYear).DataSource.Data;
            if (Return != null)
            {
                sResult = Common.Messages.RecordsSavedSuccessfully;
            }
            else
            {
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return Json(sResult);
        }

        // Delete Calender ...
        public string DeleteCalender(string sCalenderId)
        {
            dv.Clear();
            dv.Add(Common.CALENDER.CALENDER_ID, sCalenderId, EnumCommand.DataType.String);
            dv.Add(Common.CALENDER.IS_DELETED, "1", EnumCommand.DataType.String);
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                resultArgs = objViewModel.DeleteCalender(dv);
                if (resultArgs.Success)
                {
                    sResult = Common.Messages.RecordDeletedSuccessfully;
                }
                else
                {
                    sResult = Common.Messages.FailedToDeletedTryAgain;
                }
            }
            return sResult;
        }

        #endregion

        #region List Students

        // Add New Student ...

        public ActionResult AddStudent()
        {

            return View();
        }

        // Insert Courses ...
        public JsonResult InsertCourses(string str)
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (str != null)
            {
                var Result = JsonConvert.DeserializeObject<StudentCourses>(str);
                dv.Clear();
                dv.Add(Common.STU_COURSES.PROGRAM_ID, Result.program, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.CLASS_ID, Result.Class, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.PART, Result.Part, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.MAIN_SUBJECT, Result.MainSubject, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.SPECIAL_SUBJECT, Result.SplSub, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ALLIED1, Result.Allied1, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ALLIED2, Result.Allied2, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ALLIED3, Result.Allied3, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ALLIED4, Result.Allied4, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ELECTIVE1, Result.Elective1, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ELECTIVE2, Result.Elective2, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ELECTIVE3, Result.Elective3, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ELECTIVE4, Result.Elective4, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
                using (StudentViewModel objViewModel = new StudentViewModel())
                {
                    resultArgs = objViewModel.InsertCourses(dv);
                    if (resultArgs.Success)
                    {
                        sResult = "Record saved successfully ....!";
                    }
                    else
                    {
                        sResult = "Record not saved successfully ...!";
                    }
                }
            }
            return Json(sResult);
        }

        // List Student Details ..
        [UserRights("STAFF")]
        public ActionResult StudentList()
        {
            return View();
        }

        // List Table For Student Info ...

        public ActionResult ListStudent()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sAcademicYear))
            {
                Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] = null;
                DataTable dtFetchStudent = new DataTable();
                using (StudentViewModel objStudent = new StudentViewModel())
                {
                    dtFetchStudent = objStudent.ListStudent(sAcademicYear).DataSource.Table;
                }
                IList<StudentInfo> liList = new List<StudentInfo>();
                if (dtFetchStudent != null && dtFetchStudent.Rows.Count > 0)
                {
                    liList = (from DataRow dr in dtFetchStudent.Rows
                              select new StudentInfo
                              {
                                  STUDENTID = dr[Common.STU_PERSONAL_INFO.STUDENT_ID].ToString(),
                                  ROLLNO = dr[Common.STU_PERSONAL_INFO.ROLL_NO].ToString(),
                                  FIRSTNAME = dr[Common.STU_PERSONAL_INFO.FIRST_NAME].ToString(),
                                  DEPTID = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString(),
                                  CLASS_NAME = dr[Common.CP_CLASSES_2017.CLASS_NAME].ToString(),
                                  SHIFTID = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString(),
                                  IS_LEFT = dr[Common.STU_PERSONAL_INFO.IS_LEFT].ToString()
                              }).ToList();
                    return View(liList);
                }
                else
                    return View();
            }
            else
            {
                sResult = "Session Has Expired Login And Try Again ...!";
                return Json(sResult, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Fetch Methods

        // Fetch Course By Department ...
        public string FetchProgrammeByID(string Department)
        {
            string sOption = string.Empty;
            dv.Clear();
            dv.Add(Common.CP_PROGRAMME_2017.DEPARTMENT, Department, EnumCommand.DataType.String);
            DataTable dtFetchProgramme = new DataTable();
            using (StudentViewModel objStudent = new StudentViewModel())
            {
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
                dtFetchProgramme = objStudent.FetchProgrammeByID(dv, sAcademicYear).DataSource.Table;
            }

            if (dtFetchProgramme != null && dtFetchProgramme.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchProgramme.Rows)
                {
                    sOption += "<option value='" + item[Common.CP_PROGRAMME_2017.PROGRAMME_ID].ToString() + "' >" + item[Common.CP_PROGRAMME_2017.PROGRAMME_NAME].ToString() + "</option>";
                }
            }
            return sOption;
        }

        // Fetch Course By Department ...
        public string FetchClassByID(string ShiftID)
        {
            string sOption = string.Empty;
            dv.Clear();
            dv.Add(Common.CP_CLASSES_2017.SHIFT, ShiftID, EnumCommand.DataType.String);
            DataTable dtFetchClass = new DataTable();
            using (StudentViewModel objStudent = new StudentViewModel())
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                dtFetchClass = objStudent.FetchClassesByID(dv, sAcademicYear).DataSource.Table;
            }

            if (dtFetchClass != null && dtFetchClass.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchClass.Rows)
                {
                    sOption += "<option value='" + item[Common.CP_CLASSES_2017.CLASS_ID].ToString() + "' >" + item[Common.CP_CLASSES_2017.CLASS_NAME].ToString() + "</option>";
                }
            }
            return sOption;
        }

        // Fetch Student DropDown ...

        public ActionResult EditStudentInfomation(string ID)
        {
            Session[Common.SESSION_VARIABLES.STUDENT_ID] = ID;
            StudentInfo objStudentInfo = new StudentInfo();

            return View();
        }

        // Fetch Student Courses ...
        public JsonResult FetchCourses(string sStudentID)
        {
            dv.Clear();
            dv.Add(Common.STU_COURSES.STUDENT_ID, sStudentID, EnumCommand.DataType.String);
            DataTable dtCourses = new DataTable();
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                dtCourses = objViewModel.FetchCourses(dv).DataSource.Table;
            }
            if (dtCourses != null && dtCourses.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtCourses.Rows[0][Common.STU_COURSES.COURSE_REGISTRATION_ID].ToString();
                StudentCourses objCourses = new StudentCourses()
                {
                    program = dtCourses.Rows[0][Common.STU_COURSES.PROGRAM_ID].ToString(),
                    Class = dtCourses.Rows[0][Common.STU_COURSES.CLASS_ID].ToString(),
                    Allied1 = dtCourses.Rows[0][Common.STU_COURSES.ALLIED1].ToString(),
                    Allied2 = dtCourses.Rows[0][Common.STU_COURSES.ALLIED2].ToString(),
                    Allied3 = dtCourses.Rows[0][Common.STU_COURSES.ALLIED3].ToString(),
                    Allied4 = dtCourses.Rows[0][Common.STU_COURSES.ALLIED4].ToString(),
                    MainSubject = dtCourses.Rows[0][Common.STU_COURSES.MAIN_SUBJECT].ToString(),
                    Part = dtCourses.Rows[0][Common.STU_COURSES.PART].ToString(),
                    SplSub = dtCourses.Rows[0][Common.STU_COURSES.SPECIAL_SUBJECT].ToString(),
                    Elective1 = dtCourses.Rows[0][Common.STU_COURSES.ELECTIVE1].ToString(),
                    Elective2 = dtCourses.Rows[0][Common.STU_COURSES.ELECTIVE2].ToString(),
                    Elective3 = dtCourses.Rows[0][Common.STU_COURSES.ELECTIVE3].ToString(),
                    Elective4 = dtCourses.Rows[0][Common.STU_COURSES.ELECTIVE4].ToString()
                };
                return Json(objCourses);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }

        }

        #endregion

        #region Edit Methods

        // Edit Student Course ..
        public JsonResult EditCourses(string str)
        {
            string sStudentID = (Session[Common.SESSION_VARIABLES.STUDENT_ID] != null) ? Session[Common.SESSION_VARIABLES.STUDENT_ID].ToString() : string.Empty;
            if (str != null)
            {
                var Result = JsonConvert.DeserializeObject<StudentCourses>(str);
                dv.Clear();
                dv.Add(Common.STU_COURSES.PROGRAM_ID, Result.program, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.CLASS_ID, Result.Class, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.PART, Result.Part, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.MAIN_SUBJECT, Result.MainSubject, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.SPECIAL_SUBJECT, Result.SplSub, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ALLIED1, Result.Allied1, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ALLIED2, Result.Allied2, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ALLIED3, Result.Allied3, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ALLIED4, Result.Allied4, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ELECTIVE1, Result.Elective1, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ELECTIVE2, Result.Elective2, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ELECTIVE3, Result.Elective3, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.ELECTIVE4, Result.Elective4, EnumCommand.DataType.String);
                dv.Add(Common.STU_COURSES.STUDENT_ID, sStudentID, EnumCommand.DataType.String);

                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    string sCourseID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STU_COURSES.COURSE_REGISTRATION_ID, sCourseID, EnumCommand.DataType.String);
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.UpdateCourses(dv);
                        if (resultArgs.Success)
                        {
                            sResult = "Record updated successfully ...!";
                        }
                        else
                        {
                            sResult = "Record not updated try again ...!";
                        }
                    }
                }
                else
                {
                    using (StudentViewModel objViewModel = new StudentViewModel())
                    {
                        resultArgs = objViewModel.InsertCourses(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record saved successfully ...!";
                        }
                        else
                        {
                            sResult = "Record not saved try again ...!";
                        }
                    }
                }
            }
            return Json(sResult);
        }

        #endregion

        #region Delete Method 
        // Delete Student Details ...
        public string DeleteStudent(string sStudentId, string UserName)
        {
            dv.Clear();
            dv.Add(Common.STU_PERSONAL_INFO.STUDENT_ID, sStudentId, EnumCommand.DataType.String);
            dv.Add(Common.STU_PERSONAL_INFO.IS_DELETED, "1", EnumCommand.DataType.String);
            var ObjUserAccount = new USER_ACCOUNT_INFO();
            var liUserAccount = new List<USER_ACCOUNT_INFO>();
            ObjUserAccount.USERNAME = UserName;
            liUserAccount = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(ObjUserAccount, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserAccountByName)).DataSource.Data;
            ObjUserAccount.USER_ACCOUNT_ID = liUserAccount.FirstOrDefault().USER_ACCOUNT_ID;
            resultArgs = CMSHandler.SaveOrUpdate(ObjUserAccount, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.DeleteUserAccount));
            using (StudentViewModel objViewModel = new StudentViewModel())
            {
                resultArgs = objViewModel.DeleteStudent(dv);
                if (resultArgs.Success)
                {
                    sResult = "Record deleted successfully ...!";
                }
                else
                {
                    sResult = "Record not deleted try again ...!";
                }
            }
            return sResult;
        }
        #endregion

        #region Student Promotion
        public ActionResult StudentPromotionList()
        {
            StudentViewModel objViewModel = new StudentViewModel();
            var Shift = new List<Sup_Shift>();
            var Class = new List<cp_Classes_2017>();
            var Programme = new List<cp_Program_2017>();
            var AcademicYear = new List<SUP_ACADEMIC_YEAR_LIST>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Shift = (List<Sup_Shift>)CMSHandler.FetchData<Sup_Shift>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchShift)).DataSource.Data;
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
            AcademicYear = (List<SUP_ACADEMIC_YEAR_LIST>)CMSHandler.FetchData<SUP_ACADEMIC_YEAR_LIST>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearList)).DataSource.Data;
            Programme = (List<cp_Program_2017>)CMSHandler.FetchData<cp_Program_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgram), sAcademicYear).DataSource.Data;
            objViewModel.ShiftList = new SelectList(Shift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            objViewModel.ClassList = new SelectList(Class, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_CODE);
            objViewModel.AcademicYearList = new SelectList(AcademicYear, Common.ACADEMIC_YEAR.ACADEMIC_YEAR_ID, Common.ACADEMIC_YEAR.AC_YEAR);
            objViewModel.ProgrammeList = new SelectList(Programme, Common.CP_PROGRAMME_2017.PROGRAMME_ID, Common.CP_PROGRAMME_2017.PROGRAMME_DESCRIPTION);
            return View(objViewModel);
        }
        public string GetClassIdByShiftId(string sShiftId, string AcademicYear)
        {
            string sOption = string.Empty;
            string sAcademicYear = AcademicYear;
            StudentViewModel objViewModel = new StudentViewModel();
            StudentPromotionList objModel = new StudentPromotionList();
            objModel.SHIFT = sShiftId;
            var AcademicYearList = new SUP_ACADEMIC_YEAR_LIST();
            AcademicYearList.ACADEMIC_YEAR_ID = AcademicYear;
            var FetchAcademicYearById = (List<SUP_ACADEMIC_YEAR_LIST>)CMSHandler.FetchData<SUP_ACADEMIC_YEAR_LIST>(AcademicYearList, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearById)).DataSource.Data;
            foreach (var item in FetchAcademicYearById)
                sAcademicYear = FetchAcademicYearById.SingleOrDefault().AC_YEAR;
            var Class = new List<cp_Classes_2017>();
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByShiftId), sAcademicYear).DataSource.Data;
            if (Class != null && Class.Count > 0)
            {
                sOption = "<option value='" + "" + "' >" + "--select--" + "</option>";
                foreach (var List in Class)
                    sOption += "<option value='" + List.CLASS_ID + "' >" + List.CLASS_NAME + "</option>";
            }
            return sOption;
        }
        public string GetClassIdByAcademicYear(string AcademicYear)
        {
            string sOption = string.Empty;
            string sAcademicYear = AcademicYear;
            StudentViewModel objViewModel = new StudentViewModel();
            StudentPromotionList objModel = new StudentPromotionList();
            var AcademicYearList = new SUP_ACADEMIC_YEAR_LIST();
            AcademicYearList.ACADEMIC_YEAR_ID = AcademicYear;
            var FetchAcademicYearById = (List<SUP_ACADEMIC_YEAR_LIST>)CMSHandler.FetchData<SUP_ACADEMIC_YEAR_LIST>(AcademicYearList, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearById)).DataSource.Data;
            foreach (var item in FetchAcademicYearById)
                sAcademicYear = FetchAcademicYearById.SingleOrDefault().AC_YEAR;
            var Class = new List<cp_Classes_2017>();
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
            if (Class != null && Class.Count > 0)
            {
                sOption = "<option value='" + "" + "' >" + "--select--" + "</option>";
                foreach (var List in Class)
                    sOption += "<option value='" + List.CLASS_ID + "' >" + List.CLASS_NAME + "</option>";
            }
            return sOption;
        }
        public async Task<ActionResult> BindStudentToPromote(string sClassId)
        {
            StudentViewModel objViewModel = new StudentViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var StudentList = new StudentPromotionList();
            StudentList.CLASS_ID = sClassId;
            var ListStudentToPromote = await Task.Run(() => (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(StudentList, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStudentByClassId), sAcademicYear).DataSource.Data);
            objViewModel.lstStudentPromotion = ListStudentToPromote;
            return View(objViewModel);
        }
        public ActionResult BindSelectedClassList(string cAcademicYear, string pAcademicYear, string pProgramme, string cProgramme, string pShift, string cShift)
        {
            StudentViewModel objViewModel = new StudentViewModel();
            StudentPromotionList objModel = new StudentPromotionList();
            var AcademicYearList = new SUP_ACADEMIC_YEAR_LIST();
            string sAcademicYear = string.Empty;
            AcademicYearList.ACADEMIC_YEAR_ID = cAcademicYear;
            var FetchAcademicYearById = (List<SUP_ACADEMIC_YEAR_LIST>)CMSHandler.FetchData<SUP_ACADEMIC_YEAR_LIST>(AcademicYearList, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearById)).DataSource.Data;
            foreach (var item in FetchAcademicYearById)
                sAcademicYear = FetchAcademicYearById.SingleOrDefault().AC_YEAR;
            objModel.PROGRAMME = cProgramme;
            objModel.SHIFT = cShift;
            var CurrentClassList = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchClassesByProgrammeId), sAcademicYear).DataSource.Data;
            objViewModel.lstStudentPromotion = CurrentClassList;
            AcademicYearList.ACADEMIC_YEAR_ID = pAcademicYear;
            FetchAcademicYearById = (List<SUP_ACADEMIC_YEAR_LIST>)CMSHandler.FetchData<SUP_ACADEMIC_YEAR_LIST>(AcademicYearList, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearById)).DataSource.Data;
            foreach (var list in FetchAcademicYearById)
                sAcademicYear = FetchAcademicYearById.SingleOrDefault().AC_YEAR;
            objModel.PROGRAMME = cProgramme;
            objModel.SHIFT = cShift;
            var PromotionClassList = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchClassesByProgrammeId), sAcademicYear).DataSource.Data;
            objViewModel.ClassList = new SelectList(PromotionClassList, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_CODE);
            return View(objViewModel);
        }
        public string GetProgrammeByShiftId(string sShiftId, string AcademicYear)
        {
            string sOption = string.Empty;
            string sAcademicYear = AcademicYear;
            StudentViewModel objViewModel = new StudentViewModel();
            StudentPromotionList objModel = new StudentPromotionList();
            objModel.SHIFT = sShiftId;
            var AcademicYearList = new SUP_ACADEMIC_YEAR_LIST();
            AcademicYearList.ACADEMIC_YEAR_ID = AcademicYear;
            var FetchAcademicYearById = (List<SUP_ACADEMIC_YEAR_LIST>)CMSHandler.FetchData<SUP_ACADEMIC_YEAR_LIST>(AcademicYearList, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearById)).DataSource.Data;
            foreach (var item in FetchAcademicYearById)
                sAcademicYear = FetchAcademicYearById.SingleOrDefault().AC_YEAR;
            var Programme = new List<cp_Program_2017>();
            Programme = (List<cp_Program_2017>)CMSHandler.FetchData<cp_Program_2017>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgramByShiftId), sAcademicYear).DataSource.Data;
            if (Programme != null && Programme.Count > 0)
            {
                sOption = "<option value='" + "" + "' >" + "--select--" + "</option>";
                foreach (var List in Programme)
                    sOption += "<option value='" + List.PROGRAMME_ID + "' >" + List.PROGRAMME_DESCRIPTION + "</option>";
            }
            return sOption;
        }
        public string UpgradeStuClass(string sStuClass)
        {
            string sOption = string.Empty;
            try
            {
                StudentPromotionList objModel = new StudentPromotionList();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JsonStudentPromotionList objJson = JsonConvert.DeserializeObject<JsonStudentPromotionList>(sStuClass);
                string sAcademicYear = string.Empty;
                foreach (var item in objJson.StudentPromotionList)
                {
                    var AcademicYearList = new SUP_ACADEMIC_YEAR_LIST();
                    AcademicYearList.ACADEMIC_YEAR_ID = item.ACADEMIC_YEAR;
                    var FetchAcademicYearById = (List<SUP_ACADEMIC_YEAR_LIST>)CMSHandler.FetchData<SUP_ACADEMIC_YEAR_LIST>(AcademicYearList, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearById)).DataSource.Data;
                    foreach (var list in FetchAcademicYearById)
                        item.ACADEMIC_YEAR = FetchAcademicYearById.SingleOrDefault().AC_YEAR;
                    sAcademicYear = item.ACADEMIC_YEAR;
                    var FetchStuClassByStudentId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuclassByStudentIdAndAcademicYear)).DataSource.Data;
                    if (FetchStuClassByStudentId != null && FetchStuClassByStudentId.Count > 0)
                    {
                        item.STU_CLASS_ID = FetchStuClassByStudentId.SingleOrDefault().STU_CLASS_ID;
                        ResultArgs resultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuclass));
                        if (resultArgs.Success)
                        {
                            var FetchDepartmentByClassId = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(item, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDepartmentByClassId), sAcademicYear).DataSource.Data;
                            item.DEPARTMENT = FetchDepartmentByClassId.SingleOrDefault().DEPARTMENT;
                            ResultArgs resultArg = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStudentDepartment));
                            if (resultArg.Success)
                            {
                                sOption = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                sOption = Common.Messages.FailedToSaveRecords;
                            }
                        }
                        else
                        {
                            sOption = Common.Messages.FailedToSaveRecords;
                        }
                    }
                    else
                    {
                        ResultArgs resultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.SaveStuClass));
                        if (resultArgs.Success)
                        {
                            var FetchDepartmentByClassId = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(item, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDepartmentByClassId), sAcademicYear).DataSource.Data;
                            item.DEPARTMENT = FetchDepartmentByClassId.SingleOrDefault().DEPARTMENT;
                            ResultArgs resultArg = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStudentDepartment));
                            if (resultArg.Success)
                            {
                                sOption = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                sOption = Common.Messages.FailedToSaveRecords;
                            }
                        }
                        else
                        {
                            sOption = Common.Messages.FailedToSaveRecords;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("UpgradeStuClass", "Student", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("UpgradeStuClass", "Student", ex.Message);
                }
            }
            return sOption;
        }
        public string ClassWisePromotion(string sStudentclass)
        {
            string sOption = string.Empty;
            StudentPromotionList objModel = new StudentPromotionList();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            JsonStudentPromotionList objJson = JsonConvert.DeserializeObject<JsonStudentPromotionList>(sStudentclass);
            foreach (var item in objJson.StudentPromotionList)
            {
                var AcademicYearList = new SUP_ACADEMIC_YEAR_LIST();
                AcademicYearList.ACADEMIC_YEAR_ID = item.pACADEMIC_YEAR;
                item.CLASS_ID = item.pCLASS_ID;
                var FetchAcademicYearById = (List<SUP_ACADEMIC_YEAR_LIST>)CMSHandler.FetchData<SUP_ACADEMIC_YEAR_LIST>(AcademicYearList, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearById)).DataSource.Data;
                foreach (var list in FetchAcademicYearById)
                    item.ACADEMIC_YEAR = FetchAcademicYearById.SingleOrDefault().AC_YEAR;
                var FetchStuClassByClassIdAndAcademicYear = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuClassByClassId)).DataSource.Data;
                AcademicYearList.ACADEMIC_YEAR_ID = item.cACADEMIC_YEAR;
                FetchAcademicYearById = (List<SUP_ACADEMIC_YEAR_LIST>)CMSHandler.FetchData<SUP_ACADEMIC_YEAR_LIST>(AcademicYearList, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchAcademicYearById)).DataSource.Data;
                foreach (var list in FetchAcademicYearById)
                    item.cACADEMIC_YEAR = FetchAcademicYearById.SingleOrDefault().AC_YEAR;
                if (FetchStuClassByClassIdAndAcademicYear != null && FetchStuClassByClassIdAndAcademicYear.Count > 0)
                {
                    foreach (var dataitem in FetchStuClassByClassIdAndAcademicYear)
                    {

                        objModel.ACADEMIC_YEAR = item.pACADEMIC_YEAR;
                        objModel.STUDENT_ID = dataitem.STUDENT_ID;
                        objModel.CLASS_ID = item.pCLASS_ID;
                        var FetchStuClassByStudentId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuclassByStudentIdAndAcademicYear)).DataSource.Data;
                        if (FetchStuClassByStudentId != null && FetchStuClassByStudentId.Count > 0)
                        {
                            objModel.STU_CLASS_ID = FetchStuClassByStudentId.SingleOrDefault().STU_CLASS_ID;
                            ResultArgs resultArgs = CMSHandler.SaveOrUpdate(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuclass));
                            if (resultArgs.Success)
                            {
                                sOption = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                sOption = Common.Messages.FailedToSaveRecords;
                            }
                        }
                        else
                        {
                            ResultArgs resultArgs = CMSHandler.SaveOrUpdate(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.SaveStuClass));
                            if (resultArgs.Success)
                            {
                                sOption = Common.Messages.RecordsSavedSuccessfully;
                            }
                            else
                            {
                                sOption = Common.Messages.FailedToSaveRecords;
                            }
                        }
                    }
                }
                else
                {
                    objModel.ACADEMIC_YEAR = item.cACADEMIC_YEAR;
                    objModel.CLASS_ID = item.cCLASS_ID;
                    var FetchStuClassByClassId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuClassByClassId)).DataSource.Data;
                    foreach (var list in FetchStuClassByClassId)
                    {
                        item.STUDENT_ID = list.STUDENT_ID;
                        resultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.SaveStuClass));
                        if (resultArgs.Success)
                        {
                            sOption = Common.Messages.RecordsSavedSuccessfully;
                        }
                        else
                        {
                            sOption = Common.Messages.FailedToSaveRecords;
                        }
                    }
                }
            }
            return sOption;
        }
        public ActionResult StudentTransferList()
        {
            StudentViewModel objViewModel = new StudentViewModel();
            var Class = new List<cp_Classes_2017>();
            var ClassYear = new List<SubClassYear>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
            ClassYear = (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear)).DataSource.Data;
            objViewModel.ClassList = new SelectList(Class, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            objViewModel.ClassYear = new SelectList(ClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.SUP_CLASS_YEAR.CLASS_YEAR);
            return View(objViewModel);
        }
        public string GetClassByClassYear(string sClassYearId)
        {
            string sOption = string.Empty;
            StudentViewModel objViewModel = new StudentViewModel();
            StudentPromotionList objModel = new StudentPromotionList();
            objModel.CLASS_YEAR = sClassYearId;
            var Class = new List<cp_Classes_2017>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByClassYear), sAcademicYear).DataSource.Data;
            if (Class != null && Class.Count > 0)
            {
                sOption = "<option value='" + "" + "' >" + "--select--" + "</option>";
                foreach (var List in Class)
                    sOption += "<option value='" + List.CLASS_ID + "' >" + List.CLASS_NAME + "</option>";
            }
            return sOption;
        }
        public async Task<ActionResult> BindStudentToTransfer(string sClassId)
        {
            StudentViewModel objViewModel = new StudentViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var StudentList = new StudentPromotionList();
            StudentList.CLASS_ID = sClassId;
            var ListStudentToPromote = await Task.Run(() => (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(StudentList, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStudentByClassId), sAcademicYear).DataSource.Data);
            objViewModel.lstStudentPromotion = ListStudentToPromote;
            return View(objViewModel);
        }
        public string GetClassForTransfer(string sClassId)
        {
            string sOption = string.Empty;
            StudentViewModel objViewModel = new StudentViewModel();
            StudentPromotionList objModel = new StudentPromotionList();
            objModel.CLASS_ID = sClassId;
            var ClassYearAndProgramme = new List<cp_Classes_2017>();
            var Class = new List<cp_Classes_2017>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            ClassYearAndProgramme = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchProgrammeAndClassYearByClassId), sAcademicYear).DataSource.Data;
            foreach (var item in ClassYearAndProgramme)
            {
                objModel.CLASS_YEAR = item.CLASS_YEAR;
                objModel.PROGRAMME = item.PROGRAMME;
            }
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByProgrammeAndClassYear), sAcademicYear).DataSource.Data;
            if (Class != null && Class.Count > 0)
            {
                sOption = "<option value='" + "" + "' >" + "--select--" + "</option>";
                foreach (var List in Class)
                    sOption += "<option value='" + List.CLASS_ID + "' >" + List.CLASS_NAME + "</option>";
            }
            return sOption;
        }
        public string GetClassByClassYearAndClassLevel(string sClassYearId, string sClassId)
        {
            string sOption = string.Empty;
            StudentViewModel objViewModel = new StudentViewModel();
            StudentPromotionList objModel = new StudentPromotionList();
            objModel.CLASS_ID = sClassId;
            objModel.CLASS_YEAR = sClassYearId;
            var ClassLevel = new List<cp_Classes_2017>();
            var Class = new List<cp_Classes_2017>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            ClassLevel = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassLevelByClassId), sAcademicYear).DataSource.Data;
            objModel.CLASS_LEVEL = ClassLevel.SingleOrDefault().CLASS_LEVEL;
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByClassLevelAndClassYear), sAcademicYear).DataSource.Data;
            if (Class != null && Class.Count > 0)
            {
                sOption = "<option value='" + "" + "' >" + "--select--" + "</option>";
                foreach (var List in Class)
                    sOption += "<option value='" + List.CLASS_ID + "' >" + List.CLASS_NAME + "</option>";
            }
            return sOption;
        }
        public string UpdateStuClassAndStuCourseInfo(string sStuClass)
        {
            string sOption = string.Empty;
            try
            {
                StudentPromotionList objModel = new StudentPromotionList();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JsonStudentPromotionList objJson = JsonConvert.DeserializeObject<JsonStudentPromotionList>(sStuClass);
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                if (objJson != null)
                {
                    foreach (var item in objJson.StudentPromotionList)
                    {
                        item.ACADEMIC_YEAR = sAcademicYear;
                        var FetchCurrentProgrammeByStudentId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchCurrentProgrammeByStudentId), sAcademicYear).DataSource.Data;
                        objModel.cPROGRAMME = FetchCurrentProgrammeByStudentId.SingleOrDefault().PROGRAMME;
                        var FetchTransferedProgrammeByStudentId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchTransferedProgrammeByStudentId), sAcademicYear).DataSource.Data;
                        objModel.tPROGRAMME = FetchTransferedProgrammeByStudentId.SingleOrDefault().PROGRAMME;
                        if (objModel.cPROGRAMME == objModel.tPROGRAMME)
                        {
                            var FetchStuCourseInfoOnlyNMEByStudentId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuCourseInfoOnlyNMEByStudentId)).DataSource.Data;
                            var FetchStuClassByStudentId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuclassByStudentIdAndAcademicYear)).DataSource.Data;
                            item.STU_CLASS_ID = FetchStuClassByStudentId.SingleOrDefault().STU_CLASS_ID;
                            ResultArgs resultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuclass));
                            if (resultArgs.Success)
                            {
                                var FetchShiftByClassId = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchShiftByClassId), sAcademicYear).DataSource.Data;
                                item.SHIFT = FetchShiftByClassId.SingleOrDefault().SHIFT;
                                objModel.PROGRAMME = objModel.tPROGRAMME;
                                ResultArgs ResultArg = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStudentPersonalInfoForStudentTransfer));
                                if (ResultArg.Success)
                                {
                                    ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuCourseInfoForStudentTransferOnlyLanguagePaper), sAcademicYear);
                                    if (ResultArgs.Success)
                                    {
                                        sOption = Common.Messages.RecordsSavedSuccessfully;
                                    }
                                    else
                                    {
                                        sOption = Common.Messages.FailedToSaveRecords;
                                    }
                                }
                                else
                                {
                                    sOption = Common.Messages.FailedToSaveRecords;
                                }
                            }
                            else
                            {
                                sOption = Common.Messages.FailedToSaveRecords;
                            }

                        }
                        else
                        {
                            var FetchCIAMarks = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStudentCIAMarks), sAcademicYear).DataSource.Data;
                            if (FetchCIAMarks != null && FetchCIAMarks.Count > 0)
                            {
                                sOption = "Student can not be transfered...!!!";
                            }
                            else
                            {
                                var FetchStuClassByStudentId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuclassByStudentIdAndAcademicYear)).DataSource.Data;
                                item.STU_CLASS_ID = FetchStuClassByStudentId.SingleOrDefault().STU_CLASS_ID;
                                ResultArgs resultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuclass));
                                if (resultArgs.Success)
                                {
                                    var FetchShiftByClassId = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchShiftByClassId), sAcademicYear).DataSource.Data;
                                    item.SHIFT = FetchShiftByClassId.SingleOrDefault().SHIFT;
                                    objModel.PROGRAMME = objModel.tPROGRAMME;
                                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStudentPersonalInfoForStudentTransfer));
                                    if (ResultArg.Success)
                                    {
                                        ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuCourseInfoForStudentTransferOnlyLanguagePaper), sAcademicYear);
                                        if (ResultArgs.Success)
                                        {
                                            sOption = Common.Messages.RecordsSavedSuccessfully;
                                        }
                                        else
                                        {
                                            sOption = Common.Messages.FailedToSaveRecords;
                                        }
                                    }
                                    else
                                    {
                                        sOption = Common.Messages.FailedToSaveRecords;
                                    }
                                }
                                else
                                {
                                    sOption = Common.Messages.FailedToSaveRecords;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("UpdateStuClassAndStuCourseInfo", "Student", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("UpdateStuClassAndStuCourseInfo", "Student", ex.Message);
                }
            }
            return sOption;
        }
        #endregion

        #region Stu Course Info
        [UserRights("ADMIN")]
        public async Task<ActionResult> StuCourseInfoList()
        {
            StudentViewModel objViewModel = new StudentViewModel();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var ClassYear = (List<SubClassYear>)CMSHandler.FetchData<SubClassYear>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassYear)).DataSource.Data;
            var FetchClass = await Task.Run(() => (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data);
            objViewModel.ClassList = new SelectList(FetchClass, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            objViewModel.ClassYear = new SelectList(ClassYear, Common.SUP_CLASS_YEAR.CLASS_YEAR_ID, Common.SUP_CLASS_YEAR.CLASS_YEAR);
            return View(objViewModel);
        }
        public async Task<ActionResult> BindStudentCourseInfo(string sClassId)
        {
            StudentViewModel objViewModel = new StudentViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var StudentList = new Stu_Course_Info();
            StudentList.CLASS_ID = sClassId;
            var BindStudentCourseInfo = await Task.Run(() => (List<Stu_Course_Info>)CMSHandler.FetchData<Stu_Course_Info>(StudentList, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStuCourseInfobyClassId), sAcademicYear).DataSource.Data);
            objViewModel.lstStuCourseInfo = BindStudentCourseInfo;
            return View(objViewModel);
        }

        public ActionResult StuCourseInfo()
        {
            StudentViewModel objViewModel = new StudentViewModel();
            var Class = new List<cp_Classes_2017>();
            var Course = new List<cp_Course_Root_2017>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Class = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(null, SQL.SupportData.SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchClassByAcademicYear), sAcademicYear).DataSource.Data;
            Course = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(null, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchCourseListForStuCourseInfo), sAcademicYear).DataSource.Data;
            objViewModel.ClassList = new SelectList(Class, Common.CP_CLASSES_2017.CLASS_ID, Common.CP_CLASSES_2017.CLASS_NAME);
            objViewModel.CourseList = new SelectList(Course, Common.CP_COURSE_ROOT_2017.COURSE_ROOT_ID, Common.CP_COURSE_ROOT_2017.COURSE_TITLE);
            return View(objViewModel);
        }
        public string BindCourseByClassId(string sClassId)
        {
            string sOption = string.Empty;
            Stu_Course_Info objModel = new Stu_Course_Info();
            objModel.CLASS_ID = sClassId;
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            var FetchBatchAndProgramByClassId = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchProgrammeAndBatchByClassId), sAcademicYear).DataSource.Data;
            if (FetchBatchAndProgramByClassId != null && FetchBatchAndProgramByClassId.Count > 0)
            {
                objModel.BATCH = FetchBatchAndProgramByClassId.FirstOrDefault().BATCH;
                objModel.PROGRAMME = FetchBatchAndProgramByClassId.FirstOrDefault().PROGRAMME;
            }

            var FetchCourseByProgramAndBatch = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchCourseByProgrammeAndBatch), sAcademicYear).DataSource.Data;
            if (FetchCourseByProgramAndBatch != null && FetchCourseByProgramAndBatch.Count > 0)
            {
                sOption += "<option value='" + "" + "' >" + "--select--" + "</option>";
                foreach (var List in FetchCourseByProgramAndBatch)
                    sOption += "<option value='" + List.COURSE_ROOT_ID + "' >" + List.COURSE_TITLE + "</option>";
            }
            return sOption;
        }
        public ActionResult BindStudentsByClassId(string sClassId)
        {
            StudentViewModel objViewModel = new StudentViewModel();
            Stu_Course_Info objModel = new Stu_Course_Info();
            objModel.CLASS_ID = sClassId;
            var Student = new List<Stu_Course_Info>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Student = (List<Stu_Course_Info>)CMSHandler.FetchData<Stu_Course_Info>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.BindStudentPersonalInfoByClassId), sAcademicYear).DataSource.Data;
            objViewModel.lstStuCourseInfo = Student;
            return View(objViewModel);
        }
        public ActionResult BindSelectedStudentsByCourseId(string sCourseId, string sClassId)
        {
            StudentViewModel objViewModel = new StudentViewModel();
            Stu_Course_Info objModel = new Stu_Course_Info();
            objModel.CLASS_ID = sClassId;
            objModel.COURSE_ID = sCourseId;
            var Student = new List<Stu_Course_Info>();
            var Shift = new List<cp_Classes_2017>();
            var SelectedClassId = new List<cp_Classes_2017>();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            Shift = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchShiftByClassId), sAcademicYear).DataSource.Data;
            objModel.SHIFT = Shift.SingleOrDefault().SHIFT;
            SelectedClassId = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchSelectedClassIdByCourseIdAndShiftId), sAcademicYear).DataSource.Data;
            if (SelectedClassId != null && SelectedClassId.Count > 0)
            {
                objModel.S_CLASS_ID = SelectedClassId.SingleOrDefault().CLASS_ID;
            }
            else
            {
                objModel.S_CLASS_ID = sClassId;
            }
            Student = (List<Stu_Course_Info>)CMSHandler.FetchData<Stu_Course_Info>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.BindSelectedStudentsByCourseId), sAcademicYear).DataSource.Data;
            objViewModel.lstStuCourseInfo = Student;
            return View(objViewModel);
        }
        public string UpdateStuCourseInfo(string sStuCourseInfo)
        {
            string sOption = string.Empty;
            try
            {
                StudentViewModel objViewModel = new StudentViewModel();
                string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var objJson = JsonConvert.DeserializeObject<JsonStuCourseInfo>(sStuCourseInfo);
                Stu_Course_Info objModel = new Stu_Course_Info();
                if (objJson != null)
                {
                    foreach (var item in objJson.SelectedStudentList)
                    {
                        var CheckClassCourse = (List<ClassCourse>)CMSHandler.FetchData<ClassCourse>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.CheckClassCourse), sAcademicYear).DataSource.Data;
                        if (CheckClassCourse != null && CheckClassCourse.Count > 0)
                        {
                            sOption += item.ROLL_NO + "-This is not an Optional Paper...!!!\n";
                        }
                        else
                        {
                            var FetchShiftByClassId = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchShiftByClassId), sAcademicYear).DataSource.Data;
                            if (FetchShiftByClassId != null && FetchShiftByClassId.Count > 0)
                                item.SHIFT = FetchShiftByClassId.SingleOrDefault().SHIFT;
                            var FetchSemesterByShiftId = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(item, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchSemesterByCourseId), sAcademicYear).DataSource.Data;
                            if (FetchSemesterByShiftId != null && FetchSemesterByShiftId.Count > 0)
                                item.SEMESTER = FetchSemesterByShiftId.SingleOrDefault().SEMESTER_ID;
                            var CheckOptinalCourse = (List<cp_Classes_2017>)CMSHandler.FetchData<cp_Classes_2017>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchSelectedClassIdByCourseIdAndShiftId), sAcademicYear).DataSource.Data;
                            if (CheckOptinalCourse != null && CheckOptinalCourse.Count > 0)
                            {
                                item.S_CLASS_ID = CheckOptinalCourse.SingleOrDefault().CLASS_ID;
                                var CheckIsNMECourse = (List<cp_Course_Root_2017>)CMSHandler.FetchData<cp_Course_Root_2017>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.CheckIsNMECourse), sAcademicYear).DataSource.Data;
                                if (CheckIsNMECourse != null && CheckIsNMECourse.Count > 0)
                                {
                                    //var FetchStudentCourseByCourseIdAndStudentId = (List<Stu_Course_Info>)CMSHandler.FetchData<Stu_Course_Info>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStudentCourseByCourseIdAndStudentId), sAcademicYear).DataSource.Data;
                                    //var FetchSeletectClassIdByStudentIdAndCourseId= (List<Stu_Course_Info>)CMSHandler.FetchData<Stu_Course_Info>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchSelectedClassIdByCourseIdAndShiftId), sAcademicYear).DataSource.Data;
                                    //if (FetchSeletectClassIdByStudentIdAndCourseId != null && FetchSeletectClassIdByStudentIdAndCourseId.Count > 0)
                                    //    item.S_CLASS_ID = FetchSeletectClassIdByStudentIdAndCourseId.SingleOrDefault().CLASS_ID;
                                    var FetchStudentCourseByStudentId = (List<Stu_Course_Info>)CMSHandler.FetchData<Stu_Course_Info>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStudentCourseByStudentId), sAcademicYear).DataSource.Data;
                                    if (FetchStudentCourseByStudentId != null && FetchStudentCourseByStudentId.Count > 0)
                                    {
                                        objModel.COURSE_ID = FetchStudentCourseByStudentId.FirstOrDefault().COURSE_ID;
                                        item.COURSE_INFO_ID = FetchStudentCourseByStudentId.FirstOrDefault().COURSE_INFO_ID;
                                        objModel.STUDENT_ID = item.STUDENT_ID;
                                        var FetchCIAMarksByStudentIdAndCourseId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStudentCIAMarksByStudentIdAndCourseId), sAcademicYear).DataSource.Data;
                                        if (FetchCIAMarksByStudentIdAndCourseId != null && FetchCIAMarksByStudentIdAndCourseId.Count > 0)
                                        {
                                            sOption += item.ROLL_NO + "-CIA Mark is entered so Failed To Save.!!! \n";
                                        }
                                        else
                                        {
                                            ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuCourseInfo), sAcademicYear);
                                            if (ResultArgs.Success)
                                            {
                                                sOption += item.ROLL_NO + "-" + Common.Messages.RecordsSavedSuccessfully + "\n";
                                            }
                                            else
                                            {
                                                sOption += item.ROLL_NO + "-" + Common.Messages.FailedToSaveRecords + "\n";
                                            }
                                        }
                                        //objModel.S_COURSE_ID = FetchStudentCourseByStudentId.SingleOrDefault().S_COURSE_ID;
                                        //var FetchCIAMarksByStudentIdAndCourseId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStudentCIAMarksByStudentIdAndCourseId), sAcademicYear).DataSource.Data;
                                    }
                                    else
                                    {
                                        ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.SaveStuCourseInfo), sAcademicYear);
                                        if (ResultArgs.Success)
                                        {
                                            sOption += item.ROLL_NO + "-" + Common.Messages.RecordsSavedSuccessfully + "\n";
                                        }
                                        else
                                        {
                                            sOption += item.ROLL_NO + "-" + Common.Messages.FailedToSaveRecords + "\n";
                                        }
                                    }
                                }
                                else
                                {
                                    var FetchStudentCourseByCourseIdAndStudentId = (List<Stu_Course_Info>)CMSHandler.FetchData<Stu_Course_Info>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStudentCourseByCourseIdAndStudentId), sAcademicYear).DataSource.Data;
                                    //item.SEMESTER = FetchStudentCourseByCourseIdAndStudentId.SingleOrDefault().SEMESTER;
                                    if (FetchStudentCourseByCourseIdAndStudentId != null && FetchStudentCourseByCourseIdAndStudentId.Count > 0)
                                    {
                                        sOption += item.ROLL_NO + "-" + Common.Messages.RecordsSavedSuccessfully + "\n";
                                    }
                                    else
                                    {
                                        ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.SaveStuCourseInfo), sAcademicYear);
                                        if (ResultArgs.Success)
                                        {
                                            sOption = item.ROLL_NO + "-" + Common.Messages.RecordsSavedSuccessfully + "\n";
                                        }
                                        else
                                        {
                                            sOption = item.ROLL_NO + "-" + Common.Messages.FailedToSaveRecords + "\n";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var FetchLanguageCourseId = (List<Stu_Course_Info>)CMSHandler.FetchData<Stu_Course_Info>(item, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchLanguagePaperByStudentId), sAcademicYear).DataSource.Data;
                                if (FetchLanguageCourseId != null && FetchLanguageCourseId.Count > 0)
                                {
                                    objModel.COURSE_ID = FetchLanguageCourseId.FirstOrDefault().COURSE_ID;
                                    item.COURSE_INFO_ID = FetchLanguageCourseId.FirstOrDefault().COURSE_INFO_ID;
                                    objModel.STUDENT_ID = item.STUDENT_ID;
                                    var FetchCIAMarksByStudentIdAndCourseId = (List<StudentPromotionList>)CMSHandler.FetchData<StudentPromotionList>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchStudentCIAMarksByStudentIdAndCourseId), sAcademicYear).DataSource.Data;
                                    if (FetchCIAMarksByStudentIdAndCourseId != null && FetchCIAMarksByStudentIdAndCourseId.Count > 0)
                                    {
                                        sOption += item.ROLL_NO + "-" + "CIA Mark is entered so Failed To Save.!!!" + "\n";
                                    }
                                    else
                                    {
                                        ResultArgs ResultArgs = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateStuCourseInfoOnlyLanguagePaper), sAcademicYear);
                                        if (ResultArgs.Success)
                                        {
                                            sOption += item.ROLL_NO + "-" + Common.Messages.RecordsSavedSuccessfully + "\n";
                                        }
                                        else
                                        {
                                            sOption += item.ROLL_NO + "-" + Common.Messages.FailedToSaveRecords + "\n";
                                        }
                                    }
                                }
                                else
                                {
                                    item.S_CLASS_ID = item.CLASS_ID;
                                    ResultArgs ResultArg = CMSHandler.SaveOrUpdate(item, StudentSQL.GetStudentSQL(StudentSQLCommads.SaveStuCourseInfo), sAcademicYear);
                                    if (ResultArg.Success)
                                    {
                                        sOption += item.ROLL_NO + "-" + Common.Messages.RecordsSavedSuccessfully + "\n";
                                    }
                                    else
                                    {
                                        sOption += item.ROLL_NO + "-" + Common.Messages.FailedToSaveRecords + "\n";
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    sOption = "Failed to Save,Invalid Input(s)";
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("UpdateStuCourseInfo", "Student", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("UpdateStuCourseInfo", "Student", ex.Message);
                }
            }
            return sOption;
        }
        public string DeleteStuCourseInfo(string sStuCourseInfoId)
        {
            string sOption = string.Empty;
            Stu_Course_Info objModel = new Stu_Course_Info();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "";
            objModel.COURSE_INFO_ID = sStuCourseInfoId;
            ResultArgs ResultArg = CMSHandler.SaveOrUpdate(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.DeleteStuCourseInfo), sAcademicYear);
            if (ResultArg.Success)
            {
                sOption = Common.Messages.RecordDeletedSuccessfully;
            }
            else
            {
                sOption = Common.Messages.FailedToDeletedTryAgain;
            }
            return sOption;
        }
        #endregion

        #region Assignment
        public ActionResult AssignmentSubmission(string sCourseId, string sAssigmentId, string sAssignmentTitle, string sCourseCode)
        {
            ASSIGNMENT_SUBMISSION objModel = new ASSIGNMENT_SUBMISSION();
            StudentViewModel objViewModel = new StudentViewModel();
            if (Session[Common.SESSION_VARIABLES.USER_ID] != null)
            {
                objViewModel.AssignmentSubmission.ASSIGNMENT_ID = sAssigmentId;
                objViewModel.AssignmentSubmission.COURSE_ID = sCourseId;
                objViewModel.AssignmentSubmission.COURSE_CODE = sCourseCode;
                objViewModel.AssignmentSubmission.ASSIGNMENT_NAME = sAssignmentTitle;
            }
            else
                return RedirectToAction("ErrorMessage", "error");
            return View(objViewModel);
        }
        public JsonResult SaveAssignmentSubmission(string Assignment, string extension, string base64String)
        {
            JsonResultData objResult = new JsonResultData();
            ASSIGNMENT_SUBMISSION objModel = new ASSIGNMENT_SUBMISSION();
            try
            {
                if (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null)
                {
                    string _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STORE_PATH + "I MA HIS \\";
                    if (!Directory.Exists(_sDirectorypath))
                    {
                        Directory.CreateDirectory(_sDirectorypath);
                    }
                    _sDirectorypath = _sDirectorypath.Remove(0, 3);
                    var File = Request.Files[0];
                    var AssignmentId = Request.Form[0];
                    var AssignmentName = Request.Form[1];
                    string filePath = Common.FilePath.SERVER_PATH + "\\" + "I MA HIS \\" + Session[Common.SESSION_VARIABLES.USER_CODE].ToString() + AssignmentName + "." + File.FileName.Split('.')[1];
                    File.SaveAs(filePath);
                    filePath = filePath.Remove(0, 3);
                    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                    //var Result = JsonConvert.DeserializeObject<ASSIGNMENT_SUBMISSION>(Assignment);
                    objModel.SUBMITTED_DATE = DateTime.Today.ToString("yyyy-MM-dd");
                    objModel.STUDENT_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
                    objModel.CLASS_ID = Session[Common.SESSION_VARIABLES.CLASS_ID].ToString();
                    objModel.ASSIGNMENT_ID = AssignmentId;
                    objModel.FILE_PATH = filePath;
                    var FetchAssignmentSubmission = (List<ASSIGNMENT_SUBMISSION>)CMSHandler.FetchData<ASSIGNMENT_SUBMISSION>(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.FetchAssignmentSubmissionByAssignmentIdAndStudentId), sAcademicYear).DataSource.Data;
                    if (FetchAssignmentSubmission != null && FetchAssignmentSubmission.Count > 0)
                    {
                        objModel.ASS_SUBMISSION_ID = FetchAssignmentSubmission.SingleOrDefault().ASS_SUBMISSION_ID;
                        var SaveAssignmentSubmission = CMSHandler.SaveOrUpdate(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.UpdateAssignmentSubmission), sAcademicYear);
                        if (SaveAssignmentSubmission.Success)
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        else
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                    }
                    else
                    {
                        var SaveAssignmentSubmission = CMSHandler.SaveOrUpdate(objModel, StudentSQL.GetStudentSQL(StudentSQLCommads.SaveAssignmentSubmission), sAcademicYear);
                        if (SaveAssignmentSubmission.Success)
                            objResult.Message = Common.Messages.RecordsSavedSuccessfully;
                        else
                            objResult.Message = Common.Messages.FailedToSaveRecords;
                    }
                }
                else
                {
                    objResult.ErrorCode = Common.ErrorCode.RequestTimeout;
                    objResult.Message = Common.ErrorMessage.RequestTimeout;
                }
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "SaveAssignment", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "SaveAssignment", ex.Message);
                }

            }
            return Json(objResult);
        }
        private string SaveFile(string fileName, string sExtension, string basefile)
        {
            string sfilePath = string.Empty;
            try
            {
                //this is to rename baseimgae64 formart.
                string sToReplaceBaseFormat = string.Format("data:file/{0};base64,", (sExtension == "doc") ? "doc" : sExtension);
                var strBaseFile64 = string.Empty;
                string filePath = string.Empty;
                //actual replacement 
                strBaseFile64 = basefile.Replace(sToReplaceBaseFormat, string.Empty);

                string _sDirectorypath = string.Empty;
                // bool _bSuccess = true;
                _sDirectorypath = AppDomain.CurrentDomain.BaseDirectory + "\\" + Common.URLPages.STORE_PATH;

                if (!Directory.Exists(_sDirectorypath))
                {
                    Directory.CreateDirectory(_sDirectorypath);
                }
                //Convert Base64 Encoded string to Byte Array.
                byte[] fileBytes = Convert.FromBase64String(strBaseFile64);
                if (!string.IsNullOrEmpty(fileName))
                {
                    filePath = _sDirectorypath + fileName + "." + sExtension;
                    sfilePath = "\\" + Common.URLPages.STORE_PATH + fileName + "." + sExtension;
                }
                //Save the Byte Array as Image File.and replace file if exists
                System.IO.File.WriteAllBytes(filePath, fileBytes);
            }
            catch (Exception ex)
            {
                using (ErrorLog objHandler = new ErrorLog())
                {
                    objHandler.WriteError("TaskManagement", "SaveFile", "Err MSg: " + ex.Message, "Query is empty!");
                    objHandler.WriteError("TaskManagement", "SaveFile", ex.Message);
                }

            }
            return sfilePath;
        }
        #endregion
    }
}