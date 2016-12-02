using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Banana.Core.Base
{
    public static class MvcHtmlExtend
    {
        public const string REFRESH = " <a href=\"javascript:void(0);\" onclick=\"CRUD.Read();\" class=\"easyui-linkbutton l-btn l-btn-plain\" iconcls=\"fa fa-refresh\" plain=\"true\" title=\"刷新\">刷新</a>";
         public const string ADD = "<a href=\"javascript:void(0);\" onclick=\"CRUD.Add();\" class=\"easyui-linkbutton l-btn l-btn-plain\" iconcls=\"fa fa-plus-circle\" plain=\"true\" title=\"添加\">添加</a>";
        public const string EDIT=  " <a href=\"javascript:void(0);\" onclick=\"CRUD.Edit();\" class=\"easyui-linkbutton l-btn l-btn-plain\" iconcls=\"fa fa-pencil\" plain=\"true\" title=\"添加\">修改</a>";
        public const string DELETE =" <a href=\"javascript:void(0);\" onclick=\"CRUD.Delete();\" class=\"easyui-linkbutton l-btn l-btn-plain\" iconcls=\"fa fa-trash\" plain=\"true\" title=\"删除\">删除</a>";
        public const string EXPORT = "<a href=\"javascript:void(0);\" onclick=\"CRUD.Export();\" class=\"easyui-linkbutton l-btn l-btn-plain\" iconcls=\"fa fa-print\" plain=\"true\" title=\"导出\">导出</a>";
        
        public static MvcHtmlString Toolbar(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(REFRESH);
            sb.Append(ADD);
            sb.Append(EDIT);
            sb.Append(DELETE);
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString ToolbarE(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(REFRESH);
            sb.Append(ADD);
            sb.Append(EDIT);
            sb.Append(DELETE);
            sb.Append(EXPORT);
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
