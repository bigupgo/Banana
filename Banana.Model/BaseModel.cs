using System;

namespace Banana.Model
{
    public class BaseModel
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
        private bool _isDel = false;
        public bool IsDel {
            get { return this._isDel; }
            set { this._isDel = value; }
        }

    }
}
