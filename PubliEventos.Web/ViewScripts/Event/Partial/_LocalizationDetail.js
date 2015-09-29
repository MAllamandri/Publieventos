$(function () {
    google.maps.event.addDomListener(window, 'load', initialize);

    function initialize() {
        var latlng = new google.maps.LatLng(parseFloat($('#Latitude').val().replace(",", ".")), parseFloat($('#Longitude').val().replace(",", ".")));

        var icons = {
            start: new google.maps.MarkerImage(
             "/Content/themes/images/star.png"
            ),
            end: new google.maps.MarkerImage(
             '/Content/themes/images/star-gray.png'
            )
        };

        var infoBoxs = [];
        var markersArray = [];
        var directionsService = new google.maps.DirectionsService();
        var geocoder = new google.maps.Geocoder();

        var mapOptions = {
            center: latlng,
            zoom: 15,
            mapTypeControlOptions: {
                style: google.maps.MapTypeControlStyle.DROPDOWN_MENU,
                position: google.maps.ControlPosition.RIGHT_TOP
            },
            draggableCursor: 'pointer',
            mapTypeControl: true,
            streetViewControl: true,
            styles: stylesMap[0]
        };

        map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

        var input = (document.getElementById('pac-input'));
        map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

        var options = {
            componentRestrictions: { country: 'AR' }//Argentina solamente.
        };

        var autocomplete = new google.maps.places.Autocomplete(input, options);
        autocomplete.bindTo('bounds', map);

        var directionsDisplay = new google.maps.DirectionsRenderer({ suppressMarkers: true, suppressInfoWindows: true, draggable: true });
        directionsDisplay.setPanel(document.getElementById("directionsPanel"));
        directionsDisplay.setMap(map);

        if ($('#Latitude').val() != "" && $('#Longitude').val() != "") {
            makeMarker(latlng, icons.start);
        }

        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            var place = autocomplete.getPlace();
            if (!place.geometry) {
                return;
            }

            // If the place has a geometry, then present it on a map.
            if (place.geometry.viewport) {
                map.fitBounds(place.geometry.viewport);
            } else {
                map.setCenter(place.geometry.location);
            }

            // Seteo los hidden para guardar la posición.
            if (place.geometry.location.H != undefined && place.geometry.location.L != undefined) {
                // Armo el recorrido.
                calcRoute(place.geometry.location.H, place.geometry.location.L);
                setLatLng(place.geometry.location.H, place.geometry.location.L);
            } else {
                $('#DestinationLatitude').val("");
                $('#DestinationLongitude').val("");
            }
        });

        google.maps.event.addListener(map, 'click', function (event) {
            $('#pac-input').val("");

            if (!event.latLng) {
                $('#DestinationLatitude').val("");
                $('#DestinationLongitude').val("");

                return;
            }

            calcRoute(event.latLng.lat(), event.latLng.lng());
            setLatLng(event.latLng.lat(), event.latLng.lng());
        });

        function calcRoute(latDestination, lngDestination) {
            var lat = parseFloat($('#Latitude').val().replace(",", "."));
            var lng = parseFloat($('#Longitude').val().replace(",", "."));

            var start = new google.maps.LatLng(lat, lng);
            var end = new google.maps.LatLng(latDestination, lngDestination);

            if ((lat != latDestination || lng != lngDestination) && lngDestination != "" && latDestination != "") {
                $('#infoRoutes').hide();

                var selectedMode = document.getElementById("mode").value;

                // Limpio los markers anteriores.
                for (i in markersArray) {
                    markersArray[i].setMap(null);
                }

                for (i in infoBoxs) {
                    infoBoxs[i].close();
                }

                var request = {
                    origin: end,
                    destination: start,
                    travelMode: google.maps.TravelMode[selectedMode],
                    unitSystem: google.maps.UnitSystem.METRIC,
                    provideRouteAlternatives: true
                };

                $.blockUI({ message: "" });
                directionsService.route(request, function (result, status) {
                    if (status == google.maps.DirectionsStatus.OK) {
                        $('#NotFoundRoutes').hide();
                        directionsDisplay.setDirections(result);

                        var leg = result.routes[0].legs[0];

                        makeMarker(leg.start_location, icons.end);
                        makeMarker(leg.end_location, icons.start);
                    } else {
                        $('#NotFoundRoutes').show();
                        directionsDisplay.setMap(null);
                        directionsDisplay.setPanel(null);
                        setLatLng("", "");

                        directionsDisplay = new google.maps.DirectionsRenderer({ suppressMarkers: true, suppressInfoWindows: true, draggable: true });
                        directionsDisplay.setPanel(document.getElementById("directionsPanel"));
                        directionsDisplay.setMap(map);

                        makeMarker(start, icons.start);
                    }

                    $.unblockUI();
                });
            }
        }

        function makeMarker(position, icon) {
            geocoder.geocode({ 'latLng': position }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    address = results[0].formatted_address;

                    var infobox = new InfoBox({
                        content: "<div class='infoWindow'>" + address + "</div>",
                        disableAutoPan: false,
                        zIndex: null,
                    });

                    var markerEnd = new google.maps.Marker({
                        position: position,
                        map: map,
                        icon: icon
                    });

                    infobox.open(map, markerEnd);
                    markersArray.push(markerEnd);
                    infoBoxs.push(infobox);

                    google.maps.event.addListener(markerEnd, 'click', function (e) {
                        infobox.open(map, markerEnd);
                    });
                }
            });
        }

        $('#mode').change(function () {
            calcRoute($('#DestinationLatitude').val(), $('#DestinationLongitude').val());
        });

        $(document).on('click', '.mapStyle', function () {
            var mapOptions = {
                styles: stylesMap[$(this).attr('rel')]
            };

            map.setOptions(mapOptions);
            return false;
        });
    }
});

function setLatLng(latitude, longitude) {
    $('#DestinationLatitude').val(latitude.toString());
    $('#DestinationLongitude').val(longitude.toString());
}