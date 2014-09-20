$(function () {
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

    $('#SignUpModel_BirthDate').datetimepicker({
        pickTime: false,
        format: "DD-MM-YYYY",
        language: 'es',
        maxDate: new Date().toLocaleDateString()
    });

    $('#SignUpModel_UserNameToRegister').blur(function () {
        $('#alertUserExist').hide();

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
        if ($('#SignUpModel_UserNameToRegister').val() != "" && !existEmail() && $('#SignUpModel_Email').val() != "" && !existUser()) {

            $('#RegisterForm').ajaxForm({
                url: "/Account/SignUp",
                datatype: 'text/json',
                //data: $('form').serializeArray(),
                beforeSubmit: function (arr, $form, options) {
                    $.blockUI({
                        message: "<div style='font-size: 16px; padding-top: 11px;'><p>Registrando...</p><div>"
                    });
                },
                complete: function (data) {
                    if (data.Success) {
                        $('.RegisterRow').hide();
                        $('#RegisterSuccess').show();
                    } else {
                        //var validator = $("#RegisterForm").validate();

                        $.each(data.responseJSON.Keys, function (index, item) {
                            //var selector = $('input[name="' + item + '"]').val(data.responseJSON.Errors[index][0].ErrorMessage);
                            //var property = item.toString();
                            //var error = data.responseJSON.Errors[index][0].ErrorMessage.toString();
                            if (data.responseJSON.Errors[index] != "") {
                                $('span[name="' + item + '"]').text(data.responseJSON.Errors[index][0].ErrorMessage).show();
                            }
                            //validator.showErrors({ 'SignUpModel.RepeatPassword': 'error' });
                        });
                        //validator.settings.rules;
                    }
                    $.unblockUI();
                }
            });
        } else {
            return false;
        }
    });

    $('#Province').change(function () {
        $.getJSON("/Account/GetLocalitiesByProvince", { idProvince: $('#Province').val() }, function (data) {
            $('#SignUpModel_Locality').children().remove();

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

