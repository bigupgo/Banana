using Banana.Core.Base;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Bll.Base
{
    public abstract class BaseBD<TEntity> : BaseBll where TEntity : EntityObject
    {
        
        public IRepository<TEntity> Repository;
        private Expression<Func<TEntity, bool>> softDelWhereLamb;
        public string TickTimeoutMsg;
        
        public BaseBD(): this( new CommonFields())
        {
        }

        public BaseBD(CommonFields commonFields)
        {
            this.TickTimeoutMsg = "当前记录已经被改变，请刷新后重试。";
            this.ComFields = commonFields;
            this.IsWriteLog = true;
            this.initSoftDelete();
            this.SetRepository();
            this.SetEntityName();
        }
        
        public virtual AjaxReturn Add(TEntity entity)
        {
            AjaxReturn return3;
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            AjaxReturn return2 = new AjaxReturn();
            try
            {
                this.AttachCommonFieldsForAdd(entity);
                return2.success =this.Repository.Add(entity, true);
                return2.SetMessage("添加成功！", "添加失败！");
                return3 = return2;
            }
            catch (Exception exception)
            {
                return2.success=false;
                return2.message=exception.Message;
                return2.Exception=exception;
                return3 = return2;
            }
            finally
            {
                //base.WriteLog(string.Concat((string[]) new string[] { "添加", this.EntityName, " 操作结果为：【", return2.get_message(), "】" }), true);
            }
            return return3;
        }
        
        public void AttachCommonFieldsForAdd(TEntity entity)
        {
            Func<PropertyInfo, bool> func = null;
            Type type = typeof(TEntity);
            PropertyInfo property = type.GetProperty(this.Repository.GetKeyProperty(type));
            if (property == null)
            {
                throw new Exception("TBaseBLL主键必须为ID");
            }
            if (property.PropertyType.Name == "String")
            {
                property.SetValue(entity, Util.GetGUID(), (object[]) null);
            }
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            PropertyInfo info2 = null;
            if (this.IsSoftDelete)
            {
                if (func == null)
                {
                    func = a => (bool)(a.Name == this.ComFields.IsDel);
                }
                info2 = Enumerable.SingleOrDefault<PropertyInfo>(properties, func);
                if (this.IsDelType.Equals(typeof(bool)))
                {
                    info2.SetValue(entity, false, null);
                }
                else
                {
                    info2.SetValue(entity, "N", null);
                }
            }
          
        }
        
        public void AttachFieldsForEdit(TEntity source, TEntity target)
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            for (int i = 0; i < properties.Length; i = (int) (i + 1))
            {
                PropertyInfo info = properties[i];
            }
        }
        
        public void AttachFieldsForEdit(TEntity source, TEntity target, string[] fields)
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            for (int i = 0; i < properties.Length; i = (int) (i + 1))
            {
                PropertyInfo info = properties[i];
                if (Enumerable.Contains<string>(fields, info.Name) && (info.PropertyType.BaseType != typeof(EntityReference)))
                {
                    info.SetValue(target, info.GetValue(source, (object[]) null), null);
                }
            }
        }
        
        private Expression<Func<TEntity, bool>> createSoftDelExpr(Type isDelType)
        {
            ParameterExpression expression;
            ConstantExpression right = null;
            if (isDelType.Equals(typeof(bool)))
            {
                right = Expression.Constant(false);
            }
            else
            {
                right = Expression.Constant("N");
            }
            return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(Expression.PropertyOrField(expression = Expression.Parameter(typeof(TEntity), "t"), this.ComFields.IsDel), right), new ParameterExpression[] { expression });
        }
       
        
        public virtual AjaxReturn Delete(TEntity entity)
        {
            AjaxReturn return3;
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            AjaxReturn return2 = new AjaxReturn();
            try
            {
                TEntity byKeys = this.Repository.GetByKeys(entity);
                if (byKeys == null)
                {
                    throw new Exception("根据id没有找到当前对象");
                }
                if (this.IsSameTimeStamp(entity, byKeys))
                {
                    if (this.IsSoftDelete)
                    {
                        if (this.IsDelType.Equals(typeof(bool)))
                        {
                            entity.GetType().GetProperty(this.ComFields.IsDel).SetValue(byKeys, true, null);
                        }
                        else
                        {
                            entity.GetType().GetProperty(this.ComFields.IsDel).SetValue(byKeys, "Y", null);
                        }
                        return2.success =this.Repository.SaveChange() > 0;
                    }
                    else
                    {
                        return2.success= this.Repository.Delete(byKeys, true);
                    }
                    return2.SetMessage("删除成功!", "删除失败");
                }
                else
                {
                    return2.message = this.TickTimeoutMsg;
                    return2.success =false;
                }
                return3 = return2;
            }
            catch (Exception exception)
            {
                return2.success=false;
                return2.message=exception.Message;
                return2.Exception=exception;
                return3 = return2;
            }
            finally
            {
               // base.WriteLog(string.Concat((string[]) new string[] { "删除", this.EntityName, " 操作结果为：【", return2.get_message(), "】" }), true);
            }
            return return3;
        }
        
        public virtual AjaxReturn Delete(params object[] id)
        {
            AjaxReturn return3;
            if (id == null)
            {
                throw new ArgumentNullException();
            }
            AjaxReturn return2 = new AjaxReturn();
            try
            {
                TEntity byKey = this.Repository.GetByKey(id);
                if (byKey == null)
                {
                    throw new Exception("根据id没有找到当前对象");
                }
                if (this.IsSoftDelete)
                {
                    if (this.IsDelType.Equals(typeof(bool)))
                    {
                        byKey.GetType().GetProperty(this.ComFields.IsDel).SetValue(byKey, true, null);
                    }
                    else
                    {
                        byKey.GetType().GetProperty(this.ComFields.IsDel).SetValue(byKey, "Y", null);
                    }
                    return2.success=this.Repository.SaveChange() > 0;
                }
                else
                {
                    return2.success=this.Repository.Delete(byKey, true);
                }
                return2.SetMessage("删除成功!", "删除失败");
                return3 = return2;
            }
            catch (Exception exception)
            {
                return2.success=false;
                return2.message=exception.Message;
                return2.Exception=exception;
                return3 = return2;
            }
            finally
            {
               // base.WriteLog(string.Concat((string[]) new string[] { "删除", this.EntityName, " 操作结果为：【", return2.get_message(), "】" }), true);
            }
            return return3;
        }
        
        public virtual AjaxReturn DeleteByIDs(string ids)
        {
            AjaxReturn return3;
            if (string.IsNullOrEmpty(ids))
            {
                throw new ArgumentNullException();
            }
            AjaxReturn return2 = new AjaxReturn();
            try
            {
                string[] strArray = ids.Split((char[]) new char[] { ',' });
                if ((strArray == null) || (strArray.Length == 0))
                {
                    throw new ArgumentNullException();
                }
                typeof(TEntity).GetProperty(this.ComFields.IsDel);
                string[] strArray2 = strArray;
                for (int i = 0; i < strArray2.Length; i = (int) (i + 1))
                {
                    string str = strArray2[i];
                    TEntity byKey = this.Repository.GetByKey(new object[] { str });
                    if (this.IsSoftDelete)
                    {
                        if (this.IsDelType.Equals(typeof(bool)))
                        {
                            byKey.GetType().GetProperty(this.ComFields.IsDel).SetValue(byKey, true, null);
                        }
                        else
                        {
                            byKey.GetType().GetProperty(this.ComFields.IsDel).SetValue(byKey, "Y", null);
                        }
                    }
                    else
                    {
                        return2.success=this.Repository.Delete(byKey, false);
                    }
                }
                return2.success=this.Repository.SaveChange() > 0;
                return2.SetMessage("删除记录成功", "删除记录失败");
                return3 = return2;
            }
            catch (Exception exception)
            {
                return2.success=false;
                return2.message=exception.Message;
                return2.Exception=exception;
                return3 = return2;
            }
            finally
            {
              //  base.WriteLog(string.Concat((string[]) new string[] { "删除", this.EntityName, " 操作结果为：【", return2.get_message(), "】" }), true);
            }
            return return3;
        }
        
        public virtual AjaxReturn Edit(TEntity entity)
        {
            AjaxReturn return3;
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            AjaxReturn return2 = new AjaxReturn();
            try
            {
                TEntity byKeys = this.Repository.GetByKeys(entity);
                if (this.IsSameTimeStamp(entity, byKeys))
                {
                    this.AttachFieldsForEdit(entity, byKeys);
                    return2.success=this.Repository.Update(byKeys, true);
                    return2.SetMessage("编辑成功！", "编辑失败");
                }
                else
                {
                    return2.success=false;
                    return2.message=this.TickTimeoutMsg;
                }
                return3 = return2;
            }
            catch (Exception exception)
            {
                return2.success=false;
                return2.message=exception.Message;
                return2.Exception=exception;
                return3 = return2;
            }
            finally
            {
                //base.WriteLog(string.Concat((string[]) new string[] { "编辑", this.EntityName, " 操作结果为：【", return2.get_message(), "】" }), true);
            }
            return return3;
        }
        
        public virtual AjaxReturn Edit(TEntity entity, string[] editFields)
        {
            AjaxReturn return3;
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            AjaxReturn return2 = new AjaxReturn();
            try
            {
               
                TEntity byKeys = this.Repository.GetByKeys(entity);
                if (this.IsSameTimeStamp(entity, byKeys))
                {
                    this.AttachFieldsForEdit(entity, byKeys, editFields);
                    return2.success=this.Repository.Update(byKeys, true);
                    return2.SetMessage("编辑成功！", "编辑失败！");
                }
                else
                {
                    return2.success=false;
                    return2.message=this.TickTimeoutMsg;
                }
                return3 = return2;
            }
            catch (Exception exception)
            {
                return2.success=false;
                return2.message=exception.Message;
                return2.Exception=exception;
                return3 = return2;
            }
            finally
            {
               // base.WriteLog(string.Concat((string[]) new string[] { "编辑", this.EntityName, " 操作结果为：【", return2.get_message(), "】" }), true);
            }
            return return3;
        }
        
        public virtual TEntity GetByKey(params object[] id)
        {
            TEntity byKey;
            if (id == null)
            {
                throw new ArgumentNullException();
            }
            try
            {
                byKey = this.Repository.GetByKey(id);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return byKey;
        }
        
        
        public virtual IQueryable<TEntity> GetList()
        {
            if (this.IsSoftDelete)
            {
                return this.Repository.Where(this.softDelWhereLamb);
            }
            return this.Repository.GetList();
        }
        
        public virtual IQueryable<TEntity> GetList(Pagetion pagetion)
        {
            if (pagetion == null)
            {
                throw new ArgumentNullException();
            }
            return this.GetList(pagetion, false);
        }
        
        public virtual IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> whereLamb)
        {
            if (this.IsSoftDelete)
            {
                return this.Repository.Where(DynamicLinqExpressions.And<TEntity>(this.softDelWhereLamb, whereLamb));
            }
            return this.Repository.Where(whereLamb);
        }
        
        public virtual IQueryable<TEntity> GetList(Pagetion pagetion, bool orderbyAscCreateTime)
        {
            return this.GetList(null, pagetion, orderbyAscCreateTime);
        }
        
        public virtual IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> whereLamb, Pagetion pagetion, bool orderByAscCreateTime)
        {
            ParameterExpression expression;
            if (pagetion == null)
            {
                throw new ArgumentNullException("参数不能为空！");
            }
            Type type = typeof(TEntity);
            IQueryable<TEntity> list = null;
            if (whereLamb == null)
            {
                list = this.GetList();
            }
            else
            {
                list = this.GetList(whereLamb);
            }
            string str = orderByAscCreateTime ? ((string) "OrderBy") : ((string) "orderbydescending");
            MethodCallExpression expression2 = null;
            if (type.GetProperty(this.ComFields.CreateTime) != null)
            {
                expression2 = Expression.Call(typeof(Queryable), str, new Type[] { type, typeof(DateTime) }, new Expression[] { Expression.Constant(list), Expression.Lambda(Expression.Property(expression = Expression.Parameter(typeof(TEntity), "t"), this.ComFields.CreateTime), new ParameterExpression[] { expression }) });
            }
            if (expression2 == null)
            {
                throw new Exception("目标表至少要有CreateTime");
            }
            IQueryable<TEntity> queryable2 = Queryable.Take<TEntity>(Queryable.Skip<TEntity>(list.Provider.CreateQuery<TEntity>(expression2), (int) (pagetion.rows * (pagetion.page - 1))), pagetion.rows);
            pagetion.total= Queryable.Count<TEntity>(list);
            return queryable2;
        }
        
        public IQueryable<TEntity> GetList<orderType>(Expression<Func<TEntity, bool>> whereLamb, Expression<Func<TEntity, orderType>> orderName, bool isASC, Pagetion pagetion)
        {
            IQueryable<TEntity> list = this.GetList(whereLamb);
           pagetion.total= Queryable.Count<TEntity>(list);
            if (isASC)
            {
                return Queryable.Take<TEntity>(Queryable.Skip<TEntity>(Queryable.Where<TEntity>(list, whereLamb).OrderBy<TEntity, orderType>(orderName), (int) ((pagetion.page - 1) * pagetion.rows)), pagetion.rows).AsQueryable<TEntity>();
            }
            return Queryable.Take<TEntity>(Queryable.Skip<TEntity>(Queryable.Where<TEntity>(list, whereLamb).OrderByDescending<TEntity, orderType>(orderName), (int) ((pagetion.page - 1) * pagetion.rows)), pagetion.rows).AsQueryable<TEntity>();
        }
        
        private void initSoftDelete()
        {
            PropertyInfo info = Enumerable.SingleOrDefault<PropertyInfo>(typeof(TEntity).GetProperties(), (Func<PropertyInfo, bool>) (a => a.Name.Equals(this.ComFields.IsDel, StringComparison.InvariantCultureIgnoreCase)));
            this.IsSoftDelete = (bool) (info != null);
            if (this.IsSoftDelete)
            {
                this.IsDelType = info.PropertyType;
                this.ComFields.IsDel=info.Name;
                this.softDelWhereLamb = this.createSoftDelExpr(this.IsDelType);
            }
        }
        
        protected bool IsSameTimeStamp(TEntity target, TEntity source)
        {
            if ((target == null) || (source == null))
            {
                throw new ArgumentNullException("原始对象或者目标对象不能为空");
            }
            PropertyInfo property = typeof(TEntity).GetProperty(this.ComFields.TimeStamp);
            if (property == null)
            {
                return true;
            }
            if (property.GetValue(target, (object[]) null) == null)
            {
                throw new Exception("在修改记录时候，需要带上时间戳值");
            }
            return Util.EqualByteArray((byte[]) property.GetValue(target, (object[]) null), (byte[]) property.GetValue(source, (object[]) null));
        }
        
        public int SaveChange()
        {
            return this.Repository.SaveChange();
        }
        
        public abstract void SetEntityName();
        public abstract void SetRepository();
        
        public CommonFields ComFields { get; set; }
        
        public string EntityName { get; set; }
        
        private Type IsDelType { get; set; }
        
        public bool IsSoftDelete { get; private set; }
        
        public bool IsWriteLog { get; set; }
    }
}
