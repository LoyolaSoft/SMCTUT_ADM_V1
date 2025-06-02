using CMS.DAO;

namespace CMS.SQL.Examination
{
    public static class ExaminationSQL
    {
        public static string GetExaminationSQL(ExaminationSQLCommands sEnumCommand)
        {
            string query = "";
            switch (sEnumCommand)
            {
                case ExaminationSQLCommands.FetchCIAComponentsByCourseId:
                    {
                        query = @" 
                                    SELECT 
                                ''as COURSE_GROUP_MARK,'' as CIA_GROUP_MARK_ID, CLS.STUDENT_ID,
                                CONCAT(IF(SP.FIRST_NAME IS NULL,
                                            '',
                                            SP.FIRST_NAME),
                                        ' ',
                                        IF(SP.LAST_NAME IS NULL,
                                            '',
                                            SP.LAST_NAME)) NAME,
                                SP.REGISTER_NO,
                                SP.ADMISSION_NO,
                                COURSE_ROOT_ID,
                                MAX_MARK,
                                ORDER_ID,
                                CIA_GROUP_ID,
                                CIA_GROUP_COMPONENT_ID AS COURSE_GROUP_ID,
                                SUP_CIA_COMPONENT_ID AS COMPONENT_ID,
                                COMPONENT,
                                SEMESTER_ID
                            FROM
                                STU_CLASS AS CLS
                                    JOIN
                                (SELECT 
                                    CIA_GROUP_COMPONENT_ID,
                                        CIA_GROUP_ID,
                                        SUP_CIA_COMPONENT_ID,
                                        MAX_MARK,
                                        MIN_MARK,
                                        CGC.ORDER_ID,
                                        SC.COMPONENT,
                                        CR.SEMESTER_ID,
                                        CR.COURSE_ROOT_ID
                                FROM
                                    CIA_GROUP_COMPONENT AS CGC
                                INNER JOIN SUP_CIA_COMPONENTS AS SC ON SC.COMPONENT_ID = CGC.SUP_CIA_COMPONENT_ID
                                    AND SC.IS_DELETED != 1
                                INNER JOIN CP_COURSE_TYPE_GROUP_?AC_YEAR AS CT ON CT.GROUP_ID = CGC.CIA_GROUP_ID
                                    AND CT.IS_DELETED != 1
                                INNER JOIN CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_TYPE = CT.COURSE_TYPE_ID
                                    AND CR.COURSE_ROOT_ID = ?COURSE_ID
                                    AND CR.IS_DELETED != 1
                                WHERE
                                    CGC.IS_DELETED != 1
                                ORDER BY CGC.ORDER_ID) AS CIA_COMPONENT
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CLS.STUDENT_ID
                            INNER JOIN CP_CLASS_COURSE_?AC_YEAR AS CC ON CC.CLASS_ID=CLS.CLASS_ID AND CC.COURSE_ID=?COURSE_ID AND CC.IS_DELETED!=1
                            WHERE
                                CLS.CLASS_ID = ?CLASS_ID
                                    AND ACADEMIC_YEAR = ?AC_YEAR
                                    AND CLS.IS_DELETED != 1   AND SP.IS_LEFT!=1
                            UNION SELECT 
                               ''as COURSE_GROUP_MARK,'' as CIA_GROUP_MARK_ID,  CLS.STUDENT_ID,
                                CONCAT(IF(SP.FIRST_NAME IS NULL,
                                            '',
                                            SP.FIRST_NAME),
                                        ' ',
                                        IF(SP.LAST_NAME IS NULL,
                                            '',
                                            SP.LAST_NAME)) NAME,
                                SP.REGISTER_NO,
                                SP.ADMISSION_NO,
                                COURSE_ROOT_ID,
                                MAX_MARK,
                                ORDER_ID,
                                CIA_GROUP_ID,
                                CIA_GROUP_COMPONENT_ID AS COURSE_GROUP_ID,
                                SUP_CIA_COMPONENT_ID AS COMPONENT_ID,
                                COMPONENT,
                                SEMESTER_ID
                            FROM
                                STU_COURSE_INFO_?AC_YEAR AS CLS
                                    JOIN
                                (SELECT 
                                    CIA_GROUP_COMPONENT_ID,
                                        CIA_GROUP_ID,
                                        SUP_CIA_COMPONENT_ID,
                                        MAX_MARK,
                                        MIN_MARK,
                                        CGC.ORDER_ID,
                                        SC.COMPONENT,
                                        CR.SEMESTER_ID,
                                        CR.COURSE_ROOT_ID
                                FROM
                                    CIA_GROUP_COMPONENT AS CGC
                                INNER JOIN SUP_CIA_COMPONENTS AS SC ON SC.COMPONENT_ID = CGC.SUP_CIA_COMPONENT_ID
                                    AND SC.IS_DELETED != 1
                                INNER JOIN CP_COURSE_TYPE_GROUP_?AC_YEAR AS CT ON CT.GROUP_ID = CGC.CIA_GROUP_ID
                                    AND CT.IS_DELETED != 1
                                INNER JOIN CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_TYPE = CT.COURSE_TYPE_ID
                                    AND CR.COURSE_ROOT_ID = ?COURSE_ID
                                    AND CR.IS_DELETED != 1
                                WHERE
                                    CGC.IS_DELETED != 1
                                ORDER BY CGC.ORDER_ID) AS CIA_COMPONENT
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CLS.STUDENT_ID
                            WHERE
                                CLS.S_CLASS_ID = ?CLASS_ID
                                    AND ACADEMIC_YEAR = ?AC_YEAR
                                    AND COURSE_ID = ?COURSE_ID
                                    AND CLS.IS_DELETED != 1  AND SP.IS_LEFT!=1 ";
                        break;
                    }
                case ExaminationSQLCommands.FetchCIA_Marks_Info:
                    {
                        query = @"SELECT 
                                    CLS.STUDENT_ID,
                                    CIA_GROUP_MARK_ID,
                                    COURSE_GROUP_ID,
                                    COURSE_GROUP_MARK
                                FROM
                                    STU_CLASS AS CLS
                                        LEFT JOIN
                                    CIA_GROUP_MARKS_?AC_YEAR AS CGM ON CLS.STUDENT_ID = CGM.STUDENT_ID
                                        AND CGM.COURSE_ID = ?COURSE_ID
                        INNER JOIN CP_CLASS_COURSE_?AC_YEAR AS CC ON CC.CLASS_ID=CLS.CLASS_ID AND CC.COURSE_ID=?COURSE_ID AND CC.IS_DELETED!=1
                                WHERE
                                    CLS.CLASS_ID = ?CLASS_ID
                                        AND CLS.ACADEMIC_YEAR = ?AC_YEAR AND CLS.IS_DELETED!=1  UNION 
                                        SELECT 
                                    CLS.STUDENT_ID,
                                    CIA_GROUP_MARK_ID,
                                    COURSE_GROUP_ID,
                                    COURSE_GROUP_MARK
                                FROM
                                    STU_COURSE_INFO_?AC_YEAR AS CLS
                                        LEFT JOIN
                                    CIA_GROUP_MARKS_?AC_YEAR AS CGM ON CLS.STUDENT_ID = CGM.STUDENT_ID
                                        AND CGM.COURSE_ID = ?COURSE_ID
                                WHERE
                                    CLS.S_CLASS_ID = ?CLASS_ID AND CLS.COURSE_ID=?COURSE_ID AND CLS.IS_DELETED!=1 
                                        AND CLS.ACADEMIC_YEAR = ?AC_YEAR;                                        
                                        ";
                        break;
                    }
                case ExaminationSQLCommands.FetchExamSettingByAcitveId:
                    {
                        query = @"SELECT 
                        EXAM_SETTING_ID,
                        EXAM_NAME,
                        ACADEMIC_YEAR,
                        M_EXAM,
                        M_PASS,
                        SEMESTER,
                        LAST_DATE_FOR_EXAM_FEE
                    FROM
                        EXAM_SETTING
                    WHERE
                        IS_ACTIVE = 1 AND IS_DELETED != 1;";
                        break;
                    }

                case ExaminationSQLCommands.FetchArrearCourseByProgrammeIdAndBatch:
                    {
                        query = @"SELECT                                  
                                    CR.COURSE_CODE,
                                    CR.COURSE_TITLE,
                                    CR.COURSE_TYPE,
                                    CT.COURSE_TYPE,
                                    CR.SUBJECT_TYPE  AS 'SUBJECT_TYPE_ID',
                                    SM.SEMESTER,
                                    SB.SUBJECT_TYPE
                                FROM
                                    SEMESTER_MARKS_?AC_YEAR AS SM
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = SM.COURSE_ID
                                        LEFT JOIN
                                    SUP_SUBJECT_TYPE AS SB ON SB.SUBJECT_TYPE_ID = CR.SUBJECT_TYPE
                                INNER JOIN CP_COURSE_TYPE_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID=CR.COURSE_TYPE
                                WHERE
                                    SM.STUDENT_ID IN (SELECT 
                                            SP.STUDENT_ID
                                        FROM
                                            STU_PERSONAL_INFO AS SP
                                        WHERE
                                            SP.PROGRAM_ID = ?PROGRAMME_ID AND SP.BATCH = ?BATCH_ID)
                                        AND RESULT IN ('F' , 'A')
                                        AND SM.SEMESTER IN (?SEMESTER)
                                GROUP BY SM.COURSE_ID; ";
                        break;
                    }
                case ExaminationSQLCommands.FetchAcademicCurriculumByProgrammeIdAndBatch:
                    {
                        query = @"SELECT 
                                    CURRICULUM_ID,
                                    PROGRAMME,
                                    BATCH,
                                    COURSE_ID,
                                    CR.SUBJECT_TYPE AS 'SUBJECT_TYPE_ID',
                                    ST.SUBJECT_TYPE,
                                    CR.COURSE_CODE,
                                    CR.COURSE_TITLE,    CT.COURSE_TYPE,
                                    SEMESTER
                                FROM
                                    ACADEMIC_CURRICULUM_?AC_YEAR AS AC
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CR ON AC.COURSE_ID = CR.COURSE_ROOT_ID AND CR.IS_DELETED!=1
                                        LEFT JOIN
                                    SUP_SUBJECT_TYPE AS ST ON ST.SUBJECT_TYPE_ID = CR.SUBJECT_TYPE  LEFT JOIN
                                     CP_COURSE_TYPE_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID = CR.COURSE_TYPE
                                WHERE
                                    AC.IS_ACTIVE = 1 AND AC.IS_DELETED != 1
                                        AND AC.BATCH = ?BATCH_ID
                                        AND AC.PROGRAMME = ?PROGRAMME_ID;";
                        break;

                    }
                case ExaminationSQLCommands.FetchExamFeeStructureByProgrammeIdAndBatch:
                    {
                        query = @"  SELECT 
                                        PROGRAMME_HEAD_ID,
                                        PROGRAMME_ID,
                                        BATCH_ID,
                                        AMOUNT,
                                        EP.HEAD_ID,
                                        SEMESTER,
                                        ACADEMIC_YEAR,
                                        SUBJECT_TYPE,
                                        H.HEAD,EP.EXAM_SETTING_ID
                                    FROM
                                        EXAM_PROGRAMME_WISE_HEADS AS EP
                                         LEFT JOIN SUP_HEAD	 AS H ON H.HEAD_ID=EP.HEAD_ID
                                    WHERE
                                        EP.PROGRAMME_ID = ?PROGRAMME_ID AND EP.BATCH_ID = ?BATCH_ID
                                            AND EP.SEMESTER = (SELECT 
                                                semester
                                            FROM
                                                academic_semester_?AC_YEAR AS s
                                            WHERE
                                                s.PROGRAMME = ?PROGRAMME_ID AND s.BATCH = ?BATCH_ID
                                                    AND s.IS_ACTIVE = 1)
                                            AND EP.IS_DELETED != 1;   ";
                        break;
                    }
                case ExaminationSQLCommands.FetchExamFeeStatus:
                    {
                        query = @"SELECT 
                                    SA.ACADEMIC_YEAR,
                                    SA.FREQUENCY_ID,
                                    HEAD,
                                    SUM(IFNULL(CREDIT, 0)) AS CREDIT,
                                    SUM(IFNULL(DEBIT, 0)) AS DEBIT,
                                    (SUM(IFNULL(CREDIT, 0)) - SUM(IFNULL(DEBIT, 0))) AS BALANCE,
                                    TRANSACTION_DATE
                                FROM
                                    FEE_STUDENT_ACCOUNT AS SA
                                        INNER JOIN
                                    SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = SA.FREQUENCY_ID
                                        INNER JOIN
                                    SUP_SEMESTER_TYPE AS ST ON ST.SEMESTER_TYPE_ID = FF.SEMESTER_TYPE
                                        AND IS_ACTIVE = 1
                                WHERE
                                    SA.STUDENT_ID = ?STUDENT_ID
                                        AND SA.ACADEMIC_YEAR = ?AC_YEAR; ";
                        break;
                    }
                case ExaminationSQLCommands.SaveExamCreditAmountToStudentAccount:
                    {
                        query = @"INSERT INTO FEE_STUDENT_ACCOUNT
                                (STUDENT_ID,
                                ACADEMIC_YEAR,
                                FREQUENCY_ID,
                                HEAD,
                                CREDIT,FEE_MAIN_HEAD_ID)
                                VALUES
                                (
                                ?STUDENT_ID,
                                ?ACADEMIC_YEAR,
                                ?FREQUENCY_ID,
                                ?HEAD,
                                ?CREDIT,?FEE_MAIN_HEAD_ID);";
                        break;
                    }
                case ExaminationSQLCommands.FetchExamCourseHeadsByStudentId:
                    {
                        query = @"SELECT 
                                    COURSE_HEAD_ID,
                                    COURSE_CODE,
                                    CH.PROGRAMME_HEAD_ID,
                                    CH.PROGRAMME_ID,
                                    CH.BATCH_ID,
                                    EP.AMOUNT,
                                    EP.HEAD_ID
                                FROM
                                    EXAM_COURSE_WISE_HEADS AS CH
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.PROGRAM_ID = CH.PROGRAMME_ID
                                        AND SP.BATCH = CH.BATCH_ID
                                        INNER JOIN
                                    EXAM_PROGRAMME_WISE_HEADS AS EP ON EP.PROGRAMME_HEAD_ID = CH.PROGRAMME_HEAD_ID
                                WHERE
                                    CH.IS_DELETED != 1
                                        AND SP.STUDENT_ID = ?STUDENT_ID;";
                        break;
                    }


                case ExaminationSQLCommands.FetchExamProgrameHeadsWithAmountByStudentId:
                    {

                        query = @"SELECT 
                                EP.PROGRAMME_ID,
                                EP.BATCH_ID,
                                EP.HEAD_ID,
                                EP.AMOUNT,
                                EP.ACADEMIC_YEAR,
                                H.HEAD,
                                EP.SUBJECT_TYPE
                            FROM
                                EXAM_PROGRAMME_WISE_HEADS AS EP
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.PROGRAM_ID = EP.PROGRAMME_ID
                                    AND EP.BATCH_ID = SP.BATCH
                                    INNER JOIN
                                SUP_HEAD AS H ON H.HEAD_ID = EP.HEAD_ID
                            WHERE
                                SP.STUDENT_ID = ?STUDENT_ID
                                    AND EP.IS_DELETED != 1;";
                        break;
                    }

                case ExaminationSQLCommands.FetchCourseHeadList:
                    {
                        query = @"SELECT 
                                    COURSE_HEAD_ID,
                                    COURSE_CODE,
                                    CH.PROGRAMME_HEAD_ID,
                                    EP.PROGRAMME_ID,
                                    EP.BATCH_ID,    B.BATCH,
                                    EP.SEMESTER,
                                    PROGRAMME_NAME,
                                    AMOUNT,
                                    ACCOUNT,SH.HEAD,EP.HEAD_ID,CH.EXAM_SETTING_ID
                                FROM
                                    EXAM_COURSE_WISE_HEADS AS CH
                                        INNER JOIN
                                    CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = CH.PROGRAMME_ID
                                        INNER JOIN
                                    EXAM_PROGRAMME_WISE_HEADS AS EP ON EP.PROGRAMME_HEAD_ID = CH.PROGRAMME_HEAD_ID
                                        INNER JOIN
                                    SUP_HEAD AS SH ON SH.HEAD_ID = EP.HEAD_ID
                                INNER JOIN SUP_BATCHES AS B ON B.BATCH_ID=EP.BATCH_ID
                                WHERE
                                    CH.IS_DELETED != 1   AND EP.PROGRAMME_ID = ?PROGRAMME_ID
        AND EP.BATCH_ID = ?BATCH_ID;";
                        break;
                    }
                case ExaminationSQLCommands.GetProgrammeIdByCourseCodeAndBatch:
                    {
                        query = @"SELECT 
                                    PROGRAMME_HEAD_ID,COURSE_HEAD_ID
                                FROM
                                    EXAM_COURSE_WISE_HEADS AS CH
                                WHERE
                                    CH.PROGRAMME_ID = ?PROGRAMME_ID AND CH.BATCH_ID = ?BATCH_ID
                                        AND CH.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                        AND CH.COURSE_CODE = ?COURSE_CODE
                                        AND CH.IS_DELETED != 1;";
                        break;
                    }

                case ExaminationSQLCommands.SaveCourseHeads:
                    {
                        query = @"INSERT INTO EXAM_COURSE_WISE_HEADS
                                    (
                                    COURSE_CODE,
                                    PROGRAMME_HEAD_ID,
                                    PROGRAMME_ID,
                                    BATCH_ID,
                                    SEMESTER,
                                    ACADEMIC_YEAR,EXAM_SETTING_ID)
                                    VALUES
                                    (?COURSE_CODE,
                                    ?PROGRAMME_HEAD_ID,
                                    ?PROGRAMME_ID,
                                    ?BATCH_ID,
                                    ?SEMESTER,
                                    ?ACADEMIC_YEAR,?EXAM_SETTING_ID);";
                        break;
                    }
                case ExaminationSQLCommands.UpdateCourseHeads:
                    {
                        query = @"UPDATE EXAM_COURSE_WISE_HEADS
                                    SET
                                    COURSE_CODE = ?COURSE_CODE,
                                    PROGRAMME_HEAD_ID = ?PROGRAMME_HEAD_ID,
                                    PROGRAMME_ID = ?PROGRAMME_ID,
                                    BATCH_ID = ?BATCH_ID,
                                    SEMESTER = ?SEMESTER,
                                    ACADEMIC_YEAR = ?ACADEMIC_YEAR , EXAM_SETTING_ID=?EXAM_SETTING_ID
                                    WHERE COURSE_HEAD_ID = ?COURSE_HEAD_ID;";
                        break;
                    }
                case ExaminationSQLCommands.DeleteCourseHeads:
                    {
                        query = @"UPDATE EXAM_COURSE_WISE_HEADS
                                    SET
                                    IS_DELETED=1
                                    WHERE COURSE_HEAD_ID = ?COURSE_HEAD_ID;";
                        break;
                    }
                case ExaminationSQLCommands.DeleteProgrameHeads:
                    {
                        query = @"UPDATE EXAM_PROGRAMME_WISE_HEADS 
                                        SET 
                                            IS_DELETED =1 
                                        WHERE
                                            PROGRAMME_HEAD_ID = ?PROGRAMME_HEAD_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchProgramHeadsList:
                    {
                        query = @"SELECT 
                                    PROGRAMME_HEAD_ID,
                                    AMOUNT,
                                    PH.HEAD_ID,
                                    PH.BATCH_ID,
                                    B.BATCH,
	                                SEMESTER,
                                    ACADEMIC_YEAR,
                                    HEAD,
                                    HEAD_CODE,
                                    ACCOUNT,
                                    PRO.PROGRAMME_ID,
                                  PRO.PROGRAMME_NAME as PROGRAMME
                                FROM
                                    EXAM_PROGRAMME_WISE_HEADS AS PH
                                        LEFT JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = PH.HEAD_ID
                                     INNER JOIN CP_PROGRAMME_?AC_YEAR AS PRO ON PRO.PROGRAMME_ID=PH.PROGRAMME_ID
                                       INNER JOIN SUP_BATCHES AS B ON B.BATCH_ID=PH.BATCH_ID
                                WHERE
                                    PH.IS_DELETED !=1 AND H.IS_DELETED != 1
                                        AND ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case ExaminationSQLCommands.UpdateProgrammeHeads:
                    {
                        query = @"UPDATE EXAM_PROGRAMME_WISE_HEADS
                                    SET
                                    PROGRAMME_ID = ?PROGRAMME_ID,
                                    BATCH_ID = ?BATCH_ID,
                                    AMOUNT = ?AMOUNT,
                                    HEAD_ID = ?HEAD_ID,
                                    SEMESTER = ?SEMESTER,
                                    ACADEMIC_YEAR = ?ACADEMIC_YEAR,
                                    SUBJECT_TYPE=?SUBJECT_TYPE,EXAM_SETTING_ID=?EXAM_SETTING_ID
                                    WHERE PROGRAMME_HEAD_ID = ?PROGRAMME_HEAD_ID;";
                        break;
                    }
                case ExaminationSQLCommands.SaveProgrammeHeads:
                    {
                        query = @"INSERT INTO EXAM_PROGRAMME_WISE_HEADS
                                    (
                                    PROGRAMME_ID,
                                    BATCH_ID,
                                    AMOUNT,
                                    HEAD_ID,
                                    SEMESTER,
                                    ACADEMIC_YEAR,SUBJECT_TYPE,EXAM_SETTING_ID)
                                    VALUES
                                    (
                                    ?PROGRAMME_ID,
                                    ?BATCH_ID,
                                    ?AMOUNT,
                                    ?HEAD_ID,
                                    ?SEMESTER,
                                    ?ACADEMIC_YEAR,?SUBJECT_TYPE,?EXAM_SETTING_ID
                                    );";
                        break;
                    }
                case ExaminationSQLCommands.FetchProgrammeheadsWithAmount:
                    {

                        query = @"SELECT 
                                    H.HEAD,
                                    H.HEAD_CODE,
                                    H.HEAD_ID,
                                    EP.PROGRAMME_ID,
                                    EP.BATCH_ID,
                                    EP.AMOUNT,
                                    EP.PROGRAMME_HEAD_ID,EP.SUBJECT_TYPE
                                FROM
                                    SUP_HEAD AS H
                                        LEFT JOIN
                                    EXAM_PROGRAMME_WISE_HEADS AS EP ON H.HEAD_ID = EP.HEAD_ID
                                        AND EP.PROGRAMME_ID =?PROGRAMME_ID
                                        AND EP.BATCH_ID = ?BATCH_ID
                                        AND EP.IS_DELETED != 1
                                        AND EP.SEMESTER = ?SEMESTER
                                WHERE
                                    H.IS_DELETED != 1 AND H.HEAD_ID IN (?HEAD_ID);";
                        break;
                    }
                case ExaminationSQLCommands.FetchProgramHeads:
                    {
                        query = @"SELECT 
                                    HEAD,
                                    H.HEAD_ID,
                                    IF(PROGRAMME_HEAD_ID IS NOT NULL,
                                        'SELECTED',
                                        '') AS STATUS
                                FROM
                                    SUP_HEAD AS H
                                        LEFT JOIN
                                    EXAM_PROGRAMME_WISE_HEADS AS EP ON H.HEAD_ID = EP.HEAD_ID
                                        AND EP.PROGRAMME_ID = ?PROGRAMME_ID
                                    AND EP.IS_DELETED != 1
                                        AND EP.SEMESTER = (SELECT 
                                            SEMESTER
                                        FROM
                                            ACADEMIC_SEMESTER_?AC_YEAR
                                        WHERE
                                            PROGRAMME = ?PROGRAMME_ID AND BATCH = ?BATCH_ID
                                                AND IS_ACTIVE = 1)
                                WHERE
                                    H.IS_DELETED != 1";

                        break;
                    }
                case ExaminationSQLCommands.FetchQuizOptionByStudentId:
                    {
                        query = @"SELECT 
                                    OPTION_ID,
                                    OPTIONS,
                                    QP.QUESTION_ID
                                FROM
                                    QUIZ_?AC_YEAR AS Q
		                                INNER JOIN
	                                QUIZ_OPTIONS_?AC_YEAR AS QP ON QP.QUESTION_ID=Q.QUESTION_ID
                                WHERE STUDENT_ID=?STUDENT_ID ORDER BY QUESTION_ID;";
                        break;
                    }
                case ExaminationSQLCommands.QuizResult:
                    {
                        query = @"SELECT    
                                    QQ.QUESTION_ID, 
                                    QQ.QUESTION,
                                    Q.SELECTED_ANSWER AS 'SELECTED',
                                    QP.OPTIONS AS 'ANSWER',
                                    OP.OPTIONS AS 'SELECTED_ANSWER' ,
                                    IF(QP.OPTIONS !=OP.OPTIONS,'0','1') AS 'STATUS',
                                    SP.FIRST_NAME,
                                    CCR.COURSE_TITLE,
                                    DATE_FORMAT(Q.DATE,'%d/%m/%Y')AS 'DATE',
                                    (SELECT COUNT(QUESTION) FROM QUIZ_QUESTIONS_?AC_YEAR WHERE  COURSE_ID=?COURSE_ID)AS 'COUNT'
                                FROM
                                    QUIZ_?AC_YEAR AS Q 
		                                INNER JOIN
	                                QUIZ_QUESTIONS_?AC_YEAR AS QQ ON QQ.QUESTION_ID=Q.QUESTION_ID
		                                INNER JOIN
	                                QUIZ_OPTIONS_?AC_YEAR AS QP ON QP.OPTION_ID=QQ.ANSWER
		                                INNER JOIN
	                                QUIZ_OPTIONS_?AC_YEAR AS OP ON OP.OPTION_ID=Q.SELECTED_ANSWER  
		                                INNER JOIN
	                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID=Q.STUDENT_ID
		                                INNER JOIN
	                                CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID=QQ.COURSE_ID
                                WHERE Q.STUDENT_ID=?STUDENT_ID AND QQ.COURSE_ID=?COURSE_ID;";
                        break;
                    }
                case ExaminationSQLCommands.QuizValidate:
                    {
                        query = @"SELECT 
                                      STUDENT_ID
                                    FROM
                                      QUIZ_?AC_YEAR AS Q
                                      INNER JOIN
                                      QUIZ_QUESTIONS_?AC_YEAR AS QQ ON QQ.QUESTION_ID=Q.QUESTION_ID
                                    WHERE
                                      STUDENT_ID = ?STUDENT_ID AND QQ.COURSE_ID=?COURSE_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchOptionIdByOption:
                    {
                        query = @"SELECT 
                                    OPTION_ID, 
                                    OPTIONS, 
                                    QUESTION_ID, 
                                    IS_ACTIVE, 
                                    IS_DELETED
                                FROM
                                    QUIZ_OPTIONS_?AC_YEAR WHERE OPTIONS=?ANSWER AND QUESTION_ID=?QUESTION_ID;";

                        break;
                    }
                case ExaminationSQLCommands.FetchOptionIdByQuestionId:
                    {
                        query = @"SELECT 
                                    OPTION_ID,
                                    OPTIONS
                                FROM
                                    QUIZ_OPTIONS_?AC_YEAR
                                WHERE
                                    QUESTION_ID = ?QUESTION_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchQuizByQuestionId:
                    {
                        query = @"SELECT 
	                                QUESTION_ID,
                                    QUESTION,
                                    CLASS_ID,
                                    COURSE_ID,
                                    STAFF_ID,
									ANSWER
                                FROM
                                    QUIZ_QUESTIONS_?AC_YEAR 
                                WHERE QUESTION_ID=?QUESTION_ID;";
                        break;
                    }
                case ExaminationSQLCommands.ListQuiz:
                    {
                        query = @"SELECT 
	                                QQ.QUESTION_ID,
                                    QQ.QUESTION,
                                    OP.OPTIONS AS 'ANSWER',
                                    CC.CLASS_NAME,
                                    STF.FIRST_NAME
                                FROM
                                    QUIZ_QUESTIONS_?AC_YEAR AS QQ 
		                                INNER JOIN
	                                QUIZ_OPTIONS_?AC_YEAR AS OP ON OP.OPTION_ID=QQ.ANSWER
		                                INNER JOIN
	                                STF_PERSONAL_INFO AS STF ON STF.STAFF_ID=QQ.STAFF_ID
		                                INNER JOIN
	                                CP_CLASSES_?AC_YEAR AS CC ON CC.CLASS_ID=QQ.CLASS_ID
                                WHERE QQ.STAFF_ID=?STAFF_ID AND QQ.IS_DELETED!=1";
                        break;
                    }
                case ExaminationSQLCommands.FetchCourseForQuiz:
                    {
                        query = @"SELECT 
                                    CCR.COURSE_ROOT_ID,
                                    CCR.COURSE_TITLE
                                FROM
                                    STU_CLASS AS SC
		                                INNER JOIN	                                 
								    QUIZ_QUESTIONS_?AC_YEAR AS QQ ON QQ.CLASS_ID=SC.CLASS_ID
                                       INNER JOIN
								    CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID=QQ.COURSE_ID
                                WHERE SC.STUDENT_ID=?STUDENT_ID GROUP BY CCR.COURSE_ROOT_ID;";
                        break;
                    }
                case ExaminationSQLCommands.SaveQuizOption:
                    {
                        query = @"INSERT INTO QUIZ_OPTIONS_?AC_YEAR
                                    (OPTIONS,QUESTION_ID)
                                    VALUES(?OPTIONS,?QUESTION_ID);";
                        break;
                    }
                case ExaminationSQLCommands.UpdateQuizOptions:
                    {
                        query = @"UPDATE QUIZ_OPTIONS_?AC_YEAR
                                SET
                                OPTIONS = ?OPTIONS,
                                QUESTION_ID = ?QUESTION_ID
                                WHERE OPTION_ID = ?OPTION_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchQuizOptionByCourseId:
                    {
                        query = @"SELECT 
                                    OP.OPTION_ID, 
                                    OP.OPTIONS, 
                                    OP.QUESTION_ID
                                FROM
                                    QUIZ_OPTIONS_?AC_YEAR AS OP
                                        INNER JOIN
                                    QUIZ_QUESTIONS_?AC_YEAR AS QQ ON QQ.QUESTION_ID = OP.QUESTION_ID
                                WHERE QQ.COURSE_ID=?COURSE_ID AND CLASS_ID=?CLASS_ID AND QQ.IS_DELETED!=1;";
                        break;
                    }
                case ExaminationSQLCommands.FetchQuizQuestionByCourseId:
                    {
                        query = @"SELECT 
                                    QQ.QUESTION_ID,
                                    QQ.QUESTION,
                                    OP.OPTIONS AS 'ANSWER',
                                    QQ.CLASS_ID,
                                    QQ.COURSE_ID,
                                    QQ.STAFF_ID
                                FROM
                                    QUIZ_QUESTIONS_?AC_YEAR AS QQ
		                                INNER JOIN
	                                QUIZ_OPTIONS_?AC_YEAR AS OP ON OP.OPTION_ID=QQ.ANSWER
                                WHERE QQ.COURSE_ID=?COURSE_ID AND CLASS_ID=?CLASS_ID AND QQ.IS_DELETED!=1;";
                        break;
                    }
                case ExaminationSQLCommands.SaveQuizQuestion:
                    {
                        query = @"INSERT INTO QUIZ_QUESTIONS_?AC_YEAR
                                (QUESTION,CLASS_ID,COURSE_ID,STAFF_ID)
                                VALUES
                                (?QUESTION,?CLASS_ID,?COURSE_ID,?STAFF_ID);";
                        break;
                    }
                case ExaminationSQLCommands.UpdateQuizQuestion:
                    {
                        query = @"UPDATE QUIZ_QUESTIONS_?AC_YEAR
                                    SET                                    
                                    QUESTION = ?QUESTION,
                                    ANSWER = ?ANSWER,
                                    COURSE_ID = ?COURSE_ID,
                                    STAFF_ID = ?STAFF_ID
                                    WHERE QUESTION_ID = ?QUESTION_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchClassByStudentId:
                    {
                        query = @"SELECT 
                                        STUDENT_ID, 
                                        CLASS_ID
                                    FROM
                                        STU_CLASS
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchClassByStaffId:
                    {
                        query = @"SELECT 
                                    CC.CLASS_ID,
                                    CC.CLASS_NAME
                                FROM
                                   CP_CLASS_COURSE_STAFF_?AC_YEAR AS CCS
		                                INNER JOIN
	                                CP_CLASSES_?AC_YEAR AS CC ON CC.CLASS_ID=CCS.CLASS_ID
                                WHERE 
	                                CCS.STAFF_ID=?STAFF_ID AND CCS.IS_DELETED!=1;";
                        break;
                    }
                case ExaminationSQLCommands.DeleteQuiz:
                    {
                        query = @"UPDATE QUIZ_QUESTIONS_?AC_YEAR SET IS_DELETED=1 WHERE QUESTION_ID=?QUESTION_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchCourseInfoByAcademicYear:
                    {
                        query = @"SELECT 
                                        CC.COURSE_CODE, CC.COURSE_TITLE, CC.SEMESTER_ID
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR AS CC
                                    WHERE
                                        CC.COURSE_ROOT_ID=?COURSE_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchCollegeDetails:
                    {
                        query = @"SELECT 
                                    UPPER(COLLEGE_NAME) AS 'COLLEGE_NAME',
                                    UNIVERSITY AS 'UNIVERSITY',
                                    UPPER(GRADE) AS 'GRADE',
                                    CONCAT(PLACE,' - ',PINCODE) AS 'PLACE',
                                    COLLEGE_LOGO
                                FROM
                                    COLLEGE_DETAILS;";
                        break;
                    }
                case ExaminationSQLCommands.ValidateStudentForExamRegistration:
                    {
                        query = @"SELECT 
	                                COUNT(*) > 0 AS 'RESULT',
                                    STUDENT_ID
                                FROM
                                    EXAM_REGISTRATION_?AC_YEAR
                                WHERE
                                    STUDENT_ID = ?STUDENT_ID;";
                        break;
                    }
                case ExaminationSQLCommands.ExamControllerPrint:
                    {
                        query = @"SELECT 
                                   SP.ROLL_NO,
                                   SP.REGISTER_NO,
                                   CCR.COURSE_CODE,
                                   UPPER(CCR.COURSE_TITLE) AS 'COURSE_TITLE',
                                   ER.STATUS,
                                   B.BATCH
                               FROM
                                   EXAM_REGISTRATION_?AC_YEAR ER 
	                                    INNER JOIN
	                                CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID=ER.COURSE_ID
	                                    INNER JOIN 
	                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID=ER.STUDENT_ID
	                                    INNER JOIN
	                                STU_CLASS AS SC ON SC.STUDENT_ID=ER.STUDENT_ID AND SC.ACADEMIC_YEAR=?AC_YEAR
			                            INNER JOIN
		                            SUP_BATCHES AS B ON B.BATCH_ID=SP.BATCH
                               WHERE SC.CLASS_ID IN (?CLASS_ID);";
                        break;
                    }
                case ExaminationSQLCommands.FetchHallTicketList:
                    {
                        query = @"SELECT 
                                    REGISTRATION_ID,
                                    STUDENT_ID,
                                    CC.COURSE_CODE,
                                    CC.COURSE_TITLE,
                                    ER.COURSE_ID,
                                    STATUS,
                                    ER.ACADEMIC_YEAR,
                                    S.SEMESTER_NAME AS 'SEMESTER_ID',
                                    (SELECT M_PASS FROM EXAM_SETTING WHERE IS_ACTIVE=1) AS 'MONTH_YEAR',
                                    CC.SEMESTER_ID AS 'SEMESTER'
                                FROM
                                    EXAM_REGISTRATION_?AC_YEAR AS ER
										INNER JOIN 
									CP_COURSE_ROOT_?AC_YEAR AS CC ON CC.COURSE_ROOT_ID=ER.COURSE_ID
										INNER JOIN
									SUP_SEMESTER AS S ON S.SUP_SEMESTER_ID=CC.SEMESTER_ID
                                WHERE
                                    ER.STUDENT_ID = ?STUDENT_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchHallTicket:
                    {
                        query = @"SELECT 
                                    D.DEPARTMENT,
                                    REGISTER_NO,
                                    FIRST_NAME,
                                    CD.DEGREE,
                                    SS.SEMESTER_NAME,
                                    S.SEMESTER,
                                    DATE_FORMAT(DATE_OF_BIRTH,'%d/%m/%Y')AS 'DATE_OF_BIRTH',
                                    CONCAT(DATE_FORMAT(current_date(),'%M - %Y')) AS 'YEAR'
                                FROM
                                    STU_PERSONAL_INFO SP
                                        INNER JOIN
                                    CP_DEPARTMENT_?AC_YEAR AS D ON D.DEPARTMENT_ID = SP.DEPT_ID
		                                INNER JOIN
	                                CP_PROGRAMME_?AC_YEAR AS P ON P.PROGRAMME_ID=SP.PROGRAM_ID
		                                LEFT JOIN
	                                CP_DEGREE_?AC_YEAR AS CD ON CD.DEGREE_ID=P.DEGREE
		                                INNER JOIN
	                                ACADEMIC_SEMESTER_?AC_YEAR AS S ON S.PROGRAMME=SP.PROGRAM_ID AND S.BATCH=SP.BATCH
		                                INNER JOIN
	                                SUP_SEMESTER AS SS ON SS.SUP_SEMESTER_ID=S.SEMESTER
                                WHERE
                                    SP.STUDENT_ID =?STUDENT_ID AND S.IS_ACTIVE=1;";
                        break;
                    }
                case ExaminationSQLCommands.FetchProgramAndBatchByStudentID:
                    {
                        query = @"SELECT 
                                    PROGRAM_ID AS PROGRAMME,
                                    BATCH
                                FROM
                                    STU_PERSONAL_INFO WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchStudentDetailsForExamRegistration:
                    {
                        query = @"SELECT
	                                SP.STUDENT_ID, 
	                                SP.ROLL_NO,
                                    CONCAT(UPPER(SP.FIRST_NAME))AS 'NAME',
                                    DATE_FORMAT(SP.DATE_OF_BIRTH,'%d/%m/%Y')AS 'DATE_OF_BIRTH',
                                    SP.REGISTER_NO,
                                    D.DEPARTMENT,
                                    IF(SP.STU_MOBILENO IS NULL || SP.STU_MOBILENO ='',
	                                IF(SP.FR_MOBILE IS NULL || SP.FR_MOBILE ='',
	                                IF(SP.MO_MOBILE IS NULL || SP.MO_MOBILE ='','',SP.MO_MOBILE),SP.FR_MOBILE),SP.STU_MOBILENO) AS 'MOBILE_NO',
	                                CONCAT('#',A.P_DOOR_DETAIL,',',A.P_VILLAGE_AREA,'(village) ,',A.P_POST_PLACE_TOWN,'(POST) ,')AS 'ADDRESS1',
	                                CONCAT(A.P_STREET,',', A.P_DISTRICT,'D.T ,',P_PINCODE)AS 'ADDRESS2',
	                                CONCAT(DATE_FORMAT(CURRENT_DATE(),'%M'),'-',DATE_FORMAT(CURRENT_DATE(),'%Y')) AS 'YEAR',
	                                DATE_FORMAT(CURRENT_DATE(),'%d/%m/%Y') AS 'DATE' ,
                                    (SELECT M_EXAM  FROM EXAM_SETTING WHERE IS_ACTIVE=1)AS 'MONTH',
                                    (SELECT EXAM_SETTING_ID  FROM EXAM_SETTING WHERE IS_ACTIVE=1) AS 'EXAM_SETTING_ID'
                                FROM
                                    STU_PERSONAL_INFO SP
		                                INNER JOIN
                                     CP_DEPARTMENT_?AC_YEAR AS D ON D.DEPARTMENT_ID = SP.DEPT_ID
                                         LEFT JOIN
                                     STU_ADDRESS AS A ON A.STUDENT_ID = SP.STUDENT_ID
		                                INNER JOIN
	                                ACADEMIC_CURRICULUM_?AC_YEAR AS AC ON AC.BATCH=SP.BATCH
                                WHERE
                                    SP.STUDENT_ID = ?STUDENT_ID GROUP BY SP.STUDENT_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchExamRegistration:
                    {
                        query = @"SELECT 
                                   SP.STUDENT_ID,
                                   CCR.COURSE_ROOT_ID,
                                   CCR.COURSE_CODE,
                                   UPPER(CCR.COURSE_TITLE) AS 'COURSE_TITLE',
                                   CCR.IS_COMPULSORY,
                                   CCR.SEMESTER_ID,CCR.SUBJECT_TYPE AS 'SUBJECT_TYPE_ID',SS.SUBJECT_TYPE
                                FROM
                                    STU_PERSONAL_INFO AS SP
		                                INNER JOIN
                                    STU_CLASS AS SC on SC.STUDENT_ID=SP.STUDENT_ID AND SC.ACADEMIC_YEAR=?AC_YEAR
		                                INNER JOIN
	                                CP_CLASS_COURSE_?AC_YEAR AS CCC ON CCC.CLASS_ID=SC.CLASS_ID
		                                INNER JOIN
	                                CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID=CCC.COURSE_ID
		                                INNER JOIN
	                                ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.SEMESTER=CCR.SEMESTER_ID AND SEM.PROGRAMME=?PROGRAMME
                            LEFT JOIN SUP_SUBJECT_TYPE AS SS ON SS.SUBJECT_TYPE_ID=CCR.SUBJECT_TYPE AND SS.IS_DELETED!=1
                                WHERE
                                    SP.STUDENT_ID = ?STUDENT_ID AND SEM.IS_ACTIVE=1 AND  CCR.IS_DELETED!=1 AND SP.IS_DELETED!=1 AND CCC.IS_DELETED!=1 GROUP BY COURSE_ROOT_ID
                                    UNION
                                SELECT 
                                    SC.STUDENT_ID,
                                    CCR.COURSE_ROOT_ID,
                                    CCR.COURSE_CODE,
                                    UPPER(CCR.COURSE_TITLE) AS 'COURSE_TITLE',
	                                CCR.IS_COMPULSORY,
                                    CCR.SEMESTER_ID,CCR.SUBJECT_TYPE AS 'SUBJECT_TYPE_ID',SS.SUBJECT_TYPE
                                FROM
                                    STU_COURSE_INFO_?AC_YEAR AS SC
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID=SC.COURSE_ID 
		                                INNER JOIN
	                                ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.SEMESTER=CCR.SEMESTER_ID AND SEM.PROGRAMME=?PROGRAMME
                                    LEFT JOIN SUP_SUBJECT_TYPE AS SS ON SS.SUBJECT_TYPE_ID=CCR.SUBJECT_TYPE AND SS.IS_DELETED!=1
                                WHERE
                                    STUDENT_ID = ?STUDENT_ID AND SEM.IS_ACTIVE=1 AND  CCR.IS_DELETED!=1 AND SC.IS_DELETED!=1 ;";
                        break;
                    }
                case ExaminationSQLCommands.SaveExamDetails:
                    {
                        query = @"INSERT INTO EXAM_REGISTRATION_?AC_YEAR(
			                      STUDENT_ID,
			                      COURSE_ID,
			                      STATUS,ACADEMIC_YEAR,EXAM_SETTING_ID,EXAM_SEMESTER)
			                      VALUES
			                      (?STUDENT_ID,?COURSE_ID,?STATUS,?ACADEMIC_YEAR,?EXAM_SETTING_ID,?EXAM_SEMESTER);";
                        break;
                    }
                case ExaminationSQLCommands.FetchExamRegistrationIfNotCompulsory:
                    {
                        query = @"SELECT 
                                    CCR.COURSE_ROOT_ID AS 'COURSE_ID',
	                                CCR.COURSE_CODE, 
                                    UPPER(CCR.COURSE_TITLE) AS 'COURSE_TITLE'
                                FROM
                                    ACADEMIC_CURRICULUM_?AC_YEAR AS AC 
		                                INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.PROGRAM_ID=AC.PROGRAMME
		                                INNER JOIN 
	                                CP_CLASS_COURSE_?AC_YEAR AS CCC ON CCC.CLASS_ID=SP.CLASS_ID
		                                INNER JOIN
	                                CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID=CCC.COURSE_ID
                                    WHERE AC.COURSE_ID=?COURSE_ROOT_ID AND SP.STUDENT_ID=?STUDENT_ID group by CCR.COURSE_ROOT_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchActiveSemesterWithBatches:
                    {
                        query = @"SELECT 
	                                Y.ACADEMIC_YEAR_ID, 
                                    Y.AC_YEAR,
                                    S.SEMESTER,
                                    S.IS_ACTIVE,
                                    PROGRAMME,
                                    s.BATCH
                                FROM
                                    ACADEMIC_SEMESTER_?AC_YEAR S
		                                INNER JOIN
                                    ACADEMIC_BATCHES AS B ON B.BATCH_ID = S.BATCH
		                                INNER JOIN
                                    ACADEMIC_YEAR AS Y ON Y.ACADEMIC_YEAR_ID = B.ACADEMIC_YEAR_ID
                                WHERE
                                    PROGRAMME = ?PROGRAMME AND IS_ACTIVE = 1 AND S.BATCH=?BATCH;";
                        break;
                    }
                case ExaminationSQLCommands.FetchSemesterResult:
                    {
                        query = @"SELECT 
                                    CC.COURSE_ROOT_ID,
                                    CC.COURSE_CODE,
                                    UPPER(CC.COURSE_TITLE) AS 'COURSE_TITLE',
                                    IF(SM.RESULT !='F',IF(SM.RESULT!='A','PASS','AAA'),'A')AS RESULT,
                                    SM.STUDENT_ID,
                                      SM.SEMESTER AS WRITTEN_SEMESTER,
                                    CC.SEMESTER_ID AS SEMESTER ,CC.SUBJECT_TYPE AS 'SUBJECT_TYPE_ID' , SS.SUBJECT_TYPE
                                FROM
                                    SEMESTER_MARKS_?AC_YEAR SM
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CC ON CC.COURSE_ROOT_ID = SM.COURSE_ID
                                    LEFT JOIN SUP_SUBJECT_TYPE AS SS ON SS.SUBJECT_TYPE_ID = CC.SUBJECT_TYPE AND SS.IS_DELETED!=1
                                        WHERE 
	                                SM.STUDENT_ID =?STUDENT_ID AND SEMESTER in (?SEMESTER);";
                        break;
                    }
                case ExaminationSQLCommands.FetchProgram:
                    {
                        query = @"SELECT 
                                   PROGRAMME_ID, PROGRAMME_NAME
                               FROM
                                  CP_PROGRAMME_?AC_YEAR;";
                        break;
                    }
                case ExaminationSQLCommands.FetchBatch:
                    {
                        query = @"SELECT 
                                    B.BATCH_ID,
                                    B.BATCH
                                FROM
                                    ACADEMIC_CURRICULUM_?AC_YEAR AC 
                                    INNER JOIN SUP_BATCHES AS B ON B.BATCH_ID=AC.BATCH
                                    WHERE AC.PROGRAMME=?PROGRAMME GROUP BY BATCH;";
                        break;
                    }
                case ExaminationSQLCommands.FetchCourseList:
                    {
                        query = @"SELECT 
                                        CS.COURSE_ID,
                                        UPPER(CR.COURSE_TITLE) AS 'COURSE_TITLE',
                                        CR.SEMESTER_ID,
                                        SEM.PROGRAMME,
                                        SEM.SEMESTER,
                                        CLS.CLASS_ID
                                    FROM
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CS
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CS.COURSE_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = CS.CLASS_ID
                                            INNER JOIN
                                      ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.SEMESTER = CR.SEMESTER_ID
                                    WHERE
                                        CS.CLASS_ID = ?CLASS_ID AND CS.STAFF_ID = ?STAFF_ID
                                            AND SEM.IS_ACTIVE = 1 AND CS.IS_DELETED!=1
                                    GROUP BY COURSE_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchCourseByStaffId:
                    {
                        query = @"SELECT 
                                        COURSE_ROOT_ID, UPPER(CCR.COURSE_TITLE) AS 'COURSE_TITLE'
                                    FROM
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CCS
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = CCS.CLASS_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID = CCS.COURSE_ID
                                    WHERE
                                        CCS.STAFF_ID = ?STAFF_ID AND CLS.CLASS_ID = ?CLASS_ID
                                            AND CCR.SEMESTER_ID IN (SELECT 
                                                SEMESTER
                                            FROM
                                                ACADEMIC_SEMESTER_?AC_YEAR
                                            WHERE
                                                IS_ACTIVE = 1
                                                    AND PROGRAMME = CLS.PROGRAMME) AND CCS.IS_DELETED!=1 AND CCR.IS_DELETED!=1;";
                        break;
                    }
                case ExaminationSQLCommands.FetchCIAInfoForRegularByClassAndCourseId:
                    {
                        query = @"SELECT 
                                        CIA.MARK_ID,
                                        CIA.COURSE_ROOT_ID,
                                        SP.STUDENT_ID,
                                        SCLS.CLASS_ID,
                                        CONCAT(IF(SP.FIRST_NAME IS NULL,
                                                    '',
                                                    SP.FIRST_NAME),
                                                ' ',
                                                IF(SP.LAST_NAME IS NULL,
                                                    ' ',
                                                    SP.LAST_NAME)) AS NAME,
                                        SP.REGISTER_NO,
                                        IF(CIA.CIA_C1 IS NULL, '', CIA.CIA_C1) CIA_C1,
                                        IF(CIA.CIA_C2 IS NULL, '', CIA.CIA_C2) CIA_C2,
                                        IF(CIA.CIA_C3 IS NULL, '', CIA.CIA_C3) CIA_C3,
                                        IF(CIA.CIA_C4 IS NULL, '', CIA.CIA_C4) CIA_C4,
                                        IF(CIA.CIA_C5 IS NULL, '', CIA.CIA_C5) CIA_C5,
                                        IF(CIA.CIA_C6 IS NULL, '', CIA.CIA_C6) CIA_C6,
                                        IF(CIA.CIA_C7 IS NULL, '', CIA.CIA_C7) CIA_C7,
                                        IF(CIA.CIA_C8 IS NULL, '', CIA.CIA_C8) CIA_C8,
                                        IF(CIA.CIA_TOTAL IS NULL,
                                            0,
                                            CIA.CIA_TOTAL) CIA_TOTAL
                                    FROM
                                        STU_PERSONAL_INFO AS SP
                                            LEFT JOIN
                                        CIA_MARKS_?AC_YEAR AS CIA ON CIA.STUDENT_ID = SP.STUDENT_ID
                                           AND
                                           CIA.COURSE_ROOT_ID=?COURSE_ID
                                            INNER JOIN
                                        STU_CLASS AS SCLS ON SCLS.STUDENT_ID = SP.STUDENT_ID                                         
                                    WHERE
                                        SCLS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND SCLS.CLASS_ID = ?S_CLASS_ID ORDER BY REGISTER_NO;";

                        break;
                    }
                case ExaminationSQLCommands.FetchCIAInfoForSelectedByClassAndCourseId:
                    {
                        query = @"SELECT 
                                        CIA.MARK_ID,
                                        CIA.COURSE_ROOT_ID,
                                        SP.STUDENT_ID,
                                        SCLS.CLASS_ID,
                                        CONCAT(IF(SP.FIRST_NAME IS NULL,
                                                    '',
                                                    SP.FIRST_NAME),
                                                ' ',
                                                IF(SP.LAST_NAME IS NULL,
                                                    ' ',
                                                    SP.LAST_NAME)) AS NAME,
                                        SP.REGISTER_NO,
                                        IF(CIA.CIA_C1 IS NULL, 0, CIA.CIA_C1) CIA_C1,
                                        IF(CIA.CIA_C2 IS NULL, 0, CIA.CIA_C2) CIA_C2,
                                        IF(CIA.CIA_C3 IS NULL, 0, CIA.CIA_C3) CIA_C3,
                                        IF(CIA.CIA_C4 IS NULL, 0, CIA.CIA_C4) CIA_C4,
                                        IF(CIA.CIA_C5 IS NULL, 0, CIA.CIA_C5) CIA_C5,
                                        IF(CIA.CIA_C6 IS NULL, 0, CIA.CIA_C6) CIA_C6,
                                        IF(CIA.CIA_C7 IS NULL, 0, CIA.CIA_C7) CIA_C7,
                                        IF(CIA.CIA_C8 IS NULL, 0, CIA.CIA_C8) CIA_C8,
                                        IF(CIA.CIA_TOTAL IS NULL,
                                            0,
                                            CIA.CIA_TOTAL) CIA_TOTAL
                                    FROM
                                        STU_PERSONAL_INFO AS SP
                                            INNER JOIN
                                        CIA_MARKS_?AC_YEAR AS CIA ON CIA.STUDENT_ID = SP.STUDENT_ID
                                           AND
                                           CIA.COURSE_ROOT_ID=?COURSE_ID
                                            INNER JOIN
                                        STU_COURSE_INFO_?AC_YEAR AS SCLS ON SCLS.STUDENT_ID = SP.STUDENT_ID
                                    WHERE
                                        SCLS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND SCLS.S_CLASS_ID =?S_CLASS_ID GROUP BY SCLS.STUDENT_ID;";

                        break;
                    }
                case ExaminationSQLCommands.FetchCIATotal:
                    {
                        query = @"


SELECT 
                                             SUM(IFNULL(CUMULATIVE,0)) TOTAL,
                                    STUDENT_ID,
                                    ADMISSION_NO,
                                    REGISTER_NO,
                                    NAME
                                FROM
                                    (SELECT 
                                        ((SUM(IF(COURSE_GROUP_MARK = - 1, 0, COURSE_GROUP_MARK)) / SUM(MAX_MARK)) * (SELECT 
                                                    GROUP_MARK
                                                FROM
                                                    CP_COURSE_TYPE_GROUP_?AC_YEAR AS CTG
                                                WHERE
                                                    CTG.GROUP_ID = CGC.CIA_GROUP_ID
                                                LIMIT 1)) AS CUMULATIVE,
                                            SP.STUDENT_ID,
                                            SP.ADMISSION_NO,
                                            SP.REGISTER_NO,
                                            CONCAT(IF(SP.FIRST_NAME IS NULL, '', SP.FIRST_NAME), ' ', IF(SP.LAST_NAME IS NULL, '', SP.LAST_NAME)) AS 'NAME'
                                    FROM
                                        STU_CLASS AS SC
                                    INNER JOIN STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                        AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                        AND SP.IS_LEFT != 1
                                        AND SP.IS_DELETED != 1
                                    LEFT JOIN CIA_GROUP_MARKS_?AC_YEAR AS CGM ON CGM.STUDENT_ID = SC.STUDENT_ID
                                        AND CGM.COURSE_ID = ?COURSE_ID
                                    LEFT JOIN CIA_GROUP_COMPONENT AS CGC ON CGC.CIA_GROUP_COMPONENT_ID = CGM.COURSE_GROUP_ID
                                        AND CGC.IS_DELETED != 1
                                    LEFT JOIN CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID = CGM.COURSE_ID
                                        AND CCR.IS_DELETED != 1
                                    INNER JOIN CP_CLASS_COURSE_?AC_YEAR AS CC ON CC.CLASS_ID=SC.CLASS_ID AND CC.COURSE_ID=?COURSE_ID AND CC.IS_DELETED!=1
                                    WHERE
                                        SC.CLASS_ID = ?CLASS_ID
                                            AND CGM.COURSE_ID = ?COURSE_ID
                                            AND SC.IS_DELETED != 1 AND SP.IS_LEFT!=1
                                    GROUP BY SP.STUDENT_ID , CGC.CIA_GROUP_ID
                                    ) AS T
                                GROUP BY STUDENT_ID 
                                UNION SELECT 
                                         SUM(IFNULL(CUMULATIVE,0)) TOTAL,
                                    STUDENT_ID,
                                    ADMISSION_NO,
                                    REGISTER_NO,
                                    NAME
                                FROM
                                    (SELECT 
                                        ((SUM(IF(COURSE_GROUP_MARK = - 1, 0, COURSE_GROUP_MARK)) / SUM(MAX_MARK)) * (SELECT 
                                                    GROUP_MARK
                                                FROM
                                                    CP_COURSE_TYPE_GROUP_?AC_YEAR AS CTG
                                                WHERE
                                                    CTG.GROUP_ID = CGC.CIA_GROUP_ID
                                                LIMIT 1)) AS CUMULATIVE,
                                            SP.STUDENT_ID,
                                            SP.ADMISSION_NO,
                                            SP.REGISTER_NO,
                                            CONCAT(IF(SP.FIRST_NAME IS NULL, '', SP.FIRST_NAME), ' ', IF(SP.LAST_NAME IS NULL, '', SP.LAST_NAME)) AS 'NAME'
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR AS SC
                                    INNER JOIN STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                        AND SP.IS_LEFT != 1
                                        AND SP.IS_DELETED != 1
                                    LEFT JOIN CIA_GROUP_MARKS_?AC_YEAR AS CGM ON CGM.STUDENT_ID = SC.STUDENT_ID
                                        AND SC.COURSE_ID = CGM.COURSE_ID
                                    LEFT JOIN CIA_GROUP_COMPONENT AS CGC ON CGC.CIA_GROUP_COMPONENT_ID = CGM.COURSE_GROUP_ID
                                        AND CGC.IS_DELETED != 1
                                    LEFT JOIN CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID = CGM.COURSE_ID
                                        AND CCR.IS_DELETED != 1
                                    WHERE
                                        SC.S_CLASS_ID = ?CLASS_ID
                                            AND SC.COURSE_ID = ?COURSE_ID
                                            AND SC.IS_DELETED != 1 AND SP.IS_LEFT!=1
                                    GROUP BY SP.STUDENT_ID , CGC.CIA_GROUP_ID
                                    ) AS T 
                                GROUP BY STUDENT_ID ORDER BY  REGISTER_NO ";
                        break;
                    }

                case ExaminationSQLCommands.FetchCIA_Empty_Marks:
                    {
                        query = @"SELECT 
                                '' AS CIA_GROUP_MARK_ID,
                                SP.STUDENT_ID,
                                CONCAT(IF(SP.FIRST_NAME IS NULL,
                                            '',
                                            SP.FIRST_NAME),
                                        ' ',
                                        IF(SP.LAST_NAME IS NULL,
                                            '',
                                            SP.LAST_NAME)) AS 'NAME',
                                SP.REGISTER_NO,
                                CCR.COURSE_ROOT_ID,
                                '' AS COURSE_GROUP_MARK
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
                                '' AS CIA_GROUP_MARK_ID,
                                SP.STUDENT_ID,
                                CONCAT(IF(SP.FIRST_NAME IS NULL,
                                            '',
                                            SP.FIRST_NAME),
                                        ' ',
                                        IF(SP.LAST_NAME IS NULL,
                                            '',
                                            SP.LAST_NAME)) AS 'NAME',
                                SP.REGISTER_NO,
                                CCR.COURSE_ROOT_ID,
                                '' AS COURSE_GROUP_MARK
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
                case ExaminationSQLCommands.FetchCIA_Marks:
                    {
                        query = @"SELECT 
                                CGM.CIA_GROUP_MARK_ID,
                                SP.STUDENT_ID,
                                CONCAT(IF(SP.FIRST_NAME IS NULL,
                                            '',
                                            SP.FIRST_NAME),
                                        ' ',
                                        IF(SP.LAST_NAME IS NULL,
                                            '',
                                            SP.LAST_NAME)) AS 'NAME',
                                SP.REGISTER_NO,
                                SP.ADMISSION_NO,
                                CCR.COURSE_ROOT_ID,
                                CG.MAX_MARK,
                                CG.ORDER_ID,
                                CG.CIA_GROUP_ID,
                                COURSE_GROUP_ID,
                                CG.SUP_CIA_COMPONENT_ID AS 'COMPONENT_ID',
                                SCC.COMPONENT,
                                IF(CGM.COURSE_GROUP_MARK = 0,
                                    '',
                                    CGM.COURSE_GROUP_MARK) AS COURSE_GROUP_MARK,
                                CGM.SEMESTER_ID
                            FROM
                                STU_CLASS AS SC
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                    AND SP.IS_LEFT != 1
                                    AND SP.IS_DELETED != 1
                                    LEFT JOIN
                                CIA_GROUP_MARKS_?AC_YEAR AS CGM ON CGM.STUDENT_ID = SC.STUDENT_ID AND CGM.COURSE_ID = ?COURSE_ID
                                    LEFT JOIN
                                CIA_GROUP_COMPONENT AS CG ON CG.CIA_GROUP_COMPONENT_ID = CGM.COURSE_GROUP_ID
                                    AND CG.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_CIA_COMPONENTS AS SCC ON SCC.COMPONENT_ID = CG.SUP_CIA_COMPONENT_ID
                                    AND SCC.IS_DELETED != 1
                                    LEFT JOIN
                                CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID = CGM.COURSE_ID
                                    AND CCR.IS_DELETED != 1
                    INNER JOIN CP_CLASS_COURSE_?AC_YEAR AS CC ON CC.CLASS_ID = SC.CLASS_ID AND CC.COURSE_ID=?COURSE_ID
                            WHERE
                                SC.CLASS_ID = ?CLASS_ID
                                    
                                    AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                    AND SC.IS_DELETED != 1 
                            UNION SELECT 
                                CGM.CIA_GROUP_MARK_ID,
                                SP.STUDENT_ID,
                                CONCAT(IF(SP.FIRST_NAME IS NULL,
                                            '',
                                            SP.FIRST_NAME),
                                        ' ',
                                        IF(SP.LAST_NAME IS NULL,
                                            '',
                                            SP.LAST_NAME)) AS 'NAME',
                                SP.REGISTER_NO,
                                SP.ADMISSION_NO,
                                CCR.COURSE_ROOT_ID,
                                CGC.MAX_MARK,
                                CGC.ORDER_ID,
                                CGC.CIA_GROUP_ID,
                                COURSE_GROUP_ID,
                                CGC.SUP_CIA_COMPONENT_ID AS 'COMPONENT_ID',
                                SCC.COMPONENT,
                                IF(CGM.COURSE_GROUP_MARK = 0,
                                    '',
                                    CGM.COURSE_GROUP_MARK) AS COURSE_GROUP_MARK,
                                CGM.SEMESTER_ID
                            FROM
                                STU_COURSE_INFO_?AC_YEAR AS SC
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                    AND SP.IS_LEFT != 1
                                    AND SP.IS_DELETED != 1
                                    LEFT JOIN
                                CIA_GROUP_MARKS_?AC_YEAR AS CGM ON CGM.STUDENT_ID = SC.STUDENT_ID AND CGM.COURSE_ID = ?COURSE_ID 
                                    LEFT JOIN
                                CIA_GROUP_COMPONENT AS CGC ON CGC.CIA_GROUP_COMPONENT_ID = CGM.COURSE_GROUP_ID
                                    AND CGC.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_CIA_COMPONENTS AS SCC ON SCC.COMPONENT_ID = CGC.SUP_CIA_COMPONENT_ID
                                    AND SCC.IS_DELETED != 1
                                    LEFT JOIN
                                CP_COURSE_ROOT_?AC_YEAR AS CCR ON CCR.COURSE_ROOT_ID = CGM.COURSE_ID
                                    AND CCR.IS_DELETED != 1
                            WHERE
                                SC.S_CLASS_ID = ?CLASS_ID and SC.COURSE_ID=?COURSE_ID
                                    AND SC.IS_DELETED != 1
                            ORDER BY ORDER_ID , REGISTER_NO;";
                        break;
                    }
                case ExaminationSQLCommands.FetchCourseInfoByClassAndCourseId:
                    {
                        query = @"SELECT 
                                    CR.COURSE_ROOT_ID,
                                    CONCAT(IF(UGC.UGC_COURSE_TYPE IS NULL,
                                                ' - ',
                                                UPPER(UGC.UGC_COURSE_TYPE)),
                                            '(',
                                            UPPER(CR.COURSE_TITLE),
                                            ')') AS 'COURSE_TITLE',
                                    CR.COURSE_CODE,
                                    CT.COURSE_TYPE_ID,
                                    CT.COURSE_TYPE,
                                    CLS.CLASS_NAME,
                                    CR.SEMESTER_ID,
                                    CLS.CLASS_ID
                                FROM
                                    CP_COURSE_ROOT_?AC_YEAR AS CR
                                        JOIN
                                    CP_CLASSES_?AC_YEAR AS CLS ON CR.COURSE_ROOT_ID = ?COURSE_ID
                                        AND CLS.CLASS_ID = ?CLASS_ID
                                        INNER JOIN
                                    CP_COURSE_TYPE_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID = CR.COURSE_TYPE
                                        LEFT JOIN
                                    CP_UGC_COURSE_TYPE AS UGC ON UGC.UGC_COURSE_TYPE_ID = CR.UGC_COURSE_TYPE
                                WHERE
                                    CR.IS_DELETED != 1
                                        AND CLS.IS_DELETED != 1 AND CT.IS_DELETED!=1";
                        break;
                    }
                case ExaminationSQLCommands.FetchCourseComponentByCourseTypeId:
                    {
                        query = @"SELECT 
                                    GC.CIA_GROUP_COMPONENT_ID,
                                    GC.MAX_MARK,
                                    SCC.COMPONENT,
                                    CG.COURSE_TYPE_ID,
                                    CG.GROUP_MARK,
                                    GC.CIA_GROUP_ID,
                                    GC.ORDER_ID
                                FROM
                                    CIA_GROUP_COMPONENT AS GC
                                        INNER JOIN
                                    CP_COURSE_TYPE_GROUP_?AC_YEAR CG ON CG.GROUP_ID = GC.CIA_GROUP_ID
                                        INNER JOIN
                                    SUP_CIA_COMPONENTS AS SCC ON SCC.COMPONENT_ID = GC.SUP_CIA_COMPONENT_ID
                                WHERE
                                    CG.COURSE_TYPE_ID = ?COURSE_TYPE_ID ORDER BY GC.ORDER_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchClassList:
                    {

                        query = @"SELECT 
                                        CS.CLASS_ID, CLS.CLASS_NAME
                                    FROM
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CS
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = CS.CLASS_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS RT ON RT.COURSE_ROOT_ID = CS.COURSE_ID
                                            INNER JOIN
                                        ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.SEMESTER = RT.SEMESTER_ID
                                    WHERE
                                        CS.STAFF_ID =?STAFF_ID
                                            AND SEM.SEMESTER = RT.SEMESTER_ID
                                            AND SEM.IS_ACTIVE = 1 AND CS.IS_DELETED!=1
                                    GROUP BY CS.CLASS_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchCourseWiseCIAMarks:
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
                                                        SP.LAST_NAME)) AS 'NAME',
                                            round(CIA.CIA_TOTAL) AS  CIA_TOTAL
    
                                        FROM
                                            CIA_MARKS_?AC_YEAR AS CIA
                                                INNER JOIN
                                            STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CIA.STUDENT_ID  
                                            WHERE CIA.CLASS_ID = ?CLASS_ID AND CIA.COURSE_ROOT_ID=?COURSE_ROOT_ID ;";


                        break;
                    }
                case ExaminationSQLCommands.FetchCourseWiseCIAMarksForStudents:
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
                                                    SP.LAST_NAME)) AS 'NAME',
                                        CIA.CIA_C1,
                                        CIA.CIA_C2,
                                        CIA.CIA_C3,
                                        CIA.CIA_C4,
                                        CIA.CIA_C5,
                                        CIA.CIA_C6,
                                        CIA.CIA_C7,
                                        CIA.CIA_C8,
                                        round(CIA.CIA_TOTAL) AS  CIA_TOTAL
                                    FROM
                                        CIA_MARKS_?AC_YEAR AS CIA
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CIA.STUDENT_ID
                                    WHERE
                                        CIA.CLASS_ID = ?CLASS_ID
                                            AND CIA.COURSE_ROOT_ID = ?COURSE_ROOT_ID;";

                        break;
                    }
                case ExaminationSQLCommands.FetchCourseAndClassInfoByClassAndCourseId:
                    {

                        query = @"SELECT 
                                    DP.DEPARTMENT,
                                    CLS.CLASS_NAME,
                                    CR.COURSE_CODE,
                                    CR.SEMESTER_ID,
                                    UPPER(CR.COURSE_TITLE) AS 'COURSE_TITLE',
                                    CT.INTERNAL,
                                    GROUP_CONCAT(CONCAT(IF(SP.FIRST_NAME IS NULL,
                                                    '',
                                                    SP.FIRST_NAME),
                                                ' ',
                                                IF(SP.LAST_NAME IS NULL,
                                                    '',
                                                    SP.LAST_NAME))) AS STAFF_NAME,
                                    SP.STAFF_ID,
                                    CR.COURSE_TYPE
                                FROM
                                    CP_CLASS_COURSE_STAFF_?AC_YEAR AS CC
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CC.COURSE_ID
                                        INNER JOIN
                                    CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = CC.CLASS_ID
                                        INNER JOIN
                                    CP_DEPARTMENT_?AC_YEAR AS DP ON CR.DEPARTMENT = DP.DEPARTMENT_ID
                                        INNER JOIN
                                    STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = CC.STAFF_ID
                                        INNER JOIN
                                    cp_course_type_?AC_YEAR AS CT ON CT.COURSE_TYPE_ID = CR.COURSE_TYPE
    
                                                                WHERE
                                                                    CC.COURSE_ID =?COURSE_ID  AND CC.CLASS_ID =?CLASS_ID; ";
                        break;
                    }
                case ExaminationSQLCommands.FetchSclassId:
                    {

                        query = @"SELECT
                                        STUDENT_ID
                                    FROM
                                        STU_COURSE_INFO_?AC_YEAR
                                    WHERE
                                        COURSE_ID = ?COURSE_ID AND CLASS_ID = ?S_CLASS_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchExamRegistrationList:
                    {
                        query = @"SELECT 
                                    EXR.REGISTRATION_ID,
                                    SP.STUDENT_ID,
                                    SP.FIRST_NAME,
                                    EXR.COURSE_ID,
                                    EXR.STATUS,
                                    CONCAT(IFNULL(CR.COURSE_CODE, ''),
                                            '(',
                                            IFNULL(UPPER(CR.COURSE_TITLE), ''),
                                            ')') AS COURSE_TITLE
                                FROM
                                    EXAM_REGISTRATION_?AC_YEAR AS EXR
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = EXR.STUDENT_ID
                                        AND SP.IS_DELETED != 1
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = EXR.COURSE_ID;";
                        break;
                    }
                case ExaminationSQLCommands.ListExamRegisteredStudentCount:
                    {
                        query = @"SELECT 
                                        CR.COURSE_CODE, CR.COURSE_TITLE, COUNT(ER.STUDENT_ID) AS TOTAL_COUNT,ES.EXAM_NAME
                                    FROM
                                        CMS.EXAM_REGISTRATION_?AC_YEAR AS ER
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = ER.COURSE_ID
                                            INNER JOIN
                                        EXAM_SETTING AS ES ON ES.EXAM_SETTING_ID = ER.EXAM_SETTING_ID
                                   WHERE ER.EXAM_SETTING_ID=?EXAM_SETTING_ID
                                    GROUP BY ER.COURSE_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchArrearExamRegisteredStudent:
                    {
                        query = @"SELECT 
                                        CLS.CLASS_NAME,
                                        SP.ROLL_NO,
                                        SP.REGISTER_NO,
                                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                        CR.COURSE_CODE,
                                        CR.COURSE_TITLE
                                    FROM
                                        EXAM_REGISTRATION_?AC_YEAR AS ER
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.STUDENT_ID = ER.STUDENT_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = ER.COURSE_ID
                                            INNER JOIN
                                        EXAM_SETTING AS ES ON ES.EXAM_SETTING_ID = ER.EXAM_SETTING_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = SC.CLASS_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                    WHERE
                                        CLS.CLASS_ID IN (?CLASS_ID)
                                            AND ER.STATUS = 'A' AND ER.EXAM_SETTING_ID=?EXAM_SETTING_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchRegularExamRegisteredStudent:
                    {
                        query = @"SELECT 
                                        CLS.CLASS_NAME,
                                        SP.ROLL_NO,
                                        SP.REGISTER_NO,
                                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                        CR.COURSE_CODE,
                                        CR.COURSE_TITLE
                                    FROM
                                        EXAM_REGISTRATION_?AC_YEAR AS ER
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.STUDENT_ID = ER.STUDENT_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = ER.COURSE_ID
                                            INNER JOIN
                                        EXAM_SETTING AS ES ON ES.EXAM_SETTING_ID = ER.EXAM_SETTING_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = SC.CLASS_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                    WHERE
                                        CLS.CLASS_ID IN (?CLASS_ID)
                                            AND ER.STATUS = 'R' AND ER.EXAM_SETTING_ID=?EXAM_SETTING_ID;";
                        break;
                    }
                case ExaminationSQLCommands.FetchFeeTransaction:
                    {
                        query = @"";
                        break;
                    }
            }
            return query;
        }
    }

}
