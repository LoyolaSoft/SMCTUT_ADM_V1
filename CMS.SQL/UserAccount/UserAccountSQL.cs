using CMS.DAO;

namespace CMS.SQL.UserAccount
{
    public static class UserAccountSQL
    {
        public static string GetSupportDataSQL(UserAccountSQLCommands sEnumCommand)
        {
            string query = "";

            switch (sEnumCommand)
            {
                case UserAccountSQLCommands.FetchUserAccountByName:
                    {
                        query = @"SELECT USER_ACCOUNT_ID,PASSWORD,USERNAME,ROLE,USER_ID,USER_TYPE FROM USER_ACCOUNT WHERE USERNAME=?USERNAME;";
                        break;
                    }
                case UserAccountSQLCommands.FetchSupRole:
                    {
                        query = @"SELECT 
                                    ROLE_ID,
                                    ROLE_NAME
                                FROM
                                    SUP_ROLE;";
                        break;
                    }
                case UserAccountSQLCommands.FetchUserType:
                    {
                        query = @"SELECT                                     
                                    USER_TYPE_ID,
                                    USER_TYPE_NAME
                                FROM
                                    SUP_USER_TYPE;";
                        break;
                    }
                case UserAccountSQLCommands.DeleteUserAccount:
                    {
                        query = @"UPDATE USER_ACCOUNT SET IS_DELETED=1 WHERE USER_ACCOUNT_ID=?USER_ACCOUNT_ID;";
                        break;
                    }
                case UserAccountSQLCommands.FetchUserAccountByRoleAndId:
                    {
                        query = @"SELECT 
                                    USER_ACCOUNT_ID,
                                    USERNAME,
                                    PASSWORD,
                                    NAME,
                                    EMAIL,
                                    MOBILE,
                                    USER_ID,ROLE,USER_TYPE
                                    FROM
                                       USER_ACCOUNT WHERE USER_ID=?USER_ID AND USER_TYPE=?USER_TYPE;";
                        break;
                    }
                case UserAccountSQLCommands.SaveUserAccount:
                    {
                        query = @"INSERT INTO USER_ACCOUNT(
                                    USERNAME,
                                    PASSWORD,
                                    NAME,
                                    EMAIL,
                                    MOBILE,
                                    USER_ID,ROLE,USER_TYPE)
                                VALUES 
	                                (?USERNAME,?PASSWORD,?NAME,?EMAIL,?MOBILE,?USER_ID,?ROLE,?USER_TYPE);";
                        break;
                    }
                case UserAccountSQLCommands.UpdateUserAccount:
                    {
                        query = @"UPDATE USER_ACCOUNT SET  
                                USERNAME=?USERNAME,
                                PASSWORD=?PASSWORD,
                                NAME=?NAME,
                                EMAIL=?EMAIL,
                                MOBILE=?MOBILE,
                                USER_ID=?USER_ID,
                                ROLE=?ROLE,
                                USER_TYPE=?USER_TYPE
	                            WHERE USER_ACCOUNT_ID=?USER_ACCOUNT_ID";
                        break;
                    }
                case UserAccountSQLCommands.ValidateUserRole:
                    {
                        query = @"SELECT 
	                               count(*)>0 AS ROLE_STATUS
                                    FROM
                                        MENU_ITEMS AS MI
                                            INNER JOIN
                                        USER_ROLES_RIGHTS AS UR ON MI.MENU_ID = UR.MENU_ITEM_ID
                                        INNER JOIN SUP_ROLE AS SP ON SP.ROLE_ID=UR.ROLE_ID
                                    WHERE
                                        SP.ROLE_NAME = ?ROLE_NAME
                                            AND MI.CONTROLLER = ?CONTROLLER
                                            AND MI.ACTION = ?ACTION AND UR.IS_ACTIVE=1;";
                        break;
                    }
                case UserAccountSQLCommands.FetchUserCredentials:
                    {
                        query = @"SELECT 
                                        UA.USER_ACCOUNT_ID,
                                        UA.USERNAME,
                                        SR.ROLE_NAME,
                                        UA.ROLE,
                                        SU.USER_TYPE_NAME,
                                        UA.USER_ID,
                                        UA.NAME,
                                        UA.PHOTO,CONTROLLER_NAME,ACTION_NAME
                                    FROM
                                        USER_ACCOUNT AS UA
                                            LEFT JOIN
                                        SUP_ROLE AS SR ON SR.ROLE_ID = UA.ROLE
                                            LEFT JOIN
                                        SUP_USER_TYPE AS SU ON SU.USER_TYPE_ID = UA.USER_TYPE
                                    WHERE
                                        UA.USERNAME = ?USERNAME
                                            AND UA.PASSWORD = ?PASSWORD AND UA.IS_DELETED!=1;";
                        break;

                    }
                case UserAccountSQLCommands.FetchMenuItemsByRole:
                    {
                        query = @"SELECT 
                                        M.MODULE_NAME, MI.ACTION, MI.CONTROLLER, MI.MENU_NAME,M.STYLE,M.HAS_SUB
                                    FROM
                                        USER_ROLES_RIGHTS AS RR
                                            INNER JOIN
                                        MENU_ITEMS AS MI ON MI.MENU_ID = RR.MENU_ITEM_ID
                                            INNER JOIN
                                        MODULES AS M ON M.MODULES_ID = MI.PARENT_ID
                                    WHERE
                                        RR.ROLE_ID = ?ROLE_ID  AND RR.IS_ACTIVE=1
                                    ORDER BY M.MODULES_ORDER , MI.MENU_NAME; ";
                        break;
                    }
                case UserAccountSQLCommands.FetchCollegeInfo:
                    {
                        query = @"SELECT 
                                    COLLEGE_ID,
                                    MANAGEMENT_NAME,
                                    COLLEGE_NAME,
                                    PLACE,
                                    EMAIL,
                                    COLLEGE_CODE,
                                    COLLEGE_LOGO
                                FROM
                                    COLLEGE_DETAILS;";

                        break;
                    }
                case UserAccountSQLCommands.FetchActiveYear:
                    {
                        query = @"SELECT 
                                        ACADEMIC_YEAR_ID,
                                        AC_YEAR,
                                        ACADEMIC_NAME
                                    FROM
                                        ACADEMIC_YEAR
                                    WHERE
                                        ACTIVE_YEAR=1
                                    ORDER BY ACADEMIC_YEAR_ID;";
                        break;
                    }
            }
            return query;
        }
    }
}
