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
            var data = Repository.GetList();
        }
    }
}
