$(document).ready(function () {
    var userGrid = $("#NicknamesGrid").kendoGrid({
        dataSource: {
            data: model.TableData.Data,
            groupable: true,
            schema: {
                model: { id: "Id" }
            }
        },
        dataBound: function onDataBound(arg) {
            $(".k-group-col,.k-group-cell").remove();
            var spanCells = $(".k-grouping-row").children("td");
            spanCells.attr("colspan", spanCells.attr("colspan") - 1);
            $(".k-footer-template").remove();
            $(".k-group-footer").last().remove();
            var rows = arg.sender.tbody.children();
            for (var j = 0; j < rows.length; j++) {
                var row = $(rows[j]);
                for (var i = 1; i < row[0].childNodes.length-2; i++) {
                    $(row[0].childNodes[i]).css("text-align", "center");
                }
                row.addClass("row red link tab");
            }
            sort();
        },
        //height: 435,
        filterable: true,
        sortable: false, /*{
            mode: "single",
            allowUnsort: false
        },*/
        columns: [{
            field: "Name",
            title: "account name",
            headerAttributes: { style: "text-align: center; cursor:default;" },
            width: 320
        },
        {
            field: "Nickname",
            title: "nickname",
            //template: kendo.template($("#name-template").html()),
            template: "<input type=\"text\" maxlength=\"75\" size=\"40\" value=\"#=Nickname == null || Nickname == \"\" ? \"\" : Nickname#\">",
			attributes: { style: "font-family:goudyosSC" },
			headerAttributes: { style: "text-align: center; cursor:default;" },
            width: 200
        },
        {
            field: "",
            title: "",
            template: kendo.template($("#button-template").html()),
            headerAttributes: { style: "text-align: center; cursor:default;" },
            width: 80
        },
        {
            field: "ContactId",
            hidden: "true",
            template: "<span id=contactId>#=ContactId#</span>"
        },
        {
            field: "AccountId",
            hidden: "true",
            template: "<span id=accountId>#=AccountId#</span>"
        }
        ]
    }).data("kendoGrid");
    function sort() {

        $("th:has(a.k-link)").css("text-align", "center");
        $("th:has(a.k-link)").css("vertical-align", "top");
        $("a.k-link").css("color", "#163574");

        $("th:has(a.k-link)").click(onClickSort);
        $(".k-link").click(onClickSort);


        $('<span style="display:none">&nbsp;</span>').appendTo("th:has(a.k-link)");
        $('<span class="arrow-down" style="transform: rotate(180deg);display:none"></span>').appendTo("th:has(a.k-link)");
        $('<span class="arrow-down" style="display:none"></span>').appendTo("th:has(a.k-link)");
        $("a.k-link").removeClass("k-link");
    }

    function onClickSort(e) {

        var target = $(this);
        //in case if it's redirect from children
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
            if (dir == "asc") {
                set.eq(3).css("display", "block");

            }
            if (dir == "desc") {
                set.eq(4).css("display", "block");

            }
            if (dir != "asc" && dir != "desc") {
                set.eq(2).css("display", "block");

            }

        });
    }

});

function submitNickname(e){
    var target = $(e);
    var row = target.closest("tr");
    var contactId = $(row.find("#contactId")).text();
    var accountId = $(row.find("#accountId")).text();
    var input = row.find("input[type=text]");
    var text = $(input).val().trim();

    if (text.length != 0 && (text.length > 75 || text.length < 3)) {
        alertBound();
        return;
    }
    if (text.length == 0) {
        $.ajax({
            url: '/Client/Home/DeleteNickname?contactId=' + contactId + "&accountId=" + accountId,
            type: 'GET',
            dataType: "json",
            success: function (result) {
                location.reload();
            }
        });
    }
    if (text.length >= 3) {
        $.ajax({
            url: '/Client/Home/AddNickname?contactId=' + contactId + "&accountId=" + accountId + "&nickname=" + text,
            type: 'GET',
            dataType: "json",
            success: function (result) {
                location.reload();
            }
        });
    }
}

function alertBound() {
    $("#modalHeader").text("nickname length");
    $("#modalBody").text("Nickname length must be between 3 and 75 characters");
    $('#myModal').modal('show');
}
