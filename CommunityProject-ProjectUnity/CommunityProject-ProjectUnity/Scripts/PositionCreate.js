$(function () {

    $("#PositionType").change(function () {

        var selectedItemValue = $(this).val();

        var ddlJobcode = $("#availOptions");
        $.ajax({
            cache: false,
            type: "GET",
            url: '@Url.Action("GetOptListByPositionType", "Positions")',
            data: { "ID": selectedItemValue },

            error: function (xhr, ajaxOptions, thrownError) {
                alert('Found error.');
            }
        });
    });
});