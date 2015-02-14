$(function () {
    var previewTemplate = $.trim($('#dz-preview-template').hide().html());

    options = {
        url: '/Event/UploadPictures',
        params: { eventId: $('#EventId').val() },
        maxFilesize: 200,
        clickable: true,
        previewTemplate: previewTemplate,
        thumbnailWIdth: 100,
        acceptedFiles: '.png, .jpg, .gif, .jpeg, .mp4, .mpeg, .avi'
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

        file.Id = response.FileName;
        preview = $(file.previewElement);
        preview.find('.file-progress').fadeOut(20);

        if (response.Success) {
            preview.find('.result').addClass('success').show();
        } else {
            preview.find('.result').addClass('error').show();
        }

        preview.find('.close').on('click', function (e) {
            $.ajax({
                url: '/Event/DeletePictures',
                data: {
                    fileName: file.Id,
                    eventId: $('#EventId').val()
                }
            }).done(function (data) {
                if (data.Success) {
                    dropzone.removeFile(file);

                    if (data.QuantityContents == 0) {
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
    });
});