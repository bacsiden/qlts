$(document).ready(function () {
    // show hide button Edit & Delete when click checkbox
    $(".checkAll").click(function () {
        if ($(this).is(':checked')) {
            $(".checkitem").prop('checked', true);
        } else {
            $(".checkitem").prop('checked', false);
        }
    });
    $(".checkitem").click(function () {
        var numberOfChecked = $('input.checkitem:checkbox:checked').length;
        var numberItem = $('input:checkbox.checkitem').length;

        if ($(this).is(':checked')) {
            if (numberItem === numberOfChecked) {
                $(".checkAll").prop('checked', true);
            }
        } else {
            $(".checkAll").prop('checked', false);
        }
    });

    // Event click with button Edit & Delete
    $(".DeleteItem").click(function () {
        if ($(".checkAll:checked, .checkitem:checked").length === 0) {
            return false;
        }

        if (!confirm("Bạn có chắc muốn xóa?")) {
            return false;
        }

        var listID = "";
        $('input.checkitem:checkbox:checked').each(function (index, item) {
            listID += "<input type='hidden' name='ids[" + index + "]' value='" + $(this).val() + "' />";
        });
        if (listID !== "") {
            $("#form-delete .form-content").html(listID);
            $("#form-delete").submit();
        } else {
            alert('Warring!');
        }
        return false;
    });

    $(document).on('keyup', 'input.typeNumber', function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });
})