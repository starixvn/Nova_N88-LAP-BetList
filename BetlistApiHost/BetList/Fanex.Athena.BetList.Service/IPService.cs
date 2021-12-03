using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Fanex.Athena.BetList.Service
{
    public class IPService : IIPService
    {
        private static double IP32Bits = 16_777_216.0;
        private static double IPMaxRange = 65536.0;
        private static double IPMaxBytes = 256.0;
        private static int IpIndex2 = 2;
        private static int IpIndex3 = 3;

        private readonly Lazy<Tuple<IList<IPV4RangeEntity>, IList<IPV6RangeEntity>>> VNIPRanges;

        public IPService(IIPDataAccess ipDataAccess)
        {
            VNIPRanges = new Lazy<Tuple<IList<IPV4RangeEntity>, IList<IPV6RangeEntity>>>(() => ipDataAccess.GetVNIPs());
        }

        public bool IsVNIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }
            var ipVnRange = VNIPRanges.Value;

            var isIpv6 = IsIPV6(ipAddress);

            if (isIpv6)
            {
                if (ipVnRange.Item2 == null)
                {
                    return false;
                }

                var ipv6Number = ConvertIpV6ToNumber(ipAddress);
                return ipVnRange.Item2.Any(ip => ip.FromNumber <= ipv6Number && ipv6Number <= ip.ToNumber);
            }

            if (ipVnRange.Item1 == null)
            {
                return false;
            }

            var ipv4Number = ConvertIpV4ToNumber(ipAddress);
            return ipVnRange.Item1.Any(ip => ip.IPFrom <= ipv4Number && ipv4Number <= ip.IPTo);
        }

        private static bool IsIPV6(string clientIP)
            => IPAddress.TryParse(clientIP, out IPAddress address)
                && address.AddressFamily == AddressFamily.InterNetworkV6;

        public static double ConvertIpV4ToNumber(string ip)
        {
            double num = 0.0;
            if (IsValidIpV4(ip))
            {
                string[] strArray = ip.Split(new[] { '.' });
                num = double.Parse(strArray[0]) * IP32Bits + double.Parse(strArray[1]) * IPMaxRange + double.Parse(strArray[IpIndex2]) * IPMaxBytes + double.Parse(strArray[IpIndex3]);
            }

            return num;
        }

        public static BigInteger ConvertIpV6ToNumber(string ipv6)
        {
            BigInteger bigInteger = 0;

            if (IPAddress.TryParse(ipv6, out IPAddress address))
            {
                byte[] numArray = address.GetAddressBytes();
                if (BitConverter.IsLittleEndian)
                {
                    List<byte> byteList = new List<byte>(numArray);
                    byteList.Reverse();
                    numArray = byteList.ToArray();
                }
#pragma warning disable S109 // Magic numbers should not be used
                bigInteger = numArray.Length <= 8 ? BitConverter.ToUInt32(numArray, 0) : ((BigInteger)BitConverter.ToUInt64(numArray, 8) << 64) + BitConverter.ToUInt64(numArray, 0);
#pragma warning restore S109 // Magic numbers should not be used
            }

            return bigInteger;
        }

        public static bool IsValidIpV4(string ip)
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