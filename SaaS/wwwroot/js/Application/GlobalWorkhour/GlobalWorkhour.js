var globalWorkHoursDataTable;

$(document).ready(function () {
    loadGlobalWorkHoursDataTable();
});

function dateOnlyFormater(date) {
    const options = {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    };
    return new Date(date).toLocaleDateString('fr-FR', options);
}

function loadGlobalWorkHoursDataTable() {
    globalWorkHoursDataTable = $('#tableGlobalWorkHours').DataTable({
        "ajax": { url: '/application/globalworkhour/getallworkhours' },
        "columns": [
            {
                "data": { id: "id", userName: "userName" },
                "render": function (data) {
                    if (data.userName != null) {
                        return `
                            <a style="color: #0000FF; text-decoration: underline;" href="/application/globalworkhour/userdatas?id=${data.id}">${data.userName}</a>
                        `
                    }
                },
                "width": "30%"
            },
            {
                "data": "workDay",
                "render": function (data) {
                    return `
                        <a>${dateOnlyFormater(data)}</a>
                    `
                },
                "width": "20%"
            },
            { "data": "morningStart", "width": "10%" },
            { "data": "morningEnd", "width": "10%" },
            { "data": "eveningStart", "width": "10%" },
            { "data": "eveningEnd", "width": "10%" },
            /*{
                "data": "id",
                "render": function (data) {
                    return `
                        <div role="group">
                            <a href="/application/workhour/edit?id=${data}" class="btn btn-primary mx-2">
                                <i class='bx bxs-edit' style='color:#ffffff'></i>
                            </a>
                            <a onClick=deleteWorkHour('/application/workhour/delete/${data}') class="btn btn-danger mx-2">
                                <i class='bx bx-trash' style='color:#ffffff'  ></i>
                            </a>
                        </div>
                    `
                },
                "width": "20%"
            }*/
        ],
        "language": {
            processing: "Traitement en cours...",
            search: "Rechercher&nbsp;:",
            lengthMenu: "Afficher _MENU_ &eacute;l&eacute;ments",
            info: "Affichage de l'&eacute;lement _START_ &agrave; _END_ sur _TOTAL_ &eacute;l&eacute;ments",
            infoEmpty: "Affichage de l'&eacute;lement 0 &agrave; 0 sur 0 &eacute;l&eacute;ments",
            infoFiltered: "(filtr&eacute; de _MAX_ &eacute;l&eacute;ments au total)",
            infoPostFix: "",
            loadingRecords: "Chargement en cours...",
            zeroRecords: "Aucun &eacute;l&eacute;ment &agrave; afficher",
            emptyTable: "Aucune donnée disponible dans le tableau",
            paginate: {
                first: "Premier",
                previous: "Pr&eacute;c&eacute;dent",
                next: "Suivant",
                last: "Dernier"
            },
            aria: {
                sortAscending: ": activer pour trier la colonne par ordre croissant",
                sortDescending: ": activer pour trier la colonne par ordre décroissant"
            }
        }
    });
    globalWorkHoursDataTable.draw();
}