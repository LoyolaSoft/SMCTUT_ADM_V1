using CMS.ViewModel.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CMS.ViewModel.ViewModel
{
    public class TaskManagementViewModel : IDisposable
    {
        public SelectList ClassList { get; set; }
        public SelectList CourseList { get; set; }
        public SelectList AssignmentList { get; set; }
        public string HiddenId { get; set; }
        public bool IS_ACTIVE { get; set; }
        public bool IS_FILE_UPLOAD { get; set; }
        public string ClassId { get; set; }
        public string CourseId { get; set; }
        public List<AssignmentModel> liTaskManagementModel { get; set; }
        public List<ASSIGNMENT_SUBMISSION> liAssignmentSubmissionModel { get; set; }
        public AssignmentModel Assignment = new AssignmentModel();
        #region DisposeMethod
        public void Dispose()
        {
            GC.Collect();
        }
        #endregion
    }
}
