﻿using CMS.DAO;
using CMS.SQL.StaffSQL;
using CMS.Utility;
using CMS.ViewModel.Model;
using System;
using System.Collections.Generic;

namespace CMS.ViewModel.ViewModel
{
    public class StaffViewModel : IDisposable
    {
        #region Properties
        public string sSQL = string.Empty;
        ResultArgs resultArg = new ResultArgs();
        DataValue dv = new DataValue();

        StaffInfo objStaff = new StaffInfo();
        #endregion
        #region Edit Personal Info Profile
        //public STAFF_PERSONAL objStaffPersonalProfile { get; set; }
        public STAFFPROFILEVIEW StaffProfileView { get; set; }
        //[DisplayName("Marital status")]
        //public SelectList MARITAL_STATUS { get; set; }
        //[DisplayName("Blood Group")]
        //public SelectList BLOOD_GROUP { get; set; }
        //[DisplayName("Religion")]
        //public SelectList RELIGION { get; set; }
        //[DisplayName("Mother Tongue")]
        //public SelectList MOTHER_TONGUE { get; set; }
        //[DisplayName("Department")]
        //public SelectList DEPARTMENT { get; set; }
        //[DisplayName("Community")]
        //public SelectList COMMUNITY { get; set; }
        //[DisplayName("Nationality")]
        //public SelectList NATIONALITY { get; set; }
        //[DisplayName("Gender")]
        //public SelectList GENDER { get; set; }
        //public STAFF_ADDRESS objStaffAddressProfile { get; set; }
        //[DisplayName("Country")]
        //public SelectList CCOUNTRY { get; set; }
        //[DisplayName("Country")]
        //public SelectList PCOUNTRY { get; set; }
        public List<STAFF_SERVICES> lstStaffServicesProfile { get; set; }
        public List<STAFF_PAPER> lstStaffPaper { get; set; }
        public List<STAFF_PUBLISH> lstStaffPublish { get; set; }
        public List<STAFF_TRAINING> lstStaffTraining { get; set; }
        public List<STAFF_COUNSELING> lstStaffCounseling { get; set; }
        #endregion
        #region PersonalInfo
        // If Exits ..
        public ResultArgs PersonalIfExits(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.PersonalIfExits);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        // This Method is Used to Insert Personal Details ..
        public ResultArgs InsertStaffPersonal(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.InsertPersonal);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }
        // This Method is Used To Fetch Persoanl RecordsByID ...
        public ResultArgs FetchRecordById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchPersonalRecordByID);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        // This Method is Used to Update Staff Personal Information ...
        public ResultArgs UpdateStaffPersonalInfo(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffPersonal);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }

        #endregion

        #region AddressInfo
        // This Method Used to Insert Staff Address ...
        public ResultArgs InsertStaffAddress(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.InsertStaffAddress);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }
        // This Meethod is Used to Fetchdata for Address Table ..
        public ResultArgs FetchAddressById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchAddressById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        // This Method is Used to Update Staff Address ..
        public ResultArgs UpdateStaffAddress(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffAddress);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion

        #region ServiceInfo
        // This Method Used to Insert Staff Services ...
        public ResultArgs InsertStaffservices(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.InsertStaffServices);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }

        // This Meethod is Used to Fetchdata for Service Table ..
        public ResultArgs FetchServiceById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchServiceById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Meethod is Used to Bind Service ..
        public ResultArgs BindService(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.BindServices);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method is Used to Update Staff Services ...
        public ResultArgs UpdateStaffServices(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffService);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }

        // Delete Services ...
        public ResultArgs DeleteServics(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.DeleteServices);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion

        #region CounsellingInfo
        // This Method Used to Insert Staff Counseling ...
        public ResultArgs InsertStaffCounseling(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.InsertStaffCounseling);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }

        // This Meethod is Used to Fetchdata for Counseling Table ..
        public ResultArgs FetchCounselingById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchCounselingById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method Used To Bind Values In the Controles ...
        public ResultArgs BindCounselling(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.BindCounselling);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method is Used to Update Staff Counseling ...
        public ResultArgs UpdateStaffCounseling(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffCounseling);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }

        // This Method is Used to Delete Staff Counselling ...
        public ResultArgs DeleteStaffCounselling(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.DeleteCounselling);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion

        #region PaperInfo
        // This Method Used to Insert Staff Paper ...
        public ResultArgs InsertStaffPaper(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.InsertStaffPaper);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }

        // This Meethod is Used to Fetchdata for Paper Table ..
        public ResultArgs FetchPaperById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchPaperById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method is Used To Bind Values To Cotroles ...
        public ResultArgs BindPaperInfo(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.BindPaperInfo);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method is Used to Update Staff Paper ...
        public ResultArgs UpdateStaffPaper(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffPaper);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }

        // This Method is Used To Delete PaperInfo ..
        public ResultArgs DeletePaperInfo(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.DeletePaper);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }

        #endregion

        #region Book PublishInfo
        // This Method Used to Insert Staff Publish ...
        public ResultArgs InsertStaffPublish(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.InsertStaffPublish);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }

        // This Meethod is Used to Fetchdata for Publish Table ..
        public ResultArgs FetchPublishById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchPublishById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Meethod is Used to Bind Publish Info ..
        public ResultArgs BindPublish(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.BindPublish);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method is Used to Update Staff Publish ...
        public ResultArgs UpdateStaffPublish(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffPublish);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }

        // Delete Publish ...
        public ResultArgs DeletePublish(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.DeletePublish);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion

        #region RolesInfo
        // This Meethod is Used to Fetchdata for staff Roles from Staff personal Table ..
        public ResultArgs FetchStaffRolesById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchSubjectDetailsById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Meethod is Used to Fetchdata for Subject Details from Staff personal Table ..
        public ResultArgs BindStaffRoles(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchSubjectDetailsById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method Used to Update Staff SubjectDetails ...
        public ResultArgs UpdateStaffSubjectDetails(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffSubjectDetails);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion

        #region FamilyInfo
        // This Method Used To Insert Staff Family ..
        public ResultArgs InsertStaffFamily(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.InsertStaffFamily);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }

        // This Meethod is Used to Fetchdata for Family Details ..
        public ResultArgs FetchFamilyById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchFamily);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Meethod is Used to Bind  Family Details ..
        public ResultArgs BindFamily(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.BindFamily);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method Used To Update Staff Family ..
        public ResultArgs UpdateStaffFamily(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffFamily);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }

        // Delete Family ...
        public ResultArgs DeleteFamily(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.DeleteFamily);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion

        #region Qualification
        // This Method Used to Insert Staff Qualification ...
        public ResultArgs InsertStaffQualification(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.InsertStaffQualification);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }

        // This Meethod is Used to Fetchdata for Qualification Table ..
        public ResultArgs FetchQualificationById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchQualificationById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Meethod is Used to Bind Qualification ..
        public ResultArgs BindQualification(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.BindQualification);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method is Used to Update Staff Qualification ...
        public ResultArgs UpdateStaffQualification(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffQualification);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }

        // Delete Qualification ...
        public ResultArgs DeleteQualification(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.DeleteQualification);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion

        #region LeavingInfo
        // This Meethod is Used to Fetchdata for Leaving from Staff personal Table ..
        public ResultArgs FetchLeavingById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchLeavingById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }
        // Leaving ....
        public ResultArgs UpdateStaffLeaving(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffLeaving);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion

        #region TrainingInfo

        // This Method Used to Insert Staff Training ...
        public ResultArgs InsertStaffTraining(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.InsertStaffTraining);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, true);
            }
            return resultArg;
        }

        // This Meethod is Used to Fetchdata for Training Table ..
        public ResultArgs FetchTrainingById(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchTrainingById);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Meethod is Used to Fetchdata for Training Table ..
        public ResultArgs BindTraining(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.BindTraining);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable, dv);
            }
            return resultArg;
        }

        // This Method is Used to Update Staff Training ...
        public ResultArgs UpdateStaffTraining(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.UpdateStaffTraining);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }

        // Delete Training ...
        public ResultArgs DeleteTraining(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.DeleteTraining);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }
        #endregion

        #region Methods

        // This Method is Used to Fetch Last Inserted ID
        public ResultArgs FetchLastInsertedValue()
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchLastInsertedId);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }

            return resultArg;
        }

        // This Method is Used to Fetch All The Records ...
        public ResultArgs FetchStaffInfo(string sAcademicYear)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.FetchStaffList);
                sSQL = sSQL.Replace(Common.Delimiter.QUS + Common.ACADEMIC_YEAR.AC_YEAR, sAcademicYear);
                resultArg = objHandler.FetchData(sSQL, EnumCommand.DataSource.DataTable);
            }
            return resultArg;
        }


        // This Method is Used to Insert ...
        public ResultArgs InsertQuery(string sQuery)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = sQuery;
                resultArg = objHandler.ExecuteAsScripts(sSQL);
            }
            return resultArg;
        }


        // ... DELETE ....

        // This Method is Used To Delete Data
        public ResultArgs DeleteStaffInfo(DataValue dv)
        {
            using (MySQLHandler objHandler = new MySQLHandler())
            {
                sSQL = StaffSQL.GetStaffSQL(StaffSQLCommands.DeleteQuery);
                resultArg = objHandler.ExecuteCommand(dv, sSQL, false);
            }
            return resultArg;
        }


        public void Dispose()
        {
            GC.Collect();
        }

        #endregion

    }
    // Load paper Dropdown List ...
    public class DDLForPaperInfo
    {
        public string Volume { get; set; }
        public string Level { get; set; }
        public string PublishCategory { get; set; }
    }
    // Dropdown for satff Roles
    public class DDLForStaffRolesInfo
    {
        public string Qualification { get; set; }
        public string Designation { get; set; }
        public string SubCategory { get; set; }
        public string Shift { get; set; }
        public string BoardMember { get; set; }
        public string Session { get; set; }
        public string NonTeachingCategory { get; set; }
    }

}
