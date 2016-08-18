using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Banana.Core.Base
{
    public class BaseRepository<TContext, TEntity> : IRepository<TEntity>
        where TContext : ObjectContext, new()
        where TEntity : EntityObject
    {
        protected readonly TContext Context;
        protected readonly ObjectSet<TEntity> Entities;

        public BaseRepository()
        {
            this.Context = ContextManager.Instance<TContext>();
            this.Entities = this.Context.CreateObjectSet<TEntity>();
        }

        public virtual bool Add(TEntity entity, bool saveChange)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            this.Entities.AddObject(entity);
            if (saveChange)
            {
                return (this.SaveChange() > 0);
            }
            return true;
        }

        public virtual bool Delete(TEntity entity, bool saveChange)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            this.Context.ObjectStateManager.ChangeObjectState(entity, EntityState.Deleted);
            if (saveChange)
            {
                return (this.SaveChange() > 0);
            }
            return true;
        }

        public virtual bool Delete(IEnumerable<TEntity> items, bool saveChange)
        {
            if (items != null)
            {
                foreach (TEntity local in items)
                {
                    this.Context.ObjectStateManager.ChangeObjectState(local, EntityState.Deleted);
                }
            }
            if (saveChange)
            {
                return (this.SaveChange() > 0);
            }
            return true;
        }

        public virtual bool Delete(Expression<Func<TEntity, bool>> whereLamb, bool saveChange)
        {
            if (whereLamb == null)
            {
                throw new ArgumentNullException();
            }
            IQueryable<TEntity> queryable = this.Entities.Where<TEntity>(whereLamb);
            if (queryable != null)
            {
                foreach (TEntity local in queryable)
                {
                    this.Context.ObjectStateManager.ChangeObjectState(local, EntityState.Deleted);
                }
            }
            if (saveChange)
            {
                return (this.SaveChange() > 0);
            }
            return true;
        }

        public int ExecuteStoreCommand(string commandText, params object[] para)
        {
            return this.Context.ExecuteStoreCommand(commandText, para);
        }

        public ObjectResult<TEntity> ExecuteStoreQuery(string commandText, params object[] para)
        {
            return this.Context.ExecuteStoreQuery<TEntity>(commandText, para);
        }

        public virtual TEntity GetByKey(params object[] id)
        {
            int index = 0;
            List<PropertyInfo> list = new List<PropertyInfo>();
            List<EntityKeyMember> entityKeyValues = new List<EntityKeyMember>();
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            foreach (PropertyInfo info in properties)
            {
                object[] customAttributes = info.GetCustomAttributes(true);
                foreach (object obj2 in customAttributes)
                {
                    if ((obj2 is EdmScalarPropertyAttribute) && ((obj2 as EdmScalarPropertyAttribute).EntityKeyProperty && !(obj2 as EdmScalarPropertyAttribute).IsNullable))
                    {
                        if (info.PropertyType == typeof(int))
                        {
                            int keyValue = Convert.ToInt32(id[index]);
                            entityKeyValues.Add(new EntityKeyMember(info.Name, keyValue));
                        }
                        else
                        {
                            entityKeyValues.Add(new EntityKeyMember(info.Name, id[index]));
                        }
                        index++;
                    }
                }
            }
            return (this.Context.GetObjectByKey(new EntityKey(this.Context.DefaultContainerName + "." + typeof(TEntity).Name, entityKeyValues)) as TEntity);
        }

        public virtual TEntity GetByKeys(TEntity entity)
        {
            List<string> keysProperties = this.GetKeysProperties(typeof(TEntity));
            if (keysProperties.Count < 1)
            {
                throw new Exception("不是联合主键对象");
            }
            List<KeyValuePair<string, object>> entityKeyValues = new List<KeyValuePair<string, object>>();
            Type type = typeof(TEntity);
            foreach (string str in keysProperties)
            {
                object obj2 = type.GetProperty(str).GetValue(entity, null);
                entityKeyValues.Add(new KeyValuePair<string, object>(str, obj2));
            }
            EntityKey key = new EntityKey(this.Context.DefaultContainerName + "." + typeof(TEntity).Name, entityKeyValues);
            return (this.Context.GetObjectByKey(key) as TEntity);
        }

        public virtual TEntity GetEntity(Expression<Func<TEntity, bool>> whereLamb)
        {
            return this.Entities.SingleOrDefault<TEntity>(whereLamb);
        }

        public virtual string GetKeyProperty(Type entityType)
        {
            foreach (PropertyInfo info in entityType.GetProperties())
            {
                EdmScalarPropertyAttribute attribute = info.GetCustomAttributes(typeof(EdmScalarPropertyAttribute), false).FirstOrDefault<object>() as EdmScalarPropertyAttribute;
                if ((attribute != null) && attribute.EntityKeyProperty)
                {
                    return info.Name;
                }
            }
            return null;
        }

        public virtual List<string> GetKeysProperties(Type entityType)
        {
            List<string> list = new List<string>();
            foreach (PropertyInfo info in entityType.GetProperties())
            {
                EdmScalarPropertyAttribute attribute = info.GetCustomAttributes(typeof(EdmScalarPropertyAttribute), false).FirstOrDefault<object>() as EdmScalarPropertyAttribute;
                if ((attribute != null) && attribute.EntityKeyProperty)
                {
                    list.Add(info.Name);
                }
            }
            return list;
        }

        public virtual IQueryable<TEntity> GetList()
        {
            return this.Entities;
        }

        public virtual IQueryable<TEntity> GetList<orderType>(Expression<Func<TEntity, bool>> whereLamb, Expression<Func<TEntity, orderType>> orderName, bool isASC, Pagetion pagetion)
        {
            pagetion.total = this.GetTotal(whereLamb);
            if (isASC)
            {
                return this.Entities.Where<TEntity>(whereLamb).OrderBy<TEntity, orderType>(orderName).Skip<TEntity>(((pagetion.page - 1) * pagetion.rows)).Take<TEntity>(pagetion.rows).AsQueryable<TEntity>();
            }
            return this.Entities.Where<TEntity>(whereLamb).OrderByDescending<TEntity, orderType>(orderName).Skip<TEntity>(((pagetion.page - 1) * pagetion.rows)).Take<TEntity>(pagetion.rows).AsQueryable<TEntity>();
        }

        public virtual int GetTotal()
        {
            return this.Entities.Count<TEntity>();
        }

        public virtual int GetTotal(Expression<Func<TEntity, bool>> whereLamb)
        {
            return this.Entities.Count<TEntity>(whereLamb);
        }

        public virtual int SaveChange()
        {
            int num;
            try
            {
                num = this.Context.SaveChanges();
            }
            catch (Exception exception)
            {
                LogHelper.LogError("保存上下文失败", exception);
                throw;
            }
            return num;
        }

        public virtual bool Update(TEntity entity, bool saveChange)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            this.Context.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
            if (saveChange)
            {
                return (this.SaveChange() > 0);
            }
            return true;
        }

        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> whereLamb)
        {
            return this.Entities.Where<TEntity>(whereLamb);
        }
    }
}
