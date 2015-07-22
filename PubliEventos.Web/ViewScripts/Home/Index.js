﻿var viewModel = {};

$(function () {
    $('.date').datetimepicker({
        pickTime: false,
        format: "DD/MM/YYYY",
        language: 'es',
        autoclose: true,
    });

    $("#searchButton").click(function () {
        $('#search-form').slideToggle("slow", function () {
            if ($('#icon-search').hasClass('icon-caret-down')) {
                $('#icon-search').removeClass('icon-caret-down');
                $('#icon-search').addClass('icon-caret-up');
            } else if ($('#icon-search').hasClass('icon-caret-up')) {
                $('#icon-search').removeClass('icon-caret-up');
                $('#icon-search').addClass('icon-caret-down');
            }
        });
    });

    SelectUsers($(".select2-users"), false);

    viewModel = new myViewModel();

    if (events != null) {
        LoadEvents(events);
    }

    $('#SearchTerm').keypress(function () {
        if ($('#SearchTerm').val().length >= 5) {
            SearchEvents(null, $('#SearchTerm').val());
        }
    });

    $('#SearchTerm').keyup(function (e) {
        if (e.keyCode == 8 && $('#SearchTerm').val().length > 4) {
            SearchEvents(null, $('#SearchTerm').val());
        } else if (e.keyCode == 8 && $('#SearchTerm').val().length == 4) {
            SearchEvents(true, null);
        };
    });

    function SearchEvents(initialSearch, searchTerm) {
        $.ajax({
            type: 'POST',
            url: '/Home/SearchEvents',
            dataType: "json",
            data: {
                initialSearch: initialSearch,
                fullText: searchTerm
            }
        }).success(function (data) {
            viewModel.Events.removeAll();

            LoadEvents(JSON.parse(data.Events));

            $.unblockUI();
        });
    }

    $('#search').click(function () {
        if ($('#StartDate').val() != "" ||
            $('#EndDate').val() != "" ||
            $('#EventType').val() != "" ||
            $('#UserName').val()) {

            $.blockUI({ message: "" });
            $('#SearchTerm').val("");

            $.ajax({
                type: 'POST',
                url: '/Home/SearchEvents',
                dataType: "json",
                data: {
                    userId: $('#UserName').val(),
                    eventType: $('#EventType').val(),
                    startDate: $('#StartDate').val(),
                    endDate: $('#EndDate').val()
                }
            }).success(function (data) {
                viewModel.Events.removeAll();

                LoadEvents(JSON.parse(data.Events));

                $.unblockUI();
            });
        }
    });

    $('#resetFilters').click(function () {
        $.blockUI({ message: "" });

        $('#StartDate').val("");
        $('#EndDate').val("");
        $("#UserName").select2("val", "");
        $('#EventType').val("");

        SearchEvents(true, null);
    });

    ko.applyBindings(viewModel);
});

function LoadEvents(events) {
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

function myViewModel() {
    self = this;

    self.Events = ko.observableArray();
}

function EventsHeader(events) {
    var self = this;

    var month = moment(events[0].EventDate).format("MM");
    self.Date = moment(events[0].EventDate).format("DD") + " DE " + GetMonthDescription(month) + " DE " + moment(events[0].EventDate).format("YYYY");;

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
    self.EnabledActions = event.User.Id == currentUserId;
    self.AlreadyTookPlace = event.AlreadyTookPlace;

    self.EventDetail = function () {
        window.location.href = "/Event/Detail/" + event.Id;
    }

    self.UploadContents = function () {
        window.location.href = "/Event/UploadPictures/" + event.Id;
    }

    self.InviteToEvent = function () {
        window.location.href = "/Invitation/InviteToEvent/" + event.Id;
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

function GetMonthDescription(month) {
    switch (month) {
        case ("01"):
            return "ENERO"
            break;
        case ("02"):
            return "FEBRERO"
            break;
        case ("03"):
            return "MARZO"
            break;
        case ("04"):
            return "ABRIL"
            break;
        case ("05"):
            return "MAYO"
            break;
        case ("06"):
            return "JUNIO"
            break;
        case ("07"):
            return "JULIO"
            break;
        case ("08"):
            return "AGOSTO"
            break;
        case ("09"):
            return "SEPTIEMBRE"
            break;
        case ("10"):
            return "OCTUBRE"
            break;
        case ("11"):
            return "NOVIEMBRE"
            break;
        case ("12"):
            return "DICIEMBRE"
            break;
        default:
            return "";
    }
}