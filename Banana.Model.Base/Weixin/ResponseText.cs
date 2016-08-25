using Banana.Core.Base.Weixin;
using Banana.Model.Base.Weixin.Enum;
using System.Xml;

namespace Banana.Model.Base.Weixin
{
    //回复文本消息
    [System.Xml.Serialization.XmlRoot(ElementName = "xml")]
    public class ResponseText:BaseMessage
    {
        private ResponseText()
        {
            this.MsgType = XmlHelper.getXmlCDataSectionByValue(ResponseMsgType.Text.ToString().ToLower());
        }

        public ResponseText(string FromUserName, string ToUserName, string Content)
            : this()
        {
            this.FromUserName = XmlHelper.getXmlCDataSectionByValue(FromUserName);
            this.ToUserName = XmlHelper.getXmlCDataSectionByValue(ToUserName);
            this.Content = XmlHelper.getXmlCDataSectionByValue(Content);
        }

        /// <summary>
        /// 内容
        /// </summary>
        private XmlCDataSection content;
        public XmlCDataSection Content
        {
            get
            {
                return this.content;
            }
            set
            {
                XmlDataDocument doc = new XmlDataDocument();
                XmlCDataSection cd = doc.CreateCDataSection(value.Value);
                this.content = cd;
            }
        }
    }
}
