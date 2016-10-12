using Banana.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Bll.Base
{
    public abstract class BaseBll
    {
       
        public RepositoryFactory RepositoryFactory = new RepositoryFactory();

        protected string GetGUID()
        {
            return Util.GetGUID();
        }

        public void WriteExceptionLog(Exception ex)
        {
            LogHelper.LogError(ex.Message, ex);
        }

    }
}
