

$(document).ready(function () {
    setFormEnabled(false);
    getProjectModel();
    getProjectTypeSelectValues();

    $('#projectName').change(() => {
        validate();
    })
});

function getProjectTypeSelectValues() {
    $.get('/ProjectView/GetProjectTypes')
        .done((result) => {
            if (result.status === 0) {

                var options = result.value.values;

                for (var i = 0; i < options.length; i++) {
                    var o = new Option(options[i].name, options[i].value);
                    $(o).html(options[i].name);
                    $('#projectType').append(o);
                }

                $('#projectType').chosen({
                    width: '100%'
                    //,inherit_select_classes: true
                })
                $.validator.setDefaults({ ignore: ':hidden:not(.chosen-select)' });

                if ($('select.chosen-select').length > 0) {
                    $('select.chosen-select').each(function () {
                        if ($(this).attr('required') !== undefined) {
                            $(this).on('change', function () {
                                $(this).valid();
                            });
                        }
                    });
                }

                $('#projectForm').validate({
                    errorPlacement: function (error, element) {
                        if (element.is('select.chosen-select')) {
                            element.next('div.chosen-container').append(error);
                        } else {
                            error.insertAfter(element);
                        }
                    }
                });
            }
            else {
                getProjectTypesErrorDialog();
                clearForm();
            }
        })
        .fail(() => {
            getProjectTypesErrorDialog();
            clearForm();
        })
}

function getProjectModel() {
    let id = $("#hiddenProjectId").val();

    $.get('/ProjectView/GetAsync/' + id)
        .done((result) => {
            if (result.status === 0) {
                fillProjectFields(result.value);
            }
            else {
                getProjectTypesErrorDialog();
                clearForm();
            }
        })
        .fail(() => {
            getProjectTypesErrorDialog();
            clearForm();
        })
}

function fillProjectFields(model) {
    $('#projectId').val(model.id);
    $('#projectName').val(model.name);
    $('#projectDescription').val(model.description);

    $('#projectType').val(model.projectType);
    $('#projectType').trigger('chosen:updated');
}

function hideValidationErrors() {
    $('label.error').hide();
    $('#projectName').removeClass('error');
}

function cancel() {
    getProjectModel();
    switchFormToDisabled();
    hideValidationErrors();
}

function switchFormToDisabled() {
    setFormEnabled(false);
    setEditButton();
}

function editProject() {
    setFormEnabled(true);
    setCancelButton();
}

function setFormEnabled(enabled) {
    enabled = !enabled;
    $('#projectName').prop('disabled', enabled);
    $('#projectDescription').prop('disabled', enabled);

    $('#projectType').prop('disabled', enabled);
    $('#projectType').trigger('chosen:updated');

    $('#projectSendButton').prop('disabled', enabled);
}

function setCancelButton() {
    $('#projectEditCell').hide();
    $('#projectCancelCell').show();
}

function setEditButton() {
    $('#projectEditCell').show();
    $('#projectCancelCell').hide();
}

function updateProject() {
    if (validate() === false) {
        return;
    }

    let project = getProjectFromPage();

    showLoader();
    disablePage();

    $.post('/ProjectView/UpdatePost', project)
        .done((result) => {
            if (result.status === 0) {
                hideLoader();
                enablePage();
                showUpdatedSuccessfullyDialog();
                switchFormToDisabled();
            }
            else {
                hideLoader();
                enablePage();
                showErrorDialogOnUpdate(result.errorMessage);
                clearForm();
            }
        })
        .fail(() => {
            hideLoader();
            enablePage();
            showErrorDialogOnUpdate(result.errorMessage);
            clearForm();
        })
}

function showUpdatedSuccessfullyDialog() {
    showDialog('Success', 'Successfully updated', false);
}

function showErrorDialogOnUpdate(errorMessage) {
    showDialog('Error', errorMessage, false);
}