var DK = {

    init: function () {
        $(".date").datetimepicker({
            format: 'DD/MM/YYYY', locale: 'vi'
        });
        $(".date-get").datetimepicker({
            format: 'YYYY-MM-DD', locale: 'vi'
        });
    }
}