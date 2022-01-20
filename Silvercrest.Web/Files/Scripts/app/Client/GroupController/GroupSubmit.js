
function UpdateGroup() {
    var active = "";
    var notactive = "";
    var groupName = "";

/*    
    $("#selector span.xtoggle").not(".active").each(function (e, i, k) {
        if( $(this).parent().attr("hidden") != "hidden")
            notactive += $(this).attr("id") + ";";
    });
 */   
    $("#selector span.xtoggle.temp-active").each(function (e, i, k) {
        if( $(this).parent().attr("hidden")!="hidden")
            active += $(this).attr("id") + ";";
    });

    groupName = $("#editGroupName").val().trim();
    var groupId = $("#Groups").val().split('_')[1];

    var allGroups = $("#AllGroups").val();
    if (allGroups.length > 0) {
        var pairs = allGroups.split(';');
        for (var i = 0; i < pairs.length; i++) {
            if ((pairs[i].split(':')[1].toUpperCase() == groupName.toUpperCase()) && (pairs[i].split(':')[0] != groupId))  //dupe name but not same name as currently selected
            {
                showPopup("\"" + groupName + "\" already exists.", true, "Duplicate Name", false);
                return;
            }
        }
    }

    if (groupName.toUpperCase() == "ALL ACCOUNTS")
    {
        showPopup("\"All Accounts\" is a restricted name and cannot be reused.", true, "Restricted Name", false);
        return;
    }

    if (groupName.length <= 0) {
        showPopup("Group Name must be provided.", true, "Missing Name", false); return;
    }
    if (groupName.length > 100) {
        showPopup("Group Name cannot be longer than 100 characters.", true, "Group Name Length", false); return;
    }

    if (active.length == 0) {
        showPopup("Group must contain at least one account.", true, "Missing Accounts", false);

        return;
    }

    $.ajaxSetup({ cache: false });
   $.ajax({
        url: "/Client/Group/UpdateGroup",
        type: "post",
        data: {       
            contactId: $("#ContactId").val(),
            groupName: groupName,
            accountIds: active,
            changerName: $("#FullName").val(),
            accountGroupId: groupId
//            oldName: $("div.pad0 a.active").text()
        },
        success: function (response) {
            showPopup("\"" +groupName + "\" has been updated!", true, "Success", true); return;
//            window.location.reload(true);
        },
        error: function (res) {
        }
   });
}

function CreateGroup() {
    var groupName = $("#groupName").val().trim();

    var allGroups = $("#AllGroups").val();
    if (allGroups.length > 0) {
        var pairs = allGroups.split(';');
        for (var i = 0; i < pairs.length; i++) {
            if (pairs[i].split(':')[1].toUpperCase() == groupName.toUpperCase()) {
                showPopup("\"" + groupName + "\" already exists.", false, "Duplicate Name", false);
                return;
            }
        }
    }

    if (groupName.toUpperCase() == "ALL ACCOUNTS") {
        showPopup("\"All Accounts\" is a restricted name and cannot be reused.", false, "Restricted Name", false);
        return;
    }

    if (groupName.length <= 0) {
        showPopup("Group Name must be provided.", false, "Missing Name", false);
        return;
    }
    if (groupName.length > 100) {
        showPopup("Group Name cannot be longer than 100 characters.", false, "Group Name Length", false);

        return;
    }

    var active = "";

    $("#selector2 span.xtoggle.active").each(function (e, i, k) {
        active += $(this).attr("id") + ";";
    });

    if (active.length == 0) {
        showPopup("Group must contain at least one account.", false, "Missing Accounts", false);

        return;
    }

    $.ajax({
        url: "/Client/Group/CreateGroup",
        type: "post",
        data: {
            contactId: $("#ContactId").val(),
            groupName: groupName,
            accountIds: active,
            changerName: $("#FullName").val()
        },
        success: function (response) {
            $("#Groups").val(groupName);
            showPopup("\"" + groupName + "\" has been created!", false, "Success", true);
        },
        error: function (res) {
        }
    });



}


function DeleteGroup() {
    var groupId = $("#Groups").val().split('_')[1];

    var existingName = "";

    var allGroups = $("#AllGroups").val();
    var pairs = allGroups.split(';');
    for (var i = 0; i < pairs.length; i++) {
        if (pairs[i].split(':')[0] == groupId) {
            existingName = pairs[i].split(':')[1]
        }
    }


    $.ajax({
        url: "/Client/Group/DeleteGroup",
        type: "post",
        data: {
            contactId: $("#ContactId").val(),
            accountGroupId: groupId
        },
        success: function (response) {
            showPopup("\"" + existingName + "\" has been deleted!", true, "Success", true);
        },
        error: function (res) {
        }
    });

}

function Selector(group) {
    $("span.xtoggle").removeClass("temp-active");

    var groupName = $(group).text();
    var groupId = $(group).attr('id').split('_')[1];

    $("#editGroupName").val(groupName);  

    $("a").removeClass("active");
    $(group).addClass("active");
    
    $("#Groups").val("AcctGrp_" + groupId);

    //Adding temp-active to all actives so that they show up with x mark
    $("span.xtoggle.active").addClass("temp-active");

    $("li[hidden='hidden']").each(function () {
        $(this).removeAttr("hidden");
    });

    $("#selector li").not(":has(span#" + $(group).attr('id') + ")").attr("hidden", "true");
   
    
}

function FilterNames(e) {

     $("#selector li[hidden='hidden']").each(function () {
        $(this).removeAttr("hidden");
    });

     $("#selector li").not(":has(span#" + $("#Groups").val().split(' ').join('') + ")").attr("hidden", "true");

     $("#selector li").not(":has(span:contains('" + $(e).val() + "'))").attr("hidden", "true");


    

}

$(document).ready(function ()
{
    $(".btn-activate").click(function () {
        $("#popupActivateOrDeactivate").hide();
        $("#popupActivateOrDeactivate").css("visibility", "hidden");
        $(".overlay").hide();

        $("#popupDeleteConfirm").hide();
        $("#popupDeleteConfirm").css("visibility", "hidden");
        $(".overlay").hide();
    });

    $("#groupName").val("");

    var elem = $("div.pad0 a").first();

    if ($("#Groups").val().length > 0)
        Selector($(elem).get(0));

});

function showPopup(message, topPosition, messageType, pageReload) {
    $("#errorMessage").text(message);
    $("#activateMessage").text(messageType);

    if (topPosition == true)
        $("#popupActivateOrDeactivate").css("margin-top", "270px");
    if (topPosition == false)
        $("#popupActivateOrDeactivate").css("margin-top", "610px");

    if (pageReload == false)
    {
        $("#popupActivateOrDeactivate button").on('click', doNothing);
    }
    else
    {
        $("#popupActivateOrDeactivate button").on('click', refresh);
    }

    $("#popupActivateOrDeactivate").show();
    $("#popupActivateOrDeactivate").css("visibility", "visible");
    $(".overlay").show();
}

function showConfirm() 
{
    var existingName = $("div.pad0 a.active").text();

    $("#confirmBodyMessage").text("Are you sure you want to delete \"" + existingName + "\"?");

    $("#popupDeleteConfirm").css("margin-top", "270px");

    $("#popupDeleteConfirm button.cancel").on('click', doNothing);
    $("#popupDeleteConfirm button.ok").on('click', DeleteGroup);

    $("#popupDeleteConfirm").show();
    $("#popupDeleteConfirm").css("visibility", "visible");
    $(".overlay").show();
}



function refresh() {
    location.reload();
}

function doNothing() {
}