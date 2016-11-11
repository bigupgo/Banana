using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core.Base
{
    public class BoolFlag
    {
        public const string FALSE = "N";
        public const string TRUE = "Y";

        public static bool ToBool(string str)
        {
            switch (str)
            {
                case "Y":
                    return true;

                case "N":
                    return false;
            }
            throw new Exception("字符必须是Y或者N，同时注意大小写");
        }

        public static string ToBoolString(bool b)
        {
            return (b ? ((string)"Y") : ((string)"N"));
        }
    }
}
