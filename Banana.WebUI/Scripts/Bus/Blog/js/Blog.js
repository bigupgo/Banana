

var $dataGrid;

var allProjects;
//添加编辑框的大小
var iDialogH = 400, iDialogW = 690;
//操作逻辑
var baseActionPath = URL('/Blog/Blog/');
var actions =
{
    Read: baseActionPath + 'GetList',
    Add: baseActionPath + 'Add',
    Edit: baseActionPath + 'Edit',
    Delete: baseActionPath + 'Delete',
    GetProjects: baseActionPath + 'GetProjects'
};

//模板
var baseTempPath = URL('/Scripts/Bus/Blog/html/');
var htmls =
{
    Add: baseTempPath + 'add.htm',
    Edit: baseTempPath + 'edit.htm',
    Projects: baseTempPath + 'projects.htm',
};

//列表字段
var columns =
[
   { field: 'ckb', title: '选择', checkbox: true, width: 10, sorttable: true },
   {
       field: 'blogDate', title: '日志日期', align: 'center', width: 10, formatter: function (d, r) {
           var dtPub = ConvertJSONDate(r.pubDate);
           var dtAdd = ConvertJSONDate(d);
           //24+9=33为第二天的9点
           if ((dtPub - dtAdd) > 33 * 60 * 60 * 1000) {
               return "<span style='color:#d9534f;font-weight:bold'>" + d + "</span>";
           }
           return d;
       }
   },
   {
       field: 'projectName', title: '项目名称', align: 'left', width: 15, formatter: function (d) {
         
           return "<div title='" + d + "'>" + ShortWords(d, 15) + "</div>";
       }
   },
   {
       field: 'blogContent', title: '工作总结', align: 'left', width: 40, formatter: function (d) {
           return "<div title='" + d + "'>" + ShortWords(d,40) + "</div>";
       }
   },
   {
       field: 'checkState', title: '审核状态', align: 'center', width: 10, formatter: function (d) {
           switch (d) {
               case 0:
                   return "<span style='color:#575858'>未审核</span>";
               case 1:
                   return "<span style='color:#5cb85c'>已审核</span>";
               case 2:
                   return "<span style='color:#d9534f'>未通过</span>";
               default:

           }
       }
   },
   {
       field: 'checkEmployeeName', title: '审核人', align: 'center', width: 10, formatter: function (d, r) {
           if (r.checkState != 0) {
               return d;
           }
           return "";
       }
   }
];

//加载
$(document).ready(function () {
    $dataGrid = $('#dataGrid');
    CRUD.Init();
});

//增删查改
var CRUD =
{
    Init: function () {
        $dataGrid.datagrid(
                {
                    rownumbers: true,
                    checkOnSelect: true,
                    selectOnCheck: true,
                    singleSelect: true,
                    fit: true,
                    fitColumns: true,
                    toolbar: '#toolbar',
                    url: actions.Read,
                    border: false,
                    pagination: false,
                    queryParams: getParams(),
                    onRowContextMenu: onRowContextMenu,
                    columns: [columns]
                });
    },
    Detail: function () {
        var row = $dataGrid.datagrid('getSelected');
        if (!row) { top.$.messager.alert('提示', '请选择一条记录'); return; }
        var $h = top.$.hAjax(
        {
            type: 'PSOT',
            url: ActionURL.Detail,
            data: { id: row.ID },
            cache: false,
            success: function (d) {
                var $h = top.$.hDialog(
                {
                    iconCls: 'fa fa-newspaper-o',
                    title: '详情',
                    href: HtmlURL.Detail,
                    width: iDialogW,
                    height: iDialogH,
                    onLoad: function () {
                        $h.find('form').form('load', row);
                    },
                    submit: false
                });
            }
        });
    },
    Read: function () {
        $dataGrid.datagrid('reload', getParams());
    },
    Add: function () {
        var $h = top.$.hDialog(
                {
                    iconCls: 'fa fa-plus-circle',
                    title: '添加',
                    href: htmls.Add,
                    width: iDialogW,
                    height: iDialogH,
                    cache: true,
                    onLoad: function () {
                        $h.find('#moreProjects').click(function () {
                            selectMoreProject($h.find('form'));
                        });

                        loadProjects($h.find('#cbProject'));
                        $h.find('#blogDate').datebox({
                            validType: ["maxDate['" + new Date().Format('yyyy/MM/dd') + "']"]//ie8不支持-写法
                        });
                        $h.find('#blogDate').datebox('setValue', new Date().Format('yyyy-MM-dd'));
                       
                    },
                    submit: function () {

                        var $fm = $h.find('form');
                        console.log($fm);
                        if (!$fm.form('validate')) return;
                        var sendData = $fm.serializeArray();
                        sendData = convertArray(sendData);

                        sendData.beginTime = $h.find('#dateStart').combobox('getValue');
                        sendData.endTime = $h.find('#dateEnd').combobox('getValue');


                        var currentDate = new Date();
                        if (ConvertJSONDate(sendData.blogDate).Format('yyyy-MM-dd') == currentDate.Format('yyyy-MM-dd') && currentDate.getHours() < 18) {
                            top.$.messager.show({
                                title: '提示',
                                msg: '当天日志请与18点之后填写。'
                            });
                        }
                        else {
                            top.$.hAjax(
                            {
                                type: 'post',
                                data: sendData,
                                url: actions.Add,
                                success: function (d) {
                                    if (d.success) {
                                        $h.dialog('close');
                                        CRUD.Read();
                                    }
                                }
                            });
                        }
                    }
                });
    },
    Edit: function () {
        var rows = $dataGrid.datagrid('getSelections');
        if (rows.length > 1) { top.$.messager.alert('提示', '只能选择一条数据！'); return; }
        var row = $dataGrid.datagrid('getSelected');
        if (!row) { top.$.messager.alert('提示', '请选择一条记录！'); return; }
        var $h = top.$.hDialog(
               {
                   iconCls: 'fa fa-pencil-square-o',
                   title: '编辑',
                   href: htmls.Edit,
                   width: iDialogW,
                   height: iDialogH,
                   onLoad: function () {
                       $h.find('#moreProjects').click(function () {
                           selectMoreProject($h.find('form'));
                       });
                       
                       $h.find('form').form('load', row);

                       $h.find('#dateStart').combobox('setValue', row.beginTime);
                       $h.find('#dateEnd').combobox('endTime', row.beginTime);

                       loadProjects($h.find('#cbProject'), row.projectID);
                       $h.find('#blogDate').datebox({
                           validType: ["maxDate['" + new Date().Format('yyyy/MM/dd') + "']"]//ie8不支持-写法
                       });
                       $h.find('#blogDate').datebox('setValue', row.blogDate);
                   }
                   , submit: function () {
                       var $fm = $h.find('form');
                       if (!$fm.form('validate')) return;
                       var sendData = $fm.serializeArray();
                       sendData = convertArray(sendData);

                       sendData = $.extend(row, sendData);

                       var currentDate = new Date();
                       if (ConvertJSONDate(sendData.blogDate).Format('yyyy-MM-dd') == currentDate.Format('yyyy-MM-dd') && currentDate.getHours() < 18) {
                           top.$.messager.show({
                               title: '提示',
                               msg: '当天日志请与18点之后填写。'
                           });
                       }
                       else {
                           top.$.hAjax(
                           {
                               type: 'post',
                               data: sendData,
                               url: actions.Edit,
                               success: function (d) {
                                   if (d.success) {
                                       $h.dialog('close');
                                       CRUD.Read();
                                   }
                               }
                           });
                       }
                   }
               });
    },
    Delete: function () {
        var row = $dataGrid.datagrid('getSelected');
        if (!row) {
            top.$.messager.alert('提示', '请选择一条记录'); return;
        }
        top.$.messager.confirm('删除提示', '你确认删除当前选中的【<span style="color:red">' + row.blogDate + '</span>】条记录？',
        function (r) {
            if (r) {
                //var ids = [];
                //$.each(rows, function () { ids.push(this.blogID); });
                //top.$.hAjax(
                //{
                //    url: actions.Delete,
                //    type: 'POST',
                //    data: { ids: ids.join(',') },
                //    success: function () {
                //        CRUD.Read();
                //    }
                //})
                top.$.hAjax(
                {
                    url: actions.Delete,
                    type: 'POST',
                    data: { blogID: row.blogID },
                    success: function () {
                        CRUD.Read();
                    }
                })
            }
        });
    },
    Search: function () {
        CRUD.Read();
    },
    Export: function () {
        var p = getParams();
        var str = '?a=1';
        for (var i in p) {
            str += '&' + i + '=' + p[i];
        }
        top.location.href = actions.Export + str;
    },
}

//获列表加载参数
function getParams() {
    var obj = {};
    //obj.search = $('#txtSearch').searchbox('getValue');
    return obj;
}


function selectMoreProject(form) {
    var $ph = top.$.hDialog(
                 {
                     iconCls: 'fa fa-search-plus',
                     title: '项目列表',
                     href: htmls.Projects,
                     width: 600,
                     height: 600,
                     cache: false,
                     onLoad: function () {
                         $ph.find('#moreProjects').datagrid({
                             url: actions.GetProjects,
                             singleSelect: true,
                             width: 595,
                             height: 480,
                             toolbar: '#projectToolBar',
                             fitColumns: true,
                             onLoadSuccess: function (d) {
                                 if (!allProjects) {
                                     allProjects = d.rows;
                                 }
                                 $ph.find('.lab_Search').searchbox({
                                     searcher: function (value, name) {
                                         var items = $.grep(allProjects, function (n, i) {
                                             var tt = new RegExp(value);
                                             return tt.test(n.projectID + n.projectName + n.company);
                                         });
                                         $ph.find('#moreProjects').datagrid('loadData', items);

                                     },
                                     width: 300,
                                     prompt: "项目编号，项目名称，客户"
                                 });
                             },
                             columns: [[
                                 { field: 'projectID', title: '项目编号', width: 100 },
                                 { field: 'projectName', title: '项目名称', width: 250, align: 'left' },
                                 { field: 'company', title: '客户', width: 250, align: 'left' }
                             ]]
                         });
                     },
                     submit: function () {
                         var datas = form.find('#cbProject').combobox('getData');
                         var item = $ph.find('#moreProjects').datagrid('getSelected');
                         if (item) {
                             datas.push(item);
                             form.find('#cbProject').combobox('loadData', datas);
                             form.find('#cbProject').combobox('setValue', item.projectID);
                         }
                         $ph.dialog('close');
                     }
                 });
}

function loadProjects(obj, selectID, onSuccess) {
    var logData = $dataGrid.datagrid('getData').rows;

    var projects = [];

    if (logData != null && logData.length > 0) {
        $.each(logData, function (i, n) {

            if (!$(projects).is(function () { return this.projectID == n.projectID; })) {
                projects.push({ projectID: n.projectID, projectName: n.projectName });
            }
        });
    }
    obj.combobox({
        data: projects,
        valueField: 'projectID',
        textField: 'projectName',
        onLoadSuccess: function () {
            if (projects.length > 0) {
                var s = selectID == null ? projects[0].projectID : selectID;
                obj.combobox('setValue', s);
            }
            if (onSuccess) {
                onSuccess();
            }
        }
    });
}

//右键菜单
function onRowContextMenu(e, rowIndex) {
    e.preventDefault();
    $dataGrid.datagrid('selectRow', rowIndex);
    $('#contextMenu').menu('show', {
        left: e.pageX,
        top: e.pageY
    });
}

