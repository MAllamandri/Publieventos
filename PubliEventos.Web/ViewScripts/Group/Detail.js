﻿var viewModel = {};
var chat;

$(function () {
    $('#currentMessage').charactersQuantity(120);

    viewModel = new myViewModel();

    $('#currentMessage').keypress(function (e) {
        if (e.keyCode == 13 && !e.shiftKey) {
            viewModel.Send();

            return false;
        }
    });

    $('.leaveGroup').click(function () {
        var userId = $(this).attr('rel');

        bootbox.confirm({
            title: "<h4 class='title-modal'>QUITAR USUARIO</h4>",
            message: "<p class='font-text'>Esta seguro que desea quitar este usuario del grupo?</p>",
            buttons: {
                'cancel': {
                    label: "Cancelar",
                    className: "btn-cancel pull-left",
                },
                'confirm': {
                    label: "Aceptar",
                    className: "btn-confirm"
                }
            }, callback: function (result) {
                if (result) {
                    $.blockUI({ message: "" });

                    $.ajax({
                        url: "/Group/LeaveGroup",
                        data: {
                            groupId: groupId,
                            userId: userId
                        }
                    }).done(function (data) {
                        if (data.Success) {
                            window.location.href = '/Group/Detail/' + groupId;
                        }
                    });
                }
            }
        });

        return false;
    });

    $.each(messages, function (index, message) {
        viewModel.Messages.push(new messageModel(message));
    });

    $.connection.hub.start();
    chat = $.connection.BaseHubs;

    chat.client.NewMessage = function (message) {
        if (groupId == message.GroupId) {
            viewModel.Messages.push(new messageModel(message));
        }

        $('.container-chat').animate({ "scrollTop": $('.container-chat')[0].scrollHeight }, "slow");
    };

    ko.applyBindings(viewModel);

    $('.container-chat').animate({ "scrollTop": $('.container-chat')[0].scrollHeight }, "slow");
});

function myViewModel() {
    self = this;

    self.Messages = ko.observableArray();

    self.Send = function () {
        var currentMessage = $('#currentMessage').val();
        if ($.trim(currentMessage).length > 0) {
            chat.server.sendMessage(groupId, currentMessage, moment(new Date()).format("DD/MM/YYYY HH:mm:ss"));

            $('#currentMessage').val('');
        }
    }

    self.UpdateElapsedTime = function () {
        $.each(viewModel.Messages(), function (index, message) {
            var elapsedTime = CalculeElapsedTime(message.EffectDate);
            message.ElapsedTime(elapsedTime);
        });
    }
}

function messageModel(message) {
    self = this;

    self.MessageId = ko.observable(message.Id);
    self.Message = ko.observable(message.Message);
    self.UserId = ko.observable(message.User.Id);
    self.FirstName = ko.observable(message.User.FirstName);
    self.PathProfile = ko.observable(message.User.PathProfile);
    self.EffectDate = message.EffectDate;
    self.ElapsedTime = ko.observable(CalculeElapsedTime(message.EffectDate));

    self.Profile = function () {
        window.location.href = "/Account/Profile/" + self.UserId();
    }
}

window.setInterval(function () {
    viewModel.UpdateElapsedTime();
}, 10000)