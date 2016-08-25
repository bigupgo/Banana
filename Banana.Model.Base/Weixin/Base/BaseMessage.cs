using System;
using System.Xml;
using System.Xml.Serialization;
using Banana.Core.Base.Weixin;

namespace Banana.Model.Base.Weixin
{
    /// <summary>
    /// 基础消息内容
    /// </summary>
    [XmlRoot(ElementName = "xml")]
    public class BaseMessage
    {
        /// <summary>
        /// 初始化一些内容，如创建时间为整形，
        /// </summary>
        public BaseMessage()
        {
            this.CreateTime = DateTime.Now.DateTimeToInt();
        }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        private XmlCDataSection toUserName;
        public XmlCDataSection ToUserName
        {
            get
            {
                return this.toUserName;
            }
            set
            {
                XmlDataDocument doc = new XmlDataDocument();
                XmlCDataSection cd = doc.CreateCDataSection(value.Value);
                this.toUserName = cd;
            }
        }


        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        private XmlCDataSection fromUserName;
        public XmlCDataSection FromUserName
        {
            get
            {
                return this.fromUserName;
            }
            set
            {
                XmlDataDocument doc = new XmlDataDocument();
                XmlCDataSection cd = doc.CreateCDataSection(value.Value);
                this.fromUserName = cd;
            }
        }



        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public long CreateTime { get; set; }



        /// <summary>
        /// 消息类型
        /// </summary>
        private XmlCDataSection msgType;
        public XmlCDataSection MsgType
        {
            get
            {
                return this.msgType;
            }
            set
            {
                XmlDataDocument doc = new XmlDataDocument();
                XmlCDataSection cd = doc.CreateCDataSection(value.Value);
                this.msgType = cd;
            }
        }


        public virtual string ToXml()
        {
            this.CreateTime = DateTime.Now.DateTimeToInt();
            return XmlHelper.ObjectToXml(this);
        }
    }
}
