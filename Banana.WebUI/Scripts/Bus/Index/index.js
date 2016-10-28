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
    $.ajax({
        url: ActionURL.GetFirstMenu,
        type: "post",
        success: function (res) {
            var topMentHtml = "";
            if (res != null) {
                $.each(res, function (index, obj) {
                    topMentHtml += "<li><a onclick=\"loadSonMenu('" + obj.ID + "','" + obj.ModelName + "'," + obj.Type + ",'" + obj.IconCls + "','" + obj.Url + "')\" href='javascript:void(0);'>" + obj.ModelName + "</a></li>";
                });
            }
            $box.html(topMentHtml);
        }
    });
}

var _moduleDefaultId;
//加载子菜单
function loadSonMenu(pid, text, type, iconCls, url) {
    moduleID = pid;
    $('#wrap_west').panel('setTitle', text);
    $('body').layout('expand', 'west');
    var memuHtml = GetMenueHtml(pid);
    $('#ul_menuTree').html(memuHtml);
    bindMedioClick();//绑定菜单点击事件
}

//获取菜单HTML代码
function GetMenueHtml(pid) {
    var sonMenuHtml = "";
    $.ajax({
        url: ActionURL.GetOtherMenu,
        async: false,
        type: "post",
        data: { pid: pid },
        success: function (res) {
            if (res != null) {
               
                $.each(res, function (index, obj) {
                    if (obj.Type == 0) {
                        sonMenuHtml += "<li>"
                                             + "<a href='javascript:void(0);' style='text-decoration:none' class='dropdown-toggle'><i class='fa fa-list-alt'></i>人员管理<b class='fa fa-angle-down'></b></a>"
                                             + "<ul class='submenu'>";
                        $.ajax({
                            url: ActionURL.GetOtherMenu,
                            type: "post",
                            async: false,
                            data: { pid: obj.ID },
                            success: function (childres) {
                                if (childres != null) {
                                    $.each(childres, function (i, childObj) {
                                        if (childObj.Type == 1) {
                                            sonMenuHtml += "<li class='childmenu' text='" + childObj.ModelName + "' limit='5' url='" + childObj.Url + "' iconCls='" + childObj.IconCls + "' pid='" + childObj.ID + "' ><a href='javascript:void(0);' style='text-decoration:none'><i class='" + childObj.IconCls + "'></i>" + childObj.ModelName + "</a></li>";
                                        }
                                        if (childObj.Type == 0) {
                                            GetMenueHtml(childObj.ID);
                                        }
                                    })
                                }
                            }
                        })
                        sonMenuHtml += "</ul>"
                                    + "</li>";
                    }
                    if (obj.Type == 1) {
                        sonMenuHtml += "<li class='childmenu' text='" + obj.ModelName + "' limit='5' url='" + obj.Url + "' iconCls='" + obj.IconCls + "' pid='" + obj.ID + "' ><a href='javascript:void(0);' style='text-decoration:none'><i class='" + obj.IconCls + "'></i>" + obj.ModelName + "</a></li>";
                    }
                });            
            }
        }
    });
    return sonMenuHtml;
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