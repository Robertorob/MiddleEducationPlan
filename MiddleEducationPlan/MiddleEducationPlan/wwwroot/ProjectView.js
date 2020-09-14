

(function () {
    // Get project types
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
        }
        else {
            getProjectTypesErrorDialog();
            ClearForm();
        }
    })
    .fail(() => {
        getProjectTypesErrorDialog();
        ClearForm();
    })
})();

function CreateProject() {
    let name = $("#projectName").val();
    let description = $("#projectDescription").val();
    let projectType = $("#projectType").val();

    debugger;

    let project = {};
    project.Name = name;
    project.Description = description;
    project.ProjectType = projectType;

    $.post("CreatePost", project).done((result) => {
        if (result.status === 0) {
            createdSuccessDialog();
            ClearForm();
        }
        else {
            createdErrorDialog()
            ClearForm();
        }
    })
    .fail(() => {
        $("#createProjectError").dialog();
        ClearForm();
    })
}

function createdSuccessDialog() {
    showDialog("Success", "Successfully created");
}

function createdErrorDialog() {
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
}

function ClearForm() {
    $("#projectName").val("");
    $("#projectDescription").val("");

    $("#projectType").val("-1");
    $("#projectType").trigger("chosen:updated");
}