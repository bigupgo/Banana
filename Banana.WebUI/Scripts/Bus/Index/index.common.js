//当前消息
var CURRENTMSG = null;
var maxTabCount = 5;//最多打开Tab数
var isAddMainPage = false;
//窗体加载
$(document).ready(function () {
    isAddMainPage = true;
    fnHandlerRefresh(); //F5刷新处理
    initTime();         //时间日期显示
 //   bindDeptTooltip();  //绑定切换部门
    tabCloseEven();     //tabs右键菜单

});

//操作url
var ActionURL =
        {
            GetSonModules: URL('Base/Common/GetDirectSonModules'),
            GetModuleTrees: URL('Base/Common/GetAllSonModules'),
            SearchModule: URL('Base/Common/SearchModules'),
          
            LoginOut: URL('Home/LoginOut'),
       
            SelfInfo: URL('/Base/Personal/GetDetail'),
            ResetPass: URL('/Base/Personal/ResetPass'),

       
        };
//静态模板
var HtmlTemplate =
    {
        SelfInfo: URL('Content/Home/html/selfInfo.html'),
        ResetPass: URL('Content/Home/html/resetPass.html'),
        ReLogin: URL('Content/Home/html/login.html'),
        SystemHelp: URL('Content/Home/html/systemHelp.html'),

    };


//子页面iframe onload 事件 去除遮罩
function onIframeload() {
    if ($topLoading != null) {
        $topLoading.dialog('close');
        $topLoading.dialog('destroy');
        $topLoading = null;
    }
};



//顶层遮罩
var $topLoading = null;
//创建顶层遮罩
function createTopLoading() {
    if (null != $topLoading) { return; }
    $topLoading = $('<div>').appendTo($('body'));
    var loadHtml = '<p style="text-align:center;height:30px;line-height:55px;color:#0000ff">';
    loadHtml += '<img id="forLoading" />&nbsp;&nbsp;加载页面,请稍候...</p>';
    $topLoading.html(loadHtml);
    $topLoading.find('#forLoading').attr('src', URL('/Content/Home/images/loading.gif'));
    $topLoading.dialog({
        width: 240,
        height: 70,
        modal: true,
        border: false,
        noheader: true,
        closable: false,
        closed: false
    });
};

//初始化时间日期
function initTime() {
    var dNow = new Date();
    var WEEK = new Array(7);
    WEEK[0] = "日"; WEEK[1] = "一"; WEEK[2] = "二"; WEEK[3] = "三"; WEEK[4] = "四"; WEEK[5] = "五"; WEEK[6] = "六";
    var temp = "";
    temp += (dNow.getMonth() + 1) + "月" + dNow.getDate() + "日";
    temp += " 星期" + WEEK[dNow.getDay()]
    $('#sp-nowDate').html(temp);
    var $nowTime = $('#sp-nowTime');
    function t() {
        var dNow = new Date();
        var timestr = dNow.toLocaleString();
        timestr = timestr.substring(timestr.indexOf(" "));
        $nowTime.html(timestr);
    }
    setInterval(t, 1000);
}


//关闭tab页事件
function onTabColse(title, index) {
    if (index > 0) {
        $('#wrap-MainTabs').tabs("select", index - 1);
    }
}


//设置按钮
function fnSettingCenter(e) {
    e = e || event;
    var x = e.clientX || e.pageX;
    var y = e.clientY || e.pageY;
    $('#settingMenu').menu('show', {
        left: x - 150,
        top: y - 20
    });
};
//切换皮肤
function fnSwitchSkin(skin) {
    var $link = $('#link-skin');
    var url = $link.attr('href');
    url = url.replace(/themes\/.+\//, 'themes/' + skin + '/');
    $link.attr('href', url);
    //setCookie('skin', skin, 7);
    $('#wrap-MainTabs').find('iframe').each(function () {
        var $one = $(this).contents().find('#link-skin');
        if (typeof this.contentWindow.setCookie === "function") {
            this.contentWindow.setCookie('skin', skin, 7);
        }
    });
    location.reload();
}

//最大化
function fnFullScreen() {
    var $layout = $('body');
    if (!$layout.data('full')) {
        $layout.data('full', 1);
        $layout.layout('collapse', 'west');
        $layout.layout('collapse', 'north');
        //$layout.layout('collapse', 'south');
    } else {
        $layout.data('full', 0);
        $layout.layout('expand', 'west');
        $layout.layout('expand', 'north');
        //$layout.layout('expand', 'south');
    }
    return;
   
}

//折叠左边
function fnCollapseLeft() {
    $('body').layout('collapse', 'west');
    var leftPanel = $('body').layout('panel', 'west');
    leftPanel.resize({ width: 1200, height: 600 });
}

//折叠上边
function fnCollapseTop() {
    $('body').layout('collapse', 'north');
}

//新窗口打开
function fnNewWindow() {
    var $tab = $('#wrap-MainTabs').tabs('getSelected');
    var $frame = $tab.find('iframe');
    if ($frame) {
        var url = $frame.attr('src');
        //if (!(/\./).test(url)) {
        //    url = URL(url);
        //}
        window.open(url, '',
            'z-look=1,status=0,channelmode=1,fullscreen=1,scrollbars=1,toolbar=0,menubar=no');
    }
}

//刷新中间
function fnReloadCenter() {
    var $tab = $('#wrap-MainTabs').tabs('getSelected');
    var $frame = $tab.find('iframe');
    if ($frame) {
        $frame[0].contentWindow.location.reload();
    }
}
//Tabs选中事件
function fnTabOnSelect(title) {
    var name = '', id = '', icon = '', url = '';

    if (title != '首页' && history.pushState) {
        var $sel = $('#wrap-MainTabs').tabs('getTab', title);
        var url = $sel.find('iframe').attr('src');
        url = url.replace(/\?.+/, '');
        var op = $sel.panel("options");
        //history.pushState(null, title, url);

        name = op.title;
        id = op.id;
        icon = op.iconCls;
        url = $sel.find('iframe').attr('src');
       
    } else {
      

        if (!isAddMainPage) {
            //setCookie('lrsmes_selecttab_title_' + moduleID + '_' + sysUserID, name);
            //setCookie('lrsmes_selecttab_id_' + moduleID + '_' + sysUserID, id);
            //setCookie('lrsmes_selecttab_icon_' + moduleID + '_' + sysUserID, icon);
            //setCookie('lrsmes_selecttab_url_' + moduleID + '_' + sysUserID, url);
        }
    }

    isAddMainPage = false;//是否为第一次添加首页选择
}
//Tabs右键
function fnTabContextMenu(e, title, index) {
    if (title == '首页') {
        $('#tab-rightMenu').menu('disableItem', $('#close')[0])
    }
    else {
        $('#tab-rightMenu').menu('enableItem', $('#close')[0])
    }
    e.preventDefault();
    $('#tab-rightMenu').menu('show', {
        left: e.pageX,
        top: e.pageY
    });
}
//Tab右键 菜单
function tabCloseEven() {
    $('#tab-rightMenu').menu({
        onClick: function (item) {
            closeTab(item.id);
        }
    });
    return false;
}
//Tabl Action 调用方法
function closeTab(action) {
    var onlyOpenTitle = "首页";
    var $tab = $('#wrap-MainTabs');
    var alltabs = $tab.tabs('tabs');
    var currentTab = $tab.tabs('getSelected');
    var allTabtitle = [];
    $.each(alltabs, function (i, n) {
        allTabtitle.push($(n).panel('options').title);
    })

    switch (action) {
        case "refresh":
            var iframe = $(currentTab.panel('options').content);
            var src = iframe.attr('src');
            var name = iframe.attr('name');
            $tab.tabs('update', {
                tab: currentTab,
                options: {
                    content: fnCreateTabContent(src, name)
                }
            })
            break;
        case "close":
            var currtab_title = currentTab.panel('options').title;
            var index = $tab.tabs('getTabIndex', currentTab);
            if (index > 0) {
                $tab.tabs('select', index - 1);
            }

            $tab.tabs('close', currtab_title);
            break;
        case "closeall":
            $.each(allTabtitle, function (i, n) {
                if (n != onlyOpenTitle) {
                    $tab.tabs('close', n);
                }
            });
            break;
        case "closeother":
            var currtab_title = currentTab.panel('options').title;
            $.each(allTabtitle, function (i, n) {
                if (n != currtab_title && n != onlyOpenTitle) {
                    $tab.tabs('close', n);
                }
            });
            break;
        case "closeright":
            var tabIndex = $tab.tabs('getTabIndex', currentTab);

            if (tabIndex == alltabs.length - 1) {
                alert('亲，后边没有啦 !');
                return false;
            }
            $.each(allTabtitle, function (i, n) {
                if (i > tabIndex) {
                    if (n != onlyOpenTitle) {
                        $tab.tabs('close', n);
                    }
                }
            });

            break;
        case "closeleft":
            var tabIndex = $tab.tabs('getTabIndex', currentTab);
   
            $.each(allTabtitle, function (i, n) {
                if (i < tabIndex) {
                    if (n != onlyOpenTitle) {
                        $tab.tabs('close', n);
                    }
                }
            });
            break;
      
        

        case "exit":
            $('#closeMenu').menu('hide');
            break;
    }
}


//当添加tab是触发事件
function onAddTab(title, index) {
    var tabPanel = $('#wrap-MainTabs');
    var tabs = tabPanel.tabs("tabs");
    if (tabs.length > maxTabCount) {
        for (var i = 0; i < tabs.length; i++) {
            var o = $(tabs[i]).panel('options');
            if (o.closable) {
                tabPanel.tabs('close', i);
                break;
            }
        }
    }
}



//添加tab页面useURLFun是否使用url函数转换，仅为false不转换
//paras:'&aaa=1'
function addTab(name, url, iconCls, closable, id, paras, useURLFun) {
    var tagUrl = url.url || url;
    var limit = url.limit || -1;

    tagUrl = $.trim(tagUrl);
    if (tagUrl) {
        if (useURLFun != false) {
            tagUrl = URL(tagUrl);
        }
        var tabPanel = $('#wrap-MainTabs');
        var isSelect = false;
        var tabs = tabPanel.tabs("tabs");
        if (tabs.length > 9 && tabs.length % 2 == 0) {
            $.messager.show(
                {
                    title: '提醒',
                    msg: '<b style="color:red;">你打开的页面太多，请关闭一些页面再打开，避免浏览器崩溃</b>'
                });
        }
        $.each(tabs, function (index, item) {
            if (item[0] != undefined && item[0].id == id) {
                isSelect = true;
                tabPanel.tabs("select", index);
                return;
            }
        })
        if (!isSelect) {          
            tabPanel.tabs('add', {
                title: name,
                id: id,
                iconCls: iconCls,
                content: fnCreateTabContent(tagUrl, name, paras, limit),
                closable: closable == false ? false : true,
                fit: true,
                selected: true
            });
        }
    }
}

//添加tab页面
function fnAddTab(options) {
    url = $.trim(options.url);
    if (url) {
        url = URL('/' + url);
        var tabPanel = $('#wrap-MainTabs');
        var isSelect = false;
        var tabs = tabPanel.tabs("tabs");
        $.each(tabs, function (index, item) {
            if (item[0] != undefined && item[0].id == options.id) {
                isSelect = true;
                tabPanel.tabs("select", index);
                return;
            }
        });
        if (!isSelect) {
            var tabOpts = $.extend({}, {
                id: options.id,
                content: fnCreateTabContent(url, options.text, paras),
                fit: true
            }, options || {});
            tabPanel.tabs('add', tabOpts);
        }
    }
};
//创建tab 内容
function fnCreateTabContent(url, name, paras, limit) {
    if ((/./).test(url)) {
        if (!(/\?/).test(url)) {
            url = url + '?p=1';
        } else if (!(/\?p=/).test(url) && !(/&p=/).test(url)) {
            url = url + '&p=1';
        }
    } else {
        // url = url.replace(/^\//, '');
    }
    if (paras) { url += paras; }

    createTopLoading();
    if (5 == limit) {
        return '<iframe onload="onIframeload();" limit="true" src="' + url + '" name="' + name + '" scrolling="auto" frameborder="0" style="width:100%;height:99%; overflow:hidden;" ></iframe>';
    } else {
        return '<iframe onload="onIframeload();" src="' + url + '" name="' + name + '" scrolling="auto" frameborder="0" style="width:100%;height:99%; overflow:hidden;" ></iframe>';
    }
}
//退出方法
function fnLoginOut() {
    $.getJSON(ActionURL.LoginOut, null, function (d) {
        if (d && d.success) {
            location.href = URL('/Home/Login');
        } else {
            alert('登出失败！请重新试一下');
        }
    });
}

//处理F5刷新
function fnHandlerRefresh() {  
    //setTimeout(function () {
    //    var title = getCookie('lrsmes_selecttab_title_' + moduleID + '_' + sysUserID);
    //    var id = getCookie('lrsmes_selecttab_id_' + moduleID + '_' + sysUserID);
    //    var icon = getCookie('lrsmes_selecttab_icon_' + moduleID + '_' + sysUserID);
    //    var url = getCookie('lrsmes_selecttab_url_' + moduleID + '_' + sysUserID);
    //    var fromCookie = false;
    //    if (url == null || url.length <= 0) {
    //        return;
    //    }
    //    addTab(title, url, icon, true, id, null, fromCookie);
    //}, 1000);  
}

//个人资料
function fnShowSelfInfo() {
    $.hDialog(
        {
            iconCls: 'icon-person',
            title: sysDisplayName + '的个人资料',
            width: 550,
            height: 400,
            href: HtmlTemplate.SelfInfo,
            onLoad: function () {
                $.hAjax(
               {
                   url: ActionURL.SelfInfo,
                   type: 'GET',
                   cache: false,
                   success: function (d) {
                       objPerson = d;
                       var $box = $('#wrap-personInfo');
                       if (d.ImageUrl) {
                           $box.find('#img_userIcon').attr('src', URL(d.ImageUrl));
                       }
                       var txt = '';
                       var val = null;
                       for (var item in d) {
                           val = d[item];
                           switch (item) {
                               case 'Gender':
                                   txt = val ? val : '男';
                                   break;
                               case 'RegisterTime':
                                   txt = ShortDate(val);
                                   break;
                               default:
                                   txt = val;
                                   break;
                           }
                           $box.find('span[name=' + item + ']').html(txt);
                       }
                   }
               });
            },
            submit: false,
            closable: true
        });
}

//密码修改弹出框
function fnShowResetPassword() {
    var $h = $.hDialog(
        {
            iconCls: 'icon-person',
            title: '密码修改',
            width: 400,
            height: 280,
            href: HtmlTemplate.ResetPass,
            submit: function () {
                var $fm = $h.find('form');
                if (!$fm.form('validate')) return;
                var send = $fm.serializeArray();
                send = convertArray(send);
                delete send.newPass2;
                top.$.hAjax
                    ({
                        url: ActionURL.ResetPass,
                        data: send,
                        type: 'POST',
                        success: function (d) {
                            if (d && d.success) {
                                $h.dialog('close');
                            }
                        }
                    });
            }
        });
}

//显示系统帮助
function fnShowSysHelp() {
    var $h = $.hDialog(
        {
            href: HtmlTemplate.SystemHelp,
            title: "系统帮助文档",
            iconCls: "icon-help",
            width: 400,
            height: 390,
            submit: false,
            closable: true
        }
    )
}

//是否弹出登录
var $loginDialog = null, isShowLogin = false;
//show login dialog
function ShowLogin() {
    if (isShowLogin) return;
    isShowLogin = true;
    $loginDialog = $.hDialog(
               {
                   title: '&nbsp;&nbsp;会话超时，请重新登录',
                   iconCls: 'icon-appkey',
                   width: 400,
                   height: 240,
                   closable: false,
                   href: HtmlTemplate.ReLogin,
                   onLoad: function () {
                       $.parser.parse($loginDialog);
                       $loginDialog.find('input[name=userName]')[0].focus();
                   },
                   submit: fnReLogin
               });

};
//submit login info
function fnReLogin() {
    var $box = $('#wrap_login');
    var $fm = $box.find('form');
    var $result = $box.find('#lab_loginResult');
    if (!$fm.form('validate')) { return; }
    var sendData = convertArray($fm.serializeArray());
    top.$.hAjax(
                     {
                         type: 'POST',
                         url: URL("/Home/Login"),
                         data: sendData,
                         dataType: 'json',
                         beforeSend: function () {
                             $result.html('登录中...');
                         },
                         success: function (d) {
                             if (d && d.success) {
                                 $loginDialog.dialog('close');
                                 isShowLogin = false;
                                 if (sysUserID == "") { location.reload(); }
                             } else {
                                 $result.html(d.message);
                             }
                         }
                     });
}