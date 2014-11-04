$(function () {
    if ($('#CurrentsEvents').val() == "True") {
        $('#tabCurrents').addClass("active-link");
        $('#tabPrevious').removeClass("active-link");
    } else {
        $('#tabPrevious').addClass("active-link");
        $('#tabCurrents').removeClass("active-link");
    }

    $(document).on("click", '.deleteEvent', function (event) {
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
                                window.location.href = "/Event/MyEvents?currentEvents=true";
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

    $('#filter').click(function () {
        if ($('#filterRegion').is(':visible')) {
            $('#filterRegion').hide();
        } else {
            $('#filterRegion').show();
        }
    });
});