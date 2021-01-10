$(document).ready(function () {
    if (localStorage.getItem("displaySearchBox") === "true") {
        $('#collapseExample').addClass("show");
        $("#searchBox .collapse-icon a i").toggleClass("ion-chevron-down ion-chevron-up");
    }

    $(".select2").select2({ width: '100%', theme: "bootstrap" });

    $('#searchBox .collapse-icon a').click(function () {
        $("#searchBox .collapse-icon a i").toggleClass("ion-chevron-down ion-chevron-up");

        var displayStatus = $("#searchBox a i").hasClass("ion-chevron-up");
        localStorage.setItem("displaySearchBox", displayStatus);
    });

    $("#btnSearch").click(function () {
        $("#pattern").val(null);
        $("#frmSearch").submit();
    });
    $('.print-barcode').click(function () {
        if (confirm('Bạn chắc chắn muốn in mã vạch?')) {
            $("#pattern").val('barcode');
            $("#frmSearch").submit();
        }
    });
    $('.report').click(function () {
        $("#pattern").val($(this).data('pattern'));
        $("#frmSearch").submit();
    });
});

function createKiemKe(e) {
    $("#frmSearch").attr('action', '/KiemKe/Create');
    $("#frmSearch").submit();
}

function editTaiSan(id) {
    $.get('/Home/EditAsset/' + id, function (response) {
        $('#modalDefault').html(response);
        $("#modalDefault").modal('show');
    });
}

function submitEditAsset() {
    //Serialize the form datas.   
    var valdata = $("#frmAsset").serialize();
    $.ajax({
        url: $("#frmAsset").attr('action'),
        type: "POST",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: valdata
    }).done(function (response) {
        alert(response);
        $('#modalDefault').html(response);
        $("#modalDefault").modal('show');
    });
    return false;
}