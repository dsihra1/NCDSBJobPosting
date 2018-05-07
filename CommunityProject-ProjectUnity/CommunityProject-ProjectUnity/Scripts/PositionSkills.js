$('#btnLeftSkill').click(function (e) {
   
    var selectedOpts = $('#availOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }
 
    $('#selectedOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnRightSkill').click(function (e) {
    var selectedOpts = $('#selectedOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#availOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnLeftQ').click(function (e) {
    var selectedOpts = $('#availqualOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#selectedqualOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnRightQ').click(function (e) {
    var selectedOpts = $('#selectedqualOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move");
        e.preventDefault();
    }

    $('#availqualOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

});

$('#btnPSubmit').click(function (e) {
    $('#selectedOptions option').prop('selected', true);
    $('#selectedqualOptions option').prop('selected', true);
    //$('#selectedCities option').prop('selected', true);
    //$('#selectedSchools option').prop('selected', true);

});