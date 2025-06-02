using CMS.DAO;

namespace CMS.SQL.Account
{
    public class AccountSQL
    {
        public static string GetAccountSQL(AccountSQLCommands sEnumCommand)
        {
            string query = string.Empty;
            switch (sEnumCommand)
            {

                case AccountSQLCommands.FetchUserAccountListByUserIds:
                    {
                        query = @"SELECT 
                                    USER_ACCOUNT_ID,
                                    USERNAME,
                                    CONCAT(IFNULL(AI.FIRST_NAME, ''),
                                            ' ',
                                            IFNULL(AI.LAST_NAME, '')) AS NAME,
                                    DATE_FORMAT(AI.DOB, '%d/%m/%Y') AS DOB,
                                    PRO.PROGRAMME_NAME,
                                    SH.DESCRIPTION,
                                    PM.PROGRAMME_MODE,
                                    AI.HSC_NO,AI.APPLICATION_NO,UA.USER_ID,
    UA.MOBILE
                                FROM
                                    USER_ACCOUNT AS UA
                                        INNER JOIN
                                    ADM_ISSUE_APPLICATION AS AI ON AI.ISSUE_ID = UA.USER_ID
                                        INNER JOIN
                                    ADM_APPTYPE_PROG_GROUPS AS AP ON AP.PRO_GROUPS_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME_?AC_YEAR AS PRO ON PRO.PROGRAMME_ID = AP.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_SHIFT AS SH ON SH.SHIFT_ID = AP.SHIFT
                                        INNER JOIN
                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                                WHERE
                                    UA.ROLE = 12 AND AI.IS_PAID = 1 AND AI.IS_DELETED!=1
                                        AND USER_ID IN (?USER_ID)";
                        break;
                    }
                case AccountSQLCommands.FetchUserAccountList:
                    {

                        query = @"SELECT 
                                        USER_ACCOUNT_ID,
                                        USERNAME,
                                        AI.FIRST_NAME AS NAME,
                                        DATE_FORMAT(AI.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB
                                    FROM
                                        USER_ACCOUNT AS UA
                                            INNER JOIN
                                        adm_receive_application AS AI ON AI.RECEIVE_ID = UA.USER_ID
                                    WHERE
                                        UA.ROLE = 12 AND AI.IS_DELETED != 1;";

                        break;
                    }
                case AccountSQLCommands.UpdatePassword:
                    {
                        query = @"UPDATE USER_ACCOUNT SET PASSWORD=?CONFIRM_PASSWORD WHERE USER_ACCOUNT_ID=?USER_ACCOUNT_ID";
                        break;
                    }
                case AccountSQLCommands.FetchPassword:
                    {
                        query = @"SELECT 
                                        PASSWORD
                                    FROM
                                        USER_ACCOUNT
                                    WHERE
                                        USERNAME = ?USERNAME;";
                        break;
                    }
                case AccountSQLCommands.FetchEmailAddress:
                    {
                        query = @"select USER_ACCOUNT_ID,USERNAME,EMAIL from user_account where EMAIL=?EMAIL;";
                        break;
                    }
                case AccountSQLCommands.FetchUserAccountListByUserId:
                    {
                        query = @"SELECT 
                                USER_ACCOUNT_ID, USERNAME
                            FROM
                                USER_ACCOUNT AS UA
                                    INNER JOIN
                                adm_receive_application AS AI ON AI.RECEIVE_ID = UA.USER_ID
                            WHERE
                                UA.ROLE =?ROLE AND UA.USER_ID =?USER_ID
                                    AND AI.IS_DELETED != 1;";
                        break;
                    }
                case AccountSQLCommands.FetchUserRoleDetails:
                    {
                        query = @"SELECT 
                                    U.USER_ACCOUNT_ID,
                                    U.USERNAME,
                                    ROLE,
                                    NAME,
                                    U.USER_ID
                                FROM
                                    USER_ACCOUNT AS U                                        
                                WHERE
                                    U.IS_DELETED != 1;";
                        break;
                    }
                case AccountSQLCommands.FetchUserDetailsForStaff:
                    {
                        query = @"SELECT 
                                    U.USER_ACCOUNT_ID,
                                    U.USERNAME,
                                    SP.FIRST_NAME as NAME,
                                    SP.MOBILE_NO as MOBILE,
                                    U.USER_ID,
                                    date_format(SP.DATE_OF_BIRTH,'%d/%m/%Y') AS DATE_OF_BIRTH
                                FROM
                                    USER_ACCOUNT AS U
                                        INNER JOIN
                                    STF_PERSONAL_INFO AS SP ON SP.STAFF_CODE = U.USERNAME
                                        AND SP.STAFF_ID = U.USER_ID
                                        AND SP.STAFF_CODE = ?USERNAME
                                        AND SP.IS_DELETED != 1
                                WHERE
                                    U.IS_DELETED != 1;";
                        break;
                    }
                case AccountSQLCommands.FetchUserDetailsForStudent:
                    {
                        query = @"SELECT 
                                    U.USER_ACCOUNT_ID,
                                    U.USERNAME,
                                    NAME,
                                   MOBILE,
                                    U.USER_ID
                                FROM
                                    USER_ACCOUNT AS U
                                WHERE
                                    U.IS_DELETED != 1 AND U.USERNAME = ?USERNAME;";
                        break;
                    }
                case AccountSQLCommands.FetchUserDetails:
                    {
                        query = @"SELECT 
                                      U.USER_ACCOUNT_ID,
                                    U.USERNAME,
                                    ROLE,
                                    NAME,
                                    U.USER_ID
                                FROM
                                    USER_ACCOUNT AS U                                       
                                WHERE
                                    U.IS_DELETED != 1;";
                        break;
                    }
                case AccountSQLCommands.FetchUserDetailsByUsername:
                    {
                        query = @"SELECT 
                                      U.USER_ACCOUNT_ID,
                                    U.USERNAME,
                                    ROLE,
                                    NAME,
                                    U.USER_ID
                                FROM
                                    USER_ACCOUNT AS U                                       
                                WHERE
                                    U.IS_DELETED != 1 AND USERNAME=?USERNAME;";
                        break;
                    }
                case AccountSQLCommands.ResetPassword:
                    {
                        query = @"UPDATE USER_ACCOUNT SET PASSWORD=?PASSWORD WHERE USER_ACCOUNT_ID=?USER_ACCOUNT_ID;";
                        break;
                    }
            }
            return query;
        }

    }
}
