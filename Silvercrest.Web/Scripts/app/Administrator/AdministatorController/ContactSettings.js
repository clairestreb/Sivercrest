$(document).ready(function () {
    $("#saveNewEmail").removeClass("disActiveButton");
    $("#saveNewEmail").attr("disabled", false);
    $("#saveStatusBtn").removeClass("disActiveButton");
    $("#saveStatusBtn").attr("disabled", false);
    $("#activateBtn").removeClass("disActiveButton");
    $("#activateBtn").attr("disabled", false);
});

function saveNewEmail() {
    $("#saveNewEmail").addClass("disActiveButton");
    $("#saveNewEmail").attr("disabled", true);
    var re = /^[\w-\.]+@[\w-]+\.[a-z]{2,4}$/i;
    var myMail = document.getElementById('currentUserEmail').value;
    var valid = re.test(myMail);

    if (!valid) {
        showPopup("Please enter a valid email address.", true, "Error", false);
        return false;
    }

    if ($('currentUserEmail').val() === "" && $('#accountName') === "") {
        return false;
    }

    $.ajax({
        url: "/Administrator/Administrator/UpdateEmail",
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
    });
}

function saveStatus() {
    $("#saveStatusBtn").addClass("disActiveButton");
    $("#saveStatusBtn").attr("disabled", true);
    if ($('#selectStatus').val() === null && $('#accountName') === "") {
        return false;
    }
    var isActive = $("input[name='IsActive']:checked").val();

    $.ajax({
        url: "/Administrator/Administrator/UpdateStatus",
        dataType: "json",
        method: "post",
        data: {
            contactId: $("#contactId").val(),
            isActive: isActive
        },
        success: function (data) {
            showPopup("Status Updated!", true, "Success", true); return;
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    });
}

function activateOrDeactivateUser() {
    $("#activateBtn").addClass('disActiveButton');
    $("#activateBtn").attr("disabled", true);
    var email = document.getElementById('currentUserEmail').value;
    var fullName = document.getElementById('accountName').value;
    var activate = $('input[name=IsActive]:checked').val();
    if (email === '' || fullName === '') { return false; }

    if (activate === "False") {
        activate = "true";
    }

    $.ajax({
        url: "/Administrator/Administrator/ActivateUserContact",
        data: "{ 'email': '" + email + "', 'activate': '" + activate + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data === true) {
                showPopup("The portal setup email was sent to the client.", true, "Success", true);
            }

            if (data === false) {
                showPopup("The email was sent less than 10 minutes ago", true, "Error", true);
            }
        },
        error: function (res) {
            showPopup("Something went wrong, please try again.", true, "Error", true); return;
        }
    });
}

function changeTwoFactorAuth() {
    $("#twoFactorAuthBtn").addClass("disActiveButton");
    $("#twoFactorAuthBtn").attr("disabled", true);
    if ($('#selectPost').val() === null && $('#accountName') === "") {
        return false;
    }
    var twoFactorAuthInt = parseInt($("#twoFactorAuth").val());
    var email = document.getElementById('currentUserEmail').value;

    $.ajax({
        url: "/Administrator/Administrator/ChangeTwoFactorAuth",
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
    });
}

function changePhoneNumber() {
    $("#phoneNumberBtn").addClass("disActiveButton");
    $("#phoneNumberBtn").attr("disabled", true);
    if ($('#selectPost').val() === null && $('#accountName') === "") {
        return false;
    }
    var phoneNumber = $("input[name='PhoneNumber']").val();
    var email = document.getElementById('currentUserEmail').value;

    $.ajax({
        url: "/Administrator/Administrator/ChangePhoneNumber",
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
    });
}

function showPopup(message, topPosition, messageType, pageReload) {
    $("#errorMessage").text(message);
    $("#activateMessage").text(messageType);

    if (topPosition === true)
        $("#popupActivateOrDeactivate").css("margin-top", "270px");
    if (topPosition === false)
        $("#popupActivateOrDeactivate").css("margin-top", "610px");

    if (pageReload === false) {
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
    var url = new URL(window.location.href);
    let emailUrl = url.searchParams.get("email");
    var email = document.getElementById('currentUserEmail').value;
    if (emailUrl !== email)
    {
        url.searchParams.set("email", email);
        location.assign(url);
        return;
    }
    location.reload();
}

$("#contact_settings_form").submit(function (e) {
    e.preventDefault();
});

$("#activateBtn").click(function (e) {
    e.preventDefault();
    activateOrDeactivateUser();
});

$("#saveStatusBtn").click(function (e) {
    e.preventDefault();
    saveStatus();
});

$("#saveNewEmail").click(function (e) {
    e.preventDefault();
    saveNewEmail();
});  

$("#phoneNumberBtn").click(function (e) {
    e.preventDefault();
    let valid = $("#contact_settings_form").valid();
    if (valid) {
        changePhoneNumber();
    }
});

$("#twoFactorAuthBtn").click(function (e) {
    e.preventDefault();
    changeTwoFactorAuth();
});