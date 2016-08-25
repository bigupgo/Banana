using System;

namespace Banana.Model.Base
{
    public class BaseModel
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDel { get; set; }
    }
}
