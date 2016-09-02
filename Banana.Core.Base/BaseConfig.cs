using System;
using System.Collections.Generic;
using System.Xml;

namespace Banana.Core.Base
{
    public class BaseConfig
    {
        private static Dictionary<string, string> DicConfig;
        private static object thisLock = new object();

        public static string GetValue(string key)
        {
            lock (thisLock)
            {
                return DicConfig[key];
            }
        }
        public static void Init()
        {
            XmlTextReader reader = new XmlTextReader(AppDomain.CurrentDomain.BaseDirectory + "App_Data/config/base.config");
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            reader.Close();
            XmlNodeList childNodes = document.SelectSingleNode("/appSettings").ChildNodes;
            DicConfig = new Dictionary<string, string>();
            foreach (XmlNode node in childNodes)
            {
                if (node.Attributes != null)
                {
                    DicConfig.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }
            }
        }



        public static void SetWxValue(string key, string value)
        {
            string url = AppDomain.CurrentDomain.BaseDirectory + "App_Data/config/wxconfig.xml";
            XmlTextReader reader = new XmlTextReader(url);
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            reader.Close();

            XmlNode root = document.DocumentElement;
            XmlNode node = root.SelectSingleNode(key);
            node.InnerText = value;
            document.Save(url);
        }

        public static string GetWxValue(string key)
        {
            string url = AppDomain.CurrentDomain.BaseDirectory + "App_Data/config/wxconfig.xml";
            XmlTextReader reader = new XmlTextReader(url);
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            reader.Close();

            XmlNode root = document.DocumentElement;
            XmlNode node = root.SelectSingleNode(key);
            return node.InnerText;
        }

    }
}
