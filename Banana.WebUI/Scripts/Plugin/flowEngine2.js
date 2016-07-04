
/*
    插件名称：   流程引擎 v1.0
    简  介 ：
    作  者 ：      caoyi
    创建日期：   14-8-10
    最后修改日期：14-8-10
*/
(function ($) {
    var _this = null;
    var _$wraps = null;
    var _isFinish = false;  //流程是否结束

    var defaultOpts =
        {
            itemCls: "wrap-item",         //每一项共同的class名称，下面的方法需要根据此名称查找
            doneCls: 'wrap-item-done',    //已完成节点class名
            undoneCls: 'wrap-item-undone', //未完成节点class名
            doingCls:'wrap-item-doing',    //正在进行节点样式
            ProcessedNo:['no-0'],          //正在处理的节点
            SwitchCls: ['switch1', 'switch2'], //正在处理节点切换类
            SwitchInterval: 500,                //正在处理节点切换间隔时间
            fnFinish: function (no) { alert('流程在'+no+'节点结束了'); }
        };
    var opt = null;

    $.fn.FlowEngine = function (options) {
        opt = $.extend({}, defaultOpts, options || {});
        _this = this;
        _$wraps = this.find('.'+opt.itemCls);
        _initNodes(opt.ProcessedNo);

        //根据当前正在处理节点刷新
        _this.Read = function (ProcessedNo)
        {
            _this.find('.'+opt.doingCls).each(function () {
                var $t = $(this);
                var timer = $t.data('timer');
                if (timer) { clearInterval(timer); $t.data('timer', 0); }
                $t.removeClass(opt.doingCls);
                $t.removeClass(opt.SwitchCls[0]);
                $t.removeClass(opt.SwitchCls[1]);
            });
            _this.find('.' + opt.doneCls).removeClass(opt.doneCls);
            _this.find('.' + opt.undoneCls).removeClass(opt.undoneCls);
            _initNodes(ProcessedNo);
        };

        //获取已经完成节点
        _this.GetDoneItems = function () {
            var divs = _this.find('.' + opt.doneCls);
            var a = [];
            divs.each(function () { a.push(this.getAttribute('id')); });
            return a;
        };
        //获取未完成节点
        _this.GetUnDoneItems = function () {
            var divs = _this.find('.' + opt.undoneCls);
            var a = [];
            divs.each(function () { a.push(this.getAttribute('id')); });
            return a;
        };
        //获取当前正在处理的节点
        _this.GetProcessingNo = function () {
            var a = [];
            _this.find('.' + opt.doingCls).each(function () {
                a.push(this.getAttribute('id'));
            });
            return a;
        };

        //提供下次进入时候初始化ProcessedNo参数
        //获取上一步处理完成的节点
        _this.GetProcessedNode = function () {
            var a = [];
            _this.find('.' + opt.doingCls).each(function () {
                a.push(this.getAttribute('pid'));
            });
            _this.find('.' + opt.doneCls).each(function ()
            {
                var _t=$(this);
                if (!$('div[pid=' + _t.attr('id') + ']').length)
                {
                    a.push(_t.attr('id'));
                }
            });
            return _arrayUnique(a);
        };
        //进入 下一步
        _this.GoToNext = function (no)
        {
            var curr = _this.find('#' + no);
            if (curr.attr('judge')) {
                var judgeBrothers = _this.find('div[judge=' + curr.attr('judge') + '][id!='+no+']');
                judgeBrothers.each(function ()
                {
                    var $j = $(this);
                    var timer = $j.data('timer');
                    if (timer) { clearInterval(timer); }
                    $j.removeClass(opt.SwitchCls[0]);
                    $j.removeClass(opt.SwitchCls[1]);
                    $j.removeClass(opt.doingCls);
                    $j.addClass(opt.doneCls);
                });
            };

            //计时器
            var timer=curr.data('timer');
            if (timer) { clearInterval(timer); }
            curr.removeClass(opt.SwitchCls[0]);
            curr.removeClass(opt.SwitchCls[1]);
            curr.removeClass(opt.doingCls);
            curr.addClass(opt.doneCls);
            //子节点
            var nextNodes = _this.find('div[pid=' + no + ']');
            if (nextNodes.length > 0) {  //子节点
                nextNodes.each(function () {
                    var _$this = $(this);
                    _$this.removeClass(opt.undoneCls);
                    _decorateDoing(_$this);
                });
            } else  //子节点可能是汇集节点
            {
                nextNodes = _this.find('div[pid*=' + no + ']'); //root节点
                if (nextNodes.length == 1) {
                    _handlerRootNode(nextNodes.eq(0));
                };
            }
        };
        //结束流程
        _this.Complete = function ()
        {
            _this.find('.' + opt.doingCls).each(function ()
            {
                var _t = $(this);
                var timer = _t.data('timer');
                if (timer) { clearInterval(timer); }
                _t.removeClass(opt.doingCls);
                _t.removeClass(opt.SwitchCls[0]);
                _t.removeClass(opt.SwitchCls[1]);
                _t.addClass(opt.undoneCls);
            });
        }

        return _this;
    };

    //初始化已完成节点
    function _initNodes(ProcessedNo) {
        if (!$.isArray(ProcessedNo)) {
            $.error = "ProcessedNo 属性参数必须为数组！";
        };

        //开始节点
        if (ProcessedNo.length == 1 && $('#' + ProcessedNo[0]).length == 0) {
            _decorateDoing($('#no-1'));
        } else {
            //已完成的变色
            $.each(ProcessedNo, function () {
                _renderHasDone(this);
            });
            //正在进行的
            $.each(ProcessedNo, function () {
                var _$doing=this;
                var nextNodes = _this.find('div[pid=' + _$doing + ']');
                if (nextNodes.length > 0) { //直接找到
                    nextNodes.each(function () {
                        _decorateDoing($(this));
                    });
                } else //汇集到一个下级节点
                {
                    nextNodes = _this.find('div[pid*=' + this + ']'); //root节点
                    if (nextNodes.length == 1) {
                        _handlerRootNode(nextNodes.eq(0));
                    };
                }

            });
        }
        //未完成的
        _$wraps.each(function ()
        {
            var _$this = $(this);
            if (_$this.hasClass(opt.doneCls) || _$this.hasClass(opt.doingCls)) {

            } else
            {
                _$this.addClass(opt.undoneCls);
            }
        });

        function _renderHasDone(id) {
            var _$this = $('#' + id);
            _$this.addClass(opt.doneCls);
            var pid = _$this.attr('pid');
            while (pid)
            {
                if (!pid) { break; }
                var pid = pid.split(',');
                if (pid.length == 1) {
                    pid = pid[0];
                    var $p = $('#' + pid);
                    if (!$p.length) { break; }
                    if ($p.hasClass(opt.doneCls)) { break; }
                    pid = $p.attr('pid');
                    $p.addClass(opt.doneCls);
                    if (!pid) { break; }
                } else
                {
                    $.each(pid, function () {
                        _renderHasDone(this);
                    });
                    break;
                }
            }
        };
    }

    //处理树根节点 ,比如 no-6-1、no-6-2、no-6-3汇集到一个root为no-7的节点
    function _handlerRootNode($root) {
        var thisPids = $root.attr('pid').split(',');//上级nodes id
        var allOK = true;
        $.each(thisPids, function () {
            var _pre = $('#' + this);
            if (_pre.hasClass(opt.undoneCls) || _pre.hasClass(opt.doingCls)) {
                allOK = false;
                return false;
            }
        });
        if (allOK) {
            $root.removeClass(opt.undoneCls);
            _decorateDoing($root);
        }
    };
    //修饰正在处理节点
    function _decorateDoing($item) {
        //if (typeof $item.data('timer') != 'undefined') { return; }
        if ($item.hasClass(opt.doneCls)) { return; }
        $item.addClass(opt.doingCls);
        var timer = setInterval(function () {
            if ($item.hasClass(opt.SwitchCls[0])) {

                $item.removeClass(opt.SwitchCls[0]);
                $item.addClass(opt.SwitchCls[1]);

            } else {
                $item.removeClass(opt.SwitchCls[1]);
                $item.addClass(opt.SwitchCls[0]);
            }

        }, opt.SwitchInterval);
        $item.data('timer', timer);
    };
    //数组去重
    function _arrayUnique(arr) {
        var result = [], hash = {};
        for (var i = 0, elem; (elem = arr[i]) != null; i++) {
            if (!hash[elem]) {
                result.push(elem);
                hash[elem] = true;
            }
        }
        return result;
    }
})(jQuery);
