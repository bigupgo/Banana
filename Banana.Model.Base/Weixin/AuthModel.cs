using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Model.Base.Weixin
{
    public class AuthModel
    {
        /// <summary>
        /// 微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。 
        /// </summary>
        public string signature { set; get; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { set; get; }

        /// <summary>
        /// 随机数 
        /// </summary>
        public string nonce { set; get; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string echostr { set; get; }

        /// <summary>
        /// 加密字符串
        /// </summary>
        private string weixintoken;

        public string Weixintoken
        {
            get { return weixintoken; }
            set { weixintoken = value; }
        }


        public string AppID { set; get; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { set; get; }
    }
}
