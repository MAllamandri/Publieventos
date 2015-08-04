$(function () {
    $('html, body').tooltip({
        selector: '[data-toggle="tooltip"]',
        html: true,
        title: '<p>' + $(this).attr('title') + '</p>'
    });

    $.fn.charactersQuantity = function (count) {
        $(this).blur(function () {
            if ($(this).val().length > count) {
                $(this).val($(this).val().substr(0, count + 1));
            }
        });

        $(this).keypress(function () {
            if ($(this).val().length > count) {
                $(this).val($(this).val().substring(0, $(this).val().length - 1));
            }
        });
    }

    //<sumary>Setea estilos y mensaje de error a un elemento del DOM.</sumary>
    $.fn.showMessageError = function (message) {
        var elementSelector = "[name='" + $(this).attr('name') + "']";
        var fieldSelector = "[data-valmsg-for='" + $(this).attr('name') + "']";

        // Si son el mismo elemento, no seteo la clase de error asociada al input.
        if ($(elementSelector)[0] != $(fieldSelector)[0]) {
            $(elementSelector).addClass('input-validation-error');
        }

        $(fieldSelector).removeClass('field-validation-valid');
        $(fieldSelector).addClass('field-validation-error');
        $(fieldSelector).text(message);

        return this;
    };

    //<sumary>Elimina los estilos y el mensaje de error a un elemento del DOM.</sumary>
    $.fn.hideMessageError = function () {

        $.each(this, function () {
            var elementSelector = "[name='" + $(this).attr('name') + "']";
            var fieldSelector = "[data-valmsg-for='" + $(this).attr('name') + "']";
            $(elementSelector).removeClass('input-validation-error');
            $(fieldSelector).removeClass('field-validation-error');
            $(fieldSelector).addClass('field-validation-valid');
            $(fieldSelector).text('');
        });

        return this;
    };

    $(document).on('change', '.upload', function () {
        if (this.files[0] != null && this.files[0].name != "") {
            $('.title-file').text(this.files[0].name);
        }
    });

    window.CalculeElapsedTime = function (effectDate) {
        var dateTimeParts = effectDate.split('T');
        dateParts = dateTimeParts[0].split("-");

        effectDate = dateParts[0] + "/" + dateParts[1] + "/" + dateParts[2] + " " + dateTimeParts[1];

        var duration = Math.round($.now() - Date.parse(effectDate));

        var seconds = Math.round(duration / 1000);

        if (seconds < 60) {
            var text = seconds == 1 ? " segundo" : " segundos";
            return "Hace aproximadamente " + seconds + text;
        } else {
            var minutes = Math.round(seconds / 60);
            if (minutes < 60) {
                var text = minutes == 1 ? " minuto" : " minutos";
                return "Hace aproximadamente " + minutes.toString() + text;
            } else {
                var hours = Math.round(minutes / 60);
                if (hours < 24) {
                    var text = hours == 1 ? " hora" : " horas";
                    return "Hace aproximadamente " + hours + text;
                } else {
                    var days = Math.round(hours / 24);

                    if (days <= 30) {
                        var text = days == 1 ? " día" : " días";
                        return "Hace aproximadamente " + days + text;
                    } else {
                        var months = Math.round(days / 30);
                        if (months <= 12) {
                            var text = months == 1 ? " mes" : " meses";
                            return "Hace aproximadamente " + months + text;
                        } else {
                            return "Hace mas de 1 año"
                        }
                    }
                }
            }
        }
    }
});