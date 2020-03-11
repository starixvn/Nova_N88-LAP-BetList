namespace Fanex.Athena.BetList.Service.UnitTests.Helpers
{
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Service.Helpers;
    using Common.Constant;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using NUnit.Framework;

    public class BetListHelperTest
    {
        [TestCase(BetType.BetTradeBought)]
        [TestCase(BetType.CashOutBought)]
        public void IsBoughtTicket_IsBoughtBetType_ReturnTrue(BetType betType)
        {
            // Action
            var result = BetListHelper.IsBoughtTicket((int)betType);

            // Assert
            Assert.That(result, Is.True);
        }

        [TestCase(BetType.Handicap)]
        [TestCase(BetType.Outright)]
        public void IsBoughtTicket_IsNotBoughtBetType_ReturnFalse(BetType betType)
        {
            // Action
            var result = BetListHelper.IsBoughtTicket((int)betType);

            // Assert
            Assert.That(result, Is.False);
        }

        [TestCase(18000)]
        [TestCase(18001)]
        [TestCase(18007)]
        public void IsColossusBet_IsColossusBetBetType_ReturnTrue(int betType)
        {
            // Action
            var result = BetListHelper.IsColossusBet(betType);

            // Assert
            Assert.That(result, Is.True);
        }

        [TestCase(17999)]
        [TestCase(18008)]
        public void IsColossusBet_IsNotColossusBetBetType_ReturnFalse(int betType)
        {
            // Action
            var result = BetListHelper.IsColossusBet(betType);

            // Assert
            Assert.That(result, Is.False);
        }

        [TestCase("waiting")]
        [TestCase("WAITING")]
        [TestCase("running")]
        [TestCase("RUNNING")]
        [TestCase("pending")]
        [TestCase("PEnDINg")]
        public void IsRunning_IsWaitingTicket_ReturnTrue(string status)
        {
            // Action
            var result = BetListHelper.IsRunning(status);

            // Assert
            Assert.That(result, Is.True);
        }

        [TestCase("Won")]
        [TestCase("Lose")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void IsRunning_IsNotRunningTicket_ReturnFalse(string status)
        {
            // Action
            var result = BetListHelper.IsRunning(status);

            // Assert
            Assert.That(result, Is.False);
        }

        [TestCase("Combo", "BetTeam", "BetTeam - Combo")]
        [TestCase("", "BetTeam", "BetTeam")]
        public void GenerateClientDetailSystemParlayFunction_WhenCall_ReturnCorrectResult(string comboName, string betTeam, string expectedBetTeam)
        {
            // Arrange
            var tickerHelper = Substitute.For<ITicketHelper>();
            tickerHelper.GetCombinationMPOfferName(Arg.Any<string>()).Returns(comboName);
            tickerHelper.GetBetTypeNameById(Arg.Any<int>()).Returns(betTeam);

            var ticket = new LrfTicket
            {
                TransId = 1,
                RefNo = "RefNoTest",
                BetId = 2,
            };
            const long Custid = 3;
            const string BaseCurrency = "USD";

            var expected = $"showCombMPDetail(1,RefNoTest,2,3,&quot;{expectedBetTeam}&quot;,&quot;USD&quot;)";

            // Action
            var result = BetListHelper.GenerateClientDetailSystemParlayFunction(tickerHelper, BaseCurrency, ticket, Custid);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GenerateClientDetailSystemParlayFunction_MixParlay_ReturnCorrectResult()
        {
            // Arrange
            var tickerHelper = Substitute.For<ITicketHelper>();

            var ticket = new LrfTicket
            {
                TransId = 1,
                RefNo = "RefNoTest",
                BetId = 2,
                BetTypeId = 29,
                BetTeam = "1"
            };
            const long Custid = 3;
            const string BaseCurrency = "USD";

            var expected = $"showCombMPDetail(1,RefNoTest,2,3,&quot;Mix Parlay&quot;,&quot;USD&quot;)";

            // Action
            var result = BetListHelper.GenerateClientDetailSystemParlayFunction(tickerHelper, BaseCurrency, ticket, Custid);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(null, false)]
        [TestCase(" ", false)]
        [TestCase("void", true)]
        [TestCase("running", false)]
        public void IsVoid_WhenCall_ReturnCorrectResult(string status, bool expected)
        {
            // Action
            var result = BetListHelper.IsVoid(status);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}