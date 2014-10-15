$(function () {
    $('#Provinces').change(function () {
        $.getJSON("/Account/GetLocalitiesByProvince", { idProvince: $('#Provinces').val() }, function (data) {
            $('#Localities option').remove();

            $('#Localities').attr('disabled', 'disabled');

            $('#Localities').append($("<option />").val("").text(['[Seleccione Localidad]']));

            $.each(data, function (index, item) {
                $('#Localities').append($("<option />").val(item.Id).text(item.Name));
            });

            $('#Localities').removeAttr('disabled');
        });
    });

    $('.date').datetimepicker({
        pickTime: false,
        format: "DD-MM-YYYY",
        language: 'es',
        autoclose: true,
    });
});