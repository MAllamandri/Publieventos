$(function () {
    $('.nano').nanoScroller({
        flash: true
    });

    $('#Send').click(function () {
        $('#SendForm').ajaxForm({
            url: '/Invitation/InviteToEvent',
            type: 'POST',
            datatype: 'text/json',
            beforeSubmit: function (arr, $form, options) {
                $('[name="RequiredUsers"]').hideMessageError();
                $.blockUI({ message: "<div style='font-size: 16px; padding-top: 11px;'><p>Enviando Invitaciones...</p><div>" });
            },
            complete: function (data) {
                if (data.responseJSON.Success) {
                    $('#usersIds').select2('val', '');
                    $('#groupsIds').select2('val', '');

                    bootbox.dialog({
                        title: "<h4 class='title-modal'>Invitaciones</h4>",
                        message: "<p class='font-text'>Las invitaciones han sido enviadas con exito</p>",
                        buttons: {
                            success: {
                                label: "Aceptar",
                                className: "btn-confirm",
                                callback: function () { window.location.href = "/Invitation/InviteToEvent/" + $('#eventId').val() }
                            }
                        }
                    });


                } else {
                    $('[name="RequiredUsers"]').showMessageError(data.responseJSON.Errors);
                }

                $.unblockUI();
            }
        });
    });
});