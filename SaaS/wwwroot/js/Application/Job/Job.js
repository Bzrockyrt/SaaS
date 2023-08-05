var jobDataTable;

$(document).ready(function () {
    loadJobDataTable();
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
                url: '/application/job/addfunctionnalitytojob?functionnalityName=' + encodeURIComponent(movedElement[0].innerText),
                type: 'POST',
            });
        },
        remove: function (event, ui) {
            var movedElement = ui.item;
            var sourceList = $(this);

            $.ajax({
                url: '/application/job/deletefunctionnalitytojob?functionnalityName=' + encodeURIComponent(movedElement[0].innerText),
                type: 'POST',
            });
        }
    }).disableSelection();
});

/*START - Index Company*/
function loadJobDataTable() {
    jobDataTable = $('#tableJobs').DataTable({
        "ajax": { url: '/application/job/getalljobs' },
        "columns": [
            {
                "data": { id: "id", name: "name" },
                "render": function (data) {
                    if (data.name != null) {
                        return `
                            <a style="color: #0000FF; text-decoration: underline;" href="/application/job/details?id=${data.id}">${data.name}</a>
                        `
                    }
                },
                "width": "30%"
            },
            { "data": "code", "width": "20%" },
            { "data": "subsidiaryName", "width": "20%" },
            { "data": "departmentName", "width": "10%" },
            { "data": "employeesNumber", "width": "10%" },
            {
                "data": { id: "id", isEnable: "isEnable" },
                "render": function (data) {
                    if (data.isEnable == false) {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockJob('${data.id}')>
                                <span class="slider round"></span>
                            </label>
                        `
                    }
                    else {
                        return `
                            <label class="switch">
                                <input type="checkbox" onclick=lockUnlockJob('${data.id}') checked>
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
    jobDataTable.draw();
}

function deleteJob(url) {
    /*Ouvre une fenêtre modale pour confirmer la suppression de l'entreprise*/
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

function lockUnlockJob(id) {
    $.ajax({
        type: "POST",
        url: '/application/job/LockUnlockJob',
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

                toastr.success("Modification de l'\u00E9tat du poste r\u00E9ussie", "Modification \u00E9tat poste");
                jobDataTable.ajax.reload();
            }
        }
    });
}
/*END - Index Company*/