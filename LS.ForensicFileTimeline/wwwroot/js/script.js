$(document).ready(function () {
    const topPager = $('<div id="topPager" class="k-pager-wrap topPager" />').insertAfter("#Grid1 > div.k-header.k-grid-toolbar > .k-grid-search");
    grid.topPager = new kendo.ui.Pager(topPager,
        $.extend({},
            grid.options.pageable,
            {
                dataSource: grid.dataSource,
                previousNext: false,
                pageSizes: false,
                numeric: false,
                info: true,
            })
    );

    $(".k-pager-info").last().css("display", "");
    var grid = $("#Grid1").data("kendoGrid");
    var dataSource = grid.dataSource;

    //records on current view / page
    var recordsOnCurrentView = dataSource.view().length;
    //total records
    var totalRecords = dataSource.total();

    $(".k-pager-refresh").on("click", function () {
        var grid = $("#Grid1").data("kendoGrid");

        if (grid) {
            grid.dataSource.filter({});
        }
        return false;
    });

    $("#applyFilters").on("click", function () {
        filterGrid();
        return false;
    });

    $("#clearFilters").on("click", function () {
        document.getElementById("filters").reset();
        $("#selectColor span").removeClass("k-state-active");
        var grid = $("#Grid1").data("kendoGrid");

        if (grid) {
            grid.dataSource.filter({});
            //grid.dataSource.read();
        }

        return false;
    });
});

$("#eq2").on("change", function () {
    var grid = $("#Grid1").data("kendoGrid");
    if (!$('#eq2').is(":checked")) {
        grid.setOptions({
            editable: { destroy: false, create: false, update: false }
        });
        $(".k-grid-delete").css("display", "none");
    } else {
        grid.setOptions({
            editable: { destroy: true, create: true, update: true }
        });
        $(".k-grid-delete").css("display", "");
    }
    return false;
});

$("#eq3").on("change", function () {
    var grid = $("#Grid1").data("kendoGrid");
    var value = false;
    if ($('#eq3').is(":checked")) {
        value = true;
    }
    grid.setOptions({
        filterable: value
    });
    return false;
});

$("#eq4").on("change", function () {
    var grid = $("#Grid1").data("kendoGrid");
    var value = false;
    if ($('#eq4').is(":checked")) {
        value = true;
    }
    grid.setOptions({
        groupable: value
    });
    return false;
});

$("#eq5").on("change", function () {
    var grid = $("#Grid1").data("kendoGrid");
    if ($('#eq5').is(":checked")) {
        grid.setOptions({
            toolbar: [{ name: "Search" },
            { text: "Save changes", name: "save" },
            { text: "Cancel changes", name: "cancel" },
            { text: "Export to PDF", name: "pdf" }]
        });
    } else {
        grid.setOptions({
            toolbar: [{ name: "Search" }]
        });
    }

    var options = grid.getOptions();
    console.log(options);
    return false;
});

$("#eq6").on("change", function () {
    var grid = $("#Grid1").data("kendoGrid");
    if ($('#eq6').is(":checked")) {
        grid.setOptions({
            toolbar: [{ name: "Search" },
            { text: "Save changes", name: "save" },
            { text: "Cancel changes", name: "cancel" },
            { text: "Export to PDF", name: "pdf" }]
        });
    } else { 
        grid.setOptions({
            toolbar: [{ text: "Save changes", name: "save" },
            { text: "Cancel changes", name: "cancel" },
            { text: "Export to PDF", name: "pdf" }]
        });
    }

    return false;
});


function dataBound() {
    this.expandRow(this.tbody.find("tr.k-master-row").first());
}

function filterGrid() {
    var grid = $("#Grid1").data("kendoGrid"), i;
    var allFilters = { logic: "and", filters: [] };

    //datepicker
    var dateRange = new Array();
    $(".k-dateinput input").each(function () {
        if ($(this).val() !== 'month/day/year') {
            dateRange.push($(this).val());
        }
    });

    if (dateRange.length >= 2) {
        allFilters.filters.push({ field: "Date", operator: "gte", value: new Date(dateRange[0]) });
        allFilters.filters.push({ field: "Date", operator: "lte", value: new Date(dateRange[1]) });
    }

    //multiselect
    var selectArray = new Array();
    $(".k-multiselect span[unselectable='on']").each(function () {
        if ($(this).text())
            selectArray.push($(this).text());
    });

    if (selectArray.length > 0) {
        var filter = { logic: "or", filters: [] };
        for (i = 0; i < selectArray.length; i++) {
            filter.filters.push({ field: "Client", operator: "eq", value: selectArray[i] });
        }
        allFilters.filters.push(filter);
    }

    //checkbox
    var value = false;
    if ($('#eq1').is(":checked")) {
        value = true;
    }

    if (value) {
        allFilters.filters.push({ field: "DNB", operator: "eq", value: value });
    }

    //staff change
    var value = $(".k-dropdown-wrap span[unselectable='on']").text();

    if (value) {
        allFilters.filters.push({ field: "Staff", operator: "eq", value: value });
    }

    //status
    var value = $("span.k-state-active").text();
    if (value) {
        allFilters.filters.push({ field: "Status", operator: "eq", value: value });
    }

    if (allFilters.filters.length > 0) {
        grid.dataSource.filter(allFilters);
    }
    else {
        grid.dataSource.filter({});
    }
}

function change() {
    var value = $("#staff").val();
    $("#cap")
        .toggleClass("black-cap", value == 1)
        .toggleClass("orange-cap", value == 2)
        .toggleClass("grey-cap", value == 3);
};

function refreshGridSource() {
    var filter = new Array();
    var strFilter = $("#SearchBox").val();
    var gridData = $("#Grid1").data("kendoGrid");

    if (strFilter.length > 0) {
        filter.push({ field: "Memo", operator: "contains", value: strFilter });
        gridData.dataSource.filter(filter);
    }
    else {
        gridData.dataSource.filter({});
    }
};

function error_handler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        alert(message);
    }
}

//$("#SearchBox").on("change", function () {
//    var enter = $("#SearchBox").val();
//    if (enter.length >= 3) {
//        refreshGridSource();
//    }
//    return false;
//});

//$("#eq1").on("change", function () {
//    dnbChange();
//    return false;
//});

//$("#selectColor").on("click", function () {
//    colorChange();
//    return false;
//});

//function staffChange() {
//    var value = this.value(),
//        grid = $("#Grid1").data("kendoGrid");

//    if (value) {
//        grid.dataSource.filter({ field: "Staff", operator: "eq", value: value });
//    } else {
//        grid.dataSource.filter({});
//    }
//}

//function dnbChange() {
//    var grid = $("#Grid1").data("kendoGrid");
//    var value = false;
//    if ($('#eq1').is(":checked")) {
//        value = true;
//    }

//    if (value) {
//        grid.dataSource.filter({ field: "DNB", operator: "eq", value: value });
//    } else {
//        grid.dataSource.filter({});
//    }
//}

//function colorChange() {
//    var grid = $("#Grid1").data("kendoGrid");
//    var value = $("span.k-state-active").text();
//    if (value) {
//        grid.dataSource.filter({ field: "Status", operator: "eq", value: value });
//    } else {
//        grid.dataSource.filter({});
//    }
//}

//function selectChange() {
//    var value = this.value(), i;
//    var grid = $("#Grid1").data("kendoGrid");

//    if (value.length > 0) {
//        for (i = 0; i < value.length; i++) {
//            grid.dataSource.filter({ field: "Client", operator: "eq", value: value[i] });
//        }
//    } else {
//        grid.dataSource.filter({});
//    }
//}

//function filterByDate() {
//    var range = new Array();
//    $(".k-dateinput input").each(function () {
//        range.push($(this).val());
//    });
//    var grid = $("#Grid1").data("kendoGrid");

//    if (range.length >= 2) {
//        var filter = { logic: "and", filters: [] };
//        filter.filters.push({ field: "Date", operator: "gte", value: range[0] });
//        filter.filters.push({ field: "Date", operator: "lte", value: range[1] });
//        grid.dataSource.filter(filter);
//    }
//    else {
//        grid.dataSource.filter({});
//    }
//}