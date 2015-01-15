$(function () {
    $('#GroupName').charactersQuantity(50);
    $('#Message').charactersQuantity(350);

    $(".select2-remote").select2({
        placeholder: "Busqueda de usuarios...",
        minimumInputLength: 3,
        closeOnSelect: false,
        width: '100%',
        multiple: true,
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

    function formatResult(user) {
        return '<div class="font-text">' + user.text + '</div>';
    };

    function Selection(user) {
        return "<div class='font-text' value=" + user.id + ">" + user.text + "</div>";
    };


    $('#Save').click(function () {
        $('input[type="text"]').hideMessageError();

        var url = "";

        if ($('#GroupId').val() != null && $('#GroupId').val() != "") {
            url = '/Group/Edit'
        } else {
            url = '/Group/Create'
        }

        $('#Form').ajaxForm({
            url: url,
            datatype: 'text/json',
            beforeSubmit: function (arr, $form, options) {
                $.blockUI();
            },
            complete: function (data) {
                if (data.responseJSON.Success) {
                    window.location.href = "/Group/MyGroups";
                    $.blockUI({ message: "" });
                } else {
                    $.each(data.responseJSON.Errors, function (index, value) {
                        var selector = "[name='" + index + "']";
                        $(selector).showMessageError(value);
                    });
                    $.unblockUI();
                }
            }
        });
    });
});