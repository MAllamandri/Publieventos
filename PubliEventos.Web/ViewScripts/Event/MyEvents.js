﻿$(function () {
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
            title: "<h4 class='title-modal'>Eliminación de Evento</h4>",
            message: "<p class='font-text'>Esta seguro que desea eliminar el evento?</p>",
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
                        url: "/Event/DeleteEvent",
                        data: { idEvent: idEvent }
                    }).done(function (data) {
                        if (data.Success) {
                            window.location.href = "/Event/MyEvents?currentEvents=true";
                            $.blockUI({ message: "" });
                        } else {
                            bootbox.alert("Ha ocurrido un error al eliminar el registro");
                            $.unblockUI();
                        }
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