$(function () {
    $('#reset').click(function () {
        $('#futures').show();
        $('#previous').show();
        $('#regionEvents').hide();
    });

    $("#searchButton").click(function () {
        $('#search-form').slideToggle("slow", function () {
            if ($('#icon-search').hasClass('icon-caret-down')) {
                $('#icon-search').removeClass('icon-caret-down');
                $('#icon-search').addClass('icon-caret-up');
            } else if ($('#icon-search').hasClass('icon-caret-up')) {
                $('#icon-search').removeClass('icon-caret-up');
                $('#icon-search').addClass('icon-caret-down');
            }
        });
    });

    $('#search').click(function () {
        $.blockUI({ message: "<div style='font-size: 16px; padding-top: 11px;'><p>Buscando...</p><div>" });
        $.ajax({
            type: "POST",
            url: "/Event/SearchEventsUserConfirmed",
            data: {
                EventTypeId: $('#EventTypeId').val(),
                StartDate: $('#StartDate').val(),
                EndDate: $('#EndDate').val(),
                SearchPublics: $('input[name="Private"]:checked').val() == 'true' ? true : false,
                SearchPrivates: $('input[name="Private"]:checked').val() == 'false' ? true : false
            },
            dataType: "html"
        }).success(function (data) {
            $('#regionEvents').show();
            $('#regionEvents').html(data);
            $('#futures').hide();
            $('#previous').hide();

            $.unblockUI();
        });
    });
});