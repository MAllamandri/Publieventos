$(function () {
    $('#UserName').charactersQuantity(30);

    $('.date').datetimepicker({
        pickTime: false,
        format: "DD/MM/YYYY",
        language: 'es',
        autoclose: true
    });

    $('#ProvinceId').change(function () {
        $.getJSON("/Account/GetLocalitiesByProvince", { idProvince: $('#ProvinceId').val() }, function (data) {
            $('#LocalityId option').remove();

            $('#LocalityId').append($("<option />").val("").text(['[Seleccione Localidad]']));

            $.each(data, function (index, item) {
                $('#LocalityId').append($("<option />").val(item.Id).text(item.Name));
            });
        });
    });

    $('#Save').click(function () {
        var val = $('#ImageFile').val();

        if (val != "") {
            switch (val.substring(val.lastIndexOf('.') + 1).toLowerCase()) {
                case 'gif':
                case 'jpg':
                case 'png':
                case 'jpeg':
                    submitForm();
                    break;
                default:
                    $('#ImageFile').val('');
                    var selector = "[name='ImageFile']";
                    $("[name='ImageFile']").showMessageError("Tipo de archivo no permitido.");
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

        $('#form').ajaxForm({
            url: "/Account/EditProfile",
            datatype: 'text/json',
            beforeSubmit: function (arr, $form, options) {
                $.blockUI();
            },
            complete: function (data) {
                if (data.responseJSON.Success) {
                    $.blockUI({ message: "" });

                    window.location.href = "/Account/Profile/" + $("#UserId").val();
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

    $('#UserName').blur(function () {
        $('#alertUserExist').hide();

        if ($('#UserName').val() != "") {
            $.ajax({
                type: "POST",
                url: "/Account/ValidateExistUserName",
                async: false,
                data: {
                    userName: $('#UserName').val(),
                    userIdToExclude: $("#UserId").val()
                }
            }).done(function (data) {
                if (data.Exist) {
                    $('#alertUserExist').show();
                }
            });
        }
    });

    $('#Email').blur(function () {
        $('#alertEmailExist').hide();

        if ($('#Email').val() != "") {
            $.ajax({
                type: "POST",
                url: "/Account/ValidateExistEmail",
                async: false,
                data: {
                    email: $('#Email').val(),
                    userId: $("#UserId").val()
                }
            }).success(function (data) {
                if (data.Exist) {
                    $('#alertEmailExist').show();
                }
            });
        }
    });
});