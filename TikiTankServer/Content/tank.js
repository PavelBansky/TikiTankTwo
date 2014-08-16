$(document).ready(function () {
    $('#panelsTab').show();
    $('#treadsTab').hide();
    $('#barrelTab').hide();
    $('#settingsTab').hide();

    $('div.optPanels').click(function () {
        $('#panelsTab').show();
        $('#treadsTab').hide();
        $('#barrelTab').hide();
        $('#settingsTab').hide();
        $('div.optPanels').addClass("selectTab");
        $('div.optTreads').removeClass("selectTab");
        $('div.optBarrel').removeClass("selectTab");
        $('div.optSettings').removeClass("selectTab");
    });

    $('div.optTreads').click(function () {
        $('#panelsTab').hide();
        $('#treadsTab').show();
        $('#barrelTab').hide();
        $('#settingsTab').hide();
        $('div.optPanels').removeClass("selectTab");
        $('div.optTreads').addClass("selectTab");
        $('div.optBarrel').removeClass("selectTab");
        $('div.optSettings').removeClass("selectTab");
    });

    $('div.optBarrel').click(function () {
        $('#panelsTab').hide();
        $('#treadsTab').hide();
        $('#barrelTab').show();
        $('#settingsTab').hide();
        $('div.optPanels').removeClass("selectTab");
        $('div.optTreads').removeClass("selectTab");
        $('div.optBarrel').addClass("selectTab");
        $('div.optSettings').removeClass("selectTab");
    });

    $('div.optSettings').click(function () {
        $('#panelsTab').hide();
        $('#treadsTab').hide();
        $('#barrelTab').hide();
        $('#settingsTab').show();
        $('div.optPanels').removeClass("selectTab");
        $('div.optTreads').removeClass("selectTab");
        $('div.optBarrel').removeClass("selectTab");
        $('div.optSettings').addClass("selectTab");
    });


    // Get panels effects
    $.getJSON("/api/panels/effects", function (data, status) {
        $("#panelsEffectsList").html(displayEffectsList(data, "panels"));
    });

    // get treads effects
    $.getJSON("/api/treads/effects", function (data, status) {
        $("#treadsEffectsList").html(displayEffectsList(data, "treads"));
    });

    // get barrel effects
    $.getJSON("/api/barrel/effects", function (data, status) {
        $("#barrelEffectsList").html(displayEffectsList(data, "barrel"));
    });    

    getActiveEffects();
});

function displayEffectsList(effects, api) {
    var html = "<ul>";

    for (var i = 0, len = effects.length; i < len; i++) {
        html += "<li onClick=\"selectEffect(" + effects[i].id + ",'" + api + "')\">" + effects[i].name + "</li>";
        console.log(effects[i]);
    }

    html += "</ul>";

    return html;
}

function displayActiveEffectData(data, api)
{
    if (data.argumentDescription == null)
        data.argumentDescription = "n/a"

    $("#" + api + "ArgumentDescription").html(data.argumentDescription);
    $("#" + api + "ArgumentValue").val(data.argument);
    $("#" + api + "ActiveEffect").text(data.name);
}


function getActiveEffects()
{
    $.getJSON("/api/panels/effect", function (data, status) {
        displayActiveEffectData(data, "panels");
    });

    $.getJSON("/api/treads/effect", function (data, status) {
        displayActiveEffectData(data, "treads");
    });

    $.getJSON("/api/barrel/effect", function (data, status) {
        displayActiveEffectData(data, "barrel");
    });
}

function selectEffect(id, api) {
    $.post("/api/" + api + "/effect/" + id,
        function (data, status) {
            displayActiveEffectData(data, api);
            console.log(datan);        
    });
}

function setEffectParameters(api, color, arg) {
    var clr = color.toString();

    $.post("/api/" + api + "/effect/",
     {
         color: clr,
         argument: arg
     },
     function (data, status) {
         // alert("Data: " + data + "\nStatus: " + status);
     });
}

function setEffectColor(api, color) {
    var clr = color.toString();

    $.post("/api/" + api + "/effect/",
     {
         color: clr,
     },
     function (data, status) {
         //displayActiveEffectData(data, api);
     });
}

function setEffectArgument(api, arg) {
    $.post("/api/" + api + "/effect/",
     {
         argument: arg
     },
     function (data, status) {
         //displayActiveEffectData(data, api);
     });
}

function setEffectSensorDrive(api, arg) {
    $.post("/api/" + api + "/effect/",
     {
         sensordriven: arg
     },
     function (data, status) {
         displayActiveEffectData(data, api);
     });
}