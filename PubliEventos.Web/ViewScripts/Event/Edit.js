$(function () {
    if (data.Private) {
        $('#Private').prop('checked', true);;
    }

    // Seteo el valor con los mismos formatos que los elementos.
    $('#EventDate').val(data.EventDate.split("-")[2].split("T")[0] + "-" + data.EventDate.split("-")[1] + "-" + data.EventDate.split("-")[0]);
    $('#EventStartTime').val(data.EventStartTime.split(":")[0] + ":" + data.EventStartTime.split(":")[1]);
    $('#EventEndTime').val(data.EventEndTime.split(":")[0] + ":" + data.EventEndTime.split(":")[1]);

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

    $('#Save').click(function () {
        var val = $('#CoverPhoto').val();

        if (val != "") {
            switch (val.substring(val.lastIndexOf('.') + 1).toLowerCase()) {
                case 'gif':
                case 'jpg':
                case 'png':
                case 'jpeg':
                    submitForm();
                    break;
                default:
                    $('#CoverPhoto').val('');
                    $('span[name = "ErrorCoverPhoto"]').text('La extension del archivo no es correcta.').show();
                    return false;
                    break;
            }
        } else {
            submitForm();
        }
    });

    function submitForm() {
        $('input[type="text"]').hideMessageError();
        $('select').hideMessageError();

        $('#EditForm').ajaxForm({
            url: "/Event/Edit",
            datatype: 'text/json',
            beforeSubmit: function (arr, $form, options) {
                $.blockUI();
            },
            complete: function (data) {
                if (data.responseJSON.Success) {
                    $.blockUI({ message: "" });
                    window.location.href = "/Event/MyEvents?currentEvents=true";
                } else {
                    $.each(data.responseJSON.Errors, function (index, value) {
                        var selector = "[name='" + index + "']";
                        $(selector).showMessageError(value);
                    });

                    $.unblockUI();
                }
            }
        });
    }
});