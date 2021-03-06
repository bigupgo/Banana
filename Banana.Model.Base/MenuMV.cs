﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Model.Base
{
    public class MenuMV 
    {
        public string ID { get; set; }
        public string ModelName { get; set; }
        public int? ModelLevel { get; set; }
        public int? Type { get; set; }
        public string iconCls { get; set; }
        public string Url { get; set; }
        public string Pid { get; set; }
        public int? Sort { get; set; }
        public bool? Enable { get; set; }
        public DateTime? CreateTime { get; set; }
        public List<MenuMV> children { get; set; }
    }

    public class Menutree
    {
        public string id { get; set; }
        public string text { get; set; }
        public string state { get; set; }
        public TreeAttribute attributes { get; set; }
        public List<Menutree> children { get; set; }
    }

    public class TreeAttribute
    {
        public int? ModelLevel { get; set; }
        public int? Type { get; set; }
        public string Pid { get; set; }
    }
}
