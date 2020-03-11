namespace Fanex.Athena.BetList.DataAccess.Util
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Common.DataAccess.Extensions;
    using Fanex.Common.Extensions;

    public class SafeDataReader : ISafeDataReader
    {
        private readonly IDataReader _reader;

        private readonly HashSet<string> _existingColumns;

        public SafeDataReader(IDataReader reader, IEnumerable<string> columns)
        {
            _reader = reader;
            _existingColumns = new HashSet<string>();

            foreach (var item in columns)
            {
                if (reader.HasColumn(item))
                {
                    _existingColumns.Add(item);
                }
            }
        }

        public bool Read()
        {
            return _reader.Read();
        }

        public bool GetBool(string column, bool defaultValue = false)
        {
            var value = GetValue(column, defaultValue);

            if (value == null || value == Convert.DBNull)
            {
                return false;
            }

            if (value is bool)
            {
                return Convert.ToBoolean(value);
            }

            if (value is int)
            {
                return Convert.ToInt32(value) > 0;
            }

            return false;
        }

        public object GetValue(string column, object defaultValue = null)
        {
            if (_existingColumns.Contains(column))
            {
                return _reader[column];
            }

            return defaultValue;
        }

        public string GetString(string column, string defaultValue = "")
        {
            if (_existingColumns.Contains(column))
            {
                return _reader.ToString(column);
            }

            return defaultValue;
        }

        public int GetInt(string column, int defaultValue = 0)
        {
            if (_existingColumns.Contains(column))
            {
                return _reader.ToInt(column);
            }

            return defaultValue;
        }

        public long GetLong(string column, long defaultValue = 0)
        {
            if (_existingColumns.Contains(column))
            {
                return _reader.ToLong(column);
            }

            return defaultValue;
        }

        public long? GetNullableLong(string column, long? defaultValue = null)
        {
            if (_existingColumns.Contains(column))
            {
                var value = _reader[column];
                if (value != DBNull.Value)
                {
                    return (long)value;
                }
            }

            return defaultValue;
        }

        public decimal GetDecimal(string column, decimal defaultValue = 0)
        {
            if (_existingColumns.Contains(column))
            {
                return _reader.ToDecimal(column);
            }

            return defaultValue;
        }

        public DateTime GetDateTime(string column, DateTime defaultValue)
        {
            if (_existingColumns.Contains(column))
            {
                return _reader.ToDateTime(column);
            }

            return defaultValue;
        }

        public byte GetByte(string column, byte defaultValue = 0)
        {
            if (_existingColumns.Contains(column))
            {
                return _reader.ToByte(column);
            }

            return defaultValue;
        }
    }
}