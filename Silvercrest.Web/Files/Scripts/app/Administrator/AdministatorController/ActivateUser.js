$(document).ready(function () {
    var userGrid = $("#UserGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "/Administrator/Administrator/GetUsers",
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
                model: {
                    id: "Id",
                    //fields: {
                    //    ContactId: {
                    //        type: "string",
                    //        validation: { required: true }
                    //    }
                    //}
                }
            }
        },
        height: 335,
        filterable: true,
        sortable: true,
        columns: [
            {
                field: "Email",
                type: "string",
                title: "Email / Username",
                width: 250,
                attributes: {
                    style: "font-family:goudyosSC"
                },
            },
            {
                field: "ContactCode",
                type: "string",
                title: "Contact Code",
                width: 250,
                attributes: {
                    style: "font-family:goudyosSC"
                },

                //sortable: {
                //    compare: function (a, b) {
                //        return a.ContactId - b.ContactId;
                //    }
                //}
            },
            {
                field: "FullName",
                type: "string",
                title: "Contact Name",
                width: 120,
                attributes: {
                    style: "font-family:goudyosSC"
                },
                template: "<a href=\"/Administrator/Administrator/ContactSettings?email=#:Email#\">#:FullName#</a>"
            },
            {
                field: "IsActive",
                title: "Active",
                template: "<div class='check'><input type='checkbox' #= IsActive ? \'checked=\"checked\"\' : \"\" # class='chkbx-active' id='activeCheck_#=Id#'/>" +
                    "<label for='activeCheck_#=Id#'></label></div>",
                headerAttributes: {
                    style: "text-align: center;"
                },
                width: 150
            }]



    }).data("kendoGrid");
    $("#grid .k-grid-content").on("change", "input.chkbx", function (e) {

        var grid = $("#UserGrid").data("kendoGrid"),
            dataItem = grid.dataItem($(this).closest("tr"));

        dataItem.set("IsActive", this.checked);
    });
    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#UserGrid").data("kendoGrid");

        if (value) {
            grid.dataSource.query({
                filter: {
                    logic: "or", 
                    filters: [                           
                        { field: "FullName", operator: "contains", value: value },
                        { field: "Email", operator: "contains", value: value },
                        { field: "ContactCode", operator: "contains", value: value }
                    ]
                }
            });
        } else {
            grid.dataSource.filter({});
        }
    });

    userGrid.table.on("click", ".chkbx-active", activated);

    $(".btn-activate").click(function () {
        $("#popupActivateOrDeactivate").hide();
        $("#popupActivateOrDeactivate").css("visibility", "hidden");
        $(".overlay").hide();
    });
});

function activated() {
    var checked = this.checked;
    row = $(this).closest("tr");
    grid = $("#UserGrid").data("kendoGrid");
    var ds = grid.dataSource;
    dataItem = grid.dataItem(row);
    if (checked) {
        for (var c = 0; c < ds.total(); c++) {
            if (ds.at(c).Email == dataItem.Email && ds.at(c).IsActive && ds.at(c).id != dataItem.id) {

                switchCheckbox(dataItem.id, false);
                showPopup(undefined, "User with this email address is already active.", dataItem.Email, dataItem.FullName);
                return;
            }
            /*          if (ds.at(c).FullName == dataItem.FullName && ds.at(c).IsActive && ds.at(c).id != dataItem.id) 
                        {
                            switchCheckbox(dataItem.id, false);
                            showPopup(undefined, "User with this id is already active.", dataItem.Email, dataItem.FullName);
            
                            return;
                        }
            */
        }
    }
    activateOrDeactivateUserAjax(dataItem.id, checked, dataItem.UserName, dataItem.Email, dataItem.FullName);

}

function activateOrDeactivateUserAjax(id, activate, name, email, fullname) {
    $.ajax({
        url: "/Administrator/Administrator/ActivateUser",
        data: "{ 'id': '" + id + "', 'activate': '" + activate + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            showPopup(activate, name, email, fullname);
            $("#UserGrid").data("kendoGrid").dataSource.read();
        }
    });
}

function switchCheckbox(id, activate) {

    if (activate) {
        $("#deactiveCheck_" + id).prop('checked', false);

        $("#deactiveCheck_" + id).prop('disabled', false);
        $("#activeCheck_" + id).prop('disabled', false);
    } else {
        $("#activeCheck_" + id).prop('checked', false);

        $("#activeCheck_" + id).prop('disabled', false);
        $("#deactiveCheck_" + id).prop('disabled', false);
    }
}

function showPopup(activate, userName, userEmail, fullname) {
    if (activate == undefined) {
        $("#activateMessage").hide();
        $("#deactivateMessage").hide();
        $("#inUseMessage").show();
        $("#userName").text(fullname + ": " + userName);

    } else
        if (activate) {
            $("#activateMessage").show();
            $("#deactivateMessage").hide();
            $("#inUseMessage").hide();
            $("#userName").text("User: " + fullname);

        } else
            if (!activate) {
                $("#activateMessage").hide();
                $("#deactivateMessage").show();
                $("#inUseMessage").hide();
                $("#userName").text("User: " + fullname);

            }

    $("#popupActivateOrDeactivate").show();
    $("#popupActivateOrDeactivate").css("visibility", "visible");
    $(".overlay").show();
}