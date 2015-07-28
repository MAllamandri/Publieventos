$(function () {
    if (isRegister) {
        $('#myTabs a[href="#create"]').tab('show');
    } else {
        $('#myTabs a[href="#login"]').tab('show');
    }

    $('#SignUpModel_UserNameToRegister').charactersQuantity(20);
    $('#SignUpModel_FirstName').charactersQuantity(20);
    $('#SignUpModel_LastName').charactersQuantity(20);

    $('#myTabs a').click(function (e) {
        e.preventDefault()
        $(this).tab('show')
    });

    $('#recoverPass').click(function () {
        $('#RecoverPassModal').modal('show');
    });

    $('.date').datetimepicker({
        pickTime: false,
        format: "DD-MM-YYYY",
        language: 'es',
        maxDate: new Date().toLocaleDateString()
    });

    $('#SignUpModel_UserNameToRegister').blur(function () {
        $('#alertUserExist').hide();

        $(this).val($(this).val().replace(" ", ""));

        if (existUser()) {
            $('#alertUserExist').show();
        }
    });

    $('#SignUpModel_Email').blur(function () {
        $('#alertEmailExist').hide();

        if (existEmail()) {
            $('#alertEmailExist').show();
        }
    });

    $('#Entry').click(function () {
        $.blockUI({ message: "" });

        $('span[name= "Error"]').hideMessageError();

        $.ajax({
            type: "POST",
            url: "/Account/Login",
            data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                UserName: $('#UserName').val(),
                Password: $('#Password').val(),
                RememberMe: $('#RememberMe:checked').val(),
                ReturnUrl: getParameterByName("ReturnUrl")
            }
        }).done(function (data) {
            if (data.Success) {
                window.location.href = data.ReturnUrl;
            } else {
                $.each(data.Errors, function (index, value) {
                    var selector = "[name='" + index + "']";
                    $(selector).showMessageError(value);
                });

                if (data.IsExpirate) {
                    $('#resendEmailMessage').show();
                }
                $.unblockUI();
            }
        });
    });

    $('#Register').click(function () {
        $('input[type= "text"]').hideMessageError();
        $('input[type= "password"]').hideMessageError();
        $('select').hideMessageError();

        if ($('#SignUpModel_UserNameToRegister').val() != "" && !existEmail() && $('#SignUpModel_Email').val() != "" && !existUser()) {

            $('#RegisterForm').ajaxForm({
                url: "/Account/SignUp",
                datatype: 'text/json',
                beforeSubmit: function (arr, $form, options) {
                    $.blockUI({
                        message: "<div style='font-size: 16px; padding-top: 11px;'><p>Registrando...</p><div>"
                    });
                },
                complete: function (data) {
                    if (data.responseJSON.Success) {
                        $('.RegisterRow').hide();
                        $('#RegisterSuccess').show();

                    } else {
                        $.each(data.responseJSON.Errors, function (index, value) {
                            var selector = "[name='" + index + "']";
                            $(selector).showMessageError(value);
                        });
                    }
                    $.unblockUI();
                }
            });
        } else {
            $('#SignUpModel_UserNameToRegister').showMessageError("El campo usuario es obligatorio.");
            $('#SignUpModel_Email').showMessageError("El campo email es obligatorio.");
            return false;
        }
    });

    $('#Province').change(function () {
        $.getJSON("/Account/GetLocalitiesByProvince", { idProvince: $('#Province').val() }, function (data) {
            $('#SignUpModel_Locality option').remove();

            $('#SignUpModel_Locality').attr('disabled', 'disabled');

            $('#SignUpModel_Locality').append($("<option />").val("").text(['[Seleccione Localidad]']));

            $.each(data, function (index, item) {
                $('#SignUpModel_Locality').append($("<option />").val(item.Id).text(item.Name));
            });

            $('#SignUpModel_Locality').removeAttr('disabled');
        });
    });

    $("#myModal").click(function () {
        $("#Active").show();
    });

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        if (e.target.attributes[0].value == '#login') {
            $('#myTabs').attr('style', 'width: 48%');
        } else if (e.target.attributes[0].value == '#create') {
            $('#myTabs').attr('style', 'width: 100%');
        }
    })

    $('#ResendEmail').click(function (e) {
        var l = Ladda.create(this);
        l.start();

        $.ajax({
            type: "POST",
            url: "/Account/ResendEmailActivation",
            data: { userName: $('#UserName').val() }
        }).done(function (data) {
            if (data.isValid) {
                l.stop();
                $('#resendEmailMessage').hide();
                $('#Success').show();
            } else {
                alert("Error al enviar email.")
            }

            $("#Active").hide();
        }).error(function () {
            alert("Error al enviar email.")
        });
    });

    function existUser() {
        var existUser;

        $.ajax({
            type: "POST",
            url: "/Account/ValidateExistUserName",
            async: false,
            data: { userName: $('#SignUpModel_UserNameToRegister').val() }
        }).done(function (data) {
            if (data.Exist) {
                existUser = true;
            } else {
                existUser = false;
            }
        });

        return existUser;
    }

    function existEmail() {
        var existEmail;

        $.ajax({
            type: "POST",
            url: "/Account/ValidateExistEmail",
            async: false,
            data: { email: $('#SignUpModel_Email').val() }
        }).success(function (data) {
            if (data.Exist) {
                existEmail = true;
            } else {
                existEmail = false;
            }
        });

        return existEmail;
    }

    $('#recoverPassword').click(function () {
        if ($('#UserNameToRecover').val() != null && $.trim($('#UserNameToRecover').val()) != "") {
            $('#RecoverPassModal').modal('hide');
            $.blockUI({ message: "" });

            $.ajax({
                type: "POST",
                url: "/Account/SendRecoverPasswordCode",
                async: false,
                data: { UserName: $('#UserNameToRecover').val() }
            }).done(function (data) {
                if (data.Success) {
                    window.location.href = "/Account/RecoverPassword/" + data.UserId;
                } else {
                    $.unblockUI();
                    alert("Ha ocurrido un error");
                }
            }).error(function () {
                alert("Ha ocurrido un error interno");
                $.unblockUI();
            });
        } else {
            $('#UserNameToRecover').showMessageError("");
        }
    });
});

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

