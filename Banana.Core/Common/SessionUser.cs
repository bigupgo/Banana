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

        public string LoginName { get; set; }

        public string Id { get; set; }

        public string RealName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string UserIcon { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsSuperAdmin
        {
            get
            {
                return (bool)(this.LoginName == this.SUPERADMIN);
            }
        }

      

      
       

     

     
    }
}
