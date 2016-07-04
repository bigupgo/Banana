using System.Web;
using System.Web.Optimization;

namespace Banana.WebUI
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //基础工具库
            bundles.Add(new ScriptBundle("~/bundles/util")
                        .Include(
                         "~/Scripts/js/FunctionJS.js"
                        , "~/Scripts/jquery-1.8.0.js")
                        );

            //UI工具库相关
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                           "~/Scripts/json2.js"
                        , "~/Scripts/Easyui/easyui/locale/easyui-lang-zh_CN.js"
                        , "~/Scripts/Plugin/AjaxHelper.js"
                        , "~/Scripts/Easyui/extensions.dialog.js"
                        , "~/Scripts/Easyui/extensions.validate.js"
                        , "~/Scripts/Easyui/extensions.iconSelect.js"
                        ));
            //flash文件上传
            bundles.Add(new ScriptBundle("~/bundles/fileUpload").Include(
                 "~/Scripts/Uploadify/js/swfobject.js"
                 , "~/Scripts/Uploadify/js/uploadify/jquery-uploadify-min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Scripts/Uploadify/css/uploadify.css"
                        , "~/Content/Home/font-awesome/css/font-awesome.css"
                        , "~/Scripts/bootstrap-3.3.4-dist/css/bootstrap.css"
                        , "~/Content/icon.css"
                        , "~/Content/Home/sub.css"
                ));
        }
    }
}