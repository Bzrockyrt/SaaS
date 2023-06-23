$("#timepicker").flatpickr({
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    time_24hr: true,
    locale: "fr",
    theme: "light",
    minuteIncrement: 15,
});

$("#timepickerMorningStart").flatpickr({
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    defaultDate : "08:00",
    time_24hr: true,
    locale: "fr",
    theme: "light",
    minuteIncrement: 15,
});

$("#timepickerMorningEnd").flatpickr({
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    defaultDate: "12:00",
    time_24hr: true,
    locale: "fr",
    theme: "light",
    minuteIncrement: 15,
});

$("#timepickerEveningStart").flatpickr({
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    defaultDate: "13:00",
    time_24hr: true,
    locale: "fr",
    theme: "light",
    minuteIncrement: 15,
});

$("#timepickerEveningEnd").flatpickr({
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    defaultDate: "17:00",
    time_24hr: true,
    locale: "fr",
    theme: "light",
    minuteIncrement: 15,
});