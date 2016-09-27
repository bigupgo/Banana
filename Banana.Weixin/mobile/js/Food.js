var foods = null;


$.ajax({
    url: URL("/Home/GetFood"),
    async: false,
    type: "post",
    success: function (res) {
        foods = res;
        if (res != null) {
            var htmlstr = "<div class=\"small-border g9\">";
            $.each(res, function (index, obj) {
                var picUrl = obj.Pic == null ? '/Content/default.png' : 'http://cjq.360bosi.com:811/upload/food/'+obj.Pic;
                htmlstr += "<div class=\"shan\">"
                        + "<span>" + obj.FoodName + "</span>"
                        + "<img src=" + picUrl + " width=\"30%\">"
                        + "</div>";
            });
            htmlstr += "<img src=\"/mobile/images/middle.png\" width=\"50%\" class=\"middle\">";
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
        window.location.reload();
    });
});