using Aspose.Words;
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

        public void GetDocText(string path)
        {
            path = "d:\\《类似商品和服务区分表--基于尼斯分类第十版》2016文本 .doc";
            Document doc = new Document(path);
            string str = doc.GetText();
        }
    }
}
