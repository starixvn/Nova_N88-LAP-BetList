namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Collections;
    using System.Threading.Tasks;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    public partial class BetListDataAccessTest
    {
        private const string LiveReportWinlossByMatchSp = "Lrf_BetList_LiveReport_SelWinlossByMatch";
        private const string LiveReportWinlossOutrightSp = "Lrf_BetList_LiveReport_SelWinlossOutright";
        private const string LiveReportWinlossNumberGameSp = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
        private const string LiveReportWinlossParlaySp = "Lrf_BetList_LiveReport_SelWinlossParlay";

        [Test]
        public async Task GetBetListData_CalledByLiveReportBetListByMatch_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossByMatchSp)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel(LiveReportWinlossByMatchSp);

            ObjectDb.GetOutputParams().Returns(x => { return null; });

            // Action
            await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("MatchId", metaModel.MatchId);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveIndicator);
                ObjectDb.Received(1).SetParameterValue("showSB", true);
                ObjectDb.DidNotReceive().SetParameterValue("showNG", true);
            });
        }

        [Test]
        public async Task GetBetListData_CalledByLiveReportBetListByMatch_OutSqlParamIsNull_ReturnCorrectTicketDataList()
        {
            // Arrange
            var expectResult = new TicketData
            {
                RefNo = "2",
                Status = "3",
                BetTypeId = 4,
                Odds = 5
            };
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossByMatchSp)).Returns(ObjectDb);
            ObjectDb.GetOutputParams().Returns(x => { return null; });
            Reader.NextResult().Returns(true);
            Reader.Read().Returns(false, true, false);
            MapReaderData(new
            {
                refno = expectResult.RefNo,
                status = expectResult.Status,
                BetType = expectResult.BetTypeId,
                odds = expectResult.Odds
            });
            var metaModel = new LiveReportBetListMetaModel
            {
                UspKey = LiveReportWinlossByMatchSp
            };

            // Action
            var result = await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.Zero);
                Assert.That(result.TicketDataList[0], Tobe.SameProperties(expectResult));
            });
        }

        [Test]
        public async Task GetBetListData_CalledByLiveReportBetListByMatch_ReturnCorrectTicketDataList()
        {
            // Arrange
            var expectResult = new TicketData
            {
                RefNo = "2",
                Status = "3",
                BetTypeId = 4,
                Odds = 5
            };
            const int TotalRows = 10;
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossByMatchSp)).Returns(ObjectDb);
            ObjectDb.GetOutputParams().Returns(x => { return new Hashtable { { "totalrows", TotalRows } }; });
            Reader.NextResult().Returns(true);
            Reader.Read().Returns(false, true, false);
            MapReaderData(new
            {
                refno = expectResult.RefNo,
                status = expectResult.Status,
                BetType = expectResult.BetTypeId,
                odds = expectResult.Odds
            });
            var metaModel = new LiveReportBetListMetaModel
            {
                UspKey = LiveReportWinlossByMatchSp
            };

            // Action
            var result = await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.EqualTo(TotalRows));
                Assert.That(result.TicketDataList[0], Tobe.SameProperties(expectResult));
            });
        }

        [Test]
        public async Task GetBetListData_CalledByLiveReportOutRightBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossOutrightSp)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel(LiveReportWinlossOutrightSp);

            // Action
            await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("showSB", true);
                ObjectDb.DidNotReceive().SetParameterValue("showNG", true);
            });
        }

        [Test]
        public async Task GetBetListData_CalledByLiveReportNumberGameBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossNumberGameSp)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel(LiveReportWinlossNumberGameSp);

            // Action
            await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("Sort", metaModel.SortBy + " " + metaModel.SortOrder);
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveIndicator);
            });
        }

        [Test]
        public async Task GetBetListData_CalledByLiveReportParlayBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossParlaySp)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel(LiveReportWinlossParlaySp);

            // Action
            await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("Sort", metaModel.SortBy + " " + metaModel.SortOrder);
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("showSB", true);
                ObjectDb.DidNotReceive().SetParameterValue("showNG", true);
                ObjectDb.Received(1).SetParameterValue("BetType", metaModel.BetType);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveIndicator);
            });
        }

        [Test]
        public async Task GetBetListData_CalledByLiveReportVHRacingBetlist_ParametersPassedCorrectly()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_LiveReport_SelWinlossVHRacing";
            DataAccess.GetObjectDb(Arg.Is<string>(key => key.Equals(UspKey))).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel(UspKey);

            // Action
            await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("Sort", metaModel.SortBy + " " + metaModel.SortOrder);
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("SportType", metaModel.SportType);
                ObjectDb.Received(1).SetParameterValue("SUMWL", null);
                ObjectDb.Received(1).SetParameterValue("SUMLicenseeWL", null);

            });
        }

        [TestCase(true, "Team1", true, 2)]
        [TestCase(false, "", false, 0)]
        public async Task GetBetListData_ReadLrfReferneceTicket_ReturnCorrectData(
            bool hasOrdinalBetTeam,
            string expectedBetTeam,
            bool hasOrdinalStake,
            decimal expectedStake)
        {
            // Arrange
            var expectResult = new TicketData
            {
                RefNo = "2",
                Status = "3",
                BetTypeId = 4,
                Odds = 5,
                BetTeam = expectedBetTeam,
                Stake = expectedStake
            };
            DataAccess.GetObjectDb(Arg.Is(LiveReportWinlossByMatchSp)).Returns(ObjectDb);
            MapReaderData(new
            {
                refno = expectResult.RefNo,
                status = expectResult.Status,
                BetType = expectResult.BetTypeId,
                odds = expectResult.Odds,
                betteam = expectedBetTeam,
                stake = expectedStake
            });
            Reader.NextResult().Returns(true);
            Reader.Read().Returns(false, true, false);
            Reader.FieldCount.Returns(2);
            Reader.GetName(0).Returns(hasOrdinalBetTeam ? "betteam" : string.Empty);
            Reader.GetName(1).Returns(hasOrdinalStake ? "stake" : string.Empty);
            var metaModel = new LiveReportBetListMetaModel
            {
                UspKey = LiveReportWinlossByMatchSp
            };

            // Action
            var result = await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TicketDataList[0], Tobe.SameProperties(expectResult));
            });
        }

        [TestCase("WinLoss", "WinLossComm")]
        [TestCase("LicenseeWL", "LicenseeWLComm")]
        [TestCase("nickname", "ExternalId")]
        [TestCase("Status", "Status")]
        public async Task GetBetListData_IsServerPagingFalseSortData_SortCorrectProperty(string sortBy, string expectedSortedProp)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_SelWinloss_Mem";
            DataAccess.GetObjectDb(Arg.Is(UspKey)).Returns(ObjectDb);
            var metaModel = GetLiveReportBetListMetaModel(UspKey);
            metaModel.IsServerPaging = false;
            metaModel.SortBy = sortBy;

            // Action
            await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.That(metaModel.SortBy, Is.EqualTo(expectedSortedProp));
        }

        [TestCase("Lrf_BetList_RNG_Detail")]
        [TestCase("Lrf_BetList_LicenseeBonus_Detail")]
        public async Task GetBetListData_CallSpDoesNotHaveOutPutParam_SumDataToGetSummaryResult(string uspName)
        {
            // Arrange
            var expectResult = new LrfTicket
            {
                Winlost = 1,
                BetEffective = 2,
                LicenseeWL = 3,
                Comm = 4,
                PlayerComm = 5
            };
            DataAccess.GetObjectDb(Arg.Is(uspName)).Returns(ObjectDb);
            MapReaderData(new
            {
                winlost = expectResult.Winlost,
                beteffective = expectResult.BetEffective,
                licenseewl = expectResult.LicenseeWL,
                comm = expectResult.Comm,
                playercomm = expectResult.PlayerComm
            });
            Reader.NextResult().Returns(false);
            Reader.Read().Returns(true, false, false);
            SetUpColumnsForReader(5, new[] { "winlost", "beteffective", "licenseewl", "comm", "playercomm" });

            ObjectDb.GetOutputParams().Returns(x => { return null; });

            var metaModel = new BetListMetaModel { BaseCurrency = string.Empty, UspKey = uspName };

            // Action
            var result = await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.Zero);
                Assert.That(result.SumWinLoss, Is.EqualTo(expectResult.Winlost));
                Assert.That(result.SumBetEffective, Is.EqualTo(expectResult.BetEffective));
                Assert.That(result.SumLicenseeWL, Is.EqualTo(expectResult.LicenseeWL));
                Assert.That(result.SumComm, Is.EqualTo(expectResult.Comm));
                Assert.That(result.SumPlayerComm, Is.EqualTo(expectResult.PlayerComm));
            });
        }

        [TestCase("Lrf_BetList_SelWinLossByDate_Mem")]
        [TestCase("Lrf_BetList_SelWinloss_Mem")]
        public async Task GetBetListData_CallSpHaveOutPutParam_GetSummaryFromOutputParams(string uspName)
        {
            // Arrange
            const string BaseCurrency = "USD";
            var expectSummaryResult = new Hashtable
            {
                { "totalrows", 6 },
                { "sumwinlost", 7 },
                { "sumbeteffective", 8 },
                { "sumlicenseewl", 9 },
                { "sumcomm", 10 },
                { "sumplayercomm", 11 }
            };
            DataAccess.GetObjectDb(Arg.Is(uspName)).Returns(ObjectDb);
            MapReaderData(new
            {
                winlost = 1,
                beteffective = 2,
                licenseewl = 3,
                comm = 4,
                playercomm = 5
            });
            Reader.NextResult().Returns(false);
            Reader.Read().Returns(true, false, false);
            SetUpColumnsForReader(5, new[] { "winlost", "beteffective", "licenseewl", "comm", "playercomm" });

            ObjectDb.GetOutputParams().Returns(expectSummaryResult);

            var metaModel = new BetListMetaModel { BaseCurrency = BaseCurrency, UspKey = uspName };

            // Action
            var result = await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            AssertTicketResult(result, expectSummaryResult);
        }

        [TestCase("Lrf_BetList_CustomerList_Mem")]
        public async Task GetBetListData_CallSpHaveOnlyOriginalOutPutParam_GetSummaryFromOriginalOutputParams(string uspName)
        {
            // Arrange
            const string BaseCurrency = "usd";
            var expectSummaryResult = new Hashtable
            {
                { "totalrows", 6 },
                { "sumwinlost", 12 },
                { "sumbeteffective", 13 },
                { "sumlicenseewl", 14 },
                { "sumcomm", 15 },
                { "sumplayercomm", 16 },
            };
            DataAccess.GetObjectDb(Arg.Is(uspName)).Returns(ObjectDb);
            MapReaderData(new
            {
                winlost = 1,
                beteffective = 2,
                licenseewl = 3,
                comm = 4,
                playercomm = 5
            });
            Reader.NextResult().Returns(false);
            Reader.Read().Returns(true, false, false);
            SetUpColumnsForReader(5, new[] { "winlost", "beteffective", "licenseewl", "comm", "playercomm" });

            ObjectDb.GetOutputParams().Returns(expectSummaryResult);

            var metaModel = new BetListMetaModel { BaseCurrency = BaseCurrency, UspKey = uspName };

            // Action
            var result = await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            AssertTicketResult(result, expectSummaryResult);
        }

        private static void AssertTicketResult(TicketResult result, Hashtable expectSummaryResult)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.EqualTo(expectSummaryResult["totalrows"]));
                Assert.That(result.SumWinLoss, Is.EqualTo(expectSummaryResult["sumwinlost"]));
                Assert.That(result.SumBetEffective, Is.EqualTo(expectSummaryResult["sumbeteffective"]));
                Assert.That(result.SumLicenseeWL, Is.EqualTo(expectSummaryResult["sumlicenseewl"]));
                Assert.That(result.SumComm, Is.EqualTo(expectSummaryResult["sumcomm"]));
                Assert.That(result.SumPlayerComm, Is.EqualTo(expectSummaryResult["sumplayercomm"]));
            });
        }

        private static LiveReportBetListMetaModel GetLiveReportBetListMetaModel(string uspKey)
        {
            var metaModel = FakeHelper.Create<LiveReportBetListMetaModel>();
            metaModel.ProductCodeList = "SB,NG";
            metaModel.IsExportExcel = false;
            metaModel.UspKey = uspKey;
            return metaModel;
        }

        private void SetUpColumnsForReader(int numberOfCols, string[] cols)
        {
            Reader.FieldCount.Returns(numberOfCols);

            for (var i = 0; i < numberOfCols; i++)
            {
                Reader.GetName(i).Returns(cols[i]);
            }
        }
    }
}