﻿

$(document).ready(function () {
    getProjectModel();
    getProjectTypeSelectValues();
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
    let id = $("#projectId").val();

    $.get('/ProjectView/GetAsync/' + id)
        .done((result) => {
            if (result.status === 0) {
                window.alert("success");
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

function editForm() {

}
function updateProject() {

}