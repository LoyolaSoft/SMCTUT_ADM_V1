using CMS.DAO;

namespace CMS.SQL.TimeTable
{
    public class TimeTableSQL
    {
        public static string GetTimeTableSQL(TimeTableSQLCommands sEnumCommand)
        {
            string query = "";


            switch (sEnumCommand)
            {
                case TimeTableSQLCommands.FetchPaparType:
                    {
                        query = @"SELECT 
                                    PAPER_TYPE_ID, PAPER_TYPE, IS_ACTIVE, IS_DELETED
                                FROM
                                    SUP_PAPER_TYPE
                                WHERE
                                    IS_ACTIVE = 1;";
                        break;
                    }
                case TimeTableSQLCommands.FetchAcademicBatches:
                    {

                        query = @"SELECT 
                                    AC_BATCH_ID, BATCH_ID, AB.ACADEMIC_YEAR_ID, BATCH_YEAR
                                FROM
                                    ACADEMIC_BATCHES AS AB
                                        INNER JOIN
                                    ACADEMIC_YEAR AS AC ON AC.ACADEMIC_YEAR_ID = AB.ACADEMIC_YEAR_ID
                                WHERE
                                    AC.ACTIVE_YEAR = 1;";

                        break;
                    }
                case TimeTableSQLCommands.FetchTemplateOnlyStaticHours:
                    {

                        query = @"SELECT 
                                        DAY_ORDER_ID,HOUR_ID,PAPER_TYPE_ID,IS_STATIC
                                    FROM
                                        TT_TEMPLATE
                                    WHERE
                                        SETTING_ID =?SETTING_ID AND FACULTY_ID =?FACULTY_ID
                                            AND YEAR =?YEAR
                                            AND GRADUATIONTYPE =?GRADUATIONTYPE
                                            AND IS_ACTIVE =1
                                            AND IS_DELETED !=1
                                            AND IS_STATIC =?IS_STATIC";

                        break;
                    }
                case TimeTableSQLCommands.FetchSettingByAcademicYear:
                    {

                        query = @"SELECT 
                                    SETTING_ID,
                                    SETTING_NAME,
                                    ALLOW_STATIC_HOUR,
                                    NO_OF_HOURS,
                                    NO_OF_DAYS,
                                    BASIC_HOURS_FOR_HOD,
                                    BASIC_HOURS_FOR_STAFF,
                                    SEMESTER_TYPE,
                                    ACADEMIC_YEAR,
                                    IS_ACTIVE,
                                    IS_DELETED,
                                    TEMPLATE_ID
                                FROM
                                    TT_TEMPLATE_SETTING
                                WHERE
                                    IS_ACTIVE = 1 AND ACADEMIC_YEAR = ?ACADEMIC_YEAR;";

                        break;
                    }
                case TimeTableSQLCommands.FetchTimeTableSettings:
                    {

                        query = @"SELECT 
                                        SETTING_ID,
                                        SETTING_NAME,
                                        ALLOW_STATIC_HOUR,
                                        NO_OF_HOURS,
                                        NO_OF_DAYS,
                                        BASIC_HOURS_FOR_HOD,
                                        BASIC_HOURS_FOR_STAFF,
                                        SEMESTER_TYPE,
                                        ACADEMIC_YEAR,
                                        IS_ACTIVE,
                                        IS_DELETED,
                                        TEMPLATE_ID,
                                        GRADUATION_TYPE
                                    FROM
                                        TT_TEMPLATE_SETTING AS STG
                                    WHERE
                                        STG.IS_ACTIVE =?IS_ACTIVE
                                            AND STG.ACADEMIC_YEAR =?ACADEMIC_YEAR;";

                        break;
                    }
                case TimeTableSQLCommands.FetchDepartmentBySettingId:
                    {

                        query = @"SELECT 
                                                    DEPARTMENT_TEMPLATE_ID,
                                                    SETTING_ID,
                                                    DEPARTMENT_ID,
                                                    IS_ACTIVE,
                                                    IS_DELETED,
                                                    ACADEMIC_YEAR
                                                FROM
                                                    DEV_CMS_LIVE.TT_DEPARTMENT_WISE_TEMPLATE AS DP
                                                WHERE
                                                    DP.SETTING_ID =?SETTING_ID AND DP.ACADEMIC_YEAR=?ACADEMIC_YEAR;";

                        break;
                    }
                case TimeTableSQLCommands.FetchProgrammesByDepartmentAndGraduationType:
                    {

                        query = @"";

                        break;
                    }
                case TimeTableSQLCommands.FetchAcademicCurrilum:
                    {

                        query = @"";

                        break;
                    }

                case TimeTableSQLCommands.FetchClassesByProgrammeId:
                    {

                        query = @"";

                        break;
                    }
                case TimeTableSQLCommands.SaveTimeTable:
                    {

                        query = @"INSERT INTO TT_TIMETABLE_?AC_YEAR
                                            (
                                            Course_Id,
                                            Day_Order_Id,
                                            Staff_Id,
                                            Class_Id,
                                            Hour_Id,
                                            Semester_Id,
                                            Hour_Type)
                                            VALUES
                                            (
                                            ?COURSE_ID,
                                            ?DAY_ORDER_ID,
                                            ?STAFF_ID,
                                            ?CLASS_ID,
                                            ?HOUR_ID,
                                            ?SEMESTER_ID,
                                            ?HOURTYPE);";

                        break;
                    }
                case TimeTableSQLCommands.FetchTimeTableClassWiseListByClassId:
                    {
                        query = @"SELECT 
                                        TT.Time_Table_Id,
                                        CR.COURSE_CODE,
                                        ST.STAFF_CODE,
                                        TT.Day_Order_Id,
                                        TT.Hour_Id,
                                        SHT.hourType,
                                        TT.Semester_Id,
                                        SHT.hour_type_id,
                                        tt.Course_Id,
                                        tt.Staff_Id
                                    FROM
                                        tt_timetable_?AC_YEAR AS TT
                                            LEFT JOIN
                                        cp_course_root_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = TT.Course_Id
                                            INNER JOIN
                                        STF_PERSONAL_INFO AS ST ON ST.STAFF_ID = TT.Staff_Id
                                            INNER JOIN
                                        sup_hour_type SHT ON SHT.hour_type_id = TT.Hour_Type
                                    WHERE
                                        TT.Class_Id IN (SELECT 
                                                S_CLASS_ID
                                            FROM
                                                stu_course_info_?AC_YEAR AS SCI
                                            WHERE
                                                SCI.CLASS_ID = ?CLASS_ID)
                                            AND TT.Is_Deleted != 1 
                                            AND tt.Semester_Id = (SELECT 
                                                        ASE.SEMESTER
                                                    FROM
                                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                                            INNER JOIN
                                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ?CLASS_ID
                                                    WHERE
                                                        ASE.PROGRAMME = CLS.PROGRAMME
                                                            AND ASE.BATCH = (SELECT 
                                                                SP.BATCH
                                                            FROM
                                                                STU_CLASS AS SC
                                                                    INNER JOIN
                                                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                                            WHERE
                                                                SC.CLASS_ID = ?CLASS_ID
                                                                    AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                                            GROUP BY SP.BATCH)
                                                            AND ASE.IS_ACTIVE = 1)
                                    UNION SELECT 
                                        TT.Time_Table_Id,
                                        CR.COURSE_CODE,
                                        ST.STAFF_CODE,
                                        TT.Day_Order_Id,
                                        TT.Hour_Id,
                                        SHT.hourType,
                                        TT.Semester_Id,
                                        SHT.hour_type_id,
                                        tt.Course_Id,
                                        tt.Staff_Id
                                    FROM
                                        tt_timetable_?AC_YEAR AS TT
                                            LEFT JOIN
                                        cp_course_root_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = TT.Course_Id
                                            INNER JOIN
                                        stf_personal_info AS ST ON ST.STAFF_ID = TT.Staff_Id
                                            INNER JOIN
                                        sup_hour_type SHT ON SHT.hour_type_id = TT.Hour_Type
                                    WHERE
                                        TT.Class_Id = ?CLASS_ID AND TT.Is_Deleted != 1
                                        AND tt.Semester_Id = (SELECT 
                                                        ASE.SEMESTER
                                                    FROM
                                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                                            INNER JOIN
                                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ?CLASS_ID
                                                    WHERE
                                                        ASE.PROGRAMME = CLS.PROGRAMME
                                                            AND ASE.BATCH = (SELECT 
                                                                SP.BATCH
                                                            FROM
                                                                STU_CLASS AS SC
                                                                    INNER JOIN
                                                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                                            WHERE
                                                                SC.CLASS_ID = ?CLASS_ID
                                                                    AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                                            GROUP BY SP.BATCH)
                                                            AND ASE.IS_ACTIVE = 1);";
                        break;
                    }
                case TimeTableSQLCommands.FetchHoursByTimeTableHours:
                    {
                        query = @"SELECT 
                                        SH.HOUR_ID, SH.HOUR
                                    FROM
                                        SUP_HOURS AS SH
                                            INNER JOIN
                                        TT_TIMETABLE_?AC_YEAR AS TT ON TT.HOUR_ID = SH.HOUR_ID
                                    GROUP BY SH.HOUR_ID;";
                        break;
                    }
                case TimeTableSQLCommands.CheckIsRegularPaper:
                    {
                        query = @"SELECT 
                                        Time_Table_Id
                                    FROM
                                        tt_timetable_?AC_YEAR AS tt
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS cls ON cls.CLASS_ID = ?CLASS_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS ase ON ase.PROGRAMME = cls.PROGRAMME
                                            AND ase.IS_ACTIVE = 1
                                            AND ase.BATCH = (SELECT 
                                                sp.batch
                                            FROM
                                                STU_CLASS AS sc
                                                    INNER JOIN
                                                STU_PERSONAL_INFO AS sp ON sp.STUDENT_ID = sc.STUDENT_ID
                                            WHERE
                                                sc.ACADEMIC_YEAR = ?AC_YEAR
                                                    AND sc.CLASS_ID = ?CLASS_ID
                                            GROUP BY sp.BATCH)
                                    WHERE
                                        tt.Hour_Id = ?HOUR_ID AND tt.Day_Order_Id = ?DAY_ORDER_ID
                                            AND tt.Class_Id = ?CLASS_ID
                                            AND tt.Semester_Id = ase.SEMESTER AND tt.Is_Deleted!=1;";
                        break;
                    }
                case TimeTableSQLCommands.FetchTimeTableById:
                    {
                        query = @"SELECT 
                                        tt.Time_Table_Id,
                                        tt.Course_Id,
                                        tt.Day_Order_Id,
                                        tt.Staff_Id,
                                        tt.Class_Id,
                                        cls.CLASS_CODE,
                                        tt.Hour_Id
                                    FROM
                                        tt_timetable_?AC_YEAR AS tt
                                            INNER JOIN
                                        cp_classes_?AC_YEAR AS cls ON cls.CLASS_ID = tt.Class_Id
                                    WHERE
                                        tt.Time_Table_Id = ?TIME_TABLE_ID;";
                        break;
                    }
                case TimeTableSQLCommands.FetchOnlyRegularCourseByClassId:
                    {
                        query = @"SELECT 
                                        CR.COURSE_ROOT_ID,
                                        CONCAT(IFNULL(CR.COURSE_TITLE, ''),
                                                '(',
                                                IFNULL(CR.COURSE_CODE, ''),
                                                ')') AS COURSE_TITLE
                                    FROM
                                        ACADEMIC_CURRICULUM_?AC_YEAR AS AC
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = AC.COURSE_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ?CLASS_ID
                                    WHERE
                                        CR.IS_DELETED != 1
                                            AND AC.PROGRAMME = CLS.PROGRAMME
                                            AND AC.BATCH = (SELECT 
                                                SP.BATCH
                                            FROM
                                                STU_CLASS AS SC
                                                    INNER JOIN
                                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            WHERE
                                                SC.CLASS_ID = ?CLASS_ID
                                                    AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            GROUP BY SP.BATCH)
                                            AND AC.SEMESTER = (SELECT 
                                                SEMESTER
                                            FROM
                                                ACADEMIC_SEMESTER_?AC_YEAR
                                            WHERE
                                                BATCH = AC.BATCH
                                                    AND PROGRAMME = CLS.PROGRAMME
                                                    AND IS_ACTIVE = 1) AND AC.IS_DELETED!=1 AND CR.IS_COMPULSORY=1;";
                        break;
                    }
                case TimeTableSQLCommands.FetchStaffByCourseIdAndClassId:
                    {
                        query = @"SELECT 
                                        ST.STAFF_ID,
                                        ST.STAFF_CODE,
                                        CONCAT(IFNULL(FIRST_NAME, ''),
                                                '',
                                                ' ',
                                                IFNULL(LAST_NAME, ''),
                                                '(',
                                                STAFF_CODE,
                                                ')') AS FIRST_NAME
                                    FROM
                                        STF_PERSONAL_INFO AS ST
                                            INNER JOIN
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CCS ON CCS.STAFF_ID = ST.STAFF_ID
                                    WHERE
                                        CCS.COURSE_ID = ?COURSE_ID AND CCS.CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case TimeTableSQLCommands.UpdateTimeTableInfo:
                    {
                        query = @"UPDATE tt_timetable_?AC_YEAR
                                            SET
                                            Course_Id= ?COURSE_ID,
                                            Day_Order_Id= ?DAY_ORDER_ID,
                                            Staff_Id= ?STAFF_ID,
                                            Class_Id= ?CLASS_ID,
                                            Hour_Id= ?HOUR_ID,
                                            Semester_Id= ?SEMESTER_ID,
                                            Hour_Type= ?HOURTYPE
                                            WHERE Time_Table_Id= ?TIME_TABLE_ID;";
                        break;
                    }
                case TimeTableSQLCommands.IsTimeTableInfoExisting:
                    {
                        query = @"SELECT 
                                        tt.Time_Table_Id
                                    FROM
                                        tt_timetable_?AC_YEAR AS tt
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ?CLASS_ID
											INNER JOIN
										ACADEMIC_SEMESTER_?AC_YEAR AS ASE ON ASE.PROGRAMME=CLS.PROGRAMME
                                        AND ASE.BATCH = (SELECT 
                                                        SP.BATCH
                                                    FROM
                                                        STU_CLASS AS SC
                                                            INNER JOIN
                                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                                    WHERE
                                                        SC.CLASS_ID = ?CLASS_ID
                                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                                    GROUP BY SP.BATCH)
                                                    AND ASE.IS_ACTIVE = 1
                                    WHERE
                                        tt.Hour_Id = ?HOUR_ID AND tt.Day_Order_Id = ?DAY_ORDER_ID
                                            AND tt.Class_Id = ?CLASS_ID
                                            AND tt.Is_Deleted != 1;";
                        break;
                    }
                case TimeTableSQLCommands.CheckIsStaffAlloted:
                    {
                        query = @"SELECT 
                                        tt.Time_Table_Id,tt.Course_Id
                                    FROM
                                        TT_TIMETABLE_?AC_YEAR AS TT
                                    WHERE
                                        TT.HOUR_ID = ?HOUR_ID AND TT.DAY_ORDER_ID = ?DAY_ORDER_ID
                                            AND TT.STAFF_ID = ?STAFF_ID
                                            AND TT.IS_DELETED != 1 GROUP BY TT.COURSE_ID;";
                        break;
                    }
                case TimeTableSQLCommands.FetchOptinalHourByClassIdAndHourIdAndCourseId:
                    {
                        query = @"SELECT 
                                        TT.Time_Table_Id
                                    FROM
                                        TT_TIMETABLE_?AC_YEAR AS TT
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = TT.COURSE_ID
                                    WHERE
                                        TT.Class_Id IN (SELECT 
                                                S_CLASS_ID
                                            FROM
                                                STU_COURSE_INFO_?AC_YEAR AS SCI
                                            WHERE
                                                SCI.CLASS_ID = ?CLASS_ID)
                                            AND TT.Is_Deleted != 1
                                            AND CR.SEMESTER_ID = (SELECT 
                                                ASE.SEMESTER
                                            FROM
                                                ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                                    INNER JOIN
                                                CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ?CLASS_ID
                                            WHERE
                                                ASE.PROGRAMME = CLS.PROGRAMME
                                                    AND ASE.BATCH = (SELECT 
                                                        SP.BATCH
                                                    FROM
                                                        STU_CLASS AS SC
                                                            INNER JOIN
                                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                                    WHERE
                                                        SC.CLASS_ID = ?CLASS_ID
                                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                                    GROUP BY SP.BATCH)
                                                    AND ASE.IS_ACTIVE = 1)
                                            AND TT.Hour_Id = ?HOUR_ID
                                            AND TT.Day_Order_Id = ?DAY_ORDER_ID;";
                        break;
                    }
                case TimeTableSQLCommands.FetchCourseByClassId:
                    {
                        query = @"SELECT 
                                        CR.COURSE_ROOT_ID,
                                        CONCAT(IFNULL(CR.COURSE_TITLE, ''),
                                                '(',
                                                IFNULL(CR.COURSE_CODE, ''),
                                                ')') AS COURSE_TITLE
                                    FROM
                                        ACADEMIC_CURRICULUM_?AC_YEAR AS AC
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = AC.COURSE_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ?CLASS_ID
                                    WHERE
                                        CR.IS_DELETED != 1
                                            AND AC.PROGRAMME = CLS.PROGRAMME
                                            AND AC.BATCH = (SELECT 
                                                SP.BATCH
                                            FROM
                                                STU_CLASS AS SC
                                                    INNER JOIN
                                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            WHERE
                                                SC.CLASS_ID = ?CLASS_ID
                                                    AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            GROUP BY SP.BATCH)
                                            AND AC.SEMESTER = (SELECT 
                                                SEMESTER
                                            FROM
                                                ACADEMIC_SEMESTER_?AC_YEAR
                                            WHERE
                                                BATCH = AC.BATCH
                                                    AND PROGRAMME = CLS.PROGRAMME
                                                    AND IS_ACTIVE = 1) AND AC.IS_DELETED!=1;";
                        break;
                    }
                case TimeTableSQLCommands.FetchStaffByCourseId:
                    {
                        query = @"SELECT 
                                        ST.STAFF_ID,
                                        ST.STAFF_CODE,
                                        CONCAT(IFNULL(FIRST_NAME, ''),
                                                '',
                                                ' ',
                                                IFNULL(LAST_NAME, ''),
                                                '(',
                                                STAFF_CODE,
                                                ')') AS FIRST_NAME
                                    FROM
                                        STF_PERSONAL_INFO AS ST
                                            INNER JOIN
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CCS ON CCS.STAFF_ID = ST.STAFF_ID
                                    WHERE
                                        CCS.COURSE_ID = ?COURSE_ID;";
                        break;
                    }
                case TimeTableSQLCommands.FetchSelectedClassIdByCourseIdAndClassId:
                    {
                        query = @"SELECT 
                                        S_CLASS_ID
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR
                                    WHERE
                                        CLASS_ID = ?CLASS_ID AND COURSE_ID = ?COURSE_ID
                                    GROUP BY S_CLASS_ID";
                        break;
                    }
                case TimeTableSQLCommands.IsTimeTableInfoExistingForOptionalPaper:
                    {
                        query = @"SELECT 
                                        tt.Time_Table_Id
                                    FROM
                                        tt_timetable_?AC_YEAR AS tt
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = tt.Course_Id
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ?CLASS_ID
                                    WHERE
                                        tt.Hour_Id = ?HOUR_ID AND tt.Day_Order_Id = ?DAY_ORDER_ID
                                            AND tt.Class_Id = ?CLASS_ID
                                            AND tt.Course_Id=?COURSE_ID
                                            AND tt.Is_Deleted != 1
                                            AND CR.SEMESTER_ID = (SELECT 
                                                ASE.SEMESTER
                                            FROM
                                                ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                            WHERE
                                                ASE.PROGRAMME = CLS.PROGRAMME
                                                    AND ASE.BATCH = (SELECT 
                                                        SP.BATCH
                                                    FROM
                                                        STU_CLASS AS SC
                                                            INNER JOIN
                                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                                    WHERE
                                                        SC.CLASS_ID = ?CLASS_ID
                                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                                    GROUP BY SP.BATCH)
                                                    AND ASE.IS_ACTIVE = 1);";
                        break;
                    }
                case TimeTableSQLCommands.FetchHourType:
                    {
                        query = @"SELECT 
                                        TT.Paper_Type_Id
                                    FROM
                                        TT_TEMPLATE AS TT
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_LEVEL = TT.GRADUATIONTYPE
                                            AND CLS.CLASS_YEAR = TT.YEAR
                                            INNER JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DT ON DT.DEPARTMENT_ID = CLS.DEPARTMENT
                                            INNER JOIN
                                        CP_FACULTY_?AC_YEAR AS CF ON CF.FACULTY_ID = DT.FACULTY
                                    WHERE
                                        CLS.CLASS_ID = ?CLASS_ID AND TT.DAY_ORDER_ID = ?DAY_ORDER_ID
                                            AND TT.HOUR_ID = ?HOUR_ID
                                            AND TT.FACULTY_ID = CF.FACULTY_ID;";
                        break;
                    }
                case TimeTableSQLCommands.FetchAllTimeTableSettings:
                    {
                        query = @"SELECT 
                                        TTS.SETTING_ID,
                                        TTS.SETTING_NAME,
                                        TTS.NO_OF_DAYS,
                                        TTS.NO_OF_HOURS,
                                        TTS.BASIC_HOURS_FOR_HOD,
                                        TTS.BASIC_HOURS_FOR_STAFF,
                                        TTS.SEMESTER_TYPE,
                                        SS.SEMESTER_NAME,
                                        TTS.ACADEMIC_YEAR,
                                        TTS.ALLOW_STATIC_HOUR,
                                        TTS.GRADUATION_TYPE,
                                        SCL.CLASSLEVEL
                                    FROM
                                        TT_TEMPLATE_SETTING AS TTS
                                            INNER JOIN
                                        SUP_SEMESTER AS SS ON SS.SUP_SEMESTER_ID = TTS.SEMESTER_TYPE
                                            INNER JOIN
                                        SUP_CLASS_LEVEL AS SCL ON SCL.CLASSLEVELID = TTS.GRADUATION_TYPE
                                            INNER JOIN
                                        SUP_HOURS AS SH ON SH.HOUR_ID = TTS.NO_OF_HOURS
                                    WHERE
                                        TTS.IS_DELETED != 1;";
                        break;
                    }
                case TimeTableSQLCommands.SaveTimeTableSettings:
                    {
                        query = @"INSERT INTO TT_TEMPLATE_SETTING
                                            (
                                            SETTING_NAME,
                                            ALLOW_STATIC_HOUR,
                                            NO_OF_HOURS,
                                            NO_OF_DAYS,
                                            BASIC_HOURS_FOR_HOD,
                                            BASIC_HOURS_FOR_STAFF,
                                            SEMESTER_TYPE,
                                            ACADEMIC_YEAR,
                                            GRADUATION_TYPE)
                                            VALUES
                                            (
                                            ?SETTING_NAME,
                                            ?ALLOW_STATIC_HOUR,
                                            ?NO_OF_HOURS,
                                            ?NO_OF_DAYS,
                                            ?BASIC_HOURS_FOR_HOD,
                                            ?BASIC_HOURS_FOR_STAFF,
                                            ?SEMESTER_TYPE,
                                            ?ACADEMIC_YEAR,
                                            ?GRADUATION_TYPE);";
                        break;
                    }
                case TimeTableSQLCommands.IsTimeTableSettingExisting:
                    {
                        query = @"SELECT 
                                        SETTING_ID
                                    FROM
                                        TT_TEMPLATE_SETTING
                                    WHERE
                                        ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                            AND GRADUATION_TYPE = ?GRADUATION_TYPE
                                            AND SEMESTER_TYPE = ?SEMESTER_TYPE
                                            AND SETTING_NAME = ?SETTING_NAME;";
                        break;
                    }
                case TimeTableSQLCommands.FetchTimeTableSettingById:
                    {
                        query = @"SELECT 
                                        SETTING_NAME,
                                        ALLOW_STATIC_HOUR,
                                        NO_OF_HOURS,
                                        NO_OF_DAYS,
                                        BASIC_HOURS_FOR_HOD,
                                        BASIC_HOURS_FOR_STAFF,
                                        SEMESTER_TYPE,
                                        ACADEMIC_YEAR,
                                        GRADUATION_TYPE
                                    FROM
                                        TT_TEMPLATE_SETTING
                                    WHERE
                                        SETTING_ID = ?SETTING_ID;";
                        break;
                    }
                case TimeTableSQLCommands.UpdateTimeTableSettings:
                    {
                        query = @"UPDATE TT_TEMPLATE_SETTING
                                        SET
                                        SETTING_NAME= ?SETTING_NAME,
                                        ALLOW_STATIC_HOUR= ?ALLOW_STATIC_HOUR,
                                        NO_OF_HOURS= ?NO_OF_HOURS,
                                        NO_OF_DAYS= ?NO_OF_DAYS,
                                        BASIC_HOURS_FOR_HOD= ?BASIC_HOURS_FOR_HOD,
                                        BASIC_HOURS_FOR_STAFF= ?BASIC_HOURS_FOR_STAFF,
                                        SEMESTER_TYPE= ?SEMESTER_TYPE,
                                        ACADEMIC_YEAR= ?ACADEMIC_YEAR,
                                        GRADUATION_TYPE= ?GRADUATION_TYPE
                                        WHERE SETTING_ID= ?SETTING_ID;";
                        break;
                    }
                case TimeTableSQLCommands.DeleteTimeTableSettings:
                    {
                        query = @"UPDATE TT_TEMPLATE_SETTING
                                        SET
                                        IS_DELETED=1
                                        WHERE SETTING_ID=?SETTING_ID";
                        break;
                    }
                case TimeTableSQLCommands.FetchDepartmentWiseTimetableTemplate:
                    {
                        query = @"SELECT 
                                        TDW.DEPARTMENT_TEMPLATE_ID,
                                        TTS.SETTING_NAME,
                                        DT.DEPARTMENT,
                                        TDW.IS_ACTIVE,
                                        TDW.Academic_Year
                                    FROM
                                        TT_DEPARTMENT_WISE_TEMPLATE AS TDW
                                            INNER JOIN
                                        TT_TEMPLATE_SETTING AS TTS ON TTS.SETTING_ID = TDW.SETTING_ID
                                            INNER JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DT ON DT.DEPARTMENT_ID = TDW.DEPARTMENT_ID
                                    WHERE
                                        TDW.IS_DELETED != 1;";
                        break;
                    }
                case TimeTableSQLCommands.DeleteDepartmentWiseTimeTableTemplate:
                    {
                        query = @"UPDATE TT_DEPARTMENT_WISE_TEMPLATE
                                        SET
                                        IS_DELETED=1
                                        WHERE DEPARTMENT_TEMPLATE_ID=?DEPARTMENT_TEMPLATE_ID";
                        break;
                    }
                case TimeTableSQLCommands.IsDepartmentWiseTimeTableTemplateExisting:
                    {
                        query = @"SELECT 
                                        DEPARTMENT_TEMPLATE_ID
                                    FROM
                                        TT_DEPARTMENT_WISE_TEMPLATE
                                    WHERE
                                        DEPARTMENT_ID = ?DEPARTMENT_ID AND IS_DELETED != 1;";
                        break;
                    }
                case TimeTableSQLCommands.SaveDepartmentWiseTimeTableTemplate:
                    {
                        query = @"INSERT INTO TT_DEPARTMENT_WISE_TEMPLATE
                                        (
                                        SETTING_ID,
                                        DEPARTMENT_ID,
                                        ACADEMIC_YEAR,
                                        IS_ACTIVE)
                                        VALUES
                                        (
                                        ?SETTING_ID,
                                        ?DEPARTMENT_ID,
                                        ?ACADEMIC_YEAR,
                                        ?IS_ACTIVE);";
                        break;
                    }
                case TimeTableSQLCommands.FetchDepartmentWiseTimetableTemplateById:
                    {
                        query = @"SELECT 
                                        DEPARTMENT_TEMPLATE_ID,
                                        SETTING_ID,
                                        DEPARTMENT_ID,
                                        IS_ACTIVE,
                                        Academic_Year
                                    FROM
                                        TT_DEPARTMENT_WISE_TEMPLATE
                                    WHERE
                                        DEPARTMENT_TEMPLATE_ID = ?DEPARTMENT_TEMPLATE_ID;";
                        break;
                    }
                case TimeTableSQLCommands.UpdateDepartmentWiseTimeTableTemplate:
                    {
                        query = @"UPDATE TT_DEPARTMENT_WISE_TEMPLATE
                                        SET
                                        SETTING_ID= ?SETTING_ID,
                                        DEPARTMENT_ID= ?DEPARTMENT_ID,
                                        IS_ACTIVE= ?IS_ACTIVE,
                                        ACADEMIC_YEAR= ?ACADEMIC_YEAR
                                        WHERE DEPARTMENT_TEMPLATE_ID= ?DEPARTMENT_TEMPLATE_ID;";
                        break;
                    }
                case TimeTableSQLCommands.FetchTimeTableTemplate:
                    {
                        query = @"SELECT 
                                        TTT.TT_Temp_Id,
                                        SDO.DAY,
                                        SH.HOUR,
                                        SPT.PAPER_TYPE,
                                        CF.FACULTY,
                                        TTS.SETTING_NAME,
                                        SCY.CLASS_YEAR,
                                        SCL.CLASSLEVEL
                                    FROM
                                        TT_TEMPLATE AS TTT
                                            INNER JOIN
                                        SUP_DAY_ORDER AS SDO ON SDO.DAY_ORDER_ID = TTT.DAY_ORDER_ID
                                            INNER JOIN
                                        SUP_HOURS AS SH ON SH.HOUR_ID = TTT.HOUR_ID
                                            INNER JOIN
                                        SUP_PAPER_TYPE AS SPT ON SPT.PAPER_TYPE_ID = TTT.PAPER_TYPE_ID
                                            INNER JOIN
                                        CP_FACULTY_?AC_YEAR AS CF ON CF.FACULTY_ID = TTT.FACULTY_ID
                                            INNER JOIN
                                        TT_TEMPLATE_SETTING AS TTS ON TTS.SETTING_ID = TTT.SETTING_ID
                                            INNER JOIN
                                        SUP_CLASS_LEVEL AS SCL ON SCL.CLASSLEVELID = TTT.GRADUATIONTYPE
		                                    INNER JOIN
	                                    SUP_CLASS_YEAR AS SCY ON SCY.CLASS_YEAR_ID=TTT.YEAR
                                    WHERE
                                        TTT.IS_DELETED != 1;";
                        break;
                    }
                case TimeTableSQLCommands.IsTimeTableTemplateExisting:
                    {
                        query = @"SELECT 
                                        TT_Temp_Id
                                    FROM
                                        TT_TEMPLATE
                                    WHERE
                                        HOUR_ID = ?HOUR_ID AND DAY_ORDER_ID = ?DAY_ORDER_ID
                                            AND FACULTY_ID = ?FACULTY_ID
                                            AND SETTING_ID = ?SETTING_ID
                                            AND YEAR = ?YEAR
                                            AND GRADUATIONTYPE = ?GRADUATIONTYPE
                                            AND IS_DELETED!=1;";
                        break;
                    }
                case TimeTableSQLCommands.SaveTimeTableTemplate:
                    {
                        query = @"INSERT INTO TT_TEMPLATE
                                            (
                                            DAY_ORDER_ID,
                                            HOUR_ID,
                                            PAPER_TYPE_ID,
                                            IS_STATIC,
                                            FACULTY_ID,
                                            SETTING_ID,
                                            YEAR,
                                            GRADUATIONTYPE)
                                            VALUES
                                            (
                                            ?DAY_ORDER_ID,
                                            ?HOUR_ID,
                                            ?PAPER_TYPE_ID,
                                            ?IS_STATIC,
                                            ?FACULTY_ID,
                                            ?SETTING_ID,
                                            ?YEAR,
                                            ?GRADUATIONTYPE);";
                        break;
                    }
                case TimeTableSQLCommands.FetchTimeTableTemplateById:
                    {
                        query = @"SELECT 
                                        TT_Temp_Id,
                                        Day_Order_Id,
                                        Hour_Id,
                                        Paper_Type_Id,
                                        Is_Static,
                                        Faculty_Id,
                                        Setting_Id,
                                        Year,
                                        GraduationType
                                    FROM
                                        TT_TEMPLATE
                                    WHERE
                                        TT_TEMP_ID = ?TT_TEMP_ID;";
                        break;
                    }
                case TimeTableSQLCommands.UpdateTimeTableTemplate:
                    {
                        query = @"UPDATE TT_TEMPLATE
                                        SET
                                        DAY_ORDER_ID=?DAY_ORDER_ID,
                                        HOUR_ID=?HOUR_ID,
                                        PAPER_TYPE_ID=?PAPER_TYPE_ID,
                                        IS_STATIC=?IS_STATIC,
                                        FACULTY_ID=?FACULTY_ID,
                                        SETTING_ID=?SETTING_ID,
                                        YEAR=?YEAR,
                                        GRADUATIONTYPE=?GRADUATIONTYPE
                                        WHERE TT_TEMP_ID =?TT_TEMP_ID;";
                        break;
                    }
                case TimeTableSQLCommands.DeleteTimeTableTemplate:
                    {
                        query = @"UPDATE TT_TEMPLATE
                                        SET
                                        IS_DELETED=1
                                        WHERE TT_TEMP_ID=?TT_TEMP_ID;";
                        break;
                    }
                case TimeTableSQLCommands.FetchIsStaticAndPaperTypeByClassId:
                    {
                        query = @"SELECT 
                                        TTT.IS_STATIC, TTT.Paper_Type_Id
                                    FROM
                                        CP_CLASSES_?AC_YEAR AS CLS
                                            INNER JOIN
                                        SUP_CLASS_LEVEL AS SCL ON SCL.CLASSLEVELID = CLS.CLASS_LEVEL
                                            INNER JOIN
                                        SUP_CLASS_YEAR AS SCY ON SCY.CLASS_YEAR_ID = CLS.CLASS_YEAR
                                            INNER JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DT ON DT.DEPARTMENT_ID = CLS.DEPARTMENT
                                            INNER JOIN
                                        CP_FACULTY_?AC_YEAR AS CF ON CF.FACULTY_ID = DT.FACULTY
                                            INNER JOIN
                                        TT_TEMPLATE AS TTT ON TTT.GRADUATIONTYPE = CLS.CLASS_LEVEL
                                            AND TTT.YEAR = CLS.CLASS_YEAR
                                    WHERE
                                        TTT.DAY_ORDER_ID = ?DAY_ORDER_ID AND TTT.HOUR_ID = ?HOUR_ID
                                            AND CLS.CLASS_ID = ?CLASS_ID AND TTT.FACULTY_ID=CF.FACULTY_ID AND TTT.IS_DELETED!=1;";
                        break;
                    }
                case TimeTableSQLCommands.FetchTimeTableInfoForStaffByClassId:
                    {
                        query = @"SELECT 
                                        TT.Time_Table_Id,
                                        TT.Course_Id,
                                        TT.Day_Order_Id,
                                        TT.Staff_Id,
                                        TT.Class_Id,
                                        TT.Hour_Id,
                                        SHT.hour_type_id,
                                        SHT.hourType,
                                        CR.Semester_Id,
                                        CLS.CLASS_CODE,
                                        CLS.PROGRAMME,
                                        CONCAT('(',
                                                IFNULL(CR.COURSE_CODE, ''),
                                                ') ',
                                                IFNULL(CR.COURSE_TITLE, '')) AS COURSE_CODE
                                    FROM
                                        TT_TIMETABLE_?AC_YEAR AS TT
                                            INNER JOIN
                                        sup_hour_type SHT ON SHT.hour_type_id = TT.Hour_Type
                                            LEFT JOIN
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CC ON CC.COURSE_ID = TT.COURSE_ID
                                            AND CC.CLASS_ID = TT.CLASS_ID
                                            AND CC.STAFF_ID = TT.STAFF_ID
                                            LEFT JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = TT.CLASS_ID
                                            LEFT JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CC.COURSE_ID
                                            AND CR.SEMESTER_ID = (SELECT 
                                                ASE.SEMESTER
                                            FROM
                                                ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                            WHERE
                                                ASE.IS_ACTIVE = 1
                                                    AND ASE.PROGRAMME = CLS.PROGRAMME
                                                    AND ASE.BATCH = (SELECT 
                                                        SP.BATCH
                                                    FROM
                                                        STU_CLASS AS SC
                                                            INNER JOIN
                                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                                    WHERE
                                                        SC.CLASS_ID = ?CLASS_ID
                                                    GROUP BY SP.BATCH
                                                    LIMIT 1))
                                    WHERE
                                        TT.STAFF_ID = ?STAFF_ID AND TT.CLASS_ID = ?CLASS_ID
                                            AND TT.IS_DELETED != 1
                                            AND CC.IS_DELETED != 1
                                            AND CR.IS_DELETED != 1 
                                    UNION SELECT 
                                        TT.Time_Table_Id,
                                        TT.Course_Id,
                                        TT.Day_Order_Id,
                                        TT.Staff_Id,
                                        TT.Class_Id,
                                        TT.Hour_Id,
                                        SHT.hour_type_id,
                                        SHT.hourType,
                                        TT.Semester_Id,
                                        CLS.CLASS_CODE,
                                        CLS.PROGRAMME,
                                        '' AS COURSE_CODE
                                    FROM
                                        TT_TIMETABLE_?AC_YEAR AS TT
                                            INNER JOIN
                                        SUP_HOUR_TYPE SHT ON SHT.HOUR_TYPE_ID = TT.HOUR_TYPE
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = TT.CLASS_ID
                                    WHERE
                                        TT.CLASS_ID = ?CLASS_ID AND TT.STAFF_ID = ?STAFF_ID
                                            AND TT.COURSE_ID = 0;";
                        break;
                    }
                case TimeTableSQLCommands.FetchTotalHoursAndDaysByClassId:
                    {
                        query = @"SELECT 
                                        TTS.NO_OF_DAYS, TTS.NO_OF_HOURS
                                    FROM
                                        CP_CLASSES_?AC_YEAR AS CLS
                                            INNER JOIN
                                        TT_DEPARTMENT_WISE_TEMPLATE AS TTD ON TTD.DEPARTMENT_ID = CLS.DEPARTMENT
                                            INNER JOIN
                                        TT_TEMPLATE_SETTING AS TTS ON TTS.SETTING_ID = TTD.SETTING_ID
                                            AND TTD.ACADEMIC_YEAR = ?AC_YEAR
                                            AND TTS.ACADEMIC_YEAR = ?AC_YEAR
                                    WHERE
                                        CLS.CLASS_ID = ?CLASS_ID
                                    GROUP BY CLS.CLASS_ID;";
                        break;
                    }
                case TimeTableSQLCommands.FetchHoursByNoOfHours:
                    {
                        query = @"SELECT 
                                        HOUR_ID, HOUR
                                    FROM
                                        SUP_HOURS AS SH
                                    ORDER BY HOUR ASC
                                    LIMIT ?NO_OF_HOURS;";
                        break;
                    }
                case TimeTableSQLCommands.CheckIsRegularPaperByStaffId:
                    {
                        query = @"SELECT 
                                        Time_Table_Id
                                    FROM
                                        TT_TIMETABLE_?AC_YEAR AS TT
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ?CLASS_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE ON ASE.PROGRAMME = CLS.PROGRAMME
                                            AND ASE.IS_ACTIVE = 1
                                            AND ASE.BATCH = (SELECT 
                                                SP.BATCH
                                            FROM
                                                STU_CLASS AS SC
                                                    INNER JOIN
                                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            WHERE
                                                SC.ACADEMIC_YEAR = ?AC_YEAR
                                                    AND SC.CLASS_ID = ?CLASS_ID
                                            GROUP BY SP.BATCH)
                                    WHERE
                                        TT.HOUR_ID = ?HOUR_ID AND TT.DAY_ORDER_ID = ?DAY_ORDER_ID
                                            AND TT.CLASS_ID = ?CLASS_ID AND TT.STAFF_ID=?STAFF_ID
                                            AND TT.SEMESTER_ID = ASE.SEMESTER AND TT.IS_DELETED!=1;";
                        break;
                    }
                case TimeTableSQLCommands.DeleteTimeTable:
                    {
                        query = @"UPDATE TT_TIMETABLE_?AC_YEAR 
                                        SET 
                                            IS_DELETED = 1
                                        WHERE
                                            TIME_TABLE_ID = ?TIME_TABLE_ID;";
                        break;
                    }
                case TimeTableSQLCommands.FetchTimeTableInfoForStaffByClassIdOnlyOptionalPapers:
                    {
                        query = @"SELECT 
                                        TT.Time_Table_Id,
                                        TT.Course_Id,
                                        TT.Day_Order_Id,
                                        TT.Staff_Id,
                                        TT.Class_Id,
                                        TT.Hour_Id,
                                        TT.hour_Type AS hour_type_id,
                                        CR.Semester_Id,
                                        CLS.CLASS_CODE,
                                        CLS.PROGRAMME,
                                        CONCAT('(',
                                                IFNULL(CR.COURSE_CODE, ''),
                                                ') ',
                                                IFNULL(CR.COURSE_TITLE, '')) AS COURSE_CODE
                                    FROM
                                        TT_TIMETABLE_?AC_YEAR AS TT
                                            INNER JOIN
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CC ON CC.COURSE_ID = TT.COURSE_ID
                                            AND CC.CLASS_ID = TT.CLASS_ID
                                            AND CC.STAFF_ID = TT.STAFF_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = TT.CLASS_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CLS.COURSE_ID
                                    WHERE
                                        TT.STAFF_ID = ?STAFF_ID AND TT.CLASS_ID = ?CLASS_ID
                                            AND TT.IS_DELETED != 1
                                            AND CC.IS_DELETED != 1
                                            AND CR.IS_DELETED != 1;";
                        break;
                    }
            }
            return query;
        }
    }
}
