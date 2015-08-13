$(function () {
    if (data.Private) {
        $('#Private').prop('checked', true);;
    }

    if (!data.EnabledEdition) {
        $('#EventDate').attr('disabled', 'disabled');
        $('#EventStartTime').attr('disabled', 'disabled');
        $('#EventEndTime').attr('disabled', 'disabled');
    }

    // Seteo el valor con los mismos formatos que los elementos.
    $('#EventDate').val(data.EventDate.split("-")[2].split("T")[0] + "-" + data.EventDate.split("-")[1] + "-" + data.EventDate.split("-")[0]);
    $('#EventStartTime').val(data.EventStartTime.split(":")[0] + ":" + data.EventStartTime.split(":")[1]);
    $('#EventEndTime').val(data.EventEndTime.split(":")[0] + ":" + data.EventEndTime.split(":")[1]);

    $('#Description').charactersQuantity(119);
    $('#Title').charactersQuantity(49);

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

    $('#Save').click(function () {
        submitForm();
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
                    window.location.href = "/Event/Detail/" + data.responseJSON.EventId;
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