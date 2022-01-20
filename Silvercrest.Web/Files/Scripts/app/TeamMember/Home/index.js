$(document).ready(function () {
    $.tablesorter.addParser({
        id: 'currency1',
        is: function (s) {
            return false;
        },
        format: function (s) {
            return s.replace('$', '').replace(/,/g, '').replace('(','-').replace(')', '');
        },
        type: 'numeric'
    });
    for (var i = 1; i <= $("#hidden").val(); i++) {
        $("#tableManager" + i).tablesorter({
            sortList: [[0,0]],
            headers: {
                1: {
                    sorter: 'currency1'
                }
            }
        });
    }
    
}
);
function checkSort() {
    var target = this;
    //document.getElementById("").te
    if (!$(target).is("th")) {
        target = $(target).parent();
    }
    if (!$(target).is("tr")) {
        target = $(target).parent();
    }
    $(target).children('th').children('span').css("display", "none");
}
 