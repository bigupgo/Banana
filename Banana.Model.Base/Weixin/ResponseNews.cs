using Banana.Core.Base.Weixin;
using Banana.Model.Base.Weixin.Enum;
using System.Collections.Generic;

namespace Banana.Model.Base.Weixin
{
    /// <summary>
    /// 回复图文消息
    /// </summary>
    [System.Xml.Serialization.XmlRoot(ElementName = "xml")]
    public partial class ResponseNews : BaseMessage
    {
        private ResponseNews()
        {
            this.MsgType = XmlHelper.getXmlCDataSectionByValue(ResponseMsgType.News.ToString().ToLower());
            this.Articles = new List<ArticleEntity>();
        }

        public ResponseNews(string FromUserName, string ToUserName)
            : this()
        {
            this.FromUserName = XmlHelper.getXmlCDataSectionByValue(FromUserName);
            this.ToUserName = XmlHelper.getXmlCDataSectionByValue(ToUserName);
        }

        /// <summary>
        /// 图文消息个数，限制为10条以内
        /// </summary>
        public int ArticleCount
        {
            get
            {
                return this.Articles.Count;
            }
            set
            {
                //增加这个步骤才出来XML内容
            }
        }

        /// <summary>
        /// 图文列表。
        /// 多条图文消息信息，默认第一个item为大图,注意，如果图文数超过10，则将会无响应
        /// </summary>
        [System.Xml.Serialization.XmlArrayItem("item")]
        public List<ArticleEntity> Articles { get; set; }
    }
}
