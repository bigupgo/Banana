using Banana.Core.Common;
using Banana.Core.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Bll
{
    public abstract class BaseBll
    {
        protected string GetGUID()
        {
            return Util.GetGUID();
        }

        public void WriteExceptionLog(Exception ex)
        {
            LogHelper.LogError(ex.Message, ex);
        }

        public DataTable GetAllData(string tableName, DbHelper db)
        {
            DataTable dt = null;
            try
            {
                string sql = string.Format("select * from {0} where IsDel = 0", tableName);
                var cmd = db.GetSqlStringCommond(sql);
                dt = db.ExecuteDataTable(cmd);
            }
            catch (Exception e)
            {
                LogHelper.LogError(e.Message);
            }
            return dt;
        }
    }
}
