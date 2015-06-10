$(function () {
    $('#filter').click(function () {
        if ($('#filterRegion').is(':visible')) {
            $('#filterRegion').hide();
        } else {
            $('#filterRegion').show();
        }
    });

    $('#reset').click(function () {
        $('#futures').show();
        $('#previous').show();
        $('#regionEvents').hide();
    });

    $('#search').click(function () {
        $.blockUI({ message: "<div style='font-size: 16px; padding-top: 11px;'><p>Buscando...</p><div>" });
        $.ajax({
            type: "POST",
            url: "/Event/SearchEventsUserConfirmed",
            data: {
                EventTypeId: $('#EventTypeId').val(),
                StartDate: $('#StartDate').val(),
                EndDate: $('#EndDate').val()
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