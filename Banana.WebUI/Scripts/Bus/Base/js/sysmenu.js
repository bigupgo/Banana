var $treegrid;
var iDialogW = 360, iDialogH = 380;
var preIndex = null;
var $mineTree; var arrayId = [];
var treeList = null;
var ModuleDefaultIconCls = 'icon-blue-folder-close';
var PageDefaultIconCls = 'icon-first-node';
//操作逻辑preIndex
var baseActionPath = URL('/Home/');
var ActionURL =
{
    Read: baseActionPath + 'GetList',
    Add: baseActionPath + 'Add',
    Edit: baseActionPath + 'Edit',
    Delete: baseActionPath + 'Delete',
    GetTree: baseActionPath + 'GetTreeList'
};

//模板
var baseTempPath = URL('/Scripts/Bus/Base/html/');
var HtmlURL =
{
    Icon: URL('/Content/icon.htm'),
    Add: baseTempPath + 'menutype.html',
    AddPage: baseTempPath + "addpage.html"

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
        field: "Enable", title: "状态", align: "center", width: 30, formatter: function (value, row, index) {
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
        var $deptComb = null;
        var $h = top.$.hDialog(
        {
            iconCls: 'icon-add',
            title: '&nbsp;&nbsp;&nbsp;' + '选择哪种类型的模块',
            href: HtmlURL.Add,
            width: 350,
            height: 360,
            cache: true,
            onLoad: function () {
                //图表修改
                $h.find("#tmp-group").find('img').attr('src', URL("Content/Default/images/Module-Group.png"));
                $h.find("#tmp-page").find('img').attr('src', URL("Content/Default/images/Module-Page.png"));

                $h.find('#tmp-group,#tmp-page').bind('click', function () {
                    $h.dialog('close');
                });
                $h.find('#tmp-group').bind('click', initCEGroup);
                $h.find('#tmp-page').bind('click', function () {
                    initCEPage(
                        {

                            iconCls: 'icon-page-world',
                            title: '&nbsp;&nbsp;&nbsp;' + '添加页面',
                            href: HtmlURL.AddPage
                        });
                });
            },
            submit: false
        });
    },
    Edit: function () {
        var rows = $treegrid.datagrid('getSelections');
        if (rows.length > 1) { top.$.messager.alert('提示', '只能选择一条记录'); return; }
        var row = $treegrid.datagrid('getSelected');
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
        var rows = $treegrid.datagrid('getSelections');
        if (!rows || !rows.length) { rows = $treegrid.datagrid('getSelections'); }
        var len = rows.length;
        if (len == 0) { top.$.messager.alert('提示', '请选择一条记录'); return; };
        top.$.messager.confirm('删除提示', '你确认删除当前选中的【<span style="color:red">' + rows.length + '</span>】条记录？',
        function (r) {
            if (r) {
                var id = rows[0].ID;
                var type = rows[0].Type;
                $.hAjax({
                    url: ActionURL.Delete,
                    type: "post",
                    data: { Id: id, type: type },
                    success: function (res) {
                        if (res.success) {
                            CRUD.Read();
                        }
                    }
                });
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
    $treegrid.datagrid('selectRow', rowIndex);

   // top.$.messager.alert('提示', '右击');
    top.$.messager.show({title:'提示', msg:'右击', showType:"slide"});
    //$('#contextMenu').menu('show', {
    //    left: e.pageX,
    //    top: e.pageY
    //});
}


//添加、编辑组 初始化
function initCEGroup(row) {
    var isEdit = typeof row.ID != 'undefined';
    var $h = top.$.hDialog(
        {
            iconCls: 'icon-application-double',
            title: '&nbsp;&nbsp;&nbsp;' + (isEdit ? '编辑' : '添加') + '模块组',
            href: HtmlURL.AddGroup,
            width: 650,
            height: 300,
            cache: true,
            onLoad: function () {
                loadParent($h.find("#parentId"), isEdit);
                initIconSelect($h.find('#link-iconSelect'), function (n)
                {
                    $h.find('input[name=IconCls]').val(n);
                });
                if (isEdit) {
                    $h.find('form').form('load', row);
                } else {
                    $h.find('input[name=IconCls]').val(ModuleDefaultIconCls);
                }
                $h.find('#link-iconSelect').iconSelect(
                    {
                        position: 'bottom',
                        iconHref: HtmlURL.Icon,
                        onSelect: function (n) {
                            $h.find('input[name=IconCls]').val(n);
                        }
                    });
            },
            submit: function () {
                //_SubmitAddNav($h);
            }
        });
};

//初始化添加、编辑页面
function initCEPage(options) {
    var isEdit = typeof options.row != 'undefined';
    var $h = top.$.hDialog(
       {
           iconCls: options.iconCls,
           title: options.title,
           href: options.href,
           width: 700,
           height: 300,
           cache: true,
           onLoad: function () {
               loadParent($h, isEdit);
               initIconSelect($h.find('#link-iconSelect'), function (n)
               { $h.find('input[name=IconCls]').val(n); });
               if (isEdit) {
                   $h.find('form').form('load', row);
               } else {
                   $h.find('input[name=IconCls]').val(ModuleDefaultIconCls);
               }

               $h.find('#link-iconSelect').iconSelect(
                      {
                          position: 'bottom',
                          iconHref: HtmlURL.Icon,
                          onSelect: function (n) {
                              $h.find('input[name=IconCls]').val(n);                             
                          }
                      });

           },
           submit: function () {
               var $fm = $h.find('form');
               if (!$fm.form('validate')) return;
               var sendData = $fm.serializeArray();
              
               sendData = convertArray(sendData);
               sendData.ModelLevel = Number(sendData.ModelLevel) + 1;
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
  
    })
}

//初始化父级节点
function loadParent($h, isEdit) {
    var $ele = $h.find("#parentId");
    $ele.combotree({
        onSelect: function (record) {
            if (!isEdit) {
                var $modelLevel = $h.find("input[name='ModelLevel']");
                $modelLevel.val(record.attributes.ModelLevel);
            } else {
               
            }
        }
    });

    if (treeList == null) {
        $.getJSON(ActionURL.GetTree, { _: Math.random() }, function (d) {
            treeList = d;
            $ele.combotree('loadData', treeList);
        });
    } else {
        $ele.combotree('loadData', treeList);

        var row = $treegrid.treegrid('getSelected');
        if (row) {
            if (isEdit) {
                if (row.ParentID) {
                    $ele.combotree('setValue', row.Pid);
                }
            } else {
                if (row.Type == 0) {
                    $ele.combotree('setValue', row.ID);
                } else {
                    $ele.combotree('setValue', row.ParentID);
                }
            }
        } else {
            $ele.combotree('setValue', "0");
        }
    }
   
}

//初始化图标选择
function initIconSelect($ele, cbOnSelect, iconCls) {
    var $sel = $ele.iconSelect(
                      {
                          icon: iconCls,
                          iconHref: HtmlURL.Icon,
                          onSelect: cbOnSelect
                      });
};