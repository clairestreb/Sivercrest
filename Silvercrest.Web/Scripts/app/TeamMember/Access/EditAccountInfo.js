$(document).ready(function () {

    $("#saveNewEmail").removeClass("disActiveButton");
    $("#saveNewEmail").attr("disabled", false);
    $("#saveStatusBtn").removeClass("disActiveButton");
    $("#saveStatusBtn").attr("disabled", false);
    $("#sendNotificationsBtn").removeClass("disActiveButton");
    $("#sendNotificationsBtn").attr("disabled", false);
    $("#postBtn").removeClass("disActiveButton");
    $("#postBtn").attr("disabled", false);
    $("#activateBtn").removeClass("disActiveButton");
    $("#activateBtn").attr("disabled", false);
})

function saveNewEmail() {
    $("#saveNewEmail").addClass("disActiveButton");
    $("#saveNewEmail").attr("disabled", true);
    var re = /^[\w-\.]+@[\w-]+\.[a-z]{2,4}$/i;
    var myMail = document.getElementById('currentUserEmail').value;
    var valid = re.test(myMail);

    if (!valid) {
        showPopup("Please enter a valid email address.", true, "Error", false); return;
        return false;
    }

    if ($('currentUserEmail').val() == "" && $('#accountName') == "") {
        return false;
    }

    var accountName = $('#accountName').val();

    $.ajax({
        url: "/TeamMember/Access/UpdateEmail",
        dataType: "json",
        method: "post",
        data: {
            contactId: $("#contactId").val(),
            email: $('#currentUserEmail').val()
        },
        success: function (data) {
            showPopup("Email Address Updated!", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please make sure this email address is not already in use.", true, "Error", true); return;
        }
    })
}

function saveStatus(firmUserGroupId) {
    $("#saveStatusBtn").addClass("disActiveButton");
    $("#saveStatusBtn").attr("disabled", true);
    var email = document.getElementById('currentUserEmail').value;
    if ($('#selectStatus').val() == null && $('#accountName') == "" /*&& email == ""*/) {
        return false;
    }
    var accountName = $('#accountName').val();
    var isActive = $("input[name='IsActive']:checked").val();

    $.ajax({
        url: "/TeamMember/Access/UpdateStatus",
        dataType: "json",
        method: "post",
        data: {
            contactId: $("#contactId").val(),
            isActive: isActive,
            firmUserGroupId: firmUserGroupId
            //email: email
        },
        success: function (data) {
            showPopup("Status Updated!", true, "Success", true); return;
            //showPopup("The portal setup email was sent to the client.", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}

function sendNotifications() {
    $("#sendNotificationsBtn").addClass("disActiveButton");
    $("#sendNotificationsBtn").attr("disabled", true);
    if ($('#selectSendNotification').val() == null && $('#accountName') == "") {
        return false;
    }
    var accountName = $('#accountName').val();
    var sendNotifications = $("input[name='SendNotifications']:checked").val();

    $.ajax({
        url: "/TeamMember/Access/SendEmailNotifications",
        dataType: "json",
        method: "post",
        data: {
            contactId: $("#contactId").val(),
            sendNotifications: sendNotifications
        },
        success: function (data) {
            showPopup("Email Notification Settings Updated! ", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}

function postEqtyWriteUps() {
    $("#postBtn").addClass("disActiveButton");
    $("#postBtn").attr("disabled", true);
    if ($('#selectPost').val() == null && $('#accountName') == "") {
        return false;
    }
    var accountName = $('#accountName').val();
    var post = $("input[name='Post']:checked").val();

    $.ajax({
        url: "/TeamMember/Access/PostEqtyWriteUps",
        dataType: "json",
        method: "post",
        data: {
            contactId: $("#contactId").val(),
            post: post
        },
        success: function (data) {
            showPopup("Settings Updated!", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}


function activateOrDeactivateUser() {
    //debugger;
    $("#activateBtn").addClass('disActiveButton');
    $("#activateBtn").attr("disabled", true);
    var email = document.getElementById('currentUserEmail').value;
    var fullName = document.getElementById('accountName').value;
    var activate = $('input[name=IsActive]:checked').val();
    if (email == '' || fullName == '') { return false; }
    //if (activate == 'True') {
    //    showPopup("User with this email address is already active.", true, email, true);
    //    return;
    //}
    //var setActivate = (activate == 'False');

    if (activate == "False") {
        activate = "true";
    }

    $.ajax({
        url: "/TeamMember/Access/ActivateUser",
        data: "{ 'email': '" + email + "', 'activate': '" + activate + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            //debugger;

            if (data == true) {
                showPopup("The portal setup email was sent to the client.", true, "Success", true);
            }

            //if (data == false) {
            //    showPopup("The email was sent less than 12 hours ago", true, "Error", true);
            //}
            if (data == false) {
                showPopup("The email was sent less than 10 minutes ago", true, "Error", true);
            }
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}

function showPopup(message, topPosition, messageType, pageReload) {

    //debugger;
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

function postEconomicCommentary() {
    $("#economicCommentaryBtn").addClass("disActiveButton");
    $("#economicCommentaryBtn").attr("disabled", true);
    if ($('#selectPost').val() == null && $('#accountName') == "") {
        return false;
    }
    var accountName = $('#accountName').val();
    var economicCommentary = $("input[name='EconomicCommentary']:checked").val();

    $.ajax({
        url: "/TeamMember/Access/PostEconomicCommentary",
        dataType: "json",
        method: "post",
        data: {
            contactId: $("#contactId").val(),
            economicCommentary: economicCommentary
        },
        success: function (data) {
            showPopup("Settings Updated!", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}

function changeTwoFactorAuth() {
    $("#twoFactorAuthBtn").addClass("disActiveButton");
    $("#twoFactorAuthBtn").attr("disabled", true);
    if ($('#selectPost').val() == null && $('#accountName') == "") {
        return false;
    }
    var accountName = $('#accountName').val();
    var twoFactorAuthInt = parseInt($("#twoFactorAuth").val());
    var email = document.getElementById('currentUserEmail').value;

    $.ajax({
        url: "/TeamMember/Access/ChangeTwoFactorAuth",
        dataType: "json",
        method: "post",
        data: {
            contactId: $("#contactId").val(),
            twoFactorAuthInt: twoFactorAuthInt,
            email: email
        },
        success: function (data) {
            if (data.Error) {
                showPopup(data.Error, true, "Error", true); return;
            }
            showPopup("Settings Updated!", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}

function changePhoneNumber() {
    $("#phoneNumberBtn").addClass("disActiveButton");
    $("#phoneNumberBtn").attr("disabled", true);
    if ($('#selectPost').val() == null && $('#accountName') == "") {
        return false;
    }
    var accountName = $('#accountName').val();
    var phoneNumber = $("input[name='PhoneNumber']").val();
    var email = document.getElementById('currentUserEmail').value;

    $.ajax({
        url: "/TeamMember/Access/ChangePhoneNumber",
        dataType: "json",
        method: "post",
        data: {
            contactId: $("#contactId").val(),
            phoneNumber: phoneNumber,
            email: email
        },
        success: function (data) {
            showPopup("Settings Updated!", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    })
}


$("#contact_settings_form").submit(function (e) {
    e.preventDefault();
});

$("#phoneNumberBtn").click(function (e) {
    e.preventDefault();
    let valid = $("#contact_settings_form").valid();
    if (valid)
    {
        changePhoneNumber();
    }
});

$("#twoFactorAuthBtn").click(function (e) {
    e.preventDefault();
    changeTwoFactorAuth();
});

$("#economicCommentaryBtn").click(function (e) {
    e.preventDefault();
    postEconomicCommentary();
});

$("#activateBtn").click(function (e) {
    e.preventDefault();
    activateOrDeactivateUser();
});

$("#postBtn").click(function (e) {
    e.preventDefault();
    postEqtyWriteUps();
});

$("#sendNotificationsBtn").click(function (e) {
    e.preventDefault();
    sendNotifications();
});

$("#saveStatusBtn").click(function (e) {
    e.preventDefault();
    let firmUserGroupId = $('#firmUserGroupIdInp').val();
    saveStatus(firmUserGroupId);
});

$("#saveNewEmail").click(function (e) {
    e.preventDefault();
    saveNewEmail();
});   