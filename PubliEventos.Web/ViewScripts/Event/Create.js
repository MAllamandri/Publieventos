$(function () {
    $('#Description').keypress(function () {
        if ($('#Description').val().length > 139) {
            $('#Description').val($('#Description').val().substring(0, $('#Description').val().length - 1));
        }
    });

    $('#Title').keypress(function () {
        if ($('#Title').val().length > 100) {
            $('#Title').val($('#Title').val().substring(0, $('#Title').val().length - 1));
        }
    });

    $('.date').datetimepicker({
        pickTime: false,
        format: "DD-MM-YYYY",
        language: 'es',
        autoclose: true,
        minDate: new Date().toLocaleDateString()
    });

    $('.time').datetimepicker({
        pickDate: false,
        language: 'es',
        autoclose: true
    });

    $('.btn-cancel').click(function () {
        window.location.href = "/Home/Index";
    });

    $('#ProvinceId').change(function () {
        $.getJSON("/Account/GetLocalitiesByProvince", { idProvince: $('#ProvinceId').val() }, function (data) {
            $('#LocalityId option').remove();

            $('#LocalityId').attr('disabled', 'disabled');

            $('#LocalityId').append($("<option />").val("").text(['[Seleccione Localidad]']));

            $.each(data, function (index, item) {
                $('#LocalityId').append($("<option />").val(item.Id).text(item.Name));
            });

            $('#LocalityId').removeAttr('disabled');
        });
    });

    $('form').submit(function () {
        $.blockUI({
            message: "<div style='font-size: 16px; padding-top: 11px;'><p>Guardando...</p><div>"
        });
    });

    $('#Save').click(function () {
        var val = $('#CoverPhoto').val();

        if (val != "") {
            switch (val.substring(val.lastIndexOf('.') + 1).toLowerCase()) {
                case 'gif':
                case 'jpg':
                case 'png':
                case 'jpeg':
                    $('form').submit();
                    break;
                default:
                    $('#CoverPhoto').val('');
                    $('span[name = "ErrorCoverPhoto"]').text('La extension del archivo no es correcta.').show();
                    break;
            }
        } else {
            $('form').submit();
        }
    });
});