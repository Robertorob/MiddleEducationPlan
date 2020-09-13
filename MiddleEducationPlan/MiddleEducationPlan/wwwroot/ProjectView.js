function CreateProject() {
    let name = $("#projectName").val();
    let description = $("#projectDescription").val();

    let project = {};
    project.Name = name;
    project.Description = description;

    $.post("CreatePost", project).done((result) => {
        debugger;
        if (result.status === 0) {
            $("#dialog").dialog();
            ClearForm();
        }
        else {
            $("#error").dialog();
            ClearForm();
        }
    })
    .fail(() => {
        $("#error").dialog();
        ClearForm();
    })
}

function ClearForm() {
    $("#projectName").val("");
    $("#projectDescription").val("");
}