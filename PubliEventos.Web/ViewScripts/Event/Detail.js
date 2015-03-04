var contents = {
    Image: 1,
    Movie: 2,
    Event: 3,
    Comment: 4
}

$(function () {
    $('.nano').nanoScroller({
        flash: true
    });

    $('.carousel').carousel();

    $(document).on("click", '#comment', function (event) {
        $('#commentModal').modal('show');
    });

    $('#detailComment').charactersQuantity(200);
    $('#reasonReport').charactersQuantity(250);

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

    $(document).on("click", '.deleteContent', function (event) {
        var fileName = $(this).attr('rel');
        var element = $(this).parent().parent().parent().parent();
        var nextElement = element.next().length != 0 ? element.next() : element.prev();

        bootbox.confirm({
            title: "<h4 class='title-modal'>Eliminación de Contenido</h4>",
            message: "<p class='font-text'>Esta seguro que desea eliminar el contenido?</p>",
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
                    $.blockUI({ message: "Eliminando Contenido..." });

                    $.ajax({
                        type: 'POST',
                        url: "/Event/DeleteContent",
                        data: {
                            eventId: $('#EventId').val(),
                            fileName: fileName
                        }
                    }).done(function (data) {
                        if (data.Success) {
                            // Elimino el item actual.
                            element.remove();

                            // Si el siguiente elemento es un item, lo activo. Sino muestro el mensaje que no hay mas contenidos.
                            if (nextElement.length != 0 && nextElement.hasClass('item')) {
                                nextElement.addClass('active');
                            } else {
                                if (data.IsPicture) {
                                    $('#regionNotFound').show();
                                    $('#carousel-pictures').hide();
                                } else {
                                    $('#regionNotFoundMovies').show();
                                    $('#carousel-movies').hide();
                                }
                            }
                        } else {
                            bootbox.dialog({
                                title: "<h4 class='title-modal'>Contenidos</h4>",
                                message: "<p class='font-text'>Ha ocurrido un error al eliminar el contenido.</p>",
                                buttons: {
                                    success: {
                                        label: "Aceptar",
                                        className: "btn-confirm",
                                        callback: function () { }
                                    }
                                }
                            });
                        }
                        $.unblockUI();
                    }).error(function () {
                        alert("Ha ocurrido un error");
                        $.unblockUI();
                    });
                }
            }
        });
    });

    $('#tabDetail').click(function () {
        removeActiveClass();
        HideRegions();
        $(this).addClass("active-link");
        $('#regionDetail').show();
    });

    $('#tabLocalization').click(function () {
        removeActiveClass();
        HideRegions();
        $('#regionLocalization').show();
        $(this).addClass("active-link");

        var center = map.getCenter();
        google.maps.event.trigger(map, "resize");
        map.setCenter(center);
    });

    $('#tabPictures').click(function () {
        HideRegions();
        removeActiveClass();
        $('#regionPictures').show();
        $(this).addClass("active-link");
    });

    $('#tabMovies').click(function () {
        HideRegions();
        removeActiveClass();
        $('#regionMovies').show();
        $(this).addClass("active-link");
    });

    function removeActiveClass() {
        $('#tabPictures').removeClass("active-link");
        $('#tabLocalization').removeClass("active-link");
        $('#tabDetail').removeClass("active-link");
        $('#tabMovies').removeClass("active-link");
    }

    function HideRegions() {
        $('#regionLocalization').hide();
        $('#regionPictures').hide();
        $('#regionDetail').hide();
        $('#regionMovies').hide();
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
                    $('.not-found-message').hide();

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

    //#region Reports

    $(document).on("click", '#saveReport', function (event) {
        $('#reportModal').modal('hide');
        ReportContent();
    });

    $(document).on("click", '.reportEvent', function (event) {
        var eventId = $(this).attr('rel');
        $('#contentId').val(eventId);
        $('#contentType').val(contents.Event);

        $('#reportModal').modal('show');
    });

    $(document).on("click", '.reportComment', function (event) {
        var commentId = $(this).attr('rel');
        $('#contentId').val(commentId);
        $('#contentType').val(contents.Comment);

        $('#reportModal').modal('show');
    });

    $(document).on("click", '.reportMovie', function (event) {
        var movieId = $(this).attr('rel');
        $('#contentId').val(movieId);
        $('#contentType').val(contents.Movie);

        $('#reportModal').modal('show');
    });

    $(document).on("click", '.reportPicture', function (event) {
        var pictureId = $(this).attr('rel');
        $('#contentId').val(pictureId);
        $('#contentType').val(contents.Image);

        $('#reportModal').modal('show');
    });

    function ReportContent() {
        $.blockUI({ message: "" });

        $.ajax({
            type: 'POST',
            url: '/Report/ReportContent',
            data: {
                ContentId: $('#contentId').val(),
                ContentType: $('#contentType').val(),
                Reason: $('#reasonReport').val()
            }
        }).done(function (data) {
            if (data.Success) {
                if ($('#contentType').val() == contents.Comment) {
                    chat.server.refreshComments();
                    $.unblockUI();
                } else {
                    window.location.href = "/Event/Detail/" + $('#EventId').val();
                    $.blockUI({ message: "" });
                }
            } else {
                bootbox.dialog({
                    title: "<h4 class='title-modal'>Reportar</h4>",
                    message: "<p class='font-text'>Ha ocurrido un error al reportar el contenido.</p>",
                    buttons: {
                        success: {
                            label: "Aceptar",
                            className: "btn-confirm",
                            callback: function () { }
                        }
                    }
                });
            }
        });
    }

    //#endregion

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
                            bootbox.dialog({
                                title: "<h4 class='title-modal'>Comentarios</h4>",
                                message: "<p class='font-text'>Ha ocurrido un error al eliminar el comentario..</p>",
                                buttons: {
                                    success: {
                                        label: "Aceptar",
                                        className: "btn-confirm",
                                        callback: function () { }
                                    }
                                }
                            });
                            $.unblockUI();
                        }
                    }).error(function () {
                        alert("Ha ocurrido un error");
                        $.unblockUI();
                    });;
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
        $('#commentArea').html('<p style="font-size: 20px; margin-top: 30px;" class="not-found-message">No Hay Comentarios Disponibles</p>');
        chat.server.getComments($('#EventId').val());
    };

    $.connection.hub.start().done(function (data) {
        var data = chat.server.getComments($('#EventId').val());
    });

    $.connection.hub.received(function (data) {
        if (data.M != 'addNewCommentToPage' && data.R != "" && data.R != undefined) {
            $('#commentArea').html('');
            $.each(data.R, function (index, comment) {
                var userReportsIds = [];

                if (comment.UserReportsIds != null) {
                    $.each(comment.UserReportsIds, function (index, id) {
                        userReportsIds.push(id)
                    });
                }

                $('#commentArea').append($('<div>').load("/Comment/GetComment", {
                    commentId: comment.Id,
                    detail: comment.Detail,
                    imageProfile: comment.User.ImageProfile,
                    elapsedTime: comment.ElapsedTime,
                    userId: comment.User.Id,
                    userName: comment.User.UserName,
                    userReportIds: userReportsIds.join(',')
                }));
            });
        }
    });
});