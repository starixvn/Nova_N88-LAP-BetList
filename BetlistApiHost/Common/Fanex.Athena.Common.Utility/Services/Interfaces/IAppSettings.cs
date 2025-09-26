namespace Fanex.Athena.Common.Utility.Services.Interfaces
{


    public interface IAppSettings
    {
        bool IsProduction { get; }

        string TempFilePath { get; }

        string GetValue(string key);
    }
}