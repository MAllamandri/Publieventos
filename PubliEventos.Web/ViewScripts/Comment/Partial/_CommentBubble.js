$(function () {
    $(document).on("click", '#comment', function (event) {
        $('#commentModal').modal('show');
    });

    $('#commentModal').on('hidden.bs.modal', function () {
        $('#detailComment').hideMessageError();
        $('#detailComment').val("");
        $('#CommentId').val("");
    });

    $('#commentModal').on('show.bs.modal', function () {
        if ($('#CommentId').val() != "") {
            $('.title-modal').text("Editar Comentario");
        } else {
            $('.title-modal').text("Comentar");
        }
    });

    $('#saveComment').click(function () {
        $.blockUI({ message: "" });
        var url = "";

        if ($('#CommentId').val() != "") {
            url = "/Comment/Edit";
        } else {
            url = "/Comment/Create";
        }

        if ($('#detailComment').val().trim() != "") {
            $.ajax({
                type: 'POST',
                url: url,
                data: {
                    EventId: $('#EventId').val(),
                    Detail: $('#detailComment').val(),
                    CommentId: $('#CommentId').val()
                }
            }).done(function (data) {
                if (data.Success) {
                    $('#commentModal').modal('hide');
                    window.location.href = "/Event/Detail/" + $('#EventId').val()
                    $.blockUI({ message: "" });
                }
            }).error(function () {
                alert("Ha ocurrido un error");
                $.unblockUI();
            });
        } else {
            $('#detailComment').showMessageError("El Valor es obligatorio");
            $.unblockUI();
        }
    });

    $(document).on("click", '.edit-comment', function (event) {
        var comment = $(this).parent().parent().parent().children().find('p').text();
        var commentId = $(this).parent().parent().parent().children().find('input').val();

        $('#CommentId').val(commentId);
        $('#detailComment').val(comment);
        $('#commentModal').modal('show');
    });
});