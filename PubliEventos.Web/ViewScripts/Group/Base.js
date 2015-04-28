$(function () {
    $('#GroupName').charactersQuantity(50);
    $('#Message').charactersQuantity(350);

    SelectUsers($(".select2-users"), true);

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
                    $.blockUI({ message: "" });
                    window.location.href = "/Group/MyGroups";
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