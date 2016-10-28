using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core.Common
{
    public static class MD5Encoder
    {
        private static int Foo(int value, int pos)
        {
            if (value < 0)
            {
                string s = Convert.ToString(value, 2);    // 转换为二进制 
                for (int i = 0; i < pos; i++)
                {
                    s = "0" + s.Substring(0, 31);
                }
                return Convert.ToInt32(s, 2);  // 将二进制数字转换为数字      
            }
            else
            {
                return value >> pos;
            }
        }

        /// <summary>
        /// 获取加密后的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Compute(string str)
        {
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(str);

                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                byte[] updateBytes = md5.ComputeHash(bytes);
                int len = updateBytes.Length;
                char[] myChar = new char[len * 2];
                int k = 0;
                for (int i = 0; i < len; i++)
                {
                    byte byte0 = updateBytes[i];
                    myChar[k++] = hexDigits[Foo(byte0, 4) & 0x0f];
                    myChar[k++] = hexDigits[byte0 & 0x0f];
                }
                return new string(myChar);
            }
            catch
            {
                return null;
            }
        }

    }
}
