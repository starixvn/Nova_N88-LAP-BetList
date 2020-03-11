namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Excel;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Excel.Model;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.Common.Constant;
    using Fanex.Athena.Common.Utility.Collections;
    using Fanex.Athena.UnitTest.Common.Extensions;
    using Model.DTO;
    using Model.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class BettingTicketCenterBaseControllerTest : BaseControllerHelperTest
    {
        private IBettingTicketCenterDataAccess _dataAccess;

        private ITicketBuilderService _ticketBuilderService;

        private IBettingTicketCenterExcel _bettingTicketCenterExcel;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _dataAccess = Substitute.For<IBettingTicketCenterDataAccess>();
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
        public async Task ExportExcel_WhenCall_ExportCorrectData()
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

                var workbook = Substitute.For<IWorkbook>();
                _bettingTicketCenterExcel.Export(Arg.Any<BettingTicketCenterExcelModel>()).Returns(workbook);

                // Action
                await controller.GetByRefno(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    _bettingTicketCenterExcel.Received().Export(Arg.Is<BettingTicketCenterExcelModel>(x =>
                       !x.Option.ShowTotal
                        && !x.Option.ShowSumStake
                        && x.Option.ShowLicenseeWl
                        && x.Option.ShowWinLoss
                        && x.TotalText == null
                        && x.Option.IsAllowedCustomerComm == metaModel.HasPlayerCom));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public async Task ReturnData_ShowTotal_GenerateCorrectData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>(x => x.IsExport, false);

                var ticketResult = new TicketResult
                {
                    TotalRows = FakeHelper.Create<int>(),
                    SumLicenseeWL = FakeHelper.Create<decimal>(),
                    SumComm = FakeHelper.Create<decimal>(),
                    SumStake = FakeHelper.Create<decimal>()
                };
                _dataAccess.GetBetListForSingleCustomerAsync(metaModel).Returns(ticketResult);

                // Action
                var result = (await controller.GetBySingleCustomer(metaModel) as JsonResult).Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.GetValue("Total").GetValue("WinLoss"), Is.Not.Null);
                    Assert.That(result.GetValue("Total").GetValue("LicenseeWL"), Is.EqualTo((ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat)));
                    Assert.That(result.GetValue("TotalRows"), Is.EqualTo(ticketResult.TotalRows));
                    Assert.That(result.GetValue("Total").GetValue("Stake"), Is.EqualTo((ticketResult.SumStake).ToString(FormatKey.DecimalFormat)));
                });
            }
        }

        [Test]
        public async Task ReturnData_DoNotShowTotal_GenerateCorrectData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<RefNoBetListMetaModel>(x => x.IsExport, false);

                var ticketResult = new TicketResult();
                _dataAccess.GetBetListByRefnoAsync(metaModel).Returns(ticketResult);

                // Action
                var result = (await controller.GetByRefno(metaModel) as JsonResult).Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.GetValue("Total").GetValue("Stake"), Is.Null);
                });
            }
        }

        private BettingTicketCenterController CreateController()
        {
            return Substitute.ForPartsOf<BettingTicketCenterController>(
                _dataAccess,
                _ticketBuilderService,
                _bettingTicketCenterExcel);
        }
    }
}