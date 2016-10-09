var foods = null;


$.ajax({
    url: URL("/Home/GetFood"),
    async: false,
    data: { total: 9 },
    type: "post",
    success: function (res) {
        foods = res;
        if (res != null) {
            var htmlstr = "<div class=\"small-border g9\">";
            $.each(res, function (index, obj) {
                var picUrl = obj.Pic == null ? '/Content/default.png' : 'http://cjq.360bosi.com:811/upload/food/' + obj.Pic;
                htmlstr += "<div class=\"shan\">"
                        + "<span>" + obj.FoodName + "</span>"
                        + "<img src=" + picUrl + " width=\"30%\">"
                        + "</div>";
            });
            htmlstr += "<img src=\"/mobile/wheel/images/middle.png\" width=\"50%\" class=\"middle\">";
            htmlstr += "</div>";
            $(".big-border").html(htmlstr);
        }
    }
});


var valueJson = {
    'wheelBody': $('.big-border'), //转盘主体
    'wheelSmall': $('.small-border'), //转盘内部
    'starsNum': 16, //转盘边缘小黄点个数

    'starsPostion': [[50, 0.5], [70, 6], [84.5, 18], [92.5, 32], [95.5, 50], [91, 68], [81.5, 81.5], [68, 91], [50, 95.5], [32, 92.5], [16, 83], [6, 70], [0.5, 50], [3.5, 32], [14, 15], [27, 5.5]], //小圆点坐标
    'actionRan': 7200, //转盘转动弧度
    'theOnce': 0, //初始化转盘第一个
    'startBtn': $('.middle'), //开始按钮

    //需要后台传值的参数
    'clickAjaxUrl': 'www.baidu.com', //点击抽奖获取信息的交互的ajax
    'is_gz': 1, //是否开启关注 1开 2 关
    'is_follow': 1, //是否关注

    'foods': foods

};

indexApp.init(valueJson).wheelStart(); //应用开始

$(document).ready(function () {
    $('.g-num').find('a').click(function () {
        window.location.href = updateUrl(window.location.href, 'v')
    });
});

function updateUrl(url, key) {
    var key = (key || 't') + '=';  //默认是"t"
    var reg = new RegExp(key + '\\d+');  //正则：t=1472286066028
    var timestamp = +new Date();
    if (url.indexOf(key) > -1) { //有时间戳，直接更新
        return url.replace(reg, key + timestamp);
    } else {  //没有时间戳，加上时间戳
        if (url.indexOf('\?') > -1) {
            var urlArr = url.split('\?');
            if (urlArr[1]) {
                return urlArr[0] + '?' + key + timestamp + '&' + urlArr[1];
            } else {
                return urlArr[0] + '?' + key + timestamp;
            }
        } else {
            if (url.indexOf('#') > -1) {
                return url.split('#')[0] + '?' + key + timestamp + location.hash;
            } else {
                return url + '?' + key + timestamp;
            }
        }
    }
}