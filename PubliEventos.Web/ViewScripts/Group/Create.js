$(function () {
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

                for (var i = 0; i < data.Users.length; i++) {
                    data.Users[i].id = data.Users[i].Id;
                }

                return { results: data.Users, more: more };
            }
        },
        formatResult: formatResult,
        formatSelection: Selection,
        escapeMarkup: function (markup) { return markup; }
    });

    function formatResult(user) {
        return '<div class="font-text">' + user.Text + '</div>';
    };

    function Selection(user) {
        return "<div value=" + user.Id + ">" + user.Text + "</div>";
    };
});