var departmentWorkersDataTable;

$(document).ready(function () {
    /*var url = window.location.search;
    if (url.includes("unClotured")) {
        loadWorkSitesDataTable("unClotured");
    }
    else {
        if (url.includes("clotured")) {
            loadWorkSitesDataTable("clotured");
        }
        else {
            loadWorkSitesDataTable("all");
        }
    }*/
    loadDepartmentWorkersDataTable();
});

function loadDepartmentDataTable() {
    departmentWorkersDataTable = $('#tableDepartmentWorkers').DataTable({
        "ajax": { url: '/application/department/getalldepartmentworkers' },
        "columns": [
            {
                "data": { id: "id", name: "userName" },
                "render": function (data) {
                    if (data.name != null) {
                        return `
                            <a style="color: #0000FF; text-decoration: underline;" href="/application/user/details?id=${data.id}">${data.userName}</a>
                        `
                    }
                },
                "width": "25%"
            },
            { "data": "email", "width": "25%" },
            { "data": "phoneNumber", "width": "25%" },
            { "data": "jobName", "width": "25%" }
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
                sortDescending: ": activer pour trier la colonne par ordre décroissant"
            }
        }
    });
    departmentWorkersDataTable.draw();
}