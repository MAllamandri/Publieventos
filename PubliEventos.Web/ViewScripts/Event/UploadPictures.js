$(function () {
    var previewTemplate = $.trim($('#dz-preview-template').hide().html());

    options = {
        url: '/Event/UploadPictures',
        params: { eventId: $('#EventId').val() },
        maxFilesize: 200,
        clickable: true,
        previewTemplate: previewTemplate,
        thumbnailWIdth: 100,
        acceptedFiles: '.png, .jpg, .gif, .jpeg, .mp4, .webm'
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
                    eventId: $('#EventId').val()
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
});