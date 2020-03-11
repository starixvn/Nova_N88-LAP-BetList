namespace Fanex.Athena.Common.Utility.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IAppSettings
    {
        bool IsProduction { get; }

        IEnumerable<int> FakeLicenseeList { get; }

        string TempFilePath { get; }

        string GetValue(string key);
    }
}