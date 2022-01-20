$(document).ready(function () {
    var userGrid = $("#ContactGrid").kendoGrid({
        dataSource: {
            data: model.TableData.Data,
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
        dataBound: function onDataBound(e) {
            var rows = e.sender.tbody.children();
            for (var j = 0; j < rows.length; j++) {
                var row = $(rows[j]);
                var dataItem = e.sender.dataItem(row);
                var isWebUser = dataItem.get("IsWebUser");
                if (isWebUser == false) {
                    row.css("background-color", "darkgray")
                }
            }
        },
        height: 335,
        filterable: true,
        sortable: false,
        columns: [{
            field: "DisplayName",
            title: "contact name",
            template: "<a href=\"/TeamMember/Access?contactIdQuery=#=ContactIdQuery#&familyIdQuery=#=FamilyIdQuery#&firmUserGroupIdQuery=#=FirmUserGroupIdQuery#\">#=DisplayName#</a>",
            headerAttributes: { style: "cursor: default !important;" },
            width: 150
        },
        {
            field: "Email",
            title: "email",
            headerAttributes: { style: "cursor: default !important;" },
            width: 250
        },
        {
            field:"ContactId",
            title: "analytics",
            template: "<a href=\"/TeamMember/UserAnalytics?contactIdQuery=#=ContactIdQuery#&familyIdQuery=#=FamilyIdQuery#&firmUserGroupIdQuery=#=FirmUserGroupIdQuery#\">View Analytics</a>",
            headerAttributes: { style: "cursor: default !important;" },
            width: 150
        },
        {
            field: "ContactId",
            title: "client view",
            template: "<a href=\"/Client/Home?contactIdQuery=#=ContactIdQuery#\" target=\"_blank\">Client View</a>",
            headerAttributes: { style: "cursor: default !important;" },
            width: 150
        }
        ]

    }).data("kendoGrid");
    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();

        if (value.length != 0 && value.length < 2) {
            return;
        }

        grid = $("#ContactGrid").data("kendoGrid");

        if (value) {
            grid.dataSource.query({
                filter: {
                    logic: "or",
                    filters: [
                    { field: "DisplayName", operator: "contains", value: value },
                    { field: "Email", operator: "contains", value: value }
                    ]
                }
            });
        } else {
            grid.dataSource.filter({});
        }
    });

    $('#FieldFilter').focus();
});
