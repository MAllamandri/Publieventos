$(function () {
    $('#SignUpModel_UserNameToRegister').charactersQuantity(30);

    if (data.IsLogin === true) {
        $('#login').attr('class', 'tab-pane active');
        $('#create').attr('class', 'tab-pane fane');
        $('#tabLogin').attr('class', 'active');
        $('#tabCreate').removeAttr('class');
        $('#loginModal').attr('class', 'col-md-4')
    } else {
        $('#create').attr('class', 'tab-pane active');
        $('#login').attr('class', 'tab-pane fane');
        $('#tabCreate').attr('class', 'active');
        $('#tabLogin').removeAttr('class');
        $('#loginModal').attr('class', 'col-md-6');
        $('#First').attr('class', "col-md-3");
        $('#Second').attr('class', 'col-md-3');
    }

    $('#recoverPass').click(function () {
        $('#RecoverPassModal').modal('show');
    });

    $('#tabCreate').click(function () {
        $('#loginModal').attr('class', 'col-md-6');
        $('#First').attr('class', "col-md-3");
        $('#Second').attr('class', 'col-md-3');
    });

    $('#tabLogin').click(function () {
        $('#loginModal').attr('class', 'col-md-4');
        $('#First').attr('class', "col-md-4");
        $('#Second').attr('class', 'col-md-4');
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

    $('#ResendEmail').click(function (e) {
        var l = Ladda.create(this);
        l.start();

        $.ajax({
            type: "POST",
            url: "/Account/ResendEmailActivation",
            data: { userName: $('#ResendEmailUserName').val() }
        }).done(function (data) {
            if (data.isValid) {
                l.stop();
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
});

