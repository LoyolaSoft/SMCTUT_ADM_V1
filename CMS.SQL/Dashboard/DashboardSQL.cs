using CMS.DAO;

namespace CMS.SQL.Dashboard
{
    public static class DashboardSQL
    {
        public static string GetDashboardSQL(DashboardSQLCommands sEnumCommand)
        {
            string query = "";

            switch (sEnumCommand)
            {
                case DashboardSQLCommands.FetchStudentInfoByUserId:
                    {
                        //need to remove academic year which is hardcode here...

                        query = @"SELECT 
                                        ADMISSION_NO,
                                        ADMITTED_CLASS,
                                        DEPT_ID,
                                        SHIFT_ID,
                                        FIRST_NAME,
                                        CLS.CLASS_NAME,
                                        SCLS.STUDENT_ID,
                                        SP.PHOTO,
                                        SCLS.CLASS_ID    
                                    FROM
                                        STU_PERSONAL_INFO SP
                                            INNER JOIN
                                        STU_CLASS SCLS ON SCLS.STUDENT_ID = SP.STUDENT_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR CLS ON CLS.CLASS_ID = SCLS.CLASS_ID
                                    WHERE
                                        SP.STUDENT_ID =?STUDENT_ID
                                            AND SCLS.ACADEMIC_YEAR =?ACADEMIC_YEAR;";
                        break;
                    }
                case DashboardSQLCommands.FetchStaffInfoByUserId:
                    {

                        break;
                    }

                case DashboardSQLCommands.GetFeedbackCountByStudent:
                    {
                        query = @"SELECT  COUNT(distinct ASSESSOR) AS NoOfStudentsFeedbackPosted FROM fbevaluation?AC_YEAR";
                        break;
                    }
                case DashboardSQLCommands.GetReceivedAppCount:
                    {
                        query = @"SELECT  COUNT(distinct RECEIVE_ID) AS NoOfApplicationReceived FROM adm_receive_application;";
                        break;
                    }
                case DashboardSQLCommands.GetIssuedAppCount:
                    {
                        query = @"SELECT  COUNT(distinct ISSUED_ID) AS NoOfApplicationIssued FROM adm_issued_applications;";
                        break;
                    }
                case DashboardSQLCommands.GetSelectedAppCount:
                    {
                        query = @"SELECT  COUNT(distinct ISSUED_ID) AS NoOfApplicationSelected FROM adm_issued_applications where STATUS=3;";
                        break;
                    }
                case DashboardSQLCommands.GetAdmittedAppCount:
                    {
                        query = @"SELECT  COUNT(distinct ISSUED_ID) AS NoOfApplicationAdmitted FROM adm_issued_applications where STATUS=5;";
                        break;
                    }
                case DashboardSQLCommands.GetVerifiedAppCount:
                    {
                        query = @"SELECT  COUNT(distinct ISSUED_ID) AS NoOfApplicationAdmitted FROM adm_issued_applications where STATUS=4;";
                        break;
                    }
                case DashboardSQLCommands.GetTotalNoOfStudents:
                    {
                        query = @"SELECT  COUNT(student_id) AS TotalStudents FROM cms.stu_personal_info;";
                        break;
                    }
                case DashboardSQLCommands.FetchAssignmentInfo:
                    {
                        query = @"SELECT
                                    ASS.ASSIGNMENT_ID, 
                                    CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                            '(',
                                            IFNULL(SP.STAFF_CODE, ''),
                                            ')') AS STAFF_NAME,
                                    CONCAT(IFNULL(CR.COURSE_TITLE, ''),
                                            '(',
                                            IFNULL(CR.COURSE_CODE, ''),
                                            ')') AS COURSE_CODE,
                                    DATE_FORMAT(ASS.SUBMISSION_DATE, '%d/%m/%Y') AS SUBMISSION_DATE,
                                    ASS.DESCRIPTION,
                                    UA.PHOTO,
                                    ASS.STAFF_ID,
                                    ASS.COURSE_ID,
                                    ASS.ASSIGNMENT_TITLE,
                                    ASS.IS_FILE_UPLOAD,
                                    ASS.CLASS_ID
                                FROM
                                    ASSIGNMENT_?AC_YEAR AS ASS
                                        INNER JOIN
                                    CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ASS.CLASS_ID
                                        AND CLS.IS_DELETED != 1
                                        INNER JOIN
                                    STU_CLASS AS SC ON SC.CLASS_ID = CLS.CLASS_ID
                                        AND SC.STUDENT_ID = ?STUDENT_ID
                                        AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                        AND SC.IS_DELETED != 1
                                        INNER JOIN
                                    STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = ASS.STAFF_ID
                                        AND SP.IS_DELETED != 1
                                        AND SP.IS_LEFT != 1
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = ASS.COURSE_ID
                                        AND CR.IS_DELETED != 1
                                        INNER JOIN
                                    CP_CLASS_COURSE_?AC_YEAR AS CC ON CC.COURSE_ID = CR.COURSE_ROOT_ID
                                        AND CC.CLASS_ID = ASS.CLASS_ID
                                        AND CC.IS_DELETED != 1
                                        INNER JOIN
                                    USER_ACCOUNT AS UA ON UA.USERNAME = SP.STAFF_CODE
                                WHERE
                                    SC.CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case DashboardSQLCommands.FetchStaffInfoByStudentId:
                    {
                        query = @"SELECT
                                    ASS.STAFF_ID,
                                    UA.PHOTO,
                                    ASS.COURSE_ID
                                FROM
                                    ASSIGNMENT_?AC_YEAR AS ASS
                                        INNER JOIN
                                    CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ASS.CLASS_ID
                                        AND CLS.IS_DELETED != 1
                                        INNER JOIN
                                    STU_CLASS AS SC ON SC.CLASS_ID = CLS.CLASS_ID
                                        AND SC.STUDENT_ID = ?STUDENT_ID
                                        AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                        AND SC.IS_DELETED != 1
                                        INNER JOIN
                                    STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = ASS.STAFF_ID
                                        AND SP.IS_DELETED != 1
                                        AND SP.IS_LEFT != 1
                                        INNER JOIN
                                    CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = ASS.COURSE_ID
                                        AND CR.IS_DELETED != 1
                                        INNER JOIN
                                    CP_CLASS_COURSE_?AC_YEAR AS CC ON CC.COURSE_ID = CR.COURSE_ROOT_ID
                                        AND CC.CLASS_ID = ASS.CLASS_ID
                                        AND CC.IS_DELETED != 1
                                        INNER JOIN
                                    USER_ACCOUNT AS UA ON UA.USERNAME = SP.STAFF_CODE
                                WHERE
                                    SC.CLASS_ID = ?CLASS_ID GROUP BY ASS.COURSE_ID,ASS.STAFF_ID;";
                        break;
                    }
                case DashboardSQLCommands.FetchHostelApplied:
                    {
                        query = @"SELECT 
                                      RECEIVE_ID, HOSTEL_FACILITY
                                  FROM
                                      adm_receive_application
                                  WHERE
                                      RECEIVE_ID = ?RECEIVE_ID;";
                        break;
                    }
                case DashboardSQLCommands.Savehostelinfo:
                    {
                        query = @"UPDATE adm_receive_application SET  HOSTEL_FACILITY=?HOSTEL_FACILITY WHERE RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case DashboardSQLCommands.FeeHostelApplicationfee:
                    {
                        query = @"SELECT 
                                       FF.FEE_FREQUENCY_FEE_MODE_ID AS FREQUENCY_ID,
                                       FF.ACADEMIC_YEAR,
                                       FM.FEE_MAIN_HEAD_ID,
                                       FS.FEE_STRUCTURE_ID,
                                       FM.FEE_ROOT_ID,
                                       FS.AMOUNT,
                                       FS.BANK_ACCOUNT_ID,
                                       FM.HEAD_ID
                                   FROM
                                       FEE_FREQUENCY_FEE_MODE AS FF
                                           INNER JOIN
                                       FEE_MAIN_HEADS AS FM ON FM.FREQUENCY_ID = FF.FEE_FREQUENCY_FEE_MODE_ID
                                           INNER JOIN
                                       FEE_STRUCTURE_FOR_HOSTEL_AND_MESS AS FS ON FS.FEE_MAIN_HEAD_ID = FM.FEE_MAIN_HEAD_ID
                                   WHERE
                                       FF.FEE_MODE = ?FREQUENCY_TYPE_ID AND FS.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case DashboardSQLCommands.SaveHostelApplicationfee:
                    {
                        query = @"INSERT INTO 
                                FEE_STUDENT_ACCOUNT
                                (STUDENT_ID,ACADEMIC_YEAR,FREQUENCY_ID,HEAD,
                                CREDIT,FEE_MAIN_HEAD_ID,FEE_STRUCTURE_ID,FEE_ROOT_ID) 
                                VALUES(?STUDENT_ID,?ACADEMIC_YEAR,?FREQUENCY_ID,?HEAD_ID,?AMOUNT,?FEE_MAIN_HEAD_ID,?FEE_STRUCTURE_ID,?FEE_ROOT_ID);";
                        break;
                    }
                case DashboardSQLCommands.FetchFeeTransaction:
                    {
                        query = @"SELECT TRANSACTION_ID FROM FEE_TRANSACTION WHERE STUDENT_ID=?RECEIVE_ID AND FREQUENCY=?FREQUENCY_ID AND IS_DELETED!=1;";
                        break;
                    }
            }
            return query;
        }
    }


}
