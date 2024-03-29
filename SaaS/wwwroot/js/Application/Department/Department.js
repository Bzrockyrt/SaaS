var departmentDataTable;

$(document).ready(function () {
    loadDepartmentDataTable();
});

/*START - Index Company*/
function loadDepartmentDataTable() {
    departmentDataTable = $('#tableDepartments').DataTable({
        "ajax": { url: '/application/department/getalldepartments' },
        "columns": [
            {
                "data": { id: "id", name: "name" },
                "render": function (data) {
                    if (data.name != null) {
                        return `
                            <a style="color: #0000FF; text-decoration: underline;" href="/application/department/details?id=${data.id}">${data.name}</a>
                        `
                    }
                },
                "width": "30%"
            },
            { "data": "code", "width": "20%" },
            { "data": "subsidiaryName", "width": "20%" },
            { "data": "jobsNumber", "width": "10%" },
            { "data": "employeesNumber", "width": "10%" },
            {
                "data": { id: "id", isEnable: "isEnable" },
                "render": function (data) {
                    if (data.isEnable == false) {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockDepartment('${data.id}')>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
                    else {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockDepartment('${data.id}') checked>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
                },
                "width": "10%"
            }
        ],
        "language": {
            processing: "Traitement en cours...",
            search: "Rechercher...",
            lengthMenu: "Afficher _MENU_ &eacute;l&eacute;ments",
            info: "Affichage de l'\u00E9lement _START_ &agrave; _END_ sur _TOTAL_ &eacute;l&eacute;ments",
            infoEmpty: "Affichage de l'&eacute;lement 0 &agrave; 0 sur 0 &eacute;l&eacute;ments",
            infoFiltered: "(filtr&eacute; de _MAX_ &eacute;l&eacute;ments au total)",
            infoPostFix: "",
            loadingRecords: "Chargement en cours...",
            zeroRecords: "Aucun \u00E9l\u00E9ment &agrave; afficher",
            emptyTable: "Aucune donn\u00E9e disponible dans le tableau",
            paginate: {
                first: "Premier",
                previous: "Pr\u00E9c\u00E9dent",
                next: "Suivant",
                last: "Dernier"
            },
            aria: {
                sortAscending: ": activer pour trier la colonne par ordre croissant",
                sortDescending: ": activer pour trier la colonne par ordre d�croissant"
            }
        }
    });
    departmentDataTable.draw();
}

function deleteDepartment(url) {
    /*Ouvre une fen�tre modale pour confirmer la suppression de l'entreprise*/
    Swal.fire({
        title: '\u00CAtes-vous s\u00FBr?',
        text: "Vous ne serez plus capable d'annuler votre suppression!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Supprimer',
        cancelButtonText: 'Annuler',
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    /*userDataTable.ajax.reload();*/
                    toastr.success(data.message);
                }
            })
        }
    })
}

function lockUnlockDepartment(id) {
    $.ajax({
        type: "POST",
        url: '/application/department/LockUnlockDepartment',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.options = {
                    "closeButton": false,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": false,
                    "positionClass": "toast-bottom-right",
                    "preventDuplicates": false,
                    "onclick": null,
                };

                toastr.success("Modification de l'\u00E9tat du d\u00E9partement r\u00E9ussie", "Modification \u00E9tat d\u00E9partement");
                departmentDataTable.ajax.reload();
            }
        }
    });
}
/*END - Index Company*/