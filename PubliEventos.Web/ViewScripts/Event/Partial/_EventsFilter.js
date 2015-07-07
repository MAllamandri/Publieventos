$(function () {
    $('.date').datetimepicker({
        pickTime: false,
        format: "DD-MM-YYYY",
        language: 'es',
        autoclose: true,
    });

    $('#reset').click(function () {
        $('#EventTypeId').val("");
        $('#StartDate').val("");
        $('#EndDate').val("");
        $('input[name="Private"]').prop('checked', false);
    });
});