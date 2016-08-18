﻿using Banana.Core.Base;
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
    public abstract class BaseBLL<TEntity> where TEntity: EntityObject
    {
   
        public IRepository<TEntity> Repository;
        public RepositoryFactory RepositoryFactory = new RepositoryFactory();
        public BaseBLL()
        {
        }

        protected string GetGUID()
        {
            return Util.GetGUID();
        }

        public void WriteExceptionLog(Exception ex)
        {
            LogHelper.LogError(ex.Message, ex);
        }
        public virtual TEntity GetByKey(params object[] id)
        {
            TEntity byKey;
            if (null == id)
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
            return this.Repository.GetList();
        }

        public int SaveChange()
        {
            return this.Repository.SaveChange();
        }

        public abstract void SetEntityName();
        public abstract void SetRepository();

        public string EntityName { get; set; }

        private Type IsDelType { get; set; }

        public bool IsSoftDelete { get; private set; }

        public bool IsWriteLog { get; set; }
    }
}
