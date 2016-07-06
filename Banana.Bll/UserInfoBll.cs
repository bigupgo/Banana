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
        DbHelper db = null;
        public UserInfoBll()
        {
            db = new DbHelper();
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
                DataTable dt = GetAllData(TABLE_NAME, db);
                var data = ERTools.GetList<UserInfo>(dt);
                pagetion.total = data.Count();

                list = data.OrderBy(x => x.Id).Skip(pagetion.rows * (pagetion.page - 1)).Take(pagetion.rows).ToList();
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
            AjaxReturn result = new AjaxReturn();
            try
            {
                string sql = @"insert into Ba_UserInfo(Id,LoginName,RealName,Password,Phone,Email,CreateDate,UserIcon,IsDel) values(@Id,@LoginName,@RealName,@Password,@Phone,@Email,@CreateDate,@UserIcon,@IsDel)";
                var cmd = db.GetSqlStringCommond(sql);
                db.AddInParameter(cmd, "Id", DbType.String, GetGUID());
                db.AddInParameter(cmd, "LoginName", DbType.String, userInfo.LoginName);
                db.AddInParameter(cmd, "RealName", DbType.String, userInfo.RealName);
                db.AddInParameter(cmd, "Password", DbType.Int32, userInfo.Password);
                db.AddInParameter(cmd, "Phone", DbType.String, userInfo.Phone == null ? "" : userInfo.Phone);
                db.AddInParameter(cmd, "Email", DbType.String, userInfo.Email == null ? "" : userInfo.Email);

                db.AddInParameter(cmd, "CreateDate", DbType.DateTime, System.DateTime.Now);
                db.AddInParameter(cmd, "UserIcon", DbType.String, userInfo.UserIcon == null ? "" : userInfo.UserIcon);

                db.AddInParameter(cmd, "IsDel", DbType.Boolean, false);

                int num = db.ExecuteNonQuery(cmd);
                result.success = num > 0;
            }
            catch (Exception e)
            {
                result.success = false;
                WriteExceptionLog(e);
            }
            result.SetMessage("添加成功", "添加失败");
            return result;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public AjaxReturn Edit(UserInfo userInfo)
        {
            AjaxReturn result = new AjaxReturn();
            try
            {
                string sql = @"update Ba_UserInfo set LoginName=@LoginName,RealName=@RealName,Password=@Password,Phone=@Phone,Email=@Email,UserIcon=@UserIcon where Id= @Id";
                var cmd = db.GetSqlStringCommond(sql);
                db.AddInParameter(cmd, "LoginName", DbType.String, userInfo.LoginName);
                db.AddInParameter(cmd, "RealName", DbType.String, userInfo.RealName);
                db.AddInParameter(cmd, "Password", DbType.Int32, userInfo.Password);
                db.AddInParameter(cmd, "Phone", DbType.String, userInfo.Phone);
                db.AddInParameter(cmd, "Email", DbType.String, userInfo.Email);
                db.AddInParameter(cmd, "UserIcon", DbType.String, userInfo.UserIcon);
                db.AddInParameter(cmd, "Id", DbType.String, userInfo.Id);

                int num = db.ExecuteNonQuery(cmd);
                result.success = num > 0;
            }
            catch (Exception e)
            {
                result.success = false;
                WriteExceptionLog(e);
            }
            result.SetMessage("修改成功", "修改失败");

            return result;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public AjaxReturn Delete(String ids)
        {
            AjaxReturn result = new AjaxReturn();
            Trans trans = new Trans();
            try
            {
                string sql = "update Ba_UserInfo set IsDel = 1 where Id=@Id";
                if (!String.IsNullOrEmpty(ids))
                {
                    string[] array = ids.Split(',');
                    foreach (string id in array)
                    {
                        var cmd = db.GetSqlStringCommond(sql);
                        db.AddInParameter(cmd, "Id", DbType.String, id);
                        db.ExecuteNonQuery(cmd, trans);
                    }

                    trans.Commit();
                }
            }
            catch (Exception e)
            {
                trans.RollBack();
                result.success = false;
                LogHelper.LogError(e.Message);
            }
            finally
            {
                if (trans != null)
                {
                    trans.Close();
                }
            }
            result.SetMessage("删除成功", "删除失败");
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


        public void ExportExcel(string search)
        {
            try
            {
                //获取数据源
                DataTable dt = GetAllData(TABLE_NAME, db);;
                List<UserInfo> list = ERTools.GetList<UserInfo>(dt);


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
