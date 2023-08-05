function addUser(prop) {
    prop = "test";
}

var selectSubsidiary = document.getElementById("subsidiarySelect");
var selectDepartment = document.getElementById("departmentSelect");
var selectJob = document.getElementById("jobSelect");
var selectedSubsidiary;
var selectedDepartment;

selectSubsidiary.onchange = function () {
    selectedSubsidiary = selectSubsidiary.value;

    if (selectedSubsidiary !== "") {
        $.ajax({
            type: "POST",
            url: '/application/connection/GetDepartments',
            data: JSON.stringify(selectSubsidiary.value),
            contentType: "application/json",
            success: function (data) {
                selectJob.innerHTML = '';
                var newJobOption = document.createElement("option");
                newJobOption.value = '';
                newJobOption.text = 'S\u00E9lectionner un poste';
                newJobOption.disabled = true;
                newJobOption.selected = true;
                selectJob.add(newJobOption);

                selectDepartment.innerHTML = '';
                var newDepartmentOption = document.createElement("option");
                newDepartmentOption.value = '';
                newDepartmentOption.text = 'S\u00E9lectionner un d\u00E9partement';
                newDepartmentOption.disabled = true;
                newDepartmentOption.selected = true;
                selectDepartment.add(newDepartmentOption);
                
                data.data.forEach(function (valeur) {
                    var newOption = document.createElement("option");
                    newOption.value = valeur.value;
                    newOption.text = valeur.text;
                    selectDepartment.appendChild(newOption);
                });
            },
            error: function (xhr, status, error) {

            }
        });
    }
    else {
        selectDepartment.empty();
    }
}

selectDepartment.onchange = function () {
    selectedDepartment = selectDepartment.value;

    if (selectedDepartment !== "") {
        $.ajax({
            type: "POST",
            url: '/application/connection/GetJobs',
            data: JSON.stringify(selectDepartment.value),
            contentType: "application/json",
            success: function (data) {
                selectJob.innerHTML = '';
                var newOption = document.createElement("option");
                newOption.value = '';
                newOption.text = 'S\u00E9lectionner un poste';
                newOption.disabled = true;
                newOption.selected = true;
                selectJob.add(newOption);

                data.data.forEach(function (valeur) {
                    var newOption = document.createElement("option");
                    newOption.value = valeur.value;
                    newOption.text = valeur.text;
                    selectJob.appendChild(newOption);
                });
            },
            error: function (xhr, status, error) {

            }
        });
    }
    else {
        selectJob.empty();
    }
}