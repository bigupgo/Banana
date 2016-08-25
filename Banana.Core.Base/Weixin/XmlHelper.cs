using Banana.Core.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Banana.Core.Base.Weixin
{
    public class XmlHelper
    {
        /// <summary>
        /// 把模型xml
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ObjectToXml(object Model)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(Model.GetType());
            StringWriter sw = new StringWriter();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            xmlSerializer.Serialize(sw, Model, ns);
            return sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
        }

        public static T XmlToObject<T>(string xml)
        {
            using (StringReader reader = new StringReader(xml))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                var result = xmlSerializer.Deserialize(reader);
                return (T)result;
            }
        }


        /// <summary>
        /// 根据xml字符串返回XMLDocument对象
        /// </summary>
        /// <param name="XmlStr"></param>
        /// <returns></returns>
        public static XmlDocument StrToXmlDocument(string XmlStr)
        {
            if (!string.IsNullOrEmpty(XmlStr))
            {
                XmlDocument requestDocXml = new XmlDocument();

                requestDocXml.LoadXml(XmlStr);

                return requestDocXml;
            }
            else
            {
               LogHelper.LogError( "转换xml代码时出错！");
               return null;
            }
        }


        /// <summary>
        /// 根据XmlDocument和文档的xPath[如：/xml/aaa]
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <param name="xPath">xPath</param>
        /// <returns></returns>
        public static XmlNode getXmlNodeByXmlDocument(XmlDocument doc, string xPath)
        {
            XmlNode node = doc.SelectSingleNode(xPath);
            return node;
        }


        /// <summary>
        /// 根据值，返回CDATA
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlCDataSection getXmlCDataSectionByValue(string value)
        {
            XmlDataDocument doc = new XmlDataDocument();
            XmlCDataSection cd = doc.CreateCDataSection(value);
            return cd;
        }
    }
}
