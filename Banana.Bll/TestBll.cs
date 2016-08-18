using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Bll.Base;
using Banana.DBModel;

namespace Banana.Bll
{
    public class TestBll : BaseBLL<Ba_UserInfo>
    {

        public override void SetEntityName()
        {
            this.EntityName = "测试";
        }

        public override void SetRepository()
        {
            Repository = RepositoryFactory.GetBusinessRepository<Ba_UserInfo>();
        }

        public void GetlistTest()
        {
            try
            {
                var data = Repository.GetList();
            }
            catch (Exception e)
            {
            }
          
        }
    }
}
