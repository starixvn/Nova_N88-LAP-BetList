using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.Resources;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.BetList.Excel.UnitTests
{
    [TestFixture]
    internal class BetListExcelTest_AssignSheetdata
    {
        private ITicketHelper _ticketHelper;
        private IBetListExcelBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _ticketHelper = Substitute.For<ITicketHelper>();
            _builder = Substitute.For<IBetListExcelBuilder>();
        }

        [Test]
        public void AssignSheetsData_ShowSumStake_AddFooterSumStake()
        {
            // Arrange
            const string Title = "aaaa";
            var betListExcel = CreateBetListExcel(isShowSumStake: true);
            betListExcel.When(x => x.BuildTicket(Arg.Any<IBetListExcelBuilder>(), Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket { Stake = 99 });
            ticketResult.MainTicketList.Add(new LrfTicket { Stake = 100 });

            // Action
            betListExcel.AssignSheetsData(_builder, ticketResult, Title, 0);

            // Assert
            _builder.Received(1).AddCell(
                    Arg.Is<IRichTextString>(x => x.String == "199.00"),
                    Arg.Any<ICellStyle>());
        }

        [Test]
        public void AssignSheetsData_ShowWinLoss_AddFooterWinLoss()
        {
            // Arrange
            const string Title = "aaaa";
            var betListExcel = CreateBetListExcel(showingOption: new BetListShowingOption { GrandTotalVisible = true, CustomerCommVisible = true });
            betListExcel.When(x => x.BuildTicket(Arg.Any<IBetListExcelBuilder>(), Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();
            var ticketResult = new TicketResult
            {
                SumWinLoss = 100,
                SumPlayerComm = 0.88m,
                SumLicenseeWL = 200,
                SumComm = 0.99m,
                SumBetEffective = 999
            };
            ticketResult.MainTicketList.Add(new LrfTicket { Stake = 0 });

            // Action
            betListExcel.AssignSheetsData(_builder, ticketResult, Title, 0);

            // Assert
            Assert.Multiple(() =>
            {
                // Sum customerWL
                _builder.Received(1).AddCell(
                        Arg.Is<IRichTextString>(x => x.String == "100.88"),
                        Arg.Any<ICellStyle>());

                // Sum LicenseeWL
                _builder.Received(1).AddCell(
                        Arg.Is<IRichTextString>(x => x.String == "200.99"),
                        Arg.Any<ICellStyle>());

                // SumBetEffective
                _builder.Received(1).AddCell(
                        Arg.Is<IRichTextString>(x => x.String == "999.00"),
                        Arg.Any<ICellStyle>());
            });
        }

        [Test]
        public void AssignSheetsData_NotShowSumStakeAndWinLoss_AddFooterWinLoss()
        {
            // Arrange
            const string Title = "aaaa";
            var betListExcel = CreateBetListExcel(showingOption: new BetListShowingOption { GrandTotalVisible = false });
            betListExcel.When(x => x.BuildTicket(Arg.Any<IBetListExcelBuilder>(), Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();
            var ticketResult = CreateTicketResult();

            // Action
            betListExcel.AssignSheetsData(_builder, ticketResult, Title, 0);

            // Assert
            Assert.Multiple(() =>
            {
                // Sum customerWL
                _builder.DidNotReceive().AddCell(
                        Arg.Is<IRichTextString>(x => x.String == "100.88"),
                        Arg.Any<ICellStyle>());

                // Sum LicenseeWL
                _builder.DidNotReceive().AddCell(
                        Arg.Is<IRichTextString>(x => x.String == "200.99"),
                        Arg.Any<ICellStyle>());

                // SumBetEffective
                _builder.DidNotReceive().AddCell(
                        Arg.Is<IRichTextString>(x => x.String == "999.00"),
                        Arg.Any<ICellStyle>());
            });
        }

        private static TicketResult CreateTicketResult()
        {
            var ticketResult = new TicketResult
            {
                SumWinLoss = 100,
                SumPlayerComm = 0.88m,
                SumLicenseeWL = 200,
                SumComm = 0.99m,
                SumBetEffective = 999
            };
            ticketResult.MainTicketList.Add(new LrfTicket { Stake = 0 });
            return ticketResult;
        }

        [Test]
        public void AssignSheetsData_NotShowCustomerComm_AddCorrectHeader()
        {
            // Arrange
            const string Title = "aaaa";
            var betListExcel = CreateBetListExcel(showingOption: new BetListShowingOption { GrandTotalVisible = false, CustomerCommVisible = false });
            betListExcel.When(x => x.BuildTicket(Arg.Any<IBetListExcelBuilder>(), Arg.Any<TicketResult>(), Arg.Any<LrfTicket>())).DoNotCallBase();
            var ticketResult = CreateTicketResult();

            // Action
            betListExcel.AssignSheetsData(_builder, ticketResult, Title, 0);

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<IRichTextString>(x => x.String == ViewResources.WinLoss),
                Arg.Any<ICellStyle>());
        }

        private BetListExcel CreateBetListExcel(
            bool isShowSumStake = false,
            BetListShowingOption showingOption = null,
            int[] hiddenColumnIndexes = null)
        {
            var betListExcel = Substitute.ForPartsOf<BetListExcel>(
                Substitute.For<IAppSettings>(),
                _ticketHelper,
                showingOption ?? new BetListShowingOption(),
                LicenseeIdentity.OneTwoBet,
                hiddenColumnIndexes ?? new int[] { },
                isShowSumStake);
            betListExcel.CreateExcelBuilder().Returns(_builder);
            betListExcel.InitializeFonts(_builder);
            return betListExcel;
        }
    }
}