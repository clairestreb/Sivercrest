
$(document).ready(function () {
    $("#accountIds").val(localStorage["alreadyChecked"]);

    $("#AccountAccessGrid").kendoGrid({

        dataSource: {
            transport: {
                read: {
                    url: "/TeamMember/Access/GetAccountAccess?contactId=" + $("#contactId").val(),
                    dataType: "json"
                }
            },
            groupable: true,
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            schema: {
                model: { id: "Id" }
            }
        },
        dataBound: function (e) {
            var inputs = $("input.chkbx1:checked");

            var checkedInputs = inputs.map(function () {
                return this.id;
            }).get();

            var checkedInputsToString = checkedInputs.join(";");
            $("#accountIds").val(checkedInputsToString);
            localStorage.setItem("alreadyChecked", $("#accountIds").val());
        },
        height: 335,
        filterable: true,
        sortable: true,
        columns: [
            { field: "AccountId", hidden: true },
            {
                field: "AccountName",
                title: "Accounts",
                width: 445
            },
            {
                field: "ShortName",
                title: "Short Name",
                width: 178
            },
            {
                field: "ManagerCode",
                title: "Manager",
                width: 178
            },
            {
                field: "AccessType",
                title: "Select",
                template: "<div class='check'><input type='checkbox' #= AccessType ? \'checked=\"checked\"\' : \"\" # class='chkbx1' id='#=AccountId#'/>" +
                    "</div>",
                width: 89
            }

        ]

    })


    $("#AccountAccessGrid .k-grid-content").on("change", "input.chkbx1", function (e) {
        var inputs = $("input.chkbx1:checked");
        //console.log(inputs);

        var checkedInputs = inputs.map(function () {
            return this.id;
        }).get();

        var checkedInputsToString = checkedInputs.join(";");
        $("#accountIds").val(checkedInputsToString);
        localStorage.setItem("alreadyChecked", $("#accountIds").val());
    });   
});



function remove() {
    $.ajax({
        url: "/TeamMember/Access/RemoveAccess",
        type: "get", //send it through get method
        data: {
            contactId: $("#contactId").val(),
            accountIds: $("#accountIds").val()
        },
        success: function (response) {
            //$("#accountIds").val("");
            $('#AccountAccessGrid').data('kendoGrid').dataSource.read();
            $('#NonAccountAccessGrid').data('kendoGrid').dataSource.read();

        },
        error: function (xhr) {
            alert("Error");
            console.log("error");
        }
    });
}

function showPopup(message, topPosition, messageType, pageReload) {
    $("#errorMessage").text(message);
    $("#activateMessage").text(messageType);

    if (topPosition == true)
        $("#popupActivateOrDeactivate").css("margin-top", "270px");
    if (topPosition == false)
        $("#popupActivateOrDeactivate").css("margin-top", "610px");

    //if (pageReload == false) {
    //    $("#popupActivateOrDeactivate button").on('click', doNothing);
    //}
    //else {
    //    $("#popupActivateOrDeactivate button").on('click', refresh);
    //}

    $("#popupActivateOrDeactivate").show();
    $("#popupActivateOrDeactivate").css("visibility", "visible");
    $(".overlay").show();
}

function showConfirm() {
    var existingName = $("div.pad0 a.active").text();

    $("#confirmBodyMessage").text("Are you sure you want to delete \"" + existingName + "\"?");

    $("#popupDeleteConfirm").css("margin-top", "270px");

    $("#popupDeleteConfirm button.cancel").on('click', doNothing);
    $("#popupDeleteConfirm button.ok").on('click', DeleteGroup);

    $("#popupDeleteConfirm").show();
    $("#popupDeleteConfirm").css("visibility", "visible");
    $(".overlay").show();
}
