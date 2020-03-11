namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Common.Utility.Collections;
    using Excel;
    using Excel.Model;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    public class PlayerAdminBetListControllerTest
    {
        private IPlayerAdminBetListDataAccess _dataAccess;

        private ITicketBuilderService _ticketBuilderService;

        private IBettingTicketCenterExcel _bettingTicketCenterExcel;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _dataAccess = Substitute.For<IPlayerAdminBetListDataAccess>();
            _ticketBuilderService = Substitute.For<ITicketBuilderService>();
            _bettingTicketCenterExcel = Substitute.For<IBettingTicketCenterExcel>();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _dataAccess = null;
            _ticketBuilderService = null;
            _bettingTicketCenterExcel = null;
        }

        [Test]
        public async Task GetBetList_WhenCall_GenerateDataWithCorrectOption()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<PlayerAdminBetListMetaModel>();

                var ticketResult = new TicketResult();
                _dataAccess.GetRunningBetListAsync(metaModel).Returns(ticketResult);

                // Action
                await controller.GetBetList(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).ReturnData(
                        ticketResult,
                        Arg.Is<BettingTicketCenterOption>(x =>
                            x.ShowTotal
                            && x.ShowSumStake
                            && !x.ShowLicenseeWl
                            && !x.ShowWinLoss
                            && !x.IsAllowedCustomerComm),
                        metaModel);
                });
            }
        }

        [Test]
        public async Task Export_EmptyData_ReturnNoContentStatus()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<PlayerAdminBetListMetaModel>();
                metaModel.PageIndex = 1;
                metaModel.PageSize = int.MaxValue - 1;

                var ticketResult = new TicketResult();
                _dataAccess.GetRunningBetListAsync(metaModel).Returns(ticketResult);

                // Action
                var result = await controller.Export(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
            }
        }

        [Test]
        public async Task Export_WhenExport_ExportWithCorrectParameter()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<PlayerAdminBetListMetaModel>();

                var ticketResult = new TicketResult
                {
                    MainTicketList = new SortableList<LrfTicket>
                    {
                        FakeHelper.Create<LrfTicket>()
                    }
                };
                _dataAccess.GetRunningBetListAsync(metaModel).Returns(ticketResult);

                // Action
                await controller.Export(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).ExportExcel(Arg.Is<BettingTicketCenterExcelModel>(x =>
                        x.Option.ShowTotal
                        && x.Option.ShowSumStake
                        && !x.Option.ShowLicenseeWl
                        && !x.Option.ShowWinLoss
                        && x.TotalText == ViewResources.Total
                        && !x.Option.IsAllowedCustomerComm
                        && x.Title == $"{ViewResources.Currency}: {metaModel.Currency}"));
                });
            }
        }

        private PlayerAdminBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<PlayerAdminBetListController>(
                _dataAccess,
                _ticketBuilderService,
                _bettingTicketCenterExcel);

            controller.When(x => x.ReturnData(Arg.Any<TicketResult>(), Arg.Any<BettingTicketCenterOption>(), Arg.Any<ICommonValueMetaModel>())).DoNotCallBase();
            controller.When(x => x.ExportExcel(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            return controller;
        }
    }
}