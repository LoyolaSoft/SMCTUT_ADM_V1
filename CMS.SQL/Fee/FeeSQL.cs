
using CMS.DAO;

namespace CMS.SQL.Fee
{

    public static class FeeSQL
    {
        public static string GetFeeSQL(FeeSQLCommands sEnumCommand)
        {
            string query = "";
            switch (sEnumCommand)
            {


                case FeeSQLCommands.FetchFrequencyType:
                    {
                        query = @"SELECT 
                                        FEE_FREQUENCY_FEE_MODE_ID, FREQUENCY_ID, FEE_MODE
                                    FROM
                                        fee_frequency_fee_mode AS fr
                                    WHERE
                                        fr.FEE_FREQUENCY_FEE_MODE_ID = ?FREQUENCY_ID
                                            AND fr.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }

                case FeeSQLCommands.FetchFeeRazorpayTransferInfo:
                    {

                        query = @"SELECT RAZORPAY_TRANSFER_ID,
                                     ID,
                                     ENTITY, 
                                     SOURCE, 
                                     RECIPIENT, 
                                     AMOUNT,
                                     CURRENCY,
                                     AMOUNT_REVERSED,
                                     FEES, TAX,
                                     ON_HOLD, 
                                     ON_HOLD_UNTIL, 
                                     RECIPIENT_SETTLEMENT_ID, 
                                     CREATED_AT, 
                                     LINKED_ACCOUNT_NOTES, 
                                     UDF1, 
                                     UDF2,
                                     UDF3, 
                                     UDF4,
                                     UDF5
                                     FROM FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR AS T 
                                    WHERE T.UDF1=?STUDENT_ID AND T.UDF2=?RAZORPAY_ID AND T.UDF3=?MAIN_HEAD_ID AND T.UDF4=?BANK_ACCOUNT_ID ;";
                        break;
                    }
                case FeeSQLCommands.FetchIssuedApplicationInfoByReceiveIdAndProgrammeGroupId:
                    {

                        query = @"SELECT 
                                        IA.ISSUED_ID,
                                        concat(IA.APPLICATION_NO,LPAD(IA.ISSUE_NO,4,'0')) as APPLICATION_NO,
                                        IA.DEPARTMENT_CODE,
                                        IA.RECEIVE_ID,
                                        IA.PROGRAMME_GROUP_ID,
                                        IA.STATUS,
                                        IA.IS_ACTIVE,
                                        IA.IS_DELETED,
                                        IA.ACADEMIC_YEAR,
                                        IA.IS_PAID,
                                        R.FIRST_NAME,
                                        R.SMS_MOBILE_NO,
                                        R.EMAIL_ID
                                    FROM
                                        ADM_ISSUED_APPLICATIONS AS IA
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS r ON r.RECEIVE_ID = IA.RECEIVE_ID
                                WHERE
                                    IA.ACADEMIC_YEAR = ?AC_YEAR
                                        AND IA.PROGRAMME_GROUP_ID = ?PROGRAMME_GROUP_ID
                                        AND IA.RECEIVE_ID = ?RECEIVE_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchApplicatinInfo:
                    {

                        query = @"SELECT 
                                        UPPER(R.FIRST_NAME) FIRST_NAME,
                                        R.RECEIVE_ID,
                                       R.SMS_MOBILE_NO,
                                            R.EMAIL_ID,
                                            sum(FS.AMOUNT) as AMOUNT,
                                        FM.FREQUENCY_ID,
                                        SFM.MAIN_HEAD,
                                        CONCAT(P.PROGRAMME_NAME,
                                                '-',
                                                PM.PROGRAMME_MODE) AS PROGRAMME_NAME
                                    FROM
                                        fee_structure AS FS
                                            INNER JOIN
                                        fee_main_heads AS FM ON FM.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                            INNER JOIN
                                        adm_receive_application AS R ON R.RECEIVE_ID = ?RECEIVE_ID
                                            INNER JOIN
                                        sup_fee_main_head AS SFM ON SFM.MAIN_HEAD_ID = FM.MAIN_HEAD_ID
                                            INNER JOIN
                                        adm_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = FS.PROGRAMME
                                            INNER JOIN
                                        cp_programme AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                            INNER JOIN
                                        sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                            INNER JOIN
                                        sup_shift AS SH ON SH.SHIFT_ID = PG.SHIFT
                                    WHERE
                                        FS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FS.FREQUENCY  in (?FREQUENCY_ID)
                                            AND FS.PROGRAMME = ?PROGRAMME
                                            AND FS.CLASS_YEAR_ID = ?CLASS_YEAR_ID
                                    GROUP BY FM.MAIN_HEAD_ID";
                        break;
                    }
                case FeeSQLCommands.FecthMessFeesByStudentIdAndFeeRootId:
                    {
                        query = @"SELECT 
                                CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                FMH.FREQUENCY_ID,
                                FS.STUDENT_ID,
                                SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                                FF.FREQUENCY_NAME,
                                AR.FIRST_NAME,
                                SFM.MAIN_HEAD AS HEAD,
                                AR.SMS_MOBILE_NO,
                                FT.FREQUENCY_TYPE AS FREQUENCY_NAME,
                                SP.PROGRAMME_GROUP_ID AS PROGRAMME_ID
                            FROM
                                FEE_STUDENT_ACCOUNT AS FS
                                    INNER JOIN
                                ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS SP ON SP.RECEIVE_ID = FS.STUDENT_ID
                                    AND SP.IS_DELETED != 1
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SP.RECEIVE_ID
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SP.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = SP.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SP.PROGRAMME_GROUP_ID
                                    AND PG.IS_DELETED != 1
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FMH ON FS.FEE_MAIN_HEAD_ID = FMH.FEE_MAIN_HEAD_ID
                                    AND FMH.IS_DELETED != 1
                                    INNER JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                    AND FFF.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY_ID
                                    INNER JOIN
                                SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                    AND FT.FEE_ROOT_ID = ?FEE_ROOT_ID
                                    INNER JOIN
                                SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    LEFT JOIN
                                SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                    LEFT JOIN
                                SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                            WHERE
                                FS.STUDENT_ID = ?STUDENT_ID
                                    AND FT.FREQUENCY_TYPE_ID = ?FREQUENCY_ID
                                    AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FS.IS_DELETED != 1
                            GROUP BY FS.STUDENT_ID , FS.FREQUENCY_ID , FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchRazorPayOrderInfoByPaidStatus:
                    {
                        query = @"SELECT 
                                    O.ID, O.AMOUNT
                                FROM
                                    FEE_RAZORPAY_ORDER_INFO_?AC_YEAR AS O
                                        LEFT JOIN
                                    FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS P ON P.ORDER_ID = O.ID
                                WHERE
                                    O.IS_DELETED != 1 AND O.STATUS = 'PAID'
                                        AND P.ID IS NULL;";
                        break;
                    }
                case FeeSQLCommands.FetchRazorPaySettlementInfoById:
                    {
                        query = @"SELECT `SETTLEMENT_ID`,
                                    `ID`,
                                    `ENTITY`,
                                    `AMOUNT`,
                                    `STATUS`,
                                    `FEES`,
                                    `TAX`,
                                    `UTR`,
                                    `CREATED_AT`,
                                    `RECIPIENT`
                                FROM `FEE_RAZORPAY_SETTLEMENT_INFO_?AC_YEAR` WHERE ID=?ID;";
                        break;
                    }
                case FeeSQLCommands.FetchRazorPayOrderInfoForUpdate:
                    {
                        query = @"SELECT 
                                    RAZORPAY_ORDER_ID, ID
                                FROM
                                    FEE_RAZORPAY_ORDER_INFO_?AC_YEAR AS O
                                WHERE
                                    STATUS = 'CREATED';";

                        break;
                    }
                case FeeSQLCommands.FetchRazorPayTransfersForSettlemetUpdate:
                    {
                        query = @"SELECT 
                                    RAZORPAY_TRANSFER_ID, ID
                                FROM
                                    FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR
                                WHERE
                                    RECIPIENT_SETTLEMENT_ID = '' ;";

                        break;
                    }
                case FeeSQLCommands.FetchRazorPaySettlementIds:
                    {
                        query = @"SELECT 
                                    RAZORPAY_TRANSFER_ID, ID,RECIPIENT_SETTLEMENT_ID
                                FROM
                                    FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR
                                WHERE
                                    RECIPIENT_SETTLEMENT_ID != '' GROUP BY RECIPIENT_SETTLEMENT_ID;";

                        break;
                    }
                case FeeSQLCommands.FetchRazorpayTransferListForCollegeTransactoins:
                    {
                        query = @"SELECT 
                                   FMH.FREQUENCY_ID,
                                    SP.RECEIVE_ID AS STUDENT_ID,
                                    SUM((IFNULL(FC.PAID_AMOUNT, 0))) AS BALANCE,
                                    FF.FREQUENCY_NAME,
                                    CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                            ' ',
                                            IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                   SFM.MAIN_HEAD AS HEAD_NAME,
                                    SP.EMAIL_ID AS STU_EMAILID,
                                    SP.SMS_MOBILE_NO AS  STU_MOBILENO,
                                    -- SP.ROLL_NO,
                                    -- SP.REGISTER_NO,
                                    PR.STATUS,PR.UDF6,
                                    DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS TRANSACTION_DATE,F.RECEIPT_NO,RA.ACCOUNT_ID,PR.ID,FMH.MAIN_HEAD_ID,FMH.BANK_ACCOUNT_ID,F.RAZORPAY_ID,PR.UDF2
                                    
                                    
                                FROM
                                    FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PR
                                        INNER JOIN
                                    FEE_TRANSACTION AS F ON F.RAZORPAY_ID = PR.RAZORPAY_PAMENT_ID
                                        INNER JOIN
                                    FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = F.TRANSACTION_ID
                                        INNER JOIN
                                    adm_receive_application AS SP ON SP.RECEIVE_ID = PR.UDF1
                                        
                                        
                                        INNER JOIN
                                    FEE_MAIN_HEADS AS FMH ON 
                                        FMH.FEE_MAIN_HEAD_ID=FC.FEE_MAIN_HEAD_ID
                                        INNER JOIN
                                    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                        AND FFF.ACADEMIC_YEAR =  ?AC_YEAR
                                        AND FFF.FEE_FREQUENCY_FEE_MODE_ID = F.FREQUENCY
                                        INNER JOIN
                                    SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                        AND FT.FEE_ROOT_ID = PR.UDF5
                                        INNER JOIN
                                    SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                        LEFT JOIN
                                    SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        LEFT JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                    LEFT JOIN fee_razorpay_accounts AS RA ON RA.BANK_ACCOUNT_ID=FMH.BANK_ACCOUNT_iD
                                WHERE
                                    PR.RAZORPAY_PAMENT_ID = ?RAZORPAY_PAMENT_ID
                                GROUP BY SP.RECEIVE_ID , F.FREQUENCY , FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchRazorpayTransferListForNonCollegeTransactoins:
                    {
                        query = @"SELECT 
                                FMH.FREQUENCY_ID,
                                SP.RECEIVE_ID AS STUDENT_ID,
                                SUM((IFNULL(FC.PAID_AMOUNT, 0))) AS BALANCE,
                                FF.FREQUENCY_NAME,
                                CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                        ' ',
                                        IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                SFM.MAIN_HEAD AS HEAD_NAME,
                                SP.EMAIL_ID AS STU_EMAILID,
                                SP.SMS_MOBILE_NO AS STU_MOBILENO,
                                PR.STATUS,
                                PR.UDF6,
                                DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS TRANSACTION_DATE,
                                F.RECEIPT_NO,
                                RA.ACCOUNT_ID,
                                PR.ID,
                                FMH.MAIN_HEAD_ID,
                                FMH.BANK_ACCOUNT_ID,
                                F.RAZORPAY_ID,
                                PR.UDF2
                            FROM
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PR
                                    INNER JOIN
                                FEE_TRANSACTION AS F ON F.RAZORPAY_ID = PR.RAZORPAY_PAMENT_ID
                                    INNER JOIN
                                FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = F.TRANSACTION_ID
                                    INNER JOIN
                                adm_receive_application AS SP ON SP.RECEIVE_ID = PR.UDF1
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID
                                    INNER JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                    AND FFF.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FFF.FEE_FREQUENCY_FEE_MODE_ID = F.FREQUENCY
                                    INNER JOIN
                                SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                    AND FT.FEE_ROOT_ID = PR.UDF5
                                    INNER JOIN
                                SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    LEFT JOIN
                                SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                    LEFT JOIN
                                SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                    LEFT JOIN
                                fee_razorpay_accounts AS RA ON RA.BANK_ACCOUNT_ID = FMH.BANK_ACCOUNT_iD
                            WHERE
                                PR.RAZORPAY_PAMENT_ID = ?RAZORPAY_PAMENT_ID
                            GROUP BY SP.RECEIVE_ID , F.FREQUENCY , FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchRazorpayPaymentStatusForNonCollegeTransactions:
                    {
                        query = @"SELECT 
                                    FMH.FREQUENCY_ID,
                                    SP.STUDENT_ID,
                                    SUM((IFNULL(FC.PAID_AMOUNT, 0))) AS BALANCE,
                                    FF.FREQUENCY_NAME,
                                    CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                            ' ',
                                            IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                    SFM.MAIN_HEAD AS HEAD_NAME,
                                    SP.STU_EMAILID,
                                    SP.STU_MOBILENO,
                                    SP.ROLL_NO,
                                    SP.REGISTER_NO,
                                    PR.STATUS,
                                    DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS TRANSACTION_DATE,F.RECEIPT_NO
                                FROM
                                    FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PR
                                        INNER JOIN
                                    FEE_TRANSACTION AS F ON F.RAZORPAY_ID = PR.RAZORPAY_PAMENT_ID
                                        INNER JOIN
                                    FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = F.TRANSACTION_ID
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = PR.UDF1
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SP.PROGRAM_ID
                                        AND PG.IS_DELETED != 1
                                        INNER JOIN
                                    FEE_MAIN_HEADS AS FMH ON F.FREQUENCY=FMH.FREQUENCY_ID
                                        AND FC.HEAD = FMH.HEAD_ID
                                        AND FMH.ACADEMIC_YEAR = ?AC_YEAR
                                        AND FMH.IS_DELETED != 1
                                        INNER JOIN
                                    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                        AND FFF.ACADEMIC_YEAR = ?AC_YEAR
                                        AND FFF.FEE_FREQUENCY_FEE_MODE_ID = F.FREQUENCY
                                        INNER JOIN
                                    SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                        AND FT.FEE_ROOT_ID = PR.UDF5
                                        INNER JOIN
                                    SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                        LEFT JOIN
                                    SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        LEFT JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                WHERE
                                    PR.RAZORPAY_PAMENT_ID = ?RAZORPAY_PAMENT_ID
                                GROUP BY SP.STUDENT_ID , F.FREQUENCY , FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchRazorpayPaymentStatusForCollegeTranctions:
                    {
                        query = @" SELECT FMH.FREQUENCY_ID,
                                    SP.RECEIVE_ID as STUDENT_ID,
                                    SUM((IFNULL(FC.PAID_AMOUNT, 0))) AS BALANCE,
                                    FF.FREQUENCY_NAME,
                                    upper( SP.FIRST_NAME) AS FIRST_NAME,
                                    SFM.MAIN_HEAD AS HEAD_NAME,
                                    SP.EMAIL_ID as  STU_EMAILID,
                                    SP.SMS_MOBILE_NO as  STU_MOBILENO,
                                    CONCAT(P.PROGRAMME_NAME,
                                                '-',
                                                PM.PROGRAMME_MODE
                                               ) AS PROGRAMME_NAME,
                                    PR.STATUS,
                                    DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS TRANSACTION_DATE,F.RECEIPT_NO,RA.ACCOUNT_ID,PR.ID,FMH.MAIN_HEAD_ID,FMH.BANK_ACCOUNT_ID,F.RAZORPAY_ID,PR.UDF2,CONCAT(IA.APPLICATION_NO, LPAD(IA.ISSUE_NO,4,'0')) AS APPLICATION_NO
                                FROM
                                    FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PR
                                        INNER JOIN
                                    FEE_TRANSACTION AS F ON F.RAZORPAY_ID = PR.RAZORPAY_PAMENT_ID
                                        INNER JOIN
                                    FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = F.TRANSACTION_ID
                                        INNER JOIN
                                    adm_receive_application AS SP ON SP.RECEIVE_ID = PR.UDF1                                   
                                        INNER JOIN
                                    FEE_MAIN_HEADS AS FMH ON FMH.fee_main_head_id = FC.FEE_MAIN_HEAD_ID                                       
                                        INNER JOIN
                                    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                        AND FFF.ACADEMIC_YEAR =  ?AC_YEAR
                                        AND FFF.FEE_FREQUENCY_FEE_MODE_ID = F.FREQUENCY
                                        INNER JOIN
                                    SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                        AND FT.FEE_ROOT_ID = PR.UDF5
                                        INNER JOIN
                                    SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                        LEFT JOIN
                                    SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        LEFT JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                        LEFT JOIN fee_razorpay_accounts AS RA ON RA.BANK_ACCOUNT_ID=FMH.BANK_ACCOUNT_iD
                                        LEFT JOIN
                                            adm_issued_applications AS IA ON IA.ACADEMIC_YEAR = ?AC_YEAR
                                                AND IA.PROGRAMME_GROUP_ID = PR.UDF6
                                                AND IA.RECEIVE_ID = PR.UDF1
                                        INNER JOIN
                                        CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = IA.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                        INNER JOIN
                                    SUP_SHIFT AS SH ON SH.SHIFT_ID = PG.SHIFT

                                WHERE
                                    PR.RAZORPAY_PAMENT_ID = ?RAZORPAY_PAMENT_ID
                                GROUP BY SP.RECEIVE_ID , F.FREQUENCY , FMH.MAIN_HEAD_ID;";

                        break;
                    }
                case FeeSQLCommands.FetchTransactionByRazorpayId:
                    {
                        query = @"SELECT 
                                `TRANSACTION_ID`,
                                    `STUDENT_ID`,
                                    `FREQUENCY`,
                                    `CLASS`,
                                    `PAYMENT_DATE`,
                                    `RECEIPT_NO`,
                                    `PAYMENT_MODE`,
                                    `DD_CHEQUE_NO`,
                                    `COLLECTED`,
                                    `DISCOUNT`,
                                    `DEDUCT_STUDENT_ACCOUNT`,
                                    `RECEIPT_NARRATION`,
                                    `USERNAME`,
                                    `ACADEMIC_YEAR`,
                                    `IS_DELETED`,
                                    `IS_AMOUNT_COLLECTED`,
                                    `IS_ACCOUNTANT_COLLECTED`,
                                    `IS_ADVANCE`,
                                    `UPLOAD_FLAG`,
                                    `IS_DOWNLOADED`,
                                    `IS_UPDATED`,
                                    `DOWNLOAD_TIME`,
                                    `FEE_TRANSACTION_COUNTER`,
                                    `FEE_TRANSACTION_BANK`,
                                    `TEMP_ID`,
                                    `EXCESS_AMT`,
                                    `F_TRANSACTION_ID`,
                                    `FREQUENCY_TO`,
                                    `CHALLAN_NO`,
                                    `PayUResponse_Id`,
                                    `RAZORPAY_ID`
                                FROM `FEE_TRANSACTION` WHERE  RAZORPAY_ID=?RAZORPAY_ID AND FREQUENCY=?FREQUENCY AND ACADEMIC_YEAR=?AC_YEAR ;";
                        break;
                    }
                case FeeSQLCommands.FetchRazorpayPaymentInfoById:
                    {
                        query = @"SELECT 
                                    `RAZORPAY_PAMENT_ID`,
                                    `ID`,
                                    `ENTITY`,
                                    `AMOUNT`,
                                    `CURRENCY`,
                                    `STATUS`,
                                    `ORDER_ID`,
                                    `INVOICE_ID`,
                                    `INTERNATIONAL`,
                                    `METHOD`,
                                    `AMOUNT_REFUNDED`,
                                    `REFUND_STATUS`,
                                    `CAPTURED`,
                                    `DESCRIPTION`,
                                    `CARD_ID`,
                                    `BANK`,
                                    `WALLET`,
                                    `VPA`,
                                    `EMAIL`,
                                    `CONTACT`,
                                    `FEE`,
                                    `TAX`,
                                    `ERROR_CODE`,
                                    `ERROR_DESCRIPTION`,
                                    `CREATED_AT`,
                                    `UDF1`,
                                    `UDF2`,
                                    `UDF3`,
                                    `UDF4`,
                                    `UDF5`,
                                    `UDF6`,
                                    `IS_DELETED`
                                FROM
                                    `FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR`
                                WHERE
                                    ID = ?ID AND IS_DELETED!=1";
                        break;
                    }
                case FeeSQLCommands.SaveRazorpayPaymentInfo:
                    {
                        query = @"INSERT INTO `FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR`
                                        (
                                        `ID`,
                                        `ENTITY`,
                                        `AMOUNT`,
                                        `CURRENCY`,
                                        `STATUS`,
                                        `ORDER_ID`,
                                        `INVOICE_ID`,
                                        `INTERNATIONAL`,
                                        `METHOD`,
                                        `AMOUNT_REFUNDED`,
                                        `REFUND_STATUS`,
                                        `CAPTURED`,
                                        `DESCRIPTION`,
                                        `CARD_ID`,
                                        `BANK`,
                                        `WALLET`,
                                        `VPA`,
                                        `EMAIL`,
                                        `CONTACT`,
                                        `FEE`,
                                        `TAX`,
                                        `ERROR_CODE`,
                                        `ERROR_DESCRIPTION`,
                                        `CREATED_AT`,
                                        `UDF1`,
                                        `UDF2`,
                                        `UDF3`,
                                        `UDF4`,
                                        `UDF5`,
                                        `UDF6`
                                        )
                                        VALUES
                                        (
                                        ?ID ,
                                        ?ENTITY ,
                                        ?AMOUNT ,
                                        ?CURRENCY ,
                                        ?STATUS ,
                                        ?ORDER_ID ,
                                        ?INVOICE_ID ,
                                        ?INTERNATIONAL ,
                                        ?METHOD ,
                                        ?AMOUNT_REFUNDED ,
                                        ?REFUND_STATUS ,
                                        ?CAPTURED ,
                                        ?DESCRIPTION ,
                                        ?CARD_ID ,
                                        ?BANK ,
                                        ?WALLET ,
                                        ?VPA ,
                                        ?EMAIL ,
                                        ?CONTACT ,
                                        ?FEE ,
                                        ?TAX ,
                                        ?ERROR_CODE ,
                                        ?ERROR_DESCRIPTION ,
                                        ?CREATED_AT ,
                                        ?UDF1 ,
                                        ?UDF2 ,
                                        ?UDF3 ,
                                        ?UDF4 ,
                                        ?UDF5 ,
                                        ?UDF6  );";
                        break;
                    }
                case FeeSQLCommands.SaveRazorpayOrderInfo:
                    {
                        query = @"INSERT INTO `FEE_RAZORPAY_ORDER_INFO_?AC_YEAR`
                                (`ID`,
                                `ENTITY`,
                                `AMOUNT`,
                                `AMOUNT_PAID`,
                                `AMOUNT_DUE`,
                                `CURRENCY`,
                                `RECEIPT`,
                                `OFFER_ID`,
                                `STATUS`,
                                `ATTEMPTS`,
                                `CREATED_AT`,
                                `UDF1`,
                                `UDF2`,
                                `UDF3`,
                                `UDF4`,
                                `UDF5`,`UDF6`)
                                VALUES
                                (
                                ?ID,
                                ?ENTITY,
                                ?AMOUNT,
                                ?AMOUNT_PAID,
                                ?AMOUNT_DUE,
                                ?CURRENCY,
                                ?RECEIPT,
                                ?OFFER_ID,
                                ?STATUS,
                                ?ATTEMPTS,
                                ?CREATED_AT,
                                ?UDF1,
                                ?UDF2,
                                ?UDF3,
                                ?UDF4,
                                ?UDF5,?UDF6
                                );
";
                        break;
                    }
                case FeeSQLCommands.FetchRazorpayOrderInfoByUDF:
                    {

                        query = @"SELECT 
                                    RAZORPAY_ORDER_ID,
                                    ID,
                                    ENTITY,
                                    AMOUNT,
                                    AMOUNT_PAID,
                                    AMOUNT_DUE,
                                    CURRENCY,
                                    RECEIPT,
                                    OFFER_ID,
                                    STATUS,
                                    ATTEMPTS,
                                    CREATED_AT,
                                    UDF1,
                                    UDF2,
                                    UDF3,
                                    UDF4,
                                    UDF5,
                                    UDF6,
                                    IS_ACTIVE,
                                    IS_DELETED
                                FROM
                                    FEE_RAZORPAY_ORDER_INFO_?AC_YEAR AS OI
                                WHERE
                                    UDF1 = ?UDF1 AND UDF2 = ?UDF2 AND UDF3 = ?UDF3
                                        AND UDF5 = ?UDF5 AND AMOUNT=?AMOUNT AND UDF6=?UDF6
                                ;";
                        break;
                    }
                case FeeSQLCommands.FetchRazorpayAccountInfo:
                    {
                        query = @"SELECT 
                                    RAZORPAY_ACCOUNT_ID,
                                    ACCOUNT_ID,
                                    ACCOUNT_NAME,
                                    ACCOUNT_EMAIL_ID,
                                    ACTIVATED_AT,
                                    CREATED_AT,
                                    BANK_ACCOUNT_ID,
                                    IS_ACTIVE,
                                    IS_DELETED,
                                    RAZORPAY_ACCOUNT_TYPE_ID,
                                    `KEY`,
                                    SECRET_KEY
                                FROM
                                    FEE_RAZORPAY_ACCOUNTS AS RA
                                WHERE
                                    RAZORPAY_ACCOUNT_TYPE_ID = ?RAZORPAY_ACCOUNT_TYPE_ID ";
                        break;
                    }
                case FeeSQLCommands.FecthFrequencyIdByFeeMode:
                    {

                        query = @"SELECT 
                                        FEE_FREQUENCY_FEE_MODE_ID,
                                        FM.FREQUENCY_ID,
                                        FEE_MODE,
                                        FT.FREQUENCY_TYPE,
                                        FR.FREQUENCY_NAME
                                    FROM
                                        FEE_FREQUENCY_FEE_MODE AS FM
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS FR ON FR.FREQUENCY_ID = FM.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FM.FEE_MODE
                                            INNER JOIN
                                        SUP_SEMESTER_TYPE AS SM ON SM.SEMESTER_TYPE_ID = FR.SEMESTER_TYPE
                                    WHERE
                                        FM.FEE_MODE = ?FEE_MODE
                                            AND FM.ACADEMIC_YEAR =?AC_YEAR
                                            AND SM.IS_ACTIVE = 1;";
                        break;
                    }

                case FeeSQLCommands.FetchAdmissionFeeUpdates:
                    {

                        query = @"SET SQL_SAFE_UPDATES=0;
                                    UPDATE ADM_RECEIVE_APPLICATION SET STATUS=5 WHERE RECEIVE_ID=?RECEIVE_ID AND IS_DELETED!=1;
                                    UPDATE ADM_ISSUED_APPLICATION SET STATUS=5 WHERE RECEIVE_ID=?RECEIVE_ID AND IS_DELETED!=1 AND PROGRAMME_GROUP_ID=?PROGRAMME_GROUP_ID;
                                    UPDATE ADM_SELECTION_PROCESS_?AC_YEAR SET IS_FEE_PAID=1 WHERE RECEIVE_ID=?RECEIVE_ID AND (IS_DELETED!=1 AND IS_CANCELED!=1);
                                    SET SQL_SAFE_UPDATES=1;";
                        break;
                    }
                case FeeSQLCommands.HostelFeeUpdates:
                    {

                        query = @"SET SQL_SAFE_UPDATES=0;
                                    UPDATE ADM_HOSTEL_REGISTRATION SET STATUS=5 WHERE RECEIVE_ID=?RECEIVE_ID AND IS_DELETED!=1;                                    
                                    SET SQL_SAFE_UPDATES=1;";
                        break;
                    }

                case FeeSQLCommands.FetchStudentAccountFeeInfoByUserID:
                    {
                        query = @"SELECT 
                                    FM.FREQUENCY_ID,
                                    FS.STUDENT_ID,
                                    SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                                    FR.FREQUENCY_NAME,
                                    CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                            ' ',
                                            IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                    IAPP.EMAIL_ID,
                                    AR.CMOBILENO,
                                    IAPP.APPLICATION_NO,
                                    fs.HEAD
                                FROM
                                    ADM_ISSUE_APPLICATION AS IAPP
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AR.ISSUE_ID = IAPP.ISSUE_ID
                                        INNER JOIN
                                    FEE_STUDENT_ACCOUNT AS FS ON FS.STUDENT_ID = AR.RECEIVE_ID
                                        INNER JOIN
                                 ADM_APPTYPE_PROG_GROUPS AS APG ON apg.PRO_GROUPS_ID = IAPP.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    FEE_MAIN_HEADS AS FM ON FM.FREQUENCY_ID = FS.FREQUENCY_ID
                                        AND FM.HEAD_ID = FS.HEAD
                                        AND APG.SHIFT = FM.SHIFT
                                        AND APG.PROGRAMME_MODE = FM.PROGRAMME_MODE
                                        INNER JOIN
                                    FEE_FREQUENCY_FEE_MODE AS FFM ON FFM.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY_ID
                                        INNER JOIN
                                    SUP_FEE_FREQUENCY AS FR ON FR.FREQUENCY_ID = FFM.FREQUENCY_ID
                                        LEFT JOIN
                                    SUP_SEMESTER_TYPE AS ST ON ST.SEMESTER_TYPE_ID = FR.SEMESTER_TYPE
                                        AND ST.IS_ACTIVE = 1
                                        INNER JOIN
                                    SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FM.MAIN_HEAD_ID
                                        INNER JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = FM.HEAD_ID
                                WHERE
                                    IAPP.ISSUE_ID = ?STUDENT_ID
                                        AND FM.ACADEMIC_YEAR = ?AC_YEAR AND FS.FREQUENCY_ID=?FREQUENCY_ID AND IAPP.IS_DELETED!=1 AND AR.IS_DELETED!=1
                                GROUP BY FS.STUDENT_ID , fs.head;";

                        break;
                    }
                case FeeSQLCommands.FetchStudentAccountFeeInfoByReceiveIdAndFrequencyIdByMainHead:
                    {
                        query = @"SELECT 
                                FS.FREQUENCY_ID,
                                FS.STUDENT_ID,
                                SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                                AR.FIRST_NAME,
                                CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                        'SHIFT- ',
                                        SS.SHIFT_NAME,
                                        ' ( ',
                                        PM.PROGRAMME_MODE,
                                        ' )') AS PROGRAMME_NAME,
                                AR.EMAIL_ID,
                                AI.PROGRAMME_GROUP_ID,
                                AR.SMS_MOBILE_NO,
                                CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                MH.MAIN_HEAD,
                                SH.HEAD,
                                MH.MAIN_HEAD_ID,
                                FMH.FEE_MAIN_HEAD_ID
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                    AND AR.IS_DELETED != 1
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                                    AND AI.IS_DELETED != 1
                                    INNER JOIN
                                FEE_STUDENT_ACCOUNT AS FS ON FS.STUDENT_ID = AR.RECEIVE_ID
                                    AND FS.IS_DELETED != 1
                                    AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                    AND FMH.IS_DELETED != 1
                                    INNER JOIN
                                SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                    AND MH.IS_DELETED != 1
                                    INNER JOIN
                                SUP_HEAD AS SH ON SH.HEAD_ID = FMH.HEAD_ID
                                    AND SH.IS_DELETED != 1
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    AND PG.IS_DELETED != 1
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    AND PG.IS_DELETED != 1
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                            WHERE
                                FS.STUDENT_ID = ?STUDENT_ID
                                    AND SL.IS_DELETED != 1 AND IS_CANCELED!=1)
                                    AND FS.FREQUENCY_ID = ?FREQUENCY_ID
                            GROUP BY FS.STUDENT_ID , FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchStudentAccountFeeInfoByReceiveIdAndFrequencyId:
                    {
                        query = @"SELECT 
                                FS.FREQUENCY_ID,
                                FS.STUDENT_ID,
                                SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                                CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                        ' ',
                                        IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                        'SHIFT- ',
                                        SS.SHIFT_NAME,
                                        ' ( ',
                                        PM.PROGRAMME_MODE,
                                        ' )') AS PROGRAMME_NAME,
                                AR.EMAIL_ID,
                                AI.PROGRAMME_GROUP_ID,
                                AR.SMS_MOBILE_NO,
                                CONCAT(AI.APPLICATION_NO, LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                MH.MAIN_HEAD,
                                SH.HEAD,
                                MH.MAIN_HEAD_ID,
                                FMH.FEE_MAIN_HEAD_ID
                            FROM
                               ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                    AND AR.IS_DELETED != 1
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                                    AND AI.IS_DELETED != 1
                                    INNER JOIN
                                FEE_STUDENT_ACCOUNT AS FS ON FS.STUDENT_ID = AR.RECEIVE_ID
                                    AND FS.IS_DELETED != 1
                                    AND FS.ACADEMIC_YEAR = ?AC_YEAR                              
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                    AND FMH.IS_DELETED != 1
                                    INNER JOIN
                                SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                    AND MH.IS_DELETED != 1
                                    INNER JOIN
                                SUP_HEAD AS SH ON SH.HEAD_ID = FMH.HEAD_ID
                                    AND SH.IS_DELETED != 1
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    AND PG.IS_DELETED != 1
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    AND PG.IS_DELETED != 1
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                            WHERE
                                FS.STUDENT_ID = ?STUDENT_ID AND (SL.IS_DELETED!=1 AND IS_CANCELED!=1)
                                    AND FS.FREQUENCY_ID = ?FREQUENCY_ID
                            GROUP BY FS.STUDENT_ID , FS.HEAD;";
                        break;
                    }
                case FeeSQLCommands.FetchStudentAccountFeeInfo:
                    {

                        query = @"SELECT 
	                                CONCAT(ADI.APPLICATION_NO,LPAD(ADI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    FMH.FREQUENCY_ID,
                                    FS.STUDENT_ID,
                                    SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                                    FF.FREQUENCY_NAME,
                                    ADR.FIRST_NAME,
                                    SFM.MAIN_HEAD AS HEAD,
                                    ADR.SMS_MOBILE_NO,
                                    FT.FREQUENCY_TYPE AS FREQUENCY_NAME,SP.PROGRAMME_ID
                                FROM
                                    FEE_STUDENT_ACCOUNT AS FS
                                        INNER JOIN
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SP ON SP.RECEIVE_ID = FS.STUDENT_ID
                                        AND (SP.IS_DELETED != 1 AND IS_CANCELED!=1)
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = SP.RECEIVE_ID
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS ADI ON ADI.RECEIVE_ID = ADR.RECEIVE_ID
                                        AND ADI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                        AND PG.IS_DELETED != 1
                                        INNER JOIN
                                    FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID=FS.FEE_MAIN_HEAD_ID
                                        AND FMH.IS_DELETED != 1
                                        INNER JOIN
                                    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                        AND FFF.ACADEMIC_YEAR =?AC_YEAR
                                        AND FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY_ID
                                        INNER JOIN
                                    SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                        AND FT.FEE_ROOT_ID = 1
                                        INNER JOIN
                                    SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                        LEFT JOIN
                                    SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        LEFT JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                WHERE
                                         FS.STUDENT_ID =?STUDENT_ID
                                        AND FT.FREQUENCY_TYPE_ID =?FREQUENCY_ID
                                        AND FS.ACADEMIC_YEAR =?AC_YEAR
                                        AND FS.IS_DELETED != 1
                                GROUP BY FS.STUDENT_ID , FS.FREQUENCY_ID , FMH.MAIN_HEAD_ID;";
                        //query = @"SELECT 
                        //            FMH.FREQUENCY_ID,
                        //            FS.STUDENT_ID,
                        //            SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                        //            FF.FREQUENCY_NAME,
                        //            CONCAT(IFNULL(SP.FIRST_NAME, ''),
                        //                    ' ',
                        //                    IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                        //            SFM.MAIN_HEAD AS HEAD,
                        //            SP.STU_EMAILID,
                        //            SP.STU_MOBILENO,
                        //            SP.ROLL_NO,
                        //            SP.REGISTER_NO,FT.FREQUENCY_TYPE as FREQUENCY_NAME
                        //        FROM
                        //            FEE_STUDENT_ACCOUNT AS FS
                        //                INNER JOIN
                        //            STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = FS.STUDENT_ID
                        //                AND SP.IS_DELETED != 1
                        //                INNER JOIN
                        //            CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SP.PROGRAM_ID
                        //                AND PG.IS_DELETED != 1
                        //                INNER JOIN
                        //            FEE_MAIN_HEADS AS FMH ON FMH.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                        //                AND FMH.PROGRAMME_MODE = PG.PROGRAMME_MODE
                        //                AND FMH.SHIFT = PG.SHIFT
                        //                AND FS.HEAD = FMH.HEAD_ID
                        //                AND FMH.ACADEMIC_YEAR = ?AC_YEAR
                        //                AND FMH.IS_DELETED != 1
                        //                INNER JOIN
                        //            FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                        //                AND FFF.ACADEMIC_YEAR = ?AC_YEAR
                        //                AND FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY_ID
                        //                INNER JOIN
                        //            SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                        //                AND FT.FEE_ROOT_ID = ?FEE_ROOT_ID
                        //                INNER JOIN
                        //            SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                        //                LEFT JOIN
                        //            SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                        //                LEFT JOIN
                        //            SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                        //        WHERE
                        //            FS.STUDENT_ID = ?STUDENT_ID
                        //                AND FT.FREQUENCY_TYPE_ID = ?FREQUENCY_ID
                        //                AND FS.ACADEMIC_YEAR = ?AC_YEAR
                        //                AND FS.IS_DELETED != 1
                        //        GROUP BY FS.STUDENT_ID , FS.FREQUENCY_ID, FMH.MAIN_HEAD_ID
                        //        ;";
                        break;
                    }


                case FeeSQLCommands.FetchStudentAccountFeeInfoSMC:
                    {

                        query = @"SELECT 
	                                CONCAT(ADI.APPLICATION_NO,LPAD(ADI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    FMH.FREQUENCY_ID,
                                    FS.STUDENT_ID,
                                    SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                                    FF.FREQUENCY_NAME,
                                    ADR.FIRST_NAME,
                                    SFM.MAIN_HEAD AS HEAD,
                                    ADR.SMS_MOBILE_NO,
                                    FT.FREQUENCY_TYPE AS FREQUENCY_NAME,SP.PROGRAMME_ID
                                FROM
                                    FEE_STUDENT_ACCOUNT AS FS
                                        INNER JOIN
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SP ON SP.RECEIVE_ID = FS.STUDENT_ID
                                        AND (SP.IS_DELETED != 1 AND IS_CANCELED!=1)
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = SP.RECEIVE_ID
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS ADI ON ADI.RECEIVE_ID = ADR.RECEIVE_ID
                                        AND ADI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID AND ADI.IS_DELETED!=1
                                    INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                        AND PG.IS_DELETED != 1 and PG.PROGRAMME_MODE=?PROGRAMME_MODE
                                        INNER JOIN
                                    FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID=FS.FEE_MAIN_HEAD_ID
                                        AND FMH.IS_DELETED != 1
                                        INNER JOIN
                                    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                        AND FFF.ACADEMIC_YEAR =?AC_YEAR
                                        AND FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY_ID
                                        INNER JOIN
                                    SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                        AND FT.FEE_ROOT_ID = 1
                                        INNER JOIN
                                    SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                        LEFT JOIN
                                    SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        LEFT JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                WHERE
                                         FS.STUDENT_ID =?STUDENT_ID
                                        AND FT.FREQUENCY_TYPE_ID =?FREQUENCY_ID
                                        AND FS.ACADEMIC_YEAR =?AC_YEAR
                                        AND FS.IS_DELETED != 1
                                GROUP BY FS.STUDENT_ID , FS.FREQUENCY_ID , FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchStudentAccountFeeInfoByReceiveId:
                    {
                        query = @"SELECT 
                                FM.FREQUENCY_ID,
                                FS.STUDENT_ID,
                                SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                                FR.FREQUENCY_NAME,
                                CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                        ' ',
                                        IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                IAPP.EMAIL_ID,
                                AR.CMOBILENO,
                                IAPP.APPLICATION_NO,
                                SFM.MAIN_HEAD AS HEAD,AR.STATUS,AR.RECEIVE_ID
                            FROM
                                    ADM_ISSUE_APPLICATION AS IAPP
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.ISSUE_ID = IAPP.ISSUE_ID
                                    INNER JOIN
                                FEE_STUDENT_ACCOUNT AS FS ON FS.STUDENT_ID = AR.RECEIVE_ID
                                   INNER JOIN
                                 ADM_APPTYPE_PROG_GROUPS AS APG ON apg.PRO_GROUPS_ID = IAPP.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    FEE_MAIN_HEADS AS FM ON FM.FREQUENCY_ID = FS.FREQUENCY_ID
                                        AND FM.HEAD_ID = FS.HEAD
                                        AND APG.SHIFT = FM.SHIFT
                                        AND APG.PROGRAMME_MODE = FM.PROGRAMME_MODE
                                    INNER JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFM ON FFM.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY_ID
                                    INNER JOIN
                                SUP_FEE_FREQUENCY AS FR ON FR.FREQUENCY_ID = FFM.FREQUENCY_ID
                                    INNER JOIN
                                SUP_SEMESTER_TYPE AS ST ON ST.SEMESTER_TYPE_ID = FR.SEMESTER_TYPE
                                    AND ST.IS_ACTIVE = 1
                                    LEFT JOIN
                                SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FM.MAIN_HEAD_ID
                                    LEFT JOIN
                                SUP_HEAD AS H ON H.HEAD_ID = FM.HEAD_ID
                            WHERE
                                AR.RECEIVE_ID = ?STUDENT_ID
                                    AND FM.ACADEMIC_YEAR = ?AC_YEAR AND IAPP.IS_DELETED!=1 AND AR.IS_DELETED!=1 AND FS.FREQUENCY_ID=?FREQUENCY_ID AND FS.IS_DELETED!=1
                            GROUP BY FS.STUDENT_ID , FM.MAIN_HEAD_ID;";
                        break;
                    }

                case FeeSQLCommands.FetchRefundList:
                    {
                        query = @"SELECT 
                                        PAYU_RESPONSE_ID,
                                        txnid,
                                        udf3,
                                        udf4,
                                        amount,
                                        productinfo,
                                        firstname,
                                        email,
                                        phone,
                                        bank_ref_num,
                                        bankcode,
                                        error,
                                        error_Message,
                                        name_on_card,
                                        mihpayid,
                                        de.status,
                                        unmappedstatus,
                                        addedon,fr.FREQUENCY_NAME as udf2
                                    FROM
                                        fee_payu_response_double_entry as de inner join sup_fee_frequency as fr on fr.FREQUENCY_ID = de.udf2; ";
                        break;
                    }
                case FeeSQLCommands.UpdatePayUResponse:
                    {
                        query = @"UPDATE fee_payu_response_?AC_YEAR
                                                        SET
                                                        `key` = ?KEY,
                                                        txnid = ?TXNID,
                                                        amount = ?AMOUNT,
                                                        productinfo = ?PRODUCTINFO,
                                                        firstname = ?FIRSTNAME,
                                                        email = ?EMAIL,
                                                        phone = ?PHONE,
                                                        lastname = ?LASTNAME,
                                                        address1 = ?ADDRESS1,
                                                        address2 = ?ADDRESS2,
                                                        city = ?CITY,
                                                        state = ?STATE,
                                                        country = ?COUNTRY,
                                                        zipcode = ?ZIPCODE,
                                                        udf1 = ?UDF1,
                                                        udf2 = ?UDF2,
                                                        udf3 = ?UDF3,
                                                        udf4 = ?UDF4,
                                                        udf5 = ?UDF5,
                                                        udf6 = ?UDF6,
                                                        udf7 = ?UDF7,
                                                        udf8 = ?UDF8,
                                                        udf9 = ?UDF9,
                                                        udf10 = ?UDF10,
                                                        hash = ?HASH,
                                                        payment_source = ?PAYMENT_SOURCE,
                                                        PG_TYPE = ?PG_TYPE,
                                                        bank_ref_num = ?BANK_REF_NUM,
                                                        bankcode = ?BANKCODE,
                                                        error = ?ERROR,
                                                        error_Message = ?ERROR_MESSAGE,
                                                        name_on_card = ?NAME_ON_CARD,
                                                        cardnum = ?CARDNUM,
                                                        mode = ?MODE,
                                                        mihpayid = ?MIHPAYID,
                                                        status = ?STATUS,
                                                        unmappedstatus = ?UNMAPPEDSTATUS,
                                                        addedon = ?ADDEDON,
                                                        additionalCharges = ?ADDITIONALCHARGES
                                                        WHERE PAYU_RESPONSE_ID =?PAYU_RESPONSE_ID;";
                        break;
                    }
                case FeeSQLCommands.SavePayURequest:
                    {
                        query = @"INSERT INTO FEE_PAYU_REQUEST_?AC_YEAR
                                    (
                                    `KEY`,
                                    TXNID,
                                    AMOUNT,
                                    PRODUCTINFO,
                                    FIRSTNAME,
                                    EMAIL,
                                    PHONE,
                                    LASTNAME,
                                    ADDRESS1,
                                    ADDRESS2,
                                    CITY,
                                    STATE,
                                    COUNTRY,
                                    ZIPCODE,
                                    UDF1,
                                    UDF2,
                                    UDF3,
                                    UDF4,
                                    UDF5,
                                    PG,HASH)
                                    VALUES
                                    (
                                    ?KEY,
                                    ?TXNID,
                                    ?AMOUNT,
                                    ?PRODUCTINFO,
                                    ?FIRSTNAME,
                                    ?EMAIL,
                                    ?PHONE,
                                    ?LASTNAME,
                                    ?ADDRESS1,
                                    ?ADDRESS2,
                                    ?CITY,
                                    ?STATE,
                                    ?COUNTRY,
                                    ?ZIPCODE,
                                    ?UDF1,
                                    ?UDF2,
                                    ?UDF3,
                                    ?UDF4,
                                    ?UDF5,
                                    ?PG,?HASH);";
                        break;
                    }
                case FeeSQLCommands.IsPayURequestExist:
                    {
                        query = @"SELECT 
                                   txnid
                                FROM
                                    FEE_PAYU_REQUEST_?AC_YEAR AS FP
                                WHERE
                                     TXNID = ?TXNID;";
                        break;
                    }
                case FeeSQLCommands.IsPayUResponseExist:
                    {
                        query = @"SELECT 
                                   PAYU_RESPONSE_ID, mihpayid,txnid
                                FROM
                                    fee_payu_response_?AC_YEAR AS FP
                                WHERE					
                                         TXNID = ?TXNID;";
                        break;
                    }
                case FeeSQLCommands.SavePayUResponse:
                    {
                        query = @"INSERT INTO FEE_PAYU_RESPONSE_?AC_YEAR
                                    (
                                    `KEY`,
                                    TXNID,
                                    AMOUNT,
                                    PRODUCTINFO,
                                    FIRSTNAME,
                                    EMAIL,
                                    PHONE,
                                    LASTNAME,
                                    ADDRESS1,
                                    ADDRESS2,
                                    CITY,
                                    STATE,
                                    COUNTRY,
                                    ZIPCODE,
                                    UDF1,
                                    UDF2,
                                    UDF3,
                                    UDF4,
                                    UDF5,
                                    UDF6,
                                    UDF7,
                                    UDF8,
                                    UDF9,
                                    UDF10,
                                    `HASH`,
                                    PAYMENT_SOURCE,
                                    PG_TYPE,
                                    BANK_REF_NUM,
                                    BANKCODE,
                                    `ERROR`,
                                    ERROR_MESSAGE,
                                    NAME_ON_CARD,
                                    CARDNUM,
                                    `MODE`,
                                    MIHPAYID,
                                    `STATUS`,
                                    UNMAPPEDSTATUS,ADDEDON,ADDITIONALCHARGES)
                                    VALUES
                                    (
                                    ?KEY,
                                    ?TXNID,
                                    ?AMOUNT,
                                    ?PRODUCTINFO,
                                    ?FIRSTNAME,
                                    ?EMAIL,
                                    ?PHONE,
                                    ?LASTNAME,
                                    ?ADDRESS1,
                                    ?ADDRESS2,
                                    ?CITY,
                                    ?STATE,
                                    ?COUNTRY,
                                    ?ZIPCODE,
                                    ?UDF1,
                                    ?UDF2,
                                    ?UDF3,
                                    ?UDF4,
                                    ?UDF5,
                                    ?UDF6,
                                    ?UDF7,
                                    ?UDF8,
                                    ?UDF9,
                                    ?UDF10,
                                    ?HASH,
                                    ?PAYMENT_SOURCE,
                                    ?PG_TYPE,
                                    ?BANK_REF_NUM,
                                    ?BANKCODE,
                                    ?ERROR,
                                    ?ERROR_MESSAGE,
                                    ?NAME_ON_CARD,
                                    ?CARDNUM,
                                    ?MODE,
                                    ?MIHPAYID,
                                    ?STATUS,
                                    ?UNMAPPEDSTATUS,?ADDEDON,?ADDITIONALCHARGES);
";
                        break;
                    }
                case FeeSQLCommands.FetchPayUPaymentAccountInfo:
                    {
                        query = @"SELECT 
                                    MERCHANT_ACCOUNT_INFO,
                                    ACCOUNT_ID,
                                    USERNAME,
                                    `PASSWORD`,
                                    `KEY`,
                                    SALT,
                                    BASE_URL,
                                    AUTHORIZATION,
                                    API_TYPE,
                                    HASH_SEQUENCE,
                                    BANK_ACCOUNT_ID,
                                    SP.REGISTER_NO,
                                    SP.ROLL_NO,
                                    SP.STUDENT_ID,
                                    CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                            IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                    SP.STU_EMAILID,
                                    SP.STU_MOBILENO,surl,furl,curl
                                FROM
                                    FEE_MERCHANT_ACCOUNT_INFO AS MI
                                        INNER JOIN
                                    SUP_BANK_ACCOUNT AS BA ON BA.BANK_ACCOUNT_ID = MI.ACCOUNT_ID
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = ?STUDENT_ID
                                        AND SP.PROGRAMME_MODE = BA.PROGRAMME_MODE and sp.IS_DELETED!=1
                                WHERE
                                    BA.ACCOUNT_TYPE = ?ACCOUNT_TYPE
                                        AND MI.IS_DELETED != 1
                                        AND BA.IS_DELETED != 1
                                        AND MI.API_TYPE = ?API_TYPE;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeSummaryByStudentId:
                    {
                        query = @"SELECT 
                                    (SUM(IFNULL(CREDIT, 0)) - SUM(IFNULL(DEBIT, 0))) AS BALANCE,
                                    SUM(IFNULL(CREDIT, 0)) AS CREDIT,
                                    SUM(IFNULL(DEBIT, 0)) AS DEBIT,
                                    FREQUENCY_ID,
                                    TRANSACTION_DATE,
                                    FT.TRANSACTION_ID,
                                    H.HEAD,
                                    CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                            ' ',
                                            IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                    CLS.CLASS_CODE
                                FROM
                                    FEE_STUDENT_ACCOUNT AS SA
                                        LEFT JOIN
                                    FEE_TRANSACTION AS FT ON FT.TRANSACTION_ID = SA.TRANSACTION_ID
                                        INNER JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = SA.HEAD
                                        INNER JOIN
                                    STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SA.STUDENT_ID
                                        INNER JOIN
                                    STU_CLASS AS SCLS ON SCLS.STUDENT_ID = SP.STUDENT_ID
                                        AND SCLS.ACADEMIC_YEAR = ?AC_YEAR
                                        INNER JOIN
                                    CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = SCLS.CLASS_ID
                                WHERE
                                    FREQUENCY_ID = ?FREQUENCY_ID
                                        AND SA.ACADEMIC_YEAR = ?AC_YEAR
                                        AND SA.IS_DELETED != 1
                                        AND SA.STUDENT_ID = ?STUDENT_ID
                                GROUP BY HEAD;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeMainHeads:
                    {
                        query = @"SELECT 
                                FEE_MAIN_HEAD_ID,
                                FH.MAIN_HEAD_ID,
                                AP.APPLICATION_TYPE,
                                FR.FEE_ROOT,
                                FH.HEAD_ID,
                                FH.ACADEMIC_YEAR,
                                SFH.MAIN_HEAD,
                                SH.HEAD,
                                CONCAT(IFNULL(SFF.FREQUENCY_NAME,''),'- ',IFNULL(SFT.FREQUENCY_TYPE,'')) AS FREQUENCY_NAME,
                                SF.SHIFT_NAME,
                                SPM.PROGRAMME_MODE,
                                CONCAT(IFNULL(B.PASSBOOK_NO,''),'-',IFNULL(BA.BANK_NAME,''))AS PASSBOOK_NO,
                                FCAT.FEE_CATEGORY
                            FROM
                                FEE_MAIN_HEADS AS FH
                                    INNER JOIN
                                SUP_FEE_MAIN_HEAD AS SFH ON SFH.MAIN_HEAD_ID = FH.MAIN_HEAD_ID
                                    AND SFH.IS_DELETED != 1
                                    INNER JOIN
                                SUP_HEAD AS SH ON SH.HEAD_ID = FH.HEAD_ID
                                    AND SH.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_SHIFT AS SF ON SF.SHIFT_ID = FH.SHIFT
                                    AND SF.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = FH.PROGRAMME_MODE
                                    AND SPM.IS_DELETED != 1
                                    LEFT JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FH.FREQUENCY_ID
                                    LEFT JOIN
                                SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    AND SFF.IS_DELETED != 1
                                    LEFT JOIN
                                FEE_ROOT AS FR ON FR.FEE_ROOT_ID = FH.FEE_ROOT_ID
                                    AND FR.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_APPLICATION_TYPE AS AP ON AP.APPLICATION_TYPE_ID = FH.APPLICATION_TYPE_ID
                                    AND AP.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_BANK_ACCOUNT AS B ON B.BANK_ACCOUNT_ID = FH.BANK_ACCOUNT_ID
                                    AND B.IS_DELETED != 1
                                    LEFT JOIN SUP_BANK AS BA ON BA.BANK_ID=B.BANK AND BA.IS_DELETED!=1
                                    LEFT JOIN
                                SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                    AND SFT.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_FEE_CATEGORY AS FCAT ON FCAT.FEE_CATEGORY_ID = FH.FEE_CATEGORY_ID
                                    AND FCAT.IS_DELETED != 1
                            WHERE
                                FH.IS_DELETED != 1
                                    AND FH.FEE_ROOT_ID = ?FEE_ROOT_ID
                                    AND FH.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.SaveFeeMainHead:
                    {
                        query = @"INSERT INTO FEE_MAIN_HEADS
                                (
                                FREQUENCY_ID,
                                APPLICATION_TYPE_ID,                              
                                MAIN_HEAD_ID,
                                HEAD_ID,
                                PROGRAMME_MODE,
                                SHIFT,
                                FEE_CATEGORY_ID,
                                ACADEMIC_YEAR,
                                FEE_ROOT_ID,               
                                BANK_ACCOUNT_ID                                
                                )
                                VALUES
                                (
                                ?FREQUENCY_ID,
                                ?APPLICATION_TYPE_ID,                              
                                ?MAIN_HEAD_ID,
                                ?HEAD_ID,
                                ?PROGRAMME_MODE,
                                ?SHIFT,
                                ?FEE_CATEGORY_ID,
                                ?ACADEMIC_YEAR,
                                ?FEE_ROOT_ID,               
                                ?BANK_ACCOUNT_ID
                                );";
                        break;
                    }
                case FeeSQLCommands.UpdateFeeMainHead:
                    {
                        query = @"UPDATE FEE_MAIN_HEADS
                                    SET
                                    FEE_CATEGORY_ID = ?FEE_CATEGORY_ID,
                                    BANK_ACCOUNT_ID = ?BANK_ACCOUNT_ID
                                    WHERE FEE_MAIN_HEAD_ID = ?FEE_MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeMainHead:
                    {
                        query = @"UPDATE FEE_MAIN_HEADS
                                SET
                                IS_DELETED=1
                                WHERE FEE_MAIN_HEAD_ID = ?FEE_MAIN_HEAD_ID;";
                        break;

                    }
                case FeeSQLCommands.FetchSubHeadsByMainHeadId:
                    {
                        query = @"SELECT 
                                        FEE_MAIN_HEAD_ID,
                                        FH.MAIN_HEAD_ID,
                                        SH.HEAD_ID,
                                        SH.HEAD,
                                        IF(FEE_MAIN_HEAD_ID IS NOT NULL,
                                            'SELECTED',
                                            '') AS STATUS
                                    FROM
                                        SUP_HEAD AS SH
                                            LEFT JOIN
                                        FEE_MAIN_HEADS AS FH ON SH.HEAD_ID = FH.HEAD_ID
                                            AND FH.IS_DELETED != 1
                                            AND FH.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FH.MAIN_HEAD_ID = ?MAIN_HEAD_ID
                                            AND FH.SHIFT = ?SHIFT
                                            AND FH.PROGRAMME_MODE = ?PROGRAMME_MODE
                                            AND FH.FREQUENCY_ID = ?FREQUENCY_ID
                                            AND FH.FEE_ROOT_ID = ?FEE_ROOT_ID
                                            AND FH.APPLICATION_TYPE_ID = ?APPLICATION_TYPE_ID
                                    WHERE
                                        SH.IS_DELETED != 1;";
                        break;

                    }
                case FeeSQLCommands.FetchMainHeadsByMainHeadIdAndFreId:
                    {
                        query = @"SELECT 
                                SH.HEAD_ID,
                                FH.MAIN_HEAD_ID,
                                FH.FREQUENCY_ID,
                                HEAD,                               
                                IF(FH.HEAD_ID IS NOT NULL,
                                    'SELECTED',
                                    '') AS STATUS
                            FROM
                                SUP_HEAD AS SH
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FH ON FH.HEAD_ID = SH.HEAD_ID AND  FH.FREQUENCY_ID=?FREQUENCY_ID AND FH.MAIN_HEAD_ID!=?MAIN_HEAD_ID AND FH.IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.FetchMainHeadsByFreId:
                    {
                        query = @"SELECT 
	                            FH.FREQUENCY_ID,
                                SH.HEAD_ID,
                                HEAD,
                                FH.MAIN_HEAD_ID,
                                IF(FH.HEAD_ID IS NOT NULL,
                                    'SELECTED',
                                    '') AS STATUS
                            FROM
                                SUP_HEAD AS SH
                                    LEFT JOIN
                                FEE_MAIN_HEADS AS FH ON FH.HEAD_ID = SH.HEAD_ID AND FH.FREQUENCY_ID=?FREQUENCY_ID AND FH.IS_DELETED!=1 AND SH.IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeMainHeadByMainHeadIdAndHeadId:
                    {
                        query = @"SET SQL_SAFE_UPDATES=0;
                                UPDATE FEE_MAIN_HEADS
                                SET
                                IS_DELETED=1
                                WHERE SHIFT=?SHIFT AND PROGRAMME_MODE=?PROGRAMME_MODE AND FREQUENCY_ID=?FREQUENCY_ID AND ACADEMIC_YEAR = ?ACADEMIC_YEAR AND MAIN_HEAD_ID=?MAIN_HEAD_ID AND HEAD_ID=?HEAD_ID;
                                SET SQL_SAFE_UPDATES=1;";
                        break;

                    }
                case FeeSQLCommands.FetchFeeStudentAccountByStudentIdAndFrequencyId:
                    {
                        query = @"SELECT 
                                FSA.STUDENT_ID,
                                FSA.FREQUENCY_ID,
                                SFM.HEAD_ID,
                                SFM.HEAD,
                                FSA.CREDIT,
                                FSA.DEBIT,
                                (IFNULL(SUM(FSA.CREDIT), 0) - IFNULL(SUM(FSA.DEBIT), 0)) AS BALANCE,
                                FSA.FEE_MAIN_HEAD_ID,
                                FS.FEE_STRUCTURE_ID,
                                FS.FEE_ROOT_ID
                            FROM
                                FEE_STUDENT_ACCOUNT AS FSA
                                    INNER JOIN
                                FEE_STRUCTURE AS FS ON FS.FEE_STRUCTURE_ID = FSA.FEE_STRUCTURE_ID
                                    AND FS.PROGRAMME = ?PROGRAMME
                                    AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FS.FREQUENCY = ?FREQUENCY_ID
                                    INNER JOIN
                                SUP_HEAD AS SFM ON SFM.HEAD_ID = FSA.HEAD
                            WHERE
                                FSA.STUDENT_ID = ?STUDENT_ID
                                    AND FSA.IS_DELETED != 1
                                    AND FSA.IS_CANCELLED_HEAD != 1
                                    AND FSA.IS_REFUND != 1
                                    AND SFM.IS_DELETED != 1
                                    AND FSA.FREQUENCY_ID = ?FREQUENCY_ID
                                    AND FSA.ACADEMIC_YEAR = ?AC_YEAR
                            GROUP BY FSA.HEAD
                            ORDER BY FSA.F_STUDENT_AC_ID ASC;";
                        break;
                    }
                case FeeSQLCommands.FetchCreditByStudentId:
                    {
                        query = @"SELECT 
                                        IFNULL(SUM(CREDIT), 0) CREDIT,
                                        IFNULL(SUM(DEBIT), 0) DEBIT,
                                        (IFNULL(SUM(CREDIT), 0) - IFNULL(SUM(DEBIT), 0)) AS BALANCE,
                                        FREQUENCY_ID
                                    FROM
                                        FEE_STUDENT_ACCOUNT
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID AND IS_DELETED != 1
                                            AND FREQUENCY_ID = ?FREQUENCY_ID
                                            AND ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.SaveFeeTransaction:
                    {
                        query = @"INSERT INTO FEE_TRANSACTION
                                        (
                                        STUDENT_ID,
                                        FREQUENCY,
                                        CLASS,
                                        PAYMENT_DATE,
                                        RECEIPT_NO,
                                        PAYMENT_MODE,
                                        COLLECTED,
                                        USERNAME,
                                        ACADEMIC_YEAR,
                                        FREQUENCY_TO,PAYURESPONSE_ID,RAZORPAY_ID)
                                        VALUES
                                        (
                                        ?STUDENT_ID,
                                        ?FREQUENCY,
                                        ?CLASS,
                                        ?PAYMENT_DATE,
                                        ?RECEIPT_NO,
                                        ?PAYMENT_MODE,
                                        ?COLLECTED,
                                        ?USERNAME,
                                        ?AC_YEAR,
                                        ?FREQUENCY_TO,?PAYURESPONSE_ID,?RAZORPAY_ID);
                                        ";
                        break;
                    }
                case FeeSQLCommands.SaveOfflineFeeTransaction:
                    {
                        query = @"INSERT INTO FEE_TRANSACTION
                                        (
                                        STUDENT_ID,
                                        FREQUENCY,
                                        CLASS,
                                        PAYMENT_DATE,
                                        RECEIPT_NO,
                                        PAYMENT_MODE,
                                        COLLECTED,
                                        USERNAME,
                                        ACADEMIC_YEAR,
                                        FREQUENCY_TO)
                                        VALUES
                                        (
                                        ?STUDENT_ID,
                                        ?FREQUENCY,
                                        ?CLASS,
                                        ?PAYMENT_DATE,
                                        ?RECEIPT_NO,
                                        ?PAYMENT_MODE,
                                        ?COLLECTED,
                                        ?USERNAME,
                                        ?AC_YEAR,
                                        ?FREQUENCY_TO);
                                        ";
                        break;
                    }
                case FeeSQLCommands.FetchAutoGenerateNumber:
                    {
                        query = @"SELECT 
                                        auto_generate_numbers_id, exam_receipt_no
                                    FROM
                                        AUTO_GENERATE_NUMBERS WHERE AUTO_GENERATE_NUMBERS_ID=1;";
                        break;
                    }
                case FeeSQLCommands.UpdateAutoGenerateNumber:
                    {
                        query = @"UPDATE AUTO_GENERATE_NUMBERS SET
                                        EXAM_RECEIPT_NO=?EXAM_RECEIPT_NO WHERE AUTO_GENERATE_NUMBERS_ID=1;";
                        break;
                    }
                case FeeSQLCommands.SaveFeeCollection:
                    {
                        query = @"INSERT INTO FEE_COLLECTION
                                        (
                                        TRANSACTION_ID,
                                        HEAD,
                                        PAID_AMOUNT,
                                        FREQUENCY,
                                        RECEIPT_NO,FEE_MAIN_HEAD_ID)
                                        VALUES
                                        (
                                        ?TRANSACTION_ID,
                                        ?HEAD,
                                        ?PAID_AMOUNT,
                                        ?FREQUENCY,
                                        ?RECEIPT_NO,?FEE_MAIN_HEAD_ID);";
                        break;
                    }
                case FeeSQLCommands.SaveFeeStudentAccount:
                    {
                        query = @"INSERT INTO FEE_STUDENT_ACCOUNT
                                        (
                                        STUDENT_ID,
                                        ACADEMIC_YEAR,
                                        FREQUENCY_ID,
                                        HEAD,
                                        DEBIT,
                                        TRANSACTION_DATE,
                                        TRANSACTION_ID,FEE_MAIN_HEAD_ID,FEE_STRUCTURE_ID,FEE_ROOT_ID)
                                        VALUES
                                        (
                                        ?STUDENT_ID,
                                        ?AC_YEAR,
                                        ?FREQUENCY_ID,
                                        ?HEAD,
                                        ?DEBIT,
                                        ?TRANSACTION_DATE,
                                        ?TRANSACTION_ID,?FEE_MAIN_HEAD_ID,?FEE_STRUCTURE_ID,?FEE_ROOT_ID)";
                        break;
                    }
                case FeeSQLCommands.FetchFeeStudentAccountByTransactionId:
                    {
                        query = @"SELECT 
                                        FSA.STUDENT_ID,SFM.HEAD, FSA.TRANSACTION_ID, FSA.DEBIT
                                    FROM
                                        FEE_STUDENT_ACCOUNT AS FSA
                                            INNER JOIN
                                        SUP_HEAD AS SFM ON SFM.HEAD_ID = FSA.HEAD
                                    WHERE
                                        TRANSACTION_ID = ?TRANSACTION_ID
                                            AND FSA.IS_DELETED != 1
                                            AND SFM.IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.FetchTotalDebitByTransactionId:
                    {
                        query = @"SELECT 
                                        FT.RECEIPT_NO, FT.COLLECTED,DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,SPM.PAYMENT_MODE,FT.STUDENT_ID
                                    FROM
                                        FEE_TRANSACTION AS FT INNER JOIN SUP_PAYMENT_MODE AS SPM ON SPM.PAYMENT_MODE_ID=FT.PAYMENT_MODE
                                    WHERE
                                        FT.TRANSACTION_ID = ?TRANSACTION_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchClassWiseFeeDetails:
                    {
                        query = @"SELECT
                                        SP.STUDENT_ID, 
                                        SP.ROLL_NO,
                                        SP.REGISTER_NO,
                                        (CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                IFNULL(SP.LAST_NAME, ''))) AS FIRST_NAME,
                                        SUM(IFNULL(FSAC.CREDIT, 0)) AS TOTAL_AMOUNT,
                                        SUM(IFNULL(FSAC.DEBIT, 0)) AS COLLECTED,
                                        (SUM(IFNULL(FSAC.CREDIT, 0)) - SUM(IFNULL(FSAC.DEBIT, 0))) AS BALANCE,
                                        CC.CLASS_CODE AS CLASS,
                                        SPM.PAYMENT_MODE
                                    FROM
                                        STU_CLASS AS CLS
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = CLS.STUDENT_ID
                                            LEFT JOIN
                                        FEE_STUDENT_ACCOUNT AS FSAC ON SP.STUDENT_ID = FSAC.STUDENT_ID
                                            AND FSAC.FREQUENCY_ID = ?FREQUENCY
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CC ON CC.CLASS_ID = CLS.CLASS_ID
											LEFT JOIN 
										FEE_TRANSACTION AS FT ON FT.STUDENT_ID=CLS.STUDENT_ID AND FT.TRANSACTION_ID=FSAC.TRANSACTION_ID
											LEFT JOIN 
										SUP_PAYMENT_MODE AS SPM ON SPM.PAYMENT_MODE_ID=FT.PAYMENT_MODE
                                    WHERE
                                        CLS.CLASS_ID = ?CLASS
                                            AND CLS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND SP.IS_DELETED != 1
                                            AND SP.IS_LEFT != 1
                                    GROUP BY SP.STUDENT_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchMonthlyStudentFeeDetails:
                    {
                        query = @"SELECT
	                                    SF.FREQUENCY_NAME,
                                        MONTHNAME(FT.PAYMENT_DATE) AS MONTH,
                                        DATE_FORMAT(PAYMENT_DATE, '%d/%m/%y') AS PAYMENT_DATE,
                                        SUM(FT.COLLECTED) AS COLLECTED
                                    FROM
                                        FEE_TRANSACTION AS FT
		                                    INNER JOIN
	                                    SUP_FEE_FREQUENCY AS SF ON SF.FREQUENCY_ID=FT.FREQUENCY
											INNER JOIN
										STU_CLASS AS SC ON SC.STUDENT_ID=FT.STUDENT_ID AND SC.ACADEMIC_YEAR=?AC_YEAR
											INNER JOIN
										STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID=SC.STUDENT_ID AND SP.PROGRAMME_MODE=?PROGRAMME_MODE
                                    WHERE
                                        PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
                                    GROUP BY PAYMENT_DATE;";
                        break;
                    }
                case FeeSQLCommands.FetchStudentFeeDetailsOnlyMonth:
                    {
                        query = @"SELECT
                                        MONTHNAME(FT.PAYMENT_DATE) AS MONTH,
                                        SUM(FT.COLLECTED) AS COLLECTED
                                    FROM
                                        FEE_TRANSACTION AS FT
		                                    INNER JOIN
	                                    SUP_FEE_FREQUENCY AS SF ON SF.FREQUENCY_ID=FT.FREQUENCY
											INNER JOIN
										STU_CLASS AS SC ON SC.STUDENT_ID=FT.STUDENT_ID AND SC.ACADEMIC_YEAR=?AC_YEAR
											INNER JOIN
										STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID=SC.STUDENT_ID AND SP.PROGRAMME_MODE=?PROGRAMME_MODE
                                    WHERE
                                        PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
                                    GROUP BY MONTH ORDER BY MONTH;";
                        break;
                    }
                case FeeSQLCommands.FetchStudentAccountOnlyCredit:
                    {
                        query = @"SELECT 
                                        HEAD, CREDIT
                                    FROM
                                        FEE_STUDENT_ACCOUNT
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID AND FREQUENCY_ID = ?FREQUENCY_ID
                                            AND TRANSACTION_ID IS NULL
                                            AND IS_DELETED != 1
                                            AND ACADEMIC_YEAR = ?AC_YEAR
                                            AND DEBIT IS NULL;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeCollection:
                    {
                        query = @"UPDATE FEE_COLLECTION
                                SET
                                IS_DELETED=1
                                WHERE TRANSACTION_ID = ?TRANSACTION_ID;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeTransaction:
                    {
                        query = @"UPDATE FEE_TRANSACTION
                                SET
                                IS_DELETED=1
                                WHERE TRANSACTION_ID = ?TRANSACTION_ID;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeStudentAccount:
                    {
                        query = @"UPDATE FEE_STUDENT_ACCOUNT
                                SET
                                IS_DELETED=1
                                WHERE TRANSACTION_ID = ?TRANSACTION_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchTotalDebitByStudentIdAndFrequencyId:
                    {
                        query = @"SELECT 
                                        FT.RECEIPT_NO,
                                        FT.COLLECTED,
                                        DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                        SPM.PAYMENT_MODE,
                                        FT.STUDENT_ID
                                    FROM
                                        FEE_TRANSACTION AS FT
                                            INNER JOIN
                                        SUP_PAYMENT_MODE AS SPM ON SPM.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                                            
                                    WHERE
                                        FT.STUDENT_ID = ?STUDENT_ID AND FT.FREQUENCY = ?FREQUENCY AND FT.IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeStudentAccountForReceiptByStudentIdAndFrequencyId:
                    {
                        query = @"SELECT 
                                        FSA.STUDENT_ID,SFM.HEAD, FSA.TRANSACTION_ID, FSA.DEBIT,FT.RECEIPT_NO
                                    FROM
                                        FEE_STUDENT_ACCOUNT AS FSA
											INNER JOIN
										fee_transaction AS FT ON FT.TRANSACTION_ID=FSA.TRANSACTION_ID
                                            INNER JOIN
                                        SUP_HEAD AS SFM ON SFM.HEAD_ID = FSA.HEAD
                                    WHERE
                                        FSA.STUDENT_ID=?STUDENT_ID AND FSA.FREQUENCY_ID=?FREQUENCY_ID AND FSA.TRANSACTION_ID IS NOT NULL
                                            AND FSA.IS_DELETED != 1
                                            AND SFM.IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeDiscount:
                    {
                        query = @"SELECT 
                                        FD.DISCOUNT_ID,
                                        FD.DISCOUNT_NAME,
                                        FD.DISCOUNT_VALUE,
                                        SFT.FREQUENCY_TYPE,
                                        FD.IS_USED
                                    FROM
                                        FEE_DISCOUNT AS FD
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FD.FREQUENCY_TYPE
                                    WHERE
                                        FD.IS_DELETED != 1
                                            AND SFT.IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.SaveFeeDiscount:
                    {
                        query = @"INSERT INTO FEE_DISCOUNT
                                        (
                                        DISCOUNT_NAME,
                                        FREQUENCY_TYPE,
                                        DISCOUNT_VALUE)
                                        VALUES
                                        (
                                        ?DISCOUNT_NAME,
                                        ?FREQUENCY_TYPE,
                                        ?DISCOUNT_VALUE);";
                        break;
                    }
                case FeeSQLCommands.FetchFeeDiscountById:
                    {
                        query = @"SELECT 
                                        DISCOUNT_ID, DISCOUNT_NAME, FREQUENCY_TYPE, DISCOUNT_VALUE
                                    FROM
                                        FEE_DISCOUNT
                                    WHERE
                                        DISCOUNT_ID = ?DISCOUNT_ID;";
                        break;
                    }
                case FeeSQLCommands.UpdateFeeDiscount:
                    {
                        query = @"UPDATE FEE_DISCOUNT
                                        SET
                                        DISCOUNT_NAME=?DISCOUNT_NAME,
                                        FREQUENCY_TYPE=?FREQUENCY_TYPE,
                                        DISCOUNT_VALUE=?DISCOUNT_VALUE
                                        WHERE DISCOUNT_ID=?DISCOUNT_ID;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeDiscount:
                    {
                        query = @"UPDATE FEE_DISCOUNT
                                        SET
                                        IS_DELETED=1
                                        WHERE DISCOUNT_ID=?DISCOUNT_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeDiscountAllotmentByClassIdAndAcademicyear:
                    {
                        query = @"SELECT 
                                        FDA.DISCOUNT_ALLOTMENT_ID,
                                        FDA.ACCADEMIC_YEAR,
                                        FDA.CLASS_ID,
                                        FDA.DISCOUNT_ID,
                                        FD.DISCOUNT_NAME,
                                        FD.DISCOUNT_VALUE,
                                        SP.STUDENT_ID,
                                        SP.ROLL_NO,
                                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME
                                    FROM
                                        FEE_DISCOUNT_ALLOTMENT AS FDA
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.STUDENT_ID = FDA.STUDENT_ID
                                            AND SC.CLASS_ID = FDA.CLASS_ID
                                            AND SC.ACADEMIC_YEAR = FDA.ACCADEMIC_YEAR
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            INNER JOIN
                                        FEE_DISCOUNT AS FD ON FD.DISCOUNT_ID = FDA.DISCOUNT_ID
                                    WHERE
                                        SP.IS_LEFT != 1 AND SP.IS_DELETED != 1
                                            AND SC.IS_DELETED != 1
                                            AND FDA.ACCADEMIC_YEAR = ?ACCADEMIC_YEAR
                                            AND FDA.CLASS_ID = ?CLASS_ID
                                            AND FDA.IS_DELETED != 1
                                            AND FD.IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.CheckIsFeeDiscountAllotmentExisting:
                    {
                        query = @"SELECT 
                                        DISCOUNT_ALLOTMENT_ID
                                    FROM
                                        FEE_DISCOUNT_ALLOTMENT
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID AND ACCADEMIC_YEAR = ?ACCADEMIC_YEAR
                                            AND CLASS_ID = ?CLASS_ID
                                            AND DISCOUNT_ID = ?DISCOUNT_ID
                                            AND IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.SaveFeeDiscountAllotment:
                    {
                        query = @"INSERT INTO FEE_DISCOUNT_ALLOTMENT
                                        (
                                        STUDENT_ID,
                                        ACCADEMIC_YEAR,
                                        CLASS_ID,
                                        DISCOUNT_ID)
                                        VALUES
                                        (
                                        ?STUDENT_ID,
                                        ?ACCADEMIC_YEAR,
                                        ?CLASS_ID,
                                        ?DISCOUNT_ID);";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeDiscountAllotment:
                    {
                        query = @"UPDATE FEE_DISCOUNT_ALLOTMENT
                                        SET
                                        IS_DELETED=1
                                        WHERE DISCOUNT_ALLOTMENT_ID=?DISCOUNT_ALLOTMENT_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeStructureByClassAndFrequencyId:
                    {
                        query = @"SELECT 
                                        FS.FEE_STRUCTURE_ID,
                                        FS.ACADEMIC_YEAR,
                                        FS.FREQUENCY,
                                        SFF.FREQUENCY_NAME,
                                        FS.CLASS,
                                        CLS.CLASS_NAME,
                                        SH.HEAD_ID,
                                        SH.HEAD,
                                        FS.AMOUNT,
                                        SFC.FEE_CATEGORY_ID,
                                        SFC.FEE_CATEGORY
                                    FROM
                                        FEE_STRUCTURE AS FS
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FS.FREQUENCY
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FS.HEAD
                                            AND FS.FEE_CATEGORY = SH.FEE_CATEGORY
                                            INNER JOIN
                                        SUP_FEE_CATEGORY AS SFC ON SFC.FEE_CATEGORY_ID = FS.FEE_CATEGORY
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = FS.CLASS
                                    WHERE
                                        FS.FREQUENCY = ?FREQUENCY AND FS.IS_DELETED != 1
                                            AND SH.IS_DELETED != 1
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FS.CLASS = ?CLASS;";
                        break;
                    }
                case FeeSQLCommands.CheckIsFeeStructureExisting:
                    {
                        query = @"SELECT 
                                        FEE_STRUCTURE_ID
                                    FROM
                                        FEE_STRUCTURE
                                    WHERE
                                        FREQUENCY = ?FREQUENCY AND CLASS = ?CLASS
                                            AND HEAD = ?HEAD AND ACADEMIC_YEAR=?ACADEMIC_YEAR
                                            AND IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.SaveFeeStructure:
                    {
                        query = @"INSERT INTO FEE_STRUCTURE
                                    (
                                    ACADEMIC_YEAR,
                                    FREQUENCY,
                                    CLASS_YEAR_ID,
                                    HEAD,
                                    AMOUNT,
                                    FEE_CATEGORY,
                                    PROGRAMME,
                                    BANK_ACCOUNT_ID,FEE_MAIN_HEAD_ID)
                                    VALUES
                                    (
                                    ?ACADEMIC_YEAR,
                                    ?FREQUENCY,
                                    ?CLASS_YEAR_ID,
                                    ?HEAD,
                                    ?AMOUNT,
                                    ?FEE_CATEGORY,
                                    ?PROGRAMME,
                                    ?BANK_ACCOUNT_ID,?FEE_MAIN_HEAD_ID);";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeStructure:
                    {
                        query = @"UPDATE FEE_STRUCTURE
                                SET
                                IS_DELETED=1
                                WHERE FEE_STRUCTURE_ID = ?FEE_STRUCTURE_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeStructureById:
                    {
                        query = @"SELECT 
                                        FEE_STRUCTURE_ID,
                                        ACADEMIC_YEAR,
                                        FREQUENCY,
                                        CLASS,
                                        HEAD,
                                        AMOUNT,
                                        FEE_CATEGORY,
                                        PROGRAMME
                                    FROM
                                        FEE_STRUCTURE
                                    WHERE
                                        FEE_STRUCTURE_ID = ?FEE_STRUCTURE_ID AND IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.UpdateFeeStructure:
                    {
                        query = @"UPDATE FEE_STRUCTURE
                                        SET
                                        ACADEMIC_YEAR= ?ACADEMIC_YEAR,
                                        FREQUENCY= ?FREQUENCY,
                                        CLASS_YEAR_ID= ?CLASS_YEAR_ID,
                                        HEAD= ?HEAD,
                                        AMOUNT= ?AMOUNT,
                                        FEE_CATEGORY= ?FEE_CATEGORY,
                                        PROGRAMME= ?PROGRAMME,
                                        BANK_ACCOUNT_ID= ?BANK_ACCOUNT_ID,
                                        FEE_MAIN_HEAD_ID=?FEE_MAIN_HEAD_ID
                                        WHERE FEE_STRUCTURE_ID= ?FEE_STRUCTURE_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeDiscountByClassIdAndDiscountId:
                    {
                        query = @"SELECT 
                                        DISCOUNT_ALLOTMENT_ID, STUDENT_ID, CLASS_ID, DISCOUNT_ID
                                    FROM
                                        FEE_DISCOUNT_ALLOTMENT 
                                    WHERE
                                        DISCOUNT_ID = ?DISCOUNT_ID AND CLASS_ID = ?CLASS_ID
                                            AND IS_DELETED != 1
                                            AND ACCADEMIC_YEAR = ?ACCADEMIC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.SaveFeeDiscountFrequency:
                    {
                        query = @"INSERT INTO FEE_DISCOUNT_FREQUENCY
                                        (
                                        DISCOUNT_ID,
                                        FREQUENCY_ID)
                                        VALUES
                                        (
                                        ?DISCOUNT_ID,
                                        ?FREQUENCY_ID);";
                        break;
                    }
                case FeeSQLCommands.FetchFeeDiscountFrequencyByDiscountId:
                    {
                        query = @"SELECT 
                                        DISCOUNT_FRE_ID, DISCOUNT_ID, FREQUENCY_ID
                                    FROM
                                        FEE_DISCOUNT_FREQUENCY
                                    WHERE
                                        DISCOUNT_ID = ?DISCOUNT_ID AND IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeDiscountFrequency:
                    {
                        query = @"UPDATE FEE_DISCOUNT_FREQUENCY
                                SET
                                IS_DELETED=1
                                WHERE DISCOUNT_ID = ?DISCOUNT_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeDiscountHead:
                    {
                        query = @"SELECT 
                                        FDH.DISCOUNT_HEAD_ID,
                                        FD.DISCOUNT_NAME,
                                        SFF.FREQUENCY_NAME,
                                        SH.HEAD
                                    FROM
                                        FEE_DISCOUNT_HEAD AS FDH
                                            INNER JOIN
                                        FEE_DISCOUNT_FREQUENCY AS FDF ON FDF.FREQUENCY_ID = FDH.FREQUENCY_ID
                                            AND FDF.DISCOUNT_ID = FDH.DISCOUNT_ID
                                            INNER JOIN
                                        FEE_DISCOUNT AS FD ON FD.DISCOUNT_ID = FDF.DISCOUNT_ID
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FDF.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FDH.HEAD_ID
                                    WHERE
                                        FD.IS_DELETED != 1
                                            AND FDF.IS_DELETED != 1 AND FDH.IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.FetchHeadByDiscountIdAndFrequencyId:
                    {
                        query = @"SELECT 
                                        SH.HEAD_ID,
                                        SH.HEAD,
                                        IF(FDH.DISCOUNT_HEAD_ID IS NOT NULL,
                                            'SELECTED',
                                            '') AS STATUS
                                    FROM
                                        SUP_HEAD AS SH
                                            LEFT JOIN
                                        FEE_DISCOUNT_HEAD AS FDH ON FDH.HEAD_ID = SH.HEAD_ID
                                            AND FDH.DISCOUNT_ID = ?DISCOUNT_ID
                                            AND FDH.FREQUENCY_ID = ?FREQUENCY_ID
                                            AND FDH.IS_DELETED != 1
                                    WHERE
                                        SH.IS_DELETED != 1";
                        break;
                    }
                case FeeSQLCommands.FetchFeeDiscountHeadByDiscountIdAndFrequencyId:
                    {
                        query = @"SELECT 
                                        DISCOUNT_HEAD_ID, DISCOUNT_ID, FREQUENCY_ID, HEAD_ID
                                    FROM
                                        FEE_DISCOUNT_HEAD
                                    WHERE
                                        DISCOUNT_ID = ?DISCOUNT_ID AND FREQUENCY_ID = ?FREQUENCY_ID
                                            AND IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeDiscountHead:
                    {
                        query = @"UPDATE FEE_DISCOUNT_HEAD
                                        SET
                                        IS_DELETED=1
                                        WHERE DISCOUNT_HEAD_ID=?DISCOUNT_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.SaveFeeDiscountHead:
                    {
                        query = @"INSERT INTO FEE_DISCOUNT_HEAD
                                (
                                DISCOUNT_ID,
                                FREQUENCY_ID,
                                HEAD_ID)
                                VALUES
                                (
                                ?DISCOUNT_ID,
                                ?FREQUENCY_ID,
                                ?HEAD_ID);";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeDiscountHeadByDiscountId:
                    {
                        query = @"UPDATE FEE_DISCOUNT_HEAD
                                SET
                                IS_DELETED=1
                                WHERE DISCOUNT_ID = ?DISCOUNT_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeStructureByClassAndFrequencyIdForAdm:
                    {
                        query = @"SELECT 
                                        FS.FEE_STRUCTURE_ID,
                                        FS.ACADEMIC_YEAR,
                                        FS.FREQUENCY,
                                        SFF.FREQUENCY_NAME,
                                        FS.CLASS,
                                        CLS.CLASS_NAME,
                                        SH.HEAD_ID,
                                        SH.HEAD,
                                        FS.AMOUNT,
                                        SFC.FEE_CATEGORY_ID,
                                        SFC.FEE_CATEGORY,
                                        SST.FREQUENCY_TYPE AS FEE_MODE
                                    FROM
                                        FEE_STRUCTURE AS FS
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON FFF.FREQUENCY_ID = SFF.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SST ON SST.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FS.HEAD
                                            INNER JOIN
                                        SUP_FEE_CATEGORY AS SFC ON SFC.FEE_CATEGORY_ID = FS.FEE_CATEGORY
                                            INNER JOIN
                                        ADM_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = FS.CLASS AND CLS.PROGRAMME=FS.PROGRAMME
                                    WHERE
                                        FS.IS_DELETED != 1
                                            AND SH.IS_DELETED != 1
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FS.CLASS = ?CLASS;";
                        break;
                    }
                case FeeSQLCommands.FetchHeadByProgramandFrequency:
                    {
                        query = @"SELECT 
                                    H.HEAD_ID,IF(H.HEAD_ID IS NULL,'','SELECTED')AS SELECTED,H.HEAD, FS.FEE_CATEGORY, FREQUENCY, CLASS
                                FROM
                                    FEE_STRUCTURE AS FS
                                    INNER JOIN SUP_HEAD	H ON H.HEAD_ID=FS.HEAD
                                WHERE
                                    CLASS = ?CLASS AND FREQUENCY = ?FREQUENCY;";
                        break;
                    }
                case FeeSQLCommands.FetchExamClassInfoByExamId:
                    {
                        query = @"SELECT HEAD_ID,HEAD,PROGRAMME_MODE,FEE_CATEGORY FROM SUP_HEAD WHERE IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeStructreHeadsWithAmount:
                    {
                        query = @"SELECT 
                                    H.HEAD,
                                    H.HEAD_CODE,
                                    H.HEAD_ID,
                                    FS.FEE_CATEGORY,
                                    FS.PROGRAMME,
                                    FS.FREQUENCY,
                                    FS.AMOUNT,  
									FS.CLASS,
                                    FS.FEE_STRUCTURE_ID
                                FROM
                                    SUP_HEAD AS H
                                        LEFT JOIN
                                    FEE_STRUCTURE AS FS ON H.HEAD_ID = FS.HEAD                                      
                                        AND FS.FREQUENCY = ?FREQUENCY
                                        AND FS.CLASS=?CLASS
                                        AND FS.IS_DELETED != 1                                       
                                WHERE
                                    H.IS_DELETED != 1 AND H.HEAD_ID IN (?HEAD_ID);";
                        break;
                    }
                case FeeSQLCommands.InsertFeestructureAmount:
                    {
                        query = @"INSERT INTO FEE_STRUCTURE
                                (
                                ACADEMIC_YEAR,
                                FREQUENCY,
                                CLASS,
                                HEAD,
                                AMOUNT,                                
                                FEE_CATEGORY,
                                PROGRAMME)
                                VALUES
                                (
                                ?ACADEMIC_YEAR,
                                ?FREQUENCY,
                                ?CLASS,
                                ?HEAD,
                                ?AMOUNT,                                
                                ?FEE_CATEGORY,
                                ?PROGRAMME);";
                        break;
                    }
                case FeeSQLCommands.UpdateFeestructureAmount:
                    {
                        query = @"UPDATE FEE_STRUCTURE
                                SET
                                ACADEMIC_YEAR = ?ACADEMIC_YEAR,
                                FREQUENCY = ?FREQUENCY,
                                CLASS = ?CLASS,
                                HEAD = ?HEAD ,
                                AMOUNT = ?AMOUNT,                               
                                FEE_CATEGORY = ?FEE_CATEGORY,
                                PROGRAMME = ?PROGRAMME
                                WHERE FEE_STRUCTURE_ID = ?FEE_STRUCTURE_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchTransferProgramList:
                    {
                        query = @"SELECT 
                                    RES.PAYU_RESPONSE_ID,
                                    RES.TXNID,
                                    RES.FIRSTNAME,
                                    RES.STATUS,
                                    RES.ERROR_MESSAGE,
                                    RES.UNMAPPEDSTATUS,
                                    APP.APPLICATION_TYPE_ID,
                                    APP.APPLICATION_TYPE,
                                    I.ISSUE_ID
                                FROM
                                    FEE_PAYU_REQUEST_?AC_YEAR AS REQ
                                        LEFT JOIN
                                    FEE_PAYU_RESPONSE_?AC_YEAR AS RES ON RES.TXNID = REQ.TXNID
                                        LEFT JOIN
                                    ADM_ISSUE_APPLICATION AS I ON I.ISSUE_ID = RES.UDF1
                                        AND I.IS_DELETED != 1
                                        LEFT JOIN
                                    ADM_APPLICATION_TYPE AS APP ON APP.APPLICATION_TYPE_ID = I.APPLICATION_TYPE_ID
                                        AND APP.IS_DELETED != 1
                                WHERE I.APPLICATION_TYPE_ID=?APPLICATION_TYPE_ID AND I.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID);";
                        break;
                    }
                case FeeSQLCommands.FetchPayUAPI:
                    {
                        query = @"SELECT 
                                    MERCHANT_ACCOUNT_INFO,
                                    ACCOUNT_ID,
                                    USERNAME,
                                    `PASSWORD`,
                                    `KEY`,
                                    SALT,
                                    BASE_URL,
                                    AUTHORIZATION,
                                    API_TYPE,
                                    HASH_SEQUENCE,
                                    BANK_ACCOUNT_ID,surl,furl,curl,ACCOUNT_TYPE
                                FROM
                                    FEE_MERCHANT_ACCOUNT_INFO AS MI
                                        INNER JOIN
                                    SUP_BANK_ACCOUNT AS BA ON BA.BANK_ACCOUNT_ID = MI.ACCOUNT_ID                                       
                                WHERE                                  
                                         MI.IS_DELETED != 1
                                        AND BA.IS_DELETED != 1
                                        AND MI.API_TYPE = ?API_TYPE and ACCOUNT_TYPE=?ACCOUNT_TYPE;";
                        break;
                    }
                case FeeSQLCommands.FetchPayURequestByTransactionId:
                    {

                        query = @"SELECT 
                                    pr.txnid,
                                    pr.`key`,
                                    pr.amount,
                                    pr.udf2,
                                    pr.udf1,
                                    pr.request_date,
                                    fr.PAYU_RESPONSE_ID,
                                    fr.productinfo,
                                    CONCAT(IF(fr.firstname IS NULL, '', fr.firstname),
                                                    ' ',
                                                    IF(fr.lastname IS NULL, '', fr.lastname)) AS firstname,
                                    fr.email,
                                    fr.phone,
                                    fr.udf3, 
                                    fr.udf4, 
                                    fr.udf5,
                                    fr.hash,
                                    fr.payment_source, 
                                    fr.PG_TYPE, 
                                    fr.bank_ref_num, 
                                    fr.bankcode, 
                                    fr.error, 
                                    fr.error_Message, 
                                    fr.name_on_card, 
                                    fr.cardnum, 
                                    fr.mode, 
                                    fr.mihpayid, 
                                    fr.status, 
                                    fr.unmappedstatus, 
                                    date_format(fr.addedon,'%d/%m/%Y') as 'addedon',
                                    fr.additionalCharges
                                FROM
                                    fee_payu_request_?AC_YEAR AS pr
                                        LEFT JOIN
                                    fee_payu_response_?AC_YEAR AS fr ON pr.txnid = fr.txnid
                                WHERE
                                    pr.txnid=?TXNID";
                        break;
                    }
                case FeeSQLCommands.FetchStudentApplicationNoByIssueId:
                    {
                        query = @"SELECT ISSUE_ID,APPLICATION_NO FROM ADM_ISSUE_APPLICATION WHERE ISSUE_ID=?ISSUE_ID  AND IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeMainHeadByFrequencyId:
                    {
                        query = @"SELECT 
                                        FEE_MAIN_HEAD_ID,
                                        FREQUENCY_ID,
                                        MAIN_HEAD_ID,
                                        HEAD_ID,
                                        ACADEMIC_YEAR
                                    FROM
                                        FEE_MAIN_HEADS
                                    WHERE
                                        FREQUENCY_ID = ?FREQUENCY_ID
                                            AND ACADEMIC_YEAR = ?AC_YEAR AND IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.CheckFeeStrutureIsUsed:
                    {
                        query = @"SELECT FEE_STRUCTURE_ID FROM FEE_STRUCTURE WHERE IS_DELETED!=1 AND IS_USED=1 AND FEE_STRUCTURE_ID=?FEE_STRUCTURE_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeStudentAccountByStudentId:
                    {
                        query = @"SELECT 
                                        FFF.FEE_FREQUENCY_FEE_MODE_ID AS FREQUENCY_ID,
                                        SFF.FREQUENCY_NAME,
                                        SFM.MAIN_HEAD_ID,
                                        SFM.MAIN_HEAD,
                                        SUM(FSA.CREDIT) AS CREDIT,
                                        SH.HEAD_ID,
                                        SH.HEAD
                                    FROM
                                        FEE_STUDENT_ACCOUNT AS FSA
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FSA.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                            AND SFF.ACADEMIC_YEAR = ?AC_YEAR
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = FSA.STUDENT_ID
                                            INNER JOIN
                                        ADM_ISSUE_APPLICATION AS AI ON AI.ISSUE_ID = AR.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS APP ON APP.PRO_GROUPS_ID = AI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        FEE_MAIN_HEADS AS SMH ON SMH.FREQUENCY_ID = FSA.FREQUENCY_ID
                                            AND SMH.ACADEMIC_YEAR = ?AC_YEAR
                                            AND SMH.HEAD_ID = FSA.HEAD
                                            AND APP.PROGRAMME_MODE = SMH.PROGRAMME_MODE
                                            AND APP.SHIFT = SMH.SHIFT
                                            AND SMH.ACADEMIC_YEAR = ?AC_YEAR
                                            AND SMH.FREQUENCY_ID = FSA.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = SMH.MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FSA.HEAD
                                            INNER JOIN
                                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                            AND SST.IS_ACTIVE = 1
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID AND FSA.IS_DELETED != 1
                                            AND FSA.IS_CANCELLED_HEAD != 1
                                            AND FSA.ACADEMIC_YEAR = ?AC_YEAR
                                    GROUP BY SFM.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchCreditAndDebitByStudentId:
                    {
                        query = @"SELECT 
                                        IFNULL(SUM(CREDIT), 0) CREDIT,
                                        IFNULL(SUM(DEBIT), 0) DEBIT,
                                        (IFNULL(SUM(CREDIT), 0) - IFNULL(SUM(DEBIT), 0)) AS BALANCE,
                                        FSA.FREQUENCY_ID
                                    FROM
                                        FEE_STUDENT_ACCOUNT AS FSA
                                        INNER JOIN FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID=FSA.FREQUENCY_ID AND FFF.ACADEMIC_YEAR=?AC_YEAR
                                        INNER JOIN SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID=FFF.FREQUENCY_ID AND SFF.ACADEMIC_YEAR=?AC_YEAR
                                        INNER JOIN SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID=SFF.SEMESTER_TYPE AND SST.IS_ACTIVE=1
                                    WHERE
                                        FSA.STUDENT_ID = ?STUDENT_ID AND FSA.IS_DELETED != 1 AND FSA.IS_CANCELLED_HEAD!=1 AND IS_REFUND!=1
                                            AND FSA.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeTransactionIdByStudentIdAndFrequencyId:
                    {
                        query = @"SELECT TRANSACTION_ID FROM FEE_TRANSACTION WHERE STUDENT_ID=?RECEIVE_ID AND FREQUENCY=?FREQUENCY_ID AND IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.SaveDeletedAccount:
                    {
                        query = @"INSERT INTO FEE_COLLECTION_DELETED
                                                (
                                                ACADEMIC_YEAR,
                                                DATE,
                                                RECEIPT_NO,
                                                TRANSACTION_ID,
                                                NAME,
                                                CLASS,
                                                PAYMENT_MODE,
                                                AMOUNT)
                                                VALUES
                                                (
                                                ?AC_YEAR,
                                                CURDATE(),
                                                ?RECEIPT_NO,
                                                ?TRANSACTION_ID,
                                                ?NAME,
                                                ?CLASS,
                                                ?PAYMENT_MODE,
                                                ?AMOUNT);";
                        break;
                    }
                case FeeSQLCommands.FetchFeeTransactionById:
                    {
                        query = @"SELECT 
                                        FT.TRANSACTION_ID,
                                        RECEIPT_NO,
                                        PAYMENT_MODE,
                                        COLLECTED AS AMOUNT,
                                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(AR.LAST_NAME, '')) AS NAME
                                    FROM
                                        FEE_TRANSACTION AS FT
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = FT.STUDENT_ID
                                    WHERE
                                        FT.TRANSACTION_ID = ?TRANSACTION_ID;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeStudentAccountOnlyCredit:
                    {
                        query = @"UPDATE FEE_STUDENT_ACCOUNT SET IS_DELETED=1 WHERE STUDENT_ID=?RECEIVE_ID AND FREQUENCY_ID=?FREQUENCY_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchFrequencyIdByFeeMode:
                    {
                        query = @"SELECT 
                                        FEE_FREQUENCY_FEE_MODE_ID AS FREQUENCY_ID
                                    FROM
                                        FEE_FREQUENCY_FEE_MODE AS FFF
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID=FFF.FREQUENCY_ID AND SFF.ACADEMIC_YEAR=?AC_YEAR
		                                        INNER JOIN
	                                    SUP_SEMESTER_TYPE AS SMT ON SMT.SEMESTER_TYPE_ID=SFF.SEMESTER_TYPE AND SMT.IS_ACTIVE=1
                                    WHERE
                                        FFF.FEE_MODE  IN(?FEE_MODE)
                                            AND FFF.ACADEMIC_YEAR = ?AC_YEAR";
                        break;
                    }
                case FeeSQLCommands.FetchFeeReceiptByTransactionId:
                    {
                        query = @"SELECT 
                                        FT.TRANSACTION_ID,
                                        FT.FREQUENCY,
                                        FMH.MAIN_HEAD_ID,
                                        SFT.FREQUENCY_TYPE AS FREQUENCY_NAME,
                                        MH.MAIN_HEAD,
                                        SUM(IFNULL(PAID_AMOUNT, 0)) AS DEBIT,
                                        SFT.FREQUENCY_TYPE_ID
                                    FROM
                                        FEE_TRANSACTION AS FT
                                            INNER JOIN
                                        FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = FT.TRANSACTION_ID
                                            INNER JOIN
                                        FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FMH.HEAD_ID
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    WHERE
                                            FT.TRANSACTION_ID = ?TRANSACTION_ID
                                            AND FC.IS_DELETED != 1
                                            AND FT.IS_DELETED !=1
                                            AND FC.FREQUENCY = ?FREQUENCY_ID
                                    GROUP BY FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchTransactionByIssueId:
                    {
                        query = @"SELECT 
                                            RA.RECEIVE_ID,
                                            CONCAT(CP.PROGRAMME_NAME,'-' ,PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                            ASP.APPLICATION_NO,
                                            RA.FIRST_NAME,
                                            RA.SMS_MOBILE_NO,
                                            DATE_FORMAT(RA.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                            RA.HSC_NO,
                                            FT.TRANSACTION_ID,
                                            STUDENT_ID,
                                            DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                            RECEIPT_NO,
                                            SPM.PAYMENT_MODE,
                                            COLLECTED,
                                            DISCOUNT,
                                            SFF.FREQUENCY_ID
                                        FROM
                                            FEE_TRANSACTION AS FT
                                                INNER JOIN
                                            ADM_RECEIVE_APPLICATION AS RA ON RA.RECEIVE_ID = FT.STUDENT_ID
                                                AND RA.IS_DELETED != 1
                                                INNER JOIN
                                           ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID = RA.RECEIVE_ID
                                                AND ASP.IS_DELETED != 1       
                                                INNER JOIN
                                            CP_PROGRAMME_GROUP AS AAP ON AAP.PROGRAMME_GROUP_ID = ASP.PROGRAMME_ID
                                                INNER JOIN
                                            CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                                INNER JOIN
                                            SUP_PAYMENT_MODE AS SPM ON SPM.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                                                INNER JOIN
                                            FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                                                INNER JOIN
                                            SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                           INNER JOIN SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID=AAP.PROGRAMME_MODE
                                        INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=AAP.SHIFT
                                        WHERE
		                                        FT.FREQUENCY = ?FREQUENCY_ID
                                                AND FT.STUDENT_ID = ?STUDENT_ID
                                                AND FT.IS_DELETED != 1
                                                AND RA.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.FetchDayWiseStudentDetailsByUserId:
                    {
                        query = @"SELECT 
                                CONCAT(SFF.FREQUENCY_NAME,
                                        ' (',
                                        SFT.FREQUENCY_TYPE,
                                        ')') AS FREQUENCY_NAME,
                                MONTHNAME(FT.PAYMENT_DATE) AS MONTH,
                                DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                SUM(FT.COLLECTED) AS COLLECTED
                            FROM
                                FEE_TRANSACTION AS FT
                                    INNER JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                                    INNER JOIN
                                SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                    INNER JOIN
                                SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    INNER JOIN
                                SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                    AND SST.IS_ACTIVE = 1     
                            WHERE
                                PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
                                    AND USERNAME = ?USERNAME
                                    AND FREQUENCY IN (?FREQUENCY)
                            GROUP BY PAYMENT_DATE , FREQUENCY;";
                        break;
                    }
                case FeeSQLCommands.FetchStudentFeeDetailsOnlyMonthByUserId:
                    {
                        query = @"SELECT
                                        MONTHNAME(FT.PAYMENT_DATE) AS MONTH,
                                        SUM(FT.COLLECTED) AS COLLECTED
                                    FROM
                                        FEE_TRANSACTION AS FT
											INNER JOIN
	                                    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID=FT.FREQUENCY
                                             INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
		                                    INNER JOIN
	                                    SUP_FEE_FREQUENCY AS SF ON SF.FREQUENCY_ID=FFF.FREQUENCY_ID
											INNER JOIN
										SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID=SF.SEMESTER_TYPE AND SST.IS_ACTIVE=1
											INNER JOIN
										ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID=FT.STUDENT_ID
                                    WHERE
                                         PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
										AND USERNAME = ?USERNAME
										AND FREQUENCY IN (?FREQUENCY)
                                    GROUP BY MONTH,FREQUENCY ORDER BY MONTH;";
                        break;
                    }
                case FeeSQLCommands.CheckIsExistingAccount:
                    {
                        query = @"SELECT 
                                        F_STUDENT_AC_ID
                                    FROM
                                        FEE_STUDENT_ACCOUNT
                                    WHERE
                                        STUDENT_ID = ?STUDENT_ID AND FREQUENCY_ID = ?FREQUENCY_ID
                                            AND IS_DELETED != 1
                                            AND IS_CANCELLED_HEAD != 1;";
                        break;
                    }
                case FeeSQLCommands.FetchDayWiseFeeCollection:
                    {
                        query = @"SELECT 
                                CONCAT(SFF.FREQUENCY_NAME,
                                        '- ( ',
                                        SFT.FREQUENCY_TYPE,
                                        ' )') AS FREQUENCY_NAME,
                                MONTHNAME(FT.PAYMENT_DATE) AS MONTH,
                                FT.FREQUENCY,
                                DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                SUM(FT.COLLECTED) AS COLLECTED
                            FROM
                                FEE_TRANSACTION AS FT
                                    INNER JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                                    INNER JOIN
                                sup_frequency_type AS SFT ON SFT.frequency_type_id = FFF.FEE_MODE
                                    INNER JOIN
                                SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    INNER JOIN
                                SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                    AND SST.IS_ACTIVE = 1
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = FT.STUDENT_ID
                            WHERE
                                PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
                                    AND FT.FREQUENCY IN (?FREQUENCY_ID)
                            GROUP BY PAYMENT_DATE;";
                        break;
                    }
                case FeeSQLCommands.FetchFeePaymentDetailsByDate:
                    {
                        query = @"SELECT 
                        SFF.FREQUENCY_NAME,
                        MONTHNAME(FT.PAYMENT_DATE) AS MONTH,
                        DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                        COLLECTED,
                        CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                        CONCAT(CP.PROGRAMME_NAME,
                                ' SHIFT- ',
                                SS.SHIFT_NAME,
                                ' ( ',
                                PM.PROGRAMME_MODE,
                                ')') AS PROGRAMME_NAME,
                        SPM.PAYMENT_MODE,
                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                ' ',
                                IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME
                    FROM
                        FEE_TRANSACTION AS FT
                            INNER JOIN
                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                            INNER JOIN
                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                            INNER JOIN
                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                            AND SST.IS_ACTIVE = 1
                            INNER JOIN
                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = FT.STUDENT_ID
                            INNER JOIN
                        ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                            INNER JOIN
                        CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                            INNER JOIN
                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                            INNER JOIN
                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                            INNER JOIN
                        SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                            INNER JOIN
                        SUP_PAYMENT_MODE AS SPM ON SPM.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                    WHERE
                        PAYMENT_DATE = ?PAYMENT_DATE
                            AND FREQUENCY = ?FREQUENCY;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeRefundedList:
                    {
                        query = @"SELECT 
                                    CD.DELETED_ID,
                                    CD.ACADEMIC_YEAR,
                                    DATE_FORMAT(CD.DATE,'%d/%m/%Y') AS 'DATE',
                                    CD.RECEIPT_NO,
                                    CD.TRANSACTION_ID,
                                    CD.NAME,
                                    CD.AMOUNT,
                                    CD.REMARK,
                                    P.PAYMENT_MODE
                                FROM
                                    FEE_COLLECTION_DELETED AS CD
                                        INNER JOIN
                                    SUP_PAYMENT_MODE AS P ON P.PAYMENT_MODE_ID = CD.PAYMENT_MODE
                                WHERE
                                    CD.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeMode:
                    {
                        query = @"SELECT 
                                        FFF.FEE_FREQUENCY_FEE_MODE_ID AS FREQUENCY_ID,
                                        CONCAT(IFNULL(SFF.FREQUENCY_NAME, ''),
                                        '( ',
                                        IFNULL(SFT.FREQUENCY_TYPE, ''),
                                        ')') AS FREQUENCY_NAME
                                    FROM
                                        FEE_FREQUENCY_FEE_MODE AS FFF
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                            INNER JOIN
                                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                            AND SST.IS_ACTIVE = 1
                                    WHERE
                                        FFF.ACADEMIC_YEAR = ?AC_YEAR
                                            AND SFT.IS_DELETED != 1
                                            AND SFF.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.BindFeeMainHeadBySubHead:
                    {
                        query = @"SELECT 
                                FEE_MAIN_HEAD_ID,
                                FH.MAIN_HEAD_ID,
                                FR.FEE_ROOT,
                                AP.APPLICATION_TYPE,
                                SH.HEAD_ID,
                                SFH.MAIN_HEAD,
                                SH.HEAD,
                                SFF.FREQUENCY_NAME,
                                SFT.FREQUENCY_TYPE,
                                SF.SHIFT_NAME,
                                SPM.PROGRAMME_MODE,
                                FH.ACADEMIC_YEAR,
                                B.BANK_ACCOUNT_ID,
                                FCAT.FEE_CATEGORY_ID
                            FROM
                                SUP_HEAD AS SH
                                    LEFT JOIN
                                FEE_MAIN_HEADS AS FH ON FH.HEAD_ID = SH.HEAD_ID AND FH.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FH.FREQUENCY_ID = ?FREQUENCY_ID
                                    AND FH.FEE_ROOT_ID = ?FEE_ROOT_ID
                                    AND FH.SHIFT = ?SHIFT
                                    AND FH.APPLICATION_TYPE_ID = ?APPLICATION_TYPE_ID
                                    AND FH.MAIN_HEAD_ID = ?MAIN_HEAD_ID
                                    AND FH.PROGRAMME_MODE = ?PROGRAMME_MODE
                                  AND  FH.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_FEE_MAIN_HEAD AS SFH ON SFH.MAIN_HEAD_ID = FH.MAIN_HEAD_ID
                                    AND SFH.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_SHIFT AS SF ON SF.SHIFT_ID = FH.SHIFT
                                    AND FH.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = FH.PROGRAMME_MODE
                                    AND SPM.IS_DELETED != 1
                                    LEFT JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FH.FREQUENCY_ID
                                    LEFT JOIN
                                SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    AND SFF.IS_DELETED != 1
                                    LEFT JOIN
                                FEE_ROOT AS FR ON FR.FEE_ROOT_ID = FH.FEE_ROOT_ID
                                    AND FR.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_APPLICATION_TYPE AS AP ON AP.APPLICATION_TYPE_ID = FH.APPLICATION_TYPE_ID
                                    AND AP.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_BANK_ACCOUNT AS B ON B.BANK_ACCOUNT_ID = FH.BANK_ACCOUNT_ID
                                    AND B.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                    AND SFT.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_FEE_CATEGORY AS FCAT ON FCAT.FEE_CATEGORY_ID = FH.FEE_CATEGORY_ID
                                    AND FCAT.IS_DELETED != 1
                            WHERE
                                SH.IS_DELETED != 1 
                                    AND SH.HEAD_ID IN (?HEAD_ID);";
                        break;
                    }
                case FeeSQLCommands.FetchMappedSupHead:
                    {
                        query = @"SELECT 
                                   MAIN_HEAD_ID, HEAD_ID, SHIFT, PROGRAMME_MODE, FREQUENCY_ID,APPLICATION_TYPE_ID,FEE_ROOT_ID
                                FROM
                                    FEE_MAIN_HEADS AS FH
                                WHERE
                                    IS_DELETED != 1 AND FH.SHIFT = ?SHIFT
                                        AND FH.PROGRAMME_MODE = ?PROGRAMME_MODE
                                        AND FH.FREQUENCY_ID = ?FREQUENCY_ID
                                        AND FH.APPLICATION_TYPE_ID = ?APPLICATION_TYPE_ID
                                        AND FH.FEE_ROOT_ID=?FEE_ROOT_ID AND FH.MAIN_HEAD_ID !=?MAIN_HEAD_ID AND FH.ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeMainHeadsByMainHeadId:
                    {
                        query = @"SELECT 
                                        MAIN_HEAD_ID,
                                        HEAD_ID,
                                        SHIFT,
                                        PROGRAMME_MODE,
                                        FREQUENCY_ID
                                    FROM
                                        FEE_MAIN_HEADS
                                    WHERE
                                        MAIN_HEAD_ID = ?MAIN_HEAD_ID AND SHIFT=?SHIFT AND PROGRAMME_MODE=?PROGRAMME_MODE AND FREQUENCY_ID=?FREQUENCY_ID  AND FEE_ROOT_ID = ?FEE_ROOT_ID
                                            AND APPLICATION_TYPE_ID = ?APPLICATION_TYPE_ID AND ACADEMIC_YEAR=?AC_YEAR AND IS_DELETED!=1; ";
                        break;
                    }
                case FeeSQLCommands.FetchFeemainHeadbyId:
                    {
                        query = @"SELECT 
                            FEE_MAIN_HEAD_ID,
                            FREQUENCY_ID,
                            APPLICATION_TYPE_ID,
                            MAIN_HEAD_ID,
                            HEAD_ID,
                            PROGRAMME_MODE,
                            SHIFT,
                            FEE_CATEGORY_ID,
                            FEE_ROOT_ID,
                            BANK_ACCOUNT_ID
                        FROM
                            FEE_MAIN_HEADS
                        WHERE
                            FEE_MAIN_HEAD_ID = ?FEE_MAIN_HEAD_ID AND IS_DELETED!=1 AND ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.CheckIsSubHeadMapped:
                    {
                        query = @"SELECT 
                                COUNT(STUDENT_ID)>0 AS `STATUS`
                            FROM
                                FEE_STUDENT_ACCOUNT AS FA
                            WHERE
                                FA.FREQUENCY_ID = ?FREQUENCY_ID
                                    AND FA.ACADEMIC_YEAR = ?AC_YEAR
                                    AND HEAD = ?HEAD_ID AND FA.IS_DELETED!=1;";
                        break;
                    }
                case FeeSQLCommands.BindFeeStructureForEditByClass:
                    {
                        query = @"SELECT 
                                        FS.FEE_STRUCTURE_ID,
                                        FMH.FEE_MAIN_HEAD_ID,
                                        FS.ACADEMIC_YEAR,
                                        FMH.APPLICATION_TYPE_ID,
                                        FMH.PROGRAMME_MODE,
                                        FMH.SHIFT,
                                        FMH.FREQUENCY_ID,
                                        SH.HEAD_ID,
                                        SH.HEAD,
                                        SFM.MAIN_HEAD,
                                        FS.AMOUNT,
                                        FMH.BANK_ACCOUNT_ID,
                                        FMH.FEE_CATEGORY_ID,
                                        fc.FEE_CATEGORY as FEE_CATEGORY_NAME,
                                       CONCAT(BA.PASSBOOK_NO,' - ',B.BANK_NAME) AS PASSBOOK_NO
                                    FROM
                                        FEE_MAIN_HEADS AS FMH
                                            INNER JOIN
                                        cp_programme_group AS PG ON FMH.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                                            AND FMH.PROGRAMME_MODE = PG.PROGRAMME_MODE
                                            AND FMH.SHIFT = PG.SHIFT
                                            AND FMH.ACADEMIC_YEAR = ?AC_YEAR
                                            LEFT JOIN
                                        FEE_STRUCTURE AS FS ON  FS.FEE_MAIN_HEAD_ID = FMH.FEE_MAIN_HEAD_ID
                                            AND FS.FREQUENCY = FMH.FREQUENCY_ID                                            
                                            AND FS.PROGRAMME = PG.PROGRAMME_GROUP_ID
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FS.IS_DELETED != 1
                                            AND FS.CLASS_YEAR_ID = ?CLASS_YEAR_ID
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FMH.HEAD_ID
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        LEFT join sup_fee_category as fc on fc.FEE_CATEGORY_ID=FMH.FEE_CATEGORY_ID
                                        LEFT JOIN sup_bank_account AS BA ON BA.BANK_ACCOUNT_ID=FMH.BANK_ACCOUNT_ID
                                        LEFT JOIN sup_bank AS B ON B.BANK_ID=BA.BANK
                                    WHERE
                                        FMH.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FMH.FREQUENCY_ID = ?FREQUENCY
                                            AND PG.PROGRAMME_GROUP_ID = ?PROGRAMME;";
                        break;
                    }
                case FeeSQLCommands.BindFeeHostelStructure:
                    {
                        query = @"SELECT 
                                        FS.FEE_STRUCTURE_ID,
                                        FMH.FEE_MAIN_HEAD_ID,
                                        FS.ACADEMIC_YEAR,
                                        FMH.APPLICATION_TYPE_ID,
                                        FMH.PROGRAMME_MODE,
                                        FMH.SHIFT,
                                        FMH.FREQUENCY_ID,
                                        SH.HEAD_ID,
                                        SH.HEAD,
                                        SFM.MAIN_HEAD,
                                        FS.AMOUNT,
                                        FMH.BANK_ACCOUNT_ID,
                                        FMH.FEE_CATEGORY_ID,
                                        fc.FEE_CATEGORY as FEE_CATEGORY_NAME,
                                       CONCAT(BA.PASSBOOK_NO,' - ',B.BANK_NAME) AS PASSBOOK_NO
                                    FROM
                                        FEE_MAIN_HEADS AS FMH
                                            left join
                                        fee_structure_for_hostel_and_mess AS FS ON  FS.FEE_MAIN_HEAD_ID = FMH.FEE_MAIN_HEAD_ID
                                            AND FS.FREQUENCY = FMH.FREQUENCY_ID                                                                                        
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FS.IS_DELETED != 1                                           
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FMH.HEAD_ID
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        LEFT join sup_fee_category as fc on fc.FEE_CATEGORY_ID=FMH.FEE_CATEGORY_ID
                                        LEFT JOIN sup_bank_account AS BA ON BA.BANK_ACCOUNT_ID=FMH.BANK_ACCOUNT_ID
                                        LEFT JOIN sup_bank AS B ON B.BANK_ID=BA.BANK
                                    WHERE
                                        FMH.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FMH.FREQUENCY_ID = ?FREQUENCY AND FMH.IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.UpdateFeeStructureForHostelAndMess:
                    {
                        query = @"UPDATE FEE_STRUCTURE_FOR_HOSTEL_AND_MESS
                                        SET
                                        ACADEMIC_YEAR= ?ACADEMIC_YEAR,
                                        FREQUENCY= ?FREQUENCY,                                        
                                        HEAD= ?HEAD,
                                        AMOUNT= ?AMOUNT,                                                                               
                                        BANK_ACCOUNT_ID= ?BANK_ACCOUNT_ID,
                                        FEE_MAIN_HEAD_ID=?FEE_MAIN_HEAD_ID
                                        WHERE FEE_STRUCTURE_ID= ?FEE_STRUCTURE_ID;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeHostelAndMessStructure:
                    {
                        query = @"UPDATE FEE_STRUCTURE_FOR_HOSTEL_AND_MESS
                                SET
                                IS_DELETED=1
                                WHERE FEE_STRUCTURE_ID = ?FEE_STRUCTURE_ID;";
                        break;
                    }
                case FeeSQLCommands.SaveHostelAndMessFeeStructure:
                    {
                        query = @"INSERT INTO FEE_STRUCTURE_FOR_HOSTEL_AND_MESS
                                    (
                                    ACADEMIC_YEAR,
                                    FREQUENCY,                                    
                                    HEAD,
                                    AMOUNT,                                                                        
                                    BANK_ACCOUNT_ID,FEE_MAIN_HEAD_ID)
                                    VALUES
                                    (
                                    ?ACADEMIC_YEAR,
                                    ?FREQUENCY,                                  
                                    ?HEAD,
                                    ?AMOUNT,                                                                   
                                    ?BANK_ACCOUNT_ID,?FEE_MAIN_HEAD_ID);
                                    ";
                        break;
                    }
                case FeeSQLCommands.BindFeeMainHeadBySubHeadForHostel:
                    {
                        query = @"SELECT 
                                FEE_MAIN_HEAD_ID,
                                FH.MAIN_HEAD_ID,
                                FR.FEE_ROOT,
                                AP.APPLICATION_TYPE,
                                SH.HEAD_ID,
                                SFH.MAIN_HEAD,
                                SH.HEAD,
                                SFF.FREQUENCY_NAME,
                                SFT.FREQUENCY_TYPE,
                                SF.SHIFT_NAME,
                                SPM.PROGRAMME_MODE,
                                FH.ACADEMIC_YEAR,
                                B.BANK_ACCOUNT_ID,
                                FCAT.FEE_CATEGORY_ID
                            FROM
                                SUP_HEAD AS SH
                                    LEFT JOIN
                                FEE_MAIN_HEADS AS FH ON FH.HEAD_ID = SH.HEAD_ID AND FH.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FH.FREQUENCY_ID = ?FREQUENCY_ID
                                    AND FH.FEE_ROOT_ID = ?FEE_ROOT_ID                                    
                                    AND FH.MAIN_HEAD_ID = ?MAIN_HEAD_ID                                    
                                  AND  FH.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_FEE_MAIN_HEAD AS SFH ON SFH.MAIN_HEAD_ID = FH.MAIN_HEAD_ID
                                    AND SFH.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_SHIFT AS SF ON SF.SHIFT_ID = FH.SHIFT
                                    AND FH.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = FH.PROGRAMME_MODE
                                    AND SPM.IS_DELETED != 1
                                    LEFT JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FH.FREQUENCY_ID
                                    LEFT JOIN
                                SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    AND SFF.IS_DELETED != 1
                                    LEFT JOIN
                                FEE_ROOT AS FR ON FR.FEE_ROOT_ID = FH.FEE_ROOT_ID
                                    AND FR.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_APPLICATION_TYPE AS AP ON AP.APPLICATION_TYPE_ID = FH.APPLICATION_TYPE_ID
                                    AND AP.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_BANK_ACCOUNT AS B ON B.BANK_ACCOUNT_ID = FH.BANK_ACCOUNT_ID
                                    AND B.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                    AND SFT.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_FEE_CATEGORY AS FCAT ON FCAT.FEE_CATEGORY_ID = FH.FEE_CATEGORY_ID
                                    AND FCAT.IS_DELETED != 1
                            WHERE
                                SH.IS_DELETED != 1
                                    AND SH.HEAD_ID IN (?HEAD_ID);";
                        break;
                    }
                case FeeSQLCommands.FetchSubHeadsByMainHeadIdForHostel:
                    {
                        query = @"SELECT 
                                        FEE_MAIN_HEAD_ID,
                                        FH.MAIN_HEAD_ID,
                                        SH.HEAD_ID,
                                        SH.HEAD,
                                        IF(FEE_MAIN_HEAD_ID IS NOT NULL,
                                            'SELECTED',
                                            '') AS STATUS
                                    FROM
                                        SUP_HEAD AS SH
                                            LEFT JOIN
                                        FEE_MAIN_HEADS AS FH ON SH.HEAD_ID = FH.HEAD_ID
                                            AND FH.IS_DELETED != 1
                                            AND FH.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FH.MAIN_HEAD_ID = ?MAIN_HEAD_ID                                                                               
                                            AND FH.FREQUENCY_ID = ?FREQUENCY_ID
                                            AND FH.FEE_ROOT_ID = ?FEE_ROOT_ID                                            
                                    WHERE
                                        SH.IS_DELETED != 1;";
                        break;

                    }
                case FeeSQLCommands.FetchMappedSupHeadForHostel:
                    {
                        query = @"SELECT 
                                   MAIN_HEAD_ID, HEAD_ID, SHIFT, PROGRAMME_MODE, FREQUENCY_ID,APPLICATION_TYPE_ID,FEE_ROOT_ID
                                FROM
                                    FEE_MAIN_HEADS AS FH
                                WHERE
                                    IS_DELETED != 1                                        
                                        AND FH.FREQUENCY_ID = ?FREQUENCY_ID                                        
                                        AND FH.FEE_ROOT_ID=?FEE_ROOT_ID AND FH.MAIN_HEAD_ID !=?MAIN_HEAD_ID AND FH.ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeMainHeadsByMainHeadIdForHostel:
                    {
                        query = @"SELECT 
                                        MAIN_HEAD_ID,
                                        HEAD_ID,
                                        SHIFT,
                                        PROGRAMME_MODE,
                                        FREQUENCY_ID
                                    FROM
                                        FEE_MAIN_HEADS
                                    WHERE
                                        MAIN_HEAD_ID = ?MAIN_HEAD_ID AND FREQUENCY_ID=?FREQUENCY_ID  AND FEE_ROOT_ID = ?FEE_ROOT_ID
                                            AND ACADEMIC_YEAR=?AC_YEAR AND IS_DELETED!=1; ";
                        break;
                    }
                case FeeSQLCommands.SaveFeeMainHeadForHostel:
                    {
                        query = @"INSERT INTO FEE_MAIN_HEADS
                                (
                                FREQUENCY_ID,                                                              
                                MAIN_HEAD_ID,
                                HEAD_ID,                              
                                FEE_CATEGORY_ID,
                                ACADEMIC_YEAR,
                                FEE_ROOT_ID,               
                                BANK_ACCOUNT_ID                                
                                )
                                VALUES
                                (
                                ?FREQUENCY_ID,                                                              
                                ?MAIN_HEAD_ID,
                                ?HEAD_ID,                              
                                ?FEE_CATEGORY_ID,
                                ?ACADEMIC_YEAR,
                                ?FEE_ROOT_ID,               
                                ?BANK_ACCOUNT_ID
                                );";
                        break;
                    }
                case FeeSQLCommands.FetchFeeTransaction:
                    {
                        query = @"SELECT 
                                SUM(IFNULL(FS.CREDIT, '0')) - SUM(IFNULL(FS.DEBIT, '0')) AS BALANCE,
                                SUM(FS.DEBIT) AS DEBIT,
                                FT.FREQUENCY_TYPE_ID,
                                FS.STUDENT_ID,
                                FS.FREQUENCY_ID,
                                FS.HEAD,
                                FT.FREQUENCY_TYPE AS FREQUENCY_NAME,
                                FT.FEE_ROOT_ID
                            FROM
                                FEE_STUDENT_ACCOUNT AS FS
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = FS.STUDENT_ID
                                    INNER JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY_ID
                                    INNER JOIN
                                SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                    INNER JOIN
                                SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                            WHERE
                                    FS.STUDENT_ID = ?STUDENT_ID
                                    AND ADR.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FS.IS_DELETED!=1
                            GROUP BY FFF.FEE_MODE;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeStructureByAcademicYearAndFrequencyId:
                    {
                        query = @"SELECT 
                                        FS.FEE_STRUCTURE_ID,
                                        FS.ACADEMIC_YEAR,
                                        FS.FREQUENCY,
                                        SFF.FREQUENCY_NAME,
                                        FS.CLASS,                                       
                                        SH.HEAD_ID,
                                        SH.HEAD,
                                        FS.AMOUNT,
                                        SFC.FEE_CATEGORY_ID,
                                        SFC.FEE_CATEGORY,
                                        SST.FREQUENCY_TYPE AS FEE_MODE,FS.FEE_MAIN_HEAD_ID
                                    FROM
                                        FEE_STRUCTURE AS FS
                                        INNER JOIN FEE_MAIN_HEADS AS MH ON MH.FEE_MAIN_HEAD_ID=FS.FEE_MAIN_HEAD_ID AND MH.IS_DELETED!=1                                          
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY AND FFF.ACADEMIC_YEAR=?AC_YEAR
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON FFF.FREQUENCY_ID = SFF.FREQUENCY_ID AND SFF.ACADEMIC_YEAR=?AC_YEAR
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SST ON SST.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FS.HEAD
                                            INNER JOIN
                                        SUP_FEE_CATEGORY AS SFC ON SFC.FEE_CATEGORY_ID = FS.FEE_CATEGORY                                           
                                    WHERE
                                        FS.IS_DELETED != 1
                                            AND SH.IS_DELETED != 1
                                           AND FS.CLASS_YEAR_ID=?CLASS_YEAR_ID AND  FS.PROGRAMME=?PROGRAMME
                                            AND FS.FREQUENCY = ?FREQUENCY
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeStructureReportByProgrammeWiseReport:
                    {
                        query = @"SELECT 
                                FS.FEE_STRUCTURE_ID,
                                MH.APPLICATION_TYPE_ID,
                                AP.APPLICATION_TYPE,
                                PM.PROGRAMME_MODE_ID,
                                PM.PROGRAMME_MODE,
                                MH.SHIFT,
                                SS.SHIFT_NAME,
                                MH.FREQUENCY_ID,
                                H.HEAD_ID,
                                H.HEAD,
                                MH.MAIN_HEAD_ID,
                                SMH.MAIN_HEAD,
                                FS.AMOUNT,
                                FS.BANK_ACCOUNT_ID,
                                MH.FEE_CATEGORY_ID,
                                FCAT.FEE_CATEGORY,
                                CONCAT(IFNULL(CY.CLASS_YEAR, ''),
                                        '- ',
                                        IFNULL(cp.PROGRAMME_NAME, '')) AS PROGRAMME_NAME,
                                FS.PROGRAMME,
                                FS.ACADEMIC_YEAR,
                                FS.CLASS_YEAR_ID
                            FROM
                                FEE_MAIN_HEADS AS MH
                                    LEFT JOIN
                                FEE_STRUCTURE AS FS ON MH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                    AND PROGRAMME IN (?PROGRAMME_GROUP_ID)
                                    AND FS.CLASS_YEAR_ID IN (?CLASS_YEAR_ID)
                                    AND FS.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_FEE_MAIN_HEAD AS SMH ON SMH.MAIN_HEAD_ID = MH.MAIN_HEAD_ID
                                    AND SMH.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_HEAD AS H ON H.HEAD_ID = MH.HEAD_ID
                                    AND H.IS_DELETED != 1
                                    LEFT JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = FS.PROGRAMME
                                    AND PG.IS_DELETED != 1
                                    LEFT JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    AND CP.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    AND SS.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_FEE_CATEGORY AS FCAT ON FCAT.FEE_CATEGORY_ID = MH.FEE_CATEGORY_ID
                                    AND FCAT.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = MH.PROGRAMME_MODE
                                    AND PM.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_APPLICATION_TYPE AS AP ON AP.APPLICATION_TYPE_ID = MH.APPLICATION_TYPE_ID
                                    AND AP.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_BANK_ACCOUNT AS BA ON BA.BANK_ACCOUNT_ID = MH.BANK_ACCOUNT_ID
                                    LEFT JOIN
                                SUP_BANK AS B ON B.BANK_ID = BA.BANK
                                    LEFT JOIN
                                sup_class_year AS CY ON CY.CLASS_YEAR_ID = FS.CLASS_YEAR_ID
                            WHERE
                                FS.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                    AND MH.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                    AND MH.FREQUENCY_ID = ?FREQUENCY_ID
                                    AND MH.PROGRAMME_MODE = ?PROGRAMME_MODE
                                    AND MH.SHIFT = ?SHIFT
                                    AND MH.IS_DELETED != 1
                            ORDER BY MH.APPLICATION_TYPE_ID , PG.PROGRAMME_GROUP_ID , FS.CLASS_YEAR_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchFeeMainHeadForFeeStructureByGroup:
                    {
                        query = @"SELECT 
                                    MH.MAIN_HEAD_ID, MH.MAIN_HEAD, SH.HEAD_ID, SH.HEAD
                                FROM
                                    FEE_MAIN_HEADS AS FMH
                                        LEFT JOIN
                                    FEE_STRUCTURE AS FS ON FS.FEE_MAIN_HEAD_ID = FMH.FEE_MAIN_HEAD_ID
                                        AND FS.IS_DELETED != 1
                                        AND FS.PROGRAMME IN (?PROGRAMME_GROUP_ID)
                                        AND FS.CLASS_YEAR_ID IN (?CLASS_YEAR_ID)  
                                        LEFT JOIN
                                    SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        AND MH.IS_DELETED != 2
                                        LEFT JOIN
                                    SUP_HEAD AS SH ON SH.HEAD_ID = FMH.HEAD_ID
                                        AND SH.IS_DELETED != 1
                                WHERE
                                    FMH.FREQUENCY_ID = ?FREQUENCY_ID
                                        AND FMH.PROGRAMME_MODE = ?PROGRAMME_MODE
                                        AND FMH.SHIFT = ?SHIFT
                                        AND FMH.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                        AND FS.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                        AND FMH.IS_DELETED != 1
                                GROUP BY MH.MAIN_HEAD_ID , SH.HEAD_ID
                                ORDER BY MH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchMainHeadwiseFeeReportByFeeRootIdForNewDateWiseReport:
                    {
                        query = @"SELECT 
                                FT.STUDENT_ID,
                                (IFNULL(FC.PAID_AMOUNT, 0)) AS BALANCE,
                                FT.FREQUENCY,
                                MH.FEE_MAIN_HEAD_ID,
                                MH.MAIN_HEAD_ID,
                                MH.HEAD_ID,
                                PO.UDF6 AS PROGRAMME_GROUP_ID,
                                DATE_FORMAT(FT.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                FROM_UNIXTIME(FC.SETTLEMENT_DATE, '%d/%m/%Y') AS SETTLEMENT_DATE,
                                FT.RECEIPT_NO
                            FROM
                                FEE_TRANSACTION AS FT
                                    INNER JOIN
                                FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = FT.TRANSACTION_ID
                                    INNER JOIN
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PT ON PT.RAZORPAY_PAMENT_ID = FT.RAZORPAY_ID
                                    INNER JOIN
                                FEE_RAZORPAY_ORDER_INFO_?AC_YEAR AS PO ON PO.ID = PT.ORDER_ID
                                    INNER JOIN
                                FEE_MAIN_HEADS AS MH ON MH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = PO.UDF6
                                    AND PG.SHIFT = ?SHIFT
                                    AND PG.PROGRAMME_MODE = ?PROGRAMME_MODE                                  
                            WHERE
                                FT.FREQUENCY = ?FREQUENCY
                                    AND FT.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                    AND FC.SETTLEMENT_DATE BETWEEN UNIX_TIMESTAMP(?DATE_FROM) AND UNIX_TIMESTAMP(ADDDATE(?DATE_TO, 1)) order by PG.PROGRAMME_GROUP_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchMainHeadwiseFeeReportByFeeRootIdForDateWiseReport:
                    {
                        query = @"SELECT 
                                F.STUDENT_ID,
                                (IFNULL(FC.PAID_AMOUNT, 0)) AS BALANCE,
                                F.FREQUENCY,
                                FMH.FEE_MAIN_HEAD_ID,
                                FMH.MAIN_HEAD_ID,
                                FMH.HEAD_ID,
                                PT.UDF4 AS PROGRAMME_GROUP_ID,
                                DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                FROM_UNIXTIME(ST.CREATED_AT, '%d/%m/%Y') AS SETTLEMENT_DATE,
                                F.RECEIPT_NO
                            FROM
                                FEE_TRANSACTION AS F
                                    INNER JOIN
                                FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = F.TRANSACTION_ID
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID
                                     AND FMH.PROGRAMME_MODE = ?PROGRAMME_MODE
                                    AND FMH.SHIFT = ?SHIFT
                                    AND FMH.ACADEMIC_YEAR = ?AC_YEAR								                                                         
                                    AND FMH.IS_DELETED != 1
                                    INNER JOIN
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PT ON PT.RAZORPAY_PAMENT_ID = F.RAZORPAY_ID
                                    AND UDF3 = ?FREQUENCY
                                    INNER JOIN
                                FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR AS FT ON FT.SOURCE = PT.ID
                                    AND FT.UDF3 = FMH.MAIN_HEAD_ID
                                    LEFT JOIN
                                FEE_RAZORPAY_SETTLEMENT_INFO_?AC_YEAR AS ST ON ST.ID = FT.RECIPIENT_SETTLEMENT_ID
                            WHERE
                                F.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                    AND F.FREQUENCY = ?FREQUENCY
                                    AND F.PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO;";
                        break;
                    }
                case FeeSQLCommands.FetchMainHeadwiseFeeReportForHostelFee:
                    {
                        query = @"SELECT 
                                F.STUDENT_ID,
                                (IFNULL(FC.PAID_AMOUNT, 0)) AS BALANCE,
                                F.FREQUENCY,
                                FMH.FEE_MAIN_HEAD_ID,
                                FMH.MAIN_HEAD_ID,
                                FMH.HEAD_ID,
                                PT.UDF4 AS PROGRAMME_GROUP_ID,
                                DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                FROM_UNIXTIME(ST.CREATED_AT, '%d/%m/%Y') AS SETTLEMENT_DATE,
                                F.RECEIPT_NO
                            FROM
                                FEE_TRANSACTION AS F
                                    INNER JOIN
                                FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = F.TRANSACTION_ID
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID                          
                                    AND FMH.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FMH.IS_DELETED != 1
                                    INNER JOIN
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PT ON PT.RAZORPAY_PAMENT_ID = F.RAZORPAY_ID
                                    AND UDF3 = ?FREQUENCY
                                    INNER JOIN
                                FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR AS FT ON FT.SOURCE = PT.ID
                                    AND FT.UDF3 = FMH.MAIN_HEAD_ID
                                    LEFT JOIN
                                FEE_RAZORPAY_SETTLEMENT_INFO_?AC_YEAR AS ST ON ST.ID = FT.RECIPIENT_SETTLEMENT_ID
                            WHERE
                                F.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                    AND F.FREQUENCY = ?FREQUENCY
                                    AND F.PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO;";
                        break;
                    }
                case FeeSQLCommands.FetchHeadWiseForApplicationFee:
                    {
                        query = @"SELECT 
                                F.STUDENT_ID,
                                (IFNULL(FC.PAID_AMOUNT, 0)) AS BALANCE,
                                F.FREQUENCY,
                                FMH.FEE_MAIN_HEAD_ID,
                                FMH.MAIN_HEAD_ID,
                                FMH.HEAD_ID,
                                PT.UDF6 AS PROGRAMME_GROUP_ID,
                                DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                FROM_UNIXTIME(ST.CREATED_AT, '%d/%m/%Y') AS SETTLEMENT_DATE,
                                F.RECEIPT_NO
                            FROM
                                FEE_TRANSACTION AS F
                                    INNER JOIN
                                FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = F.TRANSACTION_ID
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID
                                    AND FMH.PROGRAMME_MODE = ?PROGRAMME_MODE
                                    AND FMH.SHIFT = ?SHIFT
                                    AND FMH.ACADEMIC_YEAR = ?AC_YEAR
									AND FMH.APPLICATION_TYPE_ID=?APPTYPE_ID
                                    AND FMH.IS_DELETED != 1
                                    INNER JOIN
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PT ON PT.RAZORPAY_PAMENT_ID = F.RAZORPAY_ID
                                    AND UDF3 = ?FREQUENCY
                                    LEFT JOIN
                                FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR AS FT ON FT.SOURCE = PT.ID
                                    AND FT.UDF3 = FMH.MAIN_HEAD_ID
                                    LEFT JOIN
                                FEE_RAZORPAY_SETTLEMENT_INFO_?AC_YEAR AS ST ON ST.ID = FT.RECIPIENT_SETTLEMENT_ID
                            WHERE
                                F.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                    AND F.FREQUENCY = ?FREQUENCY
                                    AND F.PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO ;";
                        break;
                    }
                case FeeSQLCommands.FetchMainHeadsByPRogrammeGroupIdAndFrequencyIdAndFeeCategory:
                    {
                        query = @"SELECT 
                                PROGRAMME_GROUP_ID,
                                MAIN_HEAD_ID,
                                MAIN_HEAD,
                                MAX(HEAD_COUNT) AS HEAD_COUNT,
                                FEE_CATEGORY_ID
                            FROM
                                (SELECT 
                                    PG.PROGRAMME_GROUP_ID,
                                        MH.MAIN_HEAD_ID,
                                        MH.MAIN_HEAD,
                                        COUNT(FMH.HEAD_ID) AS HEAD_COUNT,
                                        FEE_CATEGORY_ID
                                FROM
                                    FEE_MAIN_HEADS AS FMH
                                INNER JOIN SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                INNER JOIN SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                INNER JOIN CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_MODE = FMH.PROGRAMME_MODE
                                    AND PG.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                                    AND PG.SHIFT = FMH.SHIFT
                                    AND PG.APPTYPE_ID = FMH.APPLICATION_TYPE_ID	
                                    AND PG.IS_DELETED != 1
                                WHERE
                                    FEE_ROOT_ID = 1 AND FREQUENCY_ID = ?FREQUENCY  AND FEE_CATEGORY_ID IN (?FEE_CATEGORY_ID)
                                        AND FMH.IS_DELETED != 1
                                GROUP BY PG.PROGRAMME_GROUP_ID , MH.MAIN_HEAD_ID) AS T
                            GROUP BY T.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchMainHeadwiseFeeReportByFeeRootIdAndFeeCategory:
                    {
                        query = @"SELECT 
                                F.STUDENT_ID,
                                (IFNULL(FC.PAID_AMOUNT, 0)) AS BALANCE,
                                F.FREQUENCY,
                                FMH.FEE_MAIN_HEAD_ID,
                                FMH.MAIN_HEAD_ID,
                                FMH.HEAD_ID,
                                PT.UDF4 AS PROGRAMME_GROUP_ID,
                                DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                FROM_UNIXTIME(ST.CREATED_AT, '%d/%m/%Y') AS SETTLEMENT_DATE,
                                F.RECEIPT_NO
                            FROM
                                FEE_TRANSACTION AS F
                                    INNER JOIN
                                FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = F.TRANSACTION_ID
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID
                                     AND FMH.PROGRAMME_MODE = ?PROGRAMME_MODE
                                    AND FMH.SHIFT = ?SHIFT
                                    AND FMH.ACADEMIC_YEAR = ?AC_YEAR
									AND FMH.FEE_CATEGORY_ID IN (?FEE_CATEGORY_ID)
                                    AND FMH.IS_DELETED != 1
                                    INNER JOIN
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PT ON PT.RAZORPAY_PAMENT_ID = F.RAZORPAY_ID
                                    AND UDF3 = ?FREQUENCY
                                    INNER JOIN
                                FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR AS FT ON FT.SOURCE = PT.ID
                                    AND FT.UDF3 = FMH.MAIN_HEAD_ID
                                    LEFT JOIN
                                FEE_RAZORPAY_SETTLEMENT_INFO_?AC_YEAR AS ST ON ST.ID = FT.RECIPIENT_SETTLEMENT_ID
                            WHERE
                                F.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                    AND F.FREQUENCY = ?FREQUENCY
                                    AND F.PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO;";
                        break;
                    }
                case FeeSQLCommands.DeleteFeeStudentAccountByReceiveIdandFrequencyId:
                    {
                        query = @"DELETE FROM FEE_STUDENT_ACCOUNT WHERE STUDENT_ID=?RECEIVE_ID AND FREQUENCY_ID=?FREQUENCY_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchApplicationTransactionByReceiveId:
                    {
                        query = @"SELECT 
                                        RA.RECEIVE_ID,
                                        CONCAT(CP.PROGRAMME_NAME,'-' ,PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                        CONCAT(ADI.APPLICATION_NO,LPAD(ADI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                        RA.FIRST_NAME,
                                        RA.SMS_MOBILE_NO,
                                        DATE_FORMAT(RA.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                        RA.HSC_NO,
                                        FT.TRANSACTION_ID,
                                        STUDENT_ID,
                                        DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                        FT.RECEIPT_NO,
                                        SPM.PAYMENT_MODE,
                                        COLLECTED,
                                        DISCOUNT,
                                        SFF.FREQUENCY_ID
                                    FROM    

                                        FEE_TRANSACTION AS FT
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS RA ON RA.RECEIVE_ID = FT.STUDENT_ID
                                            INNER JOIN
                                        adm_issued_applications AS ADI ON ADI.RECEIVE_ID = RA.RECEIVE_ID 
                                            AND IS_PAID = 1
                                            INNER JOIN
                                        cp_programme_group AS AAP ON AAP.PROGRAMME_GROUP_ID = ADI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_PAYMENT_MODE AS SPM ON SPM.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                        INNER JOIN SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID=AAP.PROGRAMME_MODE
                                        INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=AAP.SHIFT
                                    WHERE
                                            FT.FREQUENCY =?FREQUENCY_ID
                                            AND FT.STUDENT_ID = ?STUDENT_ID
                                            AND FT.IS_DELETED != 1
                                            AND ADI.RAZORPAY_ID=FT.RAZORPAY_ID
                                            AND RA.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.FetchMainHeadsByPRogrammeGroupIdAndFrequencyId:
                    {
                        query = @"SELECT 
                                        PROGRAMME_GROUP_ID, 
                                        MAIN_HEAD_ID, 
                                        MAIN_HEAD, 
                                        MAX(HEAD_COUNT) AS HEAD_COUNT
                                    FROM
                                        (SELECT 
                                            PG.PROGRAMME_GROUP_ID,
                                                MH.MAIN_HEAD_ID,
                                                MH.MAIN_HEAD,
                                                COUNT(FMH.HEAD_ID) AS HEAD_COUNT
                                        FROM
                                            FEE_MAIN_HEADS AS FMH
		                                    INNER JOIN 
                                        SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
		                                    INNER JOIN 
                                        SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
		                                    INNER JOIN 
                                        CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_MODE = FMH.PROGRAMME_MODE
                                            AND PG.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                                            AND PG.SHIFT = FMH.SHIFT
                                            AND PG.APPTYPE_ID = FMH.APPLICATION_TYPE_ID
                                            AND PG.IS_DELETED != 1
                                        WHERE
                                            FEE_ROOT_ID = ?FEE_ROOT_ID AND FREQUENCY_ID = ?FREQUENCY
                                                AND FMH.IS_DELETED != 1
                                        GROUP BY PG.PROGRAMME_GROUP_ID , MH.MAIN_HEAD_ID) AS T group by T.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchMainHeadsForHostelFee:
                    {
                        query = @"SELECT 
                                MAIN_HEAD_ID, MAIN_HEAD, MAX(HEAD_COUNT) AS HEAD_COUNT
                            FROM
                                (SELECT 
                                    MH.MAIN_HEAD_ID,
                                        MH.MAIN_HEAD,
                                        COUNT(FMH.HEAD_ID) AS HEAD_COUNT
                                FROM
                                    FEE_MAIN_HEADS AS FMH
                                INNER JOIN SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                INNER JOIN SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                WHERE
                                    FEE_ROOT_ID = ?FEE_ROOT_ID AND FREQUENCY_ID = ?FREQUENCY
                                        AND FMH.IS_DELETED != 1
                                GROUP BY MH.MAIN_HEAD_ID) AS T
                            GROUP BY T.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchMainHeadsForApplicationFeebByFrequencyId:
                    {
                        query = @"SELECT 
                                PROGRAMME_GROUP_ID,
                                MAIN_HEAD_ID,
                                MAIN_HEAD,
                                MAX(HEAD_COUNT) AS HEAD_COUNT
                            FROM
                                (SELECT 
                                    PG.PROGRAMME_GROUP_ID,
                                        MH.MAIN_HEAD_ID,
                                        MH.MAIN_HEAD,
                                        COUNT(FMH.HEAD_ID) AS HEAD_COUNT
                                FROM
                                    FEE_MAIN_HEADS AS FMH
                                INNER JOIN SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                INNER JOIN SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                INNER JOIN CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_MODE = FMH.PROGRAMME_MODE
                                    AND PG.SHIFT = FMH.SHIFT
                                    AND PG.APPTYPE_ID = FMH.APPLICATION_TYPE_ID
                                    AND PG.IS_DELETED != 1
                                WHERE
                                    FEE_ROOT_ID = ?FEE_ROOT_ID AND FREQUENCY_ID = ?FREQUENCY
                                        AND FMH.IS_DELETED != 1
                                GROUP BY PG.PROGRAMME_GROUP_ID , MH.MAIN_HEAD_ID) AS T
                            GROUP BY T.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchSettlementByDateWise:
                    {
                        query = @"SELECT                                                              
                                FROM_UNIXTIME(RS.CREATED_AT, '%d/%m/%Y') AS PAYMENT_DATE,
                                ROUND((RS.AMOUNT/100),2) AMOUNT,RS.ID,                                
                                RAC.ACCOUNT_NAME
                            FROM
                                FEE_RAZORPAY_SETTLEMENT_INFO_?AC_YEAR  AS RS                                
                                    INNER JOIN
                                FEE_RAZORPAY_ACCOUNTS AS RAC ON RAC.ACCOUNT_ID = RS.RECIPIENT
                            WHERE
                                RS.CREATED_AT BETWEEN UNIX_TIMESTAMP(?DATE_FROM) AND UNIX_TIMESTAMP(?DATE_TO) 
                                AND RAC.BANK_ACCOUNT_ID=?BANK_ACCOUNT_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchSettlementByDateWiseAndAccount:
                    {
                        query = @"SELECT 
                                AR.RECEIVE_ID,
                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                CP.PROGRAMME_NAME,
                                FROM_UNIXTIME(FT.CREATED_AT, '%d/%m/%Y') AS PAYMENT_DATE,
                                ROUND(FT.AMOUNT / 100) AS AMOUNT,
                                RAC.ACCOUNT_NAME
                            FROM
                                FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR AS FT
                                    INNER JOIN
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS FP ON FP.ID = FT.SOURCE
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = FT.UDF1
                                    LEFT JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = FT.UDF1
                                    AND IF(FP.UDF6 IS NULL,
                                    FP.UDF4 = AI.PROGRAMME_GROUP_ID,
                                    FP.UDF6 = AI.PROGRAMME_GROUP_ID)
                                    LEFT JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    LEFT JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    LEFT JOIN
                                FEE_RAZORPAY_ACCOUNTS AS RAC ON RAC.ACCOUNT_ID = FT.RECIPIENT
                            WHERE
                                FT.RECIPIENT_SETTLEMENT_ID = ?RECIPIENT_SETTLEMENT_ID
                            ORDER BY PG.APPTYPE_ID,PG.PROGRAMME_GROUP_ID,FT.CREATED_AT;";
                        break;
                    }
                case FeeSQLCommands.UpdateFeeStudentAc:
                    {
                        query = @"UPDATE FEE_STUDENT_ACCOUNT 
                                    SET 
                                        IS_DELETED = 1
                                    WHERE
                                            STUDENT_ID =?RECEIVE_ID
                                            AND FREQUENCY_ID IN (?FREQUENCY_ID) AND ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.FetchTranscationIds:
                    {
                        query = @"SELECT 
                                TRANSACTION_ID
                            FROM
                                FEE_TRANSACTION
                            WHERE
                                STUDENT_ID = ?RECEIVE_ID
                                    AND FREQUENCY IN (?FREQUENCY_ID) AND ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.UpdateTranscation:
                    {
                        query = @"UPDATE FEE_TRANSACTION 
                        SET 
                            IS_DELETED = 1
                        WHERE
                            TRANSACTION_ID IN (?TRANSACTION_ID) AND ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case FeeSQLCommands.UpdateFeecollection:
                    {
                        query = @"UPDATE  FEE_COLLECTION 
                                    SET 
                                        IS_DELETED = 1
                                    WHERE
                                        TRANSACTION_ID IN (?TRANSACTION_ID);";
                        break;
                    }
                case FeeSQLCommands.FetchApplicationFeeCountByDate:
                    {
                        //query = @"SELECT 
                        //        COUNT(AI.ISSUED_ID) AS COUNT,
                        //        DATE_FORMAT(FT.PAYMENT_DATE,'%d/%m/%Y') AS PAYMENT_DATE,
                        //        FT.COLLECTED
                        //    FROM
                        //        FEE_TRANSACTION AS FT
                        //            INNER JOIN
                        //        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = FT.STUDENT_ID
                        //            INNER JOIN
                        //        ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AI.RECEIVE_ID
                        //            AND AI.IS_PAID = 1
                        //            AND FT.RAZORPAY_ID = AI.RAZORPAY_ID
                        //            INNER JOIN
                        //        CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                        //            AND PG.SHIFT = ?SHIFT
                        //            AND PG.PROGRAMME_MODE = ?PROGRAMME_MODE
                        //            AND PG.APPTYPE_ID = ?APPTYPE_ID
                        //            INNER JOIN
                        //        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                        //    WHERE
                        //        FT.FREQUENCY = ?FREQUENCY AND PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
                        //    GROUP BY FT.PAYMENT_DATE
                        //    ORDER BY FT.PAYMENT_DATE;";
                        query = @"
                                    SELECT 
                                        COUNT(DISTINCT FT.TRANSACTION_ID) AS COUNT,
                                        SUM(FC.PAID_AMOUNT) AS AMOUNT,
                                        FT.COLLECTED,
                                        FC.PAID_AMOUNT,
                                        MH.BANK_ACCOUNT_ID,
                                        FC.HEAD,
                                        FROM_UNIXTIME(fc.SETTLEMENT_DATE, '%d/%m/%Y') AS SETTLEMENT_DATE,
                                        AC.PASSBOOK_NO
                                    FROM
                                    fee_transaction AS FT
                                        INNER JOIN
                                    fee_collection AS FC ON FC.TRANSACTION_ID = FT.TRANSACTION_ID
                                        INNER JOIN
                                    fee_razorpay_payment_info_2020 AS PT ON PT.RAZORPAY_PAMENT_ID = FT.RAZORPAY_ID
                                        INNER JOIN
                                    fee_razorpay_order_info_2020 AS PO ON PO.ID = PT.ORDER_ID
                                        INNER JOIN
                                    fee_main_heads AS MH ON MH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID
                                        INNER JOIN
                                    sup_bank_account AS AC ON AC.BANK_ACCOUNT_ID = MH.BANK_ACCOUNT_ID
                                        INNER JOIN
                                    cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = PO.UDF6
                                            AND PG.SHIFT = ?SHIFT
                                   AND PG.PROGRAMME_MODE = ?PROGRAMME_MODE
                                    AND PG.APPTYPE_ID = ?APPTYPE_ID
                                    WHERE
                                    FT.FREQUENCY = ?FREQUENCY AND
                                        fc.SETTLEMENT_DATE BETWEEN UNIX_TIMESTAMP(?DATE_FROM) AND UNIX_TIMESTAMP(ADDDATE(?DATE_TO, 1)) AND FT.ACADEMIC_YEAR=?AC_YEAR
                                    GROUP BY FC.SETTLEMENT_DATE , MH.BANK_ACCOUNT_ID
                                    ORDER BY FC.SETTLEMENT_DATE
                            
;
";
                        break;
                    }
                case FeeSQLCommands.FetchApplicationFeeCountByProgrammeMode:
                    {
                        query = @"SELECT 
                                COUNT(AI.ISSUED_ID) AS COUNT,
                                CP.PROGRAMME_NAME,
                                FT.COLLECTED
                            FROM
                                FEE_TRANSACTION AS FT
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = FT.STUDENT_ID
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AI.RECEIVE_ID
                                    AND AI.IS_PAID = 1
                                    AND FT.RAZORPAY_ID = AI.RAZORPAY_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    AND PG.SHIFT = ?SHIFT
                                    AND PG.PROGRAMME_MODE = ?PROGRAMME_MODE
                                    AND PG.APPTYPE_ID = ?APPTYPE_ID
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                            WHERE
                                FT.FREQUENCY = ?FREQUENCY AND PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
                            GROUP BY PG.PROGRAMME_GROUP_ID
                            ORDER BY PG.PROGRAMME_GROUP_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchStudentByProgrammeGroupIdForTermFeeReport:
                    {
                        query = @"SELECT 
                                FT.STUDENT_ID,
                                IF(SL.IS_CANCELED = 1,
                                    CONCAT(SL.APPLICATION_NO, '- CANCELED'),
                                    SL.APPLICATION_NO) AS APPLICATION_NO,
                                AR.ROLL_NO,
                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                SL.PROGRAMME_ID AS PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                        ' (',
                                        PM.PROGRAMME_MODE,
                                        ' ',
                                        SS.SHIFT_NAME,
                                        ')') AS PROGRAMME_NAME,
                                DATE_FORMAT(FT.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                PY.PAYMENT_MODE
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                FEE_TRANSACTION AS FT ON FT.STUDENT_ID = AR.RECEIVE_ID
                                    AND FT.FREQUENCY = ?FREQUENCY
                                    AND PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
                                    INNER JOIN
                                SUP_PAYMENT_MODE AS PY ON PY.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                            WHERE
                                (SL.IS_DELETED != 1
                                    AND SL.IS_CANCELED != 1)
                                    AND SL.PROGRAMME_ID IN (?PROGRAMME_GROUP_ID)
                            ORDER BY FT.PAYMENT_DATE;";
                        break;
                    }
                case FeeSQLCommands.FetchAdmissionStatusByStudentIdandFrequencyId:
                    {
                        query = @"SELECT 
                                    IA.RECEIVE_ID,
                                    IA.PROGRAMME_GROUP_ID,
                                    IA.STATUS,
                                    SAS.STATUS_NAME
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS IA
                                        INNER JOIN
                                    SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID = IA.STATUS
                                        INNER JOIN
                                    FEE_STUDENT_ACCOUNT AS FS ON FS.STUDENT_ID = IA.RECEIVE_ID
                                WHERE
                                    IA.RECEIVE_ID = ?STUDENT_ID
                                        AND FS.FREQUENCY_ID = ?FREQUENCY_ID
                                        AND IA.STATUS = ?STATUS
                                        AND IA.IS_DELETED != 1
                                GROUP BY FS.FREQUENCY_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchHostelAdmissionStatusByStudentId:
                    {
                        query = @"SELECT 
                                RECEIVE_ID, PROGRAMME_GROUP_ID, STATUS,SAS.STATUS_NAME
                            FROM
                                ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS HSP
                                    INNER JOIN
                                sup_adm_status AS SAS ON SAS.STATUS_ID = HSP.STATUS
                            WHERE
                                HSP.RECEIVE_ID = ?STUDENT_ID
                                    AND HSP.IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.CheckIsStudentAccountCreated:
                    {
                        query = @"SELECT 
                                SA.FREQUENCY_ID,
                                SA.HEAD,
                                SA.CREDIT,
                                SA.DEBIT,
                                SA.FEE_MAIN_HEAD_ID
                            FROM
                                FEE_STUDENT_ACCOUNT AS SA
                                    INNER JOIN
                                FEE_STRUCTURE AS FS ON SA.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                            WHERE
		                            SA.STUDENT_ID = ?RECEIVE_ID
                                    AND SA.FREQUENCY_ID = ?FREQUENCY_ID
                                    AND SA.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FS.PROGRAMME = ?PROGRAMME
                                    AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FREQUENCY = ?FREQUENCY_ID
                                    AND SA.IS_DELETED != 1;";
                        break;
                    }
                case FeeSQLCommands.FetchApplicantRefundAmountForRefundIntiate:
                    {
                        query = @"SELECT 
                                    FMH.FREQUENCY_ID,
                                    RFA.STUDENT_ID,
                                    SUM((IFNULL(RFA.AMOUNT, 0))) AS BALANCE,
                                    FF.FREQUENCY_NAME,
                                    SFM.MAIN_HEAD AS HEAD_NAME,
                                    PR.STATUS,
                                    PR.UDF6,
                                    DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                    F.RECEIPT_NO,
                                    RA.ACCOUNT_ID,
                                    PR.ID as PAYMENT_ID,
                                    TR.ID AS TRANSFER_ID,
                                    FMH.MAIN_HEAD_ID,
                                    FMH.BANK_ACCOUNT_ID,
                                    F.RAZORPAY_ID,
                                    PR.UDF2
                                FROM
                                    FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PR
                                        INNER JOIN
                                    FEE_TRANSACTION AS F ON F.RAZORPAY_ID = PR.RAZORPAY_PAMENT_ID
                                        AND F.IS_REFUND != 1
                                        INNER JOIN
                                    ADM_REFUND_STUDENT_ACCOUNT AS RFA ON RFA.TRANSACTION_ID = F.TRANSACTION_ID
                                        INNER JOIN
                                    FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = RFA.FEE_MAIN_HEAD_ID
                                        INNER JOIN
                                    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                        AND FFF.ACADEMIC_YEAR = ?AC_YEAR
                                        AND FFF.FEE_FREQUENCY_FEE_MODE_ID = F.FREQUENCY
                                        INNER JOIN
                                    SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                        AND FT.FEE_ROOT_ID = PR.UDF5
                                        INNER JOIN
                                    SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                        LEFT JOIN
                                    SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        LEFT JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                        LEFT JOIN
                                    FEE_RAZORPAY_ACCOUNTS AS RA ON RA.BANK_ACCOUNT_ID = FMH.BANK_ACCOUNT_ID
                                        INNER JOIN
                                    FEE_RAZORPAY_TRANSFER_INFO_?AC_YEAR AS TR ON TR.SOURCE = PR.ID
                                        AND TR.UDF3 = FMH.MAIN_HEAD_ID
                                WHERE
                                    PR.RAZORPAY_PAMENT_ID = ?RAZORPAY_PAMENT_ID
                                GROUP BY RFA.STUDENT_ID , F.FREQUENCY , FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.SaveStudentTransferRefundInfo:
                    {
                        query = @"INSERT INTO `FEE_RAZORPAY_REFUND_INFO_?AC_YEAR`
                                  (ID, ENTITY, AMOUNT, CURRENCY, PAYMENT_ID, RECEIPT, RRN, CREATED_AT, STATUS, SPEED_PROCESSED, SPEED_REQUESTED, UDF1, UDF2, UDF3, UDF4, UDF5, UDF6) VALUES
                                  (?ID, ?ENTITY, ?AMOUNT, ?CURRENCY, ?PAYMENT_ID, ?RECEIPT, ?RRN, ?CREATED_AT,?STATUS, ?SPEED_PROCESSED, ?SPEED_REQUESTED,?UDF1, ?UDF2, ?UDF3, ?UDF4, ?UDF5, ?UDF6);";
                        break;
                    }
                case FeeSQLCommands.SaveStudentTransferReversalInfo:
                    {
                        query = @"INSERT INTO `FEE_RAZORPAY_REVERSAL_INFO_?AC_YEAR`
                                (
                                `ID`,
                                `ENTITY`,
                                `TRANSFER_ID`,
                                `AMOUNT`,
                                `FEE`,
                                `TAX`,
                                `CURRENCY`,
                                `INITIATOR_ID`,
                                `CUSTOMER_REFUND_ID`,
                                `CREATED_AT`)
                                VALUES
                                (?ID, ?ENTITY, ?TRANSFER_ID, ?AMOUNT, ?FEE, ?TAX, ?CURRENCY, ?INITIATOR_ID, ?CUSTOMER_REFUND_ID, ?CREATED_AT);";
                        break;
                    }

                case FeeSQLCommands.FetchStudentAccountHostelFeeInfo:
                    {
                        //query = @"SELECT 
                        //    FMH.FREQUENCY_ID,
                        //    FS.STUDENT_ID,
                        //    SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                        //    FF.FREQUENCY_NAME,
                        //    ADR.FIRST_NAME,
                        //    SFM.MAIN_HEAD AS HEAD,
                        //    ADR.SMS_MOBILE_NO,
                        //    FT.FREQUENCY_TYPE AS FREQUENCY_NAME

                        //FROM
                        //    FEE_STUDENT_ACCOUNT AS FS
                        //        INNER JOIN
                        //    ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = FS.STUDENT_ID
                        //        INNER JOIN
                        //    FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                        //        AND FMH.IS_DELETED != 1
                        //        INNER JOIN
                        //    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                        //        AND FFF.ACADEMIC_YEAR = ?AC_YEAR
                        //        AND FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY_ID
                        //        INNER JOIN
                        //    SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                        //        AND FT.FEE_ROOT_ID =?FEE_ROOT_ID
                        //        INNER JOIN
                        //    SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                        //        LEFT JOIN
                        //    SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                        //        LEFT JOIN
                        //    SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                        //WHERE
                        //    FS.STUDENT_ID = ?STUDENT_ID
                        //        AND FT.FREQUENCY_TYPE_ID =?FREQUENCY_ID
                        //        AND FS.ACADEMIC_YEAR = ?AC_YEAR
                        //        AND FS.IS_DELETED != 1
                        //GROUP BY FS.STUDENT_ID , FS.FREQUENCY_ID , FMH.MAIN_HEAD_ID";
                        query = @"SELECT 
                                            FMH.FREQUENCY_ID,
                                            FS.STUDENT_ID,
                                            SUM((IFNULL(FS.CREDIT, 0) - IFNULL(FS.DEBIT, 0))) AS BALANCE,
                                            FF.FREQUENCY_NAME,
                                            ADR.FIRST_NAME,
                                            SFM.MAIN_HEAD AS HEAD,
                                            ADR.SMS_MOBILE_NO,
                                            FT.FREQUENCY_TYPE AS FREQUENCY_NAME,
                                            PG.PROGRAMME_GROUP_ID AS PROGRAMME_ID
                                        FROM
                                            FEE_STUDENT_ACCOUNT AS FS
                                                INNER JOIN
                                            ADM_SELECTION_PROCESS_?AC_YEAR AS SP ON SP.RECEIVE_ID = FS.STUDENT_ID
                                                AND (SP.IS_DELETED != 1 AND IS_CANCELED != 1)
                                                INNER JOIN
                                            ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = FS.STUDENT_ID
                                                INNER JOIN
                                            ADM_ISSUED_APPLICATIONS AS ADI ON ADI.RECEIVE_ID = ADR.RECEIVE_ID
                                                AND ADI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                                INNER JOIN
                                            CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                                AND PG.IS_DELETED != 1
                                                INNER JOIN
                                            FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                                AND FMH.IS_DELETED != 1
                                                INNER JOIN
                                            FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                                AND FFF.ACADEMIC_YEAR = ?AC_YEAR
                                                AND FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY_ID
                                                INNER JOIN
                                            SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                                AND FT.FEE_ROOT_ID = ?FEE_ROOT_ID
                                                INNER JOIN
                                            SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                                LEFT JOIN
                                            SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                                LEFT JOIN
                                            SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                        WHERE
                                            FS.STUDENT_ID = ?STUDENT_ID
                                                AND FT.FREQUENCY_TYPE_ID = ?FREQUENCY_ID
                                                AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                                AND FS.IS_DELETED != 1
                                        GROUP BY FS.STUDENT_ID , FS.FREQUENCY_ID , FMH.MAIN_HEAD_ID";
                        break;
                    }
                case FeeSQLCommands.FetchRazorpayPaymentHostelApplicationFeeStatusForCollegeTranctions:
                    {
                        query = @"SELECT 
                                FMH.FREQUENCY_ID,
                                SP.RECEIVE_ID AS STUDENT_ID,
                                SUM((IFNULL(FC.PAID_AMOUNT, 0))) AS BALANCE,
                                FF.FREQUENCY_NAME,
                                UPPER(SP.FIRST_NAME) AS FIRST_NAME,
                                SFM.MAIN_HEAD AS HEAD_NAME,
                                SP.EMAIL_ID AS STU_EMAILID,
                                SP.SMS_MOBILE_NO AS STU_MOBILENO,
                                PR.STATUS,
                                DATE_FORMAT(F.PAYMENT_DATE, '%d/%m/%Y') AS TRANSACTION_DATE,
                                F.RECEIPT_NO,
                                RA.ACCOUNT_ID,
                                PR.ID,
                                FMH.MAIN_HEAD_ID,
                                FMH.BANK_ACCOUNT_ID,
                                F.RAZORPAY_ID,
                                PR.UDF2

                            FROM
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PR
                                    INNER JOIN
                                FEE_TRANSACTION AS F ON F.RAZORPAY_ID = PR.RAZORPAY_PAMENT_ID
                                    INNER JOIN
                                FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = F.TRANSACTION_ID
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS SP ON SP.RECEIVE_ID = PR.UDF1
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID
                                    INNER JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FMH.FREQUENCY_ID
                                    AND FFF.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FFF.FEE_FREQUENCY_FEE_MODE_ID = F.FREQUENCY
                                    INNER JOIN
                                SUP_FREQUENCY_TYPE AS FT ON FT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                    AND FT.FEE_ROOT_ID = PR.UDF5
                                    LEFT JOIN
                                SUP_FEE_FREQUENCY AS FF ON FF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    LEFT JOIN
                                SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                    LEFT JOIN
                                SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                    LEFT JOIN
                                FEE_RAZORPAY_ACCOUNTS AS RA ON RA.BANK_ACCOUNT_ID = FMH.BANK_ACCOUNT_ID
       
                            WHERE
                                PR.RAZORPAY_PAMENT_ID = ?RAZORPAY_PAMENT_ID
                            GROUP BY SP.RECEIVE_ID , F.FREQUENCY , FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case FeeSQLCommands.FetchHostelApplicationFeeStudentAccountByStudentIdAndFrequencyId:
                    {
                        query = @"SELECT 
                                FSA.STUDENT_ID,
                                FSA.FREQUENCY_ID,
                                SFM.HEAD_ID,
                                SFM.HEAD,
                                FSA.CREDIT,
                                FSA.DEBIT,
                                (IFNULL(SUM(FSA.CREDIT), 0) - IFNULL(SUM(FSA.DEBIT), 0)) AS BALANCE,
                                FSA.FEE_MAIN_HEAD_ID,
                                FS.FEE_STRUCTURE_ID,
                                FS.FEE_ROOT_ID
                            FROM
                                FEE_STUDENT_ACCOUNT AS FSA
                                    INNER JOIN
                                FEE_STRUCTURE_FOR_HOSTEL_AND_MESS AS FS ON FS.FEE_STRUCTURE_ID = FSA.FEE_STRUCTURE_ID
                                    --  AND FS.PROGRAMME = ?PROGRAMME
                                    AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FS.FREQUENCY = ?FREQUENCY_ID
                                    INNER JOIN
                                SUP_HEAD AS SFM ON SFM.HEAD_ID = FSA.HEAD
                            WHERE
                                FSA.STUDENT_ID = ?STUDENT_ID
                                    AND FSA.IS_DELETED != 1
                                    AND FSA.IS_CANCELLED_HEAD != 1
                                    AND FSA.IS_REFUND != 1
                                    AND SFM.IS_DELETED != 1
                                    AND FSA.FREQUENCY_ID = ?FREQUENCY_ID
                                    AND FSA.ACADEMIC_YEAR = ?AC_YEAR
                            GROUP BY FSA.HEAD
                            ORDER BY FSA.F_STUDENT_AC_ID ASC";
                        break;
                    }

                case FeeSQLCommands.FetchApplicationTransactionByDate:
                    {
                        query = @"SELECT 
                                        RA.RECEIVE_ID,
                                        CONCAT(CP.PROGRAMME_NAME,'-' ,PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                        CONCAT(ADI.APPLICATION_NO,LPAD(ADI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                        RA.FIRST_NAME,
                                        RA.SMS_MOBILE_NO,
                                        DATE_FORMAT(RA.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                        RA.HSC_NO,
                                        FT.TRANSACTION_ID,
                                        STUDENT_ID,
                                        DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                        FT.RECEIPT_NO,
                                        SPM.PAYMENT_MODE,
                                        COLLECTED,
                                        DISCOUNT,
                                       FT.FREQUENCY AS FREQUENCY_ID,
										FROM_UNIXTIME(FC.SETTLEMENT_DATE, '%d/%m/%Y') AS SETTLEMENT_DATE
                                    FROM    

                                        FEE_TRANSACTION AS FT
											INNER JOIN
										FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = FT.TRANSACTION_ID
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS RA ON RA.RECEIVE_ID = FT.STUDENT_ID
                                            INNER JOIN
                                        adm_issued_applications AS ADI ON ADI.RECEIVE_ID = RA.RECEIVE_ID 
                                            AND IS_PAID = 1 AND ADI.RAZORPAY_ID=FT.RAZORPAY_ID
                                            INNER JOIN
                                        cp_programme_group AS AAP ON AAP.PROGRAMME_GROUP_ID = ADI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_PAYMENT_MODE AS SPM ON SPM.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                        INNER JOIN SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID=AAP.PROGRAMME_MODE
                                        INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=AAP.SHIFT
                                    WHERE
											FC.SETTLEMENT_DATE BETWEEN UNIX_TIMESTAMP(?DATE_FROM) AND UNIX_TIMESTAMP(ADDDATE(?DATE_TO, 1))  and AAP.PROGRAMME_MODE IN (?PROGRAMME_MODE)
                                        AND    FT.FREQUENCY =?FREQUENCY_ID
                                            AND FT.IS_DELETED != 1
                                            AND ADI.RAZORPAY_ID=FT.RAZORPAY_ID
                                            AND RA.ACADEMIC_YEAR = ?AC_YEAR GROUP BY  FT.TRANSACTION_ID;";
                        break;
                    }

                case FeeSQLCommands.FetchTransactionByDate:
                    {
                        query = @"SELECT 
                                                    RA.RECEIVE_ID,
                                                    CONCAT(CP.PROGRAMME_NAME,
                                                            '-',
                                                            PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                                    ASP.APPLICATION_NO,
                                                    RA.FIRST_NAME,
                                                    RA.SMS_MOBILE_NO,
                                                    DATE_FORMAT(RA.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                                    RA.HSC_NO,
                                                    FT.TRANSACTION_ID,
                                                    STUDENT_ID,
                                                    DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                                    FT.RECEIPT_NO,
                                                    SPM.PAYMENT_MODE,
                                                    COLLECTED,
                                                    DISCOUNT,
                                                    FT.FREQUENCY AS FREQUENCY_ID,
                                                    FROM_UNIXTIME(FC.SETTLEMENT_DATE, '%d/%m/%Y') AS SETTLEMENT_DATE
                                                FROM
                                                    FEE_TRANSACTION AS FT
                                                        INNER JOIN
                                                    fee_collection AS FC ON FC.TRANSACTION_ID = FT.TRANSACTION_ID
                                                        INNER JOIN
                                                    ADM_RECEIVE_APPLICATION AS RA ON RA.RECEIVE_ID = FT.STUDENT_ID
                                                        AND RA.IS_DELETED != 1
                                                        INNER JOIN
                                                    ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID = RA.RECEIVE_ID
                                                        AND ASP.IS_DELETED != 1
                                                        INNER JOIN
                                                    CP_PROGRAMME_GROUP AS AAP ON AAP.PROGRAMME_GROUP_ID = ASP.PROGRAMME_ID
                                                        INNER JOIN
                                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                                        INNER JOIN
                                                    SUP_PAYMENT_MODE AS SPM ON SPM.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                                                        INNER JOIN
                                                    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                                                        INNER JOIN
                                                    SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                                        INNER JOIN
                                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = AAP.PROGRAMME_MODE
                                                        INNER JOIN
                                                    SUP_SHIFT AS SH ON SH.SHIFT_ID = AAP.SHIFT
                                                WHERE
                                                    FC.SETTLEMENT_DATE BETWEEN UNIX_TIMESTAMP(?DATE_FROM) AND UNIX_TIMESTAMP(ADDDATE(?DATE_TO, 1))
                                                        AND AAP.PROGRAMME_MODE IN (?PROGRAMME_MODE)
                                                        AND FT.FREQUENCY = ?FREQUENCY_ID
                                                        AND FT.IS_DELETED != 1
                                                        AND RA.ACADEMIC_YEAR = ?AC_YEAR
                                                GROUP BY FT.TRANSACTION_ID;";


                        // FOR REFUNDED


                        //query = @"SELECT 
                        //                                                    RA.RECEIVE_ID,
                        //                                                    CONCAT(CP.PROGRAMME_NAME,
                        //                                                            '-',
                        //                                                            PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                        //                                                    ASP.APPLICATION_NO,
                        //                                                    RA.FIRST_NAME,
                        //                                                    RA.SMS_MOBILE_NO,
                        //                                                    DATE_FORMAT(RA.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                        //                                                    RA.HSC_NO,
                        //                                                    FT.TRANSACTION_ID,
                        //                                                    STUDENT_ID,
                        //                                                    DATE_FORMAT(PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                        //                                                    FT.RECEIPT_NO,
                        //                                                    SPM.PAYMENT_MODE,
                        //                                                    COLLECTED,
                        //                                                    DISCOUNT,
                        //                                                    FT.FREQUENCY AS FREQUENCY_ID,
                        //                                                    FROM_UNIXTIME(FC.SETTLEMENT_DATE, '%d/%m/%Y') AS SETTLEMENT_DATE
                        //                                                FROM
                        //                                                    FEE_TRANSACTION AS FT
                        //                                                        INNER JOIN
                        //                                                    fee_collection AS FC ON FC.TRANSACTION_ID = FT.TRANSACTION_ID
                        //                                                        INNER JOIN
                        //                                                    ADM_RECEIVE_APPLICATION AS RA ON RA.RECEIVE_ID = FT.STUDENT_ID
                        //                                                        AND RA.IS_DELETED != 1
                        //                                                        INNER JOIN
                        //                                                    ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID = RA.RECEIVE_ID
                        //                                                        AND ASP.IS_DELETED != 1
                        //                                                        INNER JOIN
                        //                                                    CP_PROGRAMME_GROUP AS AAP ON AAP.PROGRAMME_GROUP_ID = ASP.PROGRAMME_ID
                        //                                                        INNER JOIN
                        //                                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                        //                                                        INNER JOIN
                        //                                                    SUP_PAYMENT_MODE AS SPM ON SPM.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                        //                                                        INNER JOIN
                        //                                                    FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                        //                                                        INNER JOIN
                        //                                                    SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                        //                                                        INNER JOIN
                        //                                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = AAP.PROGRAMME_MODE
                        //                                                        INNER JOIN
                        //                                                    SUP_SHIFT AS SH ON SH.SHIFT_ID = AAP.SHIFT
                        //                                                WHERE
                        //                                               --       FC.SETTLEMENT_DATE BETWEEN UNIX_TIMESTAMP(?DATE_FROM) AND UNIX_TIMESTAMP(ADDDATE(?DATE_TO, 1)) AND
                        //                                                         AAP.PROGRAMME_MODE IN (?PROGRAMME_MODE)
                        //                                                        AND FT.FREQUENCY = ?FREQUENCY_ID
                        //                                                        AND FT.IS_DELETED != 1
                        //                                                        AND RA.ACADEMIC_YEAR = ?AC_YEAR AND FT.RECEIPT_NO IN (002463)
                        //                                                GROUP BY FT.TRANSACTION_ID;";

                        break;
                    }
                case FeeSQLCommands.FetchFeeReceiptByTransactionIdAndHead:
                    {
                        query = @"SELECT 
                                        FT.TRANSACTION_ID,
                                        FT.FREQUENCY,
                                        FMH.MAIN_HEAD_ID,
                                        SFT.FREQUENCY_TYPE AS FREQUENCY_NAME,
                                        MH.MAIN_HEAD,FT.FEE_TRANSACTION_BANK,
                                        SUM(IFNULL(PAID_AMOUNT, 0)) AS DEBIT,SFT.FREQUENCY_TYPE_ID
                                    FROM
                                        FEE_TRANSACTION AS FT
                                            INNER JOIN
                                        FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = FT.TRANSACTION_ID
                                            INNER JOIN
                                        FEE_MAIN_HEADS AS FMH ON FMH.FEE_MAIN_HEAD_ID = FC.FEE_MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FMH.HEAD_ID
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FT.FREQUENCY
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    WHERE
                                            FT.TRANSACTION_ID = ?TRANSACTION_ID
                                            AND FC.IS_DELETED != 1
                                            AND FT.IS_DELETED !=1
                                            AND FC.FREQUENCY = ?FREQUENCY_ID  AND FMH.MAIN_HEAD_ID IN (?MAIN_HEAD_ID)
                                    GROUP BY FMH.MAIN_HEAD_ID;";
                        break;
                    }
            }
            return query;
        }
    }
}
