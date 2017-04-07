var projectList = [];
$(function () {
    //绑定提交事件
    $("#showTooltips").click(function () {
        AddLog();
    });

    //绑定确定事件
    $("#projectName").click(function () {
        CheckPro();
    });

    //查询事件
    $("#searchId").click(function () {
        $('#loadingToast .weui_toast_content').html("查询中..");
       $('#loadingToast').show();
      
      
        var $weuiSearchBar = $('#search_bar');
        $weuiSearchBar.removeClass('weui_search_focusing');
        var searchText = $("#search_input").val();
        selProject(searchText);

        if ($(this).val()) {
            $('#search_text').hide();
        } else {
            $('#search_text').show();
        }
    });

    //搜索绑定
    $('form').on('focus', '#search_input', function () {
        var $weuiSearchBar = $('#search_bar');
        $weuiSearchBar.addClass('weui_search_focusing');
    }).on('input', '#search_input', function () {
        var $searchShow = $("#search_show");
        if ($(this).val()) {
            $searchShow.show();
        } else {
            $searchShow.hide();
        }
    }).on('touchend', '#search_cancel', function () {
        $("#search_show").hide();
        $('#search_input').val('');
    }).on('touchend', '#search_clear', function () {
        $("#search_show").hide();
        $('#search_input').val('');
    });

    $('#loginDilog').on('click', '.weui_btn_dialog', function () {
        var logName = $("#loginName").val();
        var password = $("#password").val()
        if (logName == "" || password == "") {
            $("#checkmsgId").html("用户名密码不能为空!");
            $('.js_tooltips').show();
        } else {
            saveUser(logName, password);
        }       
    });

    InitData();
});

function saveUser(loginName, password) {
    $('#loadingToast .weui_toast_content').html("保存中..");
    $('#loadingToast').show();
    $.ajax({
        url: URL("/Blogwx/BlogUserAdd"),
        type: "post",
        data: { BlogName: loginName, BlogPassword: password },
        success: function (res) {    
            $('#loginDilog').off('click').hide();
            $('#loadingToast').hide();
            MyBLog();
        }, error: function () {
            $('#loadingToast').hide();
            $("#toastId").attr("class", "weui_icon_msg weui_icon_warn");
            $("#submsgId").html("服务器异常");
            $('#toast').show();
            setTimeout(function () {
                $('#toast').hide();
            }, 2000);
        }
    });
}

//初始化值
function InitData() {
    var nowDate = new Date();
    $("input[name='blogDate']").val(nowDate.Format("yyyy-MM-dd"));
    $("input[name='beginTime']").val("08:30");
    $("input[name='endTime']").val("18:00");
    BindProject();
    MyBLog();
}

//设置选择项目
function selPro(projectId, name) {
    $("#projectName").html(name);
    $("#projectId").val(projectId);
    Back();
}

//预先加载
function BindProject() {
    if (projectList.length == 0) {
        $.ajax({
            url: URL("/Blogwx/GetProjects"),
            type: "post",
            success: function (res) {
            }
        });
    }
}

//筛选项目
function selProject(search) {
    if (search == "") {
        $('#loadingToast').hide();
        return;
    }
      $.ajax({
        url: URL("/Blogwx/SearchProject"),
        type: "post",
        data:{search:search},
        success: function (res) {
            if (res != null) {
                var items = "";
                res = JSON.parse(res);
                $.each(res, function (i, obj) { 
                    items += "<div class='weui_cell'>"
                    + "<div class='weui_cell_bd weui_cell_primary'>"
                    + " <p onclick=\"selPro('" + obj.projectID + "','" + obj.projectName + "')\">" + obj.projectID + '&nbsp;&nbsp;' + obj.projectName.substring(0, 5) + "</p>"
                    + "</div>"
                    + "</div>";
                });
                $("#search_show").html(items);
            }
            $('#loadingToast').hide();
        }
    });  
}

//统计已经输入字数
function countSize(obj) {
    var num = $(obj).val().length;
   $("#numId").html(num);
}

//添加日志
function AddLog() {

    if (!CheckValue()) {
        return;
    }

    $('#loadingToast').show();

    var $fm = $('form');
    //if (!$fm.form('validate')) return;
    var sendData = $fm.serializeArray();
    sendData = convertArray(sendData);
    $.ajax({
        url: URL("/Blogwx/Add"),
        type: "post",
        data: sendData,
        success: function (res) {
            MyBLog();
            res = JSON.parse(res);
            $('#loadingToast').hide();
            if (res.success) {
                $("#toastId").attr("class", "weui_icon_toast");
            } else {
                $("#toastId").attr("class", "weui_icon_msg weui_icon_warn");
            }
            $("#submsgId").html(res.message);
            $('#toast').show();
            setTimeout(function () {
                $('#toast').hide();
            }, 2000);
        }
    });
}

//主要是推荐这个函数。它将jquery系列化后的值转为name:value的形式。
function convertArray(o) {
    var v = {};
    for (var i in o) {
        if (o[i].name != '__VIEWSTATE') {
            if (typeof (v[o[i].name]) == 'undefined')
                v[o[i].name] = o[i].value;
            else
                v[o[i].name] += "," + o[i].value;
        }
    }
    return v;
}

function Back() {
    $('#second').hide();
    $('#first').show();
}

function CheckPro() {
    $('#second').show();
    $('#first').hide();
}

//绑定最新日志
function MyBLog() {
    $.ajax({
        url: URL("/Blogwx/GetNewBlog"),
        type: "post",
        success: function (data) {
            if (data != null) {
                var d = data.blogDate;
                var projectId = data.projectID;
                var proName = data.projectName;
                $("#dateShow").html(d);
                $("#proNameShow").html(proName);
     
                //绑定上次填写日志
                $("#projectName").html(proName.split(",")[1]);
                $("#projectId").val(projectId);

            }
        }
    });
}

//表单验证
function CheckValue() {
    var proId = $("#projectId").val();
    var blogContext = $("#blogcontext").html();
    var msg = "";
    if (proId == "") {
        msg = "请先选择项目";
    }
    if (msg != "") {
        $("#checkmsgId").html(msg);
        $('.js_tooltips').show();
        setTimeout(function () {
            $('.js_tooltips').hide();
        }, 3000);
        return false;
    }
    return true;
}