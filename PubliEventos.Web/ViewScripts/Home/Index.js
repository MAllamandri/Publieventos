var viewModel = {};

$(function () {
    viewModel = new myViewModel();

    if (events != null) {
        var eventsArray = [];
        $.each(events, function (index, event) {
            if (eventsArray.length == 0 || moment(eventsArray[eventsArray.length - 1].EventDate).format("DD/MM/YYYY") == moment(event.EventDate).format("DD/MM/YYYY")) {
                eventsArray.push(event);
            } else {
                viewModel.Events.push(new EventsHeader(eventsArray));
                eventsArray = [];
                eventsArray.push(event);
            }
        });

        if (eventsArray.length > 0) {
            viewModel.Events.push(new EventsHeader(eventsArray));
        }
    }

    ko.applyBindings(viewModel);
});

function myViewModel() {
    self = this;

    self.Events = ko.observableArray();
}

function EventsHeader(events) {
    var self = this;

    self.Date = moment(events[0].EventDate).format("DD/MM/YYYY");
    self.EventsDetail = ko.observableArray();

    $.each(events, function (index, event) {
        self.EventsDetail.push(new EventModel(event));

        if (index != 0 && self.EventsDetail()[index - 1].Left() == true) {
            self.EventsDetail()[index].Left(false);
        } else {
            self.EventsDetail()[index].Left(true);
        }
    });
}

function EventModel(event) {
    var self = this;

    self.Id = event.Id;
    self.Title = event.Title;
    self.EventDate = " " + moment(event.EventDate).format("DD/MM/YYYY");
    self.EventTime = " " + event.EventStartTime.substring(0, 5) + " a " + event.EventEndTime.substring(0, 5) + " Hs";
    self.Description = event.Description;
    self.PicturePath = "/Content/images/Covers/" + event.FileName;
    self.Left = ko.observable();

    self.EventDetail = function () {
        window.location.href = "/Event/Detail/" + event.Id;
    }

    self.fbShare = function () {
        var urlEvent = url.replace('#', event.Id.toString());
        var image = urlImage.replace('#', event.FileName);
        var description = event.Description == null ? "" : event.Description;

        window.fbShare(urlEvent, event.Title, description, image);
    }
}

ko.bindingHandlers.TwitterButton = {
    init: function (element, event, allBindingsAccessor) {
        var title = allBindingsAccessor().TwitterButton.title;
        var id = allBindingsAccessor().TwitterButton.id.toString();

        var urlEvent = url.replace('#', id);
        var urlTwitter = "https://twitter.com/share?text=" + title + " > &url=" + urlEvent;

        element.href = urlTwitter;
    }
};