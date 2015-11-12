$(function () {
    $('.current-date').click(function () {
        if ($('.current-date-description').length > 0) {
            $("html, body").scrollTop($('.current-date-description').offset().top - 50);

            var onlyOneTime = 0;

            setInterval(function () {
                if (onlyOneTime == 0) {
                    $('.header-scroll').removeClass('nav-up').addClass('nav-down');
                }

                onlyOneTime = 1;
            }, 250);
        }
    });
});