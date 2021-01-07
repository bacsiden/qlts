var key = {
    enter: 13,
    tab: 9,
    up: 38,
    down: 40
}

$(document).ready(function () {
    $(document).on('keydown', '#post-data input.table-text-box', function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode === key.enter) {
            if (e.shiftKey) {
                focusUp(e.target);
                return false;
            }

            if (shouldAddRow(e.target)) {
                addRow();
            } else {
                focusDown(e.target);
            }
            return false;
        }

        if (keyCode === key.tab && shouldAddRow(e.target)) {
            if (e.shiftKey) {
                focusUp(e.target);
            } else {
                addRow();
                return false;
            }
        }

        if (keyCode === key.up) {
            focusUp(e.target);
        }

        if (keyCode === key.down) {
            focusDown(e.target);
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

    if (!result) {
        $('#post-data tr:last input').focus();
        return false;
    }

    $("#inputElement").html(result);
    return true;
}

function addRow() {
    var row = $('#post-data tr:last').html();
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

function shouldAddRow(e) {
    var next = $(e).closest('tr').next();
    return next.length === 0;
}

function focusUp(e) {
    $(e).parents("tr").prev().find("input").focus();
}

function focusDown(e) {
    $(e).parents("tr").next().find("input").focus();
}

function mergeCategory() {
    var items = $("#post-data input.merge-item:checked");
    if (items.length === 0) {
        return;
    }

    var value = $(items[0]).val();
    var modalContent = $("#modal-content").html();    

    $("#modalDefault").html(modalContent);
    $("#modalDefault").find(".new-name").val(value);
    $("#modalDefault").modal('show');
}