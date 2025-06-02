using CMS.DAO;

namespace CMS.SQL.TaskManagement
{
    public class TaskManagementSQL
    {
        public static string GetTaskManagementSQL(TaskManagementSQLCommands sEnumCommand)
        {
            string query = string.Empty;
            switch (sEnumCommand)
            {
                case TaskManagementSQLCommands.FetchAssignmentList:
                    {
                        query = @"SELECT 
                                        ASS.ASSIGNMENT_ID,
                                        ASS.ASSIGNMENT_TITLE,
                                        ASS.DESCRIPTION,
                                        ASS.ACADEMIC_YEAR,
                                        DATE_FORMAT(ASS.DATE_FROM, '%d/%m/%Y') AS DATE_FROM,
                                        DATE_FORMAT(ASS.SUBMISSION_DATE, '%d/%m/%Y') AS SUBMISSION_DATE,
                                        IS_FILE_UPLOAD
                                    FROM
                                        ASSIGNMENT_?AC_YEAR AS ASS
                                            INNER JOIN
                                        STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = ASS.STAFF_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = ASS.CLASS_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = ASS.COURSE_ID
                                    WHERE
                                        ASS.IS_DELETED != 1
                                            AND SP.IS_DELETED != 1
                                            AND CR.IS_DELETED != 1
                                            AND ASS.ACADEMIC_YEAR = ?AC_YEAR AND ASS.CLASS_ID=?CLASS_ID AND ASS.STAFF_ID=?STAFF_ID AND ASS.COURSE_ID=?COURSE_ID;";
                        break;
                    }
                case TaskManagementSQLCommands.SaveAssignment:
                    {
                        query = @"INSERT INTO ASSIGNMENT_?AC_YEAR
                                            (
                                            STAFF_ID,
                                            ACADEMIC_YEAR,
                                            CLASS_ID,
                                            COURSE_ID,
                                            ASSIGNMENT_TITLE,
                                            DATE_FROM,
                                            SUBMISSION_DATE,
                                            DESCRIPTION,
                                            IS_ACTIVE,
                                            IS_FILE_UPLOAD)
                                            VALUES
                                            (
                                            ?STAFF_ID,
                                            ?AC_YEAR,
                                            ?CLASS_ID,
                                            ?COURSE_ID,
                                            ?ASSIGNMENT_TITLE,
                                            ?DATE_FROM,
                                            ?SUBMISSION_DATE,
                                            ?DESCRIPTION,
                                            ?IS_ACTIVE,
                                            ?IS_FILE_UPLOAD);";
                        break;
                    }
                case TaskManagementSQLCommands.FetchAssignmentById:
                    {
                        query = @"SELECT 
                                        ASSIGNMENT_ID,
                                        STAFF_ID,
                                        ACADEMIC_YEAR,
                                        CLASS_ID,
                                        COURSE_ID,
                                        ASSIGNMENT_TITLE,
                                        DATE_FROM,
                                        DATE_FORMAT(SUBMISSION_DATE, '%d/%m/%Y') AS SUBMISSION_DATE,
                                        DESCRIPTION,
                                        IS_ACTIVE,
                                        IS_FILE_UPLOAD
                                    FROM
                                        ASSIGNMENT_?AC_YEAR
                                    WHERE
                                        ASSIGNMENT_ID = ?ASSIGNMENT_ID AND IS_DELETED != 1;";
                        break;
                    }
                case TaskManagementSQLCommands.UpdateAssignment:
                    {
                        query = @"UPDATE ASSIGNMENT_?AC_YEAR
                                            SET
                                            STAFF_ID=?STAFF_ID,
                                            ACADEMIC_YEAR=?AC_YEAR,
                                            CLASS_ID=?CLASS_ID,
                                            COURSE_ID=?COURSE_ID,
                                            ASSIGNMENT_TITLE=?ASSIGNMENT_TITLE,
                                            SUBMISSION_DATE=?SUBMISSION_DATE,
                                            IS_ACTIVE=?IS_ACTIVE,
                                            DESCRIPTION=?DESCRIPTION,
                                            IS_FILE_UPLOAD=?IS_FILE_UPLOAD
                                            WHERE ASSIGNMENT_ID=?ASSIGNMENT_ID;";
                        break;
                    }
                case TaskManagementSQLCommands.DeleteAssignment:
                    {
                        query = @"UPDATE ASSIGNMENT_?AC_YEAR
                                            SET
                                            IS_DELETED=1
                                            WHERE ASSIGNMENT_ID=?ASSIGNMENT_ID;";
                        break;
                    }
                case TaskManagementSQLCommands.FetchAssignmentByClassIdAndCourseIdAndStaffId:
                    {
                        query = @"SELECT 
                                    ASSIGNMENT_ID, ASSIGNMENT_TITLE
                                FROM
                                    ASSIGNMENT_?AC_YEAR
                                WHERE
                                    STAFF_ID = ?STAFF_ID AND CLASS_ID = ?CLASS_ID
                                        AND COURSE_ID = ?COURSE_ID;";
                        break;
                    }
                case TaskManagementSQLCommands.FetchAssignmentSubmittedList:
                    {
                        query = @"";
                        break;
                    }
            }
            return query;
        }
    }
}
