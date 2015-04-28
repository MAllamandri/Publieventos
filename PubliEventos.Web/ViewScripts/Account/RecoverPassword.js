$(function () {
    $('#savePassword').click(function () {
        $.blockUI({ message: "" });
        $('input[type="password"]').hideMessageError();

        $.ajax({
            type: 'POST',
            url: "/Account/EditPassword",
            data: {
                UserId: $('#UserId').val(),
                NewPassword: $('#NewPassword').val(),
                RepeatPassword: $('#RepeatPassword').val(),
                ValidateCurrentPassword: false
            }
        }).done(function (data) {
            if (data.Success) {
                $.unblockUI();

                bootbox.dialog({
                    title: "<h4 class='title-modal'>Modificar Contraseña</h4>",
                    message: "<p class='font-text'>La contraseña ha sido modificada con exito</p>",
                    buttons: {
                        success: {
                            label: "Aceptar",
                            className: "btn-confirm",
                            callback: function () {
                                window.location.href = "/Account/Login";
                                $.blockUI({ message: "" });
                            }
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

    $('#validate').click(function () {
        $.blockUI({ message: "" });
        $('input[type="text"]').hideMessageError();

        $.ajax({
            type: 'POST',
            url: '/Account/ValidateRecoverPasswordCode',
            data: {
                UserId: $('#UserId').val(),
                Code: $('#Code').val()
            }
        }).done(function (data) {
            if (data.Success) {
                $('#verificationCodeForm').hide();
                $('#recoverForm').show();
                $.unblockUI();
            } else {
                $('#Code').showMessageError("El código ingresado no es correcto");
                $.unblockUI();
            }
        }).error(function () {
            alert("Ha ocurrido un error");
            $.blockUI({ message: "" });
        });
    });
});