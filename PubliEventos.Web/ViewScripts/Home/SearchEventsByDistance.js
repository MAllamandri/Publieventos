var markersArray = [];
var positionsArray = [];
var infoboxArray = [];

$(function () {
    google.maps.event.addDomListener(window, 'load', initialize);
    var map;

    var icons = {
        user: new google.maps.MarkerImage(
         "/Content/themes/images/user-icon-small.gif"
        ),
        end: new google.maps.MarkerImage(
         '/Content/themes/images/star.png'
        )
    };

    var geocoder = new google.maps.Geocoder();

    function initialize() {
        var pos;
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {

                pos = new google.maps.LatLng(position.coords.latitude,
                                                 position.coords.longitude);

                $('#LatitudeInitial').val(position.coords.latitude);
                $('#LongitudeInitial').val(position.coords.longitude);

                var mapOptions = {
                    center: pos,
                    zoom: 15,
                    mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
                    draggableCursor: 'pointer',
                    mapTypeControl: true,
                    streetViewControl: true
                };

                makeMarker(pos, icons.user, true);

                map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

                google.maps.event.addListener(map, 'click', function (e) {
                    // Limpio los markers anteriores.
                    for (i in infoboxArray) {
                        infoboxArray[i].close();
                    }
                });
            }, function error(err) {
                console.log('error: ' + err.message);
            });
        } else {
            alert("La geolocalización no esta soportada por este navegador");
        }
    }


    function makeMarker(position, icon, initialMarker) {
        geocoder.geocode({ 'latLng': position }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                address = results[0].formatted_address;

                var infobox = new InfoBox({
                    content: "<div class='m_tooltip user-position'>" + address + "</div>",
                    pixelOffset: new google.maps.Size(-140, 0),
                    zIndex: null,
                    disableAutoPan: false,
                    boxStyle: {
                        background: "url('http://google-maps-utility-library-v3.googlecode.com/svn/trunk/infobox/examples/tipbox.gif') no-repeat",
                    },
                    closeBoxMargin: "12px 2px 2px 2px",
                    closeBoxURL: "http://www.google.com/intl/en_us/mapfiles/close.gif",
                    infoBoxClearance: new google.maps.Size(1, 1),
                    isHidden: false,
                    closeBoxMargin: "12px 2px 2px 2px",
                    zIndex: null,
                });

                var markerEnd = new google.maps.Marker({
                    position: position,
                    map: map,
                    icon: icon
                });

                infobox.open(map, markerEnd);

                google.maps.event.addListener(markerEnd, 'click', function (e) {
                    infobox.open(map, markerEnd);
                });
            }
        });
    }

    function makeInfoBox(position, icon, initialMarker, description, eventId) {
        var boxText = document.createElement("div");
        boxText.innerHTML = "<div class=''><a class='link-event m_tooltip' href='/Event/Detail/" + eventId + "'>" + description.toUpperCase() + "</a></div>";

        var infobox = new InfoBox({
            content: boxText,
            disableAutoPan: false,
            maxWidth: 0,
            pixelOffset: new google.maps.Size(-140, 0),
            zIndex: null,
            boxStyle: {
                background: "url('http://google-maps-utility-library-v3.googlecode.com/svn/trunk/infobox/examples/tipbox.gif') no-repeat",
            },
            closeBoxMargin: "12px 2px 2px 2px",
            closeBoxURL: "http://www.google.com/intl/en_us/mapfiles/close.gif",
            infoBoxClearance: new google.maps.Size(1, 1),
            isHidden: false,
            pane: "floatPane",
            enableEventPropagation: false
        });

        var markerEnd = new google.maps.Marker({
            position: position,
            map: map,
            icon: icon
        });

        if (!initialMarker) {
            markersArray.push(markerEnd);
            infoboxArray.push(infobox);
        }

        google.maps.event.addListener(markerEnd, 'click', function (e) {
            infobox.open(map, markerEnd);
        });
    }

    $(".checkbox-distance").change(function () {
        if (this.checked) {
            SearchEvents();
        }
    });

    function SearchEvents() {
        $.blockUI({ message: "<div style='font-size: 16px; padding-top: 11px;'><p>Buscando Eventos...</p><div>" });
        var distance = $("input[name='distance']:checked").val();

        // Limpio los markers anteriores.
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }

        // Limpio los markers anteriores.
        for (i in infoboxArray) {
            infoboxArray[i].close();
        }

        positionsArray = [];

        $.ajax({
            type: 'POST',
            url: '/Home/SearchEventsByDistance',
            data: {
                latitudeInitial: $('#LatitudeInitial').val(),
                longitudeInitial: $('#LongitudeInitial').val(),
                maxDistance: distance
            }
        }).done(function (data) {
            if (data.Events != null) {

                $.each(data.Events, function (index, event) {
                    var pos = new google.maps.LatLng(event.Latitude, event.Longitude);

                    makeInfoBox(pos, icons.end, false, event.Title, event.Id);

                    positionsArray.push(pos);
                });

                var latlngbounds = new google.maps.LatLngBounds();

                for (var i = 0; i < positionsArray.length; i++) {
                    latlngbounds.extend(positionsArray[i]);
                }

                map.fitBounds(latlngbounds);
            }
            $.unblockUI();
        }).error(function () {
            $.unblockUI();
        });
    }
})