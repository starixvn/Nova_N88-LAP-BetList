namespace Fanex.Athena.BetList.DataAccess.Util
{
    using System;

    public interface ISafeDataReader
    {
        bool GetBool(string column, bool defaultValue = false);

        DateTime GetDateTime(string column, DateTime defaultValue);

        decimal GetDecimal(string column, decimal defaultValue = 0);

        int GetInt(string column, int defaultValue = 0);

        long GetLong(string column, long defaultValue = 0);

        long? GetNullableLong(string column, long? defaultValue = default(long?));

        string GetString(string column, string defaultValue = "");

        object GetValue(string column, object defaultValue = null);

        bool Read();

        byte GetByte(string column, byte defaultValue = 0);
    }
}