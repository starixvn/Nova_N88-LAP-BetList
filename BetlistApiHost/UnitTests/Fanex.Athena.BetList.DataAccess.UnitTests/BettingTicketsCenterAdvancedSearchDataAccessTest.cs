namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System;
    using System.Collections;
    using System.Threading.Tasks;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.Interfaces;
    using Common.DataAccess.UnitTests;
    using Common.Model.DTO;
    using Common.Utility.Services.Interfaces;
    using Model.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class BettingTicketsCenterAdvancedSearchDataAccessTest : BaseDataAccessHelperTest<BettingTicketCenterAdvancedSearchDataAccess>
    {
        private const string UspKey = "Lrf_BetList_Licensee";

        private const string TotalRowUspKey = "Lrf_BetList_Licensee_TotalRows";

        private static readonly BalanceUp BalanceUp = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 1, 1),
            BalanceUpDay = new DateTime(2016, 2, 1)
        };

        private readonly ITempFileService _tempFileService = Substitute.For<ITempFileService>();

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(BalanceUp);
            DataAccess = Substitute.ForPartsOf<BettingTicketCenterAdvancedSearchDataAccess>(
                balanceUpDataAccess,
                _tempFileService, CurrencyDataAccess);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            _tempFileService.ClearReceivedCalls();
        }

        [Test]
        public async Task GetBetListForMultipleCustomersAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>();
            metaModel.IsExport = false;

            // Action
            await DataAccess.GetBetListForMultipleCustomersAsync(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("SportType", metaModel.SportId);
                ObjectDb.Received(1).SetParameterValue("BetType", metaModel.BetTypeId);
                ObjectDb.Received(1).SetParameterValue("LeagueId", metaModel.LeagueId);
                ObjectDb.Received(1).SetParameterValue("MatchId", metaModel.MatchId);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                ObjectDb.Received(1).SetParameterValue("StatusIdList", metaModel.Status);
                ObjectDb.Received(1).SetParameterValue("Username", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("Product", metaModel.ProductCodeList);

                ObjectDb.Received(1).SetParameterValue("TotalRows", null);
                ObjectDb.Received(1).SetParameterValue("SumStake", null);
                ObjectDb.Received(1).SetParameterValue("SumWinLost", null);
                ObjectDb.Received(1).SetParameterValue("SumLicenseeWL", null);
                ObjectDb.Received(1).SetParameterValue("SumComm", null);
                ObjectDb.Received(1).SetParameterValue("SumPlayerComm", null);
            });
        }

        [Test]
        public async Task GetTotalRowForMultipleCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(TotalRowUspKey)).Returns(ObjectDb);

            var totalRow = FakeHelper.Create<int>();
            ObjectDb.GetOutputParams().Returns(new Hashtable { { "totalrows", totalRow } });

            var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>();

            // Action
            var result = await DataAccess.GetTotalRowForMultipleCustomersAsync(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("SportType", metaModel.SportId);
                ObjectDb.Received(1).SetParameterValue("BetType", metaModel.BetTypeId);
                ObjectDb.Received(1).SetParameterValue("LeagueId", metaModel.LeagueId);
                ObjectDb.Received(1).SetParameterValue("MatchId", metaModel.MatchId);
                ObjectDb.Received(1).SetParameterValue("StatusIdList", metaModel.Status);
                ObjectDb.Received(1).SetParameterValue("Username", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                ObjectDb.Received(1).SetParameterValue("Product", metaModel.ProductCodeList);

                ObjectDb.Received(1).SetParameterValue("TotalRows", null);

                Assert.That(result, Is.EqualTo(totalRow));
            });
        }

        [Test]
        public async Task GetBetListForMultipleCustomersAsync_ExportExcel_ParametersPassedCorrectly()
        {
            // Arrange
            const int RowPerGroup = 1000;

            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>(x => x.IsExport, true);
            metaModel.SortBy = string.Empty;

            // Action
            await DataAccess.GetBetListForMultipleCustomersAsync(metaModel, RowPerGroup).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("SportType", metaModel.SportId);
                ObjectDb.Received(1).SetParameterValue("BetType", metaModel.BetTypeId);
                ObjectDb.Received(1).SetParameterValue("LeagueId", metaModel.LeagueId);
                ObjectDb.Received(1).SetParameterValue("MatchId", metaModel.MatchId);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                ObjectDb.Received(1).SetParameterValue("StatusIdList", metaModel.Status);
                ObjectDb.Received(1).SetParameterValue("Username", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                ObjectDb.Received(1).SetParameterValue("PageSize", int.MaxValue - 1);
                ObjectDb.Received(1).SetParameterValue("PageIndex", 1);
                ObjectDb.Received(1).SetParameterValue("Product", metaModel.ProductCodeList);

                ObjectDb.Received(1).SetParameterValue("TotalRows", null);
                ObjectDb.Received(1).SetParameterValue("SumStake", null);
                ObjectDb.Received(1).SetParameterValue("SumWinLost", null);
                ObjectDb.Received(1).SetParameterValue("SumLicenseeWL", null);
                ObjectDb.Received(1).SetParameterValue("SumComm", null);
                ObjectDb.Received(1).SetParameterValue("SumPlayerComm", null);

                ObjectDb.DidNotReceive().SetParameterValue("Sort", Arg.Any<object>());
            });
        }

        [Test]
        public async Task GetBetListForMultipleCustomersAsync_ExportExcel_HaveOneSheet_SaveOneFileToMemory()
        {
            // Arrange
            const int RowPerGroup = 1000;

            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(true, false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>(x => x.IsExport, true);

            var ticket = FakeHelper.Create<LrfTicket>();
            MapReaderData(ticket);

            // Action
            await DataAccess.GetBetListForMultipleCustomersAsync(metaModel, RowPerGroup).ConfigureAwait(false);

            // Assert
            _tempFileService.Received(1).Save(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task GetBetListForMultipleCustomersAsync_ExportExcel_HaveTwoSheets_SaveTwoFilesToMemory()
        {
            // Arrange
            const int RowPerGroup = 1;

            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            Reader.Read().Returns(true, true, false);
            ObjectDb.GetOutputParams().Returns(new Hashtable());

            var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>(x => x.IsExport, true);

            var ticket = FakeHelper.Create<LrfTicket>();
            MapReaderData(ticket);

            // Action
            await DataAccess.GetBetListForMultipleCustomersAsync(metaModel, RowPerGroup).ConfigureAwait(false);

            // Assert
            _tempFileService.Received(2).Save(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}