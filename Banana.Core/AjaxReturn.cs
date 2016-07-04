using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core
{
    public class AjaxReturn
    {
        public AjaxReturn()
        {
            this.success = true;
            this.message = "操作成功";
        }

        public void SetMessage()
        {
            this.SetMessage("操作成功！", "操作失败!");
        }

        public void SetMessage(string okMsg, string failMsg)
        {
            this.message = this.success ? okMsg : failMsg;
        }

        public object data { get; set; }

        [JsonIgnore]
        public System.Exception Exception { get; set; }

        public string message { get; set; }

        public bool success { get; set; }
    }
}
