using CMS.DAO;

namespace CMS.SQL.StaffSQL
{
    public static class StaffSQL
    {
        public static string GetStaffSQL(StaffSQLCommands sEnumCommand)
        {
            string query = string.Empty;
            switch (sEnumCommand)
            {
                case StaffSQLCommands.FetchUserAccountByStaffCode:
                    {
                        query = @"SELECT 
                                    USER_ACCOUNT_ID,
                                    USERNAME,
                                    ROLE,
                                    USER_TYPE
                                FROM
                                    USER_ACCOUNT WHERE USERNAME=?USERNAME AND IS_DELETED!=1;";
                        break;
                    }
                case StaffSQLCommands.PersonalIfExits:
                    {
                        query = @"SELECT STAFF_ID,STAFF_CODE FROM STF_PERSONAL_INFO WHERE STAFF_CODE=?STAFF_CODE AND IS_DELETED!=1;";
                        break;
                    }
                case StaffSQLCommands.FetchLastInsertedId:
                    {
                        query = @" SELECT 
                                                    STAFF_ID
                                                FROM
                                                    STF_PERSONAL_INFO
                                                ORDER BY STF_PERSONAL_INFO.STAFF_ID DESC
                                                LIMIT 1;";
                        break;
                    }
                case StaffSQLCommands.FetchStaffList:
                    {
                        query = @"SELECT 
	                                            STAFF_ID,
                                                STAFF_CODE,
                                                FIRST_NAME,
                                                D.DEPARTMENT,
                                                G.GENDER_NAME,
                                                S.SHIFT_NAME
                                            FROM
                                                STF_PERSONAL_INFO SPI
                                                LEFT JOIN CP_DEPARTMENT_?AC_YEAR AS D ON D.DEPARTMENT_ID=SPI.DEPARTMENT
                                                LEFT JOIN SUP_GENDER AS G ON G.GENDER_ID=SPI.GENDER
                                                LEFT JOIN SUP_SHIFT AS S ON S.SHIFT_ID=SPI.SHIFT
                                                WHERE SPI.IS_DELETED!=1";
                        break;
                    }
                case StaffSQLCommands.FetchPersonalRecordByID:
                    {
                        query = @" SELECT 
	                                            STAFF_ID,
                                                
                                                STAFF_CODE,
                                                FIRST_NAME,
                                                LAST_NAME,
                                                KNOWN_AS,
                                                PLACE_OF_BIRTH,
                                                DATE_FORMAT(DATE_OF_BIRTH,'%d/%m/%Y') AS DATE_OF_BIRTH,
                                                DATE_FORMAT(DATE_OF_JOIN,'%d/%m/%Y') AS DATE_OF_JOIN,
                                                BLOOD_GROUP,
                                                MARITAL_STATUS,
                                                DEPARTMENT,
                                                
                                                MOTHER_TONGUE,
                                                GENDER,
                                                COMMUNITY,
                                                NATIONALITY,
                                                RELIGION
                                            FROM
                                                STF_PERSONAL_INFO WHERE STAFF_ID=?STAFF_ID AND IS_DELETED!=1;";
                        break;
                    }
                case StaffSQLCommands.FetchPaperById:
                    {
                        query = @"SELECT       PAPER_ID,
                                               STAFF_NO,
                                               PAPER_NAME,
                                               L.LEVEL,
                                               P.PUBLISH_CATEGORY,
                                               JOURNAL_NAME,
                                               NO_OF_PAGES,
                                               PAGES_FROM, 
                                               PAGES_TO,
                                               V.VOLUME,
											   DATE_FORMAT(YEAR_PUBLISHED,'%d/%m/%Y') AS YEAR_PUBLISHED
                                            FROM STF_PAPER SP 
                                            INNER JOIN SUP_LEVEL AS L ON L.LEVEL_ID=SP.LEVEL
                                            INNER JOIN SUP_PUBLISH_CATEGORY AS P ON P.PUBLISH_CATEGORY_ID=SP.PUBLISHING_CATEGORY
                                            INNER JOIN SUP_VOLUME AS V ON V.VOLUME_ID=SP.VOLUME
                                            WHERE  SP.IS_DELETED!=1 AND SP.STAFF_NO=?STAFF_NO;";
                        break;
                    }
                case StaffSQLCommands.BindPaperInfo:
                    {
                        query = @"SELECT 
                                    PAPER_ID,
                                    STAFF_NO, 
                                    PAPER_NAME, 
                                    LEVEL, 
                                    PUBLISHING_CATEGORY, 
                                    JOURNAL_NAME,
                                    NO_OF_PAGES,
                                    PAGES_FROM, 
                                    PAGES_TO, 
                                    VOLUME, 
                                    DATE_FORMAT(YEAR_PUBLISHED,'%d/%m/%Y') AS YEAR_PUBLISHED, 
                                    IS_DELETED
                                FROM
                                    STF_PAPER WHERE IS_DELETED!=1 AND PAPER_ID=?PAPER_ID;";
                        break;
                    }
                case StaffSQLCommands.DeletePaper:
                    {
                        query = @"UPDATE STF_PAPER 
                                SET 
                                    IS_DELETED = ?IS_DELETED
                                WHERE
                                    PAPER_ID = ?PAPER_ID;";
                        break;
                    }
                case StaffSQLCommands.FetchTrainingById:
                    {
                        query = @"SELECT TRAINING_NO,
                                                STAFF_NO,
                                                 DATE_FORMAT(DATE_FROM,'%d/%m/%Y') AS DATE_FROM,
                                                 DATE_FORMAT(DATE_TO,'%d/%m/%Y') AS DATE_TO,
                                                PROGRAMME,
                                                PLACE,
                                                TLEVEL,
                                                COMMENTS
                                            FROM STF_TRAINING WHERE ISDELETED!=1 AND STAFF_NO=?STAFF_NO;";
                        break;
                    }
                case StaffSQLCommands.BindTraining:
                    {
                        query = @"SELECT TRAINING_NO,
                                                STAFF_NO,
                                                DATE_FORMAT(DATE_FROM,'%d/%m/%Y') AS DATE_FROM,
                                                DATE_FORMAT(DATE_TO,'%d/%m/%Y') AS DATE_TO,
                                                PROGRAMME,
                                                PLACE,
                                                TLEVEL,
                                                COMMENTS,
                                                ISDELETED
                                            FROM STF_TRAINING WHERE ISDELETED!=1 AND TRAINING_NO=?TRAINING_NO;";
                        break;
                    }
                case StaffSQLCommands.DeleteTraining:
                    {
                        query = @"UPDATE STF_TRAINING  SET ISDELETED=?ISDELETED WHERE TRAINING_NO=?TRAINING_NO;";
                        break;
                    }
                case StaffSQLCommands.FetchQualificationById:
                    {
                        query = @"SELECT 
											QUALIFICATION_NO,
											STAFF_NO,
											D.DEGREE,
											DEGREE_TYPE,
											MAIN_SUBJECT,
											ALLIED_SUBJECT,
											DATE_FORMAT(COMPLETION_MONTH,'%d/%m/%Y') AS COMPLETION_MONTH,
											INSTITUTE_OF_STUDY,
											UNIVERSITY,
											PERCENTAGE,
											SQ.ISDELETED,
											DATE_FORMAT(COMPLETION_YEAR,'%d/%m/%Y') AS COMPLETION_YEAR
                                            FROM STF_QUALIFICATION SQ
                                            INNER JOIN SUP_DEGREE AS D ON D.DEGREE_ID=SQ.DEGREE
                                            WHERE SQ.ISDELETED!=1 AND STAFF_NO=?STAFF_NO;";
                        break;
                    }
                case StaffSQLCommands.BindQualification:
                    {
                        query = @"SELECT 
                                              QUALIFICATION_NO,
                                               STAFF_NO,
                                               DEGREE,
                                               DEGREE_TYPE,
                                               MAIN_SUBJECT,
                                               ALLIED_SUBJECT,
                                               DATE_FORMAT(COMPLETION_MONTH,'%d/%m/%Y') AS COMPLETION_MONTH,
                                               INSTITUTE_OF_STUDY,
                                               UNIVERSITY,
                                               PERCENTAGE,
                                               ISDELETED,
                                               DATE_FORMAT(COMPLETION_YEAR,'%d/%m/%Y') AS COMPLETION_YEAR
                                            FROM STF_QUALIFICATION WHERE ISDELETED!=1 AND QUALIFICATION_NO=?QUALIFICATION_NO;";
                        break;
                    }
                case StaffSQLCommands.DeleteQualification:
                    {
                        query = @"UPDATE STF_QUALIFICATION  SET ISDELETED=?ISDELETED WHERE QUALIFICATION_NO=?QUALIFICATION_NO;";
                        break;
                    }
                case StaffSQLCommands.FetchPublishById:
                    {
                        query = @"SELECT 
	                                            PUBLISH_ID,
                                                STAFF_NO,
                                                BOOK_NAME,
                                                L.LEVEL,
                                                SPC.PUBLISH_CATEGORY,
                                                JOURNAL_NAME,
                                                V.VOLUME,
                                                PUBLISHER,
                                                DATE_FORMAT(PYEAR,'%d/%m/%Y') AS PYEAR,
                                                EDITION
                                            FROM STF_PUBLISH SP 
                                            LEFT JOIN SUP_LEVEL AS L ON L.LEVEL_ID=SP.LEVEL
                                            LEFT JOIN SUP_PUBLISH_CATEGORY AS SPC ON SPC.PUBLISH_CATEGORY_ID=SP.PUBLISHING_CATEGORY
                                            LEFT JOIN SUP_VOLUME AS V ON V.VOLUME_ID=SP.VOLUME
                                            WHERE SP.ISDELETED!=1 AND STAFF_NO=?STAFF_NO;";
                        break;
                    }
                case StaffSQLCommands.BindPublish:
                    {
                        query = @"SELECT 
	                                            PUBLISH_ID,
                                                STAFF_NO,
                                                BOOK_NAME,
                                                LEVEL,
                                                PUBLISHING_CATEGORY,
                                                JOURNAL_NAME,
                                                VOLUME,
                                                PUBLISHER,
                                                DATE_FORMAT(PYEAR,'%d/%m/%Y') AS PYEAR,
                                                EDITION
                                            FROM STF_PUBLISH WHERE ISDELETED!=1 AND PUBLISH_ID=?PUBLISH_ID;";
                        break;
                    }
                case StaffSQLCommands.DeletePublish:
                    {
                        query = @"UPDATE STF_PUBLISH  SET ISDELETED=?ISDELETED WHERE PUBLISH_ID=?PUBLISH_ID;";
                        break;
                    }
                case StaffSQLCommands.FetchCounselingById:
                    {
                        query = @"SELECT STF_COUN_ID,
													   DATE_FORMAT(DATE_OF_COUN,'%d/%m/%Y') AS DATE_OF_COUN,
                                                       DURATION,
                                                       STAFF,
                                                       REASON,
                                                       GIVEN,
                                                       ACTION_TAKEN,
                                                       REMARK
                                                    FROM STF_COUNSELING WHERE IS_DELETED!=1 AND STAFF=?STAFF;";
                        break;
                    }
                case StaffSQLCommands.BindCounselling:
                    {
                        query = @"SELECT 
                                    STF_COUN_ID,
                                    DATE_FORMAT(DATE_OF_COUN,'%d/%m/%Y') AS DATE_OF_COUN, 
                                    DURATION, 
                                    STAFF, 
                                    REASON, 
                                    GIVEN, 
                                    ACTION_TAKEN, 
                                    REMARK
                                FROM
                                    STF_COUNSELING WHERE IS_DELETED!=1 AND STF_COUN_ID=?STF_COUN_ID;";
                        break;
                    }
                case StaffSQLCommands.DeleteCounselling:
                    {
                        query = @"UPDATE STF_COUNSELING 
                                SET 
                                    IS_DELETED = ?IS_DELETED
                                WHERE
                                    STF_COUN_ID = ?STF_COUN_ID;";
                        break;
                    }
                case StaffSQLCommands.FetchAddressById:
                    {
                        query = @"SELECT 
                                               ADDRESS_NO,
                                               STAFFNO,
                                               CDOOR_DETAILS,
                                               CSTREET,
                                               CPLACE,
                                               CCITY,
                                               CPIN_CODE,
                                               CDISTRICT,
                                               CVILLAGE,
                                               CCOUNTRY,
                                               CPHONE_NO,
                                               CCELL_NO,
                                               CEMAIL,
                                               PSTREET,
                                               PVILLAGE,
                                               PPLACE,
                                               PCITY,
                                               PPIN_CODE,
                                               PDISTRICT,
                                               PCOUNTRY,
                                               PPHONE_NO,
                                               PCELL_NO,
                                               PDOOR_DETAILS,
                                               PEMAIL
                                            FROM STF_ADDRESS WHERE STAFFNO=?STAFFNO ;";
                        break;
                    }
                case StaffSQLCommands.FetchServiceById:
                    {
                        query = @"SELECT 
                                                SERVICE_NO,
                                                STAFF_NO,
                                                DATE_FORMAT(DATE_OF_APPOINT,'%d/%m/%Y') AS DATE_OF_APPOINT,
                                                APPOINTMENT_NAME,
                                                APPOINTMENT_NATURE,
                                                DATE_FORMAT(DATE_OF_TERMINATION,'%d/%m/%Y') AS DATE_OF_TERMINATION,
                                                INSTITUTE,
                                                REMARKS,
                                                PLACE
                                            FROM STF_SERVICES WHERE ISDELETED!=1 AND STAFF_NO=?STAFF_NO;";
                        break;
                    }
                case StaffSQLCommands.BindServices:
                    {
                        query = @" SELECT SERVICE_NO,
                                                STAFF_NO,
                                                DATE_FORMAT(DATE_OF_APPOINT,'%d/%m/%Y') AS DATE_OF_APPOINT,
                                                APPOINTMENT_NAME,
                                                APPOINTMENT_NATURE,
                                                DATE_FORMAT(DATE_OF_TERMINATION,'%d/%m/%Y') AS DATE_OF_TERMINATION,
                                                INSTITUTE,
                                                REMARKS,
                                                PLACE,
                                                ISDELETED
                                            FROM STF_SERVICES WHERE  ISDELETED!=1 AND SERVICE_NO=?SERVICE_NO;";
                        break;
                    }
                case StaffSQLCommands.DeleteServices:
                    {
                        query = @"UPDATE STF_SERVICES  SET ISDELETED=?ISDELETED WHERE SERVICE_NO=?SERVICE_NO;";
                        break;
                    }
                case StaffSQLCommands.FetchSubjectDetailsById:
                    {
                        query = @"Select 
                                                STAFF_ID,
                                                BOARD_MEMBER,
                                                STAFF_CODE,
                                                DESIGNATION,
                                                KNOWLEDGE_IN_COMPUTER,
                                                MAIN_SUBJECT,
                                                NON_TEACHING_CATEGORY,
                                                QUALIFICATION,
                                                SESSIONS,
                                                SHIFT,
                                                SUB_CATEGORY FROM STF_PERSONAL_INFO 
                                            WHERE STAFF_ID=?STAFF_ID ;";
                        break;
                    }
                case StaffSQLCommands.FetchLeavingById:
                    {
                        query = @"select 
                                                DATE_FORMAT(LEAVING_DATE,'%d/%m/%Y') AS LEAVING_DATE,
                                                LEAVING_REMARKS,
                                                DEPARTMENT,
                                                DATE_FORMAT(RETIREMENT_DATE,'%d/%m/%Y') AS RETIREMENT_DATE,
                                                LEAVING_REASON,
                                                IS_LEFT  FROM STF_PERSONAL_INFO
                                                WHERE STAFF_ID =?STAFF_ID;";
                        break;
                    }
                case StaffSQLCommands.FetchFamily:
                    {
                        query = @"SELECT 
                                                FAMILY_NO, 
                                                STAFF_NO,
                                                NAME, 
                                                R.RELATION, 
                                                DATE_FORMAT(DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH
                                            FROM
                                                STF_FAMILY SF
                                                INNER JOIN SUP_RELATION AS R ON R.RELATION_ID=SF.RELATION WHERE  ISDELETED!=1 AND STAFF_NO=?STAFF_NO;";
                        break;
                    }
                case StaffSQLCommands.BindFamily:
                    {
                        query = @"SELECT 
                                                FAMILY_NO, 
                                                STAFF_NO,
                                                NAME, 
                                                RELATION, 
                                                DATE_FORMAT(DATE_OF_BIRTH, '%d/%m/%Y') AS DATE_OF_BIRTH
                                            FROM
                                                STF_FAMILY WHERE ISDELETED!=1 AND FAMILY_NO=?FAMILY_NO;";
                        break;
                    }
                case StaffSQLCommands.DeleteFamily:
                    {
                        query = @"UPDATE STF_FAMILY  SET ISDELETED=?ISDELETED WHERE FAMILY_NO=?FAMILY_NO;";
                        break;
                    }
                case StaffSQLCommands.InsertPersonal:
                    {
                        query = @"INSERT INTO STF_PERSONAL_INFO(
                                    FIRST_NAME,LAST_NAME,GENDER,
                                    KNOWN_AS,PLACE_OF_BIRTH,COMMUNITY,
                                    NATIONALITY,MARITAL_STATUS,
                                    DATE_OF_JOIN,DATE_OF_BIRTH,
                                    BLOOD_GROUP,RELIGION,
                                    MOTHER_TONGUE,DEPARTMENT,STAFF_CODE)
                                    VALUES
                                    (
                                      ?FIRST_NAME,?LAST_NAME,?GENDER,
                                      ?KNOWN_AS,?PLACE_OF_BIRTH,?COMMUNITY,
                                      ?NATIONALITY,?MARITAL_STATUS,
                                      ?DATE_OF_JOIN,?DATE_OF_BIRTH,
                                      ?BLOOD_GROUP,?RELIGION,
                                      ?MOTHER_TONGUE,?DEPARTMENT,?STAFF_CODE
                                    );";
                        break;
                    }

                case StaffSQLCommands.InsertStaffAddress:
                    {
                        query = @"INSERT INTO STF_ADDRESS(
                                                  STAFFNO,CDOOR_DETAILS, CSTREET,CPLACE,CCITY,
                                                  CPIN_CODE, CDISTRICT,CCOUNTRY,CVILLAGE,CPHONE_NO,
                                                  CCELL_NO,CEMAIL,PSTREET,PVILLAGE,PPLACE,
                                                  PCITY,PPIN_CODE,PDISTRICT,PCOUNTRY,PPHONE_NO,
                                                  PCELL_NO,PDOOR_DETAILS,PEMAIL)
                                                  VALUES
                                                  (
                                                    ?STAFFNO,?CDOOR_DETAILS,?CSTREET,?CPLACE,?CCITY,
                                                    ?CPIN_CODE,?CDISTRICT,?CCOUNTRY,?CVILLAGE,?CPHONE_NO,
                                                    ?CCELL_NO,?CEMAIL,?PSTREET,?PVILLAGE,?PPLACE,
                                                    ?PCITY,?PPIN_CODE,?PDISTRICT,?PCOUNTRY,?PPHONE_NO,
                                                    ?PCELL_NO,?PDOOR_DETAILS,?PEMAIL
                                                   );";
                        break;
                    }
                case StaffSQLCommands.InsertStaffServices:
                    {
                        query = @"INSERT INTO STF_SERVICES(
                                                STAFF_NO,
                                                DATE_OF_APPOINT,
                                                APPOINTMENT_NAME,
                                                DATE_OF_TERMINATION,
                                                INSTITUTE,
                                                REMARKS,
                                                PLACE)
                                                VALUES
                                                (
                                                  ?STAFF_NO,?DATE_OF_APPOINT,?APPOINTMENT_NAME,?DATE_OF_TERMINATION,?INSTITUTE,?REMARKS,?PLACE
                                                );";
                        break;
                    }
                case StaffSQLCommands.InsertStaffCounseling:
                    {
                        query = @"INSERT INTO STF_COUNSELING(
                                             STAFF,
                                             DATE_OF_COUN,
                                             DURATION,
                                             REASON,
                                             GIVEN,
                                             ACTION_TAKEN,
                                             REMARK)
                                             VALUES
                                            (
                                              ?STAFF,?DATE_OF_COUN,?DURATION,?REASON,?GIVEN,?ACTION_TAKEN,?REMARK
                                            );";
                        break;
                    }
                case StaffSQLCommands.InsertStaffPaper:
                    {
                        query = @"INSERT INTO STF_PAPER(
                                                    STAFF_NO,
                                                    PAPER_NAME,
                                                    LEVEL,
                                                    PUBLISHING_CATEGORY,
                                                    JOURNAL_NAME,
                                                    NO_OF_PAGES,
                                                    PAGES_FROM,
                                                    PAGES_TO,
                                                    VOLUME,
                                                    YEAR_PUBLISHED)
                                                    VALUES
                                                    (
                                                      ?STAFF_NO,?PAPER_NAME,?LEVEL,?PUBLISHING_CATEGORY,?JOURNAL_NAME,?NO_OF_PAGES,?PAGES_FROM,?PAGES_TO,?VOLUME,?YEAR_PUBLISHED
                                                    );";
                        break;
                    }
                case StaffSQLCommands.InsertStaffPublish:
                    {
                        query = @"INSERT INTO STF_PUBLISH(STAFF_NO,BOOK_NAME,JOURNAL_NAME,PUBLISHER,PYEAR,EDITION,LEVEL,VOLUME,PUBLISHING_CATEGORY)VALUES
                                                (
                                                    ?STAFF_NO,?BOOK_NAME,?JOURNAL_NAME,?PUBLISHER,?PYEAR,?EDITION,?LEVEL,?VOLUME,?PUBLISHING_CATEGORY
                                                );";
                        break;
                    }
                case StaffSQLCommands.InsertStaffQualification:
                    {
                        query = @"INSERT INTO STF_QUALIFICATION(
                                                STAFF_NO,
                                                MAIN_SUBJECT,
                                                ALLIED_SUBJECT,
                                                COMPLETION_YEAR,
                                                COMPLETION_MONTH,
                                                INSTITUTE_OF_STUDY,
                                                DEGREE,UNIVERSITY,
                                                PERCENTAGE)
                                                VALUES
                                                (
                                                   ?STAFF_NO,?MAIN_SUBJECT,?ALLIED_SUBJECT,?COMPLETION_YEAR,?COMPLETION_MONTH,?INSTITUTE_OF_STUDY,?DEGREE,?UNIVERSITY,?PERCENTAGE
                                                 );";
                        break;
                    }
                case StaffSQLCommands.InsertStaffTraining:
                    {
                        query = @"INSERT INTO STF_TRAINING(STAFF_NO,
                                                  DATE_FROM,
                                                  DATE_TO,
                                                  PROGRAMME,
                                                  PLACE,
                                                  TLEVEL,
                                                  COMMENTS)
                                                   VALUES
                                                   (
                                                    ?STAFF_NO,?DATE_FROM,?DATE_TO,?PROGRAMME,?PLACE,?TLEVEL,?COMMENTS
                                                    );";
                        break;
                    }
                case StaffSQLCommands.InsertStaffFamily:
                    {
                        query = @"INSERT INTO STF_FAMILY(NAME,RELATION,DATE_OF_BIRTH,STAFF_NO)
                                                VALUES
                                                (
                                                   ?NAME,?RELATION,?DATE_OF_BIRTH,?STAFF_NO
                                                );";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffPersonal:
                    {
                        query = @"UPDATE STF_PERSONAL_INFO SET
                                                  FIRST_NAME=?FIRST_NAME,
                                                  LAST_NAME=?LAST_NAME,
                                                  STAFF_CODE=?STAFF_CODE,
                                                  GENDER=?GENDER,
                                                  KNOWN_AS=?KNOWN_AS,
                                                  PLACE_OF_BIRTH=?PLACE_OF_BIRTH,
                                                  COMMUNITY=?COMMUNITY,
                                                  NATIONALITY=?NATIONALITY,
                                                  MARITAL_STATUS=?MARITAL_STATUS,
                                                  DATE_OF_JOIN=?DATE_OF_JOIN,
                                                  DATE_OF_BIRTH=?DATE_OF_BIRTH,
                                                  BLOOD_GROUP=?BLOOD_GROUP,
                                                  RELIGION=?RELIGION,
                                                  MOTHER_TONGUE=?MOTHER_TONGUE,
                                                  DEPARTMENT=?DEPARTMENT WHERE STAFF_ID=?STAFF_ID;";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffAddress:
                    {
                        query = @"UPDATE STF_ADDRESS
                                                 SET
                                                 ADDRESS_NO = ?ADDRESS_NO,
                                                 CDOOR_DETAILS =?CDOOR_DETAILS,
                                                 CSTREET = ?CSTREET,
                                                 CPLACE = ?CPLACE,
                                                 CCITY = ?CCITY,
                                                 CPIN_CODE =?CPIN_CODE,
                                                 CDISTRICT = ?CDISTRICT,
                                                 CVILLAGE = ?CVILLAGE,
                                                 CCOUNTRY = ?CCOUNTRY,
                                                 CPHONE_NO = ?CPHONE_NO,
                                                 CCELL_NO = ?CCELL_NO,
                                                 CEMAIL = ?CEMAIL,
                                                 PSTREET = ?PSTREET,
                                                 PVILLAGE = ?PVILLAGE,
                                                 PPLACE =?PPLACE,
                                                 PCITY =?PCITY,
                                                 PPIN_CODE =?PPIN_CODE,
                                                 PDISTRICT =?PDISTRICT,
                                                 PCOUNTRY =?PCOUNTRY,
                                                 PPHONE_NO =?PPHONE_NO,
                                                 PCELL_NO =?PCELL_NO,
                                                 PDOOR_DETAILS =?PDOOR_DETAILS,
                                                 PEMAIL = ?PEMAIL,
                                                 STAFFNO=?STAFFNO
                                                 WHERE ADDRESS_NO = ?ADDRESS_NO;";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffCounseling:
                    {
                        query = @"UPDATE STF_COUNSELING SET
                                             DATE_OF_COUN=?DATE_OF_COUN,
                                             DURATION=?DURATION,
                                             REASON=?REASON,
                                             GIVEN=?GIVEN,
                                             ACTION_TAKEN=?ACTION_TAKEN,
                                             STAFF=?STAFF,
                                             REMARK=?REMARK WHERE STF_COUN_ID=?STF_COUN_ID;";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffPaper:
                    {
                        query = @"UPDATE STF_PAPER SET
                                                    PAPER_NAME=?PAPER_NAME,
                                                    LEVEL=?LEVEL,
                                                    PUBLISHING_CATEGORY=?PUBLISHING_CATEGORY,
                                                    JOURNAL_NAME=?JOURNAL_NAME,
                                                    NO_OF_PAGES=?NO_OF_PAGES,
                                                    PAGES_FROM=?PAGES_FROM,
                                                    PAGES_TO=?PAGES_TO,
                                                    VOLUME=?VOLUME,
                                                    STAFF_NO=?STAFF_NO,
                                                    YEAR_PUBLISHED=?YEAR_PUBLISHED WHERE PAPER_ID=?PAPER_ID;";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffPublish:
                    {
                        query = @"UPDATE STF_PUBLISH SET 
	                                            BOOK_NAME=?BOOK_NAME,
                                                JOURNAL_NAME=?JOURNAL_NAME,
                                                PUBLISHER=?PUBLISHER,
                                                PYEAR=?PYEAR,
                                                STAFF_NO=?STAFF_NO,
                                                VOLUME=?VOLUME,
                                                PUBLISHING_CATEGORY=?PUBLISHING_CATEGORY,
                                                LEVEL=?LEVEL,
                                                EDITION=?EDITION WHERE PUBLISH_ID=?PUBLISH_ID;";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffQualification:
                    {
                        query = @"UPDATE STF_QUALIFICATION SET 
	                                            MAIN_SUBJECT=?MAIN_SUBJECT,
                                                ALLIED_SUBJECT=?ALLIED_SUBJECT,
                                                COMPLETION_YEAR=?COMPLETION_YEAR,
                                                COMPLETION_MONTH=?COMPLETION_MONTH,
                                                INSTITUTE_OF_STUDY=?INSTITUTE_OF_STUDY,
                                                DEGREE=?DEGREE,
                                                UNIVERSITY=?UNIVERSITY,
                                                STAFF_NO=?STAFF_NO,
                                                PERCENTAGE=?PERCENTAGE WHERE QUALIFICATION_NO=?QUALIFICATION_NO;";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffService:
                    {
                        query = @"UPDATE STF_SERVICES SET
                                                   DATE_OF_APPOINT=?DATE_OF_APPOINT,
                                                   APPOINTMENT_NAME=?APPOINTMENT_NAME,
                                                   DATE_OF_TERMINATION=?DATE_OF_TERMINATION,
                                                   INSTITUTE=?INSTITUTE,
                                                   REMARKS=?REMARKS,
                                                   STAFF_NO=?STAFF_NO,
                                                   PLACE=?PLACE WHERE SERVICE_NO=?SERVICE_NO;";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffSubjectDetails:
                    {
                        query = @"UPDATE STF_PERSONAL_INFO SET                                                 
                                                 BOARD_MEMBER =?BOARD_MEMBER,
                                                 DESIGNATION =?DESIGNATION,
                                                 KNOWLEDGE_IN_COMPUTER =?KNOWLEDGE_IN_COMPUTER,
                                                 MAIN_SUBJECT =?MAIN_SUBJECT,
                                                 NON_TEACHING_CATEGORY =?NON_TEACHING_CATEGORY,
                                                 QUALIFICATION =?QUALIFICATION,
                                                 SESSIONS =?SESSIONS,
                                                 SHIFT =?SHIFT,
                                                 SUB_CATEGORY =?SUB_CATEGORY
                                             WHERE STAFF_ID =?STAFF_ID; ";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffTraining:
                    {
                        query = @"UPDATE STF_TRAINING SET 
                                                  DATE_FROM=?DATE_FROM,
                                                  DATE_TO=?DATE_TO,
                                                  PROGRAMME=?PROGRAMME,
                                                  PLACE=?PLACE,
                                                  TLEVEL=?TLEVEL,
                                                  STAFF_NO=?STAFF_NO,
                                                  COMMENTS=?COMMENTS WHERE TRAINING_NO=?TRAINING_NO;";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffLeaving:
                    {
                        query = @"UPDATE STF_PERSONAL_INFO
                                                         SET
                                                         LEAVING_DATE =?LEAVING_DATE,
                                                         LEAVING_REMARKS =?LEAVING_REMARKS,
                                                         DEPARTMENT =?DEPARTMENT,
                                                         RETIREMENT_DATE =?RETIREMENT_DATE,
                                                         LEAVING_REASON =?LEAVING_REASON,
                                                         IS_LEFT = ?IS_LEFT
                                                         WHERE STAFF_ID =?STAFF_ID;";
                        break;
                    }
                case StaffSQLCommands.UpdateStaffFamily:
                    {
                        query = @"UPDATE STF_FAMILY 
                                                SET 
                                                    NAME = ?NAME,
                                                    RELATION = ?RELATION,
                                                    DATE_OF_BIRTH = ?DATE_OF_BIRTH,
                                                    STAFF_NO=?STAFF_NO
                                                WHERE
                                                    FAMILY_NO = ?FAMILY_NO;";
                        break;
                    }
                case StaffSQLCommands.DeleteQuery:
                    {
                        query = @"UPDATE STF_PERSONAL_INFO 
                                                SET 
                                                    IS_DELETED = ?IS_DELETED
                                                WHERE
                                                    STAFF_ID = ?STAFF_ID;";
                        break;
                    }
                case StaffSQLCommands.FetchStaffProfileView:
                    {
                        query = @"SELECT 
                                        SF.STAFF_ID,
                                        SF.STAFF_CODE,
                                        SF.FIRST_NAME,
                                        SG.GENDER_NAME,
                                        SN.NATIONALITY_NAME,
                                        SC.STF_CATEGORY,
                                        DATE_FORMAT(DATE_OF_BIRTH, '%d/%m/%y') AS DATE_OF_BIRTH,
                                        DATE_FORMAT(DATE_OF_JOIN, '%d/%m/%y') AS DATE_OF_JOIN,
                                        DT.DEPARTMENT,
                                        SH.SHIFT_NAME,
                                        SF.QUALIFICATION,
                                        SF.PHOTO
                                    FROM
                                        STF_PERSONAL_INFO AS SF
                                            LEFT JOIN
                                        SUP_GENDER AS SG ON SG.GENDER_ID = SF.GENDER
                                            LEFT JOIN
                                        SUP_NATIONALITY AS SN ON SN.NATIONALITY_ID = SF.NATIONALITY
                                            LEFT JOIN
                                        SUP_STAFF_CATEGORY AS SC ON SC.STF_CATEGORY_ID = SF.CATEGORY
		                                    LEFT JOIN
	                                    CP_DEPARTMENT_?AC_YEAR AS DT ON DT.DEPARTMENT_ID=SF.DEPARTMENT
		                                    INNER JOIN
	                                    SUP_SHIFT AS SH ON SH.SHIFT_ID=SF.SHIFT
                                    WHERE
                                        SF.STAFF_ID=?STAFF_ID;";
                        break;
                    }
                default:
                    break;
            }

            return query;
        }
    }
}
