using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core.Common
{
    public static class Util
    {
        public static string ComputeMD5(string src)
        {
            byte[] byteArray = GetByteArray(src);
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(byteArray)).Replace("-", "");
        }

        public static string ComputeMD5(string str, bool isupper)
        {
            string str2 = ComputeMD5(str);
            if (isupper)
            {
                return str2.ToUpper();
            }
            return str2.ToLower();
        }

        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime time2 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            TimeSpan span = (TimeSpan)(time - time2);
            return (int)((int)span.TotalSeconds);
        }

        public static DateTime ConvertIntDateTime(double d)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1)).AddMilliseconds(d);
        }

        public static string ConvertWeekDayToCn(DayOfWeek dayfweek)
        {
            switch (dayfweek)
            {
                case DayOfWeek.Sunday:
                    return "星期日";

                case DayOfWeek.Monday:
                    return "星期一";

                case DayOfWeek.Tuesday:
                    return "星期二";

                case DayOfWeek.Wednesday:
                    return "星期三";

                case DayOfWeek.Thursday:
                    return "星期四";

                case DayOfWeek.Friday:
                    return "星期五";

                case DayOfWeek.Saturday:
                    return "星期六";
            }
            return "";
        }

        public static string DecryptAES(string str, string aeskey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(aeskey);
            byte[] buffer2 = Convert.FromBase64String(str);
            RijndaelManaged managed = new RijndaelManaged();
            managed.Key=bytes;
            managed.Mode = CipherMode.ECB;
            managed.Padding = PaddingMode.PKCS7;
            byte[] buffer3 = managed.CreateDecryptor().TransformFinalBlock(buffer2, 0, (int)buffer2.Length);
            return Encoding.UTF8.GetString(buffer3);
        }

        public static string DecryptDES(string decryptString, string key)
        {
            try
            {
                byte[] buffer = new byte[] { 0x12, 0x34, 0x56, 120, 0x90, 0xab, 0xcd, 0xef };
                byte[] bytes = Encoding.UTF8.GetBytes(key);
                byte[] buffer3 = buffer;
                byte[] buffer4 = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, buffer3), CryptoStreamMode.Write);
                stream2.Write(buffer4, 0, (int)buffer4.Length);
                stream2.FlushFinalBlock();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        public static string EncryptAES(string str, string aeskey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(aeskey);
            byte[] buffer2 = Encoding.UTF8.GetBytes(str);
            RijndaelManaged managed = new RijndaelManaged();
            managed.Key = bytes;
            managed.Mode = CipherMode.ECB;
            managed.Padding = PaddingMode.PKCS7;
            byte[] buffer3 = managed.CreateEncryptor().TransformFinalBlock(buffer2, 0, (int)buffer2.Length);
            return Convert.ToBase64String(buffer3, 0, (int)buffer3.Length);
        }

        public static string EncryptDES(string encryptString, string key)
        {
            string str;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(key);
                byte[] buffer2 = new byte[] { 0x12, 0x34, 0x56, 120, 0x90, 0xab, 0xcd, 0xef };
                byte[] buffer3 = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                using (MemoryStream stream = new MemoryStream())
                {
                    CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, buffer2), CryptoStreamMode.Write);
                    stream2.Write(buffer3, 0, (int)buffer3.Length);
                    stream2.FlushFinalBlock();
                    str = Convert.ToBase64String(stream.ToArray());
                }
            }
            catch
            {
                str = encryptString;
            }
            return str;
        }

        public static bool EqualByteArray(byte[] arr1, byte[] arr2)
        {
            if ((arr1 == null) || (arr2 == null))
            {
                return true;
            }
            if (arr1.Length != arr2.Length)
            {
                return false;
            }
            for (int i = (int)(arr1.Length - 1); i > -1; i = (int)(i - 1))
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetAppSettingValue(string key)
        {
            return ConfigurationManager.AppSettings.GetValues(key).ToString();
        }

        private static byte[] GetByteArray(string src)
        {
            byte[] buffer = new byte[src.Length];
            for (int i = 0; i < src.Length; i = (int)(i + 1))
            {
                buffer[i] = Convert.ToByte(src[i]);
            }
            return buffer;
        }

        public static string GetContentTypeByExtions(string fileExt)
        {
            switch (fileExt.ToLower())
            {
                case "jpeg":
                case "jpg":
                case "jpe":
                    return "image/jpeg";

                case "png":
                    return "image/png";

                case "gif":
                    return "image/gif";

                case "doc":
                case "docx":
                    return "application/msword";

                case "xls":
                case "xlsx":
                    return "application/x-excel";

                case "ppt":
                case "pptx":
                    return "application/ms-powerpoint";

                case "pdf":
                    return "application/pdf";

                case "zip":
                    return "application/zip";
            }
            return ("application/" + fileExt);
        }

        public static string GetGUID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static DateTime GetTime(string timeStamp)
        {
            DateTime time = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            long num = long.Parse(timeStamp + "0000000");
            TimeSpan span = new TimeSpan(num);
            return time.Add(span);
        }

        public static string MD5Decrypt(string pToDecrypt, string sKey)
        {
            if (string.IsNullOrEmpty(pToDecrypt))
            {
                return "";
            }
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] buffer = new byte[pToDecrypt.Length / 2];
            for (int i = 0; i < (pToDecrypt.Length / 2); i = (int)(i + 1))
            {
                int num2 = Convert.ToInt32(pToDecrypt.Substring((int)(i * 2), 2), 0x10);
                buffer[i] = (byte)((byte)num2);
            }
            provider.Key=Encoding.ASCII.GetBytes(sKey);
            provider.IV=Encoding.ASCII.GetBytes(sKey);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, (int)buffer.Length);
            stream2.FlushFinalBlock();
            new StringBuilder();
            return Encoding.Default.GetString(stream.ToArray());
        }

        public static string MD5Encrypt(string pToEncrypt, string sKey)
        {
            if (string.IsNullOrEmpty(pToEncrypt))
            {
                return "";
            }
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(pToEncrypt);
            provider.Key=Encoding.ASCII.GetBytes(sKey);
            provider.IV=Encoding.ASCII.GetBytes(sKey);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, (int)bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            byte[] buffer2 = stream.ToArray();
            for (int i = 0; i < buffer2.Length; i = (int)(i + 1))
            {
                byte num = buffer2[i];
                builder.AppendFormat("{0:X2}", (byte)num);
            }
            builder.ToString();
            return builder.ToString();
        }

        public static string SHA1(string Source_String)
        {
            byte[] bytes = Encoding.Default.GetBytes(Source_String);
            bytes = new SHA1CryptoServiceProvider().ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            byte[] buffer2 = bytes;
            for (int i = 0; i < buffer2.Length; i = (int)(i + 1))
            {
                byte num = buffer2[i];
                builder.AppendFormat("{0:x2}", (byte)num);
            }
            return builder.ToString();
        }

        public static string ToBase64(string str)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(str));
        }

        public static string UnBase64(string base64str)
        {
            byte[] buffer = Convert.FromBase64String(base64str);
            return Encoding.ASCII.GetString(buffer);
        }
    }
}
