using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Banana.Core.Common
{
    public class UploadHelper
    {
        public static bool UploadFile(string filePath, HttpPostedFileBase file)
        {
            bool result = true;
            try
            {
                string path = filePath.Substring(0, filePath.LastIndexOf("\\"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
           
                file.SaveAs(filePath);
            }
            catch (Exception e)
            {
                result = false;
                LogHelper.LogError(e.Message);
            }

            return result;
        }
    }
}
