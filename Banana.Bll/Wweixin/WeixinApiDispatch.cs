using Banana.Core.Base;
using Banana.Core.Base.Weixin;
using Banana.Core.Common;
using Banana.Model.Base.Weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Banana.Bll.Weixin
{
    public class WeixinApiDispatch
    {
        public string Execute(string PostStr)
        {
            if (!string.IsNullOrEmpty(PostStr))
            {
                XmlDocument xmldoc = XmlHelper.StrToXmlDocument(PostStr);
                if (xmldoc != null)
                {
                    return TextHandle(xmldoc);
                }
            }
            return "";
        }


        public string TextHandle(XmlDocument xmldoc)
        {
            try
            {
                string responseContent = "";
                string FromUserName = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/ToUserName").InnerText;
                string ToUserName = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/FromUserName").InnerText;
                string Content = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/Content").InnerText;

                ////图文消息  注:发送数据的时候 ，Form变成了To   发送人变成了接收人
                ResponseNews News = new ResponseNews(FromUserName, ToUserName);
                News.Articles.Add(new ArticleEntity("欢迎光临马祥龙的小站!", " 0o^-^o0 今天要开心愉快哦~ 0o^-^o0 ", WeixinCommon.FormatPath("/supin/Content/Mobile/Images/1.jpg"), ""));
                responseContent = News.ToXml();

                //ResponseText text = new ResponseText(ToUserName, FromUserName, "Hello,我是马祥龙！");
                //responseContent = text.ToXml();


                return responseContent;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("错误信息:" + ex.Message);
                return "";
            }
        }
    }
}
