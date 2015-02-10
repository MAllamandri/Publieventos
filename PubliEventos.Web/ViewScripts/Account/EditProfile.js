$(function () {
    $('#UserName').charactersQuantity(30);

    $('.replaceBlank').blur(function () {
        $(this).val($(this).val().replace(/ /gi, ""));
    });

    $('.date').datetimepicker({
        pickTime: false,
        format: "DD/MM/YYYY",
        language: 'es',
        autoclose: true
    });

    $('#EditPassword').click(function () {
        $('#EditPassModal').modal('show');
        $('input[type="password"]').hideMessageError();
        $('input[type="password"]').val("");
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

    $('#savePassword').click(function () {
        $.blockUI({ message: "" });
        $('input[type="password"]').hideMessageError();

        $.ajax({
            type: 'POST',
            url: "/Account/EditPassword",
            data: {
                UserId: $('#UserId').val(),
                CurrentPassword: $('#CurrentPassword').val(),
                OldPassword: $('#OldPassword').val(),
                NewPassword: $('#NewPassword').val(),
                RepeatPassword: $('#RepeatPassword').val()
            }
        }).done(function (data) {
            if (data.Success) {
                $('#EditPassModal').modal('hide');
                $.unblockUI();

                bootbox.dialog({
                    title: "<h4 class='title-modal'>Modificar Contraseña</h4>",
                    message: "<p class='font-text'>La contraseña ha sido modificada con exito</p>",
                    buttons: {
                        success: {
                            label: "Aceptar",
                            className: "btn-confirm"
                        }
                    }
                });
            } else {
                $.each(data.Errors, function (index, value) {
                    var selector = "[name='" + index + "']";
                    $(selector).showMessageError(value);
                });
                $.unblockUI();
            }
        });
    });

    $('#Save').click(function () {
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
                    $('#imageCurrentUser').attr('src', '/Content/images/Profiles/' + data.responseJSON.ImageProfile);

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
    });

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