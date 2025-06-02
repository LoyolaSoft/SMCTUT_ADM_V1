using CMS.DAO;

namespace CMS.SQL.Attendance
{
    public static class AttendanceSQL
    {
        public static string GetAttendanceSQL(AttendanceSQLCommands sEnumCommand)
        {
            string query = string.Empty;
            switch (sEnumCommand)
            {
                case AttendanceSQLCommands.FetchStudentListForAttendanceByClassId:
                    {

                        query = @"SELECT 
                                SP.ROLL_NO,
                                CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                        IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                ABS.ATTENDANCE_ID,
                                ABS.REASON_ID,
                                ABS.ATTENDANCE_TYPE_ID,
                                ABS.ENTRY_ID,
                                ABS.ENTRY_DATE,
                                ATT.ATTENDANCE_TYPE_ID
                            FROM
                                stu_class AS scls
                                    INNER JOIN
                                stu_personal_info AS sp ON sp.STUDENT_ID = scls.STUDENT_ID
                                    LEFT JOIN
                                att_absentee_list_?AC_YEAR AS ABS ON ABS.ATTENDENCE_DATE = CURRENT_DATE()
                                    AND scls.STUDENT_ID = ABS.STUDENT_ID
                                    LEFT JOIN
                                sup_reason AS sr ON sr.REASON_ID = ABS.REASON_ID
                                    LEFT JOIN
                                SUP_ATTENDANCE_TYPE AS ATT ON ATT.ATTENDANCE_TYPE_ID = abs.ATTENDANCE_TYPE_ID
                            WHERE
                                scls.CLASS_ID = ?CLASS_ID
                                    AND scls.ACADEMIC_YEAR = ?AC_YEAR ;";
                        break;
                    }
                case AttendanceSQLCommands.SaveAbsenteeEntryByCourseStaff:
                    {
                        query = @"INSERT INTO ATT_ABSENTEE_LIST_?AC_YEAR
                                        (
                                        ATTENDENCE_DATE,
                                        STAFF_ID,
                                        COURSE_ID,
                                        HOUR_FROM,
                                        HOUR_TO,
                                        STUDENT_ID,
                                        CLASS_ID,
                                        REASON_ID,
                                        ENTRY_ID,
                                        ENTRY_DATE,
                                        S_CLASS_ID,
                                        ATTENDANCE_TYPE_ID
                                        )
                                        VALUES
                                        (?ATTENDENCE_DATE,
                                        ?STAFF_ID,
                                        ?COURSE_ID,
                                        ?HOUR_FROM,
                                        ?HOUR_TO,
                                        ?STUDENT_ID,
                                        ?CLASS_ID,
                                        ?REASON_ID,
                                        ?ENTRY_ID,
                                        ?ENTRY_DATE,
                                        ?S_CLASS_ID,
                                        ?ATTENDANCE_TYPE_ID);";
                        return query;
                    }
                case AttendanceSQLCommands.UpdateAbsenteeEntryByCourseStaff:
                    {
                        query = @"UPDATE ATT_ABSENTEE_LIST_?AC_YEAR
                                    SET
                                    ATTENDENCE_DATE = ?ATTENDENCE_DATE,
                                    STAFF_ID = ?STAFF_ID,
                                    COURSE_ID = ?COURSE_ID,
                                    HOUR_FROM = ?HOUR_FROM,
                                    HOUR_TO = ?HOUR_TO,
                                    STUDENT_ID = ?STUDENT_ID,
                                    CLASS_ID = ?CLASS_ID,
                                    REASON_ID = ?REASON_ID,
                                    ENTRY_ID = ?ENTRY_ID,
                                    ENTRY_DATE = ?ENTRY_DATE,
                                    S_CLASS_ID = ?S_CLASS_ID,
                                    ATTENDANCE_TYPE_ID = ?ATTENDANCE_TYPE_ID
                                    WHERE ATTENDANCE_ID = ?ATTENDANCE_ID;";
                        return query;
                    }
                case AttendanceSQLCommands.FetchStudentListForAttendanceByCourseIdAndStaffId:
                    {

                        query = @"SELECT 
                                SP.ROLL_NO,
                                CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                        IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                CR.COURSE_CODE,
                                TT.COURSE_ID,
                                CR.COURSE_TITLE,
                                CLS.CLASS_ID AS S_CLASS_ID,
                                CLS.CLASS_ID,
                                ABS.ATTENDANCE_ID,
                                ABS.REASON_ID,
                                ABS.ATTENDANCE_TYPE_ID,
                                ABS.ENTRY_ID,
                                ABS.ENTRY_DATE,
                                SR.REASON_ID,
                                ATT.ATTENDANCE_TYPE_ID,SP.STUDENT_ID
                            FROM
                                CALENDER_?AC_YEAR AS CAL
                                    INNER JOIN
                                TT_TIMETABLE_?AC_YEAR AS TT ON TT.DAY_ORDER_ID = CAL.DAY_ORDER
                                    AND TT.CLASS_ID = ?CLASS_ID
                                    AND TT.STAFF_ID = ?STAFF_ID
                                    AND TT.HOUR_ID = ?HOUR_ID
                                    INNER JOIN
                                STU_CLASS AS CLS ON CLS.CLASS_ID = ?CLASS_ID AND CLS.ACADEMIC_YEAR=?AC_YEAR
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CLS.STUDENT_ID
                                    INNER JOIN
                                CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = TT.COURSE_ID
                                    LEFT JOIN
                                ATT_ABSENTEE_LIST_?AC_YEAR AS ABS ON CAL.DAY_ORDER_DATE = ABS.ATTENDENCE_DATE
                                    AND ABS.STUDENT_ID = CLS.STUDENT_ID
                                    LEFT JOIN
                                SUP_REASON AS SR ON SR.REASON_ID = ABS.REASON_ID
                                    LEFT JOIN
                                SUP_ATTENDANCE_TYPE AS ATT ON ATT.ATTENDANCE_TYPE_ID = ABS.ATTENDANCE_TYPE_ID
                            WHERE
                                DAY_ORDER_DATE =?ATTENDANCE_DATE 
                                    AND CAL.IS_DELETED != 1
                            ORDER BY SP.ROLL_NO , FIRST_NAME;";
                        break;
                    }
                case AttendanceSQLCommands.FetchStudentListForAttendanceByOptionalCourseIdAndStaffId:
                    {

                        query = @"SELECT 
                                SP.ROLL_NO,
                                CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                        IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                CR.COURSE_CODE,
                                TT.COURSE_ID,
                                CR.COURSE_TITLE,
                                CLS.S_CLASS_ID,
                                CLS.CLASS_ID,
                                ABS.ATTENDANCE_ID,
                                ABS.REASON_ID,
                                ABS.ATTENDANCE_TYPE_ID,
                                ABS.ENTRY_ID,
                                ABS.ENTRY_DATE,
                                SR.REASON_ID,
                                ATT.ATTENDANCE_TYPE_ID,SP.STUDENT_ID
                            FROM
                                CALENDER_?AC_YEAR AS CAL
                                    INNER JOIN
                                TT_TIMETABLE_?AC_YEAR AS TT ON TT.DAY_ORDER_ID = CAL.DAY_ORDER
                                    AND TT.CLASS_ID = ?CLASS_ID
                                    AND TT.STAFF_ID = ?STAFF_ID
                                    AND TT.HOUR_ID = ?HOUR_ID
                                    INNER JOIN
                                STU_COURSE_INFO_?AC_YEAR AS CLS ON CLS.S_CLASS_ID = ?CLASS_ID AND CLS.COURSE_ID=TT.COURSE_ID
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CLS.STUDENT_ID
                                    INNER JOIN
                                CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CLS.COURSE_ID
                                    LEFT JOIN
                                ATT_ABSENTEE_LIST_?AC_YEAR AS ABS ON CAL.DAY_ORDER_DATE = ABS.ATTENDENCE_DATE
                                    AND ABS.STUDENT_ID = CLS.STUDENT_ID
                                    LEFT JOIN
                                SUP_REASON AS SR ON SR.REASON_ID = ABS.REASON_ID
                                    LEFT JOIN
                                SUP_ATTENDANCE_TYPE AS ATT ON ATT.ATTENDANCE_TYPE_ID = ABS.ATTENDANCE_TYPE_ID
                            WHERE
                                DAY_ORDER_DATE =?ATTENDANCE_DATE
                                    AND CAL.IS_DELETED != 1
                            ORDER BY SP.ROLL_NO , FIRST_NAME;";
                        break;
                    }
                case AttendanceSQLCommands.FetchStudentListForOverAllAttendance:
                    {

                        query = @"SELECT 
                                SP.ROLL_NO,
                                CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                        IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                CR.COURSE_CODE,
                                TT.COURSE_ID,
                                CR.COURSE_TITLE,
                                CLS.CLASS_ID AS S_CLASS_ID,
                                CLS.CLASS_ID,
                                ABS.ATTENDANCE_ID,
                                ABS.REASON_ID,
                                ABS.ATTENDANCE_TYPE_ID,
                                ABS.ENTRY_ID,
                                ABS.ENTRY_DATE,
                                SR.REASON_ID,
                                ATT.ATTENDANCE_TYPE_ID,SP.STUDENT_ID
                            FROM
                                CALENDER_?AC_YEAR AS CAL
                                    INNER JOIN
                                TT_TIMETABLE_?AC_YEAR AS TT ON TT.DAY_ORDER_ID = CAL.DAY_ORDER                           
                                    AND TT.STAFF_ID = ?STAFF_ID
                                    AND TT.HOUR_ID = ?HOUR_ID
                                    INNER JOIN
                                stu_class AS CLS ON CLS.CLASS_ID = tt.class_id AND CLS.ACADEMIC_YEAR=?AC_YEAR
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CLS.STUDENT_ID
                                    INNER JOIN
                                CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = TT.COURSE_ID
                                    LEFT JOIN
                                ATT_ABSENTEE_LIST_?AC_YEAR AS ABS ON CAL.DAY_ORDER_DATE = ABS.ATTENDENCE_DATE
                                    AND ABS.STUDENT_ID = CLS.STUDENT_ID
                                    LEFT JOIN
                                SUP_REASON AS SR ON SR.REASON_ID = ABS.REASON_ID
                                    LEFT JOIN
                                SUP_ATTENDANCE_TYPE AS ATT ON ATT.ATTENDANCE_TYPE_ID = ABS.ATTENDANCE_TYPE_ID
                            WHERE
                                DAY_ORDER_DATE = ?ATTENDANCE_DATE
                                    AND CAL.IS_DELETED != 1
                            ORDER BY SP.ROLL_NO , FIRST_NAME;";
                        break;
                    }
                case AttendanceSQLCommands.FetchOptionalCourseStudentListForOverAllAttendance:
                    {

                        query = @"SELECT 
                                        SP.STUDENT_ID,
                                        SP.ROLL_NO,
                                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                        CR.COURSE_CODE,
                                        TT.COURSE_ID,
                                        CR.COURSE_TITLE,
                                        CLS.S_CLASS_ID,
                                        CLS.CLASS_ID,
                                        ABS.ATTENDANCE_ID,
                                        ABS.REASON_ID,
                                        ABS.ATTENDANCE_TYPE_ID,
                                        ABS.ENTRY_ID,
                                        ABS.ENTRY_DATE,
                                        SR.REASON_ID,
                                        ATT.ATTENDANCE_TYPE_ID
                                    FROM
                                        CALENDER_?AC_YEAR AS CAL
                                            INNER JOIN
                                        TT_TIMETABLE_?AC_YEAR AS TT ON TT.DAY_ORDER_ID = CAL.DAY_ORDER AND TT.STAFF_ID = ?STAFF_ID
                                            AND TT.HOUR_ID = ?HOUR_ID
                                            INNER JOIN
                                        STU_COURSE_INFO_?AC_YEAR AS CLS ON CLS.S_CLASS_ID = TT.CLASS_ID
                                            AND CLS.COURSE_ID = TT.COURSE_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CLS.STUDENT_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = TT.COURSE_ID
                                            LEFT JOIN
                                        ATT_ABSENTEE_LIST_?AC_YEAR AS ABS ON CAL.DAY_ORDER_DATE = ABS.ATTENDENCE_DATE
                                            AND ABS.STUDENT_ID = CLS.STUDENT_ID
                                            LEFT JOIN
                                        SUP_REASON AS SR ON SR.REASON_ID = ABS.REASON_ID
                                            LEFT JOIN
                                        SUP_ATTENDANCE_TYPE AS ATT ON ATT.ATTENDANCE_TYPE_ID = ABS.ATTENDANCE_TYPE_ID
                                    WHERE
                                        DAY_ORDER_DATE = ?ATTENDANCE_DATE
                                            AND CAL.IS_DELETED != 1
                                            AND TT.IS_DELETED != 1
                                            AND CLS.IS_DELETED != 1
                                            AND CR.IS_DELETED != 1
                                    ORDER BY SP.ROLL_NO , FIRST_NAME;";
                        break;
                    }
                case AttendanceSQLCommands.FetchHoursByStaffIdAndDayOrder:
                    {
                        query = @"SELECT 
                                    HOUR_ID,COURSE_ID
                                FROM
                                    TT_TIMETABLE_?AC_YEAR AS TT
                                WHERE
                                    STAFF_ID =?STAFF_ID AND DAY_ORDER_ID =(select DAY_ORDER from CALENDER_?AC_YEAR where DAY_ORDER_DATE=?ATTENDANCE_DATE) AND CLASS_ID=?CLASS_ID
                                        AND IS_DELETED != 1;";
                        break;
                    }
                case AttendanceSQLCommands.FetchAttendanceStatusByClassId:
                    {
                        query = @"SELECT 
                                        SP.STUDENT_ID,
                                        SP.ROLL_NO,
                                        SP.REGISTER_NO,
                                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                        ROUND((((SELECT 
                                                        (COUNT(DAY_ORDER_DATE) * (SELECT 
                                                                    TTS.NO_OF_HOURS
                                                                FROM
                                                                    TT_DEPARTMENT_WISE_TEMPLATE AS TTD
                                                                        INNER JOIN
                                                                    TT_TEMPLATE_SETTING AS TTS ON TTS.SETTING_ID = TTD.SETTING_ID
                                                                WHERE
                                                                    DEPARTMENT_ID = (SELECT 
                                                                            DEPARTMENT
                                                                        FROM
                                                                            CP_CLASSES_?AC_YEAR
                                                                        WHERE
                                                                            CLASS_ID = ?CLASS_ID)
                                                                        AND TTD.ACADEMIC_YEAR = ?AC_YEAR
                                                                        AND TTD.IS_DELETED != 1)) AS TOTAL_HOURS
                                                    FROM
                                                        CALENDER_?AC_YEAR AS CAL
                                                    WHERE
                                                        CAL.DAY_ORDER_DATE BETWEEN (SELECT 
                                                                ASE.DATE_FROM
                                                            FROM
                                                                ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                                            WHERE
                                                                ASE.IS_ACTIVE = 1
                                                                    AND ASE.PROGRAMME = SP.PROGRAM_ID
                                                                    AND ASE.BATCH = SP.BATCH
                                                                    AND ASE.IS_DELETED != 1) AND CURDATE()
                                                            AND ((SELECT 
                                                                ASE.DATE_TO
                                                            FROM
                                                                ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                                            WHERE
                                                                ASE.IS_ACTIVE = 1
                                                                    AND ASE.PROGRAMME = SP.PROGRAM_ID
                                                                    AND ASE.BATCH = SP.BATCH
                                                                    AND ASE.IS_DELETED != 1)) <= CURDATE()
                                                            AND CAL.IS_HOLIDAY != 1) - (SELECT 
                                                        COUNT(ATT.ATTENDANCE_ID)
                                                    FROM
                                                        ATT_ABSENTEE_LIST_?AC_YEAR AS ATT
                                                    WHERE
                                                        STUDENT_ID = SC.STUDENT_ID)) / (SELECT 
                                                        (COUNT(DAY_ORDER_DATE) * (SELECT 
                                                                    TTS.NO_OF_HOURS
                                                                FROM
                                                                    TT_DEPARTMENT_WISE_TEMPLATE AS TTD
                                                                        INNER JOIN
                                                                    TT_TEMPLATE_SETTING AS TTS ON TTS.SETTING_ID = TTD.SETTING_ID
                                                                WHERE
                                                                    DEPARTMENT_ID = (SELECT 
                                                                            DEPARTMENT
                                                                        FROM
                                                                            CP_CLASSES_?AC_YEAR
                                                                        WHERE
                                                                            CLASS_ID = ?CLASS_ID)
                                                                        AND TTD.ACADEMIC_YEAR = ?AC_YEAR
                                                                        AND TTD.IS_DELETED != 1)) AS TOTAL_HOURS
                                                    FROM
                                                        CALENDER_?AC_YEAR AS CAL
                                                    WHERE
                                                        CAL.DAY_ORDER_DATE BETWEEN (SELECT 
                                                                ASE.DATE_FROM
                                                            FROM
                                                                ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                                            WHERE
                                                                ASE.IS_ACTIVE = 1
                                                                    AND ASE.PROGRAMME = SP.PROGRAM_ID
                                                                    AND ASE.BATCH = SP.BATCH
                                                                    AND ASE.IS_DELETED != 1) AND CURDATE()
                                                            AND ((SELECT 
                                                                ASE.DATE_TO
                                                            FROM
                                                                ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                                            WHERE
                                                                ASE.IS_ACTIVE = 1
                                                                    AND ASE.PROGRAMME = SP.PROGRAM_ID
                                                                    AND ASE.BATCH = SP.BATCH
                                                                    AND ASE.IS_DELETED != 1)) <= CURDATE()
                                                            AND CAL.IS_HOLIDAY != 1) * 100)) AS PERCENTAGE
                                    FROM
                                        STU_CLASS AS SC
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                    WHERE
                                        SC.CLASS_ID = ?CLASS_ID AND SC.IS_DELETED != 1
                                            AND SP.IS_LEFT != 1
                                    ORDER BY SP.STUDENT_ID;";
                        break;
                    }
                case AttendanceSQLCommands.FetchTotalHoursByClassIdAndCourseId:
                    {
                        query = @"SELECT 
                                        CCR.COURSE_ROOT_ID AS COURSE_ID,
                                        (SELECT 
                                                COUNT(TT.COURSE_ID)
                                            FROM
                                                TT_TIMETABLE_?AC_YEAR AS TT
                                                    INNER JOIN
                                                SUP_DAY_ORDER AS SDO ON SDO.DAY_ORDER_ID = TT.DAY_ORDER_ID
                                                    INNER JOIN
                                                CALENDER_?AC_YEAR AS CAL ON CAL.DAY_ORDER = SDO.DAY_ORDER_ID
                                            WHERE
                                                TT.CLASS_ID = ?CLASS_ID AND TT.COURSE_ID = ?COURSE_ID
                                                    AND CAL.DAY_ORDER_DATE BETWEEN ?DATE_FROM AND CURDATE()
                                                    AND ?DATE_TO <= CURDATE()
                                                    AND TT.IS_DELETED != 1) AS TOTAL_HOURS
                                    FROM
                                        STU_CLASS AS SC
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            AND SP.IS_LEFT != 1
                                            AND SP.IS_DELETED != 1
                                            INNER JOIN
                                        CP_CLASS_COURSE_?AC_YEAR AS CCC ON CCC.CLASS_ID = SC.CLASS_ID
                                            AND CCC.IS_DELETED != 1
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID = CCC.COURSE_ID
                                            AND CCR.IS_DELETED != 1
                                    WHERE
                                        SC.CLASS_ID = ?CLASS_ID
                                            AND CCR.COURSE_ROOT_ID = ?COURSE_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            AND SC.IS_DELETED != 1
                                    GROUP BY SC.CLASS_ID 
                                    UNION SELECT 
                                        CCR.COURSE_ROOT_ID AS COURSE_ID,
                                        (SELECT 
                                                COUNT(TT.COURSE_ID)
                                            FROM
                                                TT_TIMETABLE_?AC_YEAR AS TT
                                                    INNER JOIN
                                                SUP_DAY_ORDER AS SDO ON SDO.DAY_ORDER_ID = TT.DAY_ORDER_ID
                                                    INNER JOIN
                                                CALENDER_?AC_YEAR AS CAL ON CAL.DAY_ORDER = SDO.DAY_ORDER_ID
                                            WHERE
                                                TT.CLASS_ID = ?CLASS_ID AND TT.COURSE_ID = ?COURSE_ID
                                                    AND CAL.DAY_ORDER_DATE BETWEEN ?DATE_FROM AND CURDATE()
                                                    AND ?DATE_TO <= CURDATE()
                                                    AND TT.IS_DELETED != 1) AS TOTAL_HOURS
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR AS SC
                                            INNER JOIN
                                        STU_CLASS AS SCL ON SCL.STUDENT_ID = SC.STUDENT_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SCL.STUDENT_ID
                                            AND SP.IS_LEFT != 1
                                            AND SP.IS_DELETED != 1
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID = SC.COURSE_ID
                                            AND CCR.IS_DELETED != 1
                                    WHERE
                                        SC.S_CLASS_ID = ?CLASS_ID
                                            AND CCR.COURSE_ROOT_ID = ?COURSE_ID
                                            AND SC.IS_DELETED != 1
                                    GROUP BY SC.CLASS_ID;";
                        break;
                    }
                case AttendanceSQLCommands.FetchTotalAbsentHoursByClassIdAndCourseId:
                    {
                        query = @"SELECT 
                                        SP.STUDENT_ID,
                                        SP.REGISTER_NO,
                                        CCR.COURSE_ROOT_ID AS COURSE_ID,
                                        (SELECT 
                                                                    COUNT(CAL.DAY_ORDER_DATE)
                                                                FROM
                                                                    CALENDER_?AC_YEAR AS CAL
                                                                        INNER JOIN
                                                                    ATT_ABSENTEE_LIST_?AC_YEAR AS ATT ON ATT.ATTENDENCE_DATE = CAL.DAY_ORDER_DATE
                                                                WHERE
                                                                    CAL.IS_HOLIDAY != 1
                                                                        AND CAL.DAY_ORDER_DATE BETWEEN ?DATE_FROM AND CURDATE()
                                                                        AND ?DATE_TO <= CURDATE()
                                                                        AND ATT.COURSE_ID = ?COURSE_ID
                                                                        AND ATT.S_CLASS_ID = ?CLASS_ID
                                                                        AND ATT.STUDENT_ID = SP.STUDENT_ID AND ATT.ATTENDANCE_TYPE_ID!=1) ABSENT_HOURS
                                    FROM
                                        STU_CLASS AS SC
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            AND SP.IS_LEFT != 1
                                            AND SP.IS_DELETED != 1
                                            INNER JOIN
                                        CP_CLASS_COURSE_?AC_YEAR AS CCC ON CCC.CLASS_ID = SC.CLASS_ID
                                            AND CCC.IS_DELETED != 1
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID = CCC.COURSE_ID
                                            AND CCR.IS_DELETED != 1
                                    WHERE
                                        SC.CLASS_ID = ?CLASS_ID
                                            AND CCR.COURSE_ROOT_ID = ?COURSE_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            AND SC.IS_DELETED != 1
                                    UNION SELECT 
                                        SP.STUDENT_ID,
                                        SP.REGISTER_NO,
                                        CCR.COURSE_ROOT_ID AS COURSE_ID,
                                        (SELECT 
                                                                    COUNT(CAL.DAY_ORDER_DATE)
                                                                FROM
                                                                    CALENDER_?AC_YEAR AS CAL
                                                                        INNER JOIN
                                                                    ATT_ABSENTEE_LIST_?AC_YEAR AS ATT ON ATT.ATTENDENCE_DATE = CAL.DAY_ORDER_DATE
                                                                WHERE
                                                                    CAL.IS_HOLIDAY != 1
                                                                        AND CAL.DAY_ORDER_DATE BETWEEN ?DATE_FROM AND CURDATE()
                                                                        AND ?DATE_TO <= CURDATE()
                                                                        AND ATT.COURSE_ID = ?COURSE_ID
                                                                        AND ATT.S_CLASS_ID = ?CLASS_ID
                                                                        AND ATT.STUDENT_ID = SP.STUDENT_ID AND ATT.ATTENDANCE_TYPE_ID!=1) AS ABSENT_HOURS
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR AS SC
                                            INNER JOIN
                                        STU_CLASS AS SCL ON SCL.STUDENT_ID = SC.STUDENT_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SCL.STUDENT_ID
                                            AND SP.IS_LEFT != 1
                                            AND SP.IS_DELETED != 1
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID = SC.COURSE_ID
                                            AND CCR.IS_DELETED != 1
                                    WHERE
                                        SC.S_CLASS_ID = ?CLASS_ID
                                            AND CCR.COURSE_ROOT_ID = ?COURSE_ID
                                            AND SC.IS_DELETED != 1
                                    ORDER BY REGISTER_NO;";
                        break;
                    }
                case AttendanceSQLCommands.FetchdDayOrderByAttendanceDate:
                    {
                        query = @"SELECT 
                                        CAL.DAY_ORDER
                                    FROM
                                        CALENDER_?AC_YEAR AS CAL
                                    WHERE
                                        CAL.DAY_ORDER_DATE = ?DAY_ORDER_DATE
                                            AND CAL.IS_DELETED != 1
                                            AND CAL.IS_HOLIDAY != 1
                                            AND CAL.DAY_ORDER != 0;";
                        break;
                    }
                case AttendanceSQLCommands.FetchNoOfHours:
                    {
                        query = @"SELECT 
                                        TS.NO_OF_DAYS
                                    FROM
                                        TT_DEPARTMENT_WISE_TEMPLATE AS DWT
                                            INNER JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DT ON DT.DEPARTMENT_ID = DWT.DEPARTMENT_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.DEPARTMENT = DT.DEPARTMENT_ID
                                            INNER JOIN
                                        TT_TEMPLATE_SETTING AS TS ON TS.SETTING_ID = DWT.SETTING_ID
                                    WHERE
                                        CLS.CLASS_ID = ?S_CLASS_ID
                                            AND DWT.ACADEMIC_YEAR = ?AC_YEAR
                                            AND DWT.IS_DELETED != 1
                                            AND TS.IS_DELETED != 1;";
                        break;
                    }
                case AttendanceSQLCommands.IsAbsenteeEntryExisting:
                    {
                        query = @"SELECT 
                                        ATTENDANCE_ID
                                    FROM
                                        ATT_ABSENTEE_LIST_?AC_YEAR
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID AND CLASS_ID = ?CLASS_ID
                                            AND ATTENDENCE_DATE = ?ATTENDENCE_DATE
                                            AND HOUR_FROM = ?HOUR_FROM;";
                        break;
                    }
                case AttendanceSQLCommands.SaveAbsenteeEntryForSpecialAttendance:
                    {
                        query = @"INSERT INTO ATT_SPECIAL_ABSENTEE_LIST_?AC_YEAR
                                        (
                                        ATTENDENCE_DATE,
                                        HOUR_FROM,
                                        HOUR_TO,
                                        STUDENT_ID,
                                        CLASS_ID,
                                        REASON_ID,
                                        ENTRY_ID,
                                        ENTRY_DATE,
                                        S_CLASS_ID,
                                        ATTENDANCE_TYPE_ID,
                                        SPL_ATTENDANCE_TYPE_ID
                                        )
                                        VALUES
                                        (?ATTENDENCE_DATE,
                                        ?HOUR_FROM,
                                        ?HOUR_TO,
                                        ?STUDENT_ID,
                                        ?CLASS_ID,
                                        ?REASON_ID,
                                        ?ENTRY_ID,
                                        ?ENTRY_DATE,
                                        ?S_CLASS_ID,
                                        ?ATTENDANCE_TYPE_ID,
                                        ?SPL_ATTENDANCE_TYPE_ID);";
                        return query;
                    }
                case AttendanceSQLCommands.UpdateAbsenteeEntryForSpecialAttendance:
                    {
                        query = @"UPDATE ATT_SPECIAL_ABSENTEE_LIST_?AC_YEAR
                                    SET
                                    ATTENDENCE_DATE = ?ATTENDENCE_DATE,
                                    STAFF_ID = ?STAFF_ID,
                                    HOUR_FROM = ?HOUR_FROM,
                                    HOUR_TO = ?HOUR_TO,
                                    STUDENT_ID = ?STUDENT_ID,
                                    CLASS_ID = ?CLASS_ID,
                                    REASON_ID = ?REASON_ID,
                                    ENTRY_ID = ?ENTRY_ID,
                                    ENTRY_DATE = ?ENTRY_DATE,
                                    S_CLASS_ID = ?S_CLASS_ID,
                                    ATTENDANCE_TYPE_ID = ?ATTENDANCE_TYPE_ID,
                                    SPL_ATTENDANCE_TYPE_ID = ?SPL_ATTENDANCE_TYPE_ID
                                    WHERE ATTENDANCE_ID = ?ATTENDANCE_ID,;";
                        break;
                    }
                case AttendanceSQLCommands.FetchStudentListForAttendanceByCourseIdAndStaffIdForSpecialAttendance:
                    {
                        query = @"SELECT 
                                SP.ROLL_NO,
                                CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                        IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                SH.HOUR_TYPE_ID,
                                SH.HOURTYPE,
                                CLS.CLASS_ID AS S_CLASS_ID,
                                CLS.CLASS_ID,
                                ABS.ATTENDANCE_ID,
                                ABS.REASON_ID,
                                ABS.ATTENDANCE_TYPE_ID,
                                ABS.ENTRY_ID,
                                ABS.ENTRY_DATE,
                                SR.REASON_ID,
                                ATT.ATTENDANCE_TYPE_ID,SP.STUDENT_ID
                            FROM
                                CALENDER_?AC_YEAR AS CAL
                                    INNER JOIN
                                TT_TIMETABLE_?AC_YEAR AS TT ON TT.DAY_ORDER_ID = CAL.DAY_ORDER
                                    AND TT.STAFF_ID = ?STAFF_ID
                                    AND TT.HOUR_ID = ?HOUR_ID
                                    INNER JOIN
                                STU_CLASS AS CLS ON CLS.CLASS_ID = TT.CLASS_ID  AND CLS.ACADEMIC_YEAR=?AC_YEAR
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CLS.STUDENT_ID
                                    INNER JOIN
                                SUP_HOUR_TYPE AS SH ON SH.HOUR_TYPE_ID = TT.HOUR_TYPE
                                    LEFT JOIN
                                ATT_SPECIAL_ABSENTEE_LIST_?AC_YEAR AS ABS ON CAL.DAY_ORDER_DATE = ABS.ATTENDENCE_DATE
                                    AND ABS.STUDENT_ID = CLS.STUDENT_ID
                                    LEFT JOIN
                                SUP_REASON AS SR ON SR.REASON_ID = ABS.REASON_ID
                                    LEFT JOIN
                                SUP_ATTENDANCE_TYPE AS ATT ON ATT.ATTENDANCE_TYPE_ID = ABS.ATTENDANCE_TYPE_ID
                            WHERE
                                DAY_ORDER_DATE =?ATTENDENCE_DATE
                                    AND CAL.IS_DELETED != 1
                            ORDER BY SP.ROLL_NO , FIRST_NAME;";
                        break;
                    }
                case AttendanceSQLCommands.FetchStudentListForAttendanceByClassIdForSpecialAttendance:
                    {
                        query = @"SELECT 
                                SP.STUDENT_ID,
                                        SP.ROLL_NO,
                                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                        SCLS.CLASS_ID AS S_CLASS_ID,
                                        ABS.ATTENDANCE_ID,
                                        ABS.REASON_ID,
                                        ABS.ATTENDANCE_TYPE_ID,
                                        ABS.ENTRY_ID,
                                        ABS.ENTRY_DATE,
                                        SR.REASON_ID,
                                        ATT.ATTENDANCE_TYPE_ID,
                                        ABS.SPL_ATTENDANCE_TYPE_ID
                            FROM
                                STU_CLASS AS SCLS
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SCLS.STUDENT_ID
                                    LEFT JOIN
                                ATT_SPECIAL_ABSENTEE_LIST_?AC_YEAR AS ABS ON ABS.ATTENDENCE_DATE = ?ATTENDANCE_DATE
                                    AND SCLS.STUDENT_ID = ABS.STUDENT_ID
                                    LEFT JOIN
                                SUP_REASON AS SR ON SR.REASON_ID = ABS.REASON_ID
                                    LEFT JOIN
                                SUP_ATTENDANCE_TYPE AS ATT ON ATT.ATTENDANCE_TYPE_ID = ABS.ATTENDANCE_TYPE_ID
                            WHERE
                                SCLS.CLASS_ID = ?CLASS_ID
                                    AND SCLS.ACADEMIC_YEAR = ?AC_YEAR GROUP BY SCLS.STUDENT_ID;";
                        break;
                    }
                case AttendanceSQLCommands.FetchClassesByProgrammeId:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                '(',
                                                IFNULL(CONCAT(' SHIFT - ',SHIFT_NAME), ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                        CP_CLASSES_?AC_YEAR CLS INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=CLS.SHIFT
                                    WHERE
                                        CLS.IS_DELETED != 1 AND CLS.PROGRAMME=?PROGRAMME;";
                        break;
                    }
                case AttendanceSQLCommands.FetchStudentListByClassId:
                    {
                        query = @"SELECT 
                                    SP.STUDENT_ID,
                                    CONCAT(UPPER(FIRST_NAME), ' - ', UPPER(ROLL_NO)) AS 'FIRST_NAME'
                                FROM
                                    STU_CLASS AS SC
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                        AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                WHERE
                                    SC.CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case AttendanceSQLCommands.FetchAbsentListByStaffId:
                    {
                        query = @"SELECT 
                                    date_format(ATTENDENCE_DATE,'%d/%m/%Y')  as ATTENDENCE_DATE,
                                    CR.COURSE_TITLE,
                                    CR.COURSE_CODE,
                                    CLS.CLASS_NAME,
                                    SP.REGISTER_NO,
                                    SP.ROLL_NO,
                                    CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                            IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                    HOUR_FROM 
                                FROM
                                    ATT_ABSENTEE_LIST_?AC_YEAR AS ABS
                                        LEFT JOIN
                                    CP_COURSE_ROOT_?AC_YEAR  AS CR ON CR.COURSE_ROOT_ID = ABS.COURSE_ID
                                        LEFT JOIN
                                    SUP_ATTENDANCE_TYPE AS SAT ON SAT.ATTENDANCE_TYPE_ID = ABS.ATTENDANCE_TYPE_ID
                                        LEFT JOIN
                                    CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ABS.S_CLASS_ID
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = ABS.STUDENT_ID
                                WHERE
                                    ABS.STAFF_ID = ?STAFF_ID;";
                        break;
                    }
            }
            return query;
        }
    }
}
