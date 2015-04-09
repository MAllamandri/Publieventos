$(function () {
    window.SelectUsers = function (selector, multiple) {
        $(selector).select2({
            placeholder: "Busqueda de usuarios...",
            minimumInputLength: 3,
            closeOnSelect: false,
            width: '100%',
            multiple: multiple,
            ajax: {
                url: "/Account/SearchUsersByUserName",
                dataType: "json",
                quietMillis: 250,
                data: function (term, page) {
                    return {
                        userName: term,
                        pageNumber: page,
                        pageSize: 20
                    };
                },
                results: function (data, page) {
                    // parse the results into the format expected by Select2.
                    // since we are using custom formatting functions we do not need to alter the remote JSON data
                    var more = page * 20 < data.Quantity;

                    return { results: data.Users, more: more };
                }
            },
            initSelection: function (data, callback) {
                var ids = $('#UserIds').val().split(',');
                var text = $('#UserNames').val().split(',');

                var data = [];

                for (var i = 0; i < ids.length; i++) {
                    var dataJson = { id: ids[i], text: text[i] }

                    data.push(dataJson);
                }

                callback(data);
            },
            formatResult: formatResult,
            formatSelection: Selection,
            escapeMarkup: function (markup) { return markup; }
        });
    }

    $(".select2-groups").select2({
        placeholder: "Busqueda de grupos...",
        minimumInputLength: 3,
        closeOnSelect: false,
        width: '100%',
        multiple: true,
        ajax: {
            url: "/Group/SearchGroupsByName",
            dataType: "json",
            quietMillis: 250,
            data: function (term, page) {
                return {
                    groupName: term,
                    pageNumber: page,
                    pageSize: 20
                };
            },
            results: function (data, page) {
                // parse the results into the format expected by Select2.
                // since we are using custom formatting functions we do not need to alter the remote JSON data
                var more = page * 20 < data.Quantity;

                return { results: data.Groups, more: more };
            }
        },
        initSelection: function (data, callback) {

        },
        formatResult: formatResult,
        formatSelection: Selection,
        escapeMarkup: function (markup) { return markup; }
    });

    function formatResult(element) {
        return '<div class="font-text">' + element.text + '</div>';
    };

    function Selection(element) {
        return "<div style='padding-top: 2px' class='font-text' value=" + element.id + ">" + element.text + "</div>";
    };
});