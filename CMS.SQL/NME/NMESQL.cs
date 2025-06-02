using CMS.DAO;

namespace CMS.SQL.NME
{
    public static class NMESQL
    {
        public static string GetNMESQL(NMESQLCommands sEnumCommand)
        {
            string query = "";


            switch (sEnumCommand)
            {
                case NMESQLCommands.CheckNMEAvailability:
                    {
                        query = @"SELECT 
                                    (NC.QUOTA > (SELECT 
                                            COUNT(*) AS CC
                                        FROM
                                            NME_STUDENT_REGISTRATION_?AC_YEAR AS SR
                                        WHERE
                                            SR.CLASS_ID = ?CLASS_ID AND SR.COURSE_ID = ?COURSE_ID
                                                AND SR.SETTINGS_ID = ?SETTINGS_ID)) AS COURSEAVAILABILITY
                                FROM
                                  NME_CLASS_COURSE_QUOTA_?AC_YEAR AS NC
                                WHERE
                                    NC.CLASS_ID = ?CLASS_ID AND NC.COURSE_ID =?COURSE_ID
                                        AND NC.SETTINGS_ID =?SETTINGS_ID;";
                        break;
                    }
                case NMESQLCommands.LoadNMECourses:
                    {
                        query = @"SELECT 
                                    NC.CLASS_ID,
                                    NC.COURSE_ID,
                                    NC.SETTINGS_ID,
                                    CR.COURSE_TITLE,
                                    CR.SEMESTER_ID,
                                    cls.CLASS_ID as NME_CLASS
                                FROM
                                    nme_class_course_quota_?AC_YEAR AS NC
                                        INNER JOIN
                                    cp_course_root_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = NC.COURSE_ID
                                        INNER JOIN
                                    cp_classes_?AC_YEAR AS cls ON cls.COURSE_ID = NC.COURSE_ID
                                        AND cls.SHIFT = ?SHIFT_ID
                                WHERE
                                    NC.SETTINGS_ID = (SELECT 
                                            SETTINGS_ID
                                        FROM
                                            nme_settings ST
                                        WHERE
                                            ST.CLASS_ID = ?CLASS_ID)
                                        AND NC.QUOTA > (SELECT 
                                            COUNT(*) AS CC
                                        FROM
                                            nme_student_registration_?AC_YEAR AS SR
                                        WHERE
                                            SR.CLASS_ID = NC.CLASS_ID
                                                AND SR.COURSE_ID = NC.COURSE_ID);";
                        break;
                    }
                case NMESQLCommands.RegisterNMECourse:
                    {
                        query = @"INSERT INTO `NME_STUDENT_REGISTRATION_?AC_YEAR`
                                    (
                                    `STUDENT_ID`,
                                    `CLASS_ID`,
                                    `COURSE_ID`,
                                    `TIME_REGISTERED`,
                                    `SETTINGS_ID`,
                                    `SEMESTER`,`S_CLASS_ID`)
                                    VALUES
                                    (?STUDENT_ID,?CLASS_ID,?COURSE_ID,CURDATE(),?SETTINGS_ID,?SEMESTER,?S_CLASS_ID);";
                        break;
                    }
                case NMESQLCommands.ValidateStudent:
                    {
                        query = @"SELECT 
                                    COUNT(*) > 0 AS 'Status'
                                FROM
                                    nme_student_registration_?AC_YEAR
                                WHERE
                                    STUDENT_ID = ?STUDENT_ID;";


                        break;
                    }

                case NMESQLCommands.GetNMEClassListIDs:
                    {

                        query = @"SELECT group_concat(distinct COURSE_ID) FROM nme_class_course_?AC_YEAR";
                        break;
                    }
                case NMESQLCommands.FetchNMEClassList:
                    {

                        query = @"SELECT 
                                        CLASS_ID, CLASS_NAME
                                    FROM
                                        cp_classes_?AC_YEAR CP
                                    WHERE
                                        CP.COURSE_ID IN (?COURSE_ID)";
                        break;
                    }
                case NMESQLCommands.FetchNMEStudentList:
                    {
                        query = @"SELECT 
                                    SP.ADMISSION_NO,
                                    SP.REGISTER_NO,
                                    CONCAT(IF(SP.FIRST_NAME IS NULL,
                                                '',
                                                SP.FIRST_NAME),
                                            ' ',
                                            IF(SP.LAST_NAME IS NULL,
                                                '',
                                                SP.LAST_NAME)) AS FIRST_NAME,
                                    CR.COURSE_TITLE,
                                    CLS1.CLASS_NAME 
                                FROM
                                    NME_STUDENT_REGISTRATION_?AC_YEAR AS NSR
                                        INNER JOIN
                                    CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = NSR.CLASS_ID
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = NSR.STUDENT_ID
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = NSR.COURSE_ID
                                    INNER JOIN cp_classes_?AC_YEAR AS CLS1 ON CLS1.CLASS_ID=NSR.CLASS_ID
                                WHERE
                                    NSR.CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMESettingsList:
                    {
                        query = @"SELECT 
                                        SS.SETTINGS_ID,
                                        SS.SETTINGS_NAME,
                                        CLS.CLASS_CODE,
                                        DATE_FORMAT(DATE_FROM, '%d/%m/%Y') AS DATE_FROM,
                                        DATE_FORMAT(DATE_TO, '%d/%m/%Y') AS DATE_TO,
                                        SS.ACADEMIC_YEAR,
                                        SEM.SEMESTER_NAME,
                                        SS.IS_ALLOWED
                                    FROM
                                        NME_SETTINGS AS SS
                                            LEFT JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = SS.CLASS_ID
                                            LEFT JOIN
                                        SUP_SEMESTER AS SEM ON SEM.SUP_SEMESTER_ID = SS.SEMESTER WHERE SS.IS_DELETED!=1;";
                        break;
                    }
                case NMESQLCommands.IsNMESettingExisting:
                    {

                        query = @"SELECT 
                                        SS.SETTINGS_ID
                                    FROM
                                        NME_SETTINGS AS SS
                                            LEFT JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = SS.CLASS_ID
                                            LEFT JOIN
                                        SUP_SEMESTER AS SEM ON SEM.SUP_SEMESTER_ID = SS.SEMESTER WHERE SS.SEMESTER=?SEMESTER AND SS.CLASS_ID=?CLASS_ID;";
                        break;
                    }
                case NMESQLCommands.SaveNMESettings:
                    {

                        query = @"INSERT INTO NME_SETTINGS
                                                (
                                                SETTINGS_NAME,
                                                CLASS_ID,
                                                DATE_FROM,
                                                DATE_TO,
                                                ACADEMIC_YEAR,
                                                IS_ALLOWED,
                                                SEMESTER)
                                                VALUES
                                                (
                                                ?SETTINGS_NAME,
                                                ?CLASS_ID,
                                                ?DATE_FROM,
                                                ?DATE_TO,
                                                ?ACADEMIC_YEAR,
                                                ?IS_ALLOWED,
                                                ?SEMESTER);";
                        break;
                    }
                case NMESQLCommands.FetchNMESettingsById:
                    {
                        query = @"SELECT 
                                        NS.SETTINGS_ID,
                                        NS.SETTINGS_NAME,
                                        NS.CLASS_ID,
                                        DATE_FORMAT(NS.DATE_FROM, '%d/%m/%Y') AS DATE_FROM,
                                        DATE_FORMAT(NS.DATE_TO, '%d/%m/%Y') AS DATE_TO,
                                        NS.ACADEMIC_YEAR,
                                        NS.SEMESTER,
                                        CL.SHIFT
                                    FROM
                                        NME_SETTINGS AS NS INNER JOIN CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID=NS.CLASS_ID WHERE NS.SETTINGS_ID=?SETTINGS_ID;";


                        break;
                    }
                case NMESQLCommands.UpdateNMESettings:
                    {
                        query = @"UPDATE NME_SETTINGS
                                                    SET
                                                    SETTINGS_NAME=?SETTINGS_NAME,
                                                    CLASS_ID=?CLASS_ID,
                                                    DATE_FROM=?DATE_FROM,
                                                    DATE_TO=?DATE_TO,
                                                    ACADEMIC_YEAR=?ACADEMIC_YEAR,
                                                    SEMESTER=?SEMESTER
                                                    WHERE SETTINGS_ID=?SETTINGS_ID;";


                        break;
                    }
                case NMESQLCommands.DeleteNMESettings:
                    {

                        query = @"UPDATE NME_SETTINGS
                                                    SET
                                                    IS_DELETED=1
                                                    WHERE SETTINGS_ID=?SETTINGS_ID;";
                        break;
                    }
                case NMESQLCommands.NMESettingsRegistrationClose:
                    {

                        query = @"UPDATE NME_SETTINGS
                                                    SET
                                                    IS_ALLOWED=0
                                                    WHERE SETTINGS_ID=?SETTINGS_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMEClassCourseList:
                    {

                        query = @"SELECT 
                                        CC.NME_CLASS_COURSE_ID,
                                        RT.COURSE_CODE,
                                        RT.COURSE_TITLE,
                                        ST.SETTINGS_NAME,
                                        CL.CLASS_CODE,
                                        CQ.QUOTA
                                    FROM
                                        NME_CLASS_COURSE_?AC_YEAR AS CC
                                            LEFT JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID = CC.COURSE_ID
                                            LEFT JOIN
                                        NME_SETTINGS AS ST ON ST.SETTINGS_ID = CC.SETTINGS_ID
                                            LEFT JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = CC.CLASS_ID
                                            LEFT JOIN
                                        NME_CLASS_COURSE_QUOTA_?AC_YEAR AS CQ ON CQ.CLASS_ID = CC.CLASS_ID
                                            AND CQ.COURSE_ID = CC.COURSE_ID
                                            AND CQ.SETTINGS_ID = CC.SETTINGS_ID
                                    WHERE
                                        CC.IS_DELETED = 0";
                        break;
                    }
                case NMESQLCommands.FetchNMECourseAllotment:
                    {

                        query = @"SELECT 
                                        CA.ALLOTMENT_ID,SH.SHIFT_NAME,RT.COURSE_CODE,RT.COURSE_TITLE,CA.ALLOTED_SEATS,CA.ACADEMIC_YEAR
                                    FROM
                                        NME_COURSE_ALLOTMENT_?AC_YEAR CA
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID = CA.COURSE_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = CA.SHIFT_ID
                                    WHERE
                                        CA.IS_DELETED = 0;";
                        break;
                    }
                case NMESQLCommands.IsNMECourseAllotmentExisting:
                    {

                        query = @"SELECT 
                                        CA.ALLOTMENT_ID
                                    FROM
                                        NME_COURSE_ALLOTMENT_?AC_YEAR CA
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID = CA.COURSE_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = CA.SHIFT_ID
                                    WHERE
                                        CA.IS_DELETED = 0 AND CA.COURSE_ID=?COURSE_ID AND CA.SHIFT_ID=?SHIFT_ID;";
                        break;
                    }
                case NMESQLCommands.SaveNMECourseAllotment:
                    {

                        query = @"INSERT INTO NME_COURSE_ALLOTMENT_?AC_YEAR
                                            (
                                            COURSE_ID,
                                            SHIFT_ID,
                                            ALLOTED_SEATS,
                                            ACADEMIC_YEAR)
                                            VALUES
                                            (
                                            ?COURSE_ID,
                                            ?SHIFT_ID,
                                            ?ALLOTED_SEATS,
                                            ?ACADEMIC_YEAR);";
                        break;
                    }
                case NMESQLCommands.FetchNMECourseAllotmentById:
                    {
                        query = @"SELECT 
                                        ALLOTMENT_ID,
                                        COURSE_ID,
                                        SHIFT_ID,
                                        ALLOTED_SEATS,
                                        ACADEMIC_YEAR
                                    FROM
                                        NME_COURSE_ALLOTMENT_?AC_YEAR
                                    WHERE
                                        ALLOTMENT_ID = ?ALLOTMENT_ID;";
                        break;
                    }
                case NMESQLCommands.UpdateNMECourseAllotment:
                    {
                        query = @"UPDATE NME_COURSE_ALLOTMENT_?AC_YEAR
                                            SET
                                            ALLOTMENT_ID=?ALLOTMENT_ID,
                                            COURSE_ID=?COURSE_ID,
                                            SHIFT_ID=?SHIFT_ID,
                                            ALLOTED_SEATS=?ALLOTED_SEATS,
                                            ACADEMIC_YEAR=?ACADEMIC_YEAR
                                            WHERE ALLOTMENT_ID=?ALLOTMENT_ID;";
                        break;
                    }
                case NMESQLCommands.DeleteNMECourseAllotment:
                    {

                        query = @"UPDATE NME_COURSE_ALLOTMENT_?AC_YEAR
                                                    SET
                                                    IS_DELETED=1
                                                    WHERE ALLOTMENT_ID=?ALLOTMENT_ID;";
                        break;
                    }
                case NMESQLCommands.GetSelectedNMECourseId:
                    {
                        query = @"SELECT 
                                        CC.NME_CLASS_COURSE_ID,CL.CLASS_ID,CL.CLASS_CODE,ST.SETTINGS_NAME,CC.COURSE_ID,CONCAT(IFNULL(RT.COURSE_TITLE, ''),
                                            '(',
                                            IFNULL(RT.COURSE_CODE, ''),
                                            ')') AS COURSE_CODE,IF(CC.NME_CLASS_COURSE_ID IS NULL,
                                                                            '',
                                                                            'SELECTED') AS SELECTED
                                    FROM
                                        NME_CLASS_COURSE_?AC_YEAR AS CC
                                            LEFT JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = CC.CLASS_ID
                                            LEFT JOIN
                                        NME_SETTINGS AS ST ON ST.SETTINGS_ID = CC.SETTINGS_ID
		                                    LEFT JOIN
	                                    CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID=CC.COURSE_ID
                                    WHERE
                                        CC.SETTINGS_ID = ?SETTINGS_ID;";
                        break;
                    }
                case NMESQLCommands.GetSelectedNMEClassId:
                    {
                        query = @"SELECT 
                                        CC.NME_CLASS_COURSE_ID,CL.CLASS_ID,CL.CLASS_CODE,ST.SETTINGS_NAME,CC.COURSE_ID,CONCAT(IFNULL(RT.COURSE_TITLE, ''),
                                            '(',
                                            IFNULL(RT.COURSE_CODE, ''),
                                            ')') AS COURSE_CODE,IF(CC.NME_CLASS_COURSE_ID IS NULL,
                                                                            '',
                                                                            'SELECTED') AS SELECTED
                                    FROM
                                        NME_CLASS_COURSE_?AC_YEAR AS CC
                                            LEFT JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = CC.CLASS_ID
                                            LEFT JOIN
                                        NME_SETTINGS AS ST ON ST.SETTINGS_ID = CC.SETTINGS_ID
		                                    LEFT JOIN
	                                    CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID=CC.COURSE_ID
                                    WHERE
                                        CC.SETTINGS_ID = ?SETTINGS_ID GROUP BY CL.CLASS_ID;";
                        break;
                    }
                case NMESQLCommands.IsNMEClassCourseExisting:
                    {
                        query = @"SELECT 
                                        NME_CLASS_COURSE_ID
                                    FROM
                                        NME_CLASS_COURSE_?AC_YEAR
                                    WHERE
                                        COURSE_ID = ?COURSE_ID AND SETTINGS_ID = ?SETTINGS_ID
                                            AND CLASS_ID = ?CLASS_ID AND IS_DELETED!=1;";
                        break;
                    }
                case NMESQLCommands.SaveNMEClassCourse:
                    {

                        query = @"INSERT INTO NME_CLASS_COURSE_?AC_YEAR
                                            (
                                            COURSE_ID,
                                            SETTINGS_ID,
                                            CLASS_ID)
                                            VALUES
                                            (
                                            ?COURSE_ID,
                                            ?SETTINGS_ID,
                                            ?CLASS_ID);";
                        break;
                    }
                case NMESQLCommands.UpdateNMEClassCourse:
                    {

                        query = @"UPDATE NME_CLASS_COURSE_?AC_YEAR
                                            SET
                                            COURSE_ID=?COURSE_ID,
                                            SETTINGS_ID=?SETTINGS_ID,
                                            CLASS_ID=?CLASS_ID
                                            WHERE NME_CLASS_COURSE_ID = ?NME_CLASS_COURSE_ID;";
                        break;
                    }
                case NMESQLCommands.DeleteNMEClassCourse:
                    {

                        query = @"UPDATE NME_CLASS_COURSE_?AC_YEAR
                                                    SET
                                                    IS_DELETED=1
                                                    WHERE NME_CLASS_COURSE_ID=?NME_CLASS_COURSE_ID;";
                        break;
                    }
                case NMESQLCommands.DeleteNMEClassCourseQuotaFromClassCourse:
                    {

                        query = @"UPDATE NME_CLASS_COURSE_QUOTA_?AC_YEAR
                                                    SET
                                                    IS_DELETED=1
                                                    WHERE NME_CLASS_COURSE_QUOTA_ID=?NME_CLASS_COURSE_QUOTA_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMEClassCourseById:
                    {

                        query = @"SELECT 
                                        COURSE_ID,
                                        SETTINGS_ID,
                                        CLASS_ID
                                    FROM
                                        NME_CLASS_COURSE_?AC_YEAR
                                    WHERE
                                        NME_CLASS_COURSE_ID = ?NME_CLASS_COURSE_ID;";
                        break;
                    }
                case NMESQLCommands.FetchClassCourseQuotaByClassIdAndCourseIdAndSettingId:
                    {

                        query = @"SELECT 
                                        NME_CLASS_COURSE_QUOTA_ID
                                    FROM
                                        NME_CLASS_COURSE_QUOTA_?AC_YEAR
                                    WHERE
                                        CLASS_ID = ?CLASS_ID AND COURSE_ID = ?COURSE_ID
                                            AND SETTINGS_ID = ?SETTINGS_ID;";
                        break;
                    }
                case NMESQLCommands.SaveNMEClassCourseQuota:
                    {

                        query = @"INSERT INTO NME_CLASS_COURSE_QUOTA_?AC_YEAR
                                            (
                                            COURSE_ID,
                                            SETTINGS_ID,
                                            CLASS_ID)
                                            VALUES
                                            (
                                            ?COURSE_ID,
                                            ?SETTINGS_ID,
                                            ?CLASS_ID);";
                        break;
                    }
                case NMESQLCommands.BindNMEClassCourseQuota:
                    {
                        query = @"SELECT 
                                COURSE_ROOT_ID,
                                COURSE_CODE,
                                COURSE_TITLE,
                                (SELECT 
                                        ALLOTED_SEATS
                                    FROM
                                        NME_COURSE_ALLOTMENT_?AC_YEAR AS NA
                                            INNER JOIN
                                        SUP_SHIFT AS SS ON SS.SHIFT_ID = NA.SHIFT_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.SHIFT = SS.SHIFT_ID
                                    WHERE
                                        NA.COURSE_ID = CT.COURSE_ROOT_ID
                                            AND SS.SHIFT_ID = CLS.SHIFT
                                            AND CLS.CLASS_ID = ?CLASS_ID) AS TOTALQUOTA,
                                (SELECT 
                                        SUM(QUOTA)
                                    FROM
                                        NME_CLASS_COURSE_QUOTA_?AC_YEAR AS CQ
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = CQ.CLASS_ID
                                    WHERE
                                        CQ.COURSE_ID = CT.COURSE_ROOT_ID
                                            AND CL.CLASS_ID = ?CLASS_ID) AS CLASSQUOTA,
                                (SELECT 
                                        SUM(QUOTA)
                                    FROM
                                        NME_CLASS_COURSE_QUOTA_?AC_YEAR AS CQ
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = CQ.CLASS_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SS ON SS.SHIFT_ID = CL.SHIFT
                                    WHERE
                                        CQ.COURSE_ID = CT.COURSE_ROOT_ID
                                            AND SS.SHIFT_ID = CLS.SHIFT) AS SUMOFQUOTA,
                                (SELECT 
                                        SUM(QUOTA)
                                    FROM
                                        NME_CLASS_COURSE_QUOTA_?AC_YEAR AS CQ
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = CQ.CLASS_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SS ON SS.SHIFT_ID = CL.SHIFT
                                    WHERE
                                        CQ.COURSE_ID = CT.COURSE_ROOT_ID
                                            AND SS.SHIFT_ID = CLS.SHIFT)-   (SELECT 
                                        SUM(QUOTA)
                                    FROM
                                        NME_CLASS_COURSE_QUOTA_?AC_YEAR AS CQ
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = CQ.CLASS_ID
                                    WHERE
                                        CQ.COURSE_ID = CT.COURSE_ROOT_ID
                                            AND CL.CLASS_ID = ?CLASS_ID) AS REMAININGQUOTA
                            FROM
                                CP_COURSE_ROOT_?AC_YEAR AS CT
                                    LEFT JOIN
                                NME_CLASS_COURSE_QUOTA_?AC_YEAR AS NQ ON NQ.COURSE_ID = CT.COURSE_ROOT_ID
                                    LEFT JOIN
                                CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = NQ.CLASS_ID
                            WHERE
                                CT.COURSE_ROOT_ID IN (?COURSE_ROOT_ID)
                            GROUP BY COURSE_ROOT_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMECourseRegistration:
                    {
                        query = @"SELECT 
                                        NR.REGISTRATION_ID,
                                        CT.COURSE_TYPE,
                                        CR.COURSE_CODE,
                                        CR.COURSE_TITLE,
                                        SS.SEMESTER_NAME
                                    FROM
                                        NME_COURSE_REGISTRATION_?AC_YEAR AS NR
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = NR.COURSE_ID
                                            INNER JOIN
                                        CP_COURSE_TYPE_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID = NR.COURSE_TYPE
                                            INNER JOIN
                                        SUP_SEMESTER AS SS ON SS.SUP_SEMESTER_ID = CR.SEMESTER_ID WHERE NR.IS_DELETED!=1;";
                        break;
                    }
                case NMESQLCommands.IsNMECourseRegistrationExisting:
                    {

                        query = @"SELECT 
                                        REGISTRATION_ID
                                    FROM
                                        NME_COURSE_REGISTRATION_?AC_YEAR
                                    WHERE
                                        COURSE_ID = ?COURSE_ID AND IS_DELETED!=1;";
                        break;
                    }
                case NMESQLCommands.SaveNMECourseRegistration:
                    {

                        query = @"INSERT INTO NME_COURSE_REGISTRATION_?AC_YEAR
                                                (
                                                COURSE_TYPE,
                                                COURSE_ID)
                                                VALUES
                                                (
                                                ?COURSE_TYPE,
                                                ?COURSE_ID);";
                        break;
                    }
                case NMESQLCommands.GetNMECourseTypeByCourseId:
                    {

                        query = @"SELECT 
                                        CT.COURSE_TYPE_ID, CT.COURSE_TYPE
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR AS CR
                                            INNER JOIN
                                        CP_COURSE_TYPE_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID = CR.COURSE_TYPE
                                    WHERE
                                        COURSE_ROOT_ID = ?COURSE_ROOT_ID;";
                        break;
                    }
                case NMESQLCommands.DeleteNMECourseRegistration:
                    {

                        query = @"UPDATE NME_COURSE_REGISTRATION_?AC_YEAR
                                                    SET
                                                    IS_DELETED=1
                                                    WHERE REGISTRATION_ID=?REGISTRATION_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMECourseRegistrationById:
                    {
                        query = @"SELECT 
                                        NR.REGISTRATION_ID,
                                        CT.COURSE_TYPE_ID,
                                        CT.COURSE_TYPE,
                                        NR.COURSE_ID
                                    FROM
                                        NME_COURSE_REGISTRATION_?AC_YEAR AS NR
                                            INNER JOIN
                                        CP_COURSE_TYPE_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID = NR.COURSE_TYPE
                                    WHERE
                                        REGISTRATION_ID = ?REGISTRATION_ID;";


                        break;
                    }
                case NMESQLCommands.UpdateNMECourseRegistration:
                    {
                        query = @"UPDATE NME_COURSE_REGISTRATION_?AC_YEAR
                                            SET
                                            COURSE_TYPE = ?COURSE_TYPE,
                                            COURSE_ID = ?COURSE_ID
                                            WHERE REGISTRATION_ID =?REGISTRATION_ID;";


                        break;
                    }
                case NMESQLCommands.GetRegisteredCoursesFromCourseAllotment:
                    {
                        query = @"SELECT 
                                        CR.COURSE_ROOT_ID,
                                        CONCAT(IFNULL(COURSE_TITLE, ''),
                                                '(',
                                                IFNULL(COURSE_CODE, ''),
                                                ')') AS COURSE_TITLE
                                    FROM
                                        NME_SETTINGS AS NS
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = NS.CLASS_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = CL.SHIFT
                                            INNER JOIN
                                        NME_COURSE_ALLOTMENT_?AC_YEAR AS NC ON NC.SHIFT_ID = SH.SHIFT_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = NC.COURSE_ID
                                    WHERE
                                        NS.SETTINGS_ID = ?SETTINGS_ID AND NC.IS_DELETED!=1;";


                        break;
                    }
                case NMESQLCommands.UpdateNMEClassCourseQuota:
                    {

                        query = @"UPDATE NME_CLASS_COURSE_QUOTA_?AC_YEAR
                                            SET
                                            COURSE_ID=?COURSE_ID,
                                            SETTINGS_ID=?SETTINGS_ID,
                                            CLASS_ID=?CLASS_ID,
                                            QUOTA=?QUOTA
                                            WHERE NME_CLASS_COURSE_QUOTA_ID = ?NME_CLASS_COURSE_QUOTA_ID;";
                        break;
                    }
                case NMESQLCommands.IsNMEClassCourseQuotaExisting:
                    {
                        query = @"SELECT 
                                        NME_CLASS_COURSE_QUOTA_ID
                                    FROM
                                        NME_CLASS_COURSE_QUOTA_?AC_YEAR
                                    WHERE
                                        COURSE_ID = ?COURSE_ID AND SETTINGS_ID = ?SETTINGS_ID
                                            AND CLASS_ID = ?CLASS_ID AND IS_DELETED!=1;";
                        break;
                    }
                case NMESQLCommands.DeleteBulkNMEClassCourse:
                    {
                        query = @"UPDATE NME_CLASS_COURSE_?AC_YEAR
                                                    SET
                                                    IS_DELETED=1
                                                    WHERE COURSE_ID NOT IN (?COURSE_ID) AND CLASS_ID=?CLASS_ID;";
                        break;
                    }
                case NMESQLCommands.DeleteBulkNMEClassCourseQuota:
                    {
                        query = @"UPDATE NME_CLASS_COURSE_QUOTA_?AC_YEAR
                                                    SET
                                                    IS_DELETED=1
                                                    WHERE COURSE_ID NOT IN (?COURSE_ID) AND CLASS_ID=?CLASS_ID;";
                        break;
                    }
                case NMESQLCommands.GetSelectedClassIDBySettingsId:
                    {
                        query = @"SELECT 
                                        CL.CLASS_ID, CL.CLASS_CODE
                                    FROM
                                        NME_SETTINGS AS NS
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = NS.CLASS_ID
                                    WHERE
                                        NS.SETTINGS_ID =?SETTINGS_ID;";
                        break;
                    }
                case NMESQLCommands.GetRegisteredCoursesFromCourseRegistration:
                    {
                        query = @"SELECT 
                                        CR.COURSE_ROOT_ID,
                                        CONCAT(IFNULL(COURSE_TITLE, ''),
                                                '(',
                                                IFNULL(COURSE_CODE, ''),
                                                ')') AS COURSE_TITLE
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR AS CR
                                            INNER JOIN
                                        NME_COURSE_REGISTRATION_?AC_YEAR AS NR ON NR.COURSE_ID = CR.COURSE_ROOT_ID;";
                        break;
                    }
                case NMESQLCommands.FetchCourseTypeByCourseId:
                    {
                        query = @"SELECT 
                                        COURSE_TYPE
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR
                                    WHERE
                                        COURSE_ROOT_ID = 409";
                        break;
                    }
                case NMESQLCommands.UpdateNMECourseRegistrationByCourseId:
                    {
                        query = @"UPDATE NME_COURSE_REGISTRATION_?AC_YEAR
                                        SET 
                                            IS_DELETED = 0
                                        WHERE
                                            COURSE_ID = COURSE_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMEClassByCourseId:
                    {
                        query = @"SELECT 
                                        COURSE_ID
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        COURSE_ID = ?COURSE_ID AND IS_DELETED!=1 GROUP BY COURSE_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMERegisteredStudentByClassId:
                    {
                        query = @"SELECT 
                                        NR.REGISTRATION_ID,
                                        NR.STUDENT_ID,
                                        ST.ROLL_NO,
                                        ST.FIRST_NAME,
                                        CLS.CLASS_CODE,
                                        CONCAT(IFNULL(CR.COURSE_TITLE, ''),
                                                '(',
                                                IFNULL(CR.COURSE_CODE, ''),
                                                ')') AS COURSE_CODE,
                                        NS.SETTINGS_NAME,
                                        CL.CLASS_CODE AS SELECTED_CLASS
                                    FROM
                                        NME_STUDENT_REGISTRATION_?AC_YEAR AS NR
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = NR.CLASS_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS ST ON ST.STUDENT_ID = NR.STUDENT_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = NR.COURSE_ID
                                            INNER JOIN
                                        NME_SETTINGS AS NS ON NS.SETTINGS_ID = NR.SETTINGS_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CL ON CL.CLASS_ID = NR.S_CLASS_ID
                                    WHERE
                                        CLS.CLASS_ID = ?CLASS_ID";
                        break;
                    }
                case NMESQLCommands.FetchNMERegisteredStudentById:
                    {
                        query = @"SELECT 
                                        ST.REGISTRATION_ID,
                                        SI.ROLL_NO,
                                        ST.STUDENT_ID,
                                        SI.FIRST_NAME,
                                        ST.COURSE_ID,
                                        CONCAT(IFNULL(CR.COURSE_TITLE, ''),
                                                '(',
                                                IFNULL(CR.COURSE_CODE, ''),
                                                ')') AS COURSE_CODE,
                                        (SELECT 
                                                SUM(QUOTA)
                                            FROM
                                                NME_CLASS_COURSE_QUOTA_?AC_YEAR AS NQ
                                            WHERE
                                                NQ.COURSE_ID = CR.COURSE_ROOT_ID
                                                    AND NQ.CLASS_ID = CLS.CLASS_ID) AS SUMOFQUTOA,
                                        CLS.CLASS_ID,
                                        CLS.CLASS_CODE
                                    FROM
                                        NME_STUDENT_REGISTRATION_?AC_YEAR AS ST
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = ST.COURSE_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ST.CLASS_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SI ON SI.STUDENT_ID = ST.STUDENT_ID
                                    WHERE
                                        ST.REGISTRATION_ID = ?REGISTRATION_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMECourseByClassId:
                    {
                        query = @"SELECT 
                                        NQ.COURSE_ID,
                                        CONCAT(IFNULL(CR.COURSE_TITLE, ''),
                                                '(',
                                                IFNULL(CR.COURSE_CODE, ''),
                                                ')') AS COURSE_CODE
                                    FROM
                                        NME_CLASS_COURSE_QUOTA_?AC_YEAR AS NQ
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = NQ.COURSE_ID
                                    WHERE
                                        NQ.CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case NMESQLCommands.FetchSumOfNMEClassQuotaByClassIdAndCourseId:
                    {
                        query = @"SELECT 
                                        NQ.QUOTA AS ALLOTED_SEATS, COUNT(NR.COURSE_ID) AS SUMOFQUTOA
                                    FROM
                                        NME_CLASS_COURSE_QUOTA_?AC_YEAR AS NQ
                                            INNER JOIN
                                        NME_STUDENT_REGISTRATION_?AC_YEAR AS NR ON NR.COURSE_ID = NQ.COURSE_ID
                                            AND NR.CLASS_ID = NQ.CLASS_ID
                                    WHERE
                                        NQ.COURSE_ID = ?COURSE_ID AND NQ.CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case NMESQLCommands.UpdateNMERegisteredStudent:
                    {
                        query = @"UPDATE NME_STUDENT_REGISTRATION_?AC_YEAR
                                            SET
                                            STUDENT_ID= ?STUDENT_ID,
                                            CLASS_ID= ?CLASS_ID,
                                            COURSE_ID= ?COURSE_ID,
                                            S_CLASS_ID= ?S_CLASS_ID
                                            WHERE REGISTRATION_ID= ?REGISTRATION_ID;";
                        break;
                    }
                case NMESQLCommands.FetchSelectedClassIdByStudentIdAndCourseId:
                    {
                        query = @"SELECT 
                                        CLS.CLASS_ID
                                    FROM
                                        STU_PERSONAL_INFO AS ST
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.SHIFT = ST.SHIFT_ID
                                    WHERE
                                        ST.STUDENT_ID = ?STUDENT_ID AND CLS.COURSE_ID = ?COURSE_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMERegisteredStudentToPush:
                    {
                        query = @"SELECT 
                                        STUDENT_ID,
                                        CLASS_ID,
                                        COURSE_ID,
                                        SEMESTER,
                                        S_CLASS_ID
                                    FROM
                                        NME_STUDENT_REGISTRATION_?AC_YEAR
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case NMESQLCommands.DeleteNMECourseRegisteredStudent:
                    {

                        query = @"UPDATE NME_STUDENT_REGISTRATION_?AC_YEAR
                                                    SET
                                                    IS_DELETED=1
                                                    WHERE REGISTRATION_ID=?REGISTRATION_ID;";
                        break;
                    }
                case NMESQLCommands.FetchNMESettingsAcademicYear:
                    {
                        query = @"SELECT 
                                        ACADEMIC_YEAR
                                    FROM
                                        NME_SETTINGS
                                    WHERE
                                        IS_ALLOWED = 1
                                    GROUP BY ACADEMIC_YEAR;";
                        break;
                    }
                case NMESQLCommands.PushNMERegisteredStudentToStuCourseInfo:
                    {
                        query = @"INSERT INTO STU_COURSE_INFO_?ACADEMIC_YEAR
                                            (
                                            COURSE_ID,
                                            CLASS_ID,
                                            STUDENT_ID,
                                            S_CLASS_ID,
                                            ACADEMIC_YEAR,
                                            SEMESTER)
                                            VALUES
                                            (
                                            ?COURSE_ID,
                                            ?CLASS_ID,
                                            ?STUDENT_ID,
                                            ?S_CLASS_ID,
                                            ?ACADEMIC_YEAR,
                                            ?SEMESTER);";
                        break;
                    }
                case NMESQLCommands.FetchStuCourseInfo:
                    {
                        query = @"SELECT 
                                        COURSE_ID,
                                        CLASS_ID,
                                        STUDENT_ID,
                                        S_CLASS_ID,
                                        ACADEMIC_YEAR,
                                        SEMESTER
                                    FROM
                                        STU_COURSE_INFO_?ACADEMIC_YEAR WHERE IS_DELETED!=1 AND CLASS_ID!=S_CLASS_ID;";
                        break;
                    }
            }
            return query;
        }
    }
}
