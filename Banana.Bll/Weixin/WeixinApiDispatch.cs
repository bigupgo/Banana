using Banana.Core.Base;
using Banana.Core.Base.Weixin;
using Banana.Core.Common;
using Banana.DBModel;
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
                    string msgType = this.GetMsgType(xmldoc);
                    if (msgType.Equals("event"))
                    {
                        return EventHandle(xmldoc);
                    }
                    else
                    {
                        return TextHandle(xmldoc);
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 判断消息类型
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public string GetMsgType(XmlDocument xmldoc)
        {
            string msgType = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/MsgType").InnerText;
            return msgType;
        }

        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public string EventHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            SubscribeBll server = new SubscribeBll();
            string ToUserName = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/ToUserName").InnerText;
            string FromUserName = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/FromUserName").InnerText;
            string CreateTime = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/CreateTime").InnerText;
            string Event = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/Event").InnerText;
          
            Ba_Subscribe entity = new Ba_Subscribe();
            entity.FromUserName = FromUserName;
            entity.OptionDate = this.GetTime(CreateTime);
            entity.Status = Event;
            server.UpdateSub(entity);

            if (Event.Equals("subscribe"))
            {
               
                ResponseText text = new ResponseText(ToUserName, FromUserName, "回复“吃”、“吃什么”等包含“吃”文字,试试看。");
                responseContent = text.ToXml();
            }
           
            return responseContent;
        }

        public string TextHandle(XmlDocument xmldoc)
        {
            try
            {
                string responseContent = "";
                string FromUserName = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/ToUserName").InnerText;
                string ToUserName = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/FromUserName").InnerText;
                string Content = XmlHelper.getXmlNodeByXmlDocument(xmldoc, "/xml/Content").InnerText;

                if (Content.Contains("吃"))
                {
                    ////图文消息  注:发送数据的时候 ，Form变成了To   发送人变成了接收人
                    ResponseNews News = new ResponseNews(FromUserName, ToUserName);
                    News.Articles.Add(new ArticleEntity("欢迎关注【BigUpGo】", "今天吃什么？", WeixinCommon.FormatPath("/Content/wheel.png"), WeixinCommon.FormatPath("/Home/Food")));
                    responseContent = News.ToXml();
                }
                else if (Content.Contains("1"))
                {
                    ResponseNews News = new ResponseNews(FromUserName, ToUserName);
                    News.Articles.Add(new ArticleEntity("欢迎关注【BigUpGo】", "今天吃什么？", WeixinCommon.FormatPath("/Content/wheel.png"), WeixinCommon.FormatPath("/Home/Grid")));
                    responseContent = News.ToXml();
                }
                else
                {
                    ResponseText text = new ResponseText(FromUserName, ToUserName, "回复“吃”、“吃什么”等包含“吃”文字,试试看。");
                    responseContent = text.ToXml();
                }
                    
                return responseContent;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("错误信息:" + ex.Message);
                return "";
            }
        }
        /// <summary>
        /// 时间戳转化成时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }

    }


}
