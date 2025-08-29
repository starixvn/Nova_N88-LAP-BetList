namespace Fanex.Athena.Common.Utility.Services
{
    using System.Configuration;
    using Interfaces;

    /// <summary>
    /// Setting class.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        public bool IsProduction
        {
            get { return GetValue("Environment") == "PRO"; }
        }

        public string TempFilePath => GetValue("TempFilePath");

        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}