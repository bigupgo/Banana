var $datagrid;
var iDialogW = 560, iDialogH = 350;
var preIndex = null;
var $mineTree; var arrayId = [];

//操作逻辑preIndex
var baseActionPath = URL('/Weixin/Food/');
var ActionURL =
{
    Read: baseActionPath + 'GetList',
    Add: baseActionPath + 'Add',
    Edit: baseActionPath + 'Edit',
    Delete: baseActionPath + 'Delete',
    SavePic: baseActionPath + "SavePic"
};

//模板
var baseTempPath = URL('/Scripts/Bus/Food/html/');
var HtmlURL =
{
    Add: baseTempPath + 'add.html',
   
};

var column = [[
    { field: 'ckb', title: '选择', checkbox: true, width: 30 },
    { field: "FoodName", title: "菜名", align: "left", width: 100 },
    { field: "Pic", title: "图片", align: "center", width: 100 },
    {
        field: "CreateDate", title: "添加时间", align: "center", width: 100, formatter: function (value, row, index) {
            return ShortDate(value);
        }
    }
]];


//加载
$(document).ready(function () {
    $dataGrid = $('#dataGrid');
    CRUD.Init();
});

var CRUD = {
    Init: function () {
        $dataGrid.datagrid(
                {
                    rownumbers: true,
                    toolbar: '#toolbar',
                    checkOnSelect: true,
                    selectOnCheck: false,
                    singleSelect: true,
                    fit: true,
                    fitColumns: true,
                    url: ActionURL.Read,
                    border: false,
                    pageSize: 15,
                    pageList: [15, 30, 50],
                    pagination: true,
                    queryParams: getParams(),
                    onRowContextMenu: onRowContextMenu,
                    columns: column
                });
    },
    Read: function () {
        $dataGrid.datagrid('reload', getParams());
    },
    Add: function () {
        var $h = top.$.hDialog(
                {
                    iconCls: 'icon-add',
                    title: '&nbsp;&nbsp;&nbsp;' + '添加饭菜',
                    href: HtmlURL.Add,
                    width: iDialogW,
                    height: iDialogH,
                   // cache: true,
                    onLoad: function () {
                        initAdd($h);//初始化添加
                    },
                    submit: function () {
                        var eID;
                        var $fm = $h.find('form');
                        if (!$fm.form('validate')) return;
                        var sendData = $fm.serializeArray();
                        sendData = convertArray(sendData);
                        top.$.hAjax(
                        {
                            type: 'post',
                            data: sendData,
                            url: ActionURL.Add,
                            success: function (d) {
                                if (d.success) {
                                    $h.dialog('close');
                                    CRUD.Read();
                                }
                            }
                        });
                    }
                });
    },
    Edit: function () {
        var rows = $dataGrid.datagrid('getSelections');
        if (rows.length > 1) { top.$.messager.alert('提示', '只能选择一条记录'); return; }
        var row = $dataGrid.datagrid('getSelected');
        if (!row) { top.$.messager.alert('提示', '请选择一条记录'); return; }
        var $h = top.$.hDialog(
                {
                    iconCls: 'icon-edit',
                    title: '&nbsp;&nbsp;&nbsp;' + '编辑【' + row.FoodName + '】的信息',
                    href: HtmlURL.Add,
                    width: iDialogW,
                    height: iDialogH,
                    onLoad: function () {
                        $h.find('form').form('load', row);
                        initAdd($h);
                    },
                    submit: function () {
                        var $fm = $h.find('form');
                        if (!$fm.form('validate')) return;
                        var sendData = $fm.serializeArray();
                        sendData = convertArray(sendData);
                        top.$.hAjax(
                        {
                            type: 'post',
                            data: sendData,
                            url: ActionURL.Edit,
                            success: function (d) {
                                if (d.success) {
                                    $h.dialog('close');
                                    CRUD.Read();
                                }
                            }
                        });
                    }
                });
    }, Delete: function () {
        var rows = $dataGrid.datagrid('getChecked');
        if (!rows || !rows.length) { rows = $dataGrid.datagrid('getSelections'); }
        var len = rows.length;
        if (len == 0) { top.$.messager.alert('提示', '请选择一条记录'); return; };
        top.$.messager.confirm('删除提示', '你确认删除当前选中的【<span style="color:red">' + rows.length + '</span>】条记录？',
        function (r) {
            if (r) {
                var ids = [];
                $.each(rows, function () { ids.push(this.ID); });
                top.$.hAjax(
                {
                    url: ActionURL.Delete,
                    type: 'POST',
                    data: { ids: ids.join(',') },
                    success: function () {
                        CRUD.Read();
                    }
                })
            }
        });
    }
}



//获列表加载参数
function getParams() {
    return getDefaultParams();
}

//获取默认的查询参数
function getDefaultParams() {
    var obj = {};
    obj.search = $('#txtSearch').searchbox('getValue');
    return obj;
};

function initAdd($h) {
   
    var imgId = $h.find("#picIcon").val();
    var foodId = "";
    if (imgId == "") {
        foodId = "0";
        $h.find('#imgupload').attr('src', URL('/defaultFood.png'));
    } else {
        $h.find('#imgupload').attr('src', URL('/upload/food/') + imgId);
        foodId = imgId.substring(0, imgId.indexOf("."))
    }

    $h.find('#imgupload')
       .attr('title', '单击上传图像')
       .css({ 'cursor': 'pointer' })
       .unbind('click').bind('click', function () {
           $.SWFUpload(
               {
                   title: '&nbsp;' + '上传图片',
                   width: 300,
                   height: 300
               },
               {
                   'auto': false,
                   'uploader': ActionURL.SavePic,
                   'formData': { 'foodId': foodId }, //传参数'fileTypeExts': AllowDocumentType,
                   'fileTypeExts': '*.jpg;*.jpge;*.gif;*.png;',
                   'fileSizeLimit': '2MB',
                   'onUploadSuccess': function (file, data, response) {
                       var array = data.split(",");
                       $h.find('#imgupload').attr('src', URL(array[0]) + '?t=' + Math.random());
                       $h.find("#picIcon").val(array[1]);
                   }
               });
       });
}


//右键菜单
function onRowContextMenu(e, rowIndex, rowData) {
    e.preventDefault();
    $dataGrid.datagrid('selectRow', rowIndex);

   // top.$.messager.alert('提示', '右击');
    top.$.messager.show({title:'提示', msg:'右击', showType:"slide"});
    //$('#contextMenu').menu('show', {
    //    left: e.pageX,
    //    top: e.pageY
    //});
}




