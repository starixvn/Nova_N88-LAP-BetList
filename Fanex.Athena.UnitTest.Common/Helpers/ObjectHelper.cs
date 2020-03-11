using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Fanex.Athena.UnitTest.Common.Helpers
{
    public static class ObjectHelper
    {
        public static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static IDictionary<string, object> ToDictionary(object value)
        {
            return value.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(value, null));
        }
    }
}