var contentTypes = {
    Picture: 1,
    Movie: 2
}

$(function () {
    viewModel = new myViewModel();

    var previewTemplate = $.trim($('#dz-preview-template').hide().html());

    options = {
        url: '/Event/UploadPictures',
        params: { eventId: $('#EventId').val() },
        maxFilesize: 200,
        clickable: true,
        previewTemplate: previewTemplate,
        thumbnailWIdth: 100,
        acceptedFiles: '.png, .jpg, .gif, .jpeg'
    }

    var dropzone = new Dropzone('.dropfiles', options);

    dropzone.on('drop', function (data, response) {
        $('.dropfiles').removeClass('enter');
        $('.dz-message').hide();
    });

    dropzone.on('dragenter', function (e) {
        $('.dropfiles').addClass('enter');
    });

    dropzone.on('dragleave', function (e) {
        $('.dropfiles').removeClass('enter');
    });

    dropzone.on('sending', function (file, xhr, formData) {
        preview = $(file.previewElement);
        preview.find('.file-progress').fadeIn(20);
    });

    dropzone.on('success', function (file, response) {
        $('.dz-message').hide();
        $('#detailRegion').show();

        file.Id = response.FileName;
        preview = $(file.previewElement);
        preview.find('.file-progress').fadeOut(20);

        if (response.Success) {
            file.uploadSuccess = true;
            preview.find('.result').addClass('success').show();
            $('#processCorrect').text(parseInt($('#processCorrect').text()) + 1);
        } else {
            file.uploadSuccess = false;
            preview.find('.result').addClass('error').show();
            $('#processErrors').text(parseInt($('#processErrors').text()) + 1);
        }

        preview.find('.close').on('click', function (e) {
            $.ajax({
                url: '/Event/DeleteContent',
                data: {
                    fileName: file.Id,
                    eventId: $('#EventId').val(),
                    contentType: contentTypes.Picture
                }
            }).done(function (data) {
                if (data.Success) {
                    dropzone.removeFile(file);

                    if (file.uploadSuccess) {
                        $('#processCorrect').text(parseInt($('#processCorrect').text()) - 1);
                    } else {
                        $('#processErrors').text(parseInt($('#processErrors').text()) - 1);
                    }

                    if ((parseInt($('#processCorrect').text()) + parseInt($('#processErrors').text())) == 0) {
                        $('.dz-message').show();
                    }
                } else {
                    alert("Ha ocurrido un error al eliminar el archivo");
                }
            });
        });
    });

    dropzone.on('error', function (file, message) {
        preview = $(file.previewElement);
        preview.find('.result').addClass('error').show();
        $('.dz-message').hide();
        $('#detailRegion').show();
        $('#processErrors').text(parseInt($('#processErrors').text()) + 1);
    });

    $('#addMovie').click(function () {
        $('#movieModal').modal('show');
    });

    ko.applyBindings(viewModel);
});

function myViewModel() {
    self = this;

    self.Movies = ko.observableArray();
    self.CurrentMovie = ko.observable();

    self.viewMovie = ko.computed(function () {
        if (self.CurrentMovie() != null && self.CurrentMovie().length > 0 && Valid(self.CurrentMovie())) {
            var regex = /(\?v=|\&v=|\/\d\/|\/embed\/|\/v\/|\.be\/)([a-zA-Z0-9\-\_]+)/;
            var youtubeurl = self.CurrentMovie();
            var regexyoutubeurl = youtubeurl.match(regex);
            if (regexyoutubeurl) {
                var url = 'http://www.youtube.com/embed/' + regexyoutubeurl[2];
                self.CurrentMovie(url);

                return true;
            }
        }

        return false;
    });

    self.AddMovie = function () {
        var guid = Valid(self.CurrentMovie())
        $('[name="currentMovie"]').hideMessageError();

        if (guid) {
            $.blockUI({ message: "<div style='font-size: 16px; padding-top: 11px;'><p>Subiendo...</p><div>" });

            if (!ExistsContent(guid, contentTypes.Movie)) {
                $('#movieModal').modal('hide');

                $.ajax({
                    type: 'POST',
                    url: '/Event/UploadMovies',
                    data: {
                        fileName: guid,
                        eventId: $('#EventId').val()
                    }
                }).done(function (data) {
                    if (data.Success) {
                        self.Movies.push(new Movies(guid));

                        self.CurrentMovie('');
                    } else {
                        alert("Ha ocurrido un error al subir el video");
                    }

                    $.unblockUI();
                });
            } else {
                $('[name="currentMovie"]').showMessageError("Ya ha subido este video a su evento");
                $.unblockUI();
            }
        } else {
            $('[name="currentMovie"]').showMessageError("Dirección incorrecta");
        }
    }
}

function Movies(movie) {
    var self = this;

    self.FileName = "http://img.youtube.com/vi/" + movie + "/1.jpg";

    self.Remove = function () {
        $.ajax({
            url: '/Event/DeleteContent',
            data: {
                fileName: movie,
                eventId: $('#EventId').val(),
                contentType: contentTypes.Movie
            }
        }).done(function (data) {
            if (data.Success) {
                viewModel.Movies.remove(self);
            } else {
                alert("Ha ocurrido un error al eliminar el video");
            }
        }).error(function () {
            alert("Ha ocurrido un error al eliminar el video");
        });
    }
}

function Valid(url) {
    if (url != null && url.length > 0) {
        var p = /^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$/;
        return (url.match(p)) ? RegExp.$1 : false;
    }

    return false;
}

function ExistsContent(guid, type) {
    var exists = false;

    $.ajax({
        url: '/Event/ValidateExistsContent',
        async: false,
        data: {
            eventId: $('#EventId').val(),
            fileName: guid,
            contentType: type
        }
    }).done(function (data) {
        if (data.Exists) {
            exists = true;
        }
    });

    return exists;
}