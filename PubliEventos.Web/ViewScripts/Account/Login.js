$(function () {
    if (data.IsLogin === true) {
        $('#login').attr('class', 'tab-pane active');
        $('#create').attr('class', 'tab-pane fane');
        $('#tabLogin').attr('class', 'active');
        $('#tabCreate').removeAttr('class');
        $('#loginModal').attr('class', 'col-md-4')
    } else {
        $('#create').attr('class', 'tab-pane active');
        $('#login').attr('class', 'tab-pane fane');
        $('#tabCreate').attr('class', 'active');
        $('#tabLogin').removeAttr('class');
        $('#loginModal').attr('class', 'col-md-6')
    }

    $('#tabCreate').click(function () {
        $('#loginModal').attr('class', 'col-md-6');
        $('#First').attr('class', "col-md-3");
        $('#Second').attr('class', 'col-md-3');
    });

    $('#tabLogin').click(function () {
        $('#loginModal').attr('class', 'col-md-4');
        $('#First').attr('class', "col-md-4");
        $('#Second').attr('class', 'col-md-4');
    });
});