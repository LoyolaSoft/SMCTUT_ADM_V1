using CMS.DAO;

namespace CMS.SQL.Student
{
    public static class StudentSQL
    {
        public static string GetStudentSQL(StudentSQLCommads sEnumCommand)
        {
            string query = string.Empty;
            switch (sEnumCommand)
            {
                case StudentSQLCommads.IfStudentExitsByMultiTable:
                    {
                        query = @"SELECT STUDENT_ID FROM ?TABLE_NAME WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.IsStudentExistingForTC:
                    {

                        query = @"SELECT COUNT(STUDENT_ID)>0 AS STATUS FROM  STU_TRANSFER_CERTIFICATE_?AC_YEAR;";
                        break;
                    }
                case StudentSQLCommads.FetchAlliedCourseByStudentId:
                    {
                        //query = @"SELECT 
                        //            SP.STUDENT_ID,
                        //            CC.COURSE_CODE,
                        //            CC.COURSE_TITLE,
                        //            SC.CLASS_ID,
                        //            CC.IS_ALLIED_SUBJECT
                        //        FROM
                        //            CP_COURSE_ROOT_?AC_YEAR CC
                        //                INNER JOIN
                        //            CP_CLASS_COURSE_?AC_YEAR AS CCC ON CCC.COURSE_ID = CC.COURSE_ROOT_ID
                        //                AND CCC.IS_DELETED != 1
                        //                INNER JOIN
                        //            STU_CLASS AS SC ON SC.CLASS_ID = CCC.CLASS_ID
                        //                AND SC.IS_DELETED != 1
                        //                INNER JOIN
                        //            STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                        //                AND SP.IS_DELETED != 1
                        //        WHERE
                        //            CC.IS_ALLIED_SUBJECT = 1
                        //                AND SP.STUDENT_ID IN (?STUDENT_ID)
                        //        ORDER BY SP.STUDENT_ID;";
                        query = @" SELECT 
                                    SP.STUDENT_ID,
                                    CT.COURSE_CODE,
                                    CT.COURSE_TITLE,
                                    SC.CLASS_ID,
                                    CT.IS_ALLIED_SUBJECT
                                FROM
                                    STU_CLASS AS SC
                                        INNER JOIN
                                    SEMESTER_MARKS_?AC_YEAR AS SM ON SM.STUDENT_ID = SC.STUDENT_ID
                                        INNER JOIN
                                    cp_course_root_?AC_YEAR AS CT ON CT.COURSE_ROOT_ID = SM.COURSE_ID AND CT.IS_DELETED!=1
                                        INNER JOIN
                                    stu_personal_info AS SP ON SP.STUDENT_ID = SC.STUDENT_ID AND SP.IS_DELETED!=1
                                WHERE
                                    SC.STUDENT_ID IN (?STUDENT_ID)
                                        AND CT.IS_ALLIED_SUBJECT = 1 AND SC.IS_DELETED!=1
                                ORDER BY SP.STUDENT_ID,SEMESTER_ID,COURSE_TITLE";
                        break;
                    }
                case StudentSQLCommads.FetchAcademicYearByBatchAndStudentId:
                    {
                        query = @"SELECT 
                                SP.STUDENT_ID,
                                Y.ACADEMIC_YEAR_ID,
                                Y.AC_YEAR,
                                S.SEMESTER, 
                                S.IS_ACTIVE
                            FROM
                                STU_PERSONAL_INFO SP
		                            INNER JOIN
	                            ACADEMIC_BATCHES B
		                            INNER JOIN 
	                            ACADEMIC_YEAR AS Y ON Y.ACADEMIC_YEAR_ID=B.ACADEMIC_YEAR_ID
		                            INNER JOIN 
	                            ACADEMIC_SEMESTER_?AC_YEAR AS S ON S.BATCH=B.BATCH_ID
                                WHERE SP.STUDENT_ID IN (?STUDENT_ID) AND S.IS_ACTIVE=1 GROUP BY AC_YEAR;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentListByStaffId:
                    {
                        query = @"SELECT 
                                STUDENT_ID
                            FROM
                                CP_CLASS_COURSE_STAFF_?AC_YEAR C
                                    INNER JOIN
                                STU_PERSONAL_INFO AS P ON P.CLASS_ID = C.CLASS_ID
                            WHERE
                                C.STAFF_ID = ?STAFF_ID
                            GROUP BY P.STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.AcadamicIfExits:
                    {
                        query = @"SELECT ROLL_NO FROM STU_PERSONAL_INFO WHERE ROLL_NO=?ROLL_NO;";
                        break;
                    }
                case StudentSQLCommads.DeleteCalender:
                    {
                        query = @"UPDATE CALENDER SET
                                  IS_DELETED=?IS_DELETED
                                  WHERE CALENDER_ID=?CALENDER_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateDayOrder:
                    {
                        query = @"UPDATE CALENDER_?AC_YEAR SET
	                                DAY_ORDER_DATE=?DAY_ORDER_DATE, 
	                                DAY_ORDER=?DAY_ORDER,  
	                                DAY_ORDER_END_DATE=?DAY_ORDER_END_DATE
                                 WHERE DAY_ID=?DAY_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateCalender:
                    {
                        query = @"UPDATE CALENDAR_EVENTS_?AC_YEAR SET 
                                  EVENT_ID=?EVENT_ID,
                                  EVENT_NAME=?EVENT_NAME,
                                  EVENT_START_DATE=?EVENT_START_DATE,
                                  EVENT_END_DATE=?EVENT_END_DATE
                                WHERE EVENT_ID=?EVENT_ID;";
                        break;
                    }
                case StudentSQLCommads.InsertCalender:
                    {
                        query = @"INSERT INTO CALENDAR_EVENTS_?AC_YEAR(
                                DAY_ID,EVENT_START_DATE,EVENT_TYPE,
                                EVENT_NAME,EVENT_DESCRIPTION,EVENT_DEPARTMENT,
                                EVENT_END_DATE,STAFF_CATEGORY,ACCESS_VISIBILITY,
                                VIEW_MANAGE,USER_ID,RESPONSIBLE_STAFF,RESPONSIBLE_STUDENT,
                                EVENT_REPORT,EVENT_PARTICIPANTS,EVENT_STATUS,EVENT_BUDGET,
                                AMOUNT_SPENT,HOLIDAY_TYPE)
                                VALUES
                                (
                                   ?DAY_ID,?EVENT_START_DATE,?EVENT_TYPE,
                                   ?EVENT_NAME,?EVENT_DESCRIPTION,?EVENT_DEPARTMENT,
                                   ?EVENT_END_DATE,?STAFF_CATEGORY,?ACCESS_VISIBILITY,
                                   ?VIEW_MANAGE,?USER_ID,?RESPONSIBLE_STAFF,?RESPONSIBLE_STUDENT,
                                   ?EVENT_REPORT,?EVENT_PARTICIPANTS,?EVENT_STATUS,?EVENT_BUDGET,
                                   ?AMOUNT_SPENT,?HOLIDAY_TYPE
                                )";
                        break;
                    }
                case StudentSQLCommads.FetchCalenderEvent:
                    {
                        query = @"SELECT EVENT_ID,
                                CE.DAY_ID,
                                DATE_FORMAT(CE.EVENT_START_DATE,'%Y')  AS 'SYEAR', 
	                            DATE_FORMAT(CE.EVENT_START_DATE,'%m') AS 'SMONTH' , 
	                            CONCAT('-',DATE_FORMAT(CE.EVENT_START_DATE,'%d'))  AS 'SDAY',
                                CE.EVENT_TYPE,
                                CE.EVENT_NAME,
                                CE.EVENT_DESCRIPTION,
                                CE.EVENT_DEPARTMENT,
	                            DATE_FORMAT(CE.EVENT_END_DATE,'%Y')  AS 'EYEAR', 
	                            DATE_FORMAT(CE.EVENT_END_DATE,'%m') AS 'EMONTH' , 
	                            CONCAT('-',DATE_FORMAT(CE.EVENT_END_DATE,'%d'))  AS 'EDAY',
                                CE.USER_ID,
                                CE.RESPONSIBLE_STAFF,
                                CE.RESPONSIBLE_STUDENT,
                                CE.EVENT_REPORT,
                                CE.EVENT_PARTICIPANTS,
                                CE.EVENT_STATUS,
                                CE.EVENT_BUDGET,
                                CE.AMOUNT_SPENT,
                                CE.HOLIDAY_TYPE,
                                CONCAT('ORANGE')AS COLOR
                            FROM CALENDAR_EVENTS_?AC_YEAR CE
                             WHERE CE.IS_DELETED!=1;";
                        break;
                    }
                case StudentSQLCommads.FetchCalender:
                    {
                        query = @"SELECT EVENT_ID,
                                CE.DAY_ID,
                                DATE_FORMAT(CE.EVENT_START_DATE,'%Y')  AS 'SYEAR', 
                                DATE_FORMAT(CE.EVENT_START_DATE,'%m') AS 'SMONTH' , 
                                CONCAT('-',DATE_FORMAT(CE.EVENT_START_DATE,'%d'))  AS 'SDAY',
                                DATE_FORMAT(CE.EVENT_END_DATE,'%Y')  AS 'EYEAR', 
                                DATE_FORMAT(CE.EVENT_END_DATE,'%m') AS 'EMONTH' , 
                                CONCAT('-',DATE_FORMAT(CE.EVENT_END_DATE,'%d'))  AS 'EDAY',
                                CE.RESPONSIBLE_STUDENT,
                                CE.EVENT_NAME AS 'DAY_ORDER',
                                CONCAT('ORANGE') AS 'COLOR'
                            FROM CALENDAR_EVENTS_?AC_YEAR CE
                            WHERE CE.IS_DELETED!=1 UNION SELECT 
                                '' AS EVENT_ID,
                                CE.DAY_ID AS DAY_ID,
                                DATE_FORMAT(CE.EVENT_START_DATE,'%Y')  AS 'SYEAR', 
                                DATE_FORMAT(CE.EVENT_START_DATE,'%m') AS 'SMONTH' , 
                                CONCAT('-',DATE_FORMAT(CE.EVENT_START_DATE,'%d'))  AS 'SDAY',
                                DATE_FORMAT(CE.EVENT_END_DATE,'%Y')  AS 'EYEAR', 
                                DATE_FORMAT(CE.EVENT_END_DATE,'%m') AS 'EMONTH' , 
                                CONCAT('-',DATE_FORMAT(CE.EVENT_END_DATE,'%d'))  AS 'EDAY',
                                '' AS RESPONSIBLE_STUDENT,
                                 if(C.DAY_ORDER IS NULL || C.DAY_ORDER='','',CONCAT('DAY ORDER  :  ',C.DAY_ORDER))as 'DAY_ORDER',
                                CONCAT('GRAY') AS 'COLOR'
                            FROM CALENDER_?AC_YEAR C INNER JOIN CALENDAR_EVENTS_?AC_YEAR AS CE ON CE.DAY_ID=C.DAY_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchBonafideCertificate:
                    {
                        query = @"SELECT 
                                    BONAFIDE_ID, 
                                    UPPER(ROLL_NO) AS 'ROLL_NO', 
                                    UPPER(NAME)AS 'NAME', 
                                    UPPER(FATHER_NAME)AS 'FATHER_NAME', 
                                    UPPER(CLASS)AS 'CLASS', 
                                    UPPER(DURING_YEAR)AS 'DURING_YEAR',
	                                DATE_FORMAT(CURRENT_DATE(),'%d/%m/%Y')AS 'DATE'
                                FROM
                                    STU_BONAFIDE_CERTIFICATE_?AC_YEAR WHERE BONAFIDE_ID=?BONAFIDE_ID;";
                        break;
                    }
                case StudentSQLCommads.InsertBonafideCertificate:
                    {
                        query = @"INSERT INTO STU_BONAFIDE_CERTIFICATE_?AC_YEAR(
                                    ROLL_NO, 
                                    NAME, 
                                    FATHER_NAME, 
                                    CLASS, 
                                    DURING_YEAR)
                                    VALUES(?ROLL_NO,?NAME,?FATHER_NAME,?CLASS,?DURING_YEAR);";
                        break;
                    }
                case StudentSQLCommads.UpdateBonafideCertificate:
                    {
                        query = @"UPDATE STU_BONAFIDE_CERTIFICATE_?AC_YEAR SET
	                                ROLL_NO =?ROLL_NO,
	                                NAME =?NAME,
	                                FATHER_NAME =?FATHER_NAME,
	                                CLASS =?CLASS,
	                                DURING_YEAR =?DURING_YEAR
                                WHERE BONAFIDE_ID =?BONAFIDE_ID;";
                        break;
                    }
                case StudentSQLCommads.CheckBonafideIsExits:
                    {
                        query = @"SELECT 
                                    BONAFIDE_ID
                                FROM
                                    STU_BONAFIDE_CERTIFICATE_?AC_YEAR
                                WHERE
                                    ROLL_NO = ?ROLL_NO;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentBatchByStudentId:
                    {
                        query = @"SELECT 
                                    SB.BATCH
                                FROM
                                    SUP_BATCHES AS SB
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.BATCH = SB.BATCH_ID
                                WHERE
                                    STUDENT_ID IN(?STUDENT_ID);";
                        break;
                    }
                case StudentSQLCommads.FetchCourseCertificate:
                    {
                        query = @"SELECT 
                                    SP.STUDENT_ID,
                                    SP.ROLL_NO,
                                    CONCAT(IFNULL(UPPER(SP.FIRST_NAME), ''),
                                            ' ',
                                            IFNULL(UPPER(SP.LAST_NAME), '')) AS 'NAME',
                                    CONCAT(IFNULL((CASE
                                                        WHEN CLS.CLASS_YEAR = 1 THEN 'FIRST YEAR'
                                                        WHEN CLS.CLASS_YEAR = 2 THEN 'SECOND YEAR'
                                                        WHEN CLS.CLASS_YEAR = 3 THEN 'THIRD YEAR'
                                                        WHEN CLS.CLASS_YEAR = 4 THEN 'FOURTH YEAR'
                                                        WHEN CLS.CLASS_YEAR = 5 THEN 'FIVETH YEAR'
                                                    END),
                                                    ''),
                                            ' ',
                                            IFNULL(UPPER(CP.PROGRAMME_NAME), '')) AS CLASS,
	                                CONCAT(IFNULL(UPPER(SB.BATCH), ''),
                                            '-',
                                            ' ',
                                            IFNULL(AY.AC_YEAR + 1, '')) AS 'Year',
                                    UPPER(DT.DEPARTMENT) AS 'PART1',
                                    CONCAT('ENGLISH') AS 'PART2',
                                    CDT.DEPARTMENT AS 'MAIN',
                                    DATE_FORMAT(CURRENT_DATE(), '%d/%m/%Y') AS 'DATE'
                                FROM
                                    STU_PERSONAL_INFO AS SP
                                        INNER JOIN
                                    STU_CLASS AS SC ON SC.STUDENT_ID = SP.STUDENT_ID
                                        AND SC.ACADEMIC_YEAR = ?AC_YEAR AND SP.IS_DELETED!=1
                                        INNER JOIN
                                    SEMESTER_MARKS_?BATCH AS SEM ON SEM.STUDENT_ID = SC.STUDENT_ID 
                                        INNER JOIN
                                    CP_COURSE_ROOT_?BATCH AS CR ON CR.COURSE_ROOT_ID = SEM.COURSE_ID AND CR.IS_DELETED!=1
                                        INNER JOIN
                                    CP_DEPARTMENT_?BATCH AS DT ON DT.DEPARTMENT_ID = CR.DEPARTMENT AND DT.IS_DELETED!=1
                                        INNER JOIN
                                    SUP_BATCHES AS SB ON SB.BATCH_ID = SP.BATCH
                                        INNER JOIN
                                    CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = SC.CLASS_ID AND CLS.IS_DELETED!=1
                                        INNER JOIN
                                    CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = SP.PROGRAM_ID AND CP.IS_DELETED!=1
                                        INNER JOIN
                                    CP_DEPARTMENT_?AC_YEAR AS CDT ON CDT.DEPARTMENT_ID = CLS.DEPARTMENT AND CDT.IS_DELETED!=1
                                        INNER JOIN
                                    ACADEMIC_YEAR AS AY ON AY.ACTIVE_YEAR = 1 AND AY.IS_DELETED!=1
                                WHERE
                                    SC.STUDENT_ID IN (?STUDENT_ID)
                                        AND CR.PAPER_TYPE = 1 AND SC.IS_DELETED!=1
                                GROUP BY SC.STUDENT_ID
                                ORDER BY SC.STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchConductCertificate:
                    {
                        query = @"SELECT 
                                   SP.STUDENT_ID,
                               SP.TC_SERIAL_NO,
                                   SP.ROLL_NO,
                               CONCAT(IFNULL(UPPER(SP.FIRST_NAME),''),' ',IFNULL(UPPER(SP.LAST_NAME),'')) AS 'FIRST_NAME',
                                   UPPER(P.PROGRAMME_NAME)AS 'PROGRAMME_NAME',
                                   CONCAT('JUNE - ',LEFT(TC.ACADEMIC_YEARS , 4)) AS 'START_YEAR',
                               CONCAT('APRIL - ',RIGHT(TC.ACADEMIC_YEARS , 4)) AS 'END_YEAR',
                                   C.CONDUCT_NAME AS 'CONDUCT',
                                   DATE_FORMAT(TC.TC_GIVEN_DATE,'%d/%m/%Y')AS 'DATE',
                                   (SELECT PLACE FROM COLLEGE_DETAILS)AS 'PLACE'
                               FROM
                                   STU_PERSONAL_INFO SP 
                                   INNER JOIN CP_PROGRAMME_?AC_YEAR AS P ON P.PROGRAMME_ID=SP.PROGRAM_ID
                                   LEFT JOIN  SUP_CONDUCT AS C ON C.CONDUCT_ID=SP.CONDUCT
                                   LEFT JOIN STU_TRANSFER_CERTIFICATE_?AC_YEAR AS TC ON TC.STUDENT_ID=SP.STUDENT_ID
                                   WHERE SP.STUDENT_ID IN(?STUDENT_ID);";
                        break;
                    }
                case StudentSQLCommads.BindStudentsGroupById:
                    {
                        query = @"SELECT 
                                    SP.ROLL_NO,
                                    TC.CERTIFICATE_ID,
                                    TC.STUDENT_ID,
                                    UPPER(TC.FIRST_NAME) AS 'FIRST_NAME',
                                    UPPER(TC.GENDER) AS 'GENDER',
                                    CONCAT(UPPER(TC.NATIONALITY),
                                            ' - ',
                                            UPPER(TC.RELIGION)) AS 'NATIONALITY',
                                    CONCAT(UPPER(TC.CASTE),
                                            ' - ',
                                            UPPER(TC.COMMUNITY)) AS 'CASTE',
                                    CONCAT(DATE_FORMAT(TC.DATE_OF_BIRTH, '%d/%m/%Y'),
                                            ' ( ',
                                            UPPER(TC.DATETOWORDS),
                                            ' ) ') AS 'DATE_OF_BIRTH',
                                    CONCAT(DATE_FORMAT(TC.ADMISSION_DATE, '%d/%m/%Y'),
                                            ' - ',
                                            UPPER(TC.ADMITTED_CLASS)) AS 'ADMITTED_DATE',
                                    CONCAT('1. ',' ',UPPER(TC.IDENTIFICATION_MARK1))  AS 'IDENTIFICATION_MARK1',
                                    CONCAT('2. ',' ',UPPER(TC.IDENTIFICATION_MARK2)) AS 'IDENTIFICATION_MARK2',
                                    UPPER(TC.MAIN_COURSE) AS 'MAIN_COURSE',
                                    UPPER(TC.ALLIED) AS 'ALLIED',
                                    UPPER(O.OPTION_NAME) AS 'FEE_PAID',
                                    UPPER(SO.OPTION_NAME) AS 'SCHOLAR_SHIP',
                                    DATE_FORMAT(TC.LEAVING_DATE, '%d/%m/%Y') AS 'LEAVING_DATE',
                                    UPPER(TC.REASON_FOR_LEAVING) AS 'REASON_FOR_LEAVING',
                                    UPPER(TC.LEAVING_CLASS) AS 'LEAVING_CLASS',
                                    UPPER(SC.CONDUCT_NAME) AS 'CONDUCT',
                                    DATE_FORMAT(TC.TC_APPLIED_DATE, '%d/%m/%Y') AS 'TC_APPLIED_DATE',
                                    DATE_FORMAT(TC.TC_GIVEN_DATE, '%d/%m/%Y') AS 'TC_GIVEN_DATE',
                                    IF(TC.FATHER_NAME IS NULL
                                            || TC.FATHER_NAME = '',
                                        IF(TC.GUARDIAN_NAME IS NULL
                                                || TC.GUARDIAN_NAME = '',
                                            '----',
                                            UPPER(TC.GUARDIAN_NAME)),
                                        UPPER(TC.FATHER_NAME)) AS 'FATHER_NAME',
                                    UPPER(TC.ACADEMIC_YEARS) AS 'ACADEMIC_YEARS',
                                    UPPER(TC.CLASSES_STUDEIED) AS 'CLASSES_STUDEIED',
                                    UPPER(TC.FIRST_LANGUAGE) AS 'FIRST_LANGUAGE'
                                FROM
                                    STU_TRANSFER_CERTIFICATE_?AC_YEAR TC
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = TC.STUDENT_ID
                                        LEFT JOIN
                                    SUP_OPTION AS O ON O.OPTION_ID = TC.FEE_PAID
                                    LEFT JOIN 
                                    SUP_OPTION AS SO ON SO.OPTION_ID=TC.SCHOLAR_SHIP
                                     LEFT JOIN 
                                    SUP_CONDUCT AS SC ON SC.CONDUCT_ID=TC.CONDUCT
                                       
                                WHERE
                                    TC.STUDENT_ID IN (?STUDENT_ID);";
                        break;
                    }
                case StudentSQLCommads.UpdatePersonalFromTC:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO SET
                                 FIRST_NAME=?FIRST_NAME,
                                 STUDENT_ID=?STUDENT_ID, 
                                 FATHER_NAME=?FATHER_NAME, 
                                 GUARDIAN_NAME=?GUARDIAN_NAME, 
                                 NATIONALITY=?NATIONALITY, 
                                 RELIGION=?RELIGION, 
                                 CASTE=?CASTE, 
                                 COMMUNITY=?COMMUNITY,
                                 GENDER=?GENDER, 
                                 DATE_OF_BIRTH=?DATE_OF_BIRTH, 
                                 IDENTIFICATION_MARK1=?IDENTIFICATION_MARK1,
                                 IDENTIFICATION_MARK2=?IDENTIFICATION_MARK2,
                                 ADMISSION_DATE=?ADMISSION_DATE,
                                 ADMITTED_CLASS=?ADMITTED_CLASS,
                                 LEAVING_CLASS=?LEAVING_CLASS, 
                                 LEAVIN_GDATE=?LEAVIN_GDATE, 
                                 TC_APPLIED_DATE=?TC_APPLIED_DATE, 
                                 TC_GIVEN_DATE=?TC_GIVEN_DATE,
                                 CONDUCT=?CONDUCT
                                 WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.BindStudentDetailsForTC:
                    {
                        query = @"SELECT 
                                    SP.PROGRAM_ID,
                                    SP.CLASS_ID,
                                    SP.STUDENT_ID,
	                                CONCAT(IFNULL(SP.FIRST_NAME,''),' ',IFNULL(SP.LAST_NAME,'')) AS 'FIRST_NAME',
 	                                UPPER(SP.FATHER_NAME) AS 'FATHER_NAME',
                                    UPPER(SP.GUARDIAN_NAME) AS 'GUARDIAN_NAME',
                                    SP.NATIONALITY,
                                    SP.RELIGION,
	                                SP.COMMUNITY,
                                    SP.CASTE,
                                    SP.GENDER,
                                    DATE_FORMAT(SP.DATE_OF_BIRTH,'%d/%m/%Y')AS 'DATE_OF_BIRTH',
	                                SP.IDENTIFICATION_MARK1,
                                    SP.IDENTIFICATION_MARK2,
	                                DATE_FORMAT(SP.ADMISSION_DATE,'%d/%m/%Y') AS 'ADMISSION_DATE',
                                    C.CLASS_NAME,
                                    D.DEPARTMENT,
	                                DATE_FORMAT(SP.LEAVIN_GDATE,'%d/%m/%Y')AS 'LEAVIN_GDATE',
                                    DATE_FORMAT(SP.TC_APPLIED_DATE,'%d/%m/%Y')AS 'TC_APPLIED_DATE',
                                    DATE_FORMAT(SP.TC_GIVEN_DATE,'%d/%m/%Y')AS 'TC_GIVEN_DATE'
                                FROM
                                    STU_PERSONAL_INFO SP
                                    LEFT JOIN CP_DEPARTMENT_?AC_YEAR AS D ON D.DEPARTMENT_ID=SP.DEPT_ID
                                    LEFT JOIN CP_CLASSES_?AC_YEAR AS C ON C.CLASS_ID=SP.CLASS_ID
                                    WHERE SP.STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.StudentIfExits:
                    {
                        query = @"SELECT 
                                    STUDENT_ID, FIRST_NAME
                                FROM
                                    STU_TRANSFER_CERTIFICATE_?AC_YEAR WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchPriviousQualification:
                    {
                        query = @"SELECT 
                                    STUDENT_ID,
	                                LAST_SCHOOL_OR_COLLEGE,
                                    LAST_STUDIED_PLACE,
                                    LAST_STUDIED_CLASS,
                                    EXAM_PASSED
                                FROM STU_PERSONAL_INFO WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchProgrammeByID:
                    {
                        query = @"SELECT 
                                    PROGRAMME_ID,
                                    PROGRAMME_NAME
                                FROM
                                    CP_PROGRAMME_?AC_YEAR WHERE DEPARTMENT=?DEPARTMENT;";
                        break;
                    }
                case StudentSQLCommads.FetchProgrammeByShiftId:
                    {
                        query = @"SELECT 
                                CP.PROGRAMME_ID,
                                CP.PROGRAMME_NAME
                            FROM
                               CP_CLASSES_?AC_YEAR CC
                                    INNER JOIN
                                CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = CC.PROGRAMME
                            WHERE
                                CC.SHIFT =?SHIFT_ID
                            GROUP BY CP.PROGRAMME_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchClassesByID:
                    {
                        query = @"SELECT 
                                    CLASS_ID,
                                    CLASS_NAME
                                FROM
                                    CP_CLASSES_?AC_YEAR
                                WHERE
                                    SHIFT = ?SHIFT;";
                        break;
                    }
                case StudentSQLCommads.FetchClass:
                    {
                        query = @"SELECT 
                                       CLASS_ID,CLASS_NAME
                                    FROM
                                        CP_CLASSES_?AC_YEAR WHERE PROGRAMME=?PROGRAMME AND SHIFT=?SHIFT;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentsForTC:
                    {
                        query = @"SELECT 
                                       STUDENT_ID,
                                       CONCAT(UPPER(FIRST_NAME),' - ',UPPER(ROLL_NO)) AS 'FIRST_NAME'
                                    FROM
                                        STU_PERSONAL_INFO
                                        WHERE PROGRAM_ID=?PROGRAM_ID AND CLASS_ID=?CLASS_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchConduct:
                    {
                        query = @"SELECT 
                                   CONDUCT_ID, CONDUCT_NAME
                                FROM
                                    SUP_CONDUCT;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentList:
                    {
                        query = @"SELECT 
                                     SPI.STUDENT_ID, 
                                     ROLL_NO,
                                     FIRST_NAME, 
                                     D.DEPARTMENT, 
                                     CC.CLASS_NAME,
                                     S.SHIFT_NAME,
                                    if(SPI.IS_LEFT =0,'No','Yes') as IS_LEFT
                                    FROM
                                     STU_PERSONAL_INFO SPI
                                     LEFT JOIN CP_DEPARTMENT_?AC_YEAR AS D ON D.DEPARTMENT_ID=SPI.DEPT_ID
                                     LEFT JOIN SUP_SHIFT AS S ON S.SHIFT_ID=SPI.SHIFT_ID
                                     LEFT JOIN STU_CLASS AS SC ON SC.STUDENT_ID=SPI.STUDENT_ID
                                     INNER JOIN CP_CLASSES_?AC_YEAR AS CC ON CC.CLASS_ID=SC.CLASS_ID
                                     WHERE  SPI.IS_DELETED!=1  ORDER BY SPI.STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchAcadamicInfo:
                    {
                        query = @"SELECT
                                                     REGISTER_NO,
                                                     ROLL_NO,
                                                     UNIVERSITY_ROLLNO,
                                                     DATE_FORMAT(ADMISSION_DATE,'%d/%m/%Y') AS ADMISSION_DATE ,
                                                     ADMISSION_NO,
                                                     ADMITTED_CLASS,
												     DATE_FORMAT(STUDENT_REGISTERDED_DATE,'%d/%m/%Y') AS STUDENT_REGISTERDED_DATE ,
                                                     LAST_SCHOOL_OR_COLLEGE,
                                                     LAST_STUDIED_PLACE,
                                                     LAST_STUDIED_CLASS,EXAM_PASSED,
                                                     REMARKS,TC_SERIAL_NO,
                                                     CLASS_ID,
                                                     PROGRAM_ID,
                                                     BATCH,
                                                     SHIFT_ID,STU_EMAILID,IS_LEFT,
                                                     DEPT_ID,  UA.ROLE,
                                                        UA.USER_TYPE
                                                    FROM
                                                        STU_PERSONAL_INFO AS SP
                                                            INNER JOIN
                                                        USER_ACCOUNT AS UA ON UA.USER_ID = SP.STUDENT_ID
                                                            AND UA.USERNAME = SP.ROLL_NO
                                                    WHERE
                                                        STUDENT_ID=?STUDENT_ID;";
                        break;
                    }

                case StudentSQLCommads.FetchParentsDetails:
                    {
                        query = @"SELECT
                                                FATHER_NAME,
                                                DATE_FORMAT(F_DATE_OF_BIRTH,'%d/%m/%Y') AS F_DATE_OF_BIRTH,
                                                MOTHER_NAME, 
                                                DATE_FORMAT(M_DATE_OF_BIRTH,'%d/%m/%Y') AS M_DATE_OF_BIRTH, 
                                                FR_BUSINESS_PHONE,
                                                FR_MOBILE,
                                                FR_PASSPORT_number, 
                                                MO_BUSINESS_PHONE,
                                                MO_MOBILE,
                                                MO_PASSPORT_number, 
                                                FR_INCOME,
                                                MO_INCOME,
                                                DATE_FORMAT(FR_DATE_OF_EXPIRY,'%d/%m/%Y') AS FR_DATE_OF_EXPIRY, 
                                                DATE_FORMAT(MO_DATE_OF_EXPIRY,'%d/%m/%Y') AS MO_DATE_OF_EXPIRY,
                                                FR_EMAIL,
                                                MO_EMAIL,
                                                MO_OCCUPATION,
                                                FR_OCCUPATION,
                                                FATHER_EDUCATION, 
                                                MOTHER_EDUCATION,
                                                FR_NATIONALITY,
                                                MO_NATIONALITY
                                            FROM STU_PERSONAL_INFO WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentActivity:
                    {
                        query = @"SELECT 
                                                ACTIVITY_ID,
                                                STUDENT_ID,
                                                POST_HELD,
                                                INITIATIVE_SHOWN,
                                                PARTICIPATION,
                                                DATE_FORMAT(DATE_FROM,'%d/%m/%Y') AS DATE_FROM,
                                                DATE_FORMAT(DATE_TO,'%d/%m/%Y') AS DATE_TO,
                                                EXTRA_ACTIVITY,
                                                POSITION_OBTAINED,
                                                ACTIVITY,
                                                ACTIVITY_IMAGE1,
                                                ACTIVITY_IMAGE2,
                                                ACTIVITY_IMAGE3
                                            FROM
                                                STU_ACTIVITIES WHERE ACTIVITY_ID=?ACTIVITY_ID;";
                        break;
                    }
                case StudentSQLCommads.BindActivity:
                    {
                        query = @"SELECT 
                                   SA.ACTIVITY_ID,
                                   STUDENT_ID,
                                   POST_HELD,
                                   INITIATIVE_SHOWN,
                                   PARTICIPATION,
                                   DATE_FORMAT(DATE_FROM,'%d/%m/%Y') AS DATE_FROM,
                                   DATE_FORMAT(DATE_TO,'%d/%m/%Y') AS DATE_TO,
                                   EXTRA_ACTIVITY,
                                   POSITION_OBTAINED,
                                   A.ACTIVITY_NAME,
                                   ACTIVITY_IMAGE1,
                                   ACTIVITY_IMAGE2,
                                   ACTIVITY_IMAGE3
                               FROM
                                   STU_ACTIVITIES SA 
                                   INNER JOIN sup_activity AS A ON A.ACTIVITY_ID=SA.ACTIVITY WHERE SA.IS_DELETED!=1 AND STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.DeleteActivity:
                    {
                        query = @"UPDATE STU_ACTIVITIES SET IS_DELETED=?IS_DELETED WHERE ACTIVITY_ID=?ACTIVITY_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchPersonl:
                    {
                        query = @"SELECT 
		                                        FIRST_NAME,
                                                LAST_NAME,
                                                CASTE,
		                                        DATE_FORMAT(DATE_OF_BIRTH,'%d/%m/%Y') AS DATE_OF_BIRTH,
                                                ANNUAL_INCOME,
                                                RESIDENCE_NO,
                                                PASSPORT_Number,
                                                VISA_SPONSOR,
                                                VISA_Number,
                                                IDENTIFICATION_MARK1,
                                                IDENTIFICATION_MARK2,
                                                HOME_PHONE,
                                                AADHAR_NUMBER,
                                                STU_MOBILENO,
                                                PLACE_OF_BIRTH,
                                                MARITAL_STATUS,
                                                GENDER,
                                                COMMUNITY,
                                                MOTHER_TONGUE,
                                                BLOOD_GROUP,
		                                        RELIGION,
                                                SPECIALLY_ABLED,
                                                FIRST_LANGUAGE,
                                                SECOND_LANGUAGE,
                                                RESIDENCY_TYPE,
                                                NATIONALITY FROM STU_PERSONAL_INFO 
                                                WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchAddress:
                    {
                        query = @"SELECT 
                                              ADDRESS_ID,
                                              STUDENT_ID,
                                              C_DOOR_DETAIL,
                                              C_STREET,
                                              C_VILLAGE_AREA,
                                              C_POST_PLACE_TOWN,
                                              C_TALUK,C_CITY,
                                              C_DISTRICT,
                                              C_PINCODE,
                                              C_COUNTRY,
                                              P_DOOR_DETAIL,
                                              P_STREET,
                                              P_VILLAGE_AREA,
                                              P_POST_PLACE_TOWN,
                                              P_TALUK,
                                              P_CITY,
                                              P_DISTRICT,
                                              P_PINCODE,
                                              P_COUNTRY
                                            FROM STU_ADDRESS WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchCertificate:
                    {
                        query = @"SELECT 
                                 CERTIFICATE_ID,
                                 STUDENT_ID,
                                 CERTIFICATE_NO,
                                 DATE_FORMAT(RECEIVED_DATE,'%d/%m/%Y') AS RECEIVED_DATE,
                                 DATE_FORMAT(GIVEN_DATE,'%d/%m/%Y') AS GIVEN_DATE, 
                                 SA.ACHIEVEMENT_NAME,
                                 PURPOSE,
                                 REGISTER_NUMBER,
                                 CERTIFICATE_NAME
                                 FROM STU_CERTIFICATE SC 
                                 INNER JOIN SUP_ACHIEVEMENT AS SA ON SA.ACHIEVEMENT_ID=SC.ARCHIVE 
                                 WHERE SC.IS_DELETED!=1 AND STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.BindCertificate:
                    {
                        query = @"SELECT 
                                              CERTIFICATE_ID,
                                              STUDENT_ID,
                                              CERTIFICATE_NO,
                                              DATE_FORMAT(RECEIVED_DATE,'%d/%m/%Y') AS RECEIVED_DATE,
                                              DATE_FORMAT(GIVEN_DATE,'%d/%m/%Y') AS GIVEN_DATE, 
                                              ARCHIVE,
                                              PURPOSE,
                                              REGISTER_NUMBER,
                                              CERTIFICATE_NAME
                                        FROM STU_CERTIFICATE WHERE CERTIFICATE_ID=?CERTIFICATE_ID;";
                        break;
                    }
                case StudentSQLCommads.DeleteCertificate:
                    {
                        query = @"UPDATE STU_CERTIFICATE SET IS_DELETED=?IS_DELETED WHERE CERTIFICATE_ID=?CERTIFICATE_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchAccountInfo:
                    {
                        query = @"SELECT 
                                                   ACCOUNT_NO,
                                                   IFSC_CODE,
                                                   MICR_CODE
                                                FROM STU_PERSONAL_INFO WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchCounselling:
                    {
                        query = @"SELECT 
                                     SC_ID,
                                     DATE_FORMAT(DOC,'%d/%m/%Y') AS DOC, 
                                     DURATION,
                                     STUDENT_ID,
                                     REMARKS,
                                     COMMENTS,
                                     SB.BATCH,
                                     SC.IS_DELETED
                                 FROM
                                     STU_COUNSELLING SC 
                                     INNER JOIN SUP_BATCHES AS SB ON SB.BATCH_ID=SC.BATCH
                                     WHERE SC.IS_DELETED!=1 AND SC.STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.BindCounselling:
                    {
                        query = @"SELECT 
                                                SC_ID,
                                                DATE_FORMAT(DOC,'%d/%m/%Y') AS DOC, 
                                                DURATION,
                                                STUDENT_ID,
                                                REMARKS,
                                                COMMENTS,
                                                BATCH,
                                                IS_DELETED
                                            FROM
                                                STU_COUNSELLING WHERE SC_ID=?SC_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchCourses:
                    {
                        query = @"SELECT 
                                                COURSE_REGISTRATION_ID,
                                                PROGRAM_ID,
                                                CLASS_ID,
                                                STUDENT_ID,
                                                PART,
                                                MAIN_SUBJECT,
                                                ALLIED1,
                                                ALLIED2,
                                                ALLIED3,
                                                ALLIED4,
                                                ELECTIVE1,
                                                ELECTIVE2,
                                                ELECTIVE3,
                                                ELECTIVE4,
                                                SPECIAL_SUBJECT
                                            FROM
                                                STU_COURSES WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchIncident:
                    {
                        query = @"SELECT 
                                     INCIDENT_ID,
                                     STUDENT_ID,
                                     DATE_FORMAT(DATE_OF_INCIDENT,'%d/%m/%Y') AS DATE_OF_INCIDENT,
                                     TIME_OF_INCIDENT,
                                     PLACE_OF_INCIDENT,
                                     FIRST_AID_DONE,
                                     SO.OPTION_NAME,
                                     DATE_FORMAT(DATE_INFORMED,'%d/%m/%Y') AS DATE_INFORMED,
                                     INCIDENT_DETAILS
                                 FROM
                                     STU_INCIDENT SI INNER JOIN SUP_OPTION AS SO ON SO.OPTION_ID=SI.INFORMED_TO_PARENTS WHERE SI.IS_DELETED!=1 AND STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchSibling:
                    {
                        query = @"SELECT 
                                     SIBLING_ID,
                                     STUDENT_ID,
                                     NAME,
                                     AGE,
                                     INSTITUTE_NAME,
                                     P.PROGRAMME_NAME,
                                     S.IS_DELETED,
                                     QUALIFICATION,
                                     EMPLOYED,
                                     DATE_FORMAT(DATE_OF_BIRTH,'%d/%m/%Y') AS DATE_OF_BIRTH,
                                     G.GENDER_NAME,
                                     O.OCCUPATION_NAME,
                                     MONTHLY_INCOME,
                                     PROGNAME,
                                     IS_SAME_COLLEGE
                                 FROM STU_SIBLING  S
                                 INNER JOIN CP_PROGRAMME_?AC_YEAR AS P ON P.PROGRAMME_ID=S.PROGRAM 
                                 INNER JOIN SUP_GENDER AS G ON G.GENDER_ID=S.GENDER
                                 INNER JOIN SUP_OCCUPATION AS O ON O.OCCUPATION_ID=S.OCCUPATION
                                 WHERE S.IS_DELETED!=1 AND S.STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.BindSibling:
                    {
                        query = @"SELECT 
                                                    SIBLING_ID,
                                                    STUDENT_ID,
                                                    NAME,
                                                    AGE,
                                                    INSTITUTE_NAME,
                                                    PROGRAM,
                                                    IS_DELETED,
                                                    QUALIFICATION,
                                                    EMPLOYED,
                                                    DATE_FORMAT(DATE_OF_BIRTH,'%d/%m/%Y') AS DATE_OF_BIRTH,
                                                    GENDER,
                                                    OCCUPATION,
                                                    MONTHLY_INCOME,
                                                    PROGNAME,
                                                    IS_SAME_COLLEGE
                                                FROM STU_SIBLING WHERE SIBLING_ID=?SIBLING_ID;";
                        break;
                    }
                case StudentSQLCommads.DeleteSibling:
                    {
                        query = @"UPDATE STU_SIBLING  SET IS_DELETED=?IS_DELETED WHERE SIBLING_ID=?SIBLING_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchTalents:
                    {
                        query = @"SELECT TALENT_ID,
                                      STUDENT_ID,
                                      DATE_FORMAT(DATE,'%d/%m/%Y') AS DATE,
                                      L.ACTIVITY_LEVEL,
                                      A.ACTIVITY_TYPE,
                                      TALENT_DESCRIPTION,
                                      STATUS,
                                      G.OVERALL_GRADE,
                                      REMARKS,
                                      T.IS_DELETED
                                  FROM STU_TALENTS T
                                  INNER JOIN SUP_OVERALL_GRADE AS G ON G.OVERALL_GRADE_ID=T.GRADE
                                  INNER JOIN SUP_ACTIVITY_TYPE AS A ON A.ACTIVITY_TYPE_ID=T.TALENT_ACTIVITY_TYPE
                                  INNER JOIN SUP_ACTIVITY_LEVEL AS L ON L.ACTIVITY_LEVEL_ID=T.TALENT_AREA
                                  WHERE T.IS_DELETED!=1 AND STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.BindTalents:
                    {
                        query = @"SELECT TALENT_ID,
                                                    STUDENT_ID,
                                                    DATE_FORMAT(DATE,'%d/%m/%Y') AS DATE,
                                                    TALENT_AREA,
                                                    TALENT_ACTIVITY_TYPE,
                                                    TALENT_DESCRIPTION,
                                                    STATUS,
                                                    GRADE,
                                                    REMARKS,
                                                    IS_DELETED
                                                FROM STU_TALENTS WHERE TALENT_ID=?TALENT_ID;";
                        break;
                    }
                case StudentSQLCommads.DeleteTalents:
                    {
                        query = @"UPDATE STU_TALENTS  SET IS_DELETED=?IS_DELETED WHERE TALENT_ID=?TALENT_ID ;";
                        break;
                    }
                case StudentSQLCommads.FetchClgHistory:
                    {
                        query = @"SELECT 
                                                    SCHOOL_ID, 
                                                    STUDENT_ID, 
                                                    AGE, 
                                                    SCHOOL_NAME, 
                                                    GRADE, 
                                                    IS_DELETED, 
                                                    DATE_FORMAT(ENTRY_DATE,'%d/%m/%Y') AS ENTRY_DATE, 
                                                    DATE_FORMAT(EXIT_DATE,'%d/%m/%Y') AS EXIT_DATE, 
                                                    ACADEMIC_LEVELS, 
                                                    CITY, 
                                                    COUNTRY, 
                                                    OFFICIAL_WEBSITE, 
                                                    CURRICULUM, 
                                                    REASON_FOR_WITHDRAWAL, 
                                                    DATE_FORMAT(LAST_ATTENDANCE_DATE,'%d/%m/%Y') AS LAST_ATTENDANCE_DATE 
                                                FROM
                                                    STU_COLLEGE_HISTORY;";
                        break;
                    }
                case StudentSQLCommads.FetchStuIssueEtc:
                    {
                        query = @"SELECT 
                                                    TC_ID, 
                                                    STUDENT_ID, 
                                                    TC_PRODUCED_NUMBER, 
                                                    DATE_FORMAT(TC_PRODUCE_DATE,'%d/%m/%Y') AS TC_PRODUCE_DATE, 
                                                    TC_ISSUED_NUMBER, 
                                                    DATE_FORMAT(TC_ISSUED_DATE,'%d/%m/%Y') AS TC_ISSUED_DATE, 
                                                    IS_DELETED, 
                                                    IS_ISSUED_TC, 
                                                    IS_DUPLICATED, 
                                                    SERIAL_NO, 
                                                    FLAG
                                                FROM
                                                    STU_ISSU_ETC WHERE IS_DELETED!=1 AND STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.BindIssueEtc:
                    {
                        query = @"SELECT 
                                                    TC_ID, 
                                                    STUDENT_ID, 
                                                    TC_PRODUCED_NUMBER, 
                                                    DATE_FORMAT(TC_PRODUCE_DATE,'%d/%m/%Y') AS TC_PRODUCE_DATE, 
                                                    TC_ISSUED_NUMBER, 
                                                    DATE_FORMAT(TC_ISSUED_DATE,'%d/%m/%Y') AS TC_ISSUED_DATE, 
                                                    IS_DELETED, 
                                                    IS_ISSUED_TC, 
                                                    IS_DUPLICATED, 
                                                    SERIAL_NO, 
                                                    FLAG
                                                FROM
                                                    STU_ISSU_ETC WHERE TC_ID=?TC_ID;";
                        break;
                    }
                case StudentSQLCommads.DeleteIssueEtc:
                    {
                        query = @"UPDATE STU_ISSU_ETC  SET IS_DELETED=?IS_DELETED WHERE TC_ID=?TC_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchStuAchievements:
                    {
                        query = @"SELECT 
                                                    ACHIEVE_ID, 
                                                    STUDENT_ID, 
                                                    ACHIEVEMENTS, 
                                                    IS_DELETED, 
                                                    CLASS_ID, 
                                                    DATE_FORMAT(DATE,'%d/%m/%Y')AS DATE, 
                                                    IMAGE_PATH, 
                                                    ACTIVITY
                                                FROM
                                                    STU_ACHIEVEMENTS WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchMedical:
                    {
                        query = @"SELECT 
	                                                MEDICAL_ID,
                                                    STUDENT_ID,
                                                    ALLERGIES,
                                                    DOCTOR_NOTE,
                                                    DATE_FORMAT(MEDICAL_DATE,'%d/%m/%Y')AS MEDICAL_DATE,
                                                    MEDICAL_CONDITION,
                                                    IS_DELETED,
                                                    VACCINATION
                                                FROM STU_MEDICAL WHERE IS_DELETED!=1 AND STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.BindMedical:
                    {
                        query = @"SELECT 
	                                                MEDICAL_ID,
                                                    STUDENT_ID,
                                                    ALLERGIES,
                                                    DOCTOR_NOTE,
                                                    DATE_FORMAT(MEDICAL_DATE,'%d/%m/%Y')AS MEDICAL_DATE,
                                                    MEDICAL_CONDITION,
                                                    IS_DELETED,
                                                    VACCINATION
                                                FROM STU_MEDICAL WHERE MEDICAL_ID=?MEDICAL_ID;";
                        break;
                    }
                case StudentSQLCommads.DeleteMedical:
                    {
                        query = @"UPDATE STU_MEDICAL  SET IS_DELETED=?IS_DELETED WHERE MEDICAL_ID=?MEDICAL_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchLeaveLetter:
                    {
                        query = @"SELECT 
	                                                LEAVE_LETTER_ID,
                                                    STUDENT_ID,
                                                    LEAVE_LETTER_TITLE,
                                                    LEAVE_LETTER_FORMAT,
                                                    IS_ACTIVE,
                                                    IS_DELETED,
                                                    LETTER_FOR
                                                FROM STU_LEAVE_LETTER WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.InsertAcadamicInfo:
                    {
                        query = @"INSERT INTO STU_PERSONAL_INFO(
		                             REGISTER_NO,ROLL_NO,UNIVERSITY_ROLLNO,
                                     ADMISSION_DATE,ADMISSION_NO,
                                     ADMITTED_CLASS,STUDENT_REGISTERDED_DATE,
                                     REMARKS,TC_SERIAL_NO,CLASS_ID,
                                     PROGRAM_ID,BATCH,DEPT_ID,
                                     SHIFT_ID,STU_EMAILID,IS_LEFT)
                                     VALUES
		                            (
		                             ?REGISTER_NO,?ROLL_NO,?UNIVERSITY_ROLLNO,
                                     ?ADMISSION_DATE,?ADMISSION_NO,
                                     ?ADMITTED_CLASS,?STUDENT_REGISTERDED_DATE,
                                     ?REMARKS,?TC_SERIAL_NO,?CLASS_ID,
                                     ?PROGRAM_ID,?BATCH,?DEPT_ID,
                                     ?SHIFT_ID,?STU_EMAILID,?IS_LEFT
		                            );";
                        break;
                    }
                case StudentSQLCommads.InsertStudentAddressDetails:
                    {
                        query = @"INSERT INTO STU_ADDRESS(
                                                STUDENT_ID,C_DOOR_DETAIL,C_STREET,C_VILLAGE_AREA,C_POST_PLACE_TOWN,C_TALUK,C_CITY,C_DISTRICT,C_PINCODE,C_COUNTRY,
                                                P_DOOR_DETAIL,P_STREET,P_VILLAGE_AREA,P_POST_PLACE_TOWN,P_TALUK,P_CITY,P_DISTRICT,P_PINCODE,P_COUNTRY)
                                                VALUES
                                                (
                                                ?STUDENT_ID,?C_DOOR_DETAIL,?C_STREET,?C_VILLAGE_AREA,?C_POST_PLACE_TOWN,?C_TALUK,?C_CITY,?C_DISTRICT,?C_PINCODE,?C_COUNTRY,
                                                ?P_DOOR_DETAIL,?P_STREET,?P_VILLAGE_AREA,?P_POST_PLACE_TOWN,?P_TALUK,?P_CITY,?P_DISTRICT,?P_PINCODE,?P_COUNTRY
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertActivity:
                    {
                        query = @"INSERT INTO STU_ACTIVITIES(
                                                STUDENT_ID,
                                                POST_HELD,
                                                INITIATIVE_SHOWN,
                                                PARTICIPATION,
                                                DATE_FROM,
                                                DATE_TO,
                                                EXTRA_ACTIVITY,
                                                POSITION_OBTAINED,
                                                ACTIVITY)
                                                VALUES
                                                (
                                                   ?STUDENT_ID,?POST_HELD,?INITIATIVE_SHOWN,?PARTICIPATION,?DATE_FROM,?DATE_TO,?EXTRA_ACTIVITY,?POSITION_OBTAINED,?ACTIVITY
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertCdertificate:
                    {
                        query = @"INSERT INTO STU_CERTIFICATE(
                                                STUDENT_ID,
                                                CERTIFICATE_NO,
                                                RECEIVED_DATE,
                                                GIVEN_DATE,
                                                ARCHIVE,
                                                PURPOSE,
                                                REGISTER_NUMBER,
                                                CERTIFICATE_NAME)
                                                VALUES
                                                (
                                                   ?STUDENT_ID,?CERTIFICATE_NO,?RECEIVED_DATE,?GIVEN_DATE,?ARCHIVE,?PURPOSE,?REGISTER_NUMBER,?CERTIFICATE_NAME
                                                )";
                        break;
                    }
                case StudentSQLCommads.InsertParentsInfo:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO 
                                                SET
                                                    FATHER_NAME=?FATHER_NAME,
                                                    F_DATE_OF_BIRTH=?F_DATE_OF_BIRTH,
                                                    FR_OCCUPATION=?FR_OCCUPATION,
                                                    FATHER_EDUCATION=?FATHER_EDUCATION,
                                                    FR_NATIONALITY=?FR_NATIONALITY,
                                                    FR_BUSINESS_PHONE=?FR_BUSINESS_PHONE,
                                                    FR_MOBILE=?FR_MOBILE,
                                                    FR_INCOME=?FR_INCOME,
                                                    FR_DATE_OF_EXPIRY=?FR_DATE_OF_EXPIRY,
                                                    FR_EMAIL=?FR_EMAIL,
                                                    MOTHER_NAME=?MOTHER_NAME,
                                                    M_DATE_OF_BIRTH=?M_DATE_OF_BIRTH,
                                                    MO_OCCUPATION=?MO_OCCUPATION,
                                                    MOTHER_EDUCATION=?MOTHER_EDUCATION,
                                                    MO_NATIONALITY=?MO_NATIONALITY,
                                                    MO_BUSINESS_PHONE=?MO_BUSINESS_PHONE,
                                                    MO_MOBILE=?MO_MOBILE,
                                                    MO_INCOME=?MO_INCOME,
                                                    MO_DATE_OF_EXPIRY=?MO_DATE_OF_EXPIRY,
                                                    MO_EMAIL=?MO_EMAIL
                                                WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.InsertPersonalInfo:
                    {
                        query = @"INSERT INTO STU_PERSONAL_INFO 
                                    (
                                    FIRST_NAME,LAST_NAME,CASTE,DATE_OF_BIRTH,ANNUAL_INCOME
                                    ,RESIDENCE_NO,IDENTIFICATION_MARK1,IDENTIFICATION_MARK2
                                    ,HOME_PHONE,AADHAR_NUMBER,STU_MOBILENO,PLACE_OF_BIRTH
                                    ,MARITAL_STATUS,GENDER,COMMUNITY,MOTHER_TONGUE
                                    ,BLOOD_GROUP,RELIGION,NATIONALITY,FIRST_LANGUAGE
                                    ,SECOND_LANGUAGE,SPECIALLY_ABLED,RESIDENCY_TYPE
                                    ) VALUES
                                    (
                                    ?FIRST_NAME,?LAST_NAME,?CASTE,?DATE_OF_BIRTH,?ANNUAL_INCOME
                                    ,?RESIDENCE_NO,?IDENTIFICATION_MARK1,?IDENTIFICATION_MARK2
                                    ,?HOME_PHONE,?AADHAR_NUMBER,?STU_MOBILENO,?PLACE_OF_BIRTH
                                    ,?MARITAL_STATUS,?GENDER,?COMMUNITY,?MOTHER_TONGUE
                                    ,?BLOOD_GROUP,?RELIGION,?NATIONALITY,?FIRST_LANGUAGE
                                    ,?SECOND_LANGUAGE,?SPECIALLY_ABLED,?RESIDENCY_TYPE
                                    );";
                        break;
                    }
                case StudentSQLCommads.InsertAccountInfo:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO SET 
	                                            ACCOUNT_NO=?ACCOUNT_NO,
                                                IFSC_CODE=?IFSC_CODE,
                                                MICR_CODE=?MICR_CODE
                                            WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.InsertCounseling:
                    {
                        query = @"INSERT INTO STU_COUNSELLING (
                                                DOC,
                                                DURATION,
                                                STUDENT_ID,
                                                REMARKS,
                                                COMMENTS,
                                                BATCH )
                                                VALUES 
                                                (
                                                   ?DOC,?DURATION,?STUDENT_ID,?REMARKS,?COMMENTS,?BATCH
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertCourses:
                    {
                        query = @"INSERT INTO STU_COURSES
                                                    (PROGRAM_ID,
                                                    STUDENT_ID,
                                                    CLASS_ID,
                                                    PART,
                                                    MAIN_SUBJECT,
                                                    ALLIED1,
                                                    ALLIED2,
                                                    ALLIED3,
                                                    ALLIED4,
                                                    ELECTIVE1,
                                                    ELECTIVE2,
                                                    ELECTIVE3,
                                                    ELECTIVE4,
                                                    SPECIAL_SUBJECT)
                                                 VALUES
                                                    (
                                                        ?PROGRAM_ID,?STUDENT_ID,?CLASS_ID,?PART,?MAIN_SUBJECT,?ALLIED1,?ALLIED2,?ALLIED3,?ALLIED4,
                                                        ?ELECTIVE1,?ELECTIVE2,?ELECTIVE3,?ELECTIVE4,?SPECIAL_SUBJECT
                                                    );";
                        break;
                    }
                case StudentSQLCommads.InsertIncident:
                    {
                        query = @"INSERT INTO STU_INCIDENT(
                                                STUDENT_ID,
                                                DATE_OF_INCIDENT,
                                                TIME_OF_INCIDENT,
                                                PLACE_OF_INCIDENT,
                                                FIRST_AID_DONE,
                                                INFORMED_TO_PARENTS,
                                                DATE_INFORMED,
                                                INCIDENT_DETAILS)
                                                VALUES
                                                (
                                                    ?STUDENT_ID,?DATE_OF_INCIDENT,?TIME_OF_INCIDENT,?PLACE_OF_INCIDENT,?FIRST_AID_DONE,?INFORMED_TO_PARENTS,?DATE_INFORMED,?INCIDENT_DETAILS
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertSibling:
                    {
                        query = @"INSERT INTO STU_SIBLING(
                                                STUDENT_ID,NAME,AGE,
                                                INSTITUTE_NAME,PROGRAM,
                                                QUALIFICATION,EMPLOYED,
                                                DATE_OF_BIRTH,GENDER,
                                                OCCUPATION,MONTHLY_INCOME,
                                                PROGNAME,IS_SAME_COLLEGE)
                                                VALUES
                                                (
                                                   ?STUDENT_ID,?NAME,?AGE,
                                                   ?INSTITUTE_NAME,?PROGRAM,
                                                   ?QUALIFICATION,?EMPLOYED,
                                                   ?DATE_OF_BIRTH,?GENDER,
                                                   ?OCCUPATION,?MONTHLY_INCOME,
                                                   ?PROGNAME,?IS_SAME_COLLEGE
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertTalents:
                    {
                        query = @"INSERT INTO STU_TALENTS(
                                                STUDENT_ID,DATE,TALENT_AREA,
                                                TALENT_ACTIVITY_TYPE,TALENT_DESCRIPTION,
                                                STATUS,GRADE,REMARKS)
                                                VALUES
                                                (
                                                   ?STUDENT_ID,?DATE,?TALENT_AREA,
                                                   ?TALENT_ACTIVITY_TYPE,?TALENT_DESCRIPTION,
                                                   ?STATUS,?GRADE,?REMARKS
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertClgHistory:
                    {
                        query = @"INSERT INTO STU_COLLEGE_HISTORY( 
                                                    STUDENT_ID, AGE, SCHOOL_NAME, 
                                                    GRADE, ENTRY_DATE, EXIT_DATE, 
                                                    ACADEMIC_LEVELS, CITY, COUNTRY, 
                                                    OFFICIAL_WEBSITE, CURRICULUM, 
                                                    REASON_FOR_WITHDRAWAL, LAST_ATTENDANCE_DATE)
                                                VALUES
                                                (
                                                    ?STUDENT_ID,?AGE,?SCHOOL_NAME,
                                                    ?GRADE,?ENTRY_DATE,?EXIT_DATE,
                                                    ?ACADEMIC_LEVELS,?CITY,?COUNTRY,
                                                    ?OFFICIAL_WEBSITE,?CURRICULUM,
                                                    ?REASON_FOR_WITHDRAWAL,?LAST_ATTENDANCE_DATE
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertStuIssuEtc:
                    {
                        query = @"INSERT INTO STU_ISSU_ETC( 
                                                    STUDENT_ID, 
                                                    TC_PRODUCED_NUMBER, 
                                                    TC_PRODUCE_DATE, 
                                                    TC_ISSUED_NUMBER, 
                                                    TC_ISSUED_DATE)
                                                VALUE 
                                                (
                                                   ?STUDENT_ID,?TC_PRODUCED_NUMBER,?TC_PRODUCE_DATE,?TC_ISSUED_NUMBER,?TC_ISSUED_DATE
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertStuAchievements:
                    {
                        query = @"INSERT INTO STU_ACHIEVEMENTS( 
	                                                STUDENT_ID, 
                                                    ACHIEVEMENTS,  
                                                    CLASS_ID, 
	                                                DATE, 
                                                    IMAGE_PATH, 
                                                    ACTIVITY)
                                                VALUES
                                                (
                                                   ?STUDENT_ID,?ACHIEVEMENTS,?CLASS_ID,?DATE,?IMAGE_PATH,?ACTIVITY
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertMedical:
                    {
                        query = @"INSERT INTO STU_MEDICAL(
                                                STUDENT_ID,ALLERGIES,
                                                DOCTOR_NOTE,MEDICAL_DATE,
                                                MEDICAL_CONDITION,VACCINATION)
                                                VALUES
                                                (
                                                    ?STUDENT_ID,?ALLERGIES,
                                                    ?DOCTOR_NOTE,?MEDICAL_DATE,
                                                    ?MEDICAL_CONDITION,?VACCINATION
                                                );";
                        break;
                    }
                case StudentSQLCommads.InsertLeaveLetter:
                    {
                        query = @"INSERT INTO STU_LEAVE_LETTER(
                                                        STUDENT_ID,
                                                        LEAVE_LETTER_TITLE,
                                                        LEAVE_LETTER_FORMAT,
                                                        LETTER_FOR)
                                                    VALUES
                                                    (
                                                        ?STUDENT_ID,?LEAVE_LETTER_TITLE,?LEAVE_LETTER_FORMAT,?LETTER_FOR 
                                                    );";
                        break;
                    }
                case StudentSQLCommads.InsertTransferCertificate:
                    {
                        query = @"INSERT INTO STU_TRANSFER_CERTIFICATE_?AC_YEAR(
                                STUDENT_ID,SERIAL_NO,ADMISSION_NO,FIRST_NAME,GENDER,
                                RELIGION,NATIONALITY,CASTE,COMMUNITY,
                                DATE_OF_BIRTH,ADMISSION_DATE,ADMITTED_CLASS,
                                IDENTIFICATION_MARK1,IDENTIFICATION_MARK2,MAIN_COURSE,ALLIED,
                                FEE_PAID,SCHOLAR_SHIP,LEAVING_DATE,
                                LEAVING_CLASS,CONDUCT,
                                TC_APPLIED_DATE,TC_GIVEN_DATE,FATHER_NAME,
                                GUARDIAN_NAME,ACADEMIC_YEARS,CLASSES_STUDEIED,FIRST_LANGUAGE,DATETOWORDS)VALUES
                                (
                                ?STUDENT_ID,?SERIAL_NO,?ADMISSION_NO,?FIRST_NAME,?GENDER,
                                ?RELIGION,?NATIONALITY,?CASTE,?COMMUNITY,
                                ?DATE_OF_BIRTH,?ADMISSION_DATE,?ADMITTED_CLASS,
                                ?IDENTIFICATION_MARK1,?IDENTIFICATION_MARK2,?MAIN_COURSE,?ALLIED,
                                ?FEE_PAID,?SCHOLAR_SHIP,?LEAVING_DATE,
                                ?LEAVING_CLASS,?CONDUCT,
                                ?TC_APPLIED_DATE,?TC_GIVEN_DATE,?FATHER_NAME,
                                ?GUARDIAN_NAME,?ACADEMIC_YEARS,?CLASSES_STUDEIED,?FIRST_LANGUAGE,?DATETOWORDS);";
                        break;
                    }
                case StudentSQLCommads.UpdateTransferCertificate:
                    {
                        query = @"UPDATE STU_TRANSFER_CERTIFICATE_?AC_YEAR
                                    SET
                                    STUDENT_ID =?STUDENT_ID,
                                    FIRST_NAME =?FIRST_NAME,
                                    GENDER =?GENDER,
                                    RELIGION =?RELIGION,
                                    NATIONALITY =?NATIONALITY,
                                    CASTE =?CASTE,
                                    COMMUNITY =?COMMUNITY,
                                    DATE_OF_BIRTH =?DATE_OF_BIRTH,
                                    ADMISSION_DATE =?ADMISSION_DATE,
                                    ADMITTED_CLASS =?ADMITTED_CLASS,
                                    IDENTIFICATION_MARK1 =?IDENTIFICATION_MARK1,
                                    IDENTIFICATION_MARK2 =?IDENTIFICATION_MARK2,
                                    MAIN_COURSE =?MAIN_COURSE,
                                    ALLIED =?ALLIED,
                                    FEE_PAID =?FEE_PAID,
                                    SCHOLAR_SHIP =?SCHOLAR_SHIP,
                                    LEAVING_DATE =?LEAVING_DATE,
                                    LEAVING_CLASS =?LEAVING_CLASS,
                                    CONDUCT =?CONDUCT,
                                    TC_APPLIED_DATE =?TC_APPLIED_DATE,
                                    TC_GIVEN_DATE =?TC_GIVEN_DATE,
                                    FATHER_NAME =?FATHER_NAME,
                                    GUARDIAN_NAME =?GUARDIAN_NAME,
                                    ACADEMIC_YEARS =?ACADEMIC_YEARS,
                                    CLASSES_STUDEIED =?CLASSES_STUDEIED,
                                    FIRST_LANGUAGE =?FIRST_LANGUAGE,
                                    DATETOWORDS =?DATETOWORDS
                                    WHERE CERTIFICATE_ID =?CERTIFICATE_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentsForEdit:
                    {
                        query = @"SELECT 
                                    CERTIFICATE_ID, 
                                    SERIAL_NO, 
                                    ADMISSION_NO, 
                                    STUDENT_ID, 
                                    FIRST_NAME,
                                    LAST_NAME, 
                                    GENDER, 
                                    RELIGION, 
                                    NATIONALITY, 
                                    CASTE, 
                                    COMMUNITY, 
                                    DATE_FORMAT(DATE_OF_BIRTH,'%d/%m/%Y')AS 'DATE_OF_BIRTH', 
									DATE_FORMAT(ADMISSION_DATE,'%d/%m/%Y')AS ADMISSION_DATE, 
                                    ADMITTED_CLASS, 
                                    IDENTIFICATION_MARK1, 
                                    IDENTIFICATION_MARK2, 
                                    MAIN_COURSE, ALLIED, 
                                    FEE_PAID, 
                                    SCHOLAR_SHIP, 
                                    DATE_FORMAT(LEAVING_DATE,'%d/%m/%Y')AS LEAVING_DATE, 
                                    REASON_FOR_LEAVING, 
                                    LEAVING_CLASS, CONDUCT, 
                                    DATE_FORMAT(TC_APPLIED_DATE,'%d/%m/%Y')AS TC_APPLIED_DATE, 
                                    DATE_FORMAT(TC_GIVEN_DATE,'%d/%m/%Y')AS TC_GIVEN_DATE, 
                                    FATHER_NAME, 
                                    GUARDIAN_NAME, 
                                    ACADEMIC_YEARS, 
                                    CLASSES_STUDEIED, 
                                    FIRST_LANGUAGE, 
                                    DATETOWORDS
                                FROM
                                    STU_TRANSFER_CERTIFICATE_?AC_YEAR WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateAcadamicInfo:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO SET
                                    REGISTER_NO=?REGISTER_NO,
                                    ROLL_NO=?ROLL_NO,
                                    UNIVERSITY_ROLLNO=?UNIVERSITY_ROLLNO,
                                    ADMISSION_DATE=?ADMISSION_DATE,
                                    ADMISSION_NO=?ADMISSION_NO,
                                    ADMITTED_CLASS=?ADMITTED_CLASS,
                                    STUDENT_REGISTERDED_DATE=?STUDENT_REGISTERDED_DATE,
                                    REMARKS=?REMARKS,
                                    TC_SERIAL_NO=?TC_SERIAL_NO,
                                    CLASS_ID=?CLASS_ID,
                                    PROGRAM_ID=?PROGRAM_ID,
                                    BATCH=?BATCH,
                                    DEPT_ID=?DEPT_ID,
                                    SHIFT_ID=?SHIFT_ID,
                                    STU_EMAILID=?STU_EMAILID,
                                    IS_LEFT=?IS_LEFT
                                WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdatePersonalInfo:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO SET
		                                            FIRST_NAME=?FIRST_NAME,
                                                    LAST_NAME=?LAST_NAME,
                                                    CASTE=?CASTE,
		                                            DATE_OF_BIRTH=?DATE_OF_BIRTH,
                                                    ANNUAL_INCOME=?ANNUAL_INCOME,
                                                    RESIDENCE_NO=?RESIDENCE_NO,
                                                    IDENTIFICATION_MARK1=?IDENTIFICATION_MARK1,
                                                    IDENTIFICATION_MARK2=?IDENTIFICATION_MARK2,
                                                    HOME_PHONE=?HOME_PHONE,
                                                    AADHAR_NUMBER=?AADHAR_NUMBER,
                                                    STU_MOBILENO=?STU_MOBILENO,
                                                    PLACE_OF_BIRTH=?PLACE_OF_BIRTH,
                                                    MARITAL_STATUS=?MARITAL_STATUS,
                                                    GENDER=?GENDER,
                                                    COMMUNITY=?COMMUNITY,
                                                    MOTHER_TONGUE=?MOTHER_TONGUE,
                                                    BLOOD_GROUP=?BLOOD_GROUP,
		                                            RELIGION=?RELIGION,
                                                    NATIONALITY=?NATIONALITY
                                                 WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateParentsInfo:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO 
                                                SET
                                                    FATHER_NAME=?FATHER_NAME,
                                                    F_DATE_OF_BIRTH=?F_DATE_OF_BIRTH,
                                                    FR_OCCUPATION=?FR_OCCUPATION,
                                                    FATHER_EDUCATION=?FATHER_EDUCATION,
                                                    FR_NATIONALITY=?FR_NATIONALITY,
                                                    FR_BUSINESS_PHONE=?FR_BUSINESS_PHONE,
                                                    FR_MOBILE=?FR_MOBILE,
                                                    FR_INCOME=?FR_INCOME,
                                                    FR_DATE_OF_EXPIRY=?FR_DATE_OF_EXPIRY,
                                                    FR_EMAIL=?FR_EMAIL,
                                                    MOTHER_NAME=?MOTHER_NAME,
                                                    M_DATE_OF_BIRTH=?M_DATE_OF_BIRTH,
                                                    MO_OCCUPATION=?MO_OCCUPATION,
                                                    MOTHER_EDUCATION=?MOTHER_EDUCATION,
                                                    MO_NATIONALITY=?MO_NATIONALITY,
                                                    MO_BUSINESS_PHONE=?MO_BUSINESS_PHONE,
                                                    MO_MOBILE=?MO_MOBILE,
                                                    MO_INCOME=?MO_INCOME,
                                                    MO_DATE_OF_EXPIRY=?MO_DATE_OF_EXPIRY,
                                                    MO_EMAIL=?MO_EMAIL
                                                WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateAddress:
                    {
                        query = @"Update STU_ADDRESS set
                                               C_DOOR_DETAIL=?C_DOOR_DETAIL,
                                               C_STREET=?C_STREET,
                                               C_VILLAGE_AREA=?C_VILLAGE_AREA,
                                               C_POST_PLACE_TOWN=?C_POST_PLACE_TOWN,
                                               C_TALUK=?C_TALUK,
                                               C_CITY=?C_CITY,
                                               C_DISTRICT=?C_DISTRICT,
                                               C_PINCODE=?C_PINCODE,
                                               C_COUNTRY=?C_COUNTRY,
                                               P_DOOR_DETAIL=?P_DOOR_DETAIL,
                                               P_STREET=?P_STREET,
                                               P_VILLAGE_AREA=?P_VILLAGE_AREA,
                                               P_POST_PLACE_TOWN=?P_POST_PLACE_TOWN,
                                               P_TALUK=?P_TALUK,
                                               P_CITY=?P_CITY,
                                               P_DISTRICT=?P_DISTRICT,
                                               P_PINCODE=?P_PINCODE,
                                               P_COUNTRY=?P_COUNTRY
                                            WHERE ADDRESS_ID=?ADDRESS_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateActivity:
                    {
                        query = @"UPDATE STU_ACTIVITIES SET
                                                  POST_HELD=?POST_HELD,
                                                  INITIATIVE_SHOWN=?INITIATIVE_SHOWN,
                                                  PARTICIPATION=?PARTICIPATION,
                                                  DATE_FROM=?DATE_FROM,
                                                  DATE_TO=?DATE_TO,
                                                  EXTRA_ACTIVITY=?EXTRA_ACTIVITY,
                                                  POSITION_OBTAINED=?POSITION_OBTAINED,
                                                  ACTIVITY=?ACTIVITY
                                                WHERE ACTIVITY_ID=?ACTIVITY_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateCertificate:
                    {
                        query = @"UPDATE STU_CERTIFICATE SET
                                                    CERTIFICATE_NO=?CERTIFICATE_NO,
                                                    RECEIVED_DATE=?RECEIVED_DATE,
                                                    GIVEN_DATE=?GIVEN_DATE,
                                                    ARCHIVE=?ARCHIVE,
                                                    PURPOSE=?PURPOSE,
                                                    REGISTER_NUMBER=?REGISTER_NUMBER,
                                                    CERTIFICATE_NAME=?CERTIFICATE_NAME
                                                WHERE CERTIFICATE_ID=?CERTIFICATE_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateAccountInfo:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO SET 
	                                            ACCOUNT_NO=?ACCOUNT_NO,
                                                IFSC_CODE=?IFSC_CODE,
                                                MICR_CODE=?MICR_CODE
                                            WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateCounseling:
                    {
                        query = @"UPDATE STU_COUNSELLING
                                                SET
                                                DOC =?DOC,
                                                DURATION =?DURATION,
                                                REMARKS =?REMARKS,
                                                COMMENTS =?COMMENTS,
                                                BATCH =?BATCH,
                                                STUDENT_ID=?STUDENT_ID
                                                WHERE SC_ID =?SC_ID;";
                        break;
                    }
                case StudentSQLCommads.DeleteCounselling:
                    {
                        query = @"UPDATE STU_COUNSELLING  SET IS_DELETED=?IS_DELETED WHERE SC_ID=?SC_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateCourses:
                    {
                        query = @"UPDATE STU_COURSES SET
                                                    PROGRAM_ID=?PROGRAM_ID,
                                                    CLASS_ID=?CLASS_ID,
                                                    PART=?PART,
                                                    MAIN_SUBJECT=?MAIN_SUBJECT,
                                                    ALLIED1=?ALLIED1,
                                                    ALLIED2=?ALLIED2,
                                                    ALLIED3=?ALLIED3,
                                                    ALLIED4=?ALLIED4,
                                                    ELECTIVE1=?ELECTIVE1,
                                                    ELECTIVE2=?ELECTIVE2,
                                                    ELECTIVE3=?ELECTIVE3,
                                                    ELECTIVE4=?ELECTIVE4,
                                                    SPECIAL_SUBJECT=SPECIAL_SUBJECT
                                                WHERE COURSE_REGISTRATION_ID=?COURSE_REGISTRATION_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateIncident:
                    {
                        query = @"UPDATE STU_INCIDENT SET
                                                    STUDENT_ID=?STUDENT_ID,
                                                    DATE_OF_INCIDENT=?DATE_OF_INCIDENT,
                                                    TIME_OF_INCIDENT=?TIME_OF_INCIDENT,
                                                    PLACE_OF_INCIDENT=?PLACE_OF_INCIDENT,
                                                    FIRST_AID_DONE=?FIRST_AID_DONE,
                                                    INFORMED_TO_PARENTS=?INFORMED_TO_PARENTS,
                                                    DATE_INFORMED=?DATE_INFORMED,
                                                    INCIDENT_DETAILS=?INCIDENT_DETAILS
                                                WHERE INCIDENT_ID=?INCIDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.BindIncident:
                    {
                        query = @"SELECT 
                                                INCIDENT_ID,
                                                STUDENT_ID,
                                                DATE_FORMAT(DATE_OF_INCIDENT,'%d/%m/%Y') AS DATE_OF_INCIDENT,
                                                TIME_OF_INCIDENT,
                                                PLACE_OF_INCIDENT,
                                                FIRST_AID_DONE,
                                                INFORMED_TO_PARENTS,
                                                DATE_FORMAT(DATE_INFORMED,'%d/%m/%Y') AS DATE_INFORMED,
                                                INCIDENT_DETAILS
                                            FROM
                                                STU_INCIDENT WHERE INCIDENT_ID=?INCIDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.DeleteIncident:
                    {
                        query = @"UPDATE STU_INCIDENT SET IS_DELETED=?IS_DELETED WHERE INCIDENT_ID=?INCIDENT_ID; ";
                        break;
                    }
                case StudentSQLCommads.UpdateSibling:
                    {
                        query = @"UPDATE STU_SIBLING SET
                                                STUDENT_ID=?STUDENT_ID,
                                                NAME=?NAME,
                                                AGE=?AGE,
                                                INSTITUTE_NAME=?INSTITUTE_NAME,
                                                PROGRAM=?PROGRAM,
                                                QUALIFICATION=?QUALIFICATION,
                                                EMPLOYED=?EMPLOYED,
                                                DATE_OF_BIRTH=?DATE_OF_BIRTH,
                                                GENDER=?GENDER,
                                                OCCUPATION=?OCCUPATION,
                                                MONTHLY_INCOME=?MONTHLY_INCOME,
                                                PROGNAME=?PROGNAME,
                                                IS_SAME_COLLEGE=?IS_SAME_COLLEGE WHERE SIBLING_ID=?SIBLING_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateTalents:
                    {
                        query = @"UPDATE STU_TALENTS
                                                SET
                                                STUDENT_ID =?STUDENT_ID,
                                                DATE =?DATE,
                                                TALENT_AREA =?TALENT_AREA,
                                                TALENT_ACTIVITY_TYPE =?TALENT_ACTIVITY_TYPE,
                                                TALENT_DESCRIPTION =?TALENT_DESCRIPTION,
                                                STATUS =?STATUS,
                                                GRADE =?GRADE,
                                                REMARKS =?REMARKS
                                                WHERE TALENT_ID =?TALENT_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateClgHistory:
                    {
                        query = @"UPDATE STU_COLLEGE_HISTORY SET
                                                    AGE=?AGE, 
                                                    SCHOOL_NAME=?SCHOOL_NAME, 
                                                    GRADE=?GRADE, 
                                                    ENTRY_DATE=?ENTRY_DATE, 
                                                    EXIT_DATE=?EXIT_DATE, 
                                                    ACADEMIC_LEVELS=?ACADEMIC_LEVELS, 
                                                    CITY=?CITY, 
                                                    COUNTRY=?COUNTRY, 
                                                    OFFICIAL_WEBSITE=?OFFICIAL_WEBSITE, 
                                                    CURRICULUM=?CURRICULUM, 
                                                    REASON_FOR_WITHDRAWAL=?REASON_FOR_WITHDRAWAL, 
                                                    LAST_ATTENDANCE_DATE=?LAST_ATTENDANCE_DATE
                                                WHERE SCHOOL_ID=?SCHOOL_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateStuissueEtc:
                    {
                        query = @"UPDATE STU_ISSU_ETC SET
                                                    STUDENT_ID=?STUDENT_ID, 
                                                    TC_PRODUCED_NUMBER=?TC_PRODUCED_NUMBER, 
                                                    TC_PRODUCE_DATE=?TC_PRODUCE_DATE, 
                                                    TC_ISSUED_NUMBER=?TC_ISSUED_NUMBER, 
                                                    TC_ISSUED_DATE=?TC_ISSUED_DATE
                                                WHERE TC_ID=?TC_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateStuAchievements:
                    {
                        query = @"UPDATE STU_ACHIEVEMENTS SET
	                                                STUDENT_ID=?STUDENT_ID, 
                                                    ACHIEVEMENTS=?ACHIEVEMENTS,  
                                                    CLASS_ID=?CLASS_ID, 
	                                                DATE=?DATE, 
                                                    IMAGE_PATH=?IMAGE_PATH, 
                                                    ACTIVITY=?ACTIVITY
                                                WHERE ACHIEVE_ID=?ACHIEVE_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateMedical:
                    {
                        query = @"UPDATE STU_MEDICAL SET
	                                                STUDENT_ID=?STUDENT_ID,
	                                                ALLERGIES=?ALLERGIES,
	                                                DOCTOR_NOTE=?DOCTOR_NOTE,
	                                                MEDICAL_DATE=?MEDICAL_DATE,
	                                                MEDICAL_CONDITION=?MEDICAL_CONDITION,
	                                                VACCINATION=?VACCINATION
                                                WHERE MEDICAL_ID=?MEDICAL_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateLeaveLetter:
                    {
                        query = @"UPDATE STU_LEAVE_LETTER SET
                                                    STUDENT_ID=?STUDENT_ID,
                                                    LEAVE_LETTER_TITLE=?LEAVE_LETTER_TITLE,
                                                    LEAVE_LETTER_FORMAT=?LEAVE_LETTER_FORMAT,
                                                    LETTER_FOR=?LETTER_FOR
                                                WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdatePriviousQualification:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO SET
	                                LAST_SCHOOL_OR_COLLEGE=?LAST_SCHOOL_OR_COLLEGE,
                                    LAST_STUDIED_PLACE=?LAST_STUDIED_PLACE,
                                    LAST_STUDIED_CLASS=?LAST_STUDIED_CLASS,
                                    EXAM_PASSED=?EXAM_PASSED
                                WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.DeleteStudent:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO SET IS_DELETED=?IS_DELETED WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentByClassId:
                    {
                        query = @"SELECT 
                                        SP.STUDENT_ID,
                                        DT.DEPARTMENT,
                                        SP.ROLL_NO,
                                        SP.FIRST_NAME,
                                        DATE_FORMAT(SP.DATE_OF_BIRTH,'%d/%m/%Y')AS 'DATE_OF_BIRTH'
                                    FROM
                                        STU_PERSONAL_INFO AS SP
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.STUDENT_ID = SP.STUDENT_ID
                                            INNER JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DT ON DT.DEPARTMENT_ID = SP.DEPT_ID WHERE SC.CLASS_ID=?CLASS_ID AND SP.IS_DELETED!=1;";
                        break;
                    }
                case StudentSQLCommads.FetchStuclassByStudentIdAndAcademicYear:
                    {
                        query = @"SELECT 
                                        STU_CLASS_ID
                                    FROM
                                        STU_CLASS
                                    WHERE
                                        ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                            AND STUDENT_ID = ?STUDENT_ID";
                        break;
                    }
                case StudentSQLCommads.UpdateStuclass:
                    {
                        query = @"UPDATE STU_CLASS
                                        SET
                                        STUDENT_ID= ?STUDENT_ID,
                                        ACADEMIC_YEAR= ?ACADEMIC_YEAR,
                                        CLASS_ID= ?CLASS_ID
                                        WHERE STU_CLASS_ID= ?STU_CLASS_ID;";
                        break;
                    }
                case StudentSQLCommads.SaveStuClass:
                    {
                        query = @"INSERT INTO STU_CLASS
                                            (
                                            STUDENT_ID,
                                            ACADEMIC_YEAR,
                                            CLASS_ID
                                            )
                                            VALUES(
                                            ?STUDENT_ID,
                                            ?ACADEMIC_YEAR,
                                            ?CLASS_ID);";
                        break;
                    }
                case StudentSQLCommads.FetchClassesByProgrammeId:
                    {
                        query = @"SELECT 
                                        CLASS_ID, CLASS_CODE
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        SHIFT = ?SHIFT AND PROGRAMME = ?PROGRAMME;";
                        break;
                    }
                case StudentSQLCommads.FetchStuClassByClassId:
                    {
                        query = @"SELECT 
                                        STU_CLASS_ID, STUDENT_ID, ACADEMIC_YEAR, CLASS_ID
                                    FROM
                                        STU_CLASS
                                    WHERE
                                        CLASS_ID = ?CLASS_ID AND ACADEMIC_YEAR = ?ACADEMIC_YEAR AND IS_DELETED!=1;";
                        break;
                    }
                case StudentSQLCommads.FetchStuCourseInfoByStudentIdAndAcademicYear:
                    {
                        query = @"SELECT 
                                        SC.COURSE_INFO_ID
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR AS SC
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS AR ON AR.SEMESTER = SC.SEMESTER
                                            AND AR.IS_ACTIVE = 1
                                    WHERE
                                        SC.STUDENT_ID = ?STUDENT_ID
                                    GROUP BY SC.COURSE_INFO_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateStudentDepartment:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO SET DEPT_ID=?DEPARTMENT WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateStuCourseInfo:
                    {
                        query = @"UPDATE STU_COURSE_INFO_?AC_YEAR
                                        SET 
                                            COURSE_ID = ?COURSE_ID,
                                            S_CLASS_ID=?S_CLASS_ID
                                        WHERE
                                            COURSE_INFO_ID = ?COURSE_INFO_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchCurrentProgrammeByStudentId:
                    {
                        query = @"SELECT 
                                        CLS.PROGRAMME
                                    FROM
                                        STU_CLASS AS SC
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = SC.CLASS_ID
                                    WHERE
                                        SC.STUDENT_ID = ?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchTransferedProgrammeByStudentId:
                    {
                        query = @"SELECT 
                                        PROGRAMME
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentCIAMarks:
                    {
                        query = @"SELECT 
                                        STUDENT_ID
                                    FROM
                                        CIA_GROUP_MARKS_?AC_YEAR
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchShiftByClassId:
                    {
                        query = @"SELECT 
                                        SHIFT
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateStudentPersonalInfoForStudentTransfer:
                    {
                        query = @"UPDATE STU_PERSONAL_INFO
                                        SET
                                        PROGRAM_ID = ?PROGRAMME,
                                        SHIFT_ID = ?SHIFT
                                        WHERE STUDENT_ID = ?STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.UpdateStuCourseInfoForStudentTransferOnlyLanguagePaper:
                    {
                        query = @"UPDATE STU_COURSE_INFO_?AC_YEAR
                                        SET
                                        CLASS_ID = ?CLASS_ID,
                                        S_CLASS_ID = ?CLASS_ID
                                        WHERE 
                                        STUDENT_ID=?STUDENT_ID AND CLASS_ID=S_CLASS_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchStuCourseInfobyClassId:
                    {
                        query = @"SELECT 
                                        SCI.COURSE_INFO_ID,
                                        SP.STUDENT_ID,
                                        SP.ROLL_NO,
                                        SP.FIRST_NAME,
                                        SCI.COURSE_ID,
                                        CONCAT(IFNULL(CR.COURSE_TITLE, ''),
                                                '(',
                                                IFNULL(CR.COURSE_CODE, ''),
                                                ')') AS COURSE_TITLE
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR AS SCI
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SCI.STUDENT_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = SCI.COURSE_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE ON ASE.SEMESTER = SCI.SEMESTER
                                            AND ASE.IS_ACTIVE = 1
                                    WHERE
                                        SCI.CLASS_ID = ?CLASS_ID
                                            AND ASE.SEMESTER = SCI.SEMESTER AND SCI.IS_DELETED!=1
                                    GROUP BY SCI.STUDENT_ID,SCI.COURSE_ID
                                    ORDER BY SCI.STUDENT_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchCourseListForStuCourseInfo:
                    {
                        query = @"SELECT 
                                        COURSE_ROOT_ID,CONCAT(IFNULL(COURSE_CODE, ''),
                                                '(',
                                                IFNULL(COURSE_TITLE, ''),
                                                ')') AS COURSE_TITLE
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR
                                    WHERE
                                        IS_COMPULSORY != 1;";
                        break;
                    }
                case StudentSQLCommads.BindStudentPersonalInfoByClassId:
                    {
                        query = @"SELECT 
                                        SP.STUDENT_ID,
                                        SP.FIRST_NAME,
                                        SP.ROLL_NO
                                    FROM
                                        STU_PERSONAL_INFO AS SP
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.STUDENT_ID = SP.STUDENT_ID
                                    WHERE
                                        SC.CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case StudentSQLCommads.BindSelectedStudentsByCourseId:
                    {
                        query = @"SELECT 
                                        SP.STUDENT_ID,
                                        SP.FIRST_NAME,
                                        SP.ROLL_NO
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR AS SC
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SC.STUDENT_ID = SP.STUDENT_ID
                                    WHERE
                                        SC.S_CLASS_ID = ?S_CLASS_ID AND SC.COURSE_ID=?COURSE_ID AND SC.CLASS_ID=?CLASS_ID AND SC.IS_DELETED!=1 AND SP.IS_DELETED!=1;";
                        break;
                    }
                case StudentSQLCommads.FetchSelectedClassIdByCourseIdAndShiftId:
                    {
                        query = @"SELECT 
                                        CLASS_ID
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        SHIFT = ?SHIFT AND COURSE_ID = ?COURSE_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentCourseByStudentId:
                    {
                        query = @"SELECT 
                                        SC.COURSE_ID,
                                        SC.COURSE_INFO_ID
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR AS SC
                                            INNER JOIN
										CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID=SC.COURSE_ID AND CR.IS_NME_SUBJECT=1
											INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE ON ASE.SEMESTER = CR.SEMESTER_ID
                                            AND ASE.IS_ACTIVE = 1
                                    WHERE
                                        SC.STUDENT_ID = ?STUDENT_ID AND SC.IS_DELETED!=1
                                    GROUP BY SC.COURSE_ID";
                        break;
                    }
                case StudentSQLCommads.FetchStudentCourseByCourseIdAndStudentId:
                    {
                        query = @"SELECT 
                                        S_CLASS_ID
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID AND CLASS_ID = ?CLASS_ID
                                            AND COURSE_ID = ?COURSE_ID AND IS_DELETED!=1;";
                        break;
                    }
                case StudentSQLCommads.FetchStudentCIAMarksByStudentIdAndCourseId:
                    {
                        query = @"SELECT 
                                        STUDENT_ID
                                    FROM
                                        CIA_GROUP_MARKS_?AC_YEAR
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID AND COURSE_ID=?COURSE_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchProgrammeAndBatchByClassId:
                    {
                        query = @"SELECT 
                                        CLS.PROGRAMME, SP.BATCH
                                    FROM
                                        CP_CLASSES_?AC_YEAR AS CLS
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = CLS.PROGRAMME
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.CLASS_ID = CLS.CLASS_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                    WHERE
                                        CLS.CLASS_ID = ?CLASS_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                    GROUP BY SC.CLASS_ID";
                        break;
                    }
                case StudentSQLCommads.FetchCourseByProgrammeAndBatch:
                    {
                        query = @"SELECT 
                                        CR.COURSE_ROOT_ID,CONCAT(IFNULL(CR.COURSE_TITLE, ''),
                                                '(',
                                                IFNULL(CR.COURSE_CODE, ''),
                                                ')') AS COURSE_TITLE
                                    FROM
                                        ACADEMIC_CURRICULUM_?AC_YEAR AS AC
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = AC.COURSE_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE ON ASE.PROGRAMME = AC.PROGRAMME
                                    WHERE
                                        AC.BATCH = ?BATCH AND AC.PROGRAMME = ?PROGRAMME
                                            AND AC.SEMESTER = ASE.SEMESTER AND ASE.IS_ACTIVE=1
                                    GROUP BY AC.COURSE_ID";
                        break;
                    }
                case StudentSQLCommads.FetchClassCourseByCourseIdAndClassId:
                    {
                        query = @"SELECT 
                                        CLASS_COURSE_ID
                                    FROM
                                        CP_CLASS_COURSE_?AC_YEAR AS CC
                                    WHERE
                                        COURSE_ID = ?COURSE_ID AND CLASS_ID = ?CLASS_ID
                                            AND IS_DELETED != 1";
                        break;
                    }
                case StudentSQLCommads.CheckIsNMECourse:
                    {
                        query = @"SELECT 
                                        COURSE_ROOT_ID
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR
                                    WHERE
                                        IS_NME_SUBJECT = 1
                                            AND COURSE_ROOT_ID = ?COURSE_ID;";
                        break;
                    }
                case StudentSQLCommads.FetchLanguagePaperByStudentId:
                    {
                        query = @"SELECT 
                                        SC.COURSE_ID,
                                        SC.COURSE_INFO_ID
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR AS SC
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = SC.COURSE_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE ON ASE.SEMESTER = CR.SEMESTER_ID
                                            AND ASE.IS_ACTIVE = 1
                                    WHERE
                                        SC.STUDENT_ID = ?STUDENT_ID
                                            AND SC.CLASS_ID = SC.S_CLASS_ID AND SC.IS_DELETED!=1 AND SC.COURSE_ID=?COURSE_ID
                                   GROUP BY SC.COURSE_ID";
                        break;
                    }
                case StudentSQLCommads.UpdateStuCourseInfoOnlyLanguagePaper:
                    {
                        query = @"UPDATE STU_COURSE_INFO_?AC_YEAR
                                        SET 
                                            COURSE_ID = ?COURSE_ID
                                        WHERE
                                            COURSE_INFO_ID = ?COURSE_INFO_ID;";
                        break;
                    }
                case StudentSQLCommads.SaveStuCourseInfo:
                    {
                        query = @"INSERT INTO STU_COURSE_INFO_?AC_YEAR
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
                                            ?AC_YEAR,
                                            ?SEMESTER);";
                        break;
                    }
                case StudentSQLCommads.DeleteStuCourseInfo:
                    {
                        query = @"UPDATE STU_COURSE_INFO_?AC_YEAR SET IS_DELETED=1 WHERE COURSE_INFO_ID=?COURSE_INFO_ID";
                        break;
                    }
                case StudentSQLCommads.CheckClassCourse:
                    {
                        query = @"SELECT 
                                        COURSE_ID
                                    FROM
                                        CP_CLASS_COURSE_?AC_YEAR
                                    WHERE
                                        CLASS_ID = ?CLASS_ID
                                            AND COURSE_ID = ?COURSE_ID
                                            AND IS_DELETED!=1;";
                        break;
                    }
                case StudentSQLCommads.SaveAssignmentSubmission:
                    {
                        query = @"INSERT INTO ASSIGNMENT_SUBMISSION_?AC_YEAR
                                            (
                                            STUDENT_ID,
                                            CLASS_ID,
                                            ASSIGNMENT_ID,
                                            SUBMITTED_DATE,
                                            FILE_PATH)
                                            VALUES
                                            (
                                            ?STUDENT_ID,
                                            ?CLASS_ID,
                                            ?ASSIGNMENT_ID,
                                            ?SUBMITTED_DATE,
                                            ?FILE_PATH);";
                        break;
                    }
                case StudentSQLCommads.FetchAssignmentSubmissionByAssignmentIdAndStudentId:
                    {
                        query = @"SELECT 
                                        ASS_SUBMISSION_ID
                                    FROM
                                        ASSIGNMENT_SUBMISSION_?AC_YEAR
                                    WHERE
                                        ASSIGNMENT_ID = ?ASSIGNMENT_ID AND STUDENT_ID = ?STUDENT_ID
                                            AND IS_DELETED != 1;";
                        break;
                    }
                case StudentSQLCommads.UpdateAssignmentSubmission:
                    {
                        query = @"UPDATE ASSIGNMENT_SUBMISSION_?AC_YEAR
                                        SET
                                        STUDENT_ID= ?STUDENT_ID,
                                        CLASS_ID= ?CLASS_ID,
                                        ASSIGNMENT_ID= ?ASSIGNMENT_ID,
                                        SUBMITTED_DATE= ?SUBMITTED_DATE,
                                        FILE_PATH= ?FILE_PATH
                                        WHERE ASS_SUBMISSION_ID= ?ASS_SUBMISSION_ID;";
                        break;
                    }
                default:
                    break;
            }
            return query;
        }
    }
}
