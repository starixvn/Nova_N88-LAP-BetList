namespace Fanex.Athena.UnitTest.Common.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using NSubstitute;
    using NSubstitute.Core;

    public static class DataAccessTestHelper
    {
        public static void MapReaderData(IDataReader reader, object readerDataMapping)
        {
            var func = CreateMappingFunc(readerDataMapping);
            reader[Arg.Any<string>()].Returns(func);
        }

        private static Func<CallInfo, object> CreateMappingFunc(object mappingData)
        {
            var dictionary = ToDictionary(mappingData);
            return x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (dictionary.ContainsKey(paramName))
                {
                    return dictionary[paramName];
                }

                return string.Empty;
            };
        }

        private static IDictionary<string, object> ToDictionary(object data)
        {
            if (data is IDictionary)
            {
                return (IDictionary<string, object>)data;
            }

            var type = data.GetType();
            var properties = type.GetProperties();
            return properties.ToDictionary(x => x.Name, x => x.GetValue(data, null), StringComparer.OrdinalIgnoreCase);
        }
    }
}