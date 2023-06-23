var dailyHoursDataTable;
var button = document.getElementById("helpButton");
var container = document.getElementById("");

$(document).ready(function () {
    /*$('.select2').select2();*/
    var v = currentUser.username;
    var f = moninstance;
    loadDailyHoursDataTable();
});

window.addEventListener('DOMContentLoaded', function () {
    const cardToggles = document.querySelectorAll('.icon');
    const comboboxes = document.querySelectorAll('.combobox');

    cardToggles.forEach(function (toggle) {
        toggle.addEventListener('click', function () {
            const card = this.closest('.card');
            card.classList.toggle('closed');
        });
    });
    comboboxes.forEach(function (combobox) {
        const select = combobox.querySelector('.combobox-select');
        const options = combobox.querySelector('.combobox-options');
        const optionList = combobox.querySelectorAll('.combobox-option');

        select.addEventListener('click', function (event) {
            event.stopPropagation();
            options.style.display = options.style.display === 'block' ? 'none' : 'block';
        });

        optionList.forEach(function (option) {
            option.addEventListener('click', function (event) {
                event.stopPropagation();
                select.value = this.textContent;
                options.style.display = 'none';
            });
        });

        document.addEventListener('click', function () {
            options.style.display = 'none';
        });
    });
});

button.addEventListener("click", function () {
    Swal.fire({
        title: 'Informations sur les heures du salarié',
        text: 'Do you want to continue',
        icon: 'info',
        confirmButtonText: 'Cool'
    })
});

function loadDailyHoursDataTable() {
    dailyHoursDataTable = $('#tableDailyHours').DataTable({
        "ajax": { url: '/application/dailyhours/getdailyhoursofuser' },
        "columns": [
            { "data": "workday", "width": "10%" },
            { "data": "morningStart", "width": "10%" },
            { "data": "morningEnd", "width": "10%" },
            { "data": "eveningStart", "width": "10%" },
            { "data": "eveningEnd", "width": "10%" },
            { "data": "comment", "width": "10%" },
            { "data": "lunchBox", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    /*Si l'utilisateur possède les droits pour modifier ses heures, le bouton modifier s'affiche.
                      S'il ne les possède pas, rien de n'affiche*/
                    return `
                        <div role="group">
                            <a href="/supercompany/company/edit?id=${data}" class="btn btn-primary mx-2">
                                <i class='bx bxs-edit' style='color:#ffffff'></i>
                            </a>
                            <a href="/supercompany/company/configuration?id=${data}" class="btn btn-warning mx-2">
                                <i class='bx bx-cog' style='color:#ffffff' ></i>
                            </a>
                            <a onClick=deleteCompany('/supercompany/company/delete/${data}') class="btn btn-danger mx-2">
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
        }
    });
    dailyHoursDataTable.draw();
}