function addUser(prop) {
    prop = "test";
}

$(document).ready(function () {
    $('#subsidiarySelect').change(function () {
        var subsidiaryId = $(this).val();

        $.ajax({
            type: 'POST',
            url: '/application/connection/getdepartments',
            data: subsidiaryId,
            contentType: "application/json",
            success: function (data) {
                // Supprime les anciennes options
                $('#departmentSelect').empty();

                // Ajoute les nouvelles options des départements
                $.each(data, function (index, department) {
                    $('#departmentSelect').append($('<option></option>').val(department.id).text(department.name));
                });
            }
        });
    });
});