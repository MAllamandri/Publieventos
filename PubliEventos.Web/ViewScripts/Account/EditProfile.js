$(function () {
    var cropperOptions = {
        uploadUrl: "/Account/UploadOriginalImage",
        cropUrl: "/Account/CroppedImage",
        rotateControls: false,
        modal: true,
        imgEyecandyOpacity: 0.4,
        loaderHtml: '<div class="loader bubblingG"><span id="bubblingG_1"></span><span id="bubblingG_2"></span><span id="bubblingG_3"></span></div> ',
        onBeforeImgUpload: function () {
            DeleteProfileImages();
        },
        onAfterImgCrop: function () {
            $('#ImageCrop').val(response.fileName);
        },
        onAfterImgUpload: function () {
            $('#UploadImage').val(response.fileName);
        }
    }

    var cropper = new Croppic('cropContainerModal', cropperOptions);

    $(document).on('click', '.cropControlRemoveCroppedImage', function () {
        DeleteProfileImages();
    });

    $(document).on('click', '.cropControlReset', function () {
        DeleteProfileImages();

        $('.cropControlRemoveCroppedImage').click();
    });

    $('#cancel').click(function (event) {
        $(window).unbind('beforeunload');
        DeleteProfileImages();
    });

    $(window).bind("beforeunload", function () {
        DeleteProfileImages();
    })

    function DeleteProfileImages() {
        $.ajax({
            url: "/Account/DeleteProfilePicture",
            type: 'POST',
            data: {
                imageUploaded: $('#UploadImage').val(),
                imageCrop: $('#ImageCrop').val()
            }
        }).done(function () {
            $('#UploadImage').val("");
            $('#ImageCrop').val("");
        });
    };

    $('#EditPhoto').click(function () {
        if ($('#editPhotoRegion').is(':visible')) {
            $('#editPhotoRegion').hide('slow');
        } else {
            $('#editPhotoRegion').show('slow');
        }

        return false;
    });


    $('#UserName').charactersQuantity(30);
    $('#FirstName').charactersQuantity(25);
    $('#LastName').charactersQuantity(25);

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
                RepeatPassword: $('#RepeatPassword').val(),
                ValidateCurrentPassword: true
            }
        }).done(function (data) {
            if (data.Success) {
                $('#EditPassModal').modal('hide');
                $.unblockUI();

                bootbox.dialog({
                    title: "<h4 class='title-modal'>MODIFICAR CONTRASEÑA</h4>",
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
                $(window).unbind('beforeunload');
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