$(document).ready(function () {

    var contacts = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/Administrator/Administrator/GetContacts",
                type: "GET",
                dataType: "json"
            }
        },
        schema: {
            model: {
                id: "Id",
                fields: {
                    id: { type: "Id" },
                    name: { type: "string" }
                }
            }
        }
    });

    var autoComplete = $("#contact").kendoAutoComplete({
        minLength: 3,
        dataSource: contacts,
        serverFiltering: true,
        dataTextField: "DisplayName",
        filter: "contains",
        select: function (ev) {
            console.log(ev);
            var dataItem = this.dataItem(ev.item.index());
            $("#selectedContactId").val(dataItem.Id);
        }
    }).data("kendoAutoComplete");


    function openPopup(headerMessage, bodyMessage, style) {
        $("#emailPopupMessage").text(headerMessage);
        $("#emailPopupBodyMessage").text(bodyMessage);
        $("#popupSendEmail").show();
        $("#popupHeaderStyle").addClass(" " + style);
        $("#popupFooterStyle").addClass(" " + style);
        $("#popupSendEmail").css("visibility", "visible");
        $(".overlay").show();
    }

    $(".btn-create").click(function () {
        $("#popupSendEmail").hide();
        $("#popupSendEmail").css("visibility", "hidden");
        $(".overlay").hide();
    });

    $("#createUser").click(function () {
        var email = $("#email").val();
        var contactId = $("#selectedContactId").val();
        if (isEmail(email)) {
            $.ajax({
                url: "/Administrator/Administrator/CreateUser?contactEmail=" + email + "&contactId=" + contactId,
                type: "POST",
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == true) {
                        $("#contact").val("");
                        $("#email").val("");
                        openPopup("Success", "User Setup was successful!", "alert alert-success");
                    }
                    else if (data === "email") {
                        openPopup("Duplicate Email Address", "This Email Address is already in use.", "alert alert-warning");
                    }
                    else if (data === "contact") {
                        openPopup("Duplicate Contact", "This contact is already setup.", "alert alert-warning");
                    }
                    else {
                        openPopup("Email Error", "Email was not sent due to an application error! Please try again.", "alert alert-danger");
                    }

                },
            });
        }
        else {
            openPopup("Invalid Email Address", "Email Address is not valid.", "alert alert-danger");
        }
    });

    function isEmail(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }
  
});
function closePopup() {
    $("#popupSendEmail").hide();
    $(".overlay").hide();
    $("#email").val("");
    $("#contact").val(""); 

}
