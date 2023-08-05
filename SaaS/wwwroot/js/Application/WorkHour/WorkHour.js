var workHoursDataTable;

var workSiteList = [];
var workSitesNumber = 0;
/*var button = document.getElementById("helpButton");
var container = document.getElementById("");*/

$(document).ready(function () {
    /*var selectedWorkSites = [];*/
    loadWorkHoursDataTable();

    //var wSListe = [
    //    "Chantier 1",
    //    "Chantier 2",
    //    "Chantier 3"
    //];

    //$("#nameInput").autocomplete({
    //    source: wSListe,
    //    select: function (event, ui) {
    //        selectedWorkSites.push(ui.item.value);
    //        wSListe.splice(wSListe.indexOf(ui.item.value), 1);
    //        addWorkSite(ui.item.value);
    //    }
    //});
    //$("#submitButton").click(function () {
    //    $.ajax({
    //        url: "/application/workhour/addworksite",
    //        method: "POST",
    //        data: { workSiteList: selectedWorkSites },
    //        success: function (response) {
    //            console.log("Réponse du serveur :", response);
    //        },
    //        error: function (xhr, status, error) {
    //            console.error("Erreur AJAX :", error);
    //        }
    //    });
    //});
});

//window.addEventListener('DOMContentLoaded', function () {
//    const cardToggles = document.querySelectorAll('.icon');
//    const comboboxes = document.querySelectorAll('.combobox');

//    cardToggles.forEach(function (toggle) {
//        toggle.addEventListener('click', function () {
//            const card = this.closest('.card');
//            card.classList.toggle('closed');
//        });
//    });
//    comboboxes.forEach(function (combobox) {
//        const select = combobox.querySelector('.combobox-select');
//        const options = combobox.querySelector('.combobox-options');
//        const optionList = combobox.querySelectorAll('.combobox-option');

//        select.addEventListener('click', function (event) {
//            event.stopPropagation();
//            options.style.display = options.style.display === 'block' ? 'none' : 'block';
//        });

//        optionList.forEach(function (option) {
//            option.addEventListener('click', function (event) {
//                event.stopPropagation();
//                select.value = this.textContent;
//                options.style.display = 'none';
//            });
//        });

//        document.addEventListener('click', function () {
//            options.style.display = 'none';
//        });
//    });
//});

//button.addEventListener("click", function () {
//    Swal.fire({
//        title: 'Informations sur les heures du salarié',
//        text: 'Do you want to continue',
//        icon: 'info',
//        confirmButtonText: 'Cool'
//    })
//});

function dateOnlyFormater(date) {
    const options = {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    };
    return new Date(date).toLocaleDateString('fr-FR', options);
}

function loadWorkHoursDataTable() {
    workHoursDataTable = $('#tableWorkHours').DataTable({
        "ajax": { url: '/application/workhour/getworkhours' },
        "columns": [
            {
                "data": "workDay",
                "render": function (data) {
                    return `
                        <a>${dateOnlyFormater(data)}</a>
                    `
                },
                "width": "10%"
            },
            { "data": "morningStart", "width": "10%" },
            { "data": "morningEnd", "width": "10%" },
            { "data": "totalMorningHours", "width": "10%" },
            { "data": "eveningStart", "width": "10%" },
            { "data": "eveningEnd", "width": "10%" },
            { "data": "totalEveningHours", "width": "10%" }
            //{ "data": "comment", "width": "10%" },
            //{ "data": "lunchbox", "width": "10%" },
            //{
            //    "data": "id",
            //    "render": function (data) {
            //        return `
            //            <div role="group">
            //                <a href="/application/workhour/edit?id=${data}" class="btn btn-primary mx-2">
            //                    <i class='bx bxs-edit' style='color:#ffffff'></i>
            //                </a>
            //                <a onClick=deleteWorkHour('/application/workhour/delete/${data}') class="btn btn-danger mx-2">
            //                    <i class='bx bx-trash' style='color:#ffffff'  ></i>
            //                </a>
            //            </div>
            //        `
            //    },
            //    "width": "20%"
            //}
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
    workHoursDataTable.draw();
}

function deleteWorkHour(url) {
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

function lockUnlockWorkHour(id) {
    $.ajax({
        type: "POST",
        url: '/application/workhour/LockUnlockWorkhour',
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


function addWorkSite(name) {
    var nameInput = document.getElementById("nameInput");
    //var name = nameInput.value;

    if (name) {
        var workSite = { name: name };
        workSiteList.push(workSite);
        displayWorkSites();

        nameInput.value = "";
    }
}

function displayWorkSites() {
    var workSiteListDiv = document.getElementById("workSiteList");
    workSiteListDiv.innerHTML = "";
    workSitesNumber = 0;
    workSiteList.forEach(function (workSite) {
        workSitesNumber = workSitesNumber + 1;
        var workSiteCard = document.createElement("div");
        workSiteCard.classList.add("workSiteCard");
        workSiteCard.textContent = `Chantier n\u00B0${workSitesNumber}: ` + workSite.name;

        workSiteListDiv.appendChild(workSiteCard);
    });
}

document.getElementById("form").addEventListener("submit", function (event) {
    var workSiteListInput = document.createElement("input");
    workSiteListInput.type = "hidden";
    workSiteListInput.name = "workSiteList";
    workSiteListInput.value = JSON.stringify(workSiteList);
    this.appendChild(workSiteListInput);
});