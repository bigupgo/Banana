using Banana.Core;
using Banana.Core.Common;
using Banana.Core.Db;
using Banana.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Bll
{
    public class UserInfoBll
    {
        DbHelper db = null;
        public UserInfoBll()
        {
            db = new DbHelper();
        }

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
    }
}
