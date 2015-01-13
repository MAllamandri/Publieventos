$(function () {
    $(document).on("click", '.deleteGroup', function (event) {
        var groupId = $(this).attr('rel');

        bootbox.confirm({
            title: "<h4 class='title-modal'>Eliminación de Grupo</h4>",
            message: "<p class='font-text'>Esta seguro que desea eliminar el grupo?</p>",
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
                        url: "/Group/Delete",
                        data: { groupId: groupId }
                    }).done(function (data) {
                        if (data.Success) {
                            window.location.href = '/Group/MyGroups'
                            $.unblockUI();
                        } else {
                            bootbox.alert("Ha ocurrido un error al eliminar el registro");
                            $.unblockUI();
                        }
                    });
                }
            }
        });
    });

    $(document).on("click", '.leaveGroup', function (event) {
        var groupId = $(this).attr('rel');

        bootbox.confirm({
            title: "<h4 class='title-modal'>Abandonar Grupo</h4>",
            message: "<p class='font-text'>Esta seguro que desea abandonar el grupo?</p>",
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
                        url: "/Group/LeaveGroup",
                        data: { groupId: groupId }
                    }).done(function (data) {
                        if (data.Success) {
                            window.location.href = '/Group/MyGroups'
                            $.unblockUI();
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