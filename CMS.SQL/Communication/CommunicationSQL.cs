using CMS.DAO;

namespace CMS.SQL.Communication
{
    public static class CommunicationSQL
    {
        public static string GetCommunicationSQL(CommunicationSQLCommands sEnumCommand)
        {
            string query = "";

            switch (sEnumCommand)
            {
                case CommunicationSQLCommands.FetchSolutionInfiApi:
                    {
                        query = @"SELECT SMS_SETTING_ID,
                                    BASE_URL,
                                    API_KEY,
                                    METHOD,
                                    FORMAT,
                                    UNICODE,
                                    FLASH,
                                    DLRURL,SENDER,REQUEST_METHOD
                                FROM SMS_SETTING_FOR_SOLUTIONINFI
                                 WHERE 
                                 IS_DELETED!=1
                                AND API_MODE=?API_MODE 
                                AND IS_ACTIVE=1;";
                        break;
                    }
                case CommunicationSQLCommands.UpdateSMSBalance:
                    {
                        query = @"
                                UPDATE SMS_BALANCE
                                        SET
                                        USED_CREDITS = ?USED_CREDITS,
                                        BALANCE_CREDITS = ?BALANCE_CREDITS                                        
                                        WHERE IS_ACTIVE=?IS_ACTIVE  AND SMS_BALANCE_ID!=0;";
                        break;
                    }
                case CommunicationSQLCommands.SaveSentMessages:
                    {
                        query = @"INSERT INTO SENT_SMS_?AC_YEAR (
                                            CLASS_IDS,
                                            START_TIME,
                                            END_TIME,
                                            CONTENT,
                                            SMS_COUNT,
                                            DATE,
                                            STATUS,
                                            TRANSACTION_ID,
                                            CREDIT_COUNT,
                                            SMS_MODE,
                                            RECEIPIENT_NOS,
                                            ACADEMIC_YEAR,USER_TYPE)
                                            VALUES
                                            (
                                            ?CLASS_IDS,
                                            ?START_TIME,
                                            ?END_TIME,
                                            ?CONTENT,
                                            ?SMS_COUNT,
                                            current_date(),
                                            ?STATUS,
                                            ?TRANSACTION_ID,
                                            ?CREDIT_COUNT,
                                            ?SMS_MODE,
                                            ?RECEIPIENT_NOS,
                                            ?ACADEMIC_YEAR,?USER_TYPE);";
                        break;
                    }
                case CommunicationSQLCommands.FetchSMSBalance:
                    {
                        query = @"SELECT SMS_BALANCE_ID,
                                    SMS_DATE,
                                    RECEIVED_CREDITS,
                                    USED_CREDITS,
                                    BALANCE_CREDITS,
                                    IS_DELETED,
                                    ACADEMIC_YEAR,
                                    SMS_LIMIT
                                FROM SMS_BALANCE WHERE IS_ACTIVE=1 AND  IS_DELETED!=1;";
                        break;
                    }
                case CommunicationSQLCommands.FetchStudentInfoForIndividualSMS:
                    {
                        query = @"SELECT 
                                        SC.STUDENT_ID AS USER_ID,
                                        CONCAT(IF(SP.FIRST_NAME IS NULL,
                                                    '',
                                                    FIRST_NAME),
                                                ' ',
                                                IF(SP.LAST_NAME IS NULL, '', LAST_NAME)) AS FIRST_NAME,
                                                SP.FR_MOBILE AS MOBILE,
                                        CLS.CLASS_NAME AS CLASS,
                                        SC.CLASS_ID
                                    FROM
                                        STU_CLASS AS SC
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = SC.CLASS_ID
                                    WHERE
                                        SP.IS_LEFT != 1 AND SP.IS_DELETED != 1
                                            AND SC.IS_DELETED != 1
                                            AND CLS.IS_DELETED != 1
                                            AND SC.CLASS_ID IN (?CLASS_ID);";
                        break;
                    }
                case CommunicationSQLCommands.FetchClasswiseRecordsCount:
                    {
                        query = @"SELECT 
                                COUNT(SPI.STUDENT_ID) AS RECORDSCOUNT
                            FROM
                                STU_CLASS AS SC
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SPI ON SPI.STUDENT_ID = SC.STUDENT_ID
                                    AND SPI.IS_DELETED != 1
                                    AND SPI.IS_LEFT!=1
                            WHERE
                                SC.CLASS_ID IN (?CLASS_ID)
                                    AND SC.IS_DELETED != 1
                                    AND SC.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case CommunicationSQLCommands.FetchStaffInfoForIndividualSMS:
                    {
                        query = @"SELECT 
                                        SP.STAFF_ID AS USER_ID,
                                        CONCAT(IF(SP.FIRST_NAME IS NULL,
                                                    '',
                                                    FIRST_NAME),
                                                ' ',
                                                IF(SP.LAST_NAME IS NULL, '', LAST_NAME)) AS FIRST_NAME,
                                        SA.CCELL_NO AS MOBILE,
                                        '' AS CLASS,
                                        '' AS CLASS_ID
                                    FROM
                                        STF_PERSONAL_INFO AS SP
                                            INNER JOIN
                                        STF_ADDRESS AS SA ON SA.STAFFNO = SP.STAFF_ID
                                    WHERE
                                        SP.IS_DELETED != 1 AND SP.IS_LEFT != 1 AND SP.CATEGORY=?STF_CATEGORY_ID;";

                        break;
                    }
                case CommunicationSQLCommands.FetchClasswiseMobileNumbers:
                    {
                        query = @"SELECT 
                                SPI.MOBILE,
                                SC.STUDENT_ID AS RECEIPIENT_ID,
                                SC.CLASS_ID,
                                '2' AS IS_STAFF
                            FROM
                                STU_CLASS AS SC
                                    INNER JOIN
                                STU_PERSONAL_INFO AS SPI ON SPI.STUDENT_ID = SC.STUDENT_ID
                                    AND SPI.IS_DELETED != 2
                            WHERE
                                SC.CLASS_ID IN (?CLASS_ID)
                                    AND SPI.MOBILE IS NOT NULL
                                    AND SPI.MOBILE != ''
                                    AND SC.IS_DELETED != 2
                                    AND SC.ACADEMIC_YEAR_ID = ?ACADEMIC_YEAR_ID;";
                        break;
                    }
                case CommunicationSQLCommands.FetchStaffRecordsCount:
                    {
                        query = @"SELECT 
                                    COUNT(SA.CCELL_NO) AS 'RECORDSCOUNT'
                                FROM
                                    STF_PERSONAL_INFO AS S
										INNER JOIN
									STF_ADDRESS AS SA ON SA.STAFFNO=S.STAFF_ID 
                                WHERE
                                    SA.CCELL_NO IS NOT NULL AND S.IS_DELETED!=1 AND S.IS_LEFT!=1 AND S.SUB_CATEGORY IN (?STF_CATEGORY_ID);";
                        break;
                    }
                case CommunicationSQLCommands.FetchStaffAndStudentRecordsCount:
                    {
                        query = @"SELECT 
                                COUNT(C.MOBILE) AS RECORDSCOUNT
                            FROM
                                (SELECT 
                                    STAFF.STAFF_ID AS MOBILE
                                FROM
                                    STF_PERSONAL_INFO AS STAFF
                                WHERE
                                        STAFF.IS_DELETED != 1 UNION ALL SELECT 
                                    SP.STUDENT_ID AS MOBILE
                                FROM
                                    STU_PERSONAL_INFO AS SP
                                INNER JOIN STU_CLASS AS SC ON SC.STUDENT_ID = SP.STUDENT_ID
                                    AND SP.IS_DELETED != 1 AND SP.IS_LEFT!=1
                                WHERE
                                        SC.IS_DELETED != 1
                                        AND SC.ACADEMIC_YEAR = ?AC_YEAR) AS C;";
                        break;
                    }
                case CommunicationSQLCommands.FetchSentMessages:
                    {
                        query = @"SELECT 
                                    SENT_SMS_ID,
                                    CLASS_IDS,
                                    DATE_FORMAT(DATE, '%d-%m-%Y') AS DATE,
                                    SS.END_TIME,
                                    CONTENT,
                                    SMS_COUNT,
                                    CREDIT_COUNT,
SS.SMS_MODE,
                                    MT.MESSAGE_TYPE_NAME,
                                    JOB_ID,                                   
                                    IF(STATUS = 0, 'SUCCESS', 'FAILED') AS STATUS
                                FROM
                                    SENT_SMS_?AC_YEAR SS                                      
                                        LEFT JOIN
                                    SUP_MESSAGE_TYPE AS MT ON MT.MSG_TYPE_ID = SS.SMS_MODE
                                ORDER BY SENT_SMS_ID DESC;";
                        break;
                    }
                case CommunicationSQLCommands.FetchTransactionIdBySentId:
                    {
                        query = @"SELECT 
	                                SENT_SMS_LIST_ID,
                                    SENT_ITEM_ID,
                                    TRANSACTION_ID,
                                    MOBILE_NO,
                                    IS_STAFF,
                                    CONTENT
                                FROM
                                    SENT_SMS_LIST_?AC_YEAR WHERE SENT_ITEM_ID = ?SENT_ITEM_ID AND IS_UPDATED!=1;";
                        break;
                    }
                case CommunicationSQLCommands.FetchSentListBySentId:
                    {
                        query = @"SELECT 
	                                SENT_SMS_LIST_ID,
                                    SENT_ITEM_ID,
                                    TRANSACTION_ID,
                                    MOBILE_NO,
                                    IS_STAFF,
                                    CONTENT
                                FROM
                                    SENT_SMS_LIST_?AC_YEAR WHERE SENT_ITEM_ID = ?SENT_ITEM_ID GROUP BY SENT_ITEM_ID;";
                        break;
                    }
                case CommunicationSQLCommands.FetchSentItemForViewMode:
                    {
                        query = @"SELECT 
                                    SP.SENT_SMS_LIST_ID,
                                   'N/A' AS REGISTER_NUMBER,
                                    '' AS NAME,
                                    SP.`DATE`,
                                    SP.COMPOSED_TIME,
                                    SP.SENT_TIME,
                                    SP.CONTENT,
                                    SP.RECEIPIENT_ID,
                                    'N/A' AS CLASS,
                                    SP.STATUS AS 'STATUS',
                                    SP.CREDIT_COUNT,
                                    SP.MOBILE_NO,
                                    DATE_FORMAT(SP.DELIVERED_DATE,'%d/%m/%y  %H:%I:%S %P') AS DELIVERED_DATE ,
	                                DATE_FORMAT(SP.SENT_DATE,'%d/%m/%y %H:%I:%S %P') AS SENT_DATE ,
                                    SP.SENT_ITEM_ID
                               FROM
                                   SENT_SMS_LIST_?AC_YEAR AS SP
                               WHERE
	                               SENT_ITEM_ID = ?SENT_ITEM_ID;";
                        break;
                    }
                case CommunicationSQLCommands.FetchSenderInfoForResend:
                    {
                        query = @"SELECT 
                                    RECEIPIENT_ID, IS_STAFF, MOBILE_NO, CLASS_ID AS CLASS,CONTENT
                                FROM
                                    SENT_SMS_LIST_?AC_YEAR
                                WHERE
                                    SENT_SMS_LIST_ID = ?SENT_SMS_LIST_ID;";
                        break;
                    }
                case CommunicationSQLCommands.FetchStaffMobileNumbers:
                    {
                        query = @"SELECT 
                                    SA.CCELL_NO AS 'MOBILE',
                                        S.STAFF_ID AS RECEIPIENT_ID,
                                        '' AS CLASS_ID,
                                        '3' AS IS_STAFF
                                FROM
                                    STF_PERSONAL_INFO AS S
										INNER JOIN
									STF_ADDRESS AS SA ON SA.STAFFNO=S.STAFF_ID 
                                WHERE
                                    SA.CCELL_NO IS NOT NULL AND S.IS_DELETED!=1 AND S.IS_LEFT!=1 AND S.SUB_CATEGORY IN (?STF_CATEGORY_ID);";
                        break;
                    }
                case CommunicationSQLCommands.FetchStudentSentItemForViewMode:
                    {
                        query = @"SELECT 
                                    SP.SENT_SMS_LIST_ID,
                                    CONCAT(AI.APPLICATION_NO, AI.ISSUED_ID) AS APPLICATION_NO,
                                    CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                            ' ',
                                            IFNULL(AR.LAST_NAME, '')) AS NAME,
                                    SP.CONTENT,
                                    SP.RECEIPIENT_ID,
                                    CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                            '- ',
                                            SS.SHIFT_NAME,
                                            '- ',
                                            IF(PG.PROGRAMME_MODE = 1, 'AIDED', 'SF')) AS PROGRAMME_NAME,
                                    SP.STATUS AS 'STATUS',
                                    SP.MOBILE_NO,
                                    DATE_FORMAT(SP.DELIVERED_DATE,
                                            '%d/%m/%y  %h:%i:%s %p') AS DELIVERED_DATE,
                                    DATE_FORMAT(SP.SENT_DATE, '%d/%m/%y %h:%i:%s %p') AS SENT_DATE,
                                    SP.SENT_ITEM_ID
                                FROM
                                    SENT_SMS_LIST_?AC_YEAR AS SP
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS AI ON AI.ISSUED_ID = SP.RECEIPIENT_ID
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                WHERE
                                    SENT_ITEM_ID = ?SENT_ITEM_ID;";
                        break;
                    }
                case CommunicationSQLCommands.FetchStaffSentItemForViewMode:
                    {
                        query = @"SELECT 
                                SP.SENT_SMS_LIST_ID,
                                ST.STAFF_CODE AS APPLICATION_NO,
                                CONCAT(IFNULL(ST.FIRST_NAME, ''),
                                        ' ',
                                        IFNULL(ST.LAST_NAME,'')) AS NAME,
                                SP.CONTENT,
                                SP.RECEIPIENT_ID,
                                'N/A' AS PROGRAMME_NAME,
                                SP.STATUS AS 'STATUS',
                                SP.MOBILE_NO,
                                DATE_FORMAT(SP.DELIVERED_DATE,
                                        '%d/%m/%y  %h:%i:%s %p') AS DELIVERED_DATE,
                                DATE_FORMAT(SP.SENT_DATE, '%d/%m/%y %h:%i:%s %p') AS SENT_DATE,
                                SP.SENT_ITEM_ID
                            FROM
                                SENT_SMS_LIST_?AC_YEAR AS SP
                                    INNER JOIN
                                STF_PERSONAL_INFO AS ST ON ST.STAFF_ID = SP.RECEIPIENT_ID
                            WHERE
                                SENT_ITEM_ID = ?SENT_ITEM_ID;";
                        break;
                    }
                case CommunicationSQLCommands.FetchOthersSentItemForViewMode:
                    {
                        query = @"SELECT 
                            SP.SENT_SMS_LIST_ID,
                            'N/A' AS APPLICATION_NO,
                            'N/A' AS NAME,
                            SP.CONTENT,
                            SP.RECEIPIENT_ID,
                            'N/A' AS PROGRAMME_NAME,
                            SP.STATUS AS 'STATUS',
                            SP.MOBILE_NO,
                            DATE_FORMAT(SP.DELIVERED_DATE,
                                    '%d/%m/%y  %h:%i:%s %p') AS DELIVERED_DATE,
                            DATE_FORMAT(SP.SENT_DATE, '%d/%m/%y %h:%i:%s %p') AS SENT_DATE,
                            SP.SENT_ITEM_ID
                        FROM
                            SENT_SMS_LIST_?AC_YEAR AS SP
                        WHERE
                            SENT_ITEM_ID =?SENT_ITEM_ID;";
                        break;
                    }
                case CommunicationSQLCommands.FetchDovesoftSettings:
                    {
                        query = @"SELECT 
                                    SMS_SETTING_ID,
                                    SENDER,
                                    BASE_URL,
                                    API_KEY,
                                    METHOD,
                                    FORMAT,
                                    ACCUSAGE,
                                    USER,
                                    IS_ACTIVE,
                                    IS_DELETED
                                FROM
                                    SMS_SETTING_FOR_DOVE_SOFT
                                WHERE
                                    IS_DELETED != 1 AND API_METHOD = ?API_METHOD;";
                        break;
                    }
                case CommunicationSQLCommands.FetchActiveSMSvendors:
                    {
                        query = @"SELECT 
                                SMS_VENDOR_ID,
                                VENDOR_NAME,
                                VENDOR_PHONE,
                                URL,
                                ACCOUNT_MANAGER_NO,
                                SUPPORT_EMAIL_ID,
                                SALES_EMAIL_ID,
                                INTEGREATION_EMAIL_ID,
                                IS_ACTIVE,
                                IS_DELETED
                            FROM
                                SMS_VENDOR_DETAILS
                            WHERE
                                IS_ACTIVE = 1 AND IS_DELETED != 1;";
                        break;
                    }


            }
            return query;
        }
    }
}
