$(function () {
    $('#tabDetail').click(function () {
        removeActiveClass();
        $('#regionDetail').show();
        $('#regionLocalization').hide();
        $('#regionContents').hide();
        $(this).addClass("active");
    });

    $('#tabLocalization').click(function () {
        removeActiveClass();
        $('#regionLocalization').show();
        $('#regionDetail').hide();
        $('#regionContents').hide();
        $(this).addClass("active");
    });

    $('#tabContents').click(function () {
        removeActiveClass();
        $('#regionContents').show();
        $('#regionDetail').hide();
        $('#regionLocalization').hide();
        $(this).addClass("active");
    });

    function removeActiveClass() {
        $('#tabContents').removeClass("active");
        $('#tabLocalization').removeClass("active");
        $('#tabDetail').removeClass("active");
    }
});