$(function () {
    $('.deleteEvent').click(function (event) {
        var idEvent = $(this).attr('rel');

        bootbox.confirm({
            title: "Eliminación de evento",
            message: "Esta seguro que desea eliminar el evento?",
            buttons: {
                'cancel': {
                    label: "Cancelar"
                },
                'confirm': {
                    label: "Aceptar"
                }
            }, callback: function (result) {
                if (result) {
                    $.blockUI({ message: "" });

                    $.ajax({
                        url: "/Event/DeleteEvent",
                        data: { idEvent: idEvent }
                    }).done(function (data) {
                        if (data.Success) {
                            bootbox.alert("El evento ha sido eliminado correctamente", function () {
                                window.location.href = "/Event/MyEvents";
                            });
                        } else {
                            bootbox.alert("Ha ocurrido un error al eliminar el registro");
                        }

                        $.unblockUI();
                    });
                }
            }
        });
    });

    $('a .eventsSearch').click(function () {
        $.blockUI({ message: '' });
    });

    $('#filter').click(function () {
        if ($('#filterRegion').is(':visible')) {
            $('#filterRegion').hide();
        } else {
            $('#filterRegion').show();
        }
    });
});