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

function showLoader() {
    $('.loader').fadeIn();
}

function hideLoader() {
    $('.loader').fadeOut();
}

function disablePage() {
    $("#projectFormContainer").addClass("disabledContainer");
}

function enablePage() {
    $("#projectFormContainer").removeClass("disabledContainer");
}

function getProjectTypesErrorDialog() {
    showDialog('Error', 'Error while retrieving project types');
}

function showDialog(header, text, fade) {

    let dialogOptions = {
        title: header,
        hide: { effect: 'explode' },
        show: { effect: 'explode' },
        buttons: [
            {
                text: "Ok",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ],
    };

    //if (fade === true) {
    //   dialogOptions.dialogClass = 'no-close';
    //}

    $('#dialog').dialog(dialogOptions);

    $('#dialogBody').text(text);

    $('#dialog').dialog('open');

    if (fade === true) {
        setTimeout(() => {
            $('#dialog').dialog('close');
        }, 3000)

    }

}

function clearForm() {
    $('#projectName').val('');
    $('#projectDescription').val('');

    $('#projectType').val('');
    $('#projectType').trigger('chosen:updated');
}