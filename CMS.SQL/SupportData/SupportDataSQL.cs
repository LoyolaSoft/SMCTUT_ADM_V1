using CMS.DAO;

namespace CMS.SQL.SupportData
{
    public static class SupportDataSQL
    {
        public static string GetSupportDataSQL(SupportDataSQLCommands sEnumCommand)
        {
            string query = "";
            switch (sEnumCommand)
            {


                case SupportDataSQLCommands.GetActiveSemesterType:
                    {
                        query = @"SELECT 
                                    SEMESTER_TYPE_ID, SEMESTER_TYPE
                                FROM
                                    SUP_SEMESTER_TYPE
                                WHERE
                                    IS_ACTIVE = 1 AND IS_DELETED != 1;";
                        break;
                    }

                case SupportDataSQLCommands.FetchFrequencyIdForExam:
                    {
                        query = @"SELECT 
                                    FREQUENCY_ID,
                                    ACADEMIC_YEAR,
                                    FREQUENCY_NAME,
                                    DATE_FROM,
                                    DATE_TO,
                                    STATUS,                               
                                    IS_USED,
                                    date_format(LAST_DATE_OF_PAYMENT,'%Y/%m/%d') as LAST_DATE_OF_PAYMENT,
                                    IS_DOWNLOADED,
                                    IS_UPDATED,
                                    DOWNLOAD_TIME,
                                    TYPE
                                FROM
                                    SUP_FEE_FREQUENCY AS ff
                                      INNER JOIN
                                        SUP_SEMESTER_TYPE AS ST ON ST.SEMESTER_TYPE_ID = FF.SEMESTER_TYPE
                                            AND ST.IS_ACTIVE = 1
                                WHERE
                                    ff.IS_DELETED != 1 AND ff.TYPE = ?TYPE and ff.ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }

                case SupportDataSQLCommands.FecthSubjectType:
                    {
                        query = @"SELECT 
                                    SUBJECT_TYPE_ID, SUBJECT_TYPE
                                FROM
                                    SUP_SUBJECT_TYPE AS ST
                                WHERE
                                    ST.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.IsSupHeadExist:
                    {
                        query = @"SELECT 
                                    HEAD_ID, 
                                    HEAD, 
                                    FEE_CATEGORY
                                FROM
                                    SUP_HEAD WHERE HEAD=?HEAD AND FEE_CATEGORY=?FEE_CATEGORY AND IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.DeleteSupHead:
                    {
                        query = @"UPDATE SUP_HEAD SET IS_DELETED=1 WHERE HEAD_ID=?HEAD_ID;";
                        break;
                    }
                case SupportDataSQLCommands.SaveSupHead:
                    {
                        query = @"INSERT INTO SUP_HEAD (HEAD,FEE_CATEGORY)VALUES(?HEAD,?FEE_CATEGORY);";
                        break;
                    }
                case SupportDataSQLCommands.UpdateSupHead:
                    {
                        query = @"UPDATE SUP_HEAD SET HEAD=?HEAD,FEE_CATEGORY=?FEE_CATEGORY WHERE HEAD_ID=?HEAD_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupHeadForEdit:
                    {
                        query = @"SELECT 
                                    HEAD_ID, 
                                    HEAD, 
                                    FEE_CATEGORY
                                FROM
                                    SUP_HEAD WHERE HEAD_ID=?HEAD_ID";

                        break;
                    }
                case SupportDataSQLCommands.FetchddlCategory:
                    {
                        query = @"SELECT 
                                    FEE_CATEGORY_ID,
                                    FEE_CATEGORY
                                FROM
                                    SUP_FEE_CATEGORY ";

                        break;
                    }
                case SupportDataSQLCommands.FetchSupHeadList:
                    {
                        query = @"SELECT 
                                    SH.HEAD_ID, 
                                    SH.HEAD, 
                                    SC.FEE_CATEGORY
                                FROM
                                    SUP_HEAD AS SH 
                                    INNER JOIN
	                                SUP_FEE_CATEGORY AS SC ON SC.FEE_CATEGORY_ID=SH.FEE_CATEGORY WHERE SH.IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchCourseListByProgrammeAndBatch:
                    {
                        query = @"SELECT CP.COURSE_CODE,
                                CP.COURSE_ROOT_ID,
                                CONCAT('(',
                                        IFNULL(CP.COURSE_CODE, ''),
                                        ') ',
                                        IFNULL(CP.COURSE_TITLE, '')) AS COURSE_TITLE
                            FROM
                                ACADEMIC_CURRICULUM_?AC_YEAR AS AC
                                    INNER JOIN
                                CP_COURSE_ROOT_?AC_YEAR AS CP ON CP.COURSE_ROOT_ID = AC.COURSE_ID
                            WHERE
                                AC.PROGRAMME = ?PROGRAMME AND AC.BATCH = ?BATCH
                                    AND AC.IS_DELETED != 1
                                    AND AC.IS_ACTIVE = 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHead:
                    {
                        query = @"SELECT 
                                    HEAD_ID,
                                    HEAD,
                                    HEAD_CODE,
                                    ACCOUNT,
                                    HEAD_TYPE,
                                    IS_USED,
                                    IS_DOWNLOADED,
                                    DOWNLOAD_TIME,
                                    IS_UPDATED,
                                    PROGRAMME_MODE,
                                    FEE_CATEGORY,
                                    TEMP_ID,
                                    HEAD_ORDER,
                                    IS_REFUNDABLE,
                                    SHIFT
                                FROM
                                    SUP_HEAD AS H
                                WHERE
                                    H.IS_DELETED != 1;";
                        break;
                    }

                case SupportDataSQLCommands.FetchActiveSemesterByProgrammeAndBatch:
                    {
                        query = @"SELECT 
                                    ACC_SEMESTER_ID, BATCH, PROGRAMME, SEMESTER, DATE_FROM, DATE_TO
                                FROM
                                    ACADEMIC_SEMESTER_?AC_YEAR AS ACS
                                WHERE
                                    ACS.PROGRAMME = ?PROGRAMME AND BATCH = ?BATCH
                                        AND IS_ACTIVE = 1;";
                        break;
                    }

                case SupportDataSQLCommands.FetchAcademicBatches:
                    {
                        query = @"SELECT 
                                AB.BATCH_ID, SB.BATCH
                            FROM
                                ACADEMIC_BATCHES AS AB
                                    INNER JOIN
                                ACADEMIC_YEAR AS AY ON AB.ACADEMIC_YEAR_ID = AY.ACADEMIC_YEAR_ID
                                    INNER JOIN
                                SUP_BATCHES AS SB ON SB.BATCH_ID = AB.BATCH_ID
                            WHERE
                                AY.ACTIVE_YEAR = 1
                                    AND AY.IS_DELETED != 1
                                    AND SB.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchOnlyRegularClasses:
                    {
                        query = @"SELECT 
                                    CLS.CLASS_ID,
                                    CONCAT(IFNULL(CLS.CLASS_DESCRIPTION, ''),
                                            ' (SHIFT ',
                                            S.SHIFT_NAME,
                                            ')') AS CLASS_NAME
                                FROM
                                    CP_CLASSES_?AC_YEAR AS CLS
                                        INNER JOIN
                                    SUP_SHIFT AS S ON S.SHIFT_ID = CLS.SHIFT
                                WHERE
                                    CLS.IS_DELETED != 1
                                        AND CLS.CLASS_TYPE = 2;";
                        break;
                    }

                case SupportDataSQLCommands.FetchHours:
                    {
                        query = @"SELECT 
                                        HOUR_ID, HOUR, TIME_FROM, TIME_TO, DESCRIPTION
                                    FROM
                                        SUP_HOURS
                                    WHERE
                                        IS_DELETED != 1 AND IS_ACTIVE = 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassesByStaffIdAndActiveSemester:
                    {
                        query = @" SELECT 
                                        CS.CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                ' (SHIFT ',
                                                IFNULL(ST.SHIFT_NAME, ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                        CP_CLASS_COURSE_STAFF_?AC_YEAR AS CS
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = CS.CLASS_ID
                                            INNER JOIN
                                        CP_COURSE_ROOT_?AC_YEAR AS CR ON CR.COURSE_ROOT_ID = CS.COURSE_ID
                                            LEFT JOIN
                                        SUP_SHIFT AS ST ON ST.SHIFT_ID = CLS.SHIFT
                                    WHERE
                                        STAFF_ID = ?STAFF_ID
                                            AND CR.SEMESTER_ID IN (?SEMESTER_ID)
                                            AND CS.IS_DELETED!=1
                                    GROUP BY CS.CLASS_ID; 
                             ";
                        break;
                    }

                case SupportDataSQLCommands.FetchActiveSemesterForStudentByAcademicYear:
                    {

                        query = @"SELECT 
                                        GROUP_CONCAT(SEMESTER) AS SEMESTER
                                    FROM
                                        (SELECT 
                                            SEMESTER
                                        FROM
                                            ACADEMIC_SEMESTER_?AC_YEAR
                                        WHERE
                                            IS_ACTIVE = 1 AND IS_DELETED != 1
                                                AND BATCH = (SELECT 
                                                    BATCH
                                                FROM
                                                    STU_PERSONAL_INFO AS SP
                                                WHERE
                                                    SP.STUDENT_ID = ?STUDENT_ID) AND PROGRAMME=(SELECT 
                                                    PROGRAM_ID
                                                FROM
                                                    STU_PERSONAL_INFO AS SP
                                                WHERE
                                                    SP.STUDENT_ID = ?STUDENT_ID)
                                        GROUP BY SEMESTER) T; 
                             ";
                        break;
                    }
                case SupportDataSQLCommands.FetchActiveSemesterForStaffByAcademicYear:
                    {

                        query = @" SELECT 
                                        GROUP_CONCAT(SEMESTER) AS SEMESTER
                                    FROM
                                        (SELECT 
                                            SEMESTER
                                        FROM
                                            ACADEMIC_SEMESTER_?AC_YEAR
                                        WHERE
                                            IS_ACTIVE = 1 AND IS_DELETED!=1
                                        GROUP BY SEMESTER) T
                             ";
                        break;
                    }
                case SupportDataSQLCommands.FetchReasonForAttendance:
                    {

                        query = @"SELECT 
                                REASON_ID, REASON, IS_ACTIVE, IS_DELETED
                            FROM
                                SUP_REASON
                            WHERE
                                IS_DELETED != 1;
                             ";
                        break;
                    }
                case SupportDataSQLCommands.FetchAttendanceTypes:
                    {

                        query = @"SELECT 
                                    ATTENDANCE_TYPE_ID, ATTENDANCE_TYPE, IS_DELETED, IS_ACTIVE
                                FROM
                                    SUP_ATTENDANCE_TYPE
                                WHERE
                                    IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchDayOrderInfoByCurrentDate:
                    {

                        query = @"SELECT 
                                            DAY_ID,
                                            DAY_ORDER_DATE,
                                            DAY_TYPE,
                                             IF(DAY_ORDER='','N/A',DAY_ORDER) AS DAY_ORDER,
                                            DAY_ORDER_MONTH,
                                            IS_HOLIDAY
                                        FROM
                                            CALENDER_?AC_YEAR AS CAL
                                        WHERE
                                            CAL.DAY_ORDER_DATE = CURDATE() AND IS_DELETED!=1 ;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassesByStaffId:
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
                                        ACADEMIC_SEMESTER_?AC_YEAR AS SEM ON SEM.PROGRAMME = CLS.PROGRAMME
                                    WHERE
                                        CS.STAFF_ID =?STAFF_ID
                                            AND SEM.PROGRAMME = CLS.PROGRAMME
                                            AND SEM.SEMESTER = RT.SEMESTER_ID
                                            AND SEM.IS_ACTIVE = 1
                                    GROUP BY CS.CLASS_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchCourseInfoByCourseCode:
                    {
                        query = @"SELECT 
                                    COURSE_ROOT_ID,
                                    YEAR_FROM,
                                    YEAR_TO,
                                    DEPARTMENT,
                                    YEAR,
                                    SEMESTER_FROM,
                                    SEMESTER_TO,
                                    PART,
                                    PAPER,
                                    COURSE_TYPE,
                                    HRS_PER_WEEK,
                                    CREDITS,
                                    OPTION_NAME,
                                    PAPER_CODE,
                                    COURSE_TITLE,
                                    COURSE_CODE,
                                    SEMESTER_ID,
                                    IS_ACTIVE,
                                    IS_NME_SUBJECT,
                                    IS_ALLIED_SUBJECT,
                                    COURSE_ORDER,
                                    IS_DELETED,
                                    SUBJECTS,
                                    IS_COMPULSORY,
                                    UGC_COURSE_TYPE
                                FROM
                                    CP_COURSE_ROOT_?AC_YEAR
                                WHERE
                                    COURSE_CODE = ?COURSE_CODE;";
                        break;

                    }
                case SupportDataSQLCommands.FetchAcademicYearList:
                    {
                        query = @"SELECT 
                                    ACADEMIC_YEAR_ID, AC_YEAR, ACTIVE_YEAR
                                FROM
                                    ACADEMIC_YEAR
                                WHERE
                                    IS_DELETED != 1;";
                        break;

                    }
                case SupportDataSQLCommands.FetchDegree:
                    {
                        query = @"SELECT 
                                                DEGREE_ID,
                                                DEGREE
                                            FROM
                                                SUP_DEGREE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchCourseType:
                    {
                        query = @"SELECT 
                                        COURSE_TYPE_ID, COURSE_TYPE
                                    FROM
                                        CP_COURSE_TYPE_?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchBloodGroup:
                    {
                        query = @"SELECT 
                                                BLOOD_GROUP_ID,
                                                BLOOD_GROUP_NAME
                                            FROM
                                                SUP_BLOOD_GROUP;";
                        break;
                    }
                case SupportDataSQLCommands.FetchCommunity:
                    {
                        query = @"SELECT 
                                                COMMUNITYID,
                                                COMMUNITY
                                            FROM
                                                SUP_COMMUNITY;";
                        break;
                    }
                case SupportDataSQLCommands.FetchGender:
                    {
                        query = @"SELECT 
                                                GENDER_ID,
                                                GENDER_NAME
                                            FROM
                                                SUP_GENDER;";
                        break;
                    }
                case SupportDataSQLCommands.FetchMarritalStatus:
                    {
                        query = @"SELECT 
                                                MARITAL_STAUS_ID,
                                                MARITAL_STATUS_NAME
                                            FROM
                                                SUP_MARRITAL_STATUS;";
                        break;
                    }
                case SupportDataSQLCommands.FetchMotherTongue:
                    {
                        query = @"SELECT 
                                                MOTHER_TONGUE_ID,
                                                MOTHER_TONGUE_NAME
                                            FROM
                                                SUP_MOTHER_TONGUE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchNationality:
                    {
                        query = @"SELECT 
                                                NATIONALITY_ID,
                                                NATIONALITY_NAME
                                            FROM
                                                SUP_NATIONALITY;";
                        break;
                    }
                case SupportDataSQLCommands.FetchDepartment:
                    {
                        query = @"SELECT 
                                                DEPARTMENT_ID,
                                                DEPARTMENT
                                            FROM
                                                CP_DEPARTMENT_?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchReligion:
                    {
                        query = @"SELECT 
                                                RELIGIONID,
                                                RELIGION
                                            FROM
                                                SUP_RELIGION;";
                        break;
                    }
                case SupportDataSQLCommands.FetchLevel:
                    {
                        query = @"SELECT 
                                                LEVEL_ID,
                                                LEVEL
                                            FROM
                                                SUP_LEVEL;";
                        break;
                    }
                case SupportDataSQLCommands.FetchPublishCategory:
                    {
                        query = @"SELECT 
                                                    PUBLISH_CATEGORY_ID,
                                                    PUBLISH_CATEGORY
                                                FROM
                                                    SUP_PUBLISH_CATEGORY;";
                        break;
                    }
                case SupportDataSQLCommands.FetchVolume:
                    {
                        query = @"SELECT 
                                                VOLUME_ID,
                                                VOLUME
                                            FROM
                                                SUP_VOLUME;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSubCategory:
                    {
                        query = @"SELECT 
                                                STF_CATEGORY_ID,
                                                STF_CATEGORY
                                            FROM
                                                SUP_STAFF_CATEGORY;";
                        break;
                    }
                case SupportDataSQLCommands.FetchShift:
                    {
                        query = @"SELECT 
                                        SHIFT_ID,
                                      CONCAT(' ', DESCRIPTION, '    (', TIME, ')') AS SHIFT_NAME
                                    FROM
                                        SUP_SHIFT
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchQualification:
                    {
                        query = @"SELECT 
                                                QUALIFICATION_ID,
                                                QUALIFICATION
                                            FROM
                                                SUP_QUALIFICATION;";
                        break;
                    }
                case SupportDataSQLCommands.FetchDesignation:
                    {
                        query = @"SELECT 
                                                DESIGNATIONID, 
                                                DESIGNATION
                                            FROM
                                                SUP_DESIGNATION
                                            WHERE
                                                ISDELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchRelaion:
                    {
                        query = @"SELECT 
                                                RELATION_ID, RELATION
                                            FROM
                                                SUP_RELATION;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAchievement:
                    {
                        query = @"SELECT 
                                                ACHIEVEMENT_ID,ACHIEVEMENT_NAME
                                            FROM
                                                SUP_ACHIEVEMENT;";
                        break;
                    }
                case SupportDataSQLCommands.Fetchclass:
                    {
                        query = @"SELECT
                                                CLASS_ID,
                                                CLASS_NAME
                                            FROM
                                                CP_CLASSES_?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchBatch:
                    {
                        query = @"SELECT 
                                    BATCH_ID,
                                    BATCH
                                FROM
                                    SUP_BATCHES;";
                        break;
                    }
                case SupportDataSQLCommands.FetchBatchByProgramId:
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
                case SupportDataSQLCommands.FetchProgram:
                    {
                        query = @"SELECT 
                                                PROGRAMME_ID, PROGRAMME_NAME
                                            FROM
                                               CP_PROGRAMME_?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSpokenLanguage:
                    {
                        query = @"SELECT 
                                                    SPOKEN_LANGUAGE_ID, SPOKEN_LANGUAGE_NAME
                                                FROM
                                                    SUP_SPOKEN_LANGUAGE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchLanguage:
                    {
                        query = @"SELECT 
                                                    LANGUAGE_ID,LANGUAGE_NAME
                                                FROM
                                                    SUP_LANGUAGE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSpeciallyAbled:
                    {
                        query = @"SELECT 
                                                    SPECIALLY_ABLED_ID, SPECIALLY_ABLED_NAME
                                                FROM
                                                   SUP_SPECIALLYABLED;";
                        break;
                    }
                case SupportDataSQLCommands.FetchResidencyType:
                    {
                        query = @"SELECT 
                                                RESIDENCY_TYPE_ID, RESIDENCY_TYPE_NAME
                                            FROM
                                                SUP_RESIDENCY_TYPE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchOccupation:
                    {
                        query = @"SELECT 
                                                    OCCUPATION_ID, OCCUPATION_NAME
                                                FROM
                                                    SUP_OCCUPATION;";
                        break;
                    }
                case SupportDataSQLCommands.FetchEducation:
                    {
                        query = @"SELECT 
                                                EDUCATION_ID,EDUCATION_NAME
                                            FROM
                                                SUP_EDUCATION;";
                        break;
                    }
                case SupportDataSQLCommands.FetchActivity:
                    {
                        query = @"SELECT 
                                                    ACTIVITY_ID,ACTIVITY_NAME
                                                FROM
                                                    sup_activity group by ACTIVITY_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchWeekNo:
                    {
                        query = @"SELECT 
                                                    WEEK_NO_ID, WEEK_NAME
                                                FROM
                                                    SUP_WEEK_NO;";
                        break;
                    }
                case SupportDataSQLCommands.FetchPart:
                    {
                        query = @"SELECT 
                                                    PART_ID,
                                                    PART
                                                FROM
                                                    SUP_PART;";
                        break;
                    }
                case SupportDataSQLCommands.FetchTalentActivityType:
                    {
                        query = @"SELECT 
                                                    ACTIVITY_TYPE_ID,
                                                    ACTIVITY_TYPE
                                                FROM
                                                    SUP_ACTIVITY_TYPE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchActivityArea:
                    {
                        query = @"SELECT 
                                                    ACTIVITY_LEVEL_ID,
                                                    ACTIVITY_LEVEL
                                                FROM
                                                    SUP_ACTIVITY_LEVEL;";
                        break;
                    }
                case SupportDataSQLCommands.FetchExamPassed:
                    {
                        query = @"SELECT 
                                                    EXAM_PASSED_ID,
                                                    EXAM_PASSED
                                                FROM
                                                    SUP_EXAMPASSED;";
                        break;
                    }
                case SupportDataSQLCommands.FetchOption:
                    {
                        query = @"SELECT 
                                                    OPTION_ID,
                                                    OPTION_NAME
                                                FROM
                                                    SUP_OPTION;";
                        break;
                    }
                case SupportDataSQLCommands.FetchGrade:
                    {
                        query = @"SELECT 
                                                    OVERALL_GRADE_ID,
                                                    OVERALL_GRADE
                                                FROM
                                                    SUP_OVERALL_GRADE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchCountry:
                    {
                        query = @"SELECT 
                                                    COUNTRY_ID,
                                                    COUNTRY_NAME
                                                FROM
                                                    SUP_COUNTRY;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSemester:
                    {
                        query = @"SELECT 
                                                    SUP_SEMESTER_ID, SEMESTER_NAME
                                                FROM
                                                    CMS.SUP_SEMESTER;";
                        break;
                    }
                //case SupportDataSQLCommands.FetchOptionName:
                //    {
                //        query = @"SELECT 
                //                                    OPTION_ID, OPTION_NAME
                //                                FROM
                //                                    SUP_OPTION_NAME;";
                //        break;
                //    }
                //case SupportDataSQLCommands.FetchIsNME:
                //    {
                //        query = @"SELECT 
                //                                    IS_NME_ID, IS_NME_NAME
                //                                FROM
                //                                    SUP_IS_NME;";
                //        break;
                //    }
                //case SupportDataSQLCommands.FetchIsAllied:
                //    {
                //        query = @"SELECT 
                //                                    IS_ALLIED_ID, IS_ALLIED_NAME
                //                                FROM
                //                                    SUP_IS_ALLIED;";
                //        break;
                //    }
                //case SupportDataSQLCommands.FetchIsCompulsory:
                //    {
                //        query = @"SELECT 
                //                                    IS_COMPULSORY_ID, IS_COMPULSORY_NAME
                //                                FROM
                //                                    SUP_IS_COMPULSORY;";
                //        break;
                //    }
                case SupportDataSQLCommands.FetchUGCCoursetype:
                    {
                        query = @"SELECT 
                                                    UGC_COURSE_TYPE_ID, UGC_COURSE_TYPE
                                                FROM
                                                    CP_UGC_COURSE_TYPE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchPaperCode:
                    {
                        query = @"SELECT 
                                                    PAPER_CODE_ID, PAPER_CODE
                                                FROM
                                                    SUP_PAPER_CODE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchCourse:
                    {
                        query = @"SELECT 
                                                    COURSE_ROOT_ID, COURSE_CODE
                                                FROM
                                                    CP_COURSE_ROOT_?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchStaff:
                    {
                        query = @"SELECT 
                                    STAFF_ID,
                                    STAFF_CODE,
                                    CONCAT(IFNULL(FIRST_NAME, ''),
                                            '',
                                            ' ',
                                            IFNULL(LAST_NAME, ''),
                                            '(',
                                            STAFF_CODE,
                                            ')') AS FIRST_NAME
                                FROM
                                    STF_PERSONAL_INFO
                                WHERE
                                    IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchActiveBatch:
                    {
                        query = @"SELECT 
                                        SBT.BATCH_ID,SBT.BATCH
                                    FROM
                                        ACADEMIC_YEAR AS ACY
                                            INNER JOIN
                                        ACADEMIC_BATCHES AS ACB ON ACB.ACADEMIC_YEAR_ID = ACY.ACADEMIC_YEAR_ID
		                                    INNER JOIN
	                                    SUP_BATCHES AS SBT ON SBT.BATCH_ID=ACB.BATCH_ID WHERE ACY.ACTIVE_YEAR=1";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassType:
                    {
                        query = @"SELECT 
                                        CLASS_TYPE_ID, CLASS_TYPE
                                    FROM
                                        SUP_CLASS_TYPE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchStaffConcat:
                    {
                        query = @"SELECT 
                                        STAFF_ID, CONCAT(IFNULL(FIRST_NAME, ''),
                                                '(',
                                                IFNULL(STAFF_CODE, ''),
                                                ')') AS STAFF_NAME
                                    FROM
                                        STF_PERSONAL_INFO;";
                        break;
                    }
                case SupportDataSQLCommands.FetchNonTeachingCategory:
                    {
                        query = @"SELECT 
                                    NON_TEACHING_CATEGORY_ID,
                                    NON_TEACHING_CATEGORY_NAME
                                FROM
                                    SUP_NON_TEACHING_CATEGORY WHERE IS_DELETE!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchTeachingCategory:
                    {
                        query = @"SELECT 
                                    TEACHING_CATEGORY_ID,
                                    TEACHING_CATEGORY,
                                    IS_ACTIVE,
                                    IS_DELETED
                                FROM
                                    SUP_TEACHING_CATEGORY WHERE IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFaculty:
                    {
                        query = @"SELECT 
                                        FACULTY_ID, FACULTY
                                    FROM
                                        CP_FACULTY_?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassLevel:
                    {
                        query = @"SELECT 
                                        CLASSLEVELID, CLASSLEVEL
                                    FROM
                                        SUP_CLASS_LEVEL WHERE ISDELETED=0;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassYear:
                    {
                        query = @"SELECT 
                                        CLASS_YEAR_ID, CLASS_YEAR
                                    FROM
                                        SUP_CLASS_YEAR
                                    WHERE
                                        IS_DELETED = 0;";
                        break;
                    }
                case SupportDataSQLCommands.FetchNMECourses:
                    {
                        query = @"SELECT 
                                        COURSE_ROOT_ID, COURSE_CODE
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR
                                    WHERE
                                        IS_NME_SUBJECT = 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgrammeLevel:
                    {
                        query = @"SELECT 
                                        PROGRAMME_LEVEL_ID, PROGRAMME_LEVEL
                                    FROM
                                        SUP_PROGRAMME_LEVEL WHERE IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchDurationUnit:
                    {
                        query = @"SELECT 
                                        DURATION_UNIT_ID, DURATION_UNIT
                                    FROM
                                        SUP_DURATION_UNIT WHERE IS_DELETED=0;";
                        break;
                    }
                case SupportDataSQLCommands.FetchChannel:
                    {
                        query = @"SELECT 
                                        CHANNEL_ID, CHANNEL
                                    FROM
                                        SUP_CHANNEL
                                    WHERE
                                        IS_DELETED = 0";
                        break;
                    }
                case SupportDataSQLCommands.FetchSubjects:
                    {
                        query = @"SELECT 
                                        SUBJECT_ID, SUBJECT
                                    FROM
                                        SUP_SUBJECT;";
                        break;
                    }
                case SupportDataSQLCommands.FetchMediumOfInstruction:
                    {
                        query = @"SELECT 
                                        MEDIUM_ID, MEDIUM_OF_INSTRUCTION
                                    FROM
                                        SUP_MEDIUM_OF_INSTRUCTION WHERE IS_DELETED=0;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgrammeType:
                    {
                        query = @"SELECT 
                                        PROGRAMME_TYPE_ID, PROGRAMME_TYPE
                                    FROM
                                        SUP_PROGRAMME_TYPE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAllCourseList:
                    {
                        query = @"SELECT COURSE_ROOT_ID,
                                    CONCAT(IFNULL(COURSE_TITLE,''),'(',IFNULL(COURSE_CODE,''),')') AS COURSE_TITLE 
                                    FROM CP_COURSE_ROOT_?AC_YEAR WHERE IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchOnlyAlliedOptionalCoursesList:
                    {
                        query = @"SELECT 
                                    COURSE_ROOT_ID,
                                    CONCAT(IFNULL(COURSE_TITLE, ''),
                                            '(',
                                            IFNULL(COURSE_CODE, ''),
                                            ')') AS COURSE_TITLE
                                FROM
                                    CP_COURSE_ROOT_?AC_YEAR
                                WHERE
                                    IS_NME_SUBJECT != 1
                                        AND IS_ALLIED_SUBJECT = 1
                                        AND IS_COMPULSORY = 0
                                        AND IS_DELETED != 1";
                        break;
                    }
                case SupportDataSQLCommands.FetchOnlyNMECoursesList:
                    {
                        query = @"SELECT COURSE_ROOT_ID,
                            CONCAT(IFNULL(COURSE_TITLE,''),'(',IFNULL(COURSE_CODE,''),')') AS COURSE_TITLE 
                            FROM CP_COURSE_ROOT_?AC_YEAR WHERE IS_NME_SUBJECT=1 AND IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchOnlyRegularCoursesList:
                    {
                        query = @"SELECT 
                                    COURSE_ROOT_ID,
                                    CONCAT(IFNULL(COURSE_TITLE, ''),
                                            '(',
                                            IFNULL(COURSE_CODE, ''),
                                            ')') AS COURSE_TITLE
                                FROM
                                    CP_COURSE_ROOT_?AC_YEAR
                                WHERE
                                    IS_COMPULSORY = 1 AND IS_DELETED != 1";
                        break;
                    }
                case SupportDataSQLCommands.FetchBoardMmber:
                    {
                        query = @"SELECT 
                                        BOARD_MEMBER_ID, BOARD_MEMBER_NAME
                                    FROM
                                        SUP_BOARD_MEMBER;";
                        break;
                    }
                case SupportDataSQLCommands.FetchNMESetting:
                    {
                        query = @"SELECT 
                                        SETTINGS_ID, SETTINGS_NAME, CLASS_ID
                                    FROM
                                        NME_SETTINGS WHERE IS_DELETED!=1 AND IS_ALLOWED!=0;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassByShiftId:
                    {
                        query = @"SELECT 
                                    CLASS_ID,
                                    CLASS_NAME
                                FROM
                                    CP_CLASSES_?AC_YEAR
                                WHERE
                                    SHIFT = ?SHIFT AND COURSE_ID=0 AND IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchOnlyOrdinaryClass:
                    {
                        query = @"SELECT 
                                    CLASS_ID,
                                    CLASS_CODE
                                FROM
                                    CP_CLASSES_?AC_YEAR
                                WHERE
                                     COURSE_ID=0 AND IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAcademicYearById:
                    {
                        query = @"SELECT 
                                    ACADEMIC_YEAR_ID, AC_YEAR, ACTIVE_YEAR
                                FROM
                                    ACADEMIC_YEAR
                                WHERE
                                    IS_DELETED != 1 AND ACADEMIC_YEAR_ID=?ACADEMIC_YEAR_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgramByShiftId:
                    {
                        query = @"SELECT 
                                        PROGRAMME_ID, PROGRAMME_DESCRIPTION
                                    FROM
                                        CP_PROGRAMME_?AC_YEAR AS PR
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.PROGRAMME = PR.PROGRAMME_ID
                                    WHERE
                                        CLS.SHIFT = ?SHIFT
                                    GROUP BY CLS.PROGRAMME;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassByAcademicYear:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                '(',
                                                IFNULL(CONCAT(' SHIFT - ',SHIFT_NAME), ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                        CP_CLASSES_?AC_YEAR CLS INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=CLS.SHIFT
                                    WHERE
                                        CLS.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchDepartmentByClassId:
                    {
                        query = @"SELECT 
                                                CLASS_ID,
                                                DEPARTMENT
                                            FROM
                                                CP_CLASSES_?AC_YEAR WHERE CLASS_ID=?CLASS_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgrammeAndClassYearByClassId:
                    {
                        query = @"SELECT 
                                        CLASS_YEAR, PROGRAMME
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassByProgrammeAndClassYear:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                '(',
                                                IFNULL(CONCAT(' SHIFT - ',SHIFT_NAME), ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                        CP_CLASSES_?AC_YEAR CLS INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=CLS.SHIFT
                                    WHERE
                                        CLS.CLASS_YEAR = ?CLASS_YEAR AND CLS.PROGRAMME = ?PROGRAMME;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassByClassYear:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                '(',
                                                IFNULL(CONCAT(' SHIFT - ',SHIFT_NAME), ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                        CP_CLASSES_?AC_YEAR CLS INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=CLS.SHIFT
                                    WHERE
                                        CLS.CLASS_YEAR = ?CLASS_YEAR  AND CLS.IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassLevelByClassId:
                    {
                        query = @"SELECT 
                                        CLASS_LEVEL
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        CLASS_ID = ?CLASS_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassByClassLevelAndClassYear:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                '(',
                                                IFNULL(CONCAT(' SHIFT - ',SHIFT_NAME), ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                        CP_CLASSES_?AC_YEAR CLS INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=CLS.SHIFT
                                    WHERE
                                        CLS.CLASS_YEAR = ?CLASS_YEAR AND CLS.CLASS_LEVEL=?CLASS_LEVEL AND CLS.COURSE_ID='' AND CLS.IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchStudentPersonalInfo:
                    {
                        query = @"SELECT 
                                        STUDENT_ID, CONCAT(IFNULL(FIRST_NAME, ''),
                                                '(',
                                                IFNULL(ROLL_NO, ''),
                                                ')') AS FIRST_NAME
                                    FROM
                                        STU_PERSONAL_INFO;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSemesterByCourseId:
                    {
                        query = @"SELECT 
                                        SEMESTER_ID
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR
                                    WHERE
                                        COURSE_ROOT_ID = ?COURSE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchDayOrders:
                    {
                        query = @"SELECT 
                                        DAY_ORDER_ID,DAY
                                    FROM
                                        SUP_DAY_ORDER;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHourType:
                    {
                        query = @"SELECT 
                                        hour_type_id, hourType
                                    FROM
                                        SUP_HOUR_TYPE
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.CheckIsOptionalPaper:
                    {
                        query = @"SELECT 
                                        COURSE_CODE
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR
                                    WHERE
                                        COURSE_ROOT_ID = ?COURSE_ID
                                            AND IS_NME_SUBJECT = 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchPaperType:
                    {
                        query = @"SELECT 
                                        PAPER_TYPE
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR
                                    WHERE
                                        COURSE_ROOT_ID = ?COURSE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchTimeTableSetting:
                    {
                        query = @"SELECT 
                                        SETTING_ID, SETTING_NAME
                                    FROM
                                        TT_TEMPLATE_SETTING;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupFeeMainHead:
                    {
                        query = @"SELECT 
                                        MAIN_HEAD_ID, MAIN_HEAD
                                    FROM
                                        SUP_FEE_MAIN_HEAD
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.SaveSupFeeMainHead:
                    {
                        query = @"INSERT INTO SUP_FEE_MAIN_HEAD(MAIN_HEAD)VALUES(?MAIN_HEAD);";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupFeeMainHeadById:
                    {
                        query = @"SELECT 
                                        MAIN_HEAD_ID, MAIN_HEAD
                                    FROM
                                        SUP_FEE_MAIN_HEAD
                                    WHERE
                                        MAIN_HEAD_ID=?MAIN_HEAD_ID AND IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.UpdateSupFeeMainHead:
                    {
                        query = @"UPDATE SUP_FEE_MAIN_HEAD
                                        SET
                                        MAIN_HEAD = ?MAIN_HEAD
                                        WHERE MAIN_HEAD_ID = ?MAIN_HEAD_ID;";
                        break;
                    }
                case SupportDataSQLCommands.DeleteSupFeeMainHead:
                    {
                        query = @"UPDATE SUP_FEE_MAIN_HEAD
                                        SET
                                        IS_DELETED = 1
                                        WHERE MAIN_HEAD_ID = ?MAIN_HEAD_ID;";
                        break;
                    }
                case SupportDataSQLCommands.IsSupFeeMainHeadExisting:
                    {
                        query = @"SELECT 
                                        MAIN_HEAD_ID
                                    FROM
                                        SUP_FEE_MAIN_HEAD
                                    WHERE
                                        MAIN_HEAD=?MAIN_HEAD AND IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAllPaperType:
                    {
                        query = @"SELECT 
                                        PAPER_TYPE_ID,PAPER_TYPE
                                    FROM
                                        SUP_PAPER_TYPE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAllFrequency:
                    {
                        query = @"SELECT 
                                        FREQUENCY_ID, FREQUENCY_NAME
                                    FROM
                                        SUP_FEE_FREQUENCY
                                    WHERE
                                        IS_DELETED != 1 AND ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAllHostelFrequency:
                    {
                        query = @"SELECT 
                                            FREQUENCY_ID, FREQUENCY_NAME
                                        FROM
                                            SUP_FEE_FREQUENCY
                                        WHERE
                                            IS_DELETED != 1
                                                AND FEE_ROOT_ID IN (2 , 3, 4, 5)
                                                AND ACADEMIC_YEAR = 2021;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAllStudent:
                    {
                        query = @"SELECT 
                                        STUDENT_ID,
                                        CONCAT(IFNULL(FIRST_NAME, ''),
                                                ' (',
                                                REGISTER_NO,
                                                ')') AS FIRST_NAME
                                    FROM
                                        STU_PERSONAL_INFO
                                    WHERE
                                        IS_DELETED != 1 AND IS_LEFT != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchStudentInfoByClassID:
                    {
                        query = @"SELECT 
                                    SP.STUDENT_ID,
                                    CONCAT(IFNULL(FIRST_NAME, ''),
                                            ' (',
                                            REGISTER_NO,
                                            ')') AS FIRST_NAME
                                FROM
                                    STU_PERSONAL_INFO AS SP
                                        INNER JOIN
                                    STU_CLASS AS SCLS ON SCLS.STUDENT_ID = SP.STUDENT_ID
                                WHERE
                                    SP.IS_DELETED != 1 AND IS_LEFT != 1
                                        AND SCLS.CLASS_ID = ?CLASS_ID
                                        AND ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchStudentInfoByStudentId:
                    {
                        query = @"SELECT 
                                        SP.STUDENT_ID,
                                        CONCAT(IFNULL(SP.FIRST_NAME, ''),
                                                IFNULL(SP.LAST_NAME, '')) AS FIRST_NAME,
                                        CLS.CLASS_ID,
                                        CLS.CLASS_CODE,
                                        SP.REGISTER_NO,
                                        SS.SHIFT_NAME
                                    FROM
                                        STU_PERSONAL_INFO AS SP
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.STUDENT_ID = SP.STUDENT_ID
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.CLASS_ID = SC.CLASS_ID
                                            INNER JOIN
                                        SUP_SHIFT AS SS ON SS.SHIFT_ID = SP.SHIFT_ID
                                    WHERE
                                        SP.IS_DELETED != 1 AND SP.IS_LEFT != 1
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR AND SP.STUDENT_ID=?STUDENT_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupProgrammeMode:
                    {
                        query = @"SELECT 
                                        PROGRAMME_MODE_ID, PROGRAMME_MODE
                                    FROM
                                        SUP_PROGRAMME_MODE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupProgrammeModeRegular:
                    {
                        query = @"SELECT 
                                        PROGRAMME_MODE_ID, PROGRAMME_MODE
                                    FROM
                                        SUP_PROGRAMME_MODE WHERE PROGRAMME_MODE_ID=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupProgrammeModeSSC:
                    {
                        query = @"SELECT 
                                        PROGRAMME_MODE_ID, PROGRAMME_MODE
                                    FROM
                                        SUP_PROGRAMME_MODE WHERE PROGRAMME_MODE_ID=2;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupBank:
                    {
                        query = @"SELECT 
                                        BANK_ID,
                                        BANK_NAME,
                                        ADDRESS,
                                        PHONE
                                    FROM
                                        SUP_BANK WHERE IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.DeleteSupBank:
                    {
                        query = @"UPDATE SUP_BANK SET IS_DELETED=1 WHERE BANK_ID=?BANK_ID;";
                        break;
                    }
                case SupportDataSQLCommands.SaveSupBank:
                    {
                        query = @"INSERT INTO SUP_BANK
                                        (
                                        BANK_NAME,
                                        ADDRESS,
                                        PHONE)
                                        VALUES
                                        (
                                        ?BANK_NAME,
                                        ?ADDRESS,
                                        ?PHONE);";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupBankById:
                    {
                        query = @"SELECT 
                                        BANK_ID,
                                        BANK_NAME,
                                        ADDRESS,
                                        PHONE
                                    FROM
                                        SUP_BANK WHERE IS_DELETED!=1 AND BANK_ID=?BANK_ID;";
                        break;
                    }
                case SupportDataSQLCommands.UpdateSupBank:
                    {
                        query = @"UPDATE SUP_BANK
                                            SET
                                            BANK_NAME = ?BANK_NAME,
                                            ADDRESS = ?ADDRESS,
                                            PHONE = ?PHONE
                                            WHERE BANK_ID = ?BANK_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupBankAccount:
                    {
                        query = @"SELECT 
                                        SBA.BANK_ACCOUNT_ID,
                                        SBA.ACCOUNT_PURPOSE,
                                        SBA.PASSBOOK_NO,
                                        SB.BANK_NAME,
                                        DATE_FORMAT(SBA.STARTED_DATE, '%d/%m/%y') AS STARTED_DATE,
                                        DATE_FORMAT(SBA.CLOSED_DATE, '%d/%m/%y') AS CLOSED_DATE
                                    FROM
                                        SUP_BANK_ACCOUNT AS SBA
                                            LEFT JOIN
                                        SUP_BANK AS SB ON SB.BANK_ID = SBA.BANK
                                    WHERE
                                        SBA.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchBankList:
                    {
                        query = @"SELECT 
                                        BANK_ID, BANK_NAME
                                    FROM
                                        SUP_BANK
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.DeleteSupBankAccount:
                    {
                        query = @"UPDATE SUP_BANK_ACCOUNT SET IS_DELETED=1 WHERE BANK_ACCOUNT_ID=?BANK_ACCOUNT_ID; ";
                        break;
                    }
                case SupportDataSQLCommands.SaveSupBankAccount:
                    {
                        query = @"INSERT INTO SUP_BANK_ACCOUNT
                                        (
                                        ACCOUNT_PURPOSE,
                                        BANK,
                                        PASSBOOK_NO,
                                        STARTED_DATE,
                                        CLOSED_DATE)
                                        VALUES
                                        (
                                        ?ACCOUNT_PURPOSE,
                                        ?BANK,
                                        ?PASSBOOK_NO,
                                        ?STARTED_DATE,
                                        ?CLOSED_DATE);";
                        break;
                    }
                case SupportDataSQLCommands.UpdateSupBankAccount:
                    {
                        query = @"UPDATE SUP_BANK_ACCOUNT
                                            SET
                                            BANK_ACCOUNT_ID= ?BANK_ACCOUNT_ID,
                                            ACCOUNT_PURPOSE= ?ACCOUNT_PURPOSE,
                                            BANK= ?BANK,
                                            PASSBOOK_NO= ?PASSBOOK_NO,
                                            STARTED_DATE= ?STARTED_DATE,
                                            CLOSED_DATE= ?CLOSED_DATE
                                            WHERE BANK_ACCOUNT_ID= ?BANK_ACCOUNT_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupBankAcountByAccountId:
                    {
                        query = @"SELECT 
                                        BANK_ID
                                    FROM
                                        SUP_BANK
                                    WHERE
                                        IS_DELETED != 1 AND PASSBOOK_NO=?PASSBOOK_NO;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupBankAcountById:
                    {
                        query = @"SELECT 
                                        ACCOUNT_PURPOSE,
                                        BANK,
                                        PASSBOOK_NO,
                                        DATE_FORMAT(STARTED_DATE, '%d/%m/%Y') AS STARTED_DATE,
                                        DATE_FORMAT(CLOSED_DATE, '%d/%m/%Y') AS CLOSED_DATE
                                    FROM
                                        SUP_BANK_ACCOUNT
                                    WHERE
                                        BANK_ACCOUNT_ID = ?BANK_ACCOUNT_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchExamSetting:
                    {
                        query = @"SELECT 
                                        EXAM_SETTING_ID, EXAM_NAME
                                    FROM
                                        EXAM_SETTING
                                    WHERE
                                        IS_DELETED != 1 AND ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupPaperType:
                    {
                        query = @"SELECT 
                                        PAPER_TYPE_ID, PAPER_TYPE
                                    FROM
                                        SUP_PAPER_TYPE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupSubjectType:
                    {
                        query = @"SELECT 
                                        SUBJECT_TYPE_ID, SUBJECT_TYPE
                                    FROM
                                        SUP_SUBJECT_TYPE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupSplAttendanceType:
                    {
                        query = @"SELECT 
                                    SPL_ATTENDANCE_TYPE_ID, SPL_ATTENDANCE_TYPE
                                FROM
                                    SUP_SPL_ATTENDANCE_TYPE
                                WHERE
                                    IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchOptionalPaperByCourseId:
                    {
                        query = @"SELECT 
                                        COURSE_CODE,COURSE_ROOT_ID AS Course_Id
                                    FROM
                                        CP_COURSE_ROOT_?AC_YEAR
                                    WHERE
                                        COURSE_ROOT_ID = ?COURSE_ID
                                            AND IS_COMPULSORY!= 1;";
                        break;
                    }
                case SupportDataSQLCommands.CheckIsSelectedClass:
                    {
                        query = @"SELECT 
                                        COURSE_ID
                                    FROM
                                        CP_CLASSES_?AC_YEAR
                                    WHERE
                                        CLASS_ID = ?CLASS_ID AND IS_CHOICE = 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSemesterStartDateByClassId:
                    {
                        query = @"SELECT 
                                        date_format(ASE.DATE_FROM,'%Y-%m-%d') as DATE_FROM
                                    FROM
                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.PROGRAMME = ASE.PROGRAMME
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.CLASS_ID = CLS.CLASS_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                    WHERE
                                        ASE.PROGRAMME = CLS.PROGRAMME
                                            AND ASE.BATCH = SP.BATCH
                                            AND IS_ACTIVE = 1
                                            AND CLS.CLASS_ID = 1
                                    GROUP BY SP.BATCH
                                    LIMIT 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSemesterEndDateByClassId:
                    {
                        query = @"SELECT 
                                        date_format(ASE.DATE_TO,'%Y-%m-%d') as DATE_TO
                                    FROM
                                        ACADEMIC_SEMESTER_?AC_YEAR AS ASE
                                            INNER JOIN
                                        CP_CLASSES_?AC_YEAR AS CLS ON CLS.PROGRAMME = ASE.PROGRAMME
                                            INNER JOIN
                                        STU_CLASS AS SC ON SC.CLASS_ID = CLS.CLASS_ID
                                            AND SC.ACADEMIC_YEAR = ?AC_YEAR
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                    WHERE
                                        ASE.PROGRAMME = CLS.PROGRAMME
                                            AND ASE.BATCH = SP.BATCH
                                            AND IS_ACTIVE = 1
                                            AND CLS.CLASS_ID = 1
                                    GROUP BY SP.BATCH
                                    LIMIT 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSubFeeFrequency:
                    {
                        query = @"SELECT 
                                        SF.FREQUENCY_ID,
                                        SF.FREQUENCY_NAME,
                                        SF.ACADEMIC_YEAR,
                                        date_format(SF.DATE_FROM,'%Y/%m/%d') as DATE_FROM,
                                        date_format(SF.DATE_TO,'%Y/%m/%d') as DATE_TO,
                                        date_format(SF.LAST_DATE_OF_PAYMENT,'%Y/%m/%d') as LAST_DATE_OF_PAYMENT,
                                        SFT.FREQUENCY_TYPE
                                    FROM
                                        SUP_FEE_FREQUENCY AS SF
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = SF.TYPE
                                    WHERE
                                        SF.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFrequencytype:
                    {
                        query = @"SELECT 
                                        FREQUENCY_TYPE_ID, FREQUENCY_TYPE
                                    FROM
                                        SUP_FREQUENCY_TYPE
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.SaveSubFeeFrequency:
                    {
                        query = @"INSERT INTO SUP_FEE_FREQUENCY
                                            (
                                            ACADEMIC_YEAR,
                                            FREQUENCY_NAME,
                                            DATE_FROM,
                                            DATE_TO,
                                            LAST_DATE_OF_PAYMENT,
                                            TYPE,
                                            SEMESTER_TYPE)
                                            VALUES
                                            (
                                            ?ACADEMIC_YEAR,
                                            ?FREQUENCY_NAME,
                                            ?DATE_FROM,
                                            ?DATE_TO,
                                            ?LAST_DATE_OF_PAYMENT,
                                            ?TYPE,
                                            ?SEMESTER_TYPE);";
                        break;
                    }
                case SupportDataSQLCommands.FetchSemesterType:
                    {
                        query = @"SELECT 
                                        SEMESTER_TYPE_ID, SEMESTER_TYPE
                                    FROM
                                        SUP_SEMESTER_TYPE
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.CheckIsFrequencyExisting:
                    {
                        query = @"SELECT 
                                        FREQUENCY_ID
                                    FROM
                                        SUP_FEE_FREQUENCY
                                    WHERE
                                        TYPE = ?TYPE AND SEMESTER_TYPE = ?SEMESTER_TYPE
                                            AND FREQUENCY_NAME = ?FREQUENCY_NAME AND ACADEMIC_YEAR=?ACADEMIC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupFeeFrequencyById:
                    {
                        query = @"SELECT 
                                        FREQUENCY_ID,
                                        ACADEMIC_YEAR,
                                        FREQUENCY_NAME,
                                        date_format(DATE_FROM,'%d/%m/%Y') as DATE_FROM,
	                                    date_format(DATE_TO,'%d/%m/%Y') as DATE_TO,
	                                    date_format(LAST_DATE_OF_PAYMENT,'%d/%m/%Y') as LAST_DATE_OF_PAYMENT,
                                        FREQUENCY_TYPE_ID AS FREQUENCY_TYPE,
                                        SEMESTER_TYPE
                                    FROM
                                        SUP_FEE_FREQUENCY
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = TYPE
                                    WHERE
                                        FREQUENCY_ID = ?FREQUENCY_ID;";
                        break;
                    }
                case SupportDataSQLCommands.UpdateSupFeeFrequency:
                    {
                        query = @"UPDATE SUP_FEE_FREQUENCY
                                            SET
                                            FREQUENCY_NAME= ?FREQUENCY_NAME,
                                            DATE_FROM= ?DATE_FROM,
                                            DATE_TO= ?DATE_TO,
                                            LAST_DATE_OF_PAYMENT= ?LAST_DATE_OF_PAYMENT,
                                            TYPE= ?TYPE,
                                            SEMESTER_TYPE= ?SEMESTER_TYPE
                                            WHERE FREQUENCY_ID=?FREQUENCY_ID;";
                        break;
                    }
                case SupportDataSQLCommands.DeleteSupFeeFrequency:
                    {
                        query = @"UPDATE SUP_FEE_FREQUENCY
                                            SET
                                            IS_DELETED=1 WHERE FREQUENCY_ID=?FREQUENCY_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupFrequency:
                    {
                        query = @"SELECT 
                                        FREQUENCY_ID, FREQUENCY_NAME
                                    FROM
                                        SUP_FEE_FREQUENCY
                                    WHERE
                                        IS_DELETED != 1 AND ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchDiscount:
                    {
                        query = @"SELECT 
                                        DISCOUNT_ID, DISCOUNT_NAME
                                    FROM
                                        FEE_DISCOUNT
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFeeCategory:
                    {
                        query = @"SELECT 
                                        FEE_CATEGORY_ID, FEE_CATEGORY
                                    FROM
                                        SUP_FEE_CATEGORY
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassByProgrammeId:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                '(',
                                                IFNULL(CONCAT(' SHIFT - ',SHIFT_NAME), ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                        CP_CLASSES_?AC_YEAR CLS INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=CLS.SHIFT
                                    WHERE
                                         CLS.PROGRAMME = ?PROGRAMME;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHeadByFeeCategory:
                    {
                        query = @"SELECT 
                                        HEAD_ID, HEAD
                                    FROM
                                        SUP_HEAD
                                    WHERE
                                        IS_DELETED != 1 AND FEE_CATEGORY = ?FEE_CATEGORY;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFrequencyByType:
                    {
                        query = @"SELECT 
                                        FREQUENCY_ID, FREQUENCY_NAME
                                    FROM
                                        SUP_FEE_FREQUENCY 
                                    WHERE
                                        IS_DELETED != 1 AND ACADEMIC_YEAR = ?AC_YEAR AND TYPE=?TYPE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchStudentInfoByClassIdAndDiscountId:
                    {
                        query = @"SELECT 
                                    SP.STUDENT_ID,
                                    CONCAT(IFNULL(FIRST_NAME, ''),
                                            ' (',
                                            REGISTER_NO,
                                            ')') AS FIRST_NAME,IF(FDA.DISCOUNT_ALLOTMENT_ID IS NOT NULL,'SELECTED','' ) AS STATUS
                                FROM
                                    STU_PERSONAL_INFO AS SP
                                        INNER JOIN
                                    STU_CLASS AS SCLS ON SCLS.STUDENT_ID = SP.STUDENT_ID
									LEFT JOIN FEE_DISCOUNT_ALLOTMENT AS FDA ON FDA.STUDENT_ID=SCLS.STUDENT_ID AND FDA.IS_DELETED!=1 AND FDA.DISCOUNT_ID=?DISCOUNT_ID
                                WHERE
                                    SP.IS_DELETED != 1 AND IS_LEFT != 1
                                        AND SCLS.CLASS_ID = ?CLASS_ID
                                        AND ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFrequencyByTypeAndDiscountId:
                    {
                        query = @"SELECT 
                                        SFF.FREQUENCY_ID,
                                        SFF.FREQUENCY_NAME,
                                        IF(FDF.DISCOUNT_ID IS NOT NULL,
                                            'SELECTED',
                                            '') AS STATUS
                                    FROM
                                        SUP_FEE_FREQUENCY AS SFF
                                            LEFT JOIN
                                        FEE_DISCOUNT_FREQUENCY AS FDF ON FDF.FREQUENCY_ID = SFF.FREQUENCY_ID
                                            AND FDF.DISCOUNT_ID = ?DISCOUNT_ID AND FDF.IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFrequencyByDiscountId:
                    {
                        query = @"SELECT 
                                        SFF.FREQUENCY_ID, SFF.FREQUENCY_NAME
                                    FROM
                                        SUP_FEE_FREQUENCY AS SFF
                                            INNER JOIN
                                        FEE_DISCOUNT_FREQUENCY AS FDF ON FDF.FREQUENCY_ID = SFF.FREQUENCY_ID
                                    WHERE
                                        FDF.DISCOUNT_ID = ?DISCOUNT_ID
                                            AND FDF.IS_DELETED != 1
                                            AND SFF.IS_DELETED != 1
                                            AND SFF.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHSSGroups:
                    {
                        query = @"SELECT 
                                        HSS_GROUP_ID, HSS_GROUP, HSS_GROUP_CODE
                                    FROM
                                        SUP_HSS_GROUPS
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHssSubjectsByHSSGroup:
                    {
                        query = @"SELECT 
                                        AHG.GRPSUB_ID,
                                        AHG.HSS_GROUP,
                                        ACADEMIC_YEAR,
                                        HSS_SUBJECT_ID AS SUBJECT_ID,
                                        SHS.HSS_SUBJECT,
                                        SHS.PART
                                    FROM
                                        ADM_HSS_GROUP_SUBJECTS AS AHG
                                            INNER JOIN
                                        SUP_HSS_SUBJECTS AS SHS ON SHS.HSS_SUBJECT_ID = AHG.HSS_SUBJECT
                                    WHERE
                                        AHG.HSS_GROUP = ?HSS_GROUP
                                            AND AHG.IS_DELETED != 1
                                          -- AND AHG.ACADEMIC_YEAR = ?ACADEMIC_YEAR
                                    ORDER BY PART,HSS_SUBJECT;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHssSubjectsByReceiveId:
                    {
                        query = @"SELECT 
                                    MARK_ID,
                                    RECEIVE_STUID,
                                    MAX_MARK,
                                    MARK,
                                    M_PASS,
                                    ASU.PROGRAMME_ID,
                                    ASU.ACADEMIC_YEAR,
                                    SHS.HSS_SUBJECT_ID AS SUBJECT_ID,
                                    SHS.HSS_SUBJECT,
                                    ASU.NO_OF_ATTEMPTS,
                                    SHS.PART,
                                    REGISTER_NO
                                FROM
                                    ADM_STU_SUBMARKS AS ASU
                                        INNER JOIN
                                    SUP_HSS_SUBJECTS AS SHS ON SHS.HSS_SUBJECT_ID = ASU.SUBJECT_ID
                                WHERE
                                    ASU.RECEIVE_STUID = ?RECEIVE_STUID and ACADEMIC_YEAR=?AC_YEAR
                                        AND ASU.IS_DELETED != 1
                                    ORDER BY SHS.PART,MARK_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHssSubjectsFor11ThByReceiveId:
                    {
                        query = @"SELECT 
                                    MARK_ID,
                                    RECEIVE_STUID,
                                    MAX_MARK,
                                    MARK,
                                    M_PASS,
                                    ASU.PROGRAMME_ID,
                                    ASU.ACADEMIC_YEAR,
                                    SHS.HSS_SUBJECT_ID AS SUBJECT_ID,
                                    SHS.HSS_SUBJECT,
                                    ASU.NO_OF_ATTEMPTS,
                                    SHS.PART,
                                    REGISTER_NO
                                FROM
                                    ADM_STU_SUBMARKS_FOR_11TH AS ASU
                                        INNER JOIN
                                    SUP_HSS_SUBJECTS AS SHS ON SHS.HSS_SUBJECT_ID = ASU.SUBJECT_ID
                                WHERE
                                    ASU.RECEIVE_STUID = ?RECEIVE_STUID and ACADEMIC_YEAR=?AC_YEAR
                                        AND ASU.IS_DELETED != 1
                                        ORDER BY SHS.PART,MARK_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFileUploadsCount:
                    {
                        query = @"SELECT 
                                        COUNT(RECEIVE_STU_ID) AS FILE_COUNT
                                    FROM
                                        ADM_UPLOADED_FILES
                                    WHERE
                                        RECEIVE_STU_ID = ?RECEIVE_STU_ID and ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSelectionCycle:
                    {
                        query = @"SELECT 
                                        SELECTION_CYCLE_ID, SELECTION_CYCLE
                                    FROM
                                        SUP_SELECTION_CYCLE
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchApplicantType:
                    {
                        query = @"SELECT 
                                        APPLICANT_TYPE_ID, APPLICANT_TYPE
                                    FROM
                                        SUP_APPLICANT_TYPE
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgrammeInchargeByStaffId:
                    {
                        query = @"SELECT 
                                        APP.PROGRAMME_GROUP_ID,
                                        CP.PROGRAMME_ID,
                                        APP.SHIFT,
                                        APP.PROGRAMME_MODE,
                                        CONCAT(IFNULL(CP.PROGRAMME_NAME, ''),
                                                ' (',
                                                IFNULL(CONCAT(PM.PROGRAMME_MODE), ''),
                                                ')') AS PROGRAMME_NAME
                                    FROM
                                        ADM_PROGRAMME_INCHARGE AS API
                                            INNER JOIN
                                        cp_programme_group AS APP ON APP.PROGRAMME_GROUP_ID = API.PROGRAMME_GROUP_ID
                                            INNER JOIN
                                        CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = APP.PROGRAMME_ID
                                            INNER JOIN
                                        SUP_SHIFT SS ON SS.SHIFT_ID = APP.SHIFT
                                            AND SS.IS_DELETED != 1
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = APP.PROGRAMME_MODE
                                    WHERE
                                        API.STAFF_ID = ?STAFF_ID AND API.IS_ACTIVE = 1
                                            AND API.ACADEMIC_YEAR = ?AC_YEAR
                                            AND API.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFrequencyBySemesterType:
                    {
                        query = @"SELECT 
                                    FREQUENCY_ID,FREQUENCY_NAME
                                FROM
                                   SUP_FEE_FREQUENCY AS FF
                                        INNER JOIN
                                    SUP_SEMESTER_TYPE AS ST ON ST.SEMESTER_TYPE_ID = FF.SEMESTER_TYPE
                                        AND ST.IS_ACTIVE = 1 WHERE FF.IS_DELETED!=1 AND FF.ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchADMClass:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                '(',
                                                IFNULL(CONCAT(' SHIFT - ',SHIFT_NAME), ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                        ADM_CLASSES CLS INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=CLS.SHIFT
                                    WHERE
                                        CLS.IS_DELETED != 1 ";
                        break;
                    }
                case SupportDataSQLCommands.FetchHostel:
                    {
                        query = @"SELECT HOSTEL_ID,HOSTEL_NAME,HOSTEL_CODE FROM SUP_HOSTEL WHERE IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchUniversity:
                    {
                        query = @"SELECT 
                                        UNIVERSITY_ID, UNIVERSITY
                                    FROM
                                        SUP_UNIVERSITY
                                    WHERE
                                        IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchState:
                    {
                        query = @"SELECT 
                                        STATE_ID, STATE
                                    FROM
                                        SUP_STATE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchMainCommunity:
                    {
                        query = @"SELECT 
                                MAIN_COMMUNITY_ID, MAIN_COMMUNITY
                            FROM
                                SUP_MAIN_COMMUNITY
                            WHERE
                                IS_DELETED != 1
                            ORDER BY ORDER_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHostelByHostelIds:
                    {
                        query = @"SELECT 
                                        HOSTEL_ID, HOSTEL_NAME, HOSTEL_CODE
                                    FROM
                                        SUP_HOSTEL
                                    WHERE
                                        HOSTEL_ID IN (?HOSTEL_ID);";
                        break;
                    }
                case SupportDataSQLCommands.FetchSalutation:
                    {
                        query = @"SELECT 
                                    SALUTATION_ID,
                                    SALUTATION
                                FROM
                                    SUP_SALUTATION;";
                        break;
                    }
                case SupportDataSQLCommands.FectchFeeRoot:
                    {
                        query = @"SELECT FEE_ROOT_ID, FEE_ROOT FROM FEE_ROOT WHERE IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchApplicationType:
                    {
                        query = @"SELECT 
                                APPLICATION_TYPE_ID, APPLICATION_TYPE, PREFIX, SUFFIX
                            FROM
                                SUP_APPLICATION_TYPE
                            WHERE
                                IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupBankAccounts:
                    {
                        query = @"SELECT 
                                        BANK_ACCOUNT_ID,
                                        CONCAT(IFNULL(SBA.PASSBOOK_NO, ''),
                                                '(',
                                                IFNULL(SB.BANK_NAME, ''),
                                                ')') AS PASSBOOK_NO
                                    FROM
                                        sup_bank_account AS SBA
                                            INNER JOIN
                                        sup_bank AS SB ON SB.BANK_ID = SBA.BANK
                                    WHERE
                                        SB.IS_DELETED != 1 AND SBA.IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupBankAccountsProgrammewise:
                    {
                        query = @"SELECT 
                                        BANK_ACCOUNT_ID,
                                        CONCAT(IFNULL(SBA.PASSBOOK_NO, ''),
                                                '(',
                                                IFNULL(SB.BANK_NAME, ''),
                                                ')') AS PASSBOOK_NO
                                    FROM
                                        sup_bank_account AS SBA
                                            INNER JOIN
                                        sup_bank AS SB ON SB.BANK_ID = SBA.BANK
                                    WHERE
                                        SBA.BANK_ACCOUNT_ID IN (?BANK_ACCOUNT_ID) AND 
                                        SB.IS_DELETED != 1 AND SBA.IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgrammeGroupProgramme:
                    {
                        query = @"SELECT 
                                        AP.PROGRAMME_GROUP_ID,
                                        P.PROGRAMME_ID,
                                        CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                                ' (SHIFT - ',
                                                IFNULL(SH.SHIFT_NAME, ''),
                                                ') ',
                                                IFNULL(SPM.PROGRAMME_MODE, '')) AS PROGRAMME_NAME
                                    FROM
                                        CP_PROGRAMME_GROUP AS AP
                                            INNER JOIN
                                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = AP.PROGRAMME_ID
                                            AND P.IS_DELETED != 1
                                            INNER JOIN
                                        SUP_SHIFT SH ON SH.SHIFT_ID = AP.SHIFT
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                                    WHERE
                                        AP.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupFeeFrequencyByFeeFrequencyFeeModeAndFeeRootId:
                    {
                        query = @"SELECT 
                                        FFF.FEE_FREQUENCY_FEE_MODE_ID AS FREQUENCY_ID,
                                        CONCAT(IFNULL(SFF.FREQUENCY_NAME, ''),
                                        '( ',
                                        IFNULL(SFT.FREQUENCY_TYPE, ''),'-',IFNULL(FFF.ACADEMIC_YEAR,''),
                                        ')') AS FREQUENCY_NAME,
                                        FFF.FEE_MODE
                                    FROM
                                        FEE_FREQUENCY_FEE_MODE AS FFF
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE AND SFT.FEE_ROOT_ID in(?FEE_ROOT_ID)
                                            LEFT JOIN
                                        SUP_SEMESTER_TYPE AS SST ON SST.SEMESTER_TYPE_ID = SFF.SEMESTER_TYPE
                                            AND SST.IS_ACTIVE = 1
                                    WHERE
                                        FFF.ACADEMIC_YEAR = ?AC_YEAR
                                            AND SFT.IS_DELETED != 1
                                            AND SFF.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassByProgrammeGroupId:
                    {
                        query = @"SELECT 
                                        CLASS_ID,
                                        CONCAT(IFNULL(CLS.CLASS_NAME, ''),
                                                '(',
                                                IFNULL(CONCAT(' SHIFT - ',SHIFT_NAME), ''),
                                                ')') AS CLASS_NAME
                                    FROM
                                        CP_CLASSES CLS INNER JOIN SUP_SHIFT AS SH ON SH.SHIFT_ID=CLS.SHIFT
                                    WHERE
                                         CLS.PROGRAMME_GROUP_ID IN(?PROGRAMME_GROUP_ID);";
                        break;
                    }
                case SupportDataSQLCommands.FetchMainHeadsByFrequencyIdForMessAndHostel:
                    {
                        query = @"SELECT 
                                    MH.FEE_MAIN_HEAD_ID,MH.MAIN_HEAD_ID,SMH.MAIN_HEAD
                                FROM
                                    FEE_MAIN_HEADS AS MH
                                        INNER JOIN
                                    SUP_FEE_MAIN_HEAD AS SMH ON SMH.MAIN_HEAD_ID = MH.MAIN_HEAD_ID
                                WHERE
                                    MH.ACADEMIC_YEAR = ?AC_YEAR
                                        AND FREQUENCY_ID = ?FREQUENCY_ID GROUP BY MH.MAIN_HEAD_ID";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupStatus:
                    {
                        query = @"SELECT 
                            STATUS_ID, `STATUS`
                        FROM
                            SUP_STATUS
                        WHERE
                            IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchElevathMark:
                    {
                        query = @"SELECT 
                                MARK_ID,
                                RECEIVE_STUID,
                                MAX_MARK,
                                MARK,
                                M_PASS,
                                ASU.PROGRAMME_ID,
                                ASU.ACADEMIC_YEAR,
                                SHS.HSS_SUBJECT_ID AS SUBJECT_ID,
                                SHS.HSS_SUBJECT,
                                ASU.NO_OF_ATTEMPTS,
                                SHS.PART
                            FROM
                                ADM_STU_SUBMARKS_FOR_11TH AS ASU
                                    INNER JOIN
                                SUP_HSS_SUBJECTS AS SHS ON SHS.HSS_SUBJECT_ID = ASU.SUBJECT_ID
                            WHERE
		                            ASU.RECEIVE_STUID = ?RECEIVE_STUID
                                    AND ACADEMIC_YEAR = ?AC_YEAR
                                    AND ASU.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFeeFrequencyByAcademicyear:
                    {
                        query = @"SELECT 
                                FEE_FREQUENCY_FEE_MODE_ID AS FREQUENCY_ID,
                                SFF.FREQUENCY_NAME,
                                SFT.FREQUENCY_TYPE AS FEE_NAME
                            FROM
                                FEE_FREQUENCY_FEE_MODE AS FFF
                                    INNER JOIN
                                SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                    INNER JOIN
                                SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                            WHERE
                                SFF.IS_DELETED != 1
                                    AND FFF.ACADEMIC_YEAR = ?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchFeeFrequency:
                    {
                        query = @"SELECT 
                                        FEE_FREQUENCY_FEE_MODE_ID AS FREQUENCY_ID,
                                        CONCAT(FREQUENCY_NAME,' (',SFT.FREQUENCY_TYPE,')') AS FREQUENCY_NAME,
                                        SFT.FREQUENCY_TYPE
                                    FROM
                                        FEE_FREQUENCY_FEE_MODE AS FFF
                                            INNER JOIN
                                        SUP_FEE_FREQUENCY AS SFF ON SFF.FREQUENCY_ID = FFF.FREQUENCY_ID
                                            INNER JOIN
                                        SUP_FREQUENCY_TYPE AS SFT ON SFT.FREQUENCY_TYPE_ID = FFF.FEE_MODE
                                    WHERE
                                         SFF.IS_DELETED != 1 AND FFF.ACADEMIC_YEAR=?AC_YEAR;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgrammeGroupByShiftAndProgrammeMode:
                    {
                        query = @"SELECT 
                                        AP.PROGRAMME_GROUP_ID,
                                        P.PROGRAMME_ID,                                       
                                        CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                                ' (SHIFT - ',
                                                IFNULL(SH.SHIFT_NAME, ''),
                                                ') ',
                                                IFNULL(SPM.PROGRAMME_MODE, '')) AS PROGRAMME_NAME
                                    FROM
                                        CP_PROGRAMME_GROUP AS AP
                                            INNER JOIN
                                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = AP.PROGRAMME_ID
                                            AND P.IS_DELETED != 1
                                            INNER JOIN
                                        SUP_SHIFT SH ON SH.SHIFT_ID = AP.SHIFT
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                                    WHERE
                                        AP.IS_DELETED != 1 AND AP.PROGRAMME_MODE=?PROGRAMME_MODE AND SHIFT=?SHIFT ORDER BY APPTYPE_ID;";
                        break;
                    }

                case SupportDataSQLCommands.FetchProgrammeGroupByAppType:
                    {
                        query = @"SELECT 
                                        AP.PROGRAMME_GROUP_ID,
                                        P.PROGRAMME_ID,                                       
                                        CONCAT(IFNULL(P.PROGRAMME_NAME, ''),
                                                ' (SHIFT - ',
                                                IFNULL(SH.SHIFT_NAME, ''),
                                                ') ',
                                                IFNULL(SPM.PROGRAMME_MODE, '')) AS PROGRAMME_NAME
                                    FROM
                                        CP_PROGRAMME_GROUP AS AP
                                            INNER JOIN
                                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = AP.PROGRAMME_ID
                                            AND P.IS_DELETED != 1
                                            INNER JOIN
                                        SUP_SHIFT SH ON SH.SHIFT_ID = AP.SHIFT
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                                    WHERE
                                        AP.IS_DELETED != 1 AND AP.APPTYPE_ID=?APPTYPE_ID  ORDER BY PROGRAMME_GROUP_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchStudentsForNewDateReport:
                    {
                        query = @"SELECT 
                                FT.STUDENT_ID,    
                                AI.ISSUED_ID,
                                CONCAT(AI.APPLICATION_NO,LPAD(AI.ISSUE_NO,4,'0')) AS APPLICATION_NO,                      
                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                PG.PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                        ' (',
                                        PM.PROGRAMME_MODE,
                                        ' ',
                                        SS.SHIFT_NAME,
                                        ')') AS PROGRAMME_NAME,
                                DATE_FORMAT(FT.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                                PY.PAYMENT_MODE,from_unixtime(FC.SETTLEMENT_DATE,'%d/%m/%Y') AS SETTLEMENT_DATE
                            FROM
                                FEE_TRANSACTION AS FT
                                    INNER JOIN
                                FEE_COLLECTION AS FC ON FC.TRANSACTION_ID = FT.TRANSACTION_ID
                                    INNER JOIN
                                FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PT ON PT.RAZORPAY_PAMENT_ID = FT.RAZORPAY_ID
                                    INNER JOIN
                                FEE_RAZORPAY_ORDER_INFO_?AC_YEAR AS PO ON PO.ID = PT.ORDER_ID
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = PO.UDF6
                                    AND PG.PROGRAMME_MODE = ?PROGRAMME_MODE		                            
                                    AND PG.SHIFT = ?SHIFT
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                SUP_PAYMENT_MODE AS PY ON PY.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                                    INNER JOIN
                                ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = FT.STUDENT_ID
                                    LEFT JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    AND AI.PROGRAMME_GROUP_ID = PO.UDF6
                            WHERE
                                FT.FREQUENCY = ?FREQUENCY AND AI.PROGRAMME_GROUP_ID in (?PROGRAMME_GROUP_ID)
                                    AND FC.SETTLEMENT_DATE BETWEEN UNIX_TIMESTAMP(?DATE_FROM) AND UNIX_TIMESTAMP(ADDDATE(?DATE_TO, 1))
                            GROUP BY FT.TRANSACTION_ID  order by PG.APPTYPE_ID,PG.PROGRAMME_GROUP_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchStudentByProgrammeGroupIdForDateReport:
                    {
                        query = @"SELECT 
                                FT.STUDENT_ID,
                                IF(SL.IS_CANCELED = 1,
                                    CONCAT(SL.APPLICATION_NO, '- CANCELED'),
                                    SL.APPLICATION_NO) AS APPLICATION_NO,
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
                                SL.IS_DELETED != 1
                            ORDER BY FT.PAYMENT_DATE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAppliedStudentByProgrammeModeAndShift:
                    {
                        query = @"SELECT 
                            FT.STUDENT_ID,
                            AI.ISSUED_ID,
                            CONCAT(AI.APPLICATION_NO, LPAD(AI.ISSUED_ID,4,'0')) AS APPLICATION_NO,
                            UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                            PG.PROGRAMME_GROUP_ID,
                            CONCAT(CP.PROGRAMME_NAME,
                                    ' (',
                                    PM.PROGRAMME_MODE,
                                    ' ',
                                    SS.SHIFT_NAME,
                                    ')') AS PROGRAMME_NAME,
                            DATE_FORMAT(FT.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,
                            PY.PAYMENT_MODE
                        FROM
                            FEE_TRANSACTION AS FT
                                INNER JOIN
                            FEE_RAZORPAY_PAYMENT_INFO_?AC_YEAR AS PT ON PT.RAZORPAY_PAMENT_ID = FT.RAZORPAY_ID     
                                INNER JOIN
                            ADM_ISSUED_APPLICATIONS AS AI ON AI.RAZORPAY_ID = FT.RAZORPAY_ID
                                INNER JOIN
                            ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                INNER JOIN
                            CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = PT.UDF6
                                AND PG.SHIFT = ?SHIFT
                                AND PG.PROGRAMME_MODE = ?PROGRAMME_MODE
                                AND PG.APPTYPE_ID = ?APPTYPE_ID
                                INNER JOIN
                            CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                INNER JOIN
                            SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                INNER JOIN
                            SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                INNER JOIN
                            SUP_PAYMENT_MODE AS PY ON PY.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                        WHERE
                            AR.IS_DELETED != 1 AND FT.FREQUENCY = ?FREQUENCY
                                AND PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO ORDER BY PG.APPTYPE_ID,PG.PROGRAMME_GROUP_ID ;";

                        break;
                    }
                case SupportDataSQLCommands.FetchAdmittedStudentByProgrammeModeAndShift:
                    {

                        query = @"SELECT 
                                FT.STUDENT_ID,
                                AI.ISSUED_ID,                             
                                CONCAT(AI.APPLICATION_NO, AI.ISSUED_ID) AS APPLICATION_NO,
                                UPPER(AR.FIRST_NAME) AS FIRST_NAME,
                                PG.PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                        ' (',
                                        PM.PROGRAMME_MODE,
                                        ' ',
                                        SS.SHIFT_NAME,
                                        ')') AS PROGRAMME_NAME,
                                DATE_FORMAT(FT.PAYMENT_DATE, '%d/%m/%Y') AS PAYMENT_DATE,                               
                                PY.PAYMENT_MODE
                            FROM
                                ADM_RECEIVE_APPLICATION AS AR
                                    INNER JOIN
                                ADM_ISSUED_APPLICATIONS AS AI ON AI.RECEIVE_ID = AR.RECEIVE_ID
                                    AND AI.STATUS = 5
                                    INNER JOIN
                                CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = AI.PROGRAMME_GROUP_ID
                                    AND PG.SHIFT = ?SHIFT
                                    AND PG.PROGRAMME_MODE = ?PROGRAMME_MODE
                                    AND PG.APPTYPE_ID=?APPTYPE_ID
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = PG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = PG.PROGRAMME_MODE
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = PG.SHIFT
                                    INNER JOIN
                                FEE_TRANSACTION AS FT ON FT.STUDENT_ID = AR.RECEIVE_ID
                                  --   AND FT.RAZORPAY_ID = AI.RAZORPAY_ID
                                    AND FT.FREQUENCY = ?FREQUENCY
                                    AND PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
                                    INNER JOIN
                                SUP_PAYMENT_MODE AS PY ON PY.PAYMENT_MODE_ID = FT.PAYMENT_MODE
                                WHERE AR.IS_DELETED!=1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHeadsByFeeRootIdAndProgrammeGroupId:
                    {
                        query = @"SELECT 
	                                    MH.MAIN_HEAD_ID,
                                        MH.MAIN_HEAD,
                                        H.HEAD_ID,
                                        H.HEAD
                                    FROM
                                        FEE_MAIN_HEADS AS FMH
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
		                                    INNER JOIN
	                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_MODE=FMH.PROGRAMME_MODE
                                            AND PG.SHIFT=FMH.SHIFT
                                            AND PG.APPTYPE_ID=FMH.APPLICATION_TYPE_ID
                                            AND PG.IS_DELETED!=1
                                    WHERE
                                        FEE_ROOT_ID = ?FEE_ROOT_ID
                                        AND PG.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                                        AND FREQUENCY_ID = ?FREQUENCY AND FMH.IS_DELETED!=1 
                                        GROUP BY MH.MAIN_HEAD_ID,H.HEAD_ID ORDER BY MH.MAIN_HEAD_ID";
                        break;
                    }
                case SupportDataSQLCommands.FetchHeadsForHostelFee:
                    {
                        query = @"SELECT 
                                    MH.MAIN_HEAD_ID, MH.MAIN_HEAD, H.HEAD_ID, H.HEAD
                                FROM
                                    FEE_MAIN_HEADS AS FMH
                                        INNER JOIN
                                    SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                        INNER JOIN
                                    SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
                                WHERE
                                    FEE_ROOT_ID = ?FEE_ROOT_ID AND FREQUENCY_ID = ?FREQUENCY
                                        AND FMH.IS_DELETED != 1
                                GROUP BY MH.MAIN_HEAD_ID , H.HEAD_ID
                                ORDER BY MH.MAIN_HEAD_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHeadsForApplicationFee:
                    {
                        query = @"SELECT 
	                                    MH.MAIN_HEAD_ID,
                                        MH.MAIN_HEAD,
                                        H.HEAD_ID,
                                        H.HEAD
                                    FROM
                                        FEE_MAIN_HEADS AS FMH
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
		                                    INNER JOIN
	                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_MODE=FMH.PROGRAMME_MODE
                                            AND PG.SHIFT=FMH.SHIFT
                                            AND PG.APPTYPE_ID=FMH.APPLICATION_TYPE_ID
                                            AND PG.IS_DELETED!=1
                                    WHERE
                                        FEE_ROOT_ID = ?FEE_ROOT_ID                                       
                                        AND FREQUENCY_ID = ?FREQUENCY AND FMH.IS_DELETED!=1 
                                        GROUP BY MH.MAIN_HEAD_ID,H.HEAD_ID ORDER BY MH.MAIN_HEAD_ID";
                        break;
                    }
                case SupportDataSQLCommands.FetchStudentByProgrammeGroupId:
                    {
                        query = @"SELECT 
                                        SP.STUDENT_ID,
                                        SP.ROLL_NO,
                                        C.CLASS_ID,
                                        UPPER(SP.FIRST_NAME) AS FIRST_NAME,
                                        PG.PROGRAMME_GROUP_ID,
                                        P.PROGRAMME_NAME,
                                        UPPER(C.CLASS_NAME) AS CLASS,
                                        DATE_FORMAT(T.PAYMENT_DATE,'%d/%m/%Y') AS PAYMENT_DATE
                                    FROM
                                        STU_CLASS AS SC
                                            INNER JOIN
                                        CP_CLASSES AS C ON C.CLASS_ID = SC.CLASS_ID
                                            INNER JOIN
                                        STU_PERSONAL_INFO AS SP ON SP.STUDENT_ID = SC.STUDENT_ID
                                            AND SP.IS_DELETED != 1
                                            AND SP.IS_LEFT != 1
                                            INNER JOIN
                                        CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_GROUP_ID = C.PROGRAMME_GROUP_ID
                                            AND PG.IS_DELETED != 1
                                            INNER JOIN
                                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = PG.PROGRAMME_ID
                                            INNER JOIN
										FEE_TRANSACTION AS T ON T.STUDENT_ID=SC.STUDENT_ID
                                            AND T.FREQUENCY=?FREQUENCY
                                            AND T.PAYMENT_DATE BETWEEN ?DATE_FROM AND ?DATE_TO
                                    WHERE
                                        SC.ACADEMIC_YEAR = ?ACADEMIC_YEAR 
                                        AND PG.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID)
                                        AND C.CLASS_YEAR=?CLASS_YEAR
                                        AND SC.IS_DELETED != 1 ORDER BY T.PAYMENT_DATE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHeadsByFeeRootIdAndProgrammeGroupIdAndFeeCategory:
                    {
                        query = @"SELECT 
	                                    MH.MAIN_HEAD_ID,
                                        MH.MAIN_HEAD,
                                        H.HEAD_ID,
                                        H.HEAD
                                    FROM
                                        FEE_MAIN_HEADS AS FMH
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                            INNER JOIN
                                        SUP_HEAD AS H ON H.HEAD_ID = FMH.HEAD_ID
		                                    INNER JOIN
	                                    CP_PROGRAMME_GROUP AS PG ON PG.PROGRAMME_MODE=FMH.PROGRAMME_MODE
                                            AND PG.SHIFT=FMH.SHIFT
                                            AND PG.APPTYPE_ID=FMH.APPLICATION_TYPE_ID
                                            AND PG.IS_DELETED!=1
                                    WHERE
                                        FEE_ROOT_ID = ?FEE_ROOT_ID
                                        AND PG.PROGRAMME_GROUP_ID IN (?PROGRAMME_GROUP_ID) AND FEE_CATEGORY_ID IN (?FEE_CATEGORY_ID)                                       
                                        AND FREQUENCY_ID = ?FREQUENCY AND FMH.IS_DELETED!=1 
                                        GROUP BY MH.MAIN_HEAD_ID,H.HEAD_ID ORDER BY MH.MAIN_HEAD_ID";
                        break;
                    }
                case SupportDataSQLCommands.UpdatesafeUpdateon:
                    {
                        query = @"SET SQL_SAFE_UPDATES = 1;";
                        break;
                    }
                case SupportDataSQLCommands.UpdatesafeUpdateoff:
                    {
                        query = @"SET SQL_SAFE_UPDATES = 0;";
                        break;
                    }
                case SupportDataSQLCommands.FetchClassessByProgrammeId:
                    {
                        query = @"SELECT 
                            CLASS_ID, CLASS_NAME
                        FROM
                            ?TODB.CP_CLASSES
                        WHERE
                               PROGRAMME_GROUP_ID=?PROGRAMME_GROUP_ID
                                AND CLASS_YEAR = 1
                                AND IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchBatchIdByAc:
                    {
                        query = @"SELECT 
                                BATCH_ID, BATCH_YEAR
                            FROM
                                ?TODB.ACADEMIC_BATCHES
                            WHERE
                                ACADEMIC_YEAR_ID =?ACADEMIC_YEAR_ID AND BATCH_YEAR = 1; ";
                        break;
                    }
                case SupportDataSQLCommands.FetchStudentInfo:
                    {
                        query = @"SELECT 
                                STUDENT_ID, PROGRAM_ID AS PROGRAMME_ID, BATCH AS BATCH_ID, CLASS_ID,FIRST_NAME,STU_MOBILENO,STU_EMAILID,ROLL_NO
                            FROM
                                ?TODB.STU_PERSONAL_INFO
                            WHERE
                                ROLL_NO =?ROLL_NO;";
                        break;
                    }

                case SupportDataSQLCommands.FetchsmsTemplateByTemplateId:
                    {
                        query = @"SELECT 
                                TEMPLATE_ID, TEMPLATE_NAME, TEMPLATE_TEXT,fields_ids as SMS_TEMPLATE_ID
                            FROM
                                SUP_SMS_TEMPLATE_MODULE AS T
                                    INNER JOIN
                                SMS_USERDEFINED_TEMPLATE AS UT ON T.SMS_TEMPLATE_MODULE_ID = UT.TEMPLATE_ID
                            WHERE
                                T.ACADEMIC_YEAR = ?AC_YEAR
                                    AND UT.TEMPLATE_ID = ?TEMPLATE_ID
                                    AND T.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgrammesByAppType:
                    {
                        query = @"SELECT 
                                CPG.PROGRAMME_GROUP_ID,
                                CONCAT(CP.PROGRAMME_NAME,
                                        '(-',
                                        SS.SHIFT_NAME,
                                        '-',
                                        PM.PROGRAMME_MODE,
                                        ')') AS PROGRAMME_NAME
                            FROM
                                CP_PROGRAMME_GROUP AS CPG
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID=CPG.SHIFT
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS PM ON PM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                            WHERE
                                CPG.APPTYPE_ID =?APPTYPE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupFields:
                    {
                        query = @"SELECT 
                                FIELD_ID, FIELD_NAME, ALICE_NAME,PROPERTY_NAME
                            FROM
                                SUP_FIELDS
                            WHERE
                                IS_DELETED != 2;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupDocuments:
                    {
                        query = @"SELECT 
                                DOCUMENT_ID,
                                DOCUMENT_NAME
                            FROM
                                SUP_ADM_DOCUMENTS
                            WHERE
                                IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.InsertUploadedDocuments:
                    {
                        query = @"INSERT INTO 
                                ADM_DOCUMENTS_UPLOADED
                                (RECEIVE_ID,DOCUMENT_ID,ACADEMIC_YEAR,PROGRAMME_ID)
                                 VALUES(?RECEIVE_ID,?DOCUMENT_ID,?AC_YEAR,?PROGRAMME_GROUP_ID);";
                        break;
                    }
                case SupportDataSQLCommands.FetchReceiveIdByAppno:
                    {
                        query = @"SELECT 
                                RECEIVE_ID,ROLL_NO
                            FROM
                                adm_receive_application
                            WHERE
                                RECEIVE_ID =?RECEIVE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.UpdateImagePath:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        PHOTO_PATH =?PHOTO_PATH
                                    WHERE
                                       RECEIVE_ID=?RECEIVE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchReceiveByIssueId:
                    {
                        query = @"SELECT 
                                        AR.RECEIVE_ID,AR.ROLL_NO
                                    FROM
                                        ADM_ISSUED_APPLICATIONS AS AI
                                            INNER JOIN
                                        ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                    WHERE
                                        ISSUED_ID = ?ISSUED_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgramme:
                    {
                        query = @"SELECT 
                            CPG.PROGRAMME_GROUP_ID,
                            CONCAT(CP.PROGRAMME_NAME,
                                    '(',
                                    SPM.PROGRAMME_MODE,
                                    '-',
                                    SS.SHIFT_NAME,
                                    ')') AS PROGRAMME_NAME
                        FROM
                            ADM_ISSUED_APPLICATIONS AS AI
                                INNER JOIN
                            CP_PROGRAMME_GROUP AS CPG ON AI.PROGRAMME_GROUP_ID = CPG.PROGRAMME_GROUP_ID
                                INNER JOIN
                            CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                INNER JOIN
                            SUP_SHIFT AS SS ON SS.SHIFT_ID = CPG.SHIFT
                                INNER JOIN
                            SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                        WHERE
                            ISSUED_ID =?ISSUED_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgrammeBymodeandShift:
                    {
                        query = @"SELECT 
                                CPG.PROGRAMME_GROUP_ID, CP.PROGRAMME_NAME
                            FROM
                                CP_PROGRAMME_GROUP AS CPG
                                    INNER JOIN
                                CP_PROGRAMME AS CP ON CP.PROGRAMME_ID = CPG.PROGRAMME_ID
                                    INNER JOIN
                                SUP_SHIFT AS SS ON SS.SHIFT_ID = CPG.SHIFT
                                    INNER JOIN
                                SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = CPG.PROGRAMME_MODE
                            WHERE
		                            CPG.SHIFT =?SHIFT
                                    AND CPG.PROGRAMME_MODE =?PROGRAMME_MODE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchStudentsByRollno:
                    {
                        query = @"SELECT ROLL_NO,FIRST_NAME,PHOTO_PATH,RECEIVE_ID FROM ADM_RECEIVE_APPLICATION WHERE ROLL_NO IN(?ROLL_NO);";
                        break;
                    }
                case SupportDataSQLCommands.FetchAlladmittedList:
                    {
                        query = @"SELECT 
                                AI.ISSUED_ID,
                                CONCAT(AI.APPLICATION_NO,
                                        LPAD(AI.ISSUED_ID,4,'0'),
                                        '-',
                                        IFNULL(AR.ROLL_NO, ''),
                                        '-(',
                                        AR.FIRST_NAME,
                                        ')') AS APPLICATION_NO,
                                AR.PHOTO_PATH,
                                AR.ROLL_NO,
                                AR.FIRST_NAME,AR.BOID_IMAGE
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
                                    AND SP.IS_CANCELED != 1) AND AR.ROLL_NO IS NOT NULL;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupEducationBoard:
                    {
                        query = @"SELECT 
                                BOARD_ID, BOARD_NAME
                            FROM
                                SUP_EDUCATION_BOARD
                            WHERE
                                IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchGenderByApptypeId:
                    {
                        query = @"SELECT 
                                SAG.GENDER_ID, SG.GENDER_NAME
                            FROM
                                SUP_APPTYPE_GENDER AS SAG
                                    INNER JOIN
                                SUP_GENDER AS SG ON SAG.GENDER_ID = SG.GENDER_ID
                            WHERE
                                SAG.APPTYPE_ID = ?APPLICATION_TYPE_ID
                                    AND SAG.IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchProgrammeByApptypeId:
                    {
                        query = @"SELECT 
                                        AP.PROGRAMME_GROUP_ID,
                                        P.PROGRAMME_ID,                                       
                                        CONCAT(IFNULL(P.PROGRAMME_NAME, '')) AS PROGRAMME_NAME
                                    FROM
                                        CP_PROGRAMME_GROUP AS AP
                                            INNER JOIN
                                        CP_PROGRAMME AS P ON P.PROGRAMME_ID = AP.PROGRAMME_ID
                                            AND P.IS_DELETED != 1
                                            INNER JOIN
                                        SUP_SHIFT SH ON SH.SHIFT_ID = AP.SHIFT
                                            INNER JOIN
                                        SUP_PROGRAMME_MODE AS SPM ON SPM.PROGRAMME_MODE_ID = AP.PROGRAMME_MODE
                                    WHERE
                                        AP.IS_DELETED != 1 AND AP.IS_ACTIVE = 1 AND AP.APPTYPE_ID = ?APPLICATION_TYPE_ID group by AP.PROGRAMME_ID ORDER BY APPTYPE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAllSubjects:
                    {
                        query = @"SELECT 
                                HSS_SUBJECT_ID, HSS_SUBJECT, PART
                            FROM
                                SUP_HSS_SUBJECTS
                            WHERE
                                IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHostelDetails:
                    {
                        query = @"SELECT 
                                HOSTEL_ID, HOSTEL_NAME, IS_OUTSIDE, TOTAL_STRENGTH
                            FROM
                                SUP_HOSTEL
                            WHERE
                                IS_DELETED != 1 AND HOSTEL_ID = ?HOSTEL_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHostelSelected:
                    {
                        query = @"SELECT 
                                    COUNT(STATUS) AS 'SELECTED'
                                FROM
                                    ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS HSP
                                WHERE
		                                HSP.HOSTEL_ID = ?HOSTEL_ID
                                        AND HSP.IS_DELETED != 1
                                        AND HSP.IS_CANCELED != 1 AND STATUS=?STATUS;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHostelAdmitted:
                    {
                        query = @"SELECT 
                                    COUNT(STATUS) AS 'ADMITTED'
                                FROM
                                    ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS HSP
                                WHERE
		                                HSP.HOSTEL_ID = ?HOSTEL_ID
                                        AND HSP.IS_DELETED != 1
                                        AND HSP.IS_CANCELED != 1 AND STATUS=?STATUS;";
                        break;
                    }
                case SupportDataSQLCommands.SaveorUpdateIsEleventhPass:
                    {
                        query = @"UPDATE ADM_RECEIVE_APPLICATION 
                                    SET 
                                        IS_ELEVENTH_PASS = ?IS_ELEVENTH_PASS
                                    WHERE
                                        RECEIVE_ID = ?RECEIVE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAllHostelDetails:
                    {
                        query = @"SELECT 
                                HOSTEL_ID, HOSTEL_NAME, IS_OUTSIDE, TOTAL_STRENGTH
                            FROM
                                SUP_HOSTEL
                            WHERE
                                IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAllHostelSelected:
                    {
                        query = @"SELECT 
                                HSP.HOSTEL_ID,COUNT(STATUS) AS 'SELECTED'
                            FROM
                                ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS HSP
                            WHERE
                                HSP.IS_DELETED != 1
                                    AND HSP.IS_CANCELED != 1
                                    AND STATUS = ?STATUS
                            GROUP BY HSP.HOSTEL_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchAllHostelAdmitted:
                    {
                        query = @"SELECT 
                            HSP.HOSTEL_ID,COUNT(STATUS) AS 'ADMITTED'
                        FROM
                            ADM_HOSTEL_SELECTION_PROCESS_?AC_YEAR AS HSP
                        WHERE
                            HSP.IS_DELETED != 1
                                AND HSP.IS_CANCELED != 1
                                AND STATUS = ?STATUS
                        GROUP BY HSP.HOSTEL_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchDatewiseSMS:
                    {
                        query = @"SELECT 
                                    SS.SENT_SMS_ID,
                                    SL.MOBILE_NO,
                                    SL.CONTENT,
                                    DATE_FORMAT(SS.DATE, '%Y/%m/%d') AS DATE
                                FROM
                                    SENT_SMS_2019 AS SS
                                        INNER JOIN
                                    SENT_SMS_LIST_2019 AS SL ON SS.SENT_SMS_ID = SL.SENT_ITEM_ID
                                WHERE
                                    SS.DATE BETWEEN ?FROM_DATE AND ?TO_DATE;";
                        break;
                    }
                case SupportDataSQLCommands.FetchPhysicalychallangedtype:
                    {
                        query = @"SELECT 
                                TYPE_ID, TYPE
                            FROM
                                SUP_PHYSICALY_CHALLENGED_TYPES
                            WHERE
                                IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSelectionCycleByActiveAndIsShowwebsite:
                    {
                        query = @"SELECT 
                                    SELECTION_CYCLE_ID,
                                    SELECTION_CYCLE
                                FROM
                                    SUP_SELECTION_CYCLE
                                WHERE
                                    IS_ACTIVE = 1 AND IS_SHOW_WEBSITE = 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupSelectionCycle:
                    {
                        query = @"SELECT 
                                SELECTION_CYCLE_ID,
                                SELECTION_CYCLE,
                                IS_ACTIVE,
                                IS_SHOW_WEBSITE,
                                IS_USED
                            FROM
                                SUP_SELECTION_CYCLE
                            WHERE
                                IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchHssSubjects:
                    {
                        query = @"SELECT 
                                HSS_SUBJECT_ID,
                                HSS_SUBJECT
                            FROM
                                SUP_HSS_SUBJECTS
                            WHERE
                                IS_DELETED != 1;";
                        break;
                    }

                case SupportDataSQLCommands.FetchSupSelectionCycleById:
                    {
                        query = @"SELECT 
                                    SELECTION_CYCLE_ID,
                                    SELECTION_CYCLE,
                                    IS_ACTIVE,
                                    IS_SHOW_WEBSITE,
                                    IS_USED
                                FROM
                                    SUP_SELECTION_CYCLE
                                WHERE
                                    IS_DELETED != 1
                                    AND SELECTION_CYCLE_ID=?SELECTION_CYCLE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.SaveSupSelectionCycle:
                    {
                        query = @"INSERT INTO 
                                sup_selection_cycle
                                (
                                SELECTION_CYCLE,
                                IS_SHOW_WEBSITE)
                                VALUES
                                (
                                ?SELECTION_CYCLE,
                                ?IS_SHOW_WEBSITE);";
                        break;
                    }
                case SupportDataSQLCommands.UpdateSupSelectionCycle:
                    {
                        query = @"UPDATE SUP_SELECTION_CYCLE SET SELECTION_CYCLE=?SELECTION_CYCLE,IS_ACTIVE=?IS_ACTIVE,IS_SHOW_WEBSITE=?IS_SHOW_WEBSITE

                                WHERE SELECTION_CYCLE_ID=?SELECTION_CYCLE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.DeleteSupSelectionCycle:
                    {
                        query = @"UPDATE SUP_SELECTION_CYCLE SET IS_DELETED=?IS_DELETED WHERE SELECTION_CYCLE_ID=?SELECTION_CYCLE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.ActivateOrDeactivateSupSelectionCycleById:
                    {
                        query = @"UPDATE SUP_SELECTION_CYCLE SET IS_ACTIVE=?IS_ACTIVE WHERE SELECTION_CYCLE_ID=?SELECTION_CYCLE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.IsActivateOrDeactivateIsShowWebsiteSupSelectionCycleById:
                    {
                        query = @"UPDATE SUP_SELECTION_CYCLE SET IS_SHOW_WEBSITE=?IS_SHOW_WEBSITE WHERE SELECTION_CYCLE_ID=?SELECTION_CYCLE_ID;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupAdmTransferStatus:
                    {
                        query = @"SELECT 
                                    ADM_TRANSFER_STATUS_ID, STATUS_NAME
                                FROM
                                    SUP_ADM_TRANSFER_STATUS
                                WHERE
                                    IS_DELETED != 1;";
                        break;
                    }
                case SupportDataSQLCommands.FetchSupAdmApprovedContent:
                    {
                        query = @"SELECT 
                                    ADM_APPROVED_CONTENT_ID,
                                    CONTENT_NAME
                                FROM
                                    SUP_ADM_APPROVED_CONTENT
                                WHERE
                                    IS_DELETED != 1;";
                        break;
                    }

                case SupportDataSQLCommands.FetchSupSemester:
                    {
                        query = @"SELECT 
                                                    SUP_SEMESTER_ID, SEMESTER_NAME
                                                FROM
                                                    SUP_SEMESTER WHERE IS_DELETED!=1;";
                        break;
                    }

                case SupportDataSQLCommands.FetchSupApplicationSubmissionType:
                    {
                        query = @"SELECT 
                                        APP_TYPE_ID,
                                        APPLICATION_TYPE
                                    FROM
                                        sup_adm_application_type
                                    WHERE
                                        is_deleted != 1;";
                        break;
                    }

                case SupportDataSQLCommands.FetchMainHeadList:
                    {
                        query = @"SELECT 
                                       FMH.MAIN_HEAD_ID,
                                       MH.MAIN_HEAD
                                    FROM
                                        FEE_MAIN_HEADS AS FMH
                                            INNER JOIN
                                        SUP_FEE_MAIN_HEAD AS MH ON MH.MAIN_HEAD_ID = FMH.MAIN_HEAD_ID
                                    WHERE
                                        FMH.FREQUENCY_ID = ?FREQUENCY_ID AND FMH.ACADEMIC_YEAR=?AC_YEAR  AND FMH.IS_DELETED!=1
                                            AND FMH.PROGRAMME_MODE = ?PROGRAMME_MODE
                                    GROUP BY FMH.MAIN_HEAD_ID;";
                        break;
                    }
                case SupportDataSQLCommands.IssuedStudentlistByDate:
                    {
                        query = @"SELECT 
                                 AR.RECEIVE_ID,ISSUE_ID,FIRST_NAME,PROGRAMME_GROUP_ID
                                  FROM
                                      ADM_ISSUED_APPLICATIONS AS AI
                                          INNER JOIN
                                      ADM_RECEIVE_APPLICATION AS AR ON AR.RECEIVE_ID = AI.RECEIVE_ID
                                  WHERE
                                 AI.PROGRAMME_GROUP_ID = ?PROGRAMME_GROUP_ID
                                AND DATE BETWEEN (?FROM_DATE) AND (?TO_DATE)  AND AI.STATUS = 2 GROUP BY AI.RECEIVE_ID ORDER BY AI.ISSUED_ID;";
                        break;
                    }
            }
            return query;

        }
    }
}
