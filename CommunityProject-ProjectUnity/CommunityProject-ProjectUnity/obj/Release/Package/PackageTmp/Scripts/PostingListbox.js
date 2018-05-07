
$('#btnLeft').click(function (e) {
    var selectedOpts = $('#availOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#selectedOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnLeftSchool').click(function (e) {
    var selectedOpts = $('#availSchools option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#selectedSchools').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});


$('#btnRight').click(function (e) {
    var selectedOpts = $('#selectedOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#availOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnRightSchool').click(function (e) {
    var selectedOpts = $('#selectedSchools option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#availSchools').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnSubmit').click(function (e) {
    $('#selectedOptions option').prop('selected', true);
    $('#selectedSchools option').prop('selected', true);

});

