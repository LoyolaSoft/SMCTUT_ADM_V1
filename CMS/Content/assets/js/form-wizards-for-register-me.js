
var handleBootstrapWizardsValidation = function () {
    "use strict";
    $("#wizard").bwizard({
       
        validating: function (e, t) {
            if (t.index == 0) {
                if (false === $('#devForm').parsley().validate("wizard-step-1")) {
                    return false
                } else {
                    if (!sendOTP()) {
                        return false;
                    }
                    divBar('33%');
                }
            } else if (t.index == 1) {
             
                if (false === $('#devForm').parsley().validate("wizard-step-2")) {
                    return false
                } else {
                    if (!Validate()) {
                        return false
                    }
                    SaveOrUpdateIssue();
                    getApplicant();
                    divBar('70%');
                }
            } else if (t.index == 2) {
                if (false === $('#devForm').parsley().validate("wizard-step-3")) {
                    return false
                }
            }
        }
    })
};
var FormWizardValidation = function () {
    "use strict";
    return {
        init: function () {
            handleBootstrapWizardsValidation()
        }
    }
}()