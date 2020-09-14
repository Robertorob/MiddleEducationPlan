﻿

$(document).ready(function () {
    $.get("GetProjectTypes").done((result) => {
        if (result.status === 0) {
            var options = result.value.values;

            for (var i = 0; i < options.length; i++) {
                var o = new Option(options[i].name, options[i].value);
                /// jquerify the DOM object 'o' so we can use the html method
                $(o).html(options[i].name);
                $("#projectType").append(o);
            }

            $("#projectType").chosen({ width: "100%" })
            //$.validator.setDefaults({ ignore: ":hidden:not(select)" });
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
                    console.log("placement");
                    if (element.is("select.chosen-select")) {
                        console.log("placement for chosen");
                        // placement for chosen
                        element.next("div.chosen-container").append(error);
                    } else {
                        // standard placement
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


});

function CreateProject() {
    if (validate() === false) {
        return;
    }

    debugger;

    let name = $("#projectName").val();
    let description = $("#projectDescription").val();
    let projectType = $("#projectType").val();

    debugger;

    let project = {};
    project.Name = name;
    project.Description = description;
    project.ProjectType = projectType;

    $.post("CreatePost", project).done((result) => {
        debugger;
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
        debugger;
        showErrorDialogOnCreated();
        clearForm();
    })
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