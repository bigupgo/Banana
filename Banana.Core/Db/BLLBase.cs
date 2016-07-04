using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Banana.Core.Db
{
    public class BLLBase
    {
        protected DbHelper db = new DbHelper();

        public long GetLastInsertId()
        {
            //sqlite
            //return long.Parse(this.db.ExecuteScalar("select last_insert_rowid();").ToString());
            //mysql
            //return long.Parse(this.db.ExecuteScalar("select LAST_INSERT_ID()").ToString());
            //sql server 
            var val = this.db.ExecuteScalar("select @@identity").ToString();
            return long.Parse(val);
        }

        public long GetLastInsertId(Trans trans)
        {
            return long.Parse(this.db.ExecuteScalar("select @@identity", trans).ToString());
        }
    }
}