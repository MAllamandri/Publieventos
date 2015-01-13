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

    $('#tabDetail').click(function () {
        removeActiveClass();
        $('#regionLocalization').hide();
        $('#regionContents').hide();
        $(this).addClass("active-link");
        $('#regionDetail').show();
    });

    $('#tabLocalization').click(function () {
        removeActiveClass();
        $('#regionDetail').hide();
        $('#regionContents').hide();
        $('#regionLocalization').show();
        $(this).addClass("active-link");

        var center = map.getCenter();
        google.maps.event.trigger(map, "resize");
        map.setCenter(center);
    });

    $('#tabContents').click(function () {
        removeActiveClass();
        $('#regionDetail').hide();
        $('#regionLocalization').hide();
        $('#regionContents').show();
        $(this).addClass("active-link");
    });

    function removeActiveClass() {
        $('#tabContents').removeClass("active-link");
        $('#tabLocalization').removeClass("active-link");
        $('#tabDetail').removeClass("active-link");
    }

    $('#saveComment').click(function () {
        $.blockUI({ message: "" });
        var url = "";
        var isEdition = false;

        if ($('#CommentId').val() != "") {
            url = "/Comment/Edit";
            isEdition = true;
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

                    // Si no es una edición agrego el nuevo comentario.
                    if (!isEdition) {
                        chat.server.addNewComment(
                            data.Comment.Detail,
                            data.Comment.Id,
                            data.Comment.User.ImageProfile,
                            data.Comment.ElapsedTime,
                            data.Comment.User.Id,
                            data.Comment.User.UserName);
                    } else {
                        // Si no actualizo toda la lista.
                        chat.server.refreshComments();
                    }

                    $.unblockUI();
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

    $(document).on("click", '.deleteComment', function (event) {
        var commentId = $(this).attr('rel');

        bootbox.confirm({
            title: "<h4 class='title-modal'>Eliminación de Comentario</h4>",
            message: "<p class='font-text'>Esta seguro que desea eliminar el comentario?</p>",
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
                        url: "/Comment/Delete",
                        data: { commentId: commentId }
                    }).done(function (data) {
                        if (data.Success) {
                            // Si no actualizo toda la lista.
                            chat.server.refreshComments();
                            $.unblockUI();
                        } else {
                            bootbox.alert("Ha ocurrido un error al eliminar el registro");
                            $.unblockUI();
                        }
                    });
                }
            }
        });
    });

    var chat = $.connection.CommentHub;

    chat.client.addNewCommentToPage = function (detail, commentId, imageProfile, elapsedTime, userId, userName) {
        $('#commentArea').children().first().before($('<div>').load("/Comment/GetComment", {
            commentId: commentId,
            detail: detail,
            imageProfile: imageProfile,
            elapsedTime: elapsedTime,
            userId: userId,
            userName: userName
        }));
    };

    chat.client.refreshCommentsInPage = function () {
        chat.server.getComments($('#EventId').val());
    };

    $.connection.hub.start().done(function (data) {
        var data = chat.server.getComments($('#EventId').val());
    });

    $.connection.hub.received(function (data) {
        if (data.M != 'addNewCommentToPage' && data.R != "" && data.R != undefined) {
            $('#commentArea').html('');
            $.each(data.R, function (index, comment) {
                $('#commentArea').append($('<div>').load("/Comment/GetComment", {
                    commentId: comment.Id,
                    detail: comment.Detail,
                    imageProfile: comment.User.ImageProfile,
                    elapsedTime: comment.ElapsedTime,
                    userId: comment.User.Id,
                    userName: comment.User.UserName
                }));
            });
        }
    });
});