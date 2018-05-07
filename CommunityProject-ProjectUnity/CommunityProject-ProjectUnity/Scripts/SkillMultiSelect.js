
$('#btnSkillLeft').click(function (e) {
    var selectedOpts = $('#availOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#selectedOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnQualLeft').click(function (e) {
    var selectedOpts = $('#availqualOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#selectedqualOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnLeftCity').click(function (e) {
    var selectedOpts = $('#availCities option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#selectedCities').append($(selectedOpts).clone());
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

$('#btnSkillRight').click(function (e) {
    var selectedOpts = $('#selectedOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#availOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnQualRight').click(function (e) {
    var selectedOpts = $('#selectedqualOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#availqualOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

//$('#btnRightCity').click(function (e) {
//    var selectedOpts = $('#selectedCities option:selected');
//    if (selectedOpts.length == 0) {
//        alert("Nothing to move");
//        e.preventDefault();
//    }

//    $('#availCity').append($(selectedOpts).clone());
//    $(selectedOpts).remove();
//    e.preventDefault();

//});

$('#btnRightSchool').click(function (e) {
    var selectedOpts = $('#selectedSchools option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#availSchool').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnSubmit').click(function (e) {
    $('#selectedOptions option').prop('selected', true);
    $('#selectedSchools option').prop('selected', true);

});

if ($("#PositionType").val().valueOf() == "Teaching".valueOf()) {
    $('#availlabel').html('Available Qualifications');
    $('#selectlabel').html('Selected Qualifications');
}