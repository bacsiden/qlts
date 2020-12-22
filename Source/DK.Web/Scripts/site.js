// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

jQuery(document).ready(function () {

    "use strict";
    function validateMenuLink($this) {
        var itemUrl = $this.attr('href') + '/' + $this.attr('data-href');
        var isActive = itemUrl.indexOf(iclassic.currentLink) !== -1;
        return isActive;
    }

    function activeMenu() {
        var found = false;
        jQuery("#LeftMenu a").each(function () {
            var $this = jQuery(this);
            var isActive = validateMenuLink($this);
            if (!isActive) return;

            found = true;
            $this.addClass("active");
        });

        if (found) return;

        jQuery("#LeftMenu a").each(function () {
            var $this = jQuery(this);
            var isActive = validateMenuLink($this);
            if (!isActive) return;

            $this.parent().addClass("active");
            if (!$("body").hasClass("collapsed-menu")) {
                $this.parent().parent().show();
            }
            $this.parents(".nav-parent").addClass("active nav-active");
        });
    }
    activeMenu();
});

function addYoutubeProfile() {
    var body = $("#box-yt-profiles tbody");
    var count = body.find("tr").length;
    var tr = "<tr class='stat even'>" +
        "<td class='ac'>" +
        (count + 1) +
        "</td>" +
        "<td>" +
        "	<input name='Campaign.YoutubeConfig.Profiles[" + count + "].Title' type='text' value='' class='form-control' placeholder='Nhập tiêu đề'>" +
        "</td>" +
        "<td>" +
        "	<label class='custom-file'>" +
        "		<input name='Campaign.YoutubeConfig.Profiles[" + count + "].ProfileFilePath' type='hidden' value=''>" +
        "		<input accept='.zip' class='custom-file-input text-box single-line' name='Campaign.YoutubeConfig.Profiles[" + count + "].ProfileFile' type='file'>" +
        "		<span class='custom-file-control' data-text='Chèn profile...'>" +
        "		</span>" +
        "	</label>" +
        "</td>" +
        "<td>" +
        "	<label class='custom-file'>" +
        "		<input name='Campaign.YoutubeConfig.Profiles[" + count + "].TitlePath' type='hidden' value=''>" +
        "		<input accept='.txt' class='custom-file-input text-box single-line' name='Campaign.YoutubeConfig.Profiles[" + count + "].TitleFile' type='file'>" +
        "		<span class='custom-file-control' data-text='Chèn title...'>" +
        "		</span>" +
        "	</label>" +
        "</td>" +
        "<td>" +
        "	<label class='custom-file'>" +
        "		<input name='Campaign.YoutubeConfig.Profiles[" + count + "].TagPath' type='hidden' value=''>" +
        "		<input accept='.txt' class='custom-file-input text-box single-line' name='Campaign.YoutubeConfig.Profiles[" + count + "].TagFile' type='file'>" +
        "		<span class='custom-file-control' data-text='Chèn tag...'>" +
        "		</span>" +
        "	</label>" +
        "</td>" +
        "<td>" +
        "	<a href='javascript:void(0);' class='btn btn-sm btn-danger' onclick='removeRow(this);'>Xóa</a>" +
        "</td>" +
        "</tr>";

    body.append(tr);

    $("#box-yt-profiles .custom-file input[type='file']").each(function () {
        resetInputFile($(this));
    });
}

function addTiktokProfile() {
    var body = $("#box-tt-profiles tbody");
    var count = body.find("tr").length;
    var tr = "<tr class='stat even'>" +
        "<td class='ac'>" +
        (count + 1) +
        "</td>" +
        "<td>" +
        "	<input name='Campaign.TikTokConfig.Profiles[" + count + "].Title' type='text' value='' class='form-control' placeholder='Nhập tiêu đề'>" +
        "</td>" +        
        "<td>" +
        "	<label class='custom-file'>" +
        "		<input name='Campaign.TikTokConfig.Profiles[" + count + "].TitlePath' type='hidden' value=''>" +
        "		<input accept='.txt' class='custom-file-input text-box single-line' name='Campaign.TikTokConfig.Profiles[" + count + "].TitleFile' type='file'>" +
        "		<span class='custom-file-control' data-text='Chèn title...'>" +
        "		</span>" +
        "	</label>" +
        "</td>" +        
        "<td>" +
        "	<a href='javascript:void(0);' class='btn btn-sm btn-danger' onclick='removeRow(this);'>Xóa</a>" +
        "</td>" +
        "</tr>";

    body.append(tr);

    $("#box-tt-profiles .custom-file input[type='file']").each(function () {
        resetInputFile($(this));
    });
}

function removeRow($this) {
    $($this).parents('tr').remove();
}