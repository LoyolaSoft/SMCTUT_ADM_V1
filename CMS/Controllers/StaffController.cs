using CMS.DAO;
using CMS.Models;
using CMS.SQL.StaffSQL;
using CMS.SQL.SupportData;
using CMS.Utility;
using CMS.ViewModel.Model;
using CMS.ViewModel.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class StaffController : Controller
    {
        ResultArgs resultArgs = new ResultArgs();
        DataValue dv = new DataValue();
        string sInsertQuery = string.Empty;
        string sResult = string.Empty;
        string sUpdateQuery = string.Empty;

        public ActionResult ViewDataTable()
        {
            return View();
        }

        #region Navigation Tabs
        // GET: Staff

        public ActionResult StaffInfo()
        {
            return View();
        }

        // Edit Staff Details ...

        public ActionResult EditStaffInfomation(string Id)
        {
            Session[Common.SESSION_VARIABLES.STAFF_ID] = Id;
            return View();
        }
        #endregion

        #region Staff personal Infomation
        // Staff Personal Information ...

        public ActionResult StaffPersoanlInfo()
        {
            StaffPersonalInfo objStaffPersonalInfo = new StaffPersonalInfo();
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            // Department ...
            DataTable dtFetchDepartment = new DataTable();
            dtFetchDepartment = SupportDataMethod.FetchDepartment(sAcademicYear).DataSource.Table;
            List<SupDepartment> liDepartment = new List<SupDepartment>();
            if (dtFetchDepartment != null && dtFetchDepartment.Rows.Count > 0)
            {
                liDepartment = (from DataRow dr in dtFetchDepartment.Rows
                                select new SupDepartment()
                                {
                                    DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                    DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                }).ToList();
                objStaffPersonalInfo.Department = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
            }

            // Community ...

            DataTable dtFetchCommunity = new DataTable();
            dtFetchCommunity = SupportDataMethod.FetchCommunity().DataSource.Table;
            List<SupCommunity> liCommunity = new List<SupCommunity>();
            if (dtFetchCommunity != null && dtFetchCommunity.Rows.Count > 0)
            {
                liCommunity = (from DataRow dr in dtFetchCommunity.Rows
                               select new SupCommunity()
                               {
                                   COMMUNITYID = dr[Common.SUP_COMMUNITY.COMMUNITYID].ToString(),
                                   COMMUNITY = dr[Common.SUP_COMMUNITY.COMMUNITY].ToString()
                               }).ToList();
                objStaffPersonalInfo.Community = new SelectList(liCommunity, Common.SUP_COMMUNITY.COMMUNITYID, Common.SUP_COMMUNITY.COMMUNITY);
            }

            // Gender ...

            DataTable dtFetchGender = new DataTable();
            dtFetchGender = SupportDataMethod.FetchGender().DataSource.Table;
            List<SupGender> liGender = new List<SupGender>();
            if (dtFetchGender != null && dtFetchGender.Rows.Count > 0)
            {
                liGender = (from DataRow dr in dtFetchGender.Rows
                            select new SupGender()
                            {
                                GENDER_ID = dr[Common.SUP_GENDER.GENDER_ID].ToString(),
                                GENDER_NAME = dr[Common.SUP_GENDER.GENDER_NAME].ToString()
                            }).ToList();
                objStaffPersonalInfo.Gender = new SelectList(liGender, Common.SUP_GENDER.GENDER_ID, Common.SUP_GENDER.GENDER_NAME);

            }

            // MaritalStatus ...
            DataTable dtFetchMaritalStatus = new DataTable();
            dtFetchMaritalStatus = SupportDataMethod.FetchMaritalStatus().DataSource.Table;
            List<SupMaritalStatus> liMaritalStatus = new List<SupMaritalStatus>();
            if (dtFetchMaritalStatus != null && dtFetchMaritalStatus.Rows.Count > 0)
            {
                liMaritalStatus = (from DataRow dr in dtFetchMaritalStatus.Rows
                                   select new SupMaritalStatus()
                                   {
                                       MARITAL_STAUS_ID = dr[Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID].ToString(),
                                       MARITAL_STATUS_NAME = dr[Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME].ToString()
                                   }).ToList();
                objStaffPersonalInfo.Maritalstatus = new SelectList(liMaritalStatus, Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID, Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME);
            }

            // MotherTongue ...
            DataTable dtFetchMotherTongue = new DataTable();
            dtFetchMotherTongue = SupportDataMethod.FetchMotherTongue().DataSource.Table;
            List<SupMotherTongue> liMotherTongue = new List<SupMotherTongue>();
            if (dtFetchMotherTongue != null && dtFetchMotherTongue.Rows.Count > 0)
            {
                liMotherTongue = (from DataRow dr in dtFetchMotherTongue.Rows
                                  select new SupMotherTongue()
                                  {
                                      MOTHER_TONGUE_ID = dr[Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_ID].ToString(),
                                      MOTHER_TONGUE_NAME = dr[Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_NAME].ToString()
                                  }).ToList();
                objStaffPersonalInfo.MotherTongue = new SelectList(liMotherTongue, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_ID, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_NAME);
            }

            // Nationality ...
            DataTable dtFetchNationality = new DataTable();
            dtFetchNationality = SupportDataMethod.FetchNationality().DataSource.Table;
            List<SupNationality> liNationality = new List<SupNationality>();
            if (dtFetchNationality != null && dtFetchNationality.Rows.Count > 0)
            {
                liNationality = (from DataRow dr in dtFetchNationality.Rows
                                 select new SupNationality()
                                 {
                                     NATIONALITY_ID = dr[Common.SUP_NATIONALITY.NATIONALITY_ID].ToString(),
                                     NATIONALITY_NAME = dr[Common.SUP_NATIONALITY.NATIONALITY_NAME].ToString()
                                 }).ToList();
                objStaffPersonalInfo.Nationality = new SelectList(liNationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);
            }

            // Religion ...
            DataTable dtFetchReligion = new DataTable();
            dtFetchReligion = SupportDataMethod.FetchReligion().DataSource.Table;
            List<SUP_RELIGION> liReligion = new List<SUP_RELIGION>();
            if (dtFetchReligion != null && dtFetchReligion.Rows.Count > 0)
            {
                liReligion = (from DataRow dr in dtFetchReligion.Rows
                              select new SUP_RELIGION()
                              {
                                  RELIGIONID = dr[Common.SUP_RELIGION.RELIGIONID].ToString(),
                                  RELIGION = dr[Common.SUP_RELIGION.RELIGION].ToString()
                              }).ToList();
                objStaffPersonalInfo.Religion = new SelectList(liReligion, Common.SUP_RELIGION.RELIGIONID, Common.SUP_RELIGION.RELIGION);
            }

            // BloodGroup ...
            DataTable dtFetchBloodGroup = new DataTable();
            dtFetchBloodGroup = SupportDataMethod.FetchBloodGroup().DataSource.Table;
            List<SupBloodGroup> liBloodGroup = new List<SupBloodGroup>();
            if (dtFetchBloodGroup != null && dtFetchBloodGroup.Rows.Count > 0)
            {
                liBloodGroup = (from DataRow dr in dtFetchBloodGroup.Rows
                                select new SupBloodGroup()
                                {
                                    BLOOD_GROUP_ID = dr[Common.SUP_BLOOD_GROUP.BLOOD_GROUP_ID].ToString(),
                                    BLOOD_GROUP_NAME = dr[Common.SUP_BLOOD_GROUP.BLOOD_GROUP_NAME].ToString()
                                }).ToList();
                objStaffPersonalInfo.BloodGroup = new SelectList(liBloodGroup, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_ID, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_NAME);
            }

            // Sup_Role ...
            var liRole = new List<SUP_ROLE>();
            liRole = (List<SUP_ROLE>)CMSHandler.FetchData<SUP_ROLE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchSupRole)).DataSource.Data;
            objStaffPersonalInfo.Role = new SelectList(liRole, Common.SUP_ROLE.ROLE_ID, Common.SUP_ROLE.ROLE_NAME);

            // UserType ...
            var liUserType = new List<SUP_USER_TYPE>();
            liUserType = (List<SUP_USER_TYPE>)CMSHandler.FetchData<SUP_USER_TYPE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserType)).DataSource.Data;
            objStaffPersonalInfo.UserType = new SelectList(liUserType, Common.SUP_USER_TYPE.USER_TYPE_ID, Common.SUP_USER_TYPE.USER_TYPE_NAME);
            return View(objStaffPersonalInfo);
        }

        //Insert Staff Personal ...
        public string StaffPersonal(string str)
        {

            if (str != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffPersonal>(str);
                string DateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.DateofBirth);
                string DateOfJoin = CommonMethods.ConvertdatetoyearFromat(Result.DateOfJoin);
                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.FIRST_NAME, Result.FirstName, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.LAST_NAME, Result.Lastname, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.KNOWN_AS, Result.NickName, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.GENDER, Result.Gender, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.PLACE_OF_BIRTH, Result.PlaceOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.COMMUNITY, Result.Community, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.NATIONALITY, Result.Nationality, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.MARITAL_STATUS, Result.Maritalstatus, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DATE_OF_JOIN, DateOfJoin, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DATE_OF_BIRTH, DateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.BLOOD_GROUP, Result.BloodGroup, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.RELIGION, Result.Religion, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.MOTHER_TONGUE, Result.MotherTongue, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DEPARTMENT, Result.Department, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_CODE, Result.StaffCode, EnumCommand.DataType.String);

                using (StaffViewModel objStaffView = new StaffViewModel())
                {
                    DataTable dtIfExits = new DataTable();
                    dtIfExits = objStaffView.PersonalIfExits(dv).DataSource.Table;
                    if (dtIfExits != null && dtIfExits.Rows.Count > 0)
                    {
                        sResult = "Staff Already Exit Try Again ...!";
                        if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                        {
                            string staffId = dtIfExits.Rows[0][Common.STF_PERSONAL_INFO.STAFF_ID].ToString();
                            string staffcode = dtIfExits.Rows[0][Common.STF_PERSONAL_INFO.STAFF_CODE].ToString();
                            if (staffId == Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() && staffcode == Result.StaffCode)
                            {
                                dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, staffId, EnumCommand.DataType.String);
                                resultArgs = objStaffView.UpdateStaffPersonalInfo(dv);
                                sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.Messages.FailedToSaveRecords;
                            }
                        }
                    }
                    else
                    {
                        resultArgs = objStaffView.InsertStaffPersonal(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] = resultArgs.RowUniqueId.ToString();
                            var objAccountInfo = new USER_ACCOUNT_INFO();
                            var liSuPAccountDetails = new List<SUP_USER>();
                            objAccountInfo.NAME = Result.FirstName;
                            objAccountInfo.USERNAME = Result.StaffCode.ToUpper();
                            objAccountInfo.EMAIL = "";
                            objAccountInfo.MOBILE = "";
                            objAccountInfo.USER_ID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
                            objAccountInfo.PASSWORD = CommonMethods.GetSha256FromString(objAccountInfo.USERNAME + "@" + DateOfBirth.Substring(0, 4));

                            objAccountInfo.ROLE = Result.Role;
                            objAccountInfo.USER_TYPE = Result.UserType;
                            resultArgs = CMSHandler.SaveOrUpdate(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.SaveUserAccount), "", true);
                            //objAccountInfo.USER_ID = sStaffID;
                            sResult = (resultArgs.Success) ? Common.ErrorCode.Ok : Common.Messages.FailedToSaveRecords;

                        }
                        else
                        {
                            sResult = Common.Messages.FailedToSaveRecords;
                        }
                    }

                }
            }
            return sResult;
        }

        // Fetch Staff PersonalInfo
        public JsonResult FetchPersonalInfo()
        {
            string sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchRecordbyId = new DataTable();
            var ObjUserAccount = new USER_ACCOUNT_INFO();
            StaffViewModel objStaff = new StaffViewModel();
            dtFetchRecordbyId = objStaff.FetchRecordById(dv).DataSource.Table;
            if (dtFetchRecordbyId != null && dtFetchRecordbyId.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.STAFF_ID].ToString();
                StaffPersonal objstaffInfo = new StaffPersonal()
                {
                    FirstName = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.FIRST_NAME].ToString(),
                    Lastname = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.LAST_NAME].ToString(),
                    NickName = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.KNOWN_AS].ToString(),
                    DateofBirth = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.DATE_OF_BIRTH].ToString(),
                    PlaceOfBirth = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.PLACE_OF_BIRTH].ToString(),
                    DateOfJoin = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.DATE_OF_JOIN].ToString(),
                    Maritalstatus = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.MARITAL_STATUS].ToString(),
                    Gender = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.GENDER].ToString(),
                    Department = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.DEPARTMENT].ToString(),
                    Community = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.COMMUNITY].ToString(),
                    Nationality = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.NATIONALITY].ToString(),
                    Religion = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.RELIGION].ToString(),
                    MotherTongue = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.MOTHER_TONGUE].ToString(),
                    BloodGroup = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.BLOOD_GROUP].ToString(),
                    StaffCode = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.STAFF_CODE].ToString(),
                };
                ObjUserAccount.USERNAME = dtFetchRecordbyId.Rows[0][Common.STF_PERSONAL_INFO.STAFF_CODE].ToString();
                var liUserAccount = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(ObjUserAccount, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchUserAccountByStaffCode)).DataSource.Data;
                if (liUserAccount != null && liUserAccount.Count > 0)
                {
                    objstaffInfo.Role = liUserAccount.FirstOrDefault().ROLE;
                    objstaffInfo.UserType = liUserAccount.FirstOrDefault().USER_TYPE;
                }
                return Json(objstaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Staff Personal Information ...
        public ActionResult EditStaffPersoanlInfo()
        {
            string sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchRecordbyId = new DataTable();
            StaffViewModel objStaff = new StaffViewModel();
            dtFetchRecordbyId = objStaff.FetchRecordById(dv).DataSource.Table;

            StaffPersonalInfo objStaffPersonalInfo = new StaffPersonalInfo();


            // Department ...
            DataTable dtFetchDepartment = new DataTable();
            dtFetchDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            List<SupDepartment> liDepartment = new List<SupDepartment>();
            if (dtFetchDepartment != null && dtFetchDepartment.Rows.Count > 0)
            {
                liDepartment = (from DataRow dr in dtFetchDepartment.Rows
                                select new SupDepartment()
                                {
                                    DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                    DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                }).ToList();
                objStaffPersonalInfo.Department = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
            }

            // Community ...

            DataTable dtFetchCommunity = new DataTable();
            dtFetchCommunity = SupportDataMethod.FetchCommunity().DataSource.Table;
            List<SupCommunity> liCommunity = new List<SupCommunity>();
            if (dtFetchCommunity != null && dtFetchCommunity.Rows.Count > 0)
            {
                liCommunity = (from DataRow dr in dtFetchCommunity.Rows
                               select new SupCommunity()
                               {
                                   COMMUNITYID = dr[Common.SUP_COMMUNITY.COMMUNITYID].ToString(),
                                   COMMUNITY = dr[Common.SUP_COMMUNITY.COMMUNITY].ToString()
                               }).ToList();
                objStaffPersonalInfo.Community = new SelectList(liCommunity, Common.SUP_COMMUNITY.COMMUNITYID, Common.SUP_COMMUNITY.COMMUNITY);
            }

            // Gender ...

            DataTable dtFetchGender = new DataTable();
            dtFetchGender = SupportDataMethod.FetchGender().DataSource.Table;
            List<SupGender> liGender = new List<SupGender>();
            if (dtFetchGender != null && dtFetchGender.Rows.Count > 0)
            {
                liGender = (from DataRow dr in dtFetchGender.Rows
                            select new SupGender()
                            {
                                GENDER_ID = dr[Common.SUP_GENDER.GENDER_ID].ToString(),
                                GENDER_NAME = dr[Common.SUP_GENDER.GENDER_NAME].ToString()
                            }).ToList();
                objStaffPersonalInfo.Gender = new SelectList(liGender, Common.SUP_GENDER.GENDER_ID, Common.SUP_GENDER.GENDER_NAME);

            }

            // MaritalStatus ...
            DataTable dtFetchMaritalStatus = new DataTable();
            dtFetchMaritalStatus = SupportDataMethod.FetchMaritalStatus().DataSource.Table;
            List<SupMaritalStatus> liMaritalStatus = new List<SupMaritalStatus>();
            if (dtFetchMaritalStatus != null && dtFetchMaritalStatus.Rows.Count > 0)
            {
                liMaritalStatus = (from DataRow dr in dtFetchMaritalStatus.Rows
                                   select new SupMaritalStatus()
                                   {
                                       MARITAL_STAUS_ID = dr[Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID].ToString(),
                                       MARITAL_STATUS_NAME = dr[Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME].ToString()
                                   }).ToList();
                objStaffPersonalInfo.Maritalstatus = new SelectList(liMaritalStatus, Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID, Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME);
            }

            // MotherTongue ...
            DataTable dtFetchMotherTongue = new DataTable();
            dtFetchMotherTongue = SupportDataMethod.FetchMotherTongue().DataSource.Table;
            List<SupMotherTongue> liMotherTongue = new List<SupMotherTongue>();
            if (dtFetchMotherTongue != null && dtFetchMotherTongue.Rows.Count > 0)
            {
                liMotherTongue = (from DataRow dr in dtFetchMotherTongue.Rows
                                  select new SupMotherTongue()
                                  {
                                      MOTHER_TONGUE_ID = dr[Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_ID].ToString(),
                                      MOTHER_TONGUE_NAME = dr[Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_NAME].ToString()
                                  }).ToList();
                objStaffPersonalInfo.MotherTongue = new SelectList(liMotherTongue, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_ID, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_NAME);
            }

            // Nationality ...
            DataTable dtFetchNationality = new DataTable();
            dtFetchNationality = SupportDataMethod.FetchNationality().DataSource.Table;
            List<SupNationality> liNationality = new List<SupNationality>();
            if (dtFetchNationality != null && dtFetchNationality.Rows.Count > 0)
            {
                liNationality = (from DataRow dr in dtFetchNationality.Rows
                                 select new SupNationality()
                                 {
                                     NATIONALITY_ID = dr[Common.SUP_NATIONALITY.NATIONALITY_ID].ToString(),
                                     NATIONALITY_NAME = dr[Common.SUP_NATIONALITY.NATIONALITY_NAME].ToString()
                                 }).ToList();
                objStaffPersonalInfo.Nationality = new SelectList(liNationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);
            }

            // Religion ...
            DataTable dtFetchReligion = new DataTable();
            dtFetchReligion = SupportDataMethod.FetchReligion().DataSource.Table;
            List<SUP_RELIGION> liReligion = new List<SUP_RELIGION>();
            if (dtFetchReligion != null && dtFetchReligion.Rows.Count > 0)
            {
                liReligion = (from DataRow dr in dtFetchReligion.Rows
                              select new SUP_RELIGION()
                              {
                                  RELIGIONID = dr[Common.SUP_RELIGION.RELIGIONID].ToString(),
                                  RELIGION = dr[Common.SUP_RELIGION.RELIGION].ToString()
                              }).ToList();
                objStaffPersonalInfo.Religion = new SelectList(liReligion, Common.SUP_RELIGION.RELIGIONID, Common.SUP_RELIGION.RELIGION);
            }

            // BloodGroup ...
            DataTable dtFetchBloodGroup = new DataTable();
            dtFetchBloodGroup = SupportDataMethod.FetchBloodGroup().DataSource.Table;
            List<SupBloodGroup> liBloodGroup = new List<SupBloodGroup>();
            if (dtFetchBloodGroup != null && dtFetchBloodGroup.Rows.Count > 0)
            {
                liBloodGroup = (from DataRow dr in dtFetchBloodGroup.Rows
                                select new SupBloodGroup()
                                {
                                    BLOOD_GROUP_ID = dr[Common.SUP_BLOOD_GROUP.BLOOD_GROUP_ID].ToString(),
                                    BLOOD_GROUP_NAME = dr[Common.SUP_BLOOD_GROUP.BLOOD_GROUP_NAME].ToString()
                                }).ToList();
                objStaffPersonalInfo.BloodGroup = new SelectList(liBloodGroup, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_ID, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_NAME);
            }
            // Sup_Role ...
            var liRole = new List<SUP_ROLE>();
            liRole = (List<SUP_ROLE>)CMSHandler.FetchData<SUP_ROLE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchSupRole)).DataSource.Data;
            objStaffPersonalInfo.Role = new SelectList(liRole, Common.SUP_ROLE.ROLE_ID, Common.SUP_ROLE.ROLE_NAME);

            // UserType ...
            var liUserType = new List<SUP_USER_TYPE>();
            liUserType = (List<SUP_USER_TYPE>)CMSHandler.FetchData<SUP_USER_TYPE>(null, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserType)).DataSource.Data;
            objStaffPersonalInfo.UserType = new SelectList(liUserType, Common.SUP_USER_TYPE.USER_TYPE_ID, Common.SUP_USER_TYPE.USER_TYPE_NAME);
            return View(objStaffPersonalInfo);
        }

        // Update Personal ...
        public string UpdatePersonal(string str)
        {
            string sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            if (str != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffModel>(str);
                string DateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.DateofBirth);
                string DateOfJoin = CommonMethods.ConvertdatetoyearFromat(Result.DateOfJoin);
                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.FIRST_NAME, Result.FirstName, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.LAST_NAME, Result.Lastname, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_CODE, Result.StaffCode, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.KNOWN_AS, Result.NickName, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.GENDER, Result.Gender, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.PLACE_OF_BIRTH, Result.PlaceOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.COMMUNITY, Result.Community, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.NATIONALITY, Result.Nationality, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.MARITAL_STATUS, Result.Maritalstatus, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DATE_OF_JOIN, DateOfJoin, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DATE_OF_BIRTH, DateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.BLOOD_GROUP, Result.BloodGroup, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.RELIGION, Result.Religion, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.MOTHER_TONGUE, Result.MotherTongue, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DEPARTMENT, Result.Department, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.UpdateStaffPersonalInfo(dv);
                        if (resultArgs.Success)
                        {
                            var objAccountInfo = new USER_ACCOUNT_INFO();
                            var liAccountInfo = new List<USER_ACCOUNT_INFO>();
                            var liSuPAccountDetails = new List<SUP_USER>();

                            objAccountInfo.NAME = Result.FirstName;
                            objAccountInfo.USERNAME = Result.StaffCode.ToUpper();
                            objAccountInfo.PASSWORD = CommonMethods.GetSha256FromString(Result.StaffCode.ToUpper() + "@" + DateOfBirth.Substring(0, 4));
                            objAccountInfo.USER_ID = sStaffID;
                            objAccountInfo.ROLE = Result.Role;
                            objAccountInfo.USER_TYPE = Result.UserType;

                            liAccountInfo = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserAccountByRoleAndId)).DataSource.Data;
                            if (liAccountInfo != null && liAccountInfo.Count > 0)
                            {
                                objAccountInfo.USER_ACCOUNT_ID = liAccountInfo.FirstOrDefault().USER_ACCOUNT_ID;
                                objAccountInfo.EMAIL = liAccountInfo.FirstOrDefault().EMAIL;
                                objAccountInfo.MOBILE = liAccountInfo.FirstOrDefault().MOBILE;

                                resultArgs = CMSHandler.SaveOrUpdate(objAccountInfo, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.UpdateUserAccount));

                                sResult = (resultArgs.Success) ? "Record updated successfully ...!" : "Record not updated try again ...!";
                            }
                            else
                            {
                                sResult = "Please verify all the Details are filled correctly  ...!";
                            }

                        }
                        else
                        {
                            sResult = "Record not updated try again ...!";
                        }

                    }
                }
                else
                {
                    using (StaffViewModel objStaffView = new StaffViewModel())
                    {
                        resultArgs = objStaffView.InsertStaffPersonal(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record saved successfully ...!";
                        }
                        else
                        {
                            sResult = "Record not saved try again ...!";
                        }
                    }
                }

            }
            return sResult;
        }

        #endregion

        #region Staff Addrress Infomation
        // Staff Address Information ...

        public ActionResult StaffAddressInfo()
        {
            StaffAddressInfo objStaffAddressInfo = new StaffAddressInfo();
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
                objStaffAddressInfo.CCOUNTRY = new SelectList(liCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);
                objStaffAddressInfo.PCOUNTRY = new SelectList(liCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);
            }
            return View(objStaffAddressInfo);
        }

        //Insert Address ...
        public string StaffAddress(string str)
        {
            var sStaffId = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (sStaffId != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffAddress>(str);
                dv.Clear();
                dv.Add(Common.STF_ADDRESS.STAFFNO, sStaffId, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CDOOR_DETAILS, Result.CDOORDETAILS, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CSTREET, Result.CSTREET, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CPLACE, Result.CPLACE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CCITY, Result.CCITY, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CPIN_CODE, Result.CPINCODE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CDISTRICT, Result.CDISTRICT, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CCOUNTRY, Result.CCOUNTRY, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CVILLAGE, Result.CVILLAGE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CPHONE_NO, Result.CPHONENO, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CCELL_NO, Result.CCELLNO, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CEMAIL, Result.CEMAIL, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PSTREET, Result.PSTREET, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PVILLAGE, Result.PVILLAGE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PPLACE, Result.PPLACE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PCITY, Result.PCITY, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PPIN_CODE, Result.PPINCODE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PDISTRICT, Result.PDISTRICT, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PCOUNTRY, Result.PCOUNTRY, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PPHONE_NO, Result.PPHONENO, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PCELL_NO, Result.PCELLNO, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PDOOR_DETAILS, Result.PDOORDETAILS, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PEMAIL, Result.PEMAIL, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    // Fetch Address Details ......
                    DataTable dtFetchAddressData = new DataTable();
                    dtFetchAddressData = objStaff.FetchAddressById(dv).DataSource.Table;
                    // Update Or Insert Staff Address Table ...
                    if (dtFetchAddressData != null && dtFetchAddressData.Rows.Count > 0)
                    {
                        string AddressId = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.ADDRESS_NO].ToString();
                        dv.Add(Common.STF_ADDRESS.ADDRESS_NO, AddressId, EnumCommand.DataType.String);
                        resultArgs = objStaff.UpdateStaffAddress(dv);
                    }
                    else
                        resultArgs = objStaff.InsertStaffAddress(dv);

                    // Check The Statement Exicuted Successfully ....
                    if (resultArgs.Success)
                        sResult = "Record saved successfully ...!";
                    else
                        sResult = "Record not saved successfully ...!";
                }
            }

            return sResult;
        }

        // Fetch Staff AddressData...
        public JsonResult FetchAddressData(string sStaffId)
        {
            DataTable dtFetchAddressData = new DataTable();
            dv.Clear();
            dv.Add(Common.STF_ADDRESS.STAFFNO, sStaffId, EnumCommand.DataType.String);
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchAddressData = objStaff.FetchAddressById(dv).DataSource.Table;
            }
            if (dtFetchAddressData != null && dtFetchAddressData.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.ADDRESS_NO].ToString();
                StaffAddress objStaffInfo = new StaffAddress()
                {
                    CDOORDETAILS = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CDOOR_DETAILS].ToString(),
                    CSTREET = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CSTREET].ToString(),
                    CPLACE = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CPLACE].ToString(),
                    CCITY = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CCITY].ToString(),
                    CPINCODE = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CPIN_CODE].ToString(),
                    CDISTRICT = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CDISTRICT].ToString(),
                    CVILLAGE = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CVILLAGE].ToString(),
                    CPHONENO = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CPHONE_NO].ToString(),
                    CCELLNO = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CCELL_NO].ToString(),
                    CEMAIL = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CEMAIL].ToString(),
                    PDOORDETAILS = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PDOOR_DETAILS].ToString(),
                    PSTREET = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PSTREET].ToString(),
                    PPLACE = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PPLACE].ToString(),
                    PCITY = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PCITY].ToString(),
                    PPINCODE = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PPIN_CODE].ToString(),
                    PDISTRICT = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PDISTRICT].ToString(),
                    PVILLAGE = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PVILLAGE].ToString(),
                    PPHONENO = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PPHONE_NO].ToString(),
                    PCELLNO = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PCELL_NO].ToString(),
                    PEMAIL = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PEMAIL].ToString(),
                    CCOUNTRY = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.CCOUNTRY].ToString(),
                    PCOUNTRY = dtFetchAddressData.Rows[0][Common.STF_ADDRESS.PCOUNTRY].ToString()
                };

                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Staff Address Information...

        public ActionResult EditStaffAddressInfo()
        {
            StaffAddressInfo objStaffAddressInfo = new StaffAddressInfo();
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
                objStaffAddressInfo.CCOUNTRY = new SelectList(liCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);
                objStaffAddressInfo.PCOUNTRY = new SelectList(liCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);
            }
            return View(objStaffAddressInfo);
        }

        // Update Staff Address ..
        public string UpdateAddress(string str)
        {
            var sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStaffID))
            {
                var Result = JsonConvert.DeserializeObject<StaffAddress>(str);
                dv.Clear();
                dv.Add(Common.STF_ADDRESS.CDOOR_DETAILS, Result.CDOORDETAILS, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CSTREET, Result.CSTREET, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CPLACE, Result.CPLACE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CCITY, Result.CCITY, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CPIN_CODE, Result.CPINCODE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CDISTRICT, Result.CDISTRICT, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CCOUNTRY, Result.CCOUNTRY, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CVILLAGE, Result.CVILLAGE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CPHONE_NO, Result.CPHONENO, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CCELL_NO, Result.CCELLNO, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.CEMAIL, Result.CEMAIL, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PSTREET, Result.PSTREET, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PVILLAGE, Result.PVILLAGE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PPLACE, Result.PPLACE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PCITY, Result.PCITY, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PPIN_CODE, Result.PPINCODE, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PDISTRICT, Result.PDISTRICT, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PCOUNTRY, Result.PCOUNTRY, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PPHONE_NO, Result.PPHONENO, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PCELL_NO, Result.PCELLNO, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PDOOR_DETAILS, Result.PDOORDETAILS, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.PEMAIL, Result.PEMAIL, EnumCommand.DataType.String);
                dv.Add(Common.STF_ADDRESS.STAFFNO, sStaffID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        var sAddressNo = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                        dv.Add(Common.STF_ADDRESS.ADDRESS_NO, sAddressNo, EnumCommand.DataType.String);

                        resultArgs = objStaff.UpdateStaffAddress(dv);
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
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.InsertStaffAddress(dv);
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
            return sResult;
        }
        #endregion

        #region Staff Service Infomation
        // Staff Service Information ...

        public ActionResult StaffServiceInfo()
        {
            string sStaffID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_SERVICES.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchService = new DataTable();
            IList<StaffService> liService = new List<StaffService>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchService = objViewModel.FetchServiceById(dv).DataSource.Table;
            }
            if (dtFetchService != null && dtFetchService.Rows.Count > 0)
            {
                liService = (from DataRow dr in dtFetchService.Rows
                             select new StaffService
                             {
                                 SERVICEID = dr[Common.STF_SERVICES.SERVICE_NO].ToString(),
                                 APPOINTMENTNATURE = dr[Common.STF_SERVICES.APPOINTMENT_NATURE].ToString(),
                                 DATEOFAPPOINT = dr[Common.STF_SERVICES.DATE_OF_APPOINT].ToString(),
                                 DATEOFTERMINATION = dr[Common.STF_SERVICES.DATE_OF_TERMINATION].ToString(),
                                 INSTITUTE = dr[Common.STF_SERVICES.INSTITUTE].ToString(),
                                 NAME = dr[Common.STF_SERVICES.APPOINTMENT_NAME].ToString(),
                                 Place = dr[Common.STF_SERVICES.PLACE].ToString(),
                                 REMARKS = dr[Common.STF_SERVICES.REMARKS].ToString()
                             }).ToList();
                return View(liService);
            }
            else
            {
                return View();
            }

        }

        //Insert Service ...
        public string StaffService(string JsonService)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            else
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStaffID))
            {
                var Result = JsonConvert.DeserializeObject<StaffService>(JsonService);
                string DateOfAppoint = CommonMethods.ConvertdatetoyearFromat(Result.DATEOFAPPOINT);
                string DateOfTermination = CommonMethods.ConvertdatetoyearFromat(Result.DATEOFTERMINATION);
                dv.Clear();
                dv.Add(Common.STF_SERVICES.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.DATE_OF_APPOINT, DateOfAppoint, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.APPOINTMENT_NAME, Result.NAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.DATE_OF_TERMINATION, DateOfTermination, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.INSTITUTE, Result.INSTITUTE, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.REMARKS, Result.REMARKS, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.PLACE, Result.Place, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    // Insert Staff Service Table ...
                    resultArgs = objStaff.InsertStaffservices(dv);

                    // Check the Statement is success or not ...
                    if (resultArgs.Success)
                        sResult = "Record saved successfully ...!";
                    else
                        sResult = "Record not saved successfully ...!";
                }
            }

            return sResult;
        }

        // Fetch Staff SeviceData...
        public JsonResult FetchSeviceData(string sStaffId)
        {
            dv.Clear();
            dv.Add(Common.STF_SERVICES.STAFF_NO, sStaffId, EnumCommand.DataType.String);
            DataTable dtFetchServiceData = new DataTable();
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchServiceData = objStaff.FetchServiceById(dv).DataSource.Table;
            }
            if (dtFetchServiceData != null && dtFetchServiceData.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchServiceData.Rows[0][Common.STF_SERVICES.SERVICE_NO].ToString();
                return Json(sResult);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Bind Service Infomation ...
        public JsonResult BindService(string sServiceID)
        {
            dv.Clear();
            dv.Add(Common.STF_SERVICES.SERVICE_NO, sServiceID, EnumCommand.DataType.String);
            DataTable dtFetchServiceData = new DataTable();
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchServiceData = objStaff.BindService(dv).DataSource.Table;
            }
            if (dtFetchServiceData != null && dtFetchServiceData.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchServiceData.Rows[0][Common.STF_SERVICES.SERVICE_NO].ToString();
                StaffService objStaffInfo = new StaffService()
                {
                    DATEOFAPPOINT = dtFetchServiceData.Rows[0][Common.STF_SERVICES.DATE_OF_APPOINT].ToString(),
                    NAME = dtFetchServiceData.Rows[0][Common.STF_SERVICES.APPOINTMENT_NAME].ToString(),
                    APPOINTMENTNATURE = dtFetchServiceData.Rows[0][Common.STF_SERVICES.APPOINTMENT_NATURE].ToString(),
                    DATEOFTERMINATION = dtFetchServiceData.Rows[0][Common.STF_SERVICES.DATE_OF_TERMINATION].ToString(),
                    INSTITUTE = dtFetchServiceData.Rows[0][Common.STF_SERVICES.INSTITUTE].ToString(),
                    REMARKS = dtFetchServiceData.Rows[0][Common.STF_SERVICES.REMARKS].ToString(),
                    Place = dtFetchServiceData.Rows[0][Common.STF_SERVICES.PLACE].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Staff Service Information ...

        public ActionResult EditStaffServiceInfo()
        {
            string sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStaffID))
            {
                dv.Clear();
                dv.Add(Common.STF_SERVICES.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                DataTable dtFetchService = new DataTable();
                IList<StaffService> liService = new List<StaffService>();
                using (StaffViewModel objViewModel = new StaffViewModel())
                {
                    dtFetchService = objViewModel.FetchServiceById(dv).DataSource.Table;
                }
                if (dtFetchService != null && dtFetchService.Rows.Count > 0)
                {
                    liService = (from DataRow dr in dtFetchService.Rows
                                 select new StaffService
                                 {
                                     SERVICEID = dr[Common.STF_SERVICES.SERVICE_NO].ToString(),
                                     APPOINTMENTNATURE = dr[Common.STF_SERVICES.APPOINTMENT_NATURE].ToString(),
                                     DATEOFAPPOINT = dr[Common.STF_SERVICES.DATE_OF_APPOINT].ToString(),
                                     DATEOFTERMINATION = dr[Common.STF_SERVICES.DATE_OF_TERMINATION].ToString(),
                                     INSTITUTE = dr[Common.STF_SERVICES.INSTITUTE].ToString(),
                                     NAME = dr[Common.STF_SERVICES.APPOINTMENT_NAME].ToString(),
                                     Place = dr[Common.STF_SERVICES.PLACE].ToString(),
                                     REMARKS = dr[Common.STF_SERVICES.REMARKS].ToString()
                                 }).ToList();
                    return View(liService);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                sResult = "Sesstion Has Expired Please Login Again ...!";
                return Json(sResult);
            }

        }

        // Update Staff Service ..
        public string UpdateStaffServices(string JsonService)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }

            if (JsonService != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffService>(JsonService);
                string DateofAppoint = CommonMethods.ConvertdatetoyearFromat(Result.DATEOFAPPOINT);
                string DateofTermination = CommonMethods.ConvertdatetoyearFromat(Result.DATEOFTERMINATION);
                dv.Clear();
                dv.Add(Common.STF_SERVICES.DATE_OF_APPOINT, DateofAppoint, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.APPOINTMENT_NAME, Result.NAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.DATE_OF_TERMINATION, DateofTermination, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.INSTITUTE, Result.INSTITUTE, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.REMARKS, Result.REMARKS, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.PLACE, Result.Place, EnumCommand.DataType.String);
                dv.Add(Common.STF_SERVICES.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        var sServiceID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                        dv.Add(Common.STF_SERVICES.SERVICE_NO, sServiceID, EnumCommand.DataType.String);

                        resultArgs = objStaff.UpdateStaffServices(dv);
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
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.InsertStaffservices(dv);
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
            return sResult;
        }

        // Delete Staff Service ...
        public string DeleteServices(string sServiceId)
        {
            dv.Clear();
            dv.Add(Common.STF_SERVICES.SERVICE_NO, sServiceId, EnumCommand.DataType.String);
            dv.Add(Common.STF_SERVICES.ISDELETED, "1", EnumCommand.DataType.String);
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                resultArgs = objViewModel.DeleteServics(dv);
                if (resultArgs.Success)
                {
                    sResult = "Record deleted successfully ...!";
                }
                else
                {
                    sResult = "Record not deleted try again...!";
                }
            }
            return sResult;
        }
        #endregion

        #region Staff Qualification Infomation
        // Staff Qualification Information ...

        public ActionResult StaffQualificationInfo()
        {
            string sStaffID = string.Empty;
            sStaffID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_QUALIFICATION.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchService = new DataTable();
            IList<StaffQualification> liQualification = new List<StaffQualification>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchService = objViewModel.FetchQualificationById(dv).DataSource.Table;
            }
            if (dtFetchService != null && dtFetchService.Rows.Count > 0)
            {
                liQualification = (from DataRow dr in dtFetchService.Rows
                                   select new StaffQualification
                                   {
                                       QualificationId = dr[Common.STF_QUALIFICATION.QUALIFICATION_NO].ToString(),
                                       AilledSubject = dr[Common.STF_QUALIFICATION.ALLIED_SUBJECT].ToString(),
                                       CompletionMonth = dr[Common.STF_QUALIFICATION.COMPLETION_MONTH].ToString(),
                                       CompletionYear = dr[Common.STF_QUALIFICATION.COMPLETION_YEAR].ToString(),
                                       Degree = dr[Common.STF_QUALIFICATION.DEGREE].ToString(),
                                       DegreeType = dr[Common.STF_QUALIFICATION.DEGREE_TYPE].ToString(),
                                       InstituteOfStudy = dr[Common.STF_QUALIFICATION.INSTITUTE_OF_STUDY].ToString(),
                                       MainSubject = dr[Common.STF_QUALIFICATION.MAIN_SUBJECT].ToString(),
                                       Percentange = dr[Common.STF_QUALIFICATION.PERCENTAGE].ToString(),
                                       University = dr[Common.STF_QUALIFICATION.UNIVERSITY].ToString()
                                   }).ToList();
                return View(liQualification);
            }
            else
            {
                return View();
            }

        }

        //Inssert Staff Qualification ...
        public string Qualification(string JsonQualification)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            else
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStaffID))
            {
                var Result = JsonConvert.DeserializeObject<StaffQualification>(JsonQualification);
                string CompletionYear = CommonMethods.ConvertdatetoyearFromat(Result.CompletionYear);
                //string CompletionMonth = CommonMethods.ConvertdatetoyearFromat(Result.Completion);
                dv.Clear();
                dv.Add(Common.STF_QUALIFICATION.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.MAIN_SUBJECT, Result.MainSubject, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.ALLIED_SUBJECT, Result.AilledSubject, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.COMPLETION_YEAR, CompletionYear, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.COMPLETION_MONTH, Result.Completion, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.INSTITUTE_OF_STUDY, Result.InstituteOfStudy, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.DEGREE, Result.Degree, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.UNIVERSITY, Result.University, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.PERCENTAGE, Result.Percentange, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.InsertStaffQualification(dv);
                    sResult = (resultArgs.Success) ? "Record saved successfully ...!" : "Record not saved successfully ...!";
                }
            }
            else
                sResult = "Session Has Expired Try Again ...!";

            return sResult;
        }

        // Fetch Staff QualificationData...
        public JsonResult FetchQualification(string sSatffID)
        {
            dv.Clear();
            dv.Add(Common.STF_QUALIFICATION.STAFF_NO, sSatffID, EnumCommand.DataType.String);
            DataTable dtFetchQualification = new DataTable();
            using (StaffViewModel objStaffViewModel = new StaffViewModel())
            {
                dtFetchQualification = objStaffViewModel.FetchQualificationById(dv).DataSource.Table;
            }
            if (dtFetchQualification != null && dtFetchQualification.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.QUALIFICATION_NO].ToString();
                StaffQualification objStaffInfo = new StaffQualification()
                {
                    QualificationId = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.QUALIFICATION_NO].ToString(),
                    MainSubject = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.MAIN_SUBJECT].ToString(),
                    AilledSubject = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.ALLIED_SUBJECT].ToString(),
                    CompletionMonth = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.COMPLETION_MONTH].ToString(),
                    CompletionYear = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.COMPLETION_YEAR].ToString(),
                    InstituteOfStudy = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.INSTITUTE_OF_STUDY].ToString(),
                    University = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.UNIVERSITY].ToString(),
                    Percentange = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.PERCENTAGE].ToString(),
                    Degree = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.DEGREE].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Bind Qualification ...
        public JsonResult BindQualification(string sQualification)
        {
            dv.Clear();
            dv.Add(Common.STF_QUALIFICATION.QUALIFICATION_NO, sQualification, EnumCommand.DataType.String);
            DataTable dtFetchQualification = new DataTable();
            using (StaffViewModel objStaffViewModel = new StaffViewModel())
            {
                dtFetchQualification = objStaffViewModel.BindQualification(dv).DataSource.Table;
            }
            if (dtFetchQualification != null && dtFetchQualification.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.QUALIFICATION_NO].ToString();
                StaffQualification objStaffInfo = new StaffQualification()
                {
                    MainSubject = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.MAIN_SUBJECT].ToString(),
                    AilledSubject = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.ALLIED_SUBJECT].ToString(),
                    CompletionMonth = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.COMPLETION_MONTH].ToString(),
                    CompletionYear = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.COMPLETION_YEAR].ToString(),
                    InstituteOfStudy = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.INSTITUTE_OF_STUDY].ToString(),
                    University = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.UNIVERSITY].ToString(),
                    Percentange = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.PERCENTAGE].ToString(),
                    Degree = dtFetchQualification.Rows[0][Common.STF_QUALIFICATION.DEGREE].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Fetch DropDown Qualification ...
        public string DDLQualification()
        {
            string sOption = string.Empty;
            // Degree ...
            DataTable dtFetchDegree = new DataTable();
            dtFetchDegree = SupportDataMethod.FetchDegree().DataSource.Table;
            if (dtFetchDegree != null && dtFetchDegree.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchDegree.Rows)
                {
                    sOption += "<option value='" + item[Common.SUP_DEGREE.DEGREE_ID].ToString() + "'>" + item[Common.SUP_DEGREE.DEGREE].ToString() + "</option>";
                }
            }
            return sOption;
        }

        // Staff Qualification Information ...

        public ActionResult EditStaffQualificationInfo()
        {
            string sStaffID = string.Empty;
            sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_QUALIFICATION.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchService = new DataTable();
            IList<StaffQualification> liQualification = new List<StaffQualification>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchService = objViewModel.FetchQualificationById(dv).DataSource.Table;
            }
            if (dtFetchService != null && dtFetchService.Rows.Count > 0)
            {
                liQualification = (from DataRow dr in dtFetchService.Rows
                                   select new StaffQualification
                                   {
                                       QualificationId = dr[Common.STF_QUALIFICATION.QUALIFICATION_NO].ToString(),
                                       AilledSubject = dr[Common.STF_QUALIFICATION.ALLIED_SUBJECT].ToString(),
                                       CompletionMonth = dr[Common.STF_QUALIFICATION.COMPLETION_MONTH].ToString(),
                                       CompletionYear = dr[Common.STF_QUALIFICATION.COMPLETION_YEAR].ToString(),
                                       Degree = dr[Common.STF_QUALIFICATION.DEGREE].ToString(),
                                       DegreeType = dr[Common.STF_QUALIFICATION.DEGREE_TYPE].ToString(),
                                       InstituteOfStudy = dr[Common.STF_QUALIFICATION.INSTITUTE_OF_STUDY].ToString(),
                                       MainSubject = dr[Common.STF_QUALIFICATION.MAIN_SUBJECT].ToString(),
                                       Percentange = dr[Common.STF_QUALIFICATION.PERCENTAGE].ToString(),
                                       University = dr[Common.STF_QUALIFICATION.UNIVERSITY].ToString()
                                   }).ToList();
                return View(liQualification);
            }
            else
            {
                return View();
            }

        }

        // UPdate Qualification ...
        public string UpdateStaffQualification(string JsonQualification)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            else
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(sStaffID))
            {
                var Result = JsonConvert.DeserializeObject<StaffQualification>(JsonQualification);
                string CompletionYear = CommonMethods.ConvertdatetoyearFromat(Result.CompletionYear);
                //string CompletionMonth = CommonMethods.ConvertdatetoyearFromat(Result.Completion);
                dv.Clear();
                dv.Add(Common.STF_QUALIFICATION.MAIN_SUBJECT, Result.MainSubject, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.ALLIED_SUBJECT, Result.AilledSubject, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.COMPLETION_YEAR, CompletionYear, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.COMPLETION_MONTH, Result.Completion, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.INSTITUTE_OF_STUDY, Result.InstituteOfStudy, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.DEGREE, Result.Degree, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.UNIVERSITY, Result.University, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.PERCENTAGE, Result.Percentange, EnumCommand.DataType.String);
                dv.Add(Common.STF_QUALIFICATION.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    var sQualificationNo = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STF_QUALIFICATION.QUALIFICATION_NO, sQualificationNo, EnumCommand.DataType.String);
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.UpdateStaffQualification(dv);
                        sResult = (resultArgs.Success) ? "Record saved successfully ...!" : "Record not saved try again ...!";
                    }
                }
                else
                {
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.InsertStaffQualification(dv);
                        if (resultArgs.Success)
                        {
                            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = resultArgs.RowUniqueId.ToString();
                            sResult = "Record updated successfully ...!";
                        }
                        else
                            sResult = "Record not updated try again ...!";
                    }
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return sResult;
        }

        // Delete Staff Qualification ...
        public string DeleteQualification(string sQualificationId)
        {
            dv.Clear();
            dv.Add(Common.STF_QUALIFICATION.QUALIFICATION_NO, sQualificationId, EnumCommand.DataType.String);
            dv.Add(Common.STF_QUALIFICATION.ISDELETED, "1", EnumCommand.DataType.String);
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                resultArgs = objViewModel.DeleteQualification(dv);
                sResult = (resultArgs.Success) ? "Record deleted successfully ...!" : "Record not deleted try again...!";
            }
            return sResult;
        }
        #endregion

        #region Staff paper Infomation
        // Staff Paper Information ...

        public ActionResult StaffPaperInfo()
        {
            string sStaffID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_PAPER.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchPaperInfo = new DataTable();
            IList<StaffPaper> liPaper = new List<StaffPaper>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchPaperInfo = objViewModel.FetchPaperById(dv).DataSource.Table;
            }
            if (dtFetchPaperInfo != null && dtFetchPaperInfo.Rows.Count > 0)
            {
                liPaper = (from DataRow dr in dtFetchPaperInfo.Rows
                           select new StaffPaper
                           {
                               PAPERID = dr[Common.STF_PAPER.PAPER_ID].ToString(),
                               JOURNALNAME = dr[Common.STF_PAPER.JOURNAL_NAME].ToString(),
                               LEVEL = dr[Common.sup_LEVEL.LEVEL].ToString(),
                               NOOFPAGES = dr[Common.STF_PAPER.NO_OF_PAGES].ToString(),
                               PAGESFROM = dr[Common.STF_PAPER.PAGES_FROM].ToString(),
                               PAGESTO = dr[Common.STF_PAPER.PAGES_TO].ToString(),
                               PAPERNAME = dr[Common.STF_PAPER.PAPER_NAME].ToString(),
                               PUBLISHINGCATEGORY = dr[Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY].ToString(),
                               VOLUME = dr[Common.Sup_VOLUME.VOLUME].ToString(),
                               YEARPUBLISHED = dr[Common.STF_PAPER.YEAR_PUBLISHED].ToString()
                           }).ToList();
                return View(liPaper);
            }
            else
            {
                return View();
            }

        }

        //Insert Staff Paper ..
        public string Paper(string JsonPaperInfo)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStaffID))
            {
                var Result = JsonConvert.DeserializeObject<StaffPaper>(JsonPaperInfo);
                string YearPublished = CommonMethods.ConvertdatetoyearFromat(Result.YEARPUBLISHED);
                dv.Clear();
                dv.Add(Common.STF_PAPER.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.PAPER_NAME, Result.PAPERNAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.LEVEL, Result.LEVEL, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.PUBLISHING_CATEGORY, Result.PUBLISHINGCATEGORY, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.JOURNAL_NAME, Result.JOURNALNAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.NO_OF_PAGES, Result.NOOFPAGES, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.PAGES_FROM, Result.PAGESFROM, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.PAGES_TO, Result.PAGESTO, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.VOLUME, Result.VOLUME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.YEAR_PUBLISHED, YearPublished, EnumCommand.DataType.String);

                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.InsertStaffPaper(dv);
                    sResult = (resultArgs.Success) ? "Record saved successfully ...!" : "Record not saved try again ...!";
                }
            }
            else
                sResult = "Session Has Expired Login And Try Again ...!";
            return sResult;
        }

        // Fetch Staff PaperData..
        public JsonResult FetchpaperData(string sStaffId)
        {
            if (sStaffId == null)
            {
                sStaffId = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            }
            dv.Clear();
            dv.Add(Common.STF_PAPER.STAFF_NO, sStaffId, EnumCommand.DataType.String);
            DataTable dtFetchPaperData = new DataTable();
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchPaperData = objStaff.FetchPaperById(dv).DataSource.Table;
            }
            if (dtFetchPaperData != null && dtFetchPaperData.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchPaperData.Rows[0][Common.STF_PAPER.PAPER_ID].ToString();
                StaffPaper objStaffInfo = new StaffPaper()
                {
                    PAPERID = dtFetchPaperData.Rows[0][Common.STF_PAPER.PAPER_ID].ToString(),
                    PAPERNAME = dtFetchPaperData.Rows[0][Common.STF_PAPER.PAPER_NAME].ToString(),
                    LEVEL = dtFetchPaperData.Rows[0][Common.STF_PAPER.LEVEL].ToString(),
                    JOURNALNAME = dtFetchPaperData.Rows[0][Common.STF_PAPER.JOURNAL_NAME].ToString(),
                    NOOFPAGES = dtFetchPaperData.Rows[0][Common.STF_PAPER.NO_OF_PAGES].ToString(),
                    PAGESTO = dtFetchPaperData.Rows[0][Common.STF_PAPER.PAGES_TO].ToString(),
                    PAGESFROM = dtFetchPaperData.Rows[0][Common.STF_PAPER.PAGES_FROM].ToString(),
                    PUBLISHINGCATEGORY = dtFetchPaperData.Rows[0][Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY].ToString(),
                    VOLUME = dtFetchPaperData.Rows[0][Common.STF_PAPER.VOLUME].ToString(),
                    YEARPUBLISHED = dtFetchPaperData.Rows[0][Common.STF_PAPER.YEAR_PUBLISHED].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }

        }

        //Bind Values To Controlers ..
        public JsonResult BindControles(string sPaperID)
        {
            dv.Clear();
            dv.Add(Common.STF_PAPER.PAPER_ID, sPaperID, EnumCommand.DataType.String);
            DataTable dtFetchpaper = new DataTable();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchpaper = objViewModel.BindPaperInfo(dv).DataSource.Table;
            }
            if (dtFetchpaper != null && dtFetchpaper.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchpaper.Rows[0][Common.STF_PAPER.PAPER_ID].ToString();
                StaffPaper objPaper = new StaffPaper()
                {
                    JOURNALNAME = dtFetchpaper.Rows[0][Common.STF_PAPER.JOURNAL_NAME].ToString(),
                    LEVEL = dtFetchpaper.Rows[0][Common.STF_PAPER.LEVEL].ToString(),
                    NOOFPAGES = dtFetchpaper.Rows[0][Common.STF_PAPER.NO_OF_PAGES].ToString(),
                    PAGESFROM = dtFetchpaper.Rows[0][Common.STF_PAPER.PAGES_FROM].ToString(),
                    PAGESTO = dtFetchpaper.Rows[0][Common.STF_PAPER.PAGES_TO].ToString(),
                    PAPERNAME = dtFetchpaper.Rows[0][Common.STF_PAPER.PAPER_NAME].ToString(),
                    PUBLISHINGCATEGORY = dtFetchpaper.Rows[0][Common.STF_PAPER.PUBLISHING_CATEGORY].ToString(),
                    VOLUME = dtFetchpaper.Rows[0][Common.STF_PAPER.VOLUME].ToString(),
                    YEARPUBLISHED = dtFetchpaper.Rows[0][Common.STF_PAPER.YEAR_PUBLISHED].ToString()
                };
                return Json(objPaper);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Staff Paper Information ...

        public ActionResult EditStaffPaperInfo()
        {
            string sStaffID = string.Empty;
            sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_PAPER.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchPaperInfo = new DataTable();
            IList<StaffPaper> liPaper = new List<StaffPaper>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchPaperInfo = objViewModel.FetchPaperById(dv).DataSource.Table;
            }
            if (dtFetchPaperInfo != null && dtFetchPaperInfo.Rows.Count > 0)
            {
                liPaper = (from DataRow dr in dtFetchPaperInfo.Rows
                           select new StaffPaper
                           {
                               PAPERID = dr[Common.STF_PAPER.PAPER_ID].ToString(),
                               JOURNALNAME = dr[Common.STF_PAPER.JOURNAL_NAME].ToString(),
                               LEVEL = dr[Common.sup_LEVEL.LEVEL].ToString(),
                               NOOFPAGES = dr[Common.STF_PAPER.NO_OF_PAGES].ToString(),
                               PAGESFROM = dr[Common.STF_PAPER.PAGES_FROM].ToString(),
                               PAGESTO = dr[Common.STF_PAPER.PAGES_TO].ToString(),
                               PAPERNAME = dr[Common.STF_PAPER.PAPER_NAME].ToString(),
                               PUBLISHINGCATEGORY = dr[Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY].ToString(),
                               VOLUME = dr[Common.Sup_VOLUME.VOLUME].ToString(),
                               YEARPUBLISHED = dr[Common.STF_PAPER.YEAR_PUBLISHED].ToString()
                           }).ToList();
                return View(liPaper);
            }
            else
            {
                return View();
            }

        }

        // Bind DropDown List ....
        public string StaffPaperDropDown()
        {
            string sVolume = string.Empty;
            string sLevel = string.Empty;
            string sPublishCategory = string.Empty;
            string JsonData = string.Empty;

            // Volume ...
            DataTable dtFetchVolume = new DataTable();
            dtFetchVolume = SupportDataMethod.FetchVolume().DataSource.Table;
            if (dtFetchVolume != null && dtFetchVolume.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchVolume.Rows)
                {
                    sVolume += "<option value='" + item[Common.Sup_VOLUME.VOLUME_ID].ToString() + "'>" + item[Common.Sup_VOLUME.VOLUME].ToString() + "</option>";
                }
            }

            // Level ...
            DataTable dtFetchLevel = new DataTable();
            dtFetchLevel = SupportDataMethod.FetchLevel().DataSource.Table;
            if (dtFetchVolume != null && dtFetchVolume.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchLevel.Rows)
                {
                    sLevel += "<option value='" + item[Common.sup_LEVEL.LEVEL_ID].ToString() + "'>" + item[Common.sup_LEVEL.LEVEL].ToString() + "</option>";
                }
            }

            // Publish Category ...
            DataTable dtFetchPublishCategory = new DataTable();
            dtFetchPublishCategory = SupportDataMethod.FetchPublishCategory().DataSource.Table;
            if (dtFetchPublishCategory != null && dtFetchPublishCategory.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchPublishCategory.Rows)
                {
                    sPublishCategory += "<option value='" + item[Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY_ID].ToString() + "'>" + item[Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY].ToString() + "</option>";
                }
            }

            var objJsonData = new DDLForPaperInfo() { Level = sLevel, PublishCategory = sPublishCategory, Volume = sVolume };
            JsonData = JsonConvert.SerializeObject(objJsonData);

            //   JsonData = "{'Volume':'"+ sVolume + "','Level':'" + sLevel + "','PublishCategory':'" + sPublishCategory + "'}";

            return JsonData;
        }

        // Update Staff Paper ..
        public string UpdateStaffPaper(string JsonPaperInfo)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }

            if (JsonPaperInfo != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffPaper>(JsonPaperInfo);
                string YearPublished = CommonMethods.ConvertdatetoyearFromat(Result.YEARPUBLISHED);
                dv.Clear();
                dv.Add(Common.STF_PAPER.PAPER_NAME, Result.PAPERNAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.LEVEL, Result.LEVEL, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.PUBLISHING_CATEGORY, Result.PUBLISHINGCATEGORY, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.JOURNAL_NAME, Result.JOURNALNAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.NO_OF_PAGES, Result.NOOFPAGES, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.PAGES_FROM, Result.PAGESFROM, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.PAGES_TO, Result.PAGESTO, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.VOLUME, Result.VOLUME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.YEAR_PUBLISHED, YearPublished, EnumCommand.DataType.String);
                dv.Add(Common.STF_PAPER.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    var sPaperID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STF_PAPER.PAPER_ID, sPaperID, EnumCommand.DataType.String);

                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.UpdateStaffPaper(dv);
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
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.InsertStaffPaper(dv);
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
            return sResult;
        }

        // Delete Paper Record ..
        public string DeleteStaffPaper(string sPaperID)
        {
            dv.Clear();
            dv.Add(Common.STF_PAPER.PAPER_ID, sPaperID, EnumCommand.DataType.String);
            dv.Add(Common.STF_PAPER.IS_DELETED, "1", EnumCommand.DataType.String);
            using (StaffViewModel objViwModel = new StaffViewModel())
            {
                resultArgs = objViwModel.DeletePaperInfo(dv);
                if (resultArgs.Success)
                {
                    sResult = "Record deleted successfully ...!";
                }
                else
                {
                    sResult = "Record not  deleted try again...!";
                }
            }
            return sResult;
        }
        #endregion

        #region Staff Book Publish Infomation
        // Staff Publish Information ...

        public ActionResult StaffPublishInfo()
        {
            string sStaffID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_PUBLISH.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchPublish = new DataTable();
            IList<StaffPublish> liPublish = new List<StaffPublish>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchPublish = objViewModel.FetchPublishById(dv).DataSource.Table;
            }
            if (dtFetchPublish != null && dtFetchPublish.Rows.Count > 0)
            {
                liPublish = (from DataRow dr in dtFetchPublish.Rows
                             select new StaffPublish
                             {
                                 PUBLISHID = dr[Common.STF_PUBLISH.PUBLISH_ID].ToString(),
                                 BOOKNAME = dr[Common.STF_PUBLISH.BOOK_NAME].ToString(),
                                 EDITION = dr[Common.STF_PUBLISH.EDITION].ToString(),
                                 PJOURNALNAME = dr[Common.STF_PUBLISH.JOURNAL_NAME].ToString(),
                                 PLEVEL = dr[Common.STF_PUBLISH.LEVEL].ToString(),
                                 PPUBLISHINGCATEGORY = dr[Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY].ToString(),
                                 PUBLISHER = dr[Common.STF_PUBLISH.PUBLISHER].ToString(),
                                 PVOLUME = dr[Common.STF_PUBLISH.VOLUME].ToString(),
                                 PYEAR = dr[Common.STF_PUBLISH.PYEAR].ToString()
                             }).ToList();
                return View(liPublish);
            }
            else
            {
                return View();
            }

        }

        //Insert Staff Publish ..
        public string Publish(string JsonPublish)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }
            if (JsonPublish != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffPublish>(JsonPublish);
                string Year = CommonMethods.ConvertdatetoyearFromat(Result.PYEAR);
                dv.Clear();
                dv.Add(Common.STF_PUBLISH.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.BOOK_NAME, Result.BOOKNAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.JOURNAL_NAME, Result.PJOURNALNAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.PUBLISHER, Result.PUBLISHER, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.PYEAR, Year, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.EDITION, Result.EDITION, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.LEVEL, Result.PLEVEL, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.VOLUME, Result.PVOLUME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.PUBLISHING_CATEGORY, Result.PPUBLISHINGCATEGORY, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.InsertStaffPublish(dv);
                    if (resultArgs.Success)
                    {
                        sResult = "Record saved successfully ...!";
                    }
                    else
                    {
                        sResult = "Record not saved successfully ...!";
                    }
                }
            }

            return sResult;
        }

        // Fetch Staff PublishData...
        public JsonResult FetchPublishData(string sStaffID)
        {
            dv.Clear();
            dv.Add(Common.STF_PUBLISH.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchPublish = new DataTable();
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchPublish = objStaff.FetchPublishById(dv).DataSource.Table;
            }
            if (dtFetchPublish != null && dtFetchPublish.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchPublish.Rows[0][Common.STF_PUBLISH.PUBLISH_ID].ToString();
                StaffPublish objStaffInfo = new StaffPublish()
                {
                    PUBLISHID = dtFetchPublish.Rows[0][Common.STF_PUBLISH.PUBLISH_ID].ToString(),
                    BOOKNAME = dtFetchPublish.Rows[0][Common.STF_PUBLISH.BOOK_NAME].ToString(),
                    PJOURNALNAME = dtFetchPublish.Rows[0][Common.STF_PUBLISH.JOURNAL_NAME].ToString(),
                    PUBLISHER = dtFetchPublish.Rows[0][Common.STF_PUBLISH.PUBLISHER].ToString(),
                    PYEAR = dtFetchPublish.Rows[0][Common.STF_PUBLISH.PYEAR].ToString(),
                    EDITION = dtFetchPublish.Rows[0][Common.STF_PUBLISH.EDITION].ToString(),
                    PLEVEL = dtFetchPublish.Rows[0][Common.STF_PUBLISH.LEVEL].ToString(),
                    PPUBLISHINGCATEGORY = dtFetchPublish.Rows[0][Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY].ToString(),
                    PVOLUME = dtFetchPublish.Rows[0][Common.STF_PUBLISH.VOLUME].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // BindPublish Details...
        public JsonResult BindPublish(string sPublishID)
        {
            dv.Clear();
            dv.Add(Common.STF_PUBLISH.PUBLISH_ID, sPublishID, EnumCommand.DataType.String);
            DataTable dtFetchPublish = new DataTable();
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchPublish = objStaff.BindPublish(dv).DataSource.Table;
            }
            if (dtFetchPublish != null && dtFetchPublish.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchPublish.Rows[0][Common.STF_PUBLISH.PUBLISH_ID].ToString();
                StaffPublish objStaffInfo = new StaffPublish()
                {
                    BOOKNAME = dtFetchPublish.Rows[0][Common.STF_PUBLISH.BOOK_NAME].ToString(),
                    PJOURNALNAME = dtFetchPublish.Rows[0][Common.STF_PUBLISH.JOURNAL_NAME].ToString(),
                    PUBLISHER = dtFetchPublish.Rows[0][Common.STF_PUBLISH.PUBLISHER].ToString(),
                    PYEAR = dtFetchPublish.Rows[0][Common.STF_PUBLISH.PYEAR].ToString(),
                    EDITION = dtFetchPublish.Rows[0][Common.STF_PUBLISH.EDITION].ToString(),
                    PLEVEL = dtFetchPublish.Rows[0][Common.STF_PUBLISH.LEVEL].ToString(),
                    PPUBLISHINGCATEGORY = dtFetchPublish.Rows[0][Common.STF_PUBLISH.PUBLISHING_CATEGORY].ToString(),
                    PVOLUME = dtFetchPublish.Rows[0][Common.STF_PUBLISH.VOLUME].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Load DropDown List ..
        public string DDLPublish()
        {
            string sLevel = string.Empty;
            string sVolume = string.Empty;
            string sPublishCategory = string.Empty;
            string JsonData = string.Empty;

            // Volume ...
            DataTable dtFetchVolume = new DataTable();
            dtFetchVolume = SupportDataMethod.FetchVolume().DataSource.Table;
            if (dtFetchVolume != null && dtFetchVolume.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchVolume.Rows)
                {
                    sVolume += "<option value='" + item[Common.Sup_VOLUME.VOLUME_ID].ToString() + "'>" + item[Common.Sup_VOLUME.VOLUME].ToString() + "</option>";
                }
            }

            // Level ...
            DataTable dtFetchLevel = new DataTable();
            dtFetchLevel = SupportDataMethod.FetchLevel().DataSource.Table;
            if (dtFetchVolume != null && dtFetchVolume.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchLevel.Rows)
                {
                    sLevel += "<option value='" + item[Common.sup_LEVEL.LEVEL_ID].ToString() + "'>" + item[Common.sup_LEVEL.LEVEL].ToString() + "</option>";
                }
            }

            // Publish Category ...
            DataTable dtFetchPublishCategory = new DataTable();
            dtFetchPublishCategory = SupportDataMethod.FetchPublishCategory().DataSource.Table;
            if (dtFetchPublishCategory != null && dtFetchPublishCategory.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchPublishCategory.Rows)
                {
                    sPublishCategory += "<option value='" + item[Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY_ID].ToString() + "'>" + item[Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY].ToString() + "</option>";
                }
            }

            var objJsonData = new DDLForPaperInfo() { Level = sLevel, PublishCategory = sPublishCategory, Volume = sVolume };
            JsonData = JsonConvert.SerializeObject(objJsonData);

            return JsonData;
        }

        // Staff Publish Information ...

        public ActionResult EditStaffPublishInfo()
        {
            string sStaffID = string.Empty;
            sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_PUBLISH.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchPublish = new DataTable();
            IList<StaffPublish> liPublish = new List<StaffPublish>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchPublish = objViewModel.FetchPublishById(dv).DataSource.Table;
            }
            if (dtFetchPublish != null && dtFetchPublish.Rows.Count > 0)
            {
                liPublish = (from DataRow dr in dtFetchPublish.Rows
                             select new StaffPublish
                             {
                                 PUBLISHID = dr[Common.STF_PUBLISH.PUBLISH_ID].ToString(),
                                 BOOKNAME = dr[Common.STF_PUBLISH.BOOK_NAME].ToString(),
                                 EDITION = dr[Common.STF_PUBLISH.EDITION].ToString(),
                                 PJOURNALNAME = dr[Common.STF_PUBLISH.JOURNAL_NAME].ToString(),
                                 PLEVEL = dr[Common.STF_PUBLISH.LEVEL].ToString(),
                                 PPUBLISHINGCATEGORY = dr[Common.Sup_PUBLISH_CATEGORY.PUBLISH_CATEGORY].ToString(),
                                 PUBLISHER = dr[Common.STF_PUBLISH.PUBLISHER].ToString(),
                                 PVOLUME = dr[Common.STF_PUBLISH.VOLUME].ToString(),
                                 PYEAR = dr[Common.STF_PUBLISH.PYEAR].ToString()
                             }).ToList();
                return View(liPublish);
            }
            else
            {
                return View();
            }
        }

        // Update Staff Publish ..
        public string UpdateStaffPublish(string JsonPublish)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }

            if (JsonPublish != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffPublish>(JsonPublish);
                string PublishYear = CommonMethods.ConvertdatetoyearFromat(Result.PYEAR);
                dv.Clear();
                dv.Add(Common.STF_PUBLISH.BOOK_NAME, Result.BOOKNAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.JOURNAL_NAME, Result.PJOURNALNAME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.PUBLISHER, Result.PUBLISHER, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.PYEAR, PublishYear, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.EDITION, Result.EDITION, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.VOLUME, Result.PVOLUME, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.PUBLISHING_CATEGORY, Result.PPUBLISHINGCATEGORY, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.LEVEL, Result.PLEVEL, EnumCommand.DataType.String);
                dv.Add(Common.STF_PUBLISH.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        var sPublishID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                        dv.Add(Common.STF_PUBLISH.PUBLISH_ID, sPublishID, EnumCommand.DataType.String);

                        resultArgs = objStaff.UpdateStaffPublish(dv);
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
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.InsertStaffPublish(dv);
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
            return sResult;
        }

        // Delete Staff Publish ...
        public string DeletePublish(string sPublishId)
        {
            dv.Clear();
            dv.Add(Common.STF_PUBLISH.PUBLISH_ID, sPublishId, EnumCommand.DataType.String);
            dv.Add(Common.STF_PUBLISH.ISDELETED, "1", EnumCommand.DataType.String);
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                resultArgs = objViewModel.DeletePublish(dv);
                if (resultArgs.Success)
                {
                    sResult = "Record deleted successfully ...!";
                }
                else
                {
                    sResult = "Record not deleted try again...!";
                }
            }
            return sResult;
        }
        #endregion

        #region Staff Counselling Infomation
        // Staff Counselling Information ...

        public ActionResult StaffCounsellingInfo()
        {
            string sStaffID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_COUNSELING.STAFF, sStaffID, EnumCommand.DataType.String);
            DataTable dtCounselling = new DataTable();
            IList<StaffCounseling> liCounselling = new List<StaffCounseling>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtCounselling = objViewModel.FetchCounselingById(dv).DataSource.Table;
            }
            if (dtCounselling != null && dtCounselling.Rows.Count > 0)
            {
                liCounselling = (from DataRow dr in dtCounselling.Rows
                                 select new StaffCounseling
                                 {
                                     CounsellingID = dr[Common.STF_COUNSELING.STF_COUN_ID].ToString(),
                                     ACTIONTAKEN = dr[Common.STF_COUNSELING.ACTION_TAKEN].ToString(),
                                     DateOfCounsel = dr[Common.STF_COUNSELING.DATE_OF_COUN].ToString(),
                                     DURATION = dr[Common.STF_COUNSELING.DURATION].ToString(),
                                     GIVEN = dr[Common.STF_COUNSELING.GIVEN].ToString(),
                                     REASON = dr[Common.STF_COUNSELING.REASON].ToString(),
                                     REMARK = dr[Common.STF_COUNSELING.REMARK].ToString()
                                 }).ToList();
                return View(liCounselling);
            }
            else
            {
                return View();
            }
        }

        // Insert Staff Counseling ...
        public string Counseling(string JsonCounselling)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }
            if (JsonCounselling != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffCounseling>(JsonCounselling);
                string DateOfCounselling = CommonMethods.ConvertdatetoyearFromat(Result.DateOfCounsel);
                dv.Clear();
                dv.Add(Common.STF_COUNSELING.STAFF, sStaffID, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.DATE_OF_COUN, DateOfCounselling, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.DURATION, Result.DURATION, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.REASON, Result.REASON, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.GIVEN, Result.GIVEN, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.REMARK, Result.REMARK, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.ACTION_TAKEN, Result.ACTIONTAKEN, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.InsertStaffCounseling(dv);
                    if (resultArgs.Success)
                    {
                        sResult = "Record saved successfully ...!";
                    }
                    else
                    {
                        sResult = "Record not saved successfully ...!";
                    }
                }
            }

            return sResult;
        }

        // Fetch Staff CounselingData...
        public JsonResult FetchCounselingData(string sStaffID)
        {
            dv.Clear();
            dv.Add(Common.STF_COUNSELING.STAFF, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchCounseling = new DataTable();
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchCounseling = objStaff.FetchCounselingById(dv).DataSource.Table;
            }
            if (dtFetchCounseling != null && dtFetchCounseling.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchCounseling.Rows[0][Common.STF_COUNSELING.STF_COUN_ID].ToString();
                StaffCounseling objStaffInfo = new StaffCounseling()
                {
                    DateOfCounsel = dtFetchCounseling.Rows[0][Common.STF_COUNSELING.DATE_OF_COUN].ToString(),
                    REASON = dtFetchCounseling.Rows[0][Common.STF_COUNSELING.REASON].ToString(),
                    GIVEN = dtFetchCounseling.Rows[0][Common.STF_COUNSELING.GIVEN].ToString(),
                    ACTIONTAKEN = dtFetchCounseling.Rows[0][Common.STF_COUNSELING.ACTION_TAKEN].ToString(),
                    REMARK = dtFetchCounseling.Rows[0][Common.STF_COUNSELING.REMARK].ToString(),
                    DURATION = dtFetchCounseling.Rows[0][Common.STF_COUNSELING.DURATION].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Bind Values In Counselling Countrols ...
        public JsonResult BindCounselling(string sCounsellingID)
        {
            dv.Clear();
            dv.Add(Common.STF_COUNSELING.STF_COUN_ID, sCounsellingID, EnumCommand.DataType.String);
            DataTable dtFetchCounselling = new DataTable();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchCounselling = objViewModel.BindCounselling(dv).DataSource.Table;
            }
            if (dtFetchCounselling != null && dtFetchCounselling.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchCounselling.Rows[0][Common.STF_COUNSELING.STF_COUN_ID].ToString();
                StaffCounseling objCounselling = new StaffCounseling()
                {
                    ACTIONTAKEN = dtFetchCounselling.Rows[0][Common.STF_COUNSELING.ACTION_TAKEN].ToString(),
                    DateOfCounsel = dtFetchCounselling.Rows[0][Common.STF_COUNSELING.DATE_OF_COUN].ToString(),
                    DURATION = dtFetchCounselling.Rows[0][Common.STF_COUNSELING.DURATION].ToString(),
                    GIVEN = dtFetchCounselling.Rows[0][Common.STF_COUNSELING.GIVEN].ToString(),
                    REASON = dtFetchCounselling.Rows[0][Common.STF_COUNSELING.REASON].ToString(),
                    REMARK = dtFetchCounselling.Rows[0][Common.STF_COUNSELING.REMARK].ToString()
                };
                return Json(objCounselling);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }

        }

        // Staff Counselling Information ...

        public ActionResult EditStaffCounsellingInfo()
        {
            string sStaffID = string.Empty;
            sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_COUNSELING.STAFF, sStaffID, EnumCommand.DataType.String);
            DataTable dtCounselling = new DataTable();
            IList<StaffCounseling> liCounselling = new List<StaffCounseling>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtCounselling = objViewModel.FetchCounselingById(dv).DataSource.Table;
            }
            if (dtCounselling != null && dtCounselling.Rows.Count > 0)
            {
                liCounselling = (from DataRow dr in dtCounselling.Rows
                                 select new StaffCounseling
                                 {
                                     CounsellingID = dr[Common.STF_COUNSELING.STF_COUN_ID].ToString(),
                                     ACTIONTAKEN = dr[Common.STF_COUNSELING.ACTION_TAKEN].ToString(),
                                     DateOfCounsel = dr[Common.STF_COUNSELING.DATE_OF_COUN].ToString(),
                                     DURATION = dr[Common.STF_COUNSELING.DURATION].ToString(),
                                     GIVEN = dr[Common.STF_COUNSELING.GIVEN].ToString(),
                                     REASON = dr[Common.STF_COUNSELING.REASON].ToString(),
                                     REMARK = dr[Common.STF_COUNSELING.REMARK].ToString()
                                 }).ToList();
                return View(liCounselling);
            }
            else
            {
                return View();
            }

        }

        // Update Staff Counseling ...
        public string UpdateCounseling(string JsonCounselling)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }

            if (JsonCounselling != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffCounseling>(JsonCounselling);
                string sDateOfCounselling = CommonMethods.ConvertdatetoyearFromat(Result.DateOfCounsel);
                dv.Clear();
                dv.Add(Common.STF_COUNSELING.DATE_OF_COUN, sDateOfCounselling, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.DURATION, Result.DURATION, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.REASON, Result.REASON, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.GIVEN, Result.GIVEN, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.REMARK, Result.REMARK, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.ACTION_TAKEN, Result.ACTIONTAKEN, EnumCommand.DataType.String);
                dv.Add(Common.STF_COUNSELING.STAFF, sStaffID, EnumCommand.DataType.String);
                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        var sCounselingNo = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                        dv.Add(Common.STF_COUNSELING.STF_COUN_ID, sCounselingNo, EnumCommand.DataType.String);
                        resultArgs = objStaff.UpdateStaffCounseling(dv);
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
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.InsertStaffCounseling(dv);
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
            return sResult;
        }

        // Delete Staff Counselling ..
        public string DeleteCounselling(string sCounsellingId)
        {
            dv.Clear();
            dv.Add(Common.STF_COUNSELING.STF_COUN_ID, sCounsellingId, EnumCommand.DataType.String);
            dv.Add(Common.STF_COUNSELING.IS_DELETED, "1", EnumCommand.DataType.String);
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                resultArgs = objViewModel.DeleteStaffCounselling(dv);
                if (resultArgs.Success)
                {
                    sResult = "Record deleted successfully ...!";
                }
                else
                {
                    sResult = "Record not deleted try again...!";
                }
            }
            return sResult;
        }
        #endregion

        #region Staff Leaving Infomation
        // Staff Leaving Information ...

        public ActionResult StaffLeavingInfo()
        {
            StaffLeavingInfo objStaffLeavingInfo = new StaffLeavingInfo();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            // Department ...
            DataTable dtFetchDepartment = new DataTable();
            dtFetchDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            List<SupDepartment> liDepartment = new List<SupDepartment>();
            if (dtFetchDepartment != null && dtFetchDepartment.Rows.Count > 0)
            {
                liDepartment = (from DataRow dr in dtFetchDepartment.Rows
                                select new SupDepartment()
                                {
                                    DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                    DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                }).ToList();
                objStaffLeavingInfo.DEPARTMENT = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
            }
            return View(objStaffLeavingInfo);
        }

        //Insert Staff Leaving ...
        public string Leaving(string str)
        {
            var sStaffId = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sStaffId))
            {
                var Result = JsonConvert.DeserializeObject<StaffLeaving>(str);
                string LeavingDate = CommonMethods.ConvertdatetoyearFromat(Result.LeavingDate);
                string RetirementDate = CommonMethods.ConvertdatetoyearFromat(Result.RetirementDate);
                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.LEAVING_DATE, LeavingDate, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.LEAVING_REMARKS, Result.LeavingRemark, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DEPARTMENT, Result.Department, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.RETIREMENT_DATE, RetirementDate, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.LEAVING_REASON, Result.LeavingReason, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.IS_LEFT, "1", EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffId, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.UpdateStaffLeaving(dv);
                    sResult = (resultArgs.Success) ? "Record saved successfully ...!" : "Record not saved successfully ...!";
                }
            }
            else
                sResult = "Session Has Expired Please Login And Try Again ...!";
            return sResult;
        }

        // Fetch Staff Leaving ...
        public JsonResult FetchLeavingData(string sStaffID)
        {
            if (!string.IsNullOrEmpty(sStaffID))
            {
                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffID, EnumCommand.DataType.String);
                DataTable dtFetchLeaving = new DataTable();
                using (StaffViewModel objStaffViewModel = new StaffViewModel())
                {
                    dtFetchLeaving = objStaffViewModel.FetchLeavingById(dv).DataSource.Table;
                }
                if (dtFetchLeaving != null && dtFetchLeaving.Rows.Count > 0)
                {
                    StaffLeavingInfo objStaffInfo = new StaffLeavingInfo()
                    {
                        LEAVINGDATE = dtFetchLeaving.Rows[0][Common.STF_PERSONAL_INFO.LEAVING_DATE].ToString(),
                        LEAVINGREMARKS = dtFetchLeaving.Rows[0][Common.STF_PERSONAL_INFO.LEAVING_REMARKS].ToString(),
                        RETIREMENTDATE = dtFetchLeaving.Rows[0][Common.STF_PERSONAL_INFO.RETIREMENT_DATE].ToString(),
                        LEAVINGREASON = dtFetchLeaving.Rows[0][Common.STF_PERSONAL_INFO.LEAVING_REASON].ToString(),
                        //sDepartment = dtFetchLeaving.Rows[0][Common.STF_PERSONAL_INFO.DEPARTMENT].ToString()
                    };
                    return Json(objStaffInfo);
                }
                else
                {
                    Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                    return Json(sResult);
                }
            }
            else
            {
                sResult = "Bad Request ...!";
                return Json(sResult);
            }
        }

        // Staff Leaving Information ...
        //[UserRights("STAFF")]
        public ActionResult EditStaffLeavingInfo()
        {
            StaffLeavingInfo objStaffLeavingInfo = new StaffLeavingInfo();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            // Department ...
            DataTable dtFetchDepartment = new DataTable();
            dtFetchDepartment = SupportDataMethod.FetchDepartment((Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
            List<SupDepartment> liDepartment = new List<SupDepartment>();
            if (dtFetchDepartment != null && dtFetchDepartment.Rows.Count > 0)
            {
                liDepartment = (from DataRow dr in dtFetchDepartment.Rows
                                select new SupDepartment()
                                {
                                    DEPARTMENT_ID = dr[Common.SUP_DEPARTMENT.DEPARTMENT_ID].ToString(),
                                    DEPARTMENT = dr[Common.SUP_DEPARTMENT.DEPARTMENT].ToString()
                                }).ToList();
                objStaffLeavingInfo.DEPARTMENT = new SelectList(liDepartment, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);
            }
            return View(objStaffLeavingInfo);
        }

        //Update Staff Leaving ...
        public JsonResult UpdateLeaving(string str)
        {
            var sStaffId = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            if (str != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffLeaving>(str);
                string LeavingDate = CommonMethods.ConvertdatetoyearFromat(Result.LeavingDate);
                string RetirementDate = CommonMethods.ConvertdatetoyearFromat(Result.RetirementDate);
                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.LEAVING_DATE, LeavingDate, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.LEAVING_REMARKS, Result.LeavingRemark, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DEPARTMENT, Result.Department, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.RETIREMENT_DATE, RetirementDate, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.LEAVING_REASON, Result.LeavingReason, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.IS_LEFT, "1", EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffId, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.UpdateStaffLeaving(dv);
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

            return Json(sResult);
        }

        #endregion

        #region Staff Training Infomation
        // Staff Training Information ...
        public ActionResult StaffTrainingInfo()
        {
            string sStaffID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_TRAINING.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchTraining = new DataTable();
            IList<StaffTraining> liTraining = new List<StaffTraining>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchTraining = objViewModel.FetchTrainingById(dv).DataSource.Table;
            }
            if (dtFetchTraining != null && dtFetchTraining.Rows.Count > 0)
            {
                liTraining = (from DataRow dr in dtFetchTraining.Rows
                              select new StaffTraining
                              {
                                  TraininngId = dr[Common.STF_TRAINING.TRAINING_NO].ToString(),
                                  COMMENTS = dr[Common.STF_TRAINING.COMMENTS].ToString(),
                                  DateFrom = dr[Common.STF_TRAINING.DATE_FROM].ToString(),
                                  DateTo = dr[Common.STF_TRAINING.DATE_TO].ToString(),
                                  LEVEL = dr[Common.STF_TRAINING.TLEVEL].ToString(),
                                  PLACE = dr[Common.STF_TRAINING.PLACE].ToString(),
                                  PROGRAMME = dr[Common.STF_TRAINING.PROGRAMME].ToString()
                              }).ToList();
                return View(liTraining);
            }
            else
            {
                return View();
            }

        }

        //Insert Staff Training ..
        public string Training(string JsonTraining)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }
            if (JsonTraining != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffTraining>(JsonTraining);
                string DateFrom = CommonMethods.ConvertdatetoyearFromat(Result.DateFrom);
                string DateTo = CommonMethods.ConvertdatetoyearFromat(Result.DateTo);
                dv.Clear();
                dv.Add(Common.STF_TRAINING.DATE_FROM, DateFrom, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.DATE_TO, DateTo, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.PROGRAMME, Result.PROGRAMME, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.PLACE, Result.PLACE, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.TLEVEL, Result.LEVEL, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.COMMENTS, Result.COMMENTS, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.InsertStaffTraining(dv);
                    if (resultArgs.Success)
                    {
                        sResult = "Record saved successfully ...!";
                    }
                    else
                    {
                        sResult = "Record not saved successfully ...!";
                    }
                }
            }

            return sResult;
        }

        // Fetch Staff TrainingData...
        public JsonResult FetchTrainingData(string sStaffID)
        {
            dv.Clear();
            dv.Add(Common.STF_TRAINING.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchTraining = new DataTable();
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchTraining = objStaff.FetchTrainingById(dv).DataSource.Table;
            }

            if (dtFetchTraining != null && dtFetchTraining.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchTraining.Rows[0][Common.STF_TRAINING.TRAINING_NO].ToString();

                StaffTraining objStaffInfo = new StaffTraining()
                {
                    TraininngId = dtFetchTraining.Rows[0][Common.STF_TRAINING.TRAINING_NO].ToString(),
                    DateFrom = dtFetchTraining.Rows[0][Common.STF_TRAINING.DATE_FROM].ToString(),
                    DateTo = dtFetchTraining.Rows[0][Common.STF_TRAINING.DATE_TO].ToString(),
                    PROGRAMME = dtFetchTraining.Rows[0][Common.STF_TRAINING.PROGRAMME].ToString(),
                    PLACE = dtFetchTraining.Rows[0][Common.STF_TRAINING.PLACE].ToString(),
                    COMMENTS = dtFetchTraining.Rows[0][Common.STF_TRAINING.COMMENTS].ToString(),
                    LEVEL = dtFetchTraining.Rows[0][Common.STF_TRAINING.TLEVEL].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Bind Values To Controles ...
        public JsonResult BindTraining(string sTrainingID)
        {
            dv.Clear();
            dv.Add(Common.STF_TRAINING.TRAINING_NO, sTrainingID, EnumCommand.DataType.String);
            DataTable dtFetchTraining = new DataTable();
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchTraining = objStaff.BindTraining(dv).DataSource.Table;
            }

            if (dtFetchTraining != null && dtFetchTraining.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchTraining.Rows[0][Common.STF_TRAINING.TRAINING_NO].ToString();

                StaffTraining objStaffInfo = new StaffTraining()
                {
                    DateFrom = dtFetchTraining.Rows[0][Common.STF_TRAINING.DATE_FROM].ToString(),
                    DateTo = dtFetchTraining.Rows[0][Common.STF_TRAINING.DATE_TO].ToString(),
                    PROGRAMME = dtFetchTraining.Rows[0][Common.STF_TRAINING.PROGRAMME].ToString(),
                    PLACE = dtFetchTraining.Rows[0][Common.STF_TRAINING.PLACE].ToString(),
                    COMMENTS = dtFetchTraining.Rows[0][Common.STF_TRAINING.COMMENTS].ToString(),
                    LEVEL = dtFetchTraining.Rows[0][Common.STF_TRAINING.TLEVEL].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Staff Training Information ...
        public ActionResult EditStaffTrainingInfo()
        {
            string sStaffID = string.Empty;
            sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_TRAINING.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchTraining = new DataTable();
            IList<StaffTraining> liTraining = new List<StaffTraining>();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchTraining = objViewModel.FetchTrainingById(dv).DataSource.Table;
            }
            if (dtFetchTraining != null && dtFetchTraining.Rows.Count > 0)
            {
                liTraining = (from DataRow dr in dtFetchTraining.Rows
                              select new StaffTraining
                              {
                                  TraininngId = dr[Common.STF_TRAINING.TRAINING_NO].ToString(),
                                  COMMENTS = dr[Common.STF_TRAINING.COMMENTS].ToString(),
                                  DateFrom = dr[Common.STF_TRAINING.DATE_FROM].ToString(),
                                  DateTo = dr[Common.STF_TRAINING.DATE_TO].ToString(),
                                  LEVEL = dr[Common.STF_TRAINING.TLEVEL].ToString(),
                                  PLACE = dr[Common.STF_TRAINING.PLACE].ToString(),
                                  PROGRAMME = dr[Common.STF_TRAINING.PROGRAMME].ToString()
                              }).ToList();
                return View(liTraining);
            }
            else
            {
                return View();
            }

        }

        // Update Training ..
        public string UpdateStaffTraining(string JsonTraining)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }

            if (JsonTraining != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffTraining>(JsonTraining);
                string DateFrom = CommonMethods.ConvertdatetoyearFromat(Result.DateFrom);
                string DateTo = CommonMethods.ConvertdatetoyearFromat(Result.DateTo);
                dv.Clear();
                dv.Add(Common.STF_TRAINING.DATE_FROM, DateFrom, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.DATE_TO, DateTo, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.PROGRAMME, Result.PROGRAMME, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.PLACE, Result.PLACE, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.TLEVEL, Result.LEVEL, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.COMMENTS, Result.COMMENTS, EnumCommand.DataType.String);
                dv.Add(Common.STF_TRAINING.STAFF_NO, sStaffID, EnumCommand.DataType.String);

                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    var sTrainingNo = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                    dv.Add(Common.STF_TRAINING.TRAINING_NO, sTrainingNo, EnumCommand.DataType.String);
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.UpdateStaffTraining(dv);
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
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.InsertStaffTraining(dv);
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
            return sResult;
        }

        // Delete Staff Training ..
        public string DeleteTraining(string sTrainingId)
        {
            dv.Clear();
            dv.Add(Common.STF_TRAINING.TRAINING_NO, sTrainingId, EnumCommand.DataType.String);
            dv.Add(Common.STF_TRAINING.ISDELETED, "1", EnumCommand.DataType.String);
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                resultArgs = objViewModel.DeleteTraining(dv);
                if (resultArgs.Success)
                {
                    sResult = "Record deleted successfully ...!";
                }
                else
                {
                    sResult = "Record not deleted try again...!";
                }
            }
            return sResult;
        }
        #endregion

        #region Staff Roles Infomation
        // Staff Roles Information ...
        public ActionResult StaffRolesInfo()
        {
            StaffRolesInfo objStaffRolesInfo = new StaffRolesInfo();

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
                objStaffRolesInfo.SHIFT = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            }

            // Qualification ...
            DataTable dtFetchQualification = new DataTable();
            dtFetchQualification = SupportDataMethod.FetchQualification().DataSource.Table;
            List<Sup_Qualification> liQualification = new List<Sup_Qualification>();
            if (dtFetchQualification != null && dtFetchQualification.Rows.Count > 0)
            {
                liQualification = (from DataRow dr in dtFetchQualification.Rows
                                   select new Sup_Qualification()
                                   {
                                       QUALIFICATION_ID = dr[Common.Sup_Qualification.QUALIFICATION_ID].ToString(),
                                       QUALIFICATION = dr[Common.Sup_Qualification.QUALIFICATION].ToString()
                                   }).ToList();
                objStaffRolesInfo.QUALIFICATION = new SelectList(liQualification, Common.Sup_Qualification.QUALIFICATION_ID, Common.Sup_Qualification.QUALIFICATION);
            }

            // Designation ...
            DataTable dtFetchDesignation = new DataTable();
            dtFetchDesignation = SupportDataMethod.FetchDesignation().DataSource.Table;
            List<Sup_Designation> liDesignation = new List<Sup_Designation>();
            if (dtFetchDesignation != null && dtFetchDesignation.Rows.Count > 0)
            {
                liDesignation = (from DataRow dr in dtFetchDesignation.Rows
                                 select new Sup_Designation()
                                 {
                                     DESIGNATIONID = dr[Common.sup_designation.DESIGNATIONID].ToString(),
                                     DESIGNATION = dr[Common.sup_designation.DESIGNATION].ToString()
                                 }).ToList();
                objStaffRolesInfo.DESIGNATION = new SelectList(liDesignation, Common.sup_designation.DESIGNATIONID, Common.sup_designation.DESIGNATION);
            }

            // Sub_Category ...
            DataTable dtFetchSubCategory = new DataTable();
            dtFetchSubCategory = SupportDataMethod.FetchSubCategory().DataSource.Table;
            List<Sup_SubCategory> liSubCategory = new List<Sup_SubCategory>();
            if (dtFetchSubCategory != null && dtFetchSubCategory.Rows.Count > 0)
            {
                liSubCategory = (from DataRow dr in dtFetchSubCategory.Rows
                                 select new Sup_SubCategory()
                                 {
                                     STF_CATEGORY_ID = dr[Common.sup_staff_subcategory.STF_CATEGORY_ID].ToString(),
                                     STF_CATEGORY = dr[Common.sup_staff_subcategory.STF_CATEGORY].ToString()
                                 }).ToList();
                objStaffRolesInfo.SUBCATEGORY = new SelectList(liSubCategory, Common.sup_staff_subcategory.STF_CATEGORY_ID, Common.sup_staff_subcategory.STF_CATEGORY);
            }

            // Non Teaching Category ..
            DataTable dtFetchNonTeachingCategory = new DataTable();
            dtFetchNonTeachingCategory = SupportDataMethod.FetchNonTeachingCategory().DataSource.Table;
            List<SUP_NON_TEACHING_CATEGORY> liNonTeachingCategory = new List<SUP_NON_TEACHING_CATEGORY>();
            if (dtFetchNonTeachingCategory != null && dtFetchNonTeachingCategory.Rows.Count > 0)
            {
                liNonTeachingCategory = (from DataRow dr in dtFetchNonTeachingCategory.Rows
                                         select new SUP_NON_TEACHING_CATEGORY
                                         {
                                             NON_TEACHING_CATEGORY_ID = dr[Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_ID].ToString(),
                                             NON_TEACHING_CATEGORY_NAME = dr[Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_NAME].ToString()
                                         }).ToList();
                objStaffRolesInfo.NONTEACHINGCATEGORY = new SelectList(liNonTeachingCategory, Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_ID, Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_NAME);
            }
            // Board Member ..
            DataTable dtBoardMember = new DataTable();
            dtBoardMember = SupportDataMethod.FetchBoardMmber().DataSource.Table;
            List<SUP_BOARD_MEMBER> liBoardMember = new List<SUP_BOARD_MEMBER>();
            if (dtBoardMember != null && dtBoardMember.Rows.Count > 0)
            {
                liBoardMember = (from DataRow dr in dtBoardMember.Rows
                                 select new SUP_BOARD_MEMBER()
                                 {
                                     BOARD_MEMBER_ID = dr[Common.SUP_BOARD_MEMBER.BOARD_MEMBER_ID].ToString(),
                                     BOARD_MEMBER_NAME = dr[Common.SUP_BOARD_MEMBER.BOARD_MEMBER_NAME].ToString(),
                                 }).ToList();
                objStaffRolesInfo.BOARDMEMBER = new SelectList(liBoardMember, Common.SUP_BOARD_MEMBER.BOARD_MEMBER_ID, Common.SUP_BOARD_MEMBER.BOARD_MEMBER_NAME);
            }
            return View(objStaffRolesInfo);
        }

        //Insert Staff Roles Details ...
        public string SubjectDetails(string JsonRoles)
        {
            var sStaffID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            if (JsonRoles != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffRoles>(JsonRoles);

                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.BOARD_MEMBER, Result.BOARDMEMBER, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DESIGNATION, Result.DESIGNATION, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.KNOWLEDGE_IN_COMPUTER, Result.KNOWLEDGEINCOMPUTER, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.MAIN_SUBJECT, Result.MAINSUBJECT, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.NON_TEACHING_CATEGORY, Result.NONTEACHINGCATEGORY, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.QUALIFICATION, Result.QUALIFICATION, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.SESSIONS, Result.SESSIONS, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.SHIFT, Result.SHIFT, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.SUB_CATEGORY, Result.SUBCATEGORY, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffID, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_CODE, Result.StaffCode, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.UpdateStaffSubjectDetails(dv);
                    if (resultArgs.Success)
                    {
                        sResult = "Record saved successfully ...!";
                    }
                    else
                    {
                        sResult = "Record not saved successfully ...!";
                    }
                }
            }

            return sResult;
        }

        // Fetch Staff Category ..
        public string FetchStaffCategory(string sCategory)
        {
            string sOption = string.Empty;
            var ObjCategory = new CATEGORY();
            if (sCategory != "1")
            {
                ObjCategory.liNonTeachingCategory = (List<SUP_NON_TEACHING_CATEGORY>)CMSHandler.FetchData<SUP_NON_TEACHING_CATEGORY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchNonTeachingCategory)).DataSource.Data;
                foreach (var item in ObjCategory.liNonTeachingCategory)
                {
                    sOption += "<option value='" + item.NON_TEACHING_CATEGORY_ID + "'>" + item.NON_TEACHING_CATEGORY_NAME + "</option>";
                }
            }
            else
            {
                ObjCategory.liTeachingCategory = (List<SUP_TEACHING_CATEGORY>)CMSHandler.FetchData<SUP_TEACHING_CATEGORY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchTeachingCategory)).DataSource.Data;
                foreach (var item in ObjCategory.liTeachingCategory)
                {
                    sOption += "<option value='" + item.TEACHING_CATEGORY_ID + "'>" + item.TEACHING_CATEGORY + "</option>";
                }
            }
            return sOption;
        }

        // Fetch Staff Roles ..
        public JsonResult FetchSubjectDetailsData(string sStaffID)
        {
            dv.Clear();
            dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchSubjectDetails = new DataTable();
            using (StaffViewModel objStaffViewModel = new StaffViewModel())
            {
                dtFetchSubjectDetails = objStaffViewModel.FetchStaffRolesById(dv).DataSource.Table;
            }
            if (dtFetchSubjectDetails != null && dtFetchSubjectDetails.Rows.Count > 0)
            {
                //StaffInfo objStaffInfo = new StaffInfo()
                //{
                //    BOARDMEMBER = dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.BOARD_MEMBER].ToString(),
                //    StaffCode = dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.STAFF_CODE].ToString(),
                //    KNOWLEDGEINCOMPUTER = dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.KNOWLEDGE_IN_COMPUTER].ToString(),
                //    NONTEACHINGCATEGORY = dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.NON_TEACHING_CATEGORY].ToString(),
                //    SESSIONS = dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.SESSIONS].ToString(),
                //    MAINSUBJECT = dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.MAIN_SUBJECT].ToString(),
                //    sShift=dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.SHIFT].ToString(),
                //    //sNonTeachingCategory=dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.NON_TEACHING_CATEGORY].ToString(),
                //    sDesignation=dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.DESIGNATION].ToString(),
                //    sQualification=dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.QUALIFICATION].ToString(),
                //    sSubCategory=dtFetchSubjectDetails.Rows[0][Common.STF_PERSONAL_INFO.SUB_CATEGORY].ToString()
                //};
                return Json(sResult);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }

        }

        // Load Dropdown List ...
        public string LoadStaffRolesDropdown()
        {
            string sShift = string.Empty;
            string sQualification = string.Empty;
            string sDesignation = string.Empty;
            string sSubcategory = string.Empty;
            string sNonteachingCategory = string.Empty;
            string JsonData = string.Empty;
            // Non Teaching Category ..
            DataTable dtFetchNonTeachingCategory = new DataTable();
            dtFetchNonTeachingCategory = SupportDataMethod.FetchNonTeachingCategory().DataSource.Table;
            if (dtFetchNonTeachingCategory != null && dtFetchNonTeachingCategory.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchNonTeachingCategory.Rows)
                {
                    sNonteachingCategory += "<option value='" + item[Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_ID].ToString() + "'>" + item[Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_NAME].ToString() + "</option>";
                }
            }
            // Shift ...
            DataTable dtFetchShift = new DataTable();
            dtFetchShift = SupportDataMethod.FetchShift().DataSource.Table;
            if (dtFetchShift != null && dtFetchShift.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchShift.Rows)
                {
                    sShift += "<option value='" + item[Common.SUP_SHIFT.SHIFT_ID].ToString() + "'>" + item[Common.SUP_SHIFT.SHIFT_NAME].ToString() + "</option>";
                }
            }

            // Qualification ...
            DataTable dtFetchQualification = new DataTable();
            dtFetchQualification = SupportDataMethod.FetchQualification().DataSource.Table;
            if (dtFetchQualification != null && dtFetchQualification.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchQualification.Rows)
                {
                    sQualification += "<option value='" + item[Common.Sup_Qualification.QUALIFICATION_ID].ToString() + "'>" + item[Common.Sup_Qualification.QUALIFICATION].ToString() + "</option>";
                }
            }
            // Designation ...
            DataTable dtFetchDesignation = new DataTable();
            dtFetchDesignation = SupportDataMethod.FetchDesignation().DataSource.Table;
            if (dtFetchDesignation != null && dtFetchDesignation.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchDesignation.Rows)
                {
                    sDesignation += "<option value='" + item[Common.SUP_DESIGNATION.DESIGNATIONID].ToString() + "'>" + item[Common.SUP_DESIGNATION.DESIGNATION].ToString() + "</option>";
                }
            }
            // Sub_Category ...
            DataTable dtFetchSubCategory = new DataTable();
            dtFetchSubCategory = SupportDataMethod.FetchSubCategory().DataSource.Table;
            if (dtFetchSubCategory != null && dtFetchSubCategory.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchSubCategory.Rows)
                {
                    sSubcategory += "<option value='" + item[Common.sup_staff_subcategory.STF_CATEGORY_ID].ToString() + "'>" + item[Common.sup_staff_subcategory.STF_CATEGORY].ToString() + "</option>";
                }
            }
            var objJson = new DDLForStaffRolesInfo() { Qualification = sQualification, Shift = sShift, SubCategory = sSubcategory, Designation = sDesignation, NonTeachingCategory = sNonteachingCategory };
            JsonData = JsonConvert.SerializeObject(objJson);
            return JsonData;
        }

        // Staff Roles Information ...
        public ActionResult EditStaffRolesInfo()
        {
            StaffRolesInfo objStaffRolesInfo = new StaffRolesInfo();

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
                objStaffRolesInfo.SHIFT = new SelectList(liShift, Common.SUP_SHIFT.SHIFT_ID, Common.SUP_SHIFT.SHIFT_NAME);
            }

            // Qualification ...
            DataTable dtFetchQualification = new DataTable();
            dtFetchQualification = SupportDataMethod.FetchQualification().DataSource.Table;
            List<Sup_Qualification> liQualification = new List<Sup_Qualification>();
            if (dtFetchQualification != null && dtFetchQualification.Rows.Count > 0)
            {
                liQualification = (from DataRow dr in dtFetchQualification.Rows
                                   select new Sup_Qualification()
                                   {
                                       QUALIFICATION_ID = dr[Common.Sup_Qualification.QUALIFICATION_ID].ToString(),
                                       QUALIFICATION = dr[Common.Sup_Qualification.QUALIFICATION].ToString()
                                   }).ToList();
                objStaffRolesInfo.QUALIFICATION = new SelectList(liQualification, Common.Sup_Qualification.QUALIFICATION_ID, Common.Sup_Qualification.QUALIFICATION);
            }

            // Designation ...
            DataTable dtFetchDesignation = new DataTable();
            dtFetchDesignation = SupportDataMethod.FetchDesignation().DataSource.Table;
            List<Sup_Designation> liDesignation = new List<Sup_Designation>();
            if (dtFetchDesignation != null && dtFetchDesignation.Rows.Count > 0)
            {
                liDesignation = (from DataRow dr in dtFetchDesignation.Rows
                                 select new Sup_Designation()
                                 {
                                     DESIGNATIONID = dr[Common.sup_designation.DESIGNATIONID].ToString(),
                                     DESIGNATION = dr[Common.sup_designation.DESIGNATION].ToString()
                                 }).ToList();
                objStaffRolesInfo.DESIGNATION = new SelectList(liDesignation, Common.sup_designation.DESIGNATIONID, Common.sup_designation.DESIGNATION);
            }

            // Sub_Category ...
            DataTable dtFetchSubCategory = new DataTable();
            dtFetchSubCategory = SupportDataMethod.FetchSubCategory().DataSource.Table;
            List<Sup_SubCategory> liSubCategory = new List<Sup_SubCategory>();
            if (dtFetchSubCategory != null && dtFetchSubCategory.Rows.Count > 0)
            {
                liSubCategory = (from DataRow dr in dtFetchSubCategory.Rows
                                 select new Sup_SubCategory()
                                 {
                                     STF_CATEGORY_ID = dr[Common.sup_staff_subcategory.STF_CATEGORY_ID].ToString(),
                                     STF_CATEGORY = dr[Common.sup_staff_subcategory.STF_CATEGORY].ToString()
                                 }).ToList();
                objStaffRolesInfo.SUBCATEGORY = new SelectList(liSubCategory, Common.sup_staff_subcategory.STF_CATEGORY_ID, Common.sup_staff_subcategory.STF_CATEGORY);
            }
            // Non Teaching Category ..
            DataTable dtFetchNonTeachingCategory = new DataTable();
            dtFetchNonTeachingCategory = SupportDataMethod.FetchNonTeachingCategory().DataSource.Table;
            List<SUP_NON_TEACHING_CATEGORY> liNonTeachingCategory = new List<SUP_NON_TEACHING_CATEGORY>();
            if (dtFetchNonTeachingCategory != null && dtFetchNonTeachingCategory.Rows.Count > 0)
            {
                liNonTeachingCategory = (from DataRow dr in dtFetchNonTeachingCategory.Rows
                                         select new SUP_NON_TEACHING_CATEGORY
                                         {
                                             NON_TEACHING_CATEGORY_ID = dr[Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_ID].ToString(),
                                             NON_TEACHING_CATEGORY_NAME = dr[Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_NAME].ToString()
                                         }).ToList();
                objStaffRolesInfo.NONTEACHINGCATEGORY = new SelectList(liNonTeachingCategory, Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_ID, Common.SUP_NON_TEACHING_CATEGORY.NON_TEACHING_CATEGORY_NAME);
            }
            // Board Member ..
            DataTable dtBoardMember = new DataTable();
            dtBoardMember = SupportDataMethod.FetchBoardMmber().DataSource.Table;
            List<SUP_BOARD_MEMBER> liBoardMember = new List<SUP_BOARD_MEMBER>();
            if (dtBoardMember != null && dtBoardMember.Rows.Count > 0)
            {
                liBoardMember = (from DataRow dr in dtBoardMember.Rows
                                 select new SUP_BOARD_MEMBER()
                                 {
                                     BOARD_MEMBER_ID = dr[Common.SUP_BOARD_MEMBER.BOARD_MEMBER_ID].ToString(),
                                     BOARD_MEMBER_NAME = dr[Common.SUP_BOARD_MEMBER.BOARD_MEMBER_NAME].ToString(),
                                 }).ToList();
                objStaffRolesInfo.BOARDMEMBER = new SelectList(liBoardMember, Common.SUP_BOARD_MEMBER.BOARD_MEMBER_ID, Common.SUP_BOARD_MEMBER.BOARD_MEMBER_NAME);
            }
            return View(objStaffRolesInfo);

        }

        //Update Subject Details ...
        public JsonResult UpdateSubjectDetails(string str)
        {
            var sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            if (str != null)
            {
                var Result = JsonConvert.DeserializeObject<StaffRoles>(str);

                dv.Clear();
                dv.Add(Common.STF_PERSONAL_INFO.BOARD_MEMBER, Result.BOARDMEMBER, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.DESIGNATION, Result.DESIGNATION, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.KNOWLEDGE_IN_COMPUTER, Result.KNOWLEDGEINCOMPUTER, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.MAIN_SUBJECT, Result.MAINSUBJECT, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.NON_TEACHING_CATEGORY, Result.NONTEACHINGCATEGORY, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.QUALIFICATION, Result.QUALIFICATION, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.SESSIONS, Result.SESSIONS, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.SHIFT, Result.SHIFT, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.SUB_CATEGORY, Result.SUBCATEGORY, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffID, EnumCommand.DataType.String);
                dv.Add(Common.STF_PERSONAL_INFO.STAFF_CODE, Result.StaffCode, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.UpdateStaffSubjectDetails(dv);
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

            return Json(sResult);
        }
        #endregion

        #region Staff Family Infomation
        // Staff Family Information ...
        public ActionResult StaffFamilyInfo()
        {
            string sStaffID = string.Empty;
            sStaffID = (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null) ? Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString() : string.Empty;
            dv.Clear();
            dv.Add(Common.STF_FAMILY.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchFamilyById = new DataTable();

            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchFamilyById = objViewModel.FetchFamilyById(dv).DataSource.Table;
            }
            if (dtFetchFamilyById != null && dtFetchFamilyById.Rows.Count > 0)
            {
                IList<Family> liFamily = new List<Family>();
                liFamily = (from DataRow dr in dtFetchFamilyById.Rows
                            select new Family
                            {
                                FamilyId = dr[Common.STF_FAMILY.FAMILY_NO].ToString(),
                                Name = dr[Common.STF_FAMILY.NAME].ToString(),
                                Relation = dr[Common.STF_FAMILY.RELATION].ToString(),
                                DateOfBirth = dr[Common.STF_FAMILY.DATE_OF_BIRTH].ToString()
                            }).ToList();
                return View(liFamily);
            }
            else
            {
                return View();
            }
        }

        //Insert Staff Family ..
        public JsonResult AddFamily(string JsonFamily)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }

            if (JsonFamily != null)
            {
                var Result = JsonConvert.DeserializeObject<Family>(JsonFamily);
                string DateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.DateOfBirth);

                dv.Clear();
                dv.Add(Common.STF_FAMILY.NAME, Result.Name, EnumCommand.DataType.String);
                dv.Add(Common.STF_FAMILY.RELATION, Result.Relation, EnumCommand.DataType.String);
                dv.Add(Common.STF_FAMILY.DATE_OF_BIRTH, DateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STF_FAMILY.STAFF_NO, sStaffID, EnumCommand.DataType.String);
                using (StaffViewModel objStaff = new StaffViewModel())
                {
                    resultArgs = objStaff.InsertStaffFamily(dv);
                    if (resultArgs.Success)
                    {
                        sResult = "Record saved successfully ...!";
                    }
                    else
                    {
                        sResult = "Record not saved successfully ...!";
                    }
                }
            }
            return Json(sResult);
        }

        // Fetch Staff Family ...
        public JsonResult FetchFamily(string sStaffID)
        {
            dv.Clear();
            dv.Add(Common.STF_FAMILY.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchFamily = new DataTable();
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                dtFetchFamily = objStaff.FetchFamilyById(dv).DataSource.Table;
            }
            if (dtFetchFamily != null && dtFetchFamily.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchFamily.Rows[0][Common.STF_FAMILY.FAMILY_NO].ToString();
                Family objStaffInfo = new Family()
                {
                    FamilyId = dtFetchFamily.Rows[0][Common.STF_FAMILY.FAMILY_NO].ToString(),
                    Name = dtFetchFamily.Rows[0][Common.STF_FAMILY.NAME].ToString(),
                    Relation = dtFetchFamily.Rows[0][Common.STF_FAMILY.RELATION].ToString(),
                    DateOfBirth = dtFetchFamily.Rows[0][Common.STF_FAMILY.DATE_OF_BIRTH].ToString()
                };
                return Json(objStaffInfo);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }

        }

        // Bind values To Controles...
        public JsonResult BindFamily(string sFamilyID)
        {
            dv.Clear();
            dv.Add(Common.STF_FAMILY.FAMILY_NO, sFamilyID, EnumCommand.DataType.String);
            DataTable dtFetchFamily = new DataTable();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchFamily = objViewModel.BindFamily(dv).DataSource.Table;
            }
            if (dtFetchFamily != null && dtFetchFamily.Rows.Count > 0)
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = dtFetchFamily.Rows[0][Common.STF_FAMILY.FAMILY_NO].ToString();
                Family objFamily = new Family()
                {
                    DateOfBirth = dtFetchFamily.Rows[0][Common.STF_FAMILY.DATE_OF_BIRTH].ToString(),
                    Name = dtFetchFamily.Rows[0][Common.STF_FAMILY.NAME].ToString(),
                    Relation = dtFetchFamily.Rows[0][Common.STF_FAMILY.RELATION].ToString()
                };
                return Json(objFamily);
            }
            else
            {
                Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = null;
                return Json(sResult);
            }
        }

        // Bind DropDown List ..
        public string DDLFamily()
        {
            string sOption = string.Empty;
            // Relation ...
            DataTable dtFetchRelation = new DataTable();
            dtFetchRelation = SupportDataMethod.FetchRelaion().DataSource.Table;
            if (dtFetchRelation != null && dtFetchRelation.Rows.Count > 0)
            {
                foreach (DataRow item in dtFetchRelation.Rows)
                {
                    sOption += "<option value='" + item[Common.SUP_FAMILY.RELATION_ID].ToString() + "'>" + item[Common.SUP_FAMILY.RELATION].ToString() + "</option>";
                }
            }
            return sOption;
        }

        // Staff Family Information ...
        public ActionResult EditStaffFamilyInfo()
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }

            dv.Clear();
            dv.Add(Common.STF_FAMILY.STAFF_NO, sStaffID, EnumCommand.DataType.String);
            DataTable dtFetchFamilyById = new DataTable();

            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                dtFetchFamilyById = objViewModel.FetchFamilyById(dv).DataSource.Table;
            }
            if (dtFetchFamilyById != null && dtFetchFamilyById.Rows.Count > 0)
            {
                IList<Family> liFamily = new List<Family>();
                liFamily = (from DataRow dr in dtFetchFamilyById.Rows
                            select new Family
                            {
                                FamilyId = dr[Common.STF_FAMILY.FAMILY_NO].ToString(),
                                Name = dr[Common.STF_FAMILY.NAME].ToString(),
                                Relation = dr[Common.STF_FAMILY.RELATION].ToString(),
                                DateOfBirth = dr[Common.STF_FAMILY.DATE_OF_BIRTH].ToString()
                            }).ToList();
                return View(liFamily);
            }
            else
            {
                return View();
            }
        }

        // Update Family ...
        public JsonResult UpdateFamily(string JsonFamily)
        {
            string sStaffID = string.Empty;
            if (Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] != null)
            {
                sStaffID = Session[Common.SESSION_VARIABLES.LAST_INSERT_ID].ToString();
            }
            else
            {
                sStaffID = (Session[Common.SESSION_VARIABLES.STAFF_ID] != null) ? Session[Common.SESSION_VARIABLES.STAFF_ID].ToString() : string.Empty;
            }


            if (JsonFamily != null)
            {
                var Result = JsonConvert.DeserializeObject<Family>(JsonFamily);
                string DateOfBirth = CommonMethods.ConvertdatetoyearFromat(Result.DateOfBirth);
                dv.Clear();
                dv.Add(Common.STF_FAMILY.NAME, Result.Name, EnumCommand.DataType.String);
                dv.Add(Common.STF_FAMILY.RELATION, Result.Relation, EnumCommand.DataType.String);
                dv.Add(Common.STF_FAMILY.DATE_OF_BIRTH, DateOfBirth, EnumCommand.DataType.String);
                dv.Add(Common.STF_FAMILY.STAFF_NO, sStaffID, EnumCommand.DataType.String);

                if (Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] != null)
                {
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        var PrimaryID = Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID].ToString();
                        dv.Add(Common.STF_FAMILY.FAMILY_NO, PrimaryID, EnumCommand.DataType.String);
                        resultArgs = objStaff.UpdateStaffFamily(dv);
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
                    using (StaffViewModel objStaff = new StaffViewModel())
                    {
                        resultArgs = objStaff.InsertStaffFamily(dv);
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

        // Delete Staff Family ..
        public string DeleteFamily(string sFamilyId)
        {
            dv.Clear();
            dv.Add(Common.STF_FAMILY.FAMILY_NO, sFamilyId, EnumCommand.DataType.String);
            dv.Add(Common.STF_FAMILY.ISDELETED, "1", EnumCommand.DataType.String);
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                resultArgs = objViewModel.DeleteFamily(dv);
                if (resultArgs.Success)
                {
                    sResult = "Record deleted successfully ...!";
                }
                else
                {
                    sResult = "Record not deleted try again...!";
                }
            }
            return sResult;
        }
        #endregion

        #region List Staff Details
        //Staff List ...
        [UserRights("STAFF")]
        public ActionResult ListStaffDetails()
        {
            return View();
        }


        public ActionResult StaffList()
        {
            Session[Common.SESSION_VARIABLES.LAST_INSERT_ID] = null;
            DataTable dtStaffList = new DataTable();
            using (StaffViewModel objViewModel = new StaffViewModel())
            {
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                dtStaffList = objViewModel.FetchStaffInfo(sAcademicYear).DataSource.Table;
            }
            IList<StaffList> liList = new List<StaffList>();
            if (dtStaffList != null && dtStaffList.Rows.Count > 0)
            {
                liList = (from DataRow dr in dtStaffList.Rows
                          select new StaffList
                          {
                              STAFFID = dr[Common.STF_PERSONAL_INFO.STAFF_ID].ToString(),
                              STAFFCODE = dr[Common.STF_PERSONAL_INFO.STAFF_CODE].ToString(),
                              FIRSTNAME = dr[Common.STF_PERSONAL_INFO.FIRST_NAME].ToString(),
                              GENDERNAME = dr[Common.SUP_GENDER.GENDER_NAME].ToString(),
                              DEPARTMENT = dr[Common.CP_DEPARTMENT_2017.DEPARTMENT].ToString(),
                              SHIFTNAME = dr[Common.SUP_SHIFT.SHIFT_NAME].ToString()
                          }).ToList();

                return View(liList);
            }
            else
            {
                return View();
            }

        }
        #endregion

        #region Delete Staff Details 
        // Delete Staff Details ...
        public string DeleteStaff(string sStaffId, string UserName)
        {
            dv.Clear();
            dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffId, EnumCommand.DataType.String);
            dv.Add(Common.STF_PERSONAL_INFO.IS_DELETED, "1", EnumCommand.DataType.String);
            var ObjUserAccount = new USER_ACCOUNT_INFO();
            var liUserAccount = new List<USER_ACCOUNT_INFO>();
            ObjUserAccount.USERNAME = UserName;
            liUserAccount = (List<USER_ACCOUNT_INFO>)CMSHandler.FetchData<USER_ACCOUNT_INFO>(ObjUserAccount, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.FetchUserAccountByName)).DataSource.Data;
            ObjUserAccount.USER_ACCOUNT_ID = liUserAccount.FirstOrDefault().USER_ACCOUNT_ID;
            resultArgs = CMSHandler.SaveOrUpdate(ObjUserAccount, SQL.UserAccount.UserAccountSQL.GetSupportDataSQL(UserAccountSQLCommands.DeleteUserAccount));
            using (StaffViewModel objStaff = new StaffViewModel())
            {
                resultArgs = objStaff.DeleteStaffInfo(dv);
                if (resultArgs.Success)
                {
                    sResult = "Record deleted successfully ...!";
                }
                else
                {
                    sResult = "Record deleted successfully ..!";
                }
            }
            return sResult;
        }
        #endregion

        #region RDLC Report
        public ActionResult TransferCertificate()
        {
            return View();
        }
        #endregion

        #region Edit Staff Profile
        public ActionResult EditStaffProfile()
        {
            //Session[Common.SESSION_VARIABLES.USER_ID]=Id;
            return View();
        }

        #region Edit Staff Personal Profile
        //public ActionResult EditStaffPersonalProfile()
        //{
        //    //string sStaffID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
        //    dv.Clear();
        //    //dv.Add(Common.STF_PERSONAL_INFO.STAFF_ID, sStaffID, EnumCommand.DataType.String);
        //    //DataTable dtFetchRecordbyId = new DataTable();
        //    StaffViewModel objStaff = new StaffViewModel();
        //    string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();


        //    // Department ...
        //    var Department = new List<SupDepartment>();
        //    Department = (List<SupDepartment>)CMSHandler.FetchData<SupDepartment>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchDepartment), sAcademicYear).DataSource.Data;
        //    objStaff.DEPARTMENT = new SelectList(Department, Common.SUP_DEPARTMENT.DEPARTMENT_ID, Common.SUP_DEPARTMENT.DEPARTMENT);

        //    // Community ...

        //    var Community = new List<SupCommunity>();
        //    Community = (List<SupCommunity>)CMSHandler.FetchData<SupCommunity>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCommunity)).DataSource.Data;
        //    objStaff.COMMUNITY = new SelectList(Community, Common.SUP_COMMUNITY.COMMUNITYID, Common.SUP_COMMUNITY.COMMUNITY);

        //    // Gender ...

        //    var Gender = new List<SupGender>();
        //    Gender = (List<SupGender>)CMSHandler.FetchData<SupGender>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchGender)).DataSource.Data;
        //    objStaff.GENDER = new SelectList(Gender, Common.SUP_GENDER.GENDER_ID, Common.SUP_GENDER.GENDER_NAME);

        //    // MaritalStatus ...

        //    var MaritalStatus = new List<SupMaritalStatus>();
        //    MaritalStatus = (List<SupMaritalStatus>)CMSHandler.FetchData<SupMaritalStatus>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMarritalStatus)).DataSource.Data;
        //    objStaff.MARITAL_STATUS = new SelectList(MaritalStatus, Common.SUP_MARRITAL_STATUS.MARITAL_STAUS_ID, Common.SUP_MARRITAL_STATUS.MARITAL_STATUS_NAME);

        //    // MotherTongue ...

        //    var MotherTongue = new List<SupMotherTongue>();
        //    MotherTongue = (List<SupMotherTongue>)CMSHandler.FetchData<SupMotherTongue>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchMotherTongue)).DataSource.Data;
        //    objStaff.MOTHER_TONGUE = new SelectList(MotherTongue, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_ID, Common.SUP_MOTHER_TONGUE.MOTHER_TONGUE_NAME);

        //    // Nationality ...

        //    var Nationality = new List<SupNationality>();
        //    Nationality = (List<SupNationality>)CMSHandler.FetchData<SupNationality>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchNationality)).DataSource.Data;
        //    objStaff.NATIONALITY = new SelectList(Nationality, Common.SUP_NATIONALITY.NATIONALITY_ID, Common.SUP_NATIONALITY.NATIONALITY_NAME);

        //    // Religion ...

        //    var Religion = new List<SUP_RELIGION>();
        //    Religion = (List<SUP_RELIGION>)CMSHandler.FetchData<SUP_RELIGION>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchReligion)).DataSource.Data;
        //    objStaff.RELIGION = new SelectList(Religion, Common.SUP_RELIGION.RELIGIONID, Common.SUP_RELIGION.RELIGION);

        //    // BloodGroup ...

        //    var BloodGroup = new List<SupBloodGroup>();
        //    BloodGroup = (List<SupBloodGroup>)CMSHandler.FetchData<SupBloodGroup>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchBloodGroup)).DataSource.Data;
        //    objStaff.BLOOD_GROUP = new SelectList(BloodGroup, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_ID, Common.SUP_BLOOD_GROUP.BLOOD_GROUP_NAME);

        //    return View(objStaff);
        //}
        public JsonResult FetchStaffPersonalProfile()
        {
            var StaffPersonalProfile = new List<STAFF_PERSONAL>();
            var StaffPersonal = new STAFF_PERSONAL();
            StaffPersonal.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            StaffPersonalProfile = (List<STAFF_PERSONAL>)CMSHandler.FetchData<STAFF_PERSONAL>(StaffPersonal, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchPersonalRecordByID)).DataSource.Data;
            return new JsonResult { Data = StaffPersonalProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public string UpdateStaffPersonalProflie(string StaffPersonal)
        {
            var Result = JsonConvert.DeserializeObject<STAFF_PERSONAL>(StaffPersonal);
            Result.DATE_OF_BIRTH = CommonMethods.ConvertdatetoyearFromat(Result.DATE_OF_BIRTH);
            Result.DATE_OF_JOIN = CommonMethods.ConvertdatetoyearFromat(Result.DATE_OF_JOIN);
            var StaffPersonalProfile = new List<STAFF_PERSONAL>();
            var StaffId = new STAFF_PERSONAL();
            Result.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            var StaffPersonalResult = CMSHandler.SaveOrUpdate(Result, StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffPersonal), string.Empty, false).DataSource.Data;
            if (StaffPersonalResult != null)
            {
                sResult = Common.Messages.RecordsSavedSuccessfully;
            }
            else
            {
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return sResult;
        }
        #endregion
        #region Edit Staff Address Profile
        //public ActionResult EditStaffAddressProfile()
        //{
        //    STAFF_ADDRESS objStaffAddressProfile = new STAFF_ADDRESS();
        //    StaffViewModel objStaff = new StaffViewModel();
        //    //C_Country ...
        //    var cCountry = new List<SUP_COUNTRY>();
        //    cCountry = (List<SUP_COUNTRY>)CMSHandler.FetchData<SUP_COUNTRY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCountry)).DataSource.Data;
        //    objStaff.CCOUNTRY = new SelectList(cCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);
        //    // P_Country
        //    var pCountry = new List<SUP_COUNTRY>();
        //    pCountry = (List<SUP_COUNTRY>)CMSHandler.FetchData<SUP_COUNTRY>(null, SupportDataSQL.GetSupportDataSQL(SupportDataSQLCommands.FetchCountry)).DataSource.Data;
        //    objStaff.PCOUNTRY = new SelectList(pCountry, Common.SUP_COUNTRY.COUNTRY_ID, Common.SUP_COUNTRY.COUNTRY_NAME);

        //    return View(objStaff);
        //}
        public JsonResult FetchStaffAddressProfile()
        {
            var StaffAddressProfile = new List<STAFF_ADDRESS>();
            var StaffAddress = new STAFF_ADDRESS();
            StaffAddress.STAFFNO = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            StaffAddressProfile = (List<STAFF_ADDRESS>)CMSHandler.FetchData<STAFF_ADDRESS>(StaffAddress, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchAddressById)).DataSource.Data;
            return new JsonResult { Data = StaffAddressProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public string UpdateStaffAddressProflie(string StaffAddress)
        {
            var Result = JsonConvert.DeserializeObject<STAFF_ADDRESS>(StaffAddress);
            var StaffAddressProfile = new List<STAFF_ADDRESS>();
            var StaffId = new STAFF_ADDRESS();
            Session[Common.SESSION_VARIABLES.TABLE_PRIMARY_ID] = Result.ADDRESS_NO;
            var StaffAddressResult = CMSHandler.SaveOrUpdate(Result, StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffAddress), string.Empty, false).DataSource.Data;
            if (StaffAddressResult != null)
            {
                sResult = Common.Messages.RecordsSavedSuccessfully;
            }
            else
            {
                sResult = Common.Messages.FailedToSaveRecords;
            }
            return sResult;
        }
        #endregion
        #region Edit Staff Services Profile
        public ActionResult EditStaffServicesProfile()
        {
            return View();
        }
        public JsonResult FetchStaffServiceProfile()
        {
            var StaffServicesProfile = new List<STAFF_SERVICES>();
            var StaffServices = new STAFF_SERVICES();
            StaffServices.STAFF_NO = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            StaffServicesProfile = (List<STAFF_SERVICES>)CMSHandler.FetchData<STAFF_SERVICES>(StaffServices, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchServiceById)).DataSource.Data;
            return new JsonResult { Data = StaffServicesProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion
        public ActionResult StaffProfileView()
        {
            StaffViewModel objViewModel = new StaffViewModel();
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            var StaffProfile = new STAFFPROFILEVIEW();
            var StaffService = new STAFF_SERVICES();
            var StaffCounseling = new STAFF_COUNSELING();
            StaffProfile.STAFF_ID = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            StaffService.STAFF_NO = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            StaffCounseling.STAFF = Session[Common.SESSION_VARIABLES.USER_ID].ToString();
            var StaffProfileView = (List<STAFFPROFILEVIEW>)CMSHandler.FetchData<STAFFPROFILEVIEW>(StaffProfile, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchStaffProfileView), sAcademicYear).DataSource.Data;
            var GetStaffServices = (List<STAFF_SERVICES>)CMSHandler.FetchData<STAFF_SERVICES>(StaffService, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchServiceById)).DataSource.Data;
            var GetStaffPaper = (List<STAFF_PAPER>)CMSHandler.FetchData<STAFF_PAPER>(StaffService, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchPaperById)).DataSource.Data;
            var GetStaffPublish = (List<STAFF_PUBLISH>)CMSHandler.FetchData<STAFF_PUBLISH>(StaffService, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchPublishById)).DataSource.Data;
            var GetStaffTraining = (List<STAFF_TRAINING>)CMSHandler.FetchData<STAFF_TRAINING>(StaffService, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchTrainingById)).DataSource.Data;
            var GetStaffCounseling = (List<STAFF_COUNSELING>)CMSHandler.FetchData<STAFF_COUNSELING>(StaffCounseling, StaffSQL.GetStaffSQL(StaffSQLCommands.FetchCounselingById)).DataSource.Data;
            objViewModel.lstStaffServicesProfile = GetStaffServices;
            objViewModel.lstStaffPaper = GetStaffPaper;
            objViewModel.lstStaffPublish = GetStaffPublish;
            objViewModel.lstStaffTraining = GetStaffTraining;
            objViewModel.lstStaffCounseling = GetStaffCounseling;
            objViewModel.StaffProfileView = (StaffProfileView != null && StaffProfileView.Count > 0) ? StaffProfileView[0] : null;
            return View(objViewModel);
        }
        #endregion
    }
}