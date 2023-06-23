var companyFunctionnalitiesDataTable;
$(document).ready(function () {
    var companyId = document.getElementById('tableCompanyFunctionnalities').getAttribute('data-model-id');
    loadCompanyFunctionnalitiesDataTable(companyId);
});

/*START - Company Functionnalities*/
function loadCompanyFunctionnalitiesDataTable(companyId) {
    companyFunctionnalitiesDataTable = $('#tableCompanyFunctionnalities').DataTable({
        "ajax": {
            url: '/supercompany/company/GetAllFunctionnalitiesForCompanyConfiguration',
            type: "GET",
            data: JSON.stringify(companyId),
            contentType: "application/json",
        },
        "columns": [
            { "data": "name", "width": "40%" },
            { "data": "description", "width": "40%" },
            {
                "data": { id: "id", hasAccess: "hasAccess" },
                "render": function (data) {
                    if (data.hasAccess == false) {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockCompanyFunctionnality('${companyId}', '${data.id}')>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
                    else {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockCompanyFunctionnality('${companyId}', '${data.id}') checked>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
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
        }
    });
    companyFunctionnalitiesDataTable.draw();
}

function lockUnlockCompanyFunctionnality(companyId, functionnalityId) {
    var data = {
        companyId: companyId,
        functionnalityId: functionnalityId
    };

    $.ajax({
        type: "POST",
        url: '/supercompany/company/LockUnlockCompanyFunctionnalities',
        data: data,
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

                toastr.success("Test modification fonctionnalités", "Réussite");
                companyFunctionnalitiesDataTable.ajax.reload();
            }
        }
    });
}
/*END - Company Functionnalities*/