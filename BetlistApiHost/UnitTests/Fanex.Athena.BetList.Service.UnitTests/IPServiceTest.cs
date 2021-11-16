using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.BetList.Service.UnitTests
{
    [TestFixture]
    public class IPServiceTest
    {
        private IIPDataAccess _ipDataAccess;
        private IMemoryCacheService _cache;
        private IIPService _ipService;

        [SetUp]
        public void Setup()
        {
            _ipDataAccess = Substitute.For<IIPDataAccess>();
            _cache = Substitute.For<IMemoryCacheService>();
            _ipService = new IPService(_ipDataAccess, _cache);
        }

        [TearDown]
        public void TearDown()
        {
            _ipDataAccess = null;
            _cache = null;
        }

        [Test]
        public void GetIPVN_WhenCall_ReturnListIPVN()
        {
            // Arrange
            var expectedResult = Tuple.Create<IList<IPV4RangeEntity>, IList<IPV6RangeEntity>>(new List<IPV4RangeEntity>() { new IPV4RangeEntity { IPFrom = 1234, IPTo = 5678 } },
                new List<IPV6RangeEntity>() { new IPV6RangeEntity { IPFrom = "aaaa", IPTo = "ffff" } });
            _ipDataAccess.GetVNIPs().Returns(expectedResult);

            // Action
            var result = _ipService.GetIPVN();

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsVNIP_WhenCall_IPNullOrEmpty_ReturnFalse()
        {
            // Arrange
            var ip = string.Empty;

            // Action
            var result = _ipService.IsVNIP(ip);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsVNIP_WhenCall_ListIPVNIsEmpty_ReturnFalse()
        {
            // Arrange
            var ip = "1.1.1.1";
            var expectedResult = Tuple.Create<IList<IPV4RangeEntity>, IList<IPV6RangeEntity>>(new List<IPV4RangeEntity>(), new List<IPV6RangeEntity>());
            _ipDataAccess.GetVNIPs().Returns(expectedResult);

            // Action
            var result = _ipService.IsVNIP(ip);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsVNIP_WhenCall_IpIsNotInRangeIP_ReturnFalse()
        {
            // Arrange
            var ip = "1.1.1.1";
            var expectedResult = Tuple.Create<IList<IPV4RangeEntity>, IList<IPV6RangeEntity>>(new List<IPV4RangeEntity>() { new IPV4RangeEntity { IPFrom = 1234, IPTo = 5678 } },
                new List<IPV6RangeEntity>() { new IPV6RangeEntity { IPFrom = "aaaa", IPTo = "ffff" } });
            _ipDataAccess.GetVNIPs().Returns(expectedResult);

            // Action
            var result = _ipService.IsVNIP(ip);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void IsVNIP_WhenCall_IsIPVN_ReturnTrue()
        {
            // Arrange
            var ip = "1.1.1.1";
            var expectedResult = Tuple.Create<IList<IPV4RangeEntity>, IList<IPV6RangeEntity>>(new List<IPV4RangeEntity>() { new IPV4RangeEntity { IPFrom = 1234, IPTo = 16843009 } }, new List<IPV6RangeEntity>() { });
            _ipDataAccess.GetVNIPs().Returns(expectedResult);

            // Action
            var result = _ipService.IsVNIP(ip);

            // Assert
            Assert.True(result);
        }
    }
}