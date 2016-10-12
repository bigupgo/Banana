using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core.Base
{
    public class CommonFields
    {
         public CommonFields()
        {
            this.CreateTime = "CreateTime";
            this.TimeStamp = "TimeStamp";
            this.IsDel = "IsDel";
            this.EntityKey = "EntityKey";
            this.EntityState = "EntityState";
        }
        
        public CommonFields(string createTime, string timeStamp, string isDel) : this(createTime, timeStamp, isDel, "EntityKey", "EntityState")
        {
        }
        
        public CommonFields(string createTime, string timeStamp, string isDel, string entityKey, string entityState)
        {
            this.CreateTime = createTime;
            this.TimeStamp = timeStamp;          
            this.IsDel = isDel;
            this.EntityKey = entityKey;
            this.EntityState = entityState;
        }
        
        public string[] GetFields()
        {
            Type type = typeof(CommonFields);
            string[] strArray = (from a in type.GetProperties() select a.Name).ToArray<string>();
            List<string> list = new List<string>();
            string[] strArray2 = strArray;
            for (int i = 0; i < strArray2.Length; i = (int) (i + 1))
            {
                string str = strArray2[i];
                list.Add(type.GetProperty(str).GetValue(this, (object[]) null).ToString());
            }
            return list.ToArray();
        }
        
        public string CreateTime { get; set; }
          
        public string EntityKey { get; set; }
        
        public string EntityState { get; set; }
        
        public string IsDel { get; set; }
                
        public string TimeStamp { get; set; }
        

    }
}
