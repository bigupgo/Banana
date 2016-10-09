$.ajax({
    url: URL("/Home/GetFood"),
    async: false,
    data: { total: 12 },
    type: "post",
    success: function (res) {
        foods = res;
        if (res != null) {
            var htmlstr = '<table border="0" cellpadding="0" cellspacing="0">'
             + ' <tr>'
			+ '<td class="lottery-unit lottery-unit-0"><img src="{0}"><br/><lable>{t0}</label></td>'
			+ '<td class="lottery-unit lottery-unit-1"><img src="{1}"><br/><lable>{t1}</label></td>'
			+ '<td class="lottery-unit lottery-unit-2"><img src="{2}"><br/><lable>{t2}</label></td>'
            + '<td class="lottery-unit lottery-unit-3"><img src="{3}"><br/><lable>{t3}</label></td>'
		+ '</tr>'
		+ '<tr>'
		+ '	<td class="lottery-unit lottery-unit-11"><img src="{11}"><br/><lable>{t11}</label></td>'
		+ '	<td colspan="2" rowspan="2"><a href="#"></a></td>'
			+ '<td class="lottery-unit lottery-unit-4"><img src="{4}"><br/><lable>{t4}</label></td>'
	  + '</tr>'
		+ '<tr>'
			+ '<td class="lottery-unit lottery-unit-10"><img src="{10}"><br/><lable>{t10}</label></td>'
		+ '	<td class="lottery-unit lottery-unit-5"><img src="{5}"><br/><lable>{t5}</label></td>'
	   + '	</tr>'
       + ' <tr>'
		+ '	<td class="lottery-unit lottery-unit-9"><img src="{9}"><br/><lable>{t9}</label></td>'
		+ '	<td class="lottery-unit lottery-unit-8"><img src="{8}"><br/><lable>{t8}</label></td>'
		+ '	<td class="lottery-unit lottery-unit-7"><img src="{7}"><br/><lable>{t7}</label></td>'
         + '<td class="lottery-unit lottery-unit-6"><img src="{6}"><br/><lable>{t6}</label></td>'
		+ '</tr>';

            $.each(res, function (index, obj) {
                var picUrl = obj.Pic == null ? '/Content/default.png' : 'http://cjq.360bosi.com:811/upload/food/' + obj.Pic;
                htmlstr = htmlstr.replace("{" + index + "}", picUrl);
                htmlstr = htmlstr.replace("{t" + index + "}", obj.FoodName);
            });
            htmlstr += "</table>";
            $("#lottery").html(htmlstr);
        }
    }
});


var lottery = {
    index: -1,	//当前转动到哪个位置，起点位置
    count: 0,	//总共有多少个位置
    timer: 0,	//setTimeout的ID，用clearTimeout清除
    speed: 20,	//初始转动速度
    times: 0,	//转动次数
    cycle: 50,	//转动基本次数：即至少需要转动多少次再进入抽奖环节
    prize: -1,	//中奖位置
    init: function (id) {
        if ($("#" + id).find(".lottery-unit").length > 0) {
            $lottery = $("#" + id);
            $units = $lottery.find(".lottery-unit");
            this.obj = $lottery;
            this.count = $units.length;
            $lottery.find(".lottery-unit-" + this.index).addClass("active");
        };
    },
    roll: function () {
        var index = this.index;
        var count = this.count;
        var lottery = this.obj;
        $(lottery).find(".lottery-unit-" + index).removeClass("active");
        index += 1;
        if (index > count - 1) {
            index = 0;
        };
        $(lottery).find(".lottery-unit-" + index).addClass("active");
        this.index = index;
        return false;
    },
    stop: function (index) {
        this.prize = index;
        return false;
    }
};

function roll() {
    lottery.times += 1;
    lottery.roll();
    if (lottery.times > lottery.cycle + 10 && lottery.prize == lottery.index) {
        clearTimeout(lottery.timer);
        lottery.prize = -1;
        lottery.times = 0;
        click = false;
    } else {
        if (lottery.times < lottery.cycle) {
            lottery.speed -= 10;
        } else if (lottery.times == lottery.cycle) {
            var index = Math.random() * (lottery.count) | 0;
            lottery.prize = index;
        } else {
            if (lottery.times > lottery.cycle + 10 && ((lottery.prize == 0 && lottery.index == 7) || lottery.prize == lottery.index + 1)) {
                lottery.speed += 110;
            } else {
                lottery.speed += 20;
            }
        }
        if (lottery.speed < 40) {
            lottery.speed = 40;
        };
        //console.log(lottery.times+'^^^^^^'+lottery.speed+'^^^^^^^'+lottery.prize);
        lottery.timer = setTimeout(roll, lottery.speed);
    }
    return false;
}

var click = false;

window.onload = function () {
    lottery.init('lottery');
    $("#lottery a").click(function () {
        if (click) {
            return false;
        } else {
            lottery.speed = 100;
            roll();
            click = true;
            return false;
        }
    });
};