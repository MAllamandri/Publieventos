$(function () {
    $('#Provinces').change(function () {
        $.getJSON("/Account/GetLocalitiesByProvince", { idProvince: $('#Provinces').val() }, function (data) {
            $('#LocalityId option').remove();

            $('#LocalityId').attr('disabled', 'disabled');

            $('#LocalityId').append($("<option />").val("").text(['[Seleccione Localidad]']));

            $.each(data, function (index, item) {
                $('#LocalityId').append($("<option />").val(item.Id).text(item.Name));
            });

            $('#LocalityId').removeAttr('disabled');
        });
    });

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
                LocalityId: $('#LocalityId').val(),
                EventTypeId: $('#EventTypeId').val(),
                StartDate: $('#StartDate').val(),
                EndDate: $('#EndDate').val(),
                MyEvents: true
            },
            dataType: "html"
        }).success(function (data) {
            $('#regionEvents').html(data);
            $.unblockUI();
        });
    });

    $('#reset').click(function () {
        $('#LocalityId').val("");
        $('#EventTypeId').val("");
        $('#StartDate').val("");
        $('#EndDate').val("");

        $('#search').click();
    });
});