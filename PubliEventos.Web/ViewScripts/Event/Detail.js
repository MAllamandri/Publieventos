﻿$(function () {
    $('#tabDetail').click(function () {
        removeActiveClass();
        $('#regionLocalization').hide();
        $('#regionContents').hide();
        $(this).addClass("active-link");
        $('#regionDetail').show();
    });

    $('#tabLocalization').click(function () {
        removeActiveClass();
        $('#regionDetail').hide();
        $('#regionContents').hide();
        $('#regionLocalization').show();
        $(this).addClass("active-link");
    });

    $('#tabContents').click(function () {
        removeActiveClass();
        $('#regionDetail').hide();
        $('#regionLocalization').hide();
        $('#regionContents').show();
        $(this).addClass("active-link");
    });

    //$('#tabDetail').click();

    function removeActiveClass() {
        $('#tabContents').removeClass("active-link");
        $('#tabLocalization').removeClass("active-link");
        $('#tabDetail').removeClass("active-link");
    }
});