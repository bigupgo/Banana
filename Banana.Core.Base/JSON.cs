using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;

namespace Banana.Core.Base
{
    public class JSON
    {
        public static T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string Serialize(object obj)
        {
            return Serialize(obj, true);
        }

        public static string Serialize(object obj, bool ignoreComPropers)
        {
            if (ignoreComPropers)
            {
                List<string> excludedProperties = new List<string> { "EntityKey", "EntityState", "$id" };
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.ContractResolver = new ExcludePropertiesContractResolver(excludedProperties);
                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
            }
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static string Serialize(object obj, params string[] ignores)
        {
            List<string> excludedProperties = new List<string>();
            if ((obj is EntityObject) || (obj is IEnumerable))
            {
                excludedProperties.Add("EntityKey");
                excludedProperties.Add("EntityState");
                excludedProperties.Add("$id");
            }
            else
            {
                if (ignores.Length == 0)
                {
                    return Serialize(obj);
                }
                excludedProperties.AddRange(ignores);
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.ContractResolver = new ExcludePropertiesContractResolver(excludedProperties);
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }
    }
}
