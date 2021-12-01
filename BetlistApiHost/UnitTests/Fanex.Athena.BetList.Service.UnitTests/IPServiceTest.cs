using Fanex.Athena.BetList.DataAccess.Interfaces;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Service.Interfaces;

namespace Fanex.Athena.BetList.Service.UnitTests
{
    [TestFixture]
    public class IPServiceTest
    {
        private IIPDataAccess _ipDataAccess;
        private IIPService _ipService;

        [SetUp]
        public void Setup()
        {
            _ipDataAccess = Substitute.For<IIPDataAccess>();
            _ipService = new IPService(_ipDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _ipDataAccess = null;
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