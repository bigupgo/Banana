using Banana.Core;
using Banana.Core.Common;
using Banana.Core.Db;
using Banana.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Banana.Bll
{
    public class UserInfoBll:BaseBll
    {
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
                string sql = @"select * from UserInfo where 1=1 ";
                if (!string.IsNullOrEmpty(search))
                {
                    sql += string.Format(" and Name like '%{0}%'", search);
                }
                var cmd = db.GetSqlStringCommond(sql);
                DataTable dt = db.ExecuteDataTable(cmd);

                var data  = ERTools.GetList<UserInfo>(dt);

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

                string sql = @"insert into UserInfo(Name,Age,Email) values(@Name,@Age,@Email)";
                var cmd = db.GetSqlStringCommond(sql);
                db.AddInParameter(cmd, "Name", DbType.String, userInfo.Name);
                db.AddInParameter(cmd, "Age", DbType.Int32, userInfo.Age);
                db.AddInParameter(cmd, "Email", DbType.String, userInfo.Email);

                int num = db.ExecuteNonQuery(cmd);
                result.success = num > 0;
            }
            catch (Exception e)
            {
                result.success = false;
                LogHelper.LogError(e.Message);
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
                string sql = @"update UserInfo set Name=@Name,Age=@Age,Email=@Email where Id= @Id";
                var cmd = db.GetSqlStringCommond(sql);
                db.AddInParameter(cmd, "Name", DbType.String, userInfo.Name);
                db.AddInParameter(cmd, "Age", DbType.Int32, userInfo.Age);
                db.AddInParameter(cmd, "Email", DbType.String, userInfo.Email);
                db.AddInParameter(cmd, "Id", DbType.Int32, userInfo.Id);

                int num = db.ExecuteNonQuery(cmd);
               result.success =  num > 0;
            }
            catch (Exception e)
            {
                result.success = false;
                LogHelper.LogError(e.Message);
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
                string sql = "delete from UserInfo where Id=@Id";
                if (!String.IsNullOrEmpty(ids))
                {
                    string[] array = ids.Split(',');
                    foreach (string id in array)
                    {
                        var cmd = db.GetSqlStringCommond(sql);
                        db.AddInParameter(cmd, "Id", DbType.Int32, Convert.ToInt32(id));
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
            return url;
        }
    }
}
