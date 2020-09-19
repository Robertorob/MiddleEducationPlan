

$(document).ready(function () {
    getProjectTypeSelectValues();

});

function createProject() {
    if (validate() === false) {
        return;
    }

    let project = getProjectFromPage();

    showLoader();
    disablePage();

    $.post('/ProjectView/CreatePost', project)
        .done((result) => {


            //hideLoader();

            //enablePage();

            if (result.status === 0) {
                window.location.href = "/ProjectView/Update/" + result.value.id;

                //showSuccessDialogOnCreated();
                clearForm();
            }
            else {
                showErrorDialogOnCreated();
                clearForm();
            }
        })
        .fail(() => {
            hideLoader();
            enablePage();

            showErrorDialogOnCreated();
            clearForm();
        })
}

function getProjectFromPage() {
    let name = $('#projectName').val();
    let description = $('#projectDescription').val();
    let projectType = $('#projectType').val();

    let project = {};
    project.Name = name;
    project.Description = description;
    project.ProjectType = projectType;

    return project;
}

function showSuccessDialogOnCreated() {
    showDialog('Success', 'Successfully created', true);
}

function showErrorDialogOnCreated() {
    showDialog('Error', 'Error occured');
}

function validate() {

    console.log('validate');

    let projectNameValidated = $('#projectForm').validate().element('#projectName');
    let projectTypeValidated = $('#projectForm').validate().element('#projectType');
     
    return projectNameValidated && projectTypeValidated;
}