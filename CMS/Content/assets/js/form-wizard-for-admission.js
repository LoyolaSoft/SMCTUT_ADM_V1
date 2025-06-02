
var handleBootstrapWizardsValidation = function () {
    "use strict";
    $("#wizard").bwizard({
        validating: function (e, t) {            
            if (t.index === 0) {
                if (false === $('#devForm').parsley().validate("wizard-step-1")) {                      
                    return false
                } else {                          
                    SaveOrUpdateIssuePersonal();
                    loadMarks();
                    $('#btn-back').addClass('hidden');
                    //console.log('step1');

                }
            } else if (t.index === 1) {
                if (false === $('#devForm').parsley().validate("wizard-step-2")) {
                    return false
                } else {
                    SaveOrUpdateIssuePersonal();
                    loadMarks();
                    $('#btn-back').addClass('hidden');
                    //console.log('step2');
                }
            } else if (t.index === 2)
            {
                if (Validate())
                {                    
                    return false
                }
                if (false === $('#devForm').parsley().validate("wizard-step-3"))
                {                    
                    return false
                }
                else
                {                             
                    SaveOrUpdateIssueMarks();   
                    loadMarks();
                    $('#btn-back').addClass('hidden');
                }
            } else if (t.index === 3) {
                if (false === $('#devForm').parsley().validate("wizard-step-4")) {
                    return false 
                } else {
                    SaveOrUpdateFiles();
                    $('#btn-back').addClass('hidden');
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