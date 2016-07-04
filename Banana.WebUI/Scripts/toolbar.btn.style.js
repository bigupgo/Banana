function faIconBtn(ID) {
    var $ID = ID;
    $ID.each(function () {
        $(this).attr("title", $(this).text());
        if (typeof ($(this).attr("iconcls")) != "undefined") {
            var $iconcls = $(this).attr("iconcls");
            var i = $iconcls.indexOf("fa fa-");
            if (i >= 0) {
                var fa = $(this).find("span.fa");
                fa.removeClass($iconcls).prepend("<i class='" + $iconcls + "'></i>");
                $(".fa").parent(".l-btn-icon-left").css("padding-left", 0);
            }
        }
    })
}

$(document).ready(function () {
    faIconBtn($("#toolbar a.easyui-linkbutton"));
    faIconBtn($("#Toolbar a.easyui-linkbutton"));
    faIconBtn($("#OutToolbar a.easyui-linkbutton"));
    faIconBtn($("#OtherToolbar a.easyui-linkbutton"));
    faIconBtn($("#wrap-tool a.easyui-linkbutton"));
    faIconBtn($("#tunnelToolbar a.easyui-linkbutton"));
    faIconBtn($("#proToolbar a.easyui-linkbutton"));
    faIconBtn($("#divone a.easyui-linkbutton"));
});