$(document).ready(function () {
    function showNav() {
        $("#nav-left").removeClass("hidden");
        $("#myOverlay").removeClass("hidden");
    }
    function hideNav() {
        $("#nav-left").addClass("hidden");
        $("#myOverlay").addClass("hidden");
    }
    $('#show-nav').on('click', function () {
        showNav();
    });
    $('.hide-nav').on('click', function () {
        hideNav();
    });
    //check mobile device
    function isMobile() {
        if (navigator.userAgent.match(/Android/i)
        || navigator.userAgent.match(/webOS/i)
        || navigator.userAgent.match(/iPhone/i)
        || navigator.userAgent.match(/iPad/i)
        || navigator.userAgent.match(/iPod/i)
        || navigator.userAgent.match(/BlackBerry/i)
        || navigator.userAgent.match(/Windows Phone/i)
        ) {
            return true;
        }
        else {
            return false;
        }
    }

    if (!isMobile())
        showNav();
});