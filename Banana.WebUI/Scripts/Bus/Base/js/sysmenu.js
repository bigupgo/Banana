var $treegrid;
var iDialogW = 560, iDialogH = 350;
var preIndex = null;
var $mineTree; var arrayId = [];

//操作逻辑preIndex
var baseActionPath = URL('/Home/');
var ActionURL =
{
    Read: baseActionPath + 'GetList',
    Add: baseActionPath + 'Add',
    Edit: baseActionPath + 'Edit',
    Delete: baseActionPath + 'Delete',
};

//模板
var baseTempPath = URL('/Scripts/Bus/User/html/');
var HtmlURL =
{
    Add: baseTempPath + 'add.html',
   
};

var column = [[
    { field: 'ID', hidden:true },
    { field: "ModelName", title: "模块名", align: "left", width: 90 },
    {
        field: "Type", title: "类型", align: "center", width: 50, formatter: function (value, row, index) {
            if (value == 0) {
                return "<div>组</div>";
            } else {
                return "<div>页面</div>";
            }
        }
    },
    {
        field: "State", title: "状态", align: "center", width: 30, formatter: function (value, row, index) {
            if (value) {
                return "<div style='color:green;'>启用</div>";
            } else {
                return "<div style='color:red;'>禁用</div>";
            }
        }
    },
    { field: "Url", title: "地址", align: "center", width: 110 },
    
    { field: "CreateTime", title: "创建时间", align: "center", width: 90,formatter:function(value,row,index){
        return ShortDate(value);
       }
    },
    { field: "Sort", title: "排序", align: "center", width: 50 }
   
]];


//加载
$(document).ready(function () {
    $treegrid = $('#dataGrid');
    CRUD.Init();
});

var CRUD = {
    Init: function () {
        $treegrid.treegrid(
                {
                   
                    toolbar: '#toolbar',
                    fit: true,
                    fitColumns: true,
                    idField: 'ID',
                    treeField:'ModelName',
                    url: ActionURL.Read,
                    border: false,
                    queryParams: getParams(),
                    onRowContextMenu: onRowContextMenu,
                    columns: column
                });
    },
    Read: function () {
        $treegrid.treegrid('reload', getParams());
    },
    Add: function () {
        var $h = top.$.hDialog(
                {
                    iconCls: 'icon-add',
                    title: '&nbsp;&nbsp;&nbsp;' + '添加从业人员信息',
                    href: HtmlURL.Add,
                    width: iDialogW,
                    height: iDialogH,
                   // cache: true,
                    onLoad: function () {
                        $h.find("#loginNameId").validatebox({
                            validType: ['loginName', 'loginNameHave']
                        });

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
                    title: '&nbsp;&nbsp;&nbsp;' + '编辑【' + row.LoginName + '】的信息',
                    href: HtmlURL.Add,
                    width: iDialogW,
                    height: iDialogH,
                  //  cache: true,
                    onLoad: function () {
                      
                        initAdd($h);
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
                $.each(rows, function () { ids.push(this.Id); });
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




