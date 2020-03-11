namespace Fanex.Athena.Common.DataAccess.UnitTests
{
    using System;
    using Data;
    using DataAccess;
    using Model.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Utility.Services.Interfaces;

    public class BalanceUpDataAccessTest : BaseDataAccessHelperTest<BalanceUpDataAccess>
    {
        private const string UspName = "Lrf_SelBalanceUpDay";

        private IMemoryCacheService _cache;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _cache = Substitute.For<IMemoryCacheService>();
            DataAccess = Substitute.ForPartsOf<BalanceUpDataAccess>(_cache);
            ObjectDb = Substitute.For<IObjectDb>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _cache = null;
        }

        [Test]
        public void GetBalanceUp_NoCache_ReturnDataFromDB()
        {
            // Arrange
            var balanceUpDay = new DateTime(2017, 1, 1);
            var moveBetsDay = new DateTime(2017, 2, 2);
            MapReaderData(new
            {
                balanceupday = balanceUpDay.ToString("MM/dd/yyyy"),
                movebetsday = moveBetsDay.ToString("MM/dd/yyyy")
            });
            Reader.Read().Returns(true);
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
            _cache.Get<BalanceUp>(Arg.Any<string>()).Returns(x => { return null; });

            // Action
            var result = DataAccess.GetBalanceUp();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.BalanceUpDay, Is.EqualTo(balanceUpDay));
                Assert.That(result.MoveBetsDay, Is.EqualTo(moveBetsDay));
                _cache.Received().Set(
                    Arg.Any<string>(),
                    Arg.Any<BalanceUp>(),
                    Arg.Any<int>());
            });
        }

        [Test]
        public void GetBalanceUp_NoCache_DataFromDBIsEmpty_ReturnEmptyData()
        {
            // Arrange
            Reader.Read().Returns(false);
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
            _cache.Get<BalanceUp>(Arg.Any<string>()).Returns(x => { return null; });

            var expectedBalanceUp = new BalanceUp();

            // Action
            var result = DataAccess.GetBalanceUp();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.BalanceUpDay, Is.EqualTo(expectedBalanceUp.BalanceUpDay));
                Assert.That(result.MoveBetsDay, Is.EqualTo(expectedBalanceUp.MoveBetsDay));
            });
        }

        [Test]
        public void GetBalanceUp_Cached_ReturnDataFromCache()
        {
            // Arrange
            var balanceUp = new BalanceUp
            {
                BalanceUpDay = new DateTime(2017, 1, 1),
                MoveBetsDay = new DateTime(2017, 2, 2)
            };
            _cache.Get<BalanceUp>(Arg.Any<string>()).Returns(balanceUp);

            // Action
            var result = DataAccess.GetBalanceUp();

            // Assert
            Assert.That(result, Is.EqualTo(balanceUp));
        }
    }
}