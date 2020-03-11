namespace Fanex.Athena.BetList.Service.UnitTests
{
    using Common.Constant;
    using Fanex.Athena.BetList.Service.Helpers;
    using Fanex.Athena.Common.Utility.Collections;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class TicketBuilderServiceTest
    {
        private IBetListHTMLBuilder _betlistHtmlBuilder;

        private ITicketHelper _ticketHelper;
        private  ITicketBuilderService _ticketBuilderService;
        [SetUp]
        public void SetUp()
        {
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _ticketBuilderService = new TicketBuilderService(
                _betlistHtmlBuilder,
                _ticketHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _ticketHelper = null;
            _betlistHtmlBuilder = null;
            _ticketBuilderService = null;
        }
     

        [Test]
        public void BuildTicketText_IsCombinationMixParlay_ReturnCustomOddText()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket { BetTypeId = (int)BetType.CombinationMixParlay, Status = BetStatus.Lose };
            ticketResult.MainTicketList.Add(ticket);

            // Action
            _ticketBuilderService.BuildTicketText(ticketResult, metaModel, false);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Not.Empty);
        }

        [Test]
        public void BuildTicketText_NotCombinationMixParlay_ReturnNormalOddText()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket { BetTypeId = (int)BetType.Handicap, Status = BetStatus.Won };
            ticketResult.MainTicketList.Add(ticket);

            _ticketHelper.ClientDetailSystemParlayFunction.Returns(string.Empty);

            // Action
            _ticketBuilderService.BuildTicketText(ticketResult, metaModel, false);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Empty);
        }

        [Test]
        public void BuildTicketTextList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = 0,
                SportTypeId = (int)SportType.Soccer,
                Status = "running",
                BetEffective = 0
            };
            var ticketResult = new TicketResult
            {
                MainTicketList = new SortableList<LrfTicket> { ticket }
            };
            var metaModel = FakeHelper.Create<BetListMetaModel>();

            var expectedTicketText = new TicketText
            {
                Index = 0,
                ExternalId = ticket.ExternalId,
                SystemId = ticket.UserName,
                TransTime = FakeHelper.Create<string>(),
                Status = FakeHelper.Create<string>(),
                Choice = FakeHelper.Create<string>(),
                Odds = FakeHelper.Create<string>(),
                Stake = FakeHelper.Create<string>(),
                LicenseeWL = "-",
                WinLoss = "-",
                BetEffective = "<div class='underdog'>0.00</div>"
            };
            _betlistHtmlBuilder.BuildTrans(ticket).Returns(expectedTicketText.TransTime);
            _betlistHtmlBuilder.BuildStatus(ticket, _ticketHelper, ticketResult.TicketDataList, false).Returns(expectedTicketText.Status);
            _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false).Returns(expectedTicketText.Choice);
            _betlistHtmlBuilder.BuildOdds(ticket, ticketResult.TicketDataList, Arg.Any<GetCachePropertyById>()).Returns(expectedTicketText.Odds);
            _betlistHtmlBuilder.BuildStake(ticket).Returns(expectedTicketText.Stake);

            // Action
            var result = _ticketBuilderService.BuildTicketText(ticketResult, metaModel, 0);
            var firstItem = result[0];

            // Assert
            Assert.That(firstItem, Tobe.SameProperties(expectedTicketText));
        }

        [Test]
        public void BuildTicketTextList_CombinationMixParlayTicket_AddCombMPDetailLink()
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.CombinationMixParlay,
                SportTypeId = (int)SportType.Soccer,
                Status = "running",
                BetTeam = "1"
            };
            var ticketResult = new TicketResult
            {
                MainTicketList = new SortableList<LrfTicket> { ticket }
            };
            var metaModel = FakeHelper.Create<BetListMetaModel>();

            _ticketHelper.GetCombinationMPOfferName(ticket.BetTeam).Returns(FakeHelper.Create<string>());

            var expected = BetListHelper.GenerateClientDetailSystemParlayFunction(_ticketHelper, metaModel.BaseCurrency, ticket, metaModel.CustId);

            // Action
            _ticketBuilderService.BuildTicketText(ticketResult, metaModel);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.EqualTo(expected));
        }

        [Test]
        public void BuildTicketTextList_CombinationMixParlayTicket_MetaModelIsNull_DoNotAddCombMPDetailLink()
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.CombinationMixParlay,
                SportTypeId = (int)SportType.Soccer,
                Status = "running",
                BetTeam = "1",
                CustId = 1
            };
            var ticketResult = new TicketResult
            {
                MainTicketList = new SortableList<LrfTicket> { ticket }
            };

            // Action
            _ticketBuilderService.BuildTicketText(ticketResult);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Empty);
        }

        [Test]
        public void BuildSumWinLoss_WhenCall_ShouldHasData()
        {
            // Assert
            var ticketResult = new TicketResult { SumWinLoss = FakeHelper.Create<decimal>(), SumPlayerComm = FakeHelper.Create<decimal>() };

            // Action
            var result = _ticketBuilderService.BuildSumWinLoss(ticketResult, FakeHelper.Create<bool>());
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
            });
        }
    }
}