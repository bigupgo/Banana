using Banana.Core.Base;
using Banana.Core.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Banana.Bll
{
    public abstract class BaseSqlBll<T>
    {
        public static DbHelper db = null;

        public BaseSqlBll()
        {
            db = new DbHelper();
            this.SetTableName();
        }
        /// <summary>
        /// 要操作的表名
        /// </summary>
        public string TableName { get; set; }
        public abstract void SetTableName();
        
        /// <summary>
        /// 设置GUID
        /// </summary>
        /// <returns></returns>
        protected string GetGUID()
        {
            return Util.GetGUID();
        }

        /// <summary>
        /// 添加异常日志
        /// </summary>
        /// <param name="ex"></param>
        public void WriteExceptionLog(Exception ex)
        {
            LogHelper.LogError(ex.Message, ex);
        }

        /// <summary>
        /// 获取操作表内所有数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            DataTable dt = null;
            try
            {
                string sql = string.Format("select * from {0} where IsDel = 0", TableName);
                var cmd = db.GetSqlStringCommond(sql);
                dt = db.ExecuteDataTable(cmd);
            }
            catch (Exception e)
            {
                LogHelper.LogError(e.Message);
            }
            return dt;
        }

        public List<T> GetList()
        {
            List<T> list = new List<T>();
            try
            {

                string sql = string.Format("select * from {0} where IsDel = 0", TableName);
                var cmd = db.GetSqlStringCommond(sql);
                DataTable dt = db.ExecuteDataTable(cmd);
                if (dt != null)
                {
                    list = ERTools.GetList<T>(dt);
                }
            }
            catch (Exception e)
            {
                LogHelper.LogError(e.Message);
            }
            return list;
        }

        public AjaxReturn Add(T t)
        {
            AjaxReturn result = new AjaxReturn();
            try
            {
                int num = 0;
                List<string> listFiled = new List<string>();
                Type tt = t.GetType();
                PropertyInfo[] propertypes = tt.GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    listFiled.Add(pro.Name);
                }
                string fileds = "";
                string values = "";

                for (int i = 0; i < listFiled.Count(); i++)
                {
                    if (i == (listFiled.Count() - 1))
                    {
                        fileds += listFiled[i];
                        values += "@" + listFiled[i];
                    }
                    else
                    {
                        fileds += listFiled[i] + ",";
                        values += "@" + listFiled[i] + ",";
                    }
                }

                //设置添加固定值
                tt.GetProperty("Id").SetValue(t, GetGUID());
                tt.GetProperty("CreateDate").SetValue(t, System.DateTime.Now);
                tt.GetProperty("IsDel").SetValue(t, false);

                string sql = string.Format("insert into {0}({1}) values({2})", TableName, fileds, values);
                var cmd = db.GetSqlStringCommond(sql);

                foreach (PropertyInfo pro in propertypes)
                {
                    object value = t.GetType().GetProperty(pro.Name).GetValue(t);
                    db.AddInParameter(cmd, pro.Name, ConvertDbType(pro.PropertyType), value == null ? "" : value);
                }
                num = db.ExecuteNonQuery(cmd);
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

        public AjaxReturn Edit(T t, List<string> paramExclude = null)
        {
            AjaxReturn result = new AjaxReturn();
            try
            {
                if (paramExclude == null)
                {
                    paramExclude = new List<string>();
                }
                paramExclude.Add("Id");
                paramExclude.Add("CreateDate");
                paramExclude.Add("IsDel");
                int num = 0;
                List<string> listFiled = new List<string>();
                Type tt = t.GetType();
                PropertyInfo[] propertypes = tt.GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    if (!paramExclude.Contains(pro.Name))
                    {
                        listFiled.Add(pro.Name);
                    }
                }
                string fileds = "";
                for (int i = 0; i < listFiled.Count(); i++)
                {

                    if (i == (listFiled.Count() - 1))
                    {

                        fileds += listFiled[i] + "=@" + listFiled[i];
                    }
                    else
                    {
                        fileds += listFiled[i] + "=@" + listFiled[i] + ",";
                    }
                }
                object id = t.GetType().GetProperty("Id").GetValue(t);
                string sql = string.Format("update {0} set {1} where Id= '{2}'", TableName, fileds, id);
                var cmd = db.GetSqlStringCommond(sql);
                foreach (PropertyInfo pro in propertypes)
                {
                    if (!paramExclude.Contains(pro.Name))
                    {
                        object value = t.GetType().GetProperty(pro.Name).GetValue(t);
                        db.AddInParameter(cmd, pro.Name, ConvertDbType(pro.PropertyType), value == null ? "" : value);
                    }
                }

                num = db.ExecuteNonQuery(cmd);
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

        public AjaxReturn Delete(string ids)
        {
            AjaxReturn result = new AjaxReturn();
            Trans trans = new Trans();
            try
            {
                string sql =string.Format("update {0} set IsDel = 1 where Id=@Id",TableName);
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

        public T GetEntity(string Id)
        {
            T t = default(T);
            try
            {
                string sql = string.Format("select * from {0} where Id ='{1}'", TableName, Id);
                DataTable dt = db.ExecuteDataTable(sql);
                t = ERTools.GetFirst<T>(dt);
            }
            catch (Exception e)
            {
                WriteExceptionLog(e);
            }

            return t;
        }

        public DbType ConvertDbType(Type type)
        {
            if (type.Equals(typeof(Int16)))
            {
                return DbType.Int16;
            }
            else if (type.Equals(typeof(Int32)))
            {
                return DbType.Int32;
            }
            else if (type.Equals(typeof(Int64)))
            {
                return DbType.Int64;
            }
            else if (type.Equals(typeof(UInt16)))
            {
                return DbType.UInt16;
            }
            else if (type.Equals(typeof(UInt32)))
            {
                return DbType.UInt32;
            }
            else if (type.Equals(typeof(UInt64)))
            {
                return DbType.UInt64;
            }
            else if (type.Equals(typeof(Boolean)))
            {
                return DbType.Boolean;
            }
            else if (type.Equals(typeof(Byte)))
            {
                return DbType.Byte;
            }
            else if (type.Equals(typeof(Char)))
            {
                return DbType.String;
            }
            else if (type.Equals(typeof(DateTime)))
            {
                return DbType.DateTime;
            }
            else if (type.Equals(typeof(Decimal)))
            {
                return DbType.Decimal;
            }
            else if (type.Equals(typeof(Double)))
            {
                return DbType.Double;
            }
            else if (type.Equals(typeof(SByte)))
            {
                return DbType.SByte;
            }
            else if (type.Equals(typeof(Single)))
            {
                return DbType.Single;
            }
            else if (type.Equals(typeof(String)))
            {
                return DbType.String;
            }
            else
            {
                return DbType.String;
            }
          
        }

        public List<string> GetFileds()
        {
            List<string> list = new List<string>();
            T t = Activator.CreateInstance<T>();
            PropertyInfo[] propertypes = t.GetType().GetProperties();
            foreach (PropertyInfo pro in propertypes)
            {
               list.Add( pro.Name);
            }
            return list;
        }


        public SessionUser GetCurrentUser()
        {
          return  ContextHelper.GetCurrentUser();
        }
    }
}
