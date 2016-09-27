var $datagrid;
var iDialogW = 560, iDialogH = 350;

//操作逻辑preIndex
var baseActionPath = URL('/Weixin/Subscribe/');
var ActionURL =
{
    Read: baseActionPath + 'GetList'
};



var column = [[
    { field: "FromUserName", title: "OpenID", align: "left", width: 100 },
    { field: "Status", title: "状态", align: "center", width: 100 },
    {
        field: "OptionDate", title: "时间", align: "center", width: 100, formatter: function (value, row, index) {
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
                    columns: column
                });
    },
    Read: function () {
        $dataGrid.datagrid('reload', getParams());
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








