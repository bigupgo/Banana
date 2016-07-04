/*
  * 创建人：曹熠
  * 日 期：2014-5-1
  * 备 注：当前文件为 easyui插件扩展，包括弹出框、datagrid、tree等一系列扩展方法
          需要添加自己方法的童鞋，请写好备注(姓名、日期、功能)
  *
*/
(function ($) {
    function guidDialogId() {
        var s4 = function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };
        return "caoyi-" + (s4() + s4() + "-" + s4() + "-" + s4() + "-" + s4() + "-" + s4() + s4() + s4());
    }

    /***********************************弹出框扩展 开始****************************/
    //弹出框，Enter键触发确定事件
    var isOpen = false;
    var fnSubmit = null;
    $(document).unbind('keyup').bind('keyup',
	function (e) {
	    if (e.keyCode == 13 && isOpen && e.target.nodeName !== 'TEXTAREA' && $.isFunction(fnSubmit)) {
	        fnSubmit();
	    }
	});

    $.hDialog = function (options) {
        options = $.extend({},
		$.hDialog.defaults, options || {});
        var dialogId = guidDialogId();
        if (options.id) dialogId = options.id;
        var defaultBtn = [];

        if (options.submit) {
            defaultBtn.push({
                text: '确定',
                iconCls: 'icon-ok',
                handler: options.submit
            });
        }
        if (options.closable) {
            defaultBtn.push({
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#" + dialogId).dialog("close");
                }
            });
        }

        if (!options.showBtns) defaultBtn = [];

        if (options.buttons.length == 0) options.buttons = defaultBtn;

        if (options.max) {
            //dialog.dialog('maximize');
            var winWidth = $(window).width();
            var winHeight = $(window).height();
            options.width = winWidth - 20;
            options.height = winHeight - 20;
        }

        var $dialog = $('<div/>').css('padding', options.boxPadding).appendTo($('body'));

        var dialog = $dialog.dialog($.extend(options, {
            onClose: function () {
                isOpen = false;
                fnSubmit = null;
                dialog.dialog('destroy');
            }
        })).attr('id', dialogId);
        isOpen = true;
        fnSubmit = options.submit;
        if (options.HotKey == false) { //Enter 作为hotkey
            fnSubmit = null;
        };
        $dialog.find('.dialog-button').css('text-align', options.align);
        return dialog;
    };

    $.hDialog.defaults = $.extend({},
	$.fn.dialog.defaults, {
	    boxPadding: '3px',
	    align: 'right',
	    //按钮对齐方式
	    href: '',
	    id: '',
	    content: '',
	    height: 200,
	    width: 400,
	    collapsible: false,
	    minimizable: false,
	    maximizable: false,
	    closable: true,
	    modal: true,
	    shadow: false,
	    mask: true,
	    cache: false,
	    closed: false,
	    //默认是否关闭窗口 如果为true,需调用open方法打开
	    showBtns: true,
	    buttons: [],
	    submit: function () {
	        return false;
	    },
	    onBeforeClose: function () {
	        $(this).find(".combo-f").each(function () {
	            var panel = $(this).data().combo.panel;
	            panel.panel("destroy");
	        });
	        $(this).empty();
	    },
	    onMove: function (left, right) {
	        $('.validatebox-tip').remove();
	    }

	});

    $.hWindow = function (options) {
        var windowId = guidDialogId();

        options = $.extend({},
		$.hDialog.defaults, options || {});
        if (!options.href && !options.content) {
            alert('缺少必要的参数 href or content');
            return false;
        }

        var $dialog = $('<div/>').attr('id', windowId).appendTo($('body'));

        if (options.max) {
            //dialog.dialog('maximize');
            var winWidth = $(window).width();
            var winHeight = $(window).height();
            options.width = winWidth - 20;
            options.height = winHeight - 20;
        }

        var win = $dialog.window($.extend(options, {
            onClose: function () {
                win.window('destroy');
            }
        })).window('refresh').attr('id', windowId);

        return win;
    };

    $.hWindow.defaults = $.extend({},
	$.fn.window.defaults, {
	    href: '',
	    content: '',
	    height: 300,
	    width: 400,
	    collapsible: false,
	    //折叠
	    closable: true,
	    //显示右上角关闭按钮
	    minimizable: false,
	    //最小化
	    maximizable: false,
	    //最大化
	    resizable: false,
	    //是否允许改变窗口大小
	    title: '窗口标题',
	    //窗口标题
	    modal: true,
	    //模态	
	    draggable: true,
	    //允许拖动
	    max: false,
	    onBeforeClose: function () {
	        $(this).find(".combo-f").each(function () {
	            var panel = $(this).data().combo.panel;
	            alert(panel.html());
	            panel.panel("destroy");
	        });
	        $(this).empty();
	    }
	});
    //释放IFRAME内存
    $.fn.panel.defaults = $.extend({},
	$.fn.panel.defaults, {
	    onBeforeDestroy: function () {
	        var frame = $('iframe', this);
	        if (frame.length > 0) {
	            frame[0].contentWindow.document.write('');
	            frame[0].contentWindow.close();
	            frame.remove();
	            if ($.browser.msie) {
	                CollectGarbage();
	            }
	        }
	    }
	});
    /*+++++++++++++++++++++++++++++++++弹出框扩展 结束+++++++++++++++++++++++++++++++++*/

    /**********************************datagrid扩展 开始************************************/

    //扩展datagrid 方法 getSelectedIndex
    $.extend($.fn.datagrid.methods, {
        getSelectedIndex: function (jq) {
            var row = $(jq).datagrid('getSelected');
            if (row) return $(jq).datagrid('getRowIndex', row);
            else return -1;
        },
        checkRows: function (jq, idValues) {
            if (idValues && idValues.length > 0) {
                var rows = $(jq).datagrid('getRows');
                var keyFild = $(jq).datagrid('options').idField;
                $.each(rows,
				function (i, n) {
				    if ($.inArray(n[keyFild], idValues)) {
				        $(jq).datagrid('checkRow', row);
				    }
				})
            }
            return jq;
        }
    });

    /*+++++++++++++++++++++++++++++++++datagrid扩展 结束+++++++++++++++++++++++++++++++++*/

    /****************************tree扩展 开始***************************************/
    //tree 方法扩展 全选、取消全选
    $.extend($.fn.tree.methods, {
        checkedAll: function (jq, target) {
            var data = $(jq).tree('getChildren');
            if (target) data = $(jq).tree('getChildren', target);

            $.each(data,
			function (i, n) {
			    $(jq).tree('check', n.target);
			});
        }
    });

    $.extend($.fn.tree.methods, {
        uncheckedAll: function (jq) {
            var data = $(jq).tree('getSelected');
            $.each(data,
			function (i, n) {
			    $(jq).tree('uncheck', n.target);
			});
        }
    });

    //tree 方法扩展 全选、取消全选
    $.extend($.fn.tree.methods, {
        checkedAll: function (jq, target) {
            var data = $(jq).tree('getChildren');
            if (target) data = $(jq).tree('getChildren', target);

            $.each(data,
			function (i, n) {
			    $(jq).tree('check', n.target);
			});
        }
    });

    $.extend($.fn.tree.methods, {
        uncheckedAll: function (jq) {
            var data = $(jq).tree('getChildren');
            $.each(data,
			function (i, n) {
			    $(jq).tree('uncheck', n.target);
			});
        }
    });

    $.extend($.fn.tree.methods, {
        getParents: function (jq, target) {
            var a = [];
            var p = $(jq).tree('getParent', target);
            if (p != null) {
                a.push(p.id);
                _getTreeParents(jq, p.target, a);
            }
            return a;
        }
    });

    function _getTreeParents(jq, target, arr) {
        var p = $(jq).tree('getParent', target);
        if (p != null) {
            arr.push(p.id);
            _getTreeParents(jq, p.target, arr);
        }
    }
    /*******************datagrid的扩展****************************/
    //editor时间选择
    $.extend($.fn.datagrid.defaults.editors, {
        datetimebox: { //datetimebox就是你要自定义editor的名称
            init: function (container, options) {
                var input = $('<input class="easyuidatetimebox">').appendTo(container);
                return input.datetimebox({
                    formatter: function (date) {
                        return new Date(date).format("yyyy-MM-dd hh:mm:ss");
                    }
                });
            },
            getValue: function (target) {
                return $(target).parent().find('input.combo-value').val();
            },
            setValue: function (target, value) {
                $(target).datetimebox("setValue", value);
            },
            resize: function (target, width) {
                var input = $(target);
                if ($.boxModel == true) {
                    input.width(width - (input.outerWidth() - input.width()));
                } else {
                    input.width(width);
                }
            }
        }
    });

    //editor中icon图标的选择
    $.extend($.fn.datagrid.defaults.editors, {
        iconbox: { //
            init: function (container, options) {
                var $input = $('<input type="hidden" name="hd-rowSelectIcon" value="ok" />').appendTo(container);
                var $a = $('<a href="javascript:void(0);" >选择图标</a>').appendTo(container);
                return $a.iconSelect({
                    iconHref: URL('/Content/icon.htm'),
                    onSelect: function (n) {
                        $input.val(n);
                    },
                    icon: 'icon-ok'
                });
            },
            getValue: function (target) {
                return $(target).parent().find('input[name=hd-rowSelectIcon]').val();
            },
            setValue: function (target, value) {
                return $(target).parent().find('input[name=hd-rowSelectIcon]').val(value);
            },
            resize: function (target, width) {
                var input = $(target);
                if ($.boxModel == true) {
                    input.width(width - (input.outerWidth() - input.width()));
                } else {
                    input.width(width);
                }
            }
        }
    });

    /*datagrid editor 扩展combogrid*/
    $.extend($.fn.datagrid.defaults.editors, {
        combogrid: {
            init: function (container, options) {
                var input = $('<input type="text" class="datagrid-editable-input">').appendTo(container);
                input.combogrid(options);
                return input;
            },
            destroy: function (target) {
                $(target).combogrid('destroy');
            },
            getValue: function (target) {
                var g = $(target).combogrid('grid');
                var r = g.datagrid('getSelected');
                var idField = $(target).combogrid('options').idField;
                return r[idField];
            },
            setValue: function (target, value) {
                $(target).combogrid('setValue', value);
            },
            resize: function (target, width) {
                $(target).combogrid('resize', width);
            }
        }
    });

    /*+++++++++++++++++++++++++++++++++tree扩展 结束+++++++++++++++++++++++++++++++++*/

    /****************************combobox扩展 开始***************************************/

    //扩展 combobox 方法 selectedIndex
    $.extend($.fn.combobox.methods, {
        selectedIndex: function (jq, index) {
            if (!index) index = 0;
            var data = $(jq).combobox('options').data;
            var vf = $(jq).combobox('options').valueField;
            $(jq).combobox('setValue', eval('data[index].' + vf));
        }
    });
    /*+++++++++++++++++++++++++++++++++combobox扩展 结束+++++++++++++++++++++++++++++++++*/

})(jQuery);