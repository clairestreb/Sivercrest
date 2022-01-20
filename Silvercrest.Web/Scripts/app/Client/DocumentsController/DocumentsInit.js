$(document).ready(function () {
    var url = "/Client/Documents/GetDocuments?contactId=" + getUrlParam("contactId");
    initDatepickers(url);
    
    // To put arrow below text when page Loads
    var asOfColumn = $("documentsTable .k-grid-header tr th[data-field='AsOf']");
    //var sortedColumn = $("#documentsTable .k-grid-header tr th[data-field='UploadDate']");
    $('<span class="arrow-down" style="display:block></span>"').appendTo(asOfColumn);
    //$('<span class="arrow-down" style="display:block"></span>').appendTo(sortedColumn);
    var grid = $('#documentsTable').getKendoGrid();

    //var entity = localStorage["selected-filter-entity"];
    //var docType =localStorage["selected-filter-docType"];
    var entity = sessionStorage.getItem("selected-filter-entity");
    var docType = sessionStorage.getItem("selected-filter-docType");

    sortByEntityName(entity);
    sortByDocumentType(docType);

    //var options = localStorage["kendo-grid-options"];
    var options = sessionStorage.getItem("kendo-grid-options");
    if (options) {
        grid.setOptions(JSON.parse(options));
    }
});

function initDatepickers(url) {
    //var now = new Date();
    //actualDate = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds());
    //var firstDate = new Date(actualDate.setFullYear(actualDate.getFullYear() - 1));
    //var secondDate = new Date(actualDate.setFullYear(actualDate.getFullYear() + 1));
    $("#datepickerStart").datepicker().inputmask("mm/dd/yyyy");
    //$('#datepickerStart').datepicker({ dateFormat: 'mm/dd/yyyy' });
    //$("#datepickerStart").val(getDateString(firstDate));
    $("#datepickerEnd").datepicker().inputmask("mm/dd/yyyy");
    //$('#datepickerEnd').datepicker({ dateFormat: 'mm/dd/yyyy' });
    //$("#datepickerEnd").val(getDateString(secondDate));

    $("#documentsTable").kendoGrid({
        dataSource: {
            data: model.Documents,
            schema: {
                model: {
                    fields: {
                        //AsOf: {
                        //    type: "date"
                        //},
                        //UploadDate: {
                        //    type: "date"
                        //},
                    }
                }
            },
            //sort: [
            //    { field: "AsOf", dir: "desc" },
            //    { field: "UploadDate", dir: "desc" }
            //]
        },
        serverSorting: false,
        pageSize: 20,
        serverFiltering: false,
        serverPaging: false,
        sortable: {
            mode: "single",
            allowUnsort: false
        },
        dataBound: function grid_dataBound(e) {
            sort();
            var rows = e.sender.tbody.children();
            for (var j = 0; j < rows.length; j++) {
                var row = $(rows[j]);
                row.addClass("row red link tab");
            }
        },
        columns: [
            {
                field: "FileName",
                title: "FILE NAME",
                attributes: {
                    "class": "col1"
                },
                template: '<a href="/Client/Documents/DownloadFile?fileId=#=Id#&asOf=#= kendo.toString(kendo.parseDate(AsOf, \'MM/dd/yyyy\'), \'MM-dd-yyyy\') #">#=FileName#</a>',
                //template: '<a href="/Client/Documents/DownloadFile?fileId=#=Id#&asOf=#= kendo.toString(kendo.parseDate(AsOf, \'MM-dd-yyyy\'), \'MM-dd-yyyy\') #">#=FileName#</a>',
                width: 350
            },
            {
                field: "AsOfSort",
                //field: "AsOf",
                //field: "UploadDate",
                title: "AS OF",
                format: "{0:MM/dd/yyyy}",
                //format: "{0:MM/dd/yyyy HH:mm}",
                template: "#= kendo.toString(AsOf) #",
                //template: "#= AsOf #",
                width: 100,
                attributes: {
                    "class": "col2"
                }
            },
            //{
            //    field: "UploadDate",
            //    title: "UPLOAD TIME",
            //    format: "{0:MM/dd/yyyy HH:mm}",
            //template: "#= kendo.toString(kendo.parseDate(UploadDate, 'MM-dd-yyyy'), 'MM/dd/yyyy') #",
            //    width: 100,
            //    attributes: {
            //        "class": "col2"
            //    }
            //},
            {
                field: "DocumentType",
                title: "DOCUMENT TYPE",
                width: 150,
                attributes: {
                    "class": "col3"
                }
            },
            {
                field: "IsFavorite",
                title: " ",
                width: 50,
                template: '# if (model.HasPermission && IsStrategy == false && IsSamg == false) {# <a href="" style="display: inline" onclick="showConfirm(\'#= Id #\'); return false;"><i class="fa fa-trash"></i></a> #}#' +
                '<a style="display: inline;" href="" onclick="setFavorite(\'#= Id #\', #= IsStrategy#, #= IsFavorite #, #= IsSamg #);"><i class="fa fa-star#=!IsFavorite ? "-o" : ""#"></i></a>',
                attributes: {
                    style: "text-align: center;"
                },
                sortable: false
            },
        ],
    });
    var grid = $("#documentsTable").data("kendoGrid");
    //options = localStorage["kendo-grid-options"];
    var options = sessionStorage.getItem("kendo-grid-options");
    if (options) {
        grid.setOptions(JSON.parse(options));
    }
}
///Client/Documents/MakeDocumentFavorite?fileId=#=Id#&isFavorite=#=!IsFavorite#&contactIdQuery=#=getUrlParam("contactIdQuery")#
//sort();

function grid_dataBound(e) {
    var rows = e.sender.tbody.children();
    for (var j = 0; j < rows.length; j++) {
        var row = $(rows[j]);
        row.addClass("row red link tab");
    }
}

function sort() {
    $("th:has(a.k-link)").css("text-align", "center");
    $("th:has(a.k-link)").css("vertical-align", "top");
    $("a.k-link").css("color", "#163574");

    $("th:has(a.k-link)").bind("click", function (e) {

    });
      
    $("th:has(a.k-link)").click(onClickDocumentsSort);
    $(".k-link").click(onClickDocumentsSort);

    $('<span style="display:none">&nbsp;</span>').appendTo("th:has(a.k-link)");
    $('<span class="arrow-down" style="transform: rotate(180deg);display:none"></span>').appendTo("th:has(a.k-link)");
    $('<span class="arrow-down" style="display:none"></span>').appendTo("th:has(a.k-link)");

    $("a.k-link").removeClass("k-link");
}

function onClickDocumentsSort(e) {
    var target = $(this);
    // in case if it's redirect from children
    if (!$(target).is("th")) {
        target = $(target).parent();
    }

    $(target.closest('tr')).children('th').children('span').css("display", "none");
    var set = $(target).children();
    var dir = $(target).attr("data-dir");

    set.each(function (index, elem) {

        if (index > 0) {
            $(this).css("display", "none");
        }
        if (dir == "asc") {
            set.eq(2).css("display", "block");
        }
        if (dir == "desc") {
            set.eq(3).css("display", "block");
        }
        if (dir != "asc" && dir != "desc") {
            set.eq(1).css("display", "block");
        }
        var grid = $("#documentsTable").data("kendoGrid");

        //localStorage["kendo-grid-options"] = kendo.stringify(grid.getOptions());
        sessionStorage.setItem("kendo-grid-options", kendo.stringify(grid.getOptions()));
    });

    $("span.k-icon").remove();
}
//function onClickDocumentsSort(e) {
//    var target = $(this);
//    // in case if it's redirect from children
//    if (!$(target).is("th")) {
//        target = $(target).parent();
//    }

//    $(target.closest('tr')).children('th').children('span').css("display", "none");
//    var set = $(target).children();
//    var dir = $(target).attr("data-dir");

//    set.each(function (index, elem) {

//        if (index > 0) {
//            $(this).css("display", "none");
//        }
//        if (dir == "asc") {
//            set.eq(2).css("display", "block");
//        }
//        if (dir == "desc") {
//            set.eq(3).css("display", "block");
//        }
//        if (dir != "asc" && dir != "desc") {
//            set.eq(1).css("display", "block");
//        }

//    });
//    $("span.k-icon").remove();
//}

function getDateString(date) {
    var result = "";
    result += date.getUTCMonth();
    result += "/";
    result += date.getUTCDate();
    result += "/";
    result += date.getUTCFullYear();
    return result;
}

function isValidDate(date) {

    var isValid = false;
    if (date.match(/\d{1,2}[^\d]\d{1,2}[^\d]\d{4,4}/gi) == null) {
        isValid = false;
    }
    else {
        var t = date.split(/[^\d]/);
        var m0 = parseInt(t[0], 10) - 1;
        var dd = parseInt(t[1], 10);
        var yyyy = parseInt(t[2], 10);
        var d = new Date(yyyy, m0, dd);
        if (d.getDate() != dd || d.getMonth() != m0 || d.getFullYear() != yyyy) {
            
            isValid = false;
        }
        else {
            isValid = true;
        }
    }

    return isValid;
}

function insertDates() {

    var searchFrom = $('#datepickerStart').val();
    var searchTo = $('#datepickerEnd').val();

    if (isValidDate(searchFrom) && isValidDate(searchTo)) {
        var url = "/Client/Documents/GetDocuments?searchFrom=" + searchFrom + '&searchTo=' + searchTo + "&contactIdQuery=" + getUrlParam("contactIdQuery");
        location.href = url;
    }
    else {
        showPopup("Invalid date.", true, "Error", true);
    }
}

var entity = "All";
var docType = "All";


function sortByDocumentType(item) {
    //debugger;
    if (item == undefined) {
        item = "All";
    }
    item = item.replace("\_", " ");
    var grid = $('#documentsTable').getKendoGrid();
    docType = item;
    if (docType != "All") {
        if (docType === 'Favorites') {
            var data = model.Documents.filter(function (value) {
                if (entity != "All") {
                    return (value.EntityName === entity && value.IsFavorite === true);
                }
                else
                    return (value.IsFavorite === true);
            })
            grid.dataSource.data(data);
            grid.refresh();
        }
        else {
            var data = model.Documents.filter(function (value) {
                if (entity != "All")
                    return (value.EntityName === entity && value.DocumentType === docType);
                else
                    return (value.DocumentType === docType);
            })
            grid.dataSource.data(data);
            grid.refresh();
        }
    }

    else if (entity != "All")
        sortByEntityName(entity);
    else {
        grid.dataSource.data(model.Documents);
        grid.refresh();
    }

    //var checkedElement = $("ul#DocumentTypeUl a.linkSelected");
    //checkedElement[0].className = "red";

    item = item.replace(" ", "\_");

    var documentTypeElement = $("ul#DocumentTypeUl a#" + item);
    documentTypeElement[0].className = "red linkSelected";

    //localStorage["kendo-grid-options"] = kendo.stringify(grid.getOptions());
    //localStorage.setItem('selected-filter-docType', item);
    sessionStorage.setItem("kendo-grid-options", kendo.stringify(grid.getOptions()));
    sessionStorage.setItem("selected-filter-docType", item);
}

function sortByEntityName(item) {

    if (item == undefined) {
        item = "All";
    }

    entity = item;
    var grid = $('#documentsTable').getKendoGrid();
    $("#dropDownMenu2Text").text(item);
    if (entity != "All") {
        var data = model.Documents.filter(function (value) {

            if (docType != "All") {
                if (docType === 'Favorites') {
                    return (value.EntityName === entity && value.IsFavorite === true);
                }
                return (value.EntityName === entity && value.DocumentType === docType);
            }
            else
                return value.EntityName === entity;
        })
        grid.dataSource.data(data);
        grid.refresh();
    }
    else if (docType != "All")
        sortByDocumentType(docType);
    else {
        grid.dataSource.data(model.Documents);
        grid.refresh();
    }

    //localStorage.setItem('selected-filter-entity', item);
    sessionStorage.setItem("selected-filter-entity", item);
}

$('.sideBorder ul.links li a').on('click', function () {
    $('li a.linkSelected').removeClass('linkSelected');
    $(this).addClass('linkSelected');
});

function downloadFile(fileId) {
    $.ajax({
        url: '/Client/Documents/DownloadFile?fileId=' + fileId + '&contactId=' + getUrlParam("contactId"),
        type: 'POST',
        dataType: "json",
        success: function (result) {
            alert("!!");
        },
        error: function (err) {
            alert(err);
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

    if (pageReload == false) {
        $("#popupActivateOrDeactivate button").on('click', closeResultMsgPopup);
    }
    else {
        $("#popupActivateOrDeactivate button").on('click', refresh);
    }
    $("#popupActivateOrDeactivate").show();
    $("#popupActivateOrDeactivate").css("visibility", "visible");
    $(".overlay").show();
}


function showConfirm(fileId) {
    $("#confirmBodyMessage").text("Are you sure you want to delete this document ?");
    $("#popupDeleteConfirm").css("margin-top", "270px");

    $("#popupDeleteConfirm button.cancel").on('click', closeConfirmPopup);
    $("#popupDeleteConfirm button.ok").on('click', function () {
        $.ajax({
            url: '/Client/Documents/RemoveDocument?fileId=' + fileId,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                //showPopup("Document has been deleted!", true, "Success", true);
                location.reload();
            },
            error: function (res) {
            }
        })
    });

    $("#popupDeleteConfirm").show();
    $("#popupDeleteConfirm").css("visibility", "visible");
    $(".overlay").show();
}

function setFavorite(fileId, isStrategy, isFavorite, isSamg) {
    $.ajax({
        url: '/Client/Documents/MakeDocumentFavorite?fileId=' + fileId + '&isStrategy=' + isStrategy + '&isFavorite=' + !isFavorite +'&isSamg=' + isSamg + '&contactIdQuery=' + getUrlParam("contactIdQuery"),
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            //localStorage["kendo-grid-options"] = kendo.stringify(grid.getOptions());
            //localStorage.setItem('selected-filter', item);
            sessionStorage.setItem("kendo-grid-options", kendo.stringify(grid.getOptions()));
        },
        error: function (res) {

        }
    });
}

function refresh() {
    location.reload();
}

function closeConfirmPopup() {
    $("#popupDeleteConfirm").hide();
    $("#popupDeleteConfirm").css("visibility", "hidden");
    $(".overlay").hide();
}

function closeResultMsgPopup() {
    $("#popupActivateOrDeactivate").hide();
    $("#popupActivateOrDeactivate").css("visibility", "hidden");
    $(".overlay").hide();
}