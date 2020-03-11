using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Fanex.Athena.Common.Utility.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class JsonNetModelBinderAttribute : ActionFilterAttribute
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
        };
        public Type JsonDataType { get; set; }
        public string Parameter { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.ContentType.Contains("application/json"))
            {
                filterContext.HttpContext.Request.InputStream.Position = 0;
                string inputContent;
                using (var sr = new StreamReader(filterContext.HttpContext.Request.InputStream))
                {
                    inputContent = sr.ReadToEnd();
                }

                var result = JsonConvert.DeserializeObject(inputContent, JsonDataType, _jsonSerializerSettings);
                filterContext.ActionParameters[Parameter] = result;
            }
        }
    }
}
