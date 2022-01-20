$(document).ready(function () {

})

function updateTeamSettings() {
    //debugger;
    var url = new URL(window.location.href);
    var param = url.searchParams.get("firmUserGroupIdQuery");
    //var param = getURLParameter("firmUserGroupIdQuery");
    //console.log(param);
    var statementUploadOnHold = $("input[name=StatementUploadOnHold]:checked").val();
    $("#saveUploadOnHoldBtn").attr("disabled", true);
    $.ajax({
        url: "/TeamMember/TeamSettings/UpdateTeamSettings",
        dataType: "json",
        method: "post",
        data: {
            firmUserGroupIdQuery: param,
            onHold: statementUploadOnHold
        },
        success: function (data) {
            showPopup("Saved", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}

function updateTSEmailNotification() {
    var url = new URL(window.location.href);
    var param = url.searchParams.get("firmUserGroupIdQuery");
    var statementEmailNotification = $("input[name=EmailNotification]:checked").val();
    $("#saveEmailNotificationBtn").attr("disabled", true);
    $.ajax({
        url: "/TeamMember/TeamSettings/UpdateTSEmailNotification",
        dataType: "json",
        method: "post",
        data: {
            firmUserGroupIdQuery: param,
            emailNotification: statementEmailNotification
        },
        success: function (data) {
            showPopup("Saved", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}

function updateTSEquityWriteUps() {
    var url = new URL(window.location.href);
    var param = url.searchParams.get("firmUserGroupIdQuery");
    var statementEquityWriteUps = $("input[name=EquityWriteUps]:checked").val();
    $("#saveEquityWriteUpsBtn").attr("disabled", true);
    $.ajax({
        url: "/TeamMember/TeamSettings/UpdateTSEquityWriteUps",
        dataType: "json",
        method: "post",
        data: {
            firmUserGroupIdQuery: param,
            equityWriteUps: statementEquityWriteUps
        },
        success: function (data) {
            showPopup("Saved", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}

function updateTSEconomicCommentary() {
    var url = new URL(window.location.href);
    var param = url.searchParams.get("firmUserGroupIdQuery");
    var statementEconomicCommentary = $("input[name=EconomicCommentary]:checked").val();
    $("#saveEconomicCommentaryBtn").attr("disabled", true);
    $.ajax({
        url: "/TeamMember/TeamSettings/UpdateTSEconomicCommentary",
        dataType: "json",
        method: "post",
        data: {
            firmUserGroupIdQuery: param,
            economicCommentary: statementEconomicCommentary
        },
        success: function (data) {
            showPopup("Saved", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}

function showPopup(message, topPosition, messageType, pageReload) {
    $("#errorMessage").text(message);
    $("#activateMessage").text(messageType);

    if (topPosition == true)
        $("#popupActivateOrDeactivate").css("margin-top", "270px");
    if (topPosition == false)
        $("#popupActivateOrDeactivate").css("margin-top", "610px");

    if (pageReload == false) {
        $("#popupActivateOrDeactivate button").on('click', closePopup);
    }
    else {
        $("#popupActivateOrDeactivate button").on('click', refresh);
    }

    $("#popupActivateOrDeactivate").show();
    $("#popupActivateOrDeactivate").css("visibility", "visible");
    $(".overlay").show();
}

function closePopup() {
    $("#popupActivateOrDeactivate").hide();
    $("#popupActivateOrDeactivate").css("visibility", "hidden");
    $(".overlay").hide();
}

function refresh() {
    location.reload();
}