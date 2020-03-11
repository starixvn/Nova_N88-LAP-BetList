namespace Fanex.Athena.UnitTest.Common.Utility
{
    using System.Reflection;
    using System.Text;

    internal static class ObjectDumper
    {
        internal static string Dump(object obj, string separator = "\n")
        {
            if (obj == null)
            {
                return "null";
            }

            var t = obj.GetType();
            var result = new StringBuilder();
            foreach (PropertyInfo item in t.GetProperties())
            {
                var value = t.GetProperty(item.Name).GetValue(obj, null);
                result.AppendFormat("{0}:{1}{2}", item.Name, value, separator);
            }

            return result.ToString();
        }
    }
}