(function ($) {
    var $dialog, $upload;
    //上传插件默认值
    var defaultUplOpts =
        {
            'debug': false,
            'auto': false,
            'buttonText': '选择文件',
            'swf': URL("/Scripts/Uploadify/js/uploadify/uploadify.swf"),
            'queueID': 'uploadfileQueue',
            'width': '75',
            'height': '24',
            'multi': true,
            'fileTypeDesc': '支持的格式：',
            'fileSizeLimit': '50MB',
            'removeTimeout': 1,
         
            //返回一个错误，选择文件的时候触发  
            'onSelectError': function (file, errorCode, errorMsg) {
                switch (errorCode) {
                    case -100:
                        alert("上传的文件数量已经超出系统限制的" + $upload.uploadify('settings', 'queueSizeLimit') + "个文件！");
                        break;
                    case -110:
                        alert("文件 [" + file.name + "] 大小超出系统限制的" + $upload.uploadify('settings', 'fileSizeLimit') + "大小！");
                        break;
                    case -120:
                        alert("文件 [" + file.name + "] 大小异常！");
                        break;
                    case -130:
                        alert("文件 [" + file.name + "] 类型不正确！");
                        break;
                }
            },
            //检测FLASH失败调用  
            'onFallback': function () {
                alert("您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。");
                $dialog.dialog('close');
            }
        };

    //弹出层默认值
    var defaultDiaOpts =
        {   
            iconCls: 'icon-upload',
            title: '&nbsp;&nbsp;&nbsp;上传文档',
            href: URL('/Scripts/Bus/Base/html/upload.html'),
            width: 450,
            height: 500,
            cache: true,
            buttons: [
                {
                    text: '取消上传',
                    iconCls: 'icon-cross',
                    handler: function () {
                        $upload.uploadify('cancel', '*');
                        $dialog.dialog('close');
                    }
                }
               
            ],
            submit: false,
            onBeforeClose: function () { $upload.uploadify('destroy'); }
        };

    $.extend(
        {
            SWFUpload:swfUpload
        });

     function swfUpload(dialogOptions, uploadifyOptions) {
         var obj = {};
         var diaOpts = $.extend({},defaultDiaOpts, dialogOptions || {});
         var uplOpts = $.extend({},defaultUplOpts, uploadifyOptions || {});
        if (!uplOpts.auto)
        {
            if (diaOpts.buttons.length == 1) {
                diaOpts.buttons.push({
                    text: '开始上传',
                    iconCls: 'icon-upload',
                    handler: function () {
                        $upload.uploadify('upload', '*');
                    }
                });
            }
        }
        $dialog = top.$.hDialog(
            $.extend
            (diaOpts,
            {
                onLoad: function () {
                    $upload = $dialog.find('#file_upload').uploadify(uplOpts);
                }
            }));

        //获取弹出框
        obj.GetDialog = function () {
            return $dialog;
        };

        //获取uploadify对象
        obj.GetUploadify = function () {
            return $upload;
        };
        return obj;
    };
})(jQuery);
