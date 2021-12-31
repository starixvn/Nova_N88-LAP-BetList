namespace Fanex.Athena.Common.Constant
{
    using System.Collections.Generic;
    using Resources;

    public static class PlatformType
    {
        private const string DesktopNewAsia = "DesktopNewAsia";
        private const string DesktopChina = "DesktopChina";
        private const string DesktopOthers = "DesktopOthers";
        private const string MobileStandard = "MobileStandard";
        private const string MobileEuro = "MobileEuro";
        private const string MobileCompact = "MobileCompact";
        private const string MobileOthers = "MobileOthers";
        private const string ESportsDesktop = "ESportsDesktop";
        private const string ESportsMobile = "ESportsMobile";
        private const string ESportsApp = "ESportsApp";
        private const string MobileApp = "MobileApp";
        private const string NativeApp = "NativeApp";
        private const string Tablet = "Tablet";
        private const string DeepLink = "DeepLink";
        private const string Wap = "Wap";
        private const string BetfromOthers = "BetfromOthers";

        private static IDictionary<string, string> _platforms = new Dictionary<string, string>
        {
            { "x", DesktopNewAsia },
            { "a", DesktopChina },
            { "z", DesktopChina },
            { "[", DesktopChina },
            { "9", DesktopOthers },
            { ">", DesktopOthers },
            { "l", MobileStandard },
            { "]", MobileStandard },
            { "%", MobileEuro },
            { "c", MobileCompact },
            { "@", MobileCompact },
            { "7", MobileOthers },
            { "8", MobileOthers },
            { "_", MobileOthers },
            { "$", MobileOthers },
            { ".", MobileOthers },
            { "1", ESportsDesktop },
            { "4", ESportsDesktop },
            { "2", ESportsMobile },
            { "5", ESportsMobile },
            { "3", ESportsApp },
            { "6", ESportsApp },
            { "o", MobileApp },
            { "~", MobileApp },
            { "0", NativeApp },
            { "r", Tablet },
            { "t", DeepLink },
            { "m", Wap },
            { "v", BetfromOthers },
            { "d", BetfromOthers },
            { "g", BetfromOthers },
            { "e", BetfromOthers },
            { "u", BetfromOthers },
            { "^", BetfromOthers }
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