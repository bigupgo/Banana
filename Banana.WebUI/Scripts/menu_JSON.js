var COMMONMenu =
    [
        { iconCls: 'icon-reload', Flag: 'Read', Name: '刷新'},
        { iconCls: 'icon-add', Flag: 'Add', Name: '添加'},
        { iconCls: 'icon-edit-add', Flag: 'Add', Name: '添加'},

        { iconCls: 'icon-edit', Flag: 'Edit', Name: '编辑' },
        { iconCls: 'icon-pencil', Flag: 'Edit', Name: '编辑' },

        { iconCls: 'icon-cancel', Flag: 'Delete', Name: '删除' },
        { iconCls: 'icon-cross', Flag: 'Delete', Name: '删除' },
        { iconCls: 'icon-edit-remove', Flag: 'Delete', Name: '删除' },

        { iconCls: 'icon-advancedsettings', Flag: 'Setting', Name: '设置' },
        { iconCls: 'icon-advancedsettings2', Flag: 'Setting', Name: '设置' },
        { iconCls: 'icon-settings', Flag: 'Setting', Name: '设置' },
        { iconCls: 'icon-cog', Flag: 'Setting', Name: '设置' },

        { iconCls: 'icon-search', Flag: 'Search', Name: '搜索' },
        { iconCls: 'icon-zoom', Flag: 'Search', Name: '搜索' },

        { iconCls: 'icon-disk-black', Flag: 'Save', Name: '保存' },

        { iconCls: 'icon-upload', Flag: 'Upload', Name: '上传' },
        { iconCls: 'icon-download', Flag: 'Download', Name: '下载' },
        { iconCls: 'icon-application-osx-go', Flag: 'Export', Name: '导出' },
        { iconCls: 'icon-table-column-add', Flag: 'Import', Name: '导入' },
        { iconCls: 'icon-print', Flag: 'Print', Name: '打印' },
        { iconCls: 'icon-help', Flag: 'Help', Name: '帮助' }
    ];
var flagMenuShowType = { Toolbar: '2', ContextMenu: '3', Both: '1' };  //按钮出现位置
var PAGEMENU =
    [
        { Name: '刷新', Flag: 'Read', IconCls: 'icon-reload', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 1 ,Checked: true },
        { Name: '添加', Flag: 'Add', IconCls: 'icon-add', RequestType: 'POST', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 2, Checked: true },
        { Name: '编辑', Flag: 'Edit', IconCls: 'icon-edit', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 3, Checked: true },
        { Name: '删除', Flag: 'Delete', IconCls: 'icon-cross', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 4, Checked: true },
        { Name: '上传', Flag: 'Upload', IconCls: 'icon-upload', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 5 },
        { Name: '下载', Flag: 'Download', IconCls: 'icon-download', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 6 },
        { Name: '导入', Flag: 'Import', IconCls: 'icon-table-column-add', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 7 },
        { Name: '导出', Flag: 'Export', IconCls: 'icon-application-osx-go', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 8 },
        { Name: '打印', Flag: 'Print', IconCls: 'icon-print', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 9 },
        { Name: '高级搜索', Flag: 'Search', IconCls: 'icon-search', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 10 },
        { Name: '保存', Flag: 'Save', IconCls: 'icon-disk-black', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 11 },
        { Name: '设置', Flag: 'Setting', IconCls: 'icon-settings', RequestType: 'POST', MenuType: flagMenuShowType.Both, IsVisible: true, Sort: 12 }
    ]

// 提供上传文档模块按钮项
/* 新建文件夹、编辑、删除、上传、下载、刷新、上传附件、查看文档、查看附件、授权、共享、关联 */
var UPLOADMENU = [
    { Name: '刷新', Flag: 'Read', IconCls: 'icon-reload', RequestType: 'GET', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 1, Checked: true ,title:'上传页面必备的操作按钮！'},
    { Name: '新建', Flag: 'Add', IconCls: 'icon-folder-add', RequestType: 'POST', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 2, title: '如果上传页面需要有【新建文件夹】功能，请勾选上' },
    { Name: '编辑', Flag: 'Edit', IconCls: 'icon-edit', RequestType: 'POST', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 3, Checked: true, title: '上传页面必备的操作按钮！' },
    { Name: '删除', Flag: 'Delete', IconCls: 'icon-cross', RequestType: 'POST', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 4, Checked: true, title: '上传页面必备的操作按钮！' },
    { Name: '上传', Flag: 'Upload', IconCls: 'icon-upload', RequestType: 'POST', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 5, Checked: true, title: '上传页面必备的操作按钮！' },
    { Name: '下载', Flag: 'Download', IconCls: 'icon-download', RequestType: 'POST', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 6, Checked: true, title: '上传页面必备的操作按钮！' },
    { Name: '查看', Flag: 'Browse', IconCls: 'icon-zoom', RequestType: 'GET', MenuType: flagMenuShowType.ContextMenu, IsVisible: true, Sort: 7, Checked: true, title: '上传页面必备的操作按钮！' },
    //{ Name: '授权', Flag: 'SetAuthority', IconCls: 'icon-group-key', RequestType: 'POST', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 8 },
    //{ Name: '共享', Flag: 'Share', IconCls: 'icon-star', RequestType: 'POST', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 9 },
    //{ Name: '关联', Flag: 'Relation', IconCls: 'icon-share', RequestType: 'POST', MenuType: flagMenuShowType.Toolbar, IsVisible: true, Sort: 10 },
    { Name: '附件', Flag: 'ShowAttach', IconCls: 'icon-checkbox-no', RequestType: 'POST', MenuType: flagMenuShowType.ContextMenu, IsVisible: true, Sort: 10, title: '如果上传页面需要有【附加】功能，请勾选上' },
    { Name: '操作记录', Flag: 'OperateHistory', IconCls: 'icon-keyboard-magnify', RequestType: 'POST', MenuType: flagMenuShowType.ContextMenu, IsVisible: true, Sort: 11, Checked: true, title: '上传页面必备的操作按钮！' }
];
