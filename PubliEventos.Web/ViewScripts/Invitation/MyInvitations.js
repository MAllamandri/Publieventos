$(function () {
    $(document).on("click", '.replyInvitation', function (event) {
        var invitationId = $(this).attr('rel');
        var description = "";
        var reply = false;

        if ($(this).hasClass("confirm")) {
            description = "confirmar";
            reply = true;
        } else {
            description = "cancelar";
        }

        bootbox.confirm({
            title: "<h4 class='title-modal'>RESPONDER INVITACIÓN</h4>",
            message: "<p class='font-text'>Esta seguro que desea " + description + " la invitación?</p>",
            buttons: {
                'cancel': {
                    label: "Cancelar",
                    className: "btn-cancel pull-left",
                },
                'confirm': {
                    label: "Aceptar",
                    className: "btn-confirm"
                }
            }, callback: function (result) {
                if (result) {
                    $.blockUI({ message: "" });

                    $.ajax({
                        url: "/Invitation/ReplyInvitation",
                        data: {
                            invitationId: invitationId,
                            reply: reply
                        }
                    }).done(function (data) {
                        if (data.Success) {
                            $.blockUI({ message: "" });
                            window.location.href = '/Invitation/MyInvitations'
                        } else {
                            bootbox.alert("Ha ocurrido un error al eliminar el registro");
                            $.unblockUI();
                        }
                    });
                }
            }
        });
    });
});