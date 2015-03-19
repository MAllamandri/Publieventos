var viewModel = {};

$(function () {
    viewModel = new myViewModel();

    if (multimdiaContents != null) {
        $.each(multimdiaContents, function (index, content) {
            viewModel.MultimediaContents.push(new ContentModel(content));
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

    ko.applyBindings(viewModel);

    $('#tabEvents').click(function () {
        HideRegions();
        removeActiveClass();
        $('#regionEvents').show();
        $(this).addClass("active-link");
    });

    $('#tabComments').click(function () {
        HideRegions();
        removeActiveClass();
        $('#regionComments').show();
        $(this).addClass("active-link");
    });

    $('#tabMultimediaContents').click(function () {
        HideRegions();
        removeActiveClass();
        $('#regionMultimediaContents').show();
        $(this).addClass("active-link");
    });

    function HideRegions() {
        $('#regionEvents').hide();
        $('#regionComments').hide();
        $('#regionMultimediaContents').hide();
    }

    function removeActiveClass() {
        $('#tabEvents').removeClass("active-link");
        $('#tabComments').removeClass("active-link");
        $('#tabMultimediaContents').removeClass("active-link");
    }
});

function myViewModel() {
    self = this;

    self.Events = ko.observableArray();
    self.Comments = ko.observableArray();
    self.MultimediaContents = ko.observableArray();

}

function ContentModel(content) {
    var self = this;

    self.FileName = content.FileName;
    self.Path = "/Content/images/EventsPictures/" + content.FileName;
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
}

function CommentModel(comment) {
    var self = this;

    self.CommentId = comment.Id;
    self.Detail = comment.Detail;
    self.ImageProfile = comment.User.ImageProfile != null && comment.User.ImageProfile != "" ?
                        "/Content/images/Profiles/" + comment.User.ImageProfile :
                        "/Content/themes/images/contact-default-image.jpg";
    self.UserName = comment.User.UserName;
    self.CreatedBy = "Realizado por " + self.UserName;

    self.UserProfile = function () {
        window.location.href = "/Account/Profile/" + event.User.Id;
    }
}