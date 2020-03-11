namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Constant;
    using DataAccess.Interfaces;
    using Fanex.Athena.Common.DataAccess.Interfaces;
    using Fanex.Athena.Common.Model.DTO;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Builder.StatusBuilder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Model.DTO;
    using Model.MetaModel;
    using Model.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    public class CashOutBetListControllerTest
    {
        private ICashOutBetListDataAccess _cashOutBetListDataAccess;

        private ITicketHelper _ticketHelper;

        private IBetListHTMLBuilder _betListHTMLBuilder;

        [SetUp]
        public void SetUp()
        {
            _cashOutBetListDataAccess = Substitute.For<ICashOutBetListDataAccess>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _betListHTMLBuilder = Substitute.For<IBetListHTMLBuilder>();
        }

        [TearDown]
        public void TearDown()
        {
            _cashOutBetListDataAccess = null;
            _ticketHelper = null;
            _betListHTMLBuilder = null;
        }

        [Test]
        public async Task Detail_BuilOriginalTicketText_OriginalTicketIsEmpty_ReturnEmptyObject()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();

                var viewModel = new CashOutBetListDetailViewModel();

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;

                // Assert
                Assert.That(result.OriginalTicketText.Choice, Is.Null);
            }
        }

        [Test]
        public async Task Detail_BuildCashOutTicketTextList_CashoutTicketIsNull_ReturnEmptyObject()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();

                var viewModel = new CashOutBetListDetailViewModel();

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;

                // Assert
                Assert.That(result.CashOutTicketTextList, Is.Empty);
            }
        }

        [Test]
        public async Task Detail_BuildCashOutTicketTextList_CashoutTicketIsEmpty_ReturnEmptyObject()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();

                var viewModel = new CashOutBetListDetailViewModel { CashOutTicketList = Enumerable.Empty<CashOutTicket>() };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;

                // Assert
                Assert.That(result.CashOutTicketTextList, Is.Empty);
            }
        }

        [Test]
        public async Task Detail_BuildCashOutTicketTextList_ReturnCorrectOriginalTicketText()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>(x => x.BetdaqId, 0);

                var originalTicket = FakeHelper.Create<LrfTicket>();
                originalTicket.TransId = 0;
                originalTicket.Status = "running";
                var viewModel = new CashOutBetListDetailViewModel { OriginalTicket = originalTicket };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                var statusHtml = FakeHelper.Create<Status_Template>();
                var status = Substitute.For<IStatus>();
                status.Render(originalTicket, _ticketHelper, null, true).Returns(statusHtml);
                _betListHTMLBuilder.CreateStatusBuilder(originalTicket.BetTypeId).Returns(status);

                var transTime = FakeHelper.Create<string>();
                _betListHTMLBuilder.BuildTrans(originalTicket).Returns(transTime);

                var choice = FakeHelper.Create<string>();
                _betListHTMLBuilder.BuildChoice(originalTicket, _ticketHelper, null, false).Returns(choice);

                var odds = FakeHelper.Create<string>();
                _betListHTMLBuilder.BuildOdds(originalTicket, null, Arg.Any<GetCachePropertyById>()).Returns(odds);

                var stake = FakeHelper.Create<string>();
                _betListHTMLBuilder.BuildStake(originalTicket).Returns(stake);

                var winloss = "-";

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;
                var originalTicketText = result.OriginalTicketText;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(originalTicketText.ExternalId, Is.EqualTo(originalTicket.ExternalId));
                    Assert.That(originalTicketText.SystemId, Is.EqualTo(originalTicket.UserName));
                    Assert.That(originalTicketText.TransTime, Is.EqualTo(transTime));
                    Assert.That(originalTicketText.Status, Is.EqualTo(statusHtml.ToString()));
                    Assert.That(originalTicketText.Choice, Is.EqualTo(choice));
                    Assert.That(originalTicketText.Odds, Is.EqualTo(odds));
                    Assert.That(originalTicketText.Stake, Is.EqualTo(stake));
                    Assert.That(originalTicketText.WinLoss, Is.EqualTo(winloss));
                });
            }
        }

        [TestCase(1, 2, false)]
        [TestCase(2, 2, true)]
        public async Task Detail_BuildCashOutTicketTextList_ReturnCorrectCashOutTicketText(int metaModelTransId, int ticketTransId, bool expectedIsHighlight)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();
                metaModel.BetdaqId = 0;
                metaModel.TransId = metaModelTransId;

                var cashOutTicket = FakeHelper.Create<CashOutTicket>();
                cashOutTicket.TransId = ticketTransId;
                var cashOutTicketList = new List<CashOutTicket> { cashOutTicket };
                var viewModel = new CashOutBetListDetailViewModel { CashOutTicketList = cashOutTicketList };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                var statusHtml = FakeHelper.Create<Status_Template>();
                var status = Substitute.For<IStatus>();
                status.Render(Arg.Any<LrfTicket>(), _ticketHelper, null, true).Returns(statusHtml);
                _betListHTMLBuilder.CreateStatusBuilder(cashOutTicket.BetType).Returns(status);

                var expectedTranstime = ViewResources.RefNo + ": " + cashOutTicket.TransId + Environment.NewLine + "<div class='time'>" + cashOutTicket.TransDate + "</div>";

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;
                var cashoutTicketText = result.CashOutTicketTextList.ElementAt(0);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(cashoutTicketText.TransTime, Is.EqualTo(expectedTranstime));
                    Assert.That(cashoutTicketText.Stake, Is.EqualTo(cashOutTicket.Stake.ToString("#,##0.##")));
                    Assert.That(cashoutTicketText.Price, Is.EqualTo(cashOutTicket.Price.ToString(FormatKey.DecimalFormat)));
                    Assert.That(cashoutTicketText.WinLoss, Is.EqualTo(cashOutTicket.WinLoss.ToString(FormatKey.DecimalFormat)));
                    Assert.That(cashoutTicketText.Status, Is.EqualTo(statusHtml.ToString()));
                    Assert.That(cashoutTicketText.IsHighlight, Is.EqualTo(expectedIsHighlight));

                    status.Received().Render(
                        Arg.Is<LrfTicket>(x => x.BetTypeId == cashOutTicket.BetType && x.Status == cashOutTicket.Status),
                        _ticketHelper,
                        null,
                        true);
                });
            }
        }

        [TestCase(2, 1)]
        [TestCase(1, 2)]
        public async Task Detail_IsNotOriginalTicket_DoNotHighlightOriginalTicket(int metaModelTransId, int ticketTransId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();
                metaModel.BetdaqId = 0;
                metaModel.TransId = metaModelTransId;

                var originalTicket = FakeHelper.Create<LrfTicket>();
                originalTicket.TransId = ticketTransId;
                originalTicket.Status = "running";

                var viewModel = new CashOutBetListDetailViewModel { OriginalTicket = originalTicket };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                var statusHtml = FakeHelper.Create<Status_Template>();
                var status = Substitute.For<IStatus>();
                status.Render(originalTicket, _ticketHelper, null, true).Returns(statusHtml);
                _betListHTMLBuilder.CreateStatusBuilder(originalTicket.BetTypeId).Returns(status);

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;

                // Assert
                Assert.That(result.IsHighlightOriginalTicket, Is.False);
            }
        }

        [Test]
        public async Task Detail_BetdaqidIsZero_DoNotHighlightOriginalTicket()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();
                metaModel.BetdaqId = 0;

                var originalTicket = FakeHelper.Create<LrfTicket>();
                originalTicket.TransId = metaModel.TransId - 1;
                originalTicket.Status = "running";

                var viewModel = new CashOutBetListDetailViewModel { OriginalTicket = originalTicket };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                var statusHtml = FakeHelper.Create<Status_Template>();
                var status = Substitute.For<IStatus>();
                status.Render(originalTicket, _ticketHelper, null, true).Returns(statusHtml);
                _betListHTMLBuilder.CreateStatusBuilder(originalTicket.BetTypeId).Returns(status);

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;

                // Assert
                Assert.That(result.IsHighlightOriginalTicket, Is.False);
            }
        }

        [TestCase(900)]
        [TestCase(912)]
        public async Task Detail_IsNotSpecialCashoutBettype_DoNotHighlightOriginalTicket(int bettype)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();
                metaModel.BetdaqId = 1;
                metaModel.BetType = bettype;

                var originalTicket = FakeHelper.Create<LrfTicket>();
                originalTicket.TransId = metaModel.TransId - 1;
                originalTicket.Status = "running";

                var viewModel = new CashOutBetListDetailViewModel { OriginalTicket = originalTicket };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                var statusHtml = FakeHelper.Create<Status_Template>();
                var status = Substitute.For<IStatus>();
                status.Render(originalTicket, _ticketHelper, null, true).Returns(statusHtml);
                _betListHTMLBuilder.CreateStatusBuilder(originalTicket.BetTypeId).Returns(status);

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;

                // Assert
                Assert.That(result.IsHighlightOriginalTicket, Is.False);
            }
        }

        [Test]
        public async Task Detail_IsOriginalTicket_HighlightOriginalTicket()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();
                metaModel.BetdaqId = 1;
                metaModel.BetType = 1;

                var originalTicket = FakeHelper.Create<LrfTicket>();
                originalTicket.TransId = metaModel.TransId;
                originalTicket.Status = "running";

                var viewModel = new CashOutBetListDetailViewModel { OriginalTicket = originalTicket };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                var statusHtml = FakeHelper.Create<Status_Template>();
                var status = Substitute.For<IStatus>();
                status.Render(originalTicket, _ticketHelper, null, true).Returns(statusHtml);
                _betListHTMLBuilder.CreateStatusBuilder(originalTicket.BetTypeId).Returns(status);

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;

                // Assert
                Assert.That(result.IsHighlightOriginalTicket, Is.True);
            }
        }

        [Test]
        public async Task Detail_IsCashoutBettype_HighlightOriginalTicket()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();
                metaModel.BetdaqId = 1;
                metaModel.BetType = 1;

                var originalTicket = FakeHelper.Create<LrfTicket>();
                originalTicket.TransId = metaModel.TransId - 1;
                originalTicket.Status = "running";

                var viewModel = new CashOutBetListDetailViewModel { OriginalTicket = originalTicket };

                _cashOutBetListDataAccess.GetDetail(metaModel).Returns(viewModel);

                var statusHtml = FakeHelper.Create<Status_Template>();
                var status = Substitute.For<IStatus>();
                status.Render(originalTicket, _ticketHelper, null, true).Returns(statusHtml);
                _betListHTMLBuilder.CreateStatusBuilder(originalTicket.BetTypeId).Returns(status);

                // Action
                var result = (CashOutBetListDetailViewModel)(await controller.Detail(metaModel)).Data;

                // Assert
                Assert.That(result.IsHighlightOriginalTicket, Is.True);
            }
        }

        private CashOutBetListController CreateController()
        {
            return new CashOutBetListController(_cashOutBetListDataAccess, _ticketHelper, _betListHTMLBuilder);
        }
    }
}