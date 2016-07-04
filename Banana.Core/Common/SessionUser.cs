using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Banana.Core.Common
{
    public class SessionUser
    {
        private readonly string SUPERADMIN = BaseConfig.GetValue("SuperAdmin");

        public SessionUser()
        {
            if (string.IsNullOrEmpty(this.SUPERADMIN))
            {
                this.SUPERADMIN = "admin";
            }
        }

        public string DisplayName { get; set; }

        public string ID { get; set; }

        public bool IsSuperAdmin
        {
            get
            {
                return (bool)(this.LoginName == this.SUPERADMIN);
            }
        }

        public string LoginName { get; set; }

      
       

     

     
    }
}
