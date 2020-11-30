using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.BetList.Excel.UnitTests
{
    [TestFixture]
    internal class BetListExcelTest_BuildTicket
    {
        private ITicketHelper _ticketHelper;
        private IBetListExcelBuilder _builder;
        private BetListExcel _betListExcel;

        [SetUp]
        public void Setup()
        {
            _ticketHelper = Substitute.For<ITicketHelper>();
            _builder = Substitute.For<IBetListExcelBuilder>();
            _betListExcel = CreateBetListExcel();
            _betListExcel.InitializeFonts(_builder);
        }

        [Test]
        public void BuildTicket_WhenCall_CallReleventMethods()
        {
            // Arrange
            const int Index = 9000;
            _ticketHelper.Index.Returns(Index);

            var ticketResult = new TicketResult();
            var ticket = new LrfTicket
            {
                ExternalId = "ExternalId",
                UserName = "UserName",
                Winlost = 1,
                BetEffective = 2,
                LicenseeWL = 3,
                Comm = 4,
                Status = BetStatus.Won,
                BetTypeId = (int)BetType.Handicap
            };
            ticketResult.MainTicketList.Add(ticket);

            // Action
            _betListExcel.BuildTicket(_builder, ticketResult, ticket);

            // Assert
            Assert.Multiple(() =>
            {
                // AddIndex
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == Index.ToString()), Arg.Any<ICellStyle>());

                // AddExternalId
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == ticket.ExternalId), Arg.Any<ICellStyle>());

                // AddSystemId
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == ticket.UserName), Arg.Any<ICellStyle>());

                // AddCustomerWL
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == string.Format("{0:#,##0.00}", ticket.Winlost)));

                // AddBetEffective
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == string.Format("{0:#,##0.00}\n", ticket.BetEffective)));

                // AddLicenseeWL
                _builder.Received(1).AddCell(Arg.Is<IRichTextString>(x => x.String == string.Format("{0:#,##0.00}", ticket.LicenseeWL + ticket.Comm)));

                _builder.Received(1).AddTrans(ticket, _ticketHelper);
                _builder.Received(1).AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList);
                _builder.Received(1).AddOdds(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
                _builder.Received(1).AddStake(ticket, _ticketHelper);
                _builder.Received(1).AddStatus(ticket, _ticketHelper, ticketResult.TicketDataList, false);
            });
        }

        private BetListExcel CreateBetListExcel()
        {
            var betListExcel = Substitute.ForPartsOf<BetListExcel>(
                Substitute.For<IAppSettings>(),
                _ticketHelper,
                new BetListShowingOption(),
                LicenseeIdentity.OneTwoBet,
                new int[] { },
                false);
            betListExcel.CreateExcelBuilder().Returns(_builder);
            return betListExcel;
        }
    }
}
