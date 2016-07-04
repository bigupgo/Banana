var $hLoading = null;

var hAjax_Dialog_Config = {
    width: 240,
    height: 70,
    modal: true,
    border: false,
    noheader: true,
    closable: false,
    closed: false
};

var hMessage_Config =
{
    title: '提示信息',
    msg: '操作成功！',
    timeout: 3500,
    showType: 'slide'
}

var hAlert_Config =
{
    title: '错误提示',
    msg: '操作失败',
    timeout: 3500,
    icon: 'error'
}
var ISLOADING = false;
$.hAjax = function (options) {
    var $loading = null;
    var c =
    {
        beforeSend: function () {
            if (options.isLoading!=false) {
                if (ISLOADING) { return; }
                ISLOADING = true;
                $hLoading = $('<div>').appendTo($('body'));
                $hLoading.html('<p style="text-align:center;height:30px;line-height:55px;color:#0000ff"><img id="forLoading" />&nbsp;&nbsp;正在处理,请稍候...</p>');
                $hLoading.find('#forLoading').attr('src', URL('/Content/Home/images/loading.gif'));
                $hLoading.dialog(hAjax_Dialog_Config);
            }
        },
        complete: function (xhr) {
            if ($hLoading != null) {
                $hLoading.dialog('destroy');
                ISLOADING = false;
                $hLoading = null;
            }
            //renjinquan2015.11.27添加autoShowMessage，当为false不自动弹出消息
            if (options.autoShowMessage != false) {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    try {
                        var respObj = $.parseJSON(xhr.responseText);
                        if (respObj) {
                            if (respObj.success) {
                                $.hMessage({ timeout: options.timeshow || 2000, msg: '<h3 style="color:green;" >' + respObj.message + '</h3>' });
                            } else if (respObj.success == false) {
                                $.hAlert({ width: 380, msg: '<h3 style="color:red" >' + respObj.message + '</h3>' });
                            }
                        }
                    }
                    catch (er) {
                    }
                }
            }
        }
    }
    var config = $.extend({}, c, options || {});
    $.ajax(config);
}

$.hMessage = function (options) {
    var opt = $.extend({}, hMessage_Config, options || {});
    top.$.messager.show(opt);
}

$.hAlert = function (options) {
    var opt = $.extend({}, hAlert_Config, options || {});
    top.$.messager.show(opt);
}

//主要是推荐这个函数。它将jquery系列化后的值转为name:value的形式。
function convertArray(o) {
    var v = {};
    for (var i in o) {
        if (o[i].name != '__VIEWSTATE') {
            if (typeof (v[o[i].name]) == 'undefined')
                v[o[i].name] = o[i].value;
            else
                v[o[i].name] += "," + o[i].value;
        }
    }
    return v;
}

//处理ajax异常
$(document).ajaxError(function (event, request, settings) {

    //20151105-renjinquan修改，添加try主要处理不在index框架内的页面链接超时调用top.ShowLogin()出错。
    if (request.getResponseHeader('X-Requested-SessionOut') == 'true') {
        try {
            // alert('登录超时，需要重新登录'); //页面内多次Ajax 弹窗提示重复
            if (top.sysDeptID) {
                top.ShowLogin();
            } else { location.href = URL('/Home/Login'); }
            return;
        } catch (e) {
            location.href = URL('/Home/Login');
        }

    }

    if (request.getResponseHeader('X-Requested-Logining') == 'true') {
        // alert('登录超时，需要重新登录'); //页面内多次Ajax 弹窗提示重复
        alert('本账号在其他地方登录，您被迫下线！');
        location.href = URL('/Home/Login');
        return;
    }

    if (request.getResponseHeader('X-Requested-UnAuthorized') == 'true') {
        var errorMsg = '<p class="messager-icon messager-error" style="color:red;"></p><h3 style="color:red">你正在尝试，越权操作，请求已被终止！<br>错误代码：' + request.status + '<br /> 错误地址:' + settings.url + '</h3>';
        $.hMessage({ width: 380, msg: errorMsg, title: '越权操作' });
        return;
    }
    var title = "服务器端异常";
    var matchs = request.responseText.match('<title>(.+)</title>');
    if (matchs && matchs.length > 1) { title = matchs[1]; }
    var errorMsg = '<p class="messager-icon messager-error" style="color:red;"></p><h1 style="color:red">异常：' + title + '<br />代码：' + request.status + '<br /> 地址:' + settings.url + '</h1>';
    $.hMessage({ width: 380, msg: errorMsg, title: '服务器端错误' });
});