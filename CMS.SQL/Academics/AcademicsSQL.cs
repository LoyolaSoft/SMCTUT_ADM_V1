using CMS.DAO;
namespace CMS.SQL.Academics
{
    public class AcademicsSQL
    {
        public static string GetAcademicSQL(AcademicSQLCommands sEnumCommand)
        {
            string query = string.Empty;
            switch (sEnumCommand)
            {
                #region Academic Year
                case AcademicSQLCommands.FetchAcademicYearInfo:
                    {
                        query = @"SELECT 
                                        ACADEMIC_YEAR_ID,
                                        AC_YEAR,
                                        DATE_FORMAT(DATE_FROM, '%d/%m/%Y') AS DATE_FROM,
                                        DATE_FORMAT(DATE_TO, '%d/%m/%Y') AS DATE_TO,
                                        ACTIVE_YEAR,
                                        ACADEMIC_NAME
                                    FROM
                                        ACADEMIC_YEAR
                                    WHERE
                                        IS_DELETED !=1
                                    ORDER BY ACADEMIC_YEAR_ID;";
                        break;
                    }
                case AcademicSQLCommands.FetchActiveYear:
                    {
                        query = @"SELECT 
                                        AC_YEAR,
                                        ACADEMIC_NAME
                                    FROM
                                        ACADEMIC_YEAR
                                    WHERE
                                        ACTIVE_YEAR=1
                                    ORDER BY ACADEMIC_YEAR_ID;";
                        break;
                    }
                case AcademicSQLCommands.InsertAcademicYearInfo:
                    {
                        query = @"INSERT INTO ACADEMIC_YEAR(
                                                   AC_YEAR,
                                                    DATE_FROM,
                                                    DATE_TO,
                                                    ACADEMIC_NAME)
                                                     VALUES
                                                     (
                                                        ?AC_YEAR,
                                                        ?DATE_FROM,
                                                        ?DATE_TO,
                                                        ?ACADEMIC_NAME
                                                        );";
                        break;
                    }
                case AcademicSQLCommands.FetchAcademicYearById:
                    {
                        query = @"SELECT 
                                                     AC_YEAR , DATE_FORMAT(DATE_FROM,'%d/%m/%Y') AS DATE_FROM, DATE_FORMAT(DATE_TO,'%d/%m/%Y') AS DATE_TO, ACTIVE_YEAR, ACADEMIC_NAME
                                                FROM
                                                    ACADEMIC_YEAR
                                                WHERE
                                                    ACADEMIC_YEAR_ID=?ACADEMIC_YEAR_ID;";
                        break;
                    }
                case AcademicSQLCommands.UpdateAcademicInfo:
                    {
                        query = @"UPDATE ACADEMIC_YEAR
                                                SET
                                                AC_YEAR = ?AC_YEAR,
                                                DATE_FROM = ?DATE_FROM,
                                                DATE_TO = ?DATE_TO,
                                                ACADEMIC_NAME = ?ACADEMIC_NAME
                                                WHERE ACADEMIC_YEAR_ID =?ACADEMIC_YEAR_ID ;";
                        break;
                    }
                case AcademicSQLCommands.DeleteAcademicInfo:
                    {
                        query = (@"UPDATE ACADEMIC_YEAR
                                                SET
                                                IS_DELETED =1
                                                WHERE ACADEMIC_YEAR_ID =?ACADEMIC_YEAR_ID ;");
                        break;
                    }
                case AcademicSQLCommands.UpdateActiveAcademic:
                    {
                        query = @"UPDATE ACADEMIC_YEAR
                                                SET
                                                ACTIVE_YEAR =0
                                                WHERE ACADEMIC_YEAR_ID !=?ACADEMIC_YEAR_ID ;";
                        break;
                    }
                case AcademicSQLCommands.UpdateActiveYear:
                    {
                        query = @"UPDATE ACADEMIC_YEAR
                                                SET
                                                ACTIVE_YEAR =1
                                                WHERE ACADEMIC_YEAR_ID =?ACADEMIC_YEAR_ID ;";
                        break;
                    }
                #endregion
                #region Course Root
                case AcademicSQLCommands.FetchCourseRootInfo:
                    {
                        query = @"SELECT
                                      RT.COURSE_ROOT_ID, 
                                      DT.DEPARTMENT,
                                      RT.YEAR,
                                      PT.PART,
                                      CT.COURSE_TYPE,
                                      RT.COURSE_TITLE,
                                      RT.COURSE_CODE,
                                      SEM.SEMESTER_NAME,
                                      NME.IS_NME_NAME,
                                      AL.IS_ALLIED_NAME
                                  FROM
                                      CP_COURSE_ROOT_?AC_YEAR AS RT
                                          LEFT JOIN
                                      SUP_SEMESTER SEM ON SEM.SUP_SEMESTER_ID = RT.SEMESTER_ID
                                          LEFT JOIN
                                      CP_DEPARTMENT_?AC_YEAR DT ON DT.DEPARTMENT_ID = RT.DEPARTMENT
                                          LEFT JOIN
                                      SUP_PART PT ON PT.PART_ID = RT.PART
                                          LEFT JOIN
                                      CP_COURSE_TYPE_?AC_YEAR CT ON CT.COURSE_TYPE_ID = RT.COURSE_TYPE
                                          LEFT JOIN
                                      SUP_IS_NME NME ON NME.IS_NME_ID = RT.IS_NME_SUBJECT
                                          LEFT JOIN
                                      SUP_IS_ALLIED AL ON AL.IS_ALLIED_ID = RT.IS_ALLIED_SUBJECT WHERE RT.IS_DELETED=0 ORDER BY RT.COURSE_ROOT_ID;";
                        break;
                    }
                case AcademicSQLCommands.InsertCourseRootInfo:
                    {
                        query = @"INSERT INTO CP_COURSE_ROOT_?AC_YEAR
                                                                (
                                                                YEAR_FROM,
                                                                YEAR_TO,
                                                                DEPARTMENT,
                                                                YEAR,
                                                                PART,
                                                                COURSE_TYPE,
                                                                HRS_PER_WEEK,
                                                                CREDITS,
                                                                PAPER_CODE,
                                                                COURSE_TITLE,
                                                                COURSE_CODE,
                                                                SEMESTER_ID,
                                                                IS_NME_SUBJECT,
                                                                IS_ALLIED_SUBJECT,
                                                                COURSE_ORDER,
                                                                SUBJECTS,
                                                                IS_COMPULSORY,
                                                                UGC_COURSE_TYPE,
                                                                PAPER_TYPE,
                                                                SUBJECT_TYPE)
                                                                VALUES
                                                                (?YEAR_FROM,
                                                                ?YEAR_TO,
                                                                ?DEPARTMENT,
                                                                ?YEAR,
                                                                ?PART,
                                                                ?COURSE_TYPE,
                                                                ?HRS_PER_WEEK,
                                                                ?CREDITS,
                                                                ?PAPER_CODE,
                                                                ?COURSE_TITLE,
                                                                ?COURSE_CODE,
                                                                ?SEMESTER_ID,
                                                                ?IS_NME_SUBJECT,
                                                                ?IS_ALLIED_SUBJECT,
                                                                ?COURSE_ORDER,
                                                                ?SUBJECTS,
                                                                ?IS_COMPULSORY,
                                                                ?UGC_COURSE_TYPE,
                                                                ?PAPER_TYPE,
                                                                ?SUBJECT_TYPE);";
                        break;
                    }
                case AcademicSQLCommands.DeleteCourseRootInfo:
                    {
                        query = @"UPDATE CP_COURSE_ROOT_?AC_YEAR
                                                            SET
                                                            IS_DELETED = 1
                                                            WHERE COURSE_ROOT_ID =?COURSE_ROOT_ID;";
                        break;
                    }
                case AcademicSQLCommands.FetchCourseRootInfoById:
                    {
                        query = @"SELECT 
	                                  COURSE_ROOT_ID,
                                      YEAR_FROM,
                                      YEAR_TO,
                                      DEPARTMENT,
                                      YEAR,
                                      SEMESTER_FROM,
                                      SEMESTER_TO,
                                      PART,
                                      COURSE_TYPE,
                                      HRS_PER_WEEK,
                                      CREDITS,
                                      OPTION_NAME,
                                      PAPER_CODE,
                                      COURSE_TITLE,
                                      COURSE_CODE,
                                      SEMESTER_ID,
                                      IS_NME_SUBJECT,
                                      IS_ALLIED_SUBJECT,
                                      COURSE_ORDER,
                                      SUBJECTS,
                                      IS_COMPULSORY,
                                      UGC_COURSE_TYPE,
                                      PAPER_TYPE,
                                      SUBJECT_TYPE
                                  FROM CP_COURSE_ROOT_?AC_YEAR WHERE COURSE_ROOT_ID=?COURSE_ROOT_ID;";
                        break;
                    }
                case AcademicSQLCommands.UpdateCourseRootInfo:
                    {
                        query = @"UPDATE CP_COURSE_ROOT_?AC_YEAR
                                                            SET
                                                            YEAR_FROM= ?YEAR_FROM,
                                                            YEAR_TO= ?YEAR_TO,
                                                            DEPARTMENT= ?DEPARTMENT,
                                                            YEAR= ?YEAR,
                                                            PART= ?PART,
                                                            COURSE_TYPE= ?COURSE_TYPE,
                                                            HRS_PER_WEEK= ?HRS_PER_WEEK,
                                                            CREDITS= ?CREDITS,
                                                            PAPER_CODE= ?PAPER_CODE,
                                                            COURSE_TITLE= ?COURSE_TITLE,
                                                            COURSE_CODE= ?COURSE_CODE,
                                                            SEMESTER_ID= ?SEMESTER_ID,
                                                            IS_NME_SUBJECT= ?IS_NME_SUBJECT,
                                                            IS_ALLIED_SUBJECT= ?IS_ALLIED_SUBJECT,
                                                            COURSE_ORDER= ?COURSE_ORDER,
                                                            SUBJECTS= ?SUBJECTS,
                                                            IS_COMPULSORY= ?IS_COMPULSORY,
                                                            UGC_COURSE_TYPE= ?UGC_COURSE_TYPE,
                                                            PAPER_TYPE=?PAPER_TYPE,
                                                            SUBJECT_TYPE=?SUBJECT_TYPE
                                                            WHERE COURSE_ROOT_ID=?COURSE_ROOT_ID;";
                        break;
                    }
                case AcademicSQLCommands.CheckCourseRootInfo:
                    {
                        query = @"SELECT 
	                                  COURSE_ROOT_ID,
                                      YEAR_FROM,
                                      YEAR_TO,
                                      DEPARTMENT,
                                      YEAR,
                                      SEMESTER_FROM,
                                      SEMESTER_TO,
                                      PART,
                                      COURSE_TYPE,
                                      HRS_PER_WEEK,
                                      CREDITS,
                                      OPTION_NAME,
                                      PAPER_CODE,
                                      COURSE_TITLE,
                                      COURSE_CODE,
                                      SEMESTER_ID,
                                      IS_NME_SUBJECT,
                                      IS_ALLIED_SUBJECT,
                                      COURSE_ORDER,
                                      SUBJECTS,
                                      IS_COMPULSORY,
                                      UGC_COURSE_TYPE
                                  FROM CP_COURSE_ROOT_?AC_YEAR WHERE COURSE_CODE=?COURSE_CODE;";
                        break;
                    }
                #endregion
                #region Course Wise Staff
                case AcademicSQLCommands.FetchCourseWiseStaffInfo:
                    {
                        query = @"SELECT 
                                        CCS.CLSCRSSTF_ID,
                                        CCS.CLASS_ID,
                                        CCS.COURSE_ID,
                                        CLS.CLASS_NAME,
                                        CLS.CLASS_CODE,
                                        CR.COURSE_CODE,
                                        CR.COURSE_TITLE,
                                        GROUP_CONCAT(CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                    '(',
                                                    IFNULL(SP.STAFF_CODE, ''),
                                                    ')')) AS STAFF_NAME,
                                        COUNT(CCS.STAFF_ID) AS STAFF_COUNT,
                                        CCS.SHIFT,
                                        CCS.SEMESTER_ID
                                    FROM
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CCS
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CCS.COURSE_ID
                                            INNER JOIN
                                        STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = CCS.STAFF_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = CCS.CLASS_ID
                                        WHERE CCS.IS_DELETED=0
                                    GROUP BY CCS.CLASS_ID , CCS.COURSE_ID ;";
                        break;
                    }
                case AcademicSQLCommands.IsCourseWiseStaffExisting:
                    {
                        query = @"SELECT 
                                        CLSCRSSTF_ID
                                    FROM
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR
                                    WHERE
                                        CLASS_ID =?CLASS_ID
                                        AND COURSE_ID =?COURSE_ID AND STAFF_ID=?STAFF_ID AND IS_DELETED!=1;";
                        break;
                    }
                case AcademicSQLCommands.DeleteCourseWiseStaffInfo:
                    {
                        query = @"UPDATE CP_CLASS_COURSE_STAFF_?AC_YEAR SET IS_DELETED=1 WHERE CLSCRSSTF_ID=?CLSCRSSTF_ID";
                        break;
                    }
                case AcademicSQLCommands.FetchCourseWiseStaffInfoById:
                    {
                        query = @"SELECT 
                                                    CLSCRSSTF_ID,CLASS_ID, COURSE_ID,STAFF_ID, HRS_WEEK, HRS_TERM, SHIFT, SEMESTER_ID
                                                FROM
                                                    CP_CLASS_COURSE_STAFF_?AC_YEAR WHERE CLSCRSSTF_ID=?CLSCRSSTF_ID;";
                        break;
                    }
                case AcademicSQLCommands.GetClassListByClassTypeId:
                    {
                        query = @" SELECT 
                                        CLASS_ID,CLASS_CODE
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        DEPARTMENT =?DEPARTMENT 
		                                    AND 
                                        SHIFT =?SHIFT
		                                    AND 
                                        CLASS_TYPE = ?CLASS_TYPE;";
                        break;
                    }
                case AcademicSQLCommands.GetClassTypeByShiftId:
                    {
                        query = @"SELECT 
                                        CLASS_TYPE_ID, CLASS_TYPE
                                    FROM
                                        SUP_CLASS_TYPE;";
                        break;
                    }
                case AcademicSQLCommands.FetchCourseWiseStaffInfoByClassId:
                    {
                        query = @"SELECT 
                                        CLS.CLASS_ID,
                                        CLS.CLASS_CODE,
                                        AC.COURSE_ID,
                                        CR.COURSE_TITLE,
                                        CS.HRS_TERM,
                                        CS.HRS_WEEK,
                                        CR.COURSE_CODE,
                                        GROUP_CONCAT(CS.STAFF_ID) AS STAFF_NAME,
                                        SEM.SEMESTER_NAME,
                                        SEM.SUP_SEMESTER_ID
                                    FROM
                                        ACADEMIC_CURRICULUM_?AC_YEAR AS AC
                                            LEFT JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON AC.COURSE_ID = CR.COURSE_ROOT_ID
                                            AND CR.COURSE_ROOT_ID NOT IN (SELECT 
                                                CLS.COURSE_ID
                                            FROM
                                                CP_CLASSES_?AC_YEAR AS CLS
                                            WHERE
                                                CLS.COURSE_ID != '')
                                            AND CR.IS_NME_SUBJECT != 1
                                            LEFT JOIN
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CS ON CS.COURSE_ID = CR.COURSE_ROOT_ID
                                            AND CS.CLASS_ID = ?CLASS_ID AND CS.IS_DELETED!=1
                                            LEFT JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = CS.CLASS_ID
                                            AND CLS.CLASS_TYPE = ?CLASS_TYPE
                                            INNER JOIN
                                        SUP_SEMESTER AS SEM ON SEM.SUP_SEMESTER_ID = CR.SEMESTER_ID
                                    WHERE
                                        AC.PROGRAMME = (SELECT 
                                                PROGRAMME
                                            FROM
                                                CP_CLASSES_?AC_YEAR
                                            WHERE
                                                CLASS_ID = ?CLASS_ID)
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
                                            AND CR.SEMESTER_ID = (SELECT 
                                        ASE.SEMESTER
                                    FROM
                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.PROGRAMME = ASE.PROGRAMME
                                            AND CLS.CLASS_ID = ?CLASS_ID
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.CLASS_ID = CLS.CLASS_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                    WHERE
                                        SC.CLASS_ID = ?CLASS_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            AND ASE.PROGRAMME = CLS.PROGRAMME
                                            AND ASE.BATCH = SP.BATCH
                                            AND ASE.IS_ACTIVE = 1
                                    GROUP BY ASE.SEMESTER) AND AC.IS_DELETED!=1
                                    GROUP BY AC.COURSE_ID
                                    UNION SELECT 
                                        CLS.CLASS_ID,
                                        CLS.CLASS_CODE,
                                        CLS.COURSE_ID,
                                        CR.COURSE_TITLE,
                                        CS.HRS_TERM,
                                        CS.HRS_WEEK,
                                        CR.COURSE_CODE,
                                        GROUP_CONCAT(CS.STAFF_ID) AS STAFF_NAME,
                                        SEM.SEMESTER_NAME,
                                        SEM.SUP_SEMESTER_ID
                                    FROM
                                        CP_CLASSES_?AC_YEAR AS CLS
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CLS.COURSE_ID
                                            LEFT JOIN
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CS ON CS.COURSE_ID = CLS.COURSE_ID
                                            AND CS.CLASS_ID = CLS.CLASS_ID AND CS.IS_DELETED!=1
                                            INNER JOIN
                                        SUP_SEMESTER AS SEM ON SEM.SUP_SEMESTER_ID = CR.SEMESTER_ID
                                    WHERE
                                        CLS.CLASS_ID = ?CLASS_ID
                                            AND CLS.CLASS_TYPE = ?CLASS_TYPE 
                                           -- AND CS.STAFF_ID!=0
                                    GROUP BY CS.CLASS_ID , CS.COURSE_ID;";
                        break;
                    }
                case AcademicSQLCommands.SaveCourseWiseStaff:
                    {
                        query = @"INSERT INTO CP_CLASS_COURSE_STAFF_?AC_YEAR
                                    (
                                    CLASS_ID,
                                    COURSE_ID,
                                    STAFF_ID,
                                    SHIFT,
                                    SEMESTER_ID)
                                    VALUES
                                    (?CLASS_ID,?COURSE_ID,?STAFF_ID,?SHIFT,?SEMESTER_ID);";
                        break;
                    }
                case AcademicSQLCommands.UpdateCourseWiseStaff:
                    {
                        query = @"UPDATE  CP_CLASS_COURSE_STAFF_?AC_YEAR SET                                     
                                    CLASS_ID=?CLASS_ID,
                                    COURSE_ID=?COURSE_ID,
                                    STAFF_ID=?STAFF_ID,
                                    SHIFT=?SHIFT,
                                    SEMESTER_ID=?SEMESTER_ID WHERE CLSCRSSTF_ID=?CLSCRSSTF_ID";
                        break;
                    }
                #endregion
                #region Academic Curriculum
                case AcademicSQLCommands.UpdateAcademicCurriculumInfo:
                    {
                        query = @"UPDATE  ACADEMIC_CURRICULUM_?AC_YEAR SET                                     
                                    PROGRAMME=?PROGRAMME,
                                    BATCH=?BATCH,
                                    COURSE_ID=?COURSE_ID,
                                    IS_ACTIVE=?IS_ACTIVE WHERE CURRICULUM_ID=?CURRICULUM_ID";
                        break;
                    }
                case AcademicSQLCommands.SaveAcademicCurriculum:
                    {
                        query = @"INSERT INTO ACADEMIC_CURRICULUM_?AC_YEAR
                                    (
                                    PROGRAMME,
                                    BATCH,
                                    COURSE_ID,
                                    IS_ACTIVE,
                                    SEMESTER)
                                    VALUES
                                    (?PROGRAMME,?BATCH,?COURSE_ID,?IS_ACTIVE,(SELECT SEMESTER_ID FROM CP_COURSE_ROOT_?AC_YEAR AS CR  WHERE CR.COURSE_ROOT_ID=?COURSE_ID));";
                        break;
                    }
                case AcademicSQLCommands.IsAcademicCurriculumExisting:
                    {
                        query = @"SELECT 
                                        CURRICULUM_ID
                                    FROM
                                        ACADEMIC_CURRICULUM_?AC_YEAR AS AC
                                    WHERE
                                        PROGRAMME =?PROGRAMME AND BATCH =?BATCH
                                            AND COURSE_ID =?COURSE_ID AND AC.IS_DELETED!=1;";
                        break;
                    }
                case AcademicSQLCommands.CheckActiveCurriculum:
                    {
                        query = @"UPDATE ACADEMIC_CURRICULUM_?AC_YEAR  SET IS_ACTIVE='0' WHERE PROGRAMME='?PROGRAMME' AND BATCH='?BATCH';";
                        break;
                    }
                case AcademicSQLCommands.FetchAcademicCurriculumInfo:
                    {
                        query = @"SELECT 
                                    CUR.CURRICULUM_ID,
                                    PR.PROGRAMME_CODE,
                                    SBT.BATCH,
                                    CONCAT(IFNULL(COURSE_TITLE, ''),
                                            '(',
                                            IFNULL(COURSE_CODE, ''),
                                            ')') AS COURSE_CODE,
                                    RT.COURSE_TITLE,
                                    SEM.SEMESTER_NAME
                                FROM
                                    ACADEMIC_CURRICULUM_?AC_YEAR AS CUR
                                        INNER JOIN
                                    CP_PROGRAMME_?AC_YEAR AS PR ON PR.PROGRAMME_ID = CUR.PROGRAMME
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID = CUR.COURSE_ID
		                                INNER JOIN
	                                SUP_SEMESTER AS SEM ON SEM.SUP_SEMESTER_ID=CUR.SEMESTER
										INNER JOIN 
									SUP_BATCHES AS SBT ON SBT.BATCH_ID=CUR.BATCH WHERE CUR.IS_DELETED!=1 AND CUR.SEMESTER IN (SELECT SEMESTER FROM ACADEMIC_SEMESTER_?AC_YEAR WHERE IS_ACTIVE=1 GROUP BY SEMESTER);";
                        break;
                    }
                case AcademicSQLCommands.FetchCurriculumCourseInfoById:
                    {
                        query = @"SELECT 
                                    CUR.CURRICULUM_ID,
                                    PR.PROGRAMME_ID,
                                    PR.PROGRAMME_CODE,
                                    SBT.BATCH_ID,
                                    SBT.BATCH,
                                    RT.COURSE_ROOT_ID,
                                    RT.COURSE_CODE,
                                    RT.COURSE_TITLE,
                                    SEM.SUP_SEMESTER_ID,
                                    SEM.SEMESTER_NAME
                                FROM
                                    ACADEMIC_CURRICULUM_?AC_YEAR AS CUR
                                        INNER JOIN
                                    CP_PROGRAMME_?AC_YEAR AS PR ON PR.PROGRAMME_ID = CUR.PROGRAMME
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID = CUR.COURSE_ID
		                                INNER JOIN
	                                SUP_SEMESTER AS SEM ON SEM.SUP_SEMESTER_ID=CUR.SEMESTER
										INNER JOIN 
									SUP_BATCHES AS SBT ON SBT.BATCH_ID=CUR.BATCH WHERE CUR.IS_DELETED=0 AND CUR.PROGRAMME=?PROGRAMME AND CUR.BATCH=?BATCH;";
                        break;
                    }
                case AcademicSQLCommands.DeleteCurriculumInfo:
                    {
                        query = @"UPDATE ACADEMIC_CURRICULUM_?AC_YEAR SET IS_DELETED=1 WHERE CURRICULUM_ID=?CURRICULUM_ID;";
                        break;
                    }
                case AcademicSQLCommands.FetchCourseBySemester:
                    {
                        query = @"SELECT 
                                    COURSE_ROOT_ID,
                                    CONCAT(IFNULL(COURSE_TITLE, ''),
                                            '(',
                                            IFNULL(COURSE_CODE, ''),
                                            ')') AS COURSE_TITLE,
                                    IF(AC.CURRICULUM_ID IS NULL OR  AC.IS_ACTIVE=0,
                                        '',
                                        'SELECTED') AS SELECTED
                                FROM
                                    CP_COURSE_ROOT_?AC_YEAR AS CR
                                        LEFT JOIN
                                    ACADEMIC_CURRICULUM_?AC_YEAR AS AC ON AC.COURSE_ID = CR.COURSE_ROOT_ID
                                        AND AC.BATCH =?BATCH
                                        AND AC.PROGRAMME =?PROGRAMME AND AC.IS_DELETED!=1
                                WHERE
                                    SEMESTER_ID = (SELECT 
                                            SEMESTER
                                        FROM
                                            ACADEMIC_SEMESTER_?AC_YEAR as AC
                                        WHERE
                                            BATCH =?BATCH AND PROGRAMME =?PROGRAMME
                                                AND IS_ACTIVE = 1 AND AC.IS_DELETED!=1);";
                        break;

                    }
                case AcademicSQLCommands.FetchSelectedCourseWiseStaff:
                    {
                        query = @"SELECT 
                                        CCS.CLSCRSSTF_ID,
                                        CCS.CLASS_ID,
                                        CCS.COURSE_ID,
                                        CLS.CLASS_NAME,
                                        CR.COURSE_CODE,
                                        CR.COURSE_TITLE,
                                        CCS.SHIFT,
                                        CR.SEMESTER_ID,
                                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                '(',
                                                IFNULL(SP.STAFF_CODE, ''),
                                                ')') AS STAFF_NAME
                                    FROM
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CCS
                                            LEFT JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CCS.COURSE_ID
                                            LEFT JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = CCS.CLASS_ID
                                            LEFT JOIN
                                        STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = CCS.STAFF_ID
                                    WHERE
                                        CCS.CLASS_ID =?CLASS_ID
                                        AND CCS.COURSE_ID =?COURSE_ID;";
                        break;
                    }
                #endregion
                #region Semester Root
                case AcademicSQLCommands.FetchSemesterRootInfo:
                    {
                        query = @"SELECT 
                                        SR.ACC_SEMESTER_ID,
                                        SB.BATCH,
                                        CP.PROGRAMME_NAME,
                                        SS.SEMESTER_NAME,
                                        DATE_FORMAT(DATE_FROM, '%d/%m/%Y') AS DATE_FROM,
                                        DATE_FORMAT(DATE_TO, '%d/%m/%Y') AS DATE_TO,
                                        SR.IS_ACTIVE
                                    FROM
                                        ACADEMIC_SEMESTER_?AC_YEAR AS SR
                                            INNER JOIN
                                        SUP_BATCHES AS SB ON SB.BATCH_ID = SR.BATCH
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = SR.PROGRAMME
                                            INNER JOIN
                                        SUP_SEMESTER AS SS ON SS.SUP_SEMESTER_ID = SR.SEMESTER WHERE SR.IS_DELETED!=1;";
                        break;
                    }
                //case AcademicSQLCommands.UpdateActiveSemester:
                //    {
                //        query = @"UPDATE CP_SEMESTER_ROOT_ 
                //                                SET 
                //                                    IS_ACTIVE = IF(IS_ACTIVE = 0,
                //                                        1,
                //                                        IF(IS_ACTIVE = 1, 0, IS_ACTIVE))
                //                                WHERE
                //                                    SEMESTER_ID =?SEMESTER_ID;";
                //        break;
                //    }
                case AcademicSQLCommands.GetSemesterId:
                    {
                        query = @"SELECT 
                                        SEM.SUP_SEMESTER_ID,
                                        SEM.SEMESTER_NAME,
                                        IF(SR.SEMESTER IS NULL OR  SR.IS_ACTIVE=0,
                                        '',
                                        'SELECTED') AS SELECTED
                                    FROM
                                        ACADEMIC_SEMESTER_?AC_YEAR AS SR
                                            INNER JOIN
                                        SUP_SEMESTER AS SEM ON SEM.SUP_SEMESTER_ID = SR.SEMESTER
                                    WHERE
                                        SR.BATCH = ?BATCH AND SR.PROGRAMME =?PROGRAMME AND SR.IS_ACTIVE=1;;";
                        break;
                    }
                case AcademicSQLCommands.CheckActiveSemesterRoot:
                    {
                        query = @"UPDATE ACADEMIC_SEMESTER_?AC_YEAR  SET IS_ACTIVE='0' WHERE PROGRAMME='?PROGRAMME' AND BATCH='?BATCH';";
                        break;
                    }
                case AcademicSQLCommands.IsSemesterRootExisting:
                    {
                        query = @"SELECT 
                                        ACC_SEMESTER_ID
                                    FROM
                                        ACADEMIC_SEMESTER_?AC_YEAR AS AC
                                    WHERE
                                        PROGRAMME =?PROGRAMME AND BATCH =?BATCH
                                            AND SEMESTER =?SEMESTER;";
                        break;
                    }
                case AcademicSQLCommands.SaveSemesterRoot:
                    {
                        query = @"INSERT INTO ACADEMIC_SEMESTER_?AC_YEAR
                                    (
                                    BATCH,
                                    PROGRAMME,
                                    SEMESTER,
                                    DATE_FROM,
                                    DATE_TO,
                                    IS_ACTIVE)
                                    VALUES
                                    (?BATCH,?PROGRAMME,?SEMESTER,?DATE_FROM,?DATE_TO,?IS_ACTIVE);";
                        break;
                    }
                case AcademicSQLCommands.UpdateSemesterRoot:
                    {
                        query = @"UPDATE  ACADEMIC_SEMESTER_?AC_YEAR SET                                     
                                    BATCH=?BATCH,
                                    PROGRAMME=?PROGRAMME,
                                    SEMESTER=?SEMESTER,
                                    DATE_FROM=?DATE_FROM,
                                    DATE_TO=?DATE_TO,
                                    IS_ACTIVE=?IS_ACTIVE WHERE ACC_SEMESTER_ID=?ACC_SEMESTER_ID;";
                        break;
                    }
                #endregion
                #region Class Course
                case AcademicSQLCommands.IsClassCourseExisting:
                    {

                        query = @"SELECT 
                                    COUNT(CLASS_COURSE_ID) > 0 AS IS_EXIST
                                FROM
                                    CP_CLASS_COURSE_?AC_YEAR
                                WHERE
                                    CLASS_ID =?CLASS_ID AND COURSE_ID =?COURSE_ID
                                        AND IS_DELETED != 1;";
                        break;
                    }
                case AcademicSQLCommands.DeleteClassCourseInfo:
                    {

                        query = @"UPDATE CP_CLASS_COURSE_?AC_YEAR
                                    SET
                                    IS_DELETED =1
                                    WHERE CLASS_COURSE_ID = ?CLASS_COURSE_ID;";
                        break;
                    }
                case AcademicSQLCommands.SaveClassCourseInfo:
                    {

                        query = @"INSERT INTO CP_CLASS_COURSE_?AC_YEAR
                                    (CLASS_ID,
                                    COURSE_ID)
                                    VALUES
                                    (?CLASS_ID,?COURSE_ID);";
                        break;
                    }
                case AcademicSQLCommands.UpdateClassCourseInfo:
                    {

                        query = @"UPDATE CP_CLASS_COURSE_?AC_YEAR
                                    SET
                                    CLASS_ID = ?CLASS_ID,
                                    COURSE_ID = ?COURSE_ID
                                    WHERE CLASS_COURSE_ID = ?CLASS_COURSE_ID;";
                        break;
                    }
                case AcademicSQLCommands.FetchCourseListByClassId:
                    {
                        query = @"SELECT 
                                        CR.COURSE_ROOT_ID,
                                        CONCAT(IFNULL(CR.COURSE_CODE, ''),
                                                '(',
                                                IFNULL(CR.COURSE_TITLE, ''),
                                                ')') AS COURSE_TITLE
                                    FROM
                                        ACADEMIC_CURRICULUM_?AC_YEAR AS AC
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = AC.COURSE_ID
                                            AND CR.IS_COMPULSORY = 1
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AC.PROGRAMME
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.PROGRAMME = CP.PROGRAMME_ID
                                            AND CLS.CLASS_ID = ?CLASS_ID
                                    WHERE
                                        AC.BATCH = (SELECT 
                                                SP.BATCH
                                            FROM
                                                STU_CLASS AS SC
                                                    INNER JOIN
                                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                                    AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            WHERE
                                                SC.CLASS_ID = ?CLASS_ID
                                            GROUP BY SP.BATCH)
                                            AND AC.IS_ACTIVE = 1;";

                        break;
                    }
                case AcademicSQLCommands.FetchClassesByClassTypeId:
                    {
                        query = @"SELECT CLASS_ID,CLASS_NAME FROM CP_CLASSES_?AC_YEAR AS CLS WHERE CLS.CLASS_TYPE=?CLASS_TYPE and CLS.IS_DELETED!=1;";
                        break;
                    }

                case AcademicSQLCommands.FetchClassCourseByClassCourseId:
                    {
                        query = @"SELECT 
                                    CLASS_ID, COURSE_ID
                                FROM
                                    CP_CLASS_COURSE_?AC_YEAR
                                WHERE
                                    CLASS_COURSE_ID =?CLASS_COURSE_ID  AND IS_DELETED != 1;";
                        break;
                    }
                case AcademicSQLCommands.FetchClassCourseList:
                    {
                        query = @"SELECT 
                                        CLASS_COURSE_ID,
                                        CC.CLASS_ID,
                                        CC.COURSE_ID,
                                        CT.COURSE_TYPE,
                                        CR.COURSE_TITLE,
                                        CR.COURSE_CODE,
                                        IF(CR.IS_NME_SUBJECT = 1, 'YES', 'NO') AS IS_NME_SUBJECT,
                                        IF(CR.IS_ALLIED_SUBJECT = 1,
                                            'YES',
                                            'NO') AS IS_ALLIED_SUBJECT,
                                        CLS.CLASS_NAME,
                                        SH.SHIFT_NAME,
                                        SEM.SEMESTER_NAME
                                    FROM
                                        CP_CLASS_COURSE_?AC_YEAR AS CC
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CC.COURSE_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = CC.CLASS_ID
                                            INNER JOIN
                                        CP_COURSE_TYPE_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID = CR.COURSE_TYPE
                                            LEFT JOIN
                                        SUP_SEMESTER AS SEM ON SEM.SUP_SEMESTER_ID = CR.SEMESTER_ID
                                            LEFT JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = CLS.SHIFT
                                            LEFT JOIN
                                        CP_PROGRAMME_?AC_YEAR AS PRO ON PRO.PROGRAMME_ID = CLS.PROGRAMME
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
                                                SC.CLASS_ID = ?CLASS_ID
                                                    AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            GROUP BY SP.BATCH)
                                    WHERE
                                        CLS.CLASS_ID = ?CLASS_ID AND CC.IS_DELETED != 1
                                            AND CR.SEMESTER_ID = ASE.SEMESTER;";
                        break;
                    }
                #endregion
                #region Department
                case AcademicSQLCommands.FetchDepartmentInfo:
                    {
                        query = @"SELECT 
                                        DT.DEPARTMENT_ID,
                                        DT.DEPARTMENT_CODE,
                                        DT.DEPARTMENT,
                                        DT.DEPARTMENT_ORDER,
                                        FY.FACULTY,
                                        DT.YEAR_OF_PUBLISHMENT,
                                        DT.IS_ACTIVE
                                    FROM
                                        CP_DEPARTMENT_?AC_YEAR as DT LEFT JOIN CP_FACULTY_?AC_YEAR AS FY ON FY.FACULTY_ID=DT.FACULTY
                                    WHERE
                                        DT.IS_DELETED=0;";
                        break;
                    }
                case AcademicSQLCommands.InsertDepartmentInfo:
                    {
                        query = @"INSERT INTO CP_DEPARTMENT_?AC_YEAR
                                            (
                                            DEPARTMENT_CODE,
                                            DEPARTMENT,
                                            DEPARTMENT_ORDER,
                                            FACULTY,
                                            YEAR_OF_PUBLISHMENT,
                                            IS_ACTIVE)
                                            VALUES
                                            (
                                            ?DEPARTMENT_CODE,
                                            ?DEPARTMENT,
                                            ?DEPARTMENT_ORDER,
                                            ?FACULTY,
                                            ?YEAR_OF_PUBLISHMENT,
                                            ?IS_ACTIVE);";
                        break;
                    }
                case AcademicSQLCommands.CheckDepartment:
                    {
                        query = @"SELECT 
                                    DEPARTMENT_ID,
                                    DEPARTMENT_CODE,
                                    DEPARTMENT,
                                    DEPARTMENT_ORDER,
                                    FACULTY,
                                    YEAR_OF_PUBLISHMENT,
                                    IS_ACTIVE
                                FROM
                                    CP_DEPARTMENT_?AC_YEAR where DEPARTMENT_CODE=?DEPARTMENT_CODE;";
                        break;
                    }
                case AcademicSQLCommands.UpdateDepartmentInfo:
                    {
                        query = @"UPDATE CP_DEPARTMENT_?AC_YEAR
                                                            SET
                                                            DEPARTMENT= ?DEPARTMENT,
                                                            DEPARTMENT_CODE=?DEPARTMENT_CODE,
                                                            DEPARTMENT=?DEPARTMENT,
                                                            DEPARTMENT_ORDER=?DEPARTMENT_ORDER,
                                                            FACULTY=?FACULTY,
                                                            YEAR_OF_PUBLISHMENT=?YEAR_OF_PUBLISHMENT,
                                                            IS_ACTIVE=?IS_ACTIVE
                                                            WHERE DEPARTMENT_ID=?DEPARTMENT_ID;";
                        break;
                    }
                case AcademicSQLCommands.FetchDepartmentInfoById:
                    {
                        query = @"SELECT 
                                    DEPARTMENT_ID,
                                    DEPARTMENT_CODE,
                                    DEPARTMENT,
                                    DEPARTMENT_ORDER,
                                    FACULTY,
                                    YEAR_OF_PUBLISHMENT,
                                    IS_ACTIVE
                                FROM
                                    CP_DEPARTMENT_?AC_YEAR WHERE DEPARTMENT_ID=?DEPARTMENT_ID;";
                        break;
                    }
                case AcademicSQLCommands.DeleteDepartmentInfo:
                    {
                        query = @"UPDATE CP_DEPARTMENT_?AC_YEAR
                                                            SET
                                                            IS_DELETED = 1
                                                            WHERE DEPARTMENT_ID =?DEPARTMENT_ID;";
                        break;
                    }
                #endregion
                #region Facutly
                case AcademicSQLCommands.FetchFAcutlyInfo:
                    {
                        query = @"SELECT 
                                        FACULTY_ID,
                                        FACULTY,
                                        FACULTY_CODE,
                                        FACULTY_ORDER,
                                        FAC_SERIES,
                                        IS_ACTIVE
                                    FROM
                                        CP_FACULTY_?AC_YEAR
                                    WHERE
                                        IS_DELETED = 0";
                        break;
                    }
                case AcademicSQLCommands.CheckFaculty:
                    {
                        query = @"SELECT 
                                        FACULTY_ID,
                                        FACULTY,
                                        FACULTY_CODE,
                                        FACULTY_ORDER,
                                        FAC_SERIES,
                                        IS_ACTIVE
                                    FROM
                                        CP_FACULTY_?AC_YEAR
                                    WHERE
                                        FACULTY_CODE = ?FACULTY_CODE";
                        break;
                    }
                case AcademicSQLCommands.InsertFacultyInfo:
                    {
                        query = @"INSERT INTO CP_FACULTY_?AC_YEAR
                                            (
                                            FACULTY_CODE,
                                            FACULTY,
                                            FACULTY_ORDER,
                                            FAC_SERIES)
                                            VALUES
                                            (
                                            ?FACULTY_CODE,
                                            ?FACULTY,
                                            ?FACULTY_ORDER,
                                            ?FAC_SERIES);";
                        break;
                    }
                case AcademicSQLCommands.FetchFacultyInfoById:
                    {
                        query = @"SELECT 
                                        FACULTY_ID,
                                        FACULTY,
                                        FACULTY_CODE,
                                        FACULTY_ORDER,
                                        FAC_SERIES
                                FROM
                                    CP_FACULTY_?AC_YEAR WHERE FACULTY_ID=?FACULTY_ID;";
                        break;
                    }
                case AcademicSQLCommands.UpdateFacultyInfo:
                    {
                        query = @"UPDATE CP_FACULTY_?AC_YEAR 
                                                    SET 
                                                        FACULTY_ID = ?FACULTY_ID,
                                                        FACULTY_CODE = ?FACULTY_CODE,
                                                        FACULTY = ?FACULTY,
                                                        FACULTY_ORDER = ?FACULTY_ORDER,
                                                        FAC_SERIES = ?FAC_SERIES
                                                    WHERE
                                                        FACULTY_ID = ?FACULTY_ID;";
                        break;
                    }
                case AcademicSQLCommands.DeleteFacultyInfo:
                    {
                        query = @"UPDATE CP_FACULTY_?AC_YEAR 
                                                            SET
                                                            IS_DELETED = 1
                                                            WHERE FACULTY_ID =?FACULTY_ID;";
                        break;
                    }
                #endregion
                #region Class
                case AcademicSQLCommands.FetchClassInfo:
                    {
                        query = @"SELECT 
                                        CLS.CLASS_ID,
                                        CLS.CLASS_CODE,
                                        CLS.CLASS_NAME,
                                        CLS.CLASS_DESCRIPTION,
                                        CT.CLASS_TYPE,
                                        CL.CLASSLEVEL,
                                        CY.CLASS_YEAR,
                                        PR.PROGRAMME_NAME,
                                        DT.DEPARTMENT,
                                        LN.LANGUAGE_NAME,
                                        ST.SHIFT_NAME
                                    FROM
                                        CP_CLASSES_?AC_YEAR AS CLS
                                            LEFT JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DT ON DT.DEPARTMENT_ID = CLS.DEPARTMENT
                                            LEFT JOIN
                                        CP_PROGRAMME_?AC_YEAR AS PR ON PR.PROGRAMME_ID = CLS.PROGRAMME
                                            LEFT JOIN
                                        SUP_CLASS_LEVEL AS CL ON CL.CLASSLEVELID = CLS.CLASS_LEVEL
                                            LEFT JOIN
                                        SUP_CLASS_TYPE AS CT ON CT.CLASS_TYPE = CLS.CLASS_TYPE
                                            LEFT JOIN
                                        SUP_CLASS_YEAR AS CY ON CY.CLASS_YEAR_ID = CLS.CLASS_YEAR
                                            LEFT JOIN
                                        SUP_LANGUAGE AS LN ON LN.LANGUAGE_ID = CLS.LANGUAGE
                                            LEFT JOIN
                                        SUP_SHIFT AS ST ON ST.SHIFT_ID = CLS.SHIFT WHERE CLS. IS_DELETED=0";
                        break;
                    }
                case AcademicSQLCommands.CheckClass:
                    {
                        query = @"SELECT 
	                                    CLASS_ID,
                                        CLASS_CODE,
                                        CLASS_DESCRIPTION,
                                        CLASS_YEAR 
                                    FROM CP_CLASSES_?AC_YEAR 
                                    WHERE CLASS_CODE=?CLASS_CODE;";
                        break;
                    }
                case AcademicSQLCommands.InsertClassInfo:
                    {
                        query = @"INSERT INTO CP_CLASSES_?AC_YEAR
                                                            (
                                                            CLASS_CODE,
                                                            CLASS_NAME,
                                                            CLASS_DESCRIPTION,
                                                            CLASS_TYPE,
                                                            CLASS_LEVEL,
                                                            CLASS_YEAR,
                                                            PROGRAMME,
                                                            DEPARTMENT,
                                                            LANGUAGE,
                                                            IS_CHOICE,
                                                            CLASS_ORDER,
                                                            SHIFT,
                                                            COURSE_ID)
                                                            VALUES
                                                            (
                                                            ?CLASS_CODE,
                                                            ?CLASS_NAME,
                                                            ?CLASS_DESCRIPTION,
                                                            ?CLASS_TYPE,
                                                            ?CLASS_LEVEL,
                                                            ?CLASS_YEAR,
                                                            ?PROGRAMME,
                                                            ?DEPARTMENT,
                                                            ?LANGUAGE,
                                                            ?IS_CHOICE,
                                                            ?CLASS_ORDER,
                                                            ?SHIFT,
                                                            ?COURSE_ID);";
                        break;
                    }
                case AcademicSQLCommands.FetchClassInfoById:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CLASS_NAME,
                                        CLASS_CODE,
                                        CLASS_DESCRIPTION,
                                        CLASS_TYPE,
                                        CLASS_LEVEL,
                                        CLASS_YEAR,
                                        PROGRAMME,
                                        DEPARTMENT,
                                        LANGUAGE,
                                        IS_CHOICE,
                                        CLASS_ORDER,
                                        SHIFT,
                                        COURSE_ID
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case AcademicSQLCommands.UpdateClassInfo:
                    {
                        query = @"UPDATE CP_CLASSES_?AC_YEAR
                                                        SET
                                                        CLASS_ID=?CLASS_ID,
                                                        CLASS_CODE=?CLASS_CODE,
                                                        CLASS_NAME=?CLASS_NAME,
                                                        CLASS_DESCRIPTION=?CLASS_DESCRIPTION,
                                                        CLASS_TYPE=?CLASS_TYPE,
                                                        CLASS_LEVEL=?CLASS_LEVEL,
                                                        CLASS_YEAR=?CLASS_YEAR,
                                                        PROGRAMME=?PROGRAMME,
                                                        DEPARTMENT=?DEPARTMENT,
                                                        LANGUAGE=?LANGUAGE,
                                                        IS_CHOICE=?IS_CHOICE,
                                                        CLASS_ORDER=?CLASS_ORDER,
                                                        SHIFT=?SHIFT,
                                                        COURSE_ID=?COURSE_ID
                                                        WHERE CLASS_ID=?CLASS_ID;";
                        break;
                    }
                case AcademicSQLCommands.DeleteClassInfo:
                    {
                        query = @"UPDATE CP_CLASSES_?AC_YEAR
                                                            SET
                                                            IS_DELETED = 1
                                                            WHERE CLASS_ID =?CLASS_ID;";
                        break;
                    }
                #endregion
                #region Batch
                case AcademicSQLCommands.FetcAcademichBatchInfo:
                    {
                        query = @"SELECT 
                                        AB.AC_BATCH_ID,
                                        SB.BATCH,
                                        AY.AC_YEAR
                                    FROM
                                        ACADEMIC_BATCHES AS AB
                                            LEFT JOIN
                                        SUP_BATCHES AS SB ON SB.BATCH_ID = AB.BATCH_ID
                                            LEFT JOIN
                                        ACADEMIC_YEAR AS AY ON AY.ACADEMIC_YEAR_ID = AB.ACADEMIC_YEAR_ID;";
                        break;
                    }
                case AcademicSQLCommands.CheckAcademicBatch:
                    {
                        query = @"SELECT 
                                        AC_BATCH_ID
                                    FROM
                                        ACADEMIC_BATCHES
                                    WHERE
                                        BATCH_ID = ?BATCH_ID AND ACADEMIC_YEAR_ID=?ACADEMIC_YEAR_ID;";
                        break;
                    }
                case AcademicSQLCommands.InsertAcademicBatchInfo:
                    {
                        query = @"INSERT INTO ACADEMIC_BATCHES
                                            (
                                            BATCH_ID,
                                            ACADEMIC_YEAR_ID)
                                            VALUES
                                            (
                                            ?BATCH_ID,
                                            ?ACADEMIC_YEAR_ID);";
                        break;
                    }
                case AcademicSQLCommands.UpdateAcademicBatchInfo:
                    {
                        query = @"UPDATE ACADEMIC_BATCHES
                                                        SET
                                                        AC_BATCH_ID = ?AC_BATCH_ID,
                                                        BATCH_ID = ?BATCH_ID,
                                                        ACADEMIC_YEAR_ID = ?ACADEMIC_YEAR_ID
                                                        WHERE AC_BATCH_ID = ?AC_BATCH_ID;";
                        break;
                    }
                #endregion
                #region CourseType
                case AcademicSQLCommands.FetchCourseTypeInfo:
                    {
                        query = @"SELECT 
                                        CT.COURSE_TYPE_ID,
                                        CT.COURSE_TYPE,
                                        CT.NO_OF_COMPONENTS,
                                        CT.COURSE_TYPE_ORDER,
                                        CT.IS_STU_BASED_SELECTION,
                                        CT.INTERNAL,
                                        CT.EXTERNAL,
                                        CT.TOTAL,
                                        CT.HOURS,
                                        PT.PART,
                                        PL.PROGRAMME_LEVEL,
                                        CT.CREDITS
                                    FROM
                                        CP_COURSE_TYPE_?AC_YEAR AS CT
		                                    LEFT JOIN
	                                    SUP_PROGRAMME_LEVEL AS PL ON PL.PROGRAMME_LEVEL_ID=CT.PROGRAMME_LEVEL
		                                    LEFT JOIN
	                                    SUP_PART AS PT ON PT.PART_ID=CT.PART_ID WHERE CT.IS_DELETED=0;";
                        break;
                    }
                case AcademicSQLCommands.CheckCourseType:
                    {
                        query = @"SELECT 
                                        COURSE_TYPE_ID,COURSE_TYPE,INTERNAL,EXTERNAL,TOTAL
                                    FROM
                                        CP_COURSE_TYPE_?AC_YEAR
                                    WHERE
                                        COURSE_TYPE = ?COURSE_TYPE;";
                        break;
                    }
                case AcademicSQLCommands.InsertCourseTypeInfo:
                    {
                        query = @"INSERT INTO CP_COURSE_TYPE_?AC_YEAR
                                                                    (
                                                                    COURSE_TYPE,
                                                                    NO_OF_COMPONENTS,
                                                                    COURSE_TYPE_ORDER,
                                                                    IS_STU_BASED_SELECTION,
                                                                    INTERNAL,
                                                                    EXTERNAL,
                                                                    TOTAL,
                                                                    HOURS,
                                                                    PART_ID,
                                                                    PROGRAMME_LEVEL,
                                                                    CREDITS)
                                                                    VALUES
                                                                    (
                                                                    ?COURSE_TYPE,
                                                                    ?NO_OF_COMPONENTS,
                                                                    ?COURSE_TYPE_ORDER,
                                                                    ?IS_STU_BASED_SELECTION,
                                                                    ?INTERNAL,
                                                                    ?EXTERNAL,
                                                                    ?TOTAL,
                                                                    ?HOURS,
                                                                    ?PART_ID,
                                                                    ?PROGRAMME_LEVEL,
                                                                    CREDITS);";
                        break;
                    }
                case AcademicSQLCommands.FetchCourseTypeInfoById:
                    {
                        query = @"SELECT 
                                        COURSE_TYPE_ID,
                                        COURSE_TYPE,
                                        NO_OF_COMPONENTS,
                                        COURSE_TYPE_ORDER,
                                        IS_STU_BASED_SELECTION,
                                        INTERNAL,
                                        EXTERNAL,
                                        TOTAL,
                                        HOURS,
                                        PART_ID,
                                        PROGRAMME_LEVEL,
                                        CREDITS
                                    FROM
                                        CP_COURSE_TYPE_?AC_YEAR WHERE COURSE_TYPE_ID=?COURSE_TYPE_ID";
                        break;
                    }
                case AcademicSQLCommands.UpdateCourseTypeInfo:
                    {
                        query = @"UPDATE CP_COURSE_TYPE_?AC_YEAR
                                                                SET
                                                                COURSE_TYPE_ID= ?COURSE_TYPE_ID,
                                                                COURSE_TYPE= ?COURSE_TYPE,
                                                                NO_OF_COMPONENTS= ?NO_OF_COMPONENTS,
                                                                COURSE_TYPE_ORDER= ?COURSE_TYPE_ORDER,
                                                                IS_STU_BASED_SELECTION= ?IS_STU_BASED_SELECTION,
                                                                INTERNAL= ?INTERNAL,
                                                                EXTERNAL= ?EXTERNAL,
                                                                TOTAL= ?TOTAL,
                                                                HOURS= ?HOURS,
                                                                PART_ID= ?PART_ID,
                                                                PROGRAMME_LEVEL= ?PROGRAMME_LEVEL,
                                                                CREDITS= ?CREDITS
                                                                WHERE COURSE_TYPE_ID =?COURSE_TYPE_ID;";
                        break;
                    }
                case AcademicSQLCommands.DeleteCourseTypeInfo:
                    {
                        query = @"UPDATE CP_COURSE_TYPE_?AC_YEAR
                                                                SET
                                                                IS_DELETED = 1
                                                                WHERE COURSE_TYPE_ID =?COURSE_TYPE_ID;";
                        break;
                    }
                #endregion
                #region Programme
                case AcademicSQLCommands.FetchProgrammeInfo:
                    {
                        query = @"SELECT 
                                        PR.PROGRAMME_ID,
                                        PR.PROGRAMME_CODE,
                                        PR.PROGRAMME_NAME,
                                        PR.PROGRAMME_DESCRIPTION,
                                        DT.DEPARTMENT,
                                        DE.DEGREE,
                                        PR.PROGRAMME_ORDER,
                                        PR.DURATION,
                                        DU.DURATION_UNIT,
                                        PR.NO_OF_SEMESTER,
                                        CH.CHANNEL,
                                        PR.PRO_SERIES_ROLLNO,
                                        SUB.SUBJECT,
                                        PR.PRO_SERIES_REGNO,
                                        PR.PRO_SERIES_ADMNO,
                                        ME.MEDIUM_OF_INSTRUCTION,
                                        IA.IS_AIDED_NAME,
                                        NA.NON_AIDED_NAME,
                                        IR.IS_REGULAR_NAME
                                    FROM
                                        CP_PROGRAMME_?AC_YEAR AS PR
                                            LEFT JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DT ON DT.DEPARTMENT_ID = PR.DEPARTMENT
                                            LEFT JOIN
                                        CP_DEGREE_?AC_YEAR AS DE ON DE.DEGREE_ID = PR.DEGREE
                                            LEFT JOIN
                                        SUP_DURATION_UNIT AS DU ON DU.DURATION_UNIT_ID = PR.DURATION_UNIT
		                                    LEFT JOIN
	                                    SUP_CHANNEL AS CH ON CH.CHANNEL_ID=PR.CHANNEL
		                                    LEFT JOIN
	                                    SUP_SUBJECT AS SUB ON SUB.SUBJECT_ID=PR.SUBJECTS
		                                    LEFT JOIN
	                                    SUP_MEDIUM_OF_INSTRUCTION AS ME ON ME.MEDIUM_ID=PR.MEDIUM_OF_INSTRACTION
		                                    LEFT JOIN
	                                    SUP_IS_AIDED AS IA ON IA.IS_AIDED_ID=PR.IS_AIDED
		                                    LEFT JOIN
	                                    SUP_NON_AIDED AS NA ON NA.NON_AIDED_ID=PR.NON_AIDED
		                                    LEFT JOIN
	                                    SUP_IS_REGULAR AS IR ON IR.IS_REGULAR_ID=PR.IS_REGULAR WHERE PR.IS_DELETED=0";
                        break;
                    }
                case AcademicSQLCommands.CheckProgramme:
                    {
                        query = @"SELECT 
                                        PROGRAMME_ID, PROGRAMME_CODE, PROGRAMME_NAME
                                    FROM
                                        CP_PROGRAMME_?AC_YEAR
                                    WHERE
                                        PROGRAMME_CODE =?PROGRAMME_CODE;";
                        break;
                    }
                case AcademicSQLCommands.InsertProgrammeInfo:
                    {
                        query = @"INSERT INTO CP_PROGRAMME_?AC_YEAR
                                                                (
                                                                PROGRAMME_CODE,
                                                                PROGRAMME_NAME,
                                                                PROGRAMME_DESCRIPTION,
                                                                DEPARTMENT,
                                                                DEGREE,
                                                                PROGRAMME_ORDER,
                                                                DURATION_UNIT,
                                                                NO_OF_SEMESTER,
                                                                CHANNEL,
                                                                IS_AIDED,
                                                                PRO_SERIES_ROLLNO,
                                                                NON_AIDED,
                                                                IS_REGULAR,
                                                                SUBJECTS,
                                                                PRO_SERIES_REGNO,
                                                                PRO_SERIES_ADMNO,
                                                                MEDIUM_OF_INSTRACTION)
                                                                VALUES
                                                                (
                                                                ?PROGRAMME_CODE,
                                                                ?PROGRAMME_NAME,
                                                                ?PROGRAMME_DESCRIPTION,
                                                                ?DEPARTMENT,
                                                                ?DEGREE,
                                                                ?PROGRAMME_ORDER,
                                                                ?DURATION_UNIT,
                                                                ?NO_OF_SEMESTER,
                                                                ?CHANNEL,
                                                                ?IS_AIDED,
                                                                ?PRO_SERIES_ROLLNO,
                                                                ?NON_AIDED,
                                                                ?IS_REGULAR,
                                                                ?SUBJECTS,
                                                                ?PRO_SERIES_REGNO,
                                                                ?PRO_SERIES_ADMNO,
                                                                ?MEDIUM_OF_INSTRACTION);";
                        break;
                    }
                case AcademicSQLCommands.FetchProgrammeInfoById:
                    {
                        query = @"SELECT 
                                        PROGRAMME_ID,
                                        PROGRAMME_CODE,
                                        PROGRAMME_NAME,
                                        PROGRAMME_DESCRIPTION,
                                        DEPARTMENT,
                                        DEGREE,
                                        PROGRAMME_ORDER,
                                        DURATION_UNIT,
                                        NO_OF_SEMESTER,
                                        CHANNEL,
                                        IS_AIDED,
                                        PRO_SERIES_ROLLNO,
                                        NON_AIDED,
                                        IS_REGULAR,
                                        SUBJECTS,
                                        PRO_SERIES_REGNO,
                                        PRO_SERIES_ADMNO,
                                        MEDIUM_OF_INSTRACTION
                                    FROM
                                        CP_PROGRAMME_?AC_YEAR
                                    WHERE PROGRAMME_ID=?PROGRAMME_ID;";
                        break;
                    }
                case AcademicSQLCommands.UpdateProgrammeInfo:
                    {
                        query = @"UPDATE CP_PROGRAMME_?AC_YEAR
                                                                SET
                                                                PROGRAMME_ID= ?PROGRAMME_ID,
                                                                PROGRAMME_CODE= ?PROGRAMME_CODE,
                                                                PROGRAMME_NAME= ?PROGRAMME_NAME,
                                                                PROGRAMME_DESCRIPTION= ?PROGRAMME_DESCRIPTION,
                                                                DEPARTMENT= ?DEPARTMENT,
                                                                DEGREE= ?DEGREE,
                                                                PROGRAMME_ORDER= ?PROGRAMME_ORDER,
                                                                DURATION_UNIT= ?DURATION_UNIT,
                                                                NO_OF_SEMESTER= ?NO_OF_SEMESTER,
                                                                CHANNEL= ?CHANNEL,
                                                                IS_AIDED= ?IS_AIDED,
                                                                PRO_SERIES_ROLLNO= ?PRO_SERIES_ROLLNO,
                                                                NON_AIDED= ?NON_AIDED,
                                                                IS_REGULAR= ?IS_REGULAR,
                                                                SUBJECTS= ?SUBJECTS,
                                                                PRO_SERIES_REGNO= ?PRO_SERIES_REGNO,
                                                                PRO_SERIES_ADMNO= ?PRO_SERIES_ADMNO,
                                                                MEDIUM_OF_INSTRACTION= ?MEDIUM_OF_INSTRACTION
                                                                WHERE PROGRAMME_ID= ?PROGRAMME_ID;";
                        break;
                    }
                case AcademicSQLCommands.DeleteProgrammeInfo:
                    {
                        query = @"UPDATE CP_PROGRAMME_?AC_YEAR
                                                            SET
                                                            IS_DELETED = 1
                                                            WHERE PROGRAMME_ID =?PROGRAMME_ID;";
                        break;
                    }
                #endregion
                #region Degree
                case AcademicSQLCommands.FetchDegreeInfo:
                    {
                        query = @"SELECT 
                                        DE.DEGREE_ID,
                                        DE.DEGREE,
                                        PT.PROGRAMME_TYPE,
                                        PL.PROGRAMME_LEVEL,
                                        DE.DEGREE_ORDER,
                                        FY.FACULTY,
                                        DE.BOS_NAME,
                                        DE.PREFIX,
                                        DE.BOS_SERIES_ROLLNO,
                                        DE.BOS_SERIES_REGNO,
                                        DE.BOS_SERIES_ADMNO
                                    FROM
                                        CP_DEGREE_?AC_YEAR AS DE
                                            INNER JOIN
                                        SUP_PROGRAMME_TYPE AS PT ON PT.PROGRAMME_TYPE_ID = DE.PROGRAMME_TYPE
                                            INNER JOIN
                                        SUP_PROGRAMME_LEVEL AS PL ON PL.PROGRAMME_LEVEL_ID = DE.PROGRAMME_LEVEL
                                            INNER JOIN
                                        CP_FACULTY_?AC_YEAR AS FY ON FY.FACULTY_ID = DE.FACULTY
                                    WHERE DE.IS_DELETED=0;";
                        break;
                    }
                case AcademicSQLCommands.CheckDegree:
                    {
                        query = @"SELECT 
                                        DEGREE_ID,
                                        DEGREE,
                                        PROGRAMME_TYPE,
                                        PROGRAMME_LEVEL,
                                        DEGREE_ORDER,
                                        FACULTY,
                                        BOS_NAME,
                                        PREFIX,
                                        BOS_SERIES_ROLLNO,
                                        BOS_SERIES_REGNO,
                                        BOS_SERIES_ADMNO
                                    FROM
                                        CP_DEGREE_?AC_YEAR WHERE PREFIX=?PREFIX;";
                        break;
                    }
                case AcademicSQLCommands.InsertDegreeInfo:
                    {
                        query = @"INSERT INTO CP_DEGREE_?AC_YEAR
                                                                (
                                                                DEGREE,
                                                                PROGRAMME_TYPE,
                                                                PROGRAMME_LEVEL,
                                                                DEGREE_ORDER,
                                                                FACULTY,
                                                                BOS_NAME,
                                                                PREFIX,
                                                                BOS_SERIES_ROLLNO,
                                                                BOS_SERIES_REGNO,
                                                                BOS_SERIES_ADMNO)
                                                                VALUES
                                                                (
                                                                ?DEGREE,
                                                                ?PROGRAMME_TYPE,
                                                                ?PROGRAMME_LEVEL,
                                                                ?DEGREE_ORDER,
                                                                ?FACULTY,
                                                                ?BOS_NAME,
                                                                ?PREFIX,
                                                                ?BOS_SERIES_ROLLNO,
                                                                ?BOS_SERIES_REGNO,
                                                                ?BOS_SERIES_ADMNO);";
                        break;
                    }
                case AcademicSQLCommands.FetchDegreeInfoById:
                    {
                        query = @"SELECT 
                                        DEGREE_ID,
                                        DEGREE,
                                        PROGRAMME_TYPE,
                                        PROGRAMME_LEVEL,
                                        DEGREE_ORDER,
                                        FACULTY,
                                        BOS_NAME,
                                        PREFIX,
                                        BOS_SERIES_ROLLNO,
                                        BOS_SERIES_REGNO,
                                        BOS_SERIES_ADMNO
                                    FROM
                                        CP_DEGREE_?AC_YEAR
                                    WHERE DEGREE_ID=?DEGREE_ID;";
                        break;
                    }
                case AcademicSQLCommands.UpdateDegreeInfo:
                    {
                        query = @"UPDATE CP_DEGREE_?AC_YEAR
                                                                SET
                                                                DEGREE_ID= ?DEGREE_ID,
                                                                DEGREE= ?DEGREE,
                                                                PROGRAMME_TYPE= ?PROGRAMME_TYPE,
                                                                PROGRAMME_LEVEL= ?PROGRAMME_LEVEL,
                                                                DEGREE_ORDER= ?DEGREE_ORDER,
                                                                FACULTY= ?FACULTY,
                                                                BOS_NAME= ?BOS_NAME,
                                                                PREFIX= ?PREFIX,
                                                                BOS_SERIES_ROLLNO= ?BOS_SERIES_ROLLNO,
                                                                BOS_SERIES_REGNO= ?BOS_SERIES_REGNO,
                                                                BOS_SERIES_ADMNO= ?BOS_SERIES_ADMNO
                                                                WHERE DEGREE_ID= ?DEGREE_ID;";
                        break;
                    }
                case AcademicSQLCommands.DeleteDegreeInfo:
                    {
                        query = @"UPDATE CP_DEGREE_?AC_YEAR
                                                            SET
                                                            IS_DELETED = 1
                                                            WHERE DEGREE_ID =?DEGREE_ID;";
                        break;
                    }
                    #endregion
            }
            return query;
        }
    }
}
