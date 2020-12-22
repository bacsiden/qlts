$(document).ready(function () {
    $(document).on('keydown', '#post-data input.table-text-box', function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode === 13 || keyCode === 9) {
            var next = $(e).closest('tr').next();
            if (next.length === 0) addRow();
            return false;
        }
    });
})

function beforeSubmit() {
    var i = 0;
    var result = "";
    $('#post-data tr').each(function () {
        var value = $(this).find('input.table-text-box').val();
        if (value) {
            result += "<input type='hidden' name='list[" + i + "]' value='" + value + "' />";
            i++;
        }
    });
    $("#inputElement").html(result);
    return true;
}

function addRow() {
    var row = $('#post-data tr:last').html();
    var tlast = $('#post-data tr:last input:first').val();
    $('#post-data tr:last').after('<tr>' + row + '</tr>');
    $('#post-data tr:last input').val('');

    // add STT
    $('#post-data tr').each((index, item) => {
        $(item).data('index', index);
        $(item).find("td.stt").text(index + 1);
    });

    //$('#post-data tr:last input:first').val(100);

    $('#post-data tr:last input').focus();
}

function onChange(e) {
    var next = $(e).closest('tr').next();
    if (next.length === 0) addRow();
}