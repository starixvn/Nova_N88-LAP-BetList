namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Utility.Collections;
    using Common.Utility.Services.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Builder.StatusBuilder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Extensions;
    using UnitTest.Common.Helpers;

    public class MixParlayBetListControllerTest : BaseControllerHelperTest
    {
        private const string TextRenderPattern = "<p class='system-parlay-ticket-infor'>"
                                   + "<span class='system-parlay-combo-type'>{0}</span> x "
                                   + "<span class='system-parlay-unit-stake'>{1} {2}</span>"
                                   + "</p>";

        private ICacheRefAdapter _cacheRefAdapter;

        private IBetListService _betListService;

        private ITicketHelper _ticketHelper;

        private IBetListHTMLBuilder _betlistHtmlBuilder;
        private ITicketBuilderService _ticketBuilderService;

        [SetUp]
        public void SetUp()
        {
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _betListService = Substitute.For<IBetListService>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _ticketBuilderService = Substitute.For<ITicketBuilderService>();

        }

        [TearDown]
        public void TearDown()
        {
            _cacheRefAdapter = null;
            _betListService = null;
            _ticketHelper = null;
            _betlistHtmlBuilder = null;
            _ticketBuilderService = null;

        }

        [Test]
        public async Task Detail_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<MixParlayDetailMetaModel>();

                var ticketResult = new TicketResult();
                _betListService.GetMixParlayDetailTicket(metaModel).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _ticketBuilderService.BuildTicketText(ticketResult: ticketResult, showOdds: true).Returns(ticketText);

                // Action
                var result = (await controller.Detail(metaModel)).Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.EqualTo(ticketText));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public async Task NewParlayDetail_CombinationTypeComboIsNull_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<NewParlayDetailMetaModel>();

                _cacheRefAdapter.GetCombinationTypeCombo(metaModel.BetTeam, metaModel.BetCheck, metaModel.Handicap1, metaModel.Handicap2)
                    .Returns(x => null);

                var ticketResult = new TicketResult();
                _betListService.GetMixParlayDetailTicket(metaModel).Returns(ticketResult);

                // Action
                var result = await controller.NewParlayDetail(metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task NewParlayDetail_NoMainTicket_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<NewParlayDetailMetaModel>();

                var expectedTexts = FakeHelper.CreateMany<string>(2).ToArray();
                _cacheRefAdapter.GetCombinationTypeCombo(metaModel.BetTeam, metaModel.BetCheck, metaModel.Handicap1, metaModel.Handicap2).Returns(expectedTexts);

                var ticketResult = new TicketResult();
                _betListService.GetMixParlayDetailTicket(metaModel).Returns(ticketResult);

                // Action
                var result = (await controller.NewParlayDetail(metaModel)).Data;
                var data = result.GetValue("Data");
                var texts = result.GetValue("Texts");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.Empty);
                    Assert.That(texts, Is.EqualTo(expectedTexts));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public async Task NewParlayDetail_NoParlayTicket_NoRenderParlayContent()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<NewParlayDetailMetaModel>();

                var expectedTexts = FakeHelper.CreateMany<string>(2).ToArray();
                _cacheRefAdapter.GetCombinationTypeCombo(metaModel.BetTeam, metaModel.BetCheck, metaModel.Handicap1, metaModel.Handicap2).Returns(expectedTexts);

                var ticket = FakeHelper.Create<LrfTicket>(x => x.Stake, 0);
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket } };
                _betListService.GetMixParlayDetailTicket(metaModel).Returns(ticketResult);

                var status = Substitute.For<IStatus>();
                _betlistHtmlBuilder.CreateStatusBuilder(ticket.BetTypeId).Returns(status);

                var statusTemplate = FakeHelper.Create<Status_Template>();
                status.Render(ticket, _ticketHelper, ticketResult.TicketDataList, isShowVNIP: false).Returns(statusTemplate);

                var choice = FakeHelper.Create<string>();
                _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false).Returns(choice);

                var transDateAndStatus = FakeHelper.Create<string>();
                _betlistHtmlBuilder.BuildTransEventDateAndStatusResult(ticket, ticketResult.TicketDataList).Returns(transDateAndStatus);

                var expectedData = new List<string> { choice + transDateAndStatus };

                // Action
                var result = (await controller.NewParlayDetail(metaModel)).Data;
                var data = result.GetValue("Data");
                var texts = result.GetValue("Texts");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(expectedData));
                    Assert.That(texts, Is.EqualTo(expectedTexts));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public async Task NewParlayDetail_HasSystemParlayTicket_RenderParlayContent()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<NewParlayDetailMetaModel>();
                metaModel.Handicap1 = 0;

                var texts = FakeHelper.CreateMany<string>(2).ToArray();
                _cacheRefAdapter.GetCombinationTypeCombo(metaModel.BetTeam, metaModel.BetCheck, metaModel.Handicap1, metaModel.Handicap2).Returns(texts);

                var ticket = FakeHelper.Create<LrfTicket>(x => x.Stake, 1);
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket } };
                _betListService.GetMixParlayDetailTicket(metaModel).Returns(ticketResult);

                var status = Substitute.For<IStatus>();
                _betlistHtmlBuilder.CreateStatusBuilder(ticket.BetTypeId).Returns(status);

                status.Render(ticket, _ticketHelper, ticketResult.TicketDataList, isShowVNIP: false).Returns(FakeHelper.Create<Status_Template>());
                _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false).Returns(FakeHelper.Create<string>());

                _betlistHtmlBuilder.BuildTransEventDateAndStatusResult(ticket, ticketResult.TicketDataList).Returns(FakeHelper.Create<string>());

                var expectedTexts = new List<string> { string.Format(TextRenderPattern, texts[0], ticket.Currency, ticket.Stake.ToString("N0")), texts[1] };

                // Action
                var result = (await controller.NewParlayDetail(metaModel)).Data;
                var actualTexts = result.GetValue("Texts");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(actualTexts, Is.EqualTo(expectedTexts));
                });
            }
        }

        [Test]
        public async Task NewParlayDetail_HasComboParlayTicket_RenderParlayContent()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<NewParlayDetailMetaModel>();
                metaModel.Handicap1 = 1;

                var texts = FakeHelper.CreateMany<string>(3).ToArray();
                _cacheRefAdapter.GetCombinationTypeCombo(metaModel.BetTeam, metaModel.BetCheck, metaModel.Handicap1, metaModel.Handicap2).Returns(texts);

                var ticket1 = FakeHelper.Create<LrfTicket>(x => x.Stake, 1);
                ticket1.MatchId = 2;
                var ticket2 = FakeHelper.Create<LrfTicket>(x => x.Stake, 1);
                ticket2.MatchId = 1;
                var ticketResult = new TicketResult { MainTicketList = new SortableList<LrfTicket> { ticket2, ticket1 } };
                _betListService.GetMixParlayDetailTicket(metaModel).Returns(ticketResult);

                var status = Substitute.For<IStatus>();
                _betlistHtmlBuilder.CreateStatusBuilder(Arg.Any<int>()).Returns(status);

                status.Render(Arg.Any<LrfTicket>(), _ticketHelper, ticketResult.TicketDataList, isShowVNIP: false).Returns(FakeHelper.Create<Status_Template>());
                _betlistHtmlBuilder.BuildChoice(Arg.Any<LrfTicket>(), _ticketHelper, ticketResult.TicketDataList, false).Returns(FakeHelper.Create<string>());

                _betlistHtmlBuilder.BuildTransEventDateAndStatusResult(Arg.Any<LrfTicket>(), ticketResult.TicketDataList).Returns(FakeHelper.Create<string>());

                var expectedTexts = new List<string>
                {
                    string.Format(TextRenderPattern, texts[0], ticket2.Currency, ticket2.Stake.ToString("N0")),
                    string.Format(TextRenderPattern, texts[1], ticket1.Currency, ticket1.Stake.ToString("N0")),
                    texts[2]
                };

                // Action
                var result = (await controller.NewParlayDetail(metaModel)).Data;
                var actualTexts = result.GetValue("Texts");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(actualTexts, Is.EqualTo(expectedTexts));
                });
            }
        }

        private MixParlayBetListController CreateController()
        {
            return new MixParlayBetListController(_cacheRefAdapter, _betListService, _ticketHelper, _betlistHtmlBuilder, _ticketBuilderService);
        }
    }
}