$(function () {
    var previewTemplate = $.trim($('#dz-preview-template').hide().html());

    options = {
        url: '/Event/UploadPictures',
        maxFilesize: 200,
        clickable: true,
        previewTemplate: previewTemplate,
        thumbnailWIdth: 100,
        acceptedFiles: '.png, .jpg, .gif, .jpeg, .mp4'
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

        preview = $(file.previewElement);
        preview.find('.file-progress').fadeOut(20);
        preview.find('.result').addClass('success').show();
        preview.find('.close').on('click', function (e) {
            preview.fadeOut(100, function () {
                dropzone.removeFile(file);

                //Llamar a metodo para eliminar archivo.
            })
        });
    });

    dropzone.on('error', function (file, message) {
        preview = $(file.previewElement);
        preview.find('.result').addClass('error').show();
    });
    ////File Upload response from the server
    //Dropzone.options.dropzoneForm = {
    //    url: "/Event/UploadPictures",
    //    init: function () {
    //        this.on("complete", function (data) {
    //            console.log(data.xhr.responseText);
    //        });

    //        this.on("addedfile", function (file) {

    //            // Create the remove button
    //            var removeButton = Dropzone.createElement("<button>Quitar</button>");

    //            // Capture the Dropzone instance as closure.
    //            var _this = this;

    //            // Listen to the click event
    //            removeButton.addEventListener("click", function (e) {
    //                // Make sure the button click doesn't submit the form:
    //                e.preventDefault();
    //                e.stopPropagation();
    //                // Remove the file preview.
    //                _this.removeFile(file);
    //                // If you want to the delete the file on the server as well,
    //                // you can do the AJAX request here.
    //            });

    //            // Add the button to the file preview element.
    //            file.previewElement.appendChild(removeButton);
    //        });
    //    },
    //    addRemoveLinks: false,
    //    dictCancelUpload: "Cancelar",
    //    dictRemoveFile: "Quitar Archivo",
    //    uploadprogress: true,
    //    fallback: Dropzone.createElement("<input type='file' />"),
    //    previewTemplate: 
    //};

    //$("form.dropzone").data("dropzone")
});