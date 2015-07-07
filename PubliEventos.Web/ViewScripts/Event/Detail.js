var contents = {
    Image: 1,
    Movie: 2,
    Event: 3,
    Comment: 4
}

var viewModel = {};
var chat;

$(function () {
    $('#detailComment').charactersQuantity(200);
    $('#reasonReport').charactersQuantity(250);

    viewModel = new myViewModel();
    viewModel.ShowNotFoundComments(false);

    viewModel.Event(new EventModel(eventDetail));
    viewModel.Event().AttendEventByCurrentUser(attendEventByCurrentUser);

    if (pictures != null) {
        $.each(pictures, function (index, picture) {
            viewModel.MyPictures.push(new contentModel(picture));
        });
    }

    if (movies != null) {
        $.each(movies, function (index, movie) {
            viewModel.MyMovies.push(new contentModel(movie));
        });
    }

    if (participants != null) {
        $.each(participants, function (index, user) {
            viewModel.Participants.push(new InvitationModel(user));
        });
    }

    if (standby != null) {
        $.each(standby, function (index, user) {
            viewModel.Standby.push(new InvitationModel(user));
        });
    }

    if (viewModel.MyPictures().length > 0) {
        viewModel.MyPictures()[0].Active(self.MyPictures()[0].Active() + " active");
    }

    if (viewModel.MyMovies().length > 0) {
        viewModel.MyMovies()[0].Active(self.MyMovies()[0].Active() + " active");
    }

    $('.nano').nanoScroller({
        flash: true
    });

    $('.carousel').carousel();

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

    $('#tabDetail').click(function () {
        removeActiveClass();
        HideRegions();
        $(this).addClass("active-link");
        $('#regionDetail').show();
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

    //#region Reports

    $('#reportModal').on('show.bs.modal', function () {
        $('#reasonReport').val('');
    });

    $('#reportModal').on('hidden.bs.modal', function () {
        $('#contentId').val("");
        $('#contentType').val("");
    });

    $(document).on("click", '.reportEvent', function (event) {
        var eventId = $(this).attr('rel');
        $('#contentId').val(eventId);
        $('#contentType').val(contents.Event);

        $('#reportModal').modal('show');
    });

    //#endregion

    //#region Comments 

    $(document).on("click", '#comment', function (event) {
        $('#commentModal').modal('show');
        $('#myModalLabel').text("Comentar");
    });

    $('#commentModal').on('hidden.bs.modal', function () {
        $('#detailComment').hideMessageError();
        $('#detailComment').val("");
        $('#commentId').val("");
    });

    //#endregion

    //#region Hubs

    chat = $.connection.BaseHubs;

    chat.client.addNewCommentToPage = function (comment) {
        viewModel.Comments.push(new CommentModel(comment));

        // Ordena los comentarios.
        viewModel.OrderComments();
    };

    chat.client.DeleteComment = function (commentId) {
        viewModel.RemoveCommentToModel(commentId);

        // Ordena los comentarios.
        viewModel.OrderComments();
    };

    chat.client.EditComment = function (commentId, detail) {
        viewModel.EditCommentInModel(commentId, detail);
    };

    chat.client.DeleteContent = function (id, contentType, eventId) {
        viewModel.RemoveContent(id, contentType, eventId);
    }

    $.connection.hub.start().done(function (data) {
        var data = chat.server.getComments($('#EventId').val());
    });

    $.connection.hub.received(function (data) {
        $('#loading').hide();
        viewModel.ShowNotFoundComments(true);

        if (data.M != 'addNewCommentToPage' && data.R != "" && data.R != undefined) {
            $.each(data.R, function (index, comment) {
                viewModel.Comments.push(new CommentModel(comment));
            });
            viewModel.UpdateElapsedTime();
        }
    });

    //#endregion

    ko.applyBindings(viewModel);
});

function myViewModel() {
    self = this;

    self.Event = ko.observable();
    self.MyMovies = ko.observableArray();
    self.MyPictures = ko.observableArray();
    self.Comments = ko.observableArray();
    self.Participants = ko.observableArray();
    self.Standby = ko.observableArray();

    self.ParticipantsTitle = ko.computed(function () {
        return "Asistirán (" + self.Participants().length + ")";
    });

    self.StandbyTitle = ko.computed(function () {
        return "Esperando Confirmación (" + self.Standby().length + ")";
    });

    self.ShowNotFoundComments = ko.observable();

    self.ReportContent = function () {
        $('#reportModal').modal('hide');
        var contentType = $('#contentType').val();
        var id = $('#contentId').val();
        $.blockUI({ message: "" });

        $.ajax({
            type: 'POST',
            url: '/Report/ReportContent',
            async: false,
            data: {
                ContentId: id,
                ContentType: contentType,
                Reason: $('#reasonReport').val(),
                EventId: $('#EventId').val()
            }
        }).done(function (data) {
            if (data.Success) {
                if (data.IsDisabled) {
                    bootbox.dialog({
                        title: "<h4 class='title-modal'>Reportar</h4>",
                        message: "<p class='font-text'>El contenido ha sido deshabilitado.</p>",
                        buttons: {
                            success: {
                                label: "Aceptar",
                                className: "btn-confirm",
                                callback: function () {
                                    if (contentType == contents.Event) {
                                        window.location.href = "/";
                                        $.blockUI({ message: "" });
                                    } else if (contentType == contents.Image || contentType == contents.Movie) {
                                        chat.server.deleteContent(id, contentType, $('#EventId').val());
                                        $.unblockUI();
                                    } else if (contentType == contents.Comment) {
                                        chat.server.deleteComment(id);
                                        $.unblockUI();
                                    }
                                }
                            }
                        }
                    });
                } else {
                    var element;

                    if (contentType == contents.Image) {
                        element = ko.utils.arrayFirst(self.MyPictures(), function (picture) {
                            return picture.FileName == id;
                        });
                    } else if (contentType == contents.Movie) {
                        element = ko.utils.arrayFirst(self.MyMovies(), function (movie) {
                            return movie.FileName == id;
                        });
                    } else if (contentType == contents.Comment) {
                        element = ko.utils.arrayFirst(self.Comments(), function (comment) {
                            return comment.CommentId == id;
                        });
                    } else {
                        window.location.href = "/Event/Detail/" + $('#EventId').val();
                        $.blockUI({ message: "" });
                    }

                    if (element != null) {
                        element.IsReported(true);
                        $.unblockUI();
                    }
                }
            } else {
                $.unblockUI();
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

    self.RemoveContent = function (id, contentType, eventId) {
        var element;

        if (contentType == contents.Image) {
            var element = ko.utils.arrayFirst(self.MyPictures(), function (picture) {
                return picture.FileName == id;
            });

            if (element != null) {
                //Obtengo la posición del elemento.
                var index = self.MyPictures().indexOf(element);

                self.MyPictures.remove(element);

                if (self.MyPictures().length > 0) {
                    $.each(self.MyPictures(), function (index, item) {
                        item.Active('item');
                    });

                    index = index == self.MyPictures().length ? 0 : index;
                    self.MyPictures()[index].Active("item active");
                }
            }
        } else if (contentType == contents.Movie) {
            var element = ko.utils.arrayFirst(self.MyMovies(), function (movie) {
                return movie.FileName == id && movie.EventId == eventId;
            });

            if (element != null) {
                //Obtengo la posición del elemento.
                var index = self.MyMovies().indexOf(element);

                self.MyMovies.remove(element);

                if (self.MyMovies().length > 0) {
                    $.each(self.MyMovies(), function (index, item) {
                        item.Active('item');
                    });

                    index = index == self.MyMovies().length ? 0 : index;
                    self.MyMovies()[index].Active("item active");
                }
            }
        } else if (contentType == contents.Comment) {
            var element = ko.utils.arrayFirst(self.Comments(), function (comment) {
                return comment.CommentId == id;
            });

            self.Comments.remove(element);
        }
    }

    self.NewAndEditComment = function () {
        var url;
        var isEdition = false;

        if ($('#commentId').val() != "") {
            url = "/Comment/Edit";
            isEdition = true;
        } else {
            url = "/Comment/Create";
        }

        if ($('#detailComment').val().trim() != "") {
            $('#commentModal').modal('hide');
            $.blockUI({ message: "" });

            var now = new Date();
            var time = now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();

            $.ajax({
                type: 'POST',
                url: url,
                data: {
                    EventId: $('#EventId').val(),
                    Detail: $('#detailComment').val(),
                    CommentId: $('#commentId').val(),
                    Date: new Date().toLocaleDateString(),
                    Time: time
                }
            }).done(function (data) {
                if (data.Success) {
                    if (isEdition) {
                        // Edito el comentario en todos los usuarios.
                        chat.server.editComment(data.Comment.Id, data.Comment.Detail);
                    } else {
                        // Agrego el comentario a todos los usuarios.
                        chat.server.addNewComment(data.Comment.Id);
                        viewModel.UpdateElapsedTime();
                    }

                    $.unblockUI();
                } else {
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
    }

    self.RemoveCommentToModel = function (commentId) {
        var element = ko.utils.arrayFirst(self.Comments(), function (comment) {
            return comment.CommentId == commentId;
        });

        if (element != null) {
            self.Comments.remove(element);
        }
    }

    self.EditCommentInModel = function (commentId, detail) {
        var element = ko.utils.arrayFirst(self.Comments(), function (comment) {
            return comment.CommentId == commentId;
        });

        if (element != null) {
            element.Detail(detail);
        }
    }

    self.OrderComments = function () {
        self.Comments.sort(function (left, right) {
            return left.CommentId == right.CommentId ? 0 : (left.CommentId > right.CommentId ? -1 : 1)
        })
    }

    self.UpdateElapsedTime = function () {
        $.each(self.Comments(), function (index, comment) {
            var elapsedTime = CalculeElapsedTime(comment.EffectDate);
            comment.ElapsedTime(elapsedTime);
        });
    }
}

function contentModel(content) {
    var self = this;

    self.EventId = $('#EventId').val();
    self.FileName = content.FileName;
    self.Path = content.ContentType == contents.Image ? "/Content/images/EventsPictures/" + content.FileName : 'http://www.youtube.com/embed/' + content.FileName;
    self.IsReported = ko.observable(content.IsReportedByUser);
    self.Active = ko.observable("item");

    self.ShowReportModal = function () {
        $('#contentId').val(content.FileName);
        $('#contentType').val(content.ContentType);
        $('.title-modal').text("Reportar Contenido");

        $('#reportModal').modal('show');
    }

    self.RemoveContent = function () {
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
                    $.blockUI({ message: "<div style='font-size: 16px; padding-top: 11px;'><p>Eliminando Contenido...</p><div>" });

                    $.ajax({
                        type: 'POST',
                        url: "/Event/DeleteContent",
                        data: {
                            eventId: $('#EventId').val(),
                            fileName: content.FileName,
                            contentType: content.ContentType
                        }
                    }).done(function (data) {
                        if (data.Success) {
                            //Elimino la foto de la lista.
                            chat.server.deleteContent(content.FileName, content.ContentType, self.EventId);
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
    }
}

function CommentModel(comment) {
    var self = this;

    self.CommentId = comment.Id;
    self.Detail = ko.observable(comment.Detail);
    self.ImageProfile = comment.User.PathProfile;
    self.UserId = comment.User.Id;
    self.UserName = comment.User.UserName;
    self.FullName = comment.User.FullName != "" ? comment.User.FullName : comment.User.UserName;
    self.IsReported = ko.observable(comment.IsReportedByUser);
    self.EnabledActions = comment.User.Id == currentUserId;
    self.EffectDate = comment.EffectDate;
    self.ElapsedTime = ko.observable();

    self.UserProfile = function () {
        window.location.href = "/Account/Profile/" + comment.User.Id;
        $.blockUI({ message: "" });
    }

    self.DeleteComment = function () {
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
                        data: { commentId: comment.Id }
                    }).done(function (data) {
                        if (data.Success) {
                            // Actualizo los comentarios en todos los usuarios.
                            chat.server.deleteComment(comment.Id);

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
    }

    self.ShowCommentModal = function () {
        $('#detailComment').val(self.Detail());
        $('#commentId').val(comment.Id);
        $('#commentModal').modal('show');
        $('.title-modal').text("Editar Comentario");
    }

    self.ShowReportModal = function () {
        $('#contentId').val(comment.Id);
        $('#contentType').val(contents.Comment);
        $('.title-modal').text("Reportar Comentario");

        $('#reportModal').modal('show');
    }
}

function InvitationModel(user) {
    var self = this;

    self.UserId = user.Id;
    self.UserName = user.UserName;
    self.FullName = user.FullName != "" ? user.FullName : user.UserName;
    self.ImageProfile = user.PathProfile;

    self.Profile = function () {
        window.location.href = "/Account/Profile/" + self.UserId;
    }
}

function EventModel(event) {
    var self = this;

    var dateParts = event.EventDate.toString().substring(0, 10).split('-');

    self.Id = event.Id;
    self.Title = event.Title;
    self.Path = "/Content/images/Covers/" + event.FileName;
    self.EventDate = dateParts[2] + "/" + dateParts[1] + "/" + dateParts[0];
    self.Time = event.EventStartTime.toString().substring(0, 5) + " a " + event.EventEndTime.toString().substring(0, 5) + " Hs";
    self.Description = event.Description;
    self.Detail = event.Detail;
    self.AlreadyTookPlace = event.AlreadyTookPlace;

    self.UserImageProfile = event.User.PathProfile;
    self.UserName = event.User.UserName;
    self.FullName = event.User.FullName != "" ? event.User.FullName : event.User.UserName;

    self.AdministratorIsCurrentUser = event.User.Id == currentUserId ? true : false;
    self.UserId = event.User.Id;
    self.AttendEventByCurrentUser = ko.observable();

    self.InviteUsers = function () {
        window.location.href = "/Invitation/InviteToEvent/" + self.Id;
    }

    self.UploadPictures = function () {
        window.location.href = "/Event/UploadPictures/" + self.Id;
    }

    self.AdministratorProfile = function () {
        window.location.href = "/Account/Profile/" + self.UserId;
    }

    self.IsReportedByCurrentUser = ko.computed(function () {
        if (eventDetail.Reports == null) {
            return false;
        } else {
            var element = ko.utils.arrayFirst(eventDetail.Reports, function (report) {
                return report.User.Id == currentUserId && report.IsReported == null;
            });

            if (element != null) {
                return true;
            }
        }

        return false;
    });

    self.AttendDescription = ko.computed(function () {
        if (self.AttendEventByCurrentUser()) {
            return "<p style='text-align: center'><i class='attend-star icon-star' title='Estas asistiendo a este evento'></i>Cancelar Asistencia</p>";
        }

        return "<p style='text-align: center'><i class='attend-star icon-star-empty' title='Marca tu asistencia al evento'></i>Asistiré</p>";
    });

    self.AttendEvent = function () {
        $.ajax({
            type: 'POST',
            url: '/Invitation/AttendEvent',
            data: {
                eventId: $('#EventId').val(),
            }
        }).done(function (data) {
            self.AttendEventByCurrentUser(data.Attend);

            if (data.Attend) {
                // Lo agrego a la lista de participantes.
                viewModel.Participants.push(new InvitationModel(data.User));

                var element = ko.utils.arrayFirst(viewModel.Standby(), function (standby) {
                    return standby.UserId == data.User.Id;
                });

                // Lo elimina de la lista de esperando confirmación.
                if (element != null) {
                    viewModel.Standby.remove(element);
                }
            } else {
                var element = ko.utils.arrayFirst(viewModel.Participants(), function (participant) {
                    return participant.UserId == data.User.Id;
                });

                if (element != null) {
                    viewModel.Participants.remove(element);
                }
            }
        });
    }
}

window.setInterval(function () {
    viewModel.UpdateElapsedTime();
}, 10000)

function CalculeElapsedTime(effectDate) {
    var dateTimeParts = effectDate.split('T');
    dateParts = dateTimeParts[0].split("-");

    effectDate = dateParts[0] + "/" + dateParts[1] + "/" + dateParts[2] + " " + dateTimeParts[1];

    var duration = Math.round($.now() - Date.parse(effectDate));

    var seconds = Math.round(duration / 1000);

    if (seconds < 60) {
        var text = seconds == 1 ? " segundo" : " segundos";
        return "Hace aproximadamente " + seconds + text;
    } else {
        var minutes = Math.round(seconds / 60);
        if (minutes < 60) {
            var text = minutes == 1 ? " minuto" : " minutos";
            return "Hace aproximadamente " + minutes.toString() + text;
        } else {
            var hours = Math.round(minutes / 60);
            if (hours < 24) {
                var text = hours == 1 ? " hora" : " horas";
                return "Hace aproximadamente " + hours + text;
            } else {
                var days = Math.round(hours / 24);

                if (days <= 30) {
                    var text = days == 1 ? " día" : " días";
                    return "Hace aproximadamente " + days + text;
                } else {
                    var months = Math.round(days / 30);
                    if (months <= 12) {
                        var text = months == 1 ? " mes" : " meses";
                        return "Hace aproximadamente " + months + text;
                    } else {
                        return "Hace mas de 1 año"
                    }
                }
            }
        }
    }
}