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
        if (!existEmail() && !existUser()) {
            $('form').submit();
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

