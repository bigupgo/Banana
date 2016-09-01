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
        var url = wxUrl.Token(appId, appId);
        $.ajax({
            url: url,
            type: "get",
            dataType: "json",
            success: function (data) {
                var accessToken = JSON.parse(data);
                storageUtils.setParam("token", accessToken);
                return accessToken;
            }
        });
        
    }

};