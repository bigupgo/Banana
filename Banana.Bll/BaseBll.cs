using Banana.Core.Common;
using System;
using System.Collections.Generic;
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
    }
}
