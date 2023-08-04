var workSitesDataTable;

$(document).ready(function () {
    var url = window.location.search;
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
    }
});

function loadWorkSitesDataTable(status) {
    workSitesDataTable = $('#tableWorkSites').DataTable({
        "ajax": { url: '/application/worksite/getallworksites?status=' + status },
        "columns": [
            { "data": "name", "width": "35%" },
            { "data": "description", "width": "35%" },
            {
                "data": { id: "id", isEnable: "isEnable" },
                "render": function (data) {
                    if (data.isEnable == false) {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockWorkSite('${data.id}')>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
                    else {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockWorkSite('${data.id}') checked>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
                },
                "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div role="group">
                            <a href="/application/worksite/edit?id=${data}" class="btn btn-primary mx-2">
                                <i class='bx bxs-edit' style='color:#ffffff'></i>
                            </a>
                            <a onClick=deleteWorkSite('/application/worksite/delete/${data}') class="btn btn-danger mx-2">
                                <i class='bx bx-trash' style='color:#ffffff'  ></i>
                            </a>
                        </div>
                    `
                },
                "width": "20%"
            }
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
        },
        responsive: true
    });
    workSitesDataTable.draw();
}

function deleteWorkSite(url) {
    Swal.fire({
        title: 'Êtes-vous sûr?',
        text: "Vous ne serez plus capable d'annuler votre suppression!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Oui, supprimer!',
        cancelButtonText: 'Annuler',
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    workSitesDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}

function lockUnlockWorkSite(id) {
    $.ajax({
        type: "POST",
        url: '/application/worksite/LockUnlockWorkSite',
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

                toastr.success("Modification de l'état du chantier réussie", "Modification état chantier");
                workSitesDataTable.ajax.reload();
            }
        }
    });
}
