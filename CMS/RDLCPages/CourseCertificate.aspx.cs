using CMS.Utility;
using System;
using System.Data;

namespace CMS.RDLCPages
{
    public partial class CourseCertificate : System.Web.UI.Page
    {
        string sStudentIds = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sStudentIds = Request.QueryString["sStudentIds"].ToString();
                RenderReport();
            }
        }
        public void RenderReport()
        {
            string sAcademicYear = (Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR] != null) ? Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString() : string.Empty;
            string sBatch = string.Empty;
            DataTable dtCourseCertificate = new DataTable();
            DataTable dtGetAcademicYear = new DataTable();
            DataTable dtAlliedSubjects = new DataTable();
            DataTable dtAllied = new DataTable();
            DataTable Allied = new DataTable();
            DataTable dtBatch = new DataTable();
            //var ObjAcademicYear = new List<GET_ACADEMIC_YEAR_BY_ID>();
            // var ObjAlliedSubjects = new List<Get_Allied_Subjects>();
            // Get_Allied_Subjects AlliedSubjects = new Get_Allied_Subjects();
            //GET_ACADEMIC_YEAR_BY_ID obj =new GET_ACADEMIC_YEAR_BY_ID();
            //obj.STUDENT_ID = sStudentIds;
            using (ViewModel.ViewModel.StudentViewModel ObjViewModel = new ViewModel.ViewModel.StudentViewModel())
            {
                dtBatch = ObjViewModel.FetchBatchByStudentId(sStudentIds).DataSource.Table;
                sBatch = dtBatch.Rows[0][Common.SUP_BATCHES.BATCH].ToString();
                dtCourseCertificate = ObjViewModel.FetchCourseCertificate(sStudentIds, sAcademicYear, sBatch).DataSource.Table;
                dtGetAcademicYear = ObjViewModel.FetchAcademicYear(sStudentIds, sAcademicYear).DataSource.Table;
            }


            if (dtCourseCertificate != null && dtCourseCertificate.Rows.Count > 0)
            {
                // Get Allied Subject By Academic Year ...
                foreach (DataRow dr in dtGetAcademicYear.Rows)
                {
                    using (ViewModel.ViewModel.StudentViewModel ObjViewModel = new ViewModel.ViewModel.StudentViewModel())
                    {
                        dtAlliedSubjects = ObjViewModel.FetchAlliedCourseByStudentId(sStudentIds, dr[Common.ACADEMIC_YEAR.AC_YEAR].ToString()).DataSource.Table;
                        if (dtAlliedSubjects != null && dtAlliedSubjects.Rows.Count > 0)
                        {
                            dtAllied.Merge(dtAlliedSubjects);
                        }
                    }
                }

                dtCourseCertificate.Columns.Add("ALLIED1");
                dtCourseCertificate.Columns.Add("ALLIED2");
                dtCourseCertificate.Columns.Add("ALLIED3");
                dtCourseCertificate.Columns.Add("ALLIED4");
                dtCourseCertificate.Columns.Add("ALLIED5");
                dtCourseCertificate.Columns.Add("ALLIED6");


                // Set Allied Subject To DataSet ..
                foreach (DataRow col in dtCourseCertificate.Rows)
                {
                    int count = 0;
                    foreach (DataRow item in dtAllied.Rows)
                    {
                        if (item[Common.STU_PERSONAL_INFO.STUDENT_ID].ToString() == col[Common.STU_PERSONAL_INFO.STUDENT_ID].ToString())
                        {
                            count++;
                            if (count == 1)
                            {
                                col["ALLIED1"] = "1." + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString();
                            }
                            else if (count == 2)
                            {
                                col["ALLIED2"] = "2." + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString();
                            }
                            else if (count == 3)
                            {
                                col["ALLIED3"] = "3." + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString();
                            }
                            else if (count == 4)
                            {
                                col["ALLIED4"] = "4." + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString();
                            }
                            else if (count == 5)
                            {
                                col["ALLIED5"] = "5." + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString();
                            }
                            else if (count == 6)
                            {
                                col["ALLIED6"] = "6." + item[Common.CP_COURSE_ROOT_2017.COURSE_TITLE].ToString();
                            }
                        }
                    }
                    dtCourseCertificate.AcceptChanges();
                }
            }

            rvCourseCertificate.Reset();
            rvCourseCertificate.LocalReport.EnableExternalImages = true;
            rvCourseCertificate.LocalReport.ReportPath = Server.MapPath("~/CMSReports/CourseCertificate.rdlc");
            rvCourseCertificate.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("CourseCertificate", dtCourseCertificate));
        }
    }
}