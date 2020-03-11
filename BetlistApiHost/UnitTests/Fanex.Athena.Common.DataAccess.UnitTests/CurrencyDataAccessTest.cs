using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Common.DataAccess.UnitTests
{
    [TestFixture]
    public class CurrencyDataAccessTest : BaseDataAccessHelperTest<CurrencyDataAccess>
    {
        private const int CacheTime1DayInSecond = 86400;

        private const string CacheKey = "_LAP_BETLIST_CACHE_CURRENCY_";

        private IMemoryCacheService _cache;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _cache = Substitute.For<IMemoryCacheService>();
            DataAccess = Substitute.ForPartsOf<CurrencyDataAccess>(_cache);
            ObjectDb = Substitute.For<IObjectDb>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _cache = null;
        }

        [Test]
        public async Task GetCurrencyListAsync_DataInCache_DoNotCallDataAccess()
        {
            // Arrange
            var cachedData = FakeHelper.Create<IDictionary<int, string>>();
            _cache.Get<IDictionary<int, string>>(CacheKey).Returns(cachedData);

            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);

            // Action
            var result = await DataAccess.GetCurrencyListAsync();

            // Assert
            await ObjectDb.DidNotReceiveWithAnyArgs().ExecuteReaderAsync();
            Assert.That(result, Is.EqualTo(cachedData));
        }

        [Test]
        public async Task GetCurrencyListAsync_DataNotInCache_DataAccessAndReturnCorrectData()
        {
            // Arrange
            const string UspName = "Lrf_CurrencySel";
            const int Key = 1;
            const string Value = "UUS";
            _cache.Get<IDictionary<int, string>>(CacheKey).Returns(x => null);

            MapReaderData(new
            {
                Id = Key,
                Name = Value
            });
            Reader.Read().Returns(true, false);
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);

            // Action
            var result = await DataAccess.GetCurrencyListAsync();

            // Assert
            _cache.Received(1).Set(CacheKey, Arg.Is<IDictionary<int, string>>(x => x.ContainsKey(Key)), CacheTime1DayInSecond);
            Assert.That(result[Key], Is.EqualTo(Value));
        }
    }
}