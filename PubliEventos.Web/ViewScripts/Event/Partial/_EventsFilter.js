$(function () {
    $('.date').datetimepicker({
        pickTime: false,
        format: "DD-MM-YYYY",
        language: 'es',
        autoclose: true,
    });

    $('#search').click(function () {
        $.blockUI({ message: "<div style='font-size: 16px; padding-top: 11px;'><p>Buscando...</p><div>" });
        $.ajax({
            type: "POST",
            url: "/Event/GetFilteredEvents",
            data: {
                EventTypeId: $('#EventTypeId').val(),
                StartDate: $('#StartDate').val(),
                EndDate: $('#EndDate').val(),
                myEvents: true
            },
            dataType: "html"
        }).success(function (data) {
            $('#regionEvents').html(data);
            $.unblockUI();
        });
    });

    $('#reset').click(function () {
        $('#EventTypeId').val("");
        $('#StartDate').val("");
        $('#EndDate').val("");

        $('#search').click();
    });
});