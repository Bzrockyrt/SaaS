﻿var companyDataTable;

$(document).ready(function () {
    loadCompanyDataTable();
});

/*START - Index Company*/
function loadCompanyDataTable() {
    companyDataTable = $('#tableCompany').DataTable({
        /*ajax : renseigne le chemin de la méthode à invoquer pour récupérer le contenu de la table*/
        "ajax": { url: '/supercompany/company/getallcompanies' },
        /*columns : permet de configurer les colonnes de la table avec différents paramètres*/
        "columns": [
            {
                "data": { id: "id", name: "name" },
                "render": function (data) {
                    if (data.name != null) {
                        return `
                            <a style="color: #0000FF; text-decoration: underline;" href="/supercompany/company/details?id=${data.id}">${data.name}</a>
                        `
                    }
                },
                "width": "10%"
            },
            { "data": "description", "width": "10%" },
            { "data": "siret", "width": "10%" },
            { "data": "email", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "companyCode", "width": "10%" },
            { "data": "tenantCode", "width": "10%" },
            {
                "data": { id: "id", isEnable: "isEnable" },
                "render": function (data) {
                    if (data.isEnable == false) {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockCompany('${data.id}')>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
                    else {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockCompany('${data.id}') checked>
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
                            <a href="/supercompany/company/configuration?id=${data}" class="btn btn-primary mx-2">
                                <i class='bx bx-cog' style='color:#ffffff;'></i>
                            </a>
                        </div>
                    `
                },
                "width": "10%"
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
        }
    });
    companyDataTable.draw();
}

function deleteCompany(url) {
    /*Ouvre une fenêtre modale pour confirmer la suppression de l'entreprise*/
    Swal.fire({
        title: 'Êtes-vous sûr?',
        text: "Vous ne serez plus capable d'annuler votre suppression!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Oui, supprimer!',
        cancelButtonText : 'Annuler',
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    companyDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}

function lockUnlockCompany(id) {
    $.ajax({
        type: "POST",
        url: '/supercompany/company/LockUnlockCompany',
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

                toastr.success("Modification de l'état de l'entreprise réussie", "Modification état entreprise");
                companyDataTable.ajax.reload();
            }
        }
    });
}
/*END - Index Company*/