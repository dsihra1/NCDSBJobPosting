
    $(function () {
        
        if ($("#CityID").val() == '0') {
            var cityDefaultValue = "<option value='0' >--Select a City--</option>"
            $("#SchoolID").html(cityDefaultValue).show();
        }
         $("#CityID").change(function () {
            var selectedItemValue = $(this).val();

            var ddlSchools = $("#SchoolID");
            $.ajax({
                cache: false,
                type: "GET",
                url: '@Url.Action("GetSchoolByCityID", "Postings")',
                data: { "ID": selectedItemValue },
                success: function (data) {
                    ddlSchools.html('');
                    $.each(data, function (id, option) {
                        ddlSchools.append($('<option></option>').val(option.id).html(option.name));
                    });
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Found error.');
                }
            });
        });
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
                url: '@Url.Action("GetPositionByJobCode", "Postings")',
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

       

        
       
        if (navigator.userAgent.indexOf('Chrome') != -1) {
            $('input[type=date]').on('click', function (event) {
                event.preventDefault();
            });
        }
        $(document).on('click input', 'input[type="date"], input[type="text"].date-picker', function (e) {
            var $this = $(this);
            $this.prop('type', 'text').datepicker({
                showOtherMonths: true,
                selectOtherMonths: true,
                showButtonPanel: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd',
                showWeek: true,
                firstDay: 1
            });

            setTimeout(function () {
                $this.datepicker('show');
            }, 1);
        });
    });

$(document).ready(function () {
    $(function () {
        $(".date-time-picker").datetimepicker({
            changeMonth: true,
            changeYear: true,
            yearRange: "-100:+0",
            dateFormat: 'yy-mm-dd',
            controlType: 'select',
            timeFormat: 'hh:mm TT'
        });
    });

    jQuery.validator.methods.date = function (value, element) {
        var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
        if (isChrome) {
            var d = new Date();
            return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
        } else {
            return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
        }
    };
});