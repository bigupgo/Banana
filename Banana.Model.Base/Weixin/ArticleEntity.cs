using Banana.Core.Base.Weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Banana.Model.Base.Weixin
{
    public class ArticleEntity
    {
        private ArticleEntity() { }
        public ArticleEntity(string Title, string Description, string PicUrl, string Url)
        {
            this.Title = XmlHelper.getXmlCDataSectionByValue(Title);
            this.Description = XmlHelper.getXmlCDataSectionByValue(Description);
            this.PicUrl = XmlHelper.getXmlCDataSectionByValue(PicUrl);
            this.Url = XmlHelper.getXmlCDataSectionByValue(Url);
        }
        /// <summary>
        /// 图文消息标题 
        /// </summary>
        private XmlCDataSection title;
        public XmlCDataSection Title
        {
            get
            {
                return this.title;
            }
            set
            {
                XmlDataDocument doc = new XmlDataDocument();
                XmlCDataSection cd = doc.CreateCDataSection(value.Value);
                this.title = cd;
            }
        }

        /// <summary>
        /// 图文消息描述 
        /// </summary>
        private XmlCDataSection description;
        public XmlCDataSection Description
        {
            get
            {
                return this.description;
            }
            set
            {
                XmlDataDocument doc = new XmlDataDocument();
                XmlCDataSection cd = doc.CreateCDataSection(value.Value);
                this.description = cd;
            }
        }


        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        private XmlCDataSection picUrl;
        public XmlCDataSection PicUrl
        {
            get
            {
                return this.picUrl;
            }
            set
            {
                XmlDataDocument doc = new XmlDataDocument();
                XmlCDataSection cd = doc.CreateCDataSection(value.Value);
                this.picUrl = cd;
            }
        }



        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        private XmlCDataSection url;
        public XmlCDataSection Url
        {
            get
            {
                return this.url;
            }
            set
            {
                XmlDataDocument doc = new XmlDataDocument();
                XmlCDataSection cd = doc.CreateCDataSection(value.Value);
                this.url = cd;
            }
        }


    }
}
