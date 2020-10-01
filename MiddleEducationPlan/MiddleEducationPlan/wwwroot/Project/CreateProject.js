

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
            if (result.status === 0) {
                window.location.href = "/ProjectView/Update/" + result.value.id;
            }
            else {
                hideLoader();
                enablePage();
                showErrorDialogOnCreated(result.errorMessage);
                clearForm();
            }
        })
        .fail(() => {
            hideLoader();
            enablePage();
            showErrorDialogOnCreated(result.errorMessage);
            clearForm();
        })
}

function showSuccessDialogOnCreated() {
    showDialog('Success', 'Successfully created', true);
}

function showErrorDialogOnCreated(errorMessage) {
    showDialog('Error', errorMessage);
}

