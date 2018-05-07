if ($("#PositionType").val() == '0') {
            var jobDefaultValue = "<optipn value='0' >--Select a Job Type--</option>"
            $("#JobTypeID").html(jobDefaultValue).show();
        }
        $("#PositionType").change(function () {
            var selectedItemValue = $(this).val();

            var ddlJobcode = $("#JobTypeID");
            $.ajax({
                cache: false,
                type: "GET",
                url: '@Url.Action("GetJobCodeByPositionType", "Positions")',
                data: { "ID": selectedItemValue },
                success: function (data) {
                    ddlJobcode.html('');
                    $.each(data, function (id, option) {

                        ddlJobcode.append($('<option></option>').val(option.id).html(option.name));
                    });
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Found error.');
                }
            });
        });

        $('#JobTypeID').change(function () {
            var selectedItemValue = $(this).val();

            var ddlPosition = $('#PositionID');
            $.ajax({
                cache: false,
                type: "GET",
                url: '@Url.Action("GetPositionByJobCode", "Positions")',
                data: { "ID": selectedItemValue },
                success: function (data) {
                    ddlPosition.html('');
                    $.each(data, function (id, option) {
                        ddlPosition.append($('<option></option>').val(option.id).html(option.name));
                    });
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Found error.');
                }

            });
        });

        $('#JobTypeID').change(function () {
            $('#Name').val('');
            $('#Description').empty();
            $('#Salary').val('');
            $('#SkillsList').empty();
            var selectedPosition = $("#PositionID").val();
            var URL = "/Postings/GetAPosition/" + selectedPosition;
            $.getJSON(URL, function (data) {
                if (data != null && !jQuery.isEmptyObject(data)) {
                    $('#Name').val(data.Name);
                    $('#Description').html(data.PositionDescription);
                    $('#Salary').val(data.Salary);
                    $('#SkillsList').html(data.Assignedskill);
                };
            });
        });