var userDataTable;

$(document).ready(function () {
    loadUserDataTable();
});

/*START - Index Company*/
function loadUserDataTable() {
    userDataTable = $('#tableEmployees').DataTable({
        "ajax": { url: '/application/user/getallusers' },
        "columns": [
            {
                "data": { id: "id", name: "userName" },
                "render": function (data) {
                    if (data.userName != null) {
                        return `
                            <a style="color: #0000FF; text-decoration: underline;" href="/application/user/details?id=${data.id}">${data.userName}</a>
                        `
                    }
                },
                "width": "15%"
            },
            { "data": "email", "width": "20%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "jobName", "width": "15%" },
            { "data": "departmentName", "width": "15%" },
            { "data": "subsidiaryName", "width": "15%" },
            {
                "data": { id: "id", isEnable: "isEnable" },
                "render": function (data) {
                    if (data.isEnable == false) {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockUser('${data.id}')>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
                    else {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockUser('${data.id}') checked>
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
    userDataTable.draw();
}

function deleteUser(url) {
    /*Ouvre une fenêtre modale pour confirmer la suppression de l'entreprise*/
    Swal.fire({
        title: '\u00CAtes-vous s\u00FBr de supprimer cet utilisateur?',
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
                    /*userDataTable.ajax.reload();*/
                    toastr.success(data.message);
                }
            })
        }
    })
}

function lockUnlockUser(id) {
    $.ajax({
        type: "POST",
        url: '/application/user/LockUnlockUser',
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

                toastr.success("Modification de l'\u00E9tat de l'utilisateur r\u00E9ussie", "Modification \u00E9tat utilisateur");
                userDataTable.ajax.reload();
            }
        }
    });
}
/*END - Index Company*/