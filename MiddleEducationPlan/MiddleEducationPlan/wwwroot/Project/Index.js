function deleteProject(id) {
    showLoader();
    disableProjectTable();
    $.get('/ProjectView/Delete/' + id)
        .done((result) => {
            hideLoader();
            enableProjectTable();
            if (result.status === 0) {
                showDeletedSuccess();
                $('#' + id).remove();
            }
            else {
                showDeletedError();
            }
        })
        .fail(() => {
            hideLoader();
            enableProjectTable();
            showDeletedError();
        });
}

function showDeletedSuccess() {
    showDialog('Success', 'Successfully deleted', true);
}

function showDeletedError() {
    showDialog('Error', 'Error occured', false);
}

function disableProjectTable() {
    $("#projectTable").addClass("disabledContainer");
}

function enableProjectTable() {
    $("#projectTable").removeClass("disabledContainer");
}

function showDeleteDialog(element) {
    let id = getId(element);

    let text = 'Are you sure?';
    let header = 'Attention';

    let dialogOptions = {
        buttons: [
            {
                text: "Cancel",
                click: function () {
                    $(this).dialog("close");
                }
            },
            {
                text: "Ok",
                click: function () {
                    deleteProject(id);
                    $(this).dialog("close");
                }
            }
        ],
        title: header,
        hide: { effect: 'explode' },
        show: { effect: 'explode' }
    };


    $('#deleteDialog').dialog(dialogOptions);

    $('#deleteDialogBody').text(text);

    $('#deleteDialog').dialog('open');

}

function updateProject(element) {
    let id = getId(element);

    window.location.href = "/ProjectView/Update/" + id;
}

function getId(element) {
    return $(element).closest('tr').attr('id');
}