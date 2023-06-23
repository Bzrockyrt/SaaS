function animateButton() {
    var button = document.querySelector('.animated-button');
    button.classList.add('clicked');

    setTimeout(function () {
        button.classList.remove('clicked');
    }, 300);
}