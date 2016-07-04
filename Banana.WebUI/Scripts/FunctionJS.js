
//获取url参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};
//设置Cookie
function setCookie(c_name, value, expiredays) {
    var exdate = new Date()
    exdate.setDate(exdate.getDate() + expiredays)
    document.cookie = c_name + "=" + escape(value) +
((expiredays == null) ? "" : ";expires=" + exdate.toGMTString())
}

//获取Cookie
function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=")
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return unescape(document.cookie.substring(c_start, c_end))
        }
    }
    return ""
}

//把json化后的\/Date(1234656000000)\/转换为时间类型对象
function ConvertJSONDate(str) {
    if (str.indexOf('T') > -1) {
        return new Date(LongDate(str).replace(/-/g, '/'));
    }
    else if (str.indexOf('/') > -1) {
        return new Date(parseInt(str.replace("/Date(", "").replace(")/", ""), 10));
    }
    return str;
}

//把这种格式的时间转化为\/Date(1234656000000)\/ 年月日的是日期 
function ShortDate(cellval) {
    if (!cellval) return '';
    if (cellval.indexOf('T') > -1) {
        return cellval.replace(/T.+/, '');
    }
    else if (cellval.indexOf('/') > -1) {
        var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minu = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var sec = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

        return date.getFullYear() + "-" + month + "-" + currentDate; //+ " " + hour + ":" + minu + ":" + sec;
    }
    return cellval;
}
//把这种格式的时间转化为\/Date(1234656000000)\/ 年月日时分的是日期 
function LongDate(cellval) {
    if (!cellval) return '';

    if (cellval.indexOf('T') > -1) {
        return cellval.replace('T', ' ').replace(/\.\d.+$/, '');//renjinquan添加了.replace(/.\d+$/, '')去掉毫秒部分
    }
    else if (cellval.indexOf('/') > -1) {
        var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minu = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var sec = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

        return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minu + ":" + sec;
    }
    return cellval;
}

//把这种格式的时间转化为\/Date(1234656000000)\/ 年月日时分的是日期 
function LongDateNoSec(cellval) {
    if (!cellval) return '';
    if (cellval.indexOf('T') > -1) {
        return cellval.replace('T', ' ').replace(/:\d+$/, '');
    }
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
    var minu = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();

    return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minu;
}

//日期转换把一个时间转成yyyy-MM-dd
function ConvertDate(tmpDate) {
    if (tmpDate != null && tmpDate != '') {
        var fulldate = new Date(tmpDate);
        var mon = fulldate.getMonth() + 1;
        mon = mon < 10 ? '0' + mon : mon;
        var days = fulldate.getDate();
        days = days < 10 ? '0' + days : days;
        var toShowDate = fulldate.getFullYear() + '-' + mon + '-' + days;
        return toShowDate;
    } else {
        return '';
    }
}

//时间转换 yyyy-MM-dd HH:mm:ss
function ConvertDateTime(tmpDateTime) {
    if (tmpDateTime != null && tmpDateTime != '') {
        var tDate = ConvertDate(tmpDateTime)
        var t = new Date(tmpDateTime);

        var h = t.getHours();
        if (h < 10) { h = '0' + h; }

        var m = t.getMinutes();
        if (m < 10) { m = '0' + m; }

        var s = t.getSeconds();
        if (s < 10) { s = '0' + s; }

        var toShowDate = tDate + ' ' + h + ":" + m + ":" + s;
        return toShowDate;
    } else {
        return '';
    }
}


//计算strInclude在strFull中出现的次数，如果isSensitive的值为false，大小写不敏感，反之大小写敏感
function GetSubstrCount(strFull, strInclude, isSensitive) {
    var count = 0;
    var pos;
    if (!isSensitive) {
        strFull = strFull.toLowerCase();
        strInclude = strInclude.toLowerCase();
    }
    while ((pos = strFull.indexOf(strInclude)) != -1) {
        strFull = strFull.substr(pos + strInclude.length, strFull.length);
        count++;
    }
    return count;
}
//长文字截断
function ShortWords(str, len) {
    if (!str) { return ''; }
    if (str.length > len) {
        return str.substr(0, len) + '...';
    } else { return str; }
}

//解决console兼容问题
if (!window.console) {
    var console = {};
    console.log = function () { };
    window.console = console;
}
// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2015-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2015-7-2 8:9:4.18   
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

Date.prototype.addDays = function (days) {
    //this = this.valueOf()
    //this = this + days * 24 * 60 * 60 * 1000
    //this = new Date(this)
    //return this;
    var a = new Date(this);
    a = a.valueOf();
    a = a + days * 24 * 60 * 60 * 1000;
    a = new Date(a);
    return a;
}
Date.prototype.addMonths = function (months) {
    var a = new Date(this);

    a = a.setMonth(a.getMonth() + 1 + months);
    a = new Date(a);
    return a;
}



//马祥龙   2015-11-10      
//参数1【$DeptTreeList】: 树形对象 如:$("#Tree")
//参数2【$West】：Layout对象   如:$("#LayourID")
function Layout_West_Collapse($DeptTreeList, $West) {
    //获得所有父级结点
    var Roots = $DeptTreeList.tree('getRoots');
    if (Roots.length > 1) { return false; }

    for (var i = 0; i < Roots.length; i++) {
        var row = Roots[i];
        if ($DeptTreeList.tree('find', row.id).children != null) {
            return false;
        }
    }

    $West.layout('collapse', 'west');
}