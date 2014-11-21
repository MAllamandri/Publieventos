$(function () {
    $('#tabDetail').click(function () {
        removeActiveClass();
        $('#regionLocalization').hide();
        $('#regionContents').hide();
        $(this).addClass("active-link");
        $('#regionDetail').show();
    });

    $('#tabLocalization').click(function () {
        removeActiveClass();
        $('#regionDetail').hide();
        $('#regionContents').hide();
        $('#regionLocalization').show();
        $(this).addClass("active-link");

        var center = map.getCenter();
        google.maps.event.trigger(map, "resize");
        map.setCenter(center);
    });

    $('#tabContents').click(function () {
        removeActiveClass();
        $('#regionDetail').hide();
        $('#regionLocalization').hide();
        $('#regionContents').show();
        $(this).addClass("active-link");
    });

    function removeActiveClass() {
        $('#tabContents').removeClass("active-link");
        $('#tabLocalization').removeClass("active-link");
        $('#tabDetail').removeClass("active-link");
    }
});