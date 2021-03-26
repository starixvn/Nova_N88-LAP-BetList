using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Service.Constants;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fanex.Athena.BetList.Service
{
    public class IPService : IIPService
    {
        private const int CacheTimeout = 3600;
        private readonly IIPDataAccess _ipDataAccess;
        private readonly IMemoryCacheService _cache;

        private static double IP32Bits = 16777216.0;
        private static double IPMaxRange = 65536.0;
        private static double IPMaxBytes = 256.0;
        private static int IpIndex2 = 2;
        private static int IpIndex3 = 3;

        public IPService(IIPDataAccess ipDataAccess, IMemoryCacheService cache)
        {
            _ipDataAccess = ipDataAccess;
            _cache = cache;
        }

        public IList<IPRangeEntity> GetIPVN()
        {
            List<IPRangeEntity> ipsFromCaching = _cache.Get<List<IPRangeEntity>>(SessionKey.VNIPList);
            if (ipsFromCaching != null && ipsFromCaching.Count > 0)
            {
                return ipsFromCaching;
            }

            var ipsFromDB = _ipDataAccess.GetVNIPs();

            _cache.Set(SessionKey.VNIPList, ipsFromDB, CacheTimeout);

            return ipsFromDB;
        }

        public bool IsVNIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }

            double ipNumber = ConvertIpToNumber(ipAddress);

            List<IPRangeEntity> ips = GetIPVN().ToList();
            if (ips == null)
            {
                return false;
            }

            var ipRange = from q in ips where q.IPFrom <= ipNumber && q.IPTo >= ipNumber select q;

            if (ipRange.Any())
            {
                return true;
            }

            return false;
        }

        public static double ConvertIpToNumber(string ip)
        {
            double num = 0.0;
            if (IsValidIp(ip))
            {
                string[] strArray = ip.Split(new[] { '.' });
                num = double.Parse(strArray[0]) * IP32Bits + double.Parse(strArray[1]) * IPMaxRange + double.Parse(strArray[IpIndex2]) * IPMaxBytes + double.Parse(strArray[IpIndex3]);
            }

            return num;
        }

        public static bool IsValidIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }

            const string regexCharacters =
                @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)";
            Regex regex = new Regex(regexCharacters);

            return regex.IsMatch(ip, 0);
        }
    }
}
