$(document).ready(function () {
    $("#NonAccountAccessGrid").kendoGrid({

        dataSource: {
            transport: {
                read: {
                    url: "/TeamMember/Access/GetNonAccountAccess?contactId=" + $("#contactId").val(),
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
        height: 335,
        filterable: true,
        sortable: true,
        columns: [{ field: "AccountId", hidden: true},
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
            template: "<div class='check'><input type='checkbox' #= AccessType ? \'checked=\"checked\"\' : \"\" # class='chkbx2' id='#=AccountId#'/>" +
              "</div>",
            width: 89
        }]
        
    });
    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#NonAccountAccessGrid").data("kendoGrid");

        if (value) {
            grid.dataSource.query({
                filter: {
                    logic: "or",
                    filters: [
                        { field: "AccountName", operator: "contains", value: value },
                        { field: "ShortName", operator: "contains", value: value }
                    ]
                }
            });
        } else {
            grid.dataSource.filter({});
        }
    });


    $("#NonAccountAccessGrid .k-grid-content").on("change", "input.chkbx2", function (e) {
        var grid = $("#NonAccountAccessGrid").data("kendoGrid");
  
        var primary = $("#grantAccountIds").val();

        if (this.checked == true) {
            $("#grantAccountIds").val(primary + e.target.id + ";");
        }
        else {
            var index = $("#grantAccountIds").val().indexOf(e.target.id);
            if (index != -1) {
                $("#grantAccountIds").val(primary.substring(0, index) + primary.substring(index + e.target.id.toString().length + 1));
            }
        }
    });
});


function grant() {

    if ($("#grantAccountIds").val() == "") {
        alert("empty")
    } else {
        $.ajax({
            url: "/TeamMember/Access/GrantAccess",
            type: "get", //send it through get method
            data: {
                contactId :$("#contactId").val(),
                accountIds: $("#grantAccountIds").val()
            },
            success: function (response) {
                $("#grantAccountIds").val("");
                $('#NonAccountAccessGrid').data('kendoGrid').dataSource.read();
                $('#AccountAccessGrid').data('kendoGrid').dataSource.read();
            },
            error: function (xhr) {
                alert("error");
                console.log("error");
            }
        });
    }
}


