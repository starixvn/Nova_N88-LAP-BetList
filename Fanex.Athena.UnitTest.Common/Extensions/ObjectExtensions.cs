namespace Fanex.Athena.UnitTest.Common.Extensions
{
    public static class ObjectExtensions
    {
#pragma warning disable S4225 // Extension methods should not extend "object"
        public static object GetValue(this object item, string property)
#pragma warning restore S4225 // Extension methods should not extend "object"
        {
            return item.GetType().GetProperty(property).GetValue(item, null);
        }
    }
}