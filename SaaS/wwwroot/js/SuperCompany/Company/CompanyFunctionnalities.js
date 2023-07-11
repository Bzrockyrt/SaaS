var companyUsersDataTable;

$(document).ready(function () {
    loadCompanyUsersDataTable();
});
$(window).on('beforeunload', function () {
    $.ajax({
        url: '/supercompany/company/exitcompany',
        type: 'POST',
        async: false, // Synchronisez la requ�te si n�cessaire
        data: {}, // Ajoutez des donn�es suppl�mentaires si n�cessaire
        success: function (result) {
            // Traitez la r�ponse du contr�leur
        },
        error: function (xhr, status, error) {
            // Traitez les erreurs �ventuelles
        }
    });
});

$(function () {
    $("#sortable1").sortable({
        connectWith: "#sortable2",
    }).disableSelection();

    $("#sortable2").sortable({
        connectWith: "#sortable1",
        receive: function (event, ui) {
            var movedElement = ui.item
            var targetList = $(this);

            $.ajax({
                url: '/supercompany/company/addfunctionnalitytocompany?functionnalityName=' + encodeURIComponent(movedElement[0].innerText),
                type: 'POST',
            });
        },
        remove: function (event, ui) {
            var movedElement = ui.item;
            var sourceList = $(this);

            $.ajax({
                url: '/supercompany/company/deletefunctionnalitytocompany?functionnalityName=' + encodeURIComponent(movedElement[0].innerText),
                type: 'POST',
            });
        }
    }).disableSelection();
});


function loadCompanyUsersDataTable() {
    companyUsersDataTable = $('#tableCompanyUsers').DataTable({
        /*ajax : renseigne le chemin de la m�thode � invoquer pour r�cup�rer le contenu de la table*/
        "ajax": { url: '/supercompany/company/getcompanyusers' },
        /*columns : permet de configurer les colonnes de la table avec diff�rents param�tres*/
        "columns": [
            { "data": "id", "width": "30%" },
            { "data": "username", "width": "35%" },
            {
                "data": { id: "id", isEnable: "isEnable" },
                "render": function (data) {
                    if (data.isEnable == false) {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockCompanyUser('${data.id}')>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
                    else {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockCompanyUser('${data.id}') checked>
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
                            <a href="/supercompany/company/edituser?id=${data}" class="btn btn-primary mx-2">
                                <i class='bx bxs-edit' style='color:#ffffff'></i>
                            </a>
                            <a onClick=deleteUser('/supercompany/company/deleteuser/${data}') class="btn btn-danger mx-2">
                                <i class='bx bx-trash' style='color:#ffffff'  ></i>
                            </a>
                        </div>
                    `
                },
                "width": "15%"
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
            emptyTable: "Aucune donn�e disponible dans le tableau",
            paginate: {
                first: "Premier",
                previous: "Pr&eacute;c&eacute;dent",
                next: "Suivant",
                last: "Dernier"
            },
            aria: {
                sortAscending: ": activer pour trier la colonne par ordre croissant",
                sortDescending: ": activer pour trier la colonne par ordre d�croissant"
            }
        }
    });
    companyUsersDataTable.draw();
}