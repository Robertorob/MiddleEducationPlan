

$(document).ready(function () {

    getProjectTypeSelectValues();

});

function getProjectTypeSelectValues() {
    $.get("GetProjectTypes")
        .done((result) => {
            if (result.status === 0) {
                var options = result.value.values;

                for (var i = 0; i < options.length; i++) {
                    var o = new Option(options[i].name, options[i].value);
                    $(o).html(options[i].name);
                    $("#projectType").append(o);
                }

                $("#projectType").chosen({ width: "100%" })
                $.validator.setDefaults({ ignore: ":hidden:not(.chosen-select)" });

                if ($("select.chosen-select").length > 0) {
                    $("select.chosen-select").each(function () {
                        if ($(this).attr('required') !== undefined) {
                            $(this).on("change", function () {
                                $(this).valid();
                            });
                        }
                    });
                }

                $("#projectForm").validate({
                    errorPlacement: function (error, element) {
                        if (element.is("select.chosen-select")) {
                            element.next("div.chosen-container").append(error);
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

function CreateProject() {
    if (validate() === false) {
        return;
    }

    let project = getProjectFromPage();

    $.post("CreatePost", project)
        .done((result) => {
            if (result.status === 0) {
                showSuccessDialogOnCreated();
                clearForm();
            }
            else {
                showErrorDialogOnCreated();
                clearForm();
            }
        })
        .fail(() => {
            showErrorDialogOnCreated();
            clearForm();
        })
}

function getProjectFromPage() {
    let name = $("#projectName").val();
    let description = $("#projectDescription").val();
    let projectType = $("#projectType").val();

    let project = {};
    project.Name = name;
    project.Description = description;
    project.ProjectType = projectType;

    return project;
}

function showSuccessDialogOnCreated() {
    showDialog("Success", "Successfully created");
}

function showErrorDialogOnCreated() {
    showDialog("Error", "Error occured");
}

function getProjectTypesErrorDialog() {
    showDialog("Error", "Error while retrieving project types");
}

function showDialog(header, text) {
    $("#dialog").dialog({
        title: header
    });

    $("#dialogBody").text(text);

    $("#dialog").show();
}

function clearForm() {
    $("#projectName").val("");
    $("#projectDescription").val("");

    $("#projectType").val("");
    $("#projectType").trigger("chosen:updated");
}

function validate() {

    console.log("validate");

    let projectNameValidated = $("#projectForm").validate().element("#projectName");
    let projectTypeValidated = $("#projectForm").validate().element("#projectType");
     
    return projectNameValidated && projectTypeValidated;
}