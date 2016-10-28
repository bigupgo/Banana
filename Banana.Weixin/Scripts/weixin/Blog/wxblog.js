$(function () {
    $("#showTooltips").click(function () {
        //$('.js_tooltips').show();
        //setTimeout(function () {
        //    $('.js_tooltips').hide();
        //}, 3000);

        var $fm = $('form');
        //if (!$fm.form('validate')) return;
        var sendData = $fm.serializeArray();
        $('#toast').show();
        setTimeout(function () {
            $('#toast').hide();
        }, 2000);
        
    });

   

});
