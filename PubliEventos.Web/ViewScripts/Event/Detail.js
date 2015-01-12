$(function () {
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
                    chat.server.addNewComment(
                        data.Comment.Detail,
                        data.Comment.Id,
                        data.Comment.User.ImageProfile,
                        data.Comment.ElapsedTime,
                        data.Comment.User.Id,
                        data.Comment.User.UserName);
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

    var chat = $.connection.CommentHub;

    chat.client.addNewCommentToPage = function (detail, commentId, imageProfile, elapsedTime, userId, userName) {
        $('#commentArea').prepend($('<div>').load("/Comment/GetComment", {
            commentId: commentId,
            detail: detail,
            imageProfile: imageProfile,
            elapsedTime: elapsedTime,
            userId: userId,
            userName: userName
        }));
    };

    $.connection.hub.start().done(function (data) {
        var data = chat.server.getComments($('#EventId').val());
    });

    $.connection.hub.received(function (data) {
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
    });
});