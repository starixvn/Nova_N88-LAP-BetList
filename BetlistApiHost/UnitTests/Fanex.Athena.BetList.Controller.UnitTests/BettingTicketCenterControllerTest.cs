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
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    public class BettingTicketCenterControllerTest
    {
        private IBettingTicketCenterDataAccess _dataAccess;

        private IBettingTicketCenterExcel _bettingTicketCenterExcel;

        private ITicketBuilderService _ticketBuilderService;

        [SetUp]
        public void FixtureSetUp()
        {
            _dataAccess = Substitute.For<IBettingTicketCenterDataAccess>();
            _bettingTicketCenterExcel = Substitute.For<IBettingTicketCenterExcel>();
            _ticketBuilderService = Substitute.For<ITicketBuilderService>();
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _ticketBuilderService = null;
            _bettingTicketCenterExcel.Dispose();
            _bettingTicketCenterExcel = null;
            _dataAccess = null;
        }

        [Test]
        public async Task GetBySingleCustomer_WhenGet_GenerateDataWithCorrectOption()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>(x => x.IsExport, false);

                var ticketResult = new TicketResult();
                _dataAccess.GetBetListForSingleCustomerAsync(metaModel).Returns(ticketResult);

                // Action
                await controller.GetBySingleCustomer(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).ReturnData(
                        ticketResult,
                        Arg.Is<BettingTicketCenterOption>(x =>
                            x.ShowTotal
                            && x.ShowSumStake
                            && x.ShowLicenseeWl
                            && x.ShowWinLoss
                            && x.ShowExternalId
                            && x.ShowSystemId
                            && x.IsAllowedCustomerComm == metaModel.HasPlayerCom),
                        metaModel);
                });
            }
        }

        [Test]
        public async Task GetBySingleCustomer_WhenExport_EmptyData_ReturnNoContentStatus()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>(x => x.IsExport, true);

                var ticketResult = new TicketResult();
                _dataAccess.GetBetListForSingleCustomerAsync(metaModel).Returns(ticketResult);

                // Action
                var result = await controller.GetBySingleCustomer(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
            }
        }

        [Test]
        public async Task GetBySingleCustomer_WhenExport_ExportWithCorrectParameter()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>(x => x.IsExport, true);

                var ticketResult = new TicketResult
                {
                    MainTicketList = new SortableList<LrfTicket>
                    {
                        FakeHelper.Create<LrfTicket>()
                    }
                };
                _dataAccess.GetBetListForSingleCustomerAsync(metaModel).Returns(ticketResult);

                // Action
                await controller.GetBySingleCustomer(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).ExportExcel(Arg.Is<BettingTicketCenterExcelModel>(x =>
                        x.Option.ShowTotal
                        && x.Option.ShowSumStake
                        && x.Option.ShowLicenseeWl
                        && x.Option.ShowWinLoss
                        && x.TotalText == ViewResources.Total
                        && x.Option.ShowExternalId
                        && x.Option.ShowSystemId
                        && x.Option.IsAllowedCustomerComm == metaModel.HasPlayerCom));
                });
            }
        }

        [Test]
        public async Task GetByRefno_WhenGet_GenerateDataWithCorrectOption()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<RefNoBetListMetaModel>(x => x.IsExport, false);

                var ticketResult = new TicketResult();
                _dataAccess.GetBetListByRefnoAsync(metaModel).Returns(ticketResult);

                // Action
                await controller.GetByRefno(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).ReturnData(
                        ticketResult,
                        Arg.Is<BettingTicketCenterOption>(x =>
                            !x.ShowTotal
                            && !x.ShowSumStake
                            && x.ShowLicenseeWl
                            && x.ShowWinLoss
                            && x.ShowExternalId
                            && x.ShowSystemId
                            && x.IsAllowedCustomerComm == metaModel.HasPlayerCom),
                        metaModel);
                });
            }
        }

        [Test]
        public async Task GetByRefno_WhenExport_ExportWithCorrectParameter()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<RefNoBetListMetaModel>(x => x.IsExport, true);

                var ticketResult = new TicketResult
                {
                    MainTicketList = new SortableList<LrfTicket>
                    {
                        FakeHelper.Create<LrfTicket>()
                    }
                };
                _dataAccess.GetBetListByRefnoAsync(metaModel).Returns(ticketResult);

                // Action
                await controller.GetByRefno(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).ExportExcel(Arg.Is<BettingTicketCenterExcelModel>(x =>
                        !x.Option.ShowTotal
                        && !x.Option.ShowSumStake
                        && x.Option.ShowLicenseeWl
                        && x.Option.ShowWinLoss
                        && x.Option.ShowExternalId
                        && x.Option.ShowSystemId
                        && x.TotalText == null
                        && x.Option.IsAllowedCustomerComm == metaModel.HasPlayerCom));
                });
            }
        }

        [Test]
        public async Task GetByRunning_WhenGet_GenerateDataWithCorrectOption()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<BaseBettingTicketMetaModel>(x => x.IsExport, false);

                var ticketResult = new TicketResult();
                _dataAccess.GetRunningBetListAsync(metaModel).Returns(ticketResult);

                // Action
                await controller.GetByRunning(metaModel);

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
                            && x.ShowExternalId
                            && x.ShowSystemId
                            && !x.IsAllowedCustomerComm),
                        metaModel);
                });
            }
        }

        [Test]
        public async Task GetByRunning_WhenExport_ExportWithCorrectParameter()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<BaseBettingTicketMetaModel>(x => x.IsExport, true);


                var ticketResult = new TicketResult
                {
                    MainTicketList = new SortableList<LrfTicket>
                    {
                        FakeHelper.Create<LrfTicket>()
                    }
                };
                _dataAccess.GetRunningBetListAsync(metaModel).Returns(ticketResult);

                // Action
                await controller.GetByRunning(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).ExportExcel(Arg.Is<BettingTicketCenterExcelModel>(x =>
                        x.Option.ShowTotal
                        && x.Option.ShowSumStake
                        && !x.Option.ShowLicenseeWl
                        && !x.Option.ShowWinLoss
                        && x.Option.ShowExternalId
                        && x.Option.ShowSystemId
                        && x.TotalText == $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}"
                        && !x.Option.IsAllowedCustomerComm));
                });
            }
        }

        private BettingTicketCenterController CreateController()
        {
            var controller = Substitute.ForPartsOf<BettingTicketCenterController>(
                _dataAccess,
                _ticketBuilderService,
                _bettingTicketCenterExcel);

            controller.When(x => x.ReturnData(Arg.Any<TicketResult>(), Arg.Any<BettingTicketCenterOption>(), Arg.Any<ICommonValueMetaModel>())).DoNotCallBase();
            controller.When(x => x.ExportExcel(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            return controller;
        }
    }
}