var jsApiList = [
       'checkJsApi',
       'onMenuShareTimeline',
       'onMenuShareAppMessage',
       'onMenuShareQQ',
       'onMenuShareWeibo',
       'onMenuShareQZone',
       'hideMenuItems',
       'showMenuItems',
       'hideAllNonBaseMenuItem',
       'showAllNonBaseMenuItem',
       'translateVoice',
       'startRecord',
       'stopRecord',
       'onVoiceRecordEnd',
       'playVoice',
       'onVoicePlayEnd',
       'pauseVoice',
       'stopVoice',
       'uploadVoice',
       'downloadVoice',
       'chooseImage',
       'previewImage',
       'uploadImage',
       'downloadImage',
       'getNetworkType',
       'openLocation',
       'getLocation',
       'hideOptionMenu',
       'showOptionMenu',
       'closeWindow',
       'scanQRCode',
       'chooseWXPay',
       'openProductSpecificView',
       'addCard',
       'chooseCard',
       'openCard'
];

var wxUtil = {
    getAccessToken: function () {
        var url = URL("/Home/GetAccessToken");
        $.ajax({
            url: url,
            async: false,
            type: "post",
            success: function (res) {
                return res.data;
            }
        }); 
    },
    getJsapiTicket: function () {
        var url = URL("/Home/GetJsapiTicket");
        $.ajax({
            url: url,
            async: false,
            type: "post",
            success: function (res) {
                jsapiTicket = res.data;
            }
        });
    },
    getSignature: function (noncestr, jsapiTicket, timestamp, url) {
        var url = URL("/Home/GetSignature");
        $.ajax({
            url: url,
            async:false,
            type: "post",
            data: { noncestr: noncestr, jsapiTicket: jsapiTicket, timestamp: timestamp, url: url },
            success: function (res) {
                signature = res.data;
            }
        });
    }
    
};