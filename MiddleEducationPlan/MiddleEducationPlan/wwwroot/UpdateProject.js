

$(document).ready(function () {
    getProjectModel();
});

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