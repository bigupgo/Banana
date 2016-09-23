var moduleID = null;
//窗体加载
$(document).ready(function () {

    $('#sysName').html("");
    addTab("首页", 'Home/Home', 'icon-house', false);
    $(".curt-user").click(function () {
        var $left = $(this).offset().left - 50;
        $("#downBtns").toggle("fast").css("left", $left).mouseleave(function () {
            $(this).hide();
        })
    })
    initFirstMenu();
});

//加载第一层菜单
function initFirstMenu() {
    var $box = $('#ul_FirstMenu');
    var topMentHtml = "<li><a onclick=\"loadSonMenu('page1','null','人员管理',1,'icon-blue-folder-close','null')\" href='javascript:void(0);'>人员管理</a></li>"
                    + "<li><a onclick=\"loadSonMenu('page2','null','巡查管理',1,'icon-blue-folder-close','null')\" href='javascript:void(0);'>巡查管理</a></li>";
    $box.html(topMentHtml);
}

var _moduleDefaultId;
//加载子菜单
function loadSonMenu(pid, defaultId, text, type, iconCls, url) {
    moduleID = pid;
    $('#wrap_west').panel('setTitle', text);

    $('body').layout('expand', 'west');

    var sonMideoHtml = "";
    if (pid == "page1") {
        sonMideoHtml = "<li class='childmenu' text='会员管理' limit='5' url='User/User/Index' iconCls='fa fa-list-alt' pid='page11' ><a href='javascript:void(0);' style='text-decoration:none'><i class='fa fa-list-alt'></i>会员管理</a></li>"
                     + "<li>"
                      + "<a href='javascript:void(0);' style='text-decoration:none' class='dropdown-toggle'><i class='fa fa-list-alt'></i>权限管理<b class='fa fa-angle-down'></b></a>"
                      + "<ul class='submenu'>"
                       + "<li class='childmenu' text='角色授权' limit='5' url='Information/DeptInformation/Design' iconCls='fa fa-list-alt' pid='page12' ><a href='javascript:void(0);' style='text-decoration:none'><i class='fa fa-list-alt'></i>角色授权</a></li>"
                      + "</ul>"
                     + "</li>";
    }

    if (pid == "page2") {

        sonMideoHtml = "<li class='childmenu' text='巡查管理' limit='5' url='Information/Regulator/Index' iconCls='fa fa-list-alt' pid='page21' ><a href='javascript:void(0);' style='text-decoration:none'><i class='fa fa-list-alt'></i>巡查管理</a></li>"
    }



    $('#ul_menuTree').html(sonMideoHtml);
    bindMedioClick();//绑定菜单点击事件

}

//绑定菜单点击事件
function bindMedioClick(){
    //绑定单击
    $('#ul_menuTree .childmenu').unbind('click').bind('click', function () {
        var $this = $(this);
        $('#ul_menuTree .childmenu').removeClass('active');
        $this.addClass('active');

        if ($this.attr('pid') == "cc2afc151c964f6f98e9957fcab2fae9") {
            window.open($this.attr('url'));
        } else {
            var attr = { url: $this.attr('url'), limit: $this.attr('limit') };
            top.addTab($this.attr('text'), attr, $this.attr('iconCls'), true, $this.attr('pid'));
        }
    });
}

//tree Click事件
function fnMenuTreeSelected(node) {
    //表示为模块组。
    if (node.attributes.type == 1) {
        return;
    }
    top.addTab(node.text, node.attributes, node.iconCls, true, node.id);
}

    //当导航树加载结束，打开默认页
    function fnOnTreeLoadeSuccess(node, data) {
        var selectNode = $('#ul_menuTree').tree('find', _moduleDefaultId);
        if (selectNode != undefined) {
            fnMenuTreeSelected(selectNode);
        }
    }

    //单击事件搜索
    function fnSearchModule(v, n) {
        v = $.trim(v);
        if (!v) { $('#wrap-searchResult').tree('loadData', {}); return; }
        $.hAjax(
            {
                data: { search: v },
                url: URL('Base/Common/SearchModules'),
                success: function (d) {
                    $('#wrap-searchResult').tree('loadData', d);
                }
            });
    }



    function paramEncode(user,pwd)
    {
        var UserObj = JSON.stringify({ username: user, password: pwd });
        return $.md5($.base64.encode(UserObj)) + $.base64.encode(UserObj);
    }