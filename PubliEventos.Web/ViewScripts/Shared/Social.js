$(function () {
    (function () {
        var e = document.createElement('script'); e.async = true;
        e.src = document.location.protocol +
        '//connect.facebook.net/en_US/all.js';
        document.getElementById('fb-root').appendChild(e);
    }());

    window.fbAsyncInit = function () {
        FB.init({
            appId: '321354234728519', status: true, cookie: true,
            xfbml: true
        });
    };

    window.fbShare = function(url, title, description, image) {
        FB.ui(
        {
            method: 'feed',
            name: title,
            link: url,
            picture: image,
            caption: ' ',
            description: description.length > 0 ? description.substring(0, 269) : " ",
            message: ' '
        });
        return false;
    };
});