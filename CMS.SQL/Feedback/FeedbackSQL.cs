using CMS.DAO;

namespace CMS.SQL.Feedback
{
    public static class FeedbackSQL
    {
        public static string GetFeedbackSQL(FeedbackSQLCommands sEnumCommand)
        {
            string query = "";


            switch (sEnumCommand)
            {
                case FeedbackSQLCommands.FetchFBObjectives:
                    {
                        query = @"SELECT 
                                        OBJECTIVE_ID, OBJECTIVES, RATING, OBJECTIVESHORTTERM
                                    FROM
                                        FBOBJECTIVES
                                    WHERE
                                        QUESTION_TYPE = ?QUESTION_TYPE
                                    ORDER BY OBJECTIVE_ORDER;";
                        break;
                    }

                case FeedbackSQLCommands.FectchFBQuestions:
                    {
                        query = @"SELECT 
                                    QUESTION, QUESTION_ID, GROUP_TITLE AS QUESTION_TYPE
                                FROM
                                    FBFEEDBACK_QUESTIONS AS FQ
                                        INNER JOIN
                                    fbquestions_group AS FG ON FG.QUESTION_GROUP_ID = FQ.QUESTION_GROUP
                                        AND FG.QUESTION_GROUP_ID in (?QUESTION_GROUP_ID);";

                        break;
                    }

                case FeedbackSQLCommands.FetchFBClasswiseStaff:
                    {
                        query = @"SELECT 
                                        FB_ID,
                                        FS.CLASS_ID,
                                        ST.DEPARTMENT AS 'DEPARTMENT_ID',
                                        FS.STAFF_ID,
                                        CONCAT(IF(ST.FIRST_NAME IS NULL,
                                                    '',
                                                    ST.FIRST_NAME),
                                                ' ',
                                                IF(ST.LAST_NAME IS NULL,
                                                    '',
                                                    ST.LAST_NAME)) AS STAFF_NAME
                                    FROM
                                        FBCLASS_WISE_STAFF?AC_YEAR AS FS
                                            INNER JOIN
                                        stf_personal_info AS ST ON FS.STAFF_ID = ST.STAFF_ID
                                            INNER JOIN
                                        FBFEEDBACK_SETTINGS AS FSTG ON FSTG.FEEDBACK_ID = FS.FEEDBACK_ID
                                    WHERE
                                        FSTG.IS_ACTIVE = 1 AND FS.CLASS_ID=?CLASS_ID;";
                        break;
                    }
                case FeedbackSQLCommands.FetchFBClassesByStaffId:
                    {

                        query = @"SELECT 
                                        CLS.CLASS_ID,
                                        CLS.CLASS_DESCRIPTION,
                                        SEM.PROGRAMME,
                                        FCS.COURSE_ID,
                                        RT.SEMESTER_ID
                                    FROM
                                        FBCLASS_WISE_STAFF?AC_YEAR AS FCS
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = FCS.CLASS_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.PROGRAMME = CLS.PROGRAMME
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID = FCS.COURSE_ID
                                    WHERE
                                        STAFF_ID = ?STAFF_ID
                                            AND SEM.PROGRAMME = CLS.PROGRAMME
                                            AND RT.SEMESTER_ID = SEM.SEMESTER
                                            AND SEM.IS_ACTIVE = 1
                                    GROUP BY CLASS_ID;";

                        break;
                    }
                case FeedbackSQLCommands.FetchStudentInfo:
                    {
                        query = @"SELECT 
                                    SP.STUDENT_ID,
                                    SP.FIRST_NAME,
                                    CLS.CLASS_CODE,
                                    PRO.PROGRAMME_CODE,
                                    DEP.DEPARTMENT,
                                    BT.BATCH_NAME,
                                    CLS.CLASS_ID
                                FROM
                                    STUDENT_PERSONAL_INFO AS SP
                                        INNER JOIN
                                    STUDENT_CLASS AS SCLS ON SCLS.STUDENT_ID = SP.STUDENT_ID
                                        INNER JOIN
                                    ACD_CLASSES?AC_YEAR AS CLS ON CLS.CLASS_ID = SCLS.CLASS_ID
                                        INNER JOIN
                                    ACD_PROGRAMME?AC_YEAR AS PRO ON PRO.PROGRAMME_ID = SP.PROGRAM_ID
                                        INNER JOIN
                                    ACD_DEPARTMENT?AC_YEAR AS DEP ON DEP.DEPARTMENT_ID = PRO.DEPARTMENT
                                        INNER JOIN
                                    ACD_SEMESTER_ROOT?AC_YEAR AS SEM ON SEM.BATCH = SP.BATCH
                                        AND SEM.IS_ACTIVE = 1
                                        INNER JOIN
                                    ACD_BATCH AS BT ON BT.BATCH_ID = SP.BATCH
                                WHERE
                                    SP.STUDENT_ID = ?STUDENT_ID
                                GROUP BY SEM.BATCH;
                                    ;";
                        break;
                    }

                case FeedbackSQLCommands.FetchOverallRatingForStaff:
                    {

                        query = @"SELECT 
                                        COUNT(FE.ANSWER) AS OBJECTIVES_COUNT,
                                        COUNT(FE.ASSESSOR)AS STUDENT_COUNT,
                                        STP.FIRST_NAME,
                                        CLS.CLASS_CODE,
                                        FO.OBJECTIVES,
                                        FE.QUESTION
                                    FROM
                                        fbevaluation?AC_YEAR AS FE
                                            INNER JOIN
                                        acd_classes?AC_YEAR AS CLS ON CLS.CLASS_ID = FE.CLASS_ID
                                            INNER JOIN
                                        staff_personal_info AS STP ON FE.ASSESSEE = STP.STAFF_NO
                                            INNER JOIN
                                        fbobjectives AS FO ON FO.OBJECTIVE_ID = FE.ANSWER
                                    GROUP BY FE.ASSESSEE, FE.ANSWER
                                    ORDER BY STP.FIRST_NAME, FO.OBJECTIVE_ORDER";
                        break;
                    }

                case FeedbackSQLCommands.FetchOverallRatingByStaffId:
                    {
                        query = @"";
                        break;
                    }

                case FeedbackSQLCommands.FetchQuestionWiseRatingByStaffId:
                    {
                        query = @"SELECT 
                                    COUNT(FE.ASSESSOR) AS ASSESSOR,
                                    FQ.QUESTION,
                                    FG.GROUP_TITLE,	                                   
                                    COUNT(IF(FE.ANSWER = 1, 1, NULL)) AS VERY_GOOD,
									COUNT(IF(FE.ANSWER = 2, 1, NULL)) AS GOOD,
									COUNT(IF(FE.ANSWER = 3, 1, NULL)) AS SATISFACTORY,
									COUNT(IF(FE.ANSWER = 4, 1, NULL)) AS POOR,
									COUNT(IF(FE.ANSWER = 5, 1, NULL)) AS VERY_POOR
                                FROM
                                    FBEVALUATION?AC_YEAR AS FE
                                        INNER JOIN
                                    FBFEEDBACK_QUESTIONS AS FQ ON FQ.QUESTION_ID = FE.QUESTION
                                        INNER JOIN
                                    FBQUESTIONS_GROUP AS FG ON FG.QUESTION_GROUP_ID = FQ.QUESTION_GROUP
                                WHERE
                                    FE.COURSE_ID =?COURSE_ID
                                        AND ASSESSEE =?ASSESSEE
                                            AND CLASS_ID=?CLASS_ID        
                                GROUP BY FE.QUESTION ORDER BY FG.QUESTION_GROUP_ID;";
                        break;
                    }

                case FeedbackSQLCommands.GetAssessorCountByClassID:
                    {
                        break;
                    }
                case FeedbackSQLCommands.FetchQuestionwiseReportByDepartmentsIds:
                    {
                        query = @"SELECT 
                                    CONCAT(IF(STF.FIRST_NAME IS NULL,
                                                '',
                                                STF.FIRST_NAME),
                                            ' ',
                                            IF(STF.LAST_NAME IS NULL,
                                                '',
                                                STF.LAST_NAME)) AS 'STAFF_NAME',
                                    STF.STAFF_CODE,
                                    FE.COURSE_ID,
                                    CR.COURSE_TITLE,
                                    CT.COURSE_TYPE,
                                    CR.COURSE_CODE,
                                    FQ.QUESTION,
                                    FG.GROUP_TITLE,
                                    COUNT(IF(FE.ANSWER = 1, 1, NULL)) AS VERY_GOOD,
                                    COUNT(IF(FE.ANSWER = 2, 1, NULL)) AS GOOD,
                                    COUNT(IF(FE.ANSWER = 3, 1, NULL)) AS SATISFACTORY,
                                    COUNT(IF(FE.ANSWER = 4, 1, NULL)) AS POOR,
                                    COUNT(IF(FE.ANSWER = 5, 1, NULL)) AS VERY_POOR,
                                    FE.ASSESSEE,
                                    DP.DEPARTMENT,
                                    CLS.CLASS_NAME,
                                    COUNT(FE.ASSESSOR) AS ASSESSOR,
                                   
                                                        (SELECT 
                                        COUNT(*)
                                    FROM
                                        CP_CLASS_COURSE_?AC_YEAR AS SCLS
                                    WHERE
                                        SCLS.COURSE_ID = CR.COURSE_ROOT_ID) AS TOTAL_STUDENTS
                                FROM
                                    FBEVALUATION?AC_YEAR AS FE
                                        INNER JOIN
                                    FBFEEDBACK_QUESTIONS AS FQ ON FQ.QUESTION_ID = FE.QUESTION
                                        INNER JOIN
                                    FBQUESTIONS_GROUP AS FG ON FG.QUESTION_GROUP_ID = FQ.QUESTION_GROUP
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = FE.COURSE_ID
                                        INNER JOIN
                                    CP_COURSE_TYPE_?AC_YEAR AS CT ON CR.COURSE_TYPE = CT.COURSE_TYPE_ID
                                        INNER JOIN
                                    CP_DEPARTMENT_?AC_YEAR AS DP ON DP.DEPARTMENT_ID = CR.DEPARTMENT
                                        INNER JOIN
                                    CP_CLASSES_?AC_YEAR AS CLS ON FE.CLASS_ID = CLS.CLASS_ID
                                        INNER JOIN
                                    STF_PERSONAL_INFO AS STF ON STF.STAFF_ID = FE.ASSESSEE
										INNER JOIN
									ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.PROGRAMME=CLS.PROGRAMME
                                WHERE
                                     STF.DEPARTMENT IN (?DEPARTMENT) AND CLS.SHIFT IN (?SHIFT)AND CR.SEMESTER_ID=SEM.SEMESTER AND SEM.PROGRAMME=CLS.PROGRAMME AND SEM.IS_ACTIVE=1
                                GROUP BY FE.QUESTION , FE.ASSESSEE , FE.COURSE_ID,FE.CLASS_ID,STF.STAFF_CODE
                                ORDER BY FE.ASSESSEE , FG.QUESTION_GROUP_ID;";

                        break;
                    }

                case FeedbackSQLCommands.FetchQuestionWiseReportByStaffIds:
                    {
                        query = @"SELECT 
                                        CONCAT(IF(STF.FIRST_NAME IS NULL,
                                                    '',
                                                    STF.FIRST_NAME),
                                                ' ',
                                                IF(STF.LAST_NAME IS NULL,
                                                    '',
                                                    STF.LAST_NAME)) AS 'STAFF_NAME',
                                        STF.STAFF_CODE,
                                        FE.COURSE_ID,
                                        CR.COURSE_TITLE,
                                        CT.COURSE_TYPE,
                                        CR.COURSE_CODE,
                                        FQ.QUESTION,
                                        FG.GROUP_TITLE,
                                        COUNT(IF(FE.ANSWER = 1, 1, NULL)) AS VERY_GOOD,
                                        COUNT(IF(FE.ANSWER = 2, 1, NULL)) AS GOOD,
                                        COUNT(IF(FE.ANSWER = 3, 1, NULL)) AS SATISFACTORY,
                                        COUNT(IF(FE.ANSWER = 4, 1, NULL)) AS POOR,
                                        COUNT(IF(FE.ANSWER = 5, 1, NULL)) AS VERY_POOR,
                                        FE.ASSESSEE,
                                        DP.DEPARTMENT,
                                        CLS.CLASS_NAME,
                                        COUNT(FE.ASSESSOR) AS ASSESSOR,
                                        (SELECT 
                                                COUNT(*)
                                            FROM
                                                STU_COURSE_INFO_?AC_YEAR AS SCLS
                                            WHERE
                                                SCLS.S_CLASS_ID = CLS.CLASS_ID
                                                    AND SCLS.COURSE_ID = CR.COURSE_ROOT_ID) AS TOTAL_STUDENTS
                                    FROM
                                        FBEVALUATION?AC_YEAR AS FE
                                            INNER JOIN
                                        FBFEEDBACK_QUESTIONS AS FQ ON FQ.QUESTION_ID = FE.QUESTION
                                            INNER JOIN
                                        FBQUESTIONS_GROUP AS FG ON FG.QUESTION_GROUP_ID = FQ.QUESTION_GROUP
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = FE.COURSE_ID
                                            INNER JOIN
                                        CP_COURSE_TYPE_?AC_YEAR AS CT ON CR.COURSE_TYPE = CT.COURSE_TYPE_ID
                                            INNER JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DP ON DP.DEPARTMENT_ID = CR.DEPARTMENT
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON FE.CLASS_ID = CLS.CLASS_ID
                                            INNER JOIN
                                        STF_PERSONAL_INFO AS STF ON STF.STAFF_ID = FE.ASSESSEE
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.PROGRAMME = CLS.PROGRAMME
                                    WHERE
                                        ASSESSEE IN (?ASSESSEE)
                                            AND SEM.PROGRAMME = CLS.PROGRAMME
                                            AND SEM.SEMESTER = CR.SEMESTER_ID
                                            AND SEM.IS_ACTIVE = 1
                                    GROUP BY FE.QUESTION , FE.ASSESSEE , FE.COURSE_ID , FE.CLASS_ID
                                    ORDER BY FE.ASSESSEE , FG.QUESTION_GROUP_ID;";

                        break;
                    }
                case FeedbackSQLCommands.FetchStudentCourseListByClassId:
                    {

                        query = @"SELECT 
                                        CC.COURSE_ID, RT.COURSE_TITLE
                                    FROM
                                        STU_PERSONAL_INFO AS ST
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ST.CLASS_ID
                                            INNER JOIN
                                        CP_CLASS_COURSE_?AC_YEAR AS CC ON CC.CLASS_ID = ST.CLASS_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID = CC.COURSE_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.PROGRAMME = CLS.PROGRAMME
                                    WHERE
                                        ST.STUDENT_ID = ?STUDENT_ID
                                            AND SEM.BATCH = ST.BATCH
                                            AND SEM.PROGRAMME = CLS.PROGRAMME
                                            AND RT.SEMESTER_ID = SEM.SEMESTER
                                            AND SEM.IS_ACTIVE = 1 
                                    UNION SELECT 
                                        RT.COURSE_ROOT_ID, RT.COURSE_TITLE
                                    FROM
                                        STU_PERSONAL_INFO AS ST
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ST.CLASS_ID
                                            INNER JOIN
                                        CP_CLASS_COURSE_?AC_YEAR AS CC ON CC.CLASS_ID = ST.CLASS_ID
                                            INNER JOIN
                                        STU_COURSE_INFO_?AC_YEAR AS SC ON SC.STUDENT_ID = ST.STUDENT_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID = SC.COURSE_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.PROGRAMME = CLS.PROGRAMME
                                    WHERE
                                        SC.STUDENT_ID = ?STUDENT_ID
                                            AND SC.CLASS_ID != SC.S_CLASS_ID
                                            AND SEM.BATCH = ST.BATCH
                                            AND RT.SEMESTER_ID = SEM.SEMESTER
                                            AND SEM.PROGRAMME = CLS.PROGRAMME
                                            AND SEM.IS_ACTIVE = 1
                                    GROUP BY SC.COURSE_ID;";
                        break;
                    }
                case FeedbackSQLCommands.FetchFBCoursewiseStaff:
                    {

                        query = @"SELECT 
                                        CONCAT(IF(SP.FIRST_NAME IS NULL,
                                                    '',
                                                    SP.FIRST_NAME),
                                                ' ',
                                                IF(SP.LAST_NAME IS NULL,
                                                    '',
                                                    SP.LAST_NAME)) AS STAFF_NAME,
                                        S.STAFF_ID,
                                        S.CLASS_ID
                                    FROM
                                        FBCLASS_WISE_STAFF?AC_YEAR S
                                            INNER JOIN
                                        STF_PERSONAL_INFO SP ON SP.STAFF_ID = S.STAFF_ID
                                    WHERE
                                        s.class_id = (SELECT 
                                                class_id
                                            FROM
                                                STU_PERSONAL_INFO AS sc
                                            WHERE
                                                sc.STUDENT_ID =?STUDENT_ID
                                            LIMIT 1)
                                            AND s.COURSE_ID = ?COURSE_ID
                                    UNION
SELECT 
                                        CONCAT(IF(SP.FIRST_NAME IS NULL,
                                                    '',
                                                    SP.FIRST_NAME),
                                                ' ',
                                                IF(SP.LAST_NAME IS NULL,
                                                    '',
                                                    SP.LAST_NAME)) AS STAFF_NAME,
                                        S.STAFF_ID,
                                        S.CLASS_ID
                                    FROM
                                        FBCLASS_WISE_STAFF?AC_YEAR S
                                            INNER JOIN
                                        STF_PERSONAL_INFO SP ON SP.STAFF_ID = S.STAFF_ID
                                    WHERE
                                        s.class_id = (SELECT 
                                                S_CLASS_ID
                                            FROM
                                                stu_course_info_?AC_YEAR AS sc
                                            WHERE
                                                sc.STUDENT_ID =?STUDENT_ID AND COURSE_ID = ?COURSE_ID
                                            LIMIT 1)
                                            AND s.COURSE_ID = ?COURSE_ID;";
                        break;
                    }
                case FeedbackSQLCommands.ValidateCourseByStudentId:
                    {
                        query = @"SELECT 
                                        COUNT(*) > 0 'STATUS'
                                    FROM
                                        FBEVALUATION?AC_YEAR
                                    WHERE
                                        ASSESSEE =?ASSESSEE AND COURSE_ID =?COURSE_ID
                                            AND ASSESSOR =?ASSESSOR;";
                        break;
                    }
                case FeedbackSQLCommands.CheckCourseTypeByCourseId:
                    {
                        query = @"SELECT 
                                        COUNT(*) > 0 AS STATUS
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR AS CR
                                            INNER JOIN
                                        CP_COURSE_TYPE_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID = CR.COURSE_TYPE
                                    WHERE
                                        CR.COURSE_ROOT_ID = ?COURSE_ROOT_ID
                                            AND CT.COURSE_TYPE LIKE '%PRACTICAL%'; ";
                        break;
                    }
                case FeedbackSQLCommands.FetchFeedbackInfo:
                    {

                        query = @"SELECT 
                                        FEEDBACK_NAME, DATE_FROM, DATE_TO, ACADEMIC_YEAR
                                    FROM
                                        FBFEEDBACK_SETTINGS
                                    WHERE
                                        IS_ACTIVE=1;";

                        break;
                    }
                case FeedbackSQLCommands.FetchCourseAndClassInfoByCourse:
                    {
                        query = @"SELECT 
                                        (SELECT 
                                                COUNT(CLS1.STUDENT_ID)
                                            FROM
                                                STU_CLASS AS CLS1
                                            WHERE
                                                CLS1.CLASS_ID = CLS.CLASS_ID) AS TOTAL_STUDENTS,
                                        CR.COURSE_TITLE,
                                        CLS.CLASS_NAME,
                                        CT.COURSE_TYPE,
                                        CR.COURSE_CODE,
                                        DP.DEPARTMENT,
                                        CONCAT(IF(SP.FIRST_NAME IS NULL,
                                                    '',
                                                    SP.FIRST_NAME),
                                                ' ',
                                                IF(SP.LAST_NAME IS NULL,
                                                    '',
                                                    SP.LAST_NAME)) AS STAFF_NAME,
                                        SP.STAFF_CODE
                                    FROM
                                        FBCLASS_WISE_STAFF?AC_YEAR AS FS
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = FS.COURSE_ID
                                            LEFT JOIN
                                        CP_COURSE_TYPE_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID = CR.COURSE_TYPE
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = FS.CLASS_ID
                                            INNER JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DP ON DP.DEPARTMENT_ID = CR.DEPARTMENT
                                            INNER JOIN
                                        STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = FS.STAFF_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.PROGRAMME = CLS.PROGRAMME
                                    WHERE
                                        FS.COURSE_ID = ?COURSE_ID AND FS.STAFF_ID = ?STAFF_ID
                                            AND FS.CLASS_ID = ?CLASS_ID
                                            AND SEM.PROGRAMME = CLS.PROGRAMME
                                            AND SEM.SEMESTER = CR.SEMESTER_ID
                                            AND SEM.IS_ACTIVE = 1
                                    GROUP BY CR.COURSE_CODE;";
                        break;
                    }
                case FeedbackSQLCommands.FetchCourseListByStaffId:
                    {
                        query = @"SELECT 
                                        cs.COURSE_ID, cr.COURSE_TITLE
                                    FROM
                                       fbclass_wise_staff?AC_YEAR AS cs
                                            INNER JOIN
                                        cp_course_root_?AC_YEAR AS cr ON cr.COURSE_ROOT_ID = cs.COURSE_ID
                                    WHERE
                                        STAFF_ID =?STAFF_ID ;";
                        break;
                    }
                case FeedbackSQLCommands.FetchDepartmentList:
                    {

                        query = @"SELECT 
                                    DEPARTMENT, DEPARTMENT_ID
                                FROM
                                    CP_DEPARTMENT_?AC_YEAR;";
                        break;
                    }
                case FeedbackSQLCommands.FetchStaffListByDepartment:
                    {

                        query = @"SELECT 
                                    STAFF_ID,
                                    CONCAT(IF(FIRST_NAME IS NULL, '', FIRST_NAME),
                                            ' ',
                                            IF(LAST_NAME IS NULL, ' ', LAST_NAME)) AS STAFF_NAME
                                FROM
                                   STF_PERSONAL_INFO
                                WHERE
                                    DEPARTMENT =?DEPARTMENT AND CATEGORY =?CATEGORY";
                        break;
                    }
                case FeedbackSQLCommands.FetchShiftList:
                    {

                        query = @"SELECT SHIFT_ID, SHIFT_NAME FROM SUP_SHIFT;";
                        break;
                    }
                case FeedbackSQLCommands.FetchDepartmentByShiftId:
                    {

                        query = @"SELECT 
                                        CLS.DEPARTMENT AS DEPARTMENT_ID , DP.DEPARTMENT
                                    FROM
                                        CP_CLASSES_?AC_YEAR AS CLS
                                            INNER JOIN
                                        CP_DEPARTMENT_?AC_YEAR AS DP ON DP.DEPARTMENT_ID = CLS.DEPARTMENT
                                    WHERE
                                        CLS.SHIFT =?SHIFT
                                    GROUP BY CLS.DEPARTMENT;";
                        break;
                    }
            }
            return query;
        }
    }
}
