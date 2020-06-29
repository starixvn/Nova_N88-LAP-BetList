namespace Fanex.Athena.Common.Constant
{
    using System.Collections.Generic;
    using Resources;

    public static class PlatformType
    {
        private static IDictionary<string, string> _platforms = new Dictionary<string, string>
        {
            { "a", "China" },
            { "z", "China" },
            { "d", "Asia" },
            { "g", "Asia" },
            { "e", "Euro" },
            { "u", "Euro" },
            { "m", "Wap" },
            { "l", "Mobile" },
            { "r", "Tablet" },
            { "x", "NewAsia" },
            { "o", "MobileApp" },
            { "c", "MobileLiteVersion" },
            { "t", "DeepLink" },
            { "0", "NativeApp" },
            { "1", "ESportsDesktop" },
            { "2", "ESportsMobile" },
            { "3", "ESportsApp" },
            { "4", "ESportsDesktop" },
            { "5", "ESportsMobile" },
            { "6", "ESportsApp" },
            { "v", "BetfromOthers" }
        };

        public static string GetPlatformName(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && _platforms.ContainsKey(code))
            {
                return ViewResources.ResourceManager.GetString(_platforms[code]);
            }

            return ViewResources.Others;
        }
    }
}