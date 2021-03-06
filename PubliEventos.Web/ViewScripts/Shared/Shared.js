﻿$(function () {
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
});