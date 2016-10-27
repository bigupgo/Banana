
using Banana.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Model
{
    public class UserInfo : BaseModel
    {
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserIcon { get; set; }
    }
}
