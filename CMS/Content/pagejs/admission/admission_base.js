
    $(document).ready(function () {
        FormWizardValidation.init();
    var dt = new Date();
        dt.setFullYear(new Date().getFullYear() - 16);


        $('.datepicker').datepicker({
        format: 'dd/mm/yyyy', orientation: 'bottom',
            endDate: dt
        });

        $('#ddlApplicationType').change(function () {
        $('#ddlShift').val('');
    $('#ddlCourses').val('');
        });

        $('#ddlShift').change(function (e) {
            if ($('#ddlApplicationType').val() == '') {
        alert('Please Choose Application Type..!');
    e.stopPropagation();
                return false;
            }
            $.ajax({
        url: "@Url.Action("FetchCoursesForAdmission", "Admission")",
    method: "POST",
    data: {sApplicationType: $('#ddlApplicationType').val(), sShift: $('#ddlShift').val() },
    success: function (data) {
        var ddl = $('#ddlCourses');
        ddl.empty();
        ddl.append("<option value=''>--select--</option>");
        ddl.append(data);

    }, error: function (error) {
        alert('Try Again!!');
    }
});
});

});
function Validate() {
var returnValue = false;
var sOTP = $("#txtOTP").val();
var form = new FormData();
form.append("sOTP", sOTP);
$.ajax({
        url: "@Url.Action("ValidateOTP", "Admission")",
method: "POST",
    cache: false,
    contentType: false,
    processData: false,
    async: false,
    data: form,
    success: function (data) {
        var result = String(data);
        if ('false' === result) {
        alert('Invalid OTP!!!');
    return false;
        } else {
        returnValue = true;
    }
    }, error: function (error) {
        alert('Try Again!!');
    return false;
    }
});
return returnValue;
}
function SaveOrUpdateIssue() {
var json;
json = '{"APPLICATION_TYPE_ID":"' + $('#ddlApplicationType').val() + '","PROGRAMME_ID":"' + $('#ddlCourses').val() + '","FIRST_NAME":"' + $('#txtFirstName').val() + '","LAST_NAME":"' + $('#txtLastName').val() + '","CONTACT_NO":"' + $('#txtPhone').val() + '","DOB":"' + $('#txtDOB').val() + '","SHIFT":"' + $('#ddlShift').val() + '","HSC_NO":"' + $('#txtHSCNo').val() + '","EMAIL_ID":"' + $('#txtEmail').val() + '"}'
$.ajax({

        url: "@Url.Action("SaveOrUpdateForIssueApplicaiton", "Admission")",

    method: "POST",
    data: {sJson: json },
    async: false,
    success: function (data) {
        if (!data) {
            return false;
        }
    }, error: function (error) {
        alert('Try Again!!');
    return false;
    }

});
}
function getApplicant() {
        $.ajax({
            /**/
            url: '@Url.Action("FetchApplicantInfo", "admission")',
            /**/
            type: 'GET',
            cache: false,
            success: function (result) {
                if (result.ErrorCode == '200') {
                    $('#txtContact').val(result.oResult.CONTACT_NO);
                    $('#txtamount').val(result.oResult.APP_FEE);
                    $('#txtfname').val(result.oResult.FIRST_NAME);
                    $('#txtemailid').val(result.oResult.EMAIL_ID);
                } else {
                    alert(result.Message);
                }
            }
        });
    }

$('#resend').click(function () {
        $.ajax({
            /**/
            url: "@Url.Action("ResendOTP", "Admission")",
            /**/
            method: "POST",
            data: { sContactNo: $('#txtPhone').val(), sEmailId: $('#txtEmail').val(), sFirstName: $('#txtFirstName').val() },
            success: function (data) {
                alert(data);
            }, error: function (error) {
                alert('Try Again!!');
                return false;
            }
        });
    });

        function sendOTP() {

        $.ajax({

            url: "@Url.Action("SendOTP", "Admission")",
            async: false,
            method: "POST",
            data: { sContactNo: $('#txtPhone').val(), sEmailId: $('#txtEmail').val(), sFirstName: $('#txtFirstName').val() },
            success: function (data) {

            }, error: function (error) {
                alert('Try Again!!');
                return false;
            }
        });
    }
