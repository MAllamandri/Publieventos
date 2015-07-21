var viewModel = {};

$(function () {
    viewModel = new myViewModel();
    viewModel.Event(new EventModel(eventDetail));

    if (participants != null) {
        $.each(participants, function (index, user) {
            viewModel.Participants.push(new InvitationModel(user));
        });
    }

    if (standby != null) {
        $.each(standby, function (index, user) {
            viewModel.Standby.push(new InvitationModel(user));
        });
    }

    $('.nano').nanoScroller({
        flash: true
    });

    SelectUsers($(".select2-users"), true);

    $('#Send').click(function () {
        $('#SendForm').ajaxForm({
            url: '/Invitation/InviteToEvent',
            type: 'POST',
            datatype: 'text/json',
            beforeSubmit: function (arr, $form, options) {
                $('[name="RequiredUsers"]').hideMessageError();
                $.blockUI({ message: "<div style='font-size: 16px; padding-top: 11px;'><p>Enviando Invitaciones...</p><div>" });
            },
            complete: function (data) {
                if (data.responseJSON.Success) {
                    $('#usersIds').select2('val', '');
                    $('#groupsIds').select2('val', '');

                    bootbox.dialog({
                        title: "<h4 class='title-modal'>INVITACIONES</h4>",
                        message: "<p class='font-text'>Las invitaciones han sido enviadas con exito</p>",
                        buttons: {
                            success: {
                                label: "Aceptar",
                                className: "btn-confirm",
                                callback: function () {
                                    window.location.href = "/Invitation/InviteToEvent/" + $('#eventId').val()
                                    $.blockUI({ message: "" });
                                }
                            }
                        }
                    });


                } else {
                    $('[name="RequiredUsers"]').showMessageError(data.responseJSON.Errors);
                }

                $.unblockUI();
            }
        });
    });

    ko.applyBindings(viewModel);
});

function myViewModel() {
    self = this;

    self.Event = ko.observable();
    self.Participants = ko.observableArray();
    self.Standby = ko.observableArray();

    self.ParticipantsTitle = ko.computed(function () {
        return "ASISTIRÁN (" + self.Participants().length + ")";
    });

    self.StandbyTitle = ko.computed(function () {
        return "ESPERANDO CONFIRMACIÓN (" + self.Standby().length + ")";
    });
}

function InvitationModel(user) {
    var self = this;

    self.UserId = user.Id;
    self.UserName = user.UserName;
    self.FullName = user.FullName != "" ? user.FullName : user.UserName;
    self.ImageProfile = user.PathProfile;

    self.Profile = function () {
        window.location.href = "/Account/Profile/" + self.UserId;
    }
}

function EventModel(event) {
    var self = this;

    var dateParts = event.EventDate.toString().substring(0, 10).split('-');

    self.Id = event.Id;
    self.Title = event.Title;
    self.Path = "/Content/images/Covers/" + event.FileName;
    self.EventDate = dateParts[2] + "/" + dateParts[1] + "/" + dateParts[0];
    self.Time = event.EventStartTime.toString().substring(0, 5) + " a " + event.EventEndTime.toString().substring(0, 5) + " Hs";
    self.Description = event.Description;
    self.Detail = event.Detail;

    self.UserImageProfile = event.User.PathProfile;
    self.UserName = event.User.UserName;
    self.FullName = event.User.FullName != "" ? event.User.FullName : event.User.UserName;
    self.AdministratorIsCurrentUser = event.User.Id == currentUserId ? true : false;
    self.UserId = event.User.Id;
}