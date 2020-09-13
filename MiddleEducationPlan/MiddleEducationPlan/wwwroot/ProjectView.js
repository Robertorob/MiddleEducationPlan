$(document).ready(function () {
    console.log("ready!");
});

function CreateProject() {
    let name = $("#projectName").val();

    let project = {};
    project.Name = name;

    $.post("CreatePost", project, (result) => {
        if (result.status === 0) {
            $("#dialog").dialog();
            //window.alert("success");
            ClearForm();
        }
        else {
            window.alert(result.errorMessage);
        }
    })
}

function ClearForm() {
    $("#projectName").val("");
}