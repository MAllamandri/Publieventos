$(function () {
    $(document).on("click", '#comment', function (event) {
        $('#commentModal').modal('show');
    });

    $('#detailComment').charactersQuantity(200);

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

    $(document).on("click", '.edit-comment', function (event) {
        var comment = $(this).parent().parent().parent().children().find('p').text();
        var commentId = $(this).parent().parent().parent().children().find('input').val();

        $('#CommentId').val(commentId);
        $('#detailComment').val(comment);
        $('#commentModal').modal('show');
    });
});
