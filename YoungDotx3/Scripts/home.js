$(document).ready(function () {

    var start = -50;

    function rotateX() {
        setTimeout(function () {
            if (start <= 0) {
                $('#main').css({ transform: 'rotateX(' + start + 'deg)' });
                start++;
                rotateX();
            }
        }, 10);
    }

    rotateX();
});