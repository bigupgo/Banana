using Banana.Core;
using Banana.Core.Common;
using Banana.Core.Db;
using Banana.Model;
using ExcelReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;


namespace Banana.Bll
{
    public class UserInfoBll : BaseBll
    {
        private static string TABLE_NAME = "Ba_UserInfo";

        /// <summary>
        /// 设置操作的表名
        /// </summary>
        public override void SetTableName()
        {
            this.TableName = TABLE_NAME;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pagetion"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<UserInfo> GetList(Pagetion pagetion, string search)
        {
            List<UserInfo> list = new List<UserInfo>();
            try
            {
                DataTable dt = GetAllData();
                var data = ERTools.GetList<UserInfo>(dt);
                var totalData = data
                    .Where(x => x.LoginName.Contains(search) || x.RealName.Contains(search))
                    .OrderByDescending(x => x.CreateDate);
                pagetion.total = totalData.Count();

                list = totalData.Skip(pagetion.rows * (pagetion.page - 1)).Take(pagetion.rows).ToList();
            }
            catch (Exception e)
            {
                LogHelper.LogError(e.Message);
            }
            return list;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public AjaxReturn Add(UserInfo userInfo)
        {
            AjaxReturn result = base.Add(userInfo);
            return result;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public AjaxReturn Edit(UserInfo userInfo)
        {
            AjaxReturn result = base.Edit(userInfo);          
            return result;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public AjaxReturn Deletes(string ids)
        {
            AjaxReturn result = base.Delete(ids);
            return result;
        }

        /// <summary>
        /// 保存用户头像
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public string SaveUserIcon(string userId, HttpPostedFileBase file)
        {
            HttpServerUtility Server = HttpContext.Current.Server;
            if (string.IsNullOrEmpty(userId) || file == null)
            {
                throw new ArgumentNullException();
            }
            if (userId == "0")
            {
                userId = GetGUID();
            }
            string basePath = BaseConfig.GetValue("UserImgUrl");
            string url = basePath + userId + Path.GetExtension(file.FileName);
            string filePath = Server.MapPath("~" + url);
            UploadHelper.UploadFile(filePath, file);
            return url + "," + userId;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="search"></param>
        public void ExportExcel(string search)
        {
            try
            {
                //获取数据源
                DataTable dt = GetAllData();
                List<UserInfo> list = ERTools.GetList<UserInfo>(dt);

                list.Where(x => x.LoginName.Contains(search) || x.RealName.Contains(search));
                //获取文件模板路径
                var filename = "会员";
                var tmpPath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\excelTemplate\\" + filename + ".xml";
                var xlsPath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\excelTemplate\\" + filename + ".xls";

                //Excel表单名
                var TableSheetName = "Sheet1";

                //加载报表
                ParameterCollection collection = new ParameterCollection();
                collection.Load(tmpPath);

                List<ElementFormatter> formatters = new List<ElementFormatter>();

                //从Sheet1的X轴开始,加载GetList数据源
                formatters.Add(new TableFormatter<UserInfo>(
                    collection[TableSheetName, "LoginName"].X, list,
                    new TableColumnInfo<UserInfo>(collection[TableSheetName, "LoginName"].Y, x => x.LoginName),
                    new TableColumnInfo<UserInfo>(collection[TableSheetName, "RealName"].Y, x => x.RealName),
                    new TableColumnInfo<UserInfo>(collection[TableSheetName, "Phone"].Y, x => x.Phone),
                    new TableColumnInfo<UserInfo>(collection[TableSheetName, "Email"].Y, x => x.Email),
                    new TableColumnInfo<UserInfo>(collection[TableSheetName, "CreateDate"].Y, x => x.CreateDate.ToString("yyyy-MM-dd")))
                  );

                //导出文件
                ExportHelper.ExportToWeb(xlsPath, filename + ".xls", new SheetFormatterContainer(TableSheetName, formatters));

               
            }
            catch (Exception ex)
            {
                WriteExceptionLog(ex);
            }
        }

    }
}
