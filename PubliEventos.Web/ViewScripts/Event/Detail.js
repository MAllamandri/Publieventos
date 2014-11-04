$(function () {
    $('#tabDetail').click(function () {
        removeActiveClass();
        $('#regionDetail').show();
        $('#regionLocalization').hide();
        $('#regionContents').hide();
        $(this).addClass("active-link");
    });

    $('#tabLocalization').click(function () {
        removeActiveClass();
        $('#regionLocalization').show();
        $('#regionDetail').hide();
        $('#regionContents').hide();
        $(this).addClass("active-link");
    });

    $('#tabContents').click(function () {
        removeActiveClass();
        $('#regionContents').show();
        $('#regionDetail').hide();
        $('#regionLocalization').hide();
        $(this).addClass("active-link");
    });

    function removeActiveClass() {
        $('#tabContents').removeClass("active-link");
        $('#tabLocalization').removeClass("active-link");
        $('#tabDetail').removeClass("active-link");
    }
});