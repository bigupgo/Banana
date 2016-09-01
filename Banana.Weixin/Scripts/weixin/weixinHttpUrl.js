var wxUrl = {
    //获取token GET 有效期7200秒
    Token: function (APPID, APPSECRET) {
        return "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + APPID + "&secret=" + APPSECRET + "";
    },
    //获取微信服务器IP地址 GET
    IP: function (ACCESS_TOKEN) {
        return "https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token=" + ACCESS_TOKEN;
    },
    //获得jsapi_ticket（有效期7200秒) GET
    JsapiTicket: function (ACCESS_TOKEN) {
        return "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + ACCESS_TOKEN + "&type=jsapi";
    }
}