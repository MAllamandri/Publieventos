var contents = {
    Image: 1,
    Movie: 2,
    Event: 3,
    Comment: 4
}

var viewModel = {};

$(function () {
    viewModel = new myViewModel();
    viewModel.ActiveContent(contents.Event);

    if (pictures != null) {
        $.each(pictures, function (index, picture) {
            viewModel.Pictures.push(new ContentModel(picture));
        });
    }

    if (events != null) {
        $.each(events, function (index, event) {
            viewModel.Events.push(new EventModel(event));
        });
    }

    if (comments != null) {
        $.each(comments, function (index, comment) {
            viewModel.Comments.push(new CommentModel(comment));
        });
    }

    if (movies != null) {
        $.each(movies, function (index, movie) {
            viewModel.Movies.push(new ContentModel(movie));
        });
    }

    ko.applyBindings(viewModel);

    $('#tabEvents').click(function () {
        viewModel.ActiveContent(contents.Event);
        removeActiveClass();
        $(this).addClass("active-link");
    });

    $('#tabComments').click(function () {
        viewModel.ActiveContent(contents.Comment);
        removeActiveClass();
        $(this).addClass("active-link");
    });

    $('#tabPictures').click(function () {
        viewModel.ActiveContent(contents.Image);
        removeActiveClass();
        $(this).addClass("active-link");
    });

    $('#tabMovies').click(function () {
        viewModel.ActiveContent(contents.Movie);
        removeActiveClass();
        $(this).addClass("active-link");
    });

    function removeActiveClass() {
        $('#tabEvents').removeClass("active-link");
        $('#tabComments').removeClass("active-link");
        $('#tabPictures').removeClass("active-link");
        $('#tabMovies').removeClass("active-link");
    }
});

function myViewModel() {
    self = this;

    self.Events = ko.observableArray();
    self.Comments = ko.observableArray();
    self.Pictures = ko.observableArray();
    self.Movies = ko.observableArray();

    self.ActiveContent = ko.observable();
    self.ContentId = ko.observable();
    self.ContentType = ko.observable();
    self.EventId = ko.observable();

    self.DisabledContent = function () {
        bootbox.confirm({
            title: "<h4 class='title-modal'>Deshabilitar Contenido</h4>",
            message: "<p class='font-text'>Esta seguro que desea deshabilitar el contenido?</p>",
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
                        type: 'POST',
                        url: "/Report/AdministrationReported",
                        data: {
                            ContentId: self.ContentId(),
                            ContentType: self.ContentType(),
                            IsDisabled: true,
                            EventId: self.EventId()
                        }
                    }).done(function (data) {
                        if (data.Success) {
                            // Quito el contenido del modelo.
                            viewModel.RemoveContent(self.ContentId(), self.ContentType(), self.EventId());
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

    self.IgnoreReportedContent = function () {
        bootbox.confirm({
            title: "<h4 class='title-modal'>Ignorar Contenido Reportado</h4>",
            message: "<p class='font-text'>Esta seguro que desea volver a activar el contenido?</p>",
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
                        type: 'POST',
                        url: "/Report/AdministrationReported",
                        data: {
                            ContentId: self.ContentId(),
                            ContentType: self.ContentType(),
                            IsDisabled: false,
                            EventId: self.EventId()
                        }
                    }).done(function (data) {
                        if (data.Success) {
                            // Quito el contenido del modelo.
                            viewModel.RemoveContent(self.ContentId(), self.ContentType(), self.EventId());
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

    self.RemoveContent = function (contentId, contentType, eventId) {
        if (contentType == contents.Image) {
            var element = ko.utils.arrayFirst(self.Pictures(), function (picture) {
                return picture.FileName == contentId;
            });

            if (element != null) {
                self.Pictures.remove(element);
            }
        }

        if (contentType == contents.Movie) {
            var element = ko.utils.arrayFirst(self.Movies(), function (movie) {
                return movie.FileName == contentId && movie.EventId == eventId;
            });

            if (element != null) {
                self.Movies.remove(element);
            }
        }

        if (contentType == contents.Event) {
            var element = ko.utils.arrayFirst(self.Events(), function (event) {
                return event.Id == contentId;
            });

            if (element != null) {
                self.Events.remove(element);
            }
        }

        if (contentType == contents.Comment) {
            var element = ko.utils.arrayFirst(self.Comments(), function (comment) {
                return comment.CommentId == contentId;
            });

            if (element != null) {
                self.Comments.remove(element);
            }
        }
    }
}

function ContentModel(content) {
    var self = this;

    self.EventId = content.EventId;
    self.FileName = content.FileName;
    self.Path = content.ContentType == contents.Image ? "/Content/images/EventsPictures/" + content.FileName : 'http://www.youtube.com/embed/' + content.FileName;
    self.ContentType = content.ContentType;

    self.DisabledMultimediaContent = function () {
        viewModel.ContentId(content.FileName);
        viewModel.ContentType(content.ContentType);
        viewModel.EventId(content.EventId);
        viewModel.DisabledContent();
    }

    self.IgnoreReportedMultimediaContent = function () {
        viewModel.ContentId(content.FileName);
        viewModel.ContentType(content.ContentType);
        viewModel.EventId(content.EventId);
        viewModel.IgnoreReportedContent();
    }
}

function EventModel(event) {
    var self = this;

    self.Id = event.Id;
    self.Title = event.Title;
    self.EventDate = moment(event.EventDate).format("DD/MM/YYYY");
    self.EventTime = event.EventStartTime.substring(0, 5) + " a " + event.EventEndTime.substring(0, 5) + " Hs";
    self.Description = event.Description;
    self.Detail = event.Detail;
    self.PicturePath = "/Content/images/Covers/" + event.FileName;
    self.UserName = event.User.UserName;
    self.ImageProfile = "/Content/images/Profiles/" + event.User.ImageProfile;

    self.UserProfile = function () {
        window.location.href = "/Account/Profile/" + event.User.Id;
    }

    self.DisabledEvent = function () {
        viewModel.ContentId(event.Id);
        viewModel.ContentType(contents.Event);
        viewModel.DisabledContent();
    }

    self.IgnoreReportedEvent = function () {
        viewModel.ContentId(event.Id);
        viewModel.ContentType(contents.Event);
        viewModel.IgnoreReportedContent();
    }
}

function CommentModel(comment) {
    var self = this;

    self.CommentId = comment.Id;
    self.Detail = comment.Detail;
    self.ImageProfile = comment.User.ImageProfile != null && comment.User.ImageProfile != "" ?
                        "/Content/images/Profiles/" + comment.User.ImageProfile :
                        "/Content/images/Profiles/contact-default-image.jpg";
    self.UserName = comment.User.UserName;
    self.CreatedBy = "Realizado por " + self.UserName;

    self.UserProfile = function () {
        window.location.href = "/Account/Profile/" + event.User.Id;
    }

    self.DisabledComment = function () {
        viewModel.ContentId(comment.Id);
        viewModel.ContentType(contents.Comment);
        viewModel.DisabledContent();
    }

    self.IgnoreReportedComment = function () {
        viewModel.ContentId(comment.Id);
        viewModel.ContentType(contents.Comment);
        viewModel.IgnoreReportedContent();
    }
}