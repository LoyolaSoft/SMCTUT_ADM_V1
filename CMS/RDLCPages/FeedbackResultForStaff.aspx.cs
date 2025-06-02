using CMS.Utility;
using CMS.ViewModel.ViewModel;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;


namespace CMS.RDLCPages
{
    public partial class FeedbackResultForStaff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RenderReport();
            }
        }

        private void RenderReport()
        {
            try
            {
                ResultArgs resultArgs = new ResultArgs();
                DataValue dv = new DataValue();

                DataTable dtCourseClassInfo = new DataTable();
                DataTable dtStaffRatting = new DataTable();
                DataTable dtFeedbackSetting = new DataTable();
                DataTable dtCollegeInfo = new DataTable();
                string sCourseId = string.Empty;
                string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
                if (Session[Common.SESSION_VARIABLES.COLLEGE_INFO] != null)
                {
                    dtCollegeInfo = (DataTable)Session[Common.SESSION_VARIABLES.COLLEGE_INFO];
                    sCourseId = (Session[Common.SESSION_VARIABLES.COURSE_ID] != null) ? Session[Common.SESSION_VARIABLES.COURSE_ID].ToString() : "0";
                }
                FeedbackViewModel objFeedback = new FeedbackViewModel();
                dv.Clear();
                dv.Add(Common.FBEVALUATION2017.COURSE_ID, sCourseId, EnumCommand.DataType.String);
                dv.Add(Common.FBCLASS_WISE_STAFF2017.STAFF_ID, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                dtCourseClassInfo = objFeedback.FetchFBClassCourseInfo(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;
                dtFeedbackSetting = objFeedback.FetchFBSettingInfo().DataSource.Table;
                dv.Clear();
                dv.Add(Common.FBEVALUATION2017.COURSE_ID, sCourseId, EnumCommand.DataType.String);
                dv.Add(Common.FBEVALUATION2017.ASSESSEE, Session[Common.SESSION_VARIABLES.USER_ID].ToString(), EnumCommand.DataType.String);
                dtStaffRatting = objFeedback.FetchFBResultByCourseId(dv, (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : "").DataSource.Table;

                rvFeedbackResultForStaff.Reset();
                rvFeedbackResultForStaff.LocalReport.EnableExternalImages = true;
                rvFeedbackResultForStaff.LocalReport.ReportPath = Server.MapPath("~/CMSReports/FeedResult.rdlc");
                rvFeedbackResultForStaff.LocalReport.DataSources.Add(new ReportDataSource("FeedbackInfo", dtFeedbackSetting));
                rvFeedbackResultForStaff.LocalReport.DataSources.Add(new ReportDataSource("CourseAndClassInfo", dtCourseClassInfo));
                rvFeedbackResultForStaff.LocalReport.DataSources.Add(new ReportDataSource("FeedbackResult", dtStaffRatting));
                rvFeedbackResultForStaff.LocalReport.DataSources.Add(new ReportDataSource("CollegeInfo", dtCollegeInfo));
            }
            catch (Exception ex)
            {


            }


        }
    }
}