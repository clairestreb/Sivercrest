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
                model: {
                    fields: {
                        UserName: {
                            type: "string"
                        },
                        LastLogin: {
                            type: "date"
                        },
                        LoginFrequency: {
                            type: "number"
                        },
                        LoggedInTime: {
                            type: "number"
                        }
                    }
                }
            }
        },
        dataBound: function onDataBound(e) {
            var rows = e.sender.tbody.children();
            sort();
        },
        height: 335,
        filterable: true,
        sortable: {
            allowUnsort: false, mode: "single"
        },
        columns: [{
            field: "UserName",
            title: "contact name",
            attributes: {
                style: "font-family:copperplate"
            },

            template: "<a href=\"/TeamMember/UserAnalytics?contactIdQuery=#=ContactIdQuery#&familyIdQuery=#=FamilyIdQuery#&firmUserGroupIdQuery=#=FirmUserGroupIdQuery#&isFromQuery=#=IsFromQuery#\">#=UserName#</a>",
            width: 250,
            sortable: false
        },
        {
            field: "LastLogin",
            title: "last login",
            attributes: {
                "align": "center",
                style: "font-family:goudyosSC"
            },
            template: "#= kendo.toString(kendo.parseDate(LastLogin, 'yyyy-MM-dd'), 'MM/dd/yyyy HH:mm') #",
            width: 200
        },
        {
            field: "LoginFrequency",
            title: "login frequency",
            attributes: {
                "align": "center",
                style: "font-family:goudyosSC"
            },
            template: "#=getFrequency(LoginFrequency)#",
            width: 150
        },
        {
            field: "LoggedInTime",
            title: "average time spent",
            attributes: {
                "align": "center",
                style: "font-family:goudyosSC"
            },
            template: "#=getAverageTime(LoggedInTime)#",
            width: 150
        }
        ]

    }).data("kendoGrid");

    function sort() {

        $("th:has(a.k-link)").css("text-align", "center");
        $("th:has(a.k-link)").css("vertical-align", "top");
        $("a.k-link").css("color", "#fff");

        $("th:has(a.k-link)").click(onClickSort);
        $(".k-link").click(onClickSort);


        $('<span style="display:none">&nbsp;</span>').appendTo("th:has(a.k-link)");
        $('<span class="arrow-down" style="transform: rotate(180deg);display:none"></span>').appendTo("th:has(a.k-link)");
        $('<span class="arrow-down" style="display:none"></span>').appendTo("th:has(a.k-link)");


        $("a.k-link").removeClass("k-link");
    }

    function onClickSort(e) {
        var target = $(this);
        // in case if it's redirect from children
        if (!$(target).is("th")) {
            target = $(target).parent();
        }

        $(target.closest('tr')).children('th').children('span').css("display", "none");
        var set = $(target).children();
        var dir = $(target).attr("data-dir");

        set.each(function (index, elem) {

            if (index > 1) {
                $(this).css("display", "none");
            }
            if (dir === "asc") {
                set.eq(3).css({ "display": "block", "border-top": "4px solid #fff" });

            }
            if (dir === "desc") {
                set.eq(4).css({ "display": "block", "border-top": "4px solid #fff" });

            }
            if (dir !== "asc" && dir !== "desc") {
                set.eq(1).css({ "display": "block" });

            }

        });

    }

});

function getLastLogin(value) {
    var dt = Date.parse(value);
    var oldDate = new Date(1970, 1, 1);
    /*
        if (dt < oldDate) {
            return "";
        }
    */
    return dt.formatDate('MM/dd/yyyy HH:mm');
}



function getFrequency(value) {
    if (value == null) {
        return "";
    }

    return value > 1 ? Math.round(value) + " day(s)" : Math.round(value * 24) + " hour(s)";
}

function getAverageTime(value) {
    if (value == null) {
        return "";
    }

    return value >= 60 ? (value / 60).toFixed(1) + " hour(s)" : value + " minute(s)"
}

Date.prototype.formatDate = function (format) {
    var date = this;
    if (!format)
        format = "MM/dd/yyyy";

    var month = date.getMonth() + 1;
    var year = date.getFullYear();

    format = format.replace("MM", month.toString().padL(2, "0"));

    if (format.indexOf("yyyy") > -1)
        format = format.replace("yyyy", year.toString());
    else if (format.indexOf("yy") > -1)
        format = format.replace("yy", year.toString().substr(2, 2));

    format = format.replace("dd", date.getDate().toString().padL(2, "0"));

    var hours = date.getHours();
    if (format.indexOf("t") > -1) {
        if (hours > 11)
            format = format.replace("t", "pm")
        else
            format = format.replace("t", "am")
    }
    if (format.indexOf("HH") > -1)
        format = format.replace("HH", hours.toString().padL(2, "0"));
    if (format.indexOf("hh") > -1) {
        if (hours > 12) hours - 12;
        if (hours == 0) hours = 12;
        format = format.replace("hh", hours.toString().padL(2, "0"));
    }
    if (format.indexOf("mm") > -1)
        format = format.replace("mm", date.getMinutes().toString().padL(2, "0"));
    if (format.indexOf("ss") > -1)
        format = format.replace("ss", date.getSeconds().toString().padL(2, "0"));
    return format;
}