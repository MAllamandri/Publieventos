var stylesMap = [];
stylesMap.push([{
    "featureType": "all",
    "stylers": [{
        "saturation": 0
    }, {
        "hue": "#e7ecf0"
    }]
},
{
    "featureType": "road",
    "stylers": [
        {
            "saturation": -70
        }
    ]
},
{
    "featureType": "transit",
    "stylers": [
        {
            "visibility": "off"
        }
    ]
},
{
    "featureType": "poi",
    "stylers": [
        {
            "visibility": "off"
        }
    ]
},
{
    "featureType": "water",
    "stylers": [{
        "visibility": "simplified"
    }, {
        "saturation": -60
    }]
}]);

stylesMap.push([{
    "featureType": "water",
    "elementType": "all",
    "stylers": [{
        "hue": "#bbbbbb"
    }, {
        "saturation": -100
    }, {
        "lightness": -4
    }, {
        "visibility": "on"
    }]
}, {
    "featureType": "landscape",
    "elementType": "all",
    "stylers": [{
        "hue": "#999999"
    }, {
        "saturation": -100
    }, {
        "lightness": -33
    }, {
        "visibility": "on"
    }]
}, {
    "featureType": "road",
    "elementType": "all",
    "stylers": [{
        "hue": "#999999"
    }, {
        "saturation": -100
    }, {
        "lightness": -6
    }, {
        "visibility": "on"
    }]
}, {
    "featureType": "poi",
    "elementType": "all",
    "stylers": [{
        "hue": "#aaaaaa"
    }, {
        "saturation": -100
    }, {
        "lightness": -15
    }, {
        "visibility": "on"
    }]
}]);

stylesMap.push([{
    "featureType": "water",
    "elementType": "geometry",
    "stylers": [{
        "color": "#e9e9e9"
    }, {
        "lightness": 17
    }]
}, {
    "featureType": "landscape",
    "elementType": "geometry",
    "stylers": [{
        "color": "#f5f5f5"
    }, {
        "lightness": 20
    }]
}, {
    "featureType": "road.highway",
    "elementType": "geometry.fill",
    "stylers": [{
        "color": "#ffffff"
    }, {
        "lightness": 17
    }]
}, {
    "featureType": "road.highway",
    "elementType": "geometry.stroke",
    "stylers": [{
        "color": "#ffffff"
    }, {
        "lightness": 29
    }, {
        "weight": 0.2
    }]
}, {
    "featureType": "road.arterial",
    "elementType": "geometry",
    "stylers": [{
        "color": "#ffffff"
    }, {
        "lightness": 18
    }]
}, {
    "featureType": "road.local",
    "elementType": "geometry",
    "stylers": [{
        "color": "#ffffff"
    }, {
        "lightness": 16
    }]
}, {
    "featureType": "poi",
    "elementType": "geometry",
    "stylers": [{
        "color": "#f5f5f5"
    }, {
        "lightness": 21
    }]
}, {
    "featureType": "poi.park",
    "elementType": "geometry",
    "stylers": [{
        "color": "#dedede"
    }, {
        "lightness": 21
    }]
}, {
    "elementType": "labels.text.stroke",
    "stylers": [{
        "visibility": "on"
    }, {
        "color": "#ffffff"
    }, {
        "lightness": 16
    }]
}, {
    "elementType": "labels.text.fill",
    "stylers": [{
        "saturation": 36
    }, {
        "color": "#333333"
    }, {
        "lightness": 40
    }]
}, {
    "elementType": "labels.icon",
    "stylers": [{
        "visibility": "off"
    }]
}, {
    "featureType": "transit",
    "elementType": "geometry",
    "stylers": [{
        "color": "#f2f2f2"
    }, {
        "lightness": 19
    }]
}, {
    "featureType": "administrative",
    "elementType": "geometry.fill",
    "stylers": [{
        "color": "#fefefe"
    }, {
        "lightness": 20
    }]
}, {
    "featureType": "administrative",
    "elementType": "geometry.stroke",
    "stylers": [{
        "color": "#fefefe"
    }, {
        "lightness": 17
    }, {
        "weight": 1.2
    }]
}]);