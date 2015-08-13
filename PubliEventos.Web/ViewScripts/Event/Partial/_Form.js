$(function () {
    $(window).load(function () {
        var options =
        {
            thumbBox: '.thumbBox',
            spinner: '.spinner',
            imgSrc: ''
        }
        var cropper;

        $('#file').on('change', function () {
            var reader = new FileReader();
            reader.onload = function (e) {
                options.imgSrc = e.target.result;
                cropper = $('.imageBox').cropbox(options);
            }
            reader.readAsDataURL(this.files[0]);
            this.files = [];
        })

        $('#btnCrop').on('click', function () {
            if (cropper != null) {
                var img = cropper.getDataURL()
                $('.cropped').html('<img src="' + img + '">');
                $('#CoverPhoto').val(img);

                $('#editPhotoRegion').show('slow');
            }
        })

        $('#btnZoomIn').on('click', function () {
            if (cropper != null) {
                cropper.zoomIn();
            }
        })

        $('#btnZoomOut').on('click', function () {
            if (cropper != null) {
                cropper.zoomOut();
            }
        })
    });

    $('#EditPhoto').click(function () {
        $('#EditPhotoModal').modal('show');

        return false;
    });
});