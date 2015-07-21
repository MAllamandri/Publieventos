$(function () {
    if ($('#CurrentsEvents').val() == "True") {
        $('#tabCurrents').addClass("active-tab");
        $('#tabPrevious').removeClass("active-tab");
    } else {
        $('#tabPrevious').addClass("active-tab");
        $('#tabCurrents').removeClass("active-tab");
    }

    $("#searchButton").click(function () {
        $('#search-form').slideToggle("slow", function () {
            if ($('#icon-search').hasClass('icon-caret-down')) {
                $('#icon-search').removeClass('icon-caret-down');
                $('#icon-search').addClass('icon-caret-up');
            } else if ($('#icon-search').hasClass('icon-caret-up')) {
                $('#icon-search').removeClass('icon-caret-up');
                $('#icon-search').addClass('icon-caret-down');
            }
        });
    });

    $(document).on("click", '.deleteEvent', function (event) {
        var idEvent = $(this).attr('rel');

        bootbox.confirm({
            title: "<h4 class='title-modal'>ELIMINACIÓN DE EVENTO</h4>",
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

    $('#reset').click(function () {
        $('#myEvents').show();
        $('#regionEvents').hide();
    });

    $('#search').click(function () {
        $.blockUI({ message: "<div style='font-size: 16px; padding-top: 11px;'><p>Buscando...</p><div>" });
        $.ajax({
            type: "POST",
            url: "/Event/GetFilteredEvents",
            data: {
                EventTypeId: $('#EventTypeId').val(),
                StartDate: $('#StartDate').val(),
                EndDate: $('#EndDate').val(),
                myEvents: true,
                SearchPublics: $('input[name="Private"]:checked').val() == 'true' ? true : false,
                SearchPrivate: $('input[name="Private"]:checked').val() == 'false' ? true : false,
            },
            dataType: "html"
        }).success(function (data) {
            $('#regionEvents').show();
            $('#regionEvents').html(data);
            $('#myEvents').hide();
            $.unblockUI();
        });
    });
});