//$.fn.datagrid.defaults = $.extend($.fn.datagrid.defaults,
//    {
//        cache: false,
//        method:'POST'
//    });

//设置DataGrid默认值
//请求
$.fn.datagrid.defaults.method = "POST";
//全屏
$.fn.datagrid.defaults.fit = true;
//无边框
$.fn.datagrid.defaults.border = false;
//单选
$.fn.datagrid.defaults.singleSelect = true;
//选中一行,则勾选中复选框
//$.fn.datagrid.defaults.checkOnSelect = false;
//单击一个复选框，将始终选择行
$.fn.datagrid.defaults.selectOnCheck = false;
//带行高
$.fn.datagrid.defaults.rownumbers = true;
//是否分页
$.fn.datagrid.defaults.pagination = true;
//默认每页显示行数
$.fn.datagrid.defaults.pageSize = 15;
//选择每页显示行数
$.fn.datagrid.defaults.pageList = [15, 30, 60];
//自动设置高度设置为false,可提高性能
$.fn.datagrid.defaults.autoRowHeight = false;
//交替显示行背景
$.fn.datagrid.defaults.striped = true;
//查不到数据时,默认提示
$.fn.datagrid.defaults.onLoadSuccess = function(data) {
    if (data.rows == null || data.rows.length == 0) {
        $.messager.show({
            title: "提示",
            msg: "当前查到0条数据!"
        });
    }
}

//设置Combogrid默认值
//是否分页
$.fn.combogrid.defaults.pagination = false;
//交替显示行背景
$.fn.combogrid.defaults.striped = true;
//mode
$.fn.combogrid.defaults.mode = 'remote';

//设置Dialog默认值
//在本侧内显示对话框
$.fn.dialog.defaults.inline = true;
$.fn.dialog.defaults.resizable = true;
$.fn.dialog.defaults.modal = true
$.fn.dialog.defaults.closed = true;
$.fn.dialog.defaults.width = 260;

//设置PropertyGrid默认值
//全屏
$.fn.propertygrid.defaults.fit = true;
//无下拉
$.fn.propertygrid.defaults.scrollbarSize = 0;
//不显示头
$.fn.propertygrid.defaults.showHeader = false;

//设置combobox的默认值
$.fn.combobox.defaults.width = 155;

//设置linkbutton的默认值
$.fn.linkbutton.defaults.plain = true;

/*
//设置TreeGrid默认值
//设置节点值为id字段
$.fn.treegrid.defaults.idField = "id";
//设置节点值为字段
$.fn.treegrid.defaults.treeField = "name";*/
//设置为单选
$.fn.treegrid.defaults.singleSelect = true;
//设置动画效果为true
$.fn.treegrid.defaults.animate = true;
//全屏
$.fn.treegrid.defaults.fit = true;
//无边框
$.fn.treegrid.defaults.border = false;
//行号
$.fn.treegrid.defaults.rownumbers = true;
//多选
$.fn.treegrid.defaults.singleSelect = false;
//选中一行,则勾选中复选框
$.fn.treegrid.defaults.checkOnSelect = false;
//单击一个复选框，将始终选择行
$.fn.treegrid.defaults.selectOnCheck = false;

//设置布局
$.fn.layout.defaults.fit = true;
$.fn.layout.defaults.border = false;

$.fn.tree.defaults.lines = true;
$.fn.tree.defaults.checkbox = true;

//下拉
$.fn.combo.defaults.editable = false;

$.fn.combotree.defaults.lines = true;

$.fn.tabs.defaults.border = false;
$.fn.tabs.defaults.fit = true;



/** 
*解决linkbutton组件disable方法无法禁用jQuery绑定事件的问题
* linkbutton方法扩展  
* @param {Object} jq 
*/
$.extend($.fn.linkbutton.methods, {
    /**  
    * 激活选项（覆盖重写）  
    * @param {Object} jq  
    */
    enable: function (jq) {
        return jq.each(function () {
            var state = $.data(this, 'linkbutton');
            if ($(this).hasClass('l-btn-disabled')) {
                var itemData = state._eventsStore;
                //恢复超链接  
                if (itemData.href) {
                    $(this).attr("href", itemData.href);
                }

                //回复点击事件  
                if (itemData.onclicks) {
                    for (var j = 0; j < itemData.onclicks.length; j++) {
                        $(this).bind('click', itemData.onclicks[j]);
                    }
                }

                //设置target为null，清空存储的事件处理程序 
                itemData.target = null;
                itemData.onclicks = [];
                $(this).removeClass('l-btn-disabled');
            }
        });
    },
    /**  
    * 禁用选项（覆盖重写） 
    * @param {Object} jq  
    */
    disable: function (jq) {
        return jq.each(function () {
            var state = $.data(this, 'linkbutton');
            if (!state._eventsStore)
                state._eventsStore = {};
            if (!$(this).hasClass('l-btn-disabled')) {
                var eventsStore = {};
                eventsStore.target = this;
                eventsStore.onclicks = [];
                //处理超链接  
                var strHref = $(this).attr("href");
                if (strHref) {
                    eventsStore.href = strHref;
                    $(this).attr("href", "javascript:void(0)");
                }

                //处理直接耦合绑定到onclick属性上的事件 
                var onclickStr = $(this).attr("onclick");
                if (onclickStr && onclickStr != "") {
                    eventsStore.onclicks[eventsStore.onclicks.length] = new Function(onclickStr);
                    $(this).attr("onclick", "");
                }

                //处理使用jquery绑定的事件  
                var eventDatas = $(this).data("events") || $._data(this, 'events');
                if (eventDatas["click"]) {
                    var eventData = eventDatas["click"];
                    for (var i = 0; i < eventData.length; i++) {
                        if (eventData[i].namespace != "menu") {
                            eventsStore.onclicks[eventsStore.onclicks.length] = eventData[i]["handler"];
                            $(this).unbind('click', eventData[i]["handler"]);
                            i--;
                        }
                    }
                }

                state._eventsStore = eventsStore;
                $(this).addClass('l-btn-disabled');
            }
        });
    }
});


/*扩展*/
/**
 * 合并单元格
 * groupfields:分组的key,自由合并的字段,不受keyfields的影响;
 * keyfields:最小粒度的key;
 * fields:其他需要分组的field,受到keyfields的影响.
 **/
$.extend($.fn.datagrid.methods, {
    autoMergeCells: function (jq, options) {
        var groupfields = options.groupfields;
        var keyfield = options.keyfield;
        var fields = options.fields;

        return jq.each(function () {
            var target = $(this);

            if (!groupfields) {
                groupfields = target.datagrid("getColumnFields", true);
                groupfields = groupfields.concat(target.datagrid("getColumnFields"));
            }
            var rows = target.datagrid("getRows");

            var temp = {};
            for (var i=0; i < rows.length; i++) {
                var row = rows[i];

                for (var j = 0; j < groupfields.length; j++) {
                    var f = groupfields[j];
                    var tf = temp[f];
                    if (!tf) {
                        tf = temp[f] = {};
                        tf[row[f]] = [i];
                    } else {
                        var tfv = tf[row[f]];
                        if (tfv) {
                            tfv.push(i);
                        } else {
                            tfv = tf[row[f]] = [i];
                        }
                    }
                }
            }

            //给剩余分组字段分组数赋值.
            $.each(fields, function(i, n) {
                temp[n] = temp[keyfield];
            });

            $.each(temp, function (f, colunm) {
                $.each(colunm, function () {
                    var group = this;
                    
                    if (group.length > 1) {
                        var before,
                        after,
                        megerIndex = group[0];
                        for (var i = 0; i < group.length; i++) {
                            before = group[i];
                            after = group[i + 1];
                            if (after && (after - before) == 1) {
                                continue;
                            }
                            var rowspan = before - megerIndex + 1;
                            if (rowspan > 1) {
                                target.datagrid('mergeCells', {
                                    index: megerIndex,
                                    field: f,
                                    rowspan: rowspan
                                });
                            }
                            if (after && (after - before) != 1) {
                                megerIndex = after;
                            }
                        }
                    }
                });
            });
        });
    }
});