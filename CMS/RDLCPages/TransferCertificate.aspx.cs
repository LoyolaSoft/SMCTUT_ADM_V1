﻿using CMS.Utility;
using CMS.ViewModel.ViewModel;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;

namespace CMS.RDLCPages
{
    public partial class TransferCertificate : System.Web.UI.Page
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
            DataTable dtCollegeInfo = new DataTable();
            DataTable dtTransferCertificate = new DataTable();

            if (Session[Common.SESSION_VARIABLES.COLLEGE_INFO] != null)
            {
                dtCollegeInfo = (DataTable)Session[Common.SESSION_VARIABLES.COLLEGE_INFO];
                //  sCourseId = (Session[Common.SESSION_VARIABLES.COURSE_ID] != null) ? Session[Common.SESSION_VARIABLES.COURSE_ID].ToString() : "0";
            }
            string sAcademicYear = Session[Common.SESSION_VARIABLES.ACADEMIC_YEAR].ToString();
            StudentViewModel objViewModel = new StudentViewModel();
            dtTransferCertificate = objViewModel.BindStudentsForTC(sStudentIds, sAcademicYear).DataSource.Table;
            rvTransferCertificate.Reset();
            rvTransferCertificate.LocalReport.EnableExternalImages = true;
            rvTransferCertificate.LocalReport.ReportPath = Server.MapPath("~/CMSReports/TransferCertificates.rdlc");
            rvTransferCertificate.LocalReport.DataSources.Add(new ReportDataSource("CollegeInfo", dtCollegeInfo));
            rvTransferCertificate.LocalReport.DataSources.Add(new ReportDataSource("TransferCertifcate", dtTransferCertificate));
        }
    }
}