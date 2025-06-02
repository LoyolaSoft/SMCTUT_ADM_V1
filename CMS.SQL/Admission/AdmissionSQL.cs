using CMS.DAO;

namespace CMS.SQL.Admission
{
    public static class AdmissionSQL
    {
        public static string GetAdmissionSQL(AdmissionSQLCommands sEnumCommand)
        {
            string query = string.Empty;
            switch (sEnumCommand)
            {

                case AdmissionSQLCommands.FetchAppliedApplicationInfo:
                    {
                        //query = @"SELECT 
                        //            CONCAT(IA.
                        //
                        //            ,LPAD(IA.ISSUED_ID,4,'0') ) AS APPLICATION_NO,
                        //            CONCAT(P.PROGRAMME_NAME,
                        //                    '-',
                        //                    PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                        //            S.STATUS_NAME AS STATUS,
                        //            PTM.PAYMENT_MODE
                        //        FROM
                        //            ADM_ISSUED_APPLICATIONS AS IA
                        //                INNER JOIN
                        //            CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = IA.PROGRAMME_GROUP_ID
                        //                INNER JOIN
                        //            CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                        //                INNER JOIN
                        //            SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                        //                INNER JOIN
                        //            SUP_SHIFT AS SH ON SH.SHIFT_ID = PG.SHIFT
                        //                INNER JOIN
                        //            SUP_ADM_STATUS AS S ON S.STATUS_ID = IA.STATUS
                        //                INNER JOIN
                        //            SUP_PAYMENT_MODE AS PTM ON PTM.PAYMENT_MODE_ID = IA.PAYMENT_MODE
                        //        WHERE
                        //                IA.RECEIVE_ID = ?RECEIVE_ID
                        //                AND IA.ACADEMIC_YEAR = ?AC_YEAR
                        //                AND IS_PAID = 1;";


                        query = @"SELECT 
                                    CONCAT(IA.APPLICATION_NO,
                                            LPAD(IA.ISSUE_NO, 4, '0')) AS APPLICATION_NO,
                                    CONCAT(P.PROGRAMME_NAME, '-', PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                    S.STATUS_NAME AS STATUS,
                                    PTM.PAYMENT_MODE,
                                    IA.STATUS AS STATUS_ID,
                                    DATE_FORMAT(SC.START_DATE, '%d/%m/%Y') AS START_DATE,
                                    if(curdate() between SC.START_DATE and END_DATE,'1','0') AS IS_BLOCKED
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS IA
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = IA.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                        INNER JOIN
                                    SUP_SHIFT AS SH ON SH.SHIFT_ID = PG.SHIFT
                                        INNER JOIN
                                    SUP_ADM_STATUS AS S ON S.STATUS_ID = IA.STATUS
                                        INNER JOIN
                                    SUP_PAYMENT_MODE AS PTM ON PTM.PAYMENT_MODE_ID = IA.PAYMENT_MODE
                                        INNER JOIN
                                    adm_selection_setting AS SC ON SC.ACADEMIC_YEAR = IA.ACADEMIC_YEAR AND  SC.APPTYPE_ID=PG.APPTYPE_ID
                                        AND SC.IS_ACTIVE = 1
                                WHERE
                                    IA.RECEIVE_ID = ?RECEIVE_ID AND SC.IS_DELETED!=1
                                        AND IA.ACADEMIC_YEAR = ?AC_YEAR
                                        AND IS_PAID = 1;";
                        break;
                    }

                case AdmissionSQLCommands.SelectInsertsForApplicationFee:
                    {
                        query = @"INSERT INTO fee_student_account  (STUDENT_ID,ACADEMIC_YEAR,FREQUENCY_ID,HEAD,CREDIT,FEE_MAIN_HEAD_ID,FEE_STRUCTURE_ID,FEE_ROOT_ID )

                                SELECT 
                                    '?STUDENT_ID' as STUDENT_ID,
                                    ACADEMIC_YEAR,
                                    FREQUENCY,
                                    HEAD,
                                    AMOUNT,
                                    FEE_MAIN_HEAD_ID,
                                    FEE_STRUCTURE_ID,
                                    FEE_ROOT_ID
                                FROM
                                    fee_structure AS fs
                                WHERE
                                    fS.ACADEMIC_YEAR = ?AC_YEAR
                                        AND FS.FREQUENCY =?FREQUENCY_ID
                                        AND FS.PROGRAMME = ?PROGRAMME
                                        AND FS.CLASS_YEAR_ID = ?CLASS_YEAR_ID;";
                        break;
                    }
                case AdmissionSQLCommands.SelectApplicationFee:
                    {
                        query = @"SELECT 
                                    '?STUDENT_ID' as STUDENT_ID,
                                    ACADEMIC_YEAR,
                                    FREQUENCY,
                                    HEAD,
                                    AMOUNT,
                                    FEE_MAIN_HEAD_ID,
                                    FEE_STRUCTURE_ID,
                                    FEE_ROOT_ID
                                FROM
                                    fee_structure AS fs
                                WHERE
                                    fS.ACADEMIC_YEAR = ?AC_YEAR
                                        AND FS.FREQUENCY =?FREQUENCY_ID
                                        AND FS.PROGRAMME = ?PROGRAMME
                                        AND FS.CLASS_YEAR_ID = ?CLASS_YEAR_ID;";
                        break;
                    }
                case AdmissionSQLCommands.SaveIssueAppicationsAfterPayment:
                    {

                        query = @"INSERT INTO ADM_ISSUED_APPLICATIONS(RECEIVE_ID,PROGRAMME_GROUP_ID,STATUS,IS_PAID,PAYMENT_MODE,RAZORPAY_ID,ACADEMIC_YEAR)VALUES(?RECEIVE_ID,?PROGRAMME_GROUP_ID,?STATUS,?IS_PAID,?PAYMENT_MODE,?RAZORPAY_ID,?AC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.GetApplicationTypeById:
                    {

                        query = @"SELECT 
                                    R.APPLICATION_TYPE_ID, R.RECEIVE_ID,R.GENDER
                                FROM
                                    ADM_RECEIVE_APPLICATION R
                                WHERE
                                    R.ACADEMIC_YEAR = ?AC_YEAR
                                        AND R.RECEIVE_ID = ?RECEIVE_ID ;";
                        break;
                    }
                case AdmissionSQLCommands.GetHostelFacilityByReceiveId:
                    {
                        query = @"SELECT 
                                        RECEIVE_ID, HOSTEL_FACILITY
                                    FROM
                                        ADM_RECEIVE_APPLICATION
                                    WHERE
                                        IS_DELETED != 1 AND RECEIVE_ID = ?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelFeePaidStatus:
                    {
                        query = @"SELECT 
                                        STUDENT_ID,TRANSACTION_ID,FREQUENCY AS FREQUENCY_ID 
                                    FROM
                                        fee_transaction
                                    WHERE
                                        FREQUENCY = ?FREQUENCY_ID AND STUDENT_ID = ?STUDENT_ID;";
                        break;
                    }
                case AdmissionSQLCommands.VerifiedStudentList:
                    {
                        query = @"SELECT 
                                UPPER(CONCAT(RE.FIRST_NAME,'-',ISS.APPLICATION_NO,LPAD(IA.ISSUE_NO,4,'0'))) AS FIRST_NAME,
                                CONCAT(ISS.APPLICATION_NO,LPAD(ISS.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                ISS.ISSUED_ID,
                                RE.RECEIVE_ID
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS RE ON RE.RECEIVE_ID = SP.RECEIVE_ID
                                    INNER JOIN
                                adm_issued_applications AS ISS ON ISS.RECEIVE_ID = RE.RECEIVE_ID
                                    AND ISS.IS_DELETED != 1
                            WHERE
                                SP.IS_VERIFIED = 1 AND ISS.STATUS = 4
                                    AND (SP.IS_DELETED != 1 AND SP.IS_CANCELED!=1);";
                        break;
                    }

                case AdmissionSQLCommands.FetchIssueApplicationByReceivedId:
                    {
                        query = @"SELECT 
                                            ADR.RECEIVE_ID,
                                            ADR.APPLICATION_TYPE_ID,
                                            AD.PROGRAMME_GROUP_ID,
                                            CP.PROGRAMME_NAME,
                                            CP.PROGRAMME_CODE,
                                            DATE_FORMAT(ADR.RECEIVE_DATE, '%d/%m/%Y') AS RECEIVE_DATE,
                                            CONCAT(AD.APPLICATION_NO,LPAD(AD.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                            ADR.FIRST_NAME,
                                            ADR.AGE,
                                            AD.IS_DELETED,
                                            ADR.SMS_MOBILE_NO,
                                            DATE_FORMAT(ADR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                            ADR.HSC_NO,
                                            AD.PROGRAMME_GROUP_ID,
                                            ADR.FATHER_NAME,
                                            ADR.RELIGION_ID,
                                            ADR.MOTHER_TONGUE,
                                            ADR.COMMUNITY_ID,
                                            ADR.OCCUPATION,
                                            ADR.ANNUAL_INCOME,
                                            ADR.MARITAL_STATUS_ID,
                                            ADR.EXSERVICE_MAN,
                                            ADR.EDUCATION_BOARD_ID,
                                            ADR.SPECIALLYABLED_ID,
                                            ADR.GENDER,
                                            ADR.IS_FIRSTGENERATION,
                                            ADR.CDOORDETAIL,
                                            ADR.PDOORDETAIL,
                                            ADR.CSTREET,
                                            ADR.PSTREET,
                                            ADR.PPOST_PLACE_TOWN,
                                            ADR.CTALUK_CITY,
                                            ADR.PTALUK_CITY,
                                            ADR.CPINCODE,
                                            ADR.PPINCODE,
                                            ADR.CDISTRICT,
                                            ADR.PDISTRICT,
                                            ADR.CCOUNTRY,
                                            ADR.PCOUNTRY,
                                            ADR.PVILLAGE_AREA,
                                            ADR.CPHONENO,
                                            ADR.PPHONENO,
                                            ADR.HSTOTAL,
                                            ADR.HSPERCENTAGE,
                                            ADR.ACADEMIC_YEAR,
                                            ADR.YEAR_OF_PASSING,
                                            CPG.PROGRAMME_ID,
                                            CPG.PROGRAMME_MODE,
                                            ADR.LAST_STUDIED_SCHOOL,
                                           -- ADR.UNIVERSITY,
                                            ADR.IS_REGISTERED_TENANT,
                                            ADR.PLACE_OF_BIRTH,
                                            ADR.HOSTEL_FACILITY,
                                            ADR.IS_ROMAN_CATHOLIC,
                                            ADR.PHOTO_PATH,
                                            ADR.IS_SINGLE_GIRL_CHILD,
                                            ADR.IS_SUBMITTED,
                                            ADR.IS_DALIT,
                                            ADR.LAST_STUDIED_PLACE,
                                            ADR.OCCUPATION,
                                            ASP.IS_VERIFIED,
                                            ADR.LAST_STUDIED_CLASS,
                                            ADR.STATUS,
                                            AHR.ROOM_NO,
                                            AHR.PARISH_NAME,
                                            AHR.COLLEGE_RELATION,
                                            AHR.STUDENT_INFO,
                                            AHR.FAMILY_PHOTO,
                                            SAS.STATUS_NAME,
                                            AHR.HOSTEL_REGISTRATION_ID,
                                            AHR.IS_SUBMITTED AS HOSTEL_SUBMITTED,
                                            ADR.BLOOD_GROUP,
                                            ADR.EXTRA_CURRICULAR,
                                            AAT.APPLICATION_TYPE,
                                            CO.COUNTRY_NAME,
                                            SU.UNIVERSITY,
                                            CM.COMMUNITY
                                        FROM
                                            ADM_ISSUED_APPLICATIONS AS AD
                                                INNER JOIN
                                            ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = AD.RECEIVE_ID
                                                AND AD.PROGRAMME_GROUP_ID = PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AD.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                                INNER JOIN
                                            ADM_APPLICATION_TYPE AS AAT ON AAT.APPLICATION_TYPE_ID = ADR.APPLICATION_TYPE_ID
                                                LEFT JOIN
                                            ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID = AD.RECEIVE_ID AND (ASP.IS_DELETED!=1 AND ASP.IS_CANCELED!=1)
                                                LEFT JOIN
                                            ADM_HOSTEL_REGISTRATION AS AHR ON AHR.RECEIVE_ID = AD.RECEIVE_ID
                                                LEFT JOIN
                                            SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID = AD.STATUS
                                                LEFT JOIN
                                            SUP_COUNTRY AS CO ON CO.COUNTRY_ID = ADR.CCOUNTRY
                                                LEFT JOIN
                                            SUP_UNIVERSITY AS SU ON SU.UNIVERSITY_ID = ADR.UNIVERSITY
                                                LEFT JOIN
                                            SUP_COMMUNITY AS CM ON CM.COMMUNITYID = ADR.CASTE_ID
                                        WHERE
                                            ADR.RECEIVE_ID =?RECEIVE_ID
                                                AND AD.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchApplicationFormByProgrammeId:
                    {
                        query = @"SELECT 
	                                R.RECEIVE_ID,
                                    CONCAT(I.APPLICATION_NO,LPAD(I.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    UPPER(R.FIRST_NAME) AS FIRST_NAME,
                                    SMS_MOBILE_NO,
                                    PG.PROGRAMME_GROUP_ID,
                                    UPPER(CONCAT(IFNULL(P.PROGRAMME_NAME,''))) AS PROGRAMME_NAME,
                                    IFNULL(DATE_FORMAT(R.DATE_OF_BIRTH,'%d-%m-%Y'),' - ') AS DATE_OF_BIRTH,
                                    GEN.GENDER_NAME,
                                    BG.BLOOD_GROUP_NAME,
                                    COM.COMMUNITY,
	                                R.COMMUNITY_ID,
                                    IF(R.IS_ROMAN_CATHOLIC = 1, 'YES', 'NO') AS IS_ROMAN_CATHOLIC, 
                                    RI.RELIGION,
                                    IF(R.IS_DALIT =1,'YES','NO') AS DALIT,
                                    N.NATIONALITY_NAME,
                                    R.AADHAR_NUMBER,
                                    MT.MOTHER_TONGUE_NAME AS 'LANGUAGE_NAME',
                                    SS.STATE,
                                    R.CDISTRICT AS CDISTRICT1,
                                    R.CPOST_PLACE_TOWN AS CPOST_PLACE_TOWN1,
                                    R.CVILLAGE_AREA AS CVILLAGE_AREA1,
                                    R.IS_NRI,
                                    R.PASSPORT_NUMBER,
                                    R.FATHER_NAME,
                                    OCC.OCCUPATION_NAME,
                                    MOCC.OCCUPATION_NAME AS 'MOTHER_OCCUPATION',
                                    R.ANNUAL_INCOME,
                                    R.MOTHER_NAME,
                                    IF(R.IS_FIRSTGENERATION =1,'YES','NO') AS IS_FIRST_GENERATION,
                                    IF(R.EXSERVICE_MAN =1,'YES','NO') AS EXSERVICE_MAN,
                                    IF(R.EXSERVICE_MAN =1,'APPLICABLE','NOT APPLICABLE') AS EXSERVICE_MAN_APPLICABLE,
                                    R.LAST_STUDIED_PLACE,
                                    R.LAST_STUDIED_SCHOOL,
                                     IF(R.SPECIALLYABLED_ID =1,'YES','NO') AS SPECIALLYABLED,
                                    R.EXTRA_CURRICULAR,
                                    R.LAST_STUDIED_CLASS,
                                    IF(R.MARITAL_STATUS_ID =1,'YES','NO') AS MARITAL_STATUS,
                                    IF(R.HOSTEL_FACILITY =1,'YES','NO') AS HOSTEL_FACILITY,
                                    IF(R.EDUCATION_BOARD_ID =1,'STATEBOARD',if(R.EDUCATION_BOARD_ID =0,'CBSC','')) AS EDUCATION_BOARD,
                                    R.HSC_NO,
                                    R.NAME_IN_NATIVE,
                                    R.MOTHER_INCOME,
                                    R.MOTHER_OCCUPATION,
                                    R.OCCUPATION,
                                    -- R.MOTHER_NAME_IN_NATIVE,
                                    -- R.MEDIUM_OF_STUDY,
                                    R.SPORTS,
                                    CONCAT(IFNULL(CDOORDETAIL,''),' ',IFNULL(CSTREET,'')) AS CDOORDETAIL,
                                    CONCAT(IFNULL(CPOST_PLACE_TOWN,''),' ',IFNULL(CVILLAGE_AREA,'')) AS CVILLAGE_AREA,
                                    CONCAT(IFNULL(CDISTRICT,''),' , ',SS.STATE) AS CDISTRICT,
                                    CPINCODE,
                                    CPHONENO,
                                    CMOBILENO,
                                    CONCAT(IFNULL(PDOORDETAIL,''),' ',IFNULL(PSTREET,'')) AS PDOORDETAIL,
                                    CONCAT(IFNULL(PPOST_PLACE_TOWN,''),' ',IFNULL(PVILLAGE_AREA,'')) AS PVILLAGE_AREA,
                                    CONCAT(IFNULL(PDISTRICT,''),' , ',SS.STATE) AS PDISTRICT,
                                    PPINCODE,
                                    PMOBILENO,
                                    PPHONENO,
                                    EMAIL_ID,
                                   -- CONCAT(IFNULL(HSS_GROUP_CODE,''),' -  ') AS HSS_GROUP,
                                    HSTOTAL,HSPERCENTAGE,HS_MAX_MARK,TOTAL_CUT_OFF_MARK,
                                    ATT.APPLICATION_TYPE,
                                    -- R.SPECIALLYABLED_TYPE,
                                    R.PARISHI_FRC,
                                    PHOTO_PATH,
                                   --  R.NO_OF_SEMESTERS,
                                   --  IFNULL(R.IS_ARREAR,'NO') AS IS_ARREAR,
                                    PG.PROGRAMME_GROUP_ID 
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS I
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS R ON R.RECEIVE_ID = I.RECEIVE_ID
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = I.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
		                                LEFT JOIN
	                                SUP_GENDER AS GEN ON GEN.GENDER_ID=R.GENDER
		                                INNER JOIN
	                                SUP_APPLICATION_TYPE AS ATT ON ATT.APPLICATION_TYPE_ID=R.APPLICATION_TYPE_ID
		                                LEFT JOIN
	                                SUP_COMMUNITY AS COM ON COM.COMMUNITYID=R.CASTE_ID
		                                LEFT JOIN
	                                SUP_NATIONALITY AS N ON N.NATIONALITY_ID=R.NATIONALITY_ID
		                                LEFT JOIN
	                                SUP_RELIGION AS RI ON RI.RELIGIONID=R.RELIGION_ID
		                                LEFT JOIN
	                                SUP_MOTHER_TONGUE AS MT ON MT.MOTHER_TONGUE_ID=R.MOTHER_TONGUE
		                                LEFT JOIN
	                                SUP_OCCUPATION AS OCC ON OCC.OCCUPATION_ID=R.OCCUPATION
                                        LEFT JOIN
	                                SUP_OCCUPATION AS MOCC ON MOCC.OCCUPATION_ID=R.MOTHER_OCCUPATION
		                                LEFT JOIN
	                                SUP_MARRITAL_STATUS AS M ON M.MARITAL_STAUS_ID=R.MARITAL_STATUS_ID
		                                LEFT JOIN
	                                SUP_STATE AS SS ON SS.STATE_ID=R.CSTATE
		                                LEFT JOIN
	                                SUP_COUNTRY AS SC ON SC.COUNTRY_ID=R.CCOUNTRY
		                                LEFT JOIN
	                                SUP_STATE AS S ON S.STATE_ID=R.PSTATE
		                                LEFT JOIN
	                                SUP_COUNTRY AS CON ON CON.COUNTRY_ID=R.PCOUNTRY
		                                LEFT JOIN
	                                SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID=R.BLOOD_GROUP
		                               
                                WHERE I.RECEIVE_ID=?RECEIVE_ID AND  PG.PROGRAMME_GROUP_ID=?PROGRAMME_GROUP_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmittedStudentListByProgrammeId:
                    {
                        query = @"SELECT 
                                                AR.RECEIVE_ID,
                                                AR.FIRST_NAME,
                                                SL.APPLICATION_NO,
                                                AR.SMS_MOBILE_NO,
                                                CP.PROGRAMME_NAME,
                                                PM.PROGRAMME_MODE,
                                                sl.PROGRAMME_ID
   
                                            FROM
                                                fee_transaction AS ft
                                                    INNER JOIN
                                                adm_selection_process_?AC_YEAR AS sl ON sl.RECEIVE_ID = ft.STUDENT_ID
                                                    AND SL.IS_DELETED != 1
                                                    INNER JOIN
                                                adm_receive_application AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                                    INNER JOIN
                                                cp_programme_group AS pg ON pg.PROGRAMME_GROUP_ID = sl.PROGRAMME_ID
                                                    INNER JOIN
                                                cp_programme AS cp ON cp.PROGRAMME_ID = pg.PROGRAMME_ID
                                                    INNER JOIN
                                                sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                            WHERE
                                                ft.FREQUENCY = ?FREQUENCY_ID and sl.PROGRAMME_ID=?PROGRAMME_ID  order by PG.PROGRAMME_GROUP_ID,SL.APPLICATION_NO; ";
                        break;
                    }
                case AdmissionSQLCommands.UpdateStatusByReceiveId:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS SET STATUS =?STATUS WHERE RECEIVE_ID=?RECEIVE_ID  AND PROGRAMME_GROUP_ID=?PROGRAMME_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateSelectedStatusByReceiveId:
                    {
                        //query = @"UPDATE ADM_ISSUED_APPLICATIONS SET STATUS =?STATUS WHERE RECEIVE_ID=?RECEIVE_ID  AND PROGRAMME_GROUP_ID=?PROGRAMME_ID;UPDATE adm_waiting_application_?AC_YEAR SET STATUS=?STATUS WHERE RECEIVE_ID=?RECEIVE_ID ;";
                        query = @"UPDATE adm_waiting_application_?AC_YEAR SET STATUS =?STATUS WHERE RECEIVE_ID=?RECEIVE_ID  AND PROGRAMME_ID=?PROGRAMME_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateStatusByReceiveIdInHostelselection:
                    {
                        query = @"UPDATE ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR SET STATUS =?STATUS WHERE RECEIVE_ID=?RECEIVE_ID  AND PROGRAMME_GROUP_ID=?PROGRAMME_ID;";
                        break;
                    }
                case AdmissionSQLCommands.IsExistingUser:
                    {
                        query = @"SELECT 
                                    COUNT(*) > 0 AS STATUS
                                FROM
                                    ADM_ISSUE_APPLICATION AS I
                                WHERE
                                    I.IS_PAID = 1 AND HSC_NO =?HSC_NO AND PROGRAMME_GROUP_ID=?PROGRAMME_GROUP_ID AND IS_DELETED!=1 ";
                        break;
                    }

                case AdmissionSQLCommands.FetchPayUresponseByTxnid:
                    {

                        query = @"SELECT 
    PAYU_RESPONSE_ID,
    `key`,
    txnid,
    amount,
    productinfo,
    firstname,
    email,
    phone,
    lastname,
    address1,
    address2,
    city,
    state,
    country,
    zipcode,
    udf1,
    udf2,
    udf3,
    udf4,
    udf5,
    udf6,
    udf7,
    udf8,
    udf9,
    udf10,
    hash,
    payment_source,
    PG_TYPE,
    bank_ref_num,
    bankcode,
    error,
    error_Message,
    name_on_card,
    cardnum,
    mode,
    mihpayid,
    status,
    unmappedstatus,
    addedon,
    additionalCharges
FROM
    fee_payu_response_?AC_YEAR
WHERE
    txnid =?TXNID;";
                        break;
                    }

                case AdmissionSQLCommands.FetchUserAccountByUserId:
                    {
                        query = @"SELECT 
                                    USER_ACCOUNT_ID,
                                    USERNAME,
                                    PASSWORD,
                                    ROLE,
                                    NAME,
                                    EMAIL,
                                    MOBILE,
                                    LAST_LOGIN,
                                    IS_DELETED,
                                    IS_ACTIVE,
                                    USER_ID,
                                    USER_TYPE,
                                    PHOTO
                                FROM
                                    user_account
                                WHERE
                                    USER_ID = ?USER_ID;";


                        break;
                    }

                case AdmissionSQLCommands.FetchForgotApplicationNo:
                    {

                        query = @"SELECT 
                                        IA.APPLICATION_NO, FIRST_NAME, PRO.PROGRAMME_NAME
                                    FROM
                                        ADM_ISSUE_APPLICATION AS IA
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS AG ON AG.PRO_GROUPS_ID = IA.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS PRO ON AG.PROGRAMME_ID = PRO.PROGRAMME_ID
                                    WHERE
                                        IA.HSC_NO = ?HSC_NO
                                            AND IA.DOB = ?DOB
                                            AND IS_PAID = 1 AND IA.IS_DELETED!=1;";

                        return query;
                    }
                case AdmissionSQLCommands.FetchIssuedApplications:
                    {
                        query = @"SELECT 
                                    iss.APPLICATION_NO,
                                    iss.FIRST_NAME,
                                    pro.PROGRAMME_NAME,
                                    pm.PROGRAMME_MODE,
                                    s.SHIFT_NAME,
                                    aat.APP_FEE,date_format(ISSUE_DATE,'%d/%m/%Y') as ISSUE_DATE,AAT.APPLICATION_TYPE
                                FROM
                                    adm_issue_application AS iss
                                        INNER JOIN
                                    adm_apptype_prog_groups AS ap ON ap.PRO_GROUPS_ID = iss.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    sup_programme_mode AS pm ON pm.PROGRAMME_MODE_ID = ap.PROGRAMME_MODE
                                        INNER JOIN
                                    cp_programme_2018 AS pro ON pro.PROGRAMME_ID = ap.PROGRAMME_ID
                                        INNER JOIN
                                    adm_application_type AS aat ON aat.APPLICATION_TYPE_ID = ap.APPTYPE_ID
                                        LEFT JOIN
                                    SUP_SHIFT AS s ON ap.SHIFT = s.SHIFT_ID
                                WHERE
                                    iss.IS_PAID = 1 AND iss.IS_DELETED!=1 and ISSUE_DATE between ?FROM_DATE and ?TO_DATE;";
                        break;
                    }

                case AdmissionSQLCommands.FetchCoursesOffered:
                    {
                        query = @"SELECT 
                                    AGP.APPTYPE_ID,
                                    AGP.PRO_GROUPS_ID,
                                    CP.PROGRAMME_ID,
                                    CP.PROGRAMME_NAME,
                                    AGP.SHIFT,
                                    SH.SHIFT_NAME,
                                    APT.APPLICATION_TYPE_ID,
                                    APT.APPLICATION_TYPE,
                                    APT.APPLICATION_COST,
                                    SPM.PROGRAMME_MODE_ID,
                                    SPM.PROGRAMME_MODE
                                FROM
                                    ADM_APPTYPE_PROG_GROUPS AS AGP
                                        INNER JOIN
                                    ADM_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = AGP.APPTYPE_ID
                                        INNER JOIN
                                    SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = AGP.PROGRAMME_MODE
                                        INNER JOIN
                                    SUP_SHIFT AS SH ON SH.SHIFT_ID = AGP.SHIFT
                                        INNER JOIN
                                    CP_PROGRAMME_2018 AS CP ON CP.PROGRAMME_ID = AGP.PROGRAMME_ID
                                WHERE
                                    APT.ACADEMIC_YEAR = 2018;";
                        break;
                    }
                case AdmissionSQLCommands.FetchTempCourse:
                    {
                        //query = @"SELECT 
                        //            AGP.APPTYPE_ID,
                        //            AGP.PRO_GROUPS_ID,
                        //            CP.PROGRAMME_ID,
                        //            CP.PROGRAMME_NAME,
                        //            AGP.SHIFT,
                        //            SH.SHIFT_NAME,
                        //            APT.APPLICATION_TYPE_ID,
                        //            APT.APPLICATION_TYPE,
                        //            APT.APPLICATION_COST,
                        //            SPM.PROGRAMME_MODE_ID,
                        //            SPM.PROGRAMME_MODE,PL.PROGRAMME_LEVEL,PL.PROGRAMME_LEVEL_ID,AGP.IS_NEW,SH.TIME
                        //        FROM
                        //            ADM_APPTYPE_PROG_GROUPS AS AGP
                        //                INNER JOIN
                        //            ADM_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = AGP.APPTYPE_ID
                        //                INNER JOIN
                        //            SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = AGP.PROGRAMME_MODE
                        //                INNER JOIN
                        //            SUP_SHIFT AS SH ON SH.SHIFT_ID = AGP.SHIFT
                        //                INNER JOIN
                        //            CP_PROGRAMME_2018 AS CP ON CP.PROGRAMME_ID = AGP.PROGRAMME_ID
                        //            inner join cp_degree_2018 as d on d.DEGREE_ID=CP.DEGREE
                        //            INNER JOIN sup_programme_level AS PL ON PL.PROGRAMME_LEVEL_ID=d.PROGRAMME_LEVEL
                        //        WHERE
                        //            APT.ACADEMIC_YEAR = 2018;";
                        query = @"SELECT 
                                        PG.PROGRAMME_GROUP_ID,
                                        CONCAT(P.PROGRAMME_NAME,
                                                ' - ',
                                                PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                        APT.APPLICATION_TYPE,
                                        SH.DESCRIPTION as SHIFT_NAME,
                                        SH.TIME,
                                        pg.IS_NEW
                                    FROM
                                        ADM_PROGRAMME_GROUP AS PG
                                            INNER JOIN
                                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = PG.SHIFT
                                    WHERE
                                        pg.IS_DELETED != 1 AND apt.IS_ACTIVE = 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchCoursesByApplicationTypeId:
                    {
                        //query = @"SELECT 
                        //                PG.PROGRAMME_GROUP_ID,
                        //                CONCAT(P.PROGRAMME_NAME,
                        //                        ' - ',
                        //                        PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                        //                APT.APPLICATION_TYPE,
                        //                SH.DESCRIPTION as SHIFT_NAME,
                        //                SH.TIME,
                        //                pg.IS_NEW
                        //            FROM
                        //                ADM_PROGRAMME_GROUP AS PG
                        //                    INNER JOIN
                        //                CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                        //                    INNER JOIN
                        //                SUP_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                        //                    INNER JOIN
                        //                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                        //                    INNER JOIN
                        //                SUP_SHIFT AS SH ON SH.SHIFT_ID = PG.SHIFT
                        //            WHERE
                        //                pg.IS_DELETED != 1 AND apt.IS_ACTIVE = 1 AND PG.APPTYPE_ID=?APPLICATION_TYPE_ID;";
                        //break;
                        query = @"SELECT 
                                PG.PROGRAMME_GROUP_ID,
                                CONCAT(P.PROGRAMME_NAME,
                                        ' - ',
                                        PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                APT.APPLICATION_TYPE,
                                SH.DESCRIPTION AS SHIFT_NAME,
                                SH.TIME,
                                PG.IS_NEW
                            FROM
                                CP_PROGRAMME_GROUP AS PG
                                    INNER JOIN
                                CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_SHIFT AS SH ON SH.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_GENDER_SEPARATION AS SGS ON SGS.PROGRAMME_GROUP_ID = PG.PROGRAMME_GROUP_ID
                            WHERE
                                PG.IS_DELETED != 1 AND APT.IS_ACTIVE = 1
                                    AND PG.APPTYPE_ID = ?APPLICATION_TYPE_ID AND SGS.GENDER_ID=?GENDER
                            ORDER BY PG.PROGRAMME_ORDER;";
                        break;

                    }
                case AdmissionSQLCommands.IsExistEmailId:
                    {

                        query = @"SELECT 
                                COUNT(EMAIL_ID) > 0 as STATUS,RECEIVE_ID
                            FROM
                                ADM_RECEIVE_APPLICATION
                            WHERE
                                EMAIL_ID = ?EMAIL_ID AND IS_DELETED!=1;";
                        break;
                    }

                case AdmissionSQLCommands.SaveApplicationNo:
                    {

                        query = @"INSERT INTO `ADM_APPLICATIONSERIALNO`
                                    (
                                    `ISSUE_ID`,
                                    `ACADEMIC_YEAR`,
                                    `DEPT_CODE`)
                                    VALUES
                                    (
                                    ?ISSUE_ID,
                                    ?ACADEMIC_YEAR,
                                    ?DEPT_CODE);";
                        break;
                    }
                case AdmissionSQLCommands.UpdatePaidStatusInIssueApplication:
                    {

                        query = @"UPDATE ADM_ISSUE_APPLICATION 
                                    SET ISSUE_DATE=CURRENT_DATE(),APPLICATION_NO=?APPLICATION_NO,IS_ONLINE_APPLICANT='1' , 
                                    IS_PAID=1,payu_response_id=?PAYU_RESPONSE_ID WHERE ISSUE_ID=?ISSUE_ID";
                        break;
                    }
                case AdmissionSQLCommands.FetchMerchantInfo:
                    {
                        query = @"
                            SELECT 
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
                                   surl,furl,curl
                                FROM
                                    FEE_MERCHANT_ACCOUNT_INFO AS MI
                                        INNER JOIN
                                    SUP_BANK_ACCOUNT AS BA ON BA.BANK_ACCOUNT_ID=MI.ACCOUNT_ID                            
                                WHERE
                                    BA.ACCOUNT_TYPE=?ACCOUNT_TYPE
                                        AND MI.IS_DELETED !=1
                                        AND BA.IS_DELETED !=1
                                        AND MI.API_TYPE=?API_TYPE;
                                ";
                        break;
                    }
                case AdmissionSQLCommands.FetchIssueApplicationById:
                    {
                        query = @"SELECT 
                                AR.APPLICATION_TYPE_ID,
                                ATP.PROGRAMME_ID,
                                AR.FIRST_NAME,
                                AR.SMS_MOBILE_NO AS CONTACT_NO,
                                AR.EMAIL_ID,
                                AR.HSC_NO,
                                APPLICATION_TYPE,
                                APP_FEE,
                                DP.DEPARTMENT_CODE,
                                ISS.PROGRAMME_GROUP_ID,
                                CONCAT(ISS.APPLICATION_NO,LPAD(ISS.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                CONCAT(PR.PROGRAMME_NAME,
                                        '(',
                                        SPM.PROGRAMME_MODE,
                                        ')') AS PROGRAMME_NAME
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SP.RECEIVE_ID
                                    AND AR.IS_DELETED != 1
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = AR.RECEIVE_ID
                                    AND SP.PROGRAMME_ID = ISS.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS ATP ON ATP.PROGRAMME_GROUP_ID = ISS.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                ADM_APPLICATION_TYPE AS APP ON APP.APPLICATION_TYPE_ID = ATP.APPTYPE_ID
                                    INNER JOIN
                                CP_PROGRAMME AS PR ON PR.PROGRAMME_ID = ATP.PROGRAMME_ID
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = ATP.PROGRAMME_MODE
                                    INNER JOIN
                                CP_DEPARTMENT AS DP ON PR.DEPARTMENT = DP.DEPARTMENT_ID
                                    LEFT JOIN
                                SUP_SHIFT AS S ON S.SHIFT_ID = ATP.SHIFT
                            WHERE
                                SP.RECEIVE_ID = ?RECEIVE_ID
                                    AND SP.PROGRAMME_ID = ?PROGRAMME_ID
                                    AND (SP.IS_DELETED != 1 AND IS_CANCELED!=1)
                                    AND ISS.IS_DELETED != 1;";
                        break;

                        //               query = @"SELECT 
                        //                           ISSUE_ID,
                        //                           ISS.APPLICATION_TYPE_ID,
                        //                           ATP.PROGRAMME_ID,
                        //                           FIRST_NAME,
                        //                           LAST_NAME,
                        //                           CONTACT_NO,
                        //                           EMAIL_ID,
                        //                           ISS.SHIFT,
                        //                           HSC_NO,
                        //                           APPLICATION_TYPE,
                        //                           APP_FEE,
                        //                           DP.DEPARTMENT_CODE,
                        //                           ISS.PROGRAMME_GROUP_ID,
                        //                           ISS.APPLICATION_NO,
                        //                           DATE_FORMAT(iss.dob, '%d/%m/%Y') AS DOB,
                        //                           CONCAT(PR.PROGRAMME_NAME,
                        //                           '(',SPM.PROGRAMME_MODE,')',
                        //                                   ' (',
                        //                                   S.DESCRIPTION,' ',
                        //                                   S.TIME,
                        //                                   ')') AS PROGRAMME_NAME
                        //                       FROM
                        //                           ADM_ISSUE_APPLICATION AS ISS
                        //                               INNER JOIN
                        //                           ADM_APPTYPE_PROG_GROUPS AS ATP ON ATP.PRO_GROUPS_ID = ISS.PROGRAMME_GROUP_ID
                        //                               INNER JOIN
                        //                           ADM_APPLICATION_TYPE AS APP ON APP.APPLICATION_TYPE_ID = ATP.APPTYPE_ID
                        //                               INNER JOIN
                        //                           CP_PROGRAMME_?AC_YEAR AS PR ON PR.PROGRAMME_ID = ATP.PROGRAMME_ID
                        //	INNER JOIN
                        //SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID=ATP.PROGRAMME_MODE
                        //                               INNER JOIN
                        //                           CP_DEPARTMENT_?AC_YEAR AS DP ON PR.DEPARTMENT = DP.DEPARTMENT_ID
                        //                               LEFT JOIN
                        //                           SUP_SHIFT AS S ON S.SHIFT_ID = ATP.SHIFT
                        //                       WHERE
                        //                           ISSUE_ID = ?ISSUE_ID AND ISS.IS_DELETED!=1;";

                        //               break;
                    }
                case AdmissionSQLCommands.FetchHostelSelectedStudentInfo:
                    {
                        query = @"SELECT 
                                AR.RECEIVE_ID,
                                ISS.ISSUED_ID,
                                AR.FIRST_NAME,
                                AR.SMS_MOBILE_NO,
                                AR.EMAIL_ID,
                                AR.HSC_NO,
                                APPLICATION_TYPE,
                                SP.PROGRAMME_GROUP_ID,
                                CONCAT(ISS.APPLICATION_NO,LPAD(ISS.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH,
                                CONCAT(PR.PROGRAMME_NAME,
                                        '(',
                                        SPM.PROGRAMME_MODE,
                                        ')',
                                        ' - ',
                                        S.DESCRIPTION,
                                        ' ',
                                        S.TIME) AS PROGRAMME_NAME,
                                H.HOSTEL_NAME
                            FROM
                                ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS SP
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SP.RECEIVE_ID
                                    AND AR.IS_DELETED != 1
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = AR.RECEIVE_ID
                                    AND SP.PROGRAMME_GROUP_ID = ISS.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS ATP ON ATP.PROGRAMME_GROUP_ID = ISS.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                ADM_APPLICATION_TYPE AS APP ON APP.APPLICATION_TYPE_ID = ATP.APPTYPE_ID
                                    INNER JOIN
                                CP_PROGRAMME AS PR ON PR.PROGRAMME_ID = ATP.PROGRAMME_ID
                                    LEFT JOIN
                                SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = ATP.PROGRAMME_MODE
                                    LEFT JOIN
                                SUP_SHIFT AS S ON S.SHIFT_ID = ATP.SHIFT
                                    LEFT JOIN
                                sup_hostel AS H ON H.HOSTEL_ID = SP.HOSTEL_ID
                            WHERE
                                SP.RECEIVE_ID = ?RECEIVE_ID
                                    AND SP.PROGRAMME_GROUP_ID = ?PROGRAMME_GROUP_ID
                                    AND SP.IS_DELETED != 1
                                    AND ISS.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchCourseForAdmission:
                    {

                        query = @"SELECT 
                                    ADT.PRO_GROUPS_ID AS  PROGRAMME_ID,
                                    CONCAT(PRO.PROGRAMME_NAME,
                                            ' (',
                                            SP.PROGRAMME_MODE,
                                            ')') AS PROGRAMME_NAME
                                FROM
                                    ADM_APPTYPE_PROG_GROUPS AS ADT
                                        INNER JOIN
                                    CP_PROGRAMME_?AC_YEAR AS PRO ON PRO.PROGRAMME_ID = ADT.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_PROGRAMME_MODE AS SP ON SP.PROGRAMME_MODE_ID = ADT.PROGRAMME_MODE
                                WHERE
                                    ADT.APPTYPE_ID = ?APPTYPE_ID AND ADT.SHIFT = ?SHIFT;";

                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeByApplicationTypeAndShift:
                    {
                        query = @"SELECT 
                                PG.PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                        ' (',
                                        SP.PROGRAMME_MODE,
                                        ')') AS PROGRAMME_NAME
                            FROM
                                CP_PROGRAMME_GROUP AS PG
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS SP ON SP.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                            WHERE
                                PG.APPTYPE_ID IN (?APPTYPE_ID)
                                    AND PG.SHIFT IN (?SHIFT)
                            ORDER BY PG.APPTYPE_ID , PG.SHIFT;";

                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionProgrammeByApplicationTypeAndShiftAndProgrammemode:
                    {

                        query = @"SELECT 
                                PG.PROGRAMME_GROUP_ID,
                                PG.PROGRAMME_ID,
                                    CONCAT(IFNULL(PRO.PROGRAMME_NAME, ''),
                                    ' - ',
                                    IFNULL(PM.PROGRAMME_MODE, '')) AS PROGRAMME_NAME
                            FROM
                                CP_PROGRAMME_GROUP AS PG
                                    INNER JOIN
                                CP_PROGRAMME AS PRO ON PRO.PROGRAMME_ID = PG.PROGRAMME_ID
                                    AND PRO.IS_DELETED != 1
                                    INNER JOIN
                                SUP_APPLICATION_TYPE AS AP ON AP.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                                    AND AP.IS_ACTIVE = 1
                                    AND AP.IS_DELETED != 1
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                            WHERE
                                PG.APPTYPE_ID IN (?APPTYPE_ID) AND PG.SHIFT IN (?SHIFT)
                                    AND PG.PROGRAMME_MODE IN (?PROGRAMME_MODE);";

                        break;
                    }
                case AdmissionSQLCommands.FetchApplicationType:
                    {

                        query = @"SELECT 
                                    APPLICATION_TYPE_ID, APPLICATION_TYPE
                                FROM
                                    SUP_APPLICATION_TYPE where APPLICATION_TYPE_ID IN (1,2) AND IS_DELETED!=1 AND IS_ACTIVE=1;";
                        break;
                    }
                case AdmissionSQLCommands.SaveIssueApplication:
                    {
                        //query = @"INSERT INTO ADM_ISSUE_APPLICATION
                        //        (
                        //        APPLICATION_TYPE_ID,

                        //        FIRST_NAME,

                        //        CONTACT_NO,

                        //        EMAIL_ID,HSC_NO,ACADEMIC_YEAR)
                        //        VALUES
                        //        (
                        //        ?APPLICATION_TYPE_ID,

                        //        ?FIRST_NAME,

                        //        ?CONTACT_NO,

                        //        ?EMAIL_ID,

                        //        ?HSC_NO,?AC_YEAR);
                        //        ";
                        query = @"INSERT INTO ADM_RECEIVE_APPLICATION
                               (
                               APPLICATION_TYPE_ID,

                               FIRST_NAME,

                               SMS_MOBILE_NO,

                               EMAIL_ID,HSC_NO,ACADEMIC_YEAR,NAME_IN_NATIVE,GENDER)
                               VALUES
                               (
                               ?APPLICATION_TYPE_ID,

                               ?FIRST_NAME,

                               ?SMS_MOBILE_NO,

                               ?EMAIL_ID,
                               
                               ?HSC_NO,?AC_YEAR,?NAME_IN_NATIVE,?GENDER)";
                        break;
                    }
                case AdmissionSQLCommands.SaveIssueApplications:
                    {
                        query = @"INSERT INTO ADM_RECEIVE_APPLICATION
                               (
                               APPLICATION_TYPE_ID,

                               FIRST_NAME,

                               SMS_MOBILE_NO,PROGRAMME_ID,

                               EMAIL_ID,HSC_NO,ACADEMIC_YEAR,NAME_IN_NATIVE,GENDER)
                               VALUES
                               (
                               ?APPLICATION_TYPE_ID,

                               ?FIRST_NAME,

                               ?SMS_MOBILE_NO,?PROGRAMME_ID,

                               ?EMAIL_ID,
                               
                               ?HSC_NO,?AC_YEAR,?NAME_IN_NATIVE,?GENDER)";
                        break;
                    }
                case AdmissionSQLCommands.UpdateIssueApplication:
                    {
                        query = @"UPDATE ADM_ISSUE_APPLICATION
                                SET
                                `APPLICATION_TYPE_ID` = ?APPLICATION_TYPE_ID,
                                
                                `FIRST_NAME` = ?FIRST_NAME,
                                
                                `CONTACT_NO` = ?CONTACT_NO,
                                `EMAIL_ID` = ?EMAIL_ID,
                                
                               
                                `HSC_NO` = ?HSC_NO,
                                 `NAME_IN_NATIVE` = ?NAME_IN_NATIVE,
                                 `GENDER` = ?GENDER                            
                                                            
                                WHERE `ISSUE_ID` = ?ISSUE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionIssueApplicationById:
                    {
                        query = @"SELECT 
	                                    CONCAT(ADM.APPLICATION_NO,LPAD(ADM.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                        ADR.RECEIVE_ID,
                                        ADR.APPLICATION_TYPE_ID,
                                        ADR.FIRST_NAME,
                                        ADR.LAST_NAME,
                                        ADR.AGE,
                                        ADR.SMS_MOBILE_NO,
                                        DATE_FORMAT(ADR.DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH,
                                        ADR.HSC_NO,
                                        ADR.FATHER_NAME,
                                        ADR.CASTE_ID,
                                        ADR.RELIGION_ID,
                                        ADR.MOTHER_TONGUE,
                                        ADR.COMMUNITY_ID,
                                        ADR.OCCUPATION,
                                        ADR.ANNUAL_INCOME,
                                        ADR.MARITAL_STATUS_ID,
                                        ADR.EXSERVICE_MAN,
                                        ADR.EDUCATION_BOARD_ID,
                                        ADR.SPECIALLYABLED_ID,
                                        ADR.GENDER,
                                        ADR.IS_FIRSTGENERATION,
                                        ADR.NATIVE_DISTRICT,
                                        ADR.CDOORDETAIL,
                                        ADR.PDOORDETAIL,
                                        ADR.CSTREET,
                                        ADR.PSTREET,
                                        ADR.CPOST_PLACE_TOWN,
                                        ADR.PPOST_PLACE_TOWN,
                                        ADR.CTALUK_CITY,
                                        ADR.PTALUK_CITY,
                                        ADR.CPINCODE,
                                        ADR.PPINCODE,
                                        ADR.CDISTRICT,
                                        ADR.PDISTRICT,
                                        ADR.CCOUNTRY,
                                        ADR.PCOUNTRY,
                                        ADR.CSTATE,
                                        ADR.PSTATE,
                                        ADR.CVILLAGE_AREA,
                                        ADR.PVILLAGE_AREA,
                                        ADR.CPHONENO,
                                        ADR.PPHONENO,
                                        ADR.HSTOTAL,
                                        ADR.HSPERCENTAGE,
                                        ADR.ACADEMIC_YEAR,
                                        ADR.YEAR_OF_PASSING,
                                        ADR.LAST_STUDIED_SCHOOL,
                                        ADR.UNIVERSITY,
                                        ADR.IS_REGISTERED_TENANT,
                                        ADR.PLACE_OF_BIRTH,
                                        ADR.HOSTEL_FACILITY,
                                        ADR.IS_ROMAN_CATHOLIC,
                                        ADR.PHOTO_PATH,
                                        ADR.IS_SINGLE_GIRL_CHILD,
                                        ADR.IS_SUBMITTED,
                                        ADR.IS_DALIT,
                                        ADR.LAST_STUDIED_PLACE,
                                        ADR.OCCUPATION,
                                        ADR.LAST_STUDIED_CLASS,
                                        ADM.STATUS,
                                        ADR.BLOOD_GROUP,
                                        ADR.EXTRA_CURRICULAR,
                                        ADR.FATHER_AGE,
                                        ADR.MOTHER_AGE,
                                        ADR.MOTHER_OCCUPATION,
                                        ADR.MOTHER_INCOME,
                                        ADR.IS_NRI,
                                        ADR.FATHER_MOBILE_NUMBER,
                                        ADR.MOTHER_MOBILE_NUMBER,
                                        ADR.AADHAR_NUMBER,
                                        ADR.PASSPORT_NUMBER,
                                        ADR.MOTHER_NAME,
                                        ADM.PROGRAMME_GROUP_ID,
                                        ADR.IS_ELEVENTH_PASS,
                                        ADR.SINGLE_PARENT_PROOF,
                                        ADR.PHYSICALY_CHALLENGED_PROOF,
                                        ADR.EX_SERVICEMAN_PROOF, 
                                        ADR.COMMUNITY_PROOF,
                                        ADR.NRI_PROOF,
                                        ADR.IS_SINGLE_PARENT,
                                        ADR.PHYSICALY_CHALLANGED_TYPE,
                                        ADR.UG_TOTAL_SEMESTER,          
                                        ADR.PARISHI_FRC,
                                        ADR.IS_HAVE_CONSOLIDATE_MARKSHEET,
                                        ADR.COLLEGE_DISTRICT,
                                        ADR.COLLEGE_REGION,
                                        ADR.AADHAR_NAME,
                                        ADR.COMMUNITY_CERT_NO,
                                        ADR.IS_ORPHAN,
                                        ADR.INCOME_CERT_NO,
                                        ADR.BANK_NAME,
                                        ADR.BANK_BRANCH,
                                        ADR.CITY,
                                        ADR.IFSC_CODE,
                                        ADR.ACCOUNT_TYPE,
                                        ADR.ACCOUNT_NO
                                        
                                    FROM
                                        ADM_RECEIVE_APPLICATION AS ADR
                                            INNER JOIN
                                        ADM_ISSUED_APPLICATIONS AS ADM ON ADM.RECEIVE_ID = ADR.RECEIVE_ID
                                        
                                    WHERE
                                            ADr.RECEIVE_ID = ?RECEIVE_ID
                                            AND ADr.IS_DELETED != 1
                                            AND ADR.ACADEMIC_YEAR = ?AC_YEAR ORDER BY ADM.STATUS DESC;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionIssuededApplicationById:
                    {
                        query = @"SELECT 
                                            CONCAT(ADM.APPLICATION_NO,
                                                    LPAD(ADM.ISSUE_NO,4, '0')) AS APPLICATION_NO,
                                            ADR.RECEIVE_ID,
                                            ADR.APPLICATION_TYPE_ID,
                                            ADR.FIRST_NAME,
                                            ADR.LAST_NAME,
                                            ADR.AGE,
                                            ADR.SMS_MOBILE_NO,
                                            DATE_FORMAT(ADR.DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH,
                                            ADR.HSC_NO,
                                            ADR.FATHER_NAME,
                                            ADR.CASTE_ID,
                                            ADR.RELIGION_ID,
                                            ADR.MOTHER_TONGUE,
                                            ADR.COMMUNITY_ID,
                                            ADR.OCCUPATION,
                                            ADR.ANNUAL_INCOME,
                                            ADR.MARITAL_STATUS_ID,
                                            ADR.EXSERVICE_MAN,
                                            ADR.EDUCATION_BOARD_ID,
                                            ADR.SPECIALLYABLED_ID,
                                            ADR.GENDER,
                                            ADR.IS_FIRSTGENERATION,
                                            ADR.NATIVE_DISTRICT,
                                            ADR.CDOORDETAIL,
                                            ADR.PDOORDETAIL,
                                            ADR.CSTREET,
                                            ADR.PSTREET,
                                            ADR.CPOST_PLACE_TOWN,
                                            ADR.PPOST_PLACE_TOWN,
                                            ADR.CTALUK_CITY,
                                            ADR.PTALUK_CITY,
                                            ADR.CPINCODE,
                                            ADR.PPINCODE,
                                            ADR.CDISTRICT,
                                            ADR.PDISTRICT,
                                            ADR.CCOUNTRY,
                                            ADR.PCOUNTRY,
                                            ADR.CSTATE,
                                            ADR.PSTATE,
                                            ADR.CVILLAGE_AREA,
                                            ADR.PVILLAGE_AREA,
                                            ADR.CPHONENO,
                                            ADR.PPHONENO,
                                            ADR.HSTOTAL,
                                            ADR.HSPERCENTAGE,
                                            ADR.ACADEMIC_YEAR,
                                            ADR.YEAR_OF_PASSING,
                                            ADR.LAST_STUDIED_SCHOOL,
                                            ADR.UNIVERSITY,
                                            ADR.IS_REGISTERED_TENANT,
                                            ADR.PLACE_OF_BIRTH,
                                            ADR.HOSTEL_FACILITY,
                                            ADR.IS_ROMAN_CATHOLIC,
                                            ADR.PHOTO_PATH,
                                            ADR.IS_SINGLE_GIRL_CHILD,
                                            ADR.IS_SUBMITTED,
                                            ADR.IS_DALIT,
                                            ADR.LAST_STUDIED_PLACE,
                                            ADR.OCCUPATION,
                                            ADR.LAST_STUDIED_CLASS,
                                            ADR.STATUS,
                                            ADR.BLOOD_GROUP,
                                            ADR.EXTRA_CURRICULAR,
                                            ADR.FATHER_AGE,
                                            ADR.MOTHER_AGE,
                                            ADR.MOTHER_OCCUPATION,
                                            ADR.MOTHER_INCOME,
                                            ADR.IS_NRI,
                                            ADR.FATHER_MOBILE_NUMBER,
                                            ADR.MOTHER_MOBILE_NUMBER,
                                            ADR.AADHAR_NUMBER,
                                            ADR.PASSPORT_NUMBER,
                                            ADR.MOTHER_NAME,
                                            ADM.PROGRAMME_GROUP_ID,
                                            ADR.IS_ELEVENTH_PASS,
                                            ADR.SINGLE_PARENT_PROOF,
                                            ADR.PHYSICALY_CHALLENGED_PROOF,
                                            ADR.EX_SERVICEMAN_PROOF,
                                            ADR.COMMUNITY_PROOF,
                                            ADR.NRI_PROOF,
                                            ADR.IS_SINGLE_PARENT,
                                            ADR.PHYSICALY_CHALLANGED_TYPE,
                                            ADR.UG_TOTAL_SEMESTER,
                                            ADR.PARISHI_FRC,
                                            R.RELIGION,
                                            CO.COMMUNITY
                                        FROM
                                            ADM_RECEIVE_APPLICATION AS ADR
                                                INNER JOIN
                                            ADM_ISSUED_APPLICATIONS AS ADM ON ADM.RECEIVE_ID = ADR.RECEIVE_ID
                                                INNER JOIN
                                            sup_religion AS r ON r.RELIGIONID = ADR.RELIGION_ID
                                                INNER JOIN
                                            sup_community AS co ON co.COMMUNITYID = ADR.caste_id
                                        WHERE
                                            ADr.RECEIVE_ID = ?RECEIVE_ID
                                                AND ADr.IS_DELETED != 1
                                                AND ADR.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.SaveFileUploads:
                    {
                        query = @"INSERT INTO ADM_UPLOADED_FILES
                                            (
                                            RECEIVE_STU_ID,
                                            FILE_PATH,
                                            IS_UPLOADED,ACADEMIC_YEAR)
                                            VALUES
                                            (
                                            ?RECEIVE_STU_ID,
                                            ?FILE_PATH,
                                            ?IS_UPLOADED,?AC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.UpdateIssueApplicationById:
                    {
                        query = @"UPDATE ADM_ISSUE_APPLICATION
                                            SET
                                            PROGRAMME_ID= ?PROGRAMME_ID,
                                            RELIGION= ?RELIGION_ID,
                                            FIRST_NAME= ?FIRST_NAME,
                                            LAST_NAME= ?LAST_NAME,
                                            CASTE= ?CASTE,
                                            CDOOR_DETAIL= ?CDOOR_DETAIL,
                                            CSTREET= ?CSTREET,
                                            CVILLAGE_AREA= ?CVILLAGE_AREA,
                                            CPOST_PLACE_TOWN= ?CPOST_PLACE_TOWN,
                                            CTALUK_CITY= ?CTALUK_CITY,
                                            CDISTRICT= ?CDISTRICT,
                                            CPINCODE= ?CPINCODE,
                                            CCOUNTRY= ?CCOUNTRY,
                                            CSTATE= ?CSTATE
                                            WHERE ISSUE_ID= ?ISSUE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateReceiveAplicationById:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION
                                                SET                                               
                                                APPLICATION_TYPE_ID= ?APPLICATION_TYPE_ID,                                              
                                                GENDER=?GENDER,
                                                DATE_OF_BIRTH=?DATE_OF_BIRTH,
                                                FIRST_NAME= ?FIRST_NAME,
                                                LAST_NAME= ?LAST_NAME,
                                                PARISHI_FRC=?PARISHI_FRC,                                        
                                                COMMUNITY_ID= ?COMMUNITY_ID,
                                                CASTE_ID= ?CASTE,
                                                PLACE_OF_BIRTH= ?PLACE_OF_BIRTH,
                                                RELIGION_ID= ?RELIGION_ID,
                                                MOTHER_TONGUE= ?MOTHER_TONGUE,
                                                FATHER_NAME= ?FATHER_NAME,
                                                ANNUAL_INCOME= ?ANNUAL_INCOME,
                                                MARITAL_STATUS_ID= ?MARITAL_STATUS_ID,
                                                IS_FIRSTGENERATION= ?IS_FIRSTGENERATION,
                                                SPECIALLYABLED_ID= ?SPECIALLYABLED_ID,
                                                EXSERVICE_MAN= ?EXSERVICE_MAN,
                                                EXTRA_CURRICULAR= ?EXTRA_CURRICULAR,
                                                EDUCATION_BOARD_ID= ?EDUCATION_BOARD_ID,
                                                CDOORDETAIL= ?CDOORDETAIL,
                                                CSTREET= ?CSTREET,
                                                CVILLAGE_AREA= ?CVILLAGE_AREA,
                                                CPOST_PLACE_TOWN= ?CPOST_PLACE_TOWN,
                                                CTALUK_CITY= ?CTALUK_CITY,
                                                CDISTRICT= ?CDISTRICT,
                                                CSTATE= ?CSTATE,
                                                CPINCODE= ?CPINCODE,
                                                CCOUNTRY= ?CCOUNTRY,
                                                CPHONENO= ?CPHONENO,
                                                CMOBILENO= ?MOBILE_NO,
                                                NATIVE_DISTRICT=?NATIVE_DISTRICT,
                                                FATHER_MOBILE_NUMBER=?FATHER_MOBILE_NUMBER,
                                                MOTHER_MOBILE_NUMBER=?MOTHER_MOBILE_NUMBER,
                                                -- HOSTEL_FACILITY= ?HOSTEL_FACILITY,
                                                PDOORDETAIL= ?PDOORDETAIL,
                                                PSTREET= ?PSTREET,
                                                PVILLAGE_AREA= ?PVILLAGE_AREA,
                                                PPOST_PLACE_TOWN= ?PPOST_PLACE_TOWN,
                                                PTALUK_CITY= ?PTALUK_CITY,
                                                PDISTRICT= ?PDISTRICT,
                                                PPINCODE= ?PPINCODE,
                                                PCOUNTRY= ?PCOUNTRY,
                                                PSTATE= ?PSTATE,
                                                PPHONENO= ?PPHONENO,
                                                PMOBILENO= ?MOBILE_NO,
                                                IS_SINGLE_GIRL_CHILD= ?IS_SINGLE_GIRL_CHILD,
                                                LAST_STUDIED_SCHOOL= ?LAST_STUDIED_SCHOOL,
                                                LAST_STUDIED_PLACE= ?LAST_STUDIED_PLACE,
                                                LAST_STUDIED_CLASS= ?LAST_STUDIED_CLASS,
                                                OCCUPATION= ?OCCUPATION,
                                                IS_DALIT= ?IS_DALIT,
                                                UNIVERSITY= ?UNIVERSITY,
                                                ACADEMIC_YEAR= ?ACADEMIC_YEAR,
                                                IS_ROMAN_CATHOLIC= ?IS_ROMAN_CATHOLIC,
                                                IS_REGISTERED_TENANT= ?IS_REGISTERED_TENANT,
                                                PHOTO_PATH= ?PHOTO_PATH,
                                                HS_MAX_MARK= ?HS_MAX_MARK,
                                                HSTOTAL= ?HSTOTAL,
                                                BLOOD_GROUP= ?BLOOD_GROUP,
                                                IS_SUBMITTED=?IS_SUBMITTED,
                                                STATUS=?STATUS,
                                                FATHER_AGE=?FATHER_AGE,
                                                MOTHER_AGE=?MOTHER_AGE,
                                                MOTHER_INCOME=?MOTHER_INCOME,
                                                MOTHER_OCCUPATION=?MOTHER_OCCUPATION,
                                                IS_NRI=?IS_NRI,
                                                AADHAR_NUMBER=?AADHAR_NUMBER,
                                                PASSPORT_NUMBER=?PASSPORT_NUMBER,MOTHER_NAME=?MOTHER_NAME,IS_SINGLE_PARENT= ?IS_SINGLE_PARENT,PHYSICALY_CHALLANGED_TYPE=?PHYSICALY_CHALLANGED_TYPE,
                                                UG_TOTAL_SEMESTER=?UG_TOTAL_SEMESTER,
                                                COLLEGE_DISTRICT=?COLLEGE_DISTRICT,
                                                COLLEGE_REGION=?COLLEGE_REGION,
                                                AADHAR_NAME=?AADHAR_NAME,
                                                IS_ORPHAN=?IS_ORPHAN,
                                                INCOME_CERT_NO=?INCOME_CERT_NO,
                                                BANK_NAME=?BANK_NAME,
                                                BANK_BRANCH=?BANK_BRANCH,
                                                CITY=?CITY,
                                                ACCOUNT_TYPE=?ACCOUNT_TYPE,
                                                IFSC_CODE=?IFSC_CODE,
                                                COMMUNITY_CERT_NO=?COMMUNITY_CERT_NO,
                                                ACCOUNT_NO=?ACCOUNT_NO
                                                WHERE RECEIVE_ID= ?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.SaveReceiveAplication:
                    {
                        query = @"INSERT INTO ADM_RECEIVE_APPLICATION
                                        (
                                        ISSUE_ID,
                                        SMS_MOBILE_NO,
                                        APPLICATION_TYPE_ID,
                                        APPLICATION_NO,
                                        PROGRAMME_ID,
                                        FIRST_NAME,
                                        LAST_NAME,
                                        AGE,
                                        GENDER,
                                        DATE_OF_BIRTH,
                                        COMMUNITY_ID,
                                        CASTE_ID,
                                        PLACE_OF_BIRTH,
                                        RELIGION_ID,
                                        MOTHER_TONGUE,
                                        FATHER_NAME,
                                        ANNUAL_INCOME,
                                        MARITAL_STATUS_ID,
                                        IS_FIRSTGENERATION,
                                        SPECIALLYABLED_ID,
                                        EXSERVICE_MAN,
                                        EXTRA_CURRICULAR,
                                        EDUCATION_BOARD_ID,
                                        CDOORDETAIL,
                                        CSTREET,
                                        CVILLAGE_AREA,
                                        CPOST_PLACE_TOWN,
                                        CTALUK_CITY,
                                        CDISTRICT,
                                        CPINCODE,
                                        CCOUNTRY,
                                        CSTATE,
                                        CPHONENO,
                                        CMOBILENO,
                                        PDOORDETAIL,
                                        PSTREET,
                                        PVILLAGE_AREA,
                                        PPOST_PLACE_TOWN,
                                        PTALUK_CITY,
                                        PDISTRICT,
                                        PPINCODE,
                                        PCOUNTRY,
                                        PSTATE,
                                        PPHONENO,
                                        PMOBILENO,
                                        HOSTEL_FACILITY,
                                        PROGGROUP_ID,
                                        IS_SINGLE_GIRL_CHILD,
                                        LAST_STUDIED_SCHOOL,
                                        LAST_STUDIED_PLACE,
                                        LAST_STUDIED_CLASS,
                                        OCCUPATION,
                                        IS_DALIT,
                                        UNIVERSITY,
                                        ACADEMIC_YEAR,
                                        IS_ROMAN_CATHOLIC,
                                        PHOTO_PATH,
                                        HS_MAX_MARK,
                                        HSTOTAL,
                                        BLOOD_GROUP,
                                        IS_SUBMITTED,STATUS)
                                        VALUES
                                        (
                                        ?ISSUE_ID,
                                        ?SMS_MOBILE_NO,
                                        ?APPLICATION_TYPE_ID,
                                        ?APPLICATION_NO,
                                        ?PROGRAMME_ID,
                                        ?FIRST_NAME,
                                        ?LAST_NAME,
                                        ?AGE,
                                        ?GENDER,
                                        ?DOB,
                                        ?COMMUNITY_ID,
                                        ?CASTE,
                                        ?PLACE_OF_BIRTH,
                                        ?RELIGION_ID,
                                        ?MOTHER_TONGUE,
                                        ?FATHER_NAME,
                                        ?ANNUAL_INCOME,
                                        ?MARITAL_STATUS_ID,
                                        ?IS_FIRSTGENERATION,
                                        ?SPECIALLYABLED_ID,
                                        ?EXSERVICE_MAN,
                                        ?EXTRA_CURRICULAR,
                                        ?EDUCATION_BOARD_ID,
                                        ?CDOORDETAIL,
                                        ?CSTREET,
                                        ?CVILLAGE_AREA,
                                        ?CPOST_PLACE_TOWN,
                                        ?CTALUK_CITY,
                                        ?CDISTRICT,
                                        ?CPINCODE,
                                        ?CCOUNTRY,
                                        ?CSTATE,
                                        ?CPHONENO,
                                        ?MOBILE_NO,
                                        ?PDOORDETAIL,
                                        ?PSTREET,
                                        ?PVILLAGE_AREA,
                                        ?PPOST_PLACE_TOWN,
                                        ?PTALUK_CITY,
                                        ?PDISTRICT,
                                        ?PPINCODE,
                                        ?PCOUNTRY,
                                        ?PSTATE,
                                        ?PPHONENO,
                                        ?MOBILE_NO,
                                        ?HOSTEL_FACILITY,
                                        ?PROGRAMME_GROUP_ID,
                                        ?IS_SINGLE_GIRL_CHILD,
                                        ?LAST_STUDIED_SCHOOL,
                                        ?LAST_STUDIED_PLACE,
                                        ?LAST_STUDIED_CLASS,
                                        ?OCCUPATION,
                                        ?IS_DALIT,
                                        ?UNIVERSITY,
                                        ?ACADEMIC_YEAR,
                                        ?IS_ROMAN_CATHOLIC,
                                        ?PHOTO_PATH,
                                        ?HS_MAX_MARK,
                                        ?HSTOTAL,
                                        ?BLOOD_GROUP,
                                        ?IS_SUBMITTED,?STATUS);";
                        break;
                    }
                case AdmissionSQLCommands.FetchStuMarksBySubjectIdAndReceiveId:
                    {
                        //query = @"SELECT 
                        //                MARK_ID
                        //            FROM
                        //                ADM_STU_SUBMARKS
                        //            WHERE
                        //                SUBJECT_ID = ?SUBJECT_ID AND RECEIVE_STUID = ?RECEIVE_STUID and ACADEMIC_YEAR=?AC_YEAR
                        //                    AND IS_DELETED != 1;";
                        query = @"SELECT 
                                MARK_ID
                            FROM
                                ADM_STU_SUBMARKS
                            WHERE
                                MARK_ID = ?MARK_ID AND ACADEMIC_YEAR = ?AC_YEAR
                                    AND IS_DELETED != 1;";
                        break;

                    }
                case AdmissionSQLCommands.FetchStuMarksBySubjectIdAndReceiveIdFor11th:
                    {
                        //query = @"SELECT 
                        //                MARK_ID
                        //            FROM
                        //                ADM_STU_SUBMARKS_FOR_11TH
                        //            WHERE
                        //                SUBJECT_ID = ?SUBJECT_ID AND RECEIVE_STUID = ?RECEIVE_STUID and ACADEMIC_YEAR=?AC_YEAR
                        //                    AND IS_DELETED != 1;";
                        query = @"SELECT 
                                    MARK_ID
                                FROM
                                    ADM_STU_SUBMARKS_FOR_11TH
                                WHERE
                                    MARK_ID = ?MARK_ID AND ACADEMIC_YEAR = ?AC_YEAR
                                        AND IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateAdmissionStudentSubMarks:
                    {
                        query = @"UPDATE ADM_STU_SUBMARKS
                                            SET
                                            RECEIVE_STUID= ?RECEIVE_STUID,
                                            SUBJECT_ID= ?SUBJECT_ID,
                                            MAX_MARK= ?MAX_MARK,
                                            MARK= ?MARK,
                                            M_PASS= ?M_PASS,
                                            ACADEMIC_YEAR= ?AC_YEAR,
                                            NO_OF_ATTEMPTS= ?NO_OF_ATTEMPTS,
                                            REGISTER_NO=?REGISTER_NO
                                            WHERE MARK_ID= ?MARK_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateAdmissionStudentSubMarksFor11th:
                    {
                        query = @"UPDATE ADM_STU_SUBMARKS_FOR_11TH
                                            SET
                                            RECEIVE_STUID= ?RECEIVE_STUID,
                                            SUBJECT_ID= ?SUBJECT_ID,
                                            MAX_MARK= ?MAX_MARK,
                                            MARK= ?MARK,
                                            M_PASS= ?M_PASS,
                                            ACADEMIC_YEAR= ?AC_YEAR,
                                            NO_OF_ATTEMPTS= ?NO_OF_ATTEMPTS,
                                            REGISTER_NO=?REGISTER_NO
                                            WHERE MARK_ID= ?MARK_ID;";
                        break;
                    }
                case AdmissionSQLCommands.SaveAdmissionStudentSubMarks:
                    {
                        query = @"INSERT INTO ADM_STU_SUBMARKS
                                            (
                                            RECEIVE_STUID,
                                            SUBJECT_ID,
                                            MAX_MARK,
                                            MARK,
                                            M_PASS,
                                            ACADEMIC_YEAR,
                                            REGISTER_NO,
                                            NO_OF_ATTEMPTS)
                                            VALUES
                                            (
                                            ?RECEIVE_STUID,
                                            ?SUBJECT_ID,
                                            ?MAX_MARK,
                                            ?MARK,
                                            ?M_PASS,
                                            ?AC_YEAR,
                                            ?REGISTER_NO,
                                            ?NO_OF_ATTEMPTS);";
                        break;
                    }
                case AdmissionSQLCommands.SaveAdmissionStudentSubMarksFor11th:
                    {
                        query = @"INSERT INTO ADM_STU_SUBMARKS_FOR_11TH
                                            (
                                            RECEIVE_STUID,
                                            SUBJECT_ID,
                                            MAX_MARK,
                                            MARK,
                                            M_PASS,
                                            ACADEMIC_YEAR,
                                            REGISTER_NO,
                                            NO_OF_ATTEMPTS)
                                            VALUES
                                            (
                                            ?RECEIVE_STUID,
                                            ?SUBJECT_ID,
                                            ?MAX_MARK,
                                            ?MARK,
                                            ?M_PASS,
                                            ?AC_YEAR,
                                            ?REGISTER_NO,
                                            ?NO_OF_ATTEMPTS);";
                        break;
                    }
                case AdmissionSQLCommands.FetchUploadedImagesById:
                    {
                        query = @"SELECT 
                                        UF.FILE_ID,
                                        UF.RECEIVE_STU_ID,
                                        UF.FILE_PATH,
                                        R.PHOTO_PATH
                                    FROM
                                        ADM_UPLOADED_FILES AS UF
											INNER JOIN
										ADM_RECEIVE_APPLICATION AS R ON R.RECEIVE_ID=UF.RECEIVE_STU_ID AND R.IS_DELETED!=1
                                    WHERE
                                        UF.RECEIVE_STU_ID = ?RECEIVE_STU_ID AND UF.IS_DELETED!=1 AND UF.ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.FetchFilePathById:
                    {
                        query = @"SELECT 
                                        FILE_PATH
                                    FROM
                                        ADM_UPLOADED_FILES
                                    WHERE
                                        FILE_ID = ?FILE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.DeleteFileUploads:
                    {
                        query = @"UPDATE ADM_UPLOADED_FILES SET IS_DELETED=1 WHERE FILE_ID=?FILE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateApplicationSubmittion:
                    {
                        query = @"UPDATE adm_issued_applications AS IA  SET  STATUS=?STATUS WHERE RECEIVE_ID=?RECEIVE_ID and STATUS=1;  UPDATE ADM_RECEIVE_APPLICATION AS IA  SET  STATUS=?STATUS,IS_HAVE_CONSOLIDATE_MARKSHEET=?IS_HAVE_CONSOLIDATE_MARKSHEET,IS_SUBMITTED=?IS_SUBMITTED WHERE RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchTotalByIssueId:
                    {
                        query = @"SELECT 
                                    HS_MAX_MARK,
                                    HSTOTAL,
                                    HSPERCENTAGE,
                                    EDUCATION_BOARD_ID,
                                    APPLICATION_TYPE_ID
                                FROM
                                    ADM_RECEIVE_APPLICATION
                                WHERE
                                    RECEIVE_ID =?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateReceiveTotal:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                        SET 
                                            HSTOTAL = ?HSTOTAL,
                                            HS_MAX_MARK = ?HS_MAX_MARK,
                                            HSPERCENTAGE =?HSPERCENTAGE,
                                            TOTAL_CUT_OFF_MARK =?TOTAL_CUT_OFF_MARK,
                                            LAST_STUDIED_CLASS= ?LAST_STUDIED_CLASS
                                        WHERE
                                            RECEIVE_ID = ?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmEligibilityByProgrammeGroupIdId:
                    {
                        query = @"SELECT 
                                        ELIGIBILITY_ID,
                                        PROGRAMME_GROUP_ID,
                                        ELIGIBILITY,
                                        MAIN_SUBJECT_ID,
                                        ACADEMIC_YEAR
                                    FROM
                                        ADM_ELEGIBILITIES
                                    WHERE
                                        PROGRAMME_GROUP_ID = ?PROGRAMME_GROUP_ID
                                            AND IS_DELETED != 1
                                            AND ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionReceivedApplicationByApplicantTypeIdAndProgrammeIdAndCasteId:
                    {
                        query = @"SELECT 
                                        AR.RECEIVE_ID,
                                        CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                        AR.APPLICATION_TYPE_ID,
                                        AR.CMOBILENO AS MOBILE_NO,
                                        AR.FIRST_NAME,
                                        SG.GENDER_NAME,
                                        CONCAT(IFNULL(AR.CTALUK_CITY, ''),
                                                '. ',
                                                IFNULL(AR.CVILLAGE_AREA, '')) AS CVILLAGE_AREA,
                                        AR.IS_SINGLE_GIRL_CHILD,
                                        AR.APPLICATION_TYPE_ID,
                                        AR.TOTAL_CUT_OFF_MARK,
                                        AR.HSTOTAL,
                                        AR.HSPERCENTAGE,
                                        AR.HS_MAX_MARK,
                                        AR.ISSUE_ID,
                                        AR.ACADEMIC_YEAR,
                                        AR.HSC_NO,
                                        SC.COMMUNITYID AS COMMUNITY_ID,
                                        SC.COMMUNITY,
                                        AR.LAST_STUDIED_CLASS,
                                        AR.LAST_STUDIED_SCHOOL,
                                        SU.UNIVERSITY,
                                        AR.OCCUPATION,
                                        AR.ANNUAL_INCOME,
                                        AR.EXSERVICE_MAN,
                                        AR.SPECIALLYABLED_ID,
                                        AR.IS_FIRSTGENERATION,
                                        R.RELIGION 
                                   FROM
                                        ADM_ISSUED_APPLICATIONS AS AI
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                            LEFT JOIN
                                        SUP_COMMUNITY AS SC ON SC.COMMUNITYID = AR.CASTE_ID
                                            LEFT JOIN
                                        SUP_RELIGION AS R ON R.RELIGIONID = AR.RELIGION_ID
                                            LEFT JOIN
                                        GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = SC.COMMUNITYID
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                            LEFT JOIN
                                        SUP_UNIVERSITY AS SU ON SU.UNIVERSITY_ID = AR.UNIVERSITY
                                           -- LEFT JOIN
                                       -- SUP_OCCUPATION AS OC ON OC.OCCUPATION_ID = AR.OCCUPATION
                                        INNER JOIN SUP_GENDER AS SG ON SG.GENDER_ID=AR.GENDER
                                    WHERE
                                        AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID)
                                            AND AI.STATUS =?STATUS
                                            AND AR.HOSTEL_FACILITY IN(?HOSTEL_FACILITY)
                                            AND AR.ACADEMIC_YEAR = ?AC_YEAR
                                            AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                                            AND AR.RELIGION_ID IN (?RELIGION_ID)
                                            AND AR.IS_ROMAN_CATHOLIC IN (?IS_ROMAN_CATHOLIC)
                                            -- AND AR.HSTOTAL!=0
                                            AND AR.IS_DELETED != 1
                                            AND AR.IS_DELETED != 1
                                            AND AR.IS_ELEVENTH_PASS!=1 ORDER BY  AR.TOTAL_CUT_OFF_MARK DESC;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionReceivedApplication:
                    {
                        query = @"SELECT 
                                        AR.RECEIVE_ID,
                                        CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                        AR.APPLICATION_TYPE_ID,
                                        AR.SMS_MOBILE_NO AS MOBILE_NO,
                                        AR.FIRST_NAME,
                                        SG.GENDER_NAME,
                                        CONCAT(IFNULL(AR.CTALUK_CITY, ''),
                                                '. ',
                                                IFNULL(AR.CVILLAGE_AREA, '')) AS CVILLAGE_AREA,
                                        AR.IS_SINGLE_GIRL_CHILD,
                                        AR.APPLICATION_TYPE_ID,
                                        AR.TOTAL_CUT_OFF_MARK,
                                        AR.HSTOTAL,
                                        AR.HSPERCENTAGE,
                                        AR.HS_MAX_MARK,
                                        AR.ISSUE_ID,
                                        AR.ACADEMIC_YEAR,
                                        AR.HSC_NO,
                                        SC.COMMUNITYID AS COMMUNITY_ID,
                                        SC.COMMUNITY,
                                        AR.LAST_STUDIED_CLASS,
                                        AR.LAST_STUDIED_SCHOOL,
                                        SU.UNIVERSITY,
                                        AR.OCCUPATION,
                                        AR.ANNUAL_INCOME,
                                        AR.EXSERVICE_MAN,
                                        AR.SPECIALLYABLED_ID,
                                        AR.IS_FIRSTGENERATION,
                                        R.RELIGION ,
                                        CPG.PROGRAMME_MODE
                                   FROM
                                        ADM_ISSUED_APPLICATIONS AS AI
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                            LEFT JOIN
										adm_waiting_application_?AC_YEAR AS WT ON WT.RECEIVE_ID=AI.RECEIVE_ID and WT.IS_DELETED!=1
                                            AND  WT.PROGRAMME_MODE IN (SELECT 
                                                    PROGRAMME_MODE
                                                FROM
                                                    CP_PROGRAMME_GROUP
                                                WHERE
                                                    PROGRAMME_GROUP_ID IN (?PROGRAMME_ID))
                                            LEFT JOIN
                                        SUP_COMMUNITY AS SC ON SC.COMMUNITYID = AR.CASTE_ID
                                            LEFT JOIN
                                        SUP_RELIGION AS R ON R.RELIGIONID = AR.RELIGION_ID
                                            LEFT JOIN
                                        GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = SC.COMMUNITYID
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                            LEFT JOIN
                                        SUP_UNIVERSITY AS SU ON SU.UNIVERSITY_ID = AR.UNIVERSITY
                                           -- LEFT JOIN
                                       -- SUP_OCCUPATION AS OC ON OC.OCCUPATION_ID = AR.OCCUPATION
                                        INNER JOIN SUP_GENDER AS SG ON SG.GENDER_ID=AR.GENDER
                                    WHERE
                                        WT.RECEIVE_ID IS NULL  AND 
                                        AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID)
                                            AND AI.STATUS =?STATUS
                                            AND AR.HOSTEL_FACILITY IN(?HOSTEL_FACILITY)
                                            AND AR.ACADEMIC_YEAR = ?AC_YEAR
                                            AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                                            AND AR.RELIGION_ID IN (?RELIGION_ID)
                                            AND AR.IS_ROMAN_CATHOLIC IN (?IS_ROMAN_CATHOLIC)
                                            -- AND AR.HSTOTAL!=0
                                            AND AR.IS_DELETED != 1
                                            AND AR.IS_DELETED != 1 and  AR.HSTOTAL!=0 and AR.HSPERCENTAGE!=0 and AR.TOTAL_CUT_OFF_MARK is not null
                                            AND AR.IS_ELEVENTH_PASS!=1 ORDER BY  AR.TOTAL_CUT_OFF_MARK DESC;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionReceivedApplicationForEnglish:
                    {
                        query = @"SELECT 
                                        AR.RECEIVE_ID,
                                        CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                        AR.APPLICATION_TYPE_ID,
                                        AR.SMS_MOBILE_NO AS MOBILE_NO,
                                        AR.FIRST_NAME,
                                        SG.GENDER_NAME,
                                        CONCAT(IFNULL(AR.CTALUK_CITY, ''),
                                                '. ',
                                                IFNULL(AR.CVILLAGE_AREA, '')) AS CVILLAGE_AREA,
                                        AR.IS_SINGLE_GIRL_CHILD,
                                        AR.APPLICATION_TYPE_ID,
                                        MK.MARK AS TOTAL_CUT_OFF_MARK,
                                        AR.HSTOTAL,
                                        AR.HSPERCENTAGE,
                                        AR.HS_MAX_MARK,
                                        AR.ISSUE_ID,
                                        AR.ACADEMIC_YEAR,
                                        AR.HSC_NO,
                                        SC.COMMUNITYID AS COMMUNITY_ID,
                                        SC.COMMUNITY,
                                        AR.LAST_STUDIED_CLASS,
                                        AR.LAST_STUDIED_SCHOOL,
                                        SU.UNIVERSITY,
                                        AR.OCCUPATION,
                                        AR.ANNUAL_INCOME,
                                        AR.EXSERVICE_MAN,
                                        AR.SPECIALLYABLED_ID,
                                        AR.IS_FIRSTGENERATION,
                                        R.RELIGION ,
                                        CPG.PROGRAMME_MODE
                                   FROM
                                        ADM_ISSUED_APPLICATIONS AS AI
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                             INNER JOIN
                                       adm_stu_submarks AS MK ON MK.RECEIVE_STUID = AI.RECEIVE_ID AND MK.SUBJECT_ID IN (2,75)
                                            LEFT JOIN
										adm_waiting_application_?AC_YEAR AS WT ON WT.RECEIVE_ID=AI.RECEIVE_ID AND WT.IS_DELETED!=1
                                           AND WT.PROGRAMME_MODE  IN (SELECT 
                                                    PROGRAMME_MODE
                                                FROM
                                                    CP_PROGRAMME_GROUP
                                                WHERE
                                                    PROGRAMME_GROUP_ID IN (?PROGRAMME_ID))
                                            LEFT JOIN
                                        SUP_COMMUNITY AS SC ON SC.COMMUNITYID = AR.CASTE_ID
                                            LEFT JOIN
                                        SUP_RELIGION AS R ON R.RELIGIONID = AR.RELIGION_ID
                                            LEFT JOIN
                                        GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = SC.COMMUNITYID
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                            LEFT JOIN
                                        SUP_UNIVERSITY AS SU ON SU.UNIVERSITY_ID = AR.UNIVERSITY
                                           -- LEFT JOIN
                                       -- SUP_OCCUPATION AS OC ON OC.OCCUPATION_ID = AR.OCCUPATION
                                        INNER JOIN SUP_GENDER AS SG ON SG.GENDER_ID=AR.GENDER
                                    WHERE
                                         WT.RECEIVE_ID IS NULL AND
                                        AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID)
                                            AND AI.STATUS =?STATUS
                                            AND AR.HOSTEL_FACILITY IN(?HOSTEL_FACILITY)
                                            AND AR.ACADEMIC_YEAR = ?AC_YEAR
                                            AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                                            AND AR.RELIGION_ID IN (?RELIGION_ID)
                                            AND AR.IS_ROMAN_CATHOLIC IN (?IS_ROMAN_CATHOLIC)
                                            -- AND AR.HSTOTAL!=0
                                            AND AR.IS_DELETED != 1
                                            AND AR.IS_DELETED != 1
                                            AND AR.IS_ELEVENTH_PASS!=1 ORDER BY  MK.MARK DESC;";
                        break;
                    }

                case AdmissionSQLCommands.SaveSelectionProcess:
                    {
                        query = @"INSERT INTO ADM_SELECTION_PROCESS_?AC_YEAR
                                            (
                                            APPLICATION_NO, 
                                            APPLICATION_TYPE_ID,
                                            PROGRAMME_ID,
                                            SELECTION_TYPE,
                                            SELECTION_CYCLE,
                                            TOTAL_CUT_OFF_MARK,
                                            TOTAL_SECURED,
                                            MAX_TOTAL,
                                            SELECTED_BY,
                                            RECEIVE_ID,IS_SPORTS_QUOTA)
                                            VALUES
                                            (
                                            ?APPLICATION_NO,
                                            ?APPLICATION_TYPE_ID,
                                            ?PROGRAMME_ID,
                                            ?SELECTION_TYPE,
                                            ?SELECTION_CYCLE,
                                            ?TOTAL_CUT_OFF_MARK,
                                            ?TOTAL_SECURED,
                                            ?MAX_TOTAL,
                                            ?SELECTED_BY,
                                            ?RECEIVE_ID,?IS_SPORTS_QUOTA);";
                        break;
                    }
                case AdmissionSQLCommands.SaveSelectionWaitingProcess:
                    {
                        query = @"INSERT INTO adm_waiting_application_?AC_YEAR
                                            (
                                            APPLICATION_NO, 
                                            APPLICATION_TYPE_ID,
                                            PROGRAMME_ID,
                                            SELECTION_TYPE,
                                            SELECTION_CYCLE,
                                            TOTAL_CUT_OFF_MARK,
                                            TOTAL_SECURED,
                                            MAX_TOTAL,
                                            SELECTED_BY,
                                            RECEIVE_ID,IS_SPORTS_QUOTA,STATUS,PROGRAMME_MODE)
                                            VALUES
                                            (
                                            ?APPLICATION_NO,
                                            ?APPLICATION_TYPE_ID,
                                            ?PROGRAMME_ID,
                                            ?SELECTION_TYPE,
                                            ?SELECTION_CYCLE,
                                            ?TOTAL_CUT_OFF_MARK,
                                            ?TOTAL_SECURED,
                                            ?MAX_TOTAL,
                                            ?SELECTED_BY,
                                            ?RECEIVE_ID,?IS_SPORTS_QUOTA,?STATUS,?PROGRAMME_MODE);";
                        break;
                    }
                case AdmissionSQLCommands.FetchReceiveToSendMailAndMessage:
                    {
                        query = @"SELECT 
                                        AI.ISSUE_ID, AR.SMS_MOBILE_NO, AI.EMAIL_ID
                                    FROM
                                        ADM_RECEIVE_APPLICATION AS AR
                                            INNER JOIN
                                        ADM_ISSUE_APPLICATION AS AI ON AI.ISSUE_ID = AR.ISSUE_ID
                                    WHERE
                                        RECEIVE_ID = ?RECEIVE_ID AND AR.IS_DELETED!=1 AND AI.IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchSelectionProcessByProgrammeIdAndCycle:
                    {
                        query = @"SELECT 
                                        SP.SELECTION_ID,
                                        SP.APPLICATION_NO,
                                        DATE_FORMAT(SP.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                        AR.TOTAL_CUT_OFF_MARK,
                                        AR.HSTOTAL AS 'TOTAL_SECURED',
                                        AR.HS_MAX_MARK AS 'MAX_TOTAL',
                                        SP.RECEIVE_ID,
                                        AR.FIRST_NAME,
                                        SP.IS_VERIFIED,
                                        AR.RECEIVE_ID,                                      
                                        SP.PROGRAMME_ID,
                                        AR.SMS_MOBILE_NO,
                                        AI.STATUS,
                                        ST.STATUS_NAME
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                            INNER JOIN
                                        SUP_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = SP.APPLICATION_TYPE_ID
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SP.RECEIVE_ID
                                            INNER JOIN
                                        ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                            AND AI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
		                                    INNER JOIN
	                                    SUP_ADM_STATUS AS ST ON ST.STATUS_ID=AI.STATUS
                                    WHERE
                                        SP.SELECTION_CYCLE = ?SELECTION_CYCLE
                                            AND SP.PROGRAMME_ID IN (?PROGRAMME_ID) AND (SP.IS_DELETED!=1  AND SP.IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.UpdateVerificationByReceiveId:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR SET IS_VERIFIED=1 ,VERIFIED_DATE=CURDATE(), VERIFIED_BY=?VERIFIED_BY WHERE SELECTION_ID=?SELECTION_ID AND (IS_DELETED!=1 AND IS_CANCELED!=1);";
                        //query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR SET IS_VERIFIED=1,VERIFIED_BY=?VERIFIED_BY WHERE SELECTION_ID=?SELECTION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchSelectionProcessByHSCNo:
                    {
                        query = @"SELECT 
                                        SP.SELECTION_ID,
                                        CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                        DATE_FORMAT(SP.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                        SP.TOTAL_CUT_OFF_MARK,
                                        SP.TOTAL_SECURED,
                                        SP.MAX_TOTAL,
                                        SP.RECEIVE_ID,
                                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                        SP.IS_VERIFIED,
                                        AR.HSC_NO,
                                        PG.PROGRAMME_MODE
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON SP.RECEIVE_ID = AR.RECEIVE_ID
                                            INNER JOIN
                                        ADM_ISSUED_APPLICATIONS AS AI ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                            AND AI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                         INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        SUP_APPLICANT_TYPE AS APT ON APT.APPLICANT_TYPE_ID = SP.APPLICATION_TYPE_ID
                                    WHERE
                                        SP.RECEIVE_ID IN (?RECEIVE_ID)
                                            AND AI.IS_DELETED != 1
                                            AND AR.IS_DELETED != 1                                        
                                            AND (SP.IS_DELETED != 1 AND SP.IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeEligibilityByProgrammeId:
                    {
                        query = @"SELECT 
                                        APE.PROG_ELIGIBILITY_ID,
                                        APE.PROGRAMME_ID,
                                        APE.SUBJECT_ID,
                                        SHS.HSS_SUBJECT,
                                        SHS.CODE
                                    FROM
                                        ADM_PROG_ELIGIBILITY_2018 AS APE
                                            INNER JOIN
                                         CP_PROGRAMME_GROUP AS CP ON CP.PROGRAMME_ID = APE.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_HSS_SUBJECTS AS SHS ON SHS.HSS_SUBJECT_ID = APE.SUBJECT_ID
                                    WHERE
                                        CP.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID)
                                            AND APE.IS_DELETED != 1
                                            AND CP.IS_DELETED != 1 GROUP BY SUBJECT_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStuSubMarksByReceivedId:
                    {
                        query = @"SELECT 
                                    MARK_ID,
                                    RECEIVE_STUID,
                                    SUBJECT_ID,
                                    SS.HSS_SUBJECT,
                                    MAX_MARK,
                                    MARK,
                                    M_PASS,
                                    PROGRAMME_ID,
                                    ACADEMIC_YEAR,
                                    NO_OF_ATTEMPTS,
                                    PART,
                                    IF(PART=3,ROUND((MARK/MAX_MARK)*100,2),0) AS PERSENTAGE
                                FROM
                                    ADM_STU_SUBMARKS AS M
                                        INNER JOIN
                                    SUP_HSS_SUBJECTS AS SS ON SS.HSS_SUBJECT_ID = M.SUBJECT_ID
                                WHERE
                                        M.RECEIVE_STUID = ?RECEIVE_STUID AND ACADEMIC_YEAR = ?AC_YEAR
                                        AND M.IS_DELETED != 1
                                        ORDER BY SS.PART,MARK_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchCoursesAppliedByHSCNo:
                    {
                        query = @"SELECT 
                                    AI.RECEIVE_ID, GROUP_CONCAT(CP.PROGRAMME_NAME) AS PROGRAMME_ID
                                FROM
                                     ADM_RECEIVE_APPLICATION AS AR
                                        INNER JOIN
	                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID=AR.RECEIVE_ID
                                    INNER JOIN
	                                CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID=AI.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                WHERE
		                                AI.RECEIVE_ID IN(?RECEIVE_ID) AND AI.IS_PAID = 1
                                        AND AI.ACADEMIC_YEAR =?AC_YEAR
                                        AND AI.IS_DELETED != 1
                                        AND AR.IS_DELETED != 1
                                GROUP BY AI.RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchCoursesAndProgrammeModeAppliedByHSCNo:
                    {
                        query = @"SELECT 
                                    AI.RECEIVE_ID, GROUP_CONCAT(CP.PROGRAMME_NAME ,' - ',PM.PROGRAMME_MODE) AS PROGRAMME_ID
                                FROM
                                     ADM_RECEIVE_APPLICATION AS AR
                                        INNER JOIN
	                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID=AR.RECEIVE_ID
                                    INNER JOIN
	                                CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID=AI.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                       INNER JOIN 
                                    sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID=CPG.PROGRAMME_MODE
                                WHERE
		                                AI.RECEIVE_ID IN(?RECEIVE_ID) AND AI.IS_PAID = 1
                                        AND AI.ACADEMIC_YEAR =?AC_YEAR
                                        AND AI.IS_DELETED != 1
                                        AND AR.IS_DELETED != 1
                                GROUP BY AI.RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchOverallApplicantListSubmittedAndPaid:
                    {
                        query = @"SELECT 
                                        AD.ISSUE_ID,
                                        ADR.RECEIVE_ID,
                                        AD.APPLICATION_TYPE_ID,
                                        AD.PROGRAMME_ID,
                                        CP.PROGRAMME_NAME,
                                        DATE_FORMAT(AD.ISSUE_DATE, '%d/%m/%Y') AS ISSUE_DATE,
                                        AD.APPLICATION_NO,
                                        AD.FIRST_NAME,
                                        AD.LAST_NAME,
                                        AD.CONTACT_NO,
                                        AD.SHIFT,
                                        DATE_FORMAT(AD.DOB, '%d/%m/%Y') AS DOB,
                                        AD.PROGRAMME_GROUP_ID,
                                        AD.CASTE,
                                        ADR.COMMUNITY_ID,
                                        ADR.CTALUK_CITY,
                                        ADR.IS_SUBMITTED,
                                        ASP.IS_VERIFIED,
                                        ASP.IS_FEE_PAID,
                                        ADR.SMS_MOBILE_NO,
                                        SAS.STATUS_NAME AS STATUS
                                    FROM
                                        ADM_ISSUE_APPLICATION AS AD
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS ADR ON ADR.ISSUE_ID = AD.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = AD.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                            LEFT JOIN
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID = ADR.RECEIVE_ID AND (ASP.IS_DELETED!=1 AND ASP.IS_CANCELED!=1)
                                            LEFT JOIN
										SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID=ADR.STATUS
                                    WHERE
                                        AD.IS_PAID = 1 AND ASP.IS_FEE_PAID = ?IS_FEE_PAID
                                            AND ADR.IS_SUBMITTED = ?IS_SUBMITTED
                                            AND AD.APPLICATION_TYPE_ID = ?APPLICATION_TYPE_ID
                                            AND AD.PROGRAMME_GROUP_ID IN(?PROGRAMME_GROUP_ID)
                                            AND AD.SHIFT = ?SHIFT AND AD.IS_DELETED!=1 ;";
                        break;
                    }
                case AdmissionSQLCommands.FetchOverallApplicantListSubmittedAndUnPaid:
                    {
                        query = @"SELECT 
                                        AD.ISSUE_ID,
                                        ADR.RECEIVE_ID,
                                        AD.APPLICATION_TYPE_ID,
                                        AD.PROGRAMME_ID,
                                        CP.PROGRAMME_NAME,
                                        DATE_FORMAT(AD.ISSUE_DATE, '%d/%m/%Y') AS ISSUE_DATE,
                                        AD.APPLICATION_NO,
                                        AD.FIRST_NAME,
                                        AD.LAST_NAME,
                                        AD.CONTACT_NO,
                                        AD.SHIFT,
                                        DATE_FORMAT(AD.DOB, '%d/%m/%Y') AS DOB,
                                        AD.PROGRAMME_GROUP_ID,
                                        AD.CASTE,
                                        ADR.COMMUNITY_ID,
                                        ADR.CTALUK_CITY,
                                        ADR.IS_SUBMITTED,
                                        ASP.IS_VERIFIED,
                                        ASP.IS_FEE_PAID,
                                        ADR.SMS_MOBILE_NO,
                                        SAS.STATUS_NAME AS STATUS
                                    FROM
                                        ADM_ISSUE_APPLICATION AS AD
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS ADR ON ADR.ISSUE_ID = AD.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = AD.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                            LEFT JOIN
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID = ADR.RECEIVE_ID AND (ASP.IS_DELETED!=1 AND ASP.IS_CANCELED!=1)
                                            LEFT JOIN
										SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID=ADR.STATUS
                                    WHERE
                                        AD.IS_PAID = 1
                                            AND ADR.IS_SUBMITTED = ?IS_SUBMITTED AND ADR.STATUS=2
                                            AND AD.APPLICATION_TYPE_ID = ?APPLICATION_TYPE_ID
                                            AND AD.PROGRAMME_GROUP_ID IN(?PROGRAMME_GROUP_ID)
                                            AND AD.SHIFT = ?SHIFT AND AD.IS_DELETED!=1 ;";
                        break;
                    }
                case AdmissionSQLCommands.FetchOverallApplicantListNotSubmittedAndUnPaid:
                    {
                        query = @"SELECT 
                                        AD.ISSUE_ID,
                                        ADR.RECEIVE_ID,
                                        AD.APPLICATION_TYPE_ID,
                                        AD.PROGRAMME_ID,
                                        CP.PROGRAMME_NAME,
                                        DATE_FORMAT(AD.ISSUE_DATE, '%d/%m/%Y') AS ISSUE_DATE,
                                        AD.APPLICATION_NO,
                                        AD.FIRST_NAME,
                                        AD.LAST_NAME,
                                        AD.CONTACT_NO,
                                        AD.SHIFT,
                                        DATE_FORMAT(AD.DOB, '%d/%m/%Y') AS DOB,
                                        AD.PROGRAMME_GROUP_ID,
                                        AD.CASTE,
                                        ADR.COMMUNITY_ID,
                                        ADR.CTALUK_CITY,
                                        ADR.IS_SUBMITTED,
                                        ASP.IS_VERIFIED,
                                        ASP.IS_FEE_PAID,
                                        ADR.SMS_MOBILE_NO,
                                        SAS.STATUS_NAME AS STATUS
                                    FROM
                                        ADM_ISSUE_APPLICATION AS AD
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS ADR ON ADR.ISSUE_ID = AD.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = AD.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                            LEFT JOIN
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID = ADR.RECEIVE_ID AND (ASP.IS_DELETED!=1 AND ASP.IS_CANCELED!=1)
                                            LEFT JOIN
										SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID=ADR.STATUS
                                    WHERE
                                        AD.IS_PAID = 1
                                            AND (ADR.IS_SUBMITTED != 1 OR ADR.IS_SUBMITTED IS NULL)
                                            AND AD.APPLICATION_TYPE_ID = ?APPLICATION_TYPE_ID
                                            AND AD.PROGRAMME_GROUP_ID IN(?PROGRAMME_GROUP_ID)
                                            AND AD.SHIFT = ?SHIFT AND AD.IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionClassesByProgrammeMode:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                '(',
                                                IFNULL(CONCAT(' SHIFT - ',SHIFT_NAME), ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                       ADM_CLASSES_?AC_YEAR CLS INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=CLS.SHIFT
                                    WHERE
                                        CLS.IS_DELETED != 1 AND PROGRAMME_MODE=?PROGRAMME_MODE;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeByApprogramme:
                    {
                        query = @"SELECT 
                        AP.PROGRAMME_GROUP_ID,
                        P.PROGRAMME_ID,
                        AP.SHIFT,
                        AP.PROGRAMME_MODE,
                        CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                ' (',
                                IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                ')') AS PROGRAMME_NAME
                    FROM
                       CP_PROGRAMME_GROUP AS AP
                            INNER JOIN
                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = AP.PROGRAMME_ID
                            AND P.IS_DELETED != 1
                            INNER JOIN
                        SUP_SHIFT SS ON SS.SHIFT_ID = AP.SHIFT
                            AND SS.IS_DELETED != 1
                            INNER JOIN
                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                    WHERE
                        AP.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgramByProGroupId:
                    {
                        query = @"SELECT 
                        PROGRAMME_GROUP_ID,
                        P.PROGRAMME_ID,
                        AP.SHIFT,
                        AP.PROGRAMME_MODE,
                        CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                ' (',
                                IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                ')',
                                '-',
                                IFNULL(CONCAT(' SHIFT - ', SS.SHIFT_NAME), '')) AS PROGRAMME_NAME
                    FROM
                       CP_PROGRAMME_GROUP AS AP
                            INNER JOIN
                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = AP.PROGRAMME_ID
                            AND P.IS_DELETED != 1
                            INNER JOIN
                        SUP_SHIFT SS ON SS.SHIFT_ID = AP.SHIFT
                            AND SS.IS_DELETED != 1
                            INNER JOIN
                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                    WHERE
                        AP.IS_DELETED != 1 AND PROGRAMME_GROUP_ID IN (?PROGRAMME_ID);";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionClassesByProGroupId:
                    {
                        query = @"SELECT 
                                CLASS_ID,
                                CLS.PROGRAMME,
                                CLS.SHIFT,
                                CLS.PROGRAMME_MODE,
                                CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                        '(',
                                        IFNULL(CONCAT(' SHIFT - ', SHIFT_NAME), ''),
                                        ')') AS CLASS_NAME
                            FROM
                                ADM_CLASSES_?AC_YEAR CLS
                                    INNER JOIN
                                ADM_APPTYPE_PROG_GROUPS AS AP ON AP.PRO_GROUPS_ID = CLS.PROGRAMME                                    
                                    AND AP.IS_DELETED != 1
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = CLS.SHIFT
                                    AND SS.IS_DELETED != 1
                            WHERE
                                CLS.IS_DELETED != 1 AND CLS.CLASS_YEAR=?CLASS_YEAR AND CLS.PROGRAMME=?PROGRAMME;";
                        break;
                    }
                case AdmissionSQLCommands.FetchFeeStructureForStudentAccount:
                    {
                        query = @"SELECT 
                                        FS.FEE_STRUCTURE_ID,
                                        AR.RECEIVE_ID,
                                        FS.FREQUENCY,
                                        FS.CLASS,
                                        FS.HEAD,
                                        SH.HEAD AS 'HEAD_NAME',
                                        FS.AMOUNT,
                                        FS.FEE_MODE,
                                        FS.FEE_CATEGORY,
                                        FS.PROGRAMME,
                                        CPG.PROGRAMME_MODE,
                                        CPG.SHIFT,
                                        MH.FEE_MAIN_HEAD_ID
                                    FROM
                                        FEE_STRUCTURE AS FS
                                            INNER JOIN
                                        FEE_MAIN_HEADS AS MH ON MH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                            AND MH.IS_DELETED != 1
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = FS.PROGRAMME
                                            INNER JOIN
                                        ADM_ISSUED_APPLICATIONS AS AI ON AI.PROGRAMME_GROUP_ID = CPG.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                            AND SST.IS_ACTIVE = 1
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FS.HEAD
                                    WHERE
                                            AI.RECEIVE_ID =?RECEIVE_ID AND FREQUENCY = ?FREQUENCY
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FS.IS_DELETED != 1
                                            AND AI.IS_DELETED != 1
                                            AND AR.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.SelectInsertForStudentAccountByReceiveIdAndFrequencyId:
                    {
                        query = @"INSERT INTO FEE_STUDENT_ACCOUNT (STUDENT_ID,ACADEMIC_YEAR,FREQUENCY_ID,HEAD,CREDIT,FEE_MAIN_HEAD_ID,FEE_STRUCTURE_ID,FEE_ROOT_ID )                                     
                                    SELECT 
                                        AR.RECEIVE_ID AS STUDENT_ID,
                                        FS.ACADEMIC_YEAR,
                                        FS.FREQUENCY,
                                        FS.HEAD,
                                        FS.AMOUNT,
                                        FS.FEE_MAIN_HEAD_ID,
                                        FS.FEE_STRUCTURE_ID,
                                        FS.FEE_ROOT_ID
                                    FROM
                                        FEE_STRUCTURE AS FS
                                            INNER JOIN
                                        FEE_MAIN_HEADS AS MH ON MH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                            AND FS.FREQUENCY = MH.FREQUENCY_ID
                                            AND FS.ACADEMIC_YEAR = MH.ACADEMIC_YEAR
                                            AND MH.IS_DELETED != 1
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = FS.PROGRAMME
                                            AND CPG.IS_DELETED != 1
                                            INNER JOIN
                                        ADM_ISSUED_APPLICATIONS AS AI ON AI.PROGRAMME_GROUP_ID = CPG.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                            INNER JOIN
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP ON SP.RECEIVE_ID = AI.RECEIVE_ID
                                            AND SP.PROGRAMME_ID = AI.PROGRAMME_GROUP_ID
                                            AND (SP.IS_DELETED != 1 AND IS_CANCELED != 1)
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                            AND SST.IS_ACTIVE = 1
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FS.HEAD
                                            AND SH.IS_DELETED != 1
                                    WHERE
                                        AI.RECEIVE_ID = ?RECEIVE_ID AND FS.FREQUENCY = ?FREQUENCY
                                            AND FS.HEAD NOT IN (80 , 81, 82, 83)
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FS.IS_DELETED != 1
                                            AND AI.IS_DELETED != 1
                                            AND AR.IS_DELETED != 1
                                            AND FS.AMOUNT IS NOT NULL
                                            AND FS.AMOUNT != '';";
                        break;
                    }
                case AdmissionSQLCommands.SelectInsertForHostelStudentAccountByReceiveIdAndFrequencyId:
                    {
                        //query = @"INSERT INTO FEE_STUDENT_ACCOUNT (STUDENT_ID,ACADEMIC_YEAR,FREQUENCY_ID,HEAD,CREDIT,FEE_MAIN_HEAD_ID )
                        //            SELECT 
                        //                '?RECEIVE_ID' AS STUDENT_ID,
                        //                FS.ACADEMIC_YEAR,
                        //                FS.FREQUENCY,
                        //                FS.HEAD,
                        //                FS.AMOUNT,
                        //                FS.FEE_MAIN_HEAD_ID
                        //            FROM
                        //                FEE_STRUCTURE_FOR_HOSTEL_AND_MESS AS FS
                        //                    INNER JOIN
                        //                FEE_MAIN_HEADS AS MH ON MH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                        //                    AND MH.IS_DELETED != 1
                        //                    INNER JOIN
                        //                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY
                        //                    INNER JOIN
                        //                SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                        //                    LEFT JOIN
                        //                SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                        //                    AND SST.IS_ACTIVE = 1
                        //                    INNER JOIN
                        //                SUP_HEAD AS SH ON SH.HEAD_ID = FS.HEAD
                        //            WHERE
                        //                FREQUENCY = ?FREQUENCY
                        //                    AND FS.ACADEMIC_YEAR = ?AC_YEAR
                        //                    AND FS.IS_DELETED != 1 AND FS.AMOUNT IS NOT NULL AND FS.AMOUNT!='';";
                        query = @"INSERT INTO FEE_STUDENT_ACCOUNT (STUDENT_ID,ACADEMIC_YEAR,FREQUENCY_ID,HEAD,CREDIT,FEE_MAIN_HEAD_ID,FEE_STRUCTURE_ID,FEE_ROOT_ID )
                                    SELECT 
                                        '?RECEIVE_ID' AS STUDENT_ID,
                                        FS.ACADEMIC_YEAR,
                                        FS.FREQUENCY,
                                        FS.HEAD,
                                        FS.AMOUNT,
                                        FS.FEE_MAIN_HEAD_ID,
                                        FS.FEE_STRUCTURE_ID,
                                        FS.FEE_ROOT_ID
                                    FROM
                                        FEE_STRUCTURE_FOR_HOSTEL_AND_MESS AS FS
                                            INNER JOIN
                                        FEE_MAIN_HEADS AS MH ON MH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                            AND MH.IS_DELETED != 1
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                            LEFT JOIN
                                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                            AND SST.IS_ACTIVE = 1
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FS.HEAD
                                    WHERE
                                        FREQUENCY = ?FREQUENCY
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FS.IS_DELETED != 1 AND FS.AMOUNT IS NOT NULL AND FS.AMOUNT!='';";
                        break;
                    }
                case AdmissionSQLCommands.SendRequestToTransferApplicant:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION SET REQUEST_ID=?REQUEST_ID WHERE RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.ApproveTransferApplicant:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION SET APPROVE_ID=?APPROVE_ID WHERE RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchApplicantsToTransfer:
                    {
                        query = @"SELECT 
                                        SP.SELECTION_ID,
                                        SP.APPLICATION_NO,
                                        DATE_FORMAT(SP.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                        SP.TOTAL_CUT_OFF_MARK,
                                        SP.TOTAL_SECURED,
                                        SP.MAX_TOTAL,
                                        SP.RECEIVE_ID,
                                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                        SP.IS_VERIFIED,
                                        AR.ISSUE_ID,
                                        AT.REQUEST_ID,
                                        AT.APPROVE_ID,
                                        SP.PROGRAMME_ID,
                                        SSC.SELECTION_CYCLE,
                                        AR.SMS_MOBILE_NO,
                                        SC.COMMUNITY
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                            INNER JOIN
                                        ADM_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = SP.APPLICATION_TYPE_ID
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SP.RECEIVE_ID
											LEFT JOIN
                                        ADM_TRANSFER AS AT ON AT.RECEIVE_ID=AR.RECEIVE_ID AND AT.ACADEMIC_YEAR=?AC_YEAR
											LEFT JOIN
										STF_PERSONAL_INFO AS SF ON SF.STAFF_ID=AT.REQUEST_ID
                                            LEFT JOIN
										SUP_SELECTION_CYCLE AS SSC ON SSC.SELECTION_CYCLE_ID=SP.SELECTION_CYCLE
											LEFT JOIN SUP_COMMUNITY AS SC ON SC.COMMUNITYID=AR.CASTE_ID
                                    WHERE
                                        SP.SELECTION_CYCLE = ?SELECTION_CYCLE
                                            AND SP.PROGRAMME_ID IN (?PROGRAMME_ID) AND (SP.IS_DELETED!=1 AND SP.IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchApplicantsInfoForTransfer:
                    {
                        query = @"SELECT 
                                        AD.ISSUE_ID,
                                        ADR.RECEIVE_ID,
                                        AD.APPLICATION_TYPE_ID,
                                        AD.PROGRAMME_ID,
                                        CP.PROGRAMME_NAME,
                                        DATE_FORMAT(AD.ISSUE_DATE, '%d/%m/%Y') AS ISSUE_DATE,
                                        AD.APPLICATION_NO,
                                        AD.FIRST_NAME,
                                        AD.LAST_NAME,
                                        AD.CONTACT_NO,
                                        AD.SHIFT,
                                        DATE_FORMAT(AD.DOB, '%d/%m/%Y') AS DOB,
                                        AD.PROGRAMME_GROUP_ID,
                                        AD.CASTE,
                                        ADR.COMMUNITY_ID,
                                        ADR.CTALUK_CITY,
                                        ADR.IS_SUBMITTED,
                                        ASP.IS_VERIFIED,
                                        ASP.IS_FEE_PAID,
                                        ADR.SMS_MOBILE_NO,
                                        SPM.PROGRAMME_MODE_ID,
                                        SPM.PROGRAMME_MODE
                                    FROM
                                        ADM_ISSUE_APPLICATION AS AD
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS ADR ON ADR.ISSUE_ID = AD.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = AD.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                            LEFT JOIN
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID = ADR.RECEIVE_ID
											LEFT JOIN
										SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID=AAP.PROGRAMME_MODE
                                    WHERE
                                        ADR.RECEIVE_ID=?RECEIVE_ID AND AD.IS_DELETED!=1 AND (ASP.IS_DELETED!=1 AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchFrequencyByReceiveId:
                    {
                        query = @"SELECT 
                                        SFF.FREQUENCY_ID, SFF.FREQUENCY_NAME
                                    FROM
                                        FEE_STRUCTURE AS FS
                                            INNER JOIN
                                        ADM_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = FS.CLASS
                                            INNER JOIN
                                        ADM_ISSUE_APPLICATION AS AI ON AI.PROGRAMME_GROUP_ID = CLS.PROGRAMME
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.ISSUE_ID = AI.ISSUE_ID
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FS.FREQUENCY
                                            INNER JOIN
										SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID=SFF.SEMESTER_TYPE
                                    WHERE
                                        AR.RECEIVE_ID = ?RECEIVE_ID
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR AND AI.IS_DELETED!=1 AND AR.IS_DELETED!=1
                                    GROUP BY SFF.FREQUENCY_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeGroupByProgrammeId:
                    {
                        query = @"SELECT 
                        PRO_GROUPS_ID,
                        P.PROGRAMME_ID,
                        AP.SHIFT,
                        AP.PROGRAMME_MODE,
                        CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                ' (',
                                IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                ')',
                                '-',
                                IFNULL(CONCAT(' SHIFT - ', SS.SHIFT_NAME), '')) AS PROGRAMME_NAME
                    FROM
                       ADM_APPTYPE_PROG_GROUPS AS AP
                            INNER JOIN
                        CP_PROGRAMME_?AC_YEAR AS P ON P.PROGRAMME_ID = AP.PROGRAMME_ID
                            AND P.IS_DELETED != 1
                            INNER JOIN
                        SUP_SHIFT SS ON SS.SHIFT_ID = AP.SHIFT
                            AND SS.IS_DELETED != 1
                            INNER JOIN
                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                    WHERE
                        AP.IS_DELETED != 1 AND AP.SHIFT=?SHIFT AND AP.PROGRAMME_MODE=?PROGRAMME_MODE;";
                        break;
                    }
                case AdmissionSQLCommands.SaveAdmTransfer:
                    {
                        query = @"INSERT INTO 
                                     ADM_TRANSFER(RECEIVE_ID
                                     ,APPLICATION_NO,
                                     PROGRAMME_FROM,
                                     PROGRAMME_TO,
                                     ACADEMIC_YEAR,
                                     APPROVE_ID,STATUS) 
                                    VALUES(?RECEIVE_ID,
                                     ?APPLICATION_NO,
                                     ?PROGRAMME_FROM,
                                     ?PROGRAMME_TO,
                                     ?AC_YEAR,
                                     ?APPROVE_ID,
                                     ?STATUS);";
                        break;
                    }
                case AdmissionSQLCommands.FetchFeeDetailsByProgrammeGroupId:
                    {
                        query = @"SELECT 
                                        FS.FEE_STRUCTURE_ID,
                                        SFF.FREQUENCY_ID,
                                        SFF.FREQUENCY_NAME,
                                        CLS.CLASS_ID,
                                        SFM.MAIN_HEAD_ID,
                                        SFM.MAIN_HEAD,
                                        CLS.SHIFT,
                                        SUM(FS.AMOUNT) AS AMOUNT,
                                        FSC.FEE_CATEGORY
                                    FROM
                                        FEE_STRUCTURE AS FS
                                            INNER JOIN
                                        ADM_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = FS.CLASS
                                            INNER JOIN
                                        FEE_MAIN_HEADS AS FMH ON FMH.HEAD_ID = FS.HEAD
                                            AND FMH.FREQUENCY_ID = FS.FREQUENCY
                                            AND FMH.ACADEMIC_YEAR = FS.ACADEMIC_YEAR
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = CLS.PROGRAMME
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FS.FREQUENCY
                                            INNER JOIN
                                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
											INNER JOIN
										SUP_FEE_CATEGORY AS FSC ON FSC.FEE_CATEGORY_ID=FS.FEE_CATEGORY
                                    WHERE
                                        CLS.PROGRAMME_MODE = ?PROGRAMME_MODE AND CLS.SHIFT = ?SHIFT 
                                            AND CLS.PROGRAMME = ?PROGRAMME
                                            AND SFF.IS_DELETED != 1
                                            AND SST.IS_ACTIVE = 1
                                            AND FS.ACADEMIC_YEAR=?AC_YEAR
                                    GROUP BY FS.FREQUENCY , FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchFrequencyByProgrammeGroupId:
                    {
                        query = @"SELECT 
                                        SFF.FREQUENCY_ID,SFF.FREQUENCY_NAME
                                    FROM
                                        FEE_STRUCTURE AS FS
                                            INNER JOIN
                                        ADM_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = FS.CLASS
                                            INNER JOIN
                                        FEE_MAIN_HEADS AS FMH ON FMH.HEAD_ID = FS.HEAD
                                            AND FMH.FREQUENCY_ID = FS.FREQUENCY
                                            AND FMH.ACADEMIC_YEAR = FS.ACADEMIC_YEAR
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = CLS.PROGRAMME
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FS.FREQUENCY
                                            INNER JOIN
                                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                    WHERE
                                        CLS.PROGRAMME_MODE = ?PROGRAMME_MODE AND CLS.SHIFT = ?SHIFT
                                            AND CLS.PROGRAMME = ?PROGRAMME
                                            AND SFF.IS_DELETED != 1
                                            AND SST.IS_ACTIVE = 1
                                            AND FS.ACADEMIC_YEAR=?AC_YEAR
                                    GROUP BY FS.FREQUENCY;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentsForTransfer:
                    {

                        query = @"SELECT 
                                        CONCAT(ADI.APPLICATION_NO,LPAD(ADI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                        AR.FIRST_NAME,
                                        CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                                ' (',
                                                IFNULL(SPM.PROGRAMME_MODE, ''),
                                                ') - SHIFT -',
                                                IFNULL(SH.SHIFT_NAME, '')) AS PROGRAMME_FROM,
                                        AR.RECEIVE_ID,
                                        AR.HSTOTAL,
                                        AR.HS_MAX_MARK,
                                        SDS.STATUS_NAME AS STATUS,
                                        ADI.STATUS AS STATUS_ID
                                    FROM
                                        ADM_RECEIVE_APPLICATION AS AR
                                            INNER JOIN
                                        ADM_ISSUED_APPLICATIONS AS ADI ON ADI.RECEIVE_ID = AR.RECEIVE_ID
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = ADI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = CPG.SHIFT
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                                            INNER JOIN
                                        SUP_ADM_STATUS AS SDS ON SDS.STATUS_ID = ADI.STATUS
                                    WHERE
                                            ADI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID)
                                            AND AR.RECEIVE_ID = ?RECEIVE_ID
                                            AND ADI.ACADEMIC_YEAR = ?AC_YEAR;";
                        //query = @"SELECT 
                        //                        AT.TRANSFER_ID,
                        //                        AT.APPLICATION_NO,
                        //                        AT.REQUEST_ID,
                        //                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                        //                                '. ',
                        //                                IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                        //                        CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                        //                                ' (',
                        //                                IFNULL(SPM.PROGRAMME_MODE, ''),
                        //                                ') - SHIFT -',
                        //                                IFNULL(SH.SHIFT_NAME, '')) AS PROGRAMME_FROM,
                        //                        CONCAT(IFNULL(CP1.PROGRAMME_NAME, ''),
                        //                                ' (',
                        //                                IFNULL(SPM1.PROGRAMME_MODE, ''),
                        //                                ') - SHIFT -',
                        //                                IFNULL(SH1.SHIFT_NAME, '')) AS PROGRAMME_TO,
                        //                        AT.RECEIVE_ID,
                        //                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                        //                                '( ',
                        //                                IFNULL(SP.STAFF_CODE, ''),
                        //                                ')') AS REQUEST_ID,
                        //                        AR.HSTOTAL,
                        //                        AR.HS_MAX_MARK,
                        //                        AT.IS_APPROVED
                        //                    FROM
                        //                        ADM_TRANSFER AS AT
                        //                            INNER JOIN
                        //                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AT.RECEIVE_ID
                        //                            INNER JOIN
                        //                        ADM_APPTYPE_PROG_GROUPS AS APP ON APP.PRO_GROUPS_ID = AT.PROGRAMME_FROM
                        //                            INNER JOIN
                        //                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = APP.PROGRAMME_ID
                        //                            INNER JOIN
                        //                        SUP_SHIFT AS SH ON SH.SHIFT_ID = APP.SHIFT
                        //                            INNER JOIN
                        //                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = APP.PROGRAMME_MODE
                        //                            INNER JOIN
                        //                        ADM_APPTYPE_PROG_GROUPS AS APP1 ON APP1.PRO_GROUPS_ID = AT.programme_to
                        //                            INNER JOIN
                        //                        CP_PROGRAMME_?AC_YEAR AS CP1 ON CP1.PROGRAMME_ID = APP1.PROGRAMME_ID
                        //                            INNER JOIN
                        //                        SUP_SHIFT AS SH1 ON SH1.SHIFT_ID = APP1.SHIFT
                        //                            INNER JOIN
                        //                        SUP_PROGRAMME_MODE AS SPM1 ON SPM1.PROGRAMME_MODE_ID = APP1.PROGRAMME_MODE
                        //                            INNER JOIN
                        //                        STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = AT.REQUEST_ID
                        //                    WHERE
                        //                        AT.PROGRAMME_FROM IN (?PROGRAMME_ID)
                        //                            AND AT.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateApproveIdByTransferId:
                    {
                        query = @"UPDATE ADM_TRANSFER SET IS_APPROVED=1,APPROVE_ID=?APPROVE_ID WHERE TRANSFER_ID=?TRANSFER_ID;";
                        break;
                    }
                case AdmissionSQLCommands.GetSubmittedStudentForPrincipal:
                    {
                        query = @"SELECT 
                                        AD.ISSUE_ID,
                                        ADR.RECEIVE_ID,
                                        AD.APPLICATION_TYPE_ID,
                                        AD.PROGRAMME_ID,
                                        CP.PROGRAMME_NAME,
                                        DATE_FORMAT(AD.ISSUE_DATE, '%d/%m/%Y') AS ISSUE_DATE,
                                        AD.APPLICATION_NO,
                                        AD.FIRST_NAME,
                                        AD.LAST_NAME,
                                        AD.CONTACT_NO,
                                        AD.SHIFT,
                                        DATE_FORMAT(AD.DOB, '%d/%m/%Y') AS DOB,
                                        AD.PROGRAMME_GROUP_ID,
                                        AD.CASTE,
                                        ADR.COMMUNITY_ID,
                                        ADR.CTALUK_CITY,
                                        ADR.IS_SUBMITTED,
                                        ASP.IS_VERIFIED,
                                        ASP.IS_FEE_PAID,
                                        ADR.SMS_MOBILE_NO,
                                        S.STATUS_NAME AS 'STATUS',
                                        ADR.HS_MAX_MARK,
                                        ADR.HSTOTAL,
                                        ADR.TOTAL_CUT_OFF_MARK
                                    FROM
                                        ADM_ISSUE_APPLICATION AS AD
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS ADR ON ADR.ISSUE_ID = AD.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = AD.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                            LEFT JOIN
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID = ADR.RECEIVE_ID
                                            LEFT JOIN
										SUP_ADM_STATUS AS S ON S.STATUS_ID=ADR.STATUS
                                    WHERE
                                        AD.IS_PAID = 1
                                            AND ADR.IS_SUBMITTED = 1                                            
                                            AND AD.PROGRAMME_GROUP_ID IN(?PROGRAMME_GROUP_ID) AND AD.IS_DELETED!=1 AND (ASP.IS_DELETED!=1 AND ASP.IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.GetSubmittedStudentsForPrincipalByReceiveId:
                    {
                        query = @"SELECT 
                                        AR.RECEIVE_ID,
                                        AI.APPLICATION_NO,
                                        AR.APPLICATION_TYPE_ID,
                                        AR.CMOBILENO AS MOBILE_NO,
                                        AI.EMAIL_ID,
                                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                        AR.IS_SINGLE_GIRL_CHILD,
                                        AR.TOTAL_CUT_OFF_MARK,
                                        AR.HSTOTAL,
                                        AR.HSPERCENTAGE,
                                        AR.HS_MAX_MARK,
                                        AR.ISSUE_ID,
                                        AI.ACADEMIC_YEAR,
                                        AI.HSC_NO,
                                        AI.SHIFT,
                                        SC.COMMUNITYID AS COMMUNITY_ID,
                                        SC.COMMUNITY,
                                        AR.LAST_STUDIED_CLASS,
                                        AI.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID'
                                    FROM
                                        ADM_RECEIVE_APPLICATION AS AR
                                            INNER JOIN
                                        ADM_ISSUE_APPLICATION AS AI ON AI.ISSUE_ID = AR.ISSUE_ID
                                            INNER JOIN
                                        SUP_COMMUNITY AS SC ON SC.COMMUNITYID = AR.CASTE_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AI.PROGRAMME_ID
                                    WHERE
                                            AI.ACADEMIC_YEAR = ?AC_YEAR
                                            AND AR.RECEIVE_ID=?RECEIVE_ID
                                            AND AR.IS_SUBMITTED=1
                                            AND AI.IS_DELETED!=1 AND AR.IS_DELETED!=1 AND AI.IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentRelationsByReceiveId:
                    {
                        query = @"SELECT 
                                        RELATION_ID,
                                        RECEIVE_ID,
                                        RELATION_NAME,
                                        RELATION_SHIP,
                                        OCCUPATION,
                                        INCOME,
                                        ACADEMIC_YEAR
                                    FROM
                                        ADM_STU_RELATIONS
                                    WHERE
                                        RECEIVE_ID = ?RECEIVE_ID  AND IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmittedStudentList:
                    {
                        query = @"SELECT 
                                        ADI.ISSUED_ID AS ISSUE_ID,
                                        ADR.RECEIVE_ID,
                                        ADR.APPLICATION_TYPE_ID,
                                        ADI.PROGRAMME_GROUP_ID,
                                        CP.PROGRAMME_NAME,
                                        ASP.APPLICATION_NO,
                                        ADR.FIRST_NAME,
                                        SH.SHIFT_NAME,
                                        DATE_FORMAT(ADR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                        ADR.CASTE_ID,
                                        ADR.COMMUNITY_ID,
                                        ADR.CTALUK_CITY,
                                        ADR.IS_SUBMITTED,
                                        ASP.IS_VERIFIED,
                                        ASP.IS_FEE_PAID,
                                        ADR.SMS_MOBILE_NO,
                                        S.STATUS_NAME AS STATUS,
                                        ADR.HS_MAX_MARK,
                                        ADR.TOTAL_CUT_OFF_MARK,
                                        ADR.HSTOTAL
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS ASP
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS ADR ON ASP.RECEIVE_ID = ADR.RECEIVE_ID
                                            INNER JOIN
                                        adm_issued_applications AS ADI ON ADI.RECEIVE_ID = ADR.RECEIVE_ID
                                            AND ADI.PROGRAMME_GROUP_ID = ASP.PROGRAMME_ID
                                            INNER JOIN
                                        cp_programme_group AS CPG ON CPG.PROGRAMME_GROUP_ID = ASP.PROGRAMME_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_ADM_STATUS AS S ON S.STATUS_ID = ADI.STATUS
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = CPG.SHIFT
                                    WHERE
                                            ADI.STATUS = ?STATUS
                                            AND ASP.SELECTION_CYCLE = ?SELECTION_CYCLE
                                            AND ADI.IS_PAID = 1
                                            AND ASP.PROGRAMME_ID IN (?PROGRAMME_ID)
                                            AND (ASP.IS_DELETED != 1 AND ASP.IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionClassesByProgrammeId:
                    {
                        query = @"SELECT 
                                CLASS_ID,
                                CLS.PROGRAMME,
                                CLS.SHIFT,
                                CLS.PROGRAMME_MODE,
                                CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                        '(',
                                        IFNULL(CONCAT(' SHIFT - ', SHIFT_NAME), ''),
                                        ')') AS CLASS_NAME
                            FROM
                                ADM_CLASSES_?AC_YEAR CLS
                                    INNER JOIN
                                ADM_APPTYPE_PROG_GROUPS AS AP ON AP.PRO_GROUPS_ID = CLS.PROGRAMME                                    
                                    AND AP.IS_DELETED != 1
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = CLS.SHIFT
                                    AND SS.IS_DELETED != 1
                            WHERE
                                CLS.IS_DELETED != 1 AND CLS.PROGRAMME=?PROGRAMME;";
                        break;
                    }
                case AdmissionSQLCommands.SaveHostelRegistration:
                    {
                        query = @"INSERT INTO ADM_HOSTEL_REGISTRATION
                                            (
                                            RECEIVE_ID,
                                            CLASS_ID,
                                            ROOM_NO,
                                            PARISH_NAME,
                                            COLLEGE_RELATION,
                                            STUDENT_INFO,
                                            ACADEMIC_YEAR)
                                            VALUES
                                            (
                                            ?RECEIVE_ID,
                                            ?CLASS_ID,
                                            ?ROOM_NO,
                                            ?PARISH_NAME,
                                            ?COLLEGE_RELATION,
                                            ?STUDENT_INFO,
                                            ?AC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.UpdateHostelRegistration:
                    {
                        query = @"UPDATE ADM_HOSTEL_REGISTRATION
                                            SET
                                            RECEIVE_ID= ?RECEIVE_ID,
                                            CLASS_ID= ?CLASS_ID,
                                            PARISH_NAME= ?PARISH_NAME,
                                            COLLEGE_RELATION= ?COLLEGE_RELATION,
                                            STUDENT_INFO= ?STUDENT_INFO,
                                            ACADEMIC_YEAR= ?AC_YEAR
                                            WHERE HOSTEL_REGISTRATION_ID= ?HOSTEL_REGISTRATION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateReceiveApplicationByReceiveId:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION
                                                SET
                                                CPINCODE= ?CPINCODE,
                                                PDOORDETAIL= ?PDOORDETAIL,
                                                PSTREET= ?PSTREET,
                                                PVILLAGE_AREA= ?PVILLAGE_AREA,
                                                PTALUK_CITY= ?PTALUK_CITY,
                                                PPINCODE= ?PPINCODE
                                                WHERE RECEIVE_ID= ?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.SaveRelationsDetails:
                    {
                        query = @"INSERT INTO ADM_STU_RELATIONS
                                                (
                                                RECEIVE_ID,
                                                RELATION_NAME,
                                                RELATION_SHIP,
                                                OCCUPATION,
                                                INCOME,
                                                ACADEMIC_YEAR)
                                                VALUES
                                                (
                                                ?RECEIVE_ID,
                                                ?RELATION_NAME,
                                                ?RELATION_SHIP,
                                                ?OCCUPATION,
                                                ?INCOME,
                                                ?AC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.UpdateRelationsDetails:
                    {
                        query = @"UPDATE ADM_STU_RELATIONS
                                                SET
                                                RECEIVE_ID= ?RECEIVE_ID,
                                                RELATION_NAME= ?RELATION_NAME,
                                                RELATION_SHIP= ?RELATION_SHIP,
                                                OCCUPATION= ?OCCUPATION,
                                                INCOME= ?INCOME,
                                                ACADEMIC_YEAR= ?AC_YEAR
                                                WHERE RELATION_ID = ?RELATION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateFamilyPhoto:
                    {
                        query = @"UPDATE ADM_HOSTEL_REGISTRATION SET FAMILY_PHOTO =?FAMILY_PHOTO WHERE RECEIVE_ID =?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateApplicationSubmittionForHostel:
                    {
                        query = @"SET SQL_SAFE_UPDATES=0;
                                    UPDATE ADM_HOSTEL_REGISTRATION SET IS_SUBMITTED =?IS_SUBMITTED , STATUS=?STATUS WHERE RECEIVE_ID =?RECEIVE_ID;
                                    SET SQL_SAFE_UPDATES=0;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelRegisteredListByReligion:
                    {
                        query = @"SELECT 
                                CONCAT(IFNULL(RA.FIRST_NAME, ''),
                                        '. ',
                                        IFNULL(RA.LAST_NAME, '')) AS FIRST_NAME,
                                RA.FATHER_NAME,
                                PARISH_NAME,
                                CONCAT(IFNULL(RA.CVILLAGE_AREA, ''),
                                        '-',
                                        IFNULL(RA.CTALUK_CITY, '')) AS CVILLAGE_AREA,
                                CP.PROGRAMME_NAME,
                                AC.CLASS_NAME AS 'CLASS_ID',
                                R.RELIGION AS 'RELIGION_ID',
                                C.COMMUNITY,
                                COMMUNITY_ID,
                                DATE_FORMAT(RA.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                RA.PHOTO_PATH,
                                IS_ROMAN_CATHOLIC,
                                HR.`STATUS`,
                                AI.ISSUE_ID,
                                RA.SMS_MOBILE_NO,
                                AI.EMAIL_ID,
                                RA.RECEIVE_ID,
                                AI.APPLICATION_NO,
                                HOSTEL_REGISTRATION_ID,
                                AI.SHIFT,
                                IF(HR.COLLEGE_RELATION =1,'YES','NO') AS COLLEGE_RELATION
                            FROM
                                ADM_HOSTEL_REGISTRATION HR
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS RA ON RA.RECEIVE_ID = HR.RECEIVE_ID
                                    AND RA.IS_DELETED != 1
                                    INNER JOIN
                                ADM_ISSUE_APPLICATION AS AI ON AI.ISSUE_ID = RA.ISSUE_ID
                                    AND AI.IS_DELETED != 1
                                    INNER JOIN
                                ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = AI.PROGRAMME_GROUP_ID
                                    AND AAP.IS_DELETED != 1
                                    INNER JOIN
                                CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                    AND CP.IS_DELETED != 1
                                    INNER JOIN
                                ADM_CLASSES_?AC_YEAR AS AC ON AC.CLASS_ID = HR.CLASS_ID
                                    AND AC.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_COMMUNITY C ON C.COMMUNITYID = RA.CASTE_ID
                                    INNER JOIN
                                SUP_RELIGION AS R ON R.RELIGIONID = RA.RELIGION_ID
                                    LEFT JOIN
                                SUP_MOTHER_TONGUE AS MT ON MT.MOTHER_TONGUE_ID = RA.MOTHER_TONGUE
                                    LEFT JOIN
                                SUP_NATIONALITY AS SN ON SN.NATIONALITY_ID = RA.NATIONALITY_ID
                                    LEFT JOIN
                                SUP_ADM_STATUS AS ST ON ST.STATUS_ID = HR.`STATUS`
                            WHERE
                                HR.IS_SUBMITTED = 1 AND HR.IS_DELETED != 1
                                    AND AI.SHIFT = ?SHIFT
                                    AND RA.RELIGION_ID IN (?RELIGION_ID)
                                    AND RA.IS_ROMAN_CATHOLIC = 0 AND (HR.STATUS!=?STATUS AND HR.STATUS!=5);";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentListByReligionForHostelSelection:
                    {
                        query = @"SELECT 
                                    UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                    UPPER(AR.FATHER_NAME) AS FATHER_NAME,
                                    CONCAT(IFNULL(AR.CVILLAGE_AREA, ''),
                                            '-',
                                            IFNULL(AR.CTALUK_CITY, '')) AS CVILLAGE_AREA,
                                    CONCAT(CP.PROGRAMME_NAME,
                                            
                                            '( ',
                                            PM.PROGRAMME_MODE,
                                            ' ) ') AS PROGRAMME_NAME,
                                    PG.PROGRAMME_GROUP_ID,
                                    RL.RELIGION,
                                    MC.MAIN_COMMUNITY,
                                    DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH,
                                    AR.IS_ROMAN_CATHOLIC,
                                    ST.STATUS_NAME,
                                    AI.ISSUED_ID,
                                    AR.SMS_MOBILE_NO,
                                    AR.EMAIL_ID,
                                    AR.RECEIVE_ID,
                                    CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO
                                FROM
                                          fee_collection AS FC
                                            INNER JOIN
                                        fee_transaction AS FT ON FT.TRANSACTION_ID = FC.TRANSACTION_ID
                                            INNER JOIN
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SL ON SL.RECEIVE_ID = FT.STUDENT_ID
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID AND AR.HOSTEL_FACILITY IN (1,2)
                                            AND AR.APPLICATION_TYPE_ID=?APPLICATION_TYPE_ID
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SL.RECEIVE_ID
                                        AND SL.PROGRAMME_ID = AI.PROGRAMME_GROUP_ID
                                        AND AI.STATUS=5
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID AND PG.IS_DELETED!=1
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                      
                                        INNER JOIN
                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                        LEFT JOIN
                                    SUP_RELIGION AS RL ON RL.RELIGIONID = AR.RELIGION_ID
                                        LEFT JOIN
                                    SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                                        LEFT JOIN
                                    GROUP_COMMUNITY AS COM ON COM.COMMUNITY_ID = AR.CASTE_ID
                                        LEFT JOIN
                                    SUP_MAIN_COMMUNITY AS MC ON MC.MAIN_COMMUNITY_ID = COM.MAIN_COMMUNITY_ID
                                WHERE
                                    FC.FREQUENCY = 3
        AND FT.IS_AMOUNT_COLLECTED = 1 AND IS_ROMAN_CATHOLIC = 0 AND AR.RELIGION_ID IN (?RELIGION_ID) AND (SL.IS_DELETED != 1 AND SL.IS_CANCELED!=1)
        AND SL.RECEIVE_ID NOT IN (SELECT 
            RECEIVE_ID
        FROM
            ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS HS
        WHERE
            HS.RECEIVE_ID = SL.RECEIVE_ID
                AND HS.IS_DELETED != 1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelRegisteredListByMinority:
                    {
                        query = @"SELECT 
                                CONCAT(IFNULL(RA.FIRST_NAME, ''),
                                        '. ',
                                        IFNULL(RA.LAST_NAME, '')) AS FIRST_NAME,
                                RA.FATHER_NAME,
                                PARISH_NAME,
                                CONCAT(IFNULL(RA.CVILLAGE_AREA, ''),
                                        '-',
                                        IFNULL(RA.CTALUK_CITY, '')) AS CVILLAGE_AREA,
                                CP.PROGRAMME_NAME,
                                AC.CLASS_NAME AS 'CLASS_ID',
                                R.RELIGION AS 'RELIGION_ID',
                                C.COMMUNITY,
                                COMMUNITY_ID,
                                DATE_FORMAT(RA.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                RA.PHOTO_PATH,
                                IS_ROMAN_CATHOLIC,
                                HR.`STATUS`,
                                AI.ISSUE_ID,
                                RA.SMS_MOBILE_NO,
                                AI.EMAIL_ID,
                                RA.RECEIVE_ID,
                                AI.APPLICATION_NO,
                                HOSTEL_REGISTRATION_ID,
                                AI.SHIFT,
                                IF(HR.COLLEGE_RELATION =1,'YES','NO') AS COLLEGE_RELATION
                            FROM
                                ADM_HOSTEL_REGISTRATION HR
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS RA ON RA.RECEIVE_ID = HR.RECEIVE_ID
                                    AND RA.IS_DELETED != 1
                                    INNER JOIN
                                ADM_ISSUE_APPLICATION AS AI ON AI.ISSUE_ID = RA.ISSUE_ID
                                    AND AI.IS_DELETED != 1
                                    INNER JOIN
                                ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = AI.PROGRAMME_GROUP_ID
                                    AND AAP.IS_DELETED != 1
                                    INNER JOIN
                                CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                    AND CP.IS_DELETED != 1
                                    INNER JOIN
                                ADM_CLASSES_?AC_YEAR AS AC ON AC.CLASS_ID = HR.CLASS_ID
                                    AND AC.IS_DELETED != 1
                                    LEFT JOIN
                                SUP_COMMUNITY C ON C.COMMUNITYID = RA.CASTE_ID
                                    INNER JOIN
                                SUP_RELIGION AS R ON R.RELIGIONID = RA.RELIGION_ID
                                    LEFT JOIN
                                SUP_MOTHER_TONGUE AS MT ON MT.MOTHER_TONGUE_ID = RA.MOTHER_TONGUE
                                    LEFT JOIN
                                SUP_NATIONALITY AS SN ON SN.NATIONALITY_ID = RA.NATIONALITY_ID
                                    LEFT JOIN
                                SUP_ADM_STATUS AS ST ON ST.STATUS_ID = HR.`STATUS` 
                            WHERE
                                HR.IS_SUBMITTED = 1 AND HR.IS_DELETED != 1
                                    AND AI.SHIFT = ?SHIFT
                                    AND IS_ROMAN_CATHOLIC = ?IS_ROMAN_CATHOLIC AND (HR.STATUS !=?STATUS AND HR.STATUS!=5);";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentListByMinorityForHostelSelection:
                    {
                        //                query = @"SELECT 
                        //                            UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                        //                            UPPER(AR.FATHER_NAME) AS FATHER_NAME,
                        //                            CONCAT(IFNULL(AR.CVILLAGE_AREA, ''),
                        //                                    '-',
                        //                                    IFNULL(AR.CTALUK_CITY, '')) AS CVILLAGE_AREA,
                        //                            CONCAT(CP.PROGRAMME_NAME,

                        //                                    '( ',
                        //                                    PM.PROGRAMME_MODE,
                        //                                    ' ) ') AS PROGRAMME_NAME,
                        //                            RL.RELIGION,
                        //                            PG.PROGRAMME_GROUP_ID,
                        //                            MC.MAIN_COMMUNITY,
                        //                            DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH,
                        //                            AR.IS_ROMAN_CATHOLIC,
                        //                            ST.STATUS_NAME,
                        //                            AI.ISSUED_ID,
                        //                            AR.SMS_MOBILE_NO,
                        //                            AR.EMAIL_ID,
                        //                            AR.RECEIVE_ID,
                        //                            CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUED_ID,4,'0')) AS APPLICATION_NO
                        //                        FROM
                        //                            ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                        //                                INNER JOIN
                        //                            ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID AND AR.HOSTEL_FACILITY = 1 AND AR.APPLICATION_TYPE_ID=?APPLICATION_TYPE_ID
                        //                                INNER JOIN
                        //                            ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SL.RECEIVE_ID
                        //                                AND SL.PROGRAMME_ID = AI.PROGRAMME_GROUP_ID
                        //                                AND AI.STATUS=5
                        //                                INNER JOIN
                        //                            CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID 
                        //                                INNER JOIN
                        //                            CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                        //                                INNER JOIN
                        //                            SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                        //                                INNER JOIN
                        //                            SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                        //                                LEFT JOIN
                        //                            SUP_RELIGION AS RL ON RL.RELIGIONID = AR.RELIGION_ID
                        //                                LEFT JOIN
                        //                            SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                        //                                LEFT JOIN                                 
                        //                            GROUP_COMMUNITY AS COM ON COM.COMMUNITY_ID = AR.CASTE_ID
                        //                                LEFT JOIN
                        //                            SUP_MAIN_COMMUNITY AS MC ON MC.MAIN_COMMUNITY_ID = COM.MAIN_COMMUNITY_ID
                        //                        WHERE
                        //                            PG.SHIFT = ?SHIFT AND IS_ROMAN_CATHOLIC = 1 AND (SL.IS_DELETED != 1 AND SL.IS_CANCELED!=1)
                        //AND SL.RECEIVE_ID NOT IN (SELECT 
                        //    RECEIVE_ID
                        //FROM
                        //    ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS HS
                        //WHERE
                        //    HS.RECEIVE_ID = SL.RECEIVE_ID 
                        //        AND HS.IS_DELETED != 1);";

                        query = @"SELECT 
                                                    UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                                    UPPER(AR.FATHER_NAME) AS FATHER_NAME,
                                                    CONCAT(IFNULL(AR.CVILLAGE_AREA, ''),
                                                            '-',
                                                            IFNULL(AR.CTALUK_CITY, '')) AS CVILLAGE_AREA,
                                                    CONCAT(CP.PROGRAMME_NAME,
                                                            '( ',
                                                            PM.PROGRAMME_MODE,
                                                            ' ) ') AS PROGRAMME_NAME,
                                                    RL.RELIGION,
                                                    PG.PROGRAMME_GROUP_ID,
                                                    MC.MAIN_COMMUNITY,
                                                    DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH,
                                                    AR.IS_ROMAN_CATHOLIC,
                                                    ST.STATUS_NAME,
                                                    AI.ISSUED_ID,
                                                    AR.SMS_MOBILE_NO,
                                                    AR.EMAIL_ID,
                                                    AR.RECEIVE_ID,
                                                    CONCAT(AI.APPLICATION_NO,
                                                            LPAD(AI.ISSUE_NO, 4, '0')) AS APPLICATION_NO
                                                FROM
                                                    fee_collection AS FC
                                                        INNER JOIN
                                                    fee_transaction AS FT ON FT.TRANSACTION_ID = FC.TRANSACTION_ID
                                                        INNER JOIN
                                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SL ON SL.RECEIVE_ID = FT.STUDENT_ID
                                                        INNER JOIN
                                                    ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                                        AND AR.HOSTEL_FACILITY IN (1,2)
                                                        AND AR.APPLICATION_TYPE_ID = ?APPLICATION_TYPE_ID
                                                        INNER JOIN
                                                    ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SL.RECEIVE_ID
                                                        AND SL.PROGRAMME_ID = AI.PROGRAMME_GROUP_ID
                                                        AND AI.STATUS = 5
                                                        INNER JOIN
                                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                                                        INNER JOIN
                                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                        INNER JOIN
                                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                        LEFT JOIN
                                                    SUP_RELIGION AS RL ON RL.RELIGIONID = AR.RELIGION_ID
                                                        LEFT JOIN
                                                    SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                                                        LEFT JOIN
                                                    GROUP_COMMUNITY AS COM ON COM.COMMUNITY_ID = AR.CASTE_ID
                                                        LEFT JOIN
                                                    SUP_MAIN_COMMUNITY AS MC ON MC.MAIN_COMMUNITY_ID = COM.MAIN_COMMUNITY_ID
                                                WHERE
                                                    FC.FREQUENCY = 3
                                                        AND FT.IS_AMOUNT_COLLECTED = 1
                                                        AND IS_ROMAN_CATHOLIC = 1
                                                        AND (SL.IS_DELETED != 1
                                                        AND SL.IS_CANCELED != 1)
                                                        AND SL.RECEIVE_ID NOT IN (SELECT 
                                                            RECEIVE_ID
                                                        FROM
                                                            ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS HS
                                                        WHERE
                                                            HS.RECEIVE_ID = SL.RECEIVE_ID
                                                                AND HS.IS_DELETED != 1);";
                        break;
                    }
                case AdmissionSQLCommands.SaveHostelSelection:
                    {
                        query = @"UPDATE ADM_HOSTEL_REGISTRATION 
                                SET 
                                    HOSTEL_ID = ?HOSTEL_ID,
                                    ROOM_NO = ?ROOM_NO,
                                    STATUS=?STATUS,
                                    SELECTION_DATE=CURDATE(),
                                    SELECTED_BY=?SELECTED_BY
                                WHERE
                                    HOSTEL_REGISTRATION_ID = ?HOSTEL_REGISTRATION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertHostelSelection:
                    {
                        query = @"INSERT INTO `adm_hostel_selection_process_?AC_YEAR`
                                (
                                `RECEIVE_ID`,
                                `APPLICATION_NO`,
                                `ISSUED_ID`,
                                `PROGRAMME_GROUP_ID`,
                                `HOSTEL_ID`,
                                `SELECTED_BY`,
                                `STATUS`)
                                VALUES
                                (?RECEIVE_ID,?APPLICATION_NO,?ISSUED_ID,?PROGRAMME_GROUP_ID,?HOSTEL_ID,?SELECTED_BY,?STATUS);";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmissionProgrammeIncharge:
                    {
                        query = @"SELECT 
                                PI.INCHARGE_ID,
                                PG.PROGRAMME_GROUP_ID,
                                CP.PROGRAMME_ID,
                                PG.SHIFT,
                                PM.PROGRAMME_MODE_ID,
                                PI.IS_ACTIVE,
                                 PI.ACADEMIC_YEAR,
                                PM.PROGRAMME_MODE,
                                CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                        ' (',
                                        IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                        ')') AS PROGRAMME_NAME,
                                CONCAT(IFNULL(FIRST_NAME, ''),
                                        '',
                                        ' ',
                                        IFNULL(LAST_NAME, ''),
                                        '(',
                                        STAFF_CODE,
                                        ')') AS FIRST_NAME
                            FROM
                                ADM_PROGRAMME_INCHARGE AS PI
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = PI.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                STF_PERSONAL_INFO AS ST ON ST.STAFF_ID = PI.STAFF_ID
                                    AND ST.IS_DELETED != 1
                            WHERE
                                PI.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStaffListByProgramme:
                    {
                        query = @"SELECT 
                                ST.STAFF_ID,
                                PI.PROGRAMME_GROUP_ID,
                                IS_ACTIVE,
                                CONCAT(IFNULL(FIRST_NAME, ''),
                                        '',
                                        ' ',
                                        IFNULL(LAST_NAME, ''),
                                        '(',
                                        STAFF_CODE,
                                        ')') AS FIRST_NAME,
                                IF(PI.PROGRAMME_GROUP_ID,
                                    'SELECTED',
                                    '') AS SELECTED
                            FROM
                                STF_PERSONAL_INFO ST
                                    LEFT JOIN
                                ADM_PROGRAMME_INCHARGE AS PI ON PI.STAFF_ID = ST.STAFF_ID
                                    AND PI.IS_DELETED != 1
                                    AND PROGRAMME_GROUP_ID = ?PROGRAMME_GROUP_ID  AND IS_ACTIVE=1 
                            WHERE
                                ST.IS_DELETED != 1 AND ROLES=4
                            ORDER BY ST.STAFF_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStaffByProgrammeGroupId:
                    {
                        query = @"SELECT INCHARGE_ID, STAFF_ID, PROGRAMME_GROUP_ID, ACADEMIC_YEAR, IS_ACTIVE FROM ADM_PROGRAMME_INCHARGE WHERE STAFF_ID=?STAFF_ID AND PROGRAMME_GROUP_ID=?PROGRAMME_GROUP_ID AND IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateInActiveByProgramme:
                    {
                        query = @"UPDATE ADM_PROGRAMME_INCHARGE SET IS_ACTIVE=0 WHERE PROGRAMME_GROUP_ID=?PROGRAMME_GROUP_ID AND INCHARGE_ID!=0;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateActiveByInChargeId:
                    {
                        query = @"UPDATE ADM_PROGRAMME_INCHARGE SET IS_ACTIVE=?IS_ACTIVE WHERE INCHARGE_ID=?INCHARGE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateInActiveByInChargeId:
                    {
                        query = @"UPDATE ADM_PROGRAMME_INCHARGE SET IS_ACTIVE=0 WHERE INCHARGE_ID=?INCHARGE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertAdmissionIncharge:
                    {
                        query = @"INSERT INTO ADM_PROGRAMME_INCHARGE
                                (
                                STAFF_ID,
                                PROGRAMME_GROUP_ID,
                                ACADEMIC_YEAR,
                                IS_ACTIVE
                                )
                                VALUES
                                (
                                ?STAFF_ID,
                                ?PROGRAMME_GROUP_ID,
                                ?ACADEMIC_YEAR,
                                ?IS_ACTIVE
                                );";
                        break;
                    }
                case AdmissionSQLCommands.CheckStaffExistInUserAccount:
                    {
                        query = @"SELECT USER_ACCOUNT_ID FROM USER_ACCOUNT WHERE USER_ID=?USER_ID AND ROLE=4 AND USER_TYPE=4 AND IS_DELETED!=1";
                        break;
                    }
                case AdmissionSQLCommands.FetchStaffInfoForUserAccount:
                    {
                        query = @"SELECT STAFF_ID,FIRST_NAME,STAFF_CODE,DATE_FORMAT(DATE_OF_BIRTH,'%Y/%m/%d')AS DATE_OF_BIRTH FROM STF_PERSONAL_INFO WHERE STAFF_ID=?STAFF_ID AND IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentDetailsForPrintView:
                    {
                        query = @"SELECT 
                                         AD.ISSUE_ID,
                                        ADR.RECEIVE_ID,
                                        AD.APPLICATION_TYPE_ID,
                                        CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                ' (',
                                IFNULL(CONCAT(SPM.PROGRAMME_MODE), ''),
                                ')') AS PROGRAMME_NAME,
                                        CP.PROGRAMME_CODE,
                                        DATE_FORMAT(AD.ISSUE_DATE,'%d/%m/%Y') AS ISSUE_DATE,
                                        AD.APPLICATION_NO,
                                        AD.FIRST_NAME,
                                        AD.LAST_NAME,
                                        AD.CONTACT_NO,
                                        AD.EMAIL_ID,
                                        DATE_FORMAT(AD.DOB,'%d/%m/%Y') AS DOB,
                                        CLS.CLASS_NAME,
                                        ADR.FATHER_NAME,
                                        SC.COMMUNITY AS CASTE,
                                        SR.RELIGION  AS RELIGION_ID,
                                        SO.OCCUPATION_NAME AS OCCUPATION,
                                        ADR.ANNUAL_INCOME,
                                        ADR.CDOORDETAIL,
                                        ADR.CSTREET,
                                        ADR.CPOST_PLACE_TOWN,
                                        ADR.CTALUK_CITY,
                                        ADR.CPINCODE,
                                        ADR.CDISTRICT,
                                        ADR.CVILLAGE_AREA,
                                        ADR.ACADEMIC_YEAR,
                                        ADR.PLACE_OF_BIRTH,
                                        ADR.IS_ROMAN_CATHOLIC,
                                        ADR.PHOTO_PATH,
                                        AHR.ROOM_NO,
                                        AHR.PARISH_NAME,
                                        AHR.COLLEGE_RELATION,
                                        AHR.STUDENT_INFO,
                                        AHR.FAMILY_PHOTO,
                                        SAS.STATUS_NAME,
                                        AHR.HOSTEL_REGISTRATION_ID,
                                        AHR.IS_SUBMITTED AS HOSTEL_SUBMITTED,
                                        BL.BLOOD_GROUP_NAME AS BLOOD_GROUP,
                                        ADR.EXTRA_CURRICULAR
                                    FROM
                                         ADM_ISSUE_APPLICATION AS AD LEFT JOIN ADM_RECEIVE_APPLICATION AS ADR ON ADR.ISSUE_ID=AD.ISSUE_ID
                                        INNER JOIN ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID=AD.PROGRAMME_GROUP_ID
                                        INNER JOIN CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID=AAP.PROGRAMME_ID
                                        LEFT JOIN ADM_SELECTION_PROCESS_?AC_YEAR AS ASP ON ASP.RECEIVE_ID=ADR.RECEIVE_ID
                                        LEFT JOIN ADM_HOSTEL_REGISTRATION AS AHR ON AHR.RECEIVE_ID=ADR.RECEIVE_ID
                                        LEFT JOIN SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID=ADR.STATUS
                                        INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=AAP.SHIFT
                                        INNER JOIN ADM_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID=AHR.CLASS_ID
                                        INNER JOIN SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID=AAP.PROGRAMME_MODE
                                        INNER JOIN SUP_COMMUNITY AS SC ON SC.COMMUNITYID=AD.CASTE
                                        INNER JOIN SUP_RELIGION AS SR ON SR.RELIGIONID=ADR.RELIGION_ID
                                        INNER JOIN SUP_OCCUPATION AS SO ON SO.OCCUPATION_ID=ADR.OCCUPATION
                                        INNER JOIN SUP_BLOOD_GROUP AS BL ON BL.BLOOD_GROUP_ID =ADR.BLOOD_GROUP
                                    WHERE
                                        AD.ISSUE_ID = ?ISSUE_ID AND AD.IS_DELETED!=1 AND ADR.IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchRelastionsInfoForPrint:
                    {
                        query = @"SELECT 
                                        ASR.RELATION_ID,
                                        RECEIVE_ID,
                                        RELATION_NAME,
                                        SR.RELATION  AS RELATION_SHIP,
                                        SO.OCCUPATION_NAME AS OCCUPATION,
                                        INCOME,
                                        ACADEMIC_YEAR
                                    FROM
                                        ADM_STU_RELATIONS AS ASR INNER JOIN SUP_RELATION AS SR ON SR.RELATION_ID=ASR.RELATION_SHIP
                                        INNER JOIN SUP_OCCUPATION AS SO ON SO.OCCUPATION_ID=ASR.OCCUPATION
                                    WHERE
                                        RECEIVE_ID = ?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchTransferedStudentsForTransaction:
                    {
                        query = @"SELECT 
                                                AT.TRANSFER_ID,
                                                AT.APPLICATION_NO,
                                                AT.REQUEST_ID,
                                                CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                                        '. ',
                                                        IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                                CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                                        ' (',
                                                        IFNULL(SPM.PROGRAMME_MODE, ''),
                                                        ') - SHIFT -',
                                                        IFNULL(SH.SHIFT_NAME, '')) AS PROGRAMME_FROM,
                                                CONCAT(IFNULL(CP1.PROGRAMME_NAME, ''),
                                                        ' (',
                                                        IFNULL(SPM1.PROGRAMME_MODE, ''),
                                                        ') - SHIFT -',
                                                        IFNULL(SH1.SHIFT_NAME, '')) AS PROGRAMME_TO,
                                                AT.RECEIVE_ID,
                                                CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                        '( ',
                                                        IFNULL(SP.STAFF_CODE, ''),
                                                        ')') AS REQUEST_ID,
                                                AR.HSTOTAL,
                                                AR.HS_MAX_MARK,
                                                AT.IS_APPROVED
                                            FROM
                                                ADM_TRANSFER AS AT
                                                    INNER JOIN
                                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AT.RECEIVE_ID
                                                    INNER JOIN
                                                ADM_APPTYPE_PROG_GROUPS AS APP ON APP.PRO_GROUPS_ID = AT.PROGRAMME_FROM
                                                    INNER JOIN
                                                CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = APP.PROGRAMME_ID
                                                    INNER JOIN
                                                SUP_SHIFT AS SH ON SH.SHIFT_ID = APP.SHIFT
                                                    INNER JOIN
                                                SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = APP.PROGRAMME_MODE
                                                    INNER JOIN
                                                ADM_APPTYPE_PROG_GROUPS AS APP1 ON APP1.PRO_GROUPS_ID = AT.programme_to
                                                    INNER JOIN
                                                CP_PROGRAMME_?AC_YEAR AS CP1 ON CP1.PROGRAMME_ID = APP1.PROGRAMME_ID
                                                    INNER JOIN
                                                SUP_SHIFT AS SH1 ON SH1.SHIFT_ID = APP1.SHIFT
                                                    INNER JOIN
                                                SUP_PROGRAMME_MODE AS SPM1 ON SPM1.PROGRAMME_MODE_ID = APP1.PROGRAMME_MODE
                                                    INNER JOIN
                                                STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = AT.REQUEST_ID
                                            WHERE
                                                AT.ACADEMIC_YEAR = ?AC_YEAR AND AT.IS_APPROVED=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmittedStudentInfoByIssueId:
                    {
                        query = @"SELECT 
                                    ADI.ISSUED_ID,
                                    AR.APPLICATION_TYPE_ID,
                                    ATP.PROGRAMME_ID,
                                    AR.FIRST_NAME,
                                    AR.SMS_MOBILE_NO,
                                    AR.EMAIL_ID,
                                    S.SHIFT_NAME,
                                    AR.HSC_NO,
                                    APPLICATION_TYPE,
                                    APP_FEE,
                                    ADI.PROGRAMME_GROUP_ID,
                                    concat(ADI.APPLICATION_NO,LPAD(ADI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                    CONCAT(PR.PROGRAMME_NAME,
                                            ' (',
                                            S.DESCRIPTION,
                                            ' ',
                                            S.TIME,
                                            ')') AS PROGRAMME_NAME,
                                    AR.HSTOTAL,
                                    AR.TOTAL_CUT_OFF_MARK,
                                    AR.HS_MAX_MARK,
                                    SS.STATUS_NAME,
                                    AR.RECEIVE_ID,
                                    AR.GENDER,ADI.ACADEMIC_YEAR
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS ADI
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = ADI.RECEIVE_ID
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS ATP ON ATP.PROGRAMME_GROUP_ID = ADI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    ADM_APPLICATION_TYPE AS APP ON APP.APPLICATION_TYPE_ID = ATP.APPTYPE_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS PR ON PR.PROGRAMME_ID = ATP.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_SHIFT AS S ON S.SHIFT_ID = ATP.SHIFT
                                        INNER JOIN
                                    SUP_ADM_STATUS AS SS ON SS.STATUS_ID = ADI.STATUS
                                WHERE
		                                ADI.ISSUED_ID =?ISSUE_ID
                                        AND ADI.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.SaveCancelStudent:
                    {
                        query = @"INSERT INTO ADM_ADMISSION_CANCEL
                                (
                                PROGRAMME_ID,
                                RECEIVE_ID,
                                APPLICATION_NO,
								REASON,
                                ACADEMIC_YEAR,IS_HOSTEL_CANCEL)
                                VALUES
                                (?PROGRAMME_ID,
                                ?RECEIVE_ID,
                                ?APPLICATION_NO,
                                ?REASON,
                                ?ACADEMIC_YEAR,?IS_HOSTEL_CANCEL
                                );";
                        break;
                    }
                case AdmissionSQLCommands.UpdateCancelStudent:
                    {
                        query = @"UPDATE ADM_CANCEL_ADMISSION_?AC_YEAR
                                 SET
                                 APPLICATION_TYPE_ID =?APPLICATION_TYPE_ID,
                                 APPLICATION_NO = ?APPLICATION_NO,
                                 MODE_ID = ?MODE_ID,
                                 PROGRAMME_ID =?PROGRAMME_ID,
                                 CLASS_ID =?CLASS_ID,
                                 ADMISSION_NO =?ADMISSION_NO,
                                 ADMISSION_DATE =?ADMISSION_DATE,
                                 FIRST_NAME = ?FIRST_NAME,
                                 LAST_NAME = ?LAST_NAME,
                                 GENDER_ID = ?GENDER_ID,
                                 DATE_OF_BIRTH = ?DATE_OF_BIRTH,
                                 STUDENT_ID = ?STUDENT_ID,
                                 ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                 WHERE CANCEL_ADMISSION_ID = ?CANCEL_ADMISSION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchFeePaidInfo:
                    {
                        query = @"SELECT 
	                                SUM(SA.CREDIT) AS 'AMOUNT',
                                    SUM(SA.DEBIT) AS 'PAID'
                                FROM
                                    ADM_RECEIVE_APPLICATION AS R
		                                INNER JOIN
	                                FEE_STUDENT_ACCOUNT AS SA ON SA.STUDENT_ID=R.RECEIVE_ID AND SA.IS_DELETED!=1
                                WHERE RECEIVE_ID=?RECEIVE_ID AND R.IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgramWiseQuata:
                    {
                        query = @"SELECT 
                                                MT.INTAKE_ID,
                                                MT.DEPARTMENT_ID,
                                                MT.SHIFT,
                                                (MT.NO_OF_SEATS) AS NO_OF_SEATS,
                                                MT.MINORITY,
                                                MT.ACADEMIC_YEAR,
                                                MT.PROGRAMME_ID,
                                                P.PROGRAMME_NAME AS PROGRAMME_NAME,
                                                CQ.CASTE_ID,
                                                CQ.NO_OF_SEATS AS 'CQ_SEATS'
                                            FROM
                                                ADM_MAXIMUM_IN_TAKES AS MT
                                                    INNER JOIN
                                                cp_programme_group AS CPG ON CPG.PROGRAMME_GROUP_ID = MT.PROGRAMME_ID
                                                    AND CPG.IS_DELETED != 1
                                                    INNER JOIN
                                                ADM_CASTEWISE_QUATA AS CQ ON CQ.INTAKE_ID = MT.INTAKE_ID
                                                    AND  CQ.ACADEMIC_YEAR=?AC_YEAR
                                                    AND CQ.IS_DELETED != 1
                                                    INNER JOIN
                                                CP_PROGRAMME AS P ON P.PROGRAMME_ID = CPG.PROGRAMME_ID
                                                    AND P.IS_DELETED != 1
                                                  INNER JOIN
											SUP_MAIN_COMMUNITY AS SM ON SM.MAIN_COMMUNITY_ID=CQ.CASTE_ID
                                            WHERE
                                                    MT.PROGRAMME_ID IN (?PROGRAMME_ID)
                                                    AND CQ.CASTE_ID IN (?COMMUNITYID)
                                                    AND MT.ACADEMIC_YEAR=?AC_YEAR
                                                    AND MT.IS_DELETED != 1 ORDER BY SM.ORDER_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchCasteWiseQuata:
                    {
                        //query = @"SELECT 
                        //            ISS.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID',
                        //            GC.MAIN_COMMUNITY_ID AS 'CASTE_ID'
                        //        FROM
                        //            ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                        //                INNER JOIN
                        //            ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                        //                AND REC.IS_DELETED != 1
                        //                INNER JOIN
                        //            ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                        //                AND ISS.IS_DELETED != 1
                        //                AND ISS.STATUS >= 3
                        //                INNER JOIN
                        //            GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                        //        WHERE
                        //                SE.PROGRAMME_ID = ?PROGRAMME_ID
                        //                AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                        //                AND SE.SELECTION_TYPE != 1
                        //                AND (SE.IS_DELETED != 1 AND SE.IS_CANCELED!=1);";
                        query = @"SELECT 
                                    ISS.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID',
                                    GC.MAIN_COMMUNITY_ID AS 'CASTE_ID'
                                FROM
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                                        AND REC.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                                        AND ISS.IS_DELETED != 1
                                        AND ISS.STATUS >= 3
                                        INNER JOIN
                                    GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                                         INNER JOIN
                                    sup_main_community AS SM ON SM.MAIN_COMMUNITY_ID=GC.MAIN_COMMUNITY_ID
                                WHERE
                                        SE.PROGRAMME_ID = ?PROGRAMME_ID
                                        AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                                        AND SE.SELECTION_TYPE != 1
                                        AND (SE.IS_DELETED != 1 AND SE.IS_CANCELED!=1) ORDER BY SM.ORDER_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchTransferById:
                    {
                        query = @"SELECT 
                                        TRANSFER_ID,
                                        RECEIVE_ID,
                                        REQUEST_ID,
                                        APPLICATION_NO,
                                        PROGRAMME_FROM,
                                        PROGRAMME_TO,
                                        APPROVE_ID,
                                        ACADEMIC_YEAR,
                                        IS_APPROVED
                                    FROM
                                        ADM_TRANSFER
                                    WHERE
                                        TRANSFER_ID = ?TRANSFER_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchFeeDetailsByProgramme:
                    {
                        query = @"SELECT 
                                        FS.FEE_STRUCTURE_ID,
                                        FFF.FEE_FREQUENCY_FEE_MODE_ID AS FREQUENCY,
                                        SFF.FREQUENCY_NAME,
                                        SFM.MAIN_HEAD_ID,
                                        SFM.MAIN_HEAD,
                                        SUM(FS.AMOUNT) AS AMOUNT,
                                        SH.HEAD_ID,
                                        SH.HEAD
                                    FROM
                                        FEE_STRUCTURE AS FS
                                            INNER JOIN
                                        FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY
                                            AND FS.PROGRAMME = ?PROGRAMME_ID
                                            AND FFF.ACADEMIC_YEAR = ?AC_YEAR
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS APP ON APP.PRO_GROUPS_ID = FS.PROGRAMME
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                            AND SFF.ACADEMIC_YEAR = ?AC_YEAR
                                            INNER JOIN
                                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                            AND SST.IS_ACTIVE = 1
                                            INNER JOIN
                                        SUP_HEAD AS SH ON SH.HEAD_ID = FS.HEAD
                                            AND SH.IS_DELETED != 1
                                            INNER JOIN
                                        FEE_MAIN_HEADS AS FMH ON FMH.HEAD_ID = SH.HEAD_ID
                                            AND APP.PROGRAMME_MODE = FMH.PROGRAMME_MODE
                                            AND APP.SHIFT = FMH.SHIFT
                                            AND FMH.ACADEMIC_YEAR = ?AC_YEAR
                                            AND FMH.FREQUENCY_ID = FS.FREQUENCY
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS SFM ON SFM.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                    WHERE
                                        FS.PROGRAMME = ?PROGRAMME_ID AND FS.IS_DELETED != 1 AND FS.FREQUENCY=?FREQUENCY_ID
                                            AND FS.ACADEMIC_YEAR = ?AC_YEAR GROUP BY SFM.MAIN_HEAD_ID;";
                        break;
                    }
                case AdmissionSQLCommands.DeleteStudentRelation:
                    {
                        query = @"UPDATE ADM_STU_RELATIONS SET IS_DELETED=1 WHERE RELATION_ID=?RELATION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeAndClassByProgrammeGroupId:
                    {
                        query = @"SELECT 
                                        APP.PRO_GROUPS_ID AS PROGRAMME_GROUP_ID, CLS.CLASS_ID, APP.PROGRAMME_ID
                                    FROM
                                        ADM_APPTYPE_PROG_GROUPS AS APP
                                            INNER JOIN
                                        ADM_CLASSES_2018 AS CLS ON CLS.PROGRAMME = APP.PRO_GROUPS_ID
                                            AND CLS.SHIFT = APP.SHIFT
                                            AND CLS.PROGRAMME_MODE = APP.PROGRAMME_MODE
                                    WHERE
                                        APP.PRO_GROUPS_ID = ?PROGRAMME_GROUP_ID
                                            AND CLS.IS_DELETED != 1
                                            AND APP.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateIssueProgrammeAndProgrammeGroupIdForTransfer:
                    {
                        query = @"UPDATE ADM_ISSUE_APPLICATION SET PROGRAMME_GROUP_ID=?PROGRAMME_GROUP_ID , PROGRAMME_ID=?PROGRAMME_ID WHERE ISSUE_ID=?ISSUE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateReceiveProgrammeAndProgrammeGroupIdForTransfer:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION SET PROGGROUP_ID=?PROGRAMME_GROUP_ID , PROGRAMME_ID=?PROGRAMME_ID WHERE RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateSelectionProcessdProgrammeGroupIdForTransfer:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR SET PROGRAMME_ID=?PROGRAMME_GROUP_ID WHERE RECEIVE_ID=?RECEIVE_ID AND (IS_DELETED AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.UpdateHostelRegistrationClassForTransfer:
                    {
                        query = @"UPDATE ADM_HOSTEL_REGISTRATION SET CLASS_ID=?CLASS_ID WHERE RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHeadByReceiveId:
                    {
                        query = @"SELECT 
                                        FFF.FEE_FREQUENCY_FEE_MODE_ID AS FREQUENCY_ID,
                                        SFF.FREQUENCY_NAME ,
                                        SFM.MAIN_HEAD_ID ,
                                        SFM.MAIN_HEAD ,
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
											INNER JOIN
										FEE_PAYU_RESPONSE_?AC_YEAR AS FPR ON FPR.UDF1=?UDF1 AND FPR.UDF2=?UDF2
                                    WHERE
                                          FSA.IS_DELETED != 1
                                            AND FSA.IS_CANCELLED_HEAD != 1
                                            AND FSA.ACADEMIC_YEAR = ?AC_YEAR
                                    GROUP BY SFM.MAIN_HEAD_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchUnPaidStudentListByCycle:
                    {
                        query = @"SELECT 
                                    ISS.APPLICATION_NO,
                                    R.RECEIVE_ID,
                                    R.FIRST_NAME,
                                    R.LAST_NAME,
                                    P.PROGRAMME_ID,
                                    P.PROGRAMME_NAME,
                                    ISS.CONTACT_NO AS 'MOBILE_NO',
                                    ISS.EMAIL_ID,
                                    S.SHIFT_NAME AS 'SHIFT',
                                    SR.RELIGION,
                                    C.COMMUNITY AS 'CASTE_ID'
                                FROM
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SP
		                                INNER JOIN
	                                ADM_RECEIVE_APPLICATION AS R ON R.RECEIVE_ID=SP.RECEIVE_ID AND R.IS_DELETED!=1 AND R.STATUS!=7
		                                INNER JOIN
	                                ADM_ISSUE_APPLICATION AS ISS ON ISS.ISSUE_ID=R.ISSUE_ID AND ISS.IS_DELETED!=1
		                                INNER JOIN
	                                ADM_APPTYPE_PROG_GROUPS AS APP ON APP.PRO_GROUPS_ID=ISS.PROGRAMME_GROUP_ID AND APP.IS_DELETED!=1
		                                INNER JOIN
	                                CP_PROGRAMME_?AC_YEAR AS P ON P.PROGRAMME_ID=APP.PROGRAMME_ID AND P.IS_DELETED!=1
		                                INNER JOIN
	                                SUP_SHIFT AS S ON S.SHIFT_ID=ISS.SHIFT
		                                INNER JOIN
	                                SUP_RELIGION AS SR ON SR.RELIGIONID=ISS.RELIGION AND SR.ISDELETED!=1
		                                INNER JOIN
	                                SUP_COMMUNITY AS C ON C.COMMUNITYID=ISS.CASTE
                                WHERE SP.IS_FEE_PAID!=1 AND SELECTION_CYCLE= ?SELECTION_CYCLE;";
                        break;
                    }
                case AdmissionSQLCommands.CheckHostelEligibility:
                    {
                        query = @"SELECT 
                                    AI.ISSUE_ID,RA.RECEIVE_ID,HOSTEL_FACILITY,RA.STATUS,S.STATUS_NAME
                                FROM
                                    ADM_RECEIVE_APPLICATION AS RA
                                        INNER JOIN
                                    ADM_ISSUE_APPLICATION AS AI ON RA.ISSUE_ID = AI.ISSUE_ID
                                        AND AI.IS_DELETED != 1
                                        INNER JOIN SUP_ADM_STATUS AS S ON RA.STATUS =S.STATUS_ID
                                WHERE
                                    RA.IS_DELETED != 1 AND STATUS =?STATUS
                                        AND HOSTEL_FACILITY = 1 AND AI.ISSUE_ID=?ISSUE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.GetSelectionIdByReceiveId:
                    {
                        query = @"SELECT 
                                        SELECTION_ID
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR
                                    WHERE
                                        RECEIVE_ID = ?RECEIVE_ID AND PROGRAMME_ID=?PROGRAMME_GROUP_ID AND (IS_DELETED != 1 AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.GetRegisteredStudent:
                    {
                        query = @"SELECT 
                                        AI.PROGRAMME_GROUP_ID,
                                        SPM.PROGRAMME_MODE,
                                        CONCAT(PR.PROGRAMME_NAME,
                                                ' (',
                                                S.DESCRIPTION,
                                                ' ',
                                                S.TIME,
                                                ')') AS PROGRAMME_NAME,
									     COUNT(AI.ISSUE_ID)AS IS_SUBMITTED
                                    FROM
                                        ADM_ISSUE_APPLICATION AS AI
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.ISSUE_ID = AI.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS ATP ON ATP.PRO_GROUPS_ID = AI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        ADM_APPLICATION_TYPE AS APP ON APP.APPLICATION_TYPE_ID = ATP.APPTYPE_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS PR ON PR.PROGRAMME_ID = ATP.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = ATP.PROGRAMME_MODE
                                            LEFT JOIN
                                        SUP_SHIFT AS S ON S.SHIFT_ID = ATP.SHIFT
                                    WHERE
                                        PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                                            AND AI.IS_PAID = 1
                                            AND (RECEIVE_ID IS NULL
                                            OR AR.IS_SUBMITTED IS NULL) GROUP BY AI.PROGRAMME_GROUP_ID;";
                        break;
                    }
                case AdmissionSQLCommands.GetSubmittedStudent:
                    {
                        query = @"SELECT 
                                        AI.PROGRAMME_GROUP_ID,
                                        SPM.PROGRAMME_MODE,
                                        CONCAT(PR.PROGRAMME_NAME,
                                                ' (',
                                                S.DESCRIPTION,
                                                ' ',
                                                S.TIME,
                                                ')') AS PROGRAMME_NAME,
                                        COUNT(AI.ISSUE_ID)AS IS_SUBMITTED
                                    FROM
                                        ADM_ISSUE_APPLICATION AS AI
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.ISSUE_ID = AI.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS ATP ON ATP.PRO_GROUPS_ID = AI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        ADM_APPLICATION_TYPE AS APP ON APP.APPLICATION_TYPE_ID = ATP.APPTYPE_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS PR ON PR.PROGRAMME_ID = ATP.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = ATP.PROGRAMME_MODE
                                            LEFT JOIN
                                        SUP_SHIFT AS S ON S.SHIFT_ID = ATP.SHIFT
                                    WHERE
                                        PROGRAMME_GROUP_ID  IN (?PROGRAMME_GROUP_ID)
                                            AND AI.IS_PAID = 1
                                            AND AR.IS_SUBMITTED=1
                                            AND AR.STATUS=?STATUS GROUP BY PROGRAMME_GROUP_ID;";
                        break;
                    }
                case AdmissionSQLCommands.GetSelectedStudent:
                    {
                        query = @"SELECT 
                                        AI.APPLICATION_NO,
                                        AI.PROGRAMME_GROUP_ID,
                                        CONCAT(IFNULL(AI.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(AI.LAST_NAME, '')) AS FIRST_NAME,
                                        AI.ISSUE_ID,
                                        AI.PROGRAMME_ID,
                                        AI.CONTACT_NO,
                                        DATE_FORMAT(AI.dob, '%d/%m/%Y') AS DOB,
                                        SPM.PROGRAMME_MODE,
                                        CONCAT(PR.PROGRAMME_NAME,
                                                ' (',
                                                S.DESCRIPTION,
                                                ' ',
                                                S.TIME,
                                                ')') AS PROGRAMME_NAME
                                    FROM
                                        ADM_ISSUE_APPLICATION AS AI
                                            LEFT JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.ISSUE_ID = AI.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS ATP ON ATP.PRO_GROUPS_ID = AI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        ADM_APPLICATION_TYPE AS APP ON APP.APPLICATION_TYPE_ID = ATP.APPTYPE_ID
                                            INNER JOIN
                                        CP_PROGRAMME_?AC_YEAR AS PR ON PR.PROGRAMME_ID = ATP.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = ATP.PROGRAMME_MODE
                                            LEFT JOIN
                                        SUP_SHIFT AS S ON S.SHIFT_ID = ATP.SHIFT
                                    WHERE
                                        PROGRAMME_GROUP_ID  IN (?PROGRAMME_GROUP_ID)
                                            AND AI.IS_PAID = 1
                                            AND AR.IS_SUBMITTED=1
                                            AND AR.STATUS=?STATUS GROUP BY PROGRAMME_GROUP_ID;";
                        break;
                    }
                case AdmissionSQLCommands.GetAdmittedStudent:
                    {
                        query = @"";
                        break;
                    }
                case AdmissionSQLCommands.FetchSubmittedStudentForTransfer:
                    {
                        query = @"SELECT 
                            AD.ISSUE_ID,
                            ADR.RECEIVE_ID,
                            AD.APPLICATION_TYPE_ID,
                            AD.PROGRAMME_ID,
                            CP.PROGRAMME_NAME,
                            DATE_FORMAT(AD.ISSUE_DATE, '%d/%m/%Y') AS ISSUE_DATE,
                            AD.APPLICATION_NO,
                            CONCAT(IFNULL(AD.FIRST_NAME, ''),
                                    '. ',
                                    IFNULL(AD.LAST_NAME, ''),
                                    ' (',
                                    IFNULL(AD.APPLICATION_NO, ''),
                                    ')') AS FIRST_NAME,
                            AD.CONTACT_NO,
                            AD.SHIFT,
                            DATE_FORMAT(AD.DOB, '%d/%m/%Y') AS DOB,
                            AD.PROGRAMME_GROUP_ID,
                            AD.CASTE,
                            ADR.COMMUNITY_ID,
                            ADR.CTALUK_CITY,
                            ADR.IS_SUBMITTED,
                            ADR.SMS_MOBILE_NO,
                            S.STATUS_NAME AS 'STATUS',
                            ADR.HS_MAX_MARK,
                            ADR.HSTOTAL,
                            ADR.TOTAL_CUT_OFF_MARK
                        FROM
                            ADM_RECEIVE_APPLICATION AS ADR
                                INNER JOIN
                            ADM_ISSUE_APPLICATION AS AD ON AD.ISSUE_ID = ADR.ISSUE_ID
                                AND AD.IS_DELETED != 1
                                INNER JOIN
                            ADM_APPTYPE_PROG_GROUPS AS AAP ON AAP.PRO_GROUPS_ID = AD.PROGRAMME_GROUP_ID
                                INNER JOIN
                            CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID = AAP.PROGRAMME_ID
                                LEFT JOIN
                            SUP_ADM_STATUS AS S ON S.STATUS_ID = ADR.STATUS
                        WHERE
                            AD.IS_PAID = 1 AND ADR.IS_SUBMITTED = 1
                                AND ADR.STATUS = ?STATUS
                                AND AD.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                                AND ADR.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeGroupByApplicationTypeAndProgrammeMode:
                    {
                        query = @"SELECT 
                        PRO_GROUPS_ID,
                        P.PROGRAMME_ID,
                        AP.SHIFT,
                        AP.PROGRAMME_MODE,
                        CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                ' (',
                                IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                ')') AS PROGRAMME_NAME
                    FROM
                       ADM_APPTYPE_PROG_GROUPS AS AP
                            INNER JOIN
                        CP_PROGRAMME_?AC_YEAR AS P ON P.PROGRAMME_ID = AP.PROGRAMME_ID
                            AND P.IS_DELETED != 1
                            INNER JOIN
                        SUP_SHIFT SS ON SS.SHIFT_ID = AP.SHIFT
                            AND SS.IS_DELETED != 1
                            INNER JOIN
                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                    WHERE
                        AP.IS_DELETED != 1 AND AP.SHIFT=?SHIFT AND AP.PROGRAMME_MODE=?PROGRAMME_MODE AND APPTYPE_ID=?APPTYPE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.GetOverAllStatusByProgrammeId:
                    {
                        query = @"SELECT 
                                AI.PROGRAMME_GROUP_ID,
                                PM.PROGRAMME_MODE,
                                CONCAT(CP.PROGRAMME_NAME,
                                        ' (',
                                        SS.DESCRIPTION,
                                        ' ',
                                        SS.TIME,
                                        ')') AS PROGRAMME_NAME,
                                COUNT(AI.RECEIVE_ID) AS IS_SUBMITTED
                            FROM
                                ADM_RECEIVE_APPLICATION AS AR
                                    INNER JOIN
                                adm_issued_applications AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    AND AR.IS_DELETED != 1
                                    AND AI.IS_PAID = 1
                                    INNER JOIN
                                cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                sup_shift AS SS ON SS.SHIFT_ID = PG.SHIFT
                            WHERE
                                AI.STATUS = ?STATUS
	                            AND AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                            GROUP BY AI.PROGRAMME_GROUP_ID;";
                        break;
                    }
                case AdmissionSQLCommands.GetSelectedProgrammeByHSCNO:
                    {
                        query = @"SELECT 
                                    SP.SELECTION_ID,
                                    SP.APPLICATION_NO,
                                    DATE_FORMAT(SP.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                    SP.TOTAL_CUT_OFF_MARK,
                                    SP.TOTAL_SECURED,
                                    SP.MAX_TOTAL,
                                    CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                            ' (',
                                            IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                            ')') AS PROGRAMME_NAME,
                                    SP.RECEIVE_ID,
                                    CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                            '. ',
                                            IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                    SP.IS_VERIFIED,
                                    AI.ISSUE_ID,
                                    AI.HSC_NO
                                FROM
                                    ADM_ISSUE_APPLICATION AS AI
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AR.ISSUE_ID = AI.ISSUE_ID
                                        INNER JOIN
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SP ON SP.RECEIVE_ID = AR.RECEIVE_ID
                                        INNER JOIN
                                    ADM_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = SP.APPLICATION_TYPE_ID
                                        INNER JOIN
                                    ADM_APPTYPE_PROG_GROUPS AS PG ON PG.PRO_GROUPS_ID = SP.PROGRAMME_ID
                                        INNER JOIN
                                    CP_PROGRAMME_?AC_YEAR AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_SHIFT SS ON SS.SHIFT_ID = PG.SHIFT
                                        AND SS.IS_DELETED != 1
                                        INNER JOIN
                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                WHERE
                                    AI.HSC_NO = ?HSC_NO
                                        AND AI.IS_DELETED != 1
                                        AND AR.IS_DELETED != 1
                                        AND SP.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.GetAppliedProgrammeByHSCNO:
                    {
                        query = @"SELECT 
                                AI.APPLICATION_NO,
                                CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                        ' (',
                                        IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                        ')') AS PROGRAMME_NAME,
                                AR.RECEIVE_ID,
                                AI.ISSUE_ID,
                                CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                        '. ',
                                        IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                AI.HSC_NO
                            FROM
                                ADM_ISSUE_APPLICATION AS AI
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.ISSUE_ID = AI.ISSUE_ID
                                    INNER JOIN
                                ADM_APPTYPE_PROG_GROUPS AS PG ON PG.PRO_GROUPS_ID = AI.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME_?AC_YEAR AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_SHIFT SS ON SS.SHIFT_ID = PG.SHIFT
                                    AND SS.IS_DELETED != 1
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                            WHERE
                                AI.HSC_NO =?HSC_NO
                                    AND AI.IS_DELETED != 1
                                    AND AR.IS_DELETED != 1
                                    AND AI.IS_PAID = 1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateProgrammeForTransferByIssueId:
                    {
                        query = @"UPDATE ADM_ISSUE_APPLICATION SET PROGRAMME_GROUP_ID=?PROGRAMME_GROUP_ID,PROGRAMME_ID=?PROGRAMME_ID WHERE ISSUE_ID=?ISSUE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateProgrammeForTransferByReceiveId:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION SET PROGGROUP_ID=?PROGGROUP_ID,PROGRAMME_ID=?PROGRAMME_ID WHERE  RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchDateWiseFeePaidAndVerifiedStatus:
                    {
                        query = @"SELECT 
                                        CONCAT(P.PROGRAMME_NAME,
                                                '(',
                                                
                                                PM.PROGRAMME_MODE,
                                                ')') AS PROGRAMME_NAME,
                                        NO_OF_SEATS,
                                        DATE_FORMAT(S.SELECTION_DATE, '%d/%m/%y') AS SELECTION_DATE,
                                        COUNT(SELECTION_ID) TOTAL_SELECTION,
                                        COUNT(IF(IS_VERIFIED = 1, 1, NULL)) VERIFIED,
                                        COUNT(IF(IS_FEE_PAID = 1, 1, NULL)) AS PAID,
                                        COUNT(IF(RE.IS_ROMAN_CATHOLIC = 1
                                                && S.IS_FEE_PAID = 1,
                                            1,
                                            NULL)) AS PAID_MINORITY,
                                        COUNT(IF(RE.IS_ROMAN_CATHOLIC = 1, 1, NULL)) AS MINORITY_SELECTED,
                                        COUNT(IF(RE.IS_ROMAN_CATHOLIC != 1
                                                && S.IS_FEE_PAID = 1,
                                            1,
                                            NULL)) AS PAID_OTHERS,
                                        COUNT(IF(RE.IS_ROMAN_CATHOLIC != 1, 1, NULL)) AS OTHERS_SELECTED
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS S
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS RE ON RE.RECEIVE_ID = S.RECEIVE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS D ON D.PRO_GROUPS_ID = S.PROGRAMME_ID
                                            INNER JOIN
                                        CP_PROGRAMME_2018 AS P ON P.PROGRAMME_ID = D.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = D.SHIFT
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = D.PROGRAMME_MODE
                                            LEFT JOIN
                                        ADM_MAXIMUM_IN_TAKES AS QD ON QD.PROGRAMME_ID = D.PRO_GROUPS_ID
                                    WHERE
                                        S.IS_DELETED != 1
                                    AND S.SELECTION_DATE BETWEEN ?DATE_FROM AND ?DATE_TO 
                                    GROUP BY D.PRO_GROUPS_ID , S.SELECTION_DATE;";
                        break;
                    }
                case AdmissionSQLCommands.FetchOverAllFeePaidAndVerifiedStatus:
                    {
                        query = @"SELECT 
                                        CONCAT(P.PROGRAMME_NAME,
                                                '(',
                                                PM.PROGRAMME_MODE,
                                                ')') AS PROGRAMME_NAME,
                                        NO_OF_SEATS,
                                        DATE_FORMAT(S.SELECTION_DATE, '%d/%m/%y') AS SELECTION_DATE,
                                        COUNT(SELECTION_ID) TOTAL_SELECTION,
                                        COUNT(IF(IS_VERIFIED = 1, 1, NULL)) VERIFIED,
                                        COUNT(IF(IS_FEE_PAID = 1, 1, NULL)) AS PAID,
                                        COUNT(IF(RE.IS_ROMAN_CATHOLIC = 1
                                                && S.IS_FEE_PAID = 1,
                                            1,
                                            NULL)) AS PAID_MINORITY,
                                        COUNT(IF(RE.IS_ROMAN_CATHOLIC = 1, 1, NULL)) AS MINORITY_SELECTED,
                                        COUNT(IF(RE.IS_ROMAN_CATHOLIC != 1
                                                && S.IS_FEE_PAID = 1,
                                            1,
                                            NULL)) AS PAID_OTHERS,
                                        COUNT(IF(RE.IS_ROMAN_CATHOLIC != 1, 1, NULL)) AS OTHERS_SELECTED
                                    FROM
                                        ADM_SELECTION_PROCESS_2018 AS S
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS RE ON RE.RECEIVE_ID = S.RECEIVE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS D ON D.PRO_GROUPS_ID = S.PROGRAMME_ID
                                            INNER JOIN
                                        CP_PROGRAMME_2018 AS P ON P.PROGRAMME_ID = D.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = D.SHIFT
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = D.PROGRAMME_MODE
                                            LEFT JOIN
                                        ADM_MAXIMUM_IN_TAKES AS QD ON QD.PROGRAMME_ID = D.PRO_GROUPS_ID
                                    WHERE
                                        S.IS_DELETED != 1 AND D.PRO_GROUPS_ID IN (?PROGRAMME_ID)
                                    GROUP BY D.PRO_GROUPS_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchSelectedStudentsForHostel:
                    {
                        query = @"SELECT 
                                SP.APPLICATION_NO,
                                SP.RECEIVE_ID,
                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                DATE_FORMAT(SP.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                SP.ISSUED_ID,
                                SP.PROGRAMME_GROUP_ID,
                                AR.SMS_MOBILE_NO,
                                SC.MAIN_COMMUNITY,
                                H.HOSTEL_ID,
                                H.HOSTEL_NAME,
                                ST.STATUS_NAME,
                                CONCAT(IFNULL(AR.CVILLAGE_AREA, ''),
                                        '-',
                                        IFNULL(AR.CTALUK_CITY, '')) AS CVILLAGE_AREA
                            FROM
                                ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS SP
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SP.RECEIVE_ID
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SP.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = SP.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = SP.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_APPLICANT_TYPE AS APT ON APT.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                    INNER JOIN
                                GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID=AR.CASTE_ID
                                    INNER JOIN 
                                SUP_MAIN_COMMUNITY AS SC ON SC.MAIN_COMMUNITY_ID = GC.MAIN_COMMUNITY_ID
                                    INNER JOIN
                                SUP_HOSTEL AS H ON H.HOSTEL_ID = SP.HOSTEL_ID
                                    INNER JOIN
                                SUP_ADM_STATUS AS ST ON ST.STATUS_ID = SP.STATUS
                            WHERE
                                SP.HOSTEL_ID IN (?HOSTEL_ID)
                                    AND SP.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeModeByReceiveId:
                    {
                        query = @"SELECT 
                                        AI.ISSUE_ID, AR.RECEIVE_ID, APP.PROGRAMME_MODE
                                    FROM
                                        ADM_RECEIVE_APPLICATION AS AR
                                            INNER JOIN
                                        ADM_ISSUE_APPLICATION AS AI ON AI.ISSUE_ID = AR.ISSUE_ID
                                            INNER JOIN
                                        ADM_APPTYPE_PROG_GROUPS AS APP ON APP.PRO_GROUPS_ID = AI.PROGRAMME_GROUP_ID
                                    WHERE
                                        AR.RECEIVE_ID = ?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchSelectedStudentsForHostelByProgramme:
                    {
                        query = @"SELECT 
                                        SP.SELECTION_ID,
                                        SP.APPLICATION_NO,
                                        SP.TOTAL_CUT_OFF_MARK,
                                        SP.TOTAL_SECURED,
                                        SP.MAX_TOTAL,
                                        SP.RECEIVE_ID,
                                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                        DATE_FORMAT(AHR.SELECTION_DATE, '%d/%m/%y') AS SELECTION_DATE,
                                        AR.ISSUE_ID,
                                        SP.PROGRAMME_ID,
                                        AR.SMS_MOBILE_NO,
                                        SPI.STAFF_ID,
                                        CONCAT(IFNULL(SPI.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(SPI.LAST_NAME, '')) AS SELECTED_BY,
                                        SC.COMMUNITY,
                                        AHR.HOSTEL_ID,
                                        CONCAT(IFNULL(AR.CVILLAGE_AREA, ''),
                                        '-',
                                        IFNULL(AR.CTALUK_CITY, '')) AS CVILLAGE_AREA,
                                        S.STATUS_NAME AS 'STATUS'
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                            INNER JOIN
                                        SUP_APPLICANT_TYPE AS APT ON APT.APPLICANT_TYPE_ID= SP.APPLICATION_TYPE_ID
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SP.RECEIVE_ID
											INNER JOIN
										ADM_ISSUE_APPLICATION AS AI ON AI.ISSUE_ID=AR.ISSUE_ID
											INNER JOIN
										ADM_HOSTEL_REGISTRATION AS AHR ON AHR.RECEIVE_ID=AR.RECEIVE_ID	
											INNER JOIN
										STF_PERSONAL_INFO AS SPI ON SPI.STAFF_ID=AHR.SELECTED_BY
											LEFT JOIN
										SUP_COMMUNITY AS SC ON SC.COMMUNITYID=AR.CASTE_ID
											LEFT JOIN
										SUP_ADM_STATUS AS S ON S.STATUS_ID=AHR.STATUS
                                    WHERE
                                         AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID) AND AHR.IS_SUBMITTED=1 AND AHR.STATUS=3 AND AHR.ACADEMIC_YEAR=?AC_YEAR AND SP.IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmittedStudentsForHostelByProgramme:
                    {
                        query = @"SELECT 
                                        SP.SELECTION_ID,
                                        SP.APPLICATION_NO,
                                        SP.TOTAL_CUT_OFF_MARK,
                                        SP.TOTAL_SECURED,
                                        SP.MAX_TOTAL,
                                        SP.RECEIVE_ID,
                                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                        DATE_FORMAT(AHR.SELECTION_DATE, '%d/%m/%y') AS SELECTION_DATE,
                                        AR.ISSUE_ID,
                                        SP.PROGRAMME_ID,
                                        AR.SMS_MOBILE_NO,
                                        SPI.STAFF_ID,
                                        CONCAT(IFNULL(SPI.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(SPI.LAST_NAME, '')) AS SELECTED_BY,
                                        SC.COMMUNITY,
                                        AHR.HOSTEL_ID,
                                        CONCAT(IFNULL(AR.CVILLAGE_AREA, ''),
                                        '-',
                                        IFNULL(AR.CTALUK_CITY, '')) AS CVILLAGE_AREA,
                                        S.STATUS_NAME AS 'STATUS'
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                            INNER JOIN
                                        SUP_APPLICANT_TYPE AS APT ON APT.APPLICANT_TYPE_ID= SP.APPLICATION_TYPE_ID
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SP.RECEIVE_ID
											INNER JOIN
										ADM_ISSUE_APPLICATION AS AI ON AI.ISSUE_ID=AR.ISSUE_ID
											INNER JOIN
										ADM_HOSTEL_REGISTRATION AS AHR ON AHR.RECEIVE_ID=AR.RECEIVE_ID	
											INNER JOIN
										FEE_TRANSACTION AS FS ON FS.STUDENT_ID=AR.RECEIVE_ID AND FS.FREQUENCY=?FREQUENCY_ID
											INNER JOIN
										STF_PERSONAL_INFO AS SPI ON SPI.STAFF_ID=AHR.SELECTED_BY
											LEFT JOIN
										SUP_COMMUNITY AS SC ON SC.COMMUNITYID=AR.CASTE_ID
                                            INNER JOIN
										SUP_ADM_STATUS AS S ON S.STATUS_ID=AHR.STATUS
                                    WHERE
                                         AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID) AND AHR.IS_SUBMITTED=1 AND AHR.STATUS=5 AND AHR.ACADEMIC_YEAR=?AC_YEAR AND SP.IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmittedStudentListReport:
                    {
                        query = @"SELECT 
                                        SP.SELECTION_ID,
                                        SP.APPLICATION_NO,
                                        DATE_FORMAT(SP.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                        SP.TOTAL_CUT_OFF_MARK,
                                        SP.TOTAL_SECURED,
                                        SP.MAX_TOTAL,
                                        SP.RECEIVE_ID,
                                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                        SP.IS_VERIFIED,
                                        AR.ISSUE_ID,
                                        AT.REQUEST_ID,
                                        AT.APPROVE_ID,
                                        SP.PROGRAMME_ID,
                                        SSC.SELECTION_CYCLE,
                                        AR.SMS_MOBILE_NO,
                                        SC.COMMUNITY,
                                        S.STATUS_NAME AS 'STATUS'
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                            LEFT JOIN
                                        ADM_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = SP.APPLICATION_TYPE_ID
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SP.RECEIVE_ID
                                            INNER JOIN
										FEE_TRANSACTION AS FS ON FS.STUDENT_ID=AR.RECEIVE_ID AND FS.FREQUENCY=?FREQUENCY_ID
											LEFT JOIN
                                        ADM_TRANSFER AS AT ON AT.RECEIVE_ID=AR.RECEIVE_ID AND AT.ACADEMIC_YEAR=?AC_YEAR
											LEFT JOIN
										STF_PERSONAL_INFO AS SF ON SF.STAFF_ID=AT.REQUEST_ID
                                            LEFT JOIN
										SUP_SELECTION_CYCLE AS SSC ON SSC.SELECTION_CYCLE_ID=SP.SELECTION_CYCLE
											LEFT JOIN SUP_COMMUNITY AS SC ON SC.COMMUNITYID=AR.CASTE_ID
                                            LEFT JOIN
										SUP_ADM_STATUS AS S ON S.STATUS_ID=AR.STATUS
                                    WHERE
                                        SP.PROGRAMME_ID IN (?PROGRAMME_ID) AND SP.IS_DELETED!=1 AND SP.IS_FEE_PAID=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchCanceledAdmissionList:
                    {
                        query = @"SELECT 
                                        AC.CANCEL_ADMISSION_ID,
                                        AC.APPLICATION_TYPE_ID,
                                        AC.STUDENT_ID,
                                        CONCAT(IFNULL(AC.FIRST_NAME,''),' ',IFNULL(AC.LAST_NAME,'')) AS 'NAME',
                                        AC.APPLICATION_NO,
                                        G.GENDER_NAME,
                                        P.PAYMENT_MODE,
                                        APP.PRO_GROUPS_ID,
                                        CP.PROGRAMME_NAME,
                                        DATE_FORMAT(AC.ADMISSION_DATE,'%d/%m/%Y')AS 'ADMISSION_DATE',
                                        AC.REASON,
                                        AC.ACADEMIC_YEAR
                                    FROM
                                        ADM_CANCEL_ADMISSION_?AC_YEAR AS AC
		                                    INNER JOIN 
	                                    ADM_APPTYPE_PROG_GROUPS AS APP ON APP.PRO_GROUPS_ID=AC.PROGRAMME_ID AND APP.IS_DELETED!=1
		                                    INNER JOIN
	                                    CP_PROGRAMME_?AC_YEAR AS CP ON CP.PROGRAMME_ID=APP.PROGRAMME_ID AND CP.IS_DELETED!=1
		                                    INNER JOIN
	                                    ADM_RECEIVE_APPLICATION AS R  ON R.RECEIVE_ID=AC.STUDENT_ID AND R.IS_DELETED!=1
		                                    INNER JOIN
	                                    SUP_GENDER AS G ON G.GENDER_ID=AC.GENDER_ID 
		                                    INNER JOIN
	                                    SUP_PAYMENT_MODE AS P ON P.PAYMENT_MODE_ID=AC.MODE_ID AND P.IS_DELETED!=1
                                    WHERE AC.ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.FetchBankApplicationByIssueId:
                    {
                        query = @"SELECT 
                                    I.FIRST_NAME,
                                    I.LAST_NAME,
                                    I.CONTACT_NO AS 'MOBILE',
                                    I.EMAIL_ID,
                                    DATE_FORMAT(I.DOB,'%d/%m/%Y') AS 'DOB',
                                    'TAMIL NADU' AS 'STATE',
                                    I.CTALUK_CITY AS 'CITY',
                                    R.FATHER_NAME,
                                    R.GENDER,
                                    I.ISSUE_ID,
                                    R.RECEIVE_ID
                                FROM
                                  ADM_ISSUE_APPLICATION AS I
	                                INNER JOIN
                                  ADM_RECEIVE_APPLICATION AS R ON R.ISSUE_ID=I.ISSUE_ID
                                WHERE 
	                                I.IS_DELETED!=1 AND R.IS_DELETED!=1 AND I.ISSUE_ID=?ISSUE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchBankApplicationByStudentId:
                    {
                        query = @"SELECT BANK_APPLICATION_ID,
                                   COLLEGE_NAME,
                                   STUDENT_ID,
                                   DESIGNATION,
                                   SALUTATION,
                                   FIRST_NAME,
                                   LAST_NAME,
                                   GENDER_ID,
                                   MARITUAL_STATUS,
                                   FATHER_NAME,
                                   MOTHER_NAME,
                                   SPOUSE_NAME,
                                   DATE_FORMAT(DOB,'%d/%m/%Y') AS 'DOB',
                                   PERMANENT_ADDRESS1,
                                   PERMANENT_ADDRESS2,
                                   PERMANENT_ADDRESS3,
                                   PERMANENT_CITY,
                                   PERMANENT_STATE,
                                   PERMANENT_PINCODE,
                                   COMMUNICATION_ADDRESS1,
                                   COMMUNICATION_ADDRESS2,
                                   COMMUNICATION_ADDRESS3,
                                   COMMUNICATION_STATE,
                                   COMMUNICATION_CITY,
                                   COMMUNICATION_PINCODE,
                                   MOBILE_NUMBER,
                                   EMAIL,
                                   ID_TYPE,
                                   NUMBER,
                                   STUDENT_ID_CARD_NUMBER,
                                   NOMINEE_NAME,
                                   NOMINEE_AGE,
                                   NOMINEE_RELATION,
                                   NOMINEE_ADDRESS1,
                                   NOMINEE_ADDRESS2,
                                   NOMINEE_STATE,
                                   NOMINEE_CITY,
                                   NOMINNE_PINCODE,
                                   NOMINEE_PHONE,
                                   MEDIUM
                                FROM ADM_BANK_APPLICATION WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchBankApplicationByBankApplicationId:
                    {
                        query = @"SELECT BANK_APPLICATION_ID,
                                   COLLEGE_NAME,
                                   STUDENT_ID,
                                   DESIGNATION,
                                   SALUTATION,
                                   FIRST_NAME,
                                   LAST_NAME,
                                   GENDER_ID,
                                   MARITUAL_STATUS,
                                   FATHER_NAME,
                                   MOTHER_NAME,
                                   SPOUSE_NAME,
                                   DATE_FORMAT(DOB,'%d/%m/%Y') AS 'DOB',
                                   PERMANENT_ADDRESS1,
                                   PERMANENT_ADDRESS2,
                                   PERMANENT_ADDRESS3,
                                   PERMANENT_CITY,
                                   PERMANENT_STATE,
                                   PERMANENT_PINCODE,
                                   COMMUNICATION_ADDRESS1,
                                   COMMUNICATION_ADDRESS2,
                                   COMMUNICATION_ADDRESS3,
                                   COMMUNICATION_STATE,
                                   COMMUNICATION_CITY,
                                   COMMUNICATION_PINCODE,
                                   MOBILE_NUMBER,
                                   EMAIL,
                                   ID_TYPE,
                                   NUMBER,
                                   STUDENT_ID_CARD_NUMBER,
                                   NOMINEE_NAME,
                                   NOMINEE_AGE,
                                   NOMINEE_RELATION,
                                   NOMINEE_ADDRESS1,
                                   NOMINEE_ADDRESS2,
                                   NOMINEE_STATE,
                                   NOMINEE_CITY,
                                   NOMINNE_PINCODE,
                                   NOMINEE_PHONE,
                                   MEDIUM
                                FROM ADM_BANK_APPLICATION WHERE BANK_APPLICATION_ID=?BANK_APPLICATION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertBankApplication:
                    {
                        query = @"INSERT INTO ADM_BANK_APPLICATION(
                                    COLLEGE_NAME,
                                    STUDENT_ID,
                                    DESIGNATION,
                                    SALUTATION,
                                    FIRST_NAME,
                                    LAST_NAME,
                                    GENDER_ID,
                                    MARITUAL_STATUS,
                                    FATHER_NAME,
                                    MOTHER_NAME,
                                    SPOUSE_NAME,
                                    DOB,
                                    PERMANENT_ADDRESS1,
                                    PERMANENT_ADDRESS2,
                                    PERMANENT_ADDRESS3,
                                    PERMANENT_CITY,
                                    PERMANENT_STATE,
                                    PERMANENT_PINCODE,
                                    COMMUNICATION_ADDRESS1,
                                    COMMUNICATION_ADDRESS2,
                                    COMMUNICATION_ADDRESS3,
                                    COMMUNICATION_STATE,
                                    COMMUNICATION_CITY,
                                    COMMUNICATION_PINCODE,
                                    MOBILE_NUMBER,
                                    EMAIL,
                                    ID_TYPE,
                                    NUMBER,
                                    STUDENT_ID_CARD_NUMBER,
                                    NOMINEE_NAME,
                                    NOMINEE_AGE,
                                    NOMINEE_RELATION,
                                    NOMINEE_ADDRESS1,
                                    NOMINEE_ADDRESS2,
                                    NOMINEE_STATE,
                                    NOMINEE_CITY,
                                    NOMINNE_PINCODE,
                                    NOMINEE_PHONE,
                                    MEDIUM)
                                    VALUES(
                                    ?COLLEGE_NAME,
                                    ?STUDENT_ID,
                                    ?DESIGNATION,
                                    ?SALUTATION,
                                    ?FIRST_NAME,
                                    ?LAST_NAME,
                                    ?GENDER_ID,
                                    ?MARITUAL_STATUS,
                                    ?FATHER_NAME,
                                    ?MOTHER_NAME,
                                    ?SPOUSE_NAME,
                                    ?DOB,
                                    ?PERMANENT_ADDRESS1,
                                    ?PERMANENT_ADDRESS2,
                                    ?PERMANENT_ADDRESS3,
                                    ?PERMANENT_CITY,
                                    ?PERMANENT_STATE,
                                    ?PERMANENT_PINCODE,
                                    ?COMMUNICATION_ADDRESS1,
                                    ?COMMUNICATION_ADDRESS2,
                                    ?COMMUNICATION_ADDRESS3,
                                    ?COMMUNICATION_STATE,
                                    ?COMMUNICATION_CITY,
                                    ?COMMUNICATION_PINCODE,
                                    ?MOBILE_NUMBER,
                                    ?EMAIL,
                                    ?ID_TYPE,
                                    ?NUMBER,
                                    ?STUDENT_ID_CARD_NUMBER,
                                    ?NOMINEE_NAME,
                                    ?NOMINEE_AGE,
                                    ?NOMINEE_RELATION,
                                    ?NOMINEE_ADDRESS1,
                                    ?NOMINEE_ADDRESS2,
                                    ?NOMINEE_STATE,
                                    ?NOMINEE_CITY,
                                    ?NOMINNE_PINCODE,
                                    ?NOMINEE_PHONE,
                                    ?MEDIUM);";
                        break;
                    }
                case AdmissionSQLCommands.UpdatebankApplication:
                    {
                        query = @"UPDATE ADM_BANK_APPLICATION 
                                  SET
                                    COLLEGE_NAME = ?COLLEGE_NAME,
                                    STUDENT_ID = ?STUDENT_ID,
                                    DESIGNATION = ?DESIGNATION,
                                    SALUTATION = ?SALUTATION,
                                    FIRST_NAME = ?FIRST_NAME,
                                    LAST_NAME =?LAST_NAME,
                                    GENDER_ID = ?GENDER_ID,
                                    MARITUAL_STATUS = ?MARITUAL_STATUS,
                                    FATHER_NAME = ?FATHER_NAME,
                                    MOTHER_NAME = ?MOTHER_NAME,
                                    SPOUSE_NAME = ?SPOUSE_NAME,
                                    DOB = ?DOB,
                                    PERMANENT_ADDRESS1 = ?PERMANENT_ADDRESS1,
                                    PERMANENT_ADDRESS2 = ?PERMANENT_ADDRESS2,
                                    PERMANENT_ADDRESS3 = ?PERMANENT_ADDRESS3,
                                    PERMANENT_CITY = ?PERMANENT_CITY,
                                    PERMANENT_STATE = ?PERMANENT_STATE,
                                    PERMANENT_PINCODE = ?PERMANENT_PINCODE,
                                    COMMUNICATION_ADDRESS1 = ?COMMUNICATION_ADDRESS1,
                                    COMMUNICATION_ADDRESS2 = ?COMMUNICATION_ADDRESS2,
                                    COMMUNICATION_ADDRESS3 = ?COMMUNICATION_ADDRESS3,
                                    COMMUNICATION_STATE =?COMMUNICATION_STATE,
                                    COMMUNICATION_CITY = ?COMMUNICATION_CITY,
                                    COMMUNICATION_PINCODE = ?COMMUNICATION_PINCODE,
                                    MOBILE_NUMBER = ?MOBILE_NUMBER,
                                    EMAIL = ?EMAIL,
                                    ID_TYPE = ?ID_TYPE,
                                    NUMBER = ?NUMBER,
                                    STUDENT_ID_CARD_NUMBER =?STUDENT_ID_CARD_NUMBER,
                                    NOMINEE_NAME = ?NOMINEE_NAME,
                                    NOMINEE_AGE = ?NOMINEE_AGE,
                                    NOMINEE_RELATION =?NOMINEE_RELATION,
                                    NOMINEE_ADDRESS1 = ?NOMINEE_ADDRESS1,
                                    NOMINEE_ADDRESS2 =?NOMINEE_ADDRESS2,
                                    NOMINEE_STATE = ?NOMINEE_STATE,
                                    NOMINEE_CITY = ?NOMINEE_CITY,
                                    NOMINNE_PINCODE = ?NOMINNE_PINCODE,
                                    NOMINEE_PHONE = ?NOMINEE_PHONE,
                                    MEDIUM = ?MEDIUM
                                    WHERE BANK_APPLICATION_ID = ?BANK_APPLICATION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchCollegeInfo:
                    {
                        query = @"SELECT 
                                    COLLEGE_ID,
                                    MANAGEMENT_NAME,
                                    COLLEGE_NAME,
                                    COLLEGE_TYPE,
                                    ADDRESS_ONE,
                                    ADDRESS_TWO,
                                    PLACE,
                                    CITY,
                                    DISTRICT,
                                    PINCODE,
                                    PHONE,
                                    EMAIL,
                                    COLLEGE_CODE,
                                    CUSTOMER_CODE,
                                    DB_NAME,
                                    USERNAME,
                                    LICENSE_NO,
                                    PASSWORD,
                                    APPLICATION_URL,
                                    DB_SERVER,
                                    PORTAL_URL,
                                    WEBSITE_URL,
                                    COLLEGE_EMAIL,
                                    COLLEGE_EMAIL_PASSWORD,
                                    COLLEGE_LOGO,
                                    UNIVERSITY,
                                    GRADE
                                FROM
                                    COLLEGE_DETAILS;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeByProgrammeGroup:
                    {
                        query = @"SELECT 
                                PG.PROGRAMME_GROUP_ID,
                                P.PROGRAMME_ID, 
                                PG.IS_NEW,                                
                                PG.SHIFT,
                                PG.PROGRAMME_MODE,
                                CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                        ' (',
                                        IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                        ')') AS PROGRAMME_NAME
                            FROM
                                CP_PROGRAMME_GROUP AS PG
                                    INNER JOIN
                                CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                    AND P.IS_DELETED != 1
                                    INNER JOIN
                                SUP_SHIFT SS ON SS.SHIFT_ID = PG.SHIFT
                                    AND SS.IS_DELETED != 1
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                            WHERE
                                PG.IS_DELETED != 1";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeInProgrammeGroupByProgramme:
                    {
                        query = @"SELECT 
                                PG.PROGRAMME_GROUP_ID,
                                P.PROGRAMME_ID, 
                                PG.IS_NEW,                                
                                PG.SHIFT,
                                PG.PROGRAMME_MODE,
                                CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                        ' (',
                                        IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                        ')') AS PROGRAMME_NAME
                            FROM
                                CP_PROGRAMME_GROUP AS PG
                                    INNER JOIN
                                CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                    AND P.IS_DELETED != 1 
                                    INNER JOIN
                                SUP_SHIFT SS ON SS.SHIFT_ID = PG.SHIFT
                                    AND SS.IS_DELETED != 1
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                            WHERE
                                PG.IS_DELETED != 1 AND PG.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeDescriptionByDescriptionId:
                    {
                        query = @"SELECT 
                                PROGRAMME_DESCRIPTION_ID,
                                PROGRAMME_GROUP_ID,
                                PROGRAMME_DESCRIPTION,
                                COURSE_OBJECTIVE,
                                PROGRAMME_OUTCOME,
                                PROGRAMME_CONTENT,
                                PROGRAMME_ELIGIBLITY,
                                SYLLABUS_PATH
                            FROM
                                ADM_PROGRAMME_DESCRIPTION
                            WHERE
                                PROGRAMME_DESCRIPTION_ID = ?PROGRAMME_DESCRIPTION_ID AND IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.DeleteProgrammeDescription:
                    {
                        query = @"UPDATE ADM_PROGRAMME_DESCRIPTION SET IS_DELETED=1 WHERE PROGRAMME_DESCRIPTION_ID=?PROGRAMME_DESCRIPTION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertProgrammeDescription:
                    {
                        query = @"INSERT INTO adm_programme_description (`PROGRAMME_GROUP_ID`,`PROGRAMME_DESCRIPTION`,`COURSE_OBJECTIVE`,`PROGRAMME_OUTCOME`,`PROGRAMME_CONTENT`,`PROGRAMME_ELIGIBLITY`,`SYLLABUS_PATH`,`ACADEMIC_YEAR`)VALUES
                                (?PROGRAMME_GROUP_ID,?PROGRAMME_DESCRIPTION,?COURSE_OBJECTIVE,?PROGRAMME_OUTCOME,?PROGRAMME_CONTENT,?PROGRAMME_ELIGIBLITY,?SYLLABUS_PATH,?ACADEMIC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.UpdateProgrammeDescription:
                    {
                        query = @"UPDATE `ADM_PROGRAMME_DESCRIPTION`
                                SET
                                `PROGRAMME_DESCRIPTION` =?PROGRAMME_DESCRIPTION,
                                `COURSE_OBJECTIVE` =?COURSE_OBJECTIVE,
                                `PROGRAMME_OUTCOME` =?PROGRAMME_OUTCOME,
                                `PROGRAMME_CONTENT` =?PROGRAMME_CONTENT,
                                `PROGRAMME_ELIGIBLITY` =?PROGRAMME_ELIGIBLITY,
                                `SYLLABUS_PATH` =?SYLLABUS_PATH
                                WHERE `PROGRAMME_DESCRIPTION_ID` =?PROGRAMME_DESCRIPTION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchApplictionType:
                    {
                        query = @"SELECT 
                                APPLICATION_TYPE_ID, APPLICATION_TYPE, PREFIX, SUFFIX
                            FROM
                                SUP_APPLICATION_TYPE
                            WHERE
                                IS_ACTIVE != 0 AND IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeForCourses:
                    {
                        query = @"SELECT 
                                        PG.PROGRAMME_GROUP_ID,
                                        CONCAT(P.PROGRAMME_NAME,
                                                ' - ',
                                                PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                        APT.APPLICATION_TYPE,
                                        SH.DESCRIPTION AS SHIFT_NAME,
                                        APT.APPLICATION_TYPE_ID,
                                        PG.SHIFT,
                                        PG.PROGRAMME_MODE,
                                        SH.TIME,
                                        PG.IS_NEW
                                    FROM
                                        CP_PROGRAMME_GROUP AS PG
                                            INNER JOIN
                                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_APPLICATION_TYPE AS APT ON APT.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = PG.SHIFT
                                    WHERE
                                        PG.IS_DELETED != 1 AND APT.IS_ACTIVE = 1 AND APT.IS_DELETED!=1 
                                        ORDER BY PG.PROGRAMME_ORDER;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeDescriptionForList:
                    {
                        query = @"SELECT 
                                PD.PROGRAMME_DESCRIPTION_ID,
                                PD.PROGRAMME_GROUP_ID,
                                CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                        ' (',
                                        IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                        ')') AS PROGRAMME_NAME,
                                        SS.DESCRIPTION AS SHIFT_NAME
                            FROM
                                ADM_PROGRAMME_DESCRIPTION AS PD
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = PD.PROGRAMME_GROUP_ID                                   
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_APPLICATION_TYPE AS AP ON AP.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE WHERE PD.IS_DELETED!=1 ";
                        break;
                    }
                case AdmissionSQLCommands.CheckProgrammeDescriptionExistByProgrammeGroupId:
                    {
                        query = @"SELECT 
                                PROGRAMME_DESCRIPTION_ID,
                                PROGRAMME_GROUP_ID,
                                PROGRAMME_DESCRIPTION,
                                COURSE_OBJECTIVE,
                                PROGRAMME_OUTCOME,
                                PROGRAMME_CONTENT,
                                PROGRAMME_ELIGIBLITY,
                                SYLLABUS_PATH
                            FROM
                                ADM_PROGRAMME_DESCRIPTION
                            WHERE
                                PROGRAMME_GROUP_ID = ?PROGRAMME_GROUP_ID AND IS_DELETED!=1 AND ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeDescriptionByProgrammeGroupId:
                    {
                        query = @"SELECT 
                                PD.PROGRAMME_DESCRIPTION_ID,
                                PD.PROGRAMME_GROUP_ID,
                                PD.PROGRAMME_DESCRIPTION,
                                PD.COURSE_OBJECTIVE,
                                PD.PROGRAMME_OUTCOME,
                                PD.PROGRAMME_CONTENT,
                                PD.PROGRAMME_ELIGIBLITY,
                                PD.SYLLABUS_PATH,
                                CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                        ' (',
                                        IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                        ')') AS PROGRAMME_NAME,
                                PG.SHIFT,
                                SS.DESCRIPTION AS SHIFT_NAME,
                                AP.APPLICATION_TYPE_ID,
                                AP.APPLICATION_TYPE
                            FROM
                                ADM_PROGRAMME_DESCRIPTION AS PD
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = PD.PROGRAMME_GROUP_ID
                                  
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_APPLICATION_TYPE AS AP ON AP.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                            WHERE
                                PD.IS_DELETED != 1 AND PD.PROGRAMME_GROUP_ID=?PROGRAMME_GROUP_ID     
                                   ;";
                        break;
                    }
                case AdmissionSQLCommands.FetchDepartmentWiseIssuedApplication:
                    {
                        query = @"SELECT 
                            CONCAT(AIP.APPLICATION_NO,LPAD(AIP.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                            UPPER(ARP.FIRST_NAME) AS FIRST_NAME,
                            concat(CP.PROGRAMME_NAME,'(',PM.PROGRAMME_MODE,' )') AS PROGRAMME_NAME,
                            SS.STATUS_NAME AS STATUS,
                            DEP.DEPARTMENT_ID,
                            DEP.DEPARTMENT,
                            ARP.SMS_MOBILE_NO
                        FROM
                            ADM_ISSUED_APPLICATIONS AS AIP
                                INNER JOIN
                            ADM_RECEIVE_APPLICATION AS ARP ON ARP.RECEIVE_ID = AIP.RECEIVE_ID
                                INNER JOIN
                            CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AIP.PROGRAMME_GROUP_ID
                                INNER JOIN
                            CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                INNER JOIN
                            sup_shift AS S ON S.SHIFT_ID = CPG.SHIFT
                                INNER JOIN
                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                                INNER JOIN
                            CP_DEPARTMENT AS DEP ON DEP.DEPARTMENT_ID = CP.DEPARTMENT
                                INNER JOIN
                            SUP_ADM_STATUS AS SS ON SS.STATUS_ID = AIP.STATUS
                        WHERE
                            AIP.IS_PAID = 1
                                AND AIP.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.FetchSelectedApplicantByProgrammeandApplicatType:
                    {
                        query = @"SELECT 
                                    AR.RECEIVE_ID,
                                    CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    AR.APPLICATION_TYPE_ID,
                                    AR.CMOBILENO AS MOBILE_NO,
                                    CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                            '. ',
                                            IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                    CONCAT(IFNULL(AR.CTALUK_CITY, ''),
                                            '. ',
                                            IFNULL(AR.CVILLAGE_AREA, '')) AS CVILLAGE_AREA,
                                    AR.IS_SINGLE_GIRL_CHILD,
                                    AR.TOTAL_CUT_OFF_MARK,
                                    AR.HSTOTAL,
                                    AR.HSPERCENTAGE,
                                    AR.HS_MAX_MARK,
                                    AR.ISSUE_ID,
                                    AR.ACADEMIC_YEAR,
                                    AR.HSC_NO,
                                    SC.COMMUNITYID AS COMMUNITY_ID,
                                    SC.COMMUNITY,
                                    AR.LAST_STUDIED_CLASS,
                                    AR.LAST_STUDIED_SCHOOL,
                                    SU.UNIVERSITY,
                                    OC.OCCUPATION_NAME AS OCCUPATION,
                                    AR.ANNUAL_INCOME,
                                    AR.EXSERVICE_MAN,
                                    AR.SPECIALLYABLED_ID,
                                    AR.IS_FIRSTGENERATION
                                FROM
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS ASP
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = ASP.RECEIVE_ID
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                        AND AI.PROGRAMME_GROUP_ID = ASP.PROGRAMME_ID
                                        LEFT JOIN
                                    SUP_COMMUNITY AS SC ON SC.COMMUNITYID = AR.CASTE_ID
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                        LEFT JOIN
                                    SUP_UNIVERSITY AS SU ON SU.UNIVERSITY_ID = AR.UNIVERSITY
                                        LEFT JOIN
                                    SUP_OCCUPATION AS OC ON OC.OCCUPATION_ID = AR.OCCUPATION
                                WHERE
		                                ASP.PROGRAMME_ID IN (?PROGRAMME_ID)
                                        AND ASP.SELECTION_TYPE=?APPLICATION_TYPE_ID
                                        AND ASP.SELECTION_CYCLE=?SELECTION_CYCLE
                                        AND ASP.SELECTED_BY=?SELECTED_BY
                                        AND (ASP.IS_DELETED!=1 AND ASP.IS_CANCELED!=1)
                                        AND AR.ACADEMIC_YEAR =?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.UdateDeleteByRecieveId:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR
                                SET 
                                    IS_DELETED = 1
                                WHERE
                                    RECEIVE_ID =?RECEIVE_ID AND PROGRAMME_ID =?PROGRAMME_ID AND (IS_DELETED!=1 AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchCasteWiseAdmitted:
                    {
                        query = @"SELECT 
                                    ISS.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID',
                                    GC.MAIN_COMMUNITY_ID AS 'CASTE_ID'
                                FROM
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                                        AND REC.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                                        AND ISS.IS_DELETED != 1 AND ISS.STATUS>=5
                                        INNER JOIN
                                    GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                                WHERE
	                                    SE.PROGRAMME_ID=?PROGRAMME_ID
                                        AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                                        AND SE.IS_DELETED != 1";
                        break;
                    }
                case AdmissionSQLCommands.FetchMinorityWise:
                    {
                        query = @"SELECT 
                                    ISS.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID',
                                    GC.MAIN_COMMUNITY_ID AS 'CASTE_ID'
                                FROM
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                                        AND REC.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                                        AND ISS.IS_DELETED != 1
                                        AND ISS.STATUS >= 3
                                        INNER JOIN
                                    GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                                WHERE
                                        SE.PROGRAMME_ID =?PROGRAMME_ID
                                        AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                                        AND REC.IS_ROMAN_CATHOLIC= 1
                                        AND (SE.IS_DELETED != 1 AND SE.IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchMinorityWiseAdmitted:
                    {
                        query = @"SELECT 
                                    ISS.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID',
                                    GC.MAIN_COMMUNITY_ID AS 'CASTE_ID'
                                FROM
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                                        AND REC.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                                        AND ISS.IS_DELETED != 1
                                        AND ISS.STATUS >= 5
                                        INNER JOIN
                                    GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                                WHERE
                                        SE.PROGRAMME_ID =?PROGRAMME_ID
                                        AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                                        AND REC.IS_ROMAN_CATHOLIC= 1 
                                        AND (SE.IS_DELETED != 1 AND SE.IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchApplicantByProgrammeId:
                    {
                        query = @"SELECT 
                                ADR.RECEIVE_ID,
                                CONCAT(ADR.FIRST_NAME,
                                        '(',
                                        ADI.APPLICATION_NO
                                        ,LPAD(ADI.ISSUE_NO,4,'0'),
                                        ')') AS APPLICATION_NO
                            FROM
                                ADM_RECEIVE_APPLICATION AS ADR
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS ADI ON ADR.RECEIVE_ID = ADI.RECEIVE_ID
                                    AND IS_PAID = 1
                            WHERE
                                     ADI.PROGRAMME_GROUP_ID =?PROGRAMME_ID
                                    AND ADI.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchPorgrammedetails:
                    {
                        query = @"SELECT 
                                PROGRAMME_GROUP_ID, APPTYPE_ID, SHIFT, PROGRAMME_MODE
                            FROM
                                CP_PROGRAMME_GROUP WHERE PROGRAMME_GROUP_ID=?PROGRAMME_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchToProgramme:
                    {
                        query = @" SELECT 
                        AP.PROGRAMME_GROUP_ID,
                        P.PROGRAMME_ID,
                        AP.SHIFT,
                        AP.PROGRAMME_MODE,
                        CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                ' (',
                                IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                ')') AS PROGRAMME_NAME
                               
                    FROM
                       CP_PROGRAMME_GROUP AS AP
                            INNER JOIN
                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = AP.PROGRAMME_ID
                            AND P.IS_DELETED != 1
                            INNER JOIN
                        SUP_SHIFT SS ON SS.SHIFT_ID = AP.SHIFT
                            AND SS.IS_DELETED != 1
                            INNER JOIN
                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                    WHERE
						AP.APPTYPE_ID=?APPTYPE_ID AND AP.SHIFT=?SHIFT AND AP.PROGRAMME_MODE IN (1,2) AND
                       AP.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchRegisteredAndSubmittedStudentInfoWithoutSelectedTable:
                    {
                        query = @"SELECT 
                                AR.RECEIVE_ID,
                                AI.ISSUED_ID,
                                PG.APPTYPE_ID,
                                CP.PROGRAMME_ID,
                                CP.PROGRAMME_NAME,
                                DATE_FORMAT(AR.RECEIVE_DATE, '%d/%m/%Y') AS RECEIVE_DATE,
                                AI.APPLICATION_NO,
                                AR.FIRST_NAME,
                                PG.SHIFT,
                                DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                PG.PROGRAMME_GROUP_ID,
                                AR.CASTE_ID,
                                AR.COMMUNITY_ID,
                                AR.CTALUK_CITY,
                                AR.SMS_MOBILE_NO,
                                AI.STATUS AS STATUS_ID,
                                ST.STATUS
                            FROM
                                ADM_RECEIVE_APPLICATION AS AR
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    AND AI.IS_PAID = 1
                                    AND AI.IS_DELETED != 1
                                    AND AI.STATUS = ?STATUS
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    AND PG.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                            WHERE
                                AR.IS_DELETED != 1
                                    AND AI.RECEIVE_ID NOT IN (SELECT 
                                        RECEIVE_ID
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                                    WHERE
                                        SL.RECEIVE_ID = AR.RECEIVE_ID
                                            AND (SL.IS_DELETED != 1 AND SL.IS_CANCELED!=1));";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentInfoWithoutRegisteredAndSubmittedByStatus:
                    {
                        query = @"SELECT 
                                AR.RECEIVE_ID,
                                AI.ISSUED_ID,
                                PG.APPTYPE_ID,
                                CP.PROGRAMME_ID,
                                CP.PROGRAMME_NAME,
                                DATE_FORMAT(AR.RECEIVE_DATE, '%d/%m/%Y') AS RECEIVE_DATE,
                                AI.APPLICATION_NO,
                                AR.FIRST_NAME,
                                PG.SHIFT,
                                DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                PG.PROGRAMME_GROUP_ID,
                                AR.CASTE_ID,
                                AR.COMMUNITY_ID,
                                AR.CTALUK_CITY,
                                AR.SMS_MOBILE_NO,
                                AI.STATUS AS STATUS_ID,
                                ST.STATUS
                            FROM
                                ADM_RECEIVE_APPLICATION AS AR
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    AND AI.IS_PAID = 1
                                    AND AI.IS_DELETED != 1
                                    AND AI.STATUS = ?STATUS
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    AND PG.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                            WHERE
                               AR.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchIssuedIdByProgrammeIdandReceiveId:
                    {
                        query = @"SELECT 
                                ISSUED_ID AS ISSUE_ID,RECEIVE_ID
                            FROM
                                ADM_ISSUED_APPLICATIONS
                            WHERE
		                            RECEIVE_ID =?RECEIVE_ID
                                    AND PROGRAMME_GROUP_ID =?PROGRAMME_FROM
                                    AND STATUS = ?STATUS 
                                    AND ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.UpdatePorgrammeByIssueId:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS 
                                SET 
                                    PROGRAMME_GROUP_ID =?PROGRAMME_TO,STATUS=?STATUS_ID,APPLICATION_NO = ?APPLICATION_NO,DEPARTMENT_CODE = ?APPLICATION_NO,ISSUE_NO = ?ISSUE_NO
                                WHERE
                                        ISSUED_ID=?ISSUE_ID";
                        break;
                    }   
                case AdmissionSQLCommands.UpdatePorgrammeIssueCount:
                    {
                        query = @"UPDATE CP_PROGRAMME_GROUP 
                                SET 
                                    IS_NEW = ?ISSUE_NO
                                WHERE
                                       PROGRAMME_GROUP_ID =?PROGRAMME_TO;";
                        break;
                    }
                case AdmissionSQLCommands.DeleteApplicantByProgrammeAndReceiveId:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR
                                SET 
                                    IS_DELETED = 1,
                                    IS_TRANSFERED = 1
                                WHERE
                                    RECEIVE_ID = ?RECEIVE_ID AND PROGRAMME_ID =?PROGRAMME_FROM;";
                        break;
                    }
                case AdmissionSQLCommands.FetchTransferDetails:
                    {
                        query = @"SELECT 
                                        AT.TRANSFER_ID,
                                        AT.APPLICATION_NO,
                                        DATE_FORMAT(AT.DATETIME, '%d/%m/%Y') DATETIME,
                                        CONCAT(IFNULL(AR.FIRST_NAME, ''),
                                                '. ',
                                                IFNULL(AR.LAST_NAME, '')) AS FIRST_NAME,
                                        CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                                ' (',
                                                IFNULL(SPM.PROGRAMME_MODE, ''),
                                                ') ) AS PROGRAMME_FROM,
                                        CONCAT(IFNULL(CP1.PROGRAMME_NAME, ''),
                                                ' (',
                                                IFNULL(SPM1.PROGRAMME_MODE, ''),
                                                ')') AS PROGRAMME_TO,
                                        AT.RECEIVE_ID,
                                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                '( ',
                                                IFNULL(SP.STAFF_CODE, ''),
                                                ')') AS REQUEST_ID,
                                        AT.IS_APPROVED
                                    FROM
                                        ADM_TRANSFER AS AT
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AT.RECEIVE_ID
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS APP ON APP.PROGRAMME_GROUP_ID = AT.PROGRAMME_FROM
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = APP.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SH ON SH.SHIFT_ID = APP.SHIFT
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = APP.PROGRAMME_MODE
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS APP1 ON APP1.PROGRAMME_GROUP_ID = AT.PROGRAMME_TO
                                            INNER JOIN
                                        CP_PROGRAMME AS CP1 ON CP1.PROGRAMME_ID = APP1.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SH1 ON SH1.SHIFT_ID = APP1.SHIFT
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM1 ON SPM1.PROGRAMME_MODE_ID = APP1.PROGRAMME_MODE
                                            LEFT JOIN
                                        STF_PERSONAL_INFO AS SP ON SP.STAFF_ID = AT.APPROVE_ID
                                    WHERE
                                            AT.PROGRAMME_FROM =?PROGRAMME_FROM AND
                                            AT.RECEIVE_ID=?RECEIVE_ID
                                            AND AT.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.FetchSelectionIdByReceiveIdandProgrammeId:
                    {
                        query = @"SELECT 
                                SELECTION_ID, PROGRAMME_ID, RECEIVE_ID
                            FROM
                                adm_selection_process_?AC_YEAR
                            WHERE
                                RECEIVE_ID =?RECEIVE_ID AND PROGRAMME_ID =?PROGRAMME_ID AND (IS_DELETED!=1 AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.UpdateSelectionBySelectionId:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR
                                    SET             
                                        IS_CANCELED = 1
                                    WHERE
                                        SELECTION_ID = ?SELECTION_ID AND (IS_DELETED!=1 AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.UpdateAdmIssueByIssueId:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS SET STATUS=?STATUS WHERE ISSUED_ID=?ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertOtherFeeForSpecialStudents:
                    {
                        query = @"INSERT INTO FEE_STUDENT_ACCOUNT (STUDENT_ID,ACADEMIC_YEAR,FREQUENCY_ID,HEAD,CREDIT,FEE_MAIN_HEAD_ID,FEE_STRUCTURE_ID,FEE_ROOT_ID ) VALUES (?STUDENT_ID,?AC_YEAR,?FREQUENCY,?HEAD,?AMOUNT,?FEE_MAIN_HEAD_ID,?FEE_STRUCTURE_ID,?FEE_ROOT_ID);";
                        break;
                    }
                case AdmissionSQLCommands.FetchOtherFeeForInsert:
                    {
                        query = @"SELECT 
                                AR.RECEIVE_ID AS STUDENT_ID,
                                FS.ACADEMIC_YEAR,
                                FS.FREQUENCY,
                                FS.HEAD,
                                FS.AMOUNT,
                                FS.FEE_MAIN_HEAD_ID,
                                FS.FEE_STRUCTURE_ID,
                                FS.FEE_ROOT_ID
                            FROM
                                FEE_STRUCTURE AS FS
                                    INNER JOIN
                                FEE_MAIN_HEADS AS MH ON MH.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                    AND FS.FREQUENCY = MH.FREQUENCY_ID
                                    AND FS.ACADEMIC_YEAR = MH.ACADEMIC_YEAR
                                    AND MH.IS_DELETED != 1
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = FS.PROGRAMME
                                    AND CPG.IS_DELETED != 1
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.PROGRAMME_GROUP_ID = CPG.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                    INNER JOIN
                                ADM_SELECTION_PROCESS_?AC_YEAR AS SP ON SP.RECEIVE_ID = AI.RECEIVE_ID
                                    AND SP.PROGRAMME_ID = AI.PROGRAMME_GROUP_ID
                                    AND (SP.IS_DELETED != 1
                                    AND SP.IS_CANCELED != 1)
                                    INNER JOIN
                                FEE_FREQUENCY_FEE_MODE AS FFF ON FFF.FEE_FREQUENCY_FEE_MODE_ID = FS.FREQUENCY
                                    INNER JOIN
                                SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    INNER JOIN
                                SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                    AND SST.IS_ACTIVE = 1
                                    INNER JOIN
                                SUP_HEAD AS SH ON SH.HEAD_ID = FS.HEAD
                                    AND SH.IS_DELETED != 1
                            WHERE
                                AI.RECEIVE_ID = ?RECEIVE_ID AND FREQUENCY = ?FREQUENCY
                                    AND FS.HEAD IN (80 , 81, 82, 83)
                                    AND FS.ACADEMIC_YEAR = ?AC_YEAR
                                    AND FS.IS_DELETED != 1
                                    AND AI.IS_DELETED != 1
                                    AND AR.IS_DELETED != 1
                                    AND FS.AMOUNT IS NOT NULL
                                    AND FS.AMOUNT != '';";
                        break;
                    }
                case AdmissionSQLCommands.CheckIsUserExist:
                    {
                        query = @"SELECT 
                            USER_ACCOUNT_ID, USERNAME
                        FROM
                            USER_ACCOUNT AS UA
                                INNER JOIN
                            ADM_RECEIVE_APPLICATION AS ADR ON ADR.EMAIL_ID = UA.USERNAME
                                AND ADR.HSC_NO =?HSC_NO
                        WHERE
                            USERNAME =?USERNAME;";
                        break;
                    }
                case AdmissionSQLCommands.UpdatePasswordByUserAcId:
                    {
                        query = @"UPDATE USER_ACCOUNT SET PASSWORD=SHA2(?PASSWORD,256) WHERE USER_ACCOUNT_ID=?USER_ACCOUNT_ID;";
                        break;
                    }
                case AdmissionSQLCommands.IsHscNoExist:
                    {
                        query = @"SELECT 
                                    COUNT(HSC_NO) > 0 as STATUS,RECEIVE_ID
                                FROM
                                    ADM_RECEIVE_APPLICATION
                                WHERE
                                    HSC_NO=?HSC_NO  AND IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchOcWise:
                    {
                        query = @"SELECT 
                                    ISS.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID',
                                    GC.MAIN_COMMUNITY_ID AS 'CASTE_ID'
                                FROM
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                                        AND REC.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                                        AND ISS.IS_DELETED != 1
                                        AND ISS.STATUS >= 3
                                        INNER JOIN
                                    GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                                WHERE
                                        SE.PROGRAMME_ID =?PROGRAMME_ID
                                        AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                                        AND SE.SELECTION_TYPE= 3
                                        AND (SE.IS_DELETED != 1 AND SE.IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchOcWiseAdmitted:
                    {
                        query = @"SELECT 
                                    ISS.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID',
                                    GC.MAIN_COMMUNITY_ID AS 'CASTE_ID'
                                FROM
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                                        AND REC.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                                        AND ISS.IS_DELETED != 1
                                        AND ISS.STATUS >= 5
                                        INNER JOIN
                                    GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                                WHERE
                                        SE.PROGRAMME_ID =?PROGRAMME_ID
                                        AND GC.MAIN_COMMUNITY_ID IN (?COMMUNITYID)
                                        AND SE.SELECTION_TYPE= 3
                                        AND (SE.IS_DELETED != 1 AND SE.IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchIssuedApplicationByProgrammeId:
                    {
                        query = @"SELECT 
                                           SL.PROGRAMME_ID,
                                            CONCAT(CP.PROGRAMME_NAME,
                                                    
                                                    '- ',
                                                    PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                            UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                            CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                            ST.STATUS_NAME,
                                            AR.SMS_MOBILE_NO,
                                            DATE_FORMAT(SL.SELECTION_DATE,'%d/%m/%Y') as SELECTION_DATE
    
                                        FROM
                                            ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                                                INNER JOIN
                                            ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                                LEFT JOIN
                                            ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SL.RECEIVE_ID
                                                AND AI.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                                                LEFT JOIN
                                            CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                LEFT JOIN
                                            CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                LEFT JOIN
                                            SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                                LEFT JOIN
                                            SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                                                LEFT JOIN
                                            SUP_MAIN_COMMUNITY AS CM ON CM.MAIN_COMMUNITY_ID = AR.CASTE_ID
                                                LEFT JOIN
                                            SUP_APPLICANT_TYPE AS APP ON APP.APPLICANT_TYPE_ID = SL.SELECTION_TYPE
                                        WHERE
		                                        SL.PROGRAMME_ID IN(?PROGRAMME_ID)
                                                AND SL.SELECTION_CYCLE =?SELECTION_CYCLE
                                                AND (SL.IS_DELETED != 1 AND SL.IS_CANCELED!=1)
                                        ORDER BY PG.PROGRAMME_GROUP_ID , AI.ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchSelectedApplicationByProgrammeId:
                    {
                        query = @"SELECT 
                                        WP.APP_ID,          
                                        WP.APPLICATION_NO,
                                        WP.APPLICATION_TYPE_ID,
                                        WP.PROGRAMME_ID,
                                        WP.SELECTION_TYPE,
                                        WP.SELECTION_CYCLE,
                                        WP.TOTAL_CUT_OFF_MARK,
                                        WP.TOTAL_SECURED,
                                        WP.MAX_TOTAL,
                                        WP.SELECTED_BY,
                                        WP.RECEIVE_ID,
                                        RP.FIRST_NAME,
                                        SF.FIRST_NAME AS STAFF_NAME,
                                        PG.PROGRAMME_NAME,
                                        RP.SMS_MOBILE_NO,
                                        WP.IS_SPORTS_QUOTA
                                    FROM
                                        adm_waiting_application_?AC_YEAR AS WP
                                            INNER JOIN
                                        adm_receive_application AS RP ON RP.RECEIVE_ID = WP.RECEIVE_ID
                                            INNER JOIN
                                        cp_programme_group AS CP ON CP.PROGRAMME_GROUP_ID = WP.PROGRAMME_ID
                                            INNER JOIN
                                        cp_programme AS PG ON PG.PROGRAMME_ID = CP.PROGRAMME_ID
                                            INNER JOIN
                                        stf_personal_info AS SF ON SF.STAFF_ID = WP.SELECTED_BY
                                            LEFT JOIN
                                        adm_selection_process_?AC_YEAR AS SL ON SL.PROGRAMME_ID = WP.PROGRAMME_ID
                                            AND SL.RECEIVE_ID = WP.RECEIVE_ID
                                    WHERE
                                        WP.PROGRAMME_ID IN (?PROGRAMME_ID) AND WP.STATUS=?STATUS
                                            AND WP.SELECTION_CYCLE = ?SELECTION_CYCLE
                                            AND WP.IS_DELETED != 1
                                            AND WP.IS_CANCELED != 1 AND SL.RECEIVE_ID IS NULL; ";
                        break;
                    }



                case AdmissionSQLCommands.SaveAdmissionSelectionProcess:
                    {
                        query = @" INSERT INTO adm_selection_process_?AC_YEAR(APPLICATION_NO, APPLICATION_TYPE_ID, PROGRAMME_ID, SELECTION_TYPE, SELECTION_CYCLE, TOTAL_CUT_OFF_MARK, TOTAL_SECURED, MAX_TOTAL, SELECTED_BY, RECEIVE_ID, IS_SPORTS_QUOTA)
                                        SELECT 
                                            APPLICATION_NO,
                                            APPLICATION_TYPE_ID,
                                            PROGRAMME_ID,
                                            SELECTION_TYPE,
                                            SELECTION_CYCLE,
                                            TOTAL_CUT_OFF_MARK,
                                            TOTAL_SECURED,
                                            MAX_TOTAL,
                                            SELECTED_BY,
                                            RECEIVE_ID,
                                            IS_SPORTS_QUOTA
                                          
                                        FROM
                                            adm_waiting_application_?AC_YEAR AS WP
                                        WHERE
                                            WP.PROGRAMME_ID IN (?PROGRAMME_ID) AND WP.STATUS = ?STATUS
                                                AND WP.SELECTION_CYCLE = ?SELECTION_CYCLE
                                                AND (WP.IS_DELETED != 1
                                                AND WP.IS_CANCELED != 1); ";
                        break;
                    }

                case AdmissionSQLCommands.FetchSelectedApplicants:
                    {

                        //          query = @"SELECT 
                        //                                          SL.PROGRAMME_ID,
                        //                                          CONCAT(CP.PROGRAMME_NAME,
                        //                                                  '- ',
                        //                                                  PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                        //                                          PG.APPTYPE_ID AS APPLICATION_TYPE_ID,
                        //                                          AY.APPLICATION_TYPE,
                        //                                          UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                        //                                          CONCAT(AI.APPLICATION_NO,
                        //                                                  LPAD(AI.ISSUED_ID,4, '0')) AS APPLICATION_NO,
                        //                                          ST.STATUS_NAME,
                        //                                          SL.SELECTION_TYPE,
                        //                                          APPLICANT_TYPE,
                        //                                          AR.SMS_MOBILE_NO,
                        //                                          DATE_FORMAT(SL.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                        //                                          SL.SELECTION_CYCLE AS SELECTION_CYCLE_ID,
                        //                                          SSC.SELECTION_CYCLE,
                        //                                          CM.MAIN_COMMUNITY_ID,
                        //                                          CM.MAIN_COMMUNITY,
                        //                                          AR.TOTAL_CUT_OFF_MARK,
                        //                                           PM.PROGRAMME_MODE_ID
                        //                                      FROM
                        //                                          ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                        //                                              INNER JOIN
                        //                                          ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                        //                                              LEFT JOIN
                        //                                          ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SL.RECEIVE_ID
                        //                                              AND AI.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                        //                                              LEFT JOIN
                        //                                          CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                        //                                              LEFT JOIN
                        //                                          CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                        //                                              LEFT JOIN
                        //                                          SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                        //                                              LEFT JOIN
                        //                                          SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                        //                                              INNER JOIN
                        //                                          SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                        //                                              LEFT JOIN
                        //GROUP_COMMUNITY AS GM ON GM.COMMUNITY_ID=AR.CASTE_ID
                        //                                              LEFT JOIN
                        //                                          SUP_MAIN_COMMUNITY AS CM ON CM.MAIN_COMMUNITY_ID = GM.MAIN_COMMUNITY_ID
                        //                                              LEFT JOIN
                        //                                          SUP_APPLICANT_TYPE AS APP ON APP.APPLICANT_TYPE_ID = SL.SELECTION_TYPE
                        //                                              INNER JOIN
                        //                                          SUP_SELECTION_CYCLE AS SSC ON SSC.SELECTION_CYCLE_ID = SL.SELECTION_CYCLE
                        //                                              AND SSC.IS_DELETED != 1
                        //                                              AND SSC.IS_ACTIVE = 1
                        //                                              AND SSC.IS_SHOW_WEBSITE = 1
                        //                                              INNER JOIN
                        //                                          SUP_APPLICATION_TYPE AS AY ON AY.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                        //                                              AND AY.IS_DELETED != 1

                        //                                      WHERE
                        //                                          (SL.IS_DELETED != 1
                        //                                              AND SL.IS_CANCELED != 1
                        //                                              AND IS_TRANSFERED != 1)
                        //                                              AND SSC.SELECTION_CYCLE_ID = 1  GROUP BY AR.RECEIVE_ID
                        //                                      ORDER BY AY.APPLICATION_TYPE_ID,CM.ORDER_ID, AR.TOTAL_CUT_OFF_MARK DESC;";



                        query = @"SELECT 
                                                                *
                                                            FROM
                                                                (SELECT 
                                                                    SL.PROGRAMME_ID,
                                                                        CONCAT(CP.PROGRAMME_NAME, '- ', PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                                                        PG.APPTYPE_ID AS APPLICATION_TYPE_ID,
                                                                        AY.APPLICATION_TYPE,
                                                                        UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                                                        CONCAT(AI.APPLICATION_NO, LPAD(AI.ISSUE_NO, 4, '0')) AS APPLICATION_NO,
                                                                        ST.STATUS_NAME,
                                                                        SL.SELECTION_TYPE,
                                                                        APPLICANT_TYPE,
                                                                        AR.SMS_MOBILE_NO,
                                                                        DATE_FORMAT(SL.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                                                        SL.SELECTION_CYCLE AS SELECTION_CYCLE_ID,
                                                                        SSC.SELECTION_CYCLE,
                                                                        CM.MAIN_COMMUNITY_ID,
                                                                        CM.MAIN_COMMUNITY,
                                                                        AR.TOTAL_CUT_OFF_MARK,
                                                                        PM.PROGRAMME_MODE_ID
                                                                FROM
                                                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                                                                INNER JOIN ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                                                LEFT JOIN ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SL.RECEIVE_ID
                                                                    AND AI.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                                                                LEFT JOIN CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                                    AND PG.PROGRAMME_GROUP_ID NOT IN (15 , 16)
                                                                LEFT JOIN CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                                LEFT JOIN SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                                                LEFT JOIN SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                                INNER JOIN SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                                                                LEFT JOIN GROUP_COMMUNITY AS GM ON GM.COMMUNITY_ID = AR.CASTE_ID
                                                                LEFT JOIN SUP_MAIN_COMMUNITY AS CM ON CM.MAIN_COMMUNITY_ID = GM.MAIN_COMMUNITY_ID
                                                                LEFT JOIN SUP_APPLICANT_TYPE AS APP ON APP.APPLICANT_TYPE_ID = SL.SELECTION_TYPE
                                                                INNER JOIN SUP_SELECTION_CYCLE AS SSC ON SSC.SELECTION_CYCLE_ID = SL.SELECTION_CYCLE
                                                                    AND SSC.IS_DELETED != 1
                                                                    AND SSC.IS_ACTIVE = 1
                                                                    AND SSC.IS_SHOW_WEBSITE = 1
                                                                INNER JOIN SUP_APPLICATION_TYPE AS AY ON AY.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                                                                    AND AY.IS_DELETED != 1
                                                                WHERE
                                                                    (SL.IS_DELETED != 1
                                                                        AND SL.IS_CANCELED != 1
                                                                        AND IS_TRANSFERED != 1)
                                                                        AND SSC.SELECTION_CYCLE_ID = 1) AS A 
                                                            UNION (SELECT 
                                                                SL.PROGRAMME_ID,
                                                                CONCAT(CP.PROGRAMME_NAME,
                                                                        '- ',
                                                                        PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                                                PG.APPTYPE_ID AS APPLICATION_TYPE_ID,
                                                                AY.APPLICATION_TYPE,
                                                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                                                CONCAT(AI.APPLICATION_NO,
                                                                        LPAD(AI.ISSUE_NO, 4, '0')) AS APPLICATION_NO,
                                                                ST.STATUS_NAME,
                                                                SL.SELECTION_TYPE,
                                                                APPLICANT_TYPE,
                                                                AR.SMS_MOBILE_NO,
                                                                DATE_FORMAT(SL.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                                                SL.SELECTION_CYCLE AS SELECTION_CYCLE_ID,
                                                                SSC.SELECTION_CYCLE,
                                                                CM.MAIN_COMMUNITY_ID,
                                                                CM.MAIN_COMMUNITY,
                                                                MR.MARK AS TOTAL_CUT_OFF_MARK,
                                                                PM.PROGRAMME_MODE_ID
                                                            FROM
                                                                ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                                                                    INNER JOIN
                                                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                                                    LEFT JOIN
                                                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SL.RECEIVE_ID
                                                                    AND AI.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                                                                    LEFT JOIN
                                                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                                    LEFT JOIN
                                                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                                    LEFT JOIN
                                                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                                                    LEFT JOIN
                                                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                                    INNER JOIN
                                                                SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                                                                    LEFT JOIN
                                                                GROUP_COMMUNITY AS GM ON GM.COMMUNITY_ID = AR.CASTE_ID
                                                                    LEFT JOIN
                                                                SUP_MAIN_COMMUNITY AS CM ON CM.MAIN_COMMUNITY_ID = GM.MAIN_COMMUNITY_ID
                                                                    LEFT JOIN
                                                                SUP_APPLICANT_TYPE AS APP ON APP.APPLICANT_TYPE_ID = SL.SELECTION_TYPE
                                                                    INNER JOIN
                                                                SUP_SELECTION_CYCLE AS SSC ON SSC.SELECTION_CYCLE_ID = SL.SELECTION_CYCLE
                                                                    AND SSC.IS_DELETED != 1
                                                                    AND SSC.IS_ACTIVE = 1
                                                                    AND SSC.IS_SHOW_WEBSITE = 1
                                                                    INNER JOIN
                                                                SUP_APPLICATION_TYPE AS AY ON AY.APPLICATION_TYPE_ID = PG.APPTYPE_ID
                                                                    AND AY.IS_DELETED != 1
                                                                    LEFT JOIN
                                                                adm_stu_submarks AS MR ON MR.RECEIVE_STUID = SL.RECEIVE_ID
                                                                    INNER JOIN
                                                                sup_hss_subjects AS HS ON HS.HSS_SUBJECT_ID = MR.SUBJECT_ID
                                                                    AND HS.HSS_SUBJECT_ID IN (2 , 75)
                                                            WHERE
                                                                (SL.IS_DELETED != 1
                                                                    AND SL.IS_CANCELED != 1
                                                                    AND IS_TRANSFERED != 1)
                                                                    AND SSC.SELECTION_CYCLE_ID = 1
                                                                    AND PG.PROGRAMME_GROUP_ID IN (15 , 16)
                                                            GROUP BY AR.RECEIVE_ID);";
                        break;
                    }
                case AdmissionSQLCommands.IsRegistered:
                    {
                        query = @"SELECT 
                                        RECEIVE_ID
                                    FROM
                                        ADM_ISSUED_APPLICATIONS
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID AND STATUS >= 2;";
                        break;
                    }
                case AdmissionSQLCommands.FetchProgrammeWiseQutoa:
                    {
                        query = @"SELECT 
                                        MT.INTAKE_ID,
                                        MT.DEPARTMENT_ID,
                                        MT.SHIFT,
                                        (MT.NO_OF_SEATS) AS NO_OF_SEATS,
                                        MT.MINORITY,
                                        MT.ACADEMIC_YEAR,
                                        MT.PROGRAMME_ID,
                                        P.PROGRAMME_NAME AS PROGRAMME_NAME,
                                        CQ.CASTE_ID,
                                        CQ.NO_OF_SEATS AS 'CQ_SEATS'
                                    FROM
                                        ADM_MAXIMUM_IN_TAKES AS MT
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = MT.PROGRAMME_ID
                                            AND CPG.IS_DELETED != 1
                                            INNER JOIN
                                        ADM_CASTEWISE_QUATA AS CQ ON CQ.INTAKE_ID = MT.INTAKE_ID
                                            AND CQ.ACADEMIC_YEAR =?AC_YEAR
                                            AND CQ.IS_DELETED != 1
                                            INNER JOIN
                                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = CPG.PROGRAMME_ID
                                            AND P.IS_DELETED != 1
                                    WHERE
                                        MT.PROGRAMME_ID IN (?PROGRAMME_ID)
                                            AND MT.ACADEMIC_YEAR = ?AC_YEAR
                                            AND MT.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchMinorityCount:
                    {
                        query = @"SELECT 
                                SE.PROGRAMME_ID,
                                GC.MAIN_COMMUNITY_ID AS 'CASTE_ID',
                                COUNT(SE.RECEIVE_ID) AS SELECTED,
                                    COUNT(IF(SE.IS_VERIFIED = 1, 1, NULL)) AS VERIFIED,
                                    COUNT(IF((ISS.STATUS = 5) = 1, 1, NULL)) AS ADMITTED
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                                    AND REC.IS_DELETED != 1
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                                    AND ISS.IS_DELETED != 1
                                    AND ISS.STATUS >= 3
                                    INNER JOIN
                                GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                            WHERE
		                            SE.PROGRAMME_ID IN(?PROGRAMME_ID)
                                    AND REC.IS_ROMAN_CATHOLIC = 1
                                    AND (SE.IS_DELETED != 1 AND SE.IS_CANCELED!=1) GROUP BY SE.PROGRAMME_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchOthersCasteWise:
                    {
                        query = @"SELECT 
                                ISS.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID',
                                GC.MAIN_COMMUNITY_ID AS 'CASTE_ID',
 COUNT(SE.RECEIVE_ID) AS SELECTED,
    COUNT(IF(SE.IS_VERIFIED = 1, 1, NULL)) AS VERIFIED,
    COUNT(IF((ISS.STATUS = 5) = 1, 1, NULL)) AS ADMITTED
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                                    AND REC.IS_DELETED != 1
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                                    AND ISS.IS_DELETED != 1
                                    AND ISS.STATUS >= 3
                                    INNER JOIN
                                GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                            WHERE
		                            SE.PROGRAMME_ID IN(?PROGRAMME_ID)
                                    AND SE.SELECTION_TYPE != 1 AND SE.SELECTION_TYPE != 3
                                    AND (SE.IS_DELETED != 1 ANS SE.IS_CANCELED!=1) GROUP BY SE.PROGRAMME_ID, GC.MAIN_COMMUNITY_ID  ;";
                        break;
                    }
                case AdmissionSQLCommands.FetchOcCasteWise:
                    {
                        query = @"SELECT 
                            ISS.PROGRAMME_GROUP_ID AS 'PROGRAMME_ID',
                            GC.MAIN_COMMUNITY_ID AS 'CASTE_ID',
COUNT(SE.RECEIVE_ID) AS SELECTED,
    COUNT(IF(SE.IS_VERIFIED = 1, 1, NULL)) AS VERIFIED,
    COUNT(IF((ISS.STATUS = 5) = 1, 1, NULL)) AS ADMITTED
                        FROM
                            ADM_SELECTION_PROCESS_?AC_YEAR AS SE
                                INNER JOIN
                            ADM_RECEIVE_APPLICATION AS REC ON REC.RECEIVE_ID = SE.RECEIVE_ID
                                AND REC.IS_DELETED != 1
                                INNER JOIN
                            ADM_ISSUED_APPLICATIONS AS ISS ON ISS.RECEIVE_ID = REC.RECEIVE_ID
                                AND ISS.IS_DELETED != 1
                                AND ISS.STATUS >= 3
                                INNER JOIN
                            GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = REC.CASTE_ID
                        WHERE
		                        SE.PROGRAMME_ID =?PROGRAMME_ID
                                AND SE.SELECTION_TYPE != 1 AND SE.SELECTION_TYPE != 2
                                AND (SE.IS_DELETED != 1 AND SE.IS_CANCELED!=1) GROUP BY SE.PROGRAMME_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmSelectedStudentByStatusAndProgrammeAndCycle:
                    {
                        query = @"SELECT 
                                PG.PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                       
                                        '- ',
                                        PM.PROGRAMME_MODE) AS PROGRAMME,
                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                DATE_FORMAT(SL.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                ST.STATUS_NAME,
                                APP.APPLICANT_TYPE AS SELECTION_CYCLE,
                                CM.MAIN_COMMUNITY AS CASTE,
                                AR.SMS_MOBILE_NO
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                    LEFT JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SL.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                                    AND AI.STATUS = ?STATUS
                                    LEFT JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    LEFT JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    LEFT JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    LEFT JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS                                   
                                INNER JOIN group_community AS GC ON GC.COMMUNITY_ID = AR.CASTE_ID
                                    LEFT JOIN
                                SUP_MAIN_COMMUNITY AS CM ON CM.MAIN_COMMUNITY_ID = GC.MAIN_COMMUNITY_ID
                                    LEFT JOIN
                                SUP_APPLICANT_TYPE AS APP ON APP.APPLICANT_TYPE_ID = SL.SELECTION_TYPE
                            WHERE
                                SL.PROGRAMME_ID IN (?PROGRAMME_GROUP_ID) AND SL.SELECTION_CYCLE=?SELECTION_CYCLE AND (SL.IS_DELETED!=1 AND SL.IS_CANCELED!=1)
                            ORDER BY PG.PROGRAMME_GROUP_ID , AI.ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmSelectedStudentByStatusForVerificationProgrammeAndCycle:
                    {
                        query = @"SELECT 
                                PG.PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                       
                                        '- ',
                                        PM.PROGRAMME_MODE) AS PROGRAMME,
                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                DATE_FORMAT(SL.SELECTION_DATE, '%d/%m/%Y') AS SELECTION_DATE,
                                -- DATE_FORMAT(SL.VERIFIED_DATE, '%d/%m/%Y') AS VERIFIED_DATE,
                                ST.STATUS_NAME,
                                APP.APPLICANT_TYPE AS SELECTION_CYCLE,
                                CM.MAIN_COMMUNITY AS CASTE,
                                AR.SMS_MOBILE_NO
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS SL
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = SL.RECEIVE_ID
                                    LEFT JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = SL.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
                                    AND AI.STATUS = ?STATUS
                                    LEFT JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    LEFT JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    LEFT JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    LEFT JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS                                   
                                INNER JOIN group_community AS GC ON GC.COMMUNITY_ID = AR.CASTE_ID
                                    LEFT JOIN
                                SUP_MAIN_COMMUNITY AS CM ON CM.MAIN_COMMUNITY_ID = GC.MAIN_COMMUNITY_ID
                                    LEFT JOIN
                                SUP_APPLICANT_TYPE AS APP ON APP.APPLICANT_TYPE_ID = SL.SELECTION_TYPE
                            WHERE
                                SL.PROGRAMME_ID IN (?PROGRAMME_GROUP_ID) AND SL.SELECTION_CYCLE=?SELECTION_CYCLE AND (SL.IS_DELETED!=1 AND SL.IS_CANCELED!=1)
                            ORDER BY PG.PROGRAMME_GROUP_ID , AI.ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmRegisteredApplicationByProgrammeAndStatusForStaff:
                    {
                        query = @"SELECT 
                                PG.PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                        
                                        '- ',
                                        PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                ST.STATUS_NAME,
                                CM.MAIN_COMMUNITY AS CASTE,
                                AR.SMS_MOBILE_NO
                            FROM
                                ADM_RECEIVE_APPLICATION AS AR
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    AND AI.STATUS = ?STATUS
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    LEFT JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    LEFT JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    LEFT JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS                                 
                                    LEFT JOIN
                                group_community AS GC ON GC.COMMUNITY_ID = AR.CASTE_ID
                                    LEFT JOIN
                                SUP_MAIN_COMMUNITY AS CM ON CM.MAIN_COMMUNITY_ID = GC.MAIN_COMMUNITY_ID
                            WHERE
                                AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                            ORDER BY PG.PROGRAMME_GROUP_ID , AI.ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchIssuedApplicationForReport:
                    {
                        query = @"SELECT 
                                PG.PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                       
                                        '- ',
                                        PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                ST.STATUS_NAME,
                                AR.SMS_MOBILE_NO,
                                AR.EMAIL_ID,
                                AR.HSC_NO,
                                DATE_FORMAT(DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH
                            FROM
                                adm_receive_application AS AR
                                    INNER JOIN
                                adm_issued_applications AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    AND AI.IS_PAID = 1
                                    AND AI.IS_DELETED != 1
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    LEFT JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    LEFT JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    LEFT JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_ADM_STATUS AS ST ON ST.STATUS_ID = AI.STATUS
                            WHERE
                                AR.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.IsProgrammeExistByReceiveId:
                    {
                        query = @"SELECT 
                                    CONCAT(ADI.APPLICATION_NO,LPAD(ADI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    ADI.RECEIVE_ID,
                                    CONCAT(CP.PROGRAMME_NAME,
                                            '-',
                                            PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                    SAS.STATUS_NAME AS STATUS
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS ADI
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = ADI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                                        INNER JOIN
                                    SUP_SHIFT AS SS ON SS.SHIFT_ID = CPG.SHIFT
                                        INNER JOIN
                                    SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID = ADI.STATUS
                                WHERE
                                    ADI.PROGRAMME_GROUP_ID = ?PROGRAMME_TO
                                        AND ADI.RECEIVE_ID =?RECEIVE_ID
                                        AND ADI.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.RevertFromProgrammeByIssuedId:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS 
                                SET 
                                    STATUS=?STATUS_ID
                                WHERE
                                        ISSUED_ID=?ISSUE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.DeleteandUpdateCancelByReceiveandProgrammeId:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR 
                                SET 
	                                IS_CANCELED=1,
                                    IS_DELETED = 1
    
                                WHERE
                                    RECEIVE_ID =?RECEIVE_ID  AND PROGRAMME_ID =?PROGRAMME_FROM AND (IS_DELETED!=1 AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchCanceldetails:
                    {
                        query = @"SELECT 
                                    CONCAT(ADI.APPLICATION_NO,LPAD(ADI.ISSUE_NO,4,'0')) AS APPLICATION_NO,                                    
                                    ADI.RECEIVE_ID,
                                    CONCAT(CP.PROGRAMME_NAME,
                                            '-',
                                            PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                    SAS.STATUS_NAME AS STATUS
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS ADI
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = ADI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                                        INNER JOIN
                                    SUP_SHIFT AS SS ON SS.SHIFT_ID = CPG.SHIFT
                                        INNER JOIN
                                    SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID = ADI.STATUS
                                WHERE
                                    ADI.PROGRAMME_GROUP_ID = ?PROGRAMME_FROM
                                        AND ADI.RECEIVE_ID =?RECEIVE_ID
                                        AND ADI.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateCancel:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS 
                                SET 
                                 STATUS=?STATUS_ID
                                WHERE
                                        ISSUED_ID=?ISSUE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchSelectedListByProgrammeId:
                    {
                        query = @"SELECT 
                                        SP.SELECTION_ID,
                                        SP.RECEIVE_ID,
                                        ADR.FIRST_NAME,
                                        SP.APPLICATION_NO,
                                        CONCAT(CP.PROGRAMME_NAME,
                                                '-',
                                                PM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                        SP.PROGRAMME_ID,
                                        SP.TOTAL_SECURED,
                                        SP.TOTAL_CUT_OFF_MARK,
                                        SDS.STATUS_NAME,
                                        ADR.SMS_MOBILE_NO
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = SP.RECEIVE_ID
                                            INNER JOIN
                                        ADM_ISSUED_APPLICATIONS AS ADI ON ADI.RECEIVE_ID = ADR.RECEIVE_ID
                                            AND ADI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                            AND ADI.STATUS = 3
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                                            INNER JOIN
                                        SUP_SHIFT AS SS ON SS.SHIFT_ID = CPG.SHIFT
                                            INNER JOIN
                                        SUP_ADM_STATUS AS SDS ON SDS.STATUS_ID = ADI.STATUS
                                    WHERE
		                                    SP.PROGRAMME_ID IN (?PROGRAMME_ID)
                                            AND SP.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.CancelSelectedBySelectionId:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR
                                    SET 
                                        IS_DELETED = 1
                                    WHERE
                                        SELECTION_ID =?SELECTION_ID AND (IS_DELETED!=1 AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.RevertStatusByReceiveIdandProgrammeId:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS 
                                    SET 
                                        STATUS =?STATUS
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID
                                            AND PROGRAMME_GROUP_ID =?PROGRAMME_ID;";
                        break;
                    }
                case AdmissionSQLCommands.CancelSelectionByReceiveIdandProgrammeId:
                    {
                        query = @"UPDATE adm_waiting_application_?AC_YEAR 
                                    SET 
                                        IS_DELETED =1
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID
                                            AND PROGRAMME_ID =?PROGRAMME_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateSelectionByReceiveIdandProgrammeId:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR
                                SET 
                                    PROGRAMME_ID=?PROGRAMME_TO,
                                    IS_TRANSFERED = 1
                                WHERE
                                    RECEIVE_ID = ?RECEIVE_ID AND PROGRAMME_ID =?PROGRAMME_FROM AND (IS_DELETED!=1 AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStatusForDashboard:
                    {
                        query = @"SELECT 
                                CSL.RECEIVE_ID,
                                CONCAT(IA.APPLICATION_NO, LPAD(IA.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                CONCAT(P.PROGRAMME_NAME,
                                        '-',
                                        PM.PROGRAMME_MODE,
                                        ' (',
                                        SH.DESCRIPTION,
                                        '-',
                                        SH.TIME,
                                        ')') AS PROGRAMME_NAME,
                                S.STATUS,
                                PTM.PAYMENT_MODE,
                                H.HOSTEL_NAME
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS CSL
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = CSL.RECEIVE_ID
                                    AND HOSTEL_FACILITY = 1
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS IA ON IA.RECEIVE_ID = CSL.RECEIVE_ID
                                    AND CSL.PROGRAMME_ID = IA.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = IA.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_SHIFT AS SH ON SH.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_PAYMENT_MODE AS PTM ON PTM.PAYMENT_MODE_ID = IA.PAYMENT_MODE
                                    LEFT JOIN
                                ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS HSL ON HSL.RECEIVE_ID = CSL.RECEIVE_ID
                                    AND HSL.PROGRAMME_GROUP_ID = CSL.PROGRAMME_ID AND HSL.IS_DELETED!=1
                                    LEFT JOIN
                                SUP_STATUS AS S ON S.STATUS_ID = HSL.STATUS
                                    LEFT JOIN
                                sup_hostel AS H ON H.HOSTEL_ID = HSL.HOSTEL_ID
                            WHERE
                                CSL.RECEIVE_ID = ?RECEIVE_ID AND CSL.IS_DELETED!=1
                                    AND IA.ACADEMIC_YEAR = ?AC_YEAR
                                    AND IS_PAID = 1;";
                        break;
                    }

                case AdmissionSQLCommands.FetchQuataWiseReportByProgrammeAndCaste:
                    {
                        query = @"SELECT 
    TOTAL_SELECTED,
    TOTAL_VERIFIED,
    TOTAL_ADMITTED,
    MX.NO_OF_SEATS,
    T.PROGRAMME_ID,
    MINORITY_SELECTED,
    MINORITY_VERIFIED,
    MINORITY_ADMITTED,
    MX.MINORITY AS MINORITY_ALLOATED,
    OC_SELECTED,
    OC_VERIFIED,
    OC_ADMITTED,
    (SELECT 
            C.NO_OF_SEATS
        FROM
            ADM_MAXIMUM_IN_TAKES AS MX1
                INNER JOIN
            ADM_CASTEWISE_QUATA AS C ON C.INTAKE_ID = MX1.INTAKE_ID
                AND C.ACADEMIC_YEAR = 2021
        WHERE
            MX.ACADEMIC_YEAR = 2021
                AND MX1.PROGRAMME_ID = T.PROGRAMME_ID
                AND MX1.INTAKE_ID = MX.INTAKE_ID
                AND C.CASTE_ID = 2) AS OC_ALLOATED,
    BC_SELECTED,
    BC_VERIFIED,
    BC_ADMITTED,
    (SELECT 
            C.NO_OF_SEATS
        FROM
            ADM_MAXIMUM_IN_TAKES AS MX1
                INNER JOIN
            ADM_CASTEWISE_QUATA AS C ON C.INTAKE_ID = MX1.INTAKE_ID
                AND C.ACADEMIC_YEAR = 2021
        WHERE
            MX.ACADEMIC_YEAR = 2021
                AND MX1.PROGRAMME_ID = T.PROGRAMME_ID
                AND MX1.INTAKE_ID = MX.INTAKE_ID
                AND C.CASTE_ID = 1) AS BC_ALLOATED,
    SC_ST_SELECTED,
    SC_ST_VERIFIED,
    SC_ST_ADMITTED,
    (SELECT 
            C.NO_OF_SEATS
        FROM
            ADM_MAXIMUM_IN_TAKES AS MX1
                INNER JOIN
            ADM_CASTEWISE_QUATA AS C ON C.INTAKE_ID = MX1.INTAKE_ID
                AND C.ACADEMIC_YEAR = 2021
        WHERE
            MX.ACADEMIC_YEAR = 2021
                AND MX1.PROGRAMME_ID = T.PROGRAMME_ID
                AND MX1.INTAKE_ID = MX.INTAKE_ID
                AND C.CASTE_ID = 3) AS SC_ST_ALLOATED,
    `MBC_DNC_SELECTED`,
    `MBC_DNC_VERIFIED`,
    `MBC_DNC_ADMITTED`,
    (SELECT 
            C.NO_OF_SEATS
        FROM
            ADM_MAXIMUM_IN_TAKES AS MX1
                INNER JOIN
            ADM_CASTEWISE_QUATA AS C ON C.INTAKE_ID = MX1.INTAKE_ID
                AND C.ACADEMIC_YEAR = 2021
        WHERE
            MX.ACADEMIC_YEAR = 2021
                AND MX1.PROGRAMME_ID = T.PROGRAMME_ID
                AND MX1.INTAKE_ID = MX.INTAKE_ID
                AND C.CASTE_ID = 4) AS `MBC_DNC_ALLOATED`,
    OTHER_SELECTED,
    OTHER_VERIFIED,
    OTHER_ADMITTED
FROM
    (SELECT 
        SL.PROGRAMME_ID,
            COUNT(IF(RE.STATUS >= 3, 1, NULL)) AS TOTAL_SELECTED,
            COUNT(IF(SL.IS_VERIFIED = 1, 1, NULL)) AS TOTAL_VERIFIED,
            COUNT(IF(RE.STATUS = 5, 1, NULL)) AS TOTAL_ADMITTED,
            COUNT(IF(SL.SELECTION_TYPE = 1 AND RE.STATUS >= 3, 1, NULL)) AS MINORITY_SELECTED,
            COUNT(IF(SL.SELECTION_TYPE = 1
                AND SL.IS_VERIFIED = 1, 1, NULL)) AS MINORITY_VERIFIED,
            COUNT(IF(SL.SELECTION_TYPE = 1 AND RE.STATUS = 5, 1, NULL)) AS MINORITY_ADMITTED,
            COUNT(IF(SL.SELECTION_TYPE = 3 AND RE.STATUS >= 3, 1, NULL)) AS OC_SELECTED,
            COUNT(IF(SL.SELECTION_TYPE = 3
                AND SL.IS_VERIFIED = 1, 1, NULL)) AS OC_VERIFIED,
            COUNT(IF(SL.SELECTION_TYPE = 3 AND RE.STATUS = 5, 1, NULL)) AS OC_ADMITTED,
            COUNT(IF(SL.SELECTION_TYPE = 2 AND RE.STATUS >= 3
                AND SM.MAIN_COMMUNITY_ID = 1, 1, NULL)) AS BC_SELECTED,
            COUNT(IF(SL.SELECTION_TYPE = 2
                AND SL.IS_VERIFIED = 1
                AND SM.MAIN_COMMUNITY_ID = 1, 1, NULL)) AS BC_VERIFIED,
            COUNT(IF(SL.SELECTION_TYPE = 2 AND RE.STATUS = 5
                AND SM.MAIN_COMMUNITY_ID = 1, 1, NULL)) AS BC_ADMITTED,
            COUNT(IF(SL.SELECTION_TYPE = 2 AND RE.STATUS >= 3
                AND SM.MAIN_COMMUNITY_ID = 3, 1, NULL)) AS SC_ST_SELECTED,
            COUNT(IF(SL.SELECTION_TYPE = 2
                AND SL.IS_VERIFIED = 1
                AND SM.MAIN_COMMUNITY_ID = 3, 1, NULL)) AS SC_ST_VERIFIED,
            COUNT(IF(SL.SELECTION_TYPE = 2 AND RE.STATUS = 5
                AND SM.MAIN_COMMUNITY_ID = 3, 1, NULL)) AS SC_ST_ADMITTED,
            COUNT(IF(SL.SELECTION_TYPE = 2 AND RE.STATUS >= 3
                AND SM.MAIN_COMMUNITY_ID = 4, 1, NULL)) AS `MBC_DNC_SELECTED`,
            COUNT(IF(SL.SELECTION_TYPE = 2
                AND SL.IS_VERIFIED = 1
                AND SM.MAIN_COMMUNITY_ID = 4, 1, NULL)) AS `MBC_DNC_VERIFIED`,
            COUNT(IF(SL.SELECTION_TYPE = 2 AND RE.STATUS = 5
                AND SM.MAIN_COMMUNITY_ID = 4, 1, NULL)) AS `MBC_DNC_ADMITTED`,
            COUNT(IF(SL.SELECTION_TYPE = 2 AND RE.STATUS >= 3
                AND SM.MAIN_COMMUNITY_ID IN (2 , 5), 1, NULL)) AS OTHER_SELECTED,
            COUNT(IF(SL.SELECTION_TYPE = 2
                AND SL.IS_VERIFIED = 1
                AND SM.MAIN_COMMUNITY_ID IN (2 , 5), 1, NULL)) AS OTHER_VERIFIED,
            COUNT(IF(SL.SELECTION_TYPE = 2 AND RE.STATUS = 5
                AND SM.MAIN_COMMUNITY_ID IN (2 , 5), 1, NULL)) AS OTHER_ADMITTED
    FROM
        ADM_SELECTION_PROCESS_2021 AS SL
    INNER JOIN ADM_RECEIVE_APPLICATION AS R ON R.RECEIVE_ID = SL.RECEIVE_ID
    INNER JOIN ADM_ISSUED_APPLICATIONS AS RE ON RE.RECEIVE_ID = SL.RECEIVE_ID
        AND RE.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
    INNER JOIN CP_PROGRAMME_GROUP AS CP ON CP.PROGRAMME_GROUP_ID = SL.PROGRAMME_ID
    INNER JOIN group_community AS GC ON GC.COMMUNITY_ID=R.CASTE_ID
    INNER JOIN sup_main_community AS SM ON SM.MAIN_COMMUNITY_ID=GC.MAIN_COMMUNITY_ID
    INNER JOIN SUP_APPLICANT_TYPE AS A ON A.APPLICANT_TYPE_ID = SL.SELECTION_TYPE
    WHERE
        
             SL.SELECTION_TYPE IN (3 , 2, 1)
            AND (SL.IS_DELETED != 1 AND SL.IS_CANCELED!=1)
    GROUP BY SL.PROGRAMME_ID) T
        INNER JOIN
    ADM_MAXIMUM_IN_TAKES AS MX ON MX.PROGRAMME_ID = T.PROGRAMME_ID
        AND MX.ACADEMIC_YEAR = 2021;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentsByProgrammeId:
                    {
                        query = @"SELECT 
                                        SP.RECEIVE_ID,
                                        SP.PROGRAMME_ID,
                                        ADR.FIRST_NAME,
                                        ADR.CLASS,
                                        SP.APPLICATION_NO,
                                        DATE_FORMAT(ADR.DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH,
                                        CC.CLASS_NAME,
                                        ADR.ROLL_NO,
                                        ADI.STATUS,
                                        SS.STATUS_NAME
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                            INNER JOIN
                                        ADM_ISSUED_APPLICATIONS AS ADI ON SP.RECEIVE_ID = ADI.RECEIVE_ID
                                            AND ADI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                            AND ADI.STATUS=5
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = ADI.RECEIVE_ID
                                            LEFT JOIN
                                        CP_CLASSES AS CC ON CC.CLASS_ID = ADR.CLASS
                                        INNER JOIN sup_adm_status AS SS ON SS.STATUS_ID=ADI.STATUS
                                    WHERE
                                        SP.PROGRAMME_ID =?PROGRAMME_ID
                                            AND (SP.IS_DELETED != 1 AND SP.IS_CANCELED!=1)
                                    ORDER BY ADR.FIRST_NAME,ADR.DATE_OF_BIRTH;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateClassByReceiveId:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        CLASS =?CLASS
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentsByClassId:
                    {
                        query = @"SELECT 
                                    SP.RECEIVE_ID,
                                    SP.PROGRAMME_ID,
                                    ADR.FIRST_NAME,
                                    ADR.CLASS,
                                    SP.APPLICATION_NO,
                                    DATE_FORMAT(ADR.DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH,
                                    CC.CLASS_NAME,
                                    ADR.ROLL_NO,
                                    CONCAT(CPG.PREFIX, CPG.SUFFIX) AS PREFIX,
                                    CPG.RUN_ID,
                                    CPG.SHIFT,
                                    CPG.PROGRAMME_MODE,
                                    CC.DEPARTMENT,
                                    SS.STATUS_NAME,
                                    ADI.STATUS
                                FROM
                                    ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS ADI ON SP.RECEIVE_ID = ADI.RECEIVE_ID
                                        AND ADI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                        AND ADI.STATUS=5
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = ADI.RECEIVE_ID
                                        AND ADR.CLASS =?CLASS
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = ADI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                        LEFT JOIN
                                    CP_CLASSES AS CC ON CC.CLASS_ID = ADR.CLASS
                                        INNER JOIN
                                    sup_adm_status AS SS ON ADI.STATUS = SS.STATUS_ID
                                WHERE
                                    (SP.IS_DELETED != 1 AND SP.IS_CANCELED!=1)
                                ORDER BY ADR.FIRST_NAME , ADR.DATE_OF_BIRTH;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateRollnoByReceiveId:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        ROLL_NO =?ROLL_NO
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID;";
                        break;
                    }

                case AdmissionSQLCommands.UpdateLastRuningId:
                    {
                        query = @"UPDATE CP_PROGRAMME_GROUP SET RUN_ID=?RUN_ID WHERE PROGRAMME_GROUP_ID IN(?PROGRAMME_GROUP_ID);";
                        break;
                    }
                case AdmissionSQLCommands.InsertStupersonalInfo:
                    {
                        query = @"INSERT ?TODB.STU_PERSONAL_INFO(ADMISSION_NO,
                                    ADMISSION_DATE,
                                    ADMITTED_CLASS,
                                    DEPT_ID,
                                    PROGRAM_ID,
                                    GENDER,
                                    SHIFT_ID,
                                    DATE_OF_BIRTH,
                                    MOTHER_TONGUE,
                                    BLOOD_GROUP,
                                    RELIGION,
                                    FATHER_NAME,
                                    FR_OCCUPATION,
                                    MOTHER_NAME,
                                    ANNUAL_INCOME,
                                    PHOTO,
                                    FR_MOBILE,
                                    MO_MOBILE,
                                    FIRST_NAME,
                                    CASTE,
                                    ROLL_NO,
                                    CLASS_ID,
                                    STU_MOBILENO,
                                    STU_EMAILID,
                                    BATCH,
                                    PROGRAMME_MODE,ACADEMIC_YEAR,RECEIVE_ID) SELECT ?APPLICATION_NO,RECEIVE_DATE,CLASS,?DEPARTMENT,
                                    ?PROGRAMME_ID,
                                    GENDER,?SHIFT
                                    ,DATE_OF_BIRTH
                                    ,MOTHER_TONGUE
                                    ,BLOOD_GROUP
                                    ,RELIGION_ID,
                                    FATHER_NAME,
                                    OCCUPATION,
                                    MOTHER_NAME,
                                    ANNUAL_INCOME,
                                    PHOTO_PATH,
                                    SMS_MOBILE_NO,
	                                PMOBILENO,
                                    FIRST_NAME,
                                    CASTE_ID,
                                    ROLL_NO,
                                    CLASS,
                                    SMS_MOBILE_NO,
                                    EMAIL_ID,
                                    ?BATCH_ID,
                                    ?PROGRAMME_MODE,
                                    ?ACADEMIC_YEAR,RECEIVE_ID
                                    FROM ADM_RECEIVE_APPLICATION WHERE RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertStuclass:
                    {
                        query = @"INSERT
                                 INTO 
                                  ?TODB.STU_CLASS(STUDENT_ID,
                                 ACADEMIC_YEAR,CLASS_ID,
                                 PROGRAMME_GROUP_ID,
                                 BATCH)
                                 VALUES(?STUDENT_ID,
                                 ?ACADEMIC_YEAR,
                                 ?CLASS_ID,
                                 ?PROGRAMME_GROUP_ID,
                                 ?BATCH_ID);";
                        break;
                    }
                case AdmissionSQLCommands.IsStudentExists:
                    {
                        query = @"SELECT 
                                STUDENT_ID,
                                PROGRAM_ID AS PROGRAMME_ID,
                                BATCH AS BATCH_ID,
                                CLASS_ID,ROLL_NO
                            FROM
                                ?TODB.STU_PERSONAL_INFO
                            WHERE
                                ROLL_NO =?ROLL_NO;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateStuPersonal:
                    {
                        query = @"UPDATE ?TODB.STU_PERSONAL_INFO SET CLASS_ID=?CLASS WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateStuClass:
                    {
                        query = @"UPDATE ?TODB.STU_CLASS SET CLASS_ID=?CLASS WHERE STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentInfo:
                    {
                        query = @"SELECT 
                                      ?FIELDS
                                FROM
                                    ADM_RECEIVE_APPLICATION AS AR
                                        LEFT JOIN
                                    ADM_ISSUED_APPLICATIONS AS AI ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                        LEFT JOIN
                                    CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        LEFT JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                        LEFT JOIN
                                    SUP_SHIFT AS SH ON SH.SHIFT_ID = CPG.SHIFT
                                        LEFT JOIN
                                    SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                                        LEFT JOIN
                                    GROUP_COMMUNITY AS GC ON GC.COMMUNITY_ID = AR.CASTE_ID
                                        LEFT JOIN
                                    SUP_COMMUNITY AS SC ON SC.COMMUNITYID = GC.COMMUNITY_ID
                                        LEFT JOIN
                                    SUP_ADM_STATUS AS SS ON SS.STATUS_ID = AI.STATUS
                                        LEFT JOIN
                                    SUP_RELIGION AS SR ON SR.RELIGIONID = AR.RELIGION_ID
                                        LEFT JOIN
                                    SUP_GENDER AS SG ON SG.GENDER_ID = AR.GENDER
                                        LEFT JOIN
                                    CP_CLASSES AS CC ON CC.CLASS_ID = AR.CLASS
                                        LEFT JOIN
                                    SUP_MOTHER_TONGUE AS SM ON SM.MOTHER_TONGUE_ID = AR.MOTHER_TONGUE
                                        LEFT JOIN
                                    SUP_OCCUPATION AS SO ON SO.OCCUPATION_ID = AR.OCCUPATION
                                        LEFT JOIN
                                    SUP_STATE AS S ON S.STATE_ID = AR.CSTATE
                                        LEFT JOIN
                                    SUP_COUNTRY AS C ON C.COUNTRY_ID = AR.CCOUNTRY
                                        LEFT JOIN
                                    SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID = AR.BLOOD_GROUP
                                        LEFT JOIN
                                    SUP_APPLICATION_TYPE AS SA ON SA.APPLICATION_TYPE_ID = AR.APPLICATION_TYPE_ID
                                        LEFT JOIN
                                    SUP_UNIVERSITY AS SU ON SU.UNIVERSITY_ID = AR.UNIVERSITY
                                     LEFT JOIN
                                    SUP_EDUCATION_BOARD AS SEB ON SEB.BOARD_ID = AR.EDUCATION_BOARD_ID
                                    WHERE CPG.PROGRAMME_GROUP_ID IN(?PROGRAMME_GROUP_ID);";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmittedStudent:
                    {
                        query = @"SELECT 
                                AI.ISSUED_ID,
                                CONCAT(AI.APPLICATION_NO,
                                        LPAD(AI.ISSUE_NO,4,'0'),
                                        '-',
                                        IFNULL(AR.ROLL_NO, ''),
                                        '-(',
                                        AR.FIRST_NAME,
                                        ')') AS APPLICATION_NO
                            FROM
                                ADM_SELECTION_PROCESS_2019 AS SP
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON SP.RECEIVE_ID = AR.RECEIVE_ID
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                            WHERE
                                AI.STATUS = 5 AND AI.IS_DELETED != 1
                                    AND AR.IS_DELETED != 1
                                    AND (SP.IS_DELETED != 1
                                    AND SP.IS_CANCELED != 1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentByApplicationNo:
                    {
                        query = @"SELECT 
                                        AR.FIRST_NAME,
                                        DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                        CP.PROGRAMME_NAME,
                                        AR.SMS_MOBILE_NO,
                                        BG.BLOOD_GROUP_NAME,
                                        AR.CDOORDETAIL,
                                        AR.CSTREET,
                                        AR.CVILLAGE_AREA,
                                        AR.CTALUK_CITY,
                                        AR.CDISTRICT,
                                        AR.CPINCODE,
                                        AR.PHOTO_PATH AS IMAGE_PATH,
                                        AR.AADHAR_NUMBER,
                                        AR.BLOOD_GROUP
                                    FROM
                                        ADM_ISSUED_APPLICATIONS AS AI
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                        LEFT JOIN SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID=AR.BLOOD_GROUP
                                    WHERE
                                        AI.ISSUED_ID = ?ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelSelectionIdByReceiveIdandProgrammeId:
                    {
                        query = @"SELECT 
                                    HOSTEL_SELECTION_ID, PROGRAMME_GROUP_ID, RECEIVE_ID
                                FROM
                                    ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR
                                WHERE
                                    RECEIVE_ID = ?RECEIVE_ID
                                        AND PROGRAMME_GROUP_ID = ?PROGRAMME_ID
                                        AND IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateHostelSelectionBySelectionId:
                    {
                        query = @"UPDATE ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR SET STATUS=?STATUS WHERE HOSTEL_SELECTION_ID=?HOSTEL_SELECTION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateVerified:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                SET 
                                    FIRST_NAME =?FIRST_NAME,
                                    BLOOD_GROUP = ?BLOOD_GROUP_NAME,
                                    DATE_OF_BIRTH =?DATE_OF_BIRTH,
                                    AADHAR_NUMBER =?AADHAR_NUMBER,
                                    CDOORDETAIL=?CDOORDETAIL,
                                    CSTREET =?CSTREET,
                                    CTALUK_CITY =?CTALUK_CITY,
                                    CVILLAGE_AREA =?CVILLAGE_AREA,
                                    CDISTRICT =?CDISTRICT,
                                    CPINCODE =?CPINCODE,
                                    SMS_MOBILE_NO=?SMS_MOBILE_NO,
                                    ID_VERIFIED=1
                                WHERE
                                    RECEIVE_ID =?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStatusWiseStudent:
                    {
                        query = @"SELECT 
                            UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                            DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                            CP.PROGRAMME_NAME,
                            AR.SMS_MOBILE_NO,
                            BG.BLOOD_GROUP_NAME,
                            AR.CDOORDETAIL,
                            UPPER(CONCAT(AR.CSTREET,' (ST)')) AS CSTREET,
                            UPPER(CONCAT(AR.CVILLAGE_AREA,' (AREA)')) AS CVILLAGE_AREA,
	                        UPPER(CONCAT(AR.CTALUK_CITY,' (TK)')) AS CTALUK_CITY,
                            UPPER(CONCAT(AR.CDISTRICT,' (DT)')) AS CDISTRICT,
	                        AR.CPINCODE,
                            AR.PHOTO_PATH AS IMAGE_PATH,
                            AR.AADHAR_NUMBER,AR.ROLL_NO
                        FROM
                            ADM_ISSUED_APPLICATIONS AS AI
                                INNER JOIN
                            ADM_RECEIVE_APPLICATION AS AR ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                INNER JOIN
                            CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                INNER JOIN
                            CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                INNER JOIN
                            SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID = AR.BLOOD_GROUP
                        WHERE
                            AI.STATUS = 5 AND AR.ID_VERIFIED = ?STATUS
                                AND AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID) ORDER BY AR.ROLL_NO;";
                        break;
                    }
                case AdmissionSQLCommands.FetchIDVerifiedStudent:
                    {
                        query = @"SELECT 
                                    AR.ROLL_NO,
                                    AR.ACCOUNT_NO,
                                    AR.FIRST_NAME,
                                    DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                    CP.PROGRAMME_NAME,
                                    AR.SMS_MOBILE_NO,
                                    AR.EMAIL_ID,
                                    G.GENDER_NAME,
                                    MS.MARITAL_STATUS_NAME,
                                    AR.FATHER_NAME,
                                    AR.MOTHER_NAME,
                                    BG.BLOOD_GROUP_NAME,
                                    CONCAT(AR.CDOORDETAIL,
                                            ',',
                                            AR.CSTREET,
                                            ',',
                                            AR.CVILLAGE_AREA,
                                            ',',
                                            AR.CTALUK_CITY,
                                            ',',
                                            AR.CDISTRICT,
                                            '.') AS CADDRESS,
                                    AR.CDISTRICT,
                                    SS.STATE AS CSTATE,
                                    AR.CPINCODE,
                                    CONCAT(AR.PDOORDETAIL,
                                            ',',
                                            AR.PSTREET,
                                            ',',
                                            AR.PVILLAGE_AREA,
                                            ',',
                                            AR.PTALUK_CITY,
                                            ',',
                                            AR.PDISTRICT,
                                            '.') AS PADDRESS,
                                    AR.PDISTRICT,
                                    SS1.STATE AS PSTATE,
                                    AR.PPINCODE,
                                    AR.FATHER_NAME AS NOMINEE,
                                    CONCAT(CC.COLLEGE_NAME,
                                            ',',
                                            CC.ADDRESS_ONE,
                                            ',',
                                            CC.ADDRESS_TWO,
                                            '.') AS NOMINEE_ADDRESS,
                                    CC.DISTRICT AS NOMINEE_DISTRICT,
                                    CC.PINCODE AS NOMINEE_PINCODE,
                                    'Tamil Nadu' AS NOMINEE_STATE,
                                    AR.FATHER_AGE,
                                    AR.IMAGE_PATH,
                                    IF(AR.AADHAR_NUMBER != '',
                                        'AADHAR',
                                        IF(AR.PASSPORT_NUMBER != '',
                                            'PASSPORT',
                                            '')) AS ID_TYPE,
                                    IFNULL(AR.AADHAR_NUMBER,
                                            IFNULL(AR.PASSPORT_NUMBER, '')) AS NUMBER,
                                    CC.MANAGEMENT_NAME,
                                    'STUDENT' AS DESIGNATION
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS AI
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                        INNER JOIN
                                    ?TODB.STU_PERSONAL_INFO AS SP ON SP.RECEIVE_ID = AR.RECEIVE_ID
                                        AND SP.IS_LEFT != 1 AND SP.ACADEMIC_YEAR=?AC_YEAR
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID = AR.BLOOD_GROUP
                                        INNER JOIN
                                    SUP_GENDER AS G ON G.GENDER_ID = AR.GENDER
                                        LEFT JOIN
                                    SUP_MARRITAL_STATUS AS MS ON MS.MARITAL_STAUS_ID = AR.MARITAL_STATUS_ID
                                        LEFT JOIN
                                    SUP_STATE AS SS ON SS.STATE_ID = AR.CSTATE
                                        LEFT JOIN
                                    SUP_STATE AS SS1 ON SS1.STATE_ID = AR.PSTATE
                                        INNER JOIN
                                    COLLEGE_DETAILS AS CC
                                WHERE
                                    AI.STATUS = 5 AND AI.IS_DELETED != 1
                                        AND AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID);";
                        break;
                        //query = @"SELECT 
                        //        AR.ROLL_NO,
                        //        AR.ACCOUNT_NO,
                        //        AR.FIRST_NAME,
                        //        DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                        //        CP.PROGRAMME_NAME,
                        //        AR.SMS_MOBILE_NO,
                        //        AR.EMAIL_ID,
                        //        G.GENDER_NAME,
                        //        MS.MARITAL_STATUS_NAME,
                        //        AR.FATHER_NAME,
                        //        AR.MOTHER_NAME,
                        //        BG.BLOOD_GROUP_NAME,
                        //        CONCAT(AR.CDOORDETAIL,
                        //                ',',
                        //                AR.CSTREET,
                        //                ',',
                        //                AR.CVILLAGE_AREA,
                        //                ',',
                        //                AR.CTALUK_CITY,
                        //                ',',
                        //                AR.CDISTRICT,
                        //                '.') AS CADDRESS,
                        //        AR.CDISTRICT,
                        //        SS.STATE AS CSTATE,
                        //        AR.CPINCODE,
                        //        CONCAT(AR.PDOORDETAIL,
                        //                ',',
                        //                AR.PSTREET,
                        //                ',',
                        //                AR.PVILLAGE_AREA,
                        //                ',',
                        //                AR.PTALUK_CITY,
                        //                ',',
                        //                AR.PDISTRICT,
                        //                '.') AS PADDRESS,
                        //        AR.PDISTRICT,
                        //        SS1.STATE AS PSTATE,
                        //        AR.PPINCODE,
                        //        AR.FATHER_NAME AS NOMINEE,
                        //        CONCAT(CC.COLLEGE_NAME,
                        //                ',',
                        //                CC.ADDRESS_ONE,
                        //                ',',
                        //                CC.ADDRESS_TWO,
                        //                ',',
                        //                CC.CITY,
                        //                ',',
                        //                CC.DISTRICT,
                        //                '.') AS NOMINEE_ADDRESS,
                        //        CC.DISTRICT AS NOMINEE_DISTRICT,
                        //        CC.PINCODE AS NOMINEE_PINCODE,
                        //        AR.FATHER_AGE,
                        //        AR.IMAGE_PATH,
                        //        IF(AR.AADHAR_NUMBER != '',
                        //            'AADHAR',
                        //            IF(AR.PASSPORT_NUMBER != '',
                        //                'PASSPORT',
                        //                '')) AS ID_TYPE,
                        //        IFNULL(AR.AADHAR_NUMBER,
                        //                IFNULL(AR.PASSPORT_NUMBER, '')) AS NUMBER,
                        //        CC.MANAGEMENT_NAME,
                        //        'STUDENT' AS DESIGNATION
                        //    FROM
                        //        ADM_ISSUED_APPLICATIONS AS AI
                        //            INNER JOIN
                        //        ADM_RECEIVE_APPLICATION AS AR ON AI.RECEIVE_ID = AR.RECEIVE_ID
                        //            INNER JOIN
                        //        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                        //            INNER JOIN
                        //        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                        //            INNER JOIN
                        //        SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID = AR.BLOOD_GROUP
                        //            INNER JOIN
                        //        SUP_GENDER AS G ON G.GENDER_ID = AR.GENDER
                        //            LEFT JOIN
                        //        SUP_MARRITAL_STATUS AS MS ON MS.MARITAL_STAUS_ID = AR.MARITAL_STATUS_ID
                        //            LEFT JOIN
                        //        SUP_STATE AS SS ON SS.STATE_ID = AR.CSTATE
                        //            LEFT JOIN
                        //        SUP_STATE AS SS1 ON SS1.STATE_ID = AR.PSTATE
                        //            INNER JOIN
                        //        COLLEGE_DETAILS AS CC
                        //    WHERE
                        //        AI.STATUS = 5 AND AI.IS_DELETED!=1
                        //            AND AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID);";
                        //break;
                    }
                case AdmissionSQLCommands.FetchRollnoByIssuedId:
                    {
                        query = @"SELECT 
                                AR.RECEIVE_ID,AR.ROLL_NO
                            FROM
                                ADM_ISSUED_APPLICATIONS AS AI
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AI.RECEIVE_ID = AR.RECEIVE_ID
                            WHERE
                                ISSUED_ID =?ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAccountWisestatus:
                    {
                        query = @"SELECT 
                                AR.ROLL_NO,
                                AR.FIRST_NAME,
                                DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                CP.PROGRAMME_NAME,
                                AR.SMS_MOBILE_NO,
                                BG.BLOOD_GROUP_NAME,
                                AR.CDOORDETAIL,
                                AR.CSTREET,
                                AR.CVILLAGE_AREA,
                                AR.CTALUK_CITY,
                                AR.CDISTRICT,
                                AR.CPINCODE,
                                AR.PHOTO_PATH AS IMAGE_PATH,
                                AR.AADHAR_NUMBER,
                                AR.ACCOUNT_NO
                            FROM
                                ADM_ISSUED_APPLICATIONS AS AI
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID = AR.BLOOD_GROUP
                            WHERE
                                AI.STATUS = 5 AND AR.ACCOUNT_NO IS NOT NULL;";
                        break;
                    }
                case AdmissionSQLCommands.FetchNotcreatedac:
                    {
                        query = @"SELECT 
                                    AR.ROLL_NO,
                                    AR.FIRST_NAME,
                                    DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                                    CP.PROGRAMME_NAME,
                                    AR.SMS_MOBILE_NO,
                                    BG.BLOOD_GROUP_NAME,
                                    AR.CDOORDETAIL,
                                    AR.CSTREET,
                                    AR.CVILLAGE_AREA,
                                    AR.CTALUK_CITY,
                                    AR.CDISTRICT,
                                    AR.CPINCODE,
                                    AR.PHOTO_PATH AS IMAGE_PATH,
                                    AR.AADHAR_NUMBER,
                                    AR.ACCOUNT_NO
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS AI
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                        INNER JOIN
                                    SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID = AR.BLOOD_GROUP
                                WHERE
                                    AI.STATUS = 5 AND AR.ACCOUNT_NO IS NULL;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateAccountno:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        ACCOUNT_NO =?ACCOUNT_NO,
                                        IS_AC_CREATED = 1
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.Updateimageinadmreceiveapplication:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                SET 
                                    PHOTO_PATH =?PHOTO_PATH
                                WHERE
                                    RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmittedStudentForImagecorrection:
                    {
                        query = @"SELECT 
                                AI.ISSUED_ID,
                                AR.ROLL_NO,
                                AR.RECEIVE_ID,AR.PHOTO_PATH
    
                            FROM
                                ADM_SELECTION_PROCESS_2019 AS SP
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON SP.RECEIVE_ID = AR.RECEIVE_ID
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                            WHERE
                                AI.STATUS = 5 AND AI.IS_DELETED != 1
                                    AND AR.IS_DELETED != 1
                                    AND (SP.IS_DELETED != 1
                                    AND SP.IS_CANCELED != 1);";
                        break;
                    }
                case AdmissionSQLCommands.Updatestupersonalinfo:
                    {
                        query = @"UPDATE ?TODB.STU_PERSONAL_INFO 
                                SET 
                                    FIRST_NAME =?FIRST_NAME,
                                    AADHAR_NUMBER =?AADHAR_NUMBER,
                                    DATE_OF_BIRTH =?DATE_OF_BIRTH, 
                                    BLOOD_GROUP=?BLOOD_GROUP_NAME,
                                    FR_MOBILE=?SMS_MOBILE_NO,
                                    STU_MOBILENO=?SMS_MOBILE_NO
                                WHERE
                                    RECEIVE_ID =?RECEIVE_ID AND ACADEMIC_YEAR=?AC_YEAR ";
                        break;
                    }
                case AdmissionSQLCommands.InsertUseraccount:
                    {
                        query = @"INSERT INTO ?TODB.USER_ACCOUNT
                                (USERNAME,PASSWORD,ROLE,NAME,EMAIL,MOBILE,USER_ID,USER_TYPE)
                                VALUES(?ROLL_NO,SHA2(?ROLL_NO,256),5,?FIRST_NAME,?STU_EMAILID,?STU_MOBILENO,?STUDENT_ID,5);";
                        break;
                    }

                case AdmissionSQLCommands.InsertUserRolesByAcademicYear:
                    {
                        query = @"INSERT INTO ?TODB.USER_ROLES_BY_ACADEMIC_YEAR
                                (USER_ID,ROLE_ID,USER_TYPE,ACADEMIC_YEAR)
                                VALUES(?STUDENT_ID,5,5,?AC_YEAR);";
                        break;
                    }

                case AdmissionSQLCommands.UseraccountExisit:
                    {
                        query = @"SELECT 
                                USER_ID AS STUDENT_ID
                            FROM
                                ?TODB.USER_ACCOUNT
                            WHERE
                                USERNAME =?ROLL_NO AND USER_ID =?STUDENT_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UserRolesByAcademicYearExist:
                    {
                        query = @"SELECT 
                                USER_ID AS STUDENT_ID
                            FROM
                                ?TODB.USER_ROLES_BY_ACADEMIC_YEAR
                            WHERE
                                USER_ID =?STUDENT_ID AND USER_TYPE =5 AND ROLE_ID=5 AND ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.FetchRollnoNotCreatedStudent:
                    {
                        query = @"SELECT 
                                        SP.RECEIVE_ID,
                                        SP.PROGRAMME_ID,
                                        ADR.FIRST_NAME,
                                        ADR.CLASS,
                                        SP.APPLICATION_NO,
                                        DATE_FORMAT(ADR.DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH,
                                        CC.CLASS_NAME,
                                        ADR.ROLL_NO,
                                        ADI.STATUS,
                                        SS.STATUS_NAME,CP.PROGRAMME_NAME
                                    FROM
                                        ADM_SELECTION_PROCESS_?AC_YEAR AS SP
                                            INNER JOIN
                                        ADM_ISSUED_APPLICATIONS AS ADI ON SP.RECEIVE_ID = ADI.RECEIVE_ID
                                            AND ADI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                            AND ADI.STATUS = 5
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS ADR ON ADR.RECEIVE_ID = ADI.RECEIVE_ID
                                            LEFT JOIN
                                        CP_CLASSES AS CC ON CC.CLASS_ID = ADR.CLASS
                                            INNER JOIN
                                        SUP_ADM_STATUS AS SS ON SS.STATUS_ID = ADI.STATUS
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                    WHERE
                                        SP.PROGRAMME_ID IN (?PROGRAMME_ID)
                                            AND ADR.ROLL_NO IS NULL
                                            AND (SP.IS_DELETED != 1
                                            AND SP.IS_CANCELED != 1)
                                    ORDER BY ADR.FIRST_NAME;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAllstudent:
                    {
                        query = @"SELECT 
                            UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                            DATE_FORMAT(AR.DATE_OF_BIRTH, '%d/%m/%Y') AS DOB,
                            CP.PROGRAMME_NAME,
                            AR.SMS_MOBILE_NO,
                            BG.BLOOD_GROUP_NAME,
                            AR.CDOORDETAIL,
                            UPPER(CONCAT(AR.CSTREET,' (ST)')) AS CSTREET,
                            UPPER(CONCAT(AR.CVILLAGE_AREA,' (AREA)')) AS CVILLAGE_AREA,
	                        UPPER(CONCAT(AR.CTALUK_CITY,' (TK)')) AS CTALUK_CITY,
                            UPPER(CONCAT(AR.CDISTRICT,' (DT)')) AS CDISTRICT,
	                        AR.CPINCODE,
                            AR.PHOTO_PATH AS IMAGE_PATH,
                            AR.AADHAR_NUMBER,AR.ROLL_NO
                        FROM
                            ADM_ISSUED_APPLICATIONS AS AI
                                INNER JOIN
                            ADM_RECEIVE_APPLICATION AS AR ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                INNER JOIN
                            CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                INNER JOIN
                            CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                INNER JOIN
                            SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID = AR.BLOOD_GROUP
                        WHERE
                            AI.STATUS = 5 
                                AND AI.PROGRAMME_GROUP_ID IN (?PROGRAMME_ID) ORDER BY AR.ROLL_NO;";
                        break;
                    }
                case AdmissionSQLCommands.FetchApplicationtype:
                    {
                        query = @"SELECT 
                                    APPLICATION_TYPE_ID, APPLICATION_TYPE
                                FROM
                                    SUP_APPLICATION_TYPE where IS_DELETED!=1";
                        break;
                    }
                case AdmissionSQLCommands.FetchSubmittedStudentForEntranceExam:
                    {
                        query = @"SELECT 
                                R.RECEIVE_ID,
                                I.ISSUED_ID,
                                R.FIRST_NAME,
                                CONCAT(I.APPLICATION_NO, LPAD(I.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                R.SMS_MOBILE_NO,
                                I.ENTRANCE_MARK
                            FROM
                                ADM_RECEIVE_APPLICATION AS R
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS I ON I.RECEIVE_ID = R.RECEIVE_ID
                                    AND I.PROGRAMME_GROUP_ID = ?PROGRAMME_GROUP_ID
                                    AND I.`STATUS` = ?STATUS
                            ORDER BY ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateEntranceExamMarks:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS SET ENTRANCE_MARK=?ENTRANCE_MARK WHERE ISSUED_ID=?ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchSelectionSettings:
                    {
                        query = @"SELECT 
                                    SELECTION_SETTING_ID,
                                    SELECTION_CYCLE_ID,
                                    INTERVAL_DAY,
                                    IS_AUTO_CANCEL,
                                    ACADEMIC_YEAR,
                                    IS_ACTIVE
                                FROM
                                    ADM_SELECTION_SETTING
                                WHERE
                                    ACADEMIC_YEAR =?AC_YEAR AND IS_DELETED != 1 AND IS_ACTIVE=1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateStatusofDateExpiredSelectedApplicant:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS 
                                    SET 
                                        STATUS =?STATUS
                                    WHERE
                                        ISSUED_ID = ?ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateAdmissionCancelinSelectionprocess:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR 
                                    SET 
                                        IS_CANCELED = 1
                                    WHERE
                                        SELECTION_ID =?SELECTION_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchDateExpiredSelectedApplicant:
                    {
                        query = @"SELECT 
                                ASP.RECEIVE_ID,
                                ARP.FIRST_NAME,
                                DATE_FORMAT(ARP.DATE_OF_BIRTH, '%Y-%m-%d') AS DATE_OF_BIRTH,
                                ASP.APPLICATION_NO,
                                ASP.PROGRAMME_ID,
                                CP.PROGRAMME_NAME,
                                DATE_FORMAT(ASP.SELECTION_DATE, '%Y-%m-%d') AS SELECTION_DATE,
                                SC.SELECTION_CYCLE_ID,
                                SC.SELECTION_CYCLE,
                                AIP.STATUS,
                                SAS.STATUS_NAME,
                                ASP.SELECTION_ID,
                                AIP.ISSUED_ID
                             FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS ASP
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AIP ON ASP.RECEIVE_ID = AIP.RECEIVE_ID
                                    AND ASP.PROGRAMME_ID = AIP.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS ARP ON ARP.RECEIVE_ID = ASP.RECEIVE_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = ASP.PROGRAMME_ID
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID = AIP.STATUS
                                    INNER JOIN
                                SUP_SELECTION_CYCLE AS SC ON SC.SELECTION_CYCLE_ID = ASP.SELECTION_CYCLE
                            WHERE
                                ASP.IS_DELETED != 1
                                    AND DATEDIFF(CURDATE(), ASP.SELECTION_DATE) > ?INTERVAL_DAY
                                    AND AIP.STATUS < ?STATUS;";
                        break;
                    }
                case AdmissionSQLCommands.UpdatePhysicalyChallangedProof:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        PHYSICALY_CHALLENGED_PROOF = ?PHYSICALY_CHALLENGED_PROOF
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID;";
                        break;

                    }
                case AdmissionSQLCommands.UpdateNRIProof:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        NRI_PROOF = ?NRI_PROOF
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID;";
                        break;

                    }
                case AdmissionSQLCommands.UpdateCommunityProof:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        COMMUNITY_PROOF = ?COMMUNITY_PROOF
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID;";
                        break;

                    }
                case AdmissionSQLCommands.UpdateExservicemanProof:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        EX_SERVICEMAN_PROOF = ?EX_SERVICEMAN_PROOF
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID;";
                        break;

                    }
                case AdmissionSQLCommands.UpdateSingleParentProof:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        SINGLE_PARENT_PROOF = ?SINGLE_PARENT_PROOF
                                    WHERE
                                        RECEIVE_ID =?RECEIVE_ID;";
                        break;

                    }
                case AdmissionSQLCommands.FetchAdmissionHoldedApplicantByCycleId:
                    {
                        query = @"SELECT 
                                AR.RECEIVE_ID,
                                AI.ISSUED_ID,
                                CONCAT(AI.APPLICATION_NO, LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                AR.FIRST_NAME,
                                AR.HSTOTAL,
                                AR.SMS_MOBILE_NO,
                                AI.PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                        '(',
                                        
                                        PM.PROGRAMME_MODE,
                                        ')') AS PROGRAMME_NAME
                            FROM
                                ADM_SELECTION_PROCESS_2020 AS SP
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON SP.RECEIVE_ID = AI.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = SP.PROGRAMME_ID
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = CPG.SHIFT
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                            WHERE
                                AI.STATUS = ?STATUS AND AI.IS_DELETED != 1
                                    AND SP.SELECTION_CYCLE = ?SELECTION_CYCLE
                                    AND SP.IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAppliedApplicationInfoForTransferRequest:
                    {
                        query = @"SELECT 
                                CONCAT(IA.APPLICATION_NO,LPAD(IA.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                IA.ISSUED_ID,
                                IA.RECEIVE_ID,
                                IA.PROGRAMME_GROUP_ID,
                                CONCAT(P.PROGRAMME_NAME,
                                        '-',
                                        PM.PROGRAMME_MODE,
                                        ' (',
                                        SH.DESCRIPTION,
                                        '-',
                                        SH.TIME,
                                        ')') AS PROGRAMME_NAME,
                                S.STATUS_NAME AS STATUS
                            FROM
                                ADM_ISSUED_APPLICATIONS AS IA
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = IA.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_SHIFT AS SH ON SH.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_ADM_STATUS AS S ON S.STATUS_ID = IA.STATUS
	
                            WHERE
                                IA.RECEIVE_ID = ?RECEIVE_ID
                                    AND IA.ACADEMIC_YEAR = ?AC_YEAR
                                    AND IS_PAID = 1;";
                        break;
                    }
                case AdmissionSQLCommands.InsertApplicantTransferRequest:
                    {
                        query = @"INSERT INTO ADM_TRANSFER_REQUEST (RECEIVE_ID, ISSUED_ID, PROGRAMME_FROM, PROGRAMME_TO, ISSUED_STATUS, REQUEST_CONTENT, ACADEMIC_YEAR) VALUES
                                (?RECEIVE_ID, ?ISSUED_ID, ?PROGRAMME_FROM, ?PROGRAMME_TO, ?ISSUED_STATUS, ?REQUEST_CONTENT, ?ACADEMIC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.DeleteApplicantTransferRequest:
                    {
                        query = @"UPDATE ADM_TRANSFER_REQUEST SET IS_DELETED=1 WHERE TRANSFER_REQUEST_ID=?TRANSFER_REQUEST_ID WHERE IS_CANCELLED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchApprovedTransferForTransfer:
                    {
                        query = @"SELECT 
                                    TA.TRANSFER_REQUEST_ID,
                                    TA.RECEIVE_ID,
                                    AR.FIRST_NAME,
                                    AR.EMAIL_ID,
                                    AR.SMS_MOBILE_NO,
                                    TA.ISSUED_ID,
                                    TA.APPLICATION_NO,
                                    TA.PROGRAMME_FROM,
                                    TA.PROGRAMME_TO,
                                    TA.APPROVED_BY,
                                    TA.ISSUED_STATUS,
                                    ST.STATUS_NAME,
                                    TA.TRANSFER_STATUS,
                                    TA.APPROVED_DATE,
                                    TA.IS_APPROVED,
                                    TA.IS_REFUND
                                FROM
                                    ADM_TRANSFER_APPROVAL AS TA
                                        INNER JOIN
                                    ADM_TRANSFER_REQUEST AS TR ON TA.TRANSFER_REQUEST_ID = TR.TRANSFER_REQUEST_ID
                                        AND TR.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = TA.RECEIVE_ID
                                        INNER JOIN
                                    SUP_ADM_STATUS AS ST ON ST.STATUS_ID = TA.ISSUED_STATUS
                                WHERE
                                    TA.IS_APPROVED = 1 AND TA.ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }

                case AdmissionSQLCommands.FetchAllApplicantTransForRequest:
                    {
                        query = @"SELECT 
                                    TR.TRANSFER_REQUEST_ID,
                                    TR.RECEIVE_ID,
                                    UPPER(CONCAT(IFNULL(AR.FIRST_NAME,''))) AS FIRST_NAME,
                                    AR.EMAIL_ID,
                                    AR.SMS_MOBILE_NO,
                                    AR.HSC_NO
    
                                FROM
                                    ADM_TRANSFER_REQUEST AS TR
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS AI ON AI.ISSUED_ID = TR.ISSUED_ID
                                        AND AI.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                        AND AR.IS_DELETED != 1
                                WHERE
                                    TR.IS_DELETED != 1
                                        AND TR.TRANSFER_STATUS = '0'
                                        AND TR.ACADEMIC_YEAR=?AC_YEAR GROUP BY TR.RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.SaveAdmTransferApproved:
                    {
                        query = @"INSERT INTO 
                                    adm_transfer_approval
                                    (TRANSFER_REQUEST_ID,
                                    RECEIVE_ID,
                                    ISSUED_ID,
                                    APPLICATION_NO,
                                    PROGRAMME_FROM,
                                    PROGRAMME_TO,
                                    APPROVED_BY,
                                    ISSUED_STATUS,
                                    TRANSFER_STATUS,
                                    APPROVED_CONTENT,
                                    ACADEMIC_YEAR)
                                    VALUES
                                    (
                                    ?TRANSFER_REQUEST_ID,
                                    ?RECEIVE_ID,
                                    ?ISSUED_ID,
                                    ?APPLICATION_NO,
                                    ?PROGRAMME_FROM,
                                    ?PROGRAMME_TO,
                                    ?APPROVED_BY,
                                    ?ISSUED_STATUS,
                                    ?TRANSFER_STATUS,
                                    ?APPROVED_CONTENT,
                                    ?AC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.UpdateAdmTransferRequetedForApproved:
                    {
                        query = @"UPDATE 
                                    ADM_TRANSFER_REQUEST
                                    SET 
                                    TRANSFER_STATUS=?TRANSFER_STATUS 
                                    WHERE 
                                    TRANSFER_REQUEST_ID=?TRANSFER_REQUEST_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAdmTransferRequestByTransferRequestId:
                    {
                        query = @"SELECT 
                                    AR.TRANSFER_REQUEST_ID,
                                    AR.RECEIVE_ID,
                                    AR.ISSUED_ID,
                                    AR.PROGRAMME_FROM,
                                    AR.PROGRAMME_TO,
                                    AR.ISSUED_STATUS,
                                    AR.REQUEST_DATE,
                                    AR.REQUEST_CONTENT,
                                    AR.TRANSFER_STATUS,
                                    AI.APPLICATION_NO,
                                    R.SMS_MOBILE_NO,
                                    CP.PROGRAMME_NAME AS PROGRAMME_FROM_NAME,
                                    CP1.PROGRAMME_NAME AS PROGRAMME_TO_NAME,
                                    UPPER(CONCAT(IFNULL(R.FIRST_NAME,''))) AS FIRST_NAME
                                FROM
                                    ADM_TRANSFER_REQUEST AS AR
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS AI ON AI.ISSUED_ID = AR.ISSUED_ID
                                        AND AI.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS R ON R.RECEIVE_ID = AI.RECEIVE_ID
                                        AND R.IS_DELETED != 1
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPP ON CPP.PROGRAMME_GROUP_ID = AR.PROGRAMME_FROM
                                        AND CPP.IS_DELETED != 1
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPP.PROGRAMME_ID
                                        AND CP.IS_DELETED != 1
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPP1 ON CPP1.PROGRAMME_GROUP_ID = AR.PROGRAMME_TO
                                        AND CPP1.IS_DELETED != 1
                                        INNER JOIN
                                    CP_PROGRAMME AS CP1 ON CP1.PROGRAMME_ID = CPP1.PROGRAMME_ID
                                        AND CP1.IS_DELETED != 1
                                WHERE
                                    AR.TRANSFER_REQUEST_ID =?TRANSFER_REQUEST_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchAllApplicantTransForRequestByReceiveId:
                    {
                        query = @"SELECT 
                                    TR.TRANSFER_REQUEST_ID,
                                    TR.RECEIVE_ID,
                                    AI.APPLICATION_NO,
                                    CP.PROGRAMME_NAME AS PROGRAMME_FROM,
                                    CP1.PROGRAMME_NAME AS PROGRAMME_TO,
                                    DATE_FORMAT(TR.REQUEST_DATE,'%d/%m/%Y') AS REQUEST_DATE,
                                    TR.REQUEST_CONTENT
                                FROM
                                    ADM_TRANSFER_REQUEST AS TR
                                        INNER JOIN
                                    ADM_ISSUED_APPLICATIONS AS AI ON AI.ISSUED_ID = TR.ISSUED_ID
                                        AND AI.IS_DELETED != 1
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                        AND AR.IS_DELETED != 1
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPP ON CPP.PROGRAMME_GROUP_ID = TR.PROGRAMME_FROM
                                        AND CPP.IS_DELETED != 1
                                        INNER JOIN
                                    CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPP.PROGRAMME_ID
                                        AND CP.IS_DELETED != 1
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS CPP1 ON CPP1.PROGRAMME_GROUP_ID = TR.PROGRAMME_TO
                                        AND CPP1.IS_DELETED != 1
                                        INNER JOIN
                                    CP_PROGRAMME AS CP1 ON CP1.PROGRAMME_ID = CPP1.PROGRAMME_ID
                                        AND CP1.IS_DELETED != 1
                                WHERE
                                    TR.IS_DELETED != 1
                                       AND TR.TRANSFER_STATUS='0'
                                        AND TR.RECEIVE_ID =?RECEIVE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchApplicantforReminderByStatus:
                    {
                        query = @"SELECT 
                                ASP.RECEIVE_ID,
                                ARP.FIRST_NAME,
                                DATE_FORMAT(ARP.DATE_OF_BIRTH, '%Y-%m-%d') AS DATE_OF_BIRTH,
                                ASP.APPLICATION_NO,
                                ASP.PROGRAMME_ID,
                                CP.PROGRAMME_NAME,
                                DATE_FORMAT(ASP.SELECTION_DATE, '%Y-%m-%d') AS SELECTION_DATE,
                                SC.SELECTION_CYCLE_ID,
                                SC.SELECTION_CYCLE,
                                AIP.STATUS,
                                SAS.STATUS_NAME,
                                ASP.SELECTION_ID,
                                AIP.ISSUED_ID,
                                ARP.SMS_MOBILE_NO
                            FROM
                                ADM_SELECTION_PROCESS_?AC_YEAR AS ASP
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AIP ON ASP.RECEIVE_ID = AIP.RECEIVE_ID
                                    AND ASP.PROGRAMME_ID = AIP.PROGRAMME_GROUP_ID
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS ARP ON ARP.RECEIVE_ID = ASP.RECEIVE_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS CPG ON CPG.PROGRAMME_GROUP_ID = ASP.PROGRAMME_ID
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_ADM_STATUS AS SAS ON SAS.STATUS_ID = AIP.STATUS
                                    INNER JOIN
                                SUP_SELECTION_CYCLE AS SC ON SC.SELECTION_CYCLE_ID = ASP.SELECTION_CYCLE
                            WHERE
                                ASP.IS_DELETED != 1
                                    AND DATE_FORMAT(DATE_ADD(ASP.SELECTION_DATE,
                                            INTERVAL ?INTERVAL_DAY DAY),
                                        '%Y-%m-%d') = CURDATE()
                                    AND AIP.STATUS = ?STATUS;";
                        break;
                    }
                case AdmissionSQLCommands.FetchTransferApprovalByTransferRequestId:
                    {
                        query = @"SELECT 
                                TRANSFER_APPROVAL_ID,
                                TRANSFER_REQUEST_ID,
                                RECEIVE_ID,
                                ISSUED_ID,
                                APPLICATION_NO,
                                PROGRAMME_FROM,
                                PROGRAMME_TO,
                                APPROVED_BY,
                                ISSUED_STATUS,
                                TRANSFER_STATUS,
                                APPROVED_DATE,
                                IS_APPROVED,
                                IS_REFUND
                            FROM
                                ADM_TRANSFER_APPROVAL
                            WHERE
                                TRANSFER_REQUEST_ID = ?TRANSFER_REQUEST_ID
                                    AND IS_DELETED != 1
                                    AND ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateStatusForTransfer:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS 
                                SET 
                                    STATUS=?STATUS_ID,
                                    TRANSFER_APPROVAL_ID=?TRANSFER_APPROVAL_ID
                                WHERE
                                    ISSUED_ID=?ISSUED_ID;";
                        break;
                    }
                case AdmissionSQLCommands.RevertProgrammeIdForTransfer:
                    {
                        query = @"UPDATE ADM_ISSUED_APPLICATIONS 
                                SET 
                                    PROGRAMME_GROUP_ID =?PROGRAMME_TO,                                    
                                    STATUS=?STATUS_ID,
                                    TRANSFER_APPROVAL_ID=?TRANSFER_APPROVAL_ID
                                WHERE
                                    ISSUED_ID=?ISSUE_ID";
                        break;
                    }
                case AdmissionSQLCommands.UpdateIsTransferedForTransfer:
                    {
                        query = @"UPDATE TRANSFER_REQUEST_ID 
                                SET 
                                    IS_TRANSFERED =1                                    
                                WHERE
                                    TRANSFER_APPROVAL_ID=?TRANSFER_APPROVAL_ID";
                        break;
                    }
                case AdmissionSQLCommands.FetchFeeTransactionForRefundByStudentId:
                    {
                        query = @"SELECT 
                                FT.TRANSACTION_ID,
                                FT.STUDENT_ID,
                                FT.FREQUENCY,
                                DATE_FORMAT(FT.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                FT.RECEIPT_NO,
                                FT.PAYMENT_MODE,
                                FT.COLLECTED,
                                FT.RAZORPAY_ID,
                                FT.IS_REFUND,
                                FT.REFUND_TYPE,
                                DATE_FORMAT(FT.REFUND_DATE, '%d/%m/%Y') AS REFUND_DATE,
                                P.ID,
                                P.ORDER_ID,
                                P.UDF6 as PROGRAMME_GROUP_ID
                            FROM
                                FEE_TRANSACTION AS FT
                                    INNER JOIN
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS P ON P.RAZORPAY_PAMENT_ID = FT.RAZORPAY_ID
                            WHERE
                                FT.STUDENT_ID = ?STUDENT_ID AND FREQUENCY = ?FREQUENCY AND FT.IS_REFUND!=1
                                    AND FT.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case AdmissionSQLCommands.FetchStudentPaidAmountForRefund:
                    {
                        query = @"SELECT     
                                SA.STUDENT_ID,   
                                SA.FREQUENCY_ID,
                                FT.TRANSACTION_ID,  
                                DATE_FORMAT(FT.PAYMENT_DATE,'%d/%m/%Y') as PAYMENT_DATE,
                                FT.RAZORPAY_ID,
                                SA.IS_REFUND,
                                SA.F_STUDENT_AC_ID,
                                SA.FEE_MAIN_HEAD_ID,
                                SA.FEE_STRUCTURE_ID,
                                SA.FEE_ROOT_ID,
                                MH.MAIN_HEAD,
                                H.HEAD_ID,
                                H.HEAD,
                                SA.DEBIT as AMOUNT,
                                B.PASSBOOK_NO,
                                CT.FEE_CATEGORY
                            FROM
                                FEE_STUDENT_ACCOUNT AS SA
                                    INNER JOIN
                                FEE_STRUCTURE AS FS ON FS.FEE_STRUCTURE_ID = SA.FEE_STRUCTURE_ID
                                    INNER JOIN
                                FEE_MAIN_HEADS AS FM ON FM.FEE_MAIN_HEAD_ID = FS.FEE_MAIN_HEAD_ID
                                    INNER JOIN
                                SUP_BANK_ACCOUNT AS B ON B.BANK_ACCOUNT_ID = FS.BANK_ACCOUNT_ID
                                    INNER JOIN
                                FEE_TRANSACTION AS FT ON FT.TRANSACTION_ID = SA.TRANSACTION_ID  AND FT.IS_REFUND!=1
                                    INNER JOIN
                                SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FM.MAIN_HEAD_ID
                                    INNER JOIN
                                SUP_HEAD AS H ON H.HEAD_ID = FM.HEAD_ID
                                    INNER JOIN
                                SUP_FEE_CATEGORY AS CT ON CT.FEE_CATEGORY_ID = FM.FEE_CATEGORY_ID
                            WHERE
                                SA.STUDENT_ID = ?STUDENT_ID
                                    AND SA.TRANSACTION_ID = ?TRANSACTION_ID
                            ORDER BY SA.F_STUDENT_AC_ID;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateNotInterestBySelectionId:
                    {
                        query = @"UPDATE ADM_SELECTION_PROCESS_?AC_YEAR SET VERIFIED_BY=?VERIFIED_BY,IS_DELETED=1, IS_NOT_INTEREST=1 WHERE SELECTION_ID=?SELECTION_ID AND (IS_DELETED!=1 AND IS_CANCELED!=1);";
                        break;
                    }
                case AdmissionSQLCommands.FetchApplicationFormByReceiveId:
                    {
                        query = @"SELECT 
	                                R.RECEIVE_ID,
                                    CONCAT(I.APPLICATION_NO,LPAD(I.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    UPPER(R.FIRST_NAME) AS FIRST_NAME,
                                    SMS_MOBILE_NO,
                                    PG.PROGRAMME_GROUP_ID,
                                    CONCAT(PROGRAMME_NAME,' - ',SPM.PROGRAMME_MODE) AS PROGRAMME_NAME,
                                    IFNULL(DATE_FORMAT(R.DATE_OF_BIRTH,'%d-%m-%Y'),' - ') AS DATE_OF_BIRTH,
                                    YEAR(curdate())-YEAR(DATE_OF_BIRTH) AS AGE,
                                    if(SC.COUNTRY_NAME='India','Indian','') as COUNTRY_NAME,
                                    R.NATIVE_DISTRICT,
                                    GEN.GENDER_NAME,
                                    BG.BLOOD_GROUP_NAME,
                                    FATHER_MOBILE_NUMBER,
                                    MOTHER_MOBILE_NUMBER,
                                    COM.COMMUNITY,
	                                R.COMMUNITY_ID,
                                    IF(R.IS_ROMAN_CATHOLIC = 1, 'YES', 'NO') AS IS_ROMAN_CATHOLIC, 
                                    RI.RELIGION,
                                    IF(R.IS_DALIT =1,'YES','NO') AS DALIT,
                                    N.NATIONALITY_NAME,
                                    R.AADHAR_NUMBER,
                                    MT.MOTHER_TONGUE_NAME AS 'LANGUAGE_NAME',
                                    SS.STATE,
                                    R.CDISTRICT AS CDISTRICT1,
                                    R.CPOST_PLACE_TOWN AS CPOST_PLACE_TOWN1,
                                    R.CVILLAGE_AREA AS CVILLAGE_AREA1,
                                    R.IS_NRI,
                                    R.PASSPORT_NUMBER,
                                    R.FATHER_NAME,
                                    OCC.OCCUPATION_NAME,
                                    MOCC.OCCUPATION_NAME AS 'MOTHER_OCCUPATION',
                                    R.PLACE_OF_BIRTH,
                                    R.ANNUAL_INCOME,
                                    R.MOTHER_NAME,
                                    IF(R.IS_FIRSTGENERATION =1,'YES','NO') AS IS_FIRST_GENERATION,
                                    IF(R.EXSERVICE_MAN =1,'YES','NO') AS EXSERVICE_MAN,
                                    IF(R.EXSERVICE_MAN =1,'APPLICABLE','NOT APPLICABLE') AS EXSERVICE_MAN_APPLICABLE,
                                    R.LAST_STUDIED_PLACE,
                                    R.LAST_STUDIED_SCHOOL,
                                     IF(R.SPECIALLYABLED_ID =1,'YES','NO') AS SPECIALLYABLED,
                                    R.EXTRA_CURRICULAR,
                                    R.LAST_STUDIED_CLASS,
                                    IF(R.MARITAL_STATUS_ID =1,'YES','NO') AS MARITAL_STATUS,
                                    IF(R.HOSTEL_FACILITY =1,'YES','NO') AS HOSTEL_FACILITY,
                                    IF(R.EDUCATION_BOARD_ID =1,'STATE BOARD',if(R.EDUCATION_BOARD_ID =0,'CBSC','')) AS EDUCATION_BOARD,
                                    R.HSC_NO,
                                    R.NAME_IN_NATIVE,
                                    R.MOTHER_INCOME,
                                    R.MOTHER_OCCUPATION,
                                    R.OCCUPATION,
                                    -- R.MOTHER_NAME_IN_NATIVE,
                                    -- R.MEDIUM_OF_STUDY,
                                    R.SPORTS,
                                    CONCAT(IFNULL(CDOORDETAIL,''),' ',IFNULL(CSTREET,'')) AS CDOORDETAIL,
                                    CONCAT(IFNULL(CPOST_PLACE_TOWN,''),' ',IFNULL(CVILLAGE_AREA,'')) AS CVILLAGE_AREA,
                                    CONCAT(IFNULL(CDISTRICT,''),' , ',SS.STATE) AS CDISTRICT,
                                    CPINCODE,
                                    CPHONENO,
                                    CMOBILENO,
                                    CONCAT(IFNULL(PDOORDETAIL,''),' ',IFNULL(PSTREET,'')) AS PDOORDETAIL,
                                    CONCAT(IFNULL(PPOST_PLACE_TOWN,''),' ',IFNULL(PVILLAGE_AREA,'')) AS PVILLAGE_AREA,
                                    CONCAT(IFNULL(PDISTRICT,''),' , ',SS.STATE) AS PDISTRICT,
                                    PPINCODE,
                                    PMOBILENO,
                                    PPHONENO,
                                    EMAIL_ID,
                                   -- CONCAT(IFNULL(HSS_GROUP_CODE,''),' -  ') AS HSS_GROUP,
                                    HSTOTAL,HSPERCENTAGE,HS_MAX_MARK,TOTAL_CUT_OFF_MARK,
                                    ATT.APPLICATION_TYPE,
                                    -- R.SPECIALLYABLED_TYPE,
                                    R.PARISHI_FRC,
                                    PHOTO_PATH,
                                    IF(R.UG_TOTAL_SEMESTER=5,'V','VI') AS UG_TOTAL_SEMESTER
                                   --  R.NO_OF_SEMESTERS,
                                   --  IFNULL(R.IS_ARREAR,'NO') AS IS_ARREAR
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS I
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS R ON R.RECEIVE_ID = I.RECEIVE_ID
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = I.PROGRAMME_GROUP_ID
                                       
                                        INNER JOIN
                                    CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
		                                LEFT JOIN
	                                SUP_GENDER AS GEN ON GEN.GENDER_ID=R.GENDER
		                                INNER JOIN
	                                SUP_APPLICATION_TYPE AS ATT ON ATT.APPLICATION_TYPE_ID=R.APPLICATION_TYPE_ID
		                                LEFT JOIN
	                                SUP_COMMUNITY AS COM ON COM.COMMUNITYID=R.CASTE_ID
		                                LEFT JOIN
	                                SUP_NATIONALITY AS N ON N.NATIONALITY_ID=R.NATIONALITY_ID
		                                LEFT JOIN
	                                SUP_RELIGION AS RI ON RI.RELIGIONID=R.RELIGION_ID
		                                LEFT JOIN
	                                SUP_MOTHER_TONGUE AS MT ON MT.MOTHER_TONGUE_ID=R.MOTHER_TONGUE
		                                LEFT JOIN
	                                SUP_OCCUPATION AS OCC ON OCC.OCCUPATION_ID=R.OCCUPATION
                                        LEFT JOIN
	                                SUP_OCCUPATION AS MOCC ON MOCC.OCCUPATION_ID=R.MOTHER_OCCUPATION
		                                LEFT JOIN
	                                SUP_MARRITAL_STATUS AS M ON M.MARITAL_STAUS_ID=R.MARITAL_STATUS_ID
		                                LEFT JOIN
	                                SUP_STATE AS SS ON SS.STATE_ID=R.CSTATE
		                                LEFT JOIN
	                                SUP_COUNTRY AS SC ON SC.COUNTRY_ID=R.CCOUNTRY
		                                LEFT JOIN
	                                SUP_STATE AS S ON S.STATE_ID=R.PSTATE
		                                LEFT JOIN
	                                SUP_COUNTRY AS CON ON CON.COUNTRY_ID=R.PCOUNTRY
		                                LEFT JOIN
	                                SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID=R.BLOOD_GROUP
		                               
                                WHERE I.RECEIVE_ID=?RECEIVE_ID
                                ORDER BY PG.PROGRAMME_GROUP_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentExtracurricularchk:
                    {
                        query = @"SELECT 
                                    ES.EXTRACURRICULAR_ID,
                                    ES.EXTRACURRICULAR_NAME,
                                    HES.STUDENT_ID,
                                    HES.HS_EXTRACURRICULAR_ID,
                                    IF(HES.HS_EXTRACURRICULAR_ID IS NULL,'','CHECKED') AS CHECKED
                                FROM
                                    SUP_STUDENT_EXTRACURRICULAR AS ES
                                        LEFT JOIN
                                    HOSTEL_STUDENT_EXTRACURRICULAR AS HES ON ES.EXTRACURRICULAR_ID = HES.EXTRACURRICULAR_ID
                                        AND HES.STUDENT_ID = ?STUDENT_ID
                                        AND HES.ACADEMIC_YEAR = ?AC_YEAR
                                        AND HES.IS_DELETED != 1
                                WHERE
                                    ES.IS_DELETED != 1 ORDER BY ES.EXTRACURRICULAR_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentExtracurricular:
                    {
                        query = @"SELECT 
                                    ES.EXTRACURRICULAR_ID,
                                    ES.EXTRACURRICULAR_NAME,
                                    HES.STUDENT_ID,
                                    HES.HS_EXTRACURRICULAR_ID,
                                    IF(HES.HS_EXTRACURRICULAR_ID IS NULL,'','CHECKED') AS CHECKED
                                FROM
                                    SUP_STUDENT_EXTRACURRICULAR AS ES
                                        LEFT JOIN
                                    HOSTEL_STUDENT_EXTRACURRICULAR AS HES ON ES.EXTRACURRICULAR_ID = HES.EXTRACURRICULAR_ID
                                        AND HES.ACADEMIC_YEAR = ?AC_YEAR
                                        AND HES.IS_DELETED != 1
                                WHERE
                                    ES.IS_DELETED != 1 ORDER BY ES.EXTRACURRICULAR_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentGadgetschk:
                    {
                        query = @"SELECT 
                                    GS.GADGETS_ID,
                                    GS.GADGETS_NAME,
                                    HGS.STUDENT_ID,
                                    HGS.HS_GADGETS_ID,
                                    IF(HGS.HS_GADGETS_ID IS NULL,
                                        '',
                                        'CHECKED') AS CHECKED
                                FROM
                                    SUP_STUDENT_GADGETS AS GS
                                        LEFT JOIN
                                    HOSTEL_STUDENT_GADGETS AS HGS ON HGS.GADGETS_ID = GS.GADGETS_ID
                                        AND HGS.STUDENT_ID = ?STUDENT_ID
                                        AND HGS.ACADEMIC_YEAR = ?AC_YEAR
                                        AND HGS.IS_DELETED != 1
                                WHERE
                                    GS.IS_DELETED != 1
                                ORDER BY GS.GADGETS_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentGadgets:
                    {
                        query = @"SELECT 
                                    GS.GADGETS_ID,
                                    GS.GADGETS_NAME,
                                    HGS.STUDENT_ID,
                                    HGS.HS_GADGETS_ID,
                                    IF(HGS.HS_GADGETS_ID IS NULL,
                                        '',
                                        'CHECKED') AS CHECKED
                                FROM
                                    SUP_STUDENT_GADGETS AS GS
                                        LEFT JOIN
                                    HOSTEL_STUDENT_GADGETS AS HGS ON HGS.GADGETS_ID = GS.GADGETS_ID
                                        AND HGS.ACADEMIC_YEAR = ?AC_YEAR
                                        AND HGS.IS_DELETED != 1
                                WHERE
                                    GS.IS_DELETED != 1
                                ORDER BY GS.GADGETS_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentSportschk:
                    {
                        query = @"SELECT 
                                    SP.SPORTS_ID,
                                    SP.SPORTS_NAME,
                                    HSP.STUDENT_ID,
                                    HSP.HS_SPORTS_ID,
                                    IF(HSP.HS_SPORTS_ID IS NULL,
                                        '',
                                        'CHECKED') AS CHECKED
                                FROM
                                    SUP_STUDENT_SPORTS AS SP
                                        LEFT JOIN
                                    HOSTEL_STUDENT_SPORTS AS HSP ON HSP.SPORTS_ID = SP.SPORTS_ID
                                        AND HSP.STUDENT_ID = ?STUDENT_ID
                                        AND HSP.ACADEMIC_YEAR = ?AC_YEAR
                                        AND HSP.IS_DELETED != 1
                                WHERE
                                    SP.IS_DELETED != 1
                                ORDER BY SP.SPORTS_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentSports:
                    {
                        query = @"SELECT 
                                    SP.SPORTS_ID,
                                    SP.SPORTS_NAME,
                                    HSP.STUDENT_ID,
                                    HSP.HS_SPORTS_ID,
                                    IF(HSP.HS_SPORTS_ID IS NULL,
                                        '',
                                        'CHECKED') AS CHECKED
                                FROM
                                    SUP_STUDENT_SPORTS AS SP
                                        LEFT JOIN
                                    HOSTEL_STUDENT_SPORTS AS HSP ON HSP.SPORTS_ID = SP.SPORTS_ID
                                        AND HSP.ACADEMIC_YEAR = ?AC_YEAR
                                        AND HSP.IS_DELETED != 1
                                WHERE
                                    SP.IS_DELETED != 1
                                ORDER BY SP.SPORTS_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentCertificatechk:
                    {
                        query = @"SELECT 
                                CT.CERTIFICATE_ID,
                                CT.CERTIFICATE_NAME,
                                HCT.STUDENT_ID,
                                HCT.HS_CERTIFICATE_ID,
                                IF(HCT.HS_CERTIFICATE_ID IS NULL,
                                    '',
                                    'CHECKED') AS CHECKED
                            FROM
                                SUP_STUDENT_CERTIFICATE AS CT
                                    LEFT JOIN
                                HOSTEL_STUDENT_CERTIFICATE AS HCT ON HCT.CERTIFICATE_ID = CT.CERTIFICATE_ID
                                    AND HCT.STUDENT_ID = ?STUDENT_ID
                                    AND HCT.ACADEMIC_YEAR = ?AC_YEAR
                                    AND HCT.IS_DELETED != 1
                            WHERE
                                CT.IS_DELETED != 1
                            ORDER BY CT.CERTIFICATE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentCertificate:
                    {
                        query = @"SELECT 
                                CT.CERTIFICATE_ID,
                                CT.CERTIFICATE_NAME,
                                HCT.STUDENT_ID,
                                HCT.HS_CERTIFICATE_ID,
                                IF(HCT.HS_CERTIFICATE_ID IS NULL,
                                    '',
                                    'CHECKED') AS CHECKED
                            FROM
                                SUP_STUDENT_CERTIFICATE AS CT
                                    LEFT JOIN
                                HOSTEL_STUDENT_CERTIFICATE AS HCT ON HCT.CERTIFICATE_ID = CT.CERTIFICATE_ID
                                    AND HCT.ACADEMIC_YEAR = ?AC_YEAR
                                    AND HCT.IS_DELETED != 1
                            WHERE
                                CT.IS_DELETED != 1
                            ORDER BY CT.CERTIFICATE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentGadgetschkIsExist:
                    {
                        query = @"SELECT 
                                HS_GADGETS_ID,
                                STUDENT_ID,
                                GADGETS_ID,
                                ACADEMIC_YEAR
                            FROM
                                HOSTEL_STUDENT_GADGETS
                            WHERE
                                STUDENT_ID = ?STUDENT_ID AND GADGETS_ID=?GADGETS_ID AND ACADEMIC_YEAR=?AC_YEAR AND IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateHostelStudentGadgets:
                    {
                        query = @"UPDATE HOSTEL_STUDENT_GADGETS SET IS_DELETED=0 WHERE HS_GADGETS_ID=?HS_GADGETS_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertHostelStudentGadgets:
                    {
                        query = @"INSERT INTO HOSTEL_STUDENT_GADGETS (STUDENT_ID, GADGETS_ID,ACADEMIC_YEAR) VALUES (?STUDENT_ID, ?GADGETS_ID,?AC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentSportschkIsExist:
                    {
                        query = @"SELECT HS_SPORTS_ID, STUDENT_ID, SPORTS_ID, ACADEMIC_YEAR FROM hostel_student_sports WHERE STUDENT_ID = ?STUDENT_ID AND SPORTS_ID=?SPORTS_ID AND ACADEMIC_YEAR=?AC_YEAR AND IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateHostelStudentSports:
                    {
                        query = @"UPDATE hostel_student_sports SET IS_DELETED=0 WHERE HS_SPORTS_ID=?HS_SPORTS_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertHostelStudentSports:
                    {
                        query = @"INSERT INTO HOSTEL_STUDENT_SPORTS (STUDENT_ID, SPORTS_ID,ACADEMIC_YEAR) VALUES (?STUDENT_ID, ?SPORTS_ID,?AC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentExtracurricularchkIsExist:
                    {
                        query = @"SELECT HS_EXTRACURRICULAR_ID, STUDENT_ID, EXTRACURRICULAR_ID, ACADEMIC_YEAR FROM HOSTEL_STUDENT_EXTRACURRICULAR WHERE STUDENT_ID = ?STUDENT_ID AND EXTRACURRICULAR_ID=?EXTRACURRICULAR_ID AND ACADEMIC_YEAR=?AC_YEAR AND IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateHostelStudentExtracurricular:
                    {
                        query = @"UPDATE HOSTEL_STUDENT_EXTRACURRICULAR SET IS_DELETED=0 WHERE HS_EXTRACURRICULAR_ID=?HS_EXTRACURRICULAR_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertHostelStudentExtracurricular:
                    {
                        query = @"INSERT INTO HOSTEL_STUDENT_EXTRACURRICULAR (STUDENT_ID, EXTRACURRICULAR_ID,ACADEMIC_YEAR) VALUES (?STUDENT_ID, ?EXTRACURRICULAR_ID,?AC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelStudentCertificatechkIsExist:
                    {
                        query = @"SELECT HS_CERTIFICATE_ID, STUDENT_ID, CERTIFICATE_ID, ACADEMIC_YEAR FROM HOSTEL_STUDENT_CERTIFICATE WHERE STUDENT_ID = ?STUDENT_ID AND CERTIFICATE_ID=?CERTIFICATE_ID AND ACADEMIC_YEAR=?AC_YEAR AND IS_DELETED!=1;";
                        break;
                    }
                case AdmissionSQLCommands.UpdateHostelStudentCertificate:
                    {
                        query = @"UPDATE HOSTEL_STUDENT_CERTIFICATE SET IS_DELETED=0 WHERE HS_CERTIFICATE_ID=?HS_CERTIFICATE_ID;";
                        break;
                    }
                case AdmissionSQLCommands.InsertHostelStudentCertificate:
                    {
                        query = @"INSERT INTO HOSTEL_STUDENT_CERTIFICATE (STUDENT_ID, CERTIFICATE_ID,ACADEMIC_YEAR) VALUES (?STUDENT_ID, ?CERTIFICATE_ID,?AC_YEAR);";
                        break;
                    }
                case AdmissionSQLCommands.FetchApplicationFormByReceiveIdForHostel:
                    {
                        query = @"SELECT 
	                                R.RECEIVE_ID,
                                    CONCAT('H',IF(APPTYPE_ID=1,'U','P'),PG.PREFIX,PROGRAMME_GROUP_CODE,LPAD(I.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    -- CONCAT(I.APPLICATION_NO,LPAD(I.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    UPPER(R.FIRST_NAME) AS FIRST_NAME,
                                    SMS_MOBILE_NO,
                                    PG.PROGRAMME_GROUP_ID,
                                    PROGRAMME_NAME,
                                    IFNULL(DATE_FORMAT(R.DATE_OF_BIRTH,'%d-%m-%Y'),' - ') AS DATE_OF_BIRTH,
                                    YEAR(CURDATE()) - YEAR(R.DATE_OF_BIRTH) AS AGE, 
                                    GEN.GENDER_NAME,
                                    BG.BLOOD_GROUP_NAME,
                                    COM.COMMUNITY,
	                                R.COMMUNITY_ID,
                                    IF(R.IS_ROMAN_CATHOLIC = 1, 'YES', 'NO') AS IS_ROMAN_CATHOLIC, 
                                    MOTHER_MOBILE_NUMBER,
                                    FATHER_MOBILE_NUMBER,
                                    RI.RELIGION,
                                    IF(R.IS_DALIT =1,'YES','NO') AS DALIT,
                                    N.NATIONALITY_NAME,
                                    R.AADHAR_NUMBER,
                                    MT.MOTHER_TONGUE_NAME AS 'LANGUAGE_NAME',
                                    SS.STATE,
                                    R.CDISTRICT AS CDISTRICT1,
                                    R.CPOST_PLACE_TOWN AS CPOST_PLACE_TOWN1,
                                    R.CVILLAGE_AREA AS CVILLAGE_AREA1,
                                    R.IS_NRI,
                                    R.PASSPORT_NUMBER,
                                    R.FATHER_NAME,
                                    R.OCCUPATION,
                                    R.ANNUAL_INCOME,
                                    R.MOTHER_INCOME,
                                    R.MOTHER_OCCUPATION,
                                    R.ANNUAL_INCOME,
                                    R.MOTHER_NAME,
                                    R.PLACE_OF_BIRTH,
                                    IF(R.IS_FIRSTGENERATION =1,'YES','NO') AS IS_FIRST_GENERATION,
                                    IF(R.EXSERVICE_MAN =1,'YES','NO') AS EXSERVICE_MAN,
                                    IF(R.EXSERVICE_MAN =1,'APPLICABLE','NOT APPLICABLE') AS EXSERVICE_MAN_APPLICABLE,
                                    R.LAST_STUDIED_PLACE,
                                    R.LAST_STUDIED_SCHOOL,
                                     IF(R.SPECIALLYABLED_ID =1,'YES','NO') AS SPECIALLYABLED,
                                    R.EXTRA_CURRICULAR,
                                    R.LAST_STUDIED_CLASS,
                                    IF(R.MARITAL_STATUS_ID =1,'YES','NO') AS MARITAL_STATUS,
                                    IF(R.HOSTEL_FACILITY =1,'YES','NO') AS HOSTEL_FACILITY,
                                    IF(R.EDUCATION_BOARD_ID =1,'STATE BOARD',if(R.EDUCATION_BOARD_ID =0,'CBSC','')) AS EDUCATION_BOARD,
                                    R.HSC_NO,
                                    R.NAME_IN_NATIVE,
                                    R.NATIVE_DISTRICT,
                                   -- R.FATHER_NAME_IN_NATIVE,
                                    -- R.MOTHER_NAME_IN_NATIVE,
                                    -- R.MEDIUM_OF_STUDY,
                                    R.SPORTS,
                                    CONCAT(IFNULL(CDOORDETAIL,''),', ',IFNULL(CSTREET,'')) AS CDOORDETAIL,
                                    IF(CVILLAGE_AREA IS NULL,1, CONCAT(IFNULL(CPOST_PLACE_TOWN,''),' ',IFNULL(CVILLAGE_AREA,''))) AS CVILLAGE_AREA,
                                    CONCAT(IFNULL(CDISTRICT,''),', ',SS.STATE) AS CDISTRICT,
                                    CPINCODE,
                                    CPHONENO,
                                    CMOBILENO,
                                    CONCAT(IFNULL(PDOORDETAIL,''),', ',IFNULL(PSTREET,'')) AS PDOORDETAIL,
                                    CONCAT(IFNULL(PPOST_PLACE_TOWN,''),' ',IFNULL(PVILLAGE_AREA,'')) AS PVILLAGE_AREA,
                                    CONCAT(IFNULL(PDISTRICT,''),', ',SS.STATE) AS PDISTRICT,
                                    PPINCODE,
                                    PMOBILENO,
                                    PPHONENO,
                                    EMAIL_ID,
                                   -- CONCAT(IFNULL(HSS_GROUP_CODE,''),' -  ') AS HSS_GROUP,
                                    HSTOTAL,HSPERCENTAGE,HS_MAX_MARK,TOTAL_CUT_OFF_MARK,
                                    ATT.APPLICATION_TYPE,
                                    -- R.SPECIALLYABLED_TYPE,
                                    R.PARISHI_FRC,
                                    PHOTO_PATH,
                                    LDOORDETAIL,
                                    LSTREET,
                                    LCITY,
                                    LDISTRICT,
                                    LPINCODE,
                                    LPHONENO,
                                    BSNAME,
                                    BSOCCUPATION,
                                    BSINCOME,
                                    IF(BSNAME1 IS NULL,1,BSNAME1) AS BSNAME1,
                                    BSOCCUPATION1,
                                    BSINCOME1,
                                    IF(BSNAME2 IS NULL,1,BSNAME2) AS BSNAME2,
                                    BSOCCUPATION2,
                                    BSINCOME2,
                                    BSMOBILE,
                                    BSMOBILE1,
                                    BSMOBILE2
                                   --  R.NO_OF_SEMESTERS,
                                   --  IFNULL(R.IS_ARREAR,'NO') AS IS_ARREAR
                                FROM
                                    ADM_ISSUED_APPLICATIONS AS I
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS R ON R.RECEIVE_ID = I.RECEIVE_ID
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = I.PROGRAMME_GROUP_ID
                                       
                                        INNER JOIN
                                    CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
		                                LEFT JOIN
	                                SUP_GENDER AS GEN ON GEN.GENDER_ID=R.GENDER
		                                INNER JOIN
	                                SUP_APPLICATION_TYPE AS ATT ON ATT.APPLICATION_TYPE_ID=R.APPLICATION_TYPE_ID
		                                LEFT JOIN
	                                SUP_COMMUNITY AS COM ON COM.COMMUNITYID=R.CASTE_ID
		                                LEFT JOIN
	                                SUP_NATIONALITY AS N ON N.NATIONALITY_ID=R.NATIONALITY_ID
		                                LEFT JOIN
	                                SUP_RELIGION AS RI ON RI.RELIGIONID=R.RELIGION_ID
		                                LEFT JOIN
	                                SUP_MOTHER_TONGUE AS MT ON MT.MOTHER_TONGUE_ID=R.MOTHER_TONGUE
		                                LEFT JOIN
	                                SUP_OCCUPATION AS OCC ON OCC.OCCUPATION_ID=R.OCCUPATION
                                        LEFT JOIN
	                                SUP_OCCUPATION AS MOCC ON MOCC.OCCUPATION_ID=R.MOTHER_OCCUPATION
		                                LEFT JOIN
	                                SUP_MARRITAL_STATUS AS M ON M.MARITAL_STAUS_ID=R.MARITAL_STATUS_ID
		                                LEFT JOIN
	                                SUP_STATE AS SS ON SS.STATE_ID=R.CSTATE
		                                LEFT JOIN
	                                SUP_COUNTRY AS SC ON SC.COUNTRY_ID=R.CCOUNTRY
		                                LEFT JOIN
	                                SUP_STATE AS S ON S.STATE_ID=R.PSTATE
		                                LEFT JOIN
	                                SUP_COUNTRY AS CON ON CON.COUNTRY_ID=R.PCOUNTRY
		                                LEFT JOIN
	                                SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID=R.BLOOD_GROUP
		                               
                                WHERE I.RECEIVE_ID=?RECEIVE_ID
                                GROUP BY R.RECEIVE_ID
                                ORDER BY PG.PROGRAMME_GROUP_ID;";
                        break;
                    }
                case AdmissionSQLCommands.FetchHostelApplication:
                    {
                        query = @"SELECT 
	                                R.RECEIVE_ID,
                                    CONCAT('H',IF(APPTYPE_ID=1,'U','P'),PG.PREFIX,PROGRAMME_GROUP_CODE,LPAD(I.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    -- CONCAT(I.APPLICATION_NO,LPAD(I.ISSUE_NO,4,'0')) AS APPLICATION_NO,
                                    UPPER(R.FIRST_NAME) AS FIRST_NAME,
                                    SMS_MOBILE_NO,
                                    PG.PROGRAMME_GROUP_ID,
                                    PROGRAMME_NAME,
                                    IFNULL(DATE_FORMAT(R.DATE_OF_BIRTH,'%d-%m-%Y'),' - ') AS DATE_OF_BIRTH,
                                    YEAR(CURDATE()) - YEAR(R.DATE_OF_BIRTH) AS AGE, 
                                    GEN.GENDER_NAME,
                                    BG.BLOOD_GROUP_NAME,
                                    COM.COMMUNITY,
	                                R.COMMUNITY_ID,
                                    IF(R.IS_ROMAN_CATHOLIC = 1, 'YES', 'NO') AS IS_ROMAN_CATHOLIC, 
                                    MOTHER_MOBILE_NUMBER,
                                    FATHER_MOBILE_NUMBER,
                                    RI.RELIGION,
                                    IF(R.IS_DALIT =1,'YES','NO') AS DALIT,
                                    N.NATIONALITY_NAME,
                                    R.AADHAR_NUMBER,
                                    MT.MOTHER_TONGUE_NAME AS 'LANGUAGE_NAME',
                                    SS.STATE,
                                    R.CDISTRICT AS CDISTRICT1,
                                    R.CPOST_PLACE_TOWN AS CPOST_PLACE_TOWN1,
                                    R.CVILLAGE_AREA AS CVILLAGE_AREA1,
                                    R.IS_NRI,
                                    R.PASSPORT_NUMBER,
                                    R.FATHER_NAME,
                                    R.OCCUPATION,
                                    R.ANNUAL_INCOME,
                                    R.MOTHER_INCOME,
                                    R.MOTHER_OCCUPATION,
                                    R.ANNUAL_INCOME,
                                    R.MOTHER_NAME,
                                    R.PLACE_OF_BIRTH,
                                    IF(R.IS_FIRSTGENERATION =1,'YES','NO') AS IS_FIRST_GENERATION,
                                    IF(R.EXSERVICE_MAN =1,'YES','NO') AS EXSERVICE_MAN,
                                    IF(R.EXSERVICE_MAN =1,'APPLICABLE','NOT APPLICABLE') AS EXSERVICE_MAN_APPLICABLE,
                                    R.LAST_STUDIED_PLACE,
                                    R.LAST_STUDIED_SCHOOL,
                                     IF(R.SPECIALLYABLED_ID =1,'YES','NO') AS SPECIALLYABLED,
                                    R.EXTRA_CURRICULAR,
                                    R.LAST_STUDIED_CLASS,
                                    IF(R.MARITAL_STATUS_ID =1,'YES','NO') AS MARITAL_STATUS,
                                    IF(R.HOSTEL_FACILITY =1,'YES','NO') AS HOSTEL_FACILITY,
                                    IF(R.EDUCATION_BOARD_ID =1,'STATE BOARD',if(R.EDUCATION_BOARD_ID =0,'CBSC','')) AS EDUCATION_BOARD,
                                    R.HSC_NO,
                                    R.NAME_IN_NATIVE,
                                    R.NATIVE_DISTRICT,
                                   -- R.FATHER_NAME_IN_NATIVE,
                                    -- R.MOTHER_NAME_IN_NATIVE,
                                    -- R.MEDIUM_OF_STUDY,
                                    R.SPORTS,
                                    CONCAT(IFNULL(CDOORDETAIL,''),', ',IFNULL(CSTREET,'')) AS CDOORDETAIL,
                                    IF(CVILLAGE_AREA IS NULL,1, CONCAT(IFNULL(CPOST_PLACE_TOWN,''),' ',IFNULL(CVILLAGE_AREA,''))) AS CVILLAGE_AREA,
                                    CONCAT(IFNULL(CDISTRICT,''),', ',SS.STATE) AS CDISTRICT,
                                    CPINCODE,
                                    CPHONENO,
                                    CMOBILENO,
                                    CONCAT(IFNULL(PDOORDETAIL,''),', ',IFNULL(PSTREET,'')) AS PDOORDETAIL,
                                    CONCAT(IFNULL(PPOST_PLACE_TOWN,''),' ',IFNULL(PVILLAGE_AREA,'')) AS PVILLAGE_AREA,
                                    CONCAT(IFNULL(PDISTRICT,''),', ',SS.STATE) AS PDISTRICT,
                                    PPINCODE,
                                    PMOBILENO,
                                    PPHONENO,
                                    EMAIL_ID,
                                   -- CONCAT(IFNULL(HSS_GROUP_CODE,''),' -  ') AS HSS_GROUP,
                                    HSTOTAL,HSPERCENTAGE,HS_MAX_MARK,TOTAL_CUT_OFF_MARK,
                                    ATT.APPLICATION_TYPE,
                                    -- R.SPECIALLYABLED_TYPE,
                                    R.PARISHI_FRC,
                                    PHOTO_PATH,
                                    LDOORDETAIL,
                                    LSTREET,
                                    LCITY,
                                    LDISTRICT,
                                    LPINCODE,
                                    LPHONENO,
                                    BSNAME,
                                    BSOCCUPATION,
                                    BSINCOME,
                                    IF(BSNAME1 IS NULL,1,BSNAME1) AS BSNAME1,
                                    BSOCCUPATION1,
                                    BSINCOME1,
                                    IF(BSNAME2 IS NULL,1,BSNAME2) AS BSNAME2,
                                    BSOCCUPATION2,
                                    BSINCOME2,
                                    BSMOBILE,
                                    BSMOBILE1,
                                    BSMOBILE2
                                   --  R.NO_OF_SEMESTERS,
                                   --  IFNULL(R.IS_ARREAR,'NO') AS IS_ARREAR
                                FROM
                                      FEE_TRANSACTION AS FT
                                        INNER JOIN 
                                    ADM_ISSUED_APPLICATIONS AS I ON FT.STUDENT_ID = I.RECEIVE_ID
                                        INNER JOIN
                                    ADM_RECEIVE_APPLICATION AS R ON R.RECEIVE_ID = I.RECEIVE_ID
                                        INNER JOIN
                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = I.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
		                                LEFT JOIN
	                                SUP_GENDER AS GEN ON GEN.GENDER_ID=R.GENDER
		                                INNER JOIN
	                                SUP_APPLICATION_TYPE AS ATT ON ATT.APPLICATION_TYPE_ID=R.APPLICATION_TYPE_ID
		                                LEFT JOIN
	                                SUP_COMMUNITY AS COM ON COM.COMMUNITYID=R.CASTE_ID
		                                LEFT JOIN
	                                SUP_NATIONALITY AS N ON N.NATIONALITY_ID=R.NATIONALITY_ID
		                                LEFT JOIN
	                                SUP_RELIGION AS RI ON RI.RELIGIONID=R.RELIGION_ID
		                                LEFT JOIN
	                                SUP_MOTHER_TONGUE AS MT ON MT.MOTHER_TONGUE_ID=R.MOTHER_TONGUE
		                                LEFT JOIN
	                                SUP_OCCUPATION AS OCC ON OCC.OCCUPATION_ID=R.OCCUPATION
                                        LEFT JOIN
	                                SUP_OCCUPATION AS MOCC ON MOCC.OCCUPATION_ID=R.MOTHER_OCCUPATION
		                                LEFT JOIN
	                                SUP_MARRITAL_STATUS AS M ON M.MARITAL_STAUS_ID=R.MARITAL_STATUS_ID
		                                LEFT JOIN
	                                SUP_STATE AS SS ON SS.STATE_ID=R.CSTATE
		                                LEFT JOIN
	                                SUP_COUNTRY AS SC ON SC.COUNTRY_ID=R.CCOUNTRY
		                                LEFT JOIN
	                                SUP_STATE AS S ON S.STATE_ID=R.PSTATE
		                                LEFT JOIN
	                                SUP_COUNTRY AS CON ON CON.COUNTRY_ID=R.PCOUNTRY
		                                LEFT JOIN
	                                SUP_BLOOD_GROUP AS BG ON BG.BLOOD_GROUP_ID=R.BLOOD_GROUP
		                               
                                WHERE FT.FREQUENCY IN (3) -- AND  R.HOSTEL_FACILITY=1
                                GROUP BY R.RECEIVE_ID
                                ORDER BY PG.PROGRAMME_GROUP_ID;";
                        break;
                    }

                case AdmissionSQLCommands.FetchUGPersonalInfoforRankList:
                    {
                        query = @"SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                            AR.TOTAL_CUT_OFF_MARK AS TOTAL,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                            EXTRA_CURRICULAR,
                                            CP.PROGRAMME_NAME,PM.PROGRAMME_MODE,
                                            AR.HSPERCENTAGE,
                                            AR.SMS_MOBILE_NO,
                                            AR.COMMUNITY_ID AS CASTE
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN
                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID NOT IN (2 , 75, 1, 71, 73,80)
                                                AND AI.ISSUED_ID <=?ISSUED_ID
                                                AND AI.STATUS >1 AND MR.MARK!=0
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID
                                        GROUP BY AR.RECEIVE_ID
                                        ORDER BY AR.TOTAL_CUT_OFF_MARK DESC;";
                        
                        break;
                    }
                case AdmissionSQLCommands.FetchUGPersonalInfoforRankListByReceiveId:
                    {
                        query = @"SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                            AR.TOTAL_CUT_OFF_MARK AS TOTAL,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                            EXTRA_CURRICULAR,
                                            CP.PROGRAMME_NAME,PM.PROGRAMME_MODE,
                                            AR.HSPERCENTAGE,
                                            AR.SMS_MOBILE_NO,
                                            AR.COMMUNITY_ID AS CASTE
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN
                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID NOT IN (2 , 75, 1, 71, 73,80)
                                                AND AI.ISSUED_ID <=?ISSUED_ID
                                                AND AI.STATUS >1 AND MR.MARK!=0 AND AI.RECEIVE_ID IN (?RECEIVE_ID)
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID
                                        GROUP BY AR.RECEIVE_ID
                                        ORDER BY AR.TOTAL_CUT_OFF_MARK DESC;";

                        break;
                    }

                case AdmissionSQLCommands.FetchPGPersonalInfoforRankList:
                    {
                        query = @"SELECT * FROM (SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                          --   AR.TOTAL_CUT_OFF_MARK AS TOTAL,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,PM.PROGRAMME_MODE,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                            EXTRA_CURRICULAR,
                                            CP.PROGRAMME_NAME,
                                            MR.MAX_MARK,
                                            MR.MARK,
											round((MR.MARK / MR.MAX_MARK) * 100) as TOTAL,
                                             AR.COMMUNITY_ID AS CASTE,
                                            AR.SMS_MOBILE_NO
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN
                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID  IN (81)
                                                AND AI.ISSUED_ID <= ?ISSUED_ID
                                                AND AI.STATUS >1 AND MR.MARK!=0
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID 
                                        GROUP BY AR.RECEIVE_ID) AS T 
                                        ORDER BY TOTAL DESC;";
                        break;
                    }
                case AdmissionSQLCommands.FetchPGPersonalInfoforRankListByReceiveId:
                    {
                        query = @"SELECT * FROM (SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                          --   AR.TOTAL_CUT_OFF_MARK AS TOTAL,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,PM.PROGRAMME_MODE,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                            EXTRA_CURRICULAR,
                                            CP.PROGRAMME_NAME,
                                            MR.MAX_MARK,
                                            MR.MARK,
											round((MR.MARK / MR.MAX_MARK) * 100) as TOTAL,
                                             AR.COMMUNITY_ID AS CASTE,
                                            AR.SMS_MOBILE_NO
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN
                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID  IN (81)
                                                AND AI.ISSUED_ID <= ?ISSUED_ID
                                                AND AI.STATUS >1 AND MR.MARK!=0
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID  AND AI.RECEIVE_ID IN (?RECEIVE_ID)
                                        GROUP BY AR.RECEIVE_ID) AS T 
                                        ORDER BY TOTAL DESC;";
                        break;
                    }

                case AdmissionSQLCommands.FetchPGPersonalInfoforSecondRankList:
                    {
                        query = @"SELECT * FROM (SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                          --   AR.TOTAL_CUT_OFF_MARK AS TOTAL,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,PM.PROGRAMME_MODE,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                            EXTRA_CURRICULAR,
                                            CP.PROGRAMME_NAME,
                                            MR.MAX_MARK,
                                            MR.MARK,
											round((MR.MARK / MR.MAX_MARK) * 100) as TOTAL,
                                            AR.COMMUNITY_ID AS CASTE
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN
                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID  IN (81)
                                                AND AI.ISSUED_ID > ?ISSUED_ID
                                                AND AI.STATUS >1 AND MR.MARK!=0
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID
                                        GROUP BY AR.RECEIVE_ID ) AS T 
                                        ORDER BY TOTAL DESC;";
                        break;
                    }
                case AdmissionSQLCommands.FetchPGPersonalInfoforSecondRankListByReceiveId:
                    {
                        query = @"SELECT * FROM (SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                          --   AR.TOTAL_CUT_OFF_MARK AS TOTAL,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,PM.PROGRAMME_MODE,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                            EXTRA_CURRICULAR,
                                            CP.PROGRAMME_NAME,
                                            MR.MAX_MARK,
                                            MR.MARK,
											round((MR.MARK / MR.MAX_MARK) * 100) as TOTAL,
                                            AR.COMMUNITY_ID AS CASTE
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN
                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID  IN (81)
                                                AND AI.ISSUED_ID > ?ISSUED_ID
                                                AND AI.STATUS >1 AND MR.MARK!=0
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID AND AI.RECEIVE_ID IN (?RECEIVE_ID)
                                        GROUP BY AR.RECEIVE_ID ) AS T 
                                        ORDER BY TOTAL DESC;";
                        break;
                    }

                case AdmissionSQLCommands.FetchUGEnglishPersonalInfoforRankList:
                    {
                        query = @"SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                            MR.MARK AS TOTAL,
                                            MR.MARK AS MARK,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,PM.PROGRAMME_MODE,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                            EXTRA_CURRICULAR,
                                                CP.PROGRAMME_NAME,
                                             AR.COMMUNITY_ID AS CASTE, AR.SMS_MOBILE_NO
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN

                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID  IN (2 , 75)
                                                AND AI.ISSUED_ID <= ?ISSUED_ID
                                                AND AI.STATUS >1 AND MR.MARK!=0
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID -- AND AI.ISSUE_NO IN ()
                                        GROUP BY AR.RECEIVE_ID
                                        ORDER BY MR.MARK DESC;";

                        //query = @"SELECT 
                        //            RECEIVE_ID,
                        //            APPLICATION_NO,
                        //            NAME,
                        //            RELIGION,
                        //            COMMUNITY,
                        //            SUBJECT1,
                        //            IS_PHYSICALLY_CHALANGED,
                        //            IS_EXSERVICE_MAN,
                        //            TOTAL,
                        //            MARK,
                        //            PROGRAMME_ID,
                        //            IS_SPORTS,
                        //            EXTRA_CURRICULAR,
                        //            PROGRAMME_NAME,
                        //            CASTE,
                        //            SMS_MOBILE_NO
                        //        FROM
                        //            (SELECT 
                        //                T.*, WT.APPLICATION_NO AS S_APP
                        //            FROM
                        //                (SELECT 
                        //                AI.RECEIVE_ID,
                        //                    CONCAT(AI.APPLICATION_NO, LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                        //                    UPPER(AR.FIRST_NAME) AS `NAME`,
                        //                    IF(AR.IS_ROMAN_CATHOLIC = '1', 'RC', IF(R.RELIGION = 'HINDU', 'H', IF(R.RELIGION = 'MUSLIM', 'M', IF(R.RELIGION = 'CHRISTIAN', 'CH', 'OTHER')))) AS RELIGION,
                        //                    COMMUNITY,
                        //                    hs.hss_subject AS SUBJECT1,
                        //                    IF(AR.PHYSICALY_CHALLANGED_TYPE = '1', 'YES', '') AS IS_PHYSICALLY_CHALANGED,
                        //                    IF(AR.EXSERVICE_MAN = '1', 'YES', '') AS IS_EXSERVICE_MAN,
                        //                    MR.MARK AS TOTAL,
                        //                    MR.MARK AS MARK,
                        //                    pg.PROGRAMME_GROUP_ID AS PROGRAMME_ID,
                        //                    IF(AR.SPORTS = '1', 'YES', '') AS IS_SPORTS,
                        //                    EXTRA_CURRICULAR,
                        //                    CP.PROGRAMME_NAME,
                        //                    AR.COMMUNITY_ID AS CASTE,
                        //                    AR.SMS_MOBILE_NO
                        //            FROM
                        //                adm_issued_applications AS AI
                        //            INNER JOIN adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                        //            INNER JOIN cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                        //            INNER JOIN cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                        //            INNER JOIN sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                        //            INNER JOIN sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                        //            INNER JOIN adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                        //            INNER JOIN sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                        //            LEFT JOIN sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                        //            LEFT JOIN sup_community AS co ON co.COMMUNITYID = ar.caste_id
                        //            WHERE
                        //                HS.HSS_SUBJECT_ID IN (2 , 75)
                        //                    AND AI.ISSUED_ID >= ?ISSUED_ID
                        //                    AND AI.STATUS =2
                        //                    AND MR.MARK != 0
                        //                    AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID
                        //            GROUP BY AR.RECEIVE_ID
                        //            ORDER BY MR.MARK DESC) AS T
                        //            LEFT JOIN adm_waiting_application_2022 AS WT ON WT.APPLICATION_NO = T.APPLICATION_NO) AS M
                        //        WHERE
                        //            M.S_APP IS NULL;";
                        break;
                    }
                case AdmissionSQLCommands.FetchUGEnglishPersonalInfoforRankListByReceiveId:
                    {
                        query = @"SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                            MR.MARK AS TOTAL,
                                            MR.MARK AS MARK,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,PM.PROGRAMME_MODE,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                            EXTRA_CURRICULAR,
                                                CP.PROGRAMME_NAME,
                                             AR.COMMUNITY_ID AS CASTE, AR.SMS_MOBILE_NO
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN

                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID  IN (2 , 75)
                                                AND AI.ISSUED_ID <= ?ISSUED_ID
                                                AND AI.STATUS >1 AND MR.MARK!=0
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID AND AI.RECEIVE_ID IN (?RECEIVE_ID)
                                        GROUP BY AR.RECEIVE_ID
                                        ORDER BY MR.MARK DESC;";
                        break;
                    }


                case AdmissionSQLCommands.FetchUGMarksForRankList:
                    {
                        query = @"SELECT 
                                    MR.RECEIVE_STUID,
                                    HS.HSS_SUBJECT,
                                    MR.MARK,
                                    HS.SUB_CODE,
                                    AR.HSPERCENTAGE
                                FROM
                                    adm_issued_applications AS AI
                                        INNER JOIN
                                    adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                        INNER JOIN
                                    cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                        INNER JOIN
                                    sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                        INNER JOIN
                                    sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                        left JOIN
                                    adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                        left JOIN
                                    sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                        LEFT JOIN
                                    sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                        LEFT JOIN
                                    sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                WHERE
                                    HS.HSS_SUBJECT_ID NOT IN (2 , 75, 1, 71, 73,80)
                                        AND AI.ISSUED_Id <= ?ISSUED_ID
                                        AND AI.STATUS >1
                                        AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID AND MR.MARK!=0 group by MR.RECEIVE_STUID,MR.SUBJECT_ID
                                ORDER BY AR.TOTAL_CUT_OFF_MARK DESC;";
                        break;
                    }
                case AdmissionSQLCommands.FetchUGMarksForRankListByReceiveId:
                    {
                        query = @"SELECT 
                                    MR.RECEIVE_STUID,
                                    HS.HSS_SUBJECT,
                                    MR.MARK,
                                    HS.SUB_CODE,
                                    AR.HSPERCENTAGE
                                FROM
                                    adm_issued_applications AS AI
                                        INNER JOIN
                                    adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                        INNER JOIN
                                    cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                        INNER JOIN
                                    sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                        INNER JOIN
                                    sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                        left JOIN
                                    adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                        left JOIN
                                    sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                        LEFT JOIN
                                    sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                        LEFT JOIN
                                    sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                WHERE
                                    HS.HSS_SUBJECT_ID NOT IN (2 , 75, 1, 71, 73,80)
                                        AND AI.ISSUED_Id <= ?ISSUED_ID
                                        AND AI.STATUS >1 AND AI.RECEIVE_ID IN (?RECEIVE_ID)
                                        AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID AND MR.MARK!=0 group by MR.RECEIVE_STUID,MR.SUBJECT_ID
                                ORDER BY AR.TOTAL_CUT_OFF_MARK DESC;";
                        break;
                    }
                case AdmissionSQLCommands.FetchUGLateApplication:
                    {
                        query = @"SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                            AR.TOTAL_CUT_OFF_MARK AS TOTAL,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                            CP.PROGRAMME_NAME
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN
                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID NOT IN (2 , 75, 1, 71, 73,80)
                                                AND AI.ISSUED_ID > ?ISSUED_ID
                                                AND AI.STATUS > 1
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID AND MR.MARK!=0
                                        GROUP BY AR.RECEIVE_ID
                                        ORDER BY AR.TOTAL_CUT_OFF_MARK DESC;";
                        break;
                    }
                case AdmissionSQLCommands.FetchUGEnglishLateApplication:
                    {
                        query = @"SELECT 
                                            AI.RECEIVE_ID,
                                            CONCAT(AI.APPLICATION_NO,
                                                    LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                            UPPER(AR.FIRST_NAME) AS `NAME`,
                                            IF(AR.IS_ROMAN_CATHOLIC='1','RC',IF(R.RELIGION='HINDU','H',IF(R.RELIGION='MUSLIM','M',IF(R.RELIGION='CHRISTIAN','CH','OTHER')))) AS RELIGION,
                                            COMMUNITY,
                                            hs.hss_subject AS SUBJECT1,
                                            IF(AR.PHYSICALY_CHALLANGED_TYPE='1','YES','') AS IS_PHYSICALLY_CHALANGED,
                                            IF(AR.EXSERVICE_MAN='1','YES','') AS IS_EXSERVICE_MAN,
                                            MR.MARK AS TOTAL,
                                            MR.MARK AS MARK,
                                            pg.PROGRAMME_GROUP_ID as PROGRAMME_ID,
                                            IF(AR.SPORTS='1','YES','') AS IS_SPORTS,
                                                CP.PROGRAMME_NAME
                                        FROM
                                            adm_issued_applications AS AI
                                                INNER JOIN
                                            adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                                INNER JOIN
                                            cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                                INNER JOIN
                                            cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                                INNER JOIN
                                            sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                                INNER JOIN
                                            sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                                LEFT JOIN
                                            adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                                LEFT JOIN
                                            sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                                LEFT JOIN
                                            sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                                LEFT JOIN
                                            sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                        WHERE
                                            HS.HSS_SUBJECT_ID  IN (2 , 75)
                                                AND AI.ISSUED_ID > ?ISSUED_ID
                                                AND AI.STATUS > 1
                                                AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID AND MR.MARK!=0
                                        GROUP BY AR.RECEIVE_ID
                                        ORDER BY MR.MARK DESC;";
                        break;
                    }
                case AdmissionSQLCommands.FetchUGLateApplicationMarksForRankList:
                    {
                        query = @"SELECT 
                                    MR.RECEIVE_STUID,
                                    HS.HSS_SUBJECT,
                                    MR.MARK,
                                    HS.SUB_CODE
                                FROM
                                    adm_issued_applications AS AI
                                        INNER JOIN
                                    adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                        INNER JOIN
                                    cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                        INNER JOIN
                                    sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                        INNER JOIN
                                    sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                        LEFT JOIN
                                    adm_stu_submarks AS MR ON MR.RECEIVE_STUID = AR.RECEIVE_ID
                                        LEFT JOIN
                                    sup_hss_subjects AS hs ON hs.HSS_SUBJECT_ID = mr.SUBJECT_ID
                                        LEFT JOIN
                                    sup_religion AS r ON r.RELIGIONID = ar.RELIGION_ID
                                        LEFT JOIN
                                    sup_community AS co ON co.COMMUNITYID = ar.caste_id
                                WHERE
                                    HS.HSS_SUBJECT_ID NOT IN (2 , 75, 1, 71, 73,80)
                                        AND AI.ISSUED_Id > ?ISSUED_ID
                                        AND AI.STATUS > 1 AND MR.MARK!=0
                                        AND pg.PROGRAMME_GROUP_ID = ?PROGRAMME_ID  group by MR.RECEIVE_STUID,MR.SUBJECT_ID
                                ORDER BY AR.TOTAL_CUT_OFF_MARK DESC;";
                        break;
                    }
                case AdmissionSQLCommands.GetProgammeMode:
                    {
                        query = @"SELECT 
                                        PROGRAMME_GROUP_ID, APPTYPE_ID, PROGRAMME_ID,  ACADEMIC_YEAR, PROGRAMME_MODE
                                    FROM
                                        cp_programme_group
                                    WHERE
                                        PROGRAMME_GROUP_ID = ?PROGRAMME_GROUP_ID
                                            AND IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.GetVerificationDateByProgrammeAndCycle:
                    {
                        query = @"SELECT 
                                        SELECTION_CYCLE_ID AS SELECTION_CYCLE,
                                        PROGRAMME_GROUP_ID AS PROGRAMME_ID,
                                        VERIFICATION_DATE,
                                        DATE_FORMAT(VERIFICATION_DATE, '%d/%m/%Y') AS DAY
                                    FROM
                                        ADM_VERIFICATION_DATE
                                    WHERE
                                        PROGRAMME_GROUP_ID = ?PROGRAMME_ID
                                            AND SELECTION_CYCLE_ID = ?SELECTION_CYCLE
                                            AND ACADEMIC_YEAR = ?AC_YEAR
                                            AND IS_DELETED != 1;";
                        break;
                    }
                case AdmissionSQLCommands.FetchCoursesbyReceiveID:
                    {
                        query = @"SELECT 
                                    AI.RECEIVE_ID,
                                    CONCAT(AI.APPLICATION_NO,
                                            LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                    UPPER(AR.FIRST_NAME) AS `NAME`,
                                    pg.PROGRAMME_GROUP_ID AS PROGRAMME_ID,
                                    CP.PROGRAMME_NAME,
                                    CONCAT(PG.PROGRAMME_GROUP_CODE,
                                            '-',
                                            PG.PROGRAMME_MODE) AS PROGRAMME_GROUP_CODE
                                FROM
                                    adm_issued_applications AS AI
                                        INNER JOIN
                                    adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                        INNER JOIN
                                    cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                        INNER JOIN
                                    sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                        INNER JOIN
                                    sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                WHERE
                                    AI.ISSUED_ID <= ?ISSUED_ID AND AI.STATUS = 2 ;";
                        break;
                    }
                case AdmissionSQLCommands.FetchlistByReceiveid:
                    {
                        query = @"SELECT 
                                    AI.RECEIVE_ID,
                                    CONCAT(AI.APPLICATION_NO,
                                            LPAD(AI.ISSUE_NO, 4, 0)) AS APPLICATION_NO,
                                    UPPER(AR.FIRST_NAME) AS `NAME`,
                                    pg.PROGRAMME_GROUP_ID AS PROGRAMME_ID,
                                    CP.PROGRAMME_NAME,
                                    CONCAT(PG.PROGRAMME_GROUP_CODE,
                                            '-',
                                            PG.PROGRAMME_MODE) AS PROGRAMME_GROUP_CODE
                                FROM
                                    adm_issued_applications AS AI
                                        INNER JOIN
                                    adm_receive_application AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                        INNER JOIN
                                    cp_programme_group AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                        INNER JOIN
                                    cp_programme AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                        INNER JOIN
                                    sup_programme_mode AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                        INNER JOIN
                                    sup_applicant_type AS APP ON APP.APPLICANT_TYPE_ID = PG.APPTYPE_ID
                                WHERE
                                    AI.ISSUED_ID <= ?ISSUED_ID AND AI.STATUS = 2 AND AI.PROGRAMME_ID= ?PROGRAMME_GROUP_ID AND AI.RECEIVE_ID IN (?RECEIVE_ID);";
                        break;
                    }
                case AdmissionSQLCommands.FetchIssueNobyProgrameID:
                    {
                        query = @"SELECT 
                                    ISSUE_NO,PROGRAMME_GROUP_ID,APPLICATION_NO FROM ADM_ISSUED_APPLICATIONS 
                                    WHERE PROGRAMME_GROUP_ID = ?PROGRAMME_TO
                                    ORDER BY ISSUE_NO;";
                        break;
                    }
            }
            return query;
        }
    }
}
