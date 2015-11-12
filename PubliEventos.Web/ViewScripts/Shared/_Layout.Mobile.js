$(function () {
    $('.mobile-menu').click(function (event) {
        $('nav').toggleClass('active');
        $(window).scrollTop(0);
    });

    var didScroll;
    var lastScrollTop = 0;
    var delta = 5;
    var navbarHeight = $('.header-scroll').outerHeight();

    $(window).scroll(function (event) {
        didScroll = true;
    });

    setInterval(function () {
        if (didScroll) {
            hasScrolled();
            didScroll = false;
        }
    }, 250);

    function hasScrolled() {
        var st = $(this).scrollTop();

        // Make sure they scroll more than delta
        if (Math.abs(lastScrollTop - st) <= delta)
            return;

        // If they scrolled down and are past the navbar, add class .nav-up.
        // This is necessary so you never see what is "behind" the navbar.
        if (st > lastScrollTop && st > navbarHeight) {
            // Scroll Down
            $('.header-scroll').removeClass('nav-down').addClass('nav-up');
        } else {
            // Scroll Up
            if (st <= 270) {
                $('.header-scroll').removeClass('nav-down').addClass('nav-up');
            } else if (st + $(window).height() < $(document).height()) {
                $('.header-scroll').removeClass('nav-up').addClass('nav-down');
            }
        }

        lastScrollTop = st;
    }
});