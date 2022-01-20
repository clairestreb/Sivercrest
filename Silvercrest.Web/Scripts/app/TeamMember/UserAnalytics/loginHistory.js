$(document).ready(function () {
    var webUserId = $("#webUserId").val();
    var loginHistoryGrid = $("#loginHistoryGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "/TeamMember/UserAnalytics/GetLoginHistory?webUserId=" + webUserId,
                    dataType: "json"
                }
            },
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },           
        },
        sortable: false,
        height: 335,
        columns: [{
            field: "LoginDate",
            title: "login date",
            width: 150,
            headerAttributes: { style: "cursor: default !important;" },
        },
        {
            field: "LogoutDate",
            title: "logout date",
            width: 150,
            headerAttributes: { style: "cursor: default !important;" },
    },
        {
            field: "TimeOnline",
            title: "time spent",
            width: 180,
            headerAttributes: { style: "cursor: default !important;" },
        }]
    }).data("kendoGrid");
});