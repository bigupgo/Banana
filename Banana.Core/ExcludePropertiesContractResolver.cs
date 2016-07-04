﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core
{
    public class ExcludePropertiesContractResolver: DefaultContractResolver
    {
        private IEnumerable<string> lstExclude;

        public ExcludePropertiesContractResolver(IEnumerable<string> excludedProperties)
        {
            this.lstExclude = excludedProperties;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).ToList<JsonProperty>().FindAll(p => !Enumerable.Contains<string>(this.lstExclude, p.PropertyName));
        }
    }
}
