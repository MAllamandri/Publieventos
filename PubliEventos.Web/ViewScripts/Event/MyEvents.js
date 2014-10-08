$(function () {
    var MyEventsModel = function () {
        var self = this;

        self.Events = ko.observableArray();

        self.SortByLocality = function () {
            this.Events.sort(function (a, b) {
                return a.Locality < b.Locality ? -1 : 1;
            });
        };
    }

    var viewModel = new MyEventsModel();

    $.each(data, function (index, event) {
        viewModel.Events.push(new Event(event));
    });

    ko.applyBindings(viewModel);
});

function Event(event) {
    var self = this;

    self.Title = event.Title;
    self.FileName = "<img src='/Content/images/Covers/" + event.FileName + "' style='width: 80px; height: 80px;' class='img-circle' data-src='holder.js/80x80/auto'>";
    self.EventDate = event.EventDate;
    self.EventTime = event.EventStartTime + " a " + event.EventEndTime + " Hs.";
    self.Locality = event.Locality.Name;
    self.Province = event.Locality.Province.Name;

    self.Edit = function () {
        window.location.href = "/Event/Edit/" + event.Id;
    }
}