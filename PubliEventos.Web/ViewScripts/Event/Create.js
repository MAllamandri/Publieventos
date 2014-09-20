$(function () {
    $('.date').datetimepicker({
        pickTime: false,
        format: "DD-MM-YYYY",
        language: 'es',
        maxDate: new Date().toLocaleDateString()
    });
});