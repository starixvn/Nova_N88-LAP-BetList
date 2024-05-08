namespace Fanex.Athena.Common.Utility.Helpers
{
    public static class SqlHelper
    {
        public static string ToSqlTextSearch(string text)
            => string.IsNullOrEmpty(text) ? text : text.Trim().Replace("_", "[_]");
    }
}